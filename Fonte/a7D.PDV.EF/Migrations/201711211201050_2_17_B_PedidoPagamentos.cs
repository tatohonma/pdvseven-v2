namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_17_B_PedidoPagamentos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbPedidoPagamento", "DataPagamento", c => c.DateTime(nullable: true));
            AddColumn("dbo.tbPedidoPagamento", "IDUsuarioPagamento", c => c.Int(nullable: true));
            AddColumn("dbo.tbPedidoPagamento", "DataCancelado", c => c.DateTime(nullable: true));
            AddColumn("dbo.tbPedidoPagamento", "IDUsuarioCancelado", c => c.Int(nullable: true));
            AddColumn("dbo.tbPedidoPagamento", "Excluido", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbPedidoPagamento", "Excluido");
            DropColumn("dbo.tbPedidoPagamento", "IDUsuarioCancelado");
            DropColumn("dbo.tbPedidoPagamento", "DataCancelado");
            DropColumn("dbo.tbPedidoPagamento", "IDUsuarioPagamento");
            DropColumn("dbo.tbPedidoPagamento", "DataPagamento");
        }
    }
}
