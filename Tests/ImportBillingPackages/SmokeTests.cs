using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using LexEnaTrs.Web;
using LexEnaTrs.Web.Models;

// No database access or service instances. The translation check uses only embedded EDMX metadata.
internal static class SmokeTests
{
    private static Type serviceType;
    private static int passed;

    private static int Main(string[] args)
    {
        try
        {
            if (args.Length != 2) throw new ArgumentException("Expected Web bin and repository paths.");
            string bin = Path.GetFullPath(args[0]);
            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
            {
                string candidate = Path.Combine(bin, new AssemblyName(e.Name).Name + ".dll");
                return File.Exists(candidate) ? Assembly.LoadFrom(candidate) : null;
            };
            Assembly web = Assembly.LoadFrom(Path.Combine(bin, "LexEnaTrs.Web.dll"));
            serviceType = web.GetType("LexEnaTrs.Web.LexEnaMeritumDomainService", true);
            Run(args[1]);
            Console.WriteLine("PASS: {0} smoke checks; no database connections.", passed);
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("FAIL: " + ex);
            return 1;
        }
    }

    private static void Run(string repository)
    {
        Check("client may change only selection and interest", ClientChangesAreRestricted);
        Check("duplicate rejection cannot be bypassed", DuplicateCannotBeEnabled);
        Check("missing, duplicate and unknown row keys are rejected", InvalidRowListsAreRejected);
        Check("closed and in-progress batches cannot be edited", NonDraftBatchesAreReadOnly);
        Check("an unsaved draft may be deleted without changing its data", UnsavedBatchDeletion);
        Check("deletion rejects started batches even when the imported count is zero", StartedBatchDeletionIsRejected);
        Check("deletion checks saved case markers independently of the batch counter", SavedCaseDeletionIsRejected);
        Check("deletion rejects missing or corrupt import snapshots", InvalidSnapshotDeletionIsRejected);
        Check("invalid interest choice is rejected", InvalidInterestIsRejected);
        Check("merged charges occur once and source remains unchanged", MergePreservesSource);
        Check("case totals count cases rather than billing rows", CountsAndAmounts);
        Check("Polish text, decimals and identifiers survive XML/base64", SerializationRoundTrip);
        Check("selection serialization skips invalid recipient data and preserves trusted snapshot", SelectionSkipsRecipientGraph);
        Check("minimal selections still reject missing, duplicate and unknown row keys", MinimalSelectionKeysAreValidated);
        Check("distribution uses the newest active Wiena status, not any historic waiting status", CurrentDistributionStatus);
        Check("new waiting cases belong to the owning branch independently of the region", DistributionUsesOwningCompany);
        Check("distribution still excludes other companies, deleted cases and non-waiting statuses", DistributionEligibilityGuards);
        Check("history identifiers preserve leading zeroes and reject malformed PESEL/NIP", HistoryIdentifierNormalization);
        Check("history query scopes by owning company and keeps independent customer numbers", HistoryQueryScope);
        Check("history query and normalized key filters translate to SQL without opening a connection", HistoryQuerySqlTranslation);
        Check("history matches each debtor key and selects the latest previous office", HistoryLatestAssignment);
        Check("history ignores invalid identities, later cases and cases from the current batch", HistoryFalseMatchesAreIgnored);
        Check("history distinguishes absent case data from a case with no match keys", HistoryMissingCurrentCaseIsBlocked);
        Check("history combines debtor links and blocks an unavailable latest office", HistoryMultipleDebtorsAndUnavailableOffice);
        Check("distribution groups imported cases and retains failed import rows", DistributionCaseGrouping);
        Check("saved distribution restores only current eligible cases and unchanged office mappings", RestoreCurrentDistribution);
        Check("saved distributions reject a different batch and ignore ambiguous records", RestoreInvalidDistribution);
        Check("initial distribution has equal targets and no case assignments", InitialDistributionSettings);
        Check("distribution request applies only selected office weights by identifier", DistributionRequestWeights);
        Check("distribution request rejects malformed arrays, unknown offices and invalid percentages", InvalidDistributionRequests);
        Check("saved distribution restores separate count and amount targets", RestoreDistributionPercentages);
        Check("saved distribution cannot override a fresh historical debtor assignment", RestoreFixedDistribution);
        Check("legacy saved distribution initializes equal percentages without allocating", RestoreLegacyDistributionPercentages);
        Check("decree signatures use the agreed year and enforce Wiena's length and numeric limits", DistributionSignatureValidation);
        Check("office symbols resolve within the batch branch and reject missing or ambiguous mappings", DistributionSymbolResolution);
        Check("symbol choices come from active owning branches and include unused dictionary symbols", DistributionSymbolChoiceScope);
        Check("symbol choices merge equivalent designations and identify their branches", DistributionSymbolChoiceGrouping);
        Check("symbol choice queries translate to SQL without cases, yearly counters or open connections", DistributionSymbolChoiceSqlTranslation);
        Check("approval preserves the exact saved allocation including fixed cases and exclusions", DistributionApprovalUnchangedProposal);
        Check("approval rejects stale case eligibility, amount, signature and debtor history", DistributionApprovalChangedCases);
        Check("approval rejects changed offices, symbols and invalid assignments", DistributionApprovalChangedOffices);
        Check("approval is available only for a complete current proposal with valid symbols", DistributionApprovalAvailability);
        Check("approval rejects a stale client revision and keeps locked results stable", DistributionApprovalRevisionAndLockedResult);
        Check("decree refuses cases moved or assigned outside the batch waiting room", DistributionApprovalSourceCaseGuards);
        Check("numbering includes deleted cases and separates branch, symbol and year", DistributionNumberingScope);
        Check("decree updates the existing case and preserves its accounts and registration", DistributionCaseMutation);
        Check("offices sharing a symbol share consecutive numbers and other symbols have independent counters", DistributionCaseCounters);
        Check("invalid decree numbers leave both case and receipt unchanged", DistributionCaseMutationFailure);
        Check("signature preview continues per-symbol numbering without changing cases or reserving counters", DistributionSignaturePreviewNumbers);
        Check("signature preview follows manual reassignment and refreshed counters", DistributionSignaturePreviewRefresh);
        Check("signature preview reports invalid destinations and numbering limits independently", DistributionSignaturePreviewErrors);
        Check("signature columns recover XML registration and legacy committed results", DistributionSignatureColumnsFromImport);
        Check("decree preserves the signature originally registered from XML", DistributionOriginalSignaturePreserved);
        Check("approval receipt fields survive XML without losing original signatures", DistributionApprovalSerialization);
        Check("manual reassignment changes only the chosen case and updates count and amount balances", ManualAssignmentBalances);
        Check("explicit manual reassignment can override a historical office while retaining the history", ManualAssignmentHistoryOverride);
        Check("manual reassignment rejects invalid requests without changing the proposal", ManualAssignmentGuards);
        Check("manual reassignment can add an unselected configured office without invalid percentage totals", ManualAssignmentAdditionalOffice);
        Check("manual choices still require current case history and unchanged target office mappings", ManualAssignmentSnapshotValidation);
        Check("manual choices survive XML and reopening only while the reviewed history still matches", ManualAssignmentRestore);
        Check("recalculation removes manual choices and restores automatic debtor-history rules", ManualAssignmentRecalculation);
        Check("SQL and EDMX retain matching required batch fields", delegate { SchemaMatches(repository); });
    }

    private static void Check(string name, Action test)
    {
        test();
        passed++;
        Console.WriteLine("PASS " + name);
    }

    private static object Call(string name, params object[] args)
    {
        MethodInfo method = serviceType.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
        if (method == null) throw new InvalidOperationException("Missing helper " + name + "; rebuild the Web project.");
        try { return method.Invoke(null, args); }
        catch (TargetInvocationException ex) { throw ex.InnerException ?? ex; }
    }

    private static string Write(ImportSprawBilling import)
    {
        return (string)Call("WriteBatchXml", import);
    }

    private static ImportSprawBilling Read(string xml)
    {
        return (ImportSprawBilling)Call("ReadBatchXml", xml);
    }

    private static ImportSprawBilling Copy(ImportSprawBilling import)
    {
        return Read(Write(import));
    }

