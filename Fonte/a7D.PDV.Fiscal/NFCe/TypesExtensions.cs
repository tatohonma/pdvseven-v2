using System;
using System.Globalization;
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
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            var raw = value.Trim();

            // primeiro tenta interpretar como "4.00", "0.65", "3.00" (ponto como decimal)
            if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;

            // fallback: cultura atual (caso em algum lugar venha "4,00")
            return decimal.Parse(raw, NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        public static decimal ToAliquota(this string value)
        {
            return value.ToDecimal();
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