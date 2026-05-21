using System;
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
using System.Collections.ObjectModel;
using Telerik.Windows.Controls.GridView;
using System.Windows.Markup;
using System.Threading;
using System.Windows.Browser;
using LexEnaTrs;
using LexEnaTrs.Web;
using System.ServiceModel.DomainServices.Client;
using System.IO;
using System.ComponentModel;


namespace LexEnaTrs.Views 
{
    public partial class PozewView : UserControl
    {
        private QueryableCollectionView qcvListaPozwanych;
        private PozewEPU pozew;
        private PozewDomainContext _pozewcontext;
        private RadDropDownButton _faktbutton;
        private RadDropDownButton _uzasadnieniebutton;
        public event EventHandler pozewEPUvalidated;


        private readonly string[] SpecialTags = new string[] { "<$DataUmowy$>", 
                                                                 "<$NumerUmowy$>",
                                                                   "<$ListaFaktur$>",
                                                                     "<$ListaDok$>",
                                                                      "<$ListaNot$>",
                                                                        "<$DataWezwania$>",
                                                                            "<$WPS$>",
                                                                              "<$DataDowodu$>",
                                                                               "<$NumerKontrahenta$>"};


        private int saveActionType  = 0;

        protected virtual void OnpozewEPUValidated(pozewEventArgs e)
        {
            if (pozewEPUvalidated != null)
                pozewEPUvalidated(this, e);
        }

        public decimal KosztySadowe
        {
            get;
            set;

        }

        public decimal Kzp
        {
            get;
            set;

        }

        public decimal OdsNalicz
        {
            get;
            set;
        }

        public decimal KosztyInne
        {
            get;
            set;

        }

        public decimal WartPrzedmSporu
        {
            get;
            set;

        }

        public decimal NotyOdsetkowe
        {
            get;
            set;

        }
        public DateTime dWniesienia
        {
            get;
            set;
        }
        public string pozewSerialized
        {
            get;
            set;

        }

        public string odsNaliczSerialized
        {
            get;
            set;

        }

        public int docStatus
        {
            get;
            set;

        }

        public bool statusChanged
        {
            get;
            set;

        }

        public int IdSprawy
        {
            get;
            set;
        }

        public int IdDoc
        {
            get;
            set;
        }

        public string NumerKontrahenta
        {
            get;
            set;

        }
        

        public string serializePozew()
        {
            updateSelectedDowod();
            updateOdsetki(ref pozew);
            //pozew.ListaPozwanych = (qcvListaPozwanych.QueryableSourceCollection as ObservableCollection<typStrona>);
            if (pozew.ListaPozwanych != null)
            {
                foreach (typStrona ts in pozew.ListaPozwanych)
                {
                    if (ts.RodzajStrony == 0 && String.IsNullOrWhiteSpace(ts.ObcokrajowiecString))
                    { // osoba fizyczna
                        ts.ObcokrajowiecString = "obywatelstwo polskie";

                    }
                    if (ts.RodzajStrony == 2)
                    {
                        ts.ObcokrajowiecString = "";
                        if ((ts.Item as typInstytucja).CzyRejestr == 0)
                        {
                            (ts.Item as typInstytucja).Item = null;
                        }
                        

                    }
                    if (ts.RodzajStrony == 3)
                        ts.ObcokrajowiecString = "";

                }

            }

            if (pozew.ListaDowodow != null)
            {
                foreach (typDowod td in pozew.ListaDowodow)
                {
                    if (String.IsNullOrWhiteSpace(td.DataDowodu))
                    {
                        td.DataDowodu = null;

                    }
                }

            }



                
                this.pozewSerialized = ToXMLSerializers.SerializePozew(pozew,0);
            // oblicznaie sum
            this.pozew.ObliczSumy();
            this.KosztySadowe = pozew.OplataSadowa.WartoscOplaty;
            this.WartPrzedmSporu = pozew.WartoscSporu;
            this.NotyOdsetkowe = pozew.OdsKapital;
            this.OdsNalicz = pozew.OdsNalicz;
            this.odsNaliczSerialized = (pozew.OdsetkiSkapitalizowne != null ? ToXMLSerializers.SerializeToString(pozew.OdsetkiSkapitalizowne, typeof(typSprawaOds)) : null); 
            this.Kzp = this.pozew.ObliczKzp(pozew.WartoscSporu);
            if (this.pozew.InneKoszty != null)
                this.KosztyInne = this.pozew.InneKoszty.Wartosc;
            else
                this.KosztyInne = 0;
            this.dWniesienia = pozew.DataZlozenia;
            return this.pozewSerialized;
        }


         

