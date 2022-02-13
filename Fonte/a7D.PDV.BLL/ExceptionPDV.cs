using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    [Serializable]
    public class ExceptionPDV : Exception
    {
        public CodigoErro CodigoErro { get; private set; }
        public int CodigoRetorno { get; private set; }

        //public string MessageInner { get; private set; }

        private static string UltimoErroTitulo = "";
        private static DateTime UltimoErroDate = DateTime.MinValue;
        private static int UltimoErroCount = 0;

        public ExceptionPDV(CodigoErro erro, Exception ex = null, string info = null)
            : base(MessageFromCode(erro, ex, info, out string inner), ex)
        {
            //MessageInner = inner;
            if (ErroAIgnorar(inner))
                this.CodigoErro = CodigoErro.AAAA;
            else if (ex is ExceptionPDV exPDV)
                this.CodigoErro = exPDV.CodigoErro;
            else
                this.CodigoErro = erro;
        }

        public ExceptionPDV(CodigoErro erro, int codigo, string info)
            : base(MessageFromCode(erro, null, info, out string inner))
        {
            //MessageInner = inner;
            if (ErroAIgnorar(inner))
                this.CodigoErro = CodigoErro.AAAA;
            else
                this.CodigoErro = erro;

            this.CodigoRetorno = codigo;
        }

        private bool ErroAIgnorar(string msg)
        {
            return msg.Contains("SHUTDOWN")
                || msg.Contains("provider: TCP Provider")
                || msg.Contains("provider: SQL Network Interfaces")
                || msg.Contains("Execution Timeout Expired")
                || msg.Contains("Tempo Limite de Execução Expirado")
                || msg.Contains("Atingido o tempo limite da solicitação")
                || msg.Contains("O nome remoto não pôde ser resolvido")
                || msg.Contains("está sem internet")
                || msg.Contains("disposed object")
                || msg.Contains("objeto descartado");
        }

        public ExceptionPDV(CodigoErro erro, string info)
            : this(erro, null, info)
        {
        }

        private static string MessageFromCode(CodigoErro erro, Exception innerException, string info, out string inner)
        {
            string erroCodigo = erro.ToString().ToUpper();
            string erroMessage = $"[{erroCodigo}] ";

            if (innerException is ExceptionPDV exPDV)
            {
                erroCodigo = exPDV.CodigoErro.ToString().ToUpper();
                erroMessage = $"[{erroCodigo}] " + erroMessage;
            }

            inner = "";
            while (innerException != null)
            {
                string newMsg = "";
                // Tradução de algumas mensagens de erros
                if (innerException.Message.Contains("object reference not set")
                 || innerException.Message.Contains("ncia de objeto não definida"))
                    newMsg = "Ops, desculpe ocorreu um imprevisto, tente novamente.\r\n";
                else if (innerException.Message.Contains("O nome remoto não pôde ser resolvido")
                 || innerException.Message.Contains("DNS"))
                    newMsg = "Ops, provavelmente está sem internet.\r\n";
                else
                    newMsg = innerException.Message + "\r\n";

                if (!inner.Contains(newMsg))
                    inner += newMsg;

                innerException = innerException.InnerException;
            }

            string descErro = Description(erroCodigo).Replace("|", "\r\n");
            erroMessage += string.Format(descErro, inner);

            if (info != null)
                erroMessage += "\r\n" + info;

            if (erroMessage.EndsWith("\r\n"))
                erroMessage = erroMessage.Substring(0, erroMessage.Length - 2);

            return erroMessage;
        }

        public static string Description(string erro)
        {
            var erroCodeField = typeof(CodigoErro).GetField(erro);
            if (erroCodeField == null)
                return "?";

            if (erroCodeField.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] erroName
                && erroName.Length == 1)
                return erroName[0].Description;

            return "??";
        }

        public Task SendAsync()
        {
            //if (AC.Chave == null)
            //    return null;

            string titutoCurto = Message;
            if (titutoCurto.Length > 30)
                titutoCurto = titutoCurto.Substring(0, 30); // Pode ser o mesmo erro para outros dados variados

            if (UltimoErroTitulo != Message)
            {
                UltimoErroCount = 1;
                UltimoErroDate = DateTime.Now;
                UltimoErroTitulo = Message;
            }
            else
            {
                UltimoErroCount++;

                var t = DateTime.Now.Subtract(UltimoErroDate).TotalMinutes;
                if (t < 1)
                {
                    UltimoErroCount++;
                    return null; // Mesmo erro, não repete o envio dentro do mesmo minuto
                }
                else if (t > 1)
                {
                    // Reseta o contador e a data
                    UltimoErroDate = DateTime.Now;
                    UltimoErroCount = 1;
                }
                else
                    Data.Add("ErroCount", $"{UltimoErroCount} erros desde {UltimoErroDate.ToString("HH:mm:ss")}");
            }

            try
            {
                return Task.Run(() =>
                {
                    try
                    {
                        var ws = new Ativacoes.wsUtil();
                        string dados = "";
                        string stackMessage = "";
                        Exception ex = this;
                        while (ex != null)
                        {
                            stackMessage += $"{ex.GetType()}: {ex.Message}\r\n{ex.StackTrace}\r\n";
                            if (ex.Data.Count > 0)
                            {
                                foreach (string key in ex.Data.Keys)
                                    dados += $"{key}: {ex.Data[key]}\r\n";

                                dados += "\r\n";
                            }
                            ex = ex.InnerException;
                        }

                        string app = AC.Aplicacao;
                        string ver = AC.Versao;
                        if (app == null || ver == null)
                        {
                            var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                            string[] assemble = asm.FullName.Split(',');
                            app = assemble[0];
                            ver = assemble[1].Split('=')[1];
                        }

                        string result = ws.Erro(AC.Chave, app, ver, AC.idPDV, CodigoErro.ToString(), Message, stackMessage, dados);
                        Data.Add("SendAsync", result);
                    }
                    catch (Exception ex)
                    {
                        Data.Add("SendAsync", ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Data.Add("SendAsync", ex.Message);
                return null;
            }
        }
    }
}