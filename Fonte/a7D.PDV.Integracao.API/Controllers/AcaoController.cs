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
    public class AcaoController : ApiController
    {

        String Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [HttpPost]
        public HttpResponseMessage Verificar(string idPDV)
        {
            var status = HttpStatusCode.OK;
            String xmlRetorno;
            List<AcaoInformation> lista = BLL.Acao.Listar().Where(l => l.PDV.IDPDV == Convert.ToInt32(idPDV)).ToList();

            try
            {
                xmlRetorno = "<Verificar>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";

                foreach (var item in lista)
                {
                    xmlRetorno += "    <acao>";
                    xmlRetorno += "      <nome>" + item.Nome + "</nome>";
                    xmlRetorno += "      <parametro>" + item.Parametro + "</parametro>"; //caso tenha mais de um parametro, separar por |
                    xmlRetorno += "    </acao>";
                }

                xmlRetorno += "  </retorno>";
                xmlRetorno += "</Verificar>";

                foreach (var item in lista)
                    BLL.Acao.Excluir(item.IDAcao.Value);
            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<Verificar>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</Verificar>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }
    }
}
