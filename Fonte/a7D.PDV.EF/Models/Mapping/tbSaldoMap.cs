using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    internal class tbSaldoMap : EntityTypeConfiguration<tbSaldo>
    {
        public tbSaldoMap()
        {
            this.HasKey(p => p.IDSaldo);

            this.Property(p => p.IDSaldo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Relationships
            this.HasRequired(t => t.Pedido)
                .WithMany(t => t.Saldos)
                .HasForeignKey(d => d.IDPedido);

            this.HasRequired(t => t.Cliente)
                .WithMany(t => t.Saldos)
                .HasForeignKey(d => d.IDCliente);

            this.ToTable("tbSaldo");
        }
    }
}