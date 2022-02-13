using System;

namespace a7D.PDV.Integracao.EasyChopp.Model
{
    public class Retorno
    {
        public string dsError { get; set; }
        public bool stIntegracao { get; set; }
        public double? vlSaldoAtual { get; set; }

        public override string ToString()
        {
            return stIntegracao ? $"OK {vlSaldoAtual}" : dsError;
        }
    }
}
