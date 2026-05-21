
namespace LexEnaTrs.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using LexEnaTrs;
    using System.Runtime.InteropServices;
    using System.Web.Security;
    using LexEnaTrs.Web.Imports;
    using WienaDB.Models;
    using WienaDB;
    using BIG;
    using System.Xml.Serialization;
    using System.IO;
    using LexEna2BIG;
    using System.Xml;
    using System.Text;
    using Nicci.Input;
    using System.Data.SqlClient;
    using System.Runtime.Serialization;
    using LexEnaTrs.Web.UZD;
    using Tools1;
    using System.Web.Hosting;
    using System.Web.Configuration;
    using System.Configuration;
    using System.Diagnostics;
    using LexEnaTrs.Web.Models;
    using Zgony;
    using System.IO.Compression;
    using System.Net;
    using System.Globalization;
    using System.Net.Security;
    using System.Web.Script.Serialization;

    public class caseDescriptor
    {
        public int IdSprawy;
        public string Sygnatura;
        public string Nce;
        public decimal nalgl;
        public decimal wdz;
        public decimal odsKapital;
        public decimal odsNaliczoneSkapitalizowane;
        public decimal kosztySadowe;
        public decimal kzp;
        public decimal kosztyInne;   
        public byte[]  trescDok;
        public DateTime dataDok;
        public DateTime dataRej;
        public int caseType;
        public string docName;
        public ImportValidators.docTypes dt;
    }     

    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public class PozewDomainService : DomainService
    {
        private string zipContent;


        [Serializable]
        public class IdList : List<int>
        {
            private List<int> _lista;
            public List<int> ListaId
            {
                get
                {
                    return this._lista;
                }
                set
                {
                    this._lista = value;

                }


            }


        }


        [Serializable]
        public class StrList : List<string>
        {
            private List<string> _lista;
            public List<string> ListaStr
            {
                get
                {
                    return this._lista;
                }
                set
                {
                    this._lista = value;

                }


            }


        }

        [Invoke()]
        public bool UserChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (Membership.ValidateUser(userName, oldPassword))
            {
                MembershipUser memUser = Membership.GetUser(userName);

                return memUser.ChangePassword(oldPassword, newPassword);
            }
            return false;
        }
        [Invoke()]
        public string UserResetPassword(string userName, string answer)
        {

            try
            {
                MembershipUser memuser = Membership.GetUser(userName);
                if (memuser != null)
                {
                    return String.IsNullOrWhiteSpace(answer) ? memuser.ResetPassword() : memuser.ResetPassword(answer);
                }
                else
                    return "Błąd brak użytkownika o podanej nazwie";
            }
            catch (Exception ex)
            {
                return "Błąd " + ex.Message;

            }
        }

        [Invoke()]
        public string UserUnlockAccount(string userName)
        {

            try
            {
                MembershipUser memuser = Membership.GetUser(userName);
                if (memuser != null)
                {
                    if (memuser.IsLockedOut)
                        return memuser.UnlockUser() ? "Konto zostało odblokowane" : "Błąd podczas próby odblokowania konta";
                    else
                        return "Konto użytkownika nie jest zablokowane";
                }
                else
                    return "Błąd brak użytkownika o podanej nazwie";
            }
            catch (Exception ex)
            {
                return "Błąd " + ex.Message;

            }
        }
        /*
        [Invoke()]
        public string UserLockAccount(string userName)
        {

            try
            {
                MembershipUser memuser = Membership.GetUser(userName);
                if (memuser != null)
                {
                    if (memuser.IsLockedOut) return "Konto jest zablokowane";
                   return memuser.L ? "Konto zostało odblokowane" : "Błąd podczas próby odblokowania konta";
                }
                else
                    return "Błąd brak użytkownika o podanej nazwie";
            }
            catch (Exception ex)
            {
                return "Błąd " + ex.Message;

            }
        }
        */
        [Invoke()]
        public string GetUserSecurityQuestion(string userName)
        {

            try
            {
                MembershipUser memuser = Membership.GetUser(userName);
                if (memuser != null)
                {

                    return memuser.PasswordQuestion;
                }
                else
                    return "Błąd, brak użytkownika o podanej nazwie";
            }
            catch (Exception ex)
            {
                return "Błąd " + ex.Message;

            }
        }


        [Invoke()]
        // [RequiresAuthentication]
        public string ListaDok2HTML(string listaId, int what)//PozewEPU _pozew) // serializacja pozwu EPU  listy pozwów
        {
            string fname;
            try
            {
                fname = XML2HTMLTransform.TransformSeries(listaId, what);

            }
            catch (Exception e)
            {


                return "error: " + e.Message;
            }


            return fname;
        }

        [Invoke()]
        // [RequiresAuthentication]
        public string ListaDok2XML(string listaId, int what)//PozewEPU _pozew) // serializacja pozwu EPU  listy pozwów
        {
            string fname;
            try
            {
                fname = XML2HTMLTransform.TransformSeriesToXML(listaId, what);

            }
            catch (Exception e)
            {


                return "error: " + e.Message;
            }


            return fname;
        }


        [Invoke()]
        // [RequiresAuthentication]
        public string ExtraXMLGet(string listaId)//PozewEPU _pozew) // serializacja pozwu EPU  listy pozwów
        {
            string fname;
            try
            {
                fname = XML2HTMLTransform.ConcatenateXML(listaId);

            }
            catch (Exception e)
            {


                return "error: " + e.Message;
            }


            return fname;
        }


        [Invoke()]
        // [RequiresAuthentication]
        public string GetListaDokOdebr(string listaId, int UserRole, int UserId)// sklej html'i         
        {
            string fname;
            try
            {
                fname = XML2HTMLTransform.ConcatDokOdebr(listaId);
                // zaznaczenie odczytu 
                if (fname.Length > 5)
                {
                    XML2HTMLTransform.markAsRead(listaId, UserRole, UserId);

                }

            }
            catch (Exception e)
            {


                return "error: " + e.Message;
            }


            return fname;
        }



        [Invoke()]
        // [RequiresAuthentication]
        public string DokumentZEPU2HTML(string pozewSerialized, int typDok)//PozewEPU _pozew) // serializacja pozwu EPU
        {
            string content;
            try
            {
                content = XML2HTMLTransform.Transform(pozewSerialized, typDok);

            }
            catch (Exception e)
            {


                return "error: " + e.Message;
            }


            return content;
        }

        [Invoke()]
        public string ValidateDokEPUXSD(string dokXML, int dokType)//PozewEPU _pozew) // serializacja pozwu EPU
        {
            XMLValidator _validator = new XMLValidator();
            try
            {
                _validator.ValidateDokumentZEPU(dokXML, dokType);
            }
            catch (ArgumentException ex)
            {
                return ex.Message + " " + ex.InnerException;
            }
            return "OK";

        }

        [Invoke()]
        public string GetHtmlDocOdebr(int IdDoc, int UserRole, int UserId) // odczyt html'a z dokumentu odebranego z EPU       //PozewEPU _pozew) // serializacja pozwu EPU
        {
            DokOdebr DokOdebrany;
            string fileName = "";
            try
            {
                using (var _meritum = new LexEnaMeritumEntities())
                {
                    var query = from doOd in _meritum.DokOdebr
                                where doOd.Id == IdDoc
                                select doOd;

                    if (query.Count() == 1)
                    {
                        DokOdebrany = query.FirstOrDefault();
                        fileName =  DokOdebrany.TrescHtml;

                        if (fileName.Contains("Błąd"))
                            return "Błąd zapisu zbioru";
                        XML2HTMLTransform.markAsRead(IdDoc, UserRole, UserId);
                        return fileName;

                    }
                }

            }
            catch (ArgumentException ex)
            {
                return ex.Message + " " + ex.InnerException;
            }
            return "Błąd : brak zbioru html";

        }
        [Invoke()]
        public string LunchApp(int appType) // odczyt html'a z dokumentu odebranego z EPU       //PozewEPU _pozew) // serializacja pozwu EPU
        {

            string appPath;

            Process[] pname = Process.GetProcessesByName("ZadanieTimer");
            if (pname.Length > 0)
                return "Proces komunikacji z EPU jest uruchomiony. Poczekaj na jego zakończenie.";

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            try
            {
                appPath = System.Web.Hosting.HostingEnvironment.MapPath("~/ZadanieTimer/");
                info.WorkingDirectory = appPath;
                info.FileName = "ZadanieTimer.exe";
                proc.StartInfo = info;
                proc.Start();

            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd uruchamiana ZadanieTimer" + ex.Message);
                return "Błąd uruchamiana ZadanieTimer " + ex.Message + " " + ex.InnerException != null ? " " + ex.InnerException.Message : "";
            }
            return "OK";

        }

        private ZadanieSet setupZadanie(Uzytkownik user, int typZadania, LexEnaMeritumEntities context)
        {
            ZadanieSet zadanieToAdd = new ZadanieSet();
            zadanieToAdd.TypZadaniaId = typZadania;
            zadanieToAdd.Status = 0;
            zadanieToAdd.Oczasie = false;
            zadanieToAdd.DataRozpoczęcia = DateTime.Today;
            switch (typZadania)
            {
                case 3: // MOje Sprawy
                    zadanieToAdd.NazwaZadania = "Moje sprawy";
                    break;
                case 5: //MojeSprawyEPU Nakazy
                    zadanieToAdd.NazwaZadania = "Moje nakazy";
                    break;
                case 7: //Moje orzeczenia
                    zadanieToAdd.NazwaZadania = "Moje orzeczenia";
                    break;
                default:
                    return null;

            }
            zadanieToAdd.NazwaZadania += " " + user.JednostkaWindykacji.Nazwa;
            int idKonta = user.KontoEPU_Id.Value;
            string kontoStr = "<KontoEpuId>" + idKonta.ToString() + "</KontoEpuId>";

            ZadanieSet prevZad = context.ZadanieSet.Where(a => a.TypZadaniaId == typZadania && a.Status > 0 && a.Parametry.Contains(kontoStr)).OrderByDescending(a => a.DataZakonczenia).FirstOrDefault();
            // parametry 

            EPUParamModel newParams = new EPUParamModel();
          

            newParams.KryteriumFiltrowania = null;
            newParams.DataDo = DateTime.Today.AddDays(1).AddMinutes(-1);
            newParams.DataOd = prevZad == null ? DateTime.Today.AddDays(-40) :(prevZad.DataZakonczenia.Value < new DateTime(2020,11,22) ? new DateTime(2020 , 11 , 22) : prevZad.DataZakonczenia.Value.AddDays(-2));
            newParams.IdPozwuWLexEna = 0;
            newParams.IdZazaleniaWLexEna = 0;
            newParams.IdSkargiWLexEna = 0;
            newParams.IdSprzeciwWLexEna = 0;
            newParams.IdSprzeciwWLexEna = 0;
            newParams.KontoEpuId = user.KontoEPU_Id.Value;
            newParams.Rok = 0;
            zadanieToAdd.Parametry = ToXMLSerializers.SerializeToString(newParams, typeof(EPUParamModel), false);

            return zadanieToAdd;


        }

        [Invoke()]
        public string InsertJobs(int UserId) // odczyt html'a z dokumentu odebranego z EPU       //PozewEPU _pozew) // serializacja pozwu EPU
        {
            
            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {
                try
                {
                    Uzytkownik u = context.Uzytkownik.Where(a => a.Id == UserId).FirstOrDefault();
                    if (u == null)
                        return "Błąd, brak takiego użytkownika";
                    ZadanieSet zadanieToAdd = setupZadanie(u, 3, context);
                    if (zadanieToAdd == null)
                        return "Błąd podczas wystawiania zadania";
                    context.ZadanieSet.AddObject(zadanieToAdd);
                    
                    ZadanieSet zadanieToAddNak = setupZadanie(u, 5, context);
                    if (zadanieToAddNak == null)
                        return "Błąd podczas wystawiania zadania";
                    context.ZadanieSet.AddObject(zadanieToAddNak);

                    ZadanieSet zadanieToAddOrz = setupZadanie(u, 7, context);
                    if (zadanieToAddOrz == null)
                        return "Błąd podczas wystawiania zadania";
                    context.ZadanieSet.AddObject(zadanieToAddOrz);
                   

                    context.SaveChanges();
                    


                }
                catch (Exception ex)
                {
                    Utils.LogWriter("Błąd podczas zapisu zadań " + ex.Message);
                    return "Błąd podczas zapisu zadania" + ex.Message;

                }



            }
            // uruchomienie Zadania 
            
            return "";

        }

        [Invoke()]
        // [RequiresAuthentication]
        public string GenerateDocuments(string listaIdSpr, int docType, int IdKontaEPU)// sklej html'i         
        {
            DokEPU dokEPU;
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                List<int> listId;
                listId = new List<int>();
                listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaIdSpr, typeof(List<int>));
                // lista spraw 
                /*
                using (var _meritum = new LexEnaMeritumEntities())
                {
                    var query = from dp in _meritum.vw_tmpZmianaAdresuPeln
                                select dp.id;

                    listId.Clear();
                    foreach (var j in query)
                    {

                        listId.Add(j);
                    }
                }
                 */

                foreach (int i in listId)
                {
                    dokEPU = new DokEPU();
                    if (dokEPU.DocEPU(i, docType, IdKontaEPU) < 0)
                    {
                        return "error: " + "Błąd podczas generacji dokumentu dla sprawy ID " + i.ToString();

                    }
                    else
                    {
                        if (dokEPU.AddDokToDb() < 0) return "error: " + "Błąd zapisu sprawy ID " + i.ToString();

                    }
                }


            }
            catch (Exception e)
            {


                return "error: " + e.Message;
            }


            return "OK";
        }

        [Invoke()]
        // [RequiresAuthentication]
        public string GenerateWniosek(string listaIdSpr, int IdKontaEPU, DateTime DataWniosku, int SzablonId)// sklej html'i         
        {
            DokEPU dokEPU;
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                List<int> listId;
                listId = new List<int>();
                listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaIdSpr, typeof(List<int>));
                // lista spraw 
                /*
                using (var _meritum = new LexEnaMeritumEntities())
                {
                    var query = from dp in _meritum.vw_tmpZmianaAdresuPeln
                                select dp.id;

                    listId.Clear();
                    foreach (var j in query)
                    {

                        listId.Add(j);
                    }
                }
                 */
                foreach (int i in listId)
                {
                    dokEPU = new DokEPU();
                    if (dokEPU.AddPozewEPU(i, DataWniosku, IdKontaEPU, SzablonId) < 0)
                    {
                        return "error: " + "Błąd podczas generacji Wniosku egzekucyjnego dla sprawy ID " + i.ToString();

                    }
                    else
                    {
                        if (dokEPU.AddPozewToDb() < 0) return "error: " + "Błąd zapisu sprawy ID " + i.ToString();

                    }
                }


            }
            catch (Exception e)
            {


                return "error: " + e.Message;
            }


            return "OK";
        }

        [Invoke()]
        // [RequiresAuthentication]
        public string SetNewStatus(string listaIdSpr, string listaDok, DateTime DataStatusu, int Glowny, int Dodatkowy, int Dokumentu) // sklej html'i         
        {


            try
            {
                // generacja dokumentów na podstawie Id spraw.
                List<int> listId;
                listId = new List<int>();
                using (var _meritum = new LexEnaMeritumEntities())
                {
                    if (Glowny > 0)
                    {
                        listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaIdSpr, typeof(List<int>));

                        foreach (int i in listId)
                        {
                            StatusSprawy statspr = new StatusSprawy();

                            statspr.czyus = 0;
                            statspr.CzyWiena = 0;
                            statspr.DataStatusu = DataStatusu;
                            statspr.NazwaStatusu_Id = Glowny;
                            statspr.ExtraStat = Dodatkowy;
                            statspr.Sprawa_id = i;
                            _meritum.AddToStatusSprawy(statspr);

                        }

                    }
                    if (Dokumentu > 0)
                    {
                        listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaDok, typeof(List<int>));
                        foreach (int i in listId)
                        {


                            var query = from doWys in _meritum.DokWys
                                        where doWys.Id == i
                                        select doWys;

                            if (query.Count() == 1)

                                query.FirstOrDefault<DokWys>().StatusDok = Dokumentu;


                        }


                    }
                    _meritum.SaveChanges();
                }
            }
            catch (Exception e)
            {


                return "error: " + e.Message;
            }


            return "OK";
        }


        [Invoke()]
        // [RequiresAuthentication]
        public string GeneratePozew(string listaIdSpr, int IdKontaEPU, DateTime DataWniosku, int generatemode) // sklej html'i         
        {
            DokEPU dokEPU;
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                List<int> listId;
                listId = new List<int>();
                listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaIdSpr, typeof(List<int>));
                // lista spraw 
                /*
                using (var _meritum = new LexEnaMeritumEntities())
                {
                    var query = from dp in _meritum.vw_tmpZmianaAdresuPeln
                                select dp.id;

                    listId.Clear();
                    foreach (var j in query)
                    {

                        listId.Add(j);
                    }
                }
                 */
                foreach (int i in listId)
                {
                    dokEPU = new DokEPU();
                    if (dokEPU.AddPozewEPU(i, DataWniosku, IdKontaEPU, generatemode) < 0)
                    {
                        return "error: " + "Błąd podczas generacji Pozwu dla sprawy ID " + i.ToString();

                    }
                    else
                    {
                        //if (dokEPU.AddWniosekToDb() < 0) return "error: " + "Błąd zapisu sprawy ID " + i.ToString();

                    }
                }


            }
            catch (Exception e)
            {


                return "error: " + e.Message;
            }


            return "OK";
        }

        [Invoke()]
        public string DeleteNaleznosc(int Id_Nal)
        {

            try
            {
                // deserializacja 
                using (var _meritum = new LexEnaMeritumEntities())
                {
                    var queryx = from wp in _meritum.WplataPodz
                                 where wp.Naleznosc_Id == Id_Nal
                                 select wp;

                    foreach (var nx in queryx)
                    {
                        if (nx.SplataKapital != 0 || nx.SplataOdsetki != 0)
                        {

                            return "Nie można usunąć należności, na którą zaliczono  wpłatę";
                        }

                    }

                    foreach (var ny in queryx)
                    {
                        _meritum.WplataPodz.DeleteObject(ny);

                    }

                    var query = from n in _meritum.Odsetki
                                where n.Naleznosc_Id == Id_Nal
                                select n;

                    foreach (var ods in query)
                    {
                        _meritum.Odsetki.DeleteObject(ods);

                    }

                    var query1 = from s in _meritum.StanNaleznosci
                                 where s.Naleznosc_Id == Id_Nal
                                 select s;

                    foreach (var sn in query1)
                    {
                        _meritum.StanNaleznosci.DeleteObject(sn);

                    }



                    var query2 = from nal in _meritum.Naleznosc
                                 where nal.Id == Id_Nal
                                 select nal;
                    foreach (var nx in query2)
                    {
                        _meritum.Naleznosc.DeleteObject(nx);

                    }

                    _meritum.SaveChanges();
                    return "OK";
                    /*_meritum.SavingChanges += (ob, ea) =>
                    {
                        ArgumentException ex = new ArgumentException();
                        ex.Data.Add("result", 1);
                        throw ex;
                    }; */
                }
            }
            catch (ArgumentException ae)
            {

                return "Błąd podczas usuwania- " + ae.Message;
            }


            //return 0;

        }



        [Invoke()]
        public int DeleteDokumentPaczka(string listaId)
        {
            List<int> listId;
            listId = new List<int>();
            listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaId, typeof(List<int>));

            try
            {
                // deserializacja 
                using (var _meritum = new LexEnaMeritumEntities())
                {
                    var query = from dp in _meritum.DokumentPaczka
                                where listId.Contains(dp.Id)
                                select dp;

                    foreach (var dokpacz in query)
                    {

                        dokpacz.czyus = 1;
                    }
                    _meritum.SaveChanges();
                    return 1;
                    /*_meritum.SavingChanges += (ob, ea) =>
                    {
                        ArgumentException ex = new ArgumentException();
                        ex.Data.Add("result", 1);
                        throw ex;
                    }; */
                }
            }
            catch (ArgumentException ae)
            {

                return 1;
            }

            catch (Exception e)
            {
                return -1;

            }
            //return 0;

        }



        [Invoke()]
        // [RequiresAuthentication]
        public string RegeneratePozew(string listaIdDoc)// sklej html'i         
        {
            DokWysService dwServ;
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                List<int> listId;
                listId = new List<int>();
                listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaIdDoc, typeof(List<int>));
                // lista spraw 
                foreach (int i in listId)
                {
                    if (i > 0)
                    {
                        dwServ = new DokWysService(i);
                        if (dwServ.RegeneretaPozew() < 0)
                            return "Błąd podczas przetwarzania dokumentu o Id " + i.ToString();
                    }

                }


            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }

            return "OK";
        }



        [Invoke()]
        // [RequiresAuthentication]
        public string SetOdsetkiType(int IdSprawy, int ods_typ)// sklej html'i         
        {

            // ustawia typ odsetek

            try
            {

                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    List<Naleznosc> nalSpr = context.Naleznosc.Include("Odsetki").Where(a => a.Sprawa_id == IdSprawy).ToList();
                    if (nalSpr != null && nalSpr.Any())
                    {
                        foreach (Naleznosc n in nalSpr)
                        {
                            if (n.Odsetki != null)
                                if (n.Odsetki.Count > 0)
                                {
                                    foreach (Odsetki o in n.Odsetki)
                                    {
                                        o.NazwyOdsetek_Id = ods_typ;


                                    }




                                }



                        }

                        context.SaveChanges();

                    }


                }


            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }

            return "OK";
        }

        [Invoke()]
        // [RequiresAuthentication]
        public string ChangeDocDate(string listaIdDoc, DateTime newDate, int typFirma)// sklej html'i         
        {
            DokWysService dwServ;
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                List<int> listId;
                listId = new List<int>();
                listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaIdDoc, typeof(List<int>));
                // lista spraw 
                foreach (int i in listId)
                {
                    if (i > 0)
                    {
                        dwServ = new DokWysService(i);
                        if (dwServ.ChangeDocDate(newDate,typFirma) < 0)
                            return "Błąd podczas przetwarzania dokumentu o Id " + i.ToString();
                    }

                }


            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }

            return "OK";
        }

        [Invoke()]
        // [RequiresAuthentication]
        public string ChangeRadcaDok(string listaIdDoc, int Konto_Id)// sklej html'i         
        {
            DokWysService dwServ;
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                List<int> listId;
                listId = new List<int>();
                listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaIdDoc, typeof(List<int>));
                // lista spraw 
                foreach (int i in listId)
                {
                    if (i > 0)
                    {
                        dwServ = new DokWysService(i);
                        if (dwServ.ChangeRadcaDok(Konto_Id) < 0)
                            return "Błąd podczas przetwarzania dokumentu o Id " + i.ToString();
                    }

                }


            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }

            return "OK";
        }

        [Invoke()]
        // [RequiresAuthentication]
        public int LiczZalegloscBySprList(string ListaIdSpraw, DateTime DataS)// sklej html'i         
        {

            List<int> listId;
            listId = new List<int>();
            listId = (List<int>)ToXMLSerializers.DeserializeFromString(ListaIdSpraw, typeof(List<int>));
            foreach (int i in listId)
            {
                ObliczZaleglosc oblzal = new ObliczZaleglosc();
                oblzal.ObliczSprawe(i, DataS);
                oblzal.SaveResults();

            }
            return 0;
        }


        [Invoke()]
        // [RequiresAuthentication]
        public int LiczZaleglosc(int IdSprawy, DateTime DataS)// sklej html'i         
        {
            ObliczZaleglosc oblzal = new ObliczZaleglosc();
            oblzal.ObliczSprawe(IdSprawy, DataS);
            oblzal.SaveResults();


            return 0;
        }

        [Invoke()]
        // [RequiresAuthentication]
        public string ImportDocument(string document, string filename, int User_Id, int Jednostka_id, int context)// sklej html'i         
        {
            int firma = 1;
            if (User_Id < 0)
            {
                User_Id = -User_Id;
                firma = -1;

            }
            try
            {  // generacja dokumentów na podstawie Id spraw.
                if (context == 0) // import zbiorów z kancelarii
                {
                    ImportValidators impValid = new ImportValidators(filename, document);
                    impValid.ValidateDocument();
                    return impValid.UpdateImportStatusInfo(filename, User_Id, Jednostka_id, 0);

                }
                else    // import csv dla BI
                {
                    byte[] infile = Convert.FromBase64String(document);
                    BIGFile bf;
                    if (firma == -1)
                    {
                        if (context == 6)
                            bf = new BIGFile(filename, System.Text.Encoding.GetEncoding("windows-1250").GetString(infile), User_Id);
                        else
                            bf = new BIGFile(filename, System.Text.Encoding.UTF8.GetString(infile), User_Id);
                    }
                    else
                    {
                        if (context == 4)
                            bf = new BIGFile(filename, System.Text.Encoding.GetEncoding("windows-1250").GetString(infile), User_Id);
                        else
                            bf = new BIGFile(filename, /*System.Text.Encoding.GetEncoding("windows-1250").GetString(infile) */ System.Text.Encoding.UTF8.GetString(infile), User_Id);
                    }

                    if (context == 1)
                    {
                        if (!bf.ProceedFile()) return bf.GetError();
                    }
                    else if (context == 2)
                    {
                        if (bf.ProceedFileUpdate(User_Id, firma, context)) return bf.GetError();

                    }
                    else if (context == 3)
                    {
                        if (!bf.ProceedFileWiena()) return bf.GetError();
                    }
                    else if (context == 4)
                    {

                        if (bf.ProceedFileUpdate(User_Id, firma, context)) return bf.GetError();
                    }
                    else if (context == 5)
                    {

                        if (bf.ProceedFileUpdate(User_Id, firma, context)) return bf.GetError();
                    }
                    else if (context == 6)
                    {

                        if (bf.ProceedFileUpdate(User_Id, firma, context)) return bf.GetError();
                    }
                    return "";
                }


                return "";

            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string ImportDocument2eSad(string document, string filename, int User_Id, int idKontaEpu, int context)// sklej html'i         
        {
            
            string kodWalidacji= string.Empty;
            EpuProxy.ZlozDokumentyOutputDataModel importResult= null; 
            try
            {  // generacja dokumentów na podstawie Id spraw.
                if (context == 0) // import zbiorów z kancelarii
                {

                    using (LexEnaMeritumEntities lx = new LexEnaMeritumEntities())
                    {
                        KontoEPU kontoEpu = lx.KontoEPU.Where(a => a.Id == idKontaEpu).FirstOrDefault();
                        EpuProxy.EpuProxyClient cl = new EpuProxy.EpuProxyClient(true);
                        cl.setUserData(kontoEpu.LoginEPU, Utils.DecodeFrom64(kontoEpu.EPUPasswd), kontoEpu.APIKey);
                        int result =  cl.zlozDokumenty(document, ref kodWalidacji, ref importResult);
                        if (result > 0)
                            return "OK, dokumenty zostały złożone w e-Sądzie\n\rNależy je, w dniu dzisiejszym, podpisać na witrunie www.e-sad.pl";
                        else
                            return kodWalidacji;
                    }
                }


                return "";

            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }


        /*
                [Invoke()]
                // [RequiresAuthentication]
                public string ImportLargeDocument(string guid, string filename, int User_Id, int context)// sklej html'i         
                {
                    try
                    {
                        // generacja dokumentów na podstawie Id spraw.
                        int retCode = 0;

                        BIGFile bf = new BIGFile(filename, new Guid(guid), User_Id);
                        if (context == 4)
                        {
                            if ((retCode = bf.ProceedFileCCnB()) <= 0) return bf.GetError();
                            string appPath;
                            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                            System.Diagnostics.Process proc = new System.Diagnostics.Process();
                            try
                            {
                                appPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Worker/");
                                info.WorkingDirectory = appPath;
                                info.FileName = "BIGImportBGWorker.exe";
                                info.Arguments = " " + retCode.ToString();
                                proc.StartInfo = info;
                                proc.Start();

                            }
                            catch (ArgumentException ex)
                            {
                                return ex.Message + " " + ex.InnerException;
                            }
                            return "";

                        }
                        else if (context == 5)
                        {

                            // zgony 




                        }

                            return "";





                    }
                    catch (Exception e)
                    {

                        return "error: " + e.Message;
                    }


                }
        */

        [Invoke()]
        // [RequiresAuthentication]
        public string ImportLargeDocument(string guid, string filename, int User_Id, int context, int firma)// sklej html'i         
        {
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                int retCode = 0;

                BIGFile bf = new BIGFile(filename, new Guid(guid), User_Id, firma);
                if (context == 4 ||context == 5)
                {
                    if ((retCode = bf.ProceedFileCCnB(context == 5)) <= 0) return bf.GetError();

                    try
                    {
                        using (LexEnaMeritumEntities dbcontext = new LexEnaMeritumEntities())
                        {
                            Uzytkownik u = dbcontext.Uzytkownik.Where(a => a.Id == User_Id).FirstOrDefault();
                            dbcontext.CommandTimeout = 1200;
                            SqlParameter param1 = new SqlParameter("IdJob", retCode);
                            SqlParameter param2 = new SqlParameter("UserName", u.Imie + " " + u.Nazwisko);
                            object obj = dbcontext.ExecuteStoreQuery<object>("USP_BIGImportCCnB  @IdJob, @UserName ", param1, param2).FirstOrDefault();

                            return "";
                        }

                    }
                    catch (ArgumentException ex)
                    {
                        return ex.Message + " " + ex.InnerException;
                    }
                    return "";

                }

                return "";





            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }




        [Invoke()]
        public string ImportZaliczki(string document, string filename, int User_Id, int Jednostka_id, int tryb = 0 )// sklej html'i         
        {
            // tryb = 0 - import zaliczek
            //      = 1 - usuwanie kosztów
            try
            {


                ZalKomHelper ns = new ZalKomHelper(tryb);
                return ns.GetFileContent(document, filename);



            }
            
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }
        [Invoke()]
        public string ZapisZaliczki(string document, string filename, int User_Id, int Jednostka_id, int mode)// sklej html'i         
        {
            try
            {


                ZalKomHelper ns = new ZalKomHelper();
                return ns.InsertZaliczki(document, filename,mode);



            }

            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }

        [Invoke()]
        public string UsunKoszty(string document, string filename, int User_Id, int Jednostka_id)// sklej html'i         
        {
            try
            {


                ZalKomHelper ns = new ZalKomHelper(1);
                return ns.UsunKoszty(document, filename);



            }

            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }

        [Invoke()]
        public string ImportDocumentXLSX(string document, string filename, int User_Id, int Jednostka_id)// sklej html'i         
        {
            try
            {
                if (User_Id > 0)
                {
                    // generacja dokumentów na podstawie Id spraw.
                    ImportValidators impValid = new ImportValidators(filename, document, false);
                    impValid.ValidateDocument();
                    return impValid.UpdateImportStatusInfo(filename, User_Id, Jednostka_id, 0);

                }
                else
                {
                    // 
                    UZDOperations uopr = new UZDOperations();
                    switch (User_Id)
                    {
                        case -1:
                            return uopr.ConfirmPackage(Jednostka_id, document, filename);
                        case -2:
                            return uopr.ConfirmSAPNips(Jednostka_id, document, filename);
                        case -3:
                            return uopr.ConfirmPackageFinal(Jednostka_id, document, filename);
                        case -5000:
                            // Przetworzenie dokuemtu -spady EPU
                            Toools1Opertations to = new Toools1Opertations();
                            return to.GetFileEPU(document, filename);
                        case -6000:
                            EkstraktPdf epdf = new EkstraktPdf();
                            return epdf.GetFileContent(document, filename);

                        case -7100:
                            NalSprawHelper ns = new NalSprawHelper();
                            return ns.GetFileContent(document, filename);
                        case -7000:
                            SkutKancHelper sk = new SkutKancHelper();
                            return sk.GetFileContent(document, filename);

                        case -8000:
                            PotwierdzSadaHelper pSald = new PotwierdzSadaHelper();
                            // W parametrze filename zapisana jest data obliczenia stanu
                            return pSald.GetFileContent(document, filename, Jednostka_id);
                        case -8001:
                            PotwierdzSadaHelper pSaldf2 = new PotwierdzSadaHelper();
                            // W parametrze filename zapisana jest data obliczenia stanu
                            return pSaldf2.GetFileContent(document, filename, Jednostka_id,true);


                        default:
                            return "";

                    }


                }




            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }
        [Invoke()]
        public string SynchroPozew(string JobIds, int User_Id, int Jednostka_id)// sklej html'i         
        {
            List<TypSlownikFiltered> jobIds;
            List<caseDescriptor> lst2submit = new List<caseDescriptor>();
            if (String.IsNullOrWhiteSpace(JobIds)) return "";

            jobIds = (List<TypSlownikFiltered>)Utils.DeserializeFromString(JobIds, typeof(List<TypSlownikFiltered>));
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    using (wiena_centralEntities wiena = new wiena_centralEntities())
                    {
                        foreach (var keyVal in jobIds)
                        {
                            if (keyVal.Filter1 == 3 && keyVal.Numer > 0)   // mojesprawy
                            {
                                List<SprawaOutputElementModels> lst = context.SprawaOutputElementModels.Where(a => a.MojeSprawyOutputDataModels.Id == keyVal.Numer).ToList();
                                if (lst != null && lst.Count > 0)
                                {
                                    foreach (SprawaOutputElementModels item in lst)
                                    {
                                        int idSpr;
                                        caseDescriptor cd = new caseDescriptor();
                                        cd.Sygnatura = item.SygnaturaWgPowoda;
                                        cd.Nce = item.SygnaturaSprawy;
                                        cd.IdSprawy = item.IdSprawy;

                                        if (cd.Sygnatura.Contains("@"))
                                        {
                                            if (cd.Sygnatura.Contains("#"))
                                                idSpr = Convert.ToInt32(cd.Sygnatura.Substring(cd.Sygnatura.IndexOf("@") + 1, cd.Sygnatura.IndexOf("#") - cd.Sygnatura.IndexOf("@")));
                                            else
                                                idSpr = Convert.ToInt32(cd.Sygnatura.Substring(cd.Sygnatura.IndexOf("@") + 1));

                                            DokWys dw = context.DokWys.Where(c => c.Sprawa_id == idSpr && c.TypDok == 10).FirstOrDefault();
                                            if (dw != null)
                                            {
                                               
                                                
                                                int? wiena_id = context.Sprawa.Where(a => a.id == idSpr).Select(b => b.IdWiena).FirstOrDefault();
                                                cd.nalgl = (dw.WPS != null ? dw.WPS.Value : 0);
                                                decimal notyods = 0;
                                                try
                                                {
                                                    PozewEPU pozew = (PozewEPU)ToXMLSerializers.DeserializeFromString(dw.Tresc, typeof(PozewEPU));
                                                    if (Jednostka_id < 0)
                                                    {
                                                        decimal last_val = 0;
                                                        decimal ods_kapit = 0;
                                                        bool found = false;
                                                        List<WienaDB.Models.naleznosc> nallst = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id).ToList();
                                                        foreach (var r in pozew.ListaRoszczen)
                                                        {
                                                            last_val = r.Wartosc;
                                                            if (!string.IsNullOrWhiteSpace(r.Opis) && r.Opis.ToLower().Contains("kapitaliz") && r.Opis.ToLower().Contains("odsetk"))
                                                            {
                                                                ods_kapit = r.Wartosc;

                                                                found = false;
                                                                foreach (WienaDB.Models.naleznosc n in nallst)
                                                                {
                                                                    if (n.kwota == r.Wartosc && n.tytul == "kapitalizacja odsetek w pozwie")
                                                                    {
                                                                        found = true;
                                                                        break;

                                                                    }

                                                                }
                                                                if (found)
                                                                    break;


                                                            }
                                                            else
                                                            {
                                                                foreach (WienaDB.Models.naleznosc n in nallst)
                                                                {
                                                                    if (n.kwota == r.Wartosc && n.id_typ_nal == 22)
                                                                    {
                                                                        notyods += r.Wartosc;
                                                                        break;

                                                                    }

                                                                }




                                                            }


                                                        }
                                                        if (!found && last_val == ods_kapit)
                                                        {
                                                            dw.OdsetkiKapital = ods_kapit;


                                                        }
                                                        if (notyods > 0)
                                                            dw.NotyOdsetkowe = notyods;
                                                        context.SaveChanges();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    ;

                                                }
                                                cd.odsKapital = (dw.NotyOdsetkowe != null ? dw.NotyOdsetkowe.Value : 0);
                                                cd.odsNaliczoneSkapitalizowane = (dw.OdsetkiKapital != null ? dw.OdsetkiKapital.Value : 0);
                                                cd.kosztyInne = dw.InneKoszty ?? 0;
                                                
                                                if (Jednostka_id < 0)
                                                    cd.wdz = 0;
                                                else
                                                { //obrót
                                                  //  Utils.LogWriter("odczyt wdz dla " + wiena_id.ToString());
                                                    var obr = (from o in wiena.obroty
                                                               join ka in wiena.konto_anal on o.id_konta equals ka.ident
                                                               join k in wiena.konto on ka.id_konta equals k.id
                                                               where k.typ == -1 && o.czyus == 0 && ka.czyus == 0 && ka.id_sprawy == wiena_id
                                                               orderby ka.ident descending
                                                               group o by o.id_konta into s
                                                               select new
                                                               {
                                                                   wdz = s.Sum(a => a.wdz),
                                                                   
                                                               }).FirstOrDefault();

                                                    if (obr == null || obr.wdz == null)
                                                    {

                                                        cd.wdz = 0;

                                                    }
                                                    else
                                                        cd.wdz = obr.wdz.Value;

                                                    //Utils.LogWriter("odczyt wdz dla " + wiena_id.ToString() + " wdz = " + (cd.wdz).ToString());

                                                }
                                                cd.dataDok = dw.DataDok.Value;
                                                cd.dataRej = DateTime.Today;

                                            }
                                            PdfStore pdf = context.PdfStore.Where(c => c.DokWys_Id == dw.Id).FirstOrDefault();
                                            if (pdf != null && pdf.value != null)
                                                cd.trescDok = pdf.value;
                                            else // generacja 
                                            {
                                                if (!String.IsNullOrWhiteSpace(dw.Tresc))
                                                {
                                                    string s = XML2HTMLTransform.TransformNCompress(dw.Tresc, 0);
                                                    string retvalue = XML2HTMLTransform.html2pdf(s, ref cd.trescDok);
                                                    if (retvalue.Contains("Błąd"))
                                                    {
                                                        Utils.LogWriter("Wyjątek w trakcie zapisu do pdf " + " " + cd.Sygnatura);
                                                        return "Błąd w trakcie zapisu do pdf " + " " + cd.Sygnatura;
                                                    }

                                                }
                                                if (pdf == null)
                                                {
                                                    pdf = new PdfStore();
                                                    pdf.DokWys_Id = dw.Id;
                                                    pdf.name = "Pozew " + cd.Sygnatura;
                                                    pdf.type = 0;
                                                    context.PdfStore.AddObject(pdf);
                                                }
                                                pdf.value = cd.trescDok;
                                                context.SaveChanges();
                                            }

                                        }

                                        cd.caseType = keyVal.Filter1;
                                        lst2submit.Add(cd);

                                        //idSpr = 
                                    }



                                }



                            }
                            
                            if (keyVal.Filter1 == 5 && keyVal.Numer > 0)   // mojeNakazy
                            {

                                List<NakazOutputElementModels> lst = context.NakazOutputElementModels.Where(a => a.MojeNakazyOutputDataModels_Id == keyVal.Numer).ToList();
                                if (lst != null && lst.Count > 0)
                                {
                                    foreach (NakazOutputElementModels item in lst)
                                    {
                                        int idSpr;
                                        caseDescriptor cd = new caseDescriptor();
                                        cd.Sygnatura = item.SygnaturaWgPowoda;
                                        cd.Nce = item.SygnaturaSprawy;
                                        cd.IdSprawy = item.IdNakazu;
                                        
                                        if (cd.Sygnatura.Contains("@"))
                                        {
                                            if (cd.Sygnatura.Contains("#"))
                                                idSpr = Convert.ToInt32(cd.Sygnatura.Substring(cd.Sygnatura.IndexOf("@") + 1, cd.Sygnatura.IndexOf("#") - cd.Sygnatura.IndexOf("@")));
                                            else
                                                idSpr = Convert.ToInt32(cd.Sygnatura.Substring(cd.Sygnatura.IndexOf("@") + 1));
                                            DokOdebr dw = context.DokOdebr.Where(c => c.Sprawa_id == idSpr && c.TypDok == 5).FirstOrDefault();
                                            Sprawa sp = context.Sprawa.Where(c => c.id == idSpr).FirstOrDefault();
                                            

                                            if (dw != null)
                                            {

                                                cd.dataDok = dw.DataDokumentu.Value;
                                                cd.dataRej = DateTime.Today;

                                            }

                                            if (sp != null)
                                            {
                                                cd.kosztySadowe = sp.KosztyZadane ?? 0;
                                                cd.kosztyInne = sp.InneZadane ?? 0;
                                                cd.kzp = sp.KzpZadane ?? 0;



                                            }
                                            
                                            PdfStore pdf = context.PdfStore.Where(c => c.DokOdebr_Id == dw.Id).FirstOrDefault();
                                            

                                            if (pdf != null && pdf.value != null)
                                            {
                                                cd.trescDok = pdf.value;
                                                
                                            }
                                            else // generacja 
                                            {
                                                
                                                if (!String.IsNullOrWhiteSpace(dw.Tresc))
                                                {
                                                    
                                                    string s = XML2HTMLTransform.TransformNCompress(dw.Tresc, 5);
                                                    
                                                    string retvalue = XML2HTMLTransform.html2pdf(s, ref cd.trescDok);

                                                    if (retvalue.Contains("Błąd"))
                                                    {
                                                        Utils.LogWriter("Wyjątek w trakcie zapisu do pdf " + " " + cd.Sygnatura);
                                                        return "Błąd w trakcie zapisu do pdf " + " " + cd.Sygnatura;
                                                    }

                                                }
                                                if (pdf == null)
                                                {
                                                    pdf = new PdfStore();
                                                    pdf.DokOdebr_Id = dw.Id;
                                                    pdf.name = "Nakaz " + cd.Sygnatura;
                                                    pdf.type = 0;
                                                    context.PdfStore.AddObject(pdf);
                                                }
                                                pdf.value = cd.trescDok;
                                                context.SaveChanges();
                                            }
                                            cd.caseType = keyVal.Filter1;
                                            lst2submit.Add(cd);
                                        }



                                        //idSpr = 
                                    }



                                }



                            }

                            if (keyVal.Filter1 == 7 && keyVal.Numer > 0)   // moje Postanowienia
                            {

                                List<OrzeczenieVer2OutputElementModel> lst = context.OrzeczenieVer2OutputElementModel.Where(a => a.MojeOrzeczeniaVer2OutputDataModels_Id == keyVal.Numer).ToList();
                                if (lst != null && lst.Count > 0)
                                {
                                    foreach (OrzeczenieVer2OutputElementModel item in lst)
                                    {
                                        int idSpr;
                                        caseDescriptor cd = new caseDescriptor();
                                        cd.Sygnatura = item.SygnaturaWgPowoda;
                                        cd.Nce = item.SygnaturaSprawy;
                                        cd.IdSprawy = item.IdOrzeczeia;
                                        int typdok = 17;
                                        string docname="Klauzula";
                                        if (item.Id_klauzula > 0)
                                        {
                                            cd.dt = ImportValidators.docTypes.Klauzula;
                                            cd.docName = "Tytuł wykonawczy";
                                           
                                        }
                                        else // inne postanowienie
                                        {  // if (item.DokumentXML.ToLower().Contains("curr:tempprzekazanieid") && item.DokumentXML.ToLower().Contains("z a r z ą d z")) continue;
                                            OrzeczenieEPU orzeczenie;
                                            try
                                            {
                                                orzeczenie = (OrzeczenieEPU)ToXMLSerializers.DeserializeFromString(item.DokumentXML, typeof(OrzeczenieEPU));
                                            }
                                            catch (Exception ex)
                                            {
                                                continue;
                                            }
                                                if (orzeczenie.kodDecyzji == 23 || orzeczenie.kodDecyzji == 46 || orzeczenie.kodDecyzji == 47 || orzeczenie.kodDecyzji == 48 || orzeczenie.kodDecyzji == 49)   // umorzenie
                                            {
                                                cd.dt = ImportValidators.docTypes.Umorzenie;
                                                docname = "Postanowienie o umorzeniu";
                                                typdok = 23;
                                                cd.docName = docname;
                                            }
                                            else if (orzeczenie.kodDecyzji == 3)   // postanowienie o odtrzuceniu pozwu
                                            {
                                                typdok = 3;
                                                docname = "Postanowienie o odrzuceniu pozwu";
                                                cd.dt = ImportValidators.docTypes.ZwrotPozwu;
                                                cd.docName = docname;
                                            }
                                            else
                                                continue;




                                        }


                                        if (cd.Sygnatura.Contains("@"))
                                        {
                                            if (cd.Sygnatura.Contains("#"))
                                                idSpr = Convert.ToInt32(cd.Sygnatura.Substring(cd.Sygnatura.IndexOf("@") + 1, cd.Sygnatura.IndexOf("#") - cd.Sygnatura.IndexOf("@")));
                                            else
                                                idSpr = Convert.ToInt32(cd.Sygnatura.Substring(cd.Sygnatura.IndexOf("@") + 1));
                                            DokOdebr dw = context.DokOdebr.Where(c => c.Sprawa_id == idSpr && c.TypDok == typdok).FirstOrDefault();
                                            Sprawa sp = context.Sprawa.Where(c => c.id == idSpr).FirstOrDefault();

                                            if (dw == null)
                                            {
                                                dw = context.DokOdebr.Where(c => c.Sprawa_id == idSpr && c.IdEPU == cd.IdSprawy).FirstOrDefault();

                                            }

                                            if (dw == null)
                                                continue;

                                            if (dw != null)
                                            {

                                                cd.dataDok = dw.DataDokumentu.Value;
                                                cd.dataRej = DateTime.Today;

                                            }
                                            if (sp != null)
                                            {
                                                cd.kosztySadowe = sp.KosztyZadane ?? 0;
                                                cd.kosztyInne = sp.InneZadane ?? 0;
                                                cd.kzp = sp.KzpZadane ?? 0;



                                            }

                                            PdfStore pdf = context.PdfStore.Where(c => c.DokOdebr_Id == dw.Id).FirstOrDefault();
                                            if (pdf != null && pdf.value != null)
                                                cd.trescDok = pdf.value;
                                            else // generacja 
                                            {
                                                if (!String.IsNullOrWhiteSpace(dw.Tresc))
                                                {
                                                    string s = XML2HTMLTransform.TransformNCompress(dw.Tresc, 17);
                                                    string retvalue = XML2HTMLTransform.html2pdf(s, ref cd.trescDok);
                                                    if (retvalue.Contains("Błąd"))
                                                    {
                                                        Utils.LogWriter("Wyjątek w trakcie zapisu do pdf " + " " + cd.Sygnatura);
                                                        return "Błąd w trakcie zapisu do pdf " + " " + cd.Sygnatura;
                                                    }

                                                }
                                                if (pdf == null)
                                                {
                                                    pdf = new PdfStore();
                                                    pdf.DokOdebr_Id = dw.Id;
                                                    pdf.name = docname + " " + cd.Sygnatura;
                                                    pdf.type = 0;
                                                    context.PdfStore.AddObject(pdf);
                                                }
                                                pdf.value = cd.trescDok;
                                                context.SaveChanges();
                                            }
                                            cd.caseType = keyVal.Filter1;
                                            lst2submit.Add(cd);
                                        }




                                        //idSpr = 
                                    }



                                }



                            }

                        }
                    }
                }

                if (lst2submit.Count > 0)
                {
                    ImportValidators impValid = new ImportValidators();

                    impValid.FirmaTyp =  Jednostka_id;
                    string blady = string.Empty;
                    foreach (caseDescriptor cd in lst2submit)
                    {
                        switch (cd.caseType)
                        {

                            case 3:
                                if (cd.Sygnatura == "ZPZWO-3/2021@118403")
                                {

                                    ;
                                }
                                if (!impValid.addDocScan(ImportValidators.docTypes.Pozew, cd.trescDok, cd.Sygnatura + " Pozew", cd.dataDok.Date, cd.dataRej, cd.nalgl, 0, 0, cd.kosztyInne, 999, cd.Nce, cd.nalgl, cd.odsKapital, cd.wdz, 0, "Sąd Rejonowy Lublin Zachód w Lublinie", "VI Wydzial Cywilny", "Lublin", 0, string.Empty, string.Empty, 0, Jednostka_id,false,cd.odsNaliczoneSkapitalizowane))
                                {
                                    Utils.LogWriter(impValid.errDescription);
                                    blady += impValid.errDescription + "\n\r" + impValid.GetErrorsCollectionAsString();

                                }
                                break;
                            case 5:

                         if (!impValid.addDocScan(ImportValidators.docTypes.Nakaz, cd.trescDok, cd.Sygnatura + " Nakaz zapłaty", cd.dataDok.Date, cd.dataRej, cd.nalgl,cd.kosztySadowe ,cd.kzp, cd.kosztyInne , 999, cd.Nce, cd.nalgl, cd.odsKapital, cd.wdz, 0, "Sąd Rejonowy Lublin Zachód w Lublinie", "VI Wydzial Cywilny", "Lublin", 0, string.Empty, string.Empty, 0, Jednostka_id))
                                    blady += "Error Błąd podczas odnotowywania nakazu zapłaty " + cd.Sygnatura + "\n\r" + impValid.GetErrorsCollectionAsString();
                                break;

                          case 7:
                                if (!impValid.addDocScan(cd.dt, cd.trescDok, cd.Sygnatura + " " + cd.docName , cd.dataDok.Date, cd.dataRej, cd.nalgl, cd.kosztySadowe, cd.kzp, 0, 999, cd.Nce, cd.nalgl, cd.odsKapital, cd.wdz, 0, "Sąd Rejonowy Lublin Zachód w Lublinie", "VI Wydzial Cywilny", "Lublin", 0, string.Empty, string.Empty, 0, Jednostka_id))
                                    blady += "Error Błąd podczas odnotowywania klauzuli wykonalności " + cd.Sygnatura + "\n\r" + impValid.GetErrorsCollectionAsString();
                                break;


                        }
                    }
                    if (!String.IsNullOrWhiteSpace(blady))
                    {

                        return "Error " + blady;
                    }

                }

                return "";

            }
            catch (Exception e)
            {
                Utils.LogWriter("%%%%%% Synchro error" + e.Message);
                return "error: " + e.Message;
            }


        }




        [Invoke()]
        public string ImportDocumentsZipXLSX(string guidZip, string filenamezip, string documentxlsx, string filenamexlsx, int User_Id, int Jednostka_id)// sklej html'i         
        {
            try
            {

                ImportValidators impValid = new ImportValidators(filenamezip, guidZip, filenamexlsx, documentxlsx);
                if (User_Id < 0)
                {
                    impValid.FirmaTyp = -1;
                    User_Id = -User_Id;
                }
                impValid.ValidateDocument();
                return impValid.UpdateImportStatusInfo(filenamezip + "/" + filenamexlsx, User_Id, Jednostka_id, -1);

                return "";

            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }


        [Invoke()]
        public string ImportDocumentsZipPart(string documentzip, bool isfirst)// sklej html'i         
        {
            try
            {

                if (isfirst)
                    zipContent = "";
                zipContent += documentzip;
                return "";
            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }

        [Invoke()]
        public string ImportDocumentUmorzZgonXLSX(string document, string UserName, DateTime DataZestawienia, int IdHeaderRecalc)// sklej html'i         
        {
            int IdHeader = 0;
            try
            {
                bool result1;

                if (IdHeaderRecalc == 0)
                {
                    ProceedZgony pz = new ProceedZgony(document, UserName, DataZestawienia);
                    if (pz.ImportFile())
                        result1 = pz.GetWienaId();
                    IdHeader = pz.CreateDbStructure();
                    if (IdHeader > 0)
                    {
                        using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                        {
                            lexena.CommandTimeout = 1200;  // 20 minut
                            SqlParameter param1 = new SqlParameter("DataStanu", DataZestawienia);
                            SqlParameter param2 = new SqlParameter("IdHeaderZgony", IdHeader);
                            try
                            {
                                object obj = lexena.ExecuteStoreQuery<object>("usp_CalcZgony  @DataStanu, @IdHeaderZgony", param1, param2).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                Utils.LogWriter("Błąd wywolania procedury składowanej: usp_CalcZgony");
                                return "error : Błąd wywolania procedury składowanej: usp_CalcZgony";

                            }



                        }

                    }
                }
                else
                    IdHeader = IdHeaderRecalc;
                if (IdHeader > 0)
                {
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    try
                    {
                        string appPath = System.Web.Hosting.HostingEnvironment.MapPath("~/CalcZaleglosc/");
                        info.WorkingDirectory = appPath;
                        info.FileName = "CalcZaleglosc.exe";
                        info.Arguments = " " + IdHeader.ToString() + " " + DataZestawienia.ToString("yyyy-MM-dd");
                        proc.StartInfo = info;
                        proc.Start();

                    }
                    catch (Exception ex)
                    {
                        Utils.LogWriter("Błąd wywolania kalkulatora odsetek: " + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : ""));
                        return ex.Message + " " + ex.InnerException;
                    }
                }
                return IdHeader.ToString();
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd:" + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : ""));
                return "error: " + ex.Message;
            }


        }


        [Invoke()]
        // [RequiresAuthentication]
        public string ZgonyDelete(int IdPakiet)// sklej html'i 
        {
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    SqlParameter param1 = new SqlParameter("IdPakiet", IdPakiet);


                    object obj = context.ExecuteStoreQuery<object>("USP_ZgonyDelete  @IdPakiet", param1).FirstOrDefault();
                    return "";
                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
            }
        }

        [Invoke()]
        // [RequiresAuthentication]
        public string ZgonyZlecenia(int IdPakiet, DateTime dZlecen)// sklej html'i 
        {
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    SqlParameter param1 = new SqlParameter("@IdPakiet", IdPakiet);
                    SqlParameter param2 = new SqlParameter("@DZlecenia", dZlecen);
                    context.CommandTimeout = 320;
                    object obj = context.ExecuteStoreQuery<object>("USP_ZgonyZlecenia  @IdPakiet, @DZlecenia", param1, param2).FirstOrDefault();
                    return "";
                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dok_id"></param>
        /// <param name="mode">0 - dok_wys; 1 dok_odebr , 100 - pdf store id</param>
        /// <returns></returns>
        [Invoke()]
        public string GetDocumentPdf(int dok_id, int mode)// sklej html'i         
        {


            string dokpdf = "";
            PdfStore pdf;


            using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
            {

                switch (mode)
                {
                    case 0:
                        pdf = lexena.PdfStore.Where(a => a.DokWys_Id == dok_id).FirstOrDefault();
                        break;
                    case 1:
                        pdf = lexena.PdfStore.Where(a => a.DokOdebr_Id == dok_id).FirstOrDefault();
                        break;
                    case 100:
                        pdf = lexena.PdfStore.Where(a => a.Id == dok_id).FirstOrDefault();
                        break;
                    default:
                        return "";
                }

                if (pdf == null) return "";
                else
                    return Convert.ToBase64String(pdf.value);

            }
        }


        [Invoke()]
        public string UploadDocumentPdf(string document, string docname, int dokWys_id, int dokOdebr_id, int User_Id)// sklej html'i         
        {

            PdfStore pdf = null;


            DokWys dw = null;
            DokOdebr doo = null;
            try
            {
                if (String.IsNullOrWhiteSpace(document))
                {
                    return "Zbiór jest pusty";
                }


                byte[] data = Convert.FromBase64String(document);
                if (data == null)

                {
                    return "Zbiór jest pusty";
                }

                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {

                    if (dokWys_id > 0)
                    {
                        pdf = (from z in lexena.PdfStore
                               where z.DokWys_Id == dokWys_id
                               select z).FirstOrDefault();


                        if (pdf == null)
                        {
                            pdf = new PdfStore();
                            if (dokWys_id > 0)
                                dw = lexena.DokWys.Where(c => c.Id == dokWys_id).FirstOrDefault();
                            if (dw != null && dw.PdfStore == null)
                                dw.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                            if (dw != null)
                                dw.PdfStore.Add(pdf);
                            lexena.PdfStore.AddObject(pdf);
                        }
                    }
                    else
                    if (dokOdebr_id > 0)
                    {
                        pdf = (from z in lexena.PdfStore
                               where z.DokOdebr_Id == dokOdebr_id
                               select z).FirstOrDefault();


                        if (pdf == null)
                        {
                            pdf = new PdfStore();
                            if (dokOdebr_id > 0)
                                doo = lexena.DokOdebr.Where(c => c.Id == dokOdebr_id).FirstOrDefault();
                            if (doo != null && doo.PdfStore == null)
                                doo.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                            if (doo != null)
                                doo.PdfStore.Add(pdf);
                            lexena.PdfStore.AddObject(pdf);
                        }


                    }
                    else // dwa zera
                    {
                        pdf = new PdfStore();
                        lexena.PdfStore.AddObject(pdf);
                    }


                    pdf.name = docname;
                    pdf.type = 0;
                    pdf.value = data;

                    lexena.SaveChanges();

                }
                return pdf.Id.ToString();

            }



            catch (Exception ex)
            {

                string blad = "Błąd:" + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : "");

                return blad.ToString();

            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string ExportDocument(string info, string username, int Jednostka_Id)// informacja o imporcie pozwów z systemu         
        {

            impDescr impresult;
            try
            {
                // generacja dokumentów na podstawie Id spraw.

                impresult = (impDescr)ToXMLSerializers.DeserializeFromString(info, typeof(impDescr));
                ImportValidators iv = new ImportValidators();
                iv.updateExport(impresult, username, Jednostka_Id);
                return iv.GetOperationStatusInfo();





            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public IEnumerable<string> GetWienaDocNames()// informacja o imporcie pozwów z systemu         
        {
            StrList lst = new StrList();

            try
            {
                // generacja dokumentów na podstawie Id spraw.
                using (wiena_centralEntities wienaContext = new wiena_centralEntities())
                {

                    IEnumerable<string> lst1 = wienaContext.dok_odebr_nazwy.OrderBy(a => a.nazwa).Select(a => a.nazwa).Distinct().ToList();
                    return lst1;

                }



            }
            catch (Exception e)
            {

                return null;
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_ExportPakiet(int IdPakiet, int User_Id)// sklej html'i         
        {
            string outstr = String.Empty;
            int i = 0;
            try
            {

                BIGDataOperation bop = new BIGDataOperation();
                Nicci.Input.informationManagementOrderType[] ordTyp;
                Input inp = bop.Import2KrdInputNew(IdPakiet);

                //Input inpout = (Input)inp.Clone();
                // ordTyp = ((Nicci.Input.informationManagementOrdersType1)(inp.Item)).Order;
                Utils.LogWriter("Zbudowano pakiet do wysyłki ");

                // for (i = 1; i <= ordTyp.Length; i++)
                {
                    //       Nicci.Input.informationManagementOrderType[] oT = new informationManagementOrderType[1];
                    //      Array.Copy(ordTyp,i, oT,0,1);
                    //      ((Nicci.Input.informationManagementOrdersType1)(inpout.Item)).Order = oT;

                    if (inp != null)
                    {

                        var output = new MemoryStream();
                        var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };


                        using (var xmlWriter = XmlWriter.Create(output, settings))
                        {
                            var serializer = new XmlSerializer(typeof(Input));
                            xmlWriter.WriteStartDocument();

                            serializer.Serialize(xmlWriter, inp);
                        }


                        output.Seek(0L, SeekOrigin.Begin);
                        //
                        var reader = new StreamReader(output);
                        outstr = reader.ReadToEnd();


                    }
                    else
                    {
                        return "Podczas wysyłki wystąpił błąd sprawdź statusy poszczególnych wierszy ";

                    }

                }
                Utils.LogWriter("Pakiet został zserializowany");
                Utils.LogWriter(outstr);
                if (!String.IsNullOrWhiteSpace(outstr))
                {
                    SiddinService theservice = new SiddinService();
                    Guid? JobID = theservice.sendData(outstr);
                    if (JobID == null)
                        return "error " + theservice.getErrorMsg();
                    else
                    {
                        bop.addJobID(JobID, IdPakiet);
                        Utils.LogWriter("Pakiet został wysłany");
                    }
                    return "";
                }


                return "";

            }
            catch (SerializationException se)
            {
                Utils.LogWriter("error: " + se.Message + " " + (se.InnerException != null ? se.InnerException.Message : ""));
                return "error: " + se.Message + " " + (se.InnerException != null ? se.InnerException.Message : "");
            }
            catch (Exception e)
            {
                Utils.LogWriter("error: " + e.Message + " " + (e.InnerException != null ? e.InnerException.Message : ""));
                return "error: " + e.Message + " " + (e.InnerException != null ? e.InnerException.Message : "");
            }


        }




        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_CheckStatus(int IdPakiet)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {
                BIGDataOperation bop = new BIGDataOperation();
                List<Guid?> lst = bop.getUnconfirmedJobs(IdPakiet);
                if (lst == null) return "";
                SiddinService theservice = new SiddinService();
                foreach (Guid? g in lst)
                { string answer;
                    int status = -1;
                    status = theservice.getConfirmation(g.Value, out answer);
                    if (status == 2) // jesłi przeprocedowano
                    {
                        bop.updateCaseStatus(answer, g.Value);

                    }
                    else if (status >= 0)
                    {
                        bop.updateImportStatus(status, g.Value);

                    }
                    else
                        return "error: " + theservice.getErrorMsg();

                }
                return "";
            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }


        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_ImportWiena(int User_Id, int id_firma,int context )// sklej html'i         
        {
            // odczyt danych z Wieny
            try
            {
                Utils.LogWriter("Import z systemu Wiena");
                BIGFile bf = new BIGFile("Import Wiena", "", User_Id,id_firma,context);
                bf.importWiena(DateTime.Today, 14);
                return "";




            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }



        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_DelPakietSP(int IdPakiet)// sklej html'i 
        {
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    SqlParameter param1 = new SqlParameter("IdPakiet", IdPakiet);
                    context.CommandTimeout = 240;

                    object obj = context.ExecuteStoreQuery<object>("usp_delKRDPakiet  @IdPakiet", param1).FirstOrDefault();
                    return "";
                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
            }
        }

        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_DelAllDlu(string Username)// sklej html'i 
        {
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    SqlParameter param1 = new SqlParameter("username", Username);

                    context.CommandTimeout = 240;
                    object obj = context.ExecuteStoreQuery<object>("USP_DelAllKRD  @username", param1).FirstOrDefault();
                    return "";
                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
            }
        }




        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_DelPakiet(int IdPakiet)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {

                    BIG_Import bi = context.BIG_Import.Where(i => i.BIG_ImportId == IdPakiet).FirstOrDefault();
                    if (bi == null)
                    {
                        return " Brak pakietu o takim Id ";
                    }
                    List<BIG_Operacja> boLst = context.BIG_Operacja.Where(a => a.BIG_ImportId == IdPakiet).ToList();
                    List<BIG_Obligation> nalLst = boLst.Select(b => b.BIG_Obligation).Distinct().ToList();
                    List<BIG_Case> caseLst = boLst.Select(b => b.BIG_Case).Distinct().ToList();
                    List<BIG_Debtor> dtLst = boLst.Select(b => b.BIG_Debtor).Distinct().ToList();

                    List<BIG_Obligation> nalPozLst = new List<BIG_Obligation>();
                    List<BIG_Case> casePozLst = new List<BIG_Case>();



                    foreach (BIG_Operacja b in boLst)
                    {
                        if (b != null)
                            context.BIG_Operacja.DeleteObject(b);

                    }

                    foreach (BIG_Obligation n in nalLst)
                    {
                        if (n != null && !n.BIG_Operacja.Any())
                            context.BIG_Obligation.DeleteObject(n);
                        else
                            nalPozLst.Add(n);

                    }
                    if (dtLst != null)
                        foreach (BIG_Debtor d in dtLst)

                        {
                            if (d != null && !d.BIG_Operacja.Any())
                                context.BIG_Debtor.DeleteObject(d);
                        }


                    foreach (BIG_Case c in caseLst)
                    {
                        if (c != null && !c.BIG_Operacja.Any() && !c.BIG_Obligation.Any() && !c.BIG_Debtor.Any())
                        {

                            context.BIG_Case.DeleteObject(c);
                        }
                        else
                            casePozLst.Add(c);

                    }
                    context.BIG_Import.DeleteObject(bi);
                    context.SaveChanges();
                    // odtworzenie dat
                    if (nalPozLst.Any())
                    {
                        foreach (BIG_Obligation bo in nalPozLst)
                        {
                            BIG_Operacja bop = context.BIG_Operacja.Where(a => a.BIG_ObligationId == bo.BIG_ObligationId && (a.TypOperacji == 5 || a.TypOperacji == 6)).OrderByDescending(a => a.BIG_OperacjaId).FirstOrDefault(); // ostatnia operacja
                            if (bop != null)
                            {
                                if (bop.TypOperacji == 5)
                                    bo.SuspendDate = bop.SuspendDate;
                            }
                            else
                            {
                                bo.SuspendDate = null;
                                bo.AutoSuspend = false;
                            }

                        }


                    }

                    if (casePozLst.Any())
                    {
                        foreach (BIG_Case bc in casePozLst)
                        {
                            BIG_Operacja bop = context.BIG_Operacja.Where(a => a.BIG_CaseId == bc.BIG_CaseId && (a.TypOperacji == 5 || a.TypOperacji == 6)).OrderByDescending(a => a.BIG_OperacjaId).FirstOrDefault(); // ostatnia operacja
                            if (bop != null)
                            {
                                if (bop.TypOperacji == 5)
                                    bc.SuspendDate = bop.SuspendDate;
                            }
                            else
                            {
                                bc.SuspendDate = null;

                            }
                            bop = null;
                            bop = context.BIG_Operacja.Where(a => a.BIG_CaseId == bc.BIG_CaseId && a.TypOperacji == 17).OrderByDescending(a => a.BIG_OperacjaId).FirstOrDefault(); // ostatnia operacja
                            if (bop != null)
                            {

                                bc.NotifyDate = bop.NotifyDate;
                            }
                            else
                            {
                                bc.NotifyDate = null;

                            }
                        }


                    }
                    context.SaveChanges();

                }
                return "";

            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_DelCase(int IdCase, int idUser)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {

                    BIG_Case bc = context.BIG_Case.Where(i => i.BIG_CaseId == IdCase).FirstOrDefault();
                    if (bc == null)
                    {
                        return " Brak sprawy o takim Id ";
                    }

                    BIGFile bf = new BIGFile("", "", idUser);
                    if (bf.DeleteCase(bc.BIG_CaseId) == null)
                        return " błąd podczas usuwania sprawy";
                }
                return "";

            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_DelAll(int idUser)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {



                    BIGFile bf = new BIGFile("", "", idUser);
                    if (bf.DeleteAllCases() == null)
                        return " błąd podczas usuwania spraw";
                }
                return "";

            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }


        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_DelObligation(int IdOblig, int idUser)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {

                    BIG_Obligation bo = context.BIG_Obligation.Where(i => i.BIG_CaseId == IdOblig).FirstOrDefault();
                    if (bo == null)
                    {
                        return " Brak zobowiązania o takim Id ";
                    }

                    BIGFile bf = new BIGFile("", "", idUser);
                    if (bf.DeleteObligation(bo.BIG_ObligationId) == null)
                        return " Błąd podczas usuwania zobowiązania ";
                }
                return "";

            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_UpdateDetail(string roWserialized, Decimal newSaldo, DateTime newDataWymag, DateTime newDataWezw, String nrfakt, int idUser)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {



                BIGvw_ObligationLastStatus rDetails = null;
                rDetails = (BIGvw_ObligationLastStatus)Utils.DeserializeFromString(roWserialized, typeof(BIGvw_ObligationLastStatus));


                BIGFile bf = new BIGFile("Aktualizacja zobowiązania przez operatora", roWserialized, idUser);
                if (bf.UpdateObligation(rDetails, newSaldo, newDataWymag, newDataWezw, nrfakt) == null)
                    return " błąd podczas aktualizacji zobowiązania";

                return "";

            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }


        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_UpdateDebtor(string roWserialized, String Name, String Imie, String IdNumber, String Pesel, String Adres1, String Adres2, String NrKlienta, int SrcSystem, int idUser)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {



                BIGvw_DluznicyAktual rDetails = null;
                rDetails = (BIGvw_DluznicyAktual)Utils.DeserializeFromString(roWserialized, typeof(BIGvw_DluznicyAktual));


                BIGFile bf = new BIGFile("Aktualizacja danych dłużnika", roWserialized, idUser);
                if (!bf.UpdateDebtor(rDetails, Name, Imie, IdNumber, Pesel, Adres1, Adres2, NrKlienta, SrcSystem))
                    return " błąd podczas aktualizacji danych dłużnika ";

                return "";

            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }
        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_SuspendObligation(string roWserialized, DateTime suspendDate, int idUser)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {



                BIGvw_ObligationLastStatus rDetails = null;
                rDetails = (BIGvw_ObligationLastStatus)Utils.DeserializeFromString(roWserialized, typeof(BIGvw_ObligationLastStatus));


                BIGFile bf = new BIGFile("Zawieszenie zobowiązania przez operatora", roWserialized, idUser);
                if (!bf.suspendObligation(rDetails, suspendDate))
                    return " błąd podczas aktualizacji zobowiązania";

                return "";

            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_SuspendCase(string roWserialized, DateTime suspendDate, int idUser)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {



                BIGvw_DluznicyAktual rDetails = null;
                rDetails = (BIGvw_DluznicyAktual)Utils.DeserializeFromString(roWserialized, typeof(BIGvw_DluznicyAktual));


                BIGFile bf = new BIGFile("Zawieszenie sprawy przez operatora", roWserialized, idUser);
                if (!bf.suspendCase(rDetails, suspendDate))
                    return " błąd podczas zawieszania sprawy";

                return "";

            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_UnSuspendCase(string roWserialized, int idUser)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {



                BIGvw_DluznicyAktual rDetails = null;
                rDetails = (BIGvw_DluznicyAktual)Utils.DeserializeFromString(roWserialized, typeof(BIGvw_DluznicyAktual));


                BIGFile bf = new BIGFile("Podjęcie sprawy przez operatora", roWserialized, idUser);
                if (!bf.unSuspendCase(rDetails))
                    return " błąd podczas podejmowania zawieszonej sprawy";

                return "";

            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string BIG_NotifyCase(string roWserialized, int idUser)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {



                List<string> rDetails = null;
                List<BIGvw_DluznicyAktual> lstdet = new List<BIGvw_DluznicyAktual>();
                rDetails = (List<string>)Utils.DeserializeFromString(roWserialized, typeof(List<string>));
                if (rDetails.Any())
                    foreach (string xml in rDetails)
                    {
                        BIGvw_DluznicyAktual r = (BIGvw_DluznicyAktual)Utils.DeserializeFromString(xml, typeof(BIGvw_DluznicyAktual));
                        lstdet.Add(r);
                    }

                BIGFile bf = new BIGFile("Zlecenie wysyłki powaidomień", roWserialized, idUser);
                if (!bf.sendNotifications(lstdet))
                    return " błąd podczas żadania powiadomienia sprawy";

                return "";

            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string UZD_ImportPackage(DateTime theDay, String Importer)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {

                UZDOperations uopr = new UZDOperations();
                return uopr.InsertNewPackage(theDay, Importer);


            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string UZD_ImportPayment(DateTime theDay, String Importer)// sklej html'i         
        {
            string outstr = String.Empty;
            try
            {

                UZDOperations uopr = new UZDOperations();
                return uopr.InsertNewPaymentPackage(theDay, Importer);


            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }

        }

        [Invoke()]
        // [RequiresAuthentication]
        public string UZD_ComputeZaleglosc(int IdPakiet)// sklej html'i 
        {

            try
            {

                UZDOperations uopr = new UZDOperations();
                return uopr.ComputeZaleglosc(IdPakiet);


            }
            catch (Exception e)
            {

                return "error: " + Utils.exceptionMsg(e);
            }
        }



        [Invoke()]
        // [RequiresAuthentication]
        public string UZD_CheckNips(int IdPakiet)// sklej html'i         
        {
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                int retCode = 0;

                string appPath;


                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                try
                {
                    appPath = System.Web.Hosting.HostingEnvironment.MapPath("~/NipValidator/");
                    info.WorkingDirectory = appPath;
                    info.FileName = "WebClientJS.exe";
                    info.Arguments = " " + IdPakiet.ToString();
                    proc.StartInfo = info;
                    proc.Start();

                }
                catch (ArgumentException ex)
                {
                    return ex.Message + " " + ex.InnerException;
                }
                return "";

            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }

        }

        [Invoke()]
        // [RequiresAuthentication]
        public string UZD_DelPakiet(int IdPakiet)// sklej html'i 
        {
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    SqlParameter param1 = new SqlParameter("IdPakiet", IdPakiet);


                    object obj = context.ExecuteStoreQuery<object>("USPUZD_DelPakiet  @IdPakiet", param1).FirstOrDefault();
                    return "";
                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
            }
        }




        [Invoke()]
        // [RequiresAuthentication]
        public string ExportPdfs(string listaIdSkan, string IdPakiet)// sklej html'i 
        {

            // generacja dokumentów na podstawie Id spraw.
            List<int> listId;
            //string path = HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/LexEnaLog.txt") : "LexEnaLog.txt";

            Utils.LogWriter("Ekstrakcja pdfów");


            string path = WebConfigurationManager.AppSettings["PdfPath"];

            string dirname = DateTime.Today.ToString("yyyy-MM-dd");
            DirectoryInfo expDir = null;
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                {
                    di.Create();
                }
                try
                {
                    expDir = di.CreateSubdirectory(dirname);

                }
                catch (Exception e)
                {
                    ;
                }

            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd dostępu do folderu path " + ex.Message);
                return "Błąd dostępu do folderu path " + ex.Message;

            }

            listId = new List<int>();
            listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaIdSkan, typeof(List<int>));
            Guid _guid = new Guid(IdPakiet);

            try
            {

                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    using (wiena_centralEntities wienaContext = new wiena_centralEntities())
                    {
                        {

                            foreach (int id in listId)
                            {
                                // ustal nazwe zbioru
                                skan pdf = wienaContext.skan.Where(a => a.ident == id).FirstOrDefault();
                                DOKEKSTRvw_ekstrakcja dext = context.DOKEKSTRvw_ekstrakcja.Where(a => a.idpack == _guid && a.idskan == id).FirstOrDefault();
                                if (pdf != null && dext != null && pdf.skan1 != null && pdf.skan1.Length > 0)
                                {
                                    string filename = dext.sygnatura.Replace(" ", "-").Replace("/", "-")  + "_" + dext.nazwa.Replace("/", "_") + dext.typ;
                                    string fullname = expDir.FullName + "\\" + filename;
                                    if (File.Exists(fullname))
                                    {
                                        fullname = expDir.FullName + "\\" +  dext.sygnatura.Replace(" ", "-").Replace("/", "-")+"_01" + dext.typ;

                                    }
                                    if (File.Exists(fullname))
                                    {
                                        fullname = expDir.FullName + "\\" + dext.sygnatura.Replace(" ", "-").Replace("/", "-") + "_02" + dext.typ;

                                    }
                                    if (File.Exists(fullname))
                                    {
                                        fullname = expDir.FullName + "\\" + dext.sygnatura.Replace(" ", "-").Replace("/", "-") + "_03" + dext.typ;

                                    }
                                    File.WriteAllBytes(fullname, pdf.skan1);


                                }
                            }

                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd  " + ex.Message);
                return "Błąd: " + ex.Message;
            }
        }

        // oddzialy z Wieny
        [Invoke()]
        // [RequiresAuthentication]
        public string GetWienaOddzialy(int typFirmy)// informacja o imporcie pozwów z systemu         
        {
            string outxml = string.Empty;
            List<TypSlownikFiltered> lst = new List<TypSlownikFiltered>();
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                using (wiena_centralEntities wienaContext = new wiena_centralEntities())
                {

                    List<firma> lst1 = wienaContext.firma.OrderBy(a => a.ident).Where(a => a.aktywna == 1 && a.typ_firmy == typFirmy && a.typ == 1).ToList();
                    if (lst1 != null & lst1.Count > 0)
                    {
                        foreach (firma s in lst1)
                        {
                            lst.Add(new TypSlownikFiltered((s.nazwa + " " + s.nazwa1).Trim(), s.ident, Convert.ToInt16(s.typ_firmy), s.id_oddzial ?? 0, 0));
                        }


                    }

                }
                outxml = Utils.SerializeToString(lst, typeof(List<TypSlownikFiltered>));
                return outxml;

            }




            catch (Exception ex)
            {

                Utils.LogWriter("Błąd  " + ex.Message);
                return "Błąd: " + ex.Message;
            }


        }


        // repertoria z Wieny
        [Invoke()]
        // [RequiresAuthentication]
        public string GetWienaSymbole(int typFirmy)// informacja o imporcie pozwów z systemu         
        {
            string outxml = string.Empty;
            List<TypSlownikFiltered> lst = new List<TypSlownikFiltered>();

            try
            {
                using (wiena_centralEntities wienaContext = new wiena_centralEntities())
                {
                    wienaContext.Configuration.LazyLoadingEnabled = false;
                    int rok = DateTime.Today.Year;
                    List<vw_import_Symbole> lst1 = wienaContext.vw_import_Symbole.Where(a => a.typ_firmy == typFirmy || a.typ_firmy == null).OrderBy(a => a.ident).ThenBy(a => a.rok).ToList();
                    if (lst1 != null & lst1.Count > 0)
                    {
                        foreach (vw_import_Symbole s in lst1)
                        {
                            lst.Add(new TypSlownikFiltered(s.oznaczenie.Trim(), Convert.ToInt32(s.ident), Convert.ToInt16(s.poz), Convert.ToInt32(s.id_firmy), s.rok));

                        }

                    }

                    outxml = Utils.SerializeToString(lst, typeof(List<TypSlownikFiltered>));
                    return outxml;

                }



            }
            catch (Exception ex)
            {

                Utils.LogWriter("Błąd  " + ex.Message);
                return "Błąd: " + ex.Message;
            }


        }

        // repertoria z Wieny
        [Invoke()]
        // [RequiresAuthentication]
        public string GetWienaDowod()// informacja o imporcie pozwów z systemu         
        {
            string outxml = string.Empty;
            List<TypSlownikFiltered> lst = new List<TypSlownikFiltered>();

            try
            {
                // generacja dokumentów na podstawie Id spraw.
                using (wiena_centralEntities wienaContext = new wiena_centralEntities())
                {

                    List<dowod_slownik> lst1 = wienaContext.dowod_slownik.OrderBy(a => a.dowod).ToList();
                    if (lst1 != null & lst1.Count > 0)
                    {
                        foreach (dowod_slownik s in lst1)
                            lst.Add(new TypSlownikFiltered(s.dowod.Trim(), s.ident, s.typ, 0, 0));


                    }

                    outxml = Utils.SerializeToString(lst, typeof(List<TypSlownikFiltered>));
                    return outxml;

                }



            }
            catch (Exception ex)
            {

                Utils.LogWriter("Błąd  " + ex.Message);
                return "Błąd: " + ex.Message;
            }


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string ImportBillingDocument(string document, string filename, int User_Id, int Firma, int id_symbol, string symbolTekst, int nr, int rok, string dokName)// sklej html'i         
        {
            string xml, xmlUnicode;
            Sprawy spr;
            ImportSprawBilling imp = null;
            string symbol;
            XmlDocument docxml = new XmlDocument();
            try
            {

                byte[] infile = Convert.FromBase64String(document);


                spr = (Sprawy)Utils.DeserializeXMLFromString(infile, typeof(Sprawy));


                /*
                                try
                                {
                                    xml = System.Text.Encoding.UTF8.GetString(infile);
                                    xmlUnicode = System.Text.Encoding.Unicode.GetString(infile);

                                    spr = (Sprawy)Utils.DeserializeFromString(xml, typeof(Sprawy));
                                }
                                catch (Exception e)
                                {
                                    spr = (Sprawy)Utils.DeserializeFromString(xmlUnicode, typeof(Sprawy));

                                }
                  */
                if (spr.Odbiorcy != null && spr.Odbiorcy.GetUpperBound(0) >= 0)
                {
                    imp = new ImportSprawBilling();
                    imp.Oddzial = spr.Naglowek.Oddzial;
                    imp.System = spr.Naglowek.System;
                    imp.SprawaDescriptor = new SprImportDescriptor[spr.Odbiorcy.GetUpperBound(0) + 1];
                    // przenumerowamnie spraw

                    using (wiena_centralEntities context = new wiena_centralEntities())
                    {


                        for (int i = 0; i <= spr.Odbiorcy.GetUpperBound(0); i++)
                        {
                            imp.SprawaDescriptor[i] = new SprImportDescriptor();
                            imp.SprawaDescriptor[i].Odbiorca = spr.Odbiorcy[i];
                            imp.SprawaDescriptor[i].czyrej = true;
                            imp.SprawaDescriptor[i].czy_ok = false;
                            imp.SprawaDescriptor[i].id_symbol = id_symbol;
                            // walidacja nalezności


                        }

                        for (int i = 0; i <= spr.Odbiorcy.GetUpperBound(0); i++)
                        {
                            string nr_ewidancyjny = string.Empty;
                            nr_ewidancyjny = spr.Odbiorcy[i].Nr_ewidencyjny.Trim();
                            if (spr.Odbiorcy[i].Obciazenia != null)
                            {
                                for (int j = 0; j <= spr.Odbiorcy[i].Obciazenia.GetUpperBound(0); j++)
                                {
                                    string nr_dokumentu = string.Empty;
                                    nr_dokumentu = spr.Odbiorcy[i].Obciazenia[j].Nr_dokumentu.Trim();
                                    var sp = (from s in context.sprawa
                                              join n in context.naleznosc on s.ident equals n.id_sprawy
                                              join firma f in context.firma on s.id_firmy equals f.ident
                                              where s.nr_ewid == nr_ewidancyjny && n.tytul == nr_dokumentu && f.typ_firmy == Firma
                                              select s).FirstOrDefault();
                                    if (sp != null)
                                    {
                                        imp.SprawaDescriptor[i].czyrej = false;
                                        imp.SprawaDescriptor[i].message += " " + nr_dokumentu + " już istnieje";

                                    }

                                }

                            }
                        }

                        imp.SprawaDescriptor = imp.SprawaDescriptor.OrderBy(a => a.Odbiorca.PESEL).ThenBy(a => a.Odbiorca.NIP).ThenBy(a => a.nr).ToArray();
                        string prevPesel = string.Empty;
                        string prevNip = string.Empty;
                        nr--;
                        for (int i = 0; i <= spr.Odbiorcy.GetUpperBound(0); i++)
                        {
                            if (i > 0 && ((!String.IsNullOrWhiteSpace(prevPesel) && prevPesel == imp.SprawaDescriptor[i].Odbiorca.PESEL) || (!String.IsNullOrWhiteSpace(prevNip) && prevNip == imp.SprawaDescriptor[i].Odbiorca.NIP)))
                                ;
                            else
                                nr++;
                            if (!String.IsNullOrWhiteSpace(imp.SprawaDescriptor[i].Odbiorca.PESEL) && imp.SprawaDescriptor[i].Odbiorca.PESEL.Trim().Length < 11)
                                imp.SprawaDescriptor[i].Odbiorca.PESEL = String.Empty;
                            if (!String.IsNullOrWhiteSpace(imp.SprawaDescriptor[i].Odbiorca.NIP) && imp.SprawaDescriptor[i].Odbiorca.NIP.Trim().Length < 5)
                                imp.SprawaDescriptor[i].Odbiorca.NIP = String.Empty;

                            prevPesel = imp.SprawaDescriptor[i].Odbiorca.PESEL;
                            prevNip = imp.SprawaDescriptor[i].Odbiorca.NIP;

                            imp.SprawaDescriptor[i].nr = nr;
                            imp.SprawaDescriptor[i].rok = rok;
                            imp.SprawaDescriptor[i].sygn_obciaz = symbolTekst + "-" + nr.ToString() + "/" + rok.ToString();
                            
                            if (Firma == 1 && imp.SprawaDescriptor[i].Odbiorca != null && imp.SprawaDescriptor[i].Odbiorca.Obciazenia != null )
                            {
                                foreach (Obciazenie obc in imp.SprawaDescriptor[i].Odbiorca.Obciazenia)
                                {
                                    {

                                        if (obc.TypOdsetek > 0 || obc.Typ_odsetek > 0 )
                                        {
                                            if (obc.TypOdsetek > 0)
                                              imp.SprawaDescriptor[i].CzyUstawowe = (int)obc.TypOdsetek;
                                            else
                                                imp.SprawaDescriptor[i].CzyUstawowe = (int)obc.Typ_odsetek;
                                            break;
                                        }

                                    } 


                                } 


                            }
                            if (Firma == -1)
                                imp.SprawaDescriptor[i].CzyUstawowe = 8;

                        }




                    }

                }

                xml = Utils.SerializeToString(imp, typeof(ImportSprawBilling));
                return xml;




            }
            catch (Exception e)
            {

                return "Błąd: " + e.Message;
            }


        }


        [Invoke()]
        // [RequiresAuthentication]
        public string InsertBillingImport(string document, int Firma)// sklej html'i         
        {
            string xml;
            ImportSprawBilling imp = null;

            try
            {

                byte[] infile = Convert.FromBase64String(document);

                imp = (ImportSprawBilling)Utils.DeserializeXMLFromString(infile, typeof(ImportSprawBilling));
                /*
                xml = System.Text.Encoding.UTF8.GetString(infile);
                imp = (ImportSprawBilling)Utils.DeserializeFromString(xml, typeof(ImportSprawBilling));
                */
                ImportBillingHelper impHlp = new ImportBillingHelper();
                if (imp != null && imp.SprawaDescriptor.GetUpperBound(0) >= 0)
                {
                    int ii = 0;
                    while (ii <= imp.SprawaDescriptor.GetUpperBound(0))
                    {
                        // przebieg z identycznymi numerami spraw
                        if (!imp.SprawaDescriptor[ii].czyrej) { ii++; continue; };
                        int nr = imp.SprawaDescriptor[ii].nr;
                        int j = ii + 1;
                        List<string> nr_ewid = new List<string>();
                        List<Obciazenie> obc = new List<Obciazenie>();
                        while (j <= imp.SprawaDescriptor.GetUpperBound(0) && imp.SprawaDescriptor[j].nr == nr)
                        {
                            if (!imp.SprawaDescriptor[j].czyrej)
                                { j++; continue; }
                            nr_ewid.Add(imp.SprawaDescriptor[j].Odbiorca.Nr_ewidencyjny);
                            foreach (var ob in imp.SprawaDescriptor[j].Odbiorca.Obciazenia)
                                ob.Id_sprawy = j;
                            obc.AddRange(imp.SprawaDescriptor[j].Odbiorca.Obciazenia);
                            imp.SprawaDescriptor[j].czyrej = false;
                            j++;
                        }
                        if (j > ii + 1)
                        {
                            imp.SprawaDescriptor[ii].Odbiorca.Nr_ewidencyjnyArray = nr_ewid.ToArray();
                            foreach (var ob in imp.SprawaDescriptor[ii].Odbiorca.Obciazenia)
                            {
                                ob.Id_sprawy = ii;
                            }
                            imp.SprawaDescriptor[ii].Odbiorca.Obciazenia = imp.SprawaDescriptor[ii].Odbiorca.Obciazenia.Concat(obc.ToArray()).ToArray();

                        }
                        ii = j;
                    }

                    for (int i = 0; i <= imp.SprawaDescriptor.GetUpperBound(0); i++)
                    {
                        if (!imp.SprawaDescriptor[i].czyrej) continue;
                        bool result = impHlp.DoRegisterSpr(imp.SprawaDescriptor[i], imp.Oddzial, imp.DataKsiegowania, imp.NrDodowu, imp.IdOddzial, Firma, imp.System, imp.IdKancelaria);
                        if (!result)
                        {
                            Utils.LogWriter("Błąd importu dla " + imp.SprawaDescriptor[i].Odbiorca.Nazwa + " " + impHlp.ErrMsg);
                            imp.SprawaDescriptor[i].message = impHlp.ErrMsg;
                            impHlp.ErrMsg = string.Empty;
                            imp.SprawaDescriptor[i].message = impHlp.ErrMsg;
                            imp.SprawaDescriptor[i].czy_ok = false;
                            imp.SprawaDescriptor[i].czyrej = true;
                        }
                        else
                        {
                            imp.SprawaDescriptor[i].czy_ok = true;
                            imp.SprawaDescriptor[i].czyrej = false;
                        }

                    }
                }

                xml = Utils.SerializeToString(imp, typeof(ImportSprawBilling));
                return xml;




            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }


        }


        [Invoke()]
        // [RequiresAuthentication]
        public string GetWienaConfig(int typFirmy)// pobranie danych konfiguracyjnych z Wieny         
        {
            string outxml = string.Empty;
            List<TypSlownikFiltered> oddzialy = new List<TypSlownikFiltered>();
            List<TypSlownikFiltered> statusy = new List<TypSlownikFiltered>();
            List<TypSlownikFiltered> konta = new List<TypSlownikFiltered>();
            List<TypSlownikFiltered> kancelarie = new List<TypSlownikFiltered>();
            List<TypSlownikFiltered> radcowie = new List<TypSlownikFiltered>();
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                using (wiena_centralEntities wienaContext = new wiena_centralEntities())
                {
                    List<firma> lsttmp = wienaContext.firma.Where(a => a.typ == 1 && a.aktywna == 1 && a.typ_firmy == typFirmy).ToList();

                    if (lsttmp != null & lsttmp.Count > 0)
                    {
                        foreach (firma s in lsttmp)
                            oddzialy.Add(new TypSlownikFiltered(s.nazwa1, s.ident, s.typ_firmy ?? 0, s.id_oddzial ?? 0, 1));

                    }

                    List<status> lstst = wienaContext.status.Where(a => a.czyus == 0).OrderBy(a => a.etap).ThenBy(a => a.ident).ToList();

                    if (lstst != null & lstst.Count > 0)
                    {
                        foreach (status s in lstst)
                            statusy.Add(new TypSlownikFiltered(s.nazwa, s.ident, s.etap, s.def_skip ?? 0, 1));

                    }

                    List<konto> lstkn = wienaContext.konto.Where(a => a.aktywne == 1 && a.rodzaj >= 0 && a.id_firmy == typFirmy).OrderBy(a => a.rodzaj).ThenByDescending(a => a.typ.HasValue).ThenBy(a => a.typ).ThenBy(a => a.konto1).ToList();

                    if (lstkn != null & lstkn.Count > 0)
                    {
                        foreach (konto s in lstkn)
                            konta.Add(new TypSlownikFiltered(s.konto1, s.id, s.rodzaj, s.typ ?? 0, 0));

                    }

                    List<firma> lstknc = wienaContext.firma.Where(a => a.typ == 9 && a.aktywna == 1).OrderBy(a => a.ident).ToList();

                    if (lstknc != null & lstkn.Count > 0)
                    {
                        foreach (firma s in lstknc)
                        { if (!String.IsNullOrWhiteSpace(s.nazwa))
                                kancelarie.Add(new TypSlownikFiltered(s.nazwa, s.ident, 0, 0, 1));
                        }
                    }

                    List<radca> lstrad = wienaContext.radca.Where(a => a.aktywny == 1).OrderByDescending(a => a.ident).ToList();

                    if (lstrad != null & lstrad.Count > 0)
                    {
                        foreach (radca s in lstrad)
                        {
                            if (!String.IsNullOrWhiteSpace(s.nazwisko) || !String.IsNullOrWhiteSpace(s.imie))
                                radcowie.Add(new TypSlownikFiltered((s.imie + " " + s.nazwisko).Trim(), s.ident, 0, 0, 0));
                        }
                    }
                    WienaConfig wc = new WienaConfig();
                    wc.Konta = konta;
                    wc.Kancelarie = kancelarie;
                    wc.Statusy = statusy;
                    wc.Radcowie = radcowie;
                    wc.Oddzialy = oddzialy;

                    outxml = Utils.SerializeToString(wc, typeof(WienaConfig));
                    return outxml;

                }



            }
            catch (Exception ex)
            {

                Utils.LogWriter("Błąd  " + ex.Message);
                return "Błąd: " + ex.Message;
            }


        }
        [Invoke()]
        public string MergeCases(int typFirmy, string srcSygn, string destSygn)// pobranie danych konfiguracyjnych z Wieny         
        {// lączenie spraw dla EOP
            int idSrc;
            int idDest;

            using (wiena_centralEntities wienaContext = new wiena_centralEntities())
            {
                sprawa sSrc = wienaContext.sprawa.Where(a => a.sygnatura == srcSygn).FirstOrDefault();
                sprawa sDest = wienaContext.sprawa.Where(a => a.sygnatura == destSygn).FirstOrDefault();

                if (sSrc == null)
                {
                    return "Błąd - brak sprawy źródłowej";

                }
                if (sDest == null)
                {
                    return "Błąd - brak sprawy docelowej";

                }
                firma f1 = wienaContext.firma.Where(a => a.ident == sSrc.id_firmy).FirstOrDefault();
                if (f1 == null || (f1 != null && f1.typ_firmy != typFirmy))
                {
                    return "Błąd - brak sprawy źródłowej";
                }
                f1 = wienaContext.firma.Where(a => a.ident == sDest.id_firmy).FirstOrDefault();
                if (f1 == null || (f1 != null && f1.typ_firmy != typFirmy))
                {
                    return "Błąd - brak sprawy docelowej";
                }
                SqlParameter param1 = new SqlParameter("IdSrc", sSrc.ident);
                SqlParameter param2 = new SqlParameter("IdDest", sDest.ident);
                try
                {
                    object obj = wienaContext.sp_MergeCases(sSrc.ident, sDest.ident);// .Database.SqlQuery<object>("sp_MergeCases  @IdSrc, @IdDest", param1, param2);
                }
                catch (Exception ex)
                {
                    return "Błąd " + ex.Message;
                }

            }
            return "";

        }

        [Invoke()]
        public string GetDbNames()// pobranie nazw baz danych         
        {// lączenie spraw dla EOP
            string WienaDb = string.Empty;
            string LexEnaDb = string.Empty;



            using (wiena_centralEntities wienaContext = new wiena_centralEntities())
            {
                WienaDb = wienaContext.Database.Connection.Database;
                WienaDb += "(" + wienaContext.Database.Connection.DataSource + ")";

            }
            using (LexEnaMeritumEntities lexenaContext = new LexEnaMeritumEntities())
            {
                System.Data.EntityClient.EntityConnection conn = (System.Data.EntityClient.EntityConnection)lexenaContext.Connection;

                LexEnaDb = conn.StoreConnection.Database;
                LexEnaDb += "(" + lexenaContext.Connection.DataSource + ")";

            }

            return LexEnaDb + ";" + WienaDb;

        }


        [Invoke()]
        // [RequiresAuthentication]
        public string UpdateNal(string listaIdSpr, int CreationMode, DateTime dStanu)// sklej html'i         
        {
            try
            {
                DateTime? dKursu = null;
                // generacja dokumentów na podstawie Id spraw.
                List<int> listId;
                listId = new List<int>();
                listId = (List<int>)ToXMLSerializers.DeserializeFromString(listaIdSpr, typeof(List<int>));
                using (LexEnaMeritumEntities dbcontext = new LexEnaMeritumEntities())
                {
                    
                    ItemsForLawsuit items = new ItemsForLawsuit();
                    foreach (int i in listId)
                    {
                        int? iSprWiena = dbcontext.Sprawa.Where(x => x.id == i).Select(x => x.IdWiena).FirstOrDefault();
                        dbcontext.CommandTimeout = 1200;
                        
                        items.SprawaId = i;
                        SqlParameter param1 = new SqlParameter("@IdSprawy", i);
                        items.NaleznosciLst = dbcontext.ExecuteStoreQuery<NalToCorrect>("sp_EPU_UpdateNal  @IdSprawy ", param1).ToList();
                        items.DowodyLst = new List<TypDowod>();
                        items.UzasadnieniaLst = new List<TypDowod>();

                        List<Slownik> lstU = dbcontext.Slownik.ToList();
                        if (lstU != null && lstU.Any())
                        {
                            foreach (Slownik s in lstU)
                            {
                                TypDowod td = new TypDowod();
                                td.Nazwa = s.Nazwa;
                                td.Opis = s.Tresc;
                                td.Rodzaj = s.Typ ?? 0;
                                td.Grupa = s.grupa;
                                if (s.Typ == 1)
                                    items.UzasadnieniaLst.Add(td);
                                if (s.Typ == 2)
                                    items.DowodyLst.Add(td);
                            }
                        }
                        dKursu = dbcontext.Naleznosc.Where(a => a.Sprawa_id == i && a.TypNaleznosci_id != 13).OrderByDescending(a => a.data_n).Select(a => a.data_n).FirstOrDefault();
                        if (dKursu.HasValue)
                        {



                        }

                        if (CreationMode > 0 && iSprWiena.HasValue && iSprWiena.Value > 0)
                        {
                            ObliczZaleglosc oZal = new ObliczZaleglosc();
                            decimal odsetki = 0;

                            oZal.ObliczSpraweWiena(iSprWiena.Value, dStanu.AddDays(-1));
                            if (oZal.wynikiAll != null && oZal.wynikiAll.Count > 0)
                            {
                                foreach (var nale in oZal.wynikiNal)
                                {

                                    odsetki += nale.ods;

                                }


                            }
                            if (odsetki > 0)
                            {
                                NalToCorrect nt = new NalToCorrect();
                                nt.id = 0;
                                nt.data_n = dStanu;
                                nt.data_dok = dStanu;
                                nt.kwota = odsetki;
                                nt.zaleglosc = odsetki;
                                nt.opis = "odsetki skapitalizowane";

                                items.NaleznosciLst.Add(nt);

                            }


                        }
                        if (CreationMode > 0 && dKursu.HasValue)
                        {
                            DateTime d = getLastWorkingDay(dKursu.Value);
                            decimal value = getEuroExchangeRate(d);
                            if (value == 0)
                            {
                                d.AddDays(-1);
                                value = getEuroExchangeRate(d);
                                if (value == 0)
                                {
                                    d.AddDays(-1);
                                    value = getEuroExchangeRate(d);
                                }
                            }
                            if (value > 0)
                            {
                                NalToCorrect nt = new NalToCorrect();
                                nt.id = -1; 
                                nt.data_n = d;
                                nt.data_dok = d;
                                nt.kwota = Decimal.Round((value *40),2);
                                nt.zaleglosc = -1;
                                nt.opis = "Rekompensata kosztów odzyskiwania należności stanowiąca równowartość 40 EURO wg średniego kursu NBP na dzień " + d.ToString("yyy-MM-dd", CultureInfo.InvariantCulture);
                                items.NaleznosciLst.Add(nt);
                            }


                        }
                        return Utils.SerializeToString(items, typeof(ItemsForLawsuit));
                        
                    }

                }
            }
            catch (ArgumentException ex)
            {
                return "Błąd " + ex.Message + " " + ex.InnerException;
            }
            return "";


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string UpdateDataDokumentuNal(string nalXML)// sklej html'i         
        {
            try
            {
                // generacja dokumentów na podstawie Id spraw.
                ItemsForLawsuit item;
                List<NalToCorrect> lst;
                int id_sprawy = 0;

                item = (ItemsForLawsuit)ToXMLSerializers.DeserializeFromString(nalXML, typeof(ItemsForLawsuit));

                lst = item.NaleznosciLst;

                using (LexEnaMeritumEntities dbcontext = new LexEnaMeritumEntities())
                {


                    foreach (NalToCorrect i in lst)
                    {
                        Naleznosc n = dbcontext.Naleznosc.Where(a => a.Id == i.id).FirstOrDefault();
                        id_sprawy = n.Sprawa_id.Value;
                        if (n != null)
                            n.data_dok = i.data_dok;


                    }
                    // podmiana odsetek
                    if (id_sprawy > 0 && item.TypOdsetek > 0)
                    {

                        List<Naleznosc> nLst = dbcontext.Naleznosc.Where(a => a.Sprawa_id == id_sprawy).ToList();
                        if (nLst != null)
                            foreach (Naleznosc nn in nLst)
                            {
                                List<Odsetki> odsLst = dbcontext.Odsetki.Where(a => a.Naleznosc_Id == nn.Id).ToList();
                                if (odsLst != null)
                                {
                                    foreach (Odsetki o in odsLst)
                                    {
                                        ;// o.NazwyOdsetek_Id = item.TypOdsetek;

                                    }

                                }


                            }
                    }




                    dbcontext.SaveChanges();
                    // updating other 


                }
            }
            catch (ArgumentException ex)
            {
                return "Błąd zapisu danych" + ex.Message + " " + ex.InnerException;
            }
            return "";


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string CalculateWiekowanieSald(string listaIdOddzialy, string listaIdkont, DateTime dStanu, int Firma)// sklej html'i         
        {
            try
            {
                // generacja dokumentów na podstawie Id spraw.

                using (LexEnaMeritumEntities dbcontext = new LexEnaMeritumEntities())
                {
                    dbcontext.CommandTimeout = 1200;
                    SqlParameter param1 = new SqlParameter("@ListaOddzialow", listaIdOddzialy);
                    SqlParameter param2 = new SqlParameter("@ListaKont", listaIdkont);
                    SqlParameter param3 = new SqlParameter("@Dzien", dStanu);
                    SqlParameter param4 = new SqlParameter("@TypFirma", Firma);


                    System.Guid s = dbcontext.ExecuteStoreQuery<System.Guid>("USP_Wiekowanie_Salda  @ListaOddzialow, @ListaKont, @Dzien, @TypFirma", param1, param2, param3, param4).FirstOrDefault();
                    List<WiekowanieSalda> wLst = dbcontext.WiekowanieSalda.Where(a => a.myId == s && a.saldo != (a.do90 + a.do180 + a.do30 + a.do360 + a.do60 + a.pow360)).ToList();
                    if (wLst != null)
                    {
                        foreach (WiekowanieSalda w in wLst)
                        {
                            decimal sum = w.energia.Value - w.do30 - w.do60 - w.do90 - w.do180 - w.do360 - w.pow360 + w.koszty.Value + w.odsetki.Value;
                            decimal kwt = 0;
                            List<WiekowanieNaleznosci> nLst = dbcontext.WiekowanieNaleznosci.Where(a => a.id_sprawy == w.id_sprawy && a.pozostalo > 0 && a.myId == s).OrderBy(a => a.data_n).ToList();
                            if (nLst != null)
                            {

                                foreach (WiekowanieNaleznosci wn in nLst)
                                {
                                    if (wn.pozostalo > sum)
                                    {
                                        kwt = sum;
                                        sum = 0;
                                        wn.pozostalo -= sum;
                                    }
                                    else
                                    {
                                        kwt = wn.pozostalo.Value;
                                        sum -= wn.pozostalo.Value;
                                        wn.pozostalo = 0;
                                    }
                                    System.TimeSpan diff1 = dStanu.Subtract(wn.data_n.Value);
                                    if (diff1.Days <= 30)
                                        w.do30 += kwt;
                                    else if (diff1.Days <= 60)
                                        w.do60 += kwt;
                                    else if (diff1.Days <= 90)
                                        w.do90 += kwt;
                                    else if (diff1.Days <= 180)
                                        w.do180 += kwt;
                                    else if (diff1.Days <= 360)
                                        w.do360 += kwt;
                                    else
                                        w.pow360 += kwt;
                                    if (sum <= 0)
                                        break;
                                }
                            }

                            if (sum > 0)
                                w.pow360 += sum;
                            dbcontext.SaveChanges();
                        }


                    }

                    return "";
                }


            }
            catch (ArgumentException ex)
            {
                return "Błąd " + ex.Message + " " + ex.InnerException;
            }
            return "";


        }

        [Invoke()]
        // [RequiresAuthentication]
        public string CalculateWiekowanieNal(string listaIdOddzialy, string listaIdStatusy, DateTime dStanu, int Firma)// sklej html'i         
        {
            int idPakiet = 0;
            try
            {
          
                // generacja dokumentów na podstawie Id spraw.

                using (LexEnaMeritumEntities dbcontext = new LexEnaMeritumEntities())
                {
                    dbcontext.CommandTimeout = 1200;
                    SqlParameter param1 = new SqlParameter("@ListaOddzialow", listaIdOddzialy);
                    SqlParameter param2 = new SqlParameter("@ListaStatusow", listaIdStatusy);
                    SqlParameter param3 = new SqlParameter("@Dzien", dStanu);
                    SqlParameter param4 = new SqlParameter("@TypFirma", Firma);


                    idPakiet = dbcontext.ExecuteStoreQuery<int>("USP_Wiekowanie_Nal  @ListaOddzialow, @ListaStatusow, @Dzien, @TypFirma", param1, param2, param3, param4).FirstOrDefault();
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    try
                    {
                        string appPath = System.Web.Hosting.HostingEnvironment.MapPath("~/CalcZaleglosc/");
                        info.WorkingDirectory = appPath;
                        info.FileName = "CalcZaleglosc.exe";

                        Utils.LogWriter("Wywołanie z " + appPath); 
                        info.Arguments = " " + idPakiet.ToString() + " " + dStanu.ToString("yyyy-MM-dd") + "  1";
                        proc.StartInfo = info;
                        proc.Start();

                    }
                    catch (Exception ex)
                    {
                        Utils.LogWriter("Błąd wywolania kalkulatora odsetek: " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message : ""));
                        return "Błąd procedury" + ex.Message + " " + ex.InnerException;
                    }


                }

                return idPakiet.ToString();
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd wywolania kalkulatora odsetek: " + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : ""));

                return "Błąd inny" + ex.Message + " " + ex.InnerException;
            }



        }

        [Invoke()]
        // [RequiresAuthentication]
        public string GetCalcZalegloscProgress(int idPakiet)// sklej html'i         
        {
            string Msg;
            try
            {
                // generacja dokumentów na podstawie Id spraw.

                using (LexEnaMeritumEntities dbcontext = new LexEnaMeritumEntities())
                {

                    StanNalObliczPakiet st = dbcontext.StanNalObliczPakiet.Where(a => a.Id == idPakiet).FirstOrDefault();
                    if (st == null)
                    {
                        return "Błąd obliczania zaległości. Ponów operację";

                    }
                    if (st.Status == 200)
                        return "";
                    else
                        return st.StatOpis;

                }


            }
            catch (ArgumentException ex)
            {
                return "Błąd " + ex.Message + " " + ex.InnerException;
            }



        }

        [Invoke()]
        // [RequiresAuthentication]
        public string AttachDocuments(string listOfSprIDxml, int Firma)// sklej html'i         
        {
            string info = string.Empty;
            try
            {
                List<int> listId;
                listId = (List<int>)ToXMLSerializers.DeserializeFromString(listOfSprIDxml, typeof(List<int>));
               
                if (listId != null)
                {   // get 
                    string path = WebConfigurationManager.AppSettings["Chomik"];
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(path);
                        if (!di.Exists)
                        {
                            return "Błąd - nie znalezniono folderu " + path;

                        }


                    }
                    catch (Exception ex)
                    {
                        Utils.LogWriter("Błąd dostępu do folderu path " + ex.Message);
                        return "Błąd dostępu do folderu path " + ex.Message;

                    }

                    List<string> subdirectoryEntries = Directory.GetDirectories(path).ToList();


                    using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                    {
                       
                        foreach (int i in listId)
                        {
                            string nrEwid = string.Empty;
                            Sprawa spr = context.Sprawa.Where(a => a.id == i).FirstOrDefault();
                            List<DokWys> dwLst = context.DokWys.Include("PdfStore").Where(z => z.Sprawa_id == i && z.TypDok==999).ToList();

                            if (spr != null)
                            {

                                nrEwid = spr.NrEwid;

                            }
                            else
                                continue;

                            if (String.IsNullOrWhiteSpace(nrEwid))
                            {
                                info += "\n\r Brak nr ewidencyjnego w sprawie  " + spr.sygnatura;
                                continue;
                            }
                            nrEwid = nrEwid.Trim();
                            List<string> subdirs = subdirectoryEntries.Where(a => a.Contains(nrEwid)).ToList();
                            if (subdirs == null || subdirs.Count == 0)
                            {
                                info += "\n\r Brak folderu z dokumentami dla nr ewidencyjnego " + nrEwid + " w sprawie  " + spr.sygnatura;
                                continue;

                            }
                            DirectoryInfo din = null;
                            if (subdirs.Count > 0)
                            {
                                DateTime crDate = DateTime.MinValue;
                                foreach (string s in subdirs)
                                {

                                    DirectoryInfo d = new DirectoryInfo(s);
                                    if (d.CreationTime > crDate)
                                    {
                                        din = d;
                                        crDate = d.CreationTime;
                                    }

                                }
                            }
                            else
                            {
                                din = new DirectoryInfo(subdirs[0]);
                            }
                            // attach files
                            FileInfo[] Files = din.GetFiles();
                            foreach (FileInfo file in Files)
                            {
                                
                                byte[] thefile = File.ReadAllBytes(file.FullName);
                                DokWys dw = null;
                                PdfStore pdf = null;
                                bool isNew = true;
                                if (dwLst != null && dwLst.Any())
                                {
                                    foreach (DokWys d in dwLst)
                                    {
                                        isNew = true;
                                        if (d.Nazwa.ToLower().Trim() == file.Name.ToLower().Trim())
                                        {
                                            dw = d;
                                            isNew = false;
                                            if (dw.PdfStore != null && dw.PdfStore.Any())
                                            {
                                                pdf = dw.PdfStore.FirstOrDefault();

                                            }
                                            break;
                                        }

                                    }
                                }                          


                                if (pdf == null)
                                    pdf = new PdfStore();
                                pdf.name = file.Name;
                                pdf.type = pdf.name.ToLower().EndsWith(".pdf") ? 0 : 1;
                                pdf.value = thefile;
                                if (dw==null)
                                    dw = new DokWys();
                                dw.DataDok = new DateTime(2000, 1, 1);
                                dw.DataOdbioru = DateTime.Today;
                                dw.d_kreacji = DateTime.Now;
                                dw.Nazwa = file.Name;
                                dw.TypDok = 999; // dokumentacja inicjująca
                                dw.StatusDok = 0;
                                dw.Sprawa_id = i;
                                if (isNew)
                                {
                                    dw.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                                    dw.PdfStore.Add(pdf);
                                    context.DokWys.AddObject(dw);
                                }
                                    context.SaveChanges();
                            }


                        }
                    }
                }

            }
            catch (Exception ex)
            {

                return "Błąd " + ex.Message + " " + ex.InnerException ?? "";

            }
            if (!String.IsNullOrWhiteSpace(info))
                return "Błąd " + info;
            return "";
        }


        [Invoke()]
        // [RequiresAuthentication]
        public string ReturnCase(int SprawaId, int przyczyna, string opis)// sklej html'i         
        {
            string info = string.Empty;
            try
            {

                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    JednostkaWindykacji jwn = context.JednostkaWindykacji.Where(c => c.TypJednostki == 2).OrderBy(c => c.Id).FirstOrDefault();
                    if (jwn != null)
                    {
                        Dekretacja de = new Dekretacja();
                        de.JednostkaWindykacji_Id = jwn.Id;
                        de.DataDekretJednostka = DateTime.Now;
                        de.Czyus = 0;
                        de.d_kreacji = DateTime.Now;
                        de.Sprawa_id = SprawaId;
                        context.Dekretacja.AddObject(de);
                        de.Przyczyna = przyczyna;
                        de.Opis = opis;
                        context.SaveChanges();
                        return "";
                    }
                }
                return "Błąd podczas zwrotu sprawy do uzupełnienia ";
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd dostępu do folderu path " + ex.Message);
                return "Błąd podczas zwrotu sprawy do uzupełnienia " + ex.Message;

            }
                  
        }





        [Invoke()]
        // [RequiresAuthentication]
        public string GetZippedDocuments(int SprawaId)// sklej html'i         
        {

            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    List<DokWys> dwLst = context.DokWys.Include("PdfStore").Where(a => a.Sprawa_id == SprawaId).ToList();
                    if (dwLst != null && dwLst.Any())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Update))
                            {

                                foreach (DokWys dw in dwLst)
                                {
                                    if (dw.PdfStore != null && dw.PdfStore.Any())
                                    {
                                        ZipArchiveEntry orderEntry = archive.CreateEntry(dw.Nazwa); //create a file with this name
                                        using (BinaryWriter writer = new BinaryWriter(orderEntry.Open()))
                                        {
                                            writer.Write(dw.PdfStore.FirstOrDefault().value); //write the binary data
                                        }
                                    }

                                }
                            }

                            return Convert.ToBase64String(ms.ToArray());





                        }


                    }
                    return "Błąd - brak danych";

                }
            }

            catch (Exception ex)
            {

                return "Błąd " + ex.Message + " " + ex.InnerException ?? "";

            }
            
        }
        private DateTime getLastWorkingDay(DateTime day)
        {
            DateTime result;
            result = (new DateTime(day.Year, day.Month, 1)).AddDays(-1);
            if (result.DayOfWeek == DayOfWeek.Sunday)
                result = result.AddDays(-2);
            else
                if (result.DayOfWeek == DayOfWeek.Saturday)
                result = result.AddDays(-1);
            return result;
        }


        private decimal getEuroExchangeRate(DateTime day)
        {
            string s;
            decimal result=0;
            
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
                delegate
                {
                    return true;
                });
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.nbp.pl/api/exchangerates/rates/a/eur/" + day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)+ "/?format=json");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                s = reader.ReadToEnd();
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                Root json = jsonSerializer.Deserialize<Root>(s);
                result  = (decimal)(json.rates.FirstOrDefault().Mid);
                return result; 
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd pobierania kursu euro " + ex.Message);
                return (decimal)0.0;

            }


        }
    }

    public class Root
    {

        /*
            "table": "A",
            "no": "039/A/NBP/2018",
            "effectiveDate": "2018-02-23",
            "rates": [
        */
        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public IList<Rates> rates { get; set; }
        public Root()
        {

        }
    }

    public class Rates
    {
        /*
            "currency": "bat (Tajlandia)",
            "code": "THB",
            "mid": 0.1078 
        */
        public string Currency { get; set; }
        public string Code { get; set; }
        public double Mid { get; set; }
    }

}