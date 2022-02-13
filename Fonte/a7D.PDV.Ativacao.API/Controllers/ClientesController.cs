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
using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Entities;
using a7D.PDV.Ativacao.API.Filters;

namespace a7D.PDV.Ativacao.API.Controllers
{
    [ApiAuth]
    public class ClientesController : ApiController
    {
        private AtivacaoContext db = new AtivacaoContext();

        public class Filter
        {
            public int? Revenda { get; set; }
            public string Nome { get; set; }
            public string RazaoSocial { get; set; }
            public string Documento { get; set; }
            public string Telefone { get; set; }
        }

        // GET: api/Clientes
        [CustomHeaderFilter]
        public IHttpActionResult GetClientes([FromUri]int page = 0, [FromUri]int count = 0, [FromUri]Filter filter = null)
        {
            try
            {
                IQueryable<Cliente> query;
                query = db.Clientes
                    .AsNoTracking()
                    .OrderBy(a => a.Nome);

                if (filter != null)
                {
                    if (filter.Revenda.HasValue)
                        query = query.Where(c => c.IDRevenda == filter.Revenda);
                    if (!string.IsNullOrWhiteSpace(filter.Nome))
                        query = query.Where(c => c.Nome.Contains(filter.Nome));
                    if (!string.IsNullOrWhiteSpace(filter.RazaoSocial))
                        query = query.Where(c => c.RazaoSocial.Contains(filter.RazaoSocial));
                    if (!string.IsNullOrWhiteSpace(filter.Documento))
                        query = query.Where(c => c.CNPJCPF.HasValue && c.CNPJCPF.ToString().Contains(filter.Documento));
                    if (!string.IsNullOrWhiteSpace(filter.Telefone))
                        query = query.Where(c => c.Telefone.Contains(filter.Telefone));
                }

                var total = query.ToList().Count();
                Request.Properties["count"] = total.ToString();

                if (page > 0)
                    query = query.Skip((page - 1) * count);
                if (count > 0)
                    query = query.Take(count);

                var result = query.ToList();
                //return Json(query.ToList());
                var response = Request.CreateResponse(HttpStatusCode.OK, result);
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));
            }
        }

        // GET: api/Clientes/5
        [ResponseType(typeof(Cliente))]
        public IHttpActionResult GetCliente(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        // PUT: api/Clientes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCliente(int id, Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cliente.IDCliente)
            {
                return BadRequest();
            }

            db.Entry(cliente).State = EntityState.Modified;

            cliente.Revenda = db.Revendas.Find(cliente.IDRevenda);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex));
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Clientes
        [ResponseType(typeof(Cliente))]
        public IHttpActionResult PostCliente(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clientes.Add(cliente);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cliente.IDCliente }, cliente);
        }

        // DELETE: api/Clientes/5
        [ResponseType(typeof(Cliente))]
        public IHttpActionResult DeleteCliente(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }

            db.Clientes.Remove(cliente);
            db.SaveChanges();

            return Ok(cliente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClienteExists(int id)
        {
            return db.Clientes.Any(e => e.IDCliente == id);
        }
    }
}