        public PozewView()
        {



            // pozew = new PozewEPU();

            /*
             ICommand deleteCommand = RadGridViewCommands.Delete;
             ICommand beginInsertCommand = RadGridViewCommands.BeginInsert;
             ICommand cancelRowEditCommand = RadGridViewCommands.CancelRowEdit;
             ICommand commitEditCommand = RadGridViewCommands.CommitEdit;
       */

            InitializeComponent();
            this.statusChanged = false;
        }

        public void ValidatePozewEPU()
        {
            DateTime dit;

            foreach (typDowod tyd in pozew.ListaDowodow)
            {
                if (tyd.DataDowodu != null)
                {
                    if (!string.IsNullOrWhiteSpace(tyd.DataDowodu))
                    {
                        if (DateTime.TryParse(tyd.DataDowodu, out dit) == false)
                        {
                            pozewEventArgs ea = new pozewEventArgs();
                            ea.Status = -1;
                            ea.Message = "Błąd daty dowodu: " + tyd.Opis + tyd.DataDowodu;
                            OnpozewEPUValidated(ea);
                            return;
                        }
                    }
                    else tyd.DataDowodu = null;
                }
            }
            
            if (_pozewcontext == null)
                _pozewcontext = new PozewDomainContext();
            _pozewcontext.ValidateDokEPUXSD(this.pozewSerialized,10, ValidatePozewEPUCompleted, null);



        }


        private void ValidatePozewEPUCompleted(InvokeOperation<string> message)
        {
            string msg;
            pozewEventArgs ea;
            msg = message.Value;
            ea = new pozewEventArgs();
            if (msg.Contains("error") || msg.Contains("Błąd"))
            {
                ea.Status = -1;
                ea.Message = msg;
            }
            else
            {
                ea.Status = 1;
                ea.Message = "OK";

            }
            OnpozewEPUValidated(ea);
        }

        /*   
       private void radGridViewRoszczenia_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
       {

           try
           {
               if (this.radGridViewDowodyZaznacz != null && pozew.CurrentRoszczenie.Dowody != null)
               {
                   this.radGridViewDowodyZaznacz.IsBusy = true;
                   this.radGridViewDowodyZaznacz.SelectedItems.Clear();
                   foreach (typDowod td in pozew.ListaDowodow)
                       if (td.IsSelected)
                           this.radGridViewDowodyZaznacz.SelectedItems.Add(td);
                   this.radGridViewDowodyZaznacz.IsBusy = false;
               }
               this.gridViewOdsetkiNal.ItemsSource = pozew.CurrentRoszczenie.Odsetki; 

           }
           catch (Exception ee)
           {
              ErrorWindow.CreateNew(ee);
           }

            
       } */




        private void dataFormRoszczenia_CurrentItemChanged(object sender, System.EventArgs e)
        {
            typRoszczenie typRoszcz;
            // TODO: Add event handler implementation here.

            typRoszcz = (typRoszczenie)this.dataFormRoszczenia.CurrentItem;
            var dataForm = (QueryableCollectionView)this.dataFormRoszczenia.ItemsSource;
            if (dataForm.IsAddingNew && typRoszcz != null)
            { // Dodawanie nowego wiersza MessageBox.Show("NewItem");


                /*
                Scrollowanie do nowego wiersza
                 this.gridView.ScrollIntoViewAsync(this.gridView.Items[this.gridView.Items.Count - 1], //the row
                                     this.gridView.Columns[this.gridView.Columns.Count - 1], //the column
                                     new Action<FrameworkElement>((f) =>
                                     {
                                        (f as GridViewRow).IsSelected = true; // the callback method
                                     }));
             
                */
            }

        }








