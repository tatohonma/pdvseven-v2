using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Integracao.Pagamento.NTKPos;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS TelaA_FinalizaPedido()
        {
            onTerminalEvent(Terminal, "Registrando Fechamento " + Terminal.numero);
            termDisplayMessageCenter(Terminal.terminalId, "Registrando...", 10);

            if (Terminal.valorTEF == Terminal.valorPendente)
            {
                var info = pedido.Fechar(new FechamentoPedido(Terminal.pedido, Terminal.pdvID, Terminal.pdvUserChave, false, Terminal.documentoFiscal));

                if (info != null && !string.IsNullOrEmpty(info.Mensagem) && info.Mensagem != "OK")
                {
                    onTerminalEvent(Terminal, info.Mensagem);
                    termDisplayMessageCenter(Terminal.terminalId, info.Mensagem);
                    return Tela2_MenuPagamento;
                }
                else if (Terminal.tipo == (int)ETipoPedido.Mesa)
                    termDisplayMessageCenter(Terminal.terminalId, "Mesa Fechada\r\rAguarde...", 10);
                else if (Terminal.tipo == (int)ETipoPedido.Comanda)
                    termDisplayMessageCenter(Terminal.terminalId, "Comanda Fechada\r\rAguarde...", 10);

                string conta = null;

                if (IntegraNTK.ModoFiscal)
                {
                    conta = impressao.TextoFiscal(Terminal.pedido, 40);
                    if (conta == null)
                    {
                        termDisplayMessageCenter(Terminal.terminalId, "ERRO NO SAT\r\rImprimindo Conta...", 10);
                        conta = impressao.TextoConta(Terminal.pedido, 40);
                        conta += "\r\rMODO DE CONTIGENCIA\rSAT Indisponível";
                    }
                }
                else
                    conta = impressao.TextoConta(Terminal.pedido, 40);

                Terminal.UltimaConta = conta;
                Terminal.ret = termPrintTextSymbol(Terminal.terminalId, conta);

                return Tela1_MenuPrincipal;
            }

            return Tela2_MenuPagamento;
        }
    }
}