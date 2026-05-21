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
using Telerik.Windows.Controls;
using System.IO;
using System.Windows.Browser;
using Telerik.Windows.Controls.GridView;

namespace LexEnaTrs.Views
{
    public partial class ListaPaczek : UserControl
    {
        private int CurrentSprID;
        private int CurrentDokID;
        private LexEnaMeritumDomainContext _context;
        private PozewDomainContext _pozewcontext;
        // Parametry publiczne

        private ExportFormat ExFormat;
        private string format;
        private string extension;

        public int IdJednostki = -1;
        public int StatPaczki = -1;
        public int TypDok = -1;

        private int saveActionType = 0;


        public ListaPaczek()
        {
            CurrentSprID = 0;
            CurrentDokID = 0;
            InitializeComponent();
            _context = new LexEnaMeritumDomainContext();
        }

        private class operEventArgs : EventArgs
        {
            private int _context;
            private int _idPaczki;
            public int Context
            {
                get { return _context; }
                set { this._context = value; }

            }

            public int IdPaczki
            {
                get { return _idPaczki; }
                set { this._idPaczki = value; }

            }
        }
        private event EventHandler operCompleted;
        protected virtual void OnoperCompleted(EventArgs e)
        {
            if (operCompleted != null)
                operCompleted(this, e);
        }



