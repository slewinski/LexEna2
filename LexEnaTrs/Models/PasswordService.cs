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
    public class PasswordService:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string oldpwd;
        private string newpwd1;
        private string newpwd2;
        private string secQuestion;
        private string answer;
        private string usrName;

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

        public PasswordService()
        { }

        public string UsrName
        {
            get
            {
                return this.usrName;
            }
            set
            {
                if (this.usrName != value)
                {
                    this.usrName = value;
                    this.OnPropertyChanged("UsrName");
                }
            }
        }

        public string Oldpwd
        {
            get
            {
                return this.oldpwd;
            }
            set
            {
                if (this.oldpwd != value)
                {
                    this.oldpwd = value;
                    this.OnPropertyChanged("Oldpwd");
                }
            }
        }


        public string Newpwd1
        {
            get
            {
                return this.newpwd1;
            }
            set
            {
                if (this.newpwd1 != value)
                {
                    this.newpwd1 = value;
                    this.OnPropertyChanged("Newpwd1");
                }
            }
        }



        public string Newpwd2
        {
            get
            {
                return this.newpwd2;
            }
            set
            {
                if (this.newpwd2 != value)
                {
                    this.newpwd2 = value;
                    this.OnPropertyChanged("Newpwd2");
                }
            }
        }

        public string SecQuestion
        {
            get
            {
                return this.secQuestion;
            }
            set
            {
                if (this.secQuestion != value)
                {
                    this.secQuestion = value;
                    this.OnPropertyChanged("SecQuestion");
                }
            }
        }


        public string Answer
        {
            get
            {
                return this.answer;
            }
            set
            {
                if (this.answer != value)
                {
                    this.answer = value;
                    this.OnPropertyChanged("Answer");
                }
            }
        }
    }



}
