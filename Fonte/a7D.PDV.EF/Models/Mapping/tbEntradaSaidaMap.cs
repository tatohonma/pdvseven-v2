using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbEntradaSaidaMap : EntityTypeConfiguration<tbEntradaSaida>
    {
        public tbEntradaSaidaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDEntradaSaida);

            // Properties
            this.Property(t => t.GUID_Origem)
                .IsFixedLength()
                .HasMaxLength(36);

            // Table & Column Mappings
            this.ToTable("tbEntradaSaida");
            this.Property(t => t.IDEntradaSaida).HasColumnName("IDEntradaSaida");
            this.Property(t => t.GUID_Origem).HasColumnName("GUID_Origem");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.Entrada).HasColumnName("Entrada");
            this.Property(t => t.Quantidade).HasColumnName("Quantidade").HasPrecision(18, 3);
            this.Property(t => t.Data).HasColumnName("Data");

            // Relationships
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbEntradaSaidas)
                .HasForeignKey(d => d.IDProduto);

        }
    }
}
