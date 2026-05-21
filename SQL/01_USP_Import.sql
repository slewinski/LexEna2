GO
/****** Object:  StoredProcedure [dbo].[USP_BIGImportCCnB]    Script Date: 2020-02-19 07:34:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery18.sql|7|0|C:\Users\slawo\AppData\Local\Temp\~vs53EE.sql

ALTER PROCEDURE [dbo].[USP_BIGImportCCnB] 

	
	@IdJob int,
	@UserName as varchar(max)
    
AS


BEGIN 
 SET NOCOUNT ON;

Declare @Big_ImportId as int,
		@MaxBig_DebtorId as int,
		@MaxBig_CaseId as Int,
		@MaxBig_ObligationId as Int,
		@DluCount as Int,
		@PackageSize Int = 1500,
		@ImportId as Int,
		@Parts as Int,
		@PartNumber as Int
		
IF OBJECT_ID('tempdb..#current') IS NOT NULL DROP TABLE #current
IF OBJECT_ID('tempdb..#job') IS NOT NULL DROP TABLE #job
IF OBJECT_ID('tempdb..#delete') IS NOT NULL DROP TABLE #delete
IF OBJECT_ID('tempdb..#update') IS NOT NULL DROP TABLE #update

select * 
 into  #current
from BIGvw_ObligationLastStatus

update #current set IdWienaSpr = 0 
select * 
	into  #job
from BIG_JobRow where BIG_JobId = @IdJob 

alter table #job add ObligationId varchar(100) null

update #job set ObligationId = 	ltrim(rtrim(nrklienta)) +'/' + case when len(ltrim(rtrim(nip)))> 0 then ltrim(rtrim(nip)) else ltrim(rtrim(pesel)) end 
								+'/' + ltrim(rtrim(Title)) +'/'
								+  case when substring(datawymag,3,1) = '-' or substring(datawymag,3,1) = '.' then substring(datawymag,7,4) + substring(datawymag,4,2) + substring(datawymag,1,2)
						else substring(datawymag,1,4) + substring(datawymag,6,2) + substring(datawymag,9,2)		end,
				nip = ltrim(rtrim(nip)),
				pesel = ltrim(rtrim(pesel)),
				Lokal = ltrim(rtrim(Lokal)),
				Miejsce = ltrim(rtrim(Miejsce)),
				Firstname = ltrim(rtrim(Firstname)),
				datawymag = case when substring(datawymag,3,1) = '-' or substring(datawymag,3,1) = '.' then substring(datawymag,7,4) +'-' + substring(datawymag,4,2) + '-' +substring(datawymag,1,2)
						else substring(datawymag,1,4) + '-' + substring(datawymag,6,2) + '-' +substring(datawymag,9,2) end,
				datawyswezw = case when substring(datawyswezw,3,1) = '-' or substring(datawyswezw,3,1) = '.' then substring(datawyswezw,7,4) +'-' + substring(datawyswezw,4,2) + '-' +substring(datawyswezw,1,2)
						else substring(datawyswezw,1,4) + '-' + substring(datawyswezw,6,2) + '-' +substring(datawyswezw,9,2) end
				

select * 
into #new 
from  #job j where not exists ( select null from #current c where c.ObligationId Collate Polish_CI_AS = j.ObligationId Collate Polish_CI_AS)
alter table #new add Big_CaseId int null, Big_DebtorId int null, CaseId varchar(100) null
update #new set CaseId = '3'+'/' + NrKlienta + '/' + case when len(nip) >  0 then nip else pesel end 
update #new set Big_CaseId = ( select  top 1 c.BIG_CaseId from #current c where c.CaseId Collate Polish_CI_AS = #new.CaseId Collate Polish_CI_AS)
update #new set Big_DebtorId = ( select  top 1 c.BIG_DebtorId from #current c where c.CaseId Collate Polish_CI_AS = #new.CaseId Collate Polish_CI_AS)


select * 
 into #update 
 from  #current c where  exists ( select null from #job j where c.ObligationId Collate Polish_CI_AS = j.ObligationId Collate Polish_CI_AS and c.Saldo <>  replace(j.Saldo,',','.') )
update #update set IdWienaSpr = 2, TypOperacji = 'Aktualizacja'
-- update kwoty


update #update set Saldo = (select replace(Saldo,',','.')  from #job  where #update.ObligationId Collate Polish_CI_AS = #job.ObligationId Collate Polish_CI_AS)  
where IdWienaSpr = 2 


select * 
 into #delete 
 from  #current c where  not exists ( select null from #job  j where c.ObligationId Collate Polish_CI_AS = j.ObligationId Collate Polish_CI_AS)
 and c.SrcSystem = 3
update #delete set IdWienaSpr = 3 , TypOperacji = 'Usunięcie'



insert into #update
select * from #delete 

insert into #update
([Name],	Firstname,	IDNumber,	DebtorType,	
Address1L1,	Address1L2,	NrKlienta,	[System]	,[SrcSystem],	[DataWymag]	,[Saldo],	[Title],	[DataWysWezw],	[DataTytWyk],	[TypNal],	SystemId,	TypOperacji,	BIG_ObligationId,	BIG_DebtorId,	BIG_OperacjaId,	BIG_CaseId,	CaseId,	
NotifyDate,	ObligationId,	BIG_ImportId,	DataOperacji,	StatusOperacji,	StatOpis,	DataProcedowaniaKrd,	SuspendDate,	AutoSuspend,	DataImportu,	[Row],	IdWienaSpr,	IdwienaNal)
select 
[Name], Firstname,  case when len(Firstname) > 0 and len(pesel) >  0 then pesel else nip end , case when len(Firstname) > 0 and len(pesel) >  0 then  'Osoba fizyczna' else 'Osoba prawna' end,
Ulica + ' ' + Dom + case when len(Lokal)> 0  and  Lokal <> '0' then + '/' + Lokal else '' end ,
Kod + ' ' +  Miejsce, NrKlienta, 'CC&B' as system, 3 as srcsystem, 
 CONVERT(DATETIME, DataWymag, 102) as DataWymag, replace(Saldo,',','.') as Saldo, Title,CONVERT(DATETIME, [DataWysWezw], 102) , null as [DataTytWyk],
 0 as typnal, null as systemId, 'Dodanie' as typoperacji, 0 as BIG_ObligationId, isnull(Big_DebtorId,0) as BIG_DebtorId, 0 as Big_operacjaId, isnull(Big_CaseId,0) as Big_CaseId, '3'+'/' + NrKlienta + '/' + case when len(nip) >  0 then nip else pesel end  as CaseId,
 null as NotifyDate, ObligationId as ObligationId, 0 as Big_ImportId, GetDate() as DataOperacji, 0 as StatusOperacji, '' as StatOpis, GetDate() as DataProcedowaniaKrd, null as suspenddate, 0 As AutoSuspend,GetDate() as Dataimportu, 0 as Row, 1 as IdWienaSpr, 0 as IdWienaNal   
from #new

update u   set Big_CaseId =  isnull((select top 1 Big_CaseId from #update uu where u.CaseId = uu.CaseId and uu.BIG_CaseId > 0 order by uu.BIG_CaseId desc ),0)
from  #update u 
where u.BIG_CaseId = 0

update u   set Big_DebtorId =  isnull((select top 1 Big_DebtorId from #update uu where u.CaseId = uu.CaseId and uu.BIG_debtorid > 0 order by uu.BIG_DebtorId desc ),0)
from  #update u 
where u.BIG_DebtorId = 0


-- zakładanie bigImport


insert into BIG_Import 
			([DataImportu]
           ,[Username]
           ,[Status]
           ,[StatOpis]
           ,[Filename]
           ,TypImp)
select 
            GETDATE()
           ,@UserName
           ,0
           ,'W trakcie importu - nie wysyłać !!!'
           ,'Import CC&B ' + CONVERT(Varchar(20), GetDate(), 120)
           ,1 
		   
select @Big_ImportId = MAX(Big_ImportId) from BIG_Import
-- inserty 
select @MaxBig_CaseId  = MAX(BIG_CaseId) from BIG_Case

INSERT INTO [dbo].[BIG_Case]
           ([CaseId]
           ,[CaseBIGId]
           ,[SrcSystem]
           ,[IdWienaSpr]
           ,[BIG_ImportId]
           )
select  distinct
			CaseId as CaseId
           ,'New' as CaseBIGId 
           ,3 as  SrcSystem
           ,0 as IdWienaSpr
           ,@Big_ImportId as BIG_ImportId 
from #update where isnull(BIG_CaseId,0) = 0 order by CaseId	         

update #update set BIG_CaseId = (select BIG_CaseId from BIG_Case where BIG_CaseId > @MaxBig_CaseId and BIG_Case.CaseId = #update.CaseId ) where Big_CaseId = 0

select @MaxBig_DebtorId  = MAX(Big_DebtorId) from BIG_Debtor


INSERT INTO [dbo].[BIG_Debtor]
           ([DebtorType]
           ,[Name]
           ,[Firstname]
           ,[Secondname]
           ,[IdentityNumberType]
           ,[IDNumber]
           ,[Address1L1]
           ,[Address1L2]
           ,[BIG_CaseId]
           ,[NrKlienta]
           )

  select distinct
            case when DebtorType = 'Osoba prawna' then 1 else 0 end
           ,[Name]
           ,substring(Firstname,1,32)
           ,''
           ,case when DebtorType = 'Osoba prawna' then 1 else 2 end
           ,IDNumber
           ,Address1L1
           ,Address1L2
           ,BIG_CaseId
           ,NrKlienta
from #update where  isnull(BIG_DebtorId,0) = 0 order by BIG_CaseId



update #update set BIG_DebtorId = (select BIG_DebtorId from BIG_Debtor where BIG_DebtorId > @MaxBig_DebtorId and BIG_Debtor.IdNumber = #update.IDNumber and BIG_Debtor.NrKlienta = #update.NrKlienta ) where BIG_DebtorId = 0

select @MaxBig_ObligationId  = MAX(Big_ObligationId) from BIG_Obligation

INSERT INTO [dbo].[BIG_Obligation]
           ([ObligationId]
           ,[Title]
           ,[DataWysWezw]
           ,[DataWymag]
           ,[Saldo]
           ,[BIG_CaseId]
           ,[IdWienaNal]
           ,[TypNal]
           )
select  distinct
           ObligationId
           ,Title
           ,DataWysWezw
           ,DataWymag
           , Saldo
           ,BIG_CaseId
           ,0
           ,0
from #update where IdWienaSpr in (1,3)   order by BIG_CaseId,  ObligationId   

-- 
INSERT INTO [dbo].[BIG_Obligation]
           ([ObligationId]
           ,[Title]
           ,[DataWysWezw]
           ,[DataWymag]
           ,[Saldo]
           ,[BIG_CaseId]
           ,[IdWienaNal]
           ,[TypNal]
           )
select  distinct
           u.ObligationId
           ,u.Title
           ,u.DataWysWezw
           ,u.DataWymag
           , u.Saldo
           ,u.BIG_CaseId
           ,0
           ,0
from #update u   where u.IdWienaSpr = 2   order by u.BIG_CaseId,  u.ObligationId   

update #update set BIG_ObligationId = (select BIG_ObligationId from BIG_Obligation where BIG_ObligationId > @MaxBig_ObligationId and ObligationId = #update.ObligationId  ) 


INSERT INTO [dbo].[BIG_Operacja]
           ([DataOperacji]
           ,[StatusOperacji]
           ,[TypOperacji]
           ,[BIG_ImportId]
           ,[BIG_ObligationId]
           ,[OrderOp]
           ,[BIG_CaseId]
           ,[BIG_DebtorId])
select distinct
			GETDATE()
			,0
			,IdWienaSpr
			,@Big_ImportId
			,BIG_ObligationId
			,0
			,BIG_CaseId
			,BIG_DebtorId
from #update 



-- usunięcie tych, które się znie zmieniły
-- oznaczenie dodawania spraw.
update  bo  set TypOperacji = TypOperacji + 100  from big_operacja bo inner join Big_case bc on bo.BIG_CaseId = bc.BIG_CaseId
where TypOperacji = 1 and bo.Big_CaseId > @MaxBig_CaseId
-- oznaczenie usuwania spraw


update  bod set  TypOperacji = TypOperacji + 100  from big_operacja bod where bod.BIG_CaseId in 
(select c.Big_CaseId from  
(select  count(*) as ile, Big_CaseId from #current group by Big_CaseId) c
inner join 
(select  count(*) as ile , Big_CaseId from BIG_Operacja    where  Big_ImportId =  @Big_ImportId and TypOperacji = 3  group by Big_CaseId) bo
on bo.BIG_CaseId = c.BIG_CaseId and bo.ile = c.ile 
)
and bod.BIG_ImportId =  @Big_ImportId
and bod.TypOperacji = 3 
 
and not exists (select null from big_operacja  box where box.BIG_CaseId = bod.Big_caseId and box.TypOperacji <> 3 and box.BIG_ImportId = @Big_ImportId)

-- usunięcie wpisów, które nie spelniają warunków  200 pln fizyczna, 300 prawna
select * into #todel from
(select ob.Big_caseId as Big_caseId, SUM(bob.Saldo) as saldo , MAX(bd.DebtorType) as DebtorType  from BIG_Operacja ob inner join BIG_Obligation bob on ob.BIG_ObligationId = bob.BIG_ObligationId inner join BIG_Debtor bd on bd.BIG_CaseId = ob.BIG_CaseId 
	where ob.TypOperacji = 101 and ob.BIG_ImportId = @Big_ImportId group by ob.Big_caseId ) a

delete from BIG_Operacja  where BIG_CaseId in ( select big_caseid from #todel where  (Saldo < 200  and  DebtorType = 0 ) or (Saldo < 300  and  DebtorType = 1) )  

update BIG_Import set lPoz =  isnull(( select COUNT(*) from  Big_Operacja where Big_Operacja.Big_ImportId = Big_Import.Big_ImportId ),0) where BIG_ImportId = @Big_ImportId

IF OBJECT_ID('tempdb..#current') IS NOT NULL DROP TABLE #current
IF OBJECT_ID('tempdb..#job') IS NOT NULL DROP TABLE #job
IF OBJECT_ID('tempdb..#delete') IS NOT NULL DROP TABLE #delete
IF OBJECT_ID('tempdb..#update') IS NOT NULL DROP TABLE #update
IF OBJECT_ID('tempdb..#todel') IS NOT NULL DROP TABLE #todel

-- podział za duzego pakietu
select  @DluCount = Count(distinct Big_caseId) from BIG_Operacja where BIG_ImportId =  @Big_ImportId
set @Parts = @DluCount/@PackageSize + 1
set @PartNumber = 1
while @DluCount > @PackageSize
Begin
	INSERT INTO [dbo].[BIG_Import]
           ([DataImportu]
           ,[Username]
           ,[Status]
           ,[StatOpis]
           ,[DataWysylki]
           ,[ImportFile]
           ,[lPoz]
           ,[Filename]
           ,[JobId]
           ,[ChunkId]
           ,[DataStatusu]
           ,[lBlad]
           ,[lSuccess]
           ,[TypImp])
     Select [DataImportu]
           ,[Username]
           ,[Status]
           ,[StatOpis]
           ,[DataWysylki]
           ,[ImportFile]
           ,[lPoz]
           ,[Filename]
           ,[JobId]
           ,[ChunkId]
           ,[DataStatusu]
           ,[lBlad]
           ,[lSuccess]
           ,[TypImp]  from BIG_Import where BIG_ImportId = @Big_ImportId

	SELECT @ImportId = SCOPE_IDENTITY()

	update big_operacja set big_importId = @ImportId where 
	big_importId = @Big_ImportId and BIG_CaseId in (select distinct top (@PackageSize)  Big_CaseId from BIG_Operacja where BIG_ImportId = @Big_ImportId)
	set @PartNumber = @PartNumber  + 1  
 	update Big_Import set StatOpis = 'Nowy' + ' cz. ' +  cast (@PartNumber as varchar(2)) +'/' + cast (@Parts as varchar(2)), Filename = Filename + ' cz. ' +  cast (@PartNumber as varchar(2)) +'/' + cast (@Parts as varchar(2)) , lPoz = (select count(*) from big_operacja where BIG_ImportId =@ImportId )
	where BIG_ImportId = @ImportId
	select  @DluCount = Count(distinct Big_caseId) from BIG_Operacja where BIG_ImportId =  @Big_ImportId

End
update Big_Import set StatOpis = 'Nowy' + ' cz. 1/' + cast (@Parts as varchar(2)) , Filename = Filename + ' cz. 1/' + cast (@Parts as varchar(2)), lPoz = (select count(*) from big_operacja where BIG_ImportId =@Big_ImportId )
where BIG_ImportId = @Big_ImportId

select @Big_ImportId
END
