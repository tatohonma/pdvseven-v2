using a7D.PDV.Integracao.Pagamento.NTKPos;
using a7D.PDV.Model;
using System;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS Tela6_FidelidadeCPF()
        {
            var cpf = termGetText(Terminal.terminalId, "\r\r  Cartao Fidelidade\r\r  CPF: (so números)", "@@@@@@@@@@@", ref Terminal.ret);

            if (cpf == null)
                return Tela2_MenuPagamento;
            else if (string.IsNullOrEmpty(cpf))
                cpf = "19221149870";
            else if (Terminal.ret == (int)PTIRET.NOCONN || Terminal.ret == (int)PTIRET.PROTOCOLERR)
            {
                onTerminalEvent(Terminal, "QUEDA CONEXÃO");
                return Tela6_FidelidadeCPF;
            }
            else if (Terminal.ret != 0)
                return ErroValorDigitado;
            else if (string.IsNullOrEmpty(cpf))
                return ErroValorVazio;

            termDisplayMessageCenter(Terminal.terminalId, "Fidelidade\r\r" + cpf);
            Terminal.documentoFidelidade = cpf;

            return Tela7_DescontoFidelidade;
        }
    }
}