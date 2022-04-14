namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_23_GuidIdentificacaoCliente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbCliente", "GUIDIdentificacao", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbCliente", "GUIDIdentificacao");
        }
    }
}
