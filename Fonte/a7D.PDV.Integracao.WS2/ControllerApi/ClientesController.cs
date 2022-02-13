using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class ClientesController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ObterClientes([FromUri] string nome = null, [FromUri] string email = null, [FromUri] string documento = null, [FromUri] string telefone = null, [FromUri] int pagina = 1, [FromUri] int qtd = 10)
        {
            try
            {
                if (pagina < 0) pagina = 0;
                if (qtd < 1) qtd = 1;
                var clientes = BLL.Cliente.BuscarCliente(nome, email, documento, telefone, pagina, qtd);
                var retorno = new List<Cliente>();
                foreach (var cliente in clientes.Skip((pagina - 1) * qtd).Take(qtd))
                {
                    retorno.Add(DTO.Converter(cliente));
                }
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new ErroResult(10, ex)));
            }
        }

        [HttpGet]
        public IHttpActionResult ObterCliente(int id)
        {
            try
            {
                var cliente = BLL.Cliente.Carregar(id);

                if (cliente?.IDCliente != null && !string.IsNullOrWhiteSpace(cliente?.NomeCompleto))
                {
                    return Ok(DTO.Converter(cliente));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new ErroResult(10, ex)));
            }
        }

        [HttpPost]
        [Route("api/clientes/inserir")]
        public IHttpActionResult InserirCliente([FromBody] Cliente cliente)
        {
            try
            {
                string msg = BLL.Cliente.ValidarCliente(0,
                    cliente.NomeCompleto,
                    cliente.Endereco?.Endereco, cliente.Endereco?.Complemento, cliente.Endereco?.Bairro, cliente.Endereco?.Cidade,
                    cliente.Documento1, cliente.RG, (cliente.DtNascimento ?? DateTime.MinValue).ToString("dd/MM/yyyy"), false);

                if (!string.IsNullOrEmpty(msg))
                    return Ok(new ResultadoOuErro(msg, 0));

                if (cliente.Telefone1DDD == 0)
                    cliente.Telefone1DDD = BLL.ConfiguracoesSistema.Valores.DDDPadrao;

                var c = new tbCliente()
                {
                    NomeCompleto = cliente.NomeCompleto,
                    Documento1 = cliente.Documento1,
                    RG = cliente.RG,
                    DtInclusao = DateTime.Now,
                    Telefone1DDD = cliente.Telefone1DDD,
                    Telefone1Numero = cliente.Telefone1Numero,
                    Telefone2DDD = cliente.Telefone2DDD,
                    Telefone2Numero = cliente.Telefone2Numero,
                    Endereco = cliente.Endereco?.Endereco,
                    Complemento = cliente.Endereco?.Complemento,
                    Bairro = cliente.Endereco?.Bairro,
                    Cidade = cliente.Endereco?.Cidade,
                    CEP = int.Parse(cliente.Endereco?.CEP ?? "0"),
                    Bloqueado = false
                };

                EF.Repositorio.Inserir(c);

                return Ok(new ResultadoOuErro("OK", c.IDCliente));
            }
            catch (Exception ex)
            {
                BLL.Logs.Erro(ex);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new ErroResult(10, ex)));
            }
        }

        [HttpGet]
        [Route("api/clientes/{id}/pedidos")]
        public IHttpActionResult ObterPedidosCliente(int id, [FromUri]int? pagina = 1, [FromUri]int? qtd = 10, [FromUri]decimal? maiorQue = 0, [FromUri] decimal? menorQue = null, [FromUri] string dataInicio = null, [FromUri] string dataFim = null)
        {
            try
            {
                var cliente = BLL.Cliente.Carregar(id);
                if (cliente?.IDCliente != null && !string.IsNullOrWhiteSpace(cliente?.NomeCompleto))
                {
                    if (pagina < 1)
                        pagina = 1;

                    if (qtd < 1)
                        qtd = 1;

                    if (qtd > 20)
                        qtd = 20;

                    if (maiorQue != null && maiorQue < 0)
                        maiorQue = 0;

                    if (menorQue != null && menorQue < 0)
                        menorQue = 0;

                    var skip = (pagina.Value - 1) * qtd.Value;
                    var limit = qtd.Value;

                    var lista = BLL.Pedido.ListarFinalizadosPorCliente(cliente);
                    lista = lista.Skip(skip).Take(limit);

                    if (maiorQue.HasValue)
                        lista = lista.Where(p => p.ValorTotal >= maiorQue.Value);

                    if (menorQue.HasValue && menorQue > maiorQue)
                        lista = lista.Where(p => p.ValorTotal <= menorQue.Value);

                    if (!string.IsNullOrWhiteSpace(dataInicio))
                    {
                        try
                        {
                            var dtInicio = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(dataInicio)).ToLocalTime().DateTime;
                            lista = lista.Where(p => p.DtPedido >= dtInicio);
                        }
                        catch (Exception)
                        {
                            throw new ArgumentException("Data inválida", "dataInicio");
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(dataFim))
                    {
                        try
                        {
                            var dtFim = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(dataFim)).ToLocalTime().DateTime;
                            lista = lista.Where(p => p.DtPedido <= dtFim);
                        }
                        catch (Exception)
                        {
                            throw new ArgumentException("Data inválida", "dataFim");
                        }
                    }


                    var result = new List<Pedido>();
                    foreach (var item in lista)
                    {
                        item.Cliente = cliente;
                        var pedido = DTO.Converter(item);
                        var produtos = BLL.PedidoProduto.ListarPorPedido(item.IDPedido.Value).Where(pp => pp.Cancelado == false);
                        var pagamentos = BLL.PedidoPagamento.ListarNaoCanceladoPorPedido(item.IDPedido.Value);

                        pedido.Itens = produtos.Select(p => DTO.Converter(p)).ToList();
                        pedido.Pagamentos = pagamentos.Select(p => DTO.Converter(p)).ToList();

                        result.Add(pedido);
                    }

                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (ArgumentException ae)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(10, ae)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(10, ex)));
            }
        }

        [HttpGet]
        [Route("api/clientes/{id}/saldo")]
        public IHttpActionResult SaldoPorCliente(int id)
        {
            try
            {
                var saldo = BLL.Saldo.ClienteSaldoLiquido(id);
                return Ok(new ResultadoOuErro() { Mensagem = "OK", Valor = saldo });
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(10, ex)));
            }
        }
    }
}
