using a7D.PDV.Integracao.WS2.Controllers;
using System;
using System.Web.Services;

namespace a7D.PDV.Integracao.WS
{
    /// <summary>
    /// Summary description for CardapioDigital
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CardapioDigital : System.Web.Services.WebService
    {
        [WebMethod]
        public string CarregarTema(Int32 IDPDV, DateTime dtUltimaVerificacao)
        {
            String xmlRetorno;
            var hash = Guid.NewGuid().ToString().Substring(0, 8);

            try
            {
                TemaController.CarregarTema(IDPDV, dtUltimaVerificacao, out String tema, out String nome, out String dataUltimaAlteracao);

                xmlRetorno = "<temaCardapio>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <nome>" + nome + "</nome>";
                xmlRetorno += "    <layout>";
                xmlRetorno += tema;
                xmlRetorno += "    </layout>";
                xmlRetorno += "    <dtUltimaAlteracao>" + dataUltimaAlteracao + "</dtUltimaAlteracao>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</temaCardapio>";
            }
            catch (Exception ex)
            {
                ex.Data.Add("hash", hash);
                xmlRetorno = "<temaCardapio>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</temaCardapio>";
            }

            return xmlRetorno;
        }
    }
}
