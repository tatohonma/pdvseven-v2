using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbOrigemPedidoMap : EntityTypeConfiguration<tbOrigemPedido>
    {
        public tbOrigemPedidoMap()
        {
            this.ToTable("tbOrigemPedido");

            // Primary Key
            this.HasKey(t => t.IDOrigemPedido);

            this.Property(t => t.IDOrigemPedido)
                .HasColumnName("IDOrigemPedido");
            this.Property(t => t.Nome)
                .HasColumnName("Nome")
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
