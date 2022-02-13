using System.Web.Mvc;

namespace a7D.PDV.Ativacao.UIWeb2.Filters
{
    public class AuthFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object usuario = filterContext.HttpContext.Session["UsuarioLogado"];
            if (usuario == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    (
                        new { controller = "Login", action = "Index" }
                    ));
            }
        }
    }
}