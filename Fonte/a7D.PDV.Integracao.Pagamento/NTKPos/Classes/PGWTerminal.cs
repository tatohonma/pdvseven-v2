using System.Runtime.InteropServices;
using System.Text;

namespace a7D.PDV.Integracao.Pagamento.NTKPos
{
    public class ClassPOSPGW
    {

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_Init(string pszPOS_Company, string pszPOS_Version, string pszPOS_Capabilities, string pszDataFolder,
            int uiTCP_Port, int uiMaxTerminals, string pszWaitMsg, int uiAutoDiscSec,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_End();

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_ConnectionLoop(StringBuilder pszTerminalId, StringBuilder pszModel, StringBuilder pszMAC, StringBuilder pszSerNo,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_CheckStatus(string pszTerminalId, [MarshalAs(UnmanagedType.U2)] ref ushort piStatus,
            StringBuilder pszModel, StringBuilder pszMAC, StringBuilder pszSerNo,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_Display(string uiTerminalId, string pszMsg,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_Disconnect(string uiTerminalId, short uiPwrDelay,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_GetData(string uiTerminalId, string pszPrompt, string pszFormat, short uiLenMin,
            short uiLenMax, bool fFromLeft, bool fAlpha, bool fMask, short uiTimeOutSec, StringBuilder pszData, short uiCaptureLine,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_Print(string uiTerminalId, string text,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_PrnFeed(string uiTerminalId, 
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_EFT_AddParam(string uiTerminalId, int iParam, string pszValue,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_EFT_Confirm(string uiTerminalId, int iResult,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_EFT_Exec(string uiTerminalId, 
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_EFT_GetInfo(string uiTerminalId, int iInfo, int uiBufLen, StringBuilder pszValue,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_EFT_PrintReceipt(string uiTerminalId, int iCopies,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_PrnSymbolCode(string uiTerminalId, string pszMsg,
            short iSymbology, ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_EFT_Start(string uiTerminalId, int iOper,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_StartMenu(string terminalId, 
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_AddMenuOption(string terminalId, string pszOption,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);

        [DllImport("PTI_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PTI_ExecMenu(string terminalId, string pszPrompt, short uiTimeOutSec,
            ref short puiSelection,
            [MarshalAs(UnmanagedType.I2)] ref short piRet);



    }
}
