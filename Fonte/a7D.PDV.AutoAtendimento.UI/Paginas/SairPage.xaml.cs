using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    /// <summary>
    /// Interaction logic for SairPage.xaml
    /// </summary>
    public partial class SairPage : Page
    {
        public SairPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutServices.Bind(Teclado);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }

        private void ComandaNumero_onChange(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Teclado.Text))
            {
                App.Navigate<InicialPage>();
                return;
            }

            Int32.TryParse(Teclado.Text, out int codigo);
            Teclado.Clear();

            if (codigo == 0)
                return;

            if (codigo == PdvServices.SenhaSaida)
                App.Finaliza();
        }
    }
}
