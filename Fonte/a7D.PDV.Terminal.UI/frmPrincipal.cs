using a7D.PDV.Balanca;
using a7D.PDV.BLL;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Terminal.UI
{
    public partial class frmPrincipal : FormTouch
    {
        bool podeFechar = false;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                Componentes.Controles.BotaoGrid.Configure(ConfiguracoesTerminalWindows.Valores.TouchParametros);

                categorias1.CategoriaSelecionada += categorias1_CategoriaSelecionada;
                produtos1.ProdutoSelecionado += produtos1_ProdutoSelecionado;
                pedidoProduto1.PedidoFinalizado += pedidoProduto1_PedidoFinalizado;
                pedidoProduto1.onDetalhes += (gui, ev) => new frmDetalhesPedidoProduto((string)gui).ShowDialog();
                pedidoProduto1.onFinalizar += (pp, ev) => { if (txtEan.Visible) txtEan.Focus(); };
                categorias1.ListarCategorias();

                // Configura ordem de produção
                OrdemProducaoServices.UsarOrdemProducao = ConfiguracoesTerminalWindows.Valores.OrdemImpressao;
                OrdemProducaoServices.UsarAreasProducao = ConfiguracoesTerminalWindows.Valores.UsarAreas;
                OrdemProducaoServices.AreasProducaoPadrao = ConfiguracoesTerminalWindows.Valores.AreasPadrao;
                OrdemProducaoServices.ImprimirViaExpedicao = ConfiguracoesTerminalWindows.Valores.ImprimirViaExpedicao;
                OrdemProducaoServices.IDAreaViaExpedicao = ConfiguracoesTerminalWindows.Valores.IDAreaViaExpedicao;

                SelectIDValor.onSelect += (st, itens) => frmSelecao.Select("SELECIONE", st, itens);

                if (ConfiguracoesTerminalWindows.Valores.CodigoBarras)
                {
                    txtEan.Visible = true;
                }
                else
                {
                    txtEan.Visible = false;
                }

                //categorias1.ListarCategorias();
                if (txtEan.Visible)
                {
                    txtEan.LostFocus += TxtEan_LostFocus;
                    txtEan.GotFocus += TxtEan_GotFocus;
                    txtEan.Focus();
                }

                BalancaServices.InicializarBalanca(ConfiguracoesTerminalWindows.Valores.ProtocoloBalanca, ConfiguracoesTerminalWindows.Valores.PortaBalanca);
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E022, ex);
                this.Close();
            }
        }

        private void TxtEan_GotFocus(object sender, EventArgs e)
        {
            txtEan.Text = string.Empty;
        }

        private void TxtEan_LostFocus(object sender, EventArgs e)
        {
            txtEan.Focus();
        }

        private void Identificacao()
        {
            if (ConfiguracoesTerminalWindows.Valores.AutenticarSempre || AC.Usuario == null)
            {
                frmAutenticacaoTouch frm1 = new frmAutenticacaoTouch(true, true, true, true, true);
                frm1.ShowDialog();

                if (frm1.DialogResult == DialogResult.OK)
                {
                    frmIdentificacao frm2 = new frmIdentificacao();
                    frm2.ShowDialog();

                    if (frm2.GUIDIdentificacao_selecionado != null)
                        pedidoProduto1.CarregarPedido(
                            frm2.tipoPedido_selecionado,
                            frm2.GUIDIdentificacao_selecionado,
                            AC.Usuario.IDUsuario.Value,
                            AC.PDV.IDPDV.Value);
                    else
                        Identificacao();
                }
                else if (frm1.Fechar == true)
                {
                    podeFechar = true;
                    this.Close();
                }
                else
                {
                    Identificacao();
                }
            }
            else
            {
                frmIdentificacao frm2 = new frmIdentificacao();
                frm2.ShowDialog();
                if (frm2.GUIDIdentificacao_selecionado != null)
                    pedidoProduto1.CarregarPedido(frm2.tipoPedido_selecionado, frm2.GUIDIdentificacao_selecionado, AC.Usuario.IDUsuario.Value, AC.PDV.IDPDV.Value);
                else
                    Identificacao();
            }
        }

        protected void categorias1_CategoriaSelecionada(object sender, EventArgs e)
        {
            Control ctr = (Control)sender;
            Int32 idCategoriaProduto = Convert.ToInt32(ctr.Name.Substring(4));

            produtos1.ListarProdutos(idCategoriaProduto);
        }

        protected async void produtos1_ProdutoSelecionado(object sender, EventArgs e)
        {
            Control ctr = (Control)sender;
            int idProduto = Convert.ToInt32(ctr.Name.Substring(4));

            var produto = Produto.Carregar(idProduto);
            if (produto.ValorUnitario == 0m && produto.AssistenteModificacoes != true)
            {
                var frmPedidoProdutoValor = new frmTecladoValor();
                var resp = frmPedidoProdutoValor.ShowDialog();
                if (resp != DialogResult.OK)
                {
                    pedidoProduto1.VisualizarProdutos();
                    return;
                }
                else
                    produto.ValorUnitario = frmPedidoProdutoValor.Valor;
            }

            var pedidoProduto = await pedidoProduto1.AdicionarProduto(produto);
            if (pedidoProduto == null)
                return;

            pedidoProduto.Produto.ListaPainelModificacao = ProdutoPainelModificacao.Listar(idProduto);
            if (pedidoProduto.Produto.AssistenteModificacoes == true)
            {
                int? minId = -1; //  pedidoProduto1.ListaPedidoProduto.Where(l => l.IDPedidoProduto <= 0).Min(l => l.IDPedidoProduto);
                using (var form = new frmPedidoProdutosModificacoesAssistente(pedidoProduto, minId))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // Calcula o valores do item!
                        form.PainelRetornaProdutoItens(pedidoProduto);
                        if (pedidoProduto.Status == StatusModel.Inalterado)
                            pedidoProduto.Status = StatusModel.Alterado;
                    }
                    else
                    {
                        if (pedidoProduto.Status == StatusModel.Novo && pedidoProduto.ListaModificacao == null)
                            pedidoProduto.Status = StatusModel.Excluido;
                    }
                }
            }
            else if (pedidoProduto.Produto.ListaPainelModificacao.Count > 0)
            {
                new frmProdutoModificacao(ref pedidoProduto, AC.Usuario.IDUsuario.Value, AC.PDV.IDPDV.Value).ShowDialog();
            }
            pedidoProduto1.VisualizarProdutos();
        }

        protected bool pedidoProduto1_PedidoFinalizado(PedidoInformation pedido)
        {
            pedidoProduto1.LimparControle();
            Identificacao();
            return false;
        }

        private void frmPrincipal_Shown(object sender, EventArgs e)
        {
            Identificacao();
        }

        private void txtEan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtEan.Text == "9999")
                {
                    pedidoProduto1.Finalizar(sender, e);
                }
                else
                {
                    AdicionarProduto(txtEan.Text);
                }
            }
        }

        private async void AdicionarProduto(string ean)
        {
            var quantidade = 1m;
            var produto = default(ProdutoInformation);

            if (ConfiguracoesTerminalWindows.Valores.EtiquetaBalanca)
            {
                produto = Produto.ObterProdutoEtiqueta(
                    ean,
                    out decimal? quantidadeEtiqueta,
                    ConfiguracoesCaixa.Valores.DigitosCodigo,
                    ConfiguracoesCaixa.Valores.BalancaPorPeso); // A formatação de valores dos itens pode ter problemas

                if (quantidadeEtiqueta.HasValue)
                    quantidade = quantidadeEtiqueta.Value;
            }

            if (produto == null)
                produto = Produto.BuscarPorEan(ean);

            if (produto != null)
            {
                var idProduto = produto.IDProduto.Value;
                if (produto.ValorUnitario == 0m)
                {
                    var frmPedidoProdutoValor = new frmTecladoValor();
                    var resp = frmPedidoProdutoValor.ShowDialog();
                    if (resp != DialogResult.OK)
                    {
                        pedidoProduto1.VisualizarProdutos();
                        return;
                    }
                    else
                        produto.ValorUnitario = frmPedidoProdutoValor.Valor;
                }

                var pedidoProduto = await pedidoProduto1.AdicionarProduto(produto, quantidade);
                if (pedidoProduto == null)
                    return;

                if (ProdutoPainelModificacao.Listar(idProduto).Where(m => m.PainelModificacao.ListaProduto.Count > 0).ToList().Count > 0)
                {
                    new frmProdutoModificacao(ref pedidoProduto, AC.Usuario.IDUsuario.Value, AC.PDV.IDPDV.Value).ShowDialog();
                    pedidoProduto1.VisualizarProdutos();
                }
            }
            else
            {
                MessageBox.Show("Produto não cadastrado", "Atenção");
            }

            txtEan.Text = string.Empty;
        }

        private void frmPrincipal_Enter(object sender, EventArgs e)
        {
            txtEan.Focus();
        }

        private void txtEan_MouseDown(object sender, MouseEventArgs e)
        {
            var frm = new frmNumero();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                AdicionarProduto(frm.Valor.ToString());
            }

        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!podeFechar)
            {
                e.Cancel = true;
                Identificacao();
            }
        }
    }
}