        private void radGridViewDowodyZaznacz_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            // TODO: Add event handler implementation here.
           /*
            if (!this.radGridViewDowodyZaznacz.IsBusy)
            {
                foreach (typDowod selectedItem in e.AddedItems)
                {
                    selectedItem.IsSelected = true;
                }

                foreach (typDowod deselectedItem in e.RemovedItems)
                {
                    deselectedItem.IsSelected = false;
                }
            }
            */
        }

        /*
        private bool ValidPaczkaDaty(PozewEPU pozew, ref string strErrText, int krok)
        {

            strErrText = string.Empty;

            string strData = pozew.dataZlozenia.ToString();

            string strRok = pozew.dataZlozenia.Substring(0, 4);
            string strMies = pozew.dataZlozenia.Substring(5, 2);
            string strDzien = pozew.dataZlozenia.Substring(8, 2);
            DateTime dtData1 = new DateTime(1900, 1, 1);
            DateTime dtData = new DateTime(int.Parse(strRok), int.Parse(strMies), int.Parse(strDzien));

            DateTime dtDataMax = new DateTime(2020, 1, 1);
            DateTime dtDataMin = new DateTime(1950, 1, 1);

            if (krok == 1)
            {
                if (dtData.Date < dtData1.Date)
                {
                    strErrText = "BŁĄD: Niepoprawna data złożenia pozwu.";
                    return false;

                }
            }

            int i = 0, j = 0;

            if (krok == 2)
            {
                if (pozew.ListaRoszczen[i].Odsetki != null)
                {
                    for (j = 0; j <= pozew.ListaRoszczen[i].Odsetki.GetLength(0) - 1; j++)
                    {

                        if (pozew.ListaRoszczen[i].Odsetki[j].dataOd != null)
                        {
                            //data od
                            strRok = pozew.ListaRoszczen[i].Odsetki[j].dataOd.Substring(0, 4);
                            strMies = pozew.ListaRoszczen[i].Odsetki[j].dataOd.Substring(5, 2);
                            strDzien = pozew.ListaRoszczen[i].Odsetki[j].dataOd.Substring(8, 2);
                            DateTime dtDataOd = new DateTime(int.Parse(strRok), int.Parse(strMies), int.Parse(strDzien));

                            if (dtDataOd.Date > dtDataMax.Date || dtDataOd.Date < dtDataMin.Date)
                            {
                                strErrText = "BŁĄD: Niepoprawna data dla roszczenia.";
                                return false;
                            }

                        }


                        if (pozew.ListaRoszczen[i].Odsetki[j].dataDo != null)
                        {
                            //data do
                            strRok = pozew.ListaRoszczen[i].Odsetki[j].dataDo.Substring(0, 4);
                            strMies = pozew.ListaRoszczen[i].Odsetki[j].dataDo.Substring(5, 2);
                            strDzien = pozew.ListaRoszczen[i].Odsetki[j].dataDo.Substring(8, 2);
                            DateTime dtDataDo = new DateTime(int.Parse(strRok), int.Parse(strMies), int.Parse(strDzien));

                            if (dtDataDo.Date > dtDataMax.Date || dtDataDo.Date < dtDataMin.Date)
                            {
                                strErrText = "BŁĄD: Niepoprawna data dla roszczenia.";
                                return false;
                            }
                        }

                    }
                }
            }
            i = 0;
            j = 0;
            if (krok == 3)
            {
                if (pozew.ListaDowodow != null)
                {
                    for (j = 0; j <= pozew.ListaDowodow.Count() - 1; j++)
                    {

                        if (pozew.ListaDowodow[j].dataDowodu != null)
                        {
                            //data od
                            strRok = pozew.ListaDowodow[j].dataDowodu.Substring(0, 4);
                            strMies = pozew.ListaDowodow[j].dataDowodu.Substring(5, 2);
                            strDzien = pozew.ListaDowodow[j].dataDowodu.Substring(8, 2);
                            DateTime dtDataDowodu = new DateTime(int.Parse(strRok), int.Parse(strMies), int.Parse(strDzien));

                            if (dtDataDowodu.Date > dtDataMax.Date || dtDataDowodu.Date < dtDataMin.Date)
                            {
                                strErrText = "BŁĄD: Niepoprawna data dla dowodu.";
                                return false;
                            }

                        }
                    }
                }
                else
                {
                    strErrText = "brak dowodów w pozwie.";
                }
            }
            return true;
        }

        */
        private void ToXMLButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            string s;
            try
            {
                updateSelectedDowod();
                s = ToXMLSerializers.SerializePozew(pozew,0);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            ShowXML viewxml = new ShowXML();
            viewxml.tbXML.Text = s;
            viewxml.Show();
            
        }


