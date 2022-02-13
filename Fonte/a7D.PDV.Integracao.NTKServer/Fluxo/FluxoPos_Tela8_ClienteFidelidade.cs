using a7D.PDV.Integracao.Pagamento.NTKPos;
using a7D.PDV.Model;
using System;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS Tela8_ClienteFidelidade()
        {
            if (!string.IsNullOrEmpty(Terminal.documentoFidelidade))
                return Tela9_DocumentoFical;

            var cpf = termGetText(Terminal.terminalId, "\r\rCliente Fidelidade\r\rCPF: (so numeros)", "@@@@@@@@@@@", ref Terminal.ret, false, 0);
            if (!string.IsNullOrEmpty(cpf))
                Terminal.documentoFidelidade = cpf;

            return Tela9_DocumentoFical;
        }
    }
}