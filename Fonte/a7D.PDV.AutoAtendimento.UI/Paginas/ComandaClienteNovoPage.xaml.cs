using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    // Nova versão da página ComandaConfirmacaoPage
    public partial class ComandaClienteNovoPage : Page
    {
        Process TapTip;

        public ComandaClienteNovoPage()
        {
            InitializeComponent();
        }

        private void killTeclado()
        {
            try
            {
                if (TapTip != null && !TapTip.HasExited)
                    TapTip.CloseMainWindow();
            }
            catch (Exception)
            {
            }

            var processos = Process.GetProcesses();
            foreach (Process processo in processos)
            {
                try
                {
                    if (processo.ProcessName.ToLower().Contains("tabtip"))
                        processo.Kill();
                }
                catch (Exception) { }
            }
        }

        private void startTeclado()
        {
            // https://www.howtogeek.com/howto/16189/configure-what-items-are-available-in-control-panel-or-completely-disable-it-in-windows-7/
            string touchKeyboardPath = @"C:\Program Files\Common Files\Microsoft Shared\Ink\TabTip.exe";
            TapTip = Process.Start(touchKeyboardPath);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CPF.Text = App.Pedido.Comanda_ClienteDocumento;

            Teclado.SetTextEdit(Nome);
            Teclado.SetTextEdit(Telefone);

            Nome.Focus();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate<ComandaLeitoraPage>();
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Nome.Text))
                {
                    ModalSimNaoWindow.Show("Informe o nome");
                }
                else
                {
                    App.Pedido.Comanda_ClienteNome = Nome.Text;
                    App.Pedido.Comanda_ClienteTelefone = Telefone.Text;
                    App.Navigate<ComandaClienteConfirmacaoPage>();
                }
                //var result = App.Pedido.InserirCliente(CPF.Text, Nome.Text, Telefone.Text.SoNumeros());
                //if (result.Id > 0)
                //{
                //    string result2 = App.Pedido.AbrirComanda(result.Id.Value, Nome.Text, 0);
                //    if (result2 == "OK")
                //        App.Navigate<ComandaClienteCreditoPage>();
                //    else
                //        ModalSimNaoWindow.Show(result2);

                //    App.Navigate<ComandaClienteCreditoPage>();
                //}
                //else
                //{
                //    ModalSimNaoWindow.Show(result.Mensagem);
                //}
            }
            catch (Exception ex)
            {
                ModalSimNaoWindow.Show(ex);
            }
        }
    }
}
