using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class PagamentosController : ApiController
    {
        private readonly string DicName = "ExecPDictionary";
        private readonly string formatoNumero = "R$ #,##0.00";
        private HashSet<string> Exec
        {
            get
            {
                if (HttpContext.Current.Application.Get(DicName) == null)
                {
                    HttpContext.Current.Application.Set(DicName, new HashSet<string>());
                }
                return (HashSet<string>)HttpContext.Current.Application.Get(DicName);
            }
        }
        private static object @lock = new object();

        [HttpGet]
        public IHttpActionResult ObterPagamentos()
        {
            try
            {
                return Ok(BLL.TipoPagamento.ListarAtivos().Select(p => DTO.Converter(p)));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(70, ex)));
            }
        }

        // http://localhost:7777/api/pagamentos/pagar?idTipoPedido=10&numero=1&idUsuario=1&idPDV=1&valor=123&idGateway=10&referenciaPagamento=xxx&autorizacao=yyy&contaRecebivel=Stone&bandeira=visa&idMetodo=1
        [HttpGet]
        [Route("api/pagamentos/pagar")]
        public IHttpActionResult InserirPagamentos()
        {
            var pagamento = new InsercaoPagamento();
            pagamento.GUIDSolicitacao = Guid.NewGuid().ToString();
            pagamento.IDTipoPedido = int.Parse(HttpContext.Current.Request["idTipoPedido"]);
            pagamento.Numero = int.Parse(HttpContext.Current.Request["numero"]);
            pagamento.IDUsuario = int.Parse(HttpContext.Current.Request["idUsuario"]);
            pagamento.IDPDV = int.Parse(HttpContext.Current.Request["idPDV"]);
            pagamento.Pagamentos = new List<Pagamento>() { new Pagamento() };
            pagamento.Pagamentos[0].TipoPagamento = new API2.Model.TipoPagamento();
            pagamento.Pagamentos[0].TipoPagamento.IDGateway = int.Parse(HttpContext.Current.Request["idGateway"]);
            pagamento.Pagamentos[0].Valor = int.Parse(HttpContext.Current.Request["valor"]) / 100m;
            pagamento.Pagamentos[0].ReferenciaPagamento = HttpContext.Current.Request["ReferenciaPagamento"];
            pagamento.Pagamentos[0].Autorizacao = HttpContext.Current.Request["Autorizacao"];
            pagamento.Pagamentos[0].ContaRecebivel = HttpContext.Current.Request["ContaRecebivel"];
            pagamento.Pagamentos[0].Bandeira = HttpContext.Current.Request["Bandeira"];
            string metodo = HttpContext.Current.Request["idMetodo"] ?? HttpContext.Current.Request["metodo"];
            if (Int32.TryParse(metodo, out int idMetodo))
                pagamento.Pagamentos[0].IDMetodo = idMetodo;
            else
            {
                metodo = metodo.ToLower();
                if (metodo.Contains("crédito") || metodo.Contains("credito"))
                    pagamento.Pagamentos[0].IDMetodo = (int)EMetodoPagamento.Credito;
                else if (metodo.Contains("débito") || metodo.Contains("debito"))
                    pagamento.Pagamentos[0].IDMetodo = (int)EMetodoPagamento.Debito;
                else
                    pagamento.Pagamentos[0].IDMetodo = (int)EMetodoPagamento.Outros;
            }

            return InserirPagamentos(pagamento);
        }

        [HttpPost]
        public IHttpActionResult InserirPagamentos([FromBody] InsercaoPagamento inserirPagamento)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inserirPagamento.GUIDSolicitacao))
                    throw new ArgumentNullException("adicionarProdutos.GUIDSolicitacao");

                if (inserirPagamento.Pagamentos == null)
                    throw new ArgumentNullException("adicionarProdutos.Itens");

                if (inserirPagamento.Pagamentos.Any(p => p.TipoPagamento == null))
                    throw new ArgumentNullException("adicionarProdutos.Pagamentos.TipoPagamento");

                if (inserirPagamento.IDPDV < 1)
                    throw new ArgumentOutOfRangeException("adicionarProdutos.IDPDV", "IDPDV precisa ser maior que 0");

                if (inserirPagamento.IDUsuario < 1)
                    throw new ArgumentOutOfRangeException("adicionarProdutos.IDUsuario", "IDUsuario precisa ser maior que 0");

                if (inserirPagamento.Pagamentos.Count == 0)
                    throw new ArgumentOutOfRangeException("inserirPagamento.Pagamentos", "Não há pagamentos a inserir");

            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(60, ex)));
            }

            try
            {
                var pdv = BLL.PDV.Carregar(inserirPagamento.IDPDV.Value);
                var usuario = BLL.Usuario.Carregar(inserirPagamento.IDUsuario.Value);

                Task.WaitAll(Executando(inserirPagamento.GUIDSolicitacao));

                if (BLL.PedidoProduto.PedidoDuplicado(inserirPagamento.GUIDSolicitacao))
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(60, "Este GUID já existe.")));

                var guidIdentificacao = string.Empty;

                PedidoInformation pedido;
                var tipoPedido = (ETipoPedido)inserirPagamento.IDTipoPedido;
                switch (tipoPedido)
                {
                    case ETipoPedido.Mesa:
                        BLL.Mesa.Validar(inserirPagamento.Numero, true);
                        var mesa = BLL.Mesa.CarregarPorNumero(inserirPagamento.Numero);
                        guidIdentificacao = mesa.GUIDIdentificacao;
                        pedido = BLL.Pedido.CarregarUltimoPedido(guidIdentificacao);
                        break;
                    case ETipoPedido.Comanda:
                        BLL.Comanda.Validar(inserirPagamento.Numero, true);
                        ComandaInformation comanda = BLL.Comanda.CarregarPorNumeroOuCodigo(inserirPagamento.Numero);
                        guidIdentificacao = comanda.GUIDIdentificacao;
                        pedido = BLL.Pedido.CarregarUltimoPedido(guidIdentificacao);
                        break;
                    case ETipoPedido.Delivery: // Delivery
                        pedido = BLL.Pedido.Carregar(inserirPagamento.Numero);
                        break;
                    case ETipoPedido.Balcao: // Venda Balcão
                        pedido = BLL.Pedido.Carregar(inserirPagamento.Numero);
                        break;
                    default:
                        throw new NotImplementedException($"IDTipoPedido {inserirPagamento.IDTipoPedido} não foi implementado");
                }

                if (pedido?.IDPedido == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, new ErroResult("Pedido não existe")));

                var pagamentos = BLL.PedidoPagamento.ListarNaoCanceladoPorPedido(pedido.IDPedido.Value);
                var valorPago = pagamentos.Sum(p => p.Valor.Value);

                var valorASerInserido = Math.Truncate(inserirPagamento.Pagamentos.Sum(p => p.Valor.Value) * 100m) / 100m;

                if (tipoPedido != ETipoPedido.Balcao &&
                  !(tipoPedido == ETipoPedido.Comanda && ConfiguracoesSistema.Valores.ComandaComCheckin)) //Para permitir compra de credito em comandas
                {
                    if ((valorPago + valorASerInserido) > pedido.ValorTotalTemp)
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new Erro(70, $"Adicionar pagamentos com valor {valorASerInserido.ToString(formatoNumero)} ultrapassaria o valor total do pedido ({pedido.ValorTotalTemp.ToString(formatoNumero)})")));
                }

                var tiposPagamento = BLL.TipoPagamento.ListarAtivos();
                int ultimoID = 0;
                string log = "";
                foreach (var pagamento in inserirPagamento.Pagamentos)
                {
                    var idContaRevebivel = BLL.ContaRecebivel.CarregarPorNome(pagamento.ContaRecebivel)?.IDContaRecebivel ?? (int)EContaRecebivel.Dinheiro;
                    var idBandeira = BLL.Bandeira.CarregarPorNome(pagamento.Bandeira)?.IDBandeira ?? (int)EBandeira.Desconhecida;

                    TipoPagamentoInformation tipoPagamento;
                    if (pagamento.TipoPagamento.IDGateway > 0)
                    {
                        tipoPagamento = BLL.TipoPagamento.CarregarPorGateway(pagamento.TipoPagamento.IDGateway.Value, false);
                        if (tipoPagamento == null)
                            throw new Exception("Não há um meio de pagamento configurado para o Gateway: " + pagamento.TipoPagamento.IDGateway);
                    }
                    else
                    {
                        tipoPagamento = BLL.TipoPagamento.Carregar(pagamento.TipoPagamento.IDTipoPagamento.Value);
                        idContaRevebivel = tipoPagamento.ContaRecebivel?.IDContaRecebivel ?? (int)EContaRecebivel.Dinheiro;
                        idBandeira = tipoPagamento.Bandeira?.IDBandeira ?? (int)EBandeira.Desconhecida;
                        pagamento.IDMetodo = tipoPagamento.MeioPagamentoSAT?.IDMeioPagamentoSAT;
                    }

                    var novoPagamento = new PedidoPagamentoInformation
                    {
                        Pedido = pedido,
                        Valor = pagamento.Valor,
                        TipoPagamento = tipoPagamento,
                        Autorizacao = pagamento.Autorizacao + (idBandeira > 0 ? "" : ("-" + pagamento.Bandeira)),
                        IDGateway = pagamento.TipoPagamento?.IDGateway,
                        MeioPagamentoSAT = new MeioPagamentoSATInformation() { IDMeioPagamentoSAT = pagamento.IDMetodo },
                        Bandeira = new tbBandeira() { IDBandeira = idBandeira },
                        ContaRecebivel = new tbContaRecebivel() { IDContaRecebivel = idContaRevebivel },
                        Excluido = false
                    };

                    PedidoPagamento.Salvar(novoPagamento, usuario.IDUsuario.Value);

                    log += $" +{tipoPagamento.Nome}: {pagamento.ContaRecebivel} {pagamento.Bandeira} {(EMetodoPagamento)pagamento.IDMetodo} {pagamento.Valor.Value.ToString("C")}";

                    ultimoID = novoPagamento.IDPedidoPagamento.Value;
                }

                GA.PostEvento(pdv, "Pagamento: " + tipoPedido.ToString(), log, usuario);

                return Ok(new ResultadoOuErro("OK", ultimoID));

            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(70, ex)));
            }
            finally
            {
                Exec.Remove(inserirPagamento.GUIDSolicitacao);
            }
        }


        private async Task Executando(string guid)
        {
            while (Exec.Contains(guid))
            {
                await Task.Delay(100);
            }
            lock (@lock) Exec.Add(guid);
        }
    }
}
