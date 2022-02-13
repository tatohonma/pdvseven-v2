using a7D.PDV.EF;
using a7D.PDV.EF.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BLL
{
    public class Logs
    {
        #region Rotinas internas

        private static string _fonte = "Application";

        private static void EscreverLog(string mensagem, EventLogEntryType categoria)
        {
            try
            {
                using (var eventLog = new EventLog())
                {
                    eventLog.Source = _fonte;
                    if (mensagem.Length >= 10000) // 0x7FFF inteiro 16 dit sinalizado
                        mensagem = mensagem.Substring(0, 10000) + "...";

                    eventLog.WriteEntry(mensagem, categoria);
                }
            }
            catch (Exception)
            {
            }
        }

        internal static void VerificarExistenciaFonte()
        {
            try
            {
                _fonte = "PDV7";

                if (!EventLog.SourceExists(_fonte))
                {
                    using (var eventLog = new EventLog())
                    {
                        eventLog.Source = _fonte;
                        eventLog.WriteEntry("Criando fonte: " + _fonte, EventLogEntryType.SuccessAudit);
                    }
                }
            }
            catch (SecurityException ex)
            {
                _fonte = "Application";
                throw new Exception("Execute como administrador na primeira vez,\npara criar a fonte nos logs do Windows", ex);
            }
            catch (Exception ex)
            {
                _fonte = "Application";
                throw ex;
            }

            try
            {
                _fonte = "PDV7-" + AC.NomeAplicacao;

                if (!EventLog.SourceExists(_fonte))
                {
                    using (var eventLog = new EventLog())
                    {
                        eventLog.Source = _fonte;
                        eventLog.WriteEntry("Criando fonte: " + _fonte, EventLogEntryType.SuccessAudit);
                    }
                }
            }
            catch (Exception)
            {
                _fonte = "PDV7";
            }
        }

        private static void CriarFonte(string fonte)
        {
            using (var eventLog = new EventLog())
            {
                eventLog.Source = fonte;
                eventLog.WriteEntry("Criando fonte...", EventLogEntryType.Information);
            }
        }

        #endregion

        private static String Registra(string codigo, string titulo, string dados, int? pdv = null, int? usuario = null)
        {
            try
            {
                if (titulo.Length > 200)
                    titulo = titulo.Substring(0, 200);

                var log = new tbLOGInformation
                {
                    Data = DateTime.Now,
                    Aplicacao = AC.Aplicacao ?? "?",
                    Versao = AC.Versao ?? "?",
                    Codigo = codigo,
                    Titulo = titulo,
                    Dados = dados,
                    IDUsuario = usuario ?? AC.Usuario?.IDUsuario,
                    IDPDV = pdv ?? AC.PDV?.IDPDV
                };

                Repositorio.Inserir(log);

                return "OK";
            }
            catch (Exception ex)
            {
                return "ERRO AO INSERIR LOG: " + ex.Message;
            }
        }

        public static void Info(CodigoInfo codigo, string dados = null, string titulo = null, int? pdv = null, int? usuario = null)
        {
            string cod = codigo.ToString();

            if (titulo == null)
            {
                var infoCodeField = typeof(CodigoInfo).GetField(cod);
                if (infoCodeField.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] infoName && infoName.Length == 1)
                    titulo = infoName[0].Description;
                else
                    titulo = "?";
            }

            string info = Registra(codigo.ToString(), titulo, dados, pdv, usuario);
            if (info != "OK")
            {
                titulo += "\r\n\r\n" + info;
                if (dados != null)
                    titulo += "\r\n\r\n" + dados.ToString();

                EscreverLog("ERRO AO REGISTRAR: " + titulo, EventLogEntryType.Error);
            }
        }

        public static void Erro(CodigoErro erro, string dado, string info = null)
        {
            var assemblyCalling = Assembly.GetCallingAssembly().ManifestModule.Assembly;
            var exPDV = new ExceptionPDV(erro, info);
            exPDV.Data.Add("dado", dado);
            Erro(exPDV, assemblyCalling);
        }

        public static void Erro(CodigoErro erro, Exception ex, string info = null)
        {
            var assemblyCalling = Assembly.GetCallingAssembly().ManifestModule.Assembly;
            var exPDV = new ExceptionPDV(erro, ex, info);
            Erro(exPDV, assemblyCalling);
        }

        public static void Erro(Exception ex, Assembly assemblyCalling = null)
        {
            if (assemblyCalling == null)
                assemblyCalling = Assembly.GetCallingAssembly().ManifestModule.Assembly;

            string mensagem = assemblyCalling.ToString().Replace(", Culture=neutral, PublicKeyToken=null", "");

            ExceptionPDV exPDV;
            EventLogEntryType eType = EventLogEntryType.Error;
            if (ex is ExceptionPDV)
            {
                exPDV = (ExceptionPDV)ex;
                string erroCodigo = exPDV.CodigoErro.ToString().ToUpper();
                if (erroCodigo.StartsWith("A") || erroCodigo.StartsWith("B")) // Erros de usuário (sem relevancia)
                    eType = EventLogEntryType.Warning;
            }
            else
                exPDV = new ExceptionPDV(CodigoErro.E100, ex);

            if (exPDV.CodigoErro == CodigoErro.AAAA) //Ignorar
                return;

            while (ex != null)
            {
                mensagem += $"\r\n{ex.GetType().Name}: {ex.Message} \r\n{ex.StackTrace}\r\n";
                if (ex.Data?.Count > 0)
                {
                    foreach (var key in ex.Data.Keys)
                        mensagem += $"\r\n\t{key}: {ex.Data[key]}";

                    mensagem += "\r\n";
                }
                ex = ex.InnerException;
            }

            if (AC.Chave != null)
            {
                string info = Registra(exPDV.CodigoErro.ToString(), exPDV.Message, mensagem);
                if (info != "OK")
                    mensagem += "\r\n\r\n" + info;
            }

            EscreverLog(mensagem, eType);

            if (eType == EventLogEntryType.Error)
            {
                GA.PostErro(exPDV.CodigoErro, exPDV.Message);
                exPDV.SendAsync();
            }
        }

        public static void ErroBox(ExceptionPDV exPDV, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            try
            {
                if (exPDV.CodigoErro == CodigoErro.AAAA) //Ignorar
                    return;

                string erroCodigo = exPDV.CodigoErro.ToString().ToUpper();
                string erroMessage = exPDV.Message;

                if (icon == MessageBoxIcon.None)
                {
                    if (erroCodigo.StartsWith("E"))
                        icon = MessageBoxIcon.Error;
                    else if (erroCodigo.StartsWith("W"))
                        icon = MessageBoxIcon.Warning;
                    else if (erroCodigo.StartsWith("A") || erroCodigo.StartsWith("B"))
                        icon = MessageBoxIcon.Exclamation;
                }

                if (icon == MessageBoxIcon.Error)
                    erroMessage += "\nPara mais informações consulte o log do Windows";

                Erro(exPDV, Assembly.GetCallingAssembly().ManifestModule.Assembly);
                MessageBox.Show(erroMessage, "ERRO", MessageBoxButtons.OK, icon);
            }
            catch (Exception ex2)
            {
                if (exPDV == null)
                    Erro(ex2);
                else
                {
                    exPDV.Data["ex2.Message"] = ex2.Message;
                    exPDV.Data["ex2.StackTrace"] = ex2.StackTrace;
                    Erro(exPDV);
                }
            }
        }

        public static void ErroBox(CodigoErro erro, Exception ex = null, MessageBoxIcon icon = MessageBoxIcon.None, string info = null)
        {
            ExceptionPDV exPDV;
            if (ex == null)
                exPDV = new ExceptionPDV(erro, info);
            else if (ex is ExceptionPDV ex2)
                exPDV = ex2;
            else
                exPDV = new ExceptionPDV(erro, ex, info);

            ErroBox(exPDV, icon);
        }
    }
}
