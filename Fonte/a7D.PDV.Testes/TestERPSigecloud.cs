using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Text;
using a7D.PDV.Integracao.ERPSige.Model;
using a7D.PDV.Integracao.ERPSige;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestERPSigecloud
    {
        HttpClient httpClient;

        [TestInitialize]
        public void Start()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization-Token", "2298b11cdc351b0c9235e1263e4c380e6be376bd34be05634a1de93574ba34d46dc9a3a83796954a5a8634fc68b4ed664f15e61dfe6c1c63e1533c712b7f3741fa823b2ce1f482061525ac3732de3b8caeb431de1887886e370a96e04eaf221694d7f9c5835eb3a7f8a49b7326072904344b291067cec2ce77889d332822ddeb");
            httpClient.DefaultRequestHeaders.Add("User", "fabio@pdvseven.com.br");
            httpClient.DefaultRequestHeaders.Add("App", "API");
        }

        [TestMethod]
        [TestCategory("ERP Sige")]
        public void ERP_Listas_API()
        {
            var api = new APIERP();
            var a = api.PessoaListar();
            var b = api.ProdutoListar();
            var c = api.PedidoListar();
        }

        #region Produto

        [TestMethod]
        [TestCategory("ERP Sige")]
        public void ERP_Produto_Listar()
        {
            var uri = new Uri("http://pdvseven.vendaerp.com.br/api/request/produtos/getall?skip=0");
            var response = httpClient.GetAsync(uri).Result;
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            var resposta = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(resposta);
        }

        [TestMethod]
        [TestCategory("ERP Sige")]
        public void ERP_Produto_Carregar()
        {
            var uri = new Uri("http://pdvseven.vendaerp.com.br/api/request/produtos/get?codigo=36");
            var response = httpClient.GetAsync(uri).Result;
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            var resposta = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(resposta);
        }

        [TestMethod]
        [TestCategory("ERP Sige")]
        public void ERP_Produto_Criar()
        {
            var prod = new Produto()
            {
                Codigo = "2222",
                Categoria = "333",
                Marca = "444",
                Fornecedor = "555 LTDA",
                Pratileira = "",
                NumeroSerie = "2233",
                Nome = "xxxxx",
                Genero = "01",
                EstoqueUnidade = "UN",
                Especificacao = "",
                PesoKG = 0.1m,
                PrecoCusto = 45.00m,
                LucroDinheiro = "9.00",
                LucroPercentual = "0.2",
                PrecoVenda = 54.00m,
                PrecoMinimoVenda = 52.00m,
                EstoqueRisco = 0.0m,
                DepositoPadrao = null,
                EstoqueSaldo = 52.0m,
                NCM = "6106.10.00",
                GrupoTributario = "Comercializacao",
                GeneroFiscal = "",
                CFOPPadrao = "5102",
                UnidadeTributavel = "UN",
                OrigemMercadoria = "00",
                VisivelSite = false,
                DestaqueSite = false,
                IgnorarEstoque = true,
                FreteGratis = false,
                Ativo = true,
                FiltrosCategoria = " ",
                PercentualDescontoBoleto = 0.05m,
                Comprimento = null,
                Altura = null,
                Largura = null
            };

            var api = new APIERP();
            api.ProdutoSalvar(prod);
        }

        #endregion

        #region Pessoa

        [TestMethod]
        [TestCategory("ERP Sige")]
        public void ERP_Pessoa_Listar()
        {
            var uri = new Uri("https://pdvseven.vendaerp.com.br/api/request/pessoas/getall?skip=0");
            var response = httpClient.GetAsync(uri).Result;
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            var resposta = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(resposta);
        }

        [TestMethod]
        [TestCategory("ERP Sige")]
        public void ERP_Pessoa_Criar()
        {
            var uri = new Uri("https://pdvseven.vendaerp.com.br/api/request/pessoas/salvar");
            var json = @"{
""ID"":""526fa0c9e955158bb0325578"",
""PessoaFisica"":false,
""NomeFantasia"":""Eletro Peças Vargas"",
""RazaoSocial"":""Arinda Borges Vargas Me"",
""CNPJ_CPF"":""1468601000196"",
""RG"":"""",
""IE"":""85766406"",
""Logradouro"":""RUA BUARQUE DE NAZARETH"",
""LogradouroNumero"":""434"",
""Complemento"":"""",
""Bairro"":"""",
""Cidade"":""ITAPERUNA"",
""CodigoMunicipio"":""3302205"",
""Pais"":""BR"",
""CodigoPais"":""1058"",
""CEP"":""28300000"",
""UF"":""RJ"",
""CodigoUF"":"" "", 
""Telefone"":""(22) 3824-3827"", 
""Celular"":null,
""Email"":""arindabvargas@yahoo.com.br"",
""Site"":"" "",
""Cliente"":true,
""Tecnico"":false,
""Vendedor"":false,
""Transportadora"":false,
""Fonecedor"":false,
""Representada"":false,
""Ramo"":""Nenhum"",
""VendedorPadrao"":"" "",
""NomePai"":"" "",
""NomeMae"":"" "",
""Naturalidade"":"" "",
""ValorMinimoCompra"":0.0,
""DataNascimento"":""0001-01-01T00:00:00-02:00""
}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(uri, content).Result;

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            var resposta = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(resposta);
        }

        [TestMethod]
        [TestCategory("ERP Sige")]
        public void ERP_Pessoa_Criar2()
        {
            var pessoa = new Pessoa()
            {
                ID = "526fa0c9e955158bb0325578",
                NomeFantasia = "Eu Fabio F Souza",
                CNPJ_CPF = "19221149870",
            };

            var api = new APIERP();
            api.PessoaSalvar(pessoa);
        }

        #endregion

        #region Venda

        [TestMethod]
        [TestCategory("ERP Sige")]
        public void ERP_Pedido_Listar()
        {
            var uri = new Uri("http://pdvseven.vendaerp.com.br/api/request/pedidos/gettodospedidos?page=0");
            var response = httpClient.GetAsync(uri).Result;
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            var resposta = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(resposta);
        }

        [TestMethod]
        [TestCategory("ERP")]
        public void ERP_Pedido_Carregar()
        {
            var uri = new Uri("http://pdvseven.vendaerp.com.br/api/request/pedidos/pesquisar?codigo=63238&origem=&status=&categoria=&cliente=&pageSize=10&skip=0");
            var response = httpClient.GetAsync(uri).Result;
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            var resposta = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(resposta);
        }

        [TestMethod]
        [TestCategory("ERP")]
        public void ERP_Pedido_Criar()
        {
            var pedido = new Pedido()
            {
                Codigo = 333,
                OrigemVenda = "Teste",
                Cliente = "30752622854",
                Empresa = "Restaurante",
                Deposito = "PADRÃO",
                Validade = DateTime.Now,
                Items = new PedidoItem[]
                {
                    new PedidoItem () {
                        Codigo="4",
                        Descricao="Validade",
                        Quantidade=2.0m,
                        ValorUnitario=3m,
                        ValorTotal=6m
                    }
                }
            };

            var api = new APIERP();
            var result = api.PedidoSalvar(pedido);
            Console.WriteLine(result);
        }

        #endregion
    }
}