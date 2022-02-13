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
    public partial class frmUsuarioEditar : Form
    {
        private UsuarioInformation Usuario1;

        public frmUsuarioEditar()
        {
            InitializeComponent();

            Usuario1 = new UsuarioInformation();
        }

        public frmUsuarioEditar(Int32 idUsuario)
        {
            InitializeComponent();

            Usuario1 = Usuario.Carregar(idUsuario);

            txtNome.Text = Usuario1.Nome;
            txtSenha1.Text = Usuario1.Senha;
            txtSenha2.Text = Usuario1.Senha;
            ckbAtivo.Checked = Usuario1.Ativo.Value;

            ckbPermissaoAdm.Checked = Usuario1.PermissaoAdm.Value;
            ckbPermissaoGerente.Checked = Usuario1.PermissaoGerente.Value;
            ckbPermissaoCaixa.Checked = Usuario1.PermissaoCaixa.Value;
            ckbPermissaoGarcom.Checked = Usuario1.PermissaoGarcom.Value;
        }

        private void frmUsuarioEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar() == true)
            {
                Usuario1.Nome = txtNome.Text;
                Usuario1.Senha = txtSenha1.Text;
                Usuario1.Ativo = ckbAtivo.Checked;

                Usuario1.PermissaoAdm = ckbPermissaoAdm.Checked;
                Usuario1.PermissaoGerente = ckbPermissaoGerente.Checked;
                Usuario1.PermissaoCaixa = ckbPermissaoCaixa.Checked;
                Usuario1.PermissaoGarcom = ckbPermissaoGarcom.Checked;

                Usuario.Salvar(Usuario1);

                this.Close();
            }
        }

        private Boolean Validar()
        {
            String msg = "";

            if (txtNome.Text == "")
                msg += "Campo \"Nome\" é obrigatório. \n";

            if (txtSenha1.Text != txtSenha2.Text)
                msg += "Campo \"Chave de Acesso\" e \"Confirmação\" estão diferentes. \n";

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
    }
}
