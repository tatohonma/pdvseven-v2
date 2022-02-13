namespace a7D.PDV.EF.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        internal static bool Skip { get; set; } = true;

        public override void Up()
        {
            if (Skip)
                return;

            CreateTable(
                "dbo.tbAcao",
                c => new
                    {
                        IDAcao = c.Int(nullable: false, identity: true),
                        IDPDV = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Parametro = c.String(),
                        DtSolicitacao = c.DateTime(),
                    })
                .PrimaryKey(t => t.IDAcao)
                .ForeignKey("dbo.tbPDV", t => t.IDPDV, cascadeDelete: false)
                .Index(t => t.IDPDV);
            
            CreateTable(
                "dbo.tbPDV",
                c => new
                    {
                        IDPDV = c.Int(nullable: false),
                        Nome = c.String(maxLength: 50),
                        Dados = c.String(),
                    })
                .PrimaryKey(t => t.IDPDV);
            
            CreateTable(
                "dbo.tbCaixa",
                c => new
                    {
                        IDCaixa = c.Int(nullable: false, identity: true),
                        IDPDV = c.Int(nullable: false),
                        IDUsuario = c.Int(),
                        IDFechamento = c.Int(),
                        DtAbertura = c.DateTime(nullable: false),
                        DtFechamento = c.DateTime(),
                        SincERP = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDCaixa)
                .ForeignKey("dbo.tbFechamento", t => t.IDFechamento)
                .ForeignKey("dbo.tbPDV", t => t.IDPDV, cascadeDelete: false)
                .ForeignKey("dbo.tbUsuario", t => t.IDUsuario)
                .Index(t => t.IDPDV)
                .Index(t => t.IDUsuario)
                .Index(t => t.IDFechamento);
            
            CreateTable(
                "dbo.tbCaixaAjuste",
                c => new
                    {
                        IDCaixaAjuste = c.Int(nullable: false, identity: true),
                        IDCaixa = c.Int(nullable: false),
                        IDCaixaTipoAjuste = c.Int(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descricao = c.String(maxLength: 500),
                        DtAjuste = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDCaixaAjuste)
                .ForeignKey("dbo.tbCaixa", t => t.IDCaixa, cascadeDelete: false)
                .ForeignKey("dbo.tbCaixaTipoAjuste", t => t.IDCaixaTipoAjuste, cascadeDelete: false)
                .Index(t => t.IDCaixa)
                .Index(t => t.IDCaixaTipoAjuste);
            
            CreateTable(
                "dbo.tbCaixaTipoAjuste",
                c => new
                    {
                        IDCaixaTipoAjuste = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDCaixaTipoAjuste);
            
            CreateTable(
                "dbo.tbCaixaValorRegistro",
                c => new
                    {
                        IDCaixaValorRegistro = c.Int(nullable: false, identity: true),
                        IDCaixa = c.Int(nullable: false),
                        IDTipoPagamento = c.Int(nullable: false),
                        ValorAbertura = c.Decimal(precision: 18, scale: 2),
                        ValorFechamento = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IDCaixaValorRegistro)
                .ForeignKey("dbo.tbCaixa", t => t.IDCaixa, cascadeDelete: false)
                .ForeignKey("dbo.tbTipoPagamento", t => t.IDTipoPagamento, cascadeDelete: false)
                .Index(t => t.IDCaixa)
                .Index(t => t.IDTipoPagamento);
            
            CreateTable(
                "dbo.tbTipoPagamento",
                c => new
                    {
                        IDTipoPagamento = c.Int(nullable: false, identity: true),
                        CodigoImpressoraFiscal = c.String(maxLength: 50),
                        CodigoERP = c.String(maxLength: 50),
                        Nome = c.String(nullable: false, maxLength: 50),
                        RegistrarValores = c.Boolean(nullable: false),
                        PrazoCredito = c.Int(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        IDMeioPagamentoSAT = c.Int(),
                    })
                .PrimaryKey(t => t.IDTipoPagamento)
                .ForeignKey("dbo.tbMeioPagamentoSAT", t => t.IDMeioPagamentoSAT)
                .Index(t => t.IDMeioPagamentoSAT);
            
            CreateTable(
                "dbo.tbMeioPagamentoSAT",
                c => new
                    {
                        IDMeioPagamentoSAT = c.Int(nullable: false, identity: true),
                        Codigo = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        Descricao = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDMeioPagamentoSAT);
            
            CreateTable(
                "dbo.tbPedidoPagamento",
                c => new
                    {
                        IDPedidoPagamento = c.Int(nullable: false, identity: true),
                        IDPedido = c.Int(nullable: false),
                        IDTipoPagamento = c.Int(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Autorizacao = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IDPedidoPagamento)
                .ForeignKey("dbo.tbPedido", t => t.IDPedido, cascadeDelete: false)
                .ForeignKey("dbo.tbTipoPagamento", t => t.IDTipoPagamento, cascadeDelete: false)
                .Index(t => t.IDPedido)
                .Index(t => t.IDTipoPagamento);
            
            CreateTable(
                "dbo.tbPedido",
                c => new
                    {
                        IDPedido = c.Int(nullable: false, identity: true),
                        IDCliente = c.Int(),
                        IDTipoPedido = c.Int(nullable: false),
                        IDStatusPedido = c.Int(nullable: false),
                        IDCaixa = c.Int(),
                        IDUsuario_garcom = c.Int(),
                        IDTipoEntrada = c.Int(),
                        IDRetornoSAT_venda = c.Int(),
                        IDRetornoSAT_cancelamento = c.Int(),
                        IDTipoDesconto = c.Int(),
                        IDTaxaEntrega = c.Int(),
                        IDEntregador = c.Int(),
                        GUIDIdentificacao = c.String(maxLength: 50),
                        CodigoEmenu = c.String(maxLength: 50),
                        NumeroCupom = c.String(maxLength: 50),
                        DocumentoCliente = c.String(maxLength: 50),
                        DtPedido = c.DateTime(),
                        DtPedidoFechamento = c.DateTime(),
                        SincERP = c.Boolean(),
                        SincEMENU = c.Boolean(),
                        ValorConsumacaoMinima = c.Decimal(precision: 18, scale: 2),
                        ValorServico = c.Decimal(precision: 18, scale: 2),
                        ValorDesconto = c.Decimal(precision: 18, scale: 2),
                        TaxaServicoPadrao = c.Decimal(precision: 18, scale: 2),
                        ValorTotal = c.Decimal(precision: 18, scale: 2),
                        GUIDAgrupamentoPedido = c.String(maxLength: 50),
                        Observacoes = c.String(),
                        ReferenciaLocalizacao = c.String(maxLength: 500),
                        GUIDMovimentacao = c.String(maxLength: 50),
                        NumeroPessoas = c.Int(),
                        DtEnvio = c.DateTime(),
                        DtEntrega = c.DateTime(),
                        ValorEntrega = c.Decimal(precision: 18, scale: 2),
                        AplicarDesconto = c.Boolean(),
                        AplicarServico = c.Boolean(),
                        IDUsuarioDesconto = c.Int(),
                        IDUsuarioTaxaServico = c.Int(),
                    })
                .PrimaryKey(t => t.IDPedido)
                .ForeignKey("dbo.tbCaixa", t => t.IDCaixa)
                .ForeignKey("dbo.tbCliente", t => t.IDCliente)
                .ForeignKey("dbo.tbEntregador", t => t.IDEntregador)
                .ForeignKey("dbo.tbRetornoSAT", t => t.IDRetornoSAT_venda)
                .ForeignKey("dbo.tbRetornoSAT", t => t.IDRetornoSAT_cancelamento)
                .ForeignKey("dbo.tbStatusPedido", t => t.IDStatusPedido, cascadeDelete: false)
                .ForeignKey("dbo.tbTaxaEntrega", t => t.IDTaxaEntrega)
                .ForeignKey("dbo.tbTipoDesconto", t => t.IDTipoDesconto)
                .ForeignKey("dbo.tbTipoEntrada", t => t.IDTipoEntrada)
                .ForeignKey("dbo.tbTipoPedido", t => t.IDTipoPedido, cascadeDelete: false)
                .ForeignKey("dbo.tbUsuario", t => t.IDUsuario_garcom)
                .ForeignKey("dbo.tbUsuario", t => t.IDUsuarioDesconto)
                .ForeignKey("dbo.tbUsuario", t => t.IDUsuarioTaxaServico)
                .Index(t => t.IDCliente)
                .Index(t => t.IDTipoPedido)
                .Index(t => t.IDStatusPedido)
                .Index(t => t.IDCaixa)
                .Index(t => t.IDUsuario_garcom)
                .Index(t => t.IDTipoEntrada)
                .Index(t => t.IDRetornoSAT_venda)
                .Index(t => t.IDRetornoSAT_cancelamento)
                .Index(t => t.IDTipoDesconto)
                .Index(t => t.IDTaxaEntrega)
                .Index(t => t.IDEntregador)
                .Index(t => t.IDUsuarioDesconto)
                .Index(t => t.IDUsuarioTaxaServico);
            
            CreateTable(
                "dbo.tbCliente",
                c => new
                    {
                        IDCliente = c.Int(nullable: false, identity: true),
                        CodigoERP = c.String(maxLength: 50),
                        NomeCompleto = c.String(maxLength: 50),
                        Telefone1DDD = c.Int(),
                        Telefone1Numero = c.Int(),
                        Telefone2DDD = c.Int(),
                        Telefone2Numero = c.Int(),
                        Documento1 = c.String(maxLength: 50),
                        Limite = c.Decimal(precision: 18, scale: 2),
                        Credito = c.Decimal(precision: 18, scale: 2),
                        Bloqueado = c.Boolean(nullable: false),
                        Observacao = c.String(),
                        Endereco = c.String(maxLength: 500),
                        EnderecoNumero = c.String(maxLength: 50),
                        Complemento = c.String(maxLength: 500),
                        Bairro = c.String(maxLength: 500),
                        Cidade = c.String(maxLength: 500),
                        IDEstado = c.Int(),
                        CEP = c.Int(),
                        EnderecoReferencia = c.String(),
                        DataNascimento = c.DateTime(),
                        Sexo = c.String(maxLength: 1),
                        DtInclusao = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.IDCliente)
                .ForeignKey("dbo.tbEstado", t => t.IDEstado)
                .Index(t => t.IDEstado);
            
            CreateTable(
                "dbo.tbEstado",
                c => new
                    {
                        IDEstado = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Sigla = c.String(nullable: false, maxLength: 2),
                    })
                .PrimaryKey(t => t.IDEstado);
            
            CreateTable(
                "dbo.tbEntregador",
                c => new
                    {
                        IDEntregador = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 200),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDEntregador);
            
            CreateTable(
                "dbo.tbPedidoProduto",
                c => new
                    {
                        IDPedidoProduto = c.Int(nullable: false, identity: true),
                        IDPedido = c.Int(nullable: false),
                        IDProduto = c.Int(nullable: false),
                        IDPedidoProduto_pai = c.Int(),
                        IDPDV = c.Int(),
                        IDUsuario = c.Int(),
                        IDPDV_cancelamento = c.Int(),
                        IDUsuario_cancelamento = c.Int(),
                        IDMotivoCancelamento = c.Int(),
                        Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorAliquota = c.Decimal(precision: 18, scale: 2),
                        CodigoAliquota = c.String(maxLength: 50),
                        Notas = c.String(),
                        Cancelado = c.Boolean(nullable: false),
                        DtInclusao = c.DateTime(),
                        DtAlteracao = c.DateTime(),
                        ObservacoesCancelamento = c.String(maxLength: 500),
                        DtCancelamento = c.DateTime(),
                        GUIDControleDuplicidade = c.String(maxLength: 50),
                        RetornarAoEstoque = c.Boolean(nullable: false),
                        IDPainelModificacao = c.Int(),
                        ValorDesconto = c.Decimal(precision: 18, scale: 2),
                        IDUsuarioDesconto = c.Int(),
                        IDTipoDesconto = c.Int(),
                    })
                .PrimaryKey(t => t.IDPedidoProduto)
                .ForeignKey("dbo.tbMotivoCancelamento", t => t.IDMotivoCancelamento)
                .ForeignKey("dbo.tbPainelModificacao", t => t.IDPainelModificacao)
                .ForeignKey("dbo.tbPDV", t => t.IDPDV)
                .ForeignKey("dbo.tbPDV", t => t.IDPDV_cancelamento)
                .ForeignKey("dbo.tbPedido", t => t.IDPedido, cascadeDelete: false)
                .ForeignKey("dbo.tbPedidoProduto", t => t.IDPedidoProduto_pai)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .ForeignKey("dbo.tbTipoDesconto", t => t.IDTipoDesconto)
                .ForeignKey("dbo.tbUsuario", t => t.IDUsuarioDesconto)
                .ForeignKey("dbo.tbUsuario", t => t.IDUsuario_cancelamento)
                .ForeignKey("dbo.tbUsuario", t => t.IDUsuario)
                .Index(t => t.IDPedido)
                .Index(t => t.IDProduto)
                .Index(t => t.IDPedidoProduto_pai)
                .Index(t => t.IDPDV)
                .Index(t => t.IDUsuario)
                .Index(t => t.IDPDV_cancelamento)
                .Index(t => t.IDUsuario_cancelamento)
                .Index(t => t.IDMotivoCancelamento)
                .Index(t => t.IDPainelModificacao)
                .Index(t => t.IDUsuarioDesconto)
                .Index(t => t.IDTipoDesconto);
            
            CreateTable(
                "dbo.tbMotivoCancelamento",
                c => new
                    {
                        IDMotivoCancelamento = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDMotivoCancelamento);
            
            CreateTable(
                "dbo.tbPainelModificacao",
                c => new
                    {
                        IDPainelModificacao = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 500),
                        Titulo = c.String(maxLength: 500),
                        Minimo = c.Int(),
                        Maximo = c.Int(),
                        DtUltimaAlteracao = c.DateTime(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        IDPainelModificacaoOperacao = c.Int(),
                        IDTipoItem = c.Int(),
                        IDValorUtilizado = c.Int(),
                        IgnorarValorItem = c.Boolean(),
                    })
                .PrimaryKey(t => t.IDPainelModificacao)
                .ForeignKey("dbo.tbPainelModificacaoOperacao", t => t.IDPainelModificacaoOperacao)
                .Index(t => t.IDPainelModificacaoOperacao);
            
            CreateTable(
                "dbo.tbPainelModificacaoCategoria",
                c => new
                    {
                        IDPainelModificacaoCategoria = c.Int(nullable: false, identity: true),
                        IDPainelModificacao = c.Int(nullable: false),
                        IDCategoriaProduto = c.Int(nullable: false),
                        Ordem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDPainelModificacaoCategoria)
                .ForeignKey("dbo.tbCategoriaProduto", t => t.IDCategoriaProduto, cascadeDelete: false)
                .ForeignKey("dbo.tbPainelModificacao", t => t.IDPainelModificacao, cascadeDelete: false)
                .Index(t => t.IDPainelModificacao)
                .Index(t => t.IDCategoriaProduto);
            
            CreateTable(
                "dbo.tbCategoriaProduto",
                c => new
                    {
                        IDCategoriaProduto = c.Int(nullable: false, identity: true),
                        CodigoERP = c.String(maxLength: 50),
                        Nome = c.String(nullable: false, maxLength: 50),
                        DtUltimaAlteracao = c.DateTime(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        Disponibilidade = c.Boolean(nullable: false),
                        DtAlteracaoDisponibilidade = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDCategoriaProduto);
            
            CreateTable(
                "dbo.tbProdutoCategoriaProduto",
                c => new
                    {
                        IDProdutoCategoriaProduto = c.Int(nullable: false, identity: true),
                        IDProduto = c.Int(nullable: false),
                        IDCategoriaProduto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDProdutoCategoriaProduto)
                .ForeignKey("dbo.tbCategoriaProduto", t => t.IDCategoriaProduto, cascadeDelete: false)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .Index(t => t.IDProduto)
                .Index(t => t.IDCategoriaProduto);
            
            CreateTable(
                "dbo.tbProduto",
                c => new
                    {
                        IDProduto = c.Int(nullable: false, identity: true),
                        IDTipoProduto = c.Int(nullable: false),
                        Codigo = c.String(maxLength: 50),
                        CodigoERP = c.String(maxLength: 50),
                        Nome = c.String(nullable: false, maxLength: 500),
                        Descricao = c.String(maxLength: 500),
                        ValorUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorAliquota = c.Decimal(precision: 18, scale: 2),
                        CodigoAliquota = c.String(maxLength: 50),
                        Quantidade = c.Decimal(precision: 18, scale: 2),
                        Ativo = c.Boolean(nullable: false),
                        Disponibilidade = c.Boolean(nullable: false),
                        DtAlteracaoDisponibilidade = c.DateTime(nullable: false),
                        DtUltimaAlteracao = c.DateTime(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        IDClassificacaoFiscal = c.Int(),
                        IDUnidade = c.Int(),
                        cEAN = c.String(maxLength: 50),
                        ControlarEstoque = c.Boolean(nullable: false),
                        UtilizarBalanca = c.Boolean(nullable: false),
                        CodigoIntegracao1 = c.String(maxLength: 50),
                        IDIntegracao1 = c.String(maxLength: 50),
                        DescricaoIntegracao1 = c.String(maxLength: 200),
                        CodigoIntegracao2 = c.String(maxLength: 50),
                        IDIntegracao2 = c.String(maxLength: 50),
                        DescricaoIntegracao2 = c.String(maxLength: 200),
                        ValorUnitario2 = c.Decimal(precision: 18, scale: 2),
                        ValorUnitario3 = c.Decimal(precision: 18, scale: 2),
                        AssistenteModificacoes = c.Boolean(),
                    })
                .PrimaryKey(t => t.IDProduto)
                .ForeignKey("dbo.tbClassificacaoFiscal", t => t.IDClassificacaoFiscal)
                .ForeignKey("dbo.tbTipoProduto", t => t.IDTipoProduto, cascadeDelete: false)
                .ForeignKey("dbo.tbUnidade", t => t.IDUnidade)
                .Index(t => t.IDTipoProduto)
                .Index(t => t.IDClassificacaoFiscal)
                .Index(t => t.IDUnidade);
            
            CreateTable(
                "dbo.tbClassificacaoFiscal",
                c => new
                    {
                        IDClassificacaoFiscal = c.Int(nullable: false, identity: true),
                        IDTipoTributacao = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Descricao = c.String(maxLength: 500),
                        NCM = c.String(nullable: false, maxLength: 50),
                        IOF = c.Decimal(precision: 18, scale: 2),
                        IPI = c.Decimal(precision: 18, scale: 2),
                        PISPASEP = c.Decimal(precision: 18, scale: 2),
                        CIDE = c.Decimal(precision: 18, scale: 2),
                        COFINS = c.Decimal(precision: 18, scale: 2),
                        ICMS = c.Decimal(precision: 18, scale: 2),
                        ISS = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IDClassificacaoFiscal)
                .ForeignKey("dbo.tbTipoTributacao", t => t.IDTipoTributacao, cascadeDelete: false)
                .Index(t => t.IDTipoTributacao);
            
            CreateTable(
                "dbo.tbTipoTributacao",
                c => new
                    {
                        IDTipoTributacao = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Descricao = c.String(maxLength: 500),
                        CFOP = c.String(nullable: false, maxLength: 50),
                        ICMS00_Orig = c.String(maxLength: 50),
                        ICMS00_CST = c.String(maxLength: 50),
                        ICMS00_pICMS = c.String(maxLength: 50),
                        ICMS40_Orig = c.String(maxLength: 50),
                        ICMS40_CST = c.String(maxLength: 50),
                        ICMSSN102_Orig = c.String(maxLength: 50),
                        ICMSSN102_CSOSN = c.String(maxLength: 50),
                        ICMSSN900_Orig = c.String(maxLength: 50),
                        ICMSSN900_CSOSN = c.String(maxLength: 50),
                        ICMSSN900_pICMS = c.String(maxLength: 50),
                        PISAliq_CST = c.String(maxLength: 50),
                        PISAliq_pPIS = c.String(maxLength: 50),
                        PISQtde_CST = c.String(maxLength: 50),
                        PISQtde_vAliqProd = c.String(maxLength: 50),
                        PISNT_CST = c.String(maxLength: 50),
                        PISSN_CST = c.String(maxLength: 50),
                        PISOutr_CST = c.String(maxLength: 50),
                        PISOutr_pPIS = c.String(maxLength: 50),
                        PISOutr_vAliqProd = c.String(maxLength: 50),
                        PISST_pPIS = c.String(maxLength: 50),
                        PISST_vAliqProd = c.String(maxLength: 50),
                        COFINSAliq_CST = c.String(maxLength: 50),
                        COFINSAliq_pCOFINS = c.String(maxLength: 50),
                        COFINSQtde_CST = c.String(maxLength: 50),
                        COFINSQtde_vAliqProd = c.String(maxLength: 50),
                        COFINSNT_CST = c.String(maxLength: 50),
                        COFINSSN_CST = c.String(maxLength: 50),
                        COFINSOutr_CST = c.String(maxLength: 50),
                        COFINSOutr_pCOFINS = c.String(maxLength: 50),
                        COFINSOutr_vAliqProd = c.String(maxLength: 50),
                        COFINSST_pCOFINS = c.String(maxLength: 50),
                        COFINSST_vAliqProd = c.String(maxLength: 50),
                        ISSQN_vDeducISSQN = c.String(maxLength: 50),
                        ISSQN_vAliq = c.String(maxLength: 50),
                        ISSQN_cListServ = c.String(maxLength: 50),
                        ISSQN_cServTribMun = c.String(maxLength: 50),
                        ISSQN_cNatOp = c.String(maxLength: 50),
                        ISSQN_indIncFisc = c.String(maxLength: 50),
                        vItem12741 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IDTipoTributacao);
            
            CreateTable(
                "dbo.tbEntradaSaida",
                c => new
                    {
                        IDEntradaSaida = c.Int(nullable: false, identity: true),
                        GUID_Origem = c.String(maxLength: 36, fixedLength: true),
                        IDProduto = c.Int(nullable: false),
                        Entrada = c.Boolean(nullable: false),
                        Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDEntradaSaida)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .Index(t => t.IDProduto);
            
            CreateTable(
                "dbo.tbInventarioProdutos",
                c => new
                    {
                        IDInventarioProdutos = c.Int(nullable: false, identity: true),
                        IDInventario = c.Int(nullable: false),
                        IDProduto = c.Int(nullable: false),
                        IDUnidade = c.Int(nullable: false),
                        Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantidadeAnterior = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IDInventarioProdutos)
                .ForeignKey("dbo.tbInventario", t => t.IDInventario, cascadeDelete: false)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .ForeignKey("dbo.tbUnidade", t => t.IDUnidade, cascadeDelete: false)
                .Index(t => t.IDInventario)
                .Index(t => t.IDProduto)
                .Index(t => t.IDUnidade);
            
            CreateTable(
                "dbo.tbInventario",
                c => new
                    {
                        IDInventario = c.Int(nullable: false, identity: true),
                        GUID = c.String(nullable: false, maxLength: 36, fixedLength: true),
                        Descricao = c.String(nullable: false, maxLength: 255),
                        Data = c.DateTime(nullable: false),
                        Processado = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDInventario);
            
            CreateTable(
                "dbo.tbUnidade",
                c => new
                    {
                        IDUnidade = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Simbolo = c.String(nullable: false, maxLength: 50),
                        Excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDUnidade);
            
            CreateTable(
                "dbo.tbConversaoUnidade",
                c => new
                    {
                        IDConversaoUnidade = c.Int(nullable: false, identity: true),
                        IDUnidade_de = c.Int(nullable: false),
                        IDUnidade_para = c.Int(nullable: false),
                        Divisao = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Multiplicacao = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IDConversaoUnidade)
                .ForeignKey("dbo.tbUnidade", t => t.IDUnidade_de, cascadeDelete: false)
                .ForeignKey("dbo.tbUnidade", t => t.IDUnidade_para, cascadeDelete: false)
                .Index(t => t.IDUnidade_de)
                .Index(t => t.IDUnidade_para);
            
            CreateTable(
                "dbo.tbMovimentacaoProdutos",
                c => new
                    {
                        IDMovimentacaoProdutos = c.Int(nullable: false, identity: true),
                        IDMovimentacao = c.Int(nullable: false),
                        IDProduto = c.Int(nullable: false),
                        IDUnidade = c.Int(nullable: false),
                        Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IDMovimentacaoProdutos)
                .ForeignKey("dbo.tbMovimentacao", t => t.IDMovimentacao, cascadeDelete: false)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .ForeignKey("dbo.tbUnidade", t => t.IDUnidade, cascadeDelete: false)
                .Index(t => t.IDMovimentacao)
                .Index(t => t.IDProduto)
                .Index(t => t.IDUnidade);
            
            CreateTable(
                "dbo.tbMovimentacao",
                c => new
                    {
                        IDMovimentacao = c.Int(nullable: false, identity: true),
                        IDTipoMovimentacao = c.Int(nullable: false),
                        IDFornecedor = c.Int(),
                        GUID = c.String(nullable: false, maxLength: 36, fixedLength: true),
                        DataMovimentacao = c.DateTime(nullable: false),
                        Descricao = c.String(maxLength: 100),
                        NumeroPedido = c.String(maxLength: 50),
                        IDMovimentacao_reversa = c.Int(),
                        Excluido = c.Boolean(nullable: false),
                        Reversa = c.Boolean(nullable: false),
                        Processado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDMovimentacao)
                .ForeignKey("dbo.tbTipoMovimentacao", t => t.IDTipoMovimentacao, cascadeDelete: false)
                .Index(t => t.IDTipoMovimentacao);
            
            CreateTable(
                "dbo.tbTipoMovimentacao",
                c => new
                    {
                        IDTipoMovimentacao = c.Int(nullable: false, identity: true),
                        Tipo = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Descricao = c.String(maxLength: 255),
                        Excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDTipoMovimentacao);
            
            CreateTable(
                "dbo.tbProdutoReceita",
                c => new
                    {
                        IDProdutoReceita = c.Int(nullable: false, identity: true),
                        IDProduto = c.Int(nullable: false),
                        IDProduto_ingrediente = c.Int(nullable: false),
                        IDUnidade = c.Int(nullable: false),
                        Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IDProdutoReceita)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto_ingrediente, cascadeDelete: false)
                .ForeignKey("dbo.tbUnidade", t => t.IDUnidade, cascadeDelete: false)
                .Index(t => t.IDProduto)
                .Index(t => t.IDProduto_ingrediente)
                .Index(t => t.IDUnidade);
            
            CreateTable(
                "dbo.tbMapAreaImpressaoProduto",
                c => new
                    {
                        IDMapAreaImpressaoProduto = c.Int(nullable: false, identity: true),
                        IDAreaImpressao = c.Int(nullable: false),
                        IDProduto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDMapAreaImpressaoProduto)
                .ForeignKey("dbo.tbAreaImpressao", t => t.IDAreaImpressao, cascadeDelete: false)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .Index(t => t.IDAreaImpressao)
                .Index(t => t.IDProduto);
            
            CreateTable(
                "dbo.tbAreaImpressao",
                c => new
                    {
                        IDAreaImpressao = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        NomeImpressora = c.String(maxLength: 50),
                        IDTipoAreaImpressao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDAreaImpressao)
                .ForeignKey("dbo.tbTipoAreaImpressao", t => t.IDTipoAreaImpressao, cascadeDelete: false)
                .Index(t => t.IDTipoAreaImpressao);
            
            CreateTable(
                "dbo.tbOrdemImpressao",
                c => new
                    {
                        IDOrdemImpressao = c.Int(nullable: false, identity: true),
                        IDAreaImpressao = c.Int(nullable: false),
                        ConteudoImpressao = c.String(nullable: false),
                        DtOrdem = c.DateTime(nullable: false),
                        EnviadoFilaImpressao = c.Boolean(nullable: false),
                        Conta = c.Boolean(nullable: false),
                        SAT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDOrdemImpressao)
                .ForeignKey("dbo.tbAreaImpressao", t => t.IDAreaImpressao, cascadeDelete: false)
                .Index(t => t.IDAreaImpressao);
            
            CreateTable(
                "dbo.tbTipoAreaImpressao",
                c => new
                    {
                        IDTipoAreaImpressao = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDTipoAreaImpressao);
            
            CreateTable(
                "dbo.tbPainelModificacaoProduto",
                c => new
                    {
                        IDPainelModificacaoProduto = c.Int(nullable: false, identity: true),
                        IDPainelModificacao = c.Int(nullable: false),
                        IDProduto = c.Int(nullable: false),
                        IgnorarValorItem = c.Boolean(),
                        Ordem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDPainelModificacaoProduto)
                .ForeignKey("dbo.tbPainelModificacao", t => t.IDPainelModificacao, cascadeDelete: false)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .Index(t => t.IDPainelModificacao)
                .Index(t => t.IDProduto);
            
            CreateTable(
                "dbo.tbProdutoImagem",
                c => new
                    {
                        IDProdutoImagem = c.Int(nullable: false, identity: true),
                        IDProduto = c.Int(nullable: false),
                        IDImagem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDProdutoImagem)
                .ForeignKey("dbo.tbImagem", t => t.IDImagem, cascadeDelete: false)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .Index(t => t.IDProduto)
                .Index(t => t.IDImagem);
            
            CreateTable(
                "dbo.tbImagem",
                c => new
                    {
                        IDImagem = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Extensao = c.String(nullable: false, maxLength: 50),
                        Altura = c.Int(),
                        Largura = c.Int(),
                        Tamanho = c.Int(),
                        Dados = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.IDImagem);
            
            CreateTable(
                "dbo.tbImagemTema",
                c => new
                    {
                        IDImagemTema = c.Int(nullable: false, identity: true),
                        IDTemaCardapio = c.Int(nullable: false),
                        IDImagem = c.Int(nullable: false),
                        IDIdioma = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Descricao = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.IDImagemTema)
                .ForeignKey("dbo.tbIdioma", t => t.IDIdioma, cascadeDelete: false)
                .ForeignKey("dbo.tbImagem", t => t.IDImagem, cascadeDelete: false)
                .ForeignKey("dbo.tbTemaCardapio", t => t.IDTemaCardapio, cascadeDelete: false)
                .Index(t => t.IDTemaCardapio)
                .Index(t => t.IDImagem)
                .Index(t => t.IDIdioma);
            
            CreateTable(
                "dbo.tbIdioma",
                c => new
                    {
                        IDIdioma = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Codigo = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.IDIdioma);
            
            CreateTable(
                "dbo.tbProdutoTraducao",
                c => new
                    {
                        IDProdutoTraducao = c.Int(nullable: false, identity: true),
                        IDProduto = c.Int(nullable: false),
                        IDIdioma = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 500),
                        Descricao = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.IDProdutoTraducao)
                .ForeignKey("dbo.tbIdioma", t => t.IDIdioma, cascadeDelete: false)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .Index(t => t.IDProduto)
                .Index(t => t.IDIdioma);
            
            CreateTable(
                "dbo.tbTemaCardapio",
                c => new
                    {
                        IDTemaCardapio = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Descricao = c.String(),
                        XML = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IDTemaCardapio);
            
            CreateTable(
                "dbo.tbTemaCardapioPDV",
                c => new
                    {
                        IDTemaCardapioPDV = c.Int(nullable: false, identity: true),
                        IDTemaCardapio = c.Int(nullable: false),
                        IDPDV = c.Int(),
                        Ativo = c.Boolean(nullable: false),
                        DtUltimaAlteracao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDTemaCardapioPDV)
                .ForeignKey("dbo.tbPDV", t => t.IDPDV)
                .ForeignKey("dbo.tbTemaCardapio", t => t.IDTemaCardapio, cascadeDelete: false)
                .Index(t => t.IDTemaCardapio)
                .Index(t => t.IDPDV);
            
            CreateTable(
                "dbo.tbProdutoPainelModificacao",
                c => new
                    {
                        IDProdutoPainelModificacao = c.Int(nullable: false, identity: true),
                        IDProduto = c.Int(nullable: false),
                        IDPainelModificacao = c.Int(nullable: false),
                        Ordem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDProdutoPainelModificacao)
                .ForeignKey("dbo.tbPainelModificacao", t => t.IDPainelModificacao, cascadeDelete: false)
                .ForeignKey("dbo.tbProduto", t => t.IDProduto, cascadeDelete: false)
                .Index(t => t.IDProduto)
                .Index(t => t.IDPainelModificacao);
            
            CreateTable(
                "dbo.tbTipoProduto",
                c => new
                    {
                        IDTipoProduto = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDTipoProduto);
            
            CreateTable(
                "dbo.tbPainelModificacaoOperacao",
                c => new
                    {
                        IDPainelModificacaoOperacao = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDPainelModificacaoOperacao);
            
            CreateTable(
                "dbo.tbPainelModificacaoRelacionado",
                c => new
                    {
                        IDPainelModificacaoRelacionado = c.Int(nullable: false, identity: true),
                        IDPainelModificacao1 = c.Int(nullable: false),
                        IDPainelModificacao2 = c.Int(nullable: false),
                        IgnorarValorItem = c.Boolean(),
                    })
                .PrimaryKey(t => t.IDPainelModificacaoRelacionado)
                .ForeignKey("dbo.tbPainelModificacao", t => t.IDPainelModificacao1, cascadeDelete: false)
                .ForeignKey("dbo.tbPainelModificacao", t => t.IDPainelModificacao2, cascadeDelete: false)
                .Index(t => t.IDPainelModificacao1)
                .Index(t => t.IDPainelModificacao2);
            
            CreateTable(
                "dbo.tbTipoDesconto",
                c => new
                    {
                        IDTipoDesconto = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Descricao = c.String(maxLength: 500),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDTipoDesconto);
            
            CreateTable(
                "dbo.tbUsuario",
                c => new
                    {
                        IDUsuario = c.Int(nullable: false, identity: true),
                        CodigoEMENU = c.String(maxLength: 50),
                        CodigoERP = c.String(maxLength: 50),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Login = c.String(maxLength: 50),
                        Senha = c.String(maxLength: 50),
                        PermissaoAdm = c.Boolean(nullable: false),
                        PermissaoCaixa = c.Boolean(nullable: false),
                        PermissaoGarcom = c.Boolean(nullable: false),
                        PermissaoGerente = c.Boolean(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        DtUltimaAlteracao = c.DateTime(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDUsuario);
            
            CreateTable(
                "dbo.tbFechamento",
                c => new
                    {
                        IDFechamento = c.Int(nullable: false, identity: true),
                        IDPDV = c.Int(nullable: false),
                        IDUsuario = c.Int(nullable: false),
                        DtFechamento = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDFechamento)
                .ForeignKey("dbo.tbPDV", t => t.IDPDV, cascadeDelete: false)
                .ForeignKey("dbo.tbUsuario", t => t.IDUsuario, cascadeDelete: false)
                .Index(t => t.IDPDV)
                .Index(t => t.IDUsuario);
            
            CreateTable(
                "dbo.tbRetornoSAT",
                c => new
                    {
                        IDRetornoSAT = c.Int(nullable: false, identity: true),
                        IDTipoSolicitacaoSAT = c.Int(nullable: false),
                        numeroSessao = c.String(),
                        EEEEE = c.String(),
                        CCCC = c.String(),
                        mensagem = c.String(),
                        cod = c.String(),
                        mensagemSEFAZ = c.String(),
                        arquivoCFeSAT = c.String(),
                        timeStamp = c.String(),
                        chaveConsulta = c.String(),
                        valorTotalCFe = c.String(),
                        CPFCNPJValue = c.String(),
                        assinaturaQRCODE = c.String(),
                        IDRetornoSAT_cancelamento = c.Int(),
                    })
                .PrimaryKey(t => t.IDRetornoSAT)
                .ForeignKey("dbo.tbRetornoSAT", t => t.IDRetornoSAT_cancelamento)
                .ForeignKey("dbo.tbTipoSolicitacaoSAT", t => t.IDTipoSolicitacaoSAT, cascadeDelete: false)
                .Index(t => t.IDTipoSolicitacaoSAT)
                .Index(t => t.IDRetornoSAT_cancelamento);
            
            CreateTable(
                "dbo.tbProcessamentoSAT",
                c => new
                    {
                        IDProcessamentoSAT = c.Int(nullable: false, identity: true),
                        IDStatusProcessamentoSAT = c.Int(),
                        IDTipoSolicitacaoSAT = c.Int(),
                        IDRetornoSAT = c.Int(),
                        XMLEnvio = c.String(nullable: false),
                        GUID = c.String(nullable: false, maxLength: 50),
                        NumeroSessao = c.Int(),
                        DataSolicitacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDProcessamentoSAT)
                .ForeignKey("dbo.tbRetornoSAT", t => t.IDRetornoSAT)
                .ForeignKey("dbo.tbStatusProcessamentoSAT", t => t.IDStatusProcessamentoSAT)
                .ForeignKey("dbo.tbTipoSolicitacaoSAT", t => t.IDTipoSolicitacaoSAT)
                .Index(t => t.IDStatusProcessamentoSAT)
                .Index(t => t.IDTipoSolicitacaoSAT)
                .Index(t => t.IDRetornoSAT);
            
            CreateTable(
                "dbo.tbStatusProcessamentoSAT",
                c => new
                    {
                        IDStatusProcessamentoSAT = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDStatusProcessamentoSAT);
            
            CreateTable(
                "dbo.tbTipoSolicitacaoSAT",
                c => new
                    {
                        IDTipoSolicitacaoSAT = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDTipoSolicitacaoSAT);
            
            CreateTable(
                "dbo.tbStatusPedido",
                c => new
                    {
                        IDStatusPedido = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDStatusPedido);
            
            CreateTable(
                "dbo.tbTaxaEntrega",
                c => new
                    {
                        IDTaxaEntrega = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 200),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDTaxaEntrega);
            
            CreateTable(
                "dbo.tbTipoEntrada",
                c => new
                    {
                        IDTipoEntrada = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        ValorEntrada = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorConsumacaoMinima = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ativo = c.Boolean(nullable: false),
                        Padrao = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDTipoEntrada);
            
            CreateTable(
                "dbo.tbTipoPedido",
                c => new
                    {
                        IDTipoPedido = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDTipoPedido);
            
            CreateTable(
                "dbo.tbConfiguracaoBD",
                c => new
                    {
                        IDConfiguracaoBD = c.Int(nullable: true, identity: true),
                        IDTipoPDV = c.Int(),
                        IDPDV = c.Int(),
                        Chave = c.String(maxLength: 100),
                        Valor = c.String(maxLength: 1000),
                        ValoresAceitos = c.String(maxLength: 1000),
                        Obrigatorio = c.Boolean(nullable: false),
                        Titulo = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.IDConfiguracaoBD)
                .ForeignKey("dbo.tbPDV", t => t.IDPDV)
                .ForeignKey("dbo.tbTipoPDV", t => t.IDTipoPDV)
                .Index(t => t.IDTipoPDV)
                .Index(t => t.IDPDV);
            
            CreateTable(
                "dbo.tbTipoPDV",
                c => new
                    {
                        IDTipoPDV = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDTipoPDV);
            
            CreateTable(
                "dbo.tbIntegracaoSAT",
                c => new
                    {
                        IDIntegracaoSAT = c.Int(nullable: false, identity: true),
                        IDPDV = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDIntegracaoSAT)
                .ForeignKey("dbo.tbPDV", t => t.IDPDV, cascadeDelete: false)
                .Index(t => t.IDPDV);
            
            CreateTable(
                "dbo.tbCodigoSAT",
                c => new
                    {
                        IDCodigoSAT = c.Int(nullable: false, identity: true),
                        CodigoRetorno = c.String(nullable: false, maxLength: 6),
                        Grupo = c.String(maxLength: 50),
                        Mensagem = c.String(),
                        Descricao = c.String(),
                        Erro = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDCodigoSAT);
            
            CreateTable(
                "dbo.tbComanda",
                c => new
                    {
                        IDComanda = c.Int(nullable: false, identity: true),
                        IDStatusComanda = c.Int(nullable: false),
                        GUIDIdentificacao = c.String(nullable: false, maxLength: 50),
                        Numero = c.Int(nullable: false),
                        Observacao = c.String(),
                    })
                .PrimaryKey(t => t.IDComanda)
                .ForeignKey("dbo.tbStatusComanda", t => t.IDStatusComanda, cascadeDelete: false)
                .Index(t => t.IDStatusComanda);
            
            CreateTable(
                "dbo.tbStatusComanda",
                c => new
                    {
                        IDStatusComanda = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDStatusComanda);
            
            CreateTable(
                "dbo.tbMesa",
                c => new
                    {
                        IDMesa = c.Int(nullable: false, identity: true),
                        IDStatusMesa = c.Int(nullable: false),
                        GUIDIdentificacao = c.String(nullable: false, maxLength: 50),
                        Numero = c.Int(nullable: false),
                        Capacidade = c.Int(),
                    })
                .PrimaryKey(t => t.IDMesa)
                .ForeignKey("dbo.tbStatusMesa", t => t.IDStatusMesa, cascadeDelete: false)
                .Index(t => t.IDStatusMesa);
            
            CreateTable(
                "dbo.tbStatusMesa",
                c => new
                    {
                        IDStatusMesa = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDStatusMesa);
            
            CreateTable(
                "dbo.tbRelatorio",
                c => new
                    {
                        IDRelatorio = c.Int(nullable: false, identity: true),
                        IDTipoRelatorio = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 500),
                        Descricao = c.String(maxLength: 500),
                        QuerySQL = c.String(nullable: false),
                        Ordem = c.Int(nullable: false),
                        Totalizador = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.IDRelatorio)
                .ForeignKey("dbo.tbTipoRelatorio", t => t.IDTipoRelatorio, cascadeDelete: false)
                .Index(t => t.IDTipoRelatorio);
            
            CreateTable(
                "dbo.tbTipoRelatorio",
                c => new
                    {
                        IDTipoRelatorio = c.Int(nullable: false),
                        Nome = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IDTipoRelatorio);
            
        }
        
        public override void Down()
        {
        }
    }
}
