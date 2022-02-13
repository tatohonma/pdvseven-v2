using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoAreaImpressaoMap : EntityTypeConfiguration<tbTipoAreaImpressao>
    {
        public tbTipoAreaImpressaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoAreaImpressao);

            // Properties
            this.Property(t => t.IDTipoAreaImpressao)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbTipoAreaImpressao");
            this.Property(t => t.IDTipoAreaImpressao).HasColumnName("IDTipoAreaImpressao");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
