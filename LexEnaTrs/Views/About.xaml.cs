namespace LexEnaTrs
{
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using LexEnaTrs.Web;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Reflection;

    /// <summary>
    /// <see cref="Page"/> class to present information about the current application.
    /// </summary>
    public partial class About : Page
    {
        private SSISDomainContext kontext = new SSISDomainContext();
        private EpuContext epuKontext = new EpuContext();
        /// <summary>
        /// Creates a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {

           // kontext.WstawZadanieDoKolejki("Wiena2LexEnaTest", 1, RegistrationOperation_Completed, null);   // nazwa odpowiada nazwie pakietu

            int wynik2 = 1;
        }
        private void RegistrationOperation_Completed(InvokeOperation<int> wynik)
        {
            int wynik3 = wynik.Value;

        }

        private void button2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //kontext.GetZadanieStatus(8, Completed_Task, null);
            // epuKontext.MojeSprawy(
        }


        private void Completed_Task(InvokeOperation<int> wynik)
        {
            int wynik5 = wynik.Value;
            int wynik200 = 1;
        }


        private void Compl_task(InvokeOperation<StatusZadania> status)
        {
            StatusZadania stzad = status.Value;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // wersja aplikacji
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] parts = asm.FullName.Split(',');
            string version = parts[1];
            this.version.Text = version;
            PozewDomainContext domainContext = new PozewDomainContext();
            domainContext.GetDbNames(getDbNamesCompleted, null);

        }

        private void getDbNamesCompleted(InvokeOperation<string> result)
        {
            if (!result.HasError)
            {
                string message = result.Value;
                this.lexenadbname.Text = message;

            }
        }

    }
}