using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ClienteCheckinPage : Page
    {
        public ClienteCheckinPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutServices.Bind(Teclado);
        }

        private async void ComandaNumero_Change(object sender, TextChangedEventArgs e)
        {
            int comanda;

            if (Teclado.Cancelado)
            {
                App.Navigate<InicialPage>();
                return;
            }
            else if (!Int32.TryParse(Teclado.Text, out comanda) || comanda == 0)
                return;

            Teclado.Clear();

            try
            {
                if (await App.Pedido.CarregarComandaAsync(comanda))
                {
                    if (App.Pedido.ComandaStatus == 10)
                        App.Navigate<ClienteCPFPage>();
                    else if(PdvServices.ComandaComCredito)
                        App.Navigate<ClienteCreditoPage>();
                    else
                        App.Navigate<ComandaMenuPage>();
                }
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }
    }
}
