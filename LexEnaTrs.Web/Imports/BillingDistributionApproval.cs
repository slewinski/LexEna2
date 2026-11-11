using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using LexEnaTrs.Web.Models;
using WienaDB.Models;

namespace LexEnaTrs.Web
{
    public partial class LexEnaMeritumDomainService
    {
        // The client submits only the identity of the proposal it actually reviewed.
        // All assignments and amounts come from the server's saved snapshot.
        [Invoke, RequiresAuthentication]
        public string ApproveBillingDistribution(Guid batchId, int firma, Guid proposalId)
        {
            try
            {
                BillingImportBatch batch = FindDistributionBatch(batchId, firma);
                RequireDistributionProposal(batch, proposalId);
                if (batch.DistributionState == "Approved") return GetLockedBillingDistribution(batch);
                if (batch.DistributionState == "Planning")
                {
                    int claimed = ObjectContext.ExecuteStoreCommand(
                        "UPDATE dbo.BillingImportBatch SET DistributionState = N'Processing' " +
                        "WHERE Id = @id AND Firma = @firma AND DistributionState = N'Planning' AND DistributionXml = @xml",
                        new SqlParameter("@id", batchId), new SqlParameter("@firma", firma), XmlParameter("@xml", batch.DistributionXml));
                    if (claimed != 1)
                        throw new InvalidOperationException("Propozycja została zmieniona lub rozpoczęto jej dekretację. Odśwież widok.");
                }
                else if (batch.DistributionState != "Processing")
                    throw new InvalidOperationException("Nieprawidłowy stan dekretacji paczki.");

                EntityConnection lexConnection = (EntityConnection)ObjectContext.Connection;
                if (lexConnection.State != ConnectionState.Open) lexConnection.Open();
                // Separate local transactions: no TransactionScope / MSDTC requirement.
                // This row lock serializes retries and prevents recalculation until the
                // durable Wiena receipt has been copied back to LexEna.
                using (EntityTransaction lexTransaction = lexConnection.BeginTransaction(IsolationLevel.Serializable))
                {
                    ObjectContext.ExecuteStoreQuery<Guid>(
                        "SELECT Id FROM dbo.BillingImportBatch WITH (UPDLOCK, HOLDLOCK) WHERE Id = @id AND Firma = @firma",
                        new SqlParameter("@id", batchId), new SqlParameter("@firma", firma)).ToList();
                    ObjectContext.Refresh(RefreshMode.StoreWins, batch);
                    BillingDistribution saved = RequireDistributionProposal(batch, proposalId);
                    if (batch.DistributionState == "Approved")
                    {
                        string completed = GetLockedBillingDistribution(batch);
                        lexTransaction.Commit();
                        return completed;
                    }
                    if (batch.DistributionState != "Processing")
                        throw new InvalidOperationException("Poprzednia próba dekretacji została wycofana. Odśwież i ponownie sprawdź propozycję przed zatwierdzeniem.");
                    string approvedXml;
                    bool definitelyRolledBack = false;
                    try
                    {
                        approvedXml = ExecuteBillingDistributionApproval(batch, saved,
                            ServiceContext.User.Identity.Name, out definitelyRolledBack);
                    }
                    catch
                    {
                        if (definitelyRolledBack)
                        {
                            ObjectContext.ExecuteStoreCommand(
                                "UPDATE dbo.BillingImportBatch SET DistributionState = N'Planning' WHERE Id = @id AND Firma = @firma",
                                new SqlParameter("@id", batchId), new SqlParameter("@firma", firma));
                            lexTransaction.Commit();
                        }
                        // Unknown commit outcome: leave the durable Processing claim.
                        // The next attempt consults Wiena's receipt before any new write.
                        throw;
                    }
                    int completedCount = ObjectContext.ExecuteStoreCommand(
                        "UPDATE dbo.BillingImportBatch SET DistributionState = N'Approved', DistributionXml = @result " +
                        "WHERE Id = @id AND Firma = @firma AND DistributionState = N'Processing' AND DistributionXml = @original",
                        XmlParameter("@result", approvedXml), new SqlParameter("@id", batchId),
                        new SqlParameter("@firma", firma), XmlParameter("@original", batch.DistributionXml));
                    if (completedCount != 1)
                        throw new InvalidOperationException("Wiena zapisała dekretację, ale nie potwierdzono jej w LexEna. Odśwież widok i dokończ dekretację.");
                    lexTransaction.Commit();
                    return approvedXml;
                }
            }
            catch (Exception ex) { return BatchError(ex); }
        }

