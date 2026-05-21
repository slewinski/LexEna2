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
using Telerik.Windows.Controls.DomainServices;

namespace LexEnaTrs.Views
{

    
    public partial class ListaSprawById : UserControl
    {
        private int CurrentSprID;
        private List<int> IdList;
        private List<int> DistinctId;
        private int _totalItems;
        private pozewEventArgs evargs;
        private ObservableItemCollection<vw_ListaSpraw> searchresult;
        private QueryableCollectionView qcvListaSpraw; 
        
        public SearchFilter MySerachFilter = new SearchFilter();

     /*
        public EventHandler PageGetCompleted;


        protected virtual void OnPageGetCompleted(pozewEventArgs e)
            {
            if (PageGetCompleted != null)
                 PageGetCompleted(this, e);
            } 



        private void PageGetCompleted(object sender , object e)
        {
           if (this.ListaSprawSearch.)
            this.ListaSprawSearch.  
    
        }       
    
        */

        public ListaSprawById()
        {
            CurrentSprID = 0;
            InitializeComponent();
            IdList = new List<int>();
            DistinctId = new List<int>();
            searchresult = new ObservableItemCollection<vw_ListaSpraw>();
            qcvListaSpraw = new QueryableCollectionView(searchresult);
        }

        public void DoSerach()
        {
            
          
            
            DistinctId.Clear();
            IdList.Clear();
            searchresult.Clear();

            this.ListaSprawSearch.QueryParameters[0].Value = MySerachFilter.Sygnatura;
            this.ListaSprawSearch.QueryParameters[1].Value = MySerachFilter.Nazwa;
            this.ListaSprawSearch.QueryParameters[2].Value = MySerachFilter.SygnNCe;
            this.ListaSprawSearch.QueryParameters[3].Value = MySerachFilter.NrEwid;
            this.ListaSprawSearch.QueryParameters[4].Value = MySerachFilter.Opis;
            this.ListaSprawSearch.QueryParameters[5].Value = MySerachFilter.Ulica;
            this.ListaSprawSearch.QueryParameters[6].Value = MySerachFilter.Miejscowosc;
            this.ListaSprawSearch.QueryParameters[7].Value = MySerachFilter.Poczta;
            this.ListaSprawSearch.QueryParameters[8].Value = MySerachFilter.Pesel;
            this.ListaSprawSearch.Load();
            
      
            this.ListaSprawSearch.LoadedData  +=new EventHandler<Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs>(ListaSprawSearch_LoadedData);
               
          

            /*
        lstspr.SprawyGridView.ItemsSource = qcvListaSpraw;

            LexEnaMeritumDomainContext _context;  
            LoadOperation<vw_Search> loadop;
            
            List<int> IdList;
            List<int> IdDistinct;
            _context = new LexEnaMeritumDomainContext();
            
            if (searchresult == null)
            {
                searchresult = new ObservableCollection<vw_ListaSpraw>();
                qcvListaSpraw = new QueryableCollectionView(searchresult);
            }
            else
                searchresult.Clear();

            
            

            try
            {
                EntityQuery<vw_Search> query =
                    from c in _context.GetSearchSprQuery(searchfilter.Sygnatura, searchfilter.Nazwa, searchfilter.SygnNCe, searchfilter.NrEwid, searchfilter.Opis, searchfilter.Ulica, searchfilter.Miejscowosc, searchfilter.Poczta, searchfilter.Pesel)
                    select c;
                loadop = _context.Load(query);
              
                loadop.Completed += (se, ex) =>
                 {
                     IdList = new List<int>();
                     IdDistinct = new List<int>();
                     foreach (var r in loadop.Entities)
                     {
                         IdList.Add(r.id);
                         if (IdDistinct.Contains(r.id))
                         {
                             ;
                         }
                         else
                         {
                             searchresult.Add(new vw_ListaSpraw { id = r.id, Dluznik = r.Dluznik, Id_Jednostki = r.Id_Jednostki, Id_User = r.Id_User, Nazwa_Jednostki = r.Nazwa_Jednostki, Uzytkownik_Nazwa = r.Uzytkownik_Nazwa, sygnatura = r.sygnatura });
                             IdDistinct.Add(r.id);

                         }
                     }




                 };

            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex);
            }
        }
        }

        private void OpenSpr_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                SprawaWindow sprwindow = new SprawaWindow();
                sprwindow.ViewSprawa.IdSprawy = CurrentSprID;
                sprwindow.Show();
            }
            else
            {
                AlertMsg.Show("Wybierz sprawę do edycji");
            }

            */
        }


        private void ListaSprawSearch_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs ea)
        {
            //int pagecount;
            int? MyId;
            int i;
            vw_Search item;


            //pagecount = this.ListaSprawSearch.DataView.TotalItemCount / this.ListaSprawSearch.PageSize;
            foreach (var r in this.ListaSprawSearch.DataView)
            {
                item = r as vw_Search;
                MyId = item.id;
                if (!DistinctId.Contains((int)MyId))
                {
                    searchresult.Add(new vw_ListaSpraw { id = item.id,  Dluznik = item.Dluznik, Nazwa = item.NazwaStatusu, DataStatusu=item.DataStatusu, Id_Jednostki = item.Id_Jednostki, Id_User = item.Id_User, Nazwa_Jednostki = item.Nazwa_Jednostki, Uzytkownik_Nazwa = item.Uzytkownik_Nazwa, sygnatura = item.sygnatura, DataDekretJednostka = item.DataDekretJednostka });
                    DistinctId.Add((int)MyId);
                }

                IdList.Add((int)MyId);

            }

        }
            
      

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.SprawyGridView.ItemsSource = qcvListaSpraw;
        }

       

        private void OpenSpr_Click(object sender, RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                           
                if (UserProfile.Rola < 2)// jeśli nie administrator
                {
                    if (UserProfile.IdJednostki != (this.SprawyGridView.SelectedItem as vw_ListaSpraw).Id_Jednostki)
                    {
                        AlertMsg.Show("Próba odczytu sprawy z innej kancelarii ");
                        return;
                    
                    }
                
                }
                SprawaWindow sprwindow = new SprawaWindow();
                sprwindow.ViewSprawa.IdSprawy = CurrentSprID;
                sprwindow.ViewSprawa.Id_Jednostki = (this.SprawyGridView.SelectedItem as vw_ListaSpraw).Id_Jednostki;
                sprwindow.ViewSprawa.Id_User = (this.SprawyGridView.SelectedItem as vw_ListaSpraw).Id_User;
                sprwindow.Show();
            }
            else
            {
                AlertMsg.Show("Wybierz sprawę do edycji");
            }


        }

        private void RadDataPagerSprawy_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            if (e.NewPageIndex < this.RadDataPagerSprawy.PageCount) 
                        this.RadDataPagerSprawy.MoveToNextPage();
           


        }

        private void SprawyGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            vw_ListaSpraw lstSpr;

            if (e.AddedItems.Count > 0)
            {
                lstSpr = (vw_ListaSpraw)e.AddedItems[0];
                CurrentSprID = lstSpr.id;
            }


        }

        

        


     }
}
