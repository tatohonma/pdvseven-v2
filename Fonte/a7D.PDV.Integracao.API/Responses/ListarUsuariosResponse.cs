using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    class ListarUsuariosResponse : BaseResponse
    {
        public Usuario[] usuarios { get; set; }

        public class Usuario
        {
            public int idUsuario { get; set; }
            public string nome { get; set; }
            public bool ativo { get; set; }
            public DateTime dtUltimaAlteracao { get; set; }
        }
    }
}