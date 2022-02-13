using System;
using System.Drawing;
using System.Windows.Forms;

namespace a7D.PDV.Integracao.Servico.Core
{
    public class IntegracaoItem
    {
        private TextBox LogTextBox;
        private PictureBox StatusPic;
        private string lastErro = null;

        public IntegracaoTask Instance { get; private set; }

        public bool RequestClose => Instance.RequestClose;

        public string Nome => Instance.Nome;

        public static IntegracaoItem From<TIntegracao>(TextBox txtLog, PictureBox picStatus, onAddLogException eventoException)
            where TIntegracao : IntegracaoTask, new()
        {
            var integracao = new IntegracaoItem
            {
                Instance = new TIntegracao(),
                StatusPic = picStatus,
                LogTextBox = txtLog
            };
            integracao.Instance.EventoMensagem += integracao.AtualizarTextBox;
            integracao.Instance.EventoLogException += eventoException;
            return integracao;
        }

        public void AtualizarTextBox(string info)
        {
            LogTextBox.Invoke(new MethodInvoker(delegate
            {
                if (LogTextBox.TextLength > 100000)
                    LogTextBox.Clear();

                LogTextBox.AppendText($"{DateTime.Now.ToString("dd/MM/yy HH:mm:ss")} {info}\r\n");
            }));
        }

        private void AtualizarStatus(Semaforo semaforo)
        {
            StatusPic.Invoke(new MethodInvoker(delegate
            {
                StatusPic.BackColor = Color.FromArgb((int)semaforo);
            }));
        }

        private void AnalisaStatus()
        {
            if (Instance.Executando)
                AtualizarStatus(Semaforo.Rodando);
            else if (Instance.Configurado)
                AtualizarStatus(Semaforo.Parado);
            else if (Instance.Disponivel)
                AtualizarStatus(Semaforo.NaoConfigurado);
            else
                AtualizarStatus(Semaforo.Desabilitado);
        }

        public bool Executando
        {
            get
            {
                AnalisaStatus();
                return Instance.Executando;
            }
        }

        public bool ValidarOuExecutar(int revalidar, bool start)
        {
            try
            {
                if (!start && !Instance.Configurado)
                {
                    AtualizarStatus(Semaforo.NaoConfigurado);
                    return false;
                }
                if (revalidar % Instance.Revalidar != 1)
                    return false;

                AtualizarStatus(Semaforo.Iniciado);
                Instance.Executar();

                return true;
            }
            catch (Exception ex)
            {
                string message = "ERRO: ";
                if (lastErro != ex.Message)
                {
                    Instance.AddLog(ex);
                    lastErro = ex.Message;
                    while (ex != null)
                    {
                        message += ex.Message + "\r\n";
                        ex = ex.InnerException;
                    }
                }
                else
                    message += ex.Message;

                AtualizarTextBox(message);
            }
            AnalisaStatus();
            return false;
        }

        public void Parar()
        {
            Instance.Parar();
            AnalisaStatus();
        }
    }
}
