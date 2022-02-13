using a7D.PDV.BLL;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPedidoProdutosValor : Form
    {
        public Decimal Valor;
        public String Notas;

        public frmPedidoProdutosValor()
        {
            InitializeComponent();
        }

        private void frmPedidoProdutosValor_Load(object sender, EventArgs e)
        {
            GA.Post(this);
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (txtValor.Text == "")
                Valor = 0;
            else if (decimal.TryParse(txtValor.Text, out Valor) && Produto.ValorValido(Valor))
            {
                this.DialogResult = DialogResult.OK;
                Notas = txtNotas.Text;
                this.Close();
            }
            else
            {
                Valor = 0;
                MessageBox.Show("Valor acima do máximo permitido");
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void SomenteNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || (e.KeyChar == (char)44 && (txt.Text.Contains(",") == true || txt.Text.Length == 0)))
                e.Handled = true;
        }
    }
}
