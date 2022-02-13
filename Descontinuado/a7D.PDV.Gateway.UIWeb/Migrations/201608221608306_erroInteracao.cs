namespace a7D.PDV.Gateway.UIWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class erroInteracao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InteracoesPagarMe", "Capturado", c => c.Boolean(nullable: false));
            AddColumn("dbo.InteracoesPagarMe", "Erro", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InteracoesPagarMe", "Erro");
            DropColumn("dbo.InteracoesPagarMe", "Capturado");
        }
    }
}
