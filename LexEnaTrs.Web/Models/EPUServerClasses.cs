using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Text;
using LexEnaTrs;
using LexEnaTrs.Web;
using System.Globalization;
using System.Security;
using System.Xml.Linq;

namespace LexEnaTrs.Web
{
    /*
    static class ConstantsWeb
    {
        public const string newnamespace = "http://www.e-sad.gov.pl/epu";
        public const string oldnamespace = "http://www.e-sad.gov.pl/epu";
        public const string currnamespace = "http://www.e-sad.gov.pl/epu";
    }
    */


    public class DokEPU
    {
        // dokument epu
        private DokumentEPU _dokEPU; // = new DokumentEPU();
        private KontoEPU _kepu;
        private int idKontaEPU;
        private LexEnaMeritumEntities lexena;
        private string dokSerialized;
        private WniosekEgzekucyjny wniosek;
        private PozewEPU _pozew;
        private int Sprawa_id;
        private Sprawa _sprawa;
        private List<OdsTab> TabelaOdsetekUstawowych;

        private void GetKontoEPU()
        {
            _kepu = (from z in lexena.KontoEPU
                     where z.Id.Equals(idKontaEPU)
                     select z).FirstOrDefault();

        }

        public  DokEPU()
        {
            lexena = new LexEnaMeritumEntities();
        }

