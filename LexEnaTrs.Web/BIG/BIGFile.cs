using GenericParsing;
using Nicci.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using WienaDB.Models;


namespace LexEnaTrs.Web.BIG
{





    public class ImportRow
    {
        public string SystemName { get; set; }
        public string NrKlienta { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Pesel { get; set; }
        public string Nip { get; set; }
        public string Kod { get; set; }
        public string Miejsce { get; set; }
        public string Ulica { get; set; }
        public string Lokal { get; set; }
        public string Dom { get; set; }
        public string DataWysWezw { get; set; }
        public string DataWymag { get; set; }
        public string DataFaktury { get; set; }
        public string DataSprzedazy { get; set; }
        public string Saldo { get; set; }
        public string Title { get; set; }
        public string Adr1 { get; set; }
        public string Adr2 { get; set; }
        public int TypNal { get; set; }
        public int IdNal { get; set; }
        public int IdSprawaWiena { get; set; }
        public string WienaSygn { get; set; }
        public string SygnOrzecz { get; set; }
        public DateTime? dorzecz { get; set; }
        public int? czywyrok { get; set; }
        public string JobStatus { get; set;}

        public int? BIG_JobRowId { get; set; }


    }


    public class IdCaseOblig
        {

        public int   CaseId;
        public string ObligationId;
        public int DebtorId;
        public int BIGObl_Id;
        }

    public class BIGFile
    {
        private string importFileName;
        private string inDOC;
        private string userName;
        private string tempCsvFile;
        int errCode;
        List<Web.errDescription> errorsCollection;
        List<ImportRow> badRows;
        private int callContext = 0;
        private int firma = 1;

        string errDescription;
        private int rowsLimit = 5000; // liczba należności jednorazowo przekazywanych do krd 

        public BIGFile(string importfilename, string fileContent, int userId, int firma = 1 , int callContext = 0 )
        {
            this.importFileName = importfilename;
            this.inDOC = fileContent;
            this.callContext = callContext;
            this.firma = firma;
            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {
                Uzytkownik usr = context.Uzytkownik.Where(a => a.Id == userId).FirstOrDefault();
                if (usr != null)
                    this.userName = usr.Imie + " " + usr.Nazwisko;


            }

            errCode = 0;
            errDescription = "";
            errorsCollection = new List<Web.errDescription>();

        }

        public BIGFile(string importfilename, string userName)
        {
            this.importFileName = importfilename;
            this.userName  = userName;
           
            errCode = 0;
            errDescription = "";
            errorsCollection = new List<Web.errDescription>();

        }

        public BIGFile(string importfilename, Guid g, int userId, int firma)
        {
            this.importFileName = importfilename;
            this.tempCsvFile = "";
            this.firma = firma;
            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {
                Uzytkownik usr = context.Uzytkownik.Where(a => a.Id == userId).FirstOrDefault();
                if (usr != null)
                    this.userName = usr.Imie + " " + usr.Nazwisko;

                if (g != Guid.Empty)
                {
                    List<DataBuffer> lst = new List<DataBuffer>();
                    lst = context.DataBuffer.Where(a => a.ident == g).OrderBy(a => a.number).ToList();


                    if (lst != null)
                    {
                        this.tempCsvFile = System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlFiles/" + importfilename);
                        if (File.Exists(this.tempCsvFile))
                            File.Delete(this.tempCsvFile);

                        using (Stream w = new FileStream(this.tempCsvFile, FileMode.Append | FileMode.Create))
                        {
                            int bufLen = 0;

                            foreach (DataBuffer item in lst)
                            {
                                w.Write(item.binValue, 0, item.binValue.Length);
                                bufLen += item.binValue.Length;
                            }

                            w.Close();
                        }

                    

                    }
                }
            }

            errCode = 0;
            errDescription = "";
            errorsCollection = new List<Web.errDescription>();

        }

