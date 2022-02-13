using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace a7D.PDV.Gateway.UIWeb.Controllers
{
    public class AuthController : ApiController
    {
        private readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public class Usuario
        {
            public string email { get; set; }
            public string senha { get; set; }
        }

        [HttpPost]
        public IHttpActionResult Login([FromBody]Usuario usuario)
        {
            if (usuario == null)
                return BadRequest();

            if (usuario.email == "adm@pdvseven.com.br" && usuario.senha == "pdv7123")
            {
                var payload = new Dictionary<string, object>()
                {
                    { "issuer", "login" },
                    { "access", "admin" },
                    { "exp", Math.Round((DateTime.UtcNow.AddMinutes(30) - unixEpoch).TotalSeconds) }
                };
                var jwt = JWT.JsonWebToken.Encode(payload, "PDVSEVENGATEWAYPAGAMENTOSPARANGARICORIRIMIRROARO124123!", JWT.JwtHashAlgorithm.HS384);
                return Ok(new { token = jwt });
            }

            return StatusCode(HttpStatusCode.Forbidden);
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;

        }
    }
}
