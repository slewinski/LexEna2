using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using System.Windows.Media.Animation;
using System.Resources;
using System.Reflection;
using System.ServiceModel.Channels;
using LexEnaTrs.Views;
using LexEnaTrs.Web;
using System.ServiceModel.DomainServices.Client;
using System.Linq;
using System;

namespace LexEnaTrs
{

   

    public partial class MainView : UserControl
    {
        public TreeViewMenu menuItems;

        public FirmaViewModel ViewModel { get; set; }

        public ObservableCollection<vw_ListaSpraw> LstSpr;

        public MainView()
        {
            //LocalizationManager.DefaultResourceManager = new ResourceManager("Telerik.Windows.Examples.GridView.Localization.English", Assembly.GetCallingAssembly());
            
            
            //LocalizationManager.DefaultResourceManager = new ResourceManager("LexEnaTrs.Polish", Assembly.GetCallingAssembly());
            InitializeComponent();
         
          
            ((EnumDataSource)this.LayoutRoot.Resources["EnumTypDowodSource"]).EnumType = typeof(typRodzajDowodu);
            //ContactsListBox.ItemsSource = new Contacts();
            //MessagesTreeView.ItemsSource = new MailMessages();

             menuItems = new TreeViewMenu();
             this.outlookBar.DataContext = menuItems;
            MainWorkspaceWindowHandler.MenuClassHandler = menuItems;
            ViewModel = new FirmaViewModel();
            SlownikJednostekWindykacji.ReloadDictionary();
           // this.outlookBar.DataContext = ViewModel;


        }
     

        private void RadTreeViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            RadTreeViewItem firstItem = (sender as RadTreeViewItem).ItemContainerGenerator.ContainerFromIndex(0) as RadTreeViewItem;
            if (firstItem != null)
            {
                firstItem.IsSelected = true;
            }
        }

