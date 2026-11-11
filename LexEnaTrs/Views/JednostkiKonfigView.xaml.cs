using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using LexEnaTrs;
using LexEnaTrs.Web;
using LexEnaTrs.Web.Models;

namespace LexEnaTrs.Views
{
    public partial class JednostkiKonfigView : UserControl
    {
        private bool symbolsLoading;
        private bool symbolsLoaded;
        private DataFormComboBoxField symbolField;

        public JednostkiKonfigView()
        {
            InitializeComponent();
            JednostkiWindykacjidds.SubmittedChanges += JednostkiWindykacjidds_SubmittedChanges;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!symbolsLoaded && !symbolsLoading) LoadSymbols();
        }

        private void RetrySymbols_Click(object sender, RoutedEventArgs e)
        {
            if (!symbolsLoaded && !symbolsLoading) LoadSymbols();
        }

        private void LoadSymbols()
        {
            symbolsLoading = true;
            JednostkiDataForm.IsEnabled = false;
            RetrySymbolsButton.Visibility = Visibility.Collapsed;
            SymbolLoadStatus.Text = "Odczyt symboli spraw z Wieny...";
            try
            {
                new LexEnaMeritumDomainContext().GetBillingDistributionSymbols(LexEnaKonfiguracja.Firma, result =>
                {
                    if (result.HasError)
                    {
                        result.MarkErrorAsHandled();
                        SymbolLoadFailed(result.Error.Message);
                        return;
                    }
                    try
                    {
                        if (String.IsNullOrWhiteSpace(result.Value) || result.Value.StartsWith("Błąd", StringComparison.OrdinalIgnoreCase))
                            throw new InvalidOperationException(String.IsNullOrWhiteSpace(result.Value) ? "Brak odpowiedzi z serwera." : result.Value);
                        var values = (BillingDistributionSymbol[])ToXMLSerializers.XmlDeserializeFromString(result.Value, typeof(BillingDistributionSymbol[]));
                        if (values == null) throw new InvalidOperationException("Nieprawidłowy słownik symboli.");
                        var source = (WienaDistributionSymbolList)Resources["WienaDistributionSymbols"];
                        source.Clear();
                        source.Add(new BillingDistributionSymbol { Symbol = String.Empty, DisplayName = "Brak symbolu" });
                        foreach (var value in values) source.Add(value);
                        symbolsLoading = false;
                        symbolsLoaded = true;
                        SymbolLoadStatus.Text = values.Length == 0
                            ? "W Wienie nie znaleziono symboli dla tej firmy. Przed dekretacją uzupełnij tabelę symbol w Wienie i ponownie otwórz konfigurację."
                            : "Symbole pobrano z Wieny. Wybierz kancelarię i kliknij Edytuj, aby zmienić symbol.";
                        // Create the unit editor only after its lookup is complete.
                        // Binding an empty lookup first could clear an existing value.
                        JednostkiDataForm.IsEnabled = true;
                        JednostkiWindykacjidds.Load();
                    }
                    catch (Exception ex) { SymbolLoadFailed(ex.Message); }
                }, null);
            }
            catch (Exception ex) { SymbolLoadFailed(ex.Message); }
        }

        private void SymbolLoadFailed(string message)
        {
            symbolsLoading = false;
            symbolsLoaded = false;
            JednostkiDataForm.IsEnabled = false;
            SymbolLoadStatus.Text = "Nie można pobrać symboli spraw z Wieny. " + message;
            RetrySymbolsButton.Visibility = Visibility.Visible;
        }

        private void WienaSymbolField_Loaded(object sender, RoutedEventArgs e)
        {
            symbolField = (DataFormComboBoxField)sender;
            RadComboBox editor = GetSymbolEditor();
            if (editor == null) return;
            editor.IsEditable = false;
            // Use an explicit empty option; no selection may mean an obsolete stored
            // symbol and must not silently clear that value when opening the form.
            editor.ClearSelectionButtonVisibility = Visibility.Collapsed;
        }

        private RadComboBox GetSymbolEditor()
        {
            if (symbolField == null) return null;
            return symbolField.Content as RadComboBox ?? symbolField.ChildrenOfType<RadComboBox>().FirstOrDefault();
        }

