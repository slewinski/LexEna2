using System;
using System.Collections.Generic;
using System.Linq;

namespace LexEnaTrs.Web.Models
{
    public class BillingDistributionSymbol
    {
        public string Symbol { get; set; }
        public string DisplayName { get; set; }
    }

    // Shared XML contracts: no Entity Framework or Silverlight UI dependencies.
    public class BillingDistribution
    {
        public Guid BatchId { get; set; }
        public DateTime CalculatedAt { get; set; }
        public bool HasSavedProposal { get; set; }
        public Guid ProposalId { get; set; }
        public string ApprovalState { get; set; }
        public bool CanApprove { get; set; }
        public string ApprovalProblem { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ApprovedBy { get; set; }
        public int NumberingYear { get; set; }
        public int BranchId { get; set; }
        public BillingDistributionOffice[] Offices { get; set; }
        public BillingDistributionCase[] Cases { get; set; }
    }

    public class BillingDistributionOffice
    {
        public int UnitId { get; set; }
        public int WienaFirmaId { get; set; }
        public string Name { get; set; }
        public string SymbolSprawWiena { get; set; }
        public string SymbolIssue { get; set; }
        public bool Selected { get; set; }
        public decimal CountPercent { get; set; }
        public decimal AmountPercent { get; set; }
        public int CaseCount { get; set; }
        public int TargetCaseCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal AmountDeviation { get; set; }
    }

    public class BillingDistributionCase
    {
        public int WienaCaseId { get; set; }
        public int BatchRowNumber { get; set; }
        public string Signature { get; set; }
        public string SignatureBeforeApproval { get; set; }
        public string DecreeSignature { get; set; }
        public string DecreeSignatureIssue { get; set; }
        public bool Decreed { get; set; }
        public string DebtorName { get; set; }
        public string CustomerNumber { get; set; }
        public decimal Amount { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public bool Eligible { get; set; }
        public string ExclusionReason { get; set; }
        public int? AssignedUnitId { get; set; }
        public string AssignedOfficeName { get; set; }
        public bool ManualAssignment { get; set; }
        public DateTime? ManualAssignedAt { get; set; }
        public string ManualAssignedBy { get; set; }
        public int? FixedUnitId { get; set; }
        public int? PreviousWienaCaseId { get; set; }
        public int? PreviousWienaFirmaId { get; set; }
        public string PreviousSignature { get; set; }
        public string PreviousOfficeName { get; set; }
        public string AssignmentReason { get; set; }
        public string AssignmentBlockReason { get; set; }
        public bool IsFixedAssignment { get { return FixedUnitId.HasValue && !ManualAssignment; } }
    }

    public static class BillingDistributionAllocator
    {
        // Fixed work budgets keep large batches responsive and results reproducible.
        // For small groups all swaps are considered; large groups sample across the
        // sorted amount range, including both extremes. This is a heuristic, not an
        // assertion of the global optimum of the partition problem.
        private const int MaximumPasses = 8;
        private const int MaximumSwapComparisons = 250000;
        private const int MaximumOfficePairVisits = 20000;
        private const int MaximumSamplesPerOffice = 48;

        public static void SetEqualPercentages(BillingDistribution data)
        {
            Validate(data);
            int selected = data.Offices.Count(o => o.Selected);
            decimal percent = selected == 0 ? 0m : Math.Round(100m / selected, 2);
            foreach (BillingDistributionOffice office in data.Offices)
            {
                office.CountPercent = office.Selected ? percent : 0m;
                office.AmountPercent = office.Selected ? percent : 0m;
            }
        }

        public static void ValidatePercentages(BillingDistribution data)
        {
            Validate(data);
            BillingDistributionOffice[] selected = data.Offices.Where(o => o.Selected).ToArray();
            if (selected.Length == 0)
                throw new InvalidOperationException("Wybierz co najmniej jedną kancelarię do podziału spraw.");
            ValidatePercentAxis(selected.Select(o => o.CountPercent).ToArray(), "liczby spraw");
            ValidatePercentAxis(selected.Select(o => o.AmountPercent).ToArray(), "wartości spraw");
        }

