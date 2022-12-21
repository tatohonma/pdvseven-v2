using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace a7D.PDV.Integracao.Servico.Core
{
    public abstract class IntegracaoTask
    {
        public virtual int Revalidar { get; } = 10;

        protected Thread Processo;
        private Action Acao;

        public abstract string Nome { get; }
        public bool Executando { get; set; }
        public bool Disponivel { get; set; }
        public bool Configurado { get; set; }
        public abstract void Executar();
        internal event OnMensagemListener EventoMensagem;
        //public event  EventoProgress;
        public event onAddLogException EventoLogException;
        public bool RequestClose { get; protected set; }

        private string BasePath;
        private string Integracao;

        private Int32 qtdTentativas = 0;

        public IntegracaoTask()
        {
            var fi = new FileInfo(Assembly.GetEntryAssembly().Location);
            BasePath = fi.Directory.FullName;
            Integracao = GetType().Name;
        }

        public void AddLog(string info)
        {
            try
            {
                string cFile = Path.Combine(BasePath, string.Format(@"{0}-{1:yyyyMMdd}.log", Integracao, DateTime.Now));
                File.AppendAllText(cFile, string.Format("{0:HH:mm:ss} {1}", DateTime.Now, info + Environment.NewLine));
            }
            catch (Exception ex)
            {
                info += " - ERRO LOG: " + ex.Message;
            }
            try
            {
                EventoMensagem?.Invoke(info);
            }
            catch (Exception)
            {
            }
        }

        public void AddLog(Exception ex, bool saveLog = false)
        {
            string info = EventoLogException?.Invoke(ex, saveLog);
            AddLog(info);
        }

        protected void Iniciar(Action acao)
        {
            Executando = true;
            Acao = acao;
            Processo = new Thread(ExecuteWait);
            Processo.Start();
        }

        private void ExecuteWait()
        {
            AddLog("Iniciando " + Nome);
            try
            {
                Acao.Invoke();
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
            finally
            {
                Executando = false;
                AddLog("Finalizando " + Nome);

                if(qtdTentativas < 5)
                {
                    qtdTentativas++;
                    AddLog("Reiniciando " + Nome + "... Tentativa " + qtdTentativas + " de 5");

                    Executando = true;
                    this.Executar();
                }
            }
        }

        public void Parar()
        {
            if (Processo != null && Executando)
                Executando = false;
        }

        protected void Sleep(int segundos)
        {
            while (Executando && segundos-- > 0)
                Thread.Sleep(1000);
        }
    }
}
