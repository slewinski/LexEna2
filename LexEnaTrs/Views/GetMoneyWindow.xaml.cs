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
    public partial class GetMoneyWindow : ChildWindow
    {

        public Decimal? ChosenSaldo { get; set;}
        public DateTime? DataWymag { get; set; }
        public DateTime? DataWezw { get; set; }
        public String NrFaktury { get; set; } 

        public GetMoneyWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ChosenSaldo = this.Saldo.Value;
            DataWymag = this.rdDataWymag.SelectedValue;
            DataWezw= this.rdDataWezw.SelectedValue;
            NrFaktury = this.tbNumerFakt.Text;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Saldo.Value = ChosenSaldo;
            this.tbNumerFakt.Text = NrFaktury;
            this.rdDataWymag.SelectedValue = DataWymag;
            this.rdDataWezw.SelectedValue = DataWezw;
        }
    }
}

