using System.Windows;

namespace a7D.PDV.PainelMesaComanda.UI
{
    public partial class ModalMesa : Window
    {
        public ModalMesa()
        {
            InitializeComponent();
        }

        public ModalMesa(int mesa) : this()
        {
            var comandas = App.LerComandasPorMesa(mesa);
            comandaLista.ItemsSource = comandas;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Close();
        }
    }
}