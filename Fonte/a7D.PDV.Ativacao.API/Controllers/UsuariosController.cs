using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Entities;
using a7D.PDV.Ativacao.API.Filters;
using a7D.PDV.Ativacao.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace a7D.PDV.Ativacao.API.Controllers
{
    public class UsuariosController : ApiController
    {

        private UsuariosRepository usuarios;
        public UsuariosController()
        {
            usuarios = new UsuariosRepository(new AtivacaoContext());
        }

        [Route("api/usuarios/renovar")]
        [HttpPost]
        public async Task<IHttpActionResult> RenovarSenha([FromBody]string email)
        {
            try
            {
                var hash = await usuarios.SolicitarNovaSenha(email);
                if (hash != null)
                {
                    usuarios.EnviarEmailNovaSenha(email);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [Route("api/usuarios/reenviar")]
        [HttpPost]
        public async Task<IHttpActionResult> ReenviarEmail([FromBody] string email)
        {
            try
            {
                await usuarios.EnviarEmailCadastro(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [Route("api/usuarios/hash/{hash}")]
        [HttpGet]
        public async Task<IHttpActionResult> BuscarPorHash([FromUri] string hash)
        {
            try
            {
                var usuario = await usuarios.BuscarPorHash(hash);
                if (usuario == null)
                    return NotFound();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [Route("api/usuarios/hash/{hash}")]
        [HttpPost]
        public async Task<IHttpActionResult> AlterarSenha([FromBody]UsuarioDTO usuario, [FromUri]string hash)
        {
            try
            {
                await usuarios.AlterarSenha(hash, usuario.Nome, usuario.Senha);
                return Ok();
            }
            catch
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest));
            }
        }

        [ApiAuth(requerAdm: true)]
        [HttpPost]
        public async Task<IHttpActionResult> AdicionarUsuario([FromBody]UsuarioDTO usuario)
        {
            try
            {
                var novoUsuario = await usuarios.AdicionarUsuario(usuario.Email, usuario.Nome);
                await usuarios.EnviarEmailCadastro(novoUsuario.Email);
                return Created($"/api/usuarios/{novoUsuario.IDUsuario}", novoUsuario);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [ApiAuth(requerAdm: true)]
        [HttpGet]
        public async Task<IHttpActionResult> ObterUsuario(int id)
        {
            try
            {
                var usuario = await usuarios.BuscarPorIdLimpo(id);
                if (usuario == null)
                    return NotFound();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [ApiAuth(requerAdm: true)]
        [HttpDelete]
        public async Task<IHttpActionResult> RemoverUsuario(int id)
        {
            try
            {
                await usuarios.ExcluirUsuario(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [ApiAuth(requerAdm: true)]
        [HttpPut]
        public async Task<IHttpActionResult> AlterarUsuario([FromUri]int id, [FromBody]UsuarioDTO usuario)
        {
            if (id != usuario.IDUsuario.Value)
                return BadRequest();
            try
            {
                await usuarios.AlterarCadastro(usuario.IDUsuario.Value, usuario.Nome, usuario.Ativo, usuario.Adm);
                return Ok();
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [ApiAuth(requerAdm: true)]
        [ResponseType(typeof(IEnumerable<Usuario>))]
        [CustomHeaderFilter]
        [HttpGet]
        public IHttpActionResult BuscarUsuario([FromUri]int page = 0, [FromUri]int count = 0, [FromUri]Filter filter = null)
        {
            try
            {
                var result = usuarios.BuscarUsuarios(page, count, filter.Nome, filter.Email, filter.Admin, filter.CadastroPendente, filter.Ativo, filter.Excluido).ToList();
                Request.Properties["count"] = result.Count.ToString();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        public class UsuarioDTO
        {
            public int? IDUsuario { get; set; }
            public string Nome { get; set; }
            public string Email { get; set; }
            public bool? Ativo { get; set; }
            public string NovaSenha { get; set; }
            public string Senha { get; set; }
            public bool? Adm { get; set; }
        }

        public class Filter
        {
            public string Nome { get; set; }
            public string Email { get; set; }
            public string Ativo { get; set; }
            public string CadastroPendente { get; set; }
            public string Admin { get; set; }
            public string Excluido { get; set; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                usuarios.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
