using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public partial class frmNumero : Form
    {
        private string valor = string.Empty;

        public long Valor { get { return Convert.ToInt64(valor); } }

        public frmNumero()
        {
            InitializeComponent();
        }

        private void frmPedidoProdutoValor_Load(object sender, EventArgs e)
        {
            teclado1.TecladoClick += Teclado1_TecladoClick;
            teclado1.Confirmar += Teclado1_Confirmar;
        }

        private void Teclado1_Confirmar(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                valor = "0";
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Teclado1_TecladoClick(object sender, EventArgs e, int n)
        {
            valor += n;
            txtValor.Text = valor;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(valor) == false)
                txtValor.Text = valor = valor.Substring(0, valor.Length - 1);
        }
    }
}
