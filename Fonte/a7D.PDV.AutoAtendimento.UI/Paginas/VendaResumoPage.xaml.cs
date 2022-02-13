using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class VendaResumoPage : Page
    {
        public VendaResumoPage()
        {
            InitializeComponent();
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Pedido.Bind(TotalPedido, null, null);
                App.Pedido.Bind(Confirmar);
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
            }
        }

        public void ComandaNumero_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ComandaLeitoraPage>();
        }

        public void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (App.Pedido.TotalProdutos == 0)
                App.Navigate<InicialPage>();
            else
                App.Navigate<VendaCategoriaProdutoPage>();
        }

        public void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (App.Pedido.Pagar())
            {
                FimPage.MensagemFinal = "Pagamento concluído, volte sempre";
                App.Navigate<FimPage>();
            }
        }
    }
}
