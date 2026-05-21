using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace LexEnaTrs
{
    public class SearchFilter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _nazwa;
        private string _opis;
        private string _sygnatura;
        private string _sygnNCe;
        private string _ulica;
        private string _miejscowosc;
        private string _poczta;
        private string _nrewid;
        private string _pesel;


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

        public SearchFilter()
        { 
        
        }

        public string Sygnatura
        {
            get
            {
                return this._sygnatura;
            }
            set
            {
                if (this._sygnatura != value)
                {
                    this._sygnatura = value;
                    this.OnPropertyChanged("Sygnatura");
                }
            }
        }
        public string Nazwa
        {
            get
            {
                return this._nazwa;
            }
            set
            {
                if (this._nazwa != value)
                {
                    this._nazwa = value;
                    this.OnPropertyChanged("Nazwa");
                }
            }
        }

        public string SygnNCe
        {
            get
            {
                return this._sygnNCe;
            }
            set
            {
                if (this._sygnNCe != value)
                {
                    this._sygnNCe = value;
                    this.OnPropertyChanged("SygnNCe");
                }
            }
        }

        public string NrEwid
        {
            get
            {
                return this._nrewid;
            }
            set
            {
                if (this._nrewid != value)
                {
                    this._nrewid = value;
                    this.OnPropertyChanged("NrEwid");
                }
            }
        }
        // n umer faktury
        public string Opis
        {
            get
            {
                return this._opis;
            }
            set
            {
                if (this._opis != value)
                {
                    this._opis = value;
                    this.OnPropertyChanged("Opis");
                }
            }
        }

        public string Ulica
        {
            get
            {
                return this._ulica;
            }
            set
            {
                if (this._ulica != value)
                {
                    this._ulica = value;
                    this.OnPropertyChanged("Ulica");
                }
            }
        }

        public string Miejscowosc
        {
            get
            {
                return this._miejscowosc;
            }
            set
            {
                if (this._miejscowosc != value)
                {
                    this._miejscowosc = value;
                    this.OnPropertyChanged("Miejscowosc");
                }
            }
        }

        public string Poczta
        {
            get
            {
                return this._poczta;
            }
            set
            {
                if (this._poczta != value)
                {
                    this._poczta = value;
                    this.OnPropertyChanged("Poczta");
                }
            }
        }

        public string Pesel
        {
            get
            {
                return this._pesel;
            }
            set
            {
                if (this._pesel != value)
                {
                    this._pesel = value;
                    this.OnPropertyChanged("Pesel");
                }
            }
        }
    }
}
