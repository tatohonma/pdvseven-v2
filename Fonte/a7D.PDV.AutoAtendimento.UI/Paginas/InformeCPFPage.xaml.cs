using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class InformeCPFPage : Page
    {
        public InformeCPFPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CPFNumero.MaxLength = 11;
            CPFNumero.TextFormat = "999.999.999-99";
            LayoutServices.Bind(CPFNumero);
        }

        private void CPFNumero_Change(object sender, TextChangedEventArgs e)
        {
            if (CPFNumero.Cancelado)
            {
                App.Navigate<InicialPage>();
                return;
            }
            else if (!Extensions.IsCpf(CPFNumero.Text))
            {
                ModalSimNaoWindow.Show("CPF Inválido");
                return;
            }

            CPFNumero.Clear();

            try
            {

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
