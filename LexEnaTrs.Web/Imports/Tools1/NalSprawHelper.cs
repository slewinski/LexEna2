using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace LexEnaTrs.Web.Tools1
{
    public class NalSprawHelper
    {
      

            private const int NalSpraw_Sygnatura_Col = 1; // kolumn a sygnatury
          

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

            public string GetFileContent(string xlsxBase64, string filename)
            {
                bool anystat = false;
                DateTime? dzlo = null;
                try
                {
                    List<updateData> lst = parseXLSX(xlsxBase64, filename);

                    if (lst == null)
                        return "Błąd parsowania zbioru sprawdź poprawność jego zawartości";
                    if (!lst.Any())
                        return "Błąd Zbiór jest pusty";
                    // przepisanie do listy 
                    Guid _guid = Guid.NewGuid();
                    using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                    {
                        foreach (updateData u in lst)
                        {
                        SkutKanc sg = new SkutKanc();
                            sg.Pakiet_Id = _guid;
                            sg.Sygnatura = u.Sygnatura;
                            sg.Lp = u.Lp;
                            sg.NrKontrahenta = u.NrKontrahenta;
                            sg.Dluznik = u.Dluznik; 
                            sg.Oddzial = u.Oddzial;
                           context.SkutKanc.AddObject(sg);

                        }
                        context.SaveChanges();
                    }
                    return _guid.ToString();

                }
                catch (Exception ex)
                {

                    return "Błąd: " + ex.Message;
                }

            }


            private List<updateData> parseXLSX(string inXlsxDOC, string filename)
            {
            List<updateData> lst = new List<updateData>();

            DataTable worksheet = XLSX2Datatable(inXlsxDOC, filename);

            if (worksheet.Rows.Count > 0)
            {

                int columnsNo = worksheet.Columns.Count;


                int rowNo = 0;
                int lp = 0;
                string faktura;
                foreach (DataRow row in worksheet.Rows)
                {

                    string val = row[0].ToString();
                    rowNo++;
                    if (!int.TryParse(val, out lp)) continue; //( leśli nie liczba) 

                    if (String.IsNullOrWhiteSpace(val) && String.IsNullOrWhiteSpace(row[NalSpraw_Sygnatura_Col].ToString())) break; // jeśłi w ierwszej kolumnie jest coś to sprawdzamy 
                    updateData u = new updateData();
                    u.Sygnatura = row[NalSpraw_Sygnatura_Col] as string ?? "";
                    u.Lp = lp;
                    u.Oddzial = row[NalSpraw_Sygnatura_Col + 1] as string ?? "";
                    u.Dluznik = row[NalSpraw_Sygnatura_Col + 2] as string ?? "";
                    u.NrKontrahenta = (row[NalSpraw_Sygnatura_Col + 3]== null ? "": row[NalSpraw_Sygnatura_Col + 3].ToString());

                    lst.Add(u);
                }
                return lst;
            }
            else
                return null;
        }

        }

    }
