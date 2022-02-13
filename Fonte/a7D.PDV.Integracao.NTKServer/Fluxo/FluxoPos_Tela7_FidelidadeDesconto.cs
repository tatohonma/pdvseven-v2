using a7D.PDV.Integracao.Pagamento.NTKPos;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS Tela7_DescontoFidelidade()
        {
            // TODO: Integrar DONUS! e dar descontos
            string[] menu = {
                "3% de desconto: 280 pts",
                "8% de desconto: 420 pts",
                "15% de desconto: 1200 pts",
                "40% de desconto: 2800 pts",
                "Voltar"
            };

            string titulo = "Informe o desconto";
            onTerminalEvent(Terminal, titulo);
            Terminal.ret = 0;
            int selectedMenu = termGetMenuSelectionFromUser(Terminal.terminalId, titulo, menu);

            switch (selectedMenu)
            {
                case (int)PTIRET.NOCONN: // perda de conexão
                case (int)PTIRET.PROTOCOLERR:
                    onTerminalEvent(Terminal, "QUEDA CONEXÃO");
                    return Tela7_DescontoFidelidade;

                case 0:
                    Terminal.valorTEF = Terminal.valorPendente;
                    break;

                case 1:
                    Terminal.valorTEF = Terminal.valorPendente;
                    break;

                case 2:
                    Terminal.valorTEF = Terminal.valorPendente;
                    break;

                case 3:
                    Terminal.valorTEF = Terminal.valorPendente;
                    break;

                default:
                    return Tela1_MenuPrincipal;
            }

            Terminal.pagamento = new a7D.PDV.Integracao.Pagamento.NTKPos.Pagamento()
            {
                autorizacao = Terminal.documentoFidelidade,
                cardName = "Fidelidade",
                adquirente = "Fidelidade",
                cardType = 3,
                cardMask = ""
            };
            return Tela5_ConfirmaPagamento;
        }
    }
}