USE [pdv7]
GO
/****** Object:  Table [dbo].[tbAcao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbAcao](
  [IDAcao] [int] IDENTITY(1,1) NOT NULL,
  [IDPDV] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [Parametro] [varchar](max) NULL,
  [DtSolicitacao] [datetime] NULL,
 CONSTRAINT [PK_tbAcao] PRIMARY KEY CLUSTERED 
(
  [IDAcao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbAreaImpressao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbAreaImpressao](
  [IDAreaImpressao] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [NomeImpressora] [varchar](50) NULL,
  [IDTipoAreaImpressao] [int] NOT NULL,
 CONSTRAINT [PK_tbAreaImpressao] PRIMARY KEY CLUSTERED 
(
  [IDAreaImpressao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbCaixa]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbCaixa](
  [IDCaixa] [int] IDENTITY(1,1) NOT NULL,
  [IDPDV] [int] NOT NULL,
  [IDUsuario] [int] NULL,
  [IDFechamento] [int] NULL,
  [DtAbertura] [datetime] NOT NULL,
  [DtFechamento] [datetime] NULL,
  [SincERP] [bit] NOT NULL,
 CONSTRAINT [PK_tbCaixa] PRIMARY KEY CLUSTERED 
(
  [IDCaixa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbCaixaAjuste]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbCaixaAjuste](
  [IDCaixaAjuste] [int] IDENTITY(1,1) NOT NULL,
  [IDCaixa] [int] NOT NULL,
  [IDCaixaTipoAjuste] [int] NOT NULL,
  [Valor] [decimal](18, 2) NOT NULL,
  [Descricao] [varchar](500) NULL,
  [DtAjuste] [datetime] NOT NULL,
 CONSTRAINT [PK_tbCaixaSuprimento] PRIMARY KEY CLUSTERED 
(
  [IDCaixaAjuste] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbCaixaTipoAjuste]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbCaixaTipoAjuste](
  [IDCaixaTipoAjuste] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbCaixaTipoAjuste] PRIMARY KEY CLUSTERED 
(
  [IDCaixaTipoAjuste] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbCaixaValorRegistro]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbCaixaValorRegistro](
  [IDCaixaValorRegistro] [int] IDENTITY(1,1) NOT NULL,
  [IDCaixa] [int] NOT NULL,
  [IDTipoPagamento] [int] NOT NULL,
  [ValorAbertura] [decimal](18, 2) NULL,
  [ValorFechamento] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbCaixaValor] PRIMARY KEY CLUSTERED 
(
  [IDCaixaValorRegistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbCategoriaProduto]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbCategoriaProduto](
  [IDCategoriaProduto] [int] IDENTITY(1,1) NOT NULL,
  [CodigoERP] [varchar](50) NULL,
  [Nome] [varchar](50) NOT NULL,
  [DtUltimaAlteracao] [datetime] NOT NULL,
  [Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_tbCategoriaProduto] PRIMARY KEY CLUSTERED 
(
  [IDCategoriaProduto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbClassificacaoFiscal]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbClassificacaoFiscal](
  [IDClassificacaoFiscal] [int] IDENTITY(1,1) NOT NULL,
  [IDTipoTributacao] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [Descricao] [varchar](500) NULL,
  [NCM] [varchar](50) NOT NULL,
  [IOF] [decimal](18, 6) NULL,
  [IPI] [decimal](18, 6) NULL,
  [PISPASEP] [decimal](18, 6) NULL,
  [CIDE] [decimal](18, 6) NULL,
  [COFINS] [decimal](18, 6) NULL,
  [ICMS] [decimal](18, 6) NULL,
  [ISS] [decimal](18, 6) NULL,
 CONSTRAINT [PK_tbClassificacaoFiscal] PRIMARY KEY CLUSTERED 
(
  [IDClassificacaoFiscal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbCliente]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbCliente](
  [IDCliente] [int] IDENTITY(1,1) NOT NULL,
  [CodigoERP] [varchar](50) NULL,
  [NomeCompleto] [varchar](50) NULL,
  [Telefone1DDD] [int] NULL,
  [Telefone1Numero] [int] NULL,
  [Telefone2DDD] [int] NULL,
  [Telefone2Numero] [int] NULL,
  [Documento1] [varchar](50) NULL,
  [Limite] [decimal](18, 2) NULL,
  [Credito] [decimal](18, 2) NULL,
  [Bloqueado] [bit] NOT NULL,
  [Observacao] [varchar](max) NULL,
  [Endereco] [varchar](500) NULL,
  [EnderecoNumero] [varchar](50) NULL,
  [Complemento] [varchar](500) NULL,
  [Bairro] [varchar](500) NULL,
  [Cidade] [varchar](500) NULL,
  [IDEstado] [int] NULL,
  [CEP] [int] NULL,
  [EnderecoReferencia] [varchar](max) NULL,
  [DataNascimento] [date] NULL,
  [Sexo] [varchar](1) NULL,
  [DtInclusao] [date] NOT NULL,
  [Email] [varchar](500) NULL,
 CONSTRAINT [PK_tbPedidoComanda] PRIMARY KEY CLUSTERED 
(
  [IDCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbCodigoSAT]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbCodigoSAT](
  [IDCodigoSAT] [int] IDENTITY(1,1) NOT NULL,
  [CodigoRetorno] [varchar](6) NOT NULL,
  [Grupo] [varchar](50) NULL,
  [Mensagem] [varchar](max) NULL,
  [Descricao] [varchar](max) NULL,
  [Erro] [bit] NOT NULL,
 CONSTRAINT [PK_tbSATCodigoEEEEE] PRIMARY KEY CLUSTERED 
(
  [IDCodigoSAT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbComanda]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbComanda](
  [IDComanda] [int] IDENTITY(1,1) NOT NULL,
  [IDStatusComanda] [int] NOT NULL,
  [GUIDIdentificacao] [varchar](50) NOT NULL,
  [Numero] [int] NOT NULL,
  [Observacao] [varchar](5000) NULL,
 CONSTRAINT [PK_tbComanda] PRIMARY KEY CLUSTERED 
(
  [IDComanda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbConfiguracao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbConfiguracao](
  [Chave] [varchar](500) NOT NULL,
  [Valor] [varchar](500) NULL,
  [Descricao] [varchar](500) NULL,
 CONSTRAINT [PK_tbConfiguracao] PRIMARY KEY CLUSTERED 
(
  [Chave] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbConversaoUnidade]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbConversaoUnidade](
  [IDConversaoUnidade] [int] IDENTITY(1,1) NOT NULL,
  [IDUnidade_de] [int] NOT NULL,
  [IDUnidade_para] [int] NOT NULL,
  [Divisao] [numeric](18, 6) NOT NULL,
  [Multiplicacao] [numeric](18, 6) NOT NULL,
 CONSTRAINT [PK_tbConversaoUnidade] PRIMARY KEY CLUSTERED 
(
  [IDConversaoUnidade] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbEntradaSaida]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbEntradaSaida](
  [IDEntradaSaida] [int] IDENTITY(1,1) NOT NULL,
  [GUID_Origem] [nchar](36) NULL,
  [IDProduto] [int] NOT NULL,
  [Entrada] [bit] NOT NULL,
  [Quantidade] [numeric](18, 4) NOT NULL,
  [Data] [datetime] NOT NULL,
 CONSTRAINT [PK_tbEntradaSaida] PRIMARY KEY CLUSTERED 
(
  [IDEntradaSaida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbEntregador]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbEntregador](
  [IDEntregador] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](200) NOT NULL,
  [Ativo] [bit] NOT NULL,
  [Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_tbEntregador] PRIMARY KEY CLUSTERED 
(
  [IDEntregador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbEstado]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbEstado](
  [IDEstado] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [Sigla] [varchar](2) NOT NULL,
 CONSTRAINT [PK_tbEstado] PRIMARY KEY CLUSTERED 
(
  [IDEstado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbFechamento]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbFechamento](
  [IDFechamento] [int] IDENTITY(1,1) NOT NULL,
  [IDPDV] [int] NOT NULL,
  [IDUsuario] [int] NOT NULL,
  [DtFechamento] [datetime] NOT NULL,
 CONSTRAINT [PK_tbFechamento] PRIMARY KEY CLUSTERED 
(
  [IDFechamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbIdioma]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbIdioma](
  [IDIdioma] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](100) NOT NULL,
  [Codigo] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
  [IDIdioma] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbImagem]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbImagem](
  [IDImagem] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](100) NOT NULL,
  [Extensao] [varchar](50) NOT NULL,
  [Altura] [int] NULL,
  [Largura] [int] NULL,
  [Tamanho] [int] NULL,
  [Dados] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_tbImagem] PRIMARY KEY CLUSTERED 
(
  [IDImagem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbImagemTema]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbImagemTema](
  [IDImagemTema] [int] IDENTITY(1,1) NOT NULL,
  [IDTemaCardapio] [int] NOT NULL,
  [IDImagem] [int] NOT NULL,
  [IDIdioma] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [Descricao] [varchar](300) NULL,
 CONSTRAINT [PK_tbImagemTema] PRIMARY KEY CLUSTERED 
(
  [IDImagemTema] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbIntegracaoSAT]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbIntegracaoSAT](
  [IDIntegracaoSAT] [int] IDENTITY(1,1) NOT NULL,
  [IDPDV] [int] NOT NULL,
 CONSTRAINT [PK_tbIntegracaoSAT] PRIMARY KEY CLUSTERED 
(
  [IDIntegracaoSAT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbInventario]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInventario](
  [IDInventario] [int] IDENTITY(1,1) NOT NULL,
  [GUID] [nchar](36) NOT NULL,
  [Descricao] [varchar](255) NOT NULL,
  [Data] [datetime] NOT NULL,
  [Processado] [bit] NOT NULL,
  [Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_tbInventario] PRIMARY KEY CLUSTERED 
(
  [IDInventario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbInventarioProdutos]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInventarioProdutos](
  [IDInventarioProdutos] [int] IDENTITY(1,1) NOT NULL,
  [IDInventario] [int] NOT NULL,
  [IDProduto] [int] NOT NULL,
  [IDUnidade] [int] NOT NULL,
  [Quantidade] [numeric](18, 6) NOT NULL,
  [QuantidadeAnterior] [numeric](18, 6) NOT NULL,
 CONSTRAINT [PK_tbInventarioProdutos] PRIMARY KEY CLUSTERED 
(
  [IDInventarioProdutos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbMapAreaImpressaoProduto]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbMapAreaImpressaoProduto](
  [IDMapAreaImpressaoProduto] [int] IDENTITY(1,1) NOT NULL,
  [IDAreaImpressao] [int] NOT NULL,
  [IDProduto] [int] NOT NULL,
 CONSTRAINT [PK_tbMapImpressoraProduto] PRIMARY KEY CLUSTERED 
(
  [IDMapAreaImpressaoProduto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbMeioPagamentoSAT]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbMeioPagamentoSAT](
  [IDMeioPagamentoSAT] [int] IDENTITY(1,1) NOT NULL,
  [Codigo] [nchar](2) NOT NULL,
  [Descricao] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbMeioPagamentoSAT] PRIMARY KEY CLUSTERED 
(
  [IDMeioPagamentoSAT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbMesa]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbMesa](
  [IDMesa] [int] IDENTITY(1,1) NOT NULL,
  [IDStatusMesa] [int] NOT NULL,
  [GUIDIdentificacao] [varchar](50) NOT NULL,
  [Numero] [int] NOT NULL,
  [Capacidade] [int] NULL,
 CONSTRAINT [PK_tbMesa] PRIMARY KEY CLUSTERED 
(
  [IDMesa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbMotivoCancelamento]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbMotivoCancelamento](
  [IDMotivoCancelamento] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbMotivoCancelamento] PRIMARY KEY CLUSTERED 
(
  [IDMotivoCancelamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbMovimentacao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbMovimentacao](
  [IDMovimentacao] [int] IDENTITY(1,1) NOT NULL,
  [IDTipoMovimentacao] [int] NOT NULL,
  [IDFornecedor] [int] NULL,
  [GUID] [nchar](36) NOT NULL,
  [DataMovimentacao] [datetime] NOT NULL,
  [Descricao] [varchar](100) NULL,
  [NumeroPedido] [varchar](50) NULL,
  [IDMovimentacao_reversa] [int] NULL,
  [Excluido] [bit] NOT NULL,
  [Reversa] [bit] NOT NULL,
  [Processado] [bit] NOT NULL,
 CONSTRAINT [PK_tbMovimentacao] PRIMARY KEY CLUSTERED 
(
  [IDMovimentacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbMovimentacaoProdutos]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbMovimentacaoProdutos](
  [IDMovimentacaoProdutos] [int] IDENTITY(1,1) NOT NULL,
  [IDMovimentacao] [int] NOT NULL,
  [IDProduto] [int] NOT NULL,
  [IDUnidade] [int] NOT NULL,
  [Quantidade] [numeric](18, 4) NOT NULL,
 CONSTRAINT [PK_tbMovimentacaoProdutos] PRIMARY KEY CLUSTERED 
(
  [IDMovimentacaoProdutos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbOrdemImpressao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbOrdemImpressao](
  [IDOrdemImpressao] [int] IDENTITY(1,1) NOT NULL,
  [IDAreaImpressao] [int] NOT NULL,
  [ConteudoImpressao] [varchar](max) NOT NULL,
  [DtOrdem] [datetime] NOT NULL,
  [EnviadoFilaImpressao] [bit] NOT NULL,
  [Conta] [bit] NOT NULL,
  [SAT] [bit] NOT NULL,
 CONSTRAINT [PK_tbOrdemImpressao] PRIMARY KEY CLUSTERED 
(
  [IDOrdemImpressao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbPainelModificacao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbPainelModificacao](
  [IDPainelModificacao] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](500) NOT NULL,
  [Titulo] [varchar](500) NULL,
  [Minimo] [int] NULL,
  [Maximo] [int] NULL,
  [DtUltimaAlteracao] [datetime] NOT NULL,
  [Excluido] [bit] NOT NULL,
  [IDPainelModificacaoOperacao] [int] NOT NULL,
 CONSTRAINT [PK_tbPainelModificacao] PRIMARY KEY CLUSTERED 
(
  [IDPainelModificacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbPainelModificacaoOperacao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbPainelModificacaoOperacao](
  [IDPainelModificacaoOperacao] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbPainelModificacaoOperacao] PRIMARY KEY CLUSTERED 
(
  [IDPainelModificacaoOperacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbPainelModificacaoProduto]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbPainelModificacaoProduto](
  [IDPainelModificacaoProduto] [int] IDENTITY(1,1) NOT NULL,
  [IDPainelModificacao] [int] NOT NULL,
  [IDProduto] [int] NOT NULL,
  [IgnorarValorItem] [bit] NULL,
  [Ordem] [int] NOT NULL,
 CONSTRAINT [PK_tbProdutoPainelModificacao] PRIMARY KEY CLUSTERED 
(
  [IDPainelModificacaoProduto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbPDV]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbPDV](
  [IDPDV] [int] NOT NULL,
  [Nome] [varchar](50) NULL,
  [Dados] [varchar](max) NULL,
 CONSTRAINT [PK_tbPDV] PRIMARY KEY CLUSTERED 
(
  [IDPDV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbPedido]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbPedido](
  [IDPedido] [int] IDENTITY(1,1) NOT NULL,
  [IDCliente] [int] NULL,
  [IDTipoPedido] [int] NOT NULL,
  [IDStatusPedido] [int] NOT NULL,
  [IDCaixa] [int] NULL,
  [IDUsuario_garcom] [int] NULL,
  [IDTipoEntrada] [int] NULL,
  [IDRetornoSAT_venda] [int] NULL,
  [IDRetornoSAT_cancelamento] [int] NULL,
  [IDTipoDesconto] [int] NULL,
  [IDTaxaEntrega] [int] NULL,
  [IDEntregador] [int] NULL,
  [GUIDIdentificacao] [varchar](50) NULL,
  [CodigoEmenu] [varchar](50) NULL,
  [NumeroCupom] [varchar](50) NULL,
  [DocumentoCliente] [varchar](50) NULL,
  [DtPedido] [datetime] NULL,
  [DtPedidoFechamento] [datetime] NULL,
  [SincERP] [bit] NULL,
  [SincEMENU] [bit] NULL,
  [ValorConsumacaoMinima] [decimal](18, 2) NULL,
  [ValorServico] [decimal](18, 2) NULL,
  [ValorDesconto] [decimal](18, 2) NULL,
  [TaxaServicoPadrao] [decimal](18, 2) NULL,
  [ValorTotal] [decimal](18, 2) NULL,
  [GUIDAgrupamentoPedido] [varchar](50) NULL,
  [Observacoes] [varchar](max) NULL,
  [ReferenciaLocalizacao] [varchar](500) NULL,
  [GUIDMovimentacao] [varchar](50) NULL,
  [NumeroPessoas] [int] NULL,
  [DtEnvio] [datetime] NULL,
  [DtEntrega] [datetime] NULL,
  [ValorEntrega] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbPedido] PRIMARY KEY CLUSTERED 
(
  [IDPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbPedidoPagamento]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbPedidoPagamento](
  [IDPedidoPagamento] [int] IDENTITY(1,1) NOT NULL,
  [IDPedido] [int] NOT NULL,
  [IDTipoPagamento] [int] NOT NULL,
  [Valor] [decimal](18, 2) NOT NULL,
  [Autorizacao] [varchar](50) NULL,
 CONSTRAINT [PK_IDPedidoPagamento] PRIMARY KEY CLUSTERED 
(
  [IDPedidoPagamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbPedidoProduto]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbPedidoProduto](
  [IDPedidoProduto] [int] IDENTITY(1,1) NOT NULL,
  [IDPedido] [int] NOT NULL,
  [IDProduto] [int] NOT NULL,
  [IDPedidoProduto_pai] [int] NULL,
  [IDPDV] [int] NULL,
  [IDUsuario] [int] NULL,
  [IDPDV_cancelamento] [int] NULL,
  [IDUsuario_cancelamento] [int] NULL,
  [IDMotivoCancelamento] [int] NULL,
  [IDPainelModificacao] [int] NULL,
  [Quantidade] [decimal](18, 2) NOT NULL,
  [ValorUnitario] [decimal](18, 2) NOT NULL,
  [ValorAliquota] [decimal](18, 2) NULL,
  [CodigoAliquota] [varchar](50) NULL,
  [Notas] [varchar](max) NULL,
  [Cancelado] [bit] NOT NULL,
  [DtInclusao] [datetime] NULL,
  [DtAlteracao] [datetime] NULL,
  [ObservacoesCancelamento] [varchar](500) NULL,
  [DtCancelamento] [datetime] NULL,
  [GUIDControleDuplicidade] [varchar](50) NULL,
  [RetornarAoEstoque] [bit] NOT NULL,
 CONSTRAINT [PK_tbPedidoItem] PRIMARY KEY CLUSTERED 
(
  [IDPedidoProduto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbProcessamentoSAT]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbProcessamentoSAT](
  [IDProcessamentoSAT] [int] IDENTITY(1,1) NOT NULL,
  [IDStatusProcessamentoSAT] [int] NULL,
  [IDTipoSolicitacaoSAT] [int] NULL,
  [IDRetornoSAT] [int] NULL,
  [XMLEnvio] [varchar](max) NOT NULL,
  [GUID] [varchar](50) NOT NULL,
  [NumeroSessao] [int] NULL,
  [DataSolicitacao] [datetime] NOT NULL,
 CONSTRAINT [PK_tbProcessamentoSAT] PRIMARY KEY CLUSTERED 
(
  [IDProcessamentoSAT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbProduto]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbProduto](
  [IDProduto] [int] IDENTITY(1,1) NOT NULL,
  [IDTipoProduto] [int] NOT NULL,
  [Codigo] [varchar](50) NULL,
  [CodigoERP] [varchar](50) NULL,
  [Nome] [varchar](500) NOT NULL,
  [Descricao] [varchar](500) NULL,
  [ValorUnitario] [decimal](18, 2) NOT NULL,
  [ValorAliquota] [decimal](18, 2) NULL,
  [CodigoAliquota] [varchar](50) NULL,
  [Quantidade] [decimal](18, 2) NULL,
  [Ativo] [bit] NOT NULL,
  [Disponibilidade] [bit] NOT NULL,
  [DtAlteracaoDisponibilidade] [datetime] NOT NULL,
  [DtUltimaAlteracao] [datetime] NOT NULL,
  [Excluido] [bit] NOT NULL,
  [IDClassificacaoFiscal] [int] NULL,
  [IDUnidade] [int] NULL,
  [cEAN] [varchar](50) NULL,
  [ControlarEstoque] [bit] NOT NULL,
  [UtilizarBalanca] [bit] NOT NULL,
  [CodigoIntegracao1] [varchar](50) NULL,
  [IDIntegracao1] [varchar](50) NULL,
  [DescricaoIntegracao1] [varchar](200) NULL,
  [CodigoIntegracao2] [varchar](50) NULL,
  [IDIntegracao2] [varchar](50) NULL,
  [DescricaoIntegracao2] [varchar](200) NULL,
 CONSTRAINT [PK_tbProduto] PRIMARY KEY CLUSTERED 
(
  [IDProduto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbProdutoCategoriaProduto]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbProdutoCategoriaProduto](
  [IDProdutoCategoriaProduto] [int] IDENTITY(1,1) NOT NULL,
  [IDProduto] [int] NOT NULL,
  [IDCategoriaProduto] [int] NOT NULL,
 CONSTRAINT [PK_tbProdutoCategoriaProduto] PRIMARY KEY CLUSTERED 
(
  [IDProdutoCategoriaProduto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbProdutoImagem]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbProdutoImagem](
  [IDProdutoImagem] [int] IDENTITY(1,1) NOT NULL,
  [IDProduto] [int] NOT NULL,
  [IDImagem] [int] NOT NULL,
 CONSTRAINT [PK_tb] PRIMARY KEY CLUSTERED 
(
  [IDProdutoImagem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbProdutoPainelModificacao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbProdutoPainelModificacao](
  [IDProdutoPainelModificacao] [int] IDENTITY(1,1) NOT NULL,
  [IDProduto] [int] NOT NULL,
  [IDPainelModificacao] [int] NOT NULL,
  [Ordem] [int] NOT NULL,
 CONSTRAINT [PK_tbProdutoPainelModificacao_1] PRIMARY KEY CLUSTERED 
(
  [IDProdutoPainelModificacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbProdutoReceita]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbProdutoReceita](
  [IDProdutoReceita] [int] IDENTITY(1,1) NOT NULL,
  [IDProduto] [int] NOT NULL,
  [IDProduto_ingrediente] [int] NOT NULL,
  [IDUnidade] [int] NOT NULL,
  [Quantidade] [numeric](18, 4) NOT NULL,
 CONSTRAINT [PK_tbProdutoReceita] PRIMARY KEY CLUSTERED 
(
  [IDProdutoReceita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbProdutoTraducao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbProdutoTraducao](
  [IDProdutoTraducao] [int] IDENTITY(1,1) NOT NULL,
  [IDProduto] [int] NOT NULL,
  [IDIdioma] [int] NOT NULL,
  [Nome] [varchar](500) NOT NULL,
  [Descricao] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
  [IDProdutoTraducao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbRelatorio]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbRelatorio](
  [IDRelatorio] [int] IDENTITY(1,1) NOT NULL,
  [IDTipoRelatorio] [int] NOT NULL,
  [Nome] [varchar](500) NOT NULL,
  [Descricao] [varchar](500) NULL,
  [QuerySQL] [varchar](max) NOT NULL,
  [Ordem] [int] NOT NULL,
  [Totalizador] [varchar](100) NULL,
 CONSTRAINT [PK_tbRelatorio] PRIMARY KEY CLUSTERED 
(
  [IDRelatorio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbRetornoSAT]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbRetornoSAT](
  [IDRetornoSAT] [int] IDENTITY(1,1) NOT NULL,
  [IDTipoSolicitacaoSAT] [int] NOT NULL,
  [numeroSessao] [varchar](max) NULL,
  [EEEEE] [varchar](max) NULL,
  [CCCC] [varchar](max) NULL,
  [mensagem] [varchar](max) NULL,
  [cod] [varchar](max) NULL,
  [mensagemSEFAZ] [varchar](max) NULL,
  [arquivoCFeSAT] [varchar](max) NULL,
  [timeStamp] [varchar](max) NULL,
  [chaveConsulta] [varchar](max) NULL,
  [valorTotalCFe] [varchar](max) NULL,
  [CPFCNPJValue] [varchar](max) NULL,
  [assinaturaQRCODE] [varchar](max) NULL,
  [IDRetornoSAT_cancelamento] [int] NULL,
 CONSTRAINT [PK_tbPedidoRetornoSAT] PRIMARY KEY CLUSTERED 
(
  [IDRetornoSAT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbStatusComanda]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbStatusComanda](
  [IDStatusComanda] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbStatusComanda] PRIMARY KEY CLUSTERED 
(
  [IDStatusComanda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbStatusMesa]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbStatusMesa](
  [IDStatusMesa] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbStatusMesa] PRIMARY KEY CLUSTERED 
(
  [IDStatusMesa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbStatusPedido]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbStatusPedido](
  [IDStatusPedido] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbStatusPedido] PRIMARY KEY CLUSTERED 
(
  [IDStatusPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbStatusProcessamentoSAT]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbStatusProcessamentoSAT](
  [IDStatusProcessamentoSAT] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbStatusProcessamentoSAT] PRIMARY KEY CLUSTERED 
(
  [IDStatusProcessamentoSAT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTaxaEntrega]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTaxaEntrega](
  [IDTaxaEntrega] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](200) NOT NULL,
  [Valor] [decimal](18, 2) NOT NULL,
  [Ativo] [bit] NOT NULL,
  [Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_tbTaxaEntrega] PRIMARY KEY CLUSTERED 
(
  [IDTaxaEntrega] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTemaCardapio]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTemaCardapio](
  [IDTemaCardapio] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [Descricao] [varchar](max) NULL,
  [XML] [varchar](max) NOT NULL,
 CONSTRAINT [PK_tbTemaCardapio] PRIMARY KEY CLUSTERED 
(
  [IDTemaCardapio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTemaCardapioPDV]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTemaCardapioPDV](
  [IDTemaCardapioPDV] [int] IDENTITY(1,1) NOT NULL,
  [IDTemaCardapio] [int] NOT NULL,
  [IDPDV] [int] NULL,
  [Ativo] [bit] NOT NULL,
  [DtUltimaAlteracao] [datetime] NOT NULL,
 CONSTRAINT [PK_tbTemaCardapioPDV] PRIMARY KEY CLUSTERED 
(
  [IDTemaCardapioPDV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoAreaImpressao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoAreaImpressao](
  [IDTipoAreaImpressao] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbTipoAreaImpressao] PRIMARY KEY CLUSTERED 
(
  [IDTipoAreaImpressao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoDesconto]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoDesconto](
  [IDTipoDesconto] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [Descricao] [varchar](500) NULL,
  [Ativo] [bit] NOT NULL,
  [Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_tbTipoDesconto] PRIMARY KEY CLUSTERED 
(
  [IDTipoDesconto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoEntrada]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoEntrada](
  [IDTipoEntrada] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [ValorEntrada] [decimal](18, 2) NOT NULL,
  [ValorConsumacaoMinima] [decimal](18, 2) NOT NULL,
  [Ativo] [bit] NOT NULL,
  [Padrao] [bit] NOT NULL,
 CONSTRAINT [PK_tbTipoEntrada] PRIMARY KEY CLUSTERED 
(
  [IDTipoEntrada] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoMovimentacao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoMovimentacao](
  [IDTipoMovimentacao] [int] IDENTITY(1,1) NOT NULL,
  [Tipo] [char](1) NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [Descricao] [varchar](255) NULL,
  [Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_tbTipoMovimentacao] PRIMARY KEY CLUSTERED 
(
  [IDTipoMovimentacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoPagamento]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoPagamento](
  [IDTipoPagamento] [int] IDENTITY(1,1) NOT NULL,
  [CodigoImpressoraFiscal] [varchar](50) NULL,
  [CodigoERP] [varchar](50) NULL,
  [Nome] [varchar](50) NOT NULL,
  [RegistrarValores] [bit] NOT NULL,
  [PrazoCredito] [int] NOT NULL,
  [Ativo] [bit] NOT NULL,
  [IDMeioPagamentoSAT] [int] NULL,
 CONSTRAINT [PK_tbTipoPagamento] PRIMARY KEY CLUSTERED 
(
  [IDTipoPagamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoPDV]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoPDV](
  [IDTipoPDV] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbTipoPDV] PRIMARY KEY CLUSTERED 
(
  [IDTipoPDV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoPedido]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoPedido](
  [IDTipoPedido] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbTipoPedido] PRIMARY KEY CLUSTERED 
(
  [IDTipoPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoProduto]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoProduto](
  [IDTipoProduto] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbTipoProduto] PRIMARY KEY CLUSTERED 
(
  [IDTipoProduto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoRelatorio]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoRelatorio](
  [IDTipoRelatorio] [int] NOT NULL,
  [Nome] [varchar](50) NULL,
 CONSTRAINT [PK_tbTipoRelatorio] PRIMARY KEY CLUSTERED 
(
  [IDTipoRelatorio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoSolicitacaoSAT]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoSolicitacaoSAT](
  [IDTipoSolicitacaoSAT] [int] NOT NULL,
  [Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbTipoRetornoSAT] PRIMARY KEY CLUSTERED 
(
  [IDTipoSolicitacaoSAT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbTipoTributacao]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTipoTributacao](
  [IDTipoTributacao] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [Descricao] [varchar](500) NULL,
  [CFOP] [varchar](50) NOT NULL,
  [ICMS00_Orig] [varchar](50) NULL,
  [ICMS00_CST] [varchar](50) NULL,
  [ICMS00_pICMS] [varchar](50) NULL,
  [ICMS40_Orig] [varchar](50) NULL,
  [ICMS40_CST] [varchar](50) NULL,
  [ICMSSN102_Orig] [varchar](50) NULL,
  [ICMSSN102_CSOSN] [varchar](50) NULL,
  [ICMSSN900_Orig] [varchar](50) NULL,
  [ICMSSN900_CSOSN] [varchar](50) NULL,
  [ICMSSN900_pICMS] [varchar](50) NULL,
  [PISAliq_CST] [varchar](50) NULL,
  [PISAliq_pPIS] [varchar](50) NULL,
  [PISQtde_CST] [varchar](50) NULL,
  [PISQtde_vAliqProd] [varchar](50) NULL,
  [PISNT_CST] [varchar](50) NULL,
  [PISSN_CST] [varchar](50) NULL,
  [PISOutr_CST] [varchar](50) NULL,
  [PISOutr_pPIS] [varchar](50) NULL,
  [PISOutr_vAliqProd] [varchar](50) NULL,
  [PISST_pPIS] [varchar](50) NULL,
  [PISST_vAliqProd] [varchar](50) NULL,
  [COFINSAliq_CST] [varchar](50) NULL,
  [COFINSAliq_pCOFINS] [varchar](50) NULL,
  [COFINSQtde_CST] [varchar](50) NULL,
  [COFINSQtde_vAliqProd] [varchar](50) NULL,
  [COFINSNT_CST] [varchar](50) NULL,
  [COFINSSN_CST] [varchar](50) NULL,
  [COFINSOutr_CST] [varchar](50) NULL,
  [COFINSOutr_pCOFINS] [varchar](50) NULL,
  [COFINSOutr_vAliqProd] [varchar](50) NULL,
  [COFINSST_pCOFINS] [varchar](50) NULL,
  [COFINSST_vAliqProd] [varchar](50) NULL,
  [ISSQN_vDeducISSQN] [varchar](50) NULL,
  [ISSQN_vAliq] [varchar](50) NULL,
  [ISSQN_cListServ] [varchar](50) NULL,
  [ISSQN_cServTribMun] [varchar](50) NULL,
  [ISSQN_cNatOp] [varchar](50) NULL,
  [ISSQN_indIncFisc] [varchar](50) NULL,
  [vItem12741] [varchar](50) NULL,
 CONSTRAINT [PK_tbTipoTributacao] PRIMARY KEY CLUSTERED 
(
  [IDTipoTributacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbUnidade]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbUnidade](
  [IDUnidade] [int] IDENTITY(1,1) NOT NULL,
  [Nome] [varchar](50) NOT NULL,
  [Simbolo] [varchar](50) NOT NULL,
  [Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_tbUnidade] PRIMARY KEY CLUSTERED 
(
  [IDUnidade] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbUsuario]    Script Date: 28/03/2017 17:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbUsuario](
  [IDUsuario] [int] IDENTITY(1,1) NOT NULL,
  [CodigoEMENU] [varchar](50) NULL,
  [CodigoERP] [varchar](50) NULL,
  [Nome] [varchar](50) NOT NULL,
  [Login] [varchar](50) NULL,
  [Senha] [varchar](50) NULL,
  [PermissaoAdm] [bit] NOT NULL,
  [PermissaoCaixa] [bit] NOT NULL,
  [PermissaoGarcom] [bit] NOT NULL,
  [PermissaoGerente] [bit] NOT NULL,
  [Ativo] [bit] NOT NULL,
  [DtUltimaAlteracao] [datetime] NOT NULL,
  [Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_tbUsuario] PRIMARY KEY CLUSTERED 
(
  [IDUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[tbCaixaTipoAjuste] ([IDCaixaTipoAjuste], [Nome]) VALUES (20, N'Suprimento')
GO
INSERT [dbo].[tbCaixaTipoAjuste] ([IDCaixaTipoAjuste], [Nome]) VALUES (30, N'Sangria')
GO
SET IDENTITY_INSERT [dbo].[tbClassificacaoFiscal] ON 

GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (2, 1, N'Serviço   ', NULL, N'99', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (3, 2, N'Bala / Chiclete / Bombom', NULL, N'1704.10.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (4, 1, N'Prato / porção', NULL, N'2106.90.90', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (5, 1, N'Suco natural / vitamina / cocktel sem alcool', NULL, N'2106.90.90', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (6, 2, N'Refrigerante', NULL, N'2202.00.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (7, 2, N'Água / água com gás / água saborizada', NULL, N'2202.10.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (8, 2, N'Energético / Café', NULL, N'2202.90.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (9, 2, N'Cerveja / Chopp / Espumante', NULL, N'2203.00.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (10, 2, N'Vinho / Saque', NULL, N'2206.00.90', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (11, 2, N'Whisky', NULL, N'2208.30.10', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (12, 2, N'Vodka', NULL, N'2208.66.00', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal], [IDTipoTributacao], [Nome], [Descricao], [NCM], [IOF], [IPI], [PISPASEP], [CIDE], [COFINS], [ICMS], [ISS]) VALUES (13, 1, N'Não classificado', NULL, N'9999.99.99', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tbClassificacaoFiscal] OFF
GO
SET IDENTITY_INSERT [dbo].[tbCodigoSAT] ON 

GO
INSERT [dbo].[tbCodigoSAT] ([IDCodigoSAT], [CodigoRetorno], [Grupo], [Mensagem], [Descricao], [Erro]) VALUES (1, N'06000', N'EnviarDadosVenda', N'Emitido com sucesso', N'Retorno CF-e-SAT ao AC para contingência.', 0)
GO
INSERT [dbo].[tbCodigoSAT] ([IDCodigoSAT], [CodigoRetorno], [Grupo], [Mensagem], [Descricao], [Erro]) VALUES (3, N'06010', N'EnviarDadosVenda', N'Erro de validação do conteúdo.', N'Informar o erro de acordo com a tabela do item 6.3', 1)
GO
SET IDENTITY_INSERT [dbo].[tbCodigoSAT] OFF
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'AliquotaPadrao', N'II', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'atualizado211', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'autSempre', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'comandaComCheckin', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'creditoPadrao', N'0', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'DDDPadrao', N'11', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'dtUltimaVerificacao', NULL, NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'dtValidade', NULL, NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'ImagemProdutoAltura', N'800', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'ImagemProdutoAlturaThumb', N'80', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'ImagemProdutoLargura', N'1280', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'ImagemProdutoLarguraThumb', N'128', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'imprimirPedidosDescontoFechamento', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'imprimirProdutosCanceladosFechamento', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'imprimirProdutosVendidosFechamento', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'infCFe_12741', N'1', N'Define se o sistema enviará os valores aproximados dos tributos conforme lei 12.741/2012.')
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'integracao1', N'0', N'')
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'integracao2', N'0', N'')
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'limiteComanda', N'300', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'ManterImpressaoPorDias', N'0', N'Tempo em dias que as ordens de impressão serão armazenadas no sistema.')
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'msgCupom', N'Sistema PDV7
www.pdvseven.com.br', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'PermitirPedidoModificacaoInvalido', N'0', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'PossuiCardapio', N'0', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'PossuiSAT', N'0', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'RelatorioFechamento-PedidosDescontoDetalhe', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'RelatorioFechamento-PedidosDescontoResumo', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'RelatorioFechamento-ProdutosAbertos', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'RelatorioFechamento-ProdutosCancelados', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'RelatorioFechamento-ProdutosVendidos', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'RelatorioFechamento-ResumoCaixa', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'RelatorioFechamento-ResumoTipoPagamento', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'SolicitarSenhaDesconto', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'SolicitarTipoDesconto', N'0', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'taxaServicoBalcao', N'0', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'taxaServicoComanda', N'0', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'taxaServicoEntrega', N'0', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'taxaServicoMesa', N'0', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'tipoPedidoBalcao', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'tipoPedidoComanda', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'tipoPedidoEntrega', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'tipoPedidoMesa', N'1', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'toleranciaPagamento', N'0', NULL)
GO
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor], [Descricao]) VALUES (N'ValidarPedidoModificacaoInvalido', N'0', NULL)
GO
SET IDENTITY_INSERT [dbo].[tbEstado] ON 

GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (1, N'Acre', N'AC')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (2, N'Alagoas', N'AL')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (3, N'Amapá', N'AP')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (4, N'Amazonas', N'AM')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (5, N'Bahia', N'BA')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (6, N'Ceará', N'CE')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (7, N'Distrito Federal', N'DF')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (8, N'Espírito Santo', N'ES')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (9, N'Goiás', N'GO')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (10, N'Maranhão', N'MA')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (11, N'Mato Grosso', N'MT')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (12, N'Mato Grosso do Sul', N'MS')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (13, N'Minas Gerais', N'MG')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (14, N'Pará', N'PA')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (15, N'Paraíba', N'PB')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (16, N'Paraná', N'PR')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (17, N'Pernambuco', N'PE')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (18, N'Piauí', N'PI')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (19, N'Rio de Janeiro', N'RJ')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (20, N'Rio Grande do Norte', N'RN')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (21, N'Rio Grande do Sul', N'RS')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (22, N'Rondônia', N'RO')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (23, N'Roraima', N'RR')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (24, N'Santa Catarina', N'SC')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (25, N'São Paulo', N'SP')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (26, N'Sergipe', N'SE')
GO
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (27, N'Tocantins', N'TO')
GO
SET IDENTITY_INSERT [dbo].[tbEstado] OFF
GO
SET IDENTITY_INSERT [dbo].[tbIdioma] ON 

GO
INSERT [dbo].[tbIdioma] ([IDIdioma], [Nome], [Codigo]) VALUES (1, N'Português', N'pt_BR')
GO
INSERT [dbo].[tbIdioma] ([IDIdioma], [Nome], [Codigo]) VALUES (2, N'English', N'en_US')
GO
INSERT [dbo].[tbIdioma] ([IDIdioma], [Nome], [Codigo]) VALUES (3, N'Español', N'es_ES')
GO
SET IDENTITY_INSERT [dbo].[tbIdioma] OFF
GO
SET IDENTITY_INSERT [dbo].[tbMeioPagamentoSAT] ON 

GO
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (1, N'01', N'Dinheiro')
GO
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (2, N'02', N'Cheque')
GO
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (3, N'03', N'Cartão de Crédito')
GO
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (4, N'04', N'Cartão de Débito')
GO
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (5, N'05', N'Crédito Loja')
GO
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (6, N'10', N'Vale Alimentação')
GO
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (7, N'11', N'Vale Refeição')
GO
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (8, N'12', N'Vale Presente')
GO
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (9, N'13', N'Vale Combustível')
GO
INSERT [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT], [Codigo], [Descricao]) VALUES (10, N'99', N'Outros')
GO
SET IDENTITY_INSERT [dbo].[tbMeioPagamentoSAT] OFF
GO
SET IDENTITY_INSERT [dbo].[tbMotivoCancelamento] ON 

GO
INSERT [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento], [Nome]) VALUES (1, N'Teste')
GO
INSERT [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento], [Nome]) VALUES (2, N'Lançamento em comanda errada')
GO
INSERT [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento], [Nome]) VALUES (3, N'Lançamento de produto errado')
GO
INSERT [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento], [Nome]) VALUES (4, N'Cliente diz que não consumiu')
GO
INSERT [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento], [Nome]) VALUES (5, N'Outro motivo')
GO
SET IDENTITY_INSERT [dbo].[tbMotivoCancelamento] OFF
GO
SET IDENTITY_INSERT [dbo].[tbPainelModificacaoOperacao] ON 

GO
INSERT [dbo].[tbPainelModificacaoOperacao] ([IDPainelModificacaoOperacao], [Nome]) VALUES (1, N'Soma')
GO
INSERT [dbo].[tbPainelModificacaoOperacao] ([IDPainelModificacaoOperacao], [Nome]) VALUES (2, N'Maior Valor')
GO
INSERT [dbo].[tbPainelModificacaoOperacao] ([IDPainelModificacaoOperacao], [Nome]) VALUES (3, N'Média')
GO
SET IDENTITY_INSERT [dbo].[tbPainelModificacaoOperacao] OFF
GO
SET IDENTITY_INSERT [dbo].[tbProduto] ON 

GO
INSERT [dbo].[tbProduto] ([IDProduto], [IDTipoProduto], [Codigo], [CodigoERP], [Nome], [Descricao], [ValorUnitario], [ValorAliquota], [CodigoAliquota], [Quantidade], [Ativo], [Disponibilidade], [DtAlteracaoDisponibilidade], [DtUltimaAlteracao], [Excluido], [IDClassificacaoFiscal], [IDUnidade], [cEAN], [ControlarEstoque], [UtilizarBalanca], [CodigoIntegracao1], [IDIntegracao1], [DescricaoIntegracao1], [CodigoIntegracao2], [IDIntegracao2], [DescricaoIntegracao2]) VALUES (1, 10, N'pnc', NULL, N'Produto não cadastrado', NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'II', NULL, 1, 1, GETDATE(), GETDATE(), 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbProduto] ([IDProduto], [IDTipoProduto], [Codigo], [CodigoERP], [Nome], [Descricao], [ValorUnitario], [ValorAliquota], [CodigoAliquota], [Quantidade], [Ativo], [Disponibilidade], [DtAlteracaoDisponibilidade], [DtUltimaAlteracao], [Excluido], [IDClassificacaoFiscal], [IDUnidade], [cEAN], [ControlarEstoque], [UtilizarBalanca], [CodigoIntegracao1], [IDIntegracao1], [DescricaoIntegracao1], [CodigoIntegracao2], [IDIntegracao2], [DescricaoIntegracao2]) VALUES (2, 30, NULL, NULL, N' * Entrada', NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'II', NULL, 1, 0, GETDATE(), GETDATE(), 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbProduto] ([IDProduto], [IDTipoProduto], [Codigo], [CodigoERP], [Nome], [Descricao], [ValorUnitario], [ValorAliquota], [CodigoAliquota], [Quantidade], [Ativo], [Disponibilidade], [DtAlteracaoDisponibilidade], [DtUltimaAlteracao], [Excluido], [IDClassificacaoFiscal], [IDUnidade], [cEAN], [ControlarEstoque], [UtilizarBalanca], [CodigoIntegracao1], [IDIntegracao1], [DescricaoIntegracao1], [CodigoIntegracao2], [IDIntegracao2], [DescricaoIntegracao2]) VALUES (3, 30, NULL, NULL, N' * Entrada (CM)', NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'II', NULL, 1, 0, GETDATE(), GETDATE(), 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbProduto] ([IDProduto], [IDTipoProduto], [Codigo], [CodigoERP], [Nome], [Descricao], [ValorUnitario], [ValorAliquota], [CodigoAliquota], [Quantidade], [Ativo], [Disponibilidade], [DtAlteracaoDisponibilidade], [DtUltimaAlteracao], [Excluido], [IDClassificacaoFiscal], [IDUnidade], [cEAN], [ControlarEstoque], [UtilizarBalanca], [CodigoIntegracao1], [IDIntegracao1], [DescricaoIntegracao1], [CodigoIntegracao2], [IDIntegracao2], [DescricaoIntegracao2]) VALUES (4, 30, NULL, NULL, N' * Serviço', NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'II', NULL, 1, 0, GETDATE(), GETDATE(), 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tbProduto] OFF
GO
SET IDENTITY_INSERT [dbo].[tbRelatorio] ON 

GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (1, 1, N'Resumo dos Fechamentos', NULL, N'', 1, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (2, 1, N'Resumo por Sexo', NULL, N'SELECT
       case cl.Sexo
        when ''m'' then ''Homem''
        when ''f'' then ''Mulher'' END ''Tipo''
      ,COUNT(DISTINCT(p.idPedido)) as ''Qtd.''
      ,(SELECT
          SUM(ValorUnitario*Quantidade)
        FROM
          tbPedidoProduto pp1
          INNER JOIN tbPedido p1 ON p1.idPedido=pp1.idPedido
          INNER JOIN tbCaixa c1 ON c1.IDCaixa=p1.IDCaixa
          INNER JOIN tbCliente cl1 ON cl1.idCliente=p1.idCliente
        WHERE pp1.Cancelado=0 AND c1.idFechamento=@idFechamento AND cl1.Sexo=cl.Sexo AND pp1.idProduto NOT IN (2, 3)) ''Valor produto (R$)''
      ,(SELECT
          SUM(ValorUnitario*Quantidade)
        FROM
          tbPedidoProduto pp1
          INNER JOIN tbPedido p1 ON p1.idPedido=pp1.idPedido
          INNER JOIN tbCaixa c1 ON c1.IDCaixa=p1.IDCaixa
          INNER JOIN tbCliente cl1 ON cl1.idCliente=p1.idCliente
        WHERE pp1.Cancelado=0 AND c1.idFechamento=@idFechamento AND cl1.Sexo=cl.Sexo AND pp1.idProduto IN (2, 3)) ''Valor entrada (R$)''
      ,SUM(p.ValorServico) ''Valor serviço (R$)''
      ,SUM(p.ValorDesconto) ''Valor desconto (R$)''
      ,((SELECT
          SUM(ValorUnitario*Quantidade)
        FROM
          tbPedidoProduto pp1
          INNER JOIN tbPedido p1 ON p1.idPedido=pp1.idPedido
          INNER JOIN tbCaixa c1 ON c1.IDCaixa=p1.IDCaixa
          INNER JOIN tbCliente cl1 ON cl1.idCliente=p1.idCliente
        WHERE pp1.Cancelado=0 AND c1.idFechamento=@idFechamento AND cl1.Sexo=cl.Sexo) + SUM(p.ValorServico) - SUM(p.ValorDesconto)) ''Valor total (R$)''
    FROM
      tbPedido p (NOLOCK)
      LEFT JOIN tbCaixa c (NOLOCK) ON c.IDCaixa=p.IDCaixa
      LEFT JOIN tbCliente cl (NOLOCK) ON cl.idCliente=p.idCliente
    WHERE
      c.idFechamento=@idFechamento
      AND
      Sexo IS NOT NULL
    GROUP BY
      cl.Sexo', 20, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (4, 1, N'Resumo por Tipo Pagamento', NULL, N'SELECT
   tp.Nome ''Tipo pagamento''
    ,COUNT(DISTINCT(pp.IDTipoPagamento)) as ''Qtd.''
  ,SUM(pp.Valor) ''Valor total (R$)''
FROM
  tbCaixa c
  LEFT JOIN tbPedido p ON p.idCaixa=c.idCaixa
  LEFT JOIN tbPedidoPagamento pp ON pp.idPedido=p.idPedido
  LEFT JOIN tbTipoPagamento tp ON tp.idTipoPagamento=pp.idTipoPagamento
WHERE
  idFechamento=@idFechamento
  AND
  pp.idTipoPagamento IS NOT NULL
GROUP BY
  tp.Nome', 8, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (5, 1, N'Pedidos com Desconto', NULL, N'SELECT
   p.IDPedido as ''Cód. Pedido''
  ,u.Nome as ''Usuário fechamento''
  ,cl.NomeCompleto as ''Cliente''
  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto NOT IN (2, 3)), 0) as ''Valor produto (R$)''
  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto IN (2, 3)), 0) as ''Valor entrada (R$)''
  ,ISNULL(p.ValorServico, 0) as ''Valor serviço (R$)''
  ,ISNULL(p.ValorDesconto, 0) as ''Valor desconto (R$)''
  ,(ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido), 0) + ISNULL(p.ValorServico, 0) - ISNULL(p.ValorDesconto, 0)) as ''Valor total (R$)''
  ,p.Observacoes
FROM
  tbPedido p
  INNER JOIN tbCaixa ca ON ca.idCaixa=p.idCaixa
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u ON u.idUsuario=ca.idUsuario
WHERE
  ca.idFechamento=@idFechamento
  AND
  p.ValorDesconto>0', 30, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (6, 1, N'Lista de Pedidos', NULL, N'SELECT
   p.IDPedido as ''Cód. Pedido''
  ,u.Nome as ''Usuário fechamento''
  ,cl.NomeCompleto as ''Cliente''
  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto NOT IN (2, 3)), 0) as ''Valor produto (R$)''
  ,ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido AND idProduto IN (2, 3)), 0) as ''Valor entrada (R$)''
  ,ISNULL(p.ValorServico, 0) as ''Valor serviço (R$)''
  ,ISNULL(p.ValorDesconto, 0) as ''Valor desconto (R$)''
  ,(ISNULL((SELECT SUM(ValorUnitario*Quantidade) FROM tbPedidoProduto WHERE idPedido=p.idPedido), 0) + ISNULL(p.ValorServico, 0) - ISNULL(p.ValorDesconto, 0)) as ''Valor total (R$)''
  ,p.Observacoes
FROM
  tbPedido p
  INNER JOIN tbCaixa ca ON ca.idCaixa=p.idCaixa
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u ON u.idUsuario=ca.idUsuario
WHERE
  ca.idFechamento=@idFechamento', 40, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (7, 1, N'Produtos Cancelados', NULL, N'SELECT
   p.idPedido as ''Cód. Pedido''
  ,cl.NomeCompleto as ''Cliente''
  ,p1.Nome as ''PDV lançamento''
  ,u1.Nome as ''Usuário lançamento''
  ,pp.DtInclusao as ''Data lançamento''
  ,p2.Nome as ''PDV cancelamento''
  ,u2.Nome as ''Usuário cancelamento''
  ,pp.DtCancelamento ''Data cancelamento''
  ,pr.Nome as ''Produto''
  ,pp.Quantidade as ''Qtd.''
  ,pp.ValorUnitario as ''Valor unitário (R$)''
    ,(pp.ValorUnitario*pp.Quantidade) as ''Valor total (R$)''
  ,mc.Nome as ''Motivo Cancelamento''
FROM
  tbPedidoProduto pp
  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario
  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV
  LEFT JOIN tbUsuario u2 ON u2.idUsuario=pp.idUsuario_cancelamento
  LEFT JOIN tbPDV p2 ON p2.idPDV=pp.idPDV_cancelamento
  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
  LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
  LEFT JOIN tbMotivoCancelamento mc ON mc.IDMotivoCancelamento=pp.IDMotivoCancelamento
WHERE
  c.idFechamento=@idFechamento
  AND
  pp.Cancelado=1', 50, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (8, 1, N'Produtos Cancelados - Resumo por Usuário Lançamento', NULL, N'SELECT
     p1.Nome as ''PDV lançamento''
  ,u1.Nome as ''Usuário lançamento''
  ,SUM(pp.Quantidade) as ''Qtd.''
  ,SUM(pp.ValorUnitario*pp.Quantidade) as ''Valor total (R$)''
FROM
  tbPedidoProduto pp
  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario
  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV
  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
  LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
WHERE
  c.idFechamento=@idFechamento
  AND
  pp.Cancelado=1
GROUP BY
     p1.Nome
  ,u1.Nome', 60, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (9, 1, N'Produtos Cancelados - Resumo por Usuário Cancelamento', NULL, N'SELECT
     p1.Nome as ''PDV Cancelamento''
  ,u1.Nome as ''Usuário Cancelamento''
  ,SUM(pp.Quantidade) as ''Qtd.''
  ,SUM(pp.ValorUnitario*pp.Quantidade) as ''Valor total (R$)''
FROM
  tbPedidoProduto pp
  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario_cancelamento
  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV_cancelamento
  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
  LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
WHERE
  c.idFechamento=@idFechamento
  AND
  pp.Cancelado=1
GROUP BY
     p1.Nome
  ,u1.Nome', 70, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (10, 1, N'Produtos Vendidos', NULL, N'SELECT
   p.idPedido as ''Cód. Pedido''
  ,cl.NomeCompleto as ''Cliente''
  ,p1.Nome as ''PDV lançamento''
  ,u1.Nome as ''Usuário lançamento''
  ,pp.DtInclusao as ''Data lançamento''
  ,(SELECT TOP 1 c.Nome
    FROM tbProdutoCategoriaProduto pcp
    INNER JOIN tbCategoriaProduto c on c.IDCategoriaProduto = pcp.IDCategoriaProduto
    AND pcp.IDProduto = pp.IDProduto) as ''Categoria''
  ,pr.Nome as ''Produto''
  ,pp.Quantidade as ''Qtd.''
  ,pp.ValorUnitario as ''Valor unitário (R$)''
    ,(pp.ValorUnitario*pp.Quantidade) as ''Valor total (R$)''
FROM
  tbPedidoProduto pp
  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario
  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV
  LEFT JOIN tbUsuario u2 ON u2.idUsuario=pp.idUsuario_cancelamento
  LEFT JOIN tbPDV p2 ON p2.idPDV=pp.idPDV_cancelamento
  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
  LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
WHERE
  c.idFechamento=@idFechamento
  AND
  pp.Cancelado=0', 80, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (11, 1, N'Produtos Vendidos - Resumo', NULL, N'
SELECT
   (SELECT TOP 1 c.Nome
    FROM tbProdutoCategoriaProduto pcp
    INNER JOIN tbCategoriaProduto c on c.IDCategoriaProduto = pcp.IDCategoriaProduto
    and pcp.IDProduto = pp.IDProduto) as ''Categoria''
  ,pr.Nome as ''Produto''
  ,SUM(pp.Quantidade) as ''Qtd.''
    ,SUM(pp.ValorUnitario*pp.Quantidade) as ''Valor total (R$)''
FROM
  tbPedidoProduto pp
  LEFT JOIN tbPedido p ON p.idPedido=pp.idPedido
  LEFT JOIN tbCliente cl ON cl.idCliente=p.idCliente
  LEFT JOIN tbUsuario u1 ON u1.idUsuario=pp.idUsuario
  LEFT JOIN tbPDV p1 ON p1.idPDV=pp.idPDV
  LEFT JOIN tbUsuario u2 ON u2.idUsuario=pp.idUsuario_cancelamento
  LEFT JOIN tbPDV p2 ON p2.idPDV=pp.idPDV_cancelamento
  LEFT JOIN tbProduto pr ON pr.idProduto=pp.idProduto
  LEFT JOIN tbCaixa c ON c.idCaixa=p.idCaixa
WHERE
  c.idFechamento=@idFechamento
  AND
  pp.Cancelado=0
GROUP BY pp.IDProduto, pr.Nome
', 90, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (12, 2, N'Lista de Clientes', NULL, N'SELECT
   NomeCompleto
  ,(''('' + CAST(ISNULL(Telefone1DDD, 11) as VARCHAR(2)) + '') '' + CAST(Telefone1Numero as VARCHAR(9))) as ''Telefone''
  ,(CAST(DAY(DataNascimento) as VARCHAR(2)) + ''/'' + CAST(MONTH(DataNascimento) as VARCHAR(2))) as ''Aniversário''
  ,case c.Sexo
    when ''m'' then ''Homem''
    when ''f'' then ''Mulher'' END ''Tipo''
  ,Documento1 as ''CPF/CNPJ''
  ,Endereco + '', '' + EnderecoNumero as ''Endereço''
  ,Bairro
  ,Cidade
  ,e.Nome as UF
  ,c.CEP
  ,DtInclusao as ''Data cadastro''
FROM
  tbCliente c (NOLOCK)
  LEFT JOIN tbEstado e (NOLOCK) ON e.idEstado=c.idEstado
WHERE
  DtInclusao BETWEEN @dtInicio AND @dtFim
ORDER BY
  NomeCompleto', 10, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (13, 2, N'Produtos Vendidos', NULL, N'SELECT
   (SELECT TOP 1 c.Nome
    FROM tbProdutoCategoriaProduto pcp
    INNER JOIN tbCategoriaProduto c on c.IDCategoriaProduto = pcp.IDCategoriaProduto
    AND pcp.IDProduto = p.IDProduto) as ''Categoria''
  ,p.Nome as ''Item''
  ,pp.Quantidade as ''Qtd.''
  ,pp.ValorUnitario as ''Valor unit.''
  ,CAST((pp.ValorUnitario*pp.Quantidade) as DECIMAL(18, 2)) ''Valor total''
  ,pp.DtInclusao as ''Data''
  ,u.Nome as ''Vendedor''
FROM
  tbPedidoProduto pp
  INNER JOIN tbProduto p ON p.IDProduto=pp.IDProduto
  INNER JOIN tbUsuario u ON u.IDUsuario=pp.IDUsuario
WHERE
  pp.Cancelado=0
  AND
  DtInclusao BETWEEN @dtInicio AND @dtFim', 20, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (14, 2, N'Produtos Vendidos - Resumo', NULL, N'SELECT
   (SELECT TOP 1 c.Nome
    FROM tbProdutoCategoriaProduto pcp
    INNER JOIN tbCategoriaProduto c on c.IDCategoriaProduto = pcp.IDCategoriaProduto
    AND pcp.IDProduto = p.IDProduto) as ''Categoria''
  ,p.Nome as ''Item''
  ,SUM(pp.Quantidade) as ''Qtd.''
  ,CAST(SUM(pp.ValorUnitario*pp.Quantidade) as DECIMAL(18, 2)) ''Valor total''
FROM
  tbPedidoProduto pp
  INNER JOIN tbProduto p ON p.IDProduto=pp.IDProduto
  INNER JOIN tbUsuario u ON u.IDUsuario=pp.IDUsuario
WHERE
  pp.Cancelado=0
  AND
  DtInclusao BETWEEN @dtInicio AND @dtFim
GROUP BY
  p.IDProduto
  , p.Nome
ORDER BY
  p.Nome', 30, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (15, 1, N'Resumo por Tipo de Entrada', NULL, N'SELECT
   te.Nome as ''Tipo''
  ,COUNT(DISTINCT(p.idPedido)) as ''Qtd.''
FROM
  tbPedido p (NOLOCK)
  LEFT JOIN tbCaixa c (NOLOCK) ON c.IDCaixa=p.IDCaixa
  LEFT JOIN tbTipoEntrada te (NOLOCK) ON te.idTipoEntrada=p.idTipoEntrada
WHERE
  c.idFechamento=@idFechamento
  AND
  te.idTipoEntrada IS NOT NULL
GROUP BY
   te.idTipoEntrada
  ,te.Nome', 10, NULL)
GO
INSERT [dbo].[tbRelatorio] ([IDRelatorio], [IDTipoRelatorio], [Nome], [Descricao], [QuerySQL], [Ordem], [Totalizador]) VALUES (16, 2, N'Produtos Vendidos por Cliente', NULL, N'select
    isnull(tbCliente.NomeCompleto, ''Sem Cliente'') as ''Cliente''
  , tbProduto.Codigo as ''Codigo Produto''
  , tbProduto.Nome as ''Produto''
  , sum(tbPedidoProduto.Quantidade) as Quantidade
  , tbPedidoProduto.ValorUnitario
  , cast(sum(tbPedidoproduto.Quantidade * tbPedidoProduto.ValorUnitario) as decimal(10, 2)) as ''Valor''
  , cast(tbPedido.DtPedidoFechamento as date) as ''Data Fechamento Pedido''
from tbPedidoProduto (nolock)
inner join tbProduto (nolock) on tbProduto.IDProduto = tbPedidoProduto.IDProduto
                and tbProduto.IDTipoProduto in (10, 20)
inner join tbPedido (nolock) on tbPedido.IDPedido = tbPedidoProduto.IDPedido
                and tbPedido.IDStatusPedido in (40)
left join tbCliente (nolock) on tbCliente.IDCliente = tbPedido.IDCliente
where
  tbPedidoProduto.Cancelado = 0
  and (tbProduto.IDTipoProduto = 10 or tbProduto.IDTipoProduto = 20 and tbPedidoProduto.ValorUnitario > 0)
group by
    isnull(tbCliente.NomeCompleto, ''Sem Cliente'')
  , tbProduto.Codigo
  , tbProduto.Nome
  , tbPedidoProduto.ValorUnitario
  , cast(tbPedido.DtPedidoFechamento as date)
order by
  cast(tbPedido.DtPedidoFechamento as date)
  , isnull(tbCliente.NomeCompleto, ''Sem Cliente'')', 40, NULL)
GO
SET IDENTITY_INSERT [dbo].[tbRelatorio] OFF
GO
INSERT [dbo].[tbStatusComanda] ([IDStatusComanda], [Nome]) VALUES (10, N'Liberada')
GO
INSERT [dbo].[tbStatusComanda] ([IDStatusComanda], [Nome]) VALUES (20, N'Em uso')
GO
INSERT [dbo].[tbStatusComanda] ([IDStatusComanda], [Nome]) VALUES (30, N'Cancelada')
GO
INSERT [dbo].[tbStatusComanda] ([IDStatusComanda], [Nome]) VALUES (40, N'Perdida')
GO
INSERT [dbo].[tbStatusMesa] ([IDStatusMesa], [Nome]) VALUES (10, N'Liberada')
GO
INSERT [dbo].[tbStatusMesa] ([IDStatusMesa], [Nome]) VALUES (20, N'Em atendimento')
GO
INSERT [dbo].[tbStatusMesa] ([IDStatusMesa], [Nome]) VALUES (30, N'Conta solicitada')
GO
INSERT [dbo].[tbStatusMesa] ([IDStatusMesa], [Nome]) VALUES (40, N'Reservada')
GO
INSERT [dbo].[tbStatusPedido] ([IDStatusPedido], [Nome]) VALUES (10, N'Aberto')
GO
INSERT [dbo].[tbStatusPedido] ([IDStatusPedido], [Nome]) VALUES (20, N'Enviado')
GO
INSERT [dbo].[tbStatusPedido] ([IDStatusPedido], [Nome]) VALUES (40, N'Finalizado')
GO
INSERT [dbo].[tbStatusPedido] ([IDStatusPedido], [Nome]) VALUES (50, N'Cancelado')
GO
INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (10, N'Não Iniciado')
GO
INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (20, N'Processando')
GO
INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (30, N'Sucesso')
GO
INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (40, N'Abortado')
GO
INSERT [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT], [Nome]) VALUES (50, N'Erro')
GO
INSERT [dbo].[tbTipoAreaImpressao] ([IDTipoAreaImpressao], [Nome]) VALUES (0, N'Conta')
GO
INSERT [dbo].[tbTipoAreaImpressao] ([IDTipoAreaImpressao], [Nome]) VALUES (1, N'Conta Padrão')
GO
INSERT [dbo].[tbTipoAreaImpressao] ([IDTipoAreaImpressao], [Nome]) VALUES (2, N'Produção')
GO
INSERT [dbo].[tbTipoAreaImpressao] ([IDTipoAreaImpressao], [Nome]) VALUES (3, N'S@T')
GO
SET IDENTITY_INSERT [dbo].[tbTipoEntrada] ON 

GO
INSERT [dbo].[tbTipoEntrada] ([IDTipoEntrada], [Nome], [ValorEntrada], [ValorConsumacaoMinima], [Ativo], [Padrao]) VALUES (1, N'VIP', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[tbTipoEntrada] OFF
GO
SET IDENTITY_INSERT [dbo].[tbTipoMovimentacao] ON 

GO
INSERT [dbo].[tbTipoMovimentacao] ([IDTipoMovimentacao], [Tipo], [Nome], [Descricao], [Excluido]) VALUES (1, N'+', N'Recebimento de Mercadoria', N'', 0)
GO
INSERT [dbo].[tbTipoMovimentacao] ([IDTipoMovimentacao], [Tipo], [Nome], [Descricao], [Excluido]) VALUES (2, N'-', N'Entrega de Mercadoria', N'', 0)
GO
SET IDENTITY_INSERT [dbo].[tbTipoMovimentacao] OFF
GO
SET IDENTITY_INSERT [dbo].[tbTipoPagamento] ON 

GO
INSERT [dbo].[tbTipoPagamento] ([IDTipoPagamento], [CodigoImpressoraFiscal], [CodigoERP], [Nome], [RegistrarValores], [PrazoCredito], [Ativo], [IDMeioPagamentoSAT]) VALUES (1, N'01', NULL, N'Dinheiro', 1, 0, 1, NULL)
GO
INSERT [dbo].[tbTipoPagamento] ([IDTipoPagamento], [CodigoImpressoraFiscal], [CodigoERP], [Nome], [RegistrarValores], [PrazoCredito], [Ativo], [IDMeioPagamentoSAT]) VALUES (2, N'02', NULL, N'Cartão Crédito', 1, 0, 1, NULL)
GO
INSERT [dbo].[tbTipoPagamento] ([IDTipoPagamento], [CodigoImpressoraFiscal], [CodigoERP], [Nome], [RegistrarValores], [PrazoCredito], [Ativo], [IDMeioPagamentoSAT]) VALUES (3, N'03', NULL, N'Cartão Débito', 1, 0, 1, NULL)
GO
INSERT [dbo].[tbTipoPagamento] ([IDTipoPagamento], [CodigoImpressoraFiscal], [CodigoERP], [Nome], [RegistrarValores], [PrazoCredito], [Ativo], [IDMeioPagamentoSAT]) VALUES (4, N'04', NULL, N'Outros cartões', 1, 0, 1, NULL)
GO
INSERT [dbo].[tbTipoPagamento] ([IDTipoPagamento], [CodigoImpressoraFiscal], [CodigoERP], [Nome], [RegistrarValores], [PrazoCredito], [Ativo], [IDMeioPagamentoSAT]) VALUES (5, N'05', NULL, N'Conta assinada', 1, 0, 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[tbTipoPagamento] OFF
GO
INSERT [dbo].[tbTipoPDV] ([IDTipoPDV], [Nome]) VALUES (1, N'BackOffice')
GO
INSERT [dbo].[tbTipoPDV] ([IDTipoPDV], [Nome]) VALUES (10, N'Caixa')
GO
INSERT [dbo].[tbTipoPDV] ([IDTipoPDV], [Nome]) VALUES (20, N'Terminal Android')
GO
INSERT [dbo].[tbTipoPDV] ([IDTipoPDV], [Nome]) VALUES (30, N'Terminal Windows')
GO
INSERT [dbo].[tbTipoPDV] ([IDTipoPDV], [Nome]) VALUES (40, N'Comanda Eletrônica')
GO
INSERT [dbo].[tbTipoPDV] ([IDTipoPDV], [Nome]) VALUES (50, N'Controle Saída')
GO
INSERT [dbo].[tbTipoPDV] ([IDTipoPDV], [Nome]) VALUES (60, N'Cardápio Digital')
GO
INSERT [dbo].[tbTipoPDV] ([IDTipoPDV], [Nome]) VALUES (70, N'Controle de Estoque')
GO
INSERT [dbo].[tbTipoPedido] ([IDTipoPedido], [Nome]) VALUES (10, N'Mesa')
GO
INSERT [dbo].[tbTipoPedido] ([IDTipoPedido], [Nome]) VALUES (20, N'Comanda')
GO
INSERT [dbo].[tbTipoPedido] ([IDTipoPedido], [Nome]) VALUES (30, N'Delivery')
GO
INSERT [dbo].[tbTipoPedido] ([IDTipoPedido], [Nome]) VALUES (40, N'Balcão')
GO
INSERT [dbo].[tbTipoProduto] ([IDTipoProduto], [Nome]) VALUES (10, N'Item')
GO
INSERT [dbo].[tbTipoProduto] ([IDTipoProduto], [Nome]) VALUES (20, N'Modificação')
GO
INSERT [dbo].[tbTipoProduto] ([IDTipoProduto], [Nome]) VALUES (30, N'Serviço')
GO
INSERT [dbo].[tbTipoProduto] ([IDTipoProduto], [Nome]) VALUES (40, N'Ingrediente')
GO
INSERT [dbo].[tbTipoRelatorio] ([IDTipoRelatorio], [Nome]) VALUES (1, N'Fechamento')
GO
INSERT [dbo].[tbTipoRelatorio] ([IDTipoRelatorio], [Nome]) VALUES (2, N'Geral')
GO
INSERT [dbo].[tbTipoSolicitacaoSAT] ([IDTipoSolicitacaoSAT], [Nome]) VALUES (1, N'EnviarDadosVenda')
GO
INSERT [dbo].[tbTipoSolicitacaoSAT] ([IDTipoSolicitacaoSAT], [Nome]) VALUES (2, N'CancelarUltimaVenda')
GO
SET IDENTITY_INSERT [dbo].[tbTipoTributacao] ON 

GO
INSERT [dbo].[tbTipoTributacao] ([IDTipoTributacao], [Nome], [Descricao], [CFOP], [ICMS00_Orig], [ICMS00_CST], [ICMS00_pICMS], [ICMS40_Orig], [ICMS40_CST], [ICMSSN102_Orig], [ICMSSN102_CSOSN], [ICMSSN900_Orig], [ICMSSN900_CSOSN], [ICMSSN900_pICMS], [PISAliq_CST], [PISAliq_pPIS], [PISQtde_CST], [PISQtde_vAliqProd], [PISNT_CST], [PISSN_CST], [PISOutr_CST], [PISOutr_pPIS], [PISOutr_vAliqProd], [PISST_pPIS], [PISST_vAliqProd], [COFINSAliq_CST], [COFINSAliq_pCOFINS], [COFINSQtde_CST], [COFINSQtde_vAliqProd], [COFINSNT_CST], [COFINSSN_CST], [COFINSOutr_CST], [COFINSOutr_pCOFINS], [COFINSOutr_vAliqProd], [COFINSST_pCOFINS], [COFINSST_vAliqProd], [ISSQN_vDeducISSQN], [ISSQN_vAliq], [ISSQN_cListServ], [ISSQN_cServTribMun], [ISSQN_cNatOp], [ISSQN_indIncFisc], [vItem12741]) VALUES (1, N'Simples   ', NULL, N'5102', NULL, NULL, NULL, NULL, NULL, N'0', N'500', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'49', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'49', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tbTipoTributacao] ([IDTipoTributacao], [Nome], [Descricao], [CFOP], [ICMS00_Orig], [ICMS00_CST], [ICMS00_pICMS], [ICMS40_Orig], [ICMS40_CST], [ICMSSN102_Orig], [ICMSSN102_CSOSN], [ICMSSN900_Orig], [ICMSSN900_CSOSN], [ICMSSN900_pICMS], [PISAliq_CST], [PISAliq_pPIS], [PISQtde_CST], [PISQtde_vAliqProd], [PISNT_CST], [PISSN_CST], [PISOutr_CST], [PISOutr_pPIS], [PISOutr_vAliqProd], [PISST_pPIS], [PISST_vAliqProd], [COFINSAliq_CST], [COFINSAliq_pCOFINS], [COFINSQtde_CST], [COFINSQtde_vAliqProd], [COFINSNT_CST], [COFINSSN_CST], [COFINSOutr_CST], [COFINSOutr_pCOFINS], [COFINSOutr_vAliqProd], [COFINSST_pCOFINS], [COFINSST_vAliqProd], [ISSQN_vDeducISSQN], [ISSQN_vAliq], [ISSQN_cListServ], [ISSQN_cServTribMun], [ISSQN_cNatOp], [ISSQN_indIncFisc], [vItem12741]) VALUES (2, N'Simples ST', NULL, N'5405', NULL, NULL, NULL, NULL, NULL, N'0', N'102', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'49', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'49', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tbTipoTributacao] OFF
GO
SET IDENTITY_INSERT [dbo].[tbUnidade] ON 

GO
INSERT [dbo].[tbUnidade] ([IDUnidade], [Nome], [Simbolo], [Excluido]) VALUES (1, N'Unidade', N'Un', 0)
GO
SET IDENTITY_INSERT [dbo].[tbUnidade] OFF
GO
SET IDENTITY_INSERT [dbo].[tbUsuario] ON 

GO
INSERT [dbo].[tbUsuario] ([IDUsuario], [CodigoEMENU], [CodigoERP], [Nome], [Login], [Senha], [PermissaoAdm], [PermissaoCaixa], [PermissaoGarcom], [PermissaoGerente], [Ativo], [DtUltimaAlteracao], [Excluido]) VALUES (1, NULL, NULL, N'Administrador', NULL, N'2010', 1, 1, 1, 1, 1, CAST(N'2015-06-01T13:23:05.477' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[tbUsuario] OFF
GO
ALTER TABLE [dbo].[tbAreaImpressao] ADD  CONSTRAINT [DF_tbAreaImpressao_IDTipoAreaImpressao]  DEFAULT ((2)) FOR [IDTipoAreaImpressao]
GO
ALTER TABLE [dbo].[tbEntregador] ADD  CONSTRAINT [DF_tbEntregador_Ativo]  DEFAULT ((1)) FOR [Ativo]
GO
ALTER TABLE [dbo].[tbEntregador] ADD  CONSTRAINT [DF_tbEntregador_Excluido]  DEFAULT ((0)) FOR [Excluido]
GO
ALTER TABLE [dbo].[tbInventario] ADD  CONSTRAINT [DF_tbInventario_Processado]  DEFAULT ((0)) FOR [Processado]
GO
ALTER TABLE [dbo].[tbInventario] ADD  CONSTRAINT [DF_tbInventario_Excluido]  DEFAULT ((0)) FOR [Excluido]
GO
ALTER TABLE [dbo].[tbMovimentacao] ADD  CONSTRAINT [DF_tbMovimentacao_Excluido]  DEFAULT ((0)) FOR [Excluido]
GO
ALTER TABLE [dbo].[tbMovimentacao] ADD  CONSTRAINT [DF_tbMovimentacao_Reversa]  DEFAULT ((0)) FOR [Reversa]
GO
ALTER TABLE [dbo].[tbMovimentacao] ADD  CONSTRAINT [DF_tbMovimentacao_Processado]  DEFAULT ((0)) FOR [Processado]
GO
ALTER TABLE [dbo].[tbOrdemImpressao] ADD  CONSTRAINT [DF_tbOrdemImpressao_Conta]  DEFAULT ((0)) FOR [Conta]
GO
ALTER TABLE [dbo].[tbOrdemImpressao] ADD  CONSTRAINT [DF_tbOrdemImpressao_SAT]  DEFAULT ((0)) FOR [SAT]
GO
ALTER TABLE [dbo].[tbPainelModificacao] ADD  CONSTRAINT [DF_tbPainelModificacao_IDPainelModificacaoOperacao]  DEFAULT ((1)) FOR [IDPainelModificacaoOperacao]
GO
ALTER TABLE [dbo].[tbPedido] ADD  CONSTRAINT [DF_tbPedido_EnviadoSAT]  DEFAULT ((0)) FOR [IDRetornoSAT_venda]
GO
ALTER TABLE [dbo].[tbPedidoProduto] ADD  CONSTRAINT [DF_tbPedidoProduto_RetornarAoEstoque]  DEFAULT ((0)) FOR [RetornarAoEstoque]
GO
ALTER TABLE [dbo].[tbProduto] ADD  CONSTRAINT [DF_tbProduto_Excluido]  DEFAULT ((0)) FOR [Excluido]
GO
ALTER TABLE [dbo].[tbProduto] ADD  CONSTRAINT [DF_tbProduto_ControlarEstoque]  DEFAULT ((0)) FOR [ControlarEstoque]
GO
ALTER TABLE [dbo].[tbProduto] ADD  CONSTRAINT [DF_tbProduto_UtilizarBalanca]  DEFAULT ((0)) FOR [UtilizarBalanca]
GO
ALTER TABLE [dbo].[tbTaxaEntrega] ADD  CONSTRAINT [DF_tbTaxaEntrega_Ativo]  DEFAULT ((1)) FOR [Ativo]
GO
ALTER TABLE [dbo].[tbTaxaEntrega] ADD  CONSTRAINT [DF_tbTaxaEntrega_Excluido]  DEFAULT ((0)) FOR [Excluido]
GO
ALTER TABLE [dbo].[tbTipoDesconto] ADD  CONSTRAINT [DF_tbTipoDesconto_Ativo]  DEFAULT ((1)) FOR [Ativo]
GO
ALTER TABLE [dbo].[tbTipoDesconto] ADD  CONSTRAINT [DF_tbTipoDesconto_Excluido]  DEFAULT ((0)) FOR [Excluido]
GO
ALTER TABLE [dbo].[tbTipoEntrada] ADD  CONSTRAINT [DF_tbTipoEntrada_Padrao]  DEFAULT ((0)) FOR [Padrao]
GO
ALTER TABLE [dbo].[tbTipoMovimentacao] ADD  CONSTRAINT [DF_tbTipoMovimentacao_Excluido]  DEFAULT ((0)) FOR [Excluido]
GO
ALTER TABLE [dbo].[tbTipoPagamento] ADD  CONSTRAINT [DF_tbTipoPagamento_InformarFechamento]  DEFAULT ((1)) FOR [RegistrarValores]
GO
ALTER TABLE [dbo].[tbUnidade] ADD  CONSTRAINT [DF_tbUnidade_Excluido]  DEFAULT ((0)) FOR [Excluido]
GO
ALTER TABLE [dbo].[tbAcao]  WITH CHECK ADD  CONSTRAINT [FK_tbAcao_tbPDV] FOREIGN KEY([IDPDV])
REFERENCES [dbo].[tbPDV] ([IDPDV])
GO
ALTER TABLE [dbo].[tbAcao] CHECK CONSTRAINT [FK_tbAcao_tbPDV]
GO
ALTER TABLE [dbo].[tbAreaImpressao]  WITH CHECK ADD  CONSTRAINT [FK_tbAreaImpressao_tbTipoAreaImpressao] FOREIGN KEY([IDTipoAreaImpressao])
REFERENCES [dbo].[tbTipoAreaImpressao] ([IDTipoAreaImpressao])
GO
ALTER TABLE [dbo].[tbAreaImpressao] CHECK CONSTRAINT [FK_tbAreaImpressao_tbTipoAreaImpressao]
GO
ALTER TABLE [dbo].[tbCaixa]  WITH CHECK ADD  CONSTRAINT [FK_tbCaixa_tbFechamento] FOREIGN KEY([IDFechamento])
REFERENCES [dbo].[tbFechamento] ([IDFechamento])
GO
ALTER TABLE [dbo].[tbCaixa] CHECK CONSTRAINT [FK_tbCaixa_tbFechamento]
GO
ALTER TABLE [dbo].[tbCaixa]  WITH CHECK ADD  CONSTRAINT [FK_tbCaixa_tbPDV] FOREIGN KEY([IDPDV])
REFERENCES [dbo].[tbPDV] ([IDPDV])
GO
ALTER TABLE [dbo].[tbCaixa] CHECK CONSTRAINT [FK_tbCaixa_tbPDV]
GO
ALTER TABLE [dbo].[tbCaixa]  WITH CHECK ADD  CONSTRAINT [FK_tbCaixa_tbUsuario] FOREIGN KEY([IDUsuario])
REFERENCES [dbo].[tbUsuario] ([IDUsuario])
GO
ALTER TABLE [dbo].[tbCaixa] CHECK CONSTRAINT [FK_tbCaixa_tbUsuario]
GO
ALTER TABLE [dbo].[tbCaixaAjuste]  WITH CHECK ADD  CONSTRAINT [FK_tbCaixaAjuste_tbCaixa] FOREIGN KEY([IDCaixa])
REFERENCES [dbo].[tbCaixa] ([IDCaixa])
GO
ALTER TABLE [dbo].[tbCaixaAjuste] CHECK CONSTRAINT [FK_tbCaixaAjuste_tbCaixa]
GO
ALTER TABLE [dbo].[tbCaixaAjuste]  WITH CHECK ADD  CONSTRAINT [FK_tbCaixaAjuste_tbCaixaTipoAjuste] FOREIGN KEY([IDCaixaTipoAjuste])
REFERENCES [dbo].[tbCaixaTipoAjuste] ([IDCaixaTipoAjuste])
GO
ALTER TABLE [dbo].[tbCaixaAjuste] CHECK CONSTRAINT [FK_tbCaixaAjuste_tbCaixaTipoAjuste]
GO
ALTER TABLE [dbo].[tbCaixaValorRegistro]  WITH CHECK ADD  CONSTRAINT [FK_tbCaixaValor_tbCaixa] FOREIGN KEY([IDCaixa])
REFERENCES [dbo].[tbCaixa] ([IDCaixa])
GO
ALTER TABLE [dbo].[tbCaixaValorRegistro] CHECK CONSTRAINT [FK_tbCaixaValor_tbCaixa]
GO
ALTER TABLE [dbo].[tbCaixaValorRegistro]  WITH CHECK ADD  CONSTRAINT [FK_tbCaixaValor_tbTipoPagamento] FOREIGN KEY([IDTipoPagamento])
REFERENCES [dbo].[tbTipoPagamento] ([IDTipoPagamento])
GO
ALTER TABLE [dbo].[tbCaixaValorRegistro] CHECK CONSTRAINT [FK_tbCaixaValor_tbTipoPagamento]
GO
ALTER TABLE [dbo].[tbClassificacaoFiscal]  WITH CHECK ADD  CONSTRAINT [FK_tbClassificacaoFiscal_tbTipoTributacao] FOREIGN KEY([IDTipoTributacao])
REFERENCES [dbo].[tbTipoTributacao] ([IDTipoTributacao])
GO
ALTER TABLE [dbo].[tbClassificacaoFiscal] CHECK CONSTRAINT [FK_tbClassificacaoFiscal_tbTipoTributacao]
GO
ALTER TABLE [dbo].[tbCliente]  WITH CHECK ADD  CONSTRAINT [FK_tbCliente_tbEstado] FOREIGN KEY([IDEstado])
REFERENCES [dbo].[tbEstado] ([IDEstado])
GO
ALTER TABLE [dbo].[tbCliente] CHECK CONSTRAINT [FK_tbCliente_tbEstado]
GO
ALTER TABLE [dbo].[tbComanda]  WITH CHECK ADD  CONSTRAINT [FK_tbComanda_tbStatusComanda] FOREIGN KEY([IDStatusComanda])
REFERENCES [dbo].[tbStatusComanda] ([IDStatusComanda])
GO
ALTER TABLE [dbo].[tbComanda] CHECK CONSTRAINT [FK_tbComanda_tbStatusComanda]
GO
ALTER TABLE [dbo].[tbConversaoUnidade]  WITH CHECK ADD  CONSTRAINT [FK_tbConversaoUnidade_tbUnidade] FOREIGN KEY([IDUnidade_de])
REFERENCES [dbo].[tbUnidade] ([IDUnidade])
GO
ALTER TABLE [dbo].[tbConversaoUnidade] CHECK CONSTRAINT [FK_tbConversaoUnidade_tbUnidade]
GO
ALTER TABLE [dbo].[tbConversaoUnidade]  WITH CHECK ADD  CONSTRAINT [FK_tbConversaoUnidade_tbUnidade1] FOREIGN KEY([IDUnidade_para])
REFERENCES [dbo].[tbUnidade] ([IDUnidade])
GO
ALTER TABLE [dbo].[tbConversaoUnidade] CHECK CONSTRAINT [FK_tbConversaoUnidade_tbUnidade1]
GO
ALTER TABLE [dbo].[tbEntradaSaida]  WITH CHECK ADD  CONSTRAINT [FK_tbEntradaSaida_tbProduto] FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbEntradaSaida] CHECK CONSTRAINT [FK_tbEntradaSaida_tbProduto]
GO
ALTER TABLE [dbo].[tbFechamento]  WITH CHECK ADD  CONSTRAINT [FK_tbFechamento_tbPDV] FOREIGN KEY([IDPDV])
REFERENCES [dbo].[tbPDV] ([IDPDV])
GO
ALTER TABLE [dbo].[tbFechamento] CHECK CONSTRAINT [FK_tbFechamento_tbPDV]
GO
ALTER TABLE [dbo].[tbFechamento]  WITH CHECK ADD  CONSTRAINT [FK_tbFechamento_tbUsuario] FOREIGN KEY([IDUsuario])
REFERENCES [dbo].[tbUsuario] ([IDUsuario])
GO
ALTER TABLE [dbo].[tbFechamento] CHECK CONSTRAINT [FK_tbFechamento_tbUsuario]
GO
ALTER TABLE [dbo].[tbImagemTema]  WITH CHECK ADD  CONSTRAINT [FK_tbImagemTema_tbIdioma] FOREIGN KEY([IDIdioma])
REFERENCES [dbo].[tbIdioma] ([IDIdioma])
GO
ALTER TABLE [dbo].[tbImagemTema] CHECK CONSTRAINT [FK_tbImagemTema_tbIdioma]
GO
ALTER TABLE [dbo].[tbImagemTema]  WITH CHECK ADD  CONSTRAINT [FK_tbImagemTema_tbImagem] FOREIGN KEY([IDImagem])
REFERENCES [dbo].[tbImagem] ([IDImagem])
GO
ALTER TABLE [dbo].[tbImagemTema] CHECK CONSTRAINT [FK_tbImagemTema_tbImagem]
GO
ALTER TABLE [dbo].[tbImagemTema]  WITH CHECK ADD  CONSTRAINT [FK_tbImagemTema_tbTemaCardapio] FOREIGN KEY([IDTemaCardapio])
REFERENCES [dbo].[tbTemaCardapio] ([IDTemaCardapio])
GO
ALTER TABLE [dbo].[tbImagemTema] CHECK CONSTRAINT [FK_tbImagemTema_tbTemaCardapio]
GO
ALTER TABLE [dbo].[tbIntegracaoSAT]  WITH CHECK ADD  CONSTRAINT [FK_tbIntegracaoSAT_tbPDV] FOREIGN KEY([IDPDV])
REFERENCES [dbo].[tbPDV] ([IDPDV])
GO
ALTER TABLE [dbo].[tbIntegracaoSAT] CHECK CONSTRAINT [FK_tbIntegracaoSAT_tbPDV]
GO
ALTER TABLE [dbo].[tbInventarioProdutos]  WITH CHECK ADD  CONSTRAINT [FK_tbInventarioProdutos_tbInventario] FOREIGN KEY([IDInventario])
REFERENCES [dbo].[tbInventario] ([IDInventario])
GO
ALTER TABLE [dbo].[tbInventarioProdutos] CHECK CONSTRAINT [FK_tbInventarioProdutos_tbInventario]
GO
ALTER TABLE [dbo].[tbInventarioProdutos]  WITH CHECK ADD  CONSTRAINT [FK_tbInventarioProdutos_tbProduto] FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbInventarioProdutos] CHECK CONSTRAINT [FK_tbInventarioProdutos_tbProduto]
GO
ALTER TABLE [dbo].[tbInventarioProdutos]  WITH CHECK ADD  CONSTRAINT [FK_tbInventarioProdutos_tbUnidade] FOREIGN KEY([IDUnidade])
REFERENCES [dbo].[tbUnidade] ([IDUnidade])
GO
ALTER TABLE [dbo].[tbInventarioProdutos] CHECK CONSTRAINT [FK_tbInventarioProdutos_tbUnidade]
GO
ALTER TABLE [dbo].[tbMapAreaImpressaoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbMapImpressoraProduto_tbAreaImpressao] FOREIGN KEY([IDAreaImpressao])
REFERENCES [dbo].[tbAreaImpressao] ([IDAreaImpressao])
GO
ALTER TABLE [dbo].[tbMapAreaImpressaoProduto] CHECK CONSTRAINT [FK_tbMapImpressoraProduto_tbAreaImpressao]
GO
ALTER TABLE [dbo].[tbMapAreaImpressaoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbMapImpressoraProduto_tbProduto] FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbMapAreaImpressaoProduto] CHECK CONSTRAINT [FK_tbMapImpressoraProduto_tbProduto]
GO
ALTER TABLE [dbo].[tbMesa]  WITH CHECK ADD  CONSTRAINT [FK_tbMesa_tbStatusMesa] FOREIGN KEY([IDStatusMesa])
REFERENCES [dbo].[tbStatusMesa] ([IDStatusMesa])
GO
ALTER TABLE [dbo].[tbMesa] CHECK CONSTRAINT [FK_tbMesa_tbStatusMesa]
GO
ALTER TABLE [dbo].[tbMovimentacao]  WITH CHECK ADD  CONSTRAINT [FK_tbMovimentacao_tbTipoMovimentacao] FOREIGN KEY([IDTipoMovimentacao])
REFERENCES [dbo].[tbTipoMovimentacao] ([IDTipoMovimentacao])
GO
ALTER TABLE [dbo].[tbMovimentacao] CHECK CONSTRAINT [FK_tbMovimentacao_tbTipoMovimentacao]
GO
ALTER TABLE [dbo].[tbMovimentacaoProdutos]  WITH CHECK ADD  CONSTRAINT [FK_tbMovimentacaoProdutos_tbMovimentacao] FOREIGN KEY([IDMovimentacao])
REFERENCES [dbo].[tbMovimentacao] ([IDMovimentacao])
GO
ALTER TABLE [dbo].[tbMovimentacaoProdutos] CHECK CONSTRAINT [FK_tbMovimentacaoProdutos_tbMovimentacao]
GO
ALTER TABLE [dbo].[tbMovimentacaoProdutos]  WITH CHECK ADD  CONSTRAINT [FK_tbMovimentacaoProdutos_tbProduto] FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbMovimentacaoProdutos] CHECK CONSTRAINT [FK_tbMovimentacaoProdutos_tbProduto]
GO
ALTER TABLE [dbo].[tbMovimentacaoProdutos]  WITH CHECK ADD  CONSTRAINT [FK_tbMovimentacaoProdutos_tbUnidade] FOREIGN KEY([IDUnidade])
REFERENCES [dbo].[tbUnidade] ([IDUnidade])
GO
ALTER TABLE [dbo].[tbMovimentacaoProdutos] CHECK CONSTRAINT [FK_tbMovimentacaoProdutos_tbUnidade]
GO
ALTER TABLE [dbo].[tbOrdemImpressao]  WITH CHECK ADD  CONSTRAINT [FK_tbOrdemImpressao_tbAreaImpressao] FOREIGN KEY([IDAreaImpressao])
REFERENCES [dbo].[tbAreaImpressao] ([IDAreaImpressao])
GO
ALTER TABLE [dbo].[tbOrdemImpressao] CHECK CONSTRAINT [FK_tbOrdemImpressao_tbAreaImpressao]
GO
ALTER TABLE [dbo].[tbPainelModificacao]  WITH CHECK ADD  CONSTRAINT [FK_tbPainelModificacao_tbPainelModificacaoOperacao] FOREIGN KEY([IDPainelModificacaoOperacao])
REFERENCES [dbo].[tbPainelModificacaoOperacao] ([IDPainelModificacaoOperacao])
GO
ALTER TABLE [dbo].[tbPainelModificacao] CHECK CONSTRAINT [FK_tbPainelModificacao_tbPainelModificacaoOperacao]
GO
ALTER TABLE [dbo].[tbPainelModificacaoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoPainelModificacao_tbPainelModificacao] FOREIGN KEY([IDPainelModificacao])
REFERENCES [dbo].[tbPainelModificacao] ([IDPainelModificacao])
GO
ALTER TABLE [dbo].[tbPainelModificacaoProduto] CHECK CONSTRAINT [FK_tbProdutoPainelModificacao_tbPainelModificacao]
GO
ALTER TABLE [dbo].[tbPainelModificacaoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoPainelModificacao_tbProduto] FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbPainelModificacaoProduto] CHECK CONSTRAINT [FK_tbProdutoPainelModificacao_tbProduto]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbCaixa] FOREIGN KEY([IDCaixa])
REFERENCES [dbo].[tbCaixa] ([IDCaixa])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbCaixa]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbCliente] FOREIGN KEY([IDCliente])
REFERENCES [dbo].[tbCliente] ([IDCliente])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbCliente]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbEntregador] FOREIGN KEY([IDEntregador])
REFERENCES [dbo].[tbEntregador] ([IDEntregador])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbEntregador]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbRetornoSAT] FOREIGN KEY([IDRetornoSAT_venda])
REFERENCES [dbo].[tbRetornoSAT] ([IDRetornoSAT])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbRetornoSAT]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbRetornoSAT1] FOREIGN KEY([IDRetornoSAT_cancelamento])
REFERENCES [dbo].[tbRetornoSAT] ([IDRetornoSAT])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbRetornoSAT1]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbStatusPedido] FOREIGN KEY([IDStatusPedido])
REFERENCES [dbo].[tbStatusPedido] ([IDStatusPedido])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbStatusPedido]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbTaxaEntrega] FOREIGN KEY([IDTaxaEntrega])
REFERENCES [dbo].[tbTaxaEntrega] ([IDTaxaEntrega])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbTaxaEntrega]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbTipoDesconto] FOREIGN KEY([IDTipoDesconto])
REFERENCES [dbo].[tbTipoDesconto] ([IDTipoDesconto])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbTipoDesconto]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbTipoEntrada] FOREIGN KEY([IDTipoEntrada])
REFERENCES [dbo].[tbTipoEntrada] ([IDTipoEntrada])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbTipoEntrada]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbTipoPedido] FOREIGN KEY([IDTipoPedido])
REFERENCES [dbo].[tbTipoPedido] ([IDTipoPedido])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbTipoPedido]
GO
ALTER TABLE [dbo].[tbPedido]  WITH CHECK ADD  CONSTRAINT [FK_tbPedido_tbUsuario] FOREIGN KEY([IDUsuario_garcom])
REFERENCES [dbo].[tbUsuario] ([IDUsuario])
GO
ALTER TABLE [dbo].[tbPedido] CHECK CONSTRAINT [FK_tbPedido_tbUsuario]
GO
ALTER TABLE [dbo].[tbPedidoPagamento]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoPagamento_tbPedido] FOREIGN KEY([IDPedido])
REFERENCES [dbo].[tbPedido] ([IDPedido])
GO
ALTER TABLE [dbo].[tbPedidoPagamento] CHECK CONSTRAINT [FK_tbPedidoPagamento_tbPedido]
GO
ALTER TABLE [dbo].[tbPedidoPagamento]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoPagamento_tbTipoPagamento] FOREIGN KEY([IDTipoPagamento])
REFERENCES [dbo].[tbTipoPagamento] ([IDTipoPagamento])
GO
ALTER TABLE [dbo].[tbPedidoPagamento] CHECK CONSTRAINT [FK_tbPedidoPagamento_tbTipoPagamento]
GO
ALTER TABLE [dbo].[tbPedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoItem_tbPedido] FOREIGN KEY([IDPedido])
REFERENCES [dbo].[tbPedido] ([IDPedido])
GO
ALTER TABLE [dbo].[tbPedidoProduto] CHECK CONSTRAINT [FK_tbPedidoItem_tbPedido]
GO
ALTER TABLE [dbo].[tbPedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoProduto_tbMotivoCancelamento] FOREIGN KEY([IDMotivoCancelamento])
REFERENCES [dbo].[tbMotivoCancelamento] ([IDMotivoCancelamento])
GO
ALTER TABLE [dbo].[tbPedidoProduto] CHECK CONSTRAINT [FK_tbPedidoProduto_tbMotivoCancelamento]
GO
ALTER TABLE [dbo].[tbPedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoProduto_tbPainelModificacao] FOREIGN KEY([IDPainelModificacao])
REFERENCES [dbo].[tbPainelModificacao] ([IDPainelModificacao])
GO
ALTER TABLE [dbo].[tbPedidoProduto] CHECK CONSTRAINT [FK_tbPedidoProduto_tbPainelModificacao]
GO
ALTER TABLE [dbo].[tbPedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoProduto_tbPDV] FOREIGN KEY([IDPDV])
REFERENCES [dbo].[tbPDV] ([IDPDV])
GO
ALTER TABLE [dbo].[tbPedidoProduto] CHECK CONSTRAINT [FK_tbPedidoProduto_tbPDV]
GO
ALTER TABLE [dbo].[tbPedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoProduto_tbPDV1] FOREIGN KEY([IDPDV_cancelamento])
REFERENCES [dbo].[tbPDV] ([IDPDV])
GO
ALTER TABLE [dbo].[tbPedidoProduto] CHECK CONSTRAINT [FK_tbPedidoProduto_tbPDV1]
GO
ALTER TABLE [dbo].[tbPedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoProduto_tbPedidoProduto] FOREIGN KEY([IDPedidoProduto_pai])
REFERENCES [dbo].[tbPedidoProduto] ([IDPedidoProduto])
GO
ALTER TABLE [dbo].[tbPedidoProduto] CHECK CONSTRAINT [FK_tbPedidoProduto_tbPedidoProduto]
GO
ALTER TABLE [dbo].[tbPedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoProduto_tbProduto2] FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbPedidoProduto] CHECK CONSTRAINT [FK_tbPedidoProduto_tbProduto2]
GO
ALTER TABLE [dbo].[tbPedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoProduto_tbUsuario1] FOREIGN KEY([IDUsuario_cancelamento])
REFERENCES [dbo].[tbUsuario] ([IDUsuario])
GO
ALTER TABLE [dbo].[tbPedidoProduto] CHECK CONSTRAINT [FK_tbPedidoProduto_tbUsuario1]
GO
ALTER TABLE [dbo].[tbPedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbPedidoProduto_tbUsuario2] FOREIGN KEY([IDUsuario])
REFERENCES [dbo].[tbUsuario] ([IDUsuario])
GO
ALTER TABLE [dbo].[tbPedidoProduto] CHECK CONSTRAINT [FK_tbPedidoProduto_tbUsuario2]
GO
ALTER TABLE [dbo].[tbProcessamentoSAT]  WITH CHECK ADD  CONSTRAINT [FK_tbProcessamentoSAT_tbRetornoSAT] FOREIGN KEY([IDRetornoSAT])
REFERENCES [dbo].[tbRetornoSAT] ([IDRetornoSAT])
GO
ALTER TABLE [dbo].[tbProcessamentoSAT] CHECK CONSTRAINT [FK_tbProcessamentoSAT_tbRetornoSAT]
GO
ALTER TABLE [dbo].[tbProcessamentoSAT]  WITH CHECK ADD  CONSTRAINT [FK_tbProcessamentoSAT_tbStatusProcessamentoSAT] FOREIGN KEY([IDStatusProcessamentoSAT])
REFERENCES [dbo].[tbStatusProcessamentoSAT] ([IDStatusProcessamentoSAT])
GO
ALTER TABLE [dbo].[tbProcessamentoSAT] CHECK CONSTRAINT [FK_tbProcessamentoSAT_tbStatusProcessamentoSAT]
GO
ALTER TABLE [dbo].[tbProcessamentoSAT]  WITH CHECK ADD  CONSTRAINT [FK_tbProcessamentoSAT_tbTipoSolicitacaoSAT] FOREIGN KEY([IDTipoSolicitacaoSAT])
REFERENCES [dbo].[tbTipoSolicitacaoSAT] ([IDTipoSolicitacaoSAT])
GO
ALTER TABLE [dbo].[tbProcessamentoSAT] CHECK CONSTRAINT [FK_tbProcessamentoSAT_tbTipoSolicitacaoSAT]
GO
ALTER TABLE [dbo].[tbProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbProduto_tbClassificacaoFiscal] FOREIGN KEY([IDClassificacaoFiscal])
REFERENCES [dbo].[tbClassificacaoFiscal] ([IDClassificacaoFiscal])
GO
ALTER TABLE [dbo].[tbProduto] CHECK CONSTRAINT [FK_tbProduto_tbClassificacaoFiscal]
GO
ALTER TABLE [dbo].[tbProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbProduto_tbTipoProduto] FOREIGN KEY([IDTipoProduto])
REFERENCES [dbo].[tbTipoProduto] ([IDTipoProduto])
GO
ALTER TABLE [dbo].[tbProduto] CHECK CONSTRAINT [FK_tbProduto_tbTipoProduto]
GO
ALTER TABLE [dbo].[tbProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbProduto_tbUnidade] FOREIGN KEY([IDUnidade])
REFERENCES [dbo].[tbUnidade] ([IDUnidade])
GO
ALTER TABLE [dbo].[tbProduto] CHECK CONSTRAINT [FK_tbProduto_tbUnidade]
GO
ALTER TABLE [dbo].[tbProdutoCategoriaProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoCategoriaProduto_tbCategoriaProduto] FOREIGN KEY([IDCategoriaProduto])
REFERENCES [dbo].[tbCategoriaProduto] ([IDCategoriaProduto])
GO
ALTER TABLE [dbo].[tbProdutoCategoriaProduto] CHECK CONSTRAINT [FK_tbProdutoCategoriaProduto_tbCategoriaProduto]
GO
ALTER TABLE [dbo].[tbProdutoCategoriaProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoCategoriaProduto_tbProduto] FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbProdutoCategoriaProduto] CHECK CONSTRAINT [FK_tbProdutoCategoriaProduto_tbProduto]
GO
ALTER TABLE [dbo].[tbProdutoImagem]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoImagem_tbImagem] FOREIGN KEY([IDImagem])
REFERENCES [dbo].[tbImagem] ([IDImagem])
GO
ALTER TABLE [dbo].[tbProdutoImagem] CHECK CONSTRAINT [FK_tbProdutoImagem_tbImagem]
GO
ALTER TABLE [dbo].[tbProdutoImagem]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoImagem_tbProduto] FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbProdutoImagem] CHECK CONSTRAINT [FK_tbProdutoImagem_tbProduto]
GO
ALTER TABLE [dbo].[tbProdutoPainelModificacao]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoPainelModificacao_tbPainelModificacao1] FOREIGN KEY([IDPainelModificacao])
REFERENCES [dbo].[tbPainelModificacao] ([IDPainelModificacao])
GO
ALTER TABLE [dbo].[tbProdutoPainelModificacao] CHECK CONSTRAINT [FK_tbProdutoPainelModificacao_tbPainelModificacao1]
GO
ALTER TABLE [dbo].[tbProdutoPainelModificacao]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoPainelModificacao_tbProduto1] FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbProdutoPainelModificacao] CHECK CONSTRAINT [FK_tbProdutoPainelModificacao_tbProduto1]
GO
ALTER TABLE [dbo].[tbProdutoReceita]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoReceita_tbProduto] FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbProdutoReceita] CHECK CONSTRAINT [FK_tbProdutoReceita_tbProduto]
GO
ALTER TABLE [dbo].[tbProdutoReceita]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoReceita_tbProduto1] FOREIGN KEY([IDProduto_ingrediente])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbProdutoReceita] CHECK CONSTRAINT [FK_tbProdutoReceita_tbProduto1]
GO
ALTER TABLE [dbo].[tbProdutoReceita]  WITH CHECK ADD  CONSTRAINT [FK_tbProdutoReceita_tbUnidade] FOREIGN KEY([IDUnidade])
REFERENCES [dbo].[tbUnidade] ([IDUnidade])
GO
ALTER TABLE [dbo].[tbProdutoReceita] CHECK CONSTRAINT [FK_tbProdutoReceita_tbUnidade]
GO
ALTER TABLE [dbo].[tbProdutoTraducao]  WITH CHECK ADD FOREIGN KEY([IDIdioma])
REFERENCES [dbo].[tbIdioma] ([IDIdioma])
GO
ALTER TABLE [dbo].[tbProdutoTraducao]  WITH CHECK ADD FOREIGN KEY([IDProduto])
REFERENCES [dbo].[tbProduto] ([IDProduto])
GO
ALTER TABLE [dbo].[tbRelatorio]  WITH CHECK ADD  CONSTRAINT [FK_tbRelatorio_tbTipoRelatorio] FOREIGN KEY([IDTipoRelatorio])
REFERENCES [dbo].[tbTipoRelatorio] ([IDTipoRelatorio])
GO
ALTER TABLE [dbo].[tbRelatorio] CHECK CONSTRAINT [FK_tbRelatorio_tbTipoRelatorio]
GO
ALTER TABLE [dbo].[tbRetornoSAT]  WITH CHECK ADD  CONSTRAINT [FK_tbRetornoSAT_tbRetornoSAT] FOREIGN KEY([IDRetornoSAT_cancelamento])
REFERENCES [dbo].[tbRetornoSAT] ([IDRetornoSAT])
GO
ALTER TABLE [dbo].[tbRetornoSAT] CHECK CONSTRAINT [FK_tbRetornoSAT_tbRetornoSAT]
GO
ALTER TABLE [dbo].[tbRetornoSAT]  WITH CHECK ADD  CONSTRAINT [FK_tbRetornoSAT_tbTipoSolicitacaoSAT] FOREIGN KEY([IDTipoSolicitacaoSAT])
REFERENCES [dbo].[tbTipoSolicitacaoSAT] ([IDTipoSolicitacaoSAT])
GO
ALTER TABLE [dbo].[tbRetornoSAT] CHECK CONSTRAINT [FK_tbRetornoSAT_tbTipoSolicitacaoSAT]
GO
ALTER TABLE [dbo].[tbTemaCardapioPDV]  WITH CHECK ADD  CONSTRAINT [FK_tbTemaCardapioPDV_tbPDV] FOREIGN KEY([IDPDV])
REFERENCES [dbo].[tbPDV] ([IDPDV])
GO
ALTER TABLE [dbo].[tbTemaCardapioPDV] CHECK CONSTRAINT [FK_tbTemaCardapioPDV_tbPDV]
GO
ALTER TABLE [dbo].[tbTemaCardapioPDV]  WITH CHECK ADD  CONSTRAINT [FK_tbTemaCardapioPDV_tbTemaCardapio] FOREIGN KEY([IDTemaCardapio])
REFERENCES [dbo].[tbTemaCardapio] ([IDTemaCardapio])
GO
ALTER TABLE [dbo].[tbTemaCardapioPDV] CHECK CONSTRAINT [FK_tbTemaCardapioPDV_tbTemaCardapio]
GO
ALTER TABLE [dbo].[tbTipoPagamento]  WITH CHECK ADD  CONSTRAINT [FK_tbTipoPagamento_tbMeioPagamentoSAT] FOREIGN KEY([IDMeioPagamentoSAT])
REFERENCES [dbo].[tbMeioPagamentoSAT] ([IDMeioPagamentoSAT])
GO
ALTER TABLE [dbo].[tbTipoPagamento] CHECK CONSTRAINT [FK_tbTipoPagamento_tbMeioPagamentoSAT]
GO

ALTER TABLE dbo.tbCategoriaProduto ADD
  Disponibilidade bit NOT NULL CONSTRAINT DF_tbCategoriaProduto_Disponibilidade DEFAULT (1),
  DtAlteracaoDisponibilidade datetime NOT NULL DEFAULT(GETDATE())

ALTER TABLE dbo.tbPedidoProduto ADD
  ValorDesconto decimal(18, 2) NULL

ALTER TABLE dbo.tbPedido ADD
  AplicarDesconto bit NULL,
  AplicarServico bit NULL