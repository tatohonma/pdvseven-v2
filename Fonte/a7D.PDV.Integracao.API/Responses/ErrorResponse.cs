using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    public class ErrorResponse : BaseResponse
    {
        new public bool sucesso { get { return false; } }
        public string mensagemErro { get; set; }
        public string detalhes { get; set; }
    }
}