using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Filters;
using a7D.PDV.Ativacao.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace a7D.PDV.Ativacao.API.Controllers
{
    public class EmailsController : ApiController
    {

        private UsuariosRepository usuarios;
        public EmailsController()
        {
            usuarios = new UsuariosRepository(new AtivacaoContext());
        }

        [HttpGet]
        [ApiAuth(requerAdm: true)]
        public IHttpActionResult EmailValido([FromUri] string email, [FromUri] int? id = null)
        {
            try
            {
                if (usuarios.EmailExiste(email, id))
                    return StatusCode(HttpStatusCode.Conflict);
                return Ok();
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                usuarios?.Dispose();
            base.Dispose(disposing);
        }
    }
}
