using System.Globalization;
using System.Linq;
using System.Text;

namespace a7D.PDV.Integracao.Pagamento
{
    public static class Extender
    {
        public static string RemoveAcentos(this string valor)
        {
            // 6.1 - Exceto por estes dois caracteres de controle, os demais caracteres pertencem à faixa de caracteres ASCII de 20h (decimal 32) a 7Eh (decimal 126), não sendo permitidos caracteres acentuados.
            return new string(valor
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }
    }
}
