using a7D.PDV.Fiscal.Comunicacao.SAT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace a7D.PDV.SATService.Controllers
{
    public class SatController : ApiController
    {
        private static object @lock = new object();

        //private string ConverterUT8(string strSource)
        //{
        //    var srcEncoding = Encoding.Default;
        //    var utf8 = Encoding.UTF8;
        //    var bytes1252 = srcEncoding.GetBytes(strSource);
        //    var bytesutf8 = Encoding.Convert(srcEncoding, utf8, bytes1252);
        //    return utf8.GetString(bytesutf8);
        //}

        //private IHttpActionResult RetornaErro(Exception ex)
        //{
        //    string info = "";
        //    var inner = ex;
        //    while (inner != null)
        //    {
        //        info += ex.Message + " \r\n";
        //        inner = inner.InnerException;
        //    }
        //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, info, ex));
        //}

        private void VerificaDLL()
        {
            var fileDLL = System.Web.Hosting.HostingEnvironment.MapPath("~/bin/sat.dll");
            if (!System.IO.File.Exists(fileDLL))
                throw new Exception($"Arquivo '{fileDLL}' não existe no serviço WS_SAT");

        }

        // GET: api/Sat
        [ActionName("DefaultAction")]
        public IHttpActionResult Get()
        {
            try
            {
                VerificaDLL();

                var cod = new Random().Next(1, 999999);
                var ret = ComunicacaoSat.ConsultarSat(cod);
                if (!ret.Contains("|"))
                    ret = "erro|" + CodigosSAT.DescricoesOuCodigos(ret);
                return Json(new { numeroSessao = cod, mensagem = ret });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // POST api/sat/enviarvenda
        [HttpPost]
        public IHttpActionResult EnviarVenda(HttpRequestMessage request)
        {
            lock (@lock)
            {
                VerificaDLL();

                //try
                //{
                var value = request.Content.ReadAsStringAsync().Result;
                var query = request.GetQueryNameValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
                var codigoDeAtivacao = query["codigoDeAtivacao"];
                var numeroSessao = Convert.ToInt32(query["numeroSessao"]);
                return Json(new { RetStr = ComunicacaoSat.EnviarVenda(codigoDeAtivacao, value, numeroSessao) });
                //}
                //catch (Exception ex)
                //{
                //    return RetornaErro(ex);
                //}
            }
        }

        //[HttpPost]
        //public IHttpActionResult TesteFimAFim([FromUri] string versao)
        //{
        //    lock (@lock)
        //    {
        //        VerificaDLL();
        //        //try
        //        //{
        //        int numeroSessao = new Random().Next(1, 999999);
        //        var resposta = ComunicacaoSat.TesteFimAFimSat(numeroSessao, "123456789", versao);

        //        return Json(new { RetStr = ComunicacaoSat.TesteFimAFimSat(numeroSessao, "123456789", versao) });
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    return RetornaErro(ex);
        //        //}
        //    }
        //}

        // POST api/sat/cancelarvenda
        [HttpPost]
        public IHttpActionResult CancelarVenda([FromBody] EnvApi value, string codigoDeAtivacao)
        {
            lock (@lock)
            {
                VerificaDLL();
                //try
                //{
                var xmlCancelamento = value.DadosCancelamento;
                xmlCancelamento = xmlCancelamento.Replace("\\", "");
                var retStr = ComunicacaoSat.EnviarCancelamento(codigoDeAtivacao, value.NumeroSessao, value.Chave, xmlCancelamento);
                return Json(new { RetStr = retStr });
                //}
                //catch (Exception ex)
                //{
                //    return RetornaErro(ex);
                //}
            }
        }

        [HttpGet]
        public IHttpActionResult ConsultarSessao(int id, string codigoDeAtivacao)
        {
            lock (@lock)
            {
                VerificaDLL();
                //try
                //{
                return Json(new { RetStr = ComunicacaoSat.ConsultarSessao(codigoDeAtivacao, id) });
                //}
                //catch (Exception ex)
                //{
                //    return RetornaErro(ex);
                //}
            }
        }

        [HttpGet]
        public IHttpActionResult Log(string codigoDeAtivacao)
        {
            lock (@lock)
            {
                VerificaDLL();
                //try
                //{
                var resposta = ComunicacaoSat.ExtrairLogsSat(new Random().Next(1, 999999), codigoDeAtivacao).Split('|');
                if (Convert.FromBase64String(resposta[0]).ToString().ToLower().Contains("erro".ToLower()))
                {
                    throw new Exception(Convert.FromBase64String(resposta[0]).ToString());
                }

                var bytea = Convert.FromBase64String(resposta[5]);
                var log = Encoding.UTF8.GetString(bytea);
                var listaLog = log.Split('\n');
                var arrayLog = new List<object>();

                foreach (var l in listaLog)
                    arrayLog.Add(new { log = l });

                return Json(new { log = arrayLog.ToArray() });
                //}
                //catch (Exception ex)
                //{
                //    return RetornaErro(ex);
                //}
            }
        }
    }
}
