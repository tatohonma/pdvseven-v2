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
    public class PDVsController : ApiController
    {
        private AtivacaoContext db = new AtivacaoContext();

        // GET: api/PDVs
        public IQueryable<Entities.PDV> GetPDVs()
        {
            return db.PDVs;
        }

        // GET: api/PDVs/5
        [ResponseType(typeof(Entities.PDV))]
        public IHttpActionResult GetPDV(int id)
        {
            Entities.PDV pDV = db.PDVs.Find(id);
            if (pDV == null)
            {
                return NotFound();
            }

            return Ok(pDV);
        }

        // PUT: api/PDVs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPDV(int id, Entities.PDV pDV)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pDV.IDPDV)
            {
                return BadRequest();
            }

            db.Entry(pDV).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PDVExists(id))
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

        // POST: api/PDVs
        [ResponseType(typeof(Entities.PDV))]
        public IHttpActionResult PostPDV(Entities.PDV pDV)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PDVs.Add(pDV);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pDV.IDPDV }, pDV);
        }

        // DELETE: api/PDVs/5
        [ResponseType(typeof(Entities.PDV))]
        public IHttpActionResult DeletePDV(int id)
        {
            Entities.PDV pDV = db.PDVs.Find(id);
            if (pDV == null)
            {
                return NotFound();
            }

            db.PDVs.Remove(pDV);
            db.SaveChanges();

            return Ok(pDV);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PDVExists(int id)
        {
            return db.PDVs.Count(e => e.IDPDV == id) > 0;
        }
    }
}