        public string GetError()
        {
            if (String.IsNullOrWhiteSpace(errDescription))
                return "";
            else
                return "( " + errCode.ToString() + " )" + errDescription;

        }
        public bool ImportfromWiena()
        {
            DateTime d_start = DateTime.Today.AddDays(-31);
            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {
                List<BIGvw_ImportWiena2BIG> sprlst2 = new List<BIGvw_ImportWiena2BIG>();
                List<BIGvw_ImportWiena2BIG> sprlst = context.BIGvw_ImportWiena2BIG.Where(a => a.data_r > d_start).ToList();
                List<BIG_Obligation> oblLst = new List<BIG_Obligation>();
                //1. Sprawdzenie spraw, które  pojawily się w Wienie a uprzednio były w systemie billingowym   
                if (sprlst != null)
                {
                    foreach (BIGvw_ImportWiena2BIG w2b in sprlst)
                    {
                        BIG_Obligation bob = (from c in context.BIG_Case
                                              join
                           d in context.BIG_Debtor on c.BIG_CaseId equals d.BIG_CaseId
                                              join
                           o in context.BIG_Obligation on c.BIG_CaseId equals o.BIG_CaseId
                                              join
                            opr in context.BIG_Operacja on o.BIG_ObligationId equals opr.BIG_ObligationId
                                              where o.DataWymag == w2b.data_n && d.IDNumber == w2b.nip && d.NrKlienta == w2b.nr_ewid
                                                    && c.SrcSystem == 11 && o.Title == w2b.tytul && opr.DataProcedowaniaKrd > d_start && opr.StatusOperacji > 0 && (opr.TypOperacji == 1 || opr.TypOperacji == 2)
                                              orderby opr.DataProcedowaniaKrd descending
                                              select o).FirstOrDefault();
                        // czy sprawa była zarejestrowana z poziomu wieny - 
                        if (bob != null)
                            continue;
                        //czy nalezność byla procedowana z poziomu 
                        bob = (from c in context.BIG_Case
                               join
            d in context.BIG_Debtor on c.BIG_CaseId equals d.BIG_CaseId
                               join
            o in context.BIG_Obligation on c.BIG_CaseId equals o.BIG_CaseId
                               join
             opr in context.BIG_Operacja on o.BIG_ObligationId equals opr.BIG_ObligationId
                               where o.DataWymag == w2b.data_n && d.IDNumber == w2b.nip && d.NrKlienta == w2b.nr_ewid
                                     && c.SrcSystem < 10 && o.Title == w2b.tytul && opr.DataProcedowaniaKrd > d_start && opr.StatusOperacji > 0 && (opr.TypOperacji == 1 || opr.TypOperacji == 2)
                               orderby opr.DataProcedowaniaKrd descending
                               select o).FirstOrDefault();
                        // sprawa była procedowana w systemie z billingu
                        if (bob != null)
                            sprlst2.Add(w2b); // jest w momencie przejścia do Wieny
                                              // oblst.Add()
                        else
                            continue;

                    }


                }
                //odświeżenie spraw, które są z Wieny
                List<BIGvw_ObligationLastStatus> lToDel = new List<BIGvw_ObligationLastStatus>();
                List<BIGvw_ObligationLastStatus> lToUpdate = new List<BIGvw_ObligationLastStatus>();
                List<BIGvw_ObligationLastStatus> bol = context.BIGvw_ObligationLastStatus.Where(a => a.SrcSystem == 11).ToList();  //należnoiści z WIeny z ostanim statusem 
                if (bol != null)
                {
                    foreach (BIGvw_ObligationLastStatus bo in bol)
                    {
                        if (bo.IdwienaNal == null) continue;
                        int id_nal = bo.IdwienaNal.Value;
                        // odczyt danych z 
                        BIGvw_ImportWiena2BIG bg = context.BIGvw_ImportWiena2BIG.Where(a => a.idNal == id_nal).FirstOrDefault();
                        if (bg == null)
                        {
                            lToDel.Add(bo);
                            continue;
                        }
                        if (bg.kwota - bg.splata <= 0)
                        {
                            lToDel.Add(bo);
                            continue;
                        }
                        bo.Saldo = bg.kwota.Value - bg.splata;
                        lToUpdate.Add(bo);


                    }


                }
            }
            return true;
        }
        private List<ImportRow> File2Struct(int mode, int context = 2 )
        {
            ImportRow r = null;
            try
            {
                List<ImportRow> impLst = new List<ImportRow>();
                badRows = new List<ImportRow>();
                using (TextReader sr = new StringReader(this.inDOC))
                {

                    using (GenericParser parser = new GenericParser())
                    {
                        parser.SetDataSource(sr);
                        parser.ColumnDelimiter = ';';
                        parser.FirstRowHasHeader = true;
                        parser.CommentCharacter = '|';
                        //parser.SkipStartingDataRows = 10;
                        parser.MaxBufferSize = 999999999;
                        parser.MaxRows = 999999;
                        //parser.TextQualifier = '\"';

                        while (parser.Read())
                        {
                            r = new ImportRow();
                            if (mode == -1)
                            {// EOP Selen WO
                                if (context == 6) // SAP
                                {
                                    r.SystemName = "SAP";
                                    r.NrKlienta = parser["NR_KLIENTA"];
                                    r.Name = parser["NAZWA"];
                                    r.Firstname = parser["IMIE"];
                                    if ((r.Firstname ?? string.Empty).Trim().Length > 0)
                                        r.Name = parser["NAZWISKO"];
                                    r.Secondname = string.Empty; //parser["DRUGIE_IMIE"];
                                    r.Nip = parser["NIP"];
                                    r.Pesel = parser["PESEL"];
                                    r.Kod = parser["ADRES_KOD"];
                                    r.Miejsce = parser["ADRES_MIEJSCOWOSC"];
                                    r.Ulica = parser["ADRES_ULICA"];
                                    r.Dom = parser["ADRES_NRDOM"];
                                    r.Lokal = parser["ADRES_NRLOK"];
                                    r.DataWysWezw = parser["DATA_WEZWANIA"];
                                    r.DataWymag = parser["TERMIN_PLATNOSCI"];
                                    r.Saldo = parser["KWOTA_FAKTURY"];
                                    r.Title = parser["NUMER_FAKTURY"];
                                    try
                                    {
                                        r.DataFaktury = parser["DATA_FAKTURY"];
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                    try
                                    {
                                        r.DataSprzedazy = parser["DATA_SPRZEDAZY"];
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                    r.Nip = r.Nip.Replace("-", "");
                                    r.Nip = r.Nip.Replace(" ", "");
                                    r.Kod = r.Kod.Trim();
                                    r.Miejsce = r.Miejsce.Trim();
                                } else
                                if (context == 5) // AUMS
                                {
                                    r.SystemName = "AUMS";
                                    r.NrKlienta = parser[0];
                                    r.Name = parser["NAZWA"];
                                    r.Firstname = string.Empty; // parser["IMIE"];
                                    r.Secondname = string.Empty; //parser["DRUGIE_IMIE"];
                                    r.Nip = parser["NIP"];
                                    r.Pesel = string.Empty; //parser["PESEL"];
                                    r.Kod = parser["ADRES_KOD"];
                                    r.Miejsce = parser["ADRES_MIEJSCOWOSC"];
                                    r.Ulica = parser["ADRES_ULICA"];
                                    r.Dom = parser["ADRES_NRDOM"];
                                    r.Lokal = parser["ADRES_NRLOK"];
                                    r.DataWysWezw = parser["DATA_WEZWANIA"];
                                    r.DataWymag = parser["TERMIN_PLATNOSCI"];
                                    r.Saldo = parser["SALDO"];
                                    r.Title = parser["NUMER_FAKTURY"];
                                    try
                                    {
                                        r.DataFaktury = parser["DATA_FAKTURY"];
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                    try
                                    {
                                        r.DataSprzedazy = parser["DATA_SPRZEDAZY"]; 
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                    r.Nip = r.Nip.Replace("-", "");
                                    r.Nip = r.Nip.Replace(" ", "");
                                    r.Kod = r.Kod.Trim();
                                    r.Miejsce = r.Miejsce.Trim();
                                }
                                else
                                if (context == 4) // selen pzp
                                {
                                    r.SystemName = "SelenPzP";
                                    r.NrKlienta = parser["Identyfikator sprawy"];
                                    r.Name = parser["Nazwa firmy"];

                                    r.Firstname = parser["Nazwisko"];
                                    r.Secondname = String.Empty;

                                    r.Nip = parser["NIP"];
                                    r.Pesel = parser["PESEL"];
                                    r.Kod = parser["Adres siedziby2"];
                                    r.Miejsce = parser["Adres siedziby2"];
                                    r.Ulica = parser["Adres siedziby1"];
                                    r.Dom = string.Empty;
                                    r.Lokal = string.Empty;
                                    r.DataWysWezw = parser["Data wysłania wezwania"];
                                    r.DataWymag = parser["Termin wymagalności"];
                                    r.Saldo = parser["Kwota zadłużenia"];
                                    r.Title = parser["Identyfikator zobowiązania"];
                                    try
                                    {
                                        r.DataFaktury = parser["Data wystawienia"];
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                    try
                                    {
                                        r.DataSprzedazy = parser["Data sprzedaży"];
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                    r.Nip = r.Nip.Replace("-", "");
                                    r.Nip = r.Nip.Replace(" ", "");
                                    if (!String.IsNullOrWhiteSpace(r.Kod))
                                    {
                                        if (r.Kod.Length > 6)
                                        {
                                            r.Kod = r.Kod.Substring(0, 6).Trim();
                                            r.Miejsce = r.Miejsce.Substring(7).Trim();
                                        }
                                        else
                                        {
                                            r.Kod = string.Empty;
                                            r.Miejsce = string.Empty;

                                        }
                                    }
                                    else
                                    {
                                        r.Kod = string.Empty;
                                        r.Miejsce = string.Empty;
                                        r.Ulica = string.Empty;
                                        r.Dom = string.Empty;
                                        r.Lokal = string.Empty;
                                    }
                                    if (!String.IsNullOrWhiteSpace(r.Firstname))
                                    {
                                        int spIndex = r.Firstname.LastIndexOf(' ');
                                        if (spIndex > 0)
                                        {
                                            r.Name = r.Firstname.Substring(0, spIndex ).Trim();
                                            r.Firstname = r.Firstname.Substring(spIndex).Trim();
                                        }

                                    }
                                }
                                else
                                {
                                   

                                    r.SystemName = "Selen";
                                    r.NrKlienta = parser["Identyfikator sprawy"];
                                    r.Name = parser["Nazwa firmy"];
                                    r.Firstname = string.Empty; // parser["IMIE"];
                                    r.Secondname = string.Empty; //parser["DRUGIE_IMIE"];
                                    r.Nip = parser["NIP"];
                                    r.Pesel = string.Empty; //parser["PESEL"];
                                    r.Kod = parser["Adres siedziby 2"];
                                    r.Miejsce = parser["Adres siedziby 2"];
                                    r.Ulica = parser["Adres siedziby 1"];
                                    r.Dom = string.Empty;
                                    r.Lokal = string.Empty;
                                    r.DataWysWezw = parser["Data wysłania wezwania"];
                                    r.DataWymag = parser["Termin wymagalności"];
                                    r.Saldo = parser["Kwota zadłużenia"];
                                    r.Title = parser["Identyfikator zobowiązania"];
                                    try
                                    {
                                        r.DataFaktury = parser["Data sprzedaży/wystawienia faktury"];
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                    try
                                    {
                                        r.DataSprzedazy = r.DataFaktury;
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                    r.Nip = r.Nip.Replace("-", "");
                                    r.Nip = r.Nip.Replace(" ", "");
                                    r.Kod = r.Kod.Substring(0, 6).Trim();
                                    r.Miejsce = r.Miejsce.Substring(7).Trim();
                                }
                            }
                            else
                            {
                                if (context == 4)
                                {
                                       r.SystemName = "WPS";
                                        r.NrKlienta = parser["NR_KLIENTA"];
                                        r.Name = parser["NAZWA"];

                                        r.Firstname = parser["NAZWA"];
                                        r.Secondname = String.Empty;

                                        r.Nip = parser["NIP"];
                                        r.Pesel = parser["PESEL"];
                                        r.Kod = parser["ADRES_KOD"];
                                        r.Miejsce = parser["ADRES_MIEJSCOWOSC"];
                                        r.Ulica = parser["ADRES_ULICA"];
                                        r.Dom = parser["ADRES_NRDOM"];
                                        r.Lokal = parser["ADRES_NRLOK"];
                                        r.DataWysWezw = parser["DATA_WYSLANIA_WEZWANIA"];
                                        r.DataWymag = parser["TERMIN_PLATNOSCI"];
                                        r.Saldo = parser["SALDO_FAKTURY"];
                                        r.Title = parser["NUMER_DOKUMENTU"];
                                        try
                                        {
                                            r.DataFaktury = parser["DATA_DOKUMENTU"];
                                        }
                                        catch (Exception)
                                        {
                                            ;
                                        }
                                        try
                                        {
                                            r.DataSprzedazy = parser["DATA_SPRZEDAZY"];
                                        }
                                        catch (Exception)
                                        {
                                            ;
                                        }
                                        r.Nip = r.Nip.Replace("-", "");
                                        r.Nip = r.Nip.Replace(" ", "");
                                    if (String.IsNullOrWhiteSpace(r.Nip))
                                    {
                                        if (!String.IsNullOrWhiteSpace(r.Firstname))
                                        {
                                            int spIndex = r.Firstname.LastIndexOf(',');
                                            if (spIndex > 0)
                                            {
                                                r.Name = r.Firstname.Substring(0, spIndex).Trim();
                                                r.Firstname = r.Firstname.Substring(spIndex+1).Trim();
                                            }

                                        }
                                    }
                                    else
                                        r.Firstname = "";


                                }
                                else
                                {
                                    r.SystemName = parser[0];
                                    r.NrKlienta = parser["NR_KLIENTA"];
                                    r.Name = parser["NAZWA"];
                                    r.Firstname = parser["IMIE"];
                                    r.Secondname = parser["DRUGIE_IMIE"];
                                    r.Nip = parser["NIP"];
                                    r.Pesel = parser["PESEL"];
                                    r.Kod = parser["ADRES_KOD"];
                                    r.Miejsce = parser["ADRES_MIEJSCOWOSC"];
                                    r.Ulica = parser["ADRES_ULICA"];
                                    r.Dom = parser["ADRES_NRDOM"];
                                    r.Lokal = parser["ADRES_NRLOK"];
                                    r.DataWysWezw = parser["DATA_WEZWANIA"];
                                    r.DataWymag = parser["TERMIN"];
                                    r.Saldo = parser["SALDO"];
                                    r.Title = parser["numer dokumentu"];

                                    try
                                    {
                                        r.DataFaktury = parser["DATA_DOKUMENTU"];
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                    try
                                    {
                                        r.DataSprzedazy = parser["DATA_SPRZEDAZY"];
                                    }
                                    catch (Exception)
                                    {
                                        ;
                                    }
                                }
                                int i;
                                int.TryParse(parser["IdNal"], out i);
                                if (i > 0)
                                    r.IdNal = i;

                                i = 0;
                                int.TryParse(parser["IdSprWiena"], out i);
                                if (i > 0)
                                    r.IdSprawaWiena = i;
                            }


                            r.SystemName = (r.SystemName ?? "").Trim();
                            r.NrKlienta = (r.NrKlienta ?? "").Trim();
                            r.Name = (r.Name ?? "").Trim();
                            r.Firstname = (r.Firstname ?? "").Trim();
                            r.Secondname = (r.Secondname ?? "").Trim();
                            r.Nip = (r.Nip ?? "").Trim();
                            r.Pesel = (r.Pesel ?? "").Trim();
                            r.Kod = (r.Kod ?? "").Trim();
                            r.Miejsce = (r.Miejsce ?? "").Trim();
                            r.Ulica = (r.Ulica ?? "").Trim();
                            r.Dom = (r.Dom ?? "").Trim();
                            r.Lokal = (r.Lokal ?? "").Trim();
                            r.DataWysWezw = (r.DataWysWezw ?? "").Trim();
                            r.DataWymag = (r.DataWymag ?? "").Trim();
                            r.DataFaktury = (r.DataFaktury ?? "").Trim();
                            r.DataSprzedazy = (r.DataSprzedazy ?? "").Trim();
                            r.Saldo = (r.Saldo ?? "").Trim();
                            r.Title = (r.Title ?? "").Trim();
                          
                            impLst.Add(r);

                        }

                    }

                }
                List<ImportRow> outLst;
                outLst = filterList(impLst, ref badRows, context);

                return outLst;
            }
            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas parsowania zbioru wejściowego" + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return null;
            }
        }

        private List<ImportRow> BigJob2Import(List<BIG_JobRow> bLst)
        {
            List<ImportRow> lst = new List<ImportRow>();
            foreach (BIG_JobRow i in bLst)
            {
                ImportRow r = new ImportRow();
                r.SystemName = (i.SystemName ?? "").Trim();
                r.NrKlienta = (i.NrKlienta ?? "").Trim();
                r.Name = (i.Name ?? "").Trim().Replace("   "," ").Replace("  ", " ").Replace("\t"," ");
                r.Firstname = (i.Firstname ?? "").Trim();
                r.Secondname = (i.Secondname ?? "").Trim();
                r.Nip = (i.Nip ?? "").Trim();
                r.Pesel = (i.Pesel ?? "").Trim();
                r.Kod = (i.Kod ?? "").Trim();
                r.Miejsce = (i.Miejsce ?? "").Trim();
                r.Ulica = (i.Ulica ?? "").Trim();
                r.Dom = (i.Dom ?? "").Trim();
                r.Lokal = (i.Lokal ?? "").Trim();
                r.DataWysWezw = (i.DataWysWezw ?? "").Trim();
                r.DataWymag = (i.DataWymag ?? "").Trim();
                r.DataFaktury = (i.DataFaktury ?? "").Trim();
                r.DataSprzedazy = (i.DataSprzedazy ?? "").Trim();
                r.Saldo = (i.Saldo ?? "").Trim();
                r.Title = (i.Title ?? "").Trim();

                r.Nip = r.Nip.Replace("-", "");
                r.Nip = r.Nip.Replace(" ", "");
                if (String.IsNullOrWhiteSpace(r.Nip))
                {
                    if (String.IsNullOrWhiteSpace(r.Firstname))
                    { 
                        int count = r.Name.Count(f => f == ' ');
                        if (count == 1 || count == 2)
                        {
                            r.Firstname = r.Name.Substring(r.Name.IndexOf(' ')).Trim();
                            r.Name = r.Name.Substring(0,r.Name.IndexOf(' ')).Trim();
                            if (count == 2 && r.Firstname.IndexOf(' ') > 0)
                            {
                                r.Secondname = r.Firstname.Substring(r.Firstname.IndexOf(' ')).Trim();
                                r.Firstname = r.Firstname.Substring(0, r.Firstname.IndexOf(' ')).Trim();
                            }

                        }
                        
                    }
                }


                lst.Add(r);

            }

            return lst;

        }

        private void updateImportSatus(List<ImportRow> lstIn)
        {



        }


        private List<ImportRow> filterList(List<ImportRow> lstIn, ref List<ImportRow> badrows, int context)
        {
            decimal saldo = 0;
            string nrKlienta = "";
            DateTime DataWysWezw ;
            DateTime DataWymag;
            decimal kwota = 0;
            bool czyfiz = false;
            ImportRow xprev = null;
            List<ImportRow> outlst = new List<ImportRow>();

            lstIn = lstIn.OrderBy(a => a.NrKlienta).ThenBy(a => a.DataWymag).ToList();
            foreach (ImportRow x in lstIn)
            { 
                try
                {
                    x.IdSprawaWiena = 0;

                    try
                    {
                        kwota = Convert.ToDecimal(x.Saldo.Replace(".", ","), CultureInfo.GetCultureInfo("pl-PL"));
                    }
                    catch (Exception e)
                    {
                        x.JobStatus += " Błędne saldo  ";
                        // x.IdSprawaWiena = -1;
                        // goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;
                    }

                    if (kwota <= 0)
                    {
                        x.JobStatus += "Saldo < 0 ";
                        // x.IdSprawaWiena = -1;
                        //goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;
                    }


                    x.DataWysWezw = x.DataWysWezw.Replace(".", "-").Replace("/", "-").Trim().Truncate(10);
                    try
                    {
                        try
                        {
                            DataWysWezw = DateTime.ParseExact(x.DataWysWezw, "yyyy-MM-dd", CultureInfo.InstalledUICulture);
                        }
                        catch (Exception)
                        {

                            DataWysWezw = DateTime.ParseExact(x.DataWysWezw, "dd-MM-yyyy", CultureInfo.InstalledUICulture);

                        }
                        x.DataWysWezw = DataWysWezw.ToString("yyyy-MM-dd");

                    }
                    catch (Exception e)
                    {
                        x.JobStatus += " Błędna data wezw  ";
                        // x.IdSprawaWiena = -1;
                        //goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;
                    }
                    
                    if ((DateTime.Today - DataWysWezw).TotalDays <= ((context == 4) ? 31: 30))
                    {
                        x.JobStatus += " Błędna data wezw <30 dn ";
                        //x.IdSprawaWiena = -1;
                        // goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;

                    }
                    x.DataWymag = x.DataWymag.Replace(".", "-").Replace("/", "-").Trim().Truncate(10);
                    try
                    {
                        try
                        {
                            DataWymag = DateTime.ParseExact(x.DataWymag, "yyyy-MM-dd", CultureInfo.InstalledUICulture);
                        }
                        catch (Exception)
                        {
                            DataWymag = DateTime.ParseExact(x.DataWymag, "dd-MM-yyyy", CultureInfo.InstalledUICulture);

                        }

                        x.DataWymag = DataWymag.ToString("yyyy-MM-dd");
                    }
                    catch (Exception e)
                    {
                        x.JobStatus += " Błędna data wymagalności  ";
                        //x.IdSprawaWiena = -1;
                        //goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;
                    }
                    int daysno = 31;
                    if (this.firma == 1 && (this.callContext == 4 || this.callContext == 5 || context == 4))
                        daysno = 30;

                    if ((DateTime.Today - DataWymag).TotalDays <= daysno)
                    {
                        x.JobStatus += " Błędna data wymagalności < " + daysno.ToString() + " dni ";
                        //x.IdSprawaWiena = -1;
                        //goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;

                    }

                    if ((DateTime.Today - DataWymag).TotalDays > 6 * 365 - 2)
                    {
                        x.JobStatus += " Błędna data wymagalności dług przedawniony, starszy niż 6 lat";
                        //x.IdSprawaWiena = -1;
                        //goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;

                    }


                    if (DataWymag >= DataWysWezw)
                    {
                        x.JobStatus += " Błędna data wymagalności lub wezwania ";
                        //x.IdSprawaWiena = -1;
                        //goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;

                    }
                    if (String.IsNullOrWhiteSpace(x.Kod) || x.Kod.Trim().Length != 6)
                    {

                        x.JobStatus += " Błędny kod ";
                        //x.IdSprawaWiena = -1;
                        //goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;

                    }

                    if (!String.IsNullOrWhiteSpace(x.Pesel.Trim()))
                    {
                        if (!Utils.IsValidPesel(x.Pesel.Trim()))
                        {

                            x.JobStatus += " Błędny pesel ";
                            //x.IdSprawaWiena = -1;
                            //goto nextloop;
                            if (badrows != null)
                                badrows.Add(x);
                            continue;

                        }

                    }

                    if (!string.IsNullOrWhiteSpace(x.Nip.Trim()))
                    {
                        if (!Utils.IsValidNIP(x.Nip.Trim()))
                        {

                            x.JobStatus += " Błędny NIP ";
                            //x.IdSprawaWiena = -1;
                            //goto nextloop;
                            if (badrows != null)
                                badrows.Add(x);
                            continue;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(x.Nip) && string.IsNullOrWhiteSpace(x.Pesel))
                    {


                        x.JobStatus += " Brak identyfikatora nip i  pesel ";
                        //x.IdSprawaWiena = -1;
                        //goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;

                    }

                    if (string.IsNullOrWhiteSpace(x.Nip) && string.IsNullOrWhiteSpace(x.Firstname))
                    {
                        x.JobStatus += " osoba prawna musi mieć nip ";
                        //x.IdSprawaWiena = -1;
                        //goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;

                    }

                    if (string.IsNullOrWhiteSpace(x.Name))
                    {
                        x.JobStatus += " pole Name musi być wypełnione ";
                        //x.IdSprawaWiena = -1;
                        //goto nextloop;
                        if (badrows != null)
                            badrows.Add(x);
                        continue;

                    }

                    outlst.Add(x);
                    nextloop:
                    if (!String.IsNullOrWhiteSpace(nrKlienta) && x.NrKlienta != nrKlienta)
                    {


                        if ((czyfiz && saldo < 200) || (!czyfiz && saldo < 500))
                        {
                            if (xprev != null)
                            {
                                xprev.JobStatus += " Za niskie saldo ";
                                xprev.IdSprawaWiena = -1;
                                //continue;
                            }

                        }
                        saldo = 0;
                        // ustawienie nowych  wartości

                    }
                    if (!String.IsNullOrWhiteSpace(x.Firstname))
                        czyfiz = true;
                    else
                        czyfiz = false;

                    xprev = x;
                    nrKlienta = x.NrKlienta;
                    saldo += kwota;
                    kwota = 0;
                }
                catch (Exception ex)
                {
                    Utils.LogWriter("Błąd podczas parsowania faktura " + x.Title + "  " + ex.Message);
                    throw ex;
                }
                
            }

                if (xprev!= null && !String.IsNullOrWhiteSpace(nrKlienta) && xprev.NrKlienta != nrKlienta)
                {

                    saldo += kwota;
                    if ((czyfiz && saldo < 200) || (!czyfiz && saldo < 500))
                    {
                        if (xprev != null)
                        {
                        xprev.JobStatus += " Za niskie saldo ";
                            xprev.IdSprawaWiena = -1;
                          
                        }

                    }
                }

                List<ImportRow> outlstLast = new List<ImportRow>();
                nrKlienta = "";
                int packageNo = 1;
                string nkl = "";
                int rowCount  = 0;
                foreach (ImportRow row in outlst.OrderBy(a => a.NrKlienta).ThenBy(a => a.IdSprawaWiena))
                {

                    if (row.IdSprawaWiena < 0)
                    {
                        nrKlienta = row.NrKlienta;
                      if (badrows != null)
                        badrows.Add(row);

                    }
                    if (!string.IsNullOrWhiteSpace(nrKlienta) && nrKlienta == row.NrKlienta)
                        continue;
                rowCount++;
                if (!string.IsNullOrWhiteSpace(nkl) && nkl != row.NrKlienta)
                {
                    if (rowCount > this.rowsLimit)
                    {
                        rowCount = 0;
                        packageNo++; 
                    }

                }
                    row.IdSprawaWiena = packageNo;
                outlstLast.Add(row);

                    nkl = row.NrKlienta;
            }
            return outlstLast;

            }

        private List<ImportRow> FileCCnBNew2Struct()
        {
            try
            {
                List<ImportRow> impLst = new List<ImportRow>();
                badRows = new List<ImportRow>();

                // using (TextReader sr = tempCsvFile new StringReader(this.inDOC))
                // {

                using (GenericParser parser = new GenericParser())
                {
                    parser.SetDataSource(tempCsvFile, System.Text.Encoding.GetEncoding("windows-1250"));
                    parser.ColumnDelimiter = ';';
                    parser.FirstRowHasHeader = true;
                    parser.CommentCharacter = '|';
                    //parser.SkipStartingDataRows = 10;
                    parser.MaxBufferSize = 4096; //;
                    parser.MaxRows = 500000;
                    //parser.TextQualifier = '\"';

                    while (parser.Read())
                    {
                        ImportRow r = new ImportRow();
                        r.SystemName = @"CC&B";
                        r.NrKlienta = parser["Nr Klienta"];
                        r.Name = parser["Nazwa"];
                        r.Firstname = parser["Imie"];
                        r.Secondname = parser["Nazwisko"];
                        r.Nip = parser["NIP"];
                        r.Pesel = parser["PESEL"];
                        r.Kod = parser["Adres Kod"];
                        r.Miejsce = parser["Adres Miejscowosc"];
                        r.Ulica = parser["Adres Ulica"];
                        r.Dom = parser["Adres Nrdom"];
                        r.Lokal = parser["Adres Nrlokdom"];
                        r.DataWysWezw = parser["Data Wezwania"];
                        
                        r.DataWymag = parser["Termin"];
                        r.Saldo = parser["Saldo"];
                        r.Title = parser["Numer Dokumentu"];
                        try
                        {
                            r.DataFaktury = parser["DATA_DOKUMENTU"];
                        }
                        catch (Exception)
                        {
                            ;
                        }
                        try
                        {
                            r.DataSprzedazy = parser["DATA_SPRZEDAZY"];
                        }
                        catch (Exception)
                        {
                            ;
                        }
                        //  ID_PROCESU_POZP TYP_PROCESU_POZP    DATA_UTWORZENIA_POZP ID_DOKUMENTU    RODZAJ_DOKUMENTU DATA_DOKUMENTU  ID_PISMA TYP_PISMA   CZY_OTWARTA_REKLAMACJA CZY_BLOKADA_WIND    SALDO_KONTA

                        int i;
                        int.TryParse(parser["IdNal"], out i);
                        if (i > 0)
                            r.IdNal = i;

                        i = 0;
                        int.TryParse(parser["IdSprWiena"], out i);
                        if (i > 0)
                            r.IdSprawaWiena = i;



                        r.SystemName = (r.SystemName ?? "").Trim();
                        r.NrKlienta = (r.NrKlienta ?? "").Trim();
                        r.Name = (r.Name ?? "").Trim();
                        r.Firstname = (r.Firstname ?? "").Trim();
                        r.Secondname = (r.Secondname ?? "").Trim();
                        r.Nip = (r.Nip ?? "").Trim();
                        r.Pesel = (r.Pesel ?? "").Trim();
                        r.Kod = (r.Kod ?? "").Trim();
                        r.Miejsce = (r.Miejsce ?? "").Trim();
                        r.Ulica = (r.Ulica ?? "").Trim();
                        r.Dom = (r.Dom ?? "").Trim();
                        r.Lokal = (r.Lokal ?? "").Trim();
                        r.DataWysWezw = (r.DataWysWezw ?? "").Trim();
                        r.DataWymag = (r.DataWymag ?? "").Trim();
                        r.Saldo = (r.Saldo ?? "").Trim();
                        r.Title = (r.Title ?? "").Trim();
                        // opracowanie imion
                        if (String.IsNullOrWhiteSpace(r.Ulica))
                            r.Ulica = r.Miejsce;
                        if (String.IsNullOrWhiteSpace(r.Miejsce))
                            r.Miejsce = r.Ulica;
                        if (r.Name.Length > 3 && r.Name.Substring(r.Name.Length - 1) == ".")
                            r.Name = r.Name.Substring(0, r.Name.Length - 1).Trim();
                        if (r.Name.Length > 3 && r.Name.Substring(r.Name.Length - 1) == ",")
                            r.Name = r.Name.Substring(0, r.Name.Length - 1).Trim();

                        if (!string.IsNullOrWhiteSpace(r.Secondname))
                        {
                            // osoba fizyczna
                            r.Name = r.Secondname;
                            r.Secondname = string.Empty;
                            r.Nip = string.Empty;
                        }
                        else
                        {
                            r.Firstname = string.Empty;
                            r.Pesel = string.Empty;

                        }

                        // filtrowanie danych

                        //  if ( )
                        impLst.Add(r);

                    }

                }

                List<ImportRow> outLst;
                outLst = filterList(impLst, ref badRows, 0);

                //  }
                return outLst;
            }
            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas parsowania zbioru wejściowego" + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return null;
            }
        }


        private List<ImportRow> FileCCnBLatest2Struct()
        {
            try
            {
                List<ImportRow> impLst = new List<ImportRow>();
                badRows = new List<ImportRow>();

                // using (TextReader sr = tempCsvFile new StringReader(this.inDOC))
                // {

                using (GenericParser parser = new GenericParser())
                {
                    parser.SetDataSource(tempCsvFile, System.Text.Encoding.GetEncoding("windows-1250"));
                    parser.ColumnDelimiter = ';';
                    parser.FirstRowHasHeader = true;
                    parser.CommentCharacter = '|';
                    //parser.SkipStartingDataRows = 10;
                    parser.MaxBufferSize = 4096; //;
                    parser.MaxRows = 500000;
                    //parser.TextQualifier = '\"';

                    while (parser.Read())
                    {
                        ImportRow r = new ImportRow();
                        r.SystemName = @"CC&B";
                        r.NrKlienta = parser["Nr Klienta"]; 

                        r.Name = parser["Nazwa"];
                        r.Firstname = parser["Imie"];
                        r.Secondname = parser["Nazwisko"];
                        r.Nip = parser["NIP"];
                        r.Pesel = parser["PESEL"];
                        r.Kod = parser["Adres Kod"];
                        r.Miejsce = parser["Adres Miejscowosc"];
                        r.Ulica = parser["Adres Ulica"];
                        r.Dom = parser["Adres Nrdom"];
                        r.Lokal = parser["Adres Nrlokdom"];
                        r.DataWysWezw = parser["Data Wezwania"];

                        r.DataWymag = parser["Termin"];
                        r.Saldo = parser["Saldo"];
                        r.Title = parser["Numer Dokumentu"];
                        try
                        {
                            r.DataFaktury = parser["DATA_DOKUMENTU"];
                        }
                        catch (Exception)
                        {
                            ;
                        }
                        try
                        {
                            r.DataSprzedazy = parser["DATA_SPRZEDAZY"];
                        }
                        catch (Exception)
                        {
                            ;
                        }
                        //  ID_PROCESU_POZP TYP_PROCESU_POZP    DATA_UTWORZENIA_POZP ID_DOKUMENTU    RODZAJ_DOKUMENTU DATA_DOKUMENTU  ID_PISMA TYP_PISMA   CZY_OTWARTA_REKLAMACJA CZY_BLOKADA_WIND    SALDO_KONTA

                        int i;
                        int.TryParse(parser["IdNal"], out i);
                        if (i > 0)
                            r.IdNal = i;

                        i = 0;
                        int.TryParse(parser["IdSprWiena"], out i);
                        if (i > 0)
                            r.IdSprawaWiena = i;



                        r.SystemName = (r.SystemName ?? "").Trim();
                        r.NrKlienta = (r.NrKlienta ?? "").Trim();
                        r.Name = (r.Name ?? "").Trim();
                        r.Firstname = (r.Firstname ?? "").Trim();
                        r.Secondname = (r.Secondname ?? "").Trim();
                        r.Nip = (r.Nip ?? "").Trim();
                        r.Pesel = (r.Pesel ?? "").Trim();
                        r.Kod = (r.Kod ?? "").Trim();
                        r.Miejsce = (r.Miejsce ?? "").Trim();
                        r.Ulica = (r.Ulica ?? "").Trim();
                        r.Dom = (r.Dom ?? "").Trim();
                        r.Lokal = (r.Lokal ?? "").Trim();
                        r.DataWysWezw = (r.DataWysWezw ?? "").Trim();
                        r.DataWymag = (r.DataWymag ?? "").Trim();
                        r.Saldo = (r.Saldo ?? "").Trim();
                        r.Title = (r.Title ?? "").Trim();
                        // opracowanie imion
                        if (String.IsNullOrWhiteSpace(r.Ulica))
                            r.Ulica = r.Miejsce;
                        if (String.IsNullOrWhiteSpace(r.Miejsce))
                            r.Miejsce = r.Ulica;
                        if (r.Name.Length > 3 && r.Name.Substring(r.Name.Length - 1) == ".")
                            r.Name = r.Name.Substring(0, r.Name.Length - 1).Trim();
                        if (r.Name.Length > 3 && r.Name.Substring(r.Name.Length - 1) == ",")
                            r.Name = r.Name.Substring(0, r.Name.Length - 1).Trim();

                        if (!string.IsNullOrWhiteSpace(r.Secondname))
                        {
                            // osoba fizyczna
                            r.Name = r.Secondname;
                            r.Secondname = string.Empty;
                            r.Nip = string.Empty;
                        }
                        else
                        {
                            r.Firstname = string.Empty;
                            r.Pesel = string.Empty;

                        }

                        // filtrowanie danych

                        //  if ( )
                        impLst.Add(r);

                    }

                }

                List<ImportRow> outLst;
                outLst = filterList(impLst, ref badRows,0);

                //  }
                return outLst;
            }
            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas parsowania zbioru wejściowego" + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return null;
            }
        }
        private List<ImportRow> FileCCnB2Struct()
        {
            try
            {
                List<ImportRow> impLst = new List<ImportRow>();
                badRows = new List<ImportRow>();

               // using (TextReader sr = tempCsvFile new StringReader(this.inDOC))
               // {

                    using (GenericParser parser = new GenericParser())
                    {
                        parser.SetDataSource(tempCsvFile, System.Text.Encoding.GetEncoding("windows-1250"));
                        parser.ColumnDelimiter = ';';
                        parser.FirstRowHasHeader = true;
                        parser.CommentCharacter = '|';
                    //parser.SkipStartingDataRows = 10;
                        parser.MaxBufferSize = 4096; //;
                        parser.MaxRows = 500000;
                        //parser.TextQualifier = '\"';

                        while (parser.Read())
                        {
                            ImportRow r = new ImportRow();
                            r.SystemName = @"CC&B";
                            r.NrKlienta = parser["NR_KLIENTA"];
                            r.Name = parser["NAZWA"];
                            r.Firstname = parser["IMIE"];
                            r.Secondname = parser["DRUGIE_IMIE"];
                            r.Nip = parser["NIP"];
                            r.Pesel = parser["PESEL"];
                            r.Kod = parser["ADRES_KOD"];
                            r.Miejsce = parser["ADRES_MIEJSCOWOSC"];
                            r.Ulica = parser["ADRES_ULICA"];
                            r.Dom = parser["ADRES_NRDOM"];
                            r.Lokal = parser["ADRES_NRLOK"];
                            r.DataWysWezw = parser["DATA_WEZWANIA"];
                             if ( String.IsNullOrWhiteSpace(r.DataWysWezw) )
                            r.DataWysWezw = parser["DATA_WYSLANIA_WEZWANIA"]; ;
                            r.DataWymag = parser["TERMIN_PLATNOSCI"];
                            r.Saldo = parser["SALDO_FAKTURY"];
                            r.Title = parser["NUMER_DOKUMENTU"];
                        try
                        {
                            r.DataFaktury = parser["DATA_DOKUMENTU"];
                        }
                        catch (Exception)
                        {
                            ;
                        }
                        try
                        {
                            r.DataSprzedazy = parser["DATA_SPRZEDAZY"];
                        }
                        catch (Exception)
                        {
                            ;
                        }
                        //  ID_PROCESU_POZP TYP_PROCESU_POZP    DATA_UTWORZENIA_POZP ID_DOKUMENTU    RODZAJ_DOKUMENTU DATA_DOKUMENTU  ID_PISMA TYP_PISMA   CZY_OTWARTA_REKLAMACJA CZY_BLOKADA_WIND    SALDO_KONTA

                        int i;
                            int.TryParse(parser["IdNal"], out i);
                            if (i > 0)
                                r.IdNal = i;

                            i = 0;
                            int.TryParse(parser["IdSprWiena"], out i);
                            if (i > 0)
                                r.IdSprawaWiena = i;



                            r.SystemName = (r.SystemName ?? "").Trim();
                            r.NrKlienta = (r.NrKlienta ?? "").Trim();
                            r.Name = (r.Name ?? "").Trim();
                            r.Firstname = (r.Firstname ?? "").Trim();
                            r.Secondname = (r.Secondname ?? "").Trim();
                            r.Nip = (r.Nip ?? "").Trim();
                            r.Pesel = (r.Pesel ?? "").Trim();
                            r.Kod = (r.Kod ?? "").Trim();
                            r.Miejsce = (r.Miejsce ?? "").Trim();
                            r.Ulica = (r.Ulica ?? "").Trim();
                            r.Dom = (r.Dom ?? "").Trim().Truncate(20);
                            r.Lokal = (r.Lokal ?? "").Trim().Truncate(20);
                            r.DataWysWezw = (r.DataWysWezw ?? "").Trim();
                            r.DataWymag = (r.DataWymag ?? "").Trim();
                            r.Saldo = (r.Saldo ?? "").Trim();
                            r.Title = (r.Title ?? "").Trim();
                        // opracowanie imion
                        if (String.IsNullOrWhiteSpace(r.Ulica))
                             r.Ulica = r.Miejsce;
                        if (String.IsNullOrWhiteSpace(r.Miejsce))
                            r.Miejsce = r.Ulica;
                        if (r.Name.Length > 3 && r.Name.Substring(r.Name.Length - 1) == "." )
                            r.Name = r.Name.Substring(0, r.Name.Length - 1).Trim();
                        if (r.Name.Length > 3 && r.Name.Substring(r.Name.Length - 1) == ",")
                            r.Name = r.Name.Substring(0, r.Name.Length - 1).Trim();

                        if ((r.Name.Contains("'") || r.Name.Contains("\"") && !String.IsNullOrWhiteSpace(r.Nip)))  // osoba prawna
                            ;
                        else
                        if (r.Name.Contains(',') && !String.IsNullOrWhiteSpace(r.Pesel))
                        {
                            if (r.Name.Count(f => f == ',') > 1)
                            {
                                // osoba prawna
                                ;
                            }
                            else
                            {
                                int sepPos = r.Name.IndexOf(',');
                                string nazwisko = r.Name.Substring(0, sepPos).Trim();
                                string imie = "";
                                if (sepPos < r.Name.Length - 1)
                                    imie = r.Name.Substring(sepPos + 1).Trim();
                                if (!string.IsNullOrWhiteSpace(imie))
                                {
                                    r.Name = nazwisko;
                                    r.Firstname = imie;

                                }
                            }   

                           

                        }
                       

                        // filtrowanie danych

                        //  if ( )
                                  impLst.Add(r);

                        }

                    }

                List<ImportRow> outLst;
                outLst = filterList(impLst,ref badRows,0);

              //  }
                return outLst;
            }
            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas parsowania zbioru wejściowego" + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return null;
            }
        }
      

        private int saveJob(List<ImportRow> rowLst, bool wrongRows = false )
        {
            if (rowLst == null || !rowLst.Any())
            {
                return 0;


             }
            BIG_Job bj = new BIG_Job();


            try
            {

                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    bj = new BIG_Job();
                    bj.dStart = DateTime.Now;
                    bj.CurrRow = 0;
                    bj.FName = this.importFileName;
                    bj.RowsNo = 0;
                    bj.ForceStop = 0;
                    bj.Name = this.userName;
                    if (wrongRows)
                    {
                        bj.CurrRow = -1;
                        bj.Message = "Błędne zapisy";

                    }
                    context.BIG_Job.AddObject(bj);
                    context.SaveChanges();
                    foreach (ImportRow i in rowLst)
                    {
                        i.IdNal = bj.BIG_JobId;

                    }

                    string cs = ((EntityConnection)context.Connection).StoreConnection.ConnectionString;
                    using (SqlConnection sqlConn = new SqlConnection(cs))
                    {
                        DataTable dt = ExtUtils.ToDataTable<ImportRow>(rowLst);
                        sqlConn.Open();
                        
                        using (SqlBulkCopy sqlbc = new SqlBulkCopy(sqlConn))
                        {
                            sqlbc.BulkCopyTimeout = 120;
                            sqlbc.DestinationTableName = "BIG_JobRow";
                            sqlbc.ColumnMappings.Add("IdNal", "BIG_JobId");
                            sqlbc.ColumnMappings.Add("Adr1", "Adr1");
                            sqlbc.ColumnMappings.Add("Adr2", "Adr2");
                            sqlbc.ColumnMappings.Add("czywyrok", "czywyrok");
                            sqlbc.ColumnMappings.Add("DataWymag", "DataWymag");
                            sqlbc.ColumnMappings.Add("DataWysWezw", "DataWysWezw");
                            sqlbc.ColumnMappings.Add("Dom", "Dom");
                            sqlbc.ColumnMappings.Add("dorzecz", "dorzecz");
                            sqlbc.ColumnMappings.Add("Firstname", "Firstname");
                            sqlbc.ColumnMappings.Add("IdNal", "IdNal");
                            sqlbc.ColumnMappings.Add("IdSprawaWiena", "IdSprawaWiena");
                            sqlbc.ColumnMappings.Add("Kod", "Kod");
                            sqlbc.ColumnMappings.Add("Lokal", "Lokal");
                            sqlbc.ColumnMappings.Add("Miejsce", "Miejsce");
                            sqlbc.ColumnMappings.Add("Name", "Name");
                            sqlbc.ColumnMappings.Add("Nip", "Nip");
                            sqlbc.ColumnMappings.Add("NrKlienta", "NrKlienta");
                            sqlbc.ColumnMappings.Add("Pesel", "Pesel");
                            sqlbc.ColumnMappings.Add("Saldo", "Saldo");
                            sqlbc.ColumnMappings.Add("Secondname", "Secondname");
                            sqlbc.ColumnMappings.Add("SygnOrzecz", "SygnOrzecz");
                            sqlbc.ColumnMappings.Add("SystemName", "SystemName");
                            sqlbc.ColumnMappings.Add("Title", "Title");
                            sqlbc.ColumnMappings.Add("TypNal", "TypNal");
                            sqlbc.ColumnMappings.Add("Ulica", "Ulica");
                            sqlbc.ColumnMappings.Add("IdSprawaWiena", "PartNo");
                            sqlbc.ColumnMappings.Add("JobStatus", "Message");
                            sqlbc.ColumnMappings.Add("DataFaktury", "DataFaktury");
                            sqlbc.ColumnMappings.Add("DataSprzedazy", "DataSprzedazy");
                            sqlbc.WriteToServer(dt);

                        }
                    }

                    bj.RowsNo = rowLst.Count();
                    context.SaveChanges();
                }
                return bj.BIG_JobId;
            }
            catch (Exception ex)
            {
                errCode = -500;
                errDescription = "błąd podczas zapisu zbioru wejściowego " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return -1;
            }



            /*
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    bj = new BIG_Job();
                    bj.dStart = DateTime.Now;
                    bj.CurrRow = 0;
                    bj.FName = this.importFileName;
                    bj.RowsNo = 0;
                    bj.ForceStop = 0;
                    context.BIG_Job.AddObject(bj);
                    int looNo = 0;
                    foreach (ImportRow impR in rowLst)
                    {
                        BIG_JobRow bjr = new BIG_JobRow();
                        bjr.Adr1 = impR.Adr1;
                        bjr.Adr2 = impR.Adr2;
                        bjr.czywyrok = impR.czywyrok;
                        bjr.DataWymag = impR.DataWymag;
                        bjr.DataWysWezw = impR.DataWysWezw;
                        bjr.Dom = impR.Dom;
                        bjr.dorzecz = impR.dorzecz;
                        bjr.dStart = DateTime.Now;
                        bjr.Firstname = impR.Firstname;
                        bjr.IdNal = impR.IdNal;
                        bjr.IdSprawaWiena = impR.IdSprawaWiena;
                        bjr.Kod = impR.Kod;
                        bjr.Lokal = impR.Lokal;
                        bjr.Message = "";
                        bjr.Miejsce = impR.Miejsce;
                        bjr.Name = impR.Name;
                        bjr.Nip = impR.Nip;
                        bjr.NrKlienta = impR.NrKlienta;
                        bjr.Pesel = impR.Pesel;
                        bjr.Saldo = impR.Saldo;
                        bjr.Secondname = impR.Secondname;
                        bjr.Status = 0;
                        bjr.SygnOrzecz = impR.SygnOrzecz;
                        bjr.SystemName = impR.SystemName;
                        bjr.Title = impR.Title;
                        bjr.TypNal = impR.TypNal;
                        bjr.Ulica = impR.Ulica;
                        bjr.WienaSygn = impR.WienaSygn;
                        bj.RowsNo++;
                        bj.BIG_JobRow.Add(bjr);
                        looNo++;
                        if (looNo == 1000)
                        {
                            Utils.LogWriter("Zapis do joba");
                            context.SaveChanges();
                            looNo = 0; 
                        }



                    }
                    context.SaveChanges();

                }

                return bj.BIG_JobId;
            }

             catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas zapisu zbioru wejściowego " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return -1;
            }

    */

        }


        public bool ProceedFile(int mode= 1)
        {

            List<ImportRow> ipLst = File2Struct(mode);
            if (ipLst == null || !ipLst.Any())
                return false;
            return (ProceedStruct(ipLst) != null);
        }



        public int ProceedFileCCnB(bool isNew = false)
        {
            this.badRows = null;
            if (isNew)
                this.callContext = 5;
            else
                this.callContext = 4;
            List<ImportRow> ipLst;
            if (isNew)
                ipLst = FileCCnBNew2Struct();
            else
                ipLst = FileCCnB2Struct();
            if (ipLst == null || !ipLst.Any())
                return  -1;
            int code  = saveJob(ipLst);
            if (this.badRows != null & badRows.Any())
            {
                saveJob(badRows,true);

            }
                return   code ;


          // return (ProceedStruct(ipLst) != null);
        }
        public bool ProceedFileUpdate(int User_Id, int firma, int mode)
        {
            Utils.LogWriter("ProceedFileUpdate");

            List<ImportRow> ipLst = File2Struct(firma, mode);
            if (ipLst == null || !ipLst.Any())
                return false;
                int code = saveJob(ipLst);
            if (this.badRows != null & badRows.Any())
            {
                saveJob(badRows, true);

            }
            Utils.LogWriter("jobs saved");
            try
            {
                if (firma == 1) mode = 1; 
                using (LexEnaMeritumEntities dbcontext = new LexEnaMeritumEntities())
                {
                    Uzytkownik u = dbcontext.Uzytkownik.Where(a => a.Id == User_Id).FirstOrDefault();
                    dbcontext.CommandTimeout = 1200;
                    SqlParameter param1 = new SqlParameter("IdJob", code);
                    SqlParameter param2 = new SqlParameter("UserName", u.Imie + " " + u.Nazwisko);
                    SqlParameter param3 = new SqlParameter("context", mode);
                    object obj = dbcontext.ExecuteStoreQuery<object>("USP_BIGImportSelAUMS  @IdJob, @UserName, @context ", param1, param2, param3).FirstOrDefault();

                    return true;
                }

            }
            catch (ArgumentException ex)
            {
                errDescription = "Błąd " +  ex.Message + " " + ex.InnerException;
                return false;
            }
           

            //return (ProceedStruct(ipLst, 1) != null);
        }



        public bool ProceedFileWiena(int mode = 1)
        {

            List<ImportRow> ipLst = File2Struct(mode);
            if (ipLst == null || !ipLst.Any())
                return false;
            return (ProceedStruct(ipLst,2) != null);
        }


        public BIG_Import ProceedStruct(List<ImportRow> lst, int mode = 0  ,  BIG_Import biIn = null, LexEnaMeritumEntities thecontext = null, int debug = 0 )
        {
            // mode 0 - import z billingu 
            // mode = 1 - import różnicowy z billingu
            // mode = 2 - import  Wieny
            // cc&b tylko - billing !!! 

            LexEnaMeritumEntities context = null;    
            BIG_Import bi = null;
            List<BIGvw_ObligationLastStatus> oblStatLst = null;
            BIG_Case prevcase = null;
            BIG_Debtor prevdebtor = null;
            BIGvw_DluznicyAktual prevdebtaktual = null;
            bool forceSave = false;
            bool czyWiena= false;
            string mess = "";
            if (mode == 2) czyWiena = true;
            if (biIn != null)
                bi = biIn;

            if (thecontext != null)
                context = thecontext;
            else
            {
                context = new LexEnaMeritumEntities();
                if (biIn != null)
                    bi = context.BIG_Import.Where(a => a.BIG_ImportId ==  biIn.BIG_ImportId).FirstOrDefault();

            }
            if (debug > 0)
            {

                bi = context.BIG_Import.Where(a => a.BIG_ImportId == debug).FirstOrDefault();
               

            }

            Utils.LogNamedWriter("ProceedFile strct ", "AsyncLog.Txt");
            try
            {
                if (debug == 0)
                {

                    bool importAdded = false;
                    if (mode == 2)
                        oblStatLst = context.BIGvw_ObligationLastStatus.Where(a => a.SrcSystem == 11).ToList();
                    else if (mode == 4)
                        oblStatLst = context.BIGvw_ObligationLastStatus.Where(a => a.SrcSystem == 3).ToList();
                    else
                        oblStatLst = context.BIGvw_ObligationLastStatus.Where(a => a.SrcSystem < 3).ToList();
                    if (oblStatLst == null)
                        oblStatLst = new List<BIGvw_ObligationLastStatus>();

                    Utils.LogNamedWriter("Lst Loaded ", "AsyncLog.Txt");

                    foreach (ImportRow row in lst)
                    {
                        forceSave = false;
                        mess = row.NrKlienta;
                        Utils.LogNamedWriter("Klient " + mess, "AsyncLog.Txt");
                        BIG_Debtor bd = new BIG_Debtor();
                        BIG_Case bc = new BIG_Case();
                        BIG_Obligation bo = new BIG_Obligation();
                        string systemName;


                        systemName = row.SystemName;
                        switch (systemName.Trim().ToUpper())
                        {
                            case "AUMS":
                                bc.SrcSystem = 1;
                                break;
                            case "SELEN":
                                bc.SrcSystem = 2;
                                break;
                            case "CC&B":
                                bc.SrcSystem = 3;
                                break;
                            case "WIENA":
                                bc.SrcSystem = 11;
                                break;
                            case "LEXENA":
                                bc.SrcSystem = 12;
                                break;
                            default:
                                bc.SrcSystem = 0;
                                break;
                        }
                        bd.NrKlienta = row.NrKlienta;
                        bd.Name = row.Name;
                        bd.Firstname = row.Firstname;
                        bd.Secondname = row.Secondname;

                        if (String.IsNullOrWhiteSpace(bd.Firstname))
                        {
                            bd.DebtorType = 1;
                            bd.IdentityNumberType = 1;
                            bd.IDNumber = row.Nip;
                        }
                        else
                        {
                            bd.DebtorType = 0;  // osoba fizyczna
                            bd.IdentityNumberType = 2; // pesel
                            bd.IDNumber = row.Pesel;
                        }
                        string adrl1, adrl2;
                        if (!String.IsNullOrWhiteSpace(row.Adr1))
                            adrl1 = row.Adr1;
                        else
                            adrl1 = row.Ulica + " " + row.Dom + ((!String.IsNullOrWhiteSpace(row.Lokal) && row.Lokal != "0") ? "/" + row.Lokal : "");
                        if (!String.IsNullOrWhiteSpace(row.Adr2))
                            adrl2 = row.Adr2;
                        else
                            adrl2 = row.Kod + " " + row.Miejsce;
                        //     Utils.LogWriter("1 " + DateTime.Now.ToString("HHmmssfff")); ;
                        bd.Address1L1 = adrl1.Trim();
                        bd.Address1L2 = adrl2.Trim();
                        Utils.LogWriter(row.DataWysWezw + "   " + row.DataWymag);
                        bo.DataWysWezw = Convert.ToDateTime(row.DataWysWezw);
                        bo.TypNal = (int)obligationTypeEnum.Invoice;

                        bo.DataWymag = Convert.ToDateTime(row.DataWymag);
                        bo.Saldo = Convert.ToDecimal(row.Saldo.Replace(".", ","), CultureInfo.GetCultureInfo("pl-PL"));
                        bo.Title = row.Title;
                        bc.CaseId = Utils.combineCaseId(bc.SrcSystem.Value, bd.NrKlienta, bd.IDNumber);
                        bo.ObligationId = bd.NrKlienta + "/" + bd.IDNumber + "/" + bo.Title.Replace(" ", "") + "/" + row.DataWymag.Replace("-", "") + (bc.SrcSystem == 11 ? "/" + row.IdNal.ToString() : "");
                        mess = bo.ObligationId;
                        bo.IdWienaNal = row.IdNal;
                        bc.IdWienaSpr = row.IdSprawaWiena;
                        if (!importAdded)
                        {
                            if (bi == null)
                                bi = new BIG_Import();
                            else
                            {
                                if (bi.BIG_ImportId > 0)
                                {
                                    bi = context.BIG_Import.Where(b => b.BIG_ImportId == bi.BIG_ImportId).FirstOrDefault();
                                    importAdded = true;
                                }
                                else
                                {
                                    ;//  context.SaveChanges();

                                }


                            }
                            bi.DataImportu = DateTime.Now;
                            bi.Username = this.userName;
                            bi.Status = 0; // dodano
                            bi.StatOpis = "Nowa/Zaimportowana";
                            bi.lBlad = 0;
                            bi.Filename = this.importFileName;
                            bi.lSuccess = 0;
                            bi.lPoz = 0;
                            bi.TypImp = 1; // import z pliku ( upodate)

                        }

                        BIG_Operacja bigoperacja = new BIG_Operacja();
                        bigoperacja.DataOperacji = DateTime.Now;
                        bi.BIG_Operacja.Add(bigoperacja);

                        // szukamy sprawy z takim dłużnikiem

                        BIGvw_ObligationLastStatus bol = null;
                        BIGvw_DluznicyAktual bdebt = null;
                        Utils.LogWriter("2 " + DateTime.Now.ToString("HHmmssfff"));
                        if (czyWiena)
                        { bol = oblStatLst.Where(a => a.CaseId == bc.CaseId && a.ObligationId == bo.ObligationId).FirstOrDefault();

                            if (prevdebtaktual != null && prevdebtaktual.CaseId == bc.CaseId && prevdebtaktual.SrcSystem == 11)
                                bdebt = prevdebtaktual;
                            else
                            {

                                bdebt = (from c in context.BIGvw_DluznicyAktual
                                         where c.CaseId == bc.CaseId && c.SrcSystem == 11
                                         orderby c.BIG_CaseId descending, c.BIG_DebtorId descending
                                         select c).FirstOrDefault();
                                if (bdebt != null)
                                    prevdebtaktual = bdebt;

                            }
                        }
                        else
                        {
                            bol = oblStatLst.Where(a => a.CaseId == bc.CaseId && a.ObligationId == bo.ObligationId).FirstOrDefault();
                            if (prevdebtaktual != null && prevdebtaktual.CaseId == bc.CaseId && prevdebtaktual.SrcSystem < 10)
                                bdebt = prevdebtaktual;
                            else
                            {
                                bdebt = (from c in context.BIGvw_DluznicyAktual
                                         where c.CaseId == bc.CaseId && c.SrcSystem < 10
                                         orderby c.BIG_CaseId descending, c.BIG_DebtorId descending
                                         select c).FirstOrDefault();
                                if (bdebt != null)
                                    prevdebtaktual = bdebt;
                            }
                        }
                        BIG_Case bcget = null;
                        Utils.LogWriter("3 " + DateTime.Now.ToString("HHmmssfff"));
                        BIG_Debtor bdeptor = null;

                        if (bdebt != null)
                        {
                            if (prevdebtor != null && bdebt.BIG_DebtorId == prevdebtor.BIG_DebtorId)
                                bdeptor = prevdebtor;
                            else
                            {
                                List<BIG_Debtor> tmpB = context.BIG_Debtor.Where(a => a.BIG_DebtorId == bdebt.BIG_DebtorId).OrderByDescending(a => a.BIG_DebtorId).ToList();
                                if (tmpB != null && tmpB.Count == 1)
                                    bdeptor = tmpB.FirstOrDefault();
                                else if (tmpB.Count > 1)
                                {
                                    bdeptor = tmpB.OrderByDescending(a => a.BIG_DebtorId).FirstOrDefault();
                                }
                                if (bdeptor != null)
                                    prevdebtor = bdeptor;
                            }
                        }
                        else
                            bdeptor = null;

                        if (bdeptor != null)
                        {

                            bd = bdeptor;
                        }
                        else
                            forceSave = true;

                        if (bdebt != null && bdebt.BIG_CaseId > 0)
                        {
                            if (prevcase != null && prevcase.BIG_CaseId == bdebt.BIG_CaseId)
                                bcget = prevcase;
                            else
                            {
                                bcget = context.BIG_Case.Where(a => a.BIG_CaseId == bdebt.BIG_CaseId).FirstOrDefault();
                                if (bcget != null)
                                    prevcase = bcget;
                            }
                        }
                        if (bol == null)
                        {


                            bigoperacja.TypOperacji = 1; //add



                        }
                        else
                        {
                            // ew update  - standardowy

                            bigoperacja.TypOperacji = 2; // update

                        }

                        if (bcget != null)
                        {
                            bc = bcget;
                            if (bd.BIG_CaseId == bc.BIG_CaseId)
                                ;
                            else
                                bc.BIG_Debtor.Add(bd);

                        }
                        else
                        {
                            bc.CaseBIGId = "New";
                            bc.BIG_Debtor.Add(bd);
                            bi.BIG_Case.Add(bc);
                            forceSave = true;

                        }
                        Utils.LogWriter("4 " + DateTime.Now.ToString("HHmmssfff"));
                        bo.BIG_Operacja.Add(bigoperacja);
                        bc.BIG_Operacja.Add(bigoperacja);
                        bd.BIG_Operacja.Add(bigoperacja);
                        bc.BIG_Obligation.Add(bo);

                        bi.lPoz++;
                        if (bi.EntityState == System.Data.EntityState.Detached)
                        {
                            context.BIG_Import.AddObject(bi);
                            importAdded = true;

                        }
                        {
                            BIG_JobRow bigJobrow = context.BIG_JobRow.Where(a => a.BIG_JobRowId == row.BIG_JobRowId).FirstOrDefault();
                            if (bigJobrow != null)
                                bigJobrow.Status = 1;


                        }
                        Utils.LogWriter("5 " + DateTime.Now.ToString("HHmmssfff"));
                        if (forceSave)
                            context.SaveChanges();
                        prevcase = bc;
                        prevdebtor = bd;
                        Utils.LogWriter("6 " + DateTime.Now.ToString("HHmmssfff"));
                    }
                    Utils.LogNamedWriter("EndForeach " + mess, "AsyncLog.Txt");
                    context.SaveChanges();
                }
                //
                Utils.LogNamedWriter("Step 1 ", "AsyncLog.Txt");
                updateDelete:
                //dodanie pustych operacji delete 
                List<BIG_Obligation> lstOblig = (from oper in context.BIG_Operacja
                                                 join
                                                 oblig in context.BIG_Obligation on oper.BIG_ObligationId equals oblig.BIG_ObligationId
                                                 join spr in context.BIG_Case on oblig.BIG_CaseId equals spr.BIG_CaseId
                                                 where oper.BIG_ImportId == bi.BIG_ImportId
                                                 select oblig).ToList();

                if (lstOblig != null && mode == 0)
                {
                    List<string> idoblig = lstOblig.Select(a => a.ObligationId).ToList();
                    List<int?> idcase = lstOblig.Select(a => a.BIG_CaseId).Distinct().ToList();

                    List<IdCaseOblig> oblcase;
                    if (mode == 4)
                        oblcase = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_ImportId != bi.BIG_ImportId && a.SrcSystem == 3).Select(a => new IdCaseOblig { CaseId = a.BIG_CaseId, ObligationId = a.ObligationId, DebtorId = a.BIG_DebtorId, BIGObl_Id = a.BIG_ObligationId }).ToList();
                    else
                        oblcase = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_ImportId != bi.BIG_ImportId && a.SrcSystem < 3).Select(a => new IdCaseOblig { CaseId = a.BIG_CaseId, ObligationId = a.ObligationId, DebtorId = a.BIG_DebtorId, BIGObl_Id = a.BIG_ObligationId }).ToList();


                    List<IdCaseOblig> oblcaseToDel = oblcase.Where(p => !lstOblig.Any(p2 => p2.ObligationId == p.ObligationId)).Select(a => new IdCaseOblig { CaseId = a.CaseId, ObligationId = a.ObligationId, DebtorId = a.DebtorId, BIGObl_Id = a.BIGObl_Id }).ToList();




                    // foreach (int i in idcase)
                    //         {
                    //            List<string> oblcase = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_CaseId == i && a.BIG_ImportId != bi.BIG_ImportId && a.SrcSystem < 10).Select(a => a.ObligationId).ToList();
                    //             bool found = false;
                    //              foreach (string j in oblcase)
                    //               {
                    Utils.LogNamedWriter("Step 2", "AsyncLog.Txt");
                    foreach (IdCaseOblig infId in oblcaseToDel)
                    //   if (!idoblig.Contains(j)) // jeśłi nie ma na liście to usuwamy
                    {
                        BIG_Operacja b = new BIG_Operacja();
                        b.BIG_ImportId = bi.BIG_ImportId;
                        b.TypOperacji = 3; // usunięcie;
                        b.StatOpis = "Niejawne usunięcie należności";
                        b.StatusOperacji = 0;
                        b.BIG_ObligationId = infId.BIGObl_Id;
                        b.DataOperacji = DateTime.Now;
                        b.DataProcedowaniaKrd = DateTime.Now;
                        // usunięcie 
                        b.BIG_CaseId = infId.CaseId;
                        b.BIG_DebtorId = infId.DebtorId;
                        context.BIG_Operacja.AddObject(b);
                        context.SaveChanges();
                        //                 }

                        //             }

                        //          }

                    }
                }
                //
                Utils.LogNamedWriter("Step 4 ", "AsyncLog.Txt");
                if (mode == 0) // jeśli to jest import pełny z systemu billingowego - zawieszamy  pzozstałe należności lub je usuwamy
                {
                    List<BIGvw_DluznicyAktual> debtLst;
                    if (mode == 4)
                        debtLst = context.BIGvw_DluznicyAktual.Where(a => a.SrcSystem == 3).ToList();
                    else
                        debtLst = context.BIGvw_DluznicyAktual.Where(a => a.SrcSystem < 3).ToList();

                    if (debtLst != null && debtLst.Any())
                    {
                        int ii = 0;
                        foreach (BIGvw_DluznicyAktual dlu in debtLst)
                        {
                            ii++;
                            if (bi != null)
                                if (bi.BIG_Operacja.Where(b => b.BIG_CaseId == dlu.BIG_CaseId).FirstOrDefault() != null) continue;
                            BIGvw_ObligationLastStatus obli = context.BIGvw_ObligationLastStatus.Where(b => b.BIG_CaseId == dlu.BIG_CaseId && b.AutoSuspend == true).FirstOrDefault();
                            if (obli != null)
                            {
                                if (obli.SuspendDate < DateTime.Today)
                                    this.DeleteCase(dlu.BIG_DebtorId, context);


                            }
                            else
                                this.suspendCase(dlu, DateTime.Today.AddDays(14), true, context, bi);
                            // zawieszamy 
                        }

                    }

                }

                if (thecontext == null) context.Dispose();
                Utils.LogNamedWriter("Step 5 OK ", "AsyncLog.Txt");
                return bi;
            }
            catch (EntityCommandExecutionException ex2)
            {
                
                errCode = -500;
                errDescription = "błąd podczas przetwarzania struktury wejściowej " + ex2.Message + (ex2.InnerException != null ? " " + ex2.InnerException : "");
                Utils.LogWriter(errDescription);
                Utils.LogNamedWriter("Except " + errDescription + " " + mess, "AsyncLog.Txt");
                if (thecontext == null) context.Dispose();
                throw ex2;



            }

            catch (System.Data.DataException ex1)
            {
                errCode = -500;
                errDescription = "błąd podczas przetwarzania struktury wejściowej " + ex1.Message + (ex1.InnerException != null ? " " + ex1.InnerException : "");
                Utils.LogWriter(errDescription);
                Utils.LogNamedWriter("Except " + errDescription + " " + mess, "AsyncLog.Txt");
                if (thecontext == null) context.Dispose();
                throw ex1;
            }

            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas przetwarzania struktury wejściowej " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                Utils.LogWriter(errDescription);
                Utils.LogNamedWriter("Except " + errDescription + " " + mess, "AsyncLog.Txt");
                if (thecontext == null) context.Dispose();
                throw ex;
            }
        }



        public BIG_Import DeleteCase(int CaseId, LexEnaMeritumEntities thecontext  = null)
        {
            LexEnaMeritumEntities context = null;

            try
            {

                BIG_Import bi = null;
               
                if (thecontext == null)
                    context = new LexEnaMeritumEntities();
                else
                    context = thecontext; 
                

                    BIG_Case bc = context.BIG_Case.Where(a => a.BIG_CaseId == CaseId).OrderByDescending(a => a.BIG_CaseId).FirstOrDefault();
                    BIG_Obligation bo = new BIG_Obligation();


                    bi = new BIG_Import();
                    bi.DataImportu = DateTime.Now;
                    bi.Username = this.userName;
                    bi.Status = 0; // dodano
                    bi.StatOpis = "Nowa/Zaimportowana";
                    bi.lBlad = 0;
                    bi.Filename = "Usunięcie przez operatora";
                    bi.lSuccess = 0;
                    bi.lPoz = 0;
                    bi.TypImp = 3; // usunięcie

                    List<BIGvw_ObligationLastStatus> lstToDel = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_CaseId == CaseId).ToList();
                if (lstToDel != null)
                {
                    foreach (BIGvw_ObligationLastStatus bs in lstToDel)
                    {
                        BIG_Operacja bigoperacja = new BIG_Operacja();
                        bigoperacja.DataOperacji = DateTime.Now;
                        bi.BIG_Operacja.Add(bigoperacja);
                        bigoperacja.TypOperacji = 103; //delete
                        bigoperacja.StatOpis = "Wykreślenie sprawy";
                        bigoperacja.BIG_ObligationId = bs.BIG_ObligationId;
                        bigoperacja.BIG_DebtorId = bs.BIG_DebtorId;
                        bigoperacja.BIG_CaseId = bs.BIG_CaseId;
                        bi.BIG_Operacja.Add(bigoperacja);
                        
                    }
                    context.BIG_Import.AddObject(bi);
                    context.SaveChanges();
                }
                else
                {
                    if ( thecontext == null ) context.Dispose();
                    return  null;
                }
                //
                if (thecontext == null) context.Dispose();
                return bi;
            }


            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas wykreślania dłużnika " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                if (thecontext == null) context.Dispose();
                return null;
            }
        }

