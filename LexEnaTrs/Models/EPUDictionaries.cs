using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.ComponentModel;
using System.Collections.ObjectModel;
using LexEnaTrs.Web;    
using System.Collections.Generic;
namespace LexEnaTrs
{

    /************** klasa globalna - bieżący użytkownik **/
    public static class UserList
    {
        public static ObservableCollection<vw_UsersAspNet> UsersAspNetList
        {
            get;
            set;
        }
      
    

    }


    public class GetUsersProfile
    {

        public EventHandler profileCompleted;

        protected virtual void OnprofileCompleted(pozewEventArgs e)
        {
            if (profileCompleted != null)
                profileCompleted(this, e);
        }


        public  void GetProfileAndKontoEPUByUserName(string UserName)
        {
            // odczytanie konta oraz konta EPU 
            Uzytkownik _user;
            LexEnaMeritumDomainContext _kontouser;  //radaDomainDataSource.DomainContext;
            LoadOperation<Uzytkownik> loadop;
            LoadOperation<JednostkaWindykacji> loadopJW;
            pozewEventArgs pea;

            _kontouser = new LexEnaMeritumDomainContext();
            try {
            EntityQuery<Uzytkownik> query =
                from c in _kontouser.GetUzytkownikByUserNameQuery(UserName)

                select c;
            loadop = _kontouser.Load(query);
            
            loadop.Completed += (sender, e) =>
            {
                _user = loadop.Entities.FirstOrDefault();
                 pea = new pozewEventArgs();
                if (_user == null)
                {
                    
                   
                    pea.Status = -1;
                    pea.Message = "Brak użytkownika o podanej nazwie w bazie użytkowników";
                    OnprofileCompleted(pea);
                    return;
                }

                UserProfile.Nazwisko = _user.Nazwisko;
                UserProfile.Imie = _user.Imie;
                UserProfile.Inicjal = _user.Inicjal;
                UserProfile.Rola = (int)_user.Rola;     // 0 - zwykły uzytkownik; 
                UserProfile.IdKontaEPU = _user.KontoEPU_Id;
                UserProfile.IdJednostki = (int)_user.JednostkaWindykacji_Id;
                UserProfile.kontoEPU = _user.KontoEPU;
                UserProfile.UserName = _user.UserName;
                UserProfile.DbId = _user.Id;
                UserProfile.lastLogon = _user.LastLogon;

                if (_user.JednostkaWindykacji != null)
                {
                    JednostkaWindykacji jed = _user.JednostkaWindykacji;
                    UserProfile.CzyWlasna = jed.TypJednostki;

                }
                else
                {
                    EntityQuery<JednostkaWindykacji> queryJW =
                    from c in _kontouser.GetJednostkaWindykacjiByIdQuery(_user.JednostkaWindykacji_Id??0)
                    select c;
                    loadopJW = _kontouser.Load(queryJW);
                    loadopJW.Completed += (sender1, e1) =>
                    //UsersRole.rola = (int)UserProfile.Rola;
                    {
                        JednostkaWindykacji j = loadopJW.Entities.FirstOrDefault();
                        UserProfile.CzyWlasna = j.TypJednostki;
                        pea.Message = _user.UserName;
                        pea.Status = 1;

                        //UserProfile.Firma = -1; // Energa Operator
                        OnprofileCompleted(pea);
                        return;
                    };
                }
                
                
            };
            }
                catch (Exception ex)
                 {
                    pea = new pozewEventArgs();
                    pea.Status = -100;
                    pea.Message = "Błąd odczytu danych użytkownika";
                    OnprofileCompleted(pea);
                }

        }
    
    }

    public class AnyAdminRole 
    {
      

        public Visibility VisibleForAnyAdmin
        {
            get {

                if (UserProfile.Rola > 0 && UserProfile.Rola < 3)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
                }
        
        
        }
        public Visibility VisibleForSuperAdmin
        {
            get
            {

                if (UserProfile.Rola == 2 )
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }


        }

        public Visibility VisibleForSuperAdminAndEnerga
        {
            get
            {

                if (UserProfile.Rola == 2 ||(UserProfile.Rola == 1 && UserProfile.CzyWlasna == 3 && UserProfile.Firma == -1) )
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }


        }

    }

    public class KrdRole
    {
        public Visibility VisibleForNonKrd
        {
            get
            {

                if (UserProfile.Rola != 2)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }


        }

        public Visibility VisibleForKrdOnly
        {
            get
            {

                if (UserProfile.Rola == 2 || UserProfile.Rola == 3)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }


        }

    }


    public class UnZDRole
    {
        public Visibility VisibleForNonUnZD
        {
            get
            {

                if (UserProfile.Rola <= 3)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }


        }

        public Visibility VisibleForUnZDOnly
        {
            get
            {

                if (UserProfile.Rola == 4 || UserProfile.Rola == 2)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }


        }

    }

    public class SuperAdminRole
    {


        public Visibility VisibleForAnyAdmin
        {
            get
            {

                if (UserProfile.Rola > 0)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }


        }

      

        public Visibility VisibleForSuperAdmin
        {
            get
            {

                if (UserProfile.Rola == 2)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }


        }

        public Visibility VisibleForSuperAdminAndEnerga
        {
            get
            {

                if (UserProfile.Rola == 2 || (UserProfile.Rola == 1 && UserProfile.CzyWlasna == 3 && UserProfile.Firma == - 1))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }


        }

    }

    public static class UserProfile
    {

        
      

        public static int? IdKontaEPU
        {
            get;
            set;

        }

        public static string Nazwisko
        {
            get;
            set;
        }
        public static string Imie
        {
            get;
            set;
        }

        public static string Inicjal
        {
            get;
            set;
        }

        public static int? Rola
        {
            get;
            set;
        }

        public static int IdJednostki
        {
            get;
            set;
        }
        public static int DbId
        {
            get;
            set;
        }
        public static KontoEPU kontoEPU
        {
            get;
            set;
        }
        public static string UserName
        {
            get;
            set;
        }

        public static DateTime? lastLogon
        {
            get;
            set;

        }

        public static int  Firma
        {
            get;
            set;

        }

        public static int RunMode
        {
            get;
            set;

        }

        public static int CzyWlasna  // czy jednosta wewnętrzna
        {
            get;
            set;
        }
    }

    public static class LexEnaKonfiguracja
    {

        public static int Firma
        {
            get;
            set;

        }

        public static ObservableCollection<typSlownikFilter> SlownikSymboliWiena
        {
            get;
            set;

        }



    }


    public class tekstSlownik
    {
        public tekstSlownik()
        {
        }

        public tekstSlownik(int numer, string nazwa, string tresc )
        {
            Nazwa = nazwa;
            Numer = numer;
            Tresc = tresc;
        }

        public string Nazwa
        {
            get;
            set;
        }
        public int Numer
        {
            get;
            set;
        }
        public string Tresc
        {
            get;
            set;
        }

    }





    public class radcaSlownik
    {
        public radcaSlownik()
        {
        }

        public radcaSlownik(string nazwa, int numer)
        {
            Nazwa = nazwa;
            Numer = numer;
        }

        public string Nazwa
        {
            get;
            set;
        }
        public int Numer
        {
            get;
            set;
        }
    }

    public static class TabelaOdsetekUstawowych
    {

        public static List<OdsTab> Tabela { get; set;}
    
    }

    public  class TabOdsetek
    {
       


        private void _loadDictionary()
        {
            LexEnaMeritumDomainContext _nazodscontext;  //radaDomainDataSource.DomainContext;
            LoadOperation<OdsTab> loadop;

            _nazodscontext = new LexEnaMeritumDomainContext();

            EntityQuery<OdsTab> query =
                from c in _nazodscontext.GetOdsTabQuery()
                orderby c.DataP ascending
                select c;
            loadop = _nazodscontext.Load(query);
            loadop.Completed += (sender, e) =>
            {
                foreach (var r in loadop.Entities)
                {
                    TabelaOdsetekUstawowych.Tabela.Add(r);
                }


            };

        }
        public TabOdsetek()
        { 
        if (TabelaOdsetekUstawowych.Tabela == null )
          
            TabelaOdsetekUstawowych.Tabela = new List<OdsTab>();
        else
            TabelaOdsetekUstawowych.Tabela.Clear();
            _loadDictionary();
        }
       
       
        }
   

    



    /************ Słownik rodzajow odsetek Wiena ***************/
    public class SlownikOdsetekWiena
    {
        private ObservableCollection<NazwyOdsetek> _nazodsWiena;


        private void _loadDictionary()
        {
            LexEnaMeritumDomainContext _nazodscontext;  //radaDomainDataSource.DomainContext;
            LoadOperation<NazwyOdsetek> loadop;

            _nazodscontext = new LexEnaMeritumDomainContext();

            EntityQuery<NazwyOdsetek> query =
                from c in _nazodscontext.GetNazwyOdsetekQuery()

                select c;
            loadop = _nazodscontext.Load(query);
            loadop.Completed += (sender, e) =>
            {
                foreach (var r in loadop.Entities)
                {
                    _nazodsWiena.Add(r);
                }


            };

        }



