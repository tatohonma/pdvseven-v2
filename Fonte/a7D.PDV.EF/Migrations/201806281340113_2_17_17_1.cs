namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_17_17_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbComanda", "Codigo", c => c.Int());
            AddColumn("dbo.tbComanda", "IDCliente", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbComanda", "IDCliente");
            DropColumn("dbo.tbComanda", "Codigo");
        }
    }
}
