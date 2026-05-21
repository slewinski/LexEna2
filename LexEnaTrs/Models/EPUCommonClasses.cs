using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using LexEnaTrs;
using System.Text.RegularExpressions;
using Telerik.Windows.Controls;
using LexEnaTrs.Web;
using System.ServiceModel.DomainServices.Client;
using LexEnaTrs.Views;
using System.Windows.Controls;
using System.Net;
using System.ComponentModel;

namespace LexEnaTrs
{
  

    public class GetDBRowEventArgs : EventArgs
    {
        private object _obj;

        private int _status;
        private typAdresat tpadrs;
        public object DbRow
        {

            get { return this._obj; }
            set { this._obj = value; }
        }

        public int Status
        {

            get { return this._status; }
            set { this._status = value; }
        }

    }

    public class CustomLocalizationManager : LocalizationManager
    {
        public override string GetStringOverride(string key)
        {
            switch (key)
            {

                case "GridViewFilterIsNull":
                    return "Równą null";
                case "GridViewFilterIsNotNull":
                    return "Różną od null";
                case "GridViewFilterIsEmpty":
                    return "O wartości pusty ";
                case "GridViewFilterIsNotEmpty":
                    return "Jest niepusty";

                case "GridViewGroupPanelText":
                    return "Przeciągnij nagłówek kolumny i upuść w tym miejscu";
                //---------------------- RadGridView Filter Dropdown items texts:
                case "GridViewGroupPanelHeader":
                    return "Grupowanie wg:";
                case "GridViewClearFilter":
                    return "Wyczyść filtr";
                case "GridViewFilterShowRowsWithValueThat":
                    return "Pokaż wiersze z zawartością:";
                case "GridViewFilterSelectAll":
                    return "Zaznacz wszytko";
                case "GridViewFilterContains":
                    return "Zawierającą";
                case "GridViewFilterDoesNotContain":
                    return "Nie zawierającą";
                case "GridViewFilterEndsWith":
                    return "Kończącą się na";
                case "GridViewFilterIsNotContainedIn":
                    return "Nie zawartą w";
                case "GridViewFilterIsContainedIn":
                    return "Zawartą w";
                case "GridViewFilterIsEqualTo":
                    return "Równą";
                case "GridViewFilterIsGreaterThan":
                    return "Większą niż";
                case "GridViewFilterIsGreaterThanOrEqualTo":
                    return "Większa lub równą niż";
                case "GridViewFilterIsLessThan":
                    return "Mniejszą niż";
                case "GridViewFilterIsLessThanOrEqualTo":
                    return "Mniejszą lub równą niż";
                case "GridViewFilterIsNotEqualTo":
                    return "Różną niż";
                case "GridViewFilterStartsWith":
                    return "Rozpoczynającą się od";
                case "GridViewFilterAnd":
                    return " I ";
                case "GridViewFilterOr":
                    return " Lub ";
                case "GridViewFilter":
                    return "Filtruj";
// RadDataPager
                case "CurrentPageText":
                    return "Strona";
                case "LabelText":
                    return "Rozmiar";

                case "FirstButtonText":
                    return "Pierwsza";
                case "LastButtonText":
                    return "Ostatnia";
                
        case "NextButtonText":
                    return "Następna";

    case "PageSizeSubmitButtonText":
                    return "Zmień";

    case "PageSizeText":
                    return "Rozmiar strony";

        case "PrevButtonText":
                    return "Poprzednia";

        case "ReservedResource":
                    return " Please do not remove this key.";

            case "SliderDecreaseText":
                    return "Zmniejsz";

            case "SliderDragText":
                    return "Przeciągnij";

                case "SliderIncreaseText":
                    return "Powiększ";

                case "SubmitButtonText":
                    return "Idź do";

                    case "TotalPageText":
                    return "z";

                case "RadDataPagerEllipsisString":
                    return "...";
                case "RadDataPagerPage":
                    return "Strona";
                case  "RadDataPagerOf":
                    return "z";
                case "EnterDate":
                    return "Wprowadź Datę";

             

                case "DataForm_MoveCurrentToFirst":
                    return "Przejdź do początku";
                case "DataForm_MoveCurrentToPrevious":
                    return "Przejdź do poprzedniego";
                case "DataForm_MoveCurrentToNext":
                    return "Przejdź do następnego";
                case "DataForm_MoveCurrentToLast":
                    return "Przejdź na koniec";
                case "DataForm_AddNew":
                    return "Dodaj";
                case "DataForm_BeginEdit":
                    return "Edytuj";
                case "DataForm_Delete":
                    return "Usuń";


            }
            return base.GetStringOverride(key);
        }
    }

