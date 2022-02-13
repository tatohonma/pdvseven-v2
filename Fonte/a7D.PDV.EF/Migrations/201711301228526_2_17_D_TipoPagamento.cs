namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_17_D_TipoPagamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbTipoPagamento", "IDContaRecebivel", c => c.Int(nullable: true));
            CreateIndex("dbo.tbTipoPagamento", "IDContaRecebivel");
            AddForeignKey("dbo.tbTipoPagamento", "IDContaRecebivel", "dbo.tbContaRecebivel", "IDContaRecebivel", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbTipoPagamento", "IDContaRecebivel", "dbo.tbContaRecebivel");
            DropIndex("dbo.tbTipoPagamento", new[] { "IDContaRecebivel" });
            DropColumn("dbo.tbTipoPagamento", "IDContaRecebivel");
        }
    }
}
