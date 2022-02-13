using a7D.PDV.Integracao.Pagamento.NTKTEF;
using a7D.PDV.Integracao.Pagamento.NTKTEFDLL;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace a7D.PDV.Integracao.Pagamento
{
    public partial class frmTEF : Form
    {
        ITEF tef;

        public PagamentoResultado Pagamento { get; private set; }

        public frmTEF()
        {
            InitializeComponent();
        }

        public frmTEF(TipoTEF tipoTEF, string loja, string pdv, int pedido, decimal valor, decimal servico, string celular) : this()
        {
            try
            {
                label1.Text = "Inicializando TEF " + tipoTEF.ToString();

                if (tipoTEF == TipoTEF.NTKPayGo)
                {
                    var ntk = NTKBuilder.AutorizacaoVenda(pedido, loja, pdv, DateTime.Now, pedido, valor, servico);
                    tef = ntk.IniciaTransacao();
                }
                else if (tipoTEF == TipoTEF.NTKDLL)
                {
                    tef = new NTKPinpadPayGoWeb(pedido, pedido, valor, null);
                    tef.DefinirMetodoPagamento(MetodoPagamento.Selecione, 1);
                }
                else if (tipoTEF == TipoTEF.PAGO)
                {
                    var pago = new PagoTEF.PagoPinpad(pedido, pedido, valor, celular, pdv);
                    tef = pago.IniciaVenda();
                }
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
            }
            bw.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (tef != null && tef.Processando())
                {
                    if (!string.IsNullOrEmpty(tef.Mensagem))
                    {
                        label1.Invoke(new MethodInvoker(delegate
                        {
                            label1.Text = tef.Mensagem;
                        }));
                    }
                    Thread.Sleep(250);
                }
                label1.Invoke(new MethodInvoker(delegate
                {
                    label1.Text = tef.Mensagem;
                }));
                Thread.Sleep(2000);
            }
            catch (Exception)
            {
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Refresh();

                if (tef.Autorizacao != null)
                { 
                    Pagamento = new PagamentoResultado()
                    {
                        Autorizacao = tef.Autorizacao,
                        Bandeira = tef.Bandeira,
                        Debito = tef.Debito,
                        ContaRecebivel = tef.Adquirente,
                        ViaEstabelecimento = tef.ViaEstabelecimento,
                        ViaCliente = tef.ViaCliente,
                    };

                    label1.Text += "\r\nCONFIRMANDO";

                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
            }

            Refresh();
            Thread.Sleep(2500);
            this.Close();
        }
    }
}