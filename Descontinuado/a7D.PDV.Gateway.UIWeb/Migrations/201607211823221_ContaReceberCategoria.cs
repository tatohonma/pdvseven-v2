namespace a7D.PDV.Gateway.UIWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContaReceberCategoria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContasReceber", "Categoria", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContasReceber", "Categoria");
        }
    }
}
