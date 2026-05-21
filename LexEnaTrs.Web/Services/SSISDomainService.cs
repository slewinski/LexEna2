
namespace LexEnaTrs.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // TODO: Create methods containing your application logic.
    [EnableClientAccess]
    public class SSISDomainService : DomainService
    {
        [Invoke(HasSideEffects = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public int WstawZadanieDoKolejki(string nazwazadaniap, int typzadaniap)
        {
            try {
                ZadanieSet zadanie = new ZadanieSet();
                zadanie.DataRozpoczęcia = DateTime.Now;
                zadanie.NazwaZadania = nazwazadaniap;
                zadanie.TypZadaniaId = typzadaniap;
                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities()) 
                {
                    lexena.AddToZadanieSet(zadanie);
                    lexena.SaveChanges();
                    return zadanie.Id;
                              
                }
            }
            catch { return -1; }
            
        }
        [Invoke(HasSideEffects = true)]
        public int GetZadanieStatus(int numerZadania)
        {
            try {
                using (var  zadania = new LexEnaMeritumEntities())
                {
                    var query = from p in zadania.ZadanieSet
                                where p.Id.Equals(numerZadania)
                                select p;

                    foreach (var zadanie in query)
                    {
                                        
                           return zadanie.Status;
                    }

                }
            }
            catch { return -1; }

            return -1;
        }
        public string GetZadanieOpis(int numerZadania)
        {
            try
            {
                using (var zadania = new LexEnaMeritumEntities())
                {
                    var query = from p in zadania.ZadanieSet
                                where p.Id.Equals(numerZadania)
                                select p;

                    foreach (var zadanie in query)
                    {

                        return zadanie.Opis;
                    }

                }
            }
            catch { return "Błąd Serwisu"; }

            return "Nie znaleziono zadania";
        }
    }
}


