using a7D.PDV.Integracao.Pagamento.NTKPos;
using a7D.PDV.Model;
using System;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS Tela9_DocumentoFical()
        {
            if (!string.IsNullOrEmpty(Terminal.documentoFiscal))
                return TelaA_FinalizaPedido;

            var documento = termGetText(Terminal.terminalId, "\r\rInforme o CPF/CNPJ\rpara Cupom Fiscal:\r(só numeros)", "@@@@@@@@@@@@@@", ref Terminal.ret, false, 0);
            Terminal.documentoFiscal = documento;

            return TelaA_FinalizaPedido;
        }
    }
}