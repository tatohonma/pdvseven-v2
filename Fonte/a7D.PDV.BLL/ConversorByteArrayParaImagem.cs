using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public class ConversorByteArrayParaImagem
    {
        private byte[] byteData;

        public ConversorByteArrayParaImagem(byte[] byteData)
        {
            this.byteData = byteData;
        }

        public Image Imagem
        {
            get
            {
                Image returnImage = null;
                try
                {
                    MemoryStream ms = new MemoryStream(byteData, 0, byteData.Length);
                    ms.Write(byteData, 0, byteData.Length);
                    returnImage = Image.FromStream(ms);
                }
                catch
                {

                }
                return returnImage;
            }
            private set { }
        }
    }
}