    private static string Payload(ImportSprawBilling import)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(Write(import)));
    }

    private static BillingImportBatch Batch(ImportSprawBilling import)
    {
        return new BillingImportBatch { Id = Guid.NewGuid(), Status = "Draft", ImportXml = Write(import) };
    }

    private static ImportSprawBilling Apply(BillingImportBatch batch, ImportSprawBilling choices)
    {
        return (ImportSprawBilling)Call("ApplyBatchChoices", batch, Payload(choices));
    }

    private static ImportSprawBilling Fixture()
    {
        return new ImportSprawBilling
        {
            Oddzial = "Łódź & Żary", System = "Źródło <XML>", IdOddzial = 7, IdKancelaria = 0,
            NrDodowu = "PK", DataKsiegowania = new DateTime(2026, 11, 6),
            SprawaDescriptor = new[] { Row(1, 101, 101.23m), Row(2, 101, 202.34m), Row(3, 102, 303.45m) }
        };
    }

    private static SprImportDescriptor Row(int key, int number, decimal amount)
    {
        return new SprImportDescriptor
        {
            BatchRowNumber = key, CanImport = true, czyrej = true, CzyUstawowe = 0,
            nr = number, rok = 2026, id_symbol = 12, sygn_obciaz = "POC-" + number + "/2026",
            BatchAmount = amount, WienaCaseId = 0, message = "Zażółć gęślą jaźń",
            Odbiorca = new Odbiorca
            {
                Nr_ewidencyjny = "EW-" + key, Nazwa = "Żaneta Łącka & Spółka", PESEL = "80010100000",
                Adres = new Adres(), Adres_korespondencyjny = new Adres_korespondencyjny(),
                Obciazenia = new[]
                {
                    new Obciazenie
                    {
                        Nr_dokumentu = "FV/" + key, Kwota_do_zaplaty = amount,
                        Kwota_obciazenia = amount, Id_sprawy = 700 + key,
                        Wplaty = new[] { new LexEnaTrs.Web.Models.Wplata { Kwota_wplaty = 10m, Data_wplaty = new DateTime(2026, 11, 1) } }
                    }
                }
            }
        };
    }

    private static void ClientChangesAreRestricted()
    {
        ImportSprawBilling source = Fixture();
        source.SprawaDescriptor[0].WienaCaseId = 1234;
        BillingImportBatch batch = Batch(source);
        string persistedXml = batch.ImportXml;
        ImportSprawBilling edited = Copy(source);
        edited.IdOddzial = 900;
        edited.IdKancelaria = 901;
        edited.Oddzial = "inny";
        edited.NrDodowu = "inny";
        edited.DataKsiegowania = new DateTime(2030, 1, 1);
        SprImportDescriptor row = edited.SprawaDescriptor[0];
        row.czyrej = false;
        row.CzyUstawowe = 8;
        row.BatchAmount = 1m;
        row.WienaCaseId = 9999;
        row.czy_ok = true;
        row.nr = 999;
        row.rok = 2030;
        row.id_symbol = 99;
        row.sygn_obciaz = "FAKE-999/2030";
        row.message = "FAKE";
        row.Odbiorca.Nazwa = "Podmieniony dłużnik";
        row.Odbiorca.PESEL = "99999999999";
        row.Odbiorca.Obciazenia[0].Kwota_do_zaplaty = 0.01m;

        ImportSprawBilling actual = Apply(batch, edited);
        ImportSprawBilling expected = Copy(source);
        expected.SprawaDescriptor[0].czyrej = false;
        expected.SprawaDescriptor[0].CzyUstawowe = 8;
        Equal(Write(expected), Write(actual), "Untrusted client fields changed persisted values.");
        Equal(persistedXml, batch.ImportXml, "Applying choices mutated the stored XML in place.");
    }

    private static void UnsavedBatchDeletion()
    {
        ImportSprawBilling source = Fixture();
        source.SprawaDescriptor[0].CanImport = false;
        source.SprawaDescriptor[0].czyrej = false;
        BillingImportBatch batch = Batch(source);
        string originalXml = batch.ImportXml;
        Call("EnsureBillingBatchCanBeDeleted", batch);
        Equal(originalXml, batch.ImportXml, "Deletion validation changed the trusted snapshot.");
        Equal("Draft", batch.Status, "Deletion validation changed the batch status.");
        Equal(0, batch.ImportedCount, "Deletion validation changed the imported counter.");
    }

    private static void StartedBatchDeletionIsRejected()
    {
        foreach (string status in new[] { "Processing", "Completed", "Partial", "ReviewRequired", "Unknown", String.Empty })
        {
            BillingImportBatch batch = Batch(Fixture());
            batch.Status = status;
            batch.ImportedCount = 0;
            Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", batch); }, "Allowed deletion of a non-draft state: " + status);
        }
        Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", new BillingImportBatch { ImportXml = Write(Fixture()) }); }, "Allowed deletion without a status.");
        foreach (int count in new[] { 1, -1 })
        {
            BillingImportBatch batch = Batch(Fixture());
            batch.ImportedCount = count;
            Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", batch); }, "Allowed deletion with an inconsistent imported counter.");
        }
        Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", (object)null); }, "Allowed deletion without a batch.");
    }

    private static void SavedCaseDeletionIsRejected()
    {
        ImportSprawBilling source = Fixture();
        source.SprawaDescriptor[1].czy_ok = true;
        Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", Batch(source)); }, "A success marker was ignored when ImportedCount was zero.");
        source.SprawaDescriptor[1].czy_ok = false;
        source.SprawaDescriptor[1].WienaCaseId = 701;
        Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", Batch(source)); }, "A stored Wiena case ID was ignored when the success flag was false.");
        source.SprawaDescriptor[1].WienaCaseId = -1;
        Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", Batch(source)); }, "A corrupt nonzero case ID was ignored.");
    }

    private static void InvalidSnapshotDeletionIsRejected()
    {
        // EF rejects assigning null to these required properties; an unset entity tests the guard.
        Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", new BillingImportBatch { Status = "Draft" }); }, "Deletion accepted a missing snapshot.");
        foreach (string xml in new[] { String.Empty, " ", "<broken", "<Unexpected />" })
        {
            BillingImportBatch batch = Batch(Fixture());
            batch.ImportXml = xml;
            Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", batch); }, "Deletion accepted a missing or malformed snapshot.");
        }
        BillingImportBatch empty = Batch(new ImportSprawBilling { SprawaDescriptor = new SprImportDescriptor[0] });
        Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", empty); }, "Deletion accepted a snapshot without cases.");
        BillingImportBatch nullRow = Batch(new ImportSprawBilling { SprawaDescriptor = new SprImportDescriptor[] { null } });
        Reject(delegate { Call("EnsureBillingBatchCanBeDeleted", nullRow); }, "Deletion accepted an unreadable case row.");
    }

    private static void DuplicateCannotBeEnabled()
    {
        ImportSprawBilling source = Fixture();
        source.SprawaDescriptor[0].CanImport = false;
        source.SprawaDescriptor[0].czyrej = false;
        BillingImportBatch batch = Batch(source);
        ImportSprawBilling edited = Copy(source);
        edited.SprawaDescriptor[0].CanImport = true;
        edited.SprawaDescriptor[0].czyrej = true;
        Reject(delegate { Apply(batch, edited); }, "Re-enabled a duplicate rejected by the server.");
        edited.SprawaDescriptor[0].czyrej = false;
        Assert(!Apply(batch, edited).SprawaDescriptor[0].CanImport, "Client overrode CanImport.");
    }

    private static void InvalidRowListsAreRejected()
    {
        ImportSprawBilling source = Fixture();
        BillingImportBatch batch = Batch(source);
        ImportSprawBilling edited = Copy(source);
        edited.SprawaDescriptor = edited.SprawaDescriptor.Take(2).ToArray();
        Reject(delegate { Apply(batch, edited); }, "Missing row accepted.");
        edited = Copy(source);
        edited.SprawaDescriptor[1].BatchRowNumber = 1;
        Reject(delegate { Apply(batch, edited); }, "Duplicate row key accepted.");
        edited = Copy(source);
        edited.SprawaDescriptor[1].BatchRowNumber = 999;
        Reject(delegate { Apply(batch, edited); }, "Unknown row key accepted.");
    }

    private static void NonDraftBatchesAreReadOnly()
    {
        ImportSprawBilling source = Fixture();
        foreach (string status in new[] { "Processing", "Completed", "Partial", "ReviewRequired" })
        {
            BillingImportBatch batch = Batch(source);
            batch.Status = status;
            Reject(delegate { Apply(batch, source); }, "Mutable batch status: " + status);
        }
    }

    private static void InvalidInterestIsRejected()
    {
        ImportSprawBilling source = Fixture();
        ImportSprawBilling edited = Copy(source);
        edited.SprawaDescriptor[0].CzyUstawowe = 999;
        Reject(delegate { Apply(Batch(source), edited); }, "Invalid interest accepted.");
    }

    private static void MergePreservesSource()
    {
        ImportSprawBilling source = Fixture();
        source.SprawaDescriptor = source.SprawaDescriptor.Take(2).ToArray();
        source.SprawaDescriptor[0].Odbiorca.Obciazenia[0].Wplaty = new[]
        {
            new LexEnaTrs.Web.Models.Wplata { Kwota_wplaty = 10m },
            new LexEnaTrs.Web.Models.Wplata { Kwota_wplaty = 0m },
            new LexEnaTrs.Web.Models.Wplata { Kwota_wplaty = 3m }
        };
        source.SprawaDescriptor[1].Odbiorca.Obciazenia[0].Wplaty[0].Kwota_wplaty = 0m;
        string before = Write(source);
        SprImportDescriptor merged = (SprImportDescriptor)Call("MergeBatchRows", (object)source.SprawaDescriptor);
        Equal(2, merged.Odbiorca.Obciazenia.Length, "Merge dropped or duplicated a charge.");
        Equal(2, merged.Odbiorca.Obciazenia.Select(c => c.Nr_dokumentu).Distinct().Count(), "Duplicate invoices.");
        Equal(303.57m, merged.Odbiorca.Obciazenia.Sum(c => c.Kwota_do_zaplaty), "Merged amount changed.");
        Equal("EW-2", merged.Odbiorca.Nr_ewidencyjnyArray.Single(), "Missing second billing identifier.");
        Equal((int?)1, merged.Odbiorca.Obciazenia[0].Id_sprawy, "Wrong legacy source-row key.");
        Equal((int?)2, merged.Odbiorca.Obciazenia[1].Id_sprawy, "Wrong legacy source-row key.");
        Equal(2, merged.Odbiorca.Obciazenia[0].Wplaty.Length, "Zero payment was retained or a valid payment lost.");
        Equal(13m, merged.Odbiorca.Obciazenia[0].Wplaty.Sum(w => w.Kwota_wplaty), "Valid payments lost from a mixed payment list.");
        Assert(merged.Odbiorca.Obciazenia[1].Wplaty == null, "Legacy zero-payment cleanup missing.");
        Equal(before, Write(source), "Merge mutated source rows.");
        merged.Odbiorca.Obciazenia[0].Kwota_do_zaplaty = 5000m;
        Equal(before, Write(source), "Merged charges share mutable source references.");
    }

    private static void CountsAndAmounts()
    {
        ImportSprawBilling source = Fixture();
        Equal(2, (int)Call("CountBatchCases", source), "Rows counted instead of cases.");
        source.SprawaDescriptor[1].id_symbol = 13;
        Equal(3, (int)Call("CountBatchCases", source), "Distinct case types were merged in the count.");
        source.SprawaDescriptor[1].id_symbol = 12;
        source.SprawaDescriptor[1].rok = 2025;
        Equal(3, (int)Call("CountBatchCases", source), "Distinct case years were merged in the count.");
        source.SprawaDescriptor[1].rok = 2026;
        source.SprawaDescriptor[0].BatchAmount = -999m;
        Equal(607.02m, (decimal)Call("CalculateBatchAmount", source), "Wrong total receivable.");
        Equal(0, (int)Call("CountImportedBatchCases", source), "Unimported rows counted.");
        source.SprawaDescriptor[0].czy_ok = true;
        source.SprawaDescriptor[0].WienaCaseId = 987;
        source.SprawaDescriptor[1].czy_ok = true;
        source.SprawaDescriptor[1].WienaCaseId = 987;
        source.SprawaDescriptor[2].czy_ok = true;
        source.SprawaDescriptor[2].WienaCaseId = 0;
        Equal(1, (int)Call("CountImportedBatchCases", source), "Shared Wiena case counted more than once or missing ID counted.");
        source.SprawaDescriptor[2].WienaCaseId = 988;
        Equal(2, (int)Call("CountImportedBatchCases", source), "Second imported case not counted.");
    }

    private static void SerializationRoundTrip()
    {
        CultureInfo previous = Thread.CurrentThread.CurrentCulture;
        try
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pl-PL");
            ImportSprawBilling source = Fixture();
            source.SprawaDescriptor[0].WienaCaseId = 678;
            string xml = Write(source);
            ImportSprawBilling actual = Read(Encoding.UTF8.GetString(Convert.FromBase64String(Payload(source))));
            Equal(xml, Write(actual), "XML roundtrip changed data.");
            Equal("Żaneta Łącka & Spółka", actual.SprawaDescriptor[0].Odbiorca.Nazwa, "Unicode text changed.");
            Equal(101.23m, actual.SprawaDescriptor[0].Odbiorca.Obciazenia[0].Kwota_do_zaplaty, "Decimal separator lost precision.");
            Equal(678, actual.SprawaDescriptor[0].WienaCaseId, "Wiena link was lost.");
            Equal(1, actual.SprawaDescriptor[0].BatchRowNumber, "Persistent row key was lost.");
        }
        finally { Thread.CurrentThread.CurrentCulture = previous; }
    }

    private static void SelectionSkipsRecipientGraph()
    {
        CultureInfo previous = Thread.CurrentThread.CurrentCulture;
        try
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pl-PL");
            ImportSprawBilling source = Fixture();
            source.SprawaDescriptor[0].WienaCaseId = 678;
            BillingImportBatch batch = Batch(source);
            string persisted = batch.ImportXml;
            ImportSprawBilling edited = Copy(source);
            edited.SprawaDescriptor[0].czyrej = false;
            edited.SprawaDescriptor[0].CzyUstawowe = 8;
            // Reproduce a failure of the old full-graph XML serialization. The selection
            // transport must not inspect billing fields, even when they cannot serialize.
            edited.SprawaDescriptor[0].Odbiorca.Obciazenia[0].TypOdsetek = (typOdsetek)999;
            Reject(delegate { Write(edited); }, "Fixture no longer reproduces full-graph XML failure.");

            string payload = BillingImportSelectionSerializer.ToBase64(edited.SprawaDescriptor);
            XmlDocument selection = new XmlDocument();
            selection.LoadXml(Encoding.UTF8.GetString(Convert.FromBase64String(payload)));
            Equal("ImportSprawBilling", selection.DocumentElement.Name, "Selection contract root changed.");
            XmlNodeList selectedRows = selection.SelectNodes("/ImportSprawBilling/SprawaDescriptor/SprImportDescriptor");
            Equal(3, selectedRows.Count, "Selection lost rows.");
            string[] fields = { "BatchRowNumber", "czyrej", "CzyUstawowe" };
            foreach (XmlNode row in selectedRows)
                Assert(fields.OrderBy(n => n).SequenceEqual(row.ChildNodes.Cast<XmlNode>()
                    .Where(n => n.NodeType == XmlNodeType.Element).Select(n => n.Name).OrderBy(n => n)),
                    "Selection contains fields beyond the stable key and editable choices.");
            Equal(0, selection.SelectNodes("//Odbiorca|//BatchAmount|//WienaCaseId|//Kwota_do_zaplaty|//DataKsiegowania").Count,
                "Full billing data leaked into the selection payload.");

            ImportSprawBilling actual = (ImportSprawBilling)Call("ApplyBatchChoices", batch, payload);
            ImportSprawBilling expected = Copy(source);
            expected.SprawaDescriptor[0].czyrej = false;
            expected.SprawaDescriptor[0].CzyUstawowe = 8;
            Equal(Write(expected), Write(actual), "Minimal selection changed trusted billing data.");
            Equal(persisted, batch.ImportXml, "Selection mutated the durable snapshot.");
            Equal((typOdsetek)999, edited.SprawaDescriptor[0].Odbiorca.Obciazenia[0].TypOdsetek,
                "Selection serialization altered the client recipient graph.");
        }
        finally { Thread.CurrentThread.CurrentCulture = previous; }
    }

    private static void MinimalSelectionKeysAreValidated()
    {
        ImportSprawBilling source = Fixture();
        BillingImportBatch batch = Batch(source);
        SprImportDescriptor[] choices = source.SprawaDescriptor.Select(r => new SprImportDescriptor
        {
            BatchRowNumber = r.BatchRowNumber, czyrej = r.czyrej, CzyUstawowe = r.CzyUstawowe
        }).ToArray();
        // No Odbiorca is required in this transport; all recipient data comes from the batch.
        string payload = BillingImportSelectionSerializer.ToBase64(choices);
        Equal(Write(source), Write((ImportSprawBilling)Call("ApplyBatchChoices", batch, payload)),
            "Valid minimal selections were not accepted.");
        Reject(delegate
        {
            Call("ApplyBatchChoices", batch, BillingImportSelectionSerializer.ToBase64(choices.Take(2)));
        }, "Missing row in minimal selection accepted.");
        choices[1].BatchRowNumber = 1;
        Reject(delegate
        {
            Call("ApplyBatchChoices", batch, BillingImportSelectionSerializer.ToBase64(choices));
        }, "Duplicate row key in minimal selection accepted.");
        choices[1].BatchRowNumber = 999;
        Reject(delegate
        {
            Call("ApplyBatchChoices", batch, BillingImportSelectionSerializer.ToBase64(choices));
        }, "Unknown row key in minimal selection accepted.");
    }

    private static void CurrentDistributionStatus()
    {
        DateTime firstDate = new DateTime(2026, 10, 1);
        DateTime latestDate = new DateTime(2026, 11, 1);
        WienaDB.Models.status_spr waiting = new WienaDB.Models.status_spr
        {
            ident = 100, id_sprawy = 701, id_statusu = 85, od_dnia = firstDate, czyus = 0
        };
        WienaDB.Models.status_spr discontinued = new WienaDB.Models.status_spr
        {
            ident = 101, id_sprawy = 701, id_statusu = 99, od_dnia = latestDate, czyus = 0
        };
        WienaDB.Models.status_spr deletedWaiting = new WienaDB.Models.status_spr
        {
            ident = 102, id_sprawy = 701, id_statusu = 85, od_dnia = latestDate.AddDays(1), czyus = 1
        };
        WienaDB.Models.status_spr actual = (WienaDB.Models.status_spr)Call("GetCurrentDistributionStatus",
            (object)new[] { discontinued, deletedWaiting, waiting });
        Equal(99, actual.id_statusu, "A historical or deleted waiting status overrode the current non-waiting status.");
        WienaDB.Models.status_spr tied = new WienaDB.Models.status_spr
        {
            ident = 103, id_sprawy = 701, id_statusu = 86, od_dnia = latestDate, czyus = 0
        };
        actual = (WienaDB.Models.status_spr)Call("GetCurrentDistributionStatus",
            (object)new[] { waiting, tied, deletedWaiting, discontinued });
        Equal(103, actual.ident, "Equal effective dates did not use the newest status record.");
        Equal(86, actual.id_statusu, "Current status changed with enumeration order.");
        Assert(Call("GetCurrentDistributionStatus", (object)new WienaDB.Models.status_spr[0]) == null,
            "Missing history invented a waiting status.");
        Assert(Call("GetCurrentDistributionStatus", (object)new[] { deletedWaiting }) == null,
            "A deleted-only history invented an active status.");
    }

    private static List<object> DistributionCompanyFixture()
    {
        var owner = new WienaDB.Models.firma { ident = 1312, typ = 1, typ_firmy = 1 };
        var otherOwner = new WienaDB.Models.firma { ident = 1313, typ = 1, typ_firmy = 2 };
        var region = new WienaDB.Models.firma { ident = 2012, typ = 2, typ_firmy = 2 };
        var cases = Enumerable.Range(1, 12).Select(i => new WienaDB.Models.sprawa
        {
            ident = 700 + i, sygnatura = "G-" + (16 + i) + "/2026", id_firmy = owner.ident,
            // In the actual EDMX the misleading firma navigation is keyed by id_rejon.
            id_rejon = i <= 6 ? (int?)region.ident : null,
            firma = i <= 6 ? region : null, czyus = 0
        }).ToList();
        cases.Add(new WienaDB.Models.sprawa { ident = 801, id_firmy = otherOwner.ident, id_rejon = owner.ident, firma = owner, czyus = 0 });
        cases.Add(new WienaDB.Models.sprawa { ident = 802, id_firmy = 9999, id_rejon = owner.ident, firma = owner, czyus = 0 });
        cases.Add(new WienaDB.Models.sprawa { ident = 803, id_firmy = owner.ident, czyus = 1 });
        cases.Add(new WienaDB.Models.sprawa { ident = 999, id_firmy = owner.ident, czyus = 0 });
        int[] requested = Enumerable.Range(701, 12).Concat(new[] { 801, 802, 803 }).ToArray();
        object query = Call("QueryDistributionWienaCases", cases.AsQueryable(),
            new[] { owner, otherOwner, region }.AsQueryable(), requested);
        return ((System.Collections.IEnumerable)query).Cast<object>().ToList();
    }

    private static T DistributionProjection<T>(object row, string property)
    {
        PropertyInfo member = row.GetType().GetProperty(property);
        if (member == null) throw new InvalidOperationException("Missing distribution projection property: " + property);
        return (T)member.GetValue(row, null);
    }

    private static BillingDistributionCase EligibilityForProjection(object actual, int? statusId)
    {
        var item = new BillingDistributionCase
        {
            WienaCaseId = DistributionProjection<int>(actual, "Id"),
            Signature = DistributionProjection<string>(actual, "Signature"),
            Amount = 123.45m, StatusId = statusId, Eligible = true, ExclusionReason = "Previous reason"
        };
        Call("SetDistributionEligibility", item, DistributionProjection<int?>(actual, "Firma"),
            DistributionProjection<int>(actual, "Deleted"), 1);
        return item;
    }

    private static void DistributionUsesOwningCompany()
    {
        List<object> actual = DistributionCompanyFixture();
        Equal(15, actual.Count, "A missing owner removed a requested case, or an out-of-batch case leaked into the query.");
        Assert(actual.All(row => DistributionProjection<int>(row, "Id") != 999), "Query ignored the batch's case identifiers.");
        List<BillingDistributionCase> waiting = actual.Where(row => DistributionProjection<int>(row, "Id") < 800)
            .Select(row => EligibilityForProjection(row, 85)).ToList();
        Equal(12, waiting.Count, "The newly imported batch lost cases.");
        Assert(actual.Where(row => DistributionProjection<int>(row, "Id") < 800)
            .All(row => DistributionProjection<int?>(row, "Firma") == 1),
            "Company was read from the region navigation instead of sprawa.id_firmy.");
        Assert(waiting.All(item => item.Eligible && String.IsNullOrEmpty(item.ExclusionReason)),
            "New status-85 cases were greyed out because their region belonged elsewhere or was absent.");
        Equal("G-17/2026", waiting.Single(item => item.WienaCaseId == 701).Signature, "Case projection lost its signature.");
    }

    private static void DistributionEligibilityGuards()
    {
        List<object> actual = DistributionCompanyFixture();
        object otherOwner = actual.Single(row => DistributionProjection<int>(row, "Id") == 801);
        object missingOwner = actual.Single(row => DistributionProjection<int>(row, "Id") == 802);
        object deleted = actual.Single(row => DistributionProjection<int>(row, "Id") == 803);
        Equal((int?)2, DistributionProjection<int?>(otherOwner, "Firma"), "Another owner inherited the matching region's company.");
        Equal((int?)null, DistributionProjection<int?>(missingOwner, "Firma"), "A missing owner was replaced by the region.");
        foreach (object row in new[] { otherOwner, missingOwner, deleted })
        {
            BillingDistributionCase item = EligibilityForProjection(row, 85);
            Assert(!item.Eligible && !String.IsNullOrWhiteSpace(item.ExclusionReason),
                "A wrong/missing owner or deleted case remained eligible despite its current state.");
        }
        object active = actual.Single(row => DistributionProjection<int>(row, "Id") == 701);
        foreach (int? status in new int?[] { null, 99, 86 })
        {
            BillingDistributionCase item = EligibilityForProjection(active, status);
            Assert(!item.Eligible && !String.IsNullOrWhiteSpace(item.ExclusionReason),
                "Missing or non-waiting status remained eligible.");
        }
        BillingDistributionCase negative = EligibilityForProjection(active, 85);
        negative.Amount = -0.01m;
        Call("SetDistributionEligibility", negative, (int?)1, 0, 1);
        Assert(!negative.Eligible && !String.IsNullOrWhiteSpace(negative.ExclusionReason), "A negative balance remained eligible.");
        negative.Amount = 0m;
        Call("SetDistributionEligibility", negative, (int?)1, 0, 1);
        Assert(negative.Eligible && String.IsNullOrEmpty(negative.ExclusionReason), "A zero-value waiting case remained excluded after correction.");
    }

    private static void HistoryIdentifierNormalization()
    {
        Equal("EW-000123", (string)Call("NormalizeDistributionCustomerNumber", "  ew-000123  "), "Customer number normalization lost identity or zeroes.");
        foreach (string value in new[] { null, "", "   " })
            Assert(String.IsNullOrEmpty((string)Call("NormalizeDistributionCustomerNumber", value)), "Empty customer number became a match key.");
        Equal("01234567890", (string)Call("NormalizeDistributionPesel", " 01234567890 "), "Valid PESEL length or leading zeroes were lost.");
        foreach (string value in new[] { null, "", "1234567890", "123456789012", "1234567890X", "１２３４５６７８９０１", "123-45678901" })
            Assert(String.IsNullOrEmpty((string)Call("NormalizeDistributionPesel", value)), "Malformed PESEL was accepted: " + value);
        Equal("0123456789", (string)Call("NormalizeDistributionNip", " 012-345-67-89 "), "NIP separators or leading zeroes were mishandled.");
        Equal("0123456789", (string)Call("NormalizeDistributionNip", "012 345 67 89"), "NIP spaces were not normalized.");
        foreach (string value in new[] { null, "", "123456789", "12345678901", "PL1234567890", "123456789X", "１２３４５６７８９０", "123.456.78.90" })
            Assert(String.IsNullOrEmpty((string)Call("NormalizeDistributionNip", value)), "Malformed NIP was accepted: " + value);
    }

    private static object HistoryQuery(IEnumerable<WienaDB.Models.sprawa> cases,
        IEnumerable<WienaDB.Models.spr_dluz> links, IEnumerable<WienaDB.Models.skazani> debtors)
    {
        var firms = new[]
        {
            new WienaDB.Models.firma { ident = 100, typ_firmy = 1 },
            new WienaDB.Models.firma { ident = 200, typ_firmy = 2 }
        };
        return Call("QueryDistributionHistoryRows", cases.AsQueryable(), firms.AsQueryable(),
            links.AsQueryable(), debtors.AsQueryable(), 1);
    }

    private static object HistoryQuery(params WienaDB.Models.sprawa[] cases)
    {
        return HistoryQuery(cases, new WienaDB.Models.spr_dluz[0], new WienaDB.Models.skazani[0]);
    }

    private static WienaDB.Models.sprawa HistoryCase(int id, string customer, int? office, DateTime created)
    {
        return new WienaDB.Models.sprawa
        {
            ident = id, id_firmy = 100, id_rejon = 200, czyus = 0, nr_ewid = customer,
            id_kancelarii = office, d_kreacji = created, sygnatura = "H-" + id + "/2026"
        };
    }

    private static BillingDistribution HistoryDistribution(params int[] caseIds)
    {
        return new BillingDistribution
        {
            BatchId = Guid.NewGuid(),
            Offices = new[]
            {
                new BillingDistributionOffice { UnitId = 10, WienaFirmaId = 9010, Name = "Kancelaria A", Selected = true, CountPercent = 50m, AmountPercent = 50m },
                new BillingDistributionOffice { UnitId = 20, WienaFirmaId = 9020, Name = "Kancelaria B", Selected = true, CountPercent = 50m, AmountPercent = 50m }
            },
            Cases = caseIds.Select((id, i) => new BillingDistributionCase
            {
                WienaCaseId = id, BatchRowNumber = i + 1, StatusId = 85, Eligible = true, Amount = 100m + i
            }).ToArray()
        };
    }

    private static void ApplyHistory(BillingDistribution data, object current, object history)
    {
        Call("ApplyDistributionHistory", data, current, history,
            new Dictionary<int, string> { { 9010, "Kancelaria A" }, { 9020, "Kancelaria B" }, { 9999, "Kancelaria poza konfiguracją" } });
    }

    private static void HistoryQueryScope()
    {
        DateTime time = new DateTime(2026, 10, 1);
        var numbered = HistoryCase(1, "EW-A", 9010, time);
        var nullActiveDebtor = HistoryCase(2, "EW-B", 9010, time);
        var otherFirm = HistoryCase(3, "EW-C", 9010, time);
        otherFirm.id_firmy = 200;
        var deletedCase = HistoryCase(4, "EW-D", 9010, time);
        deletedCase.czyus = 1;
        var query = HistoryQuery(new[] { numbered, nullActiveDebtor, otherFirm, deletedCase },
            new[]
            {
                new WienaDB.Models.spr_dluz { ident = 1, id_sprawy = 1, id_dluz = 11 },
                new WienaDB.Models.spr_dluz { ident = 2, id_sprawy = 2, id_dluz = 12 }
            },
            new[]
            {
                new WienaDB.Models.skazani { id = 11, czyus = 1, pesel = "12345678901", nip = "1234567890" },
                new WienaDB.Models.skazani { id = 12, czyus = null, pesel = "01234567890", nip = "0123456789" }
            });
        var rows = ((System.Collections.IEnumerable)query).Cast<object>().ToList();
        Equal(2, rows.Count, "Query leaked another company/deleted case or lost a case with only a deleted debtor.");
        object first = rows.Single(r => DistributionProjection<int>(r, "CaseId") == 1);
        Equal("EW-A", DistributionProjection<string>(first, "CustomerNumber"), "Independent customer number was lost with a deleted debtor.");
        Assert(String.IsNullOrEmpty(DistributionProjection<string>(first, "Pesel")) && String.IsNullOrEmpty(DistributionProjection<string>(first, "Nip")),
            "Deleted debtor identifiers leaked into matching.");
        object second = rows.Single(r => DistributionProjection<int>(r, "CaseId") == 2);
        Equal("01234567890", DistributionProjection<string>(second, "Pesel"), "NULL deletion marker incorrectly excluded an active debtor.");
    }

    private static System.Data.EntityClient.EntityConnection OfflineWienaConnection()
    {
        Assembly modelAssembly = typeof(WienaDB.Models.sprawa).Assembly;
        string[] resources = modelAssembly.GetManifestResourceNames();
        string metadata = String.Join("|", new[] { ".Wiena.csdl", ".Wiena.ssdl", ".Wiena.msl" }.Select(suffix =>
            "res://" + modelAssembly.GetName().Name + "/" + resources.Single(name => name.EndsWith(suffix, StringComparison.Ordinal))));
        var connectionString = new System.Data.EntityClient.EntityConnectionStringBuilder
        {
            Metadata = metadata,
            Provider = "System.Data.SqlClient",
            // Deliberately unusable: this check must never connect or read application configuration.
            ProviderConnectionString = "Data Source=unused;Initial Catalog=Unused;Integrated Security=True"
        };
        return new System.Data.EntityClient.EntityConnection(connectionString.ConnectionString);
    }

    private static void HistoryQuerySqlTranslation()
    {
        Assembly modelAssembly = typeof(WienaDB.Models.sprawa).Assembly;
        using (var connection = OfflineWienaConnection())
        using (var context = new System.Data.Objects.ObjectContext(connection))
        {
            context.DefaultContainerName = "wiena_centralEntities";
            context.ContextOptions.LazyLoadingEnabled = false;
            context.ContextOptions.ProxyCreationEnabled = false;
            context.MetadataWorkspace.LoadFromAssembly(modelAssembly);
            Equal(System.Data.ConnectionState.Closed, connection.State, "Metadata setup unexpectedly opened a connection.");
            DateTime newest = new DateTime(2026, 11, 8);
            var cases = context.CreateObjectSet<WienaDB.Models.sprawa>("sprawa")
                .Where(s => s.id_kancelarii.HasValue && s.id_kancelarii.Value > 0 && s.d_kreacji <= newest);
            var query = (IQueryable)Call("QueryDistributionHistoryRows", cases,
                context.CreateObjectSet<WienaDB.Models.firma>("firma"),
                context.CreateObjectSet<WienaDB.Models.spr_dluz>("spr_dluz"),
                context.CreateObjectSet<WienaDB.Models.skazani>("skazani"), 1);
            string sql = ((System.Data.Objects.ObjectQuery)query).ToTraceString();
            Assert(sql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) >= 0 &&
                sql.Contains("[id_firmy]") && sql.Contains("[spr_dluz]") && sql.Contains("[skazani]"),
                "History query did not translate the owning-company and debtor joins.");
            foreach (string property in new[] { "CustomerNumber", "Pesel", "Nip" })
            {
                // Reproduce the actual server's normalized IN filters over the private projection type.
                ParameterExpression row = Expression.Parameter(query.ElementType, "row");
                MemberExpression field = Expression.Property(row, property);
                Expression normalized = Expression.Call(field, "Trim", Type.EmptyTypes);
                if (property == "CustomerNumber") normalized = Expression.Call(normalized, "ToUpper", Type.EmptyTypes);
                if (property == "Nip")
                {
                    normalized = Expression.Call(normalized, "Replace", Type.EmptyTypes, Expression.Constant(" "), Expression.Constant(""));
                    normalized = Expression.Call(normalized, "Replace", Type.EmptyTypes, Expression.Constant("-"), Expression.Constant(""));
                }
                Expression contains = Expression.Call(typeof(Enumerable), "Contains", new[] { typeof(string) },
                    Expression.Constant(new[] { "01234567890", "0123456789" }), normalized);
                LambdaExpression predicate = Expression.Lambda(Expression.AndAlso(
                    Expression.NotEqual(field, Expression.Constant(null, typeof(string))), contains), row);
                Expression filtered = Expression.Call(typeof(Queryable), "Where", new[] { query.ElementType },
                    query.Expression, Expression.Quote(predicate));
                var filteredQuery = (System.Data.Objects.ObjectQuery)query.Provider.CreateQuery(filtered);
                string filteredSql = filteredQuery.ToTraceString();
                Assert(filteredSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) >= 0,
                    "Normalized history key filter failed SQL translation: " + property);
            }
            Equal(System.Data.ConnectionState.Closed, connection.State, "SQL translation opened an entity connection.");
            Equal(System.Data.ConnectionState.Closed, connection.StoreConnection.State, "SQL translation opened the SQL provider connection.");
        }
    }

    private static void HistoryLatestAssignment()
    {
        DateTime now = new DateTime(2026, 11, 8, 12, 0, 0);
        BillingDistribution data = HistoryDistribution(701, 702, 703, 704);
        object current = HistoryQuery(
            new[] { HistoryCase(701, " ew-a ", null, now), HistoryCase(702, "NEW-B", null, now), HistoryCase(703, "NEW-C", null, now), HistoryCase(704, "EW-TIE", null, now) },
            new[] { new WienaDB.Models.spr_dluz { id_sprawy = 702, id_dluz = 12 }, new WienaDB.Models.spr_dluz { id_sprawy = 703, id_dluz = 13 } },
            new[] { new WienaDB.Models.skazani { id = 12, pesel = "01234567890" }, new WienaDB.Models.skazani { id = 13, nip = "012-345-67-89" } });
        object history = HistoryQuery(
            new[]
            {
                HistoryCase(10, "EW-A", 9010, now.AddDays(-2)),
                HistoryCase(11, "EW-A", 9010, now.AddDays(-1)),
                HistoryCase(12, "EW-A", 9020, now.AddDays(-1)),
                HistoryCase(20, "OLD-B", 9010, now.AddDays(-1)),
                HistoryCase(30, "OLD-C", 9020, now.AddDays(-1)),
                HistoryCase(40, "EW-TIE", 9010, now)
            },
            new[] { new WienaDB.Models.spr_dluz { id_sprawy = 20, id_dluz = 22 }, new WienaDB.Models.spr_dluz { id_sprawy = 30, id_dluz = 23 } },
            new[] { new WienaDB.Models.skazani { id = 22, pesel = "01234567890" }, new WienaDB.Models.skazani { id = 23, nip = "0123456789" } });
        ApplyHistory(data, current, history);
        Equal((int?)20, data.Cases[0].FixedUnitId, "Customer-number match did not choose the newest case, including the identity tie-break.");
        Equal((int?)12, data.Cases[0].PreviousWienaCaseId, "Wrong previous case was selected for the customer number.");
        Equal("H-12/2026", data.Cases[0].PreviousSignature, "The UI would not receive the latest previous signature.");
        Equal((int?)10, data.Cases[1].FixedUnitId, "PESEL match failed when customer numbers differed.");
        Equal((int?)20, data.Cases[2].FixedUnitId, "Normalized NIP match failed when customer numbers differed.");
        Equal((int?)40, data.Cases[3].PreviousWienaCaseId, "A lower identity with the same creation time was not considered earlier.");
        Assert(data.Cases.All(c => !c.AssignedUnitId.HasValue && !String.IsNullOrWhiteSpace(c.AssignmentReason)),
            "Reading history allocated before the user requested a proposal or lost the match reason.");
        Equal(406m, data.Cases.Sum(c => c.Amount), "History matching changed case values.");
    }

    private static void HistoryFalseMatchesAreIgnored()
    {
        DateTime now = new DateTime(2026, 11, 8, 12, 0, 0);
        BillingDistribution data = HistoryDistribution(701, 702, 703);
        object current = HistoryQuery(
            new[] { HistoryCase(701, "", null, now), HistoryCase(702, "EW-SHARED", null, now), HistoryCase(703, "EW-SHARED", 9010, now.AddDays(-1)) },
            new[] { new WienaDB.Models.spr_dluz { id_sprawy = 701, id_dluz = 1 } },
            new[] { new WienaDB.Models.skazani { id = 1, pesel = "12345", nip = "123456789X" } });
        object history = HistoryQuery(
            new[]
            {
                HistoryCase(1, " ", 9010, now.AddDays(-1)),
                HistoryCase(2, "EW-SHARED", 9010, now.AddDays(1)),
                HistoryCase(999, "EW-SHARED", 9010, now),
                HistoryCase(703, "EW-SHARED", 9010, now.AddDays(-1))
            },
            new[] { new WienaDB.Models.spr_dluz { id_sprawy = 1, id_dluz = 2 } },
            new[] { new WienaDB.Models.skazani { id = 2, pesel = "12345", nip = "123456789X" } });
        ApplyHistory(data, current, history);
        Assert(data.Cases.All(c => !c.FixedUnitId.HasValue && !c.PreviousWienaCaseId.HasValue),
            "Invalid identifiers, a later case, or another case of the current batch created a mandatory assignment.");
    }

    private static void HistoryMissingCurrentCaseIsBlocked()
    {
        DateTime now = new DateTime(2026, 11, 8, 12, 0, 0);
        BillingDistribution data = HistoryDistribution(701, 702, 703);
        data.Cases[2].Eligible = false;
        object current = HistoryQuery(HistoryCase(701, "", null, now));
        ApplyHistory(data, current, HistoryQuery());
        Assert(data.Cases[0].Eligible && !data.Cases[0].FixedUnitId.HasValue && String.IsNullOrWhiteSpace(data.Cases[0].AssignmentBlockReason),
            "An existing case without identifiers was confused with unavailable case data.");
        Assert(data.Cases[1].Eligible && !String.IsNullOrWhiteSpace(data.Cases[1].AssignmentBlockReason),
            "Missing current Wiena case data silently allowed a free allocation.");
        Assert(!data.Cases[2].Eligible && String.IsNullOrWhiteSpace(data.Cases[2].AssignmentBlockReason),
            "An already excluded case acquired an unrelated history block.");
        data = HistoryDistribution(701, 702, 703);
        data.Cases[2].Eligible = false;
        ApplyHistory(data, HistoryQuery(), HistoryQuery());
        Assert(data.Cases.Take(2).All(c => !String.IsNullOrWhiteSpace(c.AssignmentBlockReason)) &&
            String.IsNullOrWhiteSpace(data.Cases[2].AssignmentBlockReason),
            "An empty current-case result did not block exactly the eligible cases.");
    }

    private static void HistoryMultipleDebtorsAndUnavailableOffice()
    {
        DateTime now = new DateTime(2026, 11, 8, 12, 0, 0);
        BillingDistribution data = HistoryDistribution(701);
        object current = HistoryQuery(new[] { HistoryCase(701, "NEW", null, now) },
            new[] { new WienaDB.Models.spr_dluz { id_sprawy = 701, id_dluz = 1 }, new WienaDB.Models.spr_dluz { id_sprawy = 701, id_dluz = 2 } },
            new[] { new WienaDB.Models.skazani { id = 1, pesel = "01234567890" }, new WienaDB.Models.skazani { id = 2, nip = "0123456789" } });
        object history = HistoryQuery(new[] { HistoryCase(10, "OLD-A", 9010, now.AddDays(-2)), HistoryCase(20, "OLD-B", 9999, now.AddDays(-1)) },
            new[] { new WienaDB.Models.spr_dluz { id_sprawy = 10, id_dluz = 3 }, new WienaDB.Models.spr_dluz { id_sprawy = 20, id_dluz = 4 } },
            new[] { new WienaDB.Models.skazani { id = 3, pesel = "01234567890" }, new WienaDB.Models.skazani { id = 4, nip = "0123456789" } });
        ApplyHistory(data, current, history);
        BillingDistributionCase item = data.Cases.Single();
        Assert(item.Eligible && !item.FixedUnitId.HasValue && !String.IsNullOrWhiteSpace(item.AssignmentBlockReason),
            "Unavailable latest office silently turned the case into a free or excluded case.");
        Equal((int?)20, item.PreviousWienaCaseId, "Multiple debtor links did not choose the latest matching case.");
        Equal((int?)9999, item.PreviousWienaFirmaId, "Unmapped historical office ID was lost.");
        Equal("H-20/2026", item.PreviousSignature, "Blocked match lost its reviewable previous signature.");
        Assert(!String.IsNullOrWhiteSpace(item.PreviousOfficeName), "Blocked office has no display name.");
        Assert(!item.AssignedUnitId.HasValue, "Reading an unavailable assignment created a proposal.");
    }

    private static void DistributionCaseGrouping()
    {
        ImportSprawBilling source = Fixture();
        source.SprawaDescriptor = source.SprawaDescriptor.Concat(new[] { Row(4, 103, 40.04m), Row(5, 104, 50.05m) }).ToArray();
        source.SprawaDescriptor[0].czy_ok = true;
        source.SprawaDescriptor[0].WienaCaseId = 701;
        source.SprawaDescriptor[1].czy_ok = true;
        source.SprawaDescriptor[1].WienaCaseId = 701;
        source.SprawaDescriptor[2].czy_ok = true;
        source.SprawaDescriptor[2].WienaCaseId = 702;
        source.SprawaDescriptor[3].czy_ok = false;
        source.SprawaDescriptor[3].WienaCaseId = 703;
        source.SprawaDescriptor[3].message = "Błąd księgowania";
        source.SprawaDescriptor[4].czy_ok = true;
        source.SprawaDescriptor[4].WienaCaseId = 0;
        foreach (SprImportDescriptor row in source.SprawaDescriptor) row.BatchAmount = -999m;
        source.SprawaDescriptor = source.SprawaDescriptor.Reverse().ToArray();
        string original = Write(source);

        List<BillingDistributionCase> result = (List<BillingDistributionCase>)Call("BuildDistributionCases", source);
        Equal(4, result.Count, "Grouped imported case or failed positions disappeared/duplicated.");
        BillingDistributionCase merged = result.Single(c => c.WienaCaseId == 701);
        Equal(303.57m, merged.Amount, "Shared case amount must sum original XML charges, not cached row totals.");
        Equal(1, merged.BatchRowNumber, "Shared case did not retain its earliest row key.");
        Equal("POC-101/2026", merged.Signature, "Shared case lost its source signature.");
        Equal("POC-101/2026", merged.SignatureBeforeApproval, "The registration signature is absent before decree.");
        Assert(merged.CustomerNumber.Contains("EW-1") && merged.CustomerNumber.Contains("EW-2"),
            "Shared case lost a customer identifier.");
        Equal("Żaneta Łącka & Spółka", merged.DebtorName, "The same debtor was duplicated in the grouped display name.");
        Equal(303.45m, result.Single(c => c.WienaCaseId == 702).Amount, "Separate imported case amount changed.");
        BillingDistributionCase failed = result.Single(c => c.BatchRowNumber == 4);
        Equal(0, failed.WienaCaseId, "Failed import was treated as an existing Wiena case.");
        Assert(String.IsNullOrEmpty(failed.SignatureBeforeApproval), "A failed import invented a registered Wiena signature.");
        Assert(!failed.Eligible && failed.ExclusionReason.Contains("Błąd księgowania"), "Failed import lost its exclusion/error information.");
        Assert(result.Single(c => c.BatchRowNumber == 5).WienaCaseId == 0,
            "A success flag without a persistent Wiena ID invented a case.");
        Assert(result.All(c => !c.Eligible && !c.AssignedUnitId.HasValue),
            "Cases became eligible before their live Wiena status was verified.");
        Equal(697.11m, result.Sum(c => c.Amount), "Grouped and excluded amounts lost precision or counted a row twice.");
        Equal(original, Write(source), "Distribution grouping mutated the imported snapshot.");
    }

    private static BillingDistribution DistributionFixture()
    {
        return new BillingDistribution
        {
            BatchId = new Guid("5fc31546-94fb-42b3-b39d-a9ae54f84d11"),
            CalculatedAt = new DateTime(2026, 11, 6, 12, 30, 0),
            HasSavedProposal = true,
            Offices = Enumerable.Range(1, 4).Select(i => new BillingDistributionOffice
            {
                UnitId = i * 10, WienaFirmaId = 100 + i, Name = "Kancelaria " + i, Selected = i != 3
            }).ToArray(),
            Cases = Enumerable.Range(1, 6).Select(i => new BillingDistributionCase
            {
                WienaCaseId = 700 + i, BatchRowNumber = i, Signature = "POC-" + i + "/2026",
                DebtorName = "Klient " + i, CustomerNumber = "EW-" + i,
                Amount = 10m * i, Eligible = true, StatusId = 85, StatusName = "Poczekalnia"
            }).ToArray()
        };
    }

    private static string SerializeDistribution(BillingDistribution distribution)
    {
        StringWriter writer = new StringWriter();
        new XmlSerializer(typeof(BillingDistribution)).Serialize(writer, distribution);
        return writer.ToString();
    }

    private static void RestoreCurrentDistribution()
    {
        BillingDistribution saved = DistributionFixture();
        saved.Cases[0].AssignedUnitId = 10; // valid
        saved.Cases[1].AssignedUnitId = 10; // current status changes
        saved.Cases[2].AssignedUnitId = 20; // office mapping changes
        saved.Cases[3].AssignedUnitId = 30; // office not selected in the saved proposal
        saved.Cases[4].AssignedUnitId = 40; // office removed from current configuration
        saved.Cases[5].AssignedUnitId = 10; // still valid; current amounts/details take precedence
        saved.Cases[5].Amount = 99999m;

        BillingDistribution current = DistributionFixture();
        current.HasSavedProposal = false;
        current.CalculatedAt = DateTime.MinValue;
        current.Offices = current.Offices.Take(3).Concat(new[]
        {
            new BillingDistributionOffice { UnitId = 50, WienaFirmaId = 105, Name = "Nowa kancelaria", Selected = true }
        }).ToArray();
        current.Offices[0].Name = "Aktualna nazwa kancelarii";
        current.Offices[1].WienaFirmaId = 202;
        current.Cases[1].Eligible = false;
        current.Cases[1].StatusId = 99;
        current.Cases[1].StatusName = "Umorzona";
        current.Cases[1].ExclusionReason = "Aktualny status Wieny jest inny niż 85.";
        current.Cases[5].Amount = 1234.56m;
        current.Cases[5].Signature = "AKT-6/2026";
        current.Cases[5].DebtorName = "Aktualny dłużnik";
        Call("RestoreBillingDistribution", current, SerializeDistribution(saved));
        BillingDistributionAllocator.RefreshTotals(current);

        Assert(current.HasSavedProposal, "A valid saved proposal was not recognized.");
        Equal(saved.CalculatedAt, current.CalculatedAt, "Saved proposal date was lost.");
        Equal((int?)10, current.Cases[0].AssignedUnitId, "Valid assignment was lost.");
        Equal((int?)10, current.Cases[5].AssignedUnitId, "Second valid assignment was lost.");
        Assert(current.Cases.Skip(1).Take(4).All(c => !c.AssignedUnitId.HasValue),
            "Changed status, changed mapping, unselected or removed office retained an assignment.");
        Assert(current.Offices.Where(o => o.UnitId != 10).All(o => !o.Selected),
            "An unselected, remapped or newly added office inherited selection.");
        Equal("Aktualna nazwa kancelarii", current.Cases[0].AssignedOfficeName, "Stale office name was restored.");
        Equal(1234.56m, current.Cases[5].Amount, "Saved XML overrode the current trusted amount.");
        Equal("AKT-6/2026", current.Cases[5].Signature, "Saved XML overrode the current signature.");
        Equal("Aktualny dłużnik", current.Cases[5].DebtorName, "Saved XML overrode the current debtor.");
        Equal((int?)99, current.Cases[1].StatusId, "Saved XML overrode the current Wiena status.");
        Equal(2, current.Offices[0].CaseCount, "Dropped assignments still contributed to totals.");
        Equal(1244.56m, current.Offices[0].TotalAmount, "Restored totals used stale or ineligible amounts.");
    }

    private static void RestoreInvalidDistribution()
    {
        BillingDistribution current = DistributionFixture();
        current.HasSavedProposal = false;
        Call("RestoreBillingDistribution", current, String.Empty);
        Assert(!current.HasSavedProposal, "Empty saved XML invented a proposal.");
        BillingDistribution saved = DistributionFixture();
        saved.BatchId = Guid.NewGuid();
        Reject(delegate { Call("RestoreBillingDistribution", current, SerializeDistribution(saved)); },
            "A proposal for a different batch was accepted.");
        saved = DistributionFixture();
        saved.Cases[0].AssignedUnitId = 10;
        saved.Cases = saved.Cases.Concat(new[] { saved.Cases[0] }).ToArray();
        Call("RestoreBillingDistribution", current, SerializeDistribution(saved));
        Assert(!current.Cases[0].AssignedUnitId.HasValue, "An ambiguous duplicated case record was restored.");
        saved = DistributionFixture();
        saved.Cases[0].AssignedUnitId = 10;
        saved.Offices = saved.Offices.Concat(new[] { saved.Offices[0] }).ToArray();
        current = DistributionFixture();
        Call("RestoreBillingDistribution", current, SerializeDistribution(saved));
        BillingDistributionAllocator.RefreshTotals(current);
        Assert(!current.Offices[0].Selected && !current.Cases[0].AssignedUnitId.HasValue,
            "An ambiguous duplicated office record was restored.");
    }

    private static void InitialDistributionSettings()
    {
        BillingDistribution current = DistributionFixture();
        current.HasSavedProposal = false;
        current.CalculatedAt = DateTime.MinValue;
        foreach (BillingDistributionOffice office in current.Offices) office.Selected = true;
        Call("InitializeDistributionSettings", current);
        Assert(current.Offices.All(o => o.Selected && o.CountPercent == 25m && o.AmountPercent == 25m),
            "Initial targets did not select all offices with equal percentages.");
        Assert(current.Cases.All(c => !c.AssignedUnitId.HasValue) && current.Offices.All(o => o.CaseCount == 0 && o.TotalAmount == 0m),
            "Initializing the editor allocated cases before the user requested a proposal.");
        Assert(!current.HasSavedProposal && current.CalculatedAt == DateTime.MinValue,
            "Initializing the editor invented a calculated/saved proposal.");
    }

    private static void DistributionRequestWeights()
    {
        BillingDistribution current = DistributionFixture();
        Call("ApplyDistributionPercentages", current, new[] { 40, 10 }, new[] { 25m, 75m }, new[] { 60m, 40m });
        BillingDistributionOffice first = current.Offices.Single(o => o.UnitId == 10);
        BillingDistributionOffice last = current.Offices.Single(o => o.UnitId == 40);
        Assert(first.Selected && first.CountPercent == 75m && first.AmountPercent == 40m,
            "First office received weights by array position instead of selected ID.");
        Assert(last.Selected && last.CountPercent == 25m && last.AmountPercent == 60m,
            "Last office received incorrect independent count/value weights.");
        Assert(current.Offices.Where(o => o.UnitId != 10 && o.UnitId != 40).All(o => !o.Selected && o.CountPercent == 0m && o.AmountPercent == 0m),
            "An unselected office retained participation or targets.");
        Assert(current.Cases.All(c => !c.AssignedUnitId.HasValue), "Applying request weights allocated cases.");
        Equal(210m, current.Cases.Sum(c => c.Amount), "Applying weights modified trusted case amounts.");
    }

    private static void InvalidDistributionRequests()
    {
        Reject(delegate { Call("ValidateDistributionRequest", new int[0], new decimal[0], new decimal[0]); }, "Empty office selection was accepted.");
        Reject(delegate { Call("ValidateDistributionRequest", new[] { 10, 10 }, new[] { 50m, 50m }, new[] { 50m, 50m }); }, "Duplicate selected IDs were accepted.");
        Reject(delegate { Call("ValidateDistributionRequest", new[] { 10 }, null, new[] { 100m }); }, "Missing count array was accepted.");
        Reject(delegate { Call("ValidateDistributionRequest", new[] { 10, 20 }, new[] { 100m }, new[] { 50m, 50m }); }, "Mismatched arrays were accepted.");
        Reject(delegate { Call("ValidateDistributionRequest", new[] { 10 }, new[] { -1m }, new[] { 100m }); }, "Negative percentage was accepted.");
        Reject(delegate { Call("ValidateDistributionRequest", new[] { 10 }, new[] { 100m }, new[] { 101m }); }, "Percentage over 100 was accepted.");
        Reject(delegate { Call("ValidateDistributionRequest", new[] { 10 }, new[] { 33.333m }, new[] { 100m }); }, "More than two decimal places were accepted.");
        Reject(delegate { Call("ApplyDistributionPercentages", DistributionFixture(), new[] { 999 }, new[] { 100m }, new[] { 100m }); }, "Unknown office was accepted.");
        Reject(delegate { Call("ApplyDistributionPercentages", DistributionFixture(), new[] { 10, 20 }, new[] { 50m, 40m }, new[] { 50m, 50m }); }, "Invalid count total was accepted.");
        Reject(delegate { Call("ApplyDistributionPercentages", DistributionFixture(), new[] { 10, 20 }, new[] { 50m, 50m }, new[] { 20m, 50m }); }, "Invalid amount total was accepted.");
        Call("ApplyDistributionPercentages", DistributionFixture(), new[] { 10, 20, 40 }, new[] { 33.33m, 33.33m, 33.33m }, new[] { 33.33m, 33.33m, 33.33m });
    }

    private static void RestoreDistributionPercentages()
    {
        BillingDistribution saved = DistributionFixture();
        saved.Offices[0].CountPercent = 60m;
        saved.Offices[0].AmountPercent = 20m;
        saved.Offices[1].CountPercent = 30m;
        saved.Offices[1].AmountPercent = 70m;
        saved.Offices[3].CountPercent = 10m;
        saved.Offices[3].AmountPercent = 10m;
        saved.Cases[0].AssignedUnitId = 10;
        BillingDistribution current = DistributionFixture();
        Call("InitializeDistributionSettings", current);
        Call("RestoreBillingDistribution", current, SerializeDistribution(saved));
        Equal(60m, current.Offices[0].CountPercent, "Saved count percentage was replaced by an equal share.");
        Equal(20m, current.Offices[0].AmountPercent, "Saved amount percentage was replaced by the count share.");
        Equal(70m, current.Offices[1].AmountPercent, "Second office amount percentage was lost.");
        Assert(!current.Offices[2].Selected && current.Offices[2].CountPercent == 0m && current.Offices[2].AmountPercent == 0m,
            "Unselected office acquired a target.");
        Equal((int?)10, current.Cases[0].AssignedUnitId, "Changing target serialization lost a legal saved assignment.");
    }

    private static void RestoreFixedDistribution()
    {
        BillingDistribution saved = DistributionFixture();
        saved.Cases[0].AssignedUnitId = 10;
        saved.Cases[0].FixedUnitId = 10;
        saved.Cases[0].PreviousWienaCaseId = 500;
        saved.Cases[0].PreviousSignature = "OLD-500/2025";
        saved.Cases[1].AssignedUnitId = 20;
        saved.Cases[2].AssignedUnitId = 10;
        saved.Cases[3].AssignedUnitId = 10;

        BillingDistribution current = DistributionFixture();
        current.Cases[0].FixedUnitId = 30; // Fresh history now requires an office absent from old selection.
        current.Cases[0].PreviousWienaCaseId = 600;
        current.Cases[0].PreviousWienaFirmaId = current.Offices[2].WienaFirmaId;
        current.Cases[0].PreviousSignature = "NEW-600/2026";
        current.Cases[0].PreviousOfficeName = "Aktualna kancelaria";
        current.Cases[0].AssignmentReason = "PESEL";
        current.Cases[1].FixedUnitId = 20; // Still legal.
        current.Cases[1].PreviousWienaCaseId = 601;
        current.Cases[2].PreviousWienaCaseId = 602;
        current.Cases[2].PreviousWienaFirmaId = 9999;
        current.Cases[2].AssignmentBlockReason = "Brak mapowania dotychczasowej kancelarii.";
        current.Cases[3].FixedUnitId = 10;
        current.Cases[3].Eligible = false;

        Call("RestoreBillingDistribution", current, SerializeDistribution(saved));
        Assert(!current.Cases[0].AssignedUnitId.HasValue, "Saved assignment overrode a changed mandatory office.");
        Equal((int?)30, current.Cases[0].FixedUnitId, "Saved XML overrode the fresh fixed office.");
        Equal((int?)600, current.Cases[0].PreviousWienaCaseId, "Saved XML overrode the fresh previous case.");
        Equal("NEW-600/2026", current.Cases[0].PreviousSignature, "Previous signature came from stale saved XML.");
        Equal("Aktualna kancelaria", current.Cases[0].PreviousOfficeName, "Fresh historic office name was lost.");
        Equal("PESEL", current.Cases[0].AssignmentReason, "Fresh match reason was lost.");
        Assert(current.Offices[2].Selected, "A mandatory office stayed deselected after restoring old choices.");
        Equal((int?)20, current.Cases[1].AssignedUnitId, "Valid fixed assignment was not restored.");
        Assert(!current.Cases[2].AssignedUnitId.HasValue && !String.IsNullOrWhiteSpace(current.Cases[2].AssignmentBlockReason),
            "Blocked historical assignment was silently treated as free.");
        Assert(!current.Cases[3].AssignedUnitId.HasValue, "An ineligible fixed case retained its saved assignment.");

        // A unit can keep its LexEna ID while its Wiena office mapping changes.
        // Mandatory selection for the next calculation must not reinterpret an old assignment.
        current = DistributionFixture();
        current.Offices[0].WienaFirmaId = 9998;
        current.Cases[0].FixedUnitId = 10;
        current.Cases[0].PreviousWienaFirmaId = 9998;
        current.Cases[0].PreviousWienaCaseId = 650;
        current.Cases[0].PreviousSignature = "NEW-650/2026";
        Call("RestoreBillingDistribution", current, SerializeDistribution(saved));
        Assert(current.Offices[0].Selected, "Remapped required office was not selected for recalculation.");
        Assert(!current.Cases[0].AssignedUnitId.HasValue && !current.Cases[2].AssignedUnitId.HasValue,
            "Mandatory selection revived a saved assignment through a changed Wiena mapping.");
        Equal((int?)10, current.Cases[0].FixedUnitId, "Fresh mandatory office was lost while discarding a stale assignment.");
    }

    private static void RestoreLegacyDistributionPercentages()
    {
        BillingDistribution saved = DistributionFixture();
        saved.Cases[0].AssignedUnitId = 10;
        XmlDocument legacy = new XmlDocument();
        legacy.LoadXml(SerializeDistribution(saved));
        foreach (XmlNode node in legacy.SelectNodes("//*[local-name()='CountPercent' or local-name()='AmountPercent']").Cast<XmlNode>().ToList())
            node.ParentNode.RemoveChild(node);
        BillingDistribution current = DistributionFixture();
        Call("InitializeDistributionSettings", current);
        Call("RestoreBillingDistribution", current, legacy.OuterXml);
        Assert(current.Offices.Where(o => o.Selected).All(o => o.CountPercent == 33.33m && o.AmountPercent == 33.33m),
            "Legacy selected offices did not receive equal editable targets.");
        Assert(current.Cases.Skip(1).All(c => !c.AssignedUnitId.HasValue), "Restoring legacy target defaults allocated new cases.");
        Equal((int?)10, current.Cases[0].AssignedUnitId, "Restoring legacy defaults lost an existing assignment.");
    }

    private static BillingDistribution ApprovalFixture()
    {
        return new BillingDistribution
        {
            BatchId = new Guid("d952319f-6f60-43fb-91c6-28fca173c615"),
            ProposalId = new Guid("8a4df1a9-a9de-45f6-909f-44cf9ecc4526"),
            HasSavedProposal = true, CalculatedAt = new DateTime(2026, 11, 8, 10, 30, 0),
            ApprovalState = "Planning", BranchId = 7, NumberingYear = 2026,
            Offices = new[]
            {
                new BillingDistributionOffice { UnitId = 10, WienaFirmaId = 9015, Name = "Kancelaria A",
                    SymbolSprawWiena = "KA", Selected = true, CountPercent = 50m, AmountPercent = 50m },
                new BillingDistributionOffice { UnitId = 20, WienaFirmaId = 9029, Name = "Kancelaria B",
                    SymbolSprawWiena = "KB", Selected = true, CountPercent = 50m, AmountPercent = 50m }
            },
            Cases = new[]
            {
                new BillingDistributionCase { WienaCaseId = 701, BatchRowNumber = 1, Signature = "POC-1/2026",
                    Amount = 120.25m, Eligible = true, StatusId = 85, AssignedUnitId = 10, AssignedOfficeName = "Kancelaria A",
                    FixedUnitId = 10, PreviousWienaCaseId = 600, PreviousWienaFirmaId = 9015, PreviousSignature = "KA-5/2025" },
                new BillingDistributionCase { WienaCaseId = 702, BatchRowNumber = 2, Signature = "POC-2/2026",
                    Amount = 80.75m, Eligible = true, StatusId = 85, AssignedUnitId = 20, AssignedOfficeName = "Kancelaria B" },
                new BillingDistributionCase { WienaCaseId = 703, BatchRowNumber = 3, Signature = "POC-3/2026",
                    Amount = 50m, Eligible = false, StatusId = 99, ExclusionReason = "Status inny niż 85." }
            }
        };
    }

    private static BillingDistribution CopyDistribution(BillingDistribution distribution)
    {
        using (StringReader reader = new StringReader(SerializeDistribution(distribution)))
            return (BillingDistribution)new XmlSerializer(typeof(BillingDistribution)).Deserialize(reader);
    }

    private static BillingImportBatch ApprovalBatch(BillingDistribution distribution)
    {
        ImportSprawBilling import = Fixture();
        import.IdOddzial = distribution.BranchId;
        import.SprawaDescriptor = distribution.Cases.Where(c => c.WienaCaseId > 0).Select(c =>
        {
            SprImportDescriptor row = Row(c.BatchRowNumber, c.BatchRowNumber, c.Amount);
            row.WienaCaseId = c.WienaCaseId;
            row.czy_ok = true;
            row.sygn_obciaz = String.IsNullOrWhiteSpace(c.SignatureBeforeApproval) ? c.Signature : c.SignatureBeforeApproval;
            return row;
        }).ToArray();
        return new BillingImportBatch
        {
            Id = distribution.BatchId, BranchId = distribution.BranchId, Year = distribution.NumberingYear,
            SymbolId = 12, SymbolName = "POC", Status = "Completed", DistributionState = "Planning",
            ImportXml = Write(import), DistributionXml = SerializeDistribution(distribution)
        };
    }

    private static void DistributionSignatureValidation()
    {
        CultureInfo previous = Thread.CurrentThread.CurrentCulture;
        try
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            Equal("KA-108/2026", (string)Call("BuildDistributionSignature", "KA", 108, 2026), "Incorrect decree signature format.");
            Equal("ABC-1234567/2026", (string)Call("BuildDistributionSignature", "ABC", 1234567, 2026), "A 16-character Wiena signature was rejected.");
            Equal("A-1/1900", (string)Call("BuildDistributionSignature", "A", 1, 1900), "Earliest allowed numbering year changed.");
            Equal("A-1/9999", (string)Call("BuildDistributionSignature", "A", 1, 9999), "Latest allowed numbering year changed.");
        }
        finally { Thread.CurrentThread.CurrentCulture = previous; }
        Reject(delegate { Call("BuildDistributionSignature", "ABC", 12345678, 2026); }, "Overlong signature accepted for varchar(16).");
        Reject(delegate { Call("BuildDistributionSignature", "KA", 0, 2026); }, "Zero case number accepted.");
        Reject(delegate { Call("BuildDistributionSignature", "KA", -1, 2026); }, "Negative case number accepted.");
        Reject(delegate { Call("BuildDistributionSignature", "KA", 1, 1899); }, "Numbering year below 1900 accepted.");
        Reject(delegate { Call("BuildDistributionSignature", "KA", 1, 10000); }, "Numbering year above 9999 accepted.");
        Reject(delegate { Call("BuildDistributionSignature", " ", 1, 2026); }, "Blank symbol accepted.");
    }

    private static void DistributionSymbolResolution()
    {
        var symbols = new[]
        {
            new WienaDB.Models.symbol { ident = 101, id_firmy = 7, oznaczenie = " KA ", typ = 2 },
            new WienaDB.Models.symbol { ident = 102, id_firmy = 8, oznaczenie = "KA", typ = 3 },
            new WienaDB.Models.symbol { ident = 103, id_firmy = 7, oznaczenie = "KB", typ = 4 }
        };
        var resolved = (WienaDB.Models.symbol)Call("ResolveDistributionSymbol", symbols, 7, " ka ");
        Equal(101, resolved.ident, "Office symbol was resolved from the wrong branch.");
        Equal(2, resolved.typ, "Resolved symbol lost its book type.");
        Equal(102, ((WienaDB.Models.symbol)Call("ResolveDistributionSymbol", symbols, 8, "KA")).ident, "Second branch symbol was ignored.");
        Reject(delegate { Call("ResolveDistributionSymbol", symbols, 9, "KA"); }, "Symbol from another branch was accepted.");
        Reject(delegate { Call("ResolveDistributionSymbol", symbols, 7, "UNKNOWN"); }, "Unknown symbol was accepted.");
        Reject(delegate { Call("ResolveDistributionSymbol", symbols, 7, " "); }, "Empty office designation was accepted.");
        Reject(delegate { Call("ResolveDistributionSymbol", symbols, 7, "ABCDEFGHIJK"); }, "A designation longer than Wiena's ten-character column was accepted.");
        var ambiguous = symbols.Concat(new[] { new WienaDB.Models.symbol { ident = 104, id_firmy = 7, oznaczenie = "ka" } }).ToArray();
        Reject(delegate { Call("ResolveDistributionSymbol", ambiguous, 7, "KA"); }, "Ambiguous branch designation silently selected one symbol.");
    }

    private static void DistributionSymbolChoiceScope()
    {
        var branches = new[]
        {
            new WienaDB.Models.firma { ident = 7, typ = 1, aktywna = 1, typ_firmy = 1, nazwa = "Oddział A" },
            new WienaDB.Models.firma { ident = 8, typ = 1, aktywna = 1, typ_firmy = 2, nazwa = "Oddział B" },
            new WienaDB.Models.firma { ident = 9, typ = 1, aktywna = 0, typ_firmy = 1, nazwa = "Nieaktywny" },
            new WienaDB.Models.firma { ident = 10, typ = 9, aktywna = 1, typ_firmy = 1, nazwa = "Kancelaria" }
        };
        var symbols = new[]
        {
            // No cases are created: a new, unused dictionary symbol must still be selectable.
            new WienaDB.Models.symbol { ident = 101, id_firmy = 7, oznaczenie = "NOWY", typ_firmy = 2 },
            new WienaDB.Models.symbol { ident = 102, id_firmy = 8, oznaczenie = "INNA", typ_firmy = 1 },
            new WienaDB.Models.symbol { ident = 103, id_firmy = 9, oznaczenie = "NIEAKT", typ_firmy = 1 },
            new WienaDB.Models.symbol { ident = 104, id_firmy = 10, oznaczenie = "KANC", typ_firmy = 1 },
            new WienaDB.Models.symbol { ident = 105, id_firmy = 99, oznaczenie = "BEZODDZ", typ_firmy = 1 }
        };
        object query = Call("QueryDistributionSymbolChoices", symbols.AsQueryable(), branches.AsQueryable(), (int?)1);
        List<object> rows = ((System.Collections.IEnumerable)query).Cast<object>().ToList();
        Equal(1, rows.Count, "Company symbol lookup admitted inactive, missing, non-branch or another company's owners.");
        Equal("NOWY", DistributionProjection<string>(rows[0], "Symbol"), "Unused symbol was filtered by the symbol's legacy company marker instead of its owning branch.");
        Equal(7, DistributionProjection<int>(rows[0], "BranchId"), "Symbol choice lost the owning branch.");
        Equal("Oddział A", DistributionProjection<string>(rows[0], "BranchName"), "Symbol choice lost its branch description.");
        var choices = (BillingDistributionSymbol[])Call("BuildDistributionSymbolChoices", query);
        Equal("NOWY", choices.Single().Symbol, "A symbol without any numbered cases cannot be selected.");

        object allQuery = Call("QueryDistributionSymbolChoices", symbols.AsQueryable(), branches.AsQueryable(), (int?)null);
        List<object> allRows = ((System.Collections.IEnumerable)allQuery).Cast<object>().ToList();
        Assert(allRows.Select(r => DistributionProjection<int>(r, "BranchId")).OrderBy(id => id).SequenceEqual(new[] { 7, 8 }),
            "Global configuration validation omitted another active company or admitted a non-branch.");
        object absent = Call("QueryDistributionSymbolChoices", symbols.AsQueryable(), branches.AsQueryable(), (int?)3);
        Equal(0, ((System.Collections.IEnumerable)absent).Cast<object>().Count(), "Unknown company inherited symbols from other companies.");
    }

    private static void DistributionSymbolChoiceGrouping()
    {
        var branches = new[]
        {
            new WienaDB.Models.firma { ident = 7, typ = 1, aktywna = 1, typ_firmy = 1, nazwa = " Łódź " },
            new WienaDB.Models.firma { ident = 8, typ = 1, aktywna = 1, typ_firmy = 1, nazwa = "Żary" },
            new WienaDB.Models.firma { ident = 9, typ = 1, aktywna = 1, typ_firmy = 1, nazwa = " " }
        };
        var symbols = new[]
        {
            new WienaDB.Models.symbol { ident = 101, id_firmy = 8, oznaczenie = " ka " },
            new WienaDB.Models.symbol { ident = 102, id_firmy = 7, oznaczenie = "KA" },
            new WienaDB.Models.symbol { ident = 103, id_firmy = 7, oznaczenie = " KA " },
            new WienaDB.Models.symbol { ident = 104, id_firmy = 7, oznaczenie = "b" },
            new WienaDB.Models.symbol { ident = 105, id_firmy = 9, oznaczenie = "Z" },
            new WienaDB.Models.symbol { ident = 106, id_firmy = 7, oznaczenie = null },
            new WienaDB.Models.symbol { ident = 107, id_firmy = 7, oznaczenie = "   " },
            new WienaDB.Models.symbol { ident = 108, id_firmy = 7, oznaczenie = "ABCDEFGHIJK" }
        };
        object query = Call("QueryDistributionSymbolChoices", symbols.AsQueryable(), branches.AsQueryable(), (int?)1);
        var choices = (BillingDistributionSymbol[])Call("BuildDistributionSymbolChoices", query);
        Assert(choices.Select(c => c.Symbol).SequenceEqual(new[] { "b", "KA", "Z" }),
            "Equivalent symbols were not merged and sorted, or blank/overlong symbols became selectable.");
        Equal("KA — Łódź, Żary", choices.Single(c => c.Symbol == "KA").DisplayName,
            "Merged choice repeated a branch or lost the branch names.");
        Equal("Z — Oddział 9", choices.Single(c => c.Symbol == "Z").DisplayName,
            "An unnamed branch has no useful fallback in the choice label.");
        object reverseQuery = Call("QueryDistributionSymbolChoices", symbols.Reverse().AsQueryable(), branches.Reverse().AsQueryable(), (int?)1);
        var reversed = (BillingDistributionSymbol[])Call("BuildDistributionSymbolChoices", reverseQuery);
        Assert(choices.Select(c => c.Symbol + "/" + c.DisplayName).SequenceEqual(reversed.Select(c => c.Symbol + "/" + c.DisplayName)),
            "The persisted symbol or branch labels depend on dictionary enumeration order.");
    }

    private static void DistributionSymbolChoiceSqlTranslation()
    {
        Assembly modelAssembly = typeof(WienaDB.Models.symbol).Assembly;
        using (var connection = OfflineWienaConnection())
        using (var context = new System.Data.Objects.ObjectContext(connection))
        {
            context.DefaultContainerName = "wiena_centralEntities";
            context.ContextOptions.LazyLoadingEnabled = false;
            context.ContextOptions.ProxyCreationEnabled = false;
            context.MetadataWorkspace.LoadFromAssembly(modelAssembly);
            foreach (int? company in new int?[] { 1, null })
            {
                var query = (IQueryable)Call("QueryDistributionSymbolChoices", context.CreateObjectSet<WienaDB.Models.symbol>("symbol"),
                    context.CreateObjectSet<WienaDB.Models.firma>("firma"), company);
                string sql = ((System.Data.Objects.ObjectQuery)query).ToTraceString();
                Assert(sql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) >= 0 && sql.Contains("[symbol]") &&
                    sql.Contains("[firma]") && sql.Contains("[id_firmy]") && sql.Contains("[aktywna]"),
                    "Symbol choices did not translate the active owning-branch join.");
                Assert(!sql.Contains("[sprawa]") && !sql.Contains("[vw_import_Symbole]") && !sql.Contains("[rok]"),
                    "Choice query depends on existing cases or annual numbering counters.");
            }
            Equal(System.Data.ConnectionState.Closed, connection.State, "Symbol SQL translation opened an entity connection.");
            Equal(System.Data.ConnectionState.Closed, connection.StoreConnection.State, "Symbol SQL translation opened a provider connection.");
        }
    }

    private static void DistributionApprovalUnchangedProposal()
    {
        BillingDistribution saved = ApprovalFixture();
        BillingDistribution current = CopyDistribution(saved);
        string before = SerializeDistribution(saved);
        Call("ValidateDistributionApprovalProposal", saved, current);
        Equal(before, SerializeDistribution(saved), "Approval validation rewrote the saved proposal.");
        Equal((int?)10, current.Cases[0].AssignedUnitId, "Fixed allocation was changed during validation.");
        Equal((int?)20, current.Cases[1].AssignedUnitId, "Free allocation was recalculated during validation.");
        Assert(!current.Cases[2].AssignedUnitId.HasValue && !current.Cases[2].Eligible,
            "A non-waiting case became assigned during approval validation.");

        current.Cases[2].StatusId = 100; // Still excluded: it must not enter this decree.
        Call("ValidateDistributionApprovalProposal", saved, current);
        saved = ApprovalFixture();
        saved.ProposalId = Guid.Empty;
        Reject(delegate { Call("ValidateDistributionApprovalProposal", saved, ApprovalFixture()); }, "A legacy proposal without a revision key was approved.");
        saved = ApprovalFixture();
        saved.HasSavedProposal = false;
        Reject(delegate { Call("ValidateDistributionApprovalProposal", saved, ApprovalFixture()); }, "Unsaved assignments were approved.");
        saved = ApprovalFixture();
        saved.BatchId = Guid.NewGuid();
        Reject(delegate { Call("ValidateDistributionApprovalProposal", saved, ApprovalFixture()); }, "A proposal for a different batch was approved.");
        saved = ApprovalFixture();
        saved.BranchId = 8;
        Reject(delegate { Call("ValidateDistributionApprovalProposal", saved, ApprovalFixture()); }, "A proposal for a different branch was approved.");
        saved = ApprovalFixture();
        saved.NumberingYear = 2025;
        Reject(delegate { Call("ValidateDistributionApprovalProposal", saved, ApprovalFixture()); }, "A proposal for another numbering year was approved.");
    }

    private static void RejectChangedApprovalCase(Action<BillingDistribution> change, string message)
    {
        BillingDistribution current = ApprovalFixture();
        change(current);
        Reject(delegate { Call("ValidateDistributionApprovalProposal", ApprovalFixture(), current); }, message);
    }

    private static void DistributionApprovalChangedCases()
    {
        RejectChangedApprovalCase(d => { d.Cases[0].Eligible = false; d.Cases[0].StatusId = 99; }, "A case that left status 85 remained in the decree.");
        RejectChangedApprovalCase(d => d.Cases[0].StatusId = 99, "A stale eligibility flag bypassed the current status check.");
        RejectChangedApprovalCase(d => { d.Cases[2].Eligible = true; d.Cases[2].StatusId = 85; }, "A newly eligible case was silently added to the saved decree.");
        RejectChangedApprovalCase(d => d.Cases = d.Cases.Skip(1).ToArray(), "A missing approved case was ignored.");
        RejectChangedApprovalCase(d => d.Cases = d.Cases.Concat(new[] { d.Cases[0] }).ToArray(), "A duplicated current case was approved twice.");
        RejectChangedApprovalCase(d => d.Cases[1].Amount += 0.01m, "A changed case amount was accepted.");
        RejectChangedApprovalCase(d => d.Cases[1].Signature = "OTHER-2/2026", "A changed source signature was accepted.");
        RejectChangedApprovalCase(d => d.Cases[0].FixedUnitId = 20, "A changed mandatory office was ignored.");
        RejectChangedApprovalCase(d => d.Cases[0].PreviousWienaCaseId = 601, "A newer historical case did not invalidate the proposal.");
        RejectChangedApprovalCase(d => d.Cases[0].PreviousWienaFirmaId = 9029, "A changed historical office did not invalidate the proposal.");
        RejectChangedApprovalCase(d => d.Cases[0].AssignmentBlockReason = "Brak kancelarii.", "A blocked historical assignment was approved.");
    }

    private static void DistributionApprovalChangedOffices()
    {
        RejectChangedApprovalCase(d => d.Offices[0].WienaFirmaId = 9999, "A changed office mapping was approved.");
        RejectChangedApprovalCase(d => d.Offices[0].SymbolSprawWiena = "KC", "A changed office symbol was approved without a new proposal.");
        RejectChangedApprovalCase(d => d.Offices[0].SymbolIssue = "Brak symbolu w oddziale.", "An unresolved symbol was approved.");
        RejectChangedApprovalCase(d => d.Offices = d.Offices.Skip(1).ToArray(), "An unavailable assigned office was approved.");
        BillingDistribution saved = ApprovalFixture();
        saved.Cases[1].AssignedUnitId = null;
        Reject(delegate { Call("ValidateDistributionApprovalProposal", saved, ApprovalFixture()); }, "An incomplete saved allocation was approved.");
        saved = ApprovalFixture();
        saved.Cases[1].AssignedUnitId = 9999;
        Reject(delegate { Call("ValidateDistributionApprovalProposal", saved, ApprovalFixture()); }, "An unknown saved assignment was approved.");
        saved = ApprovalFixture();
        saved.Cases[0].AssignedUnitId = 20;
        Reject(delegate { Call("ValidateDistributionApprovalProposal", saved, ApprovalFixture()); }, "A saved assignment could override the debtor's mandatory office.");
        saved = ApprovalFixture();
        saved.Offices[1].Selected = false;
        Reject(delegate { Call("ValidateDistributionApprovalProposal", saved, ApprovalFixture()); }, "A case assigned to an unselected office was approved.");
    }

    private static WienaDB.Models.sprawa ApprovalSourceCase()
    {
        return new WienaDB.Models.sprawa { ident = 701, id_firmy = 7, id_symbol = 12,
            rok = 2026, poz = 1, sygnatura = "POC-1/2026", czyus = 0, id_kancelarii = null };
    }

    private static void DistributionApprovalRevisionAndLockedResult()
    {
        BillingDistribution saved = ApprovalFixture();
        BillingImportBatch batch = ApprovalBatch(saved);
        var result = (BillingDistribution)Call("RequireDistributionProposal", batch, saved.ProposalId);
        Equal(saved.ProposalId, result.ProposalId, "Exact saved proposal was not retrieved.");
        Reject(delegate { Call("RequireDistributionProposal", batch, Guid.NewGuid()); }, "A stale client could approve a different saved revision.");
        Reject(delegate { Call("RequireDistributionProposal", batch, Guid.Empty); }, "An absent client revision bypassed approval review.");

        saved.ApprovalState = "Approved";
        saved.ApprovedAt = new DateTime(2026, 11, 8, 11, 0, 0);
        saved.Cases[0].SignatureBeforeApproval = saved.Cases[0].Signature;
        saved.Cases[0].Signature = "KA-108/2026";
        saved.Cases[0].Decreed = true;
        batch.DistributionState = "Approved";
        batch.DistributionXml = SerializeDistribution(saved);
        using (StringReader reader = new StringReader((string)Call("GetLockedBillingDistribution", batch)))
            result = (BillingDistribution)new XmlSerializer(typeof(BillingDistribution)).Deserialize(reader);
        Assert(!result.CanApprove && result.ApprovalState == "Approved", "Completed receipt permits a second decree.");
        Equal(saved.ProposalId, result.ProposalId, "Opening a locked batch generated a different proposal.");
        Equal("KA-108/2026", result.Cases[0].Signature, "Opening a locked batch changed the assigned number.");
        Equal("KA-108/2026", result.Cases[0].DecreeSignature, "A legacy approved receipt did not expose its actual final signature.");
        Equal("POC-1/2026", result.Cases[0].SignatureBeforeApproval, "Opening a locked batch lost its original signature.");
        Equal((int?)20, result.Cases[1].AssignedUnitId, "Opening a locked batch recalculated assignments.");

        saved = ApprovalFixture();
        batch.DistributionXml = SerializeDistribution(saved);
        batch.DistributionState = "Processing";
        using (StringReader reader = new StringReader((string)Call("GetLockedBillingDistribution", batch)))
            result = (BillingDistribution)new XmlSerializer(typeof(BillingDistribution)).Deserialize(reader);
        Assert(result.CanApprove && result.ApprovalState == "Processing" && !String.IsNullOrWhiteSpace(result.ApprovalProblem),
            "An uncertain operation cannot be checked or resumed with the same proposal.");
        Equal(saved.ProposalId, result.ProposalId, "Resuming an operation changed its identity.");
    }

    private static void DistributionApprovalAvailability()
    {
        BillingDistribution distribution = ApprovalFixture();
        Call("SetDistributionApprovalAvailability", distribution, ApprovalBatch(distribution));
        Assert(distribution.CanApprove, "A complete unchanged proposal cannot be approved.");
        distribution = ApprovalFixture();
        Call("SetDistributionApprovalAvailability", distribution, null);
        Assert(distribution.CanApprove, "A newly calculated complete proposal cannot be approved.");

        Action<BillingDistribution>[] invalid =
        {
            d => d.ProposalId = Guid.Empty,
            d => d.HasSavedProposal = false,
            d => d.Cases[1].AssignedUnitId = null,
            d => d.Cases[0].AssignmentBlockReason = "Brak aktualnej kancelarii.",
            d => d.Offices[0].SymbolIssue = "Brak symbolu w oddziale.",
            d => { foreach (BillingDistributionCase item in d.Cases) item.Eligible = false; }
        };
        foreach (Action<BillingDistribution> change in invalid)
        {
            distribution = ApprovalFixture();
            change(distribution);
            Call("SetDistributionApprovalAvailability", distribution, ApprovalBatch(distribution));
            Assert(!distribution.CanApprove && !String.IsNullOrWhiteSpace(distribution.ApprovalProblem),
                "An incomplete or blocked proposal offers approval without an explanation.");
        }

        distribution = ApprovalFixture();
        BillingImportBatch staleBatch = ApprovalBatch(distribution);
        distribution.Cases[0].Eligible = false;
        Call("SetDistributionApprovalAvailability", distribution, staleBatch);
        Assert(!distribution.CanApprove, "Opening a stale saved proposal enabled approval after eligibility changed.");
    }

    private static void DistributionApprovalSourceCaseGuards()
    {
        BillingDistribution expected = ApprovalFixture();
        BillingImportBatch batch = ApprovalBatch(expected);
        Call("ValidateDistributionCaseForApproval", ApprovalSourceCase(), batch, expected.Cases[0]);
        var source = ApprovalSourceCase();
        source.id_kancelarii = 0;
        Call("ValidateDistributionCaseForApproval", source, batch, expected.Cases[0]);
        Action<WienaDB.Models.sprawa>[] changes =
        {
            s => s.id_kancelarii = 9015, s => s.id_firmy = 8, s => s.id_symbol = 101,
            s => s.rok = 2025, s => s.czyus = 1, s => s.sygnatura = "OTHER-1/2026"
        };
        foreach (Action<WienaDB.Models.sprawa> change in changes)
        {
            source = ApprovalSourceCase();
            change(source);
            Reject(delegate { Call("ValidateDistributionCaseForApproval", source, batch, expected.Cases[0]); },
                "A changed or already assigned Wiena case was decreed again.");
        }
        Reject(delegate { Call("ValidateDistributionCaseForApproval", null, batch, expected.Cases[0]); },
            "A missing Wiena case was accepted for decree.");
    }

    private static void DistributionNumberingScope()
    {
        var cases = new[]
        {
            new WienaDB.Models.sprawa { ident = 1, id_firmy = 7, id_symbol = 101, rok = 2026, poz = 7, czyus = 0 },
            new WienaDB.Models.sprawa { ident = 2, id_firmy = 7, id_symbol = 101, rok = 2026, poz = 108, czyus = 1 },
            new WienaDB.Models.sprawa { ident = 3, id_firmy = 8, id_symbol = 101, rok = 2026, poz = 900, czyus = 0 },
            new WienaDB.Models.sprawa { ident = 4, id_firmy = 7, id_symbol = 102, rok = 2026, poz = 901, czyus = 0 },
            new WienaDB.Models.sprawa { ident = 5, id_firmy = 7, id_symbol = 101, rok = 2025, poz = 902, czyus = 0 },
            new WienaDB.Models.sprawa { ident = 6, id_firmy = 7, id_symbol = null, rok = 2026, poz = 903, czyus = 0 }
        };
        var actual = (IQueryable<WienaDB.Models.sprawa>)Call("QueryDistributionNumberingCases", cases.AsQueryable(), 7, 101, 2026);
        Assert(actual.Select(c => c.ident).OrderBy(id => id).SequenceEqual(new[] { 1, 2 }), "Numbering included another branch, symbol or year, or excluded a deleted case.");
        int lastNumber = actual.Select(c => (int?)c.poz).Max().GetValueOrDefault();
        Equal(108, lastNumber, "The highest deleted case number would be reused.");
        var emptyYear = (IQueryable<WienaDB.Models.sprawa>)Call("QueryDistributionNumberingCases", cases.AsQueryable(), 7, 101, 2027);
        Equal(0, emptyYear.Select(c => (int?)c.poz).Max().GetValueOrDefault(), "A new year inherited an older year's counter.");
        BillingDistribution distribution = ApprovalFixture();
        int nextNumber = (int)Call("ApplyDistributionCaseApproval", ApprovalSourceCase(), ApprovalBatch(distribution), distribution.Cases[0],
            new WienaDB.Models.symbol { ident = 101, id_firmy = 7, oznaczenie = "KA", typ = 2 }, 9015, lastNumber, DateTime.Now);
        Equal(109, nextNumber, "The actual decree did not continue after the highest reserved number.");
    }

    private static Dictionary<string, object> PropertySnapshot(object value)
    {
        return value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
            .ToDictionary(p => p.Name, p => p.GetValue(value, null));
    }

    private static void AssertUnchangedProperties(object value, Dictionary<string, object> before, params string[] allowedChanges)
    {
        foreach (var property in value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0 && !allowedChanges.Contains(p.Name)))
            Equal(before[property.Name], property.GetValue(value, null), "Unexpected change to " + value.GetType().Name + "." + property.Name + ".");
    }

    private static void DistributionCaseMutation()
    {
        BillingDistribution distribution = ApprovalFixture();
        BillingDistributionCase receipt = distribution.Cases[0];
        var source = ApprovalSourceCase();
        source.ksiega = 1;
        source.data_r = new DateTime(2026, 11, 1);
        source.data_ostatnia = new DateTime(2026, 11, 3);
        source.d_kreacji = new DateTime(2026, 11, 1, 9, 10, 11);
        source.d_modyfikacji = new DateTime(2026, 11, 2);
        source.kreator = 41;
        source.modyfikator = 42;
        source.nr_ewid = "00001234";
        source.kwota = 120.25m;
        var charge = new WienaDB.Models.naleznosc { ident = 3001, id_sprawy = source.ident, kwota = 120.25m, data_n = new DateTime(2026, 10, 1) };
        source.naleznosc.Add(charge);
        Dictionary<string, object> oldCase = PropertySnapshot(source);
        Dictionary<string, object> oldCharge = PropertySnapshot(charge);
        DateTime modifiedAt = new DateTime(2026, 11, 8, 12, 34, 56);
        var target = new WienaDB.Models.symbol { ident = 101, id_firmy = 7, oznaczenie = " KA ", typ = 2 };

        int number = (int)Call("ApplyDistributionCaseApproval", source, ApprovalBatch(distribution), receipt, target, 9015, 108, modifiedAt);

        Equal(109, number, "Decree returned an incorrect next counter.");
        Equal(109, source.poz, "Existing case did not receive the next number.");
        Equal("KA-109/2026", source.sygnatura, "Existing case received an incorrect signature.");
        Equal((int?)101, source.id_symbol, "Existing case retained its waiting-room symbol.");
        Equal((int?)9015, source.id_kancelarii, "Existing case did not receive the assigned office.");
        Equal(2, source.ksiega, "Existing case book does not follow the target symbol.");
        Equal(modifiedAt, source.d_modyfikacji, "Existing case modification date was not updated.");
        AssertUnchangedProperties(source, oldCase, "id_kancelarii", "id_symbol", "ksiega", "poz", "rok", "sygnatura", "d_modyfikacji");
        Equal((short)2026, source.rok, "Decree used another numbering year.");
        Equal(1, source.naleznosc.Count, "Decree duplicated or removed a charge.");
        AssertUnchangedProperties(charge, oldCharge);
        Equal("POC-1/2026", receipt.SignatureBeforeApproval, "Decree receipt lost the waiting-room signature.");
        Equal(source.sygnatura, receipt.Signature, "Receipt and existing case have different new signatures.");
        Equal(source.sygnatura, receipt.DecreeSignature, "The display retained a preview instead of the committed signature.");
        Assert(receipt.Decreed, "Successfully mutated case has no decree marker.");
        Equal("KA-5/2025", receipt.PreviousSignature, "Decree overwrote the historical debtor signature.");
        Equal(120.25m, receipt.Amount, "Decree modified the proposal amount.");
    }

    private static void DistributionCaseCounters()
    {
        BillingDistribution distribution = ApprovalFixture();
        BillingImportBatch batch = ApprovalBatch(distribution);
        DateTime modifiedAt = new DateTime(2026, 11, 8, 12, 0, 0);
        var commonSymbol = new WienaDB.Models.symbol { ident = 101, id_firmy = 7, oznaczenie = "KA", typ = 2 };
        var first = ApprovalSourceCase();
        int commonCounter = (int)Call("ApplyDistributionCaseApproval", first, batch, distribution.Cases[0], commonSymbol, 9015, 108, modifiedAt);
        var second = ApprovalSourceCase();
        second.ident = 702;
        second.poz = 2;
        second.sygnatura = distribution.Cases[1].Signature;
        commonCounter = (int)Call("ApplyDistributionCaseApproval", second, batch, distribution.Cases[1], commonSymbol, 9029, commonCounter, modifiedAt);
        Equal(109, first.poz, "First office did not continue the shared symbol counter.");
        Equal(110, second.poz, "Second office restarted numbering for the same symbol.");
        Equal("KA-110/2026", second.sygnatura, "Second office received an inconsistent shared-symbol signature.");
        Equal((int?)9029, second.id_kancelarii, "Shared symbol replaced the second office with the first office.");

        BillingDistribution independent = ApprovalFixture();
        var third = ApprovalSourceCase();
        int otherCounter = (int)Call("ApplyDistributionCaseApproval", third, ApprovalBatch(independent), independent.Cases[0],
            new WienaDB.Models.symbol { ident = 102, id_firmy = 7, oznaczenie = "KB", typ = 3 }, 9015, 0, modifiedAt);
        Equal(1, otherCounter, "An empty symbol inherited another symbol's counter.");
        Equal("KB-1/2026", third.sygnatura, "An independent symbol did not begin at one.");
        Equal(110, commonCounter, "An independent symbol changed the shared counter.");
        Equal(3, third.ksiega, "Another symbol inherited the wrong book.");
    }

    private static void DistributionCaseMutationFailure()
    {
        foreach (int lastNumber in new[] { Int32.MaxValue, 1234567 })
        {
            BillingDistribution distribution = ApprovalFixture();
            BillingDistributionCase receipt = distribution.Cases[0];
            var source = ApprovalSourceCase();
            Dictionary<string, object> oldCase = PropertySnapshot(source);
            string oldReceipt = SerializeDistribution(distribution);
            var target = new WienaDB.Models.symbol { ident = 101, id_firmy = 7, oznaczenie = "ABCD", typ = 2 };
            bool rejected = false;
            try
            {
                Call("ApplyDistributionCaseApproval", source, ApprovalBatch(distribution), receipt, target, 9015, lastNumber, DateTime.Now);
            }
            catch (OverflowException) { rejected = lastNumber == Int32.MaxValue; }
            catch (InvalidOperationException) { rejected = lastNumber != Int32.MaxValue; }
            Assert(rejected, "Overflow or an overlong signature did not abort decree.");
            AssertUnchangedProperties(source, oldCase);
            Equal(oldReceipt, SerializeDistribution(distribution), "Failed decree mutated its receipt before validating the new signature.");
        }
    }

    private static WienaDB.Models.symbol[] PreviewSymbols()
    {
        return new[]
        {
            new WienaDB.Models.symbol { ident = 101, id_firmy = 7, oznaczenie = " KA ", typ = 2 },
            new WienaDB.Models.symbol { ident = 102, id_firmy = 7, oznaczenie = "KB", typ = 3 },
            new WienaDB.Models.symbol { ident = 103, id_firmy = 8, oznaczenie = "KA", typ = 4 }
        };
    }

    private static void FillSignaturePreview(BillingDistribution distribution, IEnumerable<WienaDB.Models.symbol> symbols,
        IDictionary<int, int> counters)
    {
        Call("PopulateDistributionSignaturePreview", distribution, symbols, counters);
    }

    private static void DistributionSignaturePreviewNumbers()
    {
        BillingDistribution distribution = ApprovalFixture();
        distribution.NumberingYear = 2031;
        distribution.Offices = distribution.Offices.Concat(new[]
        {
            new BillingDistributionOffice { UnitId = 30, WienaFirmaId = 9032, Name = "Kancelaria C",
                SymbolSprawWiena = "KA", Selected = true }
        }).ToArray();
        distribution.Cases = distribution.Cases.Concat(new[]
        {
            new BillingDistributionCase { WienaCaseId = 704, BatchRowNumber = 4, Signature = "POC-4/2026",
                Eligible = true, StatusId = 85, AssignedUnitId = 30, Amount = 4.25m },
            new BillingDistributionCase { WienaCaseId = 705, BatchRowNumber = 5, Signature = "POC-5/2026",
                Eligible = true, StatusId = 85, Amount = 5.25m },
            new BillingDistributionCase { WienaCaseId = 706, BatchRowNumber = 6, Signature = "KA-20/2026",
                Eligible = true, StatusId = 85, AssignedUnitId = 10, Amount = 6.25m, Decreed = true }
        }).Reverse().ToArray();
        foreach (var row in distribution.Cases)
        {
            row.SignatureBeforeApproval = "G-" + row.WienaCaseId + "/2026";
            row.DecreeSignature = "STALE-1/2026";
            row.DecreeSignatureIssue = "stale problem";
        }
        var originals = distribution.Cases.ToDictionary(c => c.WienaCaseId, PropertySnapshot);
        var counters = new Dictionary<int, int> { { 101, 108 }, { 102, 7 }, { 103, 999 } };
        var unchangedCounters = counters.OrderBy(p => p.Key).ToArray();
        FillSignaturePreview(distribution, PreviewSymbols(), counters);
        Equal("KA-109/2031", distribution.Cases.Single(c => c.WienaCaseId == 701).DecreeSignature,
            "Preview did not use the batch's numbering year and the maximum of its branch's symbol.");
        Equal("KB-8/2031", distribution.Cases.Single(c => c.WienaCaseId == 702).DecreeSignature,
            "Another symbol inherited the first symbol's counter.");
        Equal("KA-110/2031", distribution.Cases.Single(c => c.WienaCaseId == 704).DecreeSignature,
            "Two offices sharing a symbol generated duplicate or nonconsecutive signatures.");
        Equal("KA-20/2026", distribution.Cases.Single(c => c.WienaCaseId == 706).DecreeSignature,
            "An already committed case received a newly proposed number.");
        Assert(distribution.Cases.Where(c => !c.Eligible || !c.AssignedUnitId.HasValue)
            .All(c => String.IsNullOrEmpty(c.DecreeSignature)), "Excluded or unassigned cases retain a target signature.");
        Assert(distribution.Cases.Where(c => c.Eligible && c.AssignedUnitId.HasValue)
            .All(c => String.IsNullOrEmpty(c.DecreeSignatureIssue)), "A successful preview retained a stale issue.");
        Assert(unchangedCounters.SequenceEqual(counters.OrderBy(p => p.Key)), "A preview reserved or mutated the supplied counters.");
        foreach (var row in distribution.Cases)
            AssertUnchangedProperties(row, originals[row.WienaCaseId], "DecreeSignature", "DecreeSignatureIssue");
        var expected = distribution.Cases.ToDictionary(c => c.WienaCaseId, c => c.DecreeSignature);
        distribution.Cases = distribution.Cases.Reverse().ToArray();
        FillSignaturePreview(distribution, PreviewSymbols().Reverse(), counters);
        Assert(distribution.Cases.All(c => expected[c.WienaCaseId] == c.DecreeSignature),
            "Row or dictionary ordering changed the proposed numbering.");
    }

    private static void DistributionSignaturePreviewRefresh()
    {
        BillingDistribution distribution = ApprovalFixture();
        foreach (var row in distribution.Cases) row.SignatureBeforeApproval = row.Signature;
        var counters = new Dictionary<int, int> { { 101, 108 }, { 102, 7 } };
        FillSignaturePreview(distribution, PreviewSymbols(), counters);
        Equal("KA-109/2026", distribution.Cases[0].DecreeSignature, "Initial preview is incorrect.");
        ChangeManualOffice(distribution, 701, 20);
        FillSignaturePreview(distribution, PreviewSymbols(), counters);
        Equal("KB-8/2026", distribution.Cases[0].DecreeSignature, "The preview did not follow the manually selected office.");
        Equal("KB-9/2026", distribution.Cases[1].DecreeSignature, "Other cases under that symbol retained conflicting old preview numbers.");
        Equal((int?)20, distribution.Cases[1].AssignedUnitId, "Refreshing previews changed another case's office.");
        Equal("POC-1/2026", distribution.Cases[0].Signature, "Preview changed the actual Wiena signature.");
        Equal("POC-1/2026", distribution.Cases[0].SignatureBeforeApproval, "Preview changed the XML registration signature.");
        counters[102] = 8;
        FillSignaturePreview(distribution, PreviewSymbols(), counters);
        Equal("KB-9/2026", distribution.Cases[0].DecreeSignature, "A refreshed counter did not update the proposed signature.");
        Equal("KB-10/2026", distribution.Cases[1].DecreeSignature, "Refreshed numbering is not consecutive.");
        BillingDistribution copied = CopyDistribution(distribution);
        Equal("KB-9/2026", copied.Cases[0].DecreeSignature, "XML lost the current preview.");
        Assert(copied.Cases[0].ManualAssignment && !copied.Cases[0].Decreed,
            "Preview serialization changed the meaning of a manual assignment.");
    }

    private static void DistributionSignaturePreviewErrors()
    {
        Action<BillingDistribution, List<WienaDB.Models.symbol>, Dictionary<int, int>>[] problems =
        {
            (data, symbols, counters) => data.Offices[0].SymbolSprawWiena = "UNKNOWN",
            (data, symbols, counters) => symbols.RemoveAll(s => s.ident == 101),
            (data, symbols, counters) => symbols.Add(new WienaDB.Models.symbol { ident = 104, id_firmy = 7, oznaczenie = "ka" }),
            (data, symbols, counters) => counters.Remove(101),
            (data, symbols, counters) => counters[101] = Int32.MaxValue,
            (data, symbols, counters) => { data.Offices[0].SymbolSprawWiena = "ABCDEFGHIJ"; symbols[0].oznaczenie = "ABCDEFGHIJ"; }
        };
        foreach (var change in problems)
        {
            BillingDistribution distribution = ApprovalFixture();
            distribution.Cases = distribution.Cases.Concat(new[]
            {
                new BillingDistributionCase { WienaCaseId = 704, BatchRowNumber = 4, Signature = "POC-4/2026",
                    Eligible = true, StatusId = 85, AssignedUnitId = 10, Amount = 4m }
            }).ToArray();
            var symbols = PreviewSymbols().ToList();
            var counters = new Dictionary<int, int> { { 101, 108 }, { 102, 7 } };
            distribution.Cases[0].DecreeSignature = "STALE-1/2026";
            change(distribution, symbols, counters);
            FillSignaturePreview(distribution, symbols, counters);
            Assert(String.IsNullOrEmpty(distribution.Cases[0].DecreeSignature) &&
                !String.IsNullOrWhiteSpace(distribution.Cases[0].DecreeSignatureIssue),
                "Invalid symbol or numbering retained a signature without an explanation.");
            Equal("KB-8/2026", distribution.Cases[1].DecreeSignature, "One symbol's problem suppressed an independent valid preview.");
            Equal("POC-1/2026", distribution.Cases[0].Signature, "A preview error changed the current Wiena signature.");
            Equal((int?)10, distribution.Cases[0].AssignedUnitId, "A preview error discarded the proposed office.");
            Assert(String.IsNullOrEmpty(distribution.Cases[3].DecreeSignature) &&
                !String.IsNullOrWhiteSpace(distribution.Cases[3].DecreeSignatureIssue),
                "A later case under an invalid symbol reused or skipped the unavailable number silently.");
        }
    }

    private static void DistributionSignatureColumnsFromImport()
    {
        ImportSprawBilling import = Fixture();
        import.SprawaDescriptor[0].WienaCaseId = 701;
        import.SprawaDescriptor[1].WienaCaseId = 701;
        import.SprawaDescriptor[2].WienaCaseId = 702;
        foreach (var row in import.SprawaDescriptor) row.czy_ok = true;
        string originalXml = Write(import);
        BillingDistribution distribution = ApprovalFixture();
        distribution.Cases[0].Signature = "KA-109/2026";
        distribution.Cases[0].SignatureBeforeApproval = "OTHER-1/2026";
        distribution.Cases[0].Decreed = true;
        distribution.Cases[0].DecreeSignatureIssue = "old preview issue";
        distribution.Cases[1].Signature = "CURRENT-2/2026";
        var originalCases = distribution.Cases.ToDictionary(c => c.WienaCaseId, PropertySnapshot);
        Call("SetDistributionSignatureColumns", distribution, import);
        Equal("POC-101/2026", distribution.Cases[0].SignatureBeforeApproval,
            "Legacy receipt did not recover the original grouped XML registration signature.");
        Equal("POC-102/2026", distribution.Cases[1].SignatureBeforeApproval,
            "A changed current signature obscured the XML registration signature before decree.");
        Equal("KA-109/2026", distribution.Cases[0].DecreeSignature,
            "Legacy receipt without the new preview field did not display the committed signature.");
        Assert(String.IsNullOrEmpty(distribution.Cases[0].DecreeSignatureIssue),
            "A legacy committed result retained a preview issue.");
        foreach (var row in distribution.Cases)
            AssertUnchangedProperties(row, originalCases[row.WienaCaseId],
                "SignatureBeforeApproval", "DecreeSignature", "DecreeSignatureIssue");
        Equal(originalXml, Write(import), "Signature recovery mutated the original import XML.");
    }

    private static void DistributionOriginalSignaturePreserved()
    {
        BillingDistribution distribution = ApprovalFixture();
        var row = distribution.Cases[0];
        row.SignatureBeforeApproval = "XML-101/2026";
        row.DecreeSignature = "KA-108/2026";
        row.DecreeSignatureIssue = "old preview issue";
        var source = ApprovalSourceCase();
        var target = PreviewSymbols().First(s => s.ident == 101);
        Call("ApplyDistributionCaseApproval", source, ApprovalBatch(distribution), row, target, 9015, 108, DateTime.Now);
        Equal("XML-101/2026", row.SignatureBeforeApproval, "Final decree replaced the original XML registration signature with the latest source signature.");
        Equal("KA-109/2026", row.DecreeSignature, "Final display did not use the actually committed signature.");
        Equal(row.Signature, row.DecreeSignature, "Current and final signatures disagree after decree.");
        Assert(String.IsNullOrEmpty(row.DecreeSignatureIssue), "A committed signature retained a preview error.");
        Equal("KA-5/2025", row.PreviousSignature, "Original registration replaced the separate debtor-history signature.");
    }

    private static void DistributionApprovalSerialization()
    {
        BillingDistribution approved = ApprovalFixture();
        approved.ApprovalState = "Approved";
        approved.ApprovedAt = new DateTime(2026, 11, 8, 11, 30, 0);
        approved.ApprovedBy = "Żaneta Łącka";
        approved.CanApprove = false;
        approved.Cases[0].SignatureBeforeApproval = approved.Cases[0].Signature;
        approved.Cases[0].Signature = "KA-108/2026";
        approved.Cases[0].Decreed = true;
        BillingDistribution restored = CopyDistribution(approved);
        Equal(approved.ProposalId, restored.ProposalId, "Receipt lost its proposal revision identifier.");
        Equal(approved.ApprovalState, restored.ApprovalState, "Receipt lost the approved state.");
        Equal(approved.ApprovedAt, restored.ApprovedAt, "Receipt lost the approval timestamp.");
        Equal(approved.ApprovedBy, restored.ApprovedBy, "Receipt lost the approving user.");
        Equal("KA-108/2026", restored.Cases[0].Signature, "Receipt lost the new signature.");
        Equal("POC-1/2026", restored.Cases[0].SignatureBeforeApproval, "Receipt lost the waiting-room signature.");
        Equal("KA-5/2025", restored.Cases[0].PreviousSignature, "Waiting-room history overwrote debtor history.");
        Assert(restored.Cases[0].Decreed && !restored.Cases[2].Decreed, "Receipt changed the scope of decreed cases.");
        Equal(2026, restored.NumberingYear, "Receipt lost the numbering year.");
    }

    private static void ChangeManualOffice(BillingDistribution distribution, int caseId, int unitId)
    {
        Call("ApplyManualDistributionAssignment", distribution, caseId, unitId, "Żaneta Łącka",
            new DateTime(2026, 11, 9, 10, 15, 0));
    }

    private static void ManualAssignmentBalances()
    {
        BillingDistribution distribution = ApprovalFixture();
        BillingDistributionAllocator.RefreshTotals(distribution);
        var fixedBefore = PropertySnapshot(distribution.Cases[0]);
        var excludedBefore = PropertySnapshot(distribution.Cases[2]);
        Guid proposalId = distribution.ProposalId;
        ChangeManualOffice(distribution, 702, 10);
        Equal((int?)10, distribution.Cases[1].AssignedUnitId, "The chosen case was not moved.");
        Equal("Kancelaria A", distribution.Cases[1].AssignedOfficeName, "The assigned office label stayed stale.");
        Assert(distribution.Cases[1].ManualAssignment, "The manual choice was not marked.");
        Equal((DateTime?)new DateTime(2026, 11, 9, 10, 15, 0), distribution.Cases[1].ManualAssignedAt, "Manual change time was lost.");
        Equal("Żaneta Łącka", distribution.Cases[1].ManualAssignedBy, "Manual author was lost.");
        Equal("POC-2/2026", distribution.Cases[1].Signature, "Editing a proposal renumbered a case.");
        Assert(!distribution.Cases[1].Decreed, "Editing a proposal marked a case as decreed.");
        Equal(2, distribution.Offices[0].CaseCount, "Target office did not gain exactly one case.");
        Equal(201m, distribution.Offices[0].TotalAmount, "Target office did not gain the exact case amount.");
        Equal(0, distribution.Offices[1].CaseCount, "Source office retained the moved case.");
        Equal(0m, distribution.Offices[1].TotalAmount, "Source office retained the moved amount.");
        Assert(distribution.Offices.All(o => o.TargetCaseCount == 1 && o.TargetAmount == 100.5m),
            "Manual editing changed requested targets instead of only actual totals.");
        Equal(100.5m, distribution.Offices[0].AmountDeviation, "Manual deviation from the target is incorrect.");
        Equal(proposalId, distribution.ProposalId, "The pure reassignment helper changed the persisted revision identity.");
        AssertUnchangedProperties(distribution.Cases[0], fixedBefore);
        AssertUnchangedProperties(distribution.Cases[2], excludedBefore);
    }

    private static void ManualAssignmentHistoryOverride()
    {
        BillingDistribution distribution = ApprovalFixture();
        ChangeManualOffice(distribution, 701, 20);
        BillingDistributionCase item = distribution.Cases[0];
        Equal((int?)20, item.AssignedUnitId, "Explicit manual choice did not override the historical office.");
        Assert(item.ManualAssignment && !item.IsFixedAssignment, "Historical override still appears as a locked automatic assignment.");
        Equal((int?)10, item.FixedUnitId, "Manual override erased the historical office identifier.");
        Equal((int?)600, item.PreviousWienaCaseId, "Manual override erased the historical case.");
        Equal("KA-5/2025", item.PreviousSignature, "Manual override erased the historical signature.");
        Equal(2, distribution.Offices[1].CaseCount, "The overridden case was excluded from the balance.");
        BillingDistributionAllocator.RefreshTotals(distribution);
        Equal((int?)20, item.AssignedUnitId, "Refreshing totals undid the explicit historical override.");
        Call("ValidateDistributionApprovalProposal", distribution, ApprovalFixture());

        BillingDistribution tampered = CopyDistribution(distribution);
        tampered.Cases[0].ManualAssignment = false;
        Reject(delegate { Call("ValidateDistributionApprovalProposal", tampered, ApprovalFixture()); },
            "An automatic allocation could bypass history without an explicit manual marker.");
    }

    private static void RejectManualAssignment(Action<BillingDistribution> change, int caseId, int unitId, string reason)
    {
        BillingDistribution distribution = ApprovalFixture();
        change(distribution);
        string before = SerializeDistribution(distribution);
        Reject(delegate { ChangeManualOffice(distribution, caseId, unitId); }, reason);
        Equal(before, SerializeDistribution(distribution), "Rejected manual change mutated the proposal: " + reason);
    }

    private static void ManualAssignmentGuards()
    {
        foreach (string state in new[] { "Processing", "Approved", null })
            RejectManualAssignment(d => d.ApprovalState = state, 702, 10, "A locked or invalid state allowed editing.");
        RejectManualAssignment(d => d.HasSavedProposal = false, 702, 10, "Unsaved assignments could be edited.");
        RejectManualAssignment(d => d.ProposalId = Guid.Empty, 702, 10, "A legacy proposal without a revision could be edited.");
        RejectManualAssignment(d => d.Cases[1].Eligible = false, 702, 10, "An excluded case could be manually assigned.");
        RejectManualAssignment(d => d.Cases[1].StatusId = 99, 702, 10, "An obsolete status-85 eligibility flag bypassed the live status.");
        RejectManualAssignment(d => d.Cases[1].Decreed = true, 702, 10, "A decreed case could be edited.");
        RejectManualAssignment(d => d.Cases[1].AssignedUnitId = null, 702, 10, "A case without a proposal assignment could be edited.");
        RejectManualAssignment(d => d.Cases[1].AssignmentBlockReason = "Brak mapowania kancelarii.", 702, 10, "An unresolved case could bypass its block.");
        RejectManualAssignment(d => d.Cases[1].PreviousWienaCaseId = 100, 702, 10, "Missing historical office mapping could be bypassed.");
        RejectManualAssignment(d => d.Cases[1].PreviousWienaFirmaId = 9999, 702, 10, "Incomplete historical mapping could be bypassed.");
        RejectManualAssignment(d => d.Offices[0].WienaFirmaId = 0, 702, 10, "An office without a Wiena mapping was selectable.");
        RejectManualAssignment(d => { }, 9999, 10, "A case outside the batch could be edited.");
        RejectManualAssignment(d => { }, 702, 9999, "An office outside the configured participants was selectable.");
        RejectManualAssignment(d => { }, 0, 10, "A missing case identifier was accepted.");
        RejectManualAssignment(d => { }, 702, 0, "A missing target office was accepted.");
        RejectManualAssignment(d => { }, 702, 20, "A no-op overwrote the manual audit fields.");
        RejectManualAssignment(d => d.Offices[0].CountPercent = 49m, 702, 10, "Invalid requested proportions were silently normalized.");
    }

    private static void ManualAssignmentAdditionalOffice()
    {
        // Both positive and negative rounding remainders occur with equal shares.
        foreach (int participantCount in new[] { 3, 6 })
        {
            BillingDistribution distribution = ApprovalFixture();
            decimal equalShare = Math.Round(100m / participantCount, 2);
            distribution.Offices = Enumerable.Range(1, participantCount).Select(i => new BillingDistributionOffice
            {
                UnitId = i * 10, WienaFirmaId = 9000 + i, Name = "Kancelaria " + i,
                SymbolSprawWiena = "K" + i, Selected = true, CountPercent = equalShare, AmountPercent = equalShare
            }).Concat(new[] { new BillingDistributionOffice { UnitId = 1000, WienaFirmaId = 9999,
                Name = "Dodatkowa kancelaria", SymbolSprawWiena = "KD", Selected = false } }).ToArray();
            ChangeManualOffice(distribution, 702, 1000);
            BillingDistributionOffice target = distribution.Offices.Last();
            Assert(target.Selected, "The newly used configured office did not join the batch balance.");
            Equal(0m, target.CountPercent, "An additional office silently received a requested count share.");
            Equal(0m, target.AmountPercent, "An additional office silently received a requested amount share.");
            Equal(1, target.CaseCount, "Zero target share excluded a manual case from the actual balance.");
            Equal(80.75m, target.TotalAmount, "Additional office did not receive the moved amount.");
            Equal(100m, distribution.Offices.Sum(o => o.CountPercent), "Rounded count shares became invalid after adding a zero-share office.");
            Equal(100m, distribution.Offices.Sum(o => o.AmountPercent), "Rounded amount shares became invalid after adding a zero-share office.");
            Assert(distribution.Offices.Take(participantCount).All(o => Math.Abs(o.CountPercent - equalShare) <= 0.01m),
                "Normalizing equal percentages changed more than the rounding remainder.");
            BillingDistributionAllocator.ValidatePercentages(distribution);
            Equal((int?)10, distribution.Cases[0].AssignedUnitId, "Adding a participant redistributed another case.");
            Equal(201m, distribution.Offices.Sum(o => o.TotalAmount), "Adding a participant changed the sum of eligible amounts.");
        }
    }

    private static void ManualAssignmentSnapshotValidation()
    {
        BillingDistribution saved = ApprovalFixture();
        saved.Offices[1].SymbolSprawWiena = String.Empty;
        saved.Offices[1].SymbolIssue = "Brak symbolu.";
        BillingDistribution current = CopyDistribution(saved);
        Call("ValidateDistributionProposalSnapshot", saved, current, false);
        Reject(delegate { Call("ValidateDistributionApprovalProposal", saved, current); },
            "Allowing manual edits with missing symbols also enabled final approval.");
        current.Cases[1].FixedUnitId = 20;
        current.Cases[1].PreviousWienaCaseId = 601;
        current.Cases[1].PreviousWienaFirmaId = 9029;
        Reject(delegate { Call("ValidateDistributionProposalSnapshot", saved, current, false); },
            "Manual editing bypassed changed debtor history.");

        saved = ApprovalFixture();
        saved.Offices = saved.Offices.Concat(new[] { new BillingDistributionOffice
            { UnitId = 30, WienaFirmaId = 9032, SymbolSprawWiena = "KC", Selected = false } }).ToArray();
        current = CopyDistribution(saved);
        Call("ValidateManualDistributionOfficeSnapshot", saved, current, 30);
        current.Offices[2].WienaFirmaId = 9999;
        Reject(delegate { Call("ValidateManualDistributionOfficeSnapshot", saved, current, 30); },
            "A changed mapping on an unselected target office escaped validation.");
        current = CopyDistribution(saved);
        current.Offices[2].SymbolSprawWiena = "KD";
        Reject(delegate { Call("ValidateManualDistributionOfficeSnapshot", saved, current, 30); },
            "A changed target symbol escaped validation.");
        current.Offices = current.Offices.Take(2).ToArray();
        Reject(delegate { Call("ValidateManualDistributionOfficeSnapshot", saved, current, 30); },
            "A removed configured office escaped validation.");
    }

    private static BillingDistribution EmptyApprovalAssignments()
    {
        BillingDistribution current = ApprovalFixture();
        foreach (var item in current.Cases) { item.AssignedUnitId = null; item.AssignedOfficeName = null; }
        return current;
    }

    private static void ManualAssignmentRestore()
    {
        BillingDistribution saved = ApprovalFixture();
        ChangeManualOffice(saved, 701, 20);
        string xml = SerializeDistribution(saved);
        BillingDistribution roundTrip = CopyDistribution(saved);
        Assert(roundTrip.Cases[0].ManualAssignment, "XML lost the explicit override marker.");
        Equal(saved.Cases[0].ManualAssignedAt, roundTrip.Cases[0].ManualAssignedAt, "XML lost the manual timestamp.");
        Equal(saved.Cases[0].ManualAssignedBy, roundTrip.Cases[0].ManualAssignedBy, "XML lost the manual author.");
        BillingDistribution current = EmptyApprovalAssignments();
        Call("RestoreBillingDistribution", current, xml);
        BillingDistributionAllocator.RefreshTotals(current);
        Equal((int?)20, current.Cases[0].AssignedUnitId, "Reopening lost the reviewed historical override.");
        Assert(current.Cases[0].ManualAssignment && !current.Cases[0].IsFixedAssignment, "Reopening converted the manual exception back to an automatic lock.");
        Equal(saved.Cases[0].ManualAssignedBy, current.Cases[0].ManualAssignedBy, "Reopening lost the manual audit metadata.");

        current = EmptyApprovalAssignments();
        current.Cases[0].PreviousWienaCaseId = 650;
        Call("RestoreBillingDistribution", current, xml);
        Assert(!current.Cases[0].AssignedUnitId.HasValue && !current.Cases[0].ManualAssignment,
            "A manual exception was silently reused after the historical case changed.");
        current = EmptyApprovalAssignments();
        current.Offices[1].WienaFirmaId = 9999;
        Call("RestoreBillingDistribution", current, xml);
        Assert(!current.Cases[0].AssignedUnitId.HasValue && !current.Cases[0].ManualAssignment,
            "Reopening retargeted a manual choice through a changed office mapping.");
        current = EmptyApprovalAssignments();
        current.Cases[0].Eligible = false;
        current.Cases[0].StatusId = 99;
        Call("RestoreBillingDistribution", current, xml);
        Assert(!current.Cases[0].AssignedUnitId.HasValue && !current.Cases[0].ManualAssignment,
            "An excluded case retained a manual assignment.");
    }

    private static void ManualAssignmentRecalculation()
    {
        BillingDistribution distribution = ApprovalFixture();
        ChangeManualOffice(distribution, 701, 20);
        BillingDistributionAllocator.Allocate(distribution);
        Equal((int?)10, distribution.Cases[0].AssignedUnitId, "Automatic recalculation retained a manual exception to debtor history.");
        Assert(distribution.Cases[0].IsFixedAssignment, "Recalculation failed to restore the historical lock.");
        Assert(distribution.Cases.All(c => !c.ManualAssignment && !c.ManualAssignedAt.HasValue && c.ManualAssignedBy == null),
            "Automatic recalculation left stale manual audit fields.");
        Equal(201m, distribution.Offices.Sum(o => o.TotalAmount), "Resetting manual choices changed the total amount.");
    }

    private static void SchemaMatches(string repository)
    {
        XmlDocument model = new XmlDocument();
        model.Load(Path.Combine(repository, "LexEnaTrs.Web", "LexEnaMeritumModel.edmx"));
        XmlNamespaceManager ns = new XmlNamespaceManager(model.NameTable);
        ns.AddNamespace("c", "http://schemas.microsoft.com/ado/2008/09/edm");
        ns.AddNamespace("s", "http://schemas.microsoft.com/ado/2009/02/edm/ssdl");
        ns.AddNamespace("m", "http://schemas.microsoft.com/ado/2008/09/mapping/cs");
        XmlNodeList conceptual = model.SelectNodes("//c:EntityType[@Name='BillingImportBatch']/c:Property", ns);
        XmlNodeList storage = model.SelectNodes("//s:EntityType[@Name='BillingImportBatch']/s:Property", ns);
        XmlNodeList mappings = model.SelectNodes("//m:EntitySetMapping[@Name='BillingImportBatch']//m:ScalarProperty", ns);
        string[] expected = { "Id", "Firma", "Name", "SourceFileName", "CreatedAt", "CreatedBy", "BranchId", "SymbolId", "SymbolName", "FirstNumber", "Year", "DocumentName", "BookingDate", "Status", "CaseCount", "ImportedCount", "TotalAmount", "ImportXml", "DistributionXml", "DistributionUpdatedAt", "DistributionState" };
        foreach (XmlNodeList nodes in new[] { conceptual, storage, mappings })
        {
            Equal(expected.Length, nodes.Count, "Unexpected schema field count.");
            Assert(expected.OrderBy(n => n).SequenceEqual(nodes.Cast<XmlNode>().Select(n => n.Attributes["Name"].Value).OrderBy(n => n)), "Missing mapped field.");
        }
        foreach (XmlNode node in conceptual.Cast<XmlNode>().Concat(storage.Cast<XmlNode>()))
        {
            bool optional = node.Attributes["Name"].Value == "DistributionXml" || node.Attributes["Name"].Value == "DistributionUpdatedAt";
            string nullable = node.Attributes["Nullable"] == null ? "true" : node.Attributes["Nullable"].Value;
            Equal(optional ? "true" : "false", nullable, "Incorrect batch field nullability.");
        }
        XmlNode id = model.SelectSingleNode("//s:EntityType[@Name='BillingImportBatch']/s:Property[@Name='Id']", ns);
        Assert(id.Attributes["StoreGeneratedPattern"] == null, "Batch GUID must be supplied by the application.");
        XmlNode amount = model.SelectSingleNode("//s:EntityType[@Name='BillingImportBatch']/s:Property[@Name='TotalAmount']", ns);
        Equal("18", amount.Attributes["Precision"].Value, "Incorrect amount precision.");
        Equal("2", amount.Attributes["Scale"].Value, "Incorrect amount scale.");
        foreach (string layer in new[] { "c", "s" })
        {
            XmlNode approvalState = model.SelectSingleNode("//" + layer + ":EntityType[@Name='BillingImportBatch']/" + layer + ":Property[@Name='DistributionState']", ns);
            Equal("Planning", approvalState.Attributes["DefaultValue"].Value, "New batches do not start in the planning state.");
            Equal("20", approvalState.Attributes["MaxLength"].Value, "Incorrect approval state storage length.");
            XmlNode officeSymbol = model.SelectSingleNode("//" + layer + ":EntityType[@Name='JednostkaWindykacji']/" + layer + ":Property[@Name='SymbolSprawWiena']", ns);
            Equal("10", officeSymbol.Attributes["MaxLength"].Value, "Configured office symbol exceeds Wiena's designation limit.");
            Equal("true", officeSymbol.Attributes["Nullable"].Value, "Existing office configurations cannot retain an unset symbol.");
        }
        Equal("Planning", new BillingImportBatch().DistributionState, "Entity default diverges from the database default.");
        string sql = File.ReadAllText(Path.Combine(repository, "SQL", "ImportBillingPackages", "001_BillingImportBatch.sql"));
        Assert(sql.Contains("CREATE UNIQUE NONCLUSTERED INDEX UX_BillingImportBatch_Firma_Name") && sql.Contains("(Firma, Name)"), "Missing unique batch name per company.");
        Assert(sql.Contains("IF OBJECT_ID(N'dbo.BillingImportBatch', N'U') IS NULL"), "Create script is not guarded.");
        string metadata = File.ReadAllText(Path.Combine(repository, "LexEnaTrs.Web", "LexEnaMeritumDomainService.metadata.cs"));
        Assert(System.Text.RegularExpressions.Regex.IsMatch(metadata, @"\[Exclude\]\s+public string ImportXml"), "Batch XML leaked into history metadata.");
    }

    private static void Reject(Action action, string message)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
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
