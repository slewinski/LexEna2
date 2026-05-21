using System;
using System.Net;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ZadanieTimer
{



    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typNakaz
    {

        private ulong iDNakazuField;

        private string sygnaturaField;

        private string dataNakazuField;

        private int kodDecyzjiField;
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
        public ulong IDNakazu
        {
            get
            {
                return this.iDNakazuField;
            }
            set
            {
                if (this.iDNakazuField != value)
                {
                    this.iDNakazuField = value;
                    this.OnPropertyChanged("IDNakazu");
                }
            }
        }

        /// <remarks/>
        public string Sygnatura
        {
            get
            {
                return this.sygnaturaField;
            }
            set
            {
                if (this.sygnaturaField != value)
                {
                    this.sygnaturaField = value;
                    this.OnPropertyChanged("Sygnatura");
                }
            }
        }

        /// <remarks/>
        public string DataNakazu
        {
            get
            {
                return this.dataNakazuField;
            }
            set
            {
                if (this.dataNakazuField != value)
                {
                    this.dataNakazuField = value;
                    this.OnPropertyChanged("DataNakazu");
                };
            }
        }

        /// <remarks/>
        public int KodDecyzji
        {
            get
            {
                return this.kodDecyzjiField;
            }
            set
            {
                if (this.kodDecyzjiField != value)
                {
                    this.kodDecyzjiField = value;
                    this.OnPropertyChanged("KodDecyzji");

                }
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typKomornikWniosek
    {

        private string nazwaField;

        private ulong idField;
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong ID
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.currenda.pl/epu", IsNullable = false)]
    public partial class WniosekEgzekucyjny
    {

        private typKomornikWniosek komornikField;

        private typSadEPU sadField;

        private typNakaz nakazField;

        private typKlauzula klauzulaField;

        private typSkladajacy osobaSkladajacaField;

        private ObservableCollection<typWierzyciel> listaWierzycieliField;

        private ObservableCollection<typDluznik> listaDluznikowField;

        private ObservableCollection<typRoszczenieNakaz> listaRoszczenField;

        private string informacjeDodatkoweField;

        private typKoszty kosztyZastepstwaField;

        private typKoszty inneKosztyField;

        private int zleceniePoszukiwaniaMajatkuField;

        private bool zleceniePoszukiwaniaMajatkuFieldSpecified;

        private int zlecenieProwadzeniaArt85Field;

        private bool zlecenieProwadzeniaArt85FieldSpecified;

        private ulong idField;

        private string versionField;

        private string dataWnioskuField;
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

        public WniosekEgzekucyjny()
        {
            this.versionField = "1.0";
        }

        /// <remarks/>
        public typKomornikWniosek Komornik
        {
            get
            {
                return this.komornikField;
            }
            set
            {
                if (this.komornikField != value)
                {
                    this.komornikField = value;
                    this.OnPropertyChanged("Komornik");
                }
            }
        }

        /// <remarks/>
        public typSadEPU Sad
        {
            get
            {
                return this.sadField;
            }
            set
            {
                if (this.sadField != value)
                {
                    this.sadField = value;
                    this.OnPropertyChanged("Sad");
                }

            }
        }

        /// <remarks/>
        public typNakaz Nakaz
        {
            get
            {
                return this.nakazField;
            }
            set
            {
                if (this.nakazField != value)
                {
                    this.nakazField = value;
                    this.OnPropertyChanged("Nakaz");
                }
            }
        }

        /// <remarks/>
        public typKlauzula Klauzula
        {
            get
            {
                return this.klauzulaField;
            }
            set
            {
                if (this.klauzulaField != value)
                {
                    this.klauzulaField = value;
                    this.OnPropertyChanged("Klauzula");
                }
            }
        }

        /// <remarks/>
        public typSkladajacy OsobaSkladajaca
        {
            get
            {
                return this.osobaSkladajacaField;
            }
            set
            {
                if (this.osobaSkladajacaField != value)
                {
                    this.osobaSkladajacaField = value;
                    this.OnPropertyChanged("OsobaSkladajaca");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Wierzyciel", IsNullable = false)]
        public ObservableCollection<typWierzyciel> ListaWierzycieli
        {
            get
            {
                return this.listaWierzycieliField;
            }
            set
            {
                if (this.listaWierzycieliField != value)
                {
                    this.listaWierzycieliField = value;
                    this.OnPropertyChanged("ListaWierzycieli");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Dluznik", IsNullable = false)]
        public ObservableCollection<typDluznik> ListaDluznikow
        {
            get
            {
                return this.listaDluznikowField;
            }
            set
            {
                if (this.listaDluznikowField != value)
                {
                    this.listaDluznikowField = value;
                    this.OnPropertyChanged("ListaDluznikow");
                }

            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Roszczenie", IsNullable = false)]
        public ObservableCollection<typRoszczenieNakaz> ListaRoszczen
        {
            get
            {
                return this.listaRoszczenField;
            }
            set
            {
                if (this.listaRoszczenField != value)
                {
                    this.listaRoszczenField = value;
                    this.OnPropertyChanged("ListaRoszczen");
                }

            }
        }

        /// <remarks/>
        public string InformacjeDodatkowe
        {
            get
            {
                return this.informacjeDodatkoweField;
            }
            set
            {
                if (this.informacjeDodatkoweField != value)
                {
                    this.informacjeDodatkoweField = value;
                    this.OnPropertyChanged("InformacjeDodatkowe");
                }
            }
        }

        /// <remarks/>
        public typKoszty KosztyZastepstwa
        {
            get
            {
                return this.kosztyZastepstwaField;
            }
            set
            {
                if (this.kosztyZastepstwaField != value)
                {
                    this.kosztyZastepstwaField = value;
                    this.OnPropertyChanged("KosztyZastepstwa");
                }
            }
        }

        /// <remarks/>
        public typKoszty InneKoszty
        {
            get
            {
                return this.inneKosztyField;
            }
            set
            {
                if (this.inneKosztyField != value)
                {
                    this.inneKosztyField = value;
                    this.OnPropertyChanged("InneKoszty");
                }
            }
        }

        /// <remarks/>
        public int ZleceniePoszukiwaniaMajatku
        {
            get
            {
                return this.zleceniePoszukiwaniaMajatkuField;
            }
            set
            {
                if (this.zleceniePoszukiwaniaMajatkuField != value)
                {
                    this.zleceniePoszukiwaniaMajatkuField = value;
                    this.OnPropertyChanged("ZleceniePoszukiwaniaMajatku");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ZleceniePoszukiwaniaMajatkuSpecified
        {
            get
            {
                return this.zleceniePoszukiwaniaMajatkuFieldSpecified;
            }
            set
            {
                if (this.zleceniePoszukiwaniaMajatkuFieldSpecified != value)
                {
                    this.zleceniePoszukiwaniaMajatkuFieldSpecified = value;
                    this.OnPropertyChanged("ZleceniePoszukiwaniaMajatkuSpecified");
                }
            }
        }

        /// <remarks/>
        public int ZlecenieProwadzeniaArt85
        {
            get
            {
                return this.zlecenieProwadzeniaArt85Field;
            }
            set
            {
                if (this.zlecenieProwadzeniaArt85Field != value)
                {
                    this.zlecenieProwadzeniaArt85Field = value;
                    this.OnPropertyChanged("ZlecenieProwadzeniaArt85");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ZlecenieProwadzeniaArt85Specified
        {
            get
            {
                return this.zlecenieProwadzeniaArt85FieldSpecified;
            }
            set
            {
                if (this.zlecenieProwadzeniaArt85FieldSpecified != value)
                {
                    this.zlecenieProwadzeniaArt85FieldSpecified = value;
                    this.OnPropertyChanged("ZlecenieProwadzeniaArt85Specified");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong ID
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                if (this.versionField != value)
                {
                    this.versionField = value;
                    this.OnPropertyChanged("version");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataWniosku
        {
            get
            {
                return this.dataWnioskuField;
            }
            set
            {
                if (this.dataWnioskuField != value)
                {
                    this.dataWnioskuField = value;
                    this.OnPropertyChanged("dataWniosku");
                }
            }
        }
    }

    /// <remarks/>

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.currenda.pl/epu")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.currenda.pl/epu", IsNullable = false)]
    public partial class WnioskiEgzekucyjneEPU
    {

        private ObservableCollection<WniosekEgzekucyjny> wniosekEgzekucyjnyField;

        private string oznaczeniePaczkiField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WniosekEgzekucyjny")]
        public ObservableCollection<WniosekEgzekucyjny> WniosekEgzekucyjny
        {
            get
            {
                return this.wniosekEgzekucyjnyField;
            }
            set
            {
                this.wniosekEgzekucyjnyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string OznaczeniePaczki
        {
            get
            {
                return this.oznaczeniePaczkiField;
            }
            set
            {
                this.oznaczeniePaczkiField = value;
            }
        }
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typKlauzula
    {

        private ulong iDNakazuField;

        private ulong iDKlauzuliField;

        private string sygnaturaField;

        private string dataKlauzuliField;

        private string kodKlauzuliField;
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
        public ulong IDNakazu
        {
            get
            {
                return this.iDNakazuField;
            }
            set
            {
                if (this.iDNakazuField != value)
                {
                    this.iDNakazuField = value;
                    this.OnPropertyChanged("IDNakazu");
                }

            }
        }

        /// <remarks/>
        public ulong IDKlauzuli
        {
            get
            {
                return this.iDKlauzuliField;
            }
            set
            {
                if (this.iDKlauzuliField != value)
                {
                    this.iDKlauzuliField = value;
                    this.OnPropertyChanged("IDKlauzuli");
                }
            }
        }

        /// <remarks/>
        public string Sygnatura
        {
            get
            {
                return this.sygnaturaField;
            }
            set
            {
                if (this.sygnaturaField != value)
                {
                    this.sygnaturaField = value;
                    this.OnPropertyChanged("Sygnatura");
                }
            }
        }

        /// <remarks/>
        public string DataKlauzuli
        {
            get
            {
                return this.dataKlauzuliField;
            }
            set
            {
                if (this.dataKlauzuliField != value)
                {
                    this.dataKlauzuliField = value;
                    this.OnPropertyChanged("DataKlauzuli");
                }
            }
        }

        /// <remarks/>
        public string KodKlauzuli
        {
            get
            {
                return this.kodKlauzuliField;
            }
            set
            {
                if (this.kodKlauzuliField != value)
                {
                    this.kodKlauzuliField = value;
                    this.OnPropertyChanged("KodKlauzuli");
                }
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typRoszczenieNakaz : INotifyPropertyChanged
    {

        private ObservableCollection<typOkresOdsetkowy> odsetkiField;

        private int numerField;

        private string opisKwotyField;

        private decimal wartoscField;

        private typWaluta walutaField;

        private string wartoscSlownieField;

        private string opisField;

        private int odsetkiField1;

        private int solidarnieField;

        private int typField;

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
        [System.Xml.Serialization.XmlArrayItemAttribute("OkresOdsetkowy", IsNullable = false)]
        public ObservableCollection<typOkresOdsetkowy> Odsetki
        {
            get
            {
                return this.odsetkiField;
            }
            set
            {
                if (this.odsetkiField != value)
                {
                    this.odsetkiField = value;
                    this.OnPropertyChanged("Odsetki");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int numer
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
                    this.OnPropertyChanged("numer");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string opisKwoty
        {
            get
            {
                return this.opisKwotyField;
            }
            set
            {
                if (this.opisKwotyField != value)
                {
                    this.opisKwotyField = value;
                    this.OnPropertyChanged("opisKwoty");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal wartosc
        {
            get
            {
                return this.wartoscField;
            }
            set
            {
                if (wartoscField != value)
                {
                    this.wartoscField = value;
                    this.OnPropertyChanged("wartosc");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public typWaluta waluta
        {
            get
            {
                return this.walutaField;
            }
            set
            {
                if (this.walutaField != value)
                {
                    this.walutaField = value;
                    this.OnPropertyChanged("waluta");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string wartoscSlownie
        {
            get
            {
                return this.wartoscSlownieField;
            }
            set
            {
                if (this.wartoscSlownieField != value)
                {
                    this.wartoscSlownieField = value;
                    this.OnPropertyChanged("wartoscSlownie");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string opis
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
                    this.OnPropertyChanged("opis");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int odsetki
        {
            get
            {
                return this.odsetkiField1;
            }
            set
            {
                if (this.odsetkiField1 != value)
                {
                    this.odsetkiField1 = value;
                    this.OnPropertyChanged("odsetki");
                }

            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int solidarnie
        {
            get
            {
                return this.solidarnieField;
            }
            set
            {
                if (this.solidarnieField != value)
                {
                    this.solidarnieField = value;
                    this.OnPropertyChanged("solidarnie");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int typ
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
                    this.OnPropertyChanged("typ");
                }
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typDluznik
    {

        private int rodzajField;

        private object itemField;

        private string nIPField;

        private ObservableCollection<typAdres> adresField;

        private ObservableCollection<typSposobEgzekucjiSposobEgzekucji> listaSposobowField;

        private string uwagiField;

        private ulong idField;
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
        public int rodzaj
        {
            get
            {
                return this.rodzajField;
            }
            set
            {
                if (this.rodzajField != value)
                {
                    this.rodzajField = value;
                    this.OnPropertyChanged("rodzaj");
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
                this.itemField = value;
            }
        }

        /// <remarks/>
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
        [System.Xml.Serialization.XmlElementAttribute("Adres")]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("SposobEgzekucji", IsNullable = false)]
        public ObservableCollection<typSposobEgzekucjiSposobEgzekucji> ListaSposobow
        {
            get
            {
                return this.listaSposobowField;
            }
            set
            {
                if (this.listaSposobowField != value)
                {
                    this.listaSposobowField = value;
                    this.OnPropertyChanged("ListaSposobow");
                }
            }
        }

        /// <remarks/>
        public string Uwagi
        {
            get
            {
                return this.uwagiField;
            }
            set
            {
                if (this.uwagiField != value)
                {
                    this.uwagiField = value;
                    this.OnPropertyChanged("Uwagi");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong ID
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

    /// <remarks/>







    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.currenda.pl/epu")]
    public partial class typSposobEgzekucjiSposobEgzekucji:INotifyPropertyChanged
    {

        private typRodzajSposobu rodzajField;

        private string opisField;

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
        public typRodzajSposobu Rodzaj
        {
            get
            {
                return this.rodzajField;
            }
            set
            {
                if (this.rodzajField != value)
                {
                    this.rodzajField = value;
                    this.OnPropertyChanged("Roddzaj");
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
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public enum typRodzajSposobu
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("z ruchomości")]
        zruchomości,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("z wynagrodzenia za pracę")]
        zwynagrodzeniazapracę,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("z rachunków bankowych")]
        zrachunkówbankowych,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("z wierzytelności")]
        zwierzytelności,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("z nieruchomości")]
        znieruchomości,

        /// <remarks/>
        inny,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typWierzyciel:INotifyPropertyChanged
    {

        private int rodzajField;

        private object itemField;

        private string nIPField;

        private ObservableCollection<typAdres> adresField;

        private string nazwaBankuField;

        private string kontoBankoweField;

        private ulong idField;
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
        public int rodzaj
        {
            get
            {
                return this.rodzajField;
            }
            set
            {
                if (this.rodzajField != value)
                {
                    this.rodzajField = value;
                    this.OnPropertyChanged("rodzaj");
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
        [System.Xml.Serialization.XmlElementAttribute("Adres")]
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
        public string NazwaBanku
        {
            get
            {
                return this.nazwaBankuField;
            }
            set
            {
                if (this.nazwaBankuField != value)
                {
                    this.nazwaBankuField = value;
                    this.OnPropertyChanged("NazwaBanku");
                }
            }
        }

        /// <remarks/>
        public string KontoBankowe
        {
            get
            {
                return this.kontoBankoweField;
            }
            set
            {
                if (this.kontoBankoweField != value)
                {
                    this.kontoBankoweField = value;
                    this.OnPropertyChanged("KontoBankowe");
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong ID
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

   

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.currenda.pl/epu")]
    public partial class typSadEPU:INotifyPropertyChanged
    {

        private string nazwaField;

        private string wydzialField;

        /// <remarks/>
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
    }

        }
    


