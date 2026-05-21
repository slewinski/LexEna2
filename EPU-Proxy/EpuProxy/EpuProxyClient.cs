using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpuProxy.EpuSrv;
using System.ServiceModel.Configuration;
using System.ServiceModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Net;
using System.Net.Security;
using ZadanieTimer;

namespace EpuProxy
{

    [Serializable]
    public class StatusZadania
    {

        public int Status { set; get; }
        public string Opis { set; get; }
        public int IdZadania { set; get; }

    }


    public class EpuProxyClient
    {
        EpuSrv.EpuServiceClient _epuService = null;
        WSHttpBinding _secureServiceBinding = null;
        
        BasicHttpBinding _serviceBinding = null;
        string _EndpointAddr = "http://upom.currenda.pl/api/EpuWS.EpuService.svc";
        string usrName = null;
        string usrPass = null;
        string apiKey = null;
        string opis_zadania = "";

        

        public string EndpointAddr
        {
            get { return _EndpointAddr; }
            set
            {
                _EndpointAddr = value;
             }
        }

        public string GetOpis { get { return opis_zadania; } }
        public EpuProxyClient(bool isSecureP)
        {
            try
            {
                
                if (!isSecureP) {
                    _EndpointAddr = "http://localhost/EpuWSApplication/EpuWS.EpuService.svc";
                 }
                else {
                    _EndpointAddr = "https://www.e-sad.gov.pl/api2/EpuWS.EpuService.svc";
                }
                //Tylko w czasie dev
                Database.SetInitializer<EPUDbContext>(new EpuProxyInitializer());
                EndpointAddress endpointAddress = new EndpointAddress(_EndpointAddr);
                if (!isSecureP)
                {
                    
                    _serviceBinding = new BasicHttpBinding();
                    _serviceBinding.SendTimeout = new TimeSpan(12000000000);//
                    _serviceBinding.MaxReceivedMessageSize = 2147483647;
                    _serviceBinding.MaxBufferSize = 2147483647;
                    _serviceBinding.ReaderQuotas.MaxStringContentLength = 1048576;

                    _epuService = new EpuSrv.EpuServiceClient(_serviceBinding, endpointAddress);
                    
                }
                else
                {

#if DEBUG                  
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.Expect100Continue = false;

                    var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    binding.UseDefaultWebProxy = false;
                    binding.BypassProxyOnLocal = true;

                    binding.SendTimeout = TimeSpan.FromMinutes(5);
                    binding.ReceiveTimeout = TimeSpan.FromMinutes(5);
                    binding.OpenTimeout = TimeSpan.FromMinutes(1);
                    binding.CloseTimeout = TimeSpan.FromMinutes(1);

                    var endpoint = new EndpointAddress("https://www.e-sad.gov.pl/api2/EpuWS.EpuService.svc");
                    _epuService = new EpuSrv.EpuServiceClient(binding, endpoint);
#else


                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    ServicePointManager.ServerCertificateValidationCallback =
                        (sender, cert, chain, errors) => true; // tylko testowo

                    _serviceBinding = new BasicHttpBinding();
                    _serviceBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                    _serviceBinding.SendTimeout = TimeSpan.FromMinutes(5);
                    _serviceBinding.ReceiveTimeout = TimeSpan.FromMinutes(5);
                    _serviceBinding.MaxReceivedMessageSize = int.MaxValue;
                    _serviceBinding.MaxBufferSize = int.MaxValue;
                    _serviceBinding.ReaderQuotas.MaxStringContentLength = 1048576;

                    _epuService = new EpuSrv.EpuServiceClient(_serviceBinding, endpointAddress);

#endif


                    //  //_secureServiceBinding = new WSHttpBinding();
                    //  
                    //  ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    //  ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
                    //  delegate
                    //  {
                    //      return true;
                    //  });
                    //  _serviceBinding = new BasicHttpBinding();
                    //   _serviceBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                    //   _serviceBinding.SendTimeout = new TimeSpan(12000000000);//
                    //  _serviceBinding.ReceiveTimeout = new TimeSpan(0, 5, 0);
                    //   _serviceBinding.MaxReceivedMessageSize = 2147483647;
                    //   _serviceBinding.MaxBufferSize = 2147483647;
                    //   _serviceBinding.ReaderQuotas.MaxStringContentLength = 1048576;
                    //        //_secureServiceBinding.Security.Mode = SecurityMode.Transport;
                    //  _epuService = new EpuSrv.EpuServiceClient(_serviceBinding, endpointAddress);
                    //  /********************************/                      
                    // 
                    //  /************************************/

                }
            }
            catch
            {
                throw; 
            }
        }
        public int setUserData(string usrNameP, string usrPassP, string apiKeyP)
        {
            try 
            {
                usrName = usrNameP;
                usrPass = usrPassP;
                apiKey = apiKeyP;
                return 0;
            }
            catch 
            { 
                return -1; 
            }

        }
        public string GetApiVersion()
        {
            try
            {
                _epuService.WersjaAPI(usrName, usrPass, apiKey);
                return string.Empty;
            }
            catch (Exception ex)
            {

                return "Wystąpił błąd połączenia z usługą sieciową e-Sądu " + ex.Message;
            }

        }

