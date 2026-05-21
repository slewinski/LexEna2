using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Data;
using System.Web.Hosting;

namespace LexEnaTrs.Web
{

    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

    public static class ExtUtils
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                    //values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
    public class Utils
    {

        public static string DecodeFrom64(string encodedData)
        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;

        }

        public static void LogNamedWriter(string logMesgParam, string fname)
        {
            //Ustawienia ustawienia = new Ustawienia();
            //switch(ustawienia.logowanie)
            //{
            //   case 1:

            using (StreamWriter w = File.AppendText(HostingEnvironment.IsHosted ?  System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/"+fname) : fname))
            {
               
                Log(logMesgParam, w);

                // Close the writer and underlying file.
                w.Close();
            }
            //     break;
            //    case 2:
            //       System.Console.WriteLine(logMesgParam);
            //      break;

            //}

        }

        public static string exceptionMsg(Exception ex)
        {
            return ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message:"");


        }




    public static void LogWriter(string logMesgParam)
            {
                //Ustawienia ustawienia = new Ustawienia();
                //switch(ustawienia.logowanie)
                //{
                //   case 1:
                using (StreamWriter w = File.AppendText(HostingEnvironment.IsHosted ?  System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/LexEnaLog.txt") : "LexEnaLog.txt" ))
                {
                    Log(logMesgParam, w);

                    // Close the writer and underlying file.
                    w.Close();
                  
                }
                
                //     break;
                //    case 2:
                //       System.Console.WriteLine(logMesgParam);
                //      break;

                //}

            }
            private static void Log(string logMessage, TextWriter w)
            {
               // w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
               // w.WriteLine("  :");
               w.WriteLine("{0}\t", logMessage);
               // w.WriteLine("-------------------------------");
                // Update the underlying file.
                w.Flush();
            }

        public static string combineCaseId(int system, string klinumber, string NIPPES)
        {
            return system.ToString() + "/" + klinumber + "/" + NIPPES;

        }

        public static string combineObligationId(int system, string klinumber, string NIPPES)
        {
            return system.ToString() + "/" + klinumber + "/" + NIPPES;

        }

        public static object DeserializeFromString(string objectData, Type type)
        {
            try
            {
                var serializer = new XmlSerializer(type);
                object result;
                using (TextReader reader = new StringReader(objectData))
                { result = serializer.Deserialize(reader); }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
             }
        }


        public static object DeserializeXMLFromString(byte[] content, Type type)
        {
            try
            {

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreProcessingInstructions = true;
                settings.IgnoreWhitespace = true;
                settings.CheckCharacters = false;


                using (var memoryStream = new MemoryStream(content))
                using (XmlReader xr = XmlReader.Create(memoryStream, settings))
                {
                    XmlSerializer ser = new XmlSerializer(type);

                    if (ser.CanDeserialize(xr))
                    {
                        object result = ser.Deserialize(xr);
                        return result;
                    }
                    else
                    {
                        object result = ser.Deserialize(xr);

                        throw new Exception("Błąd podczas deserializacji");

                    }
                }



            }

            catch (Exception ex)
            {

                throw ex;
            }
            
        }



        public static string SerializeToString(object objToSerialize, Type type)
        {
            var output = new MemoryStream();
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
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

        public static string sysNameFromId(int system)
        {
            switch (system)
            {
                case 1: return "AUMS";
                case 2: return "SELEN";
                case 3: return "CC&B";
                case 4: return "SELENPzP";
                case 11: return "WIENA";
                case 12: return "LEXENA";
                default:
                    return "INNY";
            }
             
        }

        private static int CalculateControlSum(string input, int[] weights, int offset = 0)
        {
            int controlSum = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                controlSum += weights[i + offset] * int.Parse(input[i].ToString());
            }
            return controlSum;
        }

        public static bool IsValidNIP(string input)
        {
            int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
            bool result = false;
            try
            {
                if (input.Length == 10 && input != "0000000000")
                {
                    int controlSum = CalculateControlSum(input, weights);
                    int controlNum = controlSum % 11;
                    if (controlNum == 10)
                    {
                        controlNum = 0;
                    }
                    int lastDigit = int.Parse(input[input.Length - 1].ToString());
                    result = controlNum == lastDigit;
                }
                return result;
            }
            catch (Exception x)
            {
                return false;
            }
         
        }


        public static bool IsValidPesel(string szPesel)
        {
            byte[] tab = new byte[10] { 9, 7, 3, 1, 9, 7, 3, 1, 9, 7 };
            byte[] tablicz = new byte[] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57 };
            bool bResult = false;
            int suma = 0;
            int sumcontrol = 0;
            try {
                szPesel = szPesel.Trim();

                if (szPesel.Length == 11)
                {
                    foreach (char l in szPesel)
                    {
                        byte b = Convert.ToByte(l);
                        if (Array.IndexOf(tablicz, Convert.ToByte(l)) == -1) return false;
                    }

                    sumcontrol = Convert.ToInt32(szPesel[10].ToString());

                    for (int i = 0; i < 10; i++)
                    {
                        suma += tab[i] * Convert.ToInt32(szPesel[i].ToString());
                    }

                    bResult = ((suma % 10) == sumcontrol);

                    if (bResult)
                    {
                        int rok = 0;
                        int mies = 0;
                        int dzien = Convert.ToInt32(szPesel[4].ToString()) * 10 + Convert.ToInt32(szPesel[5].ToString());

                        if (szPesel[2] == '0' || szPesel[2] == '1')
                        {
                            rok = 1900;
                            mies = Convert.ToInt32(szPesel[2].ToString()) * 10 + Convert.ToInt32(szPesel[3].ToString());
                        }
                        else if (szPesel[2] == '2' || szPesel[2] == '3')
                        {
                            rok = 2000;
                            mies = (Convert.ToInt32(szPesel[2].ToString()) * 10 + Convert.ToInt32(szPesel[3].ToString()) - 20);
                        }
                        else if (szPesel[2] == '4' || szPesel[2] == '5')
                        {
                            rok = 2100;
                            mies = (Convert.ToInt32(szPesel[2].ToString()) * 10 + Convert.ToInt32(szPesel[3].ToString()) - 40);
                        }
                        else if (szPesel[2] == '6' || szPesel[2] == '7')
                        {
                            rok = 2200;
                            mies = (Convert.ToInt32(szPesel[2].ToString()) * 10 + Convert.ToInt32(szPesel[3].ToString()) - 60);
                        }
                        else if (szPesel[2] == '8' || szPesel[2] == '9')
                        {
                            rok = 1800;
                            mies = (Convert.ToInt32(szPesel[2].ToString()) * 10 + Convert.ToInt32(szPesel[3].ToString()) - 80);
                        }
                        rok += Convert.ToInt32(szPesel[0].ToString()) * 10 + Convert.ToInt32(szPesel[1].ToString());
                        String szDate = rok.ToString() + "-" + (mies < 10 ? "0" + mies.ToString() : mies.ToString()) + "-" + (dzien < 10 ? "0" + dzien.ToString() : dzien.ToString());
                        DateTime dt;
                        bResult = DateTime.TryParse(szDate, out dt);
                    }
                }
                else return false;

                return bResult;
            }
            catch 
            {
                return false;
            }
            }



    }

}