        private static void ValidatePercentAxis(decimal[] shares, string description)
        {
            if (shares.Any(p => p < 0m || p > 100m))
                throw new InvalidOperationException("Udział procentowy " + description + " musi mieścić się w zakresie od 0 do 100.");
            if (shares.Any(p => p != Math.Round(p, 2)))
                throw new InvalidOperationException("Udział procentowy " + description + " może mieć najwyżej dwa miejsca po przecinku.");
            decimal sum = shares.Sum();
            decimal equal = Math.Round(100m / shares.Length, 2);
            if (sum <= 0m || (sum != 100m && !shares.All(p => p == equal)))
                throw new InvalidOperationException("Suma udziałów " + description + " musi wynosić 100%. Dopuszczalny jest też równy podział zaokrąglony do dwóch miejsc po przecinku.");
        }

        public static BillingDistribution Allocate(BillingDistribution data)
        {
            Validate(data);
            BillingDistributionOffice[] offices = data.Offices.Where(o => o.Selected).OrderBy(o => o.UnitId).ToArray();
            BillingDistributionCase[] cases = data.Cases.Where(c => c.Eligible).ToArray();
            if (cases.Length > 0 && offices.Length == 0)
                throw new InvalidOperationException("Wybierz co najmniej jedną kancelarię do podziału spraw.");
            if (cases.Length > 0) ValidatePercentages(data);
            ValidateFixedAssignments(cases, offices);
            // Detect overflow before changing the existing proposal.
            decimal total = SumEligibleAmounts(data.Cases);
            Array.Sort(cases, CaseComparer.Instance);

            foreach (BillingDistributionCase item in data.Cases) ClearAssignment(item);
            data.HasSavedProposal = false;
            if (cases.Length == 0)
            {
                RefreshTotals(data);
                return data;
            }

            List<OfficeBucket> buckets = SetTargets(data, cases.Length, total);
            SeedFixedCases(buckets, cases);
            OfficeHeap heap = new OfficeHeap();
            foreach (OfficeBucket bucket in buckets)
                if (bucket.CaseCount < bucket.Capacity) heap.Push(bucket);
            // Largest first, respecting count quotas. Amount targets are independent
            // from count shares; compare each office's remaining amount target.
            foreach (BillingDistributionCase item in cases.Where(c => !c.FixedUnitId.HasValue))
            {
                OfficeBucket bucket = heap.Pop();
                bucket.Cases.Add(item);
                bucket.Amount += item.Amount;
                if (bucket.CaseCount < bucket.Capacity) heap.Push(bucket);
            }

            ImproveBySwaps(buckets);
            foreach (OfficeBucket bucket in buckets)
                foreach (BillingDistributionCase item in bucket.FixedCases.Concat(bucket.Cases))
                {
                    item.AssignedUnitId = bucket.Office.UnitId;
                    item.AssignedOfficeName = bucket.Office.Name ?? String.Empty;
                }
            RefreshTotals(data);
            return data;
        }

        public static void RefreshTotals(BillingDistribution data)
        {
            Validate(data);
            decimal total = SumEligibleAmounts(data.Cases);
            Dictionary<int, BillingDistributionOffice> selected = data.Offices.Where(o => o.Selected).ToDictionary(o => o.UnitId);
            // Editing percentages is allowed to be temporarily incomplete. Targets
            // are a normalized preview; only Allocate/ValidatePercentages rejects sums.
            SetTargets(data, data.Cases.Count(c => c.Eligible), total);
            foreach (BillingDistributionOffice office in data.Offices)
            {
                office.CaseCount = 0;
                office.TotalAmount = 0m;
                office.AmountDeviation = 0m;
            }
            foreach (BillingDistributionCase item in data.Cases)
            {
                BillingDistributionOffice office;
                if (!item.Eligible || !item.AssignedUnitId.HasValue || !String.IsNullOrWhiteSpace(item.AssignmentBlockReason) ||
                    HasUnresolvedHistory(item) || (!item.ManualAssignment && item.FixedUnitId.HasValue && item.AssignedUnitId != item.FixedUnitId) ||
                    !selected.TryGetValue(item.AssignedUnitId.Value, out office))
                {
                    ClearAssignment(item);
                    continue;
                }
                office.CaseCount++;
                office.TotalAmount += item.Amount;
                item.AssignedOfficeName = office.Name ?? String.Empty;
            }
            foreach (BillingDistributionOffice office in selected.Values)
                office.AmountDeviation = office.TotalAmount - office.TargetAmount;
        }

