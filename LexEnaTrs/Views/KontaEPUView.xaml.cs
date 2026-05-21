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
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using LexEnaTrs;
using LexEnaTrs.Web;
using System.ServiceModel.DomainServices.Client;

namespace LexEnaTrs.Views
{
    public partial class KontaEPUView : UserControl
    {
        public KontaEPUView()
        {
            InitializeComponent();
        }



        private void KontaEPUDataForm_DeletingItem(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogParameters dlgparm = new DialogParameters();
            dlgparm.CancelButtonContent = "Nie";
            dlgparm.Content = "Czy na pewno chcesz usunąć konto ?";
            dlgparm.Header = "Potwierdź";
            dlgparm.Closed = confdluwndClose;
            RadWindow.Confirm(dlgparm);
            e.Cancel = true;
        }

        private void KontaEPUDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            KontoEPU item;
            item = (this.KontaEPUDataForm.CurrentItem as KontoEPU);
            item.EPUPasswd = base64CodeDecode.Encode(item.EPUPasswd);
            this.KontaEPUdds.SubmitChanges();
            btCheckConnection.Visibility = Visibility.Visible;
        }




        private void confdluwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {
                DataItemCollection itemsToDel = (DataItemCollection)this.KontaEPUDataForm.ItemsSource;//  as IList<DaneDluznika>;
                itemsToDel.Remove(this.KontaEPUDataForm.CurrentItem as KontoEPU);

                this.KontaEPUdds.SubmitChanges();
            }
        }

        private void KontaEPUDataForm_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            KontoEPU newitem;
            newitem = (KontoEPU)e.NewItem;
            newitem.kraj = "PL";
            newitem.Czyaktywne = 1;
        }

        private void KontaEPUDataForm_BeginningEdit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            KontoEPU item;
            btCheckConnection.Visibility = Visibility.Collapsed;
            item = (this.KontaEPUDataForm.CurrentItem as KontoEPU);
            item.EPUPasswd = base64CodeDecode.Decode(item.EPUPasswd);

        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            if (UserProfile.Rola == 2)
                this.KontaEPUdds.QueryParameters[0].Value = 0;
            else
                this.KontaEPUdds.QueryParameters[0].Value = UserProfile.IdJednostki;
            this.KontaEPUdds.Load();

        }

        private void btCheckConnection_Click(object sender, RoutedEventArgs e)
        {
            EpuContext ec = new EpuContext();
            KontoEPU kepu = KontaEPUDataForm.CurrentItem as KontoEPU;
            ec.SprawdzPolaczenie(kepu.LoginEPU, base64CodeDecode.Decode(kepu.EPUPasswd), kepu.APIKey, SprawdzPolaczenie_Completed, null);

            // EpuProxy  ec.MojeSprawy
        }
        private void SprawdzPolaczenie_Completed(InvokeOperation<string> result)
        {
            string errText;
            if (result.HasError)
                errText = "Wystąpił nieznany błąd podczas testu.";
            else
                errText = result.Value;

            if (String.IsNullOrWhiteSpace(errText))
            {
                AlertMsg.Show("OK !!! Połączenie przebiegło pomyślnie.");


            }
            else
            {
                Exception ex = new Exception(errText);
                ErrorWindow.CreateNew(ex);

            }
        }

    }
}
