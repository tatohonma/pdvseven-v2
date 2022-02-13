using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.Ativacao.UI
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnGerarArquivoLicenca_Click(object sender, EventArgs e)
        {
            frmGerarArquivoLicenca form = new frmGerarArquivoLicenca();
            form.ShowDialog();
        }

        private void btnGerarChaveAtivacao_Click(object sender, EventArgs e)
        {
            frmGerarChaveAtivacao form = new frmGerarChaveAtivacao();
            form.ShowDialog();
        }
    }
}
