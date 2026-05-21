
namespace LexEnaTrs.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies aspnet_MembershipMetadata as the class
    // that carries additional metadata for the aspnet_Membership class.
    [MetadataTypeAttribute(typeof(aspnet_Membership.aspnet_MembershipMetadata))]
    public partial class aspnet_Membership
    {

        // This class allows you to attach custom attributes to properties
        // of the aspnet_Membership class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class aspnet_MembershipMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private aspnet_MembershipMetadata()
            {
            }

            public Guid ApplicationId { get; set; }

            public aspnet_Users aspnet_Users { get; set; }

            public string Comment { get; set; }

            public DateTime CreateDate { get; set; }

            public string Email { get; set; }

            public int FailedPasswordAnswerAttemptCount { get; set; }

            public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }

            public int FailedPasswordAttemptCount { get; set; }

            public DateTime FailedPasswordAttemptWindowStart { get; set; }

            public bool IsApproved { get; set; }

            public bool IsLockedOut { get; set; }

            public DateTime LastLockoutDate { get; set; }

            public DateTime LastLoginDate { get; set; }

            public DateTime LastPasswordChangedDate { get; set; }

            public string LoweredEmail { get; set; }

            public string MobilePIN { get; set; }

            public string Password { get; set; }

            public string PasswordAnswer { get; set; }

            public int PasswordFormat { get; set; }

            public string PasswordQuestion { get; set; }

            public string PasswordSalt { get; set; }

            public Guid UserId { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies aspnet_UsersMetadata as the class
    // that carries additional metadata for the aspnet_Users class.
    [MetadataTypeAttribute(typeof(aspnet_Users.aspnet_UsersMetadata))]
    public partial class aspnet_Users
    {

        // This class allows you to attach custom attributes to properties
        // of the aspnet_Users class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class aspnet_UsersMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private aspnet_UsersMetadata()
            {
            }

            public Guid ApplicationId { get; set; }

            public aspnet_Membership aspnet_Membership { get; set; }

            public bool IsAnonymous { get; set; }

            public DateTime LastActivityDate { get; set; }

            public string LoweredUserName { get; set; }

            public string MobileAlias { get; set; }

            public Guid UserId { get; set; }

            public string UserName { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies DaneDluznikaMetadata as the class
    // that carries additional metadata for the DaneDluznika class.
    [MetadataTypeAttribute(typeof(DaneDluznika.DaneDluznikaMetadata))]
    public partial class DaneDluznika
    {

        // This class allows you to attach custom attributes to properties
        // of the DaneDluznika class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DaneDluznikaMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DaneDluznikaMetadata()
            {
            }

            public Nullable<int> czyrejestr { get; set; }

            public Nullable<short> czyus { get; set; }

            public Nullable<DateTime> data_m { get; set; }

            public Nullable<DateTime> data_w { get; set; }

            public Nullable<int> FizPraw { get; set; }

            public string gmina { get; set; }

            public int id { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public Nullable<int> IdWienaAdres { get; set; }

            public Nullable<int> IdWienaDluzSprawa { get; set; }

            public string Imie { get; set; }

            public string Imie2 { get; set; }

            public string instytucja { get; set; }

            public string kod_pocztowy { get; set; }

            public string kraj { get; set; }

            public string krs { get; set; }

            public string miejscowosc { get; set; }

            public string Nazwa { get; set; }

            public string Nazwisko { get; set; }

            public string nip { get; set; }

            public string numer_domu { get; set; }

            public string numer_mieszkania { get; set; }

            public string numer_rejestru { get; set; }

            public Nullable<int> opr_m { get; set; }

            public Nullable<int> opr_w { get; set; }

            public string organ_rejestru { get; set; }

            public int PartitionKey { get; set; }

            public string Pesel { get; set; }

            public string poczta { get; set; }

            public string powiat { get; set; }

            public string regon { get; set; }

            public Nullable<int> rodzaj { get; set; }

            public string siedziba { get; set; }

            public Sprawa Sprawa { get; set; }

            public Nullable<int> Sprawa_Id { get; set; }

            public Nullable<int> StatusDluznika { get; set; }

            public string typ_rejestru { get; set; }

            public string ulica { get; set; }

            public string Uwagi { get; set; }

            public string wojewodztwo { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies DekretacjaMetadata as the class
    // that carries additional metadata for the Dekretacja class.
    [MetadataTypeAttribute(typeof(Dekretacja.DekretacjaMetadata))]
    public partial class Dekretacja
    {

        // This class allows you to attach custom attributes to properties
        // of the Dekretacja class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DekretacjaMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DekretacjaMetadata()
            {
            }

            public Nullable<short> Czyus { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> d_modyfikacji { get; set; }

            public Nullable<DateTime> DataDekretJednostka { get; set; }

            public Nullable<DateTime> DataDekretUser { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdUzytkownika { get; set; }

            public JednostkaWindykacji JednostkaWindykacji { get; set; }

            public Nullable<int> JednostkaWindykacji_Id { get; set; }

            public string kreator { get; set; }

            public string modyfikator { get; set; }

            public int PartitionKey { get; set; }

            public Sprawa Sprawa { get; set; }

            public int Sprawa_id { get; set; }

            public Uzytkownik Uzytkownik { get; set; }

            public Nullable<int> Uzytkownik_Id { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies DokKomunikacjaMetadata as the class
    // that carries additional metadata for the DokKomunikacja class.
    [MetadataTypeAttribute(typeof(DokKomunikacja.DokKomunikacjaMetadata))]
    public partial class DokKomunikacja
    {

        // This class allows you to attach custom attributes to properties
        // of the DokKomunikacja class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DokKomunikacjaMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DokKomunikacjaMetadata()
            {
            }

            public Nullable<DateTime> DataPrzeslania { get; set; }

            public DokWys DokWys { get; set; }

            public Nullable<int> DokWys_Id { get; set; }

            public int Id { get; set; }

            public string OpisTransmisji { get; set; }

            public string OznPaczki { get; set; }

            public Nullable<int> Paczka_id { get; set; }

            public Nullable<int> Status { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies DokOdebrMetadata as the class
    // that carries additional metadata for the DokOdebr class.
    [MetadataTypeAttribute(typeof(DokOdebr.DokOdebrMetadata))]
    public partial class DokOdebr
    {

        // This class allows you to attach custom attributes to properties
        // of the DokOdebr class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DokOdebrMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DokOdebrMetadata()
            {
            }

            public byte[] Bin { get; set; }

            public Nullable<int> CzyEPU { get; set; }

            public Nullable<short> CzyZalatw { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> d_modyfikacji { get; set; }

            public Nullable<DateTime> DataDokumentu { get; set; }

            public Nullable<DateTime> DataRejestracji { get; set; }

            public Nullable<DateTime> DataZalatwienia { get; set; }

            public Nullable<DateTime> DDoWieny { get; set; }

            public EntityCollection<DokumentKomunikacjaEPU> DokumentKomunikacjaEPU { get; set; }

            public Nullable<int> Format { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdEPU { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public Nullable<int> KontoEPU_Id { get; set; }

            public string kreator { get; set; }

            public string modyfikator { get; set; }

            public string Nazwa { get; set; }

            public int PartitionKey { get; set; }

            public Sprawa Sprawa { get; set; }

            public Nullable<int> Sprawa_id { get; set; }

            public string StatusDok { get; set; }

            public string Tresc { get; set; }

            public string TrescHtml { get; set; }

            public Nullable<int> TypDok { get; set; }

            public Nullable<int> IsChecked { get; set; }		

        }
    }

    // The MetadataTypeAttribute identifies DokumentKomunikacjaEPUMetadata as the class
    // that carries additional metadata for the DokumentKomunikacjaEPU class.
    [MetadataTypeAttribute(typeof(DokumentKomunikacjaEPU.DokumentKomunikacjaEPUMetadata))]
    public partial class DokumentKomunikacjaEPU
    {

        // This class allows you to attach custom attributes to properties
        // of the DokumentKomunikacjaEPU class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DokumentKomunikacjaEPUMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DokumentKomunikacjaEPUMetadata()
            {
            }

            public Nullable<int> czyus { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public DokOdebr DokOdebr { get; set; }

            public Nullable<int> DokOdebr_Id { get; set; }

            public DokumentOutputElementModels DokumentOutputElementModels { get; set; }

            public Nullable<int> DokumentOutputElementModels_Id { get; set; }

            public int Id { get; set; }

            public NakazOutputElementModels NakazOutputElementModels { get; set; }

            public Nullable<int> NakazOutputElementModels_Id { get; set; }

            public OrzeczenieOutputElementModels OrzeczenieOutputElementModels { get; set; }

            public Nullable<int> OrzeczenieOutputElementModels_Id { get; set; }

            public OrzeczenieVer2OutputElementModel OrzeczenieVer2OutputElementModel { get; set; }

            public Nullable<int> OrzeczenieVer2OutputElementModel_Id { get; set; }

            public Nullable<int> Status { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies DokumentOutputElementModelsMetadata as the class
    // that carries additional metadata for the DokumentOutputElementModels class.
    [MetadataTypeAttribute(typeof(DokumentOutputElementModels.DokumentOutputElementModelsMetadata))]
    public partial class DokumentOutputElementModels
    {

        // This class allows you to attach custom attributes to properties
        // of the DokumentOutputElementModels class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DokumentOutputElementModelsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DokumentOutputElementModelsMetadata()
            {
            }

            public DateTime DataDokumentu { get; set; }

            public Nullable<DateTime> DataZakreslenia { get; set; }

            public EntityCollection<DokumentKomunikacjaEPU> DokumentKomunikacjaEPU { get; set; }

            public int Id { get; set; }

            public int IdDokumentu { get; set; }

            public Nullable<int> MojePismaOutputDataModels_Id { get; set; }

            public string SygnaturaSprawy { get; set; }

            public string SygnaturaWgPowoda { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies DokumentPaczkaMetadata as the class
    // that carries additional metadata for the DokumentPaczka class.
    [MetadataTypeAttribute(typeof(DokumentPaczka.DokumentPaczkaMetadata))]
    public partial class DokumentPaczka
    {

        // This class allows you to attach custom attributes to properties
        // of the DokumentPaczka class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DokumentPaczkaMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DokumentPaczkaMetadata()
            {
            }

            public Nullable<int> czyus { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> d_modyfikacji { get; set; }

            public Nullable<DateTime> DataPrzypisania { get; set; }

            public DokWys DokWys { get; set; }

            public Nullable<int> DokWys_Id { get; set; }

            public int Id { get; set; }

            public string kreator { get; set; }

            public string modyfikator { get; set; }

            public Paczka Paczka { get; set; }

            public Nullable<int> Paczka_Id { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies DokumentWysKomunikacjaEPUMetadata as the class
    // that carries additional metadata for the DokumentWysKomunikacjaEPU class.
    [MetadataTypeAttribute(typeof(DokumentWysKomunikacjaEPU.DokumentWysKomunikacjaEPUMetadata))]
    public partial class DokumentWysKomunikacjaEPU
    {

        // This class allows you to attach custom attributes to properties
        // of the DokumentWysKomunikacjaEPU class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DokumentWysKomunikacjaEPUMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DokumentWysKomunikacjaEPUMetadata()
            {
            }

            public Nullable<int> czyus { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<int> DokumentOutputElementModels_Id { get; set; }

            public DokWys DokWys { get; set; }

            public Nullable<int> DokWys_Id { get; set; }

            public int Id { get; set; }

            public Nullable<int> PozewOutputElementModels_Id { get; set; }

            public Nullable<int> SkargaOutputElementModels_Id { get; set; }

            public Nullable<int> Status { get; set; }

            public Nullable<int> WniosekOutputElementModels_Id { get; set; }

            public Nullable<int> ZazalenieOutputElementModel_Id { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies DokWysMetadata as the class
    // that carries additional metadata for the DokWys class.
    [MetadataTypeAttribute(typeof(DokWys.DokWysMetadata))]
    public partial class DokWys
    {

        // This class allows you to attach custom attributes to properties
        // of the DokWys class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DokWysMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DokWysMetadata()
            {
            }

            public Nullable<DateTime> DataDok { get; set; }

            public EntityCollection<DokKomunikacja> DokKomunikacja { get; set; }

            public EntityCollection<DokumentPaczka> DokumentPaczka { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdPaczki { get; set; }

            public KontoEPU KontoEPU { get; set; }

            public Nullable<int> KontoEPU_Id { get; set; }

            public Nullable<decimal> Koszty { get; set; }

            public string Nazwa { get; set; }

            public string Opis { get; set; }

            public int PartitionKey { get; set; }

            [Include]
            public EntityCollection<Pozew> Pozew { get; set; }

            public Nullable<int> RodzajDok { get; set; }

            [Include]
            public Sprawa Sprawa { get; set; }

            public Nullable<int> Sprawa_id { get; set; }

            public int StatusDok { get; set; }

            public string Tresc { get; set; }

            public int TypDok { get; set; }

            public Nullable<decimal> WPS { get; set; }

            public Nullable<DateTime> DataPrawomoc { get; set; }

            public Nullable<DateTime> DataOdbioru { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> d_modyfikacji { get; set; }

            public string kreator { get; set; }

            public string modyfikator { get; set; }

            public Nullable<decimal> Kzp { get; set; }

            public Nullable<decimal> InneKoszty { get; set; }

            public Nullable<decimal> NotyOdsetkowe { get; set; }

            public Nullable<decimal> OdsetkiKapital { get; set; }
            
            public string TrescHtml { get; set; }

        }
    }
    // The MetadataTypeAttribute identifies DoreczenieOutputElementModelsMetadata as the class
    // that carries additional metadata for the DoreczenieOutputElementModels class.
    [MetadataTypeAttribute(typeof(DoreczenieOutputElementModels.DoreczenieOutputElementModelsMetadata))]
    public partial class DoreczenieOutputElementModels
    {

        // This class allows you to attach custom attributes to properties
        // of the DoreczenieOutputElementModels class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DoreczenieOutputElementModelsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DoreczenieOutputElementModelsMetadata()
            {
            }

            public Nullable<DateTime> DataDoreczenia { get; set; }

            public DateTime DataWyslania { get; set; }

            public int Id { get; set; }

            public int IdDoreczenia { get; set; }

            public Nullable<int> IdNakazu { get; set; }

            public Nullable<int> IdOrzeczenia { get; set; }

            public Nullable<int> MojeDoreczeniaOutputDataModels_Id { get; set; }

            public string Opis { get; set; }

            public string SygnaturaSprawy { get; set; }

            public string SygnaturaWgPowoda { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies DoreczenieVer2OutputElementModelMetadata as the class
    // that carries additional metadata for the DoreczenieVer2OutputElementModel class.
    [MetadataTypeAttribute(typeof(DoreczenieVer2OutputElementModel.DoreczenieVer2OutputElementModelMetadata))]
    public partial class DoreczenieVer2OutputElementModel
    {

        // This class allows you to attach custom attributes to properties
        // of the DoreczenieVer2OutputElementModel class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DoreczenieVer2OutputElementModelMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DoreczenieVer2OutputElementModelMetadata()
            {
            }

            public Nullable<DateTime> DataDoreczenia { get; set; }

            public DateTime DataWyslania { get; set; }

            public int Id { get; set; }

            public int IdDoreczeniaVer2 { get; set; }

            public Nullable<int> IdNakazu { get; set; }

            public Nullable<int> IdOrzeczenia { get; set; }

            public Nullable<int> MojeDoreczeniaVer2OutputDataModels_Id { get; set; }

            public string Opis { get; set; }

            public string SygnaturaSprawy { get; set; }

            public string SygnaturaWgPowoda { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies EdmMetadataMetadata as the class
    // that carries additional metadata for the EdmMetadata class.
    [MetadataTypeAttribute(typeof(EdmMetadata.EdmMetadataMetadata))]
    public partial class EdmMetadata
    {

        // This class allows you to attach custom attributes to properties
        // of the EdmMetadata class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class EdmMetadataMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private EdmMetadataMetadata()
            {
            }

            public int Id { get; set; }

            public string ModelHash { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies HistoriaSprawyOutputElementModelsMetadata as the class
    // that carries additional metadata for the HistoriaSprawyOutputElementModels class.
    [MetadataTypeAttribute(typeof(HistoriaSprawyOutputElementModels.HistoriaSprawyOutputElementModelsMetadata))]
    public partial class HistoriaSprawyOutputElementModels
    {

        // This class allows you to attach custom attributes to properties
        // of the HistoriaSprawyOutputElementModels class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class HistoriaSprawyOutputElementModelsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private HistoriaSprawyOutputElementModelsMetadata()
            {
            }

            public DateTime DataZdarzenia { get; set; }

            public Nullable<int> HistoriaSprawyOutputDataModels_Id { get; set; }

            public int Id { get; set; }

            public string Opis { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies JednostkaOrgMetadata as the class
    // that carries additional metadata for the JednostkaOrg class.
    [MetadataTypeAttribute(typeof(JednostkaOrg.JednostkaOrgMetadata))]
    public partial class JednostkaOrg
    {

        // This class allows you to attach custom attributes to properties
        // of the JednostkaOrg class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class JednostkaOrgMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private JednostkaOrgMetadata()
            {
            }

            public string Gmina { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdOddzial { get; set; }

            public int IdWiena { get; set; }

            public string Kod { get; set; }

            public string Kraj { get; set; }

            public string KRS { get; set; }

            public string Miasto { get; set; }

            public string Miejscowosc { get; set; }

            public string Nazwa { get; set; }

            public string NIP { get; set; }

            public string Nr_domu { get; set; }

            public string Nr_mieszkania { get; set; }

            public string Poczta { get; set; }

            public string REGON { get; set; }

            public string Siedziba { get; set; }

            public EntityCollection<Sprawa> Sprawa { get; set; }

            public EntityCollection<Sprawa> Sprawa1 { get; set; }

            public int Typ { get; set; }

            public Nullable<int> TypFirmy { get; set; }

            public string Ulica { get; set; }

            public string Wojewodztwo { get; set; }

            public string konto { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies JednostkaWindykacjiMetadata as the class
    // that carries additional metadata for the JednostkaWindykacji class.
    [MetadataTypeAttribute(typeof(JednostkaWindykacji.JednostkaWindykacjiMetadata))]
    public partial class JednostkaWindykacji
    {

        // This class allows you to attach custom attributes to properties
        // of the JednostkaWindykacji class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class JednostkaWindykacjiMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private JednostkaWindykacjiMetadata()
            {
            }

            public EntityCollection<Dekretacja> Dekretacja { get; set; }

            public int Id { get; set; }

            public EntityCollection<KontoEPU> KontoEPU { get; set; }

            public string Nazwa { get; set; }

            public int PartitionKey { get; set; }

            public EntityCollection<SposobyEgzSlownik> SposobyEgzSlownik { get; set; }

	    public int TypJednostki { get; set; }

            public EntityCollection<Uzytkownik> Uzytkownik { get; set; }
        }
    }

// The MetadataTypeAttribute identifies KancelariaKomorniczaMetadata as the class
    // that carries additional metadata for the KancelariaKomornicza class.
    [MetadataTypeAttribute(typeof(KancelariaKomornicza.KancelariaKomorniczaMetadata))]
    public partial class KancelariaKomornicza
    {

        // This class allows you to attach custom attributes to properties
        // of the KancelariaKomornicza class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class KancelariaKomorniczaMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private KancelariaKomorniczaMetadata()
            {
            }

            public int czyus { get; set; }

            public DateTime DataWprowadzenia { get; set; }

            public int Id { get; set; }

            public int IdEPU { get; set; }

            public string Nazwa { get; set; }
        }
    }



    // The MetadataTypeAttribute identifies KontoEPUMetadata as the class
    // that carries additional metadata for the KontoEPU class.
    [MetadataTypeAttribute(typeof(KontoEPU.KontoEPUMetadata))]
    public partial class KontoEPU
    {

        // This class allows you to attach custom attributes to properties
        // of the KontoEPU class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class KontoEPUMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private KontoEPUMetadata()
            {
            }

            public string APIKey { get; set; }

            public Nullable<int> Czyaktywne { get; set; }

            public int CzyZawodowy { get; set; }

            public EntityCollection<DokWys> DokWys { get; set; }

            public string EPUPasswd { get; set; }

            public string gmina { get; set; }

            public int Id { get; set; }

            public string Imie { get; set; }

            public string Imie2 { get; set; }

            public string instytucja { get; set; }

            public JednostkaWindykacji JednostkaWindykacji { get; set; }

            public Nullable<int> JednostkaWindykacji_Id { get; set; }

            public string kod_pocztowy { get; set; }

            public string kraj { get; set; }

            public string LoginEPU { get; set; }

            public string miejscowosc { get; set; }

            public string Nazwa { get; set; }

            public string NazwaKonta { get; set; }

            public string Nazwisko { get; set; }

            public string numer_domu { get; set; }

            public string numer_mieszkania { get; set; }

            public EntityCollection<Paczka> Paczka { get; set; }

            public int PartitionKey { get; set; }

            public string Pelnomocnictwo { get; set; }

            public string PESEL { get; set; }

            public string poczta { get; set; }

            public string powiat { get; set; }

            public string Stanowisko { get; set; }

            public string ulica { get; set; }

            public EntityCollection<Uzytkownik> Uzytkownik { get; set; }

            public string wojewodztwo { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies NakazOutputElementModelsMetadata as the class
    // that carries additional metadata for the NakazOutputElementModels class.
    [MetadataTypeAttribute(typeof(NakazOutputElementModels.NakazOutputElementModelsMetadata))]
    public partial class NakazOutputElementModels
    {

        // This class allows you to attach custom attributes to properties
        // of the NakazOutputElementModels class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class NakazOutputElementModelsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private NakazOutputElementModelsMetadata()
            {
            }

            public DateTime DataOrzeczenia { get; set; }

            public Nullable<DateTime> DataPrawomocnosci { get; set; }

            public EntityCollection<DokumentKomunikacjaEPU> DokumentKomunikacjaEPU { get; set; }

            public string DokumentXML { get; set; }

            public int Id { get; set; }

            public int IdNakazu { get; set; }

            public int KodDecyzji { get; set; }

            public Nullable<int> MojeNakazyOutputDataModels_Id { get; set; }

            public string StatusDokumentu { get; set; }

            public string SygnaturaSprawy { get; set; }

            public string SygnaturaWgPowoda { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies NaleznoscMetadata as the class
    // that carries additional metadata for the Naleznosc class.
    [MetadataTypeAttribute(typeof(Naleznosc.NaleznoscMetadata))]
    public partial class Naleznosc
    {

        // This class allows you to attach custom attributes to properties
        // of the Naleznosc class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class NaleznoscMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private NaleznoscMetadata()
            {
            }

            public Nullable<int> CzyNowa { get; set; }

            public Nullable<int> CzyOdsetki { get; set; }

            public Nullable<int> CzySolidarnie { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> d_modyfikacji { get; set; }

            public Nullable<DateTime> data_dok { get; set; }

            public Nullable<DateTime> data_n { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public string kreator { get; set; }

            public Nullable<decimal> kwota { get; set; }

            public string modyfikator { get; set; }

            public Nullable<int> numer { get; set; }
            [Include]
            public EntityCollection<Odsetki> Odsetki { get; set; }

            public string opis { get; set; }

            public string opis2 { get; set; }

            public int PartitionKey { get; set; }

            public Sprawa Sprawa { get; set; }

            public Nullable<int> Sprawa_id { get; set; }

            public EntityCollection<StanNaleznosci> StanNaleznosci { get; set; }

            public Nullable<int> TypNal { get; set; }

            [Include]
            public TypNaleznosci TypNaleznosci { get; set; }

            public Nullable<int> TypNaleznosci_id { get; set; }

            public Nullable<int> TypRoszczenia { get; set; }

            public Nullable<decimal> vat { get; set; }

            public Nullable<int> waluta { get; set; }
            [Include]
            public EntityCollection<WplataPodz> WplataPodz { get; set; }

            public Nullable<decimal> zaleglosc { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies NazwaStatusuMetadata as the class
    // that carries additional metadata for the NazwaStatusu class.
    [MetadataTypeAttribute(typeof(NazwaStatusu.NazwaStatusuMetadata))]
    public partial class NazwaStatusu
    {

        // This class allows you to attach custom attributes to properties
        // of the NazwaStatusu class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class NazwaStatusuMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private NazwaStatusuMetadata()
            {
            }

            public int Id { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public Nullable<int> Krok { get; set; }

            public string Nazwa { get; set; }

            public EntityCollection<StatusSprawy> StatusSprawy { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies NazwyOdsetekMetadata as the class
    // that carries additional metadata for the NazwyOdsetek class.
    [MetadataTypeAttribute(typeof(NazwyOdsetek.NazwyOdsetekMetadata))]
    public partial class NazwyOdsetek
    {

        // This class allows you to attach custom attributes to properties
        // of the NazwyOdsetek class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class NazwyOdsetekMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private NazwyOdsetekMetadata()
            {
            }

            public int CzyUstawowe { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public string Nazwa { get; set; }

            public EntityCollection<Odsetki> Odsetki { get; set; }

            public int PartitionKey { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies OdsetkiMetadata as the class
    // that carries additional metadata for the Odsetki class.
    [MetadataTypeAttribute(typeof(Odsetki.OdsetkiMetadata))]
    public partial class Odsetki
    {

        // This class allows you to attach custom attributes to properties
        // of the Odsetki class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class OdsetkiMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private OdsetkiMetadata()
            {
            }

            public Nullable<DateTime> DataK { get; set; }

            public Nullable<DateTime> DataPocz { get; set; }

            public Nullable<int> DoZaplaty { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public string Kod { get; set; }

            public Naleznosc Naleznosc { get; set; }

            public Nullable<int> Naleznosc_Id { get; set; }
            [Include]
            public NazwyOdsetek NazwyOdsetek { get; set; }

            public Nullable<int> NazwyOdsetek_Id { get; set; }

            public Nullable<int> OdWniesienia { get; set; }

            public string Opis { get; set; }

            public int PartitionKey { get; set; }

            public Nullable<decimal> Proc0 { get; set; }

            public Nullable<int> TypStopy { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies OdsTabMetadata as the class
    // that carries additional metadata for the OdsTab class.
    [MetadataTypeAttribute(typeof(OdsTab.OdsTabMetadata))]
    public partial class OdsTab
    {

        // This class allows you to attach custom attributes to properties
        // of the OdsTab class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class OdsTabMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private OdsTabMetadata()
            {
            }

            public Nullable<DateTime> DataK { get; set; }

            public Nullable<DateTime> DataP { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdTabOds { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public Nullable<decimal> Proc0 { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies OrzeczenieOutputElementModelsMetadata as the class
    // that carries additional metadata for the OrzeczenieOutputElementModels class.
    [MetadataTypeAttribute(typeof(OrzeczenieOutputElementModels.OrzeczenieOutputElementModelsMetadata))]
    public partial class OrzeczenieOutputElementModels
    {

        // This class allows you to attach custom attributes to properties
        // of the OrzeczenieOutputElementModels class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class OrzeczenieOutputElementModelsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private OrzeczenieOutputElementModelsMetadata()
            {
            }

            public Nullable<DateTime> DataKlauzuli { get; set; }

            public DateTime DataOrzeczenia { get; set; }

            public Nullable<DateTime> DataPrawomocnosci { get; set; }

            public EntityCollection<DokumentKomunikacjaEPU> DokumentKomunikacjaEPU { get; set; }

            public string DokumentXML { get; set; }

            public int Id { get; set; }

            public Nullable<int> Id_klauzula { get; set; }

            public int IdOrzeczeia { get; set; }

            public string KodKlauzuli { get; set; }

            public Nullable<int> MojeOrzeczeniaOutputDataModels_Id { get; set; }

            public string NazwaDecyzji { get; set; }

            public string Status { get; set; }

            public string SygnaturaSprawy { get; set; }

            public string SygnaturaWgPowoda { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies OrzeczenieVer2OutputElementModelMetadata as the class
    // that carries additional metadata for the OrzeczenieVer2OutputElementModel class.
    [MetadataTypeAttribute(typeof(OrzeczenieVer2OutputElementModel.OrzeczenieVer2OutputElementModelMetadata))]
    public partial class OrzeczenieVer2OutputElementModel
    {

        // This class allows you to attach custom attributes to properties
        // of the OrzeczenieVer2OutputElementModel class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class OrzeczenieVer2OutputElementModelMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private OrzeczenieVer2OutputElementModelMetadata()
            {
            }

            public Nullable<DateTime> DataKlauzuli { get; set; }

            public DateTime DataOrzeczenia { get; set; }

            public Nullable<DateTime> DataPrawomocnosci { get; set; }

            public EntityCollection<DokumentKomunikacjaEPU> DokumentKomunikacjaEPU { get; set; }

            public string DokumentXML { get; set; }

            public int Id { get; set; }

            public Nullable<int> Id_klauzula { get; set; }

            public int IdOrzeczeia { get; set; }

            public int Instancja { get; set; }

            public string KodKlauzuli { get; set; }

            public Nullable<int> MojeOrzeczeniaVer2OutputDataModels_Id { get; set; }

            public string NazwaDecyzji { get; set; }

            public string Status { get; set; }

            public string SygnaturaSprawy { get; set; }

            public string SygnaturaWgPowoda { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies PaczkaMetadata as the class
    // that carries additional metadata for the Paczka class.
    [MetadataTypeAttribute(typeof(Paczka.PaczkaMetadata))]
    public partial class Paczka
    {

        // This class allows you to attach custom attributes to properties
        // of the Paczka class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class PaczkaMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private PaczkaMetadata()
            {
            }

            public Nullable<int> czyus { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> d_modyfikacji { get; set; }

            public Nullable<DateTime> DataWyslania { get; set; }

            public Nullable<DateTime> DataZalozenia { get; set; }

            public EntityCollection<DokumentPaczka> DokumentPaczka { get; set; }

            public int Id { get; set; }

            public KontoEPU KontoEPU { get; set; }

            public Nullable<int> KontoEPU_Id { get; set; }

            public string kreator { get; set; }

            public Nullable<int> miesiac { get; set; }

            public string modyfikator { get; set; }

            public Nullable<int> nr { get; set; }

            public string Oznaczenie { get; set; }

            public EntityCollection<PaczkaKomunikacja> PaczkaKomunikacja { get; set; }

            public Nullable<int> rok { get; set; }

            public Nullable<int> StatusPaczki { get; set; }

            public Nullable<int> TypDok { get; set; }

            public Nullable<int> CzyZestaw { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies PaczkaKomunikacjaMetadata as the class
    // that carries additional metadata for the PaczkaKomunikacja class.
    [MetadataTypeAttribute(typeof(PaczkaKomunikacja.PaczkaKomunikacjaMetadata))]
    public partial class PaczkaKomunikacja
    {

        // This class allows you to attach custom attributes to properties
        // of the PaczkaKomunikacja class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class PaczkaKomunikacjaMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private PaczkaKomunikacjaMetadata()
            {
            }

            public Nullable<DateTime> DataPrzeslania { get; set; }

            public int Id { get; set; }

            public string OpisTransmisji { get; set; }

            public Paczka Paczka { get; set; }

            public Nullable<int> Paczka_Id { get; set; }

            public Nullable<int> Status { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies PozewMetadata as the class
    // that carries additional metadata for the Pozew class.
    [MetadataTypeAttribute(typeof(Pozew.PozewMetadata))]
    public partial class Pozew
    {

        // This class allows you to attach custom attributes to properties
        // of the Pozew class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class PozewMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private PozewMetadata()
            {
            }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> d_modyfikacji { get; set; }

            public Nullable<DateTime> DataZlozenia { get; set; }

            public DokWys DokWys { get; set; }

            public Nullable<int> DokWys_Id { get; set; }

            public int Id { get; set; }

            public Nullable<decimal> Koszty { get; set; }

            public string kreator { get; set; }

            public string modyfikator { get; set; }

            public string Tresc { get; set; }

            public Nullable<decimal> WPS { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies RadcaMetadata as the class
    // that carries additional metadata for the Radca class.
    [MetadataTypeAttribute(typeof(Radca.RadcaMetadata))]
    public partial class Radca
    {

        // This class allows you to attach custom attributes to properties
        // of the Radca class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class RadcaMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private RadcaMetadata()
            {
            }

            public int Id { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public string Imie { get; set; }

            public string Nazwisko { get; set; }

            public EntityCollection<Sprawa> Sprawa { get; set; }

            public string Tytul { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies SlownikMetadata as the class
    // that carries additional metadata for the Slownik class.
    [MetadataTypeAttribute(typeof(Slownik.SlownikMetadata))]
    public partial class Slownik
    {

        // This class allows you to attach custom attributes to properties
        // of the Slownik class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class SlownikMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private SlownikMetadata()
            {
            }

            public int Id { get; set; }

            public string Nazwa { get; set; }

            public string Tresc { get; set; }

            public Nullable<int> Typ { get; set; }

            public Nullable<int> filtr { get; set; }
        }
    }


    // that carries additional metadata for the SposobyEgzSlownik class.
    [MetadataTypeAttribute(typeof(SposobyEgzSlownik.SposobyEgzSlownikMetadata))]
    public partial class SposobyEgzSlownik
    {

        // This class allows you to attach custom attributes to properties
        // of the SposobyEgzSlownik class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class SposobyEgzSlownikMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private SposobyEgzSlownikMetadata()
            {
            }

            public string bankopis { get; set; }

            public int czybank { get; set; }

            public int CzyMajatek { get; set; }

            public int czynieruch { get; set; }

            public int czypraca { get; set; }

            public int czyruch { get; set; }

            public int czywierzyt { get; set; }

            public int CzyzWyboru { get; set; }

            public int Id { get; set; }
	
	        [Include]		
            public JednostkaWindykacji JednostkaWindykacji { get; set; }

            public Nullable<int> JednostkaWindykacji_Id { get; set; }

            public Nullable<int> KancelariaKomornika_Id { get; set; }

            public int KZA { get; set; }

            public Nullable<decimal> KZAKwota { get; set; }

            public string KZAOpis { get; set; }

            public string nieruchopis { get; set; }

            public string Opis { get; set; }

            public string pracaopis { get; set; }

            public string ruchopis { get; set; }

            public string wierzytopis { get; set; }
        }
    }
    // The MetadataTypeAttribute identifies SprawaMetadata as the class
    // that carries additional metadata for the Sprawa class.
    [MetadataTypeAttribute(typeof(Sprawa.SprawaMetadata))]
    public partial class Sprawa
    {

        // This class allows you to attach custom attributes to properties
        // of the Sprawa class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class SprawaMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private SprawaMetadata()
            {
            }

            public Nullable<int> CzyKoniec { get; set; }

            public Nullable<int> czyus { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> d_modyfikacji { get; set; }
            [Include]
            public EntityCollection<DaneDluznika> DaneDluznika { get; set; }

            public Nullable<DateTime> DataKon { get; set; }

            public Nullable<DateTime> DataR { get; set; }

            public Nullable<DateTime> DataZloPozwu { get; set; }

            public EntityCollection<Dekretacja> Dekretacja { get; set; }
            [Include]
            public EntityCollection<DokOdebr> DokOdebr { get; set; }

            public EntityCollection<DokWys> DokWys { get; set; }

            public int id { get; set; }

            public Nullable<int> IdSprawyEPU { get; set; }

            public Nullable<int> IdSymbol { get; set; }

            public Nullable<int> IdWiena { get; set; }
            [Include]
            public JednostkaOrg JednostkaOrg { get; set; }

            public JednostkaOrg JednostkaOrg1 { get; set; }

            public Nullable<int> JednostkaOrgOddzial_Id { get; set; }

            public Nullable<int> JednostkaOrgRejon_Id { get; set; }

            public string kreator { get; set; }

            public string LicznikMiejsce { get; set; }

            public string LicznikUlica { get; set; }

            public Nullable<int> LiczPrzedplat { get; set; }

            public string modyfikator { get; set; }
            [Include]
            public EntityCollection<Naleznosc> Naleznosc { get; set; }

            public string NrEwid { get; set; }

            public int PartitionKey { get; set; }

            public int Poz { get; set; }

            public Radca Radca { get; set; }

            public Nullable<int> Radca_Id { get; set; }

            public int Rok { get; set; }

            public string Skrot { get; set; }

            public EntityCollection<SprawaKomunikacjaEPU> SprawaKomunikacjaEPU { get; set; }

            public EntityCollection<StanSprawy> StanSprawy { get; set; }

            public EntityCollection<StatusSprawy> StatusSprawy { get; set; }

            public string sygnatura { get; set; }

            public string SygnNCe { get; set; }

            public string Uwagi { get; set; }
            [Include]
            public EntityCollection<Wplata> Wplata { get; set; }

            public Nullable<decimal> WPS { get; set; }

            public Nullable<decimal> KosztyZadane { get; set; }
	        
            public Nullable<decimal> KosztyZasadzone { get; set; }
        
            public Nullable<decimal> KosztyPrawomocne { get; set; }
	
            public Nullable<decimal> KzpZadane { get; set; }
	
            public Nullable<decimal> KzpZasadzone { get; set; }
	
            public Nullable<decimal> KzpPrawomocne { get; set; }

            public Nullable<decimal> InneZadane { get; set; }
	
            public Nullable<decimal> InneZasadzone { get; set; }

            public Nullable<decimal> InnePrawomocne { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies SprawaKomunikacjaEPUMetadata as the class
    // that carries additional metadata for the SprawaKomunikacjaEPU class.
    [MetadataTypeAttribute(typeof(SprawaKomunikacjaEPU.SprawaKomunikacjaEPUMetadata))]
    public partial class SprawaKomunikacjaEPU
    {

        // This class allows you to attach custom attributes to properties
        // of the SprawaKomunikacjaEPU class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class SprawaKomunikacjaEPUMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private SprawaKomunikacjaEPUMetadata()
            {
            }

            public Nullable<int> czyus { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public int Id { get; set; }

            public Sprawa Sprawa { get; set; }

            public Nullable<int> Sprawa_Id { get; set; }

            public SprawaOutputElementModels SprawaOutputElementModels { get; set; }

            public Nullable<int> SprawaOutputElementModels_Id { get; set; }

            public Nullable<int> Status { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies SprawaOutputElementModelsMetadata as the class
    // that carries additional metadata for the SprawaOutputElementModels class.
    [MetadataTypeAttribute(typeof(SprawaOutputElementModels.SprawaOutputElementModelsMetadata))]
    public partial class SprawaOutputElementModels
    {

        // This class allows you to attach custom attributes to properties
        // of the SprawaOutputElementModels class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class SprawaOutputElementModelsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private SprawaOutputElementModelsMetadata()
            {
            }

            public Nullable<DateTime> DataPrawomocnosci { get; set; }

            public Nullable<DateTime> DataWplywu { get; set; }

            public Nullable<DateTime> DataZakreslenia { get; set; }

            public int Id { get; set; }

            public int IdSprawy { get; set; }

            public Nullable<decimal> KwotaSporu { get; set; }

            public Nullable<int> MojeSprawyOutputDataModels_Id { get; set; }

            public string RolaWSprawie { get; set; }

            public EntityCollection<SprawaKomunikacjaEPU> SprawaKomunikacjaEPU { get; set; }

            public string StanSprawy { get; set; }

            public string StronyWSprawie { get; set; }

            public string SygnaturaSprawy { get; set; }

            public string SygnaturaWgPowoda { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies StanNaleznosciMetadata as the class
    // that carries additional metadata for the StanNaleznosci class.
    [MetadataTypeAttribute(typeof(StanNaleznosci.StanNaleznosciMetadata))]
    public partial class StanNaleznosci
    {

        // This class allows you to attach custom attributes to properties
        // of the StanNaleznosci class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class StanNaleznosciMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private StanNaleznosciMetadata()
            {
            }

            public Nullable<int> CzyNowy { get; set; }

            public Nullable<DateTime> data_s { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public Nullable<decimal> kwota_k { get; set; }

            public Nullable<decimal> kwota_n { get; set; }

            public Nullable<decimal> kwota_o { get; set; }

            [Include]
            public Naleznosc Naleznosc { get; set; }

            public Nullable<int> Naleznosc_Id { get; set; }

            public Nullable<int> oblicz { get; set; }

            public Nullable<decimal> ods_dzien { get; set; }

            public Nullable<int> typ_stan { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies StanSprawyMetadata as the class
    // that carries additional metadata for the StanSprawy class.
    [MetadataTypeAttribute(typeof(StanSprawy.StanSprawyMetadata))]
    public partial class StanSprawy
    {

        // This class allows you to attach custom attributes to properties
        // of the StanSprawy class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class StanSprawyMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private StanSprawyMetadata()
            {
            }

            public Nullable<int> CzyNowy { get; set; }

            public Nullable<DateTime> data_s { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public Nullable<decimal> kwota_k { get; set; }

            public Nullable<decimal> kwota_n { get; set; }

            public Nullable<decimal> kwota_o { get; set; }

            public Nullable<int> oblicz { get; set; }

            public Nullable<decimal> ods_dzien { get; set; }

            public Nullable<int> PartitionKey { get; set; }

            public Sprawa Sprawa { get; set; }

            public Nullable<int> Sprawa_Id { get; set; }

            public Nullable<int> typ_stan { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies StatusSprawyMetadata as the class
    // that carries additional metadata for the StatusSprawy class.
    [MetadataTypeAttribute(typeof(StatusSprawy.StatusSprawyMetadata))]
    public partial class StatusSprawy
    {

        // This class allows you to attach custom attributes to properties
        // of the StatusSprawy class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class StatusSprawyMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private StatusSprawyMetadata()
            {
            }

            public Nullable<int> czyus { get; set; }

            public Nullable<int> CzyWiena { get; set; }

            public Nullable<DateTime> DataStatusu { get; set; }

            public Nullable<DateTime> DataWiena { get; set; }

            public Nullable<int> ExtraStat { get; set; }

	    public int Id { get; set; }

            public NazwaStatusu NazwaStatusu { get; set; }

            public Nullable<int> NazwaStatusu_Id { get; set; }

            public Sprawa Sprawa { get; set; }

            public Nullable<int> Sprawa_id { get; set; }
        }
    }
// The MetadataTypeAttribute identifies TerminMetadata as the class
    // that carries additional metadata for the Termin class.
    [MetadataTypeAttribute(typeof(Termin.TerminMetadata))]
    public partial class Termin
    {

        // This class allows you to attach custom attributes to properties
        // of the Termin class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TerminMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TerminMetadata()
            {
            }

            public Nullable<int> Co { get; set; }

            public Nullable<int> czyus { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> d_modyfikacji { get; set; }

            public Nullable<DateTime> DataDoWykonania { get; set; }

            public Nullable<DateTime> DataWykonania { get; set; }

            public Nullable<DateTime> DataZapisu { get; set; }

            public int Id { get; set; }

            public string kreator { get; set; }

            public string modyfikator { get; set; }

            public string Opis { get; set; }

            public Nullable<int> Ref_Id { get; set; }

            public Sprawa Sprawa { get; set; }

            public Nullable<int> Sprawa_Id { get; set; }

            public Nullable<int> Status { get; set; }

            public TerminTyp TerminTyp { get; set; }

            public Nullable<int> TerminTyp_Id { get; set; }
        }
    }


    // The MetadataTypeAttribute identifies TerminTypMetadata as the class
    // that carries additional metadata for the TerminTyp class.
    [MetadataTypeAttribute(typeof(TerminTyp.TerminTypMetadata))]
    public partial class TerminTyp
    {

        // This class allows you to attach custom attributes to properties
        // of the TerminTyp class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TerminTypMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TerminTypMetadata()
            {
            }

            public Nullable<int> AlertAfterDays { get; set; }

            public Nullable<int> AlertEveryDayInMonth { get; set; }

            public Nullable<int> czyus { get; set; }

            public int Id { get; set; }

            public string Nazwa { get; set; }

            public int Numer { get; set; }

            public EntityCollection<Termin> Termin { get; set; }
        }
    }


    // The MetadataTypeAttribute identifies TypNaleznosciMetadata as the class
    // that carries additional metadata for the TypNaleznosci class.
    [MetadataTypeAttribute(typeof(TypNaleznosci.TypNaleznosciMetadata))]
    public partial class TypNaleznosci
    {

        // This class allows you to attach custom attributes to properties
        // of the TypNaleznosci class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TypNaleznosciMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TypNaleznosciMetadata()
            {
            }

            public Nullable<bool> CzyMem { get; set; }

            public int CzyOdsKapital { get; set; }

            public bool CzyProc { get; set; }

            public int id { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public EntityCollection<Naleznosc> Naleznosc { get; set; }

            public string Nazwa { get; set; }

            public int PartitionKey { get; set; }

            public Nullable<int> TabOds { get; set; }

            public int TypNal { get; set; }

            public int TypSum { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies TypZadaniaSetMetadata as the class
    // that carries additional metadata for the TypZadaniaSet class.
    [MetadataTypeAttribute(typeof(TypZadaniaSet.TypZadaniaSetMetadata))]
    public partial class TypZadaniaSet
    {

        // This class allows you to attach custom attributes to properties
        // of the TypZadaniaSet class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TypZadaniaSetMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TypZadaniaSetMetadata()
            {
            }

            public int Id { get; set; }

            public string Opis { get; set; }

            public bool RownolegleUruchamianie { get; set; }

            public EntityCollection<ZadanieSet> ZadanieSet { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies UzytkownikMetadata as the class
    // that carries additional metadata for the Uzytkownik class.
    [MetadataTypeAttribute(typeof(Uzytkownik.UzytkownikMetadata))]
    public partial class Uzytkownik
    {

        // This class allows you to attach custom attributes to properties
        // of the Uzytkownik class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UzytkownikMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UzytkownikMetadata()
            {
            }

            public EntityCollection<Dekretacja> Dekretacja { get; set; }

            public int Id { get; set; }

            public string Imie { get; set; }

            public string Inicjal { get; set; }

            public JednostkaWindykacji JednostkaWindykacji { get; set; }

            public Nullable<int> JednostkaWindykacji_Id { get; set; }
            [Include]
            public KontoEPU KontoEPU { get; set; }

            public Nullable<int> KontoEPU_Id { get; set; }

            public string Nazwisko { get; set; }

            public int PartitionKey { get; set; }

            public Nullable<int> Rola { get; set; }

            public Nullable<int> Status { get; set; }

            public string UserName { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies vw_DaneSprawyMetadata as the class
    // that carries additional metadata for the vw_DaneSprawy class.
    [MetadataTypeAttribute(typeof(vw_DaneSprawy.vw_DaneSprawyMetadata))]
    public partial class vw_DaneSprawy
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_DaneSprawy class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_DaneSprawyMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_DaneSprawyMetadata()
            {
            }

            public Nullable<DateTime> DataStatusu { get; set; }

            public int id { get; set; }

            public string JednWindykacji { get; set; }

            public string Oddzial { get; set; }

            public string Referent { get; set; }

            public string Rejon { get; set; }

            public string Status { get; set; }
        }
    }

// The MetadataTypeAttribute identifies vw_DokOdebrCountMetadata as the class
    // that carries additional metadata for the vw_DokOdebrCount class.
    [MetadataTypeAttribute(typeof(vw_DokOdebrCount.vw_DokOdebrCountMetadata))]
    public partial class vw_DokOdebrCount
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_DokOdebrCount class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_DokOdebrCountMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_DokOdebrCountMetadata()
            {
            }

            public int Id_Jednostki { get; set; }

            public int Id_User { get; set; }

            public Nullable<int> IsChecked { get; set; }

            public Nullable<int> TypDok { get; set; }

            public Nullable<int> Ilosc { get; set; }

            public int Id {get; set; }
        }
    }

    // The MetadataTypeAttribute identifies vw_KomunikacjaDocMetadata as the class
    // that carries additional metadata for the vw_KomunikacjaDoc class.
    [MetadataTypeAttribute(typeof(vw_KomunikacjaDoc.vw_KomunikacjaDocMetadata))]
    public partial class vw_KomunikacjaDoc
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_KomunikacjaDoc class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_KomunikacjaDocMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_KomunikacjaDocMetadata()
            {
            }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<int> DokWys_Id { get; set; }

            public int Id { get; set; }

            public Nullable<int> KodWalidacjiPozwu { get; set; }

            public string OpisWalidacji { get; set; }

            public Nullable<int> Status { get; set; }
        }
    }


    // The MetadataTypeAttribute identifies vw_ListaDokPaczkiMetadata as the class
    // that carries additional metadata for the vw_ListaDokPaczki class.
    [MetadataTypeAttribute(typeof(vw_ListaDokPaczki.vw_ListaDokPaczkiMetadata))]
    public partial class vw_ListaDokPaczki
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_ListaDokPaczki class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_ListaDokPaczkiMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_ListaDokPaczkiMetadata()
            {
            }

            public DateTime DataDekretJednostka { get; set; }

	    public Nullable<DateTime> DataDok { get; set; }

            public Nullable<DateTime> DataStatusu { get; set; }

            public string Dluznik { get; set; }

            public int Id { get; set; }

            public int Id_Jednostki { get; set; }

            public int Id_User { get; set; }

            public int IdDokumentPaczka { get; set; }

            public int IdPaczki { get; set; }

            public int IdSprawy { get; set; }

            public Nullable<decimal> Koszty { get; set; }

            public string Nazwa_Jednostki { get; set; }

            public string NazwaDok { get; set; }

            public string NazwaStatusuSprawy { get; set; }

            public string Oddzial { get; set; }

            public int StatusDok { get; set; }

            public int StatusKomunikacji { get; set; }

            public string Sygnatura { get; set; }

            public int TypDok { get; set; }

            public string Uzytkownik_Nazwa { get; set; }

            public Nullable<long> Wielkosc { get; set; }

            public Nullable<decimal> wps { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies vw_ListaDokPaczkiDoSaldaMetadata as the class
    // that carries additional metadata for the vw_ListaDokPaczkiDoSalda class.
    [MetadataTypeAttribute(typeof(vw_ListaDokPaczkiDoSalda.vw_ListaDokPaczkiDoSaldaMetadata))]
    public partial class vw_ListaDokPaczkiDoSalda
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_ListaDokPaczkiDoSalda class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_ListaDokPaczkiDoSaldaMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_ListaDokPaczkiDoSaldaMetadata()
            {
            }

            public string Dluznik { get; set; }

            public int IdDokumentPaczka { get; set; }

            public int IdPaczki { get; set; }

            public string NrEwid { get; set; }

            public string Oddzial { get; set; }

            public string Sygnatura { get; set; }

            public string Puste { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies vw_ListaDokWysMetadata as the class
    // that carries additional metadata for the vw_ListaDokWys class.
    [MetadataTypeAttribute(typeof(vw_ListaDokWys.vw_ListaDokWysMetadata))]
    public partial class vw_ListaDokWys
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_ListaDokWys class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_ListaDokWysMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_ListaDokWysMetadata()
            {
            }

            public DateTime DataDekretJednostka { get; set; }

	    public Nullable<DateTime> DataDok { get; set; }

            public Nullable<DateTime> DataStatusu { get; set; }

            public string Dluznik { get; set; }

            public int Id { get; set; }

            public int Id_Jednostki { get; set; }

            public int Id_User { get; set; }

            public int IdSprawy { get; set; }

            public int Krok { get; set; }

            public string Nazwa_Jednostki { get; set; }

            public string NazwaDok { get; set; }

            public string NazwaStatusuSprawy { get; set; }

            public string Oddzial { get; set; }

            public string Paczka { get; set; }

            public int StatusDok { get; set; }

            public int StatusKomunikacji { get; set; }

            public string Sygnatura { get; set; }

            public int TypDok { get; set; }

            public string Uzytkownik_Nazwa { get; set; }

            public string Saldo { get; set; }

            public string NrEwid { get; set; }
        }
    }


// The MetadataTypeAttribute identifies vw_ListaDoOdebrMetadata as the class
    // that carries additional metadata for the vw_ListaDoOdebr class.
    [MetadataTypeAttribute(typeof(vw_ListaDoOdebr.vw_ListaDoOdebrMetadata))]
    public partial class vw_ListaDoOdebr
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_ListaDoOdebr class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_ListaDoOdebrMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_ListaDoOdebrMetadata()
            {
            }

            public DateTime DataDekretJednostka { get; set; }
	    
            public Nullable<DateTime> DataDokumentu { get; set; }

            public Nullable<DateTime> DataRejestracji { get; set; }

            public Nullable<DateTime> DataStatusu { get; set; }

            public string Dluznik { get; set; }

            public int Id { get; set; }

            public int Id_Jednostki { get; set; }

            public int Id_User { get; set; }

            public int IdSprawy { get; set; }

            public int Krok { get; set; }

            public string Nazwa_Jednostki { get; set; }

            public string NazwaDok { get; set; }

            public string NazwaStatusuSprawy { get; set; }

            public string Oddzial { get; set; }

            public string StatusDok { get; set; }

            public int StatusKomunikacji { get; set; }

            public string Sygnatura { get; set; }

            public Nullable<int> TypDok { get; set; }

            public string Uzytkownik_Nazwa { get; set; }

	    public Nullable<int> IsChecked { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies vw_ListaPaczekMetadata as the class
    // that carries additional metadata for the vw_ListaPaczek class.
    [MetadataTypeAttribute(typeof(vw_ListaPaczek.vw_ListaPaczekMetadata))]
    public partial class vw_ListaPaczek
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_ListaPaczek class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_ListaPaczekMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_ListaPaczekMetadata()
            {
            }

            public Nullable<DateTime> DataWyslania { get; set; }

            public Nullable<DateTime> DataZalozenia { get; set; }

            public int Id { get; set; }

            public Nullable<int> LDoks { get; set; }

            public Nullable<int> miesiac { get; set; }

            public Nullable<int> nr { get; set; }

            public Nullable<decimal> OplataSadowa { get; set; }

            public string Oznaczenie { get; set; }

            public Nullable<int> rok { get; set; }

            public Nullable<int> StatusPaczki { get; set; }

            public Nullable<int> TypDok { get; set; }

            public Nullable<long> Wielkosc { get; set; }

            public Nullable<int> CzyZestaw { get; set; }

            public Nullable<int> KontoEPU_Id { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies vw_ListaSprawMetadata as the class
    // that carries additional metadata for the vw_ListaSpraw class.
    [MetadataTypeAttribute(typeof(vw_ListaSpraw.vw_ListaSprawMetadata))]
    public partial class vw_ListaSpraw
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_ListaSpraw class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_ListaSprawMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_ListaSprawMetadata()
            {
            }

            public DateTime DataDekretJednostka { get; set; }

	    public Nullable<DateTime> DataR { get; set; }

            public Nullable<DateTime> DataStatusu { get; set; }

            public string Dluznik { get; set; }

	    public int ExtraStatus { get; set; }

            public int id { get; set; }

            public int Id_Jednostki { get; set; }

            public int Id_User { get; set; }

            public int IdStatusu { get; set; }

            public int Krok { get; set; }

            public string Nazwa { get; set; }

            public string Nazwa_Jednostki { get; set; }

	        public string NazwaExtraStat { get; set; }

            public string NrEwid { get; set; }

            public string Oddzial { get; set; }

            public DateTime OstatniDataDok { get; set; }

            public string OstatniDok { get; set; }

            public int OstatniStatusDok { get; set; }

            public string sygnatura { get; set; }

            public string Uzytkownik_Nazwa { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies vw_PaczkiMetadata as the class
    // that carries additional metadata for the vw_Paczki class.
    [MetadataTypeAttribute(typeof(vw_Paczki.vw_PaczkiMetadata))]
    public partial class vw_Paczki
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_Paczki class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_PaczkiMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_PaczkiMetadata()
            {
            }

            public Nullable<DateTime> DataWyslania { get; set; }

            public Nullable<DateTime> DataZalozenia { get; set; }

            public int Id { get; set; }

            public Nullable<int> LDoks { get; set; }

            public Nullable<int> miesiac { get; set; }

            public Nullable<int> nr { get; set; }

            public string Oznaczenie { get; set; }

            public Nullable<int> rok { get; set; }

            public Nullable<int> StatusPaczki { get; set; }

            public Nullable<int> TypDok { get; set; }

            public Nullable<int> KontoEPU_Id { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies vw_SearchMetadata as the class
    // that carries additional metadata for the vw_Search class.
    [MetadataTypeAttribute(typeof(vw_Search.vw_SearchMetadata))]
    public partial class vw_Search
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_Search class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_SearchMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_SearchMetadata()
            {
            }

            public Nullable<DateTime> DataStatusu { get; set; }

            public string Dluznik { get; set; }

            public int id { get; set; }

            public int Id_Jednostki { get; set; }

            public int Id_User { get; set; }

            public Nullable<int> IdRow { get; set; }

            public string miejscowosc { get; set; }

            public string Nazwa_Jednostki { get; set; }

            public string NazwaStatusu { get; set; }

            public string NrEwid { get; set; }

            public string opis { get; set; }

            public string pesel { get; set; }

            public string poczta { get; set; }

            public string sygnatura { get; set; }

            public string SygnNCe { get; set; }

            public string ulica { get; set; }

            public string Uzytkownik_Nazwa { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies vw_SprawaKomunikacjaEPUMetadata as the class
    // that carries additional metadata for the vw_SprawaKomunikacjaEPU class.
    [MetadataTypeAttribute(typeof(vw_SprawaKomunikacjaEPU.vw_SprawaKomunikacjaEPUMetadata))]
    public partial class vw_SprawaKomunikacjaEPU
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_SprawaKomunikacjaEPU class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_SprawaKomunikacjaEPUMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_SprawaKomunikacjaEPUMetadata()
            {
            }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> DataPrawomocnosci { get; set; }

            public Nullable<DateTime> DataWplywu { get; set; }

            public int id { get; set; }

            public int IdSprawyEPU { get; set; }

            public string StanSprawy { get; set; }

            public Nullable<int> status { get; set; }

            public string SygnaturaSprawy { get; set; }

            public string SygnaturaWgPowoda { get; set; }

            public Nullable<int> IdKomunikacji { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies vw_SprawyCzynneMetadata as the class
    // that carries additional metadata for the vw_SprawyCzynne class.
    [MetadataTypeAttribute(typeof(vw_SprawyCzynne.vw_SprawyCzynneMetadata))]
    public partial class vw_SprawyCzynne
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_SprawyCzynne class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_SprawyCzynneMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_SprawyCzynneMetadata()
            {
            }

            public int id { get; set; }

            public Nullable<int> IdWiena { get; set; }
        }
    }

// The MetadataTypeAttribute identifies vw_TerminyMetadata as the class
    // that carries additional metadata for the vw_Terminy class.
    [MetadataTypeAttribute(typeof(vw_Terminy.vw_TerminyMetadata))]
    public partial class vw_Terminy
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_Terminy class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_TerminyMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_TerminyMetadata()
            {
            }

            public Nullable<int> Co { get; set; }

            public Nullable<DateTime> DataDoWykonania { get; set; }

            public Nullable<DateTime> DataZapisu { get; set; }

            public string Dluznik { get; set; }

            public int Id { get; set; }

            public Nullable<int> Id_Context { get; set; }

            public int Id_Jednostki { get; set; }

            public int Id_User { get; set; }

            public Nullable<int> IdSprawyEPU { get; set; }

            public string Nazwa { get; set; }

            public string Nazwa_Jednostki { get; set; }

            public string NazwaStatusuSprawy { get; set; }

            public string Opis { get; set; }

            public Nullable<int> Ref_Id { get; set; }

            public Nullable<int> Status { get; set; }

            public string Sygnatura { get; set; }

            public string SygnNCe { get; set; }

            public string Uzytkownik_Nazwa { get; set; }
        }
    }

// The MetadataTypeAttribute identifies vw_tmpZmianaAdresuPelnMetadata as the class
    // that carries additional metadata for the vw_tmpZmianaAdresuPeln class.
    [MetadataTypeAttribute(typeof(vw_tmpZmianaAdresuPeln.vw_tmpZmianaAdresuPelnMetadata))]
    public partial class vw_tmpZmianaAdresuPeln
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_tmpZmianaAdresuPeln class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_tmpZmianaAdresuPelnMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_tmpZmianaAdresuPelnMetadata()
            {
            }

            public int id { get; set; }

            public Nullable<int> IdSprawyEPU { get; set; }

            public string sygnatura { get; set; }

            public string SygnNce { get; set; }
        }
    }
    // The MetadataTypeAttribute identifies vw_UsersAspNetMetadata as the class
    // that carries additional metadata for the vw_UsersAspNet class.
    [MetadataTypeAttribute(typeof(vw_UsersAspNet.vw_UsersAspNetMetadata))]
    public partial class vw_UsersAspNet
    {

        // This class allows you to attach custom attributes to properties
        // of the vw_UsersAspNet class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vw_UsersAspNetMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private vw_UsersAspNetMetadata()
            {
            }

            public int Id { get; set; }

            public string Imie { get; set; }

            public bool IsApproved { get; set; }

            public string Nazwisko { get; set; }

            public Guid UserId { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies WplataMetadata as the class
    // that carries additional metadata for the Wplata class.
    [MetadataTypeAttribute(typeof(Wplata.WplataMetadata))]
    public partial class Wplata
    {

        // This class allows you to attach custom attributes to properties
        // of the Wplata class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class WplataMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private WplataMetadata()
            {
            }

            public Nullable<short> AlgorytmZaliczania { get; set; }

            public Nullable<int> CzyNowa { get; set; }

            public Nullable<DateTime> d_kreacji { get; set; }

            public Nullable<DateTime> d_modyfikacji { get; set; }

            public Nullable<DateTime> DataWplaty { get; set; }

            public int Id { get; set; }

            public Nullable<int> IdKtoWplata { get; set; }

            public Nullable<int> IdWiena { get; set; }

            public Nullable<int> kreator { get; set; }

            public decimal Kwota { get; set; }

            public Nullable<int> modyfikator { get; set; }

            public string NrDowodu { get; set; }

            public int PartitionKey { get; set; }

            public Sprawa Sprawa { get; set; }

            public Nullable<int> Sprawa_id { get; set; }

            public string Uwagi { get; set; }

            public EntityCollection<WplataPodz> WplataPodz { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies WplataPodzMetadata as the class
    // that carries additional metadata for the WplataPodz class.
    [MetadataTypeAttribute(typeof(WplataPodz.WplataPodzMetadata))]
    public partial class WplataPodz
    {

        // This class allows you to attach custom attributes to properties
        // of the WplataPodz class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class WplataPodzMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private WplataPodzMetadata()
            {
            }

            public int Id { get; set; }

            public Nullable<int> IdWiena { get; set; }
            [Include]
            public Naleznosc Naleznosc { get; set; }

            public Nullable<int> Naleznosc_Id { get; set; }

            public int PartitionKey { get; set; }

            public Nullable<decimal> SplataKapital { get; set; }

            public Nullable<decimal> SplataOdsetki { get; set; }

            public Nullable<decimal> SplataVat { get; set; }

            public Wplata Wplata { get; set; }

            public Nullable<int> Wplata_Id { get; set; }

            public Nullable<decimal> ZalegloscKapital { get; set; }

            public Nullable<decimal> ZalegloscOdsetki { get; set; }

            public Nullable<decimal> ZalegloscVat { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies ZadaniesMetadata as the class
    // that carries additional metadata for the Zadanies class.
    [MetadataTypeAttribute(typeof(Zadanies.ZadaniesMetadata))]
    public partial class Zadanies
    {

        // This class allows you to attach custom attributes to properties
        // of the Zadanies class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ZadaniesMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ZadaniesMetadata()
            {
            }

            public int Id { get; set; }

            public int RodzajMetodyValue { get; set; }

            public int Staus { get; set; }

            public int TerminWykonaniaValue { get; set; }

            public string Wynik { get; set; }

            public string XMLDoWykonania { get; set; }

            
        }
    }

    // The MetadataTypeAttribute identifies ZadanieSetMetadata as the class
    // that carries additional metadata for the ZadanieSet class.
    [MetadataTypeAttribute(typeof(ZadanieSet.ZadanieSetMetadata))]
    public partial class ZadanieSet
    {

        // This class allows you to attach custom attributes to properties
        // of the ZadanieSet class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ZadanieSetMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ZadanieSetMetadata()
            {
            }

            public DateTime DataRozpoczęcia { get; set; }

            public Nullable<DateTime> DataZakonczenia { get; set; }

            public int Id { get; set; }

            public string NazwaZadania { get; set; }

            public string Opis { get; set; }

            public string Parametry { get; set; }

            public int Status { get; set; }

            public int TypZadaniaId { get; set; }

            public TypZadaniaSet TypZadaniaSet { get; set; }

            public bool Oczasie { get; set; }

            public Nullable<int> JednostkaWindykacji_Id { get; set; }
        }
    }
}
