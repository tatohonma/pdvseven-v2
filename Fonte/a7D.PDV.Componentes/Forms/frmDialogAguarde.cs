using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public partial class frmDialogAguarde : Form
    {
        public void SetMensagem(string msg)
        {
            lblMensagem.Text = msg;
        }
        public frmDialogAguarde()
        {
            InitializeComponent();
        }
        public frmDialogAguarde(string msg)
        {
            SetMensagem(msg);
        }

        private void frmDialogAguarde_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
