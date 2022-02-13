using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoProdutoMap : EntityTypeConfiguration<tbTipoProduto>
    {
        public tbTipoProdutoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoProduto);

            // Properties
            this.Property(t => t.IDTipoProduto)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.IDTipoProduto).HasColumnName("IDTipoProduto");
            this.Property(t => t.Nome).HasColumnName("Nome");

            // Table & Column Mappings
            this.ToTable("tbTipoProduto");
        }
    }
}
