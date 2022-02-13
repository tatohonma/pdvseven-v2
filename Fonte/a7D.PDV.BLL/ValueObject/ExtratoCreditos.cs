using System;

namespace a7D.PDV.BLL.ValueObject
{
    public class ExtratoCreditos
    {
        public int IDSaldo { get; set; }
        //public int IDPai { get; set; }
        public DateTime Data { get; set; }
        public int IDTipoPedido { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; }
        public int IDPedido { get; set; }
        public string Agrupamento { get; set; }
        public int? Comanda { get; set; }
        public int? Mesa { get; set; }
        public string Origem { get; set; }
        public decimal Saldo { get; set; } // Esse item será computado!!!

        public override string ToString() => $"{IDSaldo}: {Data} {Valor} {Tipo} {IDPedido} {Agrupamento} {Origem} => {Saldo}";
    }
}
