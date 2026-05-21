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
using System.ServiceModel.DomainServices.Client;
using Telerik.Windows.Data;
using System.Windows.Data;
using LexEnaTrs.Views;
using System.ComponentModel;
using System.Collections.Specialized;
using Telerik.Windows.Controls;
using System.Windows.Browser;
using System.IO;

namespace LexEnaTrs
{
    public partial class SprawaView : UserControl
    {
    
     public int IdSprawy{ get; set;}

     public int Id_Jednostki { get; set; }

     public int Id_User { get; set; }

     private int odsetkiId = 0;
        
     public SprawaWindow SprWindHndl { get; set; }

     private SprawaExtraData danspr;// = new vw_DaneSprawy();
     public  QueryableCollectionView qcvListaDluznikow; 
            public SprawaView()
        {
            
            InitializeComponent();
            
            //CustomerGrid.ItemsSource = loadOp.Entities;

            //string myCustomValue = this.Resources as string;
        }
     



        private void Sprawa1_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
         


        	// TODO: Add event handler implementation here.
			 if (IdSprawy > 0) //LexEnaMeritumDomainContext context = (LexEnaMeritumDomainContext)sprawaDomainDataSource.DataContext;
            {

                sprawaDomainDataSource.QueryParameters[0].Value = IdSprawy;
                sprawaDomainDataSource.Load();
                DluznikDomainDataSource.QueryParameters[0].Value = IdSprawy;
                DluznikDomainDataSource.Load();
                NaleznosciDomainDataSource.QueryParameters[0].Value = IdSprawy;
                NaleznosciDomainDataSource.Load();
                ListaWplatSprawydds.QueryParameters[0].Value = IdSprawy;
                ListaWplatSprawydds.Load();
                DokWysSprawadds.QueryParameters[0].Value = IdSprawy;
                DokWysSprawadds.Load();
                DokOdebrSprawadds.QueryParameters[0].Value = IdSprawy;
                DokOdebrSprawadds.Load();
                danspr = (SprawaExtraData)this.Resources["InneDaneSprawy"];  // dane sprawy
                danspr.IdSprawy = IdSprawy;
                danspr.LoadData();
                danspr.PropertyChanged += (_sender, _e) =>
                        {
                            if (((_sender as SprawaExtraData).ExtraSprData.Referent == UserProfile.Imie + " " + UserProfile.Nazwisko) || (UserProfile.Rola == 1 && UserProfile.IdJednostki == Id_Jednostki) || (UserProfile.Rola == 2 ))
                            {
                                ;
                            }
                            else
                            {
                                this.gridViewOdsetkiNalSpr.IsReadOnly = true;
                                this.DokWysGridView.IsReadOnly = true;
                                this.RadGridListaDluznikow.IsReadOnly = true;
                                this.radGridViewDocIn.IsReadOnly = true;
                                this.radGridViewRoszczenia.IsReadOnly = true;
                                this.radGridViewStanyDetails.IsReadOnly = true;
                                this.radGridViewWplatyPodz.IsReadOnly = true;
                                this.DluznicyDataForm.IsEnabled = false;
                                this.SprawaDataForm.IsEnabled = false;
                            
                            }
                          
                            
                        };
                ZalSprawydds.QueryParameters[0].Value = IdSprawy;
                ZalSprawydds.Load();
                
            }
        }




        private void sprawaExtDomainDataSource_LoadedData(object sender, System.Windows.Controls.LoadedDataEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

        private void StatButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            StatusSprawyWindow stawindow = new StatusSprawyWindow();
            stawindow.ViewStatusy.IdSprawy = IdSprawy;
            stawindow.Closed += new EventHandler(stawindow_Closed);
            stawindow.Show();
       
        }

        void stawindow_Closed(object sender, EventArgs e)
        {
            StatusSprawyWindow stw = (StatusSprawyWindow ) sender;

            if (stw.DialogResult == true)
            {
              
                danspr.LoadData();
             }
            
        }

        void jwwindow_Closed(object sender, EventArgs e)
        {
            JednostkiWindykWindow jww = (JednostkiWindykWindow)sender;

            if (jww.DialogResult == true)
            {
               
                danspr.LoadData();
            }

        }

