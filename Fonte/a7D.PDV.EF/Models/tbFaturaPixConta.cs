using System;

namespace a7D.PDV.EF.Models
{
    public partial class tbFaturaPixConta
    {
        public int IDFaturaPixConta { get; set; }
        public int IDPedido { get; set; }
        public string CodigoFatura { get; set; }
        public string ChavePix { get; set; }
        public string Status { get; set; }
        public decimal Valor { get; set; }
        public DateTime DtInclusao { get; set; }
        public DateTime DtUltimaAlteracao { get; set; }
        public virtual tbPedido tbPedido { get; set; }
    }
}
