using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace a7D.PDV.BLL.Services
{
    public static class ImpressoraWindows
    {
        public static Font FontePadrao { get; set; }
        public static Font FonteMonoSpace { get; set; }
        public static int Largura { get; set; }
        public static int Margem { get; set; }

        static ImpressoraWindows()
        {
            // Valores padrão para não dar erro, mas que podem ser redefinidos ao iniciar o programa!
            FontePadrao = new Font("Arial", 10);
            FonteMonoSpace = new Font("Lucida Console", 8);
            Largura = 280; // pixel
        }

        public static void ConfiguraFontes(Graphics g, out Font fTitulo, out Font fNormal, out Font fNormalB, out Font fPequena, out Font fPequenaB, int tamanho)
        {
            string fonte = "NK57 Monospace Cd Rg";
            string fonteItens = "Arial";

            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.Clear(Color.White);

            int[] t;
            if (tamanho < 300)
                t = new int[] { 11, 10, 9 };
            else if (tamanho < 350)
                t = new int[] { 12, 11, 10 };
            else
                t = new int[] { 15, 13, 11 };

            fTitulo = new Font(fonte, t[0], FontStyle.Bold);
            if (fTitulo.Name != fonte)
            {
                fonte = "Lucida Console";
                fTitulo = new Font(fonte, t[0], FontStyle.Bold);
            }
            else
                fonteItens = fonte;

            fNormal = new Font(fonteItens, t[1]);
            fNormalB = new Font(fonteItens, t[1], FontStyle.Bold);

            fPequena = new Font(fonte, t[2]);
            fPequenaB = new Font(fonte, t[2], FontStyle.Bold);
        }

        public static void ImprimirTexto(string nomeImpressoraWindows, bool monoSpace, IEnumerable<string> conteudo, int? tamanhoFonte = null)
        {
            string strConteudo = "";
            foreach (var item in conteudo)
                strConteudo += item + "\n";

            ImprimirTexto(nomeImpressoraWindows, monoSpace, strConteudo, true, tamanhoFonte);
        }

        public static void ImprimirTexto(string nomeImpressoraWindows, bool monoSpace, string texto, bool lThrow = false, int? tamanhoFonte = null)
        {
            try
            {
                // Elimina qualquer espaço necessário
                while (texto.EndsWith("\r") || texto.EndsWith("\n"))
                    texto = texto.Substring(0, texto.Length - 1);

                Font fonte = monoSpace ? FonteMonoSpace : FontePadrao;
                if (tamanhoFonte != null)
                    fonte = new Font(fonte.Name, tamanhoFonte.Value, FontStyle.Regular);

                Imprimir(nomeImpressoraWindows, (s, e) =>
                {
                    if(monoSpace)
                    {
                        e.Graphics.DrawString(texto, fonte, Brushes.Black, 0, 0);
                    }
                    else
                    {
                        var size = e.Graphics.MeasureString(texto, fonte, Largura);
                        var rc = new Rectangle(Margem, 0, Largura, (int)size.Height);
                        e.Graphics.DrawString(texto, fonte, Brushes.Black, rc);
                    }
                }, lThrow);
            }
            catch (Exception ex)
            {
                if (lThrow)
                    throw new ExceptionPDV(CodigoErro.A300, ex);
                else
                    Logs.ErroBox(CodigoErro.A300, ex);
            }
        }

        public static void ImprimirImagem(Image img, string impressora)
        {
            if (img == null)
                return;

            Imprimir(impressora, (s, e) =>
            {
                var rc = new RectangleF(Margem, 0, Largura, Largura * img.Height / img.Width);
                e.Graphics.DrawImage(img, rc);
            });
        }

        public static void Imprimir(string nomeImpressoraWindows, PrintPageEventHandler renderCallback, bool lThrow = false)
        {
            try
            {
                using (var pd = new PrintDocument()) // Unico 'PrintDocument' o Caixa!
                {
                    pd.PrinterSettings.PrinterName = nomeImpressoraWindows;
                    pd.PrintPage += renderCallback;
                    pd.Print();
                }
            }
            catch (Exception ex)
            {
                if (lThrow)
                    throw ex;
                else
                    Logs.ErroBox(CodigoErro.A300, ex);
            }
        }
    }
}
