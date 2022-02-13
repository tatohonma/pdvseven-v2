namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_17_E_TipoPagamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbTipoPagamento", "IDBandeira", c => c.Int(nullable: true));
            CreateIndex("dbo.tbTipoPagamento", "IDBandeira");
            AddForeignKey("dbo.tbTipoPagamento", "IDBandeira", "dbo.tbBandeira", "IDBandeira", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbTipoPagamento", "IDBandeira", "dbo.tbBandeira");
            DropIndex("dbo.tbTipoPagamento", new[] { "IDBandeira" });
            DropColumn("dbo.tbTipoPagamento", "IDBandeira");
        }
    }
}
