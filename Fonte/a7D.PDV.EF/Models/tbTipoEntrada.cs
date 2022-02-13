using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTipoEntrada
    {
        public tbTipoEntrada()
        {
            this.tbPedidoes = new List<tbPedido>();
        }

        public int IDTipoEntrada { get; set; }
        public string Nome { get; set; }
        public decimal ValorEntrada { get; set; }
        public decimal ValorConsumacaoMinima { get; set; }
        public decimal TaxaServico { get; set; }
        public decimal LimiteComanda { get; set; }
        public bool Ativo { get; set; }
        public bool Padrao { get; set; }
        public virtual ICollection<tbPedido> tbPedidoes { get; set; }
    }
}
