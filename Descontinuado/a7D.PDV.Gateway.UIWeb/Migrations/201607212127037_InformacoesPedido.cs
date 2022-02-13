namespace a7D.PDV.Gateway.UIWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InformacoesPedido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedidos", "Parcelas", c => c.Int(nullable: false));
            AddColumn("dbo.Pedidos", "Bandeira", c => c.String());
            AddColumn("dbo.Pedidos", "UltimosDigitosCartao", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedidos", "UltimosDigitosCartao");
            DropColumn("dbo.Pedidos", "Bandeira");
            DropColumn("dbo.Pedidos", "Parcelas");
        }
    }
}
