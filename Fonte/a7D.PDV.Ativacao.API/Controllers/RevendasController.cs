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
    public class RevendasController : ApiController
    {
        private AtivacaoContext db = new AtivacaoContext();

        // GET: api/Revendas
        public IQueryable<Revenda> GetRevendas()
        {
            return db.Revendas;
        }

        // GET: api/Revendas/5
        [ResponseType(typeof(Revenda))]
        public IHttpActionResult GetRevenda(int id)
        {
            Revenda revenda = db.Revendas.Find(id);
            if (revenda == null)
            {
                return NotFound();
            }

            return Ok(revenda);
        }

        // PUT: api/Revendas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRevenda(int id, Revenda revenda)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != revenda.IDRevenda)
            {
                return BadRequest();
            }

            db.Entry(revenda).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RevendaExists(id))
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

        // POST: api/Revendas
        [ResponseType(typeof(Revenda))]
        public IHttpActionResult PostRevenda(Revenda revenda)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Revendas.Add(revenda);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = revenda.IDRevenda }, revenda);
        }

        // DELETE: api/Revendas/5
        [ResponseType(typeof(Revenda))]
        public IHttpActionResult DeleteRevenda(int id)
        {
            Revenda revenda = db.Revendas.Find(id);
            if (revenda == null)
            {
                return NotFound();
            }

            db.Revendas.Remove(revenda);
            db.SaveChanges();

            return Ok(revenda);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RevendaExists(int id)
        {
            return db.Revendas.Count(e => e.IDRevenda == id) > 0;
        }
    }
}