using a7D.PDV.BLL;
using a7D.PDV.Integracao.Pagamento;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestErroCodes
    {
        [TestMethod]
        public void Numeros()
        {
            // GeraErroCores códigos de erros "aleatórios"
            var rnd = new Random(DateTime.Now.Millisecond);
            for (int a = 0; a < 10; a++)
                Console.WriteLine(rnd.Next(0xA000, 0xFFFF).ToString("X4"));
        }

        [TestMethod]
        public void GeraTextoErro()
        {
            Type tp = typeof(CodigoErro);
            Console.WriteLine("Teste: " + ExceptionPDV.Description("xxx"));
            foreach (var fi in tp.GetFields())
            {
                Console.WriteLine(fi.Name + "|" + ExceptionPDV.Description(fi.Name));
            }
        }

        [TestMethod]
        public void TesteAcentos()
        {
            string teste1 = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            Console.WriteLine(teste1);
            string teste2 = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
            Console.WriteLine(teste2);

            teste1 = teste1.RemoveAcentos();
            Assert.IsTrue(teste1 == teste2, "Erro na remoção de acentos");
        }
    }
}