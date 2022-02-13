using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbPedidoPagamento
    {
        public int IDPedidoPagamento { get; set; }
        public int IDPedido { get; set; }
        public int IDTipoPagamento { get; set; }
        public decimal Valor { get; set; }
        public string Autorizacao { get; set; }
        public int? IDGateway { get; set; }
        public int? IDMetodo { get; set; }
        public int? IDContaRecebivel { get; set; }
        public int? IDBandeira { get; set; }
        public DateTime? DataPagamento { get; set; }
        public int? IDUsuarioPagamento { get; set; }
        public DateTime? DataCancelado { get; set; }
        public int? IDUsuarioCancelado { get; set; }
        public bool? Excluido { get; set; }
        public int? IDSaldoBaixa { get; set; } // Se tem um ID é porque esta de fato pago
        public virtual tbPedido tbPedido { get; set; }
        public virtual tbTipoPagamento tbTipoPagamento { get; set; }
    }
}
