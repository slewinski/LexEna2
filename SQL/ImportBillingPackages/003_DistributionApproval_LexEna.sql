-- Uruchomic w bazie LexEna (LexEnaMeritumEntities), po etapach 001 i 002.
-- Nie wykonuje dekretacji ani zmian w bazie Wiena. Bez automatycznego USE.
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
   OR COL_LENGTH(N'dbo.BillingImportBatch', N'DistributionXml') IS NULL
   OR COL_LENGTH(N'dbo.JednostkaWindykacji', N'WienaFirmaId') IS NULL
BEGIN
    RAISERROR(N'Brak schematu poprzednich etapow. Sprawdz baze i wykonaj etapy 001 oraz 002.', 16, 1);
    RETURN;
END;

BEGIN TRANSACTION;

-- Oznaczenie, nie symbol.ident: identyfikatory symboli Wieny zaleza od oddzialu.
-- Puste oznaczenie pozwala zachowac dotychczasowa konfiguracje i proponowac podzial;
-- przed dekretacja aplikacja wymaga poprawnego symbolu w oddziale danej paczki.
IF COL_LENGTH(N'dbo.JednostkaWindykacji', N'SymbolSprawWiena') IS NULL
    ALTER TABLE dbo.JednostkaWindykacji ADD SymbolSprawWiena nvarchar(10) NULL;

IF COL_LENGTH(N'dbo.BillingImportBatch', N'DistributionState') IS NULL
    ALTER TABLE dbo.BillingImportBatch ADD DistributionState nvarchar(20) NOT NULL
        CONSTRAINT DF_BillingImportBatch_DistributionState DEFAULT (N'Planning') WITH VALUES;

IF NOT EXISTS
(
    SELECT 1 FROM sys.check_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.BillingImportBatch')
      AND name = N'CK_BillingImportBatch_DistributionState'
)
    EXEC(N'ALTER TABLE dbo.BillingImportBatch WITH CHECK
        ADD CONSTRAINT CK_BillingImportBatch_DistributionState CHECK
        (DistributionState IN (N''Planning'', N''Processing'', N''Approved''));');

COMMIT TRANSACTION;
GO
