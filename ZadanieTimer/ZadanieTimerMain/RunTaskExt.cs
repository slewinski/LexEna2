using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using EpuProxy;
using System.Xml;
using EpuProxy.EpuSrv;
using ZadanieTimerMain;


namespace ZadanieTimer
{
    public partial class  RunTask
    {
        private static int MojePisma(string zadanieParametryp, int IdZadaniaP, out string statusZadaniap)
        {
            string listaBledowMojePisma= "";
           
            try
            {
                int IdSprawy = 0;
                Ustawienia ustawienia = new Ustawienia();
                XmlSerializer mySerializerMojePisma = new XmlSerializer(typeof(EPUParamModel));
                EPUParamModel epuParamModelMojePisma = (EPUParamModel)mySerializerMojePisma.Deserialize(new StringReader(zadanieParametryp));
                EpuProxy.EpuProxyClient klientPisma = new EpuProxy.EpuProxyClient(ustawienia.serwisProdukcyjny);
                KontoEPU kontoEpuPozwy = null;
                using (var baza = new LexEnaZadanieEntities())
                {
                    kontoEpuPozwy = (from z in baza.KontoEPU
                                     where z.Id.Equals(epuParamModelMojePisma.KontoEpuId) //UWAGA nalez zmienić na parametr z procedury
                                     select z).FirstOrDefault();

                }

                //klientPozwy.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
                klientPisma.setUserData(kontoEpuPozwy.LoginEPU, DecodeFrom64(kontoEpuPozwy.EPUPasswd), kontoEpuPozwy.APIKey);
                int wynikPisma = klientPisma.mojePisma(epuParamModelMojePisma.DataOd, epuParamModelMojePisma.DataDo, epuParamModelMojePisma.KryteriumFiltrowania, epuParamModelMojePisma.FiltrSlowny,  ref listaBledowMojePisma);
                if (wynikPisma > 0)
                {
                    using (var aktualizacajaBazy = new LexEnaZadanieEntities())
                    {
                        var query = from z in aktualizacajaBazy.DokumentOutputElementModels
                                    where z.MojePismaOutputDataModels_Id == wynikPisma

                                    select z;

                        if (query.Count() > 0)
                        {
                            foreach (var doreczeniaZEpU in query)
                            {
                                if (IsInteger(doreczeniaZEpU.SygnaturaWgPowoda.Substring(doreczeniaZEpU.SygnaturaWgPowoda.IndexOf("@") + 1)))
                                {
                                
                                }
                            }
                        }
                        //zapisanie wyników do tablicy SprawaKomunikacjaZEpu
                        // dodać zadanie - paczka dtsc ..
                        aktualizacajaBazy.SaveChanges();
                    }
                    Utils.LogWriter("OK" + listaBledowMojePisma);
                    statusZadaniap = "OK" + listaBledowMojePisma;
                    return 200;
                }
                else
                {
                    Utils.LogWriter("Błąd w trakcie pobierania listy nakazów :" + listaBledowMojePisma);
                    statusZadaniap = "Błąd w trakcie pobierania listy nakazów " + listaBledowMojePisma;
                    return wynikPisma;
                }
            }
            catch (Exception ex)
            {

                Utils.LogWriter("Wyjątek w trakcie wykonywania zadania :" + listaBledowMojePisma + ex.ToString());
                statusZadaniap = "Wyjątek w trakcie wykonuwania zadania:" + listaBledowMojePisma + ex.ToString();
                return -110;
            }

        }
        private static int ZlozWnioki(string zadanieParametryp, int IdZadaniaP, out string statusZadaniap)
        {
            List<DokWys> listaWnioskow = new List<DokWys>();
           // List<PozewRozbudowany> listaPozwow = new List<PozewRozbudowany>();//do zmiany niestety nie ma tabeli wniosek!!
            string listaBledowZEPU="";
            bool blad=false; 
            string listawnoskow="";
            Ustawienia ustawienia = new Ustawienia();
            XmlSerializer mySerializer = new XmlSerializer(typeof(EPUParamModel));
            EPUParamModel epuParamModel = (EPUParamModel)mySerializer.Deserialize(new StringReader(zadanieParametryp));
            StringBuilder paczka = new StringBuilder();
            string nazwaPaczki = "";
            DokumentWysKomunikacjaEPU dokwyskom;
           // ZlozPozewOutputDataModel zpDataModel;
            ZlozWnioskiOutputDataModel zwDataModel;
            bool iserror;

            zwDataModel = new ZlozWnioskiOutputDataModel();

            using (var dataPaczka = new LexEnaZadanieEntities())
            {
                //nie wiem czy do wniosku warto dodawać oddzielne pole czy może wykorzystamy IdPozwuWLexEna
                Paczka nazwaPaczkiData = (from z in dataPaczka.Paczka
                                          where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                          select z).FirstOrDefault();
                nazwaPaczki = nazwaPaczkiData.Oznaczenie;
            }
            paczka.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            paczka.Append("<curr:WnioskiEgzekucyjneEPU OznaczeniePaczki=\"");
            paczka.Append(nazwaPaczki);
            paczka.Append(" \" xmlns:curr=\"http://www.currenda.pl/epu\">");

            if (true)//warunek historyczny
            {
                using (var poze = new LexEnaZadanieEntities())
                {    // poprawiłem  warunki dla dokumentu w paczce
                    var zadania = from d in poze.DokWys 
                                  join dp in poze.DokumentPaczka on d.Id equals dp.DokWys_Id
                                  where dp.Paczka_Id == epuParamModel.IdPozwuWLexEna && d.StatusDok != 6 && d.StatusDok != 1 && dp.czyus == 0
                                  select new {  d };

                    /*
                    from p in poze.Pozew 
                              join d in poze.DokWys 
                              on p.DokWys_Id equals d.Id where d.IdPaczki == epuParamModel.IdPozwuWLexEna
                              select p;
                     */
                    // Sprawdzenie pozwów

                    DokWys dokumentDoWysylki;
                    WalidacjaWstepnaXSD walid = null;
                    foreach (var pojZadanie in zadania)
                    {
                        walid = new WalidacjaWstepnaXSD(pojZadanie.d.Tresc,1 );
                        dokumentDoWysylki = pojZadanie.d;
                        if (!walid.Poprawny)
                        {
                            
                            Console.WriteLine(pojZadanie.d.Id.ToString() + "jest niepoprawny");
                            dokumentDoWysylki.Opis = "Błąd Walidacji  XSD";
                            //  walid.LogWalidacji;
                            dokumentDoWysylki.StatusDok = 5;

                            blad = true;
                        }
                        listaWnioskow.Add(dokumentDoWysylki);
                    }

                    XmlDocument wnioskiXML = new XmlDocument();
                    foreach (var pojZadanie in zadania)
                    {
                        if (pojZadanie.d.Tresc != null)
                        {
                            wnioskiXML.Load(new StringReader(pojZadanie.d.Tresc));
                            foreach (XmlNode node in wnioskiXML)
                                if (node.NodeType == XmlNodeType.XmlDeclaration)
                                {
                                    wnioskiXML.RemoveChild(node);
                                }
                            string test = wnioskiXML.InnerXml.ToString();
                            paczka.AppendLine(test);
                        }
                    }
                    paczka.AppendLine("</curr:WnioskiEgzekucyjneEPU>");
                    /* 
                     var zadania = from p in poze.Pozew 
                                  join d in poze.DokKomunikacja on p.DokWys_Id equals d.Id where d.Paczka_id==1
                                  select p;

                     */
                }
                /****************************************** Ustawić statusy dokumentów i paczki **************/
                if (blad)
                {
                    using (var aktualizacja = new LexEnaZadanieEntities())
                    {
                        foreach (var wniosek in listaWnioskow)
                        {

                            {
                                DokWys pojDokWysPrzej = (from z in aktualizacja.DokWys
                                                         where z.Id.Equals((int)wniosek.Id)
                                                         select z).FirstOrDefault();
                                Paczka aktualnaPaczka = (from z in aktualizacja.Paczka
                                                         where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                                         select z).FirstOrDefault();


                                aktualnaPaczka.DataWyslania = DateTime.Now;
                                aktualnaPaczka.StatusPaczki = 4;
                                pojDokWysPrzej.StatusDok = (int)wniosek.StatusDok;
                                dokwyskom = new DokumentWysKomunikacjaEPU();
                                dokwyskom.czyus = 0;
                                dokwyskom.d_kreacji = DateTime.Now;
                                dokwyskom.DokWys_Id = wniosek.Id;
                                if (wniosek.StatusDok == 5)
                                {
                                    dokwyskom.Status = wniosek.StatusDok;  // błąd walidacji/
                                    pojDokWysPrzej.StatusDok = 5;
                                }
                                else
                                {
                                    dokwyskom.Status = 4; // zwrócony w błędnej paczce
                                    pojDokWysPrzej.StatusDok = 4;
                                }
                                aktualizacja.AddToDokumentWysKomunikacjaEPU(dokwyskom);

                            }

                        }
                        aktualizacja.SaveChanges();
                        statusZadaniap = "błąd w trakcie wew. walidacji";
                    }
                    return -100;
                }
                else
                {
                    EpuProxy.EpuProxyClient klient = new EpuProxy.EpuProxyClient(ustawienia.serwisProdukcyjny);
                    KontoEPU kontoEpu = null;
                    using (var baza = new LexEnaZadanieEntities())
                    {
                        kontoEpu = (from z in baza.KontoEPU
                                    join d in baza.Paczka on z.Id equals d.KontoEPU_Id
                                    where d.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                    select z).FirstOrDefault();

                    }

                    //klient.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
                    klient.setUserData(kontoEpu.LoginEPU, DecodeFrom64(kontoEpu.EPUPasswd), kontoEpu.APIKey);
                    int wynikp = klient.zlozWnioski(paczka.ToString(), ref listaBledowZEPU,ref zwDataModel);
                    //Console.WriteLine("zadanie wykonane");
                    Utils.LogWriter("zadanie wykonane");
                    if (wynikp > 0)
                    {
                        try
                        {
                            using (var aktualizacja = new LexEnaZadanieEntities())
                            {
                                foreach (var wniosek in listaWnioskow)
                                {

                                    DokWys pojDokWysPrzej = (from z in aktualizacja.DokWys
                                                             where z.Id.Equals((int)wniosek.Id)
                                                             select z).FirstOrDefault();
                                    Paczka aktualnaPaczka = (from z in aktualizacja.Paczka
                                                             where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                                             select z).FirstOrDefault();


                                    aktualnaPaczka.DataWyslania = DateTime.Now;
                                    aktualnaPaczka.StatusPaczki = 3;
                                    pojDokWysPrzej.StatusDok = 3;


                                    foreach (var element in zwDataModel.ZlozWnioskiOutputElementModels)
                                    {
                                        //należy zmienić Brak syg powoda w elemencie 
                                        if (true)
                                        {
                                            
                                            dokwyskom = new DokumentWysKomunikacjaEPU();
                                            dokwyskom.czyus = 0;
                                            dokwyskom.d_kreacji = DateTime.Now;
                                            dokwyskom.DokWys_Id = wniosek.Id;
                                            dokwyskom.Status = 3;
                                            dokwyskom.PozewOutputElementModels_Id = element.Id;
                                            aktualizacja.AddToDokumentWysKomunikacjaEPU(dokwyskom);
                                            break;
                                        }
                                    }

                                }

                                PaczkaKomunikacja paczkaKomunikacja = new PaczkaKomunikacja();
                                paczkaKomunikacja.OpisTransmisji = "Brak błędów";
                                paczkaKomunikacja.DataPrzeslania = DateTime.Now;
                                paczkaKomunikacja.Status = 3;
                                paczkaKomunikacja.Paczka_Id = epuParamModel.IdPozwuWLexEna;
                                aktualizacja.AddToPaczkaKomunikacja(paczkaKomunikacja);
                                aktualizacja.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            statusZadaniap = "błąd zapisu wyników";
                            return -200; // błąd zapisu wyników
                        }
                        statusZadaniap = "brak błędów";
                        return 200;


                    }
                    else
                    {


                        using (var aktualizacja = new LexEnaZadanieEntities())
                        {

                            foreach (var wniosek in listaWnioskow)
                            {

                                DokWys pojDokWysPrzej = (from z in aktualizacja.DokWys
                                                         where z.Id.Equals((int)wniosek.Id)
                                                         select z).FirstOrDefault();
                                Paczka aktualnaPaczka = (from z in aktualizacja.Paczka
                                                         where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                                         select z).FirstOrDefault();

                                aktualnaPaczka.DataWyslania = DateTime.Now;
                                aktualnaPaczka.StatusPaczki = 4;

                                iserror = false;
                                foreach (var element in zwDataModel.ZlozWnioskiOutputElementModels)
                                {
                                    //należy zmianieć brak sygnatury powoda w elemencie
                                    if (true)
                                    {
                                        dokwyskom = new DokumentWysKomunikacjaEPU();
                                        dokwyskom.czyus = 0;
                                        dokwyskom.d_kreacji = DateTime.Now;
                                        dokwyskom.DokWys_Id = wniosek.Id;
                                        //Warto w tym miejscu zmienić na WniosekOutput...
                                        dokwyskom.PozewOutputElementModels_Id = element.Id;
                                        if (element.KodWalidacji < 0)
                                        { pojDokWysPrzej.StatusDok = 5; iserror = true; dokwyskom.Status = 5; }   // dokument odrzucony
                                        else
                                            dokwyskom.Status = 4;   // zwrócony
                                        aktualizacja.AddToDokumentWysKomunikacjaEPU(dokwyskom);

                                    }

                                }
                                if (!iserror)
                                    pojDokWysPrzej.StatusDok = 4;
                            }

                            PaczkaKomunikacja paczkaKomunikacja = new PaczkaKomunikacja();
                            if (wynikp == -2) paczkaKomunikacja.OpisTransmisji = "Błąd autoryzacji użytkownika w EPU"; else paczkaKomunikacja.OpisTransmisji = listaBledowZEPU;
                            paczkaKomunikacja.DataPrzeslania = DateTime.Now;
                            paczkaKomunikacja.Status = 4;
                            paczkaKomunikacja.Paczka_Id = epuParamModel.IdPozwuWLexEna;
                            aktualizacja.AddToPaczkaKomunikacja(paczkaKomunikacja);
                            aktualizacja.SaveChanges();
                        }
                        statusZadaniap = "Ooops!";
                        Utils.LogWriter("Błąd w paczce" + listaBledowZEPU + " Numer paczki:" + epuParamModel.IdPozwuWLexEna.ToString());
                        return -100;
                    }
                }
            }
        }


        private static int ZlozDokumenty(string zadanieParametryp, int IdZadaniaP, out string statusZadaniap)
        {
            List<DokWys> listaDokumentow = new List<DokWys>();
            // List<PozewRozbudowany> listaPozwow = new List<PozewRozbudowany>();//do zmiany niestety nie ma tabeli wniosek!!
            string listaBledowZEPU = "";
            bool blad = false;
            string listawnoskow = "";
            Ustawienia ustawienia = new Ustawienia();
            XmlSerializer mySerializer = new XmlSerializer(typeof(EPUParamModel));
            EPUParamModel epuParamModel = (EPUParamModel)mySerializer.Deserialize(new StringReader(zadanieParametryp));
            StringBuilder paczka = new StringBuilder();
            string nazwaPaczki = "";
            DokumentWysKomunikacjaEPU dokwyskom;
            // ZlozPozewOutputDataModel zpDataModel;
            ZlozDokumentyOutputDataModel zwDataModel;
            bool iserror;

            zwDataModel = new ZlozDokumentyOutputDataModel();
            
            using (var dataPaczka = new LexEnaZadanieEntities())
            {
                //nie wiem czy do wniosku warto dodawać oddzielne pole czy może wykorzystamy IdPozwuWLexEna
                Paczka nazwaPaczkiData = (from z in dataPaczka.Paczka
                                          where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                          select z).FirstOrDefault();
                nazwaPaczki = nazwaPaczkiData.Oznaczenie;
            }
            paczka.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            paczka.Append("<curr:DokumentyEPU OznaczeniePaczki=\"");
            paczka.Append(nazwaPaczki);
            paczka.Append(" \" xmlns:curr=\"http://www.currenda.pl/epu\">");

            if (true)//warunek historyczny
            {
                using (var poze = new LexEnaZadanieEntities())
                {    // poprawiłem  warunki dla dokumentu w paczce
                    var zadania = from d in poze.DokWys
                                  join dp in poze.DokumentPaczka on d.Id equals dp.DokWys_Id
                                  where dp.Paczka_Id == epuParamModel.IdPozwuWLexEna && d.StatusDok != 6 && d.StatusDok != 1 && dp.czyus == 0
                                  select new { d };

                    /*
                    from p in poze.Pozew 
                              join d in poze.DokWys 
                              on p.DokWys_Id equals d.Id where d.IdPaczki == epuParamModel.IdPozwuWLexEna
                              select p;
                     */
                    // Sprawdzenie pozwów

                    DokWys dokumentDoWysylki;
                    WalidacjaWstepnaXSD walid = null;
                    foreach (var pojZadanie in zadania)
                    {
                        walid = new WalidacjaWstepnaXSD(pojZadanie.d.Tresc,2);
                        dokumentDoWysylki = pojZadanie.d;
                        if (!walid.Poprawny)
                        {

                            Console.WriteLine(pojZadanie.d.Id.ToString() + "jest niepoprawny");
                            dokumentDoWysylki.Opis = "Błąd Walidacji  XSD";
                            //  walid.LogWalidacji;
                            dokumentDoWysylki.StatusDok = 5;

                            blad = true;
                        }
                        listaDokumentow.Add(dokumentDoWysylki);
                    }

                    XmlDocument wnioskiXML = new XmlDocument();
                    foreach (var pojZadanie in zadania)
                    {
                        if (pojZadanie.d.Tresc != null)
                        {
                            wnioskiXML.Load(new StringReader(pojZadanie.d.Tresc));
                            foreach (XmlNode node in wnioskiXML)
                                if (node.NodeType == XmlNodeType.XmlDeclaration)
                                {
                                    wnioskiXML.RemoveChild(node);
                                }
                            string test = wnioskiXML.InnerXml.ToString();
                            paczka.AppendLine(test);
                        }
                    }
                    paczka.AppendLine("</curr:DokumentyEPU>");
                    /* 
                     var zadania = from p in poze.Pozew 
                                  join d in poze.DokKomunikacja on p.DokWys_Id equals d.Id where d.Paczka_id==1
                                  select p;

                     */
                }
                /****************************************** Ustawić statusy dokumentów i paczki **************/
                if (blad)
                {
                    using (var aktualizacja = new LexEnaZadanieEntities())
                    {
                        foreach (var dok in listaDokumentow)
                        {

                            {
                                DokWys pojDokWysPrzej = (from z in aktualizacja.DokWys
                                                         where z.Id.Equals((int)dok.Id)
                                                         select z).FirstOrDefault();
                                Paczka aktualnaPaczka = (from z in aktualizacja.Paczka
                                                         where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                                         select z).FirstOrDefault();


                                aktualnaPaczka.DataWyslania = DateTime.Now;
                                aktualnaPaczka.StatusPaczki = 4;
                                pojDokWysPrzej.StatusDok = (int)dok.StatusDok;
                                dokwyskom = new DokumentWysKomunikacjaEPU();
                                dokwyskom.czyus = 0;
                                dokwyskom.d_kreacji = DateTime.Now;
                                dokwyskom.DokWys_Id = dok.Id;
                                if (dok.StatusDok == 5)
                                {
                                    dokwyskom.Status = dok.StatusDok;  // błąd walidacji/
                                    pojDokWysPrzej.StatusDok = 5;
                                }
                                else
                                {
                                    dokwyskom.Status = 4; // zwrócony w błędnej paczce
                                    pojDokWysPrzej.StatusDok = 4;
                                }
                                aktualizacja.AddToDokumentWysKomunikacjaEPU(dokwyskom);

                            }

                        }
                        aktualizacja.SaveChanges();
                        statusZadaniap = "błąd w trakcie wew. walidacji";
                    }
                    return -100;
                }
                else
                {
                    EpuProxy.EpuProxyClient klient = new EpuProxy.EpuProxyClient(ustawienia.serwisProdukcyjny);
                    KontoEPU kontoEpu = null;
                    using (var baza = new LexEnaZadanieEntities())
                    {
                        kontoEpu = (from z in baza.KontoEPU
                                    join d in baza.Paczka on z.Id equals d.KontoEPU_Id
                                    where d.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                    select z).FirstOrDefault();

                    }

                    //klient.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
                    klient.setUserData(kontoEpu.LoginEPU, DecodeFrom64(kontoEpu.EPUPasswd), kontoEpu.APIKey);
                    int wynikp = klient.zlozDokumenty(paczka.ToString(), ref listaBledowZEPU, ref zwDataModel);
                    //Console.WriteLine("zadanie wykonane");
                    Utils.LogWriter("zadanie wykonane");
                    if (wynikp > 0)
                    {
                        try
                        {
                            using (var aktualizacja = new LexEnaZadanieEntities())
                            {
                                foreach (var wniosek in listaDokumentow)
                                {

                                    DokWys pojDokWysPrzej = (from z in aktualizacja.DokWys
                                                             where z.Id.Equals((int)wniosek.Id)
                                                             select z).FirstOrDefault();
                                    Paczka aktualnaPaczka = (from z in aktualizacja.Paczka
                                                             where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                                             select z).FirstOrDefault();


                                    aktualnaPaczka.DataWyslania = DateTime.Now;
                                    aktualnaPaczka.StatusPaczki = 3;
                                    pojDokWysPrzej.StatusDok = 3;


                                    foreach (var element in zwDataModel.ZlozDokumentyOutputElementModels)
                                    {
                                        //należy zmienić Brak syg powoda w elemencie 
                                        if (true)
                                        {

                                            dokwyskom = new DokumentWysKomunikacjaEPU();
                                            dokwyskom.czyus = 0;
                                            dokwyskom.d_kreacji = DateTime.Now;
                                            dokwyskom.DokWys_Id = wniosek.Id;
                                            dokwyskom.Status = 3;
                                            dokwyskom.PozewOutputElementModels_Id = element.Id;
                                            aktualizacja.AddToDokumentWysKomunikacjaEPU(dokwyskom);
                                            break;
                                        }
                                    }

                                }

                                PaczkaKomunikacja paczkaKomunikacja = new PaczkaKomunikacja();
                                paczkaKomunikacja.OpisTransmisji = "Brak błędów";
                                paczkaKomunikacja.DataPrzeslania = DateTime.Now;
                                paczkaKomunikacja.Status = 3;
                                paczkaKomunikacja.Paczka_Id = epuParamModel.IdPozwuWLexEna;
                                aktualizacja.AddToPaczkaKomunikacja(paczkaKomunikacja);
                                aktualizacja.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            statusZadaniap = "błąd zapisu wyników";
                            return -200; // błąd zapisu wyników
                        }
                        statusZadaniap = "brak błędów";
                        return 200;


                    }
                    else
                    {


                        using (var aktualizacja = new LexEnaZadanieEntities())
                        {

                            foreach (var wniosek in listaDokumentow)
                            {

                                DokWys pojDokWysPrzej = (from z in aktualizacja.DokWys
                                                         where z.Id.Equals((int)wniosek.Id)
                                                         select z).FirstOrDefault();
                                Paczka aktualnaPaczka = (from z in aktualizacja.Paczka
                                                         where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                                         select z).FirstOrDefault();

                                aktualnaPaczka.DataWyslania = DateTime.Now;
                                aktualnaPaczka.StatusPaczki = 4;

                                iserror = false;
                                foreach (var element in zwDataModel.ZlozDokumentyOutputElementModels)
                                {
                                    //należy zmianieć brak sygnatury powoda w elemencie
                                    if (true)
                                    {
                                        dokwyskom = new DokumentWysKomunikacjaEPU();
                                        dokwyskom.czyus = 0;
                                        dokwyskom.d_kreacji = DateTime.Now;
                                        dokwyskom.DokWys_Id = wniosek.Id;
                                        //Warto w tym miejscu zmienić na WniosekOutput...
                                        dokwyskom.PozewOutputElementModels_Id = element.Id;
                                        if (element.KodWalidacji < 0)
                                        { pojDokWysPrzej.StatusDok = 5; iserror = true; dokwyskom.Status = 5; }   // dokument odrzucony
                                        else
                                            dokwyskom.Status = 4;   // zwrócony
                                        aktualizacja.AddToDokumentWysKomunikacjaEPU(dokwyskom);

                                    }

                                }
                                if (!iserror)
                                    pojDokWysPrzej.StatusDok = 4;
                            }

                            PaczkaKomunikacja paczkaKomunikacja = new PaczkaKomunikacja();
                            if (wynikp == -2) paczkaKomunikacja.OpisTransmisji = "Błąd autoryzacji użytkownika w EPU"; else paczkaKomunikacja.OpisTransmisji = listaBledowZEPU;
                            paczkaKomunikacja.DataPrzeslania = DateTime.Now;
                            paczkaKomunikacja.Status = 4;
                            paczkaKomunikacja.Paczka_Id = epuParamModel.IdPozwuWLexEna;
                            aktualizacja.AddToPaczkaKomunikacja(paczkaKomunikacja);
                            aktualizacja.SaveChanges();
                        }
                        statusZadaniap = "Ooops!";
                        Utils.LogWriter("Błąd w paczce" + listaBledowZEPU + " Numer paczki:" + epuParamModel.IdPozwuWLexEna.ToString());
                        return -100;
                    }
                }
            }
        }

        private static int ListaKancelarii(string zadanieParametryp, int IdZadaniaP, out string statusZadaniap)
        {
                  
            
            ListaKancelariiKomoniczychOutputElement kancItem;
            List<ListaKancelariiKomoniczychOutputElement> kancLst;
            string listaBledow= "";
            DateTime dtstart;

            try
            {
               
                Ustawienia ustawienia = new Ustawienia();
                XmlSerializer mySerializer = new XmlSerializer(typeof(EPUParamModel));
                EPUParamModel epuParams = (EPUParamModel)mySerializer.Deserialize(new StringReader(zadanieParametryp));
                EpuProxy.EpuProxyClient WSklient = new EpuProxy.EpuProxyClient(ustawienia.serwisProdukcyjny);
                KontoEPU kontoEpu = null;
                using (var baza = new LexEnaZadanieEntities())
                {
                    kontoEpu = (from z in baza.KontoEPU
                                where z.Id.Equals(epuParams.KontoEpuId) //UWAGA nalez zmienić na parametr z procedury
                                     select z).FirstOrDefault();

                }

                //klientPozwy.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
                WSklient.setUserData(kontoEpu.LoginEPU, DecodeFrom64(kontoEpu.EPUPasswd), kontoEpu.APIKey);
                kancLst = new List<ListaKancelariiKomoniczychOutputElement>();
                int wynik = WSklient.listaKancelariiKomorniczych( ref listaBledow,ref  kancLst) ;
                if (wynik > 0)
                {
                    using (var aktualizacajaBazy = new LexEnaZadanieEntities())
                    {
                        dtstart = DateTime.Now;
                        foreach (var item in kancLst)
                        {

                            kancItem = item as ListaKancelariiKomoniczychOutputElement;
                            var query = from z in aktualizacajaBazy.KancelariaKomornicza
                                        where z.IdEPU == kancItem.id
                                        select z;

                            if (query.Count() > 0)  // jest taka kancelaria
                            {
                                if (query.FirstOrDefault<KancelariaKomornicza>().Nazwa == kancItem.nazwaKancelarii)
                                { ;}
                                else // update nazwy
                                {
                                    query.FirstOrDefault<KancelariaKomornicza>().Nazwa = kancItem.nazwaKancelarii;
                                    query.FirstOrDefault<KancelariaKomornicza>().DataWprowadzenia = DateTime.Now;
                                    kancItem.id = -1;
                                }
                            }
                            else
                            {
                                aktualizacajaBazy.AddToKancelariaKomornicza(new KancelariaKomornicza { DataWprowadzenia = DateTime.Now, czyus = 0, IdEPU = kancItem.id, Nazwa = kancItem.nazwaKancelarii });
                                kancItem.id = -1;
                            }
                        } // usunięcie nieaktualnych kancelarii;
                        var query1 = from z in aktualizacajaBazy.KancelariaKomornicza
                                    where z.DataWprowadzenia <=  dtstart
                                    select z;
                        bool found;
                        foreach (var item in query1.ToList<KancelariaKomornicza>())
                        {
                            int myId;
                            myId = (item as KancelariaKomornicza).IdEPU;
                            found = false;
                            foreach (var item1 in kancLst)
                            {
                                if ((item1 as ListaKancelariiKomoniczychOutputElement).id == myId)
                                {
                                    found = true;
                                    break;
                                }
                            
                            }
                            if (!found)
                            {
                                var query2 = from z in aktualizacajaBazy.KancelariaKomornicza
                                         where z.IdEPU == myId
                                         select z;
                                if (query2.Count() > 0)
                                {
                                    query2.FirstOrDefault<KancelariaKomornicza>().czyus = 1;
                                
                                }
                            
                            }

                        }   
                            aktualizacajaBazy.SaveChanges();
                    }
                    Utils.LogWriter("OK" + listaBledow);
                    statusZadaniap = "OK" + listaBledow;
                    return 200;
                }
                else
                {
                    Utils.LogWriter("Błąd w trakcie pobierania listy nakazów :" + listaBledow);
                    statusZadaniap = "Błąd w trakcie pobierania listy nakazów " + listaBledow;
                    return wynik;
                }
            }
            catch (Exception ex)
            {

                Utils.LogWriter("Wyjątek w trakcie wykonywania zadania :" + listaBledow + ex.ToString());
                statusZadaniap = "Wyjątek w trakcie wykonuwania zadania:" + listaBledow + ex.ToString();
                return -110;
            }

        }

                }
            }
       