        private void WienaKancelariaField_Loaded(object sender, RoutedEventArgs e)
        {
            DataFormComboBoxField field = (DataFormComboBoxField)sender;
            RadComboBox editor = field.Content as RadComboBox;
            if (editor == null) editor = field.ChildrenOfType<RadComboBox>().FirstOrDefault();
            if (editor != null)
            {
                editor.ClearSelectionButtonVisibility = Visibility.Visible;
                editor.ClearSelectionButtonContent = "Brak powiązania";
            }
        }

        private void JednostkiDataForm_EditEnding(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndingEventArgs e)
        {
            if (e.EditAction != Telerik.Windows.Controls.Data.DataForm.EditAction.Commit) return;
            JednostkaWindykacji item = JednostkiDataForm.CurrentItem as JednostkaWindykacji;
            if (item != null)
            {
                if (!symbolsLoaded)
                {
                    e.Cancel = true;
                    AlertMsg.Show("Najpierw pobierz listę symboli spraw z Wieny.");
                    return;
                }
                RadComboBox editor = GetSymbolEditor();
                var selected = editor == null || symbolField.DataContext != item ? null : editor.SelectedItem as BillingDistributionSymbol;
                // OneWay protects unknown stored symbols during template creation.
                // Copy only an explicit selection on Commit; Cancel leaves the entity alone.
                string designation = selected == null ? item.SymbolSprawWiena : selected.Symbol;
                designation = String.IsNullOrWhiteSpace(designation) ? null : designation.Trim();
                var choices = (WienaDistributionSymbolList)Resources["WienaDistributionSymbols"];
                var matching = designation == null ? null : choices.FirstOrDefault(s => String.Equals(s.Symbol, designation, StringComparison.OrdinalIgnoreCase));
                if (designation != null && matching == null)
                {
                    e.Cancel = true;
                    AlertMsg.Show("Zapisany symbol nie występuje na aktualnej liście Wieny. Wybierz symbol z listy albo „Brak symbolu”.");
                    return;
                }
                item.SymbolSprawWiena = matching == null ? null : matching.Symbol;
            }
            if (item != null && item.UczestniczyWPrzydziale &&
                (item.TypJednostki != 1 || !item.WienaFirmaId.HasValue || item.WienaFirmaId.Value <= 0))
            {
                e.Cancel = true;
                AlertMsg.Show("Udział w przydziale wymaga zewnętrznej kancelarii prawnej i powiązania z kancelarią w Wienie.");
            }
        }

        private void JednostkiWindykacjidds_SubmittedChanges(object sender,
            Telerik.Windows.Controls.DomainServices.DomainServiceSubmittedChangesEventArgs e)
        {
            if (e.HasError)
            {
                e.MarkErrorAsHandled();
                ErrorWindow.CreateNew(e.Error, "Nie zapisano konfiguracji jednostki.");
            }
        }

        private void JednostkiDataForm_DeletedItem(object sender, Telerik.Windows.Controls.Data.DataForm.ItemDeletedEventArgs e)
        {

        }

        private void JednostkiDataForm_DeletingItem(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogParameters dlgparm = new DialogParameters();
            dlgparm.CancelButtonContent = "Nie";
            dlgparm.Content = "Czy na pewno chcesz usunąć tę jednostkę ?";
            dlgparm.Header = "Potwierdź";
            dlgparm.Closed = confdluwndClose;
            RadWindow.Confirm(dlgparm);
            e.Cancel = true;
        }

        private void confdluwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {
                DataItemCollection itemsToDel = (DataItemCollection)this.JednostkiDataForm.ItemsSource;//  as IList<DaneDluznika>;
                itemsToDel.Remove(this.JednostkiDataForm.CurrentItem as KontoEPU);

                this.JednostkiWindykacjidds.SubmitChanges();
            }
        }

        private void JednostkiDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Commit)
                this.JednostkiWindykacjidds.SubmitChanges();
        }

       

        private void KontaEPUDataForm_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            this.JednostkiWindykacjidds.SubmitChanges(); 
        }

        private void JednostkiDataForm_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            JednostkaWindykacji newitem;
            newitem = (JednostkaWindykacji)e.NewItem;
            newitem.TypJednostki = 1;
            newitem.UczestniczyWPrzydziale = false;
            newitem.WienaFirmaId = null;
            newitem.SymbolSprawWiena = null;
            
        }

     
      



    }

    public sealed class WienaDistributionSymbolList : ObservableCollection<BillingDistributionSymbol>
    {
    }
}
