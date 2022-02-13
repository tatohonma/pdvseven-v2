using a7D.PDV.Ativacao.Shared.Services;
using a7D.PDV.EF.Enum;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class PedidosController : ApiController
    {

        private HashSet<string> Exec
        {
            get
            {
                if (HttpContext.Current.Application.Get("ExecDictionary") == null)
                {
                    HttpContext.Current.Application.Set("ExecDictionary", new HashSet<string>());
                }
                return (HashSet<string>)HttpContext.Current.Application.Get("ExecDictionary");
            }
        }
        private static object @lock = new object();

        [HttpGet]
        public IHttpActionResult GetPedido(int id)
        {
            try
            {
                var pedido = BLL.Pedido.CarregarCompleto(id);

                if (pedido == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Pedido não existe")));

                return Ok(PreencheInformacoesPedido(id, pedido));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(60, ex)));
            }
        }

        [HttpGet]
        [Route("api/pedidos/abertos")]
        public IHttpActionResult GetPedidosAbertos([FromUri] string segundosAtras = "", [FromUri] string idTipoPedido = "")
        {
            try
            {
                var pedidos = BLL.Pedido.ListarPendentes().Where(p => p.StatusPedido.IDStatusPedido.Value == (int)EStatusPedido.Aberto);

                if (!string.IsNullOrEmpty(idTipoPedido))
                {
                    int _idTipoPedido = Convert.ToInt32(idTipoPedido);
                    pedidos = pedidos.Where(p => p.TipoPedido.IDTipoPedido.Value == _idTipoPedido);
                }

                if (!string.IsNullOrEmpty(segundosAtras))
                {
                    int _segundosAtras = Convert.ToInt32(segundosAtras);
                    var dataBusca = DateTime.Now.AddSeconds(-_segundosAtras);
                    pedidos = pedidos.Where(p => p.DtPedido.Value > dataBusca).ToList();
                }

                var result = new List<Pedido>();
                foreach (var pedidoInformation in pedidos.OrderBy(p => p.DtPedido))
                {
                    var pedido = BLL.Pedido.CarregarCompleto(pedidoInformation.IDPedido.Value);
                    result.Add(PreencheInformacoesPedido(pedidoInformation.IDPedido.Value, pedido));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(65, ex)));
            }
        }

        [HttpGet]
        [Route("api/pedidos/{idTipoPedido}/{numero}/aberto")]
        public IHttpActionResult GetPedidoAberto(int idTipoPedido, int numero)
        {
            try
            {
                PedidoInformation pedido = null;
                if (idTipoPedido == (int)ETipoPedido.Mesa)
                {
                    pedido = BLL.Pedido.ObterPedidoAbertoPorMesa(numero);
                }
                else if (idTipoPedido == (int)ETipoPedido.Comanda)
                {
                    pedido = BLL.Pedido.ObterPedidoAbertoPorComanda(numero);
                }
                else
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(40, "IDTipoPedido inválido para esta pesquisa")));
                }

                if (pedido == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Pedido não existe")));

                return Ok(PreencheInformacoesPedido(pedido.IDPedido.Value, pedido));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(60, ex)));
            }
        }

        [HttpGet]
        public IHttpActionResult GetPedidos(
            [FromUri]string dataInicio = null,
            [FromUri]string dataFim = null,
            [FromUri]int IDCliente = 0,
            [FromUri]int pagina = 1,
            [FromUri]int qtRegistros = 20
            )
        {
            try
            {
                DateTime dtFechamentoMinimo = new DateTime(2000, 1, 1);
                DateTime dtFechamentoMaximo = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(dataInicio))
                {
                    try
                    {
                        dtFechamentoMinimo = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(dataInicio)).DateTime;
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
                        dtFechamentoMaximo = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(dataFim)).DateTime;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("Data inválida", "dataFim");
                    }
                }

                dtFechamentoMinimo = TimeZoneInfo.ConvertTimeFromUtc(dtFechamentoMinimo, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
                dtFechamentoMaximo = TimeZoneInfo.ConvertTimeFromUtc(dtFechamentoMaximo, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

                var countFinalizadosNoIntervalo = BLL.Pedido.CountFinalizadosNoIntervalo(dtFechamentoMinimo, dtFechamentoMaximo, IDCliente);
                var qtPaginas = (int)Math.Ceiling((double)countFinalizadosNoIntervalo / qtRegistros);
                var skip = (pagina * qtRegistros) - qtRegistros;

                var cancellationToken = new CancellationTokenSource();
                var ct = cancellationToken.Token;
                var lista = BLL.Pedido.ListarFinalizadosNoIntervalo(dtFechamentoMinimo, dtFechamentoMaximo, IDCliente, skip, qtRegistros, ct);
                var clientes = BLL.Cliente.Listar();

                var result = new List<Pedido>();
                foreach (var pedidoInformation in lista.OrderByDescending(p => p.DtPedidoFechamento))
                {
                    if (pedidoInformation.Cliente?.IDCliente != null)
                    {
                        pedidoInformation.Cliente = clientes.FirstOrDefault(c => c.IDCliente == pedidoInformation.Cliente.IDCliente.Value);
                    }
                    var pedido = DTO.Converter(pedidoInformation);

                    var produtos = BLL.PedidoProduto.ListarPorPedido(pedidoInformation.IDPedido.Value);
                    var pagamentos = BLL.PedidoPagamento.ListarNaoCanceladoPorPedido(pedidoInformation.IDPedido.Value);

                    pedido.Itens = BLL.PedidoProduto.FiltrarSemCancelados(produtos).Select(p => DTO.Converter(p)).ToList();
                    pedido.ItensCancelados = BLL.PedidoProduto.FiltrarApenasCancelados(produtos).Select(p => DTO.Converter(p)).ToList();
                    pedido.Pagamentos = pagamentos.Select(p => DTO.Converter(p)).ToList();

                    result.Add(pedido);
                }

                var resposta = Request.CreateResponse(HttpStatusCode.OK, result);
                resposta.Headers.Add("countTotalPesquisa", countFinalizadosNoIntervalo.ToString());
                resposta.Headers.Add("countPaginas", qtPaginas.ToString());
                resposta.Headers.Add("paginaAtual", pagina.ToString());
                return ResponseMessage(resposta);
            }
            catch (ArgumentException ae)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(60, ae)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(60, ex)));
            }
        }

        [HttpPost]
        public IHttpActionResult AddPedido([FromBody] AdicionarProdutos adicionarProdutos)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(adicionarProdutos.GUIDSolicitacao))
                    throw new ArgumentNullException("adicionarProdutos.GUIDSolicitacao");

                if (adicionarProdutos.Itens == null)
                    throw new ArgumentNullException("adicionarProdutos.Itens");

                if (adicionarProdutos.IDPDV < 1)
                    throw new ArgumentOutOfRangeException("adicionarProdutos.IDPDV", "IDPDV precisa ser maior que 0");

                if (!adicionarProdutos.IDUsuario.HasValue || adicionarProdutos.IDUsuario < 1)
                    throw new ArgumentOutOfRangeException("adicionarProdutos.IDUsuario", "IDUsuario precisa ser maior que 0");

                if (adicionarProdutos.Itens.Count == 0)
                    return Ok();
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(60, ex)));
            }
            try
            {
                var pdv = BLL.PDV.Carregar(adicionarProdutos.IDPDV.Value);
                var usuario = BLL.Usuario.Carregar(adicionarProdutos.IDUsuario.Value);

                Executando(adicionarProdutos.GUIDSolicitacao);

                if (BLL.PedidoProduto.PedidoDuplicado(adicionarProdutos.GUIDSolicitacao))
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(60, "Este GUID já existe.")));


                var guidIdentificacao = string.Empty;
                var tipoPedido = (ETipoPedido)adicionarProdutos.IDTipoPedido;
                ClienteInformation cliente = null;

                switch (tipoPedido)
                {
                    case ETipoPedido.Mesa:
                        int nMesa = int.Parse(adicionarProdutos.Numero.ToString());
                        BLL.Mesa.Validar(nMesa);
                        var mesa = BLL.Mesa.CarregarPorNumero(nMesa);
                        guidIdentificacao = mesa.GUIDIdentificacao;
                        break;

                    case ETipoPedido.Comanda:
                        long nComanda = BLL.Comanda.HexaOuDecimal(adicionarProdutos.Numero.ToString(), BLL.ConfiguracoesSistema.Valores.ComandaCodigoHEX);
                        BLL.Comanda.Validar(nComanda);
                        ComandaInformation comanda = BLL.Comanda.CarregarPorNumeroOuCodigo(nComanda);
                        guidIdentificacao = comanda.GUIDIdentificacao;
                        break;

                    case ETipoPedido.Balcao: // Venda Balcão, possivel identificação de cliente (compra de credito)
                        if (adicionarProdutos.Numero != null && int.TryParse(adicionarProdutos.Numero.ToString(), out int idCliente))
                            cliente = BLL.Cliente.Carregar(idCliente);
                        break;

                    default:
                        throw new NotImplementedException($"IDTipoPedido {adicionarProdutos.IDTipoPedido} não foi implementado");
                }

                var listaPedidoProduto = new List<PedidoProdutoInformation>();
                int nSeq = -1;
                foreach (var item in adicionarProdutos.Itens.Where(i => i.IDProduto.HasValue && i.Qtd > 0))
                {
                    ProdutoInformation produto = BLL.Produto.ObterProduto(item.IDProduto.Value);

                    PedidoProdutoInformation pedidoProduto = ObterPedidoProduto(
                        adicionarProdutos.GUIDSolicitacao, pdv, usuario, item.Notas, produto, item.Qtd.Value, item.Preco ?? produto.ValorUnitario.Value);

                    pedidoProduto.IDPedidoProduto = nSeq--;

                    if (item.Modificacoes != null)
                    {
                        foreach (var mod in item.Modificacoes)
                        {
                            var produtoMod = BLL.Produto.ObterProduto(mod.IDProduto.Value);
                            var precoMod = produto.ValorUnitario.Value;

                            var itemModificacao = ObterPedidoProduto(adicionarProdutos.GUIDSolicitacao, pdv, usuario, mod.Notas, produtoMod, mod.Qtd ?? 1, mod.Preco ?? 0);
                            itemModificacao.IDPedidoProduto = nSeq--;
                            itemModificacao.PedidoProdutoPai = pedidoProduto;
                            pedidoProduto.ListaModificacao.Add(itemModificacao);
                        }
                    }
                    listaPedidoProduto.Add(pedidoProduto);
                }

                if (listaPedidoProduto.Count == 0)
                    return Ok(new ResultadoOuErro("Não há produtos"));

                if (!adicionarProdutos.ValidarLimite.HasValue)
                    adicionarProdutos.ValidarLimite = pdv.TipoPDV.Tipo != ETipoPDV.EASYCHOPP;

                var pedido = BLL.Pedido.AdicionarProduto(tipoPedido, guidIdentificacao, usuario.IDUsuario.Value, pdv.IDPDV.Value, listaPedidoProduto, adicionarProdutos.ValidarLimite.Value, cliente);

                BLL.GA.PostEvento(pdv, "Pedido: " + tipoPedido, usuario: usuario);

                try
                {
                    if (adicionarProdutos.GerarOrdemProducao ?? true)
                        BLL.OrdemProducaoServices.GerarOrdemProducao(listaPedidoProduto);

                    var imprimirViaExpedicao = BLL.ConfiguracaoBD.ValorOuPadrao(EConfig.ImprimirViaExpedicao, pdv);
                    if (imprimirViaExpedicao == "PEDIDO" && pedido != null)
                    {
                        var idAreaViaExpedicao = int.Parse(BLL.ConfiguracaoBD.ValorOuPadrao(EConfig.IDAreaViaExpedicao, pdv));
                        BLL.OrdemProducaoServices.GerarViaExpedicao(pedido.IDPedido.Value, idAreaViaExpedicao);
                    }
                }
                catch (Exception ex)
                {
                    BLL.Logs.Erro(ex);
                }

                return Ok(new ResultadoOuErro("OK", pedido.IDPedido.Value));
            }
            catch (BLL.ExceptionPDV ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(60 + ex.CodigoRetorno, ex)));
            }
            catch (Exception ex)
            {
                var code = HttpStatusCode.InternalServerError;
                if (ex is ArgumentNullException || ex is ArgumentOutOfRangeException || ex is NotImplementedException)
                    code = HttpStatusCode.BadRequest;
                return ResponseMessage(Request.CreateResponse(code, new Erro(60, ex)));
            }
            finally
            {
                Exec.Remove(adicionarProdutos.GUIDSolicitacao);
            }
        }

        [HttpGet]
        [Route("api/pedidos/finaliza")]
        public IHttpActionResult FecharGET()
        {
            var fechamento = new FechamentoPedido();
            fechamento.IDPedido = int.Parse(HttpContext.Current.Request["idPedido"]);
            fechamento.IDPdv = int.Parse(HttpContext.Current.Request["idPDV"]);
            fechamento.ChaveAcesso = HttpContext.Current.Request["ChaveAcesso"];
            fechamento.GerarOrdemProducao = HttpContext.Current.Request["GerarOrdemProducao"] == "1";
            fechamento.DocNfe = HttpContext.Current.Request["DocNfe"];
            fechamento.DocFidelidade = HttpContext.Current.Request["DocFidelidade"];
            fechamento.Cancelar = HttpContext.Current.Request["Cancelar"] == "1";
            string comprovante = HttpContext.Current.Request["Comprovante"];
            fechamento.ImagemComprovante = comprovante == null ? default(int?) : int.Parse(comprovante);
            return FecharPedido(fechamento.IDPedido, fechamento);
        }

        [HttpPost]
        [Route("api/pedidos/fechar")]
        public IHttpActionResult FecharPedido([FromBody] FechamentoPedido fechamento)
        {
            return FecharPedido(fechamento.IDPedido, fechamento);
        }

        [HttpPost]
        [Route("api/pedidos/{idPedido}/fechar")]
        public IHttpActionResult FecharPedido(int idPedido, [FromBody] FechamentoPedido fechamento)
        {
            PedidoInformation pedido = null;
            try
            {
                if (fechamento == null)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(40, "Os parametros necessários não foram enviados")));
                }
                else if (fechamento.IDPdv == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(40, "PDV não enviado")));
                }
                else if (string.IsNullOrEmpty(fechamento.ChaveAcesso))
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(40, "Chave de acesso não informada")));
                }

                var pdv = BLL.PDV.Carregar(fechamento.IDPdv);
                if (pdv == null)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Unauthorized, new Erro(50, "PDV não encontrado")));
                }

                var usuario = BLL.Usuario.Autenticar(fechamento.ChaveAcesso);
                if (usuario == null)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Unauthorized, new Erro(50, "Chave de acesso não autorizada")));
                }

                pedido = BLL.Pedido.CarregarCompleto(idPedido);

                if (pedido?.TipoPedido == null)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new Erro(40, "Pedido não encontrado")));
                }
                else if (pedido.StatusPedido.StatusPedido != EStatusPedido.Aberto)
                {
                    if (fechamento.Cancelar == true)
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(40, "Este pedido não pode ser cancelado. Status do pedido atual é " + pedido.StatusPedido.IDStatusPedido)));
                    else
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(40, "Este pedido não pode ser fechado. Status do pedido atual é " + pedido.StatusPedido.IDStatusPedido)));
                }

                if (fechamento.Cancelar == true)
                {
                    BLL.Pedido.Cancelar(pedido, usuario.IDUsuario.Value);
                    return Ok(new ResultadoOuErro("OK", pedido.IDPedido.Value));
                }

                if (!string.IsNullOrEmpty(fechamento.DocNfe))
                {
                    if (BLL.ValidacaoDocumento.IsCpf(fechamento.DocNfe))
                        pedido.DocumentoCliente = fechamento.DocNfe;
                    else if (BLL.ValidacaoDocumento.IsCnpj(fechamento.DocNfe))
                        pedido.DocumentoCliente = fechamento.DocNfe;
                    else
                        throw new Exception("Documento informado inválido.");
                }

                //if (TipoPedidoEmUso(pedido).Erro == true)
                //    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(40, "Este pedido não pode ser fechado. Status da comanda/mesa atual é " + TipoPedidoEmUso(pedido).Status)));

                AlterarStatusContaSolicitada(pedido);

                BLL.Pedido.AdicionarProdutoServico(pedido, true, pdv, usuario);
                BLL.Pedido.AdicionarProdutoConsumacaoMinima(pedido, pdv, usuario);

                if (!ComparaValorTotalPedidoValorItens(pedido))
                    throw new BLL.ExceptionPDV(BLL.CodigoErro.EEA0);

                pedido.StatusPedido.StatusPedido = EStatusPedido.Finalizado;

                var tiposPagamentos = pedido.ListaPagamento.Select(t => t.TipoPagamento.IDTipoPagamento.Value).ToArray();
                var caixaPDV = BLL.Caixa.UsaOuAbre(fechamento.IDPdv, usuario.IDUsuario.Value, tiposPagamentos);

                pedido.Caixa = caixaPDV;

                if (fechamento.GerarOrdemProducao ?? false)
                    BLL.OrdemProducaoServices.GerarOrdemProducao(pedido.ListaProduto);

                bool fiscal = BLL.ConfiguracoesSistema.Valores.Fiscal != "none";

                if ((pdv.TipoPDV.Tipo == ETipoPDV.POS_INTEGRADO_NTK || pdv.TipoPDV.Tipo == ETipoPDV.POS_INTEGRADO_STONE)
                 && !BLL.ConfiguracoesSistema.Valores.IntegrarSATPOS)
                    fiscal = false;

                if (fiscal)
                {
                    try
                    {
                        // Mesma logica do Caixa
                        if (pedido.ValorTotalProdutosServicos > 0 && (pedido.ValorDesconto ?? 0) < pedido.ValorTotalProdutosServicos)
                        {
                            var RetornoSat = FiscalServices
                                .Venda(pedido, true, pdv.IDPDV.Value, usuario.IDUsuario.Value)
                                .Enviar();
                            pedido.RetornoSAT_venda = RetornoSat;
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.Logs.Erro(ex);
                    }
                }

                if (pedido.Cliente?.IDCliente == null && !string.IsNullOrEmpty(fechamento.DocFidelidade))
                {
                    var cliente = BLL.Cliente.BuscarCliente(BLL.Cliente.TipoCliente.CPFCNPJ, fechamento.DocFidelidade);
                    if (cliente != null)
                        pedido.Cliente = new ClienteInformation() { IDCliente = cliente.IDCliente };
                }

                BLL.Pedido.FecharVendaDB(pedido, pedido.Caixa, usuario.IDUsuario.Value);

                if (pedido.StatusPedido.StatusPedido == EStatusPedido.Finalizado)
                    AlterarStatusLiberado(pedido);

                BLL.EntradaSaida.Movimentar(pedido);


                var result = new ResultadoOuErro("OK", pedido.IDPedido.Value);
                if (fechamento.ImagemComprovante > 0)
                {
                    string prms = $"{pedido.IDPedido}-{fechamento.ImagemComprovante}";

                    byte[] bt;
                    if (BLL.ConfiguracoesSistema.Valores.Fiscal == "SAT")
                        bt = ImpressaoController.ImagemBytesSAT(prms);
                    else
                        bt = ImpressaoController.ImagemBytesConta(prms);

                    if (bt != null)
                        result.Arquivo = Convert.ToBase64String(bt);
                }

                BLL.GA.PostEvento(pdv, "Fechar: " + pedido.TipoPedido.TipoPedido.ToString(), usuario: usuario);

                return Ok(result);
            }
            catch (Exception ex)
            {
                BLL.Logs.Erro(ex);
                AlterarStatusEmUso(pedido);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, ex.Message)));
            }
        }

        private static Pedido PreencheInformacoesPedido(int id, PedidoInformation pedido)
        {
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
            AddProdutoEntradaCM(pedido);

            var mItens = BLL.PedidoProduto.FiltrarSemCancelados(pedido.ListaProduto).Select(pp => DTO.Converter(pp)).ToList();
            var mItensCancelados = BLL.PedidoProduto.FiltrarApenasCancelados(pedido.ListaProduto).Where(p => p.Cancelado.Value).Select(pp => DTO.Converter(pp)).ToList();

            pedido.ListaPagamento = BLL.PedidoPagamento.ListarNaoCanceladoPorPedido(id);
            foreach (var pedidoPagamento in pedido.ListaPagamento)
            {
                pedidoPagamento.TipoPagamento = BLL.TipoPagamento.Carregar(pedidoPagamento.TipoPagamento.IDTipoPagamento.Value);
            }
            var mPagamentos = pedido.ListaPagamento?.Select(pp => DTO.Converter(pp))?.ToList();


            var estabelecimento = new Estabelecimento();
            var retornoPedido = new Pedido(id, pedido.NumeroComanda, pedido.NumeroMesa,
                mCliente, pedido.TipoPedido.IDTipoPedido,
                pedido.StatusPedido.IDStatusPedido, mTipoEntrada,
                pedido.GUIDIdentificacao, pedido.DtPedido,
                pedido.DtPedidoFechamento, pedido.DtEnvio,
                pedido.DtEntrega, pedido.ValorServico,
                pedido.ValorDesconto, pedido.ValorEntrega,
                pedido.ValorTotalTemp, pedido.NumeroPessoas,
                mItens, mItensCancelados, mPagamentos,
                estabelecimento, pedido.DocumentoCliente);


            return retornoPedido;
        }


        [HttpPost]
        [Route("api/pedidos/{idPedido}/conta-solicitada")]
        public IHttpActionResult AlterarStatusComandaParaContaSolicitada([FromUri] string idPedido)
        {
            PedidoInformation pedido = new PedidoInformation();
            try
            {
                pedido = BLL.Pedido.CarregarCompleto(Convert.ToInt32(idPedido));

                if (pedido == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new Erro(49, "Pedido não existe")));

                AlterarStatusContaSolicitada(pedido);

                return Ok();
            }
            catch (Exception ex)
            {
                if (pedido != null)
                {
                    AlterarStatusEmUso(pedido);
                }

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, ex.Message)));
            }
        }

        private void AlterarStatusContaSolicitada(PedidoInformation pedido)
        {
            if (pedido?.TipoPedido.IDTipoPedido == (int)ETipoPedido.Comanda)
            {
                BLL.Comanda.AlterarStatus(pedido.GUIDIdentificacao, EStatusComanda.ContaSolicitada);
            }
            else if (pedido?.TipoPedido.IDTipoPedido == (int)ETipoPedido.Mesa)
            {
                BLL.Mesa.AlterarStatus(pedido.GUIDIdentificacao, EStatusMesa.ContaSolicitada);
            }
        }

        private void AlterarStatusLiberado(PedidoInformation pedido)
        {
            if (pedido.TipoPedido.IDTipoPedido == (int)ETipoPedido.Comanda)
            {
                BLL.Comanda.AlterarStatus(pedido.GUIDIdentificacao, EStatusComanda.Liberada);
            }
            else if (pedido.TipoPedido.IDTipoPedido == (int)ETipoPedido.Mesa)
            {
                BLL.Mesa.AlterarStatus(pedido.GUIDIdentificacao, EStatusMesa.Liberada);
            }
        }

        private void AlterarStatusEmUso(PedidoInformation pedido)
        {
            if (pedido.TipoPedido.IDTipoPedido == (int)ETipoPedido.Comanda)
            {
                BLL.Comanda.AlterarStatus(pedido.GUIDIdentificacao, EStatusComanda.EmUso);
            }
            else if (pedido.TipoPedido.IDTipoPedido == (int)ETipoPedido.Mesa)
            {
                BLL.Mesa.AlterarStatus(pedido.GUIDIdentificacao, EStatusMesa.EmAtendimento);
            }
        }

        private static void AddProdutoEntradaCM(PedidoInformation pedido)
        {
            if (!pedido.ListaProduto.Any(p => p.Produto.IDProduto == ProdutoInformation.IDProdutoEntracaCM))
            {
                var produtoCM = BLL.Produto.Carregar(ProdutoInformation.IDProdutoEntracaCM);
                if (pedido.ValorConsumacaoMinima != 0)
                {
                    produtoCM.ValorUnitario = pedido.ValorConsumacaoMinima;

                    if (pedido.ValorTotalProdutos < pedido.ValorConsumacaoMinima)
                    {
                        produtoCM.ValorUnitario -= pedido.ValorTotalProdutos;
                        pedido.ListaProduto.Add(new PedidoProdutoInformation
                        {
                            Produto = produtoCM,
                            Quantidade = 1,
                            ValorUnitario = produtoCM.ValorUnitario
                        });
                    }
                }


            }
        }

        private bool ComparaValorTotalPedidoValorItens(PedidoInformation pedido)
        {
            var total = pedido.ValorTotalProdutos -
                (pedido.ValorDesconto.HasValue ? pedido.ValorDesconto.Value : 0) +
                (pedido.ValorServico.HasValue ? pedido.ValorServico.Value : 0) +
                (pedido.ValorEntrega.HasValue ? pedido.ValorEntrega.Value : 0);
            var valorPago = 0m;

            if (pedido.ListaPagamento?.Count > 0)
                valorPago = pedido.ListaPagamento.Where(pp => pp.Status != StatusModel.Excluido).Sum(l => l.Valor.Value);


            var valorPendente = total - valorPago;
            if (valorPendente != 0)
                return false;
            //if (pedido.ValorTotalProdutosServicos > 0 && (pedido.ValorDesconto ?? 0) < pedido.ValorTotalProdutosServicos)
            //{
            //    return true;
            //}
            pedido.ValorTotal = total;
            return true;
        }

        private static PedidoProdutoInformation ObterPedidoProduto(string guidSolicitacao, PDVInformation pdv, UsuarioInformation usuario, string notas, ProdutoInformation produto, decimal quantidade, decimal preco)
        {
            return new PedidoProdutoInformation
            {
                ListaModificacao = new List<PedidoProdutoInformation>(),
                GUIDControleDuplicidade = guidSolicitacao,
                PDV = pdv,
                Usuario = usuario,
                Produto = produto,
                Quantidade = quantidade,
                Notas = notas,
                ValorUnitario = preco,
                CodigoAliquota = produto.CodigoAliquota,
                Cancelado = false
            };
        }

        private void Executando(string guid)
        {
            while (Exec.Contains(guid))
                Thread.Sleep(100);
            lock (@lock)
                Exec.Add(guid);
        }
    }
}


