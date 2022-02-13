using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL.Extension
{
    public static class StringExtensions
    {
        public static void Separador(this StringBuilder conteudo, string texto, char caracter, int maxLine = Constantes.Colunas)
        {
            int n1 = (maxLine - texto.Length - 2) / 2;
            int n2 = maxLine - texto.Length - 2 - n1;

            conteudo
                .Append(caracter, maxLine)
                .AppendLine()
                .Append(caracter, n1)
                .Append(" " + texto + " ")
                .Append(caracter, n2)
                .AppendLine()
                .Append(caracter, maxLine)
                .AppendLine();
        }

        public static string Sanitizar(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return string.Empty;
            string retorno = s;

            retorno.Replace(Environment.NewLine, string.Empty);
            retorno.Replace("\n", string.Empty);
            retorno.Replace("\r", string.Empty);

            return retorno;
        }

        public static string SplitToLines(this string text, int maxStringLength)
        {
            char[] splitOnCharacters = new char[] { ' ', '\r', '\n', '-', '.' };
            return SplitToLines(text, splitOnCharacters, maxStringLength);
        }

        public static string SplitToLines(this string text, char[] splitOnCharacters, int maxStringLength)
        {
            var sb = new StringBuilder();
            var index = 0;

            while (text.Length > index)
            {
                // start a new line, unless we've just started
                if (index != 0)
                    sb.AppendLine();

                // get the next substring, else the rest of the string if remainder is shorter than 'maxStringLength'
                var splitAt = index + maxStringLength <= text.Length
                    ? text.Substring(index, maxStringLength).LastIndexOfAny(splitOnCharacters)
                    : text.Length - index;

                // if can't find split location, take 'maxStringLength` character'
                splitAt = (splitAt == -1) ? maxStringLength : splitAt;

                // add result to collection & increment index
                sb.Append(text.Substring(index, splitAt).Trim());
                index += splitAt;
            }

            return sb.ToString();
        }

        public static string[] SplitToLinesArray(this string text, int maxStringLength)
        {
            char[] splitOnCharacters = new char[] { ' ', '\r', '\n', '-', '.' };
            return SplitToLinesArray(text, splitOnCharacters, maxStringLength);
        }

        public static string[] SplitToLinesArray(this string text, char[] splitOnCharacters, int maxStringLength)
        {
            return SplitToLines(text, splitOnCharacters, maxStringLength).Split(Environment.NewLine.ToArray());
        }


        public static string RemoveDiacritics(this string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC).ToUpper();
        }

        public static bool TemCaracteresEspeciais(this string valor)
        {
            foreach (var ch in valor.Normalize(NormalizationForm.FormD))
            {
                // Caracteres que podem causar problemas
                if (ch == '\"' || ch == '\'' || ch == '&' || ch == '%')
                    return true;

                // https://www.asciitable.com/
                if (ch >= 32 && ch <= 126)
                    continue; // ASC Padrão

                // https://msdn.microsoft.com/pt-br/library/system.globalization.unicodecategory(v=vs.110).aspx
                var cat = char.GetUnicodeCategory(ch);
                if (cat != UnicodeCategory.NonSpacingMark) // Catacteres acentuados
                    return true;
            }
            return false;
        }

        public static string QuebrarEmLinhas(this string texto, int qtColunas)
        {
            if (texto.Length > qtColunas)
            {
                List<string> textoQuebrado = new List<string>();
                int qtLinhas = texto.Length / qtColunas;
                for (int i = 0; i <= qtLinhas; i++)
                {
                    textoQuebrado.Add(
                        texto.Substring(i * qtColunas,
                        (((qtColunas * i) + qtColunas) > texto.Length ?
                        texto.Length - (qtColunas * i) :
                        qtColunas)));
                }
                return string.Join("\n", textoQuebrado);
            }
            else
            {
                return texto;
            }
        }
    }
}
