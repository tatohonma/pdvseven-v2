using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace a7D.PDV.Integracao.MyTapp.WS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return new FilePathResult("index.html", "text/html");
        }
    }
}