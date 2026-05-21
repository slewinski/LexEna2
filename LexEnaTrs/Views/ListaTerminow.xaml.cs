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
using Telerik.Windows.Controls;
using System.Resources;
using System.Reflection;
using System.ServiceModel.DomainServices.Client;
using Telerik.Windows.Controls.GridView;

namespace LexEnaTrs.Views
{
    public partial class ListaTerminow : UserControl
    {
        private int CurrentSprID;
        // Parametry publiczne
        
        public int IdUser=-1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki=-1;
        public int Status = -1;
        public DateTime dzien = DateTime.Today;

        

        public ListaTerminow()
        {
                  

            InitializeComponent();

            
        }

      



        private void OpenSpr_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                SprawaWindow sprwindow = new SprawaWindow();
                sprwindow.ViewSprawa.IdSprawy = CurrentSprID;
                sprwindow.ViewSprawa.Id_Jednostki = (this.SprawyGridView.CurrentItem as vw_ListaSpraw).Id_Jednostki;
                sprwindow.ViewSprawa.Id_User = (this.SprawyGridView.CurrentItem as vw_ListaSpraw).Id_User;
                sprwindow.Show();
                sprwindow.Closed += (sen, args) =>
                    {
                        this.vw_ListaTerminowdds.Load();
                    };
            }
            else
            {
                AlertMsg.Show("Wybierz sprawę do edycji");
            }


        }
        private void DoDekretSpr(int ItemId, int context)
        {
            // context - 0 - do jednostki, 1 do referenta;

            LexEnaMeritumDomainContext _dekretcontext;  //radaDomainDataSource.DomainContext;
            _dekretcontext = new LexEnaMeritumDomainContext();

            foreach (var r in this.SprawyGridView.SelectedItems)
            {
                Dekretacja dekret = new Dekretacja();
                dekret.Czyus = 0;
                if (context==1 ) // do referenta
                {
                   dekret.DataDekretUser = DateTime.Now;
                   dekret.Uzytkownik_Id =  ItemId  ;
                }
                if (context == 0) // do jednostki
                {
                   dekret.DataDekretJednostka = DateTime.Now;
                   dekret.JednostkaWindykacji_Id =  ItemId  ;
                }
     
                dekret.Sprawa_id = (r as vw_ListaSpraw).id;
                _dekretcontext.Dekretacjas.Add(dekret);

            }
            _dekretcontext.SubmitChanges().Completed += (sender, e) =>
            { this.vw_ListaTerminowdds.Load(); };


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

      

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.vw_ListaTerminowdds.QueryParameters[0].Value  = IdUser;
            this.vw_ListaTerminowdds.QueryParameters[1].Value = IdJednostki;
            this.vw_ListaTerminowdds.QueryParameters[2].Value = Status;
            this.vw_ListaTerminowdds.QueryParameters[3].Value = dzien;

            this.vw_ListaTerminowdds.Load();
        }

        private void vw_ListaSprawdds_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Błąd odczytu danych", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
            
        }

        private void Dekret_Click(object sender, RoutedEventArgs e)
        {
            
            ChildWindow askDialog;
            int ItemId;
            if (this.IdJednostki != 0 )  // jeśli jest jednostka to dekret na człowieka
                askDialog = new WyborDekretuReferentWindow();
            else
                askDialog = new  WyborDekretuWindow();

            askDialog.Show();
            askDialog.Closed += (_sender, _e) =>
            {
                ItemId = this.IdJednostki != 0 ? (askDialog as WyborDekretuReferentWindow).IdJednostki: (askDialog as WyborDekretuWindow).IdJednostki;
                if (ItemId  > 0)
                {
                    DoDekretSpr(ItemId, this.IdJednostki != 0 ? 1 : 0);
                   
                }
                //  if ( (WyborDekretuWindow)_sender
            };

        }

       

        private void vw_ListaSprawdds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }

        private void SprawyGridView_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            var row = e.Row as GridViewRow;

            if (row != null)
            {

                row.Cells[1].Content = this.RadDataPagerSprawy.PageIndex * this.RadDataPagerSprawy.PageSize + this.SprawyGridView.Items.IndexOf(row.Item) + 1;
            }
        }

        

        private void MakeDocuments(int dokType)
        {// sporządzenie pzwów dla wybranych spraw.
            List<int> IdSpr = new List<int> ();  // tablica na Id spraw
            int tabindex = 0;
            int i;
            string lstSerialized;
  

            // Wczytanie słownika
            foreach (var r in this.SprawyGridView.SelectedItems)
            {
                IdSpr.Add ((r as vw_ListaSpraw).id);
                tabindex++;

            }
            


                if (tabindex > 0)
                {
                    this.BusyIndicator.IsBusy = true;
                    this.BusyIndicator.BusyContent = "Traw generacja dokumentu proszę czekać...";
                  //  GenerateDocuments(string listaIdSpr,int docType, int IdKontaEPU)
                    PozewDomainContext _pozewcontext = new PozewDomainContext();
                    lstSerialized = ToXMLSerializers.SerializeToString(IdSpr, typeof(List<int>));
                    _pozewcontext.GenerateDocuments(lstSerialized ,dokType,(int)UserProfile.IdKontaEPU, GenrationCompleted, null); 
                }
        }




        private void GenrationCompleted(InvokeOperation<string> result)
        {
            if (result.Value.ToLower().Contains("error") || result.Value.ToLower().Contains("błąd"))
            {
                ErrorWindow.CreateNew(result.Value);

            }
            this.BusyIndicator.IsBusy = false;
        
        }

            
       


        private void ListaAkcji_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int i;
            int dokType;
            this.MakeDok.IsOpen = false;

            if (e.AddedItems.Count > 0)
            {
                i = (sender as System.Windows.Controls.ListBox).SelectedIndex;
                (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;
                switch (i)
                {
                    case 0:
                        dokType = 3;  // pismo    //   Podgląd
                        break;
                    case 1:
                        dokType = 4;  // wniosek    //   Podgląd
                        break;
                    case 2:
                        dokType = 5;  //Uzupenienie adresu  //   Podgląd
                        break;
                    case 3:
                        dokType = 6;  //Uzupenienie braków
                        break;
                    case 4:
                        dokType = 13;  //Rezygnacjka z pełnomocnictwa
                        break;
                    case 5:
                        dokType = 14;  // Zgłoszneie pełnomocnika do sprawy
                        break;
                    default:
                        dokType = 3;
                   break;
                }
                MakeDocuments(dokType);
            }

        }


     }
}
