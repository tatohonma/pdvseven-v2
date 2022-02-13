using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ClienteCPFPage : Page
    {
        public ClienteCPFPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Numero.Text = App.Pedido.ComandaNumero.ToString();
            Teclado.MaxLength = 11;
            Teclado.TextFormat = "999.999.999-99";
            LayoutServices.Bind(Teclado);
        }

        private async void CPFNumero_Change(object sender, TextChangedEventArgs e)
        {
            if (Teclado.Cancelado)
            {
                App.Navigate<InicialPage>();
                return;
            }
            else if (!Extensions.IsCpf(Teclado.Text))
            {
                ModalSimNaoWindow.Show("CPF Inválido");
                return;
            }

            try
            {
                if (await App.Pedido.AbrirComandaAsync(Teclado.Text))
                    App.Navigate<ClienteCreditoPage>();
                else if (App.Pedido.TipoFluxo == EFluxo.CheckInEntrada && App.Pedido.TotalPedido > 0)
                    App.Navigate<ClienteEntradaPage>();
                else if (App.Pedido.TipoFluxo == EFluxo.CheckInSemEntrada)
                {
                    FimPage.MensagemFinal = "Comanda aberta, pronto para uso";
                    App.Navigate<FimPage>();
                }
                else
                    Teclado.Clear();
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
                App.Navigate<InicialPage>();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ClienteCheckinPage>();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }
    }
}
