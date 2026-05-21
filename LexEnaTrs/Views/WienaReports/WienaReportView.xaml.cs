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
using Telerik.ReportViewer.Silverlight;
using Telerik.Reporting.Service.SilverlightClient;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace LexEnaTrs.Views
{
    public partial class WienaReports : UserControl, IReportServiceClientFactory
    {


        public int IdUser = -1;  // do Query oraz do   - domyślniue wartosci dowolne
        public int IdJednostki = -1;
        public int StatusDok = -1;
        public int TypRaport = -1;

        private Guid guid = Guid.Empty;
        private string oddzialStr;
        private string statusStr;
        private int firma;
        private int mode = -1;  // nie wykonuj     
        private DateTime d_od;
        private DateTime d_do;
        private int StanNalObliczPakietId = 0;

        private ObservableCollection<typSlownikFilter> lst;

        public WienaReports()
        {

            InitializeComponent();
            this.WienaReportViewer.ReportServiceClientFactory = this;

            ObservableCollection<typSlownikFilter> lst1 = this.OddzialyGrid.ItemsSource as ObservableCollection<typSlownikFilter>;
            if (lst1 != null)
            {
                foreach (var item in lst1)
                {
                    item.Filter3 = 1;

                }
                this.OddzialyGrid.ItemsSource = null;
                this.OddzialyGrid.ItemsSource = lst1;
            }

            ObservableCollection<typSlownikFilter> lst2 = this.StatusyGrid.ItemsSource as ObservableCollection<typSlownikFilter>;
            if (lst2 != null)
            {
                foreach (var item in lst2)
                {
                    item.Filter3 = 1;

                }
                this.StatusyGrid.ItemsSource = null;
                this.StatusyGrid.ItemsSource = lst2;
            }
            this.rdpOd.SelectedValue = new DateTime(DateTime.Today.Year, 1, 1);
            this.rdpDo.SelectedValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);

            // wienaConfig wwc = new wienaConfig();
            //  lst = wwc.typOddzialyWiena;
            //  this.OddzialyGrid.ItemsSource = lst;
            // this.StatusyGrid.ItemsSource = lst;
        }

        ReportServiceClient IReportServiceClientFactory.Create(Uri remoteAddress)
        {
            Binding binding = new BasicHttpBinding()
            {
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                CloseTimeout = new TimeSpan(0, 10, 0),
                MaxBufferSize = Int32.MaxValue,
                MaxReceivedMessageSize = Int32.MaxValue
            };
            EndpointAddress endpointAddress = new EndpointAddress(remoteAddress);

            return new ReportServiceClient(binding, endpointAddress);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                lst = this.OddzialyGrid.ItemsSource as ObservableCollection<typSlownikFilter>;

            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew("Błąd mapowania " + ex.Message);
            }

        }

        private void HeaderCheckBox_Click(object sender, RoutedEventArgs e)
        {
            lst = this.OddzialyGrid.ItemsSource as ObservableCollection<typSlownikFilter>;
            foreach (typSlownikFilter i in lst)
            {
                if (i.Filter3 == 0)
                    i.Filter3 = 1;
                else
                    i.Filter3 = 0;


            }
            this.OddzialyGrid.ItemsSource = null;
            this.OddzialyGrid.ItemsSource = lst;

        }

        private void HeaderStatusCheckBox_Click(object sender, RoutedEventArgs e)
        {
            lst = this.StatusyGrid.ItemsSource as ObservableCollection<typSlownikFilter>;
            foreach (typSlownikFilter i in lst)
            {
                if (i.Filter3 == 0)
                    i.Filter3 = 1;
                else
                    i.Filter3 = 0;


            }
            this.StatusyGrid.ItemsSource = null;
            this.StatusyGrid.ItemsSource = lst;

        }

        private string getOddzialy()
        {
            string result = String.Empty;
            lst = this.OddzialyGrid.ItemsSource as ObservableCollection<typSlownikFilter>;
            foreach (typSlownikFilter i in lst)
            {
                if (i.Filter3 == 1)
                {
                    if (!String.IsNullOrWhiteSpace(result)) result += ",";
                    result += i.Numer.ToString();
                }
            }
            return result;
        }

        private string getKonta()
        {
            string result = String.Empty;
            lst = this.KontaGrid.ItemsSource as ObservableCollection<typSlownikFilter>;
            foreach (typSlownikFilter i in lst)
            {
                if (i.Filter3 == 1)
                {
                    if (!String.IsNullOrWhiteSpace(result)) result += ";";
                    result += i.Numer.ToString();
                }
            }
            return result;
        }
        private string getKontaNames()
        {
            string result = String.Empty;
            lst = this.KontaGrid.ItemsSource as ObservableCollection<typSlownikFilter>;
            foreach (typSlownikFilter i in lst)
            {
                if (i.Filter3 == 1)
                {
                    if (!String.IsNullOrWhiteSpace(result)) result += ";";
                    result += i.Nazwa.ToString();
                }
            }
            return result;
        }

        private string getOddzialyNames()
        {
            string result = String.Empty;
            lst = this.OddzialyGrid.ItemsSource as ObservableCollection<typSlownikFilter>;
            foreach (typSlownikFilter i in lst)
            {
                if (i.Filter3 == 1)
                {
                    if (!String.IsNullOrWhiteSpace(result)) result += ",";
                    result += i.Nazwa;
                }
            }
            return result;
        }


        private string getStatusy()
        {
            string result = String.Empty;
            lst = this.StatusyGrid.ItemsSource as ObservableCollection<typSlownikFilter>;
            foreach (typSlownikFilter i in lst)
            {
                if (i.Filter3 == 1)
                {
                    if (!String.IsNullOrWhiteSpace(result)) result += ",";
                    result += i.Numer.ToString();
                }
            }
            return result;
        }

        private void SetReportArgs(ref Telerik.ReportViewer.Silverlight.RenderBeginEventArgs args)
        {

            /*
             * 
             * 
             * @data_s smalldatetime,
@typ_firmy int,
@ai_firma as varchar(255),
@stat_tab as varchar(255),
@ai_sel as int
            */

            if (this.TypRaport == 2 || this.TypRaport == 3 || this.TypRaport == 5 || this.TypRaport == 7 || this.TypRaport == 8)
            {
                args.ParameterValues["typfirmy"] = UserProfile.Firma;
                args.ParameterValues["datas"] = this.rdpOd.SelectedValue.Value;
                args.ParameterValues["aifirma"] = getOddzialy();
                args.ParameterValues["stattab"] = getStatusy();
                args.ParameterValues["aisel"] = mode;
                if (this.TypRaport == 5)
                {
                    args.ParameterValues["aikonta"] = getKonta();

                }
                if (this.TypRaport == 8)
                {
                    args.ParameterValues["dod"] = this.rdpOdStatus.SelectedValue.Value;
                    args.ParameterValues["ddo"] = this.rdpDoStatus.SelectedValue.Value;
                }
            }
            else if (this.TypRaport == 3)
            {
                args.ParameterValues["typfirmy"] = -1;
                args.ParameterValues["aifirma"] = getOddzialy();
                args.ParameterValues["stattab"] = getStatusy();
                args.ParameterValues["aisel"] = mode;
            }
            else if (this.TypRaport > 10 && TypRaport < 20) // wiekowanie należności
            {
                args.ParameterValues["dod"] = this.rdpOd.SelectedValue.Value;
                args.ParameterValues["ddo"] = this.rdpDo.SelectedValue.Value;
                args.ParameterValues["aifirma"] = getOddzialy();
                args.ParameterValues["aisel"] = mode;

            }
            else if (this.TypRaport == 9)
            {
                args.ParameterValues["dod"] = this.rdpOd.SelectedValue.Value;
                args.ParameterValues["ddo"] = this.rdpDo.SelectedValue.Value;
                args.ParameterValues["aifirma"] = getOddzialy();
                args.ParameterValues["aisel"] = mode;
                args.ParameterValues["ListaKont"] = getKonta();
                args.ParameterValues["asdowod"] = (cbTypDok.SelectedValue == null ? "" : cbTypDok.SelectedValue);
                args.ParameterValues["kontaString"] = getKontaNames();
                args.ParameterValues["oddzialyString"] = getOddzialyNames();

            }
            else if (this.TypRaport == 10)
            {
                args.ParameterValues["dod"] = this.rdpOd.SelectedValue.Value;
                args.ParameterValues["ddo"] = this.rdpDo.SelectedValue.Value;

            }
            else if (this.TypRaport == 8)
            {
                args.ParameterValues["dod"] = this.rdpOdStatus.SelectedValue.Value;
                args.ParameterValues["ddo"] = this.rdpDoStatus.SelectedValue.Value;

            }
            else if (this.TypRaport == 101)
            {
                args.ParameterValues["ddo"] = this.rdpDo.SelectedValue.Value;
                args.ParameterValues["aifirma"] = getOddzialy();
                args.ParameterValues["aisel"] = mode;
                args.ParameterValues["ListaKont"] = getKonta();
                args.ParameterValues["asdowod"] = (String.IsNullOrWhiteSpace(tbDocs.Text) ? "" : tbDocs.Text);
                args.ParameterValues["kontaString"] = getKontaNames();
                args.ParameterValues["oddzialyString"] = getOddzialyNames();


            }
            else
            {
                args.ParameterValues["dod"] = this.rdpOd.SelectedValue.Value;
                args.ParameterValues["ddo"] = this.rdpDo.SelectedValue.Value;
                args.ParameterValues["radca"] = this.cbRadca.SelectedValue ?? 0;
                args.ParameterValues["aifirma"] = getOddzialy();
                args.ParameterValues["asdowod"] = this.tbDocs.Text;
                args.ParameterValues["stattab"] = getStatusy();
                args.ParameterValues["aisel"] = mode;
            }
        }
        private void SetReport()
        {
            string assemblyName;
            switch (this.TypRaport)
            {
                case 2:
                    assemblyName = "LexEnaReporting.EOP.Naleznosci.Naleznosci, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;
                case 3:
                    assemblyName = "LexEnaReporting.EOP.Raty.Raty, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;
                case 4:
                    assemblyName = "LexEnaReporting.EOP.KosztyRadcow.KosztyRadcow, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;

                case 5:  // do poprawy
                    if (this.cbxDetails.IsChecked.Value)
                        assemblyName = "LexEnaReporting.EOB.WiekowanieSaldperSprawa.WiekowaneSaldperSprawa, LexEnaReporting";
                    else
                        assemblyName = "LexEnaReporting.EOB.WiekowanieSald.WiekowaneSald, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;

                case 6:
                    assemblyName = "LexEnaReporting.EOB.WgDokumentu.WgDokumentu, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;
                case 7:  
                    if (this.cbxDetails.IsChecked.Value)
                        assemblyName = "LexEnaReporting.EOB.WiekowanieNal.WiekowaneNalperSprawa, LexEnaReporting";
                    else
                        assemblyName = "LexEnaReporting.EOB.WiekowanieNal.WiekowaneNal, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;
                case 8:
                    assemblyName = "LexEnaReporting.EOP.RodzajeObciazen.RodzajeObciazen, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;
                case 9:
                    assemblyName = "LexEnaReporting.EOP.Oplaty.OplatyObroty, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;
                case 10:
                    assemblyName = "LexEnaReporting.EOP.WiekowanieWplat.WiekowanieWplat, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;

                case 11:
                    assemblyName = "LexEnaReporting.EOB.WiekowanieOS.WiekowanieOS, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;
                case 12:
                    assemblyName = "LexEnaReporting.EOB.WiekowanieES.WiekowanieES, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;
                case 13:
                    assemblyName = "LexEnaReporting.EOB.WiekowanieNS.WiekowanieNS, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;
                case 14:
                    assemblyName = "LexEnaReporting.EOB.WiekowanieKS.WiekowanieKS, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;

                case 101:
                    assemblyName = "LexEnaReporting.EOP.Salda.SaldaOryg, LexEnaReporting";
                    this.WienaReportViewer.Report = assemblyName;
                    break;
            }
        }

        private void WienaReportViewer_RenderBegin(object sender, Telerik.ReportViewer.Silverlight.RenderBeginEventArgs args)
        {
            SetReport();
            SetReportArgs(ref args);
            this.WienaReportViewer.RenderBegin -= WienaReportViewer_RenderBegin;
        }


        private void reportWiekowanieSaldPrequisites()
        {
            PozewDomainContext _context = new PozewDomainContext();
            _context.CalculateWiekowanieSald(getOddzialy().Replace(",", ";"), getKonta().Replace(",",";"), this.rdpOd.SelectedValue.Value, UserProfile.Firma, CalculateWiekowanieSaldCompleted, null);



        }

        private void CalculateWiekowanieSaldCompleted(InvokeOperation<string> result)
        {
            string message;
            if ((result != null && result.HasError) || (result != null && !String.IsNullOrWhiteSpace(result.Value)))
            {

                ErrorWindow.CreateNew("Błąd " + ((result != null && !String.IsNullOrWhiteSpace(result.Value) )? result.Value : ""));
                return;

            }
            this.WienaReportViewer.RefreshReport();
        }

        private void wiekowanieNalPrequisites()
        {
             this.StanNalObliczPakietId = 0;
            PozewDomainContext _context = new PozewDomainContext();
            _context.CalculateWiekowanieNal(getOddzialy().Replace(",", ";"), getStatusy().Replace(",", ";"), this.rdpOd.SelectedValue.Value, UserProfile.Firma, CalculateWiekowanieNalCompleted, null);



        }

        private void CalculateWiekowanieNalCompleted(InvokeOperation<string> result)
        {
            string message;
            if ((result != null && result.HasError) || (result != null && (!String.IsNullOrWhiteSpace(result.Value) && result.Value.StartsWith("Błąd") )))
            {

                ErrorWindow.CreateNew("Błąd " + ((result != null && !String.IsNullOrWhiteSpace(result.Value)) ? result.Value : ""));
                return;

            }
            this.StanNalObliczPakietId = Convert.ToInt32(result.Value);
            btProgress.Visibility = Visibility.Visible;
            AlertMsg.Show("Przeliczanie zaległości w toku. Wybierz przycisk \"Sprawdź postęp obliczeń\"");
        }

        private void btRun_Click(object sender, RoutedEventArgs e)
        {

            this.mode = 1; 
            this.WienaReportViewer.RenderBegin += new Telerik.ReportViewer.Silverlight.RenderBeginEventHandler(WienaReportViewer_RenderBegin);
            if (this.TypRaport == 5) // jełśi wiekowanie 
            {
                reportWiekowanieSaldPrequisites();
                return;
            }
            else if (this.TypRaport == 7)
            {
                wiekowanieNalPrequisites();

            }
            else
                this.WienaReportViewer.RefreshReport();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.TypRaport == 2 || this.TypRaport == 3 || this.TypRaport == 5 || this.TypRaport==7 || this.TypRaport == 8)
            {
                
                rdpDo.Visibility = Visibility.Collapsed;
                labelKancelaria.Visibility = Visibility.Collapsed;
                labelPomin.Visibility = Visibility.Collapsed;
                tbDocs.Visibility = Visibility.Collapsed;
                cbRadca.Visibility = Visibility.Collapsed;
                if (TypRaport == 3) // raty
                {
                    this.rdpOd.Visibility = Visibility.Collapsed;
                    label2.Content = ""; 
                }
                else
                {
                    label2.Content = "Stan na dzień";
                    this.rdpOd.SelectedValue = DateTime.Today;
                }
                if (this.TypRaport == 5 )
                {
                    this.StatusyGrid.Visibility = Visibility.Collapsed;
                    this.cbxDetails.Visibility = Visibility.Visible;
                    this.KontaGrid.Visibility = Visibility.Visible;

                }
                if (this.TypRaport == 7)
                {
             
                    this.cbxDetails.Visibility = Visibility.Visible;

                }
                if (this.TypRaport == 8)
                {
                    labelStatusOdDo.Visibility = Visibility.Visible;
                    rdpOdStatus.Visibility = Visibility.Visible;
                    labelkreskaOdDo.Visibility = Visibility.Visible;
                    rdpDoStatus.Visibility = Visibility.Visible;
                    rdpDoStatus.SelectedValue = DateTime.Today;
                    rdpOdStatus.SelectedValue = new DateTime(2000, 1, 1);

                }
            }
            if (this.TypRaport == 6)
            {
                labelKancelaria.Visibility = Visibility.Collapsed;
                labelPomin.Content = "Sprawy z dokumentami";
                tbDocs.Text = "ugoda;raty";
                rdpDo.SelectedValue = DateTime.Today;
                rdpOd.SelectedValue = new DateTime(2000, 1, 1);
               cbRadca.Visibility = Visibility.Collapsed;

            }
            if (this.TypRaport == 9)
            {

                labelKancelaria.Visibility = Visibility.Collapsed;
                labelPomin.Visibility = Visibility.Collapsed;
                tbDocs.Visibility = Visibility.Collapsed;
                cbRadca.Visibility = Visibility.Collapsed;
                StatusyGrid.Visibility = Visibility.Collapsed;
                rdpDo.SelectedValue = DateTime.Today;
                rdpOd.SelectedValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                KontaGrid.Visibility = Visibility.Visible;
                cbTypDok.Visibility = Visibility.Visible;
                labelRodzajDok.Visibility = Visibility.Visible;
            }
            if (this.TypRaport == 10) 
            {

                labelKancelaria.Visibility = Visibility.Collapsed;
                labelPomin.Visibility = Visibility.Collapsed;
                tbDocs.Visibility = Visibility.Collapsed;
                cbRadca.Visibility = Visibility.Collapsed;
                StatusyGrid.Visibility = Visibility.Collapsed;
                rdpDo.SelectedValue = new DateTime(DateTime.Today.Year - 1, 12, 31);
                rdpOd.SelectedValue = new DateTime(DateTime.Today.Year - 1, 1, 1);
                KontaGrid.Visibility = Visibility.Collapsed;
                cbTypDok.Visibility = Visibility.Collapsed;
                labelRodzajDok.Visibility = Visibility.Collapsed;
                OddzialyGrid.Visibility = Visibility.Collapsed;
            }

            if (this.TypRaport > 10 && TypRaport <= 20) // Wiekowanie należności
            {
                labelKancelaria.Visibility = Visibility.Collapsed;
                labelPomin.Visibility = Visibility.Collapsed;
                tbDocs.Visibility = Visibility.Collapsed;
                cbRadca.Visibility = Visibility.Collapsed;
                StatusyGrid.Visibility = Visibility.Collapsed;

            }
            if (this.TypRaport == 101)
            {
                label2.Content = "Stan na dzień";
                labelKancelaria.Visibility = Visibility.Collapsed;
                cbRadca.Visibility = Visibility.Collapsed;
                StatusyGrid.Visibility = Visibility.Collapsed;
                rdpDo.SelectedValue = DateTime.Today;
                rdpOd.Visibility = Visibility.Collapsed;
                KontaGrid.Visibility = Visibility.Visible;
                tbDocs.Visibility = Visibility.Collapsed;
                labelPomin.Visibility = Visibility.Collapsed;
                cbTypDok.Visibility = Visibility.Collapsed;
                labelRodzajDok.Visibility = Visibility.Collapsed;
                tbDocs.Text = string.Empty;
            }

            this.WienaReportViewer.RenderBegin += new Telerik.ReportViewer.Silverlight.RenderBeginEventHandler(WienaReportViewer_RenderBegin);
        }

        private void KontaHeaderCheckBox_Click(object sender, RoutedEventArgs e)
        {
            lst = this.KontaGrid.ItemsSource as ObservableCollection<typSlownikFilter>;
            foreach (typSlownikFilter i in lst)
            {
                if (i.Filter3 == 0)
                    i.Filter3 = 1;
                else
                    i.Filter3 = 0;


            }
            this.KontaGrid.ItemsSource = null;
            this.KontaGrid.ItemsSource = lst;
        }

        private void btProgress_Click(object sender, RoutedEventArgs e)
        {

            if (this.StanNalObliczPakietId > 0)
            {
                PozewDomainContext _context = new PozewDomainContext();
                _context.GetCalcZalegloscProgress(StanNalObliczPakietId, GetCalcZalegloscProgressCompleted, null);


            }

        }

        private void GetCalcZalegloscProgressCompleted(InvokeOperation<string> result)
        {
            string message;
            if ((result != null && result.HasError) || (result != null && (!String.IsNullOrWhiteSpace(result.Value) && result.Value.StartsWith("Błąd"))))
            {

                ErrorWindow.CreateNew("Błąd " + ((result != null && !String.IsNullOrWhiteSpace(result.Value)) ? result.Value : ""));
                return;

            }
            if (String.IsNullOrWhiteSpace(result.Value))
            {
                this.mode = this.StanNalObliczPakietId;
                WienaReportViewer.RefreshReport();

            } 
            else
                AlertMsg.Show(result.Value);
        }

        private void cbxDetails_Checked(object sender, RoutedEventArgs e)
        {
            string assemblyName;

            if (this.TypRaport == 5)
            {
                if (this.cbxDetails.IsChecked.Value)
                    assemblyName = "LexEnaReporting.EOB.WiekowanieSaldperSprawa.WiekowaneSaldperSprawa, LexEnaReporting";
                else
                    assemblyName = "LexEnaReporting.EOB.WiekowanieSald.WiekowaneSald, LexEnaReporting";

                this.WienaReportViewer.Report = assemblyName;
            }

            this.WienaReportViewer.RefreshReport();
        }

        private void KontaGrid_SelectedCellsChanged(object sender, GridViewSelectedCellsChangedEventArgs e)
        {
            ObservableCollection<typSlownikFilter> ds = (ObservableCollection<typSlownikFilter>)this.KontaGrid.ItemsSource;
            int count = 0;
            foreach (var row in ds.ToList())
            {
                if (row.Filter3 == 1)
                {
                    count++;

                }
            }
            tbCounter.Text = count.ToString();
            
        }
    }
     

      
    }
    

   
