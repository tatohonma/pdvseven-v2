using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmTemaEditar : Form
    {
        private TemaCardapioInformation TemaCardapio1;

        public frmTemaEditar()
        {
            InitializeComponent();

            TemaCardapio1 = new TemaCardapioInformation();
        }

        public frmTemaEditar(Int32 idCategoriaProduto)
        {
            InitializeComponent();

            TemaCardapio1 = TemaCardapio.Carregar(idCategoriaProduto);

            txtNome.Text = TemaCardapio1.Nome;
            txtXML.Text = TemaCardapio1.XML;
        }

        private void frmTemaEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar() == true)
            {
                TemaCardapio1.Nome = txtNome.Text;
                TemaCardapio1.XML = txtXML.Text;

                TemaCardapio.Salvar(TemaCardapio1);
                TemaCardapioPDV.AtualizarUltimaAlteracao();

                this.Close();
            }
        }

        private Boolean Validar()
        {
            String msg = "";

            if (txtNome.Text == "")
                msg += "Campo \"Nome\" é obrigatório. \n";

            if (msg.Length > 0)
            {
                MessageBox.Show(msg, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelecionarTudo_Click(object sender, EventArgs e)
        {
            txtXML.SelectAll();
            txtXML.Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            var keyCode = (Keys)(msg.WParam.ToInt32() &
                                  Convert.ToInt32(Keys.KeyCode));
            if ((msg.Msg == WM_KEYDOWN && keyCode == Keys.A)
                && (ModifierKeys == Keys.Control)
                && txtXML.Focused)
            {
                txtXML.SelectAll();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
