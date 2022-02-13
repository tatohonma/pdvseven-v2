using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmResumoDia : FormTouch
    {
        public frmResumoDia()
        {
            InitializeComponent();
        }

        private void frmResumoDia_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            dgvPorDia.DataSource = BLL.Relatorio.QuantidadePedidosPorDia();
            dgvPorTipoEntrada.DataSource = BLL.Relatorio.QuantidadePedidosPorTipoEntrada();
            dgvPorTipoPagamento.DataSource = BLL.Relatorio.ValorPorTipoPagamento();
        }
    }
}
