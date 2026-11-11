using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;


namespace LexEnaTrs.Web.Models
{
    // Shared by Silverlight and the server. Keep the existing XML envelope, but send
    // only editable choices; the server already owns the complete billing document.
    public static class BillingImportSelectionSerializer
    {
        public static string ToBase64(IEnumerable<SprImportDescriptor> rows)
        {
            if (rows == null) throw new ArgumentNullException("rows");
            using (var output = new MemoryStream())
            {
                var settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = true
                };
                using (var writer = XmlWriter.Create(output, settings))
                {
                    writer.WriteStartElement("ImportSprawBilling");
                    writer.WriteStartElement("SprawaDescriptor");
                    foreach (var row in rows)
                    {
                        if (row == null) throw new InvalidOperationException("Brak danych pozycji paczki.");
                        writer.WriteStartElement("SprImportDescriptor");
                        writer.WriteElementString("BatchRowNumber", XmlConvert.ToString(row.BatchRowNumber));
                        writer.WriteElementString("czyrej", XmlConvert.ToString(row.czyrej));
                        writer.WriteElementString("CzyUstawowe", XmlConvert.ToString(row.CzyUstawowe));
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                return Convert.ToBase64String(output.ToArray());
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class ImportSprawBilling
    {


        private DateTime dataKsiegowaniaField;
        private string nrDodowuField;
        private string oddzialField;
        private string systemField;
        private int    idOddzialField;
        private int    idKancelariaField;
        private SprImportDescriptor[] sprawaDescriptorField;
       

        /// <remarks/>

       
        /// <remarks/>
        /// 

        [System.Xml.Serialization.XmlArrayItemAttribute("SprImportDescriptor", IsNullable = false)]
        public SprImportDescriptor[] SprawaDescriptor
        {
            get
            {
                return this.sprawaDescriptorField;
            }
            set
            {
                this.sprawaDescriptorField = value;
            }
        }

        public DateTime DataKsiegowania
        {
            get
            {
                return this.dataKsiegowaniaField;
            }
            set
            {
                this.dataKsiegowaniaField = value;
            }
        }
        public string NrDodowu
        {
            get
            {
                return this.nrDodowuField;
            }
            set
            {
                this.nrDodowuField = value;
            }
        }
        public string Oddzial
        {
            get
            {
                return this.oddzialField;
            }
            set
            {
                this.oddzialField = value;
            }
        }
        public int IdOddzial
        {
            get
            {
                return this.idOddzialField;

            }
            set
            {
                this.idOddzialField = value;
            }
        }
        public int IdKancelaria
        {
            get
            {
                return this.idKancelariaField;

            }
            set
            {
                this.idKancelariaField = value;
            }
        }
        public string System
        {
            get
            {
                return this.systemField;
            }
            set
            {
                this.systemField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class SprImportDescriptor:INotifyPropertyChanged

    {
        // Identifiers and summary used by the persisted batch import screen.
        public int BatchRowNumber { get; set; }
        public bool CanImport { get; set; }
        public int WienaCaseId { get; set; }
        public decimal BatchAmount { get; set; }

        private int czyUstawoweField;
        private bool czy_okField;
        private bool czyrejField;
        private string sygn_obciazField;
        private int id_symbolField;
        private int nrField;
        private int rokField;
        private string messageField;
        private Odbiorca odbiorcaField;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }


        public int CzyUstawowe
        {
            get
            {
                return this.czyUstawoweField;
            }
            set
            {
                this.czyUstawoweField = value;
            }
        }

        public bool czy_ok
        {
            get
            {
                return this.czy_okField;
            }
            set
            {
                this.czy_okField = value;
            }
        }
        public bool czyrej
        {
            get
            {
                return this.czyrejField;
            }
            set
            {
                this.czyrejField = value;
                this.OnPropertyChanged("czyrej");
            }
        }


        public string sygn_obciaz
        {
            get
            {
                return this.sygn_obciazField;
            }
            set
            {
                this.sygn_obciazField = value;
            }
        }

        public int id_symbol
        {
            get
            {
                return this.id_symbolField;
            }
            set
            {
                this.id_symbolField = value;
            }
        }

        public int nr
        {
            get
            {
                return this.nrField;
            }
            set
            {
                this.nrField = value;
            }
        }

        public int rok
        {
            get
            {
                return this.rokField;
            }
            set
            {
                this.rokField = value;
            }
        }
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        public Odbiorca Odbiorca
        {
            get
            {
                return this.odbiorcaField;
            }
            set
            {
                this.odbiorcaField = value;
            }
        }
    }




}
