using a7D.PDV.Integracao.EasyChopp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace a7D.PDV.Testes
{
    // https://msdn.microsoft.com/pt-br/magazine/dn818493.aspx?f=255&MSPPError=-2147217396

    [TestClass]
    public class TestEasyChopp
    {
        private string documento = "44915687074";

        [TestInitialize]
        public void Configure()
        {
            EasyChoppServices.ConfigEasyChoppServices(
                "http://apptst.easychopp.com.br/api", 
                "EAS9876CHO0040", 
                "fabio@pdvseven.com.br");
        }

        [TestMethod]
        [TestCategory("EasyChopp")]
        public void EasyChopp_API_GetCliente()
        {
            var cliente = EasyChoppServices.GetClienteDocumento(documento);
            Console.WriteLine(cliente);
        }

        [TestMethod]
        [TestCategory("EasyChopp")]
        public void EasyChopp_API_GetAllClientes()
        {
            var clientes = EasyChoppServices.GetClientes(DateTime.Now.AddMonths(-1), out string result);
            if (result != null)
                Assert.Fail(result);

            foreach (var cliente in clientes)
                Console.WriteLine(cliente);
        }

        [TestMethod]
        [TestCategory("EasyChopp")]
        public void EasyChopp_API_AddCliente()
        {
            var cliente = EasyChoppServices.AddClienteDocumento(documento, "Teste Ferreira", "mpc.fabio@gmail.com", "M", new DateTime(1977, 6, 26));
            Console.WriteLine(cliente);
        }

        [TestMethod]
        [TestCategory("EasyChopp")]
        public void EasyChopp_API_RelacionaTAG()
        {
            var result = EasyChoppServices.DefineClienteTAG(documento, "aaDDEEaa");
            Console.WriteLine(result);
        }

        [TestMethod]
        [TestCategory("EasyChopp")]
        public void EasyChopp_API_RemoveTAG()
        {
            var result = EasyChoppServices.RemoveClienteTAG(documento, "CCDDEEaa");
            Console.WriteLine(result);
        }

        [TestMethod]
        [TestCategory("EasyChopp")]
        public void EasyChopp_API_GetSaldo()
        {
            Task.Run(async () =>
            {
                var cliente = await EasyChoppServices.GetCreditoDocumento(documento);
                Console.WriteLine(cliente);

            }).GetAwaiter().GetResult();
        }

        //[TestMethod]
        //[TestCategory("EasyChopp")]
        //public void EasyChopp_API_GetSaldo2()
        //{
        //    var cliente = EasyChoppServices.GetCredito2(documento);
        //    Console.WriteLine(cliente);
        //}

        [TestMethod]
        [TestCategory("EasyChopp")]
        public void EasyChopp_API_AddCredito()
        {
            var cliente = EasyChoppServices.AddCreditoDocumento(documento, FormaPagamento.Dinheiro, 20m, "teste", "abc34d");
            Console.WriteLine(cliente);
        }
    }
}