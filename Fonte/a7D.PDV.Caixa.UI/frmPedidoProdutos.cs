using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPedidoProdutos : Form
    {
        private PedidoInformation Pedido1;
        private ETipoPedido tipoPedido_selecionado;
        private string GUIDIdentificacao_selecionado;

        private frmPedidoProdutos()
        {
            InitializeComponent();
            pedidoProduto1.Fechar = () => Close();
            pedidoProduto1.Confirmar = btnConfirmar_Click;
        }

        public frmPedidoProdutos(ETipoPedido idTipoPedido_selecionado, String guidIdentificacao_selecionado)
            : this()
        {
            tipoPedido_selecionado = idTipoPedido_selecionado;
            pedidoProduto1.TipoPedidoSelecionado = tipoPedido_selecionado;
            GUIDIdentificacao_selecionado = guidIdentificacao_selecionado;
            Pedido1 = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);
        }

        public frmPedidoProdutos(ETipoPedido idTipoPedido_selecionado)
            : this()
        {
            tipoPedido_selecionado = idTipoPedido_selecionado;
            pedidoProduto1.TipoPedidoSelecionado = tipoPedido_selecionado;
        }

        private void frmPedidoProdutos_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            frmPedidos.ReabrirPedidos = false;
            this.LoadLocationSize(EConfig._PedidoProdutoLocationSize);

            pedidoProduto1.CarregarPedidoProduto(GUIDIdentificacao_selecionado, null);
            pedidoProduto1.Focus();
            pedidoProduto1.SolicitarFoco();
        }

        private void btnConfirmar_Click()
        {
            try
            {
                this.Enabled = false;
                this.Refresh();

                bool gerarOrdemProducao = OrdemProducaoServices.UsarOrdemProducao;

                var novosProdutos = pedidoProduto1.ListaPedidoProduto.Where(pp => pp.Status != StatusModel.Excluido).ToList();
                if (ConfiguracoesSistema.Valores.ValidarPedidoModificacaoInvalido)
                {
                    var modificacaoesInvalidas = PedidoProduto.ValidarModificacoes(novosProdutos);
                    if (!modificacaoesInvalidas.Valido)
                    {
                        string msg = PedidoProduto.TextoModificacoesInvalidas(modificacaoesInvalidas);

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

                // o ID da área de produção não é 'gravado' ! aqui é outra logica diferente da tela touch!
                if (gerarOrdemProducao == true && !OrdemProducaoServices.SelecionarOrdemProducao(novosProdutos))
                    return;

                Pedido1 = Pedido.AdicionarProduto(tipoPedido_selecionado, GUIDIdentificacao_selecionado, AC.Usuario.IDUsuario.Value, AC.PDV.IDPDV.Value, novosProdutos);

                bool fechar = false;
                if (Pedido1.TipoPedido.TipoPedido == ETipoPedido.Balcao && Pedido1.StatusPedido.StatusPedido != EStatusPedido.Finalizado)
                {
                    pedidoProduto1.CarregarProdutos(PedidoProduto.ListarPorPedido(Pedido1.IDPedido.Value));

                    DialogResult result = NormalOuTouch.FechaPagamento(Pedido1.IDPedido.Value);

                    if (result != DialogResult.OK)
                    {
                        gerarOrdemProducao = false;
                        Pedido.AlterarStatus(Pedido1.IDPedido.Value, EStatusPedido.Cancelado);
                    }
                    else
                    {
                        fechar = true;
                    }

                    frmPedidos.ReabrirPedidos = fechar;
                }

                try
                {
                    if (gerarOrdemProducao)
                    {
                        OrdemProducaoServices.GerarOrdemProducaoEscolhida(novosProdutos);
                    }

                    if (OrdemProducaoServices.ImprimirViaExpedicao == "PEDIDO")
                        OrdemProducaoServices.GerarViaExpedicao(Pedido1.IDPedido.Value, OrdemProducaoServices.IDAreaViaExpedicao);
                }
                catch (Exception ex)
                {
                    Logs.ErroBox(CodigoErro.A200, ex, MessageBoxIcon.Exclamation);
                }

                if (tipoPedido_selecionado != ETipoPedido.Balcao || fechar)
                    Close();
            }
            catch (Exception ex)
            {
                BLL.Logs.ErroBox(CodigoErro.E100, ex);
            }
            finally
            {
                Enabled = true;
                Refresh();
            }
        }

        private void frmPedidoProdutos_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveLocationSize(EConfig._PedidoProdutoLocationSize);
        }
    }
}
