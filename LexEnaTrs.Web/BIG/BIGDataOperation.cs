using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nicci.Input;
using System.Text.RegularExpressions;

namespace LexEnaTrs.Web.BIG
{

    public class removeInformationTypeEx
    {
        public int idOper { get; set; }

        public removeInformationType reInfType { get; set; }


    }


    public class suspendInformationTypeEx
    {
        public int idOper { get; set; }

        public suspendType susInfType { get; set; }


    }

    public class unSuspendInformationTypeEx
    {
        public int idOper { get; set; }

        public entityManagementWithVerificationType unSusInfType { get; set; }


    }

    public class notifyInformationTypeEx
    {
        public int idOper { get; set; }

        public notifyDebtorOrderType notifyInfType { get; set; }


    }

    public class addObligationInformationTypeEx
    {
        public int idOper { get; set; }

        public addObligationInformationType addOblig { get; set; }


    }


    public class BIGDataOperation
    {
        private string errcode = null;

        public string getExceptionMessage()
        {

            return errcode;
        }

        public List<Guid?> getUnconfirmedJobs(int IdPakiet = 0)
        {
            List<Guid?> lst ;



            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {

                if (IdPakiet == 0)
                    lst = context.BIG_Import.Where(a => a.Status > 0 && a.Status < 10).Select(b => b.JobId).ToList();
                else
                    lst = context.BIG_Import.Where(a => a.Status > 0 && a.Status < 10 && a.BIG_ImportId == IdPakiet).Select(b => b.JobId).ToList();
                
                return lst;

            }

        }
        public bool addJobID(Guid? JobId, int ImportId)
        {
            try
            {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    BIG_Import bi = context.BIG_Import.Where(c => c.BIG_ImportId == ImportId).FirstOrDefault();
                    if (bi != null)
                    {
                        bi.JobId = JobId;
                        bi.DataWysylki = DateTime.Now;
                        bi.Status = 1; // wysłana
                        bi.StatOpis = "Wysłany";
                        context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errcode = Utils.exceptionMsg(ex);
                return false;

            }

        }

        public bool updateImportStatus(int status, Guid idJob)
        {

            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {
                BIG_Import bimp = context.BIG_Import.Where(a => a.JobId == idJob).FirstOrDefault();
                if (bimp != null)
                {
                    switch (status)
                    {
                        case 0: // cancelled
                            bimp.Status = 0;
                            bimp.StatOpis = "Anulowany";
                            break;
                        case 1:
                            bimp.Status = 0;
                            bimp.StatOpis = "Błąd techniczny KRD";
                            break;
                        case 2:
                            break;
                        case 3:
                            bimp.Status = 3;
                            bimp.StatOpis = "Oczekuje w kolejce";
                            break;
                        case 4:
                            bimp.Status = 4;
                            bimp.StatOpis = "W trakcie przetwarzania";
                            break;
                    }
                    context.SaveChanges();
                }

            }
                        return true;
        }

        private bool updateOperStatus(Nicci.Output.OutputInformationManagement omsg , BIG_Import import,DateTime procDate ,LexEnaMeritumEntities context)
        {
            Nicci.Output.OutputInformationManagementOrder[] oty;
            bool hasError = false;
            oty = omsg.Order;
            if (oty == null) return false;
            //List<BIG_Operacja> oprLst = context.BIG_Operacja.Where(a => a.BIG_ImportId == import.BIG_ImportId).ToList();
           // if (oprLst == null) return false;
            
            foreach (Nicci.Output.OutputInformationManagementOrder ord in oty)
            {
                int orderNo = Convert.ToInt32(ord.ID);
                List<BIG_Operacja> oprLst1 = context.BIG_Operacja.Where(a => a.BIG_ImportId == import.BIG_ImportId &&  a.OrderOp == orderNo).ToList();

                switch (ord.ItemElementName.ToString().ToUpper())
                {
                    case "UNSUSPENDINFORMATION":
                    case "SUSPENDINFORMATION":
                    case "REMOVEINFORMATION":
                    case "UPDATEINFORMATION":
                    case "ADDINFORMATION":
                    case "SENDNOTIFICATION":
                        
                        if (ord.status == Nicci.Output.responseStatusEnum.Success)
                        {
                            if (oprLst1 != null)
                            {
                                foreach (BIG_Operacja oo in oprLst1)
                                {
                                    oo.StatusOperacji = 1;
                                    oo.DataProcedowaniaKrd = procDate;
                                    oo.StatOpis += "["+ ord.ID + "]";
                                }
                                context.SaveChanges();
                            }

                        }
                        else if (ord.status == Nicci.Output.responseStatusEnum.Fail)  // jeśłi błąd
                        {
                            hasError = true;
                            Nicci.Output.informationResponseType infresp = ord.Item as Nicci.Output.informationResponseType;
                            if (infresp.informationType == Nicci.Output.informationTypeEnum.Case)
                            {
                                Nicci.Output.errorType err = null;
                                if (infresp.Error != null)
                                    err =  infresp.Error;

                                if (oprLst1 != null)
                                {



                                    foreach (BIG_Operacja oo in oprLst1)
                                    {
                                        oo.StatusOperacji = -1;
                                        oo.DataProcedowaniaKrd = procDate;
                                        oo.StatOpis += "Błąd sprawy " + (err != null ? err.Value + (err.codeSpecified ? " (" + err.code.ToString() + ")" : "") : "");
                                    }
                                    context.SaveChanges();
                                }
                            }
                            else if (infresp.informationType == Nicci.Output.informationTypeEnum.ObligationInformation)
                            {
                                string oblNo = infresp.ID;

                                //List<BIG_Operacja> oprLst2 = oprLst.Where(c => c.BIG_Obligation.ObligationId == oblNo).ToList();
                                List<BIG_Operacja> oprLst2 = (from o in context.BIG_Obligation join oper in context.BIG_Operacja on o.BIG_ObligationId equals oper.BIG_ObligationId where oper.BIG_ImportId == import.BIG_ImportId && o.ObligationId == oblNo select oper).ToList();
                                if (oprLst2 != null && oprLst2.Any())
                                {
                                    Nicci.Output.errorType err = null;
                                    if (infresp.Error != null)
                                        err = infresp.Error;

                                    foreach (BIG_Operacja oo in oprLst2)
                                    {
                                        oo.StatusOperacji = -1;
                                        oo.DataProcedowaniaKrd = procDate;
                                        oo.StatOpis += "Błąd zobowiązania " + (err != null ? err.Value + (err.codeSpecified ? " (" + err.code.ToString() + ")" : "") : "");
                                    }

                                    context.SaveChanges();

                                }

                            }
                        }


                            break;
                    
                    
                    
                   
                    default:
                        {

                            errcode = "Nieznany rodzaj komunikatu " + ord.ItemElementName.ToString();
                            return false;
                        }

                }
                
                
            }
            if (hasError)
            {
                import.Status = 199;
                import.StatOpis = "Przetworzony ale zawierał błąd";
            }
            else
            {
                import.Status = 200; // PROCEEDED - ok
                import.StatOpis = "Przetworzony";
            }
            context.SaveChanges();
            return true;
        }

        private bool fatalErrorStatus(Nicci.Output.errorType omsg, BIG_Import import, DateTime procDate, LexEnaMeritumEntities context)
        {

            import.Status = -1;
            import.StatOpis = omsg.Value + (omsg.codeSpecified ? " ("+omsg.code.ToString()+")":"");
            if (import.StatOpis.Length > 8000)
                import.StatOpis = import.StatOpis.Substring(0, 8000);


            List<BIG_Operacja> oprLst = context.BIG_Operacja.Where(a => a.BIG_ImportId == import.BIG_ImportId).ToList();
            foreach (BIG_Operacja oo in oprLst)
                                {
                                    oo.StatusOperacji = -1;
                                    oo.StatOpis = string.Empty;
                                }

                            
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processReport"></param>
        /// <param name="importId"></param>
        /// <returns></returns>
        public bool updateCaseStatus(string processReport, Guid idJob)
       {

            if (String.IsNullOrWhiteSpace(processReport))
                return false;
            try
            {
                Nicci.Output.Output outmsg;
                outmsg = (Nicci.Output.Output)Utils.DeserializeFromString(processReport, typeof(Nicci.Output.Output));
                if (outmsg != null)
                {
                    DateTime dprocessed;
                    if (outmsg.processed > new DateTime(2015, 1, 1))
                        dprocessed = outmsg.processed;
                    else
                        dprocessed = outmsg.started;

                    object[] infoTab = outmsg.Items;
                    
                    using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                    {
                        BIG_Import bimp = context.BIG_Import.Where(a => a.JobId  == idJob).FirstOrDefault();
                        bimp.lBlad = outmsg.failCount;
                        bimp.lSuccess = outmsg.successCount;
                        foreach (var o in infoTab)
                        {

                            if (o.GetType() == typeof(Nicci.Output.OutputInformationManagement))
                            {
                                updateOperStatus(o as Nicci.Output.OutputInformationManagement, bimp, dprocessed, context) ;
                            }
                            else if (o.GetType() == typeof(Nicci.Output.errorType))
                            {
                                // Fatal error  - błąd na poziomie pakietu
                                fatalErrorStatus( o as Nicci.Output.errorType, bimp, dprocessed, context);

                            }
                            else if (o.GetType() == typeof(Nicci.Output.OutputMonitorConditionManagement))
                            {

                                ;
                            }
                            else if (o.GetType() == typeof(Nicci.Output.OutputSearchManagement))
                            {

                                ;
                            }


                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                this.errcode = Utils.exceptionMsg(ex);
                return false;
            }
        }

        private string  validateRow(BIG_Obligation bob ,        BIG_Debtor deb )
        {
            string ans = "";
            if (bob.Saldo <= 0) ans += "Saldo < 0 ";
            if (bob.DataWysWezw == null) ans += "Błędna data wys wezwania  ";
            else
                if ((DateTime.Today - bob.DataWysWezw.Value).TotalDays <= 30 ) ans += "Błędna data wys wezwania ( brak 30 dni) ";

            if (bob.DataWymag == null) ans += "Błędna data wymagalności ";
            else
            {
                if (bob.DataWymag >= bob.DataWysWezw) ans += "Błędne daty   wymagalności późniejsza od wezwania";

                if ((DateTime.Today - bob.DataWymag.Value).TotalDays <= 30) ans += "Błędna data wymagalności ( brak 30 dni) ";

            }
            if (String.IsNullOrWhiteSpace(deb.Name)) ans += "Błędne nazwisko - nazwa - puste"; // osoba fizyczna

            if (!String.IsNullOrWhiteSpace(deb.Firstname)) // osoba fizyczna
            {
                if (string.IsNullOrWhiteSpace(deb.IDNumber)) ans += " Osoba fizyczna musi mieć Pesel";
                else
                if (!Utils.IsValidPesel(deb.IDNumber)) ans += " Błędny Pesel";

            }
            else
            {
                

                if (string.IsNullOrWhiteSpace(deb.IDNumber)) ans += " Osoba prawna musi mieć NIP";
                else
                if (!Utils.IsValidNIP(deb.IDNumber)) ans += " Błędny NIP";


            }
            // adres
            if (String.IsNullOrWhiteSpace(deb.Firstname))
              { 
                if (String.IsNullOrWhiteSpace(deb.Address1L1)) ans += " Błędny Adres ( ulica / dom )";
                if (String.IsNullOrWhiteSpace(deb.Address1L2)) ans += " Błędny Adres ( kod - miasto)";
            }

            if (String.IsNullOrWhiteSpace(deb.Firstname) || (!String.IsNullOrWhiteSpace(deb.Firstname) && !String.IsNullOrWhiteSpace(deb.Address1L1) ))
            { 
                Regex regex = new Regex(@"[0-9]{2}-[0-9]{3}");
                Match match = regex.Match(deb.Address1L2);
            if (!match.Success)
            {
                 ans += " Błędny Adres ( kod pocztowy)";
            }

            }
            return ans;

        }

        /// <summary>
        /// Import do 
        /// </summary>
        /// <param name="IdIput"></param>
        /// <returns></returns>
        public Input Import2KrdInputNew(int IdIput)
        {
            Input input = new Input();

            input.timeStampSpecified = true;
            input.timeStamp = DateTime.Now;
            input.generator = "Lexena/EOP";
            bool errFound = false;
            int lp = 0; // numeracja operacji w pakiecie
            List<addCaseType> addCaseLst = new List<addCaseType>();
            List<updateCaseType> updtCaseLst = new List<updateCaseType>();
            List<addObligationInformationTypeEx> addOblig = new List<addObligationInformationTypeEx>(); // lista tylko dodanych należności do istniejacych spraw.

            List<BIG_Case> case2DelLst = new List<BIG_Case>();
            List<removeInformationTypeEx> removeLst = new List<removeInformationTypeEx>();
            List<suspendInformationTypeEx> suspendLst = new List<suspendInformationTypeEx>();
            List<unSuspendInformationTypeEx> unSuspendLst = new List<unSuspendInformationTypeEx>();
            List<notifyInformationTypeEx> notifySprLst = new List<notifyInformationTypeEx>();


            informationManagementOrdersType1 item = new informationManagementOrdersType1();
            //item.Order = 
            informationManagementOrdersType1[] arr;


            informationManagementOrdersType1 infoTyp = new informationManagementOrdersType1();
            List<informationManagementOrderType> infoOrd = new List<informationManagementOrderType>();

            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {
                BIG_Import imp = context.BIG_Import.Where(a => a.BIG_ImportId == IdIput).FirstOrDefault();

                if (imp == null)
                {
                    errcode = "Brak pakietu o takim Id = " + IdIput.ToString();
                    return null;

                }
                input.fileName = imp.Filename;


                List<BIG_Operacja> oprLst = context.BIG_Operacja.Include("BIG_Obligation").Where(a => a.BIG_ImportId == imp.BIG_ImportId).OrderBy(a => a.BIG_CaseId).ThenBy(a => a.TypOperacji).ToList();


                Utils.LogWriter("Lista załadowana sztuk " + oprLst.Count.ToString());
                if (oprLst == null || !oprLst.Any())
                {
                    errcode = "Brak operacji dla podanego pakietu";
                    return null;

                }

                if (imp.TypImp == 3) // jeśli wykreślenie spraw.
                {

                    foreach (BIG_Operacja bop in oprLst)
                    {
                        if (!case2DelLst.Contains(bop.BIG_Obligation.BIG_Case))
                        {

                            case2DelLst.Add(bop.BIG_Obligation.BIG_Case);
                        }
                    }

                    Utils.LogWriter("Budowa pokietu do usunięcia");
                    if (case2DelLst.Any())
                    {



                        {
                            input.Item = infoTyp;

                            foreach (BIG_Case bc in case2DelLst)
                            {

                                informationManagementOrderType ord = new informationManagementOrderType();
                                ord.ID = (++lp).ToString();

                                removeInformationType delInf = new removeInformationType();
                                delInf.informationType = informationTypeEnum.Case;
                                delInf.removeReason = removeReasonType.Other;
                                delInf.IDType = idTypeEnum.UserId;
                                delInf.ID = bc.CaseId;
                                delInf.verifyResult = true;
                                delInf.verifyResultSpecified = true;
                                ord.Item = delInf;
                                infoOrd.Add(ord);
                                foreach (BIG_Operacja bop in oprLst.Where(a => a.BIG_Obligation.BIG_CaseId == bc.BIG_CaseId).ToList())
                                {
                                    bop.StatusOperacji = 0;
                                    bop.OrderOp = lp;
                                    bop.StatusOperacji = 0;
                                }
                                context.SaveChanges();
                            }

                            infoTyp.Order = infoOrd.ToArray();
                        }

                        return input;



                    }
                    else return null;

                }

                Utils.LogWriter("Procedowanie pakietu");
                int count = 0;
                string prevSprId = string.Empty;
                addCaseType addSpr = null;
                try {
                    foreach (BIG_Operacja bop in oprLst)
                    {
                        string sprId = bop.BIG_Obligation.BIG_Case.CaseId;
                        int idSprawy = bop.BIG_Obligation.BIG_CaseId.Value;

                        BIG_Obligation bob = bop.BIG_Obligation;
                        BIG_Case bca = bob.BIG_Case;
                        BIG_Debtor deb = bop.BIG_Debtor;
                        // czy sprawa już jest w Krd ?

                        ++count;

                        if (bop.TypOperacji == 101)
                        {
                            string validate = validateRow(bob, deb);
                            Utils.LogWriter("Validation");
                            if (!String.IsNullOrWhiteSpace(validate))
                            {
                                Utils.LogWriter("Blad walidacji " + validate);
                                bop.StatOpis = "Błąd walidacji" + " " + validate;
                                bop.StatusOperacji = -1;
                                errFound = true;
                                context.SaveChanges();
                            }

                        }
                        //       case2DelLst.Where(a=>a.CaseId == )
                        if (prevSprId != sprId) // nowa sprawa
                        {

                            if (bop.TypOperacji == 101)   // nowa sprawa
                            {
                                addSpr = new addCaseType();
                                addSpr.Note = bca.CaseId;
                                addSpr.ApplicationSourceName = Utils.sysNameFromId(bca.SrcSystem.Value);
                                addSpr.showProvider = true;
                                addCaseLst.Add(addSpr);
                                addSpr.Items = new object[5];
                                informationManagementOrderType ord = new informationManagementOrderType();
                                ord.ID = (++lp).ToString();

                                addInformationType adInf = new addInformationType();
                                adInf.verifyResult = true;
                                adInf.userID = bca.CaseId;
                                ord.Item = adInf;
                                adInf.Item = addSpr;
                                infoOrd.Add(ord);
                                BIG_Debtor bd = context.BIG_Debtor.Where(a => a.BIG_CaseId == idSprawy && a.BIG_DebtorId == bop.BIG_DebtorId).FirstOrDefault();
                                {  // debtor
                                    debtorType debt = new debtorType();
                                    addressType adr = new addressType();
                                    adr.Line = new string[2];
                                    adr.Line[0] = bd.Address1L1;
                                    adr.Line[1] = bd.Address1L2;

                                    if (bd.DebtorType == 0) // osoba fizyczna
                                    {
                                        consumerType osfiz = new consumerType();
                                        osfiz.FirstName = bd.Firstname;
                                        osfiz.isPolishCitizen = true;
                                        if (!string.IsNullOrWhiteSpace(bd.Secondname))
                                            osfiz.SecondName = bd.Secondname;
                                        osfiz.Surname = bd.Name;
                                        osfiz.IdentityNumber = new consumerIdentityNumberType();
                                        osfiz.IdentityNumber.Item = bd.IDNumber;
                                        if (!String.IsNullOrWhiteSpace(bd.Address1L1))
                                            osfiz.Address = adr;
                                        debt.Item = osfiz;
                                    }
                                    else if (bd.DebtorType == 1) // osoba prawna)
                                    {
                                        legalPersonType ospraw = new legalPersonType();
                                        ospraw.IdentityNumber = new nonConsumerIdentityNumberType();
                                        ospraw.Name = bd.Name;
                                        ospraw.RegistrationNumber = bd.Krs;
                                        ospraw.SeatAddress = adr;
                                        ospraw.IdentityNumber.Item = bd.IDNumber;
                                        debt.Item = ospraw;
                                    }
                                    addSpr.Debtor = debt;
                                } // debtor
                            }
                            else if (bop.TypOperacji == 103)
                            { // wykreśanie sprawy 

                                informationManagementOrderType ord = new informationManagementOrderType();
                                ord.ID = (++lp).ToString();

                                removeInformationType delInf = new removeInformationType();
                                delInf.informationType = informationTypeEnum.Case;
                                delInf.removeReason = removeReasonType.Other;
                                delInf.IDType = idTypeEnum.UserId;
                                delInf.ID = bca.CaseId;
                                delInf.verifyResult = true;
                                delInf.verifyResultSpecified = true;
                                ord.Item = delInf;
                                infoOrd.Add(ord);

                            }
                        }

                        if (bop.TypOperacji == 101 || bop.TypOperacji == 1 || bop.TypOperacji % 100 == 2)
                        { // obligation
                            obligationType oblItem = new obligationType();
                            if (bob.Kwota != null && bob.Kwota > 0)
                            {
                                oblItem.Debt = new moneyType();
                                oblItem.Debt.currency = "PLN";
                                oblItem.Debt.Value = bob.Kwota.Value;
                            }
                            if (bob.Saldo > 0)
                            {
                                oblItem.Arrears = new moneyType();
                                oblItem.Arrears.currency = "PLN";
                                oblItem.Arrears.Value = bob.Saldo;


                            }
                            oblItem.CallSent = bob.DataWysWezw.Value;
                            oblItem.PaymentDate = bob.DataWymag.Value;
                            List<ItemsChoiceType> iChLst = new List<ItemsChoiceType>();
                            iChLst.Add(ItemsChoiceType.Title);
                            iChLst.Add(ItemsChoiceType.Type);
                            List<object> lst = new List<object>();
                            lst.Add(bob.Title);
                            lst.Add((obligationTypeEnum)bob.TypNal);
                            oblItem.Items = lst.ToArray();
                            oblItem.ItemsElementName = iChLst.ToArray();
                            oblItem.Item = true;

                            if (bop.TypOperacji % 100 == 2)
                            {
                                updateInformationType updtInf = new updateInformationType();
                                updtInf.IDType = idTypeEnum.UserId;
                                updtInf.ID = bob.ObligationId;
                                updtInf.verifyResult = true;
                                updtInf.Item = oblItem;
                                informationManagementOrderType ord = new informationManagementOrderType();
                                ord.ID = (++lp).ToString();
                                ord.Item = updtInf;
                                infoOrd.Add(ord);


                            }
                            else if (bop.TypOperacji == 101) // Adding in case
                            {
                                addObligationType addOblItem = new addObligationType(oblItem);
                                addOblItem.userID = bob.ObligationId;
                                addCaseTypeObligations oblArr = null;
                                List<addObligationType> obltypeList = new List<addObligationType>();
                                oblArr = addSpr.Items[0] as addCaseTypeObligations;
                                if (oblArr == null)
                                {
                                    oblArr = new addCaseTypeObligations();
                                    //oblArr.Obligation = new addObligationType[1];
                                }
                                else
                                    obltypeList = oblArr.Obligation.ToList();
                                obltypeList.Add(addOblItem);
                                oblArr.Obligation = obltypeList.ToArray();

                                addSpr.Items[0] = oblArr;
                            }
                            else if (bop.TypOperacji == 1)
                            {
                                addInformationType addInf = new addInformationType();
                                addInf.userID = bob.ObligationId;
                                addInf.verifyResult = true;
                                addObligationInformationType aobl = new addObligationInformationType();
                                aobl.Case = new addObligationInformationTypeCase();
                                aobl.Case.IDType = idTypeEnum.UserId;
                                aobl.Case.ID = bca.CaseId;
                                aobl.Item = oblItem;
                                addInf.Item = aobl;
                                addInf.verifyResult = true;
                                informationManagementOrderType ord = new informationManagementOrderType();
                                ord.ID = (++lp).ToString();
                                ord.Item = addInf;
                                infoOrd.Add(ord);

                            }
                        }

                        else //usuwanie należności
                             if (bop.TypOperacji == 3)  // usuwanie pojedynczej faktury 
                        {
                            informationManagementOrderType ord = new informationManagementOrderType();
                            ord.ID = (++lp).ToString();

                            removeInformationType delInf = new removeInformationType();
                            delInf.informationType = informationTypeEnum.ObligationInformation;
                            delInf.removeReason = removeReasonType.Other;
                            delInf.IDType = idTypeEnum.UserId;
                            delInf.ID = bob.ObligationId;
                            delInf.verifyResult = true;
                            delInf.verifyResultSpecified = true;
                            ord.Item = delInf;
                            infoOrd.Add(ord);
                        }


                        prevSprId = sprId;
                        bop.OrderOp = lp;
                    }
                    context.SaveChanges();

                }

                catch (Exception ex)
                {

                    Utils.LogWriter("Błąd podczas procedowania pakietu " +ex.Message + (ex.InnerException != null ? " " +ex.InnerException.Message : ""));

                }
                infoTyp.Order = infoOrd.ToArray();
                input.Item = infoTyp;
            }
            return input;
        }
            /*

                            }
                            else if (bop.TypOperacji == 2)
                            {
                                updtSpr = new updateCaseType();
                                updtSpr.Note = bca.CaseId;
                                updtSpr.obligationRemoveReason = removeReasonType.Other; // ApplicationSourceName = Utils.sysNameFromId(bca.SrcSystem.Value);
                                updtSpr.showProvider = true;
                                updtCaseLst.Add(updtSpr);
                                newcase = true;
                                updtSpr.Items = new object[5];
                                typspr = 2;
                            }
                            else if (bop.TypOperacji == 3)
                            { // usunięcie 
                                Utils.LogWriter("Remove ");

                                delSpr = new removeInformationTypeEx();
                                removeInformationTypeEx found = removeLst.Where(a => a.reInfType.ID == bop.BIG_Case.CaseId).FirstOrDefault();
                                Utils.LogWriter("Remove 1");
                                if (found != null) // jest juz taka sprawa
                                    delSpr.idOper = -bop.BIG_OperacjaId; // zaznaczamy do opuszczenia
                                else
                                {
                                    delSpr.idOper = bop.BIG_OperacjaId;
                                    delSpr.reInfType = new removeInformationType();
                                    delSpr.reInfType.IDType = idTypeEnum.UserId;
                                    delSpr.reInfType.removeReason = removeReasonType.Other;
                                    delSpr.reInfType.verifyResult = true;
                                    delSpr.reInfType.verifyResultSpecified = true;

                                    //List<BIGvw_ObligationLastStatus> lst = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_CaseId == bop.BIG_CaseId).ToList();
                                    List<BIGvw_ObligationLastStatus> lst = listLastStat.Where(a => a.BIG_CaseId == bop.BIG_CaseId && a.BIG_ImportId < imp.BIG_ImportId).ToList();
                                    Utils.LogWriter("Remove 2");
                                    if (lst == null || !lst.Any())
                                    {

                                        delSpr.reInfType.informationType = informationTypeEnum.Case;
                                        delSpr.reInfType.ID = bca.CaseId;
                                    }
                                    else
                                    {
                                        delSpr.reInfType.informationType = informationTypeEnum.ObligationInformation;
                                        delSpr.reInfType.ID = bob.ObligationId;
                                    }


                                    removeLst.Add(delSpr);
                                    newcase = false;
                                }
                            }
                          


                        }



                    }



                    Utils.LogWriter("before newcase 2");


                    if (newcase)
                    {
                        BIG_Debtor bd = context.BIG_Debtor.Where(a => a.BIG_CaseId == idSprawy && a.BIG_DebtorId == bop.BIG_DebtorId).FirstOrDefault();
                        if (bd != null)
                        {
                            debtorType debt = new debtorType();


                            addressType adr = new addressType();
                            adr.countryCode = "PL";
                            adr.Line = new string[2];
                            adr.Line[0] = bd.Address1L1;
                            adr.Line[1] = bd.Address1L2;

                            if (bd.DebtorType == 0) // osoba fizyczna
                            {
                                consumerType osfiz = new consumerType();
                                osfiz.FirstName = bd.Firstname;
                                osfiz.isPolishCitizen = true;
                                if (!string.IsNullOrWhiteSpace(bd.Secondname))
                                    osfiz.SecondName = bd.Secondname;
                                osfiz.Surname = bd.Name;
                                osfiz.IdentityNumber = new consumerIdentityNumberType();
                                osfiz.IdentityNumber.Item = bd.IDNumber;
                                osfiz.Address = adr;
                                debt.Item = osfiz;
                            }
                            else if (bd.DebtorType == 1) // osoba prawna)
                            {
                                legalPersonType ospraw = new legalPersonType();
                                ospraw.IdentityNumber = new nonConsumerIdentityNumberType();
                                ospraw.Name = bd.Name;
                                ospraw.RegistrationNumber = bd.Krs;
                                ospraw.SeatAddress = adr;
                                ospraw.IdentityNumber.Item = bd.IDNumber;
                                debt.Item = ospraw;
                            }

                            if (bop.TypOperacji == 1)
                                addSpr.Debtor = debt;
                            else if (bop.TypOperacji == 2)
                                updtSpr.Debtor = debt;
                        }
                        else
                        {
                            errcode = "Brak danych dłużnika";
                            return null;
                        }


                    }




                    // należności
                    if (bop.TypOperacji != 3 && bop.TypOperacji != 5 && bop.TypOperacji != 17 && bop.TypOperacji != 6) // jełsi nie usunięcie należności
                    {

                        //addObligationType oblItem = new addObligationType();
                        obligationType oblItem = new obligationType();

                        if (bob.Kwota != null && bob.Kwota > 0)
                        {
                            oblItem.Debt = new moneyType();
                            oblItem.Debt.currency = "PLN";
                            oblItem.Debt.Value = bob.Kwota.Value;
                        }
                        if (bob.Saldo > 0)
                        {
                            oblItem.Arrears = new moneyType();
                            oblItem.Arrears.currency = "PLN";
                            oblItem.Arrears.Value = bob.Saldo;


                        }
                        oblItem.CallSent = bob.DataWysWezw.Value;
                        oblItem.PaymentDate = bob.DataWymag.Value;
                        List<ItemsChoiceType> iChLst = new List<ItemsChoiceType>();
                        iChLst.Add(ItemsChoiceType.Title);
                        iChLst.Add(ItemsChoiceType.Type);
                        List<object> lst = new List<object>();
                        lst.Add(bob.Title);
                        lst.Add((obligationTypeEnum)bob.TypNal);
                        oblItem.Items = lst.ToArray();
                        oblItem.ItemsElementName = iChLst.ToArray();
                        oblItem.Item = true;
                        if (bop.TypOperacji == 1)
                        {
                            addObligationType addOblItem = new addObligationType(oblItem);
                            addOblItem.userID = bob.ObligationId;
                            addCaseTypeObligations oblArr = null;
                            List<addObligationType> obltypeList = new List<addObligationType>();
                            oblArr = (typspr == 1 ? addSpr.Items[0] as addCaseTypeObligations : updtSpr.Items[0] as addCaseTypeObligations);
                            if (oblArr == null)
                            {
                                oblArr = new addCaseTypeObligations();
                                //oblArr.Obligation = new addObligationType[1];
                            }
                            else
                                obltypeList = oblArr.Obligation.ToList();
                            obltypeList.Add(addOblItem);
                            oblArr.Obligation = obltypeList.ToArray();
                            if (typspr == 1)
                                addSpr.Items[0] = oblArr;
                            else
                            {
                                addObligationInformationTypeEx addOItem = new addObligationInformationTypeEx();
                                addOItem.addOblig = new addObligationInformationType();
                                addOItem.addOblig.Case = new addObligationInformationTypeCase();
                                addOItem.addOblig.Case.ID = sprId;
                                addOItem.addOblig.Case.IDType = idTypeEnum.UserId;
                                addOItem.addOblig.Item = oblItem;
                                addOItem.idOper = bop.BIG_OperacjaId;
                                addOblig.Add(addOItem);
                                //  updtSpr.Items[0] = oblArr;

                            }
                        }
                        else if (bop.TypOperacji == 2) // update
                        {
                            updateObligationType updateOblItem = new updateObligationType(oblItem);
                            updateOblItem.ID = bob.ObligationId;
                            updateOblItem.IDType = idTypeEnum.UserId;

                            updateCaseTypeObligations oblArr = new updateCaseTypeObligations();

                            List<updateObligationType> obltypeList = new List<updateObligationType>();
                            oblArr = updtSpr.Items[1] as updateCaseTypeObligations;
                            if (oblArr == null)
                                oblArr = new updateCaseTypeObligations();
                            else
                                obltypeList = oblArr.Obligation.ToList();
                            obltypeList.Add(updateOblItem);
                            oblArr.Obligation = obltypeList.ToArray();
                            updtSpr.Items[1] = oblArr;
                        }


                    }



                }
                Utils.LogWriter(" Koniec foreach");

                // najpierw usunięcia
             Utils.LogWriter("Before remove");
                if (removeLst != null && removeLst.Any())
                {

                    foreach (removeInformationTypeEx inf in removeLst)
                    {
                        int oprNo;
                        if (inf.idOper > 0)
                        {
                            informationManagementOrderType ord = new informationManagementOrderType();
                            ord.ID = (++lp).ToString();
                            ord.Item = inf.reInfType;
                            oprNo = inf.idOper;

                            infoOrd.Add(ord);
                        }
                        else
                        {
                            oprNo = -inf.idOper;
                        }
                        BIG_Operacja boperac = context.BIG_Operacja.Where(a => a.BIG_OperacjaId == oprNo).FirstOrDefault();
                        if (boperac != null)
                            boperac.OrderOp = lp;
                    }
                    context.SaveChanges();
                }




                Utils.LogWriter("Before add/update");
                if ((addCaseLst != null && addCaseLst.Any()) || (updtCaseLst != null && updtCaseLst.Any()))
                {


                    input.Item = infoTyp;

                    foreach (addCaseType ac in addCaseLst)
                    {
                        List<object> lstout = ac.Items.Where(a => a != null).ToList();
                        ac.Items = lstout.ToArray();
                        informationManagementOrderType ord = new informationManagementOrderType();
                        ord.ID = (++lp).ToString();
                        addInformationType adInf = new addInformationType();
                        adInf.verifyResult = true;
                        adInf.userID = ac.Note;

                        ord.Item = adInf;
                        adInf.Item = ac;
                        infoOrd.Add(ord);
                        List<BIG_Operacja> lstopr = new List<BIG_Operacja>();
                        lstopr = (from opr in context.BIG_Operacja
                                  join
      o in context.BIG_Obligation on opr.BIG_ObligationId equals o.BIG_ObligationId
                                  join
cse in context.BIG_Case on o.BIG_CaseId equals cse.BIG_CaseId
                                  where opr.BIG_ImportId == IdIput && cse.CaseId == ac.Note
                                  select opr).ToList();
                        foreach (BIG_Operacja oo in lstopr)
                        {
                            oo.OrderOp = Convert.ToInt32(ord.ID);

                        }




                        ac.Note = null;
                    }
                    Utils.LogWriter("Before update");
                    foreach (updateCaseType ac in updtCaseLst)
                    {
                        List<object> lstout = ac.Items.Where(a => a != null).ToList();
                        ac.Items = lstout.ToArray();

                        informationManagementOrderType ord = new informationManagementOrderType();
                        ord.ID = (++lp).ToString();
                        updateInformationType adInf = new updateInformationType();
                        adInf.verifyResult = true;
                        adInf.ID = ac.Note;
                        adInf.IDType = idTypeEnum.UserId;
                        ord.Item = adInf;
                        adInf.Item = ac;
                        infoOrd.Add(ord);
                        List<BIG_Operacja> lstopr = new List<BIG_Operacja>();
                        lstopr = (from opr in context.BIG_Operacja
                                  join
      o in context.BIG_Obligation on opr.BIG_ObligationId equals o.BIG_ObligationId
                                  join
cse in context.BIG_Case on o.BIG_CaseId equals cse.BIG_CaseId
                                  where opr.BIG_ImportId == IdIput && cse.CaseId == ac.Note
                                  select opr).ToList();
                        // usunięcie należność


                        foreach (BIG_Operacja oo in lstopr)
                        {
                            oo.OrderOp = Convert.ToInt32(ord.ID);

                        }




                        ac.Note = null;
                    }
                    context.SaveChanges();

                }
                // teraz dodanie nowych należności 
                foreach (addObligationInformationTypeEx inf in addOblig)
                {
                    int oprNo;
                    if (inf.idOper > 0)
                    {
                        informationManagementOrderType ord = new informationManagementOrderType();
                        ord.ID = (++lp).ToString();
                        addInformationType adInf = new addInformationType();
                        adInf.Item = inf.addOblig;
                        ord.Item = adInf;
                        oprNo = inf.idOper;
                        infoOrd.Add(ord);
                    }
                    else
                    {
                        oprNo = -inf.idOper;
                    }


                    BIG_Operacja boperac = context.BIG_Operacja.Where(a => a.BIG_OperacjaId == oprNo).FirstOrDefault();
                    if (boperac != null)
                        boperac.OrderOp = lp;



                }
                context.SaveChanges();

                infoTyp.Order = infoOrd.ToArray();
                input.Item = infoTyp;
            }
            Utils.LogWriter("At exit");
            return errFound ? null : input;
        }
    */
        public Input Import2KrdInput(int IdIput)
        {
            Input input = new Input();
           
            input.timeStampSpecified = true;
            input.timeStamp = DateTime.Now;
            input.generator = "Lexena/EOB";
            bool errFound = false;
            int lp = 0; // numeracja operacji w pakiecie
            List<addCaseType> addCaseLst = new List<addCaseType>();
            List<updateCaseType> updtCaseLst = new List<updateCaseType>();
            List<addObligationInformationTypeEx> addOblig  = new List<addObligationInformationTypeEx>(); // lista tylko dodanych należności do istniejacych spraw.

            List<BIG_Case> case2DelLst = new List<BIG_Case>();
            List<removeInformationTypeEx> removeLst = new List<removeInformationTypeEx>();
            List<suspendInformationTypeEx> suspendLst = new List<suspendInformationTypeEx>();
            List<unSuspendInformationTypeEx> unSuspendLst = new List<unSuspendInformationTypeEx>();
            List<notifyInformationTypeEx> notifySprLst = new List<notifyInformationTypeEx>();
           

            informationManagementOrdersType1 item = new informationManagementOrdersType1();
            //item.Order = 
            informationManagementOrdersType1[] arr;


            informationManagementOrdersType1 infoTyp = new informationManagementOrdersType1();
            List<informationManagementOrderType> infoOrd = new List<informationManagementOrderType>();

            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {
                BIG_Import imp = context.BIG_Import.Where(a => a.BIG_ImportId == IdIput).FirstOrDefault();
                
                if (imp == null)
                {
                    errcode = "Brak pakietu o takim Id = " + IdIput.ToString();
                    return null;

                }
                input.fileName = imp.Filename;


                List<BIG_Operacja> oprLst = context.BIG_Operacja.Include("BIG_Obligation").Where(a => a.BIG_ImportId == imp.BIG_ImportId).OrderByDescending(a=>a.TypOperacji).ThenBy(a=>a.BIG_CaseId).ToList();
                List<BIGvw_ObligationLastStatus> listLastStat = (from v in context.BIGvw_ObligationLastStatus join bo in context.BIG_Operacja on v.BIG_CaseId equals bo.BIG_CaseId where bo.BIG_ImportId == imp.BIG_ImportId select v).ToList();

                Utils.LogWriter("Lista załadowana sztuk " + listLastStat.Count.ToString());
                if (oprLst == null || !oprLst.Any())
                {
                    errcode = "Brak operacji dla podanego pakietu";
                    return null;

                }

                if (imp.TypImp == 3 ) // jeśli wykreślenie spraw.
                {
                   
                    foreach (BIG_Operacja bop in oprLst)
                    {
                        if (!case2DelLst.Contains(bop.BIG_Obligation.BIG_Case))
                        {

                            case2DelLst.Add(bop.BIG_Obligation.BIG_Case);
                        }
                    }

                    Utils.LogWriter("Budowa pokietu do usunięcia");
                    if (case2DelLst.Any())
                    {
                       


                        {
                            input.Item = infoTyp;
                        
                            foreach (BIG_Case bc in case2DelLst)
                            {

                                informationManagementOrderType ord = new informationManagementOrderType();
                                ord.ID = (++lp).ToString();

                                removeInformationType delInf = new removeInformationType();
                                delInf.informationType = informationTypeEnum.Case;
                                delInf.removeReason = removeReasonType.Other;
                                delInf.IDType = idTypeEnum.UserId;
                                delInf.ID = bc.CaseId;
                                delInf.verifyResult = true;
                                delInf.verifyResultSpecified = true;
                                ord.Item = delInf;
                                infoOrd.Add(ord);
                                foreach (BIG_Operacja bop in oprLst.Where(a=>a.BIG_Obligation.BIG_CaseId == bc.BIG_CaseId).ToList())
                                {
                                    bop.StatusOperacji = 0;
                                    bop.OrderOp = lp;
                                    bop.StatusOperacji = 0;
                                }
                                context.SaveChanges();
                            }

                            infoTyp.Order = infoOrd.ToArray();
                        }
                      
                            return input;



                    }
                    else return null;

                }
                Utils.LogWriter("Procedowanie pakietu");
                bool newspr = true;
                int sprid = 0;
                addCaseType addSpr = null;
                updateCaseType updtSpr = null;
                removeInformationTypeEx delSpr = null;
                suspendInformationTypeEx suspSpr = null;
                unSuspendInformationTypeEx unSuspSpr = null;
                notifyInformationTypeEx notifySpr = null;
                int count = 0;
                foreach (BIG_Operacja bop in oprLst)
                {
                    string sprId = bop.BIG_Obligation.BIG_Case.CaseId;
                    int idSprawy = bop.BIG_Obligation.BIG_CaseId.Value;
                    BIG_Obligation bob = bop.BIG_Obligation;
                    BIG_Case bca = bob.BIG_Case;
                    BIG_Debtor deb  = bop.BIG_Debtor; 
                    // czy sprawa już jest w Krd ?
                    object sprawaObj = null;
                    int typspr=0;
                    bool newcase = false;
                    updtSpr = null;
                    addSpr = null;
                    delSpr = null;
                    suspSpr = null;
                    ++count;
                    if (idSprawy == 33941)
                    {

                        ;

                    }
                    string validate = validateRow(bob, deb);
                    Utils.LogWriter("Validation");
                    if (!String.IsNullOrWhiteSpace(validate))
                    {
                        Utils.LogWriter("Blad walidacji " + validate);
                        bop.StatOpis = "Błąd walidacji" + " " + validate;
                        bop.StatusOperacji = -1;
                        errFound = true;
                        context.SaveChanges();
                    }

             //       case2DelLst.Where(a=>a.CaseId == )



                    updtSpr = updtCaseLst.Where(a => a.Note == sprId).FirstOrDefault();
                    if (updtSpr != null)
                        typspr = 2;
                    else
                    {
                        addSpr = addCaseLst.Where(a => a.Note == sprId).FirstOrDefault();
                        if (addSpr != null)
                            typspr = 1;
                        else
                        {
                            if (bop.TypOperacji == 1)
                            {
                                typspr = 1;
                                addSpr = new addCaseType();
                                addSpr.Note = bca.CaseId;
                                addSpr.ApplicationSourceName = Utils.sysNameFromId(bca.SrcSystem.Value);
                                addSpr.showProvider = true;
                                addCaseLst.Add(addSpr);
                                newcase = true;
                                addSpr.Items = new object[5];

                            }
                            else if (bop.TypOperacji == 2)
                            {
                                updtSpr = new updateCaseType();
                                updtSpr.Note = bca.CaseId;
                                updtSpr.obligationRemoveReason = removeReasonType.Other; // ApplicationSourceName = Utils.sysNameFromId(bca.SrcSystem.Value);
                                updtSpr.showProvider = true;
                                updtCaseLst.Add(updtSpr);
                                newcase = true;
                                updtSpr.Items = new object[5];
                                typspr = 2;
                            }
                            else if (bop.TypOperacji == 3)
                            { // usunięcie 
                                Utils.LogWriter("Remove ");

                                delSpr = new removeInformationTypeEx();
                                removeInformationTypeEx found = removeLst.Where(a => a.reInfType.ID == bop.BIG_Case.CaseId).FirstOrDefault();
                                Utils.LogWriter("Remove 1");
                                if (found != null) // jest juz taka sprawa
                                    delSpr.idOper = -bop.BIG_OperacjaId; // zaznaczamy do opuszczenia
                                else
                                {
                                    delSpr.idOper = bop.BIG_OperacjaId;
                                    delSpr.reInfType = new removeInformationType();
                                    delSpr.reInfType.IDType = idTypeEnum.UserId;
                                    delSpr.reInfType.removeReason = removeReasonType.Other;
                                    delSpr.reInfType.verifyResult = true;
                                    delSpr.reInfType.verifyResultSpecified = true;

                                    //List<BIGvw_ObligationLastStatus> lst = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_CaseId == bop.BIG_CaseId).ToList();
                                    List<BIGvw_ObligationLastStatus> lst = listLastStat.Where(a => a.BIG_CaseId == bop.BIG_CaseId && a.BIG_ImportId < imp.BIG_ImportId).ToList();
                                    Utils.LogWriter("Remove 2");
                                    if (lst == null || !lst.Any())
                                    {

                                        delSpr.reInfType.informationType = informationTypeEnum.Case;
                                        delSpr.reInfType.ID = bca.CaseId;
                                    }
                                    else
                                    {
                                        delSpr.reInfType.informationType = informationTypeEnum.ObligationInformation;
                                        delSpr.reInfType.ID = bob.ObligationId;
                                    }


                                    removeLst.Add(delSpr);
                                    newcase = false;
                                }
                            }
                            else if (bop.TypOperacji == 5) // zawieszenie
                            {
                                Utils.LogWriter("Suspend");
                                suspSpr = new suspendInformationTypeEx();
                                List<BIG_Operacja> bopsLst = oprLst.Where(a => a.TypOperacji == 5 && a.BIG_Case.CaseId == bop.BIG_Case.CaseId).ToList();
                               // List<BIGvw_ObligationLastStatus> lst = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_CaseId == bop.BIG_CaseId).ToList();
                                List<BIGvw_ObligationLastStatus> lst = listLastStat.Where(a => a.BIG_CaseId == bop.BIG_CaseId).ToList();
                                List<int> obligLst = lst.OrderBy(a => a.BIG_ObligationId).Select(a => a.BIG_ObligationId).Distinct().ToList();
                                List<int> suspObligLst = bopsLst.OrderBy(a => a.BIG_ObligationId).Select(a => a.BIG_ObligationId.Value).Distinct().ToList();


                                suspSpr.susInfType = new suspendType();
                                suspSpr.susInfType.IDType = idTypeEnum.UserId;
                                suspSpr.susInfType.DateTo = bop.SuspendDate.Value;
                                suspSpr.susInfType.DateFromSpecified = false;
                                if (obligLst.SequenceEqual(suspObligLst))   // to zawieszenie całej sprawy
                                {
                                    suspSpr.susInfType.informationType = informationTypeEnum.Case;
                                    suspSpr.susInfType.ID = bob.BIG_Case.CaseId;
                                    if (suspendLst.Where(a => a.susInfType.informationType == informationTypeEnum.Case && a.susInfType.ID == bob.BIG_Case.CaseId).Any())
                                    { // jest juz taki

                                        suspSpr.idOper = -bop.BIG_OperacjaId;
                                    }
                                    else
                                        suspSpr.idOper = bop.BIG_OperacjaId;
                                }
                                else
                                {
                                    suspSpr.idOper = bop.BIG_OperacjaId;
                                    suspSpr.susInfType.informationType = informationTypeEnum.ObligationInformation;
                                    suspSpr.susInfType.ID = bob.ObligationId;
                                }





                                suspendLst.Add(suspSpr);
                                newcase = false;
                            }
                            else if (bop.TypOperacji == 6) // podjęcie
                            {
                                unSuspSpr = new unSuspendInformationTypeEx();
                                List<BIG_Operacja> bopsLst = oprLst.Where(a => a.TypOperacji == 6 && a.BIG_Case.CaseId == bop.BIG_Case.CaseId).ToList();
                                //List<BIGvw_ObligationLastStatus> lst = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_CaseId == bop.BIG_CaseId).ToList();
                                List<BIGvw_ObligationLastStatus> lst = listLastStat.Where(a => a.BIG_CaseId == bop.BIG_CaseId).ToList();

                                List<int> obligLst = lst.OrderBy(a => a.BIG_ObligationId).Select(a => a.BIG_ObligationId).Distinct().ToList();
                                List<int> suspObligLst = bopsLst.OrderBy(a => a.BIG_ObligationId).Select(a => a.BIG_ObligationId.Value).Distinct().ToList();


                                unSuspSpr.unSusInfType = new entityManagementWithVerificationType();
                                unSuspSpr.unSusInfType.IDType = idTypeEnum.UserId;

                                if (obligLst.SequenceEqual(suspObligLst))   // to zawieszenie całej sprawy
                                {
                                    unSuspSpr.unSusInfType.informationType = informationTypeEnum.Case;
                                    unSuspSpr.unSusInfType.ID = bob.BIG_Case.CaseId;
                                    if (unSuspendLst.Where(a => a.unSusInfType.informationType == informationTypeEnum.Case && a.unSusInfType.ID == bob.BIG_Case.CaseId).Any())
                                    { // jest juz taki

                                        unSuspSpr.idOper = -bop.BIG_OperacjaId;
                                    }
                                    else
                                        unSuspSpr.idOper = bop.BIG_OperacjaId;
                                }
                                else
                                {
                                    unSuspSpr.idOper = bop.BIG_OperacjaId;
                                    unSuspSpr.unSusInfType.informationType = informationTypeEnum.ObligationInformation;
                                    unSuspSpr.unSusInfType.ID = bob.ObligationId;
                                }





                                unSuspendLst.Add(unSuspSpr);
                                newcase = false;
                            }
                            else if (bop.TypOperacji == 17)
                            {
                                notifySpr = new notifyInformationTypeEx();
                                //BIGvw_ObligationLastStatus myCase = context.BIGvw_ObligationLastStatus.Where(a => a.BIG_CaseId == bop.BIG_CaseId).FirstOrDefault();
                                BIGvw_ObligationLastStatus myCase = listLastStat.Where(a => a.BIG_CaseId == bop.BIG_CaseId).FirstOrDefault();

                                BIG_Debtor bdebtor = context.BIG_Debtor.Where(a => a.BIG_DebtorId == bop.BIG_DebtorId).FirstOrDefault();


                                notifySpr.notifyInfType = new notifyDebtorOrderType();
                                notifySpr.notifyInfType.IDType = idTypeEnum.UserId;
                                notifyDebtorType notify = new notifyDebtorType();
                                notify.notificationLanguage = "PL";
                                notify.Address = new notificationAddressType();
                                notify.Address.countryCode = "PL";
                                notify.Address.type = addressTypeEnum.Regular;
                                notify.Address.Line = new string[2];
                                notify.Address.Line[0] = bdebtor.Address1L1;
                                notify.Address.Line[1] = bdebtor.Address1L2;



                                notifySpr.notifyInfType.informationType = informationTypeEnum.Case;
                                    notifySpr.notifyInfType.Notify = notify;

                                    notifySpr.notifyInfType.ID = bob.BIG_Case.CaseId;
                                    if (notifySprLst.Where(a => a.notifyInfType.informationType == informationTypeEnum.Case && a.notifyInfType.ID == bob.BIG_Case.CaseId).Any())
                                    { // jest juz taki

                                        notifySpr.idOper = -bop.BIG_OperacjaId;
                                    }
                                    else
                                       notifySpr.idOper = bop.BIG_OperacjaId;
                            
                            /*
                               else
                                {
                                    notifySpr.idOper = bop.BIG_OperacjaId;
                                    notifySpr.notifyInfType.informationType = informationTypeEnum.ObligationInformation;
                                    notifySpr.notifyInfType.ID = bob.ObligationId;
                                }
                                */




                                notifySprLst.Add(notifySpr);
                                newcase = false;

                            }



                        }



                    }



                    Utils.LogWriter("before newcase 2");
                 

                    if (newcase)
                    {
                        BIG_Debtor bd = context.BIG_Debtor.Where(a => a.BIG_CaseId == idSprawy && a.BIG_DebtorId == bop.BIG_DebtorId).FirstOrDefault();
                        if (bd != null)
                        {
                            debtorType debt = new debtorType();


                            addressType adr = new addressType();
                            adr.countryCode = "PL";
                            adr.Line = new string[2];
                            adr.Line[0] = bd.Address1L1;
                            adr.Line[1] = bd.Address1L2;

                            if (bd.DebtorType == 0) // osoba fizyczna
                            {
                                consumerType osfiz = new consumerType();
                                osfiz.FirstName = bd.Firstname;
                                osfiz.isPolishCitizen = true;
                                if (!string.IsNullOrWhiteSpace(bd.Secondname))
                                 osfiz.SecondName = bd.Secondname;
                                osfiz.Surname = bd.Name;
                                osfiz.IdentityNumber = new consumerIdentityNumberType();
                                osfiz.IdentityNumber.Item = bd.IDNumber;
                                if (!String.IsNullOrWhiteSpace(bd.Address1L1))
                                    osfiz.Address = adr;
                                debt.Item = osfiz;
                            }
                            else if (bd.DebtorType == 1) // osoba prawna)
                            {
                                legalPersonType ospraw = new legalPersonType();
                                ospraw.IdentityNumber = new nonConsumerIdentityNumberType();
                                ospraw.Name = bd.Name;
                                ospraw.RegistrationNumber = bd.Krs;
                                ospraw.SeatAddress = adr;
                                ospraw.IdentityNumber.Item = bd.IDNumber;
                                debt.Item = ospraw;
                            }

                            if (bop.TypOperacji == 1)
                                addSpr.Debtor = debt;
                            else if (bop.TypOperacji == 2)
                                updtSpr.Debtor = debt;
                        }
                        else
                        {
                            errcode = "Brak danych dłużnika";
                            return null;
                        }


                    }




                    // należności
                    if (bop.TypOperacji != 3 && bop.TypOperacji != 5 && bop.TypOperacji != 17 && bop.TypOperacji != 6) // jełsi nie usunięcie należności
                    {

                        //addObligationType oblItem = new addObligationType();
                        obligationType oblItem = new obligationType();

                        if (bob.Kwota != null && bob.Kwota > 0)
                        {
                            oblItem.Debt = new moneyType();
                            oblItem.Debt.currency = "PLN";
                            oblItem.Debt.Value = bob.Kwota.Value;
                        }
                        if (bob.Saldo > 0)
                        {
                            oblItem.Arrears = new moneyType();
                            oblItem.Arrears.currency = "PLN";
                            oblItem.Arrears.Value = bob.Saldo;


                        }
                        oblItem.CallSent = bob.DataWysWezw.Value;
                        oblItem.PaymentDate = bob.DataWymag.Value;
                        List<ItemsChoiceType> iChLst = new List<ItemsChoiceType>();
                        iChLst.Add(ItemsChoiceType.Title);
                        iChLst.Add(ItemsChoiceType.Type);
                        List<object> lst = new List<object>();
                        lst.Add(bob.Title);
                        lst.Add((obligationTypeEnum)bob.TypNal);
                        oblItem.Items = lst.ToArray();
                        oblItem.ItemsElementName = iChLst.ToArray();
                        oblItem.Item = true;
                        if (bop.TypOperacji == 1)
                        {
                            addObligationType addOblItem = new addObligationType(oblItem);
                            addOblItem.userID = bob.ObligationId;
                            addCaseTypeObligations oblArr = null;
                            List<addObligationType> obltypeList = new List<addObligationType>();
                            oblArr = (typspr == 1 ? addSpr.Items[0] as addCaseTypeObligations : updtSpr.Items[0] as addCaseTypeObligations);
                            if (oblArr == null)
                            {
                                oblArr = new addCaseTypeObligations();
                                //oblArr.Obligation = new addObligationType[1];
                            }
                            else
                                obltypeList = oblArr.Obligation.ToList();
                            obltypeList.Add(addOblItem);
                            oblArr.Obligation = obltypeList.ToArray();
                            if (typspr == 1)
                                addSpr.Items[0] = oblArr;
                            else
                            { addObligationInformationTypeEx addOItem = new addObligationInformationTypeEx();
                                addOItem.addOblig = new addObligationInformationType();
                                addOItem.addOblig.Case = new addObligationInformationTypeCase();
                                addOItem.addOblig.Case.ID = sprId;
                                addOItem.addOblig.Case.IDType = idTypeEnum.UserId;
                                addOItem.addOblig.Item = oblItem;
                                addOItem.idOper = bop.BIG_OperacjaId;
                                addOblig.Add(addOItem); 
                              //  updtSpr.Items[0] = oblArr;

                            }
                        }
                        else if (bop.TypOperacji == 2) // update
                        {
                            updateObligationType updateOblItem = new updateObligationType(oblItem);
                            updateOblItem.ID = bob.ObligationId;
                            updateOblItem.IDType = idTypeEnum.UserId;

                            updateCaseTypeObligations oblArr = new updateCaseTypeObligations();

                            List<updateObligationType> obltypeList = new List<updateObligationType>(); 
                            oblArr = updtSpr.Items[1] as updateCaseTypeObligations;
                            if (oblArr == null)
                                oblArr = new updateCaseTypeObligations();
                            else
                                obltypeList = oblArr.Obligation.ToList();
                            obltypeList.Add(updateOblItem);
                            oblArr.Obligation = obltypeList.ToArray();
                            updtSpr.Items[1] = oblArr;
                        }

                        
                    }

                    
                   
                }
                Utils.LogWriter(" Koniec foreach");
                /*
          //        if (idsprawy =  )
                  if (spr.Items == null)
                  {
                      oblArr = new addCaseTypeObligations();

                      addObligationType oblItem = new addObligationType();
                      if (bob.Kwota != null && bob.Kwota  > 0)
                      {
                          oblItem.Arrears = new moneyType();
                          oblItem.Arrears.currency = "PLN";
                          oblItem.Arrears.Value = bob.Kwota.Value;
                      }
                      if (bob.Saldo > 0)
                      {
                          oblItem.Debt = new moneyType();
                          oblItem.Debt.currency = "PLN";
                          oblItem.Debt.Value = bob.Saldo;

                      }
                      oblItem.CallSent = bob.DataWysWezw.Value;
                      oblItem.PaymentDate = bob.DataWymag.Value;
                      oblItem.userID = bob.ObligationId;

                      obltypeList.Add(oblItem);
                      oblArr.Obligation = obltypeList.ToArray();


                      //spr.Items = new object[2];// = oblArr;
                      spr.Items = oblArr.Obligation;//

                   }


//                    BIG_Obligation bobl = bop.BIG_Obligation;



                ()
                  spr. 

                  switch (bop.TypOperacji)
                  {
                      case 1: // add



                  }
                  */
                // najpierw usunięcia
                Utils.LogWriter("Before notify");
                if (notifySprLst != null && notifySprLst.Any())
                {

                    foreach (notifyInformationTypeEx inf in notifySprLst)
                    {
                        int oprNo;
                        if (inf.idOper > 0)
                        {
                            informationManagementOrderType ord = new informationManagementOrderType();

                            ord.ID = (++lp).ToString();
                            ord.Item = inf.notifyInfType;
                            oprNo = inf.idOper;

                            infoOrd.Add(ord);
                        }
                        else
                        {
                            oprNo = -inf.idOper;
                        }


                        BIG_Operacja boperac = context.BIG_Operacja.Where(a => a.BIG_OperacjaId == oprNo).FirstOrDefault();
                        if (boperac != null)
                            boperac.OrderOp = lp;
                    }
                    context.SaveChanges();
                }
                Utils.LogWriter("Before unsusepnd");
                if (unSuspendLst != null && unSuspendLst.Any())
                {

                    foreach (unSuspendInformationTypeEx inf in unSuspendLst)
                    {
                        int oprNo;
                        if (inf.idOper > 0)
                        {
                            informationManagementOrderType ord = new informationManagementOrderType();

                            ord.ID = (++lp).ToString();
                            ord.Item =  inf.unSusInfType;
                            oprNo = inf.idOper;
                            
                            infoOrd.Add(ord);
                        }
                        else
                        {
                            oprNo = -inf.idOper;
                        }


                        BIG_Operacja boperac = context.BIG_Operacja.Where(a => a.BIG_OperacjaId == oprNo).FirstOrDefault();
                        if (boperac != null)
                            boperac.OrderOp = lp;
                    }
                    context.SaveChanges();
                }
                Utils.LogWriter("Before suspend");
                if (suspendLst != null && suspendLst.Any())
                {

                    foreach (suspendInformationTypeEx inf in suspendLst)
                    {
                        int oprNo;
                        if (inf.idOper > 0)
                        {
                            informationManagementOrderType ord = new informationManagementOrderType();
                            entityManagementWithVerificationType unsusEnt = new entityManagementWithVerificationType();
                            ord.ID = (++lp).ToString();
                            unsusEnt = inf.susInfType;
                            ord.Item = unsusEnt; 
                            
                            oprNo = inf.idOper;
                            infoOrd.Add(ord);
                        }
                        else
                        {
                            oprNo = -inf.idOper;
                        }
                       
                       
                            BIG_Operacja boperac = context.BIG_Operacja.Where(a => a.BIG_OperacjaId == oprNo).FirstOrDefault();
                            if (boperac != null)
                                 boperac.OrderOp = lp;
                    }
                    context.SaveChanges();
                }

                Utils.LogWriter("Before remove");
                if (removeLst != null && removeLst.Any())
                {
             
                    foreach (removeInformationTypeEx inf in removeLst)
                    {
                        int oprNo;
                        if (inf.idOper > 0)
                        {
                            informationManagementOrderType ord = new informationManagementOrderType();
                            ord.ID = (++lp).ToString();
                            ord.Item = inf.reInfType;
                            oprNo = inf.idOper;
                           
                            infoOrd.Add(ord);
                        }
                        else
                        {
                            oprNo = -inf.idOper;
                        }
                        BIG_Operacja boperac = context.BIG_Operacja.Where(a => a.BIG_OperacjaId == oprNo).FirstOrDefault();
                        if (boperac != null)
                            boperac.OrderOp = lp;
                    }
                    context.SaveChanges();
                }

               


                Utils.LogWriter("Before add/update");
                if ((addCaseLst != null && addCaseLst.Any()) || (updtCaseLst != null && updtCaseLst.Any()))
            {
              

                input.Item = infoTyp;
              
                foreach (addCaseType ac in addCaseLst)
                {
                   List<object> lstout = ac.Items.Where(a => a != null).ToList();
                   ac.Items = lstout.ToArray();
                    informationManagementOrderType ord = new informationManagementOrderType();
                    ord.ID = (++lp).ToString();
                    addInformationType adInf = new addInformationType();
                    adInf.verifyResult = true;
                    adInf.userID = ac.Note;
                    
                    ord.Item = adInf;
                    adInf.Item = ac;
                    infoOrd.Add(ord);
                    List<BIG_Operacja> lstopr = new List<BIG_Operacja>();
                        lstopr = (from opr in context.BIG_Operacja join
                                       o in context.BIG_Obligation on opr.BIG_ObligationId equals o.BIG_ObligationId join
                                   cse in context.BIG_Case on o.BIG_CaseId equals cse.BIG_CaseId
                                  where opr.BIG_ImportId == IdIput && cse.CaseId == ac.Note select opr ).ToList();
                        foreach (BIG_Operacja oo in lstopr)
                        {
                            oo.OrderOp = Convert.ToInt32(ord.ID);

                        }




                    ac.Note = null;
                    }
                    Utils.LogWriter("Before update");
                    foreach (updateCaseType ac in updtCaseLst)
                    {
                        List<object> lstout = ac.Items.Where(a => a != null).ToList();
                        ac.Items = lstout.ToArray();

                        informationManagementOrderType ord = new informationManagementOrderType();
                        ord.ID = (++lp).ToString();
                        updateInformationType adInf = new updateInformationType();
                        adInf.verifyResult = true;
                        adInf.ID  = ac.Note;
                        adInf.IDType = idTypeEnum.UserId;
                        ord.Item = adInf;
                        adInf.Item = ac;
                        infoOrd.Add(ord);
                        List<BIG_Operacja> lstopr = new List<BIG_Operacja>();
                        lstopr = (from opr in context.BIG_Operacja
                                  join
      o in context.BIG_Obligation on opr.BIG_ObligationId equals o.BIG_ObligationId
                                  join
cse in context.BIG_Case on o.BIG_CaseId equals cse.BIG_CaseId
                                  where opr.BIG_ImportId == IdIput && cse.CaseId == ac.Note
                                  select opr).ToList();
// usunięcie należność


                        foreach (BIG_Operacja oo in lstopr)
                        {
                            oo.OrderOp = Convert.ToInt32(ord.ID);

                        }




                        ac.Note = null;
                    }
                    context.SaveChanges();
               
            }
                // teraz dodanie nowych należności 
                foreach (addObligationInformationTypeEx inf in addOblig)
                {
                     int oprNo;
                        if (inf.idOper > 0)
                        {
                                informationManagementOrderType ord = new informationManagementOrderType();
                                ord.ID = (++lp).ToString();
                                addInformationType adInf = new addInformationType();
                            adInf.Item = inf.addOblig;
                            ord.Item = adInf;
                            oprNo = inf.idOper;
                            infoOrd.Add(ord);
                        }
                        else
                        {
                            oprNo = -inf.idOper;
                        }


                        BIG_Operacja boperac = context.BIG_Operacja.Where(a => a.BIG_OperacjaId == oprNo).FirstOrDefault();
                        if (boperac != null)
                            boperac.OrderOp = lp;
                   


                }
                context.SaveChanges();

                infoTyp.Order = infoOrd.ToArray();
                input.Item = infoTyp;
            }
            Utils.LogWriter("At exit"); 
            return errFound ? null: input;
        }

    }
}