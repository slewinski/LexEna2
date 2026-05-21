using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using EpuProxy.EpuSrv;
using EpuProxy;
using System.Text.RegularExpressions;
using ZadanieTimerMain;


namespace ZadanieTimer
{
    class DocsFromFile
    {

        private string logname = "infolog.txt";
        private object XmlDeserializeFromString(string objectData, Type type, string defnamespace)
        {
            object result;
            XmlSerializer serializer;

            try
            {
                if (defnamespace != null)
                {

                    serializer = new XmlSerializer(type, defnamespace);
                }
                else
                    serializer = new XmlSerializer(type);





                using (TextReader reader = new StringReader(objectData))
                {
                    result = serializer.Deserialize(reader);



                }

                return result;
            }
            catch (Exception ex)
            {


                return null;
            }

        }
        /*
          public static string SerializeToString(object objToSerialize, Type type)
        {
            var output = new MemoryStream();
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true};
            string outputString;
            using (var xmlWriter = XmlWriter.Create(output, settings))
            {
                var serializer = new XmlSerializer(type);
                xmlWriter.WriteStartDocument();
                serializer.Serialize(xmlWriter, objToSerialize);
            }
            output.Seek(0L, SeekOrigin.Begin);
            // zamina stream na string
            var reader = new StreamReader(output);
            outputString = reader.ReadToEnd();
            return outputString;
        
        }
        */
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

        private string clearnamespaces(string strin)
        {
            string s;

            s = strin.Replace("<a.", "<");
            s = s.Replace("</a.", "</");
            s = s.Replace("<a:", "<");
            s = s.Replace("</a:", "</");

            return s;
        }

        private string ErrorFilename(string fname)
        {
            string extension = "E";
            int i;
            string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
            if (!File.Exists(newname)) return newname;
            for (i = 1; i <= 99; i++ )
            {
                extension = "E" + i.ToString("00");
                newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) +  "." + extension;
                if (!File.Exists(newname)) return newname;
            }

            return null;
        }


        private string DoneFilename(string fname)
        {
            string extension = "X";
            int i;
            string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".XM_";
            if (!File.Exists(newname)) return newname;
            for (i = 1; i <= 99; i++ )
            {
                extension = "X" + i.ToString("00");
                newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) +  "." + extension;
                if (!File.Exists(newname)) return newname;
            }

