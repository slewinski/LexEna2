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
using Telerik.Windows.Data;

namespace LexEnaTrs.Views
{
    public partial class UpdateNalWindow : ChildWindow
    {
    
     
        public ItemsForLawsuit Items { get; set; }

        public UpdateNalWindow()
        {
            InitializeComponent();
            cbxRodzajeOds.ItemsSource = rodzajeOdsetek.GetRodzajOdsetek();
            cbxRodzajeOds.SelectedIndex = 0;

           


        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Items.TypOdsetek = (int)cbxRodzajeOds.SelectedValue;
            ParseListaDowodow(Items.DowodyLst);
            if ( RGVUzasadLst.SelectedItems.Count > 0)
            {
                ((TypDowod)RGVUzasadLst.SelectedItem).Choosen = true;
                    
            }
            Items.Czy40EURO = Add40Euro.IsChecked??false;
            this.DialogResult = true;


        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ParseListaDowodow( List<TypDowod> dLst )
        {

            string value;
            int nr = 0;
            foreach (TypDowod td in dLst)
            {
                if (!String.IsNullOrWhiteSpace(td.Opis) && td.Opis.Contains("<$DataDowodu$>") && td.DataDowodu.HasValue)
                {
                    td.Opis = td.Opis.Replace("<$DataDowodu$>", td.DataDowodu.Value.ToString("yyyy-MM-dd"));

                }
                if (!String.IsNullOrWhiteSpace(td.Nazwa) &&  td.Nazwa.Contains("<$DataDowodu$>") && td.DataDowodu.HasValue )
                {
                    td.Nazwa = td.Nazwa.Replace("<$DataDowodu$>", td.DataDowodu.Value.ToString("yyyy-MM-dd"));

                }

                if (!String.IsNullOrWhiteSpace(td.Opis) && td.Opis.Contains("<$NumerDowodu$>") &&  !String.IsNullOrWhiteSpace(td.Tekst) )
                {
                    td.Opis = td.Opis.Replace("<$NumerDowodu$>", td.Tekst);

                }

                if (!String.IsNullOrWhiteSpace(td.Nazwa) && td.Nazwa.Contains("<$NumerDowodu$>") && !String.IsNullOrWhiteSpace(td.Tekst))
                {
                    td.Nazwa = td.Nazwa.Replace("<$NumerDowodu$>", td.Tekst);

                }

               
                if (!String.IsNullOrWhiteSpace(td.Opis) && td.Opis.Contains("<$DataTabelaNBP$>") && td.DataDowodu.HasValue)
                {
                    td.Opis = td.Opis.Replace("<$DataTabelaNBP$>", td.DataDowodu.Value.ToString("yyyy-MM-dd"));
                    Items.DataNBP = td.DataDowodu.Value.ToString("yyyy-MM-dd");
                }
                if (!String.IsNullOrWhiteSpace(td.Nazwa) && td.Nazwa.Contains("<$DataTabelaNBP$>") && td.DataDowodu.HasValue)
                {
                    td.Nazwa = td.Nazwa.Replace("<$DataTabelaNBP$>", td.DataDowodu.Value.ToString("yyyy-MM-dd"));
                    Items.DataNBP = td.DataDowodu.Value.ToString("yyyy-MM-dd");
                }

                if (!String.IsNullOrWhiteSpace(td.Opis) && td.Opis.Contains("<$DataUmowy$>") && td.DataDowodu.HasValue)
                {
                    td.Opis = td.Opis.Replace("<$DataUmowy$>", td.DataDowodu.Value.ToString("yyyy-MM-dd"));

                }
                if (!String.IsNullOrWhiteSpace(td.Nazwa) && td.Nazwa.Contains("<$DataUmowy$>") && td.DataDowodu.HasValue)
                {
                    td.Nazwa = td.Nazwa.Replace("<$DataUmowy$>", td.DataDowodu.Value.ToString("yyyy-MM-dd"));

                }
                if (!String.IsNullOrWhiteSpace(td.Opis) && td.Opis.Contains("<$NumerUmowy$>") && !String.IsNullOrWhiteSpace(td.Tekst))
                {
                    td.Opis = td.Opis.Replace("<$NumerUmowy$>", td.Tekst);

                }

                if (!String.IsNullOrWhiteSpace(td.Nazwa) && td.Nazwa.Contains("<$NumerUmowy$>") && !String.IsNullOrWhiteSpace(td.Tekst))
                {
                    td.Nazwa = td.Nazwa.Replace("<$NumerUmowy$>", td.Tekst);

                }


            }
        }


        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

            this.RGVNalLst.ItemsSource = Items.NaleznosciLst.Where(a=>a.id > 0).ToList();
            this.RGVDowLst.ItemsSource = Items.DowodyLst;
            this.RGVUzasadLst.ItemsSource = Items.UzasadnieniaLst;

            // this.RGVDowLst.Gr CollapseGroup(this.RGVDowLst.Items.Groups[0] as IGroup);


        }

        private void RGVDowLst_Grouped(object sender, Telerik.Windows.Controls.GridViewGroupedEventArgs e)
        {
             this.RGVDowLst.CollapseGroup(this.RGVDowLst.Items.Groups[0] as IGroup);
        }

        private void RGVUzasadLst_Grouped(object sender, Telerik.Windows.Controls.GridViewGroupedEventArgs e)
        {
            this.RGVUzasadLst.CollapseGroup(this.RGVUzasadLst.Items.Groups[0] as IGroup);
        }
    }
}

