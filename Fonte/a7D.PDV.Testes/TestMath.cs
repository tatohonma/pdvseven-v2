using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestMath
    {
        public static decimal MultiplicaQtdValor(decimal qtd, decimal valor) => Math.Truncate(qtd * valor * 100) / 100;

        public static string ExibePessobilidades(decimal valor) => $"{valor} => Padrão: {valor.ToString("0.00")}   Round2ToEven: {Math.Round(valor, 2, MidpointRounding.ToEven)}   Round2AwayFromZero: {Math.Round(valor, 2, MidpointRounding.AwayFromZero)}";

        [TestMethod, TestCategory("Math")]
        public void Test_Math_QtdValor()
        {
            Math.Round(1.34, 2); //Não usar arredondamentos no sistema!!!

            decimal v = MultiplicaQtdValor(1m, 1m);
            Console.WriteLine(v);
            Assert.AreEqual(v, 1m);

            v = MultiplicaQtdValor(0.52m, 321.53m);
            Console.WriteLine(v);
            Assert.AreEqual(v, 167.2m);

            v = MultiplicaQtdValor(0.52m, 121.6m);
            Console.WriteLine(v);
            Assert.AreEqual(v, 63.23m);

            Console.WriteLine(ExibePessobilidades(123.462m));
            Console.WriteLine(ExibePessobilidades(123.461m));
            Console.WriteLine(ExibePessobilidades(123.460m));
            Console.WriteLine(ExibePessobilidades(123.459m));
            Console.WriteLine(ExibePessobilidades(123.458m));
            Console.WriteLine(ExibePessobilidades(123.457m));
            Console.WriteLine(ExibePessobilidades(123.456m));
            Console.WriteLine(ExibePessobilidades(123.455m));
            Console.WriteLine(ExibePessobilidades(123.454m));
            Console.WriteLine(ExibePessobilidades(123.453m));
            Console.WriteLine(ExibePessobilidades(123.452m));
            Console.WriteLine(ExibePessobilidades(123.451m));
            Console.WriteLine(ExibePessobilidades(123.450m));
            Console.WriteLine(ExibePessobilidades(123.449m));
            Console.WriteLine(ExibePessobilidades(123.448m));

        }

        [TestMethod, TestCategory("Math")]
        public void Test_Math_Dizimas()
        {
            for (int i = 1750; i < 1850; i++)
            {
                var v1 = i / 10f;
                var v2 = v1 * 100;
                Console.WriteLine($"{v1} => {v2}");
            }
            Console.WriteLine("FIM");
        }
    }
}