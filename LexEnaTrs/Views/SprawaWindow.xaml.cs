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

namespace LexEnaTrs.Views
{
    public partial class SprawaWindow : ChildWindow
    {
        public SprawaWindow()
        {
            InitializeComponent();
            this.ViewSprawa.SprWindHndl = this;
        }
        

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.ViewSprawa.UpdateAll();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }
    }
}

