using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using LexEnaTrs.Web.Models;

// Compile with BillingDistribution.cs, or reference the rebuilt server assembly.
// No service/context instances, database calls, or test framework dependencies.
internal static class DistributionSmokeTests
{
    private static int passed;

    private static int Main()
    {
        try
        {
            Check("empty inputs and zero selected offices", EmptyAndNoOffice);
            Check("one office receives all eligible cases", SingleOffice);
            Check("uneven counts stay within one case", UnevenCounts);
            Check("pair swaps improve a constrained LPT allocation", PairSwapImprovesAmounts);
            Check("the extra case can move between offices to improve amounts", ExtraCaseCanMove);
            Check("excluded cases remain visible without assignments", ExcludedCases);
            Check("only selected offices participate", SelectedOfficesOnly);
            Check("manual totals use eligible assignments and current office names", ManualTotals);
            Check("all-zero amounts remain valid and balanced", ZeroAmounts);
            Check("permuted and repeated inputs give the same assignments", Determinism);
            Check("Wiena IDs and fallback row keys are independent and stable", StableKeys);
            Check("duplicate, missing, negative and null inputs are rejected", InvalidInputs);
            Check("large exact decimals avoid squared-amount overflow", LargeDecimals);
            Check("skewed values and one large outlier preserve all amounts", SkewedAmounts);
            Check("large batches and many offices finish with balanced counts", LargeBatches);
            Check("XML roundtrip preserves the contract and decimal values", XmlRoundTrip);
            Check("equal percentage defaults remain equal without allocating", EqualDefaultsOnly);
            Check("count and amount percentages are independent", IndependentPercentages);
            Check("weighted counts use largest remainders and strict weighted limits", WeightedCountTargets);
            Check("zero count or amount shares remain valid when totals are 100", ZeroShares);
            Check("invalid percentages reject allocation but do not break editing", InvalidPercentages);
            Check("changed percentages recompute the proposal", RecomputeWithNewPercentages);
            Check("fixed cases seed both count and amount balances", FixedCasesSeedTheBalance);
            Check("fixed cases exceeding their share leave feasible optimal count quotas", FixedCasesOverQuota);
            Check("fixed assignments survive swaps and changed proportions", FixedAssignmentsStayPut);
            Check("unavailable or unresolved fixed offices block without destroying the proposal", InvalidFixedAssignments);
            Check("fixed metadata alone never allocates and stale fixed assignments are cleared", FixedPreviewAndRestore);
            Check("excluded fixed cases do not participate and zero shares do not override history", ExcludedFixedAndZeroShare);
            Check("large fixed batches preserve every count and exact amount", LargeFixedBatch);
            Console.WriteLine("PASS: {0} distribution checks; no database connections.", passed);
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("FAIL: " + ex);
            return 1;
        }
    }

    private static void Check(string name, Action action)
    {
        action();
        passed++;
        Console.WriteLine("PASS " + name);
    }

    private static BillingDistribution Fixture(decimal[] amounts, int officeCount)
    {
        return new BillingDistribution
        {
            BatchId = new Guid("3c16b1b2-2828-468f-b708-b6662b959eef"),
            CalculatedAt = new DateTime(2026, 11, 7, 12, 30, 0),
            HasSavedProposal = true,
            Offices = Enumerable.Range(1, officeCount).Select(i => new BillingDistributionOffice
            {
                UnitId = i * 10, WienaFirmaId = i + 1000, Name = "Kancelaria " + i, Selected = true,
                CountPercent = Math.Round(100m / officeCount, 2), AmountPercent = Math.Round(100m / officeCount, 2)
            }).ToArray(),
            Cases = amounts.Select((amount, index) => new BillingDistributionCase
            {
                WienaCaseId = index + 1, BatchRowNumber = index + 1,
                Signature = "POC-" + (index + 1) + "/2026", DebtorName = "Żaneta Łącka & Spółka",
                CustomerNumber = "EW-" + index, Amount = amount, StatusId = 5,
                StatusName = "Poczekalnia", Eligible = true, ExclusionReason = String.Empty
            }).ToArray()
        };
    }

    private static void AllocateAndCheck(BillingDistribution data)
    {
        BillingDistributionCase[] originalCases = data.Cases.ToArray();
        BillingDistributionOffice[] originalOffices = data.Offices.ToArray();
        DateTime calculated = data.CalculatedAt;
        Guid id = data.BatchId;
        BillingDistribution returned = BillingDistributionAllocator.Allocate(data);
        Assert(Object.ReferenceEquals(returned, data), "Allocator replaced the DTO.");
        Assert(originalCases.SequenceEqual(data.Cases), "Case references or input order changed.");
        Assert(originalOffices.SequenceEqual(data.Offices), "Office references or input order changed.");
        Equal(calculated, data.CalculatedAt, "Allocator invented a nondeterministic timestamp.");
        Equal(id, data.BatchId, "Batch ID changed.");
        Assert(!data.HasSavedProposal, "New proposal is incorrectly marked as saved.");
        VerifyAllocation(data);
    }

