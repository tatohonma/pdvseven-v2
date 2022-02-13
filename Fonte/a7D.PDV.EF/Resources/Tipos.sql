BEGIN TRANSACTION T1

INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (10, N'Não Iniciado')
INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (20, N'Processando')
INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (30, N'Sucesso')
INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (40, N'Abortado')
INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (50, N'Erro')

INSERT [dbo].[tbTipoRelatorio] ([IDTipoRelatorio], [Nome]) VALUES (1, N'Fechamento')
INSERT [dbo].[tbTipoRelatorio] ([IDTipoRelatorio], [Nome]) VALUES (2, N'Geral')

INSERT [dbo].[tbTipoSolicitacaoSAT] ([IDTipoSolicitacaoSAT], [Nome]) VALUES (1, N'EnviarDadosVenda')
INSERT [dbo].[tbTipoSolicitacaoSAT] ([IDTipoSolicitacaoSAT], [Nome]) VALUES (2, N'CancelarUltimaVenda')

INSERT [dbo].[tbCaixaTipoAjuste] ([IDCaixaTipoAjuste], [Nome]) VALUES (20, N'Suprimento')
INSERT [dbo].[tbCaixaTipoAjuste] ([IDCaixaTipoAjuste], [Nome]) VALUES (30, N'Sangria')

SET IDENTITY_INSERT [dbo].[tbTipoTributacao] ON 
INSERT [dbo].[tbTipoTributacao] ([IDTipoTributacao], [Nome], [Descricao], [CFOP], [ICMS00_Orig], [ICMS00_CST], [ICMS00_pICMS], [ICMS40_Orig], [ICMS40_CST], [ICMSSN102_Orig], [ICMSSN102_CSOSN], [ICMSSN900_Orig], [ICMSSN900_CSOSN], [ICMSSN900_pICMS], [PISAliq_CST], [PISAliq_pPIS], [PISQtde_CST], [PISQtde_vAliqProd], [PISNT_CST], [PISSN_CST], [PISOutr_CST], [PISOutr_pPIS], [PISOutr_vAliqProd], [PISST_pPIS], [PISST_vAliqProd], [COFINSAliq_CST], [COFINSAliq_pCOFINS], [COFINSQtde_CST], [COFINSQtde_vAliqProd], [COFINSNT_CST], [COFINSSN_CST], [COFINSOutr_CST], [COFINSOutr_pCOFINS], [COFINSOutr_vAliqProd], [COFINSST_pCOFINS], [COFINSST_vAliqProd], [ISSQN_vDeducISSQN], [ISSQN_vAliq], [ISSQN_cListServ], [ISSQN_cServTribMun], [ISSQN_cNatOp], [ISSQN_indIncFisc], [vItem12741]) VALUES (1, N'Simples   ', NULL, N'5102', NULL, NULL, NULL, NULL, NULL, N'0', N'500', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'49', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'49', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbTipoTributacao] ([IDTipoTributacao], [Nome], [Descricao], [CFOP], [ICMS00_Orig], [ICMS00_CST], [ICMS00_pICMS], [ICMS40_Orig], [ICMS40_CST], [ICMSSN102_Orig], [ICMSSN102_CSOSN], [ICMSSN900_Orig], [ICMSSN900_CSOSN], [ICMSSN900_pICMS], [PISAliq_CST], [PISAliq_pPIS], [PISQtde_CST], [PISQtde_vAliqProd], [PISNT_CST], [PISSN_CST], [PISOutr_CST], [PISOutr_pPIS], [PISOutr_vAliqProd], [PISST_pPIS], [PISST_vAliqProd], [COFINSAliq_CST], [COFINSAliq_pCOFINS], [COFINSQtde_CST], [COFINSQtde_vAliqProd], [COFINSNT_CST], [COFINSSN_CST], [COFINSOutr_CST], [COFINSOutr_pCOFINS], [COFINSOutr_vAliqProd], [COFINSST_pCOFINS], [COFINSST_vAliqProd], [ISSQN_vDeducISSQN], [ISSQN_vAliq], [ISSQN_cListServ], [ISSQN_cServTribMun], [ISSQN_cNatOp], [ISSQN_indIncFisc], [vItem12741]) VALUES (2, N'Simples ST', NULL, N'5405', NULL, NULL, NULL, NULL, NULL, N'0', N'102', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'49', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'49', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tbTipoTributacao] OFF

