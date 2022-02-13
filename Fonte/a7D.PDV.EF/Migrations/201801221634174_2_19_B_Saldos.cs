namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_19_B_Saldos : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tbSaldo", "IDPedido");
            CreateIndex("dbo.tbSaldo", "IDCliente");
            AddForeignKey("dbo.tbSaldo", "IDCliente", "dbo.tbCliente", "IDCliente", cascadeDelete: false);
            AddForeignKey("dbo.tbSaldo", "IDPedido", "dbo.tbPedido", "IDPedido", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbSaldo", "IDPedido", "dbo.tbPedido");
            DropForeignKey("dbo.tbSaldo", "IDCliente", "dbo.tbCliente");
            DropIndex("dbo.tbSaldo", new[] { "IDCliente" });
            DropIndex("dbo.tbSaldo", new[] { "IDPedido" });
        }
    }
}
