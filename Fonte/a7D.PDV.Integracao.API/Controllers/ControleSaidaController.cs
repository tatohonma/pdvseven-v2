using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace a7D.PDV.Integracao.API.Controllers
{
    public class ControleSaidaController : ApiController
    {
        string Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [HttpPost]
        public HttpResponseMessage ValidarSaida(int numero)
        {
            var status = HttpStatusCode.OK;
            string xmlRetorno;

            ComandaInformation comanda;
            PedidoInformation pedido;

            string cliente = "";
            string mensagem = "";
            string corMensagem = "";

            try
            {
                comanda = Comanda.CarregarPorNumero(numero);

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

                    switch (comanda.StatusComanda.IDStatusComanda)
                    {
                        case 10:
                            mensagem = "COMANDA FECHADA";
                            corMensagem = Color.FromArgb(110, 255, 82).ToArgb().ToString();// "6eff52"; //110 255 82
                            break;

                        case 20:
                            mensagem = "COMANDA ABERTA";
                            corMensagem = Color.FromArgb(255, 82, 82).ToArgb().ToString();// "ff5252"; //255 82 82
                            break;

                        case 30:
                            mensagem = "COMANDA CANCELADA";
                            corMensagem = Color.FromArgb(255, 82, 82).ToArgb().ToString();// "ff5252"; //255 82 82
                            break;

                        case 40:
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
                status = HttpStatusCode.InternalServerError;
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

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }
    }
}
