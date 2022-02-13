using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace a7D.PDV.Integracao.Pagamento
{
    public partial class ModalTEF : Window, IModalTEF
    {
        private DispatcherTimer timer;
        private ITEF tef;
        private int etapa;
        private int parcelas;
        private bool SempreAtivo;

        public ModalTEF()
        {
            InitializeComponent();
        }

        public ModalTEF(ITEF objTEF, int nParcelas, bool sempreAtivo) : this()
        {
            SempreAtivo = sempreAtivo;
            parcelas = nParcelas;
            tef = objTEF;
            etapa = 0;
        }

        public void HideWait()
        {
            Topmost = false;
            borderMain.Visibility =  Visibility.Collapsed;
        }

        public void ShowWait()
        {
            Topmost = true;
            borderMain.Visibility =  Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;

            if (tef.PrecisaSelecionar)
                txtStatus.Text = "Selecione a forma de pagamento";
            else
                IniciaProcessamento();
        }

        private void IniciaProcessamento()
        {
            btnCredito.Visibility = btnDebito.Visibility = Visibility.Collapsed;
            txtStatus.Text = "Processando...";

            timer = new DispatcherTimer();
            timer.Tick += DispatcherTimer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Start();
        }

        public Action ConfirmarPagamento { get; set; }
        public Action FecharPedido { get; set; }

        private void DispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            try
            {
                if (etapa == 0)
                {
                    if (SempreAtivo && !IsActive)
                        Activate();
                    else if (!SempreAtivo && IsActive)
                    {
                        Topmost = false;
                        Hide();
                    }

                    InvalidateVisual();
                    if (!tef.Processando())
                    {
                        btnCancelar.Visibility = Visibility.Collapsed;
                        if (tef.Autorizacao == null)
                        {
                            txtStatus.Text = tef.Mensagem;
                            etapa = 9;
                        }
                        else
                            etapa = 1;
                    }
                    else
                        txtStatus.Text = tef.Mensagem;
                }
                else if (etapa == 1)
                {
                    ConfirmarPagamento?.Invoke();
                    etapa = 2;
                }
                else if (etapa == 2)
                {
                    FecharPedido?.Invoke();
                    etapa = 3;
                }
                else
                {
                    timer.Stop();
                    Thread.Sleep(500);
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                txtStatus.Text = ex.Message;
                etapa = 9;
            }
        }

        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Name == btnDebito.Name)
                tef.DefinirMetodoPagamento(MetodoPagamento.Debito, 1);
            else if (btn.Name == btnCredito.Name)
                tef.DefinirMetodoPagamento(MetodoPagamento.Credito, parcelas);

            IniciaProcessamento();
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
            this.DialogResult = false;
            this.Close();
        }
    }
}