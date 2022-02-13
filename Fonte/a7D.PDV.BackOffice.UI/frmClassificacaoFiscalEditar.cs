using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmClassificacaoFiscalEditar : Form
    {
        public ClassificacaoFiscalInformation ClassificacaoFiscal1 { get; private set; }

        public frmClassificacaoFiscalEditar()
        {
            InitializeComponent();
            ClassificacaoFiscal1 = new ClassificacaoFiscalInformation();
        }

        public frmClassificacaoFiscalEditar(int idClassificacaoFiscal)
        {
            InitializeComponent();
            ClassificacaoFiscal1 = ClassificacaoFiscal.Carregar(idClassificacaoFiscal);
        }

        private void frmClassificacaoFiscalEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarLista();
            LimparCampos();
            PreencherCampos();
        }

        private void CarregarLista()
        {
            var listaTipoTributacao = TipoTributacao.Listar();
            listaTipoTributacao.Insert(0, new TipoTributacaoInformation());
            cbbTipoTributacao.DataSource = listaTipoTributacao;
            cbbTipoTributacao.ValueMember = "IDTipoTributacao";
            cbbTipoTributacao.DisplayMember = "Nome";
        }

        private void PreencherCampos()
        {
            if (ClassificacaoFiscal1.IDClassificacaoFiscal.HasValue)
            {
                cbbTipoTributacao.SelectedValue = ClassificacaoFiscal1.TipoTributacao.IDTipoTributacao.Value;
                txtNCM.Text = ClassificacaoFiscal1.NCM;
                txtCEST.Text = ClassificacaoFiscal1.CEST;
                txtNome.Text = ClassificacaoFiscal1.Nome;
                txtDescricao.Text = ClassificacaoFiscal1.Descricao;

                txtICMS.Text = ClassificacaoFiscal1.ICMS?.ToString("#,##0.00");
                txtIPI.Text = ClassificacaoFiscal1.IPI?.ToString("#,##0.00");
                txtPISPASEP.Text = ClassificacaoFiscal1.PISPASEP?.ToString("#,##0.00");
                txtCofins.Text = ClassificacaoFiscal1.COFINS?.ToString("#,##0.00");
                txtCIDE.Text = ClassificacaoFiscal1.CIDE?.ToString("#,##0.00");
                txtISS.Text = ClassificacaoFiscal1.ISS?.ToString("#,##0.00");
                txtIOF.Text = ClassificacaoFiscal1.IOF?.ToString("#,##0.00");
            }
        }

        private void LimparCampos()
        {
            cbbTipoTributacao.SelectedIndex = 0;
            txtNCM.Text = string.Empty;
            txtCEST.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtDescricao.Text = string.Empty;

            txtICMS.Text = string.Empty;
            txtIPI.Text = string.Empty;
            txtPISPASEP.Text = string.Empty;
            txtCofins.Text = string.Empty;
            txtCIDE.Text = string.Empty;
            txtISS.Text = string.Empty;
            txtIOF.Text = string.Empty;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validar())
                {
                    ClassificacaoFiscal1.TipoTributacao = TipoTributacao.Carregar(Convert.ToInt32(cbbTipoTributacao.SelectedValue));
                    ClassificacaoFiscal1.NCM = txtNCM.Text;
                    ClassificacaoFiscal1.CEST = txtCEST.Text;
                    ClassificacaoFiscal1.Nome = txtNome.Text;
                    ClassificacaoFiscal1.Descricao = txtDescricao.Text;

                    if (!string.IsNullOrWhiteSpace(txtICMS.Text))
                        ClassificacaoFiscal1.ICMS = Convert.ToDecimal(txtICMS.Text);
                    else
                        ClassificacaoFiscal1.ICMS = null;

                    if (!string.IsNullOrWhiteSpace(txtIPI.Text))
                        ClassificacaoFiscal1.IPI = Convert.ToDecimal(txtIPI.Text);
                    else
                        ClassificacaoFiscal1.IPI = null;

                    if (!string.IsNullOrWhiteSpace(txtPISPASEP.Text))
                        ClassificacaoFiscal1.PISPASEP = Convert.ToDecimal(txtPISPASEP.Text);
                    else
                        ClassificacaoFiscal1.PISPASEP = null;

                    if (!string.IsNullOrWhiteSpace(txtCofins.Text))
                        ClassificacaoFiscal1.COFINS = Convert.ToDecimal(txtCofins.Text);
                    else
                        ClassificacaoFiscal1.COFINS = null;

                    if (!string.IsNullOrWhiteSpace(txtCIDE.Text))
                        ClassificacaoFiscal1.CIDE = Convert.ToDecimal(txtCIDE.Text);
                    else
                        ClassificacaoFiscal1.CIDE = null;

                    if (!string.IsNullOrWhiteSpace(txtISS.Text))
                        ClassificacaoFiscal1.ISS = Convert.ToDecimal(txtISS.Text);
                    else
                        ClassificacaoFiscal1.ISS = null;

                    if (!string.IsNullOrWhiteSpace(txtIOF.Text))
                        ClassificacaoFiscal1.IOF = Convert.ToDecimal(txtIOF.Text);
                    else
                        ClassificacaoFiscal1.IOF = null;

                    if (ClassificacaoFiscal1.IDClassificacaoFiscal.HasValue)
                        ClassificacaoFiscal.Alterar(ClassificacaoFiscal1);
                    else
                        ClassificacaoFiscal.Adicionar(ClassificacaoFiscal1);

                    Close();
                }
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E013, ex);
            }
        }

        private bool Validar()
        {
            string erro = string.Empty;

            if (string.IsNullOrWhiteSpace(txtNCM.Text))
            {
                erro += "Campo NCN obrigatório!\n";
            }

            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                erro += "Campo Nome obrigatório!\n";
            }

            if (cbbTipoTributacao.SelectedIndex == 0)
            {
                erro += "Selecione um Tipo de Tributação!\n";
            }

            if (string.IsNullOrWhiteSpace(erro) == false)
            {
                MessageBox.Show(erro, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        private void ApenasNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) && e.KeyChar != ',')
                e.Handled = true;
        }
    }
}
