using a7D.PDV.Fiscal.Comunicacao.SAT;
using Microsoft.Rest;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace a7D.PDV.FiscalServices.ConttrollerApi
{
    public class SatController : ApiController
    {
        private static readonly object @lock = new object();

        private void VerificaDLL()
        {
            var fileDLL = System.Web.Hosting.HostingEnvironment.MapPath("~/bin/sat.dll");
            if (!System.IO.File.Exists(fileDLL))
                throw new Exception($"Arquivo '{fileDLL}' não existe");
        }

        //[HttpGet]
        //[Route("api/teste")]
        //public async Task<IHttpActionResult> Teste()
        //{
        //    var credentials = new TokenCredentials("fiscal", "Bearer");
        //    var ws2api = new WS2API.A7DPDVIntegracaoWS2(credentials); // new Uri("http://localhost:7777")
        //    var chaves = new string[] { "PossuiSAT", "LimiteComanda" };
        //    var valores = await ws2api.Configuracao.LerChavesWithHttpMessagesAsync(chaves);
        //    return Json(valores.Body);
        //}

        [HttpGet]
        [Route("api/sat")]
        public IHttpActionResult ConsultarSat()
        {
            lock (@lock)
            {
                VerificaDLL();

                var cod = new Random().Next(1, 999999);
                var ret = ComunicacaoSat.ConsultarSat(cod);
                if (!ret.Contains("|"))
                    ret = "erro|" + CodigosSAT.DescricoesOuCodigos(ret);

                return Json(new { numeroSessao = cod, mensagem = ret });
            }
        }

        [HttpGet]
        [Route("api/sat/consultarsessao/{id}")]
        public IHttpActionResult ConsultarSessao(int id, [FromUri] string codigoDeAtivacao)
        {
            lock (@lock)
            {
                VerificaDLL();

                var retorno = ComunicacaoSat.ConsultarSessao(codigoDeAtivacao, id);
                return Json(new RetApi { RetStr = retorno });
            }
        }

        [HttpGet]
        [Route("api/sat/log")]
        public HttpResponseMessage Log(string codigoDeAtivacao)
        {
            lock (@lock)
            {
                VerificaDLL();
                var resposta = ComunicacaoSat.ExtrairLogsSat(new Random().Next(1, 999999), codigoDeAtivacao);
                var respostas = resposta.Split('|');
                if (respostas[1] != "15000")
                {
                    throw new Exception(CodigosSAT.DescricoesOuCodigos(resposta));
                }

                var bytes = Convert.FromBase64String(respostas[5]);
                var logs = Encoding.UTF8.GetString(bytes);

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(logs, Encoding.UTF8, "text/plain")
                };
            }
        }

        [HttpPost]
        [Route("api/sat/enviarvenda")]
        public IHttpActionResult EnviarVenda(HttpRequestMessage request, [FromUri] string codigoDeAtivacao, [FromUri] string numeroSessao)
        {
            lock (@lock)
            {
                VerificaDLL();

                var value = request.Content.ReadAsStringAsync().Result;
                //var query = request.GetQueryNameValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
                //var codigoDeAtivacao = query["codigoDeAtivacao"];
                //var numeroSessao = Convert.ToInt32(query["numeroSessao"]);

                var retorno = ComunicacaoSat.EnviarVenda(codigoDeAtivacao, value, int.Parse(numeroSessao));
                return Json(new RetApi { RetStr = retorno });
            }
        }

        [HttpPost]
        [Route("api/sat/cancelarvenda")]
        public IHttpActionResult CancelarVenda([FromBody] EnvApiCancelamento value, [FromUri] string codigoDeAtivacao)
        {
            lock (@lock)
            {
                VerificaDLL();

                var xmlCancelamento = value.DadosCancelamento;
                xmlCancelamento = xmlCancelamento.Replace("\\", "");

                var retorno = ComunicacaoSat.EnviarCancelamento(codigoDeAtivacao, value.NumeroSessao, value.Chave, xmlCancelamento);
                return Json(new RetApi { RetStr = retorno });
            }
        }
    }
}
