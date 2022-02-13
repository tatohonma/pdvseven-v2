using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Integracao.Pagamento;
using MuxxLib;
using System;
using System.Windows;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal class PagamentoServices
    {
        internal ITEF TEF { get; private set; }

        FactoryBase factory;

        internal PagamentoServices()
        {
        }

        internal void CriaTEF(int pedidoID, decimal total)
        {
            if (PdvServices.MeioPagamento == "NTKDLL")
            {
                if (App.NTKCertTEST)
                    factory = new Integracao.Pagamento.NTKTEFDLL.FactoryNoScreen(false);
                else
                    factory = new Integracao.Pagamento.NTKTEFDLL.FactoryWPF(false);

                TEF = new Integracao.Pagamento.NTKTEFDLL.NTKPinpadPayGoWeb(pedidoID, pedidoID, total, factory);
            }
            else if (PdvServices.MeioPagamento == "GranitoTEF")
            {
                TEF = new Integracao.Pagamento.GranitoTEF.GranitoPinpad(pedidoID, pedidoID, total, "");
            }
                else if (PdvServices.MeioPagamento == "STONE")
            {
                TEF = new Integracao.Pagamento.StoneTEF.PinpadStoneTEF(pedidoID, total, null);
            }
            else if (PdvServices.MeioPagamento == "NTKPayGo")
            {
                var ntk = Integracao.Pagamento.NTKTEF.NTKBuilder.AutorizacaoVenda(pedidoID, "Loja", "Autoatendimento", DateTime.Now, pedidoID, total, 0);
                TEF = ntk.CriaTEF();
            }
            else
                throw new Exception("Meio de pagamento não escolhido");
        }

        internal void Execute(Window modalWindow)
        {
            if (factory != null)
                factory.Modal = modalWindow;

            modalWindow.ShowDialog();

            EventLogServices.SuccessAudit(TEF.Log);
        }
    }
}