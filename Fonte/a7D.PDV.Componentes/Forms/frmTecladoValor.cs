using System;
using System.Drawing;
using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public delegate decimal ObterValorEventHandler();

    /// <summary>
    /// Formulario de edição de valores para produto, pagamento ou CPF
    /// </summary>
    /// <remarks>
    /// Teste: T12
    /// </remarks>
    public partial class frmTecladoValor : FormTouch
    {
        public int MaxDigits { get; set; } = 9;
        public decimal Divisor { get; set; } = 100;
        public string Formato { get; set; } = "R$ #,##0.00";
        public int LimiteDigitoFormato1 { get; set; } = 9;
        public string Formato2 { get; set; }
        public bool PermitirZero { get; set; } = false;
        public bool JaEZero { get; private set; } = false;
        public decimal Valor { get; private set; } = 0;
        private decimal ZeroResult { get; set; } = 0;
        private decimal ValorPadrao = 0;

        public frmTecladoValor()
        {
            InitializeComponent();
        }

        public frmTecladoValor(decimal valorPadrao = 0, decimal valorInicial = 0) : this()
        {
            this.ValorPadrao = valorPadrao;
            this.Valor = valorInicial;
        }

        private void AtualizaValor()
        {
            SuspendLayout();
            if (Valor == 0 && (!PermitirZero || !JaEZero))
            {
                txtValor.ForeColor = Color.Gray;
                txtValor.Text = ValorPadrao.ToString(Formato);
                JaEZero = true;
                ZeroResult = ValorPadrao;
            }
            else if (Valor == 0 && PermitirZero)
            {
                txtValor.ForeColor = Color.Green;
                txtValor.Text = 0.ToString(Formato);
                ZeroResult = 0;
            }
            else
            {
                txtValor.ForeColor = Color.Black;
                if (Formato2 != null && Valor.ToString().Length > LimiteDigitoFormato1)
                    txtValor.Text = Valor.ToString(Formato2);
                else
                    txtValor.Text = Valor.ToString(Formato);
            }
            txtValor.SelectionStart = txtValor.Text.Length;
            txtValor.SelectionLength = 0;
            ResumeLayout();
        }


        private void frmPedidoProdutoValor_Load(object sender, EventArgs e)
        {
            AtualizaValor();
        }

        private void Teclado_Click(object sender, EventArgs e, int n)
        {
            if (Valor.ToString().Length >= MaxDigits)
                return;

            Valor = (Valor * 10) + ((decimal)n / Divisor);
            AtualizaValor();
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (Valor > 0)
            {
                Valor = Valor / 10;
                JaEZero = false;
            }

            Valor = ((Int64)Math.Truncate(Divisor * Valor)) / Divisor;
            AtualizaValor();
        }

        private void Teclado_Confirmar(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (Valor == 0)
                Valor = ZeroResult;

            Close();
        }

        private void frmPedidoProdutoValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
                Teclado_Click(sender, e, int.Parse(e.KeyChar.ToString()));
            else if (e.KeyChar == (int)Keys.Back)
                btnApagar_Click(sender, e);
            else if (e.KeyChar == (int)Keys.Escape)
                Close();
        }
    }
}
