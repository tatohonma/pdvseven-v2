using a7D.PDV.Integracao.Pagamento.NTKPos;
using System;
using System.IO;
using System.Text;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
#if TESTE
        private ScreenPOS Tela5_TEF()
        {
            Terminal.pagamento = new Pagamento.NTKPos.Pagamento()
            {
                autorizacao = $"123-456",
                cardName = "VISA",
                adquirente = "CIELO",
                cardType = 1,
                cardMask = "********1234"
            };

            termPrintTextSymbol(Terminal.terminalId, "Recibo Fake: " + Terminal.valorTEF.ToString("N2"));

            return Tela5_ConfirmaPagamento;
        }
#else
        private ScreenPOS Tela5_TEF()
        {
            onTerminalEvent(Terminal, "TEF " + Terminal.valorTEF.ToString("N2"));
            Terminal.ret = termMakePayment(Terminal.terminalId, Terminal.valorTEF);
            if (Terminal.ret == (int)PTIRET.TIMEOUT
             || Terminal.ret == (int)PTIRET.CANCEL
             || Terminal.ret == (int)PTIRET.PROTOCOLERR
             || Terminal.ret == (int)PTIRET.EFTERR)
            {
                onTerminalEvent(Terminal, "CANCELADO: " + Terminal.ret);
                if (Terminal.tipo == 0)
                    return Tela1_MenuPrincipal;
                else
                    return Tela2_MenuPagamento;
            }
            else if (Terminal.ret != 0)
                return ErroPagamento;

            var results = termGetPaymentResponse(Terminal.terminalId);

            try
            {
                Autorizacao.Save(results);
            }
            catch (Exception ex)
            {
                onErroEvent(Terminal, new Exception("SAVE RESULT ERRO: " + ex.Message, ex));
            }

            try
            {
                string aut1 = results.Find(p => p.type == PTIINF.AUTLOCREF)?.value;
                string aut2 = results.Find(p => p.type == PTIINF.AUTHCODE)?.value;

                if (string.IsNullOrEmpty(aut1) && string.IsNullOrEmpty(aut2))
                    throw new Exception("Erro na Autorização");

                Terminal.pagamento = new Pagamento.NTKPos.Pagamento()
                {
                    autorizacao = $"{aut1}-{aut2}",
                    cardName = results.Find(p => p.type == PTIINF.CARDNAME)?.value,
                    adquirente = results.Find(p => p.type == PTIINF.AUTHSYST)?.value,
                    cardType = Int32.Parse(results.Find(p => p.type == PTIINF.CARDTYPE)?.value ?? "0"), // “01” - Credit “02” - Debit
                    cardMask = results.Find(p => p.type == PTIINF.CARDPARCPAN)?.value
                };

                var ret = termSetTransactionResult(Terminal.terminalId, PTICNF.SUCCESS);

                onTerminalEvent(Terminal, $"TEF AUT({ret}) {Terminal.pagamento.autorizacao}");
            }
            catch (Exception ex)
            {
                if (Terminal.pagamento != null)
                {
                    Terminal.pagamento = null;
                    var ret = termSetTransactionResult(Terminal.terminalId, PTICNF.OTHERERR);
                    onErroEvent(Terminal, new Exception($"CANCELADO! TEF ERRO({ret}): " + ex.Message, ex));
                }
                else
                    onErroEvent(Terminal, new Exception("TEF ERRO: " + ex.Message, ex));

                termDisplayMessageCenter(Terminal.terminalId, "ERRO\r" + ex.Message);
                if (Terminal.tipo == 0)
                    return Tela1_MenuPrincipal;
                else
                    return Tela2_MenuPagamento;
            }

            var prn = termPrintReceipt(Terminal.terminalId);
            onTerminalEvent(Terminal, $"COMPROVANTE IMPRESSO({prn})");

            Terminal.UltimoComprovante = results.Find(p => p.type == PTIINF.RCPTCHOLDER)?.value;

            return Tela5_ConfirmaPagamento;
        }
