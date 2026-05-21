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
using System.ServiceModel.DomainServices.Client;
using System.Windows.Browser;
using Telerik.Windows.Controls;
using System.IO;
using Telerik.Windows.Controls.GridView;
using LexEnaTrs;
using System.Text;
using System.Collections.ObjectModel;

namespace LexEnaTrs.Views
{
    public partial class UmorzeniaZgony : UserControl
    {
        private int CurrentSprID;
        private int CurrentDokID;
       

        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int TypDok = -1;


        public UmorzeniaZgony()
        {
            CurrentSprID = 0;
            CurrentDokID = 0;
            InitializeComponent();
            this.rdpData.SelectedDate = DateTime.Today;
        }







        private void SprawyGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {



            ZgonyHeader lst;


            if (e.AddedItems.Count > 0)
            {
                lst = (ZgonyHeader)e.AddedItems[0];
                CurrentSprID = lst.ZgonyHeader_Id;
                BIGvw_ImportyDetaildds.QueryParameters[0].Value = lst.ZgonyHeader_Id;
                BIGvw_ImportyDetaildds.Load();
            }


        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            try
            {
                this.GetBIG_ImportByDatedds.PageSize = 300;//1000;



                // Tu ustaiwć widoczność poszczególnych kolumn this.SprawyGridView.Columns[""]
                this.GetBIG_ImportByDatedds.Load();
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex.Message.ToString() + " Błąd odczytu danych ");

            }
        }







