namespace a7D.PDV.Gateway.UIWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoverSituacao : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContasReceber", "IdSituacao", "dbo.Situacoes");
            DropIndex("dbo.ContasReceber", new[] { "IdSituacao" });
            DropColumn("dbo.ContasReceber", "IdSituacao");
            DropTable("dbo.Situacoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Situacoes",
                c => new
                    {
                        IdSituacao = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.IdSituacao);
            
            AddColumn("dbo.ContasReceber", "IdSituacao", c => c.Int(nullable: false));
            CreateIndex("dbo.ContasReceber", "IdSituacao");
            AddForeignKey("dbo.ContasReceber", "IdSituacao", "dbo.Situacoes", "IdSituacao", cascadeDelete: true);
        }
    }
}
