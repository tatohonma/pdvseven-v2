using a7D.PDV.BLL;
using a7D.PDV.Model;
using System.Data;

namespace a7D.PDV.Caixa.UI
{
    static class ComandaUtil
    {
        internal static ComandaInformation CarregarPorNumeroOuCodigo(string text)
        {
            return Comanda.CarregarPorNumeroOuCodigoOuHex(text, ConfiguracoesCaixa.Valores.ComandaCodigoHEX);
        }

        internal static DataTable ListarAbertas(string texto, string codigo)
        {
            return Comanda.ListarAbertas(texto, codigo, ConfiguracoesCaixa.Valores.ComandaCodigoHEX);
        }
    }
}
