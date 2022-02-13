using a7D.PDV.Integracao.Pagamento.NTKPos;
using static a7D.PDV.Integracao.Pagamento.NTKPos.TerminalFunctions;

namespace a7D.PDV.Integracao.NTKServer
{
    public partial class FluxoPos
    {
        private ScreenPOS TelaF_Teste()
        {
            //short ret=0;

            //ClassPOSPGW.PTI_Print(Terminal.terminalId, "3518 0214 2001 6600 0166 5990".PadBoth(40), ref ret);
            //ClassPOSPGW.PTI_Print(Terminal.terminalId, "0009 8470 0449 4434 0984".PadBoth(40), ref ret);

            //ClassPOSPGW.PTI_PrnSymbolCode(Terminal.terminalId, "1234567890", (short)CODESYMB.ITF14, ref ret);
            //ClassPOSPGW.PTI_PrnSymbolCode(Terminal.terminalId, "12345678901234567890123456789012345678901234", (short)CODESYMB.ITF14, ref ret);

            //ClassPOSPGW.PTI_PrnSymbolCode(Terminal.terminalId, "3518021420016600016659", (short)CODESYMB.ITF14, ref ret);
            //ClassPOSPGW.PTI_PrnSymbolCode(Terminal.terminalId, "9000098470044944340984", (short)CODESYMB.ITF14, ref ret);
            //ClassPOSPGW.PTI_PrnSymbolCode(Terminal.terminalId, "35180214200166000166599000098470044944340984|20180227184552|6.60||dHFPy6YtYZoxAYfIeUVOSnVNiyIvgXs6kapnRMEa/IQJuEP1rQUljt63kJ6rC2l5EQ7fSvMinpOKP0tkeeIj5iVm2LyrHAJO88OC25hGF2l78FEHjP4mMH6l94ZLjHrIKq61JV7w2gkYrnrqYtAZZJVqglOSMkzCYKlyLmOnfqBLayhdzDJcU3k6blH7drNSze2XOFSs/BgQuu8sl00rYn1Egtr+hWPy+22YQpWz0VJCRCFgHEKLeZkojcl0dkpXYxOHHXtAVRaJ/VNe+05OKXDDIlXy1wPlro6ZdPDS8F9WBCt8r+2tfIqvO8OciWinJunBNUrY3LJZSK", (short)CODESYMB.QRCODE, ref ret);

            //ClassPOSPGW.PTI_PrnFeed(Terminal.terminalId, ref ret);

            var itens = termGetPaymentResponse(Terminal.terminalId);

            //ClassPOSPGW.PTI_EFT_Start(Terminal.terminalId, (int)PTITRN.ADMIN, ref ret);
            ////paramList.Add(new PGWParam((int)PTIINF.FINTYPE, "1")); // 1: a vista

            //ClassPOSPGW.PTI_EFT_Exec(Terminal.terminalId, ref ret);


            return Tela1_MenuPrincipal;
        }
    }
}