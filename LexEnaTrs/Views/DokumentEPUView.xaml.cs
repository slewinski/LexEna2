using System;
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
using System.Collections.ObjectModel;
using Telerik.Windows.Controls.GridView;
using System.Windows.Markup;
using System.Threading;
using System.Windows.Browser;
using LexEnaTrs;
using LexEnaTrs.Web;
using System.ServiceModel.DomainServices.Client;
using System.IO;
using System.ComponentModel;


namespace LexEnaTrs.Views 
{
    public partial class DokumentEPUView : UserControl
    {
        private DokumentEPU dokument;
        private PozewDomainContext _dokcontext;
        
        public event EventHandler dokumentEPUvalidated;


        private int saveActionType  = 0;

        protected virtual void OndokumentEPUValidated(pozewEventArgs e)
        {
            if (dokumentEPUvalidated != null)
                dokumentEPUvalidated(this, e);
        }

      
        public string dokumentEPUSerialized
        {
            get;
            set;

        }

        public int docStatus
        {
            get;
            set;

        }

        public bool statusChanged
        {
            get;
            set;

        }

        public int IdSprawy
        {
            get;
            set;
        }

        public int IdDoc
        {
            get;
            set;
        }

        public string serializeDokumentEPU()
        {
            this.dokumentEPUSerialized = ToXMLSerializers.SerializeToString(dokument,typeof(DokumentEPU));
            // oblicznaie sum
            
            return this.dokumentEPUSerialized;
        }




        public DokumentEPUView()
        {




            InitializeComponent();
            this.statusChanged = false;
        }

        public void ValidateDokumentEPU()
        {
            if (_dokcontext == null)
                _dokcontext = new PozewDomainContext();
            _dokcontext.ValidateDokEPUXSD(this.dokumentEPUSerialized, dokument.Rodzaj, ValidateDokumentEPUCompleted, null);



        }


        private void ValidateDokumentEPUCompleted(InvokeOperation<string> message)
        {
            string msg;
            pozewEventArgs ea;
            msg = message.Value;
            ea = new pozewEventArgs();
            if (msg.Contains("error") || msg.Contains("Błąd"))
            {
                ea.Status = -1;
                ea.Message = msg;
            }
            else
            {
                ea.Status = 1;
                ea.Message = "OK";

            }
            OndokumentEPUValidated(ea);
        }

       
        private void ToXMLButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            string s;
            try
            {
    
                s = ToXMLSerializers.SerializeToString(dokument,typeof(DokumentEPU));
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show(s);
        }


        private void PokazButtonClicked()
        {
            this.ShowDocs.IsEnabled = false;
            dokumentEPUSerialized = ToXMLSerializers.SerializeToString(dokument, typeof(DokumentEPU));
            if (_dokcontext == null)
                _dokcontext = new PozewDomainContext();
            _dokcontext.ValidateDokEPUXSD(dokumentEPUSerialized,dokument.Rodzaj, DokumentEPUValidated, null);

        }


        private void DokumentEPUValidated(InvokeOperation<string> message)
        {
            string msg;
            msg = message.Value;
            if (msg.Contains("error") || msg.Contains("Błąd"))
            {
                ErrorWindow.CreateNew(msg);
                this.ShowDocs.IsEnabled = true;
                return;
            }
            switch (saveActionType)
	        {
                case 0:
                         _dokcontext.DokumentZEPU2HTML(dokumentEPUSerialized, dokument.Rodzaj + 1000, PozewEPUCompleted, null);  // wyświetlenie zbiou html
                         break;
                case 1:
                         _dokcontext.DokumentZEPU2HTML(dokumentEPUSerialized,dokument.Rodzaj + 1000 ,PozewToSaveCompleted, null);  // wyświetlenie zbiou html
                         break;
		        default:
                         _dokcontext.DokumentZEPU2HTML(dokumentEPUSerialized,dokument.Rodzaj + 1000 ,PozewEPUCompleted, null);  // wyświetlenie zbiou html
                         break;
	        }

                          
        }

        private void PozewEPUCompleted(InvokeOperation<string> html)
        {

            string htmlpath;
            this.ShowDocs.IsEnabled = true;
            htmlpath = html.Value;//e.Result.ToString();
            Uri uri = new Uri(Application.Current.Host.Source, htmlpath);
            
            HtmlWindow window = HtmlPage.Window;
            window.Navigate(uri, "_blank");
            
        }

