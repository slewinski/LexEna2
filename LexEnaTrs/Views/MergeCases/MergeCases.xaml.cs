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
    public partial class MergeCases : UserControl
    {
       
      
        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int TypDok = -1;

        private Guid guid = Guid.Empty;
        private string oddzialStr;
        private string systemStr;
        private ObservableCollection<LexEnaTrs.Web.Models.SprImportDescriptor> sprawyList ;

        public MergeCases()
        {
           
            InitializeComponent();
          
        }

        private void btMerge_Click(object sender, RoutedEventArgs e)
        {
            // 
            string srcSygn = this.tbSygnSrc.Text.Trim();
            string destSygn = this.tbSygnDest.Text.Trim();
            RadWindow.Confirm("Czy na pewno chcesz sprawę " + srcSygn  + " dolączyć do sprawy " + destSygn + " ? ", this.OnClosed);

        }


        private void OnClosed(object sender, WindowClosedEventArgs e)
        {
            var result = e.DialogResult;
            if (result == true)
            {
                string srcSygn = this.tbSygnSrc.Text.Trim();
                string destSygn = this.tbSygnDest.Text.Trim();

                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                //                     this.BusyIndicator.Content = "Przetwarzanie zbioru " + dlg.File.Name + " proszę czekać...";
                _pozewcontext.MergeCases(LexEnaKonfiguracja.Firma, srcSygn, destSygn, MergeCasesCompleted, null);

            }
        }

       

        private void MergeCasesCompleted(InvokeOperation<string> result)
        {
            this.BusyIndicator.IsBusy = false;
            string message = result.Value;
        
            if (message.Length > 0 && message.StartsWith("Błąd"))
            {
                ErrorWindow.CreateNew(message);
            }
            else
            {

                try
                {
                    string srcSygn = this.tbSygnSrc.Text.Trim();
                    string destSygn = this.tbSygnDest.Text.Trim();

                    RadWindow.Alert("Sprawy " + srcSygn + " oraz " + destSygn + " zostały połączone ");
                    this.tbSygnSrc.Text = "";
                    this.tbSygnDest.Text = "";

                }
                catch (Exception e)
                {
                    ErrorWindow.CreateNew(e, "Procedura łączenia spraw zwróciła niewłaściwą wartość ");
                    return;
                }

            }



        }

    }
}
    
  
    

   
