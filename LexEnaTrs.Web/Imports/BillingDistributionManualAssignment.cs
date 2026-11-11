using System;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using LexEnaTrs.Web.Models;

namespace LexEnaTrs.Web
{
    public partial class LexEnaMeritumDomainService
    {
        [Invoke, RequiresAuthentication]
        public string ChangeBillingDistributionOffice(Guid batchId, int firma, Guid proposalId, int wienaCaseId, int unitId)
        {
            try
            {
                BillingImportBatch batch = FindDistributionBatch(batchId, firma);
                if (batch.DistributionState != "Planning")
                    throw new InvalidOperationException("Nie można zmieniać kancelarii po rozpoczęciu zatwierdzania podziału.");
                BillingDistribution saved = RequireDistributionProposal(batch, proposalId);
                BillingDistribution current = LoadBillingDistribution(batch, firma);
                // Re-read eligibility and debtor history. A manual choice may override
                // history, but the browser cannot replace the historical facts or mapping.
                // Missing symbols may still be configured before final approval.
                ValidateDistributionProposalSnapshot(saved, current, false);
                ValidateManualDistributionOfficeSnapshot(saved, current, unitId);
                RestoreBillingDistribution(current, batch.DistributionXml);
                ApplyManualDistributionAssignment(current, wienaCaseId, unitId,
                    ServiceContext.User.Identity.Name, DateTime.Now);
                current.ProposalId = Guid.NewGuid();
                current.CalculatedAt = DateTime.Now;
                LoadDistributionSignaturePreview(current);
                SetDistributionApprovalAvailability(current, null);
                string xml = Utils.SerializeToString(current, typeof(BillingDistribution));
                int changed = ObjectContext.ExecuteStoreCommand(
                    "UPDATE dbo.BillingImportBatch SET DistributionXml = @xml, DistributionUpdatedAt = @updated " +
                    "WHERE Id = @id AND Firma = @firma AND Status IN (N'Completed', N'Partial') " +
                    "AND DistributionState = N'Planning' AND DistributionXml = @original",
                    XmlParameter("@xml", xml), new SqlParameter("@updated", current.CalculatedAt),
                    new SqlParameter("@id", batchId), new SqlParameter("@firma", firma), XmlParameter("@original", batch.DistributionXml));
                if (changed != 1)
                    throw new InvalidOperationException("Podział zmieniono lub rozpoczęto jego zatwierdzanie. Odśwież widok przed kolejną zmianą kancelarii.");
                return xml;
            }
            catch (Exception ex) { return BatchError(ex); }
        }

        private static void ApplyManualDistributionAssignment(BillingDistribution distribution, int wienaCaseId,
            int unitId, string login, DateTime changedAt)
        {
            if (distribution == null || distribution.ApprovalState != "Planning" || !distribution.HasSavedProposal ||
                distribution.ProposalId == Guid.Empty || distribution.Cases == null || distribution.Offices == null)
                throw new InvalidOperationException("Ręczna zmiana kancelarii jest dostępna tylko w zapisanej, niezatwierdzonej propozycji podziału.");
            if (wienaCaseId <= 0 || unitId <= 0)
                throw new InvalidOperationException("Wybierz sprawę i kancelarię.");
            var matches = distribution.Cases.Where(c => c.WienaCaseId == wienaCaseId).ToArray();
            if (matches.Length != 1)
                throw new InvalidOperationException("Wybrana sprawa nie należy do tej propozycji podziału.");
            BillingDistributionCase item = matches[0];
            if (!item.Eligible || item.StatusId != DistributionWaitingStatusId || item.Decreed || !item.AssignedUnitId.HasValue)
                throw new InvalidOperationException("Zmień kancelarię tylko dla sprawy ujętej w propozycji i nadal mającej status Wieny 85.");
            if (!String.IsNullOrWhiteSpace(item.AssignmentBlockReason) ||
                (!item.FixedUnitId.HasValue && (item.PreviousWienaCaseId.HasValue || item.PreviousWienaFirmaId.HasValue)))
                throw new InvalidOperationException("Dane wcześniejszej kancelarii wymagają wyjaśnienia. Odśwież podział po poprawieniu konfiguracji.");
            // Offices are loaded from configured participants, including those not
            // selected for the automatic split of this particular batch.
            var targets = distribution.Offices.Where(o => o.UnitId == unitId && o.WienaFirmaId > 0).ToArray();
            if (targets.Length != 1)
                throw new InvalidOperationException("Wybierz kancelarię oznaczoną w konfiguracji jako uczestniczącą w przydziale.");
            if (item.AssignedUnitId == unitId)
                throw new InvalidOperationException("Sprawa jest już przypisana do wybranej kancelarii.");
            string user = RequiredBatchText(login, 256, "użytkownik zmieniający kancelarię");
            BillingDistributionAllocator.ValidatePercentages(distribution);

            if (!targets[0].Selected)
            {
                NormalizeManualDistributionPercentages(distribution.Offices.Where(o => o.Selected).OrderBy(o => o.UnitId).ToArray());
                targets[0].Selected = true;
                targets[0].CountPercent = 0m;
                targets[0].AmountPercent = 0m;
            }
            item.AssignedUnitId = unitId;
            item.AssignedOfficeName = targets[0].Name;
            item.ManualAssignment = true;
            item.ManualAssignedAt = changedAt;
            item.ManualAssignedBy = user;
            // Recompute only the balance. Do not redistribute any other case, even
            // when the manual choice exceeds the requested count or amount targets.
            BillingDistributionAllocator.RefreshTotals(distribution);
        }

        private static void ValidateManualDistributionOfficeSnapshot(BillingDistribution saved, BillingDistribution current, int unitId)
        {
            var previous = saved.Offices.Where(o => o.UnitId == unitId).ToArray();
            var actual = current.Offices.Where(o => o.UnitId == unitId).ToArray();
            if (previous.Length != 1 || actual.Length != 1 || previous[0].WienaFirmaId != actual[0].WienaFirmaId ||
                !String.Equals((previous[0].SymbolSprawWiena ?? String.Empty).Trim(), (actual[0].SymbolSprawWiena ?? String.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Konfiguracja wybranej kancelarii zmieniła się od zapisania propozycji. Odśwież i ponownie zaproponuj podział.");
        }

        private static void NormalizeManualDistributionPercentages(BillingDistributionOffice[] selected)
        {
            // E.g. 3 x 33.33% is valid before adding a zero-share participant. Adjust
            // only the rounding remainder so the enlarged selection still sums to 100%.
            int countCents = (int)((100m - selected.Sum(o => o.CountPercent)) * 100m);
            int amountCents = (int)((100m - selected.Sum(o => o.AmountPercent)) * 100m);
            for (int i = 0; countCents != 0 || amountCents != 0; i++)
            {
                BillingDistributionOffice office = selected[i % selected.Length];
                if (countCents != 0)
                {
                    int sign = Math.Sign(countCents);
                    office.CountPercent += sign / 100m;
                    countCents -= sign;
                }
                if (amountCents != 0)
                {
                    int sign = Math.Sign(amountCents);
                    office.AmountPercent += sign / 100m;
                    amountCents -= sign;
                }
            }
        }
    }
}
