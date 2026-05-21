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

namespace LexEnaTrs.Web.UZD
{

    public class updateData {

        public string NrFaktury { get; set; }
        public DateTime? DataFaktury { get; set; }
        public Int32 Status { get; set; }

        public string Nip { get; set; }

        public string SapId { get; set; }
        public DateTime? DataZlozenia { get; set; }
        public string Sygnatura {get;set;}
    }
    public class UZDOperations
    {

        private const int UZD_DataFaktury_Col = 4;
        private const int UZD_NIP_Col = 9;
        private const int UZD_DataZlozenia_Col = 22; // dat skorzystania z ulgi
        private const int UZD_Confirm_Col = 23;  // kolumna Tak nie 
        private const int UZD_NrFaktury_Col = 3;

        private const int UZD_SAP_NIP_COL = 11;
        private const int UZD_SAP_Id_Col = 2;
        


        public string InsertNewPackage(DateTime theDay, string Importer)
        {
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    SqlParameter param1 = new SqlParameter("Data", theDay);
                    SqlParameter param2 = new SqlParameter("Importer", Importer);


                    var IdPak = context.ExecuteStoreQuery<int>("spUZD_GetInitSprLst  @Data, @Importer", param1, param2).FirstOrDefault();
                    if (IdPak > 0)
                    {
                        int IdPakiet = (int)IdPak;

                        SqlParameter param0 = new SqlParameter("IdPakiet", IdPakiet);
                        object obj = context.ExecuteStoreQuery<object>("spUZD_GetExtraSprLst @IdPakiet,  @Data, @Importer", param0, param1, param2).FirstOrDefault();


                    }
                    return "";
                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
            }

        }

