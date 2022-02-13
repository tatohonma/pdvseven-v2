using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbStatusPedidoMap : EntityTypeConfiguration<tbStatusPedido>
    {
        public tbStatusPedidoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDStatusPedido);

            // Properties
            this.Property(t => t.IDStatusPedido)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.IDStatusPedido).HasColumnName("IDStatusPedido");
            this.Property(t => t.Nome).HasColumnName("Nome");

            // Table & Column Mappings
            this.ToTable("tbStatusPedido");
        }
    }
}