        private static bool HasUnresolvedHistory(BillingDistributionCase item)
        {
            return !item.FixedUnitId.HasValue && (item.PreviousWienaCaseId.HasValue || item.PreviousWienaFirmaId.HasValue);
        }

        private static void ValidateFixedAssignments(BillingDistributionCase[] cases, BillingDistributionOffice[] offices)
        {
            HashSet<int> selected = new HashSet<int>(offices.Select(o => o.UnitId));
            foreach (BillingDistributionCase item in cases)
            {
                if (!String.IsNullOrWhiteSpace(item.AssignmentBlockReason))
                    throw new InvalidOperationException("Sprawa " + item.Signature + ": " + item.AssignmentBlockReason);
                if (HasUnresolvedHistory(item))
                    throw new InvalidOperationException("Sprawa " + item.Signature + ": uzupełnij mapowanie dotychczasowej kancelarii przed podziałem.");
                if (item.FixedUnitId.HasValue && !selected.Contains(item.FixedUnitId.Value))
                    throw new InvalidOperationException("Sprawa " + item.Signature + ": dotychczasowa kancelaria musi uczestniczyć w podziale tej paczki.");
            }
        }

        private static void SeedFixedCases(List<OfficeBucket> buckets, BillingDistributionCase[] cases)
        {
            if (!cases.Any(c => c.FixedUnitId.HasValue)) return;
            Dictionary<int, OfficeBucket> byId = buckets.ToDictionary(b => b.Office.UnitId);
            int fixedCount = 0;
            foreach (BillingDistributionCase item in cases.Where(c => c.FixedUnitId.HasValue))
            {
                OfficeBucket bucket = byId[item.FixedUnitId.Value];
                bucket.FixedCases.Add(item);
                bucket.Amount += item.Amount;
                fixedCount++;
            }

            // Fixed cases may already exceed a requested count quota. Allocate the
            // remaining count slots by marginal squared deviation from the original
            // percentage targets, with fixed counts as mandatory lower bounds.
            // The min-heap makes this exact count apportionment O(N log offices).
            OfficeHeap capacityHeap = new OfficeHeap(true);
            foreach (OfficeBucket bucket in buckets)
            {
                bucket.Capacity = bucket.FixedCases.Count;
                bucket.MinimumCount = bucket.FixedCases.Count;
                bucket.MaximumCount = cases.Length - fixedCount + bucket.FixedCases.Count;
                if (bucket.Capacity < bucket.MaximumCount) capacityHeap.Push(bucket);
            }
            for (int remaining = cases.Length - fixedCount; remaining > 0; remaining--)
            {
                OfficeBucket bucket = capacityHeap.Pop();
                bucket.Capacity++;
                if (bucket.Capacity < bucket.MaximumCount) capacityHeap.Push(bucket);
            }
        }

