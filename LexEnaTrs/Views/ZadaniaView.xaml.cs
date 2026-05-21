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
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using LexEnaTrs;
using LexEnaTrs.Web;
using System.Xml.Serialization;
using System.IO;
using System.ServiceModel.DomainServices.Client;


namespace LexEnaTrs.Views
{
    public partial class ZadaniaView : UserControl
    {

        private Zadania zadanie;//= new Zadania();


        public ZadaniaView()
        {
            InitializeComponent();

        }

        private void ZadaniaDataForm_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            ZadanieSet newitem;
            newitem = (ZadanieSet)e.NewItem;

        }



        private void ZadaniaDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            this.Zadaniadds.SubmitChanges();
        }

        private void UpdateZadanieSet(ref ZadanieSet zdSet)
        {

            try
            {

                zdSet.NazwaZadania = zadanie.NazwaZadania;
                zdSet.TypZadaniaId = zadanie.TypZadaniaId;
                zdSet.DataRozpoczęcia = (DateTime)zadanie.DataRozpoczęcia;
                zdSet.DataZakonczenia = zadanie.DataZakonczenia;
                zdSet.Oczasie = zadanie.Oczasie;
                zdSet.Opis = zadanie.Opis;
                zdSet.Status = zadanie.Status;


                EPUParamModel eParams = new EPUParamModel();
                eParams.KryteriumFiltrowania = zadanie.KryteriumFiltrowania;
                eParams.DataDo = zadanie.DataDo;
                eParams.DataOd = zadanie.DataOd;
                eParams.FiltrSlowny = zadanie.FiltrSlowny;
                eParams.IdPozwuWLexEna = zadanie.IdPozwuWLexEna;
                eParams.KontoEpuId = zadanie.KontoEPUId;
                eParams.NumerOd = zadanie.NumerOd;
                eParams.NumerDo = zadanie.NumerDo;
                eParams.Rok = zadanie.Rok;
                eParams.RodzajDaty = zadanie.KryteriumFiltrowaniaDaty;
                zdSet.Parametry = ToXMLSerializers.SerializeToString(eParams, typeof(EPUParamModel));
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, "Błąd opercji ");
            }


        }

        private void SetZadanie(ZadanieSet zdSet)
        {

            try
            {
                zadanie = new Zadania();
                zadanie.NazwaZadania = zdSet.NazwaZadania;
                zadanie.TypZadaniaId = zdSet.TypZadaniaId;
                zadanie.DataRozpoczęcia = zdSet.DataRozpoczęcia;
                zadanie.DataZakonczenia = zdSet.DataZakonczenia;
                zadanie.Oczasie = zdSet.Oczasie;
                zadanie.Parametry = zdSet.Parametry;
                zadanie.Opis = zdSet.Opis;
                zadanie.Status = zdSet.Status;
                if (zdSet.Parametry != null)
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(EPUParamModel));
                    EPUParamModel eParams = (EPUParamModel)mySerializer.Deserialize(new StringReader(zdSet.Parametry));
                    zadanie.KryteriumFiltrowania = eParams.KryteriumFiltrowania;
                    zadanie.DataDo = eParams.DataDo;
                    zadanie.DataOd = eParams.DataOd;

                    zadanie.FiltrSlowny = eParams.FiltrSlowny;
                    zadanie.IdPozwuWLexEna = eParams.IdPozwuWLexEna;
                    zadanie.KontoEPUId = eParams.KontoEpuId;
                    zadanie.NumerOd = eParams.NumerOd;
                    zadanie.NumerDo = eParams.NumerDo;
                    zadanie.Rok = eParams.Rok;
                    zadanie.KryteriumFiltrowaniaDaty = eParams.RodzajDaty;

                }
                this.LayoutRoot.DataContext = zadanie;
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, "Błąd opercji");
            }

        }



        private void ZadaniaDataGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            ZadanieSet zdSet;

           /* if (e.RemovedItems.Count > 0)
            {  // Zapamiętaj dane 

                zdSet = e.RemovedItems[0] as ZadanieSet;
                UpdateZadanieSet(ref zdSet);

            }
            */
            if (e.AddedItems.Count > 0)
            {
                SetZadanie(e.AddedItems[0] as ZadanieSet);
            }

        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            this.ZadaniaDataGrid.BeginInsert();
        }

        private void DelRow_Click(object sender, RoutedEventArgs e)
        {

            if (this.ZadaniaDataGrid.SelectedItems.Count > 0)
            {
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.Content = "Czy na pewno chcesz usunąć wybrane zadanie ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confdluwndClose;
                RadWindow.Confirm(dlgparm);
            }
        }

        private void confdluwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {
                DataItemCollection itemsToDel = (DataItemCollection)this.ZadaniaDataGrid.ItemsSource;//  as IList<DaneDluznika>;
                itemsToDel.Remove(this.ZadaniaDataGrid.CurrentItem as ZadanieSet);

                this.Zadaniadds.SubmitChanges();
            }



        }

        private void SaveRow_Click(object sender, RoutedEventArgs e)
        {
            ZadanieSet zdSet;
            try
            {
                if (this.ZadaniaDataGrid.SelectedItems.Count > 0)
                {
                    zdSet = (this.ZadaniaDataGrid.SelectedItems[this.ZadaniaDataGrid.SelectedItems.Count - 1] as ZadanieSet);
                    //zdSet = (this.ZadaniaDataGrid.CurrentItem as ZadanieSet);
                    UpdateZadanieSet(ref zdSet);
                }
                this.Zadaniadds.SubmitChanges();
                this.Zadaniadds.SubmittedChanges += (obj, exp) =>
                {

                    int i;
                    i = 1;
                };
            }

            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, "Błąd zapisu danych ");

            }
        }

        private void ZadaniaDataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {



            ZadanieSet newZadanie;
            newZadanie = new ZadanieSet();
            newZadanie.NazwaZadania = "?????";
            newZadanie.DataRozpoczęcia = DateTime.Now;
            newZadanie.Oczasie = false;

            try
            {

                e.NewObject = newZadanie;

            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex);
            }


        }

        private void LunchApp_Click(object sender, RoutedEventArgs e)
        {
            PozewDomainContext _pozewcontext = new PozewDomainContext();
            _pozewcontext.LunchApp(1, LunchCompleted, null);

        }

        private void LunchCompleted(InvokeOperation<string> result)
        {

            if (result.Value != "OK")
                ErrorWindow.CreateNew(result.Value);
            else
                AlertMsg.Show("Zadanie zostało uruchomione. Odśwież zawartosć okna by śledzić wyniki");
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ZadanieSet zdSet;
            this.Zadaniadds.Load();
            this.Zadaniadds.Loaded += (obj, ea) =>
            {
                if (this.ZadaniaDataGrid.SelectedItems.Count > 0)
                {
                    zdSet = (this.ZadaniaDataGrid.SelectedItem as ZadanieSet);
                    SetZadanie(zdSet);
                }
            };
        }

        private void Zadaniadds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }

        private void JobsCopy_Click(object sender, RoutedEventArgs e)
        {
            ZadanieSet zadanieToAdd;
            DateTime? dataCzas;
            EPUParamModel newParams;
            LexEnaMeritumDomainContext LexEnaContext;  //radaDomainDataSource.DomainContext;

            if (this.ZadaniaDataGrid.SelectedItems.Count > 0)
            {
                GetDatetimeWindow wnd = new GetDatetimeWindow();
                //dataCzas = DateTime.Today 
                
                wnd.Show();
                wnd.DataDokumentu.SelectedValue = DateTime.Today.AddDays(1).AddMinutes(-1);
                wnd.Closed += (se, ea) =>
                {
                    if (wnd.DialogResult == true)
                    {
                        dataCzas = wnd.ChosenDate;
                        LexEnaContext = new LexEnaMeritumDomainContext();
                        foreach (ZadanieSet zdSet in this.ZadaniaDataGrid.SelectedItems)
                        {
                            zadanieToAdd = new ZadanieSet();
                            zadanieToAdd.NazwaZadania = zdSet.NazwaZadania;
                            zadanieToAdd.TypZadaniaId = zdSet.TypZadaniaId;
                            zadanieToAdd.Status = 0;
                            zadanieToAdd.Oczasie = zdSet.Oczasie; // false;
                            zadanieToAdd.DataRozpoczęcia = DateTime.Today;
                            
                            if (zdSet.Parametry != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(EPUParamModel));
                                EPUParamModel eParams = (EPUParamModel)mySerializer.Deserialize(new StringReader(zdSet.Parametry));
                                newParams = new EPUParamModel();
                                newParams.KryteriumFiltrowania = eParams.KryteriumFiltrowania;
                                newParams.DataDo = dataCzas;
                                if (zdSet.DataZakonczenia > eParams.DataDo)
                                    newParams.DataOd = Convert.ToDateTime(eParams.DataDo).AddHours(-3);
                                else
                                    newParams.DataOd = Convert.ToDateTime(zdSet.DataZakonczenia).AddHours(-3)  ;

                                newParams.FiltrSlowny = eParams.FiltrSlowny;
                                newParams.IdPozwuWLexEna = eParams.IdPozwuWLexEna;
                                newParams.KontoEpuId = eParams.KontoEpuId;
                                newParams.NumerOd = eParams.NumerOd;
                                newParams.NumerDo = eParams.NumerDo;
                                newParams.Rok = eParams.Rok;
                                newParams.RodzajDaty = eParams.RodzajDaty;
                                zadanieToAdd.Parametry = ToXMLSerializers.SerializeToString(newParams, typeof(EPUParamModel));

                            }
                            /*
                            if (zadanieToAdd.Oczasie == true)
                            {
                            
                                else
                                zadanieToAdd.DataRozpoczęcia = DateTime.Today;


                            switch (zdSet.TypZadaniaId)
                            {
                                case 7: // orzeczenia
                                case 5: // nakazy
                                case 3:
                                    // moje sprawy
                                    break;
                                default:
                                    continue;

                            }*/
                            LexEnaContext.ZadanieSets.Add(zadanieToAdd);
                             
                        }
                        try
                        {

                            LexEnaContext.SubmitChanges().Completed += (sen, ex) =>
                            { 
                                
                             
                             this.Zadaniadds.Load();
                            };

                        }
                        catch (Exception exe)
                        {
                            ErrorWindow.CreateNew(exe, "Błąd zapisu danych");
                        }



                    }
                };





            }










        }
    }
}
