using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbMovimentacaoMap : EntityTypeConfiguration<tbMovimentacao>
    {
        public tbMovimentacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDMovimentacao);

            // Properties
            this.Property(t => t.GUID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36);

            this.Property(t => t.Descricao)
                .HasMaxLength(100);

            this.Property(t => t.NumeroPedido)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbMovimentacao");
            this.Property(t => t.IDMovimentacao).HasColumnName("IDMovimentacao");
            this.Property(t => t.IDTipoMovimentacao).HasColumnName("IDTipoMovimentacao");
            this.Property(t => t.IDFornecedor).HasColumnName("IDFornecedor");
            this.Property(t => t.GUID).HasColumnName("GUID");
            this.Property(t => t.DataMovimentacao).HasColumnName("DataMovimentacao");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.NumeroPedido).HasColumnName("NumeroPedido");
            this.Property(t => t.IDMovimentacao_reversa).HasColumnName("IDMovimentacao_reversa");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
            this.Property(t => t.Reversa).HasColumnName("Reversa");
            this.Property(t => t.Processado).HasColumnName("Processado");

            // Relationships
            this.HasRequired(t => t.tbTipoMovimentacao)
                .WithMany(t => t.tbMovimentacaos)
                .HasForeignKey(d => d.IDTipoMovimentacao);

        }
    }
}
