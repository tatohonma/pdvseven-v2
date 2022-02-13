using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPedidoProdutoDesconto : FormTouch
    {
        private PedidoProdutoInformation PedidoProduto1 { get; }
        private ProdutoInformation Produto1 { get; }
        private UsuarioInformation UsuarioDesconto { get; }
        private decimal ValorLista { get; }

        private decimal ValorDesconto { get; set; }

        private readonly string _formatoMoeda = "R$ #,##0.00";
        private readonly string _formatoNumero = "#,##0.00";
        private readonly IFormatProvider _cultureInfo = new CultureInfo("pt-BR");

        private bool SolicitarTipoDesconto { get; set; } = false;

        private frmPedidoProdutoDesconto()
        {
            InitializeComponent();
        }

        public frmPedidoProdutoDesconto(int idPedidoProduto, int idUsuario) : this(PedidoProduto.Carregar(idPedidoProduto), Usuario.Carregar(idUsuario))
        {
        }

        public frmPedidoProdutoDesconto(PedidoProdutoInformation pedidoProduto, UsuarioInformation usuarioDesconto) : this()
        {
            PedidoProduto1 = pedidoProduto;
            Produto1 = PedidoProduto1.Produto;
            UsuarioDesconto = usuarioDesconto;
            ValorLista = Produto1.ValorUnitario.Value;
        }

        private void frmPedidoProdutoDesconto_Load(object sender, EventArgs e)
        {
            GA.Post(this);

            lblNomeProduto.Text = Produto1.Nome;
            lblPreco.Text = ValorLista.ToString(_formatoMoeda, _cultureInfo);
            ValorDesconto = PedidoProduto1.ValorDesconto ?? 0;

            if (ValorDesconto > 0)
                lblPreco.Font = new Font(lblPreco.Font, FontStyle.Strikeout);
            else
                lblPreco.Font = new Font(lblPreco.Font, FontStyle.Regular);

            txtDescontoValor.Text = ValorDesconto.ToString(_formatoNumero, _cultureInfo);

            if (ValorDesconto >= ValorLista)
                txtDescontoPercentual.Text = "100";
            else
            {
                txtDescontoPercentual.Text = (ValorDesconto * 100m / ValorLista).ToString(_formatoNumero, _cultureInfo);
            }

            lblValorFinal.Text = (ValorLista - (PedidoProduto1.ValorDesconto ?? 0)).ToString(_formatoMoeda, _cultureInfo);

            SolicitarTipoDesconto = ConfiguracoesSistema.Valores.SolicitarTipoDescontoItem;

            if (SolicitarTipoDesconto)
            {
                lblTipoDesconto.Visible = cbbTipoDesconto.Visible = true;
                var listaTipoDesconto = TipoDesconto.ListarAtivos();
                listaTipoDesconto.Insert(0, new TipoDescontoInformation { });
                cbbTipoDesconto.DataSource = listaTipoDesconto.ToArray();

                if (PedidoProduto1.TipoDesconto?.IDTipoDesconto != null)
                {
                    cbbTipoDesconto.SelectedValue = PedidoProduto1.TipoDesconto.IDTipoDesconto.Value;
                }
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validar())
                {
                    PedidoProduto1.ValorDesconto = ValorDesconto;
                    PedidoProduto1.ValorUnitario = ValorLista - ValorDesconto;
                    if (ValorDesconto > 0)
                    {
                        PedidoProduto1.UsuarioDesconto = UsuarioDesconto;
                        if (SolicitarTipoDesconto)
                            PedidoProduto1.TipoDesconto = new TipoDescontoInformation { IDTipoDesconto = Convert.ToInt32(cbbTipoDesconto.SelectedValue) };
                    }
                    else
                    {
                        PedidoProduto1.UsuarioDesconto = null;
                        if (SolicitarTipoDesconto)
                            PedidoProduto1.TipoDesconto = null;
                    }
                    if (PedidoProduto1.Status != StatusModel.Novo)
                        PedidoProduto.Salvar(PedidoProduto1);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                BLL.Logs.ErroBox( CodigoErro.E100, ex);
                //new ExibirErro(ex).Exibir();
            }
        }

        public bool Validar()
        {
            try
            {
                var msg = string.Empty;
                ValorDesconto = Convert.ToDecimal(txtDescontoValor.Text, _cultureInfo);
                if (ValorDesconto < 0)
                    msg += "Desconto não pode ser negativo \n";
                if (ValorDesconto > ValorLista)
                    msg += "Desconto não pode ser maior que o valor do produto \n";

                if (SolicitarTipoDesconto && ValorDesconto > 0 && cbbTipoDesconto.SelectedIndex <= 0)
                {
                    msg += "Selecione o Tipo de Desconto \n";
                }

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    MessageBox.Show(msg, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        private void txtDescontoValor_TextChanged(object sender, EventArgs e)
        {
            if (txtDescontoValor.Focused)
            {
                AtualizarDescontoReais();
            }
        }

        private void txtDescontoPercentual_TextChanged(object sender, EventArgs e)
        {
            if (txtDescontoPercentual.Focused)
            {
                AtualizarDescontoPercentual();
            }
        }

        private void AtualizarExibicao()
        {
            if (ValorDesconto > 0)
                lblPreco.Font = new Font(lblPreco.Font, FontStyle.Strikeout);
            else
                lblPreco.Font = new Font(lblPreco.Font, FontStyle.Regular);

            lblValorFinal.Text = (ValorLista - ValorDesconto).ToString(_formatoMoeda, _cultureInfo);
        }

        private void AtualizarDescontoPercentual()
        {
            Decimal descontoPorcentagem = 0;
            ValorDesconto = 0;

            if (!string.IsNullOrWhiteSpace(txtDescontoPercentual.Text))
                descontoPorcentagem = Convert.ToDecimal(txtDescontoPercentual.Text, _cultureInfo);

            if (descontoPorcentagem < 100)
            {
                ValorDesconto = ValorLista * descontoPorcentagem / 100m;
            }
            else
            {
                ValorDesconto = ValorLista;
            }

            ValorDesconto = Math.Truncate(ValorDesconto * 100m) / 100m;
            txtDescontoValor.Text = ValorDesconto.ToString(_formatoNumero, _cultureInfo);

            AtualizarExibicao();
        }

        private void AtualizarDescontoReais()
        {
            Decimal descontoPorcentagem = 0;
            ValorDesconto = 0;

            if (!string.IsNullOrWhiteSpace(txtDescontoValor.Text))
                ValorDesconto = Convert.ToDecimal(txtDescontoValor.Text, _cultureInfo);

            if (ValorDesconto < ValorLista)
            {
                descontoPorcentagem = ValorDesconto * 100m / ValorLista;
            }
            else
            {
                ValorDesconto = ValorLista;
                descontoPorcentagem = 100;
            }

            txtDescontoPercentual.Text = descontoPorcentagem.ToString(_formatoNumero, _cultureInfo);

            AtualizarExibicao();
        }

        private void txtDescontoValor_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescontoValor.Text))
                txtDescontoValor.Text = 0.ToString(_formatoNumero, _cultureInfo);
        }

        private void txtDescontoPercentual_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescontoPercentual.Text))
                txtDescontoPercentual.Text = 0.ToString(_formatoNumero, _cultureInfo);
        }

        private void SomenteNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || (e.KeyChar == (char)44 && (txt.Text.Contains(",") == true || txt.Text.Length == 0)))
                e.Handled = true;
        }
    }
}
