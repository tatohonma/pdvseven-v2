using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class FimPage : Page
    {
        public static string MensagemFinal = "?";

        public FimPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Numero.Text = App.Pedido.ComandaNumero.ToString();
            Nome.Text = App.Pedido.ClienteNome;
            Mensagem.Text = MensagemFinal;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }
    }
}