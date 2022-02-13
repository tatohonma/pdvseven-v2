using a7D.PDV.BLL;
using a7D.PDV.Caixa.UI.Properties;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPedidoProdutosTouch : FormTouch
    {
        private int categoriaAtual = 0;
        private ETipoPedido tipoPedido_selecionado;
        private string GUIDIdentificacao_selecionado;

        public frmPedidoProdutosTouch()
        {
            InitializeComponent();
        }

        private int idUser;
        private int idPdv;

        public bool UsarCodigoBarras { get; set; }

        public frmPedidoProdutosTouch(ETipoPedido idTipoPedido_selecionado, String guidIdentificacao_selecionado, int idFrmUser, int idFrmPdv)
            : this()
        {
            idUser = idFrmUser;
            idPdv = idFrmPdv;

            tipoPedido_selecionado = idTipoPedido_selecionado;
            GUIDIdentificacao_selecionado = guidIdentificacao_selecionado;
            try
            {
                pedidoProduto1.OcultaResumoPedido = idTipoPedido_selecionado == ETipoPedido.Balcao;
                this.LoadLocationSize(EConfig._PedidoProdutoLocationSize, prms =>
                {
                    if (prms.Length == 6)
                    {
                        scCategoriaProdutos.SplitterDistance = int.Parse(prms[4]);
                        scItensPedido.SplitterDistance = int.Parse(prms[5]);
                    }
                });
            }
            catch (Exception)
            {
            }
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            try
            {
                bool listaCreditos = frmPrincipal.ContaCliente && tipoPedido_selecionado == ETipoPedido.Balcao;

                frmPedidos.ReabrirPedidos = false;

                produtos1.ListarCreditos = listaCreditos;
                produtos1.ProdutoSelecionado += produtos1_ProdutoSelecionado;

                pedidoProduto1.CarregarPedido(tipoPedido_selecionado, GUIDIdentificacao_selecionado, AC.Usuario.IDUsuario.Value, AC.PDV.IDPDV.Value);
                pedidoProduto1.PedidoFinalizado += PedidoProduto1_PedidoFinalizado;

                categorias1.ListarCreditos = listaCreditos;
                categorias1.CategoriaSelecionada += categorias1_CategoriaSelecionada;
                categorias1.ListarCategorias();

                if (UsarCodigoBarras)
                    txtEan.Visible = true;
                else
                {
                    pedidoProduto1.Location = new Point(0, 0);
                    pedidoProduto1.Size = new Size(pedidoProduto1.Width, pedidoProduto1.Height + 34);
                    txtEan.Visible = false;
                }

                if (txtEan.Visible)
                {
                    txtEan.LostFocus += TxtEan_LostFocus;
                    txtEan.GotFocus += TxtEan_GotFocus;
                    txtEan.Focus();
                }
            }
            catch (Exception _e)
            {
                MessageBox.Show(_e.Message, "ERRO INICIALIZAÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
        }

        private bool PedidoProduto1_PedidoFinalizado(PedidoInformation pedido)
        {
            bool fechar = false;
            if (pedido != null && pedido.TipoPedido.TipoPedido == ETipoPedido.Balcao && pedido.StatusPedido.StatusPedido != EStatusPedido.Finalizado)
            {
                if (NormalOuTouch.FechaPagamento(pedido.IDPedido.Value) != DialogResult.OK)
                {
                    Pedido.AlterarStatus(pedido.IDPedido.Value, EStatusPedido.Cancelado);
                    frmPedidos.ReabrirPedidos = true;
                }
                else
                {
                    fechar = true;
                    try
                    {
                        if (OrdemProducaoServices.UsarOrdemProducao)
                        {
                            var Pedido1 = Pedido.CarregarCompleto(pedido.IDPedido.Value);

                            // Pedido finalizado em Venda balcão quando tem áreas de produção apos o pagamento tem que selecionar onde será produzido!
                            // Não é possivel selecionar antes pois os ID das áreas onde serão produzidos não ficam gravados no banco
                            while (!OrdemProducaoServices.SelecionarOrdemProducao(Pedido1.ListaProduto)) ;

                            OrdemProducaoServices.GerarOrdemProducaoEscolhida(Pedido1.ListaProduto);
                        }

                        if (OrdemProducaoServices.ImprimirViaExpedicao == "PEDIDO")
                            OrdemProducaoServices.GerarViaExpedicao(pedido.IDPedido.Value, OrdemProducaoServices.IDAreaViaExpedicao);

                    }
                    catch (Exception ex)
                    {
                        BLL.Logs.ErroBox(CodigoErro.A200, ex);
                    }
                }

                frmPedidos.ReabrirPedidos = fechar;
            }

            if (tipoPedido_selecionado != ETipoPedido.Balcao || fechar)
            {
                this.Close();
                return true;
            }
            else
                return false;
        }

        private void frmPedidoProdutosTouch_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveLocationSize(EConfig._PedidoProdutoLocationSize, () =>
            {
                return
                    scCategoriaProdutos.SplitterDistance + "," +
                    scItensPedido.SplitterDistance;
            });
        }

        private void TxtEan_GotFocus(object sender, EventArgs e)
        {
            txtEan.Text = "";
        }

        private void TxtEan_LostFocus(object sender, EventArgs e)
        {
            if (!txtEan.Focused)
                txtEan.Focus();
        }

        protected void categorias1_CategoriaSelecionada(object sender, EventArgs e)
        {
            Int32 idCategoriaProduto = categoriaAtual;
            if (sender is Control ctr)
                idCategoriaProduto = categoriaAtual = Convert.ToInt32(ctr.Name.Substring(4));

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
                new frmProdutoModificacao(ref pedidoProduto, idUser, idPdv).ShowDialog();
            }
            pedidoProduto1.VisualizarProdutos();
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
            //else if (!txtEan.Focused)
            //{
            //    StartStringEAN = Char.ConvertFromUtf32(e.KeyValue);
            //    txtEan.Focus();
            //}
        }

        private async void AdicionarProduto(string ean)
        {
            if (string.IsNullOrEmpty(ean))
                return;

            var quantidade = 1m;
            var produto = default(ProdutoInformation);

            if (ConfiguracoesCaixa.Valores.EtiquetaBalanca)
            {
                produto = Produto.ObterProdutoEtiqueta(
                    ean,
                    out decimal? quantidadeEtiqueta,
                    ConfiguracoesCaixa.Valores.DigitosCodigo,
                    ConfiguracoesCaixa.Valores.BalancaPorPeso);

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
                    new frmProdutoModificacao(ref pedidoProduto, idUser, idPdv).ShowDialog();
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
            if (!txtEan.Focused)
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (txtEan.Visible && !txtEan.Focused)
                txtEan.Focus();
        }
    }
}