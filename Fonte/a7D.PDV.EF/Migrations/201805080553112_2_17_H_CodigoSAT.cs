namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_17_H_CodigoSAT : DbMigration
    {
        public override void Up()
        {
            //DropTable("dbo.tbCodigoSAT");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.tbCodigoSAT",
            //    c => new
            //        {
            //            IDCodigoSAT = c.Int(nullable: false, identity: true),
            //            CodigoRetorno = c.String(nullable: false, maxLength: 6),
            //            Grupo = c.String(maxLength: 50),
            //            Mensagem = c.String(),
            //            Descricao = c.String(),
            //            Erro = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.IDCodigoSAT);
        }
    }
}
