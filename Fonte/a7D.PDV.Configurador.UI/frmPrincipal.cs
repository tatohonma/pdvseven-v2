using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.Componentes;
using a7D.PDV.LocalizadorBalanca.UI;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }
        private void FecharJanelas()
        {
            foreach (var item in MdiChildren)
            {
                if (item != null)
                    item.Close();
            }
        }

        private void btnConfigurarLicenca_Click(object sender, EventArgs e)
        {
            var form = new frmBaixarLicenca();
            form.ShowDialog();
        }

        private void btnConfigurarConexaoDB_Click(object sender, EventArgs e)
        {
            frmConfigurarConexaoDB form = new frmConfigurarConexaoDB();
            form.ShowDialog();
        }

        private void btnConfigurarSAT_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            frmConfiguracoesFiscal form = new frmConfiguracoesFiscal();
            form.MdiParent = this;
            form.Dock = DockStyle.Fill;
            form.MdiParent.Refresh();
            form.Show();
        }

        private void btnConfigurarFechamento_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            var form = new frmConfigurarFechamento();
            form.MdiParent = this;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void btnConfiguracoes_Click(object sender, EventArgs e)
        {
            FecharJanelas();

            var pdvs = BLL.PDV.Listar();
            var r = MessageBox.Show("Alterar as configurações do sistema pode prejudicar seu funcionamento ou faze-lo parar completamente.\nDeseja realmente continuar?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                var form = new frmConfiguracoes(pdvs);
                form.MdiParent = this;
                form.Dock = DockStyle.Fill;
                form.Show();
            }
        }

        private void btnLocalizarBalanca_Click(object sender, EventArgs e)
        {
            FecharJanelas();
            new frmLocalizarBalanca().ShowDialog();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Enabled = false;
                Refresh();
                using (var pdvServico = new Licencas())
                {
                    foreach (var pdv in BLL.PDV.Listar())
                    {
                        pdv.ChaveHardware = null;
                        pdv.UltimaAlteracao = null;
                        pdv.UltimoAcesso = null;
                        pdv.AtualizarDados();
                        EF.Repositorio.Atualizar(pdv);
                    }
                    //pdv7Context.DB.SaveChanges();
                }
                MessageBox.Show("Atualizado com Sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                Enabled = true;
                Refresh();
            }
        }
    }
}