    private static void VerifyAllocation(BillingDistribution data)
    {
        BillingDistributionOffice[] selected = data.Offices.Where(o => o.Selected).ToArray();
        HashSet<int> selectedIds = new HashSet<int>(selected.Select(o => o.UnitId));
        foreach (BillingDistributionCase item in data.Cases)
        {
            if (item.Eligible)
            {
                Assert(item.AssignedUnitId.HasValue && selectedIds.Contains(item.AssignedUnitId.Value), "Eligible case has no selected office.");
                if (item.FixedUnitId.HasValue) Equal(item.FixedUnitId, item.AssignedUnitId, "Fixed assignment was moved.");
            }
            else
                Assert(!item.AssignedUnitId.HasValue && String.IsNullOrEmpty(item.AssignedOfficeName), "Excluded case retained an assignment.");
        }
        foreach (BillingDistributionOffice office in data.Offices)
        {
            BillingDistributionCase[] assigned = data.Cases.Where(c => c.Eligible && c.AssignedUnitId == office.UnitId).ToArray();
            Equal(assigned.Length, office.CaseCount, "Incorrect office case count.");
            Equal(assigned.Sum(c => c.Amount), office.TotalAmount, "Incorrect exact office total.");
            if (!office.Selected)
            {
                Equal(0, office.CaseCount, "Unselected office received cases.");
                Equal(0m, office.TargetAmount, "Unselected office has a target.");
                Equal(0m, office.AmountDeviation, "Unselected office has a deviation.");
            }
            else
                Equal(office.TotalAmount - office.TargetAmount, office.AmountDeviation, "Incorrect office deviation.");
        }
        Equal(data.Cases.Count(c => c.Eligible), data.Offices.Sum(o => o.CaseCount), "Cases were dropped or duplicated.");
        Equal(data.Cases.Where(c => c.Eligible).Sum(c => c.Amount), data.Offices.Sum(o => o.TotalAmount), "Amounts were lost or rounded.");
        if (selected.Length > 0)
        {
            decimal sum = selected.Sum(o => o.CountPercent);
            bool hasFixed = data.Cases.Any(c => c.Eligible && c.FixedUnitId.HasValue);
            foreach (BillingDistributionOffice office in selected)
            {
                decimal ideal = data.Cases.Count(c => c.Eligible) * office.CountPercent / sum;
                if (!hasFixed) Assert(office.CaseCount >= Math.Floor(ideal) && office.CaseCount <= Math.Ceiling(ideal), "Case count escaped weighted floor/ceil.");
            }
            Equal(data.Cases.Count(c => c.Eligible), selected.Sum(o => o.TargetCaseCount), "Count targets lost a remainder.");
            Equal(data.Cases.Where(c => c.Eligible).Sum(c => c.Amount), selected.Sum(o => o.TargetAmount), "Amount targets lost the normalization remainder.");
            if (!hasFixed && selected.All(o => o.CountPercent == selected[0].CountPercent))
                Assert(selected.Max(o => o.CaseCount) - selected.Min(o => o.CaseCount) <= 1, "Equal-weight case-count difference exceeds one.");
        }
    }

    private static void EmptyAndNoOffice()
    {
        BillingDistribution empty = new BillingDistribution();
        BillingDistributionAllocator.Allocate(empty);
        Equal(0, empty.Cases.Length, "Null cases were not normalized.");
        Equal(0, empty.Offices.Length, "Null offices were not normalized.");
        BillingDistribution noOffice = Fixture(new[] { 12.34m }, 0);
        Reject(() => BillingDistributionAllocator.Allocate(noOffice), "Eligible case accepted without offices.");
        noOffice.Cases[0].AssignedUnitId = 10;
        noOffice.Cases[0].AssignedOfficeName = "Stara kancelaria";
        BillingDistributionAllocator.RefreshTotals(noOffice);
        Assert(!noOffice.Cases[0].AssignedUnitId.HasValue, "Refresh without offices kept an assignment.");
        noOffice.Cases[0].Eligible = false;
        AllocateAndCheck(noOffice);
        AllocateAndCheck(Fixture(new decimal[0], 4));
    }

    private static void SingleOffice()
    {
        BillingDistribution data = Fixture(new[] { 0.01m, 1.99m, 109.1234m, 0m }, 1);
        AllocateAndCheck(data);
        Equal(111.1234m, data.Offices[0].TotalAmount, "Single-office amount was rounded.");
        Equal(0m, data.Offices[0].AmountDeviation, "Single office deviates from itself.");
    }

