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
    public partial class WyborStatusuPaczki : ChildWindow
    {
        public int selectedStatus
        {
            get;
            set;
        }

        public WyborStatusuPaczki()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.GridViewSelStatusPaczki.Items.Count > 0)
                this.selectedStatus = (this.GridViewSelStatusPaczki.SelectedItem as statusPaczki).Numer; 
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

