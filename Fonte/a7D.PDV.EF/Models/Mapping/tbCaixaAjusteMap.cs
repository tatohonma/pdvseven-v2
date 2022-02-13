using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbCaixaAjusteMap : EntityTypeConfiguration<tbCaixaAjuste>
    {
        public tbCaixaAjusteMap()
        {
            // Primary Key
            this.HasKey(t => t.IDCaixaAjuste);

            // Properties
            this.Property(t => t.Descricao)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("tbCaixaAjuste");
            this.Property(t => t.IDCaixaAjuste).HasColumnName("IDCaixaAjuste");
            this.Property(t => t.IDCaixa).HasColumnName("IDCaixa");
            this.Property(t => t.IDCaixaTipoAjuste).HasColumnName("IDCaixaTipoAjuste");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.DtAjuste).HasColumnName("DtAjuste");

            // Relationships
            this.HasRequired(t => t.tbCaixa)
                .WithMany(t => t.tbCaixaAjustes)
                .HasForeignKey(d => d.IDCaixa);
            this.HasRequired(t => t.tbCaixaTipoAjuste)
                .WithMany(t => t.tbCaixaAjustes)
                .HasForeignKey(d => d.IDCaixaTipoAjuste);

        }
    }
}
