using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbImagem
    {
        public tbImagem()
        {
            this.tbImagemTemas = new List<tbImagemTema>();
            this.tbProdutoImagems = new List<tbProdutoImagem>();
        }

        public int IDImagem { get; set; }
        public string Nome { get; set; }
        public string Extensao { get; set; }
        public int? Altura { get; set; }
        public int? Largura { get; set; }
        public int? Tamanho { get; set; }
        public byte[] Dados { get; set; }
        public virtual ICollection<tbImagemTema> tbImagemTemas { get; set; }
        public virtual ICollection<tbProdutoImagem> tbProdutoImagems { get; set; }
    }
}