        private static SqlParameter XmlParameter(string name, string xml)
        {
            return new SqlParameter(name, SqlDbType.NVarChar, -1) { Value = xml ?? (object)DBNull.Value };
        }

        private static BillingDistribution RequireDistributionProposal(BillingImportBatch batch, Guid proposalId)
        {
            if (proposalId == Guid.Empty || String.IsNullOrWhiteSpace(batch.DistributionXml))
                throw new InvalidOperationException("Najpierw wygeneruj i sprawdź propozycję podziału.");
            var saved = (BillingDistribution)Utils.DeserializeFromString(batch.DistributionXml, typeof(BillingDistribution));
            if (saved == null || saved.BatchId != batch.Id || saved.ProposalId != proposalId ||
                !saved.HasSavedProposal || saved.Cases == null || saved.Offices == null)
                throw new InvalidOperationException("Zapisana propozycja jest inna niż wyświetlana. Odśwież widok i sprawdź podział przed zatwierdzeniem.");
            return saved;
        }

        private static string GetLockedBillingDistribution(BillingImportBatch batch)
        {
            if (String.IsNullOrWhiteSpace(batch.DistributionXml))
                throw new InvalidOperationException("Brak zapisanego wyniku podziału paczki.");
            var saved = (BillingDistribution)Utils.DeserializeFromString(batch.DistributionXml, typeof(BillingDistribution));
            if (saved == null || saved.BatchId != batch.Id || saved.ProposalId == Guid.Empty || saved.Cases == null || saved.Offices == null)
                throw new InvalidOperationException("Nieprawidłowy zapis dekretacji paczki.");
            saved.ApprovalState = batch.DistributionState;
            SetDistributionSignatureColumns(saved, ReadBatchXml(batch.ImportXml));
            saved.CanApprove = batch.DistributionState == "Processing";
            saved.ApprovalProblem = saved.CanApprove
                ? "Dekretacja została rozpoczęta. Sprawdź / dokończ tę samą operację, aby odczytać wynik z Wieny bez ponownego nadawania numerów."
                : String.Empty;
            return Utils.SerializeToString(saved, typeof(BillingDistribution));
        }

        private static void SetDistributionApprovalAvailability(BillingDistribution distribution, BillingImportBatch batch)
        {
            distribution.CanApprove = false;
            distribution.ApprovalProblem = String.Empty;
            try
            {
                if (distribution.ApprovalState != "Planning") return;
                BillingDistribution saved = batch == null ? distribution : RequireDistributionProposal(batch, distribution.ProposalId);
                ValidateDistributionApprovalProposal(saved, distribution);
                distribution.CanApprove = true;
            }
            catch (Exception ex) { distribution.ApprovalProblem = ex.Message; }
        }

        private static void ValidateDistributionSymbols(wiena_centralEntities wiena, BillingImportBatch batch,
            IEnumerable<BillingDistributionOffice> offices)
        {
            var symbols = wiena.symbol.Where(s => s.id_firmy == batch.BranchId).ToList();
            foreach (var office in offices)
            {
                try
                {
                    symbol resolved = ResolveDistributionSymbol(symbols, batch.BranchId, office.SymbolSprawWiena);
                    BuildDistributionSignature(resolved.oznaczenie, 1, batch.Year);
                    office.SymbolIssue = String.Empty;
                }
                catch (InvalidOperationException ex) { office.SymbolIssue = ex.Message; }
            }
        }

