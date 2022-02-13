using a7D.PDV.BLL.Services;
using a7D.PDV.EF.Models;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Model;
using a7D.PDV.SAT;
using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class ImpressaoController : Controller
    {
        // Padrão par as chamadas:
        // impressao/(sat|conta|ticket)/{id}/(cols|width|qrcode)/{val}/{parametros}
        // exemplos:
        // http://localhost:7777/impressao/qrcode/Fabio%20Ferreira%20de%20Souza%2012345
        // http://localhost:7777/impressao/codebar/1234567890
        // SAT
        // http://localhost:7777/impressao/2/sat/40 => http://localhost:7777/impressao/sat/2/cols/40
        // http://localhost:7777/impressao/2-300/sat => http://localhost:7777/impressao/sat/2/width/300
        // http://localhost:7777/impressao/sat/2/qrcode
        // Conta
        // http://localhost:7777/impressao/2/conta/1 => http://localhost:7777/impressao/conta/2/cols/40/1
        // http://localhost:7777/impressao/2-300/conta/1 => http://localhost:7777/impressao/conta/2/width/300/1
        // Ticket
        // http://localhost:7777/impressao/20/ticket/0/1 => http://localhost:7777/impressao/ticket/20/0/1
        // http://localhost:7777/impressao/20/ticket/0/2 => http://localhost:7777/impressao/ticket/20/0/2
        // http://localhost:7777/impressao/20/ticket/0/3 => http://localhost:7777/impressao/ticket/20/0/3
        // Via Expedição
        // http://localhost:7777/impressao/expedicao/4208/width/400
        // http://localhost:7777/impressao/expedicao/4208/width/40

        static ImpressaoController()
        {
            var nomeServico = EF.Repositorio.Carregar<tbProduto>(p => p.IDProduto == ProdutoInformation.IDProdutoServico)?.Nome;
            if (nomeServico != null && nomeServico != " * Serviço")
                CupomSATService.NomeTaxaServico = ContaServices.NomeTaxaServico = nomeServico;
        }

        #region Utils

        private FileStreamResult FilePng(Bitmap bmp)
        {
            var outStream = new MemoryStream();
            bmp.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
            outStream.Position = 0;
            return File(outStream, "image/png");
        }

        [HttpGet]
        [Route("impressao/qrcode/{dados}")]
        [OutputCache(
           Duration = 300,
           VaryByParam = "dados",
           Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult GerarQRCODE(string dados)
        {
            return FilePng(CupomSATService.GerarQRCODE(dados));
        }

        [HttpGet]
        [Route("impressao/codebar/{dados}")]
        [OutputCache(
            Duration = 300,
            VaryByParam = "dados",
            Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult GeraCodeBar(string dados)
        {
            return FilePng(new Bitmap(CupomSATService.GeraCodeBar(dados)));
        }

        #endregion

        #region Ticket

        [HttpGet]
        [Route("impressao/ticket/{id}/{item}/{*tipo}")]
        [Route("impressao/{id}/ticket/{item}/{*tipo}")]
        public ActionResult TiketsGerar(int id, int item, string tipo = null)
        {
            try
            {
                var pedido = BLL.Pedido.CarregarCompleto(id);
                if (pedido == null)
                    return HttpNotFound();

                var bmp = TiketServices.Gerar(pedido, item, tipo);
                if (bmp == null)
                    return HttpNotFound();

                return FilePng(bmp);
            }
            catch (Exception ex)
            {
                BLL.Logs.Erro(ex);
                Response.StatusCode = 500;
                return Json(new Erro(50, ex), "application/json");
            }
        }

        #endregion

        #region Conta

        public static byte[] ImagemBytesConta(string prms)
        {
            var parms = prms.Split('-');
            int id = int.Parse(parms[0]);
            int w = 0;
            if (parms.Length == 2)
                w = int.Parse(parms[1]);

            var pedido = BLL.Pedido.PreparaConta(id, 0);
            if (pedido == null)
                return null;

            var bt = ContaServices.ImprimirImagemConta(pedido, w);

            return bt;
        }

        [HttpGet]
        [Route("impressao/conta/{id}/cols/{colunas}/{*pessoas}")] // TODO: Falta implementar colunas variáveis para conta em texto
        [Route("impressao/conta/{id}/width/{width}/{*pessoas}")]
        [Route("impressao/{prms}/conta/{*pessoas}")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ContaImpressao(string prms = null, int id = 0, int colunas = 0, int width = 0, int pessoas = 0)
        {
            try
            {
                if (prms != null)
                {
                    var parms = prms.Split('-');
                    id = int.Parse(parms[0]);
                    if (parms.Length == 2)
                        width = int.Parse(parms[1]);
                }

                var pedido = BLL.Pedido.PreparaConta(id, pessoas);
                if (pedido == null)
                    return HttpNotFound();

                if (width > 0)
                {
                    var bt = ContaServices.ImprimirImagemConta(pedido, width);
                    var img = Image.FromStream(new MemoryStream(bt));
                    var bmp = new Bitmap(img);
                    return FilePng(bmp);
                }
                else
                {
                    var h = ContaServices.PrepararDadosImpressao(pedido, colunas);
                    return Content(h.plain.ToString(), "text/plain");
                }
            }
            catch (Exception ex)
            {
                return Json(new Erro(50, ex), "application/json");
            }
        }

        #endregion

        #region SAT

        public static byte[] ImagemBytesSAT(string prms)
        {
            var parms = prms.Split('-');
            int id = int.Parse(parms[0]);
            int w = 0;
            if (parms.Length == 2)
                w = int.Parse(parms[1]);

            if (!CupomSATService.ImprimirCupomVenda(id, out byte[] bt, w))
                return null;

            return bt;
        }

        [HttpGet]
        [Route("impressao/fiscal/{id}/cols/{colunas}")]
        [Route("impressao/sat/{id}/cols/{colunas}")]
        [Route("impressao/{id}/sat/{colunas}")] // TODO: ANTIGO
        [Route("impressao/{id}/fiscal/{colunas}")] // TODO: ANTIGO
        public ActionResult TextoSAT(int id, int colunas)
        {
            try
            {
                string texto = SATService.CupomVendaTexto(id, colunas);

                if (texto == null)
                    return new RedirectResult($"/impressao/conta/{id}/cols/{colunas}/0");
                // return HttpNotFound();

                return Content(texto, "text/plain");
            }
            catch (Exception ex)
            {
                return Json(new Erro(50, ex), "application/json");
            }
        }

        [HttpGet]
        [Route("impressao/fiscal/{id}/width/{width}")]
        [Route("impressao/sat/{id}/width/{width}")]
        [Route("impressao/{prms}/sat")]
        public ActionResult ImagemSAT(string prms = null, int id = 0, int width = 0)
        {
            try
            {
                if (prms == null)
                    prms = $"{id}-{width}";

                byte[] bt = ImagemBytesSAT(prms);
                if (bt == null)
                    return new RedirectResult($"/impressao/conta/{id}/width/{width}");
                // return HttpNotFound();

                var img = Image.FromStream(new MemoryStream(bt));
                var bmp = new Bitmap(img);
                return FilePng(bmp);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new Erro(50, ex), "application/json");
            }
        }

        [HttpGet]
        [Route("impressao/fiscal/{id}/qrcode")]
        [Route("impressao/sat/{id}/qrcode")]
        [OutputCache(
            Duration = 300,
            VaryByParam = "id",
            Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult GerarQRCODEsat(int id)
        {
            var pedido = BLL.Pedido.Carregar(id);
            if (pedido?.RetornoSAT_venda?.IDRetornoSAT > 0)
            {
                var retornoSAT = BLL.RetornoSAT.Carregar(pedido.RetornoSAT_venda.IDRetornoSAT.Value);
                var dados = CupomSATService.ObterAssinaturaQRCODE(retornoSAT.arquivoCFeSAT);
                return GerarQRCODE(dados);
            }
            else
                return HttpNotFound();
        }

        #endregion

        #region Via Expedição

        [HttpGet]
        [Route("impressao/expedicao/{id}/cols/{cols}")]
        [Route("impressao/expedicao/{id}/width/{width}")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ViaExpedicao(int id = 0, int colunas = 0, int width = 0)
        {
            try
            {
                var pedido = BLL.Pedido.CarregarCompleto(id);
                if (pedido.TipoPedido?.TipoPedido != EF.Enum.ETipoPedido.Delivery)
                    return HttpNotFound();

                if (width > 0)
                {
                    var bt = ExpedicaoServices.ImagemVia(pedido, width);
                    var img = Image.FromStream(new MemoryStream(bt));
                    var bmp = new Bitmap(img);
                    return FilePng(bmp);
                }
                else
                {
                    var h = ExpedicaoServices.PrepararVia(pedido, colunas);
                    return Content(h.plain.ToString(), "text/plain");
                }
            }
            catch (Exception ex)
            {
                return Json(new Erro(50, ex), "application/json");
            }
        }

        #endregion
    }
}
