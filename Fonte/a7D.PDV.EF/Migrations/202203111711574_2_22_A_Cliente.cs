namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_22_A_Cliente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbCliente", "TAG", c => c.String());
            DropColumn("dbo.tbCliente", "IDDocumento2Tipo");
            DropColumn("dbo.tbCliente", "Documento2");
            DropColumn("dbo.tbCliente", "IDiFood");
            DropColumn("dbo.tbCliente", "DtAlteracao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbCliente", "DtAlteracao", c => c.DateTime());
            AddColumn("dbo.tbCliente", "IDiFood", c => c.String());
            AddColumn("dbo.tbCliente", "Documento2", c => c.String());
            AddColumn("dbo.tbCliente", "IDDocumento2Tipo", c => c.Int());
            DropColumn("dbo.tbCliente", "TAG");
        }
    }
}
