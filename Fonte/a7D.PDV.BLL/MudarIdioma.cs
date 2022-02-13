using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace a7D.PDV.BLL
{
    public static class MudarIdioma
    {
        public static void Mudar(string novoIdioma)
        {
            string idioma = "pt-BR";
            if (!string.IsNullOrWhiteSpace(novoIdioma))
                idioma = novoIdioma;

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(idioma);
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(idioma);
        }
    }
}
