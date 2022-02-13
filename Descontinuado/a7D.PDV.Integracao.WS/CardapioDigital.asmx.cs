using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using a7D.PDV.Model;
using a7D.PDV.BLL;

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
                String tema = "";
                String dataUltimaAlteracao = "";

                var pdv = BLL.PDV.Carregar(IDPDV);

                //BLL.EventLog.Info($"{hash} - Recebida requisição do PDV(ID = {IDPDV}, Nome = {pdv?.Nome})");

                var lista = TemaCardapioPDV.Listar().Where(l => l.Ativo == true).ToList();

                //EventLog.Info($"{hash} - Existem {lista?.Count ?? 0} temas ativos");

                var temaPDV = lista.FirstOrDefault(t => t.PDV?.IDPDV == IDPDV);

                if (temaPDV == null)
                {
                    //EventLog.Info($"{hash} - Tema específico não encontrado, procurando o padrão");
                    temaPDV = lista.FirstOrDefault(t => t.PDV == null);
                }
                else
                {
                    //EventLog.Info($"{hash} - Tema {temaPDV?.TemaCardapio?.Nome} foi encontrado.");
                }

                if (temaPDV == null)
                    throw new Exception("Não foi encontrado tema padrão e nenhum tema relacionado com o PDV informado!");


                if (temaPDV.DtUltimaAlteracao > dtUltimaVerificacao)
                {
                    //EventLog.Info($"{hash} - Atualizando tema");
                    tema = temaPDV.TemaCardapio.XML;
                    dataUltimaAlteracao = temaPDV.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                {
                    //EventLog.Info($"{hash} - Não é necessário atualizar tema");
                }

                xmlRetorno = "<temaCardapio>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
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
