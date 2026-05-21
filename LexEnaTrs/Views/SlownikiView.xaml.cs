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
    public partial class SlownikiView : UserControl
    {

        public int TypSlownika;
        public SlownikiView()
        {
            InitializeComponent();
        }



        private void SlownikiDataForm_DeletingItem(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogParameters dlgparm = new DialogParameters();
            dlgparm.CancelButtonContent = "Nie";
            dlgparm.Content = "Czy na pewno chcesz usunąć wybraną pozycję ?";
            dlgparm.Header = "Potwierdź";
            dlgparm.Closed = confdluwndClose;
            RadWindow.Confirm(dlgparm);
            e.Cancel = true;
        }

            

        private void confdluwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {
                DataItemCollection itemsToDel = (DataItemCollection)this.SlownikiDataForm.ItemsSource;//  as IList<DaneDluznika>;
                itemsToDel.Remove(this.SlownikiDataForm.CurrentItem as Slownik);

                this.Slownikidds.SubmitChanges();
            }
        }

        private void SlownikiataForm_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            Slownik newitem;
            newitem = (Slownik)e.NewItem;
            newitem.Typ = TypSlownika;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Slownikidds.QueryParameters[0].Value = TypSlownika;
            this.Slownikidds.Load();
        }

        private void SlownikiDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            this.Slownikidds.SubmitChanges();
        }

       

      

        
       



    }
}