    public class ToXMLSerializers
    {

        private static string CleanEmptyTags(String xml, string tagName)
        {

            if (tagName.Length > 0)
            {
                Regex regex = new Regex(@"(\s)*<curr:" + tagName + @"*(\s)*/>");
                return regex.Replace(xml, string.Empty);
            }
            else
            {
                
                Regex regex = new Regex(@"(\s)*<curr:(\w)*(\s)*/>");
                return regex.Replace(xml, string.Empty);
            }

        }

        public static string ReplaceNamespace(string inxml ,  bool direction)
        {

          
            if (direction == true)
                return inxml.Replace(Constants.oldnamespace, Constants.newnamespace);
            else
                return inxml.Replace(Constants.newnamespace, Constants.oldnamespace);





        }

        public static string SerializePozew(PozewEPU pozew, int generatemode)
        {
            var output = new MemoryStream();
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true, OmitXmlDeclaration = (generatemode > 0 ? true : false) };
            string outputString;
            if (pozew.ListaDowodow != null)
            {
                foreach (typDowod d in pozew.ListaDowodow)
                {
                    if (d.DataDowodu != null && d.DataDowodu == "")
                    {
                        d.DataDowodu = null;
                    } 


                }

            }
            try
            {
                if (Constants.currnamespace == Constants.oldnamespace) // jeśli stare
                {
                    pozew.CzyWniosekUmorzBrakPodstSpecified = false;
                    pozew.CzyWniosekUmorzUchylenieSpecified = false;
                    pozew.version = "2.0";


                }
                else
                {
                    pozew.CzyWniosekUmorzBrakPodstSpecified = true;
                    pozew.CzyWniosekUmorzUchylenieSpecified = true;
                    pozew.version = "2.0";

                }

                using (var xmlWriter = XmlWriter.Create(output, settings))
                {
                    var serializer = new XmlSerializer(typeof(PozewEPU));
                    var namespaces = new XmlSerializerNamespaces();
                    xmlWriter.WriteStartDocument();
                    //xmlWriter.WriteDocType("Field1", null, "someObject.dtd", null); 
                    namespaces.Add("curr", Constants.currnamespace);
                    // tu trzeba podmienić wszytkie typy rejestow w osobach prawnych Inny rejstr = 0 , teraz jest 2
                    serializer.Serialize(xmlWriter, pozew, namespaces);
                }
                output.Seek(0L, SeekOrigin.Begin);


                // zamina stream na string
                var reader = new StreamReader(output);
                outputString = reader.ReadToEnd();
                outputString = ToXMLSerializers.ReplaceNamespace(outputString, Constants.currnamespace == Constants.newnamespace);
                outputString = CleanEmptyTags(outputString, "REGON");
                outputString = CleanEmptyTags(outputString, "PESEL");
                outputString = CleanEmptyTags(outputString, "NIP");
                outputString = CleanEmptyTags(outputString, "dataDowodu");

                return outputString;
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew( ex,"Błąd serializacji pozwu ");
                return "";
            }
            }


