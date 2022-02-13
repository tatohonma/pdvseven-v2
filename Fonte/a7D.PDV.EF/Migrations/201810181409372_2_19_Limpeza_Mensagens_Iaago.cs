namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_19_Limpeza_Mensagens_Iaago : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbAcao", "IDPDV", "dbo.tbPDV");
            DropForeignKey("dbo.tbIntegracaoSAT", "IDPDV", "dbo.tbPDV");
            DropIndex("dbo.tbAcao", new[] { "IDPDV" });
            DropIndex("dbo.tbIntegracaoSAT", new[] { "IDPDV" });
            CreateTable(
                "dbo.tbMensagem",
                c => new
                    {
                        IDMensagem = c.Int(nullable: false, identity: true),
                        DataProcessamento = c.DateTime(),
                        ResultadoProcessamento = c.String(unicode: false, storeType: "text"),
                        IDRespostaProcesamento = c.Int(nullable: false),
                        DataCriada = c.DateTime(nullable: false),
                        DataRecebida = c.DateTime(),
                        IDTipo = c.Int(nullable: false),
                        IDOrigem = c.Int(nullable: false),
                        KeyOrigem = c.String(maxLength: 50),
                        IDDestino = c.Int(nullable: false),
                        KeyDestino = c.String(maxLength: 50),
                        Texto = c.String(nullable: false, unicode: false, storeType: "text"),
                        Parametros = c.String(unicode: false, storeType: "text"),
                        DataVisualizada = c.DateTime(),
                        IDMensagemOrigem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDMensagem);
            
            CreateTable(
                "dbo.tbHorarioDelivery",
                c => new
                    {
                        DiaSemana = c.Int(nullable: false),
                        Turno1Inicio = c.Time(nullable: false, precision: 7),
                        Turno1Fim = c.Time(nullable: false, precision: 7),
                        Turno2Inicio = c.Time(nullable: false, precision: 7),
                        Turno2Fim = c.Time(nullable: false, precision: 7),
                        Turno3Inicio = c.Time(nullable: false, precision: 7),
                        Turno3Fim = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.DiaSemana);
            
            AddColumn("dbo.tbPedido", "NomeCliente", c => c.String());
            AddColumn("dbo.tbPedido", "EmailCliente", c => c.String());
            AddColumn("dbo.tbPedido", "LocalEntrega", c => c.String());
            AddColumn("dbo.tbPedido", "ObservacaoCupom", c => c.String());
            AddColumn("dbo.tbPedido", "TAG", c => c.String(maxLength: 500));
            AddColumn("dbo.tbPedido", "Clima", c => c.String());
            AddColumn("dbo.tbPedidoProduto", "Viagem", c => c.Boolean());
            AddColumn("dbo.tbCategoriaProduto", "Cor", c => c.Int());
            AddColumn("dbo.tbCategoriaProduto", "IDImagem", c => c.Int());
            AddColumn("dbo.tbCategoriaProduto", "TAG", c => c.String(maxLength: 500));
            AddColumn("dbo.tbCategoriaProduto", "IDTipoCategoria", c => c.Int());
            AddColumn("dbo.tbCategoriaProduto", "Ativo", c => c.Boolean());
            AddColumn("dbo.tbProduto", "TAG", c => c.String(maxLength: 500));
            AddColumn("dbo.tbProduto", "Cor", c => c.Int());
            AddColumn("dbo.tbProduto", "IsentoTaxaServico", c => c.Boolean());
            AddColumn("dbo.tbProduto", "ValorUltimaCompra", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.tbProduto", "DataUltimaCompra", c => c.DateTime());
            AddColumn("dbo.tbProduto", "FrequenciaCompra", c => c.String());
            AddColumn("dbo.tbProduto", "DisponibilidadeModificacao ", c => c.Boolean());
            AddColumn("dbo.tbProduto", "EstoqueMinimo", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.tbProduto", "EstoqueIdeal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.tbProduto", "EstoqueAtual", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.tbProduto", "EstoqueData", c => c.DateTime());
            AddColumn("dbo.tbClassificacaoFiscal", "CEST", c => c.String());
            AddColumn("dbo.tbAreaImpressao", "IDPDV", c => c.Int());
            AlterColumn("dbo.tbPedidoProduto", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AlterColumn("dbo.tbEntradaSaida", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AlterColumn("dbo.tbInventarioProdutos", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AlterColumn("dbo.tbInventarioProdutos", "QuantidadeAnterior", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AlterColumn("dbo.tbConversaoUnidade", "Divisao", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AlterColumn("dbo.tbConversaoUnidade", "Multiplicacao", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AlterColumn("dbo.tbMovimentacaoProdutos", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AlterColumn("dbo.tbProdutoReceita", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            DropColumn("dbo.tbPedido", "CodigoEmenu");
            DropColumn("dbo.tbPedido", "SincEMENU");
            DropColumn("dbo.tbProduto", "ValorAliquota");
            DropColumn("dbo.tbProduto", "Quantidade");
            DropColumn("dbo.tbProduto", "CodigoIntegracao1");
            DropColumn("dbo.tbProduto", "IDIntegracao1");
            DropColumn("dbo.tbProduto", "DescricaoIntegracao1");
            DropColumn("dbo.tbProduto", "CodigoIntegracao2");
            DropColumn("dbo.tbProduto", "IDIntegracao2");
            DropColumn("dbo.tbProduto", "DescricaoIntegracao2");
            DropColumn("dbo.tbUsuario", "CodigoEMENU");
            DropTable("dbo.tbAcao");
            DropTable("dbo.tbIntegracaoSAT");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tbIntegracaoSAT",
                c => new
                    {
                        IDIntegracaoSAT = c.Int(nullable: false, identity: true),
                        IDPDV = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDIntegracaoSAT);
            
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
                .PrimaryKey(t => t.IDAcao);
            
            AddColumn("dbo.tbUsuario", "CodigoEMENU", c => c.String(maxLength: 50));
            AddColumn("dbo.tbProduto", "DescricaoIntegracao2", c => c.String(maxLength: 200));
            AddColumn("dbo.tbProduto", "IDIntegracao2", c => c.String(maxLength: 50));
            AddColumn("dbo.tbProduto", "CodigoIntegracao2", c => c.String(maxLength: 50));
            AddColumn("dbo.tbProduto", "DescricaoIntegracao1", c => c.String(maxLength: 200));
            AddColumn("dbo.tbProduto", "IDIntegracao1", c => c.String(maxLength: 50));
            AddColumn("dbo.tbProduto", "CodigoIntegracao1", c => c.String(maxLength: 50));
            AddColumn("dbo.tbProduto", "Quantidade", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.tbProduto", "ValorAliquota", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.tbPedido", "SincEMENU", c => c.Boolean());
            AddColumn("dbo.tbPedido", "CodigoEmenu", c => c.String(maxLength: 50));
            AlterColumn("dbo.tbProdutoReceita", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.tbMovimentacaoProdutos", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.tbConversaoUnidade", "Multiplicacao", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.tbConversaoUnidade", "Divisao", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.tbInventarioProdutos", "QuantidadeAnterior", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.tbInventarioProdutos", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.tbEntradaSaida", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.tbPedidoProduto", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.tbAreaImpressao", "IDPDV");
            DropColumn("dbo.tbClassificacaoFiscal", "CEST");
            DropColumn("dbo.tbProduto", "EstoqueData");
            DropColumn("dbo.tbProduto", "EstoqueAtual");
            DropColumn("dbo.tbProduto", "EstoqueIdeal");
            DropColumn("dbo.tbProduto", "EstoqueMinimo");
            DropColumn("dbo.tbProduto", "DisponibilidadeModificacao ");
            DropColumn("dbo.tbProduto", "FrequenciaCompra");
            DropColumn("dbo.tbProduto", "DataUltimaCompra");
            DropColumn("dbo.tbProduto", "ValorUltimaCompra");
            DropColumn("dbo.tbProduto", "IsentoTaxaServico");
            DropColumn("dbo.tbProduto", "Cor");
            DropColumn("dbo.tbProduto", "TAG");
            DropColumn("dbo.tbCategoriaProduto", "Ativo");
            DropColumn("dbo.tbCategoriaProduto", "IDTipoCategoria");
            DropColumn("dbo.tbCategoriaProduto", "TAG");
            DropColumn("dbo.tbCategoriaProduto", "IDImagem");
            DropColumn("dbo.tbCategoriaProduto", "Cor");
            DropColumn("dbo.tbPedidoProduto", "Viagem");
            DropColumn("dbo.tbPedido", "Clima");
            DropColumn("dbo.tbPedido", "TAG");
            DropColumn("dbo.tbPedido", "ObservacaoCupom");
            DropColumn("dbo.tbPedido", "LocalEntrega");
            DropColumn("dbo.tbPedido", "EmailCliente");
            DropColumn("dbo.tbPedido", "NomeCliente");
            DropTable("dbo.tbHorarioDelivery");
            DropTable("dbo.tbMensagem");
            CreateIndex("dbo.tbIntegracaoSAT", "IDPDV");
            CreateIndex("dbo.tbAcao", "IDPDV");
            AddForeignKey("dbo.tbIntegracaoSAT", "IDPDV", "dbo.tbPDV", "IDPDV", cascadeDelete: true);
            AddForeignKey("dbo.tbAcao", "IDPDV", "dbo.tbPDV", "IDPDV", cascadeDelete: true);
        }
    }
}
