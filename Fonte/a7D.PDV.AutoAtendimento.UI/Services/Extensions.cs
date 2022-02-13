using a7D.PDV.AutoAtendimento.UI.Properties;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal static class Extensions
    {
        internal static void ConfigScreen(this Window window)
        {
            var screens = System.Windows.Forms.Screen.AllScreens;
            if (screens.Length > 1 && Settings.Default.Tela <= screens.Length && Settings.Default.Tela > 0)
            {
                var sDest = screens[Settings.Default.Tela - 1];
                var rDest = sDest.WorkingArea;
                window.Top = rDest.Top;
                window.Left = rDest.Left;
            }
        }

        internal static string SoNumeros(this string texto)
        {
            string numeros = "";
            var m = Regex.Match(texto, @"\d+");
            while (m.Success)
            {
                numeros += m.Groups[0].Value;
                m = m.NextMatch();
            }
            return numeros;
        }

        internal static Boolean IsCpf(string cpf)
        {
            if (cpf == null ||
                cpf.Length != 11 ||
                cpf == "00000000000" ||
                cpf == "11111111111" ||
                cpf == "22222222222" ||
                cpf == "33333333333" ||
                cpf == "44444444444" ||
                cpf == "55555555555" ||
                cpf == "66666666666" ||
                cpf == "77777777777" ||
                cpf == "88888888888" ||
                cpf == "99999999999")
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            else if (!System.Text.RegularExpressions.Regex.IsMatch(cpf, @"\d+"))
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}
