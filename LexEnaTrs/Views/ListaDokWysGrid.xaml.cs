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

namespace LexEnaTrs.Views
{
    public partial class ListaDokWysGrid : UserControl
    {
        private int CurrentSprID;
        private int CurrentDokID;
        //private int[] IdDw = new int[] { 2556, 17004, 19496, 22310, 22155, 22311, 21856, 21781, 21769, 21773, 21779, 21774, 21860, 21867, 21861, 21977, 21987, 21981, 21979, 21973, 21996, 21990, 22049, 21941, 22007, 22001, 21882, 21886, 21879, 21877, 21873, 21871, 21883, 21966, 21967, 21984, 22027, 22025, 21995, 21982, 21997, 22308, 21991, 21972, 21986, 21970, 21978, 21975, 21998, 21974, 21968, 21869, 21870, 21887, 21888, 21890, 22053, 21950, 21962, 21960, 21952, 21961, 21951, 22064, 21946, 22019, 21944, 22061, 22017, 21945, 22003, 22002, 22008, 22011, 22005, 21918, 21893, 21913, 21897, 21914, 21899, 21909, 21900, 21902, 21896, 21916, 21904, 21911, 21892, 22062, 22066, 22067, 22063, 22068, 22016, 22010, 22014, 22050, 22046, 22029, 22018, 22031, 22032, 22020, 22041, 22042, 22037, 22035, 22043, 22038, 22039, 22036, 21957, 21959, 21964, 21965, 21955, 21958, 21956, 21954, 22059, 21953, 21881, 22315, 21894, 21903, 21905, 21912, 21922, 21930, 21940, 21891, 21895, 21910, 21920, 21921, 21925, 21929, 21932, 21889, 21927, 21938, 21935, 21937, 21939, 22316, 21934, 22491, 22144, 22077, 22090, 22092, 22085, 22084, 22082, 22079, 22080, 22081, 22086, 22091, 22089, 22088, 22078, 22122, 22124, 22118, 22209, 22099, 22098, 22141, 22138, 22097, 22100, 22211, 22168, 22205, 22203, 22202, 22200, 22095, 22102, 22094, 22096, 22101, 22207, 22173, 22172, 22171, 22210, 22214, 22104, 22106, 22112, 22107, 22163, 22167, 22165, 22166, 22119, 22121, 22123, 22126, 22152, 22149, 22147, 22146, 22145, 22206, 22199, 22198, 22197, 22196, 22191, 22189, 22180, 22194, 22190, 22178, 22182, 22195, 22177, 22183, 22193, 22176, 22186, 22159, 22157, 22161, 22244, 22230, 22327, 22234, 22325, 22323, 22232, 22331, 22322, 22324, 22329, 22321, 22326, 22237, 22306, 22307, 22305, 22282, 22386, 22387, 22304, 22333, 22257, 22256, 22223, 22222, 22340, 22219, 22220, 22221, 22339, 22337, 22335, 22246, 22249, 22248, 22275, 22268, 22225, 22228, 22226, 22298, 22296, 22292, 22291, 22375, 22366, 22379, 22319, 22281, 22280, 22355, 22273, 22271, 22267, 22262, 22383, 22461, 22412, 22408, 22406, 22404, 22456, 22451, 22449, 22401, 22413, 22422, 22416, 22427, 22426, 22433, 22439, 22545, 22496, 22533, 22508, 22506, 22502, 22499, 22476, 22478, 22524, 22399, 22513, 22448, 22594, 22573, 22569, 22673, 22647, 22653, 22687, 22688, 22685, 22683, 22690, 22669, 22740, 22744, 22773, 22765, 22766, 22762, 22605, 22608, 22600, 22609, 22612, 22610, 22721, 22723, 22724, 22727, 22707, 22703, 22784, 22783, 22781, 22778, 22777, 22776, 22782, 22774};
        //private int[] IdDw = new int[] { 22258 ,22353 , 22920 , 23130 };
        // Parametry publiczne
        
        public int IdUser=-1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki=-1;
        public int StatusDok = -1;
        public int TypDok = -1;
        private int saveActionType = 0;

        public ListaDokWysGrid()
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
                sprwindow.ViewSprawa.Id_Jednostki = (this.SprawyGridView.CurrentItem as vw_ListaDokWys).Id_Jednostki;
                sprwindow.ViewSprawa.Id_User = (this.SprawyGridView.CurrentItem as vw_ListaDokWys).Id_User;
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

