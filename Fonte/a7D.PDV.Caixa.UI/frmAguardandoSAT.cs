using a7D.PDV.BLL;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmAguardandoSAT : Form
    {
        private int idPdv;
        private int idUsuario;
        private string status = "Iniciando SAT";
        public bool ImprimirCupon { get; set; }

        public PedidoInformation Pedido1 { get; set; }
        public Exception Exception { get; set; }
        public RetornoSATInformation RetornoSat { get; private set; }
        private string ModeloImpressora { get; }
        private AreaImpressaoInformation AreaImpressao { get; }
        private bool GerarNf { get; }

        public frmAguardandoSAT(PedidoInformation pedido, bool gerarNf, int idPdv, int idUsuario, string modeloImpressora) : this(pedido, gerarNf, idPdv, idUsuario)
        {
            ModeloImpressora = modeloImpressora;
        }

        public frmAguardandoSAT(PedidoInformation pedido, bool gerarNf, int idPdv, int idUsuario, AreaImpressaoInformation areaImpressao) : this(pedido, gerarNf, idPdv, idUsuario)
        {
            AreaImpressao = areaImpressao;
        }

        private frmAguardandoSAT(PedidoInformation pedido, bool gerarNf, int idPdv, int idUsuario) : this()
        {
            Pedido1 = pedido;
            GerarNf = gerarNf;
            this.idPdv = idPdv;
            this.idUsuario = idUsuario;
        }

        public frmAguardandoSAT()
        {
            InitializeComponent();
        }

        private void frmAguardandoSAT_Load(object sender, EventArgs e)
        {
            backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                status = "Peparando cupom fiscal...";
                var venda = FiscalServices.Venda(Pedido1, GerarNf, idPdv, idUsuario);

                if (ConfiguracoesSistema.Valores.Fiscal == "NFCe")
                {
                    try
                    {
                        status = "Enviando NFCe...\r\n";
                        RetornoSat = venda.Enviar();
                        Exception = null;
                    }
                    catch (Exception ex)
                    {
                        Exception = ex;
                        status = "ERRO!\r\n" + ex.Message;
                    }
                }
                else // SAT
                {
                    int nTry = 0;
                    while (nTry++ < 3)
                    {
                        try
                        {
                            status = "Enviando SAT...\r\n";
                            if (nTry == 1)
                                status += "(enviando SAT)";
                            else
                                status += $"(enviando SAT, tentativa {nTry})";

                            RetornoSat = venda.Enviar();
                            Exception = null;
                            break;
                        }
                        catch (Exception ex)
                        {
                            Exception = ex;
                            status = "ERRO!\r\n" + ex.Message;
                            Thread.Sleep(3000);
                        }
                    }
                }

                if (RetornoSat == null)
                    throw new ExceptionPDV(CodigoErro.E500, Exception, status);

                Pedido1.RetornoSAT_venda = RetornoSat;
                status = $"Salvando retorno retorno";

                Pedido.Salvar(Pedido1);

                Exception msgErro = null;
                if (ImprimirCupon)
                {
                    if (!string.IsNullOrWhiteSpace(ModeloImpressora))
                    {
                        status = "Imprimindo cupom fiscal";
                        Invoke(new MethodInvoker(() =>
                        {
                            TopMost = false;
                            while (!CupomSATService.ImprimirCupomVenda(RetornoSat.arquivoCFeSAT, Pedido1.IDPedido.Value, ModeloImpressora, out msgErro))
                            {
                                if (MessageBox.Show("Erro ao imprimir o cupom fiscal", "ERRO NA IMPRESSORA", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Retry)
                                    break;
                            }
                        }));
                    }

                    else if (AreaImpressao != null)
                    {
                        status = $"Enviando para impressora\r\n{AreaImpressao.Nome}\r\n{AreaImpressao.NomeImpressora}";
                        OrdemProducaoServices.GerarSAT(RetornoSat.arquivoCFeSAT, Pedido1.IDPedido.Value, AreaImpressao.IDAreaImpressao.Value);
                    }
                }

                if (msgErro == null)
                    DialogResult = DialogResult.OK;
                else
                {
                    Exception = msgErro;
                    Exception.Data.Add("RetornoSat", RetornoSat?.IDRetornoSAT);
                    Logs.Erro(Exception);
                    DialogResult = DialogResult.Yes;
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("RetornoSat", RetornoSat?.IDRetornoSAT);
                Logs.Erro(ex);
                Exception = ex;
                DialogResult = DialogResult.No;
            }
        }

        private void frmAguardandoSAT_Shown(object sender, EventArgs e)
        {
            Refresh();
            backgroundWorker1.RunWorkerAsync();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblInfo.Text = status;
        }
    }
}
