using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    // Nova versão da página ComandaConfirmacaoPage
    public partial class ComandaClienteCPFPage : Page
    {
        public ComandaClienteCPFPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Numero.Text = $"COMANDA {App.Pedido.Comanda_LeitoraTAG}";
            Teclado.MaxLength = 11;
            Teclado.TextFormat = "999.999.999-99";
            LayoutServices.Bind(Teclado);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ComandaLeitoraPage>();
        }

        private void Teclado_onChange(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Teclado.Text) || Teclado.Cancelado)
            {
                Cancelar_Click(sender, null);
                return;
            }
            else if (!Extensions.IsCpf(Teclado.Text))
            {
                ModalSimNaoWindow.Show("CPF Inválido");
                return;
            }

            try
            {
                App.Pedido.Comanda_ClienteDocumento = Teclado.Text;
                var cliente = App.Pedido.ConsultaCliente(Teclado.Text);
                Teclado.Clear();

                if (cliente != null)
                {
                    App.Pedido.Comanda_IDCliente = cliente.IDCliente.Value;
                    App.Pedido.Comanda_ClienteNome = cliente.NomeCompleto;
                    App.Pedido.Comanda_ClienteTelefone = $"({cliente.Telefone1DDD}) {cliente.Telefone1Numero}";
                    App.Navigate<ComandaClienteConfirmacaoPage>();
                }
                else
                {
                    App.Pedido.Comanda_IDCliente = 0;
                    App.Pedido.Comanda_ClienteNome = "";
                    App.Pedido.Comanda_ClienteTelefone = "";
                    App.Navigate<ComandaClienteNovoPage>();
                }
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
            }
        }
    }
}
