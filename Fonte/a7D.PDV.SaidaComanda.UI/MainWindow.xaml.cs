using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.SaidaComanda.UI.Properties;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace a7D.PDV.SaidaComanda.UI
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        int timeOut;
        PedidoAPI wsPedidos;
        PdvServices pdv;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                OcultaTudo(Colors.White);
                progress.Visibility = Visibility.Visible;

                titulo.Text = "Carregando Controle de Saída";

                var ws = new ClienteWS(Settings.Default.EnderecoAPI2);

                pdv = new PdvServices(ws);
                await Task.Factory.StartNew(() =>
                {
                    string status = pdv.ValidarPDV();
                    if (status != "OK")
                    {
                        if (status.StartsWith("http://"))
                            throw new Exception("Baixe a nova versão no servidor!\r\n" + status);
                    }
                    pdv.LerConfiguracoes();
                });

                pdvInfo.Text = pdv.PDVNome;

                wsPedidos = ws.Pedido();

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();

                NovaConsulta();
            }
            catch (Exception ex)
            {
                string erros = "";
                while (ex != null)
                {
                    erros += "\n" + ex.Message;
                    ex = ex.InnerException;
                }
                titulo.Text = erros.Substring(1);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (teclado.Visibility == Visibility.Visible || progress.Visibility == Visibility.Visible)
            {
                timeOut = 6;
                return;
            }
            else if (timeOut-- < 0)
                NovaConsulta();
        }

        private void OcultaTudo(Color cor)
        {
            teclado.Visibility = progress.Visibility = btOK.Visibility = btSIM.Visibility = btNAO.Visibility = Visibility.Collapsed;
            Background = new SolidColorBrush(cor);
        }

        private void NovaConsulta()
        {
            if (pdv.PDVID == 0 || pdv.UsuarioID == 0)
            {
                Close();
                return;
            }

            OcultaTudo(Colors.White);
            teclado.Clear();

            titulo.Text = "Informe o número da comanda";
            teclado.Visibility = Visibility.Visible;
        }

        private async void teclado_onChange(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(teclado.Text))
                return;

            if (!int.TryParse(teclado.Text, out int comanda))
                return;

            if (comanda == int.Parse(pdv.SenhaSaida))
            {
                Close();
                return;
            }

            teclado.Visibility = Visibility.Collapsed;
            progress.Visibility = Visibility.Visible;
            titulo.Text = "Aguarde...";

            Comanda comandaInfo = null;
            try
            {
                comandaInfo = await Task.Factory.StartNew<Comanda>(() => wsPedidos.ComandaStatus(comanda, true));

                if (comandaInfo == null)
                {
                    OcultaTudo(Colors.LightPink);
                    titulo.Text = "Comanda " + comanda + " não existe!";
                    btOK.Visibility = Visibility.Visible;
                    btOK.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                string erros = "";
                while (ex != null)
                {
                    erros += "\n" + ex.Message;
                    ex = ex.InnerException;
                }
                OcultaTudo(Colors.LightPink);
                Background = new SolidColorBrush(Colors.LightPink);
                titulo.Text = erros.Substring(1);
                btOK.Visibility = Visibility.Visible;
                btOK.Focus();
                return;
            }

            decimal valorPendente = (comandaInfo.ValorTotal ?? 0) - (comandaInfo.ValorPago ?? 0);
            /*
            Liberada = 10,
            EmUso = 20,
            Cancelada = 30,
            Perdida = 40,
            ContaSolicitada = 50
            */
            string comandaNome = "Comanda " + comanda;
            if (!string.IsNullOrEmpty(comandaInfo.ClienteNome))
                comandaNome += "\n" + comandaInfo.ClienteNome;

            if (valorPendente == 0 && (comandaInfo.status == 20 || comandaInfo.status == 50))
            {
                titulo.Text = comandaNome + "\nSem valor pendente\nDeseja encerrar a comanda?";
                OcultaTudo(Colors.Yellow);
                btSIM.Visibility = Visibility.Visible;
                btNAO.Visibility = Visibility.Visible;
                btNAO.Focus();
            }
            else
            {
                if (comandaInfo.status == 10)
                {
                    titulo.Text = comandaNome + "\nLiberada";
                    OcultaTudo(Colors.LightGreen);
                }
                else if (comandaInfo.status == 20)
                {
                    titulo.Text = comandaNome + "\nEm uso\nValor pendente de R$ " + valorPendente.ToString("N2");
                    OcultaTudo(Colors.LightPink);
                }
                else if (comandaInfo.status == 30)
                {
                    titulo.Text = comandaNome + "\nCancelada";
                    OcultaTudo(Colors.Yellow);
                }
                else if (comandaInfo.status == 40)
                {
                    titulo.Text = comandaNome + "\nPerdida";
                    OcultaTudo(Colors.LightGray);
                }
                else if (comandaInfo.status == 50)
                {
                    titulo.Text = comandaNome + "\nConta solicitada.\nValor pendente de R$ " + valorPendente.ToString("N2");
                    OcultaTudo(Colors.LightPink);
                }
                btOK.Visibility = Visibility.Visible;
                btOK.Focus();
            }
        }

        private async void btCommand_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            if (btn != btSIM)
            {
                NovaConsulta();
                return;
            }

            try
            {
                int.TryParse(teclado.Text, out int comanda);
                titulo.Text = "Fechando a comanda...";
                OcultaTudo(Colors.LightBlue);
                progress.Visibility = Visibility.Visible;

                await Task.Factory.StartNew(() =>
                {
                    var comandaInfo = wsPedidos.ComandaStatus(comanda, true);
                    decimal valorPendente = comandaInfo.ValorTotal ?? 0 - comandaInfo.ValorPago ?? 0;
                    if (!comandaInfo.idPedidoAberto.HasValue)
                        return; // Já está fechado
                    else if (valorPendente == 0)
                        wsPedidos.Fechar(new FechamentoPedido()
                        {
                            IDPdv = pdv.PDVID,
                            ChaveAcesso = pdv.ChaveUsuario,
                            IDPedido = comandaInfo.idPedidoAberto.Value
                        });
                    else
                        throw new Exception("Há valores pendentes na comanda!");
                });


                OcultaTudo(Colors.LightGreen);
                titulo.Text = "Comanda Liberada";
                btOK.Visibility = Visibility.Visible;
                btOK.Focus();
            }
            catch (Exception ex)
            {
                OcultaTudo(Colors.Yellow);
                btSIM.Visibility = btNAO.Visibility = Visibility.Visible;
                string erros = "";
                while (ex != null)
                {
                    erros += "\n" + ex.Message;
                    ex = ex.InnerException;
                }
                titulo.Text = "Erro ao encerrar a comanda, tente novamente?" + erros;
            }
            finally
            {
                progress.Visibility = Visibility.Collapsed;
            }
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (teclado.Visibility == Visibility.Visible)
                return;

            NovaConsulta();
        }

        private void Grid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            timeOut = 5;

            if (teclado.Visibility == Visibility.Visible)
            {
                if (!teclado.IsFocused)
                    teclado.SetFocus();
                return;
            }
        }
    }
}