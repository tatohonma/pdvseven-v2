using a7D.PDV.AutoAtendimento.UI.Services;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Controles
{
    /// <summary>
    /// Interaction logic for Pedido1.xaml
    /// </summary>
    public partial class Pedido2 : UserControl
    {
        private PedidoServices pedido;

        public Pedido2()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pedido = App.Pedido;
            pedido?.Bind(pedidoLista);
        }
    }
}