using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbPedidoMap : EntityTypeConfiguration<tbPedido>
    {
        public tbPedidoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDPedido);

            // Properties
            this.Property(t => t.GUIDIdentificacao)
                .HasMaxLength(50);

            this.Property(t => t.NumeroCupom)
                .HasMaxLength(50);

            this.Property(t => t.DocumentoCliente)
                .HasMaxLength(50);

            this.Property(t => t.GUIDAgrupamentoPedido)
                .HasMaxLength(50);

            this.Property(t => t.ReferenciaLocalizacao)
                .HasMaxLength(500);

            this.Property(t => t.GUIDMovimentacao)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbPedido");
            this.Property(t => t.IDPedido).HasColumnName("IDPedido");
            this.Property(t => t.IDCliente).HasColumnName("IDCliente");
            this.Property(t => t.IDTipoPedido).HasColumnName("IDTipoPedido");
            this.Property(t => t.IDStatusPedido).HasColumnName("IDStatusPedido");
            this.Property(t => t.IDCaixa).HasColumnName("IDCaixa");
            this.Property(t => t.IDUsuario_garcom).HasColumnName("IDUsuario_garcom");
            this.Property(t => t.IDTipoEntrada).HasColumnName("IDTipoEntrada");
            this.Property(t => t.IDRetornoSAT_venda).HasColumnName("IDRetornoSAT_venda");
            this.Property(t => t.IDRetornoSAT_cancelamento).HasColumnName("IDRetornoSAT_cancelamento");
            this.Property(t => t.IDTipoDesconto).HasColumnName("IDTipoDesconto");
            this.Property(t => t.IDTaxaEntrega).HasColumnName("IDTaxaEntrega");
            this.Property(t => t.IDEntregador).HasColumnName("IDEntregador");
            this.Property(t => t.GUIDIdentificacao).HasColumnName("GUIDIdentificacao");
            this.Property(t => t.NumeroCupom).HasColumnName("NumeroCupom");
            this.Property(t => t.DocumentoCliente).HasColumnName("DocumentoCliente");
            this.Property(t => t.DtPedido).HasColumnName("DtPedido");
            this.Property(t => t.DtPedidoFechamento).HasColumnName("DtPedidoFechamento");
            this.Property(t => t.SincERP).HasColumnName("SincERP");
            this.Property(t => t.ValorConsumacaoMinima).HasColumnName("ValorConsumacaoMinima");
            this.Property(t => t.ValorServico).HasColumnName("ValorServico");
            this.Property(t => t.ValorDesconto).HasColumnName("ValorDesconto");
            this.Property(t => t.TaxaServicoPadrao).HasColumnName("TaxaServicoPadrao");
            this.Property(t => t.ValorTotal).HasColumnName("ValorTotal");
            this.Property(t => t.GUIDAgrupamentoPedido).HasColumnName("GUIDAgrupamentoPedido");
            this.Property(t => t.Observacoes).HasColumnName("Observacoes");
            this.Property(t => t.ReferenciaLocalizacao).HasColumnName("ReferenciaLocalizacao");
            this.Property(t => t.GUIDMovimentacao).HasColumnName("GUIDMovimentacao");
            this.Property(t => t.NumeroPessoas).HasColumnName("NumeroPessoas");
            this.Property(t => t.DtEnvio).HasColumnName("DtEnvio");
            this.Property(t => t.DtEntrega).HasColumnName("DtEntrega");
            this.Property(t => t.ValorEntrega).HasColumnName("ValorEntrega");
            this.Property(t => t.AplicarDesconto).HasColumnName("AplicarDesconto");
            this.Property(t => t.AplicarServico).HasColumnName("AplicarServico");
            this.Property(t => t.IDUsuarioDesconto).HasColumnName("IDUsuarioDesconto");
            this.Property(t => t.IDUsuarioTaxaServico).HasColumnName("IDUsuarioTaxaServico");
            this.Property(t => t.Clima).HasColumnName("Clima");
            this.Property(t => t.TAG).HasColumnName("TAG").HasMaxLength(500);
            this.Property(t => t.ObservacaoCupom).HasColumnName("ObservacaoCupom");
            this.Property(t => t.NomeCliente).HasColumnName("NomeCliente");
            this.Property(t => t.EmailCliente).HasColumnName("EmailCliente");
            this.Property(t => t.LocalEntrega).HasColumnName("LocalEntrega");

            // Relationships
            this.HasOptional(t => t.tbCaixa)
                .WithMany(t => t.tbPedidoes)
                .HasForeignKey(d => d.IDCaixa);
            this.HasOptional(t => t.tbCliente)
                .WithMany(t => t.tbPedidos)
                .HasForeignKey(d => d.IDCliente);
            this.HasOptional(t => t.tbEntregador)
                .WithMany(t => t.tbPedidoes)
                .HasForeignKey(d => d.IDEntregador);
            this.HasOptional(t => t.tbRetornoSAT)
                .WithMany(t => t.tbPedidoes)
                .HasForeignKey(d => d.IDRetornoSAT_venda);
            this.HasOptional(t => t.tbRetornoSAT1)
                .WithMany(t => t.tbPedidoes1)
                .HasForeignKey(d => d.IDRetornoSAT_cancelamento);
            this.HasRequired(t => t.tbStatusPedido)
                .WithMany(t => t.tbPedidoes)
                .HasForeignKey(d => d.IDStatusPedido);
            this.HasOptional(t => t.tbTaxaEntrega)
                .WithMany(t => t.tbPedidoes)
                .HasForeignKey(d => d.IDTaxaEntrega);
            this.HasOptional(t => t.tbTipoDesconto)
                .WithMany(t => t.tbPedidoes)
                .HasForeignKey(d => d.IDTipoDesconto);
            this.HasOptional(t => t.tbTipoEntrada)
                .WithMany(t => t.tbPedidoes)
                .HasForeignKey(d => d.IDTipoEntrada);
            this.HasRequired(t => t.tbTipoPedido)
                .WithMany(t => t.tbPedidoes)
                .HasForeignKey(d => d.IDTipoPedido);
            this.HasOptional(t => t.tbUsuario)
                .WithMany(t => t.tbPedidoes)
                .HasForeignKey(d => d.IDUsuario_garcom);
            this.HasOptional(t => t.tbUsuario1)
                .WithMany(t => t.tbPedidoes1)
                .HasForeignKey(d => d.IDUsuarioDesconto);
            this.HasOptional(t => t.tbUsuario2)
                .WithMany(t => t.tbPedidoes2)
                .HasForeignKey(d => d.IDUsuarioTaxaServico);

        }
    }
}
