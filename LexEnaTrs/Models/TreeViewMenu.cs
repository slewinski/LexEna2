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
    public class TreeViewMenu : INotifyPropertyChanged
    {

            private string _nakazyZaplaty;
            private string _klauzule;
            private string _inne;

            private string _nakazyBasicText = "Nakazy zapłaty";
            private  int  _nakazyAll; 
            private  int  _nakazyNew; 
            private  int  _nakazyUnReadOnlyMe;
            private  FontWeight _nakazyFontWeight;
    

            private string _klauzuleBasicText = "Klauzule";
            private int _klauzuleAll;
            private int _klauzuleNew;
            private int _klauzuleUnReadOnlyMe;
            private FontWeight _klauzuleFontWeight;
    
            private string _inneBasicText = "Inne decyzje";
            private int _inneAll;
            private int _inneNew;
            private int _inneUnReadOnlyMe;
            private FontWeight _inneFontWeight;
            private bool eopVisible = true;
            private bool eobVisible = true;


        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler PropertyFrmChanged;

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

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyFrmChanged != null)
            {
                PropertyFrmChanged(this, new PropertyChangedEventArgs(info));
            }
        }

      



        public TreeViewMenu()
            {
                _nakazyZaplaty = _nakazyBasicText;
                _klauzule = _klauzuleBasicText;
                _inne = _inneBasicText;
                _nakazyFontWeight = FontWeights.Normal;
                _klauzuleFontWeight = FontWeights.Normal;
                _inneFontWeight =  FontWeights.Normal ;
    
            }


        public bool EOPVisible
        {
            get
            {
                if (UserProfile.Firma != 1)
                    eopVisible = true;
                else
                    eopVisible = false;

                return eopVisible;
            }

            set
            {
                eopVisible = value;
                if (UserProfile.Firma != 1)
                    eopVisible = true;
                else
                    eopVisible = false;
                NotifyPropertyChanged("EOPVisible");
            }
        }

        public bool EOBVisible
        {
            get
            {

                if (UserProfile.Firma != -1)
                    eobVisible = true;
                else
                    eobVisible = false;

                return eobVisible;
            }

            set
            {
                eobVisible = value;
                if (UserProfile.Firma != -1)
                    eobVisible = true;
                else
                    eobVisible = false;
                NotifyPropertyChanged("EOBVisible");
            }
        }

        public int NakazyAll
            {
                get
                {
                    
                    return this._nakazyAll;
                }
                set
                {

                    if (value != this._nakazyAll)
                    {
                        this._nakazyAll = value;
                        this.NakazyZaplatyItem = this._nakazyBasicText + " ("  + _nakazyNew.ToString() + "/" + _nakazyUnReadOnlyMe.ToString() +  "/" + _nakazyAll.ToString() + ")";
                        //this.OnPropertyChanged("NakazyZaplatyItem");
                        this.OnPropertyChanged("NakazyAll");
                    }
                }
            }

            public int NakazyNew
            {
                get
                {

                    return this._nakazyNew;
                }
                set
                {

                    if (value != this._nakazyNew)
                    {
                        this._nakazyNew = value;
                        this.NakazyZaplatyItem = this._nakazyBasicText + " (" + _nakazyNew.ToString() + "/" + _nakazyUnReadOnlyMe.ToString() + "/" + _nakazyAll.ToString() + ")";
                        //this.OnPropertyChanged("NakazyZaplatyItem");
                        if (this._nakazyNew > 0)
                            this.NakazyFontWeight = FontWeights.Bold;
                        else
                            this.NakazyFontWeight = FontWeights.Normal;
                        this.OnPropertyChanged("NakazyNew");
                    }
                }
            }


            public string NakazyZaplatyItem
            {
                get
                {
                   // this._nakazyZaplaty = this._nakazyBasicText + " (" + _nakazyAll.ToString() + "/" + _nakazyNew.ToString() + "/" + _nakazyUnReadOnlyMe.ToString() + ")";
                    return this._nakazyZaplaty;
                }
                set
                {

                    if (value != this._nakazyZaplaty)
                    {
                        this._nakazyZaplaty = value;
                        this.OnPropertyChanged("NakazyZaplatyItem");
                    }
                }
            }

            public int NakazyUnReadOnlyMe
            {
               
                
                 get
                {

                    return this._nakazyUnReadOnlyMe;
                }
                set
                {

                    if (value != this._nakazyUnReadOnlyMe)
                    {
                        this._nakazyUnReadOnlyMe = value;
                        this.NakazyZaplatyItem = this._nakazyBasicText + " (" + _nakazyNew.ToString() + "/" + _nakazyUnReadOnlyMe.ToString() + "/" + _nakazyAll.ToString() + ")";
                        //this.OnPropertyChanged("NakazyZaplatyItem");
                        if (this._nakazyUnReadOnlyMe > 0)
                            this.NakazyFontWeight = FontWeights.Bold;
                        else
                            this.NakazyFontWeight = FontWeights.Normal;
                        this.OnPropertyChanged("NakazyUnReadOnlyMe");
                    }
                }


            }


            public FontWeight NakazyFontWeight
            {
                get
                {

                    return this._nakazyFontWeight;
                }
                set
                {

                    if (value != this._nakazyFontWeight)
                    {
                        this._nakazyFontWeight = value;
                        this.OnPropertyChanged("NakazyFontWeight");
                    }
                }
            }


            public int KlauzuleAll
            {
                get
                {

                    return this._klauzuleAll;
                }
                set
                {

                    if (value != this._klauzuleAll)
                    {
                        this._klauzuleAll = value;
                        this.KlauzuleItem = this._klauzuleBasicText + " (" + _klauzuleNew.ToString() + "/" + _klauzuleUnReadOnlyMe.ToString() + "/" + _klauzuleAll.ToString() + ")";
                        //this.OnPropertyChanged("KlauzuleItem");
                        this.OnPropertyChanged("KlauzuleAll");
                    }
                }
            }

            public int KlauzuleNew
            {
                get
                {

                    return this._klauzuleNew;
                }
                set
                {

                    if (value != this._klauzuleNew)
                    {
                        this._klauzuleNew = value;
                        this.KlauzuleItem = this._klauzuleBasicText + " (" + _klauzuleNew.ToString() + "/" + _klauzuleUnReadOnlyMe.ToString() + "/" + _klauzuleAll.ToString() + ")";
                        //this.OnPropertyChanged("KlauzuleItem");
                        if (this._klauzuleNew > 0)
                            this.KlauzuleFontWeight = FontWeights.Bold;
                        else
                            this.KlauzuleFontWeight = FontWeights.Normal;
                        this.OnPropertyChanged("KlauzuleNew");
                    }
                }
            }


            public string KlauzuleItem
            {
                get
                {
                    // this._klauzule = this._klauzuleBasicText + " (" + _klauzuleAll.ToString() + "/" + _klauzuleNew.ToString() + "/" + _klauzuleUnReadOnlyMe.ToString() + ")";
                    return this._klauzule;
                }
                set
                {

                    if (value != this._klauzule)
                    {
                        this._klauzule = value;
                        this.OnPropertyChanged("KlauzuleItem");
                    }
                }
            }

            public int KlauzuleUnReadOnlyMe
            {


                get
                {

                    return this._klauzuleUnReadOnlyMe;
                }
                set
                {

                    if (value != this._klauzuleUnReadOnlyMe)
                    {
                        this._klauzuleUnReadOnlyMe = value;
                        this.KlauzuleItem = this._klauzuleBasicText + " (" + _klauzuleNew.ToString() + "/" + _klauzuleUnReadOnlyMe.ToString() + "/" + _klauzuleAll.ToString() + ")";
                        //this.OnPropertyChanged("KlauzuleItem");
                        if (this._klauzuleUnReadOnlyMe > 0)
                            this.KlauzuleFontWeight = FontWeights.Bold;
                        else
                            this.KlauzuleFontWeight = FontWeights.Normal;
                        this.OnPropertyChanged("KlauzuleUnReadOnlyMe");
                    }
                }


            }

            public FontWeight KlauzuleFontWeight
            {
                get
                {

                    return this._klauzuleFontWeight;
                }
                set
                {

                    if (value != this._klauzuleFontWeight)
                    {
                        this._klauzuleFontWeight = value;
                        this.OnPropertyChanged("KlauzuleFontWeight");
                    }
                }
            }


            public int InneAll
            {
                get
                {

                    return this._inneAll;
                }
                set
                {

                    if (value != this._inneAll)
                    {
                        this._inneAll = value;
                        this.InneItem = this._inneBasicText + " (" + _inneNew.ToString() + "/" + _inneUnReadOnlyMe.ToString() + "/" + _inneAll.ToString() + ")";
                        //this.OnPropertyChanged("InneItem");
                        this.OnPropertyChanged("InneAll");
                    }
                }
            }

            public int InneNew
            {
                get
                {

                    return this._inneNew;
                }
                set
                {

                    if (value != this._inneNew)
                    {
                        this._inneNew = value;
                        this.InneItem = this._inneBasicText + " (" + _inneNew.ToString() + "/" + _inneUnReadOnlyMe.ToString() + "/" + _inneAll.ToString() + ")";
                        //this.OnPropertyChanged("InneItem"); 
                        if (this._inneNew > 0)
                            this.InneFontWeight = FontWeights.Bold;
                        else
                            this.InneFontWeight = FontWeights.Normal;
                        this.OnPropertyChanged("InneNew");
                    }
                }
            }


            public string InneItem
            {
                get
                {
                    // this._inne = this._inneBasicText + " (" + _inneAll.ToString() + "/" + _inneNew.ToString() + "/" + _inneUnReadOnlyMe.ToString() + ")";
                    return this._inne;
                }
                set
                {

                    if (value != this._inne)
                    {
                        this._inne = value;

                        this.OnPropertyChanged("InneItem");
                    }
                }
            }

            public int InneUnReadOnlyMe
            {


                get
                {

                    return this._inneUnReadOnlyMe;
                }
                set
                {

                    if (value != this._inneUnReadOnlyMe)
                    {
                        this._inneUnReadOnlyMe = value;
                        this.InneItem = this._inneBasicText + " (" + _inneNew.ToString() + "/" + _inneUnReadOnlyMe.ToString() + "/" + _inneAll.ToString() + ")";
                        //this.OnPropertyChanged("InneItem");
                        if (this._inneUnReadOnlyMe > 0)
                            this.InneFontWeight = FontWeights.Bold;
                        else
                            this.InneFontWeight = FontWeights.Normal;
                        this.OnPropertyChanged("InneUnReadOnlyMe");
                    }
                }


            }

            public FontWeight InneFontWeight
            {
                get
                {

                    return this._inneFontWeight;
                }
                set
                {

                    if (value != this._inneFontWeight)
                    {
                        this._inneFontWeight = value;
                        this.OnPropertyChanged("InneFontWeight");
                    }
                }
            }

    }
}
