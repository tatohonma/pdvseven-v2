using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Integracao.WS;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using System.Web.UI;

namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class ProdutosImagemController : Controller
    {
        [HttpGet]
        [Route("produtos/{id}/imagem")]
        [Route("ImagensProdutos/{id}")]
        [OutputCache(
            Duration = 300,
            VaryByParam = "id",
            Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult GetImagem(string id)
        {
            try
            {
                bool thumb = id.Contains("_thumb");
                if (thumb)
                    id = id.Replace("_thumb", "");

                var idparts = id.Split('.');
                var nid = int.Parse(idparts[0]);
                var produtoImagem = BLL.ProdutoImagem.CarregarPorProduto(nid);
                if (produtoImagem?.Imagem?.IDImagem == null)
                    return HttpNotFound();

                BLL.Imagem.CarregarDados(produtoImagem.Imagem);
                var format = new JpegFormat { Quality = 70 };
                var outStream = new MemoryStream();
                using (var imageFactory = new ImageFactory(preserveExifData: true))
                {
                    if (thumb)
                    {
                        int newWidth = (int)ImagensProdutos.TamanhoThumbLargura;
                        int newHeigth = (int)ImagensProdutos.TamanhoThumbAltura;
                        var image = ImagensProdutos.CreateThumb(produtoImagem.Imagem.Dados);

                        imageFactory.Load(image.Dados)
                             .Resize(new Size(newWidth, newHeigth))
                             .Format(new PngFormat { Quality = 65 })
                             .Save(outStream);

                        return File(outStream, "image/jpeg");
                    }
                    else
                    {
                            imageFactory.Load(produtoImagem.Imagem.Dados)
                            .Format(format)
                            .Save(outStream);

                        return File(outStream, "image/jpeg");
                    }
                }
            }
            catch (ThreadAbortException)
            {
                return null;
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new Erro(50, ex), "application/json");
            }
        }
    }
}