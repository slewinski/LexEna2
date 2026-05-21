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
using System.Windows.Data;
using System.Globalization;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using LexEnaTrs.Web;
using System.Collections.Generic;




namespace LexEnaTrs
{

    /*
     * 
     Statusy dokumentów:
     
     * wplyw 0
zadekretowano 1
przyjeto 2
odrzucono 3
odrzucono_prawomocnie 4
przekazano_do_2_instnacji 5
zadekretowano_w_2_instancji 6
oddalono_w_2_instancji 7
uwzgledniono_w_2_instancji 8
zalatwiono 9
umorzono 10
anulowano_blad 20
     * 
     */
    public class Oplata2ProwizjaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value== null) return 0;
            return 0;// EPUCalc.countProwizja((decimal)value);
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UserIdToApprovedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value == null) return false;
            foreach (var r in UserList.UsersAspNetList)
            {
                if (r.Id == (int)value) return (r.IsApproved);

            }
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DokOdebrRowStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {

            vw_ListaDoOdebr row;
             
            if (value == null) return 2;  // normalny wiersz  

            row = value as vw_ListaDoOdebr;
            if ( row.IsChecked == null || row.IsChecked == 0 ) return 0;   // nie przeczytanny przez nikogo
            if ( row.IsChecked > 0 && row.IsChecked <1000 ) return 1;    // przeczytany  tylko przez usera
            if ( row.IsChecked >= 1000 && row.IsChecked <1000000  &&  (row.IsChecked % 1000) > 0) return 2; // przeczytany przez admina i referenta
            if ( row.IsChecked >= 1000 && row.IsChecked <1000000  &&  (row.IsChecked % 1000) == 0) return 3; // przeczytany tylko przez admina

            return 2;       
           
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ImportBIGRowStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {

            BIG_Import row;

            if (value == null) return 0;  // normalny wiersz  

            row = value as BIG_Import;
            if (row.Status == 0 ) return 0;   // nowy anulowany błąd w krd itp
            if (row.Status == 3) return 3;   //  w kolejce
            if (row.Status == 4) return 4;    // w przetwarzaniu
            if (row.Status == 200 && row.lBlad == 0) return 200;
            if (row.Status == 200 && row.lBlad > 0 && (row.lPoz - row.lBlad) > 0 ) return 199; // częściowo OK
            if (row.Status == 200 && row.lBlad > 0 && row.lPoz == row.lBlad) return  -1; // częściowo OK
            if (row.Status == null) return 0;
            return row.Status;
            

        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class UZDPackageRowStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {

            UZDvw_Pakiet row;

            if (value == null) return 0;  // normalny wiersz  

            row = value as UZDvw_Pakiet;
            if (row.Status == 0) return 0;   // nowy anulowany błąd w krd itp
            if (row.Status == 3) return 3;   //  w kolejce
            if (row.Status == 4) return 4;    // w przetwarzaniu
            
            return row.Status;


        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ImportBIGDetailRowStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {

            BIGvw_ImportyDetail row;
            int? statusOper;
            if (value == null) return 0;  // normalny wiersz  
            if (value.GetType() == typeof(BIGvw_ImportyDetail))
                statusOper = (value as BIGvw_ImportyDetail).StatusOperacji;
            else
                statusOper = (value as BIGvw_ObligationLastStatus).StatusOperacji;
            return statusOper;


        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UZD_FakturaDetailRowStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {

            UZD_Faktura row;
            int? statusOper;
            if (value == null) return 0;  // normalny wiersz  
            if (value.GetType() == typeof(UZD_Faktura))
                statusOper = (value as UZD_Faktura).Status;
            else
                statusOper = (value as UZD_Faktura).Status;
            return statusOper;


        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PaczkiRowStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {




            vw_ListaPaczek row;

            if (value == null) return 1;  // normalny wiersz  

            row = value as vw_ListaPaczek;
            if (row.CzyZestaw == null || row.CzyZestaw == 0) return 0;   // nie zrobiono zestawienia
            return 1;

        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class KomornikRowStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {




            KancelariaKomornicza row;

            if (value == null) return 0;  // normalny wiersz  

            row = value as KancelariaKomornicza;
            if (row.czyus == null || row.czyus == 0) return 0;   // nie zrobiono zestawienia
            return 1;

        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /*
    public class IsVisibleForSuperAdmin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (UserProfile.Rola == 2)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsVisibleForAnyAdmin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;

            if ((int)value > 0)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    */

    public class CzyusToBackgroundColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {

            return (value as int?) == 1 ? 0xFF70706C : 0xFFFFFFFF;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




    public class BoolToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
       
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TypDok2VisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value == null) return true;
            return ((int)value == 13) ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class TypRejestruToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            switch ((string)parameter)
            {
                case "krs":
                    {
                        if (value == null) return Visibility.Collapsed;
                        if ((int)value == 1)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    }
                case "inny":
                    {
                        if (value == null) return Visibility.Collapsed;
                        if ((int)value == 2)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    }

                default: return Visibility.Collapsed;

            }

        }
        public object ConvertBack(object value, Type targetType, object parameter,
       System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    
    public class TypOsobyToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            switch ((string)parameter)
            { 
                case "fizyczna":
                        {
                        if (value== null) return Visibility.Visible;
                        if ((int)value <= 1)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                        }
                case "prawna":
                        {
                            if (value == null) return Visibility.Collapsed;
                            if ((int)value <= 1)
                                return Visibility.Collapsed;
                            else
                                return Visibility.Visible;
                        }
                case "dzialalnosc":
                        {
                            if (value == null) return Visibility.Collapsed;
                            if ((int)value < 1)
                                return Visibility.Collapsed;
                            else
                                return Visibility.Visible;
                        }
                default: return Visibility.Collapsed;
                    
            }
            
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ColumnSumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            decimal ln, lo;
            if (value == null && parameter == null) return "0,00";
            if (value == null) 
                    ln = 0;
            else
                    ln = (decimal) value;
            if (parameter == null)
                lo = 0;
            else
                lo = (decimal)parameter;

            return (lo + ln).ToString("{0:C}");

        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class IntToDokStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null) return "???";

                switch ((int)value)
                {
                    case 1:
                        return "projekt";
                    case 2:
                        return "zatwierdzony";
                    case 3:
                        return "złożony";
                    case 4:
                        return "zwrócony";
                    case 5:
                        return "odrzucony";
                    case 6:
                        return "usunięty";
                    default:
                        return "????";
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.CreateNew(ex, value.ToString());
                return "????";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
       System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

        public class IntToPaczkaStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null) return "???";
        
            switch ((int)value)
            {
                case 1:
                    return "w przygotowaniu";
                case 2:
                    return "do wysyłki";
                case 3:
                    return "złożona";
                case 4:
                    return "odrzucona";
                case 5:
                    return "rozpisana";
                case 6:
                    return "usunięta";
                default:
                    return "????";
                               }
        }
        catch (Exception ex)
        {
            ErrorWindow.CreateNew(ex, value.ToString());
            return "????";
        }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

   
public class IntToStatImpBIG: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value == null) return "???";
            switch ((int)value)
            {
                case 0:
                    return "";
                case 200:
                    return "Zażalenie";
                case 2:
                    return "Skarga";
                case 13:
                case 3:
                    return "Pismo";
                case 4:
                    return "Wniosek";
                case 5:
                    return "Uzupelnienie adresu";
                case 6:
                    return "Inne";
                case 101:
                    return "Nakaz zpałaty";
                case 102:
                    return "Postanowienie";
                case 103:
                    return "Zarządzenie";

                case 10:
                    return "Pozew";

                case 30:
                    return "WniosekEgzekucyjny";
                case 100010:
                    return "Pozew \"zwykły\"";

                case 100030:
                    return "Wniosek egz. \"zwykły\"";
                case 100003:
                    return "Inny Dokument \"zwykły\"";
                default:
                    return "";
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class IntToTypDok : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value == null) return "???";
            switch ((int)value)
            {
                case 0:
                    return "?????";
                 case 1:
                    return "Zażalenie";
                case 2:
                    return "Skarga";
                case 13:
                case 3:
                    return "Pismo";
                case 4:
                    return "Wniosek";
                case 5:
                    return "Uzupelnienie adresu";
                case 6:
                    return "Inne";
                case 101:
                    return "Nakaz zpałaty";
                case 102:
                    return "Postanowienie";
                case 103:
                    return "Zarządzenie";
                
                case 10:
                    return "Pozew";
                
                case 30:
                    return "WniosekEgzekucyjny";
                case 100010:
                    return "Pozew \"zwykły\"";
                    
                case 100030:
                    return "Wniosek egz. \"zwykły\"";
                case 100003:
                    return "Inny Dokument \"zwykły\"";
                case 100016:
                    return "Dok. dot zgonów";
                case 100014:
                    return "Post o umorzeniu";
                case 100019:
                    return "Postanowienie o kosztach doręczenia komorniczego";
                case 100020:
                    return "Postanowienie o nadaniu klauzuli na post. o kosztach doręczenia kom.";
                case 999:
                    return "Dokument inicjujący postępowanie";
                default:
                    return "????";
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToTakNIE : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value == null) return "???";
            switch ((int)value)
            {
                case 0:
                    return "nie";
                case 1:
                    return "TAK !!!";
                             default:
                    return "????";
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class IntToTypWpl : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value == null) return "???";
            switch ((int)value)
            {
                case 0:
                    return "?????";
                case 1:
                    return "Wpłata dłużnika";
                case 2:
                    return "Wpłata komornika";
                case 3:
                    return "Umorzenie";
                 default:
                    return "????";
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class IntToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            return (int)value==1 ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsEOB2VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
     
            return UserProfile.Firma == 1 ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsEOP2VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            return UserProfile.Firma == -1 ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class IntToVisibilityNegValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            return (int)value == 1 ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class BoolToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value == null) return false;   
            return  (int)value > 0 ? true : false;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value == null) return 0;
            return (bool)value ? 1 : 0;
        }
    }

    public class IntToTypJednostkiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (value == null) return "???";
            switch ((int)value)
            {
                case 1:
                    return "Zewnętrzna kancelaria prawna";
                case 2:
                    return "Własna komórka windykacji";
                case 3:
                    return "Własna komórka windykacji-operacyjna";
                default:
                    return "????";
            }
           
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }


    public class Date2StringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            DateTime  result;
 
            if (value == null) return null;

            if ( !DateTime.TryParse((string)value, out result) ) return null;

            return result; 

        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            DateTime mydate;
            if (value == null) return "";
            mydate = (DateTime)value;
            return  mydate.ToString("yyyy-MM-dd");
  

        }
    }
    
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
         DateTime dt;
         if (DateTime.TryParseExact((string)value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
             return dt;
         else
             return null;

        }
        
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            DateTime dt;
            dt = (DateTime)value;
            String dtStr;
            try
            {
                dtStr = String.Format("{0:yyyy-MM-dd}", dt);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
                    }
            return dtStr;

        }
    }



    public static class GridViewExtensions
    {
        public static GridViewCell GetCellByContent(this RadGridView gridView, object cellValue)
        {
            return
                (from cell in gridView.ChildrenOfType<GridViewCell>()
                 where cell.Value.ToString() == cellValue.ToString()
                 select cell).FirstOrDefault();
        }

        public static GridViewCell GetCellByIndexes(this RadGridView gridView, int rowIndex, int columnIndex)
        {
            return
                (from cell in gridView.ChildrenOfType<GridViewCell>()
                 where gridView.Columns.IndexOf(cell.Column) == columnIndex
                 select cell).Skip(rowIndex).FirstOrDefault();

        }
    }
    public class ConditionalStyleSelector : StyleSelector
    {
        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            object conditionValue = this.ConditionConverter.Convert(item, null, null, null);
            foreach (ConditionalStyleRule rule in this.Rules)
            {
                if (Equals(rule.Value, conditionValue))
                {
                    return rule.Style;
                }
            }


            return base.SelectStyle(item, container);
        }

        List<ConditionalStyleRule> _Rules;
        public List<ConditionalStyleRule> Rules
        {
            get
            {
                if (this._Rules == null)
                {
                    this._Rules = new List<ConditionalStyleRule>();
                }

                return this._Rules;
            }
        }

        IValueConverter _ConditionConverter;
        public IValueConverter ConditionConverter
        {
            get
            {
                return this._ConditionConverter;
            }
            set
            {
                this._ConditionConverter = value;
            }
        }
    }

    public class ConditionalStyleRule
    {
        object _Value;
        public object Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
            }
        }

        Style _Style;
        public Style Style
        {
            get
            {
                return this._Style;
            }
            set
            {
                this._Style = value;
            }
        }
    }
}
