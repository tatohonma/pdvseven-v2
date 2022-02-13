namespace a7D.PDV.Gateway.UIWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatasPedido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedidos", "DataAutorizacao", c => c.DateTime());
            AddColumn("dbo.Pedidos", "DataPedido", c => c.DateTime(nullable: false));
            AddColumn("dbo.Pedidos", "DataPagamento", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedidos", "DataPagamento");
            DropColumn("dbo.Pedidos", "DataPedido");
            DropColumn("dbo.Pedidos", "DataAutorizacao");
        }
    }
}
