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
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls;
using System.IO;

namespace LexEnaTrs.Views
{
    public partial class ListaDokOdebrGrid : UserControl
    {
        private int CurrentSprID;
        private int CurrentDokID;
        // Parametry publiczne

        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int TypDok = -1;

        private int saveActionType = 0;

        public ListaDokOdebrGrid()
        {
            CurrentSprID = 0;
            CurrentDokID = 0;
            InitializeComponent();
        }





        private void OpenSpr_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                SprawaWindow sprwindow = new SprawaWindow();
                sprwindow.ViewSprawa.IdSprawy = CurrentSprID;
                sprwindow.ViewSprawa.Id_Jednostki = UserProfile.IdJednostki;
                sprwindow.ViewSprawa.Id_User = UserProfile.DbId;
                sprwindow.Show();
            }
            else
            {
                AlertMsg.Show("Wybierz sprawę do edycji");
            }


        }


        private void SprawyGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            vw_ListaDoOdebr lstSpr;

            if (e.AddedItems.Count > 0)
            {
                lstSpr = (vw_ListaDoOdebr)e.AddedItems[0];
                CurrentSprID = lstSpr.IdSprawy;
                CurrentDokID = lstSpr.Id;
            }


        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.vw_ListadokOdebrdds.QueryParameters[0].Value = IdUser;
            if (UserProfile.Rola < 2)
                this.vw_ListadokOdebrdds.QueryParameters[1].Value = IdJednostki;
            else
                this.vw_ListadokOdebrdds.QueryParameters[1].Value = -1;
            this.vw_ListadokOdebrdds.QueryParameters[2].Value = TypDok;
            this.vw_ListadokOdebrdds.Load();
        }

        private void vw_ListadokOdebrdds_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Błąd odczytu danych", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();

            }
            else
                (MainWorkspaceWindowHandler.MainViewHandler as MainView).RefreshTreeView();

        }





        private void HtmlDocCompleted(InvokeOperation<string> html)
        {

            string htmlpath;

            htmlpath = html.Value;//e.Result.ToString();
            if (htmlpath.ToLower().Contains("błąd") || htmlpath.ToLower().Contains("error"))
                ErrorWindow.CreateNew(htmlpath);
            else
            {

                htmlpath = html.Value;//e.Result.ToString();

                /*Uri uri = new Uri(Application.Current.Host.Source, htmlpath);

                HtmlWindow window = HtmlPage.Window;
                window.Navigate(uri, "_blank");
                */
                HtmlViewer vw = new HtmlViewer();
                vw.content = htmlpath;
                vw.Show();



                /*
                Uri uri = new Uri(Application.Current.Host.Source, htmlpath);
                HtmlWindow window = HtmlPage.Window;
                window.Navigate(uri, "_blank");
                this.vw_ListadokOdebrdds.Load();
                */
            }
        }

        private void ViewButtonClick()
        {
            int IdDoc;
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                IdDoc = (this.SprawyGridView.SelectedItem as vw_ListaDoOdebr).Id;
                PozewDomainContext _pozewcontext;
                if (IdDoc > 0)
                {
                    _pozewcontext = new PozewDomainContext();
                    _pozewcontext.GetHtmlDocOdebr(IdDoc, (int)UserProfile.Rola, (int)UserProfile.DbId, HtmlDocCompleted, null);
                }



            }

        }

        /*
        private void MarkAsRead()
        {
            
            
            LexEnaMeritumDomainContext _LexEnaContext = new LexEnaMeritumDomainContext();  //radaDomainDataSource.DomainContext;

            

             EntityQuery<OdsTab> query =
                from c in _LexEnaContext.GetOdsTabQuery()
                orderby c.DataP ascending
                select new { c;
            loadop = _nazodscontext.Load(query);
            loadop.Completed += (sender, e) =>
            {
                foreach (var r in loadop.Entities)
                {
                    TabelaOdsetekUstawowych.Tabela.Add(r);
                }


            };
            using

            this.vw_ListadokOdebrdds.Load();
        
        }
        */
        private void PokazButtonClicked()
        {
            List<int> IdLst = new List<int>();
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                if (saveActionType == 0)
                {
                    // podgląd 
                }
                else
                {
                    foreach (vw_ListaDoOdebr dokP in this.SprawyGridView.SelectedItems)
                    {
                        IdLst.Add(dokP.Id);
                    }

                    ReportWindow repwin = new ReportWindow();
                    repwin.Mode = 103;
                    repwin.StringArg = String.Join(";", IdLst.Select(x => x.ToString()).ToArray());
                    repwin.Show();
                    /*
                    PozewDomainContext _pozewcontext;
                    _pozewcontext = new PozewDomainContext();
                    _pozewcontext.GetListaDokOdebr(ToXMLSerializers.SerializeToString(IdLst, typeof(List<int>)), (int)UserProfile.Rola, (int)UserProfile.DbId, DokOdebrToSaveCompleted, null);
                    */
                }
            }


        }

        private void DokOdebrToSaveCompleted(InvokeOperation<string> html)
        {

            string htmlpath;

            htmlpath = html.Value;//e.Result.ToString();
            Uri uri = new Uri(Application.Current.Host.Source, htmlpath);
            if (uri.AbsoluteUri.Length < 5 || uri.AbsoluteUri.Contains("error") || uri.AbsoluteUri.Contains("błąd"))
            {
                ErrorWindow.CreateNew("Błąd generacji zbioru ");
                return;
            }

            DaneDoEksportuReady wnd = new DaneDoEksportuReady();
            wnd.Show();
            wnd.Closed += (obj, e) =>
            {
                SaveFileDialog sfd = new SaveFileDialog();

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Hlml files (*.html)|*.html|All Files (*.*)|*.*";

                bool? result = saveDialog.ShowDialog();
                if (result.Value)
                {
                    using (Stream saveStream = saveDialog.OpenFile())
                    using (StreamWriter saveWriter = new StreamWriter(saveStream))
                    {
                        saveWriter.Write(html.Value);
                    }
                }
            };
            /*
            DownloadManager dwnMgr = new DownloadManager();
            dwnMgr.downloadButton = this.ShowDocs;
            dwnMgr.ServerFileUri = uri.AbsoluteUri;
            dwnMgr.DownloadnSave(0);
            this.vw_ListadokOdebrdds.Load();
            //(MainWorkspaceWindowHandler.MainViewHandler as MainView).RefreshTreeView();
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
                        ViewButtonClick();
                        break;
                    case 1:
                        saveActionType = 1;  // Eksport
                        PokazButtonClicked();
                        break;

                    default:
                        saveActionType = 0;
                        ViewButtonClick();
                        break;
                }
            }


        }

        private void vw_ListadokOdebrdds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;// TODO: Add event handler implementation here.
        }

        private void SprawyGridView_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            var row = e.Row as GridViewRow;

            if (row != null)
            {

                row.Cells[1].Content = this.RadDataPagerSprawy.PageIndex * this.RadDataPagerSprawy.PageSize + this.SprawyGridView.Items.IndexOf(row.Item) + 1;
            }
        }

        private void ListaAkcjiDok_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            int i;
            int dokType;
            this.MakeDok.IsOpen = false;

            if (e.AddedItems.Count > 0)
            {
                i = (sender as System.Windows.Controls.ListBox).SelectedIndex;
                (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;
                switch (i)
                {
                    case 0:
                        dokType = 3;  // pismo    //   Podgląd
                        break;
                    case 1:
                        dokType = 4;  // wniosek    //   Podgląd
                        break;
                    case 2:
                        dokType = 5;  //Uzupenienie adresu  //   Podgląd
                        break;
                    case 3:
                        dokType = 6;  //Uzupenienie braków
                        break;
                    case 4:
                        dokType = 13;  //Rezygnacjka z pełnomocnictwa
                        break;
                    case 5:
                        dokType = 14;  // Zgłoszneie pełnomocnika do sprawy
                        break;
                    case 6:
                        if (TypDok != 17) // jeśli nie kaluzule
                        { ErrorWindow.CreateNew("Wnioski egzekucyjne można tworzyć tylko z kontekstu tytułów wykonawczych");
                            return;
                        }
                        else
                            dokType = 30;  // Wniosek Egzekucyjny
                        break;
                    default:
                        dokType = 3;
                        break;
                }
                MakeDocuments(dokType);


            }
        }


        private void MakeDocuments(int dokType)
        {// sporządzenie pzwów dla wybranych spraw.
            List<int> IdSpr = new List<int>();  // tablica na Id spraw
            int tabindex = 0;
            string lstSerialized;


            // Wczytanie słownika
            foreach (var r in this.SprawyGridView.SelectedItems)
            {
                IdSpr.Add((r as vw_ListaDoOdebr).IdSprawy);
                tabindex++;

            }



            if (tabindex > 0)
            {
                this.BusyIndicator.IsBusy = true;
                this.BusyIndicator.BusyContent = "Traw generacja dokumentu proszę czekać...";
                //  GenerateDocuments(string listaIdSpr,int docType, int IdKontaEPU)
                PozewDomainContext _pozewcontext = new PozewDomainContext();
                lstSerialized = ToXMLSerializers.SerializeToString(IdSpr, typeof(List<int>));
                if (dokType == 30)   // jełśi wnioski egzekucyjne
                    _pozewcontext.GenerateWniosek(lstSerialized, (int)UserProfile.IdKontaEPU, DateTime.Today, 1, GenrationCompleted, null);
                else
                    _pozewcontext.GenerateDocuments(lstSerialized, dokType, (int)UserProfile.IdKontaEPU, GenrationCompleted, null);
            }
        }

        private void GenrationCompleted(InvokeOperation<string> result)
        {
            if (result.Value.ToLower().Contains("error") || result.Value.ToLower().Contains("błąd"))
            {
                ErrorWindow.CreateNew(result.Value);

            }
            this.BusyIndicator.IsBusy = false;

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



        /*
        private void OnConfirmed2(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                this.BusyIndicator.IsBusy = true;
                EpuContext pozContext = new EpuContext();
                pozContext.SprawdzPolaczenie() InsertJobs(UserProfile.DbId, InsertJobsCompleted, null);

            }
        }
        */

        private void OnConfirmed2(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                this.BusyIndicator.IsBusy = true;
                PozewDomainContext pozContext = new PozewDomainContext();
                pozContext.InsertJobs(UserProfile.DbId, InsertJobsCompleted, null);

            }
        }


        private void OnConfirmed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                PozewDomainContext pozContext = new PozewDomainContext();
                pozContext.InsertJobs(UserProfile.DbId, InsertJobsCompleted, null);

            }
        }


        private void InsertJobsCompleted(InvokeOperation<string> result)
        {

            if (result.Value.ToLower().Contains("error") || result.Value.ToLower().Contains("błąd"))
            {
                this.BusyIndicator.IsBusy = false;
             
                ErrorWindow.CreateNew(result.Value);

            }
            else
            {

                EpuContext epuKontext = new EpuContext();
                this.BusyIndicator.IsBusy = true;
              
                epuKontext.Sprawdz(Sprawdz_Completed, null);


            }

        }
        private void Sprawdz_Completed(InvokeOperation<string> result)
        {

            if (result.HasError)
            {
                this.BusyIndicator.IsBusy = false;
             
                ErrorWindow.CreateNew(result.Error.Message);

            }
            else
            {

                if (result.Value != null)
                {
                    try
                    {
                        List<TypSlownikFiltered> LstReturn = (List<TypSlownikFiltered>)ToXMLSerializers.XmlDeserializeFromString(result.Value, typeof(List<TypSlownikFiltered>));
                        if (LstReturn != null)
                        {
                            PozewDomainContext pozContext = new PozewDomainContext();
                            pozContext.SynchroPozew(ToXMLSerializers.SerializeToString(LstReturn, typeof(List<TypSlownikFiltered>)), UserProfile.DbId, UserProfile.Firma, PublishCompleted, null);


                        }

                    }
                    catch (Exception ex)
                    {

                        ;
                    }
                }
                
                this.BusyIndicator.IsBusy = false;


            }
        }
        private void PublishCompleted(InvokeOperation<string> result)
        {

            if (result.Value.ToLower().Contains("error") || result.Value.ToLower().Contains("błąd"))
            {
                this.BusyIndicator.IsBusy = false;
         
                ErrorWindow.CreateNew(result.Value);

            }
            else
            {
                this.vw_ListadokOdebrdds.Load();
                CurrentDokID = 0;
                this.vw_ListadokOdebrdds.Load();
                this.BusyIndicator.IsBusy = false;
                
                AlertMsg.Show("Dane zostały pobrane i poprawnie zsynchronizowane z systemami Energii");

            }

        }

        private void SprawyGridView_Filtered(object sender, GridViewFilteredEventArgs e)
        {
            this.vw_ListadokOdebrdds.Load();
        }
    } 
    
}
