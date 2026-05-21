using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LexEnaTrs.Web;
using Telerik.Windows.Data;
using Telerik.Windows.Controls.DomainServices;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Browser;
using Telerik.Windows.Controls;
using System.IO;
using Telerik.Windows.Controls.GridView;
using LexEnaTrs;
using System.Text;
using System.Collections.ObjectModel;

namespace LexEnaTrs.Views
{
    public partial class ImportBilling : UserControl
    {


        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int TypDok = -1;

        private Guid guid = Guid.Empty;
        private string oddzialStr;
        private string systemStr;
        private ObservableCollection<LexEnaTrs.Web.Models.SprImportDescriptor> sprawyList;

        public ImportBilling()
        {

            InitializeComponent();
            this.numRok.Value = DateTime.Today.Year;
            this.numRok.NumberFormatInfo = new System.Globalization.NumberFormatInfo();
            this.numRok.NumberFormatInfo.NumberGroupSeparator = "";
            this.rdpData.SelectedValue = DateTime.Today;
        }




        private void ImportFileCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;

            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }


        }


        private void ImportFileXLSXCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            LexEnaTrs.Web.Models.ImportSprawBilling spr;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0 && message.StartsWith("Błąd"))
            {
                ErrorWindow.CreateNew(message);
            }
            else
            {

                try {
                    // deserializacja

                    spr = (LexEnaTrs.Web.Models.ImportSprawBilling)ToXMLSerializers.XmlDeserializeFromString(message, typeof(LexEnaTrs.Web.Models.ImportSprawBilling));
                    this.sprawyList = new ObservableCollection<LexEnaTrs.Web.Models.SprImportDescriptor>(spr.SprawaDescriptor.ToList());
                    this.oddzialStr = spr.Oddzial;
                    this.systemStr = spr.System;
                    this.SprawyGridView.ItemsSource = this.sprawyList;


                }
                catch (Exception e)
                {
                    ErrorWindow.CreateNew(e, "Procedura importu zwróciła niewłaściwą wartość ");
                    return;
                }

            }



        }



        private List<string> getFilter()
        {
            List<string> lstr = new List<string>();

            Telerik.Windows.Controls.GridViewColumn docColumn = this.SprawyGridView.Columns["nazwa"];
            Telerik.Windows.Controls.GridView.IColumnFilterDescriptor docFilter = docColumn.ColumnFilterDescriptor;
            docFilter.SuspendNotifications();
            if (docFilter.DistinctFilter.DistinctValues != null)
            {
                foreach (string docname in docFilter.DistinctFilter.DistinctValues)
                {
                    lstr.Add(docname);
                }

            }

            docFilter.SuspendNotifications();
            return lstr;

        }

        private void SprawyGridView_Filtered(object sender, GridViewFilteredEventArgs e)
        {
            //this.Rcount.Content = "Wierszy: " + this.SprawyGridView.Items.Count.ToString();
        }



        private void btImport_Click_1(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            PozewDomainContext _pozewcontext;
            byte[] xml;
            // Set Filter to browser text files
            dlg.Filter = "Zbiory XML (*.xml)|*.xml";
            int id_symbol = 0;
            int numer = 0;
            int rok = 0;
            typSlownikFilter symbol;
            typSlownikFilter dok;

            symbol = (typSlownikFilter)this.cbRepertorium.SelectedItem;
            dok = (typSlownikFilter)this.cbTypDok.SelectedItem;
            id_symbol = (int)(this.cbRepertorium.SelectedValue ?? 0);

            rok = (int)(this.numRok.Value ?? 0);
            Int32.TryParse(this.tbNumer.Text, out numer);
            if (dok == null)
            {
                AlertMsg.Show("Wybierz oznaczenie dokumentu");
                return;

            }

            if (!(id_symbol > 0 && numer > 0 && rok > 0))
            {
                AlertMsg.Show("Uzupełnij oznaczenie pierwszej sprawy ");
                return;
            }


            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                try
                {
                    using (FileStream stream = dlg.File.OpenRead())
                    {

                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            xml = ms.ToArray();


                            // konwersja do stringa
                            try
                            {
                                string xmlString = System.Text.Encoding.UTF8.GetString(xml, 0, xml.Length);

                                // modyfikacja XML
                                xmlString = xmlString.Replace("HandlowePodmiotyLecznicze", "UstawoweDlaPodmiotowLeczniczych");

                                // powrót do byte[]
                                xml = System.Text.Encoding.UTF8.GetBytes(xmlString);
                            }
                            catch { }


                            this.BusyIndicator.IsBusy = true;
                            //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                            _pozewcontext = new PozewDomainContext();
                            _pozewcontext.ImportBillingDocument(Convert.ToBase64String(xml), dlg.File.Name, UserProfile.DbId, LexEnaKonfiguracja.Firma, id_symbol, symbol.Nazwa, numer, rok, dok.Nazwa, ImportFileXLSXCompleted, null);
                        }

                    }
                }
                catch (Exception ex)
                {
                    this.BusyIndicator.IsBusy = false;

                    ErrorWindow.CreateNew(ex, "Błąd odczytu zbioru");
                }
            }

            else
                AlertMsg.Show("Nie wybrano żadnego zbioru");


        }
        private void reloadSymbol(int oddzial, int rok)
        {
            int prev_symbol = 0;

            ObservableCollection<typSlownikFilter> lst = new ObservableCollection<typSlownikFilter>();
            List<typSlownikFilter> l = LexEnaKonfiguracja.SlownikSymboliWiena.Where(a => a.Filter2 == oddzial && (a.Filter3 == rok || a.Filter3 == 9999)).OrderBy(a => a.Numer).ThenBy(a => a.Filter3).ToList();

            if (l != null)
            {
                foreach (typSlownikFilter t in l)
                {
                    if (t.Numer == prev_symbol) continue;
                    lst.Add(t);
                    prev_symbol = t.Numer;

                }

            }

            this.cbRepertorium.ItemsSource = lst;


        }


        private void cbOddzial_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            // odfiltrowanie repertoriów
            int oddzial = 0;
            int rok = 9999;

            rok = (int)this.numRok.Value;
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                reloadSymbol(((typSlownikFilter)e.AddedItems[0]).Numer, rok);

            }
        }

        private void cbRepertorium_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            int poz = 0;
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {

                poz = ((typSlownikFilter)e.AddedItems[0]).Filter1;
                this.tbNumer.Text = poz.ToString();
            }

        }

        private void numRok_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        { int? repo;

            if (e.NewValue != null && this.cbOddzial.SelectedValue != null)
            {
                repo = (int?)cbRepertorium.SelectedValue;
                reloadSymbol((int)this.cbOddzial.SelectedValue, (int)e.NewValue);
                cbRepertorium.SelectedValue = repo;
                //this.tbNumer.Text = poz.ToString();
            }
        }

        private void btNumeruj_Click(object sender, RoutedEventArgs e)
        {
            ToXMLSerializers.XmlDeserializeFromString("XXXXX", typeof(LexEnaTrs.Web.Models.ImportSprawBilling));
        }


        private void InsertSprimportCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            LexEnaTrs.Web.Models.ImportSprawBilling spr;
            this.BusyIndicator.IsBusy = false;

            if (message.Length > 0 && message.StartsWith("Błąd"))
            {
                ErrorWindow.CreateNew(message);
            }
            else
            {

                try
                {
                    // deserializacja

                    spr = (LexEnaTrs.Web.Models.ImportSprawBilling)ToXMLSerializers.XmlDeserializeFromString(message, typeof(LexEnaTrs.Web.Models.ImportSprawBilling));
                    this.sprawyList = new ObservableCollection<LexEnaTrs.Web.Models.SprImportDescriptor>(spr.SprawaDescriptor.ToList());
                    this.SprawyGridView.ItemsSource = this.sprawyList;

                }

                catch (Exception ex)
                {
                    ErrorWindow.CreateNew(ex);

                }


            }
        }

        private ObservableCollection<Web.Models.SprImportDescriptor> cleanSpr(ObservableCollection<Web.Models.SprImportDescriptor> sprList)
        {
            foreach (Web.Models.SprImportDescriptor spr in sprList)
            {
                foreach (Web.Models.Obciazenie obc in spr.Odbiorca.Obciazenia)
                {

                    if (obc.Wplaty != null && obc.Wplaty.GetUpperBound(0) >= 0)
                    {
                        bool czywpl0 = false;
                        for (int i = 0; i <= obc.Wplaty.GetUpperBound(0); i++)
                        {
                            {
                                if (obc.Wplaty[i].Kwota_wplaty == 0)
                                {
                                    czywpl0 = true;
                                    break;
                                }
                            }

                        }
                        if (czywpl0 == true)
                        {
                            obc.Wplaty = null;


                        }

                    }
                }
            }
            return sprList;
        }
    



             
 
        private void importSpr()
        {
            // 
            typSlownikFilter  oddzial = (typSlownikFilter)this.cbOddzial.SelectedItem;
            typSlownikFilter symbol = (typSlownikFilter)this.cbRepertorium.SelectedItem;
            typSlownikFilter dok = (typSlownikFilter)this.cbTypDok.SelectedItem;
            typSlownikFilter kancelaria = (typSlownikFilter)this.cbRadca.SelectedItem;
            int id_symbol = (int)(this.cbRepertorium.SelectedValue ?? 0);
            LexEnaTrs.Web.Models.ImportSprawBilling importB = new Web.Models.ImportSprawBilling();
            importB.SprawaDescriptor = cleanSpr(sprawyList).ToArray();
            importB.Oddzial = this.oddzialStr;
            importB.NrDodowu = dok.Nazwa;
            importB.System = this.systemStr;
            importB.IdOddzial = oddzial.Numer;
            if (kancelaria != null)
                importB.IdKancelaria = kancelaria.Numer;

            importB.DataKsiegowania = this.rdpData.SelectedValue.Value;
            try
            {
                string xml = ToXMLSerializers.SerializeToString(importB, typeof(Web.Models.ImportSprawBilling));
                this.BusyIndicator.IsBusy = true;
                PozewDomainContext _pozewcontext = new PozewDomainContext();
                _pozewcontext.InsertBillingImport(Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)), LexEnaKonfiguracja.Firma, InsertSprimportCompleted, null);
            }
            catch (Exception ex)
            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(ex);

            }


        }

        private void clrView()
        {
        
            if (LexEnaKonfiguracja.SlownikSymboliWiena != null)
            {
                symboleSprawWiena ss = new symboleSprawWiena();
                LexEnaKonfiguracja.SlownikSymboliWiena = ss.typSymboleSprawWiena;
                reloadSymbol((int)this.cbOddzial.SelectedValue, (int)(this.numRok.Value??0) );
                cbRepertorium.SelectedIndex = -1;
                cbOddzial.SelectedIndex = -1;
                tbNumer.Text = "";

            } 
            this.sprawyList = new ObservableCollection<Web.Models.SprImportDescriptor>();
            this.SprawyGridView.ItemsSource = this.sprawyList;

        } 

        private void OnClosed(object sender, WindowClosedEventArgs e)
        {
            var result = e.DialogResult;
            if (result == true)
            {
                this.importSpr();
                
            }
        }

        private void btZapisz_Click(object sender, RoutedEventArgs e)
        {

            RadWindow.Confirm("Czy na pewno chcesz zaimportować wybrane sprawy ?", this.OnClosed);

        }

        private void OnClearConfirmed(object sender, WindowClosedEventArgs e)
        {
            var result = e.DialogResult;
            if (result == true)
            {
                clrView();

            }
        }

        private void btNew_Click(object sender, RoutedEventArgs e)
        {
            RadWindow.Confirm("Czy na pewno chcesz  zaimportować nowy zbiór i wyczyścić zawartość ekranu ?", this.OnClearConfirmed);
        }

        private void SprawyGridView_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
           
            if (e.Cell.Column.UniqueName == "NumerSpr")
            {
                ErrorWindow.CreateNew("!!!");
                LexEnaTrs.Web.Models.SprImportDescriptor theRow = (LexEnaTrs.Web.Models.SprImportDescriptor)SprawyGridView.SelectedItem;
                int charLocation = theRow.sygn_obciaz.IndexOf('-');
                if (charLocation >= 0)
                {
                    theRow.sygn_obciaz = theRow.sygn_obciaz.Substring(0, charLocation) + "-" + theRow.nr.ToString() + "/" + theRow.rok.ToString();
                  
                // przeliczenie sygnatury
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((GridViewComboBoxColumn)this.SprawyGridView.Columns["CzyUstawowe"]).ItemsSource = rodzajeOdsetek.GetRodzajOdsetek();
        }
    }
    }
    

   
