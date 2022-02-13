using a7D.PDV.EF;
using a7D.PDV.EF.Models;
using System;
using System.Web.Mvc;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class PesquisaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Obrigado(int nota)
        {
            try
            {
                var resp = new tbPesquisa
                {
                    Data = DateTime.Now,
                    Valor = nota
                };
                Repositorio.Inserir(resp);
            }
            catch(Exception ex)
            {
                ex.Data.Add("nota", nota);
                BLL.Logs.Erro(ex);
            }
            return View();
        }
    }
}