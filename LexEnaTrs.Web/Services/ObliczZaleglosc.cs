using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using Odsetki;
using WienaDB.Models;

namespace LexEnaTrs.Web
{
    public class ObliczZaleglosc
    {
        public List<CalcNaleznosc> wynikiNal;
        public List<CalcResults> wynikiAll;
        private int Sprawa_Id = 0;
        private DateTime DataOblicz;

        public int SaveResults()
        {
            LexEnaMeritumEntities lexena = new LexEnaMeritumEntities();

            var queryx = from x in lexena.StanNaleznosci
                         join y in lexena.Naleznosc on x.Naleznosc_Id equals y.Id
                         where x.data_s == DataOblicz && x.CzyNowy == 1 && y.Sprawa_id == Sprawa_Id
                         select x;


            if (queryx.Count() > 0)
            {
                foreach (var q in queryx)
                {
                    lexena.StanNaleznosci.DeleteObject(q);
                }  // usunięcie stanu należnosci
            }

            var queryy = from x in lexena.StanSprawy
                         where x.data_s == DataOblicz && x.Sprawa_Id == Sprawa_Id
                         select x;

            if (queryy.Count() > 0)
            {
                foreach (var p in queryy)
                {
                    lexena.StanSprawy.DeleteObject(p);
                }  // usunięcie stanu należnosci
            }
            lexena.SaveChanges();
            if (wynikiNal != null)
                foreach (var nal in wynikiNal)
                {
                    StanNaleznosci st = new StanNaleznosci();
                    st.CzyNowy = 1;
                    st.data_s = DataOblicz;
                    st.IdWiena = 0;
                    st.Naleznosc_Id = Convert.ToInt32(nal.ident);
                    st.oblicz = 0;
                    st.typ_stan = 0;


                    switch (nal.nr_nal)   // teraz to jest typ nalezności
                    {
                        case 1: // naleznosć
                            st.kwota_n = nal.kwota;
                            st.kwota_o = nal.ods;
                            break;
                        case 2:  // odsetki
                            st.kwota_o = nal.kwota + nal.ods;
                            break;
                        case 3: // koszty
                            st.kwota_k = nal.kwota;
                            st.kwota_o = nal.ods;
                            break;

                        default:
                            break;

                    }
                    st.ods_dzien = nal.ods_dzien;
                    lexena.StanNaleznosci.AddObject(st);

                }


            if (wynikiAll != null)
                foreach (var wyn in wynikiAll)
                {
                    StanSprawy stspr = new StanSprawy();
                    stspr.Sprawa_Id = Sprawa_Id;
                    stspr.CzyNowy = 1;
                    stspr.data_s = wyn.data_s;
                    stspr.IdWiena = 0;
                    stspr.kwota_k = wyn.sum_koszt;
                    stspr.kwota_n = wyn.sum_nal;
                    stspr.kwota_o = wyn.sum_ods;
                    stspr.ods_dzien = wyn.ods_dzien;
                    stspr.typ_stan = wyn.typ;
                    lexena.StanSprawy.AddObject(stspr);

                }

            lexena.SaveChanges();
            // dodatnie nowych
            return 0;
        }