        public string InsertNewPaymentPackage(DateTime theDay, string Importer)
        {
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    SqlParameter param1 = new SqlParameter("Data", theDay);
                    SqlParameter param2 = new SqlParameter("Importer", Importer);


                    var IdPak = context.ExecuteStoreQuery<int>("spUZD_ComputePayment  @Data, @Importer", param1, param2).FirstOrDefault();

                    return "";
                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
            }
        }
        public string ComputeZaleglosc(int IdPakiet)
        {
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {




                    SqlParameter param0 = new SqlParameter("IdPakiet", IdPakiet);
                    SqlParameter param1 = new SqlParameter("Data", DateTime.Today);
                    object obj = context.ExecuteStoreQuery<object>("spUZD_ComputeDebt  @IdPakiet  @Data ", param0, param1).FirstOrDefault();



                    return "";
                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
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


    private List<updateData> parseXLSX(string inXlsxDOC, string filename)
        {
            List<updateData> lst = new List<updateData>();

            DataTable worksheet = XLSX2Datatable( inXlsxDOC,  filename);

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

                    if (String.IsNullOrWhiteSpace(val)) break; // jeśłi w ierwszej kolumnie jest coś to sprawdzamy 
                    updateData u = new updateData();
                    u.NrFaktury = row[UZD_NrFaktury_Col] as string ?? "";
                    u.Nip = row[UZD_NIP_Col] as string ?? "";
                    if (!String.IsNullOrWhiteSpace(row[UZD_DataFaktury_Col] as string ?? ""))
                    {
                        string d = (row[UZD_DataFaktury_Col] as string ?? "").Trim();
                        try
                        {
                            DateTime dt = DateTime.ParseExact(d, "d-M-yyyy", CultureInfo.InvariantCulture);
                            u.DataFaktury = dt;
                        }
                        catch (Exception)
                        {
                            Utils.LogWriter("Błąd parsowania daty faktury, nr faktury " + u.NrFaktury);
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(row[UZD_DataZlozenia_Col] as string ?? ""))
                    {
                        string d = (row[UZD_DataZlozenia_Col] as string ?? "").Trim();
                        try
                        {
                            DateTime dt = DateTime.ParseExact(d, "d-M-yyyy", CultureInfo.InvariantCulture);
                            u.DataZlozenia = dt;
                        }
                        catch (Exception)
                        {
                            Utils.LogWriter("Błąd parsowania daty faktury, nr faktury " + u.NrFaktury);
                        }
                    }

                    // - 9 ta kolumna t/N
                    string tn = (row[UZD_Confirm_Col] as string ?? "").Trim().ToUpper();
                    if (tn == "T" || tn == "TAK")
                        u.Status = 1;
                    else if (tn == "N" || tn == "NIE")
                        u.Status = -1;
                    else
                        u.Status = 0;

                    lst.Add(u);
                }
                return lst;
            }
            else
                return null;
        }

        private List<updateData> parseXLSXFromSAP(string inXlsxDOC, string filename)
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

                    string val = row[UZD_SAP_Id_Col].ToString();
                    rowNo++;
                    if (!int.TryParse(val, out lp)) continue; //( leśli nie liczba) 

                    if (String.IsNullOrWhiteSpace(val)) break; // jeśłi w ierwszej kolumnie jest coś to sprawdzamy 
                    updateData u = new updateData();
                    u.SapId = row[UZD_SAP_Id_Col] as string ?? "";
                    u.Nip = row[UZD_SAP_NIP_COL] as string ?? "";  
                    u.Nip = u.Nip.Replace(" ", "").Replace("-", "").Trim();
                    lst.Add(u);
                }
                return lst;
            }
            else
                return null;
        }
        public string ConfirmPackage(int IdPackage, string xlsxBase64, string filename)
        {
            try
            {
                List<updateData> lst = parseXLSX(xlsxBase64, filename);

                if (lst == null)
                    return "Błąd parsowania zbioru sprawdź poprawność jego zawartości";
                if ( !lst.Any())
                    return "Zbiór jest pusty";
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    List<UZD_Faktura> fakts = context.UZD_Faktura.Where(p => p.UZD_PakietId == IdPackage).ToList();
                    if (fakts != null)
                    {
                        foreach (UZD_Faktura f in fakts)
                        {
                            updateData u = lst.Where(p => p.Nip == f.Nip && p.NrFaktury == f.NrFaktury).FirstOrDefault();
                            if (u != null)
                            {
                                f.Status = u.Status;
                                if (f.DataFaktury == null && u.DataFaktury > new DateTime(2010, 1, 1) && u.DataFaktury < f.DataWymagalnosci)
                                    f.DataFaktury = u.DataFaktury;
                            }
                        }
                        context.SaveChanges();
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
            }

        }

        public string ConfirmPackageFinal(int IdPackage, string xlsxBase64, string filename)
        {
            bool anystat = false;
            DateTime? dzlo = null;
            try
            {
                List<updateData> lst = parseXLSX(xlsxBase64, filename);

                if (lst == null)
                    return "Błąd parsowania zbioru sprawdź poprawność jego zawartości";
                if (!lst.Any())
                    return "Zbiór jest pusty";
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    List<UZD_Faktura> fakts = context.UZD_Faktura.Where(p => p.UZD_PakietId == IdPackage).ToList();
                    if (fakts != null)
                    {
                        foreach (UZD_Faktura f in fakts)
                        {
                            updateData u = lst.Where(p => p.Nip == f.Nip && p.NrFaktury == f.NrFaktury).FirstOrDefault();
                            if (u != null)
                            {
                                if (u.Status > 0)
                                {
                                    f.Status = 10;
                                    anystat = true;
                                    if (u.DataZlozenia != null)
                                    {

                                        dzlo = u.DataZlozenia.Value;
                                    }
                                }
                                
                            }
                        }
                        if (anystat)
                        {
                         UZD_Pakiet up =    context.UZD_Pakiet.Where(p => p.UZD_PakietId == IdPackage).FirstOrDefault();
                            if (up != null) { up.Status = 10; up.DataPotwierdzenia = DateTime.Today; up.Opis = "Potwierdzono złożenie deklaracji"; up.DataPotwierdzenia = dzlo; }
                        }
                        context.SaveChanges();
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
            }

        }

        public string ConfirmSAPNips(int IdPackage, string xlsxBase64, string filename)
        {
            try
            {
                List<updateData> lst = parseXLSXFromSAP(xlsxBase64, filename);

                if (lst == null)
                    return "Błąd parsowania zbioru sprawdź poprawność jego zawartości";
                if (!lst.Any())
                    return "Zbiór jest pusty";
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    foreach (updateData u in lst)
                    {
                        List<UZD_Faktura> fakts = context.UZD_Faktura.Where(p => p.UZD_PakietId == IdPackage).ToList();
                        if (u.SapId.Trim().Length > 0 && u.Nip.Trim().Length> 0 )
                        {
                            if (context.UZD_SapMap.Where(a => a.NIP == u.Nip).FirstOrDefault() == null)
                            {
                                UZD_SapMap usm = new UZD_SapMap();
                                usm.NIP = u.Nip;
                                usm.SapId = u.SapId;
                                context.UZD_SapMap.AddObject(usm);



                            } 



                        }
                        context.SaveChanges();

                    }
                    return "";

                }
            }
            catch (Exception ex)
            {

                return "error: " + ex.Message;
            }

        }

    }
}