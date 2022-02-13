using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbProdutoMap : EntityTypeConfiguration<tbProduto>
    {
        public tbProdutoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDProduto);

            // Properties
            this.Property(t => t.Codigo)
                .HasMaxLength(50);

            this.Property(t => t.CodigoERP)
                .HasMaxLength(50);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Descricao)
                .HasMaxLength(500);

            this.Property(t => t.CodigoAliquota)
                .HasMaxLength(50);

            this.Property(t => t.cEAN)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbProduto");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.IDTipoProduto).HasColumnName("IDTipoProduto");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
            this.Property(t => t.CodigoERP).HasColumnName("CodigoERP");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.ValorUnitario).HasColumnName("ValorUnitario");
            this.Property(t => t.CodigoAliquota).HasColumnName("CodigoAliquota");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.Disponibilidade).HasColumnName("Disponibilidade");
            this.Property(t => t.DtAlteracaoDisponibilidade).HasColumnName("DtAlteracaoDisponibilidade");
            this.Property(t => t.DtUltimaAlteracao).HasColumnName("DtUltimaAlteracao");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
            this.Property(t => t.IDClassificacaoFiscal).HasColumnName("IDClassificacaoFiscal");
            this.Property(t => t.IDUnidade).HasColumnName("IDUnidade");
            this.Property(t => t.cEAN).HasColumnName("cEAN");
            this.Property(t => t.ControlarEstoque).HasColumnName("ControlarEstoque");
            this.Property(t => t.UtilizarBalanca).HasColumnName("UtilizarBalanca");
            this.Property(t => t.ValorUnitario2).HasColumnName("ValorUnitario2");
            this.Property(t => t.ValorUnitario3).HasColumnName("ValorUnitario3");
            this.Property(t => t.AssistenteModificacoes).HasColumnName("AssistenteModificacoes");
            this.Property(t => t.EstoqueMinimo).HasColumnName("EstoqueMinimo");
            this.Property(t => t.EstoqueIdeal).HasColumnName("EstoqueIdeal");
            this.Property(t => t.EstoqueAtual).HasColumnName("EstoqueAtual");
            this.Property(t => t.EstoqueData).HasColumnName("EstoqueData");
            this.Property(t => t.ValorUltimaCompra).HasColumnName("ValorUltimaCompra");
            this.Property(t => t.DataUltimaCompra).HasColumnName("DataUltimaCompra");
            this.Property(t => t.FrequenciaCompra).HasColumnName("FrequenciaCompra");
            this.Property(t => t.TAG).HasColumnName("TAG").HasMaxLength(500);
            this.Property(t => t.Cor).HasColumnName("Cor");
            this.Property(t => t.IsentoTaxaServico).HasColumnName("IsentoTaxaServico");
            this.Property(t => t.DisponibilidadeModificacao).HasColumnName("DisponibilidadeModificacao ");

            // Relationships
            this.HasOptional(t => t.tbClassificacaoFiscal)
                .WithMany(t => t.tbProdutoes)
                .HasForeignKey(d => d.IDClassificacaoFiscal);

            this.HasRequired(t => t.tbTipoProduto)
                .WithMany(t => t.tbProdutoes)
                .HasForeignKey(d => d.IDTipoProduto);

            this.HasOptional(t => t.tbUnidade)
                .WithMany(t => t.tbProdutoes)
                .HasForeignKey(d => d.IDUnidade);

        }
    }
}
