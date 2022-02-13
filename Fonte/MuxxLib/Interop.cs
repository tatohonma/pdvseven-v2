using MuxxLib.Estruturas;
using System.Runtime.InteropServices;
using System.Text;

namespace MuxxLib
{
    public class Interop
    {
        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iInit(string pszWorkingDir);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iNewTransac(byte bOper);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iAddParam(ushort wParam, string pszValue);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iExecTransac([Out] PW_GetData[] vstParam, ref short piNumParam);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iGetResult(short iInfo, [Out] StringBuilder pszData, uint ulDataSize);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iConfirmation(uint ulResult, string pszReqNum, string pszLocRef,
                                               string pszExtRef, string pszVirtMerch, string pszAuthSyst);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iIdleProc();

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iGetOperations(byte bOperType, ref PW_Operations[] vstOperations, ref short piNumOperations);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPEventLoop([Out] StringBuilder pszDisplay, uint ulDisplaySize);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPAbort();

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPGetCard(ushort uiIndex);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPGetPIN(ushort uiIndex);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPGetData(ushort uiIndex);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPGoOnChip(ushort uiIndex);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPFinishChip(ushort uiIndex);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPConfirmData(ushort uiIndex);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPRemoveCard();

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPDisplay(string pszMsg);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPWaitEvent(ref uint pulEvent);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPGenericCMD(ushort uiIndex);

        [DllImport(Constantes.PGWebLibAdress, CallingConvention = CallingConvention.StdCall)]
        public static extern short PW_iPPPositiveConfirmation(ushort uiIndex);


        public static short iGetResult(short iInfo, ref StringBuilder pszData, uint ulDataSize)
        {
            pszData = new StringBuilder(10000); //10000 SUGESTÃO DO 'Y'
                                                //string s = null;

            return PW_iGetResult(iInfo, pszData, ulDataSize);
            //var i = PW_iGetResult(iInfo, pszData, ulDataSize);
            //pszData.Append(s);
            //return i;
        }
    }
}
