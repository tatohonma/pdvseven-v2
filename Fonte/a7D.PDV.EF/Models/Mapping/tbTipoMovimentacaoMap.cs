using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoMovimentacaoMap : EntityTypeConfiguration<tbTipoMovimentacao>
    {
        public tbTipoMovimentacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoMovimentacao);

            // Properties
            this.Property(t => t.Tipo)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Descricao)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("tbTipoMovimentacao");
            this.Property(t => t.IDTipoMovimentacao).HasColumnName("IDTipoMovimentacao");
            this.Property(t => t.Tipo).HasColumnName("Tipo");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
        }
    }
}
