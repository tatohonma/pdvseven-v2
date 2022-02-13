namespace a7D.PDV.EF.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _2_18_Loggi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbTamanhoPacote",
                c => new
                    {
                        IDTamanhoPacote = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDTamanhoPacote);
            
            AddColumn("dbo.tbEntregador", "IDGateway", c => c.Int());
            AddColumn("dbo.tbTaxaEntrega", "IDTamanhoPacote", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbTaxaEntrega", "IDTamanhoPacote");
            DropColumn("dbo.tbEntregador", "IDGateway");
            DropTable("dbo.tbTamanhoPacote");
        }
    }
}
