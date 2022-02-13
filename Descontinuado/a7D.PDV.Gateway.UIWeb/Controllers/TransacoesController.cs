using a7D.PDV.Gateway.UIWeb.Context;
using a7D.PDV.Gateway.UIWeb.Models;
using a7D.PDV.Gateway.UIWeb.PagarMe;
using a7D.PDV.Gateway.UIWeb.Repository;
using Newtonsoft.Json.Linq;
using PagarMe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace a7D.PDV.Gateway.UIWeb.Controllers
{
    public class TransacoesController : ApiController
    {
        private GatewayContext db;

        public TransacoesController()
        {
            db = new GatewayContext();
        }

        [HttpPost]
        [Route("api/transacoes/{idPedido}")]
        public IHttpActionResult PostTransacoes([FromUri] int idPedido)
        {
            var capturado = false;
            InteracaoPagarMe interacao = null;
            try
            {
                PagarMeService.DefaultApiKey = Api.API_KEY;

                //var keyBytes = Encoding.ASCII.GetBytes(Api.API_KEY);
                //HMACSHA1 hash = new HMACSHA1(keyBytes);
                IEnumerable<string> headerSignature;
                var signature = string.Empty;
                if (Request.Headers.TryGetValues("X-Hub-Signature", out headerSignature))
                {
                    signature = headerSignature.FirstOrDefault();
                }
                var data = HttpContext.Current.Request.Form;
                var bodyContent = Request.Content.ReadAsStringAsync().Result;

                if (!Utils.ValidateRequestSignature(bodyContent, signature))
                    return BadRequest();

                var t = PagarMeService.GetDefaultService().Transactions.Find(data["id"]);

                interacao = db.InteracoesPagarMe.Add(new InteracaoPagarMe
                {
                    Conteudo = bodyContent
                });

                db.SaveChanges();

                if (t == null)
                    return BadRequest();
                if ((t.PaymentMethod == PaymentMethod.CreditCard && t.Status == TransactionStatus.Authorized)
                    || (t.PaymentMethod == PaymentMethod.Boleto && t.Status == TransactionStatus.Authorized))
                {
                    t.Capture(t.Amount);
                }
                capturado = true;

                //var data = HttpUtility.ParseQueryString(bodyContent);

                //var content = Encoding.ASCII.GetBytes(bodyContent);
                //MemoryStream ms = new MemoryStream(content);
                //var computedSignature = $"sha1={hash.ComputeHash(ms).Aggregate(string.Empty, (s, e) => s + string.Format("{0:x2}", e), s => s)}";

                //if (computedSignature != signature)
                //    return BadRequest();

                var repo = new PedidoRepository(db);
                var pedido = repo.ObterPorId(idPedido);
                if (pedido != null)
                {
                    if (t.DateCreated.HasValue)
                        pedido.DataPedido = t.DateCreated.Value;

                    var currentStatus = t.Status.ToString().ToLower();
                    var method = t.PaymentMethod.ToString().ToLower();

                    if (t.Status == TransactionStatus.Authorized)
                        pedido.DataAutorizacao = DateTime.Now;

                    if (t.Status == TransactionStatus.Paid)
                        pedido.DataPagamento = DateTime.Now;

                    pedido.Status = currentStatus;
                    pedido.IdTransacao = Convert.ToInt32(data["id"]);
                    pedido.Valor = t.Amount / 100m;
                    pedido.ValorPago = t.PaidAmount / 100m;

                    var eCartao = method != "boleto";

                    if (eCartao)
                    {
                        var bandeira = t.CardBrand.ToString().ToLower();
                        pedido.Parcelas = t.Installments ?? 1;
                        pedido.Bandeira = bandeira;
                        pedido.UltimosDigitosCartao = t.CardLastDigits;
                    }

                    var boletoUrl = t.BoletoUrl;
                    var eBoleto = method == "boleto";

                    if (eBoleto)
                    {
                        pedido.UrlBoleto = boletoUrl;
                        pedido.CodigoBarrasBoleto = t.BoletoBarcode;
                        pedido.DataVencimentoBoleto = t.BoletoExpirationDate;
                    }

                    interacao.IdPedido = pedido.IdPedido;
                    interacao.Capturado = capturado;
                    repo.SalvarAlteracoes();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                if (interacao != null)
                {
                    interacao.Erro = ex.StackTrace;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch { }
                }
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/transacoes/{idPedido}/novoboleto")]
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult NovoBoleto([FromUri] int idPedido)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var repoPedido = new PedidoRepository(db);
                var pedido = repoPedido.ObterPorId(idPedido);

                if (pedido == null)
                    return NotFound();

                if (!pedido.DataVencimentoBoleto.HasValue)
                    return BadRequest();

                if ((pedido.DataVencimentoBoleto.Value - DateTime.Now).Days > 0)
                    return BadRequest();

                lock (pedido)
                {
                    PagarMeService.DefaultApiKey = Api.API_KEY;

                    var transaction = new Transaction();

                    transaction.Amount = Convert.ToInt32(pedido.Valor * 100);
                    transaction.PostbackUrl = $"http://{Request.RequestUri.Authority}/api/transacoes/{idPedido}";
                    transaction.PaymentMethod = PaymentMethod.Boleto;
                    transaction.BoletoExpirationDate = DateTime.Now.AddDays(1);

                    transaction.Save();

                    var id = transaction.Id;

                    pedido.DataVencimentoBoleto = transaction.BoletoExpirationDate;
                    pedido.IdTransacao = Convert.ToInt32(id);
                    pedido.CodigoBarrasBoleto = transaction.BoletoBarcode;
                    pedido.UrlBoleto = transaction.BoletoUrl;

                    db.SaveChanges();
                }

                db.Configuration.ProxyCreationEnabled = false;

                var repoItemPedido = new ItemPedidoRepository(db);
                pedido = repoPedido.ObterPorId(pedido.IdPedido);
                pedido.Itens = repoItemPedido.ListarPorPedido(pedido.IdPedido);
                pedido.Itens.ForEach(i => i.Pedido = null);

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        private bool PagamentoValido(int valorSemJuros, int valorComJuros, int parcelas)
        {
            const int interestRate = 19;
            using (var client = new HttpClient())
            {
                try
                {
                    var uriBuilder = new UriBuilder("https://api.pagar.me/1/transactions/calculate_installments_amount");
                    uriBuilder.Query = $"?api_key={Api.API_KEY}&max_installments={parcelas}&free_installments=1&amount={valorSemJuros}&interest_rate={interestRate}";
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = uriBuilder.Uri,
                        Method = HttpMethod.Get
                    };

                    client.BaseAddress = uriBuilder.Uri;
                    var response = client.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        dynamic body = JObject.Parse(content);

                        JObject installments = body.installments.Value;

                        var installment = installments.GetValue(parcelas.ToString());
                        var amount = installment.Value<int>("amount");

                        return amount == valorComJuros;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
