using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTemaCardapio
    {
        public tbTemaCardapio()
        {
            this.tbImagemTemas = new List<tbImagemTema>();
            this.tbTemaCardapioPDVs = new List<tbTemaCardapioPDV>();
        }

        public int IDTemaCardapio { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string XML { get; set; }
        public virtual ICollection<tbImagemTema> tbImagemTemas { get; set; }
        public virtual ICollection<tbTemaCardapioPDV> tbTemaCardapioPDVs { get; set; }
    }
}
