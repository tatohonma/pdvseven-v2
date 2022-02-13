namespace a7D.PDV.BigData.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entidade",
                c => new
                {
                    IDEntidade = c.Int(nullable: false, identity: true),
                    ChaveAtivacao = c.String(nullable: false),
                    Estabelecimento = c.String(nullable: false),
                    CNPJ = c.String(),
                    RazaoSocial = c.String(),
                    UltimoSincronismo = c.DateTime(),
                })
                .PrimaryKey(t => t.IDEntidade);

            CreateTable(
                "dbo.Usuario",
                c => new
                {
                    IDEntidade = c.Int(nullable: false),
                    IDUsuario = c.Int(nullable: false),
                    Nome = c.String(),
                    Senha = c.String(),
                    dtAlteracao = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.IDEntidade, t.IDUsuario })
                .ForeignKey("dbo.Entidade", t => t.IDEntidade, cascadeDelete: true)
                .Index(t => t.IDEntidade);

            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        IDEntidade = c.Int(nullable: false),
                        IDCliente = c.Int(nullable: false),
                        NomeCompleto = c.String(nullable: false),
                        DataNascimento = c.DateTime(),
                        Sexo = c.String(),
                        dtAlteracao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.IDEntidade, t.IDCliente })
                .ForeignKey("dbo.Entidade", t => t.IDEntidade, cascadeDelete: true)
                .Index(t => t.IDEntidade);

            CreateTable(
                "dbo.Produto",
                c => new
                {
                    IDEntidade = c.Int(nullable: false),
                    IDProduto = c.Int(nullable: false),
                    Nome = c.String(nullable: false),
                    EAN = c.String(),
                    Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Ativo = c.Boolean(nullable: false),
                    ControlarEstoque = c.Boolean(nullable: false),
                    EstoqueMinimo = c.Decimal(nullable: false, precision: 18, scale: 3),
                    EstoqueIdeal = c.Decimal(nullable: false, precision: 18, scale: 3),
                    EstoqueAtual = c.Decimal(nullable: false, precision: 18, scale: 3),
                    dtAlteracao = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => new { t.IDEntidade, t.IDProduto })
                .ForeignKey("dbo.Entidade", t => t.IDEntidade, cascadeDelete: true)
                .Index(t => t.IDEntidade);

            CreateTable(
               "dbo.TipoPagamento",
               c => new
               {
                   IDEntidade = c.Int(nullable: false),
                   IDTipoPagamento = c.Int(nullable: false),
                   Nome = c.String(nullable: false),
                   dtAlteracao = c.DateTime(nullable: false),
               })
               .PrimaryKey(t => new { t.IDEntidade, t.IDTipoPagamento })
               .ForeignKey("dbo.Entidade", t => t.IDEntidade, cascadeDelete: true)
               .Index(t => t.IDEntidade);

            CreateTable(
                "dbo.Pedido",
                c => new
                {
                    IDEntidade = c.Int(nullable: false),
                    IDPedido = c.Int(nullable: false),
                    IDCliente = c.Int(),
                    DtPedidoFechamento = c.DateTime(nullable: false),
                    ValorTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ValorDesconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ValorEntrega = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => new { t.IDEntidade, t.IDPedido })
                .ForeignKey("dbo.Cliente", t => new { t.IDEntidade, t.IDCliente })
                .ForeignKey("dbo.Entidade", t => t.IDEntidade, cascadeDelete: true)
                .Index(t => new { t.IDEntidade, t.IDCliente });

            CreateTable(
                "dbo.PedidoPagamento",
                c => new
                    {
                        IDEntidade = c.Int(nullable: false),
                        IDPedidoPagamento = c.Int(nullable: false),
                        IDPedido = c.Int(nullable: false),
                        IDTipoPagamento = c.Int(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.IDEntidade, t.IDPedidoPagamento })
                .ForeignKey("dbo.Entidade", t => t.IDEntidade, cascadeDelete: false)
                .ForeignKey("dbo.Pedido", t => new { t.IDEntidade, t.IDPedido }, cascadeDelete: true)
                .Index(t => t.IDEntidade)
                .Index(t => new { t.IDEntidade, t.IDPedido });
            
            
            CreateTable(
                "dbo.PedidoProduto",
                c => new
                    {
                        IDEntidade = c.Int(nullable: false),
                        IDPedidoProduto = c.Int(nullable: false),
                        IDPedido = c.Int(nullable: false),
                        IDProduto = c.Int(nullable: false),
                        IDPedidoProduto_pai = c.Int(),
                        Quantidade = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ValorUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DtInclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.IDEntidade, t.IDPedidoProduto })
                .ForeignKey("dbo.Entidade", t => t.IDEntidade, cascadeDelete: false)
                .ForeignKey("dbo.Pedido", t => new { t.IDEntidade, t.IDPedido }, cascadeDelete: true)
                .Index(t => t.IDEntidade)
                .Index(t => new { t.IDEntidade, t.IDPedido });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuario", "IDEntidade", "dbo.Entidade");
            DropForeignKey("dbo.TipoPagamento", "IDEntidade", "dbo.Entidade");
            DropForeignKey("dbo.Produto", "IDEntidade", "dbo.Entidade");
            DropForeignKey("dbo.PedidoProduto", new[] { "IDEntidade", "IDPedido" }, "dbo.Pedido");
            DropForeignKey("dbo.PedidoProduto", "IDEntidade", "dbo.Entidade");
            DropForeignKey("dbo.PedidoPagamento", new[] { "IDEntidade", "IDPedido" }, "dbo.Pedido");
            DropForeignKey("dbo.Pedido", "IDEntidade", "dbo.Entidade");
            DropForeignKey("dbo.Pedido", new[] { "IDEntidade", "IDCliente" }, "dbo.Cliente");
            DropForeignKey("dbo.PedidoPagamento", "IDEntidade", "dbo.Entidade");
            DropForeignKey("dbo.Cliente", "IDEntidade", "dbo.Entidade");

            DropIndex("dbo.Usuario", new[] { "IDEntidade" });
            DropIndex("dbo.TipoPagamento", new[] { "IDEntidade" });
            DropIndex("dbo.Produto", new[] { "IDEntidade" });
            DropIndex("dbo.PedidoProduto", new[] { "IDEntidade", "IDPedido" });
            DropIndex("dbo.PedidoProduto", new[] { "IDEntidade" });
            DropIndex("dbo.Pedido", new[] { "IDEntidade", "IDCliente" });
            DropIndex("dbo.PedidoPagamento", new[] { "IDEntidade", "IDPedido" });
            DropIndex("dbo.PedidoPagamento", new[] { "IDEntidade" });
            DropIndex("dbo.Cliente", new[] { "IDEntidade" });
            DropTable("dbo.Usuario");
            DropTable("dbo.TipoPagamento");
            DropTable("dbo.Produto");
            DropTable("dbo.PedidoProduto");
            DropTable("dbo.Pedido");
            DropTable("dbo.PedidoPagamento");
            DropTable("dbo.Entidade");
            DropTable("dbo.Cliente");
        }
    }
}
