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
    public partial class StatusyView : UserControl
    {
        public int IdSprawy
        {
            get;
            set;
        }
        public StatusyView()
        {
            InitializeComponent();
        }

       

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        	 if (IdSprawy > 0) //LexEnaMeritumDomainContext context = (LexEnaMeritumDomainContext)sprawaDomainDataSource.DataContext;
            {
                StatusySprawyDomainDataSource.QueryParameters[0].Value = IdSprawy;
                StatusySprawyDomainDataSource.Load();
                
            }
         
        }

       

      

        private void DelSatatus_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

        private void StatusySprawyDomainDataSource_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Błąd odczytu danych", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void gridViewStatusySprawy_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            StatusSprawy statspr;
            statspr = new StatusSprawy();
            statspr.Sprawa_id = IdSprawy;
            statspr.NazwaStatusu_Id = 1;
            statspr.DataStatusu = DateTime.Now;
            e.NewObject = statspr;
            statspr.czyus = 0;

                       
            }

      
    }
}
