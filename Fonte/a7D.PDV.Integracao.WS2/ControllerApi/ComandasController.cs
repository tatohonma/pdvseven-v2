using a7D.PDV.BLL;
using a7D.PDV.EF;
using a7D.PDV.EF.DAO;
using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Model;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class ComandasController : ApiController
    {
        public static ComandaInformation CarregarPorCodigo(string texto)
        {
            var codigo = BLL.Comanda.HexaOuDecimalOuZero(texto, ConfiguracoesSistema.Valores.ComandaCodigoHEX);
            return BLL.Comanda.CarregarPorNumeroOuCodigo(codigo);
        }

        [HttpGet]
        [Route("api/comandas/{numero}/info/{tipo}")] // tipo: Numero, TAGHEX, TAGDEC
        public IHttpActionResult Info([FromUri] string numero, [FromUri] string tipo)
        {
            try
            {
                if (!Enum.TryParse(tipo, true, out ComandaNumeroTipo eTipoComanda))
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Tipo: " + tipo + " invalido")));

                var comanda = ComandaDAO.Carregar(numero, eTipoComanda);
                if (comanda == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Comanda não existe")));

                var result = new API2.Model.Comanda()
                {
                    status = comanda.IDStatusComanda,
                    Numero = comanda.Numero,
                    TAGDEC = comanda.Codigo,
                };

                if (comanda.Codigo != null)
                    result.TAGHEX = comanda.Codigo.Value.ToString("X");

                if (comanda.IDCliente != null)
                {
                    var cliente = ClienteDAO.Carregar(comanda.IDCliente.Value);
                    result.Cliente = DTO.Converter(cliente);
                }

                return Ok(result);
            }
            catch (ExceptionPDV ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(30 + ex.CodigoRetorno, ex)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }

        [HttpPost]
        [Route("api/comandas/{numero}/cliente/{tipo}")] // tipo: Numero, TAGHEX, TAGDEC
        public IHttpActionResult RegistraCliente(
            [FromUri] string numero,
            [FromUri] string tipo,
            [FromBody] string cliente)
        {
            try
            {
                if (!Enum.TryParse(tipo, true, out ComandaNumeroTipo eTipoComanda))
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Tipo: " + tipo + " invalido")));

                if (!int.TryParse(cliente, out int idCliente))
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("id cliente inválido")));

                var comanda = ComandaDAO.Carregar(numero, eTipoComanda);
                if (comanda == null)
                {
                    comanda = new EF.Models.tbComanda()
                    {
                        IDStatusComanda = 10,
                        GUIDIdentificacao = Guid.NewGuid().ToString(),
                        IDCliente = idCliente
                    };

                    if (eTipoComanda == ComandaNumeroTipo.Numero && int.TryParse(numero, out int n))
                        comanda.Numero = n;
                    else if (eTipoComanda == ComandaNumeroTipo.TAGDEC && long.TryParse(numero, out long l))
                    {
                        comanda.Codigo = l;
                    }
                    else if (eTipoComanda == ComandaNumeroTipo.TAGHEX)
                    {
                        try
                        {
                            long h = Convert.ToInt64(numero, 16);
                            comanda.Codigo = h;
                        }
                        catch (Exception ex)
                        {
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("tag hex invalido: " + ex.Message)));
                        }
                    }

                    if (comanda.Numero == 0)
                        comanda.Numero = Repositorio.ExecuteScalar<int>("SELECT MAX(numero) FROM tbComanda") + 1;

                    Repositorio.Inserir(comanda);
                }
                else
                {
                    comanda.IDCliente = idCliente;
                    Repositorio.Atualizar(comanda);
                }

                return Ok(new ResultadoOuErro("OK", comanda.IDComanda));
            }
            catch (ExceptionPDV ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(30 + ex.CodigoRetorno, ex)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }

        [HttpGet]
        [Obsolete]
        [Route("api/comandas/{numero}/status")]
        public IHttpActionResult Status([FromUri] string numero)
        {
            try
            {
                var comanda = CarregarPorCodigo(numero);
                if (comanda?.IDComanda == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Comanda não existe")));

                var pedidoAberto = BLL.Pedido.ObterPedidoAbertoPorComanda(comanda.Numero.Value);
                return Ok(new
                {
                    status = comanda.StatusComanda.IDStatusComanda,
                    numero = comanda.Numero,
                    idPedidoAberto = pedidoAberto?.IDPedido == null ? 0 : pedidoAberto?.IDPedido.Value
                });
            }
            catch (ExceptionPDV ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(30 + ex.CodigoRetorno, ex)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }

        [HttpGet]
        [Route("api/comandas/{numero}/saldo")] // Usado no EasyChoop
        public IHttpActionResult StatusSaldo([FromUri] string numero)
        {
            return StatusValores(numero, true);
        }

        [HttpGet]
        [Route("api/comandas/{numero}/total")]
        public IHttpActionResult StatusTotal([FromUri] string numero)
        {
            return StatusValores(numero, false);
        }

        public IHttpActionResult StatusValores(string numero, bool saldos)
        {
            try
            {
                var comanda = CarregarPorCodigo(numero);
                if (comanda?.IDComanda == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Comanda não existe")));

                var pedido = BLL.Pedido.ObterPedidoAbertoPorComanda(comanda.Numero.Value);
                decimal? valorPago = null;
                if (pedido != null)
                {
                    pedido.ListaProduto = BLL.PedidoProduto.ListarPorPedido(pedido.IDPedido.Value);
                    pedido.ListaPagamento = BLL.PedidoPagamento.ListarNaoCanceladoPorPedido(pedido.IDPedido.Value);
                    valorPago = pedido.ListaPagamento.Sum(p => p.Valor);
                }

                API2.Model.Cliente cliente = null;
                if (pedido?.Cliente?.IDCliente != null)
                    cliente = DTO.Converter(pedido?.Cliente);

                var resposta = new API2.Model.Comanda()
                {
                    Mensagem = "OK",
                    Numero = comanda.Numero,
                    status = comanda.StatusComanda.IDStatusComanda,
                    idPedidoAberto = pedido?.IDPedido == null ? 0 : pedido?.IDPedido.Value,
                    ValorTotal = pedido?.ValorTotalTemp,
                    ValorServico = pedido?.ValorServicoTemp,
                    ReferenciaLocalizacao = pedido?.ReferenciaLocalizacao,
                    ValorPago = valorPago,
                    ClienteNome = cliente?.NomeCompleto,
                    ClienteDocumento = cliente?.Documento1,
                    Cliente = cliente,
                    DocumentoFiscal = pedido?.DocumentoCliente
                };

                if (saldos && pedido != null)
                {
                    if (ConfiguracoesSistema.Valores.ComandaComCredito)
                    {
                        if (pedido.Cliente != null)
                        {
                            resposta.Credito = Saldo.ClienteSaldoLiquido(pedido.Cliente.IDCliente.Value, pedido?.IDPedido);
                            resposta.Credito -= resposta.ValorTotal;
                        }
                        else
                            resposta.Credito = 0;
                    }
                    else
                        resposta.Limite = ConfiguracoesSistema.Valores.LimiteComanda - resposta.ValorTotal;
                }

                return Ok(resposta);
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
        [Route("api/comandas/entrada")]
        public IHttpActionResult Entrada()
        {
            try
            {
                var tipoEntrada = BLL.TipoEntrada.BuscarPadrao();
                if (tipoEntrada?.IDTipoEntrada == null)
                    return Ok(new ErroResult("Não há um tipo de entrada padrão configurado"));

                return Ok(new Entrada()
                {
                    Mensagem = "OK",
                    idEntrada = tipoEntrada.IDTipoEntrada.Value,
                    Nome = tipoEntrada.Nome,
                    ValorEntrada = tipoEntrada?.ValorEntrada ?? 0m,
                    ValorConsumacaoMinima = tipoEntrada?.ValorConsumacaoMinima ?? 0m
                });
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(60, ex)));
            }
        }

        [HttpGet]
        [Route("api/comandas/tipoentradas")]
        public IHttpActionResult Entradas()
        {
            try
            {
                var entradas = EF.Repositorio.Listar<EF.Models.tbTipoEntrada>(e => e.Ativo == true);
                return Ok(entradas);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(60, ex)));
            }
        }

        [HttpPost]
        [Route("api/comandas/abrir")]
        public IHttpActionResult AbrirComanda([FromBody]ComandaAbrir abrir)
        {
            try
            {
                var comanda = CarregarPorCodigo(abrir.Comanda.ToString());
                if (comanda?.IDComanda == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Comanda não existe")));

                else if (comanda.ValorStatusComanda == EStatusComanda.EmUso)
                    return Ok(new ErroResult("Comanda já está em uso"));

                else if (comanda.ValorStatusComanda != EStatusComanda.Liberada)
                    return Ok(new ErroResult("Comanda não está liberada para abertura"));

                var clienteInfo = BLL.Cliente.Carregar(abrir.ClienteID);
                if (clienteInfo?.IDCliente == null)
                    return Ok(new ErroResult("Cliente não existe"));

                if (BLL.Comanda.ClienteTemComandaAberta(clienteInfo.IDCliente.Value))
                    return Ok(new ErroResult("Já existe comanda aberta para esse cliente"));

                TipoEntradaInformation tipoEntrada;
                if (abrir.IDTipoEntrada == null)
                {
                    tipoEntrada = BLL.TipoEntrada.BuscarPadrao();
                    if (tipoEntrada?.IDTipoEntrada == null)
                        return Ok(new ErroResult("Não há um tipo de entrada padrão configurado"));
                }
                else
                {
                    tipoEntrada = BLL.TipoEntrada.Carregar(abrir.IDTipoEntrada.Value);
                    if (tipoEntrada?.IDTipoEntrada == null)
                        return Ok(new ErroResult("Tipo de entrada invalida"));
                }

                if (abrir.Validar)
                    return Ok(new ResultadoOuErro("OK"));

                var pedido = BLL.Comanda.AbrirComanda(comanda, clienteInfo, tipoEntrada, abrir.PDVID, abrir.UsuarioID);

                return Ok(new ResultadoOuErro("OK", pedido.IDPedido.Value));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(60, ex)));
            }
        }

        [HttpGet]
        [Route("api/comandas/{numero}/itens")]
        public IHttpActionResult GetItens([FromUri] string numero)
        {
            try
            {
                var comanda = CarregarPorCodigo(numero);
                if (comanda?.IDComanda == null)
                    return NotFound();

                var pedido = BLL.Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);
                if (pedido?.IDPedido == null)
                    return Ok(new API2.Model.Pedido());

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

                return Ok(new API2.Model.Pedido(id,
                    pedido.NumeroComanda, pedido.NumeroMesa,
                    mCliente, pedido.TipoPedido.IDTipoPedido, pedido.StatusPedido.IDStatusPedido, mTipoEntrada, pedido.GUIDIdentificacao, pedido.DtPedido, pedido.DtPedidoFechamento, pedido.DtEnvio, pedido.DtEntrega, pedido.ValorServico, pedido.ValorDesconto, pedido.ValorEntrega, pedido.ValorTotalTemp, pedido.NumeroPessoas, mItens, mItensCancelados, mPagamentos));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(60, ex)));
            }
        }

        [HttpGet]
        [Route("api/comandas/{numero}/referencia")]
        public IHttpActionResult GetReferencia([FromUri] string numero)
        {
            try
            {
                var comanda = CarregarPorCodigo(numero);
                if (comanda?.IDComanda == null)
                    return NotFound();
                var pedido = BLL.Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);
                return Ok(new Referencia(pedido.ReferenciaLocalizacao));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }

        [HttpPost]
        [Route("api/comandas/{numero}/referencia")]
        public IHttpActionResult PostReferencia([FromUri] string numero, [FromBody] Referencia referencia)
        {
            try
            {
                var comanda = CarregarPorCodigo(numero);
                if (comanda?.IDComanda == null)
                    return NotFound();
                var pedido = BLL.Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);
                if (pedido?.IDPedido == null)
                    return NotFound();
                if (pedido.StatusPedido.StatusPedido != EStatusPedido.Aberto)
                    return NaoPodeSerAtualizada("Status Inválido");

                pedido.ReferenciaLocalizacao = referencia._Referencia;
                BLL.Pedido.Salvar(pedido);
                return Ok(new Referencia(pedido.ReferenciaLocalizacao));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }

        private System.Web.Http.Results.ResponseMessageResult NaoPodeSerAtualizada(string msg)
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(30, msg)));
        }

        private new System.Web.Http.Results.ResponseMessageResult NotFound()
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound));
        }

        [HttpDelete]
        [Route("api/comandas/{numero}/referencia")]
        public IHttpActionResult DeleteReferencia([FromUri] string numero)
        {
            try
            {
                var comanda = CarregarPorCodigo(numero);
                if (comanda?.IDComanda == null)
                    return NotFound();
                var pedido = BLL.Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);
                if (pedido?.IDPedido == null)
                    return NotFound();
                if (pedido.StatusPedido.StatusPedido != EStatusPedido.Aberto)
                    return NaoPodeSerAtualizada("Status Inválido");

                pedido.ReferenciaLocalizacao = string.Empty;
                BLL.Pedido.Salvar(pedido);
                return Ok(new Referencia(pedido.ReferenciaLocalizacao));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }


        [HttpGet]
        [Route("api/comandas/{numero}/extrato")]
        public IHttpActionResult ExtratoItens([FromUri] string numero)
        {
            try
            {
                var comanda = CarregarPorCodigo(numero);
                if (comanda?.IDComanda == null)
                    return NotFound();
                var pedido = BLL.Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);
                if (pedido?.IDPedido == null)
                    return NotFound();
                if (pedido.StatusPedido.StatusPedido != EStatusPedido.Aberto)
                    return NaoPodeSerAtualizada("Status Inválido");

                var extrato = BLL.Saldo.ExtratoItens(pedido.Cliente.IDCliente.Value); //, pedido.GUIDAgrupamentoPedido);
                return Ok(extrato);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(30, ex)));
            }
        }
    }
}
