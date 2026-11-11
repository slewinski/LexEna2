using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LexEnaTrs.Web;
using LexEnaTrs.Web.Models;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace LexEnaTrs.Views
{
    public partial class BillingDistributionView : UserControl
    {
        private readonly Guid batchId;
        private readonly int firma;
        private BillingDistribution distribution;
        private bool loaded;
        private bool changedParameters;
        private bool updating;
        private bool confirmationOpen;
        private bool approvalOutcomeUnknown;
        private bool updatingManualSelection;
        private bool savedManualAssignments;
        private BillingDistributionOfficeInput[] officeInputs;
        private readonly CultureInfo polish = new CultureInfo("pl-PL");

        public BillingDistributionView(Guid batchId, int firma, string batchName)
        {
            InitializeComponent();
            this.batchId = batchId;
            this.firma = firma;
            BatchTitle.Text = "Podział paczki: " + batchName;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (loaded) return;
            loaded = true;
            RefreshDistribution();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (BusyIndicator.IsBusy || confirmationOpen) return;
            if (changedParameters)
            {
                RadWindow.Confirm("Zmienione udziały nie zostały jeszcze użyte do podziału. Wczytać ostatnią zapisaną propozycję i odświeżyć statusy?",
                    (s, args) => { if (args.DialogResult == true) RefreshDistribution(); });
                return;
            }
            RefreshDistribution();
        }

        private void RefreshDistribution()
        {
            BusyIndicator.BusyContent = "Odczyt statusów i zapisanego podziału...";
            BusyIndicator.IsBusy = true;
            UpdateButton();
            try
            {
                new LexEnaMeritumDomainContext().GetBillingDistribution(batchId, firma, result => ReceiveDistribution(result, false), null);
            }
            catch (Exception ex)
            {
                BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(ex, "Nie udało się odświeżyć zapisanego podziału.");
                UpdateButton();
            }
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (!CanEditDistribution) return;
            if (savedManualAssignments)
            {
                Guid proposalId = distribution.ProposalId;
                confirmationOpen = true;
                UpdateButton();
                RadWindow.Confirm("Nowa propozycja zastąpi zapisane ręczne zmiany kancelarii. Przeliczyć podział ponownie?",
                    (s, args) =>
                    {
                        confirmationOpen = false;
                        UpdateButton();
                        if (args.DialogResult == true && distribution != null && distribution.ProposalId == proposalId && CanEditDistribution)
                            CalculateDistribution();
                    });
                return;
            }
            CalculateDistribution();
        }

        private void CalculateDistribution()
        {
            if (!CanEditDistribution) return;
            string error;
            if (!TryApplyPercentages(out error)) { AlertMsg.Show(error); return; }
            var selected = distribution.Offices.Where(o => o.Selected).ToArray();
            if (selected.Length == 0) { AlertMsg.Show("Wybierz co najmniej jedną kancelarię."); return; }
            try { BillingDistributionAllocator.ValidatePercentages(distribution); }
            catch (Exception ex) { AlertMsg.Show(ex.Message); return; }
            BusyIndicator.BusyContent = "Obliczanie i zapis propozycji podziału...";
            BusyIndicator.IsBusy = true;
            UpdateButton();
            // Only participant IDs and requested proportions go to the server.
            // Eligibility, case amounts and the office mapping are read there afresh.
            new LexEnaMeritumDomainContext().CalculateBillingDistribution(batchId, firma,
                selected.Select(o => o.UnitId).ToArray(), selected.Select(o => o.CountPercent).ToArray(),
                selected.Select(o => o.AmountPercent).ToArray(),
                result => ReceiveDistribution(result, true), null);
        }

        private void SaveManualOffice_Click(object sender, RoutedEventArgs e)
        {
            if (!SaveManualOfficeButton.IsEnabled) return;
            var item = DistributionGrid.SelectedItem as BillingDistributionCase;
            var office = ManualOffice.SelectedItem as BillingDistributionOffice;
            if (item == null || office == null || !CanChangeOffice(item) || item.AssignedUnitId == office.UnitId) return;
            Guid proposalId = distribution.ProposalId;
            approvalOutcomeUnknown = true;
            BusyIndicator.BusyContent = "Zapisywanie zmiany kancelarii w propozycji...";
            BusyIndicator.IsBusy = true;
            UpdateButton();
            try
            {
                new LexEnaMeritumDomainContext().ChangeBillingDistributionOffice(batchId, firma, proposalId,
                    item.WienaCaseId, office.UnitId, result => ReceiveDistribution(result, false, false, true), null);
            }
            catch (Exception ex)
            {
                BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(ex, "Nie otrzymano potwierdzenia zmiany kancelarii. Trwa ponowny odczyt podziału.");
                RefreshDistribution();
            }
        }

        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            if (!CanEditDistribution) return;
            BillingDistributionAllocator.SetEqualPercentages(distribution);
            BindOfficeInputs();
            ParametersChanged(this, EventArgs.Empty);
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            if (!ApproveButton.IsEnabled || distribution == null) return;
            Guid proposalId = distribution.ProposalId;
            bool resuming = distribution.ApprovalState == "Processing";
            var assigned = distribution.Cases.Where(c => c.Eligible && c.AssignedUnitId.HasValue).ToArray();
            string text = String.Format(polish,
                "{0}\n\nSprawy: {1}\nKwota: {2:N2} zł\nRok numeracji: {3}\nOddział Wieny: {4}\n\nKancelarie i symbole:\n{5}\n\n{6}",
                resuming ? "Sprawdzić wynik rozpoczętej dekretacji i w razie potrzeby ją dokończyć?" : "Zatwierdzić pokazany podział i zadekretować sprawy w Wienie?",
                assigned.Length, assigned.Sum(c => c.Amount), distribution.NumberingYear, distribution.BranchId,
                String.Join("\n", distribution.Offices.Where(o => assigned.Any(c => c.AssignedUnitId == o.UnitId))
                    .Select(o => o.Name + " — " + o.SymbolSprawWiena)),
                resuming ? "Jeśli zapis już się zakończył, system odczyta jego wynik bez ponownego nadawania numerów."
                    : "Sprawy otrzymają wskazane kancelarie oraz nowe sygnatury, z kolejnymi numerami w ramach symbolu, oddziału i roku. Serwer ponownie sprawdzi aktualne statusy i przypisania przed zapisem.");
            confirmationOpen = true;
            UpdateButton();
            RadWindow.Confirm(new DialogParameters
            {
                Header = resuming ? "Dokończenie dekretacji" : "Zatwierdzenie podziału",
                Content = new ScrollViewer
                {
                    MaxHeight = 420,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Content = new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap, MaxWidth = 560 }
                },
                OkButtonContent = resuming ? "Sprawdź / dokończ" : "Zatwierdź i zadekretuj",
                CancelButtonContent = "Anuluj",
                Closed = (s, args) =>
                {
                    confirmationOpen = false;
                    UpdateButton();
                    if (args.DialogResult != true || distribution == null || distribution.ProposalId != proposalId || !ApproveButton.IsEnabled) return;
                    approvalOutcomeUnknown = true;
                    BusyIndicator.BusyContent = "Zatwierdzanie podziału i dekretacja spraw w Wienie...";
                    BusyIndicator.IsBusy = true;
                    UpdateButton();
                    try
                    {
                        new LexEnaMeritumDomainContext().ApproveBillingDistribution(batchId, firma, proposalId,
                            result => ReceiveDistribution(result, false, true), null);
                    }
                    catch (Exception ex)
                    {
                        BusyIndicator.IsBusy = false;
                        ErrorWindow.CreateNew(ex, "Nie otrzymano potwierdzenia dekretacji. Odśwież stan paczki.");
                        UpdateButton();
                    }
                }
            });
        }

        private void ReceiveDistribution(InvokeOperation<string> result, bool calculated, bool approvalRequest = false, bool manualRequest = false)
        {
            BusyIndicator.IsBusy = false;
            if (result.HasError)
            {
                result.MarkErrorAsHandled();
                ErrorWindow.CreateNew(result.Error, approvalRequest || manualRequest
                    ? "Nie otrzymano potwierdzenia zapisu. Trwa ponowny odczyt stanu paczki."
                    : "Nie udało się odczytać lub zapisać podziału paczki.");
                if (approvalRequest || manualRequest) RefreshDistribution();
                else UpdateButton();
                return;
            }
            if (String.IsNullOrWhiteSpace(result.Value) || result.Value.StartsWith("Błąd", StringComparison.OrdinalIgnoreCase))
            {
                ErrorWindow.CreateNew(String.IsNullOrWhiteSpace(result.Value) ? "Brak wyniku podziału paczki." : result.Value);
                if (approvalRequest || manualRequest) RefreshDistribution();
                else UpdateButton();
                return;
            }
            try
            {
                var value = (BillingDistribution)ToXMLSerializers.XmlDeserializeFromString(result.Value, typeof(BillingDistribution));
                if (value == null || value.BatchId != batchId || value.Offices == null || value.Cases == null)
                    throw new InvalidOperationException("Serwer zwrócił nieprawidłowe dane podziału.");
                bool addedManualOffice = manualRequest && distribution != null && value.Offices.Any(o => o.Selected &&
                    !distribution.Offices.Any(previous => previous.UnitId == o.UnitId && previous.Selected));
                distribution = value;
                approvalOutcomeUnknown = false;
                changedParameters = false;
                savedManualAssignments = value.Cases.Any(c => c.ManualAssignment);
                BindOfficeInputs();
                BindResults();
                if (value.ApprovalState == "Approved") ResultInfo.Text = String.Format(polish,
                    "Podział zatwierdzono {0:yyyy-MM-dd HH:mm}. Zatwierdzający: {1}. Widok przedstawia zapisany wynik dekretacji; tabela zawiera stare i nowe sygnatury.",
                    value.ApprovedAt, String.IsNullOrWhiteSpace(value.ApprovedBy) ? "brak danych" : value.ApprovedBy);
                else if (value.ApprovalState == "Processing") ResultInfo.Text = "Dekretacja została rozpoczęta. Udziały i przypisania są zablokowane. Użyj „Sprawdź / dokończ dekretację”, aby odczytać wynik lub bezpiecznie dokończyć tę samą operację.";
                else if (manualRequest) ResultInfo.Text = "Zapisano ręczną zmianę kancelarii w propozycji LexEna. Bilans liczby spraw i kwot został zaktualizowany; pozostałe przypisania zachowano. Zapis do Wieny nastąpi po zatwierdzeniu podziału.";
                else if (calculated) ResultInfo.Text = "Zapisano propozycję podziału przy paczce w LexEna. Sprawdź przypisania, symbole i kwoty, a następnie zatwierdź podział, aby zadekretować sprawy w Wienie. Zmiana udziałów i kliknięcie „Zaproponuj podział” obliczy nowy wynik.";
                else if (value.HasSavedProposal) ResultInfo.Text = String.Format(polish,
                    "Propozycja z {0:yyyy-MM-dd HH:mm}. Odczytano aktualne statusy z Wieny i pominięto niekwalifikujące się sprawy. Nie wykonano nowego podziału. Aby go przeliczyć, kliknij „Zaproponuj podział”.", value.CalculatedAt);
                else ResultInfo.Text = "Wybrano wszystkie kancelarie dopuszczone w konfiguracji i ustawiono równe udziały. Podział nie został jeszcze wykonany. Uruchom go przyciskiem „Zaproponuj podział” po zakończeniu weryfikacji spraw w Wienie.";
                if (!IsDistributionLocked && value.Offices.Length == 0) ResultInfo.Text = "Brak kancelarii dopuszczonych do przydziału. Uzupełnij mapowanie do Wieny i zaznacz „Uczestniczy w przydziale” w konfiguracji jednostek windykacji.";
                if (addedManualOffice) ResultInfo.Text += " Nowo włączona kancelaria ma udziały 0%; przekazana jej sprawa jest uwzględniona w bilansie.";
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, "Nie można odczytać wyniku podziału.");
                if (approvalRequest || manualRequest) { RefreshDistribution(); return; }
            }
            UpdateButton();
        }

        private void BindResults()
        {
            var previous = DistributionGrid.SelectedItem as BillingDistributionCase;
            updatingManualSelection = true;
            DistributionGrid.ItemsSource = null;
            DistributionGrid.ItemsSource = distribution.Cases;
            if (previous != null)
                DistributionGrid.SelectedItem = distribution.Cases.FirstOrDefault(c => previous.WienaCaseId > 0
                    ? c.WienaCaseId == previous.WienaCaseId : c.WienaCaseId == 0 && c.BatchRowNumber == previous.BatchRowNumber);
            // Selecting a row only prepares the editor; it never calculates or saves a division.
            if (DistributionGrid.SelectedItem == null)
                DistributionGrid.SelectedItem = distribution.Cases.FirstOrDefault(CanChangeOffice)
                    ?? distribution.Cases.FirstOrDefault(c => c.Eligible);
            ManualOffice.ItemsSource = distribution.Offices;
            SelectCurrentManualOffice();
            updatingManualSelection = false;
            foreach (var input in officeInputs) input.RefreshTotals();
            UpdateSummary();
            UpdateManualEditor();
        }

        private void UpdateSummary()
        {
            int eligible = distribution.Cases.Count(c => c.Eligible);
            Summary.Text = String.Format(polish,
                "Sprawy w bilansie: {0}  |  Kwota w bilansie: {1:N2} zł  |  Pominięte: {2}  |  Przydzielone: {3}  |  Wybrane kancelarie: {4}",
                eligible, distribution.Cases.Where(c => c.Eligible).Sum(c => c.Amount),
                distribution.Cases.Length - eligible, distribution.Cases.Count(c => c.Eligible && c.AssignedUnitId.HasValue),
                distribution.Offices.Count(o => o.Selected));
            Summary.Text += String.Format(polish, "  |  Ręczne przypisania: {0}", distribution.Cases.Count(c => c.Eligible && c.ManualAssignment));
            if (distribution.ApprovalState == "Approved") Summary.Text += String.Format(polish,
                "  |  Zadekretowane: {0}", distribution.Cases.Count(c => c.Decreed));
            int fixedCases = distribution.Cases.Count(c => c.Eligible && c.FixedUnitId.HasValue && !c.ManualAssignment);
            int blockedCases = distribution.Cases.Count(c => c.Eligible && !String.IsNullOrWhiteSpace(c.AssignmentBlockReason));
            FixedAssignmentInfo.Text = blockedCases > 0
                ? String.Format(polish, "Sprawy wymagające wyjaśnienia: {0}. Sprawdź przyczyny w kolumnie „Wymaga wyjaśnienia”; po ich usunięciu odśwież widok.", blockedCases)
                : fixedCases > 0 ? String.Format(polish, "Przypisania z historii: {0} spraw, {1:N2} zł — uwzględnione w bilansie i zachowywane przez automatyczny podział. Ręczna zmiana kancelarii jest możliwa przed zatwierdzeniem.",
                    fixedCases, distribution.Cases.Where(c => c.Eligible && c.FixedUnitId.HasValue && !c.ManualAssignment).Sum(c => c.Amount)) : String.Empty;
            string error;
            if (!IsDistributionLocked && !TryApplyPercentages(out error)) { PercentSummary.Text = error; return; }
            PercentSummary.Text = String.Format(polish, "Suma udziałów wybranych kancelarii — liczba: {0:0.##}%  |  wartość: {1:0.##}%",
                distribution.Offices.Where(o => o.Selected).Sum(o => o.CountPercent),
                distribution.Offices.Where(o => o.Selected).Sum(o => o.AmountPercent));
            if (!IsDistributionLocked)
            {
                try { BillingDistributionAllocator.ValidatePercentages(distribution); }
                catch (Exception ex) { PercentSummary.Text += "  |  " + ex.Message; }
            }
        }

        private void BindOfficeInputs()
        {
            updating = true;
            if (officeInputs != null)
                foreach (var input in officeInputs) input.Edited -= ParametersChanged;
            var requiredUnits = distribution.Cases.Where(c => c.Eligible && c.FixedUnitId.HasValue)
                .Select(c => c.FixedUnitId.Value).ToList();
            officeInputs = distribution.Offices.Select(o => new BillingDistributionOfficeInput(o, requiredUnits.Contains(o.UnitId))).ToArray();
            foreach (var input in officeInputs) input.Edited += ParametersChanged;
            OfficesGrid.ItemsSource = officeInputs;
            updating = false;
        }

        private void PercentText_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Binding and recycled grid cells also raise TextChanged. They must not invalidate
            // a saved proposal (which would clear assignments and disable the manual editor).
            // OneWay binding also prevents a later LostFocus from writing a recycled value.
            var textBox = sender as TextBox;
            if (textBox == null || updating || !CanEditDistribution || FocusManager.GetFocusedElement() != textBox) return;
            var input = textBox.DataContext as BillingDistributionOfficeInput;
            if (input == null || officeInputs == null || !officeInputs.Contains(input)) return;
            // Keep incomplete/invalid user input so a click cannot submit an old number.
            if (Object.Equals(textBox.Tag, "Count")) input.CountPercentText = textBox.Text;
            else if (Object.Equals(textBox.Tag, "Amount")) input.AmountPercentText = textBox.Text;
        }

        private bool TryApplyPercentages(out string error)
        {
            error = null;
            if (officeInputs == null) return true;
            foreach (var input in officeInputs.Where(i => i.Selected))
            {
                decimal count, amount;
                if (!TryParsePercent(input.CountPercentText, out count) || !TryParsePercent(input.AmountPercentText, out amount))
                {
                    error = "Kancelaria „" + input.Office.Name + "”: wpisz udziały od 0 do 100, maksymalnie z dwoma miejscami po przecinku.";
                    return false;
                }
            }
            foreach (var input in officeInputs.Where(i => i.Selected))
            {
                decimal count, amount;
                TryParsePercent(input.CountPercentText, out count);
                TryParsePercent(input.AmountPercentText, out amount);
                input.Office.CountPercent = count;
                input.Office.AmountPercent = amount;
            }
            return true;
        }

        private static bool TryParsePercent(string text, out decimal value)
        {
            string normalized = (text ?? String.Empty).Trim().Replace(',', '.');
            return Decimal.TryParse(normalized, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out value) && value >= 0m && value <= 100m && Decimal.Round(value, 2) == value;
        }

        private void ParametersChanged(object sender, EventArgs e)
        {
            if (updating || !CanEditDistribution) return;
            changedParameters = true;
            foreach (var row in distribution.Cases)
            {
                row.AssignedUnitId = null;
                row.AssignedOfficeName = null;
                row.ManualAssignment = false;
                row.ManualAssignedAt = null;
                row.ManualAssignedBy = null;
                row.DecreeSignature = null;
                row.DecreeSignatureIssue = null;
            }
            string error;
            bool valid = TryApplyPercentages(out error);
            if (valid)
            {
                try { BillingDistributionAllocator.ValidatePercentages(distribution); }
                catch (Exception) { valid = false; }
            }
            if (valid) BillingDistributionAllocator.RefreshTotals(distribution);
            else foreach (var office in distribution.Offices)
            {
                // Do not show targets based on an old number while the textbox is invalid.
                office.CaseCount = 0;
                office.TargetCaseCount = 0;
                office.TotalAmount = 0m;
                office.TargetAmount = 0m;
                office.AmountDeviation = 0m;
            }
            BindResults();
            ResultInfo.Text = "Zmieniono parametry. Nie wykonano nowego podziału. Kliknij „Zaproponuj podział”, aby obliczyć i zapisać wynik dla aktualnych udziałów.";
            UpdateButton();
        }

        private void UpdateButton()
        {
            bool idle = !BusyIndicator.IsBusy && !confirmationOpen;
            RefreshButton.IsEnabled = idle;
            OfficesGrid.IsEnabled = CanEditDistribution;
            CalculateButton.IsEnabled = CanEditDistribution && distribution.Offices.Any(o => o.Selected) &&
                distribution.Cases.Any(c => c.Eligible) && !distribution.Cases.Any(c => c.Eligible && !String.IsNullOrWhiteSpace(c.AssignmentBlockReason));
            EqualButton.IsEnabled = CanEditDistribution && distribution.Offices.Any(o => o.Selected);
            ApproveButton.IsEnabled = idle && !approvalOutcomeUnknown && distribution != null && distribution.CanApprove &&
                !changedParameters && distribution.ProposalId != Guid.Empty && distribution.ApprovalState != "Approved";
            ApproveButton.Content = distribution != null && distribution.ApprovalState == "Processing"
                ? "Sprawdź / dokończ dekretację" : "Zatwierdź i zadekretuj w Wienie";
            if (approvalOutcomeUnknown)
                ApprovalInfo.Text = "Wynik zapisu nie został jeszcze odczytany. Odśwież stan paczki przed kolejną operacją.";
            else if (distribution == null) ApprovalInfo.Text = String.Empty;
            else if (changedParameters) ApprovalInfo.Text = "Przed zatwierdzeniem zaproponuj podział dla zmienionych udziałów.";
            else if (!distribution.CanApprove && !String.IsNullOrWhiteSpace(distribution.ApprovalProblem))
                ApprovalInfo.Text = distribution.ApprovalProblem;
            else if (distribution.ApprovalState == "Approved") ApprovalInfo.Text = "Dekretacja zakończona. Zapisany podział jest zamknięty do zmian.";
            else ApprovalInfo.Text = String.Format(polish,
                "Nowe sygnatury: rok {0}, oddział Wieny {1}. Numery zostaną nadane podczas zatwierdzania, po ostatnim numerze każdego symbolu.",
                distribution.NumberingYear, distribution.BranchId);
            UpdateManualEditor();
        }

        private void DistributionGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (updatingManualSelection || ManualOffice == null) return;
            updatingManualSelection = true;
            SelectCurrentManualOffice();
            updatingManualSelection = false;
            UpdateManualEditor();
        }

        private void ChooseManualCase_Click(object sender, RoutedEventArgs e)
        {
            var source = sender as FrameworkElement;
            var item = source == null ? null : source.DataContext as BillingDistributionCase;
            if (item == null || distribution == null || !distribution.Cases.Contains(item)) return;
            DistributionGrid.SelectedItem = item;
            updatingManualSelection = true;
            SelectCurrentManualOffice();
            updatingManualSelection = false;
            UpdateManualEditor();
            if (CanChangeOffice(item))
            {
                ManualOffice.Focus();
                ManualOffice.IsDropDownOpen = true;
            }
            else AlertMsg.Show(ManualOfficeInfo.Text);
        }

        private void ManualOffice_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!updatingManualSelection) UpdateManualEditor();
        }

        private void SelectCurrentManualOffice()
        {
            var item = DistributionGrid.SelectedItem as BillingDistributionCase;
            ManualOffice.SelectedItem = distribution == null || item == null ? null : distribution.Offices
                .FirstOrDefault(o => item.AssignedUnitId == o.UnitId);
        }

        private bool CanChangeOffice(BillingDistributionCase item)
        {
            return CanEditDistribution && !changedParameters && distribution.ApprovalState == "Planning" &&
                distribution.HasSavedProposal && distribution.ProposalId != Guid.Empty && item != null &&
                item.WienaCaseId > 0 && item.Eligible && item.StatusId == 85 && !item.Decreed &&
                item.AssignedUnitId.HasValue &&
                String.IsNullOrWhiteSpace(item.AssignmentBlockReason);
        }

        private void UpdateManualEditor()
        {
            if (ManualOffice == null || SaveManualOfficeButton == null || ManualOfficeInfo == null || ManualCaseInfo == null || DistributionGrid == null) return;
            var item = DistributionGrid.SelectedItem as BillingDistributionCase;
            var office = ManualOffice.SelectedItem as BillingDistributionOffice;
            bool canChange = CanChangeOffice(item);
            ManualOffice.IsEnabled = canChange;
            SaveManualOfficeButton.IsEnabled = canChange && office != null && item.AssignedUnitId != office.UnitId;
            ManualCaseInfo.Text = item == null ? "Wybierz sprawę w tabeli lub kliknij w jej wierszu „Zmień kancelarię”."
                : "Wybrana sprawa: " + (String.IsNullOrWhiteSpace(item.SignatureBeforeApproval) ? item.Signature : item.SignatureBeforeApproval)
                    + "  |  Kancelaria: " + (item.AssignedOfficeName ?? "brak przypisania");
            if (approvalOutcomeUnknown)
                ManualOfficeInfo.Text = "Najpierw odśwież wynik ostatniego zapisu.";
            else if (IsDistributionLocked)
                ManualOfficeInfo.Text = "Podział jest zatwierdzony lub w trakcie dekretacji. Przypisań nie można zmieniać.";
            else if (BusyIndicator.IsBusy || confirmationOpen)
                ManualOfficeInfo.Text = "Poczekaj na zakończenie bieżącej operacji.";
            else if (changedParameters)
                ManualOfficeInfo.Text = "Zmieniono udziały kancelarii. Najpierw zaproponuj i zapisz nowy podział.";
            else if (distribution == null || !distribution.HasSavedProposal)
                ManualOfficeInfo.Text = "Najpierw kliknij „Zaproponuj podział”. Sam zapis paczki w Wienie nie tworzy propozycji przypisania kancelarii.";
            else if (distribution.ProposalId == Guid.Empty || distribution.ApprovalState != "Planning")
                ManualOfficeInfo.Text = "Wczytano starszą propozycję podziału. Kliknij „Zaproponuj podział”, aby przygotować ją do ręcznych zmian i dekretacji.";
            else if (item == null)
                ManualOfficeInfo.Text = "Zmiana dotyczy jednej sprawy i przelicza bilans kancelarii. Możesz wybrać dowolną kancelarię dopuszczoną w konfiguracji, również zamiast wskazanej przez historię dłużnika.";
            else if (!item.Eligible || item.StatusId != 85 || item.Decreed)
                ManualOfficeInfo.Text = "Ta sprawa nie kwalifikuje się do ręcznej zmiany kancelarii w podziale.";
            else if (!String.IsNullOrWhiteSpace(item.AssignmentBlockReason))
                ManualOfficeInfo.Text = item.AssignmentBlockReason;
            else if (!item.AssignedUnitId.HasValue)
                ManualOfficeInfo.Text = "Sprawa nie ma aktualnego przypisania. Odśwież dane i ponownie zaproponuj podział.";
            else if (item.FixedUnitId.HasValue || item.PreviousWienaCaseId.HasValue || item.PreviousWienaFirmaId.HasValue)
                ManualOfficeInfo.Text = "Historia dłużnika wskazuje kancelarię „" + (item.PreviousOfficeName ?? item.AssignedOfficeName) + "”. Możesz ręcznie wybrać inną; historyczne powiązanie pozostanie widoczne w tabeli.";
            else
                ManualOfficeInfo.Text = "Wybierz inną kancelarię dopuszczoną w konfiguracji i zapisz zmianę. Pozostałe sprawy zachowają swoje przypisania.";
        }

        private bool IsDistributionLocked
        {
            get { return distribution != null && (distribution.ApprovalState == "Processing" || distribution.ApprovalState == "Approved"); }
        }

        private bool CanEditDistribution
        {
            get { return distribution != null && !BusyIndicator.IsBusy && !confirmationOpen && !approvalOutcomeUnknown && !IsDistributionLocked; }
        }

        private void DistributionGrid_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            var row = e.Row as GridViewRow;
            if (row == null) return;
            var item = row.Item as BillingDistributionCase;
            // Reset on every load as Telerik recycles row containers during scrolling.
            row.Opacity = item != null && !item.Eligible ? 0.45 : 1.0;
        }
    }

    public sealed class BillingDistributionOfficeInput : INotifyPropertyChanged
    {
        private string countPercentText;
        private string amountPercentText;
        private readonly bool required;
        public BillingDistributionOffice Office { get; private set; }
        public bool CanChangeSelection { get { return !required; } }
        public string SelectionHint { get { return required ? "Kancelaria wymagana przez historię dłużników przy automatycznym podziale." : "Udział kancelarii w podziale tej paczki."; } }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Edited;

        public BillingDistributionOfficeInput(BillingDistributionOffice office, bool required)
        {
            Office = office;
            this.required = required;
            countPercentText = office.CountPercent.ToString("0.##", new CultureInfo("pl-PL"));
            amountPercentText = office.AmountPercent.ToString("0.##", new CultureInfo("pl-PL"));
        }

        public bool Selected
        {
            get { return Office.Selected; }
            set { if (Office.Selected == value || (required && !value)) return; Office.Selected = value; Changed("Selected"); }
        }
        public string CountPercentText
        {
            get { return countPercentText; }
            set { if (countPercentText == value) return; countPercentText = value; Changed("CountPercentText"); }
        }
        public string AmountPercentText
        {
            get { return amountPercentText; }
            set { if (amountPercentText == value) return; amountPercentText = value; Changed("AmountPercentText"); }
        }
        public void RefreshTotals()
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Office"));
        }
        private void Changed(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            if (Edited != null) Edited(this, EventArgs.Empty);
        }
    }
}
