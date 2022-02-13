using a7D.PDV.BLL;
using a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class ConfiguracaoController : ApiController
    {
        //public class TipoIDPDV
        //{
        //    public int? tipoPDV;
        //    public int? idPDV;
        //}

        // http://localhost:7777/api/configuracao/ChaveUsuario?tipoPDV=140&idPDV=59

        [HttpGet]
        [Route("api/configuracao/{chave}")]
        public IHttpActionResult Obter(string chave, [FromUri] int? tipoPDV = null, [FromUri] int? idPDV = null)
        {
            try
            {
                var config = ConfiguracaoBD.ConfiguracaoOuPadrao(chave, idPDV, tipoPDV);
                if (config != null)
                    return Ok(new ResultadoOuErro(config.Valor, config.IDConfiguracaoBD));
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new ErroResult(10, ex)));
            }
        }

        [HttpPost]
        [Route("api/configuracao/chaves")]
        public IHttpActionResult LerChaves(string[] chaves, [FromUri] int? tipoPDV = null, [FromUri] int? idPDV = null, [FromUri] bool? sistema = null)
        {
            try
            {
                var configuracoes = new List<ConfigKeyPar>();
                foreach (var chave in chaves)
                {
                    var config = ConfiguracaoBD.ConfiguracaoOuPadrao(chave, idPDV, tipoPDV, sistema);
                    if (config != null)
                        configuracoes.Add(new ConfigKeyPar(chave, config.Valor));
                    else
                        configuracoes.Add(new ConfigKeyPar(chave, null));
                }
                return Ok(configuracoes.ToArray());
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new ErroResult(10, ex)));
            }
        }
    }
}
