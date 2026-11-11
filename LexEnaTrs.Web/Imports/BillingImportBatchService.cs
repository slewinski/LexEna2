using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using LexEnaTrs.Web.Models;
using WienaDB.Models;

namespace LexEnaTrs.Web
{
    public partial class LexEnaMeritumDomainService
    {
        [Invoke, RequiresAuthentication]
        public string CreateBillingImportBatch(Guid batchId, string name, string fileName, string document,
            int userId, int firma, int branchId, int symbolId, string symbolName, int firstNumber,
            int year, string documentName, DateTime bookingDate)
        {
            try
            {
                string login = ServiceContext.User.Identity.Name;
                Uzytkownik currentUser = ObjectContext.Uzytkownik.FirstOrDefault(u => u.UserName == login);
                if (currentUser == null) throw new InvalidOperationException("Nie znaleziono zalogowanego użytkownika.");
                userId = currentUser.Id;
                if (batchId == Guid.Empty) throw new InvalidOperationException("Brak identyfikatora paczki.");
                name = RequiredBatchText(name, 100, "oznaczenie paczki");
                fileName = RequiredBatchText(Path.GetFileName(fileName), 260, "nazwa pliku");
                documentName = RequiredBatchText(documentName, 50, "dokument księgowy");
                if (firstNumber <= 0 || year < 1900 || year > 9999 || bookingDate < new DateTime(1753, 1, 1))
                    throw new InvalidOperationException("Nieprawidłowa sygnatura lub data księgowania.");

                // Repeating a request after a lost response must not create a second batch.
                BillingImportBatch existing = ObjectContext.BillingImportBatch.FirstOrDefault(b => b.Id == batchId);
                if (existing != null)
                {
                    if (existing.Firma != firma) throw new InvalidOperationException("Paczka nie należy do wybranej firmy.");
                    return existing.ImportXml;
                }
                using (wiena_centralEntities wiena = new wiena_centralEntities())
                {
                    if (!wiena.firma.Any(f => f.ident == branchId && f.typ_firmy == firma && f.typ == 1 && f.aktywna == 1))
                        throw new InvalidOperationException("Wybrany oddział nie należy do firmy lub jest nieaktywny.");
                    symbol selectedSymbol = wiena.symbol.FirstOrDefault(s => s.ident == symbolId && s.id_firmy == branchId);
                    if (selectedSymbol == null) throw new InvalidOperationException("Wybrany symbol nie należy do oddziału.");
                    symbolName = RequiredBatchText(selectedSymbol.oznaczenie, 50, "symbol poczekalni");
                    if (!wiena.dowod_slownik.Any(d => d.dowod.Trim() == documentName))
                        throw new InvalidOperationException("Nie znaleziono dokumentu księgowego w Wienie.");
                }

                string parsed;
                using (PozewDomainService importer = new PozewDomainService())
                    parsed = importer.ImportBillingDocument(document, fileName, userId, firma, symbolId,
                        symbolName, firstNumber, year, documentName);
                if (parsed.StartsWith("Błąd", StringComparison.OrdinalIgnoreCase)) return parsed;
                ImportSprawBilling import = ReadBatchXml(parsed);
                import.IdOddzial = branchId;
                import.IdKancelaria = 0;
                import.DataKsiegowania = bookingDate.Date;
                import.NrDodowu = documentName;
                for (int i = 0; i < import.SprawaDescriptor.Length; i++)
                {
                    SprImportDescriptor row = import.SprawaDescriptor[i];
                    ValidateBatchRow(row);
                    row.BatchRowNumber = i + 1;
                    row.CanImport = row.czyrej;
                    row.WienaCaseId = 0;
                    row.BatchAmount = row.Odbiorca.Obciazenia.Sum(o => o.Kwota_do_zaplaty);
                }

                BillingImportBatch batch = new BillingImportBatch
                {
                    Id = batchId, Firma = firma, Name = name, SourceFileName = fileName,
                    CreatedAt = DateTime.Now, CreatedBy = userId, BranchId = branchId,
                    SymbolId = symbolId, SymbolName = symbolName, FirstNumber = firstNumber, Year = year,
                    DocumentName = documentName, BookingDate = bookingDate.Date, Status = "Draft",
                    CaseCount = CountBatchCases(import),
                    ImportedCount = 0, TotalAmount = CalculateBatchAmount(import),
                    ImportXml = WriteBatchXml(import)
                };
                ObjectContext.BillingImportBatch.AddObject(batch);
                ObjectContext.SaveChanges();
                return batch.ImportXml;
            }
            catch (Exception ex) { return BatchError(ex); }
        }

