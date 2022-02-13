using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using System.Threading;
using a7D.PDV.SAT;
using System.Drawing.Printing;
using a7D.PDV.Autenticacao.UI;

namespace a7D.PDV.Pagamento.UI
{
    public partial class frmPedidoPagamento : Form
    {
        private Impressora.Impressora Impressora1 { get; set; }
        public List<PedidoInformation> Pedidos { get; set; }
        private ClienteInformation Cliente { get; set; }
        private TipoDescontoInformation TipoDesconto { get; set; }
        private UsuarioInformation UsuarioAutenticado { get; set; }
        private CaixaInformation Caixa1 { get; set; }
        private PDVInformation PDV1 { get; set; }

        private int idTipoPedido { get; set; }

        decimal Total { get; set; }
        decimal TotalSemAcrescimo { get; set; }
        decimal TotalSemEntradaESemServico { get; set; }
        decimal ValorPago { get; set; }
        decimal ValorPendente { get; set; }
        decimal ValorDesconto { get; set; }
        decimal Acrescimos { get; set; }
        decimal ConsumacaoMinima { get; set; }
        List<PedidoProdutoInformation> ListaProduto { get; set; }
        List<PedidoPagamentoInformation> ListaPagamentos { get; set; }
        public bool MostrarTeclado { get; set; }

        public Func<bool, bool, bool, bool> AuthFunc { private get; set; }

        public frmPedidoPagamento(Impressora.Impressora impressora, PDVInformation pdv, CaixaInformation caixa, UsuarioInformation usuarioAutenticado, int idTipoPedido, params int[] idPedidos)
        {
            Impressora1 = impressora;
            this.idTipoPedido = idTipoPedido;
            Pedidos = new List<PedidoInformation>();
            PDV1 = pdv;
            Caixa1 = caixa;
            UsuarioAutenticado = usuarioAutenticado;

            if (idPedidos == null || idPedidos.Length == 0)
                throw new ArgumentException("Nenhum pedido selecionado");

            foreach (var idPedido in idPedidos)
            {
                var pedido = Pedido.CarregarCompleto(idPedido);
                if (pedido.TipoPedido.IDTipoPedido != idTipoPedido)
                    throw new ArgumentException(string.Format("Pedido {0} é do tipo {1} (tipo esperado: {2})", idPedido, pedido.TipoPedido.IDTipoPedido.Value, this.idTipoPedido));
                pedido.ValorDesconto = 0;
                Pedidos.Add(pedido);
            }
            ListaPagamentos = new List<PedidoPagamentoInformation>();
            ValorDesconto = 0m;
            Acrescimos = 0m;
            InitializeComponent();
        }

