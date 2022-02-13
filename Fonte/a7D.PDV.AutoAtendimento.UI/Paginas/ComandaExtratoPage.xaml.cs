using System;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ComandaExtratoPage : Page
    {
        public ComandaExtratoPage()
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

        private void Voltar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Navigate<ClienteCreditoPage>();
        }

        private void Imprimir_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Pedido.ImprimirExtratoCreditos();
            Imprimir.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Home_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }
    }
}