using a7D.PDV.Integracao.ERPCake;
using a7D.PDV.Integracao.ERPCake.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using a7D.PDV.Integracao.ERPCake.Sync;
using a7D.PDV.EF.Models;
using a7D.PDV.BLL;
using System.Threading.Tasks;

namespace a7D.PDV.Testes
{
    [TestClass]
    public class TestERPCake
    {
        #region API Direta

        APIERPCake api;
        HttpClient httpClient;

        [TestInitialize]
        public void Start()
        {
            string token = "6819e3c368c7d4dee00d"; // DEV!!!

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("X-cake-token", token);

            api = new APIERPCake(token);
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Customer_HardwareSet()
        {
            var clientePadrao = api.GetFirst<Customer>("code=PDV7");
            //clientePadrao.address_street = "3C313F5A";
            var result = api.Save(clientePadrao);
            Console.WriteLine(result);

        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERP_Customer_All()
        {
            var uri = new Uri("https://app.cakeerp.com/api/customer/all");
            var response = httpClient.GetAsync(uri).Result;
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            var resposta = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(resposta);
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERP_List_All()
        {
            var objects = new string[] {
                //"customer",
                //"product",
                //"payment_form",
                //"product_category",
                //"sales_order",
                //"sales_order_item",
                //"sales_order_parcel"
                "cashflow"
            };

            foreach (var obj in objects)
            {
                Console.WriteLine(obj);
                var uri = new Uri($"https://app.cakeerp.com/api/{obj}/all");
                var response = httpClient.GetAsync(uri).Result;
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
                var resposta = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(resposta);
                Console.WriteLine();
            }
        }

        #endregion

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Customer_All()
        {
            bool erros = false;
            var clientes = api.All<Customer>();
            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);
            foreach (var cliente in clientes)
            {
                Console.WriteLine(cliente);
                cliente.name += " (6)";
                try
                {
                    var result = api.Save(cliente);
                    Console.WriteLine("OK, Alterado com sucesso: " + result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    erros = true;
                }
                Console.WriteLine(api.LastRequest);
                Console.WriteLine(api.LastResult);
            }
            Assert.IsFalse(erros);
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Customer_Insert()
        {
            var novo = new Customer()
            {
                name = "Insert TDD",
                BirthdayConvert = DateTime.Now.AddYears(-20)
            };
            var result = api.Save(novo);
            Console.WriteLine(result);
            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Product_Insert()
        {
            var novo = new Product()
            {
                name = "Insert TDD",
                price_sell = 23.45m
            };
            var result = api.Save(novo);
            Console.WriteLine(result);
            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Product_All()
        {
            int n = 0;
            var produtos = api.All<Product>(0, 1000);
            string lastProd = null;
            foreach (var produto in produtos)
            {
                if (!string.IsNullOrEmpty(produto.code)
                 && !int.TryParse(produto.code, out int id))
                {
                    Console.WriteLine($"{++n}: {produto}");
                    produto.code = "";
                    api.Save(produto);
                }
                if (lastProd != produto.name)
                    lastProd = produto.name;
                else
                {
                    //api.Delete(produto);
                    Console.WriteLine($"{++n}: APAGADO {produto}");
                }
            }
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Order_All()
        {
            var orders = api.All<Sales_Order>();
            foreach (var order in orders)
                Console.WriteLine(order);
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Order_BillOrder()
        {
            try
            {
                //var order = api.GetByID<Sales_Order>(1835221); // 59930
                var order = api.GetFirst<Sales_Order>("order_number=59926"); // 59930
                if (order == null)
                    Assert.Inconclusive("Pedido não encontrado");

                Console.WriteLine(order);
                var customer = api.GetByID<Customer>(order.customer);
                Console.WriteLine(customer);

                if (order.order_type != 1)
                    Assert.Inconclusive("O pedido não está no status correto");

                order.invoice_model = 59; //SAT
                api.Save(order);

                var result = api.BillOrder(order.id.Value);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(api.LastRequest);
                Console.WriteLine(api.LastResult);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Order_AFaturar()
        {
            var orders = api.All<Sales_Order>(where: "order_type=1"); // 59930
            foreach (var order in orders)
            {
                order.invoice_model = 65; //Cupom
                api.Save(order);
                Console.WriteLine("Faturando: " + order);
                try
                {
                    api.BillOrder(order.id.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\tErro ao Faturar: " + ex.Message);
                }
            }
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Dump()
        {
            var item = api.GetByID<Sales_Order>(1832508);
            Console.WriteLine(item);
            Console.WriteLine(api.LastResult);
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Order_New()
        {
            var order = new Sales_Order()
            {
                order_number = 5001,
                order_type = 1,
                customer = 2068398,
                seller = 6316,
                Date_OrderConvert = DateTime.Now.AddDays(-5),
                Delivery_TimeConvert = DateTime.Now.AddDays(5)
            };
            var result1 = api.Save(order);
            Console.WriteLine(result1);
            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);
            Assert.IsNotNull(result1);

            var item = new Sales_Order_Item()
            {
                sales_order_id = result1.id.Value,
                product_id = 5484290,
                item_name = "Insert TDD (X)",
                qtd = 1,
                price_sell = 12.34m,
            };
            item.total = item.qtd * item.price_sell;

            var result2 = api.Save(item);
            Console.WriteLine(result2);
            Console.WriteLine(api.LastRequest);
            Console.WriteLine(api.LastResult);
            Assert.IsNotNull(result2);

        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Sync()
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    var dt = DateTime.Now;

                    var erp = new MockCake(new DateTime(2018, 1, 1));

                    erp.Sincronizar<tbProduto, Product>(true);
                    //erp.Sincronizar<tbUsuario, Seller>(true);
                    erp.Sincronizar<tbTipoPagamento, Payment_Form>(true);
                    //erp.Sincronizar<tbCliente, Customer>(true);
                    erp.SincronizarPedidos();

                    erp.UpdateSync(dt);
                }
            }
            catch (Exception ex)
            {
                AC.RegitraLicenca("TDD Publicado");
                AC.RegitraPDV("TDD", "1.0.0.1", new Model.PDVInformation() { IDPDV = 1, Nome = "teste" });
                AC.RegitraUsuario(new Model.UsuarioInformation() { IDUsuario = 1, Nome = "teste" });
                var exPDV = new ExceptionPDV(CodigoErro.EE01, ex, "TDD Erro");
                Task.WaitAll(exPDV.SendAsync());
            }
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_ClientePadrao()
        {
            var result = api.GetFirst<Customer>("code=PDV7");
            Console.WriteLine(result);
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_CashFlow()
        {
            var result = api.All<CashFlow>(where: "incoming=0");
            //Console.WriteLine(result);

            var despesa = new CashFlow()
            {
                registered_date = DateTime.Now,
                DueDateConvert = DateTime.Now,
                incoming = false,
                amount = 90,
                amount_total = 80,
                received = true,
                DateReceivedConvert = DateTime.Now,
                discount = 10,
                customer = 2515921,
                //category = 118037,
                description = "teste API",
                bank_account = 6307, // Referencia a conta no banco da conta cliente
            };

            var result1 = api.Save(result[0]);

            var result2 = api.Save(despesa);
        }

        [TestMethod]
        [TestCategory("ERP Cake")]
        public void ERPAPI_Bank()
        {
            var result = api.All<Bank>();

            var banco = new Bank()
            {
                name = "Nova Conta API",
                base_bank = 26, // Outros Bancos
                agency = 123,
                account = 45678,
            };

            var result1 = api.Save(result[0]);

            var result2 = api.Save(banco);
        }
    }
}