        private void OpenSpr_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                SprawaWindow sprwindow = new SprawaWindow();
                sprwindow.ViewSprawa.IdSprawy = CurrentSprID;
                sprwindow.Show();
            }
            else
            {
                AlertMsg.Show("Wybierz sprawę do edycji");
            }


        }
        private void DoDekretSpr(int ItemId, int context)
        {
            // context - 0 - do jednostki, 1 do referenta;


        }

        private void SprawyGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            vw_ListaPaczek lstSpr;

            if (e.AddedItems.Count > 0)
            {
                lstSpr = (vw_ListaPaczek)e.AddedItems[0];

                CurrentDokID = lstSpr.Id;
                vw_ListadokWysPaczkidds.QueryParameters[0].Value = lstSpr.Id;
                vw_ListadokWysPaczkidds.Load();

            }


        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.vw_ListaPaczekdds.QueryParameters[0].Value = IdJednostki;
            this.vw_ListaPaczekdds.QueryParameters[1].Value = StatPaczki;
            this.vw_ListaPaczekdds.QueryParameters[2].Value = TypDok;
            switch (StatPaczki)
            {
                case 1: // w przygotowaniu

                    break;
                case 2:  // złożona
                    this.UsunPaczke.Visibility = Visibility.Collapsed;
                    this.SendParcel.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    this.UsunPaczke.Visibility = Visibility.Collapsed;
                    this.SendParcel.Visibility = Visibility.Collapsed;
                    this.ZmianaStatusu.Visibility = Visibility.Collapsed;
                    this.Wycofaj.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    this.UsunPaczke.Visibility = Visibility.Collapsed;
                    //this.SendParcel.Visibility = Visibility.Collapsed;
                    this.ZmianaStatusu.Visibility = Visibility.Collapsed;

                    break;
                default:
                    break;

            }


            // Tu ustaiwć widoczność poszczególnych kolumn this.SprawyGridView.Columns[""]
            this.vw_ListaPaczekdds.Load();



        }








        private void OnSubmitetdChanges(SubmitOperation so)
        {

            if (so.HasError)
            {
                ErrorWindow.CreateNew(string.Format(" {0}", so.Error.Message));
                so.MarkErrorAsHandled();
            }
            else
            {
                this.vw_ListaPaczekdds.Load();  // przeładowanie listy
            }
        }
        private void SetNewStatusKontoEPUPaczki(LexEnaMeritumDomainContext _context, int IdPaczki, int newStatus, int IdKonta)
        {
            Paczka _paczka;
            this.operCompleted += new EventHandler(ListaPaczek_operCompleted);
            LoadOperation<Paczka> loadop;
            EntityQuery<Paczka> query =
                from c in _context.GetPaczkaByIdQuery(IdPaczki)

                select c;

            loadop = _context.Load(query);
            loadop.Completed += (sender, e) =>
            {
                _paczka = ((loadop.Entities).FirstOrDefault() as Paczka);


                if (_paczka != null)
                {
                    _paczka.StatusPaczki = newStatus;
                    if (IdKonta > 0)
                        _paczka.KontoEPU_Id = IdKonta;
                    _context.SubmitChanges().Completed += (o, s) =>
                        {
                            operEventArgs ea = new operEventArgs();
                            ea.IdPaczki = _paczka.Id;
                            ea.Context = IdKonta;
                            OnoperCompleted(ea);
                        };
                }


            };
        }

        private void ListaPaczek_operCompleted(object sender, object e)
        {
            operEventArgs ea;
            ea = (operEventArgs)e;

            this.operCompleted -= ListaPaczek_operCompleted;

            if (ea.Context > 0)   //  z zapisema zadania
            {
                EpuContext epuKontext = new EpuContext();

                epuKontext.ZluzPozewZadanie(ea.IdPaczki, ea.Context, UserProfile.IdJednostki, Compl_task, null);

            }
            else
            {
                _context.SubmitChanges().Completed += (ob, eva) =>
                    {
                        this.vw_ListaPaczekdds.Load();

                    };

            }

        }


        private void ZmianaStatusu_Click(object sender, RoutedEventArgs e)
        {
            WyborStatusuPaczki selstaPaczki = new WyborStatusuPaczki();
            int newStatus;
            vw_ListaPaczek r;
            selstaPaczki.Show();
            selstaPaczki.Closed += (se, ea) =>
            {
                if (selstaPaczki.DialogResult == true)
                {
                    if (selstaPaczki.selectedStatus == 2)
                    {
                        AlertMsg.Show("Użyj funkcji wyślij dla wybranego statusu"); // wysłana paczkaDo wysyłki
                        return;
                    }

                    newStatus = selstaPaczki.selectedStatus;

                    r = (this.SprawyGridView.SelectedItem as vw_ListaPaczek);
                    SetNewStatusKontoEPUPaczki(_context, r.Id, newStatus, 0);

                }


            };

        }

        private void SendParcel_Click(object sender, RoutedEventArgs e)
        {

            vw_ListaPaczek r;
            r = (this.SprawyGridView.SelectedItem as vw_ListaPaczek);
            if (r == null) return;
            if (r.StatusPaczki == 3) // paczka złożona
            {
                DialogParameters _params = new DialogParameters();
                _params.CancelButtonContent = "Nie";
                _params.OkButtonContent = "Tak";
                _params.Header = "Potwierdź";
                _params.Content = " Ta paczka już została skutecznie wniesiona do e-Sądu. \n System  nie ma wiedzy o jej podpisaniu i opłaceniu.\n Przed powtórną wysyłką upewnij się, iż poprzednia nie została opłacona (najlepiej usuń ją z poziomu portalu)\n Czy na pewno chcesz wysłać paczkę ponownie ?";
                _params.Closed = OnConfirmed;
                RadWindow.Confirm(_params);





            }
            else
            {
                sendParcel(r);
            }

        }

        private void sendParcel(vw_ListaPaczek r)
        {
            WyborKontaJednostki selKontoEPU = new WyborKontaJednostki();
            selKontoEPU.Show();
            selKontoEPU.Closed += (obj, ea) =>
            {
                if (selKontoEPU.DialogResult == true)
                {
                    if (selKontoEPU.IDKontaEPU > 0)
                    {

                        SetNewStatusKontoEPUPaczki(_context, r.Id, 2, selKontoEPU.IDKontaEPU);
                    }
                }

            };


        }

        private void OnConfirmed(object sender, WindowClosedEventArgs e)
        {

            vw_ListaPaczek r;

            if (e.DialogResult == true)
            {
                r = (this.SprawyGridView.SelectedItem as vw_ListaPaczek);
                sendParcel(r);
            }
        }



        private void Compl_task(InvokeOperation<StatusZadania> status)
        {
            StatusZadania stzad = status.Value;
            if (stzad.Status < 0)
            {
                AlertMsg.Show(stzad.Opis);

            }
            _context.SubmitChanges().Completed += (ob, eva) =>
            {
                this.vw_ListaPaczekdds.Load();

            };
        }

        private void OpenPozew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowDocs_Click(object sender, RoutedEventArgs e)
        {


        }

        private void Wycofaj_Click(object sender, RoutedEventArgs e)
        {

            List<int> IdLista;
            string lstId;
            IdLista = new List<int>();
            foreach (var r in this.DokPaczkiGridView.SelectedItems)
            {
                IdLista.Add((r as vw_ListaDokPaczki).IdDokumentPaczka);
            }
            if (IdLista.Count > 0)
            {   // pewnie można prościej ale na razie ręczna serializacja.
                lstId = ToXMLSerializers.SerializeToString(IdLista, typeof(List<int>));
                if (_pozewcontext == null)
                    _pozewcontext = new PozewDomainContext();
                _pozewcontext.DeleteDokumentPaczka(lstId, DelPaczkaCompleted, null);

            }


        }

        private void DelPaczkaCompleted(InvokeOperation<int> status)
        {
            if (status.Value == -1)
            {
                ErrorWindow.CreateNew("Błąd zapisu w bazie danych");
                return;
            }
            if (status.Value == 1)
            {
                this.vw_ListadokWysPaczkidds.Load();

            }


        }








        private void FormatList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int reportMode;
            int dokId = 0;
            reportMode = 0;
            ExportRadDDB.IsOpen = false;

            if (e.AddedItems.Count > 0)
            {
                if (this.SprawyGridView.SelectedItems.Count > 0)
                {
                    dokId = (this.SprawyGridView.SelectedItem as vw_ListaPaczek).Id;
                }

                switch ((sender as System.Windows.Controls.ListBox).SelectedIndex)
                {
                    case 0:
                        reportMode = 1;
                        break;
                    case 1:
                        reportMode = 2;
                        break;
                    case 2:
                        reportMode = 3;
                        break;
                    default:
                        reportMode = 1;

                        break;
                }
            }
            (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;
            if (reportMode > 0 && dokId > 0)
            {
                ReportWindow repwin = new ReportWindow();
                repwin.IdPaczki = dokId;
                repwin.Mode = reportMode;
                repwin.Show();
                if (reportMode == 2) // potwierdzenia złożenia do sądu
                {
                    LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                    LoadOperation<Paczka> loadopPaczka;
                    EntityQuery<Paczka> selPaczka;
                    Paczka myPaczka;
                    _context = new LexEnaMeritumDomainContext();

                    selPaczka =
                        (from c in _context.GetPaczkaByIdQuery(dokId)
                         select c);

                    loadopPaczka = _context.Load(selPaczka);
                    loadopPaczka.Completed += (s, ea) =>
                     {

                         myPaczka = loadopPaczka.Entities.FirstOrDefault();
                         if (myPaczka != null)
                         {
                             myPaczka.CzyZestaw = 1;
                             _context.SubmitChanges(submitPaczkaCompleted, null);

                         }

                     };
                }

            }

        }

        private void submitPaczkaCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                ErrorWindow.CreateNew("Błąd zmiany statusu paczki");
                so.MarkErrorAsHandled();

            }
            else
                this.vw_ListaPaczekdds.Load();

        }



        private void PokazButtonClicked()
        {
            int IdDoc;
            if (this.DokPaczkiGridView.SelectedItems.Count > 0)
            {
                IdDoc = (this.DokPaczkiGridView.SelectedItem as vw_ListaDokPaczki).Id;
                LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                LoadOperation<DokWys> loadop;

                _context = new LexEnaMeritumDomainContext();

                EntityQuery<DokWys> query =
                    from c in _context.GetDokWysWithSprawaByIdQuery(IdDoc)

                    select c;

                loadop = _context.Load(query);
                loadop.Completed += (s, ea) =>
                {
                    DokWys dok;
                    string pozewSerialized;
                    PozewDomainContext _pozewcontext;

                    dok = loadop.Entities.FirstOrDefault();
                    if (dok != null)
                    {
                        pozewSerialized = dok.Tresc;
                        _pozewcontext = new PozewDomainContext();
                        int dokTyp = dok.TypDok;
                        if ((dok.TypDok >= 3 && dok.TypDok <= 6) || (dok.TypDok >= 14 && dok.TypDok <= 15)) dokTyp += 1000;
                        _pozewcontext.DokumentZEPU2HTML(pozewSerialized, dokTyp, PozewToSaveCompleted, null);


                    }


                };
            }


        }


        private void ViewButtoClick()
        {
            int IdDoc;
            if (this.DokPaczkiGridView.SelectedItems.Count > 0)
            {
                IdDoc = (this.DokPaczkiGridView.SelectedItem as vw_ListaDokPaczki).Id;
                LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                LoadOperation<DokWys> loadop;

                _context = new LexEnaMeritumDomainContext();

                EntityQuery<DokWys> query =
                    from c in _context.GetDokWysWithSprawaByIdQuery(IdDoc)

                    select c;

                loadop = _context.Load(query);
                loadop.Completed += (s, ea) =>
                {
                    DokWys dok;
                    string pozewSerialized;
                    PozewDomainContext _pozewcontext;

                    dok = loadop.Entities.FirstOrDefault();
                    if (dok != null)
                    {
                        pozewSerialized = dok.Tresc;
                        _pozewcontext = new PozewDomainContext();
                        int dokTyp = dok.TypDok;
                        if ((dok.TypDok >= 3 && dok.TypDok <= 6) || (dok.TypDok >= 14 && dok.TypDok <= 15)) dokTyp += 1000;
                        _pozewcontext.DokumentZEPU2HTML(pozewSerialized, dokTyp, PozewEPUCompleted, null);


                    }


                };
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

        private void ListaAkcji_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int i;
            this.ShowDocs.IsOpen = false;

            if (e.AddedItems.Count > 0)
            {
                i = (sender as System.Windows.Controls.ListBox).SelectedIndex;
                (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;
                switch (i)
                {
                    case 0:
                        saveActionType = 0;  //   Podgląd
                        ViewButtoClick();
                        break;
                    case 1:
                        saveActionType = 1;  // Eksport
                        PokazButtonClicked();
                        break;

                    default:
                        saveActionType = 0;
                        ViewButtoClick();
                        break;
                }

            }
        }

        private void DokPaczkiGridView_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            var row = e.Row as GridViewRow;

            if (row != null)
            {

                row.Cells[1].Content = this.RadDataPagerDokPaczki.PageIndex * this.RadDataPagerDokPaczki.PageSize + this.DokPaczkiGridView.Items.IndexOf(row.Item) + 1;
            }
        }

        private void vw_ListaPaczekdds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;

        }


        private void setKontoEPU()
        {

            List<int> IdDok = new List<int>();
            WyborKontaJednostki selKEPU = new WyborKontaJednostki();
            selKEPU.Show();

            selKEPU.Closed += (sndr, ex) =>
            {
                if (selKEPU.DialogResult == true)
                {

                    if (selKEPU.IDKontaEPU > 0)
                    {
                        foreach (var r in this.DokPaczkiGridView.SelectedItems)
                        {

                            IdDok.Add((r as vw_ListaDokPaczki).Id);
                            if (IdDok.Count > 0)
                            {
                                this.BusyIndicator.IsBusy = true;
                                PozewDomainContext context = new PozewDomainContext();
                                string idSprLst;
                                string idDokLst;
                                idDokLst = ToXMLSerializers.SerializeToString(IdDok, typeof(List<int>));
                                context.ChangeRadcaDok(idDokLst, selKEPU.IDKontaEPU, StatusChangedCompleted, null);

                            }
                        }

                    }
                }

            };
        }

        private void StatusChangedCompleted(InvokeOperation<string> result)
        {
            if (result.Value.ToLower().Contains("error") || result.Value.ToLower().Contains("błąd"))
            {
                ErrorWindow.CreateNew(result.Value);

            }
            else
                this.vw_ListadokWysPaczkidds.Load();

            this.BusyIndicator.IsBusy = false;

        }

        private void OperacjeLst_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // zmiana daty dokumentów   - najlepiej po stronie serwera

            int i;
            List<int> IdList;
            string strList;
            this.OtherRadDropDownButton.IsOpen = false;
            DateTime? newDate;
            PozewDomainContext _pozewcontext;
            if (e.AddedItems.Count > 0)
            {
                i = (sender as System.Windows.Controls.ListBox).SelectedIndex;
                (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;
                switch (i)
                {
                    case 0:
                        // 
                        if (this.DokPaczkiGridView.SelectedItems.Count > 0)
                        {
                            IdList = new List<int>();
                            foreach (vw_ListaDokPaczki dw in this.DokPaczkiGridView.SelectedItems)
                            {
                                IdList.Add(dw.Id);
                            }
                            // zmiana daty dokumentów
                            strList = ToXMLSerializers.SerializeToString(IdList, typeof(List<int>));

                            GetDateWindow wnd = new GetDateWindow();
                            wnd.Show();
                            wnd.Closed += (se, ea) =>
                            {
                                if (wnd.DialogResult == true)
                                {
                                    newDate = wnd.ChosenDate;
                                    _pozewcontext = new PozewDomainContext();
                                    _pozewcontext.ChangeDocDate(strList, (DateTime)newDate,UserProfile.Firma , ChangeDocDateCompleted, null);

                                }
                            };                    //  ChangeDocDate(string listaIdDoc, DateTime newDate) invoke 
                        }
                        break;
                    case 1:  // zmiana pełnomocnika
                        setKontoEPU();

                        break;
                    default:
                        saveActionType = 0;

                        break;
                }

            }
        }

        private void ChangeDocDateCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            if (message == "OK")
            {
                this.vw_ListadokWysPaczkidds.Load();
            }
            else
                ErrorWindow.CreateNew(message);


        }

        private void vw_ListadokWysPaczkidds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {

            e.LoadBehavior = LoadBehavior.RefreshCurrent;

        }

        private void SynchronizeEpu_Click(object sender, RoutedEventArgs e)
        {
            // synchronozacja z EPU
            DialogParameters _params = new DialogParameters();
            _params.CancelButtonContent = "Nie";
            _params.OkButtonContent = "Tak";
            _params.Header = "Potwierdź";
            _params.Content = " Czy chcesz wysłać żądanie sprawdzenia stanu do e-Sądu?";
            _params.Closed = OnConfirmed2;
            RadWindow.Confirm(_params);

        }

        private void OnConfirmed2(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                this.BusyIndicator.IsBusy = true;
                this.BusyIndicator2.IsBusy = true;
                PozewDomainContext pozContext = new PozewDomainContext();
                pozContext.InsertJobs(UserProfile.DbId, InsertJobsCompleted, null);

            }
        }

        private void InsertJobsCompleted(InvokeOperation<string> result)
        {
             
            if (result.Value.ToLower().Contains("error") || result.Value.ToLower().Contains("błąd"))
            {
                this.BusyIndicator.IsBusy = false;
                this.BusyIndicator2.IsBusy = false;
                ErrorWindow.CreateNew(result.Value);

            }
            else
            {
                DomainClient dc;
                
                EpuContext epuKontext = new EpuContext();
                this.BusyIndicator.IsBusy = true;
                this.BusyIndicator2.IsBusy = true;
                try
                {
                    epuKontext.Sprawdz(Sprawdz_Completed, null);
                }
                catch (Exception x)
                {

                    ;
                }

            }

        }
        private void Sprawdz_Completed(InvokeOperation<string> result)
        {
             
            if (result.HasError)
            {
                this.BusyIndicator.IsBusy = false;
                this.BusyIndicator2.IsBusy =false;
                ErrorWindow.CreateNew(result.Error.Message);

            }
            else
            {
                this.BusyIndicator2.IsBusy = false;
                this.BusyIndicator.IsBusy = false;


                if (result.Value != null )
                {
                    try
                    {
                        List<TypSlownikFiltered> LstReturn = (List<TypSlownikFiltered>)ToXMLSerializers.XmlDeserializeFromString(result.Value, typeof(List<TypSlownikFiltered>));
                        if (LstReturn != null)
                        {
                            this.BusyIndicator2.IsBusy = true;
                            this.BusyIndicator.IsBusy = true;

                            PozewDomainContext pozContext = new PozewDomainContext();
                            pozContext.SynchroPozew( ToXMLSerializers.SerializeToString( LstReturn , typeof (List<TypSlownikFiltered>)), UserProfile.DbId, UserProfile.Firma , PublishCompleted, null);
                           

                        }

                    }
                    catch (Exception ex)
                    {
                        this.BusyIndicator2.IsBusy = false;
                        this.BusyIndicator.IsBusy = false;

                        ;
                    }
                }
               

            }
        }
        private void  PublishCompleted(InvokeOperation<string> result)
        {
            this.BusyIndicator.IsBusy = false;
            this.BusyIndicator2.IsBusy = false;
            if (result.Value.ToLower().Contains("error") || result.Value.ToLower().Contains("błąd"))
            {
             
                ErrorWindow.CreateNew(result.Value);

            }
            else
            {
                this.vw_ListadokWysPaczkidds.Load();
                CurrentDokID = 0;
                vw_ListadokWysPaczkidds.QueryParameters[0].Value = 0;
                vw_ListadokWysPaczkidds.Load();
                this.BusyIndicator.IsBusy = false;
                this.BusyIndicator2.IsBusy = false;
                AlertMsg.Show("Dane zostały pobrane i poprawnie zsynchronizowane z systemami Energii");

            }

        }

        private void TestSynchro_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(IdSprawaOutput.Text);
            List<TypSlownikFiltered> LstReturn = new List<TypSlownikFiltered>();
            TypSlownikFiltered ts = new TypSlownikFiltered();
            ts.Filter1 = 3;
            ts.Numer = 350;
            LstReturn.Add(ts);
            PozewDomainContext pozContext = new PozewDomainContext();
            pozContext.SynchroPozew(ToXMLSerializers.SerializeToString(LstReturn, typeof(List<TypSlownikFiltered>)), UserProfile.DbId, UserProfile.Firma, PublishCompleted, null);
        }
    }
}
