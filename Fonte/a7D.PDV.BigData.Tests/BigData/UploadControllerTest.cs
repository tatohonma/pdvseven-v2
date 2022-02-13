using a7D.PDV.BigData.Shared.ValueObject;
using a7D.PDV.BigData.WebAPI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Http.Results;

namespace a7D.PDV.BigData.WebAPI.Tests.Controllers
{
    // TODO: Ter um banco de teste generico! 

    [TestClass]
    public class UploadControllerTest
    {
        [TestMethod(), TestCategory("BigData")]
        public void Upload_StatusSyncTest()
        {
            // Arrange
            var controller = new UploadController();
            //string chave = "001-06079-14"; // Ferreira
            //string chave = "001-63396-22"; // Ambiente DEV
            string chave = "001-59339-21"; // Ferreira 2 Casa

            // Act
            var response = controller.StatusSync(chave).Result;
            var json = response as JsonResult<bdAlteracaoInfo>;

            // Assert
            Assert.IsNotNull(json);
            var SincronismoInfo = json.Content;
            Assert.IsNotNull(SincronismoInfo.Mensagem);
            Console.WriteLine(SincronismoInfo.Mensagem);

            Assert.IsTrue(SincronismoInfo.Mensagem.StartsWith("OK"));

            // Parte 2
            /*
            var pedidos = EF.Repositorio.ListarConfig<tbPedido>(
                tb => tb.Include(nameof(tbPedido.tbPedidoProdutoes))
                        .Include(nameof(tbPedido.tbPedidoPagamentoes)),
                p => p.IDStatusPedido == 40
                 && (p.DtPedidoFechamento > SincronismoInfo.Pedido || !SincronismoInfo.Pedido.HasValue))
                .OrderBy(p => p.DtPedidoFechamento)
                .Select(p => p.ToBigData());

            int result = 0;
            int qtd = pedidos.Count();
            for (int i = 0; i < qtd; i += 50)
            {
                var up = pedidos
                    .Skip(i)
                    .Take(50)
                    .ToArray();

                response = controller.UploadPedido(chave, up).Result;
                if (!(response is JsonResult<int> json2))
                {
                    if (response is ExceptionResult jsonEX)
                        throw jsonEX.Exception;

                    Assert.Inconclusive("Sei la!");
                }
                Console.WriteLine($"Pedido {i + result}/{qtd}");
            
    */
        }
    }
}