    private static void UnevenCounts()
    {
        for (int count = 0; count <= 47; count++)
            for (int offices = 1; offices <= 9; offices++)
                AllocateAndCheck(Fixture(Enumerable.Range(0, count).Select(i => (i * 37 % 29) + 0.13m).ToArray(), offices));
    }

    private static void PairSwapImprovesAmounts()
    {
        // Constrained LPT alone gives 17 and 13 (counts 3 and 2). Swapping 8 with 6 gives 15/15.
        BillingDistribution data = Fixture(new[] { 8m, 7m, 6m, 5m, 4m }, 2);
        AllocateAndCheck(data);
        Assert(data.Offices.All(o => o.TotalAmount == 15m), "Pair improvement missed a perfect feasible split.");
    }

    private static void ExcludedCases()
    {
        BillingDistribution data = Fixture(new[] { 1000m, 11.11m, 22.22m, -500m }, 2);
        foreach (int index in new[] { 0, 3 })
        {
            data.Cases[index].Eligible = false;
            data.Cases[index].ExclusionReason = "Umorzenie — zgon";
            data.Cases[index].StatusName = "Umorzona";
            data.Cases[index].AssignedUnitId = 10;
            data.Cases[index].AssignedOfficeName = "Stara kancelaria";
        }
        AllocateAndCheck(data);
        Equal(4, data.Cases.Length, "Excluded rows disappeared.");
        Equal("Umorzenie — zgon", data.Cases[0].ExclusionReason, "Exclusion reason was lost.");
        Equal("Umorzona", data.Cases[0].StatusName, "Status was changed.");
        Equal(33.33m, data.Offices.Sum(o => o.TotalAmount), "Excluded amount included in distribution.");
        Equal(-500m, data.Cases[3].Amount, "Invalid excluded amount should remain visible for inspection.");
    }

    private static void ExtraCaseCanMove()
    {
        BillingDistribution data = Fixture(new[] { 1000m, 1m, 1m }, 2);
        AllocateAndCheck(data);
        Assert(data.Offices.Select(o => o.TotalAmount).OrderBy(a => a).SequenceEqual(new[] { 2m, 1000m }),
            "Fixed remainder office prevented a better balanced split.");
        Equal(1, data.Offices.Single(o => o.TotalAmount == 1000m).CaseCount, "Outlier office retained the extra case.");
    }

    private static void SelectedOfficesOnly()
    {
        BillingDistribution data = Fixture(Enumerable.Repeat(10m, 7).ToArray(), 4);
        data.Offices[0].Selected = false;
        data.Offices[2].Selected = false;
        BillingDistributionAllocator.SetEqualPercentages(data);
        AllocateAndCheck(data);
        foreach (BillingDistributionOffice office in data.Offices) office.Selected = false;
        BillingDistributionAllocator.RefreshTotals(data);
        Assert(data.Offices.All(o => o.CaseCount == 0 && o.TotalAmount == 0m && o.TargetAmount == 0m && o.AmountDeviation == 0m),
            "Deselecting the last office did not clear totals.");
        Assert(data.Cases.All(c => !c.AssignedUnitId.HasValue), "Deselecting the last office retained assignments.");
    }

    private static void ManualTotals()
    {
        BillingDistribution data = Fixture(new[] { 30m, 60m, 90m, 100m, 500m }, 3);
        data.Offices[2].Selected = false;
        data.Offices[0].Name = "Nowa nazwa";
        data.Cases[0].AssignedUnitId = 10;
        data.Cases[1].AssignedUnitId = 20;
        data.Cases[2].AssignedUnitId = 30;
        data.Cases[3].AssignedUnitId = 999;
        data.Cases[4].Eligible = false;
        data.Cases[4].AssignedUnitId = 10;
        BillingDistributionAllocator.RefreshTotals(data);
        Equal(30m, data.Offices[0].TotalAmount, "Manual assignment total incorrect.");
        Equal(60m, data.Offices[1].TotalAmount, "Second manual assignment total incorrect.");
        Equal(140m, data.Offices[0].TargetAmount, "Target should include all eligible amounts, even unassigned ones.");
        Equal("Nowa nazwa", data.Cases[0].AssignedOfficeName, "Office display name was not refreshed.");
        Assert(data.Cases.Skip(2).All(c => !c.AssignedUnitId.HasValue), "Invalid or excluded manual assignment retained.");
    }

    private static void ZeroAmounts()
    {
        BillingDistribution data = Fixture(Enumerable.Repeat(0m, 101).ToArray(), 7);
        AllocateAndCheck(data);
        Assert(data.Offices.All(o => o.TargetAmount == 0m && o.AmountDeviation == 0m), "Zero values produced a nonzero target.");
    }

