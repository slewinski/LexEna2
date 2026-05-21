
namespace LexEnaTrs.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.Data.Objects.DataClasses;


    // Implements application logic using the LexEnaMeritumEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class LexEnaMeritumDomainService : LinqToEntitiesDomainService<LexEnaMeritumEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'aspnet_Membership' query.
        public IQueryable<aspnet_Membership> GetAspnet_Membership()
        {
            return this.ObjectContext.aspnet_Membership;
        }

        public void InsertAspnet_Membership(aspnet_Membership aspnet_Membership)
        {
            if ((aspnet_Membership.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(aspnet_Membership, EntityState.Added);
            }
            else
            {
                this.ObjectContext.aspnet_Membership.AddObject(aspnet_Membership);
            }
        }

        public void UpdateAspnet_Membership(aspnet_Membership currentaspnet_Membership)
        {
            this.ObjectContext.aspnet_Membership.AttachAsModified(currentaspnet_Membership, this.ChangeSet.GetOriginal(currentaspnet_Membership));
        }

        public void DeleteAspnet_Membership(aspnet_Membership aspnet_Membership)
        {
            if ((aspnet_Membership.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(aspnet_Membership, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.aspnet_Membership.Attach(aspnet_Membership);
                this.ObjectContext.aspnet_Membership.DeleteObject(aspnet_Membership);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'aspnet_Users' query.
        public IQueryable<aspnet_Users> GetAspnet_Users()
        {
            return this.ObjectContext.aspnet_Users;
        }

        public void InsertAspnet_Users(aspnet_Users aspnet_Users)
        {
            if ((aspnet_Users.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(aspnet_Users, EntityState.Added);
            }
            else
            {
                this.ObjectContext.aspnet_Users.AddObject(aspnet_Users);
            }
        }

        public void UpdateAspnet_Users(aspnet_Users currentaspnet_Users)
        {
            this.ObjectContext.aspnet_Users.AttachAsModified(currentaspnet_Users, this.ChangeSet.GetOriginal(currentaspnet_Users));
        }

        public void DeleteAspnet_Users(aspnet_Users aspnet_Users)
        {
            if ((aspnet_Users.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(aspnet_Users, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.aspnet_Users.Attach(aspnet_Users);
                this.ObjectContext.aspnet_Users.DeleteObject(aspnet_Users);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DaneDluznika' query.
        public IQueryable<DaneDluznika> GetDaneDluznika()
        {
            return this.ObjectContext.DaneDluznika;
        }

        public void InsertDaneDluznika(DaneDluznika daneDluznika)
        {
            if ((daneDluznika.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(daneDluznika, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DaneDluznika.AddObject(daneDluznika);
            }
        }

        public void UpdateDaneDluznika(DaneDluznika currentDaneDluznika)
        {
            this.ObjectContext.DaneDluznika.AttachAsModified(currentDaneDluznika, this.ChangeSet.GetOriginal(currentDaneDluznika));
        }

        public void DeleteDaneDluznika(DaneDluznika daneDluznika)
        {
            if ((daneDluznika.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(daneDluznika, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DaneDluznika.Attach(daneDluznika);
                this.ObjectContext.DaneDluznika.DeleteObject(daneDluznika);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Dekretacja' query.
        public IQueryable<Dekretacja> GetDekretacja()
        {
            return this.ObjectContext.Dekretacja;
        }

        public void InsertDekretacja(Dekretacja dekretacja)
        {
            if ((dekretacja.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dekretacja, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Dekretacja.AddObject(dekretacja);
            }
        }

        public void UpdateDekretacja(Dekretacja currentDekretacja)
        {
            this.ObjectContext.Dekretacja.AttachAsModified(currentDekretacja, this.ChangeSet.GetOriginal(currentDekretacja));
        }

        public void DeleteDekretacja(Dekretacja dekretacja)
        {
            if ((dekretacja.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dekretacja, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Dekretacja.Attach(dekretacja);
                this.ObjectContext.Dekretacja.DeleteObject(dekretacja);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DokKomunikacja' query.
        public IQueryable<DokKomunikacja> GetDokKomunikacja()
        {
            return this.ObjectContext.DokKomunikacja;
        }

        public void InsertDokKomunikacja(DokKomunikacja dokKomunikacja)
        {
            if ((dokKomunikacja.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokKomunikacja, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DokKomunikacja.AddObject(dokKomunikacja);
            }
        }

        public void UpdateDokKomunikacja(DokKomunikacja currentDokKomunikacja)
        {
            this.ObjectContext.DokKomunikacja.AttachAsModified(currentDokKomunikacja, this.ChangeSet.GetOriginal(currentDokKomunikacja));
        }

        public void DeleteDokKomunikacja(DokKomunikacja dokKomunikacja)
        {
            if ((dokKomunikacja.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokKomunikacja, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DokKomunikacja.Attach(dokKomunikacja);
                this.ObjectContext.DokKomunikacja.DeleteObject(dokKomunikacja);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DokOdebr' query.
        public IQueryable<DokOdebr> GetDokOdebr()
        {
            return this.ObjectContext.DokOdebr;
        }

        public void InsertDokOdebr(DokOdebr dokOdebr)
        {
            if ((dokOdebr.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokOdebr, EntityState.Added);
            }
            else
            {

                
                    if (dokOdebr.KontoEPU_Id > 0)
                    {
                        PdfStore pdf = this.ObjectContext.PdfStore.Where(a => a.Id == dokOdebr.KontoEPU_Id).FirstOrDefault();
                        if (pdf != null)
                        {
                        dokOdebr.PdfStore = new EntityCollection<PdfStore>();
                        dokOdebr.PdfStore.Add(pdf);

                        }
                         // szukamy pdfa
                         dokOdebr.KontoEPU_Id = null;
                        // dodanie statusu 
                        if (dokOdebr.TypDok  ==  100005 || dokOdebr.TypDok == 105005 || dokOdebr.TypDok == 100017) // jełsi pozew złożony
                        {

                            StatusSprawy ss = this.ObjectContext.StatusSprawy.Where(a => a.Sprawa_id == dokOdebr.Sprawa_id && a.NazwaStatusu_Id == 5).FirstOrDefault();
                            if (ss == null)
                            { // dodajemy status sprawy
                                ss = new StatusSprawy();
                            switch (dokOdebr.TypDok)
                            {
                                case 105005:
                                case 100005:
                                    ss.NazwaStatusu_Id = 6;
                                    break;
                                case 100017:
                                    ss.NazwaStatusu_Id = 7;
                                    break;
                                    default:
                                    break;

                            }
                                ss.NazwaStatusu_Id = 5;
                                ss.Sprawa_id = dokOdebr.Sprawa_id;
                                ss.DataStatusu = DateTime.Today;
                                this.ObjectContext.StatusSprawy.AddObject(ss);
                            }
                        }
                    }


                    this.ObjectContext.DokOdebr.AddObject(dokOdebr);
            }
        }

        public void UpdateDokOdebr(DokOdebr currentDokOdebr)
        {
            this.ObjectContext.DokOdebr.AttachAsModified(currentDokOdebr, this.ChangeSet.GetOriginal(currentDokOdebr));
        }

        public void DeleteDokOdebr(DokOdebr dokOdebr)
        {
            if ((dokOdebr.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokOdebr, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DokOdebr.Attach(dokOdebr);
                this.ObjectContext.DokOdebr.DeleteObject(dokOdebr);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DokumentKomunikacjaEPU' query.
        public IQueryable<DokumentKomunikacjaEPU> GetDokumentKomunikacjaEPU()
        {
            return this.ObjectContext.DokumentKomunikacjaEPU;
        }

        public void InsertDokumentKomunikacjaEPU(DokumentKomunikacjaEPU dokumentKomunikacjaEPU)
        {
            if ((dokumentKomunikacjaEPU.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokumentKomunikacjaEPU, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DokumentKomunikacjaEPU.AddObject(dokumentKomunikacjaEPU);
            }
        }

        public void UpdateDokumentKomunikacjaEPU(DokumentKomunikacjaEPU currentDokumentKomunikacjaEPU)
        {
            this.ObjectContext.DokumentKomunikacjaEPU.AttachAsModified(currentDokumentKomunikacjaEPU, this.ChangeSet.GetOriginal(currentDokumentKomunikacjaEPU));
        }

        public void DeleteDokumentKomunikacjaEPU(DokumentKomunikacjaEPU dokumentKomunikacjaEPU)
        {
            if ((dokumentKomunikacjaEPU.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokumentKomunikacjaEPU, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DokumentKomunikacjaEPU.Attach(dokumentKomunikacjaEPU);
                this.ObjectContext.DokumentKomunikacjaEPU.DeleteObject(dokumentKomunikacjaEPU);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DokumentOutputElementModels' query.
        public IQueryable<DokumentOutputElementModels> GetDokumentOutputElementModels()
        {
            return this.ObjectContext.DokumentOutputElementModels;
        }

        public void InsertDokumentOutputElementModels(DokumentOutputElementModels dokumentOutputElementModels)
        {
            if ((dokumentOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokumentOutputElementModels, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DokumentOutputElementModels.AddObject(dokumentOutputElementModels);
            }
        }

        public void UpdateDokumentOutputElementModels(DokumentOutputElementModels currentDokumentOutputElementModels)
        {
            this.ObjectContext.DokumentOutputElementModels.AttachAsModified(currentDokumentOutputElementModels, this.ChangeSet.GetOriginal(currentDokumentOutputElementModels));
        }

        public void DeleteDokumentOutputElementModels(DokumentOutputElementModels dokumentOutputElementModels)
        {
            if ((dokumentOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokumentOutputElementModels, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DokumentOutputElementModels.Attach(dokumentOutputElementModels);
                this.ObjectContext.DokumentOutputElementModels.DeleteObject(dokumentOutputElementModels);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DokumentPaczka' query.
        public IQueryable<DokumentPaczka> GetDokumentPaczka()
        {
            return this.ObjectContext.DokumentPaczka;
        }

        public void InsertDokumentPaczka(DokumentPaczka dokumentPaczka)
        {
            if ((dokumentPaczka.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokumentPaczka, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DokumentPaczka.AddObject(dokumentPaczka);
            }
        }

        public void UpdateDokumentPaczka(DokumentPaczka currentDokumentPaczka)
        {
            this.ObjectContext.DokumentPaczka.AttachAsModified(currentDokumentPaczka, this.ChangeSet.GetOriginal(currentDokumentPaczka));
        }

        public void DeleteDokumentPaczka(DokumentPaczka dokumentPaczka)
        {
            if ((dokumentPaczka.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokumentPaczka, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DokumentPaczka.Attach(dokumentPaczka);
                this.ObjectContext.DokumentPaczka.DeleteObject(dokumentPaczka);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DokumentWysKomunikacjaEPU' query.
        public IQueryable<DokumentWysKomunikacjaEPU> GetDokumentWysKomunikacjaEPU()
        {
            this.ObjectContext.CommandTimeout = 180;
            return this.ObjectContext.DokumentWysKomunikacjaEPU;
        }

        public void InsertDokumentWysKomunikacjaEPU(DokumentWysKomunikacjaEPU dokumentWysKomunikacjaEPU)
        {
            if ((dokumentWysKomunikacjaEPU.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokumentWysKomunikacjaEPU, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DokumentWysKomunikacjaEPU.AddObject(dokumentWysKomunikacjaEPU);
            }
        }

        public void UpdateDokumentWysKomunikacjaEPU(DokumentWysKomunikacjaEPU currentDokumentWysKomunikacjaEPU)
        {
            this.ObjectContext.DokumentWysKomunikacjaEPU.AttachAsModified(currentDokumentWysKomunikacjaEPU, this.ChangeSet.GetOriginal(currentDokumentWysKomunikacjaEPU));
        }

        public void DeleteDokumentWysKomunikacjaEPU(DokumentWysKomunikacjaEPU dokumentWysKomunikacjaEPU)
        {
            if ((dokumentWysKomunikacjaEPU.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokumentWysKomunikacjaEPU, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DokumentWysKomunikacjaEPU.Attach(dokumentWysKomunikacjaEPU);
                this.ObjectContext.DokumentWysKomunikacjaEPU.DeleteObject(dokumentWysKomunikacjaEPU);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DokWys' query.
        public IQueryable<DokWys> GetDokWys()
        {
            return this.ObjectContext.DokWys;
        }

        public void InsertDokWys(DokWys dokWys)
        {
            if ((dokWys.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokWys, EntityState.Added);
            }
            else
            {
                if (dokWys.KontoEPU_Id > 0)
                {
                    PdfStore pdf = this.ObjectContext.PdfStore.Where(a => a.Id == dokWys.KontoEPU_Id).FirstOrDefault();
                    if (pdf != null)
                    {
                        dokWys.PdfStore = new EntityCollection<PdfStore>();
                        dokWys.PdfStore.Add(pdf);

                    }
                    // szukamy pdfa
                    dokWys.KontoEPU_Id = null;
                    // dodanie statusu 
                    if (dokWys.TypDok == 100010) // jełsi pozew złożony
                    {

                        StatusSprawy ss = this.ObjectContext.StatusSprawy.Where(a => a.Sprawa_id == dokWys.Sprawa_id && a.NazwaStatusu_Id == 5).FirstOrDefault();
                        if (ss == null)
                        { // dodajemy status sprawy
                            ss = new StatusSprawy();
                            ss.NazwaStatusu_Id = 5;
                            ss.Sprawa_id = dokWys.Sprawa_id;
                            ss.DataStatusu = DateTime.Today;
                            this.ObjectContext.StatusSprawy.AddObject(ss);
                        }
                    }
                }

                /*
                if (dokWys.Sprawa_id > 0)
                {
                    Sprawa spr = this.ObjectContext.Sprawa.Where(a => a.id == dokWys.Sprawa_id).FirstOrDefault();
                    if (spr != null)
                    {
                        spr.DokWys.Add(dokWys);
                        dokWys.Sprawa_id = null;    

                    }
                    // szukamy pdfa
                   

                }
                */
              
                this.ObjectContext.DokWys.AddObject(dokWys);
            }
        }

        public void UpdateDokWys(DokWys currentDokWys)
        {

            

           
            this.ObjectContext.DokWys.AttachAsModified(currentDokWys, this.ChangeSet.GetOriginal(currentDokWys));
        }

        public void DeleteDokWys(DokWys dokWys)
        {
            if ((dokWys.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dokWys, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DokWys.Attach(dokWys);
                this.ObjectContext.DokWys.DeleteObject(dokWys);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DoreczenieOutputElementModels' query.
        public IQueryable<DoreczenieOutputElementModels> GetDoreczenieOutputElementModels()
        {
            return this.ObjectContext.DoreczenieOutputElementModels;
        }

        public void InsertDoreczenieOutputElementModels(DoreczenieOutputElementModels doreczenieOutputElementModels)
        {
            if ((doreczenieOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(doreczenieOutputElementModels, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DoreczenieOutputElementModels.AddObject(doreczenieOutputElementModels);
            }
        }

        public void UpdateDoreczenieOutputElementModels(DoreczenieOutputElementModels currentDoreczenieOutputElementModels)
        {
            this.ObjectContext.DoreczenieOutputElementModels.AttachAsModified(currentDoreczenieOutputElementModels, this.ChangeSet.GetOriginal(currentDoreczenieOutputElementModels));
        }

        public void DeleteDoreczenieOutputElementModels(DoreczenieOutputElementModels doreczenieOutputElementModels)
        {
            if ((doreczenieOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(doreczenieOutputElementModels, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DoreczenieOutputElementModels.Attach(doreczenieOutputElementModels);
                this.ObjectContext.DoreczenieOutputElementModels.DeleteObject(doreczenieOutputElementModels);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DoreczenieVer2OutputElementModel' query.
        public IQueryable<DoreczenieVer2OutputElementModel> GetDoreczenieVer2OutputElementModel()
        {
            return this.ObjectContext.DoreczenieVer2OutputElementModel;
        }

        public void InsertDoreczenieVer2OutputElementModel(DoreczenieVer2OutputElementModel doreczenieVer2OutputElementModel)
        {
            if ((doreczenieVer2OutputElementModel.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(doreczenieVer2OutputElementModel, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DoreczenieVer2OutputElementModel.AddObject(doreczenieVer2OutputElementModel);
            }
        }

        public void UpdateDoreczenieVer2OutputElementModel(DoreczenieVer2OutputElementModel currentDoreczenieVer2OutputElementModel)
        {
            this.ObjectContext.DoreczenieVer2OutputElementModel.AttachAsModified(currentDoreczenieVer2OutputElementModel, this.ChangeSet.GetOriginal(currentDoreczenieVer2OutputElementModel));
        }

        public void DeleteDoreczenieVer2OutputElementModel(DoreczenieVer2OutputElementModel doreczenieVer2OutputElementModel)
        {
            if ((doreczenieVer2OutputElementModel.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(doreczenieVer2OutputElementModel, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DoreczenieVer2OutputElementModel.Attach(doreczenieVer2OutputElementModel);
                this.ObjectContext.DoreczenieVer2OutputElementModel.DeleteObject(doreczenieVer2OutputElementModel);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'EdmMetadata' query.
        public IQueryable<EdmMetadata> GetEdmMetadata()
        {
            return this.ObjectContext.EdmMetadata;
        }

        public void InsertEdmMetadata(EdmMetadata edmMetadata)
        {
            if ((edmMetadata.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(edmMetadata, EntityState.Added);
            }
            else
            {
                this.ObjectContext.EdmMetadata.AddObject(edmMetadata);
            }
        }

        public void UpdateEdmMetadata(EdmMetadata currentEdmMetadata)
        {
            this.ObjectContext.EdmMetadata.AttachAsModified(currentEdmMetadata, this.ChangeSet.GetOriginal(currentEdmMetadata));
        }

        public void DeleteEdmMetadata(EdmMetadata edmMetadata)
        {
            if ((edmMetadata.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(edmMetadata, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.EdmMetadata.Attach(edmMetadata);
                this.ObjectContext.EdmMetadata.DeleteObject(edmMetadata);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'HistoriaSprawyOutputElementModels' query.
        public IQueryable<HistoriaSprawyOutputElementModels> GetHistoriaSprawyOutputElementModels()
        {
            return this.ObjectContext.HistoriaSprawyOutputElementModels;
        }

        public void InsertHistoriaSprawyOutputElementModels(HistoriaSprawyOutputElementModels historiaSprawyOutputElementModels)
        {
            if ((historiaSprawyOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(historiaSprawyOutputElementModels, EntityState.Added);
            }
            else
            {
                this.ObjectContext.HistoriaSprawyOutputElementModels.AddObject(historiaSprawyOutputElementModels);
            }
        }

        public void UpdateHistoriaSprawyOutputElementModels(HistoriaSprawyOutputElementModels currentHistoriaSprawyOutputElementModels)
        {
            this.ObjectContext.HistoriaSprawyOutputElementModels.AttachAsModified(currentHistoriaSprawyOutputElementModels, this.ChangeSet.GetOriginal(currentHistoriaSprawyOutputElementModels));
        }

        public void DeleteHistoriaSprawyOutputElementModels(HistoriaSprawyOutputElementModels historiaSprawyOutputElementModels)
        {
            if ((historiaSprawyOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(historiaSprawyOutputElementModels, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.HistoriaSprawyOutputElementModels.Attach(historiaSprawyOutputElementModels);
                this.ObjectContext.HistoriaSprawyOutputElementModels.DeleteObject(historiaSprawyOutputElementModels);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'JednostkaOrg' query.
        public IQueryable<JednostkaOrg> GetJednostkaOrg()
        {
            return this.ObjectContext.JednostkaOrg;
        }

        public void InsertJednostkaOrg(JednostkaOrg jednostkaOrg)
        {
            if ((jednostkaOrg.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(jednostkaOrg, EntityState.Added);
            }
            else
            {
                this.ObjectContext.JednostkaOrg.AddObject(jednostkaOrg);
            }
        }

        public void UpdateJednostkaOrg(JednostkaOrg currentJednostkaOrg)
        {
            this.ObjectContext.JednostkaOrg.AttachAsModified(currentJednostkaOrg, this.ChangeSet.GetOriginal(currentJednostkaOrg));
        }

        public void DeleteJednostkaOrg(JednostkaOrg jednostkaOrg)
        {
            if ((jednostkaOrg.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(jednostkaOrg, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.JednostkaOrg.Attach(jednostkaOrg);
                this.ObjectContext.JednostkaOrg.DeleteObject(jednostkaOrg);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'JednostkaWindykacji' query.
        public IQueryable<JednostkaWindykacji> GetJednostkaWindykacji()
        {
            return this.ObjectContext.JednostkaWindykacji;
        }

        public JednostkaWindykacji GetJednostkaWindykacjiById(int Id)
        {
            return this.ObjectContext.JednostkaWindykacji.Where(a=>a.Id == Id).FirstOrDefault();
        }

        public void InsertJednostkaWindykacji(JednostkaWindykacji jednostkaWindykacji)
        {
            if ((jednostkaWindykacji.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(jednostkaWindykacji, EntityState.Added);
            }
            else
            {
                this.ObjectContext.JednostkaWindykacji.AddObject(jednostkaWindykacji);
            }
        }

        public void UpdateJednostkaWindykacji(JednostkaWindykacji currentJednostkaWindykacji)
        {
            this.ObjectContext.JednostkaWindykacji.AttachAsModified(currentJednostkaWindykacji, this.ChangeSet.GetOriginal(currentJednostkaWindykacji));
        }

        public void DeleteJednostkaWindykacji(JednostkaWindykacji jednostkaWindykacji)
        {
            if ((jednostkaWindykacji.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(jednostkaWindykacji, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.JednostkaWindykacji.Attach(jednostkaWindykacji);
                this.ObjectContext.JednostkaWindykacji.DeleteObject(jednostkaWindykacji);
            }
        }

        public IQueryable<KancelariaKomornicza> GetKancelariaKomornicza()
        {
            return this.ObjectContext.KancelariaKomornicza;
        }

        public void InsertKancelariaKomornicza(KancelariaKomornicza kancelariaKomornicza)
        {
            if ((kancelariaKomornicza.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(kancelariaKomornicza, EntityState.Added);
            }
            else
            {
                this.ObjectContext.KancelariaKomornicza.AddObject(kancelariaKomornicza);
            }
        }

        public void UpdateKancelariaKomornicza(KancelariaKomornicza currentKancelariaKomornicza)
        {
            this.ObjectContext.KancelariaKomornicza.AttachAsModified(currentKancelariaKomornicza, this.ChangeSet.GetOriginal(currentKancelariaKomornicza));
        }

        public void DeleteKancelariaKomornicza(KancelariaKomornicza kancelariaKomornicza)
        {
            if ((kancelariaKomornicza.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(kancelariaKomornicza, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.KancelariaKomornicza.Attach(kancelariaKomornicza);
                this.ObjectContext.KancelariaKomornicza.DeleteObject(kancelariaKomornicza);
            }
        }
        
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'KontoEPU' query.
        public IQueryable<KontoEPU> GetKontoEPU()
        {
            return this.ObjectContext.KontoEPU;
        }

        public void InsertKontoEPU(KontoEPU kontoEPU)
        {
            if ((kontoEPU.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(kontoEPU, EntityState.Added);
            }
            else
            {
                this.ObjectContext.KontoEPU.AddObject(kontoEPU);
            }
        }

        public void UpdateKontoEPU(KontoEPU currentKontoEPU)
        {
            this.ObjectContext.KontoEPU.AttachAsModified(currentKontoEPU, this.ChangeSet.GetOriginal(currentKontoEPU));
        }

        public void DeleteKontoEPU(KontoEPU kontoEPU)
        {
            if ((kontoEPU.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(kontoEPU, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.KontoEPU.Attach(kontoEPU);
                this.ObjectContext.KontoEPU.DeleteObject(kontoEPU);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'NakazOutputElementModels' query.
        public IQueryable<NakazOutputElementModels> GetNakazOutputElementModels()
        {
            return this.ObjectContext.NakazOutputElementModels;
        }

        public void InsertNakazOutputElementModels(NakazOutputElementModels nakazOutputElementModels)
        {
            if ((nakazOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(nakazOutputElementModels, EntityState.Added);
            }
            else
            {
                this.ObjectContext.NakazOutputElementModels.AddObject(nakazOutputElementModels);
            }
        }

        public void UpdateNakazOutputElementModels(NakazOutputElementModels currentNakazOutputElementModels)
        {
            this.ObjectContext.NakazOutputElementModels.AttachAsModified(currentNakazOutputElementModels, this.ChangeSet.GetOriginal(currentNakazOutputElementModels));
        }

        public void DeleteNakazOutputElementModels(NakazOutputElementModels nakazOutputElementModels)
        {
            if ((nakazOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(nakazOutputElementModels, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.NakazOutputElementModels.Attach(nakazOutputElementModels);
                this.ObjectContext.NakazOutputElementModels.DeleteObject(nakazOutputElementModels);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Naleznosc' query.
        public IQueryable<Naleznosc> GetNaleznosc()
        {
            return this.ObjectContext.Naleznosc;
        }

        public void InsertNaleznosc(Naleznosc naleznosc)
        {
            if ((naleznosc.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(naleznosc, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Naleznosc.AddObject(naleznosc);
            }
        }

        public void UpdateNaleznosc(Naleznosc currentNaleznosc)
        {
            this.ObjectContext.Naleznosc.AttachAsModified(currentNaleznosc, this.ChangeSet.GetOriginal(currentNaleznosc));
        }

        public void DeleteNaleznosc(Naleznosc naleznosc)
        {
            if ((naleznosc.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(naleznosc, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Naleznosc.Attach(naleznosc);
                this.ObjectContext.Naleznosc.DeleteObject(naleznosc);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'NazwaStatusu' query.
        public IQueryable<NazwaStatusu> GetNazwaStatusu()
        {
            return this.ObjectContext.NazwaStatusu;
        }

        public void InsertNazwaStatusu(NazwaStatusu nazwaStatusu)
        {
            if ((nazwaStatusu.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(nazwaStatusu, EntityState.Added);
            }
            else
            {
                this.ObjectContext.NazwaStatusu.AddObject(nazwaStatusu);
            }
        }

        public void UpdateNazwaStatusu(NazwaStatusu currentNazwaStatusu)
        {
            this.ObjectContext.NazwaStatusu.AttachAsModified(currentNazwaStatusu, this.ChangeSet.GetOriginal(currentNazwaStatusu));
        }

        public void DeleteNazwaStatusu(NazwaStatusu nazwaStatusu)
        {
            if ((nazwaStatusu.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(nazwaStatusu, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.NazwaStatusu.Attach(nazwaStatusu);
                this.ObjectContext.NazwaStatusu.DeleteObject(nazwaStatusu);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'NazwyOdsetek' query.
        public IQueryable<NazwyOdsetek> GetNazwyOdsetek()
        {
            return this.ObjectContext.NazwyOdsetek.Where(a=>a.PartitionKey == 1 );
        }

        public void InsertNazwyOdsetek(NazwyOdsetek nazwyOdsetek)
        {
            if ((nazwyOdsetek.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(nazwyOdsetek, EntityState.Added);
            }
            else
            {
                this.ObjectContext.NazwyOdsetek.AddObject(nazwyOdsetek);
            }
        }

        public void UpdateNazwyOdsetek(NazwyOdsetek currentNazwyOdsetek)
        {
            this.ObjectContext.NazwyOdsetek.AttachAsModified(currentNazwyOdsetek, this.ChangeSet.GetOriginal(currentNazwyOdsetek));
        }

        public void DeleteNazwyOdsetek(NazwyOdsetek nazwyOdsetek)
        {
            if ((nazwyOdsetek.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(nazwyOdsetek, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.NazwyOdsetek.Attach(nazwyOdsetek);
                this.ObjectContext.NazwyOdsetek.DeleteObject(nazwyOdsetek);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Odsetki' query.
        public IQueryable<Odsetki> GetOdsetki()
        {
            return this.ObjectContext.Odsetki;
        }

        public void InsertOdsetki(Odsetki odsetki)
        {
            if ((odsetki.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(odsetki, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Odsetki.AddObject(odsetki);
            }
        }

        public void UpdateOdsetki(Odsetki currentOdsetki)
        {
            this.ObjectContext.Odsetki.AttachAsModified(currentOdsetki, this.ChangeSet.GetOriginal(currentOdsetki));
        }

        public void DeleteOdsetki(Odsetki odsetki)
        {
            if ((odsetki.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(odsetki, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Odsetki.Attach(odsetki);
                this.ObjectContext.Odsetki.DeleteObject(odsetki);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'OdsTab' query.
        public IQueryable<OdsTab> GetOdsTab()
        {
            return this.ObjectContext.OdsTab;
        }

        public void InsertOdsTab(OdsTab odsTab)
        {
            if ((odsTab.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(odsTab, EntityState.Added);
            }
            else
            {
                this.ObjectContext.OdsTab.AddObject(odsTab);
            }
        }

        public void UpdateOdsTab(OdsTab currentOdsTab)
        {
            this.ObjectContext.OdsTab.AttachAsModified(currentOdsTab, this.ChangeSet.GetOriginal(currentOdsTab));
        }

        public void DeleteOdsTab(OdsTab odsTab)
        {
            if ((odsTab.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(odsTab, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.OdsTab.Attach(odsTab);
                this.ObjectContext.OdsTab.DeleteObject(odsTab);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'OrzeczenieOutputElementModels' query.
        public IQueryable<OrzeczenieOutputElementModels> GetOrzeczenieOutputElementModels()
        {
            return this.ObjectContext.OrzeczenieOutputElementModels;
        }

        public void InsertOrzeczenieOutputElementModels(OrzeczenieOutputElementModels orzeczenieOutputElementModels)
        {
            if ((orzeczenieOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(orzeczenieOutputElementModels, EntityState.Added);
            }
            else
            {
                this.ObjectContext.OrzeczenieOutputElementModels.AddObject(orzeczenieOutputElementModels);
            }
        }

        public void UpdateOrzeczenieOutputElementModels(OrzeczenieOutputElementModels currentOrzeczenieOutputElementModels)
        {
            this.ObjectContext.OrzeczenieOutputElementModels.AttachAsModified(currentOrzeczenieOutputElementModels, this.ChangeSet.GetOriginal(currentOrzeczenieOutputElementModels));
        }

        public void DeleteOrzeczenieOutputElementModels(OrzeczenieOutputElementModels orzeczenieOutputElementModels)
        {
            if ((orzeczenieOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(orzeczenieOutputElementModels, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.OrzeczenieOutputElementModels.Attach(orzeczenieOutputElementModels);
                this.ObjectContext.OrzeczenieOutputElementModels.DeleteObject(orzeczenieOutputElementModels);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'OrzeczenieVer2OutputElementModel' query.
        public IQueryable<OrzeczenieVer2OutputElementModel> GetOrzeczenieVer2OutputElementModel()
        {
            return this.ObjectContext.OrzeczenieVer2OutputElementModel;
        }

        public void InsertOrzeczenieVer2OutputElementModel(OrzeczenieVer2OutputElementModel orzeczenieVer2OutputElementModel)
        {
            if ((orzeczenieVer2OutputElementModel.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(orzeczenieVer2OutputElementModel, EntityState.Added);
            }
            else
            {
                this.ObjectContext.OrzeczenieVer2OutputElementModel.AddObject(orzeczenieVer2OutputElementModel);
            }
        }

        public void UpdateOrzeczenieVer2OutputElementModel(OrzeczenieVer2OutputElementModel currentOrzeczenieVer2OutputElementModel)
        {
            this.ObjectContext.OrzeczenieVer2OutputElementModel.AttachAsModified(currentOrzeczenieVer2OutputElementModel, this.ChangeSet.GetOriginal(currentOrzeczenieVer2OutputElementModel));
        }

        public void DeleteOrzeczenieVer2OutputElementModel(OrzeczenieVer2OutputElementModel orzeczenieVer2OutputElementModel)
        {
            if ((orzeczenieVer2OutputElementModel.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(orzeczenieVer2OutputElementModel, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.OrzeczenieVer2OutputElementModel.Attach(orzeczenieVer2OutputElementModel);
                this.ObjectContext.OrzeczenieVer2OutputElementModel.DeleteObject(orzeczenieVer2OutputElementModel);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Paczka' query.
        public IQueryable<Paczka> GetPaczka()
        {
            return this.ObjectContext.Paczka;
        }

        public void InsertPaczka(Paczka paczka)
        {
            if ((paczka.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(paczka, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Paczka.AddObject(paczka);
            }
        }

        public void UpdatePaczka(Paczka currentPaczka)
        {
            this.ObjectContext.Paczka.AttachAsModified(currentPaczka, this.ChangeSet.GetOriginal(currentPaczka));
        }

        public void DeletePaczka(Paczka paczka)
        {
            if ((paczka.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(paczka, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Paczka.Attach(paczka);
                this.ObjectContext.Paczka.DeleteObject(paczka);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'PaczkaKomunikacja' query.
        public IQueryable<PaczkaKomunikacja> GetPaczkaKomunikacja()
        {
            return this.ObjectContext.PaczkaKomunikacja;
        }

        public void InsertPaczkaKomunikacja(PaczkaKomunikacja paczkaKomunikacja)
        {
            if ((paczkaKomunikacja.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(paczkaKomunikacja, EntityState.Added);
            }
            else
            {
                this.ObjectContext.PaczkaKomunikacja.AddObject(paczkaKomunikacja);
            }
        }

        public void UpdatePaczkaKomunikacja(PaczkaKomunikacja currentPaczkaKomunikacja)
        {
            this.ObjectContext.PaczkaKomunikacja.AttachAsModified(currentPaczkaKomunikacja, this.ChangeSet.GetOriginal(currentPaczkaKomunikacja));
        }

        public void DeletePaczkaKomunikacja(PaczkaKomunikacja paczkaKomunikacja)
        {
            if ((paczkaKomunikacja.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(paczkaKomunikacja, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.PaczkaKomunikacja.Attach(paczkaKomunikacja);
                this.ObjectContext.PaczkaKomunikacja.DeleteObject(paczkaKomunikacja);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Pozew' query.
        public IQueryable<Pozew> GetPozew()
        {
            return this.ObjectContext.Pozew;
        }

        public void InsertPozew(Pozew pozew)
        {
            if ((pozew.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(pozew, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Pozew.AddObject(pozew);
            }
        }

        public void UpdatePozew(Pozew currentPozew)
        {
            this.ObjectContext.Pozew.AttachAsModified(currentPozew, this.ChangeSet.GetOriginal(currentPozew));
        }

        public void DeletePozew(Pozew pozew)
        {
            if ((pozew.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(pozew, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Pozew.Attach(pozew);
                this.ObjectContext.Pozew.DeleteObject(pozew);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Radca' query.
        public IQueryable<Radca> GetRadca()
        {
            return this.ObjectContext.Radca;
        }

        public void InsertRadca(Radca radca)
        {
            if ((radca.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(radca, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Radca.AddObject(radca);
            }
        }

        public void UpdateRadca(Radca currentRadca)
        {
            this.ObjectContext.Radca.AttachAsModified(currentRadca, this.ChangeSet.GetOriginal(currentRadca));
        }

        public void DeleteRadca(Radca radca)
        {
            if ((radca.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(radca, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Radca.Attach(radca);
                this.ObjectContext.Radca.DeleteObject(radca);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Slownik' query.
        public IQueryable<Slownik> GetSlownik()
        {
            return this.ObjectContext.Slownik;
        }

        public void InsertSlownik(Slownik slownik)
        {
            if ((slownik.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(slownik, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Slownik.AddObject(slownik);
            }
        }

        public void UpdateSlownik(Slownik currentSlownik)
        {
            this.ObjectContext.Slownik.AttachAsModified(currentSlownik, this.ChangeSet.GetOriginal(currentSlownik));
        }

        public void DeleteSlownik(Slownik slownik)
        {
            if ((slownik.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(slownik, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Slownik.Attach(slownik);
                this.ObjectContext.Slownik.DeleteObject(slownik);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SposobyEgzSlownik' query.
        public IQueryable<SposobyEgzSlownik> GetSposobyEgzSlownik()
        {
            return this.ObjectContext.SposobyEgzSlownik;
        }

        public void InsertSposobyEgzSlownik(SposobyEgzSlownik sposobyEgzSlownik)
        {
            if ((sposobyEgzSlownik.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sposobyEgzSlownik, EntityState.Added);
            }
            else
            {
                this.ObjectContext.SposobyEgzSlownik.AddObject(sposobyEgzSlownik);
            }
        }

        public void UpdateSposobyEgzSlownik(SposobyEgzSlownik currentSposobyEgzSlownik)
        {
            this.ObjectContext.SposobyEgzSlownik.AttachAsModified(currentSposobyEgzSlownik, this.ChangeSet.GetOriginal(currentSposobyEgzSlownik));
        }

        public void DeleteSposobyEgzSlownik(SposobyEgzSlownik sposobyEgzSlownik)
        {
            if ((sposobyEgzSlownik.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sposobyEgzSlownik, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.SposobyEgzSlownik.Attach(sposobyEgzSlownik);
                this.ObjectContext.SposobyEgzSlownik.DeleteObject(sposobyEgzSlownik);
            }
        }
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Sprawa' query.
        public IQueryable<Sprawa> GetSprawa()
        {
            return this.ObjectContext.Sprawa;
        }

        public void InsertSprawa(Sprawa sprawa)
        {
            if ((sprawa.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sprawa, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Sprawa.AddObject(sprawa);
            }
        }

        public void UpdateSprawa(Sprawa currentSprawa)
        {
            this.ObjectContext.Sprawa.AttachAsModified(currentSprawa, this.ChangeSet.GetOriginal(currentSprawa));
        }

        public void DeleteSprawa(Sprawa sprawa)
        {
            if ((sprawa.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sprawa, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Sprawa.Attach(sprawa);
                this.ObjectContext.Sprawa.DeleteObject(sprawa);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SprawaKomunikacjaEPU' query.
        public IQueryable<SprawaKomunikacjaEPU> GetSprawaKomunikacjaEPU()
        {
            return this.ObjectContext.SprawaKomunikacjaEPU;
        }

        public void InsertSprawaKomunikacjaEPU(SprawaKomunikacjaEPU sprawaKomunikacjaEPU)
        {
            if ((sprawaKomunikacjaEPU.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sprawaKomunikacjaEPU, EntityState.Added);
            }
            else
            {
                this.ObjectContext.SprawaKomunikacjaEPU.AddObject(sprawaKomunikacjaEPU);
            }
        }

        public void UpdateSprawaKomunikacjaEPU(SprawaKomunikacjaEPU currentSprawaKomunikacjaEPU)
        {
            this.ObjectContext.SprawaKomunikacjaEPU.AttachAsModified(currentSprawaKomunikacjaEPU, this.ChangeSet.GetOriginal(currentSprawaKomunikacjaEPU));
        }

        public void DeleteSprawaKomunikacjaEPU(SprawaKomunikacjaEPU sprawaKomunikacjaEPU)
        {
            if ((sprawaKomunikacjaEPU.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sprawaKomunikacjaEPU, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.SprawaKomunikacjaEPU.Attach(sprawaKomunikacjaEPU);
                this.ObjectContext.SprawaKomunikacjaEPU.DeleteObject(sprawaKomunikacjaEPU);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SprawaOutputElementModels' query.
        public IQueryable<SprawaOutputElementModels> GetSprawaOutputElementModels()
        {
            return this.ObjectContext.SprawaOutputElementModels;
        }

        public void InsertSprawaOutputElementModels(SprawaOutputElementModels sprawaOutputElementModels)
        {
            if ((sprawaOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sprawaOutputElementModels, EntityState.Added);
            }
            else
            {
                this.ObjectContext.SprawaOutputElementModels.AddObject(sprawaOutputElementModels);
            }
        }

        public void UpdateSprawaOutputElementModels(SprawaOutputElementModels currentSprawaOutputElementModels)
        {
            this.ObjectContext.SprawaOutputElementModels.AttachAsModified(currentSprawaOutputElementModels, this.ChangeSet.GetOriginal(currentSprawaOutputElementModels));
        }

        public void DeleteSprawaOutputElementModels(SprawaOutputElementModels sprawaOutputElementModels)
        {
            if ((sprawaOutputElementModels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sprawaOutputElementModels, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.SprawaOutputElementModels.Attach(sprawaOutputElementModels);
                this.ObjectContext.SprawaOutputElementModels.DeleteObject(sprawaOutputElementModels);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'StanNaleznosci' query.
        public IQueryable<StanNaleznosci> GetStanNaleznosci()
        {
            return this.ObjectContext.StanNaleznosci;
        }

        public void InsertStanNaleznosci(StanNaleznosci stanNaleznosci)
        {
            if ((stanNaleznosci.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(stanNaleznosci, EntityState.Added);
            }
            else
            {
                this.ObjectContext.StanNaleznosci.AddObject(stanNaleznosci);
            }
        }

        public void UpdateStanNaleznosci(StanNaleznosci currentStanNaleznosci)
        {
            this.ObjectContext.StanNaleznosci.AttachAsModified(currentStanNaleznosci, this.ChangeSet.GetOriginal(currentStanNaleznosci));
        }

        public void DeleteStanNaleznosci(StanNaleznosci stanNaleznosci)
        {
            if ((stanNaleznosci.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(stanNaleznosci, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.StanNaleznosci.Attach(stanNaleznosci);
                this.ObjectContext.StanNaleznosci.DeleteObject(stanNaleznosci);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'StanSprawy' query.
        public IQueryable<StanSprawy> GetStanSprawy()
        {
            return this.ObjectContext.StanSprawy;
        }

        public void InsertStanSprawy(StanSprawy stanSprawy)
        {
            if ((stanSprawy.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(stanSprawy, EntityState.Added);
            }
            else
            {
                this.ObjectContext.StanSprawy.AddObject(stanSprawy);
            }
        }

        public void UpdateStanSprawy(StanSprawy currentStanSprawy)
        {
            this.ObjectContext.StanSprawy.AttachAsModified(currentStanSprawy, this.ChangeSet.GetOriginal(currentStanSprawy));
        }

        public void DeleteStanSprawy(StanSprawy stanSprawy)
        {
            if ((stanSprawy.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(stanSprawy, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.StanSprawy.Attach(stanSprawy);
                this.ObjectContext.StanSprawy.DeleteObject(stanSprawy);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'StatusSprawy' query.
        public IQueryable<StatusSprawy> GetStatusSprawy()
        {
            return this.ObjectContext.StatusSprawy;
        }

        public void InsertStatusSprawy(StatusSprawy statusSprawy)
        {
            if ((statusSprawy.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(statusSprawy, EntityState.Added);
            }
            else
            {
                this.ObjectContext.StatusSprawy.AddObject(statusSprawy);
            }
        }

        public void UpdateStatusSprawy(StatusSprawy currentStatusSprawy)
        {
            this.ObjectContext.StatusSprawy.AttachAsModified(currentStatusSprawy, this.ChangeSet.GetOriginal(currentStatusSprawy));
        }

        public void DeleteStatusSprawy(StatusSprawy statusSprawy)
        {
            if ((statusSprawy.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(statusSprawy, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.StatusSprawy.Attach(statusSprawy);
                this.ObjectContext.StatusSprawy.DeleteObject(statusSprawy);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Termin' query.
        public IQueryable<Termin> GetTermin()
        {
            return this.ObjectContext.Termin;
        }

        public void InsertTermin(Termin termin)
        {
            if ((termin.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(termin, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Termin.AddObject(termin);
            }
        }

        public void UpdateTermin(Termin currentTermin)
        {
            this.ObjectContext.Termin.AttachAsModified(currentTermin, this.ChangeSet.GetOriginal(currentTermin));
        }

        public void DeleteTermin(Termin termin)
        {
            if ((termin.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(termin, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Termin.Attach(termin);
                this.ObjectContext.Termin.DeleteObject(termin);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'TerminTyp' query.
        public IQueryable<TerminTyp> GetTerminTyp()
        {
            return this.ObjectContext.TerminTyp;
        }

        public void InsertTerminTyp(TerminTyp terminTyp)
        {
            if ((terminTyp.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(terminTyp, EntityState.Added);
            }
            else
            {
                this.ObjectContext.TerminTyp.AddObject(terminTyp);
            }
        }

        public void UpdateTerminTyp(TerminTyp currentTerminTyp)
        {
            this.ObjectContext.TerminTyp.AttachAsModified(currentTerminTyp, this.ChangeSet.GetOriginal(currentTerminTyp));
        }

        public void DeleteTerminTyp(TerminTyp terminTyp)
        {
            if ((terminTyp.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(terminTyp, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.TerminTyp.Attach(terminTyp);
                this.ObjectContext.TerminTyp.DeleteObject(terminTyp);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'TypNaleznosci' query.
        public IQueryable<TypNaleznosci> GetTypNaleznosci()
        {
            return this.ObjectContext.TypNaleznosci;
        }

        public void InsertTypNaleznosci(TypNaleznosci typNaleznosci)
        {
            if ((typNaleznosci.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(typNaleznosci, EntityState.Added);
            }
            else
            {
                this.ObjectContext.TypNaleznosci.AddObject(typNaleznosci);
            }
        }

        public void UpdateTypNaleznosci(TypNaleznosci currentTypNaleznosci)
        {
            this.ObjectContext.TypNaleznosci.AttachAsModified(currentTypNaleznosci, this.ChangeSet.GetOriginal(currentTypNaleznosci));
        }

        public void DeleteTypNaleznosci(TypNaleznosci typNaleznosci)
        {
            if ((typNaleznosci.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(typNaleznosci, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.TypNaleznosci.Attach(typNaleznosci);
                this.ObjectContext.TypNaleznosci.DeleteObject(typNaleznosci);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'TypZadaniaSet' query.
        public IQueryable<TypZadaniaSet> GetTypZadaniaSet()
        {
            return this.ObjectContext.TypZadaniaSet;
        }

        public void InsertTypZadaniaSet(TypZadaniaSet typZadaniaSet)
        {
            if ((typZadaniaSet.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(typZadaniaSet, EntityState.Added);
            }
            else
            {
                this.ObjectContext.TypZadaniaSet.AddObject(typZadaniaSet);
            }
        }

        public void UpdateTypZadaniaSet(TypZadaniaSet currentTypZadaniaSet)
        {
            this.ObjectContext.TypZadaniaSet.AttachAsModified(currentTypZadaniaSet, this.ChangeSet.GetOriginal(currentTypZadaniaSet));
        }

        public void DeleteTypZadaniaSet(TypZadaniaSet typZadaniaSet)
        {
            if ((typZadaniaSet.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(typZadaniaSet, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.TypZadaniaSet.Attach(typZadaniaSet);
                this.ObjectContext.TypZadaniaSet.DeleteObject(typZadaniaSet);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Uzytkownik' query.
        public IQueryable<Uzytkownik> GetUzytkownik()
        {
            return this.ObjectContext.Uzytkownik;
        }

        public void InsertUzytkownik(Uzytkownik uzytkownik)
        {
            if ((uzytkownik.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(uzytkownik, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Uzytkownik.AddObject(uzytkownik);
            }
        }

        public void UpdateUzytkownik(Uzytkownik currentUzytkownik)
        {
            this.ObjectContext.Uzytkownik.AttachAsModified(currentUzytkownik, this.ChangeSet.GetOriginal(currentUzytkownik));
        }

        public void DeleteUzytkownik(Uzytkownik uzytkownik)
        {
            if ((uzytkownik.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(uzytkownik, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Uzytkownik.Attach(uzytkownik);
                this.ObjectContext.Uzytkownik.DeleteObject(uzytkownik);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_DaneSprawy' query.
        public IQueryable<vw_DaneSprawy> GetVw_DaneSprawy()
        {
            return this.ObjectContext.vw_DaneSprawy;
        }


  	public IQueryable<vw_DokOdebrCount> GetVw_DokOdebrCount()
        {
            return this.ObjectContext.vw_DokOdebrCount;
        }


        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_KomunikacjaDoc' query.
        public IQueryable<vw_KomunikacjaDoc> GetVw_KomunikacjaDoc()
        {
            return this.ObjectContext.vw_KomunikacjaDoc;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_ListaDokPaczki' query.
        public IQueryable<vw_ListaDokPaczki> GetVw_ListaDokPaczki()
        {
            return this.ObjectContext.vw_ListaDokPaczki;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_ListaDokPaczkiDoSalda' query.
        public IQueryable<vw_ListaDokPaczkiDoSalda> GetVw_ListaDokPaczkiDoSalda()
        {
            return this.ObjectContext.vw_ListaDokPaczkiDoSalda;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_ListaDokWys' query.
        public IQueryable<vw_ListaDokWys> GetVw_ListaDokWys()
        {
            return this.ObjectContext.vw_ListaDokWys;
        }
 // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_ListaDoOdebr' query.
        public IQueryable<vw_ListaDoOdebr> GetVw_ListaDoOdebr()
        {
            return this.ObjectContext.vw_ListaDoOdebr;
        }
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_ListaPaczek' query.
        public IQueryable<vw_ListaPaczek> GetVw_ListaPaczek()
        {
            return this.ObjectContext.vw_ListaPaczek;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_ListaSpraw' query.
        public IQueryable<vw_ListaSpraw> GetVw_ListaSpraw()
        {   
            return this.ObjectContext.vw_ListaSpraw;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_Paczki' query.
        public IQueryable<vw_Paczki> GetVw_Paczki()
        {
            return this.ObjectContext.vw_Paczki;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_Search' query.
        public IQueryable<vw_Search> GetVw_Search()
        {
            return this.ObjectContext.vw_Search;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_SprawaKomunikacjaEPU' query.
        public IQueryable<vw_SprawaKomunikacjaEPU> GetVw_SprawaKomunikacjaEPU()
        {
            return this.ObjectContext.vw_SprawaKomunikacjaEPU;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_SprawyCzynne' query.
        public IQueryable<vw_SprawyCzynne> GetVw_SprawyCzynne()
        {
            return this.ObjectContext.vw_SprawyCzynne;
        }

 	// TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_Terminy' query.
        public IQueryable<vw_Terminy> GetVw_Terminy()
        {
            return this.ObjectContext.vw_Terminy;
        }
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_UsersAspNet' query.
        public IQueryable<vw_UsersAspNet> GetVw_UsersAspNet()
        {
            return this.ObjectContext.vw_UsersAspNet;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Wplata' query.
        public IQueryable<Wplata> GetWplata()
        {
            return this.ObjectContext.Wplata;
        }

        public void InsertWplata(Wplata wplata)
        {
            if ((wplata.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(wplata, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Wplata.AddObject(wplata);
            }
        }

        public void UpdateWplata(Wplata currentWplata)
        {
            this.ObjectContext.Wplata.AttachAsModified(currentWplata, this.ChangeSet.GetOriginal(currentWplata));
        }

        public void DeleteWplata(Wplata wplata)
        {
            if ((wplata.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(wplata, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Wplata.Attach(wplata);
                this.ObjectContext.Wplata.DeleteObject(wplata);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'WplataPodz' query.
        public IQueryable<WplataPodz> GetWplataPodz()
        {
            return this.ObjectContext.WplataPodz;
        }

        public void InsertWplataPodz(WplataPodz wplataPodz)
        {
            if ((wplataPodz.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(wplataPodz, EntityState.Added);
            }
            else
            {
                this.ObjectContext.WplataPodz.AddObject(wplataPodz);
            }
        }

        public void UpdateWplataPodz(WplataPodz currentWplataPodz)
        {
            this.ObjectContext.WplataPodz.AttachAsModified(currentWplataPodz, this.ChangeSet.GetOriginal(currentWplataPodz));
        }

        public void DeleteWplataPodz(WplataPodz wplataPodz)
        {
            if ((wplataPodz.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(wplataPodz, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.WplataPodz.Attach(wplataPodz);
                this.ObjectContext.WplataPodz.DeleteObject(wplataPodz);
            }
        }
              
            // TODO:
            // Consider constraining the results of your query method.  If you need additional input you can
            // add parameters to this method or create additional query methods with different names.
            // To support paging you will need to add ordering to the 'ZgonyDetails' query.
            public IQueryable<ZgonyDetails> GetZgonyDetails()
            {
                return this.ObjectContext.ZgonyDetails;
            }
            public IQueryable<ZgonyDetails> GetZgonyDetailsByHeaderId(int HeaderId)
            {
                return this.ObjectContext.ZgonyDetails.Where(d=>d.ZgonyHeader_ID == HeaderId).OrderBy(d=>d.ZgonyDetails_Id);
            }
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'ZgonyHeader' query.
        public IQueryable<ZgonyHeader> GetZgonyHeader()
            {
                return this.ObjectContext.ZgonyHeader;
            }

            public IQueryable<ZgonyHeader> GetZgonyHeaderInOrder()
                {
                    return this.ObjectContext.ZgonyHeader.OrderByDescending(a=>a.ZgonyHeader_Id);
                }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Zadanies' query.
        public IQueryable<Zadanies> GetZadanies()
        {
            return this.ObjectContext.Zadanies;
        }

        public void InsertZadanies(Zadanies zadanies)
        {
            if ((zadanies.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(zadanies, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Zadanies.AddObject(zadanies);
            }
        }

        public void UpdateZadanies(Zadanies currentZadanies)
        {
            this.ObjectContext.Zadanies.AttachAsModified(currentZadanies, this.ChangeSet.GetOriginal(currentZadanies));
        }

        public void DeleteZadanies(Zadanies zadanies)
        {
            if ((zadanies.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(zadanies, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Zadanies.Attach(zadanies);
                this.ObjectContext.Zadanies.DeleteObject(zadanies);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'ZadanieSetdequery.
        public IQueryable<ZadanieSet> GetZadanieSet()
        {
            return this.ObjectContext.ZadanieSet;
        }

        public void InsertZadanieSet(ZadanieSet zadanieSet)
        {
            if ((zadanieSet.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(zadanieSet, EntityState.Added);
            }
            else
            {
                this.ObjectContext.ZadanieSet.AddObject(zadanieSet);
            }
        }

        public void UpdateZadanieSet(ZadanieSet currentZadanieSet)
        {
            this.ObjectContext.ZadanieSet.AttachAsModified(currentZadanieSet, this.ChangeSet.GetOriginal(currentZadanieSet));
        }

        public void DeleteZadanieSet(ZadanieSet zadanieSet)
        {
            if ((zadanieSet.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(zadanieSet, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.ZadanieSet.Attach(zadanieSet);
                this.ObjectContext.ZadanieSet.DeleteObject(zadanieSet);
            }
        }
        #region manualAdd
        public IQueryable<Import> GetImport()
        {
            return this.ObjectContext.Import;
        }

        public void InsertImport(Import import)
        {
            if ((import.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(import, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Import.AddObject(import);
            }
        }

        public void UpdateImport(Import currentImport)
        {
            this.ObjectContext.Import.AttachAsModified(currentImport, this.ChangeSet.GetOriginal(currentImport));
        }

        public void DeleteImport(Import import)
        {
            if ((import.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(import, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Import.Attach(import);
                this.ObjectContext.Import.DeleteObject(import);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'ImportDetail' query.
        public IQueryable<ImportDetail> GetImportDetail()
        {
            return this.ObjectContext.ImportDetail;
        }

        public void InsertImportDetail(ImportDetail importDetail)
        {
            if ((importDetail.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(importDetail, EntityState.Added);
            }
            else
            {
                this.ObjectContext.ImportDetail.AddObject(importDetail);
            }
        }

        public void UpdateImportDetail(ImportDetail currentImportDetail)
        {
            this.ObjectContext.ImportDetail.AttachAsModified(currentImportDetail, this.ChangeSet.GetOriginal(currentImportDetail));
        }

        public void DeleteImportDetail(ImportDetail importDetail)
        {
            if ((importDetail.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(importDetail, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.ImportDetail.Attach(importDetail);
                this.ObjectContext.ImportDetail.DeleteObject(importDetail);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_Importy' query.
        public IQueryable<vw_Importy> GetVw_Importy()
        {
            return this.ObjectContext.vw_Importy;
        }

         // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'vw_ImportDetails' query.
        public IQueryable<vw_ImportDetails> GetVw_ImportDetails()
        {
            return this.ObjectContext.vw_ImportDetails;
        }


        public IQueryable<vw_ImportDetails> GetVw_ImportDetailsByImportId(int Import_Id)
        {
            return this.ObjectContext.vw_ImportDetails.Where(a=>a.Import_Id==Import_Id).OrderBy(a=>a.Id);
        }

        public void InsertVw_ImportDetails(vw_ImportDetails vw_ImportDetails)
        {
            if ((vw_ImportDetails.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(vw_ImportDetails, EntityState.Added);
            }
            else
            {
                this.ObjectContext.vw_ImportDetails.AddObject(vw_ImportDetails);
            }
        }

        public void UpdateVw_ImportDetails(vw_ImportDetails currentvw_ImportDetails)
        {
            this.ObjectContext.vw_ImportDetails.AttachAsModified(currentvw_ImportDetails, this.ChangeSet.GetOriginal(currentvw_ImportDetails));
        }

        public void DeleteVw_ImportDetails(vw_ImportDetails vw_ImportDetails)
        {
            if ((vw_ImportDetails.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(vw_ImportDetails, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.vw_ImportDetails.Attach(vw_ImportDetails);
                this.ObjectContext.vw_ImportDetails.DeleteObject(vw_ImportDetails);
            }
        }

        
            // TODO:
            // Consider constraining the results of your query method.  If you need additional input you can
            // add parameters to this method or create additional query methods with different names.
            // To support paging you will need to add ordering to the 'MapDokKom' query.
            public IQueryable<MapDokKom> GetMapDokKom()
            {
                return this.ObjectContext.MapDokKom;
            }

            public void InsertMapDokKom(MapDokKom mapDokKom)
            {
                if ((mapDokKom.EntityState != EntityState.Detached))
                {
                    this.ObjectContext.ObjectStateManager.ChangeObjectState(mapDokKom, EntityState.Added);
                }
                else
                {
                    this.ObjectContext.MapDokKom.AddObject(mapDokKom);
                }
            }

            public void UpdateMapDokKom(MapDokKom currentMapDokKom)
            {
                this.ObjectContext.MapDokKom.AttachAsModified(currentMapDokKom, this.ChangeSet.GetOriginal(currentMapDokKom));
            }

            public void DeleteMapDokKom(MapDokKom mapDokKom)
            {
                if ((mapDokKom.EntityState != EntityState.Detached))
                {
                    this.ObjectContext.ObjectStateManager.ChangeObjectState(mapDokKom, EntityState.Deleted);
                }
                else
                {
                    this.ObjectContext.MapDokKom.Attach(mapDokKom);
                    this.ObjectContext.MapDokKom.DeleteObject(mapDokKom);
                }
            }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SadSlownik' query.
        public IQueryable<SadSlownik> GetSadSlownik()
        {
            return this.ObjectContext.SadSlownik;
        }

        public void InsertSadSlownik(SadSlownik sadSlownik)
        {
            if ((sadSlownik.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sadSlownik, EntityState.Added);
            }
            else
            {
                this.ObjectContext.SadSlownik.AddObject(sadSlownik);
            }
        }

        public void UpdateSadSlownik(SadSlownik currentSadSlownik)
        {
            this.ObjectContext.SadSlownik.AttachAsModified(currentSadSlownik, this.ChangeSet.GetOriginal(currentSadSlownik));
        }

        public void DeleteSadSlownik(SadSlownik sadSlownik)
        {
            if ((sadSlownik.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sadSlownik, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.SadSlownik.Attach(sadSlownik);
                this.ObjectContext.SadSlownik.DeleteObject(sadSlownik);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SadSprawa' query.
        public IQueryable<SadSprawa> GetSadSprawa()
        {
            return this.ObjectContext.SadSprawa;
        }

        public void InsertSadSprawa(SadSprawa sadSprawa)
        {
            if ((sadSprawa.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sadSprawa, EntityState.Added);
            }
            else
            {
                this.ObjectContext.SadSprawa.AddObject(sadSprawa);
            }
        }

        public void UpdateSadSprawa(SadSprawa currentSadSprawa)
        {
            this.ObjectContext.SadSprawa.AttachAsModified(currentSadSprawa, this.ChangeSet.GetOriginal(currentSadSprawa));
        }

        public void DeleteSadSprawa(SadSprawa sadSprawa)
        {
            if ((sadSprawa.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sadSprawa, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.SadSprawa.Attach(sadSprawa);
                this.ObjectContext.SadSprawa.DeleteObject(sadSprawa);
            }
        }

        #endregion

        #region  myqueries

        public IQueryable<vw_tmpZmianaAdresuPeln> GetVw_tmpZmianaAdresuPeln()
        {
            return this.ObjectContext.vw_tmpZmianaAdresuPeln;
        }

        public IQueryable<Sprawa> GetSprawaById(int IdSprawy)
        {
            return this.ObjectContext.Sprawa.Where(a => a.id == IdSprawy);
        }
        // Zwraca dane z tabel referencyjnych
        public IQueryable<vw_DaneSprawy> GetVw_DaneSprawyById(int IdSprawy)
        {
            return this.ObjectContext.vw_DaneSprawy.Where(a => a.id == IdSprawy);
        }

        public vw_DaneSprawy GetVw_DaneSprawyById2(int IdSprawy)
        {
            return this.ObjectContext.vw_DaneSprawy.FirstOrDefault(a => a.id == IdSprawy);
        }
        public IQueryable<StatusSprawy> GetStatusSprawyBySprId(int IdSprawy)
        {
            return this.ObjectContext.StatusSprawy.Where(a => a.Sprawa_id == IdSprawy).Where(a => a.czyus == 0).OrderByDescending(a => a.DataStatusu);
        }
        public IQueryable<NazwaStatusu> GetNazwaStatusuInOrder()
        {
            return this.ObjectContext.NazwaStatusu.OrderBy(a => a.Krok);
        }
        public IQueryable<Dekretacja> GetDekretacjaJednostkaByIdSprawy(int IdSprawy)
        {
            return this.ObjectContext.Dekretacja.Where(a => a.Sprawa_id == IdSprawy).Where(a => a.JednostkaWindykacji_Id > 0).OrderByDescending(a => a.DataDekretJednostka).OrderByDescending(a => a.Id);
        }

        public IQueryable<Dekretacja> GetDekretacjaReferentByIdSprawy(int IdSprawy)
        {
            return this.ObjectContext.Dekretacja.Where(a => a.Sprawa_id == IdSprawy).Where(a => a.Uzytkownik_Id > 0).OrderByDescending(a => a.DataDekretUser).OrderByDescending(a => a.Id);
        }
        public IQueryable<DaneDluznika> GetDaneDluznikaByIdSprawy(int IdSprawy)
        {
            return this.ObjectContext.DaneDluznika.Where(a => a.Sprawa_Id == IdSprawy);
        }


        public IQueryable<Naleznosc> GetNaleznoscByIdSprawy(int IdSprawy)
        {
            return this.ObjectContext.Naleznosc.Where(a => a.Sprawa_id == IdSprawy).OrderBy(a=>a.numer);
        }
        public IQueryable<Odsetki> GetOdsetkiByNaleznoscId(int IdNal)
        {
            return this.ObjectContext.Odsetki.Where(a => a.Naleznosc_Id == IdNal).OrderBy(b => b.DataPocz);
        }
        public IQueryable<TypNaleznosci> GetTypNaleznosciInOrder()
        {
            return this.ObjectContext.TypNaleznosci.OrderBy(a => a.TypNal).OrderBy(b => b.id);
        }


        public IQueryable<vw_ListaSpraw> GetVw_ListaSprByUser(int IdUser)
        {
            //  Parametry dla każdego parametru  -1 dowolny, 0 - nie wpisany,  


            return this.ObjectContext.vw_ListaSpraw.Where(a => a.Id_User == IdUser).Where(b => b.Id_Jednostki > 0).OrderBy(c => c.id);
        }
        public IQueryable<vw_ListaSpraw> GetVw_ListaSprawParams(int IdUser, int IdJednostki, int IdStatusu, int Krok)
        {
            IQueryable<vw_ListaSpraw> myquery;
            myquery = this.ObjectContext.vw_ListaSpraw;
            this.ObjectContext.CommandTimeout = 180;
            //  Parametry dla każdego parametru  -1 dowolny, 0 - nie wpisany, inny - dokładnie podany  
            switch (IdUser)
            {
                case 0:

                    myquery = myquery.Where(a => a.Id_User == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.Id_User == IdUser);
                    break;
            }

            switch (IdJednostki)
            {
                case 0:

                    myquery = myquery.Where(a => a.Id_Jednostki == 0);
                    break;
                case -1:
                    break;

                case -100:
                        
                default:
                    myquery = myquery.Where(a => a.Id_Jednostki == IdJednostki);
                    break;
            }


            switch (IdStatusu)
            {
                case 0:

                    myquery = myquery.Where(a => a.IdStatusu == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.IdStatusu == IdStatusu);
                    break;
            }

            switch (Krok)
            {
                case 0:

                    myquery = myquery.Where(a => a.Krok == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.Krok == Krok);
                    break;
            }


            return myquery.OrderBy(a => a.id);
        }


        ///////////////////////////////////
        public IQueryable<vw_Terminy> GetVw_ListaTerminowParams(int IdUser, int IdJednostki, int Status, DateTime dzien)
        {
            IQueryable<vw_Terminy> myquery;
            myquery = this.ObjectContext.vw_Terminy;

            //  Parametry dla każdego parametru  -1 dowolny, 0 - nie wpisany, inny - dokładnie podany  
            switch (IdUser)
            {
                case 0:

                    myquery = myquery.Where(a => a.Id_User == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.Id_User == IdUser);
                    break;
            }

            switch (IdJednostki)
            {
                case 0:

                    myquery = myquery.Where(a => a.Id_Jednostki == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.Id_Jednostki == IdJednostki);
                    break;
            }


            switch (Status)
            {
                
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.Status == Status);
                    break;
            }

           
            myquery = myquery.Where(a => a.DataZapisu <= dzien).Where(a=>a.DataDoWykonania>=dzien);
           
            return myquery.OrderBy(a => a.Id);
        }




        // odczyt danych sprawy;

        public Sprawa GetSprawaInclude(int IdSprawy)
        {
            Sprawa spr, spr1;
            spr = this.ObjectContext.Sprawa.Include("DaneDluznika").Include("Naleznosc").Include("Wplata").Include("JednostkaOrg").Where(a => a.id == IdSprawy).FirstOrDefault();
            EntityCollection<Naleznosc> nal;

            nal = new EntityCollection<Naleznosc>();
            try
            {
            
                
                spr1 = new Sprawa();
            spr1.id = spr.id;
            spr1.IdWiena = spr.IdWiena;
            spr1.sygnatura = spr.sygnatura;
            spr1.Rok = spr.Rok;
            spr1.PartitionKey = spr.PartitionKey;
            spr1.Poz = spr.Poz;

            //spr1.Naleznosc = new EntityCollection<Naleznosc>();
           

                foreach (var p in spr.Naleznosc.OrderBy(a => a.numer))
                {
                    spr1.Naleznosc.Add(p);
                }
                spr.Naleznosc.Clear();
                foreach (var q in spr1.Naleznosc.OrderBy(a => a.numer))
                {
                    
                    spr.Naleznosc.Add(q);
                }
                
            }

                /*
                  var lateCustomers = from c in ctx.Customers 
                where c.IsMoreThan30DaysInArrears 
                select new {  
                    Customer = c,  
                    Orders = c.Orders.OrderByDescending( 
                                 o => o.OrderDate 
                             )  
                }; 
                  */

            catch (Exception ex)
            {
                string mess;
                mess = ex.Message;
            
            }
            return spr;
            //return this.ObjectContext.Sprawa.Include("DaneDluznika").Include("Naleznosc").Include("JednostkaOrg").Where(a => a.id == IdSprawy).FirstOrDefault();
            
        }


        public Naleznosc GetOdsetkiNalInclude(int IdNal)
        {
            this.ObjectContext.CommandTimeout = 120;
            return this.ObjectContext.Naleznosc.Include("Odsetki").Include("WplataPodz").Include("TypNaleznosci").Include("StanNaleznosci").Where(a => a.Id == IdNal).FirstOrDefault();
        }

        public Uzytkownik GetUzytkownikById(int IdUzytkownik)
        {
            return this.ObjectContext.Uzytkownik.Include("KontoEPU").Where(a => a.Id == IdUzytkownik).FirstOrDefault();
        }

        public Uzytkownik GetUzytkownikByUserName(string UserName)
        {
            Uzytkownik u =  this.ObjectContext.Uzytkownik.Include("KontoEPU").Where(a => a.UserName == UserName).FirstOrDefault();
            JednostkaWindykacji jed = this.ObjectContext.JednostkaWindykacji.Where(a => a.Id == u.JednostkaWindykacji_Id).FirstOrDefault();
            u.JednostkaWindykacji = jed;
            return u;
        }

        public KontoEPU GetKontoById(int IdKonta)
        {
            return this.ObjectContext.KontoEPU.Where(a => a.Id == IdKonta).FirstOrDefault();
        }

        public JednostkaOrg GetOddzialasPowodById(int IdOddzial)
        {
            return this.ObjectContext.JednostkaOrg.Where(a => a.Id == IdOddzial).FirstOrDefault();
        }

        public NazwaStatusu GetNazwaStatusbyKrok(int Krok)
        {
            return this.ObjectContext.NazwaStatusu.Where(a => a.Krok == Krok).FirstOrDefault();
        }

        public IQueryable<vw_ListaDokWys> GetVw_ListaDokWysParams(int IdUser, int IdJednostki, int StatusDok, int TypDok)
        {
            IQueryable<vw_ListaDokWys> myquery;
            myquery = this.ObjectContext.vw_ListaDokWys;

            //  Parametry dla każdego parametru  -1 dowolny, 0 - nie wpisany, inny - dokładnie podany  
            switch (IdUser)
            {
                case 0:

                    myquery = myquery.Where(a => a.Id_User == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.Id_User == IdUser);
                    break;
            }

            switch (IdJednostki)
            {
                case 0:

                    myquery = myquery.Where(a => a.Id_Jednostki == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.Id_Jednostki == IdJednostki);
                    break;
            }


            switch (StatusDok)
            {
                case 0:

                    myquery = myquery.Where(a => a.StatusDok == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.StatusDok == StatusDok);
                    break;
            }
            switch (TypDok)
            {
                case 0:

                    myquery = myquery.Where(a => a.TypDok == 0);
                    break;
                case -1:
                    break;


                default:
                    if (TypDok < 0)
                    {
                        TypDok =  - TypDok;
                        myquery = myquery.Where(a => a.TypDok != TypDok);
                    }
                    else
                     myquery = myquery.Where(a => a.TypDok == TypDok);
                    break;
            }



            return myquery.OrderByDescending (a => a.Id);
        }

        public IQueryable<vw_ListaDoOdebr> GetVw_ListaDokOdebrParams(int IdUser, int IdJednostki, int TypDok)
        {
            IQueryable<vw_ListaDoOdebr> myquery;
            myquery = this.ObjectContext.vw_ListaDoOdebr;

            //  Parametry dla każdego parametru  -1 dowolny, 0 - nie wpisany, inny - dokładnie podany  
            switch (IdUser)
            {
                case 0:

                    myquery = myquery.Where(a => a.Id_User == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.Id_User == IdUser);
                    break;
            }

            switch (IdJednostki)
            {
                case 0:

                    myquery = myquery.Where(a => a.Id_Jednostki == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.Id_Jednostki == IdJednostki);
                    break;
            }


           
            switch (TypDok)
            {
                case 0:

                    myquery = myquery.Where(a => a.TypDok != 5).Where(a=>a.TypDok!= 17);   // pozostałe dokumenty poza nakazami i klauzulami
                    break;
                case -1:

                    break;


                default:
                    myquery = myquery.Where(a => a.TypDok == TypDok);
                    break;
            }



            return myquery.OrderByDescending(a=>a.DataRejestracji).OrderByDescending(b => b.Id);
        }




        public DokWys GetDokWysWithPozewById(int IdDok)
        {
            return this.ObjectContext.DokWys.Include("Pozew").Include("Sprawa").Where(a => a.Id == IdDok).FirstOrDefault();
        }
        public DokWys GetDokWysWithPdfById(int IdDok)
        {
            return this.ObjectContext.DokWys.Include("PdfStore").Where(a => a.Id == IdDok).FirstOrDefault();
        }

        public DokWys GetDokWysById(int IdDok)
        {
            return this.ObjectContext.DokWys.Where(a => a.Id == IdDok).FirstOrDefault();
        }

        public DokWys GetDokWysWithSprawaById(int IdDok)
        {
            return this.ObjectContext.DokWys.Include("Sprawa").Where(a => a.Id == IdDok).FirstOrDefault();
        }

        public IQueryable<vw_Paczki> GetVw_PaczkiToAdd(int IdJednostki)
        {
            return this.ObjectContext.vw_Paczki.Where(a => a.StatusPaczki == 1).Where(a=>a.miesiac == IdJednostki).OrderByDescending(a => a.DataZalozenia);
        }

        public Paczka GetMaxPaczka(int IdJednostki)
        {
            return this.ObjectContext.Paczka.Where(a => a.miesiac == IdJednostki).OrderByDescending(a => a.rok).ThenByDescending(a => a.nr).FirstOrDefault();
        }
        public IQueryable<vw_ListaPaczek> GetVw_ListaPaczekParams(int IdJednostki, int StatusPaczki, int TypDok)
        {
            IQueryable<vw_ListaPaczek> myquery;
            myquery = this.ObjectContext.vw_ListaPaczek;

            //  Parametry dla każdego parametru  -1 dowolny, 0 - nie wpisany, inny - dokładnie podany  


            switch (IdJednostki)
            {
                case 0:

                    myquery = myquery.Where(a => a.miesiac == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.miesiac == IdJednostki);
                    break;
            }


            switch (StatusPaczki)
            {
                case 0:

                    myquery = myquery.Where(a => a.StatusPaczki == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.StatusPaczki == StatusPaczki);
                    break;
            }
            switch (TypDok)
            {
                case 0:

                    myquery = myquery.Where(a => a.TypDok == 0);
                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.TypDok == TypDok);
                    break;
            }



            return myquery.OrderByDescending(a => a.Id);
        }

        public Paczka GetPaczkaById(int IdPaczki)
        {
            return this.ObjectContext.Paczka.Where(a => a.Id == IdPaczki).FirstOrDefault();
        }

        public IQueryable<KontoEPU> GetKontoEPUByIdJednostki(int IdJednostki)
        {
            return this.ObjectContext.KontoEPU.Where(a => a.JednostkaWindykacji_Id == IdJednostki);
        }

        public IQueryable<KontoEPU> GetKontoEPUByIdJednostkiExtended(int IdJednostki)
        {
            if (IdJednostki == 0)
                return this.ObjectContext.KontoEPU;
            else
                return this.ObjectContext.KontoEPU.Where(a => a.JednostkaWindykacji_Id == IdJednostki);
        }

        public IQueryable<vw_ListaDokPaczki> GetVw_ListaDokPaczkiByIdPaczki(int IdPaczki)
        {
            return this.ObjectContext.vw_ListaDokPaczki.Where(a => a.IdPaczki == IdPaczki).OrderBy(a => a.Id);
        }
        public DokumentPaczka GetDokumentPaczkaById(int IdDokumentPaczka)
        {
            return this.ObjectContext.DokumentPaczka.Where(a => a.Id == IdDokumentPaczka).FirstOrDefault();
        }

        public IQueryable<vw_Search> GetSearchSpr(string Sygnatura, string Nazwa, string SygnNCe, string NrEwid, string Opis, string Ulica, string Miejscowosc, string Poczta, string Pesel)
        {
            IQueryable<vw_Search> myquery;
            myquery = this.ObjectContext.vw_Search;

            if (Sygnatura != null)
                if (Sygnatura.Trim().Length > 0)
                    myquery = myquery.Where(a => a.sygnatura.Contains(Sygnatura));

            if (Nazwa != null)
                if (Nazwa.Trim().Length > 0)
                    myquery = myquery.Where(a => a.Dluznik.Contains(Nazwa));

            if (SygnNCe != null)
                if (SygnNCe.Trim().Length > 0)
                    myquery = myquery.Where(a => a.SygnNCe.Contains(SygnNCe));

            if (NrEwid != null)
                if (NrEwid.Trim().Length > 0)
                    myquery = myquery.Where(a => a.NrEwid.Contains(NrEwid));

            if (Opis != null)
                if (Opis.Trim().Length > 0)
                    myquery = myquery.Where(a => a.opis.Contains(Opis));

            if (Ulica != null)
                if (Ulica.Trim().Length > 0)
                    myquery = myquery.Where(a => a.ulica.Contains(Ulica));

            if (Miejscowosc != null)
                if (Miejscowosc.Trim().Length > 0)
                    myquery = myquery.Where(a => a.miejscowosc.Contains(Miejscowosc));
            if (Poczta != null)
                if (Poczta.Trim().Length > 0)
                    myquery = myquery.Where(a => a.poczta.Contains(Poczta));
            if (Pesel != null)
                if (Pesel.Trim().Length > 0)
                    myquery = myquery.Where(a => a.pesel.Contains(Pesel));

        

            return myquery.OrderBy(a => a.IdRow);
        }

        public IQueryable<vw_ListaSpraw> GetVw_ListaSprByIdList()
        {
            //  Parametry dla każdego parametru  -1 dowolny, 0 - nie wpisany,  


            return this.ObjectContext.vw_ListaSpraw;
        }

        public IQueryable<Wplata> GetWplataBySprId(int IdSprawy)
        {
            return this.ObjectContext.Wplata.Where(a => a.Sprawa_id == IdSprawy).OrderBy(a => a.Id);
        }
        public IQueryable<WplataPodz> GetWplataPodzByIdWplaty(int IdWplaty)
        {
            return this.ObjectContext.WplataPodz.Include("Naleznosc").Where(a => a.Wplata_Id == IdWplaty).Where(a => a.SplataKapital + a.SplataOdsetki != 0).OrderBy(a => a.Id);
        }

        public IQueryable<vw_ListaDokWys> GetDokWysByIdSprawy(int IdSprawy)
        {
            return this.ObjectContext.vw_ListaDokWys.Where(a => a.IdSprawy == IdSprawy).OrderBy(a => a.Id);
        }
        public IQueryable<DokOdebr> GetDokOdebrByIdSprawy(int IdSprawy)
        {
            return this.ObjectContext.DokOdebr.Include("PdfStore").Where(a => a.Sprawa_id == IdSprawy).OrderBy(a => a.Id);
        }

        public DokOdebr GetDokOdebrById(int Id)
        {
            return this.ObjectContext.DokOdebr.Include("PdfStore").Where(a => a.Id == Id).FirstOrDefault();
        }

        public IQueryable<Uzytkownik> GetUzytkownikByIdJednostki(int IdJednostki, int rola)
        {
            if (rola == 2)  // admin
                return this.ObjectContext.Uzytkownik.OrderBy(a => a.JednostkaWindykacji_Id).OrderBy(a => a.Id);
            if (rola == 1)
                return this.ObjectContext.Uzytkownik.Where(a => a.JednostkaWindykacji_Id == IdJednostki).Where(a => a.Rola != 2).OrderBy(a => a.Id);
            return this.ObjectContext.Uzytkownik.Where(a => a.JednostkaWindykacji_Id == -1000);
        }

        public aspnet_Membership GetAspnet_MembershipByUserId(Guid _UserId)
        {
            return this.ObjectContext.aspnet_Membership.Where(a => a.UserId == _UserId).FirstOrDefault();
            //return this.ObjectContext.aspnet_Membership.FirstOrDefault();// this.ObjectContext.aspnet_Membership;//.Where(a => a.UserId == _UserId).FirstOrDefault();
        }

        public IQueryable<Slownik> GetSlownikByTyp(int typ)
        {
            return this.ObjectContext.Slownik.Where(a => a.Typ == typ).OrderBy(a => a.Id);
        }

        public IQueryable<Slownik> GetSlownikByTypnFiltr(int typ, int filtr)
        {
            if (filtr == 0)
                return this.ObjectContext.Slownik.Where(a => a.Typ == typ).OrderBy(a => a.Id);
            else
                return this.ObjectContext.Slownik.Where(a => a.Typ == typ).Where(a => a.filtr == filtr).OrderBy(a => a.Id);
        }
        public IQueryable<vw_SprawaKomunikacjaEPU> GetVw_SprawaKomunikacjaEPUByIdSprawy(int IdSprawy)
        {
            return this.ObjectContext.vw_SprawaKomunikacjaEPU.Where(a => a.id == IdSprawy).OrderByDescending(a => a.IdKomunikacji);
        }

        public IQueryable<StanSprawy> GetStanSprawyByIdSprawy(int IdSprawy)
        {
            return this.ObjectContext.StanSprawy.Where(a => a.Sprawa_Id == IdSprawy).OrderByDescending(a => a.data_s);
        }

        public IQueryable<StanNaleznosci> GetStanNaleznosciByIdSprawyNDataStanu(int IdSprawy,DateTime DataStanu)
        {
            IQueryable<StanNaleznosci> query;

            query = this.ObjectContext.StanNaleznosci.Include("Naleznosc").Where(a => a.Naleznosc.Sprawa_id == IdSprawy).Where(a => a.data_s == DataStanu).OrderByDescending(a => a.data_s);
            return query;
        }
        
        public IQueryable<vw_ListaDokPaczkiDoSalda> GetVw_ListaDokPaczkiDoSaldaById(int IdPaczki)
        {
            return this.ObjectContext.vw_ListaDokPaczkiDoSalda.Where(a => a.IdPaczki == IdPaczki).OrderBy(a => a.NrEwid);
        }
        public IQueryable<vw_KomunikacjaDoc> GetVw_KomunikacjaDocByIdDoc(int IdDocWys)
        {
            return this.ObjectContext.vw_KomunikacjaDoc.Where(a => a.DokWys_Id == IdDocWys).OrderByDescending(a => a.d_kreacji);
        }

        public IQueryable<ZadanieSet> GetZadanieSetInOrder()
        {
            return this.ObjectContext.ZadanieSet.OrderByDescending(a=>a.Id);
        }


       

        public IQueryable<StanNaleznosci> GetStanNaleznosciByIdNal(int IdNal)
        {
            return this.ObjectContext.StanNaleznosci.Where(a=>a.Naleznosc_Id == IdNal);
        }

        public IQueryable<StanNaleznosci> GetStanNaleznosciByDateNSprId(int sprId, DateTime theDay)
        {
            

            return (from n in this.ObjectContext.Naleznosc 
                    join s in this.ObjectContext.StanNaleznosci  on n.Id equals s.Naleznosc_Id 
                    where n.Sprawa_id == sprId && s.data_s == theDay 
                    select s );
        }

        public IQueryable<KancelariaKomornicza> GetKancelariaKomorniczaInOrder()
        {
            return this.ObjectContext.KancelariaKomornicza.OrderBy(a=>a.IdEPU);
        }

        public IQueryable<SposobyEgzSlownik> GetSposobyEgzSlownikByJednostka(int IdJednostki)
        {
            if (IdJednostki > 0 ) 
                return this.ObjectContext.SposobyEgzSlownik.Include("JednostkaWindykacji").Where(a=>a.JednostkaWindykacji_Id == IdJednostki);
            else
                return this.ObjectContext.SposobyEgzSlownik.Include("JednostkaWindykacji");

        }

        public IQueryable<OdsTab> GetOdsTabinOrder()
        {
            return this.ObjectContext.OdsTab.OrderByDescending(a=>a.DataP);
        }

        // kimporty
        public IQueryable<vw_Importy> GetVw_ListaImportsParams( int IdJednostki, int ImpTyp)
        {
            IQueryable<vw_Importy> myquery;
            if (ImpTyp < 0)
                myquery = this.ObjectContext.vw_Importy.Where(a => a.ImpExp < 0);
            else
                myquery = this.ObjectContext.vw_Importy.Where(a => a.ImpExp >= 0);


            switch (IdJednostki)
            {
                case 0:

                    break;
                case -1:
                    break;


                default:
                    myquery = myquery.Where(a => a.JednostkaWindykacji_Id == IdJednostki);
                    break;
            }




            return myquery.OrderByDescending(a => a.DataTransferu);
        }

        public IQueryable<SadSprawa> GetSadSprawaBySprId(int IdSprawy)
        {
            return this.ObjectContext.SadSprawa.Where(a => a.sprawa_id == IdSprawy).Where(a => a.czyus == 0).OrderByDescending(a => a.d_skierowania).ThenByDescending(a=>a.id);
        }

        public IQueryable<SadSlownik> GetSadyInOrderAll()
        {
            return this.ObjectContext.SadSlownik.OrderBy(a => a.miasto).ThenBy(a=>a.rejokr).ThenBy(a=>a.nazwa);
        }

        public IQueryable<SadSlownik> GetSadyInOrderActive()
        {
            return this.ObjectContext.SadSlownik.Where(a=>a.czyus== 0 ).OrderBy(a => a.miasto).ThenBy(a => a.rejokr).ThenBy(a => a.nazwa);
        }

        public  SadSprawa GetSadSprawaById(int Id)
        {
            return this.ObjectContext.SadSprawa.Where(a => a.id == Id).FirstOrDefault();
        }
        #endregion myqueries
        #region UzD
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UZD_Konfig' query.
        public IQueryable<UZD_Konfig> GetUZD_Konfig()
        {
            return this.ObjectContext.UZD_Konfig;
        }

        public void InsertUZD_Konfig(UZD_Konfig UZD_Konfig)
        {
            if ((UZD_Konfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(UZD_Konfig, EntityState.Added);
            }
            else
            {
                this.ObjectContext.UZD_Konfig.AddObject(UZD_Konfig);
            }
        }

        public void UpdateUZD_Konfig(UZD_Konfig currentUZD_Konfig)
        {
            this.ObjectContext.UZD_Konfig.AttachAsModified(currentUZD_Konfig, this.ChangeSet.GetOriginal(currentUZD_Konfig));
        }

        public void DeleteUZD_Konfig(UZD_Konfig UZD_Konfig)
        {
            if ((UZD_Konfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(UZD_Konfig, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.UZD_Konfig.Attach(UZD_Konfig);
                this.ObjectContext.UZD_Konfig.DeleteObject(UZD_Konfig);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UZD_PakietWplat' query.
        public IQueryable<UZD_PakietWplat> GetUZD_PakietWplat()
        {
            return this.ObjectContext.UZD_PakietWplat.OrderByDescending(a=>a.DataTypowania);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UZD_Wplata' query.
        public IQueryable<UZD_Wplata> GetUZD_Wplata()
        {
            return this.ObjectContext.UZD_Wplata;
        }

        public void InsertUZD_Wplata(UZD_Wplata uZD_Wplata)
        {
            if ((uZD_Wplata.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(uZD_Wplata, EntityState.Added);
            }
            else
            {
                this.ObjectContext.UZD_Wplata.AddObject(uZD_Wplata);
            }
        }

        public void UpdateUZD_Wplata(UZD_Wplata currentUZD_Wplata)
        {
            this.ObjectContext.UZD_Wplata.AttachAsModified(currentUZD_Wplata, this.ChangeSet.GetOriginal(currentUZD_Wplata));
        }

        public void DeleteUZD_Wplata(UZD_Wplata uZD_Wplata)
        {
            if ((uZD_Wplata.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(uZD_Wplata, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.UZD_Wplata.Attach(uZD_Wplata);
                this.ObjectContext.UZD_Wplata.DeleteObject(uZD_Wplata);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UZDvw_Wplaty' query.
        public IQueryable<UZDvw_Wplaty> GetUZDvw_WplatyByPakietId(int IdPakiet)
        {
            return this.ObjectContext.UZDvw_Wplaty.Where(a=>a.UZD_PakietWplatId == IdPakiet).OrderBy(a=>a.DataWpl);
        }
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UZDvw_Wplaty' query.
        public IQueryable<UZDvw_Wplaty> GetUZDvw_Wplaty()
        {
            return this.ObjectContext.UZDvw_Wplaty;
        }
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UZD_Pakiet' query.
        public IQueryable<UZD_Pakiet> GetUZD_Pakiet()
        {
            return this.ObjectContext.UZD_Pakiet;
        }

       
            // TODO:
            // Consider constraining the results of your query method.  If you need additional input you can
            // add parameters to this method or create additional query methods with different names.
            // To support paging you will need to add ordering to the 'UZDvw_Pakiet' query.
            public IQueryable<UZDvw_Pakiet> GetUZDvw_Pakiet()
            {
                return this.ObjectContext.UZDvw_Pakiet.OrderByDescending(a=>a.UZD_PakietId);
            }


        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UZD_Faktura' query.
        public IQueryable<UZD_Faktura> GetUZD_Faktura()
        {
            return this.ObjectContext.UZD_Faktura;
        }

        public IQueryable<UZD_Faktura> GetUZD_FakturaByPakietId(int IdPakiet)
        {
            return this.ObjectContext.UZD_Faktura.Where(p=>p.UZD_PakietId == IdPakiet).OrderBy(p=>p.UZD_FakturaId);
        }

        #endregion

        #region BIG

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'BIG_Case' query.
        public IQueryable<BIG_Case> GetBIG_Case()
        {
            return this.ObjectContext.BIG_Case;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'BIG_Debtor' query.
        public IQueryable<BIG_Debtor> GetBIG_Debtor()
        {
            return this.ObjectContext.BIG_Debtor;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'BIG_Import' query.
        public IQueryable<BIG_Import> GetBIG_Import()
        {
            return this.ObjectContext.BIG_Import;
        }
        public IQueryable<BIG_Import> GetBIG_ImportByDate()
        {
            return this.ObjectContext.BIG_Import.OrderByDescending(a=>a.DataImportu);
        }
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'BIG_Obligation' query.
        public IQueryable<BIG_Obligation> GetBIG_Obligation()
        {
            return this.ObjectContext.BIG_Obligation;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'BIG_Operacja' query.
        public IQueryable<BIG_Operacja> GetBIG_Operacja()
        {
            return this.ObjectContext.BIG_Operacja;
        }

        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'BIGvw_ImportyDetail' query.
        public IQueryable<BIGvw_ImportyDetail> GetBIGvw_ImportyDetail()
        {
            this.ObjectContext.CommandTimeout = 180;
            return this.ObjectContext.BIGvw_ImportyDetail.OrderByDescending(a=>a.BIG_OperacjaId);
        }

        public void InsertBIGvw_ImportyDetail(BIGvw_ImportyDetail bIGvw_ImportyDetail)
        {
            if ((bIGvw_ImportyDetail.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(bIGvw_ImportyDetail, EntityState.Added);
            }
            else
            {
                this.ObjectContext.BIGvw_ImportyDetail.AddObject(bIGvw_ImportyDetail);
            }
        }

        public void UpdateBIGvw_ImportyDetail(BIGvw_ImportyDetail currentBIGvw_ImportyDetail)
        {
            this.ObjectContext.BIGvw_ImportyDetail.AttachAsModified(currentBIGvw_ImportyDetail, this.ChangeSet.GetOriginal(currentBIGvw_ImportyDetail));
        }

        public void DeleteBIGvw_ImportyDetail(BIGvw_ImportyDetail bIGvw_ImportyDetail)
        {
            if ((bIGvw_ImportyDetail.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(bIGvw_ImportyDetail, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.BIGvw_ImportyDetail.Attach(bIGvw_ImportyDetail);
                this.ObjectContext.BIGvw_ImportyDetail.DeleteObject(bIGvw_ImportyDetail);
            }
        }
    
    // TODO:
    // Consider constraining the results of your query method.  If you need additional input you can
    // add parameters to this method or create additional query methods with different names.
    // To support paging you will need to add ordering to the 'BIGvw_ImportWiena2BIG' query.
    public IQueryable<BIGvw_ImportWiena2BIG> GetBIGvw_ImportWiena2BIG()
        {
            return this.ObjectContext.BIGvw_ImportWiena2BIG;
        }
           public IQueryable<BIGvw_Dluznicy> GetBIGvw_Dluznicy()
            {
            IQueryable<BIGvw_Dluznicy> dlx;
            dlx = this.ObjectContext.BIGvw_Dluznicy.OrderBy(a => a.BIG_CaseId);

            return dlx;
            }

        public IQueryable<BIGvw_ImportyDetail> GetBIGvw_ImportyDetailByCaseId(int Import_Id)
        {
            this.ObjectContext.CommandTimeout = 180;
            return this.ObjectContext.BIGvw_ImportyDetail.Where(a => a.BIG_CaseId == Import_Id).OrderByDescending(a => a.BIG_OperacjaId);
        }

        public IQueryable<BIGvw_ImportyDetail> GetBIGvw_ImportyDetailByImportId(int Import_Id)
        {
            this.ObjectContext.CommandTimeout = 180;
            return this.ObjectContext.BIGvw_ImportyDetail.Where(a=>a.BIG_ImportId == Import_Id).OrderBy(a=>a.StatusOperacji).ThenBy(a=>a.BIG_OperacjaId);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'BIG_Konfig' query.
        public IQueryable<BIG_Konfig> GetBIG_Konfig()
        {
            return this.ObjectContext.BIG_Konfig;
        }

      

        public void UpdateBIG_Konfig(BIG_Konfig currentBIG_Konfig)
        {
            this.ObjectContext.BIG_Konfig.AttachAsModified(currentBIG_Konfig, this.ChangeSet.GetOriginal(currentBIG_Konfig));
        }

        public IQueryable<BIGvw_ObligationLastStatus> GetBIGvw_ObligationLastStatus()
        {
            return this.ObjectContext.BIGvw_ObligationLastStatus.OrderByDescending(a=>a.BIG_OperacjaId);
        }

        public IQueryable<BIGvw_ObligationLastStatus> GetBIGvw_ObligationLastStatusByCaseId(int Import_Id)
        {
            return this.ObjectContext.BIGvw_ObligationLastStatus.Where(a=>a.BIG_CaseId ==Import_Id).OrderBy(a=>a.BIG_ObligationId);
        }


        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'BIGvw_DluznicyAktual' query.

        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'BIGvw_DluznicyAktual' query.
        public IQueryable<BIGvw_DluznicyAktual> GetBIGvw_DluznicyAktual()
        {
            this.ObjectContext.CommandTimeout = 300;
            return this.ObjectContext.BIGvw_DluznicyAktual.OrderBy(a=>a.BIG_DebtorId);
        }

        public void InsertBIGvw_DluznicyAktual(BIGvw_DluznicyAktual bIGvw_DluznicyAktual)
        {
            if ((bIGvw_DluznicyAktual.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(bIGvw_DluznicyAktual, EntityState.Added);
            }
            else
            {
                this.ObjectContext.BIGvw_DluznicyAktual.AddObject(bIGvw_DluznicyAktual);
            }
        }

        public void UpdateBIGvw_DluznicyAktual(BIGvw_DluznicyAktual currentBIGvw_DluznicyAktual)
        {
            this.ObjectContext.BIGvw_DluznicyAktual.AttachAsModified(currentBIGvw_DluznicyAktual, this.ChangeSet.GetOriginal(currentBIGvw_DluznicyAktual));
        }

        public void DeleteBIGvw_DluznicyAktual(BIGvw_DluznicyAktual bIGvw_DluznicyAktual)
        {
            if ((bIGvw_DluznicyAktual.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(bIGvw_DluznicyAktual, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.BIGvw_DluznicyAktual.Attach(bIGvw_DluznicyAktual);
                this.ObjectContext.BIGvw_DluznicyAktual.DeleteObject(bIGvw_DluznicyAktual);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'BIGvw_ObligationLastStatus' query.
          public void InsertBIGvw_ObligationLastStatus(BIGvw_ObligationLastStatus bIGvw_ObligationLastStatus)
        {
            if ((bIGvw_ObligationLastStatus.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(bIGvw_ObligationLastStatus, EntityState.Added);
            }
            else
            {
                this.ObjectContext.BIGvw_ObligationLastStatus.AddObject(bIGvw_ObligationLastStatus);
            }
        }

        public void UpdateBIGvw_ObligationLastStatus(BIGvw_ObligationLastStatus currentBIGvw_ObligationLastStatus)
        {
            this.ObjectContext.BIGvw_ObligationLastStatus.AttachAsModified(currentBIGvw_ObligationLastStatus, this.ChangeSet.GetOriginal(currentBIGvw_ObligationLastStatus));
        }

        public void DeleteBIGvw_ObligationLastStatus(BIGvw_ObligationLastStatus bIGvw_ObligationLastStatus)
        {
            if ((bIGvw_ObligationLastStatus.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(bIGvw_ObligationLastStatus, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.BIGvw_ObligationLastStatus.Attach(bIGvw_ObligationLastStatus);
                this.ObjectContext.BIGvw_ObligationLastStatus.DeleteObject(bIGvw_ObligationLastStatus);
            }
        }
        #endregion BIG

        public IQueryable<DataBuffer> GetDataBuffer()
        {
            return this.ObjectContext.DataBuffer;
        }

        public void InsertDataBuffer(DataBuffer dataBuffer)
        {
            if ((dataBuffer.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dataBuffer, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DataBuffer.AddObject(dataBuffer);
            }
        }

        public void UpdateDataBuffer(DataBuffer currentDataBuffer)
        {
            this.ObjectContext.DataBuffer.AttachAsModified(currentDataBuffer, this.ChangeSet.GetOriginal(currentDataBuffer));
        }

        public void DeleteDataBuffer(DataBuffer dataBuffer)
        {
            if ((dataBuffer.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dataBuffer, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DataBuffer.Attach(dataBuffer);
                this.ObjectContext.DataBuffer.DeleteObject(dataBuffer);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DOKEKSTR_SygnLst' query.
        public IQueryable<DOKEKSTR_SygnLst> GetDOKEKSTR_SygnLst()
        {
            return this.ObjectContext.DOKEKSTR_SygnLst;
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DOKEKSTRvw_ekstrakcja' query.
        public IQueryable<DOKEKSTRvw_ekstrakcja> GetDOKEKSTRvw_ekstrakcja()
        {
            return this.ObjectContext.DOKEKSTRvw_ekstrakcja;
        }
        public IQueryable<DOKEKSTRvw_ekstrakcja> GetDOKEKSTRvw_ekstrakcjaByPakietId(string IdPakiet)
        {
            this.ObjectContext.CommandTimeout = 180;
            Guid _guid = new Guid(IdPakiet);
            return this.ObjectContext.DOKEKSTRvw_ekstrakcja.Where(a=>a.idpack==_guid).OrderBy(a=>a.id);
        }

    }



}