        public BIG_Import DeleteAllCases( LexEnaMeritumEntities thecontext = null)
        {
            LexEnaMeritumEntities context = null;

            try
            {

                BIG_Import bi = null;

                if (thecontext == null)
                    context = new LexEnaMeritumEntities();
                else
                    context = thecontext;

                BIG_Obligation bo = new BIG_Obligation();


                bi = new BIG_Import();
                bi.DataImportu = DateTime.Now;
                bi.Username = this.userName;
                bi.Status = 0; // dodano
                bi.StatOpis = "Nowa/Zaimportowana";
                bi.lBlad = 0;
                bi.Filename = "Usunięcie przez operatora";
                bi.lSuccess = 0;
                bi.lPoz = 0;
                bi.TypImp = 3; // usunięcie

                List<BIGvw_ObligationLastStatus> lstToDel = context.BIGvw_ObligationLastStatus.ToList();
                if (lstToDel != null)
                {
                    foreach (BIGvw_ObligationLastStatus bs in lstToDel)
                    {
                        BIG_Operacja bigoperacja = new BIG_Operacja();
                        bigoperacja.DataOperacji = DateTime.Now;
                        bi.BIG_Operacja.Add(bigoperacja);
                        bigoperacja.TypOperacji = 103; //delete
                        bigoperacja.StatOpis = "Wykreślenie sprawy";
                        bigoperacja.BIG_ObligationId = bs.BIG_ObligationId;
                        bigoperacja.BIG_DebtorId = bs.BIG_DebtorId;
                        bigoperacja.BIG_CaseId = bs.BIG_CaseId;
                        bi.BIG_Operacja.Add(bigoperacja);

                    }
                    context.BIG_Import.AddObject(bi);
                    context.SaveChanges();
                }
                else
                {
                    if (thecontext == null) context.Dispose();
                    return null;
                }
                //
                if (thecontext == null) context.Dispose();
                return bi;
            }


            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas wykreślania dłużnika " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                if (thecontext == null) context.Dispose();
                return null;
            }
        }