#endif
        private ScreenPOS Tela5_ConfirmaPagamento()
        {
            if (Terminal.tipo == 0)
            {
                Terminal.pagamento = null;
                return Tela1_MenuPrincipal;
            }
            else if (Terminal.valorTEF > 0)
            {
                termDisplayMessageCenter(Terminal.terminalId, "Registrando...", 10);
                var info = pedido.Pagar(Terminal);
                if (info?.Mensagem == "OK" && info.Id > 0)
                {
                    Terminal.pagamento = null;
                    Terminal.RequerDesconectar = true; // Necessário sempre que se conclui um pagamento para enviar a confirmação
                    if (Terminal.valorTEF == Terminal.valorPendente)
                        return Tela8_ClienteFidelidade;
                    else
                        termDisplayMessageCenter(Terminal.terminalId,
                            "Pagamento " + info.Id + " OK\r\rR$ " + Terminal.valorTEF.ToString("N2"), 500);
                }
                else
                {
                    onTerminalEvent(Terminal, info.Mensagem);
                    termDisplayMessageCenter(Terminal.terminalId, info.Mensagem);
                    return ErroRegistrarPagamento;
                }
            }
            return Tela2_MenuPagamento;
        }

        private ScreenPOS ErroRegistrarPagamento()
        {
            string[] menu = { "Sim", "Não" };
            int selectedMenu = termGetMenuSelectionFromUser(Terminal.terminalId, "Erro, tentar novamente?", menu);
            switch (selectedMenu)
            {
                case 1:
                    Terminal.pagamento = null;
                    return Tela1_MenuPrincipal;
                default:
                    return Tela5_ConfirmaPagamento;
            }
        }

        //results2.Find(p=>p.field==50).value
        /*
? Exemplo de resultados! Count = 20

[0]: {50: TRANSACAO AUTORIZADA}
[1]: {51: 1}
[2]: {52: 0000386120}
[3]: {54: 044464}
[4]: {55: }
[5]: {56: 000000042395}
[6]: {57: 986}
[7]: {58: 1}
[8]: {62: MASTERCARD  }
[9]: {66: REDE}
[10]: {71: 000002000000001}
[11]: {72: 002244442}
[12]: {73: 271477}
[13]: {74: 1}
[14]: {75: 4}
[15]: {76: 537110******8017}
[16]: {77: }
[17]: {100: 00}
[18]: {2000:                                         

*** PAY&GO WEB - CERTIFICACAO ***    

REDECARD 
MASTERCARD 
SIMULADOR PDV REDECARD 
COMPR:002244442 VALOR: 000000042395
ESTAB:000000001 02/12/2017-103354
TERM:PV000002/271477
CARTAO: 537110******8017 
AUTORIZACAO: 044464
ARQC:E36B7769D3BDE9A8
TRANSACAO AUTORIZADA MEDIANTE 
USO DE SENHA PESSOAL 
----------------------------------------
52704 0000000001 0000271477 
NOME FANTASIA: NTK EXCLUSIVO
CERTIFICACAO
FEDERAL TAX ID: 07.383.312/0001-70
*** SETIS & NTK - AMBIENTE APP02 ***  
}

[19]: {4000:                                         

*** PAY&GO WEB - CERTIFICACAO ***    
REDECARD 
MASTERCARD 
SIMULADOR PDV REDECARD 
COMPR:002244442 VALOR: 000000042395
ESTAB:000000001 02/12/2017-103354
TERM:PV000002/271477
CARTAO: 537110******8017 
AUTORIZACAO: 044464
ARQC:E36B7769D3BDE9A8
TRANSACAO AUTORIZADA MEDIANTE 
USO DE SENHA PESSOAL 
----------------------------------------
52704 0000000001 0000271477 
NOME FANTASIA: NTK EXCLUSIVO
CERTIFICACAO
FEDERAL TAX ID: 07.383.312/0001-70
*** SETIS & NTK - AMBIENTE APP02 ***  
}
*/
    }
}