        private void ContactsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
           // if (this.businessCard != null)
          //  {
               // this.businessCard.SetDataContext(ContactsListBox.SelectedItem as Employee);
          //  }
             
        }

        private void RadTreeViewItem_Selected(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
	
        }

        private void Dekretacja_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            RadTreeView tv = (RadTreeView)sender;
            RadTreeViewItem tvi;
         
            ListaSprawGrid sprwref;
            ListaSprawGridDekretacja sprwrefDekret;
            LargeReportWindow lreportWin;

            if (e.AddedItems.Count == 1)
            {
                tvi = (RadTreeViewItem)e.AddedItems[0];
               
                switch ((string)tvi.Tag)
                {
                    case "DekretDojednostek":
                        sprwrefDekret = new ListaSprawGridDekretacja();
                        sprwrefDekret.IdUser = -1;  // dowolny
                        sprwrefDekret.Krok = -1;  // nowe zwieny - status
                        sprwrefDekret.IdStatusu = -1;  // obojętny
                        sprwrefDekret.IdJednostki = 0;
                        this.MainWorkspace.Child = sprwrefDekret; //"SprawyGrid";
                        break;
                    case "DekretDoReferentow":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = 0; //UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = -1;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";
                        break;
                    case "ZestawienieZwrot":
                      lreportWin = new LargeReportWindow();
                       
                        lreportWin.Mode = 4;
                        this.MainWorkspace.Child = lreportWin; //"Zwroty";
                        break;
                    default:
                        break;
                }
                }
            //switch(tv.SelectedItem.
        }

        
        private void LexEnaMainTreeView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            
            ListaSprawGrid sprwref;
            RadTreeView tv = (RadTreeView)sender;
            RadTreeViewItem tvi;
            
         
            
            if (e.AddedItems.Count == 1)
            {
                tvi = (RadTreeViewItem)e.AddedItems[0];
               
               
                switch ((string)tvi.Tag)
                {
                    case "NoweZWieny":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 1;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki =  UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";  
                        break;
                        /*
                    case "Pobrane":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = -1;  // pobrane
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                        */
                    case "ZPozwem":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 2;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";    
                        break;
                    case  "WEPU":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 3;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny    
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";  
                        break;
                    case "ZNakazemZaplaty":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 4;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";  
                        break;
                 case "TytulyWykonawcze":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId; // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 5;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";  
                        break;
                    case  "DoEgzekucji": 
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 10;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";  
                        break;
                    case  "Sprzeciw":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 11;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";  
                        break;
                    case "BrakPodstaw":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 12;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";  
                        break;
                    case "Umorzone":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 13;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";  
                        break;
                    case "Inne":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 14;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";  
                       
                        break;
                    case "Wycofano":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 15;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref ; //"SprawyGrid";  
                        break;

                    case "PozewPapierowy":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 16;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                    case "PozewpomorzeniuwEPU":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 17;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                    case "Zgloszonowierzytelnosc":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 18;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                    case "PoszukiwanieSpadkobiercow":
                        sprwref = new ListaSprawGrid();
                        sprwref.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.Krok = 19;  // nowe zwieny - status
                        sprwref.IdStatusu = -1;  // obojętny
                        sprwref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                    default:
                        break;
                }
                }

        }

        private void DokumntyWychodzace_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            ListaDokWysGrid sprwref;
            ListaPaczek paczref;
            ListaDokOdebrGrid docOdebrRef;
            ListaImportGrid importRef;
            ListaImportTradGrid importTradRef;

            RadTreeView tv = (RadTreeView)sender;
            RadTreeViewItem tvi;



            if (e.AddedItems.Count == 1)
            {
                tvi = (RadTreeViewItem)e.AddedItems[0];


                switch ((string)tvi.Tag)
                {
                    case "DokWn":
                        // import dokumentów z kancelarii i wniosków rgz
                        importRef = new ListaImportGrid();
                        importRef.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        importRef.IdJednostki = UserProfile.IdJednostki;
                        importRef.TypDok = 10;  // nowe wszytkie
                        importRef.StatusDok = 3;  // do projekty
                        this.MainWorkspace.Child = importRef; //"SprawyGrid";  
                      
                        break;
                    case "DokWnTrad":
                        // import dokumentów z kancelarii i wniosków rgz
                        importTradRef = new ListaImportTradGrid();
                        importTradRef.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        importTradRef.IdJednostki = UserProfile.IdJednostki;
                        importTradRef.TypDok = 10;  // nowe wszytkie
                        importTradRef.StatusDok = 3;  // do projekty
                        this.MainWorkspace.Child = importTradRef; //"SprawyGrid";  

                        break;
                    case "DokKom":
                        // import pozostałych 
                        break;
                    case "Pozwy":
                        sprwref = new ListaDokWysGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.IdJednostki = UserProfile.IdJednostki;
                        sprwref.TypDok = 10;  // nowe wszytkie
                        sprwref.StatusDok = 3;  // do projekty
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                    case "Inne":
                        sprwref = new ListaDokWysGrid();
                        sprwref.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.IdJednostki = UserProfile.IdJednostki;
                        sprwref.TypDok = -10;  // nowe wszytkie
                        sprwref.StatusDok = 3;  // do projekty
                        this.MainWorkspace.Child = sprwref;
                        sprwref.ImportXML.Visibility = Visibility.Visible;
                        break;
                    
                    case "ProjektyDok":
                        sprwref = new ListaDokWysGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.IdJednostki = UserProfile.IdJednostki;
                        sprwref.TypDok = -1;  // nowe wszytkie
                        sprwref.StatusDok = 1;  // do projekty
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                    case "DoWysylkiDok":
                        sprwref = new ListaDokWysGrid();
                        sprwref.IdUser = UserProfile.Rola >0 ? -1 : UserProfile.DbId; // zamienić na Id bieżącegu uzytkownika
                        sprwref.IdJednostki = UserProfile.IdJednostki;
                        sprwref.TypDok = -1;  // nowe wszytkie
                        sprwref.StatusDok = 2;  // do wysysłki
                        
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                    case "UsunieteDok":
                        sprwref = new ListaDokWysGrid();
                        sprwref.IdUser =  UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.IdJednostki = UserProfile.IdJednostki;
                        sprwref.TypDok = -1;  // nowe wszytkie
                        sprwref.StatusDok = 6;  // usuniete
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                    case "OdrzuconeDok":
                        sprwref = new ListaDokWysGrid();
                        sprwref.IdUser =  UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.IdJednostki = UserProfile.IdJednostki;
                        sprwref.TypDok = -1;  // nowe wszytkie
                        sprwref.StatusDok = 5;  // odrzucone
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                    case "ZwroconeDok":
                        sprwref = new ListaDokWysGrid();
                        sprwref.IdUser =  UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        sprwref.IdJednostki = UserProfile.IdJednostki;
                        sprwref.TypDok = -1;  // nowe wszytkie
                        sprwref.StatusDok = 4;  // zwrocone 
                        this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        break;
                        // Paczki
                    case "PaczkiProjekty" :
                        paczref = new ListaPaczek();
                        paczref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        paczref.TypDok = -1;  // nowe wszytkie
                        paczref.StatPaczki = 1;  // projekty              this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        this.MainWorkspace.Child = paczref;
                        break;
                    case "PaczkiDoWysylki" :
                        paczref = new ListaPaczek();
                        paczref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        paczref.TypDok = -1;  // nowe wszytkie
                        paczref.StatPaczki = 2;  // do wysyłki             this.MainWorkspace.Child = sprwref; //"SprawyGrid";  
                        this.MainWorkspace.Child = paczref;
                        break;
                    case "PaczkiWyslane" :
                        paczref = new ListaPaczek();
                        paczref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        paczref.TypDok = -1;  // nowe wszytkie
                        paczref.StatPaczki = 3;  // wyslane
                        this.MainWorkspace.Child = paczref;
                        break;
                    case "PaczkiOdrzucone" :
                        paczref = new ListaPaczek();
                        paczref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        paczref.TypDok = -1;  // nowe wszytkie
                        paczref.StatPaczki = 4;  // wyslane
                        this.MainWorkspace.Child = paczref;
                        break;
                    case "PaczkiRozpisane":
                        paczref = new ListaPaczek();
                        paczref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        paczref.TypDok = -1;  // nowe wszytkie
                        paczref.StatPaczki = 5;  // rozpisane
                        this.MainWorkspace.Child = paczref;
                        break; 
                    case "PaczkiUsuniete":
                        paczref = new ListaPaczek();
                        paczref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        paczref.TypDok = -1;  // nowe wszytkie
                        paczref.StatPaczki = 6;  // wyslane
                        this.MainWorkspace.Child = paczref;
                        break;

                    case "Nakazy":
                        docOdebrRef = new ListaDokOdebrGrid();
                        docOdebrRef.IdUser =  UserProfile.Rola >0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        docOdebrRef.IdJednostki = UserProfile.IdJednostki;
                        docOdebrRef.TypDok = 5;  // nowe wszytkie
                        this.MainWorkspace.Child = docOdebrRef; //"SprawyGrid";  
                        break;
                    case "Klauzule":
                        docOdebrRef = new ListaDokOdebrGrid();
                        docOdebrRef.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        docOdebrRef.IdJednostki = UserProfile.IdJednostki;
                        docOdebrRef.TypDok = 17;  // nowe wszytkie  klauzule wykonalości
                        this.MainWorkspace.Child = docOdebrRef; //"SprawyGrid";  
                        break;
                    case "InneDecyzje":
                        docOdebrRef = new ListaDokOdebrGrid();
                        docOdebrRef.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
                        docOdebrRef.IdJednostki = UserProfile.IdJednostki;
                        docOdebrRef.TypDok = 0;  // nowe wszytkie poza nakazami i klauzulami
                        this.MainWorkspace.Child = docOdebrRef; //"SprawyGrid";  
                        break;
                    default:
                        break;
                }
            }

        }

        private void outlookBar_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            ListaSprawById sprByIdref;

            RadOutlookBarItem selectedItem = outlookBar.SelectedItem as RadOutlookBarItem;
            this.MainWorkspace.Child = null;
            

            string tag = selectedItem.Tag as string;
            switch (tag)
            {
                case "Sprawy":
                    LexEnaMainTreeView.SelectedItem = null;
                    AllDocuments.SelectedItem = null;
                    break;

                case "Dokumenty":
                    AllDocuments.SelectedItem = null;
                    LexEnaMainTreeView.SelectedItem = null;
                    break;
                case "Wyszukiwanie":
                    sprByIdref = new ListaSprawById();
                    this.MainWorkspace.Child = sprByIdref;
                    MainWorkspaceWindowHandler.WinHandler =  sprByIdref;
                   
                    break;
                default:
                    break;

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MainWorkspaceWindowHandler.MainViewHandler = this; // handler do okna

        }
        

        public void SetUserRights()
        {
            if (UserProfile.Rola > 0)
            {
                this.DekretacjaBar.Visibility = Visibility.Visible;
                if (UserProfile.Rola == 2 || (UserProfile.Rola == 1 && UserProfile.CzyWlasna == 3 && UserProfile.Firma == -1))
                {
                    this.DekretDojednostek.Visibility = Visibility.Visible;
                    this.ZestawienieZwrot.Visibility = Visibility.Visible;
                }
                else
                {
                    this.DekretDojednostek.Visibility = Visibility.Collapsed;
                    this.ZestawienieZwrot.Visibility = Visibility.Collapsed;
                }
                if (UserProfile.Rola == 3)
                {
                    this.MojeSprawyBarItem.Visibility = Visibility.Collapsed;
                    this.MojeDokumentyBarItem.Visibility = Visibility.Collapsed;
                    this.MojeDoreczeniaBarItem.Visibility = Visibility.Collapsed;
                    this.CalendarBarItem.Visibility = Visibility.Collapsed;
                    this.ZdarzeniaBarItem.Visibility = Visibility.Collapsed;
                    this.SearchBarItem.Visibility = Visibility.Collapsed;
                    this.ReportsBar.Visibility = Visibility.Collapsed;
                    this.DekretacjaBar.Visibility = Visibility.Collapsed;
                    this.outlookBar.SelectedItem = this.KRD;
                }
                else
                    if ((UserProfile.Rola != 2 && UserProfile.Firma == 1 ) || (UserProfile.Rola == 0 &&  UserProfile.Firma == -1) )
                     {
                    this.KRD.Visibility = Visibility.Collapsed;
                    this.UZD.Visibility = Visibility.Collapsed;
                }

            }
            else
            {
                this.KRD.Visibility = Visibility.Collapsed;
                this.UZD.Visibility = Visibility.Collapsed;
                this.DekretacjaBar.Visibility = Visibility.Collapsed;
            }
            // inicjalizacja słownika Odsetek
            new TabOdsetek(); // napełnienia tabeli
        }

        private void LexEnaMainTreeView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MojeDokumentyBarItem_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void RefreshTreeView()
        {// Odświeża  
            LexEnaMeritumDomainContext _dbcontext;  //radaDomainDataSource.DomainContext;
            LoadOperation<vw_DokOdebrCount> loadop;
            int[,] myArray = new int[3,5];
         
            _dbcontext = new LexEnaMeritumDomainContext();
         

            EntityQuery<vw_DokOdebrCount> query =
                from c in _dbcontext.GetVw_DokOdebrCountQuery()
                select c;
            loadop = _dbcontext.Load(query);
            loadop.Completed += (sender, e) =>
            {
                 

                foreach (var r in loadop.Entities )
                {

                   
                    if (UserProfile.Rola == 0 && UserProfile.DbId != r.Id_User) continue;   // zwykły użytkownik nie w swoim referacie
                    if (UserProfile.Rola == 1 && UserProfile.IdJednostki != r.Id_Jednostki) continue;
                    switch (r.TypDok)
                    {
                        case 5:   // zakazy zapałty
                            myArray[0, 0] += (int)r.Ilosc;
                            switch (r.IsChecked)
                            {
                                case 0: //nowy
                                    myArray[0, 1] += (int)r.Ilosc;
                                    break;
                                case 1:  // przeczytany przeze mnie ale nie przez admina
                                    myArray[0, 2] += (int)r.Ilosc;
                                    break;
                                case 2:  // przeczytany przez admina i usera
                                    myArray[0, 3] += (int)r.Ilosc;
                                    break;
                                case 3:  // przeczytany tylko przez admina 
                                    if (UserProfile.Rola == 1)
                                    {// jeśli w moim refereacie i nie przeczytana
                                        if (r.Id_User == UserProfile.DbId) myArray[0, 4] += (int)r.Ilosc;
                                    }
                                    else
                                        myArray[0, 4] += (int)r.Ilosc;
                                    break;
                            }
                            break;
                        case 17:
                            myArray[1, 0] += (int)r.Ilosc;

                            switch (r.IsChecked)
                            {
                                case 0: //nowy
                                    myArray[1, 1] += (int)r.Ilosc;
                                    break;
                                case 1:  // przeczytany przeze mnie ale nie przez admina
                                    myArray[1, 2] += (int)r.Ilosc;
                                    break;
                                case 2:  // przeczytany przez admina i usera
                                    myArray[1, 3] += (int)r.Ilosc;
                                    break;
                                case 3:  // przeczytany tylko przez admina 
                                    myArray[1, 4] += (int)r.Ilosc;
                                    break;
                            }
                            break;

                        default:
                            myArray[2, 0] += (int)r.Ilosc;
                            switch (r.IsChecked)
                            {
                                case 0: //nowy
                                    myArray[2, 1] += (int)r.Ilosc;
                                    break;
                                case 1:  // przeczytany przeze mnie ale nie przez admina
                                    myArray[2, 2] += (int)r.Ilosc;
                                    break;
                                case 2:  // przeczytany przez admina i usera
                                    myArray[2, 3] += (int)r.Ilosc;
                                    break;
                                case 3:  // przeczytany tylko przez admina 
                                    myArray[2, 4] += (int)r.Ilosc;
                                    break;
                            }

                            break;


                    }

                }

                // Załadowanie danych do strunktury
                {
                    TreeViewMenu tvm;
                    tvm = (MainWorkspaceWindowHandler.MenuClassHandler as TreeViewMenu);
                    if (tvm != null)
                    {
                        tvm.NakazyAll = myArray[0, 0];
                        tvm.NakazyNew = myArray[0, 1]; 
                        tvm.NakazyUnReadOnlyMe = myArray[0, 4];

                        tvm.KlauzuleAll = myArray[1, 0];
                        tvm.KlauzuleNew = myArray[1, 1];
                        tvm.KlauzuleUnReadOnlyMe = myArray[1, 4];

                        tvm.InneAll = myArray[2, 0];
                        tvm.InneNew = myArray[2, 1];
                        tvm.InneUnReadOnlyMe = myArray[2, 4];



                    }
                }

            };

        }

        private void RadCalendar_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            ListaTerminow lstTrm = new ListaTerminow();

            lstTrm.IdUser = UserProfile.Rola > 0 ? -1 : UserProfile.DbId;  // zamienić na Id bieżącegu uzytkownika
            lstTrm.Status = 0;  // obojętny
            lstTrm.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
            lstTrm.dzien = (sender as Telerik.Windows.Controls.RadCalendar).SelectedDate.Value;
            this.MainWorkspace.Child = lstTrm; //"SprawyGrid";  

        }

        private void Reports_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            RadTreeView tv = (RadTreeView)sender;
            RadTreeViewItem tvi;
            ReportWindow repwin;
            LargeReportWindow lreportWin;

       

            if (e.AddedItems.Count == 1)
            {
                tvi = (RadTreeViewItem)e.AddedItems[0];

                switch ((string)tvi.Tag)
                {
                    case "TerminPozwu":
                        repwin = new ReportWindow();
                         repwin.DataOd = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                         repwin.DataDo = DateTime.Today;
                         repwin.Mode = 101;
                         repwin.Show();
                        this.MainWorkspace.Child = null; //"SprawyGrid";
                        break;
                    case "TerminNakaz":
                         repwin = new ReportWindow();
                         repwin.DataOd = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                         repwin.DataDo = DateTime.Today;
                         repwin.Mode = 102;
                         repwin.Show();
                        this.MainWorkspace.Child = null; //"SprawyGrid";
                        break;
                    case "TerminCzynn":
                        repwin = new ReportWindow();
                        repwin.DataOd = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        repwin.DataDo = DateTime.Today;
                        repwin.Mode = 1000;
                        repwin.Show();
                        this.MainWorkspace.Child = null; //"SprawyGrid";
                        break;
                    case "DatyDekret":
                        lreportWin = new LargeReportWindow();
                        //repwin.Mode = 10001;
                        //repwin.Show();
                        lreportWin.Mode = 1;
                        this.MainWorkspace.Child = lreportWin; //"Daty dekretacji";
                        break;
                    case "Wierzytelnosci":
                        lreportWin = new LargeReportWindow();
                        //repwin.Mode = 10001;
                        //repwin.Show();
                        lreportWin.Mode = 2;
                        this.MainWorkspace.Child = lreportWin; //"Daty dekretacji";
                        break;

                    default:
                        break;
                }
            }
        }

        private void Krd_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            RadTreeView tv = (RadTreeView)sender;
            RadTreeViewItem tvi;
            KrdNoweOperacje krdoprref;



            if (e.AddedItems.Count == 1)
            {
                tvi = (RadTreeViewItem)e.AddedItems[0];

                switch ((string)tvi.Tag)
                {
                    case "KRDNowe":
                        krdoprref = new KrdNoweOperacje();
                        krdoprref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        krdoprref.TypDok = -1;  // nowe wszytkie
                        this.MainWorkspace.Child = krdoprref;
                        break;
                    case "KRDDluznicy":
                        KrdDluznicy krddlu = new KrdDluznicy();
                        krddlu.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        krddlu.TypDok = -1;  // nowe wszytkie
                        this.MainWorkspace.Child = krddlu;
                        break;
                    case "KRDOperacje":
                        KrdOperacje krdopref = new KrdOperacje();
                        krdopref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        krdopref.TypDok = -1;  // nowe wszytkie
                        this.MainWorkspace.Child = krdopref;
                        break;
                    default:
                        break;
                }
            }

        }

       

        private void UZD_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            RadTreeViewItem tvi;
            UzDNoweOperacje uzdoprref;
            UzDWplaty uzdwpl;


            if (e.AddedItems.Count == 1)
            {
                tvi = (RadTreeViewItem)e.AddedItems[0];

                switch ((string)tvi.Tag)
                {
                    case "UzDNowe":

                        uzdoprref = new UzDNoweOperacje();
                        uzdoprref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        uzdoprref.TypDok = -1;  // nowe wszytkie
                        this.MainWorkspace.Child = uzdoprref;
                        break;
                    case "UzDDeklaracje":
                        uzdwpl = new UzDWplaty();
                        uzdwpl.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        uzdwpl.TypDok = -1;  // nowe wszytkie
                        this.MainWorkspace.Child = uzdwpl;
                        break;


                    case "KRDDluznicy":
                        KrdDluznicy krddlu = new KrdDluznicy();
                        krddlu.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        krddlu.TypDok = -1;  // nowe wszytkie
                        this.MainWorkspace.Child = krddlu;
                        break;
                    case "KRDOperacje":
                        KrdOperacje krdopref = new KrdOperacje();
                        krdopref.IdJednostki = UserProfile.Rola == 2 ? -1 : UserProfile.IdJednostki;
                        krdopref.TypDok = -1;  // nowe wszytkie
                        this.MainWorkspace.Child = krdopref;
                        break;
                    default:
                        break;
                }
            }
        }

        private void INNE_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            Tools1 toolsWin;
            ToolsExtractPdf toolsExtractWin;
            ToolsSkutKanc toolsSkutKancWin;
            ToolsNaleznosciSpraw toolsNalSpr;
            ToolsSaldoEgz toolsSaldoEgz;
            ToolsUmorzeniaKoszty toolsUmoKoszty;
            ImportBilling importBilling;
            MergeCases mergeCases;
            RadTreeView tv = (RadTreeView)sender;
            RadTreeViewItem tvi;
            WienaReports wReports;
            UmorzeniaZgony umoreport;
            ToolsImportZal importZal;
            LargeReportWindow lreportWin;
            if (e.AddedItems.Count == 1)
            {
                tvi = (RadTreeViewItem)e.AddedItems[0];

                switch ((string)tvi.Tag)
                {
                    case "SpadyEPU":
                        toolsWin = new Tools1();// 
                        this.MainWorkspace.Child = toolsWin;
                        break;
                    case "ExtractPdf":
                        toolsExtractWin = new ToolsExtractPdf();
                        this.MainWorkspace.Child = toolsExtractWin;
                        break;
                    case "SkutKanc":
                        toolsSkutKancWin = new ToolsSkutKanc();
                        this.MainWorkspace.Child = toolsSkutKancWin;
                        break;
                    case "NalSpr": // zestawienie należności w sprawach
                        toolsNalSpr = new ToolsNaleznosciSpraw();
                        this.MainWorkspace.Child = toolsNalSpr;
                        break;
                    case "PotwSald":
                        toolsSaldoEgz = new ToolsSaldoEgz();
                        this.MainWorkspace.Child = toolsSaldoEgz;
                        break;
                    case "ImportBilling":
                        importBilling = new ImportBilling();
                        this.MainWorkspace.Child = importBilling;
                        break;
                    case "ImportZaliczek":
                        importZal = new  ToolsImportZal();
                        this.MainWorkspace.Child = importZal;
                            ;
                        break;
                      case  "UsunKoszty":
                        toolsUmoKoszty = new ToolsUmorzeniaKoszty();
                        this.MainWorkspace.Child = toolsUmoKoszty;

                        break;
                    case "BezUz2":
                        lreportWin = new LargeReportWindow();
                        //repwin.Mode = 10001;
                        //repwin.Show();
                        lreportWin.Mode = 3;
                        this.MainWorkspace.Child = lreportWin; //"Daty dekretacji";
                        break;
                       
                    case "MergeCases":
                        mergeCases = new MergeCases();
                        this.MainWorkspace.Child = mergeCases;
                        break;
                    case "RaportKosztyRadcow":
                        wReports = new WienaReports();
                        wReports.TypRaport = 1; 
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "ObrotyDziennik":
                        lreportWin = new LargeReportWindow();
                        //repwin.Mode = 10001;
                        //repwin.Show();
                        lreportWin.Mode = 5;
                        this.MainWorkspace.Child = lreportWin; //"Obroty dziennika";
                        break;

                    case "RaportNaleznosci":
                        wReports = new WienaReports();
                        wReports.TypRaport = 2;
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "RaportRaty":
                        wReports = new WienaReports();
                        wReports.TypRaport = 3;
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "RaportZgony":
                        umoreport = new UmorzeniaZgony();
                        this.MainWorkspace.Child = umoreport;
                        break;
                    case "WiekowanieSald":
                        wReports = new WienaReports();
                        wReports.TypRaport = 5;
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "WiekowanieNal":
                        wReports = new WienaReports();
                        wReports.TypRaport = 7;
                        this.MainWorkspace.Child = wReports;
                        break;

                    case "RodzajeObciazen":
                        wReports = new WienaReports();
                        wReports.TypRaport = 8;
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "ZestObr":
                        wReports = new WienaReports();
                        wReports.TypRaport = 9;
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "ZestSaldaOryg":
                        wReports = new WienaReports();
                        wReports.TypRaport = 101;
                        this.MainWorkspace.Child = wReports;
                        break;

                    case "WiekowanieWplat":
                        wReports = new WienaReports();
                        wReports.TypRaport = 10;
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "RaportSprawyWgDok":
                        wReports = new WienaReports();
                        wReports.TypRaport = 6;
                        this.MainWorkspace.Child = wReports;
                        break;

                    case "BezskutEgzekucje":
                        wReports = new WienaReports();
                        wReports.TypRaport = 4;
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "WiekOS":
                        wReports = new WienaReports();
                        wReports.TypRaport = 11;   // wiekowanie należności  na koncie OS
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "WiekES":
                        wReports = new WienaReports();
                        wReports.TypRaport = 12;   // wiekowanie należności  na koncie ES
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "WiekNS":
                        wReports = new WienaReports();
                        wReports.TypRaport = 13;   // wiekowanie należności  na koncie NS
                        this.MainWorkspace.Child = wReports;
                        break;
                    case "WiekKS":
                        wReports = new WienaReports();
                        wReports.TypRaport = 14;   // wiekowanie należności  na koncie Egz
                        this.MainWorkspace.Child = wReports;
                        break;

                    default:
                        break;
                }
            }


           
            // obsługa 
        }
    }
}
