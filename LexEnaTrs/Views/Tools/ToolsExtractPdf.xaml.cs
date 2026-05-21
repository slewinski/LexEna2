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
    public partial class ToolsExtractPdf : UserControl
    {
        private int CurrentSprID;
        private int CurrentDokID;
      
        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int TypDok = -1;

        private Guid guid = Guid.Empty; 

        public ToolsExtractPdf()
        {
            CurrentSprID = 0;
            CurrentDokID = 0;
            InitializeComponent();
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

            if (message.Length > 0 && message.Contains("Błąd"))
            {
                ErrorWindow.CreateNew(message);
            }
            else
            {

                try {
                    this.guid = new Guid(message);
                }
                catch (Exception e)
                {
                    ErrorWindow.CreateNew("Procedura importu zwróciła niewłaściwą wartość " );
                    return;
                }
                // odczyt
                this.EkstarakcjaPdfdds.QueryParameters[0].Value = this.guid.ToString();
                this.EkstarakcjaPdfdds.Load();
            }



        }


        private void ExtractPdfCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;

            if (message.Length > 0 && message.Contains("Błąd"))
            {
                ErrorWindow.CreateNew(message);
            }
            else
            {

               
                // odczyt
                this.EkstarakcjaPdfdds.QueryParameters[0].Value = this.guid.ToString();
                this.EkstarakcjaPdfdds.Load();
            }



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

      


        private void Import_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {


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


                               //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                                _pozewcontext = new PozewDomainContext();
                                _pozewcontext.ImportDocumentXLSX(Convert.ToBase64String(theWorksheet), dlg.File.Name, -1,0, ImportFileXLSXCompleted, null);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                      
                        ErrorWindow.CreateNew(ex, "Bład odczytu zbioru");
                    }
                }
            
         else
                AlertMsg.Show("Nie wybrano żadnego zbioru");
        }

       


        private void EkstarakcjaPdfdds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }

        private void ImpFile_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OperacjeLst.SelectedIndex = -1;
            rdbImport.IsOpen = false;
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


                            //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                            _pozewcontext = new PozewDomainContext();
                            _pozewcontext.ImportDocumentXLSX(Convert.ToBase64String(theWorksheet), dlg.File.Name, -6000, 0, ImportFileXLSXCompleted, null);
                        }

                    }
                }
                catch (Exception ex)
                {

                    ErrorWindow.CreateNew(ex, "Bład odczytu zbioru");
                }
            }

            else
                AlertMsg.Show("Nie wybrano żadnego zbioru");


        }

        private void Operacje_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           
        }

        private void SprawyGridView_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {

        }

        private void EkstarakcjaPdfdds_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
            this.Rcount.Content = "Wierszy: " + this.SprawyGridView.Items.TotalItemCount.ToString();
        }

        private void ExpSelected_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<int> lst = new List<int>();
            // ekstrakcja  zaznaczonych
            if (SprawyGridView.SelectedItems.Count > 0)
            {
                foreach (var r in SprawyGridView.SelectedItems)
                {

                    if( (r as DOKEKSTRvw_ekstrakcja).idskan != null )
                    lst.Add((r as DOKEKSTRvw_ekstrakcja).idskan.Value );

                }

                this.doExtract(lst);

            }

        }

        private void ExpAll_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<int> lst = new List<int>();
            // ekstrakcja  zaznaczonych
            if (SprawyGridView.Items.Count > 0)
            {
                foreach (var r in SprawyGridView.Items)
                {

                    if ((r as DOKEKSTRvw_ekstrakcja).idskan != null)
                    lst.Add((r as DOKEKSTRvw_ekstrakcja).idskan.Value);

                }

                this.doExtract(lst);

            }
        }



        private void doExtract(List<int> lst)
        {

            PozewDomainContext _pozewcontext;
            string listId = ToXMLSerializers.SerializeToString(lst, typeof(List<int>));

            _pozewcontext = new PozewDomainContext();
            _pozewcontext.ExportPdfs(listId, this.guid.ToString(), ExtractPdfCompleted, null);
            




        }

      

        private void rbreport_Click(object sender, RoutedEventArgs e)
        {
            rdbImport.IsOpen = false;

                   ReportWindow repwin = new ReportWindow();

            if (this.guid != null && this.guid != Guid.Empty)
            {


                repwin.Mode = 4000;
                repwin.StringArg = this.guid.ToString();
                repwin.Show();

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
                foreach (string docname  in  docFilter.DistinctFilter.DistinctValues)
                {
                    lstr.Add(docname);
                }

            }
            
            docFilter.SuspendNotifications();
            return lstr;

        }

        private void SprawyGridView_Filtered(object sender, GridViewFilteredEventArgs e)
        {
           this.Rcount.Content = "Wierszy: " + this.SprawyGridView.Items.Count.ToString();
        }

        private void rbreportvalid_Click(object sender, RoutedEventArgs e)
        {
            string nazwy = "";
            rdbImport.IsOpen = false;

            List<string> lst = getFilter();
            if (lst != null && lst.Count > 0)
            {
                nazwy = String.Join(";", lst.ToArray());
            }
            ReportWindow repwin = new ReportWindow();

            if (this.guid != null && this.guid != Guid.Empty)
            {


                repwin.Mode = 4001;
                repwin.StringArg = this.guid.ToString();
                repwin.StringArg2 = nazwy;
                repwin.Show();

            }
        }
    }
    
}
   
