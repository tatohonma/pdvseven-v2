using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbMovimentacao
    {
        public tbMovimentacao()
        {
            this.tbMovimentacaoProdutos = new List<tbMovimentacaoProduto>();
        }

        public int IDMovimentacao { get; set; }
        public int IDTipoMovimentacao { get; set; }
        public Nullable<int> IDFornecedor { get; set; }
        public string GUID { get; set; }
        public System.DateTime DataMovimentacao { get; set; }
        public string Descricao { get; set; }
        public string NumeroPedido { get; set; }
        public Nullable<int> IDMovimentacao_reversa { get; set; }
        public bool Excluido { get; set; }
        public bool Reversa { get; set; }
        public bool Processado { get; set; }
        public virtual tbTipoMovimentacao tbTipoMovimentacao { get; set; }
        public virtual ICollection<tbMovimentacaoProduto> tbMovimentacaoProdutos { get; set; }
    }
}
