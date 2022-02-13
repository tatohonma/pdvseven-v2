using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbAreaImpressaoMap : EntityTypeConfiguration<tbAreaImpressao>
    {
        public tbAreaImpressaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDAreaImpressao);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NomeImpressora)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbAreaImpressao");
            this.Property(t => t.IDAreaImpressao).HasColumnName("IDAreaImpressao");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.NomeImpressora).HasColumnName("NomeImpressora");
            this.Property(t => t.IDPDV).HasColumnName("IDPDV");
            this.Property(t => t.IDTipoAreaImpressao).HasColumnName("IDTipoAreaImpressao");

            // Relationships
            this.HasRequired(t => t.tbTipoAreaImpressao)
                .WithMany(t => t.tbAreaImpressaos)
                .HasForeignKey(d => d.IDTipoAreaImpressao);

        }
    }
}
