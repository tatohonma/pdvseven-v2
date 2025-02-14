using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmImprimirConta : FormTouch
    {

        private int Quantidade { get; set; }
        public PedidoInformation Pedido1 { get; }
        public bool Opcoes
        {
            set
            {
                ckbAplicarServico.Enabled = ckbAplicaDesconto.Enabled = value;
            }
        }

        private readonly string _formatoNumero = "#,##0.00";
        private readonly IFormatProvider _provider = new CultureInfo("pt-BR");

        private frmImprimirConta()
        {
            InitializeComponent();
            txtQuantidade.MouseWheel += TxtQuantidade_MouseWheel;
            Quantidade = 1;
        }

        public frmImprimirConta(PedidoInformation pedido) : this()
        {
            Pedido1 = pedido;
        }

        private void frmImprimirConta_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            if (Pedido1.NumeroPessoas > 0)
            {
                Quantidade = Pedido1.NumeroPessoas.Value;
                txtQuantidade.Text = Quantidade.ToString();
            }

            txtAcrescimoPorcentagem.Text = Pedido1.TaxaServicoPadrao.Value.ToString(_formatoNumero, _provider);
            txtAcrescimoReais.Text = Pedido1.ValorServicoTemp.ToString(_formatoNumero, _provider);

            ckbAplicarServico.Checked = Pedido1.AplicarServico == null || Pedido1.AplicarServico == true;
            CalcularServico();
            CalcularConsumacaoMinima();

            if (Pedido1.AplicarDesconto == true & Pedido1.ValorDesconto > 0)
            {
                //cid
                if (Pedido1.TipoDesconto.Nome != null)
                {
                    ckbAplicaDesconto.Text = Pedido1.TipoDesconto.Nome;
                }
                //cid
                ckbAplicaDesconto.Checked = true;
                txtDescontoPorcentagem.Enabled = txtDescontoReais.Enabled = true;
                txtDescontoReais.Text = Pedido1.ValorDesconto.Value.ToString(_formatoNumero, _provider);
                AtualizarDescontoReais();
            }

            AtualizarPainel();
        }

        private void TxtQuantidade_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                Quantidade++;
            else
            {
                if (Quantidade > 1)
                    Quantidade--;
            }
            txtQuantidade.Text = Quantidade.ToString();
            txtQuantidade.SelectAll();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            DefinirQuantidade();
        }

        private void DefinirQuantidade()
        {
            try
            {
                Quantidade = Convert.ToInt32(txtQuantidade.Text);
                if (Quantidade <= 0)
                    Quantidade = 1;
            }
            catch
            {
                Quantidade = 1;
            }
            Pedido1.NumeroPessoas = Quantidade;
            Pedido.Salvar(Pedido1);
            Close();
        }

        private void txtQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void ckbAplicarServico_Click(object sender, EventArgs e)
        {
            var usuarioTaxaServico = default(UsuarioInformation);
            if (Autenticar(out usuarioTaxaServico))
            {
                Pedido1.AplicarServico = ckbAplicarServico.Checked;
                Pedido1.UsuarioTaxaServico = usuarioTaxaServico;

                CalcularServico();
                CalcularConsumacaoMinima();
                AtualizarPainel();
            }
            else
            {
                ckbAplicarServico.Checked = !ckbAplicarServico.Checked;
            }
        }

        private void ckbAplicaDesconto_Click(object sender, EventArgs e)
        {

            var solicitarSenha = ConfiguracoesSistema.Valores.SolicitarSenhaDesconto;
            DialogResult r = DialogResult.None;
            var usuarioDesconto = default(UsuarioInformation);

            if (solicitarSenha)
                r = NormalOuTouch.Autenticacao(false, true, false, false, out usuarioDesconto);
            else
                usuarioDesconto = AC.Usuario;

            if (solicitarSenha == false || r == DialogResult.OK)
            {
                Pedido1.AplicarDesconto = ckbAplicaDesconto.Checked;
                if (ckbAplicaDesconto.Checked == true)
                {
                    DialogResult tipoR = DialogResult.None;

                    var solicitarTipoDesconto = ConfiguracoesSistema.Valores.SolicitarTipoDesconto;
                    frmTipoDesconto frmTipoDesconto = null;

                    if (solicitarTipoDesconto)
                    {
                        frmTipoDesconto = new frmTipoDesconto();
                        frmTipoDesconto.ShowDialog();
                        tipoR = frmTipoDesconto.DialogResult;
                    }

                    if (solicitarTipoDesconto && tipoR == DialogResult.OK)
                    {
                        Pedido1.UsuarioDesconto = usuarioDesconto;
                        Pedido1.TipoDesconto = new TipoDescontoInformation { IDTipoDesconto = frmTipoDesconto.IDTipoDesconto, Descricao = frmTipoDesconto.Descricao };
                        txtDescontoPorcentagem.Enabled = true;
                        txtDescontoReais.Enabled = true;
                        ckbAplicaDesconto.Text = Pedido1.TipoDesconto.Descricao;
                    }
                    else if (solicitarTipoDesconto && tipoR != DialogResult.OK)
                    {
                        Pedido1.UsuarioDesconto = null;
                        ckbAplicaDesconto.Checked = !ckbAplicaDesconto.Checked;
                    }
                    else
                    {
                        Pedido1.UsuarioDesconto = usuarioDesconto;
                        txtDescontoPorcentagem.Enabled = true;
                        txtDescontoReais.Enabled = true;
                    }
                }
                else
                {
                    ckbAplicaDesconto.Text = "Aplica desconto";
                    Pedido1.TipoDesconto = null;
                    txtDescontoPorcentagem.Text = "0,00";
                    AtualizarDescontoPorcentagem();

                    Pedido1.UsuarioDesconto = usuarioDesconto;

                    txtDescontoPorcentagem.Enabled = false;
                    txtDescontoReais.Enabled = false;
                }

            }
            else
            {
                ckbAplicaDesconto.Checked = !ckbAplicaDesconto.Checked;
            }
        }

        private static bool Autenticar(out UsuarioInformation usuario)
        {
            return NormalOuTouch.Autenticacao(false, true, false, false, out usuario) == DialogResult.OK;
        }

        private void CalcularServico()
        {
            Pedido.AdicionarProdutoServico(Pedido1, ckbAplicarServico.Checked, AC.PDV, AC.Usuario);
        }

        private void CalcularConsumacaoMinima()
        {
            Pedido.AdicionarProdutoConsumacaoMinima(Pedido1, AC.PDV, AC.Usuario);
        }

        private void txtDescontoPorcentagem_TextChanged(object sender, EventArgs e)
        {
            if (txtDescontoPorcentagem.Focused == true)
            {
                AtualizarDescontoPorcentagem();
            }
        }

        private void txtDescontoReais_TextChanged(object sender, EventArgs e)
        {
            if (txtDescontoReais.Focused == true)
            {
                AtualizarDescontoReais();
            }
        }

        private void AtualizarDescontoPorcentagem()
        {
            Decimal descontoPorcentagem = 0;
            Decimal descontoReais = 0;

            if (txtDescontoPorcentagem.Text != "")
                descontoPorcentagem = Convert.ToDecimal(txtDescontoPorcentagem.Text);

            if (descontoPorcentagem < 100)
            {
                descontoReais = Pedido1.ValorTotalProdutosServicos * descontoPorcentagem / 100m;
            }
            else
            {
                descontoReais = Pedido1.ValorTotalProdutosServicos;
            }

            descontoReais = Math.Truncate(descontoReais * 100m) / 100m;
            txtDescontoReais.Text = descontoReais.ToString("#,##0.00");
            Pedido1.ValorDesconto = descontoReais;

            AtualizarPainel();
        }

        private void AtualizarDescontoReais()
        {
            Decimal descontoPorcentagem = 0;
            Decimal descontoReais = 0;

            if (txtDescontoReais.Text != "")
                descontoReais = Convert.ToDecimal(txtDescontoReais.Text);

            if (descontoReais < Pedido1.ValorTotalProdutosServicos)
            {
                descontoPorcentagem = descontoReais * 100m / Pedido1.ValorTotalProdutosServicos;

                txtDescontoPorcentagem.Text = descontoPorcentagem.ToString("#,##0.00");
            }
            else
            {
                descontoReais = Pedido1.ValorTotalProdutosServicos;
                descontoPorcentagem = 100;

                txtDescontoPorcentagem.Text = descontoPorcentagem.ToString("#,##0.00");
            }

            Pedido1.ValorDesconto = descontoReais;

            AtualizarPainel();
        }

        private void AtualizarPainel()
        {
            var total = Pedido1.ValorTotalProdutos - (Pedido1.ValorDesconto ?? 0) + (Pedido1.ValorServico ?? 0) + (Pedido1.ValorEntrega ?? 0);
            lblTotal.Text = total.ToString(_formatoNumero, _provider);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
