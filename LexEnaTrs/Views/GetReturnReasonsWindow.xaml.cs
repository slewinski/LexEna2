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
using Telerik.Windows.Controls;

namespace LexEnaTrs.Views
{
    public partial class GetReturnReasonsWindow : ChildWindow
    {

        public int Przyczyna { get; set; }
        public string Opis { get; set; }
        
        public GetReturnReasonsWindow()
        {
            InitializeComponent();
        }
        private string getInnaPrzyczynaDescript()
        {   
            string s = "";
            this.Przyczyna = 0;
            if (cbFaktura.IsChecked == true) { s += ", Brak Faktury/noty"; Przyczyna += 1; }
            if (cbKalkulacjaKoszt.IsChecked == true)  { s += ", Brak Kalkulacji kosztów"; Przyczyna += 2; }
            if (cbOswiadczenie.IsChecked == true)  { s += ", Brak Oświadczenia o gotowości instalacji przyłączanej"; Przyczyna += 4; }
            if (cbPotwierdzenieWezwanie.IsChecked == true)  { s += ", Brak Potwierdzenia doręczenia wezwania"; Przyczyna += 8; }
            if (cbProtokol.IsChecked == true)  { s += ", Brak Protokołu"; Przyczyna += 16; }
            if (cbUmowa.IsChecked == true)  { s += ", Brak Umowy"; Przyczyna += 32; }
            if (cbUpowaznienie.IsChecked == true)  { s += ", Brak Upoważnienia do kontroli"; Przyczyna += 64; }
            if (cbWarunki.IsChecked == true)  { s += ", Brak Warunków przyłączenia"; Przyczyna += 128; }
            if (cbWezwanie.IsChecked == true)  { s += ", Brak Wezwania przedsądowego"; Przyczyna += 256; }
            if (cbWniosekPrzylaczenia.IsChecked == true)  { s += ", Brak Wniosku o określenie warunków przyłączenia"; Przyczyna +=512; }
            if (cbWyciag.IsChecked == true)  { s += ", Brak Wyciągu z KRS/CEIDG"; Przyczyna += 1024; }
            if (cbWypowiedzenie.IsChecked == true) { s+= ", Brak Wypowiedzenia umowy"; Przyczyna += 2048;}
            if (cbZlecenie.IsChecked == true)  { s += ", Brak Zlecenia OT"; Przyczyna += 4096; }
            if (cbInna.IsChecked == true)
            {
                s += ", Inna przyczyzna:";
                Przyczyna += 8192;
                s += tbPrzyczyna.Text;
            }
            if (s.Length > 1)
                s = s.Substring(1); // pomijamy pierwszy przecinek
            return s;
        } 

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (cbFaktura.IsChecked == false &&
                cbKalkulacjaKoszt.IsChecked == false &&
                cbOswiadczenie.IsChecked == false &&
                cbPotwierdzenieWezwanie.IsChecked == false &&
                cbProtokol.IsChecked == false &&
                cbUmowa.IsChecked == false &&
                cbUpowaznienie.IsChecked == false &&
                cbWarunki.IsChecked == false &&
                cbWezwanie.IsChecked == false &&
                cbWniosekPrzylaczenia.IsChecked == false &&
                cbWyciag.IsChecked == false &&
                cbWypowiedzenie.IsChecked == false &&
                cbZlecenie.IsChecked == false &&
                cbInna.IsChecked == false) return;

                if (cbInna.IsChecked == true && String.IsNullOrWhiteSpace(tbPrzyczyna.Text)) // inna przyczyna
                {
                    RadWindow.Alert("Wprowadź opis innej przyczyny.");
                    return;
                    
                }

            
            this.Opis = getInnaPrzyczynaDescript();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ;
           
        }

     

        private void BrakDok_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lbPrzyczynaTxt.Visibility = Visibility.Collapsed;
            tbPrzyczyna.Visibility = Visibility.Collapsed;
        }


    
        private void cbInna_Checked(object sender, RoutedEventArgs e)
        {
            if  (cbInna.IsChecked == true)
                tbPrzyczyna.Visibility = Visibility.Visible;
            else
                tbPrzyczyna.Visibility = Visibility.Collapsed;
        }
    }
}

