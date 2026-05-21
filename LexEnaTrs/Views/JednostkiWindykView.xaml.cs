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
    public partial class JednostkiWindykView : UserControl
    {

        public int IdSprawy
        {
            get;
            set;
        }
        public JednostkiWindykView()
        {
            InitializeComponent();
        }

        private void DelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

       

        private void JednostkiView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IdSprawy > 0) //LexEnaMeritumDomainContext context = (LexEnaMeritumDomainContext)sprawaDomainDataSource.DataContext;
            {
                JednostkiWindykacjiDomainDataSource.QueryParameters[0].Value = IdSprawy;
                JednostkiWindykacjiDomainDataSource.Load();

            }
        }

       
        private void JednostkiWindykacjiDomainDataSource_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
        	if (e.HasError)
            {
                ErrorWindow.CreateNew(e.Error.ToString() + " Błąd odczytu danych");
                e.MarkErrorAsHandled();
            } 
        }

        private void gridViewJednostkiSprawy_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            // TODO: Add event handler implementation here.
            Dekretacja dekret;
            dekret = new Dekretacja();
            dekret.Sprawa_id = IdSprawy;
            dekret.Czyus = 0;
            dekret.DataDekretJednostka = DateTime.Now;
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
