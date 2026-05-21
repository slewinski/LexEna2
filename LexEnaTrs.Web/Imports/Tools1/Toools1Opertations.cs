using Excel;
using LexEnaTrs.Web.UZD;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace LexEnaTrs.Web.Tools1
{


    public class Toools1Opertations
    {

        private const int UZD_Sygnatura_Col = 3; // dat skorzystania z ulgi

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

        public string GetFileEPU( string xlsxBase64, string filename)
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
                string ans = String.Empty;

                foreach (updateData u in lst)
                {
                    ans = ans + (String.IsNullOrEmpty(ans) ? "":";") + u.Sygnatura; 


                }

                return ans;
               
            }
            catch (Exception ex)
            {

                return "Błąd: " + ex.Message;
            }

        }


        private List<updateData> parseXLSX(string inXlsxDOC, string filename)
        {
            List<updateData> lst = new List<updateData>();

            DataTable worksheet = XLSX2Datatable(inXlsxDOC,filename);

            if (worksheet.Rows.Count > 0)
            {

                int columnsNo = worksheet.Columns.Count;
    

                bool isinside = false;
                int rowNo = 0;
                int sygnCol = 0;
                int lp = 0;
                string faktura;
                foreach (DataRow row in worksheet.Rows)
                {

                    string val = row[0].ToString();
                    rowNo++;
                    if (!int.TryParse(val, out lp)) continue; //( leśli nie liczba) 

                    if (String.IsNullOrWhiteSpace(val) && String.IsNullOrWhiteSpace(row[UZD_Sygnatura_Col].ToString()) ) break; // jeśłi w ierwszej kolumnie jest coś to sprawdzamy 
                    updateData u = new updateData();
                    u.Sygnatura= row[UZD_Sygnatura_Col] as string ?? "";
             

                    lst.Add(u);
                }
                return lst;
            }
            else
                return null;
        }

    }
}