        private void PokazButtonClicked()
        {
            updateSelectedDowod();
            this.ShowDocs.IsEnabled = false;
            pozewSerialized = ToXMLSerializers.SerializePozew(pozew,0);
            if (_pozewcontext == null)
                _pozewcontext = new PozewDomainContext();
            _pozewcontext.ValidateDokEPUXSD(pozewSerialized, 10,PozewValidated, null);

        }


        private void PozewValidated(InvokeOperation<string> message)
        {
            string msg;
            msg = message.Value;
            if (msg.Contains("error") || msg.Contains("Błąd"))
            {
                ErrorWindow.CreateNew(msg);
                this.ShowDocs.IsEnabled = true;
                return;
            }
            switch (saveActionType)
	        {
                case 0:
                         _pozewcontext.DokumentZEPU2HTML(pozewSerialized, -10 , PozewEPUCompleted, null);  // wyświetlenie zbiou html
                         break;
                case 1:
                         _pozewcontext.DokumentZEPU2HTML(pozewSerialized,0, PozewToSaveCompleted, null);  // wyświetlenie zbiou html
                         break;
		        default:
                         _pozewcontext.DokumentZEPU2HTML(pozewSerialized,-10, PozewEPUCompleted, null);  // wyświetlenie zbiou html
                         break;
	        }

                          
        }