        public int AddDokToDb()
        {
            DokWys dw;

            if (this._dokEPU == null) return -1; // brak takiego 
            if (this.lexena == null)  lexena = new LexEnaMeritumEntities();
            try
            {
                dw = new DokWys();
                dw.d_kreacji = DateTime.Now;
                dw.DataDok = DateTime.Today;
                dw.Nazwa = EPUTypesDictionary.DocNameDictionary(this._dokEPU.Rodzaj);
                dw.Sprawa_id = _dokEPU.IDSprawyProEPU;
                dw.StatusDok = 1;  //Projekt ?
                this.dokSerialized = ToXMLSerializers.SerializeToString(_dokEPU, typeof(DokumentEPU),true);
                dw.Tresc = this.dokSerialized;
                dw.TypDok = _dokEPU.Rodzaj;
                lexena.AddToDokWys(dw);
                lexena.SaveChanges();

            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        public int AddPozewToDb()
        {


            return 0;
        }

        public int AddWniosekToDb()
        {
            DokWys dw;

            if (this.wniosek == null) return -1; // brak takiego 
            if (this.lexena == null) lexena = new LexEnaMeritumEntities();
            try
            {
                dw = new DokWys();
                dw.d_kreacji = DateTime.Now;
                dw.DataDok = Convert.ToDateTime(wniosek.dataWniosku);
                dw.Nazwa = EPUTypesDictionary.DocNameDictionary(30); // wmniosek egzekucyjny
                dw.Sprawa_id = this.Sprawa_id;
                dw.StatusDok = 2;  //Projekt ?
                dw.RodzajDok = 30;
                this.dokSerialized = ToXMLSerializers.SerializeToString(wniosek, typeof(WniosekEgzekucyjny), true);
                dw.Tresc = this.dokSerialized;
                XMLValidator xmlvalid= new XMLValidator();
                xmlvalid.ValidateDokumentZEPU(this.dokSerialized, 30);
                dw.TypDok = 30;
                lexena.AddToDokWys(dw);
                lexena.SaveChanges();

            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

          private List<typRoszczenie> _buildRoszczenie(Naleznosc nal)
            {
                // dodanie roszczenia w przypadku wpłat
                int _idWiena;
                DateTime _dataWpl;
                bool found;
                WplataPodz wpPodzMain;
                Decimal _kwota;
                Wplata wpl;
                 List<typRoszczenie> roszcz = new List<typRoszczenie>();
                typRoszczenie _ro;
                typOkresOdsetkowy  _ods;
                decimal zalOds, zalKapital;
                decimal sumaWplatKapital, sumaWplatOdsetki; 
                _dataWpl = new DateTime();
                _idWiena = 0; 
                wpPodzMain = new WplataPodz();
               
                if (nal.WplataPodz.Count > 0)
                { // znajdź wplata podz z najwyższą datą ( IdWiena)
                    sumaWplatKapital =  0 ;
                    sumaWplatOdsetki  = 0;
                    foreach (var wplpo in nal.WplataPodz)
                    {
                        found = false;
                        sumaWplatKapital += (decimal)wplpo.SplataKapital;
                        sumaWplatOdsetki += (decimal)wplpo.SplataOdsetki;
                        wpl = _sprawa.Wplata.FirstOrDefault();
                        foreach (var w in _sprawa.Wplata)
                        {
                            if (w.Id == wplpo.Wplata_Id)
                            {
                                wpl = w as Wplata;
                                found = true;
                                break;
                            }

                        }
                        if (found)
                        {
                            if (wpl.DataWplaty > _dataWpl || ((wpl.DataWplaty == _dataWpl) && (wpl.IdWiena > _idWiena)))
                            {
                                wpPodzMain = wplpo;
                                _dataWpl = (DateTime)wpl.DataWplaty;
                                _idWiena = (int)wpl.IdWiena;
                                _kwota = (decimal)wpl.Kwota;
                            }

                        }
                      }
                     if      (wpPodzMain.Id > 0 )
                     { // jeśli jest wpłata podz
                         zalOds = (decimal)wpPodzMain.ZalegloscOdsetki - (decimal)wpPodzMain.SplataOdsetki;
                         zalKapital = (decimal)wpPodzMain.ZalegloscKapital - (decimal)wpPodzMain.SplataKapital;
                         if (zalKapital  > 0)
                         {
                             _ro = new typRoszczenie();
                             _ro.Wartosc = zalKapital;
                             _ro.Waluta  = typWaluta.PLN;
                             _ro.Solidarnie = (int) nal.CzySolidarnie;
                             _ro.Typ = (int) nal.TypRoszczenia;
                             _ro.Odsetki = new System.Collections.ObjectModel.ObservableCollection<typOkresOdsetkowy>();
                             if (nal.Odsetki.Count > 0 )
                             {   
                                 _ro.czyodsetki = 1 ;
                                 _ods = new typOkresOdsetkowy ();
                                 _ods.Od_Wniesienia = 1;
                                 _ods.DataOd = new DateTime(2000, 1, 1); 
                                 _ods.Do_Zaplaty = 1;
                                 if (nal.Odsetki.FirstOrDefault().NazwyOdsetek_Id == 2) // ustawowe 
                                        _ods.CzyUstawowe = 0;
                                    else
                                    {
                                        _ods.CzyUstawowe = 1;
                                        _ods.Stopa = (decimal)nal.Odsetki.FirstOrDefault().Proc0;
                                        _ods.Okres = (int)nal.Odsetki.FirstOrDefault().TypStopy;
                                    }
                                 
                                 _ro.Odsetki.Add(_ods);  // od wnisienia do  dnia zapłaty
                             }
                             else
	                                _ro.czyodsetki = 0;
                             _ro.Opis = "należność główna wynikająca z ";
                              if (nal.TypNaleznosci.CzyOdsKapital == 1)
                                _ro.Opis += " noty odsetkowej ";
                             else
                                _ro.Opis += " faktury VAT ";
                              _ro.Opis += " nr " + nal.opis  + " pozostała po uiszczeniu kwoty "   + sumaWplatKapital.ToString("C", new CultureInfo("pl-PL"));
                                
                             roszcz.Add(_ro);     
                             }

                         if (((zalOds) > 0 || (zalKapital) > 0) && nal.TypNaleznosci.CzyOdsKapital != 1)      // jeśli to jest faktura a nie odsetki skapitalizowena 
                        {
                      
                           TimeSpan tms;
                           DateTime tmpDate, tmpDate2;
                           OdsTab _odsTabSave;
                           
                           int inFlag = 0;
                           decimal odsKwt = 0 ;

                          
                           tms = new TimeSpan();
                           _odsTabSave = TabelaOdsetekUstawowych.FirstOrDefault<OdsTab>();
                            odsKwt = 0 ;
                            foreach (var  _odsTab   in TabelaOdsetekUstawowych)
                            {
                                if (
                                    ((_odsTab as OdsTab).DataP <= _dataWpl) &&
                                      (((_odsTab as OdsTab).DataK >= _dataWpl) || ((_odsTab as OdsTab).DataK == null))
                                    )
                                {   inFlag = 1 ;
                                    _odsTabSave = _odsTab;
                                }
                                if (inFlag == 1 )
                                {
                                    inFlag = 1 ;
                                    if (_odsTab.DataK == null )
                                        tms = _pozew.DataZlozenia.Subtract(_dataWpl);        
                                    else
                                        {
                                         tmpDate =  Convert.ToDateTime(_odsTabSave.DataK); //  .DataK);
                                         tms = (tmpDate).Subtract(_dataWpl);
                                        }
                                
                                
                                }
                                if  ( inFlag > 1 ) 
                                    {
                                        if (_odsTab.DataK == null )
                                        {
                                         tmpDate =  Convert.ToDateTime(_odsTab.DataP); //  .DataK);
                                         tms = _pozew.DataZlozenia.Subtract(tmpDate);
                                        }else
                                        {
                                            tmpDate =  Convert.ToDateTime(_odsTab.DataK);
                                            tmpDate2 = Convert.ToDateTime(_odsTab.DataP);
                                            tms = tmpDate.Subtract (tmpDate2);
                                        }
                                            
                                     }
                                if (inFlag > 0 ) 
                                odsKwt += zalKapital*(decimal)_odsTab.Proc0 / 36500 * tms.Days; 
                                                                   
                                if (inFlag > 0) inFlag ++;

                            }
                            odsKwt = Math.Round(odsKwt, 2);
                            zalOds += odsKwt;
                            if (zalOds > 0)
                            {
                                _ro = new typRoszczenie();
                                _ro.Wartosc = zalOds;
                                _ro.Waluta = typWaluta.PLN;
                                _ro.Solidarnie = (int)nal.CzySolidarnie;
                                _ro.Typ = (int)nal.TypRoszczenia;
                                _ro.Odsetki = new System.Collections.ObjectModel.ObservableCollection<typOkresOdsetkowy>();
                                _ro.Opis = "Skapitalizowana kwota odsetek za opóźnienie w zapłacie kwoty głównej w wysokości " + nal.kwota.Value.ToString("C", new CultureInfo("pl-PL")) + " wynikajacej z faktury VAT " + nal.opis; 
                                if (zalKapital <= 0)
                                _ro.Opis +=  " odsetki wyliczone za okres od " +  Convert.ToDateTime(nal.data_n).AddDays(1).ToString("yyyy-MM-dd") +
                                           " do dnia" + _dataWpl.AddDays(-1).ToString("yyyy-MM-dd");
                                else
                                _ro.Opis +=  " odsetki wyliczone za okres od " +  Convert.ToDateTime(nal.data_n).AddDays(1).ToString("yyyy-MM-dd") +
                                           " do dnia" + _pozew.DataZlozenia.AddDays(-1).ToString("yyyy-MM-dd");
                                
                                // za jaki okres , dodac  wpłate jako dowód wpłata na kwoatę 
                                if (nal.Odsetki.Count > 0)
                                {
                                    _ro.czyodsetki = 1;
                                    _ods = new typOkresOdsetkowy();
                                    _ods.Od_Wniesienia = 1;
                                    _ods.DataOd = new DateTime(2000, 1, 1); 
                                    _ods.Do_Zaplaty = 1;
                                    if (nal.Odsetki.FirstOrDefault().NazwyOdsetek_Id == 2) // ustawowe 
                                        _ods.CzyUstawowe = 0;
                                    else
                                    {
                                        _ods.CzyUstawowe = 1;
                                        _ods.Stopa = (decimal)nal.Odsetki.FirstOrDefault().Proc0;
                                        _ods.Okres = (int)nal.Odsetki.FirstOrDefault().TypStopy;
                                    }
                                    _ro.Odsetki.Add(_ods);  // od wnisienia do  dnia zapłaty
                                }
                                else
                                    _ro.czyodsetki = 0;
                                roszcz.Add(_ro);     
                            
                            }

                        }
                     }
                
                }
                


                return roszcz;
            }
 

        public int AddPozewEPU(int IdSprawy, DateTime DataWniosku, int _idKontaEPU, int generatemode)
        {

            typStrona _powod;
            typAdres _adres;
            typInstytucja _instytucja;
            typOsobaFizyczna _osoba;
            typStrona _pozwany;
            typInnyRejestr _innyRejestr;
            typDowod _dowod;
            typOsoba _podpis;
            typRoszczenie _roszczenie;
            typOkresOdsetkowy _odsetki;
            List<int> nr_dowodow = new List<int>();
            List<typRoszczenie> listaPoWplacie;
            int i;
            int j;
            bool czyfiz = false;
            bool czypraw = false;
            bool czydzial = false;
            List<LexEnaTrs.Web.Slownik> fakty;
            string return_status;
            decimal Oplata;
            decimal WPS;
            decimal InneKoszty;
            DateTime dZlozenia;
            Naleznosc _myNal;
            TypNaleznosci _typNal;
            Odsetki _myodsetki;
            WplataPodz _wplpodz;
            try
            {

                


                this.idKontaEPU = _idKontaEPU;
                GetKontoEPU();
                if (_kepu == null) return -3;
 

                _sprawa  = ( from c in lexena.Sprawa.Include("DaneDluznika").Include("Naleznosc").Include("Wplata").Include("JednostkaOrg") 
                             where c.id == IdSprawy 
                             select c).FirstOrDefault();

                if (_sprawa.Naleznosc.Count <= 0)
                {
                    return_status = "Błąd  -  podczas generacji pozwu - brak należności " + _sprawa.sygnatura;
                    return -107;

                }
                else

                { 
                      foreach (var r in _sprawa.Naleznosc)
                      {
                         
                          _myNal = ( from x in lexena.Naleznosc.Include("Odsetki").Include("WplataPodz").Include("TypNaleznosci")
                                     where x.Id == r.Id select x).FirstOrDefault();

                          if (_myNal != null)
                          {
                              _typNal = new TypNaleznosci();
                              _typNal.CzyOdsKapital = _myNal.TypNaleznosci.CzyOdsKapital;
                              _typNal.TypNal = _myNal.TypNaleznosci.TypNal;
                              _typNal.IdWiena = _myNal.TypNaleznosci.IdWiena;
                              _typNal.CzyProc = _myNal.TypNaleznosci.CzyProc;
                              _typNal.TabOds = _myNal.TypNaleznosci.TabOds;
                              _typNal.id = _myNal.TypNaleznosci.id;
                              r.TypNaleznosci = _typNal;
                            // przepisz odsetki
                            foreach (var ods in _myNal.Odsetki)
                            {
                                _myodsetki = new Odsetki();
                                _myodsetki.DataK = ods.DataK;
                                _myodsetki.DataPocz = ods.DataPocz;
                                _myodsetki.DoZaplaty = ods.DoZaplaty;
                                _myodsetki.Id = ods.Id;
                                _myodsetki.IdWiena = ods.IdWiena;
                                _myodsetki.Kod = ods.Kod;
                                _myodsetki.Naleznosc_Id = ods.Naleznosc_Id;
                                _myodsetki.NazwyOdsetek = ods.NazwyOdsetek;
                                _myodsetki.OdWniesienia = ods.OdWniesienia;
                                if (ods.OdWniesienia == 1) _myodsetki.DataPocz = new DateTime(2000, 1, 1);
                                _myodsetki.NazwyOdsetek_Id = ods.NazwyOdsetek_Id;
                                _myodsetki.Opis = ods.Opis;
                                _myodsetki.PartitionKey = ods.PartitionKey;
                                _myodsetki.Proc0 = ods.Proc0;
                                _myodsetki.TypStopy = ods.TypStopy;

                                r.Odsetki.Add(_myodsetki);
                            }
                            foreach (var wp in _myNal.WplataPodz)
                            {
                                _wplpodz = new WplataPodz();
                                _wplpodz.Id = wp.Id;
                                _wplpodz.IdWiena = wp.IdWiena;
                                _wplpodz.Naleznosc_Id = wp.Naleznosc_Id;
                                _wplpodz.PartitionKey = wp.PartitionKey;
                                _wplpodz.SplataKapital = wp.SplataKapital;
                                _wplpodz.SplataOdsetki = wp.SplataOdsetki;
                                _wplpodz.SplataVat = wp.SplataVat;
                                _wplpodz.Wplata_Id = wp.Wplata_Id;
                                _wplpodz.ZalegloscKapital = wp.ZalegloscKapital;
                                _wplpodz.ZalegloscOdsetki = wp.ZalegloscOdsetki;
                                _wplpodz.ZalegloscVat = wp.ZalegloscVat;


                                r.WplataPodz.Add(_wplpodz);

                            }



                        }


                      }
                 
                }


                fakty = (from c in lexena.Slownik where c.Typ == 2 select c).ToList<LexEnaTrs.Web.Slownik>(); 


                TabelaOdsetekUstawowych = (from c in lexena.OdsTab orderby c.DataP descending select c).ToList<OdsTab>();
                                            

                
                _pozew = new PozewEPU();  // nie ma daty złożenia
                _pozew.DataZlozenia = DateTime.Today;
                _pozew.Oswiadczenie = "nie";
                _pozew.version = "2.0";
                _pozew.ID = IdSprawy;
                if (generatemode > 0 && _sprawa.NrEwid != null) // jeśli brudny pozew
                    _pozew.SprawaWgPowoda = _sprawa.sygnatura + "@" + _sprawa.id.ToString() + "#" + @_sprawa.NrEwid.Trim();
                else
                    _pozew.SprawaWgPowoda = _sprawa.sygnatura + "@" + _sprawa.id.ToString();


                _pozew.Adresat = new typAdresat();
                _pozew.Adresat.ID = 1;   // sąd 
                _pozew.Adresat.Nazwa = "Sąd Rejonowy Lublin-Zachód w Lublinie";
                _pozew.Adresat.Wydzial = "VI Wydział Cywilny";
                _pozew.Adresat.Adres = new typAdres();
                _pozew.Adresat.Adres.Kraj = "PL";
                _pozew.Adresat.Adres.Gmina = "Lublin";
                _pozew.Adresat.Adres.Miejscowosc = "Lublin";
                _pozew.Adresat.Adres.Kod = "20070";
                _pozew.Adresat.Adres.Poczta = "Lublin";
                _pozew.Adresat.Adres.NrDomu = "13";
                _pozew.Adresat.Adres.Ulica = "ul. Boczna Lubomelskiej";
                _pozew.Adresat.Adres.Wojewodztwo = "lubelskie";

                _pozew.OsobaSkladajaca = new typSkladajacy();
                _pozew.OsobaSkladajaca.Adres = new typAdres();
                _pozew.OsobaSkladajaca.pelnomocnik =  _kepu.CzyZawodowy;
                _pozew.OsobaSkladajaca.podstawa = _kepu.Pelnomocnictwo;
                _pozew.OsobaSkladajaca.Nazwa = _kepu.Nazwa;
                _pozew.OsobaSkladajaca.Osoba = new typOsoba();
                _pozew.OsobaSkladajaca.Osoba.Imie = _kepu.Imie;
                _pozew.OsobaSkladajaca.Osoba.Imie2 = _kepu.Imie2;
                _pozew.OsobaSkladajaca.Osoba.Nazwisko = _kepu.Nazwisko;
                _pozew.OsobaSkladajaca.Osoba.PESEL = _kepu.PESEL;
                _pozew.OsobaSkladajaca.Osoba.stanowisko = _kepu.Stanowisko;
                _pozew.OsobaSkladajaca.Adres = new typAdres();
                _pozew.OsobaSkladajaca.Adres.Kraj = "PL";
                _pozew.OsobaSkladajaca.Adres.Kod = _kepu.kod_pocztowy;
                _pozew.OsobaSkladajaca.Adres.Miejscowosc = _kepu.miejscowosc;
                _pozew.OsobaSkladajaca.Adres.NrDomu = _kepu.numer_domu;
                _pozew.OsobaSkladajaca.Adres.NrMieszkania = _kepu.numer_mieszkania;
                _pozew.OsobaSkladajaca.Adres.Poczta = _kepu.poczta;
                _pozew.OsobaSkladajaca.Adres.Ulica = _kepu.ulica;
                _pozew.OsobaSkladajaca.Adres.Wojewodztwo = _kepu.wojewodztwo;
                _pozew.RachunekDoZwrotuOplat = new typRachunekDoZwrotuOplat();
                _pozew.RachunekDoZwrotuOplat.NumerRachunkuDoZwrotuOplat = _sprawa.JednostkaOrg.konto;
                _pozew.RachunekDoZwrotuOplat.WlascicielRachunku = _sprawa.JednostkaOrg.Nazwa;


                _pozew.ListaPowodow = new System.Collections.ObjectModel.ObservableCollection<typStrona>();
                _powod = new typStrona();
                _adres = new typAdres();
                _adres.Kraj = "PL";
                _adres.Miejscowosc = _sprawa.JednostkaOrg.Miejscowosc;
                _adres.NrDomu = _sprawa.JednostkaOrg.Nr_domu;
                if (_adres.NrDomu == null) _adres.NrDomu = "";
                _adres.NrMieszkania = _sprawa.JednostkaOrg.Nr_mieszkania;
                _adres.Poczta = _sprawa.JednostkaOrg.Poczta;
                _adres.Ulica = _sprawa.JednostkaOrg.Ulica;
                _adres.Wojewodztwo = _sprawa.JednostkaOrg.Wojewodztwo;
                _adres.Kod = _sprawa.JednostkaOrg.Kod;
                _powod.Adres = new System.Collections.ObjectModel.ObservableCollection<typAdres>();
                _powod.Adres.Add(_adres);
                _powod.ID = _sprawa.JednostkaOrg.Id;
                _powod.numerKonta = _sprawa.JednostkaOrg.konto;
                _powod.BrakNumerowIdentyfikacyjnych = false;
                _powod.Obcokrajowiec = false;
                _powod.ObcokrajowiecString = "";
                _powod.Reprezentacja = 1;
                _powod.RodzajStrony = 2;   // osoba prawna;
                _powod.NIP = _sprawa.JednostkaOrg.NIP;
                _instytucja = new typInstytucja();  // osoba prawna; - powód
                _instytucja.Nazwa = _sprawa.JednostkaOrg.Nazwa;
                _instytucja.Siedziba = _sprawa.JednostkaOrg.Siedziba;
                _instytucja.REGON = _sprawa.JednostkaOrg.REGON;
                _instytucja.CzyRejestr = 1;
                _instytucja.Item = _sprawa.JednostkaOrg.KRS;
                _powod.Item = _instytucja;
                _pozew.ListaPowodow.Add(_powod);

                _pozew.ListaPodpisow = new System.Collections.ObjectModel.ObservableCollection<typOsoba>();
                _podpis = new typOsoba();
                _podpis.Imie = _kepu.Imie;
                _podpis.Imie2 = _kepu.Imie2;
                _podpis.Nazwisko = _kepu.Nazwisko;
                _podpis.PESEL = _kepu.PESEL;
                _podpis.stanowisko = _kepu.Stanowisko;
                _pozew.ListaPodpisow.Add(_podpis);


                _pozew.ListaPozwanych = new System.Collections.ObjectModel.ObservableCollection<typStrona>();
                foreach (DaneDluznika dd in _sprawa.DaneDluznika)
                {
                    _pozwany = new typStrona();
                    _pozwany.ID = dd.id;
                    _pozwany.NIP = dd.nip;
                    _pozwany.Reprezentacja = 1;
                    _pozwany.RodzajStrony = (int)dd.FizPraw;
                    _adres = new typAdres();
                    _adres.Kod = dd.kod_pocztowy;
                    _adres.Kraj = "PL";
                    _adres.Miejscowosc = dd.miejscowosc;
                    _adres.NrDomu = dd.numer_domu;
                    if (_adres.NrDomu == null) _adres.NrDomu = "";
                    _adres.NrMieszkania = dd.numer_mieszkania;
                    _adres.Poczta = dd.poczta;
                    _adres.Wojewodztwo = dd.wojewodztwo;
                    _adres.Ulica = dd.ulica;
                    if (generatemode > 0) // jeśli brudny pozew
                    {
                        ;//_adres.Powiat = _sprawa.NrEwid;
                    }
                    // to jest w konstruktorze
                    _pozwany.Adres = new System.Collections.ObjectModel.ObservableCollection<typAdres>();
                    _pozwany.Adres.Add(_adres);
                    if (dd.FizPraw == 0 || dd.FizPraw == 1) // odoba fizyczna
                    {
                        _osoba = new typOsobaFizyczna();
                        _osoba.Imie = dd.Imie;
                        _osoba.Nazwisko = dd.Nazwisko;
                        _osoba.Imie2 = dd.Imie2;
                        _osoba.PESEL = dd.Pesel;
                        czyfiz = true;
                        if (dd.FizPraw == 1)
                        {
                            _osoba.Nazwa = dd.Nazwa;
                            czydzial = true;
                        }
                        _pozwany.Item = _osoba;
                    }
                    else // osoba prawna
                    {
                        _instytucja = new typInstytucja();  // osoba prawna; - powód
                        _instytucja.Nazwa = dd.Nazwa;
                        if (!string.IsNullOrWhiteSpace(dd.siedziba))
                             _instytucja.Siedziba = dd.siedziba;
                        _instytucja.REGON = dd.regon;
                        czypraw = true;
                        _instytucja.CzyRejestr = (int)dd.czyrejestr;

                        switch ((int)dd.czyrejestr)
                        {
                            case 1:  // krs
                                _instytucja.Item = dd.krs.Trim();
                                break;
                            case 2:
                                _innyRejestr = new typInnyRejestr();
                                _innyRejestr.Organ = dd.organ_rejestru;
                                _innyRejestr.Numer = dd.numer_rejestru;
                                _innyRejestr.Typ = dd.typ_rejestru;
                                _instytucja.Item = _innyRejestr;
                                _instytucja.CzyRejestr = 1;
                                _instytucja.Item = _innyRejestr;
                                // tu zmiana - uwaga !!!!!  podmian  2 na 1  do serializacji 
                                break;
                            default:
                                _instytucja.CzyRejestr = 0;
                                break;
                        }
                        _pozwany.Item = _instytucja;
                        //_pozew.ListaPozwanych.Add
                    }
                    _pozew.ListaPozwanych.Add(_pozwany);
                }
                // lista dowodów
                i = 0;
                _pozew.ListaDowodow = new System.Collections.ObjectModel.ObservableCollection<typDowod>();
                foreach (var rr in _sprawa.Naleznosc)
                {  // faktury 
                    if (rr.TypNaleznosci.TypNal != 1) continue;

                    _dowod = new typDowod();
                    _dowod.Numer = ++i;
                    if (rr.data_dok != null)
                        try
                        {
                            _dowod.DataDowodu = ((DateTime)(rr.data_dok)).ToString("yyyy-MM-dd"); //(DateTime)rr.data_dok; 
                            if (_dowod.DataDowodu == "")
                                _dowod.DataDowodu = null;
                        }
                        catch
                        {
                            _dowod.DataDowodu = null;    
                        }
                    else
                    {
                        if (generatemode == 0)
                        {
                            // return_status = "Należność  w sprawie " + _sprawa.sygnatura + " brak daty dokumentu ";
                            // return -100;
                        }
                    }

                    _dowod.Oznaczenie = rr.opis;
                    _dowod.Opis = rr.opis2;
                    if (rr.TypNaleznosci.CzyOdsKapital == 1)
                    {// nota odsetkowa z dnia
                        _dowod.TypDowodu = typRodzajDowodu.inny;
                        _dowod.FaktStwierdzany = "Istnienie zobowiązania, jego wysokość i termin zapłaty";
                        _dowod.Opis = "Nota odsetkowa na kwotę " + rr.kwota.Value.ToString("C", new CultureInfo("pl-PL")) + " z terminem płatności przypadajacym na " + ((DateTime)(rr.data_n)).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        _dowod.TypDowodu = typRodzajDowodu.faktura;
                        _dowod.Opis = "Faktura VAT na kwotę " + rr.kwota.Value.ToString("C", new CultureInfo("pl-PL")) + " z terminem płatności przypadającym na " + ((DateTime)(rr.data_n)).ToString("yyyy-MM-dd");
                        _dowod.FaktStwierdzany = "Istnienie zobowiązania, jego wysokość i termin zapłaty";
                    }
                    _pozew.ListaDowodow.Add(_dowod);
                }
                foreach (var rx in _sprawa.DokOdebr)
                {  // pozostałe dowody 
                    _dowod = new typDowod();
                    _dowod.Numer = ++i;
                    if (rx.DataDokumentu != null)
                        _dowod.DataDowodu = ((DateTime)(rx.DataDokumentu)).ToString("yyyy-MM-dd"); // (DateTime)rx.DataDokumentu; 
                    _dowod.Opis = rx.Nazwa;
                    _dowod.TypDowodu = typRodzajDowodu.inny;
                    _dowod.FaktStwierdzany = "Istnienie zobowiązania";
                    _pozew.ListaDowodow.Add(_dowod);
                }
                 

                foreach (var rf in fakty)
                {

                    switch (rf.filtr)
                    {
                        case 0:
                            continue;
                        case 1: // zawsze dołączaj
                            break;
                        case 2:  // jeśli oczoba rowana
                            if (czypraw)
                                break;
                            else
                                continue;
                        case 3:   // jeśli działąlnośc gospodarcza
                            if (czydzial)
                                break;
                            else
                                continue;
                        case 4:
                            if (czyfiz)
                                break;
                            else
                                continue;
                        default:
                            continue;
                    }
                    _dowod = new typDowod();
                    _dowod.Numer = ++i;
                    if (rf.Nazwa.ToLower().Contains("umowa"))
                        _dowod.TypDowodu = typRodzajDowodu.umowa;
                    else
                        _dowod.TypDowodu = typRodzajDowodu.inny;
                    _dowod.Opis = rf.Nazwa;
                    _dowod.FaktStwierdzany = rf.Tresc;
                    nr_dowodow.Add(_dowod.Numer);
                    _pozew.ListaDowodow.Add(_dowod);
                }

                // Dowody dopisane z liosty 
                _pozew.ListaRoszczen = new System.Collections.ObjectModel.ObservableCollection<typRoszczenie>();
                // roszczenia
                j = 0;
                foreach (var ry in _sprawa.Naleznosc)
                {
                    if (ry.TypNaleznosci.TypNal != 1) continue;    
                    _roszczenie = new typRoszczenie();
                    _roszczenie.Numer = ++j;
                    _roszczenie.Waluta = typWaluta.PLN;
                    _roszczenie.dataWymagalnosci = Convert.ToDateTime(ry.data_n).ToString("yyyy-MM-dd");
                    if (ry.WplataPodz.Count > 0)  // były wpłaty - będzie inna zaległość 
                    {

                        listaPoWplacie = _buildRoszczenie(ry);
                        if (listaPoWplacie.Count > 0)
                        {
                            _roszczenie = listaPoWplacie.First();
                            _roszczenie.Numer = j;
                            _roszczenie.Dowody = new System.Collections.ObjectModel.ObservableCollection<int>();
                            _roszczenie.Dowody.Add(j);  // jeden dowód domyślnie
                            foreach (int ix in nr_dowodow)
                            {
                                _roszczenie.Dowody.Add(ix);
                            }
                            _pozew.ListaRoszczen.Add(_roszczenie);
                            if (listaPoWplacie.Count == 2)
                            {
                                _roszczenie = listaPoWplacie.Last();
                                _roszczenie.Numer = j;
                                _roszczenie.Dowody = new System.Collections.ObjectModel.ObservableCollection<int>();
                                _roszczenie.Dowody.Add(j);  // jeden dowód domyślnie
                                foreach (int ix in nr_dowodow)
                                {
                                    _roszczenie.Dowody.Add(ix);
                                }
                                _pozew.ListaRoszczen.Add(_roszczenie);

                            }
                            continue;
                        }
                        else
                            continue;
                    }
                    else
                        _roszczenie.Wartosc = (decimal)ry.kwota;

                    if (ry.Odsetki.Count > 0)
                        _roszczenie.czyodsetki = 1;
                    else
                        _roszczenie.czyodsetki = 0;
                    _roszczenie.Solidarnie = (int)ry.CzySolidarnie;
                    _roszczenie.Typ = (int)ry.TypRoszczenia;
                    _roszczenie.Dowody = new System.Collections.ObjectModel.ObservableCollection<int>();
                    _roszczenie.Dowody.Add(j);  // jeden dowód domyślnie
                    foreach (int ix in nr_dowodow)
                    {
                        _roszczenie.Dowody.Add(ix);
                    }
                    _roszczenie.Odsetki = new System.Collections.ObjectModel.ObservableCollection<typOkresOdsetkowy>();
                    if (ry.Odsetki.Count > 0)  // jeśli sa odsetki
                    {
                        foreach (var rz in ry.Odsetki)
                        {
                            _odsetki = new typOkresOdsetkowy();//_odsetki = new
                            if (rz.NazwyOdsetek_Id == 2) // ustawowe 
                                _odsetki.CzyUstawowe = 0;
                            else
                            {
                                _odsetki.CzyUstawowe = 1;
                                _odsetki.Stopa = (decimal)rz.Proc0;
                                _odsetki.Okres = (int)rz.TypStopy;
                            }
                            if (ry.TypNaleznosci.CzyOdsKapital == 1) // jesli odseki skapitalizowane to zmiana daty : od wniesienia pozwu 
                            {
                                rz.OdWniesienia = 1;
                                _odsetki.DataOd = new DateTime(2000, 1, 1);
                            }
                            if (rz.OdWniesienia == 1)
                            {
                                _odsetki.Od_Wniesienia = 1;
                                _odsetki.DataOd = new DateTime(2000, 1, 1);
                            }
                            else
                            {
                                _odsetki.Od_Wniesienia = 0;
                                _odsetki.DataOd = (DateTime)rz.DataPocz;
                            }
                            if (rz.DoZaplaty == 1)
                                _odsetki.Do_Zaplaty = 1;
                            else
                            {
                                _odsetki.Do_Zaplaty = 0;
                                _odsetki.DataDo = (DateTime)rz.DataK;
                            }

                            // dodać kwotę jeśli   są wpłaty - tu trzeba poprawić

                            //if
                            // dołączenie dowodów
                            _roszczenie.Odsetki.Add(_odsetki);

                        }

                    }
                    _pozew.ListaRoszczen.Add(_roszczenie);
                }

                // opłata sądowa
                _pozew.OplataSadowa = new typOplata();
                _pozew.ObliczSumy();
                Oplata = _pozew.OplataSadowa.WartoscOplaty;
                WPS = _pozew.WartoscSporu;
                dZlozenia = _pozew.DataZlozenia;
                _pozew.OplataSadowa.Zasadzenie = 1;
                _pozew.OplataSadowa.Zwolnienie = 0;
                _pozew.KosztyZastepstwa = new typKoszty();
                _pozew.KosztyZastepstwa.Zasadzenie = 1;
                _pozew.KosztyZastepstwa.WgNorm = 1;
                // oplata od prowizji 
                /*
                _pozew.InneKoszty = new typKoszty();
                _pozew.InneKoszty.WgNorm = 0;
                _pozew.InneKoszty.Zasadzenie = 1;
                _pozew.InneKoszty.Opis = "opłata manipulacyjna dla dostawcy usług płatności";
                _pozew.InneKoszty.Wartosc = 0; //  EPUCalc.countProwizja(Oplata);
                */
                InneKoszty = 0;// _pozew.InneKoszty.Wartosc;
                _pozew.Uzasadnienie = " ";




            }  // try
            catch (Exception ex)
            {
               return_status = "Błąd podczas generacji pozwu " + _sprawa.sygnatura;
                return  -7;
                
            }

            
            return 0;
        }
       
        public int AddWniosekEgzekucyjnyEPU(int IdSprawy, DateTime DataWniosku, int _idKontaEPU, int SzablonId)
        {

            KancelariaKomornicza kanckom;
            
            DokWys  mypozew;
            DokOdebr mynakaz, myklauzula;
            PozewEPU mypozewEPU;
            NakazEPU nak;
            typStrona osfrm;
            IQueryable<DaneDluznika> dlulst;
            DaneDluznika dd;
            OrzeczenieEPU klauzulaEPU;
            Sprawa _sprawa;
            SposobyEgzSlownik sposegz;
            
            bool czyfiz, czydzial, czypraw;

            try
            {

                this.idKontaEPU = _idKontaEPU;
                GetKontoEPU();
                if (_kepu == null) return -3;


                kanckom = (from z in lexena.SposobyEgzSlownik
                           join y in lexena.KancelariaKomornicza on z.KancelariaKomornika_Id equals y.IdEPU
                           where z.Id.Equals(SzablonId)
                           select y).FirstOrDefault();

                sposegz = (from z in lexena.SposobyEgzSlownik where z.Id.Equals(SzablonId) select z).FirstOrDefault();

                mypozew = (from z in lexena.DokWys
                           where z.Sprawa_id == (IdSprawy) && z.TypDok == 10
                           select z).OrderByDescending(z => z.DataDok).FirstOrDefault();

                mynakaz = (from z in lexena.DokOdebr
                           where z.Sprawa_id == IdSprawy && z.TypDok == 5
                           select z).OrderByDescending(z => z.DataDokumentu).FirstOrDefault();
                myklauzula = (from z in lexena.DokOdebr
                              where z.Sprawa_id == IdSprawy && z.TypDok == 17
                              select z).OrderByDescending(z => z.DataDokumentu).FirstOrDefault();


                if (kanckom == null) return -100;  // brak kancelarii
                if (mynakaz == null) return -101;   // brak nakazu zapłaty
                if (myklauzula == null) return -102;  // brak kaluzuli 
                this.Sprawa_id = IdSprawy;

                nak = (NakazEPU)ToXMLSerializers.DeserializeFromString(mynakaz.Tresc, typeof(NakazEPU));
                mypozewEPU = (PozewEPU)ToXMLSerializers.DeserializeFromString(mypozew.Tresc, typeof(PozewEPU));

                wniosek = new WniosekEgzekucyjny();
                wniosek = new WniosekEgzekucyjny();
                wniosek.dataWniosku = DataWniosku.ToString("yyyy-MM-dd");
                wniosek.ID = 0;  //  do uzupełnienia 
                wniosek.version = "1.0";
                wniosek.Sad = new typSadEPU();
                wniosek.Sad.Nazwa = "Sąd Rejonowy Lublin-Zachód w Lublinie";
                wniosek.Sad.Wydzial = "VI Wydział Cywilny";
                wniosek.Komornik = new typKomornikWniosek();
                wniosek.Komornik.ID = (ulong)kanckom.IdEPU;
                wniosek.Komornik.Nazwa = kanckom.Nazwa;
                wniosek.ListaDluznikow = new System.Collections.ObjectModel.ObservableCollection<typDluznik>();
                wniosek.Nakaz = new typNakaz();
                wniosek.Nakaz.IDNakazu = (ulong)mynakaz.IdEPU;
                wniosek.Nakaz.DataNakazu = nak.dataNakazu;//((DateTime)(mynakaz.DataDokumentu)).ToString("yyyy-MM-dd");
                wniosek.Nakaz.KodDecyzji = 0;
                wniosek.Nakaz.Sygnatura = nak.Sygnatura;
                //wniosek.Klauzula.DataKlauzuli = 
                wniosek.ListaDluznikow = new System.Collections.ObjectModel.ObservableCollection<typDluznik>();
                dlulst = (from z in lexena.DaneDluznika
                          where z.Sprawa_Id == IdSprawy
                          select z);

                if (dlulst != null)
                {
                    foreach (var d in dlulst)
                    {
                        dd = d as DaneDluznika;
                        typDluznik dlu = new typDluznik();
                        typAdres _adres;
                        typOsobaFizyczna _osoba;
                        typInstytucja _instytucja;
                        typInnyRejestr _innyRejestr;
                        typSposobEgzekucjiSposobEgzekucji typsposegz;

                        dlu.ID = (ulong)dd.id;
                        dlu.NIP = dd.nip;
                        dlu.rodzaj = (int)dd.FizPraw;
                        _adres = new typAdres();
                        _adres.Kod = dd.kod_pocztowy;
                        _adres.Kraj = "PL";
                        _adres.Miejscowosc = dd.miejscowosc;
                        _adres.NrDomu = dd.numer_domu;
                        if (_adres.NrDomu == null) _adres.NrDomu = "";
                        _adres.NrMieszkania = dd.numer_mieszkania;
                        _adres.Poczta = dd.poczta;
                        _adres.Wojewodztwo = dd.wojewodztwo;
                        _adres.Ulica = dd.ulica;
                        dlu.Adres = new System.Collections.ObjectModel.ObservableCollection<typAdres>();
                        dlu.Adres.Add(_adres);
                        dlu.ListaSposobow = new System.Collections.ObjectModel.ObservableCollection<typSposobEgzekucjiSposobEgzekucji>();
                        if (sposegz.czybank == 1)
                        {
                            typsposegz = new typSposobEgzekucjiSposobEgzekucji();
                            typsposegz.Rodzaj = typRodzajSposobu.zrachunkówbankowych;
                            typsposegz.Opis = sposegz.bankopis;
                            dlu.ListaSposobow.Add(typsposegz);
                        }
                        if (sposegz.czynieruch == 1)
                        {
                            typsposegz = new typSposobEgzekucjiSposobEgzekucji();
                            typsposegz.Rodzaj = typRodzajSposobu.znieruchomości;
                            typsposegz.Opis = sposegz.nieruchopis;
                            dlu.ListaSposobow.Add(typsposegz);
                        }
                        if (sposegz.czyruch == 1)
                        {
                            typsposegz = new typSposobEgzekucjiSposobEgzekucji();
                            typsposegz.Rodzaj = typRodzajSposobu.zruchomości;
                            typsposegz.Opis = sposegz.ruchopis;
                            dlu.ListaSposobow.Add(typsposegz);
                        }
                        if (sposegz.czywierzyt == 1)
                        {
                            typsposegz = new typSposobEgzekucjiSposobEgzekucji();
                            typsposegz.Rodzaj = typRodzajSposobu.zwierzytelności;
                            typsposegz.Opis = sposegz.wierzytopis;
                            dlu.ListaSposobow.Add(typsposegz);
                        }

                        if (dd.FizPraw == 0 || dd.FizPraw == 1) // odoba fizyczna
                        {
                            _osoba = new typOsobaFizyczna();
                            _osoba.Imie = dd.Imie;
                            _osoba.Nazwisko = dd.Nazwisko;
                            _osoba.Imie2 = dd.Imie2;
                            _osoba.PESEL = dd.Pesel;

                            czyfiz = true;
                            if (dd.FizPraw == 1)
                            {
                                _osoba.Nazwa = dd.Nazwa;
                                czydzial = true;
                            }
                            dlu.Item = _osoba;
                            if (sposegz.czypraca == 1)
                            {
                                typsposegz = new typSposobEgzekucjiSposobEgzekucji();
                                typsposegz.Rodzaj = typRodzajSposobu.zwynagrodzeniazapracę;
                                typsposegz.Opis = sposegz.pracaopis;
                                dlu.ListaSposobow.Add(typsposegz);
                            }
                        }
                        else // osoba prawna
                        {
                            _instytucja = new typInstytucja();  // osoba prawna; - powód
                            _instytucja.Nazwa = dd.Nazwa;
                            if ( !string.IsNullOrWhiteSpace(dd.siedziba))
                            _instytucja.Siedziba = dd.siedziba;
                            if (dd.regon != null )
                            if (dd.regon.Trim ().Length > 0 )
                            _instytucja.REGON = dd.regon;
                            czypraw = true;
                            _instytucja.CzyRejestr = (int)dd.czyrejestr;

                            switch ((int)dd.czyrejestr)
                            {
                                case 1:  // krs
                                    _instytucja.Item = dd.krs.Trim();
                                    break;
                                case 2:
                                    _innyRejestr = new typInnyRejestr();
                                    _innyRejestr.Organ = dd.organ_rejestru;
                                    _innyRejestr.Numer = dd.numer_rejestru;
                                    _innyRejestr.Typ = dd.typ_rejestru;
                                    _instytucja.Item = _innyRejestr;
                                    _instytucja.CzyRejestr = 1;
                                    _instytucja.Item = _innyRejestr;
                                    // tu zmiana - uwaga !!!!!  podmian  2 na 1  do serializacji 
                                    break;
                                default:
                                    _instytucja.CzyRejestr = 0;
                                    break;
                            }
                            dlu.Item = _instytucja;
                            //_pozew.ListaPozwanych.Add
                        }
                        // jest domyślna lista sposobów egzekucji.
                        // dlu.ListaSposobow.Add(
                         wniosek.ListaDluznikow.Add(dlu);
                    }
                }
                else return -200;   // brk dłużników 
                wniosek.Klauzula = new typKlauzula();
                klauzulaEPU = (OrzeczenieEPU)ToXMLSerializers.DeserializeFromString(myklauzula.Tresc, typeof(OrzeczenieEPU));
                wniosek.Klauzula = new typKlauzula();
                wniosek.Klauzula.DataKlauzuli = klauzulaEPU.dataOrzeczenia;
                wniosek.Klauzula.IDKlauzuli = klauzulaEPU.ID;
                wniosek.Klauzula.IDNakazu = klauzulaEPU.Nakaz.IDNakazu;
                wniosek.Klauzula.KodKlauzuli = klauzulaEPU.KOD;
                wniosek.Klauzula.Sygnatura = klauzulaEPU.Sygnatura;
                // lista wierzycieli           
                wniosek.ListaWierzycieli = new System.Collections.ObjectModel.ObservableCollection<typWierzyciel>();
                typWierzyciel wierzyciel = new typWierzyciel();
                wierzyciel.Adres = new System.Collections.ObjectModel.ObservableCollection<typAdres>();
                _sprawa = (from z in lexena.Sprawa.Include("JednostkaOrg")
                           where z.id.Equals(IdSprawy)
                           select z).FirstOrDefault();
                typAdres _adresw = new typAdres();
                _adresw.Kraj = "PL";
                _adresw.Miejscowosc = _sprawa.JednostkaOrg.Miejscowosc;
                _adresw.NrDomu = _sprawa.JednostkaOrg.Nr_domu;
                if (_adresw.NrDomu == null) _adresw.NrDomu = "";
                _adresw.NrMieszkania = _sprawa.JednostkaOrg.Nr_mieszkania;
                _adresw.Poczta = _sprawa.JednostkaOrg.Poczta;
                _adresw.Ulica = _sprawa.JednostkaOrg.Ulica;
                _adresw.Wojewodztwo = _sprawa.JednostkaOrg.Wojewodztwo;
                _adresw.Kod = _sprawa.JednostkaOrg.Kod;
                wierzyciel.Adres.Add(_adresw);
                wierzyciel.ID = (ulong)_sprawa.JednostkaOrg.Id;
                wierzyciel.KontoBankowe = _sprawa.JednostkaOrg.konto;
                wierzyciel.NIP = _sprawa.JednostkaOrg.NIP;
                wierzyciel.rodzaj = 2; // osoba prawna
                typInstytucja _instytucjaw = new typInstytucja();
                _instytucjaw.Nazwa = _sprawa.JednostkaOrg.Nazwa;
                _instytucjaw.Siedziba = _sprawa.JednostkaOrg.Siedziba;
                _instytucjaw.REGON = _sprawa.JednostkaOrg.REGON;
                _instytucjaw.CzyRejestr = 1;
                _instytucjaw.Item = _sprawa.JednostkaOrg.KRS;
                wierzyciel.Item = _instytucjaw;
                wniosek.ListaWierzycieli.Add(wierzyciel);
                wniosek.ListaRoszczen = new System.Collections.ObjectModel.ObservableCollection<typRoszczenieNakaz>();
                int i = 0 ;
                foreach (var ro in nak.ListaRoszczen)
                {
                    
                    if (ro.typ == 16) continue;
                    ro.numer = ++i;
                    if (ro.Odsetki.Count > 0 )
                       foreach (var  ods in ro.Odsetki)
                       {
                           ods.Do_Zaplaty = 1;                         
                       
                       }
                    if (ro.typ == 13)
                    {

                        ro.opis = "Tytułem zwrotu kosztów postępowania";
                    }
                    else
                        ro.opis = "Należność główna";
                    wniosek.ListaRoszczen.Add(ro);
                    //(ro as typRoszczenieNakaz).

                }
                wniosek.KosztyZastepstwa = new typKoszty();
                switch (sposegz.KZA)
                {
                    case 0: // brak 
                        wniosek.KosztyZastepstwa.Zasadzenie = 0;
                        wniosek.KosztyZastepstwa.WgNorm = 0;
                        wniosek.KosztyZastepstwa.Wartosc = 0;
                        break;
                    case 1: // wg norm
                        wniosek.KosztyZastepstwa.Zasadzenie = 1;
                        wniosek.KosztyZastepstwa.WgNorm = 1;
                        wniosek.KosztyZastepstwa.Wartosc = 0;
                        break;
                    case 2:  // w wysokości
                        wniosek.KosztyZastepstwa.Zasadzenie = 1;
                        wniosek.KosztyZastepstwa.WgNorm = 0;
                        wniosek.KosztyZastepstwa.Wartosc = 0;
                        break;
                    default:
                        wniosek.KosztyZastepstwa.Zasadzenie = 0;
                        wniosek.KosztyZastepstwa.WgNorm = 0;
                        wniosek.KosztyZastepstwa.Wartosc = 0;
                        break;

                }
                if (sposegz.CzyzWyboru == 1)
                {
                    wniosek.ZlecenieProwadzeniaArt85 = 1;
                    wniosek.ZlecenieProwadzeniaArt85Specified = true;

                }
                if (sposegz.CzyMajatek == 1)
                {
                    wniosek.ZleceniePoszukiwaniaMajatku = 1;
                    wniosek.ZleceniePoszukiwaniaMajatkuSpecified = true;
                }


                wniosek.OsobaSkladajaca = new typSkladajacy();
                wniosek.OsobaSkladajaca.Adres = new typAdres();
                wniosek.OsobaSkladajaca.pelnomocnik = this._kepu.CzyZawodowy;
                wniosek.OsobaSkladajaca.podstawa = this._kepu.Pelnomocnictwo;
                wniosek.OsobaSkladajaca.Nazwa = this._kepu.Nazwa;
                wniosek.OsobaSkladajaca.Osoba = new typOsoba();
                wniosek.OsobaSkladajaca.Osoba.Imie = this._kepu.Imie;
                wniosek.OsobaSkladajaca.Osoba.Imie2 = this._kepu.Imie2;
                wniosek.OsobaSkladajaca.Osoba.Nazwisko = this._kepu.Nazwisko;
                wniosek.OsobaSkladajaca.Osoba.PESEL = this._kepu.PESEL;
                wniosek.OsobaSkladajaca.Osoba.stanowisko = this._kepu.Stanowisko;
                wniosek.OsobaSkladajaca.Adres = new typAdres();
                wniosek.OsobaSkladajaca.Adres.Kraj = "PL";
                wniosek.OsobaSkladajaca.Adres.Kod = this._kepu.kod_pocztowy;
                wniosek.OsobaSkladajaca.Adres.Miejscowosc = this._kepu.miejscowosc;
                wniosek.OsobaSkladajaca.Adres.NrDomu = this._kepu.numer_domu;
                wniosek.OsobaSkladajaca.Adres.NrMieszkania = this._kepu.numer_mieszkania;
                wniosek.OsobaSkladajaca.Adres.Poczta = this._kepu.poczta;
                wniosek.OsobaSkladajaca.Adres.Ulica = this._kepu.ulica;
                wniosek.OsobaSkladajaca.Adres.Wojewodztwo = this._kepu.wojewodztwo;
                if (sposegz.Opis != null)
                    if (sposegz.Opis.Length > 0)
                        wniosek.InformacjeDodatkowe = sposegz.Opis;
            }
            catch (Exception ex)
            {
                
                return -1;
                
            }
            

            return 1;
        }


       



        public int DocEPU(int IdSprawy, int typDok, int _idKontaEPU)
        { // generuje  dokument na bazie sprawy
            typStrona _powod;
            typAdres _adres;
            typInstytucja _instytucja;



           
            this.idKontaEPU = _idKontaEPU;
            GetKontoEPU();
            if (_kepu == null) return -3;// bład oznaczenia konta EPU
            Sprawa spr = (from z in lexena.Sprawa.Include("JednostkaOrg")
                          where z.id.Equals(IdSprawy)
                          select z).FirstOrDefault();
            if (spr == null) return -1;// brak sprawy

            this._dokEPU = new DokumentEPU();
            this._dokEPU.DataZlozenia = DateTime.Today;
            this._dokEPU.IDSprawy = (int)spr.IdSprawyEPU; // IdSprawy;
            this._dokEPU.IDSprawyProEPU = IdSprawy;
            this._dokEPU.Rodzaj = typDok;
            this._dokEPU.Sygnatura = spr.SygnNCe;
            this._dokEPU.Przedmiot = "";
            
            this._dokEPU.OsobaSkladajaca = new typSkladajacy();
            this._dokEPU.OsobaSkladajaca.Adres = new typAdres();
            this._dokEPU.OsobaSkladajaca.pelnomocnik = this._kepu.CzyZawodowy;
            this._dokEPU.OsobaSkladajaca.podstawa = this._kepu.Pelnomocnictwo;
            this._dokEPU.OsobaSkladajaca.Nazwa = this._kepu.Nazwa;
            this._dokEPU.OsobaSkladajaca.Osoba = new typOsoba();
            this._dokEPU.OsobaSkladajaca.Osoba.Imie = this._kepu.Imie;
            this._dokEPU.OsobaSkladajaca.Osoba.Imie2 = this._kepu.Imie2;
            this._dokEPU.OsobaSkladajaca.Osoba.Nazwisko = this._kepu.Nazwisko;
            this._dokEPU.OsobaSkladajaca.Osoba.PESEL = this._kepu.PESEL;
            this._dokEPU.OsobaSkladajaca.Osoba.stanowisko = this._kepu.Stanowisko;
            this._dokEPU.OsobaSkladajaca.Adres = new typAdres();
            this._dokEPU.OsobaSkladajaca.Adres.Kraj = "PL";
            this._dokEPU.OsobaSkladajaca.Adres.Kod = this._kepu.kod_pocztowy;
            this._dokEPU.OsobaSkladajaca.Adres.Miejscowosc = this._kepu.miejscowosc;
            this._dokEPU.OsobaSkladajaca.Adres.NrDomu = this._kepu.numer_domu;
            this._dokEPU.OsobaSkladajaca.Adres.NrMieszkania = this._kepu.numer_mieszkania;
            this._dokEPU.OsobaSkladajaca.Adres.Poczta = this._kepu.poczta;
            this._dokEPU.OsobaSkladajaca.Adres.Ulica = this._kepu.ulica;
            this._dokEPU.OsobaSkladajaca.Adres.Wojewodztwo = this._kepu.wojewodztwo;
            
            //this._dokEPU.PESELPelnomocnika = "74120204523";
            //this._dokEPU.Tresc = " Zawiadomienie o wypowiedzeniu pełnomocnictwa przez powoda W imieniu mojego Mocodawcy- Energa – Obrót S.A. z siedzibą w Gdańsku przy Al. Grunwaldzkiej 472, 80 – 309 Gdańsk, wpisanej do Krajowego Rejestru Sądowego – Rejestru Przedsiębiorców prowadzonego przez Sąd Rejonowy Gdańsk – Północ w Gdańsku, VII Wydział Gospodarczy pod numerem KRS 0000280916, NIP: 957-096-83-70, z kapitałem zakładowym w całości wpłaconym w wysokości 368 160 239,00 zł, zawiadamiam, że  pełnomocnictwo procesowe udzielone adwokatowi/radcy prawnemu Agnieszka Cwalina Kowalewska zostało wypowiedziane. Jednocześnie wskazuję, iż udzielone zostało nowe pełnomocnictwo procesowe Radcy Prawnemu Sylwii Zarzyckiej nr pesel 74120204523, wpisanej na listę Radców Prawnych prowadzoną przez Okręgową Izbę Radców Prawnych we Wrocławiu nr wpisu 1630, z Kancelarii CASUS ZARZYCKA & Wspólnicy Kancelaria Prawna spółka komandytowa z siedzibą we Wrocławiu przy ul. Kleczkowskiej 43 (adres do doręczeń : CASUS ZARZYCKA & Wspólnicy Kancelaria Prawna sp. komandytowa, 50-227 Wrocław, ul. Kleczkowska 43) Wnoszę o wypisanie ze sprawy aktualnego pełnomocnika procesowego oraz wpisanie Radcę Prawnego Sylwię Zarzycką, tak aby nowy pełnomocnik miał dostęp do spraw w systemie informatycznym E-Sądu.";
            this._dokEPU.Tresc = "";
            
            this._dokEPU.ListaPowodow = new System.Collections.ObjectModel.ObservableCollection<typStrona>();
            _powod = new typStrona();
            _adres = new typAdres();
            _adres.Kraj = "PL";
            _adres.Miejscowosc = spr.JednostkaOrg.Miejscowosc;
            _adres.NrDomu = spr.JednostkaOrg.Nr_domu;
            if (_adres.NrDomu == null) _adres.NrDomu = "";
            _adres.NrMieszkania = spr.JednostkaOrg.Nr_mieszkania;
            _adres.Poczta = spr.JednostkaOrg.Poczta;
            _adres.Ulica = spr.JednostkaOrg.Ulica;
            _adres.Wojewodztwo = spr.JednostkaOrg.Wojewodztwo;
            _adres.Kod = spr.JednostkaOrg.Kod;
            _powod.Adres = new System.Collections.ObjectModel.ObservableCollection<typAdres>();
            _powod.Adres.Add(_adres);
            _powod.ID = spr.JednostkaOrg.Id;
            _powod.numerKonta = spr.JednostkaOrg.konto;
            _powod.Reprezentacja = 1;
            _powod.RodzajStrony = 2;   // osoba prawna;
            _powod.NIP = spr.JednostkaOrg.NIP;
            _instytucja = new typInstytucja();  // osoba prawna; - powód
            _instytucja.Nazwa = spr.JednostkaOrg.Nazwa;
            _instytucja.Siedziba = spr.JednostkaOrg.Siedziba;
            _instytucja.REGON = spr.JednostkaOrg.REGON;
            _instytucja.CzyRejestr = 1;
            _instytucja.Item = spr.JednostkaOrg.KRS;
            _powod.Item = _instytucja;
            this._dokEPU.ListaPowodow.Add(_powod);
            // lista pozawanych 

            /*
            this._dokEPU.ListaPozwanych = new System.Collections.ObjectModel.ObservableCollection<typStrona>();
            IQueryable<DaneDluznika> LstDlu = (from z in lexena.DaneDluznika
                                                where z.Sprawa_Id == IdSprawy
                                                select z);
                        
            foreach (DaneDluznika dd in LstDlu)
            {
                _pozwany = new typStrona();
                _pozwany.ID = dd.id;
                if (dd.nip != null)
                if (dd.nip.Length > 0 )
                _pozwany.NIP = dd.nip;
                _pozwany.Reprezentacja = 1;
                _pozwany.RodzajStrony = (int)dd.FizPraw;
                _adres = new typAdres();
                _adres.Kod = dd.kod_pocztowy;
                _adres.Kraj = "PL";
                _adres.Miejscowosc = dd.miejscowosc;
                _adres.NrDomu = dd.numer_domu;
                if (_adres.NrDomu == null) _adres.NrDomu = "";
                _adres.NrMieszkania = dd.numer_mieszkania;
                _adres.Poczta = dd.poczta;
                _adres.Wojewodztwo = dd.wojewodztwo;
                _adres.Ulica = dd.ulica;
                // to jest w konstruktorze
                _pozwany.Adres = new System.Collections.ObjectModel.ObservableCollection<typAdres>();
                _pozwany.Adres.Add(_adres);
                if (dd.FizPraw == 0 || dd.FizPraw == 1) // odoba fizyczna
                {
                    _osoba = new typOsobaFizyczna();
                    _osoba.Imie = dd.Imie;
                    _osoba.Nazwisko = dd.Nazwisko;
                    _osoba.Imie2 = dd.Imie2;
                    if (dd.Pesel != null)
                    if (dd.Pesel.Length > 0) 
                    _osoba.PESEL = dd.Pesel;
                    
                    if (dd.FizPraw == 1)
                    {
                        _osoba.Nazwa = dd.Nazwa;
                        
                    }
                    _pozwany.Item = _osoba;
                }
                else // osoba prawna
                {
                    _instytucja = new typInstytucja();  // osoba prawna; - powód
                    _instytucja.Nazwa = dd.Nazwa;
                    _instytucja.Siedziba = dd.siedziba;
                    if (_instytucja.REGON != null)
                    if (_instytucja.REGON.Length > 0)
                    _instytucja.REGON = dd.regon;
                    
                    _instytucja.CzyRejestr = (int)dd.czyrejestr;

                    switch ((int)dd.czyrejestr)
                    {
                        case 1:  // krs
                            if (dd.krs != null)
                                if (dd.krs.Length > 0 )
                            _instytucja.Item = dd.krs;
                            break;
                        case 2:
                            _innyRejestr = new typInnyRejestr();
                            _innyRejestr.Organ = dd.organ_rejestru;
                            _innyRejestr.Numer = dd.numer_rejestru;
                            _innyRejestr.Typ = dd.typ_rejestru;
                            _instytucja.Item = _innyRejestr;
                            _instytucja.CzyRejestr = 1;
                            _instytucja.Item = _innyRejestr;
                            // tu zmiana - uwaga !!!!!  podmian  2 na 1  do serializacji 
                            break;
                        default:
                            _instytucja.CzyRejestr = 0;
                            break;
                    }
                    _pozwany.Item = _instytucja;
                    //_pozew.ListaPozwanych.Add
                }
                this._dokEPU.ListaPozwanych.Add(_pozwany);
            }
             */ 
          return 1; // OK
        }

      
    }


    public class DokWysService
    {
         private DokWys dw;
         private Pozew  pozew;
         private Sprawa spr;
         private int idDookWys;
         private LexEnaMeritumEntities lexena;

        private  int  readDokWys( int IdDokWys)
        {
             dw = null;
             pozew = null;
            try 
            {
             if (this.lexena == null)  lexena = new LexEnaMeritumEntities();
                dw  =  (from z in lexena.DokWys.Include("Pozew")
                          where z.Id.Equals(IdDokWys)
                          select z).FirstOrDefault(); 
              if (dw.TypDok == 10 ) // pozew
              {
                  pozew = dw.Pozew.FirstOrDefault();
              }
            }
            catch (Exception ex)
            {
                // bład odczytu 
                return -100;
            
            }
            if (dw == null) return -1 ;  // brak dokumentu
            return 1;
        }

        private int readSprawa(int IdSprawy)
        {
            
            try
            {
                if (this.lexena == null) lexena = new LexEnaMeritumEntities();
                spr = (from z in lexena.Sprawa.Include("Naleznosc")
                       where z.id.Equals(IdSprawy)
                      select z).FirstOrDefault();
               
            }
            catch (Exception ex)
            {
                // bład odczytu 
                return -100;

            }
            if (spr == null) return -1;  // brak dokumentu
            return 1;
        }
       
        public  DokWysService(int IdDokWys)
        {
            idDookWys = IdDokWys;
        }


        public int RegeneretaPozew()
        {
            int retcode;
            PozewEPU poz;
           
            string pozewTxt;

          
            retcode = readDokWys(idDookWys);  // id dokumentu wychodzącego 
            if (retcode < 0) return retcode;
          
            try
            {
                switch (dw.TypDok)
                {
                    case 10:
                        poz = (PozewEPU)ToXMLSerializers.DeserializeFromString(dw.Tresc, typeof(PozewEPU));
                        poz.Oswiadczenie = "nie";
                        foreach (typStrona ts in poz.ListaPowodow)
                        {
                            if (ts.RodzajStrony > 1)  //osoba prawna
                            {
                                ts.ObcokrajowiecString = "";
                            }
                            else  // osoba fizyczna
                            {
                                ts.ObcokrajowiecString = "Obywatelstwo polskie";
                                                          
                            }
                            ts.Obcokrajowiec = false;
                            ts.BrakNumerowIdentyfikacyjnych = false;
                                               
                        }
                        foreach (typStrona ts in poz.ListaPozwanych)
                        {
                            if (ts.RodzajStrony > 1)  //osoba prawna
                            {
                                ts.ObcokrajowiecString = "";
                            }
                            else  // osoba fizyczna
                            {
                                ts.ObcokrajowiecString = "Obywatelstwo polskie";

                            }
                            ts.Obcokrajowiec = false;
                            ts.BrakNumerowIdentyfikacyjnych = false;

                        }
                        // zamiana  
                        
                        if (readSprawa((int)dw.Sprawa_id) < 0)
                            return -1; // błąd odczytu sprawy

                        DateTime? d_ods;
                        foreach (typRoszczenie tr in poz.ListaRoszczen)
                        {
                            d_ods = null;
                            if (tr.dataWymagalnosci != null)
                                if (Convert.ToDateTime(tr.dataWymagalnosci) > Convert.ToDateTime("2001-01-01"))
                                    continue;
                            if (tr.Odsetki != null)
                                if (tr.Odsetki.Count > 0)
                                {
                                    d_ods = tr.Odsetki.LastOrDefault().DataOd;
                                    if (d_ods != null)
                                    {
                                        if (d_ods > Convert.ToDateTime("2001-01-01"))
                                        {
                                            d_ods = Convert.ToDateTime(d_ods).AddDays(-1);
                                            tr.dataWymagalnosci = Convert.ToDateTime(d_ods).ToString("yyyy-MM-dd");
                                        }
                                        else
                                            tr.dataWymagalnosci = null;
                                    }
                                }
                        }
                        foreach (typRoszczenie tr in poz.ListaRoszczen)
                        {
                            if (tr.dataWymagalnosci == null)
                            {
                                foreach (Naleznosc nal in spr.Naleznosc)
                                {
                                    if (tr.Wartosc == nal.kwota)
                                    {
                                        tr.dataWymagalnosci = Convert.ToDateTime(nal.data_n).ToString("yyyy-MM-dd");
                                        break;
                                    }
                                }
                            }
                        }


                        pozewTxt = ToXMLSerializers.SerializeToString(poz, typeof(PozewEPU), true);
                        dw.Tresc = pozewTxt;
                        if (pozew != null)
                        {
                            pozew.Tresc = pozewTxt;

                        }
                        else return -3;
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 13:
                    case 14:
                        
                        break;
                    default:
                        break;
                }
                if (lexena != null) lexena.SaveChanges();
            }
            catch (Exception ex)
            {
                return -4; // bład zapisu 
            }
            return 1; 
        
        
        
        
        }

        public int ChangeDocDate(DateTime newDate,int typFirma )
        {
            int retcode;
            PozewEPU poz;
            DokumentEPU dok;
            string pozewTxt;
            
            if (newDate == null) return -2;
            retcode = readDokWys(idDookWys);
            if (retcode < 0) return retcode;
            this.dw.DataDok = newDate;
            readSprawa(this.dw.Sprawa_id.Value);
            try
            {
                switch (dw.TypDok)
                {
                    case 10:
                        poz = (PozewEPU)ToXMLSerializers.DeserializeFromString(dw.Tresc, typeof(PozewEPU));
                        dw.DataDok = newDate;
                        if (typFirma == -1 && dw.InneKoszty > 0 && this.spr != null && this.spr.IdWiena.HasValue)
                        { // przeliczenie odsetek
                               ObliczZaleglosc oZal = new ObliczZaleglosc();
                                decimal odsetki = 0;

                                oZal.ObliczSpraweWiena(this.spr.IdWiena.Value, newDate.AddDays(-1));
                                if (oZal.wynikiAll != null && oZal.wynikiAll.Count > 0)
                                {
                                    foreach (var nale in oZal.wynikiNal)
                                    {

                                        odsetki += nale.ods;

                                    }


                                }
                            if (odsetki > 0)
                            {
                                typRoszczenie odNal = poz.ListaRoszczen.Where(x => x.Opis.Contains("kapitaliz") && x.Opis.Contains("odset")).FirstOrDefault();
                                if (odNal != null)
                                {
                                    odNal.Wartosc = odsetki;
                                    
                                }
                            }



                            }
                            poz.DataZlozenia = newDate;
                        pozewTxt = ToXMLSerializers.SerializeToString(poz, typeof(PozewEPU),true);
                        dw.Tresc = pozewTxt;
                        if (pozew != null)
                        {
                            pozew.Tresc = pozewTxt;

                        }
                        else return -3;
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 13:
                    case 14:
                        dok = (DokumentEPU)ToXMLSerializers.DeserializeFromString(dw.Tresc, typeof(DokumentEPU));
                        dok.DataZlozenia = newDate;
                        pozewTxt = ToXMLSerializers.SerializeToString(dok, typeof(DokumentEPU),true);
                        dw.Tresc = pozewTxt;
                                           
                        break;
                    default:
                        break;
                }
                if (lexena != null) lexena.SaveChanges();
            }
            catch (Exception ex)
            {
                return -4; // bład zapisu 
            }
             return 1; 
        
        }

        public int ChangeRadcaDok(int KontoEPU_Id)
        {
            int retcode;
            PozewEPU poz;
            DokumentEPU dok;
            KontoEPU kontoEPU;
            string pozewTxt;

            if (KontoEPU_Id == null) return -2;
            retcode = readDokWys(idDookWys);
            if (retcode < 0) return retcode;
            this.dw.KontoEPU_Id = KontoEPU_Id;
            try
            {
                switch (dw.TypDok)
                {
                    case 10:
                        poz = (PozewEPU)ToXMLSerializers.DeserializeFromString(dw.Tresc, typeof(PozewEPU));
                        //poz.OsobaSkladajaca = new typSkladajacy();
                        //poz.OsobaSkladajaca.Adres = new typAdres();
                        kontoEPU = (from z in lexena.KontoEPU
                              where z.Id.Equals(KontoEPU_Id)
                              select z).FirstOrDefault();
                        if (kontoEPU == null) return -2;
                        poz.OsobaSkladajaca.pelnomocnik = kontoEPU.CzyZawodowy;
                        poz.OsobaSkladajaca.podstawa = kontoEPU.Pelnomocnictwo;
                        poz.OsobaSkladajaca.Nazwa = kontoEPU.Nazwa;
                        //poz.OsobaSkladajaca.Osoba = new typOsoba();
                        poz.OsobaSkladajaca.Osoba.Imie = kontoEPU.Imie;
                        poz.OsobaSkladajaca.Osoba.Imie2 = kontoEPU.Imie2;
                        poz.OsobaSkladajaca.Osoba.Nazwisko = kontoEPU.Nazwisko;
                        poz.OsobaSkladajaca.Osoba.PESEL = kontoEPU.PESEL;
                        poz.OsobaSkladajaca.Osoba.stanowisko = kontoEPU.Stanowisko;
                        //poz.OsobaSkladajaca.Adres = new typAdres();
                        poz.OsobaSkladajaca.Adres.Kraj = "PL";
                        poz.OsobaSkladajaca.Adres.Kod = kontoEPU.kod_pocztowy;
                        poz.OsobaSkladajaca.Adres.Miejscowosc = kontoEPU.miejscowosc;
                        poz.OsobaSkladajaca.Adres.NrDomu = kontoEPU.numer_domu;
                        poz.OsobaSkladajaca.Adres.NrMieszkania = kontoEPU.numer_mieszkania;
                        poz.OsobaSkladajaca.Adres.Poczta = kontoEPU.poczta;
                        poz.OsobaSkladajaca.Adres.Ulica = kontoEPU.ulica;
                        poz.OsobaSkladajaca.Adres.Wojewodztwo = kontoEPU.wojewodztwo;
                        poz.ListaPodpisow.Clear();
                        poz.ListaPodpisow = new System.Collections.ObjectModel.ObservableCollection<typOsoba>();
                        typOsoba podpis = new typOsoba();
                        podpis.Imie = poz.OsobaSkladajaca.Osoba.Imie;
                        podpis.Imie2 = poz.OsobaSkladajaca.Osoba.Imie2;
                        podpis.Nazwisko = poz.OsobaSkladajaca.Osoba.Nazwisko;
                        podpis.PESEL = poz.OsobaSkladajaca.Osoba.PESEL;
                        podpis.stanowisko = poz.OsobaSkladajaca.Osoba.stanowisko;
                        poz.ListaPodpisow.Add(podpis);
                                        
                        
                        pozewTxt = ToXMLSerializers.SerializeToString(poz, typeof(PozewEPU), true);




                        dw.Tresc = pozewTxt;
                        if (pozew != null)
                        {
                            pozew.Tresc = pozewTxt;

                        }
                        else return -3;
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 13:
                    case 14:
                        dok = (DokumentEPU)ToXMLSerializers.DeserializeFromString(dw.Tresc, typeof(DokumentEPU));
                         kontoEPU = (from z in lexena.KontoEPU
                              where z.Id.Equals(KontoEPU_Id)
                              select z).FirstOrDefault();
                        if (kontoEPU == null) return -2;
                        dok.OsobaSkladajaca.pelnomocnik = kontoEPU.CzyZawodowy;
                        dok.OsobaSkladajaca.podstawa = kontoEPU.Pelnomocnictwo;
                        dok.OsobaSkladajaca.Nazwa = kontoEPU.Nazwa;
                        //dok.OsobaSkladajaca.Osoba = new typOsoba();
                        dok.OsobaSkladajaca.Osoba.Imie = kontoEPU.Imie;
                        dok.OsobaSkladajaca.Osoba.Imie2 = kontoEPU.Imie2;
                        dok.OsobaSkladajaca.Osoba.Nazwisko = kontoEPU.Nazwisko;
                        dok.OsobaSkladajaca.Osoba.PESEL = kontoEPU.PESEL;
                        dok.OsobaSkladajaca.Osoba.stanowisko = kontoEPU.Stanowisko;
                        //dok.OsobaSkladajaca.Adres = new typAdres();
                        dok.OsobaSkladajaca.Adres.Kraj = "PL";
                        dok.OsobaSkladajaca.Adres.Kod = kontoEPU.kod_pocztowy;
                        dok.OsobaSkladajaca.Adres.Miejscowosc = kontoEPU.miejscowosc;
                        dok.OsobaSkladajaca.Adres.NrDomu = kontoEPU.numer_domu;
                        dok.OsobaSkladajaca.Adres.NrMieszkania = kontoEPU.numer_mieszkania;
                        dok.OsobaSkladajaca.Adres.Poczta = kontoEPU.poczta;
                        dok.OsobaSkladajaca.Adres.Ulica = kontoEPU.ulica;
                        dok.OsobaSkladajaca.Adres.Wojewodztwo = kontoEPU.wojewodztwo;




                        pozewTxt = ToXMLSerializers.SerializeToString(dok, typeof(DokumentEPU), true);
                        dw.Tresc = pozewTxt;

                        break;
                    default:
                        break;
                }
                if (lexena != null) lexena.SaveChanges();
            }
            catch (Exception ex)
            {
                return -4; // bład zapisu 
            }
            return 1;

        }

    }


    public static class XML2HTMLTransform
    {
        
     
       
        /*
        public static int createPDF2(string html, string filename)
        {

            Document document = new Document(PageSize.A4, 20, 20, 20, 20);
            
            PdfWriter.GetInstance(document, new  FileStream(filename, FileMode.Create));
            document.Open();
            BaseFont bf = BaseFont.CreateFont("c:/windows/fonts/arial.ttf", BaseFont.CP1250, BaseFont.EMBEDDED);
            Font font = new Font(bf, 12); 
            List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(html), null);
            for (int k = 0; k < htmlarraylist.Count; k++)
            {
                for (int l = 0; l< htmlarraylist[k].Chunks.Count; l++)
                        htmlarraylist[k].Chunks[k].Font = font;
            }
            for (int k = 0; k < htmlarraylist.Count; k++)
            {
                document.Add((IElement)htmlarraylist[k]);
            }
            document.Close();
            return 1;
        } */
        public static int  WriteMemoryStream(MemoryStream ms, string filename)
        {
            try
            {

                FileStream file = new FileStream(filename, FileMode.Create, System.IO.FileAccess.Write);
               
                byte[] bytes = new byte[ms.Length];
                ms.Read(bytes, 0, (int)ms.Length);
                file.Write(bytes, 0, bytes.Length);
                file.Close();
              }
            catch (Exception ex)
            {
                return -1;
            
            
            }
            return 0;    
        }

        public static string WriteXMLFile(string xmldoc)
        {
            string OutputFilename;
            string htmlfile;
            
            StreamWriter stwr;

            try
            {
                OutputFilename = string.Format(@"{0}.xml", Guid.NewGuid());
                htmlfile = System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/" + OutputFilename);
                stwr = new StreamWriter(htmlfile, false, Encoding.UTF8);
                stwr.Write(xmldoc);
                stwr.Close();
                // pdfFname = string.Format(@"{0}.pdf", Guid.NewGuid());
                // pdfFname = System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/" + pdfFname);
                // createPDF(htmlfile, pdfFname);
                return "/HtmlFiles/" + OutputFilename;
            }
            catch (Exception ex)
            {
                return "Błąd: " + ex.Message;
            }


        }
        public static string WriteHtmlFile(string htmldoc)
        {
            string OutputFilename;
            string htmlfile;
            StreamWriter stwr;
       
            try
            {
                OutputFilename = string.Format(@"{0}.html", Guid.NewGuid());
                htmlfile = System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/" + OutputFilename);
                stwr = new StreamWriter(htmlfile,false,Encoding.UTF8);
                stwr.Write(htmldoc);
                stwr.Close();
                // pdfFname = string.Format(@"{0}.pdf", Guid.NewGuid());
                // pdfFname = System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/" + pdfFname);
                // createPDF(htmlfile, pdfFname);
                return htmlfile;

                //return  "/HtmlFiles/" + OutputFilename;
            }
            catch (Exception ex)
            {
                return "Błąd: " + ex.Message;
            }

                  
        }

       
        public static String MemoryStreamToString(System.IO.MemoryStream ms)
        {
            byte[] ByteArray = ms.ToArray();
            return System.Text.Encoding.UTF8.GetString(ByteArray);
        }

        private static string parseCharbyChar(string instring)
        {
            string outstring = string.Empty;
            bool found = false;
            if (String.IsNullOrEmpty(instring))
                return instring;

            for (int i = 0; i < instring.Length; i++)
            {
                if (instring[i] == '&')
                {
                    found = false;
                    // możliwy 
                    for (int j = i + 1; j < instring.Length; j++)
                    {
                        if (j - i > 7)
                            break;
                        if (instring[j] == ';' && j - i > 1)
                        {
                            found = true;
                            break;
                        }
                        if (!Char.IsLetter(instring[j]))
                            break;



                    }
                    if (!found)
                    {
                        outstring += "&amp;";
                    }
                    else
                        outstring += instring[i];



                }
                else
                    outstring += instring[i];

            }

            return outstring;

        }

        public static string cleanXMLPozew(string docString)
        { // czyszczenie stringa - xml z "lewych" znaków

            try
            {
                XNamespace curr = "http://www.currenda.pl/epu";
                XDocument doc = XDocument.Parse(docString);

                foreach (var el in doc.Descendants(curr + "Nazwa"))
                {
                    // sprawdzamy, czy ma przodka <curr:Dane>
                    if (el.Ancestors(curr + "ListaPowodow").Any() || el.Ancestors(curr + "ListaPozwanych").Any())
                    {
                        el.Value = el.Value
                                      .Replace("\r\n", " ")
                                      .Replace("\n", " ")
                                      .Replace("\r", " ").Trim();
                    }
                }
                docString = doc.ToString(SaveOptions.DisableFormatting);
            }
            catch { }
            //docString = System.Web.HttpUtility.HtmlDecode(docString);
#if DEBUG
            File.WriteAllText(@"C:\epu_logs\Epu\Xml_przed" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html", docString, Encoding.UTF8);
#endif            
            docString = docString.Replace("&#xC;", ""); //jakaś menta używa znaków specjalnych w treści pisma
            char shit = (char)31;
            docString = docString.Replace(shit, ' ');//to już przegięcie separator dokumentu
            docString = docString.Replace("&#x1A;", "");//to już przegięcie separator dokumentu
            docString = docString.Replace("&#x8;", ""); //ktoś użył w dokumencie znaku &#x8;
            docString = docString.Replace("&#x1F;", ""); //ktoś użył w dokumencie znaku &#x1F;
            docString = docString.Replace("&#xB;", ""); //ktoś użył w dokumencie znaku &#xB; 
            docString = docString.Replace("&#x0;", "");
            docString = docString.Replace("&#x15;", "");
            //docString = docString.Replace("&", "&amp;").Replace("<\n", "&lt;\n").Replace("<\r", "&lt;\r").Replace("\n>", "\n&gt;").Replace("\r>", "\r&gt;");
            docString = docString.Replace("<\n", "&lt;\n").Replace("<\r", "&lt;\r").Replace("\n>", "\n&gt;").Replace("\r>", "\r&gt;");
            docString = parseCharbyChar(docString);

            // aspose niepoprawnie interpretuje <br /> dla justyfikacji dlatego używamy <p> dla miejsc justowanych np przy uzasadnieniu, lub tresci pisma

            int uzasadnienieStart = docString.IndexOf("<curr:Uzasadnienie>", StringComparison.CurrentCultureIgnoreCase);
            int uzasadnienieEnd = docString.IndexOf("</curr:Uzasadnienie>", StringComparison.CurrentCultureIgnoreCase);
            if (uzasadnienieStart <= 0)
            {
                uzasadnienieStart = docString.IndexOf("<curr:Tresc>", StringComparison.CurrentCultureIgnoreCase);
                uzasadnienieEnd = docString.IndexOf("</curr:Tresc>", StringComparison.CurrentCultureIgnoreCase);
            }
            int nextLineIndex = 0;

            // ">\r\n<" takie wpisy trzeba usunac przed zamiana "znakow konca linji"
            docString = docString.Trim();
            while (docString.IndexOf(">\r\n<", nextLineIndex) > 0)
            {
                nextLineIndex = docString.IndexOf(">\r\n<", nextLineIndex);
                docString = docString.Remove(nextLineIndex + 1, 2);
            }
            nextLineIndex = 0;
            while (docString.IndexOf("\r\n", nextLineIndex) > 0)
            {
                nextLineIndex = docString.IndexOf("\r\n", nextLineIndex);

                docString = docString.Remove(nextLineIndex, 2);
                if (nextLineIndex < uzasadnienieStart)
                {
                    docString = docString.Insert(nextLineIndex, "znakNowejLinjiP");
                    uzasadnienieStart += 14;
                    uzasadnienieEnd += 14;
                }
                else if (nextLineIndex > uzasadnienieStart && nextLineIndex < uzasadnienieEnd)
                {
                    docString = docString.Insert(nextLineIndex, "znakNowejLinjiP");
                    uzasadnienieEnd += 14;
                }
                else
                    docString = docString.Insert(nextLineIndex, "znakNowejLinjiP");
            }
            nextLineIndex = 0;
            /*
            while (docString.IndexOf("\n\n", nextLineIndex) > 0)
            {
                nextLineIndex = docString.IndexOf("\n\n", nextLineIndex);
                docString = docString.Remove(nextLineIndex, 2);
                if (nextLineIndex < uzasadnienieStart)
                {
                    docString = docString.Insert(nextLineIndex, "znakNowejLinjiP");
                    uzasadnienieStart += 14;
                    uzasadnienieEnd += 14;
                }
                else if (nextLineIndex > uzasadnienieStart && nextLineIndex < uzasadnienieEnd)
                {
                    docString = docString.Insert(nextLineIndex, "znakNowejLinjiP");
                    uzasadnienieEnd += 14;
                }
                else
                {
                    docString = docString.Insert(nextLineIndex, "znakNowejLinjiP");

                }
            }
            */
            nextLineIndex = 0;
            while (docString.IndexOf("\n", nextLineIndex) > 0)
            {
                nextLineIndex = docString.IndexOf("\n", nextLineIndex);
                //docString = docString.Remove(nextLineIndex, 1);
                /*
                if (nextLineIndex < uzasadnienieStart)
                {
                    docString = docString.Insert(nextLineIndex, "znakNowejLinjiBr");
                    uzasadnienieStart += 14;
                    uzasadnienieEnd += 14;
                }
                else 
                */
                if (nextLineIndex > uzasadnienieStart && nextLineIndex < uzasadnienieEnd)
                {
                    docString = docString.Remove(nextLineIndex, 1);
                    docString = docString.Insert(nextLineIndex, "znakNowejLinjiP");
                    uzasadnienieEnd += 14;
                }
                else
                {

                    nextLineIndex += 1;
                    if (nextLineIndex >= docString.Length)
                        break;
                }
                /*
                else
                    docString = docString.Insert(nextLineIndex, "znakNowejLinjiBr");
                */
            }

            //File.WriteAllText(@"E:\Epu\Tmp\Xml_po.html", docString, Encoding.UTF8);
            return docString;

        }

        private static int getEndOfPar(string s, int pos)
        {
            int endPar = 99999999, div = -1, endDiv = -1, znak = -1;

            znak = s.Substring(pos + 1).IndexOf("znakNowejLinjiP");
            div = s.Substring(pos + 1).IndexOf("<div");
            endDiv = s.Substring(pos + 1).IndexOf("</div>");
            if (endPar > div && div > 0)
                endPar = div;
            if (endPar > endDiv && endDiv > 0)
                endPar = endDiv;
            if (znak > 0 && endPar > znak)
                endPar = znak;
            return endPar == 99999999 ? -1 : endPar + pos + 1;
        }


        private static string replaceParagraphTags(string s)
        {
            bool found = false;
            int pos = 0, posEnd = -1;
            do
            {
                found = false;
                pos = s.IndexOf("znakNowejLinjiP");
                if (pos > 0)
                {
                    found = true;
                    int prev_parag = s.Substring(0, pos).LastIndexOf("/p>");
                    if ((prev_parag > 0 && prev_parag < s.Substring(0, pos).LastIndexOf("<p ")) || (prev_parag == -1 && s.Substring(0, pos).LastIndexOf("<p ") > 0))
                    {
                        s = s.Substring(0, pos) + "&nbsp;</p> <p style='text-align: justify;margin-top:2pt;margin-bottom:2pt;'>" + s.Substring(pos + "znakNowejLinjiP".Length);
                    }
                    else
                    {

                        s = s.Substring(0, pos) + "<p style='text-align: justify;margin-top:2pt;margin-bottom:2pt;'>" + s.Substring(pos + "znakNowejLinjiP".Length);
                        // szukamy najblizszego <div </div znakNowejLinjiP
                        posEnd = getEndOfPar(s, pos + "<p style='text-align: justify;margin-top:2pt;margin-bottom:2pt;'>".Length);
                        if (posEnd > 0)
                            s = s.Substring(0, posEnd) + "&nbsp;</p>" + s.Substring(posEnd);


                    }

                }


            }
            while (found);

            return s;

        }

        public static string TransformXML(string sXmlPath, int what)
        {
            string filePath;
            //byte[] compressed;//  = new byte[]();
            string outstring;
            bool swapCrLf = false;
             
            if (what < 0)
            {
                swapCrLf = true;
                what = -what;

            }

            MemoryStream stream;
            try
            {
                if (swapCrLf)
                {
                    sXmlPath = cleanXMLPozew(sXmlPath);

                }
               
                System.IO.StringReader reader = new StringReader(sXmlPath);
                XPathDocument myXPathDoc = new XPathDocument(reader);
                stream = new MemoryStream();
                XslCompiledTransform myXslTrans = new XslCompiledTransform();
                switch (what)
                {
                    case 10:
                    case 0:   // pozew 
                        filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/XSLT/Pozew.xslt");
                        Utils.LogWriter(filePath);
                        break;
                    //case 1:
                    case  1003:
                    case  1004:
                    case  1005:
                    case 1006:    
                    case 1013:
                    case 1014:
                        filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/XSLT/DokumentEPU.xslt");
                        break;
                    case 5:
                        filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/XSLT/NakazEPU.xslt");
                        break;
                    case 17:
                        filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/XSLT/OrzeczenieEPU.xslt");
                        break;
                    case 101:
                        filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/XSLT/OrzeczenieEPU.xslt");
                        
                        break;
                    case 30:
                        filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/XSLT/WniosekEgzekucyjny.xslt");

                        break;

                    default:
                        filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/XSLT/Pozew.xslt");
                        
                        break;
                }

                myXslTrans.Load(filePath);
                //Utils.LogWriter("Load XSLT");
                XmlTextWriter myWriter = new XmlTextWriter(stream, Encoding.UTF8);
                myXslTrans.Transform(myXPathDoc, null, myWriter);
                //Utils.LogWriter("Transform");
                outstring = MemoryStreamToString(stream);
                //Utils.LogWriter(outstring);

                if (swapCrLf)
                {
                    outstring = outstring.Replace("znakNowejLinjiBr", "<br />");
                    outstring = replaceParagraphTags(outstring);

                }

                return outstring;


            }
            catch (Exception e)
            {
                Utils.LogWriter("Wyjątek" + e.Message);
                return null;
            }

        }

        public static string TransformSeries(string IdListSerialized, int what)
        {
            string htmlstring;
            string fullstring;
            List<int> listId;
            string    xmlDoc;
            listId = new List<int>();
            listId = (List<int>)ToXMLSerializers.DeserializeFromString(IdListSerialized, typeof(List<int>));
            htmlstring = "";
            fullstring = "";
            int i, count;

            // odczyt w pętli  pozwów + transformata
            try
            {
            using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
            {
                 IQueryable<DokWys> LstDok = (from z in lexena.DokWys
                                      where listId.Contains(z.Id)
                                      select z);

                 i = 0;
                 count = LstDok.Count();
                foreach (DokWys currDoc in LstDok)
                 {
                    i ++; 
                    xmlDoc = currDoc.Tresc;
                     htmlstring = TransformXML(xmlDoc, what);
                     if (i < count)
                        htmlstring = htmlstring.Replace("</body></html>", "<h4 style=\"page-break-before: always;\">&nbsp</h4></body>");
                     fullstring += htmlstring; 
                 }
                
            }


            return WriteHtmlFile(fullstring);

            }
            catch (Exception e)
            {

                Console.WriteLine("Exception: {0}", e.ToString());
                return "";
            }

        }

        public static string TransformSeriesToXML(string IdListSerialized, int what)
        {
            
           
            List<int> listId;
            string xmlDoc;
            listId = new List<int>();
            listId = (List<int>)ToXMLSerializers.DeserializeFromString(IdListSerialized, typeof(List<int>));
            int i, count;
            StringBuilder paczka = new StringBuilder();

            // odczyt w pętli  pozwów + transformata
            try
            {
                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {
                    IQueryable<DokWys> LstDok = (from z in lexena.DokWys.Include("Sprawa")
                                                 where listId.Contains(z.Id)
                                                 select z );

                    i = 0;
                    count = LstDok.Count();
                    paczka.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    paczka.Append("<curr:Pozwy OznaczeniePaczki=\"");
                    paczka.Append("Pozwy " + DateTime.Today.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:MM"));
                    paczka.Append(" \" xmlns:curr=\"" + Constants.currnamespace+"\">");
                    foreach (DokWys currDoc in LstDok)
                    {
                        i++;
                        XmlDocument pozewXML = new XmlDocument();
                        pozewXML.Load(new StringReader(currDoc.Tresc));
                        var names = new XmlNamespaceManager( pozewXML.NameTable );
                        names.AddNamespace("curr", Constants.currnamespace);
                        foreach (XmlNode node in pozewXML)
                            if (node.NodeType == XmlNodeType.XmlDeclaration)
                            {
                                pozewXML.RemoveChild(node);
                            }

                        if (currDoc.Sprawa.NrEwid != null)
                        {
                            XmlNodeList xnList = pozewXML.SelectNodes("/curr:PozewEPU/curr:SprawaWgPowoda", names);
                            foreach (XmlNode nd2 in xnList)
                            {
                                nd2.InnerText = nd2.InnerText + "#" + currDoc.Sprawa.NrEwid.Trim();
                            }
                        }
                        string test = pozewXML.InnerXml.ToString();
                        paczka.AppendLine(test);

                    }
                    paczka.AppendLine("</curr:Pozwy>");

                }


                return WriteXMLFile(paczka.ToString());

            }
            catch (Exception e)
            {

                //Console.WriteLine("Exception: {0}", e.ToString());
                return "";
            }

        }


        public static string ConcatenateXML(string IdListSerialized)
        {


            List<int> listId;
            listId = new List<int>();
            listId = (List<int>)ToXMLSerializers.DeserializeFromString(IdListSerialized, typeof(List<int>));
            int i, count;
            StringBuilder paczka = new StringBuilder();

            // odczyt w pętli  pozwów + transformata
            try
            {
                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {
                    IQueryable<DokWys> LstDok = (from z in lexena.DokWys.Include("Sprawa")
                                                 where listId.Contains(z.Id)
                                                 select z);

                    i = 0;
                    count = LstDok.Count();
                    paczka.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    paczka.AppendLine("<Sprawy>");
                    foreach (DokWys currDoc in LstDok)
                    {
                        if (String.IsNullOrEmpty(currDoc.OdsNalicz)) continue;  
                        i++;
                        XmlDocument docXML = new XmlDocument();
                        docXML.Load(new StringReader(currDoc.OdsNalicz));
                        var names = new XmlNamespaceManager(docXML.NameTable);
                        foreach (XmlNode node in docXML)
                            if (node.NodeType == XmlNodeType.XmlDeclaration)
                            {
                                docXML.RemoveChild(node);
                            }

                        
                        string test = docXML.InnerXml.ToString();
                        paczka.AppendLine(test);

                    }
                    paczka.AppendLine("</Sprawy>");

                }


                return WriteXMLFile(paczka.ToString());

            }
            catch (Exception e)
            {

                //Console.WriteLine("Exception: {0}", e.ToString());
                return "";
            }

        }
        
        
        public static int markAsRead(int IdDok, int userRole, int userId)
        {

            int offset = 0 ;
        try
            {
            
            // odczyt w pętli  pozwów + transformata
            
                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {
                    var DokOdebrToCheck = (from z in lexena.DokOdebr
                                                 where  z.Id.Equals(IdDok) 
                                                 select z).FirstOrDefault();

                   
                    if (DokOdebrToCheck != null)
                    {
                        if (userRole > 0) // jeśli administrator
                        {
                            // sprawdzamy czy dokumnet nie jest w referacie 
                            vw_ListaDoOdebr dok = (from z in lexena.vw_ListaDoOdebr
                                                   where z.Id.Equals(IdDok)
                                                   select z).FirstOrDefault();
                            if (dok != null)
                            if (dok.Id_User == userId) offset = 1; 
                        }

                        switch (userRole)
	                    {
                            case 1:
                                DokOdebrToCheck.IsChecked += 1000 + offset;
                                break;
                            case 2:
		                        DokOdebrToCheck.IsChecked += 1000000 + offset;
                                break;
                            default:
                                DokOdebrToCheck.IsChecked += 1;
                                break;
	                    }

                    }

                    lexena.SaveChanges();
                    return 1; // OK
                }


        
        
        }
            catch(Exception ex)
            {
                return  -1;
            }
        
        
        
        }


        public static int markAsRead(string IdDokList, int userRole, int  userId)
        {
            List<int> listId;
            listId = new List<int>();
            int offset = 0;
            try
            {
            listId = (List<int>)ToXMLSerializers.DeserializeFromString(IdDokList, typeof(List<int>));
          
            // odczyt w pętli  pozwów + transformata
            
                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {
                    var LstDokIsChecked = (from z in lexena.DokOdebr
                                                 where listId.Contains(z.Id)
                                                 select z);

                     foreach (var currDoc in LstDokIsChecked)
                    {

                        offset = 0;
                        if (userRole > 0) // jeśli administrator
                        {
                            // sprawdzamy czy dokumnet nie jest w referacie 
                            vw_ListaDoOdebr dok = (from x in lexena.vw_ListaDoOdebr
                                                   where x.Id.Equals(currDoc.Id)
                                                   select x).FirstOrDefault();
                            if (dok != null)
                                if (dok.Id_User == userId) offset = 1;
                        }

                        switch (userRole)
	                    {
                            case 1:
                                currDoc.IsChecked += 1000 + offset;
                                break;
                            case 2:
		                        currDoc.IsChecked += 1000000 + offset;
                                break;
                            default:
                                currDoc.IsChecked += 1;
                                break;
	                    }

                    }

                    lexena.SaveChanges();
                    return 1; // OK
                }


        
        
        }
            catch(Exception ex)
            {
                return  -1;
            }
        }



        public static string ConcatDokOdebr(string IdDokList)
        {
            string htmlstring;
            string fullstring;
            List<int> listId;
            listId = new List<int>();
            listId = (List<int>)ToXMLSerializers.DeserializeFromString(IdDokList, typeof(List<int>));
            htmlstring = "";
            fullstring = "";
            int i, count;

            // odczyt w pętli  pozwów + transformata
            try
            {
                using (LexEnaMeritumEntities lexena = new LexEnaMeritumEntities())
                {
                    IQueryable<DokOdebr> LstDok = (from z in lexena.DokOdebr
                                                 where listId.Contains(z.Id)
                                                 select z);

                    i = 0;
                    count = LstDok.Count();
                    foreach (DokOdebr currDoc in LstDok)
                    {
                        i++;
                        htmlstring = currDoc.TrescHtml;
                        if (i < count)
                            htmlstring = htmlstring.Replace("</body></html>", "<h4 style=\"page-break-before: always;\">&nbsp</h4></body>");
                        fullstring += htmlstring;
                    }

                }


                return fullstring; //WriteHtmlFile(fullstring);

            }
            catch (Exception e)
            {

                //Console.WriteLine("Exception: {0}", e.ToString());
                throw(e);
                //return "";
            }

        }


        public static string Transform(string sXmlPath, int what)
        {
            string htmlstring;
           
            try
            {
                htmlstring = TransformXML(sXmlPath, what);
                return htmlstring; // WriteHtmlFile(htmlstring);

            }
            catch (Exception e)
            {

                Console.WriteLine("Exception: {0}", e.ToString());
                return "";
            }

        }
      


        public static string html2pdf(string htmlcontent, ref byte[] pdfcontent) // odczyt html'a z dokumentu odebranego z EPU       //PozewEPU _pozew) // serializacja pozwu EPU
        {

          
            try
            {

                //htmlfname = WriteHtmlFile(htmlcontent);
               
                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                pdfcontent = htmlToPdf.GeneratePdf(htmlcontent);

               
            }
            catch (Exception ex)
            {
                Utils.LogWriter(" błąd podczas tworzenia pdf " + ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                return "Błąd " + ex.Message ;
            }
            return "OK";

        }

        /*
          
         */

        public static string TransformNCompress(string sXmlPath, int what)
        {
            string filePath;
            //byte[] compressed;//  = new byte[]();
            string outstring;
            MemoryStream stream;
            try
            {
                

                sXmlPath = (sXmlPath.Replace("&amp;", "@$%$!^")).Replace("&", "&amp;").Replace("@$%$!^", "&amp;");
                System.IO.StringReader reader = new StringReader(sXmlPath);
                XPathDocument myXPathDoc = new XPathDocument(reader);
                stream = new MemoryStream();
                XslCompiledTransform myXslTrans = new XslCompiledTransform();
                switch (what)
                {
                    case 0:   // pozew 
                        
                        filePath = @"Pozew.xslt";
                        break;
                    case 5:
                        filePath = @"nakazEPU.xslt";
                        break;
                    case 17: // orzeczenia ( postanowienia )
                    case 101:
                        filePath = @"OrzeczenieEPU.xslt";
                        break;
                    case 30:
                        filePath = @"WniosekEgzekucyjny.xslt";
                        break;
                    default:
                        filePath = @"nakazEPU.xslt";
                        break;
                }

                filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/XSLT/"+filePath);

                myXslTrans.Load(filePath);

                XmlTextWriter myWriter = new XmlTextWriter(stream, Encoding.UTF8);
                myXslTrans.Transform(myXPathDoc, null, myWriter);
                // stream zawiera html'a
                //compressed = new  byte[stream.Length];
                //compressed = Zip(MemoryStreamToString(stream));

                /*
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (GZipStream tinyStream = new GZipStream(outStream, CompressionMode.Compress))
                    stream.CopyTo(tinyStream);
                    filePath = MemoryStreamToString(outStream);
                    
                    //outStream.Write(compressed, 0, (int)stream.Length);
                    //compressed = outStream.ToArray();

                } 
                */
                outstring = MemoryStreamToString(stream);
                return outstring;


            }
            catch (Exception e)
            {
                Utils.LogWriter(" błąd podczas tworzenia html " + e.Message + " " + (e.InnerException!= null ? e.InnerException.Message:"" ));
                //Console.WriteLine("Exception: {0}", e.ToString());
                return null;
            }

        }


    }

    // XML validation
    public class XMLValidator
    {
        // Validation Error Count
        static int ErrorsCount = 0;

        // Validation Error Message
        static string ErrorMessage = "";

        public static void ValidationHandler(object sender,
                                             ValidationEventArgs args)
        {
            ErrorMessage = ErrorMessage + args.Message + "\r\n";
            ErrorsCount++;
        }

        public void ValidateDokumentZEPU(string strXMLDoc, int dokType)
        {
            try
            {
                // Declare local objects
                XmlTextReader tr = null;
                XmlSchemaCollection xsc = null;
                XmlValidatingReader vr = null;


                ErrorMessage = "";
                ErrorsCount = 0;
                // Text reader object
                switch (dokType)
                {
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 13:
                    case 14:
                        tr = new XmlTextReader(System.Web.Hosting.HostingEnvironment.MapPath("~/XSD/DokumentEPU.xsd"));
                        break;
                    case 10:
                        tr = new XmlTextReader(System.Web.Hosting.HostingEnvironment.MapPath("~/XSD/PozewEPU.xsd"));
                        break;  
                    case 30:
                        tr = new XmlTextReader(System.Web.Hosting.HostingEnvironment.MapPath("~/XSD/WniosekEgzekucyjny.xsd"));
                        break;
                    default:
                        tr = new XmlTextReader(System.Web.Hosting.HostingEnvironment.MapPath("~/XSD/PozewEPU.xsd"));
                        break;
                }
                
                xsc = new XmlSchemaCollection();
                xsc.Add(null, tr);

                // XML validator object

                vr = new XmlValidatingReader(strXMLDoc,
                             XmlNodeType.Document, null);

                vr.Schemas.Add(xsc);

                // Add validation event handler

                vr.ValidationType = ValidationType.Schema;
                vr.ValidationEventHandler +=
                         new ValidationEventHandler(ValidationHandler);

                // Validate XML data

                while (vr.Read()) ;

                vr.Close();

                // Raise exception, if XML validation fails
                if (ErrorsCount > 0)
                {
                    throw new Exception(ErrorMessage);
                }

                // XML Validation succeeded
               
            }
            catch (Exception error)
            {
                // XML Validation failed
                /* Console.WriteLine("XML validation failed." + "\r\n" +
                "Error Message: " + error.Message); */
                throw new System.ArgumentException("Błąd walidacji xml", error.Message);

            }
        }
    }
        
        public class ToXMLSerializers
      {



        public static string SerializeToString(object objToSerialize, Type type, bool czyCurr)
        {
            var output = new MemoryStream();
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
            string outputString;
            using (var xmlWriter = XmlWriter.Create(output, settings))
            {
                var serializer = new XmlSerializer(type);
                
                
                xmlWriter.WriteStartDocument();
                if (czyCurr)
                {
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("curr", Constants.currnamespace);
                    serializer.Serialize(xmlWriter, objToSerialize, namespaces);
                }
                else
                    serializer.Serialize(xmlWriter, objToSerialize);

            }
            output.Seek(0L, SeekOrigin.Begin);
            // zamina stream na string
            var reader = new StreamReader(output);
            outputString = reader.ReadToEnd();
            return outputString;
        
        }


       
        public static object  DeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;
            using (TextReader reader = new StringReader(objectData))
            { result = serializer.Deserialize(reader); }

            return result;
        }

        

       
    }
    public static class EPUTypesDictionary
    {
    
       public static string DocNameDictionary(int doktype)
          {

              switch (doktype)
              {
                  case 3:
                                    
                       return "Pismo";
                  case 4:  
                        return "Wniosek";
                  case 5:  
                        return "Uzupełnienie adresu";
                  case 6:
                        return "Uzupełnienie braków";
                  case 13:  
                        return "Rezygnacja z pełnomocnictwa";
                  case 14:  
                        return "Zgłoszenie pełnomocnika do sprawy";
                  
                  case 30:
                        return "Wniosek Egzekucyjny";
                case 999:
                    return "Dokumentacja inicjująca postępowanie";  
  
                  
                  default:
                        return "";
              }
        
        }
    
    }


    public enum ErrLevel
        {
        OK,
        Info,
        Warning,
        Error,
        Fatal
    };

    public class errDescription
    {
        public bool isSummary { get; set; }
        public int code { get; set; }
        public ErrLevel level { get; set; }
        public string description { get; set; }
        public string reference { get; set;}

    }

  


}