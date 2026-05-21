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
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using LexEnaTrs;
using LexEnaTrs.Web;
using System.ServiceModel.DomainServices.Client;
using LexEnaTrs.LoginUI;

namespace LexEnaTrs.Views
{
    public partial class UsersKonfigView : UserControl
    {
        public UsersKonfigView()
        {
            InitializeComponent();
        }

        private int _status; 
    
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            new SlownikUzytkownikowAspNet().ReloadDictionary(); 
            this.Usersdds.QueryParameters[0].Value = UserProfile.IdJednostki;
            this.Usersdds.QueryParameters[1].Value = UserProfile.Rola;
            this.Usersdds.Load();
            if (UserProfile.Rola < 2)
            {
                this.AddNewUser.Visibility = Visibility.Collapsed;
                this.ResetPwd.Visibility = Visibility.Collapsed;
            
            }
            
        }

        private void UserDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            Guid memberGuid;
            int UserId;
            LexEnaMeritumDomainContext _kontouser;
            LoadOperation<aspnet_Membership> loadop;
            aspnet_Membership userMembersh;
            
            if (!this.Usersdds.HasChanges) return;

            if (_status != (this.UserDataForm.CurrentItem as Uzytkownik).Status)
            { // Update Membership
                // odczytaj guida bieżącego edytowanego użytkownika;
                _status =(int) (this.UserDataForm.CurrentItem as Uzytkownik).Status;
                UserId = (this.UserDataForm.CurrentItem as Uzytkownik).Id;
                memberGuid = new Guid();
                foreach (var r in UserList.UsersAspNetList)
                {
                    if (r.Id == UserId)
                    {
                        memberGuid = r.UserId;
                        break;
                    }
                }
                if (memberGuid != null)
                {

                    try
                    {
                        _kontouser = new LexEnaMeritumDomainContext();//(this.Usersdds.DataContext as LexEnaMeritumDomainContext); //new LexEnaMeritumDomainContext();
                        EntityQuery<aspnet_Membership> query =
                        from c in _kontouser.GetAspnet_MembershipByUserIdQuery(memberGuid) // .GetUzytkownikByIdQuery(DbId)
                        select c;
                        loadop = _kontouser.Load(query);
                        loadop.Completed += (se, ea) =>
                        {
                            userMembersh = loadop.Entities.FirstOrDefault();
                            if (userMembersh != null)
                            {
                                userMembersh.IsApproved = (_status > 0 ? true : false);
                                _kontouser.SubmitChanges();
                                this.Usersdds.SubmitChanges();
                                return;
                            }

                        };
                    }
                    catch (Exception ex)
                    {
                        ErrorWindow.CreateNew(ex, "Błąd zapisu statusu użytkownika");
                    }
                }
            }
            else
                this.Usersdds.SubmitChanges();

        }

       

        private void UserDataForm_BeginningEdit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _status = (this.UserDataForm.CurrentItem as Uzytkownik).Status.Value;
        }

        private void AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            LoginRegistrationWindow loginWindow = new LoginRegistrationWindow(true);
            loginWindow.Show();
            loginWindow.Closed += (se, ea) =>
                {
                    this.Usersdds.Load();  
                };
        }

        private void ChdPwd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.

            Uzytkownik usr;
            
            ChangePassword chngPwd;
             if (UserGridView.SelectedItems.Count > 0)   
            {
                usr = UserGridView.SelectedItem as Uzytkownik;
                chngPwd = new ChangePassword();
                chngPwd.UserName = usr.UserName;
                chngPwd.Show();
            
            }
            
            
            
            
             
        }

        

        private void ResetPwd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
                	// TODO: Add event handler implementation here.
           Uzytkownik usr;
            
            
           if (UserGridView.SelectedItems.Count > 0)   
           {
               usr = UserGridView.SelectedItem as Uzytkownik;
               PozewDomainContext context = new PozewDomainContext();
               context.GetUserSecurityQuestion(usr.UserName, GetUserSecurityQuestionCompleted, null);
            
           }

        }






        private void GetUserSecurityQuestionCompleted(InvokeOperation<string> result)
        {
            if (result.Value.StartsWith("Błąd"))
            {
                ErrorWindow.CreateNew("Próba resetu hasła nie powiodła się " + result.Value);

            }

            else
            {
                Uzytkownik usr;
                ResetPassword resetPwd;
                if (UserGridView.SelectedItems.Count > 0)
                {
                    usr = UserGridView.SelectedItem as Uzytkownik;
                    resetPwd = new ResetPassword();
                    resetPwd.UserName = usr.UserName;
                    if (UserProfile.Rola == 2)
                    {
                        resetPwd.SecureQuestion = "";
                    }
                    else
                        resetPwd.SecureQuestion = result.Value;
                    resetPwd.Show();

                }
            

                
            }

        }

        private void UnlockUsr_Click(object sender, RoutedEventArgs e)
        {
            Uzytkownik usr;
            if (UserGridView.SelectedItems.Count > 0)
            {
                usr = UserGridView.SelectedItem as Uzytkownik;
                PozewDomainContext context = new PozewDomainContext();
                context.UserUnlockAccount(usr.UserName, UserUnlockAccountCompleted, null);

            }
        }

        private void UserUnlockAccountCompleted(InvokeOperation<string> result)
        {
      
            if (result.Value.StartsWith("Błąd"))
            {
                ErrorWindow.CreateNew("Próba odblokowania konta nie powiodła się " + result.Value);

            }

            else
            {
                AlertMsg.Show(result.Value);
            }



            }

        }
    }

