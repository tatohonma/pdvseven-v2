using a7D.PDV.BLL;
using a7D.PDV.Integracao.API.Responses;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace a7D.PDV.Integracao.API.Controllers
{
    public class PedidoJsonController : ApiController
    {
        private string Versao => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
            set { }
        }

        private static void GerarResponse(ValidarResponse response, Model.PedidoInformation pedido, string guidIdentificacao)
        {

            response.guidIdentificacao = guidIdentificacao;
            response.idPedido = pedido.IDPedido.Value;
            response.referenciaLocalizacao = pedido.ReferenciaLocalizacao;

            if (pedido.Cliente != null)
                response.cliente = pedido.Cliente.NomeCompleto;

            response.valorProdutos = pedido.ValorTotalProdutos;
            response.valorServico = pedido.ValorServicoTemp;

            response.valorEntrada = 0;
            if (pedido.TipoEntrada != null)
                response.valorConsumacaoMinima = pedido.TipoEntrada.ValorConsumacaoMinima ?? 0;
            else
                response.valorConsumacaoMinima = 0;

            response.valorTotal = pedido.ValorTotalTemp;
            response.percentagemServico = pedido.TaxaServicoPadrao ?? 0;
        }

        [HttpPost]
        public IHttpActionResult ValidarMesa([FromBody]string numero)
        {
            try
            {
                var response = new ValidarResponse(Responses.TipoPedido.MESA);

                var mesa = Mesa.Validar(Convert.ToInt32(numero));
                var pedido = Pedido.CarregarUltimoPedido(mesa.GUIDIdentificacao);

                if (pedido.IDPedido.HasValue)
                {
                    GerarResponse(response, pedido, mesa.GUIDIdentificacao);
                }
                else
                {
                    response.guidIdentificacao = mesa.GUIDIdentificacao;
                }

                response.sucesso = true;
                response.versao = Versao;

                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.OK
                });
            }
            catch (Exception e)
            {
                var response = new ErrorResponse
                {
                    detalhes = e.ToString(),
                    mensagemErro = e.Message,
                    versao = Versao
                };
                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpPost]
        public IHttpActionResult ValidarComanda([FromBody]int numero)
        {
            try
            {
                var response = new ValidarResponse(Responses.TipoPedido.COMANDA);

                var comanda = Comanda.Validar(numero);
                var pedido = Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);

                GerarResponse(response, pedido, comanda.GUIDIdentificacao);

                response.sucesso = true;
                response.versao = Versao;

                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.OK
                });
            }
            catch (Exception e)
            {
                var response = new ErrorResponse
                {
                    detalhes = e.ToString(),
                    mensagemErro = e.Message,
                    versao = Versao
                };
                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }

        private static object @lock = new object();

        [HttpPost]
        public IHttpActionResult AdicionarProdutos([FromBody]AdicionarProdutosDados dados)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(dados.guidPedido))
                {
                    try
                    {

                        string guidIdentificacao = string.Empty;

                        while (Exec.Contains(dados.guidPedido)) { }
                        lock (@lock)
                        {
                            Exec.Add(dados.guidPedido);
                        }

                        if (!PedidoProduto.PedidoDuplicado(dados.guidPedido))
                        {
                            switch (dados.idTipoPedido)
                            {
                                case 10:
                                    guidIdentificacao = Mesa.CarregarPorNumero(dados.numero).GUIDIdentificacao;
                                    break;
                                case 20:
                                    guidIdentificacao = Comanda.CarregarPorNumero(dados.numero).GUIDIdentificacao;
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }

                            var listaProdutos = new List<PedidoProdutoInformation>();
                            var listaProdutoImpressao1 = new List<PedidoProdutoInformation>();
                            var listaProdutoImpressao2 = new Dictionary<int, List<PedidoProdutoInformation>>();

                            foreach (var item in dados.produtos)
                            {
                                var produto = Produto.Carregar(item.idProduto);
                                var pp = new PedidoProdutoInformation
                                {
                                    ListaModificacao = new List<PedidoProdutoInformation>(),
                                    GUIDControleDuplicidade = dados.guidPedido,
                                    PDV = new PDVInformation { IDPDV = dados.idPdv },
                                    Usuario = new UsuarioInformation { IDUsuario = dados.idUsuario },
                                    Produto = produto,
                                    Quantidade = item.qtd,
                                    Notas = item.notas,
                                    ValorUnitario = item.valor ?? produto.ValorUnitario,
                                    CodigoAliquota = produto.CodigoAliquota,
                                    Cancelado = false
                                };

                                foreach (var idModificacao in item.modificacoes)
                                {
                                    var modificacao = Produto.Carregar(idModificacao);
                                    pp.ListaModificacao.Add(new PedidoProdutoInformation
                                    {
                                        Produto = modificacao,
                                        PDV = new PDVInformation { IDPDV = dados.idPdv },
                                        Usuario = new UsuarioInformation { IDUsuario = dados.idUsuario },
                                        Quantidade = 1,
                                        ValorUnitario = modificacao.ValorUnitario,
                                        CodigoAliquota = modificacao.CodigoAliquota,
                                        Cancelado = false
                                    });
                                }

                                listaProdutos.Add(pp);

                                if (item.idAreaImpressao.HasValue)
                                {
                                    if (listaProdutoImpressao2.ContainsKey(item.idAreaImpressao.Value))
                                    {
                                        listaProdutoImpressao2[item.idAreaImpressao.Value].Add(pp);
                                    }
                                    else
                                    {
                                        listaProdutoImpressao2.Add(item.idAreaImpressao.Value, new List<PedidoProdutoInformation>() { pp });
                                    }
                                }
                                else
                                {
                                    listaProdutoImpressao1.Add(pp);
                                }
                            }

                            Pedido.AdicionarProduto(dados.idTipoPedido, guidIdentificacao, dados.idUsuario, dados.idPdv, listaProdutos, dados.referenciaLocalizacao);

                            var ordemImpressao = Convert.ToBoolean(ConfigurationManager.AppSettings["OrdemImpressao"]);
                            if (ordemImpressao)
                            {
                                OrdemImpressao.GerarOrdemProducao(listaProdutoImpressao1);
                                OrdemImpressao.GerarOrdemProducao(listaProdutoImpressao2);
                            }
                        }
                    }
                    finally
                    {
                        Exec.Remove(dados.guidPedido);
                    }

                }

                var response = new BaseResponse();

                response.sucesso = true;
                response.versao = Versao;

                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.OK
                });
            }
            catch (Exception e)
            {
                var response = new ErrorResponse
                {
                    detalhes = e.ToString(),
                    mensagemErro = e.Message,
                    versao = Versao
                };
                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpPost]
        public IHttpActionResult SolicitarFechamento([FromBody]SolicitarFechamentoDados dados)
        {
            try
            {
                string guidIdentificacao = string.Empty;
                if (dados.idTipoPedido == 10)
                    guidIdentificacao = Mesa.CarregarPorNumero(dados.numero).GUIDIdentificacao;
                else
                    throw new Exception("Solicitação de conta pode ser feita apenas para mesas!");

                var pedido = Pedido.CarregarUltimoPedido(guidIdentificacao);
                if (pedido.IDPedido == null || pedido.StatusPedido.StatusPedido != EnumStatusPedido.Aberto)
                    pedido = BLL.Pedido.NovoPedidoMesa(guidIdentificacao);

                Mesa.AlterarStatus(guidIdentificacao, 30);

                var informacoes = dados.informacoes;
                if (informacoes != null)
                {
                    pedido.NumeroPessoas = informacoes.numeroPessoas;
                    pedido.Cliente = Cliente.BuscarCliente((Cliente.TipoCliente)informacoes.tipoCliente, informacoes.cliente);
                    pedido.DocumentoCliente = informacoes.documento;
                }

                if (dados.idAreaImpressao.HasValue)
                {
                    if (OrdemImpressao.ListarNaoEnviados().Where(o => o.Conta == true && o.ConteudoImpressao == pedido.IDPedido.Value.ToString()).ToList().Count() < 1)
                    {
                        OrdemImpressao.Salvar(new OrdemImpressaoInformation
                        {
                            Conta = true,
                            ConteudoImpressao = pedido.IDPedido.Value.ToString(),
                            DtOrdem = DateTime.Now,
                            AreaImpressao = new AreaImpressaoInformation { IDAreaImpressao = dados.idAreaImpressao.Value },
                            EnviadoFilaImpressao = false
                        });
                    }
                }

                var response = new BaseResponse();

                response.sucesso = true;
                response.versao = Versao;

                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.OK
                });
            }
            catch (Exception e)
            {
                var response = new ErrorResponse
                {
                    detalhes = e.ToString(),
                    mensagemErro = e.Message,
                    versao = Versao
                };
                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpGet]
        public IHttpActionResult RetornarPedido([FromUri]int idTipoPedido, [FromUri]int numero)
        {
            try
            {
                var response = new RetornarPedidoResponse();
                var listaProdutos = new List<RetornarPedidoResponse.Produto>();

                string guidIdentificacao = string.Empty;

                switch (idTipoPedido)
                {
                    case 10://Mesa
                        guidIdentificacao = Mesa.CarregarPorNumero(numero).GUIDIdentificacao;
                        break;
                    case 20://Comanda
                        guidIdentificacao = Comanda.CarregarPorNumero(numero).GUIDIdentificacao;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                var pedido = Pedido.CarregarUltimoPedido(guidIdentificacao);

                response.idPedido = pedido.IDPedido.Value;

                pedido
                    .ListaProduto
                    .Where(l => l.Cancelado != true)
                    .ToList()
                    .ForEach(pedidoProduto =>
                    {
                        var rProduto = new RetornarPedidoResponse.Produto
                        {
                            idProduto = pedidoProduto.Produto.IDProduto.Value,
                            notas = pedidoProduto.Notas,
                            quantidade = pedidoProduto.Quantidade ?? 0,
                            valorUnitario = pedidoProduto.ValorUnitario ?? 0
                        };

                        var listaModificacoes = new List<RetornarPedidoResponse.Modificacao>();

                        pedidoProduto
                        .ListaModificacao
                        .ForEach(modificacao =>
                        {
                            listaModificacoes.Add(new RetornarPedidoResponse.Modificacao
                            {
                                idProduto = modificacao.Produto.IDProduto.Value,
                                valorUnitario = modificacao.ValorUnitario ?? 0
                            });
                        });

                        rProduto.modificacaoes = listaModificacoes.ToArray();

                        listaProdutos.Add(rProduto);
                    });

                response.produtos = listaProdutos.ToArray();
                response.sucesso = true;
                response.versao = Versao;

                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.OK
                });
            }
            catch (Exception e)
            {
                var response = new ErrorResponse
                {
                    detalhes = e.ToString(),
                    mensagemErro = e.Message,
                    versao = Versao
                };
                return ResponseMessage(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }

        public class SolicitarFechamentoDados
        {
            public int idTipoPedido { get; set; }
            public int numero { get; set; }
            public InformacoesFechamento informacoes { get; set; }
            public int? idAreaImpressao { get; set; }


            public class InformacoesFechamento
            {
                public int numeroPessoas { get; set; }
                public string documento { get; set; }
                public string cliente { get; set; }
                public int tipoCliente { get; set; }
            }
        }

        public class AdicionarProdutosDados
        {
            public Produto[] produtos { get; set; }
            public int idTipoPedido { get; set; }
            public int numero { get; set; }
            public int idUsuario { get; set; }
            public int idPdv { get; set; }
            public string guidPedido { get; set; }
            public string referenciaLocalizacao { get; set; }

            public class Produto
            {
                public int idProduto { get; set; }
                public decimal qtd { get; set; }
                public string notas { get; set; }
                public decimal? valor { get; set; }
                public int[] modificacoes { get; set; }
                public int? idAreaImpressao { get; set; }
            }
        }
    }
}