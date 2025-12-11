using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using System;
using System.Xml.Serialization;

namespace a7D.PDV.Fiscal.NFCe
{
    public static class TypesExtensions
    {
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        
        public static T ToEnumFromXml<T>(this string value) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Valor vazio para enum " + typeof(T).Name);

            var tipo = typeof(T);

            if (Enum.TryParse<T>(value, ignoreCase: true, out var result))
                return result;

            foreach (var field in tipo.GetFields())
            {
                var attr = Attribute.GetCustomAttribute(field, typeof(XmlEnumAttribute))
                    as XmlEnumAttribute;

                if (attr != null && attr.Name == value)
                    return (T)field.GetValue(null);
            }

            if (int.TryParse(value, out var intVal) && Enum.IsDefined(tipo, intVal))
                return (T)Enum.ToObject(tipo, intVal);

            throw new ArgumentException($"'{value}' não é um valor válido para enum {tipo.Name}");
        }

        public static decimal ToDecimal(this string value)
        {
            return decimal.Parse(value);
        }
    }
}
