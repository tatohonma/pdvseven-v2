using System.Text.RegularExpressions;

namespace a7D.PDV.Integracao.EasyChopp
{
    internal static class Extensions
    {
        internal static string SoNumeros(this string texto)
        {
            if (texto == null)
                return "";

            string numeros = "";
            var m = Regex.Match(texto, @"\d+");
            while (m.Success)
            {
                numeros += m.Groups[0].Value;
                m = m.NextMatch();
            }
            return numeros;
        }
    }
}
