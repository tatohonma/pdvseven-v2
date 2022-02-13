namespace a7D.PDV.BigData.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IaagoChanelUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChannelUser",
                c => new
                    {
                        IDChannelUser = c.Int(nullable: false, identity: true),
                        IDEntidade = c.Int(nullable: false),
                        IDUsuario = c.Int(nullable: false),
                        ChannelId = c.String(nullable: false),
                        FromId = c.String(nullable: false, maxLength: 100),
                        LastLogin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDChannelUser)
                .ForeignKey("dbo.Entidade", t => t.IDEntidade, cascadeDelete: true)
                .Index(t => t.IDEntidade);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChannelUser", "IDEntidade", "dbo.Entidade");
            DropIndex("dbo.ChannelUser", new[] { "IDEntidade" });
            DropTable("dbo.ChannelUser");
        }
    }
}
