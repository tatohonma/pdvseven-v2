using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Model;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static a7D.PDV.BLL.Mesa;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class MesasController : ApiController
    {
        [HttpGet]
        [Route("api/mesas/{numero}/status")]
        public IHttpActionResult Status([FromUri] int numero)
        {
            try
            {
                var mesa = CarregarPorNumero(numero);
                if (mesa?.IDMesa == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Mesa não existe")));
                //Validar(numero);

                var pedidoAberto = BLL.Pedido.ObterPedidoAbertoPorMesa(mesa.Numero.Value);
                return Ok(RespostaStatus(mesa.StatusMesa.IDStatusMesa.Value, pedidoAberto?.IDPedido));
            }
            catch (BLL.ExceptionPDV ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(30 + ex.CodigoRetorno, ex)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }

        [HttpGet]
        [Route("api/mesas/{numero}/total")]
        public IHttpActionResult StatusTotal([FromUri] int numero)
        {
            try
            {
                var mesa = CarregarPorNumero(numero);
                if (mesa?.IDMesa == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Mesa não existe")));

                var pedido = BLL.Pedido.ObterPedidoAbertoPorMesa(mesa.Numero.Value);
                decimal? valorPago = null;
                if (pedido != null)
                {
                    pedido.ListaProduto = BLL.PedidoProduto.ListarPorPedido(pedido.IDPedido.Value);
                    pedido.ListaPagamento = BLL.PedidoPagamento.ListarNaoCanceladoPorPedido(pedido.IDPedido.Value);
                    valorPago = pedido.ListaPagamento.Sum(p => p.Valor);
                }

                return Ok(RespostaTotal(
                    mesa.StatusMesa.IDStatusMesa.Value,
                    pedido?.IDPedido,
                    pedido?.ValorTotalTemp,
                    pedido?.ValorServicoTemp,
                    valorPago,
                    pedido?.Cliente?.NomeCompleto,
                    pedido?.Cliente?.Documento1,
                    pedido?.DocumentoCliente));
            }
            catch (BLL.ExceptionPDV ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(30 + ex.CodigoRetorno, ex)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }

        private dynamic RespostaStatus(int status, int? idPedidoAberto)
        {

            return new
            {
                status = status,
                idPedidoAberto = idPedidoAberto == null ? 0 : idPedidoAberto
            };
        }

        private Mesa RespostaTotal(int status, int? idPedidoAberto, decimal? valorTotal, decimal? valorServico, decimal? valorPago, string nome, string documento, string fiscal)
        {
            return new Mesa
            {
                status = status,
                idPedidoAberto = idPedidoAberto == null ? 0 : idPedidoAberto,
                ValorTotal = valorTotal,
                ValorServico = valorServico,
                ValorPago = valorPago,
                ClienteNome = nome,
                ClienteDocumento = documento,
                DocumentoFiscal = fiscal
            };
        }

        [HttpGet]
        [Route("api/mesas/{numero}/itens")]
        public IHttpActionResult GetItens([FromUri] int numero)
        {
            try
            {
                var mesa = CarregarPorNumero(numero);
                if (mesa?.IDMesa == null)
                    return NotFound();

                var pedido = BLL.Pedido.CarregarUltimoPedido(mesa.GUIDIdentificacao);
                if (pedido?.IDPedido == null)
                    return Ok(new Pedido());

                var id = pedido.IDPedido.Value;
                pedido = BLL.Pedido.CarregarCompleto(id);

                var mCliente = DTO.Converter(pedido.Cliente);
                var mTipoEntrada = DTO.Converter(pedido.TipoEntrada);

                pedido.ListaProduto = BLL.PedidoProduto.ListarPorPedido(id);
                foreach (var pedidoProduto in pedido.ListaProduto)
                {
                    pedidoProduto.Produto = BLL.Produto.CarregarCompleto(pedidoProduto.Produto.IDProduto.Value);
                    foreach (var pedidoModificacao in pedidoProduto.ListaModificacao)
                    {
                        pedidoModificacao.Produto = BLL.Produto.CarregarCompleto(pedidoModificacao.Produto.IDProduto.Value);
                    }
                }

                BLL.Pedido.AdicionarProdutoServico(pedido, true, new PDVInformation() { IDPDV = 1 }, new UsuarioInformation() { IDUsuario = 1 });
                BLL.Pedido.AdicionarProdutoConsumacaoMinima(pedido, new PDVInformation() { IDPDV = 1 }, new UsuarioInformation() { IDUsuario = 1 });

                var mItens = BLL.PedidoProduto.FiltrarSemCancelados(pedido.ListaProduto).Select(pp => DTO.Converter(pp)).ToList();
                var mItensCancelados = BLL.PedidoProduto.FiltrarApenasCancelados(pedido.ListaProduto).Where(p => p.Cancelado.Value).Select(pp => DTO.Converter(pp)).ToList();

                pedido.ListaPagamento = BLL.PedidoPagamento.ListarNaoCanceladoPorPedido(id);
                foreach (var pedidoPagamento in pedido.ListaPagamento)
                {
                    pedidoPagamento.TipoPagamento = BLL.TipoPagamento.Carregar(pedidoPagamento.TipoPagamento.IDTipoPagamento.Value);
                }
                var mPagamentos = pedido.ListaPagamento?.Select(pp => DTO.Converter(pp))?.ToList();

                return Ok(new Pedido(id,
                    pedido.NumeroComanda, pedido.NumeroMesa,
                    mCliente, pedido.TipoPedido.IDTipoPedido, pedido.StatusPedido.IDStatusPedido, mTipoEntrada, pedido.GUIDIdentificacao, pedido.DtPedido, pedido.DtPedidoFechamento, pedido.DtEnvio, pedido.DtEntrega, pedido.ValorServico, pedido.ValorDesconto, pedido.ValorEntrega, pedido.ValorTotalTemp, pedido.NumeroPessoas, mItens, mItensCancelados, mPagamentos));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(60, ex)));
            }
        }

        [HttpGet]
        [Route("api/mesas/{numero}/referencia")]
        public IHttpActionResult GetReferencia([FromUri] int numero)
        {
            try
            {
                var mesa = CarregarPorNumero(numero);
                if (mesa?.IDMesa == null)
                    return NotFound();
                var pedido = BLL.Pedido.CarregarUltimoPedido(mesa.GUIDIdentificacao);
                return Ok(new Referencia(pedido.ReferenciaLocalizacao));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(40, ex)));
            }
        }

        [HttpPost]
        [Route("api/mesas/{numero}/referencia")]
        public IHttpActionResult PostReferencia([FromUri] int numero, [FromBody] Referencia referencia)
        {
            try
            {
                var mesa = CarregarPorNumero(numero);
                if (mesa?.IDMesa == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Mesa não existe")));

                var pedido = BLL.Pedido.CarregarUltimoPedido(mesa.GUIDIdentificacao);
                if (pedido?.IDPedido == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Pedido não existe")));

                if (pedido.StatusPedido.StatusPedido != EStatusPedido.Aberto)
                    return NaoPodeSerAtualizada("Status Inválido");

                pedido.ReferenciaLocalizacao = referencia._Referencia;
                BLL.Pedido.Salvar(pedido);
                return Ok(new Referencia(pedido.ReferenciaLocalizacao));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(40, ex)));
            }
        }

        private System.Web.Http.Results.ResponseMessageResult NaoPodeSerAtualizada(string msg)
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(40, msg)));
        }

        private new System.Web.Http.Results.ResponseMessageResult NotFound()
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound));
        }

        [HttpDelete]
        [Route("api/mesas/{numero}/referencia")]
        public IHttpActionResult DeleteReferencia([FromUri] int numero)
        {
            try
            {
                var mesa = CarregarPorNumero(numero);
                if (mesa?.IDMesa == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Mesa não existe")));

                var pedido = BLL.Pedido.CarregarUltimoPedido(mesa.GUIDIdentificacao);
                if (pedido?.IDPedido == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Pedido não existe")));

                if (pedido.StatusPedido.StatusPedido != EStatusPedido.Aberto)
                    return NaoPodeSerAtualizada("Status Inválido");

                pedido.ReferenciaLocalizacao = string.Empty;
                BLL.Pedido.Salvar(pedido);
                return Ok(new Referencia(pedido.ReferenciaLocalizacao));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(40, ex)));
            }
        }
    }
}
