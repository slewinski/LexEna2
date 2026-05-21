using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using ZadanieTimerMain;



namespace ZadanieTimer
{

    public class TypSlownikFiltered
    {
        public string Nazwa { get; set; }
        public int Numer { get; set; }
        public int Filter1 { get; set; }
        public int Filter2 { get; set; }
        public int Filter3 { get; set; }

        public TypSlownikFiltered()
        {

        }

        public TypSlownikFiltered(string s, int n, int f1, int f2, int f3)
        {
            Nazwa = s;
            Numer = n;
            Filter1 = f1;
            Filter2 = f2;
            Filter3 = f3;
        }

    }

    public  class ZadanieTimerMain
    {
       public  bool zadanieWToku = false;

       
        private static void DoTest3()
        {
            using (var lexena = new LexEnaZadanieEntities())
            {
                var pojDokWysPrzej = from z in lexena.DokWys    select z;
                foreach (DokWys dok in pojDokWysPrzej)
                {
                    dok.TrescHtml = XML2HTMLTransform.TransformNCompress(dok.Tresc,0);
                }
                lexena.SaveChanges();
                
            }
        }





        private static void DoTest2()
        {
            ZadanieSet zadanie = new ZadanieSet();
            using (var lexena = new LexEnaZadanieEntities())
            {
                zadanie.DataRozpoczęcia = new DateTime(2012,1,18);
                zadanie.DataZakonczenia = new DateTime(2012, 1, 19);
                zadanie.Oczasie = true;
                zadanie.NazwaZadania = EpuZadania.MojeSprawy.ToString();
                zadanie.TypZadaniaId = 3;
                zadanie.Parametry = DoSerializeToXML(new EPUParamModel { DataDo = DateTime.Today, DataOd = DateTime.Today.Add(new TimeSpan(1, 0, 0, 0)), KryteriumFiltrowania = null, FiltrSlowny = null, KontoEpuId = 1});
                lexena.AddToZadanieSet(zadanie);
                lexena.SaveChanges();
            }
        
        }

