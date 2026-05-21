using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexEnaTrs.Web;
using System.Data.SqlClient;

namespace CalcZaleglosc
{
    class Program
    {
        static void Main(string[] args)
        {
            Utils.LogWriter("Start Calc Zaległość ");
            int idZgonyHeader = 0;
            string d_stanu;
            DateTime dataStanu;
            int mode = 0; 

            string option = "";
            if (args.Length == 0)
            {

                Utils.LogWriter("Brak parametrów wejściowych");
                return;
            }
            try
            {
                if (args.Length == 2 || args.Length == 3)
                {
                    idZgonyHeader = Int32.Parse(args[0]);
                    d_stanu = args[1].ToUpper();
                    if (args.Length == 3)
                    {

                        mode = Int32.Parse(args[2]);
                    }

                }
                else
                {
                    Utils.LogWriter("Błędna liczba parametrów wejściowych");
                    return;

                }

                if (!DateTime.TryParse(d_stanu, out dataStanu))
                {
                    Utils.LogWriter("Błąd parsowania daty stanu ");
                    return;

                }
                switch (mode)
                {
                    case 0:
                        liczZalegloscZgon(idZgonyHeader, dataStanu);
                        break;
                    case 1:
                        liczZalegloscWiekowanie(idZgonyHeader, dataStanu);
                        break;

                }
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd parsowania argumentów wejściowych :" + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : ""));
                return;
            }


        }
        static private void liczZalegloscWiekowanie(Int32 pakietId, DateTime dStanu)
        {
            ObliczZaleglosc oZal = new ObliczZaleglosc();
            List<WienaDB.Models.typ_nal> tNalLst = null;
            using (WienaDB.Models.wiena_centralEntities wiena = new WienaDB.Models.wiena_centralEntities())
            {
                tNalLst = wiena.typ_nal.ToList();
            }
                using (LexEnaMeritumEntities context  =new LexEnaTrs.Web.LexEnaMeritumEntities() )
            {
               StanNalObliczPakiet zh = context.StanNalObliczPakiet.Where(a => a.Id == pakietId).FirstOrDefault();
                zh.StatOpis = " Rozpoczęcie obliczania odsetek ";
                zh.Status = 1;
                try
                {
                    context.SaveChanges();
                    List<int?> pLst = context.StanNalOblicz.Where(a => a.PakietId == zh.PakietId && (a.data_s == null)).Select(a=>a.Sprawa_Id).Distinct().ToList();
                    int i = 0;
                    foreach (int? p in pLst)
                    {
                        if (p == null) continue;
                        i++;
                        
                        oZal.ObliczSpraweWiena(p ?? 0, dStanu);
                        if (i % 30 == 0)
                            zh.StatOpis = " Obliczania odsetek  dla sprawy " + i.ToString() + " z " + pLst.Count.ToString();
                    
                        if (oZal.wynikiAll != null && oZal.wynikiAll.Count > 0)
                        {
                           
                                decimal? kk = oZal.wynikiNal.Where(n => n.typ_nal == 9 || n.typ_nal == 13).Sum(a => a.kwota);


                            foreach (var nale in oZal.wynikiNal)
                            {
                                StanNalOblicz sn = context.StanNalOblicz.Where(a => a.PakietId == zh.PakietId && a.Naleznosc_Id == nale.ident).FirstOrDefault();
                                if (sn != null)
                                {
                                    sn.kwota_n = nale.kwota;
                                    sn.kwota_o = nale.ods;
                                    sn.data_s = dStanu;
                                }
                            }
                                
                                context.SaveChanges();
                            }




                       
                   
                        context.SaveChanges();
                        // obliczono odsetki

                    }


                    zh.StatOpis = "Zakończono przetwarzanie zestawu. Dane gotowe do raportu";
                    zh.Status = 200;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.LogWriter("Błąd:" + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : ""));
                    using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                    {

                        zh.Status = -1;
                        zh.StatOpis = " Wystąpił błąd podczas przeliczania odsetek ";
                        lexena.SaveChanges();
                    }
                }
            }
        }
    

