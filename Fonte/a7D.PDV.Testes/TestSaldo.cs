using a7D.PDV.EF;
using a7D.PDV.EF.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestSaldo
    {
        [TestMethod]
        public void Saldo_Validacao_Integridade()
        {
            /*
             
SELECT c.idSaldo, c.idCliente, c.dtMovimento, c.valor, 
	(SELECT SUM(valor) FROM tbsaldo d WHERE d.tipo='D' AND d.IDPai=c.idSaldo) saldo
FROM tbsaldo c WHERE c.tipo='C'

SELECT p.IDTipoProduto, SUM(pp.ValorUnitario) Total FROM tbPedidoProduto pp inner join tbProduto p ON pp.IDProduto=p.IDProduto GROUP BY p.IDTipoProduto;

SELECT SUM(Valor) TotalPagamentos FROM tbPedidoPagamento WHERE Excluido=0;

SELECT SUM(ValorTotal) TotalPedidos, sum(ValorDesconto) TotalDescontos  FROM tbPedido WHERE IDStatusPedido<>50;

SELECT * FROM tbSaldo s INNER JOIN tbPedido p ON s.IDPedido=p.IDPedido;

SELECT * FROM tbPedidoPagamento;
SELECT * FROM tbPedido;
SELECT * FROM tbSaldo;             
             */

            using (var pdv = new pdv7Context())
            {
                // O Total de Creditos disponibilizado na tabela de saldos deve ser igual ao total de creditos vendidos
                var creditosTotal = pdv.Database.SqlQuery<decimal>("SELECT SUM(valor) FROM tbSaldo WHERE tipo='C'").First();
                Console.WriteLine("Valor Total em Creditos: " + creditosTotal.ToString("C"));

                var creditosVendidos = pdv.Database.SqlQuery<decimal>("SELECT SUM(pp.ValorUnitario) FROM tbPedidoProduto pp inner join tbProduto p ON pp.IDProduto=p.IDProduto WHERE p.IDTipoProduto=50;").First();
                Console.WriteLine("Valor Creditos Vendidos: " + creditosVendidos.ToString("C"));

                Assert.AreEqual(creditosTotal, creditosVendidos);
                Console.WriteLine();

                // O quanto foi pago em dinheiro deve bater com o total de pedidos
                var pagamentosTotal = pdv.Database.SqlQuery<decimal>("SELECT SUM(Valor) TotalPagamentos FROM tbPedidoPagamento WHERE Excluido=0").First();
                Console.WriteLine("Valor Total Pagamentos: " + pagamentosTotal.ToString("C"));

                var pedidosTotal = pdv.Database.SqlQuery<decimal>("SELECT SUM(ValorTotal) TotalPedidos FROM tbPedido WHERE IDStatusPedido<>50").First();
                Console.WriteLine("Valor Total Pedidos: " + pedidosTotal.ToString("C"));

                Assert.AreEqual(pedidosTotal, pagamentosTotal);


            }
        }
    }
}