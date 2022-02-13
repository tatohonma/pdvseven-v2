namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_21_Ticket_Integracoes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbIntegracao",
                c => new
                    {
                        IDTipo = c.Int(nullable: false),
                        Tabela = c.String(nullable: false, maxLength: 50),
                        IDInterno = c.Int(nullable: false),
                        Operacao = c.String(maxLength: 1),
                        IDExterno = c.String(nullable: false, maxLength: 50),
                        dtMovimento = c.DateTime(nullable: false),
                        tbTipoIntegracao_IDTipoIntegracao = c.Int(),
                    })
                .PrimaryKey(t => new { t.IDTipo, t.Tabela, t.IDInterno })
                .ForeignKey("dbo.tbTipoIntegracao", t => t.tbTipoIntegracao_IDTipoIntegracao)
                .Index(t => t.tbTipoIntegracao_IDTipoIntegracao);
            
            CreateTable(
                "dbo.tbTicket",
                c => new
                    {
                        IDTicket = c.Int(nullable: false, identity: true),
                        IDPedidoProduto = c.Int(nullable: false),
                        dtUtilizacao = c.DateTime(),
                        IDUtilizacaoPDV = c.Int(),
                        IDUtilizacaoUsuario = c.Int(),
                    })
                .PrimaryKey(t => t.IDTicket);
            
            CreateTable(
                "dbo.tbTipoIntegracao",
                c => new
                    {
                        IDTipoIntegracao = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IDTipoIntegracao);
            
            AddColumn("dbo.tbPedidoProduto", "tbTicket_IDTicket", c => c.Int());
            CreateIndex("dbo.tbPedidoProduto", "tbTicket_IDTicket");
            AddForeignKey("dbo.tbPedidoProduto", "tbTicket_IDTicket", "dbo.tbTicket", "IDTicket");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbIntegracao", "tbTipoIntegracao_IDTipoIntegracao", "dbo.tbTipoIntegracao");
            DropForeignKey("dbo.tbPedidoProduto", "tbTicket_IDTicket", "dbo.tbTicket");
            DropIndex("dbo.tbPedidoProduto", new[] { "tbTicket_IDTicket" });
            DropIndex("dbo.tbIntegracao", new[] { "tbTipoIntegracao_IDTipoIntegracao" });
            DropColumn("dbo.tbPedidoProduto", "tbTicket_IDTicket");
            DropTable("dbo.tbTipoIntegracao");
            DropTable("dbo.tbTicket");
            DropTable("dbo.tbIntegracao");
        }
    }
}
