namespace a7D.PDV.Gateway.UIWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PedidoOpcional : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InteracoesPagarMe", "IdPedido", "dbo.Pedidos");
            DropIndex("dbo.InteracoesPagarMe", new[] { "IdPedido" });
            AlterColumn("dbo.InteracoesPagarMe", "IdPedido", c => c.Int());
            CreateIndex("dbo.InteracoesPagarMe", "IdPedido");
            AddForeignKey("dbo.InteracoesPagarMe", "IdPedido", "dbo.Pedidos", "IdPedido");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InteracoesPagarMe", "IdPedido", "dbo.Pedidos");
            DropIndex("dbo.InteracoesPagarMe", new[] { "IdPedido" });
            AlterColumn("dbo.InteracoesPagarMe", "IdPedido", c => c.Int(nullable: false));
            CreateIndex("dbo.InteracoesPagarMe", "IdPedido");
            AddForeignKey("dbo.InteracoesPagarMe", "IdPedido", "dbo.Pedidos", "IdPedido", cascadeDelete: true);
        }
    }
}
