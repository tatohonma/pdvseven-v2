namespace a7D.PDV.Gateway.UIWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PedidoPendente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContasReceber", "Pendente", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContasReceber", "Pendente");
        }
    }
}
