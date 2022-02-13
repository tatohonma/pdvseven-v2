using a7D.PDV.Ativacao.API.Extensions;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel.Security.Tokens;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace a7D.PDV.Ativacao.API.Filters
{
    public class ApiAuthAttribute : AuthorizeAttribute
    {
        private bool RequerAdm { get; }

        public ApiAuthAttribute(bool requerAdm = true)
        {
            RequerAdm = requerAdm;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (IsAuthorized(actionContext))
                return;
            
            HandleUnauthorizedRequest(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                //if (actionContext.IsLocal())
                //    return true;

                var auth = actionContext.Request.Headers.GetValues("x-auth-token").ToList();
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
                    if (RequerAdm)
                        return jwtToken.Claims.Any(c => c.Type == "role" && c.Value == "adm");

                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;

        }
    }
}