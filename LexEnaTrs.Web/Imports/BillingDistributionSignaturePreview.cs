using System;
using System.Collections.Generic;
using System.Linq;
using LexEnaTrs.Web.Models;
using WienaDB.Models;

namespace LexEnaTrs.Web
{
    public partial class LexEnaMeritumDomainService
    {
        private static void LoadDistributionSignaturePreview(BillingDistribution distribution)
        {
            if (!distribution.Cases.Any(c => c.Eligible && c.AssignedUnitId.HasValue && !c.Decreed)) return;
            using (var wiena = new wiena_centralEntities())
            {
                var symbols = wiena.symbol.Where(s => s.id_firmy == distribution.BranchId).ToList();
                var assignedUnits = new HashSet<int>(distribution.Cases.Where(c => c.Eligible && c.AssignedUnitId.HasValue && !c.Decreed)
                    .Select(c => c.AssignedUnitId.Value));
                var symbolIds = new HashSet<int>();
                foreach (var office in distribution.Offices.Where(o => assignedUnits.Contains(o.UnitId)))
                {
                    try { symbolIds.Add(ResolveDistributionSymbol(symbols, distribution.BranchId, office.SymbolSprawWiena).ident); }
                    catch (InvalidOperationException) { /* The row will show the dictionary problem below. */ }
                }
                var numbers = new Dictionary<int, int>();
                foreach (int symbolId in symbolIds.OrderBy(id => id))
                    numbers.Add(symbolId, QueryDistributionNumberingCases(wiena.sprawa, distribution.BranchId, symbolId, distribution.NumberingYear)
                        .Select(s => (int?)s.poz).Max().GetValueOrDefault());
                PopulateDistributionSignaturePreview(distribution, symbols, numbers);
            }
        }

        private static void PopulateDistributionSignaturePreview(BillingDistribution distribution,
            IEnumerable<symbol> symbols, IDictionary<int, int> lastNumbers)
        {
            var dictionary = symbols.ToList();
            var numbers = new Dictionary<int, int>(lastNumbers);
            var numberIssues = new Dictionary<int, string>();
            foreach (var item in distribution.Cases)
            {
                item.DecreeSignature = item.Decreed ? item.Signature : null;
                item.DecreeSignatureIssue = null;
            }
            // Match the final assignment order and share one counter between offices
            // that use the same symbol. This is a read-only estimate, not a reservation.
            foreach (var item in distribution.Cases.Where(c => c.Eligible && c.AssignedUnitId.HasValue && !c.Decreed).OrderBy(c => c.WienaCaseId))
            {
                symbol target = null;
                try
                {
                    var offices = distribution.Offices.Where(o => o.UnitId == item.AssignedUnitId.Value && o.Selected).ToArray();
                    if (offices.Length != 1)
                        throw new InvalidOperationException("Brak kancelarii przypisanej do sprawy w bieżącym podziale.");
                    target = ResolveDistributionSymbol(dictionary, distribution.BranchId, offices[0].SymbolSprawWiena);
                    string previousIssue;
                    if (numberIssues.TryGetValue(target.ident, out previousIssue))
                        throw new InvalidOperationException(previousIssue);
                    int previous;
                    if (!numbers.TryGetValue(target.ident, out previous))
                        throw new InvalidOperationException("Nie odczytano numeracji symbolu w Wienie.");
                    int number = checked(Math.Max(0, previous) + 1);
                    item.DecreeSignature = BuildDistributionSignature(target.oznaczenie, number, distribution.NumberingYear);
                    numbers[target.ident] = number;
                }
                catch (Exception ex)
                {
                    if (!(ex is InvalidOperationException) && !(ex is OverflowException)) throw;
                    item.DecreeSignatureIssue = ex is OverflowException ? "Wyczerpano zakres numeracji symbolu w Wienie." : ex.Message;
                    if (target != null) numberIssues[target.ident] = item.DecreeSignatureIssue;
                }
            }
        }

        private static void SetDistributionSignatureColumns(BillingDistribution distribution, ImportSprawBilling import)
        {
            var registered = BuildDistributionCases(import).Where(c => c.WienaCaseId > 0).ToDictionary(c => c.WienaCaseId);
            foreach (var item in distribution.Cases)
            {
                BillingDistributionCase source;
                if (registered.TryGetValue(item.WienaCaseId, out source) && !String.IsNullOrWhiteSpace(source.SignatureBeforeApproval))
                    item.SignatureBeforeApproval = source.SignatureBeforeApproval;
                if (item.Decreed)
                {
                    item.DecreeSignature = item.Signature;
                    item.DecreeSignatureIssue = null;
                }
            }
        }
    }
}
