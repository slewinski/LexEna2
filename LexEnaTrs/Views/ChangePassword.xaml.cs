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

namespace LexEnaTrs
{
    public partial class ChangePassword : ChildWindow
    {
        public PasswordService pwdSrw;
        public string UserName;

        public ChangePassword()
        {
            InitializeComponent();
            pwdSrw = new PasswordService();
            this.LayoutRoot.DataContext = pwdSrw;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (pwdSrw.Newpwd1.Length < 5 )
            {
                AlertMsg.Show("Hasło jest za krótkie");
                return;
            }
            if (pwdSrw.Newpwd1 !=  pwdSrw.Newpwd2)
            {
                AlertMsg.Show("Nowe hasło nie pasuje");
                return;
            }
            PozewDomainContext context = new PozewDomainContext();
            context.UserChangePassword(UserProfile.UserName, pwdSrw.Oldpwd, pwdSrw.Newpwd1, ChangePasswordCompleted, null);
            
        }


        private void ChangePasswordCompleted(InvokeOperation<bool> result)
        {
            if (!result.Value)
            {
                ErrorWindow.CreateNew("Próba zmiany hasła nie powiodła się");

            }

            else
            {
                AlertMsg.Show("Hasło zostało zmienine");
                this.DialogResult = true;
            }

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

