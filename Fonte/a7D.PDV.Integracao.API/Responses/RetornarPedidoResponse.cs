using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    class RetornarPedidoResponse : BaseResponse
    {
        public int idPedido { get; set; }
        public Produto[] produtos { get; set; }

        public class Produto
        {
            public int idProduto { get; set; }
            public decimal valorUnitario { get; set; }
            public decimal quantidade { get; set; }
            public string notas { get; set; }
            public Modificacao[] modificacaoes { get; set; }
        }

        public class Modificacao
        {
            public int idProduto { get; set; }
            public decimal valorUnitario { get; set; }
        }
    }
}