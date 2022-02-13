using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using a7D.PDV.Gateway.UIWeb.Context;
using a7D.PDV.Gateway.UIWeb.Models;
using a7D.PDV.Gateway.UIWeb.Repository;
using System.Configuration;
using Newtonsoft.Json.Linq;
using a7D.PDV.Gateway.UIWeb.Helpers;

namespace a7D.PDV.Gateway.UIWeb.Controllers
{
    public class ContasReceberController : ApiController
    {
        private GatewayContext db = new GatewayContext();


        [ResponseType(typeof(IList<ContaReceber>))]
        public IHttpActionResult GetContasReceber([FromUri] string chaveLicenca = null, [FromUri] string idBroker = null)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(chaveLicenca))
                {
                    var id = SiteAtivacoesHelper.ObterIdBroker(chaveLicenca);
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        id = chaveLicenca;
                    }
                    var repo = new ClienteRepository(db);

                    var cliente = repo.BuscarPorIdBroker(id);
                    if (cliente == null)
                        return NotFound();

                    var contas = db.ContasReceber
                        .Include(cr => cr.Cliente)
                        .Where(cr => cr.Cliente.IdCliente == cliente.IdCliente)
                        .Where(cr => cr.Pendente == true);
                    var contasPagas = db.ItensPedido.Where(ip => ip.Pedido.Status == "authorized" || ip.Pedido.Status == "paid" || ip.Pedido.Status == "waitingpayment")
                        .Where(ip => ip.ContaReceber.Pendente == true)
                        .Select(ip => ip.IdContaReceber).Distinct().ToList();
                    contas = contas.Where(c => contasPagas.Contains(c.IdContaReceber) == false);
                    var rContas = contas.ToList();
                    rContas.ForEach(c =>
                    {
                        c.Data = c.Data.ToUniversalTime();
                        c.Vencimento = c.Vencimento.ToUniversalTime();
                    });
                    rContas = rContas
                                .Where(cr => !string.IsNullOrWhiteSpace(cr.Categoria))
                                .Where(cr => !cr.Categoria.ToLowerInvariant().Contains("mensalidade") || (cr.Categoria.ToLowerInvariant().Contains("mensalidade") && (cr.Vencimento - DateTime.UtcNow).Days <= 30))
                                .ToList();
                    return Ok(rContas);
                }
                else if (!string.IsNullOrWhiteSpace(idBroker))
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var contas = db.ContasReceber.Include(cr => cr.Cliente).Where(cr => cr.Cliente.IdBroker == idBroker && cr.Pendente == true);
                    var rContas = contas.ToList();
                    rContas.ForEach(c =>
                    {
                        c.Data = c.Data.ToUniversalTime();
                    });
                    return Ok(rContas);
                }
                else
                {
                    var rContas = db.ContasReceber.Include(cr => cr.Cliente).Where(cr => cr.Pendente == true).ToList();
                    rContas.ForEach(c =>
                    {
                        c.Data = c.Data.ToUniversalTime();
                    });
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/ContasReceber/5
        [ResponseType(typeof(ContaReceber))]
        public IHttpActionResult GetContaReceber(int id)
        {
            ContaReceber contaReceber = db.ContasReceber.Find(id);
            if (contaReceber == null)
            {
                return NotFound();
            }

            return Ok(contaReceber);
        }

        // PUT: api/ContasReceber/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContaReceber(int id, ContaReceber contaReceber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contaReceber.IdContaReceber)
            {
                return BadRequest();
            }

            db.Entry(contaReceber).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContaReceberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ContasReceber
        [ResponseType(typeof(ContaReceber))]
        public IHttpActionResult PostContaReceber(ContaReceber contaReceber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ContasReceber.Add(contaReceber);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = contaReceber.IdContaReceber }, contaReceber);
        }

        // DELETE: api/ContasReceber/5
        [ResponseType(typeof(ContaReceber))]
        public IHttpActionResult DeleteContaReceber(int id)
        {
            ContaReceber contaReceber = db.ContasReceber.Find(id);
            if (contaReceber == null)
            {
                return NotFound();
            }

            db.ContasReceber.Remove(contaReceber);
            db.SaveChanges();

            return Ok(contaReceber);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContaReceberExists(int id)
        {
            return db.ContasReceber.Count(e => e.IdContaReceber == id) > 0;
        }
    }
}