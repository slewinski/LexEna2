using System;
using System.Net;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;


namespace ZadanieTimer
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.currenda.pl/epu")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.currenda.pl/epu", IsNullable = false)]
    public partial class Types
    {

        private typRodzajCzynnosci alaField;

        /// <remarks/>
        public typRodzajCzynnosci ala
        {
            get
            {
                return this.alaField;
            }
            set
            {
                this.alaField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public enum typRodzajCzynnosci
    {


        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("podjęcie postępowania egzekucyjnego")]
        podjęciepostępowaniaegzekucyjnego,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("koszty egzekucyjne poniesione przez wierzyciela")]
        kosztyegzekucyjneponiesioneprzezwierzyciela,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("umorzono przez wyegzekwowanie w całości")]
        umorzonoprzezwyegzekwowaniewcałości,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("umorzono przez wyegzekwowanie w części")]
        umorzonoprzezwyegzekwowaniewczęści,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("umorzono w inny sposób")]
        umorzonowinnysposób,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("zwrot wniosku")]
        zwrotwniosku,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("przekazano sprawę")]
        przekazanosprawę,


    }



    public partial class typDowod : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string faktStwierdzanyField;
        private string opisField;
        private int numerField;
        private typRodzajDowodu typDowoduField;
        private string oznaczenieField;
        private String dataDowoduField;
        private bool isSelected;
        private string dataDowStrField;

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



        public string FaktStwierdzany
        {
            get
            {
                return this.faktStwierdzanyField;
            }
            set
            {

                if (value != this.faktStwierdzanyField)
                {
                    this.faktStwierdzanyField = value;
                    this.OnPropertyChanged("FaktStwierdzany");
                }
            }
        }

        /// <remarks/>
        public string Opis
        {
            get
            {
                return this.opisField;
            }
            set
            {
                if (value != this.opisField)
                {
                    this.opisField = value;
                    this.OnPropertyChanged("Opis");
                }

            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("numer")]
        public int Numer
        {
            get
            {
                return this.numerField;
            }
            set
            {
                if (value != this.numerField)
                {
                    this.numerField = value;
                    this.OnPropertyChanged("Numer");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("typDowodu")]
        public typRodzajDowodu TypDowodu
        {
            get
            {
                return this.typDowoduField;
            }
            set
            {

                if (value != this.typDowoduField)
                {
                    this.typDowoduField = value;
                    this.OnPropertyChanged("TypDowodu");
                }
            }


        }

        /// <remarks/>
        /// 

        [System.Xml.Serialization.XmlAttributeAttribute("oznaczenie")]
        public string Oznaczenie
        {
            get
            {
                return this.oznaczenieField;
            }
            set
            {
                if (value != this.oznaczenieField)
                {
                    this.oznaczenieField = value;
                    this.OnPropertyChanged("Oznaczenie");
                }

            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute("dataDowodu")]
        public String DataDowodu
        {
            get
            {
                return this.dataDowoduField;
            }
            set
            {
                if (value != this.dataDowoduField)
                {
                    this.dataDowoduField = value;
                    this.OnPropertyChanged("DataDowodu");
                }

            }
        }
        /// <remarks/>
        /*
         [XmlIgnore]
         public DateTime DataDowodu
         {
             get
             {
                 return this.dataDowoduField;
             }
             set
             {
                 if (value != this.dataDowoduField)
                 {
                     this.dataDowoduField = value;
                     this.OnPropertyChanged("DataDowodu");
                 }
                
             }
         }

         [System.Xml.Serialization.XmlAttributeAttribute("dataDowodu")]
         public string XmlDateTimedataod
         {


             get
             {
                 return this.DataDowodu.ToString("yyyy-MM-dd");
             }
             set { this.DataDowodu = Convert.ToDateTime(value); }
         }
        

         */



        [XmlIgnore]
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (value != this.isSelected)
                {
                    this.isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }

            }
        }
        public typDowod()
        {

        }



    }







    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public enum typRodzajDowodu
    {

        /// <remarks/>
        inny,

        /// <remarks/>
        umowa,

        /// <remarks/>
        faktura,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("akt notarialny")]
        aktnotarialny,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typOkresOdsetkowy : INotifyPropertyChanged
    {
        private DateTime dataOdField;
        private DateTime dataDoField;
        private decimal kwotaField;
        private int czyUstawoweField;
        private decimal stopaField;
        private int okresField;
        private int odWniesieniaField;
        private int doZaplatyField;
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


        /// <remarks/>
        /// 
        public typOkresOdsetkowy()
        {
            // DataOd = System.DateTime.Now;
            // DataDo = System.DateTime.Now;

        }


        [System.Xml.Serialization.XmlAttributeAttribute("doZaplaty")]
        public int Do_Zaplaty
        {
            get
            {
                return this.doZaplatyField;
            }
            set
            {

                if (value != this.doZaplatyField)
                {
                    this.doZaplatyField = value;
                    this.OnPropertyChanged("Do_Zaplaty");
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute("dataOd")]
        public string XmlDateTimedataod
        {


            get
            {
                return this.DataOd.ToString("yyyy-MM-dd");
            }
            set { this.DataOd = Convert.ToDateTime(value); }
        }
        [XmlIgnore]
        public DateTime DataOd
        {
            get
            {
                return this.dataOdField;
            }
            set
            {

                if (value != this.dataOdField)
                {
                    this.dataOdField = value;
                    this.OnPropertyChanged("DataOd");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("dataDo")]
        public string XmlDateTimedatado
        {
            get
            {
                if (this.Do_Zaplaty == 1) return null;
                if (this.DataDo == null) return null;
                if (this.DataDo < Convert.ToDateTime("1800-01-01")) return null;
                return this.DataDo.ToString("yyyy-MM-dd");
            }
            set { this.DataDo = Convert.ToDateTime(value); }
        }

        [XmlIgnore]
        public DateTime DataDo
        {
            get
            {
                return this.dataDoField;
            }
            set
            {
                if (value != this.dataDoField)
                {
                    this.dataDoField = value;
                    this.OnPropertyChanged("DataDo");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("kwota")]
        public decimal Kwota
        {
            get
            {
                return this.kwotaField;
            }
            set
            {

                if (value != this.kwotaField)
                {
                    this.kwotaField = value;
                    this.OnPropertyChanged("Kwota");
                }
            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("czyUstawowe")]
        public int CzyUstawowe
        {
            get
            {
                return this.czyUstawoweField;
            }
            set
            {

                if (value != this.czyUstawoweField)
                {
                    this.czyUstawoweField = value;
                    this.OnPropertyChanged("CzyUstawowe");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("stopa")]
        public decimal Stopa
        {
            get
            {
                return this.stopaField;
            }
            set
            {
                if (value != this.stopaField)
                {
                    this.stopaField = value;
                    this.OnPropertyChanged("Stopa");
                }
            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("okres")]
        public int Okres
        {
            get
            {
               
                return this.okresField;
            }
            set
            {

                if (value != this.okresField)
                {
                    this.okresField = value;
                    this.OnPropertyChanged("Okres");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("odWniesienia")]
        public int Od_Wniesienia
        {
            get
            {
                return this.odWniesieniaField;
            }
            set
            {

                if (value != this.odWniesieniaField)
                {
                    this.odWniesieniaField = value;
                    this.OnPropertyChanged("Od_Wniesienia");
                }
            }
        }




    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typRoszczenie : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<typOkresOdsetkowy> odsetkiField;
        private ObservableCollection<int> dowodyField;
        private int numerField;
        private decimal wartoscField;
        private typWaluta walutaField;
        private string opisField;
        private int czyodsetkiField1;
        private int solidarnieField;
        private int typField;
        private int rodzajField;  // rodzaj z systemu dziedzinowego






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
        /// <remarks/>



        public typRoszczenie()
        { }
        [System.Xml.Serialization.XmlArrayItemAttribute("OkresOdsetkowy", IsNullable = false)]
        public ObservableCollection<typOkresOdsetkowy> Odsetki
        {
            get
            {
                return this.odsetkiField;
            }
            set
            {
                this.odsetkiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Dowod", IsNullable = false)]
        public ObservableCollection<int> Dowody
        {
            get
            {
                return this.dowodyField;
            }
            set
            {
                this.dowodyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("numer")]
        public int Numer
        {
            get
            {
                return this.numerField;
            }
            set
            {
                if (value != this.numerField)
                {
                    this.numerField = value;
                    this.OnPropertyChanged("Numer");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("wartosc")]
        public decimal Wartosc
        {
            get
            {
                return this.wartoscField;
            }
            set
            {

                if (value != this.wartoscField)
                {
                    this.wartoscField = value;
                    this.OnPropertyChanged("Wartosc");

                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("waluta")]
        public typWaluta Waluta
        {
            get
            {
                return this.walutaField;
            }
            set
            {


                if (value != this.walutaField)
                {
                    this.walutaField = value;
                    this.OnPropertyChanged("Waluta");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("opis")]
        public string Opis
        {
            get
            {
                return this.opisField;
            }
            set
            {

                if (value != this.opisField)
                {
                    this.opisField = value;
                    this.OnPropertyChanged("Opis");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("odsetki")]
        public int czyodsetki
        {
            get
            {
                return this.czyodsetkiField1;
            }
            set
            {

                if (value != this.czyodsetkiField1)
                {
                    this.czyodsetkiField1 = value;
                    this.OnPropertyChanged("odsetki");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("solidarnie")]
        public int Solidarnie
        {
            get
            {
                return this.solidarnieField;
            }
            set
            {

                if (value != this.solidarnieField)
                {
                    this.solidarnieField = value;
                    this.OnPropertyChanged("Solidarnie");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("typ")]
        public int Typ
        {
            get
            {
                return this.typField;
            }
            set
            {
                if (value != this.typField)
                {
                    this.typField = value;
                    this.OnPropertyChanged("Typ");
                }
            }
        }

        //[System.Xml.Serialization.XmlAttributeAttribute("rodzaj")]
        [XmlIgnore]
        public int Rodzaj
        {
            get
            {
                return this.typField;
            }
            set
            {
                if (value != this.typField)
                {
                    this.typField = value;
                    this.OnPropertyChanged("Typ");
                }
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public enum typWaluta
    {

        /// <remarks/>
        PLN,

        /// <remarks/>
        USD,

        /// <remarks/>
        EUR,

        /// <remarks/>
        CHF,

        /// <remarks/>
        GBP,

        /// <remarks/>
        AUD,

        /// <remarks/>
        CAD,

        /// <remarks/>
        CZK,

        /// <remarks/>
        DKK,

        /// <remarks/>
        HUF,

        /// <remarks/>
        JPY,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typKoszty : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private decimal wartoscField;
        private int zasadzenieField;
        private int wgNormField;
        private string opisField;


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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("wartosc")]
        public decimal Wartosc
        {
            get
            {
                return this.wartoscField;
            }
            set
            {
                if (this.wartoscField != value)
                {
                    this.wartoscField = value;
                    this.OnPropertyChanged("Wartosc");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("zasadzenie")]
        public int Zasadzenie
        {
            get
            {
                return this.zasadzenieField;
            }
            set
            {
                if (this.zasadzenieField != value)
                {
                    this.zasadzenieField = value;
                    if (this.zasadzenieField == 0) this.WgNorm = 0;

                    this.OnPropertyChanged("Zasadzenie");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("wgNorm")]
        public int WgNorm
        {
            get
            {
                return this.wgNormField;
            }
            set
            {

                if (this.wgNormField != value)
                {
                    this.wgNormField = value;
                    this.OnPropertyChanged("WgNorm");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("opis")]
        public string Opis
        {
            get
            {
                return this.opisField;
            }
            set
            {
                if (this.opisField != value)
                {
                    this.opisField = value;
                    this.OnPropertyChanged("Opis");
                }
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]

    public partial class typOplata : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private decimal wartoscField;

        private int zwolnienieField;

        private int zasadzenieField;

        private long identyfikatorField;

        private bool identyfikatorFieldSpecified;

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

        public typOplata()
        {
            zasadzenieField = 1;

        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("wartosc")]
        public decimal WartoscOplaty
        {
            get
            {
                return this.wartoscField;
            }
            set
            {
                if (this.wartoscField != value)
                {
                    this.wartoscField = value;
                    this.OnPropertyChanged("WartoscOplaty");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("zwolnienie")]
        public int Zwolnienie
        {
            get
            {
                return this.zwolnienieField;
            }
            set
            {
                if (this.zwolnienieField != value)
                {
                    this.zwolnienieField = value;
                    this.OnPropertyChanged("Zwolnienie");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("zasadzenie")]
        public int Zasadzenie
        {
            get
            {
                return this.zasadzenieField;
            }
            set
            {
                if (this.zasadzenieField != value)
                {
                    this.zasadzenieField = value;
                    this.OnPropertyChanged("Zasadzenie");
                }
            }
        }

        /// <remarks/>

        [System.Xml.Serialization.XmlIgnoreAttribute()] //[System.Xml.Serialization.XmlAttributeAttribute()]
        public long identyfikator
        {
            get
            {
                return this.identyfikatorField;
            }
            set
            {
                this.identyfikatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool identyfikatorSpecified
        {
            get
            {
                return this.identyfikatorFieldSpecified;
            }
            set
            {
                this.identyfikatorFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typInnyRejestr
    {

        private string typField;

        private string organField;

        private string numerField;
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
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("typ")]
        public string Typ
        {
            get
            {
                return this.typField;
            }
            set
            {
                if (this.typField != value)
                {
                    this.typField = value;
                    this.OnPropertyChanged("Typ");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Organ
        {
            get
            {
                return this.organField;
            }
            set
            {
                if (this.organField != value)
                {
                    this.organField = value;
                    this.OnPropertyChanged("Organ");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Numer
        {
            get
            {
                return this.numerField;
            }
            set
            {
                if (this.numerField != value)
                {
                    this.numerField = value;
                    this.OnPropertyChanged("Numer");
                }
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typInstytucja : INotifyPropertyChanged
    {

        private string nazwaField;

        private string siedzibaField;

        private string rEGONField;

        private int czyRejestrField;

        private object itemField;
        private typOsoba[] listaZarzaduField;
        private bool czyKrsVisible;
        private bool czyInnyRejVisible;
        private string nazwaStrony;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        public typInstytucja()
        {
            czyRejestrField = 1;
            czyKrsVisible = true;
            czyInnyRejVisible = false;

        }
        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        [XmlIgnore]
        public bool CzyKrsVisible
        {
            get
            {
                return this.czyKrsVisible;
            }
            set
            {
                if (this.czyKrsVisible != value)
                {
                    this.czyKrsVisible = value;
                    this.OnPropertyChanged("CzyKrsVisible");
                }
            }
        }
        [XmlIgnore]
        public bool CzyInnyRejVisible
        {
            get
            {
                return this.czyInnyRejVisible;
            }
            set
            {
                if (this.czyInnyRejVisible != value)
                {
                    this.czyInnyRejVisible = value;
                    this.OnPropertyChanged("CzyInnyRejVisible");
                }
            }
        }
        [XmlIgnore]
        public string NazwaStrony
        {
            get
            {
                return this.nazwaStrony;
            }
            set
            {
                if (this.nazwaStrony != value)
                {
                    this.nazwaStrony = value;
                    this.OnPropertyChanged("NazwaStrony");
                }
            }
        }
        [XmlElement(ElementName = "Nazwa")]
        public string Nazwa
        {
            get
            {
                return this.nazwaField;
            }
            set
            {
                if (this.nazwaField != value)
                {
                    this.nazwaField = value;
                    this.NazwaStrony = value;
                    this.OnPropertyChanged("Nazwa");
                }
            }
        }

        /// <remarks/>
        [XmlElement(ElementName = "Siedziba")]
        public string Siedziba
        {
            get
            {
                return this.siedzibaField;
            }
            set
            {
                if (this.siedzibaField != value)
                {
                    this.siedzibaField = value;
                    this.OnPropertyChanged("Siedziba");
                }
            }
        }

        /// <remarks/>
        //   [XmlElement(IsNullable = false)] 
        [XmlElement(ElementName = "REGON")]
        public string REGON
        {
            get
            {
                return this.rEGONField;
            }
            set
            {
                if (this.rEGONField != value)
                {
                    this.rEGONField = value;
                    this.OnPropertyChanged("REGON");
                }
            }
        }

        /// <remarks/>
        [XmlElement(ElementName = "czyRejestr")]
        public int CzyRejestr
        {
            get
            {
                return this.czyRejestrField;
            }
            set
            {
                if (this.czyRejestrField != value)
                {


                    this.czyRejestrField = value;

                    if (value == 1) // KRS
                    {
                        if (this.Item == null)
                            this.Item = (string)"";
                        else
                            if (this.Item.GetType().FullName == "typInnyRejestr")
                                this.Item = (string)"";
                        CzyKrsVisible = true;
                        CzyInnyRejVisible = false;

                    }
                    else if (value == 2)
                    {
                        if (this.Item == null)
                            this.Item = new typInnyRejestr();
                        else
                            if (this.Item.GetType().FullName == "string")
                                this.Item = new typInnyRejestr();

                        CzyKrsVisible = false;
                        CzyInnyRejVisible = true;

                    }
                    else // brak rejstru
                    {
                        CzyKrsVisible = false;
                        CzyInnyRejVisible = false;
                    }

                    this.OnPropertyChanged("CzyRejestr");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("InnyRejestr", typeof(typInnyRejestr))]
        [System.Xml.Serialization.XmlElementAttribute("KRS", typeof(string))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                if (this.itemField != value)
                {
                    if (value != null) value = value.ToString().Trim();
                    this.itemField = value;
                    this.OnPropertyChanged("Item");
                }
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public typOsoba[] ListaZarzadu
        {
            get
            {
                return this.listaZarzaduField;
            }
            set
            {
                this.listaZarzaduField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typOsoba : INotifyPropertyChanged
    {

        private string imieField;

        private string imie2Field;

        private string nazwiskoField;

        private string pESELField;

        private string stanowiskoField;


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

        [XmlElement(ElementName = "Imie")]
        public string Imie
        {
            get
            {
                return this.imieField;
            }
            set
            {
                if (this.imieField != value)
                {
                    this.imieField = value;
                    this.OnPropertyChanged("Imie");

                }

            }
        }

        /// <remarks/>
        [XmlElement(ElementName = "Imie2")]
        public string Imie2
        {
            get
            {
                return this.imie2Field;
            }
            set
            {
                this.imie2Field = value;
                this.OnPropertyChanged("Imie2");
            }
        }


        [XmlElement(ElementName = "Nazwisko")]
        public string Nazwisko
        {
            get
            {
                return this.nazwiskoField;
            }
            set
            {
                if (this.nazwiskoField != value)
                {
                    this.nazwiskoField = value;
                    this.OnPropertyChanged("Nazwisko");
                }
            }
        }

        [XmlElement(ElementName = "PESEL")]
        public string PESEL
        {
            get
            {
                return this.pESELField;
            }
            set
            {
                this.pESELField = value;
                this.OnPropertyChanged("PESEL");
            }
        }

        /// <remarks/>
        [XmlElement(ElementName = "stanowisko")]
        public string stanowisko
        {
            get
            {
                return this.stanowiskoField;
            }
            set
            {
                this.stanowiskoField = value;
                this.OnPropertyChanged("stanowisko");
            }
        }


    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typOsobaFizyczna : INotifyPropertyChanged
    {

        private string imieField;

        private string imie2Field;

        private string nazwiskoField;

        private string nazwaField;

        private string pESELField;

        private string nrDokumentuField;

        private string imieOjcaField;

        private string imieMatkiField;

        private string nazwiskoRodoweField;

        private string nazwiskoRodoweMatkiField;

        private string miejsceUrodzeniaField;
        private string dataUrodzeniaField;

        private bool czyKrsVisible = false;
        private bool czyInnyRejVisible = false;

        private string nazwaStrony;

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
        /// <remarks/>


        [XmlIgnore]
        public bool CzyKrsVisible
        {
            get
            {
                return this.czyKrsVisible;
            }
            set
            {
                if (this.czyKrsVisible != value)
                {
                    this.czyKrsVisible = value;
                    this.OnPropertyChanged("CzyKrsVisible");
                }
            }
        }

        [XmlIgnore]
        public bool CzyInnyRejVisible
        {
            get
            {
                return this.czyInnyRejVisible;
            }
            set
            {
                if (this.czyInnyRejVisible != value)
                {
                    this.czyInnyRejVisible = value;
                    this.OnPropertyChanged("CzyInnyRejVisible");
                }
            }
        }
        /// <remarks/>
        [XmlIgnore]
        public string NazwaStrony
        {
            get
            {
                return this.nazwaStrony;
            }
            set
            {
                if (this.nazwaStrony != value)
                {
                    this.nazwaStrony = value;
                    this.OnPropertyChanged("NazwaStrony");
                }
            }
        }
        [XmlElement(ElementName = "Imie")]
        public string Imie
        {
            get
            {
                return this.imieField;
            }
            set
            {

                if (this.imieField != value)
                {
                    this.imieField = value;
                    this.NazwaStrony = this.imieField + " " + this.imie2Field + " " + this.nazwiskoField; //System.String.Concat(this.imieField, this.nazwiskoField);  
                    this.OnPropertyChanged("Imie");
                }
            }
        }
        [XmlElement(ElementName = "Imie2")]
        public string Imie2
        {
            get
            {
                return this.imie2Field;
            }
            set
            {
                if (this.imie2Field != value)
                {
                    this.imie2Field = value;
                    this.NazwaStrony = this.imieField + " " + this.imie2Field + " " + this.nazwiskoField;
                    this.OnPropertyChanged("Imie2");
                }
            }
        }

        /// <remarks/>
        [XmlElement(ElementName = "Nazwisko")]
        public string Nazwisko
        {
            get
            {
                return this.nazwiskoField;
            }
            set
            {
                if (this.nazwiskoField != value)
                {
                    this.nazwiskoField = value;
                    this.NazwaStrony = this.imieField + " " + this.imie2Field + " " + this.nazwiskoField;
                    this.OnPropertyChanged("Nazwisko");
                }
            }
        }

        /// <remarks/>
        [XmlElement(ElementName = "Nazwa")]
        public string Nazwa
        {
            get
            {
                return this.nazwaField;
            }
            set
            {
                if (this.nazwaField != value)
                {
                    this.nazwaField = value;
                    this.OnPropertyChanged("Nazwa");
                }
            }
        }

        /// <remarks/>
        [XmlElement(ElementName = "PESEL")]
        public string PESEL
        {
            get
            {
                return this.pESELField;
            }
            set
            {
                if (this.pESELField != value)
                {
                    this.pESELField = value;
                    this.OnPropertyChanged("PESEL");
                }
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string NrDokumentu
        {
            get
            {
                return this.nrDokumentuField;
            }
            set
            {
                this.nrDokumentuField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string ImieOjca
        {
            get
            {
                return this.imieOjcaField;
            }
            set
            {
                this.imieOjcaField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string ImieMatki
        {
            get
            {
                return this.imieMatkiField;
            }
            set
            {
                this.imieMatkiField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string NazwiskoRodowe
        {
            get
            {
                return this.nazwiskoRodoweField;
            }
            set
            {
                this.nazwiskoRodoweField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string NazwiskoRodoweMatki
        {
            get
            {
                return this.nazwiskoRodoweMatkiField;
            }
            set
            {
                this.nazwiskoRodoweMatkiField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string MiejsceUrodzenia
        {
            get
            {
                return this.miejsceUrodzeniaField;
            }
            set
            {
                this.miejsceUrodzeniaField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string DataUrodzenia
        {
            get
            {
                return this.dataUrodzeniaField;
            }
            set
            {
                this.dataUrodzeniaField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typStrona : INotifyPropertyChanged
    {

        private int reprezentacjaField;

        private int rodzajStronyField;

        private object itemField;

        private string nIPField;
        private bool czyNazwaVisible;
        private bool czyFizycznaVisible;
        private bool czyPrawnaVisible;
        private bool czyPowod;
        private string numerKontaField;


        private ObservableCollection<typAdres> adresField;

        private long idField;

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

        public typStrona()
        {
            CzyFizycznaVisible = true;
            CzyPrawnaVisible = false;
            CzyNazwaVisible = false;
            this.Item = new typOsobaFizyczna();
            RodzajStrony = 0;
            this.Adres = new ObservableCollection<typAdres>();
            this.Reprezentacja = 1;
        }
        [XmlIgnore()]
        public bool CzyNazwaVisible
        {
            get
            {
                return this.czyNazwaVisible;
            }
            set
            {
                if (this.czyNazwaVisible != value)
                {
                    this.czyNazwaVisible = value;
                    this.OnPropertyChanged("CzyNazwaVisible");
                }
            }
        }
        [XmlIgnore()]
        public bool CzyFizycznaVisible
        {
            get
            {
                return this.czyFizycznaVisible;
            }
            set
            {
                if (this.czyFizycznaVisible != value)
                {
                    this.czyFizycznaVisible = value;
                    this.OnPropertyChanged("CzyFizycznaVisible");
                }
            }
        }
        [XmlIgnore()]
        public bool CzyPrawnaVisible
        {
            get
            {
                return this.czyPrawnaVisible;
            }
            set
            {
                if (this.czyPrawnaVisible != value)
                {
                    this.czyPrawnaVisible = value;
                    this.OnPropertyChanged("CzyPrawnaVisible");
                }
            }
        }

        [XmlIgnore()]
        public bool CzyPowod
        {
            get
            {
                return this.czyPowod;
            }
            set
            {
                if (this.czyPowod != value)
                {
                    this.czyPowod = value;
                    this.OnPropertyChanged("CzyPowod");
                }
            }
        }
        /// <remarks/>
        [XmlElement(ElementName = "reprezentacja")]
        public int Reprezentacja
        {
            get
            {
                return this.reprezentacjaField;
            }
            set
            {
                if (this.reprezentacjaField != value)
                {
                    this.reprezentacjaField = value;
                    this.OnPropertyChanged("Reprezentacja");
                }
            }
        }



        /// <remarks/>
        [XmlElement(ElementName = "rodzajStrony")]
        public int RodzajStrony
        {

            get
            {
                return this.rodzajStronyField;
            }
            set
            {
                if (this.rodzajStronyField != value)
                {
                    if (this.rodzajStronyField < 2 && value > 1)
                    {
                        this.Item = new typInstytucja();

                    }
                    if (this.rodzajStronyField > 1 && value < 2)
                        this.Item = new typOsobaFizyczna();
                    this.rodzajStronyField = value;
                    switch (value)
                    {
                        case 0: // fizyczna
                            CzyPrawnaVisible = false;
                            CzyNazwaVisible = false;
                            CzyFizycznaVisible = true;

                            break;
                        case 1:
                            CzyPrawnaVisible = false;
                            CzyNazwaVisible = true;
                            CzyFizycznaVisible = true;
                            break;
                        case 2:
                        case 3:
                            CzyPrawnaVisible = true;
                            CzyNazwaVisible = true;
                            CzyFizycznaVisible = false;
                            break;
                        default:
                            //MessageBox.Show("Błędna wartość : rodzaj strony");
                            break;
                    }
                    if (value == 0 || value == 1)
                    {
                        if (this.Item == null)
                            this.Item = new typOsobaFizyczna();
                        else
                            if (this.Item.GetType().FullName == "typInstytucja")
                                this.Item = new typOsobaFizyczna();




                    }
                    else if (value == 2 || value == 3)
                    {
                        if (this.Item == null)
                            this.Item = new typInstytucja();
                        else
                            if (this.Item.GetType().FullName == "typOsobaFizyczna")
                                this.Item = new typInstytucja();


                    }
                    this.OnPropertyChanged("RodzajStrony");

                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Instytucja", typeof(typInstytucja))]
        [System.Xml.Serialization.XmlElementAttribute("OsobaFizyczna", typeof(typOsobaFizyczna))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                if (this.itemField != value)
                {
                    this.itemField = value;
                    this.OnPropertyChanged("Item");
                }
            }
        }

        /// <remarks/>
        [XmlElement(ElementName = "NIP")]
        public string NIP
        {
            get
            {
                return this.nIPField;
            }
            set
            {
                if (this.nIPField != value)
                {
                    this.nIPField = value;
                    this.OnPropertyChanged("NIP");
                }
            }
        }

        /// <remarks/>
        [XmlElementAttribute("Adres")]
        public ObservableCollection<typAdres> Adres
        {
            get
            {
                return this.adresField;
            }
            set
            {
                if (this.adresField != value)
                {
                    this.adresField = value;
                    this.OnPropertyChanged("Adres");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                if (idField != value)
                {
                    this.idField = value;
                    this.OnPropertyChanged("ID");
                }
            }
        }
        [XmlElement(ElementName = "numerKonta")]
        public string numerKonta
        {
            get
            {
                return this.numerKontaField;
            }
            set
            {
                if (this.numerKontaField != value)
                {
                    this.numerKontaField = value;
                    this.OnPropertyChanged("numerKonta");
                }
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typAdres : INotifyPropertyChanged
    {

        private string ulicaField;

        private string nr_domuField;

        private string nr_mieszkaniaField;

        private string kodField;

        private string miejscowoscField;

        private string pocztaField;

        private string krajField;

        private string wojewodztwoField;

        private string powiatField;

        private string gminaField;

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
        /// <remarks/>
        [XmlAttributeAttribute("ulica")]
        public string Ulica
        {
            get
            {
                return this.ulicaField;
            }
            set
            {
                if (this.ulicaField != value)
                {
                    this.ulicaField = value;
                    this.OnPropertyChanged("Ulica");
                }
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute("nr_domu")]
        public string NrDomu
        {
            get
            {
                return this.nr_domuField;
            }
            set
            {
                if (this.nr_domuField != value)
                {
                    this.nr_domuField = value;
                    this.OnPropertyChanged("NrDomu");
                }
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute("nr_mieszkania")]
        public string NrMieszkania
        {
            get
            {
                return this.nr_mieszkaniaField;
            }
            set
            {
                if (this.nr_mieszkaniaField != value)
                {
                    this.nr_mieszkaniaField = value;
                    this.OnPropertyChanged("NrMieszkania");
                }
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute("kod")]
        public string Kod
        {
            get
            {
                return this.kodField;
            }
            set
            {
                if (this.kodField != value)
                {
                    this.kodField = value;
                    this.OnPropertyChanged("Kod");
                }
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute("miejscowosc")]
        public string Miejscowosc
        {
            get
            {
                return this.miejscowoscField;
            }
            set
            {
                if (this.miejscowoscField != value)
                {
                    this.miejscowoscField = value;
                    this.OnPropertyChanged("Miejscowosc");
                }
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute("poczta")]
        public string Poczta
        {
            get
            {
                return this.pocztaField;
            }
            set
            {
                if (this.pocztaField != value)
                {
                    this.pocztaField = value;
                    this.OnPropertyChanged("Poczta");

                }
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute("kraj")]
        public string Kraj
        {
            get
            {
                return this.krajField;
            }
            set
            {
                if (this.krajField != value)
                {
                    this.krajField = value;
                    this.OnPropertyChanged("Kraj");
                }
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute("wojewodztwo")]
        public string Wojewodztwo
        {
            get
            {
                return this.wojewodztwoField;
            }
            set
            {
                if (this.wojewodztwoField != value)
                {
                    this.wojewodztwoField = value;
                    this.OnPropertyChanged("Wojewodztwo");
                }
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute("powiat")]
        public string Powiat
        {
            get
            {
                return this.powiatField;
            }
            set
            {
                if (this.powiatField != value)
                {
                    this.powiatField = value;
                    this.OnPropertyChanged("Powiat");
                }
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string Gmina
        {
            get
            {
                return this.gminaField;
            }
            set
            {
                if (this.gminaField != value)
                {
                    this.gminaField = value;
                    this.OnPropertyChanged("Gmina");
                }
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typSkladajacy : INotifyPropertyChanged
    {

        private typOsoba osobaField;

        private string nazwaField;

        private typAdres adresField;

        private int pelnomocnikField;

        private string podstawaField;

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

        public typOsoba Osoba
        {
            get
            {
                return this.osobaField;
            }
            set
            {
                if (this.osobaField != value)
                {
                    this.osobaField = value;
                    this.OnPropertyChanged("Osoba");
                }
            }
        }

        /// <remarks/>
        public string Nazwa
        {
            get
            {
                return this.nazwaField;
            }
            set
            {
                if (this.nazwaField != value)
                {
                    this.nazwaField = value;
                    this.OnPropertyChanged("Nazwa");
                }
            }
        }

        /// <remarks/>
        public typAdres Adres
        {
            get
            {
                return this.adresField;
            }
            set
            {
                if (this.adresField != value)
                {
                    this.adresField = value;
                    this.OnPropertyChanged("Adres");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int pelnomocnik
        {
            get
            {
                return this.pelnomocnikField;
            }
            set
            {
                if (this.pelnomocnikField != value)
                {
                    this.pelnomocnikField = value;
                    this.OnPropertyChanged("pelnomocnik");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string podstawa
        {
            get
            {
                return this.podstawaField;
            }
            set
            {
                if (this.podstawaField != value)
                {
                    this.podstawaField = value;
                    this.OnPropertyChanged("podstawa");
                }
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]

    [System.Diagnostics.DebuggerStepThroughAttribute()]

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typAdresat : INotifyPropertyChanged
    {

        private string nazwaField;

        private string wydzialField;

        private typAdres adresField;

        private long idField;


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
        /// <remarks/>
        public string Nazwa
        {
            get
            {
                return this.nazwaField;
            }
            set
            {
                if (this.nazwaField != value)
                {
                    this.nazwaField = value;
                    this.OnPropertyChanged("Nazwa");
                }
            }
        }

        /// <remarks/>
        public string Wydzial
        {
            get
            {
                return this.wydzialField;
            }
            set
            {
                if (this.wydzialField != value)
                {
                    this.wydzialField = value;
                    this.OnPropertyChanged("Wydzial");
                }
            }
        }

        /// <remarks/>
        public typAdres Adres
        {
            get
            {
                return this.adresField;
            }
            set
            {
                if (this.adresField != value)
                {
                    this.adresField = value;
                    this.OnPropertyChanged("Adres");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                if (this.idField != value)
                {
                    this.idField = value;
                    this.OnPropertyChanged("ID");
                }
            }
        }



    }

    
        public class MojaSprawaEPU
        {   
            
            /*public int Id { get; set; }
            public DateTime? DataWplywu { get; set; }
            public Decimal? KwotaSporu { get; set; }
            public string RolaWSprawie { get; set; }
            public string StanSprawy { get; set; }
            public string SygnaturaWgPowoda { get; set; }
            public string SygnaturaSprawy { get; set; }
             */
            [XmlElement("Id")]
            public int Id { get; set; }
            [XmlElement("DataWplywu")]
            public DateTime? DataWplywu { get; set; }
            [XmlElement("KwotaSporu")]
            public Decimal? KwotaSporu { get; set; }
            [XmlElement("RolaWSprawie")]
            public string RolaWSprawie { get; set; }
            [XmlElement("StanSprawy")]
            public string StanSprawy { get; set; }
            [XmlElement("SygnaturaWgPowoda")]
            public string SygnaturaWgPowoda { get; set; }
            [XmlElement("SygnaturaSprawy")]
            public string SygnaturaSprawy { get; set; }
        }


    [XmlRoot("MojeSprawyEPU")]
    public class MojeSprawyEPU
    {


      public MojeSprawyEPU()
     {
       mojspr = new List<MojaSprawaEPU>();
      }
       [XmlElement("MojaSprawaEPU")]
        public List<MojaSprawaEPU> mojspr{get;set;}

     }

       
 struct DecyzjaKod 
{ 
    public int KodDecyzji {get;set;}
    public string NazwaDecyzji {get;set;} 
            
   public  DecyzjaKod(string n, int k) :this()
   {
      this.KodDecyzji = k;
      this.NazwaDecyzji = n;
   }
    
}

public  class KodyDecyzji
{
    
    private DecyzjaKod[]  _katalogdecyzji = 
                       {
                       new DecyzjaKod("Zarządzenie o zwrocie pozwu", 1),
                       new DecyzjaKod("Zarządzenie o wezwaniu do uzupełnienia braków.", 2),
                       new DecyzjaKod("Postanowienie o odrzuceniu pozwu", 3),
                       new DecyzjaKod("Postanowienie o braku podstaw do wydania nakazu i przekazaniu wg właściwości", 4),
                       new DecyzjaKod("Nakaz", 5),
                       new DecyzjaKod("Zarządzenie o przyjęciu skargi", 6),
                       new DecyzjaKod("Postanowienie o odrzuceniu skargi", 7),
                       new DecyzjaKod("Zarządzenie o przyjęciu zażalenia", 8),
                       new DecyzjaKod("Postanowienie o odrzuceniu zażalenia", 9),
                       new DecyzjaKod("Zarządzenie o zwrocie opłaty", 10),
                       new DecyzjaKod("Zarządzenie o przyjęciu zażalenia i przekazaniu do sądu II instancji", 11),
                       new DecyzjaKod("Zarządzenie o wezwaniu do uzupełnienia adresu", 12),
                       new DecyzjaKod("Postanowienie o przyjęciu sprzeciwu i przekazaniu wg właściwości", 13),
                       new DecyzjaKod("Postanowienie o odrzuceniu sprzeciwu", 14),
                       new DecyzjaKod("Zarządzenie o przekazaniu wg właściwości", 15),
                       new DecyzjaKod("Postanowienie o uchyleniu nakazu", 16),
                       new DecyzjaKod("Postanowienie o nadaniu klauzuli wykonalności", 17),
                       new DecyzjaKod("Zarządzenie o reklamacji przesyłki", 18),
                       new DecyzjaKod("Zarządzenie inne", 19),
                       new DecyzjaKod("Postanowienie inne", 20),
                       new DecyzjaKod("Postanowienie o połączeniu spraw", 21),
                       new DecyzjaKod("Postanowienie o wyłączeniu sprawy", 22),
                       new DecyzjaKod("Postanowienie o umorzeniu", 23),
                       new DecyzjaKod("Postanowienie o zawieszeniu", 24),
                       new DecyzjaKod("Postanowienie o podjęciu zawieszonego postepowania", 25),
                       new DecyzjaKod("Postanowienie o odrzuceniu wniosku", 26),
                       new DecyzjaKod("Zarządzenie o stwierdzeniu doręczenia", 27),
                       new DecyzjaKod("Zarządzenie o ponownym doręczeniu", 28),
                       new DecyzjaKod("Zarządzenie o stwierdzeniu braku doręczenia", 29),
                       new DecyzjaKod("Zarządzenie o stwierdzeniu doręczenia elektronicznego", 30),
                       new DecyzjaKod("Postanowienie o sprostowaniu omyłki pisarskiej", 31),
                       new DecyzjaKod("Zarządzenie o wezwaniu do uzupełnienia braków środka odwoławczego", 32),
                       new DecyzjaKod("Postanowienie o odrzuceniu skargi i przekazaniu wg właściwości", 33),
                       new DecyzjaKod("Postanowienie o uchyleniu orzeczenia", 34),
                       new DecyzjaKod("Postanowienie dotyczące kosztów", 35),
                       new DecyzjaKod("Postanowienie o uwzględnieniu zażalenia w całości", 101),
                       new DecyzjaKod("Postanowienie o oddaleniu zażalenia", 102),
                       new DecyzjaKod("Postanowienie o odrzuceniu zażalenia", 103),
                       new DecyzjaKod("Zarządzenie o zwrocie akt sądowi I instancji", 104),
                       new DecyzjaKod("Zarządzenie", 105),
                       new DecyzjaKod("Postanowienie", 106),
                       new DecyzjaKod("Postanowienie o uwzględnieniu zażalenia w części", 107),
                       new DecyzjaKod("Stwierdzenie prawomocności", 201),
                       new DecyzjaKod("Stwierdzenie prawomocności i nadanie klauzuli", 202),
                       new DecyzjaKod("Utrata mocy nakazu", 203),
                       new DecyzjaKod("Uchylenie nakazu", 204)
                       };
    
       public string GetDecName(int kod)
        {
            for (int i= 1 ; i < _katalogdecyzji.GetLength(0); i++)
            {
                if (_katalogdecyzji[i].KodDecyzji == kod)
                    return _katalogdecyzji[i].NazwaDecyzji;
            
            }
            return null;
        
        
        }
    
        }

       
}






 /*

 public class DecyzjeWEPU
        {
            public ObservableCollection<DecyzjaKod> _map;
            

            private void loadList()
            {
                if (_map == null)
                    {
                        _map= new ObservableCollection<DecyzjaKod>();

                        _map.Add(new DecyzjaKod("Zarządzenie o zwrocie pozwu", 1));
                        _map.Add(new DecyzjaKod("Zarządzenie o wezwaniu do uzupełnienia braków.", 2));
                        _map.Add(new DecyzjaKod("Postanowienie o odrzuceniu pozwu", 3));
                        _map.Add(new DecyzjaKod("Postanowienie o braku podstaw do wydania nakazu i przekazaniu wg właściwości", 4));
                        _map.Add(new DecyzjaKod("Nakaz", 5));
                        _map.Add(new DecyzjaKod("Zarządzenie o przyjęciu skargi", 6));
                        _map.Add(new DecyzjaKod("Postanowienie o odrzuceniu skargi", 7));
                        _map.Add(new DecyzjaKod("Zarządzenie o przyjęciu zażalenia", 8));
                        _map.Add(new DecyzjaKod("Postanowienie o odrzuceniu zażalenia", 9));
                        _map.Add(new DecyzjaKod("Zarządzenie o zwrocie opłaty", 10));
                        _map.Add(new DecyzjaKod("Zarządzenie o przyjęciu zażalenia i przekazaniu do sądu II instancji", 11));
                        _map.Add(new DecyzjaKod("Zarządzenie o wezwaniu do uzupełnienia adresu", 12));
                        _map.Add(new DecyzjaKod("Postanowienie o przyjęciu sprzeciwu i przekazaniu wg właściwości", 13));
                        _map.Add(new DecyzjaKod("Postanowienie o odrzuceniu sprzeciwu", 14));
                        _map.Add(new DecyzjaKod("Zarządzenie o przekazaniu wg właściwości", 15));
                        _map.Add(new DecyzjaKod("Postanowienie o uchyleniu nakazu", 16));
                        _map.Add(new DecyzjaKod("Postanowienie o nadaniu klauzuli wykonalności", 17));
                        _map.Add(new DecyzjaKod("Zarządzenie o reklamacji przesyłki", 18));
                        _map.Add(new DecyzjaKod("Zarządzenie inne", 19));
                        _map.Add(new DecyzjaKod("Postanowienie inne", 20));
                        _map.Add(new DecyzjaKod("Postanowienie o połączeniu spraw", 21));
                        _map.Add(new DecyzjaKod("Postanowienie o wyłączeniu sprawy", 22));
                        _map.Add(new DecyzjaKod("Postanowienie o umorzeniu", 23));
                        _map.Add(new DecyzjaKod("Postanowienie o zawieszeniu", 24));
                        _map.Add(new DecyzjaKod("Postanowienie o podjęciu zawieszonego postepowania", 25));
                        _map.Add(new DecyzjaKod("Postanowienie o odrzuceniu wniosku", 26));
                        _map.Add(new DecyzjaKod("Zarządzenie o stwierdzeniu doręczenia", 27));
                        _map.Add(new DecyzjaKod("Zarządzenie o ponownym doręczeniu", 28));
                        _map.Add(new DecyzjaKod("Zarządzenie o stwierdzeniu braku doręczenia", 29));
                        _map.Add(new DecyzjaKod("Zarządzenie o stwierdzeniu doręczenia elektronicznego", 30));
                        _map.Add(new DecyzjaKod("Postanowienie o sprostowaniu omyłki pisarskiej", 31));
                        _map.Add(new DecyzjaKod("Zarządzenie o wezwaniu do uzupełnienia braków środka odwoławczego", 32));
                        _map.Add(new DecyzjaKod("Postanowienie o odrzuceniu skargi i przekazaniu wg właściwości", 33));
                        _map.Add(new DecyzjaKod("Postanowienie o uchyleniu orzeczenia", 34));
                        _map.Add(new DecyzjaKod("Postanowienie dotyczące kosztów", 35));
                        _map.Add(new DecyzjaKod("Postanowienie o uwzględnieniu zażalenia w całości", 101));
                        _map.Add(new DecyzjaKod("Postanowienie o oddaleniu zażalenia", 102));
                        _map.Add(new DecyzjaKod("Postanowienie o odrzuceniu zażalenia", 103));
                        _map.Add(new DecyzjaKod("Zarządzenie o zwrocie akt sądowi I instancji", 104));
                        _map.Add(new DecyzjaKod("Zarządzenie", 105));
                        _map.Add(new DecyzjaKod("Postanowienie", 106));
                        _map.Add(new DecyzjaKod("Postanowienie o uwzględnieniu zażalenia w części", 107));
                        _map.Add(new DecyzjaKod("Stwierdzenie prawomocności", 201));
                        _map.Add(new DecyzjaKod("Stwierdzenie prawomocności i nadanie klauzuli", 202));
                        _map.Add(new DecyzjaKod("Utrata mocy nakazu", 203));
                        _map.Add(new DecyzjaKod("Uchylenie nakazu", 204));

                        

                    }

            
            }
            public ObservableCollection<DecyzjaKod> MapaDecyzji
            {
                get
                {
                    loadList();
                    return _map;
                }

            }
            
        public string GetElement(int kod)
        {
          (_map.ElementAt(0) as DecyzjaKod).









        }
                         
           }

     */
            
      





    

    
