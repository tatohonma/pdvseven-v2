using a7D.PDV.AutoAtendimento.UI.Services;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class InicialPage : Page
    {
        bool produtosEnable;
        public InicialPage()
        {
            InitializeComponent();
            App.NovoPedido(); //Sempre que volta ao inicio recria o Pedido Services para não ficar nada em memoria!
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.Timeout = false; // Só a página inicial não tem timeout
            App.StatusBar = PdvServices.PDVNome; // Na página inicial apenas mostra o nome do PDV

            // Se tiver modulo de venda
            if (VendaCategoriaProduto.Visibility != Visibility.Visible)
                return;

            VendaCategoriaProduto.IsEnabled = false;
            produtosEnable = await App.Produtos.SyncProdutos();
            VendaCategoriaProduto.IsEnabled = true;
        }

        public void ComandaNumero_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ComandaNumeroPage>();
        }

        private void ClienteCheckin_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ClienteCheckinPage>();
        }

        private void VendaCategoriaProduto_Click(object sender, RoutedEventArgs e)
        {
            //var pedido = App.Pedido.LerPedido(6060);
            //TicketServices.Imprime(pedido);
            if (produtosEnable)
                //App.Navigate<ComandaClienteCreditoPage>();
                App.Navigate<ComandaLeitoraPage>();
            else
                App.Navigate<InicialPage>();
        }
    }
}
