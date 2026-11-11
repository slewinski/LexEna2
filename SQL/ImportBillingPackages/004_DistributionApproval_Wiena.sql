-- Uruchomic w bazie WIENA (wiena_centralEntities), nie w bazie LexEna.
-- Przed wdrozeniem aplikacji. Nie dekretuje zadnych spraw i nie naprawia danych.
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;
GO

IF OBJECT_ID(N'dbo.sprawa', N'U') IS NULL
   OR COL_LENGTH(N'dbo.sprawa', N'id_symbol') IS NULL
BEGIN
    RAISERROR(N'Brak tabeli sprawa w bazie Wiena. Sprawdz wybrana baze.', 16, 1);
    RETURN;
END;

BEGIN TRANSACTION;

-- Ochrona przed nadaniem tego samego numeru przez rownolegly zapis, takze poza LexEna.
-- Uwzgledniamy rowniez sprawy usuniete: zajety numer nie jest odzyskiwany.
IF EXISTS
(
    SELECT 1 FROM dbo.sprawa
    WHERE id_symbol IS NOT NULL AND poz > 0 AND rok > 0
    GROUP BY id_firmy, id_symbol, rok, poz
    HAVING COUNT(*) > 1
)
BEGIN
    ROLLBACK TRANSACTION;
    RAISERROR(N'Istnieja powtorzone numery spraw (oddzial, symbol, rok, pozycja). Wyjasnij duplikaty przed wdrozeniem dekretacji. Skrypt nie zmienil danych.', 16, 1);
    RETURN;
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.sprawa')
      AND name = N'UX_sprawa_LexEna_Numbering'
)
    CREATE UNIQUE NONCLUSTERED INDEX UX_sprawa_LexEna_Numbering
        ON dbo.sprawa (id_firmy, id_symbol, rok, poz)
        WHERE id_symbol IS NOT NULL AND poz > 0 AND rok > 0;

-- Potwierdzenie jest zapisywane w tej samej transakcji co nowe sygnatury.
-- Pozwala odzyskac wynik po utracie odpowiedzi lub awarii zapisu do bazy LexEna.
IF OBJECT_ID(N'dbo.LexEnaBillingDistributionApproval', N'U') IS NULL
    CREATE TABLE dbo.LexEnaBillingDistributionApproval
    (
        BatchId uniqueidentifier NOT NULL,
        Firma int NOT NULL,
        ProposalId uniqueidentifier NOT NULL,
        ApprovedAt datetime NOT NULL,
        ApprovedBy nvarchar(256) NOT NULL,
        ResultXml nvarchar(max) NOT NULL,
        CONSTRAINT PK_LexEnaBillingDistributionApproval PRIMARY KEY (BatchId)
    );

COMMIT TRANSACTION;
GO