        public BIG_Import  DeleteObligation(int ObligationId, LexEnaMeritumEntities thecontext = null, BIG_Import bimp = null)
        {
            LexEnaMeritumEntities context = null;

            try
            {

                BIG_Import bi = null;

                if (thecontext == null)
                    context = new LexEnaMeritumEntities();
                else
                    context = thecontext;



          
                    BIG_Obligation bo = context.BIG_Obligation.Where(a => a.BIG_ObligationId == ObligationId).FirstOrDefault();
                    BIG_Debtor bdebt = context.BIG_Debtor.Where(a => a.BIG_CaseId == bo.BIG_CaseId).OrderByDescending(b => b.BIG_DebtorId).FirstOrDefault();

                if (bimp == null)
                {
                    bi = new BIG_Import();
                    bi.Filename = "Usunięcie przez operatora";
                }
                else
                    bi = bimp;
                    bi.DataImportu = DateTime.Now;
                    bi.Username = this.userName;
                    bi.Status = 0; // dodano
                    bi.StatOpis = "Nowa/Zaimportowana";
                    bi.lBlad = 0;
                 
                    bi.lSuccess = 0;
                    bi.lPoz = 0;
                    bi.TypImp = 13; // usunięcie
                            BIG_Operacja bigoperacja = new BIG_Operacja();
                            bigoperacja.DataOperacji = DateTime.Now;
                            bi.BIG_Operacja.Add(bigoperacja);
                            bigoperacja.TypOperacji = 3; //usuniecie zobowiązania
                            bigoperacja.StatOpis = "Wykreślenie zbowiązania";
                            bigoperacja.BIG_ObligationId = bo.BIG_ObligationId;
                            if (bdebt != null)
                                bigoperacja.BIG_DebtorId = bdebt.BIG_DebtorId; 
                            bigoperacja.BIG_CaseId = bo.BIG_CaseId;
                            bi.BIG_Operacja.Add(bigoperacja);
                        if (bi.EntityState == System.Data.EntityState.Detached)
                            context.BIG_Import.AddObject(bi);
                        context.SaveChanges();


                //
                if (thecontext == null) context.Dispose();
                return bi;
            }


            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas wykreślania zobowiązania" + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                if (thecontext == null) context.Dispose();
                return null;
            }
        }

