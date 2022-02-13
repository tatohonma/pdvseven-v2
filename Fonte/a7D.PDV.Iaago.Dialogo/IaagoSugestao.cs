using System;

namespace a7D.PDV.Iaago.Dialogo
{
    public class IaagoSugestao
    {
        public string titulo { get; set; }
        public string texto { get; set; }

        public override string ToString() => $"{titulo}: {texto}";
    }
}
