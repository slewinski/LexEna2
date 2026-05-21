namespace LexEnaTrs
{
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Telerik.Windows.Controls.Navigation;
    using Telerik.Windows.Controls;
    using LexEnaTrs.Views;
    using System;

    /// <summary>
    /// Home page for the application.
    /// </summary>
    public partial class Admin : Page
    {
        /// <summary>
        /// Creates a new <see cref="Home"/> instance.
        /// </summary>
        public Admin()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AdminPageTitle;
        }

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void RadMenu_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadMenuItem Item;
            KontaEPUView kontaView;
            JednostkiKonfigView jednView;
            UsersKonfigView usrView;
            SlownikiView slWiew;
            ZadaniaView zdView;
            ListaKomornikow komView;
            SlownikiSposEgz sposView;
            OdsetkiUstawowe odsView;
            MapCzynView mpczyn;
            BigKrdKonfig bkk;
            UzDKonfig uzdKonf;


            Item = e.Source as RadMenuItem;
            switch (Item.Tag as string)
            {           
                case "KontaEPU":
                    kontaView = new KontaEPUView();
                    this.MainWorkspace.Child = kontaView ;
                    break;

                case "Jednostki":
                    jednView = new JednostkiKonfigView();
                    this.MainWorkspace.Child = jednView;
                    break;
                case "Users":
                    usrView = new UsersKonfigView();
                    this.MainWorkspace.Child = usrView;
                    break;
                case "Uzasadnienia":
                    slWiew = new SlownikiView();
                    slWiew.TypSlownika = 1 ; // uzasadnienia
                    this.MainWorkspace.Child = slWiew;
                    break;
                case "Fakty":
                    slWiew = new SlownikiView();
                    slWiew.TypSlownika = 2; // fakty stwierdzone
                    this.MainWorkspace.Child = slWiew;
                    break;
                case "Zadania":
                    zdView = new ZadaniaView();
                    this.MainWorkspace.Child = zdView;
                    break;
                case "Komornicy":  // komornicy 
                    komView = new ListaKomornikow();
                    this.MainWorkspace.Child = komView;
                    break;
                case "SzablonWniosku":
                    sposView = new SlownikiSposEgz();
                    this.MainWorkspace.Child =sposView;
                    break;
                case "OdsetkiUstawowe":
                    odsView = new OdsetkiUstawowe();
                    this.MainWorkspace.Child =odsView;
                    break;
                case "KrdKonfig":
                    bkk = new BigKrdKonfig();
                    this.MainWorkspace.Child = bkk;
                    break;

                case "TerminowoscPozwow":
                    ReportWindow repwin = new ReportWindow();
                    repwin.DataOd = new DateTime(DateTime.Today.Year, 1, 1);
                    repwin.DataDo = DateTime.Today;
                    repwin.Mode = 101;
                    repwin.Show();
                    break;

                case "CzynKomo":
                    mpczyn = new MapCzynView();
                    mpczyn.TypSlownika = 2; // fakty stwierdzone
                    this.MainWorkspace.Child = mpczyn;
                    break;
                case "UzDKonfig":
                    uzdKonf = new UzDKonfig();
                    this.MainWorkspace.Child = uzdKonf;
                    break;
                default:
                    break;
            }

        }
    }
}