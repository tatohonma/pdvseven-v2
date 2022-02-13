using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace a7D.PDV.Integracao.WS2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // Unificação WS1
            //routes.MapPageRoute("imagens",
            //   "ImagensProdutos/{id}",
            //   "~/ImagensProdutos.ashx", true);

            //routes.MapPageRoute("imagensthumb",
            //    "ImagensProdutos/{id}/{*thumb}",
            //    "~/ImagensProdutos.ashx", true);

            //routes.MapPageRoute("imagenstemaloc",
            //    "imagensTema/{tema}/{locale}/{nome}",
            //    "~/ImagensTema.ashx", true);

            //routes.MapPageRoute("imagenstema",
            //    "imagensTema/{tema}/{nome}",
            //    "~/ImagensTema.ashx", true);

            //routes.MapPageRoute("imagenstemalista",
            //    "imagensTema/{tema}",
            //    "~/ImagensTema.ashx", true);

            //routes.MapPageRoute("imagensprodutoszip", "imagensprodutos.zip", "~/ImagensProdutosZip", true);
        }
    }
}
