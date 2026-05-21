USE [LexEnaPro]
GO
/****** Object:  Table [dbo].[ZgonyDetails]    Script Date: 2020-02-16 19:55:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZgonyDetails](
	[ZgonyDetails_Id] [int] IDENTITY(1,1) NOT NULL,
	[Sygnatura] [varchar](30) NULL,
	[SaldoKSiegowe] [decimal](18, 2) NULL,
	[NalGlownaKsiegowa] [decimal](18, 2) NULL,
	[NalGlownaKsiegowaPrzedaw] [decimal](18, 2) NULL,
	[OdsetkiKapitalKsiegowaPrzedaw] [decimal](18, 2) NULL,
	[OdsetkiNaliczoneKsiegowaPrzedaw] [decimal](18, 2) NULL,
	[WDZKsiegowaPrzedaw] [decimal](18, 2) NULL,
	[DataPrzedawnieniaNalGl] [datetime] NULL,
	[KosztyKsiegowaPrzedaw] [decimal](18, 2) NULL,
	[DataPrzedawnieniaKosztow] [datetime] NULL,
	[OdsetkiZaksKsiegowaPrzedaw] [decimal](18, 2) NULL,
	[DataPrzedawnieniaOdsetek] [decimal](18, 2) NULL,
	[NalGlownaKsiegowaNiePrzedaw] [decimal](18, 2) NULL,
	[OdsetkiKapitalKsiegowaNiePrzedaw] [decimal](18, 2) NULL,
	[OdsetkiNaliczoneKsiegowaNiePrzedaw] [decimal](18, 2) NULL,
	[WDZKsiegowaNiePrzedaw] [decimal](18, 2) NULL,
	[DataPlatnosciNalGl] [datetime] NULL,
	[KosztyKsiegowaNiePrzedaw] [decimal](18, 2) NULL,
	[DataPlatnosciKosztow] [datetime] NULL,
	[OdsetkiZaksKsiegowaNiePrzedaw] [decimal](18, 2) NULL,
	[DataPlatnosciOdsetek] [decimal](18, 2) NULL,
	[WindykKosztyEgz] [decimal](18, 2) NULL,
	[WindykOdsetki] [decimal](18, 2) NULL,
	[KontoKsiegowe] [varchar](20) NULL,
	[Status] [int] NULL,
	[StatusText] [varchar](200) NULL,
	[ZgonyHeader_ID] [int] NULL,
 CONSTRAINT [PK_ZgonyDetails] PRIMARY KEY CLUSTERED 
(
	[ZgonyDetails_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ZgonyHeader]    Script Date: 2020-02-16 19:55:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZgonyHeader](
	[ZgonyHeader_Id] [int] IDENTITY(1,1) NOT NULL,
	[DataImportu] [datetime] NULL,
	[DataZestawienia] [datetime] NULL,
	[GuidZestawienia] [uniqueidentifier] NULL,
	[StatusZestawienia] [int] NULL,
	[UserName] [varchar](200) NULL,
	[StatusText] [varchar](200) NULL,
 CONSTRAINT [PK_ZgonyHeader] PRIMARY KEY CLUSTERED 
(
	[ZgonyHeader_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ZgonyDetails]  WITH CHECK ADD  CONSTRAINT [FK_ZgonyDetails_ZgonyDetails] FOREIGN KEY([ZgonyDetails_Id])
REFERENCES [dbo].[ZgonyDetails] ([ZgonyDetails_Id])
GO
ALTER TABLE [dbo].[ZgonyDetails] CHECK CONSTRAINT [FK_ZgonyDetails_ZgonyDetails]
GO
