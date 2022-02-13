using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Shared.Atualizacao;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace a7D.PDV.Componentes
{
    public partial class SplashScreen : Window, IShowInfo
    {
        private Action<IShowInfo> ProcessAction;

        public SplashScreen(Action<IShowInfo> waitAction)
        {
            InitializeComponent();
            ProcessAction = waitAction;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            versao.Text = "Versão: " + Atualizacao.VersaoAtual;
            await Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(200);
                    ProcessAction.Invoke(this);
                    Thread.Sleep(200);
                }
                catch (Exception ex)
                {
                    ShowInfo(ex.Message);
                    Thread.Sleep(5000);
                }
            });
            Close();
        }

        public static void ShowRun(Action<IShowInfo> waitAction)
        {
            var s = new SplashScreen(waitAction);
            s.ShowDialog();
        }

        private void SetLoadText(string info)
        {
            if (UDPClient.ServerIP != null && servidor.Text == "?")
                servidor.Text = "Servidor: " + UDPClient.ServerIP.Address.ToString();

            loading.Text = info;
        }

        public void ShowInfo(string info)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new UpdateText(SetLoadText), info);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            // Quando fecha a tela é porque acabou a etapa de atualização
            Atualizacao.Cancel = true;
        }
    }
}
