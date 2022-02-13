using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using a7D.PDV.BLL;

namespace a7D.PDV.Integracao.eMenu.UI
{
    public partial class frmPrincipal : Form
    {
        Pedido ObjPedido;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
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

                if (!ValidacaoSistema.VerificarConexaoDB())
                {
                    throw new ExceptionPDV( CodigoErro.E6F5);
                }
                #endregion

                ObjPedido = new Pedido();

                Boolean sincProdutos = Convert.ToBoolean(ConfigurationManager.AppSettings["SincProdutos"]);
                Boolean sincPedidos = Convert.ToBoolean(ConfigurationManager.AppSettings["SincPedidos"]);

                if (sincProdutos == true)
                {
                    SincProdutos();
                    btnForcarSinc.Enabled = true;
                }

                if (sincPedidos == true)
                    timer1.Enabled = true;
            }
            catch (Exception _e)
            {
                MessageBox.Show(_e.Message);
                Application.Exit();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SincPedidos();
            //lblStatus.Text = "Ativo";
        }

        private void SincPedidos()
        {
            //lblStatus.Text = "Sincronizando Pedido (eMenu > PDV7)"; lblStatus.Refresh();
            ObjPedido.ImportarPedido();
            //lblStatus.Text += " - Concluído"; lblStatus.Refresh();

            //lblStatus.Text = "Sincronizando Fechamento de Conta (PDV7 > eMenu)"; lblStatus.Refresh();
            ObjPedido.FinalizarPedidoEMENU();
            //lblStatus.Text += " - Concluído"; lblStatus.Refresh();

            //lblStatus.Text = "Sincronizando Solicitação de Conta (eMenu > PDV7)"; lblStatus.Refresh();
            ObjPedido.ImportarSolicitacaoServico();
            //lblStatus.Text += " - Concluído"; lblStatus.Refresh();
        }

        private void SincProdutos()
        {
            //lblStatus.Text = "Sincronizando Produtos (PDV7 > eMenu)"; lblStatus.Refresh();

            Produto objProduto = new Produto();
            objProduto.SincItens();
            objProduto.SincModificacoes();

            //lblStatus.Text += " - Concluído"; lblStatus.Refresh();
        }
        private void btnForcarSinc_Click(object sender, EventArgs e)
        {
            SincProdutos();
            //lblStatus.Text = "Ativo";
        }
    }
}
