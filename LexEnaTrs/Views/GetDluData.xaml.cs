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
    public partial class GetDluData : ChildWindow
    {

        public String Nazwa{ get; set; }
        public String Imie { get; set; }
        public String Pesel { get; set; }
        public String Nip { get; set; }
        public String NrKlienta { get; set; }
        public String Adres1 { get; set; }
        public String Adres2 { get; set; }
        public int SrcSystem { get; set; }

        public GetDluData()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
           
            Nazwa = this.tbNazwa.Text;
            Imie = this.tbImie.Text;
            Nip = this.tbNIP.Value;
            Pesel = this.tbPESEL.Value;
            NrKlienta = this.tbNrKlienta.Text;
            Adres1 = this.tbAdres1.Text;
            Adres2 = this.tbAdres2.Text;
            SrcSystem = (int)this.cbSrcSystem.SelectedValue ;

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.tbNazwa.Text = Nazwa;
            this.tbImie.Text= Imie ?? "" ;
            this.tbNIP.Value=Nip ??"";
            this.tbPESEL.Value = Pesel ?? "";
            this.tbNrKlienta.Text =NrKlienta ??"" ;
            this.tbAdres1.Text=Adres1 ??"" ;
            this.tbAdres2.Text= Adres2 ??"" ;
            this.cbSrcSystem.SelectedValue = SrcSystem;
        }
    }
}

