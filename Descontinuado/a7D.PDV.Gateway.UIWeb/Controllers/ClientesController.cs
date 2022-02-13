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
using a7D.PDV.Gateway.UIWeb.Helpers;

namespace a7D.PDV.Gateway.UIWeb.Controllers
{
    public class ClientesController : ApiController
    {
        private GatewayContext db = new GatewayContext();

        // GET: api/Clientes
        public IQueryable<Cliente> GetClientes()
        {
            return db.Clientes;
        }

        // GET: api/Clientes/5
        //[ResponseType(typeof(Cliente))]
        //public IHttpActionResult GetCliente(int id)
        //{
        //    Cliente cliente = db.Clientes.Find(id);
        //    if (cliente == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(cliente);
        //}

        [ResponseType(typeof(Cliente))]
        public IHttpActionResult GetCliente([FromUri]string chaveLicenca)
        {
            var idBroker = SiteAtivacoesHelper.ObterIdBroker(chaveLicenca);
            Cliente cliente = db.Clientes.FirstOrDefault(c => c.IdBroker == idBroker);
            if (cliente == null)
            {
                cliente = db.Clientes.FirstOrDefault(c => c.IdBroker == chaveLicenca);
                if (cliente == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(cliente);
                }
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

            if (id != cliente.IdCliente)
            {
                return BadRequest();
            }

            db.Entry(cliente).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

            return CreatedAtRoute("DefaultApi", new { id = cliente.IdCliente }, cliente);
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
            return db.Clientes.Count(e => e.IdCliente == id) > 0;
        }
    }
}