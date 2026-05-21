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

namespace LexEnaTrs.Views
{
    public partial class JednostkiKonfigView : UserControl
    {
        public JednostkiKonfigView()
        {
            InitializeComponent();
        }

        private void JednostkiDataForm_DeletedItem(object sender, Telerik.Windows.Controls.Data.DataForm.ItemDeletedEventArgs e)
        {

        }

        private void JednostkiDataForm_DeletingItem(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogParameters dlgparm = new DialogParameters();
            dlgparm.CancelButtonContent = "Nie";
            dlgparm.Content = "Czy na pewno chcesz usunąć tę jednostkę ?";
            dlgparm.Header = "Potwierdź";
            dlgparm.Closed = confdluwndClose;
            RadWindow.Confirm(dlgparm);
            e.Cancel = true;
        }

        private void confdluwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {
                DataItemCollection itemsToDel = (DataItemCollection)this.JednostkiDataForm.ItemsSource;//  as IList<DaneDluznika>;
                itemsToDel.Remove(this.JednostkiDataForm.CurrentItem as KontoEPU);

                this.JednostkiWindykacjidds.SubmitChanges();
            }
        }

        private void JednostkiDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            this.JednostkiWindykacjidds.SubmitChanges();
        }

       

        private void KontaEPUDataForm_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            this.JednostkiWindykacjidds.SubmitChanges(); 
        }

        private void JednostkiDataForm_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            JednostkaWindykacji newitem;
            newitem = (JednostkaWindykacji)e.NewItem;
            newitem.TypJednostki = 1;
            
        }

     
      



    }
}
