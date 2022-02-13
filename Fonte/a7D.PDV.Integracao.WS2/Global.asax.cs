using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.Fiscal.NFCe;
using a7D.PDV.Integracao.WS2.Controllers;
using a7D.PDV.Integracao.WS2.Helpers;
using a7D.PDV.Integracao.WS2.Properties;
using System;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace a7D.PDV.Integracao.WS2
{
    public class WebApiApplication : HttpApplication
    {
        protected static string FileName => Settings.Default.TraceLog;

        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();
                GlobalConfiguration.Configure(WebApiConfig.Register);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                ConfiguracoesSistema.TimeRefresh = 5;

                string[] assemble = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',');
                string exeDll = assemble[0];
                AC.RegitraPDV(exeDll, assemble[1].Split('=')[1]);

                if (ConfiguracoesSistema.Valores.Fiscal == "NFCe")
                {
                    NFeFacade.ConfigPathXSD(Server.MapPath("~/bin"));
                }

                System.Threading.Tasks.Task.Run(() =>
                {
                    using (var pdvServico = new Licencas())
                    {
                        pdvServico.Validar(TipoApp.SERVER); // Tenta sincronizar licenças e obter cliente
                        ProdutosController.ValidateCache();
                    }
                });
            }
            catch (Exception ex)
            {
                Logs.Erro(ex);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var ex = Server.GetLastError();
                ExceptionPDV exPDV;
                if (ex.Message.Contains("não encontrado") || ex.Message.Contains("not found"))
                    exPDV = new ExceptionPDV(CodigoErro.A002, ex);
                else
                    exPDV = new ExceptionPDV(CodigoErro.E040, ex);
                Logs.Erro(exPDV);
                Response.Write(exPDV.Message + "\r\nConsulte o log do Windows");
                Server.ClearError();
            }
            catch (Exception)
            {
            }
        }

        protected void Application_BeginRequest()
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "0");
                HttpContext.Current.Response.End();
            }

            WriteTraceLog();
        }

        protected void WriteTraceLog()
        {
            if (string.IsNullOrEmpty(FileName))
                return;

            string id = String.Format("Id: {0} Uri: {1}", Guid.NewGuid(), Request.Url);
            string file = Server.MapPath(string.Format(FileName, DateTime.Now));

            var fi = new FileInfo(file);
            if (!fi.Directory.Exists)
                fi.Directory.Create();

            var input = new FilterSaveLog(HttpContext.Current, Request.Filter, file, id);
            input.SetFilter(false);
            Request.Filter = input;

            var output = new FilterSaveLog(HttpContext.Current, Response.Filter, file, id);
            output.SetFilter(true);
            Response.Filter = output;
        }
    }
}