        private void JednostkaButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            JednostkiWindykWindow jwwindow = new JednostkiWindykWindow();
            jwwindow.ViewJednostki.IdSprawy = IdSprawy;
            jwwindow.Closed += new EventHandler(jwwindow_Closed);
            jwwindow.Show();
        }

        

       
       

        private void DluznicyDataForm_AddingNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddingNewItemEventArgs e)
        {
        
        }

        private void DluznikDomainDataSource_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
        	 if (e.HasError)
            {
            RadWindow.Alert("Błąd Odczytu danych dłużnika ");
            return;
            }// TODO: Add event handler implementation here.
        }
        private void NaleznosciDomainDataSource_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
            int i = 1; 
            if (e.HasError)
            {
            RadWindow.Alert ("Błąd Odczytu należności " + e.Error.ToString());
            return;
            }// TODO: Add event handler plementation here.
            // Numeracja 
             foreach (Naleznosc r in e.Entities)
             {
                 if (r.numer == 0)
                     r.numer = i;
                 i++;
             }
        }

        
        private void DluznicyDataForm_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            DaneDluznika newitem;
            newitem = (DaneDluznika)e.NewItem;
            newitem.Sprawa_Id = IdSprawy;
            newitem.FizPraw = 0;
            newitem.czyus = 0;

        }

       
        private void DluznicyDataForm_EditEnding(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndingEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            DluznikDomainDataSource.SubmitChanges();
        }

      

        private void DluznicyDataForm_DeletingItem(object sender, System.ComponentModel.CancelEventArgs e)
        {
        	// TODO: Add event handlmer implementation here.
            DialogParameters dlgparm = new DialogParameters();
            dlgparm.CancelButtonContent ="Nie";
            dlgparm.Content = "Czy na pewno chcesz usunąć dłużnika ?";
            dlgparm.Header="Potwierdź";
            dlgparm.Closed = confdluwndClose;
            RadWindow.Confirm(dlgparm); 
            e.Cancel = true;
        }

        private void confdluwndClose(object sender, WindowClosedEventArgs e)
        {
        if((sender as RadWindow).DialogResult == true)
            {
                DataItemCollection itemsToDel = (DataItemCollection)this.DluznicyDataForm.ItemsSource;//  as IList<DaneDluznika>;
                itemsToDel.Remove(this.DluznicyDataForm.CurrentItem as DaneDluznika);

                DluznikDomainDataSource.SubmitChanges();
            }     
        }

        private void DluznicyDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            DluznikDomainDataSource.SubmitChanges();
        }




        private void radGridViewRoszczenia_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            Naleznosc nal;
                if (e.AddedItems.Count > 0)
            {
               if  (e.RemovedItems.Count > 0 )  // sprawdzenie czy były zmiany w odsetkach
               {
                   // czy były odsetki
                   if (this.gridViewOdsetkiNalSpr.Items.Count > 0)
                       if ((e.RemovedItems[0] as Naleznosc).CzyOdsetki == 0)
                           (e.RemovedItems[0] as Naleznosc).CzyOdsetki = 1;
                       else
                           if ((e.RemovedItems[0] as Naleznosc).CzyOdsetki == 1)
                               (e.RemovedItems[0] as Naleznosc).CzyOdsetki = 0;
                }
               if (OdsetkiDomainDataSource.HasChanges)
                    {
                        OdsetkiDomainDataSource.SubmitChanges();
                        OdsetkiDomainDataSource.SubmittedChanges += (_sender, _e) =>
                            {
                                nal = (Naleznosc)e.AddedItems[0];
                                OdsetkiDomainDataSource.QueryParameters[0].Value = nal.Id;// IdSprawy;
                                OdsetkiDomainDataSource.Load();
                            };
                    }
               else
                     {
                        nal = (Naleznosc)e.AddedItems[0];
                        OdsetkiDomainDataSource.QueryParameters[0].Value = nal.Id;// IdSprawy;
                        OdsetkiDomainDataSource.Load();
                     }
               
            }

        }

        private void gridViewOdsetkiNalSpr_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            Odsetki ods = new Odsetki();
            ods.DataPocz = DateTime.Now;
            ods.DoZaplaty = 1;
            ods.OdWniesienia = 0;
            ods.Naleznosc_Id = (this.radGridViewRoszczenia.CurrentItem as Naleznosc).Id;
            ods.NazwyOdsetek_Id = 2;  // domuyślnie ustawowe
            ods.TypStopy = 0; // roczna
            e.NewObject = ods;
        }

        private void radGridViewRoszczenia_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            Naleznosc nal = new Naleznosc();
            nal.CzyOdsetki = 0;
            nal.CzySolidarnie = 0;
            nal.data_dok = DateTime.Now;
            nal.Sprawa_id = this.IdSprawy;
            nal.waluta = 0;
            nal.TypNaleznosci_id = 1;
            nal.TypRoszczenia = 0;
            e.NewObject = nal;

        }

        private void radGridViewRoszczenia_Deleting(object sender, GridViewDeletingEventArgs e)
        {
            
    
        	// TODO: Add event handlmer implementation here.
            DialogParameters dlgparm = new DialogParameters();
            dlgparm.CancelButtonContent ="Nie";
            dlgparm.Content = "Czy na pewno chcesz usunąć wskazane roszczenie ?";
            dlgparm.Header="Potwierdź";
            dlgparm.Closed = confnalwndClose;
            RadWindow.Confirm(dlgparm); 
            e.Cancel = true;
        }


        private void confnalwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {
                // usuń najpierw wszytkie odsetki
                Naleznosc nalToDelete;
                nalToDelete = this.radGridViewRoszczenia.CurrentItem as Naleznosc;
                PozewDomainContext thecontext = new PozewDomainContext();
                thecontext.DeleteNaleznosc(nalToDelete.Id, NalDeleteCompleted, null);



                return;
                DataItemCollection itemsodsToDel = (DataItemCollection)this.gridViewOdsetkiNalSpr.Items;
                if (itemsodsToDel.CanRemove)
                {
                    for (int i = 0; i< itemsodsToDel.Count; i++)
                        itemsodsToDel.RemoveAt(0);
                }
                try
                {

                    OdsetkiDomainDataSource.SubmitChanges();
                    OdsetkiDomainDataSource.SubmittedChanges += (_sender, _e) =>
                        {
                           
                            LexEnaMeritumDomainContext _lexena;  //radaDomainDataSource.DomainContext;
                            LoadOperation<StanNaleznosci> loadop;
                            _lexena = new LexEnaMeritumDomainContext();
                            EntityQuery<StanNaleznosci> query =
                            from c in _lexena.GetStanNaleznosciByIdNalQuery(nalToDelete.Id)
                            select c;
                            loadop = _lexena.Load(query);
                            loadop.Completed += (se, exe) =>
                            {
                                foreach (StanNaleznosci snn in loadop.Entities)
                                {
                                    _lexena.StanNaleznoscis.Remove(snn);

                                }

                                _lexena.SubmitChanges().Completed += (sendr, oj) =>
                                    {
                                        DataItemCollection nalitemToDel = (DataItemCollection)this.radGridViewRoszczenia.Items;//  as IList<DaneDluznika>;
                                        nalitemToDel.Remove(this.radGridViewRoszczenia.CurrentItem as Naleznosc);
                                        NaleznosciDomainDataSource.SubmitChanges();
                                        NaleznosciDomainDataSource.SubmittedChanges += (_sender1, _e1) =>
                                            {

                                                ;

                                            };
                                        //NaleznosciDomainDataSource.Su

                                        
                                    };

                            };

                            


                        };
                }
                catch (Exception ex)
                {
                    ErrorWindow.CreateNew(ex, "Błąd zapisu w bazie danych");
                }
         
            }
        }
        private void NalDeleteCompleted(InvokeOperation<string> retcode)
        {

            if (retcode.HasError)
            {
                ErrorWindow.CreateNew("Błąd poczas usuwania należności " + (String.IsNullOrEmpty(retcode.Value) ? "":retcode.Value));
            }
            else
            {
                NaleznosciDomainDataSource.Load();

            }

        }
        public void UpdateAll()
        {// Zapis niezapisanych danych
          // odsetki i roszczenia
            
            if (this.gridViewOdsetkiNalSpr.Items.Count > 0)
                if ((this.radGridViewRoszczenia.CurrentItem as Naleznosc).CzyOdsetki == 0)
                    (this.radGridViewRoszczenia.CurrentItem as Naleznosc).CzyOdsetki = 1;
                else
                    if ((this.radGridViewRoszczenia.CurrentItem as Naleznosc).CzyOdsetki == 1)
                        (this.radGridViewRoszczenia.CurrentItem as Naleznosc).CzyOdsetki = 0;
            if (OdsetkiDomainDataSource.HasChanges)
                    OdsetkiDomainDataSource.SubmitChanges();
            if (NaleznosciDomainDataSource.HasChanges)
                    NaleznosciDomainDataSource.SubmitChanges();   
                
                 
        }

        private void gridViewOdsetkiNalSpr_Deleted(object sender, GridViewDeletedEventArgs e)
        {
            
        }

        private void radGridViewRoszczenia_SelectionChanging(object sender, SelectionChangingEventArgs e)
        {  //zmiana parametru  CzyOdsetki 

            if (e.RemovedItems.Count == 1)
            {
                if (this.gridViewOdsetkiNalSpr.Items.Count > 0)
                    if ((e.RemovedItems[0] as Naleznosc).CzyOdsetki == 0 )
                    (e.RemovedItems[0] as Naleznosc).CzyOdsetki = 1;
                else
                        if ((e.RemovedItems[0] as Naleznosc).CzyOdsetki == 1)
                        (e.RemovedItems[0] as Naleznosc).CzyOdsetki = 0;

            }

        }

        private void SprawaDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            sprawaDomainDataSource.SubmitChanges();
        }

        private void sprawaDomainDataSource_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Błąd Odczytu danych", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
            else
            {




            }

        }

        private void WplatyGridView_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            int IdWplaty;
            if (e.AddedItems.Count > 0)
            {
             
                IdWplaty = (e.AddedItems[0] as Wplata).Id;
                this.PodzialWplatydds.QueryParameters[0].Value = IdWplaty;
                this.PodzialWplatydds.Load();
            
            }


        }
        
       

        private void ShowDoc_Click(object sender, RoutedEventArgs e)
        {
            vw_ListaDokWys Item;
            DokWys dw;
            int dokTyp;
            PozewDomainContext _pozewcontext;
            // pokaz dookument
            if (this.DokWysGridView.SelectedItems.Count > 0)
            {
                Item = this.DokWysGridView.SelectedItem as vw_ListaDokWys;
                if (Item != null && Item.TypDok > 10000)
                {
                    AlertMsg.Show("Do podglądu dokumentu dodanego ręcznie użyj funkcji Edycji");
                    return;
                }


                 GetTaleDbRow _GetRecord = new GetTaleDbRow();
                  _GetRecord.GetDocWysXML(Item.Id);
                 _GetRecord.rowCompleted += ( se, ea) =>
                     {
                         dw = ((ea as GetDBRowEventArgs).DbRow as DokWys);
                         _pozewcontext = new PozewDomainContext();
                         dokTyp = dw.TypDok;
                         if ((dw.TypDok >= 3 && dw.TypDok <= 6) || (dw.TypDok >= 13 && dw.TypDok <= 15)) dokTyp += 1000;
                          	_pozewcontext.DokumentZEPU2HTML(dw.Tresc, dokTyp,PozewEPUCompleted, null);   

                             
                     
                     };
                //odczyt 
            }

        }

        private void PozewEPUCompleted(InvokeOperation<string> html)
        {

            string htmlpath;

            htmlpath = html.Value;//e.Result.ToString();
            if (htmlpath != null)
            {
                Uri uri = new Uri(Application.Current.Host.Source, htmlpath);
                HtmlWindow window = HtmlPage.Window;
                window.Navigate(uri, "_blank");
            }
        }

        private void EditDoc_Click(object sender, RoutedEventArgs e)
        {
            // edycja.
            DokWys dokGet = null;
            DokWys dokWys = null; 

            if (this.DokWysGridView.SelectedItems.Count > 0)
            {
                vw_ListaDokWys doklWys = (this.DokWysGridView.CurrentItem as vw_ListaDokWys);
                if (doklWys == null)
                {
                    AlertMsg.Show("Wybierz pozycję  do edycji");
                    return;
                }

               
                
               
                LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                _context = new LexEnaMeritumDomainContext();
                EntityQuery<DokWys> query = _context.GetDokWysWithPdfByIdQuery(doklWys.Id);
                LoadOperation<DokWys> loadop = _context.Load(query);
                AddOutDocumentWindow outDoc = new AddOutDocumentWindow();
                loadop.Completed += (sendr, except) =>
                {
                    dokWys = loadop.Entities.FirstOrDefault();
                    if (dokWys != null)
                    {// odczyt opdf'a
                        PdfStore pdf = dokWys.PdfStore.FirstOrDefault();
                        if (pdf != null)
                        {
                          //  dokWys.KontoEPU_Id = pdf.Id;
                            outDoc.IdPdf = pdf.Id;
                        }
                    }

                    
                    outDoc.dokWys = dokWys;
                    outDoc.Show();
                    outDoc.Closed += (obj, ex) =>
                    {
                        if (outDoc.DialogResult == true)
                        {

                            _context = new LexEnaMeritumDomainContext();
                            query = _context.GetDokWysWithPozewByIdQuery(dokWys.Id);
                            loadop = _context.Load(query);
                            loadop.Completed += (sen, exe) =>
                            {
                                dokGet = loadop.Entities.FirstOrDefault();
                                if (dokGet != null)
                                {
                                    dokGet.Nazwa = dokWys.Nazwa;
                                    dokGet.DataDok = dokWys.DataDok;
                                    dokGet.d_modyfikacji = DateTime.Now;

                                    dokGet.modyfikator = UserProfile.DbId.ToString();

                                    /* if (outDoc.IdPdf > 0)
                                     {
                                         dokGet.KontoEPU_Id = outDoc.IdPdf;

                                     }
                                     */
                                    _context.SubmitChanges().Completed += (s, eva) =>
                                    {
                                        this.sprawaDomainDataSource.SubmitChanges();
                                        DokWysSprawadds.Load();
                                    };


                                };


                            };
                        }

                    };
                };
            }
            else
                AlertMsg.Show("Wybierz pozycję  do edycji");
        }
        private void AddDoc_Click(object sender, RoutedEventArgs e)
        {
            DokWys dw = new DokWys();
            dw.d_kreacji = DateTime.Now;
            dw.DataDok = DateTime.Today;
            dw.Sprawa_id = IdSprawy;
            dw.kreator = UserProfile.DbId.ToString();
            dw.TypDok = 100010;
            Sprawa spr = new Sprawa();

            Sprawa curspr = this.sprawaDomainDataSource.DataView.CurrentItem as Sprawa;
            spr.KosztyZadane = curspr.KosztyZadane;
            spr.KzpZadane = curspr.KzpZadane;
            spr.InneZadane = curspr.InneZadane;
          //  spr.DokWys.Add(dw);

            dw.Sprawa =spr;

            rodzajeDokumentow rd = new rodzajeDokumentow();
            rodzajDokumentu ts = rd.typDokumentu.Where(a => a.Numer == 100010).FirstOrDefault();
            if (ts != null) dw.Nazwa = ts.Nazwa;
            dw.PartitionKey = 0;
            dw.StatusDok = 3;


            AddOutDocumentWindow outDoc = new AddOutDocumentWindow();
            outDoc.dokWys = dw;
            outDoc.Show();
            outDoc.Closed += (obj, ex) =>
            {
                if (outDoc.DialogResult == true)
                {

                    LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                    
                    _context = new LexEnaMeritumDomainContext();
                    if (outDoc.IdPdf > 0)
                    {
                        dw.KontoEPU_Id = outDoc.IdPdf;


                    }
                    else
                        dw.KontoEPU_Id = null;

                    curspr.KosztyZadane = dw.Sprawa.KosztyZadane;
                    curspr.KzpZadane = dw.Sprawa.KzpZadane;
                    curspr.InneZadane = dw.Sprawa.InneZadane;
                    
                    dw.Sprawa_id = IdSprawy;
                    _context.DokWys.Add(dw);
                    _context.SubmitChanges().Completed += (s, eva) =>
                            {

                                // jeśli pozew 
                               
                                this.sprawaDomainDataSource.SubmitChanges();
                                DokWysSprawadds.Load();
                            };
                }
            };


        }
        
        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StatButtonReferent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            ReferentWindow refwindow = new ReferentWindow();
            refwindow.IdSprawy = IdSprawy;
            refwindow.Closed += new EventHandler(refwindow_Closed);
            refwindow.Show();
        }


        void refwindow_Closed(object sender, EventArgs e)
        {
            ReferentWindow rw = (ReferentWindow)sender;

            if (rw.DialogResult == true)
            {
                danspr.LoadData();
            }

        }

        private void ZalegGridView_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {   
                       
                this.ZalKwotyDetailsdds.QueryParameters[0].Value = IdSprawy;
                this.ZalKwotyDetailsdds.QueryParameters[1].Value = Convert.ToDateTime((e.AddedItems[0] as StanSprawy).data_s);
                this.ZalKwotyDetailsdds.Load();
                       
            }

        }

        private void DluznicyDataForm_CurrentItemChanged(object sender, EventArgs e)
        {
            DaneDluznika dd;

            dd = this.DluznicyDataForm.CurrentItem as DaneDluznika;
            if (dd != null)
                dd.PropertyChanged +=new PropertyChangedEventHandler(dd_PropertyChanged);

        }

        private void dd_PropertyChanged(object sender, PropertyChangedEventArgs pea)
        {
            DaneDluznika dandl;

            if (pea.PropertyName == "FizPraw")
            {
                dandl = sender as DaneDluznika;
                if (dandl.FizPraw < 2)
                {
                    dandl.czyrejestr = 0;
                    dandl.krs = "";
                }
                switch(dandl.FizPraw)
                {
                    case 0:
                        if (string.IsNullOrWhiteSpace(dandl.Nazwisko))
                            dandl.Nazwisko = dandl.Nazwa;
                        dandl.Nazwa = "";
                        break;
                    case 2:
                    case 3:
                        if (string.IsNullOrWhiteSpace(dandl.Nazwa))
                            dandl.Nazwa = dandl.Nazwisko;
                       // dandl.Nazwisko = "";
                       // dandl.Imie = "";
                        break;

                }
            }
        
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            int IdDoc;
            if (this.radGridViewDocIn.SelectedItems.Count > 0)
            {
                IdDoc = (this.radGridViewDocIn.SelectedItem as DokOdebr).Id;
                PozewDomainContext _pozewcontext;
                if (IdDoc > 0)
                {
                    _pozewcontext = new PozewDomainContext();
                    _pozewcontext.GetHtmlDocOdebr(IdDoc,(int)UserProfile.Rola, (int)UserProfile.DbId, HtmlDocCompleted, null);
                }



            }
        }

        private void HtmlDocCompleted(InvokeOperation<string> html)
        {

            string htmlpath;

            htmlpath = html.Value;//e.Result.ToString();
            if (htmlpath.ToLower().Contains("błąd") || htmlpath.ToLower().Contains("error"))
                ErrorWindow.CreateNew(htmlpath);
            else
            {
                Uri uri = new Uri(Application.Current.Host.Source, htmlpath);
                HtmlWindow window = HtmlPage.Window;
                window.Navigate(uri, "_blank");
            }
        }

        private void LiczZalButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            PozewDomainContext thecontext; 
         GetDateWindow wnd = new GetDateWindow();
                            wnd.Show();
                            wnd.Closed += (se, ea) =>
                            {
                                if (wnd.DialogResult == true)
                                {
                                    DateTime? newDate = wnd.ChosenDate;
                                    if (newDate != null)
                                    {
                                    thecontext = new PozewDomainContext();
                                    this.BusyIndicator.IsBusy = true;
                                    thecontext.LiczZaleglosc(this.IdSprawy,Convert.ToDateTime(newDate), LiczZalCompleted, null);
                                    }
                                }
                            }; 

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
                ZalSprawydds.Load();
                ZalKwotyDetailsdds.Load();
                this.BusyIndicator.IsBusy = false;
            }
        }
        void sadwindow_Closed(object sender, EventArgs e)
        {
            SadySprawyWindow stw = (SadySprawyWindow)sender;

            if (stw.DialogResult == true)
            {

                danspr.LoadData();
            }

        }
        private void SadSelButton_Click(object sender, RoutedEventArgs e)
        {
            SadySprawyWindow stawindow = new SadySprawyWindow();
            stawindow.ViewSady.IdSprawy = IdSprawy;
            stawindow.Closed += new EventHandler(sadwindow_Closed);
            stawindow.Show();
        }

        private void AddOdebrButton_Click(object sender, RoutedEventArgs e)
        {
            DokOdebr dokOd = new DokOdebr();
            dokOd.d_kreacji = DateTime.Now;
            dokOd.DataDokumentu = DateTime.Today;
            dokOd.Sprawa_id = IdSprawy;
            dokOd.kreator = UserProfile.DbId.ToString();
            dokOd.TypDok = 100005;
            dokOd.DataRejestracji = DateTime.Today;
            Sprawa spr = new Sprawa();

            Sprawa curspr = this.sprawaDomainDataSource.DataView.CurrentItem as Sprawa;
            spr.KosztyZadane = curspr.KosztyZadane;
            spr.KzpZadane = curspr.KzpZadane;
            spr.InneZadane = curspr.InneZadane;

            rodzajeDokumentowOdebr rd = new rodzajeDokumentowOdebr();
            rodzajDokumentu ts = rd.typDokumentu.Where(a => a.Numer == 100005).FirstOrDefault();
            if (ts != null) dokOd.Nazwa = ts.Nazwa;
            dokOd.PartitionKey = 0;
            dokOd.StatusDok = "wydany" ;
            dokOd.Sprawa = spr;

            AddOutDocumentWindow outDoc = new AddOutDocumentWindow();
            outDoc.dokOdebr = dokOd;
            outDoc.Show();
            outDoc.Closed += (obj, ex) =>
            {
                if (outDoc.DialogResult == true)
                {

                    LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                    _context = new LexEnaMeritumDomainContext();
                    if (outDoc.IdPdf > 0)
                    {
                        dokOd.KontoEPU_Id = outDoc.IdPdf;


                    }
                    else
                        dokOd.KontoEPU_Id = null;

                    dokOd.Sprawa_id = IdSprawy;
                    _context.DokOdebrs.Add(dokOd);
                    _context.SubmitChanges().Completed += (s, eva) =>
                    {
                        if (dokOd.Sprawa != null)
                        {
                            curspr.KosztyZadane = dokOd.Sprawa.KosztyZadane;
                            curspr.KzpZadane = dokOd.Sprawa.KzpZadane;
                            curspr.InneZadane = dokOd.Sprawa.InneZadane;
                        }
                        this.sprawaDomainDataSource.SubmitChanges();
                        DokOdebrSprawadds.Load();
                    };
                }
            };

        }

        private void EditOdebrButton_Click(object sender, RoutedEventArgs e)
        {
            
                // edycja.
                DokOdebr dokOd = null;
                DokOdebr dokOdebr = null;

                if (this.radGridViewDocIn.SelectedItems.Count > 0)
                {
                     dokOdebr = (this.radGridViewDocIn.CurrentItem) as DokOdebr;
                    if (dokOdebr == null)
                    {
                        AlertMsg.Show("Wybierz pozycję  do edycji");
                        return;
                    }


                    AddOutDocumentWindow outDoc = new AddOutDocumentWindow();
                    PdfStore pdf = dokOdebr.PdfStore.FirstOrDefault();
                    if (pdf != null)
                    {
                        outDoc.IdPdf = pdf.Id;
                    }
                    outDoc.dokOdebr = dokOdebr;
                    outDoc.Show();
                    outDoc.Closed += (obj, ex) =>
                    {
                        if (outDoc.DialogResult == true)
                        {

                            LexEnaMeritumDomainContext _context = new LexEnaMeritumDomainContext();
                            EntityQuery<DokOdebr> query = _context.GetDokOdebrByIdQuery(dokOdebr.Id);
                            LoadOperation<DokOdebr> loadop = _context.Load(query);
                            loadop.Completed += (sen, exe) =>
                            {
                                dokOd= loadop.Entities.FirstOrDefault();
                                if (dokOd != null)
                                {
                                    dokOd.Nazwa  = dokOdebr.Nazwa;
                                    dokOd.DataDokumentu = dokOdebr.DataDokumentu;
                                    dokOd.DataRejestracji = dokOdebr.DataRejestracji;
                                    dokOd.d_modyfikacji = DateTime.Now;
                                    dokOd.modyfikator = UserProfile.DbId.ToString();

                                    this.sprawaDomainDataSource.SubmitChanges();
                                    _context.SubmitChanges().Completed += (s, eva) =>
                                    {
                                        DokOdebrSprawadds.Load();
                                    };


                                };


                            };
                        }

                  
                };

                }
                else
                    AlertMsg.Show("Wybierz pozycję  do edycji");
            }

        private void DokWysSprawadds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }

        private void DokOdebrSprawadds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }

        private void gridViewOdsetkiNalSpr_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            if (e.EditingElement.GetType() == typeof(Telerik.Windows.Controls.RadComboBox))
            {
                if (e.OldData != e.NewData)
                {
                    odsetkiId = (int) ( e.NewData );
                    DialogParameters dlgparm = new DialogParameters();
                    dlgparm.CancelButtonContent = "Nie";
                    dlgparm.Content = "Czy chcesz ustawić taki sam rodzaj odsetek  dla wszystkich należności w sprawie ?";
                    dlgparm.Header = "Potwierdź";
                    dlgparm.Closed = confodsClose;
                    RadWindow.Confirm(dlgparm);
                   

                }
            }
          
         
        }


       
       

    private void confodsClose(object sender, WindowClosedEventArgs e)
    {
        if ((sender as RadWindow).DialogResult == true)
        {

               PozewDomainContext  myContext = new PozewDomainContext() ;
                myContext.SetOdsetkiType(this.IdSprawy, odsetkiId, OdsUpdateCompleted, null);

        }
    }

        private void OdsUpdateCompleted(InvokeOperation<string> retcode)
        {

            if (retcode.HasError)
            {
                ErrorWindow.CreateNew("Błąd poczas usuwania należności " + (String.IsNullOrEmpty(retcode.Value) ? "" : retcode.Value));
            }
            else
            {
              

               
                NaleznosciDomainDataSource.QueryParameters[0].Value = IdSprawy;
                NaleznosciDomainDataSource.Load();
               

            }

        }

         



        private void NaleznosciDomainDataSource_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;

            NaleznosciDomainDataSource.DomainContext.EntityContainer.Clear();
          
            //NaleznosciDomainDataSource.DataView.Clear();
        }

        private void OdsetkiDomainDataSource_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }

        private void RefreshDownloadAll_Click(object sender, RoutedEventArgs e)
        {
            if (this.DokWysGridView.Items.Count > 0)
            {
                PozewDomainContext context = new PozewDomainContext();
                List<int> lst = new List<int>();
                lst.Add(IdSprawy);
                context.AttachDocuments(ToXMLSerializers.SerializeToString( lst,typeof(List<int>)), UserProfile.Firma, AttachDocumentsCompleted, null);




            }


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
                DokWysSprawadds.QueryParameters[0].Value = IdSprawy;
                DokWysSprawadds.Load();
                this.BusyIndicator.IsBusy = false;
                // call 
            }
        }




        private void DownloadAll_Click(object sender, RoutedEventArgs e)
        {
            if (this.DokWysGridView.Items.Count > 0)
            {
                PozewDomainContext context = new PozewDomainContext();
                context.GetZippedDocuments(IdSprawy, GetZippedDocumentsCompleted, null);

              


            }

                
        }

        private void GetZippedDocumentsCompleted(InvokeOperation<string> retcode)
        {

            if (retcode.HasError)
            {
                ErrorWindow.CreateNew("Błąd poczas pobierania dokumentów " + (String.IsNullOrEmpty(retcode.Value) ? "" : retcode.Value));
            }
            else
            {
                if (retcode.Value.StartsWith("Błąd"))
                {

                    ErrorWindow.CreateNew(retcode.Value);
                    return;

                }

                DaneDoEksportuReady wnd = new DaneDoEksportuReady();
                wnd.Show();
                wnd.Closed += (obj, e) =>
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Dokumenty skompresowane ZIP | *.zip";
                    sfd.DefaultExt = "zip";
                    //sfd.DefaultFileName = "AlamAkota.zip";
                    bool? isSaveDialogShown = sfd.ShowDialog();
                    if (isSaveDialogShown == true)
                    {
                        using (Stream fs = (Stream)sfd.OpenFile())
                        {
                            using (BinaryWriter writer = new BinaryWriter(fs))
                            {
                                byte[] f = Convert.FromBase64String(retcode.Value);
                                writer.Write(f);
                            }
                        }

                    }

                };
            }

        }

      

        private void ZwrocButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserProfile.Rola == 2)
            {
                RadWindow.Alert("Funkcja dostępna tylko dla zewnętrznych kancelarii");
                return;
            }
            DialogParameters dlgparm = new DialogParameters();
            dlgparm.CancelButtonContent = "Nie";
            dlgparm.Content = "Czy na pewno chcesz zwrócić Enerdze sprawę do uzupełnienia  ?";
            dlgparm.Header = "Potwierdź";
            dlgparm.Closed = confirmReturnClose;
            RadWindow.Confirm(dlgparm);
             
        }

        
      

    private void confirmReturnClose(object sender, WindowClosedEventArgs e)
    {
        if ((sender as RadWindow).DialogResult == true)
        {
                GetReturnReasonsWindow grWin = new GetReturnReasonsWindow();
                grWin.Show();
                grWin.Closed += (se, ea) =>
                     {
                         if (grWin.DialogResult == true)
                         {
                             int przycz = -1;
                             przycz = grWin.Przyczyna;
                             string opis = grWin.Opis;

                             PozewDomainContext pdmc = new PozewDomainContext();
                             pdmc.ReturnCase(this.IdSprawy,przycz, opis, returnCaseCallback, null);

                         }
                     };

              

        }
    }
        private void returnCaseCallback(InvokeOperation<string> retcode)
        {
            if (retcode.HasError)
            {
                RadWindow.Alert(retcode.Error.Message);
                return;
            }
            if (!String.IsNullOrWhiteSpace(retcode.Value))
            {
                RadWindow.Alert(retcode.Error.Message);
                return;
            }
            if (SprWindHndl != null)
                SprWindHndl.DialogResult = true;

        }



        /*
         * 
         *    private void EditDoc_Click(object sender, RoutedEventArgs e)
        {
            // edycja.
            DokWys dokGet = null;
            DokWys dokWys = null; 

            if (this.DokWysGridView.SelectedItems.Count > 0)
            {
                vw_ListaDokWys doklWys = (this.DokWysGridView.CurrentItem as vw_ListaDokWys);
                if (doklWys == null)
                {
                    AlertMsg.Show("Wybierz pozycję  do edycji");
                    return;
                }




                LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                _context = new LexEnaMeritumDomainContext();
                EntityQuery<DokWys> query = _context.GetDokWysWithPdfByIdQuery(doklWys.Id);
                LoadOperation<DokWys> loadop = _context.Load(query);
                AddOutDocumentWindow outDoc = new AddOutDocumentWindow();
                loadop.Completed += (sendr, except) =>
                {
                    dokWys = loadop.Entities.FirstOrDefault();
                    if (dokWys != null)
                    {// odczyt opdf'a
                        PdfStore pdf = dokWys.PdfStore.FirstOrDefault();
                        if (pdf != null)
                        {
                            dokWys.KontoEPU_Id = pdf.Id;
                            outDoc.IdPdf = pdf.Id;
                        }
                    }


                    outDoc.dokWys = dokWys;
                    outDoc.Show();
                    outDoc.Closed += (obj, ex) =>
                    {
                        if (outDoc.DialogResult == true)
                        {

                            _context = new LexEnaMeritumDomainContext();
                            query = _context.GetDokWysWithPozewByIdQuery(dokWys.Id);
                            loadop = _context.Load(query);
                            loadop.Completed += (sen, exe) =>
                            {
                                dokGet = loadop.Entities.FirstOrDefault();
                                if (dokGet != null)
                                {
                                    dokGet.Nazwa = dokWys.Nazwa;
                                    dokGet.DataDok = dokWys.DataDok;
                                    dokGet.d_modyfikacji = DateTime.Now;

                                    dokGet.modyfikator = UserProfile.DbId.ToString();


        _context.SubmitChanges().Completed += (s, eva) =>
                                    {
                                        DokWysSprawadds.Load();
                                    };


    };


                            };
                        }

                    };
                };
            }
            else
                AlertMsg.Show("Wybierz pozycję  do edycji");
        }
        private void AddDoc_Click(object sender, RoutedEventArgs e)
    {
    DokWys dw = new DokWys();
    dw.d_kreacji = DateTime.Now;
    dw.DataDok = DateTime.Today;
    dw.Sprawa_id = IdSprawy;
    dw.kreator = UserProfile.DbId.ToString();
    dw.TypDok = 100010;
    rodzajeDokumentow rd = new rodzajeDokumentow();
    rodzajDokumentu ts = rd.typDokumentu.Where(a => a.Numer == 10010).FirstOrDefault();
    if (ts != null) dw.Nazwa = ts.Nazwa;
    dw.PartitionKey = 0;
    dw.StatusDok = 3;


    AddOutDocumentWindow outDoc = new AddOutDocumentWindow();
    outDoc.dokWys = dw;
    outDoc.Show();
    outDoc.Closed += (obj, ex) =>
    {
        if (outDoc.DialogResult == true)
        {

            LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
            _context = new LexEnaMeritumDomainContext();
            if (outDoc.IdPdf > 0)
            {
                dw.KontoEPU_Id = outDoc.IdPdf;


            }
            else
                dw.KontoEPU_Id = null;

            _context.DokWys.Add(dw);
            _context.SubmitChanges().Completed += (s, eva) =>
            {

                // jeśli pozew 


                DokWysSprawadds.Load();
            };
        }
    };


    }


         *
         */
    }
}
