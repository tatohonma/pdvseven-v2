using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbCategoriaProdutoMap : EntityTypeConfiguration<tbCategoriaProduto>
    {
        public tbCategoriaProdutoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDCategoriaProduto);

            // Properties
            this.Property(t => t.CodigoERP)
                .HasMaxLength(50);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbCategoriaProduto");
            this.Property(t => t.IDCategoriaProduto).HasColumnName("IDCategoriaProduto");
            this.Property(t => t.CodigoERP).HasColumnName("CodigoERP");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.DtUltimaAlteracao).HasColumnName("DtUltimaAlteracao");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
            this.Property(t => t.Disponibilidade).HasColumnName("Disponibilidade");
            this.Property(t => t.DtAlteracaoDisponibilidade).HasColumnName("DtAlteracaoDisponibilidade");
            this.Property(t => t.Cor).HasColumnName("Cor");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.IDImagem).HasColumnName("IDImagem");
            this.Property(t => t.TAG).HasColumnName("TAG").HasMaxLength(500);
            this.Property(t => t.IDTipoCategoria).HasColumnName("IDTipoCategoria");
        }
    }
}
