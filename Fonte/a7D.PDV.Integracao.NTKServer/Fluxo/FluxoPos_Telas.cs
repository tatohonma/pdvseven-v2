using a7D.PDV.Integracao.Pagamento.NTKPos;
using a7D.PDV.Model;
using System;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.Servico.Core.NTKPos
{
    public partial class FluxoPos
    {
        private ScreenPOS TelaPresente()
        {
            var cpf = termGetData(Terminal.terminalId, "\r\r   Cartao Presente\r\r  CPF:  (so numeros)", "    @@@.@@@.@@@-@@", 1, 11, true, false, false, 60, 7, ref Terminal.ret);
            if (cpf == null)
                return Tela2_MenuPagamento;
            else if (Terminal.ret == (int)PTIRET.NOCONN || Terminal.ret == (int)PTIRET.PROTOCOLERR)
                return TelaPresente;
            else if (Terminal.ret != 0)
                return ErroValorDigitado;
            else if (string.IsNullOrEmpty(cpf))
                return ErroValorVazio;

            termDisplayMessageCenter(Terminal.terminalId, "Validando " + cpf);
            Terminal.pagamento = new a7D.PDV.Integracao.Pagamento.NTKPos.Pagamento()
            {
                autorizacao = cpf,
                cardName = "Presente",
                adquirente = "Presente",
                cardType = 4,
                cardMask = cpf
            };
            return TelaA_FinalizaPedido;
        }
    }
}