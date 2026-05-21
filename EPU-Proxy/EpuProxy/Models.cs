using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;



namespace EpuProxy
{
    public enum RodzajMetody
    {
        ZlozPozwy,
        MojeSprawy
    }
    public enum TerminWykonania 
    {
        Natychmiast,
        IDLE,
        StandardOneDayRunTime

    }
    public class Zadanie 
    {
        public int Id { get; set; }
        public int RodzajMetodyValue { get; set; }
        public RodzajMetody RodzajZadania 
        {
            get { return (RodzajMetody)RodzajMetodyValue; }
            set { RodzajMetodyValue = (int)value; }
        }
        public int TerminWykonaniaValue {get; set;}
        public TerminWykonania Wykonac 
        {
            get { return (TerminWykonania)TerminWykonaniaValue; }
            set { TerminWykonaniaValue = (int)value; }
        }
        public string XMLDoWykonania { get; set; }
        public int Staus { get; set; }
        public string Wynik { get; set; }
        

    }
    public class PageOutputDataModel
    {
        public string Informacja { get; set; }
        public int IloscWszystkichElementow { get; set; }
        public string Opis { get; set; }
        public EpuSrv.KodOdpowiedzi KodOdpowiedzi { get; set; }
    }
    public class WynikWalidacjiOutputElementModel 
    {
        public string InformacjaWalidacji { get; set; }
        public EpuSrv.KodWalidacji KodWalidacji { get; set; }
        public int LiczbaPorzadkowa { get; set; }
        public string OpisWalidacji { get; set; }
        public bool Status { get; set; }
           
    }
    //Class do ListaKomorników
    public class ListaKancelariiKomorniczychOutputDataModel : PageOutputDataModel 
    {
        [Key]
        public int ListaId { get; set; }
        public virtual ICollection<ListaKancelariiKomorniczychOutputElementModel> ListaKancelariiKomorniczychOutputElementModels { get; set; } 
    }
    public class ListaKancelariiKomorniczychOutputElementModel 
    {
        public int Id { get; set; }
        public string NazwaKancelarii { get; set; }
        public virtual ListaKancelariiKomorniczychOutputDataModel ListaKancelariiKomorniczychOutputDataModel { get; set; }
    }
    //Class do Moje Sprawy
    public class MojeSprawyOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<SprawaOutputElementModel> SprawaOutputElementModels { get; set; }

    }
    public class SprawaOutputElementModel 
    {
        public int Id { get; set; }
        public int IdSprawy { get; set; }
        public DateTime? DataPrawomocnosci { get; set; }
        public DateTime? DataWplywu { get; set; }
        public DateTime? DataZakreslenia { get; set; }
        public Decimal? KwotaSporu { get; set; }
        public string RolaWSprawie { get; set; }
        public string StanSprawy { get; set; }
        public string StronyWSprawie { get; set; }
        public string SygnaturaWgPowoda { get; set; }
        public string SygnaturaSprawy { get; set; }
        public virtual MojeSprawyOutputDataModel MojeSprawyOutputDataModels { get; set; } 
    }
    //Klasy do Zlóż pozew
    public class ZlozPozewOutputDataModel 
    {
        [Key]
        public int Id { get; set; }

        public decimal LacznaWartoscPrzedmiotowSporu { get; set; }
        public int LiczbaPozwow { get; set; }
        public string OznaczeniaPaczki { get; set; }
        public decimal SumaoplatySadowej { get; set; }
        public virtual ICollection<ZlozPozewOutputElementModel> ZlozPozewOutputElementModels { get; set; } 
    }

    public class ZlozPozewOutputElementModel : WynikWalidacjiOutputElementModel 
    {
        public int Id { get; set; }
        public int KodWalidacjiPozwu {get; set;}
        public decimal? OplataSadowa { get; set; }
        public string SygnaturaWgPowoda { get; set; }
        public decimal? WartoscPrzedmiotuSporu { get; set; }
        public virtual ZlozPozewOutputDataModel ZlozPozewOutputDataModel { get; set; } 
    }
    public class ZlozZazalenieOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public string OznaczeniaPaczki { get; set; }
        public virtual ICollection<ZlozZazalenieOutputElementModel> ZlozZazalenieOutputElementModels { get; set; }
    }
    public class ZlozZazalenieOutputElementModel:WynikWalidacjiOutputElementModel
    {
        public int Id { get; set; }
        public virtual ZlozZazalenieOutputDataModel ZlozZazalenieOutputDataModels { get; set; }
    }
    //Zloz skargi
    public class ZlozSkargiOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public string OznaczeniaPaczki { get; set; }
        public virtual ICollection<ZlozSkargiOutputElementModel> ZlozSkargiOutputElementModels { get; set; }
    }
    public class ZlozSkargiOutputElementModel : WynikWalidacjiOutputElementModel
    {
        public int Id { get; set; }
        public virtual ZlozSkargiOutputDataModel ZlozSkargiOutputDataModels { get; set; }
    }
    //zloz sprzeciw
    public class ZlozSprzeciwOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public string OznaczeniaPaczki { get; set; }
        public virtual ICollection<ZlozSprzeciwOutputElementModel> ZlozSprzeciwOutputElementModels { get; set; }
    }
    public class ZlozSprzeciwOutputElementModel : WynikWalidacjiOutputElementModel
    {
        public int Id { get; set; }
        public virtual ZlozSprzeciwOutputDataModel ZlozSprzeciwOutputDataModels { get; set; }
    }
    //Zloz Wioski
    public class ZlozWnioskiOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public string OznaczeniaPaczki { get; set; }
        public virtual ICollection<ZlozWnioskiOutputElementModel> ZlozWnioskiOutputElementModels { get; set; }
    }
    public class ZlozWnioskiOutputElementModel : WynikWalidacjiOutputElementModel
    {
        public int Id { get; set; }
        public virtual ZlozWnioskiOutputDataModel ZlozWnioskiOutputDataModels { get; set; }
    }
    //Zloz dokumenty
    public class ZlozDokumentyOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public string OznaczeniaPaczki { get; set; }
        public virtual ICollection<ZlozDokumentyOutputElementModel> ZlozDokumentyOutputElementModels { get; set; }
    }
    public class ZlozDokumentyOutputElementModel : WynikWalidacjiOutputElementModel
    {
        public int Id { get; set; }
        public virtual ZlozDokumentyOutputDataModel ZlozDokumentyOutputDataModels { get; set; }
    }
    //Moje nakazy
    public class MojeNakazyOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<NakazOutputElementModel> NakazOutputElementModels { get; set; }

    }
    public class NakazOutputElementModel
    {
        public int Id { get; set; }
        public int IdNakazu { get; set; }
        public DateTime DataOrzeczenia { get; set; }
        public DateTime? DataPrawomocnosci { get; set; }
        public string DokumentXML {get; set;}
        public int KodDecyzji { get; set; }
        public string StatusDokumentu { get; set; }
        public string SygnaturaSprawy { get; set; }
        public string SygnaturaWgPowoda { get; set; }
       
        public virtual MojeNakazyOutputDataModel MojeNakazyOutputDataModels { get; set; }
    }
    public class MojeDoreczeniaOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<DoreczenieOutputElementModel> DoreczenieOutputElementModels { get; set; }

    }
    public class DoreczenieOutputElementModel
    {
        public int Id { get; set; }
        public int IdDoreczenia { get; set; }
        public DateTime? DataDoreczenia { get; set; }
        public DateTime DataWyslania { get; set; }
        public int? IdNakazu { get; set; }
        public int? IdOrzeczenia { get; set; }
        public string Opis { get; set; }
        public string SygnaturaSprawy { get; set; }
        public string SygnaturaWgPowoda { get; set; }
        public virtual MojeDoreczeniaOutputDataModel MojeDoreczeniaOutputDataModels { get; set; }
    }
    public class MojeDoreczeniaVer2OutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<DoreczenieVer2OutputElementModel> DoreczenieVer2OutputElementModels { get; set; }

    }
    public class DoreczenieVer2OutputElementModel
    {
        [Key]
        public int Id { get; set; }
        public int IdDoreczeniaVer2 { get; set; }
        public DateTime? DataDoreczenia { get; set; }
        public DateTime DataWyslania { get; set; }
        public int? IdNakazu { get; set; }
        public int? IdOrzeczenia { get; set; }
        public string Opis { get; set; }
        public string SygnaturaSprawy { get; set; }
        public string SygnaturaWgPowoda { get; set; }
        public virtual MojeDoreczeniaVer2OutputDataModel MojeDoreczeniaVer2OutputDataModels { get; set; }
    }
    public class MojeOrzeczeniaOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<OrzeczenieOutputElementModel> OrzeczenieOutputElementModels { get; set; }

    }
    public class OrzeczenieOutputElementModel
    {
        public int Id { get; set; }
        public int IdOrzeczeia { get; set; }
        public DateTime? DataKlauzuli { get; set; }
        public DateTime DataOrzeczenia { get; set; }
        public DateTime? DataPrawomocnosci { get; set; }
        public string  DokumentXML { get; set; }
        public int? Id_klauzula { get; set; }
        public string KodKlauzuli { get; set; }
        public string NazwaDecyzji { get; set; }
        public string Status { get; set; }
        public string SygnaturaSprawy { get; set; }
        public string SygnaturaWgPowoda { get; set; }
        public virtual MojeOrzeczeniaOutputDataModel MojeOrzeczeniaOutputDataModels { get; set; }
    }
    public class MojeOrzeczeniaVer2OutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<OrzeczenieVer2OutputElementModel> OrzeczenieVer2OutputElementModels { get; set; }

    }
    public class OrzeczenieVer2OutputElementModel
    {
        public int Id { get; set; }
        public int IdOrzeczeia { get; set; }
        public DateTime? DataKlauzuli { get; set; }
        public DateTime DataOrzeczenia { get; set; }
        public DateTime? DataPrawomocnosci { get; set; }
        public string DokumentXML { get; set; }
        public int? Id_klauzula { get; set; }
        public string KodKlauzuli { get; set; }
        public string NazwaDecyzji { get; set; }
        public string Status { get; set; }
        public string SygnaturaSprawy { get; set; }
        public string SygnaturaWgPowoda { get; set; }
        public int Instancja { get; set; }
        public virtual MojeOrzeczeniaVer2OutputDataModel MojeOrzeczeniaVer2OutputDataModels { get; set; }
    }
    public class MojePismaOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<DokumentOutputElementModel> DokumentOutputElementModels { get; set; }

    }
    public class DokumentOutputElementModel
    {
        public int Id { get; set; }
        public int IdDokumentu { get; set; }
        public DateTime DataDokumentu { get; set; }
        public DateTime? DataZakreslenia { get; set; }
        public EpuSrv.RodzajDokumentu RodzajDokumentu { get; set; }
        public EpuSrv.StatusDokumentu  StatusDokumentu { get; set; }
        public string SygnaturaSprawy { get; set; }
        public string SygnaturaWgPowoda { get; set; }
        public virtual MojePismaOutputDataModel MojePismaOutputDataModels { get; set; }
    }

    public class HistoriaSprawyOutputDataModel : PageOutputDataModel
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<HistoriaSprawyOutputElementModel> HistoriaSprawyOutputElementModels { get; set; }

    }
    public class HistoriaSprawyOutputElementModel
    {
        public int Id { get; set; }
        public DateTime DataZdarzenia { get; set; }
        public EpuSrv.KategoriaZdarzenia KategoriaZdarzenia { get; set; }
        public string Opis { get; set; }
        
        public virtual HistoriaSprawyOutputDataModel HistoriaSprawyOutputDataModels { get; set; }
    }
    public class EPUDbContext : DbContext
    {
        public DbSet<ListaKancelariiKomorniczychOutputDataModel> ListaKancelariiKomorniczychOutputDataModels { get; set; }
        public DbSet<ListaKancelariiKomorniczychOutputElementModel> ListaKancelariiKomorniczychOutputElementModels { get; set; }
        
        public DbSet<MojeSprawyOutputDataModel> MojeSprawyOutputDataModels { get; set; }
        public DbSet<SprawaOutputElementModel> SprawaOutputElementModels { get; set; }
        
        public DbSet<MojeNakazyOutputDataModel> MojeNakazyOutputDataModels { get; set; }
        public DbSet<NakazOutputElementModel> NakazOutputElementModels { get; set; }
        
        public DbSet<ZlozPozewOutputDataModel> ZlozPozewOutputDataModels { get; set; }
        public DbSet<ZlozPozewOutputElementModel> ZlozPozewOutputElementModels { get; set; }
        
        public DbSet<ZlozZazalenieOutputDataModel> ZlozZazalenieOutputDataModels { get; set; }
        public DbSet<ZlozZazalenieOutputElementModel> ZlozZazalenieOutputElementModels { get; set; }
        
        public DbSet<ZlozSprzeciwOutputDataModel> ZlozSprzeciwOutputDataModels { get; set; }
        public DbSet<ZlozSprzeciwOutputElementModel> ZlozSprzeciwOutputElementModels { get; set; }
        
        public DbSet<ZlozSkargiOutputDataModel> ZlozSkargiOutputDataModels { get; set; }
        public DbSet<ZlozSkargiOutputElementModel> ZlozSkargiOutputElementModels { get; set; }
        
        public DbSet<ZlozWnioskiOutputDataModel> ZlozWnioskiOutputDataModels { get; set; }
        public DbSet<ZlozWnioskiOutputElementModel> ZlozWnioskiOutputElementModels { get; set; }
        
        public DbSet<ZlozDokumentyOutputDataModel> ZlozDokumentyOutputDataModels { get; set; }
        public DbSet<ZlozDokumentyOutputElementModel> ZlozDokumentyOutputElementModels { get; set; }
        
        public DbSet<MojeDoreczeniaVer2OutputDataModel> MojeDoreczeniaVer2OutputDataModels { get; set; }
        public DbSet<DoreczenieVer2OutputElementModel> DoreczenieVer2OutputElementModels { get; set; }

        public DbSet<MojeDoreczeniaOutputDataModel> MojeDoreczeniaOutputDataModels { get; set; }
        public DbSet<DoreczenieOutputElementModel> DoreczenieOutputElementModels { get; set; }
        

        public DbSet<MojeOrzeczeniaOutputDataModel> MojeOrzeczeniaOutputDataModels { get; set; }
        public DbSet<OrzeczenieOutputElementModel> OrzeczenieOutputElementModels { get; set; }

        public DbSet<MojeOrzeczeniaVer2OutputDataModel> MojeOrzeczeniaVer2OutputDataModels { get; set; }
        public DbSet<OrzeczenieVer2OutputElementModel> OrzeczenieVer2OutputElementModels { get; set; }

        public DbSet<MojePismaOutputDataModel> MojePismaOutputDataModels { get; set; }
        public DbSet<DokumentOutputElementModel> DokumentOutputElementModels { get; set; }

        public DbSet<HistoriaSprawyOutputDataModel> HistoriaSprawyOutputDataModels { get; set; }
        public DbSet<HistoriaSprawyOutputElementModel> HistoriaSprawyOutputElementModels { get; set; }
        

//Delikatnie nie po Poskiemu lae chcę zachować konwencję
        public DbSet<Zadanie> Zadanies { get; set; }
        
        

        /*
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        }
         * public static string CreateDatabaseScript(this DbContext context)
         *    protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // TODO: Perform any fluent API configuration here!
        }
        {
            return ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
        }
         * */


    }
    //in : DropCreateDatabaseIfModelChanges<EPUDbContext>
    public class EpuProxyInitializer : CreateDatabaseIfNotExists<EPUDbContext>
    {
       
    }
     
}
