using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoEntradaMap : EntityTypeConfiguration<tbTipoEntrada>
    {
        public tbTipoEntradaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoEntrada);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbTipoEntrada");
            this.Property(t => t.IDTipoEntrada).HasColumnName("IDTipoEntrada");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.ValorEntrada).HasColumnName("ValorEntrada");
            this.Property(t => t.ValorConsumacaoMinima).HasColumnName("ValorConsumacaoMinima");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.Padrao).HasColumnName("Padrao");
        }
    }
}
