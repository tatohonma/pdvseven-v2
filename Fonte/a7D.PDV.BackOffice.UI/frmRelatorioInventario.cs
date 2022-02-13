using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmRelatorioInventario : Form
    {
        private int idInventario;


        public frmRelatorioInventario(int idInventario)
        {
            this.idInventario = idInventario;
            InitializeComponent();
        }

        private void frmRelatorioInventario_Load(object sender, EventArgs e)
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
            var agora = DateTime.Now;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("Inventario", EntradaSaida.RelatorioInventario(idInventario)));
            //reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("DataRelatorio", data.ToString("dd/MM/yyyy")));
            //reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("DataImpressao", agora.ToString("dd/MM/yyyy HH:mm:ss")));
            reportViewer1.LocalReport.DisplayName = "RelatorioInventario_" + idInventario + "_" + agora.ToString("yyyyMMddHHmmss");
            var setup = reportViewer1.GetPageSettings();
            setup.Margins = new System.Drawing.Printing.Margins(1, 1, 1, 1);
            setup.Landscape = true;
            reportViewer1.SetPageSettings(setup);
        }
    }
}