        private void frmPagamento_Load(object sender, EventArgs e)
        {
            if (MostrarTeclado)
            {
                txtCodigoFormaPagamento.Visible = false;
                cbbFormaPagamento.Focus();
            }

            var varios = Pedidos.Count > 1;

            Total = 0m;
            switch (idTipoPedido)
            {
                case 10:
                    lblIdentificacao.Text = "MESA" + (varios ? "S " : " ");
                    break;
                case 20:
                    lblIdentificacao.Text = "COMANDA" + (varios ? "S " : " ");
                    break;
                case 30:
                    lblIdentificacao.Text = "DELIVERY";
                    break;
                case 40:
                    lblIdentificacao.Text = "BALCÃO";
                    break;
            }

            var pedidos = new List<string>();

            var mesas = new List<string>();
            var comandas = new List<string>();


            CalcularServico();
            ListarPagamento();
            foreach (var pedido in Pedidos)
            {
                PedidoPagamento.ExcluirPorPedido(pedido.IDPedido.Value);
                if (Caixa1 != null)
                    pedido.Caixa = Caixa1;

                switch (pedido.TipoPedido.IDTipoPedido.Value)
                {
                    case 10:
                        mesas.Add(Mesa.Carregar(pedido.GUIDIdentificacao).Numero.ToString());
                        break;
                    case 20:
                        comandas.Add(Comanda.CarregarPorGUID(pedido.GUIDIdentificacao).Numero.ToString());
                        break;
                }

                pedidos.Add(pedido.IDPedido.Value.ToString("000000"));


                //Total += pedido.ValorTotalProdutos + pedido.ValorServico.Value;
                TotalSemAcrescimo += pedido.ValorTotalProdutos;
                TotalSemEntradaESemServico += pedido.ValorTotalProdutosParaAcrescimo;

                txtCodigoFormaPagamento.Focus();
            }
            CalcularConsumacaoMinima();
            ListarProdutos();
            Total = TotalSemAcrescimo + Pedidos.Sum(p => p.ValorServico.Value);

            CarregarTipoPagamento();
            AtualizarPainel();

            if (mesas.Count > 0 || comandas.Count > 0)
                lblIdentificacao.Text += mesas.Count > 0 ? string.Join(",", mesas) : string.Join(",", comandas);

            lblCodigoPedido.Text = Pedidos.First().IDPedido.Value.ToString("000000");
            lblPedido.Visible = lblCodigoPedido.Visible = (mesas.Count < 2 && comandas.Count < 2);

            txtDocumentoCliente.Text = string.IsNullOrWhiteSpace(Pedidos.First().DocumentoCliente) ? string.Empty : Pedidos.First().DocumentoCliente;

            if (Pedidos.First().Cliente != null && Pedidos.First().Cliente.IDCliente != null)
            {
                txtNomeCliente.Text = Pedidos.First().Cliente.NomeCompleto;
                if (string.IsNullOrWhiteSpace(txtDocumentoCliente.Text))
                    txtDocumentoCliente.Text = Pedidos.First().Cliente.Documento1;
            }

            var taxaPadrao = TipoPedido.RetornarTaxaServico(idTipoPedido);
            if (taxaPadrao > 0)
            {
                txtAcrescimoPorcentagem.Text = taxaPadrao.ToString("0.00");
                txtAcrescimoReais.Text = Acrescimos.ToString("#,##0.00");
            }
        }

        private void CarregarTipoPagamento()
        {
            List<TipoPagamentoInformation> list = BLL.TipoPagamento.ListarAtivos();
            //list.Insert(0, new TipoPagamentoInformation());

            //cbbFormaPagamento.ValueMember = "IDTipoPagamento";
            //cbbFormaPagamento.DisplayMember = "Nome";

            //cbbFormaPagamento.DataSource = list;
            //cbbFormaPagamento.SelectedIndex = 0;

            string formato = "({0}) {1}";
            cbbFormaPagamento.Items.Add(new TipoPagamentoInformation());
            foreach (var item in list)
            {
                item.Nome = string.Format(formato, item.IDTipoPagamento.Value.ToString("00"), item.Nome);
                cbbFormaPagamento.Items.Add(item);
            }
            cbbFormaPagamento.SelectedIndex = 0;
        }

        public void CarregarPedido()
        {
            AtualizarPainel();
        }

