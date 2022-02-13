using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using a7D.PDV.DAL;
using a7D.PDV.BLL;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class RelatorioInventarioTest
    {
        [TestMethod]
        public void TestMethod1()
        {

            using (var dt = EntradaSaida.RelatorioInventario(5))
            {

            }
        }
    }
}
