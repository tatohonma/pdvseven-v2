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

namespace a7D.PDV.Autenticacao.UI
{
    public partial class frmAutenticacao2 : Form
    {
        public UsuarioInformation Usuario1 { get; private set; }

        bool? Administrador;
        bool? Gerente;
        bool? Caixa;
        bool? Garcom;

        public frmAutenticacao2(bool administrador, bool gerente, bool caixa, bool garcom)
        {
            Administrador = administrador;
            Gerente = gerente;
            Caixa = caixa;
            Garcom = garcom;

            InitializeComponent();
        }

        public frmAutenticacao2(bool gerente, bool caixa, bool garcom) : this(false, gerente, caixa, garcom)
        { }

        private void frmAutenticacao2_Load(object sender, EventArgs e)
        {

        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string senha = txtSenha.Text;

            try
            {
                Usuario1 = Usuario.Autenticar(senha);

                if (
                    Usuario1.PermissaoAdm == true && Administrador == true ||
                    Usuario1.PermissaoGerente == true && Gerente == true ||
                    Usuario1.PermissaoCaixa == true && Caixa == true ||
                    Usuario1.PermissaoGarcom == true && Garcom == true)
                {

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    throw new Exception("Usuário sem permissão para executar função!");
                }
            }
            catch (Exception _e)
            {
                Usuario1 = null;
                lblMensagem.Text = _e.Message;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
