using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbMesaMap : EntityTypeConfiguration<tbMesa>
    {
        public tbMesaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDMesa);

            // Properties
            this.Property(t => t.GUIDIdentificacao)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbMesa");
            this.Property(t => t.IDMesa).HasColumnName("IDMesa");
            this.Property(t => t.IDStatusMesa).HasColumnName("IDStatusMesa");
            this.Property(t => t.GUIDIdentificacao).HasColumnName("GUIDIdentificacao");
            this.Property(t => t.Numero).HasColumnName("Numero");
            this.Property(t => t.Capacidade).HasColumnName("Capacidade");

            // Relationships
            this.HasRequired(t => t.tbStatusMesa)
                .WithMany(t => t.tbMesas)
                .HasForeignKey(d => d.IDStatusMesa);

        }
    }
}
