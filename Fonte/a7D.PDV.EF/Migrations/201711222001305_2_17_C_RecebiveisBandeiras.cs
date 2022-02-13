namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_17_C_RecebiveisBandeiras : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbBandeira",
                c => new
                    {
                        IDBandeira = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDBandeira);
            
            CreateTable(
                "dbo.tbContaRecebivel",
                c => new
                    {
                        IDContaRecebivel = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                        CodigoIntegracao = c.String(nullable: true, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDContaRecebivel);
            
            AddColumn("dbo.tbPedidoPagamento", "IDMetodo", c => c.Int(nullable: true));
            AddColumn("dbo.tbPedidoPagamento", "IDContaRecebivel", c => c.Int(nullable: true));
            AddColumn("dbo.tbPedidoPagamento", "IDBandeira", c => c.Int(nullable: true));
            AlterColumn("dbo.tbTipoPagamento", "PrazoCredito", c => c.Int(nullable:true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbTipoPagamento", "PrazoCredito", c => c.Int(nullable: false));
            DropColumn("dbo.tbPedidoPagamento", "IDBandeira");
            DropColumn("dbo.tbPedidoPagamento", "IDContaRecebivel");
            DropColumn("dbo.tbPedidoPagamento", "IDMetodo");
            DropTable("dbo.tbContaRecebivel");
            DropTable("dbo.tbBandeira");
        }
    }
}
