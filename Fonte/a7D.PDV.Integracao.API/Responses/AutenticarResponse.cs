using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    class AutenticarResponse : BaseResponse
    {
        public Usuario usuario { get; set; }

        public class Usuario
        {
            public int idUsuario { get; set; }
            public string nome { get; set; }
            public Permissoes permissoes { get; set; }
        }

        public class Permissoes
        {
            public bool adm { get; set; }
            public bool caixa { get; set; }
            public bool garcom { get; set; }
            public bool gerente { get; set; }
        }
    }
}