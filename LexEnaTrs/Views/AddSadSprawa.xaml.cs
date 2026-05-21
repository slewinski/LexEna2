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
using LexEnaTrs.Web;

namespace LexEnaTrs.Views
{
    public partial class AddSadSprawa : ChildWindow
    {
        public SadSprawa SadSpr {get; set;}

        public AddSadSprawa()
        {
            InitializeComponent();
          
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = SadSpr;
        }
    }
}

