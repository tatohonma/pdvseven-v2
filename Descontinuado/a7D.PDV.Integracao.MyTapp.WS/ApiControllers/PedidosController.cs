using a7D.PDV.BLL;
using a7D.PDV.Model;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using static a7D.PDV.BLL.Comanda;

namespace a7D.PDV.Integracao.MyTapp.WS.ApiControllers
{
    public class PedidosController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
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

        private bool Executando(string guid)
        {
            var executando = Exec.Contains(guid);

            if (executando)
                Log.Trace($"GUID {guid} está executando");
            else
                Log.Trace($"GUID {guid} NÃO está executando");

            return executando;
        }

        private static object @lock = new object();

        [HttpPost]
        public IHttpActionResult CriarPedido([FromBody]Models.Pedido pedido)
        {
            try
            {
                Log.Trace($"Pedido recebido");
                Log.Debug($"Pedido recebido\n{JsonConvert.SerializeObject(pedido, Formatting.Indented)}");
                if (!pedido.Valido)
                {
                    Log.Error($"Pedido inválido");
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { motivo = 1, mensagem = "todos os campos do pedido são obrigatórios" }));
                }

                try
                {
                    while (Executando(pedido.Guid))
                    {
                        Thread.Sleep(500);
                    }

                    lock (@lock)
                    {
                        Exec.Add(pedido.Guid);
                    }

                    if (PedidoProduto.PedidoDuplicado(pedido.Guid))
                    {
                        Log.Trace("Pedido duplicado");
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created));
                    }

                    var comanda = Validar(pedido.Comanda.Value);
                    var guidIdentificacao = comanda.GUIDIdentificacao;
                    var produto = Produto.BuscarPorCodigoIntegracao((int)Config.Configuracao.TipoIntegracao, pedido.Produto);
                    if (produto == null)
                    {
                        Log.Error($"Produto com código de integração {pedido.Produto} não encontrado");
                        produto = new ProdutoInformation { IDProduto = 1 };
                    }

                    var pedidoProduto = new PedidoProdutoInformation();

                    pedidoProduto.ListaModificacao = new List<PedidoProdutoInformation>();
                    pedidoProduto.GUIDControleDuplicidade = pedido.Guid;

                    // TODO: TROCAR ESSE CODIGO
                    pedidoProduto.PDV = new PDVInformation { IDPDV = 2 };

                    // TODO: usuário?
                    pedidoProduto.Usuario = new UsuarioInformation { IDUsuario = 1 };
                    pedidoProduto.Produto = produto;
                    pedidoProduto.Quantidade = pedido.Quantidade;
                    pedidoProduto.Notas = pedido.Descricao;
                    pedidoProduto.ValorUnitario = pedido.ValorUnitario;
                    pedidoProduto.Cancelado = false;

                    Log.Trace("Adicionando pedido");
                    Log.Debug($"Adicionando pedido\n{JsonConvert.SerializeObject(pedidoProduto, Formatting.Indented)}");

                    Pedido.AdicionarProduto(20, guidIdentificacao, 1, 2, new List<PedidoProdutoInformation> { pedidoProduto });

                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created));
                }
                finally
                {
                    Log.Debug($"Removendo GUID {pedido.Guid}");
                    Exec.Remove(pedido.Guid);
                }
            }
            catch (ComandaException ex)
            {
                Log.Error($"Comanda {pedido.Comanda} - Erro de validação: Status {ex.Codigo}");
                Log.Error(ex, ex.Message);
                if (ex.Codigo == -1)
                    return NotFound();
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, new { motivo = ex.Codigo });
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Erro inesperado {ex.Message}");
                return InternalServerError();
            }
        }
    }
}
