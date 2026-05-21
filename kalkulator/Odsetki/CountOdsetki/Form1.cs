using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Odsetki;

namespace CountOdsetki
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int IdSprawy;
            DateTime dtOblicz;
            DateTime? DStanu;
            List<CalcWplata> WplLst;
            List<CalcWplataPodz>WplPodzLst;
            List<CalcNaleznosc>NalLst;
            IdSprawy = Convert.ToInt32(textBox1.Text);
            dtOblicz = dateTimePicker1.Value;
            LexEnaProEntities lexena = new LexEnaProEntities();
            // obliczenie 
            // ustalenie daty ostatniego stanu 
            
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
                nal.typ_nal = Convert.ToInt32(rec2.TypNaleznosci_id);
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
        }
     
        }

         
  }

