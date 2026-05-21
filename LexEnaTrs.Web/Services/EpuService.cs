
namespace LexEnaTrs.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.Xml.Serialization;
    using System.IO;
    using System.Data.Entity;
    using ZadanieTimer;
     

    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public class EpuService : DomainService
    {
        [Invoke()]
        public string SprawdzPolaczenie(string userName, string userPassword, string APIKey)
        {
             EpuProxy.EpuProxyClient klient = new EpuProxy.EpuProxyClient(true);

                 //klient.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
            klient.setUserData(userName, userPassword, APIKey);
            return klient.GetApiVersion();

        }



        [Invoke()]
        public string Sprawdz()
        {
            List<ZadanieTimer.TypSlownikFiltered> LstReturn = new List<ZadanieTimer.TypSlownikFiltered>();
            LstReturn =  ZadanieTimerMain.OneTimedEvent();
            return Utils.SerializeToString(LstReturn, typeof(List<ZadanieTimer.TypSlownikFiltered>));
        }
        [Invoke()]
        public StatusZadania ZluzPozewZadanie(int IdPaczkiWLexEnaP, int idKonta, int idJednostki)
        {
            StatusZadania statusZadania = new StatusZadania { Status = -1 };
            try
            {

                ZadanieSet zadanie = new ZadanieSet();
                zadanie.DataRozpoczęcia = DateTime.Now;
                zadanie.Oczasie = false;
                zadanie.JednostkaWindykacji_Id = idJednostki;
                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {
                    Paczka _paczka = (from z in lexena.Paczka
                                      where z.Id == IdPaczkiWLexEnaP
                                      select z).FirstOrDefault();
                    zadanie.Parametry = this.SerializeToXML(new EPUParamModel { IdPozwuWLexEna = IdPaczkiWLexEnaP, KontoEpuId = idKonta });
                    zadanie.Opis = (_paczka.Id).ToString();
                    switch (_paczka.TypDok)
                    {
                        case 10:  // pozwy
                            zadanie.NazwaZadania = EpuZadania.ZlozPozew.ToString();
                            zadanie.TypZadaniaId = 2;
                            break;

                        case 6:
                        case 3: // dokumenty
                            zadanie.NazwaZadania = EpuZadania.ZlozDokument.ToString();
                            zadanie.TypZadaniaId = 9;
                            break;


                    }
                    string IdPaczki = (_paczka.Id).ToString();
                    ZadanieSet zd  = (from z in lexena.ZadanieSet
                                      where z.Status == 0 && z.Opis == IdPaczki
                                      select z).FirstOrDefault();
                    if (zd != null)
                    {
                        lexena.ZadanieSet.DeleteObject(zd);
                    }
                    lexena.AddToZadanieSet(zadanie);
                    lexena.SaveChanges();
                    statusZadania.IdZadania = zadanie.Id;
                    statusZadania.Opis = "Zgłoszono do kolejki";
                    statusZadania.Status = 0;
                    ZadanieTimerMain.OneTimedEvent();

                    /*
                    PozewDomainService poz = new PozewDomainService();
                    string ret = poz.LunchApp(0); 
                    if (ret != "OK")
                    {
                        statusZadania.Opis = ret;
                        if (ret.StartsWith("Błąd"))
                            statusZadania.Status = -100; // status zadania - błąd podxczas uruchamina
                        else
                            statusZadania.Status = -2;  // proces jest uruchomiuony  

                    }
                    */

                }
                return statusZadania;
            }
            catch (Exception ex)
            {
                statusZadania.Status = -1;
                statusZadania.Opis = " problem w czasie dodawania do kolejki" + ex.ToString();
                statusZadania.IdZadania = -1;
                return statusZadania;
            }

        }
        [Invoke()]
        public StatusZadania MojeSprawy(DateTime? dataOdP, DateTime? dataDoP, int? kryteriumFiltrowaniaP, string filtrSlownyP, int kontoEpuIdP, bool Oczasie, DateTime? terminRozpoczęcia) 
        {
            StatusZadania statusZadania = new StatusZadania { Status = -1 };
            try
            {
                ZadanieSet zadanie = new ZadanieSet();
                if (Oczasie && terminRozpoczęcia != null)
                {
                    zadanie.DataRozpoczęcia = (DateTime)terminRozpoczęcia;
                    zadanie.Oczasie = true;
                }
                else
                {
                    zadanie.DataRozpoczęcia = DateTime.Now;
                    zadanie.Oczasie = false;
                }
                zadanie.NazwaZadania = EpuZadania.MojeSprawy.ToString();
                zadanie.TypZadaniaId = 3;
                zadanie.Parametry = this.SerializeToXML(new EPUParamModel { DataDo = dataOdP, DataOd=dataOdP, KryteriumFiltrowania=kryteriumFiltrowaniaP, FiltrSlowny=filtrSlownyP  });
                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {
                    lexena.AddToZadanieSet(zadanie);
                    lexena.SaveChanges();
                    statusZadania.IdZadania = zadanie.Id;
                    statusZadania.Opis = "Zgłoszono do kolejki";
                    statusZadania.Status = 0;
                    return statusZadania;

                }

            }
            catch(Exception ex) 
            {
                statusZadania.Status = -1;
                statusZadania.Opis = " problem w czasie dodawania do kolejki" + ex.ToString();
                statusZadania.IdZadania = -1;
                return statusZadania;
            }
            //return statusZadania; 
        }
        [Invoke()]
        public StatusZadania MojeSprawyPrzezZakresSygnatur(int numerOdP, int numerDoP, int rokP) { return new StatusZadania(); }

        [Invoke()]
        public StatusZadania PobierzStatusZadania(int idZadania)
        {
            using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
            {
                ZadanieSet zadanie = (from z in lexena.ZadanieSet
                                      where z.Id == (int)idZadania
                                      select z).FirstOrDefault();
                return new StatusZadania
                {
                    Status = zadanie.Status
                };
            }
        }

        public IEnumerable<StatusZadania> GetStstusZadania()
        {
             throw new NotImplementedException();
        }

        private string SerializeToXML(EPUParamModel epuParamModel)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EPUParamModel));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, epuParamModel);
            return stringWriter.ToString();
        }
    
    }

    [Serializable]
    public class StatusZadania  
    {
        
        public int Status { set;get; }
        public string Opis { set; get; }
        [Key]
        public int IdZadania { set; get; }
        
    }
    
    public enum EpuZadania { ZlozPozew,ZlozZazalenie, ZlozSkarge,ZlozSprzeciw,MojeSprawy, ZlozDokument}

    
}



