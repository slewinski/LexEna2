using System.Xml.Serialization;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Xml.Schema;
using System.IO;
using LexEnaTrs;
using System.ComponentModel.DataAnnotations;

namespace LexEnaTrs
{
   
   // [System.Xml.Serialization.XmlTypeAttribute(Namespace = Constants.currnamespace)]
    public class Zadania:INotifyPropertyChanged
    {
        private string _nazwaZadania;
        private int  _id;
        private DateTime? _dRozpocz;
        private DateTime? _dZakoncz;
        private bool? _oczasie;
        private int _status;
        private string _opis;
        private int _typZadaniaId;
        private string _parametry;
        private int _IdPozwuWLexEna;
        private int _IdZazaleniaWLexEna;
        private int _IdSprzeciwWLexEna;
        private DateTime? _dataOd;
        private DateTime? _dataDo;
        //private int _IdDokumentWLexEna
        //private int _IdwnioskuEgzWLexEna
        private int? _KryteriumFiltrowania;
        private int? _KryteriumFiltrowaniaDaty;
        private string _FiltrSlowny; 
        private int _NumerOd;
        private int _NumerDo;
        private int _Rok;
        private int _KontoEpuId;
        private bool _IsChecked;
     

        /*
        public class EPUParamModel
        {
            public int IdPozwuWLexEna { get; set; }
            public int IdZazaleniaWLexEna { get; set; }
            public int IdSkargiWLexEna { get; set; }
            public int IdSprzeciwWLexEna { get; set; }
            public DateTime? DataOd { get; set; }
            public DateTime? DataDo { get; set; }
            public int? KryteriumFiltrowania { get; set; }
            public string FiltrSlowny { get; set; }
            public int NumerOd { get; set; }
            public int NumerDo { get; set; }
            public int Rok { get; set; }
            public int KontoEpuId { get; set; }
        }
         */
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


         [XmlIgnore]
         public bool IsChecked
         {
             get
             {
                 return this._IsChecked;
             }
             set
             {

                 if (value != this._IsChecked)
                 {
                     this._IsChecked = value;
                     this.OnPropertyChanged("IsChecked");
                 }
             }
         }


         [XmlIgnore]   
         public int Id
         {
             get
             {
                 return this._id;
             }
             set
             {

                 if (value != this._id)
                 {
                     this._id= value;
                     this.OnPropertyChanged("Id");
                 }
             }
         }


        [XmlIgnore]
         public int TypZadaniaId
         {
             get
             {
                 return this._typZadaniaId;
             }
             set
             {

                 if (value != this._typZadaniaId)
                 {
                     this._typZadaniaId = value;
                     this.OnPropertyChanged("TypZadaniaId");
                 }
             }
         }


        [XmlIgnore]
        public int Status
        {
            get
            {
                return this._status;
            }
            set
            {

                if (value != this._status)
                {
                    this._status = value;
                    this.OnPropertyChanged("Status");
                }
            }
        }


        [XmlIgnore]
         public string NazwaZadania
         {
             get
             {
                 return this._nazwaZadania;
             }
             set
             {

                 if (value != this._nazwaZadania)
                 {
                     this._nazwaZadania = value;
                     this.OnPropertyChanged("NazwaZadania");
                 }
             }
         }
        [XmlIgnore]
         public bool? Oczasie
         {
             get
             {
                 return this._oczasie;
             }
             set
             {

                 if (value != this._oczasie)
                 {
                     this._oczasie = (value == null) ? false: value;
                     this.OnPropertyChanged("Oczasie");
                 }
             }
         }

        [XmlIgnore]
        public DateTime? DataRozpoczęcia
        {
            get
            {
                return this._dRozpocz;
            }
            set
            {

                if (value != this._dRozpocz)
                {
                    this._dRozpocz = value;
                    this.OnPropertyChanged("DataRozpoczęcia");
                }
            }
        }
        [XmlIgnore]
        public DateTime? DataZakonczenia
        {
            get
            {
                return this._dZakoncz;
            }
            set
            {

                if (value != this._dZakoncz)
                {
                    this._dZakoncz = value;
                    this.OnPropertyChanged("DataZakonczenia");
                }
            }
        }


        [XmlIgnore]
        public string Opis
        {
            get
            {
                return this._opis;
            }
            set
            {

                if (value != this._opis)
                {
                    this._opis = value;
                    this.OnPropertyChanged("Opis");
                }
            }
        }

        [XmlIgnore]
        public string Parametry
        {
            get
            {
                return this._parametry;
            }
            set
            {

                if (value != this._parametry)
                {
                    this._parametry = value;
                    this.OnPropertyChanged("Parametry");
                }
            }
        }



        // Parametry zadania do XML'a
         public DateTime? DataOd
         {
             get
             {
                 return this._dataOd;
             }
             set
             {

                 if (value != this._dataOd)
                 {
                     this._dataOd = value;
                     this.OnPropertyChanged("DataOd");
                 }
             }
         }

         public DateTime? DataDo
         {
             get
             {
                 return this._dataDo;
             }
             set
             {

                 if (value != this._dataDo)
                 {
                     this._dataDo = value;
                     this.OnPropertyChanged("DataDo");
                 }
             }
         }

        

        public  int IdPozwuWLexEna
         {
             get
             {
                 return this._IdPozwuWLexEna;
             }
             set
             {

                 if (value != this._IdPozwuWLexEna)
                 {
                     this._IdPozwuWLexEna = value;
                     this.OnPropertyChanged("IdPozwuWLexEna");
                 }
             }
         }

        public int KontoEPUId
        {
            get
            {
                return this._KontoEpuId;
            }
            set
            {

                if (value != this._KontoEpuId)
                {
                    this._KontoEpuId = value;
                    this.OnPropertyChanged("KontoEpuId");
                }
            }
        } 
            


         public int? KryteriumFiltrowania
         {
             get
             {
                 return this._KryteriumFiltrowania;
             }
             set
             {

                 if (value != this._KryteriumFiltrowania)
                 {
                     this._KryteriumFiltrowania = value;
                     this.OnPropertyChanged("KryteriumFiltrowania");
                 }
             }
         }

         public int? KryteriumFiltrowaniaDaty
         {
             get
             {
                 return this._KryteriumFiltrowaniaDaty;
             }
             set
             {

                 if (value != this._KryteriumFiltrowaniaDaty)
                 {
                     this._KryteriumFiltrowaniaDaty = value;
                     this.OnPropertyChanged("KryteriumFiltrowaniaDaty");
                 }
             }
         }


         public string FiltrSlowny
         {
             get
             {
                 return this._FiltrSlowny;
             }
             set
             {

                 if (value != this._FiltrSlowny)
                 {
                     this._FiltrSlowny = value;
                     this.OnPropertyChanged("FiltrSlowny");
                 }
             }
         }

         public int NumerOd
         {
             get
             {
                 return this._NumerOd;
             }
             set
             {

                 if (value != this._NumerOd)
                 {
                     this._NumerOd = value;
                     this.OnPropertyChanged("NumerOd");
                 }
             }
         }
         public int NumerDo
         {
             get
             {
                 return this._NumerDo;
             }
             set
             {

                 if (value != this._NumerDo)
                 {
                     this._NumerDo = value;
                     this.OnPropertyChanged("NumerDo");
                 }
             }
         }


         public int Rok
         {
             get
             {
                 return this._Rok;
             }
             set
             {

                 if (value != this._Rok)
                 {
                     this._Rok = value;
                     this.OnPropertyChanged("Rok");
                 }
             }
         }

    }


  
        
      

         /// <remar
}
