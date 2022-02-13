using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    // Nova versão da página ComandaConfirmacaoPage
    public partial class ComandaClienteConfirmacaoPage : Page
    {
        public ComandaClienteConfirmacaoPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Nome.Text = App.Pedido.Comanda_ClienteNome;
            Numero.Text = $"COMANDA {App.Pedido.Comanda_LeitoraTAG}";
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<VendaCategoriaProdutoPage>();
        }

        private void Trocar_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Compareça ao caixa.  Não é possível efetuar esta transação em um terminal de autoatendimento";
            ModalSimNaoWindow.Show(msg);
            App.Navigate<VendaCategoriaProdutoPage>();

            return;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ComandaClienteCreditoPage>();
        }
    }
}