        private void PozewToSaveCompleted(InvokeOperation<string> html)
        {

            string htmlpath;
            
            htmlpath = html.Value;//e.Result.ToString();
            Uri uri = new Uri(Application.Current.Host.Source, htmlpath);
            DownloadManager dwnMgr  = new DownloadManager();
            dwnMgr.downloadButton = this.ShowDocs;
            dwnMgr.ServerFileUri = uri.AbsoluteUri;
            dwnMgr.DownloadnSave(0);
            

        }



      





        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            QueryableCollectionView qcvListaPowodow;
            QueryableCollectionView qcvListaPozwanych;


            if (this.dokumentEPUSerialized != null)
            {
        
                dokument = ToXMLSerializers.XmlDeserializeFromString<DokumentEPU>(this.dokumentEPUSerialized);

                if (dokument.ListaPowodow != null)
                {
                    qcvListaPowodow = new QueryableCollectionView(dokument.ListaPowodow); // <typDowod>(typDowod.GetDowody());
                    this.DataFormPowod.ItemsSource = qcvListaPowodow;
                    foreach (var pw in qcvListaPowodow)
                        (pw as typStrona).CzyPowod = true;

                }
                if (dokument.ListaPozwanych != null)
                {
                    qcvListaPozwanych = new QueryableCollectionView(dokument.ListaPozwanych);
                    foreach (var pw in qcvListaPozwanych)
                        (pw as typStrona).CzyPowod = false;
                    this.DataFormPozwani.ItemsSource = qcvListaPozwanych;
                    this.RadGridListaPozwanych.ItemsSource = qcvListaPozwanych;

                }
                                        
                    this.LayoutRoot.DataContext = dokument;
                   
                    this.DataFormSkladajacy.CurrentItem = dokument.OsobaSkladajaca;
                    this.vw_KomunikacjaDocWysdds.QueryParameters[0].Value = IdDoc;
                    this.vw_KomunikacjaDocWysdds.Load();
            
        
            }
        }



        private void DataFormPozwani_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            typStrona ts;
            ts = e.NewItem as typStrona;
            ts.CzyPowod = false;
            ts.Adres.Add(new typAdres());
        }

        private void DataFormPowod_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            typStrona ts;
            ts = e.NewItem as typStrona;
            ts.CzyPowod = true;
            ts.Adres.Add(new typAdres());

        }

       



     


        private void OpenSpr_Click(object sender, System.Windows.RoutedEventArgs e)
        {


            SprawaWindow sprwindow = new SprawaWindow();
            sprwindow.ViewSprawa.IdSprawy = (int)dokument.IDSprawy;
            sprwindow.Show();

        }

        public void SaveContent(object sender)
        {

            rodzajeDokumentowEPU rodo;
            try
            {
                if (sender != null)    (sender as Button).IsEnabled = false;
                // zapisanie pozwu do bazy 
                LexEnaMeritumDomainContext _context = new LexEnaMeritumDomainContext();
                LoadOperation<DokWys> loadop;

                EntityQuery<DokWys> query =
                from c in _context.GetDokWysWithSprawaByIdQuery(this.IdDoc)
                select c;
                loadop = _context.Load(query);
                loadop.Completed += (sd, ea) =>
                {
                    DokWys dok;

                    dok = loadop.Entities.FirstOrDefault();
                    dok.StatusDok = this.docStatus;
                    dok.Tresc = this.dokumentEPUSerialized;
                    dok.DataDok = dokument.DataZlozenia;
                    dok.d_modyfikacji = DateTime.Now;
                    dok.modyfikator = UserProfile.Nazwisko + " " + UserProfile.Imie;
                    dok.TypDok = dokument.Rodzaj;
                    rodo = new rodzajeDokumentowEPU();
                    dok.Nazwa = rodo.GetNameByNumber(dokument.Rodzaj);
                    //             dok.DataDok = this. dWniesienia;
                    _context.SubmitChanges().Completed += (obj, evargs) =>
                    {

                        if (sender != null) (sender as Button).IsEnabled = true;
                    };



                };

            }

            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex);
                if ( sender != null) (sender as Button).IsEnabled = true;
            }
        
        
        }


            
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.serializeDokumentEPU();

        }

       

        private void ListaAkcji_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {   int index;
            this.ShowDocs.IsOpen = false;
            index = (sender as System.Windows.Controls.ListBox).SelectedIndex;
            (sender as System.Windows.Controls.ListBox).SelectedIndex = -1;
            if (e.AddedItems.Count > 0)
            {
                switch (index)
                {
                    case 0:
                        this.saveActionType = 0;  //   wyświetlenie  strony 
                        break;
                    case 1:
                        this.saveActionType = 1;  // Eksport
                        break;

                    default:
                        this.saveActionType = 0;

                        break;
                }
                PokazButtonClicked();
            }
        }

       

       
        
    }

        
    }

   

