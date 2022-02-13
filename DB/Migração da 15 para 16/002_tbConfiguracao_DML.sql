USE PDV7
go

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT

begin tran a
BEGIN TRY
       		
	
insert into tbConfiguracaoBD(Chave,Valor,ValoresAceitos,Titulo)
	values('ImprimirViaMotoboy','1','0|1','Imprimir Via da conta para Motoboy no Delivery')

	commit tran a		

	select * from tbConfiguracaoBD

END TRY
BEGIN CATCH	
	rollback tran a
	SELECT ERROR_MESSAGE() as 'ERROR_MESSAGE', ERROR_SEVERITY() as 'ERROR_SEVERITY', ERROR_STATE() as 'ERROR_STATE'
END CATCH
