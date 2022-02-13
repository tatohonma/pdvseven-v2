using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Pagamento.UI
{
    public partial class frmPedidoProdutoValor : Form
    {

        private string Formato => Tipo == TipoNumero.DINHEIRO ? "R$ #,##0.00" : (Tipo == TipoNumero.DECIMAL ? "#,##0.00" : "#");

        public decimal Valor { get; private set; }
        private TipoNumero Tipo = TipoNumero.DINHEIRO;

        public enum TipoNumero
        {
            DINHEIRO,
            DECIMAL,
            INTEIRO
        }


        public frmPedidoProdutoValor()
        {
            InitializeComponent();
            Valor = 0m;
        }

        public frmPedidoProdutoValor(TipoNumero tipo) : this()
        {
            Tipo = tipo;
        }

        private void frmPedidoProdutoValor_Load(object sender, EventArgs e)
        {
            teclado1.TecladoClick += Teclado1_TecladoClick;
            teclado1.Confirmar += Teclado1_Confirmar;
            txtValor.Text = Valor.ToString(Formato);
        }

        private void Teclado1_Confirmar(object sender, EventArgs e)
        {
            if (Valor > 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void Teclado1_TecladoClick(object sender, EventArgs e, int n)
        {
            if (Tipo == TipoNumero.INTEIRO)
                Valor = (Valor * 10) + n;
            else
                Valor = (Valor * 10) + ((decimal)n / 100);

            txtValor.Text = Valor.ToString(Formato);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (Valor > 0)
                Valor = Valor / 10;
            if (Tipo == TipoNumero.INTEIRO)
                Valor = Math.Truncate(Valor);
            else
            {
                Valor = TruncateDecimal(Valor, 2);
            }
            txtValor.Text = Valor.ToString(Formato);
        }

        public decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            int tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }
    }
}
