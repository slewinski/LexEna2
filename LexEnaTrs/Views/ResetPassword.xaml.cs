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
using System.ServiceModel.DomainServices.Client;
using LexEnaTrs.Web;
using LexEnaTrs.Views;

namespace LexEnaTrs
{
    public partial class ResetPassword : ChildWindow
    {
        public PasswordService pwdSrw;
        public string UserName;
        public string SecureQuestion;

        public ResetPassword()
        {
            InitializeComponent();
            pwdSrw = new PasswordService();
            this.LayoutRoot.DataContext = pwdSrw;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
           
            PozewDomainContext context = new PozewDomainContext();
            context.UserResetPassword(this.UserName, pwdSrw.Answer,ResetPasswordCompleted, null);
            
        }


        private void ResetPasswordCompleted(InvokeOperation<string> result)
        {
            if (result.Value.StartsWith("Błąd"))
            {
                ErrorWindow.CreateNew("Próba resetu hasła nie powiodła się. " + result.Value);

            }

            else
            {
                NewPasswordWindow npass = new NewPasswordWindow();
                npass.pwd = result.Value;
                npass.Show();
                npass.Closed += (se, ea) =>
                {
                    this.DialogResult = true;
                };
            }

        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            if (String.IsNullOrWhiteSpace(SecureQuestion))
            {
                this.tbOdpowiedz.Visibility = Visibility.Collapsed;
                this.tbPytanie.Text = "Potwierdź. ";
                pwdSrw.SecQuestion = " Czy na pewno chcesz ustawić nowe hasło ?";
                this.Answer.Visibility = Visibility.Collapsed;
            }    
            else
            pwdSrw.SecQuestion = SecureQuestion;
            pwdSrw.UsrName = UserName;

        }

       
    }
}

