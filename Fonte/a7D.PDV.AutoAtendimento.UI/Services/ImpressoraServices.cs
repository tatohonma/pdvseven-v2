using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Threading.Tasks;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal class ImpressoraServices
    {
        static Font fNormal = new Font("Lucida Console", 8);
        public static int Width = 300;
        public static int Left = 0;

        PrinterSettings config;

        internal ImpressoraServices(string impressora)
        {
            config = new PrinterSettings
            {
                PrinterName = impressora
            };
        }

        internal void ImprimirTexto(string texto, int wait = 1000)
        {
            if (string.IsNullOrEmpty(texto))
                return;

            Imprimir((s, e) =>
            {
                var size = e.Graphics.MeasureString(texto, fNormal, Width);
                var rc = new Rectangle(Left, 0, Width, (int)size.Height);
                e.Graphics.DrawString(texto, fNormal, Brushes.Black, rc);
            });
        }

        internal void ImprimirImagem(Stream imgBytes, int wait = 1000)
        {
            if (imgBytes == null)
                return;

            Imprimir((s, e) =>
            {
                var img = Image.FromStream(imgBytes);
                var rc = new RectangleF(Left, 0, Width, Width * img.Height / img.Width);
                e.Graphics.DrawImage(img, rc);

            }, wait);
        }

        internal async void Imprimir(PrintPageEventHandler renderCallback, int wait = 1000)
        {
            try
            {
                await Task.Delay(wait);
                await Task.Run(() =>
                {
                    using (var document = new PrintDocument()) // Unico 'PrintDocument' para o autoatendimento!
                    {
                        document.PrinterSettings = config;
                        document.PrintPage += renderCallback;
                        document.Print();
                    }
                });
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
            }
        }
    }
}