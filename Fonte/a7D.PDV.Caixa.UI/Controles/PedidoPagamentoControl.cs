using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI.Controles
{
    public partial class PedidoPagamentoControl : UserControl
    {
        private IPedidoPagamentoForm frmPP;
        private PedidoInformation PedidoAtual => frmPP?.PedidoAtual;
        public decimal Total { get; private set; }
        public decimal ValorPago { get; private set; }
        public decimal ValorPendenteSemDesconto { get; private set; }
        public decimal ValorPendente { get; private set; }
        public bool AceitarParcial { get; private set; }

        public event EventHandler ParametrosAlterados;

        public PedidoPagamentoControl()
        {
            InitializeComponent();
        }

        #region Eventos

        private void PedidoPagamento_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            frmPP = this.ParentForm as IPedidoPagamentoForm;
            if (PedidoAtual == null)
                return;

            txtAcrescimoReais.Text = PedidoAtual.ValorServicoTemp.ToString("#,#0.00");
            txtAcrescimoPorcentagem.Text = PedidoAtual.TaxaServicoPadrao.Value.ToString("0.00");

            if (PedidoAtual.AplicarServico == null)
                PedidoAtual.AplicarServico = true;

            ckbAplicarServico.Checked = PedidoAtual.AplicarServico == true;

            if (PedidoAtual.AplicarDesconto == true && PedidoAtual.ValorDesconto > 0)
            {
                txtDescontoReais.Text = PedidoAtual.ValorDesconto.Value.ToString("0.00");
                //cid
                if (PedidoAtual.TipoDesconto.Nome != null)
                {
                    ckbAplicaDesconto.Text = PedidoAtual.TipoDesconto.Nome;
                }
                //cid

                ckbAplicaDesconto.Checked = true;
                txtDescontoPorcentagem.Enabled = txtDescontoReais.Enabled = true;
            }
            else
                PedidoAtual.ValorDesconto = 0;

            CalcularExibir();
        }

        private void ckbAplicaDesconto_Click(object sender, EventArgs e)
        {
            UsuarioInformation usuarioDesconto = null;
            if (ValorPendenteSemDesconto <= 0)
            {
                MessageBox.Show("Não há valores para aplicar desconto");
                if (ckbAplicaDesconto.Checked)
                    ckbAplicaDesconto.Checked = false;
                return;
            }

            var resultSenha = DialogResult.None;
            if (ConfiguracoesSistema.Valores.SolicitarSenhaDesconto)
                resultSenha = NormalOuTouch.Autenticacao(false, true, false, false, out usuarioDesconto);
            else
            {
                resultSenha = DialogResult.OK;
                usuarioDesconto = AC.Usuario;
            }

            if (resultSenha != DialogResult.OK)
            {
                // Volta a situação anterior
                ckbAplicaDesconto.Checked = !ckbAplicaDesconto.Checked;
                return;
            }

            if (ckbAplicaDesconto.Checked)
            {
                var solicitarTipoDesconto = ConfiguracoesSistema.Valores.SolicitarTipoDesconto;
                TipoDescontoInformation tipoDesconto = null;
                var tipoResult = DialogResult.None;
                if (solicitarTipoDesconto)
                {
                    var frmTipoDesconto = new frmTipoDesconto();
                    tipoResult = frmTipoDesconto.ShowDialog();
                    tipoDesconto = new TipoDescontoInformation { IDTipoDesconto = frmTipoDesconto.IDTipoDesconto, Descricao = frmTipoDesconto.Descricao };
                }
                else
                    tipoResult = DialogResult.OK;

                if (tipoResult == DialogResult.OK)
                {
                    PedidoAtual.TipoDesconto = tipoDesconto;
                    PedidoAtual.UsuarioDesconto = usuarioDesconto;
                    txtDescontoPorcentagem.Enabled = true;
                    txtDescontoReais.Enabled = true;
                }
                else
                {
                    // Volta a situação anterior
                    ckbAplicaDesconto.Checked = !ckbAplicaDesconto.Checked;
                    return;
                }
            }
            else
            {
                ckbAplicaDesconto.Text = "Aplica desconto";
                PedidoAtual.TipoDesconto = null;
                PedidoAtual.UsuarioDesconto = usuarioDesconto;

                txtDescontoPorcentagem.Text = "0,00";
                txtDescontoReais.Text = "0,00";
                PedidoAtual.ValorDesconto = 0;

                txtDescontoPorcentagem.Enabled = false;
                txtDescontoReais.Enabled = false;

            }
            CalcularExibir();
            ParametrosAlterados?.Invoke(null, null);
        }

        private void txtDescontoPorcentagem_TextChanged(object sender, EventArgs e)
        {
            AtualizarDescontoPorcentagem();
            CalcularExibir();
            ParametrosAlterados?.Invoke(null, null);
        }

        private void txtDescontoReais_TextChanged(object sender, EventArgs e)
        {
            if (txtDescontoReais.Focused == true)
            {
                AtualizarDescontoReais();
                CalcularExibir();
                ParametrosAlterados?.Invoke(null, null);
            }
        }

        private void SomenteNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || (e.KeyChar == (char)44 && (txt.Text.Contains(",") == true || txt.Text.Length == 0)))
                e.Handled = true;
        }

        private void txtDescontoReais_Leave(object sender, EventArgs e)
        {
            if (PedidoAtual.ValorDesconto.Value > 0)
                txtDescontoReais.Text = PedidoAtual.ValorDesconto.Value.ToString("N2");
            else
                txtDescontoReais.Text = "0,00";
        }

        private void txtDescontoPercentual_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal descontoPorcentagem = Convert.ToDecimal(txtDescontoPorcentagem.Text);
                if (descontoPorcentagem < 0)
                    txtDescontoPorcentagem.Text = "0,00";
                else if (descontoPorcentagem > 100)
                    txtDescontoPorcentagem.Text = "100,00";
            }
            catch (Exception)
            {
                txtDescontoPorcentagem.Text = "0,00";
            }
        }

        #endregion

        public void HabilitarParcial()
        {
            AceitarParcial = true;
        }

        public void CalcularExibir()
        {
            // Essa rotina deve ser a ultima coisa a ser chamada para que seja calculado tudo corretamente
            // E para evitar qualquer confusão somente os eventos primerios devem chamar essa rotina e depois notificar a 'ParametrosAlterados'
            Pedido.AdicionarProdutoConsumacaoMinima(PedidoAtual, AC.PDV, AC.Usuario);
            Pedido.AdicionarProdutoServico(PedidoAtual, ckbAplicarServico.Checked, AC.PDV, AC.Usuario);

            if (!AceitarParcial && PedidoAtual.ListaPagamento?.Any(p => p.IDPedidoPagamento > 0) == true)
                AceitarParcial = true;

            lblSubTotal.Text = PedidoAtual.ValorTotalProdutos.ToString("#,##0.00");

            AtualizarDescontoPorcentagem();
            AtualizarPainel();
        }

        private void AtualizarDescontoPorcentagem()
        {
            // tem que validar aqui dentro de porcentagem (padrão)
            if (!txtDescontoPorcentagem.Focused)
                return;

            Decimal descontoPorcentagem = 0;
            Decimal descontoReais = 0;

            if (txtDescontoPorcentagem.Text != "")
            {
                try
                {
                    descontoPorcentagem = Convert.ToDecimal(txtDescontoPorcentagem.Text);
                }
                catch (Exception)
                {
                    descontoPorcentagem = 0;
                    txtDescontoPorcentagem.Text = "0,00";
                }
            }

            if (ValorPendenteSemDesconto <= 0)
            {
                ckbAplicaDesconto.Checked = false;
                return;
            }
            else if (descontoPorcentagem < 100)
            {
                descontoReais = ValorPendenteSemDesconto * descontoPorcentagem / 100m;
            }
            else
            {
                descontoReais = ValorPendenteSemDesconto;
            }

            descontoReais = Math.Truncate(descontoReais * 100m) / 100m;
            txtDescontoReais.Text = descontoReais.ToString("#,##0.00");
            PedidoAtual.ValorDesconto = descontoReais;
        }

        private void AtualizarDescontoReais()
        {
            Decimal descontoPorcentagem = 0;
            Decimal descontoReais = 0;

            if (txtDescontoReais.Text != "")
            {
                try
                {
                    descontoReais = Convert.ToDecimal(txtDescontoReais.Text);
                }
                catch (Exception)
                {
                    descontoReais = 0;
                    txtDescontoReais.Text = "0,00";
                }
            }

            if (ValorPendenteSemDesconto <= 0)
            {
                ckbAplicaDesconto.Checked = false;
                return;
            }
            else if (descontoReais < ValorPendenteSemDesconto)
            {
                descontoPorcentagem = descontoReais * 100m / ValorPendenteSemDesconto;
            }
            else
            {
                descontoReais = ValorPendenteSemDesconto;
                descontoPorcentagem = 100m;
            }

            descontoReais = Math.Truncate(descontoReais * 100m) / 100m;
            PedidoAtual.ValorDesconto = descontoReais;
            txtDescontoPorcentagem.Text = descontoPorcentagem.ToString("#,##0.00");
        }

        private void AtualizarPainel()
        {
            if (PedidoAtual.ListaPagamento != null)
                ValorPago = PedidoAtual.ListaPagamento.Sum(l => l.Valor.Value);

            Total = PedidoAtual.ValorTotalProdutos + PedidoAtual.ValorServico.Value;
            ValorPendenteSemDesconto = Total - ValorPago;

            Total -= (PedidoAtual.ValorDesconto ?? 0);
            ValorPendente = Total - ValorPago;

            lblTotal.Text = Total.ToString("#,##0.00");
            lblValorPago.Text = ValorPago.ToString("#,##0.00");

            if (ValorPendente <= Convert.ToDecimal("0,009"))
            {
                gbxValorPendente.Text = ValorPendente == 0 ? "SEM VALOR" : "TROCO";
                lblValorPendente.Text = (ValorPendente * -1).ToString("#,##0.00");
                lblValorPendente.ForeColor = Color.Green;
            }
            else
            {
                gbxValorPendente.Text = "FALTA PAGAR";
                lblValorPendente.Text = ValorPendente.ToString("#,##0.00");
                lblValorPendente.ForeColor = Color.Red;
            }
        }

        private void CkbAplicarServico_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuarioTaxaServico) == DialogResult.OK)
            {
                PedidoAtual.UsuarioTaxaServico = usuarioTaxaServico;

                CalcularExibir();
                ParametrosAlterados?.Invoke(null, null);
            }
            else
            {
                ckbAplicarServico.Checked = !ckbAplicarServico.Checked;
            }
        }
    }
}
