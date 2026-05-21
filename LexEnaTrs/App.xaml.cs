namespace LexEnaTrs
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using LexEnaTrs;
    using LexEnaTrs.LoginUI;
    using Telerik.Windows.Controls;
    using LexEnaTrs.Controls;
 
    /// <summary>
    /// Main <see cref="Application"/> class.
    /// </summary>
    /// 
    

    public partial class App : Application
    {
        private LexEnaTrs.Controls.BusyIndicator  busyIndicator;
       


        /// <summary>
        /// Creates a new <see cref="App"/> instance.
        /// </summary>
        public App()
        {
            //StyleManager.ApplicationTheme = new Windows7Theme();  
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            // Create a WebContext and add it to the ApplicationLifetimeObjects
            // collection.  This will then be available as WebContext.Current.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // This will enable you to bind controls in XAML files to WebContext.Current
            // properties
            // new CurrentUser();   // globalny obikt użytkownika 

            string runmode = string.Empty;
            
            try
            {
                runmode = e.InitParams["Client"];
            }
            catch (Exception)
            { }
            UserProfile.Firma = 0;
            string[] parameters = runmode.Split(';');
            if (parameters != null)
            {
                foreach (string s in parameters)
                {
                    string[] row = s.Split('=');
                    if (row.GetLowerBound(0) >= 0)
                    {
                
                        switch (row[0].ToUpper().Trim())
                        {
                            case "MODE":
                             
                                if (row[1].ToUpper() == "TEST")
                                                UserProfile.RunMode = 1;
                                             break;
                            case "FIRMA" :
                                try
                                {
     
                                    UserProfile.Firma = Int32.Parse(row[1].ToUpper());
                                }
                                catch (Exception)
                                { }
                                break;
                        }


                    } 
                }



            }

           
            this.Resources.Add("WebContext", WebContext.Current);

            // This will automatically authenticate a user when using windows authentication
            // or when the user chose "Keep me signed in" on a previous login attempt
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);
            
            WebContext.Current.Authentication.LoggedIn += new EventHandler<AuthenticationEventArgs>(MyAuthentication_LoggedIn);

            LexEnaKonfiguracja.Firma = UserProfile.Firma;
            // Show some UI to the user while LoadUser is in progress
            this.InitializeRootVisual();
           
        
        }

        private void MyAuthentication_LoggedIn (object sender, AuthenticationEventArgs ea  )
        {
            string _username;
            
            _username = ea.User.Identity.Name;
            // zaczytanie profilu użytkownika
            //_userName = loginOperation.LoginParameters.UserName;
            GetUsersProfile userProfile = new GetUsersProfile();
            userProfile.GetProfileAndKontoEPUByUserName(_username);
            userProfile.profileCompleted += new EventHandler(userProfileCompleted);
        }


        
        private void userProfileCompleted(object sender, EventArgs ea)
        {
            pozewEventArgs e;
            MainView _mainview;

            e = (ea as pozewEventArgs);
            if (e.Status < 0)
            {
                ErrorWindow.CreateNew(e.Message);
                ((this.RootVisual as LexEnaTrs.Controls.BusyIndicator).Content as MainPage).ContentBorder.Visibility = Visibility.Collapsed;
                return;
            }
            else
            { 
            // ustawienie praw dostępu
                ((this.RootVisual as LexEnaTrs.Controls.BusyIndicator).Content as MainPage).ContentBorder.Visibility = Visibility.Visible;
                if (UserProfile.Rola  == 1 || UserProfile.Rola == 2 )
                    ((this.RootVisual as LexEnaTrs.Controls.BusyIndicator).Content as MainPage).LinkAdmin.Visibility = Visibility.Visible;
                else
                    ((this.RootVisual as LexEnaTrs.Controls.BusyIndicator).Content as MainPage).LinkAdmin.Visibility = Visibility.Collapsed;

                _mainview = MainWorkspaceWindowHandler.MainViewHandler as MainView;
                _mainview.SetUserRights();
                _mainview.RefreshTreeView();
                //((this.RootVisual as BusyIndicator).Content as MainPage).U
               
                //HyperlinkButton x:Name="Link1"
                // ustawienie praw dostępu
            }
        
        }

        

        /// <summary>
        /// Invoked when the <see cref="LoadUserOperation"/> completes. Use this
        /// event handler to switch from the "loading UI" you created in
        /// <see cref="InitializeRootVisual"/> to the "application UI"
        /// </summary>
        private void Application_UserLoaded(LoadUserOperation operation)
        {
            if (! operation.User.Identity.IsAuthenticated )
             {
                LoginRegistrationWindow lowin = new LoginRegistrationWindow();
                lowin.Show();
            }
        }

        /// <summary>
        /// Initializes the <see cref="Application.RootVisual"/> property. The
        /// initial UI will be displayed before the LoadUser operation has completed
        /// (The LoadUser operation will cause user to be logged automatically if
        /// using windows authentication or if the user had selected the "keep
        /// me signed in" option on a previous login).
        /// </summary>
        protected virtual void InitializeRootVisual()
        {
            this.busyIndicator = new LexEnaTrs.Controls.BusyIndicator();
            this.busyIndicator.Content = new MainPage();
            this.busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            this.busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch;

            this.RootVisual = this.busyIndicator;
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // a ChildWindow control.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}