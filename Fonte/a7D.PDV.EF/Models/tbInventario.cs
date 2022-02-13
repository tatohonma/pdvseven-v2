using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbInventario
    {
        public tbInventario()
        {
            this.tbInventarioProdutos = new List<tbInventarioProduto>();
        }

        public int IDInventario { get; set; }
        public string GUID { get; set; }
        public string Descricao { get; set; }
        public System.DateTime Data { get; set; }
        public bool Processado { get; set; }
        public bool Excluido { get; set; }
        public virtual ICollection<tbInventarioProduto> tbInventarioProdutos { get; set; }
    }
}