        private void ImportFileCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length > 0)
            {
                ErrorWindow.CreateNew(message);
            }

            this.GetBIG_ImportByDatedds.Load();




        }


        private void ImportFileXLSXCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length == 0 || message.ToLower().StartsWith("error"))
            {
                ErrorWindow.CreateNew(message);
                return;
            }
            this.GetBIG_ImportByDatedds.Load();
            AlertMsg.Show("Przetwarzanie danych -OK. Przeliczanie odsetek w toku");
            
        }

        private void RecalcCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;
            this.BusyIndicator.IsBusy = false;
            if (message.Length == 0 || message.ToLower().StartsWith("error"))
            {
                ErrorWindow.CreateNew(message);
                return;
            }
            this.GetBIG_ImportByDatedds.Load();
            
            AlertMsg.Show("Przeliczanie odsetek w toku. Przyciśnij odśwież by sprawdzić stan");

        }



        private void GetBIG_ImportByDate_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {
            e.LoadBehavior = LoadBehavior.RefreshCurrent;
        }



       

        private void BIGvw_ImportyDetaildds_LoadingData(object sender, Telerik.Windows.Controls.DomainServices.LoadingDataEventArgs e)
        {

        }




     

        private void SendToKrd_Click(object sender, RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
                ZgonyHeader zh = (ZgonyHeader)this.SprawyGridView.SelectedItem;
                ReportWindow repwin = new ReportWindow();
                repwin.IdPaczki = zh.ZgonyHeader_Id;
                repwin.DataDo = DateTime.Today;
                repwin.Mode = 5001;
                repwin.Show();
            }
            else
            {
                AlertMsg.Show("Musisz wybrać zestaw, dla którgo który generujesz raport");

            }

        }

      




        private void btDelImport_Click(object sender, RoutedEventArgs e)
        {
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {
            
                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.OkButtonContent = "Tak";
                dlgparm.Content = "Czy na pewno chcesz usunąć wybrany zestaw ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confdeluwndClose;
                RadWindow.Confirm(dlgparm);
            }
        }

        private void confdeluwndClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {

                ZgonyHeader bi = (ZgonyHeader)this.SprawyGridView.SelectedItem;
                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                _pozewcontext.ZgonyDelete(bi.ZgonyHeader_Id, delPakietCompleted, null);
            }
        }

        private void delPakietCompleted(InvokeOperation<string> result)
        {

            string message = result.Value;

            if (message.Length > 0)

            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(message);

            }

            this.GetBIG_ImportByDatedds.Loaded += GetBIG_ImportByDatedds_Loaded;
            this.GetBIG_ImportByDatedds.Load();
            this.BusyIndicator.IsBusy = false;



        }

        private void GetBIG_ImportByDatedds_Loaded(object sender, RoutedEventArgs e)
        {
            this.BusyIndicator.IsBusy = false;
            if (this.SprawyGridView.SelectedItem == null)
            {
                if (this.SprawyGridView.Items.Count == 0)
                {
                    BIGvw_ImportyDetaildds.QueryParameters[0].Value = 0;
                    BIGvw_ImportyDetaildds.Load();
                }
                else
                {
                    (this.SprawyGridView.Items).MoveCurrentToFirst();

                }
            }
            //throw new NotImplementedException();
        }

       


        

        private void btImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            PozewDomainContext _pozewcontext = null;


            // Set Filter to browser text files
            dlg.Filter = "Zbiory XLSX (*.xlsx)|*.xlsx|Wszystkie zbiory (*.*)|*.*";
            try
            {
                bool? result = dlg.ShowDialog();
                if (result.HasValue && result.Value)
                {

                    using (FileStream stream = dlg.File.OpenRead())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            byte[] xlsxfile = ms.ToArray();
                            this.BusyIndicator.IsBusy = true;
                            if (_pozewcontext == null)
                             _pozewcontext = new PozewDomainContext();
                             _pozewcontext.ImportDocumentUmorzZgonXLSX(Convert.ToBase64String(xlsxfile), UserProfile.Imie + " " + UserProfile.Nazwisko,rdpData.SelectedDate.Value ,0, ImportFileXLSXCompleted, null);
                            
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.BusyIndicator.IsBusy = false;
                ErrorWindow.CreateNew(ex, "Błąd odczytu zbioru");
            }


        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                this.GetBIG_ImportByDatedds.PageSize = 300;//1000;



                // Tu ustaiwć widoczność poszczególnych kolumn this.SprawyGridView.Columns[""]
                this.GetBIG_ImportByDatedds.Load();
                
                
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex.Message.ToString() + " Błąd odczytu danych ");

            }
        }

        private void PrzeliczOdsetki_Click(object sender, RoutedEventArgs e)
        {

            if (SprawyGridView.SelectedItems.Count > 0)
            {
                ZgonyHeader lst = (ZgonyHeader)SprawyGridView.SelectedItem;
                

                PozewDomainContext _pozewcontext = null;
                if (_pozewcontext == null)
                    _pozewcontext = new PozewDomainContext();
                _pozewcontext.ImportDocumentUmorzZgonXLSX("", UserProfile.Imie + " " + UserProfile.Nazwisko, rdpData.SelectedDate.Value, lst.ZgonyHeader_Id , RecalcCompleted , null);
            }
            else
                AlertMsg.Show("Musisz wskazać zestaw, dla którego wznowić przeliczanie odsetek");
        }

        private void DodajZlecenia_Click(object sender, RoutedEventArgs e)
        {
            
            if (this.SprawyGridView.SelectedItems.Count > 0)
            {

                DialogParameters dlgparm = new DialogParameters();
                dlgparm.CancelButtonContent = "Nie";
                dlgparm.OkButtonContent = "Tak";
                dlgparm.Content = "Czy na pewno chcesz założyć zlecenia dla wybranego zestawu ?";
                dlgparm.Header = "Potwierdź";
                dlgparm.Closed = confZleceniaClose;
                RadWindow.Confirm(dlgparm);
            }
       
    }
        private void confZleceniaClose(object sender, WindowClosedEventArgs e)
        {
            if ((sender as RadWindow).DialogResult == true)
            {

                ZgonyHeader bi = (ZgonyHeader)this.SprawyGridView.SelectedItem;
                PozewDomainContext _pozewcontext;
                _pozewcontext = new PozewDomainContext();
                this.BusyIndicator.IsBusy = true;
                _pozewcontext.ZgonyZlecenia(bi.ZgonyHeader_Id, this.rdpData.SelectedDate.Value, delPakietCompleted, null);
            }
        }
    }
}
    

   
