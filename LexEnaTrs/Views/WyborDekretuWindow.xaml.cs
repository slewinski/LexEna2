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
   

    public partial class WyborDekretuWindow : ChildWindow
    {
        public int IdJednostki
        {

            get;
            set;
        }

        public WyborDekretuWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.GridViewSelJednostka.Items.Count > 0 ) 
                this.IdJednostki = (this.GridViewSelJednostka.SelectedItem as JednostkaWindykacji).Id; 
                 
            
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

