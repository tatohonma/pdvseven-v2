using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace a7D.PDV.BLL.Utils
{
    public static class ImageUtil
    {
        public static Bitmap LogoPDV7_Horizontal_PB()
        {
            return Properties.Resources.PDV7_horizontal_PB;
        }

        public static Bitmap Imagem_ViagemSacola()
        {
            return Properties.Resources.ViagemSacola;
        }

        public static byte[] ReduzEtransforma(Bitmap bmp, int height)
        {
            using (var bmp2 = new Bitmap(bmp.Width, height))
            {
                var g = Graphics.FromImage(bmp2);
                g.DrawImage(bmp, 0, 0);
                using (var ms = new MemoryStream())
                {
                    bmp2.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        private static string ConverterImagemParaBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] imageArray;

            using (MemoryStream imageStream = new MemoryStream())
            {
                image.Save(imageStream, format);
                imageArray = new byte[imageStream.Length];
                imageStream.Seek(0, System.IO.SeekOrigin.Begin);
                int lenght = (int)imageStream.Length;
                imageStream.Read(imageArray, 0, lenght);
            }

            return Convert.ToBase64String(imageArray);
        }

        public static Image RedimensionarCortandoCentralizado(Image image, bool produto = true, int width = 0, int height = 0)
        {
            ImageCodecInfo jpgEncoder = null;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpgEncoder = codec;
                    break;
                }
            }


            if (produto)
            {
                width = Convert.ToInt32(ConfiguracoesSistema.Valores.ImagemProdutoLargura);
                if (width == 0) width = 500;
                height = Convert.ToInt32(ConfiguracoesSistema.Valores.ImagemProdutoAltura);
                if (height == 0) height = 500;

                width = Math.Min(width, 1920);
                height = Math.Min(height, 1080);
            }

            var rect = ImageUtil.CreateCroppedRectangle(image, width, height);
            rect.X = (image.Width / 2) - (rect.Width / 2);
            rect.Y = (image.Height / 2) - (rect.Height / 2);

            var destImage = Resize(image, new Rectangle(0, 0, width, height), rect);

            if (jpgEncoder != null)
            {
                var encoder = Encoder.Quality;
                var encoderParameters = new EncoderParameters(1);
                int quality = 90;

                EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;

                using (var ms = new MemoryStream())
                using (var jpg = new ConvertImageParaJpeg(destImage).Jpeg)
                {
                    //var caminhofoto = Path.Combine(caminho, rnd + ".jpg");
                    jpg.Save(ms, jpgEncoder, encoderParameters);
                    destImage.Dispose();
                    destImage = null;
                    destImage = new Bitmap(Image.FromStream(ms));
                }
            }
            return destImage;
        }

        public static Image Resize(Image image, Rectangle destRectange, Rectangle sourceRectangle)
        {
            var rezisedImage = new Bitmap(destRectange.Width, destRectange.Height);
            using (var g = Graphics.FromImage(rezisedImage))
            {
                g.SmoothingMode = SmoothingMode.Default;
                g.InterpolationMode = InterpolationMode.Default;
                g.PixelOffsetMode = PixelOffsetMode.Default;
                g.DrawImage(image, destRectange, sourceRectangle, GraphicsUnit.Pixel);
                return rezisedImage;
            }
        }

        public static Rectangle CreateCroppedRectangle(Image image, int width, int height)
        {
            var size = new Size(width, height);
            var size2 = new Size(image.Width, image.Height);

            //The maximum scale width we could use
            float maxWidthScale = (float)size2.Width / (float)size.Width;

            //The maximum scale height we could use
            float maxHeightScale = (float)size2.Height / (float)size.Height;

            //Use the smaller of the 2 scales for the scaling
            float scale = Math.Min(maxHeightScale, maxWidthScale);


            size.Width = (int)(size.Width * scale);
            size.Height = (int)(size.Height * scale);

            return new Rectangle(new Point(), size);
        }

        public static Image RedimensionarImagem(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
    }
}
