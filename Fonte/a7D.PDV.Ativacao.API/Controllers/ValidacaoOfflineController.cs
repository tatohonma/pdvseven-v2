using a7D.PDV.Ativacao.API.Filters;
using a7D.PDV.Ativacao.API.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace a7D.PDV.Ativacao.API.Controllers
{
    [ApiAuth(requerAdm: false)]
    public class ValidacaoOfflineController : BaseSecureApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Gerar(string id)
        {
            try
            {
                var ativacao = db.Ativacoes.FirstOrDefault(a => a.ChaveAtivacao == id);
                if (ativacao == null)
                    return NotFound();
                var result = new { ativacaoOffline = Shared.Services.PDVSecurity.GerarAtivacaoOffline(id) };

                if (!IsAdminRequest())
                {
                    var ativacaoUnproxed = db.UnProxy(ativacao);
                    ativacaoUnproxed.Cliente = db.UnProxy(ativacao.Cliente);
                    var usuarioUnproxed = await UsuarioRequisicaoAsync();
                    EmailServices.EnviarAtivacao(ETipoEmailAtivacao.AtivacaoOffline, ativacaoUnproxed, usuarioUnproxed);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));
            }
        }
    }
}
