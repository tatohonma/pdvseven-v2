using a7D.PDV.BLL.Ativacoes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestCFe
    {
        [TestMethod, TestCategory("CFe")]
        public void CFe_Arquivo_Enviar()
        {
            var ws = new wsUtil();
            var tamanhos = new int[] {
                1000, // 1KB
                1024 * 1000, // 1 MB
                2 * 1024 * 1000, // 2 MB
                4 * 1024 * 1000, // 4 MB
                8 * 1024 * 1000, // 8 MB
                20 * 1024 * 1000 // 20 MB
            };
            foreach (var t in tamanhos)
            {
                var bt = new byte[t];
                for (int n = 0; n < t; n++)
                    bt[n] = (byte)(65 + n % 30);

                // Maximum request length exceeded: Padrão 4MB
                var result = ws.CFE("ferreira@pdvseven.com.br", "nome", "cnpj", "01/01/2000", "20/10/2018", $"arquivo{t}.zip", bt);
                Console.WriteLine($"Envio {t} bytes: {result}");
                Assert.AreEqual("OK", result);

                Thread.Sleep(1000);
            }
        }

        //[TestMethod, TestCategory("CFe")]
        //public void CFe_Email_Testar()
        //{
        //    var ws = new wsUtil();
        //    ws.Email("ferreira@pdvseven.com.br", "aaaa", "bb");
        //}
    }
}