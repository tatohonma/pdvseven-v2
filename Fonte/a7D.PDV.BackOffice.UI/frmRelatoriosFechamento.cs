using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.BLL;
using System.Xml;
using System.IO;
using System.Configuration;
using a7D.PDV.Model;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmRelatoriosFechamento : Form
    {
        DataTable DtRelatorio;
        private RelatorioInformation RelatorioAtual { get; set; }

        public frmRelatoriosFechamento()
        {
            InitializeComponent();
        }

        private void frmRelatorios_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            List<FechamentoInformation> listaFechamento = Fechamento.Listar().OrderByDescending(l => l.DtFechamento).ToList();
            Dictionary<Int32, String> listaFechamento2 = new Dictionary<Int32, String>();
            CaixaInformation caixa;
            String fechamento;


            foreach (var item in listaFechamento)
            {
                caixa = Caixa.ListarPorFechamento(item.IDFechamento.Value).OrderBy(l => l.DtAbertura).FirstOrDefault();

                if (caixa != null)
                {
                    fechamento = caixa.DtAbertura.Value.ToString("dd/MM/yy HH:mm") + " até " + item.DtFechamento.Value.ToString("dd/MM/yy HH:mm");
                    listaFechamento2.Add(item.IDFechamento.Value, fechamento);

                    cbbFechamento.Items.Add(item);
                }
            }

            cbbRelatorio.DataSource = Relatorio.Listar().Where(l => l.TipoRelatorio.IDTipoRelatorio == 1).OrderBy(l => l.Ordem).ToList();

            if (listaFechamento.Count > 0)
                cbbFechamento.DataSource = new BindingSource(listaFechamento2, null);
        }

        private void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            dgvPrincipal.DataSource = null;
            dgvPrincipal.Rows.Clear();
            dgvPrincipal.Columns.Clear();

            if (cbbFechamento.Items.Count > 0 && String.IsNullOrEmpty(cbbFechamento.SelectedItem.ToString()) == false && String.IsNullOrEmpty(cbbRelatorio.SelectedItem.ToString()) == false)
            {
                var idFechamento = ((KeyValuePair<Int32, String>)cbbFechamento.SelectedItem).Key;
                var idRelatorio = ((RelatorioInformation)cbbRelatorio.SelectedItem).IDRelatorio.Value;

                RelatorioAtual = Relatorio.Carregar(idRelatorio);

                if (idRelatorio == 1)
                {
                    DtRelatorio = Relatorio.ResumoFechamento(idFechamento);
                }
                else
                {
                    DtRelatorio = Relatorio.CarregarRelatorioFechamento(idRelatorio, idFechamento);
                }

                var dtSumarizada = DtRelatorio.Totalizar(RelatorioAtual?.Totalizador);

                dgvPrincipal.DataSource = dtSumarizada;
                if (dtSumarizada.Rows.Count > DtRelatorio.Rows.Count)
                {
                    DtRelatorio = dtSumarizada;
                    EstilizarSumario();
                }
            }
            else
            {
                MessageBox.Show("Favor selecionar o tipo de relatório e o período de fechamento!");
            }
        }

        private void btnExportarCSV_Click(object sender, EventArgs e)
        {
            if (DtRelatorio != null)
            {
                Int32 idRelatorio = ((RelatorioInformation)cbbRelatorio.SelectedItem).IDRelatorio.Value;
                RelatorioInformation relatorio = Relatorio.Carregar(idRelatorio);

                String nomeArquivo = Path.GetDirectoryName(Application.ExecutablePath) + "\\Relatorios\\" + relatorio.Nome.ToLower().Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmssmmm") + ".csv";
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
