using System;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ComandaMenuPage : Page
    {
        public ComandaMenuPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Numero.Text = App.Pedido.ComandaNumero.ToString();
                Nome.Text = App.Pedido.ClienteNome;
                App.Pedido.Bind(TotalPedido, TotalAPagar, null);

                Pagar.Visibility = App.Pedido.TotalAPagar == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
                Fechar.Visibility = App.Pedido.TotalAPagar != 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
                App.Navigate<InicialPage>();
            }
        }

        private void Voltar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Navigate<ComandaNumeroPage>();
        }

        private void Imprimir_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Pedido.ImprimirConta();
            Imprimir.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void Pagar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (await App.Pedido.CarregarComandaPedidoAsync(App.Pedido.ComandaNumero))
            {
                if (App.Pedido.Pagar())
                {
                    FimPage.MensagemFinal = "Comanda finalizada, volte sempre";
                    App.Navigate<FimPage>();
                }
            }
            else
                App.Navigate<InicialPage>();
        }

        private void Home_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }

        private async void Fechar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (await App.Pedido.CarregarComandaPedidoAsync(App.Pedido.ComandaNumero))
            {
                await App.Pedido.FecharComandaSemValorAsync();
                FimPage.MensagemFinal = "Comanda Encerrada.\nVolte sempre";
                App.Navigate<FimPage>();
            }
            else
                App.Navigate<InicialPage>();
        }
    }
}