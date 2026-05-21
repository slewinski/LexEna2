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
    public partial class SelTypDokWindow : ChildWindow
    {
        public int TypDok { get; set; }
        public SelTypDokWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (GridViewSelTypDok.SelectedItems.Count > 0)
            {
                TypDok = (GridViewSelTypDok.SelectedItem as rodzajDokumentu).Numer;
                this.DialogResult = true;

            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

