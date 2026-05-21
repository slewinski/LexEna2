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
    public partial class GetDateWindow : ChildWindow
    {

        public DateTime? ChosenDate { get; set;}
        public string prompt { get; set; }

        public GetDateWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ChosenDate = this.DataDokumentu.SelectedValue;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if ( ChosenDate != null)
                this.DataDokumentu.SelectedValue = ChosenDate.Value;
            else
                this.DataDokumentu.SelectedValue = DateTime.Today;
            if (!String.IsNullOrWhiteSpace(prompt))
            {
                this.Title = prompt;

            }
        }
    }
}

