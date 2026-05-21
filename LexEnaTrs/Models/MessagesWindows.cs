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
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using LexEnaTrs;

namespace LexEnaTrs
{
    public static class AlertMsg
    {
        public static void Show (int? errorcode, string errormsg)
            {   
            string errmsg;
            DialogParameters  pars= new DialogParameters();
            errmsg = "";
            if (errorcode != null)
             {
                if (errorcode > 0) errmsg = "Kod " + errorcode.ToString();
                errmsg += " " + errorcode;
            }
            pars.Content = errmsg;
            RadWindow.Alert(pars.Content);
               
                    
            }
        public static void Show(string errormsg)
        {
            DialogParameters pars = new DialogParameters();
            pars.Content = errormsg;


            RadWindow.Alert(pars);


        }

    }

    
}