        private bool compareDebtorData(BIG_Debtor dbt, BIGvw_ImportyDetail therow)
        {
            if (dbt.Address1L1.Replace(" ","") != therow.Address1L1.Replace(" ","")) return false;
            if (dbt.Address1L2.Replace(" ", "") != therow.Address1L2.Replace(" ", "")) return false;
            if (dbt.Firstname != therow.Firstname) return false;
            if (dbt.IDNumber != therow.IDNumber) return false;
            if (dbt.Name.Replace(" ", "") != therow.Name.Replace(" ", "")) return false;
            if (dbt.NrKlienta != therow.NrKlienta) return false;

            return true;

        }

        private bool compareDebtorData(BIG_Debtor dbt, BIG_Debtor therow)
        {
            if (dbt.Address1L1.Replace(" ", "") != therow.Address1L1.Replace(" ", "")) return false;
            if (dbt.Address1L2.Replace(" ", "") != therow.Address1L2.Replace(" ", "")) return false;
            if (dbt.Firstname != therow.Firstname) return false;
            if (dbt.IDNumber != therow.IDNumber) return false;
            if (dbt.Name.Replace(" ", "") != therow.Name.Replace(" ", "")) return false;
            if (dbt.NrKlienta != therow.NrKlienta) return false;
            
            return true;

        }

