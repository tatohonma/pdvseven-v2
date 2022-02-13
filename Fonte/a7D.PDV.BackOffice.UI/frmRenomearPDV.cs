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
    public partial class frmRenomearPDV : Form
    {
        private PDVInformation PDV1 { get; }
        public frmRenomearPDV()
        {
            InitializeComponent();
        }

        public frmRenomearPDV(PDVInformation pdv) : this()
        {
            PDV1 = pdv;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                PDV1.Nome = txtNome.Text;
                PDV1.UltimaAlteracao = DateTime.Now;
                BLL.PDV.Salvar(PDV1);
                Close();
            }
        }

        private bool Validar()
        {
            var msg = default(string);

            if (string.IsNullOrWhiteSpace(txtNome.Text))
                msg += "Insira um nome";

            if (!string.IsNullOrWhiteSpace(msg))
            {
                MessageBox.Show(msg, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void frmRenomearPDV_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            txtNome.Text = PDV1?.Nome;
        }
    }
}
