namespace a7D.PDV.BigData.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Query_Nome : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Querys", "Nome", c => c.String(nullable: false, maxLength: 100));
        }

        public override void Down()
        {
            DropColumn("dbo.Querys", "Nome");
        }
    }
}
