using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.MyTapp.WS.Models
{
    public class Pedido
    {
        public int? Comanda { get; set; }
        public string Produto { get; set; }
        public string Descricao { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? ValorTotal { get; set; }
        public string Guid { get; set; }

        internal bool Valido
        {
            get
            {
                return Comanda.HasValue
                    && !string.IsNullOrWhiteSpace(Produto)
                    && !string.IsNullOrWhiteSpace(Descricao)
                    && !string.IsNullOrWhiteSpace(Guid)
                    && Quantidade.HasValue
                    && ValorUnitario.HasValue
                    && ValorTotal.HasValue;
            }
        }
    }
}