using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.MigracaoImagens.UI
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnImagensProduto_Click(object sender, EventArgs e)
        {
            new frmMigrarProdutos().ShowDialog();
        }

        private void btnImagensTema_Click(object sender, EventArgs e)
        {
            new frmMigrarTema().ShowDialog();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.ConnectionStrings["connectionString"] == null)
            {
                new Configurador.UI.frmConfigurarConexaoDB().ShowDialog();
                BeginInvoke(new MethodInvoker(Close));
            }
        }
    }
}
