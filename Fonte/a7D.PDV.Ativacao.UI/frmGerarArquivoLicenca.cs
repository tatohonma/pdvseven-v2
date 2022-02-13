using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using a7D.PDV.BLL;

namespace a7D.PDV.Ativacao.UI
{
    public partial class frmGerarArquivoLicenca : Form
    {
        public frmGerarArquivoLicenca()
        {
            InitializeComponent();
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            string scriptDB = txtScript.Text;
            string scriptDBCript = CryptMD5.Criptografar(scriptDB);
            string arquivo = txtArquivo.Text + ".lic";

            StreamWriter sw = new StreamWriter(arquivo, false);
            sw.Write(scriptDBCript);
            sw.Close();

            FileInfo fi = new FileInfo(arquivo);

            MessageBox.Show("Arquivo gerado em " + fi.FullName);
        }
    }
}
