using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbPedido
    {
        public tbPedido()
        {
            this.tbPedidoProdutoes = new List<tbPedidoProduto>();
            this.tbPedidoPagamentoes = new List<tbPedidoPagamento>();
        }

        public int IDPedido { get; set; }
        public int? IDCliente { get; set; }
        public int IDTipoPedido { get; set; }
        public int IDStatusPedido { get; set; }
        public int? IDCaixa { get; set; }
        public int? IDUsuario_garcom { get; set; }
        public int? IDTipoEntrada { get; set; }
        public int? IDRetornoSAT_venda { get; set; }
        public int? IDRetornoSAT_cancelamento { get; set; }
        public int? IDTipoDesconto { get; set; }
        public int? IDTaxaEntrega { get; set; }
        public int? IDEntregador { get; set; }
        public string GUIDIdentificacao { get; set; } // Identificação única da MESA, Comanda ou IFOOD
        public string NumeroCupom { get; set; }
        public string DocumentoCliente { get; set; }
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public string LocalEntrega { get; set; }
        public DateTime? DtPedido { get; set; }
        public DateTime? DtPedidoFechamento { get; set; }
        public bool? SincERP { get; set; }
        public decimal? ValorConsumacaoMinima { get; set; }
        public decimal? ValorServico { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal? TaxaServicoPadrao { get; set; }
        public decimal? ValorTotal { get; set; }
        public string GUIDAgrupamentoPedido { get; set; } // No IFOOD é o número curto do PEDIDO 
        public string Observacoes { get; set; }
        public string ObservacaoCupom { get; set; }
        public string ReferenciaLocalizacao { get; set; }
        public string GUIDMovimentacao { get; set; }
        public int? NumeroPessoas { get; set; }
        public DateTime? DtEnvio { get; set; }
        public DateTime? DtEntrega { get; set; }
        public decimal? ValorEntrega { get; set; }
        public bool? AplicarDesconto { get; set; }
        public bool? AplicarServico { get; set; }
        public int? IDUsuarioDesconto { get; set; }
        public int? IDUsuarioTaxaServico { get; set; }
        public string TAG { get; set; }
        public string Clima { get; set; }
        public int? IDOrigemPedido { get; set; }
        public bool? PermitirAlterar { get; set; }

        public virtual tbCaixa tbCaixa { get; set; }
        public virtual tbCliente tbCliente { get; set; }
        public virtual tbEntregador tbEntregador { get; set; }
        public virtual tbRetornoSAT tbRetornoSAT { get; set; }
        public virtual tbRetornoSAT tbRetornoSAT1 { get; set; }
        public virtual tbStatusPedido tbStatusPedido { get; set; }
        public virtual tbTaxaEntrega tbTaxaEntrega { get; set; }
        public virtual tbTipoDesconto tbTipoDesconto { get; set; }
        public virtual tbTipoEntrada tbTipoEntrada { get; set; }
        public virtual tbTipoPedido tbTipoPedido { get; set; }
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
        public virtual tbUsuario tbUsuario2 { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProdutoes { get; set; }
        public virtual ICollection<tbPedidoPagamento> tbPedidoPagamentoes { get; set; }
        public virtual ICollection<tbSaldo> Saldos { get; set; }
        public virtual ICollection<tbFaturaPixConta> FaturasPixConta { get; set; }
    }
}