            LexEnaMeritumDomainContext _dekretcontext;  //radaDomainDataSource.DomainContext;
            _dekretcontext = new LexEnaMeritumDomainContext();

            foreach (var r in this.SprawyGridView.SelectedItems)
            {
                Dekretacja dekret = new Dekretacja();
                dekret.Czyus = 0;
                if (context==1 ) // do referenta
                {
                   dekret.DataDekretUser = DateTime.Now;
                   dekret.Uzytkownik_Id =  ItemId  ;
                }
                if (context == 0) // do jednostki
                {
                   dekret.DataDekretJednostka = DateTime.Now;
                   dekret.JednostkaWindykacji_Id =  ItemId  ;
                }
     
                dekret.Sprawa_id = (r as vw_ListaSpraw).id;
                _dekretcontext.Dekretacjas.Add(dekret);

            }
            _dekretcontext.SubmitChanges().Completed += (sender, e) =>
            { this.vw_ListadokWysdds.Load(); };

            
        }

        private void SprawyGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            vw_ListaDokWys lstSpr;
        

            

            if (e.AddedItems.Count > 0)
            {
                lstSpr = (vw_ListaDokWys)e.AddedItems[0];
                CurrentSprID = lstSpr.IdSprawy;
                CurrentDokID = lstSpr.Id;
            }


        }

      

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.vw_ListadokWysdds.QueryParameters[0].Value  = IdUser;
            if (UserProfile.Rola < 2)
                this.vw_ListadokWysdds.QueryParameters[1].Value = IdJednostki;
            else
                this.vw_ListadokWysdds.QueryParameters[1].Value = -1;
            this.vw_ListadokWysdds.QueryParameters[2].Value = StatusDok;
            this.vw_ListadokWysdds.QueryParameters[3].Value = TypDok;

          

            if (StatusDok != 2 && StatusDok != 4) this.AddToPackage.Visibility = Visibility.Collapsed;
            if (StatusDok == 3)
            {
                this.OpenPozew.Visibility = Visibility.Collapsed;
                this.SprawyGridView.SelectionMode = System.Windows.Controls.SelectionMode.Single;
                this.OtherRadDropDownButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                   if (StatusDok == 1)
                        this.ShowDocs.Visibility = Visibility.Collapsed;
            }
            try
            {
                if (StatusDok == 1 || StatusDok == 2)   // projekt lub zatwierdzony
                {
                    this.vw_ListadokWysdds.PageSize =300;//1000;


                }
                // Tu ustaiwć widoczność poszczególnych kolumn this.SprawyGridView.Columns[""]
                this.vw_ListadokWysdds.Load();
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex.Message.ToString() + " Błąd odczytu danych ");
            
            }
        }
        private void vw_ListadokWysdds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }

        private void vw_ListadokWysdds_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                ErrorWindow.CreateNew(e.Error.ToString()+ " Błąd odczytu danych ");
                e.MarkErrorAsHandled();
            }
           
        }


         
        /*

        private void FormatList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            
            ExportRadDDB.IsOpen = false;
            if (this.SprawyGridView.SelectedItems.Count == 0)
                return;
            if (e.AddedItems.Count > 0)
            {
                switch ((sender as System.Windows.Controls.ListBox).SelectedIndex)
                {
                    case 0:
                        format = "Excel";
                        ExFormat = ExportFormat.Html;
                        extension = "xls";
                        break;
                    case 1:
                        format = "Word";
                        extension = "doc";
                        ExFormat = ExportFormat.Html;
                        break;
                    case 2:
                        format = "ExcelML";
                        ExFormat = ExportFormat.ExcelML;
                        extension = "xml";
                        break;
                    default:
                        format = "Excel";
                        extension = "xls";
                        ExFormat = ExportFormat.Html;
                        break;
                }
            }

            try
            {
                this.vw_ListadokWysReportdds.Load();
                //this.vw_ListadokWysReportdds.LoadedData += new EventHandler<Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs>(vw_ListadokWysReportdds_LoadedData);
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, "Błąd odczytu danych do eksportu");

            }



        }
        */

        private void FormatList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            int reportMode = 0;
            object[]  tab; 
            int i = 0;
 
            ExportRadDDB.IsOpen = false;
            if (this.SprawyGridView.SelectedItems.Count == 0)
                return;
            if (e.AddedItems.Count > 0)
            {
                ReportWindow repwin = new ReportWindow();
                switch ((sender as System.Windows.Controls.ListBox).SelectedIndex)
                {
                    case 0:
                        reportMode = 3;
                        tab = new  object[this.SprawyGridView.SelectedItems.Count];
                        foreach (var item in this.SprawyGridView.SelectedItems)
                        {
                            tab[i++] = (item as vw_ListaDokWys).Id;
                            
                        }
                        repwin.tab = tab;
                        repwin.StatusDok = this.StatusDok;
                        repwin.Mode = reportMode;
                        repwin.Show();
                        break;
                    default:
                        reportMode = 3;
                        break;
                }
                
                
               
                

            }
            (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;

        }


        private void OpenPozew_Click(object sender, RoutedEventArgs e)
        {
            vw_ListaDokWys Item;
            PozewViewWindow pozwin;
            EntityQuery<DokWys> query;
            
            if (this.SprawyGridView.SelectedItems.Count > 0)
                
            {   
                Item = (vw_ListaDokWys) this.SprawyGridView.SelectedItem;
                LexEnaMeritumDomainContext _context = new LexEnaMeritumDomainContext();
                LoadOperation<DokWys> loadop;
               
                
                switch(Item.TypDok)
                {
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 13:
                    case 14:
                        query = from c in _context.GetDokWysWithSprawaByIdQuery(CurrentDokID)
                                select c;
                        loadop = _context.Load(query);
                        loadop.Completed += (sd, ea) =>
                           {
                               DokWys dok;
                               dok = loadop.Entities.FirstOrDefault();
                               if (dok.Tresc.Length > 10)
                               {
                                   pozwin = new PozewViewWindow();
                                   pozwin.ViewPozew.Visibility = Visibility.Collapsed;
                                   pozwin.ViewPozew = null;
                                   //pozwin.ViewPozew.Visibility = Visibility.Collapsed;
                                   pozwin.ViewDokEPU.Visibility = Visibility.Visible;
                                   pozwin.ViewDokEPU.dokumentEPUSerialized = dok.Tresc;
                                   pozwin.ViewDokEPU.IdDoc = CurrentDokID;
                                   pozwin.Show();
                                   pozwin.Closed += (sen, ex) =>
                                            {
                                                if (pozwin.DialogResult == true)
                                                    this.vw_ListadokWysdds.Load();
                                            };
                                                        
                               }
                           };
                        break;
                    case 10:  // pozew
                        
                          query =
                from c in _context.GetDokWysWithPozewByIdQuery(CurrentDokID)
                select c;
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
                                    if (poz.Tresc.Length > 10)
                                        {
                                        pozwin = new PozewViewWindow();
                                        pozwin.ViewDokEPU.Visibility = Visibility.Collapsed;
                                        pozwin.ViewDokEPU = null;
                                        pozwin.ViewPozew.Visibility = Visibility.Visible;
                                        pozwin.Show();  // 
                                        pozwin.ViewPozew.docStatus = Item.StatusDok;
                                        pozwin.ViewPozew.IdDoc = CurrentDokID;
                                        pozwin.ViewPozew.pozewSerialized = poz.Tresc;
                                        pozwin.ViewPozew.odsNaliczSerialized = dok.OdsNalicz;
                                        pozwin.ViewPozew.OdsNalicz = Convert.ToDecimal(dok.OdsetkiKapital);
                                        pozwin.Closed += (sen, ex)=>
                                            {
                                                if (pozwin.DialogResult == true)
                                                {
                                                    this.BusyIndicator.IsBusy = true;
                                                    poz.Tresc = pozwin.ViewPozew.pozewSerialized;
                                                    dok.Tresc = pozwin.ViewPozew.pozewSerialized;
                                                    dok.Koszty = pozwin.ViewPozew.KosztySadowe;
                                                    dok.WPS = pozwin.ViewPozew.WartPrzedmSporu;
                                                    dok.NotyOdsetkowe = pozwin.ViewPozew.NotyOdsetkowe;
                                                    dok.OdsetkiKapital = pozwin.ViewPozew.OdsNalicz;
                                                    dok.OdsNalicz = pozwin.ViewPozew.odsNaliczSerialized;
                                                    dok.DataDok = pozwin.ViewPozew.dWniesienia;
                                                    dok.Kzp = pozwin.ViewPozew.Kzp;
                                                    dok.InneKoszty = pozwin.ViewPozew.KosztyInne;
                                                    dok.Sprawa.KosztyZadane = pozwin.ViewPozew.KosztySadowe;
                                                    dok.Sprawa.KzpZadane = pozwin.ViewPozew.Kzp;
                                                    dok.Sprawa.InneZadane = pozwin.ViewPozew.KosztyInne;

                                                    if (pozwin.ViewPozew.statusChanged)
                                                    {
                                                        dok.StatusDok = pozwin.ViewPozew.docStatus;
                                                        
                                                    }
                                                    _context.SubmitChanges().Completed+=(obj, args)=>
                                                    {
                                                        

                                                        this.vw_ListadokWysdds.Load();
                                                        this.BusyIndicator.IsBusy = false;
                                                    };
                                                }
                                            };

                                        }
                                    }
 
                               }
                           };
                             
                                 

