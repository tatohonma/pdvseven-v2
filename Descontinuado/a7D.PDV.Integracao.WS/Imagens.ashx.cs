using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace a7D.PDV.BackOffice.UIWeb
{
    /// <summary>
    /// Summary description for Imagens
    /// </summary>
    public class Imagens : System.Web.UI.Page, IHttpHandler
    {
        private int id;
        private string locale;
        private bool thumb;
        private readonly int tamanhoThumb = 150;

        public override void ProcessRequest(HttpContext context)
        {
            id = Page.RouteData.Values["id"] == null ? 0 : Convert.ToInt32(Page.RouteData.Values["id"]);
            locale = Page.RouteData.Values["locale"] == null ? "pt_BR" : Page.RouteData.Values["locale"].ToString();
            thumb = Page.RouteData.Values["thumb"] != null;

            if (id > 0)
            {
                ProdutoImagemInformation pi = ProdutoImagem.CarregarPorProduto(id);
                if (pi != null && pi.Imagem != null && pi.Imagem.IDImagem.HasValue)
                {
                    byte[] dadosImagem = null;
                    long tamanhoImagem = 0;

                    pi.Imagem = Imagem.CarregarCompleto(pi.Imagem.IDImagem.Value);

                    if (thumb)
                    {
                        var image = CreateThumbnailImage(tamanhoThumb, GetImageFromBytes(pi.Imagem.Dados), true);
                        dadosImagem = GetImageBytes(image, ImageFormat.Png);
                        tamanhoImagem = dadosImagem.LongLength;
                    }
                    else
                    {
                        dadosImagem = pi.Imagem.Dados;
                        tamanhoImagem = pi.Imagem.Dados.LongLength;
                    }

                    context.Response.ContentType = string.Format("image/{0}", pi.Imagem.Extensao);
                    context.Response.AddHeader("Content-Length", tamanhoImagem.ToString());
                    context.Response.BinaryWrite(dadosImagem);
                }
            }
        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static Image CreateThumbnailImage(int width, int height, Image image, bool center)
        {
            // Create our new image
            Bitmap newImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                if (center && image.Width != image.Height)
                {
                    Rectangle srcRect = new Rectangle();

                    if (image.Width > image.Height)
                    {
                        srcRect.Width = image.Height;
                        srcRect.Height = image.Height;
                        srcRect.X = (image.Width - image.Height) / 2;
                        srcRect.Y = 0;
                    }
                    else
                    {
                        srcRect.Width = image.Width;
                        srcRect.Height = image.Width;
                        srcRect.Y = (image.Height - image.Width) / 2;
                        srcRect.X = 0;
                    }

                    g.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(image, 0, 0, width, height);
                }
            }

            return newImage;
        }

        public static Image CreateThumbnailImage(int size, Image image, bool center)
        {
            return CreateThumbnailImage(size, size, image, center);
        }

        public static Image GetImageFromBytes(byte[] imageBytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);

                    return image;
                }
            }
            catch
            {
                return null;
            }
        }

        public static byte[] GetImageBytes(Image image, ImageFormat format)
        {
            byte[] imageBytes;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    imageBytes = ms.ToArray();

                    return imageBytes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}