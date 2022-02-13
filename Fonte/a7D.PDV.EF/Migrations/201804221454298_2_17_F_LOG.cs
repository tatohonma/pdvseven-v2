namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_17_F_LOG : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbLOG",
                c => new
                    {
                        IDLOG = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Aplicacao = c.String(nullable: false, maxLength: 50),
                        Versao = c.String(nullable: false, maxLength: 16),
                        Codigo = c.String(nullable: false, maxLength: 4),
                        Titulo = c.String(nullable: false, maxLength: 200),
                        Dados = c.String(unicode: false, storeType: "text"),
                        IDUsuario = c.Int(),
                        IDPDV = c.Int(),
                    })
                .PrimaryKey(t => t.IDLOG);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbLOG");
        }
    }
}