SET IDENTITY_INSERT [dbo].[tbUnidade] ON 
INSERT [dbo].[tbUnidade] ([IDUnidade], [Nome], [Simbolo], [Excluido]) VALUES (1, N'Unidade', N'Un', 0)
SET IDENTITY_INSERT [dbo].[tbUnidade] OFF

SET IDENTITY_INSERT [dbo].[tbClassificacaoFiscal] ON 
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (2, 1, N'Serviço   ', NULL, N'99', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (3, 2, N'Bala / Chiclete / Bombom', NULL, N'1704.10.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (4, 1, N'Prato / porção', NULL, N'2106.90.90', NULL, NULL, NULL, NULL, NULL, 1.25, 1.75)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (5, 1, N'Suco natural / vitamina / cocktel sem alcool', NULL, N'2106.90.90', NULL, NULL, NULL, NULL, NULL, 1.25, 1.75)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (6, 2, N'Refrigerante', NULL, N'2202.00.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (7, 2, N'Água / água com gás / água saborizada', NULL, N'2202.10.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (8, 2, N'Energético / Café', NULL, N'2202.90.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (9, 2, N'Cerveja / Chopp / Espumante', NULL, N'2203.00.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (10, 2, N'Vinho / Saque', NULL, N'2206.00.90', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (11, 2, N'Whisky', NULL, N'2208.30.10', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (12, 2, N'Vodka', NULL, N'2208.66.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (13, 1, N'Não classificado', NULL, N'9999.99.99', NULL, NULL, NULL, NULL, NULL, 1.25, 1.75)
SET IDENTITY_INSERT [dbo].[tbClassificacaoFiscal] OFF

SET IDENTITY_INSERT [dbo].[tbCodigoSAT] ON 
INSERT [dbo].[tbCodigoSAT] ([IDCodigoSAT], [CodigoRetorno], [Grupo], [Mensagem], [Descricao], [Erro]) VALUES (1, N'06000', N'EnviarDadosVenda', N'Emitido com sucesso', N'Retorno CF-e-SAT ao AC para contingência.', 0)
INSERT [dbo].[tbCodigoSAT] ([IDCodigoSAT], [CodigoRetorno], [Grupo], [Mensagem], [Descricao], [Erro]) VALUES (3, N'06010', N'EnviarDadosVenda', N'Erro de validação do conteúdo.', N'Informar o erro de acordo com a tabela do item 6.3', 1)
SET IDENTITY_INSERT [dbo].[tbCodigoSAT] OFF

SET IDENTITY_INSERT [dbo].[tbMeioPagamentoSAT] ON 
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (1, N'01', N'Dinheiro')
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (2, N'02', N'Cheque')
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (3, N'03', N'Cartão de Crédito')
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (4, N'04', N'Cartão de Débito')
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (5, N'05', N'Crédito Loja')
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (6, N'10', N'Vale Alimentação')
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (7, N'11', N'Vale Refeição')
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (8, N'12', N'Vale Presente')
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (9, N'13', N'Vale Combustível')
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (10, N'99', N'Outros')
SET IDENTITY_INSERT [dbo].[tbMeioPagamentoSAT] OFF

SET IDENTITY_INSERT [dbo].[tbMotivoCancelamento] ON 
INSERT [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento], [Nome]) VALUES (1, N'Teste')
INSERT [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento], [Nome]) VALUES (2, N'Lançamento em comanda errada')
INSERT [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento], [Nome]) VALUES (3, N'Lançamento de produto errado')
INSERT [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento], [Nome]) VALUES (4, N'Cliente diz que não consumiu')
INSERT [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento], [Nome]) VALUES (5, N'Outro motivo')
SET IDENTITY_INSERT [dbo].[tbMotivoCancelamento] OFF

SET IDENTITY_INSERT [dbo].[tbPainelModificacaoOperacao] ON 
INSERT [dbo].[tbPainelModificacaoOperacao] ([IDPainelModificacaoOperacao], [Nome]) VALUES (1, N'Soma')
INSERT [dbo].[tbPainelModificacaoOperacao] ([IDPainelModificacaoOperacao], [Nome]) VALUES (2, N'Maior Valor')
INSERT [dbo].[tbPainelModificacaoOperacao] ([IDPainelModificacaoOperacao], [Nome]) VALUES (3, N'Média')
SET IDENTITY_INSERT [dbo].[tbPainelModificacaoOperacao] OFF

SET IDENTITY_INSERT [dbo].[tbTipoEntrada] ON 
INSERT [dbo].[tbTipoEntrada] ([IDTipoEntrada], [Nome], [ValorEntrada], [ValorConsumacaoMinima], [Ativo], [Padrao]) VALUES (1, N'VIP', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 1, 1)
SET IDENTITY_INSERT [dbo].[tbTipoEntrada] OFF

