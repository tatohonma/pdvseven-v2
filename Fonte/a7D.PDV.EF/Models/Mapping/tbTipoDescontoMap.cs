using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoDescontoMap : EntityTypeConfiguration<tbTipoDesconto>
    {
        public tbTipoDescontoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoDesconto);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Descricao)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("tbTipoDesconto");
            this.Property(t => t.IDTipoDesconto).HasColumnName("IDTipoDesconto");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
        }
    }
}
