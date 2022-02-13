using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Pagamento.NTKPos
{
    public sealed class Const
    {
        public const string Path = @"C:\PayGo"; // Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PayGoPOS";
        public const int sleepTime = 100;

        internal const int maxTerminals = 10;
        internal const int autoDisconect = 0;
        internal const string appCompany = "PDVSeven";
        internal const string waitMessage = @"      PDVSeven
    POS Integrado";
    }
}
