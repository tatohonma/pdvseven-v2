using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmCaixaOuServer : Form
    {
        public int Instalacao { get; set; }
        public frmCaixaOuServer()
        {
            Instalacao = -1;
            InitializeComponent();
        }

        private void btnCaixa_Click(object sender, EventArgs e)
        {
            Instalacao = 0;
            Close();
        }

        private void btnServidor_Click(object sender, EventArgs e)
        {
            Instalacao = 1;
            Close();
        }
    }
}
