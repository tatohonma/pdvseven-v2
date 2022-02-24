using a7D.PDV.Balanca;
using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Componentes.Controles
{
    public partial class PedidoProduto : UserControl
    {
        public delegate bool PedidoFinalizadoEventHandler(PedidoInformation pedido);
        public event PedidoFinalizadoEventHandler PedidoFinalizado;

        private String GUIDIdentificacao_selecionado;
        private ETipoPedido tipoPedido_selecionado;
        private int idUser;
        private int idPDV;
        public event EventHandler onFinalizar;
        public event EventHandler onDetalhes;

        public bool OcultaResumoPedido { get; set; }

        internal List<PedidoProdutoInformation> ListaPedidoProduto { get; set; }

        private decimal ValorPedido;
        private bool retornouCredito;
        private Decimal taxaServico;

        public PedidoProduto()
        {
            InitializeComponent();
        }

        private void PedidoProduto_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            btnViagem.Visible = ConfiguracoesSistema.Valores.ProdutoViagem;
            pnlTitulo.Visible = !OcultaResumoPedido;

            dgvItensSelecionados.Location = new Point(0, OcultaResumoPedido ? pnlTitulo2.Height : pnlTitulo.Height);
            dgvItensSelecionados.Size = new Size(
                dgvItensSelecionados.Size.Width,
                this.Height - btnConfirmar.Height - (OcultaResumoPedido ? pnlTitulo2.Height : pnlTitulo.Height));

            pnlTitulo2.Location = new Point(
                OcultaResumoPedido ? 0 : this.Width - 232,
                OcultaResumoPedido ? 0 : pnlTitulo.Height - 56);

            if (OcultaResumoPedido)
                pnlTitulo2.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            pnlTitulo2.Size = new Size(OcultaResumoPedido ? this.Width : 230, 54);
        }

        public async Task<PedidoProdutoInformation> AdicionarProduto(ProdutoInformation produto, decimal quantidade = 1m)
        {
            var min = ListaPedidoProduto?.Min(p => p.IDPedidoProduto);
            var idPedidoProduto = min.HasValue ? min.Value - 1 : 0;

            PedidoProdutoInformation pedidoProduto = new PedidoProdutoInformation { IDPedidoProduto = idPedidoProduto };
            pedidoProduto.CodigoAliquota = produto.CodigoAliquota;
            pedidoProduto.ValorUnitario = produto.ValorUnitario;
            if (produto.UtilizarBalanca == true && BalancaServices.TemBalanca)
            {
                quantidade = await BalancaServices.LerBalancaLoop();
                if (quantidade == 0)
                    return null;
            }

            pedidoProduto.Quantidade = quantidade;
            pedidoProduto.Cancelado = false;
            pedidoProduto.Usuario = new UsuarioInformation() { IDUsuario = idUser };
            pedidoProduto.PDV = new PDVInformation() { IDPDV = idPDV };

            pedidoProduto.Produto = produto;

            ListaPedidoProduto.Add(pedidoProduto);

            VisualizarProdutos();

            return pedidoProduto;
        }

        public void VisualizarProdutos()
        {
            dgvItensSelecionados.Rows.Clear();
            Object[] row;

            var produtos = ListaPedidoProduto.Where(p => p.Status != StatusModel.Excluido);
            var novos = produtos.Sum(p => p.ValorTotal);

            decimal adicionais = 0;
            int qtdAdicionais = 0;
            if (novos != 0)
            {
                foreach (var prods in produtos.Where(p => p.ListaModificacao != null))
                {
                    foreach (var adds in prods.ListaModificacao.Where(p => p.ValorUnitario != null))
                    {
                        adicionais += (decimal)adds.ValorUnitario;
                        qtdAdicionais++;
                    }
                }

            }
            decimal novosMaisAdicionais = novos + adicionais;
            decimal valorTaxa = novosMaisAdicionais / taxaServico;
            decimal novosAdicionaisTaxa = novosMaisAdicionais + valorTaxa;

            lblValorServico.Text = $"R$ {valorTaxa.ToString("N2")} ({taxaServico}%)";
            lblValorProdutos.Text = $"R$ {novosMaisAdicionais.ToString("N2")} ({produtos.Count() + qtdAdicionais} itens)";
            lblTotalNovos.Text = "R$ " + (novosMaisAdicionais).ToString("N2");

            //novos *= 1 + taxaServico / 100;
            if (retornouCredito)
                lblValorTotal.Text = "R$ " + (ValorPedido - novosAdicionaisTaxa).ToString("N2");
            else
                lblValorTotal.Text = "R$ " + (ValorPedido + novosAdicionaisTaxa).ToString("N2");

            foreach (var item in produtos)
            {
                row = new Object[]
                {
                    null,
                    null,
                    true,
                    item.Produto.Nome + System.Environment.NewLine + item.Notas,
                    item.Quantidade,
                    item.ValorUnitario.Value.ToString("#,##0.00"),
                    (item.ValorTotal).ToString("#,##0.00"),
                    item.IDPedidoProduto,
                    item.Viagem==true
                };

                dgvItensSelecionados.Rows.Add(row);

                if (item.ListaModificacao != null)
                {
                    foreach (var modificacao in item.ListaModificacao)
                    {
                        row = new Object[]
                        {
                            null,
                            null,
                            false,
                            "- " + modificacao.Produto.Nome,
                            modificacao.Quantidade.Value,
                            modificacao.ValorUnitario.Value.ToString("#,##0.00"),
                            (item.Quantidade.Value * modificacao.ValorTotal).ToString("#,##0.00"),
                            modificacao.IDPedidoProduto,
                            false
                        };

                        dgvItensSelecionados.Rows.Add(row);
                        dgvItensSelecionados.Rows[dgvItensSelecionados.Rows.Count - 1].Cells[0].Value = Properties.Resources.semImagem;
                        dgvItensSelecionados.Rows[dgvItensSelecionados.Rows.Count - 1].Cells[1].Value = Properties.Resources.semImagem;
                    }
                }
            }
        }

        public void CarregarPedido(ETipoPedido tipoPedido, String guidIdentificacao, int idFrmUser, int idFrmPdv)
        {

            GUIDIdentificacao_selecionado = guidIdentificacao;
            tipoPedido_selecionado = tipoPedido;
            idUser = idFrmUser;
            idPDV = idFrmPdv;

            PedidoInformation pedido = new PedidoInformation();
            MesaInformation mesa;
            ComandaInformation comanda;

            pedido = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);

            if (tipoPedido_selecionado == ETipoPedido.Mesa)
            {
                mesa = Mesa.CarregarPorGUID(GUIDIdentificacao_selecionado);
                lblIdentificacao.Text = "MESA " + mesa.Numero;
                taxaServico = TipoPedido.RetornarTaxaServico(tipoPedido_selecionado);
            }
            else if (tipoPedido_selecionado == ETipoPedido.Comanda)
            {
                comanda = Comanda.CarregarPorGUID(GUIDIdentificacao_selecionado);
                lblIdentificacao.Text = "COMANDA " + comanda.Numero;
                if (pedido.ValorConsumacaoMinima > 0 && ConfiguracoesSistema.Valores.ComandaComCredito)
                    // Quando tem comando com credito e consumação mínina fica confuso os calculos
                    taxaServico = 0;
                else
                    // TODO: Obter vaor da taxa de serviço da entrada
                    taxaServico = TipoPedido.RetornarTaxaServico(tipoPedido_selecionado);
            }

            if (pedido.IDPedido != null)
            {
                if (pedido.Cliente != null && pedido.Cliente.NomeCompleto != "")
                    lblCliente.Text = pedido.Cliente.NomeCompleto;
                else
                    lblCliente.Text = "";

                //servico = pedido.ValorTotalProdutos * taxaServico / 100;
                //lblValorProdutos.Text = " R$ " + pedido.ValorTotalProdutos.ToString("#,##0.00") + " (" + pedido.ListaProduto.Where(l => l.Cancelado == false).Sum(l => l.Quantidade).Value.ToString("#,##0") + " itens)";
                //lblValorServico.Text = " R$ " + servico.ToString("#,##0.00") + " (" + taxaServico + "%)";
                //lblValorTotal.Text = " R$ " + (pedido.ValorTotalProdutos + servico).ToString("#,##0.00");

                ValorPedido = Pedido.ValorTotalOuCredito(pedido, out retornouCredito);

                lblValorProdutos.Text = "R$ " + pedido.ValorTotalProdutos.ToString("#,##0.00");
                lblValorServico.Text = "R$ " + pedido.ValorServicoTemp.ToString("#,##0.00") + " (" + pedido.TaxaServicoPadrao + "%)";
                lblValorTotal.Text = "R$ " + ValorPedido.ToString("#,##0.00");
                lblValorTotal.ForeColor = retornouCredito ? Color.Green : Color.Black;

            }
            else
            {
                lblCliente.Text = "";
                lblValorProdutos.Text = " R$ 0,00 (0 itens)";
                lblValorServico.Text = " R$ 0,00 (" + taxaServico + "%)";
                lblValorTotal.Text = " R$ 0,00 ";
                ValorPedido = 0;
            }

            dgvItensSelecionados.Columns[8].Visible = false;
            ListaPedidoProduto = new List<PedidoProdutoInformation>();
            VisualizarProdutos();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            Finalizar(sender, e);
        }

        public void Finalizar(object sender, EventArgs e)
        {
            Enabled = false;
            PedidoInformation pedido = null;
            try
            {
                if (ListaPedidoProduto.Count() > 0)
                {
                    bool gerarOrdemProducao = tipoPedido_selecionado != ETipoPedido.Balcao;

                    var novosProdutos = ListaPedidoProduto.Where(pp => pp.Status != StatusModel.Excluido).ToList();
                    if (ConfiguracoesSistema.Valores.ValidarPedidoModificacaoInvalido)
                    {
                        var modificacaoesInvalidas = BLL.PedidoProduto.ValidarModificacoes(novosProdutos);
                        if (!modificacaoesInvalidas.Valido)
                        {
                            Enabled = true;
                            Refresh();
                            string msg = BLL.PedidoProduto.TextoModificacoesInvalidas(modificacaoesInvalidas);

                            var mboxButtons = MessageBoxButtons.OK;
                            if (Convert.ToBoolean(ConfiguracoesSistema.Valores.PermitirPedidoModificacaoInvalido))
                            {
                                mboxButtons = MessageBoxButtons.YesNo;
                                msg += "\r\n\r\nConfirma mesmo assim?";
                            }

                            var resp = MessageBox.Show(msg, "Quantidade de modificações inválida!", mboxButtons, MessageBoxIcon.Information);
                            if (resp != DialogResult.Yes)
                                return;
                        }
                    }

                    // o ID da área de produção não é 'gravado' então não adianta selecionar antes
                    if (gerarOrdemProducao == true && !OrdemProducaoServices.SelecionarOrdemProducao(novosProdutos))
                        return;

                    pedido = Pedido.AdicionarProduto(tipoPedido_selecionado, GUIDIdentificacao_selecionado, idUser, idPDV, novosProdutos);
                    VisualizarProdutos();

                    try
                    {
                        if (gerarOrdemProducao == true)
                            OrdemProducaoServices.GerarOrdemProducaoEscolhida(novosProdutos);

                        if (OrdemProducaoServices.ImprimirViaExpedicao == "PEDIDO")
                            OrdemProducaoServices.GerarViaExpedicao(pedido.IDPedido.Value, OrdemProducaoServices.IDAreaViaExpedicao);

                    }
                    catch (Exception ex)
                    {
                        Logs.ErroBox(CodigoErro.A200, ex);
                    }
                }
            }
            catch (ExceptionPDV ex) when (ex.CodigoErro == CodigoErro.AA20)
            {
                MessageBox.Show(ex.Message, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E100, ex);
            }
            finally
            {
                Enabled = true;
            }

            onFinalizar?.Invoke(this, null);

            if (PedidoFinalizado(pedido))
                LimparControle();
        }

        public void LimparControle()
        {
            lblIdentificacao.Text = string.Empty;
            lblCliente.Text = "Cliente";
            lblValorProdutos.Text = " R$ 0,00 (0 itens)";
            lblValorServico.Text = " R$ 0,00 (0%)";
            lblValorTotal.Text = " R$ 0,00 ";

            ListaPedidoProduto = new List<PedidoProdutoInformation>();
            VisualizarProdutos();
        }

        private void dgvItensSelecionados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (Convert.ToBoolean(dgvItensSelecionados.Rows[e.RowIndex].Cells[2].Value) == false)
                    return;

                int idPedProd = Convert.ToInt32(dgvItensSelecionados.Rows[e.RowIndex].Cells["IDPedidoProduto"].Value);
                if (e.ColumnIndex == 0)
                {
                    var pedidoProduto = ListaPedidoProduto.First(pp => pp.IDPedidoProduto == idPedProd);

                    ListaPedidoProduto = ListaPedidoProduto.Where(pp => pp.PedidoProdutoPai == null || pp.PedidoProdutoPai.IDPedidoProduto != pedidoProduto.IDPedidoProduto.Value).ToList();
                    ListaPedidoProduto = ListaPedidoProduto.Where(pp => pp.IDPedidoProduto != pedidoProduto.IDPedidoProduto).ToList();

                    VisualizarProdutos();
                }
                else if (e.ColumnIndex == 1)
                {

                    var pedidoProduto = ListaPedidoProduto.First(pp => pp.IDPedidoProduto == idPedProd);
                    if (pedidoProduto.Produto.AssistenteModificacoes == true)
                    {
                        int? minId = ListaPedidoProduto.Where(l => l.IDPedidoProduto <= 0).Min(l => l.IDPedidoProduto);
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
                    else
                    {
                        new frmProdutoModificacao(ref pedidoProduto, idUser, idPDV).ShowDialog();
                    }
                    VisualizarProdutos();
                }
                else if (e.ColumnIndex == 8)
                {
                    var pedidoProduto = ListaPedidoProduto.First(pp => pp.IDPedidoProduto == idPedProd);
                    pedidoProduto.Viagem = pedidoProduto.Viagem != true;
                    dgvItensSelecionados.Rows[e.RowIndex].Cells[8].Value = pedidoProduto.Viagem;
                }
            }
        }

        private void dgvItensSelecionados_DoubleClick(object sender, EventArgs e)
        {
            if (AC.Usuario == null)
                return;

            // TODO: Criar como evento
            if (ListaPedidoProduto.Count > 0 && ListaPedidoProduto[0].Pedido?.IDPedido > 0)
            {
                onDetalhes?.Invoke(GUIDIdentificacao_selecionado, null);
                CarregarPedido(tipoPedido_selecionado, GUIDIdentificacao_selecionado, idUser, idPDV);
            }
        }

        private void btnViagem_Click(object sender, EventArgs e)
        {
            dgvItensSelecionados.Columns[8].Visible = !dgvItensSelecionados.Columns[8].Visible;
        }
    }
}
