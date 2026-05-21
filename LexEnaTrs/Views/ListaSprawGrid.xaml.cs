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
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace LexEnaTrs.Views
{



    public partial class ListaSprawGrid : UserControl
    {

        private class pozewDscriptor
        {
            public string pozew;
            public string sygnatura;


        }

        private int CurrentSprID;
        // Parametry publiczne

        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int IdStatusu = -1;
        public int Krok = -1;
        private List<Slownik> fakty;
        // private string[] pozwy; 
        private pozewDscriptor[] pozwy;
        private int strindex = 0;
        private DateTime theDay;
        private string lstSerialized;
        private int CreationMode = 0;  // sposób generaji 0 - bez kapitalizacji, 1 z akpitalizacją odsetek
        private ItemsForLawsuit xtraItems;
        private NalToCorrect odsKapital = null;
        private NalToCorrect koszty40E = null;

        public ListaSprawGrid()
        {
            CurrentSprID = 0;



            InitializeComponent();
            this.SprawyGridView.AddHandler(GridViewRow.MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseDoubleClick), true);
            if (UserProfile.Firma == 1)
            {
                ((System.Windows.Controls.ListBoxItem)(PozewOpcje.Items[1])).Visibility = Visibility.Collapsed;

            }
        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && ((UIElement)e.OriginalSource).ParentOfType<GridViewCell>() != null)
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
                        this.vw_ListaSprawdds.Load();
                    };
                }
                else
                {
                    AlertMsg.Show("Wybierz sprawę do edycji");
                }

            }
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
                        this.vw_ListaSprawdds.Load();
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
            List<int> idLst;
            LexEnaMeritumDomainContext _dekretcontext;  //radaDomainDataSource.DomainContext;
            _dekretcontext = new LexEnaMeritumDomainContext();
            idLst = new List<int>();

            foreach (var r in this.SprawyGridView.SelectedItems)
            {
                Dekretacja dekret = new Dekretacja();
                dekret.Czyus = 0;
                if (context == 1) // do referenta
                {
                    dekret.DataDekretUser = DateTime.Now;
                    dekret.Uzytkownik_Id = ItemId;
                }
                if (context == 0) // do jednostki
                {
                    dekret.DataDekretJednostka = DateTime.Now;
                    dekret.JednostkaWindykacji_Id = ItemId;

                }

                dekret.Sprawa_id = (r as vw_ListaSpraw).id;
                _dekretcontext.Dekretacjas.Add(dekret);
                idLst.Add(dekret.Sprawa_id);
            }
            _dekretcontext.SubmitChanges().Completed += (sender, e) =>
            {
                if (context == 0 && UserProfile.Firma == -1)
                {
                    PozewDomainContext domContext = new PozewDomainContext();
                    domContext.AttachDocuments(ToXMLSerializers.SerializeToString(idLst, typeof(List<int>)), UserProfile.Firma, AttachDocumentsCompleted, null);


                }
                this.vw_ListaSprawdds.Load();


            };


        }
        private void AttachDocumentsCompleted(InvokeOperation<string> errcode)
        {

            if (errcode.HasError)
            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew("Błąd podczas dołączania dokumentów");
                this.BusyIndicator.IsBusy = false;
                return;
            }
            else
            {
                if (errcode.Value.StartsWith("Błąd"))
                {
                    ErrorWindow.CreateNew(errcode.Value);
                    this.BusyIndicator.IsBusy = false;
                    return;
                }
                this.BusyIndicator.IsBusy = false;

                // call 
            }
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
            this.vw_ListaSprawdds.QueryParameters[0].Value = IdUser;
            if (IdJednostki == 0)
                this.vw_ListaSprawdds.QueryParameters[1].Value = IdJednostki;
            else
                if (UserProfile.Rola == 2)
                this.vw_ListaSprawdds.QueryParameters[1].Value = -1;
            else
                this.vw_ListaSprawdds.QueryParameters[1].Value = IdJednostki;

            this.vw_ListaSprawdds.QueryParameters[2].Value = IdStatusu;
            this.vw_ListaSprawdds.QueryParameters[3].Value = Krok;
            if (IdUser > 0)   // sprawa jest zadekretowana 
            {
                this.Dekret.Visibility = Visibility.Collapsed;
                if (Krok == 1)
                    this.PozewOpcje.Visibility = Visibility.Visible;
                else
                    this.PozewOpcje.Visibility = Visibility.Collapsed;
            }
            else
                if (Krok != 1)
                this.PozewOpcje.Visibility = Visibility.Collapsed;
            else
                this.PozewOpcje.Visibility = Visibility.Visible;

            // Tu ustaiwć widoczność poszczególnych kolumn this.SprawyGridView.Columns[""]
            this.vw_ListaSprawdds.Load();
        }

        private void vw_ListaSprawdds_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Błąd odczytu danych", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }

        }

        private void Dekret_Click()
        {

            ChildWindow askDialog;
            int ItemId;
            if (this.IdJednostki != 0)  // jeśli jest jednostka to dekret na człowieka
                askDialog = new WyborDekretuReferentWindow();
            else
                askDialog = new WyborDekretuWindow();

            askDialog.Show();
            askDialog.Closed += (_sender, _e) =>
            {
                ItemId = this.IdJednostki != 0 ? (askDialog as WyborDekretuReferentWindow).IdJednostki : (askDialog as WyborDekretuWindow).IdJednostki;
                if (ItemId > 0)
                {
                    DoDekretSpr(ItemId, this.IdJednostki != 0 ? 1 : 0);

                }
                //  if ( (WyborDekretuWindow)_sender
            };

        }
        private void PrzeliczZaleglosc()
        {
            // funkcja przelicza zaległosć w sprawach na liście dla zadanego dnia 
            // TODO: Add event handler implementation here.
            PozewDomainContext thecontext;
            thecontext = new PozewDomainContext();

            this.BusyIndicator.IsBusy = true;
            this.BusyIndicator.BusyContent = "Trwa obliczanie stanow zaległości proszę czekać...";
            thecontext.LiczZalegloscBySprList(lstSerialized, theDay.AddDays(-1), LiczZalCompleted, null);

        }


        private void LiczZalCompleted(InvokeOperation<int> errcode)
        {




            if (errcode.HasError)
            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew("Błąd w procedurze obliczania zaległości");
            }
            else
            {

                this.BusyIndicator.IsBusy = false;
                BudujPozwy();
                // call 
            }
        }





        private void DoPozwy()
        {
            int[] IdSpr = new int[100];  // tablica na Id spraw
            int tabindex = 0;
            List<int> listIdSpr;
            this.xtraItems = null;
            // pobierz datę 
            GetDateWindow wnd = new GetDateWindow();
            wnd.Title = "Podaj datę złożenia pozwu";
            wnd.Show();

            wnd.Closed += (se, eve) =>
                {
                    if (wnd.DialogResult == true)
                    {
                        DateTime? newDate = wnd.ChosenDate;
                        if (newDate != null)
                        {


                            try
                            {
                                this.theDay = Convert.ToDateTime(newDate);
                                this.PozewOpcje.IsEnabled = false;
                                listIdSpr = new List<int>();

                                foreach (var r in this.SprawyGridView.SelectedItems)
                                {

                                    listIdSpr.Add((r as vw_ListaSpraw).id);
                                    if (++tabindex >= 100) break;
                                }
                                // przeliczenie stanów zaległosci

                                lstSerialized = ToXMLSerializers.SerializeToString(listIdSpr, typeof(List<int>));
                                PozewDomainContext thecontext;
                                thecontext = new PozewDomainContext();
                                thecontext.UpdateNal(lstSerialized, this.CreationMode, this.theDay, UpdateNalCompleted, null);



                            }


                            catch (Exception ex)
                            {
                                this.PozewOpcje.IsEnabled = true;
                                ErrorWindow.CreateNew(ex);
                            }

                        }
                    }
                };
        }





        private void UpdateNalCompleted(InvokeOperation<string> errcode)
        {

            if (errcode.HasError)
            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew("Błąd w procedurze aktualizacji dat należności" + errcode.Value);
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(errcode.Value) && errcode.Value.StartsWith("Błąd"))
                {
                    this.PozewOpcje.IsEnabled = true;
                    ErrorWindow.CreateNew("Błąd w procedurze aktualizacji dat należności" + errcode.Value);
                    return;
                }
                if (!String.IsNullOrWhiteSpace(errcode.Value))
                    try
                    {
                        this.odsKapital = null;
                        ItemsForLawsuit items = (ItemsForLawsuit)ToXMLSerializers.XmlDeserializeFromString(errcode.Value, typeof(ItemsForLawsuit));
                        if (this.CreationMode > 0 && items != null && items.NaleznosciLst != null && items.NaleznosciLst.Any())
                        {
                            this.odsKapital = items.NaleznosciLst.Where(a => a.id == 0).FirstOrDefault();

                            if (this.odsKapital != null)
                            {
                                items.NaleznosciLst.Remove(odsKapital);
                            }

                            this.koszty40E = items.NaleznosciLst.Where(a => a.id == -1).FirstOrDefault();

                            if (this.koszty40E != null)
                            {
                                items.NaleznosciLst.Remove(koszty40E);
                            }

                        }

                        UpdateNalWindow nalWindow = new UpdateNalWindow();
                        nalWindow.Items = items;
                        nalWindow.Show();

                        nalWindow.Closed += (sndr, ex) =>
                        {
                            if (nalWindow.DialogResult == true)
                            {
                                PozewDomainContext thecontext;
                                thecontext = new PozewDomainContext();
                             

                                this.xtraItems = items;
                                
                                if (!items.Czy40EURO)
                                {
                                    this.koszty40E = null;

                                }
                                thecontext.UpdateDataDokumentuNal(ToXMLSerializers.SerializeToString(items, typeof(ItemsForLawsuit)), UpdateNalDateCompleted, null);

                                ;
                            }
                        };
                        return;
                    }
                    catch (Exception e)
                    {
                        this.PozewOpcje.IsEnabled = true;
                        ErrorWindow.CreateNew(e);
                        return;
                    }
                try
                {
                    // if (CreationMode > 0)
                    //     PrzeliczZaleglosc();
                    // else

                    BudujPozwy();
                    // call
                }
                catch (Exception ex)
                {
                    this.PozewOpcje.IsEnabled = true;
                    ErrorWindow.CreateNew(ex);
                }

            }
        }
        private void UpdateNalDateCompleted(InvokeOperation<string> errcode)
        {

            if (errcode.HasError)
            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew("Błąd w procedurze aktualizacji dat należności" + errcode.Value);
            }
            else
            {
                try
                {
                    // if (CreationMode > 0)
                    //     PrzeliczZaleglosc();
                    // else
                    //
                  

                    BudujPozwy();
                    // call
                }
                catch (Exception ex)
                {
                    this.PozewOpcje.IsEnabled = true;
                    ErrorWindow.CreateNew(ex);
                }
            }
        }

        private void OpenPozew(Pozew poz)
        {

            LexEnaMeritumDomainContext _context = new LexEnaMeritumDomainContext();

            EntityQuery<DokWys> query;
            query = from c in _context.GetDokWysWithPozewByIdQuery(poz.DokWys_Id.Value)
                    select c;
            LoadOperation<DokWys> loadop = _context.Load(query);
            loadop.Completed += (sd, ea) =>
            {
                DokWys dok;

                dok = loadop.Entities.FirstOrDefault();
                if (dok.Pozew != null)
                {
                    poz = dok.Pozew.FirstOrDefault();


                    PozewViewWindow pozwin;

                    if (poz != null)
                    {
                        if (poz.Tresc.Length > 10)
                        {
                            pozwin = new PozewViewWindow();
                            pozwin.ViewDokEPU.Visibility = Visibility.Collapsed;
                            pozwin.ViewDokEPU = null;
                            pozwin.ViewPozew.Visibility = Visibility.Visible;
                            pozwin.Show();  // 
                            pozwin.ViewPozew.docStatus = poz.DokWys.StatusDok;
                            pozwin.ViewPozew.IdDoc = poz.DokWys_Id.Value;
                            pozwin.ViewPozew.pozewSerialized = poz.Tresc;
                            pozwin.ViewPozew.odsNaliczSerialized = poz.DokWys.OdsNalicz;
                            pozwin.ViewPozew.OdsNalicz = Convert.ToDecimal(poz.DokWys.OdsetkiKapital);
                            pozwin.Closed += (sen, ex) =>
                            {
                                if (pozwin.DialogResult == true)
                                {
                                    this.BusyIndicator.IsBusy = true;
                                    poz.Tresc = pozwin.ViewPozew.pozewSerialized;
                                    poz.DokWys.Tresc = pozwin.ViewPozew.pozewSerialized;
                                    poz.DokWys.Koszty = pozwin.ViewPozew.KosztySadowe;
                                    poz.DokWys.WPS = pozwin.ViewPozew.WartPrzedmSporu;
                                    poz.DokWys.NotyOdsetkowe = pozwin.ViewPozew.NotyOdsetkowe;
                                    poz.DokWys.OdsetkiKapital = pozwin.ViewPozew.OdsNalicz;
                                    poz.DokWys.OdsNalicz = pozwin.ViewPozew.odsNaliczSerialized;
                                    poz.DokWys.DataDok = pozwin.ViewPozew.dWniesienia;
                                    poz.DokWys.Kzp = pozwin.ViewPozew.Kzp;
                                    poz.DokWys.InneKoszty = pozwin.ViewPozew.KosztyInne;
                                    poz.DokWys.Sprawa.KosztyZadane = pozwin.ViewPozew.KosztySadowe;
                                    poz.DokWys.Sprawa.KzpZadane = pozwin.ViewPozew.Kzp;
                                    poz.DokWys.Sprawa.InneZadane = pozwin.ViewPozew.KosztyInne;

                                    if (pozwin.ViewPozew.statusChanged)
                                    {
                                        poz.DokWys.StatusDok = pozwin.ViewPozew.docStatus;

                                    }
                                    _context.SubmitChanges().Completed += (obj, args) =>
                                    {


                                        this.vw_ListaSprawdds.Load();
                                        this.BusyIndicator.IsBusy = false;
                                    };
                                }
                            };

                        }
                    }

                }
            };

        }



        private void BudujPozwy()
        {

            List<int> listId;
            listId = new List<int>();
            listId = (List<int>)ToXMLSerializers.XmlDeserializeFromString(lstSerialized, typeof(List<int>));

            SlownikFaktow slfakt = new SlownikFaktow();
            fakty = slfakt.faktSlownik;
            slfakt.faktyCompeted += (obj, exp) =>
            {


                if (listId.Count > 0)
                {
                    this.BusyIndicator.IsBusy = true;
                    this.BusyIndicator.BusyContent = "Trwa generacja pozwów proszę czekać...";
                    foreach (int i in listId)
                    {
                        PozewExtend pozextd = new PozewExtend();
                        pozextd.generatemode = 0; // tryb generacji 
                        pozextd.dZlozenia = theDay;
                        pozextd.IdSprawy = i;
                        pozextd.fakty = fakty;
                        pozextd.extraData = this.xtraItems;
                        pozextd.CreationMode = CreationMode;
                        pozextd.PozewFromSprawa(this.odsKapital, this.koszty40E);
                        pozextd.pozewCompleted += (sndr, ex) =>
                        {
                            if ((ex as pozewEventArgs).Status < 0) { this.PozewOpcje.IsEnabled = true; this.BusyIndicator.IsBusy = false; return; } // bład ocdczytu pozwu
                            //spozew = pozextd.GetPozewSerialized();
                            //if (spozew != null) ;
                            pozextd.InsertPozew();
                            pozextd.pozewInserted += (s, ea) =>
                            {
                                if ((ex as pozewEventArgs).Status < 0)
                                {
                                    this.PozewOpcje.IsEnabled = true;
                                    this.BusyIndicator.IsBusy = false;
                                    ErrorWindow.CreateNew((ex as pozewEventArgs).Message);

                                }
                                else
                                {
                                    if (i == listId.Last<int>())
                                    {
                                        this.BusyIndicator.IsBusy = false;
                                        this.vw_ListaSprawdds.Load();
                                        this.PozewOpcje.IsEnabled = true;
                                        OpenPozew((ex as pozewEventArgs).MyPozew);
                                    }

                                }


                            };

                        };
                    }

                }

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
            List<int> IdSpr = new List<int>();  // tablica na Id spraw
            int tabindex = 0;
            int i;
            string lstSerialized;



            // Wczytanie słownika
            foreach (var r in this.SprawyGridView.SelectedItems)
            {
                IdSpr.Add((r as vw_ListaSpraw).id);
                tabindex++;

            }



            if (tabindex > 0)
            {
                this.BusyIndicator.IsBusy = true;
                this.BusyIndicator.BusyContent = "Trwa generacja dokumentu proszę czekać...";
                //  GenerateDocuments(string listaIdSpr,int docType, int IdKontaEPU)
                PozewDomainContext _pozewcontext = new PozewDomainContext();
                lstSerialized = ToXMLSerializers.SerializeToString(IdSpr, typeof(List<int>));
                _pozewcontext.GenerateDocuments(lstSerialized, dokType, (int)UserProfile.IdKontaEPU, GenrationCompleted, null);
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

        /*
         * 
         * private void PokazButtonClicked()
        {
            List<int> IdLst = new List<int>();
            int dokTyp = -1;
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                if (saveActionType == 0)
                {
                    // podgląd 
                }
                else
                {
                    foreach (vw_ListaDokWys dokP in this.SprawyGridView.SelectedItems)
                    {
                        IdLst.Add(dokP.Id);
                        if (dokTyp > 0)
                        {
                           if (dokTyp != dokP.TypDok)
                            {
                                ErrorWindow.CreateNew(" Dokumenty muszą być tego samego typu ");
                                return;
                            }
 
                        }
                        dokTyp = dokP.TypDok;
                    }
                    PozewDomainContext _pozewcontext;
                    _pozewcontext = new PozewDomainContext();
                    _pozewcontext.ListaDok2HTML(ToXMLSerializers.SerializeToString(IdLst, typeof(List<int>)), dokTyp, PozewToSaveCompleted, null);

                }
            }


        }

        private void PozewToSaveCompleted(InvokeOperation<string> html)
        {

            string htmlpath;

            htmlpath = html.Value;//e.Result.ToString();
            Uri uri = new Uri(Application.Current.Host.Source, htmlpath);
            if (uri.AbsoluteUri.Length < 5)
            {
                ErrorWindow.CreateNew("Błąd generacji zbioru ");
                return;
            }
            DownloadManager dwnMgr = new DownloadManager();
            dwnMgr.downloadButton = this.ShowDocs;
            dwnMgr.ServerFileUri = uri.AbsoluteUri;
            dwnMgr.DownloadnSave(1);


        }
         * 
         * 
         * */
        /*
        private void SaveDirtyPozew()
        { 
        // zapisz pozwy w   
            StringBuilder paczka = new StringBuilder();
            int i ;
            paczka.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            paczka.Append("<curr:Pozwy OznaczeniePaczki=\"");
            paczka.Append("Pozwy "+ DateTime.Today.ToString("yyyy-MM-dd"));
            paczka.Append(" \" xmlns:curr=\"http://www.e-sad.gov.pl/epu\">");

            
            for (i = 0; i < strindex;i++ )
            {
                StringBuilder sb = new StringBuilder();
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                                      
using (XmlWriter xw = XmlWriter.Create(sb, xws)) {
    );



    root.Save(xw);
}
Console.WriteLine(sb.ToString());


        */
        /*
        private void SaveDirtyPozew()
        {
            int i;
            StringBuilder paczka = new StringBuilder();
            paczka.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            paczka.Append("<curr:Pozwy OznaczeniePaczki=\"");
            paczka.Append("Pozwy " + DateTime.Today.ToString("yyyy-MM-dd"));
            paczka.Append(" \" xmlns:curr=\"http://www.e-sad.gov.pl/epu\">");
            for (i = 0; i < strindex; i++)
            {
                XDocument pozewXML = XDocument.Parse(pozwy[i]);
                StringBuilder outputsb = new StringBuilder();
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                xws.Encoding = Encoding.UTF8;
                XmlWriter xw = XmlWriter.Create(outputsb, xws);
                pozewXML.Save(xw);
                
                paczka.Append(outputsb.ToString());

            }
            paczka.AppendLine("</curr:Pozwy>");
            DaneDoEksportuReady wnd = new DaneDoEksportuReady();
            wnd.Show();
            wnd.Closed += (obj, e) =>
            {

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Dokumenty XML | *.xml";
                sfd.DefaultExt = "xml";
                bool? isSaveDialogShown = sfd.ShowDialog();
                if (isSaveDialogShown == true)
                {
                    using (Stream fs = (Stream)sfd.OpenFile())
                    {
                        using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                        {
                            writer.Write(paczka);
                        }
                    }

                }
            };
        }
        */



        private void ExportFileCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }

        }

        private impDescr toimpDescr(impDescriptor ids)
        {
            impDescr outId = new impDescr();
            if (ids.imp != null)
            {
                outId.ContentType = ids.imp.ContentType.Value;
                outId.DataTransferu = ids.imp.DataTransferu;
                outId.ImpExp = ids.imp.ImpExp.Value;
                outId.OpisOperacji = ids.imp.OpisOperacji;
                outId.StatusOperacji = ids.imp.StatusOperacji;

            }
            if (ids.impdet != null)
            {
                outId.impdt = new List<Web.impdet>();
                foreach (ImportDetail i in ids.impdet)
                {
                    impdet o = new impdet();
                    o.Code = i.Code == null ? 0 : i.Code.Value;
                    o.DataImportu = i.DataImportu;
                    o.Sygnatura = i.Sygnatura;
                    o.ErrLevel = i.ErrLevel == null ? 0 : i.ErrLevel.Value;
                    o.ErrDescription = i.ErrDescription;
                    outId.impdt.Add(o);

                }

            }

            return outId;
        }

        private void NotifyExportPozew(impDescriptor impD)
        {
            PozewDomainContext _pozewcontext;

            // Set Filter to browser text files
            _pozewcontext = new PozewDomainContext();
            _pozewcontext.ExportDocument(ToXMLSerializers.SerializeToString(toimpDescr(impD), typeof(impDescr)), UserProfile.Nazwisko + " " + UserProfile.Imie, UserProfile.IdJednostki, ExportFileCompleted, null);

        }








        private void SaveDirtyPozews()
        {
            int i;
            impDescriptor idesc;

            StringBuilder paczka = new StringBuilder();
            idesc = new impDescriptor();
            idesc.imp = new Import();
            idesc.impdet = new List<ImportDetail>();
            idesc.imp.DataTransferu = DateTime.Now;
            idesc.imp.ContentType = 10;  // pozew
            idesc.imp.FileType = 1; // xml;
            idesc.imp.ImpExp = 1; // Export
            idesc.imp.JednostkaWindykacji_Id = UserProfile.IdJednostki;


            paczka.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            paczka.Append("<curr:Pozwy OznaczeniePaczki=\"");
            paczka.Append("Pozwy " + DateTime.Today.ToString("yyyy-MM-dd"));
            paczka.Append(" \" xmlns:curr=\"" + Constants.currnamespace + "\">");


            for (i = 0; i < strindex; i++)
            {
                ImportDetail idet = new ImportDetail();

                idet.Sygnatura = pozwy[i].sygnatura;
                idesc.impdet.Add(idet);
                paczka.Append(pozwy[i].pozew);


            }
            paczka.AppendLine("</curr:Pozwy>");

            DaneDoEksportuReady wnd = new DaneDoEksportuReady();
            wnd.Show();
            wnd.Closed += (obj, e) =>
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Dokumenty XML | *.xml";
                sfd.DefaultExt = "xml";
                bool? isSaveDialogShown = sfd.ShowDialog();
                if (isSaveDialogShown == true)
                {
                    using (Stream fs = (Stream)sfd.OpenFile())
                    {
                        using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                        {
                            writer.Write(paczka);
                        }
                    }

                }
                this.NotifyExportPozew(idesc);
            };

        }

        /*        
        paczka.AppendLine("</curr:Pozwy>");

                using (StreamWriter outfile = new StreamWriter("OutDoc.xml"))
                {
                    outfile.Write(paczka);
                }


            }

    }
        */
        private void DoPozwyDraft_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // sporządzenie pzwów dla wybranych spraw.
            int[] IdSpr = new int[100];  // tablica na Id spraw
            int tabindex = 0;
            int i;
            pozwy = new pozewDscriptor[100];
            strindex = 0;
            // Wczytanie słownika
            //AlertMsg.Show("Funkcja w budowie");
            //return;
            foreach (var r in this.SprawyGridView.SelectedItems)
            {
                if (tabindex >= 100) break;
                IdSpr[tabindex++] = (r as vw_ListaSpraw).id;

            }
            SlownikFaktow slfakt = new SlownikFaktow();
            fakty = slfakt.faktSlownik;
            slfakt.faktyCompeted += (obj, exp) =>
            {


                if (tabindex > 0)
                {
                    this.BusyIndicator.IsBusy = true;
                    this.BusyIndicator.BusyContent = "Trwa generacja szkiców pozwów proszę czekać...";
                    i = 0;
                    for (i = 0; i < tabindex; i++)
                    {
                        PozewExtend pozextd = new PozewExtend();
                        pozextd.generatemode = 1; // tryb generacji 
                        pozextd.IdSprawy = IdSpr[i];
                        pozextd.fakty = fakty;
                        pozextd.PozewFromSprawa();
                        pozextd.dZlozenia = DateTime.Today;
                        pozextd.pozewCompleted += (sndr, ex) =>
                        {
                            if ((ex as pozewEventArgs).Status < 0) { this.BusyIndicator.IsBusy = false; return; } // bład ocdczytu pozwu
                                                                                                                  //spozew = pozextd.GetPozewSerialized();
                                                                                                                  //if (spozew != null) ;

                            pozewDscriptor pDesc = new pozewDscriptor();

                            pDesc.pozew = pozextd.GetPozewSerialized();

                            if (String.IsNullOrWhiteSpace(pDesc.pozew))
                            {
                                this.BusyIndicator.IsBusy = false;
                                ErrorWindow.CreateNew("Błąd podczas serializacji pozwu");
                                return;
                            }
                            pDesc.sygnatura = pozextd.sygnatura;

                            pozwy[strindex++] = pDesc;
                            if (strindex == tabindex)
                            {
                                this.BusyIndicator.IsBusy = false;
                                // propt do zpisywania 

                                SaveDirtyPozews();



                            }


                        };
                    }

                }
            };

        }

        private void setStatusySpraw()
        {
            List<int> IdSpr = new List<int>();
            SetStatusSprawy setStat = new SetStatusSprawy();
            setStat.Show();

            setStat.Closed += (sndr, ex) =>
                {
                    if (setStat.DialogResult == true)
                    {
                        if (setStat.Status != null)
                        {
                            if (setStat.ExtraStatus == null) setStat.ExtraStatus = 0;
                            foreach (var r in this.SprawyGridView.SelectedItems)
                            {
                                IdSpr.Add((r as vw_ListaSpraw).id);


                            }
                            if (IdSpr.Count > 0)
                            {
                                this.BusyIndicator.IsBusy = true;
                                PozewDomainContext context = new PozewDomainContext();
                                string idSprLst;
                                idSprLst = ToXMLSerializers.SerializeToString(IdSpr, typeof(List<int>));
                                context.SetNewStatus(idSprLst, "", setStat.DataZmianyStatusu, (int)setStat.Status, (int)setStat.ExtraStatus, 0, StatusChangedCompleted, null);
                            }
                        }
                    }

                };
        }






        private void StatusChangedCompleted(InvokeOperation<string> result)
        {
            if (result.Value.ToLower().Contains("error") || result.Value.ToLower().Contains("błąd"))
            {
                ErrorWindow.CreateNew(result.Value);

            }
            else
                this.vw_ListaSprawdds.Load();

            this.BusyIndicator.IsBusy = false;

        }

        private void ListaZmian_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            int i;
            if (e.AddedItems.Count > 0)
            {
                i = (sender as System.Windows.Controls.ListBox).SelectedIndex;
                (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;
                switch (i)
                {
                    case 0:
                        Dekret_Click();   // dekretacja
                        break;
                    case 1:
                        setStatusySpraw();

                        break;
                    case 2:
                        ChildWindow askDialog;

                        askDialog = new WyborDekretuWindowMasowo();
                        askDialog.Show();
                        askDialog.Closed += (_sender, _e) =>
                        {
                            this.vw_ListaSprawdds.Load();
                        };
                        break;

                    default:
                        break;

                }

            }

        }

        private void PozewOpcje_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            int i;
            if (e.AddedItems.Count > 0)
            {
                i = (sender as System.Windows.Controls.ListBox).SelectedIndex;
                (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;
                switch (i)
                {
                    case 0:
                        CreationMode = 0;
                        DoPozwy();   // pozwy bez kapitalizacji
                        break;
                    case 1:
                        //DoPozwyNew();  // p;ozwy z kapitalizacja odestek
                        CreationMode = 1;
                        DoPozwy();
                        break;

                    default:
                        break;

                }
            }

        }

        private void SprawyGridView_Filtered(object sender, GridViewFilteredEventArgs e)
        {
            this.vw_ListaSprawdds.Load();
        }

    }
}