        [Invoke, RequiresAuthentication]
        public string GetBillingImportBatchXml(Guid batchId, int firma)
        {
            try { return FindBillingBatch(batchId, firma).ImportXml; }
            catch (Exception ex) { return BatchError(ex); }
        }

        [Invoke, RequiresAuthentication]
        public string DeleteBillingImportBatch(Guid batchId, int firma)
        {
            try
            {
                BillingImportBatch batch = FindBillingBatch(batchId, firma);
                EnsureBillingBatchCanBeDeleted(batch);
                // The registration claim changes Draft to Processing before any Wiena write.
                // Recheck state and the validated snapshot atomically with deletion so neither
                // a concurrent registration nor a changed draft can lose its batch record.
                int changed = ObjectContext.ExecuteStoreCommand(
                    "DELETE FROM dbo.BillingImportBatch WHERE Id = @id AND Firma = @firma AND Status = N'Draft' AND ImportedCount = 0 AND ImportXml = @xml",
                    new SqlParameter("@id", batchId), new SqlParameter("@firma", firma), BatchXmlParameter(batch.ImportXml));
                if (changed != 1)
                    throw new InvalidOperationException("Paczka została zmieniona, usunięta lub rozpoczęto jej zapis do Wieny. Odśwież listę paczek.");
                return "OK";
            }
            catch (Exception ex) { return BatchError(ex); }
        }

        private static void EnsureBillingBatchCanBeDeleted(BillingImportBatch batch)
        {
            if (batch == null || batch.Status != "Draft" || batch.ImportedCount != 0)
                throw new InvalidOperationException("Można usunąć tylko przygotowaną paczkę, której zapis do Wieny nie został rozpoczęty.");
            if (String.IsNullOrWhiteSpace(batch.ImportXml))
                throw new InvalidOperationException("Brak danych paczki. Nie można potwierdzić, że nie została zapisana w Wienie.");
            ImportSprawBilling import = ReadBatchXml(batch.ImportXml);
            if (import.SprawaDescriptor.Any(row => row == null || row.czy_ok || row.WienaCaseId != 0))
                throw new InvalidOperationException("Paczka zawiera zapisane sprawy lub nieprawidłowe dane. Usunięcie jest zablokowane; sprawdź jej stan w Wienie.");
        }

        [Invoke, RequiresAuthentication]
        public string SaveBillingImportBatchDraft(Guid batchId, int firma, string document)
        {
            try
            {
                BillingImportBatch batch = FindBillingBatch(batchId, firma);
                string xml = WriteBatchXml(ApplyBatchChoices(batch, document));
                int changed = ObjectContext.ExecuteStoreCommand(
                    "UPDATE dbo.BillingImportBatch SET ImportXml = @xml WHERE Id = @id AND Firma = @firma AND Status = N'Draft'",
                    BatchXmlParameter(xml), new SqlParameter("@id", batchId), new SqlParameter("@firma", firma));
                if (changed != 1) throw new InvalidOperationException("Paczka jest już zapisywana lub została zamknięta. Odśwież listę paczek.");
                return xml;
            }
            catch (Exception ex) { return BatchError(ex); }
        }

