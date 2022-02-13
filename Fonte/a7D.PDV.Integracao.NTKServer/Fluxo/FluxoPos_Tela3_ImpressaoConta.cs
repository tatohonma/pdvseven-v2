using a7D.PDV.Integracao.Pagamento.NTKPos;
using a7D.PDV.Model;
using System;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS Tela3_Imprimir()
        {
            var qtd = termGetText(Terminal.terminalId, "\r\r\r\rInforme a quantidade\rde pessoas:", "@@", ref Terminal.ret);

            if (qtd == null)
                return Tela2_MenuPagamento;

            else if (Terminal.ret == (int)PTIRET.NOCONN || Terminal.ret == (int)PTIRET.PROTOCOLERR)
            {
                onTerminalEvent(Terminal, "QUEDA CONEXÃO");
                return Tela2_MenuPagamento;
            }
            else if (Terminal.ret != 0)
                return ErroValorDigitado;

            else if (string.IsNullOrEmpty(qtd))
                return ErroValorVazio;

            Terminal.pessoas = int.Parse(qtd);

            return Tela3_Imprimindo;
        }

        private ScreenPOS Tela3_Imprimindo()
        {
            try
            {
                var conta = impressao.TextoConta(Terminal.pedido, 40, Terminal.pessoas);
                Terminal.ret = termPrintTextSymbol(Terminal.terminalId, conta);
                return Tela2_MenuPagamento;
            }
            catch (Exception ex)
            {
                termDisplayMessageCenter(Terminal.terminalId, "ERRO\r" + ex.Message);
                return Tela1_MenuPrincipal;
            }
        }
    }
}