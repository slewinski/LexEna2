GO
/****** Object:  View [dbo].[BIGvw_Dluznicy]    Script Date: 14.02.2020 01:45:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Alter VIEW [dbo].[BIGvw_Dluznicy]
AS

select   
d.Name,
d.Firstname,
d.IDNumber,
case d.DebtorType when 0 then 'Osoba fizyczna' when 1 then 'Osoba prawna' else 'Działalność Gosp' end as DebtorType,
d.Address1L1,
d.Address1L2,
d.NrKlienta,
case c.SrcSystem when 1 then 'AUMS' when 2 then 'SELEN' when 3 then 'CC&B' when 11 then 'WIENA' end as System,
d.BIG_DebtorId,
c.BIG_CaseId,
ROW_NUMBER() OVER(PARTITION BY c.BIG_CaseId ORDER BY c.BIG_CaseId) AS Row
from BIG_Debtor d 
inner join BIG_Case c on d.BIG_CaseId =  c.BIG_CaseId
GO
/****** Object:  View [dbo].[BIGvw_DluznicyAktual]    Script Date: 14.02.2020 01:45:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Alter VIEW [dbo].[BIGvw_DluznicyAktual]
AS

select  distinct  
d.Name,
d.Firstname,
d.IDNumber,
d.Pesel,
case d.DebtorType when 0 then 'Osoba fizyczna' when 1 then 'Osoba prawna' else 'Działalność Gosp' end as DebtorType,
d.Address1L1,
d.Address1L2,
d.NrKlienta,
d.BIG_DebtorId,
case c.SrcSystem when 1 then 'AUMS' when 2 then 'SELEN' when 3 then 'CC&B' when 11 then 'WIENA' end as System,
c.CaseId,
c.SrcSystem,
c.BIG_CaseId,
c.NotifyDate,
ROW_NUMBER() OVER(PARTITION BY c.BIG_CaseId ORDER BY c.BIG_CaseId) AS Row
from BIG_Case c
inner join 
BIG_Debtor d
on  d.BIG_CaseId =  c.BIG_CaseId
where 
 
exists 
 (select null from  
 (select max(binf.BIG_OperacjaId) idOpr, max(bobli.BIG_CaseId) as bCaseId, max (binf.BIG_DebtorId ) as bDebtorId
from BIG_Operacja binf inner join BIG_Obligation bobli on bobli.BIG_ObligationId = binf.BIG_ObligationId    where  binf.TypOperacji%100 in (1,2) and 
not exists(select null  from  BIG_Operacja bop inner join BIG_Obligation bigobl on bigobl.BIG_ObligationId =  bop.BIG_ObligationId
 where bigobl.BIG_CaseId = bobli.BIG_CaseId and bigobl.ObligationId = bobli.ObligationId and bop.TypOperacji%100 = 3 and bop.BIG_OperacjaId >binf.BIG_OperacjaId  ) 
  group by  bobli.ObligationId
 ) a 
 where  bCaseId = c.BIG_CaseId and d.BIG_DebtorId = bDebtorId  )
GO
/****** Object:  View [dbo].[BIGvw_ImportWiena2BIG]    Script Date: 14.02.2020 01:45:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  View [dbo].[BIGvw_ImportyDetail]    Script Date: 14.02.2020 01:45:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Alter VIEW [dbo].[BIGvw_ImportyDetail]
AS

select   
d.Name,
d.Firstname,
d.IDNumber,
case d.DebtorType when 0 then 'Osoba fizyczna' when 1 then 'Osoba prawna' else 'Działalność Gosp' end as DebtorType,
d.Address1L1,
d.Address1L2,
d.NrKlienta,
case c.SrcSystem when 1 then 'AUMS' when 2 then 'SELEN' when 3 then 'CC&B' when 11 then 'WIENA' end as System,
o.DataWymag,
o.Saldo,
o.Title,
case bopr.TypOperacji%100  when 1 then 'Dodanie' when 2 then 'Aktualizacja' when 3 then 'Usunięcie'  when 5 then 'Zawieszenie' when 6 then 'Podjęcie' when 17 then 'Wys. powiadomienia' end as TypOperacji,
o.BIG_ObligationId,
o.DataWysWezw,
o.DataTytWyk,
o.TypNal,
o.SystemId,
o.IdWienaNal,
d.BIG_DebtorId,
bopr.BIG_OperacjaId,
c.BIG_CaseId,
bopr.BIG_ImportId ,
bopr.DataOperacji,
bopr.StatusOperacji,
bopr.StatOpis,
bopr.DataProcedowaniaKrd,
bopr.SuspendDate,
bimp.DataImportu,
ROW_NUMBER() OVER(PARTITION BY bopr.BIG_ImportId ORDER BY BIG_OperacjaId) AS Row
from BIG_Debtor d 
inner join BIG_Case c on d.BIG_CaseId =  c.BIG_CaseId
inner join BIG_Obligation o on c.BIG_CaseId = o.BIG_CaseId
inner join BIG_Operacja bopr  on o.BIG_ObligationId = bopr.BIG_ObligationId
inner join BIG_Import bimp on bimp.BIG_ImportId = bopr.BIG_ImportId
where bopr.BIG_DebtorId = d.BIG_DebtorId
GO
/****** Object:  View [dbo].[BIGvw_ObligationLastStatus]    Script Date: 14.02.2020 01:45:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Alter View [dbo].[BIGvw_ObligationLastStatus]
AS

select   
d.Name,
d.Firstname,
d.IDNumber,
case d.DebtorType when 0 then 'Osoba fizyczna' when 1 then 'Osoba prawna' else 'Działalność Gosp' end as DebtorType,
d.Address1L1,
d.Address1L2,
d.NrKlienta,
case c.SrcSystem when 1 then 'AUMS' when 2 then 'SELEN' when 3 then 'CC&B' when 11 then 'WIENA' end as System,
c.SrcSystem,
o.DataWymag,
o.Saldo,
o.Title,
o.DataWysWezw,
o.DataTytWyk,
o.TypNal,
o.SystemId,
case bopr.TypOperacji%100 when 1 then 'Dodanie' when 2 then 'Aktualizacja' end as TypOperacji,
o.BIG_ObligationId,
d.BIG_DebtorId,
bopr.BIG_OperacjaId,
c.BIG_CaseId,
c.CaseId,
c.NotifyDate,
o.ObligationId,
bopr.BIG_ImportId ,
bopr.DataOperacji,
bopr.StatusOperacji,
bopr.StatOpis,
bopr.DataProcedowaniaKrd,
o.SuspendDate,
isnull(o.AutoSuspend,0) as AutoSuspend,
bimp.DataImportu,
ROW_NUMBER() OVER(PARTITION BY bopr.BIG_ImportId ORDER BY BIG_OperacjaId) AS Row,
c.IdWienaSpr,
o.IdwienaNal
from 

BIG_Debtor d 
inner join BIG_Case c on d.BIG_CaseId =  c.BIG_CaseId
inner join BIG_Obligation o on c.BIG_CaseId = o.BIG_CaseId
inner join BIG_Operacja bopr  on o.BIG_ObligationId = bopr.BIG_ObligationId
inner join BIG_Import bimp on bimp.BIG_ImportId = bopr.BIG_ImportId 
inner join (select max(binf.BIG_OperacjaId) idOpr, max(bobli.BIG_CaseId) as bCaseId
from BIG_Operacja binf inner join BIG_Obligation bobli on bobli.BIG_ObligationId = binf.BIG_ObligationId    where  binf.TypOperacji%100 in (1,2) and 
not exists(select null  from  BIG_Operacja bop inner join BIG_Obligation bigobl on bigobl.BIG_ObligationId =  bop.BIG_ObligationId
 where bigobl.BIG_CaseId = bobli.BIG_CaseId and bigobl.ObligationId = bobli.ObligationId and bop.TypOperacji%100 = 3 and bop.BIG_OperacjaId >binf.BIG_OperacjaId  ) 
  group by  bobli.ObligationId
 ) a 
 on a.idOpr = bopr.BIG_OperacjaId and bCaseId = c.BIG_CaseId
 where bopr.BIG_DebtorId = d.BIG_DebtorId
GO





SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[BIGvw_ImportyDetail]
AS

select   
d.Name,
d.Firstname,
d.IDNumber,
case d.DebtorType when 0 then 'Osoba fizyczna' when 1 then 'Osoba prawna' else 'Działalność Gosp' end as DebtorType,
d.Address1L1,
d.Address1L2,
d.NrKlienta,
case c.SrcSystem when 1 then 'AUMS' when 2 then 'SELEN' when 3 then 'CC&B' when 11 then 'WIENA' end as System,
o.DataWymag,
o.Saldo,
o.Title,
case bopr.TypOperacji  when 1 then 'Dodanie należności' when 2 then 'Aktualizacja' when 3 then 'Usunięcie należności'  when 5 then 'Zawieszenie' when 6 then 'Podjęcie' when 17 then 'Wys. powiadomienia' when 101 then 'Dodanie dłużnika' when 102 then 'Aktualizacja' when 103 then 'Wykreślenie dłużnika' end as TypOperacji,
o.BIG_ObligationId,
o.DataWysWezw,
o.DataTytWyk,
o.TypNal,
o.SystemId,
o.IdWienaNal,
d.BIG_DebtorId,
bopr.BIG_OperacjaId,
c.BIG_CaseId,
bopr.BIG_ImportId ,
bopr.DataOperacji,
bopr.StatusOperacji,
bopr.StatOpis,
bopr.DataProcedowaniaKrd,
bopr.SuspendDate,
bimp.DataImportu,
ROW_NUMBER() OVER(PARTITION BY bopr.BIG_ImportId ORDER BY BIG_OperacjaId) AS Row
from BIG_Debtor d 
inner join BIG_Case c on d.BIG_CaseId =  c.BIG_CaseId
inner join BIG_Obligation o on c.BIG_CaseId = o.BIG_CaseId
inner join BIG_Operacja bopr  on o.BIG_ObligationId = bopr.BIG_ObligationId
inner join BIG_Import bimp on bimp.BIG_ImportId = bopr.BIG_ImportId
where bopr.BIG_DebtorId = d.BIG_DebtorId
GO



