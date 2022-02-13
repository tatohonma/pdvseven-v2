using a7D.PDV.BLL;
using a7D.PDV.BLL.Extension;
using a7D.PDV.BLL.Services;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.SAT;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestDraw
    {
        [TestMethod]
        public void ImprimeLinha()
        {
            // Lista criada dentro do caixa, ou app
            var list = new List<ItemValor>
            {
                new ItemValor("Teste", 123.45m.ToString("N2"), "dados"),
                new ItemValor("OK é uma linha longa de algo", 9345.21m.ToString("N2")),
                new ItemValor("item com crop em com duas linha sendo impresso pelo c#", 123.45m),
                new ItemValor("item com quebra em várias linha sendo impresso pelo c#", "123.45", multLines: true),
                new ItemValor("item com quebra em várias linha sendo impresso pelo c#", "123.45", "extra dados", multLines: true),
                new ItemValor("Ação !@#$% OK...", 1.ToString("N2"), "1 x 123.45"),
                new ItemValor("Sem valor"),
                new ItemValor("Multiplas linhas\n1\n2\r\n3\r\n4 foi!", multLines: true),
                new ItemValor("Nivel com multiplas linhas\n1\n2\r\n3\r\n4 foi!", nivel: 3, multLines: true),
                new ItemValor("Texto longo com dados", 4, "dados extra"),
                new ItemValor("Tabulando texto 1", nivel: 1),
                new ItemValor("Tabulando texto 2", 234.56m, nivel: 2),
                new ItemValor("Tabulando texto 3", 3 * 123.45m, "3x123.45", nivel: 3),
                new ItemValor("Tabulando max valor", 99999.99m, "ok", nivel: 4)
            };

            // processmento que ocorre na impressão
            int total = 300;
            var bmp = new Bitmap(total, 700);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            int linha = 10;
            var fnt = new Font("Arial", 10);
            foreach (var item in list)
                linha += g.DrawItemValor(item, fnt, linha, 280) + 1;

            bmp.Save("teste.png", ImageFormat.Png);
        }

        [TestMethod]
        public void ImprimeTexto()
        {
            ImpressoraWindows.ImprimirTexto("OneNote", true, "Teste\r\nde qualquer coisa\r\n1234567890123456789012345678901234567890\r\n");
        }
    }
}