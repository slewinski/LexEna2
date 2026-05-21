using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using LexEnaTrs.Web.EpuSrv;
//using Excel;
using System.Data;
using Excel;
using WienaDB.Models;
using SharpCompress.Archives;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity.Validation;

namespace LexEnaTrs.Web.Imports
{
    public class ImportValidators
    {


        private string logname = "infolog.txt";
        private Pozwy paczkapozwow { get; set; }
        private MojeNakazyOutputData paczkanakazow { get; set; }
        private Postanowienia paczkapostanowien1 { get; set; }
        private MojeOrzeczeniaOutputData paczkapostanowien2 { get; set; }
        private WnioskiEgzekucyjneEPU paczkawnioskow { get; set; }
        private MojeSprawyOutputData paczkaspraw1 { get; set; }
        private MojeSprawyEPU paczkaspraw2 { get; set; }
        public string errDescription { get; set; }
        private int errCode = 0;
        private DocType typDok = DocType.None;

        private List<errDescription> errorsCollection;


        private string importFileName;
        private string importZipFileName;
        private string importXlsxFileName;
        private byte[] zipContent;
        private string inXlsxDOC;
        private string tempZipfile;

        private int KontoEpu;
        private string inDOC;
        private int isXML = 1;
        public enum docTypes { Pozew, Nakaz, Wyrok, Klauzula, WniosekEgz, Zawezwanie, Sprzeciw, Oddalenie, Bezskuteczne, Sprostowanie, Uchylenie, Umorzenie, Zgon, ZwrotPozwu, Doreczenie, KlauzulaDoreczenia, ZgloszenieWierzytelnosci, UmorzenieEPU, OdrzucenieZgon }
        public int FirmaTyp = 1; // Domyślnie EOB


        public class contentDescriptor
        {
            public docTypes docType { get; set; }
            public string description { get; set; }

            public contentDescriptor(docTypes dT, string dsc)
            {

                docType = dT;
                description = dsc;

            }

        }

        public class xlsxItem
        {
            public int rowNo { get; set; }
            public string sygnatura { get; set; }
            public decimal ks { get; set; }
            public decimal kzp { get; set; }
            public decimal kinne { get; set; }
            public DateTime dataDokumentu { get; set; }
            public DateTime dataRejestracji { get; set; }
            public string sygnSad { get; set; }
            public decimal nalglowna { get; set; }
            public decimal odskapital { get; set; }
            public decimal wzd { get; set; }
            public decimal klauzula { get; set; }
            public decimal odsetki { get; set; }
            public string sad { get; set; }
            public string wydzial { get; set; }
            public string sadmiasto { get; set; }
            public string komornik { get; set; }
            public string sygnKM { get; set; }
            public decimal kladw { get; set; }
            public decimal doreczenia { get; set; }
            public decimal kladwdoreczenia { get; set; }
            public decimal klauzulakosztdoreczenia { get; set; }

        }


        private string[] colNames =
        {
            "sygnatura",
            "koszty sądowe",
            "koszty zastępstwa",
            "koszty inne",
            "data dokumentu",
            "data otrzymania",
            "sygnatura sądowa",
            "należność główna",
            "w tym odsetki skapitalizowane",
            "wezwania do zapłaty",
            "koszty klauzuli",
                "sąd",
                "wydział",
                "miejscowość",
                "odsetki",
                "komornik",
                "sygnatura komornicza",
                "koszty zastępstwa w postępowaniu klauzulowym",
                "koszty doręczeń komorniczych",
                "koszty klauzuli doręczeń komorniczych",
                "koszty zastępstwa w postępowaniu klauzulowym doręczeń komorniczych"
                
        };

        private contentDescriptor[] infilesDesctiptor = new contentDescriptor[] {
            new contentDescriptor(docTypes.Pozew,"Pozwy|Pozew|MZP"),
            new contentDescriptor(docTypes.Nakaz,"Nakazy|Nakaz|SNZ"),
            new contentDescriptor(docTypes.Wyrok,"Wyroki|Wyrok|SWZ"),
            new contentDescriptor(docTypes.Doreczenie,"Doreczen|Doręczen|Komornicze"),
            new contentDescriptor(docTypes.KlauzulaDoreczenia,"DoreczKomKlauzula|DoręczKomKlauzula"),
            new contentDescriptor(docTypes.Klauzula,"Klauzule|Klauzula|Tytuł|Tytuły|SPK|STW"),
            new contentDescriptor(docTypes.Zawezwanie,"Zawezwanie|Zawezwania"),
            new contentDescriptor(docTypes.Oddalenie,"Oddalenia|Oddalenie|SWO"),
            new contentDescriptor(docTypes.Bezskuteczne,"Bezskuteczna|Bezskuteczne|bek"),
            new contentDescriptor(docTypes.Sprzeciw,"Sprzeciw|Sprzeciwy"),
            new contentDescriptor(docTypes.WniosekEgz,"Wniosek|Wnioski"),
            new contentDescriptor(docTypes.Uchylenie,"Uchylenie|Uchylenia"),
            new contentDescriptor(docTypes.Sprostowanie,"Sprostownie|Sprostowania"),
            new contentDescriptor(docTypes.Umorzenie,"Umorzenie|Umorzenia"),
            new contentDescriptor(docTypes.ZwrotPozwu,"Zwrot|Zwroty"),
            new contentDescriptor(docTypes.Zgon,"Zgon|Zgony"),
            new contentDescriptor(docTypes.ZgloszenieWierzytelnosci,"Wierzyt|oszenie"),
            new contentDescriptor(docTypes.UmorzenieEPU,"EPU"),
            new contentDescriptor(docTypes.OdrzucenieZgon,"Odrzuc")



        };

        private docTypes? getDocType(string descriptor)
        {
            foreach (contentDescriptor cd in infilesDesctiptor)
            {
                List<String> lst = cd.description.Split('|').ToList();
                if (lst == null) continue;
                foreach (string s in lst)
                {
                    if (descriptor.ToLower().Contains("odrzuc"))
                         return docTypes.OdrzucenieZgon;

                    if (descriptor.ToUpper().Contains(s.ToUpper()))
                        return cd.docType;



                }

            }

            return null;

        }



        public ImportValidators()
        {
            errCode = 0;
            errDescription = "";
            errorsCollection = new List<Web.errDescription>();
            importFileName = "";
            KontoEpu = 0;
        }

        public ImportValidators(string importfilename, string fileContent, bool czyxml = true)
        {
            this.importFileName = importfilename;
            this.inDOC = fileContent;
            isXML = (czyxml ? 1 : 0);
            errCode = 0;
            errDescription = "";
            errorsCollection = new List<Web.errDescription>();
            // importFileName = "";
            KontoEpu = 0;
        }


        public ImportValidators(string importZipFilename, string zipGuid, string importXlsxFilename, string XlsxfileContent)
        {


            this.inXlsxDOC = XlsxfileContent;
            this.importZipFileName = importZipFilename;
            this.importXlsxFileName = importXlsxFilename;


            isXML = 2;
            errCode = 0;
            errDescription = "";
            errorsCollection = new List<Web.errDescription>();
            // importFileName = "";
            KontoEpu = 0;
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    Guid g = new Guid(zipGuid);
                    if (g != Guid.Empty)
                    {
                        List<DataBuffer> lst = new List<DataBuffer>();
                        lst = context.DataBuffer.Where(a => a.ident == g).OrderBy(a => a.number).ToList();


                        if (lst != null)
                        {
                            this.tempZipfile = System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/" + importZipFilename);
                            if (File.Exists(this.tempZipfile))
                                File.Delete(this.tempZipfile);

                            using (Stream w = new FileStream(this.tempZipfile, FileMode.Append | FileMode.Create))
                            {
                                int bufLen = 0;

                                foreach (DataBuffer item in lst)
                                {
                                    w.Write(item.binValue, 0, item.binValue.Length);
                                    bufLen += item.binValue.Length;
                                }

                                /*

                            int arrSize = lst.Sum(a => a.binValue.Length);
                            zipContent = new byte[arrSize];
                            int bufLen = 0;
                            foreach (DataBuffer item in lst)
                            {

                                //Array.Resize(ref zipContent, zipContent.Length + item.binValue.Length);
                                Array.Copy(item.binValue, 0, zipContent, bufLen, item.binValue.Length);
                                bufLen += item.binValue.Length;
                            }
                            */
                                w.Close();
                            }


                        }

                    }
                }
            }

