using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WienaDB.Models;

namespace LexEnaTrs.Web.Zgony
{

    public class xlsxRow
    {
        public int rowNo { get; set; }
        public string sygnatura { get; set; }
        public int idSprawyWiena { get; set; }
        public int status { get; set; }
        public string statusTxt { get; set; }

    }

   


    public class ProceedZgony
    {
        private string errDescription { get; set; }
        private int errCode = 0;
        private string inXlsxDOC;
        private List<errDescription> errorsCollection = new List<Web.errDescription>();
        private string userName;
        private DateTime DataZestawienia;
        private string[] colNames =
      {
            "sygnatura"
          
        };

        List<xlsxRow> sygnLst;

        public ProceedZgony()
        {

        }

        public ProceedZgony(string xlsxDocument, string UserName, DateTime d_zestawienia)
        {
            this.inXlsxDOC = xlsxDocument;
            this.userName = UserName;
            this.DataZestawienia = d_zestawienia;
        }

        private void addError(ErrLevel lv, int code, string description, string reference = "", bool isSummary = false)
        {
            errDescription er = new errDescription();
            er.code = code;
            er.description = description;
            er.level = lv;
            er.isSummary = isSummary;
            this.errorsCollection.Add(er);
            er.reference = reference;

        }

        private int getCellNoByName(string colname, Dictionary<int, string> tab)
        {
            if (tab == null) return -1;
            int c = tab.Where(a => a.Value == colname).Select(a => a.Key).FirstOrDefault();
            if (c > 0) return c;
            else return -1;

        }

        public bool ImportFile()
        {
            this.sygnLst = null;

            if ((sygnLst = parseXLSX()) == null)
                return false;
            else
                return true;
        }