        public static XmlSerializer GetEntityXmlSerializer<TEntity>()
         where TEntity : Entity
        {
            XmlAttributes ignoreAttribute = new XmlAttributes()
            {
                XmlIgnore = true,
            };

            // use base class of Entity, 
            // if you use type of implementation 
            // you will get the error.
            Type entityType = typeof(Entity);

            var xmlAttributeOverrides = new XmlAttributeOverrides();
            xmlAttributeOverrides.Add(entityType, "EntityConflict", ignoreAttribute);
            xmlAttributeOverrides.Add(entityType, "EntityState", ignoreAttribute);

            return new XmlSerializer(typeof(TEntity), xmlAttributeOverrides);
        }


        public static string SerializeEntity(object objToSerialize, Type type)
        {
            var output = new MemoryStream();
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
            string outputString;
            XmlSerializer serializer= null;
            // 
            string typeName = type.ToString();
            typeName = typeName.Substring(typeName.LastIndexOf('.')+1);
              
            try
            {
                using (var xmlWriter = XmlWriter.Create(output, settings))
                {
                    switch (typeName)
                    {
                        case "BIGvw_ObligationLastStatus":
                            serializer = GetEntityXmlSerializer<BIGvw_ObligationLastStatus>();
                            break;
                        case "BIGvw_DluznicyAktual":
                            serializer = GetEntityXmlSerializer<BIGvw_DluznicyAktual>();
                            break;
                            
                    }
                     
                    xmlWriter.WriteStartDocument();
                    if (type == typeof(typSprawaOds))
                    {
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");
                        serializer.Serialize(xmlWriter, objToSerialize, ns);
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
            catch (Exception ex)
            {
                throw ex;

            }

        }



        public static string SerializeToString(object objToSerialize, Type type)
        {
            var output = new MemoryStream();
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true};
            string outputString;
            try
            {
                using (var xmlWriter = XmlWriter.Create(output, settings))
                {
                    var serializer = new XmlSerializer(type);
                    xmlWriter.WriteStartDocument();
                    if (type == typeof(typSprawaOds))
                    {
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");
                        serializer.Serialize(xmlWriter, objToSerialize, ns);
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
            catch (Exception ex)
            {
                throw ex;

            }

        }


        public static PozewEPU XmlDeserializeFromString<PozewEPU>(string objectData)
        {

            // korekta  głupich ustawień. 
            return (PozewEPU)XmlDeserializeFromString(objectData, typeof(PozewEPU));
        }
        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            XmlSerializer serializer = null;

            try
            {
                serializer = new XmlSerializer(type);
               
            }
            catch (Exception ee)
            {

                ErrorWindow.CreateNew(ee, "Błąd inicjalizacji");
                return null;
            }
        
            object result;
            try
            {
                using (TextReader reader = new StringReader(objectData))
                { result = serializer.Deserialize(reader); }

                return result;
            }
            catch ( Exception ex)
            {
                ErrorWindow.CreateNew(ex,"Błąd deserializacji");
                return null;

            }
           
        }


    }

    public class DownloadManager
    {

        class WebRequestInfo
        {
            public Stream SourceStream { get; set; }
            public Stream DestinationStream { get; set; }
        }

        public  Button  downloadButton{get;set;} // przycisk wywołujący akcję
        
        public string ServerFileUri { get; set; } // 

        private RadBusyIndicator radBusy;

        public void DownloadnSave(int format)
        {
            /* 0 - doc, 1 - xml */

            if (downloadButton != null)
                downloadButton.IsEnabled = false;

            DaneDoEksportuReady wnd = new DaneDoEksportuReady();
            wnd.Show();
            wnd.Closed += (obj, e) =>
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                switch (format)
                { 
                    case 0:
                          saveDialog.Filter = "Dokumenty MsWord | *.doc";
                          saveDialog.DefaultExt = "doc";
                          break;
                    case 1:
                        saveDialog.Filter = "Dokumenty XML | *.xml";
                        saveDialog.DefaultExt = "xml";
                        break;
                    default:
                        saveDialog.Filter = "Dokumenty MsWord | *.doc";
                        saveDialog.DefaultExt = "doc";
                        break;
                }
                
                if (saveDialog.ShowDialog() == true)
                {
                    // Update UI 

                    // progressBar.Value = 0;

                    // Open the output stream 
                    Stream deststream = saveDialog.OpenFile();
                    // Initiate asynchronous download
                    radBusy = new RadBusyIndicator();
                    radBusy.BusyContent = " Pobieranie zbioru... ";
                    radBusy.IsBusy = true;
                    
                    WebClient wc = new WebClient();
                    wc.AllowReadStreamBuffering = false;
                    wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
                    wc.OpenReadAsync(new Uri(ServerFileUri, UriKind.Absolute),
                                                new WebRequestInfo() { SourceStream = null, DestinationStream = deststream });
                }
                else
                {
                    if (downloadButton != null)
                    downloadButton.IsEnabled = true;
                }
            };

        }
        void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                WebRequestInfo wri = e.UserState as WebRequestInfo;

                wri.SourceStream = e.Result;

                // Do download/save on background thread 
                BackgroundWorker downloadBackgroundWorker = new BackgroundWorker();
                downloadBackgroundWorker.DoWork += downloadBackgroundWorker_DoWork;
                downloadBackgroundWorker.RunWorkerCompleted += downloadBackgroundWorker_RunWorkerCompleted;
                downloadBackgroundWorker.RunWorkerAsync(wri);
            }
            else
            {
                if (downloadButton != null)
                downloadButton.IsEnabled = true;
                radBusy.IsBusy = false;
            }
        }

        void downloadBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Running on background thread here! 

            WebRequestInfo wri = e.Argument as WebRequestInfo;

            // Dowload the entire response stream, writing it to the output stream 
            int buffersize = 16384;
            byte[] buffer = new byte[buffersize];
            int bytesread = wri.SourceStream.Read(buffer, 0, buffersize);
            while (bytesread != 0)
            {
                wri.DestinationStream.Write(buffer, 0, bytesread);
                bytesread = wri.SourceStream.Read(buffer, 0, buffersize);
            }

            e.Result = wri;
        }

