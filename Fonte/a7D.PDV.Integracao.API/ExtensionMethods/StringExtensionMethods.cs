using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.ExtensionMethods
{

    public static class StringExtensionMethods
    {
        private const string _format = "yyyyMMddHHmmssfff";

        public static DateTime ConverterData(this string strData)
        {
            DateTime ret = DateTime.MinValue;
            DateTime.TryParseExact(strData, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out ret);
            if (ret != DateTime.MinValue)
                return ret;
            throw new ArgumentException($"Não foi possível converter a data {strData}");
        }
    }
}