    static private void  liczZalegloscZgon(Int32 pakietId, DateTime dStanu)
        {
            ObliczZaleglosc oZal = new ObliczZaleglosc();
            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {
                ZgonyHeader zh = context.ZgonyHeader.Where(a => a.ZgonyHeader_Id == pakietId).FirstOrDefault();
                zh.StatusText = " Rozpoczęcie obliczania odsetek ";
                try
                {
                    context.SaveChanges();
                    List<ZgonyDetails> pLst = context.ZgonyDetails.Where(a => a.ZgonyHeader_ID == pakietId && (a.Status == null || a.Status!= 1)).ToList();
                    int i = 0;
                    foreach (ZgonyDetails p in pLst)
                    {
                        i++;
                        oZal.ObliczSpraweWiena(p.IdSprWiena ?? 0, dStanu);
                        if (i % 10 == 0)
                            zh.StatusText = " Obliczania odsetek  dla sprawy " + i.ToString() + " z " + pLst.Count.ToString();
                        if (oZal.wynikiAll != null && oZal.wynikiAll.Count > 0)
                        {
                            decimal? odsetki = oZal.wynikiAll.ElementAt(oZal.wynikiAll.Count - 1).sum_ods;
                            p.WindykOdsetki = odsetki - p.OdsetkiZaksKsiegowaPrzedaw;
                            if (p.NalGlownaKsiegowaPrzedaw <= 0)
                            {
                                p.OdsetkiZaksKsiegowaNiePrzedaw = p.OdsetkiZaksKsiegowaPrzedaw;
                                p.OdsetkiZaksKsiegowaPrzedaw = 0;
                                if (p.OdsetkiZaksKsiegowaNiePrzedaw > 0)
                                    p.DataPlatnosciOdsetek = p.DataPlatnosciNalGl;
                            }
                            else if (p.NalGlownaKsiegowaNiePrzedaw <= 0)
                            {
                                p.OdsetkiZaksKsiegowaNiePrzedaw = 0;
                                if (p.OdsetkiZaksKsiegowaPrzedaw > 0)
                                    p.DataPrzedawnieniaOdsetek = p.DataPrzedawnieniaNalGl;


                            }
                            else
                            {
                                if (p.NalGlownaKsiegowaPrzedaw > 0 && p.OdsetkiZaksKsiegowaPrzedaw > 0)
                                {
                                    p.OdsetkiZaksKsiegowaNiePrzedaw = Decimal.Round(p.OdsetkiZaksKsiegowaPrzedaw.Value * p.NalGlownaKsiegowaNiePrzedaw.Value / (p.NalGlownaKsiegowaNiePrzedaw.Value + p.NalGlownaKsiegowaPrzedaw.Value), 2);
                                    p.OdsetkiZaksKsiegowaPrzedaw = p.OdsetkiZaksKsiegowaPrzedaw - p.OdsetkiZaksKsiegowaNiePrzedaw;
                                    p.DataPrzedawnieniaOdsetek = p.DataPrzedawnieniaNalGl;
                                    p.DataPlatnosciOdsetek = p.DataPlatnosciNalGl;
                                }

                            }

                            // ustawienie odsetek przedawnionyccch i dat 
                            // ustawienie statusu procesowania



                        }
                        p.Status = 1;
                        context.SaveChanges();
                        // obliczono odsetki
                        
                    }


                    zh.StatusText = "Zakończono przetwarzanie zestawu. Dane gotowe do raportu";
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.LogWriter("Błąd:" + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : ""));
                    using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                    {

                        zh.StatusZestawienia = -1;
                        zh.StatusText = " Wystąpił błąd podczas przeliczania odsetek ";
                        lexena.SaveChanges();
                    }
                }
            }
        }
    }
}
