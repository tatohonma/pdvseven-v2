using System.Windows.Forms;

namespace a7D.PDV.Integracao.Pagamento
{
    public partial class frmFidelidade : Form
    {
        public PagamentoResultado Pagamento { get; private set; }

        public frmFidelidade()
        {
            InitializeComponent();
        }

        public frmFidelidade(string loja, string pdv, int pedido, decimal valor) : this()
        {
        }

        private void txtCartao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                Pagamento = new PagamentoResultado()
                {
                    Autorizacao = txtCartao.Text,
                    Bandeira = "Todo",
                    Debito = false,
                    ContaRecebivel = "Todo",
                };
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (e.KeyChar == 27)
            {
                this.Close();
            }
        }
    }
}
