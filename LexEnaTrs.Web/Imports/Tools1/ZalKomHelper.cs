using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using WienaDB.Models;
using System.Web;
using System.Data.SqlClient;

namespace LexEnaTrs.Web.Tools1
{
    public class ZalKomHelper
    {
      

            private  int NalSpraw_Sygnatura_Col = 2; // kolumn a sygnatury

        public ZalKomHelper()
        {

        }

        public ZalKomHelper(int tryb)
        {
            switch (tryb)
            {
                case 0: // zaliczki
                    NalSpraw_Sygnatura_Col = 2;
                    break;
                case 1:
                    NalSpraw_Sygnatura_Col = 1;
                    break;

            }

        }


        private DataTable XLSX2Datatable(string inXlsxDOC, string filename)
            {

                byte[] data = Convert.FromBase64String(inXlsxDOC);


                if (data == null)
                {
                    Utils.LogWriter("Błąd w trakcie odczytu zbioru xlsx potwierdzenia - zbiór jest pusty");
                    return null;

                }
                Stream stream = new MemoryStream(data);

                IExcelDataReader excelReader;
                var file = new FileInfo(filename);

                if (file.Extension.Equals(".xls"))
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                else
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                excelReader.IsFirstRowAsColumnNames = false;
                DataSet result = excelReader.AsDataSet();
                return result.Tables[0];
            }

