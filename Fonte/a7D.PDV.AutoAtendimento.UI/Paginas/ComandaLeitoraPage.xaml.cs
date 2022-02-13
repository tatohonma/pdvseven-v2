using a7D.PDV.EF.Enum;
using System;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ComandaLeitoraPage : Page
    {
        private DateTime lastKeyEvent = DateTime.Now;
        private string bufferKeyboard = "";

        public ComandaLeitoraPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
            ExibeComanda();
        }

        private void ExibeComanda()
        {
            // 5 digitos no máximo para número, 6 ou mais para TAG! mas deve mostrar sempre o numero da comanda
            App.Pedido.Comanda_LeitoraTAG = "";
            Numero.Text = $"COMANDA 00000000";
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<InicialPage>();
        }

        private void HandleKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
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
                Numero.Text = bufferKeyboard;
            }
            else if (x == "A" || x == "B" || x == "C" || x == "D" || x == "E" || x == "F")
            {
                bufferKeyboard += x;
                Numero.Text = bufferKeyboard;
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
                        ExibeComanda();
                        return;
                    }
                    else
                    {
                        if ((EStatusComanda)comanda.status != EStatusComanda.EmUso)
                        {
                            ModalSimNaoWindow.Show($"Comanda em status inválido [{(EStatusComanda)comanda.status}]");
                            bufferKeyboard = "";
                            ExibeComanda();
                        }

                        var pedidoComanda = BLL.Pedido.ObterPedidoAbertoPorComanda(comanda.Numero.Value);
                        if (pedidoComanda != null && pedidoComanda.IDPedido.HasValue && pedidoComanda.Cliente != null && pedidoComanda.Cliente.IDCliente.HasValue)
                        {
                            var cliente = BLL.Cliente.Carregar(pedidoComanda.Cliente.IDCliente.Value);
                            if (cliente != null)
                            {
                                App.Pedido.Comanda_Numero = comanda.Numero.Value;
                                App.Pedido.Comanda_IDCliente = cliente.IDCliente.Value;
                                App.Pedido.Comanda_ClienteNome = cliente.NomeCompleto;
                                App.Pedido.Comanda_ClienteDocumento = cliente.Documento1;
                                App.Pedido.Comanda_ClienteTelefone = $"({cliente.Telefone1DDD}) {cliente.Telefone1}";
                                App.Navigate<ComandaClienteConfirmacaoPage>();
                            }
                            else
                            {
                                throw new Exception("Comanda sem cliente associado");
                            }
                        }
                    }

                    //else if (bufferKeyboard.Length < 6 && int.TryParse(bufferKeyboard, out int num))
                    //{
                    //    nComanda = num;
                    //}

                    //// Comanda existe!
                    //if (info.status == 10)
                    //{
                    //    ComandaClienteCPFPage.ComandaNumero = info.Numero.Value;
                    //    App.Navigate<ComandaClienteCPFPage>();
                    //}
                    //else if (info.status == 30)
                    //    ModalSimNaoWindow.Show("Comanda cancelada");
                    //else if (info.status == 40)
                    //    ModalSimNaoWindow.Show("Comanda perdida");
                    //else // 20
                    //{
                    //    App.Pedido.ClienteDefine(info.Cliente.IDCliente.Value, info.Cliente.NomeCompleto, nComanda);
                    //    App.Navigate<ComandaClienteConfirmacaoPage>();
                    //}
                }
                catch (Exception ex)
                {
                    ModalSimNaoWindow.Show(ex);
                }
                bufferKeyboard = "";
                ExibeComanda();

            }
        }
    }
}
