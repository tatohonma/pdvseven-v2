using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbImagemTema
    {
        public int IDImagemTema { get; set; }
        public int IDTemaCardapio { get; set; }
        public int IDImagem { get; set; }
        public int IDIdioma { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public virtual tbIdioma tbIdioma { get; set; }
        public virtual tbImagem tbImagem { get; set; }
        public virtual tbTemaCardapio tbTemaCardapio { get; set; }
    }
}
