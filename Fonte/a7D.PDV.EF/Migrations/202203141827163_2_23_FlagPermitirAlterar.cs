namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_23_FlagPermitirAlterar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbPedido", "PermitirAlterar", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbPedido", "PermitirAlterar");
        }
    }
}