        public bool GetWienaId()
        {
            bool result = true;
            xlsxRow xr1 = null;
            try
            {
                using (wiena_centralEntities wienaContext = new wiena_centralEntities())
                {
                    foreach (xlsxRow xr in this.sygnLst)
                    {
                        xr1 = xr;
                        xr.sygnatura = xr.sygnatura.Trim();
                        sprawa spr = (from sp in wienaContext.sprawa
                                  join fr in wienaContext.firma on sp.id_firmy equals fr.ident
                                  where fr.typ_firmy == 1 && sp.sygnatura == xr.sygnatura
                                  select sp).FirstOrDefault();


                     //int id = wienaContext.sprawa.Where(a => a.sygnatura == xr.sygnatura).Select(a => a.ident).FirstOrDefault();
                        if (spr != null)
                            xr.idSprawyWiena = spr.ident;
                        else
                        {
                            xr.idSprawyWiena = 0;
                            result = false;
                            xr.status = -1;
                            xr.statusTxt = "Brak sprawy w Wienie";
                            this.addError(ErrLevel.Error, -153, "Brak sprawy w syst.Wiena sygnatura :" + xr.sygnatura);

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd odczytu z systemu Wiena : " + xr1.sygnatura+ " " + ex.Message);
                this.addError(ErrLevel.Fatal, -900, "Błąd odczytu z systemu Wiena : " + ex.Message);
                return false;
            }
            return result;
        }

        public int CreateDbStructure()
        {
            ZgonyHeader zg = null;
            try
            {
                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {
                    zg = new ZgonyHeader();
                    zg.DataImportu = DateTime.Now;
                    zg.DataZestawienia = this.DataZestawienia;
                    zg.UserName = this.userName;
                    zg.StatusText = "Nowa";
                    zg.StatusZestawienia = 0;
                    lexena.ZgonyHeader.AddObject(zg);

                    foreach (xlsxRow xr in this.sygnLst)
                    {
                        ZgonyDetails zd = new ZgonyDetails();
                        zd.Status = 0;
                        zd.Sygnatura = xr.sygnatura;
                        zd.IdSprWiena = xr.idSprawyWiena;
                        zg.ZgonyDetails.Add(zd);
                    }
                    lexena.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                Utils.LogWriter("Błąd zapisu w LexEnie : " + ex.Message);
                this.addError(ErrLevel.Fatal, -900, "Błąd zapisu w LexEnie : " + ex.Message);
                return -1;
            }
            return zg.ZgonyHeader_Id;
        }

        private List<xlsxRow> parseXLSX()
        {
            byte[] data = Convert.FromBase64String(this.inXlsxDOC);
            if (data == null)
            {
                this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                Utils.LogWriter("Błąd w trakcie odczytu zbioru - jest pusty");
                return null;

            }
            Stream stream = new MemoryStream(data);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = false;
            DataSet result = excelReader.AsDataSet();
            DataTable worksheet = result.Tables[0];

            if (worksheet.Rows.Count > 0)
            {

                List<xlsxRow> itemLst = new List<xlsxRow>();
                int i = 0;
                int IdSprawy = 0;
                Dictionary<int, string> koldict = null;// = getDataRowName(worksheet);
                IdSprawy = 0;
                bool czyblad = false;
                errDescription theError = new errDescription();
                theError.level = ErrLevel.OK;
                theError.code = 0;
                theError.reference = "wiersz " + i.ToString();
                int columnsNo = worksheet.Columns.Count;

                Regex r = new Regex(@"[A-Z][A-Za-z]{0,1}-\d{1,5}[/]\d{4}");

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


                        for (int col = 0; col < 3; col++)
                        {
                            string val = row[col] as string != null ? row[col] as string : "";
                            Match m = r.Match(val);
                            if (m.Success)
                            {
                                isinside = true;
                                sygnCol = col;
                                sygnRow = rowNo;
                                break;
                            }

                        }

                        if (!isinside)

                        {
                            if (rowNo > 20)
                            {
                                theError.description += "brak sygnatury w trzech pierwszych kolumnach i dwudziestu wierszach";
                                theError.level = ErrLevel.Error;
                                theError.code = -845;
                                theError.reference = "";
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                Utils.LogWriter("brak sygnatury w trzech pierwszych kolumnach");
                                return null;
                            }
                            else
                                continue;
                        }
                    }
                    else
                    {

                        if (sygnCol > 0)
                        {
                            Match m = r.Match(row[sygnCol] as string == null ? "" : row[sygnCol] as string);
                            if (!m.Success)
                            {

                                break; // koniec wczytywania
                            }


                        }


                    }
                    // i
                    if (isinside && koldict == null)
                    {
                        // dodanie kolumn
                        DataRow rPrev = null;
                        DataRow rPPrev = null;
                        koldict = new Dictionary<int, string>();
                        koldict.Add(sygnCol, colNames[0]);
                        if (sygnRow > 0)
                        {
                            rPrev = worksheet.Rows[sygnRow - 1];
                            if (sygnRow > 1)
                                rPPrev = worksheet.Rows[sygnRow - 2];
                        }
                        for (int cc = 0; cc < columnsNo; cc++)
                        {
                            if (cc == sygnCol) continue;
                            string colname = (rPrev != null ? rPrev[cc] as string : "");
                            if (String.IsNullOrWhiteSpace(colname) && rPPrev != null)
                                colname = (rPPrev != null ? rPPrev[cc] as string : "");

                            if (String.IsNullOrWhiteSpace(colname))
                                continue;

                            colname = colname.ToLower().Replace(" ", "");
                            foreach (string cn in colNames)
                            {
                                string c = cn.ToLower().Replace(" ", "");
                                if (c == colname)
                                {
                                    if (koldict.Where(a => a.Value == cn).Select(a => a.Key).FirstOrDefault() > 0)
                                        ;
                                    else
                                        koldict.Add(cc, cn);
                                }


                            }

                        }



                    }


                    int cellNo = 0;
                    // szukamy pierwszej 





                    xlsxRow item = new xlsxRow();
                    i++;
                    item.rowNo = i;
              
                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[0], koldict)) >= 0)
                        {
                            item.sygnatura = row[cellNo] as string;


                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu sygnatury";
                        theError.level = ErrLevel.Error;
                        theError.code = -801;
                        theError.reference = "";
                        this.addError(theError.level, theError.code, theError.description, theError.reference);

                    }


                    itemLst.Add(item);
                }
                return itemLst;
            }
            return null;
        }





    }
}