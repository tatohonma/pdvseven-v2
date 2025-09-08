using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace a7D.PDV.Ativacao.API.Filters
{
    public class CustomHeaderFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Items.ContainsKey("count"))
            {
                if (context.HttpContext.Items["count"] is string count)
                {
                    context.HttpContext.Response.Headers.Append("count", count);
                }
            }

            base.OnActionExecuted(context);
        }
    }
}
