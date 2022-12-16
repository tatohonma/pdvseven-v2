using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.BLL.Services;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.Pagamento;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPedidos : Form
    {
        frmPrincipal FormPrincipal;
        UserControl selecionado;
        public System.Windows.Media.MediaPlayer Player { get; private set; }

        public static ETipoPedido tipoPedido_selecionado { get; set; }
        public static string GUIDIdentificacao_selecionado { get; set; }
        public static int? NumeroComanda { get; set; }

        private bool LoadComplete = false;

        internal static bool _reabrirPedidos = false;
        internal static bool ReabrirPedidos
        {
            get
            {
                return _reabrirPedidos;
            }
            set
            {
                if (ConfiguracoesCaixa.Valores.TipoPedidoPadrao == 40)
                    _reabrirPedidos = value;
                else
                    _reabrirPedidos = false;
            }
        }

        public frmPedidos(frmPrincipal frm)
        {
            FormPrincipal = frm;
            InitializeComponent();
        }

        private void frmPedidos_Load(object sender, EventArgs e)
        {

            GA.Post(this);
            try
            {
                // ATENÇÃO: O SoundPlayer padrão deixa a aplicação travada!
                Player = new System.Windows.Media.MediaPlayer();
                string path = Assembly.GetExecutingAssembly().Location;
                path = path.Substring(0, path.LastIndexOf(@"\"));
                Player.Open(new Uri(Path.Combine(path, "Telefone.wav")));
            }
            catch (Exception)
            {
            }

            clientesMenuSaldos.Visible = clientesMenuCadastro.Visible = frmPrincipal.ContaCliente;

            informarPerdaToolStripMenuItem.Visible = BLL.PDV.PossuiEstoque();

            lblTituloForm.Text += " - " + AC.PDV.Nome;

            string spWidth = ConfiguracaoBD.ValorOuPadrao(EConfig._PedidoSpliterSize, ETipoPDV.CAIXA, AC.PDV.IDPDV.Value);
            if (!string.IsNullOrEmpty(spWidth) && Int32.TryParse(spWidth, out int nspWidth))
                spContainer.SplitterDistance = nspWidth;

            ConfiguracoesDeliveryOnline ConfigDO = new ConfiguracoesDeliveryOnline();

            if (BLL.PDV.PossuiIFOOD() && ConfiguracoesCaixa.Valores.NotificarDelivery != "NAO")
            {
                tmrDelivery.Enabled = ConfiguracoesCaixa.Valores.ConfirmarDelivery;

                iFoodHabilitado.Checked = ConfiguracaoBD.ValorOuPadrao(EConfig.HabilitarIFood, ETipoPDV.IFOOD) == "1";
                iFoodAprovacao.Checked = ConfiguracaoBD.ValorOuPadrao(EConfig.AprovarIFood, ETipoPDV.IFOOD) == "1";
            }
            else if (ConfigDO.IntegracaoDeliveryOnline == true)
            {
                tmrDelivery.Enabled = ConfiguracoesCaixa.Valores.ConfirmarDelivery;
            }
            else
            {
                integracaoMenu.Visible = false;
            }                      

            CarregarMenuTipoPedido();

            listaPedidoMesa1.PedidoSelecionado += new Controles.ListaPedidoMesa.PedidoSelecionadoEventHandler(ListaPedidoMesa_PedidoSelecionado);

            listaPedidoComanda1.PedidoSelecionado += new Controles.ListaPedidoComanda.PedidoSelecionadoEventHandler(ListaPedidoComanda_PedidoSelecionado);
            listaPedidoComanda1.AdicionarCreditos += guid => AdicionarCreditos(guid);

            listaPedidoComandaSemCheckin1.PedidoSelecionado += new Controles.ListaPedidoComandaSemCheckin.PedidoSelecionadoEventHandler(listaPedidoComandaSemCheckin_PedidoSelecionado);

            listaPedidoEntrega1.PedidoSelecionado += new Controles.ListaPedidoEntrega.PedidoSelecionadoEventHandler(ListaPedidoEntrega_PedidoSelecionado);


            LoadComplete = true;
        }

        private void CarregarMenuTipoPedido()
        {
            int totalBotoes = 0;
            ETipoPedido tipoPedido_padrao = (ETipoPedido)ConfiguracoesCaixa.Valores.TipoPedidoPadrao;

            if (rdbMesas.Visible = ConfiguracoesCaixa.Valores.TipoPedidoMesa)
                totalBotoes++;
            else if (tipoPedido_padrao == ETipoPedido.Mesa)
                tipoPedido_padrao = ETipoPedido.Comanda; // Aponta para o proximo

            if (rdbComandas.Visible = ConfiguracoesCaixa.Valores.TipoPedidoComanda)
                totalBotoes++;
            else if (tipoPedido_padrao == ETipoPedido.Comanda)
                tipoPedido_padrao = ETipoPedido.Delivery;

            if (rdbEntrega.Visible = ConfiguracoesCaixa.Valores.TipoPedidoEntrega)
                totalBotoes++;
            else if (tipoPedido_padrao == ETipoPedido.Delivery)
                tipoPedido_padrao = ETipoPedido.Retirada;

            if (rdbRetirada.Visible = ConfiguracoesCaixa.Valores.TipoPedidoRetirada)
                totalBotoes++;
            else if (tipoPedido_padrao == ETipoPedido.Retirada)
                tipoPedido_padrao = ETipoPedido.Balcao;

            if (btnBalcao.Visible = ConfiguracoesCaixa.Valores.TipoPedidoBalcao)
                totalBotoes++;

            if (totalBotoes == 0)
            {
                MessageBox.Show("Nenhum tipo de pedido habilitado nas configurações");
                return;
            }

            int w = tbMenuTop.Width / totalBotoes;
            btnBalcao.Width = rdbComandas.Width = rdbMesas.Width = rdbEntrega.Width = rdbRetirada.Width = w - 7;

            if (tipoPedido_padrao == ETipoPedido.Balcao)
            {
                if (rdbMesas.Visible)
                    AlterarListaTipoPedido(ETipoPedido.Mesa);
                else if (rdbComandas.Visible)
                    AlterarListaTipoPedido(ETipoPedido.Comanda);
                else if (rdbEntrega.Visible)
                    AlterarListaTipoPedido(ETipoPedido.Delivery);
                else if (rdbRetirada.Visible)
                    AlterarListaTipoPedido(ETipoPedido.Retirada);
                else
                {
                    listaPedidoMesa1.Visible = false;
                    listaPedidoComanda1.Visible = false;
                    listaPedidoEntrega1.Visible = false;
                    listaPedidoComandaSemCheckin1.Visible = false;
                }

                ReabrirPedidos = true;
                btnBalcao_Click(null, null);
            }
            else
            {
                AlterarListaTipoPedido(tipoPedido_padrao);
            }
        }

        private void AlterarListaTipoPedido(ETipoPedido idTipoPedido)
        {
            tipoPedido_selecionado = idTipoPedido;
            GUIDIdentificacao_selecionado = null;
            btnAdicionarCreditos.Visible = false;
            listaPedidoMesa1.Visible = false;
            listaPedidoComanda1.Visible = false;
            listaPedidoEntrega1.Visible = false;
            listaPedidoComandaSemCheckin1.Visible = false;

            switch (tipoPedido_selecionado)
            {
                case ETipoPedido.Mesa:
                    rdbMesas.Checked = true;
                    mesasToolStripMenuItem.Visible = true;
                    comandasToolStripMenuItem.Visible = false;
                    listaPedidoMesa1.GUIDIdentificacao_selecionado = null;
                    listaPedidoMesa1.Visible = true;
                    selecionado = listaPedidoMesa1;
                    break;
                case ETipoPedido.Comanda:
                    btnAdicionarCreditos.Visible = ConfiguracoesSistema.Valores.ComandaComCredito; ;
                    mesasToolStripMenuItem.Visible = false;
                    comandasToolStripMenuItem.Visible = true;
                    rdbComandas.Checked = true;
                    listaPedidoComanda1.GUIDIdentificacao_selecionado = null;
                    if (ConfiguracoesSistema.Valores.ComandaComCheckin)
                    {
                        selecionado = listaPedidoComanda1;
                        listaPedidoComanda1.Visible = true;
                    }
                    else
                    {
                        selecionado = listaPedidoComandaSemCheckin1;
                        listaPedidoComandaSemCheckin1.Visible = true;
                    }
                    break;
                case ETipoPedido.Delivery:
                    rdbEntrega.Checked = true;
                    listaPedidoEntrega1.GUIDIdentificacao_selecionado = null;
                    listaPedidoEntrega1.Visible = true;
                    selecionado = listaPedidoEntrega1;
                    break;
                case ETipoPedido.Retirada:
                    rdbRetirada.Checked = true;

                    break;
            }
            AtualizarListaPedidos(true);

            if (tipoPedido_selecionado != ETipoPedido.Balcao)
                GA.PostEvento("Pedidos: " + tipoPedido_selecionado);
        }

        protected void ListaPedidoComanda_PedidoSelecionado(object sender, EventArgs e)
        {
            GUIDIdentificacao_selecionado = listaPedidoComanda1.GUIDIdentificacao_selecionado;
            NumeroComanda = null;
            AtualizarListaPedidos();
        }

        protected void ListaPedidoMesa_PedidoSelecionado(object sender, EventArgs e)
        {
            GUIDIdentificacao_selecionado = listaPedidoMesa1.GUIDIdentificacao_selecionado;
            NumeroComanda = null;
            AtualizarListaPedidos();
        }

        protected void ListaPedidoEntrega_PedidoSelecionado(object sender, EventArgs e)
        {
            GUIDIdentificacao_selecionado = listaPedidoEntrega1.GUIDIdentificacao_selecionado;
            NumeroComanda = null;
            AtualizarListaPedidos();
        }

        protected void listaPedidoComandaSemCheckin_PedidoSelecionado(object sender, EventArgs e)
        {
            GUIDIdentificacao_selecionado = listaPedidoComandaSemCheckin1.GUIDIdentificacao_selecionado;
            NumeroComanda = listaPedidoComandaSemCheckin1.NumeroComanda;
            AtualizarListaPedidos();
        }

        public void AtualizarListaPedidos(bool clear = false)
        {
            switch (tipoPedido_selecionado)
            {
                case ETipoPedido.Mesa:
                    listaPedidoMesa1.AtualizarLista();
                    break;

                case ETipoPedido.Comanda:
                    if (ConfiguracoesSistema.Valores.ComandaComCheckin)
                        listaPedidoComanda1.AtualizarLista(clear);
                    else
                        listaPedidoComandaSemCheckin1.AtualizarLista();
                    break;

                case ETipoPedido.Delivery:
                    listaPedidoEntrega1.AtualizarLista();
                    break;
            }

            AtualizarListaProdutos();

            selecionado?.Focus();
        }

        private void frmPedidos_FormClosing(object sender, FormClosingEventArgs e)
        {
            Player?.Stop();

            FormPrincipal.Enabled = true;
            FormPrincipal.AtualizarStatus();
            FormPrincipal.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AtualizarListaPedidos();
        }

        private void btnImprimirConta_Click(object sender, EventArgs e)
        {
            if (GUIDIdentificacao_selecionado != null)
            {
                var pedido = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);
                if (pedido.IDPedido != null)
                    ImprimirPreConta(pedido, true);
            }
            else
                MessageBox.Show("Selecione uma mesa/comanda", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void ImprimirPreConta(PedidoInformation pedido, bool liberarOpcoes)
        {
            Pedido.AdicionarProdutoServico(pedido, true, AC.PDV, AC.Usuario);
            Pedido.AdicionarProdutoConsumacaoMinima(pedido, AC.PDV, AC.Usuario);

            using (var frm = new frmImprimirConta(pedido))
            {
                frm.Opcoes = liberarOpcoes;
                if (!liberarOpcoes || frm.ShowDialog() == DialogResult.OK)
                {
                    switch (ConfiguracoesCaixa.Valores.GerenciadorImpressao)
                    {
                        case ETipoGerenciadorImpressao.ACBr:
                        case ETipoGerenciadorImpressao.ECFBemafii:
                            frmPrincipal.Impressora1.GerarCupom(pedido, false);
                            break;
                        case ETipoGerenciadorImpressao.ImpressoraWindows:
                        case ETipoGerenciadorImpressao.SAT:
                            ContaServices.ImprimirConta(ConfiguracoesCaixa.Valores.ModeloImpressora, pedido);
                            break;
                    }
                }
            }
        }

        private void btnPagamento_Click(object sender, EventArgs e)
        {
            PedidoInformation pedido;
            if (GUIDIdentificacao_selecionado != null)
            {
                pedido = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);

                if (pedido?.IDPedido != null)
                {
                    // Verificar se o pedido é Delivery e Retirada é false
                    if (tipoPedido_selecionado == ETipoPedido.Delivery)
                    {
                        using (var frmDelivery = frmNovoDelivery.EnviarOuFinalizarPedidoDelivery(GUIDIdentificacao_selecionado))
                        {
                            frmDelivery.ShowDialog();
                        }
                    }
                    else
                    {
                        bool parcial = false;
                        if (sender is ToolStripMenuItem ctrl)
                        {
                            parcial = ctrl.Name == pagamentoParcialComandaToolStripMenuItem.Name ||
                                      ctrl.Name == pagamentoParcialMesaToolStripMenuItem.Name;
                        }

                        NormalOuTouch.FechaPagamento(pedido.IDPedido.Value, parcial);
                    }
                }

                AtualizarListaPedidos();
            }
            else
            {
                MessageBox.Show("Selecione um pedido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnAdicionarProduto_Click(object sender, EventArgs e)
        {
            if (GUIDIdentificacao_selecionado != null)
            {
                if (tipoPedido_selecionado == ETipoPedido.Delivery)
                {
                    using (var form = frmNovoDelivery.EditarPedidoDelivery(GUIDIdentificacao_selecionado))
                    {
                        form.AdicionarProdutos = true;
                        form.ShowDialog();
                    }
                }
                else
                    NormalOuTouch.AdicionaProdutos(tipoPedido_selecionado, GUIDIdentificacao_selecionado);

                AtualizarListaPedidos();
            }
            else
            {
                MessageBox.Show("Selecione uma pedido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void AtualizarListaProdutos()
        {
            dgvItens.Rows.Clear();

            var pedido = new PedidoInformation();
            MesaInformation mesa;
            ComandaInformation comanda;
            Object[] row;

            if (tipoPedido_selecionado == ETipoPedido.Mesa)
            {
                mesa = Mesa.CarregarPorGUID(GUIDIdentificacao_selecionado);
                lblIdentificacao.Text = "MESA " + mesa.Numero;
            }
            else if (tipoPedido_selecionado == ETipoPedido.Comanda)
            {
                comanda = Comanda.CarregarPorGUID(GUIDIdentificacao_selecionado);
                lblIdentificacao.Text = "COMANDA " + comanda.Numero;
            }
            else if (tipoPedido_selecionado == ETipoPedido.Delivery)
            {
                lblIdentificacao.Text = "DELIVERY";
            }

            if (GUIDIdentificacao_selecionado != null)
                pedido = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);

            if (pedido != null && pedido.IDPedido != null)
            {
                if (pedido.IDPedido != null && pedido.Cliente != null && pedido.Cliente.NomeCompleto != "")
                    lblCliente.Text = pedido.Cliente.NomeCompleto;

                if (tipoPedido_selecionado == ETipoPedido.Delivery)
                {
                    if (pedido.OrigemPedido != null && pedido.OrigemPedido.IDOrigemPedido == (int)EOrigemPedido.ifood)
                    {
                        TagInformation tagDisplayId = BLL.Tag.Carregar(pedido.GUIDIdentificacao, "ifood-displayId");

                        lblIdentificacao.Text = "IFOOD " + tagDisplayId.Valor;
                    }
                    else if (pedido.OrigemPedido != null && pedido.OrigemPedido.IDOrigemPedido == (int)EOrigemPedido.deliveryOnline)
                    {
                        TagInformation tagId = BLL.Tag.Carregar(pedido.GUIDIdentificacao, "DeliveryOnline-order_id");

                        lblIdentificacao.Text = "Delivery Online " + tagId.Valor;
                    }
                    else
                    {
                        lblIdentificacao.Text = "DELIVERY " + pedido.IDPedido;
                    }
                }

                if (pedido.ValorConsumacaoMinima > 0)
                    lblValorConsumacaoMinima.Text = "R$ " + pedido.ValorConsumacaoMinima.Value.ToString("#,##0.00");
                else
                    lblValorConsumacaoMinima.Text = "R$ 0,00";

                if (pedido.TipoPedido.TipoPedido == ETipoPedido.Delivery)
                {
                    lblTaxaEntregaTexto.Visible = true;
                    lblTaxaEntrega.Visible = true;
                    if (pedido.ValorEntrega > 0)
                        lblTaxaEntrega.Text = $"R$ {pedido.ValorEntrega.Value.ToString("#,##0.00")}";
                    else
                        lblTaxaEntrega.Text = "R$ 0,00";
                }
                else
                {
                    lblTaxaEntregaTexto.Visible = false;
                    lblTaxaEntrega.Visible = false;
                }

                lblValorProdutos.Text = "R$ " + pedido.ValorTotalProdutos.ToString("#,##0.00");
                lblValorServico.Text = "R$ " + pedido.ValorServicoTemp.ToString("#,##0.00") + " (" + pedido.TaxaServicoPadrao + "%)";
                lblValorTotal.Text = "R$ " + Pedido.ValorTotalOuCredito(pedido, out bool retornouCredito).ToString("#,##0.00");
                if (retornouCredito)
                {
                    lblValorTotal.Text = "Crédito: " + lblValorTotal.Text;
                    lblInformacaoAdicional.Text = "";
                }
                else
                {
                    lblValorTotal.Text = "Total: " + lblValorTotal.Text;
                    var pago = pedido.ListaPagamento.Where(p => p.Excluido != true).Sum(p => p.Valor) ?? 0;
                    if (pago > 0)
                        lblInformacaoAdicional.Text = "Pago: R$ " + pago.ToString("#,##0.00");
                    else
                        lblInformacaoAdicional.Text = "";
                }
                lblValorTotal.ForeColor = retornouCredito ? Color.Green : Color.Black;

                var tipoEntrada = string.Empty;
                if (pedido.TipoEntrada?.IDTipoEntrada.HasValue == true)
                    tipoEntrada = TipoEntrada.Carregar(pedido.TipoEntrada.IDTipoEntrada.Value).Nome;

                var lista = PedidoProduto.ListarPorPedidoAgrupado(pedido.IDPedido.Value);
                foreach (var item in lista)
                {
                    row = new Object[]
                    {
                         ObterNome(item, tipoEntrada)
                        ,item.Quantidade
                        //,item.ValorUnitario.Value.ToString("#,##0.00")
                        //,(item.Quantidade.Value * item.ValorUnitario.Value).ToString("#,##0.00")
                    };

                    dgvItens.Rows.Add(row);
                }
            }
            else
            {
                lblCliente.Text = "";
                lblValorProdutos.Text = "R$ 0,00";
                lblValorServico.Text = "R$ 0,00 (0%)";
                lblValorTotal.Text = "R$ 0,00 ";
                lblValorConsumacaoMinima.Text = "R$ 0,00";
                lblInformacaoAdicional.Text = "";

                pedido.ListaProduto = new List<PedidoProdutoInformation>();
            }

            dgvItens.ClearSelection();
        }

        private static string ObterNome(PedidoProdutoInformation item, string tipoEntrada)
        {
            if (item.Produto.IDProduto == ProdutoInformation.IDProdutoEntrada || item.Produto.IDProduto == ProdutoInformation.IDProdutoEntracaCM)
            {
                if (string.IsNullOrWhiteSpace(tipoEntrada) == false)
                    return $"{item.Produto.Nome} ({tipoEntrada})";
            }
            return item.Produto.Nome;
        }

        private void btnTransferirProdutos_Click(object sender, EventArgs e)
        {
            if (GUIDIdentificacao_selecionado != null)
            {
                if (tipoPedido_selecionado == ETipoPedido.Delivery)
                {
                    MessageBox.Show("Não é possível transferir itens do Delivery", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    using (frmTransferirProdutos form = new frmTransferirProdutos(tipoPedido_selecionado, GUIDIdentificacao_selecionado))
                    {
                        if (form.DialogResult == DialogResult.Cancel)
                            return;

                        form.ShowDialog();

                        AtualizarListaPedidos();
                    }
                }
            }
        }

        private void rdbMesas_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbMesas.Checked == true)
                AlterarListaTipoPedido(ETipoPedido.Mesa);

            AtualizarListaProdutos();
        }

        private void rdbComandas_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbComandas.Checked == true)
                AlterarListaTipoPedido(ETipoPedido.Comanda);

            AtualizarListaProdutos();
        }

        private void rdbEntrega_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEntrega.Checked == true)
                AlterarListaTipoPedido(ETipoPedido.Delivery);

            AtualizarListaProdutos();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void alterarTipoDeEntradaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
            {
                using (frmAlterarEntrada frm2 = new frmAlterarEntrada(usuario.IDUsuario.Value))
                {
                    frm2.ShowDialog();
                    AtualizarListaProdutos();
                }
            }
        }

        private void bloquearLiberarComandaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAlterarStatusComanda frm = new frmAlterarStatusComanda())
            {
                frm.ShowDialog();
                AtualizarListaPedidos(true);
            }
        }

        private void juntarComandasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAgruparComanda form = new frmAgruparComanda(this.FormPrincipal))
            {
                form.ShowDialog();
            }
        }

        private void dgvItens_DoubleClick(object sender, EventArgs e)
        {
            if (GUIDIdentificacao_selecionado != null)
            {
                if (tipoPedido_selecionado == ETipoPedido.Delivery)
                {
                    using (var frmDelivery = frmNovoDelivery.EditarPedidoDelivery(GUIDIdentificacao_selecionado))
                    {
                        frmDelivery.AdicionarProdutos = true;
                        frmDelivery.ShowDialog();
                    }
                }
                else
                {
                    using (frmDetalhesPedidoProduto form = new frmDetalhesPedidoProduto(FormPrincipal, GUIDIdentificacao_selecionado))
                    {
                        form.ShowDialog();
                    }
                }
                AtualizarListaPedidos();
            }
        }

        private void cancelarSolicitaçãoDeContaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mesa = Mesa.CarregarPorGUID(GUIDIdentificacao_selecionado);

            if (mesa.IDMesa != null && mesa.StatusMesa.IDStatusMesa == (int)EStatusMesa.ContaSolicitada)
                Mesa.AlterarStatus(GUIDIdentificacao_selecionado, EStatusMesa.EmAtendimento);
        }

        private void btnBalcao_Click(object sender, EventArgs e)
        {
            GA.PostEvento("Pedidos: " + ETipoPedido.Balcao);
            NormalOuTouch.AdicionaProdutos(ETipoPedido.Balcao, null);
            if (ReabrirPedidos)
                btnBalcao_Click(sender, e);
        }

        private void cancelarPedidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
            {
                using (var frmCancelarSat = new frmCancelarFiscal(usuario.IDUsuario.Value, AC.PDV.IDPDV.Value))
                {
                    frmCancelarSat.ShowDialog();
                }
            }
        }

        private void informarPerdaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
            {
                using (var frmSaidaAvulsa1 = new frmSaidaAvulsa())
                {
                    frmSaidaAvulsa1.ShowDialog();
                }
            }
        }

        private void reimprimirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
            {
                using (var frmReimprimirSAT = new frmReimprimirFiscal())
                {
                    frmReimprimirSAT.ShowDialog();
                }
            }
        }

        private void abrirGavetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, false, true, false, out UsuarioInformation usuario) == DialogResult.OK)
                Impressora.ImpressoraHelper.AbrirGaveta(ConfiguracoesCaixa.Valores.ModeloImpressora);
        }

        private void cancelarSolicitaçãoDeContaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var comanda = Comanda.CarregarPorGUID(GUIDIdentificacao_selecionado);

            if (comanda.IDComanda != null && comanda.ValorStatusComanda == EStatusComanda.ContaSolicitada)
            {
                Comanda.AlterarStatus(GUIDIdentificacao_selecionado, EStatusComanda.EmUso);
                AtualizarListaPedidos(true);
            }
        }

        private void ImprimeRecibo(string recibo)
        {
            if (string.IsNullOrEmpty(recibo))
                return;

            ImpressoraWindows.ImprimirTexto(ConfiguracoesCaixa.Valores.ModeloImpressora, true, recibo);
        }

        private void iFoodHabilitado_Click(object sender, EventArgs e)
        {
            var atual = ConfiguracaoBD.ValorOuPadrao(EConfig.HabilitarIFood, ETipoPDV.IFOOD) == "1";
            if (iFoodHabilitado.Checked != atual)
                iFoodHabilitado.Checked = atual;
            else
            {
                iFoodHabilitado.Checked = !iFoodHabilitado.Checked;
                ConfiguracaoBD.DefinirValorPadraoTipo(EConfig.HabilitarIFood, ETipoPDV.IFOOD, iFoodHabilitado.Checked ? "1" : "0");
            }
        }

        private void iFoodAprovacao_Click(object sender, EventArgs e)
        {
            var atual = ConfiguracaoBD.ValorOuPadrao(EConfig.AprovarIFood, ETipoPDV.IFOOD) == "1";
            if (iFoodAprovacao.Checked != atual)
                iFoodAprovacao.Checked = atual;
            else
            {
                iFoodAprovacao.Checked = !iFoodAprovacao.Checked;
                ConfiguracaoBD.DefinirValorPadraoTipo(EConfig.AprovarIFood, ETipoPDV.IFOOD, iFoodAprovacao.Checked ? "1" : "0");
            }

            MessageBox.Show("É necessário reiniciar o integrador para concluir essa alteração!");
        }

        private void tmrDelivery_Tick(object sender, EventArgs e)
        {
            if (Form.ActiveForm?.TopMost == true)
                return;

            var novoPedido = EF.Repositorio.Carregar<EF.Models.tbPedido>(
                p => p.IDTipoPedido == (int)ETipoPedido.Delivery
                  && (p.IDStatusPedido == (int)EStatusPedido.NaoConfirmado
                   || p.IDStatusPedido == (int)EStatusPedido.EmCancelamento));

            if (novoPedido == null)
                return;

            tmrDelivery.Stop();

            var pedido = Pedido.CarregarCompleto(novoPedido.IDPedido);
            var hash1 = Pedido.GetHash(pedido, out List<object> i1);

            if (novoPedido.IDStatusPedido == (int)EStatusPedido.NaoConfirmado)
            {
                var frm = new frmConfirmarDelivery
                {
                    Owner = this,
                    Pedido = pedido
                };
                var result = frm.ShowDialog();

                if (result != DialogResult.Cancel)
                {
                    var pedidoVerifica = Pedido.CarregarCompleto(novoPedido.IDPedido);

                    if (pedidoVerifica.StatusPedido.StatusPedido == EStatusPedido.Cancelado)
                    {
                        MessageBox.Show("ATENÇÃO! O pedido foi cancelado automaticamente");
                    }
                    else
                    {
                        var exDiff = Pedido.Compare(hash1, i1, pedidoVerifica);
                        if (exDiff != null)
                        {
                            Logs.ErroBox(CodigoErro.A900, exDiff, MessageBoxIcon.Exclamation);

                        }
                        else if (result == DialogResult.OK)
                        {
                            OrdemProducaoServices.GerarOrdemProducao(pedido.ListaProduto, false);
                            pedido.StatusPedido.StatusPedido = EStatusPedido.Aberto;
                            Pedido.Salvar(pedido);

                            if (ConfiguracoesSistema.Valores.ImprimirViaExpedicao == "NOVO")
                                OrdemProducaoServices.GerarViaExpedicao(pedido.IDPedido.Value, ConfiguracoesSistema.Valores.IDAreaViaExpedicao);
                        }
                        else if (result == DialogResult.No)
                        {
                            // Cancelamento ou Rejeição
                            var form = new frmCancelarPedido(AC.Usuario.IDUsuario.Value, pedido.GUIDIdentificacao);
                            form.ShowDialog();
                        }
                    }
                }
            }
            else if (novoPedido.IDStatusPedido == (int)EStatusPedido.EmCancelamento)
            {
                var form = new frmCancelarPedido(AC.Usuario.IDUsuario.Value, pedido.GUIDIdentificacao);
                form.ShowDialog();
            }

            tmrDelivery.Start();
        }

        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            lblModoContingencia.Text = frmPrincipal.InfoTitle;
        }

        private void clientesMenu_Click(object sender, EventArgs e)
        {
            if (!clientesMenuCadastro.Visible)
                clientesMenuCadastro_Click(sender, e);
        }

        private void clientesMenuCadastro_Click(object sender, EventArgs e)
        {
            using (var frm = new frmPesquisarCliente())
            {
                frm.Consulta = true;
                frm.ShowDialog();
            }
        }

        private void clientesMenuSaldos_Click(object sender, EventArgs e)
        {
            using (var frm = new frmExtratoCliente())
            {
                frm.ShowDialog();
            }
        }

        private void spContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!LoadComplete)
                return;

            ConfiguracaoBD.DefinirValorPadraoTipoPDV(EConfig._PedidoSpliterSize, ETipoPDV.CAIXA, AC.PDV.IDPDV.Value, spContainer.SplitterDistance.ToString());
        }

        private void btnAdicionarCreditos_Click(object sender, EventArgs e)
        {
            AdicionarCreditos(GUIDIdentificacao_selecionado);
        }

        private void AdicionarCreditos(string guid)
        {
            if (string.IsNullOrEmpty(guid) && tipoPedido_selecionado == ETipoPedido.Comanda)
            {
                MessageBox.Show("Selecione uma comanda", "VENDA DE CRÉDITO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pedido = Pedido.CarregarUltimoPedido(guid);
            if (pedido?.Cliente == null)
            {
                MessageBox.Show("Pedido não existe ou não tem cliente (Comanda com Chekin)", "VENDA DE CRÉDITO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var cliente = pedido.Cliente;

            var frmValor = new frmTecladoValor(0);
            frmValor.Text = "CRÉDITOS A ADICIONAR";

            if (frmValor.ShowDialog() != DialogResult.OK || frmValor.Valor <= 0)
                return;

            var produto = Produto.Listar(new ProdutoInformation()
            {
                TipoProduto = new TipoProdutoInformation()
                {
                    IDTipoProduto = (int)ETipoProduto.Credito,
                },
                ValorUnitario = frmValor.Valor,
            }).FirstOrDefault();

            if (produto == null)
            {
                produto = Produto.Listar(new ProdutoInformation()
                {
                    TipoProduto = new TipoProdutoInformation()
                    {
                        IDTipoProduto = (int)ETipoProduto.Credito,
                    },
                    ValorUnitario = 0,
                }).FirstOrDefault();
            }

            if (produto == null)
            {
                MessageBox.Show("Não foi encontrado um produto de crédito disponivel para realizar a venda", "VENDA DE CRÉDITO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Novo pedido!
            pedido = Pedido.NovoPedidoBalcao(cliente);
            var entradaCM = new PedidoProdutoInformation
            {
                Produto = produto,
                Quantidade = 1,
                Cancelado = false,
                ValorUnitario = frmValor.Valor,
                PDV = AC.PDV,
                Usuario = AC.Usuario,
                Pedido = pedido,
                CodigoAliquota = produto.CodigoAliquota
            };
            PedidoProduto.Salvar(entradaCM);

            var frm = NormalOuTouch.FechaPagamento(pedido.IDPedido.Value, false);
            if (frm != DialogResult.OK)
            {
                Pedido.AlterarStatus(pedido.IDPedido.Value, EStatusPedido.Cancelado);
                return;
            }
        }

        private void menuTEFcancelarTransacao_Click(object sender, EventArgs e)
        {

        }

        private void rdbRetirada_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Tipo de Pedido não implementado!");
        }
    }
}