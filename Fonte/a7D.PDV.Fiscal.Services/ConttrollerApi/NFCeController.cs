using a7D.PDV.Fiscal.Comunicacao.SAT;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.FiscalServices.ConttrollerApi
{
    public class NFCeController : ApiController
    {
        [HttpPost]
        [Route("api/nfce/enviarvenda")]
        public IHttpActionResult EnviarVenda(HttpRequestMessage request)
        {
            var value = request.Content.ReadAsStringAsync().Result;
            string retorno = "ok";
            return Json(new RetApi { RetStr = retorno });
        }
    }
}
