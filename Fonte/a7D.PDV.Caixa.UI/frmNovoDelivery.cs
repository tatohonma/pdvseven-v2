using a7D.PDV.BLL;
using a7D.PDV.BLL.Services;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using a7D.PDV.Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmNovoDelivery : Form
    {
        #region DTO
        struct Grupo
        {
            public RadioButton Radio { get; set; }
            public GroupBox GroupBox { get; set; }
            public Control Focus { get; set; }
        }

        private enum EstadoDelivery
        {
            NaoConfirmado,
            Novo,
            Edicao,
            Entregador,
            Finalizar,
            Identificar
        }
        #endregion

        #region Variáveis

        private PedidoInformation Pedido1 { get; set; }
        //private OrcamentoResponse loggiOrcamento = null;
        //private Task loggiTask = null;
        private List<TaxaEntregaInformation> TaxasEntrega { get; set; }
        private List<EntregadorInformation> Entregadores { get; set; }
        private List<PedidoPagamentoInformation> Pagamentos { get; set; }
        private Dictionary<int, InformacoesCancelamento?> InformacoesCancelamento { get; set; }
        private bool TaxaSelecionada { get; set; }
        private bool ClienteSelecionado { get; set; }
        private bool Alterado { get; set; }
        private bool ConfirmarSaida { get; set; }
        private decimal Troco { get; set; }
        private decimal TaxaServico { get; set; }
        private static List<Grupo> Grupos { get; set; }
        private Stack<string> Navegacao { get; set; }
        private bool Navegar { get; set; }
        public bool AdicionarProdutos { get; set; }
        private EstadoDelivery _estado { get; set; }
        private bool AppDelivery;

        private IFormatProvider _provider = new CultureInfo("pt-BR");

        #region Calculados

        private Controles.PedidoProduto controlePedidoProduto => pedidoProduto1;
        private decimal TotalProdutos => PedidoProdutoInformation.SomaValorTotal(controlePedidoProduto.ListaPedidoProduto);
        private decimal ValorServico => ckbTaxaServico.Checked ? TotalProdutos * (TaxaServico / 100) : 0;
        private decimal TotalEntrega => TotalProdutos + (AppDelivery ? (Pedido1.ValorEntrega ?? 0) : (Pedido1.TaxaEntrega != null ? (Pedido1.TaxaEntrega.Valor ?? 0) : 0));
        private decimal ValorPago => Pedido1.ListaPagamento.Where(p => p.Status != StatusModel.Excluido).Sum(p => p.Valor.Value);

        #endregion

        private static DataGridViewCellStyle cellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleRight
        };

        public static frmNovoDelivery NovoPedidoDelivery()
        {
            return new frmNovoDelivery(true);
        }

        public static frmNovoDelivery EditarPedidoDelivery(string guidIdentificacao)
        {
            return new frmNovoDelivery(guidIdentificacao, EstadoDelivery.Edicao);
        }

        public static frmNovoDelivery EnviarOuFinalizarPedidoDelivery(string guidIdentificacao)
        {
            return new frmNovoDelivery(guidIdentificacao, EstadoDelivery.Identificar);
        }

        private frmNovoDelivery()
        {
            InitializeComponent();
            controlePedidoProduto.TipoPedidoSelecionado = ETipoPedido.Delivery;
        }

        private frmNovoDelivery(bool novo) : this()
        {
            _estado = EstadoDelivery.Novo;
            Pedido1 = Pedido.NovoPedidoDelivery(frmPrincipal.Caixa1);
            AppDelivery = false;
        }

        private frmNovoDelivery(string guidIdentificacao, EstadoDelivery estado) : this()
        {
            Pedido1 = Pedido.CarregarUltimoPedido(guidIdentificacao);

            if (Pedido1.OrigemPedido != null &&
                (Pedido1.OrigemPedido.IDOrigemPedido == (int)EOrigemPedido.ifood ||
                Pedido1.OrigemPedido.IDOrigemPedido == (int)EOrigemPedido.deliveryOnline))
            {
                AppDelivery = true;

                controlePedidoProduto.BloqueiaEdicao();
            }

            if (estado == EstadoDelivery.Identificar)
            {
                if (Pedido1.StatusPedido.StatusPedido == EStatusPedido.Enviado)
                {
                    _estado = EstadoDelivery.Finalizar;
                }
                else if (Pedido1.StatusPedido.StatusPedido == EStatusPedido.NaoConfirmado)
                {
                    _estado = EstadoDelivery.NaoConfirmado;
                }
                else
                {
                    _estado = EstadoDelivery.Entregador;
                }
            }
            else if (estado == EstadoDelivery.Edicao)
            {
                if (Pedido1.StatusPedido.StatusPedido == EStatusPedido.Enviado)
                {
                    _estado = EstadoDelivery.Finalizar;
                }
                else if (Pedido1.StatusPedido.StatusPedido == EStatusPedido.NaoConfirmado)
                {
                    _estado = EstadoDelivery.NaoConfirmado;
                }
                else
                {
                    _estado = estado;
                }
            }
            else
                _estado = estado;
        }

        #region eventos

        private void CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton.Checked)
            {
                radioButton.ImageIndex = 1;
            }
            else
            {
                if (Navegar)
                    Navegacao?.Push(radioButton.Name);
                Navegar = true;
                lnkVoltar.Visible = Navegacao?.Count > 0;
                radioButton.ImageIndex = 0;
            }

            AlterarVisibilidade();
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            var dgv = sender as DataGridView;
            dgv?.ClearSelection();
        }

        private void SomenteNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            var txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || (e.KeyChar == (char)44 && (txt.Text.Contains(",") == true || txt.Text.Length == 0)))
                e.Handled = true;
        }

        #region form
        private void frmNovoDelivery_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            Cursor = Cursors.WaitCursor;
            gbLoggi.Visible = BLL.PDV.PossuiLoggi();

            Refresh();

            ConfirmarSaida = true;

            InformacoesCancelamento = new Dictionary<int, InformacoesCancelamento?>();

            cbbEstado.DataSource = Estado.Listar().OrderBy(l => l.Sigla).ToList();
            cbbEstado.SelectedValue = 25;

            TaxasEntrega = TaxaEntrega.ListarAtivos();
            bool possuiTaxaEntrega = TaxasEntrega.Count != 0;
            if (!possuiTaxaEntrega || AppDelivery)
            {
                rb3.Enabled = false;
                gbResumoTaxaEntrega.Visible = false;
            }
            else
            {
                dgvTaxaEntrega.DataSource = TaxasEntrega.Select(t => new { t.IDTaxaEntrega, t.Nome, t.Valor }).ToArray();
                dgvTaxaEntrega.ClearSelection();
            }

            controlePedidoProduto.ProdutoAdicionado = (PedidoProdutoInformation pedidoProduto) =>
            {
                Alterado = true;
                AtualizarListaProdutos();
                AtualizarPainel();
                AtualizarExibicao();
            };

            controlePedidoProduto.ProdutoRemovido = (PedidoProdutoInformation pedidoProduto, InformacoesCancelamento? informacoesCancelamento) =>
            {
                if (informacoesCancelamento != null)
                    InformacoesCancelamento.Add(pedidoProduto.IDPedidoProduto.Value, informacoesCancelamento);

                Alterado = true;
                AtualizarListaProdutos();
                AtualizarPainel();
                AtualizarExibicao();
            };

            controlePedidoProduto.ModificacaoAlterada = (PedidoProdutoInformation pedidoProduto, List<PedidoProdutoInformation> modificacoes) =>
            {
                Alterado = true;
                AtualizarListaProdutos();
                AtualizarPainel();
                AtualizarExibicao();
            };

            controlePedidoProduto.Confirmar = () =>
            {
                if (controlePedidoProduto.ListaPedidoProduto.Count > 0)
                {
                    if (dgvTaxaEntrega.DataSource != null)
                    {
                        rb3.Enabled = true;
                        rb3.Checked = true;
                    }
                    else
                    {
                        rb4.Enabled = true;
                        rb4.Checked = true;
                    }
                }
                else
                {
                    MessageBox.Show("Selecione os produtos antes de continuar", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            controlePedidoProduto.DescontoAplicado = (PedidoProdutoInformation pp, UsuarioInformation usuario) =>
            {
                AtualizarPainel();
                AtualizarExibicao();
            };

            controlePedidoProduto.Cancelar = false;
            controlePedidoProduto.Visible = gb2.Visible;
            controlePedidoProduto.CarregarPedidoProduto(null, null);
            CarregarTipoPagamento();

            Grupos = new List<Grupo>{
                new Grupo
                {
                    Radio = rb1,
                    GroupBox = gb1,
                    Focus = txtNomeCompleto
                },
                new Grupo
                {
                    Radio = rb2,
                    GroupBox = gb2,
                    Focus = controlePedidoProduto
                },
                new Grupo
                {
                    Radio = rb3,
                    GroupBox = gb3,
                    Focus = null
                },
                new Grupo
                {
                    Radio = rb4,
                    GroupBox = gb4,
                    Focus = null
                },
                new Grupo
                {
                    Radio = rb5,
                    GroupBox = gb5,
                    Focus = txtCodigoFormaPagamento
                },
                new Grupo
                {
                    Radio = rb6,
                    GroupBox = gb6,
                    Focus = null
                },
                new Grupo
                {
                    Radio = rb7,
                    GroupBox = gb7,
                    Focus = null
                },
                new Grupo
                {
                    Radio = rb8,
                    GroupBox = gb8,
                    Focus = null
                }
            };

            lblTotalPedido.Text = "R$ 0,00";

            TaxaServico = TipoPedido.RetornarTaxaServico(ETipoPedido.Delivery);

            switch (_estado)
            {
                case EstadoDelivery.NaoConfirmado:
                case EstadoDelivery.Novo:
                    rb1.Checked = true;
                    gb1.Visible = true;

                    gb2.Visible =
                        gb3.Visible =
                        gb4.Visible =
                        gb5.Visible =
                        gb6.Visible =
                        gb7.Visible =
                        gb8.Visible =
                        false;

                    rb8.Enabled = false;
                    break;
                case EstadoDelivery.Edicao:
                    rb2.Checked = true;

                    rb1.Enabled =
                        rb2.Enabled =
                        rb4.Enabled =
                        rb5.Enabled =
                        rb6.Enabled =
                        true;

                    rb3.Enabled = possuiTaxaEntrega && !AppDelivery;

                    rb7.Enabled = false;

                    gb2.Visible = true;

                    gb1.Visible =
                       gb3.Visible =
                       gb4.Visible =
                       gb5.Visible =
                       gb6.Visible =
                       gb7.Visible =
                       gb8.Visible =
                       false;

                    rb8.Enabled = false;
                    break;
                case EstadoDelivery.Entregador:
                    rb7.Checked = true;

                    rb1.Enabled =
                        rb2.Enabled =
                        rb4.Enabled =
                        rb5.Enabled =
                        rb6.Enabled =
                        rb7.Enabled =
                        true;

                    rb3.Enabled = possuiTaxaEntrega && !AppDelivery;

                    gb7.Visible = true;

                    gb1.Visible =
                        gb2.Visible =
                        gb3.Visible =
                        gb4.Visible =
                        gb5.Visible =
                        gb6.Visible =
                        gb8.Visible =
                        false;

                    rb8.Enabled = false;
                    LoggiOrcamento();
                    break;
                case EstadoDelivery.Finalizar:
                    DefinirEntregador(Pedido1.Entregador);

                    rb1.Enabled =
                        rb2.Enabled =
                        rb3.Enabled =
                        rb4.Enabled =
                        rb6.Enabled =
                        rb7.Enabled = false;

                    gb1.Visible =
                        gb2.Visible =
                        gb3.Visible =
                        gb4.Visible =
                        gb5.Visible =
                        gb6.Visible =
                        gb7.Visible = false;

                    rb8.Checked = true;
                    rb8.Enabled = true;
                    gb8.Visible = true;
                    break;
                default:
                    throw new ExceptionPDV(CodigoErro.EC2D);
            }

            if (_estado != EstadoDelivery.Novo)
            {
                controlePedidoProduto.CarregarProdutos(Pedido1.ListaProduto.Where(pp => pp.Cancelado == false).ToList());
                if (Pedido1.TaxaEntrega?.IDTaxaEntrega != null)
                    Pedido1.TaxaEntrega = TaxaEntrega.Carregar(Pedido1.TaxaEntrega.IDTaxaEntrega.Value);

                ckbTaxaServico.Checked = Pedido1.AplicarServico == true;

                DefinirTaxaEntrega(Pedido1.TaxaEntrega);
                DefinirCliente(Pedido1.Cliente);
                Pedido.AdicionarProdutoServico(Pedido1, ckbTaxaServico.Checked, AC.PDV, AC.Usuario);

                txtCPF.Text = Pedido1.Cliente.Documento1;
                ListarPagamento();
                AtualizarPainel();

                if (Pedido1?.AplicarDesconto == true && Pedido1?.ValorDesconto > 0)
                {
                    ckbDesconto.Checked = true;
                    txtDescontoReais.Text = Pedido1.ValorDesconto.Value.ToString("#,##0.00", _provider);
                    txtDescontoReais.Enabled = txtDescontoPercentual.Enabled = true;
                    AtualizarDescontoReais();
                }
            }

            Entregadores = Entregador.ListarAtivos();
            var enviados = Pedido.ListarEnviados();

            foreach (var entregador in Entregadores)
            {
                var row = new object[]
                {
                    entregador.IDEntregador.Value,
                    $"{entregador.Nome}{(enviados.Any(p => p.Entregador.IDEntregador.Value == entregador.IDEntregador.Value) ? " (com entrega)" : string.Empty)}"
                };
                dgvEntregador.Rows.Add(row);
            }

            if (_estado == EstadoDelivery.Entregador)
                LoggiOrcamento();

            txtCPF.Text = Pedido1.DocumentoCliente;
            if (string.IsNullOrEmpty(Pedido1.DocumentoCliente))
            {
                txtCPF.Text = Pedido1.Cliente?.Documento1;
                ckbVincularCPF.Checked = false;
            }
            else
            {
                ckbVincularCPF.Checked = true;
            }

            if (Pedido1.DtPedidoFechamento != null)
            {
                ckbVincularCPF.Enabled = false;
                txtCPF.Enabled = false;
            }


            Navegacao = new Stack<string>();
            Navegar = true;

            AtualizarExibicao();
            Cursor = Cursors.Default;
            Refresh();
        }

        private void frmNovoDelivery_Shown(object sender, EventArgs e)
        {
            if (_estado == EstadoDelivery.Novo)
                PesquisarCliente();
        }

        private void frmNovoDelivery_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ConfirmarSaida && MessageBox.Show("Suas alterações não serão salvas!\nDeseja realmente fechar?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                e.Cancel = true;
                IrAoUltimoPasso();
            }
        }

        private void frmNovoDelivery_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (controlePedidoProduto?.IsDisposed == false)
                controlePedidoProduto?.Dispose();
        }
        #endregion

        private void lnkVoltar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            VoltarUmaAcao();
        }

        #region cliente
        private void txtDocumento1_Leave(object sender, EventArgs e)
        {
            if (txtDocumento1.Text.Length == 11 && !ValidacaoDocumento.IsCpf(txtDocumento1.Text))
            {
                MessageBox.Show("CPF Inválido!");
                txtDocumento1.Focus();
            }
            else if (txtDocumento1.Text.Length == 14 && !ValidacaoDocumento.IsCnpj(txtDocumento1.Text))
            {
                MessageBox.Show("CNPJ Inválido!");
                txtDocumento1.Focus();
            }
            else if (txtDocumento1.Text.Length != 0 && txtDocumento1.Text.Length != 11 && txtDocumento1.Text.Length != 14)
            {
                MessageBox.Show("CPF/CNPJ Incompleto!");
                txtDocumento1.Focus();
            }
        }

        /// <summary>
        /// Validação do cliente no delivery
        /// </summary>
        /// <remarks>
        /// Código de Teste: T22
        /// </remarks>
        private void btnConfirmarCliente_Click(object sender, EventArgs e)
        {
            var endereco = txtEndereco.Text;
            var numero = txtEnderecoNumero.Text;
            var enderecoVazio = string.IsNullOrWhiteSpace(endereco);
            var numeroVazio = string.IsNullOrWhiteSpace(numero);
            int cep = 0;

            tbCliente cliente;

            if (Pedido1.Cliente.IDCliente > 0)
                cliente = EF.Repositorio.Carregar<tbCliente>(c => c.IDCliente == Pedido1.Cliente.IDCliente.Value);
            else
                cliente = new tbCliente();

            string msg = Cliente.ValidarCliente(cliente.IDCliente,
                txtNomeCompleto.Text, txtEndereco.Text + txtEnderecoNumero, txtComplemento.Text, txtBairro.Text, txtCidade.Text,
                txtDocumento1.Text, null, null, true);

            if (enderecoVazio && numeroVazio)
                msg += "Informe o endereço completo da entrega\n";
            else if (enderecoVazio)
                msg += "Informe o endereço da entrega\n";
            else if (numeroVazio)
                msg += "Informe o número do endereço da entrega\n";
            else if (!string.IsNullOrEmpty(txtCEP.Text) && !int.TryParse(txtCEP.Text, out cep))
                msg += "Informe um CEP válido (somente digitos)";
            else if (cep > 0 && txtCEP.Text.Length < 8)
                msg += "Informe o CEP com 8 dígitos";

            if (BLL.PDV.PossuiLoggi()) // log
                txtEnderecoNumero_TextChanged(sender, e);

            if (!string.IsNullOrWhiteSpace(msg))
            {
                MessageBox.Show(msg, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cliente.NomeCompleto = txtNomeCompleto.Text;
            cliente.Endereco = txtEndereco.Text;
            cliente.EnderecoNumero = txtEnderecoNumero.Text;
            cliente.Complemento = txtComplemento.Text;
            cliente.Bairro = txtBairro.Text;
            cliente.Cidade = txtCidade.Text;
            cliente.Observacao = txtObservacao.Text;
            cliente.Email = txtClienteEmail.Text;
            cliente.RG = txtRG.Text;
            cliente.EnderecoReferencia = txtReferencia.Text;
            cliente.CEP = cep;
            cliente.Bloqueado = ckbBloqueado.Checked;


            if (txtTelefone1DDD.Text != "")
                cliente.Telefone1DDD = Convert.ToInt32(txtTelefone1DDD.Text);
            else
                cliente.Telefone1DDD = 0;

            if (txtTelefone1Numero.Text != "")
                cliente.Telefone1Numero = Convert.ToInt32(txtTelefone1Numero.Text);
            else
                cliente.Telefone1Numero = 0;

            if (txtDocumento1.Text != "")
            {
                cliente.Documento1 = txtDocumento1.Text;
                txtCPF.Text = Pedido1.Cliente.Documento1;
            }
            else
            {
                cliente.Documento1 = null;
                txtCPF.Text = null;
            }

            if (cbbEstado.SelectedItem.ToString() != "")
                cliente.IDEstado = Convert.ToInt32(cbbEstado.SelectedValue);

            if (cliente.IDCliente > 0)
                EF.Repositorio.Atualizar(cliente);
            else
                EF.Repositorio.Inserir(cliente);

            var cliente1 = Cliente.Carregar(cliente.IDCliente);

            DefinirCliente(cliente1);

            rb2.Enabled = true;
            rb2.Checked = true;
        }

        private void llAlterarCliente_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Pedido1.Cliente = null;
            DefinirCliente(null);
            PesquisarCliente();
        }

        private void ckbBloqueado_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbBloqueado.Checked)
                ckbBloqueado.BackColor = Color.Red;
            else
                ckbBloqueado.BackColor = Color.Transparent;
        }
        #endregion

        private void btnConfirmarTaxaEntrega_Click(object sender, EventArgs e)
        {
            if (AppDelivery)
                return;
            else if (dgvTaxaEntrega.SelectedRows.Count > 0)
            {
                var idTaxaEntrega = Convert.ToInt32(dgvTaxaEntrega.SelectedRows[0].Cells[nameof(colID)].Value);
                var taxaEntrega = TaxasEntrega.First(t => t.IDTaxaEntrega == idTaxaEntrega);
                DefinirTaxaEntrega(taxaEntrega);
                rb4.Enabled = true;
                rb4.Checked = true;
            }
            else
            {
                MessageBox.Show("Selecione a taxa de entrega", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region confirmação
        private void llConfirmacaoAlterarCliente_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rb1.Checked = true;
            if (PerguntarAlteracao("o cliente") == DialogResult.Yes)
            {
                DefinirCliente(null);
                PesquisarCliente();
                AtualizarExibicao();
            }
        }

        private void llConfirmacaoAlterarProdutos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rb2.Checked = true;
        }

        private void llConfirmacaoAlterarTaxaEntrega_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rb3.Checked = true;
            if (PerguntarAlteracao("a taxa de entrega") == DialogResult.Yes)
            {
                DefinirTaxaEntrega(null);
            }
        }

        private void btnContinuarConfirmacao_Click(object sender, EventArgs e)
        {
            rb5.Enabled = true;
            rb5.Checked = true;
        }
        #endregion

        #region pagamento
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

        private void cbbFormaPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtValorPagamento.Focus();
        }

        private void txtValorPagamento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && cbbFormaPagamento.SelectedIndex > 0 && (txtValorPagamento.Text == "" || Convert.ToDecimal(txtValorPagamento.Text) > 0))
                AdicionarPagamento();
        }

        private void btnAdicionarPagamento_Click(object sender, EventArgs e)
        {
            //string aux = txtValorPagamento.Text;
            //decimal valor = 0;
            //if (string.IsNullOrEmpty(aux))
            //{

            //}

            //if (cbbFormaPagamento.SelectedIndex > 0 && (txtValorPagamento.Text != "" || Convert.ToDecimal() > 0))
            AdicionarPagamento();
        }

        private void dgvPagamentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var pagamento = Pedido1.ListaPagamento[e.RowIndex];
                if (!string.IsNullOrEmpty(pagamento.Autorizacao))
                {
                    if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) != DialogResult.OK)
                        return;

                    PedidoPagamento.Cancelar(Pedido1.ListaPagamento[e.RowIndex], usuario.IDUsuario.Value);
                }
                else if (pagamento.IDPedidoPagamento.HasValue)
                    PedidoPagamento.Cancelar(pagamento, AC.Usuario.IDUsuario.Value);

                Pedido1.ListaPagamento.RemoveAt(e.RowIndex);
                ListarPagamento();
                AtualizarPainel();
                AtualizarExibicao();
            }
        }

        private void ckbTaxaServico_Click(object sender, EventArgs e)
        {
            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) != DialogResult.OK)
            {
                ckbTaxaServico.Checked = !ckbTaxaServico.Checked;
                Pedido1.UsuarioTaxaServico = usuario;
            }
            AtualizarExibicao();
            AtualizarPainel();
            AtualizarExibicao();
        }

        private void ckbDesconto_Click(object sender, EventArgs e)
        {
            var solicitarSenha = ConfiguracoesSistema.Valores.SolicitarSenhaDesconto;
            var result = DialogResult.None;

            if (solicitarSenha)
                result = NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario);

            if (!solicitarSenha || result == DialogResult.OK)
            {
                if (ckbDesconto.Checked)
                {
                    var tipoDescontoResult = DialogResult.None;

                    var solicitarTipoDesconto = ConfiguracoesSistema.Valores.SolicitarTipoDesconto;
                    using (var frmTipoDesconto = new frmTipoDesconto())
                    {
                        if (solicitarTipoDesconto)
                            tipoDescontoResult = frmTipoDesconto.ShowDialog();

                        if (solicitarTipoDesconto && tipoDescontoResult == DialogResult.OK)
                        {
                            Pedido1.TipoDesconto = new TipoDescontoInformation { IDTipoDesconto = frmTipoDesconto.IDTipoDesconto };
                            txtDescontoPercentual.Enabled = true;
                            txtDescontoReais.Enabled = true;
                        }
                        else if (solicitarTipoDesconto && tipoDescontoResult != DialogResult.OK)
                        {
                            ckbDesconto.Checked = !ckbDesconto.Checked;
                        }
                        else
                        {
                            txtDescontoPercentual.Enabled = true;
                            txtDescontoReais.Enabled = true;
                        }
                    }
                }
                else
                {
                    Pedido1.TipoDesconto = null;
                    txtDescontoPercentual.Text = "0,00";
                    AtualizarDescontoPercentual();

                    AtualizarPainel();
                    AtualizarExibicao();

                    txtDescontoReais.Enabled = false;
                    txtDescontoPercentual.Enabled = false;
                }
            }
            else
            {
                ckbDesconto.Checked = !ckbDesconto.Checked;
                if (!ckbDesconto.Checked)
                {
                    Pedido1.TipoDesconto = null;
                    txtDescontoPercentual.Text = "0,00";
                    AtualizarDescontoPercentual();

                    AtualizarPainel();
                    AtualizarExibicao();

                    txtDescontoReais.Enabled = false;
                    txtDescontoPercentual.Enabled = false;
                }
            }
        }

        private void txtDescontoPercentual_TextChanged(object sender, EventArgs e)
        {
            if (txtDescontoPercentual.Focused)
            {
                AtualizarDescontoPercentual();

                AtualizarPainel();
                AtualizarExibicao();
            }
        }

        private void txtDescontoReais_TextChanged(object sender, EventArgs e)
        {
            if (txtDescontoReais.Focused)
            {
                AtualizarDescontoReais();

                AtualizarPainel();
                AtualizarExibicao();
            }
        }

        private void btnConfirmarPagamento_Click_1(object sender, EventArgs e)
        {
            var valorPago = Pedido1.ListaPagamento.Sum(l => l.Valor.Value);
            var valorPendente = TotalEntrega + ValorServico - valorPago - (Pedido1.ValorDesconto ?? 0);

            if (valorPendente > 0)
            {
                MessageBox.Show("Existem pagamentos pendentes", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigoFormaPagamento.Focus();
                return;
            }

            if (_estado == EstadoDelivery.Finalizar)
            {
                rb8.Checked = true;
            }
            else
            {
                rb6.Enabled = true;
                rb6.Checked = true;
            }
        }
        #endregion

        #region resumo
        private void llResumoAlterarCliente_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rb1.Checked = true;
            if (PerguntarAlteracao("o cliente") == DialogResult.Yes)
            {
                DefinirCliente(null);
                PesquisarCliente();
                AtualizarExibicao();
            }
        }

        private void llResumoAlterarTaxaEntrega_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rb3.Checked = true;
            if (PerguntarAlteracao("a taxa de entrega") == DialogResult.Yes)
            {
                DefinirTaxaEntrega(null);
            }
        }

        private void llResumoAlterarProdutos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rb2.Checked = true;
        }

        private void btnCriarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validar())
                {
                    if (_estado == EstadoDelivery.NaoConfirmado || _estado == EstadoDelivery.Novo || _estado == EstadoDelivery.Edicao)
                    {
                        SalvarPedido();
                        ConfirmarSaida = false;
                        if (ConfiguracoesSistema.Valores.ImprimirViaExpedicao == "NOVO")
                            OrdemProducaoServices.GerarViaExpedicao(Pedido1.IDPedido.Value, ConfiguracoesSistema.Valores.IDAreaViaExpedicao);

                        Close();
                    }
                    else
                    {
                        rb7.Enabled = true;
                        rb7.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Logs.ErroBox(CodigoErro.E100, ex);
            }
        }
        private void llResumoAlterarFormaPagamento_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rb5.Checked = true;
        }
        #endregion

        #region envio/entregador
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (dgvEntregador.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione o Entregador", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }

            try
            {
                var idEntregador = Convert.ToInt32(dgvEntregador.SelectedRows[0].Cells[nameof(cellIDEntregador)].Value);
                var entregador = Entregadores.First(et => et.IDEntregador == idEntregador);

                DefinirEntregador(entregador);

                if (!Validar())
                    return;

                Pedido1.DtEnvio = DateTime.Now;

                if (ckbVincularCPF.Checked)
                {
                    Pedido1.DocumentoCliente = txtCPF.Text.Trim();
                }
                else
                {
                    Pedido1.DocumentoCliente = "";
                }

                SalvarPedido();

                Pedido.AdicionarProdutoServico(Pedido1, ckbTaxaServico.Checked, AC.PDV, AC.Usuario);

                //var impressoraSAT = ConfiguracoesCaixa.Valores.ModeloImpressora;
                //var areaImpressaoSat = AreaImpressao.AreaImpressaoSAT();

                //var result = DialogResult.None;
                Pedido1.Caixa = frmPrincipal.Caixa1;

                // Primeira impressão (FISCAL)
                FecharVenda telaFechar = new FecharVenda(Pedido1);

                //if (areaImpressaoSat != null)
                //    telaFechar.ComAreaDeImpressao(areaImpressaoSat);
                //else
                telaFechar.NaImpressora(ConfiguracoesCaixa.Valores.ModeloImpressora);

                var result = telaFechar.Fechar(false, true, false);

                if (result != DialogResult.OK)
                    return;

                if (ConfiguracoesSistema.Valores.ImprimirViaExpedicao == "ENTREGA")
                {
                    OrdemProducaoServices.GerarViaExpedicao(Pedido1.IDPedido.Value, ConfiguracoesSistema.Valores.IDAreaViaExpedicao);
                }
                else  // não ou novo
                {
                    if (ConfiguracoesCaixa.Valores.GerenciadorImpressao != ETipoGerenciadorImpressao.SemImpressora)
                    {
                        try
                        {
                            switch (ConfiguracoesCaixa.Valores.GerenciadorImpressao)
                            {
                                case ETipoGerenciadorImpressao.ACBr:
                                case ETipoGerenciadorImpressao.ECFBemafii:

                                    if (ConfiguracoesSistema.Valores.ImprimirViaMotoboy)
                                        frmPrincipal.Impressora1.GerarCupom(Pedido1, false);

                                    break;

                                case ETipoGerenciadorImpressao.ImpressoraWindows:
                                case ETipoGerenciadorImpressao.SAT:

                                    if (ConfiguracoesSistema.Valores.ImprimirViaMotoboy)
                                        ContaServices.ImprimirConta(ConfiguracoesCaixa.Valores.ModeloImpressora, Pedido1);

                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logs.ErroBox(CodigoErro.E300, ex);
                        }
                    }

                    try
                    {
                        if (ConfiguracoesSistema.Valores.ImprimirViaControleDelivery)
                            frmPrincipal.Impressora1.GerarCupomMotoboy(Pedido1);
                    }
                    catch (Exception ex)
                    {
                        Logs.ErroBox(CodigoErro.A300, ex);
                    }
                }

                Pedido.AlterarStatus(Pedido1.IDPedido.Value, EStatusPedido.Enviado);
                ConfirmarSaida = false;
                Close();
            }
            catch (Exception ex)
            {
                BLL.Logs.ErroBox(CodigoErro.E100, ex);
                Pedido1 = Pedido.CarregarCompleto(Pedido1.IDPedido.Value);
            }
        }
        #endregion

        #region receber/finalizar
        private void llFinalizarAlterarPagamento_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rb5.Checked = true;
        }


        private void btnFinalizarPedido_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                try
                {
                    SalvarPedido();
                    Pedido.AlterarStatus(Pedido1.IDPedido.Value, EStatusPedido.Finalizado);

                    if (Pedido1.TipoPedido.IDTipoPedido != 30) // || ConfiguracoesSistema.Valores.ImprimirViaMotoboy)
                    {
                        switch (ConfiguracoesCaixa.Valores.GerenciadorImpressao)
                        {
                            case ETipoGerenciadorImpressao.ACBr:
                            case ETipoGerenciadorImpressao.ECFBemafii:
                                frmPrincipal.Impressora1.GerarCupom(Pedido1, false);
                                break;
                            case ETipoGerenciadorImpressao.ImpressoraWindows:
                            case ETipoGerenciadorImpressao.SAT:
                                ContaServices.ImprimirConta(ConfiguracoesCaixa.Valores.ModeloImpressora, Pedido1);
                                break;
                        }
                    }

                    ConfirmarSaida = false;
                    frmPedidos.GUIDIdentificacao_selecionado = null;
                    Close();
                }
                catch (Exception ex)
                {
                    var msg = new StringBuilder();
                    msg.AppendLine(ex.Message);
                    msg.AppendLine(ex.StackTrace);
                    var inner = ex.InnerException;

                    while (inner != null)
                    {
                        msg.AppendLine(inner.Message);
                        msg.AppendLine(inner.StackTrace);

                        inner = inner.InnerException;
                    }
                    MessageBox.Show(msg.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #endregion

        #region métodos

        private void AtualizarExibicao()
        {
            lnkVoltar.Visible = Navegacao?.Count > 0;

            #region cliente
            if (Pedido1.Cliente != null)
            {
                gbEditarCliente.Visible = true;
                txtNomeCompleto.Text = Pedido1.Cliente.NomeCompleto;
                txtTelefone1DDD.Text = Pedido1.Cliente.Telefone1DDD.ToString();
                txtTelefone1Numero.Text = Pedido1.Cliente.Telefone1Numero.ToString();
                txtDocumento1.Text = Pedido1.Cliente.Documento1;
                txtRG.Text = Pedido1.Cliente.RG;
                txtEndereco.Text = Pedido1.Cliente.Endereco;
                txtEnderecoNumero.Text = Pedido1.Cliente.EnderecoNumero;
                txtComplemento.Text = Pedido1.Cliente.Complemento;
                txtBairro.Text = Pedido1.Cliente.Bairro;
                txtCidade.Text = Pedido1.Cliente.Cidade;
                txtObservacao.Text = Pedido1.Cliente.Observacao;
                txtClienteEmail.Text = Pedido1.Cliente.Email;
                txtReferencia.Text = Pedido1.Cliente.EnderecoReferencia;
                ckbBloqueado.Checked = Pedido1.Cliente.Bloqueado ?? false;

                if (Pedido1.Cliente.CEP > 0)
                    txtCEP.Text = Pedido1.Cliente.CEP.Value.ToString("00000000");
                else
                    txtCEP.Text = "";

                if (Pedido1.Cliente.Estado != null && Pedido1.Cliente.Estado.IDEstado != null)
                    cbbEstado.SelectedValue = Pedido1.Cliente.Estado.IDEstado;

                if (Pedido1.Cliente.Bloqueado == true)
                    ckbBloqueado.BackColor = Color.Red;
                else
                    ckbBloqueado.BackColor = Color.Transparent;

                rb1.Text = Pedido1.Cliente.NomeCompleto;
                lblResumoNomeCliente.Text = lblConfirmacaoNome.Text = lblFinalizarNome.Text = Pedido1.Cliente.NomeCompleto;
                lblResumoTelefoneCliente.Text = lblConfirmacaoTelefone.Text = lblFinalizarTelefone.Text = Pedido1.Cliente.Telefone1;
                lblResumoEnderecoObs.Text = lblConfirmacaoObs.Text = lblFinalizarObs.Text = $@"{Pedido1.Cliente.Endereco} {(!string.IsNullOrWhiteSpace(Pedido1.Cliente.EnderecoNumero) ? $" - {Pedido1.Cliente.EnderecoNumero}" : "")}
{(!string.IsNullOrWhiteSpace(Pedido1.Cliente.Complemento) ? $"{Pedido1.Cliente.Complemento}\n" : "")}{(!string.IsNullOrWhiteSpace(Pedido1.Cliente.Bairro) ? $"{Pedido1.Cliente.Bairro}\n" : "")}{(!string.IsNullOrWhiteSpace(Pedido1.Cliente.Cidade) ? $"{Pedido1.Cliente.Cidade}\n" : "")}{(!string.IsNullOrWhiteSpace(Pedido1.Cliente.Estado?.Sigla) ? $"{Pedido1.Cliente.Estado.Sigla}\n" : "")}
{Pedido1.Cliente.Observacao}";
            }
            else
            {
                lblResumoNomeCliente.Text = lblResumoTelefoneCliente.Text = lblResumoEnderecoObs.Text = string.Empty;
                rb1.Text = string.Empty;
                gbEditarCliente.Visible = false;
            }

            lblResumoNomeCliente.Visible = Pedido1.Cliente != null;
            lblResumoTelefoneCliente.Visible = Pedido1.Cliente != null;
            lblResumoEnderecoObs.Visible = Pedido1.Cliente != null;
            #endregion

            #region produtos
            var total = TotalProdutos;
            Pedido1.ValorServico = ValorServico;
            txtTaxaServicoReais.Text = Pedido1.ValorServico.Value.ToString("#,##0.00", _provider);
            txtTaxaServicoPercentual.Text = TaxaServico.ToString("0.00", _provider);

            var quantidade = controlePedidoProduto.ListaPedidoProduto.Where(pp => pp.Status != StatusModel.Excluido).Sum(pp => pp.Quantidade);
            if (quantidade == 1)
                rb2.Text = "1 Produto";
            else
                rb2.Text = $"{(quantidade ?? 0).ToString("0")} Produtos";


            var stringTotal = total.ToString("R$ #,##0.00", _provider);

            rb2.Text += $"\n{stringTotal}";
            lblConfirmacaoTotalProdutos.Text = lblTotalProdutos.Text = lblFinalizarTotalProdutos.Text = total.ToString("R$ #,##0.00", _provider);
            AtualizarListaProdutos();
            #endregion

            #region taxa entrega
            //if (AppDelivery && Pedido1.ValorEntrega > 0 && Pedido1.OrigemPedido.IDOrigemPedido == (Int32)EOrigemPedido.ifood)
            //{
            //    var stringValor = Pedido1.ValorEntrega.Value.ToString("R$ #,##0.00", _provider);
            //    rb3.Text = $"Taxa de Entrega\n{stringValor}";
            //    lblTaxaEntrega.Text = $"iFood: {stringValor}";
            //    lblResumoTaxaEntrega.Text = lblFinalizarTaxaEntrega.Text = stringValor;
            //    lblTaxaEntrega.Visible = true;
            //    llConfirmacaoAlterarTaxaEntrega.Visible = false;
            //    TaxaSelecionada = true;
            //}
            //else if (AppDelivery && Pedido1.ValorEntrega > 0 && Pedido1.OrigemPedido.IDOrigemPedido == (Int32)EOrigemPedido.deliveryOnline)
            //{
            //    var stringValor = Pedido1.ValorEntrega.Value.ToString("R$ #,##0.00", _provider);
            //    rb3.Text = $"Taxa de Entrega\n{stringValor}";
            //    lblTaxaEntrega.Text = $"iFood: {stringValor}";
            //    lblResumoTaxaEntrega.Text = lblFinalizarTaxaEntrega.Text = stringValor;
            //    lblTaxaEntrega.Visible = true;
            //    llConfirmacaoAlterarTaxaEntrega.Visible = false;
            //    TaxaSelecionada = true;
            //}
            if (Pedido1.TaxaEntrega != null)
            {
                var stringValor = Pedido1.TaxaEntrega.Valor.Value.ToString("R$ #,##0.00", _provider);
                rb3.Text = $"Taxa de Entrega\n{stringValor}";
                lblTaxaEntrega.Text = $"{Pedido1.TaxaEntrega.Nome}: {stringValor}";
                lblResumoTaxaEntrega.Text = lblFinalizarTaxaEntrega.Text = stringValor;
                lblTaxaEntrega.Visible = true;
                TaxaSelecionada = true;
            }
            else
            {
                //teste sem taxa de entrega
                rb3.Text = "Taxa de Entrega";
                lblTaxaEntrega.Text = string.Empty;
                lblTaxaEntrega.Visible = false;
                dgvTaxaEntrega.Visible = true;
                TaxaSelecionada = false;
                lblResumoTaxaEntrega.Text = string.Empty;
            }
            string stringTaxaEntrega;

            if (AppDelivery)
                stringTaxaEntrega = Pedido1.ValorEntrega.Value.ToString("R$ #,##0.00", _provider);
            else
                stringTaxaEntrega = Pedido1.TaxaEntrega?.Valor != null ? Pedido1.TaxaEntrega.Valor.Value.ToString("R$ #,###.00", _provider) : "R$ 0,00";

            lblConfirmacaoTaxaEntrega.Text = stringTaxaEntrega;

            llResumoAlterarTaxaEntrega.Visible = TaxaSelecionada && !AppDelivery;
            lblResumoTaxaEntrega.Visible = TaxaSelecionada;
            if (Pedido1.ValorDesconto.Value > 0)
            {
                lblResumoDesconto.Visible = lblFinalizarDesconto.Visible = true;
                lblResumoDesconto.Text = lblFinalizarDesconto.Text = Pedido1.ValorDesconto.Value.ToString("Desconto: -R$ #,##0.00", _provider);
            }
            else
                lblResumoDesconto.Visible = lblFinalizarDesconto.Visible = false;
            #endregion

            #region acrescimo
            if (ckbTaxaServico.Checked)
            {
                lblAcrescimo.Visible = lblFinalizarAcrescimo.Visible = true;
                lblAcrescimo.Text = lblFinalizarAcrescimo.Text = Pedido1.ValorServico.Value.ToString("Acréscimo: +R$ #,##0.00", _provider);
            }
            else
                lblAcrescimo.Visible = lblFinalizarAcrescimo.Visible = false;
            #endregion

            #region pagamentos

            #endregion

            #region total

            var totalGeral = TotalEntrega + ValorServico;
            totalGeral = totalGeral - Pedido1.ValorDesconto.Value;

            lblTotalPedido.Text = totalGeral.ToString("R$ #,##0.00", _provider);
            #endregion

            #region entregador
            if (Pedido1.Entregador != null)
                lblEntregador.Text = Pedido1.Entregador.Nome;
            else
                lblEntregador.Text = string.Empty;

            #endregion

            if (_estado == EstadoDelivery.NaoConfirmado)
                btnCriarPedido.Text = "CONFIRMAR PEDIDO";
            if (_estado == EstadoDelivery.Novo)
                btnCriarPedido.Text = "CRIAR PEDIDO";
            else if (_estado == EstadoDelivery.Edicao)
                btnCriarPedido.Text = "EDITAR PEDIDO";
            else
                btnCriarPedido.Text = "CONFIRMAR";


            if (Alterado)
            {
                VoltarATaxaDeEntrega();
                Alterado = false;
            }
        }

        private void DefinirTaxaEntrega(TaxaEntregaInformation taxaEntrega)
        {
            if (AppDelivery)
                return;

            if (Pedido1.TaxaEntrega?.IDTaxaEntrega != taxaEntrega?.IDTaxaEntrega)
            {
                VoltarAConfirmacao();
            }
            Pedido1.TaxaEntrega = taxaEntrega;
            Pedido1.ValorEntrega = taxaEntrega?.Valor;
            AtualizarPainel();
            AtualizarExibicao();
        }

        private void AtualizarListaProdutos()
        {
            dgvProdutos.Rows.Clear();
            dgvConfirmacaoProdutos.Rows.Clear();
            dgvProdutosPagamento.Rows.Clear();
            dgvFinalizarProdutos.Rows.Clear();
            foreach (var item in controlePedidoProduto.ListaPedidoProduto.Where(pp => pp.Status != StatusModel.Excluido).OrderBy(l => l.Produto.Nome).ToList())
            {
                if (item.ValorUnitario > 0)
                {
                    var row = new object[] { $"{item.Quantidade.Value.ToString("0.000", _provider)}×{item.Produto.Nome}", item.ValorUnitario.Value.ToString("#,##0.00", _provider) };
                    dgvConfirmacaoProdutos.Rows.Add(row);
                    dgvProdutos.Rows.Add(row);
                    dgvProdutosPagamento.Rows.Add(row);
                    dgvFinalizarProdutos.Rows.Add(row);

                    if (item.ListaModificacao != null)
                    {
                        foreach (var modificacao in item.ListaModificacao)
                        {
                            row = new object[] { " - " + modificacao.Produto.Nome, modificacao.ValorUnitario.Value.ToString("#,##0.00", _provider) };
                            dgvProdutos.Rows.Add(row);
                            dgvConfirmacaoProdutos.Rows.Add(row);
                            dgvProdutosPagamento.Rows.Add(row);
                            dgvFinalizarProdutos.Rows.Add(row);
                        }
                    }
                }
                else if (item.ListaModificacao != null)
                {
                    foreach (var m in item.ListaModificacao.Where(mod => mod.ValorUnitario > 0))
                    {
                        var row = new object[] { "", $"{item.Quantidade.Value.ToString("0.000", _provider)}×{item.Produto.Nome} - {m.Produto.Nome}", m.ValorUnitario.Value.ToString("#,##0.00", _provider) };
                        dgvConfirmacaoProdutos.Rows.Add(row);
                        dgvProdutos.Rows.Add(row);
                        dgvProdutosPagamento.Rows.Add(row);
                        dgvFinalizarProdutos.Rows.Add(row);
                    }
                }
            }

            foreach (DataGridViewRow row in dgvProdutos.Rows)
            {
                row.Cells[1].Style = cellStyle;
            }

            foreach (DataGridViewRow row in dgvConfirmacaoProdutos.Rows)
            {
                row.Cells[1].Style = cellStyle;
            }

            foreach (DataGridViewRow row in dgvFinalizarProdutos.Rows)
            {
                row.Cells[1].Style = cellStyle;
            }

            dgvProdutos.ClearSelection();
            dgvConfirmacaoProdutos.ClearSelection();
            dgvProdutosPagamento.ClearSelection();
            dgvFinalizarProdutos.ClearSelection();

            llResumoAlterarProdutos.Visible = controlePedidoProduto.ListaPedidoProduto.Count != 0;
        }

        private void CarregarTipoPagamento()
        {
            var patagmentos = BLL.TipoPagamento.ListarAtivos();

            cbbFormaPagamento.Items.Add(new TipoPagamentoInformation());
            foreach (var item in patagmentos)
            {
                item.Nome = string.Format($"({item.IDTipoPagamento.Value.ToString("00")}) {item.Nome}");
                cbbFormaPagamento.Items.Add(item);
            }
            cbbFormaPagamento.SelectedIndex = 0;
        }

        private static void AlterarVisibilidade()
        {
            foreach (var grupo in Grupos)
            {
                if (grupo.Radio.Checked)
                {
                    grupo.GroupBox.Show();
                    grupo.Focus?.Focus();
                }
                else
                    grupo.GroupBox.Hide();
            }
        }

        private void DefinirCliente(ClienteInformation cliente)
        {
            Pedido1.Cliente = cliente;
            ClienteSelecionado = cliente != null;
            AtualizarExibicao();
        }

        private void DefinirEntregador(EntregadorInformation entregador)
        {
            Pedido1.Entregador = entregador;
            Pedido1.DtEnvio = DateTime.Now;
            dgvEntregador.ClearSelection();
            foreach (DataGridViewRow row in dgvEntregador.Rows)
            {
                var idEntregador = Convert.ToInt32(row.Cells[nameof(cellIDEntregador)].Value);
                if (Pedido1.Entregador?.IDEntregador == idEntregador)
                    row.Selected = true;
                break;
            }
        }

        private void LoggiOrcamento()
        {
            if (!BLL.PDV.PossuiLoggi()
             //|| loggiTask != null // Se ja estiver em execução, ão faz nada
             || dgvEntregador.SelectedRows.Count == 0
             || Pedido1.IDPedido == null
             || Pedido1.Cliente == null
             //|| loggiOrcamento != null
             || !(Pedido1.TaxaEntrega?.IDTamanhoPacote > 0))
                return;

            var idEntregador = Convert.ToInt32(dgvEntregador.SelectedRows[0].Cells[nameof(cellIDEntregador)].Value);
            var entregador = Entregadores.First(et => et.IDEntregador == idEntregador);

            Pedido1.Entregador = entregador;
            Pedido1.DtEnvio = DateTime.Now;

            //loggiTask = Task.Run(() =>
            //{
            //    lIgnorado = false;

            //    loggiOrcamento = LoggiDelivery.Orcamento(
            //        Pedido1.TaxaEntrega.IDTamanhoPacote.Value,
            //        (Pedido1.Cliente.CEP > 0 ? Pedido1.Cliente.CEP.Value.ToString("00000000") : Pedido1.Cliente.Endereco) + ", " + Pedido1.Cliente.EnderecoNumero + " " + Pedido1.Cliente.Complemento,
            //        "Retirar pedido #" + Pedido1.IDPedido,
            //        "Entregar para " + Pedido1.Cliente.NomeCompleto);

            //    loggiTask = null;
            //});
        }

        private void PesquisarCliente()
        {
            using (var frmPesquisarCliente = new frmPesquisarCliente())
            {
                frmPesquisarCliente.Delivery = true;
                frmPesquisarCliente.ShowDialog();
                DefinirCliente((frmPesquisarCliente.Cliente1?.IDCliente == null ? null : frmPesquisarCliente.Cliente1));

                if (!ClienteSelecionado && _estado == EstadoDelivery.Novo)
                {
                    ConfirmarSaida = false;
                    Close();
                }

                if (!ClienteSelecionado && _estado != EstadoDelivery.Novo)
                {
                    MessageBox.Show("Informe o cliente", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PesquisarCliente();
                }
                txtNomeCompleto.Focus();
            }
        }

        public static DialogResult PerguntarAlteracao(string oQue)
        {
            return MessageBox.Show($"Deseja alterar {oQue}?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void AdicionarPagamento()
        {
            PedidoPagamentoInformation pagamento = new PedidoPagamentoInformation();
            pagamento.Excluido = false;

            if (cbbFormaPagamento.SelectedIndex < 1 && cbbFormaPagamento.Items.Count > 0)
                pagamento.TipoPagamento = (TipoPagamentoInformation)cbbFormaPagamento.Items[1];
            else
                pagamento.TipoPagamento = (TipoPagamentoInformation)cbbFormaPagamento.SelectedItem;


            var valorPago = Pedido1.ListaPagamento.Sum(l => l.Valor.Value);

            if (txtValorPagamento.Text == "")
            {
                pagamento.Valor = TotalEntrega + ValorServico - Pedido1.ValorDesconto.Value - valorPago;
            }
            else
            {
                pagamento.Valor = Convert.ToDecimal(txtValorPagamento.Text);
            }

            valorPago += pagamento.Valor.Value;
            var valorPagoDinheiro = Pedido1.ListaPagamento.Where(l => l.TipoPagamento.IDTipoPagamento == 1).Sum(l => l.Valor.Value);


            if (pagamento.TipoPagamento.IDTipoPagamento == 1)
                valorPagoDinheiro += pagamento.Valor.Value;

            var valorPendente = TotalEntrega + ValorServico - valorPago;

            if ((valorPagoDinheiro + valorPendente) < 0)
            {
                MessageBox.Show("Não é permitido voltar troco maior que o valor pago em Dinheiro!");
            }
            else if (PagamentoVenda.Efetiva(Pedido1, pagamento, valorPendente))
            {
                Pedido1.ListaPagamento.Add(pagamento);
                cbbFormaPagamento.SelectedIndex = 0;
                txtValorPagamento.Text = "";

                txtCodigoFormaPagamento.Text = "";
                txtCodigoFormaPagamento.Focus();
            }
            ListarPagamento();
            AtualizarPainel();
            AtualizarExibicao();
        }

        private void AtualizarPainel()
        {
            lblPagamentoValorProdutos.Text = TotalProdutos.ToString("#,##0.00", _provider);
            var total = TotalEntrega + ValorServico - Pedido1.ValorDesconto.Value;
            var valorPendente = total - ValorPago;

            lblPagamentoValorTotal.Text = total.ToString("#,##0.00", _provider);
            lblPagamentoValorPago.Text = ValorPago.ToString("#,##0.00", _provider);

            if (valorPendente <= 0.009m)
            {
                gbxValorPendente.Text = "TROCO";
                Troco = (valorPendente * -1);
                lblPagamentoValorPendente.Text = Troco.ToString("#,##0.00", _provider);
                lblPagamentoValorPendente.ForeColor = Color.Green;
            }
            else
            {
                gbxValorPendente.Text = "FALTA PAGAR";
                Troco = 0;
                lblPagamentoValorPendente.Text = valorPendente.ToString("#,##0.00", _provider);
                lblPagamentoValorPendente.ForeColor = Color.Red;
            }

            if (Troco > 0)
            {
                lblResumoTroco.Visible = lblFinalizarTroco.Visible = true;
                lblResumoTroco.Text = lblFinalizarTroco.Text = Troco.ToString("Troco: #,##0.00", _provider);
            }
            else
            {
                lblResumoTroco.Visible = lblFinalizarTroco.Visible = false;
            }
        }

        private void ListarPagamento()
        {
            //var list = Pedido1.ListaPagamento?.Where(l => l.Status != StatusModel.Excluido)?.Select(l => new { TipoPagamento = l.TipoPagamento.Nome + (string.IsNullOrEmpty(l.Autorizacao) ? "" : " (PAGO)"), l.Valor });
            var list = Pedido1.ListaPagamento?.Where(l => l.Status != StatusModel.Excluido)
                .Select(l => new
                {
                    TipoPagamento = l.TipoPagamento.Nome + $"\r\n{l.Autorizacao}",
                    l.Valor
                });

            dgvPagamentos.DataSource = list.ToArray();

            var ds = Pedido1.ListaPagamento.Where(l => l.Status != StatusModel.Excluido)
                .Select(p => new
                {
                    Pagamento = p.TipoPagamento.Nome + $"\r\n{p.Autorizacao}",
                    Valor = p.Valor.Value.ToString("R$ #,##0.00")
                }).ToArray();

            dgvResumoFormasPagamento.DataSource = dgvFinalizarPagamento.DataSource = ds;

            dgvResumoFormasPagamento.ClearSelection();
            dgvFinalizarPagamento.ClearSelection();

            llResumoAlterarFormaPagamento.Visible = Pedido1.ListaPagamento.Count != 0;
        }

        private bool Validar()
        {
            var msg = string.Empty;

            #region validação cliente
            if (Pedido1.Cliente == null)
            {
                msg += "Selecione o cliente\n";
                rb1.ImageIndex = 2;
            }
            #endregion

            #region validação taxa entrega
            if (dgvTaxaEntrega.DataSource != null && Pedido1.TaxaEntrega == null)
            {
                msg += "Selecione a taxa de entrega\n";
                rb3.ImageIndex = 2;
            }
            //            else if (_estado == EstadoDelivery.Entregador && Pedido1.Entregador?.IDGateway == (int)EGateway.Loggi)
            //            {
            //                if (loggiOrcamento == null)
            //                {
            //                    LoggiOrcamento();
            //                    loggiTask?.Wait(5000);
            //                }

            //                if (loggiOrcamento == null)
            //                    msg += "Sem resposta da Loggi\r\n";

            //                else if (string.IsNullOrEmpty(loggiOrcamento.id))
            //                    msg += "Loggi ERRO: " + loggiOrcamento.ToString() + "\r\n";

            //                else if (loggiOrcamento.waypoints.Length == 2
            //                    && loggiOrcamento.waypoints[0]?.address_data.formatted_address != null
            //                    && loggiOrcamento.waypoints[1]?.address_data.formatted_address != null)
            //                {
            //                    string info = $@"Confirma entrega via Loggi?

            //Origem:
            //{loggiOrcamento.waypoints[0].address_data.formatted_address}

            //Destino:
            //{loggiOrcamento.waypoints[1].address_data.formatted_address}";

            //                    rb1.Checked = true;
            //                    AlterarVisibilidade();
            //                    Application.DoEvents();

            //                    if (MessageBox.Show(info, "Loggi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
            //                        msg += "Loggi: Não foi confirmado";
            //                    else
            //                    {
            //                        var result = LoggiDelivery.Confirmar(loggiOrcamento.id);

            //                        if (result.errors != null)
            //                            msg = result.errors.ToString();

            //                        if (result.error_message != null)
            //                            msg += " " + result.error_message;

            //                        if (result.id == 0)
            //                        {
            //                            Logs.Erro(CodigoErro.EE21, result.last_request + "\r\n" + result.last_result, loggiOrcamento.ToString());
            //                            msg = "Não foi possível realizar o pedido na Loggi\r\n" + msg;
            //                        }
            //                        else
            //                            MessageBox.Show("Pedido Loggi #" + result.id + " criado", "Loggi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //                else
            //                    msg += "Loggi ERRO: Não foi possível obter os endereços de origem e destino da entrega";
            //            }
            #endregion

            var existemProdutos = controlePedidoProduto.ListaPedidoProduto.Count != 0;

            #region validação produtos
            if (!existemProdutos)
            {
                msg += "Selecione os produtos\n";
                rb2.ImageIndex = 2;
            }
            #endregion

            #region validação pagamentos
            var pagamentos = Pedido1.ListaPagamento.Where(p => p.Status != StatusModel.Excluido).ToList();

            if (existemProdutos && ValorPago < (TotalEntrega + ValorServico - (Pedido1.ValorDesconto ?? 0)))
            {
                msg += "Existem valores pendentes\n";
                rb5.ImageIndex = 2;
            }

            #endregion

            if (!string.IsNullOrWhiteSpace(msg))
            {
                MessageBox.Show(msg, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void AtualizarDescontoPercentual()
        {
            var descontoPercentual = 0m;
            var descontoReais = 0m;

            if (!string.IsNullOrWhiteSpace(txtDescontoPercentual.Text))
                descontoPercentual = Convert.ToDecimal(txtDescontoPercentual.Text);

            if (descontoPercentual < 100)
            {
                descontoReais = (TotalEntrega + ValorServico) * descontoPercentual / 100m;
            }
            else
            {
                descontoReais = (TotalEntrega + ValorServico);
            }

            descontoReais = Math.Truncate(descontoReais * 100m) / 100m;
            txtDescontoReais.Text = descontoReais.ToString("#,##0.00", _provider);
            Pedido1.ValorDesconto = descontoReais;
        }

        private void AtualizarDescontoReais()
        {
            var descontoPercentual = 0m;
            var descontoReais = 0m;

            if (!string.IsNullOrWhiteSpace(txtDescontoReais.Text))
                descontoReais = Convert.ToDecimal(txtDescontoReais.Text);

            if (descontoReais < (TotalEntrega + ValorServico))
                descontoPercentual = descontoReais * 100m / (TotalEntrega + ValorServico);
            else
            {
                descontoReais = (TotalEntrega + ValorServico);
                descontoPercentual = 100;
            }

            txtDescontoPercentual.Text = descontoPercentual.ToString("#,##0.00", _provider);

            Pedido1.ValorDesconto = Math.Truncate(descontoReais * 100m) / 100m;
        }

        private void VoltarATaxaDeEntrega()
        {
            rb3.Enabled =
            rb4.Enabled =
            rb5.Enabled =
            rb6.Enabled =
            rb7.Enabled =
            rb8.Enabled =
            false;
            rb2.Checked = true;
        }

        private void VoltarAConfirmacao()
        {
            rb5.Enabled =
            rb6.Enabled =
            rb7.Enabled =
            rb8.Enabled =
            false;
        }

        private void IrAoUltimoPasso()
        {
            if (VerificarRadioButton(rb8))
                return;
            if (VerificarRadioButton(rb7))
                return;
            if (VerificarRadioButton(rb6))
                return;
            if (VerificarRadioButton(rb5))
                return;
            if (VerificarRadioButton(rb4))
                return;
            if (VerificarRadioButton(rb3))
                return;
            if (VerificarRadioButton(rb2))
                return;
            if (VerificarRadioButton(rb1))
                return;
        }

        private static bool VerificarRadioButton(RadioButton rb)
        {
            if (rb.Enabled)
                rb.Checked = true;
            return rb.Checked;
        }

        private void SalvarPedido()
        {
            Pedido1.Caixa = frmPrincipal.Caixa1;

            if (ckbVincularCPF.Checked)
            {
                Pedido1.DocumentoCliente = txtCPF.Text;
            }
            else
            {
                Pedido1.DocumentoCliente = "";
            }

            if (Pedido1.ValorDesconto > 0)
                Pedido1.AplicarDesconto = true;
            else
                Pedido1.AplicarDesconto = false;

            if (Pedido1.ValorServico > 0)
                Pedido1.AplicarServico = true;
            else
                Pedido1.AplicarServico = false;

            if (_estado == EstadoDelivery.NaoConfirmado)
                Pedido1.StatusPedido.StatusPedido = EStatusPedido.Aberto;

            Pedido.Salvar(Pedido1);
            var pedidoAlterado = false;

            foreach (var alterado in controlePedidoProduto.ListaPedidoProduto.Where(pp => pp.Status == StatusModel.Alterado))
            {
                pedidoAlterado = true;
                Pedido.CancelarProduto(AC.PDV.IDPDV.Value, AC.Usuario.IDUsuario.Value, alterado.IDPedidoProduto.Value, 1, string.Empty, true);
                Pedido.AdicionarProduto(ETipoPedido.Delivery, Pedido1.GUIDIdentificacao, AC.Usuario.IDUsuario.Value, AC.PDV.IDPDV.Value, new List<PedidoProdutoInformation> { alterado });
            }

            var cancelados = controlePedidoProduto.ListaPedidoProduto.Where(pp => pp.Status == StatusModel.Excluido && pp.IDPedidoProduto > 0);

            foreach (var cancelado in cancelados)
            {
                pedidoAlterado = true;
                try
                {
                    var id = cancelado.IDPedidoProduto.Value;
                    var informacoes = InformacoesCancelamento[id];
                    if (informacoes.HasValue)
                        Pedido.CancelarProduto(AC.PDV.IDPDV.Value, informacoes.Value.IDUsuario, id, informacoes.Value.IDMotivoCancelamento, informacoes.Value.Observacoes, informacoes.Value.RetornarAoEstoque);
                    else
                        Pedido.CancelarProduto(AC.PDV.IDPDV.Value, AC.Usuario.IDUsuario.Value, id, 1, string.Empty, true);

                    frmPedidoProdutoCancelamento.ImprimirComprovanteCancelamento(cancelado.IDPedidoProduto.Value);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("IDPedidoProduto", cancelado?.IDPedidoProduto);
                    ex.Data.Add("InformacoesCancelamento.Count", InformacoesCancelamento?.Count);
                    Logs.ErroBox(CodigoErro.E230, ex, MessageBoxIcon.Exclamation);
                }
            }

            if (cancelados.Count() > 0)
            {
                OrdemProducaoServices.GerarOrdemProducao(cancelados.ToList(), pedidoCancelado: true);
            }

            //PedidoPagamento.CancelarPorPedido(Pedido1.IDPedido.Value, AC.Usuario.IDUsuario.Value);
            var novosPagamentos = new List<PedidoPagamentoInformation>();
            foreach (var pagamento in Pedido1.ListaPagamento) //.Where(p => p.Status != StatusModel.Excluido))
            {
                if (pagamento.IDPedidoPagamento == null)
                {
                    if (pagamento.Status == StatusModel.Excluido)
                        continue;

                    var novoPagamento = new PedidoPagamentoInformation
                    {
                        Pedido = Pedido1,
                        Autorizacao = pagamento.Autorizacao,
                        TipoPagamento = pagamento.TipoPagamento,
                        MeioPagamentoSAT = pagamento.MeioPagamentoSAT,
                        Bandeira = pagamento.Bandeira,
                        IDGateway = pagamento.IDGateway,
                        ContaRecebivel = pagamento.ContaRecebivel,
                        Status = pagamento.Status,
                        Valor = pagamento.Valor,
                        Excluido = false
                    };
                    PedidoPagamento.Salvar(novoPagamento, AC.Usuario.IDUsuario.Value);
                    novosPagamentos.Add(novoPagamento);
                }
                else
                {
                    if (pagamento.Status == StatusModel.Excluido)
                        PedidoPagamento.Cancelar(pagamento, AC.Usuario.IDUsuario.Value);
                }
            }

            Pedido1.ListaPagamento.AddRange(novosPagamentos);

            if (_estado == EstadoDelivery.Finalizar)
            {
                var total = Pedido1.ValorTotalProdutos - (Pedido1.ValorDesconto.HasValue ? Pedido1.ValorDesconto.Value : 0) + Pedido1.ValorServico.Value + (Pedido1.ValorEntrega.HasValue ? Pedido1.ValorEntrega.Value : 0);
                FecharVenda.CalcularPagamentoDinheiro(Pedido1, total - ValorPago);
            }

            if (_estado != EstadoDelivery.Finalizar)
            {
                Pedido.AdicionarProduto(ETipoPedido.Delivery, Pedido1.GUIDIdentificacao, AC.Usuario.IDUsuario.Value, AC.PDV.IDPDV.Value, controlePedidoProduto.ListaPedidoProduto.Where(pp => pp.Status == StatusModel.Novo).ToList());
                Pedido.AdicionarProdutoTaxaEntrega(Pedido1, true, AC.PDV, AC.Usuario);
                List<PedidoProdutoInformation> listaProdutoProduzir;

                if (_estado == EstadoDelivery.NaoConfirmado)
                    listaProdutoProduzir = controlePedidoProduto.ListaPedidoProduto.Where(pp => pp.Status != StatusModel.Excluido).ToList();
                else
                    listaProdutoProduzir = controlePedidoProduto.ListaPedidoProduto.Where(pp => pp.Status == StatusModel.Novo).ToList();

                OrdemProducaoServices.GerarOrdemProducao(listaProdutoProduzir, pedidoAlterado);
            }

            Pedido1 = Pedido.CarregarCompleto(Pedido1.IDPedido.Value);
            //Pedido1.ListaPagamento = PedidoPagamento.ListarNaoCanceladoPorPedido(Pedido1.IDPedido.Value);
            //Pedido1.ListaProduto = controlePedidoProduto.ListaPedidoProduto.Where(pp => pp.Status != StatusModel.Excluido).ToList();
        }

        private void VoltarUmaAcao()
        {
            if (Navegacao.Count > 0)
            {
                Navegar = false;
                var name = Navegacao.Pop();
                var rb = Controls.Find(name, true).FirstOrDefault() as RadioButton;
                if (rb != null)
                    rb.Checked = true;
            }
        }

        #endregion

        private void enterCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                btnConfirmarCliente_Click(sender, e);
            }
        }

        private void txtCPF_Leave(object sender, EventArgs e)
        {
            if (txtCPF.Text.Length == 11 && !ValidacaoDocumento.IsCpf(txtCPF.Text))
            {
                MessageBox.Show("CPF Inválido!");
                txtCPF.Focus();
            }
            else if (txtCPF.Text.Length == 14 && !ValidacaoDocumento.IsCnpj(txtCPF.Text))
            {
                MessageBox.Show("CNPJ Inválido!");
                txtCPF.Focus();
            }
            else if (txtCPF.Text.Length != 0 && txtCPF.Text.Length != 11 && txtCPF.Text.Length != 14)
            {
                MessageBox.Show("CPF/CNPJ Incompleto!");
                txtCPF.Focus();
            }
        }

        bool lPesquisando = false;
        bool lIgnorado = false;
        private async void txtEnderecoNumero_TextChanged(object sender, EventArgs e)
        {
            if (lPesquisando)
            {
                lIgnorado = true;
                return;
            }

            //if (BLL.PDV.PossuiLoggi()
            // && !string.IsNullOrEmpty(txtEndereco.Text)
            // && !string.IsNullOrEmpty(txtEnderecoNumero.Text))
            //{
            //    lPesquisando = true;
            //    loggiOrcamento = null;
            //    lblLoggiEstimativa.Text = await LoggiDelivery.EstimarAsync((txtCEP.Text.Length == 8 ? txtCEP.Text : txtEndereco.Text) + ", " + txtEnderecoNumero.Text + " " + txtComplemento.Text);
            //    lPesquisando = false;
            //    if (lIgnorado)
            //    {
            //        lIgnorado = false;
            //        txtEnderecoNumero_TextChanged(null, null);
            //    }
            //}
        }
    }
}