            return null;
        }
       


        public int ReadPozew(string fname, int KontoEpu)
        {

            PozewEPU[] lista_pozwow;
            Pozwy paczkapozwow;
            Pozew pozewDB;
            PdfStore pdf;

            String SygnWgPowoda;
            int idSprawy;
            string retvalue;
            string sygnatura;
            decimal Noty;
            decimal WDZ;
            decimal odsNal;
            byte[] pdffile;
            string pozewStr;
            Sprawa spr;
            bool newPozew;
            NazwaStatusu nazstat;
            int dokWys_id;
            pdffile = new byte[0];
            string mylogname;
            bool iserror = false;
            bool setBreak;

            mylogname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + logname;

            Utils.LogNamedWriter(" Start Importu zbioru: " + fname, mylogname);
          //  Utils.LogNamedWriter(" Start Importu zbioru: " + fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + "raport.txt");

            SygnWgPowoda = "";
            try
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader(fname);
                string thedata = myFile.ReadToEnd();
                myFile.Close();
                paczkapozwow = (Pozwy)XmlDeserializeFromString(thedata, typeof(Pozwy), null);
                if (paczkapozwow == null)
                {// błąd podczas deserializacji 

                    Utils.LogWriter("Błąd w trakcie deserializacji Pozwu  zbiór" + fname);
                    Utils.LogNamedWriter("Błąd w trakcie deserializacji Pozwu  zbiór" + fname, mylogname);
                    string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                    File.Move(fname, ErrorFilename(fname));


                    return -100;
                }
                lista_pozwow = paczkapozwow.PozewEPU;



                foreach (PozewEPU pozew in lista_pozwow)
                {
                    string OrygOzn;
                    LexEnaZadanieEntities lexena = new LexEnaZadanieEntities();
                    pozewStr = SerializeToString(pozew, typeof(PozewEPU), true);
                    newPozew = false;
                    dokWys_id = 0;
                    bool sygnOK = true;
                    SygnWgPowoda = pozew.SprawaWgPowoda;
                    
                   // Utils.LogNamedWriter(SygnWgPowoda, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + "raport.txt");
                   // continue;
                    
                    OrygOzn = SygnWgPowoda;
                    
                    if (SygnWgPowoda.Trim().StartsWith("ENERGA#"))
                    {
                        string sygn;
                        sygnOK = false;
                        Regex r = new Regex(@"posługiwanie się w korespondencji nr sygnatury [a-zA-Z]{1,2}-\d{1,6}/\d{4}");
                        Match m = r.Match(pozew.Uzasadnienie);
                        if (m.Success)
                        {
                            sygn = m.Value.Replace("posługiwanie się w korespondencji nr sygnatury ", "");
                            SygnWgPowoda = sygn + "@0";
                            sygnOK = true;
                        }
                        else 
                        {
                            // szukanie po fakturze 
                            foreach (typDowod dow in pozew.ListaDowodow)
                            {
                                if (dow.Oznaczenie != null)
                                {
                                   Naleznosc  nalx = (from z in lexena.Naleznosc 
                                           where z.opis == dow.Oznaczenie orderby z.Id descending 
                                           select z).FirstOrDefault();
                                   if (nalx != null)
                                   {
                                       SygnWgPowoda = nalx.Sprawa.sygnatura + "@0";
                                       sygnOK = true;
                                       break;
                                   }
                                }
                            
                            
                            }
                          
                        
                        
                        }
                        if (sygnOK == false)
                        {
                            Utils.LogNamedWriter(" Błąd importu pozwu : " + SygnWgPowoda + " nie można ustalić sygnatury sprawy ", mylogname);
                            iserror = true;
                            continue;
                        }
                    }
                    Utils.LogNamedWriter(" Start Importu pozwu: " + SygnWgPowoda, mylogname);
                   
                    if (SygnWgPowoda.Contains("#"))
                        idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                    else
                        idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));
                    // dodanie encji pozew 
                  
                    spr = (from z in lexena.Sprawa
                           where z.id == idSprawy
                           select z).FirstOrDefault();

                    if (spr == null)
                    {
                        sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                        spr = (from z in lexena.Sprawa
                               where z.sygnatura == sygnatura orderby z.id descending 
                               select z).FirstOrDefault();

                       


                    }
                    
                    if (spr == null)
                    {
                        Utils.LogWriter("Błąd w trakcie wczytywania Pozwu  " + SygnWgPowoda + " nie istnieje w Lexena, zbiór" + fname);
                        Utils.LogNamedWriter("Błąd w trakcie wczytywania Pozwu  " + SygnWgPowoda + " nie istnieje w Lexena, zbiór" + fname,mylogname);

                        //File.Move(fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR");
                        //return -100;
                        iserror = true;
                        continue;
                    }
                    else
                        idSprawy = spr.id;


                    DokWys dw = (from z in lexena.DokWys.Include("Pozew")
                                 where z.Sprawa_id == idSprawy
                                 select z).FirstOrDefault();
                    if (dw == null)
                    {
                        dw = new DokWys();
                        newPozew = true;
                        Utils.LogNamedWriter("Tworzę nowy pozew  dla " + SygnWgPowoda , mylogname);
                    }
                    else
                    {
                        dokWys_id = dw.Id;
                    }
                    dw.d_kreacji = DateTime.Today;
                    dw.DataDok = pozew.DataZlozenia;
                    dw.DataOdbioru = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                    dw.InneKoszty = pozew.InneKoszty.Wartosc;
                    dw.Koszty = pozew.OplataSadowa.WartoscOplaty;
                    
                    if (pozew.KosztyZastepstwa.Zasadzenie == 1)
                        if (pozew.KosztyZastepstwa.WgNorm == 1)
                            dw.Kzp = EPUCalc.countKZP(pozew.WartoscSporu);
                        else
                            dw.Kzp = pozew.KosztyZastepstwa.Wartosc;
                    dw.Nazwa = "Pozew";
                    if (KontoEpu > 0) dw.KontoEPU_Id = KontoEpu;

                    Regex rAUMS = new Regex(@"^[A-Z]\d{8}$");
                    Regex rSelen = new Regex(@".+\/WDZ$");
                    Regex rCCnB = new Regex(@"^\d{9}\/\d{4}$");
                    int[] arr = new int[200];
                    int index = 0;
                    foreach (typDowod td in pozew.ListaDowodow)
                    {
                        if (String.IsNullOrWhiteSpace(td.Oznaczenie)) continue;

                        string dOpis = td.Oznaczenie.Trim();
                        
                        Match m = rAUMS.Match(dOpis);
                        if (!m.Success)
                        {
                            m = rSelen.Match(dOpis);
                            if (!m.Success)
                                m = rCCnB.Match(dOpis);
                        }
                        if (m.Success)
                            arr[index++] = td.Numer;
                    }

                    
                    Noty = 0;
                    WDZ = 0 ;
                    odsNal = 0;
                    foreach (typRoszczenie roszcz in pozew.ListaRoszczen)
                    {
                        if (roszcz.Odsetki != null)
                        {
                            if (roszcz.Odsetki.Count == 1)
                            {
                               // if (roszcz.Odsetki[0].Od_Wniesienia == 1 && roszcz.Odsetki[0].DataOd == Convert.ToDateTime("2000-01-01"))
                                if (roszcz.Odsetki[0].Od_Wniesienia == 1)    
                                    Noty += roszcz.Wartosc;


                            }


                        }
                      for (int i = 0; i < index ; i++ )
                      {
                          if (roszcz.Dowody != null && roszcz.Dowody.Contains(arr[i]))
                          {
                              WDZ += roszcz.Wartosc;
                          }

                        }
                    }



                    if (WDZ > 0 && WDZ <= Noty)
                        Noty -= WDZ;
                    dw.OdsetkiKapital = WDZ;
                    dw.NotyOdsetkowe = Noty;
                    dw.Opis = "Pozew w EPU";
                    dw.RodzajDok = 10;
                    dw.StatusDok = 3;
                    dw.Tresc = pozewStr;
                    dw.TypDok = 10;
                    dw.WPS = pozew.WartoscSporu;
                    dw.Sprawa_id = idSprawy;
                    dw.TrescHtml = XML2HTMLTransform.TransformNCompress(dw.Tresc, 0);
                    if (newPozew)
                        pozewDB = new Pozew();
                    else
                        pozewDB = dw.Pozew.FirstOrDefault<Pozew>();
                 
                    pozewDB.DataZlozenia = pozew.DataZlozenia;
                    pozewDB.Koszty = pozew.OplataSadowa.WartoscOplaty;
                    pozewDB.d_kreacji = DateTime.Today;
                    pozewDB.WPS = pozew.WartoscSporu;
                    pozewDB.Tresc = pozewStr;
                    pdf = null;
                    if (newPozew)
                    {
                        dw.Pozew = new System.Data.Objects.DataClasses.EntityCollection<Pozew>();
                        dw.Pozew.Add(pozewDB);
                    }
                    // czy jest pdf
                    if (!newPozew && dokWys_id > 0)
                    {
                        pdf = (from z in lexena.PdfStore
                               where z.DokWys_Id == dokWys_id
                               select z).FirstOrDefault();
                    }

                    if (pdf == null)
                    {
                        pdf = new PdfStore();
                        if (dw.PdfStore == null)
                            dw.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                        dw.PdfStore.Add(pdf);
                        lexena.PdfStore.AddObject(pdf);
                    }
                    pdf.name = "Pozew " + SygnWgPowoda;
                    retvalue = XML2HTMLTransform.html2pdf(dw.TrescHtml, ref pdffile);
                    if (retvalue.Contains("Błąd"))
                    {
                        Utils.LogWriter("Wyjątek w trakcie zapisu do pdf " + retvalue + " " + SygnWgPowoda + "zbiór " + fname);
                        return -1;
                    }
                   
                    pdf.value = pdffile;
                    if (dw.DokumentWysKomunikacjaEPU == null)
                        dw.DokumentWysKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentWysKomunikacjaEPU>();
                    DokumentWysKomunikacjaEPU dokwyskom = new DokumentWysKomunikacjaEPU();
                    dokwyskom.czyus = 0;
                    dokwyskom.d_kreacji = DateTime.Now;
                    dokwyskom.Status = 3;
                    dw.DokumentWysKomunikacjaEPU.Add(dokwyskom);
                    lexena.AddToDokumentWysKomunikacjaEPU(dokwyskom);
                    // Dodanie  radcy skałdającego

                    string pesel = pozew.OsobaSkladajaca.Osoba.PESEL;
                    Radca radca = (from w in lexena.Radca where w.Pesel == pesel select w).FirstOrDefault();

                    if (radca != null)
                    {
                        if (spr != null)
                        {
                            spr.Radca_Id = radca.Id;

                        }


                    }
                    // dodanie statusu do sprawy 
                    nazstat = (from x in lexena.NazwaStatusu
                               where x.Krok == 2
                               select x).FirstOrDefault();
                    if (spr != null && nazstat != null)
                    {
                        if (spr.IdSprawyEPU == 0 || spr.IdSprawyEPU == null)
                        {

                            spr.DataZloPozwu = dw.DataDok;
                            spr.InneZadane = dw.InneKoszty;
                            spr.KosztyZadane = dw.Koszty;
                            spr.KzpZadane = dw.Kzp;
                            spr.Uwagi = SygnWgPowoda;


                            StatusSprawy oststat = (from x in lexena.NazwaStatusu
                                                    join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                                    where y.Sprawa_id == idSprawy && x.Krok == 2
                                                    select y).FirstOrDefault();
                            if (oststat == null)
                            {
                                StatusSprawy stspr = new StatusSprawy();
                                stspr.czyus = 0;
                                stspr.CzyWiena = 0;
                                stspr.DataStatusu = DateTime.Now;
                                stspr.Sprawa_id = spr.id;
                                stspr.NazwaStatusu_Id = nazstat.Id;
                                lexena.AddToStatusSprawy(stspr);
                            }
                        }
                        spr.Uwagi = OrygOzn;

                    }
              
                    lexena.SaveChanges(0);
                    Utils.LogNamedWriter(" Import OK:  " + SygnWgPowoda, mylogname);
                }
                if (iserror)
                {
                    Utils.LogWriter("Błąd importu niektórych pozwów "  + " zbiór" + fname);
                    File.Move(fname, ErrorFilename(fname));
                    return -200;
                }
                else
                {
                    Utils.LogNamedWriter(" Pozytywny koniec importu: " + fname, mylogname);
                    File.Move(fname, DoneFilename(fname));
                }
                    return 1;


            }
            catch (System.IO.IOException ioex)
            {
                Utils.LogWriter("Błąd podczas zmiany nazwy zbioru " + ioex.Message + " zbiór" + fname);
                return -200;
            }

            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania Pozwu " + ex.Message + " zbiór" + fname);
                Utils.LogNamedWriter(" !!!!!! BŁĄD IMPORTU POZWU: " + SygnWgPowoda + "  kod błędu:" + ex.Message, mylogname);
                string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                File.Move(fname, ErrorFilename(fname));


                return -100;
            }


        }

        public int ReadWniosekEgz(string fname, int KontoEpu)
        {

            WniosekEgzekucyjny[] lista_wnioskow;
            WnioskiEgzekucyjneEPU paczkawnioskow;
            WniosekEgzekucyjny wniosekDB;
            PdfStore pdf;

            String SygnWgPowoda;
            int idSprawy;
            string retvalue;
            string sygnatura;
            decimal Noty;
            byte[] pdffile;
            string wniosekStr;
            Sprawa spr;
            bool newWniosek;
            NazwaStatusu nazstat;
            int dokWys_id;
            pdffile = new byte[0];
            string mylogname;
            bool iserror = false;
            bool setBreak;

            mylogname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + logname;

            Utils.LogNamedWriter(" Start Importu zbioru: " + fname, mylogname);
            SygnWgPowoda = "";
            try
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader(fname);
                string thedata = myFile.ReadToEnd();
                myFile.Close();
                paczkawnioskow = (WnioskiEgzekucyjneEPU)XmlDeserializeFromString(thedata, typeof(WnioskiEgzekucyjneEPU), null);
                if (paczkawnioskow == null)
                {// błąd podczas deserializacji 

                    Utils.LogWriter("Błąd w trakcie deserializacji Wniosków  zbiór" + fname);
                    Utils.LogNamedWriter("Błąd w trakcie deserializacji Wniosków  zbiór" + fname, mylogname);
                    string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                    File.Move(fname, ErrorFilename(fname));


                    return -100;
                }
                lista_wnioskow = paczkawnioskow.WniosekEgzekucyjny.ToArray();



                foreach (WniosekEgzekucyjny wniosek in lista_wnioskow)
                {
                    LexEnaZadanieEntities lexena = new LexEnaZadanieEntities();
                    wniosekStr = SerializeToString(wniosek, typeof(WniosekEgzekucyjny), true);
                    newWniosek = false;
                    dokWys_id = 0;

                    sygnatura = wniosek.Nakaz.Sygnatura;
                    
                    Utils.LogNamedWriter(" Start Importu wniosku: " + sygnatura, mylogname);

                    sygnatura = sygnatura.Replace(" ", "");
                    if ( sygnatura.Contains("2668167/13"))
                    {
                    ;
                    }
                    spr = (from z in lexena.Sprawa
                           where sygnatura.EndsWith(z.SygnNCe.Replace(" ",""))
                           select z).FirstOrDefault();


                    


                    if (spr == null)

                    {

                        Utils.LogWriter("Błąd w trakcie wczytywania Wniosku Egzekucyjnego  " + sygnatura + " nie istnieje w Lexena, zbiór" + fname);
                        Utils.LogNamedWriter("Błąd w trakcie wczytywania Wniosku Egzekucyjnego  " + sygnatura + " nie istnieje w Lexena, zbiór" + fname, mylogname);

                        //File.Move(fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR");
                        //return -100;
                        iserror = true;
                        continue;
                    }
                    else
                    {
                        idSprawy = spr.id;
                        SygnWgPowoda = spr.sygnatura;
                        
                    }

                    DokWys dw = (from z in lexena.DokWys
                                 where z.Sprawa_id == idSprawy &&  z.TypDok == 30
                                 select z).FirstOrDefault();
                    if (dw == null)
                    {
                        dw = new DokWys();
                        newWniosek = true;
                        Utils.LogNamedWriter("Tworzę nowy Wniosek egzekucyjny dla " + SygnWgPowoda, mylogname);
                        Utils.LogWriter("Tworzę nowy Wniosek egzekucyjny dla " + SygnWgPowoda);
                    }
                    else
                    {
                        dokWys_id = dw.Id;
                    }
                    dw.d_kreacji = DateTime.Today;
                    dw.DataDok = Convert.ToDateTime(wniosek.dataWniosku);
                    dw.DataOdbioru = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                    dw.InneKoszty = wniosek.InneKoszty == null ? 0 : wniosek.InneKoszty.Wartosc;
                    dw.Koszty = 0;
                    int IdKomornik = (int)wniosek.Komornik.ID; 
                    // czy jest taki omornik
                    KancelariaKomornicza kom = (from z in lexena.KancelariaKomornicza
                                 where z.IdEPU  == IdKomornik
                                 select z).FirstOrDefault();
                    if (kom == null)
                    {
                        kom = new KancelariaKomornicza();
                        kom.IdEPU = IdKomornik;
                        kom.Nazwa = wniosek.Komornik.Nazwa;
                        kom.DataWprowadzenia = DateTime.Today;
                        kom.czyus = 0;
                        using (LexEnaZadanieEntities lx2 = new LexEnaZadanieEntities())
                        {
                            lx2.AddToKancelariaKomornicza(kom);
                            lx2.SaveChanges();

                        }
                        
                    }
                    dw.Komornik_Id = kom.Id;

                    if (wniosek.KosztyZastepstwa.Zasadzenie == 1)
                        if (wniosek.KosztyZastepstwa.WgNorm == 1)
                            dw.Kzp = EPUCalc.countKZA(spr.WPS.Value + (spr.KosztyZadane == null ? 0 : spr.KosztyZadane.Value) + (spr.KzpZadane == null ? 0:spr.KzpZadane.Value)  + (spr.InneZadane == null ? 0:spr.InneZadane.Value));
                        else
                            dw.Kzp = wniosek.KosztyZastepstwa.Wartosc;
                    dw.Nazwa = "Wniosek Egzekucyjny";
                    if (KontoEpu > 0) dw.KontoEPU_Id = KontoEpu;
                    Noty = 0;
                    dw.Opis = wniosek.Komornik.Nazwa + " :Id="+ wniosek.Komornik.ID.ToString() ;
                    dw.RodzajDok = 30;
                    dw.StatusDok = 3;
                    dw.Tresc = wniosekStr;
                    dw.TypDok = 30;
                    dw.WPS = spr.WPS +spr.KosztyZadane + spr.KzpZadane + spr.InneZadane;
                    dw.Sprawa_id = idSprawy;
                    dw.TrescHtml = XML2HTMLTransform.TransformNCompress(dw.Tresc, 30);
                    pdf = null;
                    
                    if (!newWniosek && dokWys_id > 0) 
                    {
                        pdf = (from z in lexena.PdfStore
                               where z.DokWys_Id == dokWys_id
                               select z).FirstOrDefault();
                    }

                    if (pdf == null)
                    {
                        pdf = new PdfStore();
                        if (dw.PdfStore == null)
                            dw.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                        dw.PdfStore.Add(pdf);
                        lexena.PdfStore.AddObject(pdf);
                    }
                    pdf.name = "WniosekEgz" + SygnWgPowoda;
                    retvalue = XML2HTMLTransform.html2pdf(dw.TrescHtml, ref pdffile);
                    if (retvalue.Contains("Błąd"))
                    {
                        Utils.LogWriter("Wyjątek w trakcie zapisu do pdf " + retvalue + " " + SygnWgPowoda + "zbiór " + fname);
                        return -1;
                    }

                    pdf.value = pdffile;
                    if (dw.DokumentWysKomunikacjaEPU == null)
                        dw.DokumentWysKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentWysKomunikacjaEPU>();
                    DokumentWysKomunikacjaEPU dokwyskom = new DokumentWysKomunikacjaEPU();
                    dokwyskom.czyus = 0;
                    dokwyskom.d_kreacji = DateTime.Now;
                    dokwyskom.Status = 3;
                    dw.DokumentWysKomunikacjaEPU.Add(dokwyskom);
                    lexena.AddToDokumentWysKomunikacjaEPU(dokwyskom);
                    // Dodanie  radcy skałdającego

                    string pesel = wniosek.OsobaSkladajaca.Osoba.PESEL;
                    Radca radca = (from w in lexena.Radca where w.Pesel == pesel select w).FirstOrDefault();

                    if (radca != null)
                    {
                        if (spr != null)
                        {
                            spr.Radca_Id = radca.Id;

                        }


                    }
                    // dodanie statusu do sprawy 
                    nazstat = (from x in lexena.NazwaStatusu
                               where x.Krok == 10
                               select x).FirstOrDefault();
                    if (spr != null && nazstat != null)
                    {
                        //if (spr.IdSprawyEPU == 0 || spr.IdSprawyEPU == null)
                        {

                                                       
                            StatusSprawy oststat = (from x in lexena.NazwaStatusu
                                                    join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                                    where y.Sprawa_id == idSprawy && x.Krok == 10
                                                    select y).FirstOrDefault();
                            if (oststat == null)
                            {
                                StatusSprawy stspr = new StatusSprawy();
                                stspr.czyus = 0;
                                stspr.CzyWiena = 0;
                                stspr.DataStatusu = DateTime.Now.Date;
                                stspr.Sprawa_id = spr.id;
                                stspr.NazwaStatusu_Id = nazstat.Id;
                                lexena.AddToStatusSprawy(stspr);
                            }
                        }

                    }

                    lexena.SaveChanges(0);
                    Utils.LogNamedWriter(" Import OK:  " + SygnWgPowoda, mylogname);
                }
                if (iserror)
                {
                    Utils.LogWriter("Błąd importu niektórych wniosków " + " zbiór" + fname);
                    File.Move(fname, ErrorFilename(fname));
                    return -200;
                }
                else
                {
                    Utils.LogNamedWriter(" Pozytywny koniec importu: " + fname, mylogname);
                    File.Move(fname, DoneFilename(fname));
                }
                return 1;


            }
            catch (System.IO.IOException ioex)
            {
                Utils.LogWriter("Błąd podczas zmiany nazwy zbioru " + ioex.Message + " zbiór" + fname);
                return -200;
            }

            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania Wniosku " + ex.Message + " zbiór" + fname);
                Utils.LogNamedWriter(" !!!!!! BŁĄD IMPORTU WNIOSKU: " + SygnWgPowoda + "  kod błędu:" + ex.Message, mylogname);
                string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                File.Move(fname, ErrorFilename(fname));


                return -100;
            }


        }

        public int ReadMojeSprawy(string fname, int KontoEpu)
        {


            MojeSprawyOutputData mojesprawy;
            MojeSprawyEPU listamoichspraw;
            SprawaOutputElement[] listaspraw;
            String sygnatura;
            String SygnWgPowoda;
            NazwaStatusu oststat;
            int idSprawy;
            byte[] pdffile;
            NazwaStatusu nazstat;
            pdffile = new byte[0];
            string mylogname;

            mylogname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + logname;

            Utils.LogNamedWriter(" Start Importu zbioru: " + fname, mylogname);
            SygnWgPowoda = "";
            try
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader(fname);
                string thedata = myFile.ReadToEnd();
                thedata = clearnamespaces(thedata);
                myFile.Close();

                if (thedata.Contains("MojeSprawyOutputData")) // rozpoznać 
                {
                    mojesprawy = (MojeSprawyOutputData)XmlDeserializeFromString(thedata, typeof(MojeSprawyOutputData), null);
                    if (mojesprawy == null)
                    {
                        Utils.LogWriter("Błąd w trakcie wczytywania Sprawa, błęd deserializacji XML zbiór: " + fname);
                        Utils.LogNamedWriter("Błąd w trakcie wczytywania Sprawa, błąd deserializacji XML zbiór: " + fname, mylogname);
                        string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                        File.Move(fname, ErrorFilename(fname));


                        return -100;

                    }
                    listaspraw = mojesprawy.listaSpraw;
                }
                else
                {
                    listamoichspraw = (MojeSprawyEPU)XmlDeserializeFromString(thedata, typeof(MojeSprawyEPU), "http://www.currenda.pl/epu");
                    int i = 0;
                    listaspraw = new SprawaOutputElement[listamoichspraw.mojspr.Count];// mojesprawy.listaSpraw;
                  

                    foreach (MojaSprawaEPU myspr in listamoichspraw.mojspr)
                    {
                        listaspraw[i] = new SprawaOutputElement();
                        listaspraw[i].id = myspr.Id;
                        listaspraw[i].kwotaSporu = myspr.KwotaSporu;
                        listaspraw[i].rolaWSprawie = myspr.RolaWSprawie;
                        listaspraw[i].stanSprawy = myspr.StanSprawy;
                        listaspraw[i].sygnaturaSprawy = myspr.SygnaturaSprawy;
                        listaspraw[i].sygnaturaWgPowoda = myspr.SygnaturaWgPowoda;
                        i++;
                    }

                }



                bool iserror = false;

                foreach (SprawaOutputElement pojsprawa in listaspraw)
                {

                    LexEnaZadanieEntities lexena = new LexEnaZadanieEntities();
                    SygnWgPowoda = pojsprawa.sygnaturaWgPowoda;
                    String OrygSygn = SygnWgPowoda; 
                    Utils.LogNamedWriter(" Start Importu Sprawy: " + SygnWgPowoda, mylogname);
                    if (SygnWgPowoda.Trim().StartsWith("ENERGA#"))
                    {
                        SygnWgPowoda += "@0";
                        idSprawy = 0;
                    }
                    else
                    {

                        if (SygnWgPowoda.Contains("#"))
                            idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                        else
                            idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));
                    }
                    // dodanie encji pozew 
                    Sprawa spr = (from z in lexena.Sprawa
                                  where z.id == idSprawy
                                  select z).FirstOrDefault();
                    if (spr == null)
                    {
                        sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                        spr = (from z in lexena.Sprawa
                               where z.sygnatura == sygnatura 
                               select z).FirstOrDefault();
                     

                    }
                    if (spr == null)
                    {
                        spr = (from z in lexena.Sprawa
                               where z.Uwagi == OrygSygn
                               select z).FirstOrDefault();


                    }
                    if (spr == null)
                    {
                        Utils.LogWriter("Błąd w trakcie wczytywania Sprawy " + SygnWgPowoda + " zbiór" + fname);
                        File.Move(fname, ErrorFilename(fname));
                        //return -100;
                        iserror = true;
                        continue;
                    }
                    else
                        idSprawy = spr.id;
                    spr.SygnNCe = pojsprawa.sygnaturaSprawy;
                    spr.IdSprawyEPU = pojsprawa.id;
                    spr.WPS = pojsprawa.kwotaSporu;
                    // dodanie statusu sprawy 

                    spr = (from z in lexena.Sprawa
                           where z.id == idSprawy
                           select z).FirstOrDefault();


                    nazstat = (from x in lexena.NazwaStatusu
                               where x.Krok == 3
                               select x).FirstOrDefault();
                    oststat = (from x in lexena.NazwaStatusu
                               join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                               where y.Sprawa_id == idSprawy
                               orderby x.Krok descending
                               select x).FirstOrDefault();
                    if (oststat != null)
                    {
                        if (oststat.Krok > 2)
                        {
                            Utils.LogWriter("Sprawa ma już wyższy status " + oststat.Krok.ToString() + " " + SygnWgPowoda + " zbiór" + fname);
                            Utils.LogNamedWriter("Sprawa ma już wyższy status " + oststat.Krok.ToString() + " " + SygnWgPowoda + " zbiór" + fname, mylogname);

                        }
                        else
                        {

                            if (spr != null && nazstat != null)
                            {
                                StatusSprawy stspr = new StatusSprawy();
                                stspr.czyus = 0;
                                stspr.CzyWiena = 0;
                                stspr.DataStatusu = DateTime.Now;
                                stspr.Sprawa_id = spr.id;
                                stspr.NazwaStatusu_Id = nazstat.Id;
                                lexena.AddToStatusSprawy(stspr);

                            }

                        }

                    }


                    lexena.SaveChanges(0);
                    Utils.LogNamedWriter(" Import OK:  " + SygnWgPowoda, mylogname);
                }
                if (iserror)
                {
                    Utils.LogWriter("Błąd importu niektórych spraw " + " zbiór" + fname);
                    File.Move(fname, ErrorFilename(fname));
                    return -200;
                }
                else
                {
                    Utils.LogNamedWriter(" Pozytywny koniec importu: " + fname, mylogname);
                    File.Move(fname, DoneFilename(fname));
                    return 1;
                }

            }
            catch (System.IO.IOException ioex)
            {
                Utils.LogWriter("Błąd podczas zmiany nazwy zbioru " + ioex.Message + " zbiór" + fname);
                return -200;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania Spraw " + ex.Message + " zbiór" + fname);
                Utils.LogNamedWriter(" !!!!!! BŁĄD IMPORTU Sprawy: " + SygnWgPowoda + "  kod błędu:" + ex.Message, mylogname);
                File.Move(fname, ErrorFilename(fname));


                return -100;
            }


        }

        ////////////


        public int ReadNakaz(string fname, int KontoEpu, int jaki)
        {

            MojeNakazyOutputData nakazy;
            NakazOutputElement[] lista_nakazow;
            string sygnatura;
            NazwaStatusu oststat;
            PdfStore pdf;

            String SygnWgPowoda;
            int idSprawy;
            string retvalue;
            decimal Noty;
            byte[] pdffile;
            string nakazStr;
            Sprawa spr;
            bool newDok;
            Nakazy nakazybest;
            NazwaStatusu nazstat;
            int dokOdebr_id;
            pdffile = new byte[0];
            string mylogname;
            string fextension = ".XM_"; 

            mylogname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + logname;

            Utils.LogNamedWriter(" Start Importu zbioru: " + fname, mylogname);
            SygnWgPowoda = "";
            try
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader(fname);
                string thedata = myFile.ReadToEnd();
                thedata = clearnamespaces(thedata);
                myFile.Close();
                nakazy = (MojeNakazyOutputData)XmlDeserializeFromString(thedata, typeof(MojeNakazyOutputData), null);
                if (nakazy == null)
                {
                    Utils.LogWriter("Błąd w trakcie wczytywania Nakazu, błąd deserializacji XML zbiór: " + fname);
                    Utils.LogNamedWriter("Błąd w trakcie wczytywania nakazu, błąd deserializacji XML zbiór: " + fname, mylogname);
                    string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                    File.Move(fname, ErrorFilename(fname));
                }
                lista_nakazow = nakazy.mojeNakazy;


                foreach (NakazOutputElement nakaz in lista_nakazow)
                {
                    LexEnaZadanieEntities lexena = new LexEnaZadanieEntities();
                    nakazStr = nakaz.dokumentXml;
                    SygnWgPowoda = nakaz.sygnaturaWgPowoda;
                    spr = null;
                    Utils.LogNamedWriter(" Start Importu nazaku: " + SygnWgPowoda + " " + nakaz.sygnaturaSprawy, mylogname);
                    if (!String.IsNullOrEmpty(SygnWgPowoda.Trim()) && !SygnWgPowoda.Trim().StartsWith("ENERGA#"))
                    {
                         if (SygnWgPowoda.Contains("#"))
                                idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                            else
                                idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));
                           // dodanie encji pozew
                        spr = (from z in lexena.Sprawa
                               where z.id == idSprawy
                               select z).FirstOrDefault();
                        if (spr == null)
                        {
                            sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                            spr = (from z in lexena.Sprawa
                                   where z.sygnatura == sygnatura
                                   select z).FirstOrDefault();


                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(nakaz.sygnaturaSprawy))
                        {
                            spr = (from z in lexena.Sprawa
                                   where z.SygnNCe.Replace(" ", "").Contains(nakaz.sygnaturaSprawy.Replace(" ", ""))
                                   select z).FirstOrDefault();
                            if (spr != null) SygnWgPowoda = spr.sygnatura;
                        }
                    }
                    if (spr == null)
                    {
                        Utils.LogWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname);
                        Utils.LogNamedWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname, mylogname);
                        fextension = ".XME";
                        continue;
                        //File.Move(fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR");
                       //return -1;
                    }
                    else
                        idSprawy = spr.id;

                    DokOdebr dood = (from z in lexena.DokOdebr
                                     where z.Sprawa_id == idSprawy && z.TypDok == 5
                                     select z).FirstOrDefault();
                    if (dood == null)
                    {
                        dood = new DokOdebr();
                        newDok = true;
                        dokOdebr_id = 0;
                    }
                    else
                    {
                        dokOdebr_id = dood.Id;
                        newDok = false;
                    }

                    dood.d_kreacji = DateTime.Today;
                    dood.DataDokumentu = nakaz.dataOrzeczenia;
                    dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                    dood.CzyEPU = 1;
                    dood.IdEPU = nakaz.id;
                    dood.Nazwa = "Nakaz";
                    dood.TypDok = 5; // nakza zapłaty
                    dood.Sprawa_id = idSprawy;
                    dood.PartitionKey = 0;
                    dood.Format = 100;
                    dood.StatusDok = "wydano";
                    dood.Tresc = nakaz.dokumentXml;
                    dood.TrescHtml = XML2HTMLTransform.TransformNCompress(dood.Tresc, 5);
                    dood.CzyZalatw = 0;
                    if (KontoEpu > 0) dood.KontoEPU_Id = KontoEpu;
                    // czy jest pdf
                    pdf = null;
                    if (!newDok && dokOdebr_id > 0)
                    {
                        pdf = (from z in lexena.PdfStore
                               where z.DokOdebr_Id == dokOdebr_id
                               select z).FirstOrDefault();
                    }

                    if (pdf == null)
                    {

                        pdf = new PdfStore();
                        if (dood.PdfStore == null)
                            dood.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                        dood.PdfStore.Add(pdf);
                        lexena.PdfStore.AddObject(pdf);
                    }

                    pdf.name = "Nakaz zapłaty " + SygnWgPowoda;
                    retvalue = XML2HTMLTransform.html2pdf(dood.TrescHtml, ref pdffile);
                    if (retvalue.Contains("Błąd"))
                    {
                        Utils.LogWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname);
                        Utils.LogNamedWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname, mylogname);
                        File.Move(fname, ErrorFilename(fname));
                        return -1;
                    }
                    pdf.value = pdffile;
                    if (dood.DokumentKomunikacjaEPU == null)
                        dood.DokumentKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentKomunikacjaEPU>();
                    DokumentKomunikacjaEPU dokwyskom = new DokumentKomunikacjaEPU();
                    dokwyskom.czyus = 0;
                    dokwyskom.d_kreacji = DateTime.Now;
                    dokwyskom.Status = 3;
                    dood.DokumentKomunikacjaEPU.Add(dokwyskom);
                    lexena.AddToDokumentKomunikacjaEPU(dokwyskom);

                    // dodanie statusu do sprawy 
                    spr = (from z in lexena.Sprawa
                           where z.id == idSprawy
                           select z).FirstOrDefault();
                    nazstat = (from x in lexena.NazwaStatusu
                               where x.Krok == 4
                               select x).FirstOrDefault();
                    oststat = (from x in lexena.NazwaStatusu
                               join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                               where y.Sprawa_id == idSprawy
                               orderby x.Krok descending
                               select x).FirstOrDefault();


                    if (spr != null && oststat.Krok <= 3)
                    {

                        StatusSprawy stspr = new StatusSprawy();
                        stspr.czyus = 0;
                        stspr.CzyWiena = 0;
                        stspr.DataStatusu = DateTime.Now;
                        stspr.Sprawa_id = spr.id;
                        stspr.NazwaStatusu_Id = nazstat.Id;
                        lexena.AddToStatusSprawy(stspr);
                    }


                    lexena.SaveChanges(0);
                    Utils.LogNamedWriter(" Import OK:  " + SygnWgPowoda, mylogname);
                }
                Utils.LogNamedWriter(" Pozytywny koniec importu: " + fname, mylogname);
                File.Move(fname, DoneFilename(fname));
                return 1;


            }
                catch (System.IO.IOException ioex)
            {
                Utils.LogWriter("Błąd podczas zmiany nazwy zbioru " + ioex.Message + " zbiór" + fname);
                return -200;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania Nakazu " + ex.Message + " zbiór" + fname);
                Utils.LogNamedWriter(" !!!!!! BŁĄD IMPORTU Nakazu: " + SygnWgPowoda + "  kod błędu:" + ex.Message, mylogname);
                File.Move(fname, ErrorFilename(fname));


                return -100;
            }


        }


        public int ReadNakazBest(string fname, int KontoEpu, int jaki)
        {

            MojeNakazyOutputData nakazy;
            NakazOutputElement[] lista_nakazow;
            string sygnatura;
            NazwaStatusu oststat;
            PdfStore pdf;

            String SygnWgPowoda;
            int idSprawy;
            string retvalue;
            decimal Noty;
            byte[] pdffile;
            string nakazStr;
            Sprawa spr;
            bool newDok;
            Nakazy nakazybest;
            NazwaStatusu nazstat;
            int dokOdebr_id;
            pdffile = new byte[0];
            string mylogname;

            mylogname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + logname;

            Utils.LogNamedWriter(" Start Importu zbioru: " + fname, mylogname);
            SygnWgPowoda = "";
            try
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader(fname);
                string thedata = myFile.ReadToEnd();
                thedata = clearnamespaces(thedata);
                myFile.Close();
                nakazybest = (Nakazy)XmlDeserializeFromString(thedata, typeof(Nakazy), null);
                if (nakazybest == null)
                {
                    Utils.LogWriter("Błąd w trakcie wczytywania Nakazu, błąd deserializacji XML zbiór: " + fname);
                    Utils.LogNamedWriter("Błąd w trakcie wczytywania nakazu, błąd deserializacji XML zbiór: " + fname, mylogname);
                    string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                    File.Move(fname, ErrorFilename(fname));
                    return -1;
                }



                for (int i = 0; i < nakazybest.NakazEPU.GetLength(0); i++)
                {
                    NakazEPU mynakaz;
                    LexEnaZadanieEntities lexena = new LexEnaZadanieEntities();
                    mynakaz = nakazybest.NakazEPU[i];
                    nakazStr = SerializeToString(mynakaz, typeof(NakazEPU), true);
                    SygnWgPowoda = mynakaz.SprawaWgPowoda;
                    Utils.LogNamedWriter(" Start Importu nazaku: " + SygnWgPowoda + " " + mynakaz.Sygnatura, mylogname);
                    if (SygnWgPowoda.Contains("#"))
                        idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                    else
                        idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));

                    // dodanie encji pozew
                    spr = (from z in lexena.Sprawa
                           where z.id == idSprawy
                           select z).FirstOrDefault();
                    if (spr == null)
                    {
                        sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                        spr = (from z in lexena.Sprawa
                               where z.sygnatura == sygnatura
                               select z).FirstOrDefault();
                        

                    }
                    if (spr == null)
                    {
                        Utils.LogWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname);
                        Utils.LogNamedWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname, mylogname);
                        File.Move(fname, ErrorFilename(fname));
                        return -1;
                    }
                    else
                        idSprawy = spr.id;

                    DokOdebr dood = (from z in lexena.DokOdebr
                                     where z.Sprawa_id == idSprawy && z.TypDok == 5
                                     select z).FirstOrDefault();
                    if (dood == null)
                    {
                        dood = new DokOdebr();
                        newDok = true;
                        dokOdebr_id = 0;
                    }
                    else
                    {
                        dokOdebr_id = dood.Id;
                        newDok = false;
                    }

                    dood.d_kreacji = DateTime.Today;
                    dood.DataDokumentu = Convert.ToDateTime(mynakaz.dataNakazu);
                    dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                    dood.CzyEPU = 1;
                    dood.IdEPU = (int)mynakaz.ID;
                    dood.Nazwa = "Nakaz";
                    dood.TypDok = 5; // nakza zapłaty
                    dood.Sprawa_id = idSprawy;
                    dood.PartitionKey = 0;
                    dood.Format = 100;
                    dood.StatusDok = "wydano";
                    dood.Tresc = nakazStr;
                    dood.TrescHtml = XML2HTMLTransform.TransformNCompress(dood.Tresc, 5);
                    dood.CzyZalatw = 0;
                    if (KontoEpu > 0) dood.KontoEPU_Id = KontoEpu;
                    // czy jest pdf
                    pdf = null;
                    if (!newDok && dokOdebr_id > 0)
                    {
                        pdf = (from z in lexena.PdfStore
                               where z.DokOdebr_Id == dokOdebr_id
                               select z).FirstOrDefault();
                    }

                    if (pdf == null)
                    {

                        pdf = new PdfStore();
                        if (dood.PdfStore == null)
                            dood.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                        dood.PdfStore.Add(pdf);
                        lexena.PdfStore.AddObject(pdf);
                    }

                    pdf.name = "Nakaz zapłaty " + SygnWgPowoda;
                    retvalue = XML2HTMLTransform.html2pdf(dood.TrescHtml, ref pdffile);
                    if (retvalue.Contains("Błąd"))
                    {
                        Utils.LogWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname);
                        Utils.LogNamedWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname, mylogname);
                        File.Move(fname, ErrorFilename(fname));
                        return -1;
                    }
                    pdf.value = pdffile;
                    if (dood.DokumentKomunikacjaEPU == null)
                        dood.DokumentKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentKomunikacjaEPU>();
                    DokumentKomunikacjaEPU dokwyskom = new DokumentKomunikacjaEPU();
                    dokwyskom.czyus = 0;
                    dokwyskom.d_kreacji = DateTime.Now;
                    dokwyskom.Status = 3;
                    dood.DokumentKomunikacjaEPU.Add(dokwyskom);
                    lexena.AddToDokumentKomunikacjaEPU(dokwyskom);

                    // dodanie statusu do sprawy 
                    spr = (from z in lexena.Sprawa
                           where z.id == idSprawy
                           select z).FirstOrDefault();
                    nazstat = (from x in lexena.NazwaStatusu
                               where x.Krok == 4
                               select x).FirstOrDefault();
                    oststat = (from x in lexena.NazwaStatusu
                               join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                               where y.Sprawa_id == idSprawy
                               orderby x.Krok descending
                               select x).FirstOrDefault();


                    if (spr != null && oststat.Krok <= 3)
                    {

                        StatusSprawy stspr = new StatusSprawy();
                        stspr.czyus = 0;
                        stspr.CzyWiena = 0;
                        stspr.DataStatusu = DateTime.Now;
                        stspr.Sprawa_id = spr.id;
                        stspr.NazwaStatusu_Id = nazstat.Id;
                        lexena.AddToStatusSprawy(stspr);
                    }


                    lexena.SaveChanges(0);
                    Utils.LogNamedWriter(" Import OK:  " + SygnWgPowoda, mylogname);
                }
                Utils.LogNamedWriter(" Pozytywny koniec importu: " + fname, mylogname);
                File.Move(fname, DoneFilename(fname));
                return 1;


            }
        catch (System.IO.IOException ioex)
            {
                Utils.LogWriter("Błąd podczas zmiany nazwy zbioru " + ioex.Message + " zbiór" + fname);
                return -200;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania Nakazu " + ex.Message + " zbiór" + fname);
                Utils.LogNamedWriter(" !!!!!! BŁĄD IMPORTU Nakazu: " + SygnWgPowoda + "  kod błędu:" + ex.Message, mylogname);
                File.Move(fname, ErrorFilename(fname));


                return -100;
            }


        }



        public int ReadOrzeczenie(string fname, int KontoEpu, int jaki)
        {

            MojeOrzeczeniaOutputData orzeczenia;
            OrzeczenieOutputElement[] lista_orzeczen;

            string sygnatura;
            NazwaStatusu oststat;
            PdfStore pdf;
            int kodstat;
            String SygnWgPowoda;
            int idSprawy;
            string retvalue;
            decimal Noty;
            byte[] pdffile;
            string nakazStr;
            Sprawa spr= null;
            bool newDok;
            NazwaStatusu nazstat;
            int dokOdebr_id;
            pdffile = new byte[0];
            string mylogname;

            mylogname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + logname;

            Utils.LogNamedWriter(" Start Importu zbioru: " + fname, mylogname);
            SygnWgPowoda = "";
            try
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader(fname);
                string thedata = myFile.ReadToEnd();
                myFile.Close();
                thedata = clearnamespaces(thedata);
                orzeczenia = (MojeOrzeczeniaOutputData)XmlDeserializeFromString(thedata, typeof(MojeOrzeczeniaOutputData), null);
                if (orzeczenia == null)
                {
                    Utils.LogWriter("Błąd w trakcie wczytywania Postanowienia, błąd deserializacji XML zbiór: " + fname);
                    Utils.LogNamedWriter("Błąd w trakcie wczytywania Postanowienia, błąd deserializacji XML zbiór: " + fname, mylogname);
                    string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                    File.Move(fname, ErrorFilename(fname));
                    return -1;

                }

                lista_orzeczen = orzeczenia.listaOrzeczen;


                foreach (OrzeczenieOutputElement orzecz in lista_orzeczen)
                {
                    LexEnaZadanieEntities lexena = new LexEnaZadanieEntities();
                    nakazStr = orzecz.dokumentXml;
                    SygnWgPowoda = orzecz.sygnaturaWgPowoda;
                    Utils.LogNamedWriter(" Start Importu orzeczenia: " + SygnWgPowoda + " " + orzecz.sygnaturaSprawy, mylogname);
                    if (!String.IsNullOrEmpty(SygnWgPowoda.Trim()) && !SygnWgPowoda.Trim().StartsWith("ENERGA#"))
                    {
                        if (SygnWgPowoda.Contains("#"))
                            idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                        else
                            idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));

                        // dodanie encji pozew
                        spr = (from z in lexena.Sprawa
                               where z.id == idSprawy
                               select z).FirstOrDefault();
                        if (spr == null)
                        {
                            sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                            spr = (from z in lexena.Sprawa
                                   where z.sygnatura == sygnatura
                                   select z).FirstOrDefault();


                        }
                    }
                    else 
                    {
                        if (!String.IsNullOrEmpty(orzecz.sygnaturaSprawy))
                        {
                            spr = (from z in lexena.Sprawa
                                   where z.SygnNCe.Replace(" ", "").Contains(orzecz.sygnaturaSprawy.Replace(" ", ""))
                                   select z).FirstOrDefault();
                            if (spr != null) SygnWgPowoda = spr.sygnatura;
                        }     
                    }
                    if (spr == null)
                    {
                        Utils.LogWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname);
                        Utils.LogNamedWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname, mylogname);
                        File.Move(fname, ErrorFilename(fname));
                        return -1;
                    }
                    else
                        idSprawy = spr.id;
                    DokOdebr dood = (from z in lexena.DokOdebr
                                     where z.Sprawa_id == idSprawy && z.TypDok == 101 && z.Nazwa == orzecz.nazwaDecyzji && z.DataDokumentu == orzecz.dataOrzeczenia
                                     select z).FirstOrDefault();
                    if (dood == null)
                    {
                        dood = new DokOdebr();
                        newDok = true;
                        dokOdebr_id = 0;
                    }
                    else
                    {
                        dokOdebr_id = dood.Id;
                        newDok = false;
                    }

                    dood.d_kreacji = DateTime.Today;
                    dood.DataDokumentu = orzecz.dataOrzeczenia;
                    dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                    dood.CzyEPU = 1;
                    dood.IdEPU = orzecz.id;
                    dood.Nazwa = orzecz.nazwaDecyzji;
                    dood.TypDok = 101;
                    dood.Sprawa_id = idSprawy;
                    dood.PartitionKey = 0;
                    dood.Format = 100;
                    dood.StatusDok = "wydano";
                    dood.Tresc = orzecz.dokumentXml;
                    dood.TrescHtml = XML2HTMLTransform.TransformNCompress(dood.Tresc, 101);
                    dood.CzyZalatw = 0;
                    if (KontoEpu > 0) dood.KontoEPU_Id = KontoEpu;
                    // czy jest pdf
                    pdf = null;
                    if (!newDok && dokOdebr_id > 0)
                    {
                        pdf = (from z in lexena.PdfStore
                               where z.DokOdebr_Id == dokOdebr_id
                               select z).FirstOrDefault();
                    }

                    if (pdf == null)
                    {

                        pdf = new PdfStore();
                        if (dood.PdfStore == null)
                            dood.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                        dood.PdfStore.Add(pdf);
                        lexena.PdfStore.AddObject(pdf);
                    }

                    pdf.name = orzecz.nazwaDecyzji + " " + SygnWgPowoda;
                    pdf.name = pdf.name.Substring(0, (pdf.name.Length > 100 ? 100 : pdf.name.Length));
                    retvalue = XML2HTMLTransform.html2pdf(dood.TrescHtml, ref pdffile);
                    if (retvalue.Contains("Błąd"))
                    {
                        Utils.LogWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname);
                        Utils.LogNamedWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname, mylogname);
                        File.Move(fname, ErrorFilename(fname));
                        return -1;
                    }
                    pdf.value = pdffile;
                    if (dood.DokumentKomunikacjaEPU == null)
                        dood.DokumentKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentKomunikacjaEPU>();
                    DokumentKomunikacjaEPU dokwyskom = new DokumentKomunikacjaEPU();
                    dokwyskom.czyus = 0;
                    dokwyskom.d_kreacji = DateTime.Now;
                    dokwyskom.Status = 3;
                    dood.DokumentKomunikacjaEPU.Add(dokwyskom);
                    //lexena.AddToDokumentKomunikacjaEPU(dokwyskom);



                    if ((orzecz.nazwaDecyzji.ToLower().Contains("podsta") && orzecz.nazwaDecyzji.ToLower().Contains("przekaza")) || (orzecz.nazwaDecyzji.ToLower().Contains("uchyl") && orzecz.nazwaDecyzji.ToLower().Contains("nakaz")))
                        kodstat = 12;
                    else if (orzecz.nazwaDecyzji.ToLower().Contains("sprzeci") && orzecz.nazwaDecyzji.ToLower().Contains("przekaza"))
                        kodstat = 11;
                    else if (orzecz.nazwaDecyzji.ToLower().Contains("umorzen"))
                        kodstat = 13;
                    else if ((orzecz.nazwaDecyzji.ToLower().Contains("zwroci") || orzecz.nazwaDecyzji.ToLower().Contains("odrzuc")) && orzecz.nazwaDecyzji.ToLower().Contains("pozw"))
                        kodstat = 14;
                    else kodstat = 0;
                    if (kodstat > 0)
                    {
                        // dodanie statusu do sprawy 
                        spr = (from z in lexena.Sprawa
                               where z.id == idSprawy
                               select z).FirstOrDefault();
                        nazstat = (from x in lexena.NazwaStatusu
                                   where x.Krok == kodstat
                                   select x).FirstOrDefault();
                        oststat = (from x in lexena.NazwaStatusu
                                   join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                   where y.Sprawa_id == idSprawy
                                   orderby y.DataStatusu descending
                                   select x).FirstOrDefault();


                        if (spr != null && oststat.Krok < kodstat)
                        {

                            StatusSprawy stspr = new StatusSprawy();
                            stspr.czyus = 0;
                            stspr.CzyWiena = 0;
                            stspr.DataStatusu = DateTime.Now;
                            stspr.Sprawa_id = spr.id;
                            stspr.NazwaStatusu_Id = nazstat.Id;
                            lexena.AddToStatusSprawy(stspr);
                        }

                    }


                    lexena.SaveChanges(0);
                    Utils.LogNamedWriter(" Import OK:  " + SygnWgPowoda, mylogname);
                }
                Utils.LogNamedWriter(" Pozytywny koniec importu: " + fname, mylogname);
                File.Move(fname, DoneFilename(fname));
                return 1;


            }
            catch (System.IO.IOException ioex)
            {
                Utils.LogWriter("Błąd podczas zmiany nazwy zbioru " + ioex.Message + " zbiór" + fname);
                return -200;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania orzeczenia " + ex.Message + " zbiór" + fname);
                Utils.LogNamedWriter(" !!!!!! BŁĄD IMPORTU Orzeczenia: " + SygnWgPowoda + "  kod błędu:" + ex.Message, mylogname);
                File.Move(fname, ErrorFilename(fname));


                return -100;
            }


        }

        public int ReadKlauzula(string fname, int KontoEpu, int jaki)
        {

            MojeOrzeczeniaOutputData orzeczenia;
            OrzeczenieOutputElement[] lista_orzeczen;

            string sygnatura;
            NazwaStatusu oststat;
            PdfStore pdf;
            int kodstat;
            String SygnWgPowoda;
            int idSprawy;
            string retvalue;
            decimal Noty;
            byte[] pdffile;
            string nakazStr;
            Sprawa spr;
            bool newDok;
            NazwaStatusu nazstat;
            int dokOdebr_id;
            pdffile = new byte[0];
            string mylogname;
            string fextension = ".XM_";

            mylogname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + logname;

            Utils.LogNamedWriter(" Start Importu zbioru: " + fname, mylogname);
            SygnWgPowoda = "";
            try
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader(fname);
                string thedata = myFile.ReadToEnd();
                myFile.Close();
                thedata = clearnamespaces(thedata);
                orzeczenia = (MojeOrzeczeniaOutputData)XmlDeserializeFromString(thedata, typeof(MojeOrzeczeniaOutputData), null);
                if (orzeczenia == null)
                {
                    Utils.LogWriter("Błąd w trakcie wczytywania Klauzul, błąd deserializacji XML zbiór: " + fname);
                    Utils.LogNamedWriter("Błąd w trakcie wczytywania Klauzul, błąd deserializacji XML zbiór: " + fname, mylogname);
                    string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                    File.Move(fname, ErrorFilename(fname));
                    return -1;

                }

                lista_orzeczen = orzeczenia.listaOrzeczen;


                foreach (OrzeczenieOutputElement orzecz in lista_orzeczen)
                {
                    LexEnaZadanieEntities lexena = new LexEnaZadanieEntities();
                    nakazStr = orzecz.dokumentXml;
                    SygnWgPowoda = orzecz.sygnaturaWgPowoda;
                    spr = null;
                if (!String.IsNullOrEmpty(SygnWgPowoda.Trim()) && !SygnWgPowoda.Trim().StartsWith("ENERGA#"))
                  {
                        Utils.LogNamedWriter(" Start Importu orzeczenia: " + SygnWgPowoda + " " + orzecz.sygnaturaSprawy, mylogname);
                        if (SygnWgPowoda.Contains("#"))
                            idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                        else
                            idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));

                        // dodanie encji pozew
                        spr = (from z in lexena.Sprawa
                               where z.id == idSprawy
                               select z).FirstOrDefault();
                        if (spr == null)
                        {
                            sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                            spr = (from z in lexena.Sprawa
                                   where z.sygnatura == sygnatura
                                   select z).FirstOrDefault();

                            
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(orzecz.sygnaturaSprawy))
                        {
                            spr = (from z in lexena.Sprawa
                                   where z.SygnNCe.Replace(" ", "").Contains(orzecz.sygnaturaSprawy.Replace(" ", ""))
                                   select z).FirstOrDefault();
                            if (spr != null) SygnWgPowoda = spr.sygnatura;
                        }               
                    }


                   
                    if (spr == null)
                    {
                        Utils.LogWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname);
                        Utils.LogNamedWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname, mylogname);
                        fextension = ".XME";
                        continue;
                        //File.Move(fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR");
                        //return -1;
                    }
                    else
                        idSprawy = spr.id;
                    DokOdebr dood = (from z in lexena.DokOdebr
                                     where z.Sprawa_id == idSprawy && z.TypDok == 17 && z.Nazwa == orzecz.nazwaDecyzji && z.DataDokumentu == orzecz.dataOrzeczenia
                                     select z).FirstOrDefault();
                    if (dood == null)
                    {
                        dood = new DokOdebr();
                        newDok = true;
                        dokOdebr_id = 0;
                    }
                    else
                    {
                        dokOdebr_id = dood.Id;
                        newDok = false;
                    }

                    dood.d_kreacji = DateTime.Today;
                    dood.DataDokumentu = orzecz.dataOrzeczenia;
                    dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                    dood.CzyEPU = 1;
                    dood.IdEPU = orzecz.id;
                    dood.Nazwa = orzecz.nazwaDecyzji;
                    dood.TypDok = 17;
                    dood.Sprawa_id = idSprawy;
                    dood.PartitionKey = 0;
                    dood.Format = 100;
                    dood.StatusDok = "wydano";
                    dood.Tresc = orzecz.dokumentXml;
                    dood.TrescHtml = XML2HTMLTransform.TransformNCompress(dood.Tresc, 101);
                    dood.CzyZalatw = 0;
                    if (KontoEpu > 0) dood.KontoEPU_Id = KontoEpu;
                    // czy jest pdf
                    pdf = null;
                    if (!newDok && dokOdebr_id > 0)
                    {
                        pdf = (from z in lexena.PdfStore
                               where z.DokOdebr_Id == dokOdebr_id
                               select z).FirstOrDefault();
                    }

                    if (pdf == null)
                    {

                        pdf = new PdfStore();
                        if (dood.PdfStore == null)
                            dood.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                        dood.PdfStore.Add(pdf);
                        lexena.PdfStore.AddObject(pdf);
                    }

                    pdf.name = orzecz.nazwaDecyzji + " " + SygnWgPowoda;
                    pdf.name = pdf.name.Substring(0, (pdf.name.Length > 100 ? 100 : pdf.name.Length));
                    retvalue = XML2HTMLTransform.html2pdf(dood.TrescHtml, ref pdffile);
                    if (retvalue.Contains("Błąd"))
                    {
                        Utils.LogWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname);
                        Utils.LogNamedWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname, mylogname);
                        File.Move(fname, ErrorFilename(fname));
                        return -1;
                    }
                    pdf.value = pdffile;
                    if (dood.DokumentKomunikacjaEPU == null)
                        dood.DokumentKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentKomunikacjaEPU>();
                    DokumentKomunikacjaEPU dokwyskom = new DokumentKomunikacjaEPU();
                    dokwyskom.czyus = 0;
                    dokwyskom.d_kreacji = DateTime.Now;
                    dokwyskom.Status = 3;
                    dood.DokumentKomunikacjaEPU.Add(dokwyskom);
                    //lexena.AddToDokumentKomunikacjaEPU(dokwyskom);
                    kodstat = 5;
                    
                        // dodanie statusu do sprawy 
                        spr = (from z in lexena.Sprawa
                               where z.id == idSprawy
                               select z).FirstOrDefault();
                        nazstat = (from x in lexena.NazwaStatusu
                                   where x.Krok == kodstat
                                   select x).FirstOrDefault();
                        oststat = (from x in lexena.NazwaStatusu
                                   join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                   where y.Sprawa_id == idSprawy
                                   orderby y.DataStatusu descending
                                   select x).FirstOrDefault();


                        if (spr != null && oststat.Krok < kodstat)
                        {

                            StatusSprawy stspr = new StatusSprawy();
                            stspr.czyus = 0;
                            stspr.CzyWiena = 0;
                            stspr.DataStatusu = DateTime.Now;
                            stspr.Sprawa_id = spr.id;
                            stspr.NazwaStatusu_Id = nazstat.Id;
                            lexena.AddToStatusSprawy(stspr);
                        }

                    


                    lexena.SaveChanges(0);
                    Utils.LogNamedWriter(" Import OK:  " + SygnWgPowoda, mylogname);
                }
                Utils.LogNamedWriter(" Pozytywny koniec importu: " + fname, mylogname);
                File.Move(fname, DoneFilename(fname) );
                return 1;


            }
            catch (System.IO.IOException ioex)
            {
                Utils.LogWriter("Błąd podczas zmiany nazwy zbioru " + ioex.Message + " zbiór" + fname);
                return -200;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania klauzuli " + ex.Message + " zbiór" + fname);
                Utils.LogNamedWriter(" !!!!!! BŁĄD IMPORTU klauzuli: " + SygnWgPowoda + "  kod błędu:" + ex.Message, mylogname);
                File.Move(fname, ErrorFilename(fname));


                return -100;
            }


        }



        public int ReadOrzeczenieBest(string fname, int KontoEpu, int jaki)
        {

            Postanowienia orzeczenia;

            string sygnatura;
            NazwaStatusu oststat;
            PdfStore pdf;
            int kodstat;
            string SygnWgPowoda;
            string nazwaorzeczenia;
            int idSprawy;
            string retvalue;
            decimal Noty;
            byte[] pdffile;
            string nakazStr;
            Sprawa spr;
            bool newDok;
            NazwaStatusu nazstat;
            int dokOdebr_id;
            pdffile = new byte[0];
            string mylogname;
            KodyDecyzji kdec = new KodyDecyzji();

            mylogname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + logname;
            Utils.LogNamedWriter(" Start Importu zbioru: " + fname, mylogname);
            SygnWgPowoda = "";
            try
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader(fname);
                string thedata = myFile.ReadToEnd();
                myFile.Close();
                thedata = clearnamespaces(thedata);
                orzeczenia = (Postanowienia)XmlDeserializeFromString(thedata, typeof(Postanowienia), null);
                if (orzeczenia == null)
                {
                    Utils.LogWriter("Błąd w trakcie wczytywania Postanowienia, błąd deserializacji XML zbiór: " + fname);
                    Utils.LogNamedWriter("Błąd w trakcie wczytywania Postanowienia, błąd deserializacji XML zbiór: " + fname, mylogname);
                    string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                    File.Move(fname, ErrorFilename(fname));
                    return -1;

                }





                for (int i = 0; i < orzeczenia.OrzeczenieEPU.GetLength(0); i++)
                {
                    OrzeczenieEPU myorzecz;
                    LexEnaZadanieEntities lexena = new LexEnaZadanieEntities();
                    myorzecz = orzeczenia.OrzeczenieEPU[i];
                    nakazStr = SerializeToString(myorzecz, typeof(OrzeczenieEPU), true);
                    SygnWgPowoda = myorzecz.SprawaWgPowoda;
                    Utils.LogNamedWriter(" Start Importu orzeczenia: " + SygnWgPowoda + " " + myorzecz.Sygnatura, mylogname);
                    if (SygnWgPowoda.Contains("#"))
                        idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                    else
                        idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));

                    // dodanie encji pozew
                    spr = (from z in lexena.Sprawa
                           where z.id == idSprawy
                           select z).FirstOrDefault();
                    if (spr == null)
                    {
                        sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                        spr = (from z in lexena.Sprawa
                               where z.sygnatura == sygnatura
                               select z).FirstOrDefault();
                     

                    }
                    if (spr == null)
                    {
                        Utils.LogWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname);
                        Utils.LogNamedWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname, mylogname);
                        File.Move(fname, ErrorFilename(fname));
                        return -1;
                    }
                    else
                        idSprawy = spr.id;
                    DateTime dOrzecz;
                    dOrzecz = Convert.ToDateTime(myorzecz.dataOrzeczenia);
                    nazwaorzeczenia = kdec.GetDecName(myorzecz.kodDecyzji);
                    if (nazwaorzeczenia == null)
                        nazwaorzeczenia = myorzecz.nazwaOrzeczenia;


                    DokOdebr dood = (from z in lexena.DokOdebr
                                     where z.Sprawa_id == idSprawy && z.TypDok == 101 && z.Nazwa == nazwaorzeczenia && z.DataDokumentu == dOrzecz
                                     select z).FirstOrDefault();
                    if (dood == null)
                    {
                        dood = new DokOdebr();
                        newDok = true;
                        dokOdebr_id = 0;
                    }
                    else
                    {
                        dokOdebr_id = dood.Id;
                        newDok = false;
                    }

                    dood.d_kreacji = DateTime.Today;
                    dood.DataDokumentu = dOrzecz;
                    dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                    dood.CzyEPU = 1;
                    dood.IdEPU = (int)myorzecz.ID;
                    dood.Nazwa = nazwaorzeczenia;
                    dood.TypDok = 101;
                    dood.Sprawa_id = idSprawy;
                    dood.PartitionKey = 0;
                    dood.Format = 100;
                    dood.StatusDok = "wydano";
                    dood.Tresc = nakazStr;
                    dood.TrescHtml = XML2HTMLTransform.TransformNCompress(dood.Tresc, 101);
                    dood.CzyZalatw = 0;
                    if (KontoEpu > 0) dood.KontoEPU_Id = KontoEpu;
                    // czy jest pdf
                    pdf = null;
                    if (!newDok && dokOdebr_id > 0)
                    {
                        pdf = (from z in lexena.PdfStore
                               where z.DokOdebr_Id == dokOdebr_id
                               select z).FirstOrDefault();
                    }

                    if (pdf == null)
                    {

                        pdf = new PdfStore();
                        if (dood.PdfStore == null)
                            dood.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                        dood.PdfStore.Add(pdf);
                        lexena.PdfStore.AddObject(pdf);
                    }

                    pdf.name = nazwaorzeczenia + " " + SygnWgPowoda;    // <---  do poprawy
                    pdf.name = pdf.name.Substring(0, (pdf.name.Length > 100 ? 100 : pdf.name.Length));
                    retvalue = XML2HTMLTransform.html2pdf(dood.TrescHtml, ref pdffile);
                    if (retvalue.Contains("Błąd"))
                    {
                        Utils.LogWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname);
                        Utils.LogNamedWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname, mylogname);
                        File.Move(fname, ErrorFilename(fname));
                        return -1;
                    }
                    pdf.value = pdffile;
                    if (dood.DokumentKomunikacjaEPU == null)
                        dood.DokumentKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentKomunikacjaEPU>();
                    DokumentKomunikacjaEPU dokwyskom = new DokumentKomunikacjaEPU();
                    dokwyskom.czyus = 0;
                    dokwyskom.d_kreacji = DateTime.Now;
                    dokwyskom.Status = 3;
                    dood.DokumentKomunikacjaEPU.Add(dokwyskom);
                    //lexena.AddToDokumentKomunikacjaEPU(dokwyskom);



                    if ((dood.Nazwa.ToLower().Contains("podsta") && dood.Nazwa.ToLower().Contains("przekaza")) || (dood.Nazwa.ToLower().Contains("uchyl") && dood.Nazwa.ToLower().Contains("nakaz")))
                        kodstat = 12;
                    else if (dood.Nazwa.ToLower().Contains("sprzeci") && dood.Nazwa.ToLower().Contains("przekaza"))
                        kodstat = 11;
                    else if (dood.Nazwa.ToLower().Contains("umorzen"))
                        kodstat = 13;
                    else if ((dood.Nazwa.ToLower().Contains("zwroci") || dood.Nazwa.ToLower().Contains("odrzuc")) && dood.Nazwa.ToLower().Contains("pozw"))
                        kodstat = 14;
                    else kodstat = 0;
                    if (kodstat > 0)
                    {
                        // dodanie statusu do sprawy 
                        spr = (from z in lexena.Sprawa
                               where z.id == idSprawy
                               select z).FirstOrDefault();
                        nazstat = (from x in lexena.NazwaStatusu
                                   where x.Krok == kodstat
                                   select x).FirstOrDefault();
                        oststat = (from x in lexena.NazwaStatusu
                                   join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                   where y.Sprawa_id == idSprawy
                                   orderby y.DataStatusu descending
                                   select x).FirstOrDefault();


                        if (spr != null && oststat.Krok < kodstat)
                        {

                            StatusSprawy stspr = new StatusSprawy();
                            stspr.czyus = 0;
                            stspr.CzyWiena = 0;
                            stspr.DataStatusu = DateTime.Now;
                            stspr.Sprawa_id = spr.id;
                            stspr.NazwaStatusu_Id = nazstat.Id;
                            lexena.AddToStatusSprawy(stspr);
                        }

                    }


                    lexena.SaveChanges(0);
                    Utils.LogNamedWriter(" Import OK:  " + SygnWgPowoda, mylogname);
                }
                Utils.LogNamedWriter(" Pozytywny koniec importu: " + fname, mylogname);
                File.Move(fname, DoneFilename(fname));
                return 1;


            }
            catch (System.IO.IOException ioex)
            {
                Utils.LogWriter("Błąd podczas zmiany nazwy zbioru " + ioex.Message + " zbiór" + fname);
                return -200;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania orzeczenia " + ex.Message + " zbiór" + fname);
                Utils.LogNamedWriter(" !!!!!! BŁĄD IMPORTU Orzeczenia: " + SygnWgPowoda + "  kod błędu:" + ex.Message, mylogname);
                File.Move(fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR");


                return -100;
            }


        }

        public int ReadKlauzulaBest(string fname, int KontoEpu, int jaki)
        {

            Postanowienia orzeczenia;

            string sygnatura;
            NazwaStatusu oststat;
            PdfStore pdf;
            int kodstat;
            string SygnWgPowoda;
            string nazwaorzeczenia;
            int idSprawy;
            string retvalue;
            decimal Noty;
            byte[] pdffile;
            string nakazStr;
            Sprawa spr;
            bool newDok;
            NazwaStatusu nazstat;
            int dokOdebr_id;
            pdffile = new byte[0];
            string mylogname;
            KodyDecyzji kdec = new KodyDecyzji();

            mylogname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + logname;
            Utils.LogNamedWriter(" Start Importu zbioru: " + fname, mylogname);
            SygnWgPowoda = "";
            try
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader(fname);
                string thedata = myFile.ReadToEnd();
                myFile.Close();
                thedata = clearnamespaces(thedata);
                orzeczenia = (Postanowienia)XmlDeserializeFromString(thedata, typeof(Postanowienia), null);
                if (orzeczenia == null)
                {
                    Utils.LogWriter("Błąd w trakcie wczytywania Postanowienia, błąd deserializacji XML zbiór: " + fname);
                    Utils.LogNamedWriter("Błąd w trakcie wczytywania Postanowienia, błąd deserializacji XML zbiór: " + fname, mylogname);
                    string newname = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR";
                    File.Move(fname, ErrorFilename(fname));
                    return -1;

                }





                for (int i = 0; i < orzeczenia.OrzeczenieEPU.GetLength(0); i++)
                {
                    OrzeczenieEPU myorzecz;
                    LexEnaZadanieEntities lexena = new LexEnaZadanieEntities();
                    myorzecz = orzeczenia.OrzeczenieEPU[i];
                    nakazStr = SerializeToString(myorzecz, typeof(OrzeczenieEPU), true);
                    SygnWgPowoda = myorzecz.SprawaWgPowoda;
                    Utils.LogNamedWriter(" Start Importu orzeczenia: " + SygnWgPowoda + " " + myorzecz.Sygnatura, mylogname);
                    if (SygnWgPowoda.Contains("#"))
                        idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                    else
                        idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));

                    // dodanie encji pozew
                    spr = (from z in lexena.Sprawa
                           where z.id == idSprawy
                           select z).FirstOrDefault();
                    if (spr == null)
                    {
                        sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                        spr = (from z in lexena.Sprawa
                               where z.sygnatura == sygnatura
                               select z).FirstOrDefault();
                    

                    }
                    if (spr == null)
                    {
                        Utils.LogWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname);
                        Utils.LogNamedWriter("Błąd w trakcie wczytywania Dokumentu, brak sprawy w LexEna  " + SygnWgPowoda + " zbiór" + fname, mylogname);
                        File.Move(fname, ErrorFilename(fname));
                        return -1;
                    }
                    else
                        idSprawy = spr.id;
                    DateTime dOrzecz;
                    dOrzecz = Convert.ToDateTime(myorzecz.dataOrzeczenia);
                    nazwaorzeczenia = kdec.GetDecName(myorzecz.kodDecyzji);
                    if (nazwaorzeczenia == null)
                        nazwaorzeczenia = myorzecz.nazwaOrzeczenia;


                    DokOdebr dood = (from z in lexena.DokOdebr
                                     where z.Sprawa_id == idSprawy && z.TypDok == 17 && z.DataDokumentu == dOrzecz
                                     select z).FirstOrDefault();
                    if (dood == null)
                    {
                        dood = new DokOdebr();
                        newDok = true;
                        dokOdebr_id = 0;
                    }
                    else
                    {
                        dokOdebr_id = dood.Id;
                        newDok = false;
                    }

                    dood.d_kreacji = DateTime.Today;
                    dood.DataDokumentu = dOrzecz;
                    dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                    dood.CzyEPU = 1;
                    dood.IdEPU = (int)myorzecz.ID;
                    dood.Nazwa = nazwaorzeczenia;
                    dood.TypDok = 17;
                    dood.Sprawa_id = idSprawy;
                    dood.PartitionKey = 0;
                    dood.Format = 100;
                    dood.StatusDok = "wydano";
                    dood.Tresc = nakazStr;
                    dood.TrescHtml = XML2HTMLTransform.TransformNCompress(dood.Tresc, 101);
                    dood.CzyZalatw = 0;
                    if (KontoEpu > 0) dood.KontoEPU_Id = KontoEpu;
                    // czy jest pdf
                    pdf = null;
                    if (!newDok && dokOdebr_id > 0)
                    {
                        pdf = (from z in lexena.PdfStore
                               where z.DokOdebr_Id == dokOdebr_id
                               select z).FirstOrDefault();
                    }

                    if (pdf == null)
                    {

                        pdf = new PdfStore();
                        if (dood.PdfStore == null)
                            dood.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                        dood.PdfStore.Add(pdf);
                        lexena.PdfStore.AddObject(pdf);
                    }

                    pdf.name = nazwaorzeczenia + " " + SygnWgPowoda;    // <---  do poprawy
                    pdf.name = pdf.name.Substring(0, (pdf.name.Length > 100 ? 100 : pdf.name.Length));
                    retvalue = XML2HTMLTransform.html2pdf(dood.TrescHtml, ref pdffile);
                    if (retvalue.Contains("Błąd"))
                    {
                        Utils.LogWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname);
                        Utils.LogNamedWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda + "zbiór " + fname, mylogname);
                        File.Move(fname, ErrorFilename(fname));
                        return -1;
                    }
                    pdf.value = pdffile;
                    if (dood.DokumentKomunikacjaEPU == null)
                        dood.DokumentKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentKomunikacjaEPU>();
                    DokumentKomunikacjaEPU dokwyskom = new DokumentKomunikacjaEPU();
                    dokwyskom.czyus = 0;
                    dokwyskom.d_kreacji = DateTime.Now;
                    dokwyskom.Status = 3;
                    dood.DokumentKomunikacjaEPU.Add(dokwyskom);
                    //lexena.AddToDokumentKomunikacjaEPU(dokwyskom);
                    kodstat = 5; // Tytuł Wykonawczy
                    
                        // dodanie statusu do sprawy 
                        spr = (from z in lexena.Sprawa
                               where z.id == idSprawy
                               select z).FirstOrDefault();
                        nazstat = (from x in lexena.NazwaStatusu
                                   where x.Krok == kodstat
                                   select x).FirstOrDefault();
                        oststat = (from x in lexena.NazwaStatusu
                                   join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                   where y.Sprawa_id == idSprawy
                                   orderby y.DataStatusu descending
                                   select x).FirstOrDefault();


                        if (spr != null && oststat.Krok < kodstat)
                        {

                            StatusSprawy stspr = new StatusSprawy();
                            stspr.czyus = 0;
                            stspr.CzyWiena = 0;
                            stspr.DataStatusu = DateTime.Now;
                            stspr.Sprawa_id = spr.id;
                            stspr.NazwaStatusu_Id = nazstat.Id;
                            lexena.AddToStatusSprawy(stspr);
                        }

                    

                    lexena.SaveChanges(0);
                    Utils.LogNamedWriter(" Import OK:  " + SygnWgPowoda, mylogname);
                }
                Utils.LogNamedWriter(" Pozytywny koniec importu: " + fname, mylogname);
                File.Move(fname, DoneFilename(fname));
                return 1;


            }
            catch (System.IO.IOException ioex)
            {
                Utils.LogWriter("Błąd podczas zmiany nazwy zbioru " + ioex.Message + " zbiór" + fname);
                return -200;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania orzeczenia " + ex.Message + " zbiór" + fname);
                Utils.LogNamedWriter(" !!!!!! BŁĄD IMPORTU Orzeczenia: " + SygnWgPowoda + "  kod błędu:" + ex.Message, mylogname);
                File.Move(fname, ErrorFilename(fname));


                return -100;
            }


        }

    }
}

  





