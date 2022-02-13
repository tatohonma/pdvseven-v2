using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.BLL;
using System.IO;
using a7D.PDV.DAL;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmConfigurarLicenca : Form
    {
        public frmConfigurarLicenca()
        {
            InitializeComponent();
        }

        private void btnSelecionarArquivo_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtArquivo.Text = openFileDialog1.FileName;
            }
        }

        private void btnAtivar_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(txtArquivo.Text);

            String scriptDBCript = sr.ReadLine();
            String scriptDB = BLL.PDV.Descriptografar(scriptDBCript);

            DB.ExecutarQuery(scriptDB);
            this.Close();
        }
    }
}
