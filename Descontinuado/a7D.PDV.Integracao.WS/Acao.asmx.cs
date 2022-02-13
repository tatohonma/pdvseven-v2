using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using a7D.PDV.Model;

namespace a7D.PDV.Integracao.WS
{
    /// <summary>
    /// Summary description for Acao
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Acao : System.Web.Services.WebService
    {

        [WebMethod]
        public string Verificar(Int32 idPDV)
        {
            String xmlRetorno;

            try
            {
                List<AcaoInformation> lista = BLL.Acao.Listar().Where(l => l.PDV.IDPDV == idPDV).ToList();

                xmlRetorno = "<Verificar>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
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
            catch (Exception ex)
            {
                xmlRetorno = "<Verificar>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</Verificar>";
            }

            return xmlRetorno;
        }
    }
}
