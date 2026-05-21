INSERT INTO [dbo].[BIG_JobRow]
           ([BIG_JobId]
           ,[FName]
           ,[dStart]
           ,[SystemName]
           ,[NrKlienta]
           ,[Name]
           ,[Firstname]
           ,[Secondname]
           ,[Pesel]
           ,[Nip]
           ,[Kod]
           ,[Miejsce]
           ,[Ulica]
           ,[Lokal]
           ,[Dom]
           ,[DataWysWezw]
           ,[DataWymag]
           ,[Saldo]
           ,[Title]
           ,[Adr1]
           ,[Adr2]
           ,[TypNal]
           ,[IdNal]
           ,[IdSprawaWiena]
           ,[WienaSygn]
           ,[SygnOrzecz]
           ,[dorzecz]
           ,[czywyrok]
           ,[Message]
           ,[Status]
           ,[PartNo]
           ,[DataFaktury]
           ,[DataSprzedazy])
     select 
            2181
           ,'Dodanie ręczne'
           ,GetDate()
           ,'Wiena'
           ,s.nr_ewid
           ,sk.nazwisko
           ,sk.imie
           ,''
           ,sk.pesel
           ,sk.nip
           ,''
           ,''
           ,''
           ,''
           ,''
           ,'2024-01-18'
           ,convert(varchar(10),n.data_n,120)
           ,n.kwota
           ,n.tytul
           ,''
           ,''
           ,''
           ,0
           ,0
           ,s.sygnatura
           ,''
           ,''
           ,''
           ,''
           ,0
           ,0
           ,convert(varchar(10),n.data_fv,120)
           ,''

		   from wiena_eob_prod.dbo.sprawa  s inner join wiena_eob_prod.dbo.naleznosc n on n.id_sprawy = s.ident 
		   inner join wiena_eob_prod.dbo.spr_dluz sd on sd.id_sprawy = s.ident inner join wiena_eob_prod.dbo.skazani sk on sd.id_dluz = sk.id
		   where n.id_typ_nal = 2 and n.tytul in ('501021947/3/W/2023',
'501021947/4/W/2023',
'501021947/5/W/2023',
'501021947/6/W/2023',
'501021947/7/W/2023')
		   
		
GO