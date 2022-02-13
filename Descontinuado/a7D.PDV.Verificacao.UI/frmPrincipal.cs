using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Verificacao.UI
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                #region Verificar DB
                if (ConfigurationManager.ConnectionStrings["connectionString"] == null || ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString == "")
                {
                    Configurador.UI.frmConfigurarConexaoDB formConfigurarConexaoDB = new Configurador.UI.frmConfigurarConexaoDB();
                    formConfigurarConexaoDB.ShowDialog();

                    Application.Exit();
                }

                if (!BLL.ValidacaoSistema.VerificarConexaoDB())
                {
                    throw new Exception("Não foi possível conectar no Servidor!");
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            PopularVerificacoes();
        }

        private void PopularVerificacoes()
        {
            flowLayout.Controls.Clear();

            var grupo = VerificacaoProvider.Verificacoes().GroupBy(v => v.Categoria);

            foreach (var g in grupo.OrderBy(g => g.ToString()))
            {
                var l = new Label();
                l.Text = g.Key.ToString();
                l.Font = new Font(Font.FontFamily, Font.Size + 4, FontStyle.Bold);
                flowLayout.Controls.Add(l);
                l.AutoSize = true;
                foreach (var verificacao in g.OrderByDescending(v => v.Invalido).ThenBy(v => v.Nivel))
                {
                    var uc = new UcVerificacao(verificacao);
                    flowLayout.Controls.Add(uc);
                    uc.Width = flowLayout.DisplayRectangle.Width - flowLayout.Margin.Horizontal;
                }
            }
        }

        private void flowLayout_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void verificarNovamenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopularVerificacoes();
        }
    }
}
