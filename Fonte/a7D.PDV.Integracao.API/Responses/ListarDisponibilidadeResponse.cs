using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    class ListarDisponibilidadeResponse: BaseResponse
    {
        public Produto[] produtos { get; set; }

        public class Produto
        {
            public int idProduto { get; set; }
            public bool disponibilidade { get; set; }
            public DateTime dtAlteracaoDisponibilidade { get; set; }
        }
    }
}