        public void ObliczSpraweWiena(int IdSprawy, DateTime dtOblicz)
        {
            DateTime? DStanu;
            List<CalcWplata> WplLst;
            List<CalcWplataPodz> WplPodzLst;
            List<CalcNaleznosc> NalLst;

            using (wiena_centralEntities wiena = new wiena_centralEntities())
            {


                var query = from z in wiena.spr_stan
                            where z.typ_stan == 3 && z.id_sprawy == IdSprawy
                            orderby z.data_s descending
                            select z;

                if (query.Count() > 0)
                {
                    DStanu = query.FirstOrDefault<spr_stan>().data_s;
                }
                else
                    DStanu = null;

                if (DStanu >= (dtOblicz as DateTime?))
                {
                    //MessageBox.Show("Próba obliczenia zaległości po ustalonym stanie ");
                    return;
                }
                // odc zytanie wpłat
                WplLst = new List<CalcWplata>();
                CalcWplata wplc;
                var query1 = from x in wiena.wplata
                             where x.id_sprawy == IdSprawy && ((x.data_w > DStanu) || DStanu == null) && x.data_w <= dtOblicz
                             orderby x.data_w
                             select x;

                foreach (var rec in query1)
                {
                    wplc = new CalcWplata();
                    wplc.algo = Convert.ToInt32(rec.algo);
                    wplc.data_w = rec.data_w;
                    wplc.ident = rec.ident;
                    wplc.kwota = rec.kwota;
                    WplLst.Add(wplc);

                }
                // Zaczytanie wpłąta podzial
                var query2 = from y in wiena.wplata_podz
                             join w in wiena.wplata on y.id_wplaty equals w.ident
                             where w.id_sprawy == IdSprawy && ((w.data_w > DStanu) || DStanu == null) && w.data_w <= dtOblicz
                             select y;
                CalcWplataPodz wplp;
                WplPodzLst = new List<CalcWplataPodz>();
                foreach (var rec1 in query2)
                {
                    wplp = new CalcWplataPodz();
                    wplp.id_nal = Convert.ToInt32(rec1.id_nal);
                    wplp.id_wplaty = Convert.ToInt32(rec1.id_wplaty);
                    wplp.ident = rec1.ident;
                    wplp.spl_nal = Convert.ToDecimal(rec1.spl_kap);
                    wplp.spl_ods = Convert.ToDecimal(rec1.spl_ods);
                    WplPodzLst.Add(wplp);
                }
                // Zaczytanie należności
                var query3 = from a in wiena.naleznosc
                             join b in wiena.typ_nal on a.id_typ_nal equals b.ident
                             where a.id_sprawy == IdSprawy && a.data_n <= dtOblicz
                             orderby a.data_n
                             select a;

                CalcNaleznosc nal;
                NalLst = new List<CalcNaleznosc>();
                List<CalcOpisNal> OpisNalLst = new List<CalcOpisNal>();
                CalcOpisNal opisNal;
                List<CalcOdsNal> OdsNalLst = new List<CalcOdsNal>();
                CalcOdsNal odsNal;
                List<CalcTabOds> ListTabOds = new List<CalcTabOds>();
                CalcTabOds tabOds;

                foreach (var rec2 in query3)
                {
                    nal = new CalcNaleznosc();
                    nal.czy_proc = rec2.czy_proc;
                    if (rec2.ods_sprawy != null)
                        if (rec2.ods_sprawy.Count > 0)
                            nal.czy_proc = true;
                    nal.date_p = rec2.data_n;
                    nal.ident = rec2.ident;
                    nal.kwota = Convert.ToDecimal(rec2.kwota);
                    nal.ods = 0;
                    nal.typ_nal = rec2.typ_nal.ident;
                    nal.nr_nal = Convert.ToInt32(rec2.id_typ_nal);
                    var query4 = from c in wiena.spr_stan
                                 where c.id_sprawy == IdSprawy && c.typ_stan == 3
                                 select c;
                    if (query4.Count() > 0 && rec2.data_n <= DStanu)// zmieniono z rec2.data_n > DStanu 
                    {
                        nal.date_p = DStanu;
                        var query5 = from d in wiena.nal_stan
                                     where d.id_nal == nal.ident && d.data_s == DStanu
                                     orderby d.ident descending
                                     select d;
                        if (query5.Count() > 0)
                        {
                            nal_stan nalst = query5.FirstOrDefault();

                            nal.kwota = Convert.ToDecimal(nalst.kwota_k) + Convert.ToDecimal(nalst.kwota_n);
                            nal.ods = Convert.ToDecimal(nalst.kwota_o);

                        }


                    }
                    NalLst.Add(nal);
                    // opisy nal;eżnosci
                    bool found = false;
                    foreach (var o in OpisNalLst)
                    {
                        if (o.ident == nal.nr_nal)
                        {
                            found = true;
                            break;

                        }
                    }
                    if (!found)
                    {
                        opisNal = new CalcOpisNal();
                        opisNal.ident = nal.nr_nal;
                        OpisNalLst.Add(opisNal);
                    }

                    // dodanie odsetek dla każdej należności
                    var query7 = from f in wiena.ods_sprawy
                                 where f.id_nal == nal.ident && ((f.data_k > DStanu) || (f.data_k == null) || (DStanu == null))
                                 orderby f.data_p
                                 select f;

                    if (query7.Count() > 0)
                    {
                        foreach (var rec6 in query7)
                        {
                            odsNal = new CalcOdsNal();
                            odsNal.ident = rec6.ident;
                            odsNal.id_nal = Convert.ToInt32(nal.ident);
                            odsNal.id_tab_ods = Convert.ToInt32(rec6.id_typ_ods);
                            odsNal.licz_do = rec6.data_k;
                            odsNal.licz_od = rec6.data_p;
                            odsNal.proc = Convert.ToDecimal(rec6.proc0 / 365);
                            if (odsNal.licz_do == null)
                                odsNal.licz_do = Convert.ToDateTime("2040-01-01");
                            if (odsNal.licz_od <= DStanu && odsNal.licz_do > DStanu)
                                odsNal.licz_od = Convert.ToDateTime(DStanu).AddDays(1);
                            OdsNalLst.Add(odsNal);
                            if (odsNal.id_tab_ods > 1)
                            {
                                var query10 = from l in wiena.ods_tab
                                              where l.id_tab_ods == rec6.id_typ_ods
                                              orderby l.data_p
                                              select l;
                                foreach (var rec10 in query10)
                                {
                                    bool foundx = false;
                                    foreach (var tods in ListTabOds)
                                    {
                                        if (rec10.ident == tods.ident)
                                        {
                                            foundx = true;
                                            break;
                                        }
                                    }
                                    if (!foundx)
                                    {
                                        tabOds = new CalcTabOds();
                                        tabOds.ident = rec10.ident;
                                        tabOds.nr_tab = Convert.ToInt32(rec10.id_tab_ods);
                                        tabOds.licz_od = rec10.data_p;
                                        tabOds.licz_do = rec10.data_k;
                                        tabOds.proc = Convert.ToDecimal(rec10.proc0 / 365);
                                        if (tabOds.licz_do == null) tabOds.licz_do = Convert.ToDateTime("2040-01-01");
                                        ListTabOds.Add(tabOds);
                                    }

                                }




                                // jeśli odsetki ustawowe

                            }


                        }


                    }
                    else  // załadowanie domyślengo okresu odsetkowego
                    {
                        var query8 = from h in wiena.ods_sprawy
                                     where h.id_nal == nal.ident
                                     orderby h.data_k descending
                                     select h;
                        if (query8.Count() > 0)
                        {
                            ods_sprawy ods_ = query8.FirstOrDefault();
                            odsNal = new CalcOdsNal();
                            odsNal.id_nal = Convert.ToInt32(nal.ident);
                            odsNal.id_tab_ods = 1;
                            odsNal.ident = 1;
                            odsNal.licz_do = null;
                            odsNal.licz_od = Convert.ToDateTime(ods_.data_k).AddDays(1);
                            odsNal.proc = 0;
                            OdsNalLst.Add(odsNal);
                        }

                    }


                }

                // uzupełnienie opisów nalezności
                var query6 = from d in wiena.typ_nal select d;
                foreach (var rec3 in query6)
                {
                    foreach (var i in OpisNalLst)
                    {
                        if (i.ident == rec3.ident)
                        {
                            i.typ_nal = rec3.typ_nal1;
                            i.typ_sum = rec3.typ_sum;
                            break;
                        }

                    }

                }


                Zaleglosc LiczZal = new Zaleglosc();
                LiczZal.LoadData(dtOblicz, ref WplLst, ref WplPodzLst, ref NalLst, ref OdsNalLst, ref ListTabOds, ref OpisNalLst);
                LiczZal.LiczOdsetki();
                wynikiAll = new List<CalcResults>();
                wynikiNal = new List<CalcNaleznosc>();
                LiczZal.GetResults(ref wynikiNal, ref wynikiAll);
                foreach (CalcNaleznosc cn in wynikiNal)
                {
                    long id_nal = cn.ident;
                    decimal kw = NalLst.Where(a => a.ident == id_nal).Select(a => a.kwota).FirstOrDefault();
                    foreach (CalcWplataPodz wp in WplPodzLst)
                    {
                        if (wp.id_nal == id_nal)
                        {
                            kw -= wp.spl_nal;

                        }


                    }
                    if (kw < 0) kw = 0;
                    cn.kwota = kw;
                }

                // this.SaveResults();

                // Zapisanie stanu sprawy do bazy

            }



        }

