using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using System;
using System.Threading.Tasks;
using System.Web.Routing;

namespace a7D.PDV.Integracao.WS
{
    public class Global : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("imagens",
            "ImagensProdutos/{id}",
            "~/ImagensProdutos.ashx", true);

            routes.MapPageRoute("imagensthumb",
                "ImagensProdutos/{id}/{*thumb}",
                "~/ImagensProdutos.ashx", true);

            //routes.MapPageRoute("imagensloc",
            //    "ImagensProdutos/{locale}/{id}/{*thumb}",
            //    "~/Imagens.ashx", true,
            //    new RouteValueDictionary {
            //        { "locale", "pt_BR" } },
            //    new RouteValueDictionary {
            //        { "locale", "[a-z]{2}_[A-Z]{2}"},
            //        { "id", @"\d" } });

            routes.MapPageRoute("imagenstemaloc",
                "imagensTema/{tema}/{locale}/{nome}",
                "~/ImagensTema.ashx", true);

            routes.MapPageRoute("imagenstema",
                "imagensTema/{tema}/{nome}",
                "~/ImagensTema.ashx", true);

            routes.MapPageRoute("imagenstemalista",
                "imagensTema/{tema}",
                "~/ImagensTema.ashx", true);

            routes.MapPageRoute("imagensprodutoszip", "imagensprodutos.zip", "~/ImagensProdutosZip", true);
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
                RegisterRoutes(RouteTable.Routes);
                ConfiguracoesSistema.TimeRefresh = 5;
            }
            catch (Exception ex)
            {
                LOG.Erro(ex);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                LOG.Erro(Server.GetLastError());
                Server.ClearError();
            }
            catch (Exception)
            {
            }
        }
    }
}