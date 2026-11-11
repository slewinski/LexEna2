using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Xml;
using LexEnaTrs.Web.Models;
using WienaDB.Models;

namespace LexEnaTrs.Web
{
    public partial class LexEnaMeritumDomainService
    {
        private const int DistributionWaitingStatusId = 85;

        [Invoke, RequiresAuthentication]
        public string GetBillingDistributionSymbols(int firma)
        {
            try
            {
                using (var wiena = new wiena_centralEntities())
                {
                    // Read the dictionary itself, including symbols with no cases yet.
                    // The import view contains yearly counters and is not a dictionary.
                    var choices = BuildDistributionSymbolChoices(
                        QueryDistributionSymbolChoices(wiena.symbol, wiena.firma, firma).ToList());
                    return Utils.SerializeToString(choices, typeof(BillingDistributionSymbol[]));
                }
            }
            catch (Exception ex) { return BatchError(ex); }
        }

        private sealed class DistributionSymbolChoiceRow
        {
            public string Symbol { get; set; }
            public int BranchId { get; set; }
            public string BranchName { get; set; }
        }

        private static IQueryable<DistributionSymbolChoiceRow> QueryDistributionSymbolChoices(
            IQueryable<symbol> symbols, IQueryable<firma> branches, int? firma)
        {
            return from item in symbols
                   join branch in branches on item.id_firmy equals branch.ident
                   where branch.typ == 1 && branch.aktywna == 1 && (!firma.HasValue || branch.typ_firmy == firma.Value)
                   select new DistributionSymbolChoiceRow
                   {
                       Symbol = item.oznaczenie, BranchId = branch.ident,
                       BranchName = ((branch.nazwa ?? String.Empty).Trim() + " " + (branch.nazwa1 ?? String.Empty).Trim()).Trim()
                   };
        }