        private void AtualizarPainel()
        {
            Total = 0m;
            ValorPago = 0m;
            ValorPendente = 0m;


            if (ListaPagamentos != null)
                ValorPago += ListaPagamentos.Sum(l => l.Valor.Value);

            Total += (Pedidos.Sum(p => p.ValorTotalProdutos) - ValorDesconto) + Acrescimos;

            ValorPendente = Total - ValorPago;

            lblTotal.Text = Total.ToString("#,##0.00");
            lblValorPago.Text = ValorPago.ToString("#,##0.00");

            if (ValorPendente <= Convert.ToDecimal("0,009"))
            {
                gbxValorPendente.Text = "TROCO";
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

        public void FecharVenda()
        {
            frmAguardandoSAT frmSAT = null;
            var idPedidoFinal = 0;
            try
            {
                PedidoInformation pedidoFinal = new PedidoInformation { TipoPedido = new TipoPedidoInformation { IDTipoPedido = idTipoPedido } };
                if (Pedidos.Count > 1)
                {
                    pedidoFinal.Caixa = Pedidos.First().Caixa;
                    pedidoFinal.DtPedido = DateTime.Now;
                    pedidoFinal.DtPedidoFechamento = DateTime.Now;
                    pedidoFinal.GUIDAgrupamentoPedido = Guid.NewGuid().ToString();
                    pedidoFinal.GUIDIdentificacao = Pedidos.First().GUIDIdentificacao;
                    pedidoFinal.GUIDMovimentacao = Guid.NewGuid().ToString();
                    //pedidoFinal.NumeroPessoas = Convert.ToInt32(Pedidos.Where(p => p.NumeroPessoas.HasValue == true).ToList().Average(p => p.NumeroPessoas.Value));
                    //pedidoFinal.Observacoes = Pedidos.Select(p => p.Observacoes).Aggregate(new StringBuilder(), (sb, obs) => sb.AppendLine(string.Join(",", obs)), sb => sb.ToString());
                    pedidoFinal.ReferenciaLocalizacao = Pedidos.Select(p => p.ReferenciaLocalizacao).Aggregate(new StringBuilder(), (sb, refr) => sb.AppendLine(string.Join(",", refr)), sb => sb.ToString());
                    pedidoFinal.StatusPedido = new StatusPedidoInformation { IDStatusPedido = 10 };
                }
                else
                    pedidoFinal = Pedidos.First();

                pedidoFinal.Cliente = Cliente;
                pedidoFinal.TipoDesconto = TipoDesconto;
                pedidoFinal.ValorConsumacaoMinima = Pedidos.Where(p => p.ValorConsumacaoMinima.HasValue).Sum(p => p.ValorConsumacaoMinima.Value);
                pedidoFinal.ValorDesconto = ValorDesconto;
                pedidoFinal.ValorServico = Acrescimos;
                pedidoFinal.ListaPagamento = ListaPagamentos;
                pedidoFinal.ListaProduto = Pedidos.SelectMany(p => p.ListaProduto).ToList();


                if (txtDocumentoCliente.Text != "" && ckbNFe.Checked == true)
                {
                    pedidoFinal.DocumentoCliente = txtDocumentoCliente.Text;

                    if (Cliente != null)
                    {
                        Cliente = BLL.Cliente.Carregar(Cliente.IDCliente.Value);
                        Cliente.Documento1 = pedidoFinal.DocumentoCliente;

                        BLL.Cliente.Salvar(Cliente);
                    }
                }

                var tipoGerenciadorImpressao = ConfigurationManager.AppSettings["TipoGerenciadorImpressao"];
                if ((Total + Acrescimos) > 0 && ValorDesconto < (Total + Acrescimos))
                {
                    SalvarDB(pedidoFinal, Caixa1);
                    idPedidoFinal = pedidoFinal.IDPedido.Value;
                    switch (tipoGerenciadorImpressao)
                    {
                        case "1": //Impressora Win
                            Impressora1.TamanhoFonte = 9;
                            pedidoFinal.NumeroCupom = Impressora1.GerarCupom(pedidoFinal, true);

                            break;
                        case "2": //ECF - ACbr
                        case "3": //ECF - Bemafii
                            Impressora1.TamanhoFonte = 9;
                            pedidoFinal.NumeroCupom = Impressora1.GerarCupom(pedidoFinal, true);
                            break;
                        case "4": //SAT
                            frmSAT = new frmAguardandoSAT(pedidoFinal, PDV1.IDPDV.Value, UsuarioAutenticado.IDUsuario.Value);
                            var result = frmSAT.ShowDialog();
                            if (result == DialogResult.No)
                                throw frmSAT.Exception;
                            pedidoFinal.RetornoSAT_venda = frmSAT.RetornoSat;
                            break;
                    }

                }
                else if ((Total + Acrescimos) > 0 && ValorDesconto == (Total + Acrescimos))
                {
                    switch (tipoGerenciadorImpressao)
                    {
                        case "4":
                            using (PrintDocument pd = ImpressaoSat.CriarPrintDocument())
                                ImpressaoSat.ImprimirConta(pedidoFinal, pd);
                            break;
                        default:
                            Impressora1.TamanhoFonte = 9;
                            pedidoFinal.NumeroCupom = Impressora1.GerarCupom(pedidoFinal, false);
                            break;
                    }
                }

                GerarTicket(pedidoFinal);

                pedidoFinal.Observacoes = txtObservacoes.Text;

                decimal valorDinheiro = pedidoFinal.ListaPagamento.Where(l => l.TipoPagamento.IDTipoPagamento == 1).Sum(l => l.Valor.Value);
                PedidoPagamentoInformation pagamentoDinheiro;


                SalvarDB(pedidoFinal, Caixa1);
                Pedidos.ForEach(pedido =>
                {
                    SalvarDB(pedido, Caixa1);
                });
                if (tipoGerenciadorImpressao == "4")
                {
                    string msgErro = string.Empty;
                    var sucesso = ImpressaoSat.ImprimirCupomVenda(pedidoFinal.RetornoSAT_venda.arquivoCFeSAT, pedidoFinal.IDPedido.Value, out msgErro);

                }

                if ((valorDinheiro + ValorPendente) > 0)
                {
                    pagamentoDinheiro = new PedidoPagamentoInformation();
                    pagamentoDinheiro.Pedido = new PedidoInformation { IDPedido = pedidoFinal.IDPedido };
                    pagamentoDinheiro.TipoPagamento = new TipoPagamentoInformation { IDTipoPagamento = 1 };
                    pagamentoDinheiro.Valor = valorDinheiro + ValorPendente;

                    PedidoPagamento.Salvar(pagamentoDinheiro);
                }

                Pedidos.ForEach(pedido =>
                {
                    pedido.GUIDAgrupamentoPedido = pedidoFinal.GUIDAgrupamentoPedido;
                    EntradaSaida.Movimentar(pedido);
                    AlterarStatus(pedido.TipoPedido.IDTipoPedido.Value, pedido.GUIDIdentificacao);
                });

                AlterarStatus(pedidoFinal.TipoPedido.IDTipoPedido.Value, pedidoFinal.GUIDIdentificacao);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                if (idPedidoFinal > 0)
                    Pedido.AlterarStatus(idPedidoFinal, 50);
                //Log.Registrar(Log.TipoLog.error, "Pedido " + Pedido1.IDPedido.Value.ToString("00000") + "\n\r" + form.Erro.ToString(), true);

                //MessageBox.Show("Ocorreu um erro no fechamento! Favor tentar novamente\r\n", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show(_e.Message, "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (MessageBox.Show(ex.Message + "\nVer detalhes?", "Ocorreu um erro", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    MessageBox.Show(ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace, "Detalhes do erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (frmSAT != null)
                {
                    frmSAT.Close();
                    frmSAT = null;
                }
            }
            this.Close();
        }

        private void GerarTicket(PedidoInformation pedido)
        {
            if (ConfigurationManager.AppSettings["GerarTicketPrePago"] == "1" && pedido.TipoPedido.IDTipoPedido == 40)
            {
                Impressora1.GerarTicketPrePago(pedido);
            }
        }

        public static void SalvarDB(PedidoInformation Pedido1, CaixaInformation caixa)
        {
            Pedido.Salvar(Pedido1);

            foreach (var item in Pedido1.ListaPagamento.Where(l => l.TipoPagamento.IDTipoPagamento != 1))
            {
                item.Pedido = new PedidoInformation { IDPedido = Pedido1.IDPedido };
                PedidoPagamento.Salvar(item);
            }

            PedidoProdutoInformation servico = Pedido1.ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == 4);
            if (servico != null)
            {
                servico.Pedido = Pedido1;
                PedidoProduto.Salvar(servico);
            }

            PedidoProdutoInformation entradaCM = Pedido1.ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == 3);
            if (entradaCM != null)
            {
                entradaCM.Pedido = Pedido1;
                PedidoProduto.Salvar(entradaCM);
            }

            if (caixa != null)
                Pedido1.Caixa = new CaixaInformation { IDCaixa = caixa.IDCaixa };
            Pedido1.DtPedidoFechamento = DateTime.Now;
            Pedido1.SincERP = false;
            Pedido1.SincEmenu = false;
            Pedido1.StatusPedido = new StatusPedidoInformation { IDStatusPedido = 40 };
            Pedido1.ValorTotal = Pedido1.ValorTotalProdutosServicos - Pedido1.ValorDesconto;

            Pedido.Salvar(Pedido1);
        }

