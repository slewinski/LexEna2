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

namespace LexEnaTrs.Views
{
    public partial class WyborPaczkiWindow : ChildWindow
    {
        
  

        public int IdPaczki
        {
            get;
            set;
        }
        public Paczka wybPaczka
        {
            get;
            set;
        }
        public int TypPaczki
        {
            get;
            set;
        }
        public WyborPaczkiWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            vw_Paczki item;
            Paczka _paczka;
            if (this.GridViewWyborPaczki.SelectedItems.Count  == 1 )
            {
                item = (this.GridViewWyborPaczki.SelectedItem as vw_Paczki) ;
                if (item.Id > 0)  // jmuż zapisana
                {
                    this.IdPaczki = item.Id;
                    this.TypPaczki = (int)item.TypDok;
                    this.DialogResult = true;
                }
                else
                {
                    LexEnaMeritumDomainContext _context = new LexEnaMeritumDomainContext();
                    _paczka = new Paczka();
                    _paczka.czyus = 0;
                    _paczka.DataZalozenia = item.DataZalozenia;
                    _paczka.Oznaczenie = item.Oznaczenie;
                    _paczka.nr = item.nr;
                    _paczka.rok = item.rok;
                    _paczka.miesiac = item.miesiac;
                    _paczka.StatusPaczki = 1;
                    _paczka.TypDok = item.TypDok;
                    try
                    {
                        _context.Paczkas.Add(_paczka);
                        _context.SubmitChanges(OnSubmitCompleted, null);
                    }
                    catch (Exception ex)
                    {
                        ErrorWindow.CreateNew(ex, "Błąd zapisu nowej paczki");
                        return;
                    }

                }   // dodaj paczkę 
                

            }
            
        }

        private void OnSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                ErrorWindow.CreateNew(string.Format(" {0}", so.Error.Message));
                so.MarkErrorAsHandled();
            }
            else
            {
            this.IdPaczki = (so.ChangeSet.AddedEntities[0] as Paczka).Id;
            this.TypPaczki = (int)(so.ChangeSet.AddedEntities[0] as Paczka).TypDok;
            this.wybPaczka = (so.ChangeSet.AddedEntities[0] as Paczka);
            this.DialogResult = true;
            }
        }
       



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        
        
     

        private void GridViewWyborPaczki_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            vw_Paczki  newpaczka;
            PaczkaExtnd pextd;
            Paczka oznaczp;
            try
            {
                this.AddPaczka.IsEnabled = false;
                newpaczka = new vw_Paczki();
                newpaczka.DataZalozenia = DateTime.Now;
                newpaczka.Oznaczenie = "";
                newpaczka.StatusPaczki = 1;
                newpaczka.TypDok = 10;
                newpaczka.KontoEPU_Id = UserProfile.IdKontaEPU;
                e.NewObject = newpaczka;
                pextd = new PaczkaExtnd();
                pextd.GetMaxPaczka(UserProfile.IdJednostki);
                pextd.paczkaNoCompleted += (Section, ea) =>
                    {
                        oznaczp = (ea as PaczkaNoEventArgs).Paczka;
                        newpaczka.Oznaczenie = oznaczp.Oznaczenie;
                        newpaczka.nr = oznaczp.nr;
                        newpaczka.rok = oznaczp.rok;
                        newpaczka.miesiac = oznaczp.miesiac;
                        this.AddPaczka.IsEnabled = true;
                        this.GridViewWyborPaczki.SelectedItem = this.GridViewWyborPaczki.Items[this.GridViewWyborPaczki.Items.Count - 1];

                    };

            }
            catch (Exception ex)
            {

                ErrorWindow.CreateNew(ex);
            }
            
        }

        private void AddPaczka_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GridViewWyborPaczki_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {

        }

        private void GridViewWyborPaczki_Loaded(object sender, RoutedEventArgs e)
        {
            this.vw_ListaPaczekdds.QueryParameters[0].Value = UserProfile.IdJednostki;
            this.vw_ListaPaczekdds.Load();
        }

        
      
    }
}