    private static void Determinism()
    {
        decimal[] amounts = Enumerable.Range(0, 301).Select(i => (i * 7919 % 137) + 0.17m).ToArray();
        BillingDistribution first = Fixture(amounts, 7);
        BillingDistribution second = Fixture(amounts, 7);
        second.Cases = second.Cases.Reverse().ToArray();
        second.Offices = second.Offices.Reverse().ToArray();
        AllocateAndCheck(first);
        AllocateAndCheck(second);
        Equal(AssignmentMap(first), AssignmentMap(second), "Input ordering changed assignment.");
        string original = AssignmentMap(first);
        AllocateAndCheck(first);
        Equal(original, AssignmentMap(first), "Repeated allocation changed assignment.");
    }

    private static string AssignmentMap(BillingDistribution data)
    {
        return String.Join(";", data.Cases.OrderBy(c => c.WienaCaseId).ThenBy(c => c.BatchRowNumber)
            .Select(c => c.WienaCaseId + ":" + c.BatchRowNumber + "=" + c.AssignedUnitId).ToArray());
    }

    private static void StableKeys()
    {
        BillingDistribution data = Fixture(new[] { 10m, 10m, 10m, 10m }, 2);
        data.Cases[1].WienaCaseId = 0;
        data.Cases[1].BatchRowNumber = 1;
        data.Cases[3].WienaCaseId = 0;
        data.Cases[3].BatchRowNumber = 999;
        AllocateAndCheck(data);
        string map = AssignmentMap(data);
        data.Cases = data.Cases.Reverse().ToArray();
        AllocateAndCheck(data);
        Equal(map, AssignmentMap(data), "Fallback keys were unstable.");
    }

    private static void InvalidInputs()
    {
        Reject(() => BillingDistributionAllocator.Allocate(null), "Null DTO accepted.");
        BillingDistribution data = Fixture(new[] { 1m, 2m }, 2);
        data.Offices[1].UnitId = data.Offices[0].UnitId;
        Reject(() => BillingDistributionAllocator.Allocate(data), "Duplicate office accepted.");
        data = Fixture(new[] { 1m, 2m }, 2);
        data.Cases[1].WienaCaseId = data.Cases[0].WienaCaseId;
        Reject(() => BillingDistributionAllocator.Allocate(data), "Duplicate Wiena case accepted.");
        data = Fixture(new[] { 1m, 2m }, 2);
        data.Cases[0].WienaCaseId = 0;
        data.Cases[1].WienaCaseId = 0;
        data.Cases[1].BatchRowNumber = data.Cases[0].BatchRowNumber;
        Reject(() => BillingDistributionAllocator.Allocate(data), "Duplicate fallback row accepted.");
        data = Fixture(new[] { 1m }, 1);
        data.Cases[0].WienaCaseId = 0;
        data.Cases[0].BatchRowNumber = 0;
        Reject(() => BillingDistributionAllocator.Allocate(data), "Missing stable key accepted.");
        data = Fixture(new[] { -0.01m }, 1);
        Reject(() => BillingDistributionAllocator.Allocate(data), "Negative amount accepted.");
        data = Fixture(new[] { 1m }, 1);
        data.Cases[0] = null;
        Reject(() => BillingDistributionAllocator.RefreshTotals(data), "Null case accepted.");
        data = Fixture(new[] { 1m }, 1);
        data.Offices[0] = null;
        Reject(() => BillingDistributionAllocator.RefreshTotals(data), "Null office accepted.");
    }

    private static void LargeDecimals()
    {
        BillingDistribution data = Fixture(new[] { Decimal.MaxValue / 4m, Decimal.MaxValue / 4m, Decimal.MaxValue / 8m }, 2);
        AllocateAndCheck(data);
        data = Fixture(new[] { Decimal.MaxValue, 1m }, 2);
        data.Cases[0].AssignedUnitId = 123;
        Reject(() => BillingDistributionAllocator.Allocate(data), "Overflowing total accepted.");
        Equal((int?)123, data.Cases[0].AssignedUnitId, "Overflow damaged the original proposal.");
    }

    private static void SkewedAmounts()
    {
        decimal[] amounts = new[] { 1000000.01m, 0m, 0.01m, 999.99m, 0.02m, 17.17m, 0m, 0.03m, 3.33m, 0.04m, 0m };
        BillingDistribution data = Fixture(amounts, 4);
        AllocateAndCheck(data);
        Assert(data.Offices.Count(o => o.TotalAmount >= 1000000m) == 1, "Outlier was split or duplicated.");
    }

    private static void LargeBatches()
    {
        Stopwatch timer = Stopwatch.StartNew();
        decimal[] amounts = Enumerable.Range(0, 25003).Select(i => (i * 37 % 100003) / 100m).ToArray();
        amounts[0] = 10000000.01m;
        AllocateAndCheck(Fixture(amounts, 17));
        AllocateAndCheck(Fixture(Enumerable.Repeat(1m, 2000).ToArray(), 2000));
        Console.WriteLine("  Large-batch elapsed: {0} ms", timer.ElapsedMilliseconds);
    }

