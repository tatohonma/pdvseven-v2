using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ComandaNumeroPage : Page
    {
        private DateTime lastKeyEvent = DateTime.Now;
        private string bufferKeyboard = "";

        public ComandaNumeroPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;

            LayoutServices.Bind(Teclado);
        }

        private async void HandleKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!e.IsDown)
                return;

            if (DateTime.Now.Subtract(lastKeyEvent).TotalSeconds > 1)
                bufferKeyboard = "";

            lastKeyEvent = DateTime.Now;

            string x = e.Key.ToString();

            if (x.StartsWith("D") && x.Length == 2)
            {
                bufferKeyboard += x.Substring(1, 1);
                //Numero.Text = bufferKeyboard;
            }
            else if (x == "A" || x == "B" || x == "C" || x == "D" || x == "E" || x == "F")
            {
                bufferKeyboard += x;
                //Numero.Text = bufferKeyboard;
            }
            else if (e.Key == System.Windows.Input.Key.Return && bufferKeyboard.Length > 0)
            {
                try
                {
                    // Permite digitação de numero pelo teclado, caso contrario TAG HEX por enquant por padrão
                    App.Pedido.Comanda_LeitoraTIPO = bufferKeyboard.Length < 6 ? "numero" : "TAGDEC";
                    App.Pedido.Comanda_LeitoraTAG = bufferKeyboard.ToUpper();
                    var comanda = App.Pedido.ComandaInfo(App.Pedido.Comanda_LeitoraTAG, App.Pedido.Comanda_LeitoraTIPO);

                    if (comanda == null)
                    {

                        ModalSimNaoWindow.Show($"Comanda fechada.  Favor comparecer ao caixa.");
                        bufferKeyboard = "";
                        return;
                    }
                    else
                    {
                        if (await App.Pedido.CarregarComandaPedidoAsync(comanda.Numero.Value))
                        {
                            if (string.IsNullOrEmpty(App.Pedido.ClienteNome))
                                App.Navigate<ComandaMenuPage>();
                            else
                                App.Navigate<ComandaConfirmacaoPage>();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModalSimNaoWindow.Show(ex);
                }
            }
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
                if (await App.Pedido.CarregarComandaPedidoAsync(comanda))
                {
                    if (string.IsNullOrEmpty(App.Pedido.ClienteNome))
                        App.Navigate<ComandaMenuPage>();
                    else
                        App.Navigate<ComandaConfirmacaoPage>();
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
