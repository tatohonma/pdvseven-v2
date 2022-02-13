using a7D.PDV.Model;
using System.Collections.Generic;

namespace a7D.PDV.BLL
{
    public static class ConfiguracaoExtensions
    {

        public static IEnumerable<string> ListaValoresAceitos(this ConfiguracaoBDInformation config)
        {
            if (string.IsNullOrWhiteSpace(config?.ValoresAceitos))
                yield break;

            var valores = config.ValoresAceitos.Split('|');
            foreach (var valor in valores)
            {
                var indice = valor.IndexOf(":");
                if (indice >= 0)
                    yield return valor.Substring(0, indice);
                else
                    yield return valor;
            }
        }

        public static IEnumerable<string> ListaTitulosAceitos(this ConfiguracaoBDInformation config)
        {
            if (string.IsNullOrWhiteSpace(config?.ValoresAceitos))
                yield break;
            var valores = config.ValoresAceitos.Split('|');
            foreach (var valor in valores)
            {
                var indice = valor.IndexOf(":");
                if (indice >= 0)
                    yield return valor.Substring(indice + 1);
                else
                    yield return valor;
            }
        }

        public static bool ConfiguracaoSistema(this ConfiguracaoBDInformation config)
        {
            return config.TipoPDV == null && config.PDV == null;
        }

        public static bool ConfiguracaoPadrao(this ConfiguracaoBDInformation config)
        {
            return config.TipoPDV?.IDTipoPDV != null && config.PDV?.IDPDV == null;
        }
    }
}
