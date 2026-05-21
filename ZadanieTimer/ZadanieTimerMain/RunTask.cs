using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using EpuProxy;
using ZadanieTimer;
using System.Text.RegularExpressions;
using ZadanieTimerMain;
using System.Web.Hosting;

namespace ZadanieTimer
{
    

    public partial class RunTask
    {
        bool walidacja = true;
        string log_walidacji = "";



        public static int WykonajZadanie(string nazwaZadaniap, int typZadaniap, ref string statusZadaniap, string zadanieParametryp, int IdZadaniaP, ref int IdModel)
        {

            IdModel = -1;
            Ustawienia ustawienia = new Ustawienia();


            List<PozewRozbudowany> listaPozwow = new List<PozewRozbudowany>();
            string listaBledowZEPU = null;
            bool blad = false;

            byte[] pdffile;
            string retvalue;

            using (var context = new LexEnaZadanieEntities())
            { 
            switch (typZadaniap)
            {
                /*
                case 1:
                     try
                     {
                         if (File.Exists(nazwaZadaniap + ".dtsx"))
                         {

                             Application app = new Application();
                             Package package = app.LoadPackage(nazwaZadaniap + ".dtsx", null);
                             if (File.Exists(nazwaZadaniap + ".dtsConfig")) package.ImportConfigurationFile(nazwaZadaniap + ".dtsConfig");
                             DTSExecResult result = package.Execute();
                             statusZadaniap = result.ToString();
                             if (result.ToString() == DTSExecResult.Success.ToString()) { return 200; } else { return -300; }

                         }
                         else
                         {
                             statusZadaniap = "Brak pliku transformaty";
                             return -100;
                         }
                     }
                     catch (Exception ex)
                     {
                         statusZadaniap = ex.ToString();
                         return -150;
                     }
                 */
                case 2:

                    XmlSerializer mySerializer = new XmlSerializer(typeof(EPUParamModel));
                    EPUParamModel epuParamModel = (EPUParamModel)mySerializer.Deserialize(new StringReader(zadanieParametryp));
                    StringBuilder paczka = new StringBuilder();
                    string nazwaPaczki = "";
                    DokumentWysKomunikacjaEPU dokwyskom;
                    ZlozPozewOutputDataModel zpDataModel;
                    int _IdSprawy = 0;
                    KontoEPU kontoEpu = null;

                    bool iserror;

                    zpDataModel = new ZlozPozewOutputDataModel();


                    Paczka nazwaPaczkiData = (from z in context.Paczka
                                              where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                              select z).FirstOrDefault();
                    nazwaPaczki = nazwaPaczkiData.Oznaczenie;

                    paczka.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    paczka.Append("<curr:Pozwy OznaczeniePaczki=\"");
                    paczka.Append(nazwaPaczki);
                    paczka.Append(" \" xmlns:curr=\"http://www.currenda.pl/epu\">");

                    if (nazwaZadaniap == "ZlozPozew")
                    {
                        // poprawiłem  warunki dla dokumentu w paczce
                        var zadania = from p in context.Pozew
                                      join d in context.DokWys on p.DokWys_Id equals d.Id
                                      join dp in context.DokumentPaczka on d.Id equals dp.DokWys_Id
                                      where dp.Paczka_Id == epuParamModel.IdPozwuWLexEna && d.StatusDok != 6 && d.StatusDok != 1 && dp.czyus == 0
                                      select new { p, d };

                        /*
                        from p in poze.Pozew 
                                  join d in poze.DokWys 
                                  on p.DokWys_Id equals d.Id where d.IdPaczki == epuParamModel.IdPozwuWLexEna
                                  select p;
                         */
                        // Sprawdzenie pozwów


                        WalidacjaWstepnaXSD walid = null;
                        foreach (var pojZadanie in zadania)
                        {
                            PozewRozbudowany pozewRozbudowany = new PozewRozbudowany();
                            pozewRozbudowany.ConvertPozew(pojZadanie.p);
                            pozewRozbudowany.IdSprawy = (int)pojZadanie.d.Sprawa_id;
                            pozewRozbudowany.DokWys_Id = pojZadanie.d.Id;
                            walid = new WalidacjaWstepnaXSD(pojZadanie.p.Tresc, 0);
                            if (!walid.Poprawny)
                            {
                                Console.WriteLine(pojZadanie.p.Id.ToString() + "jest niepoprawny");
                                pozewRozbudowany.Opis = "Błąd Walidacji  XSD";
                                //  walid.LogWalidacji;
                                pozewRozbudowany.Status = 5;

                                blad = true;
                            }
                            listaPozwow.Add(pozewRozbudowany);
                        }

                        XmlDocument pozewXML = new XmlDocument();
                        foreach (var pojZadanie in zadania)
                        {
                            if (pojZadanie.p.Tresc != null)
                            {
                                pozewXML.Load(new StringReader(pojZadanie.p.Tresc));
                                // usuwanie atrybutu 
                                foreach (XmlNode node in pozewXML)
                                    if (node.NodeType == XmlNodeType.XmlDeclaration)
                                    {
                                        pozewXML.RemoveChild(node);
                                    }
                                string test = pozewXML.InnerXml.ToString();
                                paczka.AppendLine(test);
                            }
                        }
                        paczka.AppendLine("</curr:Pozwy>");
                        /* 
                         var zadania = from p in poze.Pozew 
                                      join d in poze.DokKomunikacja on p.DokWys_Id equals d.Id where d.Paczka_id==1
                                      select p;

                         */
                        /*
                       using (StreamWriter outfile = new StreamWriter("OutDoc.xml"))
                       {
                           outfile.Write(paczka);
                       }
                       */

                        /****************************************** Ustawić statusy dokumentów i paczki **************/
                        if (blad)
                        {

                            {
                                foreach (var pozew in listaPozwow)
                                {

                                    {
                                        DokWys pojDokWysPrzej = (from z in context.DokWys
                                                                 where z.Id.Equals((int)pozew.DokWys_Id)
                                                                 select z).FirstOrDefault();
                                        Paczka aktualnaPaczka = (from z in context.Paczka
                                                                 where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                                                 select z).FirstOrDefault();


                                        aktualnaPaczka.DataWyslania = DateTime.Now;
                                        aktualnaPaczka.StatusPaczki = 4;
                                        pojDokWysPrzej.StatusDok = (int)pozew.Status;
                                        dokwyskom = new DokumentWysKomunikacjaEPU();
                                        dokwyskom.czyus = 0;
                                        dokwyskom.d_kreacji = DateTime.Now;
                                        dokwyskom.DokWys_Id = pozew.DokWys_Id;
                                        if (pozew.Status == 5)
                                        {
                                            dokwyskom.Status = pozew.Status;  // błąd walidacji/
                                            pojDokWysPrzej.StatusDok = 5;
                                        }
                                        else
                                        {
                                            dokwyskom.Status = 4; // zwrócony w błędnej paczce
                                            pojDokWysPrzej.StatusDok = 4;
                                        }
                                        context.AddToDokumentWysKomunikacjaEPU(dokwyskom);

                                    }

                                }
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            EpuProxy.EpuProxyClient klient = new EpuProxy.EpuProxyClient(ustawienia.serwisProdukcyjny);

                            using (var baza = new LexEnaZadanieEntities())
                            {
                                kontoEpu = (from z in baza.KontoEPU
                                            join d in baza.Paczka on z.Id equals d.KontoEPU_Id
                                            where d.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                            select z).FirstOrDefault();

                            }

                            //klient.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
                            klient.setUserData(kontoEpu.LoginEPU, DecodeFrom64(kontoEpu.EPUPasswd), kontoEpu.APIKey);
                            int wynikp = klient.zlozPozwy(paczka.ToString(), ref listaBledowZEPU, ref zpDataModel);
                            //Console.WriteLine("zadanie wykonane");
                            Utils.LogWriter("zadanie wykonane");
                            Utils.LogWriter("lista błędów :" + listaBledowZEPU);

                            if (zpDataModel != null)
                                Utils.LogWriter(zpDataModel + " " + zpDataModel.SumaoplatySadowej.ToString() + " " + zpDataModel.LiczbaPozwow.ToString());
                            if (zpDataModel.ZlozPozewOutputElementModels != null)
                            {
                                foreach (ZlozPozewOutputElementModel zx in zpDataModel.ZlozPozewOutputElementModels)
                                {
                                    Utils.LogWriter(zx.LiczbaPorzadkowa.ToString() + " " + zx.SygnaturaWgPowoda + " " + zx.OpisWalidacji + " " + zx.KodWalidacjiPozwu.ToString());


                                }


                            }
                            if (wynikp > 0)
                            {

                                pdffile = new byte[1];
                                try
                                {
                                    using (var aktualizacja = new LexEnaZadanieEntities())
                                    {
                                        Paczka aktualnaPaczka = (from z in aktualizacja.Paczka
                                                                 where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                                                 select z).FirstOrDefault();
                                        foreach (var pozew in listaPozwow)
                                        {

                                            DokWys pojDokWysPrzej = (from z in aktualizacja.DokWys.Include("Sprawa")
                                                                     where z.Id.Equals((int)pozew.DokWys_Id)
                                                                     select z).FirstOrDefault();



                                            aktualnaPaczka.DataWyslania = DateTime.Now;
                                            aktualnaPaczka.StatusPaczki = 3;
                                            pojDokWysPrzej.StatusDok = 3;
                                            pojDokWysPrzej.TrescHtml = XML2HTMLTransform.TransformNCompress(pojDokWysPrzej.Tresc, 0);
                                            retvalue = XML2HTMLTransform.html2pdfSharp(pojDokWysPrzej.TrescHtml, ref pdffile);
                                            if (retvalue.Contains("Błąd"))
                                            {

                                                Utils.LogWriter("Wyjątek w trakcie wykonywania zadania : Złóż pozew, błąd zapisu do .pdf" + retvalue);
                                                return -200;
                                            }
                                            PdfStore mypdf = (from z in aktualizacja.PdfStore
                                                              where z.DokWys_Id == pozew.DokWys_Id
                                                              select z).FirstOrDefault();
                                            if (mypdf == null)
                                            {
                                                mypdf = new PdfStore();
                                                mypdf.DokWys_Id = (int)pozew.DokWys_Id;
                                                mypdf.type = 0;
                                                pojDokWysPrzej.PdfStore.Add(mypdf);
                                            }
                                            mypdf.name = "Pozew " + pojDokWysPrzej.Sprawa.sygnatura;
                                            mypdf.value = pdffile;

                                            pojDokWysPrzej.Sprawa.DataZloPozwu = pojDokWysPrzej.DataDok;
                                            // zmiana statusu 

                                            if (zpDataModel.ZlozPozewOutputElementModels.Count > 0)
                                            {
                                                foreach (var element in zpDataModel.ZlozPozewOutputElementModels)
                                                {
                                                    if (element.SygnaturaWgPowoda == null) continue;
                                                    if (element.SygnaturaWgPowoda.Contains("@"))
                                                    {
                                                        _IdSprawy = Convert.ToInt32(element.SygnaturaWgPowoda.Substring(element.SygnaturaWgPowoda.IndexOf("@") + 1));
                                                    }
                                                    else
                                                    {
                                                        Sprawa _spr = (from z in aktualizacja.Sprawa
                                                                       where z.sygnatura.Equals(element.SygnaturaWgPowoda)
                                                                       select z).FirstOrDefault();
                                                        if (_spr != null)
                                                            _IdSprawy = _spr.id;
                                                        else
                                                            Utils.LogWriter("Błąd sygnatury pozwu:" + element.SygnaturaWgPowoda);
                                                    }
                                                    if (_IdSprawy == pozew.IdSprawy)
                                                    {
                                                        dokwyskom = new DokumentWysKomunikacjaEPU();
                                                        dokwyskom.czyus = 0;
                                                        dokwyskom.d_kreacji = DateTime.Now;
                                                        dokwyskom.DokWys_Id = pozew.DokWys_Id;
                                                        dokwyskom.Status = 3;
                                                        dokwyskom.PozewOutputElementModels_Id = element.Id;
                                                        aktualizacja.AddToDokumentWysKomunikacjaEPU(dokwyskom);
                                                        // złóż zapytanie o status sprawy


                                                        break;
                                                    }
                                                }
                                            }

                                        }

                                        PaczkaKomunikacja paczkaKomunikacja = new PaczkaKomunikacja();
                                        paczkaKomunikacja.OpisTransmisji = "Brak błędów";
                                        paczkaKomunikacja.DataPrzeslania = DateTime.Now;
                                        paczkaKomunikacja.Status = 3;
                                        paczkaKomunikacja.Paczka_Id = epuParamModel.IdPozwuWLexEna;
                                        aktualizacja.AddToPaczkaKomunikacja(paczkaKomunikacja);
                                        /*
                                         ZadanieSet zadanie = new ZadanieSet();
                                         zadanie.DataRozpoczęcia = DateTime.Today.Add(new TimeSpan(1, 7, 0, 0));
                                         zadanie.Oczasie = true;
                                         zadanie.NazwaZadania = EpuZadania.MojeSprawy.ToString();
                                         zadanie.TypZadaniaId = 3;
                                         zadanie.Parametry = SerializeToXML(new EPUParamModel { DataOd = DateTime.Today, DataDo = DateTime.Today.Add(new TimeSpan(2, 0, 0, 0)), KryteriumFiltrowania = null, FiltrSlowny = null, KontoEpuId = Convert.ToInt32(aktualnaPaczka.KontoEPU_Id) });
                                         aktualizacja.AddToZadanieSet(zadanie);
                                        */
                                        aktualizacja.SaveChanges();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Utils.LogWriter("Błąd zapisu wyników" + ex.ToString());
                                    return -200; // błąd zapisu wyników
                                }
                                return 200;


                            }
                            else
                            {
                                Utils.LogWriter("Błąd podczas składania pozwów ");

                                {


                                    foreach (var pozew in listaPozwow)
                                    {




                                        DokWys pojDokWysPrzej = (from z in context.DokWys
                                                                 where z.Id.Equals((int)pozew.DokWys_Id)
                                                                 select z).FirstOrDefault();
                                        Paczka aktualnaPaczka = (from z in context.Paczka
                                                                 where z.Id.Equals(epuParamModel.IdPozwuWLexEna)
                                                                 select z).FirstOrDefault();

                                        aktualnaPaczka.DataWyslania = DateTime.Now;
                                        aktualnaPaczka.StatusPaczki = 4;

                                        iserror = false;
                                        if (zpDataModel.ZlozPozewOutputElementModels.Count > 0)
                                        {
                                            foreach (var element in zpDataModel.ZlozPozewOutputElementModels)
                                            {

                                                if (element.SygnaturaWgPowoda == null) continue;
                                                if (element.SygnaturaWgPowoda.Contains("@"))
                                                {
                                                    _IdSprawy = Convert.ToInt32(element.SygnaturaWgPowoda.Substring(element.SygnaturaWgPowoda.IndexOf("@") + 1));
                                                }
                                                else
                                                {
                                                    Sprawa _spr = (from z in context.Sprawa
                                                                   where z.sygnatura.Equals(element.SygnaturaWgPowoda)
                                                                   select z).FirstOrDefault();
                                                    if (_spr != null)
                                                        _IdSprawy = _spr.id;
                                                    else
                                                        Utils.LogWriter("Błąd sygnatury pozwu:" + element.SygnaturaWgPowoda);
                                                }
                                                if (_IdSprawy == pozew.IdSprawy)
                                                {

                                                    dokwyskom = new DokumentWysKomunikacjaEPU();
                                                    dokwyskom.czyus = 0;
                                                    dokwyskom.d_kreacji = DateTime.Now;
                                                    dokwyskom.DokWys_Id = pozew.DokWys_Id;
                                                    dokwyskom.PozewOutputElementModels_Id = element.Id;
                                                    if (element.KodWalidacjiPozwu < 0)
                                                    { pojDokWysPrzej.StatusDok = 5; iserror = true; dokwyskom.Status = 5; }   // dokument odrzucony
                                                    else
                                                        dokwyskom.Status = 4;   // zwrócony
                                                    context.AddToDokumentWysKomunikacjaEPU(dokwyskom);
                                                   
                                                }

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
                                    context.AddToPaczkaKomunikacja(paczkaKomunikacja);
                                    context.SaveChanges();
                                }
                                statusZadaniap = "Ooops!";
                                Utils.LogWriter("Błąd w paczce" + listaBledowZEPU + " Numer paczki:" + epuParamModel.IdPozwuWLexEna.ToString());
                                return -100;
                            }
                        }
                    }
                    break;
                case 3:  // Wywołanie i odpowieź na moje sprawy
                    MojeSprawyOutputDataModel msModel = new MojeSprawyOutputDataModel
                    {
                        SprawaOutputElementModels = new List<SprawaOutputElementModel>()
                    };
                    int IdSprawy = 0;
                    string listaBledowMojeSprawy = "";
                    Sprawa spr;
                    NazwaStatusu nazstat;
                    Paczka dokPacz;
                    bool nowaSprawaEpu = false;
                    List<int> IdSpraw = new List<int>();
                    try
                    {
                        XmlSerializer mySerializerMojePozwy = new XmlSerializer(typeof(EPUParamModel));
                        EPUParamModel epuParamModelMojePozwy = (EPUParamModel)mySerializerMojePozwy.Deserialize(new StringReader(zadanieParametryp));
                        EpuProxy.EpuProxyClient klientPozwy = new EpuProxy.EpuProxyClient(ustawienia.serwisProdukcyjny);
                        KontoEPU kontoEpuPozwy = null;
                        {
                            kontoEpuPozwy = (from z in context.KontoEPU
                                             where z.Id.Equals(epuParamModelMojePozwy.KontoEpuId) //UWAGA nalez zmienić na parametr z procedury
                                             select z).FirstOrDefault();

                        }

                        //klientPozwy.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
                        klientPozwy.setUserData(kontoEpuPozwy.LoginEPU, DecodeFrom64(kontoEpuPozwy.EPUPasswd), kontoEpuPozwy.APIKey);
                        int wynikPozwy = klientPozwy.mojeSprawy(epuParamModelMojePozwy.DataOd, epuParamModelMojePozwy.DataDo, epuParamModelMojePozwy.KryteriumFiltrowania, epuParamModelMojePozwy.FiltrSlowny, ref listaBledowMojeSprawy);
                        if (wynikPozwy > 0)
                        {
                            {
                                var query = from z in context.SprawaOutputElementModels
                                            where z.MojeSprawyOutputDataModels_Id == wynikPozwy
                                            select z;

                                if (query.Count() > 0)
                                {
                                    foreach (var sprawaZEpu in query)
                                    {
                                        if (sprawaZEpu.SygnaturaWgPowoda == null) continue;

                                        if (sprawaZEpu.SygnaturaWgPowoda.Contains("@") &&  !sprawaZEpu.SygnaturaWgPowoda.Contains("#"))
                                        {
                                            _IdSprawy = Convert.ToInt32(sprawaZEpu.SygnaturaWgPowoda.Substring(sprawaZEpu.SygnaturaWgPowoda.IndexOf("@") + 1));
                                        }
                                        else
                                        {
                                             if (sprawaZEpu.SygnaturaWgPowoda.Contains("#"))
                                              {
                                                    sprawaZEpu.SygnaturaWgPowoda = sprawaZEpu.SygnaturaWgPowoda.Substring(0, sprawaZEpu.SygnaturaWgPowoda.IndexOf('@'));

                                              }                  

                                            Sprawa _spr = (from z in context.Sprawa
                                                           where z.sygnatura.Equals(sprawaZEpu.SygnaturaWgPowoda)
                                                           select z).FirstOrDefault();
                                            if (_spr != null)
                                                _IdSprawy = _spr.id;
                                            else
                                            {
                                                Utils.LogWriter("Błąd sygnatury pozwu:" + sprawaZEpu.SygnaturaWgPowoda);
                                                continue;
                                            }
                                        }

                                        IdSprawy = _IdSprawy;
                                        context.AddToSprawaKomunikacjaEPU(new SprawaKomunikacjaEPU
                                        {
                                            SprawaOutputElementModels_Id = sprawaZEpu.Id,
                                            Zadanie_Id = IdZadaniaP,
                                            Status = 4,
                                            d_kreacji = DateTime.Now,
                                            Sprawa_Id = IdSprawy

                                        }

                                        );
                                        spr = (from z in context.Sprawa
                                               where z.id == IdSprawy
                                               select z).FirstOrDefault();
                                        nazstat = (from x in context.NazwaStatusu
                                                   where x.Krok == 3
                                                   select x).FirstOrDefault();
                                        if (spr != null && nazstat != null)
                                        {
                                            if (spr.IdSprawyEPU == 0 || spr.IdSprawyEPU == null)
                                            {

                                                spr.SygnNCe = sprawaZEpu.SygnaturaSprawy;
                                                spr.IdSprawyEPU = sprawaZEpu.IdSprawy;
                                                StatusSprawy stspr = new StatusSprawy();
                                                stspr.czyus = 0;
                                                stspr.CzyWiena = 0;
                                                stspr.DataStatusu = DateTime.Now;
                                                stspr.Sprawa_id = spr.id;
                                                stspr.NazwaStatusu_Id = nazstat.Id;
                                                context.AddToStatusSprawy(stspr);



                                            }

                                        }
                                        if (IdSprawy > 0)
                                        {

                                            dokPacz = (from x in context.Paczka
                                                       join dp in context.DokumentPaczka
                                                       on x.Id equals dp.Paczka_Id
                                                       join d in context.DokWys
                                                       on dp.DokWys_Id equals d.Id
                                                       where d.Sprawa_id == IdSprawy && dp.czyus == 0 && d.TypDok == 10 && d.StatusDok == 3 && x.StatusPaczki == 3
                                                       select x).FirstOrDefault();
                                            if (dokPacz != null)
                                            {
                                                dokPacz.StatusPaczki = 5; // rozpisana;
                                                dokPacz.d_modyfikacji = DateTime.Now;
                                            }
                                            else
                                                Utils.LogWriter("Brak paczki - możliwy błąd, lub wielokrotne wywołanie moje Sprawy dla takiego samego zakresu");

                                        }

                                    }
                                }
                                //zapisanie wyników do tablicy SprawaKomunikacjaZEpu
                                // dodać zadanie - paczka dtsc ..

                                context.SaveChanges();
                            }
                            Utils.LogWriter("OK" + listaBledowMojeSprawy);
                            statusZadaniap = "OK" + listaBledowMojeSprawy;
                            IdModel = wynikPozwy;
                            return 200;
                        }
                        else
                        {
                            Utils.LogWriter("Błąd w trakcie pobierania listy spraw :" + listaBledowMojeSprawy);
                            statusZadaniap = "Błąd w trakcie pobierania listy spraw " + listaBledowMojeSprawy;
                            return wynikPozwy;
                        }
                    }
                    catch (Exception ex)
                    {

                        Utils.LogWriter("Wyjątek w trakcie wykonywania zadania :" + listaBledowMojeSprawy + ex.ToString());
                        statusZadaniap = "Wyjątek w trakcie wykonuwania zadania:" + listaBledowMojeSprawy + ex.ToString();
                        return -110;
                    }
                case 5:   // moje nakazy

                    pdffile = new byte[1];

                    MojeNakazyOutputDataModel mnModel = new MojeNakazyOutputDataModel
                    {
                        NakazOutputElementModels = new List<NakazOutputElementModel>()
                    };
                    listaBledowMojeSprawy = "";
                    try
                    {
                        Sprawa _spr;
                        XmlSerializer mySerializerMojePozwy = new XmlSerializer(typeof(EPUParamModel));
                        Utils.LogWriter("Wywołano Moje nakazy z Parametrami:" + zadanieParametryp);
                        EPUParamModel epuParamModelMojePozwy = (EPUParamModel)mySerializerMojePozwy.Deserialize(new StringReader(zadanieParametryp));
                        EpuProxy.EpuProxyClient klientPozwy = new EpuProxy.EpuProxyClient(ustawienia.serwisProdukcyjny);
                        KontoEPU kontoEpuPozwy = null;
                        List<ulong> IdEPULst;
                        using (var baza = new LexEnaZadanieEntities())
                        {
                            kontoEpuPozwy = (from z in baza.KontoEPU
                                             where z.Id.Equals(epuParamModelMojePozwy.KontoEpuId) //UWAGA nalez zmienić na parametr z procedury
                                             select z).FirstOrDefault();

                        }

                        //klientPozwy.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
                        klientPozwy.setUserData(kontoEpuPozwy.LoginEPU, DecodeFrom64(kontoEpuPozwy.EPUPasswd), kontoEpuPozwy.APIKey);

                            if (epuParamModelMojePozwy.DataOd == null || epuParamModelMojePozwy.DataOd.Value < new DateTime(2000, 1, 1) || epuParamModelMojePozwy.DataOd.Value < new DateTime(2000, 1, 1))
                            epuParamModelMojePozwy.DataOd = DateTime.Today.AddDays(-14);
                            if (epuParamModelMojePozwy.DataOd > DateTime.Today.AddDays(-7))
                                epuParamModelMojePozwy.DataOd = DateTime.Today.AddDays(-7);

                        int wynikNakazy = klientPozwy.mojeNakazy(epuParamModelMojePozwy.DataOd, epuParamModelMojePozwy.DataDo, epuParamModelMojePozwy.KryteriumFiltrowania, epuParamModelMojePozwy.FiltrSlowny, ref listaBledowMojeSprawy);
                        if (wynikNakazy > 0)
                         {
                            IdEPULst = new List<ulong>();

                            {
                                var query = from z in context.NakazOutputElementModels
                                            where z.MojeNakazyOutputDataModels_Id == wynikNakazy

                                            select z;

                                if (query.Count() > 0)
                                {
                                    foreach (var nakazZEpU in query)
                                    {
                                            if (nakazZEpU.SygnaturaWgPowoda == null)
                                            {
                                                Utils.LogWriter("Brak sygn wg powoda");
                                                continue;

                                            }
                                            Utils.LogWriter("%%%%%% Nakaz z sygnaturą " + nakazZEpU.SygnaturaWgPowoda);
                                            if (nakazZEpU.SygnaturaWgPowoda.Contains("@") && !nakazZEpU.SygnaturaWgPowoda.Contains("#"))
                                            {
                                                _IdSprawy = Convert.ToInt32(nakazZEpU.SygnaturaWgPowoda.Substring(nakazZEpU.SygnaturaWgPowoda.IndexOf("@") + 1));
                                            }
                                            else
                                            {
                                                if (nakazZEpU.SygnaturaWgPowoda.Contains("#"))
                                                {
                                                    nakazZEpU.SygnaturaWgPowoda = nakazZEpU.SygnaturaWgPowoda.Substring(0, nakazZEpU.SygnaturaWgPowoda.IndexOf('@'));

                                                }
                                                _spr = (from z in context.Sprawa
                                                    where z.sygnatura.Equals(nakazZEpU.SygnaturaWgPowoda)
                                                    select z).FirstOrDefault();
                                            if (_spr != null)
                                                _IdSprawy = _spr.id;
                                            else
                                            {
                                                Utils.LogWriter("Błąd sygnatury nakazu:" + nakazZEpU.SygnaturaWgPowoda);
                                                continue;
                                            }
                                        }
                                            // sprawdzenie czy jest takja sprawa
                                            Utils.LogWriter("%%%%%% Nakaz z sygnaturą + idSprawy " + nakazZEpU.SygnaturaWgPowoda + " id sprawy = " + _IdSprawy.ToString() +  " Id Nakazu EPU " + nakazZEpU.IdNakazu.ToString());
                                            _spr = (from z in context.Sprawa
                                                where z.id.Equals(_IdSprawy)
                                                select z).FirstOrDefault();
                                        if (_spr == null)
                                        {
                                            Utils.LogWriter("Brak sprawy o Id :" + _IdSprawy.ToString());
                                            continue;
                                        }
                                        // Sprawdzenie czy w bazie jest już taki nakaz

                                        DokOdebr checkDok = (from z in context.DokOdebr
                                                             where z.IdEPU == nakazZEpU.IdNakazu && z.TypDok == 5
                                                             select z).FirstOrDefault();
                                        if (checkDok != null)
                                            continue;   // jest już taki doc zapisany.
                                        IdSprawy = _IdSprawy;
                                        DokOdebr dode = new DokOdebr();

                                        DokumentKomunikacjaEPU dokkom = new DokumentKomunikacjaEPU();
                                        dode.Sprawa_id = IdSprawy;
                                        dode.TypDok = nakazZEpU.KodDecyzji; //5; // nakaz
                                        dode.StatusDok = nakazZEpU.StatusDokumentu;
                                        dode.Tresc = nakazZEpU.DokumentXML;
                                        dode.IdEPU = nakazZEpU.IdNakazu;
                                        dode.CzyEPU = 1;
                                        dode.CzyZalatw = 0;
                                        dode.d_kreacji = DateTime.Now;
                                        dode.TrescHtml = XML2HTMLTransform.TransformNCompress(dode.Tresc, 5);
                                        dode.Format = 100; // html
                                        dode.Nazwa = "Nakaz";
                                        dode.DataDokumentu = nakazZEpU.DataOrzeczenia.Date;
                                        dode.DataRejestracji = DateTime.Today;
                                        dode.IsChecked = 0;   // nowy dokument
                                        dode.DataZalatwienia = nakazZEpU.DataPrawomocnosci;
                                        dokkom.NakazOutputElementModels_Id = nakazZEpU.Id;
                                        dokkom.DokOdebr = dode;
                                        dokkom.czyus = 0;
                                        IdEPULst.Add((ulong)(dode.IdEPU));
                                        dokkom.d_kreacji = DateTime.Now;
                                        dokkom.Status = 1;
                                        retvalue = XML2HTMLTransform.html2pdfSharp(dode.TrescHtml, ref pdffile);
                                        if (retvalue.Contains("Błąd"))
                                        {
                                            Utils.LogWriter("Wyjątek w trakcie wykonywania zadania : Moje Nakazy, błąd zapisu do .pdf" + retvalue);

                                            return -110;

                                        }
                                        dode.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                                        PdfStore pdf = new PdfStore();
                                        pdf.name = dode.Nazwa + " " + nakazZEpU.SygnaturaWgPowoda + " " + nakazZEpU.SygnaturaSprawy;
                                        pdf.value = pdffile;
                                        dode.PdfStore.Add(pdf);
                                        context.PdfStore.AddObject(pdf);
                                        //dode.PdfStore.Add(pdf);
                                        context.AddToDokOdebr(dode);

                                        spr = (from z in context.Sprawa
                                               where z.id == IdSprawy
                                               select z).FirstOrDefault();
                                        nazstat = (from x in context.NazwaStatusu
                                                   where x.Krok == 4 // z nalkazem
                                                   select x).FirstOrDefault();
                                        if (spr != null && nazstat != null)
                                        {
                                            StatusSprawy stspr = new StatusSprawy();
                                            stspr.czyus = 0;
                                            stspr.CzyWiena = 0;
                                            stspr.DataStatusu = DateTime.Now;
                                            stspr.Sprawa_id = spr.id;
                                            stspr.NazwaStatusu_Id = nazstat.Id;
                                            context.AddToStatusSprawy(stspr);

                                        }

                                    }
                                }
                                //zapisanie wyników do tablicy SprawaKomunikacjaZEpu
                                // dodać zadanie - paczka dtsc ..
                                context.SaveChanges();
                                // GeneratePdfs(IdEPULst, type = Odebrane Nakazy);


                            }
                            Utils.LogWriter("OK" + listaBledowMojeSprawy);
                            statusZadaniap = "OK" + listaBledowMojeSprawy;
                            IdModel = wynikNakazy;
                            return 200;
                        }
                        else
                        {
                            Utils.LogWriter("Błąd w trakcie pobierania listy nakazów :" + listaBledowMojeSprawy);
                            statusZadaniap = "Błąd w trakcie pobierania listy nakazów " + listaBledowMojeSprawy;
                            return wynikNakazy;
                        }
                    }
                    catch (Exception ex)
                    {

                        Utils.LogWriter("Wyjątek w trakcie wykonywania zadania :" + listaBledowMojeSprawy + ex.ToString());
                        statusZadaniap = "Wyjątek w trakcie wykonywania zadania:" + listaBledowMojeSprawy + ex.ToString();
                        return -110;
                    }



                    break;
                case 6:
                    return MojeDoreczenia(zadanieParametryp, IdZadaniaP, out statusZadaniap);

                case 7:
                        int wynik;
                        MojeOrzeczenia(zadanieParametryp, IdZadaniaP, out statusZadaniap, out wynik);
                        IdModel = wynik;
                        if (wynik <= 0)
                            return wynik;
                        else
                            return 200;

                    case 8:
                    return ZlozWnioki(zadanieParametryp, IdZadaniaP, out statusZadaniap);
                case 9:
                    return ZlozDokumenty(zadanieParametryp, IdZadaniaP, out statusZadaniap);
                case 10:
                    return ListaKancelarii(zadanieParametryp, IdZadaniaP, out statusZadaniap);

                default:
                    statusZadaniap = " Nie znaleziono typu procedury";
                    return -200;
            }
        }
        return -1;
        }

        private static int MojeOrzeczenia(string zadanieParametryp, int IdZadaniaP, out string statusZadaniap, out int idModel)
        {
            string listaBledowMojeSprawy = "";
            int _IdSprawy;
            string retvalue;
            Sprawa spr;
            NazwaStatusu nazstat;
            byte[] pdffile;
            List<ulong> IdEPULst;
            pdffile = new byte[1];
            try
            {
                int IdSprawy = 0;
                Sprawa _spr;
                Ustawienia ustawienia = new Ustawienia();
                XmlSerializer mySerializerMojeOrzeczenia = new XmlSerializer(typeof(EPUParamModel));
                EPUParamModel epuParamModelMojeOrzeczenia = (EPUParamModel)mySerializerMojeOrzeczenia.Deserialize(new StringReader(zadanieParametryp));
                EpuProxy.EpuProxyClient klientOrzeczenia= new EpuProxy.EpuProxyClient(ustawienia.serwisProdukcyjny);
                IdEPULst = new List<ulong>();
                KontoEPU kontoEpuPozwy = null;
                using (var baza = new LexEnaZadanieEntities())
                {
                    kontoEpuPozwy = (from z in baza.KontoEPU
                                     where z.Id.Equals(epuParamModelMojeOrzeczenia.KontoEpuId) //UWAGA nalez zmienić na parametr z procedury
                                     select z).FirstOrDefault();

                }

                //klientPozwy.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
                klientOrzeczenia.setUserData(kontoEpuPozwy.LoginEPU, DecodeFrom64(kontoEpuPozwy.EPUPasswd), kontoEpuPozwy.APIKey);
                int wynikOrzeczenia = klientOrzeczenia.mojeOrzeczenia( epuParamModelMojeOrzeczenia.DataOd, epuParamModelMojeOrzeczenia.DataDo, epuParamModelMojeOrzeczenia.KryteriumFiltrowania, epuParamModelMojeOrzeczenia.FiltrSlowny, ref listaBledowMojeSprawy);
                if (wynikOrzeczenia > 0)
                {
                    using (var aktualizacajaBazy = new LexEnaZadanieEntities())
                    {
                        var query = from z in aktualizacajaBazy.OrzeczenieVer2OutputElementModel
                                    where z.MojeOrzeczeniaVer2OutputDataModels_Id == wynikOrzeczenia

                                    select z;

                        if (query.Count() > 0)
                        {
                            foreach (var orzeczeniaZEpU in query)
                            {
                                if (orzeczeniaZEpU.SygnaturaWgPowoda == null) continue;
                                if (orzeczeniaZEpU.SygnaturaWgPowoda.Contains("@") && !orzeczeniaZEpU.SygnaturaWgPowoda.Contains("#"))
                                {
                                    _IdSprawy = Convert.ToInt32(orzeczeniaZEpU.SygnaturaWgPowoda.Substring(orzeczeniaZEpU.SygnaturaWgPowoda.IndexOf("@") + 1));
                                }
                                else
                                {
                                    if (orzeczeniaZEpU.SygnaturaWgPowoda.Contains("#"))
                                    {
                                        orzeczeniaZEpU.SygnaturaWgPowoda = orzeczeniaZEpU.SygnaturaWgPowoda.Substring(0, orzeczeniaZEpU.SygnaturaWgPowoda.IndexOf('@'));

                                    }
                                    _spr = (from z in aktualizacajaBazy.Sprawa
                                                   where z.sygnatura.Equals(orzeczeniaZEpU.SygnaturaWgPowoda)
                                                   select z).FirstOrDefault();
                                    if (_spr != null)
                                        _IdSprawy = _spr.id;
                                    else
                                    {
                                        Utils.LogWriter("Błąd sygnatury :" + orzeczeniaZEpU.SygnaturaWgPowoda);
                                        continue;
                                    }
                                }
                                _spr = (from z in aktualizacajaBazy.Sprawa
                                        where z.id.Equals(_IdSprawy)
                                        select z).FirstOrDefault();
                                if (_spr == null)
                                {
                                    Utils.LogWriter("Brak sprawy o Id :" + _IdSprawy.ToString());
                                    continue;
                                }
                                DokOdebr checkDok = (from z in aktualizacajaBazy.DokOdebr
                                                     where z.IdEPU == orzeczeniaZEpU.IdOrzeczeia && z.TypDok != 5
                                                     select z).FirstOrDefault();
                                if (checkDok != null)  continue;   // jeśli jest takie pismo - kontynuuj.
                                    IdSprawy = _IdSprawy;
                                DokOdebr dode = new DokOdebr();
                                DokumentKomunikacjaEPU dokkom = new DokumentKomunikacjaEPU();
                                    dode.Sprawa_id = IdSprawy;
                                    if (orzeczeniaZEpU.DataKlauzuli > new DateTime(2000, 1, 1))   // to jest klauzula
                                        dode.TypDok = 17;
                                    else
                                        dode.TypDok = 101; // inne  orzeczenie
                                        
                                    
                                    dode.StatusDok = orzeczeniaZEpU.Status;
                                    dode.Tresc = orzeczeniaZEpU.DokumentXML;
                                    dode.IdEPU = orzeczeniaZEpU.IdOrzeczeia;
                                    dode.CzyEPU = 1;
                                    dode.CzyZalatw = 0; 
                                    dode.d_kreacji = DateTime.Now;
                                    dode.TrescHtml = XML2HTMLTransform.TransformNCompress(dode.Tresc, 17);
                                    dode.Format = 100; // html
                                    dode.Nazwa = orzeczeniaZEpU.NazwaDecyzji;
                                    dode.DataDokumentu = orzeczeniaZEpU.DataOrzeczenia.Date;
                                    dode.DataRejestracji = DateTime.Today;
                                    dode.IsChecked = 0;   // nowy dokument
                                    dode.DataZalatwienia = orzeczeniaZEpU.DataPrawomocnosci;
                                    dokkom.OrzeczenieVer2OutputElementModel_Id = orzeczeniaZEpU.Id;
                                    dokkom.DokOdebr = dode;
                                    dokkom.czyus = 0;
                                    dokkom.d_kreacji = DateTime.Now;
                                    dokkom.Status = 1;
                                    IdEPULst.Add((ulong)(dode.IdEPU));
                                    if (dode.TypDok == 17)
                                    {
                                        retvalue = XML2HTMLTransform.html2pdfSharp(dode.TrescHtml, ref pdffile);
                                        if (retvalue.Contains("Błąd"))
                                        {
                                            Utils.LogWriter("Wyjątek w trakcie wykonywania zadania : Moje Nakazy, błąd zapisu do .pdf" + retvalue);
                                            statusZadaniap = "Wyjątek w trakcie wykonuwania zadania:" + retvalue;
                                            idModel = wynikOrzeczenia;
                                            return -110;

                                        }
                                        dode.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                                    PdfStore pdf = new PdfStore();
                                        pdf.name = dode.Nazwa + " " + orzeczeniaZEpU.SygnaturaWgPowoda + " " + orzeczeniaZEpU.SygnaturaSprawy;
                                        pdf.value = pdffile;
                                        dode.PdfStore.Add(pdf);
                                        aktualizacajaBazy.PdfStore.AddObject(pdf);
                                    }


                                    aktualizacajaBazy.AddToDokOdebr(dode);
                                    if (dode.TypDok == 17)  // jeśli klauzula to zmień status
                                    {
                                        nazstat = (from x in aktualizacajaBazy.NazwaStatusu
                                                   where x.Krok == 5 // tytuł wykonawczy
                                                   select x).FirstOrDefault();
                                        if (nazstat != null)
                                        {
                                            StatusSprawy stspr = new StatusSprawy();
                                            stspr.czyus = 0;
                                            stspr.CzyWiena = 0;
                                            stspr.DataStatusu = DateTime.Now;
                                            stspr.Sprawa_id = IdSprawy;
                                            stspr.NazwaStatusu_Id = nazstat.Id;
                                            aktualizacajaBazy.AddToStatusSprawy(stspr);

                                        }

                                    
                                    
                                    }
                                
                            }
                        }
                        //zapisanie wyników do tablicy SprawaKomunikacjaZEpu
                        // dodać zadanie - paczka dtsc ..
                        aktualizacajaBazy.SaveChanges();
                        // daodanie
                        // GeneratePdfs(IdEPULst, type = Odebrane );
                        
                    }
                    Utils.LogWriter("OK" + listaBledowMojeSprawy);
                    statusZadaniap = "OK" + listaBledowMojeSprawy;
                    idModel = wynikOrzeczenia;
                    return 200;
                }
                else
                {
                    Utils.LogWriter("Błąd w trakcie pobierania listy postanowień :" + listaBledowMojeSprawy);
                    statusZadaniap = "Błąd w trakcie pobierania listy postanowień " + listaBledowMojeSprawy;
                    idModel = wynikOrzeczenia;
                    return wynikOrzeczenia;
                }
            }
            catch (Exception ex)
            {

                Utils.LogWriter("Wyjątek w trakcie wykonywania zadania :" + listaBledowMojeSprawy + ex.ToString());
                statusZadaniap = "Wyjątek w trakcie wykonuwania zadania:" + listaBledowMojeSprawy + ex.ToString();
                idModel = -1; 
                return -110;
            }
        }

        private static int MojeDoreczenia(string zadanieParametryp, int IdZadaniaP, out string statusZadaniap)
        {
            string listaBledowMojeSprawy = "";
            int _IdSprawy=0;
            try
            {
                int IdSprawy = 0;
                Ustawienia ustawienia = new Ustawienia();
                XmlSerializer mySerializerMojeDoreczenia = new XmlSerializer(typeof(EPUParamModel));
                EPUParamModel epuParamModelMojeDoreczenia = (EPUParamModel)mySerializerMojeDoreczenia.Deserialize(new StringReader(zadanieParametryp));
                EpuProxy.EpuProxyClient klientDoreczenia = new EpuProxy.EpuProxyClient(ustawienia.serwisProdukcyjny);
                KontoEPU kontoEpuPozwy = null;
                using (var baza = new LexEnaZadanieEntities())
                {
                    kontoEpuPozwy = (from z in baza.KontoEPU
                                     where z.Id.Equals(epuParamModelMojeDoreczenia.KontoEpuId) //UWAGA nalez zmienić na parametr z procedury
                                     select z).FirstOrDefault();

                

                //klientPozwy.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
                klientDoreczenia.setUserData(kontoEpuPozwy.LoginEPU, DecodeFrom64(kontoEpuPozwy.EPUPasswd), kontoEpuPozwy.APIKey);
                int wynikDoreczenia = klientDoreczenia.mojeDoreczeniav2(epuParamModelMojeDoreczenia.RodzajDaty, epuParamModelMojeDoreczenia.DataOd, epuParamModelMojeDoreczenia.DataDo, epuParamModelMojeDoreczenia.KryteriumFiltrowania, epuParamModelMojeDoreczenia.FiltrSlowny, epuParamModelMojeDoreczenia.StatusDoreczenia, ref listaBledowMojeSprawy);
                if (wynikDoreczenia > 0)
                {
                   
                        var query = from z in baza.DoreczenieVer2OutputElementModel
                                    where z.MojeDoreczeniaVer2OutputDataModels_Id == wynikDoreczenia 

                                    select z;

                        if (query.Count() > 0)
                        {
                            // 
                            Termin oldTermin;
                            TerminTyp tt = (from x in baza.TerminTyp where x.Numer.Equals(1) select x).FirstOrDefault();
                            if (tt == null) { Utils.LogWriter("Błąd - brak wiersza w tabeli TerminTyp"); statusZadaniap = "Błąd - brak wiersza w tabeli TerminTyp"; return -110; }

                            foreach (var doreczeniaZEpU in query)
                            {
                                if (doreczeniaZEpU.SygnaturaWgPowoda == null) continue;
                                if (doreczeniaZEpU.SygnaturaWgPowoda.Contains("@"))
                                {
                                    _IdSprawy = Convert.ToInt32(doreczeniaZEpU.SygnaturaWgPowoda.Substring(doreczeniaZEpU.SygnaturaWgPowoda.IndexOf("@") + 1));
                                }
                                else
                                {
                                    Sprawa _spr = (from z in baza.Sprawa
                                                   where z.sygnatura.Equals(doreczeniaZEpU.SygnaturaWgPowoda)
                                                   select z).FirstOrDefault();
                                    if (_spr != null)
                                        _IdSprawy = _spr.id;
                                    else
                                    {
                                        Utils.LogWriter("Błąd sygnatury :" + doreczeniaZEpU.SygnaturaWgPowoda);
                                        continue;
                                    }
                                }
                                // 
                                     oldTermin = (from y in baza.Termin
                                                  join m in baza.DoreczenieVer2OutputElementModel on y.Ref_Id equals m.Id
                                                where y.Co == 1 && m.IdDoreczeniaVer2 == doreczeniaZEpU.IdDoreczeniaVer2 && y.czyus == 0  
                                                orderby m.Id descending
                                                select y ).FirstOrDefault();
                                     if (oldTermin == null)
                                     {

                                         IdSprawy = _IdSprawy;
                                         Termin trmn = new Termin();
                                         trmn.czyus = 0;
                                         trmn.d_kreacji = DateTime.Now;
                                         trmn.Sprawa_Id = IdSprawy;
                                         trmn.TerminTyp_Id = tt.Id;
                                         trmn.Status = 0; // nowy termin
                                         trmn.Ref_Id = doreczeniaZEpU.Id;
                                         trmn.Co = 1;  // doręczenie
                                         trmn.DataZapisu = DateTime.Today;
                                         trmn.Opis = doreczeniaZEpU.Opis;
                                         trmn.DataDoWykonania = doreczeniaZEpU.DataDoreczenia.GetValueOrDefault(DateTime.Today).AddDays(tt.AlertAfterDays.GetValueOrDefault(7));
                                         baza.AddToTermin(trmn);
                                     }
                                     else
                                     {

                                         oldTermin.Ref_Id = doreczeniaZEpU.Id;
                                         oldTermin.d_modyfikacji = DateTime.Now;
                                         if (oldTermin.Status == 0)
                                         {
                                             oldTermin.DataDoWykonania = doreczeniaZEpU.DataDoreczenia.GetValueOrDefault(DateTime.Today).AddDays(tt.AlertAfterDays.GetValueOrDefault(7));
                                         }                                
                                     }
                                                                    
                            }
                        }
                        //zapisanie wyników do tablicy SprawaKomunikacjaZEpu
                        // dodać zadanie - paczka dtsc ..
                        baza.SaveChanges();
                   
                    Utils.LogWriter("OK" + listaBledowMojeSprawy);
                    statusZadaniap = "OK" + listaBledowMojeSprawy;
                    return 200;
                }
                else
                {
                    Utils.LogWriter("Błąd w trakcie pobierania listy doręczeń :" + listaBledowMojeSprawy);
                    statusZadaniap = "Błąd w trakcie pobierania listy doręczeń " + listaBledowMojeSprawy;
                    return wynikDoreczenia;
                }
            }
            }
            catch (Exception ex)
            {

                Utils.LogWriter("Wyjątek w trakcie wykonywania zadania Moje Doręczenia :" + listaBledowMojeSprawy + ex.ToString());
                statusZadaniap = "Wyjątek w trakcie wykonuwania zadania Moje Doręczenia:" + listaBledowMojeSprawy + ex.ToString();
                return -110;
            }

        }

        static public string DecodeFrom64(string encodedData)
        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;

        }
        private static Regex _isNumber = new Regex(@"^\d+$");

        public static bool IsInteger(string theValue)
        {
            Match m = _isNumber.Match(theValue);
            return m.Success;
        } //IsInteger
        
        private static bool   MojeSprawy(DateTime? dataOdP, DateTime? dataDoP, int? kryteriumFiltrowaniaP, string filtrSlownyP, int kontoEpuIdP, bool Oczasie, DateTime? terminRozpoczęcia, LexEnaZadanieEntities lexena)
        {
            
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
                zadanie.Parametry = SerializeToXML(new EPUParamModel { DataDo = dataOdP, DataOd = dataOdP, KryteriumFiltrowania = kryteriumFiltrowaniaP, FiltrSlowny = filtrSlownyP });
                lexena.AddToZadanieSet(zadanie);
                //lexena.SaveChanges();
                return true;

                

            }
            catch (Exception ex)
            {
                
                return false;
            }
            return false;
        }
        private static string SerializeToXML(EPUParamModel epuParamModel)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EPUParamModel));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, epuParamModel);
            return stringWriter.ToString();
        }
    }
   
  

    public enum EpuZadania { ZlozPozew, ZlozZazalenie, ZlozSkarge, ZlozSprzeciw, MojeSprawy, ZlozDokumenty }

    /* 
      3 -pismo
      4- wniosek
      5 - uzupełnienie adresu
      6 - uzupełnienie braków
      13 - rezugnacja z pełnomocnictwa
      14 - zgłoszenie pełnomocnika do sprawy
    
     */

    /* kody decyzji
     * 
      nakaz - 5 
      
     * 
     * */
    public  class  WalidacjaWstepnaXSD
    {
        
        public bool Poprawny { get; set; }
        public string LogWalidacji { get; set;     }
        public WalidacjaWstepnaXSD(string xmlDoWalidacji,int  typDokumentu)
        {
            try
            {
              
                this.LogWalidacji = "";
                this.Poprawny = true;
                string path =  HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.MapPath("~/XSD") : "~/XSD";

                    XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                switch (typDokumentu)
                    
                {
                     case 0 :
                        settings.Schemas.Add(null, XmlReader.Create(path  +  @"/PozewEPU.xsd"));
                        break;
                    case 1:
                        settings.Schemas.Add(null, XmlReader.Create(path +  @"/WnioskiEgzekucyjneEPU.xsd"));
                        break;
                    case 2:
                        settings.Schemas.Add(null, XmlReader.Create(path + @"/DokumentEPU.xsd"));
                        break;
                    default:
                        settings.Schemas.Add(null, XmlReader.Create (path +  @"/PozewEPU.xsd"));
                        break;
                };
               
                // Create the XmlReader object.
                if (xmlDoWalidacji == null) xmlDoWalidacji = "";
                XmlReader reader = XmlReader.Create(new StringReader(xmlDoWalidacji), settings);

                // Parse the file. 
                while (reader.Read()) ;
          
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                Utils.LogWriter(ex.ToString());
                this.LogWalidacji = ex.Message.ToString();
                this.Poprawny = true; // false;
            }
        }
        private  void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                //Console.WriteLine("\tBrak pliku XSD Pozwu Walidacja niemożliwa wszystkie pliki pozwu zostają uznane za błędne" + args.Message);
                Utils.LogWriter("\tBrak pliku XSD Pozwu Walidacja niemożliwa wszystkie pliki pozwu zostają uznane za błędne" + args.Message);
            }
            else
            {

                Utils.LogWriter("\tValidation error: " + args.Message);
            }
            this.Poprawny = false;
            this.LogWalidacji = this.LogWalidacji + args.Message;
        }
    }
    public class EPUParamModel
    {
        public int IdPozwuWLexEna { get; set; }
        public int IdZazaleniaWLexEna { get; set; }
        public int IdSkargiWLexEna { get; set; }
        public int IdSprzeciwWLexEna { get; set; }
        public DateTime? DataOd { get; set; }
        public DateTime? DataDo { get; set; }
        public int? KryteriumFiltrowania { get; set; }
        public string FiltrSlowny { get; set; }
        public int NumerOd { get; set; }
        public int NumerDo { get; set; }
        public int Rok { get; set; }
        public int KontoEpuId { get; set; }
        public int? StatusDoreczenia { get; set; }
        public int? RodzajDaty { get; set; }
    }
    public class PozewRozbudowany : Pozew
    {
        public int Status { get; set; }
        public string Opis { get; set; }
        public int IdSprawy { get; set; }
        public void ConvertPozew(Pozew pozew)
        {
            this.DokWys_Id = pozew.DokWys_Id;
            this.Id = pozew.Id;
            
        }

    }



}