       public void ObliczSprawe(int IdSprawy , DateTime dtOblicz)
        {   
            
            DateTime? DStanu;
            List<CalcWplata> WplLst;
            List<CalcWplataPodz>WplPodzLst;
            List<CalcNaleznosc>NalLst;
            LexEnaMeritumEntities  lexena = new LexEnaMeritumEntities();
            
            // obliczenie 
            // ustalenie daty ostatniego stanu 

            Sprawa_Id = IdSprawy;
            DataOblicz = dtOblicz;

            var query = from z in lexena.StanSprawy
                        where z.typ_stan == 3 && z.Sprawa_Id == IdSprawy
                        orderby z.data_s descending
                        select z;
   
            if (query.Count() > 0)
              {
                  DStanu = query.FirstOrDefault<StanSprawy>().data_s;
              }
            else
                DStanu = null;

            if (DStanu >= (dtOblicz as DateTime?) )
            {   
                //MessageBox.Show("Próba obliczenia zaległości po ustalonym stanie ");
                return;
            }
            // odc zytanie wpłat
            WplLst = new List<CalcWplata>();
            CalcWplata wplc;
            var query1 = from x in lexena.Wplata
                         where   x.Sprawa_id == IdSprawy &&  ((x.DataWplaty > DStanu) ||  DStanu == null ) && x.DataWplaty<=dtOblicz  orderby x.DataWplaty select x;

            foreach (var rec in query1)
            {
                wplc = new CalcWplata();
                wplc.algo = Convert.ToInt32(rec.AlgorytmZaliczania);
                wplc.data_w = rec.DataWplaty;
                wplc.ident = rec.Id;
                wplc.kwota = rec.Kwota;
                WplLst.Add(wplc);

            }
            // Zaczytanie wpłąta podzial
            var query2 =  from y in lexena.WplataPodz join w in lexena.Wplata on y.Wplata_Id equals  w.Id
                          where w.Sprawa_id == IdSprawy &&  ((w.DataWplaty > DStanu) ||  DStanu == null ) && w.DataWplaty<=dtOblicz 
                          select y;
            CalcWplataPodz wplp;
            WplPodzLst = new List<CalcWplataPodz>();
            foreach (var rec1 in  query2)
            {
                wplp = new CalcWplataPodz();
                wplp.id_nal = Convert.ToInt32(rec1.Naleznosc_Id);
                wplp.id_wplaty = Convert.ToInt32(rec1.Wplata_Id);
                wplp.ident = rec1.Id;
                wplp.spl_nal = Convert.ToDecimal(rec1.SplataKapital);
                wplp.spl_ods = Convert.ToDecimal(rec1.SplataOdsetki);
                WplPodzLst.Add(wplp);
            }
            // Zaczytanie należności
            var query3 =  from a in lexena.Naleznosc  join b in lexena.TypNaleznosci on a.TypNaleznosci_id equals b.id
                          where a.Sprawa_id == IdSprawy && a.data_n <= dtOblicz orderby a.data_n 
                          select a ;

            CalcNaleznosc nal;
            NalLst = new List<CalcNaleznosc>();
            List<CalcOpisNal> OpisNalLst = new List<CalcOpisNal>();
            CalcOpisNal opisNal;
            List<CalcOdsNal> OdsNalLst = new List<CalcOdsNal>();
            CalcOdsNal odsNal;
            List<CalcTabOds> ListTabOds = new List<CalcTabOds>();
            CalcTabOds tabOds; 

            foreach(var rec2 in query3 )
            {
                nal = new CalcNaleznosc();
                nal.czy_proc = (rec2.CzyOdsetki > 0 ? true: false);
                if (rec2.Odsetki != null)
                    if (rec2.Odsetki.Count > 0)
                        nal.czy_proc = true;
                nal.date_p = rec2.data_n;
                nal.ident = rec2.Id;
                nal.kwota = Convert.ToDecimal( rec2.kwota);
                nal.ods = 0;
                nal.nr_nal = Convert.ToInt32(rec2.TypNaleznosci_id);
                var query4 = from c in  lexena.StanSprawy where c.Sprawa_Id == IdSprawy && c.typ_stan ==3 
                             select c;
                if (query4.Count() > 0  && rec2.data_n > DStanu )
                {
                    nal.date_p = DStanu;
                    var query5  = from d in lexena.StanNaleznosci 
                                  where d.Naleznosc_Id == nal.ident && d.data_s == DStanu
                                  orderby d.Id descending 
                                  select d;
                    if (query5.Count() > 0 )
                    {
                      StanNaleznosci nalst = query5.FirstOrDefault();

                      nal.kwota = Convert.ToDecimal( nalst.kwota_k) + Convert.ToDecimal(nalst.kwota_n);
                      nal.ods = Convert.ToDecimal(nalst.kwota_o);  
                    
                    }
 
                
                }
                NalLst.Add(nal);
                // opisy nal;eżnosci
                bool found = false;
                foreach (var o in OpisNalLst)
                {
                    if (o.ident == nal.nr_nal)
                    {
                        found = true;
                        break ;

                    }
                }
                if (!found)
                {
                    opisNal = new CalcOpisNal();
                    opisNal.ident = nal.nr_nal;
                    OpisNalLst.Add(opisNal);
                }

                // dodanie odsetek dla każdej należności
                var query7 = from f in lexena.Odsetki 
                             where f.Naleznosc_Id == nal.ident && ((f.DataK > DStanu) || (f.DataK == null ) || (DStanu == null )) orderby f.DataPocz
                             select f;

                if (query7.Count() > 0)
                {
                    foreach (var rec6 in query7)
                    {
                        odsNal = new CalcOdsNal();
                        odsNal.ident = rec6.Id;
                        odsNal.id_nal = Convert.ToInt32(nal.ident);
                        odsNal.id_tab_ods = Convert.ToInt32(rec6.NazwyOdsetek_Id);
                        odsNal.licz_do = rec6.DataK;
                        odsNal.licz_od = rec6.DataPocz;
                        odsNal.proc = Convert.ToDecimal(rec6.Proc0 / 365);
                        if (odsNal.licz_do == null)
                        odsNal.licz_do = Convert.ToDateTime("2040-01-01");
                        if (odsNal.licz_od <=  DStanu  && odsNal.licz_do > DStanu)
                             odsNal.licz_od = Convert.ToDateTime(DStanu).AddDays(1);
                        OdsNalLst.Add(odsNal);
                        if (odsNal.id_tab_ods > 1 )
                        {
                            var query10 = from l in lexena.OdsTab where l.IdTabOds == rec6.NazwyOdsetek_Id
                                          orderby l.DataP 
                                          select l;
                            foreach (var rec10 in query10)
                            {   
                                    bool foundx = false;
                                    foreach (var tods in ListTabOds)
                                    {
                                        if (rec10.Id == tods.ident)
                                        {
                                            foundx = true;
                                            break;
                                        }
                                    }
                                    if (!foundx)
                                    {
                                        tabOds = new CalcTabOds();
                                        tabOds.ident = rec10.Id;
                                        tabOds.nr_tab = Convert.ToInt32(rec10.IdTabOds);
                                        tabOds.licz_od = rec10.DataP;
                                        tabOds.licz_do = rec10.DataK;
                                        tabOds.proc = Convert.ToDecimal(rec10.Proc0 / 365);
                                        if (tabOds.licz_do == null ) tabOds.licz_do = Convert.ToDateTime("2040-01-01");
                                        ListTabOds.Add(tabOds);                                
                                    }
                            
                            }
 



                            // jeśli odsetki ustawowe

                        }

                        
                    }


                }
                else  // załadowanie domyślengo okresu odsetkowego
                { 
                var query8  = from h in lexena.Odsetki where h.Naleznosc_Id == nal.ident orderby h.DataK descending
                              select h;
                if  (query8.Count() > 0 ) 
                {
                    Odsetki ods_ = query8.FirstOrDefault();
                    odsNal = new CalcOdsNal();
                    odsNal.id_nal = Convert.ToInt32(nal.ident);
                    odsNal.id_tab_ods = 1;
                    odsNal.ident = 1;
                    odsNal.licz_do = null;
                    odsNal.licz_od = Convert.ToDateTime(ods_.DataK).AddDays(1);
                    odsNal.proc = 0;
                    OdsNalLst.Add(odsNal);
                }
                
                }

            
            }

         // uzupełnienie opisów nalezności
            var query6 = from d in lexena.TypNaleznosci select d;
            foreach (var rec3 in query6)
            {   
                foreach (var i in OpisNalLst)
                {
                    if (i.ident == rec3.id)
                    {
                    i.typ_nal = rec3.TypNal;
                    i.typ_sum = rec3.TypSum;
                    break;
                    }
            
                }
            
            }

            
            Zaleglosc LiczZal = new Zaleglosc();
            LiczZal.LoadData(dtOblicz, ref WplLst, ref WplPodzLst, ref NalLst, ref OdsNalLst, ref ListTabOds, ref OpisNalLst);
            LiczZal.LiczOdsetki();
            wynikiAll = new List<CalcResults>();
            wynikiNal = new List<CalcNaleznosc>();
            LiczZal.GetResults(ref wynikiNal, ref wynikiAll);
            foreach (CalcNaleznosc cn in wynikiNal)
            {
                long id_nal = cn.ident;
                decimal kw = NalLst.Where(a => a.ident == id_nal).Select(a => a.kwota).FirstOrDefault();
                foreach (CalcWplataPodz wp in WplPodzLst)
                {
                    if (wp.id_nal == id_nal)
                    {
                        kw -= wp.spl_nal;

                    }


                }
                if (kw < 0) kw = 0;
                cn.kwota = kw;
            }
            // this.SaveResults();

            // Zapisanie stanu sprawy do bazy

        }
     
        
        
        }




    }
