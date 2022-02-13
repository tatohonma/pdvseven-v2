using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.Pagamento.NTKPos;
using a7D.PDV.Model;
using System;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS Tela1_MenuPrincipal()
        {
            if (Terminal.RequerDesconectar)
            {
                Terminal.RequerDesconectar = false;
                onTerminalEvent(Terminal, "Desconectando e enviando confirmações (Tela1)");
                termDisconnect(Terminal.terminalId, 0);
                return Tela1_MenuPrincipal;
            }

            // RESET!
            Terminal.ret = 0;
            Terminal.documentoFidelidade = null;
            Terminal.documentoFiscal = null;
            Terminal.tipo = 0;
            Terminal.pedido = 0;


            string[] menu = {
                "Mesa",
                "Comanda",
                //"Delivery",
                //"Caixa",
                "Pagamento Direto",
                //"Sair",
                "Reimpressão"
            };

            int selectedMenu = termGetMenuSelectionFromUser(Terminal.terminalId, "Menu Principal", menu);

            switch (selectedMenu)
            {
                case (int)PTIRET.NOCONN: // perda de conexão
                case (int)PTIRET.PROTOCOLERR:
                    return Tela1_MenuPrincipal;

                case (int)PTIRET.CANCEL:
                    termDisconnect(Terminal.terminalId, 0);
                    return Tela1_MenuPrincipal;

                case 0:
                    return Tela11_Mesa;

                case 1:
                    return Tela12_Comanda;

                //case 2:
                //    // Para reimpressão ??
                //    //var results = termGetPaymentResponse(Terminal.terminalId);
                //    return Tela13_Delivery;

                //case 3:
                //    return Tela14_Pedido;

                case 2:
                    return Tela15_PagamentoAVulso;

                case 3:
                    return TelaB_Reimpressao;

                //case 5:
                //    return TelaLogin;
                default:
                    return Tela1_MenuPrincipal;
            }
        }



        private ScreenPOS Tela11_Mesa()
        {
            var mesa = termGetText(Terminal.terminalId, "\r\r\r\rMesa:", "@@@", ref Terminal.ret);

            if (mesa == null)
                return Tela1_MenuPrincipal;

            else if (Terminal.ret == (int)PTIRET.NOCONN)
                return Tela11_Mesa;

            else if (Terminal.ret != 0)
                return ErroValorDigitado;

            else if (string.IsNullOrEmpty(mesa))
                return ErroValorVazio;

            Terminal.tipo = (int)ETipoPedido.Mesa;
            Terminal.numero = int.Parse(mesa);
            return Tela2_MenuPagamento;
        }

        private ScreenPOS Tela12_Comanda()
        {
            var comanda = termGetText(Terminal.terminalId, "\r\r\r\rComanda:", "@@@@", ref Terminal.ret);

            if (comanda == null)
                return Tela1_MenuPrincipal;
            //else if (string.IsNullOrEmpty(comanda))
            //    comanda = "200";
            else if (Terminal.ret == (int)PTIRET.NOCONN || Terminal.ret == (int)PTIRET.PROTOCOLERR)
            {
                onTerminalEvent(Terminal, "QUEDA CONEXÃO");
                return Tela12_Comanda;
            }
            else if (Terminal.ret != 0)
                return ErroValorDigitado;
            else if (string.IsNullOrEmpty(comanda))
                return ErroValorVazio;

            Terminal.tipo = (int)ETipoPedido.Comanda;
            Terminal.numero = int.Parse(comanda);

            return Tela2_MenuPagamento;
        }

        private ScreenPOS Tela13_Delivery()
        {
            var pedido = termGetText(Terminal.terminalId, "\r\r\r\rPedido Delivery:", "@@@@@@", ref Terminal.ret);
            if (pedido == null)
                return Tela1_MenuPrincipal;

            else if (Terminal.ret == (int)PTIRET.NOCONN)
                return Tela14_Pedido;

            else if (Terminal.ret != 0)
                return ErroValorDigitado;

            else if (string.IsNullOrEmpty(pedido))
                return ErroValorVazio;

            Terminal.tipo = (int)ETipoPedido.Delivery;
            Terminal.numero = Terminal.pedido = int.Parse(pedido);

            return Tela2_MenuPagamento;
        }

        private ScreenPOS Tela14_Pedido()
        {
            var pedido = termGetText(Terminal.terminalId, "\r\r\r\rPedido:", "@@@@@@", ref Terminal.ret);
            if (pedido == null)
                return Tela1_MenuPrincipal;

            else if (Terminal.ret == (int)PTIRET.NOCONN)
                return Tela14_Pedido;

            else if (Terminal.ret != 0)
                return ErroValorDigitado;

            else if (string.IsNullOrEmpty(pedido))
                return ErroValorVazio;

            Terminal.tipo = (int)ETipoPedido.Balcao;
            Terminal.numero = Terminal.pedido = int.Parse(pedido);

            return Tela2_MenuPagamento;
        }

        private ScreenPOS Tela15_PagamentoAVulso()
        {
            var pagamento = termGetAmount(Terminal.terminalId, "\r\r\r\r" + "Valor:".PadBoth());
            Terminal.tipo = 0;
            Terminal.valorTEF = TextoValor(pagamento);

            if (Terminal.valorTEF == 0)
                return Tela1_MenuPrincipal;

            else if (Terminal.ret == (int)PTIRET.NOCONN || Terminal.ret == (int)PTIRET.PROTOCOLERR)
            {
                onTerminalEvent(Terminal, "QUEDA CONEXÃO");
                return Tela15_PagamentoAVulso;
            }

            else if (Terminal.ret != 0)
                return ErroValorDigitado;

            else if (string.IsNullOrEmpty(pagamento))
                return ErroValorVazio;

            return Tela5_TEF;
        }

    }
}