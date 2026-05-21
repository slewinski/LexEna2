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

namespace LexEnaTrs.Views
{
    public partial class ReferentWindow : ChildWindow
    {
        public ReferentWindow()
        {
            InitializeComponent();
        }

        public int IdSprawy
        {

            get;
            set;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ReferentDomainDataSource.SubmitChanges();
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex);
            }
            this.DialogResult = true;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReferentDomainDataSource.RejectChanges();
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReferentDomainDataSource.QueryParameters[0].Value = IdSprawy;
            this.ReferentDomainDataSource.Load();

        }

        private void Adddekret_Click(object sender, RoutedEventArgs e)
        {
              // TODO: Add event handler implementation here.
           
      
        }

        private void Deldekret_Click(object sender, RoutedEventArgs e)
        {

        }

        private void gridViewJednostkiSprawy_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            Dekretacja dekret;
            dekret = new Dekretacja();
            dekret.Sprawa_id = IdSprawy;
            dekret.Czyus = 0;
            dekret.DataDekretUser = DateTime.Now;
            try
            {

                e.NewObject = dekret;
               
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex);
            }
        }

       
    }
}

