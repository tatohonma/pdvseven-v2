using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ClienteEntradaPage : Page
    {
        public ClienteEntradaPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Numero.Text = App.Pedido.ComandaNumero.ToString();
            Nome.Text = App.Pedido.ClienteNome;
            App.Pedido.Bind(Entrada, null, Saldo);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }

        private void Pagar_Click(object sender, RoutedEventArgs e)
        {
            if (App.Pedido.Pagar()) // Se transacionar abre a comanda, adiciona o pagamento, mas não fecha a comanda
            {
                FimPage.MensagemFinal = "Comanda aberta, pronto para uso";
                App.Navigate<FimPage>();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }

        private void Creditos_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ClienteCreditoPage>();
        }
    }
}
