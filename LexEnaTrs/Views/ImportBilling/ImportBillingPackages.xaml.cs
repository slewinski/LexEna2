using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using LexEnaTrs.Web;
using LexEnaTrs.Web.Models;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace LexEnaTrs.Views
{
    public partial class ImportBillingPackages : UserControl
    {
        private Guid newBatchId;
        private BillingImportBatch currentBatch;
        private ObservableCollection<SprImportDescriptor> rows;
        private bool loaded;
        private bool restoring;
        private bool dirty;
        private bool registerAttempted;
        private bool updatingBatchSelection;
        private int batchOpenVersion;
        private int historyRequestVersion;
        private readonly CultureInfo polish = new CultureInfo("pl-PL");

        public ImportBillingPackages()
        {
            InitializeComponent();
            BatchYear.NumberFormatInfo = new NumberFormatInfo { NumberGroupSeparator = "" };
            ((GridViewComboBoxColumn)CasesGrid.Columns["InterestType"]).ItemsSource = rodzajeOdsetek.GetRodzajOdsetek();
            NewBatch();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (loaded) return;
            loaded = true;
            RefreshSymbols(() => RefreshHistory(null));
        }

        private void NewBatch()
        {
            ++batchOpenVersion;
            BusyIndicator.IsBusy = false;
            SelectHistoryBatch(null);
            restoring = true;
            DetachRows();
            newBatchId = Guid.NewGuid();
            currentBatch = null;
            rows = new ObservableCollection<SprImportDescriptor>();
            CasesGrid.ItemsSource = rows;
            dirty = false;
            registerAttempted = false;
            BatchName.Text = "";
            BatchYear.Value = DateTime.Today.Year;
            BookingDate.SelectedValue = DateTime.Today;
            Branch.SelectedIndex = -1;
            Symbol.ItemsSource = null;
            FirstNumber.Text = "";
            DocumentType.SelectedIndex = -1;
            BatchInfo.Text = "";
            Summary.Text = "";
            StateInfo.Text = "Nadaj paczce własne oznaczenie i wybierz sygnaturę poczekalni. Paczka zostanie zachowana po wczytaniu XML.";
            restoring = false;
            SetEditingState();
        }

        private void SetEditingState()
        {
            bool draft = currentBatch != null && currentBatch.Status == "Draft" && !registerAttempted;
            BatchSettings.IsEnabled = currentBatch == null;
            CasesGrid.IsReadOnly = !draft;
            SaveDraftButton.IsEnabled = draft;
            RegisterButton.IsEnabled = draft && rows != null && rows.Any(r => r.czyrej && r.CanImport);
            DeleteBatchButton.IsEnabled = CanDeleteCurrentBatch();
            DistributionButton.IsEnabled = currentBatch != null &&
                (currentBatch.Status == "Completed" || currentBatch.Status == "Partial");
        }

        private void Distribution_Click(object sender, RoutedEventArgs e)
        {
            if (currentBatch == null || (currentBatch.Status != "Completed" && currentBatch.Status != "Partial")) return;
            Guid openedBatchId = currentBatch.Id;
            int openedBatchVersion = batchOpenVersion;
            var window = new RadWindow
            {
                Header = "Podział spraw na kancelarie",
                Content = new BillingDistributionView(currentBatch.Id, LexEnaKonfiguracja.Firma, currentBatch.Name),
                Width = Math.Max(720, Math.Min(1180, Application.Current.Host.Content.ActualWidth - 40)),
                Height = Math.Max(500, Math.Min(760, Application.Current.Host.Content.ActualHeight - 40)),
                WindowStartupLocation = Telerik.Windows.Controls.WindowStartupLocation.CenterScreen
            };
            window.Closed += (s, args) =>
            {
                // Refresh the selected package only if it is still the package whose
                // distribution was opened. The existing request guards protect a
                // subsequent selection while the history request is in flight.
                bool sameBatch = openedBatchVersion == batchOpenVersion && currentBatch != null &&
                    currentBatch.Id == openedBatchId && !dirty;
                RefreshHistory(sameBatch ? (Guid?)openedBatchId : null);
            };
            window.ShowDialog();
        }

        private void WithDiscardConfirmation(Action action, Action cancelled = null)
        {
            CasesGrid.CommitEdit();
            if (!dirty) { action(); return; }
            RadWindow.Confirm("Wybór spraw lub odsetek nie został zapisany w paczce. Czy pominąć te zmiany?",
                (sender, args) =>
                {
                    if (args.DialogResult == true) action();
                    else if (cancelled != null) cancelled();
                });
        }

        private void NewBatch_Click(object sender, RoutedEventArgs e)
        {
            WithDiscardConfirmation(() => { NewBatch(); RefreshSymbols(null); });
        }

        private bool CanDeleteCurrentBatch()
        {
            return currentBatch != null && currentBatch.Status == "Draft" && !registerAttempted &&
                currentBatch.ImportedCount == 0 && rows != null && rows.All(r => !r.czy_ok && r.WienaCaseId == 0);
        }

        private void DeleteBatch_Click(object sender, RoutedEventArgs e)
        {
            if (BusyIndicator.IsBusy || !CanDeleteCurrentBatch()) return;
            CasesGrid.CommitEdit();
            Guid id = currentBatch.Id;
            string name = currentBatch.Name;
            int firma = LexEnaKonfiguracja.Firma;
            RadWindow.Confirm("Usunąć paczkę „" + name + "” wraz z wczytanym XML i wyborem spraw? Operacji nie można cofnąć.",
                (sender2, args) =>
                {
                    if (args.DialogResult != true || !CanDeleteCurrentBatch() || currentBatch.Id != id) return;
                    int requestVersion = ++batchOpenVersion;
                    ++historyRequestVersion;
                    BusyIndicator.IsBusy = true;
                    DeleteBatchButton.IsEnabled = false;
                    new LexEnaMeritumDomainContext().DeleteBillingImportBatch(id, firma, result =>
                    {
                        if (requestVersion != batchOpenVersion)
                        {
                            if (result.HasError) result.MarkErrorAsHandled();
                            return;
                        }
                        string value;
                        if (!TryReadResult(result, out value)) { SetEditingState(); return; }
                        if (value != "OK")
                        {
                            ErrorWindow.CreateNew("Nie potwierdzono usunięcia paczki. Odśwież historię, aby sprawdzić jej stan.");
                            SetEditingState();
                            return;
                        }
                        NewBatch();
                        // Remove the row immediately, even if the subsequent history refresh fails.
                        var history = BatchGrid.ItemsSource as IEnumerable<BatchListRow>;
                        updatingBatchSelection = true;
                        try
                        {
                            if (history != null) BatchGrid.ItemsSource = history.Where(b => b.Batch.Id != id).ToList();
                            BatchGrid.SelectedItem = null;
                        }
                        finally { updatingBatchSelection = false; }
                        StateInfo.Text = "Usunięto paczkę „" + name + "”. Możesz utworzyć nową paczkę.";
                        RefreshHistory(null);
                    }, null);
                });
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            // Refreshing the list must not discard unsaved selections in the open draft.
            if (currentBatch != null && !dirty) RefreshHistory(currentBatch.Id);
            else if (currentBatch == null) RefreshSymbols(() => RefreshHistory(null));
            else RefreshHistory(null);
        }

        private void RefreshSymbols(Action completed)
        {
            BusyIndicator.IsBusy = true;
            new PozewDomainContext().GetWienaSymbole(LexEnaKonfiguracja.Firma, result =>
            {
                string xml;
                if (TryReadResult(result, out xml))
                {
                    try
                    {
                        var values = (List<TypSlownikFiltered>)ToXMLSerializers.XmlDeserializeFromString(xml, typeof(List<TypSlownikFiltered>));
                        LexEnaKonfiguracja.SlownikSymboliWiena = new ObservableCollection<typSlownikFilter>(
                            values.Select(v => new typSlownikFilter(v.Nazwa, v.Numer, v.Filter1, v.Filter2, v.Filter3)));
                        var branch = Branch.SelectedItem as typSlownikFilter;
                        if (currentBatch == null && branch != null)
                        {
                            object previous = Symbol.SelectedValue;
                            ReloadSymbols(branch.Numer, (int)(BatchYear.Value ?? DateTime.Today.Year));
                            Symbol.SelectedValue = previous;
                        }
                    }
                    catch (Exception ex) { ErrorWindow.CreateNew(ex, "Nie można odczytać symboli spraw z Wieny."); }
                }
                if (completed != null) completed();
            }, null);
        }

        private void RefreshHistory(Guid? batchToOpen)
        {
            int requestVersion = ++historyRequestVersion;
            int openVersion = batchOpenVersion;
            BusyIndicator.IsBusy = true;
            var context = new LexEnaMeritumDomainContext();
            context.Load(context.GetBillingImportBatchesQuery(LexEnaKonfiguracja.Firma), operation =>
            {
                if (requestVersion != historyRequestVersion || openVersion != batchOpenVersion)
                {
                    if (operation.HasError) operation.MarkErrorAsHandled();
                    return;
                }
                BusyIndicator.IsBusy = false;
                if (operation.HasError)
                {
                    operation.MarkErrorAsHandled();
                    ErrorWindow.CreateNew(operation.Error, "Nie można odczytać historii paczek.");
                    return;
                }
                var history = operation.Entities.Select(b => new BatchListRow(b)).ToList();
                Guid? selectedId = batchToOpen ?? (currentBatch == null ? (Guid?)null : currentBatch.Id);
                var selected = history.FirstOrDefault(b => b.Batch.Id == selectedId);
                // Rebinding history must not reopen the draft or discard its local edits.
                updatingBatchSelection = true;
                try
                {
                    BatchGrid.ItemsSource = history;
                    BatchGrid.SelectedItem = selected;
                }
                finally { updatingBatchSelection = false; }
                if (batchToOpen.HasValue && selected != null) OpenBatch(selected.Batch);
            }, null);
        }

        private void SelectHistoryBatch(Guid? id)
        {
            var history = BatchGrid.ItemsSource as IEnumerable<BatchListRow>;
            updatingBatchSelection = true;
            try { BatchGrid.SelectedItem = history == null ? null : history.FirstOrDefault(b => b.Batch.Id == id); }
            finally { updatingBatchSelection = false; }
        }

        private void RestoreCurrentBatchSelection()
        {
            SelectHistoryBatch(currentBatch == null ? (Guid?)null : currentBatch.Id);
        }

        private void BatchGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (updatingBatchSelection) return;
            var selected = BatchGrid.SelectedItem as BatchListRow;
            if (selected == null) return;
            WithDiscardConfirmation(() => OpenBatch(selected.Batch), RestoreCurrentBatchSelection);
        }

        private void OpenBatch_Click(object sender, RoutedEventArgs e)
        {
            var selected = BatchGrid.SelectedItem as BatchListRow;
            if (selected == null) { AlertMsg.Show("Wybierz paczkę z historii."); return; }
            WithDiscardConfirmation(() => OpenBatch(selected.Batch), RestoreCurrentBatchSelection);
        }

        private void OpenBatch(BillingImportBatch batch)
        {
            int requestVersion = ++batchOpenVersion;
            BusyIndicator.IsBusy = true;
            var context = new LexEnaMeritumDomainContext();
            context.GetBillingImportBatchXml(batch.Id, LexEnaKonfiguracja.Firma, result =>
            {
                // A slower response must not replace a more recently selected batch.
                if (requestVersion != batchOpenVersion)
                {
                    if (result.HasError) result.MarkErrorAsHandled();
                    return;
                }
                string xml;
                if (!TryReadResult(result, out xml)) { RestoreCurrentBatchSelection(); return; }
                try
                {
                    var parsed = Deserialize(xml);
                    currentBatch = batch;
                    registerAttempted = batch.Status != "Draft";
                    restoring = true;
                    BatchName.Text = batch.Name;
                    BatchYear.Value = batch.Year;
                    BookingDate.SelectedValue = batch.BookingDate;
                    Branch.SelectedValue = batch.BranchId;
                    ReloadSymbols(batch.BranchId, batch.Year);
                    Symbol.SelectedValue = batch.SymbolId;
                    // Historic settings remain visible even if the dictionary entry was removed.
                    if (Symbol.SelectedItem == null)
                        Symbol.ItemsSource = new[] { new typSlownikFilter { Numer = batch.SymbolId, Nazwa = batch.SymbolName } };
                    Symbol.SelectedValue = batch.SymbolId;
                    FirstNumber.Text = batch.FirstNumber.ToString(polish);
                    DocumentType.SelectedValue = batch.DocumentName;
                    restoring = false;
                    SetImport(parsed);
                    BatchInfo.Text = String.Format(polish, "Plik: {0}  |  Utworzono: {1:yyyy-MM-dd HH:mm}  |  Poczekalnia: {2}-{3}/{4}  |  Dokument: {5}",
                        batch.SourceFileName, batch.CreatedAt, batch.SymbolName, batch.FirstNumber, batch.Year, batch.DocumentName);
                    StateInfo.Text = GetStateDescription(batch);
                    SetEditingState();
                    SelectHistoryBatch(batch.Id);
                }
                catch (Exception ex)
                {
                    restoring = false;
                    RestoreCurrentBatchSelection();
                    ErrorWindow.CreateNew(ex, "Nie można odczytać zawartości paczki.");
                }
            }, null);
        }

        private void ReloadSymbols(int branchId, int year)
        {
            var dictionary = LexEnaKonfiguracja.SlownikSymboliWiena;
            if (dictionary == null) return;
            Symbol.ItemsSource = dictionary.Where(s => s.Filter2 == branchId && (s.Filter3 == year || s.Filter3 == 9999))
                .OrderBy(s => s.Numer).ThenBy(s => s.Filter3).GroupBy(s => s.Numer).Select(g => g.First()).ToList();
        }

        private void Branch_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (restoring || BatchYear == null || Symbol == null) return;
            var branch = Branch.SelectedItem as typSlownikFilter;
            if (branch != null) ReloadSymbols(branch.Numer, (int)(BatchYear.Value ?? DateTime.Today.Year));
        }

        private void Symbol_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (restoring || FirstNumber == null) return;
            var symbol = Symbol.SelectedItem as typSlownikFilter;
            FirstNumber.Text = symbol == null ? "" : symbol.Filter1.ToString(polish);
        }

        private void BatchYear_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (restoring || Branch == null || Symbol == null || e.NewValue == null) return;
            var branch = Branch.SelectedItem as typSlownikFilter;
            if (branch == null) return;
            object previous = Symbol.SelectedValue;
            ReloadSymbols(branch.Numer, (int)e.NewValue.Value);
            Symbol.SelectedValue = previous;
        }

        private void ReadXml_Click(object sender, RoutedEventArgs e)
        {
            var branch = Branch.SelectedItem as typSlownikFilter;
            var symbol = Symbol.SelectedItem as typSlownikFilter;
            var documentType = DocumentType.SelectedItem as typSlownikFilter;
            int first;
            if (String.IsNullOrWhiteSpace(BatchName.Text)) { AlertMsg.Show("Podaj oznaczenie paczki."); return; }
            if (branch == null || symbol == null || !Int32.TryParse(FirstNumber.Text, out first) || first <= 0 || BatchYear.Value == null)
            { AlertMsg.Show("Wybierz oddział, symbol poczekalni, dodatni numer pierwszej sprawy i rok."); return; }
            if (documentType == null || BookingDate.SelectedValue == null)
            { AlertMsg.Show("Wybierz dokument księgowy i datę księgowania."); return; }
            var dialog = new OpenFileDialog { Multiselect = false, Filter = "Zbiory XML (*.xml)|*.xml" };
            if (dialog.ShowDialog() != true) return;
            try
            {
                byte[] bytes;
                using (var stream = dialog.File.OpenRead())
                using (var buffer = new MemoryStream())
                {
                    stream.CopyTo(buffer);
                    bytes = buffer.ToArray();
                }
                // The same legacy interest-name conversion as in ImportBilling.
                string xml = Encoding.UTF8.GetString(bytes, 0, bytes.Length)
                    .Replace("HandlowePodmiotyLecznicze", "UstawoweDlaPodmiotowLeczniczych");
                BusyIndicator.IsBusy = true;
                var context = new LexEnaMeritumDomainContext();
                context.CreateBillingImportBatch(newBatchId, BatchName.Text.Trim(), dialog.File.Name,
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)), UserProfile.DbId, LexEnaKonfiguracja.Firma,
                    branch.Numer, symbol.Numer, symbol.Nazwa, first, (int)BatchYear.Value.Value,
                    documentType.Nazwa, BookingDate.SelectedValue.Value, result =>
                    {
                        string value;
                        if (!TryReadResult(result, out value)) return;
                        // A stable ID lets a repeated request reopen the same batch after a lost response.
                        BatchSettings.IsEnabled = false;
                        RefreshHistory(newBatchId);
                    }, null);
            }
            catch (Exception ex)
            {
                BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(ex, "Błąd odczytu XML.");
            }
        }

        private static ImportSprawBilling Deserialize(string xml)
        {
            var value = (ImportSprawBilling)ToXMLSerializers.XmlDeserializeFromString(xml, typeof(ImportSprawBilling));
            if (value == null || value.SprawaDescriptor == null) throw new InvalidOperationException("Brak danych spraw w paczce.");
            return value;
        }

        private void DetachRows()
        {
            if (rows != null) foreach (var row in rows) row.PropertyChanged -= Row_PropertyChanged;
        }

        private void SetImport(ImportSprawBilling value)
        {
            DetachRows();
            rows = new ObservableCollection<SprImportDescriptor>(value.SprawaDescriptor);
            foreach (var row in rows) row.PropertyChanged += Row_PropertyChanged;
            CasesGrid.ItemsSource = rows;
            dirty = false;
            UpdateSummary();
        }

        private void Row_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (restoring || currentBatch == null || currentBatch.Status != "Draft") return;
            var row = (SprImportDescriptor)sender;
            if (!row.CanImport && row.czyrej)
            {
                row.czyrej = false;
                AlertMsg.Show("Ta pozycja została wyłączona przez kontrolę istniejących należności w Wienie.");
                return;
            }
            dirty = true;
            UpdateSummary();
            SetEditingState();
        }

        private void CasesGrid_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            if (currentBatch != null && currentBatch.Status == "Draft")
            {
                dirty = true;
                UpdateSummary();
            }
        }

        private void UpdateSummary()
        {
            var selected = rows.Where(r => r.czyrej && r.CanImport).ToList();
            int cases = rows.Select(r => new { r.id_symbol, r.rok, r.nr }).Distinct().Count();
            int selectedCases = selected.Select(r => new { r.id_symbol, r.rok, r.nr }).Distinct().Count();
            int saved = rows.Where(r => r.czy_ok).Select(r => r.WienaCaseId).Distinct().Count();
            Summary.Text = String.Format(polish,
                "Pozycje XML: {0}  |  Sprawy: {1}  |  Wybrano spraw: {2}  |  Kwota wybranych należności: {3:N2} zł  |  W Wienie: {4}{5}",
                rows.Count, cases, selectedCases, selected.Sum(r => r.BatchAmount), saved,
                dirty ? "  |  Niezapisane zmiany" : "");
        }

        private bool TrySerializeSelection(out string document)
        {
            document = null;
            try
            {
                CasesGrid.CommitEdit();
                document = BillingImportSelectionSerializer.ToBase64(rows);
                return true;
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, "Nie można przygotować wyboru spraw do zapisu. Zmiany pozostały na ekranie; niczego nie wysłano do Wieny.");
                return false;
            }
        }

        private void SaveDraft_Click(object sender, RoutedEventArgs e)
        {
            if (currentBatch == null || currentBatch.Status != "Draft" || registerAttempted) return;
            string document;
            if (!TrySerializeSelection(out document)) return;
            BusyIndicator.IsBusy = true;
            var context = new LexEnaMeritumDomainContext();
            context.SaveBillingImportBatchDraft(currentBatch.Id, LexEnaKonfiguracja.Firma, document, result =>
            {
                string xml;
                if (!TryReadResult(result, out xml)) return;
                try { SetImport(Deserialize(xml)); SetEditingState(); StateInfo.Text = "Zapisano wybór spraw i odsetek w LexEna. Sprawy nie zostały jeszcze utworzone w Wienie. Możesz wrócić do paczki później lub wybrać „Utwórz sprawy w Wienie”."; }
                catch (Exception ex) { ErrorWindow.CreateNew(ex); }
            }, null);
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            CasesGrid.CommitEdit();
            if (currentBatch == null || currentBatch.Status != "Draft" || registerAttempted || !rows.Any(r => r.czyrej && r.CanImport)) return;
            RadWindow.Confirm("Zapisać wybrane sprawy z paczki „" + currentBatch.Name + "” w poczekalni Wieny? Po rozpoczęciu zapisu paczka będzie dostępna tylko do odczytu.",
                (sender2, args) => { if (args.DialogResult == true) RegisterBatch(); });
        }

        private void RegisterBatch()
        {
            string document;
            if (!TrySerializeSelection(out document)) return;
            Guid id = currentBatch.Id;
            registerAttempted = true;
            SetEditingState();
            BusyIndicator.IsBusy = true;
            StateInfo.Text = "Trwa zapis w Wienie. Wynik można później sprawdzić, otwierając paczkę z historii.";
            var context = new LexEnaMeritumDomainContext();
            context.RegisterBillingImportBatch(id, LexEnaKonfiguracja.Firma, document, result =>
            {
                string xml;
                if (TryReadResult(result, out xml))
                {
                    try { SetImport(Deserialize(xml)); }
                    catch (Exception ex) { ErrorWindow.CreateNew(ex); }
                }
                else StateInfo.Text = "Nie potwierdzono wyniku zapisu. Odśwież historię i sprawdź stan paczki przed dalszą pracą.";
                RefreshHistory(id);
            }, null);
        }

        private bool TryReadResult(InvokeOperation<string> result, out string value)
        {
            BusyIndicator.IsBusy = false;
            value = null;
            if (result.HasError)
            {
                result.MarkErrorAsHandled();
                ErrorWindow.CreateNew(result.Error, "Operacja paczki nie została potwierdzona. Sprawdź jej stan w historii.");
                return false;
            }
            value = result.Value;
            if (String.IsNullOrWhiteSpace(value) || value.StartsWith("Błąd", StringComparison.OrdinalIgnoreCase) || value.StartsWith("error:", StringComparison.OrdinalIgnoreCase))
            {
                ErrorWindow.CreateNew(String.IsNullOrWhiteSpace(value) ? "Serwer nie zwrócił zawartości paczki." : value);
                return false;
            }
            return true;
        }

        private static string GetStateDescription(BillingImportBatch batch)
        {
            if (batch.Status == "Completed" || batch.Status == "Partial")
            {
                string importWarning = batch.Status == "Partial" ? "Import zakończył się z błędami — sprawdź informacje przy pozycjach. " : String.Empty;
                if (batch.DistributionState == "Approved")
                    return importWarning + "Podział zatwierdzono i zadekretowano sprawy w Wienie. Poniższa lista zachowuje dane importu i sygnatury poczekalni. Nowe sygnatury oraz przypisane kancelarie są dostępne w oknie „Podział na kancelarie”.";
                if (batch.DistributionState == "Processing")
                    return importWarning + "Dekretacja została rozpoczęta. Otwórz „Podział na kancelarie”, aby sprawdzić lub dokończyć jej zapis. Poniższa lista przedstawia dane importu i sygnatury poczekalni.";
            }
            switch (batch.Status)
            {
                case "Draft": return "Paczka przygotowana. Sprawdź wybór spraw i odsetki. Pozycje o wspólnej sygnaturze zostaną połączone w jedną sprawę w Wienie. Zapisz wybór przed opuszczeniem ekranu.";
                case "Completed": return "Wybrane sprawy zapisano w poczekalni Wieny. Otwórz „Podział na kancelarie”, aby rozdzielić sprawy z aktualnym statusem 85. Pozostałe sprawy będą widoczne jako pominięte.";
                case "Partial": return "Zapis zakończył się z błędami. Sprawdź informacje przy sprawach oraz wynik w Wienie. Ponowny zapis tej paczki jest zablokowany.";
                case "Processing": return "Zapis jest w toku albo został przerwany. Odśwież historię; jeśli stan się utrzymuje, wynik wymaga weryfikacji w Wienie. Ponowny zapis jest zablokowany.";
                default: return "Wynik importu wymaga weryfikacji w Wienie. Ponowny zapis tej paczki jest zablokowany.";
            }
        }

        public sealed class BatchListRow
        {
            public BatchListRow(BillingImportBatch batch) { Batch = batch; }
            public BillingImportBatch Batch { get; private set; }
            public string Name { get { return Batch.Name; } }
            public DateTime CreatedAt { get { return Batch.CreatedAt; } }
            public int CaseCount { get { return Batch.CaseCount; } }
            public int ImportedCount { get { return Batch.ImportedCount; } }
            public string DistributionStateText
            {
                get
                {
                    if (Batch.Status != "Completed" && Batch.Status != "Partial") return "—";
                    if (Batch.DistributionState == "Approved") return "Zatwierdzona";
                    if (Batch.DistributionState == "Processing") return "Dekretacja w toku";
                    return Batch.DistributionUpdatedAt.HasValue ? "Propozycja" : "Do podziału";
                }
            }
            public string StatusText
            {
                get
                {
                    switch (Batch.Status)
                    {
                        case "Draft": return "Przygotowana";
                        case "Processing": return "W trakcie zapisu";
                        case "Completed": return "W poczekalni";
                        case "Partial": return "Zakończona z błędami";
                        default: return "Do weryfikacji";
                    }
                }
            }
        }
    }
}
