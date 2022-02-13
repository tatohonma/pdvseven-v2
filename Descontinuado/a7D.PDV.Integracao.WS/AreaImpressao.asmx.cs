using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using a7D.PDV.Model;
using a7D.PDV.BLL;

namespace a7D.PDV.Integracao.WS
{
    /// <summary>
    /// Summary description for AreaImpressao
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AreaImpressao : System.Web.Services.WebService
    {
        [WebMethod]
        public string ListarAreaImpressao()
        {
            String xmlRetorno;

            try
            {
                List<AreaImpressaoInformation> lista = BLL.AreaImpressao.Listar().OrderBy(l => l.Nome).ToList();

                xmlRetorno = "<listarAreasImpressao>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <listaAreaImpressao>";

                foreach (var item in lista)
                {
                    xmlRetorno += "      <areaImpressao>";
                    xmlRetorno += "        <idAreaImpressao>" + item.IDAreaImpressao + "</idAreaImpressao>";
                    xmlRetorno += "        <nome>" + item.Nome + "</nome>";
                    xmlRetorno += "        <tipo>" + item.TipoAreaImpressao.IDTipoAreaImpressao.Value + "</tipo>";
                    xmlRetorno += "      </areaImpressao>";
                }

                xmlRetorno += "    </listaAreaImpressao>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarAreasImpressao>";
            }
            catch (Exception ex)
            {
                xmlRetorno = "<listarAreasImpressao>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</listarAreasImpressao>";
            }

            return xmlRetorno;
        }
    }
}
