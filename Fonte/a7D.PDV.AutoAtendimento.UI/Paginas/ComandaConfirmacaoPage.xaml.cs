using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ComandaConfirmacaoPage : Page
    {
        public ComandaConfirmacaoPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Numero.Text = App.Pedido.ComandaNumero.ToString();
                Nome.Text = App.Pedido.ClienteNome;
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
                App.Navigate<InicialPage>();
            }
        }

        private async void ComandaMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (await App.Pedido.CarregarComandaPedidoAsync(App.Pedido.ComandaNumero))
                App.Navigate<ComandaMenuPage>();
        }

        private void Sim_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (PdvServices.ComandaComCredito)
                App.Navigate<ClienteCreditoPage>();
            else
                App.Navigate<ComandaMenuPage>();
        }

        private void Nao_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (App.Pedido.TipoFluxo == Services.EFluxo.CheckInEntrada)
                App.Navigate<ClienteCheckinPage>();
            else
                App.Navigate<ComandaNumeroPage>();
        }

        private void Voltar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Voltar();
        }

        private void Home_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }
    }
}
