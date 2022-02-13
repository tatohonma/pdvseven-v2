using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    class CarregarTemaResponse : BaseResponse
    {
        public JObject layout { get; set; }
        public DateTime dtUltimaAlteracao { get; set; }
    }
}