using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbPainelModificacaoRelacionadoMap : EntityTypeConfiguration<tbPainelModificacaoRelacionado>
    {
        public tbPainelModificacaoRelacionadoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDPainelModificacaoRelacionado);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbPainelModificacaoRelacionado");
            this.Property(t => t.IDPainelModificacaoRelacionado).HasColumnName("IDPainelModificacaoRelacionado");
            this.Property(t => t.IDPainelModificacao1).HasColumnName("IDPainelModificacao1");
            this.Property(t => t.IDPainelModificacao2).HasColumnName("IDPainelModificacao2");
            this.Property(t => t.IgnorarValorItem).HasColumnName("IgnorarValorItem");

            // Relationships
            this.HasRequired(t => t.tbPainelModificacao)
                .WithMany(t => t.tbPainelModificacaoRelacionadoes)
                .HasForeignKey(d => d.IDPainelModificacao1);
            this.HasRequired(t => t.tbPainelModificacao1)
                .WithMany(t => t.tbPainelModificacaoRelacionadoes1)
                .HasForeignKey(d => d.IDPainelModificacao2);

        }
    }
}
