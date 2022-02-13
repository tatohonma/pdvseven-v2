using a7D.PDV.BLL;
using a7D.PDV.BLL.Utils;
using a7D.PDV.Caixa.UI.Properties;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPedidoPagamento : Form, IPedidoPagamentoForm
    {
        private decimal Total { get; set; }
        private decimal ValorPago { get; set; }
        private decimal ValorPendenteSemDesconto { get; set; }
        //private Action PosExecucao { get; set; }

        public PedidoInformation PedidoAtual { get; private set; }
        public decimal ValorPendente { get; private set; }
        List<PedidoProdutoInformation> ListaProduto;
        public bool AceitarParcial { get; set; }
        public void HabilitarParcial() => AceitarParcial = true;

        public frmPedidoPagamento()
        {
            InitializeComponent();
        }

        public frmPedidoPagamento(Int32 idPedido) : this() // Action posExecucao = null
        {
            PedidoAtual = Pedido.CarregarCompleto(idPedido);
            Saldo.AdicionaPagamentoPorSaldo(PedidoAtual);
        }

        private void frmPagamento_Load(object sender, EventArgs e)
        {
            GA.Post(this);

            this.LoadLocationSize(EConfig._PedidoPagamentoLocationSize);

            PedidoAtual.Caixa = frmPrincipal.Caixa1;
            dgvItens.Columns[2].Visible = PedidoAtual.ListaProduto.Any(p => p.Viagem == true);
            chkExpedicao.Visible = chkExpedicao.Checked = dgvItens.Columns[2].Visible && ConfiguracoesCaixa.Valores.ImprimirViaExpedicao == "CONTA";

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

            if (!AceitarParcial && PedidoAtual.ListaPagamento?.Count > 0)
                AceitarParcial = true;

            CarregarTipoPagamento();

            txtDocumentoCliente.Text = string.IsNullOrWhiteSpace(PedidoAtual.DocumentoCliente) ? string.Empty : PedidoAtual.DocumentoCliente;
            txtEmail.Text = string.IsNullOrWhiteSpace(PedidoAtual.EmailCliente) ? string.Empty : PedidoAtual.EmailCliente;
            ckbEnviarEmail.Checked = !string.IsNullOrEmpty(txtEmail.Text);

            if (PedidoAtual.Cliente != null && PedidoAtual.Cliente.IDCliente != null)
            {
                txtNomeCliente.Text = PedidoAtual.Cliente.NomeCompleto;
                if (string.IsNullOrWhiteSpace(txtDocumentoCliente.Text))
                    txtDocumentoCliente.Text = PedidoAtual.Cliente.Documento1;
            }

            txtAcrescimoReais.Text = PedidoAtual.ValorServicoTemp.ToString("#,#0.00");
            txtAcrescimoPorcentagem.Text = PedidoAtual.TaxaServicoPadrao.Value.ToString("0.00");

            if (PedidoAtual.AplicarServico == null)
                PedidoAtual.AplicarServico = true;

            ckbAplicarServico.Checked = PedidoAtual.AplicarServico == true;
            CalcularServico();
            CalcularConsumacaoMinima();

            Total = PedidoAtual.ValorTotalProdutos + PedidoAtual.ValorServico.Value;

            AtualizarPainel();
            ListarPagamento();
            ListarProdutos();

            if (PedidoAtual.AplicarDesconto == true && PedidoAtual.ValorDesconto > 0)
            {
                txtDescontoReais.Text = PedidoAtual.ValorDesconto.Value.ToString("0.00");
                ckbAplicaDesconto.Checked = true;
                txtDescontoPorcentagem.Enabled = txtDescontoReais.Enabled = true;
                AtualizarDescontoReais();
            }
            else
                PedidoAtual.ValorDesconto = 0;

            txtCodigoFormaPagamento.Focus();

            chkImprimir.Checked = ConfiguracoesCaixa.Valores.ImprimirCupomFiscal == "SIM";
        }

        private void CarregarTipoPagamento()
        {
            List<TipoPagamentoInformation> list = BLL.TipoPagamento.ListarAtivos();

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

        private void btnAdicionarPagamento_Click(object sender, EventArgs e)
        {
            AdicionarPagamento();
        }

        private void AdicionarPagamento()
        {
            PedidoPagamentoInformation pagamento = new PedidoPagamentoInformation();
            pagamento.Excluido = false;
            Decimal valorPago = 0;
            Decimal valorPendente = 0;
            Decimal valorPagoDinheiro = 0;

            if (PedidoAtual.ListaPagamento == null)
                PedidoAtual.ListaPagamento = new List<PedidoPagamentoInformation>();

            // no load sempre carrega um pagamento vazio para ser opção a seleção
            if (cbbFormaPagamento.SelectedIndex < 1 && cbbFormaPagamento.Items.Count > 0)
                pagamento.TipoPagamento = (TipoPagamentoInformation)cbbFormaPagamento.Items[1];
            else
                pagamento.TipoPagamento = (TipoPagamentoInformation)cbbFormaPagamento.SelectedItem;

            if (PedidoAtual.ListaPagamento != null)
                valorPago = PedidoAtual.ListaPagamento.Sum(l => l.Valor.Value);

            if (txtValorPagamento.Text == "")
                pagamento.Valor = PedidoAtual.ValorTotalProdutosServicos - (PedidoAtual.ValorDesconto ?? 0) - valorPago;
            else
                pagamento.Valor = Convert.ToDecimal(txtValorPagamento.Text);

            if (PedidoAtual.ListaPagamento != null)
            {
                valorPago += pagamento.Valor.Value;
                valorPagoDinheiro = PedidoAtual.ListaPagamento.Where(l => l.TipoPagamento.IDTipoPagamento == 1).Sum(l => l.Valor.Value);
            }

            if (pagamento.TipoPagamento.MeioPagamentoSAT?.IDMeioPagamentoSAT == (int)EMetodoPagamento.Dinheiro)
                valorPagoDinheiro += pagamento.Valor.Value;

            valorPendente = Total - valorPago;

            btnFecharVenda.Text = AceitarParcial && valorPendente > 0 ? "&PAGAR PARCIAL" : "&FECHAR";
            PedidoAtual.DocumentoCliente = txtDocumentoCliente.Text;

            bool lFechar = false;
            TopMost = false;
            if ((valorPagoDinheiro + valorPendente) < 0)
                MessageBox.Show("Não é permitido voltar troco maior que o valor pago em Dinheiro!");
            else if (pagamento.Valor == 0)
            {
                // Precisa exibir alguma mensagem, ou só discato? 
                // MessageBox.Show("Informe o valor para o pagamento");
            }
            else if (PagamentoVenda.Efetiva(PedidoAtual, pagamento, valorPendente))
            {
                if (pagamento.TipoPagamento.IDGateway > 0) // Pagamento por gateway pode fechar a tela diretamente, se não houver valor pendente
                {
                    lFechar = ValorPendente == pagamento.Valor;
                    AceitarParcial = true;
                }

                PedidoAtual.ListaPagamento.Add(pagamento);

                cbbFormaPagamento.SelectedIndex = 0;
                txtValorPagamento.Text = "";
                txtCodigoFormaPagamento.Text = "";

                if (valorPendente > 0)
                    txtCodigoFormaPagamento.Focus();
                else
                    btnFecharVenda.Focus();
            }
            TopMost = true;

            AtualizarPainel();
            ListarPagamento();

            if (lFechar)
            {
                Fechar();
            }
        }

        private void ListarPagamento()
        {
            if (PedidoAtual.ListaPagamento != null)
            {
                var list = from l in PedidoAtual.ListaPagamento
                           select new { TipoPagamento = l.TipoPagamento.Nome, l.Valor };

                //dgvPagamentos.DataSource = null;
                dgvPagamentos.DataSource = list.ToArray();
            }
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
                Close();
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
                else if (pedidoAtual.ListaPagamento.Any(p => p.TipoPagamento.Gateway == EGateway.ContaCliente))
                {
                    Logs.ErroBox(CodigoErro.AE33);
                    return;
                }
            }

            this.Enabled = false;
            this.Refresh();

            if (ValorPendente > 0)
            {
                if (AceitarParcial && PedidoAtual.ListaPagamento != null && PedidoAtual.ListaPagamento.Count > 0)
                {
                    foreach (var pagamentoParcial in PedidoAtual.ListaPagamento.Where(p => p.Status != StatusModel.Inalterado))
                    {
                        if (pagamentoParcial.Status == StatusModel.Excluido)
                            PedidoPagamento.Cancelar(pagamentoParcial, AC.Usuario.IDUsuario.Value);
                        else
                        {
                            pagamentoParcial.Pedido = new PedidoInformation { IDPedido = PedidoAtual.IDPedido };
                            PedidoPagamento.Salvar(pagamentoParcial, AC.Usuario.IDUsuario.Value);
                        }
                    }
                    Close();
                }
                else
                {
                    MessageBox.Show("Ainda existe valor pendente. Conferir os pagamentos!");
                    txtCodigoFormaPagamento.Focus();
                }
            }
            else
            {
                PedidoAtual.DocumentoCliente = txtDocumentoCliente.Text;

                try
                {
                    if (PedidoAtual.TipoPedido.TipoPedido == ETipoPedido.Balcao)
                    {
                        TopMost = false;
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

                if (!ckbNF.Checked)
                    PedidoAtual.DocumentoCliente = "";

                PedidoAtual.EmailCliente = txtEmail.Text;

                var dialog = new FecharVenda(PedidoAtual, ckbNF.Checked, txtObservacoes.Text)
                    .NaImpressora(ConfiguracoesCaixa.Valores.ModeloImpressora)
                    .Fechar(true, chkImprimir.Checked, chkExpedicao.Checked);

                if (dialog == DialogResult.OK)
                {
                    DialogResult = dialog;
                    frmPedidos.GUIDIdentificacao_selecionado = null;
                    frmPedidos.ReabrirPedidos = true;
                    Close();
                }
            }

            this.Enabled = true;
        }

        private void dgvPagamentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (PedidoAtual.ListaPagamento[e.RowIndex].IDPedidoPagamento.HasValue
                 || PedidoAtual.ListaPagamento[e.RowIndex].Autorizacao != null)
                {
                    if (PedidoAtual.ListaPagamento[e.RowIndex].TipoPagamento.Gateway == EGateway.ContaCliente)
                    {
                        MessageBox.Show("Não é possivel extornar um pagamento de 'Conta de Cliente'", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) != DialogResult.OK)
                        return;

                    if (PedidoAtual.ListaPagamento[e.RowIndex].IDPedidoPagamento != null)
                        PedidoPagamento.Cancelar(PedidoAtual.ListaPagamento[e.RowIndex], usuario.IDUsuario.Value);

                }
                PedidoAtual.ListaPagamento.RemoveAt(e.RowIndex);

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

        private void cbbFormaPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtValorPagamento.Focus();
        }

        private void ListarProdutos()
        {
            lblSubTotal.Text = PedidoAtual.ValorTotalProdutos.ToString("#,##0.00");

            ListaProduto = new List<PedidoProdutoInformation>();
            PedidoProdutoInformation produto;
            object[] row;

            foreach (var item in
                PedidoAtual.ListaProduto
                .Where(l => l.Cancelado != true && l.Produto.IDProduto != 4)
                .Where(l => ConfiguracoesSistema.Valores.ExibirItensZeradosCaixaPreConta || l.ValorUnitario > 0 || l.ListaModificacao?.Any(m => m.ValorUnitario > 0) == true))
            {
                var quantidade = item.Quantidade.Value;

                if (quantidade % 1 == 0)
                {
                    for (int i = 0; i < item.Quantidade; i++)
                    {
                        produto = new PedidoProdutoInformation();
                        produto.Produto = new ProdutoInformation();
                        produto.Produto.Nome = item.Produto.Nome;
                        produto.ValorUnitario = item.ValorUnitario;
                        produto.ListaModificacao = item.ListaModificacao;
                        produto.Viagem = item.Viagem;
                        produto.Quantidade = 1;

                        ListaProduto.Add(produto);
                    }
                }
                else
                {
                    produto = new PedidoProdutoInformation();
                    produto.Produto = new ProdutoInformation();
                    produto.Produto.Nome = item.Produto.Nome;
                    produto.ValorUnitario = item.ValorUnitario;
                    produto.ListaModificacao = item.ListaModificacao;
                    produto.Quantidade = quantidade;
                    produto.Viagem = item.Viagem;

                    ListaProduto.Add(produto);
                }
            }

            //var list = from l in ListaProduto
            //           orderby l.Produto.Nome
            //           select new { nome = l.Produto.Nome, valorUnitario = l.ValorUnitario };

            dgvItens.Rows.Clear(); // ?? Cancelados! mod => mod.Cancelado != true
            foreach (var item in ListaProduto.OrderBy(l => l.Produto.Nome).ToList())
            {
                if (item.ValorUnitario > 0 || ConfiguracoesSistema.Valores.ExibirItensZeradosCaixaPreConta)
                {
                    row = new object[] {
                        $"{item.Quantidade.Value.ToString("0.000")}×{item.Produto.Nome}",
                        item.ValorUnitarioString,
                        item.Viagem==true ? ImageUtil.Imagem_ViagemSacola():Resources.semImagem};
                    dgvItens.Rows.Add(row);

                    if (item.ListaModificacao != null)
                    {
                        foreach (var modificacao in item.ListaModificacao)
                        {
                            if (modificacao.ValorUnitario > 0 || ConfiguracoesSistema.Valores.ExibirItensZeradosCaixaPreConta)
                            {
                                row = new object[] {
                                    " - " + modificacao.Produto.Nome,
                                    modificacao.ValorUnitarioString,
                                    item.Viagem==true ? ImageUtil.Imagem_ViagemSacola():Resources.semImagem
                                };
                                dgvItens.Rows.Add(row);
                            }
                        }
                    }
                }
                else
                {
                    bool exibiuPai = false;
                    foreach (var modificacao in item.ListaModificacao.OrderByDescending(mod => valorUnitario))
                    {
                        if (modificacao.ValorUnitario > 0 || !ConfiguracoesSistema.Valores.ExibirItensZeradosCaixaPreConta)
                        {
                            if (exibiuPai)
                            {
                                row = new object[] {
                                    " - " + modificacao.Produto.Nome,
                                    modificacao.ValorUnitarioString,
                                    item.Viagem==true ? ImageUtil.Imagem_ViagemSacola():Resources.semImagem
                                };
                            }
                            else
                            {
                                row = new object[] {
                                    $"{item.Quantidade.Value.ToString("0.000")}×{item.Produto.Nome}\r\n - {modificacao.Produto.Nome}",
                                    modificacao.ValorUnitarioString,
                                    item.Viagem==true ? ImageUtil.Imagem_ViagemSacola():Resources.semImagem
                                };
                                exibiuPai = true;
                            }
                            dgvItens.Rows.Add(row);
                        }
                    }
                }
            }

            dgvItens.ClearSelection();
        }

        private void SomenteNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || (e.KeyChar == (char)44 && (txt.Text.Contains(",") == true || txt.Text.Length == 0)))
                e.Handled = true;
        }

        private void btnSelecionarCliente_Click(object sender, EventArgs e)
        {
            if (sender is TextBox && PedidoAtual.Cliente != null && PedidoAtual.Cliente.IDCliente != null)
            {
                if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
                {
                    frmAdicionarCliente form = new frmAdicionarCliente(PedidoAtual.Cliente.IDCliente.Value);
                    form.ShowDialog();
                }
            }
            else
            {
                frmPesquisarCliente frm = new frmPesquisarCliente();
                frm.ShowDialog();

                PedidoAtual.Cliente = frm.Cliente1;

                if (PedidoAtual.Cliente != null && PedidoAtual.Cliente.IDCliente != null)
                {
                    txtNomeCliente.Text = PedidoAtual.Cliente.NomeCompleto;
                    txtDocumentoCliente.Text = PedidoAtual.Cliente.Documento1;
                    txtEmail.Text = PedidoAtual.Cliente.Email;
                }
            }
        }

        private void frmPedidoPagamento_FormClosed(object sender, FormClosedEventArgs e)
        {
            Teclado.BlockInput(false);
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
            Decimal descontoPorcentagem = 0;
            Decimal descontoReais = 0;

            if (txtDescontoPorcentagem.Text != "")
                descontoPorcentagem = Convert.ToDecimal(txtDescontoPorcentagem.Text);

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
            PedidoAtual.ValorDesconto = descontoReais;
            txtDescontoReais.Text = descontoReais.ToString("#,##0.00");

            AtualizarPainel();
        }
        private void AtualizarDescontoReais()
        {
            Decimal descontoPorcentagem = 0;
            Decimal descontoReais = 0;

            if (txtDescontoReais.Text != "")
                descontoReais = Convert.ToDecimal(txtDescontoReais.Text);

            if (ValorPendenteSemDesconto <= 0)
            {
                ckbAplicaDesconto.Checked = false;
                return;
            }
            else if (descontoReais < ValorPendenteSemDesconto)
            {
                descontoPorcentagem = descontoReais * 100m / ValorPendenteSemDesconto;

                txtDescontoPorcentagem.Text = descontoPorcentagem.ToString("#,##0.00");
            }
            else
            {
                descontoReais = ValorPendenteSemDesconto;
                descontoPorcentagem = 100;

                txtDescontoPorcentagem.Text = descontoPorcentagem.ToString("#,##0.00");
            }

            PedidoAtual.ValorDesconto = descontoReais;

            AtualizarPainel();
        }

        private void CalcularConsumacaoMinima()
        {
            Pedido.AdicionarProdutoConsumacaoMinima(PedidoAtual, AC.PDV, AC.Usuario);
        }

        private void CalcularServico()
        {
            Pedido.AdicionarProdutoServico(PedidoAtual, ckbAplicarServico.Checked, AC.PDV, AC.Usuario);
        }

        private void txtDescontoPorcentagem_TextChanged(object sender, EventArgs e)
        {
            if (txtDescontoPorcentagem.Focused == true)
                AtualizarDescontoPorcentagem();
        }

        private void txtDescontoReais_TextChanged(object sender, EventArgs e)
        {
            if (txtDescontoReais.Focused == true)
                AtualizarDescontoReais();
        }

        private void ckbAplicarServico_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuarioTaxaServico) == DialogResult.OK)
            {
                PedidoAtual.UsuarioTaxaServico = usuarioTaxaServico;
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
            var solicitarSenha = ConfiguracoesSistema.Valores.SolicitarSenhaDesconto;
            DialogResult r = DialogResult.None;
            var usuarioDesconto = default(UsuarioInformation);

            if (ValorPendenteSemDesconto <= 0)
            {
                MessageBox.Show("Não há valores para aplicar desconto");
                if (ckbAplicaDesconto.Checked)
                    ckbAplicaDesconto.Checked = false;
                return;
            }

            if (solicitarSenha)
                r = NormalOuTouch.Autenticacao(false, true, false, false, out usuarioDesconto);
            else
                usuarioDesconto = AC.Usuario;

            if (solicitarSenha == false || r == DialogResult.OK)
            {

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
                        PedidoAtual.TipoDesconto = new TipoDescontoInformation { IDTipoDesconto = frmTipoDesconto.IDTipoDesconto };
                        PedidoAtual.UsuarioDesconto = usuarioDesconto;
                        txtDescontoPorcentagem.Enabled = true;
                        txtDescontoReais.Enabled = true;
                    }
                    else if (solicitarTipoDesconto && tipoR != DialogResult.OK)
                    {
                        // ngativo
                        PedidoAtual.UsuarioDesconto = null;
                        ckbAplicaDesconto.Checked = !ckbAplicaDesconto.Checked;
                    }
                    else
                    {
                        PedidoAtual.UsuarioDesconto = usuarioDesconto;
                        txtDescontoPorcentagem.Enabled = true;
                        txtDescontoReais.Enabled = true;
                    }
                }
                else
                {
                    PedidoAtual.TipoDesconto = null;

                    txtDescontoPorcentagem.Text = "0,00";
                    AtualizarDescontoPorcentagem();

                    PedidoAtual.UsuarioDesconto = usuarioDesconto;

                    txtDescontoPorcentagem.Enabled = false;
                    txtDescontoReais.Enabled = false;
                }
            }
            else
            {
                ckbAplicaDesconto.Checked = !ckbAplicaDesconto.Checked;
            }
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

            this.SaveLocationSize(EConfig._PedidoPagamentoLocationSize);

            Settings.Default.Save();
        }

        private void ckbPagamentoParcial_CheckedChanged(object sender, EventArgs e)
        {
            btnFecharVenda.Text = AceitarParcial ? "&PAGAR PARCIAL" : "&FECHAR";
        }

        private void ckbEnviarEmail_CheckedChanged(object sender, EventArgs e)
        {
            PedidoAtual.EnviarNfEmailCliente = false;
            if (!ckbEnviarEmail.Checked)
                return;

            if (txtEmail.Text == "")
            {
                ckbEnviarEmail.Checked = false;
                MessageBox.Show("Digite um email.");
                txtEmail.Focus();
                return;
            }

            if (!EmailUtil.IsValidEmail(txtEmail.Text))
            {
                ckbEnviarEmail.Checked = false;
                MessageBox.Show("Digite um email válido.");
                txtEmail.Focus();
                return;
            }

            PedidoAtual.EmailCliente = txtEmail.Text;
            PedidoAtual.EnviarNfEmailCliente = true;
        }
    }
}