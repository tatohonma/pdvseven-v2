using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmRelatorioMovimentacoes : Form
    {

        private readonly CultureInfo _cultureInfo = new CultureInfo("pt-BR");
        private DataTable Relatorio;

        public frmRelatorioMovimentacoes()
        {
            InitializeComponent();
        }

        private void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            dgvPrincipal.DataSource = null;

            Relatorio = EntradaSaida.EstoqueMovimentacoes(dtpDataInicial.Value.Date, dtpDataFinal.Value.Date, ckbExibirProdutosInativos.Checked);
            //for (int i = 0; i < Relatorio.Columns.Count; i++)
            //{
            //    DataColumn dc = Relatorio.Columns[i];
            //    if (i > 3 && (i + 1 != Relatorio.Columns.Count))
            //        dgvPrincipal.Columns.Add(new DataGridViewLinkColumn { HeaderText = dc.ColumnName, Name = dc.ColumnName, DataPropertyName = dc.ColumnName, VisitedLinkColor = Color.White, ActiveLinkColor = Color.White });
            //    else
            //        dgvPrincipal.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = dc.ColumnName, Name = dc.ColumnName, DataPropertyName = dc.ColumnName });
            //}

            dgvPrincipal.DataSource = Relatorio;
            dgvPrincipal.Columns[0].Visible = false;
        }

        private void dgvPrincipal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //var cell = dgvPrincipal.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewLinkCell;
            //if (cell != null)
            //{
            //    DateTime data = DateTime.ParseExact(dgvPrincipal.Columns[e.ColumnIndex].Name, "dd/MM/yyyy", _cultureInfo);
            //    var idProduto = Convert.ToInt32(dgvPrincipal.Rows[e.RowIndex].Cells[0].Value);
            //    new frmRelatorioMovDetalhado(data, idProduto).ShowDialog();
            //}
        }

        private void frmRelatorioMovimentacoes_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            dtpDataFinal.Value = DateTime.Now.Date;
            dtpDataInicial.Value = DateTime.Now.AddDays(-15).Date;
        }

        private void frmRelatorioMovimentacoes_FormClosed(object sender, FormClosedEventArgs e)
        {
            Relatorio?.Dispose();
        }

        private void btnExportarCSV_Click(object sender, EventArgs e)
        {
            if (Relatorio != null)
            {
                var nomeArquivo = BackofficeUtil.NomeRelatorio("RelatorioMovimentacoes_" + DateTime.Now.ToString("yyyyMMdd-hhmmssmmm") + ".csv");
                BLL.Relatorio.ExportarParaTxt(Relatorio, nomeArquivo);

                MessageBox.Show("Relatório gerado em " + nomeArquivo);
            }
        }
    }
}
