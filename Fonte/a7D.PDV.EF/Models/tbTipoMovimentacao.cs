using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTipoMovimentacao
    {
        public tbTipoMovimentacao()
        {
            this.tbMovimentacaos = new List<tbMovimentacao>();
        }

        public int IDTipoMovimentacao { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public virtual ICollection<tbMovimentacao> tbMovimentacaos { get; set; }
    }
}
