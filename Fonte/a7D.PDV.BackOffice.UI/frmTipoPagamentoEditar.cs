using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using a7D.PDV.EF.Models;
using a7D.PDV.EF;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmTipoPagamentoEditar : Form
    {
        private TipoPagamentoInformation TipoPagamento1;
        private bool possuiSat;

        public frmTipoPagamentoEditar()
        {
            InitializeComponent();
            TipoPagamento1 = new TipoPagamentoInformation();
        }

        public frmTipoPagamentoEditar(Int32 idTipoPagamento) : this()
        {
            TipoPagamento1 = TipoPagamento.Carregar(idTipoPagamento);
        }

        private void frmTipoPagamentoEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            possuiSat = ConfiguracoesSistema.Valores.Fiscal == "SAT";

            txtNome.Text = TipoPagamento1.Nome;
            ckbAtivo.Checked = TipoPagamento1.Ativo == true;
            ckbRegistrarValores.Checked = TipoPagamento1.RegistrarValores == true;
            cbbGateway.Fill<tbGateway>(TipoPagamento1.IDGateway, true, g => g.IDGateway < 100); // Somente gateways de pagamento
            cbbRecebivel.Fill<tbContaRecebivel>(TipoPagamento1?.ContaRecebivel?.IDContaRecebivel);
            cbbBandeira.Fill<tbBandeira>(TipoPagamento1?.Bandeira?.IDBandeira);
            txtCodigo.Text = TipoPagamento1.CodigoImpressoraFiscal;

            lblImpressoraFiscal.Visible = txtCodigo.Visible = !possuiSat;

            var mp = new MeioPagamentoSATInformation();
            cbbMeioPagamento.ValueMember = nameof(mp.IDMeioPagamentoSAT);
            cbbMeioPagamento.DisplayMember = nameof(mp.Descricao);

            var lista = MeioPagamentoSAT.Listar();
            cbbMeioPagamento.DataSource = lista;

            if (TipoPagamento1.MeioPagamentoSAT?.IDMeioPagamentoSAT != null)
            {
                cbbMeioPagamento.SelectedIndex = lista.IndexOf(lista.First(a => a.IDMeioPagamentoSAT == TipoPagamento1.MeioPagamentoSAT.IDMeioPagamentoSAT));
            }
            else
            {
                cbbMeioPagamento.SelectedIndex = -1;
            }

            ExibeCampos();
            var qtd = BLL.Caixa.ContarAbertos();
            string mensagem = "Não efetuar alterações enquanto o caixa estiver aberto!";
            if (qtd > 0)
            {
                string s = qtd > 1 ? "s" : "";
                mensagem = $"ATENÇÃO: Existe {qtd} caixa{s} aberto{s}\n\r\n\r";

            }
            MessageBox.Show(mensagem, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;

            TipoPagamento1.Nome = txtNome.Text;
            TipoPagamento1.CodigoImpressoraFiscal = txtCodigo.Text;
            TipoPagamento1.Ativo = ckbAtivo.Checked;
            TipoPagamento1.IDGateway = (int)cbbGateway.SelectedValue;

            if (cbbMeioPagamento.Visible)
                TipoPagamento1.MeioPagamentoSAT = new MeioPagamentoSATInformation() { IDMeioPagamentoSAT = Convert.ToInt32(cbbMeioPagamento.SelectedValue) };
            else
                TipoPagamento1.MeioPagamentoSAT = null;

            if (TipoPagamento1.Gateway == EGateway.NTKPOS
             || TipoPagamento1.Gateway == EGateway.GranitoPOS
             || TipoPagamento1.Gateway == EGateway.StonePOS)
                TipoPagamento1.Ativo = TipoPagamento1.RegistrarValores = false;

            else if (TipoPagamento1.IDGateway > 0)
            {
                TipoPagamento1.RegistrarValores = false;
                if (TipoPagamento1.Gateway == EGateway.ContaCliente)
                {
                    TipoPagamento1.ContaRecebivel = new tbContaRecebivel() { IDContaRecebivel = (int)EContaRecebivel.ContaCliente };
                    TipoPagamento1.MeioPagamentoSAT = new MeioPagamentoSATInformation() { IDMeioPagamentoSAT = (int)ECodigosPagamentoSAT.Loja };
                }
                else
                {
                    TipoPagamento1.MeioPagamentoSAT = null;
                    TipoPagamento1.ContaRecebivel = null;
                    TipoPagamento1.Bandeira = null;
                }
            }
            else
            {
                TipoPagamento1.RegistrarValores = ckbRegistrarValores.Checked;
                TipoPagamento1.ContaRecebivel = new tbContaRecebivel() { IDContaRecebivel = Convert.ToInt32(cbbRecebivel.SelectedValue) };
                TipoPagamento1.Bandeira = new tbBandeira() { IDBandeira = Convert.ToInt32(cbbBandeira.SelectedValue) };
            }

            TipoPagamento.Salvar(TipoPagamento1);

            this.Close();
        }

        private Boolean Validar()
        {
            String msg = "";

            if (txtNome.Text == "")
                msg += "Campo \"Nome\" é obrigatório. \n";

            if (cbbMeioPagamento.Visible && cbbMeioPagamento.SelectedIndex == -1)
                msg += "Selecione o Meio de Pagamento. \n";

            if (cbbRecebivel.Visible && cbbRecebivel.SelectedIndex == -1)
                msg += "Selecione uma Conta de Recebível. \n";

            if (cbbBandeira.Visible && cbbBandeira.SelectedIndex == -1)
                msg += "Selecione uma Bandeira. \n";

            if (!possuiSat && txtCodigo.Text == "")
                msg += "Campo \"Código\" é obrigatório. \n";

            if (msg.Length > 0)
            {
                MessageBox.Show(msg, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbbGateway_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExibeCampos();
        }

        private void ExibeCampos()
        {
            lblBandeira.Visible = cbbBandeira.Visible = lblRecebivel.Visible = cbbRecebivel.Visible = ckbRegistrarValores.Visible = lblMeioPagamento.Visible = cbbMeioPagamento.Visible = cbbGateway.SelectedIndex == 0;
            // Estas formas de pagamento devem ser a mesma em Tipos de PDV com fechamento automático: AutoFechamento()
            ckbAtivo.Visible =
                  ((int)cbbGateway.SelectedValue != (int)EGateway.NTKPOS
                && (int)cbbGateway.SelectedValue != (int)EGateway.GranitoPOS
                && (int)cbbGateway.SelectedValue != (int)EGateway.StonePOS
                && (int)cbbGateway.SelectedValue != (int)EGateway.iFood);

            this.Height = tlpCampos.PreferredSize.Height + 120;
        }
    }
}
