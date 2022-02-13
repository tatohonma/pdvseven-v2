using a7D.PDV.BLL;
using a7D.PDV.Integracao.API.ExtensionMethods;
using a7D.PDV.Integracao.API.Responses;
using a7D.PDV.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace a7D.PDV.Integracao.API.Controllers
{
    public class CardapioDigitalJsonController : ApiController
    {
        private string Versao => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [HttpGet]
        public IHttpActionResult CarregarTema([FromUri]int IdPdv, [FromUri]string ultimaVerificacao)
        {
            var response = new CarregarTemaResponse();
            string tema = string.Empty;
            DateTime? dtUltimaAlteracao = null;
            DateTime dtUltimaVerificacao = ultimaVerificacao.ConverterData();

            TemaCardapioPDVInformation temaPDV = null;
            List<TemaCardapioPDVInformation> lista = TemaCardapioPDV.Listar().Where(l => l.Ativo == true && (l.PDV == null || l.PDV.IDPDV == IdPdv)).ToList();

            if (lista == null || lista.Count() == 0)
                throw new Exception("Não foi encontrado tema padrão e nenhum tema relacionado com o PDV informado!");

            lista = lista.Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();

            if (lista != null && lista.Count > 0)
            {
                temaPDV = lista.FirstOrDefault(l => l.PDV != null && l.PDV.IDPDV == IdPdv);

                if (temaPDV == null)
                {
                    temaPDV = lista.FirstOrDefault(l => l.PDV == null);
                    tema = temaPDV.TemaCardapio.XML;
                    dtUltimaAlteracao = temaPDV.DtUltimaAlteracao.Value;
                }
                else
                {
                    tema = temaPDV.TemaCardapio.XML;
                    dtUltimaAlteracao = temaPDV.DtUltimaAlteracao.Value;
                }
            }

            response.dtUltimaAlteracao = dtUltimaAlteracao.HasValue ? dtUltimaAlteracao.Value : DateTime.MinValue;
            response.sucesso = true;
            response.versao = Versao;
            tema = $"<tema>{tema}</tema>";
            var xml = new XmlDocument();
            xml.LoadXml(tema);

            response.layout = JObject.Parse(JsonConvert.SerializeXmlNode(xml));

            return ResponseMessage(new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
