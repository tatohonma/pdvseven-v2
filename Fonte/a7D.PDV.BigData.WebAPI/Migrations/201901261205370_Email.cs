namespace a7D.PDV.BigData.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Email : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "Email");
        }
    }
}