        private static List<OfficeBucket> SetTargets(BillingDistribution data, int caseCount, decimal totalAmount)
        {
            foreach (BillingDistributionOffice office in data.Offices)
            {
                office.TargetCaseCount = 0;
                office.TargetAmount = 0m;
            }
            List<OfficeBucket> buckets = data.Offices.Where(o => o.Selected).OrderBy(o => o.UnitId)
                .Select(o => new OfficeBucket(o)).ToList();
            decimal countSum = buckets.Sum(b => PreviewShare(b.Office.CountPercent));
            decimal amountSum = buckets.Sum(b => PreviewShare(b.Office.AmountPercent));
            if (countSum > 0m)
            {
                foreach (OfficeBucket bucket in buckets)
                {
                    bucket.IdealCount = caseCount * PreviewShare(bucket.Office.CountPercent) / countSum;
                    // Silverlight has no decimal overloads of Math.Floor/Ceiling.
                    bucket.MinimumCount = (int)bucket.IdealCount;
                    bucket.MaximumCount = bucket.MinimumCount + (bucket.IdealCount > bucket.MinimumCount ? 1 : 0);
                    bucket.Capacity = bucket.MinimumCount;
                }
                int remainder = caseCount - buckets.Sum(b => b.Capacity);
                foreach (OfficeBucket bucket in buckets.OrderByDescending(b => b.IdealCount - b.MinimumCount)
                    .ThenBy(b => b.Office.UnitId).Take(remainder))
                    bucket.Capacity++;
                foreach (OfficeBucket bucket in buckets) bucket.Office.TargetCaseCount = bucket.Capacity;
            }
            if (amountSum > 0m)
            {
                OfficeBucket lastPositive = buckets.Last(b => PreviewShare(b.Office.AmountPercent) > 0m);
                decimal remaining = totalAmount;
                int targetScale = AmountTargetScale(totalAmount);
                foreach (OfficeBucket bucket in buckets)
                {
                    decimal share = PreviewShare(bucket.Office.AmountPercent);
                    if (share == 0m) continue;
                    // Multiply only by a fraction <=1, avoiding decimal overflow. The
                    // final positive office receives the exact division remainder.
                    decimal target = Object.ReferenceEquals(bucket, lastPositive) ? remaining :
                        Math.Round(totalAmount * (share / amountSum), targetScale);
                    target = Math.Min(target, remaining);
                    bucket.Office.TargetAmount = target;
                    remaining -= target;
                }
            }
            return buckets;
        }

        private static int AmountTargetScale(decimal total)
        {
            int desired = Math.Max(2, (Decimal.GetBits(total)[3] >> 16) & 0x7f);
            int integerDigits = 0;
            for (decimal integer = total; integer >= 1m; integer /= 10m) integerDigits++;
            // Keep target additions exact. Normally this means cents (or the source's
            // finer precision); very large decimals need fewer fractional digits.
            return Math.Min(desired, Math.Max(0, 28 - integerDigits));
        }

        private static decimal PreviewShare(decimal value)
        {
            return Math.Max(0m, Math.Min(100m, value));
        }

