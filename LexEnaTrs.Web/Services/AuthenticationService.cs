namespace LexEnaTrs.Web
{
    using System.Security.Authentication;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;
    using System.Linq;
    using System;

    /// <summary>
    /// RIA Services DomainService responsible for authenticating users when
    /// they try to log on to the application.
    ///
    /// Most of the functionality is already provided by the base class
    /// AuthenticationBase
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User> 
    {
        protected override bool  ValidateUser(string username, string password)
        {
            DateTime lastLogDate;
            bool isValidated = false;
            // tu można złapać
            using (var _meritum = new LexEnaMeritumEntities())
            {
                var LastLogonDate = (from asMemShp in _meritum.aspnet_Membership
                                     join asUsr in _meritum.aspnet_Users on asMemShp.UserId equals asUsr.UserId
                                     where asUsr.UserName == username
                                     select new { asMemShp.LastLoginDate }).FirstOrDefault();

                if (LastLogonDate != null)
                {
                        lastLogDate = LastLogonDate.LastLoginDate;

                var Uzyt = (from usr in _meritum.Uzytkownik
                                     where usr.UserName == username
                                     select  usr).FirstOrDefault();
                
                if (Uzyt != null)
                {
                    Uzyt.LastLogon = lastLogDate;
                        try
                        {
                            _meritum.SaveChanges();
                        }
                        catch ( Exception ex)
                        {
                            ;


                        }
                }
                
                }
            } 
            isValidated = base.ValidateUser(username, password);
            return isValidated;
        }
        
    }
}
