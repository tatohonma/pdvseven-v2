namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_25_tbFaturaPixConta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbFaturaPixConta",
                c => new
                    {
                        IDFaturaPixConta = c.Int(nullable: false, identity: true),
                        IDPedido = c.Int(nullable: false),
                        CodigoFatura = c.String(maxLength: 50),
                        ChavePix = c.String(),
                        Status = c.String(maxLength: 10),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DtInclusao = c.DateTime(nullable: false),
                        DtUltimaAlteracao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDFaturaPixConta)
                .ForeignKey("dbo.tbPedido", t => t.IDPedido, cascadeDelete: true)
                .Index(t => t.IDPedido);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbFaturaPixConta", "IDPedido", "dbo.tbPedido");
            DropIndex("dbo.tbFaturaPixConta", new[] { "IDPedido" });
            DropTable("dbo.tbFaturaPixConta");
        }
    }
}
