using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LexEnaTrs.Web.Tools1
{
    public class PotwierdzSadaHelper
    {
      

            private const int PotwSald_Sygnatura_Col = 3; // kolumn a sygnatury
            private const int SkutKanc_DateTime_Col = 2;

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

            public string GetFileContent(string xlsxBase64, string dataStanu, int typFirmy,bool format2 = false)
            {
                bool anystat = false;
                DateTime dzlo;
                Guid _guid = Guid.NewGuid();
            try
                {
                    List<PotwSald> lst = parseXLSX(xlsxBase64, dataStanu, out _guid);
                    if (!DateTime.TryParse(dataStanu , out dzlo))
                        return "Błędna data obliczenia odsetek";
                if (lst == null)
                        return "Błąd parsowania zbioru sprawdź poprawność jego zawartości";
                    if (!lst.Any())
                        return "Błąd Zbiór jest pusty";
                    // przepisanie do listy 
                 
                    using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                    {
                        foreach (PotwSald u in lst)
                        {
                        if (format2 == true)
                        {
                            u.ZgodnoscSalda = dataStanu;
                            

                        }
                            context.PotwSald.AddObject(u);

                        }
                        context.SaveChanges();
                    liczZaleglosc(_guid, dzlo, typFirmy);
                    }
                    return _guid.ToString();

                }
                catch (Exception ex)
                {

                    return "Błąd: " + ex.Message;
                }

            }



        private void liczZaleglosc(Guid pakietId, DateTime dStanu, int typFirmy)
        {
            ObliczZaleglosc oZal = new ObliczZaleglosc();
            List<WienaDB.Models.typ_nal> tNalLst = null; 
            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {
                List<PotwSald> pLst = context.PotwSald.Where(a => a.Pakiet_Id == pakietId).ToList();
                
                using (WienaDB.Models.wiena_centralEntities wiena = new WienaDB.Models.wiena_centralEntities())
                {
                    tNalLst = wiena.typ_nal.ToList();
                    foreach (PotwSald p in pLst)
                    {
                        WienaDB.Models.sprawa s = (from spr in wiena.sprawa
                                                   join f in wiena.firma on spr.id_firmy equals f.ident
                                                   where spr.sygnatura == p.Sygnatura && f.typ_firmy == typFirmy
                                                   select spr).FirstOrDefault();
                        

                        if (s != null)
                        {
                            p.IdSprawyWiena = s.ident;
                            if (String.IsNullOrWhiteSpace(p.NrBilling))
                            {
                                p.NrBilling = s.nr_ewid;
                            }
                            
                        }
                        try
                        {
                            if (String.IsNullOrWhiteSpace(p.Dluznik))
                            {
                                Sprawa spr = context.Sprawa.Where(a => a.sygnatura == p.Sygnatura).FirstOrDefault();
                                if (spr != null)
                                {
                                    DaneDluznika ddl = context.DaneDluznika.Where(a => a.Sprawa_Id == spr.id).FirstOrDefault();
                                    if (ddl != null)
                                    {
                                        p.Dluznik = (ddl.Nazwa  + " " + ddl.Imie  + " " + ddl.Nazwisko ).Trim();
                                    }
                                    if (String.IsNullOrWhiteSpace(p.Oddzial))
                                    {
                                        vw_DaneSprawy vwDD = context.vw_DaneSprawy.Where(a => a.id == spr.id).FirstOrDefault();
                                        if (vwDD != null)
                                        {
                                            p.Oddzial = vwDD.Oddzial;

                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception)
                        { }

                    }
                }
                foreach (PotwSald p in pLst)
                {
                    oZal.ObliczSpraweWiena(p.IdSprawyWiena ?? 0, dStanu);
                    if (oZal.wynikiAll != null && oZal.wynikiAll.Count > 0)
                    {   
                        decimal? kk = oZal.wynikiNal.Where(n => n.typ_nal == 9 || n.typ_nal == 13).Sum(a => a.kwota);
                        
                        p.KosztySadowe =  0;
                        p.NalGlowna =  0;
                        p.Odsetki = 0;

                        foreach (var nale in oZal.wynikiNal)
                        {
                            WienaDB.Models.typ_nal  tTyp = tNalLst.Where(a => a.ident == nale.typ_nal).FirstOrDefault();
                            if (tTyp != null)
                            {
                                switch (tTyp.typ_nal1)
                                {
                                    case 1:
                                    case 4:
                                        p.NalGlowna += nale.kwota;
                                        p.Odsetki += nale.ods;
                                        break;
                                    case 3:
                                        p.KosztySadowe += nale.kwota;

                                        break;
                                    case 2:
                                        p.Odsetki += nale.kwota;
                                        p.Odsetki += nale.ods;
                                        break;
                                    default:
                                        break;

                                }

                            }


                        }
                        p.KosztyKlauzuli = kk ?? 0 ;
                        p.KosztySadowe -= kk;
                        //p.NalGlowna = oZal.wynikiAll.ElementAt(oZal.wynikiAll.Count - 1).sum_nal;
                        //p.Odsetki += oZal.wynikiAll.ElementAt(oZal.wynikiAll.Count - 1).sum_ods;
                        context.SaveChanges();
                    }



                }

                try
                {
                     SqlParameter param1 = new SqlParameter("IdPakiet", pakietId);


                        object obj = context.ExecuteStoreQuery<object>("spPotwSaldEgz_AktualKorekta  @IdPakiet", param1).FirstOrDefault();
                       
                    
                }
                catch (Exception ex)
                {

                  Utils.LogWriter("spPotwSaldEgz_AktualKorekta error: " + ex.Message);
                }

            }
        }
            

        private Tuple<int,int> sygnaturaColNo(DataTable worksheet)
        {

            int i = 0;
            errDescription theError = new errDescription();
            theError.level = ErrLevel.OK;
            theError.code = 0;
            theError.reference = "wiersz " + i.ToString();
            int columnsNo = worksheet.Columns.Count;

            Regex r = new Regex(@"[A-Z][A-Za-z]{0,5}-\d{1,5}[/]\d{4}");

            bool isinside = false;
            int rowNo = -1;
            int sygnCol = 0;
            int sygnRow = 0;
            foreach (DataRow row in worksheet.Rows)
            {
                rowNo++;
                // pominięcie tych, ktore nie mają w pierwszej lub drugiej kolumnie 
                if (!isinside)
                {
                    string c;


                    for (int col = 0; col < 8; col++)
                    {
                        string val = row[col] as string != null ? row[col] as string : "";
                        Match m = r.Match(val);
                        if (m.Success)
                        {
                            isinside = true;
                            sygnRow = rowNo;
                            return new Tuple<int, int>(col, rowNo);
                           
                        }

                    }
                }
            }
            return null;
        }


        private PotwSald setupPotwSald(DataRow row)
        {
            int i;
            PotwSald p = new PotwSald();
            try
            {
                if (int.TryParse(row[0].ToString(), out i))
                {
                    p.Lp = i;
                }
                p.NrNaszSprawy = row[1].ToString();
                p.NrBilling = row[2].ToString();
                p.Dluznik = row[4].ToString();
                p.Oddzial = row[5].ToString();
                try
                {
                    p.ZgodnoscSalda = row[11].ToString();
                    p.Uwagi = row[12].ToString();
                    p.NrTytulu = row[13].ToString();
                }
                catch (Exception)
                {
                    ;

                }
                

            }
            catch (Exception ex)
            {

                Utils.LogWriter("Błąd podczas wczytywania zbioru " + ex.Message);
                return null;
            }
            return p;

        }



        private List<PotwSald> parseXLSX(string inXlsxDOC, string filename, out Guid pakietid)
            {
                List<PotwSald> lst = new List<PotwSald>();
                int rowNo = 0;
                pakietid  = Guid.NewGuid();

                DataTable worksheet = XLSX2Datatable(inXlsxDOC, filename);

                if (worksheet.Rows.Count > 0)
                {

                    int columnsNo = worksheet.Columns.Count;


                    bool isinside = false;
                    int sygnCol = 0;
                    int lp = 0;
                    string faktura;


                    Tuple<int,int> colRow = sygnaturaColNo(worksheet);

                if (colRow == null)
                {
                    return null;

                }
                    foreach (DataRow row in worksheet.Rows)
                    {
                    PotwSald p;

                    if (rowNo++ < colRow.Item2) continue;
                    
                        if (row[colRow.Item1] == null ||  String.IsNullOrWhiteSpace(row[colRow.Item1].ToString())) continue; // jeśłi w trzeciej ierwszej kolumnie jest coś to sprawdzamy 
                    p = this.setupPotwSald(row);
                    if (p == null)
                        return null; 
                    p.Sygnatura = row[colRow.Item1] as string ?? "";
                    p.Pakiet_Id = pakietid;                 

                        lst.Add(p);
                    }
                    return lst;
                }
                else
                    return null;
            }

        }

    }
