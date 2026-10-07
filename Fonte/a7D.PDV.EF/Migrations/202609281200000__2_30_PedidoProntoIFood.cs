namespace a7D.PDV.EF.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _2_30_PedidoProntoIFood : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                IF EXISTS (SELECT 1 FROM dbo.tbStatusPedido WHERE IDStatusPedido = 80)
                    UPDATE dbo.tbStatusPedido SET Nome = 'Pedido Pronto' WHERE IDStatusPedido = 80;
                ELSE
                    INSERT INTO dbo.tbStatusPedido (IDStatusPedido, Nome) VALUES (80, 'Pedido Pronto');
            ");
        }

        public override void Down()
        {
            Sql(@"
                IF NOT EXISTS (SELECT 1 FROM dbo.tbPedido WHERE IDStatusPedido = 80)
                    DELETE FROM dbo.tbStatusPedido WHERE IDStatusPedido = 80;
            ");
        }
    }
}
