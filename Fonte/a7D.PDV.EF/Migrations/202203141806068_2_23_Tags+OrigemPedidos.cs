namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_23_TagsOrigemPedidos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbOrigemPedido",
                c => new
                    {
                        IDOrigemPedido = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDOrigemPedido);
            
            CreateTable(
                "dbo.tbTag",
                c => new
                    {
                        IDTag = c.Int(nullable: false, identity: true),
                        GUIDIdentificacao = c.String(nullable: false, maxLength: 50),
                        Chave = c.String(nullable: false, maxLength: 50),
                        Valor = c.String(),
                        DtInclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDTag);
            
            AddColumn("dbo.tbPedido", "IDOrigemPedido", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbPedido", "IDOrigemPedido");
            DropTable("dbo.tbTag");
            DropTable("dbo.tbOrigemPedido");
        }
    }
}
