using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTaxaEntrega
    {
        public tbTaxaEntrega()
        {
            this.tbPedidoes = new List<tbPedido>();
        }

        public int IDTaxaEntrega { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public int? IDTamanhoPacote { get; set; }
        public int? IDGateway { get; set; }

        public virtual ICollection<tbPedido> tbPedidoes { get; set; }
    }
}