        void downloadBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WebRequestInfo wri = e.Result as WebRequestInfo;

            // Cleanup 
            wri.SourceStream.Close();
            wri.DestinationStream.Flush();
            wri.DestinationStream.Close();

            // Update UI 
             if (downloadButton != null)
              downloadButton.IsEnabled = true;
             radBusy.IsBusy = false;
            //progressBar.Value = 100;
        } 

        
        
        
       
 
    
    
    
    
    }



    

    
    


    public  class GetTaleDbRow
    {

     public event EventHandler rowCompleted;


     protected virtual void OnrowCompleted(GetDBRowEventArgs e)
     {
         if (rowCompleted != null)
             rowCompleted(this, e);
     }

        public void GetDocWysXML(int IdDoc)
        {
            LexEnaMeritumDomainContext _context;  //radaDomainDataSource.DomainContext;
            LoadOperation<DokWys> loadop;
            DokWys _dok;
            GetDBRowEventArgs eargs;

            _context = new LexEnaMeritumDomainContext();

            EntityQuery<DokWys> query =
                from c in _context.GetDokWysWithPozewByIdQuery(IdDoc)
                select c;
            try
            {
                loadop = _context.Load(query);
                loadop.Completed += (sender, e) =>
                {
                    _dok = loadop.Entities.FirstOrDefault();
                    if (_dok.Tresc == null && _dok.Pozew != null)
                       _dok.Tresc = _dok.Pozew.FirstOrDefault().Tresc;
                        eargs = new GetDBRowEventArgs();
                        eargs.Status = 200;
                        eargs.DbRow = _dok;
                        OnrowCompleted(eargs);
                    

                };
            }
            catch (Exception ex)
            {
                eargs = new GetDBRowEventArgs();
                eargs.Status = -1;
                OnrowCompleted(eargs);
            
            }
        
        }
       
    }

    public static class base64CodeDecode
    {
        public static string Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        public static string Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }
    }

    
}