        private static void ImproveBySwaps(List<OfficeBucket> allBuckets)
        {
            List<OfficeBucket> active = allBuckets.Where(b => b.MaximumCount > 0).ToList();
            int comparisons = 0;
            int pairVisits = 0;
            for (int pass = 0; pass < MaximumPasses; pass++)
            {
                bool changed = false;
                active.Sort(delegate(OfficeBucket a, OfficeBucket b)
                {
                    int result = b.Deviation.CompareTo(a.Deviation);
                    return result != 0 ? result : a.Office.UnitId.CompareTo(b.Office.UnitId);
                });
                foreach (OfficeBucket bucket in active) bucket.Cases.Sort(CaseComparer.Instance);
                for (int i = 0; i < active.Count - 1; i++)
                    for (int j = i + 1; j < active.Count; j++)
                    {
                        if (++pairVisits > MaximumOfficePairVisits || comparisons >= MaximumSwapComparisons) return;
                        OfficeBucket heavy = active[i].Deviation >= active[j].Deviation ? active[i] : active[j];
                        OfficeBucket light = Object.ReferenceEquals(heavy, active[i]) ? active[j] : active[i];
                        // Half the difference of deviations is the ideal transfer.
                        // Dividing each signed deviation first avoids a 2*total overflow.
                        decimal idealTransfer = heavy.Deviation / 2m - light.Deviation / 2m;
                        if (idealTransfer <= 0m) continue;
                        decimal bestGap = idealTransfer;
                        int heavyIndex = -1;
                        int lightIndex = -1;
                        bool moveSingleCase = false;
                        // Change ownership of a remainder only within floor/ceil and
                        // without worsening distance from weighted count ideals.
                        if (CanMoveCase(heavy, light))
                            foreach (int a in SampleIndices(heavy.Cases.Count))
                            {
                                if (comparisons++ >= MaximumSwapComparisons) break;
                                decimal transfer = heavy.Cases[a].Amount;
                                if (transfer <= 0m) continue;
                                decimal candidateGap = Math.Abs(idealTransfer - transfer);
                                if (candidateGap < bestGap)
                                {
                                    bestGap = candidateGap;
                                    heavyIndex = a;
                                    moveSingleCase = true;
                                }
                            }
                        foreach (int a in SampleIndices(heavy.Cases.Count))
                        {
                            foreach (int b in SampleIndices(light.Cases.Count))
                            {
                                if (comparisons++ >= MaximumSwapComparisons) break;
                                decimal transfer = heavy.Cases[a].Amount - light.Cases[b].Amount;
                                if (transfer <= 0m) continue;
                                // Avoid squaring money (or multiplying by two) to prevent overflow.
                                // Reducing this pair's absolute gap also reduces sum of squared deviations.
                                decimal candidateGap = Math.Abs(idealTransfer - transfer);
                                if (candidateGap < bestGap)
                                {
                                    bestGap = candidateGap;
                                    heavyIndex = a;
                                    lightIndex = b;
                                    moveSingleCase = false;
                                }
                            }
                            if (bestGap == 0m || comparisons >= MaximumSwapComparisons) break;
                        }
                        if (heavyIndex < 0) continue;
                        BillingDistributionCase heavyCase = heavy.Cases[heavyIndex];
                        decimal difference;
                        if (moveSingleCase)
                        {
                            difference = heavyCase.Amount;
                            heavy.Cases.RemoveAt(heavyIndex);
                            light.Cases.Add(heavyCase);
                        }
                        else
                        {
                            BillingDistributionCase lightCase = light.Cases[lightIndex];
                            difference = heavyCase.Amount - lightCase.Amount;
                            heavy.Cases[heavyIndex] = lightCase;
                            light.Cases[lightIndex] = heavyCase;
                        }
                        heavy.Amount -= difference;
                        light.Amount += difference;
                        changed = true;
                    }
                if (!changed) return;
            }
        }

        private static bool CanMoveCase(OfficeBucket from, OfficeBucket to)
        {
            if (from.CaseCount <= from.MinimumCount || to.CaseCount >= to.MaximumCount) return false;
            decimal fromBefore = from.CaseCount - from.IdealCount;
            decimal toBefore = to.CaseCount - to.IdealCount;
            decimal fromAfter = fromBefore - 1m;
            decimal toAfter = toBefore + 1m;
            return fromAfter * fromAfter + toAfter * toAfter <= fromBefore * fromBefore + toBefore * toBefore;
        }

        private static IEnumerable<int> SampleIndices(int count)
        {
            int samples = Math.Min(count, MaximumSamplesPerOffice);
            if (samples == 1) { yield return 0; yield break; }
            for (int i = 0; i < samples; i++)
                yield return (int)((long)i * (count - 1) / (samples - 1));
        }

        private static decimal SumEligibleAmounts(IEnumerable<BillingDistributionCase> cases)
        {
            try { return cases.Where(c => c.Eligible).Sum(c => c.Amount); }
            catch (OverflowException ex)
            {
                throw new InvalidOperationException("Suma kwot spraw przekracza obsługiwany zakres.", ex);
            }
        }

        private static void Validate(BillingDistribution data)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (data.Offices == null) data.Offices = new BillingDistributionOffice[0];
            if (data.Cases == null) data.Cases = new BillingDistributionCase[0];
            HashSet<int> officeIds = new HashSet<int>();
            foreach (BillingDistributionOffice office in data.Offices)
                if (office == null || office.UnitId <= 0 || !officeIds.Add(office.UnitId))
                    throw new InvalidOperationException("Lista kancelarii zawiera pustą pozycję albo nieprawidłowy lub powtórzony identyfikator.");
            HashSet<int> caseIds = new HashSet<int>();
            HashSet<int> rowNumbers = new HashSet<int>();
            foreach (BillingDistributionCase item in data.Cases)
            {
                if (item == null) throw new InvalidOperationException("Lista spraw zawiera pustą pozycję.");
                if (item.Eligible && item.Amount < 0m) throw new InvalidOperationException("Kwota sprawy do podziału nie może być ujemna.");
                if (item.WienaCaseId < 0 || item.BatchRowNumber < 0 ||
                    (item.WienaCaseId == 0 && (item.BatchRowNumber == 0 || !rowNumbers.Add(item.BatchRowNumber))) ||
                    (item.WienaCaseId > 0 && !caseIds.Add(item.WienaCaseId)))
                    throw new InvalidOperationException("Lista spraw zawiera nieprawidłowy lub powtórzony identyfikator.");
            }
        }