        private BIG_Debtor cloneDebtor(BIG_Debtor b)
        {
            BIG_Debtor d = new BIG_Debtor();
            d.Address1L1 = b.Address1L1;
            d.Address1L2 = b.Address1L2;
            d.Address2L1 = b.Address2L1;
            d.Address2L2 = b.Address2L2;
            d.DebtorType = b.DebtorType;
            d.Firstname = b.Firstname;
            d.IdentityNumberType = b.IdentityNumberType;
            d.IDNumber = b.IDNumber;
            d.Krs = b.Krs;
            d.Name = b.Name;
            d.NrKlienta = b.NrKlienta;
            d.NrKlientaFull = b.NrKlientaFull;
            d.Pesel = b.Pesel;
            d.Regon = b.Regon;
            d.Secondname = b.Secondname;
            return d;

        }

        private BIG_Case cloneCase(BIG_Case c)
        {
            BIG_Case b = new BIG_Case();
            b.CaseBIGId = c.CaseBIGId;
            b.CaseId = c.CaseId;
            b.IdWienaSpr = c.IdWienaSpr;
            b.SrcSystem = c.SrcSystem;

            return b;
        }

        private BIG_Debtor updateDebtor(BIG_Debtor b, BIGvw_ImportyDetail therow)
        {
            b.Address1L1 = therow.Address1L1;
            b.Address1L2 = therow.Address1L2;
            b.Firstname = therow.Firstname;
            b.IDNumber = therow.IDNumber;
            b.Name = therow.Name;
            b.NrKlienta = therow.NrKlienta;
            return b;

        }

        private BIG_Obligation obligFormRow(BIGvw_ObligationLastStatus therow)
        {
            BIG_Obligation bob = new BIG_Obligation();
            bob.DataWymag = therow.DataWymag;
            bob.Saldo = therow.Saldo;
            bob.DataWysWezw = therow.DataWysWezw;
            bob.IdWienaNal = therow.IdwienaNal;
            //bob.Kwota = 
            bob.Saldo = therow.Saldo;
            bob.SystemId = therow.SystemId;
            bob.Title = therow.Title;
            bob.TypNal = therow.TypNal;
            bob.ObligationId = therow.ObligationId;
            return bob;


        }