        [Invoke, RequiresAuthentication]
        public string RegisterBillingImportBatch(Guid batchId, int firma, string document)
        {
            bool claimed = false;
            ImportSprawBilling import = null;
            try
            {
                BillingImportBatch batch = FindBillingBatch(batchId, firma);
                import = ApplyBatchChoices(batch, document);
                if (!import.SprawaDescriptor.Any(r => r.czyrej))
                    throw new InvalidOperationException("Zaznacz co najmniej jedną sprawę do zapisania.");
                foreach (var group in import.SprawaDescriptor.Where(r => r.czyrej).GroupBy(r => r.nr))
                    if (group.Select(r => r.CzyUstawowe).Distinct().Count() != 1)
                        throw new InvalidOperationException("Wiersze wspólnej sprawy " + group.First().sygn_obciaz + " muszą mieć ten sam rodzaj odsetek.");

                // The persisted claim precedes all Wiena writes. It also blocks a second HTTP request.
                int changed = ObjectContext.ExecuteStoreCommand(
                    "UPDATE dbo.BillingImportBatch SET Status = N'Processing', ImportXml = @xml WHERE Id = @id AND Firma = @firma AND Status = N'Draft'",
                    BatchXmlParameter(WriteBatchXml(import)), new SqlParameter("@id", batchId), new SqlParameter("@firma", firma));
                if (changed != 1) throw new InvalidOperationException("Ta paczka została już skierowana do zapisu. Odśwież listę; ponowne wykonanie nie jest dozwolone.");
                claimed = true;

                bool failed = false;
                foreach (var group in import.SprawaDescriptor.Where(r => r.czyrej).GroupBy(r => r.nr).ToList())
                {
                    List<SprImportDescriptor> rows = group.ToList();
                    SprImportDescriptor merged = MergeBatchRows(rows);

                    ImportBillingHelper helper = new ImportBillingHelper();
                    bool success = helper.DoRegisterSprAtomic(merged, import.Oddzial, import.DataKsiegowania,
                        import.NrDodowu, import.IdOddzial, firma, import.System, 0, rows.ToArray());
                    failed |= !success;
                    foreach (SprImportDescriptor row in rows)
                    {
                        row.czy_ok = success;
                        row.czyrej = false;
                        row.WienaCaseId = success ? merged.WienaCaseId : 0;
                        row.message = success ? "Zapisano w poczekalni Wieny." :
                            (String.IsNullOrWhiteSpace(helper.ErrMsg) ? "Nie zapisano sprawy. Wymagana weryfikacja." : helper.ErrMsg);
                    }
                    // Checkpoint after every committed case. A crash in the commit/checkpoint gap
                    // leaves Processing and requires manual verification, never automatic retry.
                    StoreBatchProgress(batchId, firma, import, "Processing");
                }
                foreach (SprImportDescriptor row in import.SprawaDescriptor.Where(r => !r.czy_ok && String.IsNullOrWhiteSpace(r.message)))
                    row.message = "Pominięto przy zapisie paczki.";
                StoreBatchProgress(batchId, firma, import, failed ? "Partial" : "Completed");
                return WriteBatchXml(import);
            }
            catch (Exception ex)
            {
                if (claimed)
                {
                    try
                    {
                        // Preserve the last durable checkpoint; do not overwrite an uncertain result.
                        ObjectContext.ExecuteStoreCommand(
                            "UPDATE dbo.BillingImportBatch SET Status = N'ReviewRequired' WHERE Id = @id AND Firma = @firma AND Status = N'Processing'",
                            new SqlParameter("@id", batchId), new SqlParameter("@firma", firma));
                    }
                    catch { /* Processing is also a non-retryable state. */ }
                    return "Błąd: Zapis paczki został przerwany. Sprawdź wynik w Wienie i odśwież paczkę. Nie importuj jej ponownie. " + ex.Message;
                }
                return BatchError(ex);
            }
        }

        private BillingImportBatch FindBillingBatch(Guid id, int firma)
        {
            BillingImportBatch batch = ObjectContext.BillingImportBatch.FirstOrDefault(b => b.Id == id && b.Firma == firma);
            if (batch == null) throw new InvalidOperationException("Nie znaleziono paczki dla wybranej firmy.");
            return batch;
        }

        private static ImportSprawBilling ApplyBatchChoices(BillingImportBatch batch, string document)
        {
            if (batch.Status != "Draft") throw new InvalidOperationException("Paczka jest już zapisywana lub zamknięta. Można tylko przeglądać jej wynik.");
            ImportSprawBilling persisted = ReadBatchXml(batch.ImportXml);
            ImportSprawBilling edited = ReadBatchXml(Encoding.UTF8.GetString(Convert.FromBase64String(document)));
            if (edited.SprawaDescriptor.Length != persisted.SprawaDescriptor.Length ||
                edited.SprawaDescriptor.Select(r => r.BatchRowNumber).Distinct().Count() != persisted.SprawaDescriptor.Length)
                throw new InvalidOperationException("Lista pozycji nie odpowiada zapisanej paczce.");
            Dictionary<int, SprImportDescriptor> choices = edited.SprawaDescriptor.ToDictionary(r => r.BatchRowNumber);
            foreach (SprImportDescriptor row in persisted.SprawaDescriptor)
            {
                SprImportDescriptor choice;
                if (!choices.TryGetValue(row.BatchRowNumber, out choice)) throw new InvalidOperationException("Brak pozycji paczki.");
                if (choice.czyrej && !row.CanImport) throw new InvalidOperationException("Pozycja " + row.BatchRowNumber + " została odrzucona przez kontrolę duplikatów.");
                if (choice.CzyUstawowe != row.CzyUstawowe && choice.CzyUstawowe != 0 && choice.CzyUstawowe != 8 && choice.CzyUstawowe != 15 && choice.CzyUstawowe != 18)
                    throw new InvalidOperationException("Nieprawidłowy rodzaj odsetek dla pozycji " + row.BatchRowNumber + ".");
                row.czyrej = choice.czyrej;
                row.CzyUstawowe = choice.CzyUstawowe;
            }
            return persisted;
        }

