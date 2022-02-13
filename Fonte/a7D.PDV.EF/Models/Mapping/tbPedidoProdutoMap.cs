using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbPedidoProdutoMap : EntityTypeConfiguration<tbPedidoProduto>
    {
        public tbPedidoProdutoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDPedidoProduto);

            // Properties
            this.Property(t => t.CodigoAliquota)
                .HasMaxLength(50);

            this.Property(t => t.ObservacoesCancelamento)
                .HasMaxLength(500);

            this.Property(t => t.GUIDControleDuplicidade)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbPedidoProduto");
            this.Property(t => t.IDPedidoProduto).HasColumnName("IDPedidoProduto");
            this.Property(t => t.IDPedido).HasColumnName("IDPedido");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.IDPedidoProduto_pai).HasColumnName("IDPedidoProduto_pai");
            this.Property(t => t.IDPDV).HasColumnName("IDPDV");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.IDPDV_cancelamento).HasColumnName("IDPDV_cancelamento");
            this.Property(t => t.IDUsuario_cancelamento).HasColumnName("IDUsuario_cancelamento");
            this.Property(t => t.IDMotivoCancelamento).HasColumnName("IDMotivoCancelamento");
            this.Property(t => t.Quantidade).HasColumnName("Quantidade").HasPrecision(18,3);
            this.Property(t => t.ValorUnitario).HasColumnName("ValorUnitario");
            this.Property(t => t.ValorAliquota).HasColumnName("ValorAliquota");
            this.Property(t => t.CodigoAliquota).HasColumnName("CodigoAliquota");
            this.Property(t => t.Notas).HasColumnName("Notas");
            this.Property(t => t.Cancelado).HasColumnName("Cancelado");
            this.Property(t => t.Viagem).HasColumnName("Viagem");
            this.Property(t => t.DtInclusao).HasColumnName("DtInclusao");
            this.Property(t => t.DtAlteracao).HasColumnName("DtAlteracao");
            this.Property(t => t.ObservacoesCancelamento).HasColumnName("ObservacoesCancelamento");
            this.Property(t => t.DtCancelamento).HasColumnName("DtCancelamento");
            this.Property(t => t.GUIDControleDuplicidade).HasColumnName("GUIDControleDuplicidade");
            this.Property(t => t.RetornarAoEstoque).HasColumnName("RetornarAoEstoque");
            this.Property(t => t.IDPainelModificacao).HasColumnName("IDPainelModificacao");
            this.Property(t => t.ValorDesconto).HasColumnName("ValorDesconto");
            this.Property(t => t.IDUsuarioDesconto).HasColumnName("IDUsuarioDesconto");
            this.Property(t => t.IDTipoDesconto).HasColumnName("IDTipoDesconto");

            // Relationships
            this.HasOptional(t => t.tbMotivoCancelamento)
                .WithMany(t => t.tbPedidoProdutoes)
                .HasForeignKey(d => d.IDMotivoCancelamento);
            this.HasOptional(t => t.tbPainelModificacao)
                .WithMany(t => t.tbPedidoProdutoes)
                .HasForeignKey(d => d.IDPainelModificacao);
            this.HasOptional(t => t.tbPDV)
                .WithMany(t => t.tbPedidoProdutoes)
                .HasForeignKey(d => d.IDPDV);
            this.HasOptional(t => t.tbPDV1)
                .WithMany(t => t.tbPedidoProdutoes1)
                .HasForeignKey(d => d.IDPDV_cancelamento);
            this.HasRequired(t => t.tbPedido)
                .WithMany(t => t.tbPedidoProdutoes)
                .HasForeignKey(d => d.IDPedido);
            this.HasOptional(t => t.tbPedidoProduto2)
                .WithMany(t => t.tbPedidoProduto1)
                .HasForeignKey(d => d.IDPedidoProduto_pai);
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbPedidoProdutoes)
                .HasForeignKey(d => d.IDProduto);
            this.HasOptional(t => t.tbTipoDesconto)
                .WithMany(t => t.tbPedidoProdutoes)
                .HasForeignKey(d => d.IDTipoDesconto);
            this.HasOptional(t => t.tbUsuario)
                .WithMany(t => t.tbPedidoProdutoes)
                .HasForeignKey(d => d.IDUsuarioDesconto);
            this.HasOptional(t => t.tbUsuario1)
                .WithMany(t => t.tbPedidoProdutoes1)
                .HasForeignKey(d => d.IDUsuario_cancelamento);
            this.HasOptional(t => t.tbUsuario2)
                .WithMany(t => t.tbPedidoProdutoes2)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
