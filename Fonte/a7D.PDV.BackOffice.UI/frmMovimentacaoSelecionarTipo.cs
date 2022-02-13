using a7D.PDV.BLL;
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
    public partial class frmMovimentacaoSelecionarTipo : Form
    {
        public frmMovimentacaoSelecionarTipo()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            new frmMovimentacaoEditar("+").ShowDialog();
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            new frmMovimentacaoEditar("-").ShowDialog();
            Close();
        }

        private void frmMovimentacaoSelecionarTipo_Load(object sender, EventArgs e)
        {
            GA.Post(this);
        }
    }
}
