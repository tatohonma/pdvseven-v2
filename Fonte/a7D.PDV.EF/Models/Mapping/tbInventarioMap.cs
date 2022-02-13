using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbInventarioMap : EntityTypeConfiguration<tbInventario>
    {
        public tbInventarioMap()
        {
            // Primary Key
            this.HasKey(t => t.IDInventario);

            // Properties
            this.Property(t => t.GUID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36);

            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("tbInventario");
            this.Property(t => t.IDInventario).HasColumnName("IDInventario");
            this.Property(t => t.GUID).HasColumnName("GUID");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Processado).HasColumnName("Processado");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
        }
    }
}