        public void ReloadDictionary()
        {// przeładowanie słownika po zmianie
            if (_nazodsWiena == null)
                _nazodsWiena = new ObservableCollection<NazwyOdsetek>();
            else
                _nazodsWiena.Clear();
            this._loadDictionary();
        }
        public ObservableCollection<NazwyOdsetek> dictionaryNazOdsWiena
        {
            get
            {
                if (_nazodsWiena == null)
                {
                    _nazodsWiena = new ObservableCollection<NazwyOdsetek>();
                    this._loadDictionary();
                }

                return _nazodsWiena;
            }
        }
    }


    public class SlownikFaktow
    {

        private List<Slownik> _faktSlownik;
        public EventHandler faktyCompeted;

        protected virtual void OnfaktyCompleted(pozewEventArgs e)
        {
            if (faktyCompeted != null)
                faktyCompeted(this, e);
        }


        private void _loadDictionary()
        {
            LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
            LoadOperation<Slownik> loadop;
            pozewEventArgs pea;
            _context = new LexEnaMeritumDomainContext();

            EntityQuery<Slownik> query =
                from c in _context.GetSlownikByTypnFiltrQuery(2, 0)

                select c;
            loadop = _context.Load(query);
            try
            {
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        _faktSlownik.Add(r);

                    }

                    pea = new pozewEventArgs();
                    pea.Status = 100;
                    OnfaktyCompleted(pea);
                };
            }
            catch (Exception ex)
            {
                pea = new pozewEventArgs();
                pea.Status = -1;
                OnfaktyCompleted(pea);

            }

        }

        public List<Slownik> faktSlownik
        {
            get
            {
                if (_faktSlownik == null)
                {
                    _faktSlownik = new List<Slownik>();
                    _loadDictionary();
                }

                return _faktSlownik;
            }
        }

    }

        public class SlownikTekstow
        {

            private ObservableCollection<tekstSlownik> _uzasSlownik;
            private ObservableCollection<tekstSlownik> _faktSlownik;


            private void _loadDictionary(int _type)
            {
                LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
                LoadOperation<Slownik> loadop;

                _context = new LexEnaMeritumDomainContext();

                EntityQuery<Slownik> query =
                    from c in _context.GetSlownikByTypQuery(_type)

                    select c;
                loadop = _context.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        if (_type == 1)
                            _uzasSlownik.Add(new tekstSlownik(r.Id, r.Nazwa, r.Tresc));
                        else
                            _faktSlownik.Add(new tekstSlownik(r.Id, r.Nazwa, r.Tresc));
                    }


                };

            }



            public void ReloadDictionaryUzasadnienia()
            {// przeładowanie słownika po zmianie
                if (_uzasSlownik == null)
                    _uzasSlownik = new ObservableCollection<tekstSlownik>();
                else
                    _uzasSlownik.Clear();
                this._loadDictionary(1);
            }

            public void ReloadDictionaryFakty()
            {// przeładowanie słownika po zmianie
                if (_uzasSlownik == null)
                    _uzasSlownik = new ObservableCollection<tekstSlownik>();
                else
                    _uzasSlownik.Clear();
                this._loadDictionary(2);
            }


            public ObservableCollection<tekstSlownik> uzasadnienieSlownik
            {
                get
                {
                    if (_uzasSlownik == null)
                    {
                        _uzasSlownik = new ObservableCollection<tekstSlownik>();
                        _loadDictionary(1);
                    }

                    return _uzasSlownik;
                }
            }


            public ObservableCollection<tekstSlownik> faktSlownik
            {
                get
                {
                    if (_faktSlownik == null)
                    {
                        _faktSlownik = new ObservableCollection<tekstSlownik>();
                        _loadDictionary(2);
                    }

                    return _faktSlownik;
                }
            }
        }



        /******************/
        public class SlownikRadcow
        {
            private ObservableCollection<radcaSlownik> _radcowie;


            private void _loadDictionary()
            {
                LexEnaMeritumDomainContext _radcacontext;  //radaDomainDataSource.DomainContext;
                LoadOperation<Radca> loadop;

                _radcacontext = new LexEnaMeritumDomainContext();

                EntityQuery<Radca> query =
                    from c in _radcacontext.GetRadcaQuery()
                    orderby c.Nazwisko
                    select c;
                loadop = _radcacontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        _radcowie.Add(new radcaSlownik(r.Nazwisko + " " + r.Imie, r.Id));
                    }


                };

            }



            public void ReloadDictionary()
            {// przeładowanie słownika po zmianie
                if (_radcowie == null)
                    _radcowie = new ObservableCollection<radcaSlownik>();
                else
                    _radcowie.Clear();
                _loadDictionary();
            }
            public ObservableCollection<radcaSlownik> dictionaryRadca
            {
                get
                {
                    if (_radcowie == null)
                    {
                        _radcowie = new ObservableCollection<radcaSlownik>();
                        _loadDictionary();
                    }

                    return _radcowie;
                }
            }
        }

        /*************** słownik Statusów *****/
        public class SlownikStatusow
        {
            private ObservableCollection<NazwaStatusu> _statusy;


            private void _loadDictionary()
            {
                LexEnaMeritumDomainContext _statusycontext;  //radaDomainDataSource.DomainContext;
                LoadOperation<NazwaStatusu> loadop;

                _statusycontext = new LexEnaMeritumDomainContext();

                EntityQuery<NazwaStatusu> query =
                    from c in _statusycontext.GetNazwaStatusuInOrderQuery()
                    orderby c.Krok
                    select c;

                loadop = _statusycontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        _statusy.Add(r);
                    }


                };

            }



            public void ReloadDictionary()
            {// przeładowanie słownika po zmianie
                if (_statusy == null)
                    _statusy = new ObservableCollection<NazwaStatusu>();
                else
                    _statusy.Clear();
                _loadDictionary();
            }
            public ObservableCollection<NazwaStatusu> dictionaryStatusy
            {
                get
                {
                    if (_statusy == null)
                    {
                        _statusy = new ObservableCollection<NazwaStatusu>();
                        _loadDictionary();
                    }

                    return _statusy;
                }
            }
        }


    // sławmi sądów

    public class SlownikSadow
    {
        private ObservableCollection<SadSlownik> _sady;


        private void _loadDictionary()
        {
            LexEnaMeritumDomainContext _statusycontext;  //radaDomainDataSource.DomainContext;
            LoadOperation<SadSlownik> loadop;

            _statusycontext = new LexEnaMeritumDomainContext();

            EntityQuery<SadSlownik> query =
                from c in _statusycontext.GetSadyInOrderAllQuery()
                select c;

            loadop = _statusycontext.Load(query);
            loadop.Completed += (sender, e) =>
            {
                foreach (var r in loadop.Entities)
                {
                    r.nazwa = ((String.IsNullOrWhiteSpace(r.miasto) ? "" : r.miasto + ":") + r.nazwa + " " + r.wydzial).Trim();
                    _sady.Add(r);
                }


            };

        }



        public void ReloadDictionary()
        {// przeładowanie słownika po zmianie
            if (_sady == null)
                _sady = new ObservableCollection<SadSlownik>();
            else
                _sady.Clear();
            _loadDictionary();
        }
        public ObservableCollection<SadSlownik> dictionarySady
        {
            get
            {
                if (_sady == null)
                {
                    _sady = new ObservableCollection<SadSlownik>();
                    _loadDictionary();
                }

                return _sady;
            }
        }
    }





    /********Słownik typów należności **********/

    public class ListaTypNal
        {
            private ObservableCollection<TypNaleznosci> _typynal;


            private void _loadDictionary()
            {
                LexEnaMeritumDomainContext _typynalcontext;  //radaDomainDataSource.DomainCoxt;
                LoadOperation<TypNaleznosci> loadop;

                _typynalcontext = new LexEnaMeritumDomainContext();

                EntityQuery<TypNaleznosci> query =
                    from c in _typynalcontext.GetTypNaleznosciInOrderQuery()
                    select c;

                loadop = _typynalcontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        _typynal.Add(r);
                    }


                };

            }



            public void ReloadDictionary()
            {// przeładowanie słownika po zmianie
                if (_typynal == null)
                    _typynal = new ObservableCollection<TypNaleznosci>();
                else
                    _typynal.Clear();
                _loadDictionary();
            }
            public ObservableCollection<TypNaleznosci> dictionaryTypyNal
            {
                get
                {
                    if (_typynal == null)
                    {
                        _typynal = new ObservableCollection<TypNaleznosci>();
                        _loadDictionary();
                    }

                    return _typynal;
                }
            }
        }

        /*************** słownik Jednoste *****/
        public class SlownikJednostekWindykacji
        {
            private static ObservableCollection<JednostkaWindykacji> _jednostki;

            static SlownikJednostekWindykacji()
            {
            ReloadDictionary();

            }
            private static void _loadDictionary()
            {
                LexEnaMeritumDomainContext _jedwincontext;  //radaDomainDataSource.DomainContext;
                LoadOperation<JednostkaWindykacji> loadop;

                _jedwincontext = new LexEnaMeritumDomainContext();

                EntityQuery<JednostkaWindykacji> query =
                    from c in _jedwincontext.GetJednostkaWindykacjiQuery()
                    orderby c.Nazwa
                    select c;

                loadop = _jedwincontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        _jednostki.Add(r);
                    }


                };

            }



            public static void ReloadDictionary()
            {// przeładowanie słownika po zmianie
                if (_jednostki == null)
                    _jednostki = new ObservableCollection<JednostkaWindykacji>();
                else
                    _jednostki.Clear();
                _loadDictionary();
            }
            public static ObservableCollection<JednostkaWindykacji> dictionaryJednostkiWindykacji
            {
                get
                {
                    if (_jednostki == null)
                    {
                        _jednostki = new ObservableCollection<JednostkaWindykacji>();
                        _loadDictionary();
                    }

                    return _jednostki;
                }
            }
        }


        public class SlownikKomornikow
        {
            private ObservableCollection<KancelariaKomornicza> _jednostki;


            private void _loadDictionary()
            {
                LexEnaMeritumDomainContext _jedwincontext;  //radaDomainDataSource.DomainContext;
                LoadOperation<KancelariaKomornicza> loadop;

                _jedwincontext = new LexEnaMeritumDomainContext();
                KancelariaKomornicza kanckom;
                EntityQuery<KancelariaKomornicza> query =
                    from c in _jedwincontext.GetKancelariaKomorniczaInOrderQuery()
                    where c.czyus==0
                    orderby c.IdEPU
                    select c;

                loadop = _jedwincontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        kanckom = new KancelariaKomornicza();
                        kanckom.IdEPU = r.IdEPU;
                        kanckom.Id = r.Id;
                        kanckom.czyus = r.czyus;
                        kanckom.Nazwa = r.IdEPU.ToString() + " " + r.Nazwa;
                        _jednostki.Add(kanckom);
                    }


                };

            }



            public void ReloadDictionary()
            {// przeładowanie słownika po zmianie
                if (_jednostki == null)
                    _jednostki = new ObservableCollection<KancelariaKomornicza>();
                else
                    _jednostki.Clear();
                _loadDictionary();
            }
            public ObservableCollection<KancelariaKomornicza> dictionaryKancelariKomornicza
            {
                get
                {
                    if (_jednostki == null)
                    {
                        _jednostki = new ObservableCollection<KancelariaKomornicza>();
                        _loadDictionary();
                    }

                    return _jednostki;
                }
            }
        }


        public class SlownikUzytkownikow
        {
            private ObservableCollection<Uzytkownik> _users;


            private void _loadDictionary()
            {
                LexEnaMeritumDomainContext _jedwincontext;  //radaDomainDataSource.DomainContext;
                LoadOperation<Uzytkownik> loadop;

                _jedwincontext = new LexEnaMeritumDomainContext();

                EntityQuery<Uzytkownik> query =
                    from c in _jedwincontext.GetUzytkownikByIdJednostkiQuery(UserProfile.IdJednostki, (int)UserProfile.Rola)
                    orderby c.Nazwisko
                    select c;
                /*
                EntityQuery<Uzytkownik> query =
                    from c in _jedwincontext.GetUzytkownikQuery()
                    orderby c.Nazwisko
                    select c;
                */
                loadop = _jedwincontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        _users.Add(r);
                    }


                };

            }



            public void ReloadDictionary()
            {// przeładowanie słownika po zmianie
                if (_users == null)
                    _users = new ObservableCollection<Uzytkownik>();
                else
                    _users.Clear();
                _loadDictionary();
            }
            public ObservableCollection<Uzytkownik> dictionaryUzytkownicy
            {
                get
                {
                    if (_users == null)
                    {
                        _users = new ObservableCollection<Uzytkownik>();
                        _loadDictionary();
                    }

                    return _users;
                }
            }
        }

        //  użytkownicy w Membership.

        public class SlownikUzytkownikowAspNet
        {
            private ObservableCollection<vw_UsersAspNet> _users;


            private void _loadDictionary()
            {
                LexEnaMeritumDomainContext _jedwincontext;  //radaDomainDataSource.DomainContext;
                LoadOperation<vw_UsersAspNet> loadop;

                _jedwincontext = new LexEnaMeritumDomainContext();

                EntityQuery<vw_UsersAspNet> query =
                    from c in _jedwincontext.GetVw_UsersAspNetQuery()
                    orderby c.Nazwisko
                    select c;

                loadop = _jedwincontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    if (UserList.UsersAspNetList == null)
                        UserList.UsersAspNetList = new ObservableCollection<vw_UsersAspNet>();
                    else
                        UserList.UsersAspNetList.Clear();
                    foreach (var r in loadop.Entities)
                    {
                        _users.Add(r);
                        UserList.UsersAspNetList.Add(r);
                    }


                };

            }



            public void ReloadDictionary()
            {// przeładowanie słownika po zmianie
                if (_users == null)
                    _users = new ObservableCollection<vw_UsersAspNet>();
                else
                    _users.Clear();
                _loadDictionary();
            }
            public ObservableCollection<vw_UsersAspNet> dictionaryUzytkownicyAspNet
            {
                get
                {
                    if (_users == null)
                    {
                        _users = new ObservableCollection<vw_UsersAspNet>();
                        _loadDictionary();
                    }

                    return _users;
                }
            }
        }


        public class SlownikUzytkownikowJednostki
        {
            private ObservableCollection<Uzytkownik> _users;


            private void _loadDictionary()
            {
                LexEnaMeritumDomainContext _jedwincontext;  //radaDomainDataSource.DomainContext;
                LoadOperation<Uzytkownik> loadop;

                _jedwincontext = new LexEnaMeritumDomainContext();

                EntityQuery<Uzytkownik> query =
                    from c in _jedwincontext.GetUzytkownikByIdJednostkiQuery(UserProfile.IdJednostki, (int)UserProfile.Rola)
                    orderby c.Nazwisko
                    select c;

                loadop = _jedwincontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        _users.Add(r);
                    }


                };

            }



            public void ReloadDictionary()
            {// przeładowanie słownika po zmianie
                if (_users == null)
                    _users = new ObservableCollection<Uzytkownik>();
                else
                    _users.Clear();
                _loadDictionary();
            }
            public ObservableCollection<Uzytkownik> dictionaryUzytkownicy
            {
                get
                {
                    if (_users == null)
                    {
                        _users = new ObservableCollection<Uzytkownik>();
                        _loadDictionary();
                    }

                    return _users;
                }
            }
        }


        // Słownik kont jednostki

        public class SlownikKontEPU
        {
            private ObservableCollection<KontoEPU> _kEPU;


            private void _loadDictionary()
            {
                LexEnaMeritumDomainContext _jedwincontext;  //radaDomainDataSource.DomainContext;
                LoadOperation<KontoEPU> loadop;
                EntityQuery<KontoEPU> query;

                _jedwincontext = new LexEnaMeritumDomainContext();

                if (UserProfile.Rola == 2 )
                 query =  from c in _jedwincontext.GetKontoEPUQuery()
                    orderby c.NazwaKonta
                    select c;
                else
                 query = from c in _jedwincontext.GetKontoEPUByIdJednostkiQuery(UserProfile.IdJednostki)
                    orderby c.NazwaKonta
                    select c;

                loadop = _jedwincontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        _kEPU.Add(r);
                    }


                };

            }



            public void ReloadDictionary()
            {// przeładowanie słownika po zmianie
                if (_kEPU == null)
                    _kEPU = new ObservableCollection<KontoEPU>();
                else
                    _kEPU.Clear();
                _loadDictionary();
            }
            public ObservableCollection<KontoEPU> dictionaryKontaEPU
            {
                get
                {
                    if (_kEPU == null)
                    {
                        _kEPU = new ObservableCollection<KontoEPU>();
                        _loadDictionary();
                        
                    }

                    return _kEPU;
                }
            }
        }

   
        public class LstPaczek
        {
            private ObservableCollection<Paczka> _paczkaLst;


            private void _loadDictionary()
            {
                LexEnaMeritumDomainContext _jedwincontext;  //radaDomainDataSource.DomainContext;
                LoadOperation<Paczka> loadop;

                _jedwincontext = new LexEnaMeritumDomainContext();

                EntityQuery<Paczka> query =
                    from c in _jedwincontext.GetPaczkaQuery()
                    orderby c.Id descending
                    select c;

                loadop = _jedwincontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    foreach (var r in loadop.Entities)
                    {
                        _paczkaLst.Add(r);
                    }


                };

            }

        

            public void ReloadDictionary()
            {// przeładowanie słownika po zmianie
                if (_paczkaLst == null)
                    _paczkaLst = new ObservableCollection<Paczka>();
                else
                    _paczkaLst.Clear();
                _loadDictionary();
            }
            public ObservableCollection<Paczka> PaczkiLst
            {
                get
                {
                    if (_paczkaLst == null)
                    {
                        _paczkaLst = new ObservableCollection<Paczka>();
                        _loadDictionary();

                    }

                    return _paczkaLst;
                }
            }
        }



        public class SprawaExtraData : INotifyPropertyChanged
        {
            private vw_DaneSprawy _innesprdane;// = new vw_DaneSprawy();
            private int _id_sprawy;

            public event PropertyChangedEventHandler PropertyChanged;


            protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
            {
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, args);
                }
            }

            public int IdSprawy
            {
                get
                {
                    return _id_sprawy;

                }
                set
                {
                    _id_sprawy = value;

                }


            }

            public void LoadData()
            {

                LexEnaMeritumDomainContext _sprextracontext;  //radaDomainDataSource.DomainContext;
                LoadOperation<vw_DaneSprawy> loadop;


                _sprextracontext = new LexEnaMeritumDomainContext();

                EntityQuery<vw_DaneSprawy> query =
                    from c in _sprextracontext.GetVw_DaneSprawyById2Query(_id_sprawy)
                    select c;

                loadop = _sprextracontext.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    ExtraSprData = loadop.Entities.FirstOrDefault();
                    //this.OnPropertyChanged("ExtraSprData"); 
                };

            }

            private void OnPropertyChanged(string propertyName)
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }

            public SprawaExtraData()
            {
                /*  if (_innesprdane == null)
                  {
                      _innesprdane = new vw_DaneSprawy();
                      _innesprdane.Status = "Dupa";
                  } */
            }
            public SprawaExtraData(vw_DaneSprawy daneSpr)
            {
                //this._innesprdane = daneSpr;
            }

            public vw_DaneSprawy ExtraSprData
            {
                get
                {

                    return this._innesprdane;
                }
                set
                {
                    if (value != _innesprdane)
                    {
                        this._innesprdane = value;
                        this.OnPropertyChanged("ExtraSprData");

                    }
                }

            }



        }

    public class FirmaViewModel : INotifyPropertyChanged
    {
        private bool eopVisible = true;
        private bool eobVisible = true;

        public bool EOPVisible
        {
            get
            {
                if (UserProfile.Firma != 1)
                    eopVisible = true;
                else
                    eopVisible = false;

                return eopVisible;
            }

            set
            {
                     eopVisible = value;
                if (UserProfile.Firma != 1)
                    eopVisible = true;
                else
                    eopVisible = false;
                NotifyPropertyChanged("EOPVisible");
            }
        }

        public bool EOBVisible
        {
            get
            {
               
                if (UserProfile.Firma != -1  )
                    eobVisible = true;
                else
                    eobVisible = false;

                return eobVisible;
            }

            set
            {
                eobVisible = value;
                if (UserProfile.Firma != -1)
                    eobVisible = true;
                else
                    eobVisible = false;
                NotifyPropertyChanged("EOBVisible");
            }
        }
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }



