using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ComandaClienteCreditoValorPage : Page
    {
        public ComandaClienteCreditoValorPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Numero.Text = $"COMANDA {App.Pedido.Comanda_LeitoraTAG}";
            Teclado.MaxLength = 4;
            Teclado.MoneyFormat = true;
            LayoutServices.Bind(Teclado);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ComandaLeitoraPage>();
        }

        private void Teclado_onChange(object sender, TextChangedEventArgs e)
        {
            decimal valor;
            if (string.IsNullOrEmpty(Teclado.Text) || Teclado.Cancelado)
            {
                Cancelar_Click(sender, null);
                return;
            }
            else if (!decimal.TryParse(Teclado.Text, out valor) || valor < 1)
            {
                ModalSimNaoWindow.Show("Valor mínimo de R$ 1,00");
                return;
            }

            try
            {
                Teclado.Clear();

                var credito = App.Produtos.LoadProduto(5);
                credito.ValorUnitario = valor;
                App.Pedido.Adicionar(credito);
                credito.ValorUnitario = 0;
                App.Navigate<VendaResumoPage>();
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
            }
        }
    }
}
