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
    public partial class frmRelatorioMovDetalhado : Form
    {
        private DateTime data;
        private int idProduto;

        public frmRelatorioMovDetalhado(DateTime data, int idProduto)
        {
            this.data = data;
            this.idProduto = idProduto;
            InitializeComponent();
        }

        private void frmRelatoriosTeste_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            var agora = DateTime.Now;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DetalhesMovimentacaoDiaria", EntradaSaida.HistoricoPorDia(data, idProduto)));
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("DataRelatorio", data.ToString("dd/MM/yyyy")));
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("DataImpressao", agora.ToString("dd/MM/yyyy HH:mm:ss")));
            reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("Produto", Produto.Carregar(idProduto).Nome));
            reportViewer1.LocalReport.DisplayName = "DetalhesMovimentacaoDiaria" + data.ToString("yyyyMMdd") + "_" + agora.ToString("yyyyMMddHHmmss");
            var setup = reportViewer1.GetPageSettings();
            setup.Margins = new System.Drawing.Printing.Margins(1, 1, 1, 1);
            setup.Landscape = true;
            reportViewer1.SetPageSettings(setup);
            this.reportViewer1.RefreshReport();
        }
    }
}
