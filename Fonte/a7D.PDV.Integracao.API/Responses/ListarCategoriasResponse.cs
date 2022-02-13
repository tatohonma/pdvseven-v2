using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    class ListarCategoriasResponse: BaseResponse
    {
        public bool multi { get; set; }
        public Categoria[] categorias { get; set; }

       public class Categoria
        {
            public int idCategoriaProduto { get; set; }
            public bool ativo { get; set; }
            public string nome { get; set; }
            public DateTime dtUltimaAlteracao { get; set; }
        }
    }

    
}