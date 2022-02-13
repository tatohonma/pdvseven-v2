using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using System;

namespace a7D.PDV.Fiscal.NFCe
{
    public static class TypesExtensions
    {
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static decimal ToDecimal(this string value)
        {
            return decimal.Parse(value);
        }
    }
}
