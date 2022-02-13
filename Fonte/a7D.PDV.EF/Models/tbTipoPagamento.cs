using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public class tbTipoPagamento : IERP, IERPSync
    {
        public tbTipoPagamento()
        {
            this.tbCaixaValorRegistroes = new List<tbCaixaValorRegistro>();
            this.tbPedidoPagamentoes = new List<tbPedidoPagamento>();
        }

        public int IDTipoPagamento { get; set; }
        public string CodigoImpressoraFiscal { get; set; }
        public string CodigoERP { get; set; }
        public string Nome { get; set; }
        public bool? RegistrarValores { get; set; }
        public int? PrazoCredito { get; set; }
        public int? IDGateway { get; set; }
        public bool Ativo { get; set; }
        public int? IDMeioPagamentoSAT { get; set; }
        public int? IDContaRecebivel { get; set; }
        public int? IDBandeira { get; set; }

        public virtual ICollection<tbCaixaValorRegistro> tbCaixaValorRegistroes { get; set; }
        public virtual tbMeioPagamento tbMeioPagamentoSAT { get; set; }
        public virtual tbContaRecebivel ContaRecebivel { get; set; }
        public virtual tbBandeira Bandeira { get; set; }

        public virtual ICollection<tbPedidoPagamento> tbPedidoPagamentoes { get; set; }

        public bool RequerAlteracaoERP(DateTime dtSync) 
            => false;

        public int myID() => IDTipoPagamento;

        public override string ToString() => $"{IDTipoPagamento}: {Nome}";
    }
}
