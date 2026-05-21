using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Configuration;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace LexEnaTrs
{
    public class CustomLocalizationManager : LocalizationManager
    {
        public override string GetStringOverride(string key)
        {
            switch (key)
            {
                case "GridViewGroupPanelText":
                    return "Przeciągnij nagłówek kolumny i upuść w tym miejscu";
                //---------------------- RadGridView Filter Dropdown items texts:
                case "GridViewClearFilter":
                    return "Wyczyść filtr";
                case "GridViewFilterShowRowsWithValueThat":
                    return "Pokaż wiersze z zawartością:";
                case "GridViewFilterSelectAll":
                    return "Zaznacz wszytko";
                case "GridViewFilterContains":
                    return "Zawierającą";
                case "GridViewFilterDoesNotContain":
                    return "Nie zawierającą";
                case "GridViewFilterEndsWith":
                    return "Kończącą się na";
                case "GridViewFilterIsNotContainedIn":
                    return "Nie zawartą w";
                case "GridViewFilterIsContainedIn":
                    return "Zawartą w";
                case "GridViewFilterIsEqualTo":
                    return "Równą";
                case "GridViewFilterIsGreaterThan":
                    return "Większą niż";
                case "GridViewFilterIsGreaterThanOrEqualTo":
                    return "Większa lub równą niż";
                case "GridViewFilterIsLessThan":
                    return "Mniejszą niż";
                case "GridViewFilterIsLessThanOrEqualTo":
                    return "Mniejszą lub równą niż";
                case "GridViewFilterIsNotEqualTo":
                    return "Różną niż";
                case "GridViewFilterStartsWith":
                    return "Rozpoczynającą się od";
                case "GridViewFilterAnd":
                    return " I ";
                case "GridViewFilterOr":
                    return " Lub ";
                case "GridViewFilter":
                    return "Filtruj";
                // RadDataPager
                case "CurrentPageText":
                    return "Strona";
                case "LabelText":
                    return "Rozmiar";

                case "FirstButtonText":
                    return "Pierwsza";
                case "LastButtonText":
                    return "Ostatnia";

                case "NextButtonText":
                    return "Następna";

                case "PageSizeSubmitButtonText":
                    return "Zmień";

                case "PageSizeText":
                    return "Rozmiar strony";

                case "PrevButtonText":
                    return "Poprzednia";

                case "ReservedResource":
                    return " Please do not remove this key.";

                case "SliderDecreaseText":
                    return "Zmniejsz";

                case "SliderDragText":
                    return "Przeciągnij";

                case "SliderIncreaseText":
                    return "Powiększ";

                case "SubmitButtonText":
                    return "Idź do";

                case "TotalPageText":
                    return "z";

                case "RadDataPagerEllipsisString":
                    return "...";
                case "RadDataPagerPage":
                    return "Strona";
                case "RadDataPagerOf":
                    return "z";
                case "EnterDate":
                    return "Wprowadź Datę";

                case "GridViewFilterIsNull":
                    return "Pusta zawartość (null)";
                case "GridViewFilterIsNotNull":
                    return "Zawartość nie pusta (null)";
                case "GridViewFilterIsEmpty":
                    return "Pusta zawartość";
                case "GridViewFilterIsNotEmpty":
                    return "Nie pusta zawartość";

                case "DataForm_MoveCurrentToFirst":
                    return "Przejdź do początku";
case "DataForm_MoveCurrentToPrevious":
                    return "Przejdź do poprzedniego";
case "DataForm_MoveCurrentToNext":
                    return "Przejdź do następnego";
case "DataForm_MoveCurrentToLast":
                    return "Przejdź na koniec";
case "DataForm_AddNew":
                    return "Dodaj";
case "DataForm_BeginEdit":
                    return "Edytuj";
case "DataForm_Delete":
                    return "Usuń";

            }
            return base.GetStringOverride(key);
        }
    }
}