            catch (Exception ex)
            {
                Utils.LogWriter("Błąd podczas importu zbioru zip " + importZipFilename + " " + ex.Message);

            }
        }


        public string GetOperationStatusInfo()
        {
            return this.errDescription;

        }


        enum DocType
        {
            None,
            Pozew,
            Sprawa1,
            Sprawa2,
            Nakaz,
            Wniosek,
            Klauzula1,
            Klauzula2,
            Postanowienie1,
            Postanowienie2,
            KosztyEgz,
            CzynnosciKom,
            Wyrok,
            Zawezwanie,
            Oddalenie,
            Bezskutecza,
            Sprzeciw,
            Uchylenie,
            Sprostowanie,
            Umorzenie,
            Zgon,
            Empty,
            ZwrotPozwu,
            Doreczenie,
            DoreczenieKlauzula,
            ZgloszenieWierzytelnosci,
            UmorzenieEPU, 
            OdrzucenieZgon


        };


        private int docType2Int(DocType doc)
        {
            switch (doc)
            {
                case DocType.None:
                    return 0;
                case DocType.Pozew:
                    return 10;
                case DocType.Sprawa1:
                case DocType.Sprawa2:
                    return 2;
                case DocType.Nakaz:
                    return 5;
                case DocType.Wniosek:   // egzekucyjny
                    return 30;

                case DocType.Klauzula1:
                case DocType.Klauzula2:
                    return 17;
                case DocType.Postanowienie1:
                case DocType.Postanowienie2:
                    return 3;
                case DocType.Empty:
                    return 0;
                case DocType.KosztyEgz:
                    return 10001;
                case DocType.CzynnosciKom:
                    return 10002;
                case DocType.Wyrok:
                    return 105005;
                case DocType.Sprzeciw:
                    return 100013;
                case DocType.Oddalenie:
                    return 105009;
                case DocType.Zawezwanie:
                    return 100011;
                case DocType.Sprostowanie:
                    return 100012;
                case DocType.Uchylenie:
                    return 100013;
                case DocType.Umorzenie:
                    return 100014;
                case DocType.ZwrotPozwu:
                    return 100018;
                case DocType.Bezskutecza:
                    return 100004;
                case DocType.Zgon:
                    return 100016;
                case DocType.Doreczenie:
                    return 100019;
                case DocType.DoreczenieKlauzula:
                    return 100020;
                case DocType.UmorzenieEPU:
                    return 100021;
                case DocType.ZgloszenieWierzytelnosci:
                    return 100022;
                case DocType.OdrzucenieZgon:
                    return 100023;
                default:
                    return -100;
            }




        }

        private object XmlDeserializeFromString(string objectData, Type type, string defnamespace)
        {
            object result;
            XmlSerializer serializer;

            try
            {
                if (defnamespace != null)
                {

                    serializer = new XmlSerializer(type, defnamespace);
                }
                else
                    serializer = new XmlSerializer(type);





                using (TextReader reader = new StringReader(objectData))
                {
                    result = serializer.Deserialize(reader);



                }

                return result;
            }
            catch (Exception ex)
            {

                Utils.LogWriter("Błąd parsowania zbioru " + ex.Message);
                return null;
            }

        }





        /*
          public static string SerializeToString(object objToSerialize, Type type)
        {
            var output = new MemoryStream();
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true};
            string outputString;
            using (var xmlWriter = XmlWriter.Create(output, settings))
            {
                var serializer = new XmlSerializer(type);
                xmlWriter.WriteStartDocument();
                serializer.Serialize(xmlWriter, objToSerialize);
            }
            output.Seek(0L, SeekOrigin.Begin);
            // zamina stream na string
            var reader = new StreamReader(output);
            outputString = reader.ReadToEnd();
            return outputString;

        }
        */
        private string SerializeToString(object objToSerialize, Type type, bool czyCurr)
        {
            var output = new MemoryStream();
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
            string outputString;
            using (var xmlWriter = XmlWriter.Create(output, settings))
            {
                var serializer = new XmlSerializer(type);


                xmlWriter.WriteStartDocument();
                if (czyCurr)
                {
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("curr", "http://www.e-sad.gov.pl/epu");
                    serializer.Serialize(xmlWriter, objToSerialize, namespaces);
                }
                else
                    serializer.Serialize(xmlWriter, objToSerialize);

            }
            output.Seek(0L, SeekOrigin.Begin);
            // zamina stream na string
            var reader = new StreamReader(output);
            outputString = reader.ReadToEnd();
            return outputString;

        }

        private string clearnamespaces(string strin)
        {
            string s;

            s = strin.Replace("<a.", "<");
            s = s.Replace("</a.", "</");
            s = s.Replace("<a:", "<");
            s = s.Replace("</a:", "</");

            return s;
        }


        private string correctDecimals(String xml)
        {

            Regex r = new Regex("\"" + @"\d{1,3}[^0-9\.]\d{1,3}\.\d\d\" + "\"");
            MatchCollection m = r.Matches(xml);
            if (m.Count > 0)
            {
                foreach (Match ma in m)
                {
                    string mVal = ma.Value;
                    mVal = Regex.Replace(mVal, "[^0-9\\.\\\"]", "");
                    xml = xml.Replace(ma.Value, mVal);
                }

                //              sygn = m.Value.Replace("posługiwanie się w korespondencji nr sygnatury ", "");
                //            SygnWgPowoda = sygn + "@0";
                //          sygnOK = true;
            }
            return xml;
        }
        /*
        if (tagName.Length > 0)
        {
            Regex regex = new Regex("\""+ @"\d{1,3}[^0-9\.]\d{1,3}\.\d\d\" + "\"" );

        }
        else
        {


        //    return regex.Replace(xml, string.Empty);
        }

    }
*/



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

        private bool columnExists(System.Data.DataColumnCollection cols, string colname)
        {
            foreach (System.Data.DataColumn c in cols)
            {
                if (c.ColumnName.ToUpper().Trim() == colname.ToUpper().Trim())
                    return true;


            }
            return false;
        }

        private DocType recognizeDocType(string thedata)
        {
            DocType dt = DocType.None;


            if (this.isXML == 1)
            {
                try
                {
                    paczkapozwow = (Pozwy)XmlDeserializeFromString(thedata, typeof(Pozwy), null);
                    if (paczkapozwow == null) throw new Exception();

                    return dt = DocType.Pozew;
                }
                catch (Exception e0)
                {

                    try
                    {
                        string s = clearnamespaces(thedata);
                        paczkaspraw1 = (MojeSprawyOutputData)XmlDeserializeFromString(s, typeof(MojeSprawyOutputData), null);
                        if (paczkaspraw1 == null) throw new Exception();
                        return dt = DocType.Sprawa1;
                    }
                    catch (Exception e)
                    {

                        try
                        {
                            paczkaspraw2 = (MojeSprawyEPU)XmlDeserializeFromString(thedata, typeof(MojeSprawyEPU), null);
                            if (paczkaspraw2 == null) throw new Exception();
                            return dt = DocType.Sprawa2;

                        }
                        catch (Exception e1)
                        {
                            try
                            {
                                string s = clearnamespaces(thedata);
                                paczkanakazow = (MojeNakazyOutputData)XmlDeserializeFromString(s, typeof(MojeNakazyOutputData), null);
                                if (paczkanakazow == null) throw new Exception();
                                return dt = DocType.Nakaz;
                            }
                            catch (Exception ex12)
                            {
                                try
                                {
                                    string s = clearnamespaces(thedata);
                                    paczkapostanowien2 = (MojeOrzeczeniaOutputData)XmlDeserializeFromString(s, typeof(MojeOrzeczeniaOutputData), null);
                                    if (paczkapostanowien2 != null && paczkapostanowien2.listaOrzeczen.Any())
                                    {
                                        string xml = paczkapostanowien2.listaOrzeczen.FirstOrDefault().dokumentXml;
                                        if (xml.ToLower().Contains("koddecyzji=\"17\"") || xml.ToLower().Contains("koddecyzji =\"17\"") || xml.ToLower().Contains("koddecyzji= \"17\"") || xml.ToLower().Contains("koddecyzji = \"17\""))
                                            return dt = DocType.Klauzula2;
                                        else
                                            return dt = DocType.Postanowienie2;
                                    }
                                    else
                                        throw new Exception();


                                }
                                catch (Exception e2)
                                {
                                    try
                                    {
                                        string s = clearnamespaces(thedata);
                                        paczkapostanowien1 = (Postanowienia)XmlDeserializeFromString(s, typeof(Postanowienia), null);
                                        if (paczkapostanowien1 != null && paczkapostanowien1.OrzeczenieEPU.Any())
                                        {
                                            if (paczkapostanowien1.OrzeczenieEPU.FirstOrDefault().kodDecyzji == 17)
                                                return dt = DocType.Klauzula1;
                                            else
                                                return dt = DocType.Postanowienie1;
                                        }
                                        else
                                            throw new Exception();

                                    }
                                    catch (Exception e3)
                                    {
                                        try
                                        {
                                            paczkawnioskow = (WnioskiEgzekucyjneEPU)XmlDeserializeFromString(thedata, typeof(WnioskiEgzekucyjneEPU), null);
                                            return dt = DocType.Wniosek;
                                        }
                                        catch (Exception e4)
                                        {

                                            return dt = DocType.None;
                                        }

                                    }

                                }
                            }
                        }
                    }
                }
            }
            else   // to może być Excel

            {  // dokument w base64 
                try
                {


                    byte[] data = Convert.FromBase64String(thedata);
                    if (data == null) return dt = DocType.None;
                    Stream stream = new MemoryStream(data);

                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result = excelReader.AsDataSet();
                    DataTable worksheet = result.Tables[0];
                    if (worksheet.Rows.Count > 0)
                    {
                        if (worksheet.Rows[0] != null)
                        {
                            if (worksheet.Columns.Count >= 8 && columnExists(worksheet.Columns, "Komornik") && columnExists(worksheet.Columns, "Odsetki"))
                                return dt = DocType.KosztyEgz;
                            else
                              if (worksheet.Columns.Count >= 7 && columnExists(worksheet.Columns, "Data otrzymania") && columnExists(worksheet.Columns, "Nazwa dokumentu"))
                                return dt = DocType.CzynnosciKom;


                        }


                    }



                    return dt = DocType.None;
                }
                catch (Exception ex)
                {

                    return dt = DocType.None;

                }


            }
        }

        public List<errDescription> GetErrorsCollection()
        {
            return this.errorsCollection;

        }

        public string GetErrorsCollectionAsString()
        {
            string val = string.Empty;
            if (this.errorsCollection != null)
            {
                foreach (var item in this.errorsCollection)
                {
                    val += "[" + item.code + "] " + item.description + "\n\r";

                }


            }

            return val;
        }

        public string UpdateImportStatusInfo(string filename, int User_Id, int Jednostka_id, int imptyp)
        {
            errDescription err;
            Import imp;

            try
            {// zapis 
                using (LexEnaMeritumEntities theContext = new LexEnaMeritumEntities())
                {
                    imp = new Import();
                    imp.ImpExp = imptyp; // import danych
                    imp.JednostkaWindykacji_Id = Jednostka_id;
                    imp.NazwaZbioru = filename.Truncate(255);
                    imp.DataTransferu = DateTime.Now;
                    imp.ContentType = docType2Int(typDok);

                    Uzytkownik usr = theContext.Uzytkownik.Where(u => u.Id == User_Id).FirstOrDefault();
                    if (usr != null)
                        imp.UserName = usr.Nazwisko + " " + usr.Imie;
                    imp.StatusOperacji = (int)ErrLevel.OK; // zakładamy, ze OK 
                    err = this.errorsCollection.Where(a => a.isSummary == true).FirstOrDefault();
                    if (err != null)
                    {

                        imp.OpisOperacji = (err.description + (err.code != 0 ? "(" + err.code.ToString() + ")" : "")).Truncate(255);
                        imp.StatusOperacji = (int)err.level;
                        imp.FileType = 1; // xml

                    }

                    List<errDescription> lstdet = errorsCollection.Where(e => e.isSummary == false).ToList();
                    foreach (errDescription er in lstdet)
                    {
                        ImportDetail idet = new ImportDetail();
                        idet.Code = er.code;
                        idet.ErrLevel = (int)er.level;

                        idet.Sygnatura = er.reference.Truncate(200);
                        idet.ErrDescription = (er.description).Truncate(255);
                        idet.DataImportu = DateTime.Now;
                        if (idet.ErrLevel > imp.StatusOperacji)
                        {
                            imp.StatusOperacji = (int)idet.ErrLevel;
                            imp.OpisOperacji = (idet.ErrDescription + (er.code != 0 ? "(" + er.code.ToString() + ")" : "")).Truncate(255);
                            imp.FileType = 1; // x

                        }
                        idet.Import = imp;
                    }
                    theContext.Import.AddObject(imp);
                    theContext.SaveChanges();
                    return "";
                }



            }
            catch (Exception ex)
            {
                string blad = "";
                Utils.LogWriter(blad = "Błąd podczas zapisu statusu operacji importu " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : ""));
                return blad;
            }
        }
        private Dictionary<string, byte[]> ValidateTradDoc()
        {
            if (String.IsNullOrWhiteSpace(this.tempZipfile))
            {
                this.errDescription = "Wejściowy zbiór .zip jest pusty";
                return null;
            }
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            try
            {


                /*
                 *Stream stream = new MemoryStream(Convert.FromBase64String(this.inZipDOC));
                using (ZipFile z = ZipFile.Read(stream))
                {
                    foreach (ZipEntry zEntry in z)
                    {
                        MemoryStream tempS = new MemoryStream();
                        zEntry.Extract(tempS);

                        files.Add(zEntry.FileName, tempS.ToArray());
                    }


                }
                */


                var reader = ArchiveFactory.Open(this.tempZipfile);
                foreach (var entry in reader.Entries)
                {


                    if (!entry.IsDirectory)
                    {
                        MemoryStream tempS = new MemoryStream();
                        entry.WriteTo(tempS);
                        files.Add(entry.Key, tempS.ToArray());

                    }

                }
                reader.Dispose();

                if (File.Exists(this.tempZipfile))
                    File.Delete(this.tempZipfile);
                return files;
            }
            catch (Exception ex)
            {
                errDescription = ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message : "");
                return null;
            }
        }
        private string sygnFromDocName(string docName)
        {
            string[] patterns = { @"[A-Z][A-Za-z]{0,7}-\d{1,5}[-_/]\d{4}" }; //{ @"^[A-Z][A-Za-z]{0,1}-\d{1,5}[-_]\d{4}" };
            string sygn;

            foreach (string s in patterns)
            {


                Regex r = new Regex(s);
                Match m = r.Match(docName);
                if (m.Success)
                {
                    sygn = m.Value;
                    char[] sng = sygn.ToCharArray();
                    sng[sygn.Length - 5] = '/';
                    sygn = new string(sng);
                    sygn = sygn.Replace("_", "-");
                    return sygn;
                }
            }
            return null;

        }

        /*
        private int getSadId(string nazwa, string wydzial)
        {


        }
        */
        /* obsługa dokumentów - skanów */
        Dictionary<int, string> getDataRowName(DataTable dt)
        {
            Dictionary<int, string> dtc = new Dictionary<int, string>();
            int i = -1;
            foreach (DataColumn col in dt.Columns)
            {
                ++i;
                string cn = col.ColumnName.ToLower().Replace(" ", "");
                foreach (string colname in colNames)
                {
                    string c = colname.ToLower().Replace(" ", "");
                    if (cn == c)
                    {
                        dtc.Add(i, colname);
                    }


                }



            }

            return dtc;
        }

        private int getCellNoByName(string colname, Dictionary<int, string> tab)
        {
            if (tab == null) return -1;
            int c = tab.Where(a => a.Value == colname).Select(a => a.Key).FirstOrDefault();
            if (c > 0) return c;
            else return -1;

        }

        private List<xlsxItem> parseXLSX()
        {
            byte[] data = Convert.FromBase64String(this.inXlsxDOC);
            if (data == null)
            {
                this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                Utils.LogWriter("Błąd w trakcie odczytu zbioru " + importFileName + " jest pusty");
                return null;

            }
            Stream stream = new MemoryStream(data);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = false;
            DataSet result = excelReader.AsDataSet();
            DataTable worksheet = result.Tables[0];

            if (worksheet.Rows.Count > 0)
            {

                List<xlsxItem> itemLst = new List<xlsxItem>();
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

                Regex r = new Regex(@"[A-Z][A-Za-z]{0,6}-\d{1,5}[/]\d{4}");

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


                        for (int col = 0; col < 4; col++)
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





                    xlsxItem item = new xlsxItem();
                    i++;
                    item.rowNo = i;
                    item.klauzula = 0;
                    item.kladw = 0;
                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[0], koldict)) >= 0)
                        {
                            item.sygnatura = (row[cellNo] as string).ToUpper();


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


                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[1], koldict)) >= 0)
                        {
                            string s = row[cellNo].ToString();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.ks = Convert.ToDecimal(row[cellNo].ToString());
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu kosztów sądowych";
                        theError.level = ErrLevel.Error;
                        theError.code = -802;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);


                    }

                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[2], koldict)) >= 0)
                        {
                            string s = row[cellNo].ToString();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.kzp = Convert.ToDecimal(row[cellNo].ToString());

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu kosztów zastępstwa";
                        theError.level = ErrLevel.Error;
                        theError.code = -802;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }
                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[3], koldict)) >= 0)
                        {
                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.kinne = Convert.ToDecimal(s);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu kosztów innych";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }


                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[4], koldict)) >= 0)
                        {
                            string s = row[cellNo].ToString();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.dataDokumentu = Convert.ToDateTime(row[cellNo].ToString());

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu daty dokumentu";
                        theError.level = ErrLevel.Error;
                        theError.code = -804;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }

                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[5], koldict)) >= 0)
                        {
                            string s = row[cellNo].ToString();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.dataRejestracji = Convert.ToDateTime(row[cellNo].ToString());
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu daty otrzymania dokumentu";
                        theError.level = ErrLevel.Error;
                        theError.code = -805;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }


                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[6], koldict)) >= 0)
                        {
                            item.sygnSad = row[cellNo] as string;


                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu sygnatury sądowej";
                        theError.level = ErrLevel.Error;
                        theError.code = -806;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }

                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[7], koldict)) >= 0)
                        {
                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.nalglowna = Convert.ToDecimal(s);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu należnosci głownej";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }

                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[8], koldict)) >= 0)
                        {
                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.odskapital = Convert.ToDecimal(s);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu odsetek skpitalizowanych";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }

                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[9], koldict)) >= 0)
                        {
                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.wzd = Convert.ToDecimal(s);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu opłat za wezwania do zapłaty ";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }
                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[9], koldict)) >= 0)
                        {

                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.wzd = Convert.ToDecimal(s);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu opłat za wezwania do zapłaty ";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }


                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[10], koldict)) >= 0)
                        {
                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.klauzula += Convert.ToDecimal(s);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu kosztów klauzuli ";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }

                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[11], koldict)) >= 0)
                        {
                            item.sad = row[cellNo] as string;


                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu sądu ";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }

                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[12], koldict)) >= 0)
                        {
                            item.wydzial = row[cellNo] as string;


                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu wydziału ";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }
                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[13], koldict)) >= 0)
                        {
                            item.sadmiasto = row[cellNo] as string;


                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu siedziby sądu ";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }



                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[14], koldict)) >= 0)
                        {
                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.odsetki = Convert.ToDecimal(s);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu odsetek ";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }


                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[15], koldict)) >= 0)
                        {
                            item.komornik = row[cellNo] as string;


                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu komornika ";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }

                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[16], koldict)) >= 0)
                        {
                            item.sygnKM = row[cellNo] as string;


                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu sygn KM ";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }
                    item.sygnatura = item.sygnatura.Trim();
                    itemLst.Add(item);

                    //koszty doręczeń
                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[17], koldict)) >= 0)
                        {

                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.kladw += Convert.ToDecimal(s);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu koszty zastępstwa w postępowaniu klauzulowym ";
                        theError.level = ErrLevel.Error;
                        theError.code = -803;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }

                    

                    //"koszty doręczeń komorniczych",
                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[18], koldict)) >= 0)
                        {

                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.doreczenia += Convert.ToDecimal(s);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu kosztów doręczeń komorniczych ";
                        theError.level = ErrLevel.Error;
                        theError.code = -804;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }
                //    "koszty klauzuli doręczeń komorniczych",
                
                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[19], koldict)) >= 0)
                        {

                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.klauzulakosztdoreczenia += Convert.ToDecimal(s);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu kosztów klauzuli doręczeń komorniczych ";
                        theError.level = ErrLevel.Error;
                        theError.code = -804;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }


                    // "koszty zastępstwa w postępowaniu klauzulowym doręczeń komorniczych"
                    try
                    {
                        if ((cellNo = getCellNoByName(colNames[20], koldict)) >= 0)
                        {

                            string s = row[cellNo].ToString().ToLower().Replace("zł", "").Trim();
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                item.kladwdoreczenia += Convert.ToDecimal(s);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        theError.description += "błąd odczytu kosztów zast klauzuli adw doręczeń komorniczych ";
                        theError.level = ErrLevel.Error;
                        theError.code = -804;
                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }


                }
                return itemLst;
            }
            return null;
        }
        // szukamy sprawy



        private static bool IsHoliday(DateTime Day)
        {
            if (Day.DayOfWeek == DayOfWeek.Saturday) return true;
            if (Day.DayOfWeek == DayOfWeek.Sunday) return true;
            if (Day.Month == 1 && Day.Day == 1) return true; // Nowy Rok
            if (Day.Month == 5 && Day.Day == 1) return true; // 1 maja
            if (Day.Month == 5 && Day.Day == 3) return true; // 3 maja
            if (Day.Month == 8 && Day.Day == 15) return true; // Wniebowzięcie Najświętszej Marii Panny, Święto Wojska Polskiego
            if (Day.Month == 11 && Day.Day == 1) return true; // Dzień Wszystkich Świętych
            if (Day.Month == 11 && Day.Day == 11) return true; // Dzień Niepodległości 
            if (Day.Month == 12 && Day.Day == 25) return true; // Boże Narodzenie
            if (Day.Month == 12 && Day.Day == 26) return true; // Boże Narodzenie
            int a = Day.Year % 19;
            int b = Day.Year % 4;
            int c = Day.Year % 7;
            int d = (a * 19 + 24) % 30;
            int e = (2 * b + 4 * c + 6 * d + 5) % 7;
            if (d == 29 && e == 6) d -= 7;
            if (d == 28 && e == 6 && a > 10) d -= 7;
            DateTime Easter = new DateTime(Day.Year, 3, 22).AddDays(d + e);
            if (Day.AddDays(-1) == Easter)
                return true; // Wielkanoc (poniedziałek)
            if (Day.AddDays(-60) == Easter)
                return true; // Boże Ciało
            return false;
        }



        private DateTime DataZleceniaEOP(DateTime day)
        {
            int month = day.Month;
            DateTime lastDay = new DateTime(day.Year, day.Month, DateTime.DaysInMonth(day.Year, day.Month));
            int counter = 0;
            do
            {
                if (!IsHoliday(lastDay))
                    counter++;

                lastDay = lastDay.AddDays(-1);
            }
            while (counter < 2);

            if (day <= lastDay)
                return day;
            else
            {
                lastDay = (new DateTime(day.Year, day.Month, DateTime.DaysInMonth(day.Year, day.Month))).AddDays(1);
                while (IsHoliday(lastDay))
                {

                    lastDay = lastDay.AddDays(1);
                }
                return lastDay;
            }

        }




        public bool addDocScan(docTypes dt, byte[] scan, string docName, DateTime dataDokumentu, DateTime dataOtrzymania, Decimal WPS, Decimal KosztySądowe, Decimal KZP, Decimal KosztyInne, int id_sad, string sygnSad, decimal nalglowna, decimal odskapital, decimal wdz, decimal kklauzuli, string sad, string wydzial, string miastosad, decimal odsetki, string komornik, string sygnKM, decimal kladw, int mode = 0, bool czyEpu = false, decimal odsSkapitalizowanePozew = 0)
        {
            string Sygnatura;
            Sprawa spr = null;
            // norm,alizacja daty dokumentu 
            bool iserror = false;
            NazwaStatusu nazstat;
            NazwaStatusu oststat;
            DokOdebr dood = null;
            DokWys dowys = null;
            bool docfound = false;
            bool czyodebr = false;
            bool newDok = false;
            int dok_id = 0;
            int wiena_id = 0;
            int typdokLexEna = 0;
            int idSprawy = 0;
            int id_dok_odebr_typ = 0;
            int id_dok_odebr_wiena = 0;
            int id_status_wiena = 0;
            int idKonto = 0;
            int idSchemat = 0;  // schemat księgowy
            string dokKsie = "";
            bool pws_new = false;
            if (scan == null || dataDokumentu == null || dataDokumentu < new DateTime(2010, 1, 1))
                return false;

            DateTime dStatusu = DateTime.Today;
            try
            {
                dataDokumentu = new DateTime(dataDokumentu.Year, dataDokumentu.Month, dataDokumentu.Day);
                if (dataOtrzymania.Year < 2000)
                    dataOtrzymania = DateTime.Today;
                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {
                    using (wiena_centralEntities wiena = new wiena_centralEntities())
                    {



                        PdfStore pdf = null;
                        spr_sadowa ss = null;
                        errDescription theError = new errDescription();

                        theError.level = ErrLevel.OK;
                        theError.code = 0;
                        theError.description = "";
                        theError.reference = docName;

                        spr = null;
                        Utils.LogWriter(" Start Importu dokumentu  " + docName);
                        Sygnatura = sygnFromDocName(docName);
                        if (String.IsNullOrWhiteSpace(Sygnatura))
                        {
                            Utils.LogWriter("Błędne oznaczenie sygnatury " + docName);
                            this.addError(ErrLevel.Error, -701, "Błędne oznaczenie sygnatury sprawy w nazwie dokumentu " + docName, docName);
                            return false;

                        }

                        // dodanie encji pozew
                        spr = (from z in lexena.Sprawa
                               where z.sygnatura == Sygnatura
                               select z).OrderByDescending(z => z.id).FirstOrDefault();

                        if (spr == null && dt != docTypes.ZgloszenieWierzytelnosci)
                        {
                            Utils.LogWriter("Brak sprawy w systemie LexEna " + docName);
                            this.addError(ErrLevel.Error, -702, "Brak sprawy w systemie LexEna " + Sygnatura, docName);
                            return false;

                        }
                        /*
                                            roDokOdebr.Add(new rodzajDokumentu("Nakazy \"zwykłe\"", 100005));
                                            roDokOdebr.Add(new rodzajDokumentu("Wyroki \"zwykłe\"", 105005));
                                            roDokOdebr.Add(new rodzajDokumentu("Klauzule \"zwykłe\"", 100017));
                                            roDokOdebr.Add(new rodzajDokumentu("Sprzeciwy", 100013));
                                            roDokOdebr.Add(new rodzajDokumentu("Dokumenty komornicze \"zwykłe\"", 10002));

                        roDok.Add(new rodzajDokumentu("Pozwy \"zwykłe\"", 100010));
                                                roDok.Add(new rodzajDokumentu("Wnioski egz. \"zwykłe\"", 100030));
                                                roDok.Add(new rodzajDokumentu("Inne Dokumenty \"zwykłe\"", 100003));
                        */
                        int krok = 0;
                        idSprawy = spr!=null? spr.id : 0 ;
                        switch (dt)
                        {
                            case docTypes.ZgloszenieWierzytelnosci:  // tylko  konta 2442170

                                krok = 10;
                                czyodebr = false;
                                typdokLexEna = docType2Int(DocType.ZgloszenieWierzytelnosci);
                                id_status_wiena = 55;
                                dStatusu = dataDokumentu;
                                idKonto = (mode == -1 ? 6 : 15);
                                dokKsie = "Uz6";
                                idSchemat = (mode == -1 ? 126: 43 );
                                break;
                            case docTypes.UmorzenieEPU:  // tylko  konta 2442170

                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.UmorzenieEPU);
                                id_status_wiena = 123;
                                id_dok_odebr_typ = 1395; /* Postanowienie o umorzeniu postępowania*/
                                id_dok_odebr_wiena = 15601;
                                dStatusu = dataDokumentu;
                                idKonto = 0;
                                dokKsie = "";
                                idSchemat = 0;
                                break;
                            case docTypes.Pozew:
                                krok = 3;
                                czyodebr = false;
                                typdokLexEna = (mode == -1 ? 10 : 100010);
                                id_status_wiena = 1;
                                dStatusu = dataDokumentu;
                                idKonto = (mode == -1 ? 4 : 12);
                                dokKsie = "Uz3";
                                idSchemat = (mode == -1 ? 28 : 13);
                                Utils.LogWriter(" Rejestracja pozwu  ");
                                break;
                            case docTypes.Zawezwanie:
                                krok = 3;
                                czyodebr = false;
                                typdokLexEna = docType2Int(DocType.Zawezwanie);
                                id_status_wiena = 1;
                                dStatusu = dataDokumentu;
                                idKonto = 12;
                                dokKsie = "Uz3";
                                idSchemat = 13;
                                break;

                            case docTypes.Nakaz:
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = (mode == -1 ? 5 : 100005);
                                id_dok_odebr_typ = 24;
                                id_dok_odebr_wiena = 14457;
                                id_status_wiena = 41;
                                dStatusu = dataOtrzymania;
                                break;

                            case docTypes.Wyrok:
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = 100005;
                                id_dok_odebr_typ = 64; /* oddalenie powództwa 686*/
                                id_dok_odebr_wiena = 14461;
                                id_status_wiena = 41;
                                dStatusu = dataOtrzymania;
                                break;
                            case docTypes.Oddalenie:
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.Oddalenie);
                                id_dok_odebr_typ = 898; /* oddalenie powództwa 686*/
                                id_dok_odebr_wiena = 14461;
                                id_status_wiena = 41;
                                dStatusu = dataOtrzymania;
                                break;
                            case docTypes.Umorzenie:
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.Umorzenie);
                                id_dok_odebr_typ = 129; /* Postanowienie o umorzeniu postępowania*/
                                id_dok_odebr_wiena = 14936;
                                id_status_wiena = 3;
                                dStatusu = dataOtrzymania;
                                break;
                            case docTypes.ZwrotPozwu:
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.ZwrotPozwu);
                                id_dok_odebr_typ = 129; /* Postanowienie o odrzuceniu pozwu*/
                                id_dok_odebr_wiena = 14931;
                                id_status_wiena = 63;
                                dStatusu = dataOtrzymania;
                                break;

                            case docTypes.Zgon:
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.Zgon);
                                id_dok_odebr_typ = 3075; /* zgon dłużnika */
                                id_dok_odebr_wiena = 14848;
                                id_status_wiena = 119;
                                dStatusu = dataOtrzymania;
                                break;

                            case docTypes.OdrzucenieZgon:
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.Zgon);
                                id_dok_odebr_typ = 1707; /* zgon dłużnika */
                                id_dok_odebr_wiena = 15688;
                                id_status_wiena = 126;
                                dStatusu = dataOtrzymania;
                                break;

                            case docTypes.Uchylenie:
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.Uchylenie);
                                id_dok_odebr_typ = 18; /* oddalenie powództwa 686*/
                                id_dok_odebr_wiena = 14445;
                                id_status_wiena = 41;
                                dStatusu = dataOtrzymania;
                                break;
                            case docTypes.Sprostowanie:
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.Sprostowanie);
                                id_dok_odebr_typ = 33; /* oddalenie powództwa 686*/
                                id_dok_odebr_wiena = 14448;
                                id_status_wiena = 41;
                                dStatusu = dataOtrzymania;
                                break;


                            case docTypes.Bezskuteczne:
                                krok = 10;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.Bezskutecza);
                                id_dok_odebr_typ = 55; /* oddalenie powództwa 686*/
                                id_dok_odebr_wiena = 14640;
                                id_status_wiena = 30;
                                dStatusu = dataOtrzymania;
                                break;
                            case docTypes.Sprzeciw:
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.Sprzeciw);
                                id_dok_odebr_typ = 139; /* oddalenie powództwa 686*/
                                id_dok_odebr_wiena = 14618;
                                id_status_wiena = 41;
                                dStatusu = dataOtrzymania;
                                break;
                            case docTypes.Klauzula:
                                krok = 5;
                                czyodebr = true;
                                typdokLexEna = (mode == -1 ? 17 : 100017);
                                id_dok_odebr_typ = 25; /* oddalenie powództwa 686*/
                                id_dok_odebr_wiena = 14458;
                                id_status_wiena = 23;
                                dStatusu = dataOtrzymania;
                                idKonto = (mode == -1 ? 11 : 13);
                                dokKsie = "Uz4";
                                idSchemat = (mode == -1 ? 29 : 17);
                                break;
                            case docTypes.WniosekEgz:
                                krok = 10;
                                czyodebr = false;
                                typdokLexEna = 100030;
                                /* oddalenie powództwa 686*/
                                id_dok_odebr_wiena = 14549;
                                id_status_wiena = 2;
                                dStatusu = dataDokumentu;
                                idKonto = (mode == -1 ? 3 : 14);
                                dokKsie = "Uz5";
                                idSchemat = (mode == -1 ? 30 : 19);
                                break;
                            case docTypes.Doreczenie:  ///  doręczenie komornicze
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.Doreczenie);
                                id_dok_odebr_typ =  3235; // testy : 3124; //
                                id_dok_odebr_wiena = 15608;
                                id_status_wiena = 41;
                                dokKsie = "Uz4";
                                dStatusu = dataOtrzymania;
                                break;
                            case docTypes.KlauzulaDoreczenia:  ///  doręczenie komoicze
                                krok = 4;
                                czyodebr = true;
                                typdokLexEna = docType2Int(DocType.DoreczenieKlauzula);
                                id_dok_odebr_typ = 3239; /* oddalenie powództwa 686*/
                                id_dok_odebr_wiena = 15609;
                                id_status_wiena = 41;
                                dokKsie = "Uz4";
                                dStatusu = dataOtrzymania;
                                break;

                            default:
                                break;
                        }
                        nazstat = (from x in lexena.NazwaStatusu
                                   where x.Krok == krok
                                   select x).FirstOrDefault();

                        oststat = (from x in lexena.NazwaStatusu
                                   join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                   where y.Sprawa_id == idSprawy
                                   orderby x.Krok descending
                                   select x).FirstOrDefault();

                        Utils.LogWriter("Ustalenie kroku  ");
                        if (oststat == null || nazstat == null)
                        {
                            Utils.LogWriter("Ustalenie kroku  - brak statusu LexEna");
                        }
                        else
                        if (oststat.Krok > nazstat.Krok)
                        {


                            Utils.LogWriter("Sprawa ma już wyższy status " + oststat.Krok.ToString() + " " + Sygnatura);
                            this.addError(ErrLevel.Warning, 703, "Sprawa ma już wyższy status " + oststat.Krok.ToString() + " dokument  " + docName, Sygnatura);

                        }
                        Utils.LogWriter("Wyszukanie w Wienie  ");
                        wiena_id = (from x in wiena.sprawa join f in wiena.firma on x.id_firmy equals f.ident where x.sygnatura == Sygnatura && f.typ_firmy == FirmaTyp select x.ident).FirstOrDefault();
                        if (wiena_id == 0)
                        {
                            iserror = true;
                            this.addError(ErrLevel.Error, -704, "Brak sprawy w systemie Wiena " + Sygnatura + " w systemie Energii", Sygnatura);
                            this.errDescription = "Brak sprawy w systemie Wiena " + Sygnatura + " w systemie Energii";
                            lexena.Sprawa.Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, spr);
                            return false;
                        }
                        if (spr != null)
                            idSprawy = spr.id;
                        else
                        {
                            idSprawy = 0;
                            
                        }
                        Utils.LogWriter(" Znaleziono w Wiena  Id = " + idSprawy.ToString());
                        switch (dt)
                        {
                            case docTypes.Pozew:
                                dowys = (from z in lexena.DokWys
                                         where z.Sprawa_id == idSprawy && z.TypDok == typdokLexEna && z.DataDok == dataDokumentu
                                         select z).FirstOrDefault();

                                break;
                            case docTypes.Nakaz:
                                dood = (from z in lexena.DokOdebr
                                        where z.Sprawa_id == idSprawy && z.TypDok == typdokLexEna && z.DataDokumentu == dataDokumentu
                                        select z).FirstOrDefault();
                                break;

                            case docTypes.Klauzula:
                                dood = (from z in lexena.DokOdebr
                                        where z.Sprawa_id == idSprawy && z.TypDok == typdokLexEna && z.DataDokumentu == dataDokumentu
                                        select z).FirstOrDefault();
                                break;

                            case docTypes.Umorzenie:
                                dood = (from z in lexena.DokOdebr
                                        where z.Sprawa_id == idSprawy && z.TypDok == typdokLexEna && z.DataDokumentu == dataDokumentu
                                        select z).FirstOrDefault();
                                break;
                            case docTypes.UmorzenieEPU:
                                dood = (from z in lexena.DokOdebr
                                        where z.Sprawa_id == idSprawy && z.TypDok == typdokLexEna && z.DataDokumentu == dataDokumentu
                                        select z).FirstOrDefault();
                                break;
                            case docTypes.ZwrotPozwu:
                            case docTypes.ZgloszenieWierzytelnosci:
                                dood = (from z in lexena.DokOdebr
                                        where z.Sprawa_id == idSprawy && z.TypDok == typdokLexEna && z.DataDokumentu == dataDokumentu
                                        select z).FirstOrDefault();
                                break;

                        }





                        if (czyodebr)
                        {
                            if (dood == null)
                            {
                                dood = new DokOdebr();
                                newDok = true;
                                dood.d_modyfikacji = DateTime.Now;
                                dok_id = 0;
                                dood.CzyEPU = 0;
                                dood.IdEPU = 0;
                                dood.Tresc = "";
                                dood.d_kreacji = DateTime.Today;

                                //   dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                dok_id = dood.Id;
                                newDok = false;
                                dood.d_modyfikacji = DateTime.Now;
                            }

                            dood.DataDokumentu = dataDokumentu;
                            dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                            // nakaz zapłaty
                            dood.Sprawa_id = idSprawy;
                            dood.PartitionKey = 0;
                            dood.Format = 100;
                            dood.StatusDok = "wydano";
                            // dood.TrescHtml = XML2HTMLTransform.TransformNCompress(dood.Tresc, 5);
                            dood.CzyZalatw = 0;
                            dood.TypDok = typdokLexEna;
                            switch (dt)
                            {
                                case docTypes.Nakaz:
                                    dood.Nazwa = "Nakaz";

                                    break;
                                case docTypes.Wyrok:
                                    dood.Nazwa = "Wyrok";
                                    break;
                                case docTypes.Klauzula:
                                    dood.Nazwa = "Tytuł wykonawczy";
                                    break;
                                case docTypes.Bezskuteczne:
                                    dood.Nazwa = "Postanowienie o bezskut. egzek.";
                                    break;
                                case docTypes.Oddalenie:
                                    dood.Nazwa = "Wyrok oddalający";
                                    break;
                                case docTypes.Umorzenie:
                                    dood.Nazwa = "Postanowienie o umorzeniu postępowania";
                                    break;
                                case docTypes.UmorzenieEPU:
                                    dood.Nazwa = "Postanowienie o umorzeniu postępowania w epu";
                                    break;
                                case docTypes.ZwrotPozwu:
                                    dood.Nazwa = "Postanowienie o odrzuceniu pozwu";
                                    break;
                                case docTypes.OdrzucenieZgon:
                                    dood.Nazwa = "Postanowienie o odrzuceniu pozwu-zgon";
                                    break;
                                case docTypes.Zgon:
                                    dood.Nazwa = "Dokument dot. zgonu";
                                    break;
                                case docTypes.Sprzeciw:
                                    dood.Nazwa = "Sprzeciw";
                                    break;
                                case docTypes.Uchylenie:
                                    dood.Nazwa = "Postanowienie o uchyleniu nakazu";
                                    break;
                                case docTypes.Sprostowanie:
                                    dood.Nazwa = "Postanowienie o sprostowaniu orzeczenia";
                                
                                    break;

                            }


                        }
                        else //dokument wysłany
                        {

                            if (dowys == null)
                            {
                                dowys = new DokWys();
                                newDok = true;
                                Utils.LogWriter("Tworzę nowy dokument dla " + Sygnatura);
                            }
                            else
                            {
                                dok_id = dowys.Id;
                            }
                            dowys.d_kreacji = DateTime.Now;
                            dowys.DataDok = dataDokumentu;

                            dowys.DataOdbioru = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                            dowys.InneKoszty = KosztyInne;
                            dowys.Koszty = KosztySądowe;
                            dowys.Kzp = KZP;
                            dowys.WPS = WPS;
                            dowys.Sprawa_id = idSprawy;
                            dowys.RodzajDok = typdokLexEna;
                            dowys.TypDok = typdokLexEna;

                            switch (dt)
                            {
                                case docTypes.Pozew:
                                    dowys.Nazwa = "Pozew";
                                    dowys.Opis = "Pozew";
                                    dowys.StatusDok = 3;
                                    break;
                                case docTypes.WniosekEgz:
                                    dowys.Nazwa = "Wniosek Egzekucyjny";
                                    dowys.Opis = "Wniosek Egzekucyjny";
                                    break;
                                case docTypes.Zawezwanie:
                                    dowys.Nazwa = "Zawezwanie do próby ugodowej";
                                    dowys.Opis = "Zawezwanie do próby ugodowej";
                                    break;
                                case docTypes.ZgloszenieWierzytelnosci:
                                    dowys.Nazwa = "Zgłoszenie wierzytelności";
                                    dowys.Opis = "Zgłoszenie wierzytelności";
                                    break;
                            }


                        }
                        if (spr == null)
                            goto doWieny;
                        pdf = null;
                        if (czyodebr)
                        {
                            if (!newDok && dok_id > 0)
                            {
                                pdf = (from z in lexena.PdfStore
                                       where z.DokOdebr_Id == dok_id
                                       select z).FirstOrDefault();
                            }

                            if (pdf == null)
                            {

                                pdf = new PdfStore();
                                if (dood.PdfStore == null)
                                    dood.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                                dood.PdfStore.Add(pdf);

                            }
                            pdf.name = dood.Nazwa + " " + Sygnatura;
                        }
                        else
                        {
                            if (!newDok && dok_id > 0)
                            {
                                pdf = (from z in lexena.PdfStore
                                       where z.DokWys_Id == dok_id
                                       select z).FirstOrDefault();
                            }

                            if (pdf == null)
                            {

                                pdf = new PdfStore();
                                if (dowys.PdfStore == null)
                                    dowys.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                                dowys.PdfStore.Add(pdf);

                            }

                            pdf.name = dowys.Nazwa + " " + Sygnatura;
                        }

                        pdf.value = scan;

                        // dodanie statusu do sprawy 
                        spr = (from z in lexena.Sprawa
                               where z.id == idSprawy
                               select z).FirstOrDefault();



                        if (spr != null && oststat.Krok != nazstat.Krok)
                        {

                            StatusSprawy stspr = new StatusSprawy();
                            stspr.czyus = 0;
                            stspr.CzyWiena = 0;
                            stspr.DataStatusu = DateTime.Now;
                            stspr.Sprawa_id = spr.id;
                            stspr.NazwaStatusu_Id = nazstat.Id;
                            lexena.AddToStatusSprawy(stspr);
                        }
                        else
                        {
                            Utils.LogWriter("Sprawa ma już taki status " + oststat.Krok.ToString() + " " + Sygnatura);

                        }
                        if (czyodebr)
                        {
                            if (newDok)
                                lexena.DokOdebr.AddObject(dood);
                        }
                        else
                        {
                            if (newDok)
                                lexena.DokWys.AddObject(dowys);

                        }
                        // do wieny
      
                        Utils.LogWriter(" Dodano skan do LexEna ");
