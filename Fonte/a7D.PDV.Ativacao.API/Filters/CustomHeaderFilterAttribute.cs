using System.Web.Http.Filters;

namespace a7D.PDV.Ativacao.API.Filters
{
    public class CustomHeaderFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Request.Properties != null && actionExecutedContext.Request.Properties.ContainsKey("count"))
            {
                if (actionExecutedContext.Request.Properties["count"] is string count)
                    actionExecutedContext.Response.Headers.Add("count", count);
            }
        }
    }
}