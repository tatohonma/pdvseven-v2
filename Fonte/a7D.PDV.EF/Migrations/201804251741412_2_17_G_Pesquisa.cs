namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_17_G_Pesquisa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbPesquisa",
                c => new
                    {
                        IDPesquisa = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Valor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDPesquisa);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbPesquisa");
        }
    }
}
