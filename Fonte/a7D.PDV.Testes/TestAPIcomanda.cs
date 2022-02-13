using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Integracao.API2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace a7D.PDV.Testes
{
    // Usar: Newtonsoft.Json 10.0.3

    [TestClass]
    public class TestAPIcomanda
    {
        ClienteWS ws;

        [TestInitialize]
        public void Start()
        {
            //ws = new ClienteWS("https://testepdvseven.azurewebsites.net/"); // Endereço do servidor
            ws = new ClienteWS("http://localhost:7777/"); // Endereço do servidor
        }

        [TestMethod, TestCategory("API")]
        public void Comanda_Total_Informacao()
        {
            var apiPedido = ws.Pedido(); // Somente os metodos relacionado a pedidos
            // Comandas disponive: 201 a 210
            var comanda = apiPedido.ComandaStatus(218); // O segundo parametro informa se é para retornar nulo quando houver um erro de não existe (404)
            // Status 10 - Livre - não está aberta
            // Status 20 - Em uso - Comanda aberta, pode usar
            // Outros Status não usar!
            Console.WriteLine(comanda);

            var comanda2 = apiPedido.ComandaSaldo(218);
            Console.WriteLine(comanda2);
        }

        [TestMethod, TestCategory("API")]
        public void Produtos_Lista_Tudo()
        {
            var apiProdutos = ws.Produto();
            var produtos = apiProdutos.ListaProdutos();
            foreach (var prod in produtos)
                Console.WriteLine(prod);
        }

        [TestMethod, TestCategory("API")]
        public void Pedido_Adicionar_Produto()
        {
            var apiPedido = ws.Pedido();
            var dados = new AdicionarProdutos()
            {
                GerarOrdemProducao = false, // Se é para imprimir informação de produção
                GUIDSolicitacao = Guid.NewGuid().ToString(), // identificção unica da chamada paraevitar duplicidade
                IDPDV = 130, // Numero do PDV cadastrado no sistema
                IDUsuario = 1, // Usuário a ser registrado como autor da inclusão
                IDTipoPedido = 20, // 10-Mesa, 20-Comanda
                //Numero = "201", // Numero to dipo em questão: Numedo da Mesa, ou numero da comanda
                //Numero = 2863315899,
                //Numero = 0xAAAABBBB,
                Numero = "1114",
                // Numero = 2863315899,
                //ValidarLimite = false,
                Itens = new List<Item>()
                {
                    new Item()
                    {
                        IDProduto = 29, // Código do produti
                        Qtd = 0.123m, // Quantidade, pode ser parcial
                        Preco = 37.13m // Valor atual
                    }
                }
            };
            var resut = apiPedido.AdicionaPedido(dados);
            Console.WriteLine(resut);
            Assert.AreEqual(resut.Mensagem, "OK");
        }

        [TestMethod, TestCategory("API")]
        public void Comanda_Listar_Produto()
        {
            var apiPedido = ws.Pedido();
            var pedido = apiPedido.ComandaItens(201);

            foreach (var item in pedido.Itens)
                Console.WriteLine(item);

            Console.WriteLine($"Serviço: R$ {pedido.ValorServico?.ToString("N2")}");
            Console.WriteLine($"Total: R$ {pedido.ValorTotal?.ToString("N2")}");
            Console.WriteLine($"Cliente: R$ {pedido.Cliente?.NomeCompleto}");
        }

        [TestMethod, TestCategory("API")]
        public void Cliente_Cadastrar_Novo()
        {
            var apiCliente = ws.Cliente();
            var result = apiCliente.InserirCliente(new Cliente() {
                Documento1="11122233344",
                NomeCompleto="Teste 123",
            });
        }
    }
}