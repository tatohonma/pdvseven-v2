using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace a7D.PDV.Iaago.Dialogo
{
    public static class Interpretador
    {
        // https://regex101.com/
        private static readonly Regex erMensagem = new Regex(@"(\@+\w+(\w+|\.)*)(\('([^']*)'\))*");
        private static readonly Regex erComapracao = new Regex(@"^\s*([^=><!]+)([=><!]+)([^=><!]+)\s*$");
        private static readonly Regex erVariavel = new Regex(@"\@+(\w+)\s*=\s*(.*)$", RegexOptions.Multiline);
        private static readonly Regex erMatematica = new Regex(@"^\s*([^+\-%\/* ]+)\s*([+\-%\/*])(.*)$");

        private static readonly string[] sim = "sim,s,yes,y,ok,certo,pode,correto,claro,positivo".Split(',');
        private static readonly string[] nao = "nao,n,no,negativo".Split(',');
        private static readonly string[] meses = "janeiro,fevereiro,marco,abril,maio,junho,julho,agosto,setembro,outubro,novembro,dezembro".Split(',');

        public static string RemoveAcentos(this string valor)
        {
            // 6.1 - Exceto por estes dois caracteres de controle, os demais caracteres pertencem à faixa de caracteres ASCII de 20h (decimal 32) a 7Eh (decimal 126), não sendo permitidos caracteres acentuados.
            return new string(valor
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        public static string SimNao(string valor)
        {
            valor = valor.ToLower().RemoveAcentos();
            if (sim.Contains(valor))
            {
                return "sim";
            }
            else if (nao.Contains(valor))
            {
                return "nao";
            }
            else
                return null;
        }

        internal static bool ObterDateTime(string valor, out DateTime dt1, out DateTime dt2)
        {
            var dtHoje = DateTime.Now.Date;
            valor = valor.ToLower().RemoveAcentos();
            if (valor == "hoje")
            {
                dt1 = dtHoje;
            }
            else if (valor == "amanha")
            {
                dt1 = dtHoje.AddDays(1);
            }
            else if (valor == "ontem")
            {
                dt1 = dtHoje.AddDays(-1);
            }
            else if (valor == "mes" || valor == "atual" || valor == "vigente")
            {
                dt1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dt2 = dt1.AddMonths(1);
                return true;
            }
            else if (valor.Length >= 3 && meses.Any(m => m.StartsWith(valor)))
            {
                int nMes = 1;
                foreach (var mes in meses)
                {
                    if (mes.StartsWith(valor))
                    {
                        dt1 = new DateTime(DateTime.Now.Year, nMes, 1);
                        dt2 = dt1.AddMonths(1);
                        return true;
                    }
                    nMes++;
                }
                dt1 = dt2 = DateTime.MinValue;
                return false;
            }
            else if (Regex.Match(valor, @"^\d{4}$").Success && int.TryParse(valor, out int ano))
            {
                dt1 = new DateTime(ano, 1, 1);
                dt2 = dt1.AddYears(1);
                return true;
            }
            else if (Regex.Match(valor, @"^\d{4}-\d{2}-\d{2}$").Success && DateTime.TryParse(valor, out DateTime dtA))
            {
                dt1 = dtA;
            }
            else if (Regex.Match(valor, @"^\d{2}/\d{2}/\d{2,4}$").Success && DateTime.TryParse(valor, out DateTime dtB))
            {
                dt1 = dtB;
            }
            else if (Regex.Match(valor, @"^\d{2}/\d{2}$").Success && DateTime.TryParse(valor, out DateTime dtC))
            {
                dt1 = dtC;
            }
            else
            {
                dt1 = dt2 = DateTime.MinValue;
                return false;
            }
            dt2 = dt1.AddDays(1);
            return true;
        }

        public static void ExecutarAtribuicoes(string[] atribuir, Action<string, string> addVar)
        {
            var m = erVariavel.Match(string.Join("\n", atribuir));
            while (m.Success)
            {
                if (m.Groups.Count == 3)
                {
                    string key = m.Groups[1].Value.Trim();
                    string value = m.Groups[2].Value.Trim();
                    addVar(key, value);
                }

                m = m.NextMatch();
            }
        }

        public static string FormataMensagem(string mensagem, Func<string, object> obtemVariavel)
        {
            if (string.IsNullOrEmpty(mensagem))
            {
                return string.Empty;
            }

            var finalMessage = mensagem;
            var m = erMensagem.Match(finalMessage);
            while (m.Success)
            {
                string keyfull = m.Groups[0].Value; // Com a mascara!
                string keyvar = m.Groups[1].Value.ToLower(); // Só o nome da variável
                var valor = obtemVariavel(keyvar);
                try
                {
                    string str;
                    if (m.Groups.Count == 5 && !string.IsNullOrEmpty(m.Groups[4].Value))
                    {
                        // Remove o | e as aspas
                        string format = m.Groups[4].Value;
                        format = "{0:" + format + "}";
                        str = string.Format(format, valor);
                    }
                    else
                    {
                        str = valor?.ToString() ?? string.Empty;
                    }

                    finalMessage = finalMessage.Replace(keyfull, str);
                    m = erMensagem.Match(finalMessage);
                }
                catch (Exception ex)
                {
                    finalMessage = finalMessage.Replace(keyvar, "!ERRO!");
                    ex.Data.Add("mensagem", mensagem);
                    ex.Data.Add("keyfull", keyfull);
                    ex.Data.Add("key", keyvar);
                    ex.Data.Add("valor", valor);
                    throw ex;
                }
            }

            return finalMessage.Replace("@ ", "@"); // '@ ' é @ como texto e não variável
        }

        private static object ExpressaoOuVariavel(string expressao, Func<string, object> obtemVariavel)
        {
            if (expressao.StartsWith("@"))
            {
                return obtemVariavel(expressao);
            }
            else
            {
                return SimpleVar(expressao);
            }
        }

        public static object Calcula(string expressao, Func<string, object> obtemVariavel)
        {

            var m = erMatematica.Match(expressao);
            if (!m.Success)
            {
                return ExpressaoOuVariavel(expressao, obtemVariavel);
            }

            object v1 = ExpressaoOuVariavel(m.Groups[1].Value, obtemVariavel);
            var operador = m.Groups[2].Value.Trim();
            object v2 = ExpressaoOuVariavel(m.Groups[3].Value, obtemVariavel);

            if (v1 is int i1 && v2 is int i2)
            {
                if (operador == "+")
                {
                    return i1 + i2;
                }
                else if (operador == "-")
                {
                    return i1 - i2;
                }
                else if (operador == "/")
                {
                    return i1 / i2;
                }
                else if (operador == "*")
                {
                    return i1 * i2;
                }
                else if (operador == "%")
                {
                    return i1 % i2;
                }
            }
            else if (v1 is string s1 && v2 is string s2)
            {
                if (operador == "+")
                {
                    return s1 + s2;
                }
            }
            else if (v1 is DateTime dt1 && v2 is int idt2)
            {
                if (operador == "+")
                {
                    return dt1.AddDays(idt2);
                }
                else if (operador == "-")
                {
                    return dt1.AddDays(-idt2);
                }
            }

            return obtemVariavel(expressao);
        }

        public static bool ValidaCondicao(string condicao, string resultado, Func<string, object> obtemVariavel)
        {
            if (condicao == null)
                return true;

            var m = erComapracao.Match(condicao);
            if (!m.Success || m.Groups.Count != 4)
            {
                return condicao.Equals(resultado, StringComparison.CurrentCultureIgnoreCase);
            }

            object v1 = Calcula(m.Groups[1].Value, obtemVariavel);
            var operador = m.Groups[2].Value.Trim();
            object v2 = Calcula(m.Groups[3].Value, obtemVariavel);

            if (operador == "==")
            {
                return v1?.ToString() == v2?.ToString();
            }
            else if (operador == "!=")
            {
                return v1?.ToString() != v2?.ToString();
            }
            else if (v1 is int i1 && v2 is int i2)
            {
                if (operador == ">=")
                {
                    return i1 >= i2;
                }
                else if (operador == "<=")
                {
                    return i1 <= i2;
                }
                else if (operador == ">")
                {
                    return i1 > i2;
                }
                else if (operador == "<")
                {
                    return i1 < i2;
                }
            }

            return false;
        }

        public static object SimpleVar(string stringValue)
        {
            if (stringValue == null || stringValue == "null")
            {
                return null;
            }
            else if (stringValue.StartsWith("'") && stringValue.EndsWith("'"))
            {
                if (stringValue.Length > 2)
                {
                    return stringValue.Substring(1, stringValue.Length - 2);
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (Regex.Match(stringValue, @"^\d+$").Success && int.TryParse(stringValue, out int n))
            {
                return n;
            }
            else if (Regex.Match(stringValue, @"^\d+\.\d+$").Success && double.TryParse(stringValue, out double d))
            {
                return d;
            }
            else if (Regex.Match(stringValue, @"^XXXX-\d{2}$").Success)
            {
                int mes = int.Parse(stringValue.Substring(5, 2));
                int ano = DateTime.Now.Year;

                // Prioriza a busca no passado, no que já aconteceu
                if (mes > DateTime.Now.Month)
                {
                    ano--;
                }
                return new DateTime(ano, mes, 1);
            }
            else if (Regex.Match(stringValue, @"^\d{4}-\d{2}-\d{2}$").Success && DateTime.TryParse(stringValue, out DateTime dt))
            {
                return dt;
            }
            else
            {
                return stringValue;
            }
        }
    }
}
