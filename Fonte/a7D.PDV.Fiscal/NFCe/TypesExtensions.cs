using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using System;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

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
        
        public static T ToZeusEnum<T>(this string value) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            value = value.Trim();

            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var xmlEnum = field.GetCustomAttributes(typeof(XmlEnumAttribute), false)
                    .Cast<XmlEnumAttribute>()
                    .FirstOrDefault();

                if (xmlEnum != null && xmlEnum.Name == value)
                    return (T)field.GetValue(null);
            }

            throw new ArgumentException(
                $"Valor '{value}' não corresponde a nenhum XmlEnum em {typeof(T).Name}"
            );
        }
    }
}
