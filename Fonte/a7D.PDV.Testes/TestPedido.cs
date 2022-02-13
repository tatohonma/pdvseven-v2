using a7D.PDV.BLL;
using a7D.PDV.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestPedido
    {
        [TestMethod]
        public void Pedido_Hash_Validacao()
        {
            var pedido1 = new PedidoInformation() { IDPedido = 1 };
            var pedido2 = new PedidoInformation() { IDPedido = 1 };

            var h1 = Pedido.GetHash(pedido1, out List<object> i1);
            Console.WriteLine(h1);

            var h2 = Pedido.GetHash(pedido2, out List<object> i2);
            Console.WriteLine(h2);

            Assert.AreEqual(h1, h2);

            pedido2.IDPedido = 2;

            h2 = Pedido.GetHash(pedido2, out List<object> i3);
            Console.WriteLine(h2);

            Assert.AreNotEqual(h1, h2);

            pedido2.IDPedido = 1;
            pedido2.ListaPagamento = new List<PedidoPagamentoInformation>
            {
                new PedidoPagamentoInformation() { IDPedidoPagamento = 1, Valor = 12 }
            };

            h2 = Pedido.GetHash(pedido2, out List<object> i4);
            Console.WriteLine(h2);

            Assert.AreNotEqual(h1, h2);
        }
    }
}