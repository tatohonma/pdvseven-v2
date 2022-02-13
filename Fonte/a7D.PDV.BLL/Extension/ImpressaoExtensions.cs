using a7D.PDV.BLL.ValueObject;
using System.Drawing;
using System.Text;

namespace a7D.PDV.BLL.Extension
{
    public static class ImpressaoExtensions
    {
        private static Brush black = Brushes.Black;
        private const int espacamento = 10;

        public static int DrawItemValor(this Graphics g, ItemValor itemValor, Font font, int linha, int totalWidth)
        {
            int offSetWidth = itemValor.Nivel * espacamento;
            int itemWidth = totalWidth - itemValor.ValorWidth;

            // Obtem o tamanho dos dados tem que ser algo pequeno sempre, no maximo o metade do espaço!!!
            int dadosWidth = 0;
            if (!string.IsNullOrEmpty(itemValor.Dados))
            {
                dadosWidth = (int)g.MeasureString(itemValor.Dados, font, itemWidth / 2).Width + espacamento; // espaçamento necessáro para plotagem correta
                itemWidth -= dadosWidth; // vai avaça um pouco mas não tem problema
            }

            // Obtem o tamanho da linha
            int height = (int)g.MeasureString(itemValor.MultiLines ? itemValor.Item : "A", font, itemWidth - offSetWidth).Height;
            var rcItem = new Rectangle(offSetWidth, linha, itemWidth - offSetWidth, height);
            //g.FillRectangle(Brushes.Yellow, rcItem);
            g.DrawString(itemValor.Item, font, black, rcItem);

            if (!string.IsNullOrEmpty(itemValor.Dados))
            {
                var rcDados = new Rectangle(itemWidth + espacamento, linha, dadosWidth, height);
                //g.FillRectangle(Brushes.LightPink, rcDados);
                g.DrawString(itemValor.Dados, font, black, rcDados);
            }

            if (!string.IsNullOrEmpty(itemValor.Valor))
            {
                var rcValor = new Rectangle(itemWidth + dadosWidth, linha, itemValor.ValorWidth, height);
                //g.FillRectangle(Brushes.LightSkyBlue, rcValor);
                g.DrawString(itemValor.Valor, font, black, rcValor, new StringFormat() { Alignment = StringAlignment.Far });
            }

            return height;
        }

        public static int DrawCenter(this Graphics g, Font font, string text, int y, int totalWidth)
        {
            int height = (int)g.MeasureString(text, font, totalWidth).Height;
            var rcItem = new Rectangle(0, y, totalWidth, height);
            //g.FillRectangle(Brushes.Yellow, rcItem);
            g.DrawString(text, font, black, rcItem, new StringFormat() { Alignment = StringAlignment.Center });

            return height;
        }

        public static int DrawText(this Graphics g, string item1, string item2, Font font, int linha, int totalWidth)
        {
            int ItemWidth = item2 == null ? totalWidth : totalWidth / 2;
            int height = (int)g.MeasureString(item1, font, ItemWidth).Height;
            if(item2 != null)
            {
                int height2 = (int)g.MeasureString(item2, font, ItemWidth).Height;
                if (height2 > height)
                    height = height2;
            }

            //g.FillRectangle(Brushes.Yellow, new Rectangle(0, linha, ItemWidth, height));
            g.DrawString(item1, font, Brushes.Black, new Rectangle(0, linha, ItemWidth, height));

            if (!string.IsNullOrEmpty(item2))
                g.DrawString(item2, font, Brushes.Black, new Rectangle(ItemWidth, linha, ItemWidth, height), new StringFormat() { Alignment = StringAlignment.Far });

            return height;
        }

        public static int DrawSeparador(this Graphics g, int linha, int totalWidth)
        {
            g.DrawLine(Pens.Black, 0, linha, totalWidth, linha);
            return 4;
        }
  
        public static string SplitToLines2(this string text, char[] splitOnCharacters, int maxStringLength)
        {
            var sb = new StringBuilder();
            var index = 0;

            while (text.Length > index)
            {
                // start a new line, unless we've just started
                if (index != 0)
                    sb.AppendLine();

                // get the next substring, else the rest of the string if remainder is shorter than 'maxStringLength'
                var splitAt = index + maxStringLength <= text.Length
                    ? text.Substring(index, maxStringLength).LastIndexOfAny(splitOnCharacters)
                    : text.Length - index;

                // if can't find split location, take 'maxStringLength` character'
                splitAt = (splitAt == -1) ? maxStringLength : splitAt;

                // add result to collection & increment index
                sb.Append(text.Substring(index, splitAt).Trim());
                index += splitAt;
            }

            return sb.ToString();
        }
    }
}
