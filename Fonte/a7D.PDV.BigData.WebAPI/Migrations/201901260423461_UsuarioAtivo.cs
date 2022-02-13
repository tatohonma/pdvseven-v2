namespace a7D.PDV.BigData.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuarioAtivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "Ativo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "Ativo");
        }
    }
}
