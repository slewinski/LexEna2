using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexEnaTrs.Web;
using LexEnaTrs.Web.BIG;
using System.Data.SqlClient;

namespace BIGImportBGWorker
{
    public class BIGProceed
    {
        private BIG_Job Bjob;
        private int BIGImportId= 0 ;
        private int rowsLimit = 3000;

        private List<ImportRow> jobRow2Import(List<BIG_JobRow> brows)
        {
            List<ImportRow> impLst = new List<ImportRow>();
            foreach (BIG_JobRow impR in brows)
            {
                ImportRow bjr = new ImportRow();

                bjr.Adr1 = impR.Adr1;
                bjr.Adr2 = impR.Adr2;
                bjr.czywyrok = impR.czywyrok;
                bjr.DataWymag = impR.DataWymag;
                bjr.DataWysWezw = impR.DataWysWezw;
                bjr.Dom = impR.Dom;
                bjr.dorzecz = impR.dorzecz;
                bjr.Firstname = impR.Firstname;
                bjr.Kod = impR.Kod;
                bjr.Lokal = impR.Lokal;
                bjr.Miejsce = impR.Miejsce;
                bjr.Name = impR.Name;
                bjr.Nip = impR.Nip;
                bjr.NrKlienta = impR.NrKlienta;
                bjr.Pesel = impR.Pesel;
                bjr.Saldo = impR.Saldo;
                bjr.Secondname = impR.Secondname;
                bjr.SygnOrzecz = impR.SygnOrzecz;
                bjr.SystemName = impR.SystemName;
                bjr.Title = impR.Title;
                bjr.Ulica = impR.Ulica;
                bjr.WienaSygn = impR.WienaSygn;
                bjr.BIG_JobRowId = impR.BIG_JobRowId;
                impLst.Add(bjr);

            }


            return impLst;
        }



      

        public BIGProceed(int IdJob, string option = "", int ImportId = 0 )
        {

            using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
            {
                if (option == "/S")
                {
                    BIGImportId = IdJob;
                }
                else if (option == "/U")
                {
                    BIGImportId = ImportId;
                    Bjob = context.BIG_Job.Where(a => a.BIG_JobId == IdJob).FirstOrDefault();
                    if (Bjob == null)
                    {

                        Utils.LogWriter("Brak zadania " + IdJob.ToString());
                        return;
                    }

                }
                else
                {
                    Bjob = context.BIG_Job.Where(a => a.BIG_JobId == IdJob).FirstOrDefault();
                    if (Bjob == null)
                    {

                        Utils.LogWriter("Brak zadania " + IdJob.ToString());
                        return;
                    }
                }
            }


            
            

        }

        public bool ProceedJob()
        {
            BIG_Import bimpo = null;
            if (this.BIGImportId == 0 || (this.BIGImportId > 0 && this.Bjob.BIG_JobId > 0 ) )
            {

                BIGFile bf = new BIGFile(Bjob.FName, Bjob.Name);
              


                int currPackage = 0;

                //   for (int pckNo = 1; ; pckNo++)
                //  {
                List<BIG_JobRow> lst = null;
                Utils.LogNamedWriter("Proceed JOB", "AsyncLog.Txt");
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {
                    //lst = context.BIG_JobRow.Where(a => a.BIG_JobId == this.Bjob.BIG_JobId && a.PartNo == pckNo).OrderBy(a => a.NrKlienta).ThenBy(a => a.DataWymag).ToList();
                    if (this.BIGImportId > 0)
                    {
                        lst = context.BIG_JobRow.Where(a => a.BIG_JobId == this.Bjob.BIG_JobId && a.Status == null).OrderBy(a => a.NrKlienta).ThenBy(a => a.DataWymag).ToList();
                        bimpo = context.BIG_Import.Where(a => a.BIG_ImportId == this.BIGImportId).FirstOrDefault();
                    }
                    else
                        // od nowa 
                        lst = context.BIG_JobRow.Where(a => a.BIG_JobId == this.Bjob.BIG_JobId).OrderBy(a => a.NrKlienta).ThenBy(a => a.DataWymag).ToList();
                    //if (lst == null || lst.Count == 0)
                    //        break;


                }
                //Utils.LogWriter("Pakiet nr" + pckNo.ToString() + " " + Bjob.FName);
                List<ImportRow> rows = this.jobRow2Import(lst);
                BIG_Import bimp = bf.ProceedStruct(rows,4,bimpo);
                if (bimp != null)
                    this.BIGImportId = bimp.BIG_ImportId;
                else 
                if (this.BIGImportId > 0)
                {
                    using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                    {
                        try
                        {
                            context.CommandTimeout = 1200;  // 20 minut
                            SqlParameter param1 = new SqlParameter("IdPakiet", this.BIGImportId);
                            SqlParameter param2 = new SqlParameter("IdJob", this.Bjob.BIG_JobId);

                            Utils.LogNamedWriter("Executing del CCnB", "AsyncLog.Txt");
                            object obj = context.ExecuteStoreQuery<object>("usp_DelCCnB  @IdPakiet, @IdJob", param1, param2).FirstOrDefault();
                            Utils.LogNamedWriter("Processing package finished", "AsyncLog.Txt");
                            BIG_Import bim = context.BIG_Import.Where(a => a.BIG_ImportId == this.BIGImportId).FirstOrDefault();
                            if (bim != null)
                            {
                                bim.StatOpis = "Pakiet gotowy do wysyłki";

                            }
                        }
                        catch (Exception e)
                        {
                            Utils.LogWriter("Błąd podczas wysyłki " + e.Message);
                            Utils.LogNamedWriter("Bląd procedury składowanej " + e.Message, "AsyncLog.Txt");
                        }
                    }
                }
            }

            if (this.BIGImportId > 0 )
            {
                if (bimpo == null)
                    splitPackage(this.BIGImportId);
                else
                {
                    int ile = 0;
                    using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                    {

                        ile = context.BIG_JobRow.Where(a => a.BIG_JobId == this.Bjob.BIG_JobId && a.Status == null).Count();

                    }

                    if (ile == 0)
                    {
                        splitPackage(this.BIGImportId);
                    }
                }

            }


            return true;
        }

