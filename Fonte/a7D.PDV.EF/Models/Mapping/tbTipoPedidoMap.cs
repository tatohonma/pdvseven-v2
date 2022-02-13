using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoPedidoMap : EntityTypeConfiguration<tbTipoPedido>
    {
        public tbTipoPedidoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoPedido);

            // Properties
            this.Property(t => t.IDTipoPedido)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.IDTipoPedido).HasColumnName("IDTipoPedido");
            this.Property(t => t.Nome).HasColumnName("Nome");

            // Table & Column Mappings
            this.ToTable("tbTipoPedido");
        }
    }
}