        private void PozewEPUCompleted(InvokeOperation<string> html)
        {
            
            string htmlpath;
            this.ShowDocs.IsEnabled = true;
            try
            {
                htmlpath = html.Value;//e.Result.ToString();

                /*Uri uri = new Uri(Application.Current.Host.Source, htmlpath);

                HtmlWindow window = HtmlPage.Window;
                window.Navigate(uri, "_blank");
                */
                HtmlViewer vw = new HtmlViewer();
                vw.content = htmlpath;
                vw.Show();
                


            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, "Błąd prezentacji dokumentu html");
                ;

            }

        }

        private void PozewToSaveCompleted(InvokeOperation<string> html)
        {

            string htmlpath;
            
            htmlpath = html.Value;//e.Result.ToString();
            DaneDoEksportuReady wnd = new DaneDoEksportuReady();
            wnd.Show();
            wnd.Closed += (obj, e) =>
            {
                SaveFileDialog sfd = new SaveFileDialog();

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Ms Word .doc files (*.doc)|*.doc|HTML files (*.html)|*.html|All Files (*.*)|*.*";

                bool? result = saveDialog.ShowDialog();
                if (result.Value)
                {
                    using (Stream saveStream = saveDialog.OpenFile())
                    using (StreamWriter saveWriter = new StreamWriter(saveStream))
                    {
                        saveWriter.Write(htmlpath);
                    }
                }
            };

            /*
            Uri uri = new Uri(Application.Current.Host.Source, htmlpath);
            DownloadManager dwnMgr  = new DownloadManager();
            dwnMgr.downloadButton = this.ShowDocs;
            dwnMgr.ServerFileUri = uri.AbsoluteUri;
            dwnMgr.DownloadnSave(0);
            */

        }



      





        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // dodać
            // inicjalizaja pozwu
            //client.displayPozewEPUCompleted += new EventHandler<EPUCommonServiceReference.displayPozewEPUCompletedEventArgs>(client_displayPozewEPUCompleted);
            // deserializacja pozwu
         

            if (this.docStatus > 0)  // załadowanie pozwu
            {
                pozew = ToXMLSerializers.XmlDeserializeFromString<PozewEPU>(ToXMLSerializers.ReplaceNamespace( this.pozewSerialized, Constants.currnamespace == Constants.newnamespace));
                if (this.odsNaliczSerialized != null)
                {
                    typSprawaOds typspro = new typSprawaOds();
                    typspro = (typSprawaOds)ToXMLSerializers.XmlDeserializeFromString(this.odsNaliczSerialized, typeof(typSprawaOds));
                    pozew.OdsetkiSkapitalizowne = new typSprawaOds();
                    pozew.OdsetkiSkapitalizowne = typspro;
                
                }
                if (pozew.InneKoszty == null)
                    pozew.InneKoszty = new typKoszty();
                QueryableCollectionView qcvListaDowodow = new QueryableCollectionView(pozew.ListaDowodow); // <typDowod>(typDowod.GetDowody());
                QueryableCollectionView qcvListaRoszczen = new QueryableCollectionView(pozew.ListaRoszczen);
                QueryableCollectionView qcvListaPowodow = new QueryableCollectionView(pozew.ListaPowodow); // <typDowod>(typDowod.GetDowody());
                

                foreach (typStrona pw in pozew.ListaPozwanych)
                {
                    pw.CzyPowod = false;
                    if (pw.RodzajStrony == 2)
                    {
                        if ((pw.Item as typInstytucja).CzyRejestr == 2 && ((pw.Item as typInstytucja).Item == null || (pw.Item as typInstytucja).Item.GetType()==typeof(string))) // inny rejestr
                        {

                            (pw.Item as typInstytucja).Item = new typInnyRejestr();
                        }


                    }
                    
                }
                this.qcvListaPozwanych = new QueryableCollectionView(pozew.ListaPozwanych);


                foreach (var pw in qcvListaPowodow)
                    (pw as typStrona).CzyPowod = true;
    
                 this.LayoutRoot.DataContext = pozew;
                ((EnumDataSource)this.LayoutRoot.Resources["EnumTypDowodSource"]).EnumType = typeof(typRodzajDowodu);
                this.dataFormRoszczenia.CurrentItemChanged += this.dataFormRoszczenia_CurrentItemChanged;
                this.radGridViewDowodyZaznacz.SelectionChanged += this.radGridViewDowodyZaznacz_SelectionChanged;
                this.radGridViewRoszczenia.ItemsSource = qcvListaRoszczen;
                this.dataFormRoszczenia.ItemsSource = qcvListaRoszczen;
                this.radGridViewDowodyZaznacz.ItemsSource = qcvListaDowodow;
                this.radGridViewDow.ItemsSource = qcvListaDowodow;
                this.dataFormDow.ItemsSource = qcvListaDowodow;
                this.DataFormPozwani.ItemsSource = qcvListaPozwanych;
                
                this.RadGridListaPozwanych.ItemsSource = qcvListaPozwanych;
                this.DataFormPowod.ItemsSource = qcvListaPowodow;
                this.DataFormSkladajacy.CurrentItem = pozew.OsobaSkladajaca;
                ((GridViewComboBoxColumn)this.gridViewOdsetkiNal.Columns["CzyUstawowe"]).ItemsSource = rodzajeOdsetek.GetRodzajOdsetek();
                //this.gridViewOdsetkiNal.CellEditEnded += this.gridViewOdsetkiNal_CellEditEnded;
                this.vw_KomunikacjaDocWysdds.QueryParameters[0].Value = IdDoc;
                this.vw_KomunikacjaDocWysdds.Load();
            }


        }



        private void DataFormPozwani_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            typStrona ts;
            ts = e.NewItem as typStrona;
            ts.CzyPowod = false;
            ts.Adres.Add(new typAdres());
        }

        private void DataFormPowod_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            typStrona ts;
            ts = e.NewItem as typStrona;
            ts.CzyPowod = true;
            ts.Adres.Add(new typAdres());

        }

        private void ListBoxFakt_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            tekstSlownik txt;
            typDowod dowod;

            if (e.AddedItems.Count > 0)
            {
                txt = e.AddedItems[0] as tekstSlownik;
                dowod = this.dataFormDow.CurrentItem as typDowod;
                if (dowod != null)
                {
                    if (dowod.FaktStwierdzany != null)
                        dowod.FaktStwierdzany = dowod.FaktStwierdzany + ' ' + txt.Tresc;
                    else
                        dowod.FaktStwierdzany = txt.Tresc;
                    if (dowod.Opis != null)
                        dowod.Opis = dowod.Opis + ' ' + txt.Nazwa;
                    else
                        dowod.Opis = txt.Nazwa;

                }
            }

            if (_faktbutton != null)
            {
                _faktbutton.IsOpen = false;
                _faktbutton = null;
            }

        }



        private void FaktSelector_DropDownOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            _faktbutton = sender as RadDropDownButton;
        }

        private string ParseUzasadnienie(string uzasadnienie, string nrEwid)
        {
            
            string value;
            int nr = 0;

            foreach (string uitem in SpecialTags)
            { 
                if (uzasadnienie.Contains(uitem))
                { value = "";
                    switch (uitem)
                    { 
                        case "<$DataUmowy$>":
                            foreach (typDowod rr in pozew.ListaDowodow)
                            {
                                if (rr.TypDowodu == typRodzajDowodu.umowa)
                                {
                                    if (value.Length > 0)
                                        value += " ";
                                    value += rr.DataDowodu;
                                    
                                }
                            }       

                            break;
                        case "<$NumerUmowy$>" :
                            foreach (typDowod rr in pozew.ListaDowodow)
                            {
                                if (rr.TypDowodu == typRodzajDowodu.umowa)
                                {
                                    if (value.Length > 0)
                                        value += " ";
                                    value += rr.Oznaczenie;
                                    
                                }
                            }       

                            break;
                        case "<$ListaFaktur$>":
                            value = "";
                            foreach (typDowod rr in pozew.ListaDowodow)
                            {
                                if (rr.TypDowodu == typRodzajDowodu.faktura)
                                {
                                    if (value.Length > 0)
                                       value += "\n\r";
                                       nr++;
                                       value += nr.ToString() + rr.Opis + " o numerze " + rr.Oznaczenie + (rr.DataDowodu != null && rr.DataDowodu !="" ?  " z dnia " + rr.DataDowodu :"" );  
                                                                        
                                }
                            }
                            break;
                        case "<$ListaNot$>":
                            value = "";
                            foreach (typDowod rr in pozew.ListaDowodow)
                            {
                                if (rr.TypDowodu == typRodzajDowodu.inny && (rr.Opis.ToLower().Contains("not") && rr.Opis.ToLower().Contains("odsetkow")))
                                {
                                    if (value.Length > 0)
                                        value += "\n";
                                    nr++;
                                    value += nr.ToString() + rr.Opis + " o numerze " + rr.Oznaczenie + (rr.DataDowodu != null && rr.DataDowodu != "" ? " z dnia " + rr.DataDowodu : "");

                                }
                            }
                            break;
                        case "<$ListaDok$>":
                            value = "";
                            foreach (typDowod rr in pozew.ListaDowodow)
                            {
                                if ((rr.TypDowodu == typRodzajDowodu.faktura) || (rr.TypDowodu == typRodzajDowodu.inny && (rr.Opis.ToLower().Contains("not") && rr.Opis.ToLower().Contains("odsetkow"))) || (rr.TypDowodu == typRodzajDowodu.inny && (rr.Opis.ToLower().Contains("wezwanie") && rr.Opis.ToLower().Contains("termin"))))
                                {
                                    if (value.Length > 0)
                                        value += "\n";
                                    nr++;
                                    value += nr.ToString() + " " + rr.Opis + " o numerze " + rr.Oznaczenie + (rr.DataDowodu != null && rr.DataDowodu != "" ? " z dnia " + rr.DataDowodu : "");

                                }
                            }
                            break;
                        case "<$WPS$>":
                                value = String.Format("{0:C}",pozew.WartoscSporu);
                                break;

                        case "<$NumerKontrahenta$>":
                            value = NumerKontrahenta;
                            break;


                    }

                    uzasadnienie = uzasadnienie.Replace(uitem,value);
                }
            }
            return uzasadnienie;
        }



        private void ParseListaDowodow()
        {

            string value;
            int nr = 0;
            foreach (typDowod td in pozew.ListaDowodow)
            {
                if (td.Opis.Contains("<$DataDowodu$>"))
                {
                    td.Opis = td.Opis.Replace("<$DataDowodu$>", td.DataDowodu);

                }
                if (td.FaktStwierdzany.Contains("<$DataDowodu$>"))
                {
                    td.FaktStwierdzany = td.FaktStwierdzany.Replace("<$DataDowodu$>", td.DataDowodu);

                }

            }
        }

        private void ListBoxUzasadnienie_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            tekstSlownik txt;
            string przedtxt;
            string potxt;
            string TextIns;
            string _uzasadnienie;

            if (e.AddedItems.Count > 0)
            {
                txt = e.AddedItems[0] as tekstSlownik;
                przedtxt = this.UzasadTextBox.Text.Substring(0, this.UzasadTextBox.SelectionStart);
                potxt = this.UzasadTextBox.Text.Substring(this.UzasadTextBox.SelectionStart);
                TextIns = txt.Tresc;
                TextIns = ParseUzasadnienie(TextIns,"");


                _uzasadnienie = (przedtxt + " " + TextIns + " " + potxt).Trim();

                  this.pozew.Uzasadnienie = _uzasadnienie;

            }

            if (_uzasadnieniebutton != null)
            {
                _uzasadnieniebutton.IsOpen = false;
                _uzasadnieniebutton = null;
            }
        }

        private void UzasadnienieSelector_DropDownOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            _uzasadnieniebutton = sender as RadDropDownButton;
        }

        private void OpenSpr_Click(object sender, System.Windows.RoutedEventArgs e)
        {


            SprawaWindow sprwindow = new SprawaWindow();
            sprwindow.ViewSprawa.IdSprawy = (int)pozew.ID;
            sprwindow.Show();

        }

        private void updateSelectedDowod()
        {
            typRoszczenie tr;
            
            if (radGridViewRoszczenia.SelectedItems.Count > 0 )
            {
            tr = radGridViewRoszczenia.SelectedItem as typRoszczenie;
            if (tr.Dowody != null)
                tr.Dowody.Clear();
            else
                tr.Dowody = new ObservableCollection<int>();
            foreach (var td in radGridViewDowodyZaznacz.SelectedItems)
            {
                tr.Dowody.Add((td as typDowod).Numer);
            }
            }
        }

        private void updateOdsetki(ref PozewEPU pozew)
        {
            
            if (pozew.ListaRoszczen.Count > 0)
            {
                foreach (var typr in pozew.ListaRoszczen)
                {
                    if ((typr as typRoszczenie).Odsetki.Count > 0)
                        (typr as typRoszczenie).czyodsetki = 1;
                    else
                        (typr as typRoszczenie).czyodsetki = 0;
                }

                
                }
        }


        private void radGridViewRoszczenia_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            typRoszczenie tr;

            //zapisanie istniejacych roszczeń 
            if (e.RemovedItems.Count > 0)
            { // zachowaj istniejące dwody
                tr = e.RemovedItems[0] as typRoszczenie;
                if (tr.Dowody != null)
                    tr.Dowody.Clear();
                else
                    tr.Dowody = new ObservableCollection<int>();

                foreach (var tdx in radGridViewDowodyZaznacz.SelectedItems)
                {
                    tr.Dowody.Add((tdx as typDowod).Numer); //td = tdx as typDowod;

                }

            }
            if (e.AddedItems.Count > 0)
            {
                tr = e.AddedItems[0] as typRoszczenie;
                if (tr.Dowody != null)
                {
                    radGridViewDowodyZaznacz.SelectedItems.Clear();
                    foreach (var tdx in (radGridViewDowodyZaznacz.ItemsSource as QueryableCollectionView))
                    {
                        if (tr.Dowody.Contains((tdx as typDowod).Numer))
                            //(tdx as typDowod).IsSelected = true;
                            radGridViewDowodyZaznacz.SelectedItems.Add(tdx);

                    }

                }
            }
            if (pozew.CurrentRoszczenie == null)
            {
                if (e.AddedItems.Count > 0)
                {
                    tr = e.AddedItems[0] as typRoszczenie;
                    pozew.CurrentRoszczenie = tr;
                }
            }
                if (pozew.CurrentRoszczenie != null)
            {
                if (pozew.CurrentRoszczenie.Odsetki == null) pozew.CurrentRoszczenie.Odsetki = new ObservableCollection<typOkresOdsetkowy>();
                this.gridViewOdsetkiNal.ItemsSource = pozew.CurrentRoszczenie.Odsetki;
                if (pozew.CurrentRoszczenie.Odsetki.Count > 0)
                    pozew.CurrentRoszczenie.czyodsetki = 1;
                else
                    pozew.CurrentRoszczenie.czyodsetki = 0;
            }

        }

        public void SaveContent(object sender)
        {
            DateTime? nullDateTime = null;

            LexEnaMeritumDomainContext _context = new LexEnaMeritumDomainContext();
            LoadOperation<DokWys> loadop;
            EntityQuery<DokWys> query =
            from c in _context.GetDokWysWithPozewByIdQuery(this.IdDoc)
            select c;
            if (sender != null)
                (sender as Button).IsEnabled = false;
            try
            {
                loadop = _context.Load(query);
                loadop.Completed += (sd, ea) =>
                {
                    DokWys dok;
                    Pozew poz;

                    dok = loadop.Entities.FirstOrDefault();
                    if (dok.Pozew != null)
                    {
                        poz = dok.Pozew.FirstOrDefault();
                        if (poz != null)
                        {
                            poz.Tresc = this.pozewSerialized;
                            dok.Tresc = this.pozewSerialized;
                            dok.Koszty = this.KosztySadowe;
                            dok.WPS = this.WartPrzedmSporu;
                            dok.InneKoszty = this.KosztyInne;
                            dok.Kzp = this.Kzp;
                            dok.NotyOdsetkowe = this.NotyOdsetkowe;
                            dok.OdsetkiKapital = this.OdsNalicz;
                            dok.OdsNalicz = this.odsNaliczSerialized;
                            dok.DataDok = (this.dWniesienia > new DateTime(2010,1,10) ? this.dWniesienia : nullDateTime);
                            _context.SubmitChanges().Completed += (obj, evargs) =>
                            {
                                if (sender != null)
                                    (sender as Button).IsEnabled = true;
                            };



                        }

                    }
                };
            }
            catch (Exception ex)
            { 
                ErrorWindow.CreateNew(ex);
            
            }

        }
            
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
          
            // zapisanie pozwu do bazy 
            SaveContent(sender);
        }

        private void UzasadTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void dataFormDow_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            int newNr;
            typDowod newitem;
            typDowod tdmax;

            tdmax = pozew.ListaDowodow.OrderByDescending(a => a.Numer).FirstOrDefault();
            if (tdmax == null)
                newNr =  1;
            else
                newNr =   tdmax.Numer + 1;
            newitem = (typDowod)e.NewItem;
            newitem.Numer = newNr;

        }
   
        private void ListaAkcji_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.ShowDocs.IsOpen = false;

            if (e.AddedItems.Count > 0)
            {
                int option = (sender as System.Windows.Controls.ListBox).SelectedIndex;
          //      (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;
                switch (option)
                {
                    case 0:
                        this.saveActionType = 0;  //   wyświetlenie  strony 
                        break;
                    case 1:
                        this.saveActionType = 1;  // Eksport
                        break;
                        
                    default:
                        this.saveActionType = 0;

                        break;
                }
                
                PokazButtonClicked();
                ListaAkcji.SelectedIndex = -1;

            }
        }

        private void dataFormRoszczenia_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            ;
        }

        private void dataFormDow_CurrentItemChanged(object sender, EventArgs e)
        {
            ;
        }

        private void dataFormDow_EditEnding(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndingEventArgs e)
        {
            ;
        }

        private void dataFormDow_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            ;
        }

        private void DataFormPozwani_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            ;
        }
    }

        
    }

   

