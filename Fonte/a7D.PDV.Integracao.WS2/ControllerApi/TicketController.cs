using a7D.PDV.Ativacao.Shared.Services;
using a7D.PDV.BLL;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.API2.Model;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class TicketController : ApiController
    {
        /*
         * http://localhost:7777/impressao/ticket/3862/0/4
         * http://localhost:7777/api/ticket/0038620005095/E2B8815A8B17E43D58934925FF636AC6
         */

        [HttpGet]
        [Route("api/ticket/{numero}/{chave}")]
        public IHttpActionResult GetTicket(string numero, string chave, int pdv, int user)
        {
            try
            {
                if (numero.Length != 13)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Número inválido")));

                var hash = PDVSecurity.CalculateMD5Hash(numero);
                if (chave != hash)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Chave inválida")));

                string cpedido = numero.Substring(0, 6);
                string cpedidoproduto = numero.Substring(6, 7);

                var produtos = PedidoProduto.ListarPorPedido(int.Parse(cpedido));
                if (produtos == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Pedido não existe")));

                var idpedidoproduto = int.Parse(cpedidoproduto);
                var pedidoproduto = produtos.FirstOrDefault(pp => pp.IDPedidoProduto == idpedidoproduto);
                if (pedidoproduto == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Item do Pedido não existe")));

                var ticket = EF.Repositorio.Carregar<tbTicket>(p => p.IDPedidoProduto == idpedidoproduto);
                if (ticket == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Ticket não emitido")));

                string status;
                if (ticket.dtUtilizacao == null)
                {
                    if(pdv > 0 && user > 0)
                    {
                        ticket.dtUtilizacao = DateTime.Now;
                        ticket.IDUtilizacaoPDV = pdv;
                        ticket.IDUtilizacaoUsuario = user;
                        EF.Repositorio.Atualizar(ticket);
                    }
                    status = "Ticket Válido";
                    // Negativo no ID indica que é uma nova utilização
                    return Ok(new ResultadoOuErro(Mensagem: status, Codigo: -ticket.IDTicket)); 
                }
                else
                {
                    status = "Utilizado em " + ticket.dtUtilizacao.Value.ToString("dd/MM/yyyy HH:mm");
                    return Ok(new ResultadoOuErro(Mensagem: status, Codigo: ticket.IDTicket));
                }
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(60, ex)));
            }
        }
    }
}
