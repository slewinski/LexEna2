using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Odsetki
{

    // Wyliczenie stanu zaległosci następuj nd dzień adt_data  , jesli  w sprawie jest stan zaległosci  ustalony  ( typ 3) na dzien   




    [Serializable()]
    public class CalcWplata : ISerializable    {
        public long ident;
        public decimal kwota;
        public DateTime? data_w;
        public int algo;

        public CalcWplata()
        {
            ident = 0;
            kwota = 0;
            data_w = null;
            algo = 1;
        }

        public CalcWplata(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            ident = (int)info.GetValue("Ident", typeof(int));
            kwota = (decimal)info.GetValue("Kwota", typeof(decimal));
            data_w = (DateTime)info.GetValue("Data_w", typeof(DateTime));
            algo = (int)info.GetValue("Algo", typeof(int));

        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("Ident", ident);
            info.AddValue("Kwota", kwota);
            info.AddValue("Data_w", data_w);
            info.AddValue("Algo", algo);


        }

    }
    [Serializable()]
    public class CalcWplataPodz : ISerializable
    {
        public long ident;
        public long id_wplaty;
        public long id_nal;
        public decimal spl_nal;
        public decimal spl_ods;

        public CalcWplataPodz()
        {
            ident = 0;
            id_wplaty = 0;
            id_nal = 0;
            spl_nal = 0;
            spl_ods = 0;
        }

        public CalcWplataPodz(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            ident = (long)info.GetValue("Ident", typeof(long));
            id_wplaty = (long)info.GetValue("Id_wplaty", typeof(long));
            id_nal = (long)info.GetValue("Id_nal", typeof(long));
            spl_nal = (decimal)info.GetValue("Spl_nal", typeof(decimal));
            spl_ods = (decimal)info.GetValue("Spl_ods", typeof(decimal));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("Ident", ident);
            info.AddValue("Id_wplaty", id_wplaty);
            info.AddValue("Id_nal", id_nal);
            info.AddValue("Spl_nal", spl_nal);
            info.AddValue("Spl_odc", spl_ods);


        }

    }
    [Serializable()]
    public class CalcNaleznosc : ISerializable
    {
        public int nr_nal;  // typ należności ( należność głowna, koszty odsetki 
        public decimal kwota;
        public decimal ods;
        public decimal ods_dzien;
        public DateTime? date_p;
        public long ident;
        public bool czy_proc;
        public int typ_nal;
        public CalcNaleznosc()
        {
            nr_nal = 0;
            kwota = 0;
            ods = 0;
            ods_dzien = 0;
            date_p = null;
            ident = 0;
            czy_proc = false;
        }

        public CalcNaleznosc(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            ident = (long)info.GetValue("Ident", typeof(long));
            nr_nal = (int)info.GetValue("Nr_nal", typeof(int));
            ods = (decimal)info.GetValue("Ods", typeof(decimal));
            ods_dzien = (decimal)info.GetValue("Ods_dzien", typeof(decimal));
            kwota = (decimal)info.GetValue("Kwota", typeof(decimal));
            date_p = (DateTime)info.GetValue("Date_p", typeof(DateTime));
            czy_proc = (bool)info.GetValue("Czy_proc", typeof(bool));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("Ident", ident);
            info.AddValue("Nr_nal", nr_nal);
            info.AddValue("Kwota", kwota);
            info.AddValue("Ods", ods);
            info.AddValue("Ods_dzien", ods_dzien);
            info.AddValue("Date_p", date_p);
            info.AddValue("czy_proc", czy_proc);
        }



    }
    [Serializable()]
    public class CalcOpisNal : ISerializable
    {
        public int typ_nal;  // typ należności ( należność głowna, koszty odsetki 
        public int typ_sum;
        public int ident;

        public CalcOpisNal()
        {
            typ_nal = 0;
            typ_sum = 0;
            ident = 0;
        }

        public CalcOpisNal(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            ident = (int)info.GetValue("Ident", typeof(int));
            typ_nal = (int)info.GetValue("Typ_Nal", typeof(int));
            typ_sum = (int)info.GetValue("Typ_Sum", typeof(int));

        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("Ident", ident);
            info.AddValue("Typ_Nal", typ_nal);
            info.AddValue("Typ_Sum", typ_sum);


        }

    }
    [Serializable()]
    public class CalcOdsNal : ISerializable
    {
        public int ident;
        public int id_nal;
        public int id_tab_ods;
        public DateTime? licz_od;
        public DateTime? licz_do;
        public decimal proc;

        public CalcOdsNal()
        {
            ident = 0;
            id_nal = 0;
            id_tab_ods = 0;
            licz_od = null;
            licz_do = null;
            proc = 0;
        }

        public CalcOdsNal(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            ident = (int)info.GetValue("Ident", typeof(int));
            id_nal = (int)info.GetValue("Id_Nal", typeof(int));
            id_tab_ods = (int)info.GetValue("Id_Tab_Ods", typeof(int));
            licz_od = (DateTime?)info.GetValue("Licz_Od", typeof(DateTime?));
            licz_do = (DateTime?)info.GetValue("Licz_Do", typeof(DateTime?));
            proc = (decimal)info.GetValue("Proc", typeof(decimal));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("Ident", ident);
            info.AddValue("Id_Nal", id_nal);
            info.AddValue("Id_Tab_Ods", id_tab_ods);
            info.AddValue("Licz_Od", licz_od);
            info.AddValue("Licz_Do", licz_do);
            info.AddValue("Proc", proc);

        }

    }
    [Serializable()]
    public class CalcTabOds : ISerializable
    {
        public int ident;
        public int nr_tab;
        public DateTime? licz_od;
        public DateTime? licz_do;
        public decimal proc;

        public CalcTabOds()
        {
            ident = 0;
            nr_tab = 0;
            licz_od = null;
            licz_do = null;
            proc = 0;
        }

        public CalcTabOds(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            ident = (int)info.GetValue("Ident", typeof(int));
            nr_tab = (int)info.GetValue("Nr_Tab", typeof(int));
            licz_od = (DateTime?)info.GetValue("Licz_Od", typeof(DateTime?));
            licz_do = (DateTime?)info.GetValue("Licz_Do", typeof(DateTime?));
            proc = (decimal)info.GetValue("Proc", typeof(decimal));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("Ident", ident);
            info.AddValue("Nr_Tab", nr_tab);
            info.AddValue("Licz_Od", licz_od);
            info.AddValue("Licz_Do", licz_do);
            info.AddValue("Proc", proc);


        }

    }

    public class CalcResults : ISerializable
    {
        public int typ;
        public DateTime? data_s;
        public decimal sum_nal;
        public decimal sum_koszt;
        public decimal sum_ods;
        public decimal ods_dzien;

        public CalcResults()
        {
            typ = 0;
            sum_nal = 0;
            sum_koszt = 0;
            sum_ods = 0;
            ods_dzien = 0;
        }

        public CalcResults(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            typ = (int)info.GetValue("Typ", typeof(int));
            sum_nal = (decimal)info.GetValue("Suma_Nal", typeof(decimal));
            sum_koszt = (decimal)info.GetValue("Suma_Koszt", typeof(decimal));
            sum_ods = (decimal)info.GetValue("Suma_Odsetek", typeof(decimal));
            ods_dzien = (decimal)info.GetValue("Odsetki_Dzienne", typeof(decimal));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("Typ", typ);
            info.AddValue("Suma_Nal", sum_nal);
            info.AddValue("Suma_Koszt", sum_koszt);
            info.AddValue("Suma_Odsetek", sum_ods);
            info.AddValue("Odsetki_Dzienne", ods_dzien);


        }

    }


    public class Zaleglosc
    {
        private struct linetime
        {
            public DateTime? dat;
            public int typ;
            public int numer_kwoty;
            public int typ_nal;
            public int numer_nal;
            public bool czy_proc;
        }
        public struct k_wpl
        {
            public int m_typ;
            public decimal m_kwota;
            public decimal m_procent;
            public DateTime? m_ldate0;
            public int m_pos;
            public int prizn;
        }
        public struct licz_res
        {
            public int typ;
            public DateTime? data_s;
            public decimal sum_nal;
            public decimal sum_koszt;
            public decimal sum_ods;
            public decimal odz_dzien;
        }
        public struct naleznosc
        {
            public int nr_nal;
            public decimal kwota;
            public decimal ods;
            public decimal ods_dzien;
            public DateTime? date_p;
            public long ident;
            public bool czy_proc;
            public int typ_nal;
        }
        public struct ods_nal
        {
            public long ident;
            public long id_nal;
            public int id_tab_ods;
            public DateTime? licz_od;
            public DateTime? licz_do;
            public decimal proc;    // długi decimal 10 miejsc po przecinku
        }
        public struct opis_nal
        {
            public int typ_nal;
            public int typ_sum;
            public long ident;
        }
        public struct po_wpl
        {
            public int m_typ;
            public int m_pos;
            public decimal m_kwota;
            public decimal m_kwotado;
        }
        public struct tab_ods
        {
            public long ident;
            public int nr_tab;
            public DateTime? licz_od;
            public DateTime? licz_do;
            public decimal proc; // długi decimal
        }
        public struct tabstan
        {
            public DateTime? dat;
            public decimal don;
            public decimal doo;
            public decimal dok;
            public decimal pon;
            public decimal poo;
            public decimal pok;
            public decimal kw;
            public char typ;
        }
        public struct wpl_podz
        {
            public long ident;
            public long id_wplaty;
            public long id_nal;
            public decimal spl_nal;
            public decimal spl_ods;
        }
        public struct wplata
        {
            public long ident;
            public decimal kwota;
            public DateTime? data_w;
            public int algo;
        }
        public struct input_data
        {
            public opis_nal[] mas_opis_nal;
            public naleznosc[] mas_naleznosc;
            public ods_nal[] mas_ods_nal;
            public tab_ods[] mas_tab_ods;
            public wplata[] mas_wplata;
            public licz_res[] mas_licz_res;
            public wpl_podz[] mas_wpl_podz;
            public decimal suma_nowej_wplaty;
            public DateTime? data_st;
            public int size_opis_nal;
            public int size_naleznosc;
            public int size_ods_nal;
            public int size_tab_ods;
            public int size_wplata;
            public int size_licz_res;
            public int size_wpl_podz;
            public wpl_podz[] mas_wpl_plan;
            public int algo;
            public int size_wpl_plan;
        }
        private input_data lid_Data;

        public void LoadData(DateTime DataObl, ref List<CalcWplata> lstwpl, ref List<CalcWplataPodz> lstwplpodz, ref List<CalcNaleznosc> lstnal, ref List<CalcOdsNal> lstodsnal, ref List<CalcTabOds> lsttabods, ref List<CalcOpisNal> lstopisnal)
        {
            //lstwpl  order by  data_w asc 
            // przepisanie do strultury
            int index;

            lid_Data.data_st = Convert.ToDateTime(DataObl).Date;
            lid_Data.suma_nowej_wplaty = 0;

            index = 0;
            //if (lstwpl.Count  > 0 ) 
            Array.Resize<wplata>(ref lid_Data.mas_wplata, lstwpl.Count + 1);
            foreach (CalcWplata wp in lstwpl)
            {
                index++;
                lid_Data.mas_wplata[index].ident = wp.ident;
                lid_Data.mas_wplata[index].kwota = wp.kwota;
                lid_Data.mas_wplata[index].data_w = wp.data_w;
                lid_Data.mas_wplata[index].algo = wp.algo;
            }

            // wpłata podział 

            index = 0;
            Array.Resize<wpl_podz>(ref lid_Data.mas_wpl_podz, lstwplpodz.Count + 1);
            foreach (CalcWplataPodz wplp in lstwplpodz)
            {
                index++;

                lid_Data.mas_wpl_podz[index].ident = wplp.ident;
                lid_Data.mas_wpl_podz[index].id_wplaty = wplp.id_wplaty;
                lid_Data.mas_wpl_podz[index].id_nal = wplp.id_nal;
                lid_Data.mas_wpl_podz[index].spl_nal = wplp.spl_nal;
                lid_Data.mas_wpl_podz[index].spl_ods = wplp.spl_ods;
            }


            // należności order by data należnosci asc
            index = 0;
            //Array.Resize<naleznosc>(ref lid_Data.mas_naleznosc, 1);
            Array.Resize<naleznosc>(ref lid_Data.mas_naleznosc, lstnal.Count + 1);
            foreach (CalcNaleznosc nalp in lstnal)
            {
                index++;
                lid_Data.mas_naleznosc[index].czy_proc = nalp.czy_proc;
                lid_Data.mas_naleznosc[index].date_p = nalp.date_p;
                lid_Data.mas_naleznosc[index].ident = nalp.ident;
                lid_Data.mas_naleznosc[index].kwota = nalp.kwota;
                lid_Data.mas_naleznosc[index].nr_nal = nalp.nr_nal;
                lid_Data.mas_naleznosc[index].ods = nalp.ods;
                lid_Data.mas_naleznosc[index].ods_dzien = nalp.ods_dzien;
                lid_Data.mas_naleznosc[index].typ_nal = nalp.nr_nal;

            }

            index = 0;
            Array.Resize<ods_nal>(ref lid_Data.mas_ods_nal, lstodsnal.Count + 1);
            foreach (CalcOdsNal odsnal in lstodsnal)
            {
                index++;

                lid_Data.mas_ods_nal[index].id_nal = odsnal.id_nal;
                lid_Data.mas_ods_nal[index].id_tab_ods = odsnal.id_tab_ods;
                lid_Data.mas_ods_nal[index].ident = odsnal.ident;
                lid_Data.mas_ods_nal[index].licz_do = odsnal.licz_do;
                lid_Data.mas_ods_nal[index].licz_od = odsnal.licz_od;
                lid_Data.mas_ods_nal[index].proc = odsnal.proc;


            }
            // Tabel;e odsetkowe   
            index = 0;
            Array.Resize<tab_ods>(ref lid_Data.mas_tab_ods, lsttabods.Count + 1);
            foreach (CalcTabOds tabodsnal in lsttabods)
            {
                index++;
                lid_Data.mas_tab_ods[index].ident = tabodsnal.ident;
                lid_Data.mas_tab_ods[index].licz_do = tabodsnal.licz_do;
                lid_Data.mas_tab_ods[index].licz_od = tabodsnal.licz_od;
                lid_Data.mas_tab_ods[index].nr_tab = tabodsnal.nr_tab;
                lid_Data.mas_tab_ods[index].proc = tabodsnal.proc;

            }
            //  opis nalezności
            index = 0;
            Array.Resize<opis_nal>(ref lid_Data.mas_opis_nal, lstopisnal.Count + 1);
            foreach (CalcOpisNal opnal in lstopisnal)
            {
                index++;
                lid_Data.mas_opis_nal[index].ident = opnal.ident;
                lid_Data.mas_opis_nal[index].typ_nal = opnal.typ_nal == 4 ? 1 : opnal.typ_nal;
                lid_Data.mas_opis_nal[index].typ_sum = opnal.typ_sum;

            }
            lid_Data.size_opis_nal = lid_Data.mas_opis_nal.GetLength(0) - 1;
            lid_Data.size_naleznosc = lid_Data.mas_naleznosc.GetLength(0) - 1;
            lid_Data.size_ods_nal = lid_Data.mas_ods_nal.GetLength(0) - 1;
            lid_Data.size_tab_ods = lid_Data.mas_tab_ods.GetLength(0) - 1;
            lid_Data.size_wplata = lid_Data.mas_wplata.GetLength(0) - 1;
            lid_Data.size_wpl_podz = lid_Data.mas_wpl_podz.GetLength(0) - 1;
        }


        public decimal LiczOdsetki()
        {
            linetime[] mas_lnt;
            int size_lnt;
            k_wpl[] mask_wpl;
            int size_k_wpl;
            po_wpl[] mas_po_wpl;
            int size_po_wpl;
            tabstan[] ts;
            DateTime? dat, dat1;
            int size_ts;
            wplata wpl;
            int i, j;
            decimal kw;
            int sposob;

            mas_lnt = new linetime[0];
            mas_po_wpl = new po_wpl[0];
            mask_wpl = new k_wpl[0];
            ts = new tabstan[0];

            for (i = 1; i <= lid_Data.size_wplata - 1; i++)
            {
                for (j = i + 1; j <= lid_Data.size_wplata; j++)
                {
                    if (lid_Data.mas_wplata[i].data_w > lid_Data.mas_wplata[j].data_w)
                    {
                        wpl = lid_Data.mas_wplata[i];
                        lid_Data.mas_wplata[i] = lid_Data.mas_wplata[j];
                        lid_Data.mas_wplata[j] = wpl;
                    }
                }
            }
            size_lnt = 0;
            size_k_wpl = 0;
            size_po_wpl = 0;
            size_ts = 1;
            Array.Resize<tabstan>(ref ts, size_ts + 1);
            f_formlinetime(ref lid_Data, ref mas_lnt, ref size_lnt);
            for (i = 1; i <= lid_Data.size_naleznosc; i++)
            {
                if (lid_Data.mas_naleznosc[i].date_p <= lid_Data.data_st)
                {
                    if (lid_Data.mas_naleznosc[i].ods > 0)
                    {
                        newk_wpl(ref mask_wpl, ref size_k_wpl, 11, i, ref lid_Data.mas_naleznosc[i].ods, ref lid_Data.mas_naleznosc[i].date_p, 1);
                    }
                }
            }

            kw = f_computewpl(ref lid_Data, ref mas_lnt, size_lnt, ref mask_wpl, ref size_k_wpl, ref mas_po_wpl, ref size_po_wpl, ref ts, ref size_ts);

            f_formostnal(ref lid_Data, ref mask_wpl, size_k_wpl, ref kw);

            dat1 = Convert.ToDateTime(lid_Data.data_st).AddDays(1);
            dat = lid_Data.data_st;

            decimal AllSumKoszty;
            decimal AllSumOds;
            decimal AllSumNal;
            decimal AllSumProc;

            AllSumKoszty = 0;
            AllSumOds = 0;
            AllSumNal = 0;
            AllSumProc = 0;
            for (i = 1; i <= size_k_wpl; i++)
            {
                //	IF (mask_wpl[i].m_kwota = 0.0) THEN CONTINUE
                if (mask_wpl[i].m_typ == 1 )
                    AllSumNal += mask_wpl[i].m_kwota;
                else if (mask_wpl[i].m_typ == 3)
                    AllSumKoszty += mask_wpl[i].m_kwota;
                else
                {
                    AllSumOds += mask_wpl[i].m_kwota;
                    if (mask_wpl[i].m_typ == 11)
                        if (mask_wpl[i].prizn == 0)
                            AllSumProc += f_kwotaods_km(ref lid_Data, ref dat1, ref dat1, ref mask_wpl, size_k_wpl, mask_wpl[i].m_pos);

                }
            }

            if (kw == 0)
            {
                ts[size_ts].don = AllSumNal;
                ts[size_ts].doo = AllSumOds;
                ts[size_ts].dok = AllSumKoszty;
            }
            else
            {
                if (AllSumNal > 5)
                {
                ts[size_ts].don = AllSumNal;
                ts[size_ts].doo = AllSumOds;
                ts[size_ts].dok = AllSumKoszty;
                }
                else
                {
                    ts[size_ts].don = -kw;
                    ts[size_ts].doo = 0;
                    ts[size_ts].dok = 0;
                }
                ts[size_ts].don = AllSumNal;
                ts[size_ts].doo = AllSumOds;
                ts[size_ts].dok = AllSumKoszty;

            }
            ts[size_ts].dat = dat;
            ts[size_ts].typ = 'S';
            ts[size_ts].poo = AllSumProc;
            size_ts = size_ts + 1;
            Array.Resize<tabstan>(ref ts, size_ts + 1);
            for (i = 1; i <= size_ts - 1; i++)
            {
                if (i < size_ts - 1)
                {
                    if ((ts[i].typ == 'R') && (ts[i + 1].typ == 'R') && (ts[i].dat == ts[i + 1].dat))
                    {
                        ts[i + 1].don += ts[i].don;
                        ts[i + 1].dok += ts[i].dok;
                        ts[i + 1].doo += ts[i].doo;
                        ts[i + 1].kw = ts[i].kw;
                        continue;
                    }
                }
                lid_Data.size_licz_res = i;
                Array.Resize<licz_res>(ref lid_Data.mas_licz_res, lid_Data.size_licz_res + 1);
                lid_Data.mas_licz_res[i].sum_nal = ts[i].pon;
                lid_Data.mas_licz_res[i].sum_koszt = ts[i].pok;
                lid_Data.mas_licz_res[i].sum_ods = ts[i].poo;
                lid_Data.mas_licz_res[i].odz_dzien = 0;
                if (ts[i].typ == 'S')
                {   //dzien oblizenia
                    lid_Data.mas_licz_res[i].data_s = dat;
                    lid_Data.mas_licz_res[i].typ = 1;
                    lid_Data.mas_licz_res[i].sum_nal = ts[i].don;
                    lid_Data.mas_licz_res[i].sum_koszt = ts[i].dok;
                    lid_Data.mas_licz_res[i].sum_ods = ts[i].doo;
                    lid_Data.mas_licz_res[i].odz_dzien = ts[i].poo;
                }
                else
                {
                    lid_Data.mas_licz_res[i].data_s = ts[i].dat;
                    lid_Data.mas_licz_res[i].typ = 2;//wplata
                }
            }

            if (lid_Data.mas_licz_res[lid_Data.size_licz_res].sum_nal < 0)
            {
                for (i = 1; i <= lid_Data.size_naleznosc; i++)
                {
                    if (lid_Data.mas_naleznosc[i].date_p <= lid_Data.data_st)
                    {
                        lid_Data.mas_naleznosc[i].date_p = dat;
                        lid_Data.mas_naleznosc[i].kwota = 0;
                        lid_Data.mas_naleznosc[i].ods = 0;
                    }
                }
            }

            //form pusto mas_wpl_plan
            lid_Data.size_wpl_plan = 0;

            for (i = 1; i <= lid_Data.size_naleznosc; i++)
            {
                if ((lid_Data.mas_naleznosc[i].kwota > 0) || (lid_Data.mas_naleznosc[i].ods > 0))
                {
                    lid_Data.size_wpl_plan = lid_Data.size_wpl_plan + 1;
                    Array.Resize<wpl_podz>(ref lid_Data.mas_wpl_plan, lid_Data.size_wpl_plan + 1);
                    lid_Data.mas_wpl_plan[lid_Data.size_wpl_plan].ident = lid_Data.size_wpl_plan;
                    lid_Data.mas_wpl_plan[lid_Data.size_wpl_plan].id_nal = lid_Data.mas_naleznosc[i].ident;
                    lid_Data.mas_wpl_plan[lid_Data.size_wpl_plan].id_wplaty = 0;
                    lid_Data.mas_wpl_plan[lid_Data.size_wpl_plan].spl_nal = 0;
                    lid_Data.mas_wpl_plan[lid_Data.size_wpl_plan].spl_ods = 0;
                }
            }

            //razlik sum planov
            kw = lid_Data.suma_nowej_wplaty;
            if (kw > 0)
            {
                sposob = lid_Data.algo;
                if ((sposob == 1) || (sposob == 3))
                {  //KON || KNO
                    kw = f_plata_new(ref lid_Data, 3, ref kw);
                    if (kw == 0) return 0;
                    if (sposob == 1)
                    {  //KON
                        kw = f_plata_new(ref lid_Data, 11, ref kw);
                        if (kw == 0) return 0;
                        kw = f_plata_new(ref lid_Data, 1, ref kw);
                        if (kw == 0) return 0;
                    }
                    else  //KNO
                    {
                        kw = f_plata_new(ref lid_Data, 1, ref kw);
                        kw = f_plata_new(ref lid_Data, 11, ref kw);
                        if (kw == 0) return 0;
                    }
                }
                else if ((sposob == 2) || (sposob == 4))
                {  //OKN || ONK
                    kw = f_plata_new(ref lid_Data, 11, ref kw);
                    if (kw == 0) return 0;
                    if (sposob == 2)
                    {  //OKN
                        kw = f_plata_new(ref lid_Data, 3, ref kw);
                        if (kw == 0) return 0;
                        kw = f_plata_new(ref lid_Data, 1, ref kw);
                        if (kw == 0) return 0;
                    }
                    else
                    {  //ONK
                        kw = f_plata_new(ref lid_Data, 1, ref kw);
                        kw = f_plata_new(ref lid_Data, 3, ref kw);
                        if (kw == 0) return 0;
                    }
                }
                else
                {  //NKO || NOK
                    kw = f_plata_new(ref lid_Data, 1, ref kw);
                    if (kw == 0) return 0;
                    if (sposob == 5)
                    {  //NKO
                        kw = f_plata_new(ref lid_Data, 3, ref kw);
                        if (kw == 0) return 0;
                        kw = f_plata_new(ref lid_Data, 11, ref kw);
                        if (kw == 0) return 0;
                    }
                    else  //NOK
                    {
                        kw = f_plata_new(ref lid_Data, 11, ref kw);
                        if (kw == 0) return 0;
                        kw = f_plata_new(ref lid_Data, 3, ref kw);
                        if (kw == 0) return 0;
                    }
                }
            }

            return 0;
        }


        private int f_formlinetime(ref input_data input_struct, ref linetime[] ast_lnt, ref int ai_count)
        {
            DateTime? dat;
            int n, i, pos;
            int ind, count_nal;

            ai_count = 0;
            //form linetime dla nalezn.
            for (n = 1; n <= input_struct.size_naleznosc; n++)
            {
                if (Convert.ToDateTime(input_struct.data_st).Date >= Convert.ToDateTime(input_struct.mas_naleznosc[n].date_p).Date)
                {
                    ai_count = ai_count + 1;
                    Array.Resize<linetime>(ref ast_lnt, ai_count + 1);     // rozszerzenie tablicy
                    ast_lnt[ai_count].dat = input_struct.mas_naleznosc[n].date_p;
                    ast_lnt[ai_count].typ = 0;
                    ast_lnt[ai_count].numer_kwoty = n;
                    ind = 0;
                    for (i = 1; i <= input_struct.size_opis_nal; i++)
                    {
                        if (input_struct.mas_naleznosc[n].nr_nal == input_struct.mas_opis_nal[i].ident)
                        {
                            ast_lnt[ai_count].numer_nal = i;
                            ast_lnt[ai_count].typ_nal = input_struct.mas_opis_nal[i].typ_nal;
                            ind = 1;
                            break;
                        }
                    }
                    if (ind == 0) return -1;
                }
            } //for

            //form linetime dla odsetok
            count_nal = ai_count;
            for (n = 1; n <= count_nal; n++)
            {
                pos = ast_lnt[n].numer_kwoty;
                if (input_struct.mas_naleznosc[pos].czy_proc)
                {
                    ind = 0;
                    for (i = 1; i <= input_struct.size_ods_nal; i++)
                    {
                        if (input_struct.mas_ods_nal[i].id_nal == input_struct.mas_naleznosc[pos].ident)
                        {
                            if (Convert.ToDateTime(input_struct.mas_ods_nal[i].licz_do).Date >= Convert.ToDateTime(input_struct.mas_naleznosc[pos].date_p).Date)
                            {
                                ind = 1;
                                dat = input_struct.mas_ods_nal[i].licz_od;
                                if (Convert.ToDateTime(input_struct.mas_ods_nal[i].licz_od).Date < Convert.ToDateTime(input_struct.mas_naleznosc[pos].date_p).Date)
                                {
                                    dat = input_struct.mas_naleznosc[pos].date_p;
                                }
                                ai_count = ai_count + 1;
                                Array.Resize<linetime>(ref ast_lnt, ai_count + 1);
                                ast_lnt[ai_count].dat = dat;
                                ast_lnt[ai_count].typ = 1;
                                ast_lnt[ai_count].numer_kwoty = i;
                                ast_lnt[ai_count].numer_nal = pos;
                                ast_lnt[ai_count].czy_proc = true;
                                ast_lnt[ai_count].typ_nal = -1;

                                ai_count = ai_count + 1;
                                Array.Resize<linetime>(ref ast_lnt, ai_count + 1);
                                ast_lnt[ai_count].dat = input_struct.mas_ods_nal[i].licz_do;
                                ast_lnt[ai_count].typ = 2;
                                ast_lnt[ai_count].numer_kwoty = i;
                                ast_lnt[ai_count].numer_nal = pos;
                                ast_lnt[ai_count].czy_proc = true;
                                ast_lnt[ai_count].typ_nal = -1;
                            }
                        }
                    }
                    if (ind == 0)
                        ast_lnt[n].czy_proc = false;
                    else
                        ast_lnt[n].czy_proc = true;

                }
                else
                    ast_lnt[n].czy_proc = false;

            }





            for (n = 1; n <= input_struct.size_wplata; n++)
            {
                if (Convert.ToDateTime(input_struct.data_st).Date >= Convert.ToDateTime(input_struct.mas_wplata[n].data_w).Date)
                {
                    ai_count = ai_count + 1;
                    Array.Resize<linetime>(ref ast_lnt, ai_count + 1);
                    ast_lnt[ai_count].dat = input_struct.mas_wplata[n].data_w;
                    ast_lnt[ai_count].typ = 3;
                    ast_lnt[ai_count].numer_kwoty = n;
                    ast_lnt[ai_count].numer_nal = 0;
                    ast_lnt[ai_count].typ_nal = -1;
                }
            }


            //Zero wplata dla dnia stanu
            input_struct.size_wplata = input_struct.size_wplata + 1;
            Array.Resize<wplata>(ref input_struct.mas_wplata, input_struct.size_wplata + 1);
            input_struct.mas_wplata[input_struct.size_wplata].ident = input_struct.size_wplata;
            input_struct.mas_wplata[input_struct.size_wplata].kwota = -1;
            input_struct.mas_wplata[input_struct.size_wplata].data_w = input_struct.data_st;
            input_struct.mas_wplata[input_struct.size_wplata].algo = 1;
            ai_count = ai_count + 1;
            Array.Resize<linetime>(ref ast_lnt, ai_count + 1);
            ast_lnt[ai_count].dat = input_struct.data_st;
            ast_lnt[ai_count].typ = 4;
            ast_lnt[ai_count].numer_kwoty = input_struct.size_wplata;
            //	ast_lnt[ai_count].numer_kwoty = 0
            ast_lnt[ai_count].numer_nal = 0;
            ast_lnt[ai_count].typ_nal = -1;



            f_compare_l(ref ast_lnt, ai_count);
            return 0;
        }
        private void newk_wpl(ref k_wpl[] mask_wpl, ref int size_wpl, int typ, int pos, ref decimal kwota, ref DateTime? ldate, int prizn)
        {
            size_wpl = size_wpl + 1;
            Array.Resize<k_wpl>(ref mask_wpl, size_wpl + 1);
            mask_wpl[size_wpl].m_typ = typ;
            mask_wpl[size_wpl].m_pos = pos;
            mask_wpl[size_wpl].m_kwota = Math.Round(kwota, 2);
            mask_wpl[size_wpl].m_procent = 0;
            mask_wpl[size_wpl].m_ldate0 = ldate;
            mask_wpl[size_wpl].prizn = prizn;
        }
        private decimal f_computewpl(ref input_data input_struct, ref linetime[] lnt, int size_lnt, ref k_wpl[] mask_wpl, ref int size_k_wpl, ref po_wpl[] mas_po_wpl, ref int size_po_wpl, ref tabstan[] ts, ref int size_ts)
        {
            decimal kw;
            int? pzw;
            int n, i;

            i = 1;
            pzw = null;
            kw = 0;
            for (n = 1; n <= size_lnt; n++)
            {
                if (lnt[n].typ == 4) break;
                if (lnt[n].typ != 3) continue;
                if (i > 1) f_preddowpl(ref input_struct, ref lnt, n, ref i, ref mask_wpl, size_k_wpl);

                kw = f_dowpl(ref input_struct, ref lnt, n, ref i, ref ts, ref size_ts, ref mask_wpl, ref size_k_wpl, ref mas_po_wpl, ref size_po_wpl, ref pzw, ref kw);
                i = n + 1;
                //	kw = f_realwplaty(input_struct,mask_wpl,size_k_wpl,mas_po_wpl,size_po_wpl, &
                //							lnt,n,ts,size_ts,kw)

                kw = f_realwplaty_new(ref input_struct, ref mask_wpl, size_k_wpl, ref mas_po_wpl, ref size_po_wpl, ref lnt, n, ref ts, size_ts, ref kw);
                kw = Math.Round(kw, 2);
                if (kw > 0)
                {
                    ts[size_ts].pon = ts[size_ts].pon - kw;//treba chy nie????
                    ts[size_ts].typ = 'N';
                }
                size_ts = size_ts + 1;
                Array.Resize<tabstan>(ref ts, size_ts + 1);
                f_powplate(ref input_struct, ref mask_wpl, size_k_wpl);
                if (kw > 0)
                    pzw = lnt[n].numer_kwoty;
                else
                    pzw = null;

            }
            f_preddowpl(ref input_struct, ref lnt, n, ref i, ref mask_wpl, size_k_wpl);
            kw = f_dowpl(ref input_struct, ref lnt, n, ref i, ref ts, ref size_ts, ref mask_wpl, ref size_k_wpl, ref mas_po_wpl, ref size_po_wpl, ref pzw, ref kw);
            if (kw > 0)
            {
                kw = f_realwplaty_new(ref input_struct, ref mask_wpl, size_k_wpl, ref mas_po_wpl, ref size_po_wpl, ref lnt, n, ref ts, size_ts, ref kw); // był o f_realwplaty
                kw = Math.Round(kw, 2);
                f_powplate(ref input_struct, ref mask_wpl, size_k_wpl);
            }
            return kw;
        }
        private decimal f_kwotaods_km(ref input_data input_struct, ref DateTime? daysf, ref DateTime? dayse, ref k_wpl[] mask_wpl, int size_k_wpl, int ai_num)
        {
            decimal f_ods;
            decimal nal1;
            long dni;
            DateTime? daysFi, daysEi;
            int i, pos;

            f_ods = 0;
            pos = 0;
            if ((input_struct.mas_ods_nal).Length < ai_num) return f_ods;

            if ((Convert.ToDateTime(input_struct.mas_ods_nal[ai_num].licz_do).Date < Convert.ToDateTime(daysf).Date) || (Convert.ToDateTime(input_struct.mas_ods_nal[ai_num].licz_od).Date > Convert.ToDateTime(dayse).Date)) return 0;


            if (Convert.ToDateTime(input_struct.mas_ods_nal[ai_num].licz_od).Date > Convert.ToDateTime(daysf).Date)
            {
                daysf = input_struct.mas_ods_nal[ai_num].licz_od;
            }

            if (Convert.ToDateTime(input_struct.mas_ods_nal[ai_num].licz_do).Date < Convert.ToDateTime(dayse).Date)
            {
                dayse = input_struct.mas_ods_nal[ai_num].licz_do;
            }

            if (Convert.ToDateTime(dayse).Date < Convert.ToDateTime(daysf).Date) return 0;

            for (i = 1; i <= input_struct.size_naleznosc; i++)
            {
                if (input_struct.mas_naleznosc[i].ident == input_struct.mas_ods_nal[ai_num].id_nal)
                {
                    pos = i;
                    //ai_num
                    break;
                }
            }
            nal1 = -1;
            /*  Do czego to jest   ??? 26.05.2010 s.l.może dla ustalonych stanów ???? - trzeba sprawdzić   */
            for (i = 1; i <= size_k_wpl; i++)
            {
                //IF ((mask_wpl[i].m_pos = pos) and (mask_wpl[i].prizn = 0) )    THEN  // s.l. tak było 30.05.2010
                if ((mask_wpl[i].m_pos == pos) && (mask_wpl[i].prizn == 0) && (mask_wpl[i].m_typ <= 3))
                {
                    nal1 = mask_wpl[i].m_kwota;
                    break;
                }
            }

            if (nal1 == -1)
            {
                if ((input_struct.mas_naleznosc[pos].date_p) == null) return 0;
                nal1 = input_struct.mas_naleznosc[pos].kwota;
            }


            if (input_struct.mas_ods_nal[ai_num].id_tab_ods == 1)
            { 		// Rom: umowne = 1
                dni = (Convert.ToDateTime(dayse).Subtract(Convert.ToDateTime(daysf))).Days + 1;                       //  DaysAfter((daysf).Date,(dayse).Date)+1;* sprawdzić
                f_ods = (decimal)((double)(nal1 * dni * input_struct.mas_ods_nal[ai_num].proc) / 100.0);
            }
            else
            {
                for (i = 1; i <= input_struct.size_tab_ods; i++)
                {
                    if (input_struct.mas_ods_nal[ai_num].id_tab_ods == input_struct.mas_tab_ods[i].nr_tab)
                    {
                        if ((Convert.ToDateTime(input_struct.mas_tab_ods[i].licz_do).Date < Convert.ToDateTime(daysf).Date) || (Convert.ToDateTime(input_struct.mas_tab_ods[i].licz_od).Date > Convert.ToDateTime(dayse).Date)) continue;
                        if (Convert.ToDateTime(daysf).Date < Convert.ToDateTime(input_struct.mas_tab_ods[i].licz_od).Date)
                            daysFi = input_struct.mas_tab_ods[i].licz_od;
                        else
                            daysFi = daysf;

                        if (Convert.ToDateTime(dayse).Date > Convert.ToDateTime(input_struct.mas_tab_ods[i].licz_do).Date)
                            daysEi = input_struct.mas_tab_ods[i].licz_do;
                        else
                            daysEi = dayse;
                        dni = Convert.ToDateTime(daysEi).Subtract(Convert.ToDateTime(daysFi)).Days + 1; // dni = DaysAfter(Date(daysFi),Date(daysEi))+1
                        //f_ods = f_ods + nal*dni*input_struct.mas_tab_ods[ai_num].proc/100.0
                        f_ods = f_ods +  (decimal)((double)( nal1 * dni * input_struct.mas_tab_ods[i].proc) / 100.0);
                    }
                }
            }
            return f_ods;
        }
        private void f_formostnal(ref input_data input_struct, ref k_wpl[] mask_wpl, int size_k_wpl, ref decimal kwota)
        {
            DateTime? dat;
            decimal kw, AllOds;
            int n, i, k;
            int posN;
            int num_ods;

            //dat = DateTime(RelativeDate ( Date(input_struct.data_st), 1 ))  //*SL
            dat = input_struct.data_st;

            for (n = 1; n <= input_struct.size_naleznosc; n++)
            {
                if (input_struct.mas_naleznosc[n].date_p <= input_struct.data_st)
                {
                    AllOds = 0;
                    posN = 0;
                    for (i = 1; i <= size_k_wpl; i++)
                    {
                        if ((n == mask_wpl[i].m_pos) && ((mask_wpl[i].m_typ == 1) || (mask_wpl[i].m_typ == 2) || (mask_wpl[i].m_typ == 3)))
                        {
                            posN = i;
                            break;
                        }
                    }

                    input_struct.mas_naleznosc[n].date_p = dat;
                    for (k = 1; k <= input_struct.size_ods_nal; k++)
                    {
                        if (input_struct.mas_ods_nal[k].id_nal == input_struct.mas_naleznosc[n].ident)
                        {

                            for (i = 1; i <= size_k_wpl; i++)
                            {
                                if ((mask_wpl[i].m_typ > 3) && (mask_wpl[i].m_pos == k)) AllOds += mask_wpl[i].m_kwota;

                            }
                        }
                    }

                    if (posN > 0)
                    {
                        input_struct.mas_naleznosc[n].kwota = mask_wpl[posN].m_kwota;
                        num_ods = 0;
                        for (k = 1; k <= input_struct.size_ods_nal; k++)
                        {
                            if (input_struct.mas_ods_nal[k].id_nal == input_struct.mas_naleznosc[n].ident)
                            {
                                if ((input_struct.mas_ods_nal[k].licz_od <= dat) && (input_struct.mas_ods_nal[k].licz_do >= dat))
                                {
                                    num_ods = k;
                                    break;
                                }
                            }
                        }
                        kw = 0;
                        if (num_ods > 0) kw = f_kwotaods_km(ref input_struct, ref dat, ref dat, ref mask_wpl, size_k_wpl, num_ods);
                        input_struct.mas_naleznosc[n].ods = AllOds;
                        input_struct.mas_naleznosc[n].ods_dzien = kw;
                    }
                }
            } //for
        }
        private decimal f_plata_new(ref input_data input_struct, int typ, ref decimal kwota)
        {
            int n, k, i;
            int t, pos;
            t = 0;
            for (n = 1; n <= input_struct.size_naleznosc; n++)
            {
                for (k = 1; k <= input_struct.size_opis_nal; k++)
                {
                    if (input_struct.mas_naleznosc[n].nr_nal == input_struct.mas_opis_nal[k].ident)
                    {
                        t = input_struct.mas_opis_nal[k].typ_nal;
                        break;
                    }
                }
                pos = 0;
                for (i = 1; i <= input_struct.size_wpl_plan; i++)
                {
                    if (input_struct.mas_naleznosc[n].ident == input_struct.mas_wpl_plan[i].id_nal)
                    {
                        pos = i;
                        break;
                    }
                }
                if (pos == 0) continue;
                if ((t == typ) || ((typ == 11) && ((t == 2) || (t == 1))))
                {
                    switch (typ)
                    {
                        case 1:
                        case 3:
                            if (input_struct.mas_naleznosc[n].kwota <= kwota)
                            {
                                kwota = kwota - input_struct.mas_naleznosc[n].kwota;
                                input_struct.mas_wpl_plan[pos].spl_nal = input_struct.mas_naleznosc[n].kwota;
                            }
                            else
                            {
                                input_struct.mas_wpl_plan[pos].spl_nal = kwota;
                                kwota = 0;
                                break;
                            }
                            break;
                        case 11:
                            if (t == 2)
                            {
                                if (input_struct.mas_naleznosc[n].kwota <= kwota)
                                {
                                    kwota = kwota - input_struct.mas_naleznosc[n].kwota;
                                    input_struct.mas_wpl_plan[pos].spl_nal = input_struct.mas_naleznosc[n].kwota;
                                }
                                else
                                {
                                    input_struct.mas_wpl_plan[pos].spl_nal = kwota;
                                    kwota = 0;
                                    break;
                                }
                            }
                            else //NALEZNOSC
                            {
                                if (input_struct.mas_naleznosc[n].ods <= kwota)
                                {
                                    kwota = kwota - input_struct.mas_naleznosc[n].ods;
                                    input_struct.mas_wpl_plan[pos].spl_ods = input_struct.mas_naleznosc[n].ods;
                                }
                                else
                                {
                                    input_struct.mas_wpl_plan[pos].spl_ods = kwota;
                                    kwota = 0;
                                    break;
                                }
                            }
                            break;
                            //			IF (input_struct.mas_naleznosc[n].kwota<=kwota) THEN
                            //				IF (t = 2) THEN
                            //					kwota = kwota - input_struct.mas_naleznosc[n].kwota
                            //					input_struct.mas_wpl_plan[pos].spl_nal = input_struct.mas_naleznosc[n].kwota
                            //				ELSE
                            //					kwota = kwota - input_struct.mas_naleznosc[n].ods
                            //					input_struct.mas_wpl_plan[pos].spl_ods = input_struct.mas_naleznosc[n].ods					
                            //				END IF
                            //			ELSE
                            //				IF (t = 2) THEN
                            //					input_struct.mas_wpl_plan[pos].spl_nal = kwota
                            //					kwota = 0.0
                            //					EXIT
                            //				ELSE
                            //					input_struct.mas_wpl_plan[pos].spl_ods = kwota
                            //					kwota = 0.0
                            //					EXIT					
                            //				END IF
                            //			END IF
                    }
                }
            }
            return kwota;
        }
        private void f_preddowpl(ref input_data input_struct, ref linetime[] lnt, int lt_wpl, ref int odoldwpl, ref k_wpl[] mask_wpl, int size_k_wpl)
        {
            DateTime? daysF, daysE;
            decimal Rab;
            int n;
            int pr;
            int od;

            od = odoldwpl - 1;
            for (n = 1; n <= size_k_wpl; n++)
            {
                if (mask_wpl[n].m_typ == 11)
                {
                    pr = 0;
                    /*
                    FOR i=odoldwpl TO lt_wpl
                        //IF ((mask_wpl[n].m_pos = lnt[i].numer_kwoty) and (lnt[i].typ<>4))THEN 
                         IF ((lnt[i].typ = 4))THEN 
                            pr =1
                            EXIT
                        END IF
                    NEXT
                    */
                    if (lnt[odoldwpl].typ == 2) pr = 1;

                    if ((pr == 0) && od > 0)
                    {
                        daysE = lnt[lt_wpl].dat;
                        daysF = Convert.ToDateTime(lnt[od].dat).AddDays(1); // DateTime(RelativeDate(Date(lnt[od].dat), 1));
                        //			Rab = f_kwotaods_km(input_struct,daysF,daysE,mask_wpl, &   zmieniłem 
                        //									 size_k_wpl,lnt[n].numer_kwoty)
                        Rab = f_kwotaods_km(ref input_struct, ref daysF, ref daysE, ref mask_wpl, size_k_wpl, mask_wpl[n].m_pos);
                        mask_wpl[n].m_kwota = mask_wpl[n].m_kwota + Rab;
                    }
                }
            }
        }
        private decimal f_dowpl(ref input_data input_struct, ref linetime[] lnt, int lt_wpl, ref int OdOldWpl, ref tabstan[] ts, ref int size_ts, ref k_wpl[] mask_wpl, ref int size_k_wpl, ref po_wpl[] mas_po_wpl, ref int size_po_wpl, ref int? pzw, ref decimal kw)
        {
            DateTime? r_date, date_p;
            DateTime? daysF, daysE;
            decimal r_kw;
            decimal kwota;
            int Od;
            int num_nal, typ;
            int n, i;
            int pos;
            int lim_wpl;

            Od = OdOldWpl - 1;
            lim_wpl = lt_wpl - 1;
            pos = 0;
            r_date = null;
            for (n = OdOldWpl; n <= lim_wpl; n++)
            {

                switch (lnt[n].typ)
                {
                    case 0:
                        typ = lnt[n].typ_nal;				// typ naleznosci
                        num_nal = lnt[n].numer_kwoty;	// indeks w masive naleznosc
                        r_date = null;
                        r_kw = input_struct.mas_naleznosc[num_nal].kwota;
                        date_p = input_struct.mas_naleznosc[num_nal].date_p;
                        if (pzw == null)
                            newk_wpl(ref mask_wpl, ref size_k_wpl, typ, num_nal, ref r_kw, ref r_date, 0);
                        else
                        {
                            f_realnadplata(ref ts, ref size_ts, ref mask_wpl, ref size_k_wpl, ref mas_po_wpl, ref size_po_wpl, ref pzw, num_nal, typ, ref date_p, ref kw, ref r_kw);
                            if (kw >= 0 && typ == 1) input_struct.mas_naleznosc[num_nal].date_p = null;
                        }
                        break;
                    case 1:
                        num_nal = lnt[n].numer_kwoty;	// indeks w masive ods_nal
                        date_p = input_struct.mas_ods_nal[num_nal].licz_do;
                        if (date_p > lnt[lt_wpl].dat)
                        { // Koniec okresu dalej
                            typ = 11;
                            daysF = input_struct.mas_ods_nal[num_nal].licz_od;
                            daysE = lnt[lt_wpl].dat;
                            kwota = f_kwotaods_km(ref input_struct, ref daysF, ref daysE, ref mask_wpl, size_k_wpl, num_nal);
                            if (kwota == 0)
                                ;
                            else
                            {
                                r_date = null;
                                newk_wpl(ref mask_wpl, ref size_k_wpl, typ, num_nal, ref kwota, ref r_date, 0);
                            }
                        }
                        else
                        {
                            for (i = n + 1; i <= lt_wpl; i++)
                            {
                                if ((lnt[n].numer_kwoty == lnt[i].numer_kwoty) && (lnt[i].typ == 2))
                                {
                                    pos = i;
                                    break;
                                }
                            }
                            lnt[pos].typ = 5;
                            typ = 12;
                            daysF = input_struct.mas_ods_nal[num_nal].licz_od;
                            daysE = date_p;
                            kwota = f_kwotaods_km(ref input_struct, ref daysF, ref daysE, ref mask_wpl, size_k_wpl, num_nal);
                            if (kwota == 0)
                                ;
                            else
                            {
                                pos = lnt[n].numer_nal;
                                if ((pzw == null) || (input_struct.mas_naleznosc[pos].date_p > lnt[lt_wpl].dat))
                                    newk_wpl(ref mask_wpl, ref size_k_wpl, typ, num_nal, ref kwota, ref r_date, 0);
                                else
                                    f_realnadplata(ref ts, ref size_ts, ref mask_wpl, ref size_k_wpl, ref mas_po_wpl, ref size_po_wpl, ref pzw, num_nal, typ, ref daysF, ref kw, ref kwota);

                            }
                        }
                        break;
                    case 2:
                        num_nal = lnt[n].numer_kwoty;	// indeks w masive ods_nal
                        typ = 12;
                        daysF = Convert.ToDateTime(lnt[Od].dat).AddDays(1);
                        daysE = input_struct.mas_ods_nal[num_nal].licz_do;
                        kwota = f_kwotaods_km(ref input_struct, ref daysF, ref daysE, ref mask_wpl, size_k_wpl, num_nal);
                        if (kwota == 0)
                            ;
                        else
                        {
                            r_date = null;
                            pos = lnt[n].numer_nal;
                            if ((pzw == null) || (input_struct.mas_naleznosc[pos].date_p > lnt[lt_wpl].dat))
                                newk_wpl(ref mask_wpl, ref size_k_wpl, typ, num_nal, ref kwota, ref r_date, 0);
                            else
                            {
                                kwota = f_removek_wpl(ref mask_wpl, size_k_wpl, lnt[n].numer_kwoty);
                                f_realnadplata(ref ts, ref size_ts, ref mask_wpl, ref size_k_wpl, ref mas_po_wpl, ref size_po_wpl, ref pzw, num_nal, typ, ref daysE, ref kw, ref kwota);
                            }
                        }
                        break;
                }

            } // for
            OdOldWpl = n;

            return kw;
        }
        private void f_compare_l(ref linetime[] ast_lnt, int ai_count)
        {
            linetime lnt;
            int i, j;

            for (i = 1; i <= ai_count - 1; i++)
                for (j = i + 1; j <= ai_count; j++)
                    if ((ast_lnt[i].dat) > (ast_lnt[j].dat))
                    {
                        lnt = ast_lnt[i];
                        ast_lnt[i] = ast_lnt[j];
                        ast_lnt[j] = lnt;
                    }
        }
        private decimal f_realwplaty_new(ref input_data input_struct, ref k_wpl[] mask_wpl, int size_k_wpl, ref po_wpl[] mas_po_wpl, ref int size_po_wpl, ref linetime[] lnt, int ln_wpl, ref tabstan[] ts, int tsactive, ref decimal kw)
        {
            decimal kwota;
            decimal oldkw, rw;
            int n, k, j, i;
            int pos_wpl, t;
            int q;

            pos_wpl = lnt[ln_wpl].numer_kwoty;
            f_tsline(ref input_struct, ref mask_wpl, size_k_wpl, ref ts, tsactive, pos_wpl);
            ts[tsactive].kw = input_struct.mas_wplata[pos_wpl].kwota + kw;
            kwota = input_struct.mas_wplata[pos_wpl].kwota + kw;

            oldkw = kwota;
            for (n = 1; n <= input_struct.size_wpl_podz; n++)
            {
                if (input_struct.mas_wplata[pos_wpl].ident == input_struct.mas_wpl_podz[n].id_wplaty)
                {
                    for (k = 1; k <= input_struct.size_naleznosc; k++)
                    {
                        if (input_struct.mas_naleznosc[k].ident == input_struct.mas_wpl_podz[n].id_nal)
                        {
                            q = 0;
                            for (i = 1; i <= size_k_wpl; i++)
                            {
                                t = mask_wpl[i].m_typ;
                                if (t == 12) t = 11;
                                j = mask_wpl[i].m_pos;
                                if (t == 11)
                                {
                                    if (mask_wpl[i].prizn == 0)
                                    {
                                        if (input_struct.mas_naleznosc[k].ident != input_struct.mas_ods_nal[j].id_nal) continue;
                                    }
                                    else
                                        if (j != k) continue;
                                }
                                else
                                {
                                    if (j != k) continue;
                                }

                                if (t == 11)
                                {
                                    if (q == 0)
                                    {
                                        rw = input_struct.mas_wpl_podz[n].spl_ods;
                                        kwota = kwota - rw;
                                        newpo_wpl(ref mas_po_wpl, ref size_po_wpl, t, mask_wpl[i].m_pos, ref rw, ref mask_wpl[i].m_kwota);
                                        mask_wpl[i].m_kwota = mask_wpl[i].m_kwota - rw;
                                        //						ts[tsactive].poO = ts[tsactive].poO - (oldkw -rw)
                                        ts[tsactive].poo = ts[tsactive].poo - rw;
                                        q = q + 1;
                                    }
                                }
                                else
                                {
                                    rw = input_struct.mas_wpl_podz[n].spl_nal;
                                    kwota = kwota - rw;
                                    newpo_wpl(ref mas_po_wpl, ref size_po_wpl, t, mask_wpl[i].m_pos, ref rw, ref mask_wpl[i].m_kwota);
                                    mask_wpl[i].m_kwota = mask_wpl[i].m_kwota - rw;
                                    if (t == 1)
                                        //							ts[tsactive].poN = ts[tsactive].poN - (oldkw -rw)							
                                        ts[tsactive].pon = ts[tsactive].pon - rw;
                                    else if (t == 3)
                                        //							ts[tsactive].poK = ts[tsactive].poK - (oldkw -rw)						
                                        ts[tsactive].pok = ts[tsactive].pok - rw;
                                    else if (t == 2)
                                    {
                                        //							ts[tsactive].poO = ts[tsactive].poO - (oldkw -rw)							
                                        ts[tsactive].poo = ts[tsactive].poo - rw;
                                    }
                                } // else t==11
                            }
                        }//if

                    } //for
                } // if
            } // for

            return kwota;
        }
        private void f_powplate(ref input_data input_struct, ref k_wpl[] mask_wpl, int size_k_wpl)
        {
            decimal dwSum = 0;
            int n, k, i;
            int typ, pos;

            for (n = 1; n <= input_struct.size_naleznosc; n++)
            {
                typ = -1;
                if (input_struct.mas_naleznosc[n].date_p > input_struct.data_st) continue;
                for (k = 1; k <= input_struct.size_opis_nal; k++)
                {
                    if (input_struct.mas_naleznosc[n].nr_nal == input_struct.mas_opis_nal[k].ident)
                    {
                        typ = input_struct.mas_opis_nal[k].typ_nal;
                        break;
                    }
                }

                //IF ((typ<>1) or (typ<>2)) THEN CONTINUE
                if (typ != 1) continue;
                dwSum = 0;
                if (typ == 1)
                {
                    for (i = 1; i <= size_k_wpl; i++)
                    {
                        if (mask_wpl[i].m_typ != 1) continue;
                        if (mask_wpl[i].m_pos != n) continue;
                        dwSum = dwSum + mask_wpl[i].m_kwota;
                        break;
                    }
                }
                for (i = 1; i <= size_k_wpl; i++)
                {
                    if ((mask_wpl[i].m_typ != 11) && (mask_wpl[i].m_typ != 12)) continue;
                    pos = mask_wpl[i].m_pos;
                    if (input_struct.mas_naleznosc[n].ident != pos) continue;
                    dwSum = dwSum + mask_wpl[i].m_kwota;
                }
                if (dwSum != 0) continue;

                if (typ == 1)
                {
                    for (i = 1; i <= size_k_wpl; i++)
                    {
                        if (mask_wpl[i].m_typ != 1) continue;
                        if (mask_wpl[i].m_pos != n) continue;
                        //mask_wpl[i].m_typ	= -1     *SL opcjonalenie tu odkomentowć a zmienic f_formostnal -  Slawek
                        break;
                    }
                }
                for (i = 1; i <= size_k_wpl; i++)
                {
                    if ((mask_wpl[i].m_typ != 11) && (mask_wpl[i].m_typ != 12)) continue;
                    pos = mask_wpl[i].m_pos;
                    if (input_struct.mas_naleznosc[n].ident != pos) continue;
                    //		IF (typ = 1) THEN CONTINUE
                    mask_wpl[i].m_typ = -1;
                }
            } // for;
        }
        private void f_realnadplata(ref tabstan[] ts, ref int size_ts, ref k_wpl[] mask_wpl, ref int size_k_wpl, ref po_wpl[] mas_po_wpl, ref int size_po_wpl, ref int? pzw, int pos, int typ, ref DateTime? dat, ref decimal kw, ref decimal kwota)
        {
            decimal r_kw;
            DateTime? r_date;

            //size_ts = size_ts + 1
            ts[size_ts].don = 0;
            ts[size_ts].doo = 0;
            ts[size_ts].dok = 0;
            ts[size_ts].pon = 0;
            ts[size_ts].poo = 0;
            ts[size_ts].pok = 0;
            ts[size_ts].dat = dat;
            ts[size_ts].kw = kw;
            ts[size_ts].typ = 'R';

            if (typ == 1)
            {  //Nal
                ts[size_ts].pon = kwota;
                ts[size_ts].don = kwota;
            }
            else if (typ == 3)
            { //koszty
                ts[size_ts].pok = kwota;
                ts[size_ts].dok = kwota;
            }
            else //Ods
            {
                ts[size_ts].poo = kwota;
                ts[size_ts].doo = kwota;
            }

            if (kwota <= kw)
            {
                kw = kw - kwota;
                newpo_wpl(ref mas_po_wpl, ref size_po_wpl, typ, pos, ref kwota, ref kwota);
                if (kw == 0) pzw = null;
            }
            else
            {
                newpo_wpl(ref mas_po_wpl, ref size_po_wpl, typ, pos, ref kw, ref kwota);
                r_kw = kwota - kw;
                r_date = null;
                newk_wpl(ref mask_wpl, ref size_k_wpl, typ, pos, ref r_kw, ref r_date, 0);
                kw = 0;
                //	pzw = 0
                pzw = null;
            }

            if (typ == 1)
            {
                ts[size_ts].pon = ts[size_ts].pon - (ts[size_ts].kw - kw);
            }
            else if (typ == 3)
            {
                ts[size_ts].pok = ts[size_ts].pok - (ts[size_ts].kw - kw);
            }
            else
                ts[size_ts].poo = ts[size_ts].poo - (ts[size_ts].kw - kw);

            size_ts = size_ts + 1;
            Array.Resize<tabstan>(ref ts, size_ts + 1);
        }
        private decimal f_removek_wpl(ref k_wpl[] mask_wpl, int size_k_wpl, int pos)
        {
            decimal kw;
            int n;

            kw = 0;
            for (n = 1; n <= size_k_wpl; n++)
            {
                if (mask_wpl[n].m_pos == pos)
                {
                    kw = mask_wpl[n].m_kwota;
                    mask_wpl[n].m_kwota = 0;
                    //		mask_wpl[n].m_typ = -1
                    //		mask_wpl[n].m_procent = 0.0
                    //		SetNull(mask_wpl[n].m_ldate0)
                    //		mask_wpl[n].m_pos = 0
                    //		mask_wpl[n].prizn = 0
                    break;
                }
            }
            return kw;
        }
        private void f_tsline(ref input_data input_struct, ref k_wpl[] mask_wpl, int size_k_wpl, ref tabstan[] tbs, int ts_active, int pos_wpl)
        {
            int n, k;
            int pos;


            tbs[ts_active].don = 0;
            tbs[ts_active].doo = 0;
            tbs[ts_active].dok = 0;

            for (n = 1; n <= Math.Min(size_k_wpl, mask_wpl.Length); n++)
            {
                if (mask_wpl[n].m_kwota == 0) continue;

                switch (mask_wpl[n].m_typ)
                {
                    case 1:
                        tbs[ts_active].don = tbs[ts_active].don + mask_wpl[n].m_kwota;
                        break;
                    case 11:
                    case 12:
                        pos = 0;
                        for (k = 1; k <= Math.Min(input_struct.size_naleznosc, (input_struct.mas_naleznosc).Length); k++)
                        {
                            if ((input_struct.mas_ods_nal).Length > 0)
                            {
                                if (input_struct.mas_naleznosc[k].ident == input_struct.mas_ods_nal[Math.Min(mask_wpl[n].m_pos, (input_struct.mas_ods_nal).Length)].id_nal)
                                {
                                    pos = k;
                                    break;  /////////********************** tu trzeba poprawić ezit z pętli
                                }
                            }
                        }
                        if (pos > 0)
                        {
                            if (input_struct.mas_naleznosc[pos].date_p > input_struct.mas_wplata[pos_wpl].data_w)
                                continue;
                            else
                                tbs[ts_active].doo = tbs[ts_active].doo + mask_wpl[n].m_kwota;
                        }
                        break;
                    case 2:
                        tbs[ts_active].doo = tbs[ts_active].doo + mask_wpl[n].m_kwota;
                        break;
                    case 3:
                        tbs[ts_active].dok = tbs[ts_active].dok + mask_wpl[n].m_kwota;
                        break;
                }

            }
            tbs[ts_active].pon = tbs[ts_active].don;
            tbs[ts_active].poo = tbs[ts_active].doo;
            tbs[ts_active].pok = tbs[ts_active].dok;
            tbs[ts_active].dat = input_struct.mas_wplata[pos_wpl].data_w;
            tbs[ts_active].typ = 'W';
        }
        private void newpo_wpl(ref po_wpl[] maspo_wpl, ref int size_po_wpl, int typ, int pos, ref decimal kwota, ref decimal kwotado)
        {
            size_po_wpl = size_po_wpl + 1;
            Array.Resize<po_wpl>(ref maspo_wpl, size_po_wpl + 1);
            maspo_wpl[size_po_wpl].m_typ = typ;
            maspo_wpl[size_po_wpl].m_pos = pos;
            maspo_wpl[size_po_wpl].m_kwota = kwota;
            maspo_wpl[size_po_wpl].m_kwotado = kwotado;
        }

        public decimal GetResults(ref List<CalcNaleznosc> lstNal, ref List<CalcResults> lstResults)
        {
            CalcNaleznosc cnal;
            CalcResults cresult;
            decimal all = 0;
            if (lstNal == null) lstNal = new List<CalcNaleznosc>();
            lstNal.Clear();

            foreach (var nal in lid_Data.mas_naleznosc)
            {
                if (nal.ident > 0)
                {
                    cnal = new CalcNaleznosc();
                    cnal.ident = nal.ident;
                    cnal.kwota = nal.kwota;
                    cnal.nr_nal = nal.nr_nal;
                    cnal.czy_proc = nal.czy_proc;
                    cnal.ods = nal.ods;
                    cnal.ods_dzien = nal.ods_dzien;
                    cnal.typ_nal = nal.typ_nal;
                    // ustalenie typu należnosci
                    foreach (var nopis in lid_Data.mas_opis_nal)
                    {
                        if (nopis.ident == cnal.nr_nal)
                        {
                            cnal.nr_nal = nopis.typ_nal;
                            break;
                        }
                    }

                    // 1 - naleznosći 0 - koszty
                    lstNal.Add(cnal);
                    all = cnal.kwota + cnal.ods;
                }
            }


            if (lstResults == null) lstResults = new List<CalcResults>();
            lstResults.Clear();
            foreach (var res in lid_Data.mas_licz_res)
            {
                if (res.typ != 0)
                {
                    cresult = new CalcResults();
                    cresult.data_s = res.data_s;
                    cresult.ods_dzien = res.odz_dzien;
                    cresult.sum_koszt = res.sum_koszt;
                    cresult.sum_nal = res.sum_nal;
                    cresult.sum_ods = res.sum_ods;
                    cresult.typ = res.typ;
                    lstResults.Add(cresult);
                }
            }
            return all;
        }

    }
}