        private static BillingDistributionSymbol[] BuildDistributionSymbolChoices(IEnumerable<DistributionSymbolChoiceRow> rows)
        {
            return rows.Where(r => !String.IsNullOrWhiteSpace(r.Symbol) && r.Symbol.Trim().Length <= 10)
                .GroupBy(r => r.Symbol.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(g =>
                {
                    string designation = g.Select(r => r.Symbol.Trim()).OrderBy(s => s, StringComparer.Ordinal).First();
                    string[] branchNames = g.GroupBy(r => r.BranchId).OrderBy(b => b.Key)
                        .Select(b => String.IsNullOrWhiteSpace(b.First().BranchName)
                            ? "Oddział " + b.Key : b.First().BranchName.Trim()).Distinct().ToArray();
                    return new BillingDistributionSymbol
                    {
                        Symbol = designation, DisplayName = designation + " — " + String.Join(", ", branchNames)
                    };
                }).OrderBy(s => s.Symbol, StringComparer.OrdinalIgnoreCase).ToArray();
        }

        [Invoke, RequiresAuthentication]
        public string GetBillingDistribution(Guid batchId, int firma)
        {
            try
            {
                BillingImportBatch batch = FindDistributionBatch(batchId, firma);
                if (batch.DistributionState == "Approved" || batch.DistributionState == "Processing")
                    return GetLockedBillingDistribution(batch);
                BillingDistribution distribution = LoadBillingDistribution(batch, firma);
                RestoreBillingDistribution(distribution, batch.DistributionXml);
                BillingDistributionAllocator.RefreshTotals(distribution);
                LoadDistributionSignaturePreview(distribution);
                SetDistributionApprovalAvailability(distribution, batch);
                return Utils.SerializeToString(distribution, typeof(BillingDistribution));
            }
            catch (Exception ex) { return BatchError(ex); }
        }

        [Invoke, RequiresAuthentication]
        public string CalculateBillingDistribution(Guid batchId, int firma, int[] unitIds,
            decimal[] countPercents, decimal[] amountPercents)
        {
            try
            {
                ValidateDistributionRequest(unitIds, countPercents, amountPercents);
                BillingImportBatch batch = FindDistributionBatch(batchId, firma);
                if (batch.DistributionState != "Planning")
                    throw new InvalidOperationException("Podział jest zatwierdzony lub w trakcie dekretacji. Nie można go przeliczyć.");
                // Always reload the Wiena statuses and office configuration before calculating.
                BillingDistribution distribution = LoadBillingDistribution(batch, firma);
                ApplyDistributionPercentages(distribution, unitIds, countPercents, amountPercents);
                BillingDistributionAllocator.Allocate(distribution);
                distribution.CalculatedAt = DateTime.Now;
                distribution.HasSavedProposal = true;
                distribution.ProposalId = Guid.NewGuid();
                distribution.ApprovalState = "Planning";
                LoadDistributionSignaturePreview(distribution);
                SetDistributionApprovalAvailability(distribution, null);
                string xml = Utils.SerializeToString(distribution, typeof(BillingDistribution));
                int changed = ObjectContext.ExecuteStoreCommand(
                    "UPDATE dbo.BillingImportBatch SET DistributionXml = @xml, DistributionUpdatedAt = @updated WHERE Id = @id AND Firma = @firma AND Status IN (N'Completed', N'Partial') AND DistributionState = N'Planning' " +
                    "AND (DistributionXml = @original OR (DistributionXml IS NULL AND @original IS NULL))",
                    new SqlParameter("@xml", SqlDbType.NVarChar, -1) { Value = xml },
                    new SqlParameter("@updated", distribution.CalculatedAt), new SqlParameter("@id", batchId), new SqlParameter("@firma", firma),
                    XmlParameter("@original", batch.DistributionXml));
                if (changed != 1) throw new InvalidOperationException("Nie udało się zapisać propozycji podziału. Odśwież paczkę.");
                return xml;
            }
            catch (Exception ex) { return BatchError(ex); }
        }

        private BillingImportBatch FindDistributionBatch(Guid batchId, int firma)
        {
            BillingImportBatch batch = FindBillingBatch(batchId, firma);
            if (batch.Status != "Completed" && batch.Status != "Partial")
                throw new InvalidOperationException("Podział jest dostępny tylko dla zakończonego importu. Paczka w trakcie zapisu lub wymagająca weryfikacji nie może być dzielona.");
            return batch;
        }

        private BillingDistribution LoadBillingDistribution(BillingImportBatch batch, int firma)
        {
            using (wiena_centralEntities wiena = new wiena_centralEntities())
                return LoadBillingDistribution(batch, firma, wiena);
        }

        private BillingDistribution LoadBillingDistribution(BillingImportBatch batch, int firma, wiena_centralEntities wiena)
        {
            ImportSprawBilling import = ReadBatchXml(batch.ImportXml);
            List<BillingDistributionCase> cases = BuildDistributionCases(import);
            List<JednostkaWindykacji> units = ObjectContext.JednostkaWindykacji
                .Where(u => u.UczestniczyWPrzydziale && u.TypJednostki == 1 && u.WienaFirmaId.HasValue).ToList();
            {
                wiena.Configuration.LazyLoadingEnabled = false;
                HashSet<int> activeOffices = new HashSet<int>(wiena.firma
                    .Where(f => f.typ == 9 && f.aktywna == 1).Select(f => f.ident).ToList());
                List<JednostkaWindykacji> validUnits = units.Where(u => activeOffices.Contains(u.WienaFirmaId.Value)).ToList();
                if (validUnits.GroupBy(u => u.WienaFirmaId.Value).Any(g => g.Count() > 1))
                    throw new InvalidOperationException("Konfiguracja zawiera kilka uczestniczących jednostek przypisanych do tej samej kancelarii Wieny. Popraw konfigurację kancelarii.");
                BillingDistributionOffice[] offices = validUnits.OrderBy(u => u.Nazwa).ThenBy(u => u.Id)
                    .Select(u => new BillingDistributionOffice
                    {
                        UnitId = u.Id, WienaFirmaId = u.WienaFirmaId.Value,
                        Name = u.Nazwa ?? String.Empty, Selected = true,
                        SymbolSprawWiena = (u.SymbolSprawWiena ?? String.Empty).Trim()
                    }).ToArray();
                ValidateDistributionSymbols(wiena, batch, offices);

                Dictionary<int, BillingDistributionCase> imported = cases.Where(c => c.WienaCaseId > 0).ToDictionary(c => c.WienaCaseId);
                int[] caseIds = imported.Keys.ToArray();
                Dictionary<int, string> statusNames = wiena.status.ToList().ToDictionary(s => s.ident, s => s.nazwa ?? String.Empty);
                // Keep each IN query safely below SQL Server's 2100 parameter limit.
                for (int offset = 0; offset < caseIds.Length; offset += 500)
                {
                    int[] chunk = caseIds.Skip(offset).Take(500).ToArray();
                    var currentCases = QueryDistributionWienaCases(wiena.sprawa, wiena.firma, chunk).ToList();
                    Dictionary<int, status_spr> statuses = wiena.status_spr
                        .Where(s => chunk.Contains(s.id_sprawy) && s.czyus == 0).ToList()
                        .GroupBy(s => s.id_sprawy).ToDictionary(g => g.Key, g => GetCurrentDistributionStatus(g));
                    foreach (var actual in currentCases)
                    {
                        BillingDistributionCase item = imported[actual.Id];
                        item.Signature = actual.Signature ?? item.Signature;
                        status_spr latest;
                        if (statuses.TryGetValue(actual.Id, out latest) && latest != null)
                        {
                            item.StatusId = latest.id_statusu;
                            string statusName;
                            item.StatusName = statusNames.TryGetValue(latest.id_statusu, out statusName) ? statusName : "Brak nazwy statusu";
                        }
                        SetDistributionEligibility(item, actual.Firma, actual.Deleted, firma);
                    }
                }
                BillingDistribution distribution = new BillingDistribution
                {
                    BatchId = batch.Id, CalculatedAt = batch.DistributionUpdatedAt ?? DateTime.MinValue,
                    HasSavedProposal = false, Offices = offices, Cases = cases.ToArray(),
                    ApprovalState = batch.DistributionState, BranchId = batch.BranchId, NumberingYear = batch.Year
                };
                LoadDistributionHistory(wiena, distribution, firma);
                InitializeDistributionSettings(distribution);
                return distribution;
            }
        }

        private sealed class DistributionWienaCase
        {
            public int Id { get; set; }
            public string Signature { get; set; }
            public int Deleted { get; set; }
            public int? Firma { get; set; }
        }

        private static IQueryable<DistributionWienaCase> QueryDistributionWienaCases(
            IQueryable<sprawa> allCases, IQueryable<firma> firms, int[] caseIds)
        {
            // In Wiena.edmx, sprawa.firma is mapped through id_rejon. The owner
            // used by the importer is id_firmy, so this join must be explicit.
            return from current in allCases
                   where caseIds.Contains(current.ident)
                   join owner in firms on current.id_firmy equals owner.ident into owners
                   from owner in owners.DefaultIfEmpty()
                   select new DistributionWienaCase
                   {
                       Id = current.ident, Signature = current.sygnatura, Deleted = current.czyus,
                       Firma = owner == null ? (int?)null : owner.typ_firmy
                   };
        }

        private static void SetDistributionEligibility(BillingDistributionCase item, int? caseFirma, int caseDeleted, int firma)
        {
            item.Eligible = false;
            if (!caseFirma.HasValue)
                item.ExclusionReason = "Nie znaleziono powiązania sprawy z firmą w Wienie.";
            else if (caseFirma != firma)
                item.ExclusionReason = "Sprawa należy do innej firmy w Wienie.";
            else if (caseDeleted != 0)
                item.ExclusionReason = "Sprawa została usunięta w Wienie.";
            else if (!item.StatusId.HasValue)
                item.ExclusionReason = "Brak aktualnego statusu sprawy w Wienie.";
            else if (item.StatusId != DistributionWaitingStatusId)
                item.ExclusionReason = "Aktualny status Wieny jest inny niż 85.";
            else if (item.Amount < 0m)
                item.ExclusionReason = "Saldo należności z XML jest ujemne; wymagana weryfikacja.";
            else
            {
                item.Eligible = true;
                item.ExclusionReason = String.Empty;
            }
        }

        private static void InitializeDistributionSettings(BillingDistribution distribution)
        {
            // Initialize editable targets only. Opening a batch must never allocate cases.
            BillingDistributionAllocator.SetEqualPercentages(distribution);
            BillingDistributionAllocator.RefreshTotals(distribution);
        }

        private static void ValidateDistributionRequest(int[] unitIds, decimal[] countPercents, decimal[] amountPercents)
        {
            if (unitIds == null || unitIds.Length == 0)
                throw new InvalidOperationException("Wybierz co najmniej jedną kancelarię do podziału.");
            if (unitIds.Any(id => id <= 0) || unitIds.Distinct().Count() != unitIds.Length)
                throw new InvalidOperationException("Lista kancelarii zawiera nieprawidłowy lub powtórzony identyfikator.");
            if (countPercents == null || amountPercents == null || countPercents.Length != unitIds.Length || amountPercents.Length != unitIds.Length)
                throw new InvalidOperationException("Podaj udział liczby spraw i kwoty dla każdej wybranej kancelarii.");
            if (countPercents.Concat(amountPercents).Any(p => p < 0m || p > 100m || Decimal.Round(p, 2) != p))
                throw new InvalidOperationException("Udziały muszą mieścić się w zakresie 0–100% i mieć najwyżej dwa miejsca po przecinku.");
        }

        private static void ApplyDistributionPercentages(BillingDistribution distribution, int[] unitIds,
            decimal[] countPercents, decimal[] amountPercents)
        {
            ValidateDistributionRequest(unitIds, countPercents, amountPercents);
            Dictionary<int, int> indexes = unitIds.Select((id, index) => new { id, index }).ToDictionary(p => p.id, p => p.index);
            HashSet<int> validUnits = new HashSet<int>(distribution.Offices.Select(o => o.UnitId));
            if (unitIds.Any(id => !validUnits.Contains(id)))
                throw new InvalidOperationException("Wybrana kancelaria nie uczestniczy już w podziale lub nie jest aktywna w Wienie. Odśwież widok.");
            foreach (BillingDistributionOffice office in distribution.Offices)
            {
                int index;
                office.Selected = indexes.TryGetValue(office.UnitId, out index);
                office.CountPercent = office.Selected ? countPercents[index] : 0m;
                office.AmountPercent = office.Selected ? amountPercents[index] : 0m;
            }
            BillingDistributionAllocator.ValidatePercentages(distribution);
        }

        // Match the status selection used throughout ImportValidators: no date cutoff,
        // newest non-deleted row by effective date, then by identity for equal dates.
        private static status_spr GetCurrentDistributionStatus(IEnumerable<status_spr> history)
        {
            return history.Where(s => s.czyus == 0).OrderByDescending(s => s.od_dnia)
                .ThenByDescending(s => s.ident).FirstOrDefault();
        }

        private static List<BillingDistributionCase> BuildDistributionCases(ImportSprawBilling import)
        {
            List<BillingDistributionCase> result = new List<BillingDistributionCase>();
            foreach (var group in import.SprawaDescriptor.Where(r => r.czy_ok && r.WienaCaseId > 0).GroupBy(r => r.WienaCaseId))
            {
                SprImportDescriptor first = group.OrderBy(r => r.BatchRowNumber).First();
                result.Add(new BillingDistributionCase
                {
                    WienaCaseId = group.Key, BatchRowNumber = first.BatchRowNumber,
                    Signature = first.sygn_obciaz, SignatureBeforeApproval = first.sygn_obciaz,
                    DebtorName = JoinDistributionValues(group.Select(r => r.Odbiorca.Nazwa)),
                    CustomerNumber = JoinDistributionValues(group.Select(r => r.Odbiorca.Nr_ewidencyjny)),
                    Amount = group.Sum(r => r.Odbiorca.Obciazenia.Sum(o => o.Kwota_do_zaplaty)),
                    Eligible = false, StatusName = String.Empty, ExclusionReason = "Nie znaleziono sprawy w Wienie."
                });
            }
            for (int i = 0; i < import.SprawaDescriptor.Length; i++)
            {
                SprImportDescriptor row = import.SprawaDescriptor[i];
                if (row.czy_ok && row.WienaCaseId > 0) continue;
                result.Add(new BillingDistributionCase
                {
                    WienaCaseId = 0, BatchRowNumber = row.BatchRowNumber > 0 ? row.BatchRowNumber : i + 1,
                    Signature = row.sygn_obciaz, DebtorName = row.Odbiorca.Nazwa,
                    CustomerNumber = row.Odbiorca.Nr_ewidencyjny,
                    Amount = row.Odbiorca.Obciazenia.Sum(o => o.Kwota_do_zaplaty), Eligible = false, StatusName = String.Empty,
                    ExclusionReason = "Pozycja nie została zapisana w Wienie." + (String.IsNullOrWhiteSpace(row.message) ? String.Empty : " " + row.message)
                });
            }
            return result.OrderBy(c => c.BatchRowNumber).ThenBy(c => c.WienaCaseId).ToList();
        }

        private static string JoinDistributionValues(IEnumerable<string> values)
        {
            return String.Join("; ", values.Where(v => !String.IsNullOrWhiteSpace(v)).Select(v => v.Trim()).Distinct());
        }

        private static void RestoreBillingDistribution(BillingDistribution current, string savedXml)
        {
            if (String.IsNullOrWhiteSpace(savedXml)) return;
            BillingDistribution saved = (BillingDistribution)Utils.DeserializeFromString(savedXml, typeof(BillingDistribution));
            if (saved == null || saved.BatchId != current.BatchId)
                throw new InvalidOperationException("Zapisana propozycja podziału jest nieprawidłowa. Przelicz podział ponownie.");
            Dictionary<int, BillingDistributionOffice> savedOffices = (saved.Offices ?? new BillingDistributionOffice[0])
                .GroupBy(o => o.UnitId).Where(g => g.Count() == 1).ToDictionary(g => g.Key, g => g.First());
            foreach (BillingDistributionOffice office in current.Offices)
            {
                BillingDistributionOffice previous;
                office.Selected = savedOffices.TryGetValue(office.UnitId, out previous) &&
                    previous.WienaFirmaId == office.WienaFirmaId && previous.Selected;
                office.CountPercent = office.Selected ? previous.CountPercent : 0m;
                office.AmountPercent = office.Selected ? previous.AmountPercent : 0m;
            }
            // Only units selected in the saved proposal with the same Wiena mapping
            // may restore assignments. A newly required unit is selected below for
            // the next calculation, but must not revive an assignment to an old mapping.
            HashSet<int> restorableUnits = new HashSet<int>(current.Offices.Where(o => o.Selected).Select(o => o.UnitId));
            HashSet<int> requiredUnits = new HashSet<int>(current.Cases.Where(c => c.Eligible && c.FixedUnitId.HasValue)
                .Select(c => c.FixedUnitId.Value));
            foreach (BillingDistributionOffice office in current.Offices.Where(o => requiredUnits.Contains(o.UnitId)))
                office.Selected = true;
            XmlDocument savedDocument = new XmlDocument { XmlResolver = null };
            savedDocument.LoadXml(savedXml);
            // Existing saved proposals predate the configurable count/value percentages.
            // Explicit zero percentages in newer proposals must not be mistaken for old XML.
            if (savedDocument.SelectNodes("//*[local-name()='CountPercent' or local-name()='AmountPercent']").Count == 0)
                BillingDistributionAllocator.SetEqualPercentages(current);
            Dictionary<int, BillingDistributionCase> savedCases = (saved.Cases ?? new BillingDistributionCase[0])
                .Where(c => c.WienaCaseId > 0).GroupBy(c => c.WienaCaseId).Where(g => g.Count() == 1)
                .ToDictionary(g => g.Key, g => g.First());
            foreach (BillingDistributionCase item in current.Cases)
            {
                BillingDistributionCase previous;
                if (item.Eligible && String.IsNullOrWhiteSpace(item.AssignmentBlockReason) &&
                    savedCases.TryGetValue(item.WienaCaseId, out previous) && previous.AssignedUnitId.HasValue &&
                    restorableUnits.Contains(previous.AssignedUnitId.Value) &&
                    (!item.FixedUnitId.HasValue || item.FixedUnitId == previous.AssignedUnitId ||
                        (previous.ManualAssignment && item.FixedUnitId == previous.FixedUnitId &&
                         item.PreviousWienaCaseId == previous.PreviousWienaCaseId && item.PreviousWienaFirmaId == previous.PreviousWienaFirmaId)))
                {
                    item.AssignedUnitId = previous.AssignedUnitId;
                    // A reviewed manual exception is retained only while the history
                    // that the user saw still matches the latest Wiena data.
                    if (previous.ManualAssignment && item.FixedUnitId == previous.FixedUnitId &&
                        item.PreviousWienaCaseId == previous.PreviousWienaCaseId && item.PreviousWienaFirmaId == previous.PreviousWienaFirmaId)
                    {
                        item.ManualAssignment = true;
                        item.ManualAssignedAt = previous.ManualAssignedAt;
                        item.ManualAssignedBy = previous.ManualAssignedBy;
                    }
                }
            }
            current.HasSavedProposal = true;
            current.CalculatedAt = saved.CalculatedAt;
            current.ProposalId = saved.ProposalId;
        }

        private void ValidateDistributionOffice(JednostkaWindykacji unit)
        {
            unit.SymbolSprawWiena = (unit.SymbolSprawWiena ?? String.Empty).Trim();
            if (unit.SymbolSprawWiena.Length > 10)
                throw new ValidationException("Symbol spraw Wieny może mieć najwyżej 10 znaków.");
            if (unit.SymbolSprawWiena.Length > 0)
            {
                using (var wiena = new wiena_centralEntities())
                {
                    // Units are shared configuration; the particular batch branch is
                    // checked again when approving a distribution.
                    var choices = BuildDistributionSymbolChoices(QueryDistributionSymbolChoices(wiena.symbol, wiena.firma, null).ToList());
                    var chosen = choices.FirstOrDefault(s => String.Equals(s.Symbol, unit.SymbolSprawWiena, StringComparison.OrdinalIgnoreCase));
                    if (chosen == null)
                        throw new ValidationException("Wybierz symbol spraw z aktualnej listy symboli Wieny.");
                    unit.SymbolSprawWiena = chosen.Symbol;
                }
            }
            if (!unit.WienaFirmaId.HasValue && !unit.UczestniczyWPrzydziale) return;
            if (unit.TypJednostki != 1 || !unit.WienaFirmaId.HasValue || unit.WienaFirmaId.Value <= 0)
                throw new ValidationException("Uczestnik podziału musi być zewnętrzną kancelarią prawną z wybraną kancelarią Wieny.");
            int wienaId = unit.WienaFirmaId.Value;
            using (wiena_centralEntities wiena = new wiena_centralEntities())
                if (!wiena.firma.Any(f => f.ident == wienaId && f.typ == 9 && f.aktywna == 1))
                    throw new ValidationException("Wybierz aktywną kancelarię prawną z Wieny.");
            if (unit.UczestniczyWPrzydziale && ObjectContext.JednostkaWindykacji.Any(u => u.Id != unit.Id &&
                u.UczestniczyWPrzydziale && u.WienaFirmaId == wienaId))
                throw new ValidationException("Ta kancelaria Wieny jest już przypisana do innej jednostki uczestniczącej w podziale.");
        }
    }
}
