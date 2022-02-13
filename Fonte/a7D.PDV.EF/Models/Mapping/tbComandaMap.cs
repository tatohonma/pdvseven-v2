using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbComandaMap : EntityTypeConfiguration<tbComanda>
    {
        public tbComandaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDComanda);

            // Properties
            this.Property(t => t.GUIDIdentificacao)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Observacao)
                .HasMaxLength(5000);

            // Table & Column Mappings
            this.ToTable("tbComanda");
            this.Property(t => t.IDComanda).HasColumnName("IDComanda");
            this.Property(t => t.IDStatusComanda).HasColumnName("IDStatusComanda");
            this.Property(t => t.GUIDIdentificacao).HasColumnName("GUIDIdentificacao");
            this.Property(t => t.Numero).HasColumnName("Numero");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
            this.Property(t => t.IDCliente).HasColumnName("IDCliente");
            this.Property(t => t.Observacao).HasColumnName("Observacao");

            // Relationships
            this.HasRequired(t => t.tbStatusComanda)
                .WithMany(t => t.tbComandas)
                .HasForeignKey(d => d.IDStatusComanda);

        }
    }
}
