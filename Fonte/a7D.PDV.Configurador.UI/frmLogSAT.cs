using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmLogSAT : Form
    {
        public string Log { get; private set; }

        public frmLogSAT()
        {
            InitializeComponent();
        }

        public frmLogSAT(string log)
        {
            InitializeComponent();
            Log = log;
        }

        private void frmLogSAT_Load(object sender, EventArgs e)
        {
            if (Log != null)
            {
                var listaLog = Log.Split('\n');
                var split = listaLog.Select(l => l.Split('|')).ToList();
                split = split.Where(s => s.Length == 4).ToList();
                var linhasOrdenadas = (from l in split
                                       select new InformacoesLog(l)
                             ).OrderByDescending(l => l.Data);

                var linhas = from l in linhasOrdenadas
                             select new
                             {
                                 Data = l.Data.ToString("dd/MM/yyyy HH:mm:ss"),
                                 l.Fonte,
                                 l.Mensagem,
                                 l.Tipo
                             };

                dgvPrincipal.DataSource = linhas.ToArray();
            }
            else
            {
                dgvPrincipal.Visible = false;
                btnLogsSemData.Visible = false;
            }
        }

        private void btnLogsSemData_Click(object sender, EventArgs e)
        {
            new frmLogSATSemData(Log).ShowDialog();
        }

        private void dgvPrincipal_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvPrincipal.Columns["Tipo"].Index)
            {
                var cellTipo = dgvPrincipal.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cellTipo.Value != null && !string.IsNullOrWhiteSpace(cellTipo.Value.ToString()))
                {
                    if (cellTipo.Value.ToString().ToLowerInvariant().Contains("erro"))
                    {
                        cellTipo.Style.ForeColor = Color.Red;
                    }
                }
            }
        }
    }
    class InformacoesLog
    {
        private readonly IFormatProvider _cultureInfo = new CultureInfo("pt-BR");

        public InformacoesLog(string[] dados)
        {
            Data = DateTime.ParseExact(dados[0], "yyyyMMddHHmmss", _cultureInfo);
            Fonte = dados[1];
            Tipo = dados[2];
            Mensagem = dados[3];
        }

        public DateTime Data { get; set; }

        public string Fonte { get; set; }

        public string Tipo { get; set; }

        public string Mensagem { get; set; }
    }
}