        private static symbol ResolveDistributionSymbol(IEnumerable<symbol> symbols, int branchId, string designation)
        {
            string normalized = (designation ?? String.Empty).Trim();
            if (normalized.Length == 0 || normalized.Length > 10)
                throw new InvalidOperationException("Uzupełnij symbol spraw kancelarii w konfiguracji (maksymalnie 10 znaków).");
            var matches = symbols.Where(s => s.id_firmy == branchId &&
                String.Equals((s.oznaczenie ?? String.Empty).Trim(), normalized, StringComparison.OrdinalIgnoreCase)).ToList();
            if (matches.Count != 1)
                throw new InvalidOperationException("Symbol „" + normalized + "” musi występować dokładnie raz w oddziale paczki w Wienie.");
            return matches[0];
        }

        private static string BuildDistributionSignature(string symbol, int number, int year)
        {
            string designation = (symbol ?? String.Empty).Trim();
            if (designation.Length == 0 || designation.Length > 10 || number <= 0 || year < 1900 || year > 9999)
                throw new InvalidOperationException("Nieprawidłowy symbol, numer lub rok nowej sygnatury.");
            string signature = designation + "-" + number.ToString(CultureInfo.InvariantCulture) + "/" + year.ToString(CultureInfo.InvariantCulture);
            if (signature.Length > 16)
                throw new InvalidOperationException("Sygnatura „" + signature + "” przekracza limit 16 znaków w Wienie. Wybierz krótszy symbol.");
            return signature;
        }

        private static void ValidateDistributionApprovalProposal(BillingDistribution saved, BillingDistribution current)
        {
            ValidateDistributionProposalSnapshot(saved, current, true);
        }

