using LexEnaTrs.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Documents.Fixed;

namespace LexEnaTrs.Views
{
    public partial class AddOutDocumentWindow : ChildWindow
    {
        public DokWys dokWys { get; set; }
        public int IdPdf { get; set; }
        public DokOdebr dokOdebr { get; set; }


        public AddOutDocumentWindow()
        {
            IdPdf = 0;
            InitializeComponent();
            this.ShowPdfButton.Visibility = Visibility.Collapsed;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void PdfInButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            PozewDomainContext _pozewcontext;
            byte[] mypdf;
            // Set Filter to browser text files
            dlg.Filter = "Zbiory PDF (*.pdf)|*.pdf|Wszystkie zbiory (*.*)|*.*";
            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
               
                    using (FileStream stream = dlg.File.OpenRead())
                    {

                        using (MemoryStream ms = new MemoryStream())
                        {
                        stream.CopyTo(ms);
                        mypdf = ms.ToArray();
                        this.BusyIndicator.IsBusy = true;
                        _pozewcontext = new PozewDomainContext();
                       _pozewcontext.UploadDocumentPdf(Convert.ToBase64String(mypdf), (dokWys!= null) ? dokWys.Nazwa:dokOdebr.Nazwa, (dokWys != null) ?dokWys.Id :0, (dokOdebr != null) ? dokOdebr.Id : 0, UserProfile.DbId,ImportFileCompleted, null);


                    }
                }
            }

        }


        private void ImportFileCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            int DokId = 0;

            this.BusyIndicator.IsBusy = false;

            if (message == null) return;
            if (message.Length > 0)
            {
                if (Int32.TryParse(message, out DokId))
                {// zwrócono Id pdf'a
                    IdPdf = DokId;
                    this.ShowPdfButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ErrorWindow.CreateNew(message);
                    return;
                }
            }
        }
        //  ; reload info



       

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (dokWys != null)
            {
                this.DataContext = dokWys;
                if (dokWys.PdfStore.Any())
                {
                    this.ShowPdfButton.Visibility = Visibility.Visible;

                }
                else
                    this.ShowPdfButton.Visibility = Visibility.Collapsed;
                if (dokWys.Id > 0) // jeśli dokument już dodany
                {

                    this.cbTypDok.IsReadOnly = true;
                }
                this.tbNazwaOdebr.Visibility = Visibility.Collapsed;
                this.dtpDataDostOdebr.Visibility = Visibility.Collapsed;
                this.tbDataDostar.Visibility = Visibility.Collapsed;
                this.cbTypDokOdebr.Visibility = Visibility.Collapsed;
                this.dtpDataOdebr.Visibility = Visibility.Collapsed;
            }
            else
                if (dokOdebr != null)
            {
                this.DataContext = dokOdebr;
                if (dokOdebr.PdfStore.Any())
                {
                    this.ShowPdfButton.Visibility = Visibility.Visible;

                }
                else
                    this.ShowPdfButton.Visibility = Visibility.Collapsed;
                if (dokOdebr.Id > 0) // jeśli dokument już dodany
                {

                    this.cbTypDok.IsReadOnly = true;
                }

                this.cbTypDok.Visibility = Visibility.Collapsed;
                this.dtpDataWys.Visibility = Visibility.Collapsed;
                this.tbNazwaWys.Visibility = Visibility.Collapsed;



            }

        }

        private void ShowPdfButton_Click(object sender, RoutedEventArgs e)
        {
            PozewDomainContext _pozewcontext = new PozewDomainContext();

            this.BusyIndicator.IsBusy = true;
            _pozewcontext = new PozewDomainContext();
            _pozewcontext.GetDocumentPdf(this.IdPdf, 100, downloadFileCompleted, null);

        }

        private void downloadFileCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
         

            this.BusyIndicator.IsBusy = false;

            try
            {

                if (message == null) return;
                if (message.Length > 0)
                {


                    byte[] data = Convert.FromBase64String(message);
                    if (data == null)
                    {
                        ErrorWindow.CreateNew("Błąd podczas dekompresji pdf");
                        return;
                    }

                /*    WebClient webClient = new WebClient();
                     webClient.DownloadProgressChanged += new  DownloadProgressChangedEventHandler  (webClient_DownloadProgressChanged);
                     webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
                    webClient.OpenReadAsync(new Uri("YourPDF_URI", UriKind.xxx));
                    //progress can be activated this way(uncomment code above)
                     ProgressTextBlock.Text = "Downloading " + e.ProgressPercentage + "%";
                        //In the openReadCompleted handler you can retrieve the data:
                    if (e.Error != null) { ProgressTextBlock.Text = "Error: " + e.Error.Message; return; }
                    */
                    /*
                     
                     SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "pdf Files|*.pdf";
                dialog.DefaultFileName = "BeneficiaryDesignation.pdf";
                if (dialog.ShowDialog() ?? false)
                {


                    WebClient webClient = new WebClient();
                    webClient.OpenReadCompleted += (s, e2) =>
                    {
                        try
                        {
                            using (Stream fs = (Stream)dialog.OpenFile())
                            {
                                e2.Result.CopyTo(fs);
                                fs.Flush();
                                fs.Close();
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    };
                    string str = App.Current.Host.Source.AbsoluteUri;
                    string path = App.appConfiguration.GetPDFPath("BeneficiaryDesignation.pdf");
                    str = str.Replace("/ClientBin/ProjectDemo.xap", path);                    
                   
                    webClient.OpenReadAsync(new Uri(str), UriKind.RelativeOrAbsolute);
                } 
                    /*
                    PdfViewer pdfv = new PdfViewer();
                    pdfv.PdfBin = data;
                    pdfv.pdfViewer.DocumentSource = new PdfDocumentSource(new MemoryStream(data)); // file as Telerik.Windows.Documents.Fixed.FixedDocumentStreamSource;

                    pdfv.Show();
                    */

                    /*   TestPdf tpd = new TestPdf();
                       tpd.Show();
                       */
                }
                else
                {

                    ErrorWindow.CreateNew(" Pobranie pdf'a nie powiodło się ");
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, "Błąd podczas wyświetlania pdf");
                return;
            }
            
        }
    }
}

