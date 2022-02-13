using System;

namespace a7D.PDV.BLL.ValueObject
{
    public class PedidosCFe
    {
        public int IDPedido { get; set; }
        public DateTime DtPedidoFechamento { get; set; }
        public string DocumentoCliente { get; set; }
        public decimal? ValorTotal { get; set; }
        public int? IDRetornoSAT { get; set; }
        public int? IDTipoSolicitacaoSAT { get; set; }
        public string ChaveConsulta { get; set; }

        public override string ToString() => $"{IDPedido}: {DtPedidoFechamento} {DocumentoCliente} {ValorTotal} {ChaveConsulta}";
    }
}
