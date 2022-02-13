using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmValidacaoOffline : Form
    {
        public frmValidacaoOffline()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Enabled = false;
                Cursor = Cursors.WaitCursor;
                Refresh();
                if (BLL.PDV.VerificarAtivacaoOffline(txtServidor.Text))
                {
                    BLL.PDV.AlterarDataValidade(DateTime.Now.AddDays(30));
                    MessageBox.Show("Ativado com sucesso!\nPor favor reinicie a aplicação.", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    Enabled = true;
                    Cursor = Cursors.Default;
                    Refresh();
                    throw new Exception("Chave Inválida");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
