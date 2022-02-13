using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.LimparLicencas.UI
{
    public partial class frmLimparLicencas : Form
    {
        public frmLimparLicencas()
        {
            InitializeComponent();
        }

        private void frmLimparLicencas_Load(object sender, EventArgs e)
        {
            var cn = ConfigurationManager.ConnectionStrings["connectionString"];

            if (cn == null || cn.ConnectionString == "")
            {
                var formConfigurarConexaoDB = new Configurador.UI.frmConfigurarConexaoDB();
                formConfigurarConexaoDB.ShowDialog();
                Application.Exit();
                return;
            }

            if (!ValidacaoSistema.VerificarConexaoDB())
            {
                throw new Exception("Não foi possível conectar no Servidor!");
            }

            var csb = new SqlConnectionStringBuilder(cn.ConnectionString);
            linkLabel1.Text = string.Format(@"Banco de dados: {0}\{1}", csb.DataSource, csb.InitialCatalog);
        }

        private void button1_Click(object sender, EventArgs e)
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
                    }
                    pdv7Context.DB.SaveChanges();
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Configurador.UI.frmConfigurarConexaoDB formConfigurarConexaoDB = new Configurador.UI.frmConfigurarConexaoDB();
            formConfigurarConexaoDB.ShowDialog();

            Application.Exit();
            return;
        }
    }
}
