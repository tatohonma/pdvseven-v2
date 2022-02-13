using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public partial class frmAutenticacaoTouch : FormTouch
    {
        public Boolean Fechar;
        Boolean AlterarUsuarioAutenticado;
        Boolean Admin;
        Boolean Gerente;
        Boolean Caixa;
        Boolean Garcom;
        public UsuarioInformation Usuario1;

        public frmAutenticacaoTouch(Boolean alterarUsuarioAutenticado, Boolean admin, Boolean gerente, Boolean caixa, Boolean garcom)
        {
            AlterarUsuarioAutenticado = alterarUsuarioAutenticado;
            Admin = admin;
            Gerente = gerente;
            Caixa = caixa;
            Garcom = garcom;

            InitializeComponent();
        }

        private void frmAutenticacao_Load(object sender, EventArgs e)
        {
            teclado1.TecladoClick += new a7D.PDV.Componentes.Controles.TecladoClickEventHandler(teclado1_TecladoClick);
            teclado1.Confirmar += new EventHandler(teclado1_Confirmar);
        }

        protected void teclado1_TecladoClick(object sender, EventArgs e, Int32 n)
        {
            txtIdentificacao.Text += n.ToString();
        }

        protected void teclado1_Confirmar(object sender, EventArgs e)
        {
            if (txtIdentificacao.Text == "9999")
            {
                Fechar = true;
                this.Close();
            }
            else
            {
                Confirmar();
            }
        }

        private void Confirmar()
        {
            UsuarioInformation usuario;

            try
            {
                usuario = Usuario.Autenticar(txtIdentificacao.Text);

                if (
                    usuario.PermissaoAdm == true && Admin == true ||
                    usuario.PermissaoGerente == true && Gerente == true ||
                    usuario.PermissaoCaixa == true && Caixa == true ||
                    usuario.PermissaoGarcom == true && Garcom == true)
                {
                    if (AlterarUsuarioAutenticado == true)
                        AC.RegitraUsuario(usuario);

                    Usuario1 = usuario;

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    throw new ExceptionPDV(CodigoErro.A011);
                }
            }
            catch (Exception _e)
            {
                lblMensagem.Text = _e.Message;
                txtIdentificacao.Text = "";
            }
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (txtIdentificacao.Text.Length > 0)
                txtIdentificacao.Text = txtIdentificacao.Text.Substring(0, txtIdentificacao.Text.Length - 1);
        }

        private void txtIdentificacao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                Confirmar();
        }
    }
}
