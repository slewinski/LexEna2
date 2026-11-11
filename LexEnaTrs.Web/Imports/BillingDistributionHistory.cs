using System;
using System.Collections.Generic;
using System.Linq;
using LexEnaTrs.Web.Models;
using WienaDB.Models;

namespace LexEnaTrs.Web
{
    public partial class LexEnaMeritumDomainService
    {
        private sealed class DistributionHistoryRow
        {
            public int CaseId { get; set; }
            public DateTime CreatedAt { get; set; }
            public int? WienaFirmaId { get; set; }
            public string Signature { get; set; }
            public string CustomerNumber { get; set; }
            public string Pesel { get; set; }
            public string Nip { get; set; }
        }

        private static IQueryable<DistributionHistoryRow> QueryDistributionHistoryRows(
            IQueryable<sprawa> cases, IQueryable<firma> firms,
            IQueryable<spr_dluz> links, IQueryable<skazani> debtors, int firma)
        {
            // A case's owner is id_firmy; the legacy firma navigation uses id_rejon.
            // Keep the case/customer number even if there is no active debtor row.
            return from item in cases
                   where item.czyus == 0
                   join owner in firms on item.id_firmy equals owner.ident
                   where owner.typ_firmy == firma
                   join link in links on (int?)item.ident equals link.id_sprawy into caseLinks
                   from link in caseLinks.DefaultIfEmpty()
                   join debtor in debtors.Where(d => (d.czyus ?? 0) == 0)
                       on (link == null ? (int?)null : link.id_dluz) equals (int?)debtor.id into linkedDebtors
                   from debtor in linkedDebtors.DefaultIfEmpty()
                   select new DistributionHistoryRow
                   {
                       CaseId = item.ident, CreatedAt = item.d_kreacji,
                       WienaFirmaId = item.id_kancelarii, Signature = item.sygnatura,
                       CustomerNumber = item.nr_ewid,
                       Pesel = debtor == null ? null : debtor.pesel,
                       Nip = debtor == null ? null : debtor.nip
                   };
        }