        public BIG_Import UpdateObligation(BIGvw_ObligationLastStatus  bvData, Decimal newSaldo, DateTime dWymag, DateTime dWezw, String nrFaktury , LexEnaMeritumEntities thecontext = null, BIG_Import bimp = null)
        {

          

                BIG_Import bi = null;
                BIG_Debtor bdebtor= null;
                BIG_Case bcase = null;
                LexEnaMeritumEntities context = null;

            try
            {
                if (thecontext == null)
                    context = new LexEnaMeritumEntities();
                else
                    context = thecontext;


                BIG_Case bc = context.BIG_Case.Where(a => a.BIG_CaseId == bvData.BIG_CaseId).FirstOrDefault();

                BIG_Obligation bo = new BIG_Obligation();
                if (bimp == null)
                    bi = new BIG_Import();
                else
                    bi = bimp;


                bi.DataImportu = DateTime.Now;
                bi.Username = this.userName;
                bi.Status = 0; // dodano
                bi.StatOpis = "Nowa/Zaimportowana";
                bi.lBlad = 0;
                bi.Filename = "Aktualizacja przez operatora";
                bi.lSuccess = 0;
                bi.lPoz = 0;
                bi.TypImp = 12; // aktualizacja przez operatora

                // aktualizacja dłużnika
                // BIG
                // sprawdzenie czy zmieniły sie dane  ososbowe


                BIG_Obligation boblig = obligFormRow(bvData);
                boblig.Saldo = newSaldo;
                boblig.DataWymag = dWymag;
                boblig.DataWysWezw = dWezw;
                boblig.Title = nrFaktury;
                
                bc.BIG_Obligation.Add(boblig);
                BIG_Operacja bigoperacja = new BIG_Operacja();
                bigoperacja.DataOperacji = DateTime.Now;
                bi.BIG_Operacja.Add(bigoperacja);
                bigoperacja.TypOperacji = 2; //aktualizacja
                bigoperacja.StatOpis = "Aktualizacja należności";

                bigoperacja.BIG_DebtorId = bvData.BIG_DebtorId;
                bigoperacja.BIG_CaseId = bvData.BIG_CaseId;

                boblig.BIG_Operacja.Add(bigoperacja);
                // 
                // pozostawiene bez zmian pozostałych 
                if (bimp == null) // jeśli operacja z ręki;
                {
                    List<BIGvw_ObligationLastStatus> lstToUpdate = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_CaseId == bvData.BIG_CaseId && a.BIG_ObligationId != bvData.BIG_ObligationId).ToList();
                    if (lstToUpdate != null)
                    {
                        foreach (BIGvw_ObligationLastStatus bs in lstToUpdate)
                        {
                            BIG_Operacja bigo = new BIG_Operacja();
                            bigo.DataOperacji = DateTime.Now;
                            bi.BIG_Operacja.Add(bigo);
                            bigo.TypOperacji = 2; //add
                            bigo.StatOpis = "Aktualizacja należności w sprawie";
                            bigo.BIG_ObligationId = bs.BIG_ObligationId;
                            bigo.BIG_DebtorId = bs.BIG_DebtorId;
                            bigo.BIG_CaseId = bs.BIG_CaseId;
                            bi.BIG_Operacja.Add(bigo);
                        }
                    }
                }
                if (bi.EntityState == System.Data.EntityState.Detached)
                    context.BIG_Import.AddObject(bi);
                context.SaveChanges();



                //
                if (thecontext == null) context.Dispose();
                return bi;
            }