        public int getListaKanceraii()
        {
            try
            {
                int pageSize = 100;
                var lkModel = new ListaKancelariiKomorniczychOutputDataModel
                {
                    ListaKancelariiKomorniczychOutputElementModels = new List<ListaKancelariiKomorniczychOutputElementModel>()
                };

                ListaKancelariiKomoniczychOutputData wynik = _epuService.ListaKancelariiKomoniczych(usrName, usrPass, apiKey, 1, pageSize);
                ListaKancelariiKomorniczychOutputElementModel kkModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.OK)
                {
                    lkModel.IloscWszystkichElementow = wynik.liczbaWszystkichElementow;
                    lkModel.Informacja = wynik.informacja;
                    lkModel.KodOdpowiedzi = wynik.kod;
                    lkModel.Opis = wynik.opis;
                    
                    foreach (var element in wynik.listaKancelariiKomorniczych)
                    {
                        kkModel = new ListaKancelariiKomorniczychOutputElementModel { NazwaKancelarii = element.nazwaKancelarii };
                        lkModel.ListaKancelariiKomorniczychOutputElementModels.Add(kkModel);
                    }
                    
                    int iloscStron = wynik.iloscStron;
                    for (int i = 2; i <= iloscStron; i++)
                    {

                        wynik = _epuService.ListaKancelariiKomoniczych(usrName, usrPass, apiKey, i, pageSize);
                        foreach (var element in wynik.listaKancelariiKomorniczych)
                        {
                            lkModel.ListaKancelariiKomorniczychOutputElementModels.Add(new ListaKancelariiKomorniczychOutputElementModel { NazwaKancelarii = element.nazwaKancelarii });
                        }
                    }

                }
                else 
                { 
                    return -(int)wynik.kod; 
                }  
                
                //blok entity framework
                using( var db = new EPUDbContext())
                {
                    string script = ((IObjectContextAdapter)db).ObjectContext.CreateDatabaseScript();
                    db.ListaKancelariiKomorniczychOutputDataModels.Add(lkModel);
                    return db.SaveChanges();
                }

                
            }
            catch
            {
                return -1;
            }
        }
        /*
                public StatusZadania MojeSprawyAdd(DateTime? dataOdP, DateTime? dataDoP, int? kryteriumFiltrowaniaP, string filtrSlownyP, int kontoEpuIdP, bool Oczasie, DateTime? terminRozpoczęcia)
                {
                    StatusZadania statusZadania = new StatusZadania { Status = -1 };
                    try
                    {
                         using (var aktualizacja = new LexEnaMeritumEntities())
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
                        zadanie.Parametry = this.SerializeToXML(new EPUParamModel { DataDo = dataOdP, DataOd = dataOdP, KryteriumFiltrowania = kryteriumFiltrowaniaP, FiltrSlowny = filtrSlownyP });
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
                    catch (Exception ex)
                    {
                        statusZadania.Status = -1;
                        statusZadania.Opis = " problem w czasie dodawania do kolejki" + ex.ToString();
                        statusZadania.IdZadania = -1;
                        return statusZadania;
                    }
                    return statusZadania;
                }

                private string SerializeToString(object objToSerialize, Type type, bool czyCurr)
                {
                    var output = new MemoryStream();
                    var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
                    string outputString;
                    using (var xmlWriter = XmlWriter.Create(output, settings))
                    {
                        var serializer = new XmlSerializer(type);


                        xmlWriter.WriteStartDocument();
                        if (czyCurr)
                        {
                            var namespaces = new XmlSerializerNamespaces();
                            namespaces.Add("curr", "http://www.currenda.pl/epu");
                            serializer.Serialize(xmlWriter, objToSerialize, namespaces);
                        }
                        else
                            serializer.Serialize(xmlWriter, objToSerialize);

                    }
                    output.Seek(0L, SeekOrigin.Begin);
                    // zamina stream na string
                    var reader = new StreamReader(output);
                    outputString = reader.ReadToEnd();
                    return outputString;

                }
        */
        public int mojeSprawy(DateTime? dataOdP, DateTime? dataDoP, int? kryteriumFiltrowaniaP, string filtrSlownyP, ref string opisKomunikacjiP)
        {
            String dane;
            
            try
            {
                int pageSize = 100;
                var msModel = new MojeSprawyOutputDataModel
                {
                    SprawaOutputElementModels = new List<SprawaOutputElementModel>()
                };

                MojeSprawyOutputData wynik = _epuService.MojeSprawy(usrName, usrPass, apiKey, 1, pageSize, dataOdP, dataDoP, kryteriumFiltrowaniaP, filtrSlownyP);
                SprawaOutputElementModel spModel = null;



                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.OK)
                {
                    
                    msModel.IloscWszystkichElementow = wynik.liczbaWszystkichElementow;
                    msModel.Informacja = wynik.informacja;
                    msModel.KodOdpowiedzi = wynik.kod;
                    msModel.Opis = wynik.opis;

                    if (wynik.listaSpraw.Count() > 0)
                    {
                        

                        foreach (var element in wynik.listaSpraw)
                        {
                            if (!element.sygnaturaWgPowoda.Contains("@")) continue;
                                spModel = new SprawaOutputElementModel
                            {
                                DataPrawomocnosci = element.dataPrawomocnosci,
                                DataWplywu = element.dataWplywu,
                                DataZakreslenia = element.dataZakreslenia,
                                KwotaSporu = element.kwotaSporu,
                                RolaWSprawie = element.rolaWSprawie,
                                StanSprawy = element.stanSprawy,
                                StronyWSprawie = element.stronyWSprawie,
                                SygnaturaWgPowoda = element.sygnaturaWgPowoda,
                                SygnaturaSprawy = element.sygnaturaSprawy,
                                IdSprawy = element.id
                            };
                            msModel.SprawaOutputElementModels.Add(spModel);
                        }
                    }
                    int iloscStron = wynik.iloscStron;
                    for (int i = 2; i <= iloscStron; i++)
                    {

                        System.Threading.Thread.Sleep(1000);
                        wynik = wynik = _epuService.MojeSprawy(usrName, usrPass, apiKey, i, pageSize, dataOdP, dataDoP, kryteriumFiltrowaniaP, filtrSlownyP);
                        if (wynik.listaSpraw.Count() > 0)
                        {
                            foreach (var element in wynik.listaSpraw)
                            {
                                if (!element.sygnaturaWgPowoda.Contains("@")) continue;
                                spModel = new SprawaOutputElementModel
                                {
                                    DataPrawomocnosci = element.dataPrawomocnosci,
                                    DataWplywu = element.dataWplywu,
                                    DataZakreslenia = element.dataZakreslenia,
                                    KwotaSporu = element.kwotaSporu,
                                    RolaWSprawie = element.rolaWSprawie,
                                    StanSprawy = element.stanSprawy,
                                    StronyWSprawie = element.stronyWSprawie,
                                    SygnaturaWgPowoda = element.sygnaturaWgPowoda,
                                    SygnaturaSprawy = element.sygnaturaSprawy,
                                    IdSprawy = element.id
                                };
                                msModel.SprawaOutputElementModels.Add(spModel);
                            }
                        }

                    }

                }
                else
                {
                    return (int)wynik.kod;
                }

                //blok entity framework
                using (var db = new EPUDbContext())
                {
                    db.MojeSprawyOutputDataModels.Add(msModel);
                    db.SaveChanges();
                    return msModel.Id;
                }


            }
            catch (FaultException<MojeSprawyOutputData> msex)
            {
                opisKomunikacjiP = "Wyjątek z EPU wynikający z wykonywania procedurey" + msex.Detail.ToString();

                return -3000;
            }
            catch (System.Net.WebException we)
            {
                opisKomunikacjiP = "Błąd w warstwie komunikacyjnej z EPU" + we.ToString();
                return -2000;
            }
            catch (Exception ex)
            {
                opisKomunikacjiP = "Wyjątek w komunikacji z epu lub błędu logicznego w danych wejściowych:" +  ex.ToString();
                return -1000;
            }
        }
        public int mojeOrzeczenia(DateTime? dataOdP, DateTime? dataDoP, int? kryteriumFiltrowaniaP, string filtrSlownyP, ref string opisKomunikacjiP)
        {
            try
            {
                int pageSize = 100;
                var moModel = new MojeOrzeczeniaVer2OutputDataModel
                {
                    OrzeczenieVer2OutputElementModels = new List<OrzeczenieVer2OutputElementModel>()
                };

                MojeOrzeczeniaVer2OutputData wynik = _epuService.MojeOrzeczeniaVer2(usrName, usrPass, apiKey, 1, pageSize, dataOdP, dataDoP, kryteriumFiltrowaniaP, filtrSlownyP);
                OrzeczenieVer2OutputElementModel mopModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.OK)
                {
                    moModel.IloscWszystkichElementow = wynik.liczbaWszystkichElementow;
                    moModel.Informacja = wynik.informacja;
                    moModel.KodOdpowiedzi = wynik.kod;
                    moModel.Opis = wynik.opis;

                    if (wynik.listaOrzeczen.Count() > 0)
                    {
                        foreach (var element in wynik.listaOrzeczen)
                        {
                            if (!element.sygnaturaWgPowoda.Contains("@")) continue;
                            mopModel = new OrzeczenieVer2OutputElementModel
                            {
                                DataPrawomocnosci = element.dataPrawomocnosci,
                                DataOrzeczenia  =   element.dataOrzeczenia,
                                DataKlauzuli = element.dataKlauzuli,
                                DokumentXML = element.dokumentXml,
                                Id_klauzula = element.id_klauzula,
                                IdOrzeczeia = element.id,
                                Instancja = element.instancja,
                                KodKlauzuli = element.kodKlauzuli,
                                NazwaDecyzji = element.nazwaDecyzji,
                                Status = element.Status,
                                SygnaturaSprawy = element.sygnaturaSprawy,
                                SygnaturaWgPowoda = element.sygnaturaWgPowoda
                            };
                            moModel.OrzeczenieVer2OutputElementModels.Add(mopModel);
                        }
                    }
                    int iloscStron = wynik.iloscStron;
                    for (int i = 2; i <= iloscStron; i++)
                    {

                        System.Threading.Thread.Sleep(1000);
                        wynik = _epuService.MojeOrzeczeniaVer2(usrName, usrPass, apiKey, i, pageSize, dataOdP, dataDoP, kryteriumFiltrowaniaP, filtrSlownyP);
                        if (wynik.listaOrzeczen.Count() > 0)
                        {
                            foreach (var element in wynik.listaOrzeczen)
                            {
                                if (!element.sygnaturaWgPowoda.Contains("@")) continue;
                                mopModel = new OrzeczenieVer2OutputElementModel
                                {
                                    DataPrawomocnosci = element.dataPrawomocnosci,
                                    DataOrzeczenia = element.dataOrzeczenia,
                                    DataKlauzuli = element.dataKlauzuli,
                                    DokumentXML = element.dokumentXml,
                                    Id_klauzula = element.id_klauzula,
                                    IdOrzeczeia = element.id,
                                    Instancja = element.instancja,
                                    KodKlauzuli = element.kodKlauzuli,
                                    NazwaDecyzji = element.nazwaDecyzji,
                                    Status = element.Status,
                                    SygnaturaSprawy = element.sygnaturaSprawy,
                                    SygnaturaWgPowoda = element.sygnaturaWgPowoda 
                                };
                                moModel.OrzeczenieVer2OutputElementModels.Add(mopModel);
                            }
                        }

                    }

                }
                else
                {
                    return (int)wynik.kod;
                }

                //blok entity framework
                using (var db = new EPUDbContext())
                {
                    db.MojeOrzeczeniaVer2OutputDataModels.Add(moModel);
                    db.SaveChanges();
                    return moModel.Id;
                }


            }
            catch (FaultException<MojeSprawyOutputData> msex)
            {
                opisKomunikacjiP = "Wyjątek z EPU wynikający z wykonywania procedurey" + msex.Detail.ToString();

                return -3000;
            }
            catch (System.Net.WebException we)
            {
                opisKomunikacjiP = "Błąd w warstwie komunikacyjnej z EPU" + we.ToString();
                return -2000;
            }
            catch (Exception ex)
            {
                opisKomunikacjiP = "Wyjątek w komunikacji z epu lub błędu logicznego w danych wejściowych:" + ex.ToString();
                return -1000;
            }
        }

        public int mojeSprawyPrzezZakresSygnatur(int numerOdP, int numerDoP, int rokp)
        {
            try
            {
                int pageSize = 100;
                var msModel = new MojeSprawyOutputDataModel
                {
                    SprawaOutputElementModels = new List<SprawaOutputElementModel>()
                };

                MojeSprawyOutputData wynik = _epuService.MojeSprawyPrzezZakresSygnatur(usrName, usrPass, apiKey, 1, pageSize, numerOdP, numerDoP, rokp);
                SprawaOutputElementModel spModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.OK)
                {
                    msModel.IloscWszystkichElementow = wynik.liczbaWszystkichElementow;
                    msModel.Informacja = wynik.informacja;
                    msModel.KodOdpowiedzi = wynik.kod;
                    msModel.Opis = wynik.opis;

                    foreach (var element in wynik.listaSpraw)
                    {
                        spModel = new SprawaOutputElementModel
                        {
                            DataPrawomocnosci = element.dataPrawomocnosci,
                            DataWplywu = element.dataWplywu,
                            DataZakreslenia = element.dataZakreslenia,
                            KwotaSporu = element.kwotaSporu,
                            RolaWSprawie = element.rolaWSprawie,
                            StanSprawy = element.stanSprawy,
                            StronyWSprawie = element.stronyWSprawie,
                            SygnaturaWgPowoda = element.sygnaturaWgPowoda,
                            SygnaturaSprawy = element.sygnaturaSprawy,
                            IdSprawy = element.id
                        };
                        msModel.SprawaOutputElementModels.Add(spModel);
                    }

                    int iloscStron = wynik.iloscStron;
                    for (int i = 2; i <= iloscStron; i++)
                    {


                        wynik = wynik = _epuService.MojeSprawyPrzezZakresSygnatur(usrName, usrPass, apiKey, i, pageSize, numerOdP, numerDoP, rokp);
                        foreach (var element in wynik.listaSpraw)
                        {
                            spModel = new SprawaOutputElementModel
                            {
                                DataPrawomocnosci = element.dataPrawomocnosci,
                                DataWplywu = element.dataWplywu,
                                DataZakreslenia = element.dataZakreslenia,
                                KwotaSporu = element.kwotaSporu,
                                RolaWSprawie = element.rolaWSprawie,
                                StanSprawy = element.stanSprawy,
                                StronyWSprawie = element.stronyWSprawie,
                                SygnaturaWgPowoda = element.sygnaturaWgPowoda,
                                SygnaturaSprawy = element.sygnaturaSprawy,
                                IdSprawy = element.id
                            };
                            msModel.SprawaOutputElementModels.Add(spModel);
                        }

                    }

                }
                else
                {
                    return -(int)wynik.kod;
                }

                //blok entity framework
                using (var db = new EPUDbContext())
                {
                    db.MojeSprawyOutputDataModels.Add(msModel);
                    return db.SaveChanges();
                }


            }
            catch
            {
                return -1;
            }
        }
        public int mojeNakazy(DateTime? dataOdP, DateTime? dataDoP, int? kryteriumFiltrowaniaP, string filtrSlownyP, ref string Listabledow)
        {
            try
            {
                int pageSize = 100;
                var mnModel = new MojeNakazyOutputDataModel
                {
                    NakazOutputElementModels = new List<NakazOutputElementModel>()
                };

                kryteriumFiltrowaniaP = 0;
                filtrSlownyP = "";
                Utils.LogWriter("Moje Nakazy parametry ePUService wywołania:" + dataOdP.ToString() + ";" + dataDoP.ToString());
                Utils.LogWriter("Moje Nakazy (3 krok ) parametry ePUService wywołania:" + kryteriumFiltrowaniaP.ToString() );
                Utils.LogWriter("Moje Nakazy (4 krok ) parametry ePUService wywołania:" + filtrSlownyP);
               
                MojeNakazyOutputData wynik = _epuService.MojeNakazy(usrName, usrPass, apiKey, 1, pageSize, dataOdP, dataDoP, kryteriumFiltrowaniaP, filtrSlownyP);
                NakazOutputElementModel mnjModel = null;
                Utils.LogWriter("Moje Nakazy WS wykonany OK");
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.OK)
                {
                    mnModel.IloscWszystkichElementow = wynik.liczbaWszystkichElementow;
                    mnModel.Informacja = wynik.informacja;
                    mnModel.KodOdpowiedzi = wynik.kod;
                    mnModel.Opis = wynik.opis;


                    foreach (var element in wynik.mojeNakazy)
                    {
                        if (!element.sygnaturaWgPowoda.Contains("@")) continue;
                        mnjModel = new NakazOutputElementModel
                        {
                            DataPrawomocnosci = element.dataPrawomocnosci,
                            IdNakazu = element.id,
                            DataOrzeczenia = element.dataOrzeczenia,
                            DokumentXML = element.dokumentXml,
                            KodDecyzji = element.kodDecyzji,
                            StatusDokumentu = element.statusDokumentu,
                            SygnaturaSprawy = element.sygnaturaSprawy,
                            SygnaturaWgPowoda = element.sygnaturaWgPowoda,
                            
                        };
                        
                        mnModel.NakazOutputElementModels.Add(mnjModel);
                    }
                    

                    int iloscStron = wynik.iloscStron;
                    for (int i = 2; i <= iloscStron; i++)
                    {

                        System.Threading.Thread.Sleep(1000);
                        wynik = wynik = _epuService.MojeNakazy(usrName, usrPass, apiKey, i, pageSize, dataOdP, dataDoP, kryteriumFiltrowaniaP, filtrSlownyP);
                        foreach (var element in wynik.mojeNakazy)
                        {
                            if (!element.sygnaturaWgPowoda.Contains("@")) continue;
                            mnjModel = new NakazOutputElementModel
                            {
                                DataPrawomocnosci = element.dataPrawomocnosci,
                                IdNakazu = element.id,
                                DataOrzeczenia = element.dataOrzeczenia,
                                DokumentXML = element.dokumentXml,
                                KodDecyzji = element.kodDecyzji,
                                StatusDokumentu = element.statusDokumentu,
                                SygnaturaSprawy = element.sygnaturaSprawy,
                                SygnaturaWgPowoda = element.sygnaturaWgPowoda,
                                
                            };
                            mnModel.NakazOutputElementModels.Add(mnjModel);
                        }

                    }

                }
                else
                {
                    return (int)wynik.kod;
                }

                //blok entity framework
                Utils.LogWriter("Moje Nakazy Próba zapisu");
                using (var db = new EPUDbContext())
                {
                    Utils.LogWriter("Moje Nakazy Próba zapisu " +db.Database.Connection.DataSource );
                     db.MojeNakazyOutputDataModels.Add(mnModel);
                     Utils.LogWriter("Moje Nakazy Próba zapisu, Trying to Save " ); 
                    db.SaveChanges();
                     Utils.LogWriter("Moje Nakazy epUService OK" );
                     return mnModel.Id;
                }


            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd Moje Nakazy" + ex.Message + " " + ((ex.InnerException == null ) ? "":ex.InnerException.ToString()));
                Listabledow += ex.Message + " " + ((ex.InnerException == null) ? "" : ex.InnerException.ToString());
                return -1;
            }
        }
        //Moje Doręczenia
        public int mojeDoreczeniav2(int? rodzajDatyP, DateTime? dataOdP, DateTime? dataDoP, int? kryteriumFiltrowaniaP, string filtrSlownyP, int? statusDoreczeniaP, ref string opisKomunikacjiP)
        {
            try
            {
                int pageSize = 100;

                using (var db = new EPUDbContext())
                {
                var mdModel = new MojeDoreczeniaVer2OutputDataModel()
                {

                    DoreczenieVer2OutputElementModels = new List<DoreczenieVer2OutputElementModel>()
                    //DoreczenieVer2ElementModels = new List<DoreczenieVer2ElementModel>()
                };


                MojeDoreczeniaVer2OutputData wynik = _epuService.MojeDoreczeniaVer2(usrName, usrPass, apiKey, 1, pageSize, rodzajDatyP ,dataOdP, dataDoP, kryteriumFiltrowaniaP, filtrSlownyP, statusDoreczeniaP);
               
                //NakazOutputElementModel mnjModel = null;
                DoreczenieVer2OutputElementModel mdjModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.OK)
                {
                    mdModel.IloscWszystkichElementow = wynik.liczbaWszystkichElementow;
                    mdModel.Informacja = wynik.informacja;
                    mdModel.KodOdpowiedzi = wynik.kod;
                    mdModel.Opis = wynik.opis;
                    

                    foreach (var element in wynik.mojeDoreczenia)
                    {


                        mdjModel = new DoreczenieVer2OutputElementModel
                        {
                            DataDoreczenia = element.dataDoreczenia,
                            DataWyslania = element.dataWyslania,
                            IdDoreczeniaVer2 = element.id,
                            // sprawdz czy takie doreczenie juz jest
                            IdNakazu = element.idNakazu,
                            IdOrzeczenia = element.idOrzeczenia,
                            Opis = element.opis,
                            SygnaturaSprawy = element.sygnaturaSprawy,
                            SygnaturaWgPowoda = element.sygnaturaWgPowoda
                            
                        };
                        mdModel.DoreczenieVer2OutputElementModels.Add(mdjModel);
                    }

                    int iloscStron = wynik.iloscStron;
                    for (int i = 2; i <= iloscStron; i++)
                    {
                        System.Threading.Thread.Sleep(1000);

                        wynik = _epuService.MojeDoreczeniaVer2(usrName, usrPass, apiKey, i, pageSize, rodzajDatyP, dataOdP, dataDoP, kryteriumFiltrowaniaP, filtrSlownyP, statusDoreczeniaP);

                        foreach (var element in wynik.mojeDoreczenia)
                        {
                            mdjModel = new DoreczenieVer2OutputElementModel
                            {
                                DataDoreczenia = element.dataDoreczenia,
                                DataWyslania = element.dataWyslania,
                                IdDoreczeniaVer2 = element.id,
                                IdNakazu = element.idNakazu,
                                IdOrzeczenia = element.idOrzeczenia,
                                Opis = element.opis,
                                SygnaturaSprawy = element.sygnaturaSprawy,
                                SygnaturaWgPowoda = element.sygnaturaWgPowoda

                            };
                            mdModel.DoreczenieVer2OutputElementModels.Add(mdjModel);
                        }
                    }

                }
                else
                {
                    return (int)wynik.kod;
                }

                //blok entity framework
                
                    db.MojeDoreczeniaVer2OutputDataModels.Add(mdModel);
                    db.SaveChanges();
                    return mdModel.Id;
                }


            }
            catch (FaultException<MojeDoreczeniaVer2OutputData> msex)
            {
                opisKomunikacjiP = "Wyjątek z EPU wynikający z wykonywania wewnętrznej procedury" + msex.Detail.ToString();

                return -3000;
            }
            catch (System.Net.WebException we)
            {
                opisKomunikacjiP = "Błąd w warstwie komunikacyjnej z EPU" + we.ToString();
                return -2000;
            }
            catch (Exception ex)
            {
                opisKomunikacjiP = "Wyjątek w komunikacji z epu lub błędu logicznego w danych wejściowych:" + ex.ToString();
                return -1000;
            }
        }
        public int zlozPozwy(string listaPozwowXmlP, ref string kodWalidacjiPozwuP, ref ZlozPozewOutputDataModel zpDataModel)
        {
            ZlozPozewOutputElementModel zpjModel;
            ZlozPozewOutputDataModel zpModel = new ZlozPozewOutputDataModel()
           
            {
                ZlozPozewOutputElementModels = new List<ZlozPozewOutputElementModel>()
            };
            ZlozPozwyOutputData wynik;
            wynik = null;
            try
            {

                

                wynik = _epuService.ZlozPozwy(usrName, usrPass, apiKey, listaPozwowXmlP);
                zpjModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.Created)
                {
                    zpModel.LacznaWartoscPrzedmiotowSporu = wynik.lacznaWartoscPrzedmiotowSporu;
                    zpModel.LiczbaPozwow = wynik.liczbaPozow;
                    zpModel.OznaczeniaPaczki = wynik.oznaczeniePaczki;
                    zpModel.SumaoplatySadowej = wynik.sumaOplatySadawej;
                    zpModel.LacznaWartoscPrzedmiotowSporu = wynik.lacznaWartoscPrzedmiotowSporu;

                    foreach (var element in wynik.wynikiWalidacji)
                    {
                        zpjModel = new ZlozPozewOutputElementModel
                        {
                            KodWalidacjiPozwu = element.kodWalidacjiPozwu,
                            OplataSadowa = element.oplataSadowa,
                            SygnaturaWgPowoda = element.sygnaturaWgPowoda,
                            WartoscPrzedmiotuSporu = element.wartoscPrzedniotuSporu,
                            InformacjaWalidacji = element.informacjaWalidacji,
                            KodWalidacji = element.kodWalidacji,
                            LiczbaPorzadkowa = element.liczbaPorzadkowa,
                            OpisWalidacji = element.opisWalidacji,
                            Status = element.status
                        };
                        zpModel.ZlozPozewOutputElementModels.Add(zpjModel);
                    }

                    using (var db = new EPUDbContext())
                    {
                        db.ZlozPozewOutputDataModels.Add(zpModel);
                        int liczbawierszy = db.SaveChanges();
                        Console.WriteLine(zpModel.Id.ToString());
                        zpDataModel = zpModel;
                    }

                }
                else
                {
                    return (int)wynik.kod;
                }

                //blok entity framework
                /*
                using (var db = new EPUDbContext())
                {

                    db.ZlozPozewOutputDataModels.Add(zpModel);
                    int liczbawierszy = db.SaveChanges();
                    Console.WriteLine(zpModel.Id.ToString());
                    return liczbawierszy;
                }*/
                return 1;


            }
            catch (FaultException<ZlozPozwyOutputData> ex)
            {
                Console.WriteLine(ex.Detail.informacja);
                zpModel.LacznaWartoscPrzedmiotowSporu = ex.Detail.lacznaWartoscPrzedmiotowSporu;
                zpModel.LiczbaPozwow = ex.Detail.liczbaPozow;
                zpModel.OznaczeniaPaczki = ex.Detail.oznaczeniePaczki;
                zpModel.SumaoplatySadowej = ex.Detail.sumaOplatySadawej;
                foreach (ZlozPozwyOutputElement element in ex.Detail.wynikiWalidacji)
                {
                    opis_zadania = element.informacjaWalidacji;
                    kodWalidacjiPozwuP = kodWalidacjiPozwuP + "Nr pozwu w paczce :" + element.liczbaPorzadkowa.ToString() + ":" + element.kodWalidacjiPozwu.ToString() + ";" + (element.opisWalidacji != null ? element.opisWalidacji.ToString() :"");
                    zpjModel = new ZlozPozewOutputElementModel
                        {
                            KodWalidacjiPozwu = element.kodWalidacjiPozwu,
                            OplataSadowa = element.oplataSadowa,
                            SygnaturaWgPowoda = element.sygnaturaWgPowoda,
                            WartoscPrzedmiotuSporu = element.wartoscPrzedniotuSporu,
                            InformacjaWalidacji = element.informacjaWalidacji,
                            KodWalidacji = element.kodWalidacji,
                            LiczbaPorzadkowa = element.liczbaPorzadkowa,
                            OpisWalidacji = element.opisWalidacji,
                            Status = element.status
                        };
                        zpModel.ZlozPozewOutputElementModels.Add(zpjModel);
                    

                }

                try
                {
                    using (var db = new EPUDbContext())
                    {
                        db.ZlozPozewOutputDataModels.Add(zpModel);
                        int liczbawierszy = db.SaveChanges();
                        Console.WriteLine(zpModel.Id.ToString());
                        zpDataModel = zpModel;
                    }
                }
                catch (Exception exe)
                {
                    Utils.LogWriter("Error: " + exe.Message);
                    
                }
                return -1;
            }
            catch (Exception ex)
            {
                zpModel.LacznaWartoscPrzedmiotowSporu = 120;
                zpModel.LiczbaPozwow = 8;
                zpModel.OznaczeniaPaczki = "EOP1/2/3";
                zpModel.SumaoplatySadowej = 12;
                zpModel.LacznaWartoscPrzedmiotowSporu = 2342;

                    zpjModel = new ZlozPozewOutputElementModel
                    {
                        KodWalidacjiPozwu = 1,
                        OplataSadowa =20,
                        SygnaturaWgPowoda = "11234",
                        WartoscPrzedmiotuSporu = 123,
                        InformacjaWalidacji = "aaaa",
                        KodWalidacji = KodWalidacji.OK,
                        LiczbaPorzadkowa = 1,
                        OpisWalidacji = "OK",
                        Status = true
                    };
                    zpModel.ZlozPozewOutputElementModels.Add(zpjModel);
                

                using (var db = new EPUDbContext())
                {
                    db.ZlozPozewOutputDataModels.Add(zpModel);
                    int liczbawierszy = db.SaveChanges();
                    Console.WriteLine(zpModel.Id.ToString());
                    zpDataModel = zpModel;
                }



                kodWalidacjiPozwuP = ex.ToString();
                return -2;
            }
        }
        public int zlozZazalenia(string listaZazalenXMLp) {
            try
            {

                var zzModel = new ZlozZazalenieOutputDataModel()
                {
                  
                    ZlozZazalenieOutputElementModels = new List<ZlozZazalenieOutputElementModel>()
                };

                ZlozZazaleniaOutputData wynik = _epuService.ZlozZazalenia(usrName, usrPass, apiKey, listaZazalenXMLp);
                ZlozZazalenieOutputElementModel zzjModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.Created)
                {
                    zzModel.OznaczeniaPaczki = wynik.oznaczeniePaczki;
                    
                    foreach (var element in wynik.wynikiWalidacji)
                    {
                        zzjModel = new ZlozZazalenieOutputElementModel
                        {
                            
                            InformacjaWalidacji = element.informacjaWalidacji,
                            KodWalidacji = element.kodWalidacji,
                            LiczbaPorzadkowa = element.liczbaPorzadkowa,
                            OpisWalidacji = element.opisWalidacji,
                            Status = element.status
                        };
                        zzModel.ZlozZazalenieOutputElementModels.Add(zzjModel);
                    }



                }
                else
                {
                    return -(int)wynik.kod;
                }

                //blok entity framework
        
                using (var db = new EPUDbContext())
                {
                    db.ZlozZazalenieOutputDataModels.Add(zzModel);
                    int liczbawierszy = db.SaveChanges();
                    Console.WriteLine(zzModel.Id.ToString());
                    return zzModel.Id;
                }
                

            }
            catch (FaultException<ZlozZazaleniaOutputData> ex)
            {
                Console.WriteLine(ex.Detail.informacja);
                foreach (ZlozZazaleniaOutputElement element in ex.Detail.wynikiWalidacji)
                {
                    int i = 1;
                }
                return -1;
            }
        }
        public int zlozSkargi(string listaSkargXMLp)
        {
            try
            {

                var zsModel = new ZlozSkargiOutputDataModel()
                {

                    ZlozSkargiOutputElementModels = new List<ZlozSkargiOutputElementModel>()
                };

                ZlozSkargiOutputData wynik = _epuService.ZlozSkargi(usrName, usrPass, apiKey, listaSkargXMLp);
                ZlozSkargiOutputElementModel zsjModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.Created)
                {
                    zsModel.OznaczeniaPaczki = wynik.oznaczeniePaczki;

                    foreach (var element in wynik.wynikiWalidacji)
                    {
                        zsjModel = new ZlozSkargiOutputElementModel
                        {

                            InformacjaWalidacji = element.informacjaWalidacji,
                            KodWalidacji = element.kodWalidacji,
                            LiczbaPorzadkowa = element.liczbaPorzadkowa,
                            OpisWalidacji = element.opisWalidacji,
                            Status = element.status
                        };
                        zsModel.ZlozSkargiOutputElementModels.Add(zsjModel);
                    }



                }
                else
                {
                    return -(int)wynik.kod;
                }

                //blok entity framework
                using (var db = new EPUDbContext())
                {
                    db.ZlozSkargiOutputDataModels.Add(zsModel);
                    int liczbawierszy = db.SaveChanges();
                    Console.WriteLine(zsModel.Id.ToString());
                    return zsModel.Id;
                }


            }
            catch (FaultException<ZlozSkargiOutputData> ex)
            {
                Console.WriteLine(ex.Detail.informacja);
                foreach (ZlozSkargiOutputElement element in ex.Detail.wynikiWalidacji)
                {
                    int i = 1;
                }
                return -1;
            }
        }
        public int zlozSprzeciw(string listaSprzeciwXMLp) 
        {
            try
            {

                var zspModel = new ZlozSprzeciwOutputDataModel()
                {

                    ZlozSprzeciwOutputElementModels = new List<ZlozSprzeciwOutputElementModel>()
                };

                ZlozSprzeciwyOutputData wynik = _epuService.ZlozSprzeciwy(usrName, usrPass, apiKey, listaSprzeciwXMLp);
                ZlozSprzeciwOutputElementModel zspjModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.Created)
                {
                    zspModel.OznaczeniaPaczki = wynik.oznaczeniePaczki;

                    foreach (var element in wynik.wynikiWalidacji)
                    {
                        zspjModel = new ZlozSprzeciwOutputElementModel
                        {

                            InformacjaWalidacji = element.informacjaWalidacji,
                            KodWalidacji = element.kodWalidacji,
                            LiczbaPorzadkowa = element.liczbaPorzadkowa,
                            OpisWalidacji = element.opisWalidacji,
                            Status = element.status
                        };
                        zspModel.ZlozSprzeciwOutputElementModels.Add(zspjModel);
                    }



                }
                else
                {
                    return -(int)wynik.kod;
                }

                //blok entity framework
                using (var db = new EPUDbContext())
                {
                    db.ZlozSprzeciwOutputDataModels.Add(zspModel);
                    int liczbawierszy = db.SaveChanges();
                    Console.WriteLine(zspModel.Id.ToString());
                    return zspModel.Id;
                }


            }
            catch (FaultException<ZlozSprzeciwyOutputData> ex)
            {
                Console.WriteLine(ex.Detail.informacja);
                foreach (ZlozSprzeciwyOutputElement element in ex.Detail.wynikiWalidacji)
                {
                    int i = 1;
                }
                return -1;
            }
        }
        //zlozWnioski z 31.03.2012
          public int zlozWnioski(string listaWnioskowXMLp,  ref string kodWalidacjiWnioskuP,ref ZlozWnioskiOutputDataModel zwDataModel)
        {
              var zwModel = new ZlozWnioskiOutputDataModel()
                {

                    ZlozWnioskiOutputElementModels = new List<ZlozWnioskiOutputElementModel>()
                };
            try
            {

                

                ZlozWnioskiEgzekucyjneOutputData wynik = _epuService.ZlozWnioskiEgzekucyjne(usrName, usrPass, apiKey, listaWnioskowXMLp);
                ZlozWnioskiOutputElementModel zwjModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.Created)
                {
                    zwModel.OznaczeniaPaczki = wynik.oznaczeniePaczki;

                    foreach (var element in wynik.wynikiWalidacji)
                    {
                        zwjModel = new ZlozWnioskiOutputElementModel
                        {

                            InformacjaWalidacji = element.informacjaWalidacji,
                            KodWalidacji = element.kodWalidacji,
                            LiczbaPorzadkowa = element.liczbaPorzadkowa,
                            OpisWalidacji = element.opisWalidacji,
                            Status = element.status
                        };
                        zwModel.ZlozWnioskiOutputElementModels.Add(zwjModel);
                    }



                }
                else
                {
                    return -(int)wynik.kod;
                }

                //blok entity framework
                using (var db = new EPUDbContext())
                {
                    db.ZlozWnioskiOutputDataModels.Add(zwModel);
                    int liczbawierszy = db.SaveChanges();
                    Console.WriteLine(zwModel.Id.ToString());
                    zwDataModel = zwModel;
                    return zwModel.Id;
                }


            }
            catch (FaultException<ZlozWnioskiEgzekucyjneOutputData> ex)
            {
                Console.WriteLine(ex.Detail.informacja);
                foreach (ZlozWnioskiEgzekucyjneOutputElement element in ex.Detail.wynikiWalidacji)
                {
                    kodWalidacjiWnioskuP = kodWalidacjiWnioskuP + "Numer wniosku w paczce:" + element.liczbaPorzadkowa.ToString() + ":" + element.kodWalidacji.ToString() + ";";
                    zwDataModel = zwModel;
                }
                return -1;
            }
        }
        public int zlozDokumenty(string listaDokumentowXMLp, ref string kodWalidacjiDokumentuP,ref ZlozDokumentyOutputDataModel zwDataModel)
        {
            var zdModel = new ZlozDokumentyOutputDataModel()
            {

                ZlozDokumentyOutputElementModels = new List<ZlozDokumentyOutputElementModel>()
            };
            try
            {

               

                ZlozDokumentyOutputData wynik = _epuService.ZlozDokumenty(usrName, usrPass, apiKey, listaDokumentowXMLp);
                ZlozDokumentyOutputElementModel zdjModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.Created)
                {
                    zdModel.OznaczeniaPaczki = wynik.oznaczeniePaczki;

                    foreach (var element in wynik.wynikiWalidacji)
                    {
                        zdjModel = new ZlozDokumentyOutputElementModel
                        {

                            InformacjaWalidacji = element.informacjaWalidacji,
                            KodWalidacji = element.kodWalidacji,
                            LiczbaPorzadkowa = element.liczbaPorzadkowa,
                            OpisWalidacji = element.opisWalidacji,
                            Status = element.status
                        };
                        zdModel.ZlozDokumentyOutputElementModels.Add(zdjModel);
                    }



                }
                else
                {
                    return -(int)wynik.kod;
                }

                //blok entity framework
                using (var db = new EPUDbContext())
                {
                    db.ZlozDokumentyOutputDataModels.Add(zdModel);
                    int liczbawierszy = db.SaveChanges();
                    Console.WriteLine(zdModel.Id.ToString());
                    zwDataModel = zdModel;
                    return zdModel.Id;
                }


            }
            catch (FaultException<ZlozDokumentyOutputData> ex)
            {
                Console.WriteLine(ex.Detail.informacja);
                foreach (ZlozDokumentyOutputElement element in ex.Detail.wynikiWalidacji)
                {
                    kodWalidacjiDokumentuP = kodWalidacjiDokumentuP + " *Numer dokumentu w paczce:" + element.liczbaPorzadkowa.ToString() + ":" + element.kodWalidacji.ToString() + ":" + element.informacjaWalidacji + "#";
                    zwDataModel = zdModel;
                }
                return -1;
            }
        }
        //moje Pisma
        public int mojePisma(DateTime? dataOdP, DateTime? dataDoP, int? kryteriumFiltrowaniaP, string filtrSlownyP, ref string opisKomunikacjiP)
        {
            try
            {
                int pageSize = 100;
                var mpModel = new MojePismaOutputDataModel
                {
                    DokumentOutputElementModels = new List<DokumentOutputElementModel>()
                };

                MojePismaOutputData wynik = _epuService.MojePisma(usrName, usrPass, apiKey, 1, pageSize, dataOdP, dataDoP, kryteriumFiltrowaniaP, filtrSlownyP);
                DokumentOutputElementModel mppModel = null;
                //Przepisanie wartości z odpowiedzi do modelu
                if (wynik.kod == KodOdpowiedzi.OK)
                {
                    mpModel.IloscWszystkichElementow = wynik.liczbaWszystkichElementow;
                    mpModel.Informacja = wynik.informacja;
                    mpModel.KodOdpowiedzi = wynik.kod;
                    mpModel.Opis = wynik.opis;

                    if (wynik.listaPism.Count() > 0)
                    {
                        foreach (var element in wynik.listaPism)
                        {
                            mppModel = new DokumentOutputElementModel
                            {

                                DataDokumentu = element.dataDokumentu,
                                DataZakreslenia = element.dataZakreslenia,
                                IdDokumentu = element.id,
                                RodzajDokumentu = element.rodzajDokumentu,
                                StatusDokumentu = element.statusDokumentu,
                                SygnaturaSprawy = element.sygnaturaSprawy,
                                SygnaturaWgPowoda = element.sygnaturaWgPowoda

                            };
                            mpModel.DokumentOutputElementModels.Add(mppModel);
                        }
                    }
                    int iloscStron = wynik.iloscStron;
                    for (int i = 2; i <= iloscStron; i++)
                    {


                        wynik = _epuService.MojePisma(usrName, usrPass, apiKey, i, pageSize, dataOdP, dataDoP, kryteriumFiltrowaniaP, filtrSlownyP);
                        if (wynik.listaPism.Count() > 0)
                        {
                            foreach (var element in wynik.listaPism)
                            {
                                mppModel = new DokumentOutputElementModel
                                {

                                    DataDokumentu = element.dataDokumentu,
                                    DataZakreslenia = element.dataZakreslenia,
                                    IdDokumentu = element.id,
                                    RodzajDokumentu = element.rodzajDokumentu,
                                    StatusDokumentu = element.statusDokumentu,
                                    SygnaturaSprawy = element.sygnaturaSprawy,
                                    SygnaturaWgPowoda = element.sygnaturaWgPowoda

                                };
                                mpModel.DokumentOutputElementModels.Add(mppModel);
                            }
                        }

                    }

                }
                else
                {
                    return (int)wynik.kod;
                }

                //blok entity framework
                using (var db = new EPUDbContext())
                {
                    db.MojePismaOutputDataModels.Add(mpModel);
                    db.SaveChanges();
                    return mpModel.Id;
                }


            }
            catch (FaultException<MojePismaOutputData> msex)
            {
                opisKomunikacjiP = "Wyjątek z EPU wynikający z wykonywania procedurey" + msex.Detail.ToString();

                return -3000;
            }
            catch (System.Net.WebException we)
            {
                opisKomunikacjiP = "Błąd w warstwie komunikacyjnej z EPU" + we.ToString();
                return -2000;
            }
            catch (Exception ex)
            {
                opisKomunikacjiP = "Wyjątek w komunikacji z epu lub błędu logicznego w danych wejściowych:" + ex.ToString();
                return -1000;
            }
        }


        public int listaKancelariiKomorniczych( ref string opisKomunikacjiP, ref List<ListaKancelariiKomoniczychOutputElement> outlst)
        {
           
                int pageSize = 100;
                List<ListaKancelariiKomoniczychOutputElement> listaKancelarii;
                ListaKancelariiKomoniczychOutputData wynik = _epuService.ListaKancelariiKomoniczych(usrName, usrPass, apiKey, 1, pageSize);
                try
                {
                 listaKancelarii = new List<ListaKancelariiKomoniczychOutputElement>();
                // jesli wynik zwraca OK
                 
                 if (wynik.kod == KodOdpowiedzi.OK)
                    {
                    foreach (var element in wynik.listaKancelariiKomorniczych)
                        {
                        listaKancelarii.Add(element);
                        }
                        
                    int iloscStron = wynik.iloscStron;
                    for (int i = 2; i <= iloscStron; i++)
                    {
                         wynik = _epuService.ListaKancelariiKomoniczych(usrName, usrPass, apiKey, i, pageSize);
                        foreach (var element in wynik.listaKancelariiKomorniczych)
                            {
                            listaKancelarii.Add(element);
                            }
                    }
                    }
                    }
                catch (FaultException<ListaKancelariiKomoniczychOutputData> msex)
                {
                   opisKomunikacjiP = "Wyjątek z EPU wynikający z wykonywania procedurey" + msex.Detail.ToString();
                   return -3000;
                 }
                catch (System.Net.WebException we)
                  {
                    opisKomunikacjiP = "Błąd w warstwie komunikacyjnej z EPU" + we.ToString();
                    return -2000;
                    }
                catch (Exception ex)
                    {
                    opisKomunikacjiP = "Wyjątek w komunikacji z epu lub błędu logicznego w danych wejściowych:" + ex.ToString();
                    return -1000;
                    }
                outlst = listaKancelarii;
                return wynik.iloscStron;
        }
         


        public int DodajZadanie(RodzajMetody rodzajMetodyP, TerminWykonania terminWykonaniaP) 
        {
            try
            {
                Zadanie noweZadanie = new Zadanie();
                noweZadanie.RodzajZadania = rodzajMetodyP;
                noweZadanie.Wykonac = terminWykonaniaP;
                noweZadanie.Staus = 0; //Nowe zadanie 
                using (var db = new EPUDbContext())
                {
                    db.Zadanies.Add(noweZadanie);

                    db.SaveChanges();
                    return noweZadanie.Id;
                }
            }
            catch
            {
                return -1;
            }
        }
        
    }
}
