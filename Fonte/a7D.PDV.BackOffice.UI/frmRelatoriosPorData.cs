using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.BLL;
using System.IO;
using System.Configuration;
using a7D.PDV.Model;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmRelatoriosPorData : Form
    {
        DataTable DtRelatorio;
        private RelatorioInformation RelatorioAtual { get; set; }

        public frmRelatoriosPorData()
        {
            InitializeComponent();
        }

        private void frmRelatorios_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            cbbRelatorio.DataSource = Relatorio.Listar().Where(l => l.TipoRelatorio.IDTipoRelatorio == 2).OrderBy(l => l.Ordem).ToList();

            dtpInicio.Value = DateTime.Now;
            dtpTermino.Value = DateTime.Now;
        }

        private void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            Int32 idRelatorio;
            DateTime dtInicio;
            DateTime dtTermino;

            dgvPrincipal.DataSource = null;
            dgvPrincipal.Rows.Clear();
            dgvPrincipal.Columns.Clear();

            if (cbbRelatorio.SelectedItem.ToString() != "" && dtpInicio != null && dtpTermino != null)
            {
                idRelatorio = ((RelatorioInformation)cbbRelatorio.SelectedItem).IDRelatorio.Value;
                RelatorioAtual = Relatorio.Carregar(idRelatorio);
                dtInicio = dtpInicio.Value.Date;
                dtTermino = dtpTermino.Value.Date.AddHours(24).AddSeconds(-1);

                DtRelatorio = Relatorio.CarregarRelatorioPorData(idRelatorio, dtInicio, dtTermino);

                var dtSumarizada = DtRelatorio.Totalizar(RelatorioAtual?.Totalizador);

                dgvPrincipal.DataSource = dtSumarizada;

                var sumario = dtSumarizada.Rows.Count > DtRelatorio.Rows.Count;

                if (sumario)
                {
                    DtRelatorio = dtSumarizada;
                    EstilizarSumario();
                }
            }
            else
            {
                MessageBox.Show("Favor selecionar o tipo de relatório, a data de início e de fim!");
            }
        }

        private void btnExportarCSV_Click(object sender, EventArgs e)
        {
            if (DtRelatorio != null)
            {
                Int32 idRelatorio = ((RelatorioInformation)cbbRelatorio.SelectedItem).IDRelatorio.Value;
                RelatorioInformation relatorio = Relatorio.Carregar(idRelatorio);

                String nomeArquivo = BackofficeUtil.NomeRelatorio(relatorio.Nome.ToLower().Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmssmmm") + ".csv");
                BLL.Relatorio.ExportarParaTxt(DtRelatorio, nomeArquivo);

                MessageBox.Show("Relatório gerado em " + nomeArquivo);
            }
        }

        private void EstilizarSumario()
        {
            if (dgvPrincipal.Rows.Count < 1)
                return;

            var row = dgvPrincipal.Rows[dgvPrincipal.Rows.Count - 1];

            var style = default(DataGridViewCellStyle);

            if (row.Index % 2 == 0)
                style = new DataGridViewCellStyle(dgvPrincipal.AlternatingRowsDefaultCellStyle);
            else
                style = new DataGridViewCellStyle(dgvPrincipal.DefaultCellStyle);
            style.Font = new Font(Font, FontStyle.Bold);
            style.BackColor = Escurecer(style.BackColor);


            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.Style = style;
            }
        }

        private Color Escurecer(Color @base)
        {
            return Color.FromArgb(@base.A, (int)(@base.R * 0.8), (int)(@base.G * 0.8), (int)(@base.B * 0.8));
        }
    }
}
