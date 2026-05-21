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
    public partial class UzDNoweOperacje : UserControl
    {
        private int CurrentSprID;
        private int CurrentDokID;
        //private int[] IdDw = new int[] { 2556, 17004, 19496, 22310, 22155, 22311, 21856, 21781, 21769, 21773, 21779, 21774, 21860, 21867, 21861, 21977, 21987, 21981, 21979, 21973, 21996, 21990, 22049, 21941, 22007, 22001, 21882, 21886, 21879, 21877, 21873, 21871, 21883, 21966, 21967, 21984, 22027, 22025, 21995, 21982, 21997, 22308, 21991, 21972, 21986, 21970, 21978, 21975, 21998, 21974, 21968, 21869, 21870, 21887, 21888, 21890, 22053, 21950, 21962, 21960, 21952, 21961, 21951, 22064, 21946, 22019, 21944, 22061, 22017, 21945, 22003, 22002, 22008, 22011, 22005, 21918, 21893, 21913, 21897, 21914, 21899, 21909, 21900, 21902, 21896, 21916, 21904, 21911, 21892, 22062, 22066, 22067, 22063, 22068, 22016, 22010, 22014, 22050, 22046, 22029, 22018, 22031, 22032, 22020, 22041, 22042, 22037, 22035, 22043, 22038, 22039, 22036, 21957, 21959, 21964, 21965, 21955, 21958, 21956, 21954, 22059, 21953, 21881, 22315, 21894, 21903, 21905, 21912, 21922, 21930, 21940, 21891, 21895, 21910, 21920, 21921, 21925, 21929, 21932, 21889, 21927, 21938, 21935, 21937, 21939, 22316, 21934, 22491, 22144, 22077, 22090, 22092, 22085, 22084, 22082, 22079, 22080, 22081, 22086, 22091, 22089, 22088, 22078, 22122, 22124, 22118, 22209, 22099, 22098, 22141, 22138, 22097, 22100, 22211, 22168, 22205, 22203, 22202, 22200, 22095, 22102, 22094, 22096, 22101, 22207, 22173, 22172, 22171, 22210, 22214, 22104, 22106, 22112, 22107, 22163, 22167, 22165, 22166, 22119, 22121, 22123, 22126, 22152, 22149, 22147, 22146, 22145, 22206, 22199, 22198, 22197, 22196, 22191, 22189, 22180, 22194, 22190, 22178, 22182, 22195, 22177, 22183, 22193, 22176, 22186, 22159, 22157, 22161, 22244, 22230, 22327, 22234, 22325, 22323, 22232, 22331, 22322, 22324, 22329, 22321, 22326, 22237, 22306, 22307, 22305, 22282, 22386, 22387, 22304, 22333, 22257, 22256, 22223, 22222, 22340, 22219, 22220, 22221, 22339, 22337, 22335, 22246, 22249, 22248, 22275, 22268, 22225, 22228, 22226, 22298, 22296, 22292, 22291, 22375, 22366, 22379, 22319, 22281, 22280, 22355, 22273, 22271, 22267, 22262, 22383, 22461, 22412, 22408, 22406, 22404, 22456, 22451, 22449, 22401, 22413, 22422, 22416, 22427, 22426, 22433, 22439, 22545, 22496, 22533, 22508, 22506, 22502, 22499, 22476, 22478, 22524, 22399, 22513, 22448, 22594, 22573, 22569, 22673, 22647, 22653, 22687, 22688, 22685, 22683, 22690, 22669, 22740, 22744, 22773, 22765, 22766, 22762, 22605, 22608, 22600, 22609, 22612, 22610, 22721, 22723, 22724, 22727, 22707, 22703, 22784, 22783, 22781, 22778, 22777, 22776, 22782, 22774};
        //private int[] IdDw = new int[] { 22258 ,22353 , 22920 , 23130 };
        // Parametry publiczne

        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int TypDok = -1;


        public UzDNoweOperacje()
        {
            CurrentSprID = 0;
            CurrentDokID = 0;
            InitializeComponent();
        }







        private void SprawyGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {



            UZDvw_Pakiet lst;


            if (e.AddedItems.Count > 0)
            {
                lst = (UZDvw_Pakiet)e.AddedItems[0];
                CurrentSprID = lst.UZD_PakietId;
                UZD_PakietDetailsdds.QueryParameters[0].Value = lst.UZD_PakietId;
                UZD_PakietDetailsdds.Load();
            }


        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            //   if (UserProfile.Rola < 2)
            //       this.vw_ListImportsdds.QueryParameters[0].Value = IdJednostki;
            //    else
            //        this.vw_ListImportsdds.QueryParameters[0].Value = 0;




            try
            {
                this.GetUZDvw_Pakietdds.PageSize = 300;//1000;



                // Tu ustaiwć widoczność poszczególnych kolumn this.SprawyGridView.Columns[""]
                this.GetUZDvw_Pakietdds.Load();
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex.Message.ToString() + " Błąd odczytu danych ");

            }
        }










        private void ImportLargeFileCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }
            else
                AlertMsg.Show("Zbiór został poprawnie wczytany. System rozpoczął przetwarzanie, które potrwa kilka godzin. W tym czasie nie wykonuj modyfikacji danych dot. KRD");

            this.GetUZDvw_Pakietdds.Load();




        }

        private void ImportFileCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }
           
            this.GetUZDvw_Pakietdds.Load();




        }


        private void ImportFileXLSXCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }
            this.GetUZDvw_Pakietdds.Load();




        }

        private void ImportFile(int mode)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            PozewDomainContext _pozewcontext;
            byte[] theFile;

            // Set Filter to browser text files
            dlg.Filter = "Zbiory csv (*.csv)|*.csv|Wszystkie zbiory (*.*)|*.*";
            bool? result = dlg.ShowDialog();
            try
            {
                if (result.HasValue && result.Value)
                {



                    using (FileStream stream = dlg.File.OpenRead())
                    {


                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            theFile = ms.ToArray();


                            this.BusyIndicator.IsBusy = true;
                            //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                            _pozewcontext = new PozewDomainContext();
                            _pozewcontext.ImportDocument(Convert.ToBase64String(theFile), dlg.File.Name, UserProfile.DbId, UserProfile.IdJednostki, mode, ImportFileCompleted, null);

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, "Bład odczytu zbioru importu");

            }

        }

      

        private void GetUZDvw_Pakiet_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }



        private void UZD_PakietDetailsdds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }




        private void Import_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {


            if (this.SprawyGridView.SelectedItems.Count > 0)
            {

                UZDvw_Pakiet pak = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;


                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = false;
                PozewDomainContext _pozewcontext;
                byte[] theWorksheet;
                // Set Filter to browser text files
                dlg.Filter = "Zbiory XLSX (*.xlsx)|*.xlsx|Zbiory xls (*.xls)|*.xls|Wszystkie zbiory (*.*)|*.*";


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
                                theWorksheet = ms.ToArray();


                                this.BusyIndicator.IsBusy = true;
                                //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                                _pozewcontext = new PozewDomainContext();
                                _pozewcontext.ImportDocumentXLSX(Convert.ToBase64String(theWorksheet), dlg.File.Name, -1, pak.UZD_PakietId, ImportFileXLSXCompleted, null);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        this.BusyIndicator.IsBusy = false;
                        ErrorWindow.CreateNew(ex, "Bład odczytu zbioru");
                    }
                }
            }
            else
                AlertMsg.Show("Wybierz pakiet, dla którego wczytujesz potwierdzenia");
        }

        private void btExport_Click(object sender, RoutedEventArgs e)
        {

            string extension = "xls";
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} zbiory (*.{0})|*.{0}|Wszystkie zbiory (*.*)|*.*", extension, "Excel"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    ImportDetailsGridView.Export(stream,
                     new GridViewExportOptions()
                     {
                         Format = ExportFormat.ExcelML,
                         ShowColumnHeaders = true,
                         ShowColumnFooters = true,
                         ShowGroupFooters = false,
                     });
                }

            }
        }

       

      

        private void btCheckKrd_Click(object sender, RoutedEventArgs e)
        {
            PozewDomainContext _pozewcontext;
            _pozewcontext = new PozewDomainContext();
            this.BusyIndicator.IsBusy = true;
            _pozewcontext.BIG_CheckStatus(0, checkCompleted, null);
        }
        private void checkCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;

            if (message.Length > 0)
            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(message);
            }
            this.GetUZDvw_Pakietdds.Load();
            this.BusyIndicator.IsBusy = false;




        }


        private void btDelImport_Click(object sender, RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                UZDvw_Pakiet bi = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;
                if (bi.Status >= 10)
                {
                    AlertMsg.Show("Nie można usunąć pakietu, dla którego złożono deklaracje");
                    return;
                }
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.OkButtonContent = "Tak";
                dlgparm.Content = "Czy na pewno chcesz usunąć wybrany pakiet ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confdeluwndClose;
                RadWindow.Confirm(dlgparm);
            }
        }

        private void confdeluwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {

                UZDvw_Pakiet bi = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;
                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                _pozewcontext.UZD_DelPakiet(bi.UZD_PakietId, delPakietCompleted, null);

            }
        }

        private void delPakietCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;

            if (message.Length > 0)

            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(message);

            }

            this.GetUZDvw_Pakietdds.Loaded += GetBIG_ImportByDatedds_Loaded;
            this.GetUZDvw_Pakietdds.Load();
            this.BusyIndicator.IsBusy = false;



        }

        private void GetBIG_ImportByDatedds_Loaded(object sender, RoutedEventArgs e)
        {
            this.BusyIndicator.IsBusy = false;
            if (this.SprawyGridView.SelectedItem == null)
            {
                if (this.SprawyGridView.Items.Count == 0)
                {
                    UZD_PakietDetailsdds.QueryParameters[0].Value = 0;
                    UZD_PakietDetailsdds.Load();
                }
                else
                {
                    (this.SprawyGridView.Items).MoveCurrentToFirst();

                }
            }
            //throw new NotImplementedException();
        }

        private void ListaAkcji_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void Eksport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rdbImport.IsOpen = false;
            ReportWindow repwin = new ReportWindow();

            if ( this.SprawyGridView.SelectedItems.Count > 0 )
            {

                UZDvw_Pakiet pak = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;

                   
                    repwin.Mode = 9001;
                    repwin.IdPaczki = pak.UZD_PakietId;
                    repwin.Show();
                
            }
        }

        private void  WeryfikacjaNIP_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rdbImport.IsOpen = false;

            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                UZDvw_Pakiet bi = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;
                if (bi.Status >= 10)
                {
                    AlertMsg.Show("Nie można potwierdzać pakietu, dla którego złożono deklaracje");
                    return;
                }
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.OkButtonContent = "Tak";
                dlgparm.Content = "Czy na pewno chcesz weryfikować NIP dla wybranego pakietu ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confcheckNipsClose;
                RadWindow.Confirm(dlgparm);
            }
        }

        private void confcheckNipsClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {

                UZDvw_Pakiet bi = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;
                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                _pozewcontext.UZD_CheckNips(bi.UZD_PakietId, checkNIPPakietCompleted, null);

            }
        }

        private void checkNIPPakietCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;

            if (message.Length > 0)

            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(message);

            }
            else
            {
                this.BusyIndicator.IsBusy = false;
                AlertMsg.Show("Proces walidacji został zainicjowany. Po jego zakończeniu status pakietu ulegnie zmianie");
            }



        }

        private void Typowanie_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)

        {

            rdbImport.IsOpen = false;

            DateTime newDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 25);
            GetDateWindow wnd = new GetDateWindow();
            wnd.ChosenDate = newDate;
            wnd.Title = "Podaj datę planowanego złożenia deklaracji";
            wnd.Show();
            wnd.Closed += (se, ea) =>
            {
                if (wnd.DialogResult == true)
                {
                    newDate = wnd.ChosenDate.Value;

                    PozewDomainContext _pozewcontext = new PozewDomainContext();
                    this.BusyIndicator.IsBusy = true;
                    _pozewcontext.UZD_ImportPackage(newDate, UserProfile.UserName, ImportFileCompleted, null);
                                      

                }
            };




        }

        List<byte[]> zipResult;
        Guid currGuid;

        private void updatePart(LexEnaMeritumDomainContext _dbcontext, Guid guId, string fileName, int i)
        {

            

            DataBuffer dbf = new DataBuffer();
            dbf.ident = guId;
            dbf.binValue = zipResult[i];
            dbf.number = i;

            _dbcontext.DataBuffers.Add(dbf);
            _dbcontext.SubmitChanges().Completed += (ob, eva) =>
            {
                i++;
                if (i < zipResult.Count)
                    updatePart(_dbcontext, guId,fileName, i);
                else
                {
                    PozewDomainContext _pozewcontext;
                    _pozewcontext = new PozewDomainContext();
                    _pozewcontext.ImportLargeDocument( guId.ToString(),fileName,this.IdUser, 4,UserProfile.Firma, ImportLargeFileCompleted, null);

                }
            };


        }

        private void VATZD_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // generajca zestawienia dla VAT_ZD - tylko pozycje potwierdzone i ze statusem 1.
            rdbImport.IsOpen = false;
            ReportWindow repwin = new ReportWindow();

            if (this.SprawyGridView.SelectedItems.Count > 0)
            {

                UZDvw_Pakiet pak = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;


                repwin.Mode = 7002;
                repwin.IdPaczki = pak.UZD_PakietId;
                repwin.Show();

            }
        }

        private void VATZDConfirm_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rdbImport.IsOpen = false;
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {

                UZDvw_Pakiet pak = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;


                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = false;
                PozewDomainContext _pozewcontext;
                byte[] theWorksheet;
                // Set Filter to browser text files
                dlg.Filter = "Zbiory XLSX (*.xlsx)|*.xlsx|Zbiory xls (*.xls)|*.xls|Wszystkie zbiory (*.*)|*.*";


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
                                theWorksheet = ms.ToArray();


                                this.BusyIndicator.IsBusy = true;
                                //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                                _pozewcontext = new PozewDomainContext();
                                _pozewcontext.ImportDocumentXLSX(Convert.ToBase64String(theWorksheet), dlg.File.Name, -3, pak.UZD_PakietId, ImportFileXLSXCompleted, null);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        this.BusyIndicator.IsBusy = false;
                        ErrorWindow.CreateNew(ex, "Bład odczytu zbioru");
                    }
                }
            }
            else
                AlertMsg.Show("Wybierz pakiet, dla którego wczytujesz ostateczne potwierdzenia");
        }

        private void ListaAkcjiSAP_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void EksportPartner_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rdbImport.IsOpen = false;
            ReportWindow repwin = new ReportWindow();

            if (this.SprawyGridView.SelectedItems.Count > 0)
            {

                UZDvw_Pakiet pak = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;


                repwin.Mode = 9010;
                repwin.IdPaczki = pak.UZD_PakietId;
                repwin.Show();

            }
        }

        private void ImportPartner_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rdbImport.IsOpen = false;
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {

                UZDvw_Pakiet pak = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;


                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = false;
                PozewDomainContext _pozewcontext;
                byte[] theWorksheet;
                // Set Filter to browser text files
                dlg.Filter = "Zbiory XLSX (*.xlsx)|*.xlsx|Zbiory xls (*.xls)|*.xls|Wszystkie zbiory (*.*)|*.*";


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
                                theWorksheet = ms.ToArray();


                                this.BusyIndicator.IsBusy = true;
                                //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                                _pozewcontext = new PozewDomainContext();
                                _pozewcontext.ImportDocumentXLSX(Convert.ToBase64String(theWorksheet), dlg.File.Name, -2, pak.UZD_PakietId, ImportFileXLSXCompleted, null);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        this.BusyIndicator.IsBusy = false;
                        ErrorWindow.CreateNew(ex, "Bład odczytu zbioru");
                    }
                }
            }
            else
                AlertMsg.Show("Wybierz pakiet, dla którego wczytujesz dane");
        }

        private void ZestawKorekSAP_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rdbImport.IsOpen = false;
            ReportWindow repwin = new ReportWindow();

            if (this.SprawyGridView.SelectedItems.Count > 0)
            {

                UZDvw_Pakiet pak = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;


                repwin.Mode = 6000;
                repwin.IdPaczki = pak.UZD_PakietId;
                repwin.Show();

            }
            else
                AlertMsg.Show("Wybierz pakiet dla którego eksportujesz dane");
        }

        private void ListaAkcjiInne_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ;
        }

        private void Recalculate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rdbImport.IsOpen = false;
          

            if (this.SprawyGridView.SelectedItems.Count > 0)
            {

                UZDvw_Pakiet pak = (UZDvw_Pakiet)this.SprawyGridView.SelectedItem;

                if (pak.Status >= 10)
                {
                    AlertMsg.Show("Nie można przeliczać zaległości dla tego pakietu");
                    return;
                }


                PozewDomainContext _pozewcontext;
                this.BusyIndicator.IsBusy = true;
                _pozewcontext = new PozewDomainContext();
                  _pozewcontext.UZD_ComputeZaleglosc( pak.UZD_PakietId,ImportFileCompleted , null);



              }
            else
                AlertMsg.Show("Wybierz pakiet dla którego chcesz przeliczyć zaległość");
        }

        /*
        private void Import_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            rdbImport.IsOpen = false;
            OpenFileDialog dlg = new OpenFileDialog();
            int partSize = 3000000;
            List<string> zipSplit = new List<string>();
            zipResult = new List<byte[]>();
            dlg.Multiselect = false;
            byte[] thezip;
            // Set Filter to browser text files
            dlg.Filter = "Zbiory csv (*.csv)|*.csv|Wszystkie zbiory (*.*)|*.*";
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
                            thezip = ms.ToArray();
                            for (int i = 0; i < thezip.Length; i += partSize)
                            {
                                int bufSize = thezip.Length - i >= partSize ? partSize : thezip.Length - i;
                                byte[] buffer = new byte[bufSize];
                                Buffer.BlockCopy(thezip, i, buffer, 0, bufSize);
                                zipResult.Add(buffer);
                            }

                        }
                        if (zipResult.Any())
                        {
                            LexEnaMeritumDomainContext _dbcontext = new LexEnaMeritumDomainContext();
                            currGuid = Guid.NewGuid();
                            this.BusyIndicator.IsBusy = true;
                            updatePart(_dbcontext, currGuid,dlg.File.Name ,0);


                        }




                    }


                }
                catch (Exception ex)
                {
                    this.BusyIndicator.IsBusy = false;
                    ErrorWindow.CreateNew(ex, "Bład odczytu zbioru skompresowanego");
                }

            }
        }

    */
    }
    
}
   
