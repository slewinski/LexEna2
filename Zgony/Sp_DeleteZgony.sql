GO
/****** Object:  StoredProcedure [dbo].[USP_DelKRDPakiet]    Script Date: 16.02.2020 23:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery18.sql|7|0|C:\Users\slawo\AppData\Local\Temp\~vs53EE.sql

CREATE PROCEDURE [dbo].[USP_ZgonyDelete] 

	@IdPakiet int
    
AS
BEGIN
BEGIN TRAN

BEGIN TRY

  delete from ZgonyDetails where ZgonyHeader_ID = @IdPakiet
			
  delete from ZgonyHeader where ZgonyHeader_ID = @IdPakiet

   COMMIT TRAN

END TRY
BEGIN CATCH

  ROLLBACK TRAN

END CATCH

END
