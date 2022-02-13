using System;

namespace a7D.PDV.BLL.ValueObject
{
    public class ExtratoItens
    {
        public int IDPedido { get; set; }
        public DateTime? Data { get; set; }
        public decimal ValorTotal { get; set; }
        public string Tipo { get; set; }
        public string Item { get; set; }
        public decimal Valor { get; set; }

        public override string ToString() => $"{IDPedido}: {Data} {ValorTotal} {Tipo} {Item} {Valor}";
    }
}
