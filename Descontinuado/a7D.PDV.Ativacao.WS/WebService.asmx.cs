using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using a7D.PDV.Ativacao.WS.Model;
using System.Xml.Serialization;
using System.IO;

namespace a7D.PDV.Ativacao.WS
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        //[WebMethod]
        //public DateTime Ativacao(String chaveAtivacao, String chaveHardware, String xmlRecursosAtivados)
        //{
        //    return DateTime.Now.AddMonths(1);
        //}

        [WebMethod]
        public String Ativacao(String chaveAtivacao, String xmlRecursosAtivados)
        {
            AtivacaoInformation ativacao;
            String xmlRetorno;

            //ListaPDVType ListaPDV = null;
            //try
            //{
            //    var s = new XmlSerializer(typeof(ListaPDVType));
            //    using (TextReader tr = new StringReader(xmlRecursosAtivados))
            //    {
            //        ListaPDV = (ListaPDVType)s.Deserialize(tr);
            //    }

            //}
            //catch
            //{

            //}

            try
            {
                ativacao = BLL.Ativacao.Carregar(chaveAtivacao);

                if (ativacao.IDAtivacao == null)
                    throw new Exception("Chave de acesso inválida!");
                //else if (ativacao.Ativo != true)
                //    throw new Exception("Chave de acesso desativada!");

                xmlRetorno = "";
                xmlRetorno += "<ativacao>";
                xmlRetorno += "  <status>" + (ativacao.Ativo == true ? "1" : "0") +  "</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <dtValidade>" + DateTime.Now.AddDays(ativacao.DiasValidadeAtivacao.Value).ToString("yyyy-MM-dd") + "</dtValidade>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</ativacao>";
            }
            catch (Exception _e)
            {
                xmlRetorno = "";
                xmlRetorno += "<ativacao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.Message + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</ativacao>";
            }

            return xmlRetorno;
        }
    }
}
