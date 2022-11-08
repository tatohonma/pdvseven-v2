namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_24_GuidIdentificacaoProduto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbProduto", "GUIDIdentificacao", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbProduto", "GUIDIdentificacao");
        }
    }
}
