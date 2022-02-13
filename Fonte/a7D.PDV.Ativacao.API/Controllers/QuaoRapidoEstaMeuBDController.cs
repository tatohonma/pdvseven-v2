using a7D.PDV.Ativacao.API.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Ativacao.API.Controllers
{
    public class QuaoRapidoEstaMeuBDController : ApiController
    {
        private AtivacaoContext db = new AtivacaoContext();

        public IHttpActionResult Get(string cliente)
        {
            try
            {
                var inicio = DateTime.Now;
                var query = db.Clientes.AsNoTracking();
                if (!string.IsNullOrWhiteSpace(cliente))
                    query.Where(a => a.Nome.Contains(cliente) || a.RazaoSocial.Contains(cliente) || (a.CNPJCPF != null && a.CNPJCPF.ToString().Contains(cliente)));
                query.ToList();
                return Ok(new { tempo = (DateTime.Now - inicio).Milliseconds.ToString() + "ms" });
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
