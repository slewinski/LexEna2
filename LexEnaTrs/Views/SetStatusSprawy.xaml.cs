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
using Telerik.Windows.Controls;
using System.ComponentModel;

namespace LexEnaTrs.Views
{
    



    public partial class SetStatusSprawy :ChildWindow
    {



        public DateTime DataZmianyStatusu
        {
            get;
            set;
        }

        public int? Status
        {
            get;
            set;
            
        }
        public int? ExtraStatus
        {
            get;
            set;

        }

        public SetStatusSprawy()
        {
            InitializeComponent();
            this.DataStatusu.SelectedDate = DateTime.Today;
        }

       

       

        private void OK_Click(object sender, RoutedEventArgs e)
        {

            DataZmianyStatusu = this.DataStatusu.SelectedValue.Value  ;
            if (this.MainStat.SelectedValue == null) return;
            Status = (int)this.MainStat.SelectedValue ;
            if (this.StatCombo.SelectedValue != null)

                ExtraStatus = (int?)this.StatCombo.SelectedValue;

            else
                ExtraStatus = 0; 
            this.DialogResult = true;
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RadComboBox_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            if (((sender as RadComboBox).SelectedValue as int?) == 13)
            {
                StatCombo.Visibility = Visibility.Visible;
                statLabel.Visibility = Visibility.Visible;



            }
            else
            {
                StatCombo.Visibility = Visibility.Collapsed;
                statLabel.Visibility = Visibility.Collapsed;
                ExtraStatus = 0;

            }
            
        }

        private void ChildWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

       

      

       
        

       

      
    }
}
