using a7D.PDV.Model;
using a7D.PDV.SAT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.Pagamento.UI
{
    public partial class frmAguardandoSAT : Form
    {
        private int idPdv;
        private int idUsuario;

        public PedidoInformation Pedido1 { get; set; }
        public Exception Exception { get; set; }
        public RetornoSATInformation RetornoSat { get; private set; }

        public frmAguardandoSAT(PedidoInformation pedido, int idPdv, int idUsuario)
        {
            Pedido1 = pedido;
            this.idPdv = idPdv;
            this.idUsuario = idUsuario;
            InitializeComponent();
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
                RetornoSat = new SAT.SAT().Venda(Pedido1, idPdv, idUsuario).Enviar();

                DialogResult = DialogResult.OK;
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
