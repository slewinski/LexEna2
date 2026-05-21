using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Xml.Serialization;
using System.Threading;

namespace ZadanieTimer
{
    class Program
    {
       public  bool zadanieWToku = false;

        static void Main(string[] args)
        {

          

            if (args.GetLength(0) == 2) // wczytywanie
            {

                Utils.LogWriter("Uruchomienie w trybie wczytywania zbiorów XML");
                ZadanieTimerMain.GetDocsFromDirectory(args[0],args[1]);
               return;
 
            }
                try {
                    //DoTest2();
                    //DoTest();
                    //DoTest3();
                        Utils.LogWriter("Anulowanie zadań uruchominych w poprzednich cyklach i przygotowanie ich do następnego uruchomienia");
                   
                  using (var zadania = new LexEnaProEntities())
                    {

                      Utils.LogWriter("Próba połączenia z bazą danych: " + zadania.Connection.ConnectionString +";" +zadania.Connection.DataSource+";"+zadania.Connection.Database);
                      var czyZadania = from z in zadania.ZadanieSet
                                  where z.Status.Equals(100)
                                  select z;
                      if (czyZadania.Count() > 0)
                      {
                          foreach (var zadanie in czyZadania)
                          {
                              zadanie.Status = 0;
                          }
                      }
                      zadania.SaveChanges();
                  }
                  ZadanieTimerMain.OneTimedEvent();
                  ZadanieTimerMain.OnTimedEvent();
              } catch(Exception ex) {
                  Utils.LogWriter("Błąd w trakcie zerowania zadań" + ex.ToString());
                  Console.WriteLine("Błąd w trakcie zerowania zadań"+ex.ToString());
              }

          
    
    }

    }
}
