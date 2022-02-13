using a7D.PDV.AutoAtendimento.UI.Services;
using a7D.PDV.Integracao.Pagamento;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace a7D.PDV.AutoAtendimento.UI
{
    // Logica copiada mo ModalTEF
    public partial class ModalTEFWindow : Window, IModalTEF
    {
        private ITEF tef;
        private DispatcherTimer timer;
        private int etapa;
        private Action AbrirPedido;
        private Action PagarPedido;
        private Action FecharPedido;
        private int timeoutLimit = 2;
        DateTime dtStart;

        public ModalTEFWindow()
        {
            InitializeComponent();
        }

        public ModalTEFWindow(ITEF objTEF, Action abrirPedido, Action pagarPedido, Action fecharPedido) : this()
        {
            tef = objTEF;
            AbrirPedido = abrirPedido;
            PagarPedido = pagarPedido;
            FecharPedido = fecharPedido;

            if ((objTEF as Integracao.Pagamento.NTKTEF.NTKPinpadPayGo) == null)
                Topmost = true;
            else
            {
                Topmost = false;
                timeoutLimit = 60; // 1h - Vai ficar esperando o cliente fechar!!!!
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutServices.Bind(this);
            ImagemTef.Visibility = ReciboTef.Visibility = Visibility.Collapsed;
            WindowState = WindowState.Maximized;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (timer != null)
                    tef.Cancelar();
            }
            catch (Exception)
            {
            }
            etapa = 9;
            DialogResult = false;
            this.Close();
        }

        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Name == Debito.Name)
                tef.DefinirMetodoPagamento(MetodoPagamento.Debito, 1);
            else if (btn.Name == Credito.Name)
                tef.DefinirMetodoPagamento(MetodoPagamento.Credito, 1);
            else
                return;

            IniciaProcessamento();
        }

        private void IniciaProcessamento()
        {
            Debito.Visibility = Credito.Visibility = Visibility.Collapsed;
            ImagemTef.Visibility = Visibility.Visible;

            dtStart = DateTime.Now;
            timer = new DispatcherTimer();
            timer.Tick += DispatcherTimer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Start();
        }

        public void HideWait()
        {
            Modal.Visibility = Visibility.Collapsed;
        }

        public void ShowWait()
        {
            Modal.Visibility = Visibility.Visible;
        }

        private void DispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            try
            {
                InvalidateVisual();
                if (etapa == 0)
                {
                    if (DateTime.Now.Subtract(dtStart).TotalMinutes > timeoutLimit)
                    {
                        etapa = 9;
                        Status.Text = "Tempo encerrado";
                        return;
                    }
#if TESTE
                    etapa = 1;
                    return;
#endif
                    if (!tef.Processando())
                    {
                        if (tef.PagamentoConfirmado == true)
                        {
                            etapa = 1;
                        }
                        else
                        {
                            Status.Text = tef.Mensagem;
                            etapa = 9;
                        }
                    }
                    else
                        Status.Text = tef.Mensagem;
                }
                else if (etapa == 1)
                {
                    ImagemTef.Visibility = Visibility.Collapsed;
                    ReciboTef.Visibility = Visibility.Visible;
                    Status.Text = "Registrando Pedido";
                    etapa = 2;
                }
                else if (etapa == 2)
                {
                    AbrirPedido();
                    etapa = 3;
                }
                else if (etapa == 3)
                {
                    Status.Text = "Registrando Pagamento";
                    etapa = 4;
                }
                else if (etapa == 4)
                {
                    PagarPedido();
                    etapa = 5;
                }
                else if (etapa == 5)
                {
                    Status.Text = "Imprimindo Comprovantes"; // Apenas exibição label
                    etapa = 6;
                }
                else if (etapa == 6)
                {
                    FecharPedido();
                    etapa = 7;
                }
                else if (etapa == 7)
                {
                    Status.Text = "Obrigado";
                    DialogResult = true;
                    etapa = 10;
                }
                else // >=9 , para mostrar erro em tela
                {
                    etapa++;
                    if (etapa > 50)
                    {
                        timer.Stop();
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogServices.Error(ex);
                if ((etapa >= 1 && etapa <= 4) && tef.PagamentoConfirmado == true)
                    tef.Estornar();

                Status.Text = ex.Message;
                etapa = 9;
            }
        }
    }
}