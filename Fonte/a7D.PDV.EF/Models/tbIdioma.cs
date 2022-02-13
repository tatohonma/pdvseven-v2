using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbIdioma
    {
        public tbIdioma()
        {
            this.tbProdutoTraducaos = new List<tbProdutoTraducao>();
            this.tbImagemTemas = new List<tbImagemTema>();
        }

        public int IDIdioma { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public virtual ICollection<tbProdutoTraducao> tbProdutoTraducaos { get; set; }
        public virtual ICollection<tbImagemTema> tbImagemTemas { get; set; }
    }
}
