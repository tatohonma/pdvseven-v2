using a7D.PDV.BLL;
using System;

namespace a7D.PDV.Integracao.WS
{
    public static class Util
    {
        public static String Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        internal static string RetornaErro(Exception ex)
        {
            Logs.Erro(ex);
            string xmlRetorno = "";
            string cErro = "";
            while (ex != null)
            {
                if (ex is ExceptionPDV) // Erro mapeado, tem peferencia!
                    cErro = ex.Message + "\n";
                else
                    cErro += ex.Message + "\n";

                ex = ex.InnerException;
            }

            cErro = cErro
                .Replace("&", " ")
                .Replace("<", "[")
                .Replace(">", "]")
                .Replace("\"", "-")
                .Replace("'", " ");

            xmlRetorno += "  <versao>" + Versao + "</versao>";
            xmlRetorno += "  <status>0</status>";
            xmlRetorno += "  <retorno>";
            xmlRetorno += "    <descricaoErro>" + cErro + "</descricaoErro>";
            xmlRetorno += "    <descricaoDetalhadaErro></descricaoDetalhadaErro>";
            xmlRetorno += "  </retorno>";

            return xmlRetorno;
        }
    }
}