        public static void AlterarStatus(int idTipoPedido, string guidIdentificacao)
        {
            MesaInformation mesa;
            ComandaInformation comanda;

            switch (idTipoPedido)
            {
                case 10://Mesa
                    mesa = Mesa.Carregar(guidIdentificacao);
                    mesa.StatusMesa.IDStatusMesa = 10;

                    Mesa.Salvar(mesa);

                    break;

                case 20://Comanda
                    comanda = Comanda.CarregarPorGUID(guidIdentificacao);

                    if (comanda.StatusComanda.IDStatusComanda == 20)
                    {
                        comanda.StatusComanda.IDStatusComanda = 10;
                        Comanda.Salvar(comanda);
                    }

                    break;
            }
        }

        private void btnAdicionarPagamento_Click(object sender, EventArgs e)
        {
            if (cbbFormaPagamento.SelectedIndex > 0 && txtValorPagamento.Text != "" && Convert.ToDecimal(txtValorPagamento.Text) > 0)
                AdicionarPagamento();
        }

        private void AdicionarPagamento()
        {
            PedidoPagamentoInformation pagamento = new PedidoPagamentoInformation();
            decimal valorPago = 0;
            decimal valorPendente = 0;
            decimal valorPagoDinheiro = 0;

            //if (Pedido1.ListaPagamento == null)
            //    Pedido1.ListaPagamento = new List<PedidoPagamentoInformation>();

            pagamento.TipoPagamento = (TipoPagamentoInformation)cbbFormaPagamento.SelectedItem;

            //if (Pedido1.ListaPagamento != null)
            //{
            valorPago = ListaPagamentos.Sum(l => l.Valor.Value);
            //}

            if (txtValorPagamento.Text == "")
            {
                pagamento.Valor = ValorPendente;
            }
            else
            {
                pagamento.Valor = Convert.ToDecimal(txtValorPagamento.Text);
            }

            if (ListaPagamentos != null)
            {
                valorPago += pagamento.Valor.Value;
                valorPagoDinheiro = ListaPagamentos.Where(l => l.TipoPagamento.IDTipoPagamento == 1).Sum(l => l.Valor.Value);
            }

            if (pagamento.TipoPagamento.IDTipoPagamento == 1)
                valorPagoDinheiro += pagamento.Valor.Value;

            valorPendente = Total - valorPago;

            if ((valorPagoDinheiro + valorPendente) < 0)
            {
                MessageBox.Show("Não é permitido voltar troco maior que o valor pago em Dinheiro!");
            }
            else
            {
                ListaPagamentos.Add(pagamento);
                cbbFormaPagamento.SelectedIndex = 0;
                txtValorPagamento.Text = "";

                txtCodigoFormaPagamento.Text = "";
                txtCodigoFormaPagamento.Focus();
            }

            AtualizarPainel();
            ListarPagamento();
        }

