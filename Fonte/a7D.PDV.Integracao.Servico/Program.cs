using a7D.PDV.Integracao.Servico.MyFinance;
using a7D.PDV.Integracao.Servico.MyFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            //ServiceBase.Run(ServicesToRun);

            Service1 service = new Service1();

            //var pedido = new WS2.Models.Pedido
            //{
            //    IDPedido = 2,
            //    DtPedidoFechamento = DateTime.Now,
            //    Pagamentos = new List<WS2.Models.Pagamento> { { new WS2.Models.Pagamento { ID = 3 } } }
            //};
            

            //var recebivel = new Recebivel
            //{
            //    Description = "teste",
            //    Issuer = "",
            //    NominalAmount = 100,
            //    OcurredAt = DateTime.Now,
            //    PaymentMethod = "cash",
            //    SaleAccountID = 12,
            //    Pedido = pedido

            //};
            //var resposta = MyFinanceAPI.PostRecebivel(recebivel).Result;

            
            service.Init();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

        }
    }
}
