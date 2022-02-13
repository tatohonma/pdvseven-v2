using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal static class EventLogServices
    {
        private static string _fonte = "PDV7-Autoatendimento";

        public static void EscreverLog(string mensagem, EventLogEntryType categoria = EventLogEntryType.Information)
        {
            try
            {
                using (var eventLog = new EventLog())
                {
                    eventLog.Source = _fonte;
                    eventLog.WriteEntry(mensagem, categoria);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void VerificarExistenciaFonte()
        {
            try
            {
                if (EventLog.SourceExists(_fonte))
                    return;

                using (var eventLog = new EventLog())
                {
                    eventLog.Source = _fonte;
                    eventLog.WriteEntry("Criando fonte: " + _fonte, EventLogEntryType.SuccessAudit);
                }
            }
            catch (SecurityException ex)
            {
                throw new Exception("Execute como administrador na primeira vez,\npara criar a fonte nos logs do Windows", ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SuccessAudit(string mensagem)
        {
            if (string.IsNullOrEmpty(mensagem))
                return;

            EscreverLog(mensagem, EventLogEntryType.SuccessAudit);
        }

        public static void Info(string mensagem, string fonte)
        {
            EscreverLog(mensagem, EventLogEntryType.Information);
        }

        public static void Error(Exception ex)
        {
            var assembly = Assembly.GetEntryAssembly().ManifestModule.Assembly;

            string mensagem = assembly.ToString().Replace(", Culture=neutral, PublicKeyToken=null", "");

            while (ex != null)
            {
                mensagem += $"\r\n{ex.GetType().Name}: {ex.Message} \r\n{ex.StackTrace}\r\n";
                if (ex.Data?.Count > 0)
                {
                    foreach (var key in ex.Data.Keys)
                        mensagem += $"\r\n\t{key}: {ex.Data[key]}";
                }
                ex = ex.InnerException;
            }

            EscreverLog(mensagem, EventLogEntryType.Error);
        }
    }
}