SET IDENTITY_INSERT [dbo].[tbTipoMovimentacao] ON 
INSERT [dbo].[tbTipoMovimentacao] ([IDTipoMovimentacao], [Tipo], [Nome], [Descricao], [Excluido]) VALUES (1, N'+', N'Recebimento de Mercadoria', N'', 0)
INSERT [dbo].[tbTipoMovimentacao] ([IDTipoMovimentacao], [Tipo], [Nome], [Descricao], [Excluido]) VALUES (2, N'-', N'Entrega de Mercadoria', N'', 0)
SET IDENTITY_INSERT [dbo].[tbTipoMovimentacao] OFF

SET IDENTITY_INSERT [dbo].[tbTipoPagamento] ON 
INSERT [dbo].[tbTipoPagamento] ([IDTipoPagamento], [CodigoImpressoraFiscal], [CodigoERP], [Nome], [RegistrarValores], [PrazoCredito], [Ativo], [IDMeioPagamentoSAT]) VALUES (1, N'01', NULL, N'Dinheiro', 1, 0, 1, 1)
INSERT [dbo].[tbTipoPagamento] ([IDTipoPagamento], [CodigoImpressoraFiscal], [CodigoERP], [Nome], [RegistrarValores], [PrazoCredito], [Ativo], [IDMeioPagamentoSAT]) VALUES (2, N'02', NULL, N'Cartão de Crédito', 1, 0, 1, 3)
INSERT [dbo].[tbTipoPagamento] ([IDTipoPagamento], [CodigoImpressoraFiscal], [CodigoERP], [Nome], [RegistrarValores], [PrazoCredito], [Ativo], [IDMeioPagamentoSAT]) VALUES (3, N'03', NULL, N'Cartão de Débito', 1, 0, 1, 4)
INSERT [dbo].[tbTipoPagamento] ([IDTipoPagamento], [CodigoImpressoraFiscal], [CodigoERP], [Nome], [RegistrarValores], [PrazoCredito], [Ativo], [IDMeioPagamentoSAT]) VALUES (4, N'04', NULL, N'Outros Cartões', 1, 0, 1, 10)
INSERT [dbo].[tbTipoPagamento] ([IDTipoPagamento], [CodigoImpressoraFiscal], [CodigoERP], [Nome], [RegistrarValores], [PrazoCredito], [Ativo], [IDMeioPagamentoSAT]) VALUES (5, N'05', NULL, N'Conta Assinada', 1, 0, 1, 10)
SET IDENTITY_INSERT [dbo].[tbTipoPagamento] OFF

SET IDENTITY_INSERT [dbo].[tbUsuario] ON 
INSERT [dbo].[tbUsuario] ([IDUsuario], [CodigoERP], [Nome], [Login], [Senha], [PermissaoAdm], [PermissaoCaixa], [PermissaoGarcom], [PermissaoGerente], [Ativo], [DtUltimaAlteracao], [Excluido]) VALUES (1, NULL, N'Administrador', NULL, N'2010', 1, 1, 1, 1, 1, CAST(N'2015-06-01T13:23:05.477' AS DateTime), 0)
INSERT [dbo].[tbUsuario] ([IDUsuario], [CodigoERP], [Nome], [Login], [Senha], [PermissaoAdm], [PermissaoCaixa], [PermissaoGarcom], [PermissaoGerente], [Ativo], [DtUltimaAlteracao], [Excluido]) VALUES (2, NULL, N'Autoatendimento', NULL, N'9933', 0, 0, 1, 1, 1, CAST(N'2018-04-18T09:18:30.512' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[tbUsuario] OFF

SET IDENTITY_INSERT [dbo].[tbTipoDesconto] ON 
INSERT [dbo].[tbTipoDesconto] ([IDTipoDesconto], [Nome], [Descricao], [Ativo], [Excluido]) VALUES (1, N'Desconto Funcionário', N'', 1, 0)
INSERT [dbo].[tbTipoDesconto] ([IDTipoDesconto], [Nome], [Descricao], [Ativo], [Excluido]) VALUES (2, N'Desconto Ajuste Troco', N'', 1, 0)
INSERT [dbo].[tbTipoDesconto] ([IDTipoDesconto], [Nome], [Descricao], [Ativo], [Excluido]) VALUES (3, N'Desconto Cortesia', N'', 1, 0)
SET IDENTITY_INSERT [dbo].[tbTipoDesconto] OFF

COMMIT TRANSACTION T1