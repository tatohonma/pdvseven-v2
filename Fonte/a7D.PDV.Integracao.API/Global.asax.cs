using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace a7D.PDV.Integracao.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.Insert(0, new RawMediaFormatter());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
