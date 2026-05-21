using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.IO.Compression;
using System.Web.Hosting;


namespace ZadanieTimer
{
    class Utils
    {
        public static void LogWriter(string logMesgParam)
        {
            //Ustawienia ustawienia = new Ustawienia();
            //switch(ustawienia.logowanie)
            //{
            //   case 1:
            try
            {
                using (StreamWriter w = File.AppendText(HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/LexEnaLog.txt") : "LexEnaLog.txt"))
                {
                    Log(logMesgParam, w);

                    // Close the writer and underlying file.
                    w.Close();

                }
            }
            catch (Exception ex)
            {

                ;

            }

            //     break;
            //    case 2:
            //       System.Console.WriteLine(logMesgParam);
            //      break;

            //}

        }

        public static void LogNamedWriter(string logMesgParam, string fname)
        {
            //Ustawienia ustawienia = new Ustawienia();
            //switch(ustawienia.logowanie)
            //{
            //   case 1:
            using (StreamWriter w = File.AppendText(fname))
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
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
            // Update the underlying file.
            w.Flush();
            w.Close();
        }

    }
    
       
    public static class XML2HTMLTransform
    {
        static void CopyStream(System.IO.Stream src, System.IO.Stream dest)
        {
            byte[] buffer = new byte[1024];
            int len = src.Read(buffer, 0, buffer.Length);
            while (len > 0)
            {
                dest.Write(buffer, 0, len);
                len = src.Read(buffer, 0, buffer.Length);
            }
            dest.Flush();
        }

        public static System.IO.MemoryStream StringToMemoryStream(string s)
        {
            byte[] a = System.Text.Encoding.UTF8.GetBytes(s);
            return new System.IO.MemoryStream(a);
        }

        public static String MemoryStreamToString(System.IO.MemoryStream ms)
        {
            byte[] ByteArray = ms.ToArray();
            return System.Text.Encoding.UTF8.GetString(ByteArray);
        }

        public static byte[] Zip(string value)
        {
            //Transform string into byte[]  
            byte[] byteArray = new byte[value.Length];
            byte[] resultArray; 
            byte[] doclen = new byte[4];
            int indexBA = 0;
            foreach (char item in value.ToCharArray())
            {
                byteArray[indexBA++] = (byte)item;
            }

            //Prepare for compress
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Compress);

            //Compress
            sw.Write(byteArray, 0, byteArray.Length);
            //Close, DO NOT FLUSH cause bytes will go missing...
            sw.Close();
            
            
            byteArray =   ms.ToArray();
            resultArray = new byte[byteArray.Length + 4];
            doclen = BitConverter.GetBytes(value.Length);
            
            /*
            byte[] swap = new byte[2];
            swap[0] = doclen[2];
            swap[1] = doclen[3];
            doclen[2] = doclen[0];
            doclen[3] = doclen[1];
            doclen[0] = swap[0];
            doclen[1] = swap[1];
            */

            doclen.CopyTo(resultArray, 0);
            byteArray.CopyTo(resultArray, 4);
            return resultArray;
    /*
            System.Text.StringBuilder sB = new System.Text.StringBuilder(byteArray.Length);
            foreach (byte item in byteArray)
            {
                sB.Append((char)item);
            }
            ms.Close();
            sw.Dispose();
            ms.Dispose();
            return sB.ToString();
     */
        }
        public static string  createPDF(string inhtmlfile, string pdffilename) // odczyt html'a z dokumentu odebranego z EPU       //PozewEPU _pozew) // serializacja pozwu EPU
        {

            string appPath = HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.MapPath(@"~/wkhtml2pdf/") : "";
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            
                
                try
            {
                
                info.WorkingDirectory = appPath;
                info.FileName = "wkhtmltopdf.exe";
                info.Arguments = "-q -s A4 ";
                info.Arguments += inhtmlfile + "  " +pdffilename;
                info.CreateNoWindow = true;
                info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.StartInfo = info;
                proc.Start();
                proc.WaitForExit();

            }
            catch (ArgumentException ex)
            {
                return "Błąd "+ ex.Message ;
            }
            return pdffilename;

        }

        public static string WriteHtmlFile(string htmldoc)
        {
            string OutputFilename;
            string htmlfile;
            StreamWriter stwr;
            string startupPath = HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.MapPath(@"~/wkhtml2pdf/") : "";
            try
            {

            
                OutputFilename =  string.Format(@"{0}.html", Guid.NewGuid());
                htmlfile = startupPath + OutputFilename;
                stwr = new StreamWriter(htmlfile, false, Encoding.UTF8);
                stwr.Write(htmldoc);
                stwr.Close();
                // pdfFname = string.Format(@"{0}.pdf", Guid.NewGuid());
                // pdfFname = System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/" + pdfFname);
                // createPDF(htmlfile, pdfFname);
                return htmlfile;
            }
            catch (Exception ex)
            {
                return "Błąd: " + ex.Message;
            }
        }
        public static string html2pdfSharp(string htmlcontent, ref byte[] pdfcontent) // odczyt html'a z dokumentu odebranego z EPU       //PozewEPU _pozew) // serializacja pozwu EPU
        {
            pdfcontent = null;
            try
            {

                //htmlfname = WriteHtmlFile(htmlcontent);

                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                pdfcontent = htmlToPdf.GeneratePdf(htmlcontent);


            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd podczas tworzenia pdf " + ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                return "Błąd " + ex.Message;
            }
            return "";
        }


        public static string html2pdf(string htmlcontent, ref byte[] pdfcontent) // odczyt html'a z dokumentu odebranego z EPU       //PozewEPU _pozew) // serializacja pozwu EPU
        {

            /*
            string startupPath = HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.MapPath(@"~/wkhtml2pdf/") : "";
            string htmlfname;
            string pdffilename = startupPath + string.Format(@"{0}.pdf", Guid.NewGuid()); ;
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            try
            {

                htmlfname = WriteHtmlFile(htmlcontent);
                if (htmlfname.Contains("Błąd")) return htmlfname;

                
                info.WorkingDirectory = startupPath;
                info.FileName = "wkhtmltopdf.exe";
                info.Arguments = "-q -s A4 ";
                info.Arguments += htmlfname + "  " + pdffilename;
                info.CreateNoWindow = true;
                info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.StartInfo = info;
                proc.Start();
                proc.WaitForExit();
                var fs = new FileStream(pdffilename, FileMode.Open);
                var len = (int)fs.Length;
                pdfcontent = new byte[len];
                fs.Read(pdfcontent, 0, len);
                fs.Close();
                File.Delete(htmlfname);
                File.Delete(pdffilename);

            }
            catch (Exception ex)
            {
                return "Błąd " + ex.Message;
            }
            return pdffilename;
            */
            pdfcontent = null;
            try
            {

                //htmlfname = WriteHtmlFile(htmlcontent);

                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                pdfcontent = htmlToPdf.GeneratePdf(htmlcontent);


            }
            catch (Exception ex)
            {
                Utils.LogWriter("Błąd podczas tworzenia pdf " + ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                return "Błąd " + ex.Message;
            }
            return "";
        }


        public static string TransformNCompress(string sXmlPath, int what)
        {
            string filePath;
            //byte[] compressed;//  = new byte[]();
            string outstring;
            MemoryStream stream;
            string startupPath = HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.MapPath("~/") : "";

            try
            {
                System.IO.StringReader reader = new StringReader(sXmlPath);
                XPathDocument myXPathDoc = new XPathDocument(reader);
                stream = new MemoryStream();
                XslCompiledTransform myXslTrans = new XslCompiledTransform();
                switch (what)
                { 
                    case 0:   // pozew 
                        filePath = startupPath + @"XSLT/Pozew.xslt";
                        break;
                    case 5:
                        filePath = startupPath +  @"XSLT/nakazEPU.xslt";
                        break;
                    case 17: // orzeczenia ( postanowienia )
                    case 101:
                        filePath = startupPath +  @"XSLT/OrzeczenieEPU.xslt";
                        break;
                    case 30:
                        filePath = startupPath +  @"XSLT/WniosekEgzekucyjny.xslt";
                        break;
                    default:
                        filePath = startupPath +  @"XSLT/nakazEPU.xslt";
                        break;
                }
                
                myXslTrans.Load(filePath);
                        
                XmlTextWriter myWriter = new XmlTextWriter(stream, Encoding.UTF8);
                myXslTrans.Transform(myXPathDoc, null, myWriter);
               // stream zawiera html'a
                //compressed = new  byte[stream.Length];
                //compressed = Zip(MemoryStreamToString(stream));

                /*
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (GZipStream tinyStream = new GZipStream(outStream, CompressionMode.Compress))
                    stream.CopyTo(tinyStream);
                    filePath = MemoryStreamToString(outStream);
                    
                    //outStream.Write(compressed, 0, (int)stream.Length);
                    //compressed = outStream.ToArray();

                } 
                */
                outstring = MemoryStreamToString(stream);
                return outstring;
                

            }
            catch (Exception e)
            {

                Console.WriteLine("Exception: {0}", e.ToString());
                return null;
            }

        }



    }

   



}
