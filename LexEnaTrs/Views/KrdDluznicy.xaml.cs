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

namespace LexEnaTrs.Views
{
    public partial class KrdDluznicy : UserControl
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
        private bool isFull = false;
     
        public KrdDluznicy()
        {
            CurrentSprID = 0;
            CurrentDokID = 0;
            isFull = false;
            InitializeComponent();
           
        }







        private void SprawyGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {



           BIGvw_Dluznicy lst;


            if (e.AddedItems.Count > 0)
            {
                if (isFull)
                {
                    CurrentSprID = ((BIGvw_Dluznicy)e.AddedItems[0]).BIG_CaseId;
                    BIGvw_ImportyDetaildds.QueryParameters[0].Value = CurrentSprID;
                    BIGvw_ImportyDetaildds.Load();
                }
                else
                {
                    CurrentSprID = ((BIGvw_DluznicyAktual)e.AddedItems[0]).BIG_CaseId;
                    BIGvw_ImportyDetailAktualdds.QueryParameters[0].Value = CurrentSprID;
                    BIGvw_ImportyDetailAktualdds.Load();

                }
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
                this.BIGvw_Dluznicydds.PageSize = 100;//1000;
                this.BIGvw_DluznicyAktualdds.PageSize = 100;


                // Tu ustaiwć widoczność poszczególnych kolumn this.SprawyGridView.Columns[""]
                this.BIGvw_DluznicyAktualdds.Load();
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex.Message.ToString() + " Błąd odczytu danych ");

            }
        }






        private void SprawyGridView_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            var row = e.Row as GridViewRow;
            /*
                        if (row != null)
                        {

                            row.Cells[1].Content = this.RadDataPagerSprawy.PageIndex * this.RadDataPagerSprawy.PageSize + this.SprawyGridView.Items.IndexOf(row.Item) + 1;
                        }

                */
        }



        private void ImportFileCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }
            this.BIGvw_Dluznicydds.Load();




        }



        private void ImportFileXLSXCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }
            this.BIGvw_Dluznicydds.Load();




        }

        private void ImportFile_Click(object sender, RoutedEventArgs e)
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
                            _pozewcontext.ImportDocument(Convert.ToBase64String(theFile), dlg.File.Name, UserProfile.DbId, UserProfile.IdJednostki, 1, ImportFileCompleted, null);
                            
                        }
                        
                      
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, "Bład odczytu zbioru importu");

            }

        }

        private void OperacjeLst_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void GetBIG_ImportByDate_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                ErrorWindow.CreateNew(e.Error.ToString() + " Błąd odczytu danych ");
                e.MarkErrorAsHandled();
            }
         
        }

        private void GetBIG_ImportByDate_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }



        private void SearchItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void DetailsItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void BIGvw_ImportyDetaildds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {

        }

        private void ImportDetailsGridView_RowLoaded(object sender, RowLoadedEventArgs e)
        {

        }

        private void rbImportXLS_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            PozewDomainContext _pozewcontext;
            byte[] theWorksheet;
            // Set Filter to browser text files
            dlg.Filter = "Zbiory XLSX (*.xlsx)|*.xlsx|Wszystkie zbiory (*.*)|*.*";
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
                            _pozewcontext.ImportDocumentXLSX(Convert.ToBase64String(theWorksheet), dlg.File.Name, UserProfile.DbId, UserProfile.IdJednostki, ImportFileXLSXCompleted, null);
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

        private void SendToKrd_Click(object sender, RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.OkButtonContent = "Tak";
                dlgparm.Content = "Czy na pewno chcesz wysłać wskazany zestaw do KRD ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confdluwndClose;
                RadWindow.Confirm(dlgparm);
            }
        }

        private void confdluwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {

                BIG_Import bi = (BIG_Import)this.SprawyGridView.SelectedItem;
                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                _pozewcontext.BIG_ExportPakiet(bi.BIG_ImportId,UserProfile.DbId, exportCompleted, null);

            }
        }

        private void exportCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }
            this.BIGvw_Dluznicydds.Load();




        }


        private void btCheckKrd_Click(object sender, RoutedEventArgs e)
        {
            PozewDomainContext _pozewcontext;
            _pozewcontext = new PozewDomainContext();
            this.BusyIndicator.IsBusy = true;
            _pozewcontext.BIG_CheckStatus(0,checkCompleted,null);
        }

        private void checkCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
           
            if (message.Length > 0)
            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(message);
            }
             this.BIGvw_Dluznicydds.Load();
            this.BusyIndicator.IsBusy = false;




        }

        private void btImpWiena_Click(object sender, RoutedEventArgs e)
        {
            //importuj z systemu Wiena
            PozewDomainContext _pozewcontext;
            _pozewcontext = new PozewDomainContext();
            this.BusyIndicator.IsBusy = true;
            _pozewcontext.BIG_ImportWiena(UserProfile.DbId,UserProfile.Firma, (UserProfile.Firma == -1 ? 11 : 0),impWienaCompleted, null);
        }


        private void impWienaCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;

            if (message.Length > 0)
            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(message);
            }
            this.BIGvw_Dluznicydds.Load();
            this.BusyIndicator.IsBusy = false;




        }

        private void btDelImport_Click(object sender, RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.OkButtonContent = "Tak";
                dlgparm.Content = "Czy na pewno chcesz usunąć wybraną sprawę dłużnika ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confdeluwndClose;
                RadWindow.Confirm(dlgparm);
            }
        }

        private void confdeluwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {
                int idDluz = 0;
                if (isFull)
                 idDluz = ((BIGvw_Dluznicy)this.SprawyGridView.SelectedItem).BIG_CaseId;
                else
                    idDluz = ((BIGvw_DluznicyAktual)this.SprawyGridView.SelectedItem).BIG_CaseId;

                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                _pozewcontext.BIG_DelCase(idDluz, UserProfile.DbId ,delDluCompleted, null);

            }
        }

        private void delDluCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }
            this.BIGvw_Dluznicydds.Load();




        }

        private void radRadioAktual_Checked(object sender, RoutedEventArgs e)
        {

            if (isFull)
            {
                btDeleteOblig.Visibility = Visibility.Visible;
                btUpdate.Visibility = Visibility.Visible;
                btSuspend.Visibility = Visibility.Visible;
                btResume.Visibility = Visibility.Visible;
                btDeleteOblig.Visibility = Visibility.Visible;
                btUpdateDebtor.Visibility = Visibility.Visible;
                btSuspendDebtor.Visibility = Visibility.Visible;
                btResumeDebtor.Visibility = Visibility.Visible;
                btDelImport.Visibility = Visibility.Visible;

                SprawyGridView.ItemsSource = this.BIGvw_DluznicyAktualdds.DataView;
                RadDataPagerSprawy.Source = this.BIGvw_DluznicyAktualdds.DataView;
                ImportDetailsGridView.ItemsSource = this.BIGvw_ImportyDetailAktualdds.DataView;
                RadDataPagerImportDetails.Source = this.BIGvw_ImportyDetailAktualdds.DataView;
                this.BIGvw_DluznicyAktualdds.Load();
                
            }
            isFull = false;
        }

        private void radRadioPelne_Checked(object sender, RoutedEventArgs e)
        {

            if (!isFull)
            {
                btDeleteOblig.Visibility = Visibility.Collapsed;
                btUpdate.Visibility = Visibility.Collapsed; 
                btSuspend.Visibility = Visibility.Collapsed;
                btResume.Visibility = Visibility.Collapsed;
                btDeleteOblig.Visibility = Visibility.Collapsed;
                btUpdateDebtor.Visibility = Visibility.Collapsed;
                btSuspendDebtor.Visibility = Visibility.Collapsed;
                btResumeDebtor.Visibility = Visibility.Collapsed;
                btDelImport.Visibility = Visibility.Collapsed;

                SprawyGridView.ItemsSource = this.BIGvw_Dluznicydds.DataView;
                RadDataPagerSprawy.Source = this.BIGvw_Dluznicydds.DataView;
                ImportDetailsGridView.ItemsSource = this.BIGvw_ImportyDetaildds.DataView;
                RadDataPagerImportDetails.Source = this.BIGvw_ImportyDetaildds.DataView;
                this.BIGvw_Dluznicydds.Load();
            }
            isFull = true;
        }

      

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            
            BIGvw_ObligationLastStatus r1 = null;
            string rowSerialized;

            if (this.ImportDetailsGridView.SelectedItems.Count > 0)
            {
               
                    r1 = ((BIGvw_ObligationLastStatus)this.ImportDetailsGridView.SelectedItem);
                rowSerialized = ToXMLSerializers.SerializeEntity(r1, typeof(BIGvw_ObligationLastStatus));
                Decimal newSaldo;
                DateTime dataWymag;
                DateTime dataWezw;
                string nrfaktury; 
              
                    GetMoneyWindow wnd = new GetMoneyWindow();
                    wnd.ChosenSaldo = r1.Saldo;
                    wnd.NrFaktury = r1.Title;
                    wnd.DataWymag = r1.DataWymag;
                    wnd.DataWezw = r1.DataWysWezw;
                    wnd.Show();
                    wnd.Closed += (se, ea) =>
                    {
                    if (wnd.DialogResult == true)
                    {
                            newSaldo = wnd.ChosenSaldo.Value;
                            dataWezw = wnd.DataWezw.Value;
                            dataWymag = wnd.DataWymag.Value;
                            nrfaktury = wnd.NrFaktury;

                            PozewDomainContext _pozewcontext;
                            _pozewcontext = new PozewDomainContext();
                            this.BusyIndicator.IsBusy = true;
                            _pozewcontext.BIG_UpdateDetail(rowSerialized, newSaldo,dataWymag,dataWezw,nrfaktury,UserProfile.DbId, updateObligCompleted, null);

                        }
                };



               
            }
            else
                AlertMsg.Show("Musisz wybrać właściwy wiersz z listy");
        }

         private void updateObligCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }
            this.BIGvw_ImportyDetailAktualdds.Load();




        }

        private void btSuspend_Click(object sender, RoutedEventArgs e)
        {

            BIGvw_ObligationLastStatus r1 = null;
            string rowSerialized;

            if (this.ImportDetailsGridView.SelectedItems.Count > 0)
            {

                r1 = ((BIGvw_ObligationLastStatus)this.ImportDetailsGridView.SelectedItem);
                rowSerialized = ToXMLSerializers.SerializeEntity(r1, typeof(BIGvw_ObligationLastStatus));
                DateTime newDate = DateTime.Today.AddDays(14);
                GetDateWindow wnd = new GetDateWindow();
                wnd.ChosenDate = newDate;
                wnd.Title = "Podaj datę końca okresu zawieszenia";
                wnd.Show();
                wnd.Closed += (se, ea) =>
                {
                    if (wnd.DialogResult == true)
                    {
                        newDate = wnd.ChosenDate.Value;
                        PozewDomainContext _pozewcontext;
                        _pozewcontext = new PozewDomainContext();
                        this.BusyIndicator.IsBusy = true;
                        _pozewcontext.BIG_SuspendObligation(rowSerialized, newDate, UserProfile.DbId, suspendObligCompleted, null);

                    }
                };




            }
            else
                AlertMsg.Show("Musisz wybrać właściwy wiersz z listy");
        }

        private void suspendObligCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }
            this.BIGvw_ImportyDetailAktualdds.Load();
            
        }



        private void buResume_Click(object sender, RoutedEventArgs e)
        {
            AlertMsg.Show("Funkcja nie jest obsługiwana poprawnie przez KRD");
        }

        private void btDeleteOblig_Click(object sender, RoutedEventArgs e)
        {
            if (this.ImportDetailsGridView.SelectedItems.Count > 0)
            {
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.OkButtonContent = "Tak";
                if (this.ImportDetailsGridView.Items.Count == 1)
                    dlgparm.Content = "Usunięcie ostatniego zobowiązania spowoduje usunięcie całej sprawy.  Czy na pewno chcesz usunąć wybrane zobowiązanie ?";
                else
                    dlgparm.Content = "Czy na pewno chcesz usunąć wybrane zobowiązanie ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confDelObligClose;
                RadWindow.Confirm(dlgparm);
            }
        }

        private void confDelObligClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {
                int idOblig = 0;
                if (isFull)
                    idOblig = ((BIGvw_ImportyDetail)this.ImportDetailsGridView.SelectedItem).BIG_CaseId;
                else
                    idOblig = ((BIGvw_ObligationLastStatus)this.ImportDetailsGridView.SelectedItem).BIG_CaseId;

                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                _pozewcontext.BIG_DelObligation(idOblig, UserProfile.DbId, delObligCompleted, null);

            }
        }

        private void delObligCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }
            this.BIGvw_ImportyDetaildds.Load();




        }
    

        private void btUpdateDebtor_Click(object sender, RoutedEventArgs e)
        {
            BIGvw_DluznicyAktual r1 = null;
            string rowSerialized;

            if (this.SprawyGridView.SelectedItems.Count > 0)
            {

                r1 = ((BIGvw_DluznicyAktual)this.SprawyGridView.SelectedItem);
                rowSerialized = ToXMLSerializers.SerializeEntity(r1, typeof(BIGvw_DluznicyAktual));
              

                GetDluData wnd = new GetDluData();
             
                wnd.Nazwa = r1.Name;
                wnd.Imie = r1.Firstname;
                wnd.NrKlienta = r1.NrKlienta;
                wnd.Pesel = r1.Pesel;

                if (String.IsNullOrWhiteSpace(r1.Firstname))
                {
                    wnd.Nip = r1.IDNumber;

                }
                else
                    wnd.Pesel = r1.IDNumber;

                wnd.Adres1 = r1.Address1L1;
                wnd.Adres2 = r1.Address1L2;
                wnd.SrcSystem = r1.SrcSystem.Value;
                wnd.Show();
                wnd.Closed += (se, ea) =>
                {
                    if (wnd.DialogResult == true)
                    {
                        r1.Name=wnd.Nazwa ;
                        r1.Firstname = wnd.Imie  ;
                        r1.NrKlienta = wnd.NrKlienta  ;

                        if (String.IsNullOrWhiteSpace(r1.Firstname))
                        {
                            r1.IDNumber = wnd.Nip  ;

                        }
                        else
                            r1.IDNumber = wnd.Pesel;
                        r1.Pesel = wnd.Pesel  ;
                        r1.Address1L1 = wnd.Adres1 ;
                        r1.Address1L2 = wnd.Adres2  ;

                        PozewDomainContext _pozewcontext;
                        _pozewcontext = new PozewDomainContext();
                        this.BusyIndicator.IsBusy = true;
                        _pozewcontext.BIG_UpdateDebtor(rowSerialized,r1.Name,r1.Firstname,r1.IDNumber, r1.Pesel,r1.Address1L1, r1.Address1L2,r1.NrKlienta, r1.SrcSystem.Value, UserProfile.DbId, updateObligCompleted, null);

                    }
                };




            }
            else
                AlertMsg.Show("Musisz wybrać właściwy wiersz z listy");
        }

        private void btSuspendDebtor_Click(object sender, RoutedEventArgs e)
        {
            BIGvw_DluznicyAktual r1 = null;
            string rowSerialized;

            if (this.SprawyGridView.SelectedItems.Count > 0)
            {

                r1 = ((BIGvw_DluznicyAktual)this.SprawyGridView.SelectedItem);
                rowSerialized = ToXMLSerializers.SerializeEntity(r1, typeof(BIGvw_DluznicyAktual));
                
                DateTime dataZawiesz;
              

                GetDateWindow wnd = new GetDateWindow();
                wnd.ChosenDate = DateTime.Today.AddDays(14); // dom,yślnie 14 dni
                wnd.prompt = "Podaj datę końcową zawieszenia";
                wnd.Show();
                wnd.Closed += (se, ea) =>
                {
                    if (wnd.DialogResult == true)
                    {
                        dataZawiesz = wnd.ChosenDate.Value;
                        
                        PozewDomainContext _pozewcontext;
                        _pozewcontext = new PozewDomainContext();
                        this.BusyIndicator.IsBusy = true;
                        _pozewcontext.BIG_SuspendCase(rowSerialized,dataZawiesz, UserProfile.DbId, updateObligCompleted, null);

                    }
                };




            }
            else
                AlertMsg.Show("Musisz wybrać właściwy wiersz z listy");


        }

        private void btResumeDebtor_Click(object sender, RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.OkButtonContent = "Tak";
                dlgparm.Content = "Czy na pewno chcesz pojąć wybraną zawieszona sprawę ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confResumeCaseClose;
                RadWindow.Confirm(dlgparm);
            }
        }

        private void confResumeCaseClose(object sender, WindowClosedEventArgs e)
        {
            BIGvw_DluznicyAktual r1 = null;
            string rowSerialized;
            if ((sender as RadWindow).DialogResult == true)
            {

                r1 = ((BIGvw_DluznicyAktual)this.SprawyGridView.SelectedItem);
                rowSerialized = ToXMLSerializers.SerializeEntity(r1, typeof(BIGvw_DluznicyAktual));


                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                 _pozewcontext.BIG_UnSuspendCase(rowSerialized, UserProfile.DbId, delObligCompleted, null);

            }
        }

        private void btNotify_Click(object sender, RoutedEventArgs e)
        {
            // wysłanie powaidomień do zazanaczonych dłużników.
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.OkButtonContent = "Tak";
                dlgparm.Content = "Czy na pewno chcesz wysyłki powiadomień w zaznaczonych sprawach ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = sendNotifyCloseClose;
                RadWindow.Confirm(dlgparm);
            }
            else
                AlertMsg.Show("Musisz wybrać przynajmniej jeden wiersz z listy");
        }

        private void sendNotifyCloseClose(object sender, WindowClosedEventArgs e)
        {
            BIGvw_DluznicyAktual r1 = null;
            string rowSerialized;
            List<string> lst = new List<string>();
             
            if ((sender as RadWindow).DialogResult == true)
            {
                // lista zaznaczona
                foreach (var row in this.SprawyGridView.SelectedItems)
                {
                    rowSerialized = ToXMLSerializers.SerializeEntity(row, typeof(BIGvw_DluznicyAktual));
                    lst.Add(rowSerialized);


                } 
               rowSerialized = ToXMLSerializers.SerializeToString(lst,typeof(List<string>));
                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                _pozewcontext.BIG_NotifyCase(rowSerialized, UserProfile.DbId, delObligCompleted, null);

            }
        }

       
      

      private void btDelAll_Click(object sender, RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.OkButtonContent = "Tak";
                dlgparm.Content = "Czy na pewno chcesz usunąć wszystkich dłużników ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confdelAlluwndClose;
                RadWindow.Confirm(dlgparm);
            }
        }

        private void confdelAlluwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {
                int idDluz = 0;
                if (isFull)
                    idDluz = ((BIGvw_Dluznicy)this.SprawyGridView.SelectedItem).BIG_CaseId;
                else
                    idDluz = ((BIGvw_DluznicyAktual)this.SprawyGridView.SelectedItem).BIG_CaseId;

                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                _pozewcontext.BIG_DelAll(UserProfile.DbId, delDluCompleted, null);

            }
        }

      
      

       

       

        private void SprawyGridView_Filtered(object sender, GridViewFilteredEventArgs e)
        {
            if (isFull == false)
                this.BIGvw_DluznicyAktualdds.Load();
            else
                this.BIGvw_Dluznicydds.Load();

        }

        private void BIGvw_Dluznicydds_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
            this.BIGvw_Dluznicydds.Load();
        }
    }
    

          
    
}
   
