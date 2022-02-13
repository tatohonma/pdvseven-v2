using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmMotivoCancelamentoEditar : Form
    {
        private MotivoCancelamentoInformation MotivoCancelamento1 { get; set; }
        public frmMotivoCancelamentoEditar()
        {
            MotivoCancelamento1 = new MotivoCancelamentoInformation();
            InitializeComponent();
        }

        public frmMotivoCancelamentoEditar(int idMotivoCancelamento) : this()
        {
            MotivoCancelamento1 = MotivoCancelamento.Carregar(idMotivoCancelamento);
        }

        private void frmMotivoCancelamentoEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            txtNome.Text = MotivoCancelamento1.Nome;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validar())
                {
                    MotivoCancelamento1.Nome = txtNome.Text;
                    MotivoCancelamento.Salvar(MotivoCancelamento1);
                    Close();
                }
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E013, ex);
            }
        }

        private bool Validar()
        {
            var msg = string.Empty;
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                msg += "Nome não pode estar em branco!";
            }

            if (!string.IsNullOrWhiteSpace(msg))
            {
                MessageBox.Show(msg, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
