using a7D.PDV.Integracao.Pagamento.NTKPos;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS TelaB_Reimpressao()
        {
            string[] menu = {
                "Último Comprovante",
                "Última Conta"
            };

            onTerminalEvent(Terminal, "Reimpressão");

            int selectedMenu = termGetMenuSelectionFromUser(Terminal.terminalId, "Reimpressão", menu);

            switch (selectedMenu)
            {
                case 0:
                    if (string.IsNullOrEmpty(Terminal.UltimoComprovante))
                        termDisplayMessageCenter(Terminal.terminalId, "Não diponível");
                    else
                        termPrintTextSymbol(Terminal.terminalId, Terminal.UltimoComprovante);

                    break;

                case 1:

                    //Terminal.UltimaConta = impressao.TextoFiscal(6088, 40);

                    if (string.IsNullOrEmpty(Terminal.UltimaConta))
                        termDisplayMessageCenter(Terminal.terminalId, "Não diponível");
                    else
                        termPrintTextSymbol(Terminal.terminalId, Terminal.UltimaConta);

                    break;
            }
            return Tela1_MenuPrincipal;
        }
    }
}