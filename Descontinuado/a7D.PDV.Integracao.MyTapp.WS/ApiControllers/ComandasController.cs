using a7D.PDV.BLL;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static a7D.PDV.BLL.Comanda;

namespace a7D.PDV.Integracao.MyTapp.WS.ApiControllers
{
    public class ComandasController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        [Route("api/comandas/status/{numeroComanda}")]
        [HttpGet]
        public IHttpActionResult Status([FromUri]int numeroComanda)
        {
            try
            {
                Log.Trace($"Comanda {numeroComanda} - Validar Status");
                Validar(numeroComanda);
                Log.Trace($"Comanda {numeroComanda} - Status OK");
                return Ok();
            }
            catch (ComandaException ex)
            {
                Log.Error($"Comanda {numeroComanda} - Erro de validação: Status {ex.Codigo}");
                Log.Error(ex, ex.Message);
                if (ex.Codigo == -1)
                    return NotFound();
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, new { motivo = ex.Codigo });
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                Log.Error($"Comanda {numeroComanda} - Erro inesperado");
                Log.Error(ex, ex.Message);
                return InternalServerError();
            }
        }
    }
}
