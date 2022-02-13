using a7D.PDV.Gateway.UIWeb.Context;
using a7D.PDV.Gateway.UIWeb.Helpers;
using a7D.PDV.Gateway.UIWeb.Models;
using a7D.PDV.Gateway.UIWeb.Repository;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using a7D.PDV.Gateway.UIWeb.Filters;
using PagarMe;

namespace a7D.PDV.Gateway.UIWeb.Controllers
{
    public class PedidosController : ApiController
    {
        private GatewayContext db;

        public PedidosController()
        {
            db = new GatewayContext();
        }

        public class Item
        {
            public int IdContaReceber { get; set; }
        }

        [HttpPost]
        public IHttpActionResult CriarPedido([FromBody]Item[] itens)
        {
            try
            {
                var itensPedido = new List<ItemPedido>();
                foreach (var item in itens)
                {
                    itensPedido.Add(new ItemPedido
                    {
                        IdContaReceber = item.IdContaReceber,
                        ContaReceber = db.ContasReceber.Find(item.IdContaReceber)
                    });
                }
                var repo = new PedidoRepository(db);
                var pedido = repo.BuscarPedido(itensPedido);
                if (pedido == null)
                {
                    pedido = new Pedido
                    {
                        Status = "pending",
                        Itens = new List<ItemPedido>(),
                        ValorPago = 0,
                        DataPedido = DateTime.Now
                    };
                    pedido.Valor = itensPedido.Sum(i => i.ContaReceber.Saldo);
                    pedido = repo.Adicionar(pedido);
                    itensPedido.ForEach(i => i.IdPedido = pedido.IdPedido);
                    pedido.Itens.AddRange(itensPedido);
                    repo.SalvarAlteracoes();
                }
                else
                {
                    pedido.DataPedido = DateTime.Now;
                    repo.SalvarAlteracoes();
                }
                return Json(new { IdPedido = pedido.IdPedido });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult GetPedido(int id)
        {
            int tentativas = 0;
            db.Configuration.ProxyCreationEnabled = false;
            var pedido = db.Pedidos.Find(id);
            if (pedido == null)
                return NotFound();

            if (pedido.Status == "pending")
            {
                do
                {
                    Task.Delay(2000).Wait();
                    pedido = db.Pedidos.Find(id);

                    if (pedido == null)
                        return NotFound();

                    tentativas++;
                } while (tentativas < 3 || pedido.Status != "pending");
            }
            return Ok(pedido);
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<Pedido>))]
        public IHttpActionResult GetPedidos([FromUri]string chaveLicenca)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                var idBroker = string.Empty;
                if (string.IsNullOrWhiteSpace(chaveLicenca) == false)
                {
                    idBroker = SiteAtivacoesHelper.ObterIdBroker(chaveLicenca);
                    if (string.IsNullOrWhiteSpace(idBroker))
                    {
                        var cliente = db.Clientes.FirstOrDefault(c => c.IdBroker == chaveLicenca);
                        if (cliente == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            idBroker = chaveLicenca;
                        }
                    }
                }

                var repoPedido = new PedidoRepository(db);
                var repoItemPedido = new ItemPedidoRepository(db);
                var pedidos = repoPedido.BuscarPorIdBroker(idBroker);
                pedidos = pedidos
                    .Where(p => p.Status == "processing" || p.Status == "authorized" || p.Status == "waitingpayment")
                    .ToList();
                pedidos.ForEach(pedido => pedido.Itens = repoItemPedido.ListarPorPedido(pedido.IdPedido));
                foreach (var pedido in pedidos)
                {
                    pedido.Itens.ForEach(i => i.Pedido = null);
                }
                return Ok(pedidos.ToArray());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [JwtAuthorize]
        [ResponseType(typeof(IEnumerable<Pedido>))]
        public IHttpActionResult GetPedidos([FromUri] double s, [FromUri] double e)
        {
            DateTime start = FromUnixTimestamp(s).Date;
            DateTime end = FromUnixTimestamp(e).Date;
            end = end.Date + new TimeSpan(23, 59, 59);

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var repoItemPedido = new ItemPedidoRepository(db);
                var pedidos = db.Pedidos.Where(p => p.Status != "pending" && (p.DataPagamento >= start && p.DataPagamento <= end)).ToList();
                //pedidos = pedidos
                //    .Where(p => p.Status != "pending")
                //    .ToList();
                pedidos.ForEach(pedido => pedido.Itens = repoItemPedido.ListarPorPedido(pedido.IdPedido));
                foreach (var pedido in pedidos)
                {
                    pedido.Itens.ForEach(i => i.Pedido = null);
                }
                return Ok(pedidos.ToArray());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var pedido = db.Pedidos.Find(id);
            if (pedido == null)
                return NotFound();
            if (pedido.Status == "authorized" || pedido.Status == "paid")
                return BadRequest();
            try
            {

                //if (pedido.IdTransacao > 0)
                //{
                //    PagarMeService.DefaultApiKey = PagarMe.Api.API_KEY;

                //    var t = PagarMeService.GetDefaultService().Transactions.Find(pedido.IdTransacao);
                //}

                pedido.Status = "pending";
                pedido.DataPedido = DateTime.Now;
                pedido.ValorPago = 0;

                db.SaveChanges();
                db.Configuration.ProxyCreationEnabled = false;
                var rPedido = db.UnProxy(pedido);
                rPedido.Itens = db.UnProxy(db.ItensPedido.Where(i => i.IdPedido == rPedido.IdPedido).ToList()).ToList();
                foreach (var item in rPedido.Itens)
                {
                    item.ContaReceber = db.UnProxy(db.ContasReceber.Find(item.IdContaReceber));
                }
                return Ok(rPedido);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private DateTime FromUnixTimestamp(double unixTimestamp)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(unixTimestamp).ToLocalTime();
            return dt;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
