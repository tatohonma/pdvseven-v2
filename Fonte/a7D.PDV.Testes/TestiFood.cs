using a7D.PDV.Integracao.iFood;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using a7D.PDV.Model;
using a7D.PDV.BLL;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestiFood
    {
        [TestMethod]
        [TestCategory("iFood")]
        public void iFood_Token_Criar()
        {
            using (var httpClient = new HttpClient())
            {
                // https://developer.ifood.com.br/docs
                var uri = new Uri("https://pos-api.ifood.com.br/oauth/token");
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", "pdvseven"), // e-POS id
                    new KeyValuePair<string, string>("client_secret", "F_Z)f4kk"), // e-POS password
                    new KeyValuePair<string, string>("grant_type", "password"), // allways 'password'
                    new KeyValuePair<string, string>("username", "POS-541073227"), // merchant's username
                    new KeyValuePair<string, string>("password", "POS-541073227") // merchant's password
                });

                var response = httpClient.PostAsync(uri, formContent).Result;
                var resposta = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(resposta);
                //{ "access_token":"bdf71943-150a-4185-b5d0-056d04b0b0fb","token_type":"bearer","expires_in":3599,"scope":"trust read write"}

            }
        }

        [TestMethod]
        [TestCategory("iFood")]
        public void iFood_Events_Poolling()
        {
            using (var httpClient = new HttpClient())
            {
                // https://developer.ifood.com.br/v1.0/reference#eventspolling
                var uri = new Uri("https://pos-api.ifood.com.br/v1.0/events%3Apolling");
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "bearer bdf71943-150a-4185-b5d0-056d04b0b0fb");

                var response = httpClient.GetAsync(uri).Result;
                var resposta = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(resposta);
                // [{"id":"42a5f90f-8c8b-4866-8508-4488a01b729e","code":"PLACED","correlationId":"6519928897755060","createdAt":"2018-04-17T14:29:47.134Z"}]
            }
        }

        [TestMethod]
        [TestCategory("iFood")]
        public void iFood_API_Fluxo()
        {
            var ifood = new APIiFood();
            var auth = ifood.Autenticar("140288", "POS-541073227", "POS-541073227");
            Console.WriteLine("autenticação: ", auth);
            Assert.AreEqual("OK", auth);

            var poll = ifood.EventoPendentes();
            if (poll == null)
                Assert.Inconclusive("Sem nada pendente");

            foreach (var p in poll)
            {
                Console.WriteLine($"{p.createdAt.ToString("dd/MM/yyyy HH:mm:ss")} {p.id} {p.code}");
                var pedido = ifood.Pedido(p.correlationId);
                Console.WriteLine(pedido.ToString());
                //ifood.Integrado(p.correlationId);
            }

            var lista = poll.Select(p => new EventoID() { id = p.id });
            //ifood.Confirmar(lista.ToArray());
        }

        [TestMethod, TestCategory("iFood")]
        public void iFood_JSON_Pedido()
        {
            var json = File.ReadAllText("../../pedidoIfood.json");
            var pedido = JsonConvert.DeserializeObject<PedidoIFood>(json);
            Console.Write(pedido);
        }

        [TestMethod, TestCategory("iFood")]
        public void iFood_JSON_DTOPedido()
        {
            var json = File.ReadAllText("../../pedidoIfood.json");
            var pedido = JsonConvert.DeserializeObject<PedidoIFood>(json);
            Console.WriteLine(pedido);

            var ifoodPDV = BLL.PDV.Listar().FirstOrDefault();
            var ifoodCaixa = Caixa.ListarAbertos().FirstOrDefault();
            var ifoodUsuario = new UsuarioInformation { IDUsuario = 1 };

            string log = DTO.CriaPedido(pedido, out int idPedido, ifoodPDV, ifoodCaixa, ifoodUsuario, 1, 1, 1, 1, 1, 1, 1);
            Console.WriteLine(log);
        }

        [TestMethod, TestCategory("iFood")]
        public void iFood_API_Produto()
        {
            var ifood = new APIiFood();
            var merchant_id = "140288";
            var id_externalCode = "6";
            var auth = ifood.Autenticar(merchant_id, "POS-541073227", "POS-541073227");
            Console.WriteLine("autenticação: ", auth);
            Assert.AreEqual("OK", auth);

            var p = new ProdutoPrecoIFood()
            {
                externalCode = id_externalCode,
                merchantIds = new int[] { int.Parse(merchant_id) },
                price = 1.24m,
                startDate = DateTime.Now.AddMinutes(1)
            };

            var result = ifood.ProdutoPreco(p);
            Console.WriteLine(result);

            result = ifood.ProdutoDisponibilidade(merchant_id, id_externalCode, false);
            Console.WriteLine(result);
        }

        [TestMethod, TestCategory("iFood")]
        public void iFood_Telefone_Valido()
        {
            var telefones = new string[] { "11 - 966072730", "0 - 11957204061", "12314-12345678", "0 - 9912341234", "0800 608 1015 ID: 49725791" };
            var resultados = new string[] { "11966072730", "11957204061", "1212345678", "0912341234", "0" };

            for (int i = 0; i < telefones.Length; i++)
            {
                var telefone = DTO.ObtemDDDTelefone(telefones[i], out int ddd, out int numero);
                Console.WriteLine($"{telefone} => DDD: {ddd} Numero: {numero}");
                Assert.AreEqual(resultados[i], telefone);
            }
        }
    }
}