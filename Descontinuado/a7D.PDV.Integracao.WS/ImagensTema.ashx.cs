using a7D.PDV.BLL;
using a7D.PDV.Model.BLL;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace a7D.PDV.Integracao.WS
{
    /// <summary>
    /// Summary description for ImagensTema
    /// </summary>
    public class ImagensTema : System.Web.UI.Page, IHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (context.Request.RequestType == "GET")
            {
                var routeNome = Page.RouteData.Values["nome"];
                var routeTema = Page.RouteData.Values["tema"];
                var routeLocale = Page.RouteData.Values["locale"];

                var nome = routeNome == null ? string.Empty : Convert.ToString(routeNome);
                var nomeTema = routeTema == null ? string.Empty : Convert.ToString(routeTema);
                var locale = routeLocale == null ? "pt_BR" : routeLocale.ToString();

                if (string.IsNullOrWhiteSpace(nomeTema) == false && string.IsNullOrWhiteSpace(nome) == false)
                {
                    if (nome.Contains("."))
                        nome = nome.Substring(0, nome.IndexOf("."));
                    var idioma = Idioma.CarregarPorCodigo(locale);
                    var tema = TemaCardapio.CarregarPorNome(nomeTema);

                    if (idioma != null && tema != null)
                    {
                        var imagemTema = ImagemTema.CarregarPorNome(nome, tema.IDTemaCardapio.Value, idioma.IDIdioma.Value);
                        if (imagemTema?.IDImagemTema != null)
                        {
                            imagemTema.Imagem = Imagem.CarregarCompleto(imagemTema.Imagem.IDImagem.Value);

                            byte[] dadosImagem = null;
                            long tamanhoImagem = 0;

                            var imagemOtimizada = OtimizarImagem(imagemTema.Imagem.Dados);

                            dadosImagem = imagemOtimizada.Dados;
                            tamanhoImagem = dadosImagem.LongLength;

                            context.Response.ContentType = "image/png";
                            context.Response.AddHeader("Content-Length", tamanhoImagem.ToString());
                            context.Response.BinaryWrite(dadosImagem);
                            return;
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(nomeTema) == false && string.IsNullOrWhiteSpace(nome) == true)
                {
                    var tema = TemaCardapio.CarregarPorNome(nomeTema);
                    if (tema != null)
                    {
                        var listaImagens = ImagemTema.ListarPorTema(tema.IDTemaCardapio.Value);

                        var sb = new StringBuilder();

                        if (listaImagens != null && listaImagens.Count > 0)
                        {
                            var listaOrdenada = listaImagens.GroupBy(li => li.Idioma, (key, g) => new
                            {
                                Idioma = key,
                                Imagens = g.ToList()
                            }).ToList();

                            sb.Append("<html><head><title>Imagens</title></head><body>\n");

                            foreach (var item in listaOrdenada)
                            {
                                sb.Append("<h4>").Append(item.Idioma.Nome).Append("</h4><hr />\n");

                                foreach (var imagem in item.Imagens)
                                {
                                    sb.Append(string.Format("<a href='{0}/{1}/{2}'>{2}</a><br />\n", context.Request.Url.AbsoluteUri, item.Idioma.Codigo, imagem.Nome));
                                }
                            }

                            sb.Append("</body></html>");
                            context.Response.ContentType = "text/html";
                            context.Response.Write(sb.ToString());
                            return;
                        }
                    }
                    else
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(string.Format("Tema {0} não encontrado.", nomeTema));
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.End();
                    return;
                }
                context.Response.StatusCode = 404;
                context.Response.End();
                return;
            }
            else
                throw new Exception("No methods other than GET allowed");
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

        class DadosImagem
        {
            public byte[] Dados { get; set; }
            public long Tamanho { get; set; }
        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}