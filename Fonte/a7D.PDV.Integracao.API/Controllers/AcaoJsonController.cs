using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Integracao.API.Controllers
{
    public class AcaoJsonController : ApiController
    {
        private string Versao => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

    }
}