        private static void LoadDistributionHistory(wiena_centralEntities wiena, BillingDistribution distribution, int firma)
        {
            int[] eligibleIds = distribution.Cases.Where(c => c.Eligible && c.WienaCaseId > 0)
                .Select(c => c.WienaCaseId).Distinct().ToArray();
            if (eligibleIds.Length == 0) return;
            List<DistributionHistoryRow> current = new List<DistributionHistoryRow>();
            for (int offset = 0; offset < eligibleIds.Length; offset += 500)
            {
                int[] ids = eligibleIds.Skip(offset).Take(500).ToArray();
                current.AddRange(QueryDistributionHistoryRows(wiena.sprawa.Where(s => ids.Contains(s.ident)),
                    wiena.firma, wiena.spr_dluz, wiena.skazani, firma).ToList());
            }
            if (current.Count == 0)
            {
                ApplyDistributionHistory(distribution, current, new DistributionHistoryRow[0], new Dictionary<int, string>());
                return;
            }

            DateTime newest = current.Max(c => c.CreatedAt);
            var historyQuery = QueryDistributionHistoryRows(wiena.sprawa.Where(s =>
                s.id_kancelarii.HasValue && s.id_kancelarii.Value > 0 && s.d_kreacji <= newest),
                wiena.firma, wiena.spr_dluz, wiena.skazani, firma);
            List<DistributionHistoryRow> history = new List<DistributionHistoryRow>();
            string[] numbers = current.Select(r => NormalizeDistributionCustomerNumber(r.CustomerNumber))
                .Where(v => v != null).Distinct().ToArray();
            string[] pesels = current.Select(r => NormalizeDistributionPesel(r.Pesel))
                .Where(v => v != null).Distinct().ToArray();
            string[] nips = current.Select(r => NormalizeDistributionNip(r.Nip))
                .Where(v => v != null).Distinct().ToArray();

            // Only matching keys are fetched, in bounded IN queries rather than per-debtor queries.
            for (int offset = 0; offset < numbers.Length; offset += 300)
            {
                string[] keys = numbers.Skip(offset).Take(300).ToArray();
                history.AddRange(historyQuery.Where(r => r.CustomerNumber != null &&
                    keys.Contains(r.CustomerNumber.Trim().ToUpper())).ToList());
            }
            for (int offset = 0; offset < pesels.Length; offset += 300)
            {
                string[] keys = pesels.Skip(offset).Take(300).ToArray();
                history.AddRange(historyQuery.Where(r => r.Pesel != null && keys.Contains(r.Pesel.Trim())).ToList());
            }
            for (int offset = 0; offset < nips.Length; offset += 300)
            {
                string[] keys = nips.Skip(offset).Take(300).ToArray();
                history.AddRange(historyQuery.Where(r => r.Nip != null &&
                    keys.Contains(r.Nip.Trim().Replace(" ", "").Replace("-", ""))).ToList());
            }
            ApplyDistributionHistory(distribution, current, history, new Dictionary<int, string>());

            // Resolve names only for the latest matches, including currently inactive offices.
            int[] officeIds = distribution.Cases.Where(c => c.PreviousWienaFirmaId.HasValue)
                .Select(c => c.PreviousWienaFirmaId.Value).Distinct().ToArray();
            Dictionary<int, string> names = new Dictionary<int, string>();
            for (int offset = 0; offset < officeIds.Length; offset += 500)
            {
                int[] ids = officeIds.Skip(offset).Take(500).ToArray();
                foreach (var office in wiena.firma.Where(f => ids.Contains(f.ident)).Select(f => new { f.ident, f.nazwa }).ToList())
                    names[office.ident] = office.nazwa;
            }
            foreach (BillingDistributionCase item in distribution.Cases.Where(c => c.PreviousWienaFirmaId.HasValue))
            {
                string name;
                if (names.TryGetValue(item.PreviousWienaFirmaId.Value, out name) && !String.IsNullOrWhiteSpace(name))
                    item.PreviousOfficeName = name;
            }
        }

        private static string NormalizeDistributionCustomerNumber(string value)
        {
            value = (value ?? String.Empty).Trim().ToUpperInvariant();
            return value.Length == 0 ? null : value;
        }

        private static string NormalizeDistributionPesel(string value)
        {
            value = (value ?? String.Empty).Trim();
            return IsDistributionIdentifier(value, 11) ? value : null;
        }

        private static string NormalizeDistributionNip(string value)
        {
            value = (value ?? String.Empty).Trim().Replace(" ", "").Replace("-", "");
            return IsDistributionIdentifier(value, 10) ? value : null;
        }

        private static bool IsDistributionIdentifier(string value, int length)
        {
            return value.Length == length && value.All(c => c >= '0' && c <= '9');
        }

        private static IEnumerable<string> DistributionHistoryKeys(DistributionHistoryRow row)
        {
            string number = NormalizeDistributionCustomerNumber(row.CustomerNumber);
            string pesel = NormalizeDistributionPesel(row.Pesel);
            string nip = NormalizeDistributionNip(row.Nip);
            if (number != null) yield return "E:" + number;
            if (pesel != null) yield return "P:" + pesel;
            if (nip != null) yield return "N:" + nip;
        }

