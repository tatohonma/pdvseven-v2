using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbPainelModificacaoOperacaoMap : EntityTypeConfiguration<tbPainelModificacaoOperacao>
    {
        public tbPainelModificacaoOperacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDPainelModificacaoOperacao);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbPainelModificacaoOperacao");
            this.Property(t => t.IDPainelModificacaoOperacao).HasColumnName("IDPainelModificacaoOperacao");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
