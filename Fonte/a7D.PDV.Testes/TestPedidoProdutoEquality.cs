using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using System.Collections.Generic;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestPedidoProdutoEquality
    {
        [TestMethod]
        public void PedidosProdutoSaoIGuais()
        {
            var pedidoProduto1 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 1,
                Cancelado = false
            };

            var pedidoProduto2 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 1,
                Cancelado = false
            };

            Assert.IsTrue(new PedidoProdutoEqualityComparer().Equals(pedidoProduto1, pedidoProduto2));

            pedidoProduto1 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 1,
                Cancelado = false,
                ListaModificacao = new List<PedidoProdutoInformation> {
                    new PedidoProdutoInformation
                    {
                        Produto = new ProdutoInformation { IDProduto = 31 },
                        Quantidade = 1,
                        Cancelado = false,
                    },
                    new PedidoProdutoInformation
                    {
                        Produto = new ProdutoInformation { IDProduto = 30 },
                        Quantidade = 1,
                        Cancelado = false,
                    }
                }
            };

            pedidoProduto2 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 1,
                Cancelado = false,
                ListaModificacao = new List<PedidoProdutoInformation> {
                    new PedidoProdutoInformation
                    {
                        Produto = new ProdutoInformation { IDProduto = 31 },
                        Quantidade = 1,
                        Cancelado = false,
                    },
                    new PedidoProdutoInformation
                    {
                        Produto = new ProdutoInformation { IDProduto = 30 },
                        Quantidade = 1,
                        Cancelado = false,
                    }
                }
            };

            Assert.IsTrue(new PedidoProdutoEqualityComparer().Equals(pedidoProduto1, pedidoProduto2));

            pedidoProduto1 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 1,
                Cancelado = false,
                ListaModificacao = new List<PedidoProdutoInformation>()
            };

            pedidoProduto2 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 1,
                Cancelado = false
            };

            Assert.IsTrue(new PedidoProdutoEqualityComparer().Equals(pedidoProduto1, pedidoProduto2));
        }

        //[TestMethod]
        //public void TesteInconclusivo()
        //{
        //    Console.WriteLine("Escrevendo no console 444");
        //    Console.WriteLine("Escrevendo no console 555");
        //    Assert.Inconclusive("Mensagem de teste inconclusivo");
        //}

        //[TestMethod]
        //public void TesteErro()
        //{
        //    Console.WriteLine("Escrevendo no console 2222");
        //    Assert.Fail("Teste de erro");
        //}

        //[TestMethod]
        //public void TesteCorreto()
        //{
        //    Console.WriteLine("Escrevendo no console");
        //}

        [TestMethod]
        public void PedidosProdutosSaoDiferentes()
        {
            var pedidoProduto1 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 2,
                Cancelado = false
            };

            var pedidoProduto2 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 1,
                Cancelado = false
            };

            Assert.IsFalse(new PedidoProdutoEqualityComparer().Equals(pedidoProduto1, pedidoProduto2));

            pedidoProduto1 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 51 },
                Quantidade = 1,
                Cancelado = false
            };

            pedidoProduto2 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 1,
                Cancelado = false
            };

            Assert.IsFalse(new PedidoProdutoEqualityComparer().Equals(pedidoProduto1, pedidoProduto2));

            pedidoProduto1 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 1,
                Cancelado = false,
                ListaModificacao = new List<PedidoProdutoInformation> {
                    new PedidoProdutoInformation
                    {
                        Produto = new ProdutoInformation { IDProduto = 31 },
                        Quantidade = 1,
                        Cancelado = false,
                    },
                    new PedidoProdutoInformation
                    {
                        Produto = new ProdutoInformation { IDProduto = 30 },
                        Quantidade = 1,
                        Cancelado = false,
                    }
                }
            };

            pedidoProduto2 = new PedidoProdutoInformation
            {
                Produto = new ProdutoInformation { IDProduto = 50 },
                Quantidade = 1,
                Cancelado = false
            };

            Assert.IsFalse(new PedidoProdutoEqualityComparer().Equals(pedidoProduto1, pedidoProduto2));
        }
    }
}
