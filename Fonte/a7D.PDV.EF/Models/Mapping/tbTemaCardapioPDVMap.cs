using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTemaCardapioPDVMap : EntityTypeConfiguration<tbTemaCardapioPDV>
    {
        public tbTemaCardapioPDVMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTemaCardapioPDV);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbTemaCardapioPDV");
            this.Property(t => t.IDTemaCardapioPDV).HasColumnName("IDTemaCardapioPDV");
            this.Property(t => t.IDTemaCardapio).HasColumnName("IDTemaCardapio");
            this.Property(t => t.IDPDV).HasColumnName("IDPDV");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DtUltimaAlteracao).HasColumnName("DtUltimaAlteracao");

            // Relationships
            this.HasOptional(t => t.tbPDV)
                .WithMany(t => t.tbTemaCardapioPDVs)
                .HasForeignKey(d => d.IDPDV);
            this.HasRequired(t => t.tbTemaCardapio)
                .WithMany(t => t.tbTemaCardapioPDVs)
                .HasForeignKey(d => d.IDTemaCardapio);

        }
    }
}
