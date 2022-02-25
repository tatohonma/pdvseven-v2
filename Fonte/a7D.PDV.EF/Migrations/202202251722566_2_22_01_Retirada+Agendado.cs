namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_22_01_RetiradaAgendado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbPedido", "Agendado", c => c.Boolean());
            AddColumn("dbo.tbPedido", "DtAgendamento", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbPedido", "DtAgendamento");
            DropColumn("dbo.tbPedido", "Agendado");
        }
    }
}
