using System;
using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public partial class frmEscolherEmail : Form
    {

        public string Destinatario {
            get {
                return txtDestinatario.Text;
            }
            set {
                txtDestinatario.Text = value;
            }
        }


        public frmEscolherEmail()
        {
            InitializeComponent();
        }


        private void frmEscolherEmail_Load(object sender, EventArgs e)
        {

        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (txtDestinatario.Text != "")
            {
                Destinatario = txtDestinatario.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {

            }


        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
