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
    public partial class ToolsUmorzeniaKoszty : UserControl
    {
        private int CurrentSprID;
        private int CurrentDokID;

        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int TypDok = -1;
        private string filename = string.Empty;
        private Guid guid = Guid.Empty;
        List<ZaliczkiImportData> zal;


        public ToolsUmorzeniaKoszty()
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
            BusyIndicator.IsBusy = false;
            if (message.Length > 0 && message.Contains("Błąd"))
            {
                ErrorWindow.CreateNew(message);
            }
            else
            {

                this.zal = ToXMLSerializers.XmlDeserializeFromString(message, typeof(List<ZaliczkiImportData>)) as List<ZaliczkiImportData>;

                ZaliczkiGrid.ItemsSource = zal;
                rbSave.Visibility = Visibility.Visible;

            }



        }







        private void rbImpFile_Click(object sender, RoutedEventArgs e)
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
                            this.filename = dlg.File.Name;

                            //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                            _pozewcontext = new PozewDomainContext();
                            BusyIndicator.IsBusy = true;
                            _pozewcontext.ImportZaliczki(Convert.ToBase64String(theWorksheet), dlg.File.Name, UserProfile.DbId, UserProfile.Firma,1, ImportFileXLSXCompleted, null);
                        }

                    }
                }
                catch (Exception ex)
                {

                    ErrorWindow.CreateNew(ex, "Błąd odczytu zbioru");
                }
            }

            else
                AlertMsg.Show("Nie wybrano żadnego zbioru");


        }
        private void zapiszZal(int mode)
        {
            string z = ToXMLSerializers.SerializeToString(this.zal,this.zal.GetType());
            PozewDomainContext _pozewcontext = new PozewDomainContext();
             BusyIndicator.IsBusy = true;
            _pozewcontext.UsunKoszty(z, this.filename, UserProfile.DbId, UserProfile.Firma, ZapisZaliczkiCompleted, null);


        }


        private void rbSave_Click(object sender, RoutedEventArgs e)
        {

            zapiszZal(0);

        }

        private void rbRegister_Click(object sender, RoutedEventArgs e)
        {
            zapiszZal(1);
        }


        private void ZapisZaliczkiCompleted(InvokeOperation<string> result)
        {

            BusyIndicator.IsBusy = false;
            string message = result.Value;

            if (message.Length > 0 && message.Contains("Błąd"))
            {
                ErrorWindow.CreateNew(message);
            }
            else
            {

                this.zal = ToXMLSerializers.XmlDeserializeFromString(message, typeof(List<ZaliczkiImportData>)) as List<ZaliczkiImportData>;

                ZaliczkiGrid.ItemsSource = zal;
                AlertMsg.Show("Koszty zostały usunięte z systemu Wiena");

            }

        }
    }

}
   