        private static void ApplyDistributionHistory(BillingDistribution distribution,
            IEnumerable<DistributionHistoryRow> currentRows, IEnumerable<DistributionHistoryRow> historyRows,
            IDictionary<int, string> officeNames)
        {
            HashSet<int> batchIds = new HashSet<int>(distribution.Cases.Where(c => c.WienaCaseId > 0).Select(c => c.WienaCaseId));
            Dictionary<int, DistributionHistoryRow[]> current = currentRows.GroupBy(r => r.CaseId)
                .ToDictionary(g => g.Key, g => g.ToArray());
            Dictionary<string, List<DistributionHistoryRow>> history = new Dictionary<string, List<DistributionHistoryRow>>(StringComparer.Ordinal);
            foreach (DistributionHistoryRow row in historyRows.Where(r => !batchIds.Contains(r.CaseId) && r.WienaFirmaId.HasValue && r.WienaFirmaId.Value > 0))
                foreach (string key in DistributionHistoryKeys(row))
                {
                    List<DistributionHistoryRow> matches;
                    if (!history.TryGetValue(key, out matches)) history[key] = matches = new List<DistributionHistoryRow>();
                    matches.Add(row);
                }
            foreach (string key in history.Keys.ToArray())
                history[key] = history[key].GroupBy(r => r.CaseId).Select(g => g.First())
                    .OrderByDescending(r => r.CreatedAt).ThenByDescending(r => r.CaseId).ToList();
            Dictionary<int, BillingDistributionOffice> mappedOffices = distribution.Offices
                .GroupBy(o => o.WienaFirmaId).Where(g => g.Count() == 1).ToDictionary(g => g.Key, g => g.First());

            foreach (BillingDistributionCase item in distribution.Cases)
            {
                // History is always re-read; saved proposals are not an authority for fixed assignments.
                item.FixedUnitId = null;
                item.PreviousWienaCaseId = null;
                item.PreviousWienaFirmaId = null;
                item.PreviousSignature = String.Empty;
                item.PreviousOfficeName = String.Empty;
                item.AssignmentReason = String.Empty;
                item.AssignmentBlockReason = String.Empty;
                DistributionHistoryRow[] currentCase;
                if (!item.Eligible) continue;
                if (!current.TryGetValue(item.WienaCaseId, out currentCase))
                {
                    item.AssignmentBlockReason = "Nie można odczytać aktualnych danych sprawy/dłużnika w Wienie. Odśwież widok.";
                    continue;
                }
                DateTime createdAt = currentCase[0].CreatedAt;
                DistributionHistoryRow latest = null;
                List<string> matchedTypes = new List<string>();
                foreach (string key in currentCase.SelectMany(DistributionHistoryKeys).Distinct())
                {
                    List<DistributionHistoryRow> matches;
                    if (!history.TryGetValue(key, out matches)) continue;
                    DistributionHistoryRow match = matches.FirstOrDefault(r => r.CreatedAt < createdAt ||
                        (r.CreatedAt == createdAt && r.CaseId < item.WienaCaseId));
                    if (match == null) continue;
                    if (latest == null || match.CreatedAt > latest.CreatedAt ||
                        (match.CreatedAt == latest.CreatedAt && match.CaseId > latest.CaseId))
                    {
                        latest = match;
                        matchedTypes.Clear();
                    }
                    if (latest.CaseId == match.CaseId)
                        matchedTypes.Add(key.StartsWith("E:", StringComparison.Ordinal) ? "nr ewidencyjny" :
                            (key.StartsWith("P:", StringComparison.Ordinal) ? "PESEL" : "NIP"));
                }
                if (latest == null) continue;
                item.PreviousWienaCaseId = latest.CaseId;
                item.PreviousWienaFirmaId = latest.WienaFirmaId;
                item.PreviousSignature = latest.Signature;
                item.AssignmentReason = "Dotychczasowa kancelaria — " + String.Join(", ", matchedTypes.Distinct().OrderBy(s => s));
                string name;
                BillingDistributionOffice office;
                bool mapped = mappedOffices.TryGetValue(latest.WienaFirmaId.Value, out office);
                item.PreviousOfficeName = officeNames.TryGetValue(latest.WienaFirmaId.Value, out name) && !String.IsNullOrWhiteSpace(name)
                    ? name : (mapped ? office.Name : "Kancelaria Wiena ID " + latest.WienaFirmaId.Value);
                if (mapped)
                    item.FixedUnitId = office.UnitId;
                else
                    item.AssignmentBlockReason = "Dotychczasowa kancelaria Wiena (ID " + latest.WienaFirmaId.Value +
                        ") nie ma aktywnego mapowania uczestniczącego w przydziale. Uzupełnij konfigurację i odśwież podział.";
            }
        }
    }
}
