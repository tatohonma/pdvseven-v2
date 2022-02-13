using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI.Controles
{
    public partial class PedidoPagamentoFormas : UserControl
    {
        private IPedidoPagamentoForm frmPP;
        private List<PedidoPagamentoInformation> ListaPagamentos => frmPP.PedidoAtual.ListaPagamento;

        public bool FecharPagamento { get; private set; }

        public int SplitterDistance
        {
            get { return split.SplitterDistance; }
            set { split.SplitterDistance = value; }
        }

        public event EventHandler PagamentosAlterados;

        public PedidoPagamentoFormas()
        {
            InitializeComponent();
        }

        private void PedidoPagamentoFormas_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            FecharPagamento = false;
            frmPP = this.ParentForm as IPedidoPagamentoForm;

            //if (grpTouch.Visible = BLL.ConfiguracoesCaixa.Valores.CaixaTouch)
            //{
            //tlp.RowStyles[0].Height = 0;
            tpList.Fill();
            tpList.ObterValorPadrao += () => frmPP.ValorPendente;
            //}
            //if (grpNormal.Visible = !BLL.ConfiguracoesCaixa.Valores.CaixaTouch)
            //{
            //    tlp.RowStyles[1].Height = 0;
            //    CarregarTipoPagamento();
            //}
            ListarPagamento();
        }

        /*
         * 
        #region Eventos Não touch

        private void CarregarTipoPagamento()
        {
            var list = BLL.TipoPagamento.ListarAtivos();

            string formato = "({0}) {1}";
            cbbFormaPagamento.Items.Add(new TipoPagamentoInformation());
            foreach (var item in list)
            {
                item.Nome = string.Format(formato, item.IDTipoPagamento.Value.ToString("00"), item.Nome);
                cbbFormaPagamento.Items.Add(item);
            }
            cbbFormaPagamento.SelectedIndex = 0;
        }

        private void txtCodigoFormaPagamento_TextChanged(object sender, EventArgs e)
        {
            cbbFormaPagamento.SelectedIndex = 0;
            for (int i = 1; i < cbbFormaPagamento.Items.Count; i++)
            {
                if (((TipoPagamentoInformation)cbbFormaPagamento.Items[i]).IDTipoPagamento.ToString() == txtCodigoFormaPagamento.Text)
                {
                    cbbFormaPagamento.SelectedIndex = i;
                    break;
                }
            }
        }

        private void btnAdicionarPagamento_Click(object sender, EventArgs e)
        {
            var pagamento = new PedidoPagamentoInformation
            {
                Excluido = false
            };

            // no load sempre carrega um pagamento vazio para ser opção a seleção
            if (cbbFormaPagamento.SelectedIndex < 1 && cbbFormaPagamento.Items.Count > 0)
                pagamento.TipoPagamento = (TipoPagamentoInformation)cbbFormaPagamento.Items[1];
            else
                pagamento.TipoPagamento = (TipoPagamentoInformation)cbbFormaPagamento.SelectedItem;

            var valorPago = ListaPagamentos.Sum(l => l.Valor.Value);

            if (txtValorPagamento.Text == "")
                pagamento.Valor = frmPP.ValorPendente;
            else
                pagamento.Valor = Convert.ToDecimal(txtValorPagamento.Text);

            cbbFormaPagamento.SelectedIndex = 0;
            txtValorPagamento.Text = "";

            txtCodigoFormaPagamento.Text = "";
            txtCodigoFormaPagamento.Focus();

            NovoPagamentoValor(pagamento);
        }

        private void txtValorPagamento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && cbbFormaPagamento.SelectedIndex > 0 && (txtValorPagamento.Text == "" || Convert.ToDecimal(txtValorPagamento.Text) > 0))
                btnAdicionarPagamento_Click(sender, e);
        }

        private void SomenteNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || (e.KeyChar == (char)44 && (txt.Text.Contains(",") == true || txt.Text.Length == 0)))
                e.Handled = true;
        }

        private void cbbFormaPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtValorPagamento.Focus();
        }
        #endregion

        */

        #region Eventos Touch

        private void tpList_PagamentoSelecionadoValor(PedidoPagamentoInformation pagamento)
        {
            NovoPagamentoValor(pagamento);
        }

        #endregion

        #region Lista de Pagamentos

        private void NovoPagamentoValor(PedidoPagamentoInformation pagamento)
        {
            if (pagamento.Valor <= 0)
                return;

            var valorPagoDinheiro = ListaPagamentos.Where(l => l.MeioPagamentoSAT.IDMeioPagamentoSAT == (int)EMetodoPagamento.Dinheiro).Sum(l => l.Valor.Value);

            if (pagamento.TipoPagamento.MeioPagamentoSAT?.IDMeioPagamentoSAT == (int)EMetodoPagamento.Dinheiro)
                valorPagoDinheiro += pagamento.Valor.Value;

            var valorPendente = frmPP.ValorPendente - pagamento.Valor.Value;

            this.ParentForm.TopMost = false;

            if ((valorPagoDinheiro + valorPendente) < 0)
            {
                MessageBox.Show("Não é permitido voltar troco maior que o valor pago em Dinheiro!");
            }
            else if (PagamentoVenda.Efetiva(frmPP.PedidoAtual, pagamento, valorPendente))
            {
                if (pagamento.TipoPagamento.Gateway == EGateway.ContaCliente)
                {
                    FecharPagamento = true;
                }
                else if (pagamento.TipoPagamento.IDGateway > 0)
                {
                    // Pagamento por gateway pode fechar a tela diretamente, se não houver valor pendente
                    FecharPagamento = valorPendente == 0;
                    frmPP.HabilitarParcial();
                }
                ListaPagamentos.Add(pagamento);
            }
            this.ParentForm.TopMost = true;

            ListarPagamento();
            PagamentosAlterados?.Invoke(null, null);
        }

        private void ListarPagamento()
        {
            var list = from l in ListaPagamentos
                       select new { TipoPagamento = l.TipoPagamento.Nome, l.Valor };

            dgvPagamentos.DataSource = list.ToArray();
        }

        private void dgvPagamentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (ListaPagamentos[e.RowIndex].IDPedidoPagamento.HasValue
                 || ListaPagamentos[e.RowIndex].Autorizacao != null)
                {
                    if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) != DialogResult.OK)
                        return;

                    if (ListaPagamentos[e.RowIndex].IDPedidoPagamento != null)
                        BLL.PedidoPagamento.Cancelar(ListaPagamentos[e.RowIndex], usuario.IDUsuario.Value);

                }
                ListaPagamentos.RemoveAt(e.RowIndex);

                ListarPagamento();
                PagamentosAlterados?.Invoke(null, null);
            }
        }

        #endregion

    }
}
