using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using System.Drawing;

namespace a7D.PDV.Integracao.WS
{
    /// <summary>
    /// Summary description for ControleSaida
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ControleSaida : System.Web.Services.WebService
    {
        String Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [WebMethod]
        public string ValidarSaida(Int32 numero)
        {
            String xmlRetorno;

            try
            {
                ComandaInformation comanda;
                PedidoInformation pedido;

                String cliente = "";
                String mensagem = "";
                String corMensagem = "";

                comanda = Comanda.CarregarPorNumeroOuCodigo(numero);

                if (comanda.IDComanda == null)
                {
                    cliente = "";
                    mensagem = "COMANDA NÃO CADASTRADA\n Verifique o número da comanda informada!";
                    corMensagem = Color.FromArgb(255, 249, 82).ToArgb().ToString();// "fff952"; //255 249 82
                }
                else
                {
                    pedido = BLL.Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);

                    if (pedido != null && pedido.Cliente != null)
                        cliente = pedido.Cliente.NomeCompleto;

                    switch (comanda.ValorStatusComanda)
                    {
                        case EF.Enum.EStatusComanda.Liberada:
                            mensagem = "COMANDA FECHADA";
                            corMensagem = Color.FromArgb(110, 255, 82).ToArgb().ToString();// "6eff52"; //110 255 82
                            break;

                        case EF.Enum.EStatusComanda.EmUso:
                            mensagem = "COMANDA ABERTA";
                            corMensagem = Color.FromArgb(255, 82, 82).ToArgb().ToString();// "ff5252"; //255 82 82
                            break;

                        case EF.Enum.EStatusComanda.Cancelada:
                            mensagem = "COMANDA CANCELADA";
                            corMensagem = Color.FromArgb(255, 82, 82).ToArgb().ToString();// "ff5252"; //255 82 82
                            break;

                        case EF.Enum.EStatusComanda.Perdida:
                            mensagem = "COMANDA PERDIDA";
                            corMensagem = Color.FromArgb(255, 82, 82).ToArgb().ToString();// "ff5252"; //255 82 82
                            break;
                    }
                }

                xmlRetorno = "<validarComanda>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <comanda>" + numero.ToString("0000") + "</comanda>";
                xmlRetorno += "    <cliente>" + cliente + "</cliente>";
                xmlRetorno += "    <mensagem>" + mensagem + "</mensagem>";
                xmlRetorno += "    <corMensagem>" + corMensagem + "</corMensagem>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</validarComanda>";
            }
            catch (Exception _e)
            {
                xmlRetorno = "<validarComanda>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <codigoErro></codigoErro>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</validarComanda>";
            }

            return xmlRetorno;
        }
    }
}
