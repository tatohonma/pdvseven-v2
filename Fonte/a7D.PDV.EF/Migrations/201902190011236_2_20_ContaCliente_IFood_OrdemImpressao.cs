namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_20_ContaCliente_IFood_OrdemImpressao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbSaldo", "IDPedidoPagamento", c => c.Int());
            AddColumn("dbo.tbSaldo", "CodigoERP", c => c.String());
            AddColumn("dbo.tbSaldo", "Liquidado", c => c.DateTime());
            AddColumn("dbo.tbCliente", "IDDocumento2Tipo", c => c.Int());
            AddColumn("dbo.tbCliente", "Documento2", c => c.String());
            AddColumn("dbo.tbCliente", "IDiFood", c => c.String());
            AddColumn("dbo.tbCliente", "DtAlteracao", c => c.DateTime());
            AddColumn("dbo.tbPedidoPagamento", "IDGateway", c => c.Int());
            AddColumn("dbo.tbPedidoPagamento", "IDSaldoBaixa", c => c.Int());
            AddColumn("dbo.tbPedidoProduto", "IDTipoEntrada", c => c.Int());
            AddColumn("dbo.tbOrdemImpressao", "IDTipoOrdemImpressao", c => c.Int(nullable: false));
            AddColumn("dbo.tbTipoDesconto", "LimiteValor", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.tbTipoDesconto", "LimitePercentual", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.tbTipoDesconto", "NivelAcesso", c => c.Int());
            AddColumn("dbo.tbUsuario", "DireitosCaixa", c => c.Int(nullable: false));
            AddColumn("dbo.tbUsuario", "DireitosBackOffice", c => c.Int(nullable: false));
            AddColumn("dbo.tbTaxaEntrega", "IDGateway", c => c.Int());
            AddColumn("dbo.tbTipoEntrada", "TaxaServico", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tbTipoEntrada", "LimiteComanda", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tbRelatorio", "Parametros", c => c.String());
            AlterColumn("dbo.tbComanda", "Codigo", c => c.Long());
            CreateIndex("dbo.tbSaldo", "IDPedidoPagamento");
            AddForeignKey("dbo.tbSaldo", "IDPedidoPagamento", "dbo.tbPedidoPagamento", "IDPedidoPagamento");
            DropColumn("dbo.tbOrdemImpressao", "EnviadoFilaImpressao");
            DropColumn("dbo.tbOrdemImpressao", "Conta");
            DropColumn("dbo.tbOrdemImpressao", "SAT");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbOrdemImpressao", "SAT", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbOrdemImpressao", "Conta", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbOrdemImpressao", "EnviadoFilaImpressao", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.tbSaldo", "IDPedidoPagamento", "dbo.tbPedidoPagamento");
            DropIndex("dbo.tbSaldo", new[] { "IDPedidoPagamento" });
            AlterColumn("dbo.tbComanda", "Codigo", c => c.Int());
            DropColumn("dbo.tbRelatorio", "Parametros");
            DropColumn("dbo.tbTipoEntrada", "LimiteComanda");
            DropColumn("dbo.tbTipoEntrada", "TaxaServico");
            DropColumn("dbo.tbTaxaEntrega", "IDGateway");
            DropColumn("dbo.tbUsuario", "DireitosBackOffice");
            DropColumn("dbo.tbUsuario", "DireitosCaixa");
            DropColumn("dbo.tbTipoDesconto", "NivelAcesso");
            DropColumn("dbo.tbTipoDesconto", "LimitePercentual");
            DropColumn("dbo.tbTipoDesconto", "LimiteValor");
            DropColumn("dbo.tbOrdemImpressao", "IDTipoOrdemImpressao");
            DropColumn("dbo.tbPedidoProduto", "IDTipoEntrada");
            DropColumn("dbo.tbPedidoPagamento", "IDSaldoBaixa");
            DropColumn("dbo.tbPedidoPagamento", "IDGateway");
            DropColumn("dbo.tbCliente", "DtAlteracao");
            DropColumn("dbo.tbCliente", "IDiFood");
            DropColumn("dbo.tbCliente", "Documento2");
            DropColumn("dbo.tbCliente", "IDDocumento2Tipo");
            DropColumn("dbo.tbSaldo", "Liquidado");
            DropColumn("dbo.tbSaldo", "CodigoERP");
            DropColumn("dbo.tbSaldo", "IDPedidoPagamento");
        }
    }
}
