using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestComanda
    {
        [TestMethod, TestCategory("Creditos")]
        public void Comanda_FechamentoDia_Fechar()
        {
            int x = 0;
            Comanda.FecharComandasContaCliente(1, 1, ref x);
        }

        [TestMethod, TestCategory("Creditos")]
        public void Comanda_Consulta_Saldo()
        {
            var comanda = Comanda.CarregarPorNumeroOuCodigo(200);
            Console.WriteLine(comanda);
            if (comanda.ValorStatusComanda != EStatusComanda.EmUso)
                Assert.Inconclusive("Comanda não está em uso");

            var pedido = Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);
            if (pedido.IDPedido == null)
                Assert.Inconclusive("Pedido não encontrado");

            Console.WriteLine(pedido);

            var saldo = Saldo.ClienteSaldoBruto(pedido.Cliente.IDCliente.Value);
            // Calculado pela tabela de saldo apenas
            Console.WriteLine("Saldo: " + saldo);
            Console.WriteLine();

            // Apenas por saldo
            var extrato1 = Saldo.ExtratoCreditos(pedido.Cliente.IDCliente.Value);
            decimal creditos1 = 0m;
            decimal debitos1 = 0m;
            int idSaldo1 = 0; // para recalcular e validar
            Console.WriteLine("Extrato de Saldo:");
            foreach (var item in extrato1)
            {
                if (item.IDSaldo != idSaldo1)
                {
                    idSaldo1 = item.IDSaldo;
                    if (item.Tipo == "C")
                        creditos1 += item.Valor;
                    else
                        debitos1 += item.Valor;
                }
                Console.WriteLine(item);
            }

            Console.WriteLine($"Movimentações: {extrato1.Length} Creditos: {creditos1} Debitos: {debitos1} Saldo: {creditos1 - debitos1}");
            Assert.AreEqual(saldo, extrato1[extrato1.Length - 1].Saldo);
            Console.WriteLine();

            // Agora o saldo por item, que faz mais sentido para o cliente
            var extrato2 = Saldo.ExtratoItens(pedido.Cliente.IDCliente.Value);
            decimal creditos2 = 0m;
            decimal debitos2 = 0m;
            Console.WriteLine("Extrato de Itens:");
            foreach (var item in extrato2)
            {
                if (item.ValorTotal > 0)
                {
                    //if (item.IDTipoProduto == (int)ETipoProduto.Credito)
                    //    creditos2 += item.Valor;
                    // Outros itens já foram pagos
                }
                else
                    debitos2 += item.Valor;

                Console.WriteLine(item);
            }

            Console.WriteLine($"Movimentações: {extrato2.Length} Creditos: {creditos2} Debitos: {debitos2} Saldo: {creditos2 - debitos2}");
            //Assert.AreEqual(saldo, extrato2[extrato2.Length - 1].Saldo);

            // Os totais por item tem que ser coperente pelo total geral
            Assert.AreEqual(creditos1, creditos2);
            Assert.AreEqual(debitos1, debitos2);
        }

        [TestMethod, TestCategory("Creditos")]
        public void Comanda_Extrato_Itens()
        {
            var comanda = Comanda.CarregarPorNumeroOuCodigo(200);
            Console.WriteLine(comanda);
            if (comanda.ValorStatusComanda != EStatusComanda.EmUso)
                Assert.Inconclusive("Comanda não está em uso");

            var pedido = Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);
            if (pedido.IDPedido == null)
                Assert.Inconclusive("Pedido não encontrado");

            Console.WriteLine(pedido);

            // Agora o saldo por item, que faz mais sentido para o cliente
            //var extratoParcial = Saldo.ExtratoItens(pedido.Cliente.IDCliente.Value, "de06dcff-1819-4624-827a-f5794b222a96");
            var extratoParcial = Saldo.ExtratoItens(pedido.Cliente.IDCliente.Value); //, pedido.GUIDAgrupamentoPedido);
            foreach (var item in extratoParcial)
                Console.WriteLine(item);
        }

        [TestMethod, TestCategory("Creditos")]
        public void ContaCliente_LiquidarDebitos()
        {
            Saldo.LiquidarDebitos(1);
        }
    }
}