using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    public class BaseResponse
    {
        public string versao { get; set; }
        public bool sucesso { get; set; }
    }
}