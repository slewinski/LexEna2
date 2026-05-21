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
using LexEnaTrs;
using System.ServiceModel.DomainServices.Client;
using System.Collections.ObjectModel;
using Telerik.Windows.Data;
using Telerik.Windows.Controls;

namespace LexEnaTrs.Views
{
    public class  SearchResults
    {
    public int Id {get; set;}
    public  string Sygnatura {get; set;}
    public string  SygnNce {get; set;}
    
    }

    public partial class SearchView : UserControl
    {
        private SearchFilter searchfilter;
        public ObservableCollection<vw_ListaSpraw> searchresult;
        public QueryableCollectionView qcvListaSpraw;




        public SearchView()
        {
            InitializeComponent();
            searchfilter = new SearchFilter();
            this.DataContext = searchfilter;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ListaSprawById  lstspr;
            lstspr = ((this.Parent as MainView).MainWorkspace.Child as ListaSprawById);
            if (lstspr == null)
                {
                    searchresult = new ObservableCollection<vw_ListaSpraw>();
                }
                 qcvListaSpraw = new QueryableCollectionView(searchresult);
                 lstspr.SprawyGridView.ItemsSource = qcvListaSpraw;


        }






        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            ListaSprawById lstspr;
            
            lstspr = (MainWorkspaceWindowHandler.WinHandler as ListaSprawById);
            lstspr.MySerachFilter = searchfilter;
            lstspr.DoSerach();

        }
    }
}
