using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    class AreaImpressaoListarResponse : BaseResponse
    {
        public AreaImpressao[] areasImpressao { get; set; }

        public class AreaImpressao
        {
            public int idAreaImpressao { get; set; }
            public string nome { get; set; }
        }
    }
}