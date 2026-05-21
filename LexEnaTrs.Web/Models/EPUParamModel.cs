using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;


namespace LexEnaTrs.Web
{



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
        public int? StatusDoreczenia { get; set; }
        public int? RodzajDaty { get; set; }
    }

    public class TypSlownikFiltered
    {
        public string Nazwa { get; set; }
        public int Numer { get; set; }
        public int Filter1 { get; set; }
        public int Filter2 { get; set; }
        public int Filter3 { get; set; }

        public TypSlownikFiltered()
        {

        }

        public TypSlownikFiltered(string s, int n, int f1, int f2, int f3)
        {
            Nazwa = s;
            Numer = n;
            Filter1 = f1;
            Filter2 = f2;
            Filter3 = f3;
        }

    }

    public class WienaConfig
    {
        public List<TypSlownikFiltered> Oddzialy { get; set; }
        public List<TypSlownikFiltered> Statusy { get; set; }
        public List<TypSlownikFiltered> Konta { get; set; }
        public List<TypSlownikFiltered> Kancelarie { get; set; }
        public List<TypSlownikFiltered> Radcowie { get; set; }

        public WienaConfig()
        {

        }

    }
    public class ItemsForLawsuit
    {
        public int PozewId { get; set; }
        public int SprawaId { get; set; }
        public int TypOdsetek { get; set; }
        public List<NalToCorrect> NaleznosciLst { get; set; }
        public List<TypDowod> DowodyLst { get; set; }
        public List<TypDowod> UzasadnieniaLst { get; set; }
        public string DataNBP { get; set; }
        public bool Czy40EURO { get; set; }

    }  

    [Serializable]
    public class NalToCorrect
    {

        public int id { get; set; }
        public int IdWiena { get; set; }
        public DateTime? data_dok { get; set; }
        public DateTime? data_n { get; set; }
        public decimal? kwota { get; set; }
        public decimal? zaleglosc { get; set; }
        public string opis { get; set; }

    }


    public class TypDowod
    {
        public string Nazwa { get; set; }
        public DateTime? DataDowodu { get; set; }
        public string Tekst { get; set; }
        public string Opis { get; set; }
        public string Grupa { get; set;}
        public int Rodzaj { get; set; }
        public int Id { get; set; }
        public bool Choosen { get; set; }

    }
    public class ZaliczkiImportData
    {
        public int Lp { get; set; }
        public bool Zapis { get; set; }
        public string Oddzial { get; set; }
        public string Sygnatura { get; set; }
        public string Dluznik { get; set; }
        public DateTime DataZaliczki { get; set; }
        public decimal Kwota { get; set; }
        public string Uwagi { get; set; }
        public string Error { get; set; }
        public int id_sprawy { get; set; }
    }
}
  
