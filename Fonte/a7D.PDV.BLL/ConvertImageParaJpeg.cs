using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public class ConvertImageParaJpeg
    {

        private Image imagem;

        public Image Jpeg
        {
            get
            {
                var msJpg = new MemoryStream();
                imagem.Save(msJpg, ImageFormat.Jpeg);
                return Image.FromStream(msJpg);
            }
            private set { }
        }
        public ConvertImageParaJpeg(Image img)
        {
            this.imagem = img;
        }
    }
}
