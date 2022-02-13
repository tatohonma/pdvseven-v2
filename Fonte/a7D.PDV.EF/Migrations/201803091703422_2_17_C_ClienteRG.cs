namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_17_C_ClienteRG : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbCliente", "RG", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbCliente", "RG");
        }
    }
}
