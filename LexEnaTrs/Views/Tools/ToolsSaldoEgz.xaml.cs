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
    public partial class ToolsSaldoEgz : UserControl
    {
        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int TypDok = -1;

        private Guid guid = Guid.Empty; 

        public ToolsSaldoEgz()
        {
       
            InitializeComponent();
            this.dtPickStan.SelectedDate = DateTime.Today;
            if (UserProfile.IdJednostki < 0)
                this.Format.Visibility = Visibility.Visible;
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
            this.BusyIndicator.IsBusy = false;
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
                //uruchomienie raportu

                // odczyt
                //this.EkstarakcjaPdfdds.QueryParameters[0].Value = this.guid.ToString();
                //this.EkstarakcjaPdfdds.Load();
                ReportWindow repwin = new ReportWindow();

                if (this.guid != null && this.guid != Guid.Empty)
                {

                    if (this.Format.IsChecked == true)
                    {
                        repwin.Mode = 4081;
                    }
                    else
                        repwin.Mode = 4080;
                    repwin.StringArg = this.guid.ToString();
                    repwin.Show();

                }
            }



        }




       



      

        private void rbreport_Click(object sender, RoutedEventArgs e)
        {
                   ReportWindow repwin = new ReportWindow();

            if (this.guid != null && this.guid != Guid.Empty)
            {


                repwin.Mode = 4080;
                repwin.StringArg = this.guid.ToString();
                repwin.Show();

            }
        }


      
      
        private void rbImpFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            DateTime dStanu   =  this.dtPickStan.SelectedDate.Value;
            PozewDomainContext _pozewcontext;
            byte[] theWorksheet;
            // Set Filter to browser text files
            dlg.Filter = "Zbiory XLSX (*.xlsx)|*.xlsx|Zbiory xls (*.xls)|*.xls|Wszystkie zbiory (*.*)|*.*";


            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                try
                {
                    this.BusyIndicator.IsBusy = true;
                    using (FileStream stream = dlg.File.OpenRead())
                    {

                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            theWorksheet = ms.ToArray();


                            //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                            _pozewcontext = new PozewDomainContext();
                            _pozewcontext.ImportDocumentXLSX(Convert.ToBase64String(theWorksheet), dStanu.ToString("yyyy-MM-dd") , this.Format.IsChecked == true  ? -8001:-8000 , UserProfile.Firma, ImportFileXLSXCompleted, null);
                        }

                    }
                }
                catch (Exception ex)
                {
                    this.BusyIndicator.IsBusy = false;
                    ErrorWindow.CreateNew(ex, "Bład odczytu zbioru");
                }
            }

            else
                AlertMsg.Show("Nie wybrano żadnego zbioru");


        }
    }
    
}
   
