using a7D.PDV.Integracao.Pagamento.NTKPos;
using a7D.PDV.Model;
using System;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS Tela4_PagamentoParcial()
        {
            var pagamento = termGetAmount(Terminal.terminalId, "\r\r\r\r" + "Valor:".PadBoth());
            Terminal.valorTEF = TextoValor(pagamento);

            if (Terminal.valorTEF == 0)
                return Tela2_MenuPagamento;

            else if (Terminal.ret == (int)PTIRET.NOCONN || Terminal.ret == (int)PTIRET.PROTOCOLERR)
            {
                onTerminalEvent(Terminal, "QUEDA CONEXÃO");
                return Tela4_PagamentoParcial;
            }
            else if (Terminal.ret != 0)
                return ErroValorDigitado;

            else if (string.IsNullOrEmpty(pagamento))
                return ErroValorVazio;

            if (Terminal.valorTEF > Terminal.valorPendente)
            {
                termDisplayMessageCenter(Terminal.terminalId,
                    "Valor maior do\r" +
                    "que o necessario\r\r" +
                    ("R$ " + Terminal.valorPendente.ToString("N2")));

                return Tela4_PagamentoParcial;
            }
            else if (Terminal.valorTEF <= 0)
            {
                termDisplayMessageCenter(Terminal.terminalId, "Valor invalido");
                return Tela4_PagamentoParcial;
            }

            return Tela5_TEF;
        }
    }
}