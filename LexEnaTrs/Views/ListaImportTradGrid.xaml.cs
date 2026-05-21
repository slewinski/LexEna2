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
    public partial class ListaImportTradGrid : UserControl
    {
        private int CurrentSprID;
        private int CurrentDokID;
        //private int[] IdDw = new int[] { 2556, 17004, 19496, 22310, 22155, 22311, 21856, 21781, 21769, 21773, 21779, 21774, 21860, 21867, 21861, 21977, 21987, 21981, 21979, 21973, 21996, 21990, 22049, 21941, 22007, 22001, 21882, 21886, 21879, 21877, 21873, 21871, 21883, 21966, 21967, 21984, 22027, 22025, 21995, 21982, 21997, 22308, 21991, 21972, 21986, 21970, 21978, 21975, 21998, 21974, 21968, 21869, 21870, 21887, 21888, 21890, 22053, 21950, 21962, 21960, 21952, 21961, 21951, 22064, 21946, 22019, 21944, 22061, 22017, 21945, 22003, 22002, 22008, 22011, 22005, 21918, 21893, 21913, 21897, 21914, 21899, 21909, 21900, 21902, 21896, 21916, 21904, 21911, 21892, 22062, 22066, 22067, 22063, 22068, 22016, 22010, 22014, 22050, 22046, 22029, 22018, 22031, 22032, 22020, 22041, 22042, 22037, 22035, 22043, 22038, 22039, 22036, 21957, 21959, 21964, 21965, 21955, 21958, 21956, 21954, 22059, 21953, 21881, 22315, 21894, 21903, 21905, 21912, 21922, 21930, 21940, 21891, 21895, 21910, 21920, 21921, 21925, 21929, 21932, 21889, 21927, 21938, 21935, 21937, 21939, 22316, 21934, 22491, 22144, 22077, 22090, 22092, 22085, 22084, 22082, 22079, 22080, 22081, 22086, 22091, 22089, 22088, 22078, 22122, 22124, 22118, 22209, 22099, 22098, 22141, 22138, 22097, 22100, 22211, 22168, 22205, 22203, 22202, 22200, 22095, 22102, 22094, 22096, 22101, 22207, 22173, 22172, 22171, 22210, 22214, 22104, 22106, 22112, 22107, 22163, 22167, 22165, 22166, 22119, 22121, 22123, 22126, 22152, 22149, 22147, 22146, 22145, 22206, 22199, 22198, 22197, 22196, 22191, 22189, 22180, 22194, 22190, 22178, 22182, 22195, 22177, 22183, 22193, 22176, 22186, 22159, 22157, 22161, 22244, 22230, 22327, 22234, 22325, 22323, 22232, 22331, 22322, 22324, 22329, 22321, 22326, 22237, 22306, 22307, 22305, 22282, 22386, 22387, 22304, 22333, 22257, 22256, 22223, 22222, 22340, 22219, 22220, 22221, 22339, 22337, 22335, 22246, 22249, 22248, 22275, 22268, 22225, 22228, 22226, 22298, 22296, 22292, 22291, 22375, 22366, 22379, 22319, 22281, 22280, 22355, 22273, 22271, 22267, 22262, 22383, 22461, 22412, 22408, 22406, 22404, 22456, 22451, 22449, 22401, 22413, 22422, 22416, 22427, 22426, 22433, 22439, 22545, 22496, 22533, 22508, 22506, 22502, 22499, 22476, 22478, 22524, 22399, 22513, 22448, 22594, 22573, 22569, 22673, 22647, 22653, 22687, 22688, 22685, 22683, 22690, 22669, 22740, 22744, 22773, 22765, 22766, 22762, 22605, 22608, 22600, 22609, 22612, 22610, 22721, 22723, 22724, 22727, 22707, 22703, 22784, 22783, 22781, 22778, 22777, 22776, 22782, 22774};
        //private int[] IdDw = new int[] { 22258 ,22353 , 22920 , 23130 };
        // Parametry publiczne

        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int ImpTyp = - 1;
        public int TypDok = -1;
        public byte[] zipfile;
        public byte[] xlsxfile;
        public string zipString;
        PozewDomainContext _pozewcontext = null; 

        public ListaImportTradGrid()
        {
            CurrentSprID = 0;
            CurrentDokID = 0;
            InitializeComponent();
        }







        private void SprawyGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {



            vw_Importy lst;


            if (e.AddedItems.Count > 0)
            {
                lst = (vw_Importy)e.AddedItems[0];
                CurrentSprID = lst.Id;
                vw_ImportDetailsdds.QueryParameters[0].Value = lst.Id;
                vw_ImportDetailsdds.Load();
            }


        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (UserProfile.Rola < 2)
                this.vw_ListImportsdds.QueryParameters[0].Value = IdJednostki;
            else
                this.vw_ListImportsdds.QueryParameters[0].Value = 0;

            this.vw_ListImportsdds.QueryParameters[1].Value = ImpTyp;


            try
            {
                this.vw_ListImportsdds.PageSize = 300;//1000;



                // Tu ustaiwć widoczność poszczególnych kolumn this.SprawyGridView.Columns[""]
                this.vw_ListImportsdds.Load();
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
            this.vw_ListImportsdds.Load();




        }



        private void ImportFileXLSXCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
                this.rbImportXLS.Visibility = Visibility.Collapsed;
                this.labelxlsx.Content = "";
                this.labelxlsx.Visibility = Visibility.Collapsed;
                this.labelzip.Content = "";

            }
            this.vw_ListImportsdds.Load();




        }

       
        private void OperacjeLst_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void vw_ListImportsdds_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                ErrorWindow.CreateNew(e.Error.ToString() + " Błąd odczytu danych ");
                e.MarkErrorAsHandled();
            }
        }

        private void vw_ListaImportsdds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }



        private void SearchItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void DetailsItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void vw_ImportDetailsdds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {

        }

        private void ImportDetailsGridView_RowLoaded(object sender, RowLoadedEventArgs e)
        {

        }

        private void rbImportXLS_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
        
            // Set Filter to browser text files
            dlg.Filter = "Zbiory XLSX (*.xlsx)|*.xlsx|Wszystkie zbiory (*.*)|*.*";
            try
            {
                bool? result = dlg.ShowDialog();
                if (result.HasValue && result.Value)
                {

                    using (FileStream stream = dlg.File.OpenRead())
                    {
                        this.labelxlsx.Content = dlg.File.Name;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            this.xlsxfile = ms.ToArray();
                            if (this.currGuid != null && this.currGuid != Guid.Empty)
                            {
                                this.BusyIndicator.IsBusy = true;
                                if (_pozewcontext == null )
                                _pozewcontext = new PozewDomainContext();
                                _pozewcontext.ImportDocumentsZipXLSX(currGuid.ToString(), this.labelzip.Content as String, Convert.ToBase64String(xlsxfile), dlg.File.Name, UserProfile.Firma == -1 ? -UserProfile.DbId : UserProfile.DbId, UserProfile.IdJednostki, ImportFileXLSXCompleted, null);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(ex, "Błąd odczytu zbioru");
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

        List<byte[]> zipResult;
        Guid currGuid;

        private void updatePart(LexEnaMeritumDomainContext _dbcontext,  Guid guId, int i)
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
                        updatePart(_dbcontext, guId, i);
                    else
                    {
                        this.BusyIndicator.IsBusy = false;
                        this.rbImportXLS.IsEnabled = true;
                    }
                };


            }


        private void rbImportPdf_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            int partSize = 3000000;
            List<string> zipSplit = new List<string>();
             zipResult = new List<byte[]>();
            dlg.Multiselect = false;
            byte[] thezip;
            // Set Filter to browser text files
            dlg.Filter = "Zbiory zip (*.zip)|*.zip|Zbiory 7 zip (*.7z)|*.7z|Zbiory rar (*.rar)|*.rar|Wszystkie zbiory (*.*)|*.*";
            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                try
                {
                    using (FileStream stream = dlg.File.OpenRead())
                    {
                        this.labelzip.Content = dlg.File.Name;
                        using (MemoryStream ms = new MemoryStream())
                        {
                           stream.CopyTo(ms);
                           thezip = ms.ToArray();
                            for (int i = 0; i < thezip.Length; i += partSize)
                            {
                                int bufSize = thezip.Length - i >= partSize ? partSize : thezip.Length - i;
                                byte[] buffer = new byte[bufSize];
                                Buffer.BlockCopy(thezip, i, buffer, 0,bufSize);
                                zipResult.Add(buffer);
                            }

                        }
                        if (zipResult.Any())
                        {
                            LexEnaMeritumDomainContext _dbcontext = new LexEnaMeritumDomainContext();
                            currGuid = Guid.NewGuid();
                            this.BusyIndicator.IsBusy = true;
                            updatePart(_dbcontext, currGuid, 0);



                            if (this.labelxlsx.Visibility == Visibility.Collapsed)
                            {

                                this.labelxlsx.Visibility = Visibility.Visible;
                                this.rbImportXLS.Visibility = Visibility.Visible;


                            }




                        }




                        }

                        /*



                            zipString = Convert.ToBase64String(zipfile);
                            int currindex = 0;
                            string part = "";
                            int strlen = zipString.Length;
                            

                            part = zipString.Substring(currindex, currindex + partSize > zipString.Length ? zipString.Length - currindex : partSize);
                            while (part.Length > 0)
                            {
                                zipSplit.Add(part);
                                currindex += part.Length;
                                part = zipString.Substring(currindex, currindex + partSize > zipString.Length ? zipString.Length - currindex : partSize);
                                
                            }

                            if ( _pozewcontext == null)
                                _pozewcontext = new PozewDomainContext();
                            bool isfirst = true;
                            foreach ( string s in zipSplit)
                            { 
                            _pozewcontext.ImportDocumentsZipPart(s, isfirst).Completed += (Object sender1, EventArgs eve) =>
                                  {

                                      isfirst = false;
                                  };

                                isfirst = false;
                            }



                            if (this.labelxlsx.Visibility == Visibility.Collapsed)
                            {

                                this.labelxlsx.Visibility = Visibility.Visible;
                                this.rbImportXLS.Visibility = Visibility.Visible;
                                

                            }

                          
                            //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                            //_pozewcontext = new PozewDomainContext();
                            //_pozewcontext.ImportDocumentXLSX(Convert.ToBase64String(thezip), dlg.File.Name, UserProfile.DbId, UserProfile.IdJednostki, ImportFileXLSXCompleted, null);
                        }
                        */
                   

                }
                catch (Exception ex)
                {
                    this.BusyIndicator.IsBusy = false;
                    ErrorWindow.CreateNew(ex, "Bład odczytu zbioru skompresowanego");
                }
            }
        }

      
    }
}
   
