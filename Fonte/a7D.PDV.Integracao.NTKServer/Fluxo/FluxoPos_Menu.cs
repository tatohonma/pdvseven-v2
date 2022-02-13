using a7D.PDV.Integracao.Pagamento.NTKPos;
using a7D.PDV.Model;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.Servico.Core.NTKPos
{
    public partial class FluxoPos
    {
        private ScreenPOS MenuTipoPagamento()
        {
            string[] menu = {
                "Debito/Credito",
                "Cartao Fidelidade",
                "Cartao Presente",
                "Voltar"
            };

            string titulo = "A Pagar";
            onTerminalEvent(Terminal, titulo + " " + Terminal.numero);
            titulo += "\rR$ " + Terminal.valorTEF.ToString("N2");
            Terminal.ret = 0;
            int selectedMenu = termGetMenuSelectionFromUser(Terminal.terminalId, titulo, menu);

            switch (selectedMenu)
            {
                case (int)PTIRET.NOCONN: // perda de conexão
                case (int)PTIRET.PROTOCOLERR:
                    return MenuTipoPagamento;
                case 0:
                    return Tela5_TEF;
                case 1:
                    return Tela6_FidelidadeCPF;
                case 2:
                    return TelaPresente;
                case 3:
                    return Tela2_MenuPagamento;
                default:
                    return Tela1_MenuPrincipal;
            }
        }
    }
}