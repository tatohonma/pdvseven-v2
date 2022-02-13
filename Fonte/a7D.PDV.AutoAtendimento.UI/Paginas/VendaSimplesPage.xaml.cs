using a7D.PDV.AutoAtendimento.UI.Services;
using a7D.PDV.Integracao.API2.Model;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class VendaSimplesPage : Page
    {
        PedidoServices pedido;

        public VendaSimplesPage()
        {
            InitializeComponent();
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            pedido = App.Pedido;
            pedido.Bind(pedidoLista);
            //pedido.Bind(totalPedido);
            //App.Produtos.Fill(produtosLista, Adicionar_Click);
        }

        public void ComandaNumero_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ComandaNumeroPage>();
        }

        public void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (pedido.PedidoID == 0)
            {
                var produto = (sender as Button).Tag as Produto;
                pedido.Adicionar(produto);
            }
            else
                ExibeErroPedidoRegistrado();
        }

        public void Remover_Click(object sender, RoutedEventArgs e)
        {
            if (pedido.PedidoID == 0)
                pedido.Remover(sender);
            else
                ExibeErroPedidoRegistrado();
        }

        public void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (pedido.TotalProdutos == 0)
                App.Navigate<InicialPage>();
            else
            {
                btnConfirmarPagar.Content = "Confirmar";
                pedido.Cancelar();
            }
        }

        public void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            pedido.Pagar();

            if (pedido.PedidoID > 0)
                btnConfirmarPagar.Content = "Pagar";
            else if (pedido.PedidoID == 0)
                App.Navigate<InicialPage>();
        }

        private void ExibeErroPedidoRegistrado()
        {
            ModalSimNaoWindow.Show("O Pedido já foi registrado, mas falta realizar o pagamento,\r\nClique em Pagar, ou cancele o pedido");
        }
    }
}
