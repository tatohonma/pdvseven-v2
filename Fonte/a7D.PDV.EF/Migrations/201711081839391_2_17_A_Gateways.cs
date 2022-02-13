namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_17_A_Gateways : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbGateway",
                c => new
                    {
                        IDGateway = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDGateway);
            
            CreateTable(
                "dbo.tbVersao",
                c => new
                    {
                        IDVersao = c.Int(nullable: false, identity: true),
                        Versao = c.String(nullable: false, maxLength: 50),
                        Data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDVersao);
            
            AddColumn("dbo.tbTipoPagamento", "IDGateway", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbTipoPagamento", "IDGateway");
            DropTable("dbo.tbVersao");
            DropTable("dbo.tbGateway");
        }
    }
}
