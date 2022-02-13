using System;
using System.Web.Http.Controllers;

namespace a7D.PDV.Ativacao.API.Extensions
{
    public static class DateTimeExtensionMethods
    {
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static bool IsLocal(this HttpActionContext context)
        {
            return context.Request.Properties["MS_IsLocal"] is Lazy<bool> localFlag && localFlag.Value;
        }

    }
}