using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.FixedDocumentViewersUI.Dialogs;
using Telerik.Windows.Documents.Fixed;
using Telerik.Windows.Documents.Fixed.FormatProviders;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.UI.Extensibility;

namespace LexEnaTrs.Views
{
    public partial class PdfViewer : ChildWindow
    {



        public byte[] PdfBin { get; set; }

        public PdfViewer()
        {
            LocalizationManager.Manager = new LocalizationManager()
            {
                ResourceManager = PdfViewerResources.ResourceManager
            };
            ExtensibilityManager.RegisterFindDialog(new FindDialog());

            InitializeComponent();
        }
        private void tbCurrentPage_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
            }
        }

        private void LoadFromStream(object sender, System.Windows.RoutedEventArgs e)
        {
            Stream str = App.GetResourceStream(new System.Uri("FirstLook;component/SampleData/Sample.pdf", System.UriKind.Relative)).Stream;
            this.pdfViewer.DocumentSource = new PdfDocumentSource(str);
        }

        private void LoadFromUri(object sender, System.Windows.RoutedEventArgs e)
        {
            this.pdfViewer.DocumentSource = new PdfDocumentSource(new System.Uri("FirstLook;component/SampleData/Sample.pdf", System.UriKind.Relative));
        }

        private SaveFileDialog saveFileDialog;
        private byte[] document;

        private void InitializeSaveDialog()
        {
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "pdf";
            saveFileDialog.Filter = "pdf Files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;

        }
        private void rbSaveAS_Click(object sender, RoutedEventArgs e)
        {

            InitializeSaveDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                using (Stream fs = (Stream)this.saveFileDialog.OpenFile())
                {
                    fs.Write(PdfBin, 0, PdfBin.Length);
                    fs.Close();
                }
            }
        }











        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
          //  this.pdfVw2.DocumentSource = new PdfDocumentSource(new MemoryStream(PdfBin));
            
            MemoryStream stream = new MemoryStream(PdfBin);

            FormatProviderSettings settings = new FormatProviderSettings(ReadingMode.AllAtOnce);
            PdfFormatProvider provider = new PdfFormatProvider(stream, settings);
            RadFixedDocument doc = provider.Import();
            this.pdfViewer.Document = doc;
           
        }


        /*
         
         string pdfFilePath = "Sample.pdf";
MemoryStream stream = new MemoryStream();

using (Stream input = File.OpenRead(pdfFilePath))
{
    input.CopyTo(stream);
}

FormatProviderSettings settings = new FormatProviderSettings(ReadingMode.OnDemand);
PdfFormatProvider provider = new PdfFormatProvider(stream, settings);
RadFixedDocument doc = provider.Import();
this.pdfViewer.Document = doc;
         
         */


    }
}

