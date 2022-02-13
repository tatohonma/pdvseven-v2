using System.Runtime.InteropServices;
using System.Text;

namespace a7D.PDV.Integracao.Pagamento.GranitoTEF
{
    internal static class GranitoWrapperAPI
    {
        const string diretorioDll = "c:\\pdv7\\PGBC.dll";

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_Config(string cnpj, string password, string pdv);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_Init(StringBuilder message_pp, StringBuilder message);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_Tef(StringBuilder pdv_init, StringBuilder pdv_response, StringBuilder tef_response);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_Cancel(StringBuilder pdv_request, StringBuilder tef_response);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_Close_PP(StringBuilder message);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static void PG_GetData(StringBuilder rede, StringBuilder bandeira);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static void TEF_Info(string indice, StringBuilder valor);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_Split(StringBuilder mode, StringBuilder split_buffer);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_Event(); // 0 - tecla enter, 1 - tecla cancel, 2 - qualquer outra tecla, 3 - timeout(30s)

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_GetSplitList(StringBuilder response);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_Split(StringBuilder mode, StringBuilder buffer, StringBuilder estab_cupom);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_EventMsg(StringBuilder message_pp);

        [DllImport(diretorioDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int PG_Display(StringBuilder message);

    }
}
