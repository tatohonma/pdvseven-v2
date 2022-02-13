using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmRelatoriosGerais : Form
    {
        DataTable DtRelatorio;
        private RelatorioInformation RelatorioAtual { get; set; }
        private readonly IFormatProvider _provider = new CultureInfo("pt-BR");
        public RelatorioFiltro RelatorioSelecionado { get; set; }

        private int? IDRelatorio { get; set; }

        public frmRelatoriosGerais()
        {
            InitializeComponent();
        }

        private void frmRelatoriosGerais_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            var ds = Relatorio.Listar()
                .OrderBy(l => l.TipoRelatorio.IDTipoRelatorio)
                .ThenBy(l => l.Ordem)
                .Select(l =>
                        new RelatorioFiltro
                        {
                            IDRelatorio = l.IDRelatorio.Value,
                            IDTipoRelatorio = l.TipoRelatorio.IDTipoRelatorio.Value,
                            Nome = l.Nome,
                        }
                ).ToList();

            cbbRelatorio.DataSource = ds;

            var listaFechamento = Fechamento.Listar().OrderByDescending(l => l.DtFechamento).ToList();
            var listaFechamento2 = new Dictionary<Int32, String>();


            foreach (var item in listaFechamento)
            {
                var listaCaixa = Caixa.ListarPorFechamento(item.IDFechamento.Value);
                var primeiroCaixa = listaCaixa.Where(l => l.DtAbertura != null).OrderBy(l => l.DtAbertura).FirstOrDefault();
                var ultimoCaixa = listaCaixa.Where(l => l.DtFechamento != null).OrderByDescending(l => l.DtFechamento).FirstOrDefault();

                if (primeiroCaixa != null)
                {
                    var de = primeiroCaixa.DtAbertura.Value.ToString("dd/MM HH:mm", _provider);
                    var ate = string.Empty;
                    if (ultimoCaixa != null)
                    {
                        ate = $"até {ultimoCaixa.DtFechamento.Value.ToString("dd/MM HH:mm", _provider)}";
                    }
                    var dtAbertura = primeiroCaixa.DtAbertura.Value;
                    var fechamento = $"Fechamento {dtAbertura.ToString("dd/MM/yyyy", _provider)} - {TitleCase(dtAbertura.ToString("dddd", _provider))} ({de} {ate})";
                    listaFechamento2.Add(item.IDFechamento.Value, fechamento);

                    cbbFechamento.Items.Add(item);
                }
            }

            if (listaFechamento.Count > 0)
                cbbFechamento.DataSource = new BindingSource(listaFechamento2, null);

            dtpInicio.Value = DateTime.Now.AddDays(-30).Date;
            dtpFim.Value = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
        }

        private string TitleCase(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;
            if (s.Length == 1)
                return s.ToUpper();
            return $"{s.Substring(0, 1).ToUpper()}{s.Substring(1)}";
        }

        private void btnExportarCSV_Click(object sender, EventArgs e)
        {
            if (DtRelatorio != null && IDRelatorio.HasValue)
            {
                RelatorioInformation relatorio = Relatorio.Carregar(IDRelatorio.Value);

                var nomeArquivo = BackofficeUtil.NomeRelatorio(relatorio.Nome.ToLower().Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmssmmm") + ".csv");
                Relatorio.ExportarParaTxt(DtRelatorio, nomeArquivo);

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
            style.Font = new Font(dgvPrincipal.DefaultCellStyle.Font, FontStyle.Bold);
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

        public class RelatorioFiltro
        {
            public int IDRelatorio { get; set; }
            private string _nome;
            public string Nome
            {
                get { return $"{(IDTipoRelatorio == 1 ? "(F) " : (IDTipoRelatorio == 2 ? "(D) " : "(G) "))} {_nome}"; }
                set { _nome = value; }
            }
            public int IDTipoRelatorio { get; set; }

        }

        internal void GerarRelatorio(RelatorioFiltro relatorio, int? idFechamento, DateTime inicio, DateTime fim)
        {
            btnExportarCSV.Enabled = false;
            dgvPrincipal.DataSource = null;
            dgvPrincipal.Rows.Clear();
            dgvPrincipal.Columns.Clear();
            var idRelatorio = relatorio.IDRelatorio;
            IDRelatorio = idRelatorio;

            RelatorioAtual = Relatorio.Carregar(idRelatorio);
            if (idRelatorio == 1 || string.IsNullOrEmpty(RelatorioAtual.QuerySQL))
            {
                DtRelatorio = Relatorio.ResumoFechamento(idFechamento.Value);
            }
            else
            {
                switch (relatorio.IDTipoRelatorio)
                {
                    case 1:
                        DtRelatorio = Relatorio.CarregarRelatorioFechamento(idRelatorio, idFechamento.Value);
                        break;
                    case 2:
                        DtRelatorio = Relatorio.CarregarRelatorioPorData(idRelatorio, inicio, fim);
                        break;
                    default:
                        DtRelatorio = Relatorio.CarregarRelatorio(idRelatorio);
                        break;
                }

            }

            var dtSumarizada = DtRelatorio.Totalizar(RelatorioAtual?.Totalizador);
            dgvPrincipal.DataSource = dtSumarizada;
            var sumario = dtSumarizada.Rows.Count > DtRelatorio.Rows.Count;

            if (sumario)
            {
                DtRelatorio = dtSumarizada;
                EstilizarSumario();
            }
            btnExportarCSV.Enabled = true;
        }

        private void cbbRelatorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            var relatorioSelecionado = cbbRelatorio.SelectedItem as RelatorioFiltro;
            dtpInicio.Visible = false;
            dtpFim.Visible = false;
            cbbFechamento.Visible = false;
            btnGerarRelatorio.Enabled = false;
            if (relatorioSelecionado != null)
            {
                if (relatorioSelecionado.IDTipoRelatorio == 1)
                {
                    cbbFechamento.Visible = true;
                }
                else if (relatorioSelecionado.IDTipoRelatorio == 2)
                {
                    dtpInicio.Visible = dtpFim.Visible = true;
                }
                btnGerarRelatorio.Enabled = true;
            }
        }

        private void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbbRelatorio.SelectedIndex > -1)
                {
                    var relatorioSelecionado = cbbRelatorio.SelectedItem as RelatorioFiltro;
                    var idFechamento = default(int?);
                    if (relatorioSelecionado.IDTipoRelatorio == 1)
                    {
                        idFechamento = ((KeyValuePair<Int32, String>)cbbFechamento.SelectedItem).Key;
                    }
                    GerarRelatorio(relatorioSelecionado, idFechamento, dtpInicio.Value, dtpFim.Value);
                }
            }
            catch(Exception ex)
            {
                Logs.ErroBox(CodigoErro.E121, ex);
            }
        }
    }
}
