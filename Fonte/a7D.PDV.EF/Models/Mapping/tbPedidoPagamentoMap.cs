using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbPedidoPagamentoMap : EntityTypeConfiguration<tbPedidoPagamento>
    {
        public tbPedidoPagamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDPedidoPagamento);

            // Properties
            this.Property(t => t.Autorizacao)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbPedidoPagamento");
            this.Property(t => t.IDPedidoPagamento).HasColumnName("IDPedidoPagamento");
            this.Property(t => t.IDPedido).HasColumnName("IDPedido");
            this.Property(t => t.IDTipoPagamento).HasColumnName("IDTipoPagamento");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.Autorizacao).HasColumnName("Autorizacao");
            this.Property(t => t.IDGateway).HasColumnName("IDGateway");
            this.Property(t => t.IDMetodo).HasColumnName("IDMetodo");
            this.Property(t => t.IDBandeira).HasColumnName("IDBandeira");
            this.Property(t => t.IDContaRecebivel).HasColumnName("IDContaRecebivel");


            // Relationships
            this.HasRequired(t => t.tbPedido)
                .WithMany(t => t.tbPedidoPagamentoes)
                .HasForeignKey(d => d.IDPedido);
            this.HasRequired(t => t.tbTipoPagamento)
                .WithMany(t => t.tbPedidoPagamentoes)
                .HasForeignKey(d => d.IDTipoPagamento);

        }
    }
}
