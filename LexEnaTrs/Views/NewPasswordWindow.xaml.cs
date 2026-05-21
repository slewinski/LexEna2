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
    public partial class NewPasswordWindow: ChildWindow
    {

        public string pwd { get; set;}
     

        public NewPasswordWindow()
        {
            InitializeComponent();
        }

       

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.NewPassword.Text = pwd;
            
        }
    }
}

