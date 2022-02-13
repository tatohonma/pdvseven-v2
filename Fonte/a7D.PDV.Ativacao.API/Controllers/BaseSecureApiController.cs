using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Entities;
using a7D.PDV.Ativacao.API.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel.Security.Tokens;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace a7D.PDV.Ativacao.API.Controllers
{
    public abstract class BaseSecureApiController : ApiController
    {
        protected AtivacaoContext db;
        protected UsuariosRepository usuarios;

        public BaseSecureApiController()
        {
            db = new AtivacaoContext();
            usuarios = new UsuariosRepository(db);
        }

        protected bool IsAdminRequest()
        {
            var token = ObterTokenRequisicao();
            return token?.Claims?.Any(c => c.Type == "role" && c.Value == "adm") == true;
        }

        private JwtSecurityToken ObterTokenRequisicao()
        {
            try
            {
                var auth = Request.Headers.GetValues("x-auth-token").ToList();
                if (auth != null && auth.Count() > 0)
                {
                    var jwt = auth[0];
                    var tokenHandler = new JwtSecurityTokenHandler();

                    var securityKey = GetBytes("ThisIsAnImportantStringAndIHaveNoIdeaIfThisIsVerySecureOrNot!");
                    var validationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidIssuer = "self",
                        IssuerSigningToken = new BinarySecretSecurityToken(securityKey),
                        ValidateLifetime = true,
                        RequireExpirationTime = false
                    };

                    SecurityToken secureToken;
                    tokenHandler.ValidateToken(jwt, validationParameters, out secureToken);
                    var jwtToken = secureToken as JwtSecurityToken;
                    return jwtToken;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        protected async Task<Usuario> UsuarioRequisicaoAsync()
        {
            var token = ObterTokenRequisicao();
            if (token != null)
            {
                return await usuarios.BuscarPorId(Convert.ToInt32(token.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid").Value));
            }
            return null;
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
                db.Dispose();
            }
        }
    }
}