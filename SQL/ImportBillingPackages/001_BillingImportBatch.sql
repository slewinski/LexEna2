-- Uruchomic w bazie LexEna wskazanej przez LexEnaMeritumEntities, przed wdrozeniem aplikacji.
-- Skrypt nie modyfikuje bazy Wiena. Bez automatycznego USE i bez kasowania danych.
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.BillingImportBatch', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BillingImportBatch
    (
        Id uniqueidentifier NOT NULL,
        Firma int NOT NULL,
        Name nvarchar(100) NOT NULL,
        SourceFileName nvarchar(260) NOT NULL,
        CreatedAt datetime NOT NULL,
        CreatedBy int NOT NULL,
        BranchId int NOT NULL,
        SymbolId int NOT NULL,
        SymbolName nvarchar(50) NOT NULL,
        FirstNumber int NOT NULL,
        [Year] int NOT NULL,
        DocumentName nvarchar(50) NOT NULL,
        BookingDate datetime NOT NULL,
        [Status] nvarchar(32) NOT NULL,
        CaseCount int NOT NULL,
        ImportedCount int NOT NULL,
        TotalAmount decimal(18, 2) NOT NULL,
        ImportXml nvarchar(max) NOT NULL,
        CONSTRAINT PK_BillingImportBatch PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT CK_BillingImportBatch_Status CHECK
            ([Status] IN (N'Draft', N'Processing', N'Completed', N'Partial', N'ReviewRequired')),
        CONSTRAINT CK_BillingImportBatch_Counts CHECK
            (CaseCount >= 0 AND ImportedCount >= 0 AND ImportedCount <= CaseCount)
    );
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.BillingImportBatch')
      AND name = N'UX_BillingImportBatch_Firma_Name'
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_BillingImportBatch_Firma_Name
        ON dbo.BillingImportBatch (Firma, Name);
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.BillingImportBatch')
      AND name = N'IX_BillingImportBatch_Firma_CreatedAt'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_BillingImportBatch_Firma_CreatedAt
        ON dbo.BillingImportBatch (Firma, CreatedAt DESC);
END;

COMMIT TRANSACTION;
GO