        private static void ValidateDistributionProposalSnapshot(BillingDistribution saved, BillingDistribution current, bool requireSymbols)
        {
            if (saved == null || current == null || saved.BatchId == Guid.Empty || saved.BatchId != current.BatchId ||
                saved.ProposalId == Guid.Empty || !saved.HasSavedProposal || saved.Cases == null || saved.Offices == null ||
                current.Cases == null || current.Offices == null || saved.BranchId <= 0 || saved.BranchId != current.BranchId ||
                saved.NumberingYear < 1900 || saved.NumberingYear > 9999 || saved.NumberingYear != current.NumberingYear)
                throw new InvalidOperationException("Najpierw wygeneruj aktualną propozycję podziału i sprawdź jej wynik.");
            var planned = saved.Cases.Where(c => c.Eligible).ToArray();
            var actual = current.Cases.Where(c => c.Eligible).ToArray();
            if (planned.Length == 0)
                throw new InvalidOperationException("Brak spraw do dekretacji.");
            if (planned.Any(c => c.WienaCaseId <= 0 || c.Decreed) || planned.Select(c => c.WienaCaseId).Distinct().Count() != planned.Length ||
                actual.Select(c => c.WienaCaseId).Distinct().Count() != actual.Length ||
                !planned.Select(c => c.WienaCaseId).OrderBy(id => id).SequenceEqual(actual.Select(c => c.WienaCaseId).OrderBy(id => id)))
                throw new InvalidOperationException("Zmieniła się lista spraw kwalifikujących się do podziału. Odśwież statusy i ponownie zaproponuj podział.");
            if (saved.Offices.GroupBy(o => o.UnitId).Any(g => g.Count() != 1) || current.Offices.GroupBy(o => o.UnitId).Any(g => g.Count() != 1))
                throw new InvalidOperationException("Konfiguracja podziału zawiera powtórzone kancelarie.");
            var actualCases = actual.ToDictionary(c => c.WienaCaseId);
            var oldOffices = saved.Offices.ToDictionary(o => o.UnitId);
            var newOffices = current.Offices.ToDictionary(o => o.UnitId);
            foreach (var item in planned)
            {
                BillingDistributionCase now = actualCases[item.WienaCaseId];
                if (requireSymbols && !String.IsNullOrWhiteSpace(now.DecreeSignatureIssue))
                    throw new InvalidOperationException("Sprawa „" + now.Signature + "”: " + now.DecreeSignatureIssue);
                if (item.StatusId != DistributionWaitingStatusId || now.StatusId != DistributionWaitingStatusId ||
                    item.Amount < 0m || item.Amount != now.Amount || item.Signature != now.Signature ||
                    item.FixedUnitId != now.FixedUnitId || item.PreviousWienaCaseId != now.PreviousWienaCaseId ||
                    item.PreviousWienaFirmaId != now.PreviousWienaFirmaId ||
                    !String.IsNullOrWhiteSpace(now.AssignmentBlockReason) || !String.IsNullOrWhiteSpace(item.AssignmentBlockReason))
                    throw new InvalidOperationException("Zmieniły się dane lub historia sprawy „" + now.Signature + "”. Odśwież statusy i ponownie zaproponuj podział.");
                BillingDistributionOffice oldOffice, newOffice;
                if (!item.AssignedUnitId.HasValue || !oldOffices.TryGetValue(item.AssignedUnitId.Value, out oldOffice) ||
                    !oldOffice.Selected || !newOffices.TryGetValue(item.AssignedUnitId.Value, out newOffice) ||
                    oldOffice.WienaFirmaId <= 0 || oldOffice.WienaFirmaId != newOffice.WienaFirmaId ||
                    (!item.ManualAssignment && now.FixedUnitId.HasValue && item.AssignedUnitId != now.FixedUnitId))
                    throw new InvalidOperationException("Brakuje prawidłowego przypisania sprawy „" + now.Signature + "”. Ponownie zaproponuj podział.");
                if (requireSymbols && !String.IsNullOrWhiteSpace(newOffice.SymbolIssue))
                    throw new InvalidOperationException("Kancelaria „" + newOffice.Name + "”: " + newOffice.SymbolIssue);
                if ((requireSymbols && String.IsNullOrWhiteSpace(newOffice.SymbolSprawWiena)) ||
                    !String.Equals((oldOffice.SymbolSprawWiena ?? String.Empty).Trim(), (newOffice.SymbolSprawWiena ?? String.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Zmieniono lub pominięto symbol kancelarii „" + newOffice.Name + "”. Uzupełnij konfigurację i ponownie zaproponuj podział.");
            }
        }

        private static void ValidateDistributionCaseForApproval(sprawa current, BillingImportBatch batch, BillingDistributionCase expected)
        {
            if (current == null || current.ident != expected.WienaCaseId || current.czyus != 0 ||
                current.id_firmy != batch.BranchId || current.id_symbol != batch.SymbolId || current.rok != batch.Year ||
                current.sygnatura != expected.Signature || current.id_kancelarii.GetValueOrDefault() > 0)
                throw new InvalidOperationException("Sprawa „" + expected.Signature + "” nie znajduje się już w poczekalni lub ma przypisaną kancelarię. Nie zmieniono jej automatycznie.");
        }

        private sealed class DistributionApprovalReceipt
        {
            public Guid BatchId { get; set; }
            public int Firma { get; set; }
            public Guid ProposalId { get; set; }
            public string ResultXml { get; set; }
        }

        private string ExecuteBillingDistributionApproval(BillingImportBatch batch, BillingDistribution saved,
            string login, out bool definitelyRolledBack)
        {
            definitelyRolledBack = false;
            bool commitStarted = false;
            bool receiptAbsent = false;
            using (var wiena = new wiena_centralEntities())
            {
                var context = ((IObjectContextAdapter)wiena).ObjectContext;
                context.CommandTimeout = 120;
                var connection = (EntityConnection)context.Connection;
                EntityTransaction transaction = null;
                try
                {
                    connection.Open();
                    context.ExecuteStoreCommand("SET XACT_ABORT ON; SET ANSI_NULLS ON; SET QUOTED_IDENTIFIER ON; SET ANSI_PADDING ON; SET ANSI_WARNINGS ON; SET CONCAT_NULL_YIELDS_NULL ON; SET ARITHABORT ON; SET NUMERIC_ROUNDABORT OFF;");
                    transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                    int ready = context.ExecuteStoreQuery<int>(
                        "SELECT CASE WHEN OBJECT_ID(N'dbo.LexEnaBillingDistributionApproval', N'U') IS NOT NULL " +
                        "AND EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.sprawa') " +
                        "AND name = N'UX_sprawa_LexEna_Numbering' AND is_unique = 1 AND is_disabled = 0) THEN 1 ELSE 0 END").Single();
                    if (ready != 1)
                        throw new InvalidOperationException("Brak schematu dekretacji w Wienie. Wykonaj skrypt 004_DistributionApproval_Wiena.sql.");
                    var receipt = context.ExecuteStoreQuery<DistributionApprovalReceipt>(
                        "SELECT BatchId, Firma, ProposalId, ResultXml FROM dbo.LexEnaBillingDistributionApproval WITH (UPDLOCK, HOLDLOCK) WHERE BatchId = @id",
                        new SqlParameter("@id", batch.Id)).SingleOrDefault();
                    receiptAbsent = receipt == null;
                    string resultXml;
                    if (receipt != null)
                    {
                        if (receipt.Firma != batch.Firma || receipt.ProposalId != saved.ProposalId)
                            throw new InvalidOperationException("Wiena zawiera potwierdzenie innego podziału tej paczki. Wymagana weryfikacja zapisu.");
                        var result = (BillingDistribution)Utils.DeserializeFromString(receipt.ResultXml, typeof(BillingDistribution));
                        if (result == null || result.BatchId != batch.Id || result.ProposalId != saved.ProposalId || result.ApprovalState != "Approved")
                            throw new InvalidOperationException("Nieprawidłowe potwierdzenie dekretacji w Wienie. Wymagana weryfikacja zapisu.");
                        // Older receipts predate the separate target-signature column.
                        // Enrich only the returned LexEna snapshot; do not rewrite the
                        // original, already committed Wiena receipt or number any case again.
                        SetDistributionSignatureColumns(result, ReadBatchXml(batch.ImportXml));
                        resultXml = Utils.SerializeToString(result, typeof(BillingDistribution));
                    }
                    else
                    {
                        // A table lock also excludes ordinary Wiena writers which do not
                        // participate in application locks. The unique index is the final
                        // guard against a number read earlier by another client.
                        context.ExecuteStoreQuery<int>("SELECT TOP (1) ident FROM dbo.sprawa WITH (TABLOCKX, HOLDLOCK)").ToList();
                        if (!wiena.firma.Any(f => f.ident == batch.BranchId && f.typ_firmy == batch.Firma && f.typ == 1 && f.aktywna == 1))
                            throw new InvalidOperationException("Oddział paczki w Wienie jest nieaktywny lub należy do innej firmy.");
                        BillingDistribution current = LoadBillingDistribution(batch, batch.Firma, wiena);
                        ValidateDistributionApprovalProposal(saved, current);
                        RestoreBillingDistribution(current, batch.DistributionXml);
                        BillingDistributionAllocator.RefreshTotals(current);
                        ApplyDistributionToWiena(wiena, batch, current);
                        current.ApprovalState = "Approved";
                        current.CanApprove = false;
                        current.ApprovalProblem = String.Empty;
                        current.ApprovedAt = DateTime.Now;
                        current.ApprovedBy = RequiredBatchText(login, 256, "użytkownik zatwierdzający");
                        resultXml = Utils.SerializeToString(current, typeof(BillingDistribution));
                        wiena.SaveChanges();
                        context.ExecuteStoreCommand(
                            "INSERT INTO dbo.LexEnaBillingDistributionApproval (BatchId, Firma, ProposalId, ApprovedAt, ApprovedBy, ResultXml) " +
                            "VALUES (@id, @firma, @proposal, @at, @by, @xml)",
                            new SqlParameter("@id", batch.Id), new SqlParameter("@firma", batch.Firma), new SqlParameter("@proposal", current.ProposalId),
                            new SqlParameter("@at", current.ApprovedAt.Value), new SqlParameter("@by", current.ApprovedBy), XmlParameter("@xml", resultXml));
                    }
                    commitStarted = true;
                    transaction.Commit();
                    return resultXml;
                }
                catch
                {
                    if (!commitStarted)
                    {
                        try
                        {
                            if (transaction != null) transaction.Rollback();
                            // A retry may follow a previously committed attempt. Never
                            // reopen editing unless absence of that receipt was proved.
                            definitelyRolledBack = receiptAbsent;
                        }
                        catch { /* Keep Processing if rollback could not be confirmed. */ }
                    }
                    throw;
                }
                finally { if (transaction != null) transaction.Dispose(); }
            }
        }

        private static void ApplyDistributionToWiena(wiena_centralEntities wiena, BillingImportBatch batch, BillingDistribution distribution)
        {
            var candidates = distribution.Cases.Where(c => c.Eligible).OrderBy(c => c.WienaCaseId).ToArray();
            var offices = distribution.Offices.ToDictionary(o => o.UnitId);
            var symbols = wiena.symbol.Where(s => s.id_firmy == batch.BranchId).ToList();
            var targetSymbols = candidates.Select(c => c.AssignedUnitId.Value).Distinct()
                .ToDictionary(id => id, id => ResolveDistributionSymbol(symbols, batch.BranchId, offices[id].SymbolSprawWiena));
            var nextNumbers = new Dictionary<int, int>();
            foreach (int symbolId in targetSymbols.Values.Select(s => s.ident).Distinct().OrderBy(id => id))
            {
                // Never reuse the number of a deleted case.
                nextNumbers.Add(symbolId, QueryDistributionNumberingCases(wiena.sprawa, batch.BranchId, symbolId, batch.Year)
                    .Select(s => (int?)s.poz).Max().GetValueOrDefault());
            }
            DateTime modifiedAt = DateTime.Now;
            for (int offset = 0; offset < candidates.Length; offset += 500)
            {
                var chunk = candidates.Skip(offset).Take(500).ToArray();
                int[] ids = chunk.Select(c => c.WienaCaseId).ToArray();
                var actual = wiena.sprawa.Where(s => ids.Contains(s.ident)).ToDictionary(s => s.ident);
                foreach (var item in chunk)
                {
                    sprawa record;
                    actual.TryGetValue(item.WienaCaseId, out record);
                    symbol target = targetSymbols[item.AssignedUnitId.Value];
                    nextNumbers[target.ident] = ApplyDistributionCaseApproval(record, batch, item, target,
                        offices[item.AssignedUnitId.Value].WienaFirmaId, nextNumbers[target.ident], modifiedAt);
                }
            }
        }

        private static IQueryable<sprawa> QueryDistributionNumberingCases(IQueryable<sprawa> cases, int branchId, int symbolId, int year)
        {
            return cases.Where(s => s.id_firmy == branchId && s.id_symbol == symbolId && s.rok == year);
        }

        private static int ApplyDistributionCaseApproval(sprawa record, BillingImportBatch batch, BillingDistributionCase item,
            symbol target, int officeId, int lastNumber, DateTime modifiedAt)
        {
            ValidateDistributionCaseForApproval(record, batch, item);
            if (target == null || target.id_firmy != batch.BranchId || target.ident <= 0 || officeId <= 0)
                throw new InvalidOperationException("Nieprawidłowa kancelaria lub symbol docelowy sprawy.");
            int number = checked(Math.Max(0, lastNumber) + 1);
            string signature = BuildDistributionSignature(target.oznaczenie, number, batch.Year);
            // Validate the whole change before touching either the record or the receipt.
            if (String.IsNullOrWhiteSpace(item.SignatureBeforeApproval)) item.SignatureBeforeApproval = record.sygnatura;
            item.Signature = signature;
            item.DecreeSignature = signature;
            item.DecreeSignatureIssue = String.Empty;
            item.Decreed = true;
            record.id_kancelarii = officeId;
            record.id_symbol = target.ident;
            record.ksiega = target.typ;
            record.poz = number;
            record.rok = checked((short)batch.Year);
            record.sygnatura = signature;
            record.d_modyfikacji = modifiedAt;
            return number;
        }
    }
}
