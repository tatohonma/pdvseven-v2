using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbProdutoTraducao
    {
        public int IDProdutoTraducao { get; set; }
        public int IDProduto { get; set; }
        public int IDIdioma { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public virtual tbIdioma tbIdioma { get; set; }
        public virtual tbProduto tbProduto { get; set; }
    }
}
