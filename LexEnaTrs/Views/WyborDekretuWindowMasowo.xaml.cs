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
using System.ServiceModel.DomainServices.Client;
using Telerik.Windows.Controls;

namespace LexEnaTrs.Views
{
   

    public partial class WyborDekretuWindowMasowo : ChildWindow
    {
        public int IdJednostki
        {

            get;
            set;
        }

        public WyborDekretuWindowMasowo()
        {
            InitializeComponent();
        }

        private LexEnaMeritumDomainContext context = null;
        private List<vw_ListaSpraw> lstspr = null;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.GridViewSelJednostka.Items.Count > 0)
                this.IdJednostki = (this.GridViewSelJednostka.SelectedItem as JednostkaWindykacji).Id;
            else
            {
                AlertMsg.Show("Musisz wybrać jednostkę (kancelarię ) do dekretacji");
                return;
            }
            string text = ptbPrefix.Text;
            if (String.IsNullOrWhiteSpace(text))
            {
                AlertMsg.Show("Podaj prefiks sygnatury do selekcji spraw do dekretacji");
                return;

            }
            text = text.Trim()+"-";
            {
                lstspr = null; 
                if (this.context == null)
                    this.context = new LexEnaMeritumDomainContext();
                EntityQuery<vw_ListaSpraw> query = context.GetVw_ListaSprawParamsQuery(-1, 0, -1, -1).Where(a => a.Id_Jednostki == 0 && a.sygnatura.StartsWith(text));
                LoadOperation<vw_ListaSpraw> loadop;
                loadop = context.Load(query);
                loadop.Completed += (se, ev) =>
                {
                    int lp = loadop.Entities.Count();
                    if (lp > 0)
                    {
                        lstspr = loadop.Entities.ToList();
                        DialogParameters dlgparm = new DialogParameters();
                        dlgparm.CancelButtonContent = "Nie";
                        dlgparm.OkButtonContent = "Tak";
                        dlgparm.Content = "Czy na pewno chcesz zadekretować " + lp.ToString() + "  spraw do " + (this.GridViewSelJednostka.SelectedItem as JednostkaWindykacji).Nazwa + " ?";
                        dlgparm.Header = "Potwierdź";
                        dlgparm.Closed = confWndClosed;
                        RadWindow.Confirm(dlgparm);


                    }

                };

            }
            
        }


        private void confWndClosed(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {
                this.DialogResult = true;

                foreach (var r in this.lstspr)
                {
                    Dekretacja dekret = new Dekretacja();
                    dekret.Czyus = 0;
                    dekret.DataDekretJednostka = DateTime.Now;
                    dekret.JednostkaWindykacji_Id = this.IdJednostki;
                    dekret.Sprawa_id = (r as vw_ListaSpraw).id;
                    context.Dekretacjas.Add(dekret);
                    
                }
                context.SubmitChanges().Completed += (sndr, evn) =>
                {
                    this.DialogResult = true;


                };

            }
            else
                this.DialogResult = false;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

