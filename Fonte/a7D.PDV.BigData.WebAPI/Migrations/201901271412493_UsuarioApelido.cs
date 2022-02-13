namespace a7D.PDV.BigData.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuarioApelido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "Apelido", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "Apelido");
        }
    }
}
