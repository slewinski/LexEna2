using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Excel;
using System.Data;
using System.IO;

namespace LexEnaTrs.Web.Tools1
{
    public class updateData
    {
        public int Lp { get; set; }
        public DateTime DataPrzekazania { get; set;}
        public string Sygnatura { get; set; }
        public string Oddzial { get; set; }
        public string Dluznik { get; set; }
        public string NrKontrahenta { get; set; }
        

    }


    public class EkstraktPdf
    {
        private const int UZD_Sygnatura_Col = 1; // kolumn a sygnatury

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
                        DOKEKSTR_SygnLst sg = new DOKEKSTR_SygnLst();
                        sg.packId = _guid;
                        sg.Sygnatura = u.Sygnatura;
                        context.DOKEKSTR_SygnLst.AddObject(sg);

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

                    if (String.IsNullOrWhiteSpace(val) && String.IsNullOrWhiteSpace(row[UZD_Sygnatura_Col].ToString())) break; // jeśłi w ierwszej kolumnie jest coś to sprawdzamy 
                    updateData u = new updateData();
                    u.Sygnatura = row[UZD_Sygnatura_Col] as string ?? "";


                    lst.Add(u);
                }
                return lst;
            }
            else
                return null;
        }

    }

}
