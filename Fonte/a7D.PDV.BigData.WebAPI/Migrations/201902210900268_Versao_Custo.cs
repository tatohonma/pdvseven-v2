namespace a7D.PDV.BigData.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Versao_Custo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entidade", "Versao", c => c.String());
            AddColumn("dbo.Produto", "Custo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Usuario", "Email");
            DropColumn("dbo.Usuario", "Apelido");
            DropTable("dbo.Perguntas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Perguntas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Texto = c.String(nullable: false, maxLength: 1000),
                        Luis = c.String(maxLength: 50),
                        Intencao = c.String(maxLength: 50),
                        Score = c.Double(nullable: false),
                        Entidade1 = c.String(maxLength: 50),
                        Valor1 = c.String(maxLength: 100),
                        Entidade2 = c.String(maxLength: 50),
                        Valor2 = c.String(maxLength: 100),
                        Entidade3 = c.String(maxLength: 50),
                        Valor3 = c.String(maxLength: 100),
                        QTD = c.Double(nullable: false),
                        Criacao = c.DateTime(nullable: false),
                        UltimaAtualizacao = c.DateTime(nullable: false),
                        UltimoUso = c.DateTime(nullable: false),
                        Retorno = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Usuario", "Apelido", c => c.String());
            AddColumn("dbo.Usuario", "Email", c => c.String());
            DropColumn("dbo.Produto", "Custo");
            DropColumn("dbo.Entidade", "Versao");
        }
    }
}
