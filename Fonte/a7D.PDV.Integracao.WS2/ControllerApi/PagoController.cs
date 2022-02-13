using a7D.PDV.Integracao.API2.Model;
using System;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class PagoController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Transacao([FromBody] PagoPreTransacao request)
        {
            try
            {
                var result = new PagoPreRetorno();
                result.id = request.id;
                result.pdv = request.pdv;
                result.amount = 12345;
                result.multiPayment = true;
                result.cashPayment = false;
                result.condition = 1;
                result.printAfter = "PDVSeven  www.pdvseven.com.br";
                result.endOperation = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(new Erro(500, ex)));
            }
        }
    }
}
