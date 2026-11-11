-- Uruchomic w bazie LexEna (LexEnaMeritumEntities), po 001_BillingImportBatch.sql.
-- WienaFirmaId wskazuje firma.ident w Wienie. Nie jest to Id jednostki LexEna.
-- Bez automatycznego USE; skrypt nie zapisuje danych do Wieny.
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;
GO

IF OBJECT_ID(N'dbo.BillingImportBatch', N'U') IS NULL
   OR OBJECT_ID(N'dbo.JednostkaWindykacji', N'U') IS NULL
BEGIN
    RAISERROR(N'Brak tabeli BillingImportBatch lub JednostkaWindykacji. Sprawdz baze i wykonaj etap 001.', 16, 1);
    RETURN;
END;

BEGIN TRANSACTION;

IF COL_LENGTH(N'dbo.JednostkaWindykacji', N'WienaFirmaId') IS NULL
    ALTER TABLE dbo.JednostkaWindykacji ADD WienaFirmaId int NULL;

IF COL_LENGTH(N'dbo.JednostkaWindykacji', N'UczestniczyWPrzydziale') IS NULL
    ALTER TABLE dbo.JednostkaWindykacji ADD UczestniczyWPrzydziale bit NOT NULL
        CONSTRAINT DF_JednostkaWindykacji_UczestniczyWPrzydziale DEFAULT (0) WITH VALUES;

IF COL_LENGTH(N'dbo.BillingImportBatch', N'DistributionXml') IS NULL
    ALTER TABLE dbo.BillingImportBatch ADD DistributionXml nvarchar(max) NULL;

IF COL_LENGTH(N'dbo.BillingImportBatch', N'DistributionUpdatedAt') IS NULL
    ALTER TABLE dbo.BillingImportBatch ADD DistributionUpdatedAt datetime NULL;

-- Stale DDL przez EXEC: SQL Server 2008 rozpoznaje kolumny po powyzszych ALTER TABLE.
IF NOT EXISTS
(
    SELECT 1 FROM sys.check_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.JednostkaWindykacji')
      AND name = N'CK_JednostkaWindykacji_Distribution'
)
    EXEC(N'ALTER TABLE dbo.JednostkaWindykacji WITH CHECK
        ADD CONSTRAINT CK_JednostkaWindykacji_Distribution CHECK
        (
            (WienaFirmaId IS NULL OR WienaFirmaId > 0)
            AND
            (UczestniczyWPrzydziale = 0 OR
                (TypJednostki IS NOT NULL AND TypJednostki = 1 AND WienaFirmaId IS NOT NULL AND WienaFirmaId > 0))
        );');

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.JednostkaWindykacji')
      AND name = N'UX_JednostkaWindykacji_Distribution_WienaFirmaId'
)
    EXEC(N'CREATE UNIQUE NONCLUSTERED INDEX UX_JednostkaWindykacji_Distribution_WienaFirmaId
        ON dbo.JednostkaWindykacji (WienaFirmaId)
        WHERE WienaFirmaId IS NOT NULL AND UczestniczyWPrzydziale = 1;');

COMMIT TRANSACTION;
GO
