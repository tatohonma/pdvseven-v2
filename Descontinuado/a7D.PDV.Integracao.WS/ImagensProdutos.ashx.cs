using a7D.PDV.BLL;
using a7D.PDV.Model;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.WS
{
    /// <summary>
    /// Summary description for ImagensProdutos
    /// </summary>
    public class ImagensProdutos : System.Web.UI.Page, IHttpHandler
    {
        private string _cacheFolder = "/imgcache";

        public ImagensProdutos()
        {
            if (!Directory.Exists(Server.MapPath(_cacheFolder)))
            {
                Directory.CreateDirectory(Server.MapPath(_cacheFolder));
            }
        }

        private string id;
        private string locale;
        //private bool nocache = false;
        private double TamanhoThumbAltura
        {
            get
            {
                var tamanho = Convert.ToDouble(ConfiguracoesSistema.Valores.ImagemProdutoAlturaThumb);
                if (tamanho > 0)
                    return tamanho;
                return 128d;
            }
        }
        private double TamanhoThumbLargura
        {
            get
            {
                var tamanho = Convert.ToDouble(ConfiguracoesSistema.Valores.ImagemProdutoLarguraThumb);
                if (tamanho > 0)
                    return tamanho;
                return 60d;
            }
        }


        public override void ProcessRequest(HttpContext context)
        {
            //var nocacheheaders = context.Request.Headers["Cache-Control"];
            //if (string.IsNullOrWhiteSpace(nocacheheaders) == false)
            //{
            //    nocache = nocacheheaders == "no-cache";
            //}
            id = Page.RouteData.Values["id"] == null ? string.Empty : Convert.ToString(Page.RouteData.Values["id"]);
            locale = Page.RouteData.Values["locale"] == null ? "pt_BR" : Page.RouteData.Values["locale"].ToString();

            var thumb = false;

            if (string.IsNullOrWhiteSpace(id) == false)
            {
                if (id.Contains("."))
                    id = id.Substring(0, id.IndexOf("."));

                if (id.EndsWith("_thumb"))
                {
                    thumb = true;
                    id = id.Substring(0, id.IndexOf("_thumb"));
                }

                var produto = BLL.Produto.Carregar(Convert.ToInt32(id));
                var dataCache = produto.DtUltimaAlteracao.Value.ToString("yyyMMddHHmmssfff");
                var cachefile = produto.IDProduto.Value + "_" + dataCache;
                cachefile = Path.Combine(Server.MapPath(_cacheFolder), cachefile + (thumb ? "_thumb.cache" : ".cache"));

                ProdutoImagemInformation pi = ProdutoImagem.CarregarPorProduto(Convert.ToInt32(id));
                pi.Imagem = Imagem.Carregar(pi.Imagem.IDImagem.Value);

                //if (nocache == false)
                lock (cachefile)
                {
                    if (RetornarDoCache(context.Response, cachefile, pi.Imagem.Extensao, thumb))
                        return;
                }

                if (pi != null && pi.Imagem != null && pi.Imagem.IDImagem.HasValue)
                {
                    byte[] dadosImagem = null;
                    long tamanhoImagem = 0;

                    pi.Imagem = Imagem.CarregarCompleto(pi.Imagem.IDImagem.Value);
                    using (var imgDeDados = new ConversorByteArrayParaImagem(pi.Imagem.Dados).Imagem)
                    {
                        if (thumb)
                        {
                            var image = CreateThumb(TamanhoThumbLargura, TamanhoThumbAltura, pi.Imagem.Dados);
                            dadosImagem = image.Dados;
                            tamanhoImagem = image.Tamanho;
                        }
                        else
                        {
                            var imagemOtimizada = OtimizarImagem(pi.Imagem.Dados);
                            dadosImagem = imagemOtimizada.Dados;
                            tamanhoImagem = imagemOtimizada.Dados.LongLength;
                        }

                        //if (nocache == false)
                        lock (dadosImagem)
                        {
                            DeletarCacheAntigo(id, thumb, dataCache);
                            File.WriteAllBytes(cachefile, dadosImagem);
                        }

                        context.Response.ContentType = "image/png";
                        context.Response.AddHeader("Content-Length", tamanhoImagem.ToString());
                        context.Response.AddHeader("Content-Disposition", string.Format("filename=\"{0}.png\"", thumb ? id + "_thumb" : id));
                        context.Response.BinaryWrite(dadosImagem);
                        return;
                    }
                }

                context.Response.StatusCode = 404;
                context.Response.End();
                return;
            }
        }

        private bool RetornarDoCache(HttpResponse response, string cachefile, string extensao, bool thumb)
        {
            if (File.Exists(cachefile))
            {
                var dados = File.ReadAllBytes(cachefile);

                response.ContentType = string.Format("image/{0}", extensao);
                response.AddHeader("Content-Length", dados.Length.ToString());
                response.AddHeader("Content-Disposition", string.Format("filename=\"{0}.png\"", thumb ? id + "_thumb" : id));
                response.BinaryWrite(dados);

                return true;
            }
            return false;
        }

        private void DeletarCacheAntigo(string idProduto, bool thumb, string dataCache)
        {
            DirectoryInfo d = new DirectoryInfo(Server.MapPath(_cacheFolder));
            FileInfo[] files;
            string cacheFile;
            if (thumb)
            {
                files = d.GetFiles("*" + idProduto + "_*_thumb.cache");
                cacheFile = idProduto + "_" + dataCache + "_thumb.cache";
            }
            else
            {
                files = d.GetFiles("*" + idProduto + "_?????????????????.cache");
                cacheFile = idProduto + "_" + dataCache + ".cache";
            }


            if (files.LongLength < 1)
                return;

            foreach (var file in files)
            {
                if (file.Name != cacheFile)
                    file.Delete();
            }
        }

        static DadosImagem OtimizarImagem(byte[] imagem)
        {
            var format = new PngFormat { Quality = 70 };

            using (MemoryStream outStream = new MemoryStream())
            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
            {
                imageFactory.Load(imagem)
                    .Format(format)
                    .Save(outStream);

                var dadosNovaImagem = new ConversorImagemParaByteArray(Image.FromStream(outStream)).Dados;
                return new DadosImagem
                {
                    Dados = dadosNovaImagem,
                    Tamanho = dadosNovaImagem.LongLength,
                };
            }
        }

        static DadosImagem CreateThumb(double width, double height, byte[] imagem)
        {
            int newWidth = (int)width;
            int newHeigth = (int)height;

            ISupportedImageFormat format = new PngFormat { Quality = 65 };

            var size = new Size(newWidth, newHeigth);

            using (MemoryStream outStream = new MemoryStream())
            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
            {
                imageFactory.Load(imagem)
                    .Resize(size)
                    .Format(format)
                    .Save(outStream);

                var dadosNovaImagem = new ConversorImagemParaByteArray(Image.FromStream(outStream)).Dados;
                return new DadosImagem
                {
                    Dados = dadosNovaImagem,
                    Tamanho = dadosNovaImagem.LongLength,
                };
            }
            //if (width < 1)
            //    newWidth = Convert.ToInt32(image.Width * width);

            //if (height < 1)
            //    newHeigth = Convert.ToInt32(image.Height * height);

            //var rect = Imagem.CreateCroppedRectangle(image, newWidth, newHeigth);
            //return Imagem.Resize(image, new Rectangle(0, 0, newWidth, newHeigth), rect);
        }

        public new bool IsReusable
        {
            get
            {
                return true;
            }
        }

        class DadosImagem
        {
            public byte[] Dados { get; set; }
            public long Tamanho { get; set; }
        }
    }
}