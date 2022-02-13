using System;
using System.ComponentModel;
using System.Globalization;

namespace a7D.PDV.Ativacao.Shared.Services
{
    public class EnumTypeConverter<TEnum> : TypeConverter where TEnum : struct
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string valor)
            {
                if (Enum.TryParse(valor, out TEnum enValue))
                    return enValue;
                else
                    throw new Exception($"{valor} não aceito no conversor de '{typeof(TEnum).Name}'");
            }
            else
                throw new Exception($"tipo '{value}' não reconhecido no conversor de '{typeof(TEnum).Name}'");
        }
    }
}