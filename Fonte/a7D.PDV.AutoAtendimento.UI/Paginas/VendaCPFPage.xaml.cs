using a7D.PDV.AutoAtendimento.UI.Services;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    // Nova versão da página ComandaConfirmacaoPage
    public partial class VendaCPFPage : Page
    {
        public VendaCPFPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Teclado.MaxLength = 11;
            Teclado.TextFormat = "999.999.999-99";
            Teclado.ValorInicial(App.Pedido.Comanda_ClienteDocumento);
            LayoutServices.Bind(Teclado);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<VendaResumoPage>();
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (App.Pedido.Pagar())
            {
                FimPage.MensagemFinal = "Pagamento concluído, volte sempre";
                App.Navigate<FimPage>();
            }
        }

        private void Teclado_onChange(object sender, TextChangedEventArgs e)
        {
            if (Teclado.Cancelado)
            {
                Teclado.Clear();
                return;
            }
            else if (string.IsNullOrEmpty(Teclado.Text) )
            {
                ModalSimNaoWindow.Show("Informe o CPF Inválido, ou clique em 'SEM CPF'");
                return;
            }
            else if (!Extensions.IsCpf(Teclado.Text))
            {
                ModalSimNaoWindow.Show("CPF Inválido");
                return;
            }

            App.Pedido.ClienteNotaCPF(Teclado.Text);

            Confirmar_Click(sender, e);
        }
    }
}
