using a7D.PDV.BLL;
using a7D.PDV.EF;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmTaxaEntregaEditar : Form
    {
        private TaxaEntregaInformation TaxaEntrega1 { get; set; }

        public frmTaxaEntregaEditar()
        {
            TaxaEntrega1 = new TaxaEntregaInformation { Excluido = false, Ativo = true };
            InitializeComponent();
        }

        public frmTaxaEntregaEditar(int idTaxaEntrega)
        {
            TaxaEntrega1 = TaxaEntrega.Carregar(idTaxaEntrega);
            InitializeComponent();
        }

        private void frmTaxaEntregaEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            txtNome.Text = TaxaEntrega1.Nome;
            txtValor.Text = TaxaEntrega1.Valor?.ToString("#,##0.00");
            ckbAtivo.Checked = TaxaEntrega1.Ativo == true;
            cbbPacote.Fill<tbTamanhoPacote>(TaxaEntrega1.IDTamanhoPacote, true);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                TaxaEntrega1.Nome = txtNome.Text;
                TaxaEntrega1.Valor = Convert.ToDecimal(txtValor.Text);
                TaxaEntrega1.IDTamanhoPacote = (int)cbbPacote.SelectedValue;
                TaxaEntrega1.Ativo = ckbAtivo.Checked;


                TaxaEntrega.Salvar(TaxaEntrega1);
                Close();
            }
        }

        public bool Validar()
        {
            var msg = string.Empty;

            if (string.IsNullOrWhiteSpace(txtNome.Text))
                msg += "Informe o nome";

            if (!decimal.TryParse(txtValor.Text, out decimal valor))
                msg += "Informe o valor";
            else if (valor < 0)
                msg += "Valor inválido";

            if (string.IsNullOrWhiteSpace(msg))
                return true;

            MessageBox.Show(msg, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return false;
        }
    }
}
