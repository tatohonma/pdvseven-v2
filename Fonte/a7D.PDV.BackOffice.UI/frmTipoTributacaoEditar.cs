using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.Model;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmTipoTributacaoEditar : Form
    {
        public TipoTributacaoInformation TipoTributacao1 { get; private set; }

        public frmTipoTributacaoEditar()
        {
            InitializeComponent();
            TipoTributacao1 = new TipoTributacaoInformation();
        }

        public frmTipoTributacaoEditar(int idTipoTributacao)
        {
            InitializeComponent();
            TipoTributacao1 = TipoTributacao.Carregar(idTipoTributacao);
        }

        private void frmTipoTributacaoEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            PreencherCampos();
        }

        private void PreencherCampos()
        {
            if (TipoTributacao1.IDTipoTributacao.HasValue == false)
            {
                txtCFOP.Text = txtvItem12741.Text = txtNome.Text = txtDescricao.Text = string.Empty;

                txtICMS00_Orig.Text = txtICMS00_CST.Text = txtICMS00_pICMS.Text = string.Empty;
                txtICMS40_Orig.Text = txtICMS40_CST.Text = string.Empty;
                txtICMSSN102_Orig.Text = txtICMSSN102_CSOSN.Text = string.Empty;
                txtICMSSN900_CSOSN.Text = txtICMSSN900_Orig.Text = txtICMSSN900_pICMS.Text = string.Empty;

                txtPISAliq_CST.Text = txtPISAliq_pPIS.Text = string.Empty;
                txtPISQtde_CST.Text = txtPISQtde_vAliqProd.Text = string.Empty;
                txtPISNT_CST.Text = txtPISSN_CST.Text = string.Empty;
                txtPISOutr_CST.Text = txtPISOutr_pPIS.Text = string.Empty;
                txtPISOutr_vAliqProd.Text = txtPISST_pPIS.Text = string.Empty;
                txtPISST_vAliqProd.Text = string.Empty;

                txtCOFINSAliq_CST.Text = txtCOFINSAliq_pCOFINS.Text = string.Empty;
                txtCOFINSNT_CST.Text = txtCOFINSOutr_CST.Text = string.Empty;
                txtCOFINSOutr_pCOFINS.Text = txtCOFINSOutr_vAliqProd.Text = string.Empty;
                txtCOFINSQtde_CST.Text = txtCOFINSQtde_vAliqProd.Text = string.Empty;
                txtCOFINSOutr_pCOFINS.Text = txtCOFINSOutr_vAliqProd.Text = string.Empty;
                txtCOFINSQtde_CST.Text = string.Empty;

                txtISSQN_cListServ.Text = txtISSQN_cNatOp.Text = string.Empty;
                txtISSQN_cServTribMun.Text = txtISSQN_indIncFisc.Text = string.Empty;
                txtISSQN_vAliq.Text = txtISSQN_vDeducISSQN.Text = string.Empty;

                return;
            }

            #region DadosPrincipais

            txtCFOP.Text = TipoTributacao1.CFOP;
            txtvItem12741.Text = TipoTributacao1.vItem12741;
            txtNome.Text = TipoTributacao1.Nome;
            txtDescricao.Text = TipoTributacao1.Descricao;

            #endregion

            #region ICMS

            txtICMS00_Orig.Text = TipoTributacao1.ICMS00_Orig;
            txtICMS00_CST.Text = TipoTributacao1.ICMS00_CST;
            txtICMS00_pICMS.Text = TipoTributacao1.ICMS00_pICMS;
            txtICMS40_Orig.Text = TipoTributacao1.ICMS40_Orig;
            txtICMS40_CST.Text = TipoTributacao1.ICMS40_CST;
            txtICMSSN102_Orig.Text = TipoTributacao1.ICMSSN102_Orig;
            txtICMSSN102_CSOSN.Text = TipoTributacao1.ICMSSN102_CSOSN;
            txtICMSSN900_CSOSN.Text = TipoTributacao1.ICMSSN900_CSOSN;
            txtICMSSN900_Orig.Text = TipoTributacao1.ICMSSN900_Orig;
            txtICMSSN900_pICMS.Text = TipoTributacao1.ICMSSN900_pICMS;

            #endregion

            #region PIS

            txtPISAliq_CST.Text = TipoTributacao1.PISAliq_CST;
            txtPISAliq_pPIS.Text = TipoTributacao1.PISAliq_pPIS;
            txtPISQtde_CST.Text = TipoTributacao1.PISQtde_CST;
            txtPISQtde_vAliqProd.Text = TipoTributacao1.PISQtde_vAliqProd;
            txtPISNT_CST.Text = TipoTributacao1.PISNT_CST;
            txtPISSN_CST.Text = TipoTributacao1.PISSN_CST;
            txtPISOutr_CST.Text = TipoTributacao1.PISOutr_CST;
            txtPISOutr_pPIS.Text = TipoTributacao1.PISOutr_pPIS;
            txtPISOutr_vAliqProd.Text = TipoTributacao1.PISOutr_vAliqProd;
            txtPISST_pPIS.Text = TipoTributacao1.PISST_pPIS;
            txtPISST_vAliqProd.Text = TipoTributacao1.PISST_vAliqProd;

            #endregion

            #region COFINS

            txtCOFINSAliq_CST.Text = TipoTributacao1.COFINSAliq_CST;
            txtCOFINSAliq_pCOFINS.Text = TipoTributacao1.COFINSAliq_pCOFINS;
            txtCOFINSNT_CST.Text = TipoTributacao1.COFINSNT_CST;
            txtCOFINSOutr_CST.Text = TipoTributacao1.COFINSOutr_CST;
            txtCOFINSOutr_pCOFINS.Text = TipoTributacao1.COFINSOutr_pCOFINS;
            txtCOFINSOutr_vAliqProd.Text = TipoTributacao1.COFINSOutr_vAliqProd;
            txtCOFINSQtde_CST.Text = TipoTributacao1.COFINSQtde_CST;
            txtCOFINSQtde_vAliqProd.Text = TipoTributacao1.COFINSQtde_vAliqProd;
            txtCOFINSSN_CST.Text = TipoTributacao1.COFINSSN_CST;
            txtCOFINSST_pCOFINS.Text = TipoTributacao1.COFINSST_pCOFINS;
            txtCOFINSST_vAliqProd.Text = TipoTributacao1.COFINSST_vAliqProd;

            #endregion

            #region ISS

            txtISSQN_cListServ.Text = TipoTributacao1.ISSQN_cListServ;
            txtISSQN_cNatOp.Text = TipoTributacao1.ISSQN_cNatOp;
            txtISSQN_cServTribMun.Text = TipoTributacao1.ISSQN_cServTribMun;
            txtISSQN_indIncFisc.Text = TipoTributacao1.ISSQN_indIncFisc;
            txtISSQN_vAliq.Text = TipoTributacao1.ISSQN_vAliq;
            txtISSQN_vDeducISSQN.Text = TipoTributacao1.ISSQN_vDeducISSQN;

            #endregion
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var erro = string.Empty;

                if (string.IsNullOrWhiteSpace(txtNome.Text))
                    erro += "Nome não pode ser vazio!\n";

                if (string.IsNullOrWhiteSpace(txtCFOP.Text))
                    erro += "CFOP não pode ser vazio!\n";

                if (string.IsNullOrWhiteSpace(erro) == false)
                {
                    MessageBox.Show(erro, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #region Dados Principais

                TipoTributacao1.CFOP = txtCFOP.Text;
                TipoTributacao1.vItem12741 = txtvItem12741.Text;
                TipoTributacao1.Nome = txtNome.Text;
                TipoTributacao1.Descricao = txtDescricao.Text;

                #endregion

                #region ICMS

                TipoTributacao1.ICMS00_Orig = txtICMS00_Orig.Text;
                TipoTributacao1.ICMS00_CST = txtICMS00_CST.Text;
                TipoTributacao1.ICMS00_pICMS = txtICMS00_pICMS.Text;
                TipoTributacao1.ICMS40_Orig = txtICMS40_Orig.Text;
                TipoTributacao1.ICMS40_CST = txtICMS40_CST.Text;
                TipoTributacao1.ICMSSN102_Orig = txtICMSSN102_Orig.Text;
                TipoTributacao1.ICMSSN102_CSOSN = txtICMSSN102_CSOSN.Text;
                TipoTributacao1.ICMSSN900_CSOSN = txtICMSSN900_CSOSN.Text;
                TipoTributacao1.ICMSSN900_Orig = txtICMSSN900_Orig.Text;
                TipoTributacao1.ICMSSN900_pICMS = txtICMSSN900_pICMS.Text;

                #endregion

                #region PIS

                TipoTributacao1.PISAliq_CST = txtPISAliq_CST.Text;
                TipoTributacao1.PISAliq_pPIS = txtPISAliq_pPIS.Text;
                TipoTributacao1.PISQtde_CST = txtPISQtde_CST.Text;
                TipoTributacao1.PISQtde_vAliqProd = txtPISQtde_vAliqProd.Text;
                TipoTributacao1.PISNT_CST = txtPISNT_CST.Text;
                TipoTributacao1.PISSN_CST = txtPISSN_CST.Text;
                TipoTributacao1.PISOutr_CST = txtPISOutr_CST.Text;
                TipoTributacao1.PISOutr_pPIS = txtPISOutr_pPIS.Text;
                TipoTributacao1.PISOutr_vAliqProd = txtPISOutr_vAliqProd.Text;
                TipoTributacao1.PISST_pPIS = txtPISST_pPIS.Text;
                TipoTributacao1.PISST_vAliqProd = txtPISST_vAliqProd.Text;

                #endregion

                #region COFINS

                TipoTributacao1.COFINSAliq_CST = txtCOFINSAliq_CST.Text;
                TipoTributacao1.COFINSAliq_pCOFINS = txtCOFINSAliq_pCOFINS.Text;
                TipoTributacao1.COFINSNT_CST = txtCOFINSNT_CST.Text;
                TipoTributacao1.COFINSOutr_CST = txtCOFINSOutr_CST.Text;
                TipoTributacao1.COFINSOutr_pCOFINS = txtCOFINSOutr_pCOFINS.Text;
                TipoTributacao1.COFINSOutr_vAliqProd = txtCOFINSOutr_vAliqProd.Text;
                TipoTributacao1.COFINSQtde_CST = txtCOFINSQtde_CST.Text;
                TipoTributacao1.COFINSQtde_vAliqProd = txtCOFINSQtde_vAliqProd.Text;
                TipoTributacao1.COFINSSN_CST = txtCOFINSSN_CST.Text;
                TipoTributacao1.COFINSST_pCOFINS = txtCOFINSST_pCOFINS.Text;
                TipoTributacao1.COFINSST_vAliqProd = txtCOFINSST_vAliqProd.Text;

                #endregion

                #region ISS

                TipoTributacao1.ISSQN_cListServ = txtISSQN_cListServ.Text;
                TipoTributacao1.ISSQN_cNatOp = txtISSQN_cNatOp.Text;
                TipoTributacao1.ISSQN_cServTribMun = txtISSQN_cServTribMun.Text;
                TipoTributacao1.ISSQN_indIncFisc = txtISSQN_indIncFisc.Text;
                TipoTributacao1.ISSQN_vAliq = txtISSQN_vAliq.Text;
                TipoTributacao1.ISSQN_vDeducISSQN = txtISSQN_vDeducISSQN.Text;

                #endregion

                if (TipoTributacao1.IDTipoTributacao.HasValue)
                    TipoTributacao.Alterar(TipoTributacao1);
                else
                    TipoTributacao.Adicionar(TipoTributacao1);

                Close();
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E013, ex);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
