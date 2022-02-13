using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace a7D.PDV.Gateway.UIWeb.Filters
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
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
                var auth = actionContext.Request.Headers.GetValues("x-auth-token").ToList();
                if (auth != null && auth.Count() > 0)
                {
                    var jwt = auth[0];
                    var payload = JWT.JsonWebToken.DecodeToObject<Dictionary<string, object>>(jwt, "PDVSEVENGATEWAYPAGAMENTOSPARANGARICORIRIMIRROARO124123!");
                    return true;
                }
                return false;
            }
            catch (JWT.SignatureVerificationException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}