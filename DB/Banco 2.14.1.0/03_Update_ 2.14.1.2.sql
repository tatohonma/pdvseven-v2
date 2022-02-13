USE PDV7
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT

BEGIN TRAN A
BEGIN TRY

INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'intervaloSyncMyFinance', '3600', NULL, 0, 'Intervalo que o serviço de integrações faz a sincronização com a API MyFinance (em segundos)')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'dtUltimaSyncMyFinance', NULL, NULL, 0, 'Data da ultima sincornização do MyFinance')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'dtUltimaFechamentoPedidoMyFinance', NULL, NULL, 0, 'Data de fechamento do ultimo pedido enviado ao MyFinance')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'ultimoIDPedidoMyFinance', NULL, NULL, 0, 'ID do ultimo pedido enviado ao MyFinance')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'ultimoIDPagamentoMyFinance', NULL, NULL, 0, 'ID do último pagamento enviado ao MyFinance')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'EnviadoMyFinance', '0', '0|1', 0, ' Flag que indica se o ultimo pagamento foi enviado com sucesso ao MyFinance')

INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'HabilitaMyFinance', '0', '0|1', 0, ' Flag que habilita integração MyFinance')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'HabilitaWorkinHub', '0', '0|1', 0, ' Flag que habilita integração WorkinHub')

INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'intervaloSyncWorkingHub', '60', NULL, 0, 'Intervalo que o serviço de integrações faz a sincronização com a API WokingHub (em segundos)')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'dtUltimoPedidoWorkingHub', NULL, NULL, 0, 'Data do ultimo pedido enviado ao WorkinHub')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'IdentificadorClienteWorkinHub', NULL, NULL,0, 'Codigo de identificação do estabelecimento gerado pelo WorkingHub')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'dtUltimoProdutoWorkingHub', NULL, NULL, 0, 'Data do ultimo produto enviado ao WorkinHub')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'PedidoEnviadoWorkinHub', '0', '0|1', 0, ' Flag que indica se o ultimo pedido foi enviado com sucesso ao WorkinHub')
INSERT INTO tbConfiguracaoBD(IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo) VALUES(NULL, NULL, 'ProdutoEnviadoWorkinHub', '0', '0|1', 0, ' Flag que indica se o ultimo produto foi enviado com sucesso ao WorkinHub')

COMMIT TRAN A

END TRY
BEGIN CATCH 
  rollback TRAN A
  SELECT ERROR_MESSAGE() as 'ERROR_MESSAGE', ERROR_SEVERITY() as 'ERROR_SEVERITY', ERROR_STATE() as 'ERROR_STATE', ERROR_LINE() as 'ERROR_LINE', ERROR_PROCEDURE() as 'ERROR_PROCEDURE'
END CATCH