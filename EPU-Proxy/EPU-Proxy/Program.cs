using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace EPU_Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            string sciezkaDoPliku = "..\\..\\XMLFile1.xml";

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            settings.Schemas.Add(null, XmlReader.Create(@"..\..\PozewEPU.xsd"));
            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(sciezkaDoPliku, settings);

            // Parse the file. 
            while (reader.Read()) ;







            EpuProxy.EpuProxyClient klient = new EpuProxy.EpuProxyClient();
            klient.setUserData("69642454", "aq1sw2DE#", "1e77a729-f2a2-418d-80e5-e36d7399fd5a");
            
            XmlDocument pozewXML = new XmlDocument();
            pozewXML.Load(@"..\..\XMLFile7.xml");
            StringWriter writer = new StringWriter();
            pozewXML.Save(writer);
           //int wynikp = klient.zlozPozwy(writer.ToString());
           int wynikp= klient.zlozPozwy(pozewXML.InnerXml);
           
            int wynikL = klient.getListaKanceraii();
            int wynik = klient.mojeSprawy(null, null, 0, null);
           

        }
        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                Console.WriteLine("\tValidation error: " + args.Message);

        }

    }
}