// Klasy słownikowe - statyczne
// typy zadań 




public class akcjaSlownik
        {
            private ObservableCollection<typSlownik> _typAkcji;

            public ObservableCollection<typSlownik> TypAkcjiSlownik
            {
                get
                {
                    if (_typAkcji == null)
                    {
                        _typAkcji = new ObservableCollection<typSlownik>();

                        _typAkcji.Add(new typSlownik("<Brak akcji>", 0));
                        _typAkcji.Add(new typSlownik("Dołączaj zawsze", 1));
                        _typAkcji.Add(new typSlownik("Dołącz jeśli osoba prawna", 2));  // przy serializacji zmapować na 0  i poznawać po obecnoości tagi inne ..... - tylko na czas edycji
                        _typAkcji.Add(new typSlownik("Dołącz jeśli działalność gosp.", 3));
                        _typAkcji.Add(new typSlownik("Dołącz jeśli osoba fizyczna", 4));
                    }

                    return _typAkcji;
                }
            }
        }

    public class akcjaSlownikDokWiena
    {
        private ObservableCollection<typSlownik> _typDokOdebr;


        private void GetWienaDocCompleted(InvokeOperation<IEnumerable<string>> result)
        {
            if (result != null && !result.HasError)
            {
                int i = 0;
                foreach (string name in result.Value)
                {

                    _typDokOdebr.Add(new typSlownik(name, i++));



                }


            }

        }

        public ObservableCollection<typSlownik> NazwaDokWiena
        {



            get
            {
                if (_typDokOdebr == null)
                {
                    _typDokOdebr = new ObservableCollection<typSlownik>();
                    PozewDomainContext poz = new PozewDomainContext();
                    poz.GetWienaDocNames (GetWienaDocCompleted, null);
                    
                }

                return _typDokOdebr;
            }
        }
    }



    public class akcjaDokSlownik
    {
        private ObservableCollection<typSlownik> _typAkcji;

        public ObservableCollection<typSlownik> TypAkcjiDokSlownik
        {
            get
            {
                if (_typAkcji == null)
                {
                    _typAkcji = new ObservableCollection<typSlownik>();

                    _typAkcji.Add(new typSlownik("<Brak akcji>", 0));
                    _typAkcji.Add(new typSlownik("post. o zawieszeniu egzekucji", 1));
                    _typAkcji.Add(new typSlownik("post. o podjęciu zawieszonego postępowania", 2));  // przy serializacji zmapować na 0  i poznawać po obecnoości tagi inne ..... - tylko na czas edycji
                    _typAkcji.Add(new typSlownik("post. o umorzeniu - zgon dłużnika", 3));
                    _typAkcji.Add(new typSlownik("post. o umorzeniu wobec bezskuteczności egzekucji",4));
                    _typAkcji.Add(new typSlownik("post. o umorzeniu na wniosek wierzyciela", 5));
                    _typAkcji.Add(new typSlownik("post. o umorzeniu postępowania z mocy prawa", 6));
                    _typAkcji.Add(new typSlownik("post. o zakończeniu", 7));
                    _typAkcji.Add(new typSlownik("zawiadomienie o zajęciu rachunku bankowego", 8));
                    _typAkcji.Add(new typSlownik("post. o przyznaniu kosztów zastępstwa adwokackiego", 9));
                }

                return _typAkcji;
            }
        }
    }


    public class typZadania
        {
            public typZadania()
            {
            }

            public typZadania(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }

        public class typyZadan
        {
            private ObservableCollection<typZadania> tyZa;

            public ObservableCollection<typZadania> rodzajeZadan
            {
                get
                {
                    if (tyZa == null)
                    {
                        tyZa = new ObservableCollection<typZadania>();
                        tyZa.Add(new typZadania("Wymiana z systemem dziedzinowym", 1));
                        tyZa.Add(new typZadania("Złóż Pozwy", 2));
                        tyZa.Add(new typZadania("Moje Sprawy", 3));
                        tyZa.Add(new typZadania("Moje Sprawy przez zakres sygnatur", 4));
                        tyZa.Add(new typZadania("Moje Nakazy", 5));
                        tyZa.Add(new typZadania("Moje Doręczenia", 6));
                        tyZa.Add(new typZadania("Moje Orzeczenia", 7));
                        tyZa.Add(new typZadania("Złóż Wnioski", 8));
                        tyZa.Add(new typZadania("Złóż Dokumenty", 9));
                        tyZa.Add(new typZadania("Lista Kancelarii", 10));
                    }

                    return tyZa;
                }
            }
        }


        public class zasadzenieKZA
        {
            private ObservableCollection<typZadania> staZa;

            public ObservableCollection<typZadania> dictionaryKZA
            {
                get
                {
                    if (staZa == null)
                    {
                        staZa = new ObservableCollection<typZadania>();
                        staZa.Add(new typZadania("Brak żądania zasądzenia KZA", 0));
                        staZa.Add(new typZadania("Żadanie zasądzenia wg norm przypisanych", 1));
                        staZa.Add(new typZadania("Żądanie zasądzenia w podanej wysokości", 2));
                        
                    }

                    return staZa;
                }
            }
        }


        public class stausyZadan
        {
            private ObservableCollection<typZadania> staZa;

            public ObservableCollection<typZadania> statusyWykonaniaZadan
            {
                get
                {
                    if (staZa == null)
                    {
                        staZa = new ObservableCollection<typZadania>();
                        staZa.Add(new typZadania("Do wykonania", 0));
                        staZa.Add(new typZadania("W trakcie wykonywania", 100));
                        staZa.Add(new typZadania("Wykonane", 200));
                        staZa.Add(new typZadania("Błąd wykonania wyjątek", -110));
                        staZa.Add(new typZadania("Błąd brak procedury", -200));
                        staZa.Add(new typZadania("Błąd: zwrócony przez serwis e-Sądu", -100));
                        staZa.Add(new typZadania("Błąd - wykonania pakietu SSIS", -300));
                        staZa.Add(new typZadania("Prawdop. brak elementów spełniających warunek", -1));
                        staZa.Add(new typZadania("Wyjątek z EPU wynikający z wykonywania wewnętrznej procedury", -3000));
                        staZa.Add(new typZadania("Błąd w warstwie komunikacyjnej z EPU", -2000));
                        staZa.Add(new typZadania("Wyjątek w komunikacji z epu lub błędu logicznego w danych wejściowych", -1000));
                    }

                    return staZa;
                }
            }
        }

    public class rodzajRoszczenia
        {
            public rodzajRoszczenia()
            {
            }

            public rodzajRoszczenia(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }

        public class rodzajeRoszczen
        {
            private ObservableCollection<rodzajRoszczenia> roRo;

            public ObservableCollection<rodzajRoszczenia> rodzajRoszczenia
            {
                get
                {
                    if (roRo == null)
                    {
                        roRo = new ObservableCollection<rodzajRoszczenia>();

                        roRo.Add(new rodzajRoszczenia("Cywilne", 0));
                        roRo.Add(new rodzajRoszczenia("Gospodarcze", 1));
                        roRo.Add(new rodzajRoszczenia("Z prawa pracy", 2));
                        roRo.Add(new rodzajRoszczenia("Inne", 3));
                    }

                    return roRo;
                }
            }
        }
        // Słownik województw

        public class rodzajDokumentu
        {
            public rodzajDokumentu()
            {
            }

            public rodzajDokumentu(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }

        public class rodzajeDokumentow
        {
            private ObservableCollection<rodzajDokumentu> roDok;

            public ObservableCollection<rodzajDokumentu> typDokumentu
            {
                get
                {
                    if (roDok == null)
                    {
                        roDok = new ObservableCollection<rodzajDokumentu>();
                        roDok.Add(new rodzajDokumentu("Pozwy", 10));
                        roDok.Add(new rodzajDokumentu("Dokumenty", 3));
                        roDok.Add(new rodzajDokumentu("Skargi", 1));
                        roDok.Add(new rodzajDokumentu("Zażalenia", 2));
                        roDok.Add(new rodzajDokumentu("Wnioski Egzekucyjne", 30));

                        roDok.Add(new rodzajDokumentu("Pozwy \"zwykłe\"", 100010));
                        roDok.Add(new rodzajDokumentu("Wnioski egz. \"zwykłe\"", 100030));
                        roDok.Add(new rodzajDokumentu("Inne Dokumenty \"zwykłe\"", 100003));

                    //roDok.Add(new rodzajDokumentu("Sprzeciw", 0));

                }



                    return roDok;
                }
            }
        }


    public class rodzajeDokumentowOdebr
    {
        private ObservableCollection<rodzajDokumentu> roDokOdebr;

        public ObservableCollection<rodzajDokumentu> typDokumentu
        {
            get
            {
                if (roDokOdebr == null)
                {
                    roDokOdebr = new ObservableCollection<rodzajDokumentu>();
                    
                    roDokOdebr.Add(new rodzajDokumentu("Nakazy \"zwykłe\"", 100005));
                    roDokOdebr.Add(new rodzajDokumentu("Wyroki \"zwykłe\"", 105005));
                    roDokOdebr.Add(new rodzajDokumentu("Klauzule \"zwykłe\"", 100017));
                    roDokOdebr.Add(new rodzajDokumentu("Sprzeciwy", 100013));
                    roDokOdebr.Add(new rodzajDokumentu("Dokumenty komornicze \"zwykłe\"", 10002));

                    //roDok.Add(new rodzajDokumentu("Sprzeciw", 0));

                }



                return roDokOdebr;
            }
        }
    }

    public class statusPaczki
        {
            public statusPaczki()
            {
            }

            public statusPaczki(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }

        public class statusyPaczekClass
        {
            private ObservableCollection<statusPaczki> roDok;

            public ObservableCollection<statusPaczki> statusyPaczek
            {
                get
                {
                    {
                        roDok = new ObservableCollection<statusPaczki>();

                        //roDok.Add(new rodzajDokumentu("Sprzeciw", 0));
                        roDok.Add(new statusPaczki("W przygotowaniu", 1));
                        roDok.Add(new statusPaczki("Do wysyłki", 2));
                        roDok.Add(new statusPaczki("Złożona", 3));
                        roDok.Add(new statusPaczki("Odrzucona", 4));
                        roDok.Add(new statusPaczki("Rozpisana", 5));
                        roDok.Add(new statusPaczki("Usunięta", 6));
                    }

                    return roDok;
                }
            }
        }

        public class wojewodztwoClass
        {
            public wojewodztwoClass()
            {
            }

            public wojewodztwoClass(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }

        public class listaWojewodztw
        {
            private ObservableCollection<wojewodztwoClass> woj;

            public ObservableCollection<wojewodztwoClass> wojewodztwa
            {
                get
                {
                    if (woj == null)
                    {
                        woj = new ObservableCollection<wojewodztwoClass>();

                        woj.Add(new wojewodztwoClass("dolnośląskie", 0));
                        woj.Add(new wojewodztwoClass("kujawsko-pomorskie", 1));
                        woj.Add(new wojewodztwoClass("lubelskie", 2));
                        woj.Add(new wojewodztwoClass("lubuskie", 3));
                        woj.Add(new wojewodztwoClass("łódzkie", 4));
                        woj.Add(new wojewodztwoClass("małopolskie", 5));
                        woj.Add(new wojewodztwoClass("mazowieckie", 6));
                        woj.Add(new wojewodztwoClass("opolskie", 7));
                        woj.Add(new wojewodztwoClass("podkarpackie", 8));
                        woj.Add(new wojewodztwoClass("podlaskie", 9));
                        woj.Add(new wojewodztwoClass("pomorskie", 10));
                        woj.Add(new wojewodztwoClass("śląskie", 11));
                        woj.Add(new wojewodztwoClass("świętokrzyskie", 12));
                        woj.Add(new wojewodztwoClass("warmińsko-mazurskie", 13));
                        woj.Add(new wojewodztwoClass("wielkopolskie", 14));
                        woj.Add(new wojewodztwoClass("zachodniopomorskie", 15));
                    }

                    return woj;
                }
            }
        }


        public class rodzajPelnomocnik
        {
            public rodzajPelnomocnik()
            {
            }

            public rodzajPelnomocnik(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }

        public class statusyPelnomocnika
        {
            private ObservableCollection<rodzajPelnomocnik> staPe;

            public ObservableCollection<rodzajPelnomocnik> listaStatPelnomocnika
            {
                get
                {
                    if (staPe == null)
                    {
                        staPe = new ObservableCollection<rodzajPelnomocnik>();

                        staPe.Add(new rodzajPelnomocnik("pełnomocnik inny", 0));
                        staPe.Add(new rodzajPelnomocnik("pelnomocnik zawodowy", 1));

                    }

                    return staPe;
                }
            }
        }

        // słownik rodzaj osoby

        public class rodzajOsoby
        {
            public rodzajOsoby()
            {
            }

            public rodzajOsoby(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }

        public class rodzajeOsob
        {
            private ObservableCollection<rodzajOsoby> roOs;

            public ObservableCollection<rodzajOsoby> rodzajOsoby
            {
                get
                {
                    if (roOs == null)
                    {
                        roOs = new ObservableCollection<rodzajOsoby>();

                        roOs.Add(new rodzajOsoby("Osoba fizyczna", 0));
                        roOs.Add(new rodzajOsoby("Osoba fizyczna prowadząca działalność gospodarczą", 1));
                        roOs.Add(new rodzajOsoby("Osoba prawna", 2));
                        roOs.Add(new rodzajOsoby("Jednostka organizacyjna", 3));
                    }

                    return roOs;
                }
            }
        }
        // Słownik rodzaj rejestru

        public class typSlownik
        {
            public typSlownik()
            {
            }

            public typSlownik(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }


    public class typSlownikFilter
    {
        public typSlownikFilter()
        {
        }

        public typSlownikFilter(string nazwa, int numer, int filter1, int filter2, int filter3, string filter = null )
        {
            Nazwa = nazwa;
            Numer = numer;
            Filter1 = filter1;
            Filter2 = filter2;
            Filter3 = filter3;
            Filter =  filter;

        }

        public string Nazwa
        {
            get;
            set;
        }
        public int Numer
        {
            get;
            set;
        }
        public int Filter1
        {
            get;
            set;
        }
        public int Filter2
        {
            get;
            set;
        }
        public int Filter3
        {
            get;
            set;
        }
        public string Filter
        {
            get;
            set;
        }

    }
    public class rodzajeRejestrow
        {
            private ObservableCollection<typSlownik> roRej;

            public ObservableCollection<typSlownik> rodzajRejestru
            {
                get
                {
                    if (roRej == null)
                    {
                        roRej = new ObservableCollection<typSlownik>();

                        roRej.Add(new typSlownik("Brak rejestracji", 0));
                        roRej.Add(new typSlownik("Rejestracja w KRS", 1));
                        roRej.Add(new typSlownik("Rejstracja w innym rejestrze", 2));  // przy serializacji zmapować na 0  i poznawać po obecnoości tagi inne ..... - tylko na czas edycji

                    }

                    return roRej;
                }
            }
        }

    public class rodzajeSystemow
    {
        private ObservableCollection<typSlownik> roSys;

        public ObservableCollection<typSlownik> rodzajSystemu
        {
            get
            {
                if (roSys == null)
                {
                    roSys = new ObservableCollection<typSlownik>();

                    roSys.Add(new typSlownik("AUMS", 1));
                    roSys.Add(new typSlownik("Selen", 2));
                    roSys.Add(new typSlownik("CC&B", 3));
                    roSys.Add(new typSlownik("Wiena", 11));

                }

                return roSys;
            }
        }
    }



    // Słownik  - koszty zastępstwa

    public class rodzajKZP
        {
            public rodzajKZP()
            {
            }

            public rodzajKZP(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }

        public class rodzajeKZP
        {
            private ObservableCollection<rodzajKZP> roKZP;

            public ObservableCollection<rodzajKZP> rodzajKZP
            {
                get
                {
                    if (roKZP == null)
                    {
                        roKZP = new ObservableCollection<rodzajKZP>();


                        roKZP.Add(new rodzajKZP("Wg norm przypisanych", 1));
                        roKZP.Add(new rodzajKZP("W innej wysokości", 0));
                    }

                    return roKZP;
                }
            }
        }



        public class rodzajSolidarnosci
        {
            public rodzajSolidarnosci()
            {
            }

            public rodzajSolidarnosci(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }


        public class rodzajeSolidarnosci
        {
            private ObservableCollection<rodzajSolidarnosci> roRo;

            public ObservableCollection<rodzajSolidarnosci> rodzajSolidarnosci
            {
                get
                {
                    if (roRo == null)
                    {
                        roRo = new ObservableCollection<rodzajSolidarnosci>();

                        roRo.Add(new rodzajSolidarnosci("Solidarny", 0));
                        roRo.Add(new rodzajSolidarnosci("Inny", 1));
                        roRo.Add(new rodzajSolidarnosci("Inny (powodowie)", 2));
                        roRo.Add(new rodzajSolidarnosci("Inny (pozwani)", 3));
                        roRo.Add(new rodzajSolidarnosci("In Solidum", 4));
                    }

                    return roRo;
                }
            }
        }


        public class rodzajOdsetek
        {
            public rodzajOdsetek()
            {
            }

            public rodzajOdsetek(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }


        public class rodzajeOdsetek
        {
            private ObservableCollection<rodzajOdsetek> roOds;

            public ObservableCollection<rodzajOdsetek> rodzajOds
            {
                get
                {
                    if (roOds == null)
                    {
                        roOds = new ObservableCollection<rodzajOdsetek>();

                        roOds.Add(new rodzajOdsetek("Ustawowe", 0));
            //            roOds.Add(new rodzajOdsetek("Umowne o podanej stopie", 1));
                      //  roOds.Add(new rodzajOdsetek("Umowne o podanej stopie nie większe niż 4x stopa lombardowa", 2));
                      //  roOds.Add(new rodzajOdsetek("Umowne = 4x stopa lombardowa", 3));

                    //roOds.Add(new rodzajOdsetek("Umowne, nie więcej niż ods maks. od dnia/do dnia", 4));
                    //roOds.Add(new rodzajOdsetek("Umowne 4x stopa lombardowa, nie więcej niż ods maks. od dnia/do dnia", 5));
                    //roOds.Add(new rodzajOdsetek("Umowne 4x lombardowa do 31.12.2015 i 4x stopa lombardowa. nie więcej niż w wys. ods maks. od dnia 01.01.2016", 6));
                   // roOds.Add(new rodzajOdsetek("Umowne  w wys. ods. maks. od dnia/do dnia", 7));
                    roOds.Add(new rodzajOdsetek("Ustawowe za opóźnienie", 8));
                  //  roOds.Add(new rodzajOdsetek("Umowne, nie więcej niż w wys. ods. maks. za opóźnienie", 10));
                  //  roOds.Add(new rodzajOdsetek("Umowne w wys. ods. maks. za opóźnienie", 11));
                 //   roOds.Add(new rodzajOdsetek("Umowne, 4x stopa lombardowa do 31.12.2015 i umowne 4x lombardowa, nie więcej niż w wys. ods. maks. za opóźnienie od 1.01.2016", 12));
                 //   roOds.Add(new rodzajOdsetek("Umowne, 4x stopa lombardowa, nie więcej niż w wys. ods. maks. za opóźnienie", 13));
                 //   roOds.Add(new rodzajOdsetek("Umowne, nie więcej niż 4x stopa lombardowa do 31.12.2015 i umowne, nie więcej niż w wys. ods. maks. za opóźnienie od 1.01.2016", 14));
                    roOds.Add(new rodzajOdsetek("Ustawowe za opóźnienie w transakcjach handlowych", 15));
                    //   roOds.Add(new rodzajOdsetek("Wys odsetek za zwłokę  na podstawie Ordynacji podatkowej do dnia 31 grudnia 2015 roku z odsetkami ustawowymi za opóźnienie w transakcjach handlowych od  1.01.2016", 17));
                    //   roOds.Add(new rodzajOdsetek("Zgodnie z opisem", 18));
                    roOds.Add(new rodzajOdsetek("Ustawowe za opóź. podm. lecznicze", 18));

                }

                    return roOds;
                }
            }

            public static ObservableCollection<rodzajOdsetek> GetRodzajOdsetek()
            {
                ObservableCollection<rodzajOdsetek> tmpOds = new ObservableCollection<rodzajOdsetek>();
                tmpOds.Add(new rodzajOdsetek("Ustawowe", 0));
          //      tmpOds.Add(new rodzajOdsetek("Umowne o podanej stopie", 1));
        //        tmpOds.Add(new rodzajOdsetek("Umowne o podanej stopie nie większe niż 4x stopa lombardowa", 2));
         //       tmpOds.Add(new rodzajOdsetek("Umowne = 4x stopa lombardowa", 3));
        //    tmpOds.Add(new rodzajOdsetek("Umowne, nie więcej niż ods maks. od dnia/do dnia", 4));
       //     tmpOds.Add(new rodzajOdsetek("Umowne 4x stopa lombardowa, nie więcej niż ods maks. od dnia/do dnia", 5));
        //    tmpOds.Add(new rodzajOdsetek("Umowne 4x lombardowa do 31.12.2015 i 4x stopa lombardowa. nie więcej niż w wys. ods maks. od dnia 01.01.2016", 6));
         //   tmpOds.Add(new rodzajOdsetek("Umowne  w wys. ods. maks. od dnia/do dnia", 7));
            tmpOds.Add(new rodzajOdsetek("Ustawowe za opóźnienie", 8));
 
          //  tmpOds.Add(new rodzajOdsetek("Umowne, nie więcej niż w wys. ods. maks. za opóźnienie", 10));
        //    tmpOds.Add(new rodzajOdsetek("Umowne w wys. ods. maks. za opóźnienie", 11));
        //    tmpOds.Add(new rodzajOdsetek("Umowne, 4x stopa lombardowa do 31.12.2015 i umowne 4x lombardowa, nie więcej niż w wys. ods. maks. za opóźnienie od 1.01.2016", 12));
        //    tmpOds.Add(new rodzajOdsetek("Umowne, 4x stopa lombardowa, nie więcej niż w wys. ods. maks. za opóźnienie", 13));
        //    tmpOds.Add(new rodzajOdsetek("Umowne, nie więcej niż 4x stopa lombardowa do 31.12.2015 i umowne, nie więcej niż w wys. ods. maks. za opóźnienie od 1.01.2016", 14));
            tmpOds.Add(new rodzajOdsetek("Ustawowe za opóźnienie w transakcjach handlowych", 15));
            tmpOds.Add(new rodzajOdsetek("Ustawowe za opóź. podm. lecznicze", 18));


            //     tmpOds.Add(new rodzajOdsetek("Wys odsetek za zwłokę  na podstawie Ordynacji podatkowej do dnia 31 grudnia 2015 roku z odsetkami ustawowymi za opóźnienie w transakcjach handlowych od  1.01.2016", 17));
            //    tmpOds.Add(new rodzajOdsetek("Zgodnie z opisem", 18));
            return tmpOds;
            }
        }


        public class okresOdsetekowy
        {
            public okresOdsetekowy()
            {
            }

            public okresOdsetekowy(string nazwa, int numer)
            {
                Nazwa = nazwa;
                Numer = numer;
            }

            public string Nazwa
            {
                get;
                set;
            }
            public int Numer
            {
                get;
                set;
            }
        }


        public class okresyOdsetkowe
        {
            private ObservableCollection<okresOdsetekowy> okOds;

            public ObservableCollection<okresOdsetekowy> okresyOds
            {
                get
                {
                    if (okOds == null)
                    {
                        okOds = new ObservableCollection<okresOdsetekowy>();

                        okOds.Add(new okresOdsetekowy("roczne", 0));
                        okOds.Add(new okresOdsetekowy("kwartalne", 1));
                        okOds.Add(new okresOdsetekowy("miesięczne", 2));
                        okOds.Add(new okresOdsetekowy("tygodniowo", 3));
                    okOds.Add(new okresOdsetekowy("dziennie", 3));

                }

                    return okOds;
                }
            }


            public static ObservableCollection<rodzajOdsetek> GetRodzajOdsetek()
            {
                ObservableCollection<rodzajOdsetek> tmpOds = new ObservableCollection<rodzajOdsetek>();
                tmpOds.Add(new rodzajOdsetek("Ustawowe", 0));
      //          tmpOds.Add(new rodzajOdsetek("Umowne o podanej stopie", 1));
       //         tmpOds.Add(new rodzajOdsetek("Umowne o podanej stopie nie większe niż 4x stopa lombardowa", 2));
      //          tmpOds.Add(new rodzajOdsetek("Umowne = 4x stopa lombardowa", 3));
      //      tmpOds.Add(new rodzajOdsetek("Umowne, nie więcej niż ods maks. od dnia/do dnia", 4));
      //      tmpOds.Add(new rodzajOdsetek("Umowne 4x stopa lombardowa, nie więcej niż ods maks. od dnia/do dnia", 5));
      //      tmpOds.Add(new rodzajOdsetek("Umowne 4x lombardowa do 31.12.2015 i 4x stopa lombardowa. nie więcej niż w wys. ods maks. od dnia 01.01.2016", 6));
      //      tmpOds.Add(new rodzajOdsetek("Umowne  w wys. ods. maks. od dnia/do dnia", 7));
            tmpOds.Add(new rodzajOdsetek("Ustawowe za opóźnienie", 8));
      //      tmpOds.Add(new rodzajOdsetek("Umowne, nie więcej niż w wys. ods. maks. za opóźnienie", 10));
      //      tmpOds.Add(new rodzajOdsetek("Umowne w wys. ods. maks. za opóźnienie", 11));
      //      tmpOds.Add(new rodzajOdsetek("Umowne, 4x stopa lombardowa do 31.12.2015 i umowne 4x lombardowa, nie więcej niż w wys. ods. maks. za opóźnienie od 1.01.2016", 12));
      //      tmpOds.Add(new rodzajOdsetek("Umowne, 4x stopa lombardowa, nie więcej niż w wys. ods. maks. za opóźnienie", 13));
      //      tmpOds.Add(new rodzajOdsetek("Umowne, nie więcej niż 4x stopa lombardowa do 31.12.2015 i umowne, nie więcej niż w wys. ods. maks. za opóźnienie od 1.01.2016", 14));
            tmpOds.Add(new rodzajOdsetek("Ustawowe za opóźnienie w transakcjach handlowych", 15));
            tmpOds.Add(new rodzajOdsetek("Ustawowe za opóź. podm. lecznicze", 18));
            //     tmpOds.Add(new rodzajOdsetek("Wys odsetek za zwłokę  na podstawie Ordynacji podatkowej do dnia 31 grudnia 2015 roku z odsetkami ustawowymi za opóźnienie w transakcjach handlowych od  1.01.2016", 17));
            //    tmpOds.Add(new rodzajOdsetek("Zgodnie z opisem", 18));
            return tmpOds;
            }
        }

        public class rodzajeJednostekWindyk
        {
            private ObservableCollection<radcaSlownik> roJe;

            public ObservableCollection<radcaSlownik> rodzajeJednostek
            {
                get
                {
                    if (roJe == null)
                    {
                        roJe = new ObservableCollection<radcaSlownik>();

                        roJe.Add(new radcaSlownik("Zewnętrzna kancelaria prawna", 1));
                        roJe.Add(new radcaSlownik("Wewnętrzna komórka windykacji", 2));

                    }

                    return roJe;
                }
            }
        }

        public class rodzajeRol
        {
            private ObservableCollection<rodzajDokumentu> roRol;

            public ObservableCollection<rodzajDokumentu> rodzajeRolUzytkownikow
            {
                get
                {
                    if (roRol == null)
                    {
                        roRol = new ObservableCollection<rodzajDokumentu>();

                        roRol.Add(new rodzajDokumentu("Pracownik windykacji", 0));
                        roRol.Add(new rodzajDokumentu("Administrator jednostki", 1));
                    if (UserProfile.Rola == 2)
                    {
                        roRol.Add(new rodzajDokumentu("Administrator systemu", 2));
                        roRol.Add(new rodzajDokumentu("Operator KRD", 3));
                        roRol.Add(new rodzajDokumentu("Operator UnZD", 4));
                    }
                    }

                    return roRol;
                }
            }
        }


        public class rodzajeFiltra
        {
            private ObservableCollection<rodzajDokumentu> roFiltra;

            public ObservableCollection<rodzajDokumentu> RodzajeFiltra 
            {
                get
                {
                    if (roFiltra == null)
                    {
                        roFiltra = new ObservableCollection<rodzajDokumentu>();

                        roFiltra.Add(new rodzajDokumentu("Brak Filtra", 0));
                        roFiltra.Add(new rodzajDokumentu("Ozanczenie wg powoda", 1));
                        roFiltra.Add(new rodzajDokumentu("Sygnatura sprawy NC-e", 2));
                        roFiltra.Add(new rodzajDokumentu("Status dokumentu", 3));

                    }

                    return roFiltra;
                }
            }
        }

        public class rodzajeFiltraDaty
        {
            private ObservableCollection<rodzajDokumentu> roFiltra;

            public ObservableCollection<rodzajDokumentu> RodzajeFiltraDaty
            {
                get
                {
                    if (roFiltra == null)
                    {
                        roFiltra = new ObservableCollection<rodzajDokumentu>();

                        roFiltra.Add(new rodzajDokumentu("Brak Filtra", 0));
                        roFiltra.Add(new rodzajDokumentu("Data wysłania doręczenia", 1));
                        roFiltra.Add(new rodzajDokumentu("Data doręczenia", 2));
                        

                    }

                    return roFiltra;
                }
            }
        }


        public class rodzajeDokumentowEPU
        {
            private ObservableCollection<rodzajDokumentu> roDok;

            public ObservableCollection<rodzajDokumentu> RodzajeDokumentowEPU
            {
                get
                {
                    if (roDok == null)
                    {
                        roDok = new ObservableCollection<rodzajDokumentu>();

                        roDok.Add(new rodzajDokumentu("Pismo", 3));
                        roDok.Add(new rodzajDokumentu("Wniosek", 4));
                        roDok.Add(new rodzajDokumentu("Uzupełnienie adresu", 5));
                        roDok.Add(new rodzajDokumentu("Uzupełnienie braków", 6));
                        roDok.Add(new rodzajDokumentu("Rezygnacja z pełnomocnictwa", 13));
                        roDok.Add(new rodzajDokumentu("Zgłoszenie pełnomocnika do sprawy", 14));
                        roDok.Add(new rodzajDokumentu("Dokumentacja inicjująca postępowanie", 999));

                }

                    return roDok;
                }
            }

            public string GetNameByNumber(int nr)
            {
                foreach (var r in RodzajeDokumentowEPU)
                {
                    if (r.Numer == nr) return r.Nazwa; 
                
                }
                return "";
            }
        }

        public class StatusDetails
        {
            private ObservableCollection<rodzajDokumentu> _statDetails;

            public ObservableCollection<rodzajDokumentu> SzczegolyStatusu
            {
                get
                {
                    if (_statDetails == null)
                    {
                        _statDetails = new ObservableCollection<rodzajDokumentu>();

                        _statDetails.Add(new rodzajDokumentu("????", 0));
                        _statDetails.Add(new rodzajDokumentu("Wycofano z obsługi", 1));
                        _statDetails.Add(new rodzajDokumentu("Ugoda", 2));
                        

                    }

                    return _statDetails;
                }
            }

            
        }

        public class SlownikStatusowDokumentowClass
        {
            private ObservableCollection<statusPaczki> statDok;

            public ObservableCollection<statusPaczki> statusyDokumentow
            {
                get
                {
                    {
                        statDok = new ObservableCollection<statusPaczki>();

                        //roDok.Add(new rodzajDokumentu("Sprzeciw", 0));
                        statDok.Add(new statusPaczki("projekt", 1));
                        statDok.Add(new statusPaczki("zatwierdzony", 2));
                        statDok.Add(new statusPaczki("złożony", 3));
                        statDok.Add(new statusPaczki("zwrócony", 4));
                        statDok.Add(new statusPaczki("odrzucony", 5));
                        statDok.Add(new statusPaczki("usunięty", 6));
                    }

                    return statDok;
                }
            }
        }


        public static class MainWorkspaceWindowHandler
        {
            public static object WinHandler { get; set; }
            public static object MainViewHandler { get; set; }
            public static object MenuClassHandler { get; set; }    // uchwyt do klasy 
        }

    /// Slownik oddziałow Wiena

    public class oddzialyWiena
    {
        private ObservableCollection<typSlownikFilter> _typOddzialyWiena;


       


        private void GetWienaDocCompleted(InvokeOperation<string> result)
        {// deserializacja
     
            string message; 
            if (result != null && !result.HasError)
            {
                
                message = result.Value;
                var obj = ToXMLSerializers.XmlDeserializeFromString(message, typeof(List<TypSlownikFiltered>));
                foreach (TypSlownikFiltered item in ( obj as List<TypSlownikFiltered>) )
                {
                    _typOddzialyWiena.Add(new  typSlownikFilter(item.Nazwa,item.Numer, item.Filter1, item.Filter2, item.Filter3));
                }


            }

        }

        public ObservableCollection<typSlownikFilter> typOddzialyWiena
        {



            get
            {
                if (_typOddzialyWiena == null)
                {
                    _typOddzialyWiena = new ObservableCollection<typSlownikFilter>();
                    PozewDomainContext poz = new PozewDomainContext();
                    poz.GetWienaOddzialy (LexEnaKonfiguracja.Firma, GetWienaDocCompleted, null);

                }

                return _typOddzialyWiena;
            }
        }
    }
    /*public static class symboleSprawWiena
    {
        private static ObservableCollection<typSlownikFilter> _typSymboleSprawWiena;


        private static void _loadSymbole()
        {
            PozewDomainContext poz = new PozewDomainContext();
            poz.GetWienaSymbole(LexEnaKonfiguracja.Firma, GetWienaDocCompleted, null);

        }




        public static void ReloadSymbole()
        {
            _loadSymbole();



        }


        private static  void GetWienaDocCompleted(InvokeOperation<string> result)
        {// deserializacja

            string message;
            if (result != null && !result.HasError)
            {

                message = result.Value;
                var obj = ToXMLSerializers.XmlDeserializeFromString(message, typeof(List<TypSlownikFiltered>));
                foreach (TypSlownikFiltered item in (obj as List<TypSlownikFiltered>))
                {

                    _typSymboleSprawWiena.Add(new typSlownikFilter(item.Nazwa, item.Numer, item.Filter1, item.Filter2,item.Filter3));
                }

                LexEnaKonfiguracja.SlownikSymboliWiena = _typSymboleSprawWiena;
            }

        }



        public static ObservableCollection<typSlownikFilter> typSymboleSprawWiena
        {
            get
            {
                if (_typSymboleSprawWiena == null)
                {
                    _typSymboleSprawWiena = new ObservableCollection<typSlownikFilter>();
                    _loadSymbole();

                }

                return _typSymboleSprawWiena;
            }

        }
    }
    */
    public  class symboleSprawWiena
    {
        private  ObservableCollection<typSlownikFilter> _typSymboleSprawWiena;


        private  void _loadSymbole()
        {
            PozewDomainContext poz = new PozewDomainContext();
            poz.GetWienaSymbole(LexEnaKonfiguracja.Firma, GetWienaDocCompleted, null);

        }




        public  void ReloadSymbole()
        {
            _loadSymbole();



        }


        private  void GetWienaDocCompleted(InvokeOperation<string> result)
        {// deserializacja

            string message;
            if (result != null && !result.HasError)
            {

                message = result.Value;
                var obj = ToXMLSerializers.XmlDeserializeFromString(message, typeof(List<TypSlownikFiltered>));
                foreach (TypSlownikFiltered item in (obj as List<TypSlownikFiltered>))
                {

                    _typSymboleSprawWiena.Add(new typSlownikFilter(item.Nazwa, item.Numer, item.Filter1, item.Filter2, item.Filter3));
                }

                LexEnaKonfiguracja.SlownikSymboliWiena = _typSymboleSprawWiena;
            }

        }



        public  ObservableCollection<typSlownikFilter> typSymboleSprawWiena
        {
            get
            {
                if (_typSymboleSprawWiena == null)
                {
                    _typSymboleSprawWiena = new ObservableCollection<typSlownikFilter>();
                    _loadSymbole();

                }

                return _typSymboleSprawWiena;
            }

        }
    }
    public class oznaczeniaDowodowWiena
    {
        private ObservableCollection<typSlownikFilter> _typOznaczeniaDowodowWiena;


        private void GetWienaDocCompleted(InvokeOperation<string> result)
        {// deserializacja
        
            string message;
            if (result != null && !result.HasError)
            {

                message = result.Value;
                var obj = ToXMLSerializers.XmlDeserializeFromString(message, typeof(List<TypSlownikFiltered>));
                foreach (TypSlownikFiltered item in (obj as List<TypSlownikFiltered>))
                {

                    _typOznaczeniaDowodowWiena.Add(new typSlownikFilter(item.Nazwa, item.Numer, item.Filter1, item.Filter2, item.Filter3));
                }

             
            }

        }

        public ObservableCollection<typSlownikFilter> typOznaczeniaDowodowWiena
        {



            get
            {
                if (_typOznaczeniaDowodowWiena == null)
                {
                    _typOznaczeniaDowodowWiena = new ObservableCollection<typSlownikFilter>();
                    PozewDomainContext poz = new PozewDomainContext();
                    poz.GetWienaDowod (GetWienaDocCompleted, null);

                }

                return _typOznaczeniaDowodowWiena;
            }
        }
    }


    public class  wienaConfig
    {
        private WienaConfig _typWienaConfig;
        private ObservableCollection<typSlownikFilter> _typOddzialy;
        private ObservableCollection<typSlownikFilter> _typKonta;
        private ObservableCollection<typSlownikFilter> _typStatusy;
        private ObservableCollection<typSlownikFilter> _typKancelarie;
        private ObservableCollection<typSlownikFilter> _typRadcowie;

        private void GetWienaConfigCompleted(InvokeOperation<string> result)
        {// deserializacja

            string message;
            if (result != null && !result.HasError)
            {

                message = result.Value;
             
                var obj = ToXMLSerializers.XmlDeserializeFromString(message, typeof(WienaConfig));

                _typWienaConfig = (WienaConfig)obj;
               if (_typWienaConfig != null)
                {


                    /*
                    if (_typWienaConfig.Kancelarie != null)
                    {
                       
                        foreach (TypSlownikFiltered item in _typWienaConfig.Kancelarie)
                        {
                            _typKancelarie.Add(new typSlownikFilter(item.Nazwa, item.Numer, item.Filter1, item.Filter2, item.Filter3));

                        }
                    }
                    */
                    if (_typWienaConfig.Oddzialy != null)
                    {
                     
                        foreach (TypSlownikFiltered item in _typWienaConfig.Oddzialy)
                        {
                     
                            _typOddzialy.Add(new typSlownikFilter(item.Nazwa, item.Numer, item.Filter1, item.Filter2, item.Filter3));

                        }
                    }
                    if (_typWienaConfig.Statusy != null)
                    {
                        foreach (TypSlownikFiltered item in _typWienaConfig.Statusy)
                        {
                            string filter;
                            switch (item.Filter1)
                            {
                                case 1:
                                    filter = "1.Sądowe";
                                    break;
                                case 2:
                                    filter = "2.Egzekucja kom.";
                                    break;
                                case 3:
                                    filter = "3.Updałościowe";
                                    break;
                                case 4:
                                    filter = "4.Inne";
                                    break;
                                default:
                                    filter = "4.Inne";
                                    break;
                            }

                            _typStatusy.Add(new typSlownikFilter(item.Nazwa, item.Numer, item.Filter1, item.Filter2, item.Filter3,filter));

                        }
                    }
                    if (_typWienaConfig.Radcowie   != null)
                    {
                        foreach (TypSlownikFiltered item in _typWienaConfig.Radcowie)
                        {
                            _typRadcowie.Add(new typSlownikFilter(item.Nazwa, item.Numer, item.Filter1, item.Filter2, item.Filter3));

                        }
                    }
                    if (_typWienaConfig.Kancelarie != null)
                    {
                        foreach (TypSlownikFiltered item in _typWienaConfig.Kancelarie)
                        {
                            _typKancelarie.Add(new typSlownikFilter(item.Nazwa, item.Numer, item.Filter1, item.Filter2, item.Filter3));

                        }
                    }
                    if (_typWienaConfig.Konta != null)
                    {
                        foreach (TypSlownikFiltered item in _typWienaConfig.Konta)
                        {
                            _typKonta.Add(new typSlownikFilter(item.Nazwa, item.Numer, item.Filter1, item.Filter2, item.Filter3));

                        }
                    }

                }
                /*
                foreach (TypSlownikFiltered item in (obj as List<TypSlownikFiltered>))
                {

                    _Wiena.Add(new typSlownikFilter(item.Nazwa, item.Numer, item.Filter1, item.Filter2, item.Filter3));
                }
                */

            }

        }
        private void initLists()
        {
            _typOddzialy = new ObservableCollection<typSlownikFilter>();
            _typKancelarie = new ObservableCollection<typSlownikFilter>();
            _typStatusy = new ObservableCollection<typSlownikFilter>();
            _typRadcowie = new ObservableCollection<typSlownikFilter>();
            _typKonta = new ObservableCollection<typSlownikFilter>();
        }


        public ObservableCollection<typSlownikFilter> typKontaWiena
        {



            get
            {
                if (_typWienaConfig == null)
                {
                    _typWienaConfig = new WienaConfig();
                    initLists();
                    PozewDomainContext poz = new PozewDomainContext();
                    poz.GetWienaConfig(UserProfile.Firma, GetWienaConfigCompleted, null);

                }

                return _typKonta;
            }
        }



        public ObservableCollection<typSlownikFilter> typOddzialyWiena
        {



            get
            {
                if (_typWienaConfig == null)
                {
                    _typWienaConfig = new WienaConfig();
                    initLists();
                    PozewDomainContext poz = new PozewDomainContext();
                    poz.GetWienaConfig(UserProfile.Firma, GetWienaConfigCompleted, null);

                }

                return _typOddzialy;
            }
        }
        


       public ObservableCollection<typSlownikFilter> typRadcowieWiena
        {

            
            get
            {
                if (_typWienaConfig == null)
                {
                    _typWienaConfig = new WienaConfig();
                    initLists();
                    PozewDomainContext poz = new PozewDomainContext();
                    poz.GetWienaConfig(UserProfile.Firma, GetWienaConfigCompleted, null);

                }

                return _typRadcowie;
            }
        }

        public ObservableCollection<typSlownikFilter> typKancelarieWiena
        {


            get
            {
                if (_typWienaConfig == null)
                {
                    _typWienaConfig = new WienaConfig();
                    initLists();
                    PozewDomainContext poz = new PozewDomainContext();
                    poz.GetWienaConfig(UserProfile.Firma, GetWienaConfigCompleted, null);

                }

                return _typKancelarie;
            }
        }

        public ObservableCollection<typSlownikFilter> typStatusyWiena
        {



            get
            {
                if (_typWienaConfig == null)
                {
                    _typWienaConfig = new WienaConfig();
                    initLists();
                    PozewDomainContext poz = new PozewDomainContext();
                    poz.GetWienaConfig(UserProfile.Firma, GetWienaConfigCompleted, null);

                }

                return _typStatusy;
            }
        }
    }

}

