using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public class ConversorImagemParaByteArray
    {
        private Image Imagem;

        public byte[] Dados
        {
            get
            {
                byte[] retorno = null;
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        Imagem.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        retorno =  ms.ToArray();
                    }
                }
                catch { }
                return retorno;
            }
            private set { }
        }

        public ConversorImagemParaByteArray(Image imagem)
        {
            Imagem = imagem;
        }
    }
}
