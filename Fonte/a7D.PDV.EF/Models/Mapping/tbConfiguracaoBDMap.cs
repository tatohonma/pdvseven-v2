using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbConfiguracaoBDMap : EntityTypeConfiguration<tbConfiguracaoBD>
    {
        public tbConfiguracaoBDMap()
        {
            // Primary Key
            this.HasKey(t => t.IDConfiguracaoBD);

            // Properties
            this.Property(t => t.Chave)
                .HasMaxLength(100);

            this.Property(t => t.Valor)
                .HasMaxLength(1000);

            this.Property(t => t.ValoresAceitos)
                .HasMaxLength(1000);

            this.Property(t => t.Titulo)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("tbConfiguracaoBD");
            this.Property(t => t.IDConfiguracaoBD).HasColumnName("IDConfiguracaoBD");
            this.Property(t => t.IDTipoPDV).HasColumnName("IDTipoPDV");
            this.Property(t => t.IDPDV).HasColumnName("IDPDV");
            this.Property(t => t.Chave).HasColumnName("Chave");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.ValoresAceitos).HasColumnName("ValoresAceitos");
            this.Property(t => t.Obrigatorio).HasColumnName("Obrigatorio");
            this.Property(t => t.Titulo).HasColumnName("Titulo");

            // Relationships
            this.HasOptional(t => t.PDV)
                .WithMany(t => t.tbConfiguracaoBDs)
                .HasForeignKey(d => d.IDPDV);
            this.HasOptional(t => t.TipoPDV)
                .WithMany(t => t.tbConfiguracaoBDs)
                .HasForeignKey(d => d.IDTipoPDV);

        }
    }
}
