using a7D.PDV.BLL;
using a7D.PDV.BLL.Services;
using a7D.PDV.Caixa.UI.Properties;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPedidoPagamentoTouch : FormTouch, IPedidoPagamentoForm
    {
        public PedidoInformation PedidoAtual { get; private set; }
        private Action PosExecucao { get; set; }
        public void HabilitarParcial()
        {
            TextoFechamento();
            ppControl.HabilitarParcial();
        }

        public decimal ValorPendente => ppControl.ValorPendente;

        public frmPedidoPagamentoTouch()
        {
            InitializeComponent();
        }

        public frmPedidoPagamentoTouch(Int32 idPedido, Action posExecucao = null) : this()
        {
            PosExecucao = posExecucao;
            PedidoAtual = Pedido.CarregarCompleto(idPedido);
            Saldo.AdicionaPagamentoPorSaldo(PedidoAtual);
        }

        private void frmPagamento_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            GA.Post(this);

            this.LoadLocationSize(EConfig._PedidoPagamentoLocationSize, prms =>
            {
                if (prms.Length == 5)
                {
                    ppFormas.SplitterDistance = int.Parse(prms[4]);
                }
            });

            PedidoAtual.Caixa = frmPrincipal.Caixa1;
            bool viagem = PedidoAtual.ListaProduto.Any(p => p.Viagem == true);
            chkExpedicao.Visible = chkExpedicao.Checked = viagem && ConfiguracoesCaixa.Valores.ImprimirViaExpedicao == "CONTA";

            ppProdutos.Fill(PedidoAtual.ListaProduto);

            if (PedidoAtual.TipoPedido.TipoPedido == ETipoPedido.Mesa)
            {
                var mesa = Mesa.CarregarPorGUID(PedidoAtual.GUIDIdentificacao);
                lblIdentificacao.Text = "MESA " + mesa.Numero;
            }
            else if (PedidoAtual.TipoPedido.TipoPedido == ETipoPedido.Comanda)
            {
                var comanda = Comanda.CarregarPorGUID(PedidoAtual.GUIDIdentificacao);
                lblIdentificacao.Text = "COMANDA " + comanda.Numero;
            }

            else if (PedidoAtual.TipoPedido.TipoPedido == ETipoPedido.Delivery)
            {
                lblIdentificacao.Text = "DELIVERY";
            }
            else if (PedidoAtual.TipoPedido.TipoPedido == ETipoPedido.Balcao)
            {
                lblIdentificacao.Text = "BALCÃO";
            }

            lblCodigoPedido.Text = "PEDIDO " + PedidoAtual.IDPedido.Value.ToString("000000");

            TextoFechamento();

            chkImprimir.Checked = ConfiguracoesCaixa.Valores.ImprimirCupomFiscal == "SIM";
        }

        private void TextoFechamento()
        {
            btnFecharVenda.Text = ppControl.AceitarParcial && ValorPendente > 0 ? "&PAGAR PARCIAL" : "&FECHAR";
        }

        private void frmPedidoPagamento_FormClosed(object sender, FormClosedEventArgs e)
        {
            Teclado.BlockInput(false);
        }

        private void frmPedidoPagamento_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PedidoAtual.TipoPedido.TipoPedido == ETipoPedido.Balcao
             && PedidoAtual.StatusPedido.StatusPedido == EStatusPedido.Aberto
             && PedidoAtual.ListaPagamento.Any(p => p.IDPedidoPagamento != null && p.Excluido == false))
            {
                e.Cancel = true;
                MessageBox.Show("Não é possível cancelar com pagamentos parciais\nComplete ou cancele os pagamentos");
                return;
            }

            this.SaveLocationSize(EConfig._PedidoPagamentoLocationSize, () =>
            {
                return ppFormas.SplitterDistance.ToString();
            });

            Settings.Default.Save();
        }

        private void ppForma_PagamentosAlterados(object sender, EventArgs e)
        {
            ppControl.CalcularExibir();
            TextoFechamento();
            if (ppFormas.FecharPagamento) // Foi adicionado pagamento via TEF (Gateway) e não há valor pendente e nem troco 
                Fechar();
        }

        private void ppControl_ParametrosAlterados(object sender, EventArgs e)
        {
            ppProdutos.Fill(PedidoAtual.ListaProduto);
        }

        private void btnFecharVenda_Click(object sender, EventArgs e)
        {
            Fechar();
        }

        public void Fechar()
        {
            var hashPedido = Pedido.GetHash(PedidoAtual, out List<object> itens);
            var pedidoAtual = Pedido.CarregarCompleto(PedidoAtual.IDPedido.Value);
            var exDiff = Pedido.Compare(hashPedido, itens, pedidoAtual);
            if (exDiff != null)
            {
                Logs.ErroBox(CodigoErro.A900, exDiff);
                this.Close();
                return;
            }

            int qtdCredito = PedidoAtual.ListaProduto.Where(p => p.Produto.Excluido != true && p.Produto.TipoProduto.TipoProduto == ETipoProduto.Credito).Count();
            if (qtdCredito > 0)
            {
                if (PedidoAtual.Cliente == null || PedidoAtual.Cliente.IDCliente == 0)
                {
                    Logs.ErroBox(CodigoErro.AE32);
                    return;
                }
                else if (PedidoAtual.ListaPagamento.Any(p => p.TipoPagamento.Gateway == EGateway.ContaCliente))
                {
                    Logs.ErroBox(CodigoErro.AE33);
                    return;
                }
            }

            this.Enabled = false;
            this.Refresh();

            if (ValorPendente > 0)
            {
                if (ppControl.AceitarParcial && PedidoAtual.ListaPagamento != null && PedidoAtual.ListaPagamento.Count > 0)
                {
                    foreach (var pagamentoParcial in PedidoAtual.ListaPagamento.Where(p => p.Status != StatusModel.Inalterado))
                    {
                        if (pagamentoParcial.Status == StatusModel.Excluido)
                            BLL.PedidoPagamento.Cancelar(pagamentoParcial, AC.Usuario.IDUsuario.Value);
                        else
                        {
                            pagamentoParcial.Pedido = new PedidoInformation { IDPedido = PedidoAtual.IDPedido };
                            BLL.PedidoPagamento.Salvar(pagamentoParcial, AC.Usuario.IDUsuario.Value);
                        }
                    }
                    this.Close();
                }
                else
                    MessageBox.Show("Ainda existe valor pendente. Conferir os pagamentos!");
            }
            else
            {
                PedidoAtual.DocumentoCliente = ppCliente.DocumentoCliente;

                try
                {
                    if (PedidoAtual.TipoPedido.TipoPedido == ETipoPedido.Balcao)
                    {
                        this.TopMost = false;
                        if (ConfiguracoesCaixa.Valores.PreContaVendaBalcao == "SIM")
                            frmPedidos.ImprimirPreConta(PedidoAtual, false);
                        else if (ConfiguracoesCaixa.Valores.PreContaVendaBalcao == "Perguntar") // perguntar
                        {
                            if (MessageBox.Show("Deseja imprimir a pré conta", "Imprimir?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                frmPedidos.ImprimirPreConta(PedidoAtual, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logs.ErroBox(CodigoErro.A300, ex);
                }

                if (!ppCliente.DocumentoNaNota)
                    PedidoAtual.DocumentoCliente = "";

                PedidoAtual.EmailCliente = ppCliente.EmailCliente;

                var dialog = new FecharVenda(PedidoAtual, ppCliente.DocumentoNaNota, PedidoAtual.Observacoes)
                    .NaImpressora(ConfiguracoesCaixa.Valores.ModeloImpressora)
                    .Fechar(true, chkImprimir.Checked, chkExpedicao.Checked);

                if (dialog == DialogResult.OK)
                {
                    this.DialogResult = dialog;
                    frmPedidos.GUIDIdentificacao_selecionado = null;
                    frmPedidos.ReabrirPedidos = true;
                    this.Close();
                }
            }

            this.Enabled = true;
        }
    }
}