    private static void XmlRoundTrip()
    {
        CultureInfo previous = Thread.CurrentThread.CurrentCulture;
        try
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pl-PL");
            BillingDistribution data = Fixture(new[] { 12.34m, 56.78m, 0.01m }, 2);
            data.Cases[0].FixedUnitId = 10;
            data.Cases[0].PreviousWienaCaseId = 501;
            data.Cases[0].PreviousWienaFirmaId = 1001;
            data.Cases[0].PreviousSignature = "Ł-123/2025";
            data.Cases[0].PreviousOfficeName = "Kancelaria Łódź";
            data.Cases[0].AssignmentReason = "Zgodny numer ewidencyjny";
            data.Cases[2].Eligible = false;
            data.Cases[2].StatusId = null;
            data.Cases[2].ExclusionReason = "Zgon — łódź";
            AllocateAndCheck(data);
            XmlSerializer serializer = new XmlSerializer(typeof(BillingDistribution));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, data);
            BillingDistribution restored = (BillingDistribution)serializer.Deserialize(new StringReader(writer.ToString()));
            VerifyAllocation(restored);
            Equal(AssignmentMap(data), AssignmentMap(restored), "XML changed stable assignments.");
            Equal(data.Cases[0].DebtorName, restored.Cases[0].DebtorName, "XML changed Unicode data.");
            Equal(data.Cases[2].ExclusionReason, restored.Cases[2].ExclusionReason, "XML lost exclusion information.");
            Assert(!restored.Cases[2].StatusId.HasValue, "XML changed nullable status.");
            Equal(data.Cases[0].FixedUnitId, restored.Cases[0].FixedUnitId, "XML lost the fixed office.");
            Equal(data.Cases[0].PreviousSignature, restored.Cases[0].PreviousSignature, "XML lost the historic signature.");
            Equal(data.Cases[0].PreviousWienaFirmaId, restored.Cases[0].PreviousWienaFirmaId, "XML lost the historic firm.");
            Equal(data.Cases[0].PreviousWienaCaseId, restored.Cases[0].PreviousWienaCaseId, "XML lost the historic case.");
            Equal(data.Cases[0].PreviousOfficeName, restored.Cases[0].PreviousOfficeName, "XML lost the historic office name.");
            Equal(data.Cases[0].AssignmentReason, restored.Cases[0].AssignmentReason, "XML lost the matching reason.");
        }
        finally { Thread.CurrentThread.CurrentCulture = previous; }
    }

    private static void EqualDefaultsOnly()
    {
        BillingDistribution data = Fixture(Enumerable.Repeat(10m, 10).ToArray(), 3);
        data.Cases[0].AssignedUnitId = 20;
        BillingDistributionAllocator.SetEqualPercentages(data);
        Assert(data.Offices.All(o => o.CountPercent == 33.33m && o.AmountPercent == 33.33m), "Defaults were not equally rounded to two decimals.");
        Equal((int?)20, data.Cases[0].AssignedUnitId, "Setting defaults modified an existing assignment.");
        Assert(data.Cases.Skip(1).All(c => !c.AssignedUnitId.HasValue), "Setting defaults allocated cases.");
        BillingDistributionAllocator.ValidatePercentages(data);
        BillingDistributionAllocator.RefreshTotals(data);
        Equal(1, data.Offices.Sum(o => o.CaseCount), "Refreshing targets allocated cases.");
        Assert(data.Offices.Select(o => o.TargetCaseCount).SequenceEqual(new[] { 4, 3, 3 }), "Largest remainder default count targets changed.");
        AllocateAndCheck(data);
        data = Fixture(new[] { 1m }, 6);
        BillingDistributionAllocator.SetEqualPercentages(data);
        Assert(data.Offices.All(o => o.CountPercent == 16.67m), "Six-way defaults were not equally rounded.");
        AllocateAndCheck(data);
    }

    private static void IndependentPercentages()
    {
        BillingDistribution data = Fixture(new[] { 70m, 15m, 10m, 5m }, 2);
        data.Offices[0].AmountPercent = 80m;
        data.Offices[1].AmountPercent = 20m;
        AllocateAndCheck(data);
        Equal(2, data.Offices[0].CaseCount, "Value share changed the count quota.");
        Equal(2, data.Offices[1].CaseCount, "Value share changed the second count quota.");
        Equal(80m, data.Offices[0].TargetAmount, "Wrong weighted value target.");
        Equal(80m, data.Offices[0].TotalAmount, "A feasible independent value target was missed.");
        Equal(20m, data.Offices[1].TotalAmount, "Independent value target was ignored.");
        data = Fixture(new[] { 100m }, 2);
        data.Offices[0].AmountPercent = 0m;
        data.Offices[1].AmountPercent = 100m;
        AllocateAndCheck(data);
        Equal(1, data.Offices[1].CaseCount, "Remainder could not move to an initially empty office with the value target.");
    }

    private static void WeightedCountTargets()
    {
        BillingDistribution data = Fixture(Enumerable.Repeat(1m, 7).ToArray(), 3);
        data.Offices[0].CountPercent = 50m;
        data.Offices[1].CountPercent = 30m;
        data.Offices[2].CountPercent = 20m;
        AllocateAndCheck(data);
        Assert(data.Offices.Select(o => o.TargetCaseCount).SequenceEqual(new[] { 4, 2, 1 }), "Largest-remainder targets are incorrect.");
        Assert(data.Offices.Select(o => o.CaseCount).SequenceEqual(new[] { 4, 2, 1 }), "Amount improvement worsened a better count apportionment.");
        data = Fixture(Enumerable.Range(1, 10).Select(i => (decimal)i).ToArray(), 2);
        data.Offices[0].CountPercent = 70m;
        data.Offices[1].CountPercent = 30m;
        AllocateAndCheck(data);
        Equal(7, data.Offices[0].CaseCount, "Exact integer count quota was relaxed.");
        Equal(3, data.Offices[1].CaseCount, "Exact integer count quota was relaxed.");
    }

    private static void ZeroShares()
    {
        BillingDistribution data = Fixture(new[] { 100m, 10m, 1m }, 3);
        data.Offices[0].CountPercent = 100m;
        data.Offices[1].CountPercent = 0m;
        data.Offices[2].CountPercent = 0m;
        data.Offices[0].AmountPercent = 0m;
        data.Offices[1].AmountPercent = 100m;
        data.Offices[2].AmountPercent = 0m;
        AllocateAndCheck(data);
        Equal(3, data.Offices[0].CaseCount, "Conflicting value targets changed the required case counts.");
        Equal(0m, data.Offices[0].TargetAmount, "Zero amount target changed.");
        Equal(111m, data.Offices[1].TargetAmount, "Positive value target disappeared because count share was zero.");
        data.Offices[2].Selected = false;
        data.Offices[2].CountPercent = -999m;
        data.Offices[2].AmountPercent = 999m;
        BillingDistributionAllocator.ValidatePercentages(data);
    }

    private static void InvalidPercentages()
    {
        BillingDistribution data = Fixture(new[] { 10m, 20m }, 2);
        data.Cases[0].AssignedUnitId = 10;
        data.Offices[0].CountPercent = 40m;
        Reject(() => BillingDistributionAllocator.Allocate(data), "A count total of 90 was accepted.");
        Equal((int?)10, data.Cases[0].AssignedUnitId, "Invalid weights damaged the previous proposal.");
        BillingDistributionAllocator.RefreshTotals(data);
        data.Offices[0].CountPercent = 50m;
        data.Offices[1].AmountPercent = 49.99m;
        Reject(() => BillingDistributionAllocator.ValidatePercentages(data), "A non-equal value total of 99.99 was accepted.");
        BillingDistributionAllocator.RefreshTotals(data);
        data.Offices[0].AmountPercent = 0m;
        data.Offices[1].AmountPercent = 0m;
        Reject(() => BillingDistributionAllocator.Allocate(data), "Zero total value weights were accepted.");
        BillingDistributionAllocator.RefreshTotals(data);
        Assert(data.Offices.All(o => o.TargetAmount == 0m), "Zero edited weights produced nonzero targets.");
        data.Offices[0].AmountPercent = -1m;
        data.Offices[1].AmountPercent = 101m;
        Reject(() => BillingDistributionAllocator.ValidatePercentages(data), "Negative/out-of-range value weights were accepted.");
        BillingDistributionAllocator.RefreshTotals(data);
    }

    private static void RecomputeWithNewPercentages()
    {
        BillingDistribution data = Fixture(new[] { 70m, 15m, 10m, 5m }, 2);
        data.Offices[0].AmountPercent = 80m;
        data.Offices[1].AmountPercent = 20m;
        AllocateAndCheck(data);
        data.HasSavedProposal = true;
        data.Offices[0].AmountPercent = 20m;
        data.Offices[1].AmountPercent = 80m;
        AllocateAndCheck(data);
        Equal(20m, data.Offices[0].TotalAmount, "Recomputation retained the previous proposal.");
        Equal(80m, data.Offices[1].TotalAmount, "Edited value proportions were not applied.");
        data.Offices[0].CountPercent = 75m;
        data.Offices[1].CountPercent = 25m;
        AllocateAndCheck(data);
        Equal(3, data.Offices[0].CaseCount, "Edited count proportions were not applied.");
        Equal(1, data.Offices[1].CaseCount, "Edited count proportions were not applied.");
    }

    private static void FixedCasesSeedTheBalance()
    {
        BillingDistribution data = Fixture(new[] { 90m, 10m, 60m, 40m }, 2);
        data.Cases[0].FixedUnitId = 10;
        AllocateAndCheck(data);
        Equal(2, data.Offices[0].CaseCount, "Fixed case did not consume one count slot.");
        Equal(100m, data.Offices[0].TotalAmount, "Movable cases ignored the fixed office's starting amount.");
        Equal(100m, data.Offices[1].TotalAmount, "The fixed amount was excluded from the common balance.");
        Assert(data.Offices.All(o => o.TargetAmount == 100m), "Targets excluded fixed case values.");
    }

    private static void FixedCasesOverQuota()
    {
        BillingDistribution data = Fixture(Enumerable.Range(1, 10).Select(i => (decimal)i).ToArray(), 3);
        data.Offices[0].CountPercent = 20m;
        data.Offices[1].CountPercent = 40m;
        data.Offices[2].CountPercent = 40m;
        foreach (BillingDistributionCase item in data.Cases.Take(7)) item.FixedUnitId = 10;
        AllocateAndCheck(data);
        Equal(7, data.Offices[0].CaseCount, "Fixed cases were dropped or the overfull office received another case.");
        Assert(data.Offices.Skip(1).Select(o => o.CaseCount).OrderBy(c => c).SequenceEqual(new[] { 1, 2 }),
            "The remaining cases were not optimally balanced under fixed lower bounds.");
        Assert(data.Offices.Select(o => o.TargetCaseCount).SequenceEqual(new[] { 2, 4, 4 }),
            "Requested percentage targets were replaced with the unavoidable actual counts.");

        // Exhaustively compare small feasible count vectors; this checks the count
        // objective independently from the heap and the assignment implementation.
        for (int fixedFirst = 0; fixedFirst <= 6; fixedFirst++)
            for (int fixedSecond = 0; fixedSecond <= 6 - fixedFirst; fixedSecond++)
            {
                data = Fixture(Enumerable.Range(1, 8).Select(i => (decimal)i).ToArray(), 3);
                data.Offices[0].CountPercent = 50m;
                data.Offices[1].CountPercent = 30m;
                data.Offices[2].CountPercent = 20m;
                foreach (var item in data.Cases.Take(fixedFirst)) item.FixedUnitId = 10;
                foreach (var item in data.Cases.Skip(fixedFirst).Take(fixedSecond)) item.FixedUnitId = 20;
                AllocateAndCheck(data);
                decimal best = Decimal.MaxValue;
                for (int a = fixedFirst; a <= 8; a++)
                    for (int b = fixedSecond; b <= 8 - a; b++)
                        best = Math.Min(best, CountPenalty(new[] { a, b, 8 - a - b }, new[] { 4m, 2.4m, 1.6m }));
                Equal(best, CountPenalty(data.Offices.Select(o => o.CaseCount).ToArray(), new[] { 4m, 2.4m, 1.6m }),
                    "Fixed case lower bounds produced a suboptimal count allocation.");
            }
    }

    private static decimal CountPenalty(int[] actual, decimal[] ideal)
    {
        return actual.Select((count, i) => (count - ideal[i]) * (count - ideal[i])).Sum();
    }

    private static void FixedAssignmentsStayPut()
    {
        BillingDistribution data = Fixture(Enumerable.Range(1, 151).Select(i => (i * 997 % 333) + 0.11m).ToArray(), 4);
        foreach (var item in data.Cases.Where(c => c.WienaCaseId % 5 == 0)) item.FixedUnitId = 10;
        foreach (var item in data.Cases.Where(c => c.WienaCaseId % 5 == 1)) item.FixedUnitId = 40;
        AllocateAndCheck(data);
        var expected = data.Cases.Where(c => c.FixedUnitId.HasValue).ToDictionary(c => c.WienaCaseId, c => c.AssignedUnitId);
        data.Offices[0].CountPercent = 0m;
        data.Offices[1].CountPercent = 50m;
        data.Offices[2].CountPercent = 50m;
        data.Offices[3].CountPercent = 0m;
        data.Offices[0].AmountPercent = 0m;
        data.Offices[1].AmountPercent = 80m;
        data.Offices[2].AmountPercent = 20m;
        data.Offices[3].AmountPercent = 0m;
        data.Cases = data.Cases.Reverse().ToArray();
        data.Offices = data.Offices.Reverse().ToArray();
        AllocateAndCheck(data);
        foreach (var item in data.Cases.Where(c => c.FixedUnitId.HasValue))
            Equal(expected[item.WienaCaseId], item.AssignedUnitId, "Rebalancing or new proportions moved a fixed case.");
        string map = AssignmentMap(data);
        AllocateAndCheck(data);
        Equal(map, AssignmentMap(data), "Fixed allocation is not deterministic.");
    }

    private static void InvalidFixedAssignments()
    {
        BillingDistribution data = Fixture(new[] { 10m, 20m }, 2);
        data.Cases[0].AssignedUnitId = 20;
        data.Cases[0].FixedUnitId = 999;
        Reject(() => BillingDistributionAllocator.Allocate(data), "Missing fixed office was silently replaced.");
        Equal((int?)20, data.Cases[0].AssignedUnitId, "Rejected allocation damaged the previous proposal.");
        data.Cases[0].FixedUnitId = 10;
        data.Offices[0].Selected = false;
        BillingDistributionAllocator.SetEqualPercentages(data);
        Reject(() => BillingDistributionAllocator.Allocate(data), "Deselected fixed office was silently replaced.");
        data.Offices[0].Selected = true;
        BillingDistributionAllocator.SetEqualPercentages(data);
        data.Cases[0].FixedUnitId = null;
        data.Cases[0].PreviousWienaCaseId = 501;
        Reject(() => BillingDistributionAllocator.Allocate(data), "Unmapped history was treated as a freely assignable case.");
        data.Cases[0].PreviousWienaCaseId = null;
        data.Cases[0].AssignmentBlockReason = "Brak aktywnej kancelarii.";
        Reject(() => BillingDistributionAllocator.Allocate(data), "An unresolved matching problem was ignored.");
        data.Cases[0].AssignmentBlockReason = null;
        data.Cases[0].FixedUnitId = 0;
        Reject(() => BillingDistributionAllocator.Allocate(data), "Invalid fixed office ID was accepted.");
    }

    private static void FixedPreviewAndRestore()
    {
        BillingDistribution data = Fixture(new[] { 10m, 20m }, 2);
        data.Cases[0].FixedUnitId = 10;
        BillingDistributionAllocator.SetEqualPercentages(data);
        BillingDistributionAllocator.RefreshTotals(data);
        Assert(data.Cases.All(c => !c.AssignedUnitId.HasValue), "Preview auto-assigned fixed cases before the proposal button.");
        Equal(0, data.Offices.Sum(o => o.CaseCount), "Preview invented an assigned count.");
        data.Cases[0].AssignedUnitId = 20;
        BillingDistributionAllocator.RefreshTotals(data);
        Assert(!data.Cases[0].AssignedUnitId.HasValue, "A saved proposal overrode the currently required office.");
        data.Cases[0].AssignedUnitId = 10;
        BillingDistributionAllocator.RefreshTotals(data);
        Equal(1, data.Offices[0].CaseCount, "Valid fixed assignment was dropped from the count.");
        Equal(10m, data.Offices[0].TotalAmount, "Valid fixed assignment was dropped from the amount.");
        data.Cases[0].AssignmentBlockReason = "Kancelaria wymaga konfiguracji.";
        BillingDistributionAllocator.RefreshTotals(data);
        Assert(!data.Cases[0].AssignedUnitId.HasValue, "A blocked assignment stayed active in totals.");
    }

    private static void ExcludedFixedAndZeroShare()
    {
        BillingDistribution data = Fixture(new[] { 90m, 10m, 500m }, 2);
        data.Cases[0].FixedUnitId = 10;
        data.Cases[2].FixedUnitId = 999;
        data.Cases[2].AssignmentBlockReason = "Brak kancelarii.";
        data.Cases[2].Eligible = false;
        data.Offices[0].CountPercent = 0m;
        data.Offices[0].AmountPercent = 0m;
        data.Offices[1].CountPercent = 100m;
        data.Offices[1].AmountPercent = 100m;
        AllocateAndCheck(data);
        Equal(1, data.Offices[0].CaseCount, "Zero requested count overrode a historic assignment.");
        Equal(90m, data.Offices[0].TotalAmount, "Zero requested value removed a fixed case from the balance.");
        Equal(1, data.Offices[1].CaseCount, "The remaining free case was not assigned.");
        Assert(!data.Cases[2].AssignedUnitId.HasValue, "Excluded fixed case received an assignment.");
    }

    private static void LargeFixedBatch()
    {
        Stopwatch timer = Stopwatch.StartNew();
        BillingDistribution data = Fixture(Enumerable.Range(0, 25003).Select(i => (i * 37 % 100003) / 100m).ToArray(), 17);
        foreach (var item in data.Cases.Take(15000)) item.FixedUnitId = 10;
        AllocateAndCheck(data);
        Equal(15000, data.Offices[0].CaseCount, "Large overloaded office received free cases or lost fixed cases.");
        Assert(data.Offices.Skip(1).Max(o => o.CaseCount) - data.Offices.Skip(1).Min(o => o.CaseCount) <= 1,
            "Free cases were not balanced around a large fixed workload.");
        Console.WriteLine("  Large fixed-batch elapsed: {0} ms", timer.ElapsedMilliseconds);
    }

    private static void Reject(Action action, string message)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        catch (ArgumentException) { return; }
        throw new Exception(message);
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }

    private static void Equal<T>(T expected, T actual, string message)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new Exception(message + " Expected: " + expected + "; actual: " + actual);
    }
}
