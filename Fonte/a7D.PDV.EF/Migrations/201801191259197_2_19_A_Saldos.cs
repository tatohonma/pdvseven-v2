namespace a7D.PDV.EF.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class _2_19_A_Saldos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbSaldo",
                c => new
                    {
                        IDSaldo = c.Int(nullable: false, identity: true),
                        dtMovimento = c.DateTime(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tipo = c.String(nullable: false, maxLength: 1),
                        IDPai = c.Int(nullable: false),
                        IDPedido = c.Int(nullable: false),
                        IDCliente = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDSaldo);
        }
        
        public override void Down()
        {
            DropTable("dbo.tbSaldo");
        }
    }
}
