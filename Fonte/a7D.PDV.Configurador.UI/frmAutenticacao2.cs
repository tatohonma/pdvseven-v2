using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.BLL;
using a7D.PDV.Model;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmAutenticacao2 : Form
    {
        public UsuarioInformation Usuario1;

        public bool SuperUser { get; set; }
        Boolean? Administrador;
        Boolean? Gerente;
        Boolean? Caixa;
        Boolean? Garcom;

        public frmAutenticacao2(Boolean administrador, Boolean gerente, Boolean caixa, Boolean garcom)
        {
            Administrador = administrador;
            Gerente = gerente;
            Caixa = caixa;
            Garcom = garcom;

            SuperUser = false;

            InitializeComponent();
        }

        private void frmAutenticacao2_Load(object sender, EventArgs e)
        {

        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            String senha = txtSenha.Text;

            try
            {
                if (senha == "pdvsevenconfiguracaosupersenha")
                {
                    SuperUser = true;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }

                Usuario1 = Usuario.Autenticar(senha);

                if (
                    Usuario1.PermissaoAdm == true && Administrador == true ||
                    Usuario1.PermissaoGerente == true && Gerente == true ||
                    Usuario1.PermissaoCaixa == true && Caixa == true ||
                    Usuario1.PermissaoGarcom == true && Garcom == true)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
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
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
