using System.Web;
using System.Web.Mvc;

namespace a7D.PDV.Integracao.WS2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
