using a7D.PDV.BLL;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Integracao.WS;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class TemaController : ApiController
    {
        public static void CarregarTema(int IDPDV, DateTime dtVerificacao,
            out String tema, out string nome, out String dataUltimaAlteracao)
        {
            var hash = Guid.NewGuid().ToString().Substring(0, 8);
            tema = "";
            dataUltimaAlteracao = "";

            var pdv = BLL.PDV.Carregar(IDPDV);
            var lista = TemaCardapioPDV.Listar().Where(l => l.Ativo == true).ToList();
            var temaPDV = lista.FirstOrDefault(t => t.PDV?.IDPDV == IDPDV);
            if (temaPDV == null)
                temaPDV = lista.FirstOrDefault(t => t.PDV == null);

            if (temaPDV == null)
                throw new Exception("Não foi encontrado tema padrão e nenhum tema relacionado com o PDV informado!");

            nome = temaPDV.TemaCardapio.Nome;

            if (temaPDV.DtUltimaAlteracao > dtVerificacao)
            {
                tema = temaPDV.TemaCardapio.XML;
                dataUltimaAlteracao = temaPDV.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
        }

        [HttpGet]
        [Route("api/tema/{IDPDV}")]
        public IHttpActionResult CarregarTema(int IDPDV, [FromUri] DateTime? UltimaVerificacao = null)
        {
            try
            {
                if (!UltimaVerificacao.HasValue)
                    UltimaVerificacao = DateTime.Parse("2010-01-01");

                TemaController.CarregarTema(IDPDV, UltimaVerificacao.Value, out String tema, out String nome, out String dataUltimaAlteracao);

                var result = new Tema()
                {
                    TemaXML = tema,
                    Nome = nome,
                    UltimaAlteracao = DateTime.Parse(dataUltimaAlteracao),
                    Versao = Util.Versao
                };
                return Ok<Tema>(result);
            }
            catch (ExceptionPDV ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(ex.CodigoRetorno, ex)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }
    }
}