using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Ativacao.API.Services
{
    public static class DateTimeExtensions
    {
        public static DateTime AdicionarDiasUteis(this DateTime data, int dias)
        {
            if (dias < 0)
            {
                throw new ArgumentException("dias não podem ser negativos", "dias");
            }

            if (dias == 0) return data;

            if (data.DayOfWeek == DayOfWeek.Saturday)
            {
                data = data.AddDays(2);
                dias -= 1;
            }
            else if (data.DayOfWeek == DayOfWeek.Sunday)
            {
                data = data.AddDays(1);
                dias -= 1;
            }

            data = data.AddDays(dias / 5 * 7);
            int extraDays = dias % 5;

            if ((int)data.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }

            return data.AddDays(extraDays);
        }
    }
}