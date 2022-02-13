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
    public class AreaImpressaoController : ApiController
    {
        String Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [HttpGet]
        public HttpResponseMessage Listar()
        {
            var status = HttpStatusCode.OK;
            String xmlRetorno;

            try
            {
                List<AreaImpressaoInformation> lista = BLL.AreaImpressao.Listar().OrderBy(l => l.Nome).ToList();

                xmlRetorno = "<listarAreasImpressao>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <listaAreaImpressao>";

                foreach (var item in lista)
                {
                    xmlRetorno += "      <areaImpressao>";
                    xmlRetorno += "        <idAreaImpressao>" + item.IDAreaImpressao + "</idAreaImpressao>";
                    xmlRetorno += "        <nome>" + item.Nome + "</nome>";
                    xmlRetorno += "      </areaImpressao>";
                }

                xmlRetorno += "    </listaAreaImpressao>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarAreasImpressao>";
            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<listarAreasImpressao>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarAreasImpressao>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }
    }
}
