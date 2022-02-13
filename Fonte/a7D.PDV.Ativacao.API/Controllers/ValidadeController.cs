using a7D.PDV.Ativacao.API.Filters;
using a7D.PDV.Ativacao.API.Services;
using System;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace a7D.PDV.Ativacao.API.Controllers
{
    [ApiAuth(requerAdm: false)]
    public class ValidadeController : BaseSecureApiController
    {
        public IHttpActionResult GetValidade(string id)
        {
            string versao = null;
            string status = "LIC";
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var ativacao = db.Ativacoes.Include("PDVs").AsNoTracking().Where(a => a.ChaveAtivacao == id).FirstOrDefault();
                var query = Request.GetQueryNameValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);

                if (ativacao == null)
                    throw new Exception("Chave de ativação inválida");

                if (!ativacao.Ativo && !ativacao.ReativadoSuporte)
                    throw new Exception("Chave de ativação desativada");

                else if (ativacao.PDVs == null || ativacao.PDVs.Count == 0)
                    throw new Exception("Nenhum produto configurado pra essa licença");

                if (query.Count > 0)
                {
                    versao = query["versao"];
                    //if (versao == "2.18.0.14")
                    //    throw new Exception($"Versão '{versao}' desativada, entre em contato com o suporte");
                }

                if (ativacao.ReativadoSuporte)
                {
                    if (ativacao.DataValidadeProvisoria == null)
                        throw new Exception("Chave de ativação desativada");

                    return Ok(new { Validade = ativacao.DataValidadeProvisoria.Value.ToUniversalTime() });
                }

                return Ok(new { Validade = DateTime.Now.ToUniversalTime().AddDays(ativacao.DiasValidadeAtivacao) });
            }
            catch (EntityException eex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, eex.Message, eex));
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));
            }
            finally
            {
                if (!string.IsNullOrEmpty(id))
                    ClientesService.Registra(id, versao, status, false);
            }
        }

        [Route("api/validade/liberacao/{id:int}")]
        [HttpPost]
        public async Task<IHttpActionResult> Liberacao([FromUri] int id)
        {
            try
            {
                var ativacao = db.Ativacoes.Find(id);
                if (ativacao == null)
                {
                    return NotFound();
                }

                if (ativacao.Ativo == true || ativacao.ReativadoSuporte == true)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Accepted, new { Validade = ativacao.DataValidadeProvisoria.Value.ToUniversalTime() }));

                var now = DateTime.Now;

                ativacao.ReativadoSuporte = true;
                ativacao.DataReativacaoSuporte = now;
                ativacao.DataValidadeProvisoria = now.AdicionarDiasUteis(3);

                db.SaveChanges();

                if (!IsAdminRequest())
                {
                    var ativacaoUnproxed = db.UnProxy(ativacao);
                    ativacaoUnproxed.Cliente = db.UnProxy(ativacao.Cliente);
                    var usuarioUnproxed = await UsuarioRequisicaoAsync();
                    EmailServices.EnviarAtivacao(ETipoEmailAtivacao.LiberacaoTemporaria, ativacaoUnproxed, usuarioUnproxed);
                }
                return Ok(new { Validade = ativacao.DataValidadeProvisoria.Value.ToUniversalTime() });
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));
            }
        }
    }
}
