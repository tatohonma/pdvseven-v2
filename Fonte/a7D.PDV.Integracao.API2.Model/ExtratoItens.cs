using System;

namespace a7D.PDV.Integracao.API2.Model
{
    public class ExtratoItens
    {
        public int IDPedido { get; set; }
        public DateTime? Data { get; set; }
        public decimal ValorTotal { get; set; }
        public int IDTipoProduto { get; set; }
        public string Produto { get; set; }
        public decimal Valor { get; set; }
        public string Agrupamento { get; set; }
        public int? Comanda { get; set; }
        public decimal Saldo { get; set; } // Esse item será computado!!!

        public override string ToString() => $"{IDPedido}: {Data} {ValorTotal} {IDTipoProduto} {Produto} {Valor} {Agrupamento} {Comanda} => {Saldo}";
    }
}
