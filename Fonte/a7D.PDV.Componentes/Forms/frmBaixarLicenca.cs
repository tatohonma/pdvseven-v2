using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public partial class frmBaixarLicenca : Form
    {
        public frmBaixarLicenca()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Enabled = false;
            Cursor = Cursors.WaitCursor;

            pBar.Visible = true;
            pBar.Value = 0;
            pBar.Maximum = 3;
            pBar.Step = 1;

            backgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ConfiguracaoBD.AlterarConfiguracaoSistema("chaveAtivacao", null);
            }
            else if(e.Result is Boolean sucesso)
            {
                if (sucesso)
                {
                    MessageBox.Show("Configurado com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.Yes;
                }
                else
                {
                    ConfiguracaoBD.AlterarConfiguracaoSistema("chaveAtivacao", null);
                    MessageBox.Show("Não foi possível se comunicar com o servidor.\nVerifique a conexão com a internet.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.No;
                }
            }

            Enabled = true;
            Cursor = Cursors.Default;
            pBar.Visible = false;
            Refresh();
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.PerformStep();
            Refresh();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var nLicenca = txtLicenca.Text;
            var chaveAnterior = new ConfiguracoesAtivacao().ChaveAtivacao;
            if (!string.IsNullOrEmpty(chaveAnterior) && chaveAnterior != nLicenca)
            {
                e.Result = new Exception("Já existe uma licença configurada.\nEntre em contato com o suporte.");
                return;
            }

            backgroundWorker1.ReportProgress(1);

            ConfiguracaoBD.AlterarConfiguracaoSistema("chaveAtivacao", nLicenca);
            backgroundWorker1.ReportProgress(1);
            bool sucesso = false;
            using (var pdvServico = new BLL.Entity.Licencas())
            {
                try
                {
                    pdvServico.Validar(TipoApp.SERVER);
                    sucesso = true;
                }
                catch (Exception ex)
                {
                    e.Result = ex;
                    return;
                }
            }
            backgroundWorker1.ReportProgress(1);
            e.Result = sucesso;
        }

        public static bool ConfiguraChaveAtivacao(Licencas pdvServico)
        {
            if (string.IsNullOrEmpty(pdvServico.ChaveAtivacao))
            {
                var frm = new frmBaixarLicenca();
                if (frm.ShowDialog() != DialogResult.Yes)
                    return true;
            }
            return false;
        }

        private void frmBaixarLicenca_Load(object sender, EventArgs e)
        {
            backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.WorkerReportsProgress = true;
        }
    }
}
