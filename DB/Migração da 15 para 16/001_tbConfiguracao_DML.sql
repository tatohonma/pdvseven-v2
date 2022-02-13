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
       		
	
insert into tbConfiguracaoBD(IDTipoPDV,IDPDV,Chave,Valor,ValoresAceitos,Obrigatorio,Titulo) 
	values(	10,	NULL, 'CaixaTouch',	'1' , '0|1', '1','Usar seleção touch no caixa');


insert into tbConfiguracaoBD(Chave,Valor,ValoresAceitos,Titulo)
	values('ServicoComoItem','1','0|1','Ativar serviço como um item do pedido')


insert into tbConfiguracaoBD(Chave,Valor,ValoresAceitos,Titulo)
	values('DadosClienteCompletoOrdem','1','0|1','Imprimir dados completos na ordem de produção do delivery')


INSERT [dbo].[tbConfiguracaoBD] ([IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES (NULL, NULL, N'intervaloSyncMyFinance', N'3600', NULL, 0, N'Intervalo que o serviço de integrações faz a sincronização com a API MyFinance (em segundos)')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'dtUltimaSyncMyFinance', NULL, NULL, 0, N'Data da ultima sincornização do MyFinance')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'dtUltimaFechamentoPedidoMyFinance', NULL, NULL, 0, N'Data de fechamento do ultimo pedido enviado ao MyFinance')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'ultimoIDPedidoMyFinance', NULL, NULL, 0, N'ID do ultimo pedido enviado ao MyFinance')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'ultimoIDPagamentoMyFinance', NULL, NULL, 0, N'ID do último pagamento enviado ao MyFinance')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES (NULL, NULL, N'EnviadoMyFinance', N'0', N'0|1', 0, N' Flag que indica se o ultimo pagamento foi enviado com sucesso ao MyFinance')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'HabilitaMyFinance', N'0', N'0|1', 0, N' Flag que habilita integração MyFinance')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'HabilitaWorkinHub', N'0', N'0|1', 0, N' Flag que habilita integração WorkinHub')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'intervaloSyncWorkingHub', N'60', NULL, 0, N'Intervalo que o serviço de integrações faz a sincronização com a API WokingHub (em segundos)')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'dtUltimoPedidoWorkingHub', NULL, NULL, 0, N'Data do ultimo pedido enviado ao WorkinHub')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'IdentificadorClienteWorkinHub', NULL, NULL, 0, N'Codigo de identificação do estabelecimento gerado pelo WorkingHub')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'dtUltimoProdutoWorkingHub', NULL, NULL, 0, N'Data do ultimo produto enviado ao WorkinHub')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'PedidoEnviadoWorkinHub', N'0', N'0|1', 0, N' Flag que indica se o ultimo pedido foi enviado com sucesso ao WorkinHub')
INSERT [dbo].[tbConfiguracaoBD] ( [IDTipoPDV], [IDPDV], [Chave], [Valor], [ValoresAceitos], [Obrigatorio], [Titulo]) VALUES ( NULL, NULL, N'ProdutoEnviadoWorkinHub', N'0', N'0|1', 0, N' Flag que indica se o ultimo produto foi enviado com sucesso ao WorkinHub')


	commit tran a		

	select * from tbConfiguracaoBD

END TRY
BEGIN CATCH	
	rollback tran a
	SELECT ERROR_MESSAGE() as 'ERROR_MESSAGE', ERROR_SEVERITY() as 'ERROR_SEVERITY', ERROR_STATE() as 'ERROR_STATE'
END CATCH
