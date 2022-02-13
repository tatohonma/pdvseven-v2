using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    public class ValidarPdvResponse : BaseResponse
    {
        public int idPdv { get; set; }
        public string nome { get; set; }
    }
}