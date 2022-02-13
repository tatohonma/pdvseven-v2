using a7D.PDV.Integracao.API2.Model;
using System;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class TemaImagemController : Controller
    {
        [HttpGet]
        [Route("imagensTema/{tema}/{locale}/{nome}")]
        [OutputCache(
            Duration = 300,
            VaryByParam = "nome",
            Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult ImagensTema(string tema, string locale, string nome)
        {
            try
            {
                var bt = WS.ImagensTema.RequestResource(tema, locale, nome);
                if (bt == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    var outStream = new MemoryStream(bt);
                    outStream.Position = 0;
                    return File(outStream, "image/png");
                }
            }
            catch (Exception ex)
            {
                BLL.Logs.Erro(ex);
                ControllerContext.HttpContext.Response.StatusCode = 500;
                ControllerContext.HttpContext.Response.StatusDescription = ex.Message;
                return Json(new Erro(50, ex), "application/json");
            }
        }

        [HttpGet]
        [Route("imagensTema/{tema}/{nome}")]
        [OutputCache(
            Duration = 300,
            VaryByParam = "nome",
            Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult ImagensTema(string tema, string nome)
        {
            return ImagensTema(tema, "", nome);
        }
    }
}