        public string InsertZaliczki(string xlsxBase64, string filename, int mode)
        {
            List<ZaliczkiImportData> zal = null;
            try
            {
                zal = ToXMLSerializers.DeserializeFromString(xlsxBase64, typeof(List<ZaliczkiImportData>)) as List<ZaliczkiImportData>;

            }
            catch (Exception ex)
            {
                return "Błąd deserializacji " + ex.Message;

            }
            if (zal == null)
                return "Błąd - zbiór jest pusty";
            try
            {
                using (wiena_centralEntities context = new wiena_centralEntities())
                {
                    xls_import x = new xls_import();
                    x.data = DateTime.Now;
                    x.nazwa_pliku = filename;
                    x.operator_guid = 1;
                    context.xls_import.Add(x);
                    context.SaveChanges();
                    foreach (ZaliczkiImportData z in zal)
                    {
                        if (string.IsNullOrWhiteSpace(z.Sygnatura))
                            break;
                        xls_zalkom_det xzd = new xls_zalkom_det();
                        xzd.czyus = 0;
                        xzd.lp = z.Lp;
                        xzd.id_import = x.ident;
                        xzd.kwota = z.Kwota;
                        xzd.sygnatura = z.Sygnatura;
                        xzd.data = z.DataZaliczki;
                        xzd.dluznik = z.Dluznik.Truncate(300);
                        xzd.oddzial = z.Oddzial.Truncate(30);
                        xzd.sprawa_ident = z.id_sprawy;
                        xzd.komentarz = z.Uwagi.Truncate(50);
                        xzd.uwagi = z.Error;
                        xzd.status = 1;
                        context.xls_zalkom_det.Add(xzd);
                        if (mode > 0) // jeśli od razu z zapisem
                        {
                            if (z.Zapis)
                            {
                                naleznosc n = new naleznosc();
                                n.id_typ_nal = 17;
                                n.id_sprawy = z.id_sprawy;
                                n.data_n = z.DataZaliczki;
                                n.data_fv = z.DataZaliczki;
                                n.czy_proc = false;
                                n.kwota = z.Kwota;
                                n.uwagi = z.Uwagi.Truncate(80);
                                n.vat = 0;
                                z.Error = xzd.uwagi = "Koszt dopisany do sprawy.";
                                xzd.status = 2;

                                context.naleznosc.Add(n);

                            }
                        }
                     
                    }
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
               return "Błąd importu " + ex.Message;
            }

             return ToXMLSerializers.SerializeToString(zal, zal.GetType(), false);

        }


        public string UsunKoszty(string xlsxBase64, string filename)
        {
            List<ZaliczkiImportData> zal = null;
            try
            {
                zal = ToXMLSerializers.DeserializeFromString(xlsxBase64, typeof(List<ZaliczkiImportData>)) as List<ZaliczkiImportData>;

            }
            catch (Exception ex)
            {
                return "Błąd deserializacji " + ex.Message;

            }
            if (zal == null)
                return "Błąd - zbiór jest pusty";
            try
            {
                using (wiena_centralEntities context = new wiena_centralEntities())
                {
                    foreach (ZaliczkiImportData z in zal)
                    {
                        if (z.Zapis == false) continue;
                        try
                        {
                            object obj = context.sp_RemoveCosts(z.id_sprawy);// .Database.SqlQuery<object>("sp_MergeCases  @IdSrc, @IdDest", param1, param2);
                        }

                        catch (Exception ex)
                        {

                            return "error: " + ex.Message;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return "Błąd importu " + ex.Message;
            }

            return ToXMLSerializers.SerializeToString(zal, zal.GetType(), false);

        }

        public string GetFileContent(string xlsxBase64, string filename)
            {
                bool anystat = false;
                DateTime? dzlo = null;
                try
                {
                    List<ZaliczkiImportData> lst = parseXLSX(xlsxBase64, filename);

                    if (lst == null)
                        return "Błąd parsowania zbioru sprawdź poprawność jego zawartości";
                    if (!lst.Any())
                        return "Błąd Zbiór jest pusty";
                // przepisanie do listy 


                return ToXMLSerializers.SerializeToString(lst, lst.GetType(), false);    
                }
                catch (Exception ex)
                {

                    return "Błąd: " + ex.Message;
                }

            }
        private List<ZaliczkiImportData> validateInWiena(List<ZaliczkiImportData> lst, int tryb )
        {
            if (lst == null) return null;
            using (wiena_centralEntities context = new wiena_centralEntities())
            {
                foreach (ZaliczkiImportData z in lst)
                {
                    sprawa s = context.sprawa.Where(a => a.sygnatura == z.Sygnatura).FirstOrDefault();
                    if (s == null)
                    {
                        z.Zapis = false;
                        z.Error = "brak sprawy o takiej sygnaturze";
                    }
                    else
                        z.id_sprawy = s.ident;

                    if (tryb == 1 && z.id_sprawy > 0 )
                    {
                        List<naleznosc> lstNal = context.naleznosc.Where(a => a.id_sprawy == z.id_sprawy && (a.id_typ_nal == 12 || a.id_typ_nal == 8)).ToList();
                        if (lstNal != null && lstNal.Count() > 0)
                        {
                            int czyWplaty = 0;
                            foreach (naleznosc nal in lstNal)
                            {
                                czyWplaty = (from n in context.naleznosc
                                             join w in context.wplata_podz on n.ident equals w.id_nal
                                             where n.ident == nal.ident && w.spl_kap > 0
                                             select w).Count();


                                if (czyWplaty > 0)
                                {
                                    z.Zapis = false;
                                    z.Error = "w  sprawie są wpłaty na koszty";
                                    break;
                                }
                            }
                        }
                        else
                        {
                            z.Zapis = false;
                            z.Error = "brak kosztów w podanej sprawie";

                        }



                    }
                }

            }
            return lst;
        }

         
            private List<ZaliczkiImportData> parseXLSX(string inXlsxDOC, string filename)
            {
            List<ZaliczkiImportData> lst = new List<ZaliczkiImportData>();

            DataTable worksheet = XLSX2Datatable(inXlsxDOC, filename);

            if (worksheet.Rows.Count > 0)
            {

                int columnsNo = worksheet.Columns.Count;


                int rowNo = 0;
                int lp = 0;
                
                foreach (DataRow row in worksheet.Rows)
                {

                    string val = row[0].ToString();
                    rowNo++;
                    if (!int.TryParse(val, out lp)) continue; //( leśli nie liczba) 

                    if (String.IsNullOrWhiteSpace(val) && String.IsNullOrWhiteSpace(row[NalSpraw_Sygnatura_Col].ToString())) break; // jeśłi w ierwszej kolumnie jest coś to sprawdzamy 
                    ZaliczkiImportData u = new ZaliczkiImportData();
                    u.Sygnatura = row[NalSpraw_Sygnatura_Col] as string ?? "";
                    u.Sygnatura = u.Sygnatura.Replace(" ", "");
                    u.Lp = lp;
                    u.Dluznik = row[NalSpraw_Sygnatura_Col + 1] as string ?? "";
                    if (NalSpraw_Sygnatura_Col == 2)
                    {
                        u.Oddzial = row[NalSpraw_Sygnatura_Col - 1] as string ?? "";
                        u.Uwagi = row[NalSpraw_Sygnatura_Col + 4] as string ?? "";
                    }
                        u.Zapis = true;
                    DateTime d_zaliczki;
                    if (row[NalSpraw_Sygnatura_Col + 2] != null)
                    {
                        
                            string s = row[NalSpraw_Sygnatura_Col + 2].ToString().Replace("/", "-").Replace(".", "-");
                        if (!DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d_zaliczki))
                        {
                            if (!DateTime.TryParseExact(s, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d_zaliczki))
                            {
                                try
                                {
                                    d_zaliczki = Convert.ToDateTime(row[NalSpraw_Sygnatura_Col + 2]);
                                    u.DataZaliczki = d_zaliczki;
                                }
                                catch
                                {
                                    u.Zapis = false;
                                    u.Error += "błędny format daty w wierszu " + lp.ToString() + " " + u.Sygnatura;
                                }
                            }
                            else
                                u.DataZaliczki = d_zaliczki;
                        }
                        else
                            u.DataZaliczki = d_zaliczki;

                    }
                    else
                    {
                        u.Zapis = false;
                        u.Error += "błędny format daty w wierszu " + lp.ToString() + " " + u.Sygnatura;
                        
                    }

                    decimal result = 0;
                    if (NalSpraw_Sygnatura_Col == 2) // jeśli zaliczki
                    {
                        if (row[NalSpraw_Sygnatura_Col + 3] != null)
                        {
                            string s = row[NalSpraw_Sygnatura_Col + 3].ToString().Replace(",", ".");

                            if (decimal.TryParse(s, NumberStyles.Number | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result))
                                u.Kwota = result;
                            else
                            {
                                u.Zapis = false;
                                u.Error += "błędny format kwoty w wierszu " + lp.ToString() + " " + u.Sygnatura;
                            }
                        }
                        else
                        {
                            u.Zapis = false;
                            u.Error += "błędny format kwoty w wierszu " + lp.ToString() + " " + u.Sygnatura;

                        }
                    }
                    lst.Add(u);
                }
                return validateInWiena( lst, NalSpraw_Sygnatura_Col==2 ? 0 :1) ;
            }
            else
                return null;
        }

        }
  
}