        private static string DoSerializeToXML(EPUParamModel epuParamModel)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EPUParamModel));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, epuParamModel);
            return stringWriter.ToString();
        }

    private static void DoTest()
    {
    StreamReader streamReader = new StreamReader("xml.txt");
    string resultHtml; 
    string text = streamReader.ReadToEnd();
    streamReader.Close();

    resultHtml = XML2HTMLTransform.TransformNCompress(text,0);
    using (var lexena = new LexEnaZadanieEntities())
    {
        var resultquery = from z in lexena.DokWys
                                 
                                  select z ; //).FirstOrDefault();

        foreach (var r in resultquery)
        {
            r.TrescHtml = resultHtml; 
        }

        lexena.SaveChanges();
    }
    
    }
    public static List<TypSlownikFiltered>  OneTimedEvent()
    {

            int idModel = -1;
            int idModel2return = -1;
            List< ZadanieSet > lst = new List<ZadanieSet>();
            List<TypSlownikFiltered> l = new List<TypSlownikFiltered>();
            try
            {
                using (var zadania = new LexEnaZadanieEntities())
                {

                    /*
                    int czyZadania = (from z in zadania.ZadanieSet
                                  where z.Status.Equals(100)
                                  where z.Oczasie == false
                                  select z).Count();
                    */

                    var query = from p in zadania.ZadanieSet
                            where p.Status.Equals(0) || p.Status.Equals(100)
                            where p.Oczasie == false
                            select p;
                    if (query.Count() > 0)
                    {
                        foreach (var zadanie in query)
                        {
                            zadanie.Status = 100;
                            zadanie.Opis = "W trakcie wykonywania";
                            lst.Add(zadanie);
                        }
                        zadania.SaveChanges();

                       
                    }
                }

                if (lst != null && lst.Count() > 0)
                {

                    foreach (var zadanie in lst)
                    {
                        string ops = "";
                        Utils.LogWriter("Znalazłem zadanie:" + zadanie.NazwaZadania.ToString());
                        //zadanie.Status = 100;
                        Utils.LogWriter("Wykonuje zadanie");
                        //System.Threading.Thread.Sleep(1000);
                        int stat = RunTask.WykonajZadanie(zadanie.NazwaZadania.ToString(), zadanie.TypZadaniaId, ref ops, zadanie.Parametry, zadanie.Id, ref idModel);

                        using (var context = new LexEnaZadanieEntities())
                        {
                            ZadanieSet zd = context.ZadanieSet.Where(z => z.Id == zadanie.Id).FirstOrDefault();
                            if (zd != null)
                            {
                                zd.Status = stat;
                                zd.Opis = ops;
                                zd.DataZakonczenia = DateTime.Now;
                                if (stat > 0)
                                {
                                    TypSlownikFiltered t = new TypSlownikFiltered();
                                    t.Numer = idModel;
                                    t.Filter1 = zadanie.TypZadaniaId;
                                    l.Add(t);
                                }
                                    
                                context.SaveChanges();
                            }
                        }

                    }
               

                    }

                    Utils.LogWriter(DateTime.Now.ToShortTimeString());
                    return l;
             
        }
        catch(Exception ex)
        {
            Utils.LogWriter("Błąd Timera - wymagany kontakt z twórcą oprogramowania :"+ex.ToString());
                return l;
        }
        }
    public static void OnTimedEvent()
    {
        int idModel = -1;
        using (var zadania = new LexEnaZadanieEntities())
        {
            int czyZadania = (from z in zadania.ZadanieSet
                              where z.Status.Equals(100)
                              select z).Count();
            
            if (czyZadania == 0)
            {
                var query = from p in zadania.ZadanieSet
                            where p.Status.Equals(0) 
                            where p.Oczasie == true
                            where p.DataRozpoczęcia < DateTime.Now
                            select p;
                if (query.Count() > 0)
                {
                    foreach (var zadanie in query)
                    {
                        zadanie.Status = 100;
                        zadanie.Opis = "W trakcie wykonywania";
                    }
                    zadania.SaveChanges();

                    query = from p in zadania.ZadanieSet
                            where p.Status.Equals(100)
                            select p;

                    foreach (var zadanie in query)
                    {
                        string ops = "";
                        Utils.LogWriter("Znalazłem zadanie:" + zadanie.NazwaZadania.ToString());
                        //zadanie.Status = 100;
                        Utils.LogWriter("Wykonuje zadanie");
                        System.Threading.Thread.Sleep(1000);
                        zadanie.Status = RunTask.WykonajZadanie(zadanie.NazwaZadania.ToString(), zadanie.TypZadaniaId, ref ops, zadanie.Parametry, zadanie.Id, ref idModel);
                        zadanie.Opis = ops;
                        zadanie.DataZakonczenia = DateTime.Now;
                        System.Threading.Thread.Sleep(1000);
                        
                        zadania.SaveChanges();

                        }
                    
                }
                Utils.LogWriter(DateTime.Now.ToShortTimeString());
            }
            else
            {
                Utils.LogWriter("Są zadania w toku");
            }

        }
    } 

    public static void GetDocsFromDirectory(string option, string directory)
    {
        string[] filePaths = Directory.GetFiles(directory, "*.xml");
        
        if (filePaths.GetLength(0) > 0)
        {
            DocsFromFile docs = new DocsFromFile();
            foreach (string fname in filePaths)
            {

                switch (option.ToUpper())
                {
                    case "/P":
                        docs.ReadPozew(fname, 0);
                        break;/// pozwy
                    case "/S":
                        docs.ReadMojeSprawy(fname, 0);
                        break;     /// 
                    case "/N":
                        docs.ReadNakaz(fname, 0,0);
                        break;     /// 
                    case "/I": // inne orzeczenia ( kończące ) 
                        docs.ReadOrzeczenie(fname, 0, 0);
                        break;
                    case "/IB": // inne orzeczenia ( kończące ) 
                        docs.ReadOrzeczenieBest(fname, 0, 0);
                        break;
                    case "/NB":
                        docs.ReadNakazBest(fname, 0, 0);
                        break;///
                    case "/KB":   // klauzula wykonalnosci Best
                        docs.ReadKlauzulaBest(fname, 0, 0);
                        break;///
                    case "/K":   // klauzula wykonalnosci Casus
                        docs.ReadKlauzula(fname, 0, 0);
                        break;///
                    case "/W":   // Wniosek Egzekucyjny 
                        docs.ReadWniosekEgz(fname, 0);
                        break;///
                                               
                    default:
                        break;
                }
                    // rozpoznać na jakim koncie będzie 
                
                
            
            }
        
        }
        
    
    }

    }
}
