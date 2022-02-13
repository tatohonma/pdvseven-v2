using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Controles
{
    /// <summary>
    /// Interaction logic for Pedido1.xaml
    /// </summary>
    public partial class PedidoCredito : UserControl
    {
        //private PedidoServices pedido;

        public PedidoCredito()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var itens = App.Pedido.CarregaExtratoCreditos();
                pedidoLista.ItemsSource = itens;
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
            }
        }
    }
}