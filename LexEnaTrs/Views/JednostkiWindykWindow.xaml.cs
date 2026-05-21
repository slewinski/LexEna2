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
using LexEnaTrs;

namespace LexEnaTrs.Views
{
    public partial class JednostkiWindykWindow : ChildWindow
    {
        public int IdSprawy
        {
            get;
            set;
        }
        public JednostkiWindykWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewJednostki.JednostkiWindykacjiDomainDataSource.HasChanges == false)
            {
                this.DialogResult = false;
            }
            else
            {
                this.dialogbusyIndicator.IsBusy = true;
                this.ViewJednostki.JednostkiWindykacjiDomainDataSource.SubmittedChanges +=new EventHandler<Telerik.Windows.Controls.DomainServices.DomainServiceSubmittedChangesEventArgs>(JednostkiWindykacjiDomainDataSource_SubmittedChanges);             //new EventHandler<SubmittedChangesEventArgs>(JednostkiWindykacjiDomainDataSource_SubmittedChanges); 
                this.ViewJednostki.JednostkiWindykacjiDomainDataSource.SubmitChanges();

            }
        }

        private void JednostkiWindykacjiDomainDataSource_SubmittedChanges(object sender, Telerik.Windows.Controls.DomainServices.DomainServiceSubmittedChangesEventArgs e)
        {
            this.dialogbusyIndicator.IsBusy = false;
            if (e.HasError)
            {
                MessageBox.Show (e.Error.ToString()); 
            }

            this.DialogResult = true;

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewJednostki.JednostkiWindykacjiDomainDataSource.HasChanges == false)
            {

            }
            else
                this.ViewJednostki.JednostkiWindykacjiDomainDataSource.RejectChanges();
            this.DialogResult = false;
        }
    }
}

