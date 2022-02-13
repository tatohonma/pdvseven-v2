using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmFichaInventario : Form
    {

        private DataTable DtRelatorio;
        public frmFichaInventario()
        {
            InitializeComponent();
        }

        private void frmFichaInventario_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            reportViewer1.RefreshReport();
            pictureBox1.Visible = false;
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //var produtos = Produto.Listar()
            //            .Where(p => p.Excluido == false)
            //            .Where(p => p.Ativo == true)
            //            .Where(p => p.ControlarEstoque == true)
            var produtos = Produto.Listar(new Model.ProdutoInformation()
            {
                Excluido = false,
                Ativo = true,
                ControlarEstoque = true
            })
                .Where(p => ProdutoReceita.ListarPorProduto(p).Count() == 0)
                .OrderBy(p => p.Nome)
                .ToList();

            DtRelatorio = new DataTable("Relatorio");

            DtRelatorio.Columns.AddRange(new DataColumn[]
            {
                    new DataColumn("Produto", typeof(string)),
                    new DataColumn("Unidade", typeof(string)),
                    new DataColumn("Quantidade", typeof(decimal))
            });

            foreach (var produto in produtos.OrderBy(p => p.Nome))
            {
                var newRow = DtRelatorio.NewRow();
                newRow.SetField("Produto", produto.Nome);
                newRow.SetField("Unidade", Unidade.Carregar(produto.Unidade.IDUnidade.Value).Nome);
                DtRelatorio.Rows.Add(newRow);

                foreach (var unidade in ConversaoUnidade.ListarUnidadesConversaoPara(produto.Unidade))
                {
                    newRow = DtRelatorio.NewRow();
                    newRow.SetField("Produto", produto.Nome);
                    newRow.SetField("Unidade", unidade.Nome);
                    DtRelatorio.Rows.Add(newRow);
                }

            }

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("FichaInventario", DtRelatorio));
            //reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("DataRelatorio", data.ToString("dd/MM/yyyy")));
            //reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("DataImpressao", agora.ToString("dd/MM/yyyy HH:mm:ss")));
            reportViewer1.LocalReport.DisplayName = "FichaInventario_" + DateTime.Now.ToString("yyyyMMdd");
            var setup = reportViewer1.GetPageSettings();
            setup.Margins = new System.Drawing.Printing.Margins(1, 1, 1, 1);
            setup.Landscape = false;
            reportViewer1.SetPageSettings(setup);
        }

        private void frmFichaInventario_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DtRelatorio != null)
                DtRelatorio.Dispose();
        }
    }
}
