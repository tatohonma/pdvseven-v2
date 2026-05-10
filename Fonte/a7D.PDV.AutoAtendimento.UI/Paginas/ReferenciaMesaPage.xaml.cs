using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ReferenciaMesaPage : Page
    {
        public ReferenciaMesaPage()
        {
            InitializeComponent();
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Teclado.MaxLength = 4;
            Teclado.TextFormat = "";

            LayoutServices.Bind(Teclado);
        }

        void MesaNumero_Change(object sender, TextChangedEventArgs e)
        {
            if (Teclado.Cancelado)
            {
                VoltarTelaAnterior();
                return;
            }

            if (Teclado.Confirmado)
            {
                ConfirmarMesa();
                return;
            }

            Numero.Text = Teclado.Text;
        }

        void ConfirmarMesa()
        {
            if (string.IsNullOrWhiteSpace(Teclado.Text))
            {
                ModalSimNaoWindow.Show("Informe o número da mesa");
                return;
            }

            try
            {
                App.Pedido.ReferenciaMesa = Teclado.Text;

                App.Navigate<VendaResumoPage>();
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
                App.Navigate<InicialPage>();
            }
        }

        void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            VoltarTelaAnterior();
        }

        void Home_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }

        void VoltarTelaAnterior()
        {
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService.GoBack();
                return;
            }

            App.Navigate<VendaCategoriaProdutoPage>();
        }
    }
}