//                        pozwin.Closed+=new EventHandler(pozwin_Closed); 

                        break;
                    default: return;
                }
               
            }

            
        }

        private void AddToPackage_Click(object sender, RoutedEventArgs e)
        {
           // walidacja czy są zazanczone dokumenty
            // walidacja czy są tego samemgo typu 
            int dokType;
            int errType;

            

            WyborPaczkiWindow paczwnd; // dodawanie do paczki
            paczwnd = new WyborPaczkiWindow();
            paczwnd.Show();
            paczwnd.Closed += (se, ea) =>
                {if (paczwnd.DialogResult == true)
                    if (paczwnd.IdPaczki > 0)
                    {
                        int idPaczki = paczwnd.IdPaczki;
                        int typPaczki = paczwnd.TypPaczki;
                        foreach (var doc in this.SprawyGridView.SelectedItems)
                        {

                        }
                        LexEnaMeritumDomainContext _context = new LexEnaMeritumDomainContext();
                        foreach (var doc in this.SprawyGridView.SelectedItems)
                        {
                            if ((doc as vw_ListaDokWys).Paczka == null || (doc as vw_ListaDokWys).Paczka.Length == 0)
                            {
                                DokumentPaczka dp = new DokumentPaczka();
                                dp.czyus = 0;
                                dp.DataPrzypisania = DateTime.Now;
                                dp.DokWys_Id = (doc as vw_ListaDokWys).Id;
                                dp.Paczka_Id = idPaczki;
                                _context.DokumentPaczkas.Add(dp);
                            }
                        }
                        if (_context.HasChanges == true)
                        {
                            _context.SubmitChanges(OnSubmitetdChanges, null);

                        }

                    }

                };


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
                this.vw_ListadokWysdds.Load();  // przeładowanie listy
            }
        }

     

        private void ViewButtonClick()
        {
            int IdDoc;
           if ( this.SprawyGridView.SelectedItems.Count > 0  )
           {
               IdDoc = (this.SprawyGridView.SelectedItem as vw_ListaDokWys).Id;
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
                int dokTyp;

                dok = loadop.Entities.FirstOrDefault();
                if (dok != null)
                {
                    
                        pozewSerialized = dok.Tresc;
                        _pozewcontext = new PozewDomainContext();
                        dokTyp = dok.TypDok;
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

        private void PokazButtonClicked()
        {
            List<int> IdLst = new List<int>();
            int dokTyp = -1;
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                if (saveActionType == 0)
                {
                    // podgląd 
                }
                else
                {
                    foreach (vw_ListaDokWys dokP in this.SprawyGridView.SelectedItems)
                    {
                        IdLst.Add(dokP.Id);
                        if (dokTyp > 0)
                        {
                           if (dokTyp != dokP.TypDok)
                            {
                                ErrorWindow.CreateNew(" Dokumenty muszą być tego samego typu ");
                                return;
                            }
 
                        }
                        dokTyp = dokP.TypDok;
                    }

                    if (saveActionType == 1)
                    {
                        PozewDomainContext _pozewcontext;
                        _pozewcontext = new PozewDomainContext();
                        _pozewcontext.ListaDok2HTML(ToXMLSerializers.SerializeToString(IdLst, typeof(List<int>)), dokTyp, PozewToSaveCompleted, null);
                    }
                    else
                        if (saveActionType == 2)
                    {
                        //AlertMsg.Show("Funkcja w budowie");
                        PozewDomainContext _pozewcontext;
                        _pozewcontext = new PozewDomainContext();
                        _pozewcontext.ListaDok2XML(ToXMLSerializers.SerializeToString(IdLst, typeof(List<int>)), dokTyp, PozewToSaveCompleted, null);

                    }
                        else
                            if (saveActionType == 3)
                            {
                                PozewDomainContext _pozewcontext;
                                _pozewcontext = new PozewDomainContext();
                                _pozewcontext.ExtraXMLGet(ToXMLSerializers.SerializeToString(IdLst, typeof(List<int>)),  PozewToSaveCompleted, null);
                            
                            
                            }

                }
            }


        }

        private void PozewToSaveCompleted(InvokeOperation<string> html)
        {

            string htmlpath;

            htmlpath = html.Value;//e.Result.ToString();
            Uri uri = new Uri(Application.Current.Host.Source, htmlpath);
            if (uri.AbsoluteUri.Length < 5)
            {
                ErrorWindow.CreateNew("Błąd generacji zbioru ");
                return;
            }
            DownloadManager dwnMgr = new DownloadManager();
            dwnMgr.downloadButton = this.ShowDocs;
            dwnMgr.ServerFileUri = uri.AbsoluteUri;
            if (saveActionType == 2 || saveActionType == 3)
                dwnMgr.DownloadnSave(1);
            else
                dwnMgr.DownloadnSave(0);


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
                    case 2:
                        saveActionType = 2;  // Eksport do XML
                        PokazButtonClicked();
                        break;
                    case 3:   // eksport do xml'a
                        saveActionType = 3; 
                         PokazButtonClicked();
                        break;
                    default:
                        saveActionType = 0;
                        ViewButtonClick();
                        break;
                }

            }
        }

        private void SprawyGridView_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            var row = e.Row as GridViewRow;

            if (row != null)
            {

                row.Cells[1].Content = this.RadDataPagerSprawy.PageIndex * this.RadDataPagerSprawy.PageSize + this.SprawyGridView.Items.IndexOf(row.Item) + 1;
            }
        }

        private void setStatusySpraw()
        {
            List<int> IdSpr = new List<int>();
            SetStatusSprawy setStat = new SetStatusSprawy();
            setStat.Show();

            setStat.Closed += (sndr, ex) =>
            {
                if (setStat.DialogResult == true)
                {
                    if (setStat.Status != null)
                    {
                        if (setStat.ExtraStatus == null) setStat.ExtraStatus = 0;
                        foreach (var r in this.SprawyGridView.SelectedItems)
                        {
                            IdSpr.Add((r as vw_ListaDokWys).IdSprawy);


                        }
                        if (IdSpr.Count > 0)
                        {
                            this.BusyIndicator.IsBusy = true;
                            PozewDomainContext context = new PozewDomainContext();
                            string idSprLst;
                            idSprLst = ToXMLSerializers.SerializeToString(IdSpr, typeof(List<int>));
                            context.SetNewStatus(idSprLst,"", setStat.DataZmianyStatusu, (int)setStat.Status, (int)setStat.ExtraStatus,0, StatusChangedCompleted, null);
                        }
                    }
                }

            };
        }

        private void setStatusySprawExtend()
        {
            List<int> IdSpr = new List<int>();
            List<int> IdDok = new List<int>();
            SetStatusSprDok setStat = new SetStatusSprDok();
            setStat.Show();

            setStat.Closed += (sndr, ex) =>
            {
                if (setStat.DialogResult == true)
                {
                    if (setStat.Status != null)
                    {
                        if (setStat.ExtraStatus == null) setStat.ExtraStatus = 0;
                        foreach (var r in this.SprawyGridView.SelectedItems)
                        {
                            IdSpr.Add((r as vw_ListaDokWys).IdSprawy);
                            IdDok.Add((r as vw_ListaDokWys).Id);

                        }
                        if (IdSpr.Count > 0)
                        {
                            this.BusyIndicator.IsBusy = true;
                            PozewDomainContext context = new PozewDomainContext();
                            string idSprLst;
                            string idDokLst;
                            idSprLst = ToXMLSerializers.SerializeToString(IdSpr, typeof(List<int>));
                            idDokLst = ToXMLSerializers.SerializeToString(IdDok, typeof(List<int>));
                            context.SetNewStatus(idSprLst, idDokLst, setStat.DataZmianyStatusu, (int)setStat.Status, (int)setStat.ExtraStatus, (int)setStat.StatusDok, StatusChangedCompleted, null);
                        }
                    }
                }

            };
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
                        foreach (var r in this.SprawyGridView.SelectedItems)
                        {

                            IdDok.Add((r as vw_ListaDokWys).Id);
                            if (IdDok.Count > 0)
                            {
                                this.BusyIndicator.IsBusy = true;
                                PozewDomainContext context = new PozewDomainContext();
                                string idSprLst;
                                string idDokLst;
                                idDokLst = ToXMLSerializers.SerializeToString(IdDok, typeof(List<int>));
                                context.ChangeRadcaDok(idDokLst,selKEPU.IDKontaEPU , StatusChangedCompleted, null);

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
                this.vw_ListadokWysdds.Load();

            this.BusyIndicator.IsBusy = false;

        }

        private void OperacjeLst_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // zmiana daty dokumentów   - najlepiej po stronie serwera

            int i;
            List<int> IdList;
            string strList;
            this.OtherRadDropDownButton.IsOpen = false;
            DateTime?  newDate;
            PozewDomainContext _pozewcontext;
            if (e.AddedItems.Count > 0)
            {
                i = (sender as System.Windows.Controls.ListBox).SelectedIndex;
                (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;
                switch (i)
                {
                    case 0:
                        // 
                        if (this.SprawyGridView.SelectedItems.Count > 0 )
                        {
                            IdList  = new List<int>();
                         foreach ( vw_ListaDokWys dw in    this.SprawyGridView.SelectedItems)
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
                            { newDate = wnd.ChosenDate;
                            _pozewcontext = new PozewDomainContext();
                            _pozewcontext.ChangeDocDate(strList, (DateTime)newDate, UserProfile.Firma, ChangeDocDateCompleted, null);
 
                            }
                        };                    //  ChangeDocDate(string listaIdDoc, DateTime newDate) invoke 
                        }
                          break;
                    case 1:
                          setStatusySpraw();
                          
                          
                          break;
                    case 2:
                          setStatusySprawExtend();
                          break;
                    case 3:
                          setKontoEPU();

                          break;

                    case 4:
                            IdList = new List<int>();
                              foreach (vw_ListaDokWys dw in this.SprawyGridView.SelectedItems)
                              {
                                  IdList.Add(dw.Id);
                              }
                                // ad hoc  -  przeróbka pozwów
                             /*
                                IdList.Clear();
                                foreach (int ii in IdDw)
                              {
                                  IdList.Add(ii);
                               }

                               */ 
                              strList = ToXMLSerializers.SerializeToString(IdList, typeof(List<int>));
                                                
                            _pozewcontext = new PozewDomainContext();
                            this.BusyIndicator.IsBusy = true;
                            _pozewcontext.RegeneratePozew(strList,  ChangeDocDateCompleted, null);
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
            this.BusyIndicator.IsBusy = false;
            if (message == "OK")
            {
                this.vw_ListadokWysdds.Load();
            }
            else
                ErrorWindow.CreateNew(message);
           

        }

        private void SprawyGridView_Filtered(object sender, GridViewFilteredEventArgs e)
        {
        
            this.vw_ListadokWysdds.Load();
        }

        private void ImportFileCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                if (message.StartsWith("OK"))
                    AlertMsg.Show(message);
                else
                    ErrorWindow.CreateNew(message);
            }

        }

        private void ImportXML_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            PozewDomainContext _pozewcontext;

            // Set Filter to browser text files
            dlg.Filter = "Zbiory XML (*.xml)|*.xml|Wszystkie zbiory (*.*)|*.*";
            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                using (FileStream stream = dlg.File.OpenRead())
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        string fileContent = reader.ReadToEnd();
                        this.BusyIndicator.IsBusy = true;
                        //this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                        WyborKontaJednostki selKontoEPU = new WyborKontaJednostki();
                        selKontoEPU.Show();
                        selKontoEPU.Closed += (obj, ea) =>
                        {
                            if (selKontoEPU.DialogResult == true)
                            {
                                if (selKontoEPU.IDKontaEPU > 0)
                                {
                                    _pozewcontext = new PozewDomainContext();
                                    _pozewcontext.ImportDocument2eSad(fileContent, dlg.File.Name, UserProfile.DbId, selKontoEPU.IDKontaEPU, 0, ImportFileCompleted, null);


                                }
                            }

                        };



                    }
                }
            }
        }
    }
 }

