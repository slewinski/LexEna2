namespace LexEnaTrs
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using LexEnaTrs.LoginUI;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using Telerik.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// <see cref="UserControl"/> class providing the main UI for the application.
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// Creates a new <see cref="MainPage"/> instance.
        /// </summary>
        public MainPage()
        {

            // StyleManager.ApplicationTheme = new Windows7Theme();


            InitializeComponent();
           // this.loginContainer.Child = new LoginStatus();
            LoginStatus ctrl = new LoginStatus();
            ctrl.LoginStateChange += new LoginStatus.LoginHandler(LoginStateChange);
            this.loginContainer.Child = ctrl;
            // this.loginContainer.Child = new LoginStatus();

            ContentBorder.Visibility = System.Windows.Visibility.Collapsed;
        }

        void LoginStateChange(object sender, AuthenticationEventArgs e)
        {
            /*  ContentBorder.Visibility = (e != null && e.User.Identity.IsAuthenticated) ?
                                 System.Windows.Visibility.Visible :
                              System.Windows.Visibility.Collapsed;*/
            if (e != null && e.User.Identity.IsAuthenticated) { ContentBorder.Visibility = System.Windows.Visibility.Visible; }
            else
            {
                ContentBorder.Visibility = System.Windows.Visibility.Collapsed;
                LoginRegistrationWindow loginWindow = new LoginRegistrationWindow();
                loginWindow.Show();

            }
        }
        /// <summary>
        /// After the Frame navigates, ensure the <see cref="HyperlinkButton"/> representing the current page is selected
        /// </summary>
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        /// <summary>
        /// If an error occurs during navigation, show an error window
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }

        private void ContentFrame_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserProfile.RunMode == 1)
            {
                SolidColorBrush scb2 = new SolidColorBrush(Color.FromArgb(255, 12, 180, 20));

                NavigationGrid.Background = scb2;
                ApplicationNameTextBlock.Text = "LexEna - Testy";

            }

        }
    }
}