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
using Telerik.Windows.Controls;

namespace LexEnaTrs.Views
{
    public partial class SadySprView : UserControl
    {
        public int IdSprawy
        {
            get;
            set;
        }
        public SadySprView()
        {
            InitializeComponent();
        }



        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IdSprawy > 0) //LexEnaMeritumDomainContext context = (LexEnaMeritumDomainContext)sprawaDomainDataSource.DataContext;
            {
                SadSprawaDomainDataSource.QueryParameters[0].Value = IdSprawy;
                SadSprawaDomainDataSource.Load();

            }

        }





        private void DelSadSprawa_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            if (this.gridViewSadSprawa.SelectedItems.Count > 0)
            {
                SadSprawa statspr = (this.gridViewSadSprawa.CurrentItem as SadSprawa);
                if (statspr == null)
                {
                    AlertMsg.Show("Wybierz pozycję  do edycji");
                    return;
                }
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.Content = "Czy na pewno chcesz usunąć wskazaną pozycję  ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confdluwndClose;
                RadWindow.Confirm(dlgparm);
            }
        }

        private void confdluwndClose(object sender, WindowClosedEventArgs e)
        {
            SadSprawa sadSpr;
            if ((sender as RadWindow).DialogResult == true)
            {
                SadSprawa res = (this.gridViewSadSprawa.CurrentItem as SadSprawa);
                if (res == null)
                {

                    return;
                }
                try
                {
                    LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                    _context = new LexEnaMeritumDomainContext();
                    EntityQuery<SadSprawa> query = _context.GetSadSprawaByIdQuery(res.id);
                    LoadOperation<SadSprawa> loadop = _context.Load(query);
                    loadop.Completed += (sendr, except) =>
                    {
                        sadSpr = loadop.Entities.FirstOrDefault();
                        if (sadSpr != null)
                        {
                            sadSpr.modyfikator = UserProfile.DbId;
                            sadSpr.d_modyfikacji = DateTime.Now;
                            sadSpr.czyus = 1;
                            _context.SubmitChanges().Completed += (s, eva) =>
                            {
                                SadSprawaDomainDataSource.Load();

                            };
                        }
                    };
                }
                catch (Exception ex)
                {

                    ErrorWindow.CreateNew(ex, "Błąd podczas usuwania sądu");
                }
            }
        }

    

    private void SadSprawaDomainDataSource_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Błąd odczytu danych", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }


        private void AddSSad_Click(object sender, RoutedEventArgs e)
        {
            SadSprawa statspr;
            statspr = new SadSprawa();
            statspr.sprawa_id = IdSprawy;
            statspr.sad_id = 1;
            statspr.d_skierowania = DateTime.Now;
            statspr.czyus = 0;
            statspr.d_kreacji = DateTime.Now;
            statspr.kreator = UserProfile.DbId;

            AddSadSprawa adsspr = new AddSadSprawa();
            adsspr.Show();
            adsspr.SadSpr = statspr;
            adsspr.Closed += (sndr, ex) =>
                {
                    if (adsspr.DialogResult == true)
                    {
                        try
                        {
                            if (adsspr.SadSpr != null)
                            {
                                LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                                _context = new LexEnaMeritumDomainContext();
                                if (adsspr.SadSpr.id > 0) // update
                                {
                                    SadSprawa sadSpr;
                                    SadSprawa res = adsspr.SadSpr;
                                    EntityQuery<SadSprawa> query = _context.GetSadSprawaByIdQuery(adsspr.SadSpr.id);
                                    LoadOperation<SadSprawa> loadop = _context.Load(query);
                                    loadop.Completed += (sendr, except) =>
                                    {
                                        sadSpr = loadop.Entities.FirstOrDefault();
                                        if (sadSpr != null)
                                        {
                                            sadSpr.modyfikator = UserProfile.DbId;
                                            sadSpr.d_modyfikacji = DateTime.Now;
                                            sadSpr.sygnatura = res.sygnatura;
                                            sadSpr.uwagi = res.uwagi;
                                            sadSpr.sad_id = res.sad_id;
                                            sadSpr.d_skierowania = res.d_skierowania;
                                            _context.SubmitChanges().Completed += (s, eva) =>
                                            {
                                                SadSprawaDomainDataSource.Load();
                                                
                                            };

                                        }
                                    };
                                }

                                else
                                {
                                    _context.SadSprawas.Add(adsspr.SadSpr);
                                    _context.SubmitChanges().Completed += (s, eva) =>
                                    {
                                        SadSprawaDomainDataSource.Load();
                                    };
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            ErrorWindow.CreateNew(exp, "Bład dodawania sądu");


                        }

                    }

                };
        }



        private void Details_Click(object sender, System.Windows.RoutedEventArgs e)
        {



            if (this.gridViewSadSprawa.SelectedItems.Count > 0)
            {
                SadSprawa statspr = (this.gridViewSadSprawa.CurrentItem as SadSprawa);
                if (statspr == null)
                {
                    AlertMsg.Show("Wybierz pozycję  do edycji");
                    return;
                }

                AddSadSprawa adsspr = new AddSadSprawa();
                adsspr.Show();
                adsspr.SadSpr = statspr;
                adsspr.Closed += (sndr, ex) =>
                {
                    if (adsspr.DialogResult == true)
                    {
                        try
                        {
                            if (adsspr.SadSpr != null)
                            {
                                LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                            _context = new LexEnaMeritumDomainContext();
                                if (adsspr.SadSpr.id > 0) // update
                            {
                                    SadSprawa sadSpr;
                                    SadSprawa res = adsspr.SadSpr;
                                    EntityQuery<SadSprawa> query = _context.GetSadSprawaByIdQuery(adsspr.SadSpr.id);
                                    LoadOperation<SadSprawa> loadop = _context.Load(query);
                                    loadop.Completed += (sendr, except) =>
                                    {
                                        sadSpr = loadop.Entities.FirstOrDefault();
                                        if (sadSpr != null)
                                        {
                                            sadSpr.modyfikator = UserProfile.DbId;
                                            sadSpr.d_modyfikacji = DateTime.Now;
                                            sadSpr.sygnatura = res.sygnatura;
                                            sadSpr.uwagi = res.uwagi;
                                            sadSpr.sad_id = res.sad_id;
                                            sadSpr.d_skierowania = res.d_skierowania;
                                            _context.SubmitChanges().Completed += (s, eva) =>
                                            {
                                                SadSprawaDomainDataSource.Load();
                                            };

                                        }
                                    };
                                }

                                else
                                {
                                    _context.SadSprawas.Add(adsspr.SadSpr);
                                    _context.SubmitChanges().Completed += (s, eva) =>
                                    {
                                        SadSprawaDomainDataSource.Load();
                                    };
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            ErrorWindow.CreateNew(exp, "Bład dodawania sądu");


                        }

                    }

                };


            }

            else
            {
                AlertMsg.Show("Wybierz pozycję do edycji");
            }


        }

       
    }
}