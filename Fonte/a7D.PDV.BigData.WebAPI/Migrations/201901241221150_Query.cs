namespace a7D.PDV.BigData.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Query : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Querys",
                c => new
                    {
                        IDQuery = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Query = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.IDQuery);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Querys");
        }
    }
}
