using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Entities;
using a7D.PDV.Ativacao.API.Exceptions;
using a7D.PDV.Ativacao.API.Filters;
using a7D.PDV.Ativacao.API.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace a7D.PDV.Ativacao.API.Controllers
{
    public class AutenticacaoController : ApiController
    {
        private UsuariosRepository usuarios;

        public AutenticacaoController()
        {
            usuarios = new UsuariosRepository(new AtivacaoContext());
        }

        public IHttpActionResult PostAutenticacao([FromBody] Usuario usuarioReq)
        {
            if (usuarioReq == null)
                return StatusCode(HttpStatusCode.BadRequest);

            Usuario usuario = default(Usuario);
            try
            {
                usuario = usuarios.Autenticar(usuarioReq.Email, usuarioReq.Senha);
            }
            catch (CadastroPendenteException)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            if (usuario != null)
            {
                return Ok(new { usuario = usuario, jwt = ObterJwt(usuario) });
            }

            return StatusCode(HttpStatusCode.Forbidden);
        }

        internal static string ObterJwt(Usuario usuario)
        {
            var securityKey = GetBytes("ThisIsAnImportantStringAndIHaveNoIdeaIfThisIsVerySecureOrNot!");
            var credentials = new SigningCredentials(
                    new InMemorySymmetricSecurityKey(securityKey),
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256");
            var tokenHandler = new JwtSecurityTokenHandler();

            // Token Creation
            var now = DateTime.UtcNow;
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, usuario.Email));
            claims.Add(new Claim(ClaimTypes.Sid, usuario.IDUsuario.ToString()));

            if (usuario.Adm)
                claims.Add(new Claim(ClaimTypes.Role, "adm"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                TokenIssuerName = "self",
                Lifetime = new Lifetime(now, now.AddMinutes(30)),
                SigningCredentials = credentials,

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;

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
