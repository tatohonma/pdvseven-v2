using a7D.PDV.BLL;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using a7D.PDV.SAT;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmAguardandoSAT : Form
    {
        private int idPdv;
        private int idUsuario;

        public PedidoInformation Pedido1 { get; set; }
        public Exception Exception { get; set; }
        public RetornoSATInformation RetornoSat { get; private set; }
        private string ModeloImpressora { get; }
        private bool GerarNf { get; }

        public frmAguardandoSAT(PedidoInformation pedido, bool gerarNf, int idPdv, int idUsuario, string modeloImpressora, string mensagem) : this(pedido, gerarNf, idPdv, idUsuario)
        {
            ModeloImpressora = modeloImpressora;
            lblMsg.Text = mensagem;
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
                RetornoSat = FiscalServices
                    .Venda(Pedido1, GerarNf, idPdv, idUsuario)
                    .Enviar();

                Pedido1.RetornoSAT_venda = RetornoSat;
                Pedido.Salvar(Pedido1);

                Exception msgErro = null;
                if (!string.IsNullOrWhiteSpace(ModeloImpressora))
                    CupomSATService.ImprimirCupomVenda(RetornoSat.arquivoCFeSAT, Pedido1.IDPedido.Value, ModeloImpressora, out msgErro);

                if (msgErro == null)
                    DialogResult = DialogResult.OK;
                else
                {
                    Exception = msgErro;
                    DialogResult = DialogResult.Yes;
                }
            }
            catch (Exception e_)
            {
                Exception = e_;
                
                DialogResult = DialogResult.No;
            }
        }

        private void frmAguardandoSAT_Shown(object sender, EventArgs e)
        {
            Refresh();
            backgroundWorker1.RunWorkerAsync();
        }
    }
}
