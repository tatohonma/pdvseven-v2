using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace a7D.PDV.Integracao.API.Controllers
{
    public class CardapioDigitalController : ApiController
    {
        string Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public class CarregarTemaParams
        {
            public int IDPDV { get; set; }
            public DateTime DtUltimaVerificacao { get; set; }
        }

        [HttpGet]
        public HttpResponseMessage CarregarTema([FromBody]CarregarTemaParams body)
        {
            var status = HttpStatusCode.OK;
            string xmlRetorno;
            string tema = "";
            string dataUltimaAlteracao = "";

            try
            {
                TemaCardapioPDVInformation temaPDV = null;
                List<TemaCardapioPDVInformation> lista = TemaCardapioPDV.Listar().Where(l => l.Ativo == true && (l.PDV == null || l.PDV.IDPDV == body.IDPDV)).ToList();

                if (lista == null || lista.Count() == 0)
                    throw new Exception("Não foi encontrado tema padrão e nenhum tema relacionado com o PDV informado!");

                lista = lista.Where(l => l.DtUltimaAlteracao > body.DtUltimaVerificacao).ToList();
                if (lista != null && lista.Count > 0)
                {
                    temaPDV = lista.FirstOrDefault(l => l.PDV != null && l.PDV.IDPDV == body.IDPDV);

                    if (temaPDV == null)
                    {
                        temaPDV = lista.FirstOrDefault(l => l.PDV == null);
                        tema = temaPDV.TemaCardapio.XML;
                        dataUltimaAlteracao = temaPDV.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        tema = temaPDV.TemaCardapio.XML;
                        dataUltimaAlteracao = temaPDV.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                xmlRetorno = "<temaCardapio>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <layout>";
                xmlRetorno += tema;
                xmlRetorno += "    </layout>";
                xmlRetorno += "    <dtUltimaAlteracao>" + dataUltimaAlteracao + "</dtUltimaAlteracao>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</temaCardapio>";


            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<temaCardapio>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</temaCardapio>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }

    }
}
