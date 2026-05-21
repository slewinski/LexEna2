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

namespace LexEnaTrs.Views
{
    public partial class WyborKontaJednostki : ChildWindow
    {
        public int IDKontaEPU
        {
            get;
            set;
        }
        
        public WyborKontaJednostki()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            KontoEPU kEPU;
            kEPU = (this.GridViewSelKontoEPU.SelectedItem as KontoEPU);
            if (kEPU != null)
            {
                this.IDKontaEPU = kEPU.Id;
                this.DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

       
        private void GridViewSelKontoEPU_Loaded(object sender, RoutedEventArgs e)
        {
             if (this.GridViewSelKontoEPU.Items.Count > 0 )
            this.GridViewSelKontoEPU.SelectedItem = this.GridViewSelKontoEPU.Items[0];

        }

        
    }
}

