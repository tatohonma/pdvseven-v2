using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTaxaEntregaMap : EntityTypeConfiguration<tbTaxaEntrega>
    {
        public tbTaxaEntregaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTaxaEntrega);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.IDTaxaEntrega).HasColumnName("IDTaxaEntrega");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
            this.Property(t => t.IDTamanhoPacote).HasColumnName("IDTamanhoPacote").IsOptional();

            // Table & Column Mappings
            this.ToTable("tbTaxaEntrega");
        }
    }
}