            catch (Exception ex)
            {

                errCode = -500;

                errDescription = "błąd podczas aktualizacji  należności " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                if (thecontext == null) context.Dispose();
                return null;
            }
        }

        public bool UpdateDebtor(BIGvw_DluznicyAktual bvData, String Name, String Imie, String IdNumber, String Pesel, String Adres1, String Adres2, String NrKlienta, int SrcSystem)
        {

            try
            {

                BIG_Import bi = null;
                BIG_Debtor bdebtor = null;
                BIG_Case bcase = null;
                bool caseDeleted = false;
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {

                    BIG_Case bc = context.BIG_Case.Where(a => a.BIG_CaseId == bvData.BIG_CaseId).FirstOrDefault();
                    if (bc == null)
                    {
                        this.errDescription = " Brak sprawy ";
                        return false;
                    }
                    List<BIGvw_ObligationLastStatus> lstToUpdate = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_CaseId == bvData.BIG_CaseId).ToList();

                    BIG_Debtor bd = context.BIG_Debtor.Where(a => a.BIG_CaseId == bvData.BIG_CaseId).FirstOrDefault();

              

                    BIG_Debtor bdNew = cloneDebtor(bd);
                    bdNew.Address1L1 = Adres1;
                    bdNew.Address1L2 = Adres2;
                    bdNew.Firstname = Imie;
                    bdNew.Name = Name;
                    bdNew.NrKlienta = NrKlienta;
                    bdNew.Pesel = Pesel;
                    bdNew.IDNumber = IdNumber;
                 
             

                    if (bc.CaseId != Utils.combineCaseId(SrcSystem, NrKlienta, IdNumber))
                    {
                        bi = DeleteCase(bc.BIG_CaseId,  context);
                        bc = cloneCase(bc);
                        bc.CaseId = Utils.combineCaseId(SrcSystem, NrKlienta, IdNumber);
                        bc.SrcSystem = SrcSystem;
                    
                        bc.BIG_Debtor.Add(bdNew);
                        bi.TypImp = 12;
                        bi.StatOpis = "Nowa/Zaimportowana";
                        bi.lBlad = 0;
                        bi.Filename = "Aktualizacja przez operatora - zmiana identyfiaktora";
                        caseDeleted = true;
                    }
                    else
                    {
                        bi = new BIG_Import();
                        bi.DataImportu = DateTime.Now;
                        bi.Username = this.userName;
                        bi.Status = 0; // dodano
                        bi.StatOpis = "Nowa/Zaimportowana";
                        bi.lBlad = 0;
                        bi.Filename = "Aktualizacja przez operatora";
                        bi.lSuccess = 0;
                        bi.lPoz = 0;
                        bi.TypImp = 12; // aktualizacja przez operatora

                        bc.BIG_Debtor.Add(bdNew);
                                           }
                        if (lstToUpdate != null)
                        {
                            foreach (BIGvw_ObligationLastStatus bs in lstToUpdate)
                            {
                                BIG_Operacja bigo = new BIG_Operacja();
                                bigo.DataOperacji = DateTime.Now;
                                bdNew.BIG_Operacja.Add(bigo);
                                bc.BIG_Operacja.Add(bigo);
                                bi.BIG_Operacja.Add(bigo);
                            if (caseDeleted)
                            {
                                bigo.TypOperacji = 1; // add
                                BIG_Obligation bigoblig = obligFormRow(bs);
                                bigoblig.BIG_Operacja.Add(bigo);
                                bc.BIG_Obligation.Add(bigoblig);

                                
                            }
                            else
                            {
                                bigo.TypOperacji = 2; //update
                                bigo.BIG_ObligationId = bs.BIG_ObligationId;

                            }
                                
                            }
                        }
                    if (!caseDeleted)
                        context.BIG_Import.AddObject(bi);
                    context.SaveChanges();
                }


                 
                    // 
                    // pozostawiene bez zmian pozostałych 
                   
                         return true;
            }


            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas aktualizacji salda należności " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return false;
            }
        }

        public bool suspendObligation(BIGvw_ObligationLastStatus bvData, DateTime suspendDate)
        {

            try
            {

                BIG_Import bi = null;
                BIG_Debtor bdebtor = null;
                BIG_Case bcase = null;
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {

                    BIG_Case bc = context.BIG_Case.Where(a => a.BIG_CaseId == bvData.BIG_CaseId).FirstOrDefault();

                    BIG_Obligation bo = new BIG_Obligation();


                    bi = new BIG_Import();
                    bi.DataImportu = DateTime.Now;
                    bi.Username = this.userName;
                    bi.Status = 0; // dodano
                    bi.StatOpis = "Nowa/Zaimportowana";
                    bi.lBlad = 0;
                    bi.Filename = "Zawieszenie  zobowiązania przez operatora";
                    bi.lSuccess = 0;
                    bi.lPoz = 0;
                    bi.TypImp = 15; // zawieszenie

                    // aktualizacja dłużnika
                    // BIG
                    // sprawdzenie czy zmieniły sie dane  ososbowe


                  
                    BIG_Operacja bigoperacja = new BIG_Operacja();
                    bigoperacja.DataOperacji = DateTime.Now;
                    bigoperacja.SuspendDate = suspendDate;
                    bi.BIG_Operacja.Add(bigoperacja);
                    bigoperacja.TypOperacji = 5; //zawieszenie
                    bigoperacja.StatOpis = "Zawieszenie należności";
                    bigoperacja.BIG_ObligationId = bvData.BIG_ObligationId;
                    bigoperacja.BIG_DebtorId = bvData.BIG_DebtorId;
                    bigoperacja.BIG_CaseId = bvData.BIG_CaseId;
                    context.BIG_Import.AddObject(bi);
                    context.SaveChanges();
                }


                //

                return true;
            }


            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas aktualizacji salda należności " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return false;
            }
        }

        public bool suspendCase(BIGvw_DluznicyAktual bvData, DateTime suspendDate, bool autoSuspend = false, LexEnaMeritumEntities thecontext = null, BIG_Import biIn = null)
        {
            LexEnaMeritumEntities context = null;

            try
            {

                BIG_Import bi = null;
                BIG_Debtor bdebtor = null;
                BIG_Case bcase = null;

                if (biIn != null) bi = biIn;

                if (thecontext != null)
                    context = thecontext;
                else
                    context = new LexEnaMeritumEntities();

           
                {

                    BIG_Case bc = context.BIG_Case.Where(a => a.BIG_CaseId == bvData.BIG_CaseId).FirstOrDefault();
                    List<BIGvw_ObligationLastStatus> bol = context.BIGvw_ObligationLastStatus.Where(b => b.BIG_CaseId == bvData.BIG_CaseId).ToList();
                    if (bol == null || !bol.Any())
                    {
                        if (thecontext == null) context.Dispose();
                        return false;
                    }
                    if (biIn == null)
                    {
                        bi = new BIG_Import();
                        bi.DataImportu = DateTime.Now;
                        bi.Username = this.userName;
                        bi.Status = 0; // dodano
                        bi.StatOpis = "Nowa/Zaimportowana";
                        bi.lBlad = 0;
                        bi.Filename = "Zawieszenie sprawy przez operatora";
                        bi.lSuccess = 0;
                        bi.lPoz = 0;
                        bi.TypImp = 15; // zawieszenie
                    }
                    // aktualizacja dłużnika
                    // BIG
                    // sprawdzenie czy zmieniły sie dane  ososbowe
                    foreach (BIGvw_ObligationLastStatus bio in bol)
                    {
                        BIG_Operacja bigoperacja = new BIG_Operacja();
                        bigoperacja.DataOperacji = DateTime.Now;
                        bigoperacja.SuspendDate = suspendDate;
                        bigoperacja.BIG_CaseId = bio.BIG_CaseId;
                        bigoperacja.BIG_DebtorId = bio.BIG_DebtorId;
                        bigoperacja.TypOperacji = 5; //zawieszenie
                        bigoperacja.StatOpis = "Zawieszenie sprawy";
                        bigoperacja.BIG_ObligationId = bio.BIG_ObligationId;
                        BIG_Obligation boblig = context.BIG_Obligation.Where(a => a.BIG_ObligationId == bio.BIG_ObligationId).FirstOrDefault();
                        if (boblig != null)
                        {
                            boblig.SuspendDate = suspendDate;
                            boblig.AutoSuspend = autoSuspend;
                        }
                        bi.BIG_Operacja.Add(bigoperacja);
                        
                    }
                    if ( bi.EntityState == System.Data.EntityState.Deleted)
                    context.BIG_Import.AddObject(bi);
                    context.SaveChanges();
                }


                //
                if (thecontext == null) context.Dispose();
                return true;
            }


            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas aktualizacji salda należności " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                if (thecontext == null) context.Dispose();
                return false;
            }
        }


        public bool sendNotifications(List<BIGvw_DluznicyAktual> lst)
        {

            try
            {

                BIG_Import bi = null;
                BIG_Debtor bdebtor = null;
                BIG_Case bcase = null;
                if (lst == null || !lst.Any())
                    return false;

                bi = new BIG_Import();
                bi.DataImportu = DateTime.Now;
                bi.Username = this.userName;
                bi.Status = 0; // dodano
                bi.StatOpis = "Nowa/Zaimportowana";
                bi.lBlad = 0;
                bi.Filename = "Zlecenie wysyłki powiadomień";
                bi.lSuccess = 0;
                bi.lPoz = 0;
                bi.TypImp = 17; // zawieszenie

                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    foreach(BIGvw_DluznicyAktual bvData in lst )
                    { 
                    BIG_Case bc = context.BIG_Case.Where(a => a.BIG_CaseId == bvData.BIG_CaseId).FirstOrDefault();
                    List<BIGvw_ObligationLastStatus> bol = context.BIGvw_ObligationLastStatus.Where(b => b.BIG_CaseId == bvData.BIG_CaseId).ToList();
                    if (bol == null || !bol.Any())
                        return false;
                        bc.NotifyDate = DateTime.Now;
                        // aktualizacja dłużnika
                        // BIG
                        // sprawdzenie czy zmieniły sie dane  ososbowe
                        foreach (BIGvw_ObligationLastStatus bio in bol)
                        {
                            BIG_Operacja bigoperacja = new BIG_Operacja();
                            bigoperacja.DataOperacji = DateTime.Now;
                            bigoperacja.BIG_CaseId = bio.BIG_CaseId;
                            bigoperacja.BIG_DebtorId = bio.BIG_DebtorId;
                            bigoperacja.TypOperacji = 17; //zawieszenie
                            bigoperacja.StatOpis = "Wysłanie powiadomienia";
                            bigoperacja.BIG_ObligationId = bio.BIG_ObligationId;
                            bigoperacja.NotifyDate = DateTime.Today;
                            BIG_Obligation boblig = context.BIG_Obligation.Where(a => a.BIG_ObligationId == bio.BIG_ObligationId).FirstOrDefault();
                            if (boblig != null)
                                boblig.NotifyDate = DateTime.Today;
                            bi.BIG_Operacja.Add(bigoperacja);
                        }
                    }
                    if (bi.EntityState == System.Data.EntityState.Detached)
                    context.BIG_Import.AddObject(bi);
                    context.SaveChanges();
                }


                //

                return true;
            }


            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas aktualizacji salda należności " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return false;
            }
        }


        public bool unSuspendCase(BIGvw_DluznicyAktual bvData)
        {

            try
            {

                BIG_Import bi = null;
                BIG_Debtor bdebtor = null;
                BIG_Case bcase = null;
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {

                    BIG_Case bc = context.BIG_Case.Where(a => a.BIG_CaseId == bvData.BIG_CaseId).FirstOrDefault();
                    List<BIGvw_ObligationLastStatus> bol = context.BIGvw_ObligationLastStatus.Where(b => b.BIG_CaseId == bvData.BIG_CaseId).ToList();
                    if (bol == null || !bol.Any())
                        return false;

                    bi = new BIG_Import();
                    bi.DataImportu = DateTime.Now;
                    bi.Username = this.userName;
                    bi.Status = 0; // dodano
                    bi.StatOpis = "Nowa/Zaimportowana";
                    bi.lBlad = 0;
                    bi.Filename = "Podjęcie zawieszonej sprawy zawieszonej sprawy przez operatora";
                    bi.lSuccess = 0;
                    bi.lPoz = 0;
                    bi.TypImp = 15; // zawieszenie

                    // aktualizacja dłużnika
                    // BIG
                    // sprawdzenie czy zmieniły sie dane  ososbowe
                    foreach (BIGvw_ObligationLastStatus bio in bol)
                    {
                        BIG_Operacja bigoperacja = new BIG_Operacja();
                        bigoperacja.DataOperacji = DateTime.Now;
                        bigoperacja.BIG_CaseId = bio.BIG_CaseId;
                        bigoperacja.BIG_DebtorId = bio.BIG_DebtorId;
                        bigoperacja.TypOperacji = 6; //podjęcie ( zwnowienie
                        bigoperacja.StatOpis = "Zawieszenie sprawy";
                        bigoperacja.BIG_ObligationId = bio.BIG_ObligationId;
                        BIG_Obligation boblig = context.BIG_Obligation.Where(a => a.BIG_ObligationId == bio.BIG_ObligationId).FirstOrDefault();
                        if (boblig != null)
                            boblig.SuspendDate = null;

                        bi.BIG_Operacja.Add(bigoperacja);

                    }
                    context.BIG_Import.AddObject(bi);
                    context.SaveChanges();
                }


                //

                return true;
            }


            catch (Exception ex)
            {

                errCode = -500;
                errDescription = "błąd podczas aktualizacji salda należności " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException : "");
                return false;
            }
        }

        public bool importWienaNews()
        {
            int idPakiet;
            List<BIG_JobRow> lst;
            List<ImportRow> outLst;
            List<ImportRow> inLst;

            using (LexEnaMeritumEntities dbcontext = new LexEnaMeritumEntities())
            {
                 dbcontext.CommandTimeout = 1200;
                 idPakiet = dbcontext.ExecuteStoreQuery<int>("USP_ImportWienaNewKRD").FirstOrDefault();
                 lst  = dbcontext.BIG_JobRow.Where(a => a.BIG_JobId == idPakiet).ToList();
                
            }

            inLst = BigJob2Import(lst);
            this.badRows = new List<ImportRow>();
            outLst = filterList(inLst, ref badRows, 0);

           

            if (outLst == null || !outLst.Any())
                return false;

            int code = saveJob(outLst);
            if (this.badRows != null & badRows.Any())
            {
                saveJob(badRows, true);

            }
          
                try
                {
                   
                    using (LexEnaMeritumEntities dbcontext = new LexEnaMeritumEntities())
                    {
                        dbcontext.CommandTimeout = 1200;
                        SqlParameter param1 = new SqlParameter("IdJob", code);
                        SqlParameter param2 = new SqlParameter("UserName", userName);
                        SqlParameter param3 = new SqlParameter("context",11); // dla Wieny powinno być 11
                        object obj = dbcontext.ExecuteStoreQuery<object>("USP_BIGImportSelAUMS  @IdJob, @UserName, @context ", param1, param2, param3).FirstOrDefault();

                        return true;
                    }


                }
            catch (ArgumentException ex)
            {
                errDescription = "Błąd " + ex.Message + " " + ex.InnerException;
                return false;
            }


        }


        public bool importWiena(DateTime dstart, int offset)
        {
          //  if (this.callContext == 11) // EOP
            return  importWienaNews();
            /*
            else
            {
                List<BIGvw_ObligationLastStatus> sprToDelLst = new List<BIGvw_ObligationLastStatus>();
                List<BIGvw_ObligationLastStatus> sprToMoveLst = new List<BIGvw_ObligationLastStatus>();
                //List<BIGvw_ImportWiena2BIG> nalWiena=null;
                List<BIGvw_ImportWiena2BIG> nalWiena = null;
                List<BIGvw_ImportWiena2BIG> searchRes;
                List<ImportRow> outLst = null;
                try
                {
                    BIG_Import bimport = new BIG_Import();
                    bimport.Filename = "Aktualizacja Wiena";
                    using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                    {
                        DateTime dBorder = dstart.AddDays(-offset - 1);  // 
                        DateTime dEnd = DateTime.Today.AddDays(1);
                        // koniec bieżącego roku
                        List<BIGvw_ObligationLastStatus> bol = context.BIGvw_ObligationLastStatus.Where(a => a.AutoSuspend == true && a.SrcSystem < 10).ToList();   // sprawy zawieszone z systemów billingowych 
                        try
                        {
                            nalWiena = context.BIGvw_ImportWiena2BIG.Where(a => a.data_r >= dBorder && a.data_r <= dEnd).ToList();
                            //  nalWiena = qry.ToList();
                            //  nalWiena = context.BIGvw_ImportWiena2BIG.Where(a => a.data_r >= dBorder && a.data_r <= dEnd).ToList();
                        }
                        catch (Exception ex)
                        {
                            ;
                        }
                        // weryfikacja w wienie 
                        foreach (BIGvw_ObligationLastStatus b in bol)
                        {
                            if (b.SuspendDate == null) continue;
                            if (nalWiena != null || !nalWiena.Any())

                            {
                                if (b.SuspendDate.Value.AddDays(offset) < DateTime.Today)
                                {
                                    sprToDelLst.Add(b);
                                    continue;
                                }

                            }
                            searchRes = null;
                            // sprawdzamy czy jest w Wienie.
                            try
                            {
                                searchRes = nalWiena.Where(a => a.nr_ewid == b.NrKlienta && a.tytul == b.Title && a.data_n == b.DataWymag).ToList();
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                            if (searchRes != null && searchRes.Any())
                            {
                                sprToMoveLst.Add(b);
                            }
                            else
                              if (b.SuspendDate.Value.AddDays(offset) > DateTime.Today)
                                sprToDelLst.Add(b);

                        }

                        if (sprToDelLst != null && sprToDelLst.Any())
                        {// wykreślenie należności 
                            foreach (BIGvw_ObligationLastStatus d in sprToDelLst)
                            {


                                if (DeleteObligation(d.BIG_ObligationId, context, bimport) == null)
                                {
                                    return false;
                                }

                            }


                        }
                        // przepisanie należności 
                        if (sprToMoveLst != null && sprToMoveLst.Any())
                        {
                            outLst = new List<ImportRow>();
                            foreach (BIGvw_ObligationLastStatus s in sprToMoveLst)
                            {
                                ImportRow r = rowFromDB(s, nalWiena, context);
                                if (DeleteObligation(s.BIG_ObligationId, context, bimport) == null)
                                {
                                    return false;
                                }
                                if (r != null)
                                    outLst.Add(r);




                            }
                            if (outLst.Any())
                                bimport = ProceedStruct(outLst, 2, bimport, context);
                        }
                        // teraz procedowanie weryfikacja istniejacych sald w Wienie
                        List<BIGvw_ObligationLastStatus> bolW = context.BIGvw_ObligationLastStatus.Where(a => a.SrcSystem == 11).ToList();
                        List<BIGvw_ObligationLastStatus> emptyUpdate = new List<BIGvw_ObligationLastStatus>();
                        List<int> updatedCaseIds = new List<int>();
                        Utils.LogWriter("Weryfikacja istniejących sald w systemie");
                        if (bolW != null)
                        {
                            bool found = false;
                            foreach (BIGvw_ObligationLastStatus ob in bolW)
                            {
                                BIGvw_ImportWiena2BIG nal = null;
                                found = false;

                                if (sprToMoveLst != null && sprToMoveLst.Any())
                                {
                                    if (sprToMoveLst.Contains(ob)) continue;
                                }

                                if (ob.IdwienaNal > 0)
                                {
                                    nal = context.BIGvw_ImportWiena2BIG.Where(a => a.idNal == ob.IdwienaNal).FirstOrDefault();

                                    if (nal != null)
                                    {

                                        if (nal.tytul == ob.Title)
                                        {
                                            if (nal.kwota - nal.splata > 0 && nal.kwota - nal.splata != ob.Saldo)
                                            {

                                                this.UpdateObligation(ob, nal.kwota.Value - nal.splata, nal.data_n.Value, ob.DataWysWezw.Value, ob.Title, context, bimport);
                                                updatedCaseIds.Add(ob.BIG_CaseId);
                                                found = true;
                                            }
                                            else
                                                if (nal.kwota - nal.splata <= 0)
                                            {

                                                this.DeleteObligation(ob.BIG_ObligationId, context, bimport);
                                                //updatedCaseIds.Add(ob.BIG_CaseId);
                                                found = true;
                                            }

                                        }
                                    }
                                }

                                if (!found)
                                {

                                    emptyUpdate.Add(ob);
                                }

                            }

                            if (emptyUpdate.Any() && updatedCaseIds.Any())
                            {
                                foreach (BIGvw_ObligationLastStatus bs in emptyUpdate)
                                {
                                    if (updatedCaseIds.Contains(bs.BIG_CaseId))
                                    {

                                        BIG_Operacja bigo = new BIG_Operacja();
                                        bigo.DataOperacji = DateTime.Now;
                                        bimport.BIG_Operacja.Add(bigo);
                                        bigo.TypOperacji = 2; //add
                                        bigo.StatOpis = "Aktualizacja należności w sprawie";
                                        bigo.BIG_ObligationId = bs.BIG_ObligationId;
                                        bigo.BIG_DebtorId = bs.BIG_DebtorId;
                                        bigo.BIG_CaseId = bs.BIG_CaseId;
                                        bimport.BIG_Operacja.Add(bigo);
                                    }
                                }

                            }
                            if (bimport.EntityState == System.Data.EntityState.Detached && bimport.BIG_Operacja != null && bimport.BIG_Operacja.Any())
                            {

                                context.BIG_Import.AddObject(bimport);

                            }

                            context.SaveChanges();
                        }


                    }


                    return true;
                }
                catch (Exception ee)
                {
                    Utils.LogWriter("Błąd " + ee.Message);
                    return false;

                }
            }
            */
        }
        private ImportRow rowFromDB(BIGvw_ObligationLastStatus db, List<BIGvw_ImportWiena2BIG> nalWiena , LexEnaMeritumEntities context)
        {
            ImportRow imr = new ImportRow();
            BIGvw_ImportWiena2BIG nalW;
            BIG_Obligation bob = context.BIG_Obligation.Where(a => a.BIG_ObligationId == db.BIG_ObligationId).FirstOrDefault();
            BIG_Case bc = context.BIG_Case.Where(a => a.BIG_CaseId == db.BIG_CaseId).FirstOrDefault();
            BIG_Debtor bd = context.BIG_Debtor.Where(a => a.BIG_DebtorId == db.BIG_DebtorId).FirstOrDefault();
            if (bob == null) return null;

            nalW = nalWiena.Where(a => a.nr_ewid == db.NrKlienta && a.tytul == bob.Title && a.data_n == bob.DataWymag).FirstOrDefault();
            if (nalW == null) return null;
            
                if (nalW.kwota <= nalW.splata)
                    return null; // już spłacona
            imr.SystemName = "WIENA";
            imr.NrKlienta = bd.NrKlienta;
            imr.Saldo = (nalW.kwota - nalW.splata).ToString();
            imr.Title = bob.Title;
            imr.DataWymag = bob.DataWymag.Value.ToString("yyyy-MM-dd");
            imr.DataWysWezw = bob.DataWysWezw.Value.ToString("yyyy-MM-dd");
            imr.TypNal = nalW.typ_nal;
            imr.IdSprawaWiena = nalW.idSprawy;
            imr.IdNal = nalW.idNal;
            imr.Adr1 = bd.Address1L1;
            imr.Adr2 = bd.Address1L2;
            imr.Name = bd.Name;
            imr.Firstname = bd.Firstname;
            imr.Secondname = bd.Secondname;
            imr.Pesel = bd.Pesel;
            
            if (bd.DebtorType == 1) // osoba prawna - musi być NIP
                imr.Nip = bd.IDNumber;
            else
                imr.Pesel = bd.IDNumber;
            imr.WienaSygn = nalW.sygnatura;
            return imr;

        }

    }
}