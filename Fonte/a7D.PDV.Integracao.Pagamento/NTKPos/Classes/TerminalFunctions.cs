using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace a7D.PDV.Integracao.Pagamento.NTKPos
{
    public class TerminalFunctions
    {

        #region terminalFunctions

        public static short termMakePayment(string terminalId, decimal amount)//, string cardType = null)
        {
            short ret = 99;
            var paramList = new List<PGWParam>();

            //paramList.Add(new PGWParam((int)PTIPAR.AMOUNT, ((int)(amount * 100)).ToString()));
            //paramList.Add(new PGWParam((int)PTIPAR.CURRENCY, "986")); // PT (BRZ: Real)

            paramList.Add(new PGWParam((int)PTIINF.TOTAMNT, ((int)(amount * 100)).ToString()));
            paramList.Add(new PGWParam((int)PTIINF.CURRENCY, "986")); // PT (BRZ: Real)

            paramList.Add(new PGWParam((int)PTIINF.FINTYPE, "1")); // 1: a vista
            //paramList.Add(new PGWParam((int)PTIINF.CARDTYPE, "02")); // "01” - Credit,  “02” - Debit
            //paramList.Add(new PGWParam((int)PTIINF.FINTYPE, "2")); // 2: parcelado pelo Emissor
            //paramList.Add(new PGWParam((int)PTIINF.INSTALLMENTS, "3")); // numero de parcelas

            ClassPOSPGW.PTI_EFT_Start(terminalId, (int)PTITRN.SALE, ref ret);
            if (ret != 0)
                return ret;

            foreach (PGWParam item in paramList)
            {
                ClassPOSPGW.PTI_EFT_AddParam(terminalId, item.field, item.value, ref ret);
                if (ret != 0)
                    return ret;
            }

            ClassPOSPGW.PTI_EFT_Exec(terminalId, ref ret);
            return ret;

        }

        public static int termDisplayMessageCenter(string terminalId, string message, int waitDelay = 2000)
        {

            string[] linhas = message.RemoveAcentos().Replace("\n", "").Split('\r');
            string message2 = "";

            foreach (var msg in linhas)
            {
                var linha = msg;
                int max = 21;
                if (linha.Length > 0 && linha.Length < max)
                    message2 += linha.PadBoth();
                else
                {
                    while (linha.Length > 0 && message2.Length < 150)
                    {
                        if (linha.Length > max)
                        {
                            message2 += linha.Substring(0, max) + "\r";
                            linha = linha.Substring(max);
                        }
                        else
                        {
                            message2 += linha;
                            break;
                        }
                    }
                }
                message2 += "\r";
            }

            message2 = message2.Substring(0, message2.Length - 1);

            short ret = 99;
            ClassPOSPGW.PTI_Display(terminalId, message2, ref ret);
            Thread.Sleep(waitDelay);

            return ret;
        }

        public static short termPrintTextSymbol(string terminalId, string text)
        {
            short ret = 0;

            if (string.IsNullOrEmpty(text))
                return ret;

            text = text.Replace("\n", "").RemoveAcentos();
            int max = 5000;
            int simbolo;
            while (text != "" && ret == 0)
            {
                string print = text;
                simbolo = print.IndexOf("@ITF14:");
                if (simbolo == -1)
                    simbolo = print.IndexOf("@QRCODE:");

                if (print.Length > max || simbolo > 0)
                {
                    if (simbolo == -1 || simbolo > max)
                        simbolo = max;

                    var i = print.LastIndexOf('\r', simbolo);
                    print = print.Substring(0, i);
                    text = text.Substring(i + 1);
                    simbolo = -1;
                }
                else
                    text = "";

                if (simbolo == 0 && print.Length > 10)
                {
                    int endline = print.IndexOf("\r");
                    if (endline == -1)
                        endline = print.Length;

                    text = print.Substring(endline + (text == "" ? 1 : 0)) + text;
                    if (print.StartsWith("@ITF14:"))
                    {
                        print = print.Substring(7, endline - 7);
                        ret = termPrintSymbol(terminalId, CODESYMB.ITF14, print);
                    }
                    else if (print.StartsWith("@QRCODE:"))
                    {
                        print = print.Substring(8, endline - 8);
                        ret = termPrintSymbol(terminalId, CODESYMB.QRCODE, print);
                    }
                }
                else
                    ClassPOSPGW.PTI_Print(terminalId, print, ref ret);
            }

            if (ret == 0)
                ClassPOSPGW.PTI_PrnFeed(terminalId, ref ret);
            else if (ret == (int)PTIRET.PRINTERR)
                termDisplayMessageCenter(terminalId, "Erro na impressora");
            else if (ret == (int)PTIRET.NOPAPER)
                termDisplayMessageCenter(terminalId, "Sem papel");

            return ret;
        }

        public static short termSetTransactionResult(string terminalId, PTICNF iResult)
        {

            short ret = 99;

            ClassPOSPGW.PTI_EFT_Confirm(terminalId, (int)iResult, ref ret);

            return ret;
        }

        public static bool termPrintReceipt(string terminalId, int copies = 3)
        {
            short ret = 99;

            ClassPOSPGW.PTI_EFT_PrintReceipt(terminalId, copies, ref ret);

            if (ret == (int)PTIRET.PRINTERR)
                termDisplayMessageCenter(terminalId, "Erro na impressora");
            else if (ret == (int)PTIRET.NOPAPER)
                termDisplayMessageCenter(terminalId, "Sem papel");

            return ret == 0;
        }

        public static short termPrintSymbol(string terminalId, CODESYMB tipo, string simbol)
        {

            short ret = 99;

            ClassPOSPGW.PTI_PrnSymbolCode(terminalId, simbol, (short)tipo, ref ret);

            return ret;
        }

        public static string termGetAmount(string terminalId, string msgInsertAmount, short timeoutInUserInput = 60)
        {
            short ret = 99;
            string formatAmount = "@@@.@@@,@@";
            string amount = termGetData(terminalId, msgInsertAmount, formatAmount.PadBoth(), 3, (short)formatAmount.Length, false, false, false, timeoutInUserInput, 4, ref ret);

            if (string.IsNullOrEmpty(amount))
                return "0";

            return amount;
        }

        public static List<PGWParam> termGetPaymentResponse(string terminalId)
        {
            short ret = 99;

            List<PGWParam> responseObject = new List<PGWParam>();
            uint[] responseFields = Array.ConvertAll((uint[])(Enum.GetValues(typeof(PTIINF))), Convert.ToUInt32);
            foreach (int field in responseFields)
            {
                StringBuilder sb = new StringBuilder(1000);
                int counter = 0;
                string str = "";
                ClassPOSPGW.PTI_EFT_GetInfo(terminalId, (counter != 0 ? field + counter : field), 1000, sb, ref ret);
                str = sb.ToString();
                while (ret == 0 & field > 999)
                {
                    counter = counter + 1;
                    ClassPOSPGW.PTI_EFT_GetInfo(terminalId, (counter != 0 ? field + counter : field), 1000, sb, ref ret);
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        str = str + "\r\n" + sb.ToString();
                }
                responseObject.Add(new PGWParam(field, str));
            }

            return responseObject;
        }

        public static string termGetText(string terminalId, string pszPrompt, string pszFormat, ref short ret, bool mask = false, int min = 1)
        {
            return termGetData(terminalId, pszPrompt,
                pszFormat.PadBoth(), (short)min, (short)pszFormat.Length,
                true, false, mask, 60, 4, ref ret);
        }

        private static string termGetData(string terminalId, string pszPrompt, string pszFormat, short uiLenMin,
            short uiLenMax, bool fFromLeft, bool fAlpha, bool fMask, short uiTimeOutSec, short uiCaptureLine, ref short ret)
        {
            ret = 99;
            StringBuilder pszData = new StringBuilder(50);

            ClassPOSPGW.PTI_GetData(terminalId, pszPrompt.RemoveAcentos(), pszFormat, uiLenMin, uiLenMax, fFromLeft, fAlpha, fMask, uiTimeOutSec, pszData,
            uiCaptureLine, ref ret);

            if (ret == (int)PTIRET.TIMEOUT || ret == (int)PTIRET.CANCEL || ret == -210)
            {
                ret = 0;
                return null;
            }

            return pszData.ToString();

        }

        public static int termGetMenuSelectionFromUser(string terminalId, string menuLabel, string[] menuOptions)
        {
            while (true)
            {
                short ret = 99;
                int selectedMenu = termGetMenuSelection(terminalId, menuLabel.RemoveAcentos(), menuOptions, ref ret);
                switch (ret)
                {
                    case (int)PTIRET.OK:
                        break;

                    case (int)PTIRET.TIMEOUT: // -2004:
                        termDisplayMessageCenter(terminalId, "Selecione uma opção");
                        return ret;

                    case (int)PTIRET.CANCEL: // -2005:
                        //termDisplayMessageCenter(terminalId, "Cancelado", 500);
                        return ret;

                    default:
                        return ret;
                }

                if (selectedMenu > menuOptions.Length - 1)
                {
                    termDisplayMessageCenter(terminalId, "Opcao Invalida");
                    continue;
                }

                return selectedMenu;
            }
        }

        public static TerminalInformation termCheckForConnection()
        {
            short ret = 99;
            StringBuilder terminalId = new StringBuilder(30);
            StringBuilder mac = new StringBuilder(30);
            StringBuilder model = new StringBuilder(30);
            StringBuilder serialNumber = new StringBuilder(50);
            //terminalId.Append(termInfo.terminalId);

            ClassPOSPGW.PTI_ConnectionLoop(terminalId, model, mac, serialNumber, ref ret);

            if (terminalId.ToString() == "")
                return null;

            return new TerminalInformation
            {
                terminalId = terminalId.ToString(),
                model = model.ToString(),
                mac = mac.ToString(),
                serialNumber = serialNumber.ToString(),
                ret = ret
            };
        }

        public static int termCheckStatus(ref TerminalInformation termInfo)
        {

            short ret = 99;
            ushort status = 99;
            StringBuilder mac = new StringBuilder(30);
            StringBuilder model = new StringBuilder(30);
            StringBuilder serialnumber = new StringBuilder(50);

            ClassPOSPGW.PTI_CheckStatus(termInfo.terminalId, ref status, model, mac, serialnumber, ref ret);

            if (ret != 0)
                return -1;

            termInfo.mac = mac.ToString();
            termInfo.model = model.ToString();
            termInfo.serialNumber = serialnumber.ToString();

            return status;
        }

        public static int termGetMenuSelection(string terminalId, string header, string[] menu, ref short ret)
        {

            ret = 99;
            short selectedOption = -1;

            ClassPOSPGW.PTI_StartMenu(terminalId, ref ret);

            if (ret != 0)
                return -999;

            foreach (string opt in menu)
            {
                if (opt == null)
                    continue;

                ret = 99;
                ClassPOSPGW.PTI_AddMenuOption(terminalId, opt.Max(18).RemoveAcentos(), ref ret);
                if (ret != 0)
                    return ret;
            }

            ret = 99;
            ClassPOSPGW.PTI_ExecMenu(terminalId, header.Max(20).RemoveAcentos(), 30, ref selectedOption, ref ret);

            if (ret != 0)
                return -1;

            return selectedOption;
        }

        public static string termRetText(short ret)
        {
            try
            {
                return ((PTIRET)ret).ToString();
            }
            catch (Exception)
            {
                return ret.ToString();
            }
        }

        public static void termFinDLL()
        {
            ClassPOSPGW.PTI_End();
        }

        public static short termDisconnect(string terminalId, short disconnectDelay)
        {
            short ret = 99;
            try
            {
                ClassPOSPGW.PTI_Disconnect(terminalId, disconnectDelay, ref ret);
            }
            catch (Exception) { }

            return ret;
        }

        //public static string tempTransformPGResponseFile(List<PGResponse> responseObject)
        //{

        //    string response = "";


        //    foreach (PGResponse field in responseObject)
        //    {
        //        //PWINFO_PNDAUTHSYST = REDE
        //        //PWINFO_PNDVIRTMERCH = 1
        //        //PWINFO_PNDREQNUM = 0000030389
        //        //PWINFO_PNDAUTLOCREF = 16035
        //        //PWINFO_PNDAUTEXTREF = 001096043

        //        //If field.Field = "PGWINF_ACQUIRER" Then field.Field = field.Field.Replace("PGWINF_ACQUIRER", "PWINFO_AUTHSYST")
        //        //'If field.Field = "PGWINF_AUTHCODE" Then field.Field = field.Field.Replace("PGWINF_STAN", "PWINFO_AUTHCODE")
        //        //'If field.Field = "PWINFO_REQNUM" Then PWINFO_REQNUM = field.Value
        //        //'If field.Field = "PGWINF_STAN" Then field.Field = field.Field.Replace("PGWINF_STAN", "PWINFO_AUTLOCREF")
        //        //If field.Field = "PGWINF_RRN" Then field.Field = field.Field.Replace("PGWINF_RRN", "PWINFO_AUTLOCREF")
        //        //If field.Field = "PGWINF_RESPCODE" Then field.Field = field.Field.Replace("PGWINF_RESPCODE", "PWINFO_AUTRESPCODE")
        //        //If field.Field = "PGWINF_MSG" Then
        //        //    field.Field = field.Field.Replace("PGWINF_MSG", vbCrLf & "PWINFO_RESULTMSG")
        //        //    field.Value = vbCrLf & field.Value
        //        //End If

        //        //If field.Field = "PGWINF_RCPTFULL" Then
        //        //    field.Field = field.Field.Replace("PGWINF_RCPTFULL", "PWINFO_RCPTFULL")
        //        //    field.Value = vbCrLf & field.Value
        //        //End If

        //        //If field.Field = "PGWINF_TIME" Then
        //        //    field.Field = field.Field.Replace("PGWINF_TIME", "PWINFO_DATETIME")
        //        //    field.Value = "20" & field.Value
        //        //End If


        //        response = response + field.Field + " = " + field.Value + Constants.vbCrLf;
        //    }

        //    // response = response & vbCrLf & "PWINFO_CNCDSPMSG = " & vbCrLf & "OPERACAO " & vbCrLf & "CANCELADA" & vbCrLf

        //    // response = response.Replace("PGWINF_", "PWINFO_")

        //    return response;

        //}

        ////Public Function tempTransformPGWResponseFile(ByVal responseObject As List(Of PGResponse)) As String

        ////    Dim response As String = ""

        ////    For Each field As PGResponse In responseObject

        ////        'PWINFO_PNDAUTHSYST = REDE
        ////        'PWINFO_PNDVIRTMERCH = 1
        ////        'PWINFO_PNDREQNUM = 0000030389
        ////        'PWINFO_PNDAUTLOCREF = 16035
        ////        'PWINFO_PNDAUTEXTREF = 001096043

        ////        response = response & field.Field & " = " & field.Value & vbCrLf
        ////    Next

        ////    response = response & vbCrLf & "PWINFO_CNCDSPMSG = " & vbCrLf & "OPERACAO " & vbCrLf & "CANCELADA" & vbCrLf

        ////    Return response

        ////End Function


        //public static int termMakePayment(int terminalId, PAY2all_SDK.pagamentoExterno EP)
        //{

        //    string transactionId = "";
        //    int amount = 0;
        //    string acquirer = "";
        //    E_TransactionType transactionType = null;
        //    E_TransactionInstallmentType transactionInstallmentType = null;
        //    int transactionInstallments = 0;

        //    string transactionDate = "";
        //    string transactionTime = "";

        //    try
        //    {
        //        if (EP.intencaoVenda.formaPagamento.modalidade.ToLower.Contains("débito"))
        //        {
        //            transactionType = E_TransactionType.debito;
        //            transactionInstallmentType = E_TransactionInstallmentType.aVista;
        //        }
        //        else if (EP.intencaoVenda.formaPagamento.modalidade.ToLower.Contains("crédito"))
        //        {
        //            transactionType = E_TransactionType.credito;
        //            if ((EP.intencaoVenda.quantidadeParcelas == null))
        //            {
        //                EP.intencaoVenda.quantidadeParcelas = 0;
        //            }
        //            else if (EP.intencaoVenda.quantidadeParcelas < 2)
        //            {
        //                transactionInstallmentType = E_TransactionInstallmentType.aVista;
        //            }
        //            else
        //            {
        //                if (EP.tipoParcelamento == 1)
        //                {
        //                    transactionInstallmentType = E_TransactionInstallmentType.parceladoEmissor;
        //                }
        //                else if (EP.tipoParcelamento == 2)
        //                {
        //                    transactionInstallmentType = E_TransactionInstallmentType.parceladoLoja;
        //                }
        //            }
        //        }
        //        else if (EP.intencaoVenda.formaPagamento.modalidade == "Voucher")
        //        {
        //            transactionType = E_TransactionType.voucher;
        //            transactionInstallmentType = E_TransactionInstallmentType.aVista;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    try
        //    {
        //        transactionDate = Convert.ToDateTime(EP.pagamentoExternoOriginal.dataAdquirente).ToString("yyMMdd");
        //        transactionTime = Convert.ToDateTime(EP.pagamentoExternoOriginal.dataAdquirente).ToString("HHmmss");

        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    if ((EP.intencaoVenda != null))
        //    {
        //        if (string.IsNullOrEmpty(EP.intencaoVenda.referencia))
        //        {
        //            transactionId = EP.id;
        //        }
        //        else
        //        {
        //            transactionId = EP.intencaoVenda.referencia;
        //        }

        //        if ((EP.intencaoVenda.valorFinal != null))
        //        {
        //            amount = EP.intencaoVenda.valorFinal.ToString.Replace(".", "").Replace(",", "");
        //        }

        //    }

        //    if (!string.IsNullOrEmpty(EP.adquirente))
        //    {
        //        acquirer = EP.adquirente;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            acquirer = EP.pagamentoExternoOriginal.adquirente;

        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }

        //    short ret = 99;
        //    int op = 0;

        //    //Pagamento
        //    if (EP.tipo == 5)
        //    {
        //        op = 1;
        //        //Cancelamento
        //    }
        //    else if (EP.tipo == 10)
        //    {
        //        op = 4;
        //        //Administrativa
        //    }
        //    else if (EP.tipo == 15)
        //    {
        //        op = 2;
        //    }

        //    //Dim terminal As TerminalInformation = terminalsConfig.Find(Function(x) x.TerminalId.Equals(terminalId.ToString))

        //    ClassPOSPGW.PTI_EFTStart(terminalId, op, ret);
        //    if (ret != 0)
        //    {
        //        return ret;
        //    }
        //    ret = 99;
        //    ClassPOSPGW.PTI_EFTAddParam(terminalId, 1, transactionId, ret);
        //    //Id da Transacao
        //    if (ret != 0)
        //    {
        //        return ret;
        //    }
        //    ret = 99;
        //    ClassPOSPGW.PTI_EFTAddParam(terminalId, 6, amount.ToString, ret);
        //    //Valor
        //    if (ret != 0)
        //    {
        //        return ret;
        //    }
        //    ret = 99;
        //    ClassPOSPGW.PTI_EFTAddParam(terminalId, 7, "986", ret);
        //    //Currency
        //    if (ret != 0)
        //    {
        //        return ret;
        //    }


        //    if (op == 4)
        //    {
        //        ret = 99;
        //        ClassPOSPGW.PTI_EFTAddParam(terminalId, 12, EP.pagamentoExternoOriginal.nsuTid, ret);
        //        //PGWPAR_REFHOST da transacao original
        //        if (ret != 0)
        //        {
        //            return ret;
        //        }
        //        ret = 99;
        //        ClassPOSPGW.PTI_EFTAddParam(terminalId, 13, EP.pagamentoExternoOriginal.autorizacao, ret);
        //        //PGWPAR_AUTORIZ da transacao original
        //        if (ret != 0)
        //        {
        //            return ret;
        //        }
        //        ret = 99;
        //        ClassPOSPGW.PTI_EFTAddParam(terminalId, 26, transactionDate + transactionTime, ret);
        //        //PGWPAR_ORIGTIME da transacao original (AAMMDDhhmmss)
        //        if (ret != 0)
        //        {
        //            return ret;
        //        }

        //    }

        //    ret = 99;
        //    ClassPOSPGW.PTI_EFTAddParam(terminalId, 16, acquirer, ret);
        //    //Acquirer
        //    if (ret != 0)
        //    {
        //        return ret;
        //    }

        //    ret = 99;
        //    ClassPOSPGW.PTI_EFTAddParam(terminalId, 28, Conversion.Int(transactionType).ToString, ret);
        //    //CARDType
        //    if (ret != 0)
        //    {
        //        return ret;
        //    }
        //    ret = 99;
        //    ClassPOSPGW.PTI_EFTAddParam(terminalId, 29, Conversion.Int(transactionInstallmentType).ToString, ret);
        //    //FINType
        //    if (ret != 0)
        //    {
        //        return ret;
        //    }
        //    ret = 99;
        //    ClassPOSPGW.PTI_EFTAddParam(terminalId, 30, transactionInstallments.ToString, ret);
        //    //Installments
        //    if (ret != 0)
        //    {
        //        return ret;
        //    }
        //    ret = 99;
        //    ClassPOSPGW.PTI_EFTExec(terminalId, ret);
        //    return ret;

        //}

        #endregion

    }
}
