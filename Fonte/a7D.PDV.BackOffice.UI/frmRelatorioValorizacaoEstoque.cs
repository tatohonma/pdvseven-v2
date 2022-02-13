using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmRelatorioValorizacaoEstoque : Form
    {
        private DataTable Relatorio;

        public frmRelatorioValorizacaoEstoque()
        {
            InitializeComponent();
        }

        private void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            dgvPrincipal.DataSource = null;

            Relatorio = EntradaSaida.RelatorioValorizacaoEstoque(dtpDataInicial.Value, ckbExibirProdutosInativos.Checked);
            dgvPrincipal.DataSource = Relatorio;
            dgvPrincipal.Columns[0].Visible = false;
            dgvPrincipal.Columns[2].Visible = false;
        }

        private void btnExportarCSV_Click(object sender, EventArgs e)
        {
            if (Relatorio != null)
            {
                var nomeArquivo = BackofficeUtil.NomeRelatorio( "RelatorioValorizacaoEstoque_" + DateTime.Now.ToString("yyyyMMdd-hhmmssmmm") + ".csv");
                BLL.Relatorio.ExportarParaTxt(Relatorio, nomeArquivo);

                MessageBox.Show("Relatório gerado em " + nomeArquivo);
            }
        }

        private void frmRelatorioValorizacaoEstoque_FormClosed(object sender, FormClosedEventArgs e)
        {
            Relatorio?.Dispose();
        }

        private void frmRelatorioValorizacaoEstoque_Load(object sender, EventArgs e)
        {
            GA.Post(this);
        }
    }
}
