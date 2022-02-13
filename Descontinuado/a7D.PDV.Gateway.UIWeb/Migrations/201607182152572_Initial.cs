namespace a7D.PDV.Gateway.UIWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        IdBroker = c.String(nullable: false, maxLength: 100),
                        Nome = c.String(),
                        CpfCnpj = c.String(),
                    })
                .PrimaryKey(t => t.IdCliente)
                .Index(t => t.IdBroker, unique: true, name: "IX_UniqueIdBroker");
            
            CreateTable(
                "dbo.ContasReceber",
                c => new
                    {
                        IdContaReceber = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Vencimento = c.DateTime(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Saldo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdErp = c.String(),
                        IdSituacao = c.Int(nullable: false),
                        Historico = c.String(),
                        IdBroker = c.String(),
                        IdCliente = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdContaReceber)
                .ForeignKey("dbo.Clientes", t => t.IdCliente, cascadeDelete: true)
                .ForeignKey("dbo.Situacoes", t => t.IdSituacao, cascadeDelete: true)
                .Index(t => t.IdSituacao)
                .Index(t => t.IdCliente);
            
            CreateTable(
                "dbo.Situacoes",
                c => new
                    {
                        IdSituacao = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.IdSituacao);
            
            CreateTable(
                "dbo.ItensPedido",
                c => new
                    {
                        IdItemPedido = c.Int(nullable: false, identity: true),
                        IdPedido = c.Int(nullable: false),
                        IdContaReceber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdItemPedido)
                .ForeignKey("dbo.ContasReceber", t => t.IdContaReceber, cascadeDelete: true)
                .ForeignKey("dbo.Pedidos", t => t.IdPedido, cascadeDelete: true)
                .Index(t => t.IdPedido)
                .Index(t => t.IdContaReceber);
            
            CreateTable(
                "dbo.Pedidos",
                c => new
                    {
                        IdPedido = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        IdTransacao = c.Int(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorPago = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataVencimentoBoleto = c.DateTime(),
                        UrlBoleto = c.String(),
                        CodigoBarrasBoleto = c.String(),
                    })
                .PrimaryKey(t => t.IdPedido);
            
            CreateTable(
                "dbo.InteracoesPagarMe",
                c => new
                    {
                        IdInteracaoPagarMe = c.Int(nullable: false, identity: true),
                        Conteudo = c.String(),
                        IdPedido = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdInteracaoPagarMe)
                .ForeignKey("dbo.Pedidos", t => t.IdPedido, cascadeDelete: true)
                .Index(t => t.IdPedido);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItensPedido", "IdPedido", "dbo.Pedidos");
            DropForeignKey("dbo.InteracoesPagarMe", "IdPedido", "dbo.Pedidos");
            DropForeignKey("dbo.ItensPedido", "IdContaReceber", "dbo.ContasReceber");
            DropForeignKey("dbo.ContasReceber", "IdSituacao", "dbo.Situacoes");
            DropForeignKey("dbo.ContasReceber", "IdCliente", "dbo.Clientes");
            DropIndex("dbo.InteracoesPagarMe", new[] { "IdPedido" });
            DropIndex("dbo.ItensPedido", new[] { "IdContaReceber" });
            DropIndex("dbo.ItensPedido", new[] { "IdPedido" });
            DropIndex("dbo.ContasReceber", new[] { "IdCliente" });
            DropIndex("dbo.ContasReceber", new[] { "IdSituacao" });
            DropIndex("dbo.Clientes", "IX_UniqueIdBroker");
            DropTable("dbo.InteracoesPagarMe");
            DropTable("dbo.Pedidos");
            DropTable("dbo.ItensPedido");
            DropTable("dbo.Situacoes");
            DropTable("dbo.ContasReceber");
            DropTable("dbo.Clientes");
        }
    }
}
