using System;
using System.Net;


namespace LexEnaTrs
{
    public static class EPUCalc
    {

        public static decimal countProwizja(decimal kwota)
        {
            decimal retkwt = 0;
            /*
                        if (kwota <= 100 ) return (Decimal.Round( kwota * (decimal)0.026,2));
                        if (kwota <= 250) return (Decimal.Round(  kwota * (decimal)0.025,2));
                        if (kwota <= 500) return (Decimal.Round(  kwota * (decimal)0.0235,2));
                        if (kwota <= 750) return (Decimal.Round( kwota * (decimal)0.0220,2));
                        if (kwota <= 1000) return (Decimal.Round(  kwota * (decimal)0.0210,2));
                        if (kwota <= 1500) return (Decimal.Round(  kwota * (decimal)0.02,2));
                        if (kwota <= 3000) return (Decimal.Round(  kwota * (decimal)0.0195,2));
                        if (kwota <= 5000) return (Decimal.Round(kwota * (decimal)0.019, 2));
                        retkwt = kwota * (decimal)0.018;
                        return Decimal.Round(retkwt, 2);

                        */
            return 0;
            return Decimal.Round(kwota * (decimal)0.01, 2);

        }

        public static decimal countKZP(decimal WPS)
        {
            decimal ld_wynik;

            ld_wynik = 0;

            if (WPS <= (decimal)500.00)

                ld_wynik = (decimal)60.00;
            else if (WPS <= (decimal)1500.00)

                ld_wynik = (decimal)180.00;
            else if (WPS <= (decimal)5000.00)

                ld_wynik = (decimal)600.00;
            else if (WPS <= (decimal)10000.00)

                ld_wynik = (decimal)1200.00;
            else if (WPS <= (decimal)50000.00)
                ld_wynik = (decimal)2400.00;
            else if (WPS <= (decimal)200000.00)
                ld_wynik = (decimal)3600.00;
            else
                ld_wynik = (decimal)7200.00;
            return ld_wynik;

       }

        public static decimal countKZA(decimal WPS)
        {
            if (WPS <= 500) return 30;
            if (WPS > 500 && WPS <= 1500) return 90;
            if (WPS > 1500 && WPS <= 5000) return 300;
            if (WPS > 5000 && WPS <= 10000) return 600;
            if (WPS > 10000 && WPS <= 50000) return 1200;
            if (WPS > 50000 && WPS <= 200000) return 1800;
            return 1800;

        }

        public static decimal countOplata(decimal wartosc)
        {
            decimal wartosc1 = decimal.Ceiling(wartosc);
            decimal procent = 0;
            decimal proc5 = 0;



            if (wartosc > 20000)
            {
                proc5 = (wartosc * (decimal)0.05);
                if (proc5 > (decimal)200000)
                    proc5 = 200000;
            }
            else if (wartosc > (decimal)15000)
                proc5 = 1000;
            else if (wartosc > (decimal)10000)
                proc5 = 750;
            else if (wartosc > (decimal)7500)
                proc5 = 500;
            else if (wartosc > (decimal)4000)
                proc5 = 400;
            else if (wartosc > (decimal)1500)
                proc5 = 200;
            else if (wartosc > (decimal)500)
                proc5 = 100;
            else
                proc5 = 30;


            procent = proc5 / (decimal)4;


            decimal rr = decimal.Ceiling(procent);

            if (rr < (decimal)30)
                return (decimal)30;




            return rr;

        }
    }
}
