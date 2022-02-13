using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using a7D.PDV.Componentes;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmFecharCaixa : FormTouch
    {
        frmPrincipal FormPrincipal;

        public frmFecharCaixa(frmPrincipal formPrincipal)
        {
            FormPrincipal = formPrincipal;
            InitializeComponent();
        }

        private void frmFecharCaixaTurno_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarTipoPagamento();
            if (dgvValoresRecebidos.Rows.Count > 0)
                dgvValoresRecebidos.CurrentCell = dgvValoresRecebidos.Rows[0].Cells["ValorFechamento"];
        }

        private void CarregarTipoPagamento()
        {
            var list = from l in BLL.CaixaValorRegistro.ListarPorCaixa(frmPrincipal.Caixa1.IDCaixa.Value)
                       select new { l.IDCaixaValorRegistro, Nome = l.TipoPagamento.Nome, l.ValorAbertura };

            dgvValoresRecebidos.DataSource = null;
            dgvValoresRecebidos.DataSource = list.ToArray();
        }

        private void btnFecharCaixa_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                this.Refresh();

                if (Validar() == false)
                {
                    this.Enabled = true;
                    return;
                }

                CaixaValorRegistroInformation registro;
                List<String> relatorio;

                foreach (DataGridViewRow item in dgvValoresRecebidos.Rows)
                {
                    registro = new CaixaValorRegistroInformation();
                    registro = CaixaValorRegistro.Carregar(Convert.ToInt32(item.Cells["IDCaixaValorRegistro"].Value));

                    if (item.Cells["ValorFechamento"].Value == null)
                        registro.ValorFechamento = 0;
                    else
                        registro.ValorFechamento = Convert.ToDecimal(item.Cells["ValorFechamento"].Value);

                    CaixaValorRegistro.Salvar(registro);
                }

                frmPrincipal.Caixa1.DtFechamento = DateTime.Now;
                BLL.Caixa.Salvar(frmPrincipal.Caixa1);

                relatorio = Relatorio.FechamentoCaixa(frmPrincipal.Caixa1.IDCaixa.Value, false);
                frmPrincipal.Impressora1.RelatorioGerencial(relatorio, 7);
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E807, ex);
            }
            this.Close();
        }

        private Boolean Validar()
        {
            Boolean ret = true;
            Decimal n = 0;

            foreach (DataGridViewRow item in dgvValoresRecebidos.Rows)
            {
                if (item.Cells["ValorFechamento"].Value != null && Decimal.TryParse(item.Cells["ValorFechamento"].Value.ToString(), out n) == false)
                {
                    MessageBox.Show("O \"Valor\" deve ser numérico!");
                    ret = false;

                    break;
                }
            }

            return ret;
        }

        private void dgvValoresRecebidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedCell = dgvValoresRecebidos.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (selectedCell.OwningColumn.Name != "ValorFechamento")
            {
                dgvValoresRecebidos.CurrentCell = dgvValoresRecebidos.Rows[e.RowIndex].Cells["ValorFechamento"];
            }
        }

        private void dgvValoresRecebidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedCell = dgvValoresRecebidos.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (selectedCell.OwningColumn.Name != "ValorFechamento")
            {
                dgvValoresRecebidos.CurrentCell = dgvValoresRecebidos.Rows[e.RowIndex].Cells["ValorFechamento"];
            }
        }
    }
}