        private void StoreBatchProgress(Guid id, int firma, ImportSprawBilling import, string status)
        {
            int imported = CountImportedBatchCases(import);
            int changed = ObjectContext.ExecuteStoreCommand(
                "UPDATE dbo.BillingImportBatch SET ImportXml = @xml, Status = @status, ImportedCount = @count WHERE Id = @id AND Firma = @firma AND Status = N'Processing'",
                BatchXmlParameter(WriteBatchXml(import)), new SqlParameter("@status", status),
                new SqlParameter("@count", imported), new SqlParameter("@id", id), new SqlParameter("@firma", firma));
            if (changed != 1) throw new InvalidOperationException("Nie udało się utrwalić wyniku paczki.");
        }

        private static SqlParameter BatchXmlParameter(string xml)
        {
            return new SqlParameter("@xml", SqlDbType.NVarChar, -1) { Value = xml };
        }

        private static SprImportDescriptor MergeBatchRows(IEnumerable<SprImportDescriptor> rows)
        {
            // Merge a copy, leaving persisted input and payment data intact for inspection.
            ImportSprawBilling copy = ReadBatchXml(WriteBatchXml(new ImportSprawBilling { SprawaDescriptor = rows.ToArray() }));
            SprImportDescriptor merged = copy.SprawaDescriptor[0];
            merged.Odbiorca.Nr_ewidencyjnyArray = copy.SprawaDescriptor.Skip(1).Select(r => r.Odbiorca.Nr_ewidencyjny.Trim()).ToArray();
            for (int i = 0; i < copy.SprawaDescriptor.Length; i++)
            {
                foreach (Obciazenie charge in copy.SprawaDescriptor[i].Odbiorca.Obciazenia)
                {
                    // The legacy helper uses this only as an in-memory source-row key.
                    charge.Id_sprawy = i + 1;
                    if (charge.Wplaty != null)
                    {
                        charge.Wplaty = charge.Wplaty.Where(w => w.Kwota_wplaty != 0).ToArray();
                        if (charge.Wplaty.Length == 0) charge.Wplaty = null;
                    }
                }
            }
            merged.Odbiorca.Obciazenia = copy.SprawaDescriptor.SelectMany(r => r.Odbiorca.Obciazenia).ToArray();
            return merged;
        }

        private static int CountBatchCases(ImportSprawBilling import)
        {
            return import.SprawaDescriptor.Select(r => new { r.id_symbol, r.rok, r.nr }).Distinct().Count();
        }

        private static int CountImportedBatchCases(ImportSprawBilling import)
        {
            return import.SprawaDescriptor.Where(r => r.czy_ok && r.WienaCaseId > 0).Select(r => r.WienaCaseId).Distinct().Count();
        }

        private static decimal CalculateBatchAmount(ImportSprawBilling import)
        {
            return import.SprawaDescriptor.Sum(r => r.Odbiorca.Obciazenia.Sum(o => o.Kwota_do_zaplaty));
        }

        private static ImportSprawBilling ReadBatchXml(string xml)
        {
            ImportSprawBilling import = (ImportSprawBilling)Utils.DeserializeFromString(xml, typeof(ImportSprawBilling));
            if (import == null || import.SprawaDescriptor == null || import.SprawaDescriptor.Length == 0)
                throw new InvalidOperationException("Plik nie zawiera spraw do importu.");
            return import;
        }

        private static string WriteBatchXml(ImportSprawBilling import)
        {
            return Utils.SerializeToString(import, typeof(ImportSprawBilling));
        }

        private static void ValidateBatchRow(SprImportDescriptor row)
        {
            if (row == null || row.Odbiorca == null || row.Odbiorca.Obciazenia == null || row.Odbiorca.Obciazenia.Length == 0 ||
                String.IsNullOrWhiteSpace(row.Odbiorca.Nr_ewidencyjny) || row.Odbiorca.Adres == null || row.Odbiorca.Adres_korespondencyjny == null)
                throw new InvalidOperationException("Sprawa nie zawiera wymaganych danych odbiorcy, adresów lub należności.");
        }

        private static string RequiredBatchText(string value, int length, string label)
        {
            value = (value ?? String.Empty).Trim();
            if (value.Length == 0 || value.Length > length)
                throw new InvalidOperationException("Uzupełnij pole: " + label + " (maksymalnie " + length + " znaków).");
            return value;
        }

        private static string BatchError(Exception exception)
        {
            return "Błąd: " + exception.Message;
        }
    }
}
