using a7D.PDV.BLL;
using a7D.PDV.EF;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmEntregadorEditar : Form
    {

        private EntregadorInformation Entregador1 { get; }

        public frmEntregadorEditar()
        {
            Entregador1 = new EntregadorInformation { Ativo = true, Excluido = false };
            InitializeComponent();
        }

        public frmEntregadorEditar(int idEntregador)
        {
            Entregador1 = Entregador.Carregar(idEntregador);
            InitializeComponent();
        }

        private void frmEntregadorEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            txtNome.Text = Entregador1.Nome;
            ckbAtivo.Checked = Entregador1.Ativo == true;
            cbbGateway.Fill<tbGateway>(Entregador1.IDGateway, true, g => g.IDGateway > 100); // Somente gateways de entrega
        }

        public bool Validar()
        {
            var msg = string.Empty;
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                msg += "Informe o nome";
            }

            if (string.IsNullOrWhiteSpace(msg))
                return true;

            MessageBox.Show(msg, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                Entregador1.Nome = txtNome.Text;
                Entregador1.Ativo = ckbAtivo.Checked;
                Entregador1.IDGateway = (int)cbbGateway.SelectedValue;

                Entregador.Salvar(Entregador1);

                Close();
            }
        }
    }
}
