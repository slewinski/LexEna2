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

namespace LexEnaTrs.Views
{
    public partial class GetReturnReasonsWindow : ChildWindow
    {

        public int Przyczyna { get; set; }
        public string Opis { get; set; }

        public GetReturnReasonsWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (rdbOdpowiedz.SelectedIndex == -1) return;
            if (rdbOdpowiedz.SelectedIndex == rdbOdpowiedz.Items.Count -1  && String.IsNullOrWhiteSpace(tbPrzyczyna.Text)) // inna przyczyna
            {
                RadWindow.Alert("Wprowadź opis innej przyczyny.");
                return;
                
            }

            this.Przyczyna = rdbOdpowiedz.SelectedIndex;
            this.Opis = tbPrzyczyna.Text;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ;
           
        }

     

        private void BrakDok_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lbPrzyczynaTxt.Visibility = Visibility.Collapsed;
            tbPrzyczyna.Visibility = Visibility.Collapsed;
        }


        private void InnaPrzycz_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lbPrzyczynaTxt.Visibility = Visibility.Visible;
            tbPrzyczyna.Visibility = Visibility.Visible;
        }

        private void rdbOdpowiedz_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if ((e.AddedItems[0] as RadComboBoxItem).Name == "InnaPrzycz")
                {
                    lbPrzyczynaTxt.Visibility = Visibility.Visible;
                    tbPrzyczyna.Visibility = Visibility.Visible;

                }
            }
        }
    }
}