        public bool splitPackage(int PackageId)
        {
            Utils.LogNamedWriter("Split ", "AsyncLog.Txt");
            BIG_Import bi = null;
            // dzielenie pakietu na mniejsze
            try {
                using (LexEnaMeritumEntities context = new LexEnaMeritumEntities())
                {


                    bi = context.BIG_Import.Where(a => a.BIG_ImportId == PackageId).FirstOrDefault();
                    if (bi == null)
                        return false;

                    List<BIG_Operacja> bopr = context.BIG_Operacja.Include("BIG_Case").Where(a => a.BIG_ImportId == PackageId).OrderByDescending(a => a.BIG_Case.CaseId).ToList();
                    if (bopr != null && bopr.Any())
                    {
                        string caseId = "";
                        int packageNo = 0;
                        string nkl = "";
                        int rowCount = 0;
                        BIG_Import bimnew = null;
                        List<int> IdLst = new List<int>();
                        foreach (BIG_Operacja b in bopr)
                        {
                            rowCount++;
                            if (!string.IsNullOrWhiteSpace(caseId) &&  caseId != b.BIG_Case.CaseId && rowCount > rowsLimit && IdLst.Count > 0 )
                            {
                                bimnew = new BIG_Import();
                                packageNo++;
                                bimnew.Username = bi.Username;
                                bimnew.Status = bi.Status; // dodano
                                bimnew.StatOpis = bi.StatOpis;
                                bimnew.lBlad = bi.lBlad;
                                bimnew.Filename = bi.Filename + "(" + packageNo.ToString() + ")";
                                bimnew.lSuccess = bi.lSuccess;
                                bimnew.lPoz = IdLst.Count();
                                bimnew.TypImp = bi.TypImp;
                                bimnew.DataImportu = bi.DataImportu;
                                context.BIG_Import.AddObject(bimnew);
                                context.SaveChanges();

                                string lst = string.Join("|", IdLst);

                                var @params = new[]{
                                 new SqlParameter("IdPakietIn", bi.BIG_ImportId),
                                 new SqlParameter("IdPakietOut", bimnew.BIG_ImportId),
                                 new SqlParameter("OperIdLst", lst)
                                };
                                SqlParameter param1 = new SqlParameter("IdPakietIn", bi.BIG_ImportId);
                                SqlParameter param2 = new SqlParameter("IdPakietOut", bimnew.BIG_ImportId);
                                SqlParameter param3 = new SqlParameter("OperIdLst", lst);
                                Utils.LogNamedWriter("Pack no " + packageNo.ToString(), "AsyncLog.Txt");
                                object obj = context.ExecuteStoreQuery<object>("USP_SwitchPackage  @IdPakietIn, @IdPakietOut, @OperIdLst", param1,param2, param3).FirstOrDefault();


                                IdLst = new List<int>();
                                rowCount = 0;
                            }
                            IdLst.Add(b.BIG_OperacjaId);
                            caseId = b.BIG_Case.CaseId;

                        }
                        // ostatniego nie przepisujemy 
                        packageNo++;
                        if (IdLst.Count > 0)
                        {
                            bi.Filename = bi.Filename + "(" + packageNo.ToString() + ")";
                        }
                        else
                            context.BIG_Import.DeleteObject(bi);
                        
                        context.SaveChanges();

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utils.LogNamedWriter(ex.Message, "AsyncLog.Txt");
                return false;

            }
                    // zmienić jeszcze 


            }


        

    }
}
