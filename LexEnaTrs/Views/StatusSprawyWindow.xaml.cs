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
using Telerik.Windows.Controls.DomainServices;

namespace LexEnaTrs.Views
{
    public partial class StatusSprawyWindow : ChildWindow
    {
        public StatusSprawyWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewStatusy.StatusySprawyDomainDataSource.HasChanges == false)
            {
                this.DialogResult = false;
            }
            else
            {
                this.dialogbusyIndicator.IsBusy = true;
                this.ViewStatusy.StatusySprawyDomainDataSource.SubmittedChanges+=new EventHandler<DomainServiceSubmittedChangesEventArgs>(StatusySprawyDomainDataSource_SubmittedChanges);  
                this.ViewStatusy.StatusySprawyDomainDataSource.SubmitChanges();
                
            }
			
        }

        private void StatusySprawyDomainDataSource_SubmittedChanges(object sender, DomainServiceSubmittedChangesEventArgs e)
        {
            this.dialogbusyIndicator.IsBusy = false;
            this.DialogResult = true;
        
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewStatusy.StatusySprawyDomainDataSource.HasChanges == false)
            {

            }
            else
            this.ViewStatusy.StatusySprawyDomainDataSource.RejectChanges();
            this.DialogResult = false;
        }
    }
}

