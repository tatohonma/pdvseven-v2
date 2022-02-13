using System;
using System.Drawing;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    public static class BarCodeServices
    {
        /// <summary>
        /// Gera um BitMap baseado em uma string 'bfpfbl' para geração de código de barras ( Alterado por Alexandre Savelli Bencz _
        /// </summary>
        /// <param name="NumTexto">String no formarto 'bfplblpl'</param>
        /// <param name="nScale">Quanto é a escala do codigo de barras</param>
        /// <param name="resolucao">Resolucao do código de barras</param>
        /// <returns>Bitmap</returns>
        public static Bitmap BarCodeImage(string NumTexto, int nScale, int height)
        {
            // Transforma o numero em uma string padrão de barras
            string cCodBar = BarCode(NumTexto);

            if (nScale < 3)
                throw new Exception("Escala minima é 3");

            // Ajusta a Escala padrão 
            //nScale /= 3; // Atenção, o ideal para a escala é ser multiplo de 3

            int wSF = nScale / 3;
            int wSL = nScale;

            // Codigo de Barras 2 por 5  =>  2 digitos são representados por 5 Barras PBPBP largas ou finas
            int nWidth = NumTexto.Length * 4 * nScale;

            Bitmap bmp = new Bitmap(nWidth, height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            // Posição da linha atualmente desenhada (cursor)
            int nX = 0;
            for (int n = 0; n < cCodBar.Length; n += 2)
            {
                switch (cCodBar.Substring(n, 2))
                {
                    case "bf":
                        g.FillRectangle(Brushes.White, nX, 0, wSF, height);
                        nX += wSF;
                        break;
                    case "pf":
                        g.FillRectangle(Brushes.Black, nX, 0, wSF, height);
                        nX += wSF;
                        break;
                    case "bl":
                        g.FillRectangle(Brushes.White, nX, 0, wSL, height);
                        nX += wSL;
                        break;
                    case "pl":
                        g.FillRectangle(Brushes.Black, nX, 0, wSL, height);
                        nX += wSL;
                        break;
                }
            }

            // Extrai apenas a imagem 100% util
            Bitmap bmp2 = new Bitmap(nX, height);
            g = Graphics.FromImage(bmp2);
            g.DrawImage(bmp, 0, 0);

            return bmp2;
        }

        /// <summary>
        /// Gera uma string que representa um código de barras de um numero especifico
        /// </summary>
        /// <param name="NumTexto">digitos a serem codificados</param>
        /// <returns>retona uma sequancia das ssequencias "bf","bl","pf","pl"</returns>
        /// <remarks>
        /// bf -> Branco Fino
        /// bl -> Branco Largo
        /// pf -> Preto Fino
        /// pl -> Preto Largo
        /// </remarks>
        public static String BarCode(String NumTexto)
        {
            var cOut = new System.Text.StringBuilder();
            String f, texto;
            int fi, f1, f2, i;
            string[] BarCodes = new string[100];
            BarCodes[0] = "00110";
            BarCodes[1] = "10001";
            BarCodes[2] = "01001";
            BarCodes[3] = "11000";
            BarCodes[4] = "00101";
            BarCodes[5] = "10100";
            BarCodes[6] = "01100";
            BarCodes[7] = "00011";
            BarCodes[8] = "10010";
            BarCodes[9] = "01010";

            for (f1 = 9; f1 >= 0; f1--)
            {
                for (f2 = 9; f2 >= 0; f2--)
                {
                    fi = f1 * 10 + f2;
                    texto = "";
                    for (i = 0; i < 5; i++)
                    {
                        texto +=
                            BarCodes[f1].Substring(i, 1) +
                            BarCodes[f2].Substring(i, 1);
                    }
                    BarCodes[fi] = texto;
                }
            }

            // Inicialização padrão
            cOut.Append("pf");
            cOut.Append("bf");
            cOut.Append("pf");
            cOut.Append("bf");

            texto = NumTexto;
            if (texto.Length % 2 != 0)
                texto = "0" + texto;

            //Draw dos dados
            while (texto.Length > 0)
            {
                i = Int32.Parse(texto.Substring(0, 2));
                texto = texto.Substring(2);
                f = BarCodes[i];
                for (i = 0; i < 10; i += 2)
                {
                    if (f.Substring(i, 1) == "0")
                        cOut.Append("pf");
                    else
                        cOut.Append("pl");

                    if (f.Substring(i + 1, 1) == "0")
                        cOut.Append("bf");
                    else
                        cOut.Append("bl");

                }
            }

            // Finalização padrão
            cOut.Append("pl");
            cOut.Append("bf");
            cOut.Append("pf");

            return cOut.ToString();
        }
    }
}