        private void ListarPagamento()
        {
            if (ListaPagamentos != null)
            {
                var list = from l in ListaPagamentos
                           select new { TipoPagamento = l.TipoPagamento.Nome, l.Valor, Detalhes = "" };

                //dgvPagamentos.DataSource = null;
                dgvPagamentos.DataSource = list.ToArray();
            }
        }

        private void btnFecharVenda_Click(object sender, EventArgs e)
        {
            this.Enabled = false; this.Refresh();

            if (ValorPendente > 0)
            {
                MessageBox.Show("Ainda existe valor pendente. Conferir os pagamentos!");
            }
            else
            {
                FecharVenda();
            }

            this.Enabled = true;
        }

        private void frmPagamento_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void dgvPagamentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ListaPagamentos.RemoveAt(e.RowIndex);

                ListarPagamento();
                AtualizarPainel();
            }
        }

        private void txtDocumentoCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar))
                e.Handled = true;
        }

        private void txtDocumentoCliente_Leave(object sender, EventArgs e)
        {
            if (txtDocumentoCliente.Text.Length == 11 && !ValidacaoDocumento.IsCpf(txtDocumentoCliente.Text))
            {
                MessageBox.Show("CPF Inválido!");
                txtDocumentoCliente.Focus();
            }
            else if (txtDocumentoCliente.Text.Length == 14 && !ValidacaoDocumento.IsCnpj(txtDocumentoCliente.Text))
            {
                MessageBox.Show("CNPJ Inválido!");
                txtDocumentoCliente.Focus();
            }
            else if (txtDocumentoCliente.Text.Length != 0 && txtDocumentoCliente.Text.Length != 11 && txtDocumentoCliente.Text.Length != 14)
            {
                MessageBox.Show("CPF/CNPJ Incompleto!");
                txtDocumentoCliente.Focus();
            }
        }

        private void txtValorPagamento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && cbbFormaPagamento.SelectedIndex > 0 && (txtValorPagamento.Text == "" || Convert.ToDecimal(txtValorPagamento.Text) > 0))
                AdicionarPagamento();
        }

        private void frmPedidoPagamento_Activated(object sender, EventArgs e)
        {
            //txtValorPagamento.Focus();
        }

        private void cbbFormaPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!MostrarTeclado)
                txtValorPagamento.Focus();
        }

        private void ListarProdutos()
        {
            lblSubTotal.Text = (TotalSemAcrescimo + ConsumacaoMinima).ToString("#,##0.00");

            ListaProduto = new List<PedidoProdutoInformation>();
            PedidoProdutoInformation produto;
            Object[] row;

            foreach (var pedido in Pedidos)
            {
                var tipoEntrada = string.Empty;
                if (pedido.TipoEntrada?.IDTipoEntrada.HasValue == true)
                {
                    tipoEntrada = TipoEntrada.Carregar(pedido.TipoEntrada.IDTipoEntrada.Value).Nome;
                }
                foreach (var item in pedido.ListaProduto.Where(l => l.Cancelado != true && l.Produto.IDProduto != 4 && l.ValorUnitario > 0))
                {
                    var quantidadeInteira = item.Quantidade % 1 == 0;
                    if (quantidadeInteira)
                    {
                        for (int i = 0; i < item.Quantidade; i++)
                        {
                            produto = new PedidoProdutoInformation();
                            produto.Produto = new ProdutoInformation();
                            produto.Produto.Nome = ObterNomeProduto(item, tipoEntrada);
                            produto.ValorUnitario = item.ValorUnitario;
                            produto.ListaModificacao = item.ListaModificacao;
                            produto.Quantidade = 1;

                            ListaProduto.Add(produto);
                        }
                    }
                    else
                    {
                        produto = new PedidoProdutoInformation();
                        produto.Produto = new ProdutoInformation();
                        produto.Produto.Nome = ObterNomeProduto(item, tipoEntrada);
                        produto.ValorUnitario = item.ValorUnitario;
                        produto.ListaModificacao = item.ListaModificacao;
                        produto.Quantidade = item.Quantidade;

                        ListaProduto.Add(produto);
                    }
                }
            }

            //var list = from l in ListaProduto
            //           orderby l.Produto.Nome
            //           select new { nome = l.Produto.Nome, valorUnitario = l.ValorUnitario };

            dgvItens.Rows.Clear();
            foreach (var item in ListaProduto.OrderBy(l => l.Produto.Nome).ToList())
            {
                row = new object[] { "", $"{item.Quantidade?.ToString("#,##0.000")}×{item.Produto.Nome}", (item.ValorUnitario.Value * item.Quantidade.Value).ToString("#,##0.00") };
                dgvItens.Rows.Add(row);

                if (item.ListaModificacao != null)
                {
                    foreach (var modificacao in item.ListaModificacao)
                    {
                        row = new object[] { "", $" - {item.Quantidade?.ToString("#,##0.000")}×{modificacao.Produto.Nome}", modificacao.ValorUnitario.Value.ToString("#,##0.00") };
                        dgvItens.Rows.Add(row);
                    }
                }
            }

            dgvItens.ClearSelection();
        }

        private string ObterNomeProduto(PedidoProdutoInformation item, string tipoEntrada)
        {
            if (item.Produto.IDProduto == 2 || item.Produto.IDProduto == 3)
            {
                if (string.IsNullOrWhiteSpace(tipoEntrada) == false)
                    return $"{item.Produto.Nome} ({tipoEntrada})";
            }
            return item.Produto.Nome;
        }

        private void SomenteNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || (e.KeyChar == (char)44 && (txt.Text.Contains(",") == true || txt.Text.Length == 0)))
                e.Handled = true;
        }

        private void btnSelecionarCliente_Click(object sender, EventArgs e)
        {
            frmPesquisarCliente frm = new frmPesquisarCliente();
            frm.ShowDialog();

            Cliente = frm.Cliente1;

            if (Cliente != null && Cliente.IDCliente != null)
            {
                txtNomeCliente.Text = Cliente.NomeCompleto;
                txtDocumentoCliente.Text = Cliente.Documento1;
            }
        }

        private void frmPedidoPagamento_FormClosed(object sender, FormClosedEventArgs e)
        {
            Teclado.BlockInput(false);
        }

        private void rdbDesconto_CheckedChanged(object sender, EventArgs e)
        {
            txtAcrescimoPorcentagem.Text = "0,00";
            AtualizarDescontoPorcentagem();
        }

        private void rdbAcrescimo_CheckedChanged(object sender, EventArgs e)
        {
            txtAcrescimoPorcentagem.Text = "0,00";
            AtualizarDescontoPorcentagem();
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

        private void AtualizarDescontoPorcentagem()
        {
            decimal descontoPorcentagem = 0;
            decimal descontoReais = 0;

            if (txtDescontoPorcentagem.Text != "")
                descontoPorcentagem = Convert.ToDecimal(txtDescontoPorcentagem.Text);

            if (descontoPorcentagem < 100)
            {
                descontoReais = Pedidos.Sum(p => p.ValorTotalProdutosServicos) * descontoPorcentagem / 100;
                descontoReais = Math.Truncate(descontoReais * 100) / 100;
                txtDescontoReais.Text = descontoReais.ToString("#,##0.00");
            }
            else
            {
                descontoReais = Pedidos.Sum(p => p.ValorTotalProdutosServicos);
                txtDescontoReais.Text = descontoReais.ToString("#,##0.00");
            }

            ValorDesconto = descontoReais;
            AtualizarPainel();
        }
        private void AtualizarDescontoReais()
        {
            decimal descontoPorcentagem = 0;
            decimal descontoReais = 0;

            if (txtDescontoReais.Text != "")
                descontoReais = Convert.ToDecimal(txtDescontoReais.Text);

            if (descontoReais < Total)
            {
                descontoPorcentagem = descontoReais * 100 / Total;

                txtDescontoPorcentagem.Text = descontoPorcentagem.ToString("#,##0.00");
            }
            else
            {
                descontoReais = Total;
                descontoPorcentagem = 100;

                txtDescontoPorcentagem.Text = descontoPorcentagem.ToString("#,##0.00");
            }

            ValorDesconto = descontoReais;
            AtualizarPainel();
        }

        private void CalcularConsumacaoMinima()
        {
            if (Pedidos.Count == 1)
            {
                Pedido.AdicionarProdutoConsumacaoMinima(Pedidos[0], PDV1, UsuarioAutenticado);
            }
            else
            {
                var consumacaoMinima = Pedidos.Sum(l => l.TipoEntrada.ValorConsumacaoMinima.Value);
                if (consumacaoMinima > TotalSemAcrescimo)
                {
                    Pedido.AdicionarProdutoConsumacaoMinima(Pedidos[0], PDV1, UsuarioAutenticado, (consumacaoMinima - TotalSemAcrescimo));
                }
            }
            ConsumacaoMinima = Pedidos.SelectMany(p => p.ListaProduto).Where(pp => pp.Produto.IDProduto == 3).Sum(pp => pp.ValorUnitario.Value);
        }

        private void CalcularServico()
        {
            Pedidos.ForEach(pedido =>
            {
                Pedido.AdicionarProdutoServico(pedido, ckbAplicarServico.Checked, PDV1, UsuarioAutenticado);
            });
            Acrescimos = Pedidos.SelectMany(p => p.ListaProduto).Where(p => p.Produto.IDProduto == 4).Sum(p => p.ValorUnitario).Value;
        }
        //private void CalcularDesconto()
        //{
        //    if (ckbAplicarServico.Checked == true)
        //    {
        //    }
        //    else
        //    {
        //        txtDescontoPorcentagem.Text = "0,00";
        //        txtDescontoReais.Text = "0,00";
        //    }
        //}

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

        private void ckbAplicarServico_Click(object sender, EventArgs e)
        {
            var sucesso = false;
            if (AuthFunc != null)
            {
                sucesso = AuthFunc(true, false, false);
            }
            else
            {
                frmAutenticacao2 frm = new frmAutenticacao2(true, false, false);
                frm.ShowDialog();

                sucesso = frm.DialogResult == DialogResult.OK;
            }
            if (sucesso)
            {
                CalcularServico();
                CalcularConsumacaoMinima();
                AtualizarDescontoPorcentagem();
                AtualizarPainel();
                ListarProdutos();
            }
            else
            {
                ckbAplicarServico.Checked = !ckbAplicarServico.Checked;
            }
        }

        private void ckbAplicaDesconto_Click(object sender, EventArgs e)
        {
            var solicitarSenha = Configuracao.RetornarValor("SolicitarSenhaDesconto") == "1";
            var sucesso = false;

            if (solicitarSenha)
            {
                if (AuthFunc != null)
                {
                    sucesso = AuthFunc(true, false, false);
                }
                else
                {
                    frmAutenticacao2 frm = new frmAutenticacao2(false, true, false, false);
                    frm.ShowDialog();
                    sucesso = frm.DialogResult == DialogResult.OK;
                }
            }

            if (solicitarSenha == false || sucesso)
            {

                if (ckbAplicaDesconto.Checked == true)
                {
                    DialogResult tipoR = DialogResult.None;

                    var solicitarTipoDesconto = Configuracao.RetornarValor("SolicitarTipoDesconto") == "1";
                    frmTipoDesconto frmTipoDesconto = null;

                    if (solicitarTipoDesconto)
                    {
                        frmTipoDesconto = new frmTipoDesconto();
                        frmTipoDesconto.ShowDialog();
                        tipoR = frmTipoDesconto.DialogResult;
                    }

                    if (solicitarTipoDesconto && tipoR == DialogResult.OK)
                    {
                        TipoDesconto = new TipoDescontoInformation { IDTipoDesconto = frmTipoDesconto.IDTipoDesconto };
                        txtDescontoPorcentagem.Enabled = true;
                        txtDescontoReais.Enabled = true;
                    }
                    else if (solicitarTipoDesconto && tipoR != DialogResult.OK)
                    {
                        ckbAplicaDesconto.Checked = !ckbAplicaDesconto.Checked;
                    }
                    else
                    {
                        txtDescontoPorcentagem.Enabled = true;
                        txtDescontoReais.Enabled = true;
                    }
                }
                else
                {
                    TipoDesconto = null;
                    txtDescontoPorcentagem.Text = "0,00";
                    AtualizarDescontoPorcentagem();

                    txtDescontoPorcentagem.Enabled = true;
                    txtDescontoReais.Enabled = true;
                }

            }
            else
            {
                ckbAplicaDesconto.Checked = !ckbAplicaDesconto.Checked;
            }
        }

        private void txt_Enter(object sender, EventArgs e)
        {
            EnterHandler(sender, frmPedidoProdutoValor.TipoNumero.DINHEIRO);
        }

        private void EnterHandler(object sender, frmPedidoProdutoValor.TipoNumero tipo)
        {
            if (MostrarTeclado)
            {
                var txt = sender as TextBox;
                var valor = ObterValorTeclado(tipo);
                txt.Text = (valor > 0 ? valor.ToString() : string.Empty);
                Focus();
            }
        }

        private decimal ObterValorTeclado(frmPedidoProdutoValor.TipoNumero tipo = frmPedidoProdutoValor.TipoNumero.DINHEIRO)
        {
            var teclado = new frmPedidoProdutoValor(tipo);
            teclado.StartPosition = FormStartPosition.Manual;
            teclado.Location = Location;
            if (teclado.ShowDialog() == DialogResult.OK)
                return teclado.Valor;
            return 0m;
        }

        private void txt_EnterInteiro(object sender, EventArgs e)
        {
            EnterHandler(sender, frmPedidoProdutoValor.TipoNumero.INTEIRO);
        }

        private void txt_EnterDecimal(object sender, EventArgs e)
        {
            EnterHandler(sender, frmPedidoProdutoValor.TipoNumero.DECIMAL);
        }
    }
}