doWieny:
                        {
                            pws_new = false;

                            sad sadspr = null;
                            if (dt == docTypes.Pozew ||
                                dt == docTypes.Nakaz ||
                                dt == docTypes.Wyrok ||
                                dt == docTypes.Klauzula ||
                                dt == docTypes.Umorzenie ||
                                
                                dt == docTypes.ZwrotPozwu
                                )
                            {
                                ss = wiena.spr_sadowa.Where(a => a.id_sprawa == wiena_id && a.aktywne == 1 && a.czyus == 0).OrderByDescending(a => a.ident).FirstOrDefault();
                                //     Utils.LogWriter("Nakaz 1111");
                                if (ss == null)
                                {
                                    ss = new spr_sadowa();
                                    ss.aktywne = 1;
                                    ss.id_sprawa = wiena_id;
                                    ss.id_sad = 0;
                                    ss.d_kreacji = DateTime.Now;
                                    ss.czyus = 0;
                                    pws_new = true;
                                    //       Utils.LogWriter("Nakaz 2222");
                                }
                                else
                                {
                                    ss.d_modyfikacji = DateTime.Now;
                                    sadspr = wiena.sad.Where(a => a.ident == ss.id_sad).FirstOrDefault();
                                    //    Utils.LogWriter("Nakaz3333");
                                }

                                if (!string.IsNullOrWhiteSpace(sad) && !string.IsNullOrWhiteSpace(wydzial))
                                {
                                    sad mysad = wiena.sad.Where(a => a.nazwa == sad && a.wydzial == wydzial).FirstOrDefault();
                                    if (mysad == null)
                                    {
                                        mysad = new WienaDB.Models.sad();
                                        mysad.nazwa = sad;
                                        mysad.wydzial = wydzial;
                                        mysad.miasto = miastosad;
                                        mysad.rejokr = (sad.ToLower().Contains("okręgowy") ? 1 : 0);
                                        mysad.typ = (short)(wydzial.ToLower().Contains("cywiln") ? 1 : 2);
                                        mysad.sort = 100;
                                        mysad.d_kreacji = DateTime.Now;
                                        mysad.czyus = 0;
                                        wiena.sad.Add(mysad);
                                        wiena.SaveChanges();
                                        //  Utils.LogWriter("Nakaz 444");
                                    }
                                    if (ss.id_sad > 0 && ss.id_sad == 999 && ss.id_sad != mysad.ident)
                                    {
                                        ss.aktywne = 0;
                                        ss = new spr_sadowa();
                                        ss.aktywne = 1;
                                        ss.id_sprawa = wiena_id;
                                        ss.id_sad = mysad.ident;
                                        ss.d_kreacji = DateTime.Now;
                                        ss.czyus = 0;
                                        pws_new = true;
                                        // Utils.LogWriter("Nakaz 2222");

                                    }
                                    else
                                        ss.id_sad = mysad.ident;
                                    if (sadspr == null && mysad != null)
                                        sadspr = mysad;

                                }


                                if (pws_new)
                                {
                                    wiena.spr_sadowa.Add(ss);
                                    Utils.LogWriter("Dodanie sprawa sądowa w Wienie");
                                }
                            }



                            dok_odebr dok_odebrany = null;
                            pisma_wys pws = null;

                            pws_new = false;
                            if (czyodebr)
                            {
                                dok_odebrany = wiena.dok_odebr.Where(a => a.id_sprawy == wiena_id && a.d_dok == dood.DataDokumentu && a.id_dok_typ == id_dok_odebr_typ && a.czyus == 0).FirstOrDefault();
                                if (dok_odebrany == null)
                                {
                                    dok_odebrany = new dok_odebr();
                                    pws_new = true;
                                }
                                dok_odebrany.data_r = dood.DataRejestracji.Value;
                                dok_odebrany.id_sprawy = wiena_id;
                                dok_odebrany.rok = (short)dood.DataDokumentu.Value.Year;
                                dok_odebrany.id_dok_typ = id_dok_odebr_typ;
                                dok_odebrany.id_dok = id_dok_odebr_wiena;
                                dok_odebrany.czyzalatw = 0;
                                if (pws_new)
                                    dok_odebrany.d_kreacji = DateTime.Now;
                                else
                                    dok_odebrany.d_modyfikacji = DateTime.Now;
                                dok_odebrany.czyus = 0;
                                dok_odebrany.d_dok = dood.DataDokumentu;


                                if (pws_new)
                                {
                                    wiena.dok_odebr.Add(dok_odebrany);
                                }
                            }
                            else
                            {
                                Utils.LogWriter(" Dodawanie dokumentu do Wieny");
                                pws_new = false;
                                pws = wiena.pisma_wys.Where(a => a.id_sprawy == wiena_id && a.data_r == dataDokumentu && a.nazwa_pisma == dowys.Nazwa && a.czyus == 0).FirstOrDefault();
                                if (pws == null)
                                {
                                    pws = new pisma_wys();
                                    pws_new = true;
                                }
                                pws.adresat = 2;
                                pws.czyus = 0;
                                pws.data_r = dowys.DataDok;
                                pws.d_kreacji = DateTime.Now;
                                pws.id_sprawy = wiena_id;
                                if (sadspr != null && dt == docTypes.Pozew)
                                {
                                    pws.nazwa2 = sadspr.nazwa.Truncate(40);
                                    pws.nazwa3 = sadspr.wydzial.Truncate(40);

                                }

                                pws.nazwa_pisma = dowys.Nazwa;
                                pws.odpis = 0;
                                pws.stan = 1;
                                pws.sygnatura = Sygnatura;
                                if (dt == docTypes.Pozew || dt == docTypes.Zawezwanie)
                                    pws.sygn_psm = "P_0001";
                                else if (dt == docTypes.ZgloszenieWierzytelnosci)
                                    pws.sygn_psm = "P_0033";
                                else
                                    pws.sygn_psm = "P_0003"; // wniosek egzekucyjny
                                pws.termin = 0;
                                pws.typ_dok_tresc = 1;
                                pws.tresc_bin = scan;
                                if (pws_new)
                                    wiena.pisma_wys.Add(pws);




                            }
                            if (dt == docTypes.Pozew)
                            {
                                // przestawienie  dat początku okresu odsetkowego dla odsetek skapitalizowanych i WZD  
                                Utils.LogWriter("Wyszukanie wdz i odsetek skapitalizowanych ");
                                List<naleznosc> nalx3 = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && (a.id_typ_nal == 22 || a.id_typ_nal == 43)).ToList();

                                if (nalx3 != null)
                                {
                                    foreach (naleznosc nale in nalx3)
                                    {
                                        List<ods_sprawy> ods1 = wiena.ods_sprawy.Where(a => a.id_nal == nale.ident).ToList();

                                        if (ods1 != null && ods1.Any())
                                        {
                                            foreach (ods_sprawy oo in ods1)
                                            {
                                                if ((wdz > 0 && nale.id_typ_nal == 43) || (odskapital > 0 && nale.id_typ_nal == 22))
                                                    oo.data_p = dataDokumentu;
                                                    
                                            }

                                        }
                                        else // dodanie odsetek dla uzd i not 
                                        {
                                            if (nale.id_typ_nal == 22)
                                            {
                                                nale.czy_proc = true;
                                                ods_sprawy ospr = new ods_sprawy();
                                                ospr.data_p = dataDokumentu;
                                                ospr.id_nal = nale.ident;
                                                ospr.id_typ_ods = 8;
                                                ospr.kod = "a";
                                                wiena.ods_sprawy.Add(ospr);
                                            }
                                            
                                        }
                                    }


                                }
                            }
                            Utils.LogWriter("Zapis do bazy Wiena");
                            wiena.SaveChanges();
                            if (czyodebr)
                            {

                                pws_new = false;
                                skan sk = null;
                                sk = wiena.skan.Where(a => a.id_dok_odebr == dok_odebrany.ident).FirstOrDefault();

                                if (pdf != null)
                                {
                                    pws_new = true;
                                    sk = new skan();
                                    sk.d_kreacji = DateTime.Now;
                                }
                                else
                                    sk.d_modyfikacji = DateTime.Now;
                                sk.id_dok_odebr = dok_odebrany.ident;
                                sk.typ = ".PDF";
                                sk.kolejnosc = 1;
                                sk.skan1 = scan;
                                if (pws_new)
                                {
                                    wiena.skan.Add(sk);
                                }

                                pws_new = false;
                            }
                            
                            if (dt == docTypes.Pozew && mode == -1)  // operator - dodane odsetek skapitalizowanych 
                            { // dodanie 
                                if (odsSkapitalizowanePozew > 0)
                                {
                                    int ods_typ = 8;   // na stałe odsetki ustawowe za opóżnienie
                                    // jakie odsetki 
                                    List<naleznosc> nalLst = wiena.naleznosc.Include("ods_sprawy").Where(a => a.id_sprawy == wiena_id).ToList();
                                    foreach (naleznosc n in nalLst)
                                    {
                                        if (n.ods_sprawy != null && n.ods_sprawy.Any())
                                        {
                                            ods_sprawy o = n.ods_sprawy.FirstOrDefault();
                                            if (o != null)
                                            {
                                                //ods_typ = o.id_typ_ods.Value;
                                                o.data_p = dataDokumentu;

                                            }

                                        }


                                    }
                                    pws_new = false;
                                    DateTime dtx = dataDokumentu.AddDays(-1);
                                    naleznosc nalx = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 22 && a.data_n == dtx && a.tytul != null && (a.tytul??string.Empty).ToLower().Contains("kapitaliz")).FirstOrDefault();
                                    if (nalx == null)
                                    {
                                        pws_new = true;
                                        nalx = new naleznosc();
                                        nalx.d_kreacji = DateTime.Now;
                                        nalx.id_sprawy = wiena_id;
                                    }
                                    else
                                        nalx.d_modyfikacji = DateTime.Now;
                                    nalx.czy_proc = false;
                                    nalx.id_typ_nal = 22;
                                    nalx.data_n = dtx;
                                    nalx.tytul = "kapitalizacja odsetek w pozwie";
                                    nalx.kwota = odsSkapitalizowanePozew;
                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx);
                                    }
                                    wiena.SaveChanges();//1111111111
                                    if (ods_typ > 0)
                                    {
                                        nalx.czy_proc = true;
                                        ods_sprawy oo = new ods_sprawy();
                                        oo.data_p = dataDokumentu;
                                        oo.id_typ_ods = ods_typ;
                                        List<ods_sprawy> olst = new List<ods_sprawy>();
                                        oo.kod = "a";
                                        if (nalx.ident > 0 )
                                            oo.id_nal = nalx.ident;
                                        wiena.ods_sprawy.Add(oo);
                                        wiena.SaveChanges();//1111111111
                                    
                                    }
                                   
                                    // zamiana odsetek w pozostałych należmościach


                                }
                                
                                if (KosztyInne > 0)
                                {
                                    naleznosc nalx = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 47 && a.data_n == dataDokumentu).FirstOrDefault();
                                    if (nalx == null)
                                    {
                                        pws_new = true;
                                        nalx = new naleznosc();
                                        nalx.d_kreacji = DateTime.Now;
                                        nalx.id_sprawy = wiena_id;
                                    }
                                    else
                                        nalx.d_modyfikacji = DateTime.Now;
                                    nalx.czy_proc = false;
                                    nalx.id_typ_nal = 47;
                                    nalx.data_n = dataDokumentu;
                                    nalx.tytul = "40 Euro";
                                    nalx.kwota = KosztyInne;
                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx);
                                        KosztyInne = 0;
                                    }
                                }
                                
                            }


                            pws_new = false;
                            if (dt == docTypes.Nakaz || dt == docTypes.Wyrok)
                            {
                                // czy są koszty sądowe w takiej wysokości
                                if (KosztyInne + KosztySądowe > 0)
                                {
                                    naleznosc nalx = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 8).FirstOrDefault();
                                    if (nalx == null)
                                    {
                                        pws_new = true;
                                        nalx = new naleznosc();
                                        nalx.d_kreacji = DateTime.Now;
                                        nalx.id_sprawy = wiena_id;
                                    }
                                    else if (Math.Abs(nalx.data_n.Value.Subtract(dood.DataDokumentu.Value).Days) > 180)
                                    {
                                        pws_new = true;
                                        nalx = new naleznosc();
                                        nalx.d_kreacji = DateTime.Now;
                                        nalx.id_sprawy = wiena_id;

                                    }
                                    else
                                        nalx.d_modyfikacji = DateTime.Now;

                                    nalx.czy_proc = false;
                                    nalx.id_typ_nal = 8;
                                    nalx.data_n = dood.DataDokumentu;
                                    if (mode != -1)
                                        nalx.kwota = KosztyInne + KosztySądowe;
                                    else
                                        nalx.kwota = KosztySądowe;
                                    nalx.vat = 0;
                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx);
                                    }
                                }
                                // koszty zast
                                if (KZP > 0)
                                {
                                    pws_new = false;
                                    // czy są koszty sądowe w takiej wysokości
                                    naleznosc nalx1 = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 12).FirstOrDefault();
                                    if (nalx1 == null)
                                    {
                                        pws_new = true;
                                        nalx1 = new naleznosc();
                                        nalx1.d_kreacji = DateTime.Now;
                                        nalx1.id_sprawy = wiena_id;
                                    }
                                    else if (Math.Abs(nalx1.data_n.Value.Subtract(dood.DataDokumentu.Value).Days) > 180)
                                    {
                                        pws_new = true;
                                        nalx1 = new naleznosc();
                                        nalx1.d_kreacji = DateTime.Now;
                                        nalx1.id_sprawy = wiena_id;

                                    }
                                    else
                                        nalx1.d_modyfikacji = DateTime.Now;
                                    nalx1.d_modyfikacji = DateTime.Now;
                                    nalx1.czy_proc = false;
                                    nalx1.id_typ_nal = 12;
                                    nalx1.data_n = dood.DataDokumentu;
                                    nalx1.kwota = KZP;
                                    nalx1.vat = 0;
                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx1);
                                    }
                                }

                                if (kklauzuli > 0)
                                {
                                    pws_new = false;
                                    // czy są koszty sądowe w takiej wysokości
                                    naleznosc nalx2 = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 9).FirstOrDefault();
                                    if (nalx2 == null)
                                    {
                                        pws_new = true;
                                        nalx2 = new naleznosc();
                                        nalx2.d_kreacji = DateTime.Now;
                                        nalx2.id_sprawy = wiena_id;
                                    }
                                    else if (Math.Abs(nalx2.data_n.Value.Subtract(dood.DataDokumentu.Value).Days) > 180)
                                    {
                                        pws_new = true;
                                        nalx2 = new naleznosc();
                                        nalx2.d_kreacji = DateTime.Now;
                                        nalx2.id_sprawy = wiena_id;

                                    }
                                    else
                                        nalx2.d_modyfikacji = DateTime.Now;

                                    nalx2.czy_proc = false;
                                    nalx2.id_typ_nal = 9;
                                    nalx2.data_n = dood.DataDokumentu;
                                    nalx2.kwota = kklauzuli;
                                    nalx2.vat = 0;

                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx2);
                                    }
                                }
                                if (kladw > 0)
                                {
                                    pws_new = false;
                                    // czy są koszty sądowe w takiej wysokości
                                    naleznosc nalx3 = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 13).FirstOrDefault();
                                    if (nalx3 == null)
                                    {
                                        pws_new = true;
                                        nalx3 = new naleznosc();
                                        nalx3.d_kreacji = DateTime.Now;
                                        nalx3.id_sprawy = wiena_id;
                                        nalx3.id_typ_nal = 13;
                                    }
                                    else if (Math.Abs(nalx3.data_n.Value.Subtract(dood.DataDokumentu.Value).Days) > 180)
                                    {
                                        pws_new = true;
                                        nalx3 = new naleznosc();
                                        nalx3.d_kreacji = DateTime.Now;
                                        nalx3.id_sprawy = wiena_id;

                                    }
                                    else
                                        nalx3.d_modyfikacji = DateTime.Now;
                                    nalx3.czy_proc = false;
                                    nalx3.data_n = dood.DataDokumentu;
                                    nalx3.kwota = kladw;

                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx3);
                                    }
                                }
                            }
                            else if (dt == docTypes.Doreczenie || dt == docTypes.KlauzulaDoreczenia)
                            {

                                if (KosztySądowe > 0)   // koszty doręczenia
                                {
                                    bool jestDorecz = false;
                                    naleznosc nalx = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 20 && a.kwota== KosztySądowe).OrderByDescending(a=>a.data_n).FirstOrDefault();
                                    if (nalx != null && Math.Abs((nalx.data_n.Value - dood.DataDokumentu.Value).Days) < 50)
                                        jestDorecz = true;
                                    if (!jestDorecz)
                                    {
                                        pws_new = true;
                                        nalx = new naleznosc();
                                        nalx.d_kreacji = DateTime.Now;
                                        nalx.id_sprawy = wiena_id;
                                    }
                                    else
                                        nalx.d_modyfikacji = DateTime.Now;
                                    nalx.czy_proc = false;
                                    nalx.id_typ_nal = 20;
                                    nalx.data_n = dood.DataDokumentu;
                                    if (mode != -1)
                                        nalx.kwota = KosztySądowe;
                                    else
                                        nalx.kwota = KosztySądowe;
                                    nalx.vat = 0;
                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx);
                                    }
                                }
                                // koszty zast
                                if (kklauzuli > 0)
                                {
                                    pws_new = false;
                                    // czy są koszty sądowe w takiej wysokości
                                    naleznosc nalx1 = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 9 && a.data_n == dood.DataDokumentu).FirstOrDefault();
                                    if (nalx1 == null)
                                    {
                                        pws_new = true;
                                        nalx1 = new naleznosc();
                                        nalx1.d_kreacji = DateTime.Now;
                                        nalx1.id_sprawy = wiena_id;
                                    }
                                    else
                                        nalx1.d_modyfikacji = DateTime.Now;
                                    nalx1.czy_proc = false;
                                    nalx1.id_typ_nal = 9;
                                    nalx1.data_n = dood.DataDokumentu;
                                    nalx1.kwota = kklauzuli;
                                    nalx1.vat = 0;

                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx1);
                                    }
                                }
                                if (kladw > 0)
                                {
                                    pws_new = false;
                                    // czy są koszty sądowe w takiej wysokości
                                    naleznosc nalx1 = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 13 && a.data_n == dood.DataDokumentu).FirstOrDefault();
                                    if (nalx1 == null)
                                    {
                                        pws_new = true;
                                        nalx1 = new naleznosc();
                                        nalx1.d_kreacji = DateTime.Now;
                                        nalx1.id_sprawy = wiena_id;
                                        nalx1.id_typ_nal = 13;
                                    }
                                    else
                                        nalx1.d_modyfikacji = DateTime.Now;
                                    nalx1.czy_proc = false;
                                    nalx1.data_n = dood.DataDokumentu;
                                    nalx1.kwota = kladw;

                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx1);
                                    }
                                }

                            }
                            else if (dt == docTypes.Sprzeciw || dt == docTypes.Oddalenie || dt == docTypes.Uchylenie || dt == docTypes.UmorzenieEPU ) //|| dt == docTypes.OdrzucenieZgon)
                            {
                                //usunięcie kosztów
                                var nalLst = (from nal in wiena.naleznosc
                                              join tn in wiena.typ_nal on nal.id_typ_nal equals tn.ident
                                              where nal.id_sprawy == wiena_id && tn.typ_sum == 3
                                              select nal).ToList();
                                // nleżności , ktore nie są spłacane
                                if (nalLst != null)
                                {
                                    foreach (naleznosc n in nalLst)
                                    {
                                        if (n.wplata_podz != null && !n.wplata_podz.Any())
                                        {
                                            List<nal_stan> ns = wiena.nal_stan.Where(a => a.id_nal == n.ident).ToList();
                                            if (ns != null && ns.Any())
                                            {
                                                foreach (nal_stan nt in ns)
                                                {
                                                    wiena.nal_stan.Remove(nt);
                                                }

                                            }

                                            wiena.naleznosc.Remove(n);
                                        }

                                    }
                                    wiena.SaveChanges();

                                }

                            }
                            if (dt == docTypes.Klauzula)
                            {
                                if (kklauzuli > 0)
                                {
                                    pws_new = false;
                                    // czy są koszty sądowe w takiej wysokości
                                    naleznosc nalx1 = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 9 && (a.data_n == dood.DataDokumentu)).FirstOrDefault();
                                    if (nalx1 == null)
                                    {
                                        pws_new = true;
                                        nalx1 = new naleznosc();
                                        nalx1.d_kreacji = DateTime.Now;
                                        nalx1.id_sprawy = wiena_id;
                                    }
                                    else
                                        nalx1.d_modyfikacji = DateTime.Now;
                                    nalx1.czy_proc = false;
                                    nalx1.id_typ_nal = 9;
                                    nalx1.data_n = dood.DataDokumentu;
                                    nalx1.kwota = kklauzuli;

                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx1);
                                    }
                                }
                                if (kladw > 0)
                                {
                                    pws_new = false;
                                    // czy są koszty sądowe w takiej wysokości
                                    naleznosc nalx1 = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 13 && (a.data_n == dood.DataDokumentu)).FirstOrDefault();
                                    if (nalx1 == null)
                                    {
                                        pws_new = true;
                                        nalx1 = new naleznosc();
                                        nalx1.d_kreacji = DateTime.Now;
                                        nalx1.id_sprawy = wiena_id;
                                    }
                                    else
                                        nalx1.d_modyfikacji = DateTime.Now;
                                    nalx1.czy_proc = false;
                                    nalx1.id_typ_nal = 13;
                                    nalx1.data_n = dood.DataDokumentu;
                                    nalx1.kwota = kladw;

                                    if (pws_new)
                                    {
                                        wiena.naleznosc.Add(nalx1);
                                    }
                                }

                            }



                            // uzupełnienie 
                            if (ss != null)
                            {
                                switch (dt)
                                {
                                    case docTypes.Pozew:
                                        ss.data_pozwu = dataDokumentu;
                                        if (!String.IsNullOrWhiteSpace(sygnSad))
                                        {
                                            ss.sygn_nakazowe = sygnSad;

                                        }
                                        break;
                                    case docTypes.Nakaz:
                                        ss.data_nakazu = dataDokumentu;
                                        ss.data_otrz_nakaz = dataOtrzymania;
                                        if (!String.IsNullOrWhiteSpace(sygnSad))
                                        {
                                            ss.sygn_nakazowe = sygnSad;

                                        }

                                        break;
                                    case docTypes.Wyrok:
                                        ss.data_wyroku = dataDokumentu;
                                        if (!String.IsNullOrWhiteSpace(sygnSad))
                                        {
                                            ss.sygn_proces = sygnSad;

                                        }
                                        break;
                                    case docTypes.ZwrotPozwu:
                                    case docTypes.Umorzenie:
                                        if (!String.IsNullOrWhiteSpace(sygnSad))
                                        {
                                            ss.sygn_proces = sygnSad;

                                        }
                                        break;
                                    case docTypes.Klauzula:

                                        ss.data_wezw_braki = dataOtrzymania;
                                        ss.data_klauzuli = dataDokumentu;
                                        if (!String.IsNullOrWhiteSpace(sygnSad))
                                        {
                                            ss.sygn_proces = sygnSad;

                                        }
                                        break;
                                    default:
                                        break;
                                }

                            }




                            pws_new = false;
                            status_spr sspr = wiena.status_spr.Where(a => a.id_sprawy == wiena_id && a.id_statusu == id_status_wiena && a.czyus == 0 && a.od_dnia == dStatusu).FirstOrDefault();
                            status_spr ost_sspr = wiena.status_spr.Where(a => a.id_sprawy == wiena_id && a.czyus == 0).OrderByDescending(a=>a.od_dnia).ThenByDescending(a=>a.ident).FirstOrDefault();

                            if (sspr == null && (ost_sspr == null || (ost_sspr!= null && ost_sspr.id_statusu!= 9 && ost_sspr.id_statusu != 11 && ost_sspr.id_statusu != 108)))
                            {
                                DateTime dd1 = DateTime.Today.AddDays(-80);
                                sspr = wiena.status_spr.Where(a => a.id_sprawy == wiena_id && a.id_statusu == id_status_wiena && a.czyus == 0 && a.od_dnia > dd1).FirstOrDefault();
                                if (sspr == null)
                                {
                                    pws_new = true;
                                    sspr = new status_spr();
                                    sspr.d_kreacji = DateTime.Now;
                                    sspr.id_sprawy = wiena_id;
                                    sspr.id_statusu = id_status_wiena;
                                    sspr.od_dnia = dStatusu;
                                    sspr.czyus = 0;
                                }
                            }
                            else
                            {
                                ;
                            }
                            if (pws_new)
                            {
                                wiena.status_spr.Add(sspr);


                            }
                            if (czyodebr)
                                dood.DDoWieny = DateTime.Now;

                        }
                        wiena.SaveChanges(); // 11111111111111

                        // wystawianie zleceń księgowych jeśłi konto analityczcne jest > 0 
                        if ((dt == docTypes.Pozew || dt == docTypes.Zawezwanie) && idKonto > 0)
                        {

                            Utils.LogWriter("Dodanie zlecenia księgowego");
                            DateTime dzlecenia = new DateTime(2005, 12, 31);
                            zlecenia_ksiegowe zlc = null;
                            bool zlc_new = false;
                            List<zlecenia_ksiegowe> lstzlc = wiena.zlecenia_ksiegowe.Where(a => a.id_sprawy == wiena_id && a.konto == idKonto && a.czyus == 0 && a.nr_dowodu == dokKsie).OrderBy(a => a.ident).ToList();
                            if (lstzlc != null)
                            {
                                foreach (zlecenia_ksiegowe z in lstzlc)
                                {
                                    if (z.d_ksiegowania == null || z.d_ksiegowania < dzlecenia)
                                    {
                                        zlc = z;
                                        break;
                                    }
                                }

                            }


                            if (zlc == null && (lstzlc == null || !lstzlc.Any()))
                            {
                                zlc = new zlecenia_ksiegowe();
                                zlc_new = true;

                            }
                            if (zlc != null)
                            {
                                if (odsSkapitalizowanePozew > 0 && mode == -1)
                                {
                                    nalglowna -= odsSkapitalizowanePozew;
                                   
                                }
                                zlc.czyus = 0;
                                zlc.d_kreacji = DateTime.Now;
                                zlc.kod = 1;
                                zlc.konto = idKonto;
                                zlc.winien = nalglowna;  // do potwierdzenia
                                zlc.ma = 0;
                                zlc.energia = nalglowna;
                                zlc.koszty = 0;
                                zlc.odsetki = 0;
                                zlc.inne = 0;


                                decimal? vat_pocz = (from x in wiena.naleznosc where x.id_sprawy == wiena_id && x.vat != null && x.vat > 0 select x.vat).Sum();
                                decimal? vat_kon = (from x in wiena.naleznosc
                                                    join wp in wiena.wplata_podz on x.ident equals wp.id_nal
                                                    join w in wiena.wplata on wp.id_wplaty equals w.ident
                                                    where x.id_sprawy == wiena_id && x.vat > 0 && w.data_w < dataDokumentu
                                                    select wp.spl_vat).Sum();
                                if (vat_kon == null)
                                    vat_kon = 0;
                                if (vat_pocz == null)
                                    vat_pocz = 0;
                                Utils.LogWriter("Vat ustalony");
                                zlc.vat = vat_pocz.Value - vat_kon.Value;
                                zlc.nr_dowodu = dokKsie;
                                zlc.rodzaj = 0;
                                zlc.d_zlecenia = (mode == -1 ? DataZleceniaEOP(dataOtrzymania) : dataDokumentu);
                                zlc.id_sprawy = wiena_id;
                                zlc.id_obroty = 0;
                                zlc.wpis = 0;
                                zlc.kzp = 0;
                                zlc.klauzula = 0;
                                zlc.kza = 0;
                                zlc.kl_adw = 0;
                                zlc.komo = 0;
                                zlc.inne_sad = 0;
                                zlc.ods_kapital = odskapital;
                                zlc.id_schemat = idSchemat;
                                zlc.wdz = wdz;

                                if (zlc_new) wiena.zlecenia_ksiegowe.Add(zlc);
                                if (odsSkapitalizowanePozew > 0 && zlc_new && mode == -1)
                                {
                                    zlecenia_ksiegowe zlc1 = new zlecenia_ksiegowe();
                                    zlc1.czyus = 0;
                                    zlc1.d_kreacji = zlc.d_kreacji;
                                    zlc1.kod = 1;
                                    zlc1.konto = idKonto;
                                    zlc1.winien = odsSkapitalizowanePozew;  // do potwierdzenia
                                    zlc1.ma = 0;
                                    zlc1.energia = odsSkapitalizowanePozew;
                                    zlc1.koszty = 0;
                                    zlc1.odsetki = 0;
                                    zlc1.inne = 0;
                                    zlc1.vat = 0;
                                    zlc1.nr_dowodu = "Uz3odsetki";
                                    zlc1.rodzaj = 0;
                                    zlc1.d_zlecenia = (mode == -1 ? DataZleceniaEOP(dataOtrzymania) : dataDokumentu);
                                    zlc1.id_sprawy = wiena_id;
                                    zlc1.id_obroty = 0;
                                    zlc1.wpis = 0;
                                    zlc1.kzp = 0;
                                    zlc1.klauzula = 0;
                                    zlc1.kza = 0;
                                    zlc1.kl_adw = 0;
                                    zlc1.komo = 0;
                                    zlc1.inne_sad = 0;
                                    zlc1.ods_kapital = odsSkapitalizowanePozew;
                                    zlc1.id_schemat = 3;
                                    zlc1.wdz = 0;
                                    wiena.zlecenia_ksiegowe.Add(zlc1);
                                    Utils.LogWriter("Dodano zlecenie księgowe dla odsetek skapitalizowanych");
                                }

                            }
                            //wiena.spr_sadowa.Where(a => a.id_sprawa == wiena_id). (a => a.id
                            Utils.LogWriter("Dodano zlecenie księgowe ");
                            wiena.SaveChanges(); // 11111111111111

                        }
                        if (dt == docTypes.ZgloszenieWierzytelnosci)
                        {
                            wiena.SaveChanges();
                            try
                            {
                                // przeksiegowanie 
                                SqlParameter parameter1 = new SqlParameter("@IdSprawy", wiena_id);
                                SqlParameter parameter2 = new SqlParameter("@DataZlecenia", (mode == -1 ? DataZleceniaEOP(dataOtrzymania) : dataOtrzymania));
                                SqlParameter parameter3 = new SqlParameter("@TypFirmy", mode);
                                SqlParameter parameter4 = new SqlParameter("@odsetki", odsetki);

                                wiena.Database.ExecuteSqlCommand("exec USP_PrzeksiegujUpadlosc @IdSprawy,@DataZlecenia, @TypFirmy, @odsetki ", parameter1, parameter2, parameter3, parameter4);
                            }
                            catch (Exception exx)
                            {
                                ;
                            }

                        }






        


                        if (dt == docTypes.Klauzula)
                        {  // zlecenie ksiągowe dla klauzuli
                            wiena.SaveChanges();

                            DateTime dzlecenia = new DateTime(2005, 12, 31);
                            decimal wpis = getZalegbyTyp(8, wiena, wiena_id);
                            decimal kzp = getZalegbyTyp(12, wiena, wiena_id);
                            decimal klauzula = getZalegbyTyp(9, wiena, wiena_id);
                            decimal kladwok = getZalegbyTyp(13, wiena, wiena_id);
                            decimal kosztyall = getZalegbyTyp(0, wiena, wiena_id);
                            decimal koszty_inne = kosztyall - wpis - kzp - klauzula - kladwok;
                            Utils.LogWriter(wpis.ToString() + " " + kzp.ToString() + " " + klauzula.ToString() + " " + kosztyall.ToString() + " " + koszty_inne.ToString() + " " + kladwok.ToString());
                            decimal winien;
                            decimal ma;
                            decimal energia;
                            decimal odsetkioblicz;
                            decimal inne;
                            decimal vat;
                            decimal kza;
                            decimal ods_kapital;
                            decimal kl_adw;
                            decimal wdzoblicz;
                            pws_new = false;
                            int id_konta_anal = getAllZaleg(wiena, wiena_id, out winien, out ma, out energia, out wdzoblicz, out odsetkioblicz, out inne, out vat, out kza, out kl_adw, out ods_kapital);
                            pws_new = false;
                            // zlecenie księgowe
                            zlecenia_ksiegowe zlc = null;
                            bool zlc_new = false;
                            List<zlecenia_ksiegowe> lstzlc = wiena.zlecenia_ksiegowe.Where(a => a.id_sprawy == wiena_id && a.konto == idKonto && a.nr_dowodu == dokKsie && a.czyus != 1).OrderBy(a => a.ident).ToList();
                            if (lstzlc != null)
                            {
                                foreach (zlecenia_ksiegowe z in lstzlc)
                                {
                                    if (z.d_ksiegowania == null || z.d_ksiegowania < dzlecenia)
                                    {
                                        zlc = z;
                                        break;
                                    }
                                }

                            }
                            if (zlc == null && (lstzlc == null || !lstzlc.Any()))
                            {
                                zlc = new zlecenia_ksiegowe();
                                zlc_new = true;
                            }
                            if (zlc != null)
                            {
                                zlc.czyus = 0;
                                zlc.d_kreacji = DateTime.Now;
                                zlc.kod = 1;
                                zlc.konto = idKonto;

                                zlc.ma = 0;
                                zlc.energia = energia;
                                zlc.koszty = kosztyall;
                                zlc.odsetki = odsetkioblicz + odsetki;
                                zlc.inne = inne;
                                zlc.winien = zlc.energia + zlc.odsetki + zlc.koszty + zlc.inne;
                                zlc.vat = vat;
                                zlc.nr_dowodu = dokKsie;
                                zlc.rodzaj = 0;
                                zlc.d_zlecenia = (mode == -1 ? DataZleceniaEOP(dood.DataRejestracji.Value.Date) : dood.DataRejestracji.Value.Date);
                                zlc.id_sprawy = wiena_id;
                                zlc.id_obroty = 0;
                                zlc.wpis = wpis;
                                zlc.kzp = kzp;
                                zlc.klauzula = klauzula;
                                zlc.kza = kza;
                                zlc.kl_adw = kladwok;
                                zlc.komo = 0;
                                zlc.inne_sad = koszty_inne;
                                zlc.ods_kapital = ods_kapital;
                                zlc.id_schemat = idSchemat;
                                zlc.konto_anal = id_konta_anal;
                                zlc.wdz = wdzoblicz;




                                if (zlc.winien > 0)
                                {
                                    Utils.LogWriter("Dodano zlecenie księgowe ");
                                    if (zlc_new)
                                    {
                                        //sprawdzenie jeszcze raz 
                                        List<zlecenia_ksiegowe> l = wiena.zlecenia_ksiegowe.Where(a => a.id_sprawy == wiena_id && a.konto == idKonto && a.nr_dowodu == dokKsie && a.czyus != 1).OrderBy(a => a.ident).ToList();
                                        if (l == null || (l != null && l.Count() == 0))
                                            wiena.zlecenia_ksiegowe.Add(zlc);
                                    }
                                }
                                else
                                    if (!zlc_new)
                                    wiena.Entry(zlc).State = EntityState.Unchanged;
                            }


                        }
                        else if (dt == docTypes.KlauzulaDoreczenia)
                        {
                            wiena.SaveChanges();

                            DateTime dzlecenia = new DateTime(2005, 12, 31);
                            decimal wpis = 0;
                            decimal kzp = 0;
                            decimal klauzula = kklauzuli;
                            decimal kladwok = kladw;
                            decimal kosztyall = KosztySądowe + kklauzuli + kladw;
                            decimal koszty_inne = KosztySądowe;
                            Utils.LogWriter(wpis.ToString() + " " + kzp.ToString() + " " + klauzula.ToString() + " " + kosztyall.ToString() + " " + koszty_inne.ToString() + " " + kladwok.ToString());
                            decimal winien;
                            decimal ma;
                            decimal energia;
                            decimal odsetkioblicz;
                            decimal inne;
                            decimal vat;
                            decimal kza;
                            decimal ods_kapital;
                            decimal kl_adw;
                            decimal wdzoblicz;
                            pws_new = false;
                            int id_konta_anal = getAllZaleg(wiena, wiena_id, out winien, out ma, out energia, out wdzoblicz, out odsetkioblicz, out inne, out vat, out kza, out kl_adw, out ods_kapital);
                            pws_new = false;
                            // zlecenie księgowe
                            zlecenia_ksiegowe zlc = null;
                            bool zlc_new = false;
                            List<zlecenia_ksiegowe> lstzlc = wiena.zlecenia_ksiegowe.Where(a => a.id_sprawy == wiena_id && a.konto == idKonto && a.nr_dowodu == dokKsie && a.czyus != 1).OrderBy(a => a.ident).ToList();
                            if (lstzlc != null)
                            {
                                foreach (zlecenia_ksiegowe z in lstzlc)
                                {
                                    if (z.d_ksiegowania == null || z.d_ksiegowania < dzlecenia)
                                    {
                                        zlc = z;
                                        break;
                                    }
                                }

                            }
                            if (zlc == null && (lstzlc == null || !lstzlc.Any()))
                            {
                                zlc = new zlecenia_ksiegowe();
                                zlc_new = true;
                            }
                            if (zlc != null)
                            {
                                zlc.czyus = 0;
                                zlc.d_kreacji = DateTime.Now;
                                zlc.kod = 1;
                                zlc.konto = idKonto;

                                zlc.ma = 0;
                                zlc.energia = 0;
                                zlc.koszty = kosztyall;
                                zlc.odsetki = 0;
                                zlc.inne = 0;
                                zlc.winien = zlc.koszty ;
                                zlc.vat = 0;
                                zlc.nr_dowodu = dokKsie;
                                zlc.rodzaj = 0;
                                zlc.d_zlecenia = (mode == -1 ? DataZleceniaEOP(dood.DataRejestracji.Value.Date) : dood.DataRejestracji.Value.Date);
                                zlc.id_sprawy = wiena_id;
                                zlc.id_obroty = 0;
                                zlc.wpis = 0;
                                zlc.kzp = 0;
                                zlc.klauzula = klauzula;
                                zlc.kza = 0;
                                zlc.kl_adw = kladwok;
                                zlc.komo = 0;
                                zlc.inne_sad = koszty_inne;
                                zlc.ods_kapital = 0;
                                zlc.id_schemat = idSchemat;
                                zlc.konto_anal = id_konta_anal;
                                zlc.wdz = 0;




                                if (zlc.winien > 0)
                                {
                                    Utils.LogWriter("Dodano zlecenie księgowe ");
                                    if (zlc_new)
                                    {
                                        //sprawdzenie jeszcze raz 
                                        List<zlecenia_ksiegowe> l = wiena.zlecenia_ksiegowe.Where(a => a.id_sprawy == wiena_id && a.konto == idKonto && a.nr_dowodu == dokKsie && a.czyus != 1).OrderBy(a => a.ident).ToList();
                                        if (l == null || (l != null && l.Count() == 0))
                                            wiena.zlecenia_ksiegowe.Add(zlc);
                                    }
                                }
                                else
                                    if (!zlc_new)
                                    wiena.Entry(zlc).State = EntityState.Unchanged;
                            }

                        }
                        else if (dt == docTypes.WniosekEgz)
                        {

                            vw_KosztyEgz koe = (from koegz in wiena.vw_KosztyEgz where koegz.id_sprawy == wiena_id select koegz).FirstOrDefault();
                            if (koe == null)
                            {

                                theError.description += "brak poprawnego księgowania dla sprawy w systemie Wiena zlecenie nie zostanie wystawione";
                                theError.level = ErrLevel.Error;
                                theError.code = -807;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                // return false;

                            }
                            else
                            {

                                DateTime dzlecenia = new DateTime(2005, 12, 31);
                                zlecenia_ksiegowe zlc = null;
                                bool zlc_new = false;
                                List<zlecenia_ksiegowe> lstzlc = wiena.zlecenia_ksiegowe.Where(a => a.id_sprawy == wiena_id && (a.konto == idKonto || a.konto == 0) && a.nr_dowodu == dokKsie).OrderBy(a => a.ident).ToList();
                                if (lstzlc != null)
                                {
                                    foreach (zlecenia_ksiegowe z in lstzlc)
                                    {
                                        if (z.d_ksiegowania == null || z.d_ksiegowania < dzlecenia)
                                        {
                                            zlc = z;
                                            break;
                                        }
                                    }

                                }
                                if (zlc == null && (lstzlc == null || !lstzlc.Any()))
                                {
                                    zlc = new zlecenia_ksiegowe();
                                    zlc_new = true;
                                    zlc.czyus = 0;
                                    zlc.d_kreacji = DateTime.Now;
                                    zlc.d_ksiegowania = null;
                                }

                                if (zlc != null)
                                {

                                    zlc.d_zlecenia = (mode == -1 ? DataZleceniaEOP(dataDokumentu) : dataDokumentu);

                                    zlc.id_sprawy = wiena_id;

                                    zlc.kod = 1;

                                    zlc.konto = idKonto;
                                    zlc.id_obroty = 0;
                                    zlc.nr_dowodu = dokKsie;

                                    zlc.konto_anal = koe.konto_anal;
                                    zlc.wdz = koe.wdz;
                                    zlc.odsetki = koe.odsetki.Value + odsetki;
                                    zlc.ods_kapital = koe.ods_kapital;

                                    zlc.kza = koe.kza;
                                    zlc.kl_adw = koe.kl_adw;
                                    zlc.rodzaj = 0;

                                    zlc.id_schemat = idSchemat;
                                    zlc.wpis = koe.wpis;

                                    zlc.kzp = koe.kzp;
                                    zlc.klauzula = koe.klauzula;
                                    zlc.inne_sad = koe.inne_sad;
                                    zlc.kza = koe.kzaa;

                                    zlc.vat = koe.vat ?? 0;
                                    zlc.komo = 0;

                                    zlc.energia = koe.energia.Value;
                                    zlc.winien = zlc.energia + zlc.odsetki + zlc.inne + zlc.kzp.Value + zlc.wpis.Value + zlc.klauzula.Value + zlc.kza.Value + zlc.kl_adw.Value + zlc.komo.Value + zlc.inne_sad.Value;

                                    zlc.koszty = zlc.kzp.Value + zlc.wpis.Value + zlc.klauzula.Value + zlc.kza.Value + zlc.kl_adw.Value + zlc.komo.Value + zlc.inne_sad.Value;

                                    if (zlc_new)
                                        wiena.zlecenia_ksiegowe.Add(zlc);
                                    Utils.LogWriter("Dodano zlecenie księgowe 1");
                                }
                                //  wiena.SaveChanges();

                            }
                        }
                        // zdodanie 
                        if (dt == docTypes.WniosekEgz || dt == docTypes.Bezskuteczne)
                        {

                            spr_egz se = wiena.spr_egz.Where(a => a.id_sprawy == wiena_id && a.aktualna == 1 && a.czyus == 0).FirstOrDefault();
                            if (se == null || (se != null && se.id_komornik == 0))
                            {
                                if (!string.IsNullOrWhiteSpace(komornik))
                                {
                                    ;

                                }

                            }



                            bool se_new = true;
                            if (se != null)
                            {
                                se_new = false;
                                se.d_modyfikacji = DateTime.Now;
                            }
                            else
                            {
                                // deaktywacja spr_egz 

                                se = new spr_egz();
                                se_new = true;
                                se.d_kreacji = DateTime.Now;
                                se.id_komornik = 0;
                            }
                            se.aktualna = 1;
                            se.id_sprawy = wiena_id;
                            if (dt == docTypes.WniosekEgz)
                                se.data_wniosku = dataDokumentu;
                            if (!String.IsNullOrWhiteSpace(sygnKM))
                            {
                                se.km = sygnKM;
                            }
                            se.czyus = 0;

                            if (dt == docTypes.Bezskuteczne)
                            { // jeśli bezskuteczna egzekucja/.

                                se.przyczyna_umo = 1;
                                se.data_umo = dataDokumentu;
                                se.data_dor_umo = dataOtrzymania;


                            }
                            if (se_new)
                                wiena.spr_egz.Add(se);


                        }





                        lexena.SaveChanges();
                        Utils.LogWriter("Zapisano do LexEna  " + docName);
                        wiena.SaveChanges();
                        Utils.LogWriter("Zapisano do wieny  " + docName);


                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                        Utils.LogWriter(" Import OK:  " + docName + " " + Sygnatura);

                    }



                }
                return true;
            }
            catch (DbUpdateException exep)
            {
                var innerEx = exep.InnerException;
                while (innerEx.InnerException != null)
                {
                    innerEx = innerEx.InnerException;
                    Utils.LogWriter(innerEx.Message);
                }
                Utils.LogWriter("Błąd w trakcie rejestracji dokumentu " + " zbiór" + docName);
                this.addError(ErrLevel.Fatal, -733, "Błąd podczas importu spraw " + docName);
                return false;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format(
                            "- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                if (outputLines.Any())
                {
                    foreach (string s in outputLines)
                    {
                        Utils.LogWriter(s);

                    }

                }
                Utils.LogWriter("Błąd w trakcie rejestracji dokumentu " + " zbiór" + docName);
                this.addError(ErrLevel.Fatal, -733, "Błąd podczas importu spraw ", docName);
                return false;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie rejestracji dokumentu " + ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "") + " zbiór" + docName);
                this.addError(ErrLevel.Fatal, -733, "Błąd podczas importu spraw " + ex.Message + ((ex.InnerException != null) ? ex.InnerException.Message : ""), docName);
                return false;

            }
        }









        /*koniec obsługi skanów */



        public bool ValidateDocument()
        {


            if (isXML == 2)
            {
                Dictionary<string, byte[]> lst = ValidateTradDoc();
                docTypes? dtyp = null;
                if (lst == null)
                    return false;
                // sprawdzenie rodzaju dokumentu
                dtyp = getDocType(importZipFileName);



                if (dtyp == null)
                {
                    errDescription = "Nierozpoznany rodzaj dokumentów";
                    return false;
                }
                switch (dtyp)
                {
                    case docTypes.Pozew:
                        typDok = DocType.Pozew;
                        break;
                    case docTypes.Nakaz:
                        typDok = DocType.Nakaz;
                        break;
                    case docTypes.Wyrok:
                        typDok = DocType.Wyrok;
                        break;
                    case docTypes.Klauzula:
                        typDok = DocType.Klauzula1;
                        break;
                    case docTypes.WniosekEgz:
                        typDok = DocType.Wniosek;
                        break;
                    case docTypes.Oddalenie:
                        typDok = DocType.Oddalenie;
                        break;
                    case docTypes.Sprzeciw:
                        typDok = DocType.Sprzeciw;
                        break;
                    case docTypes.Bezskuteczne:
                        typDok = DocType.Bezskutecza;
                        break;
                    case docTypes.Uchylenie:
                        typDok = DocType.Uchylenie;
                        break;
                    case docTypes.Umorzenie:
                        typDok = DocType.Umorzenie;
                        break;
                    case docTypes.UmorzenieEPU:
                        typDok = DocType.UmorzenieEPU;
                        break;
                    case docTypes.Sprostowanie:
                        typDok = DocType.Sprostowanie;
                        break;
                    case docTypes.Zgon:
                        typDok = DocType.Zgon;
                        break;
                    case docTypes.OdrzucenieZgon:
                        typDok = DocType.OdrzucenieZgon;
                        break;
                    case docTypes.Doreczenie:
                        typDok = DocType.Doreczenie;
                        break;
                    case docTypes.KlauzulaDoreczenia:
                        typDok = DocType.DoreczenieKlauzula;
                        break;
                    case docTypes.ZgloszenieWierzytelnosci:
                        typDok = DocType.ZgloszenieWierzytelnosci;
                        break;
                    default:
                        typDok = DocType.None;
                        break;
                }

                List<xlsxItem> lstXlsx = parseXLSX();
                        
                if (lstXlsx == null)
                {
                    errDescription = "Błąd parsowania zbioru XLSX";
                    return false;

                }
                foreach (var item in lst)
                {

                    string sygn = sygnFromDocName(item.Key);
                    if (String.IsNullOrWhiteSpace(sygn))
                    {
                        this.addError(ErrLevel.Error, -903, "Błąd - brak sygnatury w nazwie " + item.Key, item.Key);
                        continue;
                    }
                    else
                        sygn = sygn.ToUpper();


                    xlsxItem descript = lstXlsx.Where(a => a.sygnatura == sygn).FirstOrDefault();
                    if (descript == null)
                    {
                        this.addError(ErrLevel.Error, -901, "Brak wiersza opisującego   dokument dla " + item.Key, sygn);
                        continue;
                    }
                    // typDok = DocType.Doreczenie;
                    if (typDok == DocType.Doreczenie || typDok == DocType.DoreczenieKlauzula)
                    {
                        descript.ks = descript.doreczenia;
                        descript.klauzula = descript.klauzulakosztdoreczenia;
                        descript.kladw = descript.kladwdoreczenia;
                    }
                    addDocScan(dtyp.Value, item.Value, item.Key, descript.dataDokumentu, descript.dataRejestracji, 0, descript.ks, descript.kzp, descript.kinne, 0, descript.sygnSad, descript.nalglowna, descript.odskapital, descript.wzd, descript.klauzula, descript.sad, descript.wydzial, descript.sadmiasto, descript.odsetki, descript.komornik, descript.sygnKM, descript.kladw, this.FirmaTyp);


                }

                return true;
            }
            typDok = recognizeDocType(inDOC);



            if (typDok == DocType.None)
            {
                this.addError(ErrLevel.Fatal, -500, "Błędny format zbioru wejściowego - brak zgodności ze schematem", this.importFileName);
                return false;

            }
            if ((typDok == DocType.Empty))
            {
                this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty", this.importFileName);
                return false;
            }
            switch (typDok)
            {
                case DocType.Pozew:

                    return !ValidatePozew();
                case DocType.Sprawa1:
                case DocType.Sprawa2:
                    return !ValidateSprawa();
                case DocType.Klauzula1:
                case DocType.Klauzula2:
                    return !ValidateKlauzula();
                case DocType.Nakaz:
                    return !ValidateNakaz();
                case DocType.Postanowienie1:
                case DocType.Postanowienie2:
                    return !ValidateOrzeczenie();
                case DocType.Wniosek:
                    return !ValidateWniosekEgz();
                case DocType.KosztyEgz:
                    return !ValidateKosztyEgz();
                case DocType.CzynnosciKom:
                    return !ValidateCzynnosci();
                default:
                    break;
            }
            return false;
        }



        public bool ValidateSprawa()
        {
            MojeSprawyOutputData mojesprawy;
            MojeSprawyEPU listamoichspraw;
            SprawaOutputElement[] listaspraw;
            String sygnatura;
            String SygnWgPowoda;
            NazwaStatusu oststat;
            int idSprawy;
            byte[] pdffile;
            NazwaStatusu nazstat;
            pdffile = new byte[0];
            DateTime dgraniczna = DateTime.Today.AddDays(-120);
            bool skipNewUpdates = false;
            bool pws_new = false;
            Utils.LogWriter("Walidacja spraw ze zbioru : " + importFileName);
            //  Utils.LogNamedWriter(" Start Importu zbioru: " + fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + "raport.txt");

            SygnWgPowoda = "";

            if (typDok == DocType.Sprawa1)
            {
                if (this.paczkaspraw1 == null || !this.paczkaspraw1.listaSpraw.Any())
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie deserializacji zpraw  zbiór:" + importFileName + " jest pusty");
                    return true;
                }
            }
            else // sprawa 2
            {
                if (this.paczkaspraw2 == null || !this.paczkaspraw2.mojspr.Any())
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie deserializacji zpraw  zbiór:" + importFileName + " jest pusty");
                    return true;
                }

            }

            Utils.LogWriter(" Start walidacji zbioru: " + importFileName);
            SygnWgPowoda = "";
            try
            {


                if (typDok == DocType.Sprawa1)
                {
                    listaspraw = this.paczkaspraw1.listaSpraw;
                }
                else
                {

                    int i = 0;
                    listaspraw = new SprawaOutputElement[this.paczkaspraw2.mojspr.Count];// mojesprawy.listaSpraw;


                    foreach (MojaSprawaEPU myspr in this.paczkaspraw2.mojspr)
                    {
                        listaspraw[i] = new SprawaOutputElement();
                        listaspraw[i].id = myspr.Id;
                        listaspraw[i].kwotaSporu = myspr.KwotaSporu;
                        listaspraw[i].rolaWSprawie = myspr.RolaWSprawie;
                        listaspraw[i].stanSprawy = myspr.StanSprawy;
                        listaspraw[i].sygnaturaSprawy = myspr.SygnaturaSprawy;
                        listaspraw[i].sygnaturaWgPowoda = myspr.SygnaturaWgPowoda;
                        i++;
                    }

                }
                using (wiena_centralEntities wiena = new wiena_centralEntities())
                {
                    using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                    {

                        bool iserror = false;

                        foreach (SprawaOutputElement pojsprawa in listaspraw)
                        {
                            errDescription theError = new errDescription();
                            StatusSprawy stspr = null;


                            skipNewUpdates = false;

                            SygnWgPowoda = pojsprawa.sygnaturaWgPowoda;

                            theError.level = ErrLevel.OK;
                            theError.code = 0;
                            theError.description = "";
                            theError.reference = SygnWgPowoda;




                            String OrygSygn = SygnWgPowoda;
                            Utils.LogWriter(" Start Importu Sprawy: " + SygnWgPowoda);
                            idSprawy = 0;
                            if (SygnWgPowoda.Trim().StartsWith("ENERGA#"))
                            {
                                SygnWgPowoda += "@0";
                                idSprawy = 0;
                                theError.level = ErrLevel.Warning;
                                theError.code = 100;
                                theError.description = " Niepoprawne oznaczenie Sygnatury wg powoda ";
                            }
                            else
                            {
                                if (SygnWgPowoda.Contains("#"))
                                {
                                    if (SygnWgPowoda.Contains("#"))
                                        idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                                    else
                                        idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));
                                }
                                else
                                {



                                }
                            }
                            // dodanie encji pozew 

                            Sprawa spr = (from z in lexena.Sprawa
                                          where z.id == idSprawy
                                          select z).FirstOrDefault();
                            if (spr == null)
                            {
                                if (SygnWgPowoda.IndexOf('@') > 0)
                                    sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                                else
                                    sygnatura = SygnWgPowoda;

                                spr = (from z in lexena.Sprawa
                                       where z.sygnatura == sygnatura
                                       select z).FirstOrDefault();


                            }

                            if (spr == null)
                            {
                                spr = (from z in lexena.Sprawa
                                       where z.Uwagi == OrygSygn
                                       select z).FirstOrDefault();


                            }
                            if (spr == null)
                            {
                                Utils.LogWriter("Błąd w trakcie wczytywania Sprawy, brak sprawy " + SygnWgPowoda);
                                this.addError(ErrLevel.Error, -502, "brak sprawy o takim oznaczeniu lub oznaczenie sprawy niezgodne ze standardem Energa " + SygnWgPowoda, SygnWgPowoda);
                                continue;
                            }
                            else
                                idSprawy = spr.id;



                            spr = (from z in lexena.Sprawa
                                   where z.id == idSprawy
                                   select z).FirstOrDefault();



                            nazstat = (from x in lexena.NazwaStatusu
                                       where x.Krok == 3
                                       select x).FirstOrDefault();
                            oststat = (from x in lexena.NazwaStatusu
                                       join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                       where y.Sprawa_id == idSprawy
                                       orderby x.Krok descending
                                       select x).FirstOrDefault();
                            if (oststat != null)
                            {
                                if (oststat.Krok > 2)
                                {


                                    Utils.LogWriter("Sprawa ma już wyższy status " + oststat.Krok.ToString() + " " + SygnWgPowoda);
                                    this.addError(ErrLevel.Warning, -601, "Sprawa ma już wyższy status " + oststat.Krok.ToString() + " - pozew nie zostanie zaimportowany ", SygnWgPowoda);
                                    // continue;
                                }

                            }

                            PdfStore pozpdf = (from x in lexena.DokWys
                                               join y in lexena.PdfStore on x.Id equals y.DokWys_Id
                                               where x.TypDok == 10 && x.Sprawa_id == idSprawy && x.DataDok > dgraniczna
                                               orderby x.Id descending
                                               select y).FirstOrDefault();


                            if (pozpdf == null || (pozpdf != null && pozpdf.value == null))
                            {


                                Utils.LogWriter("Brak dokumentu pdf pozwu  " + SygnWgPowoda);
                                this.addError(ErrLevel.Error, -602, "Brak dokumentu pozwu;  zaimportuj pozew i ponownie sprawę  ", SygnWgPowoda);
                                continue;
                            }
                            DokWys dws = (from x in lexena.DokWys
                                          where x.TypDok == 10 && x.Sprawa_id == idSprawy
                                          orderby x.Id descending
                                          select x).FirstOrDefault();
                            if (dws == null)
                            {


                                Utils.LogWriter("Brak dokumentu  pozwu  " + SygnWgPowoda);
                                this.addError(ErrLevel.Error, -622, "Brak dokumentu pozwu;  zaimportuj pozew i ponownie sprawę  ", SygnWgPowoda);
                                continue;
                            }

                            dws.d_modyfikacji = DateTime.Now;
                            if (spr != null && nazstat != null)
                            {
                                stspr = new StatusSprawy();
                                stspr.czyus = 0;
                                stspr.CzyWiena = 1;
                                stspr.DataStatusu = DateTime.Now;
                                stspr.Sprawa_id = spr.id;
                                stspr.NazwaStatusu_Id = nazstat.Id;
                                lexena.AddToStatusSprawy(stspr);

                                // dodanie statusu sprawy 

                            }
                            spr.SygnNCe = pojsprawa.sygnaturaSprawy;
                            spr.IdSprawyEPU = pojsprawa.id;
                            spr.WPS = pojsprawa.kwotaSporu;

                            Utils.LogWriter("Aktualizacja Wiena " + SygnWgPowoda);
                            // zapis do Wieny;
                            // import do Wieny
                            {
                                int wiena_id = (from x in wiena.sprawa join f in wiena.firma on x.id_firmy equals f.ident where x.sygnatura == spr.sygnatura && f.typ_firmy == FirmaTyp select x.ident).FirstOrDefault();
                                if (wiena_id == 0)
                                {
                                    iserror = true;
                                    Utils.LogWriter("Brak sprawy w systemie Wiena" + SygnWgPowoda);
                                    this.addError(ErrLevel.Error, -562, "Brak sprawy w systemie Wiena " + SygnWgPowoda + " w systemie Energii", SygnWgPowoda);
                                    if (stspr != null) lexena.StatusSprawy.DeleteObject(stspr);
                                    lexena.Sprawa.Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, spr);

                                    continue;

                                }
                                Utils.LogWriter("Badanie statusu " + SygnWgPowoda);
                                status s = (from x in wiena.status
                                            join y in wiena.status_spr on x.ident equals y.id_statusu
                                            where y.id_sprawy == wiena_id && y.czyus == 0
                                            orderby y.od_dnia descending
                                            select x).FirstOrDefault();

                                if (s == null || s.numer != -1)
                                {
                                    Utils.LogWriter("Niepoprawny status sprawy w systemie Wiena" + SygnWgPowoda);
                                    iserror = true;
                                    this.addError(ErrLevel.Warning, -563, "Niepoprawny status sprawy w systemie Wiena " + SygnWgPowoda, SygnWgPowoda);
                                    if (stspr != null) lexena.StatusSprawy.DeleteObject(stspr);
                                    lexena.Sprawa.Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, spr);

                                    //continue;

                                }
                                // sprawdzamy czy jest pozew
                                pisma_wys pws = null;
                                pws_new = false;
                                pws = wiena.pisma_wys.Where(a => a.id_sprawy == wiena_id && a.data_r == dws.DataDok && a.nazwa_pisma == dws.Nazwa && a.czyus == 0).FirstOrDefault();
                                if (pws == null)
                                {
                                    pws = new pisma_wys();
                                    pws_new = true;
                                }
                                pws.adresat = 2;
                                pws.czyus = 0;
                                pws.data_r = dws.DataDok;
                                pws.d_kreacji = DateTime.Now;
                                pws.id_sprawy = wiena_id;
                                pws.nazwa2 = "Sąd Rejonowy Lublin Zachód w Lublinie";
                                pws.nazwa3 = "VI wydział cywilny EPU";
                                pws.nazwa_pisma = dws.Nazwa;
                                pws.odpis = 0;
                                pws.stan = 1;
                                pws.sygnatura = spr.sygnatura;
                                pws.sygn_psm = "P_0001";
                                pws.termin = 0;
                                pws.typ_dok_tresc = 1;
                                pws.tresc_bin = pozpdf.value;
                                if (pws_new)
                                    wiena.pisma_wys.Add(pws);

                                // 
                                Utils.LogWriter("Dodanie zlecenia księgowego");
                                DateTime dzlecenia = new DateTime(2005, 12, 31);
                                zlecenia_ksiegowe zlc = null;
                                bool zlc_new = false;
                                zlc = wiena.zlecenia_ksiegowe.Where(a => a.id_sprawy == wiena_id && a.konto == 12 && a.nr_dowodu == "Uz3" && a.czyus == 0).FirstOrDefault();


                                if (zlc == null)
                                {
                                    zlc = new zlecenia_ksiegowe();
                                    zlc_new = true;
                                }
                                else
                                    if (zlc.d_ksiegowania >= zlc.d_zlecenia)
                                {
                                    skipNewUpdates = true;
                                    goto sprsadowaLbl;
                                }
                                zlc.czyus = 0;
                                zlc.d_kreacji = DateTime.Now;
                                zlc.kod = 1;
                                zlc.konto = 12;
                                zlc.winien = spr.WPS ?? 0;
                                zlc.ma = 0;
                                zlc.energia = spr.WPS ?? 0;
                                zlc.koszty = 0;
                                zlc.odsetki = 0;
                                zlc.inne = 0;
                                zlc.d_zlecenia = dws.DataDok.Value;
                                // powinno być od daty złożenia pozwu
                                decimal? vat = (from x in wiena.naleznosc where x.id_sprawy == wiena_id && x.vat != null && x.vat > 0 select x.vat).Sum();
                                decimal? vatSplata = (from x in wiena.naleznosc join wp in wiena.wplata_podz on x.ident equals wp.id_nal join w in wiena.wplata on wp.id_wplaty equals w.ident where w.data_w <= zlc.d_zlecenia && x.id_sprawy == wiena_id && x.vat != null && x.vat > 0 select wp.spl_vat).Sum();

                                if (vatSplata == null) vatSplata = 0;
                                if (vat == null) vat = 0;
                                Utils.LogWriter("Vat ustalony");
                                zlc.vat = vat.Value - vatSplata.Value;
                                zlc.nr_dowodu = "Uz3";
                                zlc.rodzaj = 0;

                                zlc.id_sprawy = wiena_id;
                                zlc.id_obroty = 0;
                                zlc.wpis = 0;
                                zlc.kzp = 0;
                                zlc.klauzula = 0;
                                zlc.kza = 0;
                                zlc.kl_adw = 0;
                                zlc.komo = 0;
                                zlc.inne_sad = 0;
                                zlc.ods_kapital = (dws.NotyOdsetkowe == null ? 0 : dws.NotyOdsetkowe);
                                zlc.id_schemat = 13;
                                zlc.wdz = (dws.OdsetkiKapital == null ? 0 : dws.OdsetkiKapital);
                                if (zlc_new) wiena.zlecenia_ksiegowe.Add(zlc);

                                sprsadowaLbl:
                                //wiena.spr_sadowa.Where(a => a.id_sprawa == wiena_id). (a => a.id
                                Utils.LogWriter("Dodano zlecenie księgowe ");
                                List<spr_sadowa> ssad = wiena.spr_sadowa.Where(a => a.id_sprawa == wiena_id).ToList();
                                if (ssad != null && ssad.Any())
                                {
                                    wiena.spr_sadowa.Remove(ssad.FirstOrDefault());

                                }
                                spr_sadowa ss = new spr_sadowa();
                                ss.aktywne = 1;
                                ss.id_sprawa = wiena_id;
                                ss.id_sad = 999;
                                ss.d_kreacji = DateTime.Now;
                                ss.data_pozwu = dws.DataDok;
                                ss.sygn_nakazowe = spr.SygnNCe;
                                ss.czyus = 0;

                                wiena.spr_sadowa.Add(ss);
                                Utils.LogWriter("Dodanie sprawa sądowa w Wienie");
                                if (!skipNewUpdates)
                                {
                                    if (dws.NotyOdsetkowe != null && dws.NotyOdsetkowe > 0)
                                    {
                                        Utils.LogWriter("Zmiana dat not i wdz");
                                        List<Naleznosc> lstNal = (from n in lexena.Naleznosc
                                                                  join sp in lexena.Sprawa on n.Sprawa_id equals sp.id
                                                                  join tn in lexena.TypNaleznosci on n.TypNaleznosci_id equals tn.id
                                                                  where sp.id == spr.id && tn.CzyOdsKapital == 1
                                                                  select n).ToList();
                                        Utils.LogWriter("Zmiana dat not i wdz");
                                        if (lstNal != null && lstNal.Any())
                                        {
                                            foreach (Naleznosc nale in lstNal)
                                            {
                                                Utils.LogWriter("Pytanie o należości");
                                                naleznosc nalw = wiena.naleznosc.Where(a => a.ident == nale.IdWiena).OrderByDescending(a => a.ident).FirstOrDefault();
                                                if (nalw != null)
                                                {
                                                    Utils.LogWriter("Zmiana odsetek nal należności:  " + nalw.ident.ToString());
                                                    ods_sprawy odspr = wiena.ods_sprawy.Where(a => a.id_nal == nalw.ident).OrderByDescending(a => a.data_p).FirstOrDefault();
                                                    Utils.LogWriter("Zmnaleziono odsetki  " + nalw.ident.ToString());
                                                    if (odspr != null)
                                                    {
                                                        odspr.data_p = dws.DataDok;

                                                    }
                                                }


                                            }
                                        }
                                    }
                                }

                                Utils.LogWriter("Dodanie statusu w Wienie");

                                // dodanie statusu sprawy;
                                status_spr ost_sspr = wiena.status_spr.Where(a => a.id_sprawy == wiena_id && a.czyus == 0).OrderByDescending(a => a.od_dnia).ThenByDescending(a => a.ident).FirstOrDefault();

                                if ((ost_sspr == null || (ost_sspr != null && ost_sspr.id_statusu != 9 && ost_sspr.id_statusu != 11 && ost_sspr.id_statusu != 108)))
                                {

                                    status_spr wstat = new status_spr();
                                    wstat.id_sprawy = wiena_id;
                                    wstat.czyus = 0;
                                    wstat.d_kreacji = DateTime.Now;
                                    wstat.od_dnia = dws.DataDok.Value;
                                    wstat.id_statusu = 1;
                                    wiena.status_spr.Add(wstat);

                                }
                            }
                            Utils.LogWriter("Zapis Lexena");

                            lexena.SaveChanges();
                            Utils.LogWriter(" Import do LexEny OK:  " + SygnWgPowoda);
                            wiena.SaveChanges();
                            Utils.LogWriter(" Import do Wieny OK:  " + SygnWgPowoda);
                            this.addError(theError.level, theError.code, theError.description, theError.reference);
                        }
                    }

                }
                return true;
            }

            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania Spraw " + ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "") + " zbiór" + this.importFileName);
                this.addError(ErrLevel.Fatal, -503, "Błąd podczas importu spraw " + ex.Message + ((ex.InnerException != null) ? ex.InnerException.Message : ""), SygnWgPowoda);
                return false;

            }



        }
        public bool ValidateNakaz()
        {
            MojeNakazyOutputData nakazy;
            NakazOutputElement[] lista_nakazow;


            string sygnatura;
            NazwaStatusu oststat;
            PdfStore pdf;

            String SygnWgPowoda;
            int idSprawy;
            string retvalue;
            decimal Noty;
            byte[] pdffile;
            string nakazStr;
            Sprawa spr;
            bool newDok;
            Nakazy nakazybest;
            NazwaStatusu nazstat;
            int dokOdebr_id;
            pdffile = new byte[0];
            DateTime dgraniczna = DateTime.Today.AddDays(-120);
            bool skipNewUpdates = false;
            bool pws_new = false;

            Utils.LogWriter("Walidacja spraw ze zbioru : " + importFileName);
            //  Utils.LogNamedWriter(" Start Importu zbioru: " + fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + "raport.txt");

            SygnWgPowoda = "";
            if (this.paczkanakazow == null || !this.paczkanakazow.mojeNakazy.Any())
            {
                this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                Utils.LogWriter("Błąd w trakcie deserializacji zpraw  zbiór:" + this.importFileName + " jest pusty");
                return true;
            }



            Utils.LogWriter(" Start walidacji zbioru: " + this.importFileName);
            SygnWgPowoda = "";
            try
            {


                lista_nakazow = this.paczkanakazow.mojeNakazy;

                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {
                    using (wiena_centralEntities wiena = new wiena_centralEntities())
                    {

                        bool iserror = false;


                        foreach (NakazOutputElement nakaz in lista_nakazow)
                        {
                            int wiena_id = 0;
                            idSprawy = 0;
                            skipNewUpdates = false;

                            nakazStr = nakaz.dokumentXml;
                            SygnWgPowoda = nakaz.sygnaturaWgPowoda;

                            errDescription theError = new errDescription();

                            theError.level = ErrLevel.OK;
                            theError.code = 0;
                            theError.description = "";
                            theError.reference = SygnWgPowoda;

                            spr = null;
                            Utils.LogWriter(" Start Importu nakazu: " + SygnWgPowoda + " " + nakaz.sygnaturaSprawy);

                            if (!String.IsNullOrEmpty(SygnWgPowoda.Trim()) && (!SygnWgPowoda.Trim().StartsWith("ENERGA#") && SygnWgPowoda.Contains("@")))
                            {
                                if (SygnWgPowoda.Contains("#"))
                                    idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                                else
                                    idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));
                                // dodanie encji pozew
                                spr = (from z in lexena.Sprawa
                                       where z.id == idSprawy
                                       select z).FirstOrDefault();
                                if (spr == null)
                                {
                                    sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                                    spr = (from z in lexena.Sprawa
                                           where z.sygnatura == sygnatura
                                           select z).FirstOrDefault();


                                }
                            }
                            else
                            {
                                spr = (from z in lexena.Sprawa
                                       where z.sygnatura == SygnWgPowoda
                                       select z).FirstOrDefault();
                                if (spr == null)
                                {

                                    if (!String.IsNullOrEmpty(nakaz.sygnaturaSprawy))
                                    {
                                        spr = (from z in lexena.Sprawa
                                               where z.SygnNCe.Replace(" ", "").Contains(nakaz.sygnaturaSprawy.Replace(" ", "").Replace("VI", ""))
                                               select z).FirstOrDefault();
                                        if (spr != null) SygnWgPowoda = spr.sygnatura;
                                    }
                                }
                            }
                            if (spr == null)
                            {
                                Utils.LogWriter("Błąd w trakcie wczytywania Nakazu " + SygnWgPowoda);
                                this.addError(ErrLevel.Error, -502, "brak sprawy o takim oznaczeniu lub oznaczenie sprawy niezgodne ze standardem Energa " + SygnWgPowoda, SygnWgPowoda);
                                continue;
                            }
                            else
                                idSprawy = spr.id;

                            spr = (from z in lexena.Sprawa
                                   where z.id == idSprawy
                                   select z).FirstOrDefault();
                            nazstat = (from x in lexena.NazwaStatusu
                                       where x.Krok == 4
                                       select x).FirstOrDefault();
                            oststat = (from x in lexena.NazwaStatusu
                                       join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                       where y.Sprawa_id == idSprawy
                                       orderby x.Krok descending
                                       select x).FirstOrDefault();


                            if (oststat.Krok > 4)
                            {

                                Utils.LogWriter("Sprawa ma już wyższy status " + oststat.Krok.ToString() + " " + SygnWgPowoda);
                                this.addError(ErrLevel.Warning, -721, "Sprawa ma już wyższy status " + oststat.Krok.ToString() + " - zapisy dot. nakazu wymagają weryfikacji", SygnWgPowoda);
                                skipNewUpdates = true;
                                //continue;
                            }

                            wiena_id = (from x in wiena.sprawa join f in wiena.firma on x.id_firmy equals f.ident where x.sygnatura == spr.sygnatura && f.typ_firmy == FirmaTyp select x.ident).FirstOrDefault();
                            if (wiena_id == 0)
                            {
                                iserror = true;
                                this.addError(ErrLevel.Error, -562, "Brak sprawy w systemie Wiena " + SygnWgPowoda + " w systemie Energii", SygnWgPowoda);
                                lexena.Sprawa.Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, spr);
                                continue;
                            }
                            nakaz.dataOrzeczenia = nakaz.dataOrzeczenia.Date;

                            DokOdebr dood = (from z in lexena.DokOdebr
                                             where z.Sprawa_id == idSprawy && z.TypDok == 5 && z.DataDokumentu == nakaz.dataOrzeczenia
                                             select z).FirstOrDefault();
                            if (dood == null)
                            {
                                dood = new DokOdebr();
                                newDok = true;
                                dood.d_modyfikacji = DateTime.Now;
                                dokOdebr_id = 0;
                                //   dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                dokOdebr_id = dood.Id;
                                newDok = false;
                            }

                            dood.d_kreacji = DateTime.Today;
                            dood.DataDokumentu = nakaz.dataOrzeczenia.Date;
                            dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                            dood.CzyEPU = 1;
                            dood.IdEPU = nakaz.id;
                            dood.Nazwa = "Nakaz";
                            dood.TypDok = 5; // nakza zapłaty
                            dood.Sprawa_id = idSprawy;
                            dood.PartitionKey = 0;
                            dood.Format = 100;
                            dood.StatusDok = "wydano";
                            dood.Tresc = correctDecimals(nakaz.dokumentXml);
                            dood.TrescHtml = XML2HTMLTransform.TransformNCompress(dood.Tresc, 5);
                            dood.CzyZalatw = 0;
                            NakazEPU nepu = (NakazEPU)XmlDeserializeFromString(dood.Tresc, typeof(NakazEPU), null);
                            // weryfikacja kosztów;
                            decimal kwotaK = 0;
                            decimal sumNal = 0;
                            decimal prow = 0;
                            decimal ks = 0;
                            decimal kzp = 0;

                            //          if (!skipNewUpdates)
                            {
                                if (nepu != null && nepu.ListaRoszczen != null && nepu.ListaRoszczen.Any())
                                {
                                    typRoszczenieNakaz tn = nepu.ListaRoszczen.Where(a => a.typ == 13).FirstOrDefault();
                                    if (tn != null)

                                        kwotaK = tn.wartosc;
                                    // należnosci zasądzone
                                    sumNal = nepu.ListaRoszczen.Where(a => a.typ != 13).Sum(a => a.wartosc);
                                    // znajdż ostatni  pozwe w sprawie
                                    DokWys dw = lexena.DokWys.Where(a => a.Sprawa_id == spr.id && a.TypDok == 10).OrderByDescending(a => a.DataDok).FirstOrDefault();
                                    if (dw != null)
                                    {
                                        prow = (dw.InneKoszty != null ? dw.InneKoszty.Value : 0);
                                        ks = (dw.Koszty != null ? dw.Koszty.Value : 0);
                                    }
                                    if (ks > 0)
                                    {
                                        // są w pozwie koszty 
                                        kzp = kwotaK - ks - prow;
                                    }
                                    else
                                       if (EPUCalc.countOplata(sumNal) + EPUCalc.countProwizja(sumNal) + EPUCalc.countKZP(sumNal) == kwotaK)
                                    {
                                        ks = EPUCalc.countOplata(sumNal);
                                        prow = EPUCalc.countProwizja(sumNal);
                                        kzp = EPUCalc.countKZP(sumNal);

                                    }
                                    else
                                    {
                                        ks = EPUCalc.countOplata(sumNal);
                                        prow = kwotaK - Decimal.Ceiling(kwotaK); //EPUCalc.countProwizja(sumNal);
                                        kzp = kwotaK - ks - prow;

                                    }

                                }

                            }

                            if (KontoEpu > 0) dood.KontoEPU_Id = KontoEpu;
                            // czy jest pdf
                            pdf = null;
                            if (!newDok && dokOdebr_id > 0)
                            {
                                pdf = (from z in lexena.PdfStore
                                       where z.DokOdebr_Id == dokOdebr_id
                                       select z).FirstOrDefault();
                            }

                            if (pdf == null)
                            {

                                pdf = new PdfStore();
                                if (dood.PdfStore == null)
                                    dood.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                                dood.PdfStore.Add(pdf);

                            }

                            pdf.name = "Nakaz zapłaty " + SygnWgPowoda;
                            retvalue = XML2HTMLTransform.html2pdf(dood.TrescHtml, ref pdffile);
                            if (retvalue.Contains("Błąd"))
                            {
                                this.addError(ErrLevel.Error, -720, "Wyjątek w trakcie tworzenia pdf'a Nakazu  ", SygnWgPowoda);
                                Utils.LogWriter("Wyjątek w trakcie tworzenia pdf'a nakazu" + retvalue + " " + SygnWgPowoda);
                                iserror = true;
                                continue;
                            }
                            pdf.value = pdffile;
                            if (dood.DokumentKomunikacjaEPU == null)
                                dood.DokumentKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentKomunikacjaEPU>();

                            DokumentKomunikacjaEPU dokwyskom = new DokumentKomunikacjaEPU();
                            dokwyskom.czyus = 0;
                            dokwyskom.d_kreacji = DateTime.Now;
                            dokwyskom.Status = 3;
                            dood.DokumentKomunikacjaEPU.Add(dokwyskom);


                            // dodanie statusu do sprawy 
                            spr = (from z in lexena.Sprawa
                                   where z.id == idSprawy
                                   select z).FirstOrDefault();
                            nazstat = (from x in lexena.NazwaStatusu
                                       where x.Krok == 4
                                       select x).FirstOrDefault();
                            oststat = (from x in lexena.NazwaStatusu
                                       join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                       where y.Sprawa_id == idSprawy
                                       orderby x.Krok descending
                                       select x).FirstOrDefault();


                            if (spr != null && oststat.Krok <= 3)
                            {

                                StatusSprawy stspr = new StatusSprawy();
                                stspr.czyus = 0;
                                stspr.CzyWiena = 0;
                                stspr.DataStatusu = DateTime.Now;
                                stspr.Sprawa_id = spr.id;
                                stspr.NazwaStatusu_Id = nazstat.Id;
                                lexena.AddToStatusSprawy(stspr);
                            }
                            else
                            {
                                Utils.LogWriter("Sprawa ma już wyższy status " + oststat.Krok.ToString() + " " + SygnWgPowoda);
                                this.addError(ErrLevel.Warning, -721, "Sprawa ma już wyższy status " + oststat.Krok.ToString() + " sprawdź - opłaty i stan sprawy ", SygnWgPowoda);
                                //continue;

                            }

                            if (newDok)
                                lexena.DokOdebr.AddObject(dood);
                            // do wieny
                            {
                                dok_odebr dok_odebrany = null;
                                pws_new = false;
                                dok_odebrany = wiena.dok_odebr.Where(a => a.id_sprawy == wiena_id && a.d_dok == dood.DataDokumentu && a.id_dok_typ == 24 && a.czyus == 0).FirstOrDefault();
                                if (dok_odebrany == null)
                                {
                                    dok_odebrany = new dok_odebr();
                                    pws_new = true;
                                }
                                dok_odebrany.data_r = dood.DataRejestracji.Value;
                                dok_odebrany.id_sprawy = wiena_id;
                                dok_odebrany.rok = (short)dood.DataDokumentu.Value.Year;
                                dok_odebrany.id_dok_typ = 24;
                                dok_odebrany.id_dok = 14457;
                                dok_odebrany.czyzalatw = 0;
                                if (pws_new)
                                    dok_odebrany.d_kreacji = DateTime.Now;
                                else
                                    dok_odebrany.d_modyfikacji = DateTime.Now;
                                dok_odebrany.czyus = 0;
                                dok_odebrany.d_dok = dood.DataDokumentu;


                                if (pws_new)
                                {
                                    wiena.dok_odebr.Add(dok_odebrany);
                                }
                                wiena.SaveChanges();


                                pws_new = false;
                                skan sk = null;
                                sk = wiena.skan.Where(a => a.id_dok_odebr == dok_odebrany.ident).FirstOrDefault();

                                if (pdf != null)
                                {
                                    pws_new = true;
                                    sk = new skan();
                                    sk.d_kreacji = DateTime.Now;
                                }
                                else
                                    sk.d_modyfikacji = DateTime.Now;
                                sk.id_dok_odebr = dok_odebrany.ident;
                                sk.typ = ".PDF";
                                sk.kolejnosc = 1;
                                sk.skan1 = pdffile;
                                if (pws_new)
                                {
                                    wiena.skan.Add(sk);
                                }

                                pws_new = false;
                                // czy są koszty sądowe w takiej wysokości
                                naleznosc nalx = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 8 && a.data_n == dood.DataDokumentu).FirstOrDefault();
                                if (nalx == null)
                                {
                                    pws_new = true;
                                    nalx = new naleznosc();
                                    nalx.d_kreacji = DateTime.Now;
                                    nalx.id_sprawy = wiena_id;
                                }
                                else
                                    nalx.d_modyfikacji = DateTime.Now;
                                nalx.czy_proc = false;
                                nalx.id_typ_nal = 8;
                                nalx.data_n = dood.DataDokumentu;
                                nalx.kwota = ks + prow;

                                if (pws_new)
                                {
                                    wiena.naleznosc.Add(nalx);
                                }
                                else if (skipNewUpdates)
                                    nalx = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 8 && a.data_n == dood.DataDokumentu).FirstOrDefault();


                                // koszty zast

                                pws_new = false;
                                // czy są koszty sądowe w takiej wysokości
                                naleznosc nalx1 = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 12 && a.data_n == dood.DataDokumentu).FirstOrDefault();
                                if (nalx1 == null)
                                {
                                    pws_new = true;
                                    nalx1 = new naleznosc();
                                    nalx1.d_kreacji = DateTime.Now;
                                    nalx1.id_sprawy = wiena_id;
                                }
                                else
                                    nalx1.d_modyfikacji = DateTime.Now;
                                nalx1.czy_proc = false;
                                nalx1.id_typ_nal = 12;
                                nalx1.data_n = dood.DataDokumentu;
                                nalx1.kwota = kzp;

                                if (pws_new)
                                {
                                    wiena.naleznosc.Add(nalx1);
                                }
                                else if (skipNewUpdates)
                                    nalx1 = wiena.naleznosc.Where(a => a.id_sprawy == wiena_id && a.id_typ_nal == 12 && a.data_n == dood.DataDokumentu).FirstOrDefault();

                                spr_sadowa ss = wiena.spr_sadowa.Where(a => a.id_sprawa == wiena_id && a.aktywne == 1).OrderByDescending(a => a.ident).FirstOrDefault();
                                if (ss != null)
                                {
                                    ss.data_otrz_nakaz = dood.DataRejestracji;
                                    ss.data_nakazu = dood.DataDokumentu;
                                    ss.d_modyfikacji = DateTime.Now;
                                    ss.sygn_nakazowe = nakaz.sygnaturaSprawy;


                                }
                                pws_new = false;
                                status_spr sspr = wiena.status_spr.Where(a => a.id_sprawy == wiena_id && a.id_statusu == 41 && a.czyus == 0 && a.od_dnia == dood.DataRejestracji && a.id_statusu == 41).FirstOrDefault();
                                status_spr ost_sspr = wiena.status_spr.Where(a => a.id_sprawy == wiena_id && a.czyus == 0).OrderByDescending(a => a.od_dnia).ThenByDescending(a => a.ident).FirstOrDefault();
                                if ((ost_sspr == null || (ost_sspr != null && ost_sspr.id_statusu != 9 && ost_sspr.id_statusu != 11 && ost_sspr.id_statusu != 108)))
                                {
                                    if (sspr == null)
                                    {
                                        pws_new = true;
                                        sspr = new status_spr();
                                        sspr.d_kreacji = DateTime.Now;

                                    }
                                    else
                                        sspr.d_modyfikacji = DateTime.Now;
                                    sspr.id_sprawy = wiena_id;
                                    sspr.id_statusu = 41;
                                    sspr.od_dnia = dood.DataRejestracji.Value;
                                    sspr.czyus = 0;
                                }
                                if (pws_new)
                                {
                                    wiena.status_spr.Add(sspr);


                                }

                                dood.DDoWieny = DateTime.Now;
                            }
                            lexena.SaveChanges();
                            Utils.LogWriter("Zapisano do LexEny nakaz " + spr.sygnatura);
                            wiena.SaveChanges();
                            Utils.LogWriter("Zapisano do wieny nakaz " + spr.sygnatura);


                            this.addError(theError.level, theError.code, theError.description, theError.reference);
                            Utils.LogWriter(" Import OK:  " + SygnWgPowoda);

                        }



                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania nakazu " + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : "") + " zbiór" + this.importFileName);
                this.addError(ErrLevel.Fatal, -703, "Błąd podczas importu Nakazu " + ex.Message + ((ex.InnerException != null) ? ex.InnerException.Message : ""), SygnWgPowoda);
                return false;

            }



        }


        private int getAllZaleg(wiena_centralEntities wiena, int wiena_id, out decimal winien, out decimal ma, out decimal energia, out decimal wdz, out decimal odsetki, out decimal inne, out decimal vat, out decimal kza, out decimal kl_adw, out decimal ods_kapital)
        {


            var obr = (from o in wiena.obroty
                       join ka in wiena.konto_anal on o.id_konta equals ka.ident
                       join k in wiena.konto on ka.id_konta equals k.id
                       where k.typ == 1 && o.czyus == 0 && ka.czyus == 0 && ka.id_sprawy == wiena_id
                       orderby ka.ident descending
                       group o by o.id_konta into s
                       select new
                       {
                           winien = s.Sum(a => a.winien),
                           ma = s.Sum(a => a.ma),
                           energia = s.Sum(a => a.energia),
                           wdz = s.Sum(a => a.wdz),
                           odsetki = s.Sum(a => a.odsetki),
                           inne = s.Sum(a => a.inne),
                           vat = s.Sum(a => a.vat),
                           kza = s.Sum(a => a.kza),
                           kl_adw = s.Sum(a => a.kl_adw),
                           ods_kapital = s.Sum(a => a.ods_kapital),
                           id_konto_anal = s.Max(a => a.konto_anal.ident)
                       }).FirstOrDefault();

            if (obr == null)
            {
                winien = 0;
                ma = 0;
                energia = 0;
                wdz = 0;
                odsetki = 0;
                inne = 0;
                vat = 0;
                kza = 0;
                kl_adw = 0;
                ods_kapital = 0;
                return 0;
            }
            winien = obr.winien ?? 0;
            ma = obr.ma ?? 0;
            energia = obr.energia ?? 0;
            wdz = obr.wdz ?? 0;
            odsetki = obr.odsetki ?? 0;
            inne = obr.inne ?? 0;
            vat = obr.vat ?? 0;
            kza = obr.kza ?? 0;
            kl_adw = obr.kl_adw ?? 0;
            ods_kapital = obr.ods_kapital ?? 0;
            return obr.id_konto_anal;
        }

        private decimal getZalegbyTyp(int typ_kwt, wiena_centralEntities wiena, int wiena_id)
        {
            if (typ_kwt == 0)
            {// wszystkie koszty 

                decimal? kwt_nal = 0, kwt_spl = 0;
                kwt_nal = (from n in wiena.naleznosc
                           join tn in wiena.typ_nal on n.id_typ_nal equals tn.ident
                           where n.id_sprawy == wiena_id && tn.czy_mem == false 
                           select n.kwota
                         ).Sum();

                kwt_spl = (from n in wiena.naleznosc
                           join tn in wiena.typ_nal on n.id_typ_nal equals tn.ident
                           join wp in wiena.wplata_podz on n.ident equals wp.id_nal
                           where n.id_sprawy == wiena_id && tn.czy_mem == false
                           select wp.spl_kap
                         ).Sum();
                if (kwt_nal == null) return 0;
                if (kwt_spl == null) kwt_spl = 0;
                return kwt_nal - kwt_spl < 0 ? 0 : kwt_nal.Value - kwt_spl.Value;

                /*
                if (kwt_nal == null) return 0;
                if (kwt_spl == null) kwt_spl = 0;
                return kwt_nal - kwt_spl < 0 ? 0 : kwt_nal.Value - kwt_spl.Value;


                decimal? kosztyall = 0;
                decimal? kwt = 0;
                List<int> identy = new List<int>();



                nal_stan nals = (from k in wiena.nal_stan
                                 join n in wiena.naleznosc on k.id_nal equals n.ident
                                 join tn in wiena.typ_nal on n.id_typ_nal equals tn.ident
                                 where n.id_sprawy == wiena_id && tn.czy_mem == false
                                 orderby k.data_s descending
                                 select k).FirstOrDefault();


                if (nals != null)
                {

                    kosztyall = (from k in wiena.nal_stan
                                 join n in wiena.naleznosc on k.id_nal equals n.ident
                                 join tn in wiena.typ_nal on n.id_typ_nal equals tn.ident
                                 where n.id_sprawy == wiena_id && tn.czy_mem == false && k.data_s == nals.data_s

                                 select k.kwota_k
                               ).Sum();

                    identy = (from k in wiena.nal_stan
                              join n in wiena.naleznosc on k.id_nal equals n.ident
                              join tn in wiena.typ_nal on n.id_typ_nal equals tn.ident
                              where n.id_sprawy == wiena_id && tn.czy_mem == false && k.data_s == nals.data_s

                              select n.ident
                                                   ).ToList();

                    //if (kosztyall != null && kosztyall > 0 )
                    //      return kosztyall.Value;
                }
               
                if (identy == null) identy = new List<int>();

                if (kosztyall == null) kosztyall = 0;


                kwt = (from n in wiena.naleznosc
                       join tn in wiena.typ_nal on n.id_typ_nal equals tn.ident
                       where n.id_sprawy == wiena_id && tn.czy_mem == false &&
                             !identy.Contains(n.ident)

                       select n.kwota
                        ).Sum();

                if (kwt == null)
                    kwt = 0;
                return kwt.Value + kosztyall.Value;

                */

            }

            else
            {

                decimal? kwt_nal = 0, kwt_spl = 0;
                kwt_nal = (from n in wiena.naleznosc
                join tn in wiena.typ_nal on n.id_typ_nal equals tn.ident
                where n.id_sprawy == wiena_id && tn.czy_mem == false &&
                        tn.ident == typ_kwt
                           select n.kwota
                         ).Sum();

                kwt_spl = (from n in wiena.naleznosc
                           join tn in wiena.typ_nal on n.id_typ_nal equals tn.ident
                           join wp in wiena.wplata_podz on n.ident equals wp.id_nal
                           where n.id_sprawy == wiena_id && tn.czy_mem == false &&
                                   tn.ident == typ_kwt
                           select wp.spl_kap
                         ).Sum();

                if (kwt_nal == null) return 0;
                if (kwt_spl == null) kwt_spl = 0;
                return kwt_nal - kwt_spl < 0 ? 0 : kwt_nal.Value - kwt_spl.Value;
                /*
                nal_stan stan_kwt = (from k in wiena.nal_stan
                                     join n in wiena.naleznosc on k.id_nal equals n.ident
                                     join tn in wiena.typ_nal on n.id_typ_nal equals tn.ident
                                     where n.id_sprawy == wiena_id && tn.czy_mem == false && tn.ident == typ_kwt
                                     orderby k.data_s descending
                                     select k
                              ).FirstOrDefault();
                if (stan_kwt != null)
                    return stan_kwt.kwota_k.Value;
                else
                {
                    decimal? kwt = (from n in wiena.naleznosc
                                    join tn in wiena.typ_nal on n.id_typ_nal equals tn.ident
                                    where n.id_sprawy == wiena_id && tn.czy_mem == false && tn.ident == typ_kwt

                                    select n.kwota
                             ).Sum();

                    if (kwt == null)
                        return 0;
                    else
                        return kwt.Value;

                }
                */
            }
        }







        public bool ValidateKlauzula()
        {
            MojeOrzeczeniaOutputData orzeczenia;
            OrzeczenieOutputElement[] lista_orzeczen;


            int kodstat;
            String sygnatura;
            String SygnWgPowoda;
            NazwaStatusu oststat;
            int idSprawy;
            byte[] pdffile;
            NazwaStatusu nazstat;
            pdffile = new byte[0];
            string mylogname;
            Sprawa spr = null;
            string nakazStr;
            bool newDok;
            int dokOdebr_id;
            pdffile = new byte[0];
            PdfStore pdf;
            string retvalue;
            Utils.LogWriter("Walidacja spraw ze zbioru : " + importFileName);
            bool skipNewpdates = false;
            bool pws_new = false;
            //  Utils.LogNamedWriter(" Start Importu zbioru: " + fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + "raport.txt");

            SygnWgPowoda = "";

            if (typDok == DocType.Klauzula1)

            {
                if (this.paczkapostanowien1 == null || !this.paczkapostanowien1.OrzeczenieEPU.Any())
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie deserializacji zpraw  zbiór:" + importFileName + " jest pusty");
                    return true;
                }
            }
            else // Kla
            {
                if (this.paczkapostanowien2 == null || !this.paczkapostanowien2.listaOrzeczen.Any())
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie deserializacji zpraw  zbiór:" + importFileName + " jest pusty");
                    return true;
                }

            }

            Utils.LogWriter(" Start walidacji zbioru: " + importFileName);
            SygnWgPowoda = "";
            try
            {


                if (typDok == DocType.Klauzula2)
                {
                    lista_orzeczen = this.paczkapostanowien2.listaOrzeczen;
                }
                else
                {

                    int i = 0;
                    lista_orzeczen = new OrzeczenieOutputElement[this.paczkapostanowien1.OrzeczenieEPU.GetUpperBound(0)];// mojesprawy.listaSpraw;


                    for (i = 0; i <= this.paczkapostanowien1.OrzeczenieEPU.GetUpperBound(0); i++)
                    {
                        lista_orzeczen[i] = new OrzeczenieOutputElement();
                        /*
                        lista_orzeczen[i].dataOrzeczenia = this.paczkapostanowien1.OrzeczenieEPU[i].dataOrzeczenia.ToString();
                        listaspraw[i] = new SprawaOutputElement();
                        listaspraw[i].id = myspr.Id;
                        listaspraw[i].kwotaSporu = myspr.KwotaSporu;
                        listaspraw[i].rolaWSprawie = myspr.RolaWSprawie;
                        listaspraw[i].stanSprawy = myspr.StanSprawy;
                        listaspraw[i].sygnaturaSprawy = myspr.SygnaturaSprawy;
                        listaspraw[i].sygnaturaWgPowoda = myspr.SygnaturaWgPowoda;
                   */
                    }

                }
                using (wiena_centralEntities wiena = new wiena_centralEntities())
                {
                    using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                    {


                        bool iserror = false;


                        foreach (OrzeczenieOutputElement orzecz in lista_orzeczen)
                        {
                            nakazStr = orzecz.dokumentXml;
                            SygnWgPowoda = orzecz.sygnaturaWgPowoda;
                            idSprawy = 0;
                            skipNewpdates = false;

                            int wiena_id = 0;
                            errDescription theError = new errDescription();

                            theError.level = ErrLevel.OK;
                            theError.code = 0;
                            theError.description = "";
                            theError.reference = SygnWgPowoda;

                            orzecz.dataOrzeczenia = orzecz.dataOrzeczenia.Date;

                            spr = null;
                            if (!String.IsNullOrEmpty(SygnWgPowoda.Trim()) && !SygnWgPowoda.Trim().StartsWith("ENERGA#") && SygnWgPowoda.Contains("@"))
                            {
                                Utils.LogWriter(" Start Importu orzeczenia: " + SygnWgPowoda + " " + orzecz.sygnaturaSprawy);
                                if (SygnWgPowoda.Contains("#"))
                                    idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                                else
                                {
                                    idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));
                                }
                                // dodanie encji pozew
                                spr = (from z in lexena.Sprawa
                                       where z.id == idSprawy
                                       select z).FirstOrDefault();
                                if (spr == null)
                                {
                                    sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                                    spr = (from z in lexena.Sprawa
                                           where z.sygnatura == sygnatura
                                           select z).FirstOrDefault();


                                }
                            }
                            else if (!String.IsNullOrEmpty(SygnWgPowoda.Trim()))
                            {
                                sygnatura = SygnWgPowoda.Trim();
                                spr = (from z in lexena.Sprawa
                                       where z.sygnatura == sygnatura
                                       select z).FirstOrDefault();

                            }
                            else
                                if (!String.IsNullOrEmpty(orzecz.sygnaturaSprawy))
                            {
                                spr = (from z in lexena.Sprawa
                                       where z.SygnNCe.Replace(" ", "").Contains(orzecz.sygnaturaSprawy.Replace(" ", "").Replace("VI", ""))
                                       select z).FirstOrDefault();
                                if (spr != null) SygnWgPowoda = spr.sygnatura;
                            }


                            if (spr == null)
                            {
                                Utils.LogWriter("Błąd w trakcie wczytywania postanowienia o nad. Klauzuli " + SygnWgPowoda);
                                this.addError(ErrLevel.Error, -502, "brak sprawy o takim oznaczeniu lub oznaczenie sprawy niezgodne ze standardem Energa " + SygnWgPowoda + orzecz.sygnaturaSprawy, SygnWgPowoda);
                                continue;
                            }
                            else
                                idSprawy = spr.id;


                            // sprawdzenie czy jest odnotowany nakaz zapłaty
                            DokOdebr dood = (from z in lexena.DokOdebr
                                             where z.Sprawa_id == idSprawy && z.TypDok == 5
                                             select z).FirstOrDefault();

                            if (dood == null)
                            {
                                Utils.LogWriter("Brak nakazu zapłaty w systemie Lexena " + SygnWgPowoda);
                                this.addError(ErrLevel.Error, -620, "Brak nakazu zapłaty odnotowanego w systemie LexEna. Klauzula nie zostanie zaimportowana " + SygnWgPowoda, SygnWgPowoda);
                                continue;

                            }
                            kodstat = 5;
                            nazstat = (from x in lexena.NazwaStatusu
                                       where x.Krok == kodstat
                                       select x).FirstOrDefault();
                            oststat = (from x in lexena.NazwaStatusu
                                       join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                       where y.Sprawa_id == idSprawy
                                       orderby y.DataStatusu descending
                                       select x).FirstOrDefault();

                            if (oststat.Krok >= kodstat)
                            {


                                Utils.LogWriter("Sprawa ma już wyższy status " + oststat.Krok.ToString() + " " + SygnWgPowoda);
                                this.addError(ErrLevel.Warning, -621, "Sprawa ma już wyższy status " + oststat.Krok.ToString() + " - zapisy w systemie wymagają weryfikacji ", SygnWgPowoda);
                                //continue;

                            }



                            wiena_id = (from x in wiena.sprawa join f in wiena.firma on x.id_firmy equals f.ident where x.sygnatura == spr.sygnatura && f.typ_firmy == FirmaTyp select x.ident).FirstOrDefault();
                            if (wiena_id == 0)
                            {
                                iserror = true;

                                this.addError(ErrLevel.Error, -562, "Brak sprawy w systemie Wiena " + SygnWgPowoda + " w systemie Energii", SygnWgPowoda);
                                lexena.Sprawa.Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, spr);
                                continue;
                            }

                            var obr = (from o in wiena.obroty
                                       join ka in wiena.konto_anal on o.id_konta equals ka.ident
                                       join k in wiena.konto on ka.id_konta equals k.id
                                       where k.typ == 1 && o.czyus == 0 && ka.czyus == 0 && ka.id_sprawy == wiena_id
                                       select o).FirstOrDefault();
                            if (obr == null) // brak księgowań na koncie pozew złożony
                            {
                                Utils.LogWriter("W systemie Wiena brak księgowania na właściwym koncie dokument nie zostanie zaimportowany " + SygnWgPowoda);
                                this.addError(ErrLevel.Error, -563, "W systemie Wiena brak księgowania na właściwym koncie dokument nie zostanie zaimportowany" + SygnWgPowoda, SygnWgPowoda);
                                lexena.Sprawa.Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, spr);
                                continue;

                            }
                            DateTime dOrzecz = orzecz.dataOrzeczenia.Date;

                            dood = (from z in lexena.DokOdebr
                                    where z.Sprawa_id == idSprawy && z.TypDok == 17 && z.Nazwa == orzecz.nazwaDecyzji && z.DataDokumentu == dOrzecz
                                    select z).FirstOrDefault();
                            if (dood == null)
                            {
                                dood = new DokOdebr();
                                newDok = true;
                                dokOdebr_id = 0;
                            }
                            else
                            {
                                dokOdebr_id = dood.Id;
                                newDok = false;
                                dood.d_modyfikacji = DateTime.Now;
                            }
                            dood.d_kreacji = DateTime.Now;
                            dood.DataDokumentu = dOrzecz;
                            dood.DataRejestracji = DateTime.Today;
                            dood.CzyEPU = 1;
                            dood.IdEPU = orzecz.id;
                            dood.Nazwa = orzecz.nazwaDecyzji;
                            dood.TypDok = 17;
                            dood.Sprawa_id = idSprawy;
                            dood.PartitionKey = 0;
                            dood.Format = 100;
                            dood.StatusDok = "wydano";
                            dood.Tresc = orzecz.dokumentXml;
                            dood.TrescHtml = XML2HTMLTransform.TransformNCompress(dood.Tresc, 101);
                            dood.CzyZalatw = 0;
                            if (KontoEpu > 0) dood.KontoEPU_Id = KontoEpu;
                            // czy jest pdf
                            pdf = null;
                            if (!newDok && dokOdebr_id > 0)
                            {
                                pdf = (from z in lexena.PdfStore
                                       where z.DokOdebr_Id == dokOdebr_id
                                       select z).FirstOrDefault();
                            }

                            if (pdf == null)
                            {

                                pdf = new PdfStore();
                                if (dood.PdfStore == null)
                                    dood.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                                dood.PdfStore.Add(pdf);
                                lexena.PdfStore.AddObject(pdf);
                            }
                            pdf.name = orzecz.nazwaDecyzji + " " + SygnWgPowoda;
                            pdf.name = pdf.name.Substring(0, (pdf.name.Length > 100 ? 100 : pdf.name.Length));
                            retvalue = XML2HTMLTransform.html2pdf(dood.TrescHtml, ref pdffile);
                            if (retvalue.Contains("Błąd"))
                            {
                                this.addError(ErrLevel.Error, -620, "Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego  ", SygnWgPowoda);
                                Utils.LogWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda);
                                iserror = true;
                                continue;
                            }
                            pdf.value = pdffile;
                            if (dood.DokumentKomunikacjaEPU == null)
                                dood.DokumentKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentKomunikacjaEPU>();
                            DokumentKomunikacjaEPU dokwyskom = new DokumentKomunikacjaEPU();
                            dokwyskom.czyus = 0;
                            dokwyskom.d_kreacji = DateTime.Now;
                            dokwyskom.Status = 3;
                            dood.DokumentKomunikacjaEPU.Add(dokwyskom);
                            //lexena.AddToDokumentKomunikacjaEPU(dokwyskom);
                            kodstat = 5;

                            // dodanie statusu do sprawy 
                            spr = (from z in lexena.Sprawa
                                   where z.id == idSprawy
                                   select z).FirstOrDefault();
                            nazstat = (from x in lexena.NazwaStatusu
                                       where x.Krok == kodstat
                                       select x).FirstOrDefault();
                            oststat = (from x in lexena.NazwaStatusu
                                       join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                       where y.Sprawa_id == idSprawy
                                       orderby y.DataStatusu descending
                                       select x).FirstOrDefault();

                            if (spr != null && oststat.Krok < kodstat)
                            {

                                StatusSprawy stspr = new StatusSprawy();
                                stspr.czyus = 0;
                                stspr.CzyWiena = 0;
                                stspr.DataStatusu = DateTime.Now;
                                stspr.Sprawa_id = spr.id;
                                stspr.NazwaStatusu_Id = nazstat.Id;
                                lexena.AddToStatusSprawy(stspr);
                            }
                            else
                            {


                                Utils.LogWriter("Sprawa ma już wyższy status " + oststat.Krok.ToString() + " " + SygnWgPowoda);
                                this.addError(ErrLevel.Warning, -621, "Sprawa ma już wyższy status " + oststat.Krok.ToString() + " - wymagana jest weryfikacja zapisów ", SygnWgPowoda);
                                skipNewpdates = true;
                                //continue;

                            }
                            // do wieny
                            {
                                dok_odebr dok_odebrany = null;
                                pws_new = false;
                                dok_odebrany = wiena.dok_odebr.Where(a => a.id_sprawy == wiena_id && a.d_dok == dood.DataDokumentu && a.id_dok_typ == 25 && a.czyus == 0).FirstOrDefault();
                                if (dok_odebrany == null)
                                {
                                    dok_odebrany = new dok_odebr();
                                    pws_new = true;
                                }
                                dok_odebrany.data_r = dood.DataRejestracji.Value.Date;
                                dok_odebrany.id_sprawy = wiena_id;
                                dok_odebrany.rok = (short)dood.DataDokumentu.Value.Year;
                                dok_odebrany.id_dok_typ = 25;
                                dok_odebrany.id_dok = 14458;
                                dok_odebrany.czyzalatw = 0;
                                if (pws_new)
                                    dok_odebrany.d_kreacji = DateTime.Now;
                                else
                                    dok_odebrany.d_modyfikacji = DateTime.Now;
                                dok_odebrany.czyus = 0;
                                dok_odebrany.d_dok = dood.DataDokumentu.Value.Date;

                                if (pws_new)
                                {
                                    wiena.dok_odebr.Add(dok_odebrany);
                                }
                                wiena.SaveChanges();
                                pws_new = false;
                                skan sk = null;
                                sk = wiena.skan.Where(a => a.id_dok_odebr == dok_odebrany.ident).FirstOrDefault();

                                if (sk == null)
                                {
                                    pws_new = true;
                                    sk = new skan();
                                    sk.d_kreacji = DateTime.Now;
                                }
                                else
                                    sk.d_modyfikacji = DateTime.Now;
                                sk.id_dok_odebr = dok_odebrany.ident;
                                sk.typ = ".PDF";
                                sk.kolejnosc = 1;
                                sk.skan1 = pdffile;
                                if (pws_new)
                                {
                                    wiena.skan.Add(sk);
                                }


                                if (!skipNewpdates)
                                {
                                    decimal wpis = getZalegbyTyp(8, wiena, wiena_id);
                                    decimal kzp = getZalegbyTyp(12, wiena, wiena_id);
                                    decimal klauzula = getZalegbyTyp(9, wiena, wiena_id);
                                    decimal kosztyall = getZalegbyTyp(0, wiena, wiena_id);
                                    decimal koszty_inne = kosztyall - wpis - kzp - klauzula;
                                    Utils.LogWriter(wpis.ToString() + " " + kzp.ToString() + " " + klauzula.ToString() + " " + kosztyall.ToString() + " " + koszty_inne.ToString());
                                    decimal winien;
                                    decimal ma;
                                    decimal energia;
                                    decimal wdz;
                                    decimal odsetki;
                                    decimal inne;
                                    decimal vat;
                                    decimal kza;
                                    decimal kl_adw;
                                    decimal ods_kapital;

                                    int id_konta_anal = getAllZaleg(wiena, wiena_id, out winien, out ma, out energia, out wdz, out odsetki, out inne, out vat, out kza, out kl_adw, out ods_kapital);
                                    pws_new = false;
                                    // zlecenie księgowe
                                    zlecenia_ksiegowe zlc = null;
                                    bool zlc_new = false;
                                    zlc = wiena.zlecenia_ksiegowe.Where(a => a.id_sprawy == wiena_id && a.konto == 13 && a.nr_dowodu == "Uz4" && a.d_ksiegowania == null && a.d_zlecenia == dood.DataRejestracji).FirstOrDefault();
                                    if (zlc == null)
                                    {
                                        zlc = new zlecenia_ksiegowe();
                                        zlc_new = true;
                                    }
                                    zlc.czyus = 0;
                                    zlc.d_kreacji = DateTime.Now;
                                    zlc.kod = 1;
                                    zlc.konto = 13;

                                    zlc.ma = 0;
                                    zlc.energia = energia;
                                    zlc.koszty = kosztyall + kza + kl_adw;
                                    zlc.odsetki = odsetki;
                                    zlc.inne = inne;
                                    zlc.winien = zlc.energia + zlc.odsetki + zlc.koszty + zlc.inne;
                                    zlc.vat = vat;
                                    zlc.nr_dowodu = "Uz4";
                                    zlc.rodzaj = 0;
                                    zlc.d_zlecenia = dood.DataRejestracji.Value.Date;
                                    zlc.id_sprawy = wiena_id;
                                    zlc.id_obroty = 0;
                                    zlc.wpis = wpis;
                                    zlc.kzp = kzp;
                                    zlc.klauzula = klauzula;
                                    zlc.kza = kza;
                                    zlc.kl_adw = kl_adw;
                                    zlc.komo = 0;
                                    zlc.inne_sad = koszty_inne;
                                    zlc.ods_kapital = ods_kapital;
                                    zlc.id_schemat = 17;
                                    zlc.konto_anal = id_konta_anal;
                                    zlc.wdz = wdz;

                                    if (zlc_new) wiena.zlecenia_ksiegowe.Add(zlc);
                                }
                                spr_sadowa ss = wiena.spr_sadowa.Where(a => a.id_sprawa == wiena_id && a.aktywne == 1).OrderByDescending(a => a.ident).FirstOrDefault();
                                if (ss != null)
                                {

                                    ss.data_wezw_braki = dood.DataRejestracji.Value.Date;
                                    ss.data_klauzuli = dood.DataDokumentu.Value.Date;
                                    ss.d_modyfikacji = DateTime.Now;
                                    if (string.IsNullOrWhiteSpace(ss.sygn_nakazowe))
                                        ss.sygn_nakazowe = orzecz.sygnaturaSprawy;


                                }

                                pws_new = false;
                                status_spr sspr = wiena.status_spr.Where(a => a.id_sprawy == wiena_id && a.id_statusu == 41 && a.czyus == 0 && a.od_dnia == dood.DataRejestracji && a.id_statusu == 41).FirstOrDefault();
                                status_spr ost_sspr = wiena.status_spr.Where(a => a.id_sprawy == wiena_id && a.czyus == 0).OrderByDescending(a => a.od_dnia).ThenByDescending(a => a.ident).FirstOrDefault();

                                if ((ost_sspr == null || (ost_sspr != null && ost_sspr.id_statusu != 9 && ost_sspr.id_statusu != 11 && ost_sspr.id_statusu != 108)))
                                {
                                    if (sspr == null)
                                    {
                                        pws_new = true;
                                        sspr = new status_spr();
                                        sspr.d_kreacji = DateTime.Now;
                                    }
                                    else
                                        sspr.d_modyfikacji = DateTime.Now;

                                    sspr.id_sprawy = wiena_id;
                                    sspr.id_statusu = 23;
                                    sspr.od_dnia = dood.DataRejestracji.Value.Date;
                                    sspr.czyus = 0;
                                }
                                if (pws_new)
                                {

                                    wiena.status_spr.Add(sspr);


                                }
                                dood.DDoWieny = DateTime.Now;
                            }

                            lexena.SaveChanges();
                            Utils.LogWriter("Zapisano w LexEna");
                            wiena.SaveChanges();
                            Utils.LogWriter(" Import OK:  " + SygnWgPowoda);
                            this.addError(theError.level, theError.code, theError.description, theError.reference);
                        }

                    }
                }

                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        //raise a new exception inserting the current one as the InnerException
                        Utils.LogWriter(message);

                    }
                }
                return false;
            }

            catch (DbUpdateException e)
            {
                var sqlException = e.GetBaseException() as SqlException;

                Utils.LogWriter(sqlException.Message + ";" + sqlException.Data + (sqlException.InnerException != null ? sqlException.InnerException.Message : ""));
                return false;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania post. o nad klauzuli " + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : "") + " zbiór" + this.importFileName);
                this.addError(ErrLevel.Fatal, -503, "Błąd podczas importu klauzul " + ex.Message + ((ex.InnerException != null) ? ex.InnerException.Message : ""), SygnWgPowoda);
                return false;

            }



        }
        private string[,] mapping = {{"07-010-187001000101",    "AC-110/2020"} }
               ;


        public bool ValidatePozew()
        {

            PozewEPU[] lista_pozwow;

            String SygnWgPowoda;
            int idSprawy;
            string sygnatura;
            PdfStore pdf;
            byte[] pdffile;
            Sprawa spr;
            pdffile = new byte[0];
            bool iserror = false;
            bool newPozew = false;
            decimal Noty = 0;
            decimal WDZ = 0;
            decimal odsNal = 0;
            string pozewStr;
            string retvalue;
            NazwaStatusu nazstat = null;
            Pozew pozewDB;
            Utils.LogWriter("Walidacja pozwów ze zbioru : " + importFileName);
            //  Utils.LogNamedWriter(" Start Importu zbioru: " + fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + "raport.txt");

            SygnWgPowoda = "";
            try
            {

                if (this.paczkapozwow == null || !this.paczkapozwow.PozewEPU.Any())
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie deserializacji Pozwu  zbiór:" + importFileName + " jest pusty");
                    return true;
                }



                lista_pozwow = this.paczkapozwow.PozewEPU;

                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())

                {

                    foreach (PozewEPU pozew in lista_pozwow)
                    {
                        string OrygOzn;
                        pdf = null;
                        int dokWys_id = 0;
                        pozewDB = null;
                        retvalue = string.Empty;
                        newPozew = false;
                        pozewStr = String.Empty;

                        nazstat = null;
                        bool sygnOK = true;
                        SygnWgPowoda = pozew.SprawaWgPowoda;
                        errDescription theError = new errDescription();
                        theError.level = ErrLevel.OK;
                        theError.code = 0;
                        theError.description = "";

                        pozewStr = SerializeToString(pozew, typeof(PozewEPU), true);
                        // Utils.LogNamedWriter(SygnWgPowoda, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + "raport.txt");
                        // continue;

                        OrygOzn = SygnWgPowoda;
                        theError.reference = SygnWgPowoda;
                        if (SygnWgPowoda.Trim().StartsWith("ENERGA#"))
                        {
                            string sygn;
                            sygnOK = false;
                            Regex r = new Regex(@"posługiwanie się w korespondencji nr sygnatury [a-zA-Z]{1,2}-\d{1,6}/\d{4}");
                            Match m = r.Match(pozew.Uzasadnienie);
                            if (m.Success)
                            {
                                sygn = m.Value.Replace("posługiwanie się w korespondencji nr sygnatury ", "");
                                SygnWgPowoda = sygn + "@0";
                                sygnOK = true;
                            }
                            else
                            {
                                // szukanie po fakturze 
                                foreach (typDowod dow in pozew.ListaDowodow)
                                {
                                    if (dow.Oznaczenie != null)
                                    {
                                        Naleznosc nalx = (from z in lexena.Naleznosc
                                                          where z.opis == dow.Oznaczenie
                                                          orderby z.Id descending
                                                          select z).FirstOrDefault();
                                        if (nalx != null)
                                        {
                                            SygnWgPowoda = nalx.Sprawa.sygnatura + "@0";
                                            sygnOK = true;
                                            break;
                                        }
                                    }


                                }



                            }
                            if (sygnOK == false)
                            {
                                Utils.LogWriter(" Błąd importu pozwu : " + SygnWgPowoda + " nie można ustalić sygnatury sprawy ");
                                this.addError(ErrLevel.Error, -100, "Brak w pozwie oznaczenia sprawy w systemie Energii ", SygnWgPowoda);
                                iserror = true;
                                continue;
                            }
                            theError.level = ErrLevel.Warning;
                            theError.code = 100;
                            theError.description = " Brak oznaczenia sprawy Energii w sygnaturze powoda";
                            theError.reference = SygnWgPowoda;
                        }
                        Utils.LogWriter(" Start Importu pozwu: " + SygnWgPowoda);
                        idSprawy = 0;
                        if (SygnWgPowoda.Contains("@"))
                        {
                            if (SygnWgPowoda.Contains("#"))
                                idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                            else
                                idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));
                            // dodanie encji pozew 
                        }
                        else if (SygnWgPowoda.Contains("#") && SygnWgPowoda.IndexOf('#') < SygnWgPowoda.Length)
                        {
                            string sn = SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('#') + 1).Trim();
                            spr = (from z in lexena.Sprawa
                                   where z.sygnatura == sn
                                   select z).FirstOrDefault();
                            if (spr != null)
                                idSprawy = spr.id;

                        }
                        else
                        {
                            bool found = false;
                            string sygnat = string.Empty;
                            for (int i = 0; i <= mapping.GetUpperBound(0); i++)
                            {
                                idSprawy = 0;
                                if (SygnWgPowoda == mapping[i, 0])
                                {
                                    found = true;
                                    sygnat = mapping[i, 1];
                                    break;
                                }
                            }

                            if (found)
                            {
                                spr = (from z in lexena.Sprawa
                                       where z.sygnatura == sygnat
                                       select z).FirstOrDefault();
                                if (spr != null)
                                    idSprawy = spr.id;
                            }

                        }

                        spr = (from z in lexena.Sprawa
                               where z.id == idSprawy
                               select z).FirstOrDefault();

                        if (spr == null)
                        {
                            if (SygnWgPowoda.IndexOf('@') > 0)
                                sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                            else
                                sygnatura = SygnWgPowoda;

                            spr = (from z in lexena.Sprawa
                                   where z.sygnatura == sygnatura
                                   orderby z.id descending
                                   select z).FirstOrDefault();

                        }

                        if (spr == null)
                        {
                            Utils.LogWriter("Błąd w trakcie wczytywania Pozwu  " + SygnWgPowoda + " nie istnieje w Lexena, zbiór " + this.importFileName);

                            //File.Move(fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname) + ".ERR");
                            //return -100;
                            iserror = true;
                            this.addError(ErrLevel.Error, -502, "Brak sprawy " + SygnWgPowoda + " w systemie Energii", SygnWgPowoda);
                            continue;
                        }
                        else idSprawy = spr.id;

                        DokWys dw = (from z in lexena.DokWys.Include("Pozew")
                                     where z.Sprawa_id == idSprawy && z.DataDok == pozew.DataZlozenia
                                     select z).FirstOrDefault();
                        if (dw == null)
                        {
                            dw = new DokWys();
                            newPozew = true;
                            Utils.LogWriter("Tworzę nowy pozew  dla " + SygnWgPowoda);
                        }
                        else
                        {
                            dokWys_id = dw.Id;
                        }
                        dw.d_kreacji = DateTime.Today;
                        dw.DataDok = pozew.DataZlozenia;
                        dw.DataOdbioru = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));

                        dw.InneKoszty = pozew.InneKoszty == null ? 0 : pozew.InneKoszty.Wartosc;
                        dw.Koszty = pozew.OplataSadowa == null ? 0 : pozew.OplataSadowa.WartoscOplaty;

                        if (pozew.KosztyZastepstwa != null)
                        {
                            if (pozew.KosztyZastepstwa.Zasadzenie == 1)
                                if (pozew.KosztyZastepstwa.WgNorm == 1)
                                    dw.Kzp = EPUCalc.countKZP(pozew.WartoscSporu);
                                else
                                    dw.Kzp = pozew.KosztyZastepstwa.Wartosc;
                        }
                        dw.Nazwa = "Pozew";

                        if (KontoEpu > 0) dw.KontoEPU_Id = KontoEpu;

                        Regex rAUMS = new Regex(@"^[A-Z]\d{8}$");
                        Regex rSelen = new Regex(@".+\/WDZ$");
                        Regex rCCnB = new Regex(@"^\d{9}\/\d{4}$");
                        Regex rCCnBnew = new Regex(@".+\/NWZ\/\d{5}$");

                        Regex rCCnBNota = new Regex(@".+\/NOT\/\d{5}$");

                        int[] arr = new int[200];
                        int[] arrNoty = new int[200];

                        int index = 0;
                        int indexNoty = 0;

                        foreach (typDowod td in pozew.ListaDowodow)
                        {
                            if (String.IsNullOrWhiteSpace(td.Oznaczenie)) continue;

                            string dOpis = td.Oznaczenie.Trim();

                            Match m = rAUMS.Match(dOpis);
                            if (!m.Success)
                            {
                                m = rSelen.Match(dOpis);
                                if (!m.Success)
                                {
                                    m = rCCnB.Match(dOpis);
                                    if (!m.Success)
                                        m = rCCnBnew.Match(dOpis);
                                }
                            }
                            if (m.Success)
                                arr[index++] = td.Numer;
                        }


                        foreach (typDowod td in pozew.ListaDowodow)
                        {
                            if (String.IsNullOrWhiteSpace(td.Oznaczenie)) continue;

                            string dOpis = td.Oznaczenie.Trim();

                            Match m = rCCnBNota.Match(dOpis);

                            if (m.Success)
                                arrNoty[indexNoty++] = td.Numer;

                        }



                        Noty = 0;
                        WDZ = 0;
                        odsNal = 0;

                        foreach (typRoszczenie roszcz in pozew.ListaRoszczen)
                        {
                            if (roszcz.Odsetki != null)
                            {
                                if (roszcz.Odsetki.Count == 1)
                                {
                                    // if (roszcz.Odsetki[0].Od_Wniesienia == 1 && roszcz.Odsetki[0].DataOd == Convert.ToDateTime("2000-01-01"))
                                    //if (roszcz.Odsetki[0].Od_Wniesienia == 1 || (roszcz.Opis.ToUpper().Contains("NOT") && roszcz.Opis.ToUpper().Contains("ODSETK"))) 
                                    if ((roszcz.Opis.ToUpper().Contains("NOTA") && roszcz.Opis.ToUpper().Contains("ODSETK")))
                                        Noty += roszcz.Wartosc;


                                }


                            }

                            // szukamy WDZ
                            Match m = rAUMS.Match(roszcz.Opis);
                            if (!m.Success)
                            {
                                m = rSelen.Match(roszcz.Opis);
                                if (!m.Success)
                                {
                                    m = rCCnB.Match(roszcz.Opis);
                                    if (!m.Success)
                                        m = rCCnBnew.Match(roszcz.Opis);
                                }
                            }
                            if (m.Success || (roszcz.Opis.ToUpper().Contains("WEZWANI") || (roszcz.Odsetki == null || roszcz.Odsetki.Count == 0)))
                                WDZ += roszcz.Wartosc;

                            /*
                            for (int i = 0; i < index; i++)
                                {
                                    if (roszcz.Dowody != null && roszcz.Dowody.Contains(arr[i]))
                                    {
                                        WDZ += roszcz.Wartosc;
                                    }

                                }
                             */
                        }

                        if (Noty == 0)
                        {
                            foreach (typRoszczenie roszcz in pozew.ListaRoszczen)
                            {
                                if (roszcz.Odsetki != null)
                                {
                                    if (roszcz.Odsetki.Count == 1)
                                    {
                                        if (roszcz.Odsetki[0].Od_Wniesienia == 1)
                                        {
                                            // jeśli nie jest WDZ
                                            if (FirmaTyp == 1)
                                            {
                                                bool found = false;
                                                for (int i = 0; i < index; i++)
                                                {
                                                    if (arr[i] == 0) break;
                                                    if ((roszcz.Dowody != null && roszcz.Dowody.Contains(arr[i])) || ((roszcz.Opis.Replace(" ", "").Trim().ToLower() == "dowód" + arr[i].ToString())))
                                                    {
                                                        found = true;
                                                        break;
                                                    }

                                                }
                                                if (!found)
                                                    Noty += roszcz.Wartosc;
                                            }
                                            else
                                                Noty += roszcz.Wartosc;
                                        }

                                    }
                                }
                            }
                        }
                        if (WDZ == 0 && FirmaTyp == 1)
                        {
                            for (int i = 0; i < index; i++)
                            {
                                if (arr[i] == 0) break;
                                foreach (typRoszczenie roszcz in pozew.ListaRoszczen)
                                {
                                    if ((roszcz.Dowody != null && roszcz.Dowody.Contains(arr[i])) || (roszcz.Opis.Replace(" ", "").Trim().ToLower() == "dowód" + arr[i].ToString()))
                                    {
                                        WDZ += roszcz.Wartosc;
                                        break;
                                    }

                                }
                            }


                        }


                        dw.OdsetkiKapital = WDZ;
                        dw.NotyOdsetkowe = Noty;
                        dw.Opis = "Pozew w EPU";
                        dw.RodzajDok = 10;
                        dw.StatusDok = 3;
                        dw.Tresc = pozewStr;
                        dw.TypDok = 10;
                        dw.WPS = pozew.WartoscSporu;
                        dw.Sprawa_id = idSprawy;
                        dw.TrescHtml = XML2HTMLTransform.TransformNCompress(dw.Tresc, 0);
                        bool addpdf = false;
                        if (newPozew)
                        {
                            pozewDB = new Pozew();
                            addpdf = true;
                        }
                        else
                        {
                            pozewDB = dw.Pozew.FirstOrDefault<Pozew>();
                            if (pozewDB == null)
                            {
                                pozewDB = new Pozew();
                                addpdf = true;
                            }
                        }

                        pozewDB.DataZlozenia = pozew.DataZlozenia;
                        pozewDB.Koszty = pozew.OplataSadowa.WartoscOplaty;
                        pozewDB.d_kreacji = DateTime.Today;
                        pozewDB.WPS = pozew.WartoscSporu;
                        pozewDB.Tresc = pozewStr;
                        pdf = null;
                        if (newPozew)
                        {
                            dw.Pozew = new System.Data.Objects.DataClasses.EntityCollection<Pozew>();
                            dw.Pozew.Add(pozewDB);
                        }
                        // czy jest pdf

                        if (!newPozew && dokWys_id > 0)
                        {
                            pdf = (from z in lexena.PdfStore
                                   where z.DokWys_Id == dokWys_id
                                   select z).FirstOrDefault();
                        }

                        if (pdf == null)
                        {
                            pdf = new PdfStore();
                            if (dw.PdfStore == null)
                                dw.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                            dw.PdfStore.Add(pdf);
                            lexena.PdfStore.AddObject(pdf);
                        }
                        pdf.name = "Pozew " + SygnWgPowoda;
                        retvalue = XML2HTMLTransform.html2pdf(dw.TrescHtml, ref pdffile);
                        if (retvalue.Contains("Błąd"))
                        {
                            Utils.LogWriter("Wyjątek w trakcie zapisu do pdf " + retvalue + " " + SygnWgPowoda);
                            theError.level = ErrLevel.Error;
                            theError.code = -550;
                            theError.description = " Błąd podczas zapisu pdf'a";
                            this.addError(ErrLevel.Error, -550, " Błąd podczas zapisu pdf'a", SygnWgPowoda);
                            iserror = true;
                            continue;
                        }

                        pdf.value = pdffile;
                        if (dw.DokumentWysKomunikacjaEPU == null)
                            dw.DokumentWysKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentWysKomunikacjaEPU>();
                        DokumentWysKomunikacjaEPU dokwyskom = new DokumentWysKomunikacjaEPU();
                        dokwyskom.czyus = 0;
                        dokwyskom.d_kreacji = DateTime.Now;
                        dokwyskom.Status = 3;
                        dw.DokumentWysKomunikacjaEPU.Add(dokwyskom);
                        lexena.AddToDokumentWysKomunikacjaEPU(dokwyskom);
                        // Dodanie  radcy skałdającego

                        string pesel = pozew.OsobaSkladajaca.Osoba.PESEL;
                        Radca radca = (from w in lexena.Radca where w.Pesel == pesel select w).FirstOrDefault();

                        if (radca != null)
                        {
                            if (spr != null)
                            {
                                spr.Radca_Id = radca.Id;

                            }


                        }
                        // dodanie statusu do sprawy 
                        nazstat = (from x in lexena.NazwaStatusu
                                   where x.Krok == 2
                                   select x).FirstOrDefault();

                        if (spr != null && nazstat != null)
                        {
                            if (spr.IdSprawyEPU == 0 || spr.IdSprawyEPU == null)
                            {

                                spr.DataZloPozwu = dw.DataDok;
                                spr.InneZadane = dw.InneKoszty;
                                spr.KosztyZadane = dw.Koszty;
                                spr.KzpZadane = dw.Kzp;
                                spr.Uwagi = SygnWgPowoda;


                                StatusSprawy oststat = (from x in lexena.NazwaStatusu
                                                        join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                                        where y.Sprawa_id == idSprawy && x.Krok == 2
                                                        select y).FirstOrDefault();
                                if (oststat == null)
                                {
                                    StatusSprawy stspr = new StatusSprawy();
                                    stspr.czyus = 0;
                                    stspr.CzyWiena = 0;
                                    stspr.DataStatusu = DateTime.Now;
                                    stspr.Sprawa_id = spr.id;
                                    stspr.NazwaStatusu_Id = nazstat.Id;
                                    lexena.AddToStatusSprawy(stspr);
                                }
                            }
                            spr.Uwagi = OrygOzn;

                        }
                        // działania w wienie



                        lexena.SaveChanges();
                        Utils.LogWriter(" Import pozwu do LexEna OK:  " + SygnWgPowoda);

                        this.addError(theError.level, theError.code, theError.description, theError.reference);
                    }

                }

                return true;




            }



            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie importu " + ex.Message + " zbiór" + this.importFileName + " " + SygnWgPowoda);
                this.addError(ErrLevel.Fatal, -503, "Błąd podczas importu pozów " + ex.Message + ((ex.InnerException != null) ? ex.InnerException.Message : ""), SygnWgPowoda);
                iserror = true;
                return iserror;
            }


        }

        public bool ValidateOrzeczenie()
        {
            MojeOrzeczeniaOutputData orzeczenia;
            OrzeczenieOutputElement[] lista_orzeczen;


            int kodstat;
            String sygnatura;
            String SygnWgPowoda;
            NazwaStatusu oststat;
            int idSprawy;
            byte[] pdffile;
            NazwaStatusu nazstat;
            pdffile = new byte[0];
            string mylogname;
            Sprawa spr = null;
            string nakazStr;
            bool newDok;
            int dokOdebr_id;
            pdffile = new byte[0];
            PdfStore pdf;
            string retvalue;
            Utils.LogWriter("Walidacja spraw ze zbioru : " + importFileName);
            //  Utils.LogNamedWriter(" Start Importu zbioru: " + fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + "raport.txt");

            SygnWgPowoda = "";

            if (typDok == DocType.Klauzula1)

            {
                if (this.paczkapostanowien1 == null || !this.paczkapostanowien1.OrzeczenieEPU.Any())
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie deserializacji zpraw  zbiór:" + importFileName + " jest pusty");
                    return true;
                }
            }
            else // Kla
            {
                if (this.paczkapostanowien2 == null || !this.paczkapostanowien2.listaOrzeczen.Any())
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie deserializacji zpraw  zbiór:" + importFileName + " jest pusty");
                    return true;
                }

            }

            Utils.LogWriter(" Start walidacji zbioru: " + importFileName);
            SygnWgPowoda = "";
            try
            {


                if (typDok == DocType.Klauzula2)
                {
                    lista_orzeczen = this.paczkapostanowien2.listaOrzeczen;
                }
                else
                {

                    int i = 0;
                    lista_orzeczen = new OrzeczenieOutputElement[this.paczkapostanowien1.OrzeczenieEPU.GetUpperBound(0)];// mojesprawy.listaSpraw;


                    for (i = 0; i <= this.paczkapostanowien1.OrzeczenieEPU.GetUpperBound(0); i++)
                    {
                        lista_orzeczen[i] = new OrzeczenieOutputElement();
                        /*
                        lista_orzeczen[i].dataOrzeczenia = this.paczkapostanowien1.OrzeczenieEPU[i].dataOrzeczenia.ToString();
                        listaspraw[i] = new SprawaOutputElement();
                        listaspraw[i].id = myspr.Id;
                        listaspraw[i].kwotaSporu = myspr.KwotaSporu;
                        listaspraw[i].rolaWSprawie = myspr.RolaWSprawie;
                        listaspraw[i].stanSprawy = myspr.StanSprawy;
                        listaspraw[i].sygnaturaSprawy = myspr.SygnaturaSprawy;
                        listaspraw[i].sygnaturaWgPowoda = myspr.SygnaturaWgPowoda;
                   */
                    }

                }

                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {


                    bool iserror = false;


                    foreach (OrzeczenieOutputElement orzecz in lista_orzeczen)
                    {
                        nakazStr = orzecz.dokumentXml;
                        SygnWgPowoda = orzecz.sygnaturaWgPowoda;

                        errDescription theError = new errDescription();

                        theError.level = ErrLevel.OK;
                        theError.code = 0;
                        theError.description = "";
                        theError.reference = SygnWgPowoda;


                        spr = null;
                        if (!String.IsNullOrEmpty(SygnWgPowoda.Trim()) && !SygnWgPowoda.Trim().StartsWith("ENERGA#"))
                        {
                            Utils.LogWriter(" Start Importu orzeczenia: " + SygnWgPowoda + " " + orzecz.sygnaturaSprawy);
                            if (SygnWgPowoda.Contains("#"))
                                idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1, SygnWgPowoda.IndexOf('#') - SygnWgPowoda.IndexOf('@') - 1));
                            else
                                idSprawy = Convert.ToInt32(SygnWgPowoda.Substring(SygnWgPowoda.IndexOf('@') + 1));

                            // dodanie encji pozew
                            spr = (from z in lexena.Sprawa
                                   where z.id == idSprawy
                                   select z).FirstOrDefault();
                            if (spr == null)
                            {
                                sygnatura = SygnWgPowoda.Substring(0, SygnWgPowoda.IndexOf('@'));
                                spr = (from z in lexena.Sprawa
                                       where z.sygnatura == sygnatura
                                       select z).FirstOrDefault();


                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(orzecz.sygnaturaSprawy))
                            {
                                spr = (from z in lexena.Sprawa
                                       where z.SygnNCe.Replace(" ", "").Contains(orzecz.sygnaturaSprawy.Replace(" ", ""))
                                       select z).FirstOrDefault();
                                if (spr != null) SygnWgPowoda = spr.sygnatura;
                            }
                        }
                        if (spr == null)
                        {
                            Utils.LogWriter("Błąd w trakcie wczytywania orzeczenia " + SygnWgPowoda);
                            this.addError(ErrLevel.Error, -502, "brak sprawy o takim oznaczeniu lub oznaczenie sprawy niezgodne ze standardem Energa " + SygnWgPowoda, SygnWgPowoda);
                            continue;
                        }
                        else
                            idSprawy = spr.id;



                        DokOdebr dood = (from z in lexena.DokOdebr
                                         where z.Sprawa_id == idSprawy && z.TypDok == 101 && z.Nazwa == orzecz.nazwaDecyzji && z.DataDokumentu == orzecz.dataOrzeczenia
                                         select z).FirstOrDefault();
                        if (dood == null)
                        {
                            dood = new DokOdebr();
                            newDok = true;
                            dokOdebr_id = 0;
                        }
                        else
                        {
                            dokOdebr_id = dood.Id;
                            newDok = false;
                        }
                        dood.d_kreacji = DateTime.Today;
                        dood.DataDokumentu = orzecz.dataOrzeczenia;
                        dood.DataRejestracji = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                        dood.CzyEPU = 1;
                        dood.IdEPU = orzecz.id;
                        dood.Nazwa = orzecz.nazwaDecyzji;
                        dood.TypDok = 101;
                        dood.Sprawa_id = idSprawy;
                        dood.PartitionKey = 0;
                        dood.Format = 100;
                        dood.StatusDok = "wydano";
                        dood.Tresc = orzecz.dokumentXml;
                        dood.TrescHtml = XML2HTMLTransform.TransformNCompress(dood.Tresc, 101);
                        dood.CzyZalatw = 0;
                        if (KontoEpu > 0) dood.KontoEPU_Id = KontoEpu;
                        // czy jest pdf
                        pdf = null;
                        if (!newDok && dokOdebr_id > 0)
                        {
                            pdf = (from z in lexena.PdfStore
                                   where z.DokOdebr_Id == dokOdebr_id
                                   select z).FirstOrDefault();
                        }

                        if (pdf == null)
                        {

                            pdf = new PdfStore();
                            if (dood.PdfStore == null)
                                dood.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                            dood.PdfStore.Add(pdf);
                            lexena.PdfStore.AddObject(pdf);
                        }

                        pdf.name = orzecz.nazwaDecyzji + " " + SygnWgPowoda;
                        pdf.name = pdf.name.Substring(0, (pdf.name.Length > 100 ? 100 : pdf.name.Length));
                        retvalue = XML2HTMLTransform.html2pdf(dood.TrescHtml, ref pdffile);
                        if (retvalue.Contains("Błąd"))
                        {
                            this.addError(ErrLevel.Error, -620, "Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego  ", SygnWgPowoda);
                            Utils.LogWriter("Wyjątek w trakcie tworzenia pdf'a Dokumentu Odebranego " + retvalue + " " + SygnWgPowoda);
                            iserror = true;
                            continue;
                        }
                        pdf.value = pdffile;
                        if (dood.DokumentKomunikacjaEPU == null)
                            dood.DokumentKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentKomunikacjaEPU>();
                        DokumentKomunikacjaEPU dokwyskom = new DokumentKomunikacjaEPU();
                        dokwyskom.czyus = 0;
                        dokwyskom.d_kreacji = DateTime.Now;
                        dokwyskom.Status = 3;
                        dood.DokumentKomunikacjaEPU.Add(dokwyskom);
                        //lexena.AddToDokumentKomunikacjaEPU(dokwyskom);



                        if ((orzecz.nazwaDecyzji.ToLower().Contains("podsta") && orzecz.nazwaDecyzji.ToLower().Contains("przekaza")) || (orzecz.nazwaDecyzji.ToLower().Contains("uchyl") && orzecz.nazwaDecyzji.ToLower().Contains("nakaz")))
                            kodstat = 12;
                        else if (orzecz.nazwaDecyzji.ToLower().Contains("sprzeci") && orzecz.nazwaDecyzji.ToLower().Contains("przekaza"))
                            kodstat = 11;
                        else if (orzecz.nazwaDecyzji.ToLower().Contains("umorzen"))
                            kodstat = 13;
                        else if ((orzecz.nazwaDecyzji.ToLower().Contains("zwroci") || orzecz.nazwaDecyzji.ToLower().Contains("odrzuc")) && orzecz.nazwaDecyzji.ToLower().Contains("pozw"))
                            kodstat = 14;
                        else kodstat = 0;
                        if (kodstat > 0)
                        {
                            // dodanie statusu do sprawy 
                            spr = (from z in lexena.Sprawa
                                   where z.id == idSprawy
                                   select z).FirstOrDefault();
                            nazstat = (from x in lexena.NazwaStatusu
                                       where x.Krok == kodstat
                                       select x).FirstOrDefault();
                            oststat = (from x in lexena.NazwaStatusu
                                       join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                       where y.Sprawa_id == idSprawy
                                       orderby y.DataStatusu descending
                                       select x).FirstOrDefault();


                            if (spr != null && oststat.Krok < kodstat)
                            {

                                StatusSprawy stspr = new StatusSprawy();
                                stspr.czyus = 0;
                                stspr.CzyWiena = 0;
                                stspr.DataStatusu = DateTime.Now;
                                stspr.Sprawa_id = spr.id;
                                stspr.NazwaStatusu_Id = nazstat.Id;
                                lexena.AddToStatusSprawy(stspr);
                            }

                            lexena.SaveChanges();
                            Utils.LogWriter(" Import OK:  " + SygnWgPowoda);
                            this.addError(theError.level, theError.code, theError.description, theError.reference);
                        }

                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania post. o nad klauzuli " + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : "") + " zbiór" + this.importFileName);
                this.addError(ErrLevel.Fatal, -503, "Błąd podczas importu klauzul " + ex.Message + ((ex.InnerException != null) ? ex.InnerException.Message : ""), SygnWgPowoda);
                return false;

            }



        }

        public bool ValidateWniosekEgz()
        {

            WniosekEgzekucyjny[] lista_wnioskow;


            PdfStore pdf;

            String SygnWgPowoda;
            int idSprawy;
            string retvalue;
            string sygnatura;
            decimal Noty;
            byte[] pdffile;
            string wniosekStr;
            Sprawa spr;
            bool newWniosek;
            NazwaStatusu nazstat;
            int dokWys_id;
            pdffile = new byte[0];
            string mylogname;
            bool iserror = false;
            bool setBreak;
            bool skipNewpdates = false;

            Utils.LogWriter("Walidacja spraw ze zbioru : " + importFileName);
            //  Utils.LogNamedWriter(" Start Importu zbioru: " + fname, Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + "raport.txt");

            SygnWgPowoda = "";


            if (this.paczkawnioskow == null || !this.paczkawnioskow.WniosekEgzekucyjny.Any())
            {
                this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                Utils.LogWriter("Błąd w trakcie deserializacji zpraw  zbiór:" + importFileName + " jest pusty");
                return true;
            }



            Utils.LogWriter(" Start walidacji zbioru: " + importFileName);
            SygnWgPowoda = "";
            try
            {



                lista_wnioskow = this.paczkawnioskow.WniosekEgzekucyjny.ToArray();

                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {

                    using (wiena_centralEntities wiena = new wiena_centralEntities())
                    {
                        iserror = false;


                        foreach (WniosekEgzekucyjny wniosek in lista_wnioskow)
                        {
                            SygnWgPowoda = "";
                            wniosekStr = SerializeToString(wniosek, typeof(WniosekEgzekucyjny), true);
                            newWniosek = false;
                            dokWys_id = 0;
                            skipNewpdates = false;


                            spr = null;
                            sygnatura = wniosek.Nakaz.Sygnatura;

                            Utils.LogWriter(" Start Importu wniosku: " + sygnatura);

                            sygnatura = sygnatura.Replace(" ", "");
                            errDescription theError = new errDescription();

                            theError.level = ErrLevel.OK;
                            theError.code = 0;
                            theError.description = "";
                            theError.reference = sygnatura;


                            spr = (from z in lexena.Sprawa
                                   where sygnatura.EndsWith(z.SygnNCe.Replace(" ", ""))
                                   select z).FirstOrDefault();





                            if (spr == null)

                            {
                                spr_sadowa s_sad = (from z in wiena.spr_sadowa 
                                       where sygnatura.EndsWith(z.sygn_nakazowe.Replace(" ", ""))
                                       select z).FirstOrDefault();
                                if (s_sad == null)
                                {

                                    Utils.LogWriter("Błąd w trakcie wczytywania Wniosku Egzekucyjnego  " + sygnatura + " nie istnieje w Lexena, zbiór");
                                    this.addError(ErrLevel.Error, -502, "brak sprawy o takim oznaczeniu lub oznaczenie sprawy niezgodne ze standardem Energa " + sygnatura, sygnatura);
                                    iserror = true;
                                    continue;
                                }
                                else
                                {
                                    spr = (from z in lexena.Sprawa
                                           where z.IdWiena == s_sad.id_sprawa
                                           select z).FirstOrDefault();
                                    if (spr == null)
                                    {
                                        Utils.LogWriter("Błąd w trakcie wczytywania Wniosku Egzekucyjnego  " + sygnatura + " nie istnieje w Lexena, zbiór");
                                        this.addError(ErrLevel.Error, -503, "brak sprawy o takim oznaczeniu lub oznaczenie sprawy niezgodne ze standardem Energa " + sygnatura, sygnatura);
                                        iserror = true;
                                        continue;

                                    }
                                    spr.SygnNCe = s_sad.sygn_nakazowe;
                                    lexena.SaveChanges();
                                    idSprawy = spr.id;
                                    SygnWgPowoda = spr.sygnatura;

                                }
                            }
                            else
                            {
                                idSprawy = spr.id;
                                SygnWgPowoda = spr.sygnatura;

                            }

                            int wiena_id = (from x in wiena.sprawa join f in wiena.firma on x.id_firmy equals f.ident where x.sygnatura == spr.sygnatura && f.typ_firmy == FirmaTyp select x.ident).FirstOrDefault();
                            if (wiena_id == 0)
                            {
                                iserror = true;

                                this.addError(ErrLevel.Error, -562, "Brak sprawy w systemie Wiena " + SygnWgPowoda + " w systemie Energii", SygnWgPowoda);
                                lexena.Sprawa.Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, spr);
                                continue;
                            }
                            theError.reference = SygnWgPowoda;
                            DateTime dWn = Convert.ToDateTime(wniosek.dataWniosku);
                            DokWys dw = (from z in lexena.DokWys
                                         where z.Sprawa_id == idSprawy && z.TypDok == 30 && z.DataDok == dWn
                                         select z).FirstOrDefault();
                            if (dw == null)
                            {
                                dw = new DokWys();
                                newWniosek = true;
                                Utils.LogWriter("Tworzę nowy Wniosek egzekucyjny dla " + SygnWgPowoda);
                            }
                            else
                            {
                                newWniosek = false;
                                dokWys_id = dw.Id;
                            }

                            dw.DataDok = Convert.ToDateTime(wniosek.dataWniosku);
                            dw.DataOdbioru = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
                            dw.InneKoszty = wniosek.InneKoszty == null ? 0 : wniosek.InneKoszty.Wartosc;
                            dw.Koszty = 0;
                            if (newWniosek)
                                dw.d_kreacji = DateTime.Now;
                            else
                                dw.d_modyfikacji = DateTime.Now;

                            int IdKomornik = (int)wniosek.Komornik.ID;
                            // czy jest taki omornik
                            KancelariaKomornicza kom = (from z in lexena.KancelariaKomornicza
                                                        where z.IdEPU == IdKomornik
                                                        select z).FirstOrDefault();
                            if (kom == null)
                            {
                                kom = new KancelariaKomornicza();
                                kom.IdEPU = IdKomornik;
                                kom.Nazwa = wniosek.Komornik.Nazwa;
                                kom.DataWprowadzenia = DateTime.Today;
                                kom.czyus = 0;
                                lexena.AddToKancelariaKomornicza(kom);
                                lexena.SaveChanges();



                            }
                            dw.Komornik_Id = kom.Id;

                            if (wniosek.KosztyZastepstwa.Zasadzenie == 1)
                                if (wniosek.KosztyZastepstwa.WgNorm == 1)
                                    dw.Kzp = EPUCalc.countKZA((spr.WPS == null ? 0 : spr.WPS.Value) + (spr.KosztyZadane == null ? 0 : spr.KosztyZadane.Value) + (spr.KzpZadane == null ? 0 : spr.KzpZadane.Value) + (spr.InneZadane == null ? 0 : spr.InneZadane.Value));
                                else
                                    dw.Kzp = wniosek.KosztyZastepstwa.Wartosc;
                            dw.Nazwa = "Wniosek Egzekucyjny";
                            if (KontoEpu > 0) dw.KontoEPU_Id = KontoEpu;
                            Noty = 0;
                            dw.Opis = wniosek.Komornik.Nazwa + " :Id=" + wniosek.Komornik.ID.ToString();
                            dw.RodzajDok = 30;
                            dw.StatusDok = 3;
                            dw.Tresc = wniosekStr;
                            dw.TypDok = 30;
                            dw.WPS = spr.WPS ?? 0 + spr.KosztyZadane ?? 0 + spr.KzpZadane ?? 0 + spr.InneZadane ?? 0;
                            dw.Sprawa_id = idSprawy;
                            dw.TrescHtml = XML2HTMLTransform.TransformNCompress(dw.Tresc, 30);
                            pdf = null;

                            if (!newWniosek && dokWys_id > 0)
                            {
                                pdf = (from z in lexena.PdfStore
                                       where z.DokWys_Id == dokWys_id
                                       select z).FirstOrDefault();
                            }

                            if (pdf == null)
                            {
                                pdf = new PdfStore();
                                if (dw.PdfStore == null)
                                    dw.PdfStore = new System.Data.Objects.DataClasses.EntityCollection<PdfStore>();
                                dw.PdfStore.Add(pdf);
                                lexena.PdfStore.AddObject(pdf);
                            }
                            pdf.name = "WniosekEgz" + SygnWgPowoda;
                            retvalue = XML2HTMLTransform.html2pdf(dw.TrescHtml, ref pdffile);
                            if (retvalue.Contains("Błąd"))
                            {

                                this.addError(ErrLevel.Error, -620, "Wyjątek w trakcie tworzenia pdf'a Wniosku Egzekucyjnego  ", SygnWgPowoda);
                                Utils.LogWriter("Wyjątek w trakcie tworzenia pdf'a Wniosku Egzekucyjnego" + retvalue + " " + SygnWgPowoda);
                                iserror = true;
                                continue; ;


                            }
                            pdf.value = pdffile;

                            bool pws_new = false;
                            pisma_wys pws = wiena.pisma_wys.Where(a => a.id_sprawy == wiena_id && a.data_r == dw.DataDok && a.nazwa_pisma == dw.Nazwa && a.czyus == 0).FirstOrDefault();
                            if (pws == null)
                            {
                                pws = new pisma_wys();
                                pws_new = true;
                            }
                            pws.adresat = 3;
                            pws.czyus = 0;
                            pws.data_r = dw.DataDok;
                            pws.d_kreacji = DateTime.Now;
                            pws.id_sprawy = wiena_id;
                            pws.nazwa2 = "";
                            pws.nazwa3 = wniosek.Komornik.Nazwa.Truncate(150);
                            pws.nazwa_pisma = "Wniosek o wszczęcie postępowania egzekucyjnego";
                            pws.odpis = 0;
                            pws.stan = 1;
                            pws.sygnatura = spr.sygnatura;
                            pws.sygn_psm = "P_0013";
                            pws.termin = 0;
                            pws.typ_dok_tresc = 1;
                            pws.id_dok = 14549;
                            pws.tresc_bin = pdf.value;
                            if (pws_new)
                                wiena.pisma_wys.Add(pws);



                            if (dw.DokumentWysKomunikacjaEPU == null)
                                dw.DokumentWysKomunikacjaEPU = new System.Data.Objects.DataClasses.EntityCollection<DokumentWysKomunikacjaEPU>();
                            DokumentWysKomunikacjaEPU dokwyskom = new DokumentWysKomunikacjaEPU();
                            dokwyskom.czyus = 0;
                            dokwyskom.d_kreacji = DateTime.Now;
                            dokwyskom.Status = 3;
                            dw.DokumentWysKomunikacjaEPU.Add(dokwyskom);
                            lexena.AddToDokumentWysKomunikacjaEPU(dokwyskom);
                            // Dodanie  radcy skałdającego

                            string pesel = wniosek.OsobaSkladajaca.Osoba.PESEL;
                            Radca radca = (from w in lexena.Radca where w.Pesel == pesel select w).FirstOrDefault();

                            if (radca != null)
                            {
                                if (spr != null)
                                {
                                    spr.Radca_Id = radca.Id;

                                }


                            }

                            // dodanie radcy w LexEna
                            radca rw = wiena.radca.Where(a => a.PESEL == pesel).OrderByDescending(a => a.ident).FirstOrDefault();
                            if (rw != null)
                            {
                                sprawa spw = wiena.sprawa.Where(a => a.ident == wiena_id).FirstOrDefault();
                                if (spw != null)
                                    spw.id_radca_egz = rw.ident;


                            }
                            //deaktywacja spr_egz
                            spr_egz se = wiena.spr_egz.Where(a => a.id_sprawy == wiena_id && a.data_wniosku == dw.DataDok && a.aktualna == 1 && a.czyus == 0).FirstOrDefault();
                            bool se_new = true;
                            if (se != null)
                            {
                                se_new = false;
                                se.d_modyfikacji = DateTime.Now;
                            }
                            else
                            {
                                // deaktywacja spr_egz 
                                List<spr_egz> lstEgz = wiena.spr_egz.Where(a => a.id_sprawy == wiena_id && a.czyus == 0).ToList();
                                if (lstEgz.Count > 1)
                                {
                                    if (lstEgz != null && lstEgz.Any())
                                    {
                                        foreach (spr_egz eg in lstEgz)
                                        {
                                            eg.aktualna = 0;

                                        }

                                    }
                                    se = new spr_egz();
                                    se_new = true;
                                    se.d_kreacji = DateTime.Now;
                                    se.id_komornik = kom.IdWiena > 0 ? kom.IdWiena.Value : 0;
                                    se.aktualna = 1;
                                }
                                else
                                {
                                    spr_egz sex = lstEgz.FirstOrDefault();
                                    if (sex != null)
                                        sex.aktualna = 0;
                                    se = new spr_egz();
                                    se_new = true;
                                    se.d_kreacji = DateTime.Now;
                                    se.id_komornik = kom.IdWiena > 0 ? kom.IdWiena.Value : 0;
                                    se.aktualna = 1;

                                    

                                }
                            }
                            se.id_sprawy = wiena_id;
                            se.data_wniosku = dw.DataDok;
                            se.aktualna = 1;
                            se.czyus = 0;
                            if (se_new)
                                wiena.spr_egz.Add(se);



                            // dodanie statusu do sprawy 
                            nazstat = (from x in lexena.NazwaStatusu
                                       where x.Krok == 10
                                       select x).FirstOrDefault();
                            if (spr != null && nazstat != null)
                            {
                                //if (spr.IdSprawyEPU == 0 || spr.IdSprawyEPU == null)
                                {


                                    StatusSprawy oststat = (from x in lexena.NazwaStatusu
                                                            join y in lexena.StatusSprawy on x.Id equals y.NazwaStatusu_Id
                                                            where y.Sprawa_id == idSprawy && x.Krok == 10
                                                            select y).FirstOrDefault();
                                    if (oststat == null)
                                    {
                                        StatusSprawy stspr = new StatusSprawy();
                                        stspr.czyus = 0;
                                        stspr.CzyWiena = 0;
                                        stspr.DataStatusu = DateTime.Now.Date;
                                        stspr.Sprawa_id = spr.id;
                                        stspr.NazwaStatusu_Id = nazstat.Id;
                                        lexena.AddToStatusSprawy(stspr);
                                    }
                                }

                            }
                            // dodanie statusu w Wienie
                            pws_new = false;
                            status_spr sspr = wiena.status_spr.Where(a => a.id_sprawy == wiena_id && a.czyus == 0 && a.od_dnia == dw.DataDok && a.id_statusu == 2).FirstOrDefault();
                            status_spr ost_sspr = wiena.status_spr.Where(a => a.id_sprawy == wiena_id && a.czyus == 0).OrderByDescending(a => a.od_dnia).ThenByDescending(a => a.ident).FirstOrDefault();

                            if ((ost_sspr == null || (ost_sspr != null && ost_sspr.id_statusu != 9 && ost_sspr.id_statusu != 11 && ost_sspr.id_statusu != 108)))
                            {
                                if (sspr == null)
                                {
                                    pws_new = true;
                                    sspr = new status_spr();
                                    sspr.d_kreacji = DateTime.Now;
                                }
                                else
                                    sspr.d_modyfikacji = DateTime.Now;

                                sspr.id_sprawy = wiena_id;
                                sspr.id_statusu = 2;
                                sspr.od_dnia = dw.DataDok.Value;
                                sspr.czyus = 0;
                            }
                            if (pws_new)
                            {

                                wiena.status_spr.Add(sspr);


                            }
                            wiena.SaveChanges();
                            Utils.LogWriter(" Import wiena  OK:  " + SygnWgPowoda);
                            lexena.SaveChanges();
                            Utils.LogWriter(" Import OK:  " + SygnWgPowoda);
                            this.addError(theError.level, theError.code, theError.description, theError.reference);
                        }
                    }
                    return true;

                }
            }

            catch (DbEntityValidationException ex)
            {

                Utils.LogWriter("Błąd walidacji przy zapisie  wniosku egzekucyjnego " + ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "") + " zbiór" + this.importFileName);
                this.addError(ErrLevel.Fatal, -506, "Błąd podczas importu wniosków egzekucyjnych " + ex.Message + ((ex.InnerException != null) ? ex.InnerException.Message : ""), SygnWgPowoda);
                return false;
            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd w trakcie wczytywania wniosków egzekucyjnych " + ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "") + " zbiór" + this.importFileName);
                this.addError(ErrLevel.Fatal, -503, "Błąd podczas importu wniosków egzekucyjnych " + ex.Message + ((ex.InnerException != null) ? ex.InnerException.Message : ""), SygnWgPowoda);
                return false;
            }


        }

        public bool ValidateKosztyEgz()
        {
            // 
            try
            {


                byte[] data = Convert.FromBase64String(this.inDOC);
                if (data == null)
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie odczytu zbioru " + importFileName + " jest pusty");
                    return true;

                }
                Stream stream = new MemoryStream(data);

                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();
                DataTable worksheet = result.Tables[0];
                if (worksheet.Rows.Count > 0)
                {
                    int i = 0;
                    int IdSprawy = 0;
                    using (wiena_centralEntities wienaContext = new wiena_centralEntities())
                    {
                        bool czyblad;
                        string sygnatura;

                        decimal nalgl = 0;
                        decimal odsetki = 0;
                        decimal koszty_procesu = 0;
                        string komornik;

                        foreach (DataRow row in worksheet.Rows)
                        {
                            i++;
                            IdSprawy = 0;
                            czyblad = false;
                            errDescription theError = new errDescription();
                            theError.level = ErrLevel.OK;
                            theError.code = 0;
                            theError.reference = "wiersz " + i.ToString();
                            try
                            {
                                sygnatura = theError.reference = row[1] as string;
                            }
                            catch
                            {
                                theError.description += "błąd odczytu sygnatury";
                                theError.level = ErrLevel.Error;
                                theError.code = -801;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }
                            var idSpr = (from spr in wienaContext.sprawa
                                         join frm in wienaContext.firma on spr.id_firmy equals frm.ident
                                         where spr.sygnatura == sygnatura && frm.typ_firmy == FirmaTyp
                                         select new { idSprawy = spr.ident }).FirstOrDefault();
                            if (idSpr == null)
                            {

                                theError.description += "brak sprawy w systemie Wiena ";
                                theError.level = ErrLevel.Error;
                                theError.code = -805;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;


                            }
                            IdSprawy = idSpr.idSprawy;


                            zlecenia_ksiegowe zlc = new zlecenia_ksiegowe();
                            zlc.czyus = 0;
                            zlc.d_kreacji = DateTime.Now;
                            zlc.d_ksiegowania = null;
                            try
                            {

                                zlc.d_zlecenia = Convert.ToDateTime(row[8].ToString());

                            }
                            catch (Exception ex)
                            {
                                theError.description += "błąd odczytu daty wniosku " + ex.Message;
                                theError.level = ErrLevel.Error;
                                theError.code = -802;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }
                            bool czynew = false;
                            zlecenia_ksiegowe zks = wienaContext.zlecenia_ksiegowe.Where(a => a.d_zlecenia == zlc.d_zlecenia && a.nr_dowodu == "Uz5" && a.id_sprawy == IdSprawy).FirstOrDefault();
                            if (zks == null)
                            {
                                czynew = true;
                            }
                            else
                            {
                                czynew = false;
                                zlc = zks;
                                zlc.d_modyfikacji = DateTime.Now;

                            }


                            try
                            {
                                nalgl = Decimal.Parse(row[4].ToString());
                                odsetki = Decimal.Parse(row[5].ToString());
                                koszty_procesu = Decimal.Parse(row[6].ToString());

                            }
                            catch (Exception ex)
                            {
                                theError.description += "błąd odczytu kwot " + ex.Message;
                                theError.level = ErrLevel.Error;
                                theError.code = -803;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }
                            try
                            {
                                komornik = row[9].ToString();

                            }
                            catch (Exception ex)
                            {
                                theError.description += "błąd odczytu komornika " + ex.Message;
                                theError.level = ErrLevel.Error;
                                theError.code = -804;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }
                            // szukamy sprawy





                            // sprawdzenie czy w tej sprawie już istnieje takoe zlecenie księgowe
                            obroty obr = (from o in wienaContext.obroty
                                          join ka in wienaContext.konto_anal on o.id_konta equals ka.ident
                                          join knt in wienaContext.konto on ka.id_konta equals knt.id
                                          where knt.konto1 == "2442170" && knt.id_firmy == 1 && o.id_sprawy == IdSprawy
                                          select o).FirstOrDefault();
                            if (obr != null)
                            {

                                theError.description += "istnieje zapis na koncie egzekucji komorniczej w tej sprawie, polecenie zostanie pominięte ";
                                theError.level = ErrLevel.Warning;
                                theError.code = 806;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }
                            vw_KosztyEgz koe = (from koegz in wienaContext.vw_KosztyEgz where koegz.id_sprawy == IdSprawy select koegz).FirstOrDefault();
                            if (koe == null)
                            {

                                theError.description += "brak poprawnego księgowania dla sprawy w systemie Wiena ";
                                theError.level = ErrLevel.Error;
                                theError.code = -807;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }
                            zlc.id_sprawy = IdSprawy;
                            zlc.kod = 1;
                            zlc.konto = 14;
                            zlc.id_obroty = 0;
                            zlc.nr_dowodu = "Uz5";
                            zlc.konto_anal = koe.konto_anal;
                            zlc.wdz = koe.wdz;
                            zlc.odsetki = koe.odsetki.Value + odsetki;
                            zlc.ods_kapital = koe.ods_kapital;
                            zlc.kza = koe.kza;
                            zlc.kl_adw = koe.kl_adw;
                            zlc.rodzaj = 0;
                            zlc.id_schemat = 19;
                            zlc.wpis = koe.wpis;
                            zlc.kzp = koe.kzp;
                            zlc.klauzula = koe.klauzula;
                            zlc.inne_sad = koe.inne_sad;
                            zlc.kza = koe.kzaa;
                            zlc.vat = koe.vat ?? 0;
                            zlc.komo = 0;
                            zlc.energia = koe.energia.Value;
                            zlc.winien = zlc.energia + zlc.odsetki + zlc.inne + zlc.kzp.Value + zlc.wpis.Value + zlc.klauzula.Value + zlc.kza.Value + zlc.kl_adw.Value + zlc.komo.Value + zlc.inne_sad.Value;
                            zlc.koszty = zlc.kzp.Value + zlc.wpis.Value + zlc.klauzula.Value + zlc.kza.Value + zlc.kl_adw.Value + zlc.komo.Value + zlc.inne_sad.Value;
                            if (czynew)
                                wienaContext.zlecenia_ksiegowe.Add(zlc);
                            wienaContext.SaveChanges();
                            Utils.LogWriter(" Import OK:  " + sygnatura);
                            this.addError(theError.level, theError.code, theError.description, theError.reference);

                        }  // dodanie zlecenia księgowego



                    }

                }
                else
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie odczytu zbioru " + importFileName + " jest pusty");
                    return true;
                }


                return true;
            }
            catch (Exception ex)
            {

                Utils.LogWriter("Błąd w trakcie wczytywania zleceń dla wniosków egz -  " + (ex.Message + ex.InnerException != null ? ex.InnerException.Message : "") + " zbiór" + this.importFileName);
                this.addError(ErrLevel.Fatal, -503, "Błąd podczas zleceń dla wniosków egz " + ex.Message + ((ex.InnerException != null) ? ex.InnerException.Message : ""), "");
                return false;

            }



        }

        public bool ValidateCzynnosci()
        {
            // 
            try
            {


                byte[] data = Convert.FromBase64String(this.inDOC);
                if (data == null)
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie odczytu zbioru " + importFileName + " jest pusty");
                    return true;

                }
                Stream stream = new MemoryStream(data);

                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();
                DataTable worksheet = result.Tables[0];
                if (worksheet.Rows.Count > 0)
                {
                    int i = 0;
                    int IdSprawy = 0;
                    using (wiena_centralEntities wienaContext = new wiena_centralEntities())
                    {
                        bool czyblad;
                        string sygnatura;
                        foreach (DataRow row in worksheet.Rows)
                        {

                            DateTime dDokumentu;
                            DateTime dOtrzymania;
                            string nazwaDok;
                            string sygnKM;
                            decimal kza = 0;
                            string przycz_zawiesz = "";


                            i++;
                            IdSprawy = 0;
                            czyblad = false;
                            errDescription theError = new errDescription();
                            theError.level = ErrLevel.OK;
                            theError.code = 0;
                            theError.reference = "wiersz " + i.ToString();
                            try
                            {
                                sygnatura = theError.reference = row[3] as string;
                            }
                            catch
                            {
                                theError.description += "błąd odczytu sygnatury";
                                theError.level = ErrLevel.Error;
                                theError.code = -801;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }
                            try
                            {
                                if (row[0].GetType().ToString() == "DateTime")
                                    dOtrzymania = (DateTime)row[0];
                                else
                                    dOtrzymania = Convert.ToDateTime(row[0].ToString());

                                if (row[1].GetType().ToString() == "DateTime")
                                    dDokumentu = (DateTime)row[1];
                                else
                                    dDokumentu = Convert.ToDateTime(row[1].ToString());

                                if (dOtrzymania > DateTime.Today || dOtrzymania < new DateTime(2008, 1, 1) || dDokumentu > DateTime.Today || dDokumentu < new DateTime(2008, 1, 1))
                                {
                                    Utils.LogWriter("Daty :" + dOtrzymania.ToString() + " " + dDokumentu.ToString());
                                    theError.description += "błąd odczytu daty";
                                    theError.level = ErrLevel.Error;
                                    theError.code = -801;
                                    this.addError(theError.level, theError.code, theError.description, theError.reference);
                                    continue;


                                }

                            }
                            catch (Exception ex)
                            {
                                theError.description += "błąd odczytu daty otrzymania lub daty dokumentu " + ex.Message;
                                theError.level = ErrLevel.Error;
                                theError.code = -902;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }
                            try
                            {
                                nazwaDok = row[2].ToString().Trim();
                                sygnKM = row[4].ToString().Trim();

                            }
                            catch (Exception ex)
                            {
                                theError.description += "błąd odczytu nazwy dokumentu lub sygnatury KM  " + ex.Message;
                                theError.level = ErrLevel.Error;
                                theError.code = -903;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }
                            if (String.IsNullOrWhiteSpace(nazwaDok))
                            {
                                theError.description += " nazwa dokumentu nie może być pusta  ";
                                theError.level = ErrLevel.Error;
                                theError.code = -907;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;
                            }


                            try
                            {

                                kza = 0;
                                if (!String.IsNullOrWhiteSpace(row[5].ToString()))
                                    kza = Decimal.Parse(row[5].ToString());

                            }
                            catch (Exception ex)
                            {
                                theError.description += "błąd odczytu kosztów zast. adw. " + ex.Message;
                                theError.level = ErrLevel.Error;
                                theError.code = -904;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }
                            try
                            {
                                przycz_zawiesz = row[6].ToString().Trim();

                            }
                            catch (Exception ex)
                            {
                                theError.description += "błąd odczytu przyczyny zawieszenia " + ex.Message;
                                theError.level = ErrLevel.Error;
                                theError.code = -804;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);


                            }
                            // szukamy sprawy
                            var idSpr = (from spr in wienaContext.sprawa
                                         join frm in wienaContext.firma on spr.id_firmy equals frm.ident
                                         where spr.sygnatura == sygnatura && frm.typ_firmy == FirmaTyp
                                         select new { idSprawy = spr.ident }).FirstOrDefault();
                            if (idSpr == null)
                            {

                                theError.description += "brak sprawy w systemie Wiena ";
                                theError.level = ErrLevel.Error;
                                theError.code = -905;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;


                            }
                            IdSprawy = idSpr.idSprawy;
                            // sprawdzenie czy w tej sprawie już istnieje takoe zlecenie księgowe
                            // wyszukanie dokumentu 

                            dok_odebr_nazwy dok_naz = wienaContext.dok_odebr_nazwy.Where(a => a.nazwa == nazwaDok).OrderBy(a => a.ident).FirstOrDefault();

                            if (dok_naz == null)
                            {

                                theError.description += "Brak takiej nazwy dokumentu w systemie Wiena ";
                                theError.level = ErrLevel.Error;
                                theError.code = -907;
                                this.addError(theError.level, theError.code, theError.description, theError.reference);
                                continue;

                            }

                            using (LexEnaMeritumEntities lexenaContext = new LexEnaMeritumEntities())
                            {
                                MapDokKom mdk = lexenaContext.MapDokKom.Where(c => c.nazwaDok == nazwaDok).FirstOrDefault();
                                if (mdk == null)
                                {

                                    theError.description += "Brak takiej nazwy dokumentu w konfiguracji mapowania  ";
                                    theError.level = ErrLevel.Error;
                                    theError.code = -908;
                                    this.addError(theError.level, theError.code, theError.description, theError.reference);
                                    continue;

                                }
                                // 
                                /*
                  _typAkcji.Add(new typSlownik("<Brak akcji>", 0));
                    _typAkcji.Add(new typSlownik("post. o zawieszeniu egzekucji", 1));
                    _typAkcji.Add(new typSlownik("post. o podjęciu zawieszonego postępowania", 2));  // przy serializacji zmapować na 0  i poznawać po obecnoości tagi inne ..... - tylko na czas edycji
                    _typAkcji.Add(new typSlownik("post. o umorzeniu - zgon dłużnika", 3));
                    _typAkcji.Add(new typSlownik("post. o umorzeniu wobec bezskuteczności egzekucji",4));
                    _typAkcji.Add(new typSlownik("post. o umorzeniu na wniosek wierzyciela", 5));
                    _typAkcji.Add(new typSlownik("post. o umorzeniu postępowania z mocy prawa", 6));
                    _typAkcji.Add(new typSlownik("post. o zakończeniu", 7));
                    _typAkcji.Add(new typSlownik("zawiadomienie o zajęciu rachunku bankowego", 8));
                    _typAkcji.Add(new typSlownik("post. o przyznaniu kosztów zastępstwa adwokackiego", 9));
                                 **/
                                // dodanie spr_egze
                                spr_egz egzek = wienaContext.spr_egz.Where(a => a.czyus == 0 && a.aktualna == 1 && a.id_sprawy == IdSprawy).FirstOrDefault();
                                if (egzek == null)
                                { // dodajemy spr_egz
                                    egzek = new spr_egz();
                                    egzek.aktualna = 1;
                                    egzek.czyus = 0;
                                    egzek.d_kreacji = DateTime.Now;
                                    egzek.id_sprawy = IdSprawy;
                                    wienaContext.spr_egz.Add(egzek);

                                }
                                egzek.km = sygnKM;

                                switch (mdk.obsluga)
                                {
                                    case 1:
                                        egzek.data_zawiesz = dDokumentu;
                                        if (!String.IsNullOrWhiteSpace(przycz_zawiesz))
                                        {
                                            slow_rodzaj pzaw = wienaContext.slow_rodzaj.Where(a => a.co == 3 && a.nazwa == przycz_zawiesz).FirstOrDefault();
                                            if (pzaw == null)
                                            {
                                                theError.level = ErrLevel.Warning;
                                                theError.description += " Przyczyna zawieszenia niezgodna ze słownikiem, nie została  odnotowana";
                                            }
                                            else
                                                egzek.przyczyna_zawiesz = pzaw.ident;
                                        }
                                        break;
                                    case 2:
                                        // podjęcie zawieszonego post.
                                        break;
                                    case 3:
                                        //post.o umorzeniu -zgon dłużnika
                                        egzek.data_umo = dDokumentu;
                                        egzek.data_dor_umo = dOtrzymania;
                                        egzek.przyczyna_umo = 5;
                                        break;
                                    case 4:
                                        egzek.data_umo = dDokumentu;
                                        egzek.data_dor_umo = dOtrzymania;
                                        // post. o umorzeniu wobec bezskuteczności egzekucji
                                        egzek.przyczyna_umo = 1;
                                        break;
                                    case 5:
                                        egzek.data_umo = dDokumentu;
                                        egzek.data_dor_umo = dOtrzymania;
                                        //post. o umorzeniu na wniosek wierzyciela
                                        egzek.przyczyna_umo = 14;
                                        break;
                                    case 6:
                                        //post.o umorzeniu postępowania z mocy prawa
                                        egzek.data_umo = dDokumentu;
                                        egzek.data_dor_umo = dOtrzymania;
                                        egzek.przyczyna_umo = 6;
                                        break;
                                    case 7:
                                        // post. o zakończeniu
                                        break;
                                    case 8:
                                        //   zawiadomienie o zajęciu rachunku bankowego
                                        break;
                                    case 9:
                                        //post. o przyznaniu kosztów zastępstwa adwokackiego
                                        break;
                                    default:
                                        break;


                                }

                                WienaDB.Models.sprawa sprx = wienaContext.sprawa.Where(a => a.ident == IdSprawy).FirstOrDefault();
                                // obsługa kza 
                                if (kza > 0)
                                {
                                    // czy w sprawie są kza
                                    WienaDB.Models.naleznosc kzax = wienaContext.naleznosc.Where(a => a.id_sprawy == IdSprawy && a.id_typ_nal == 14).OrderByDescending(a => a.data_n).FirstOrDefault();
                                    if (kzax != null)
                                        if (kzax.data_n.HasValue)
                                        {
                                            if (kzax.data_n.Value.AddDays(60) < dDokumentu)
                                                kzax = null;
                                        }
                                        else
                                            kzax = null;

                                    if (kzax == null)
                                    {

                                        egzek.data_post_koszt = dDokumentu;


                                        naleznosc nal = new naleznosc();
                                        nal.id_sprawy = IdSprawy;
                                        nal.id_typ_nal = 14;
                                        nal.kwota = kza;
                                        nal.data_n = dDokumentu;
                                        nal.czy_proc = false;
                                        nal.vat = 0;
                                        sprx.naleznosc.Add(nal);
                                    }
                                    else
                                    {
                                        theError.description += " koszty zast adw są już odnotowane w sprawie i zostaną pominięte ";
                                    }


                                }
                                // dodanie dok odebr
                                /*select* from (SELECT DISTINCT ident, label, id_status
FROM            dbo.wzor_tree) [refTable]
        where[refTable].[label] = rtrim(?)
                                

 
    */
                                Utils.LogWriter("Próba dodawania statusu dla " + nazwaDok);
                                wzor_tree wzt = wienaContext.wzor_tree.Where(a => a.label == nazwaDok).FirstOrDefault();
                                if (wzt != null && wzt.id_status > 0)
                                {
                                    // zanleziono status
                                    Utils.LogWriter("Dodawanie statusu dla " + nazwaDok);
                                    status_spr sspr = (from ss in wienaContext.status_spr
                                                       join st in wienaContext.status on ss.id_statusu equals st.ident
                                                       where ss.id_sprawy == IdSprawy && ss.czyus == 0 && ss.id_statusu == wzt.id_status
                                                       select ss).FirstOrDefault();
                                    Utils.LogWriter("Nowy status " + nazwaDok);

                                    status_spr ost_sspr = wienaContext.status_spr.Where(a => a.id_sprawy == IdSprawy && a.czyus == 0).OrderByDescending(a => a.od_dnia).ThenByDescending(a => a.ident).FirstOrDefault();

                                    if (sspr == null && (ost_sspr == null || (ost_sspr != null && ost_sspr.id_statusu != 9 && ost_sspr.id_statusu != 11 && ost_sspr.id_statusu != 108)))
                                    {
                                        sspr = new status_spr();
                                        sspr.id_statusu = Convert.ToInt32(wzt.id_status);
                                        sspr.czyus = 0;
                                        sspr.od_dnia = dOtrzymania;
                                        sspr.id_sprawy = IdSprawy;
                                        sspr.d_kreacji = DateTime.Now;
                                        wienaContext.status_spr.Add(sspr);
                                        Utils.LogWriter("Dodano status ");
                                    }
                                  
                                    /*select status_spr.ident, status_spr.id_statusu, status_spr.id_sprawy from status_spr inner join status on status_spr.id_statusu = status.ident   where status_spr.czyus = 0 and status_spr.id_statusu > 2 and status.etap = 2*/

                                    dok_odebr doo;
                                    bool isNew = true;
                                    int id_naz = dok_naz.ident;
                                    doo = (from d in wienaContext.dok_odebr
                                           where d.d_dok == dDokumentu && d.id_dok_typ == id_naz && d.id_sprawy == IdSprawy && d.czyus == 0
                                           select d).FirstOrDefault();
                                    if (doo == null)
                                    {
                                        doo = new dok_odebr();
                                        isNew = true;
                                    }
                                    doo.czyus = 0;
                                    doo.czyzalatw = 0;
                                    doo.data_r = dOtrzymania;
                                    //doo.id_sprawy = IdSprawy;
                                    //doo.dok_odebr_nazwy
                                    doo.rok = Convert.ToInt16(dDokumentu.Year);
                                    doo.d_dok = dDokumentu;
                                    doo.d_kreacji = DateTime.Now;
                                    doo.id_dok = wzt.ident;
                                    dok_naz.dok_odebr.Add(doo);

                                    if (isNew)
                                    {
                                        sprx.dok_odebr.Add(doo);
                                        Utils.LogWriter("Dodano dokument do sprawy ");
                                    }
                                    else
                                    {
                                        Utils.LogWriter("Zaktualizowano dokument ");
                                    }

                                }
                                else
                                {
                                    dok_odebr doo;
                                    bool isNew = true;
                                    int id_naz = dok_naz.ident;
                                    doo = (from d in wienaContext.dok_odebr
                                           where d.d_dok == dDokumentu && d.id_dok_typ == id_naz && d.id_sprawy == IdSprawy && d.czyus == 0
                                           select d).FirstOrDefault();
                                    if (doo == null)
                                    {
                                        doo = new dok_odebr();
                                        isNew = true;
                                    }
                                    doo.czyus = 0;
                                    doo.czyzalatw = 0;
                                    doo.data_r = dOtrzymania;
                                    //doo.id_sprawy = IdSprawy;
                                    //doo.dok_odebr_nazwy
                                    doo.rok = Convert.ToInt16(dDokumentu.Year);
                                    doo.d_dok = dDokumentu;
                                    doo.d_kreacji = DateTime.Now;
                                    doo.id_dok_typ = id_naz;
                                    //dok_naz.dok_odebr.Add(doo);
                                    sprx.dok_odebr.Add(doo);

                                    if (isNew)
                                    {
                                        sprx.dok_odebr.Add(doo);
                                        Utils.LogWriter("Dodano dokument do sprawy ");
                                    }
                                    else
                                    {
                                        Utils.LogWriter("Zaktualizowano dokument ");
                                    }
                                    status_spr sspr = (from ss in wienaContext.status_spr
                                                       join st in wienaContext.status on ss.id_statusu equals st.ident
                                                       where ss.id_sprawy == IdSprawy && ss.czyus == 0 && ss.id_statusu > 2 && st.etap == 2
                                                       select ss).FirstOrDefault();
                                    status_spr ost_sspr = wienaContext.status_spr.Where(a => a.id_sprawy == IdSprawy && a.czyus == 0).OrderByDescending(a => a.od_dnia).ThenByDescending(a => a.ident).FirstOrDefault();

                                    if (sspr == null && (ost_sspr == null || (ost_sspr != null && ost_sspr.id_statusu != 9 && ost_sspr.id_statusu != 11 && ost_sspr.id_statusu != 108)))
                                    {
                                        sspr = new status_spr();
                                        sspr.id_statusu = 14;
                                        sspr.czyus = 0;
                                        sspr.od_dnia = dOtrzymania;
                                        sspr.id_sprawy = IdSprawy;
                                        sspr.d_kreacji = DateTime.Now;
                                        wienaContext.status_spr.Add(sspr);


                                    }
                                }
                                // dodanie pisma w LexEna


                                // status_spr ss = wienaContext.status_spr.Where(a => a.id_sprawy == IdSprawy && a.czyus == 0 && a.id_statusu == 

                                // dodanie dokumentu odebranego - czynności
                                Utils.LogWriter("Dodawanie dok. do lexena ");
                                Sprawa spLexena = lexenaContext.Sprawa.Where(a => a.sygnatura == sygnatura).FirstOrDefault();
                                if (spLexena != null)
                                {
                                    DokOdebr dox = spLexena.DokOdebr.Where(a => a.Sprawa_id == spLexena.id && a.DataDokumentu == dDokumentu && a.Nazwa == nazwaDok).FirstOrDefault();
                                    if (dox == null)
                                    {
                                        dox = new DokOdebr();
                                        dox.d_kreacji = DateTime.Now;
                                        dox.Sprawa_id = spLexena.id;
                                        dox.DataDokumentu = dDokumentu;
                                        dox.DataRejestracji = dOtrzymania;
                                        dox.TypDok = 10002; // dokument komorniczy 
                                        dox.PartitionKey = 0;

                                        lexenaContext.DokOdebr.AddObject(dox);
                                        Utils.LogWriter("Dodanie dok. do lexena ");
                                    }


                                }
                                lexenaContext.SaveChanges();
                                Utils.LogWriter(" Dodano dok do LexEna: " + sygnatura);

                            }

                            // czy jestnazwa w mapowaniach 
                            //  wienaContext.zlecenia_ksiegowe.Add(zlc);
                            wienaContext.SaveChanges();
                            Utils.LogWriter(" Import Wiena OK:  " + sygnatura);

                            this.addError(theError.level, theError.code, theError.description, theError.reference);
                        }
                    }

                }
                else
                {
                    this.addError(ErrLevel.Fatal, -501, "Zbiór wejściowy jest pusty");
                    Utils.LogWriter("Błąd w trakcie odczytu zbioru " + importFileName + " jest pusty");
                    return true;
                }


                return true;
            }
            catch (Exception ex)
            {

                Utils.LogWriter("Błąd w trakcie wczytywania czynności komorniczych -  " + ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "") + " zbiór" + this.importFileName);
                this.addError(ErrLevel.Fatal, -503, "Błąd podczas wczytywania czynności komorniczych " + ex.Message + ((ex.InnerException != null) ? ex.InnerException.Message : ""), "");
                return false;

            }



        }


        public bool updateExport(impDescr idesc, string username, int Jednostka_Id)
        {
            try
            {
                using (LexEnaMeritumEntities theContext = new LexEnaMeritumEntities())
                {

                    Import imp = new Import();
                    imp.DataTransferu = DateTime.Now;
                    imp.ContentType = idesc.ContentType;
                    imp.FileType = 1; // xml
                    imp.ImpExp = idesc.ImpExp;
                    imp.JednostkaWindykacji_Id = Jednostka_Id;
                    imp.StatusOperacji = 0;
                    imp.UserName = username;
                    if (idesc.impdt != null)
                        foreach (impdet idet in idesc.impdt)
                        {
                            ImportDetail idt = new ImportDetail();
                            idt.DataImportu = DateTime.Now;
                            idt.ErrLevel = (int)ErrLevel.OK;
                            idt.Sygnatura = idet.Sygnatura;
                            idt.Import = imp;

                        }
                    theContext.Import.AddObject(imp);
                    theContext.SaveChanges();


                }
                return true;
            }

            catch (Exception ex)
            {
                this.errDescription = "Błąd podczas zapisu informacji o eksporcie pozwów " + ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "");
                return false;
            }

        }




    }
}