        private static void ClearAssignment(BillingDistributionCase item)
        {
            item.AssignedUnitId = null;
            item.AssignedOfficeName = String.Empty;
            item.ManualAssignment = false;
            item.ManualAssignedAt = null;
            item.ManualAssignedBy = null;
            item.DecreeSignature = null;
            item.DecreeSignatureIssue = null;
        }

        private sealed class OfficeBucket
        {
            public readonly BillingDistributionOffice Office;
            public int Capacity;
            public int MinimumCount;
            public int MaximumCount;
            public decimal IdealCount;
            public readonly List<BillingDistributionCase> Cases = new List<BillingDistributionCase>();
            // Fixed cases are separate: swap/move candidates come only from Cases.
            public readonly List<BillingDistributionCase> FixedCases = new List<BillingDistributionCase>();
            public int CaseCount { get { return FixedCases.Count + Cases.Count; } }
            public decimal Amount;
            public decimal Deviation { get { return Amount - Office.TargetAmount; } }

            public OfficeBucket(BillingDistributionOffice office)
            {
                Office = office;
            }
        }

        private sealed class CaseComparer : IComparer<BillingDistributionCase>
        {
            public static readonly CaseComparer Instance = new CaseComparer();
            public int Compare(BillingDistributionCase a, BillingDistributionCase b)
            {
                int result = b.Amount.CompareTo(a.Amount);
                if (result != 0) return result;
                if (a.WienaCaseId > 0 && b.WienaCaseId > 0) return a.WienaCaseId.CompareTo(b.WienaCaseId);
                if (a.WienaCaseId > 0) return -1;
                if (b.WienaCaseId > 0) return 1;
                return a.BatchRowNumber.CompareTo(b.BatchRowNumber);
            }
        }

        private sealed class OfficeHeap
        {
            private readonly List<OfficeBucket> values = new List<OfficeBucket>();
            private readonly bool countCapacity;

            public OfficeHeap(bool countCapacity = false)
            {
                this.countCapacity = countCapacity;
            }

            public void Push(OfficeBucket value)
            {
                values.Add(value);
                int index = values.Count - 1;
                while (index > 0)
                {
                    int parent = (index - 1) / 2;
                    if (Compare(values[parent], value) <= 0) break;
                    values[index] = values[parent];
                    index = parent;
                }
                values[index] = value;
            }

            public OfficeBucket Pop()
            {
                OfficeBucket result = values[0];
                OfficeBucket last = values[values.Count - 1];
                values.RemoveAt(values.Count - 1);
                if (values.Count == 0) return result;
                int index = 0;
                while (index * 2 + 1 < values.Count)
                {
                    int child = index * 2 + 1;
                    if (child + 1 < values.Count && Compare(values[child + 1], values[child]) < 0) child++;
                    if (Compare(last, values[child]) <= 0) break;
                    values[index] = values[child];
                    index = child;
                }
                values[index] = last;
                return result;
            }

            private int Compare(OfficeBucket a, OfficeBucket b)
            {
                if (countCapacity)
                {
                    int capacityOrder = (a.Capacity - a.IdealCount).CompareTo(b.Capacity - b.IdealCount);
                    return capacityOrder != 0 ? capacityOrder : a.Office.UnitId.CompareTo(b.Office.UnitId);
                }
                int result = a.Deviation.CompareTo(b.Deviation);
                if (result != 0) return result;
                result = a.CaseCount.CompareTo(b.CaseCount);
                return result != 0 ? result : a.Office.UnitId.CompareTo(b.Office.UnitId);
            }
        }
    }
}
