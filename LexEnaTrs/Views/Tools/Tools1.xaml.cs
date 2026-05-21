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
    public partial class Tools1 : UserControl
    {
        private int CurrentSprID;
        private int CurrentDokID;
      
        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int TypDok = -1;


        public Tools1()
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
                 rdbImport.IsOpen = false;
                ReportWindow repwin = new ReportWindow();
                repwin.Mode = 5000;
                repwin.StringArg = message;
                repwin.Show();

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

       

        private void SpadyEpu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                            _pozewcontext.ImportDocumentXLSX(Convert.ToBase64String(theWorksheet), dlg.File.Name, -5000, 0, ImportFileXLSXCompleted, null);
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

        private void Narzędzia_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
    
}
   
