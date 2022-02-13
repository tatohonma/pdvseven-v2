using System;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;

namespace a7D.PDV.Componentes
{
    public partial class frmAutenticacao : Form
    {
        public UsuarioInformation Usuario1 { get; private set; }

        Boolean AlterarUsuarioAutenticado;
        Boolean Administrador;
        Boolean Gerente;
        Boolean Caixa;
        Boolean Garcom;

        public frmAutenticacao(Boolean alterarUsuarioAutenticado, Boolean administrador, Boolean gerente, Boolean caixa, Boolean garcom)
        {
            AlterarUsuarioAutenticado = alterarUsuarioAutenticado;
            Administrador = administrador;
            Gerente = gerente;
            Caixa = caixa;
            Garcom = garcom;

            InitializeComponent();
        }

        //public frmAutenticacao(Boolean alterarUsuarioAutenticado, Boolean gerente, Boolean caixa, Boolean garcom)
        //{
        //    AlterarUsuarioAutenticado = alterarUsuarioAutenticado;
        //    Gerente = gerente;
        //    Caixa = caixa;
        //    Garcom = garcom;
        //    InitializeComponent();
        //}

        private void frmAutenticacao2_Load(object sender, EventArgs e)
        {
            GA.Post(this);
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            String senha = txtSenha.Text;

            try
            {
                Usuario1 = Usuario.Autenticar(senha);

                if (Usuario1.PermissaoAdm == true && Administrador ||
                    Usuario1.PermissaoGerente == true && Gerente ||
                    Usuario1.PermissaoCaixa == true && Caixa ||
                    Usuario1.PermissaoGarcom == true && Garcom)
                {


                    if (AlterarUsuarioAutenticado == true)
                        AC.RegitraUsuario(Usuario1);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    throw new ExceptionPDV(CodigoErro.A011);
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
