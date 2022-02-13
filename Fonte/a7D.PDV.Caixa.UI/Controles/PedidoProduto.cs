using a7D.PDV.Balanca;
using a7D.PDV.BLL;
using a7D.PDV.Caixa.UI.Properties;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using a7D.PDV.Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI.Controles
{
    public partial class PedidoProduto : UserControl
    {
        public ETipoPedido TipoPedidoSelecionado { get; set; }
        private string GUIDIdentificacao_selecionado;

        private List<ProdutoInformation> ListaProdutos { get; set; }
        private int? NumeroComanda { get; set; }
        private PedidoInformation Pedido1 { get; set; }
        private string CodigoAliquitaPadrao { get; set; }
        public List<PedidoProdutoInformation> ListaPedidoProduto { get; set; }

        public Action Fechar { get; set; }
        public Action Confirmar { get; set; }
        public Action<PedidoProdutoInformation> ProdutoAdicionado { get; set; }
        public Action<PedidoProdutoInformation, InformacoesCancelamento?> ProdutoRemovido { get; set; }
        public Action<PedidoProdutoInformation, List<PedidoProdutoInformation>> ModificacaoAlterada { get; set; }
        public Action<PedidoProdutoInformation, UsuarioInformation> DescontoAplicado { get; set; }

        public bool PermitirDesconto { get; set; } = false;

        private Image ImgDesconto { get; set; } = Resources.desconto;
        private Image Vazio = Resources.semImagem;

        public bool Cancelar { get; set; } = true;

        public PedidoProduto()
        {
            InitializeComponent();
        }

        public void CarregarPedidoProduto(string guidIdentificacao_selecionado, int? numeroComanda = null)
        {
            //TipoPedidoSelecionado = idTipoPedido_selecionado;
            GUIDIdentificacao_selecionado = guidIdentificacao_selecionado;
            NumeroComanda = numeroComanda;
            Pedido1 = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);
            DeinirIdentificacao();
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            ListarItens();
        }

        public void SolicitarFoco()
        {
            txtNome.Focus();
        }

        private void ListarItens()
        {
            var nome = txtNome.Text;


            if (int.TryParse(nome, out int codigo))
            {
                var list = from l in ListaProdutos
                           where
                              (l.Codigo != null && l.Codigo.ToLower().Contains(nome.ToLower())) ||
                              (l.cEAN != null && nome.Length > 9 && l.cEAN.ToLower().Contains(nome.ToLower()))
                           orderby l.Nome
                           select new { l.IDProduto, l.Codigo, l.Nome, l.ValorUnitario };

                dgvItens.DataSource = list.ToArray();
            }
            else
            {
                var list = from l in ListaProdutos
                           where (CultureInfo.CurrentCulture.CompareInfo.IndexOf(l.Nome, nome, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) >= 0) ||
                              (l.Codigo != null && l.Codigo.ToLower().Contains(nome.ToLower())) ||
                              (l.cEAN != null && nome.Length > 9 && l.cEAN.ToLower().Contains(nome.ToLower()))
                           orderby l.Nome
                           select new { l.IDProduto, l.Codigo, l.Nome, l.ValorUnitario };

                dgvItens.DataSource = list.ToArray();
            }
        }

        private void PedidoProduto_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            try
            {
                btnViagem.Visible = TipoPedidoSelecionado != ETipoPedido.Delivery && ConfiguracoesSistema.Valores.ProdutoViagem;
                dgvItensSelecionados.Columns[7].Visible = false;

                if (TipoPedidoSelecionado == ETipoPedido.Comanda && NumeroComanda.HasValue)
                {
                    var msg = string.Empty;
                    var comanda = Comanda.CarregarPorNumeroOuCodigo(NumeroComanda.Value);
                    if (comanda.ValorStatusComanda == EStatusComanda.Cancelada)
                    {
                        msg = "Comanda cancelada!\nFavor entrar em contato com o gerente!";
                    }
                    else if (comanda.ValorStatusComanda == EStatusComanda.Perdida)
                    {
                        msg = "Comanda marcada como perdida!\nFavor entrar em contato com o gerente para liberá-la";
                    }

                    if (string.IsNullOrEmpty(msg) == false)
                    {
                        MessageBox.Show(msg, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Fechar?.Invoke();
                        return;
                    }
                }
                CodigoAliquitaPadrao = ConfiguracoesSistema.Valores.AliquotaPadrao;
                DeinirIdentificacao();

                bool listaCreditos = frmPrincipal.ContaCliente && TipoPedidoSelecionado == ETipoPedido.Balcao;

                if (listaCreditos)
                {
                    ListaProdutos = Produto.Listar(new ProdutoInformation()
                    {
                        Excluido = false,
                        Ativo = true,
                    }).Where(l => l.IDProduto > 4 && (l.TipoProduto.TipoProduto == ETipoProduto.Item || l.TipoProduto.TipoProduto == ETipoProduto.Credito)).ToList();

                }
                else
                    ListaProdutos = Produto.Listar(new ProdutoInformation()
                    {
                        Excluido = false,
                        Ativo = true,
                        TipoProduto = new TipoProdutoInformation() { IDTipoProduto = 10 }
                    }).Where(l => l.IDProduto > 4).ToList();


                var produtosCategoriaProduto = ProdutoCategoriaProduto.ListarComCategoria().Where(pcp => pcp.Produto != null);
                ListaProdutos = ListaProdutos.Select(p =>
                {
                    var produto = p;
                    var indisponivel = produtosCategoriaProduto.Any(pcp => pcp.Produto?.IDProduto == p.IDProduto && pcp.CategoriaProduto?.Disponibilidade == false);
                    p.Disponibilidade = indisponivel ? false : p.Disponibilidade;
                    return p;
                })
                .ToList();

                ListaPedidoProduto = new List<PedidoProdutoInformation>();

                var watchListar = Stopwatch.StartNew();
                dgvItens.SuspendLayout();
                ListarItens();
                dgvItens.ResumeLayout();
                watchListar.Stop();
                //Debug.WriteLine($"DataBinding Time: {watchListar.ElapsedMilliseconds}ms");

                if (PermitirDesconto)
                {
                    var column = new DataGridViewImageColumn();
                    column.Name = "colDesconto";
                    column.HeaderText = "";
                    column.Resizable = DataGridViewTriState.False;
                    column.ReadOnly = false;
                    column.Image = Vazio;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    dgvItensSelecionados.Columns.Insert(4, column);
                }
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E809, ex);
                Fechar?.Invoke();
            }
            finally
            {
            }
        }

        private void DeinirIdentificacao()
        {
            if (!string.IsNullOrWhiteSpace(GUIDIdentificacao_selecionado))
            {
                switch (TipoPedidoSelecionado)
                {
                    case ETipoPedido.Mesa:
                        lblIdentificacao.Text = "MESA " + Mesa.CarregarPorGUID(GUIDIdentificacao_selecionado).Numero;
                        break;

                    case ETipoPedido.Comanda:
                        lblIdentificacao.Text = "COMANDA " + Comanda.CarregarPorGUID(GUIDIdentificacao_selecionado).Numero;
                        break;
                }
            }

            switch (TipoPedidoSelecionado)
            {
                case ETipoPedido.Delivery:
                    lblIdentificacao.Text = btnAdicionarProduto.Enabled ? "DELIVERY" : "DELIVERY IFOOD\r\nNão permite edição";
                    break;

                case ETipoPedido.Balcao:
                    lblIdentificacao.Text = "BALCÃO";
                    break;
            }
        }

        private void btnMenos_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQuantidade.Text, out int qtd))
                return;

            if (qtd > 1)
                qtd--;

            txtQuantidade.Text = qtd.ToString();
        }

        private void btnMais_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQuantidade.Text, out int qtd))
                return;

            if (qtd < Produto.MaxQTD)
                qtd++;

            txtQuantidade.Text = qtd.ToString();
        }

        private void btnAdicionarProduto_Click(object sender, EventArgs e)
        {
            AdicionarProduto();
        }

        public void CarregarProdutos(List<PedidoProdutoInformation> produtos)
        {
            ListaPedidoProduto = new List<PedidoProdutoInformation>(produtos.Where(pp => pp.Produto?.IDProduto != 4));
            VisualizarProdutos();
        }

        private async void AdicionarProduto()
        {
            if (!btnAdicionarProduto.Enabled)
                return;

            if (!decimal.TryParse(txtQuantidade.Text, out decimal quantidade)
             || !Produto.QTDValido(quantidade))
            {
                MessageBox.Show($"A quantidade deve estar entre 0 e {Produto.MaxQTD}");
                return;
            }

            var produto = default(ProdutoInformation);
            if (ConfiguracoesCaixa.Valores.EtiquetaBalanca)
            {
                produto = Produto.ObterProdutoEtiqueta(
                    txtNome.Text,
                    out decimal? quantidadeEtiqueta,
                    ConfiguracoesCaixa.Valores.DigitosCodigo,
                    ConfiguracoesCaixa.Valores.BalancaPorPeso);

                if (quantidadeEtiqueta.HasValue)
                    quantidade = quantidadeEtiqueta.Value;
            }

            if (produto == null && dgvItens.SelectedRows.Count > 0)
            {
                var idProduto = Convert.ToInt32(dgvItens.SelectedRows[0].Cells[nameof(IDProduto)].Value);
                produto = Produto.Carregar(idProduto);
                if (produto.UtilizarBalanca == true && BalancaServices.TemBalanca)
                {
                    quantidade = await BalancaServices.LerBalancaLoop();
                    if (quantidade == 0)
                        return;
                }
                else
                    quantidade = Convert.ToDecimal(txtQuantidade.Text);

                produto = Produto.CarregarCompleto(idProduto);
            }

            if (produto != null)
            {
                var minID = ListaPedidoProduto.Where(l => l.IDPedidoProduto <= 0).Min(l => l.IDPedidoProduto);
                int idPedidoProduto;
                if (minID == null)
                    idPedidoProduto = -1;
                else
                    idPedidoProduto = minID.Value - 1;

                var valorUnitario = produto.ValorUnitario.Value;
                var codigoAliquita = produto.CodigoAliquota;
                var notas = "";

                if (valorUnitario == 0 && produto.AssistenteModificacoes != true)
                {
                    using (var frm = new frmPedidoProdutosValor())
                    {
                        frm.ShowDialog();

                        if (frm.DialogResult == DialogResult.OK)
                        {
                            valorUnitario = frm.Valor;
                            notas = frm.Notas;
                        }
                        else
                            return;
                    }
                }

                if (produto.CodigoAliquota == null || produto.CodigoAliquota == "")
                    codigoAliquita = CodigoAliquitaPadrao;

                bool valorFracionario = (quantidade - (int)quantidade) > 0;

                while (quantidade > 0)
                {
                    var pedidoProduto = new PedidoProdutoInformation
                    {
                        IDPedidoProduto = idPedidoProduto--,
                        CodigoAliquota = codigoAliquita,
                        ValorUnitario = valorUnitario,
                        Notas = notas,
                        Cancelado = false,
                        Usuario = AC.Usuario,
                        PDV = AC.PDV,
                        Produto = produto
                    };

                    if (valorFracionario)
                    {
                        pedidoProduto.Quantidade = quantidade;
                        quantidade = 0;
                    }
                    else
                    {
                        pedidoProduto.Quantidade = 1;
                        quantidade--;
                    }
                    ListaPedidoProduto.Add(pedidoProduto);
                    ProdutoAdicionado?.Invoke(pedidoProduto);
                }

                txtNome.Text = "";
                txtQuantidade.Text = "1";

                VisualizarProdutos();

                if (produto.AssistenteModificacoes == true && produto.ListaPainelModificacao.Count > 0)
                    EditaUltimoProduto();
            }
            else
            {
                MessageBox.Show("Produto não encontrado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            txtNome.Text = "";
            txtQuantidade.Text = "1";

            txtNome.Focus();
        }

        private void VisualizarProdutos()
        {
            dgvItensSelecionados.Rows.Clear();

            foreach (var item in ListaPedidoProduto.Where(p => p.Status != StatusModel.Excluido))
            {
                var row = new List<object>()
                {
                    null,
                    null,
                    item.Produto.Nome + Environment.NewLine + item.Notas,
                    item.Quantidade,
                    item.ValorUnitario.Value.ToString("#,##0.00"),
                    (item.ValorTotal).ToString("#,##0.00"),
                    item.IDPedidoProduto
                };

                if (PermitirDesconto)
                    row.Insert(4, null);

                dgvItensSelecionados.Rows.Add(row.ToArray());

                if (item.ListaModificacao != null)
                {
                    foreach (var modificacao in item.ListaModificacao)
                    {
                        row = new List<object>()
                        {
                            null,
                            null,
                            "- " + modificacao.Produto.Nome,
                            modificacao.Quantidade.Value,
                            modificacao.ValorUnitarioString,
                            modificacao.ValorTotalString,
                            modificacao.IDPedidoProduto
                        };

                        if (PermitirDesconto)
                            row.Insert(4, null);

                        dgvItensSelecionados.Rows.Add(row.ToArray());
                        dgvItensSelecionados.Rows[dgvItensSelecionados.Rows.Count - 1].Cells[0].Value = Vazio;
                        dgvItensSelecionados.Rows[dgvItensSelecionados.Rows.Count - 1].Cells[1].Value = Vazio;
                        if (PermitirDesconto)
                            dgvItensSelecionados.Rows[dgvItensSelecionados.Rows.Count - 1].Cells[4].Value = Vazio;
                    }
                }
            }
        }

        internal void BloqueiaEdicao()
        {
            btnAdicionarProduto.Enabled = false;
        }

        private void dgvItens_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AdicionarProduto();
        }

        private void dgvItens_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                AdicionarProduto();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (Confirmar != null)
                Confirmar();
            else Fechar?.Invoke();

            txtNome.Focus();
        }

        private void PedidoProduto_Enter(object sender, EventArgs e)
        {
            txtNome.Focus();
        }

        private void dgvItens_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvItens.SelectedRows.Count > 0)
            {
                var row = dgvItens.SelectedRows[0];
                var idProduto = Convert.ToInt32(row.Cells[nameof(IDProduto)].Value);
                var produto = ListaProdutos.FirstOrDefault(p => p.IDProduto == idProduto);
                if (produto?.Disponibilidade == false)
                {
                    dgvItens.ClearSelection();
                }
                txtNome.Focus();
            }
        }

        private void dgvItens_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvItens.Rows[e.RowIndex];
                var idProduto = Convert.ToInt32(row.Cells[nameof(IDProduto)].Value);
                var produto = ListaProdutos.FirstOrDefault(p => p.IDProduto == idProduto);
                if (produto?.Disponibilidade == false)
                {
                    row.Cells[e.ColumnIndex].Style.ForeColor = Color.Gray;
                }
                else
                {
                    row.Cells[e.ColumnIndex].Style.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private void EditaUltimoProduto()
        {
            dgvItensSelecionados_CellContentClick(dgvItensSelecionados, new DataGridViewCellEventArgs(1, dgvItensSelecionados.RowCount - 1));
        }

        private void dgvItensSelecionados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!btnAdicionarProduto.Enabled)
                return;

            var idPedidoProduto = default(int);

            if (e.RowIndex >= 0)
            {
                switch (e.ColumnIndex)
                {
                    case 0:
                        idPedidoProduto = Convert.ToInt32(dgvItensSelecionados.Rows[e.RowIndex].Cells[nameof(IDPedidoProduto)].Value);
                        var pedidoProdutoRemovido = ListaPedidoProduto.FirstOrDefault(pp => pp.IDPedidoProduto == idPedidoProduto);

                        if (pedidoProdutoRemovido.Status == StatusModel.Novo)
                        {
                            ListaPedidoProduto = ListaPedidoProduto.Where(pp => pp.IDPedidoProduto != idPedidoProduto).ToList();
                            ProdutoRemovido?.Invoke(pedidoProdutoRemovido, null);
                        }
                        else
                        {
                            if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
                            {
                                using (var frmCancelamento = new frmPedidoProdutoCancelamento(pedidoProdutoRemovido.IDPedidoProduto.Value, usuario.IDUsuario.Value))
                                {
                                    frmCancelamento.Cancelar = Cancelar;
                                    if (frmCancelamento.ShowDialog() == DialogResult.OK)
                                    {
                                        pedidoProdutoRemovido.Status = StatusModel.Excluido;
                                        ProdutoRemovido?.Invoke(pedidoProdutoRemovido, frmCancelamento.InformacoesCancelamento);
                                    }
                                }
                            }
                        }

                        VisualizarProdutos();
                        break;
                    case 1:
                        idPedidoProduto = Convert.ToInt32(dgvItensSelecionados[nameof(IDPedidoProduto), e.RowIndex].Value);

                        if (idPedidoProduto != 0)
                        {
                            var pedidoProduto = ListaPedidoProduto.First(l => l.IDPedidoProduto == idPedidoProduto);
                            int? minId = ListaPedidoProduto.Where(l => l.IDPedidoProduto <= 0).Min(l => l.IDPedidoProduto);
                            if (pedidoProduto.Produto.AssistenteModificacoes == true
                             && pedidoProduto.Produto.ListaPainelModificacao.Count > 0)
                            {
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

                                    VisualizarProdutos();
                                    ModificacaoAlterada?.Invoke(pedidoProduto, pedidoProduto.ListaModificacao);
                                }
                            }
                            else
                            {
                                using (var form = new frmPedidoProdutosModificacoes(pedidoProduto, minId))
                                {
                                    form.ShowDialog();

                                    if (pedidoProduto.Status == StatusModel.Inalterado)
                                        pedidoProduto.Status = StatusModel.Alterado;

                                    pedidoProduto.ListaModificacao = form.ListaProduto;
                                    pedidoProduto.Notas = form.Notas;

                                    VisualizarProdutos();
                                    ModificacaoAlterada?.Invoke(pedidoProduto, form.ListaProduto);
                                }
                            }

                        }
                        break;
                    case 4:
                    case 5:
                        if (PermitirDesconto)
                        {
                            var row = dgvItensSelecionados.Rows[e.RowIndex];
                            idPedidoProduto = Convert.ToInt32(row.Cells[nameof(IDPedidoProduto)].Value);
                            var pedidoProduto = ListaPedidoProduto.FirstOrDefault(pp => pp.IDPedidoProduto == idPedidoProduto);
                            if (pedidoProduto == null)
                                pedidoProduto = ListaPedidoProduto.Where(pp => pp.ListaModificacao?.Count > 0).SelectMany(pp => pp.ListaModificacao).FirstOrDefault(pp => pp.IDPedidoProduto == idPedidoProduto);
                            if (pedidoProduto?.ValorUnitario > 0)
                            {
                                UsuarioInformation usuario;
                                if (ConfiguracoesSistema.Valores.SolicitarSenhaDesconto)
                                {
                                    if (NormalOuTouch.Autenticacao(false, true, false, false, out usuario) != DialogResult.OK)
                                        return;
                                }
                                else
                                    usuario = AC.Usuario;

                                pedidoProduto.Produto = Produto.Carregar(pedidoProduto.Produto.IDProduto.Value);
                                using (var frmDesconto = new frmPedidoProdutoDesconto(pedidoProduto, usuario))
                                {
                                    if (frmDesconto.ShowDialog() == DialogResult.OK)
                                        DescontoAplicado?.Invoke(pedidoProduto, usuario);
                                }
                                VisualizarProdutos();
                            }
                            else
                            {
                                MessageBox.Show("Não é possível dar desconto para produtos sem preço!");
                            }
                        }
                        break;
                    case 7:
                    case 8:
                        var rowV = dgvItensSelecionados.Rows[e.RowIndex];
                        idPedidoProduto = Convert.ToInt32(rowV.Cells[nameof(IDPedidoProduto)].Value);
                        var pedidoProdutoViagem = ListaPedidoProduto.FirstOrDefault(pp => pp.IDPedidoProduto == idPedidoProduto);
                        if (pedidoProdutoViagem != null)
                        {
                            pedidoProdutoViagem.Viagem = pedidoProdutoViagem.Viagem != true;
                            dgvItensSelecionados.Rows[e.RowIndex].Cells[dgvItensSelecionados.Columns.Count - 1].Value = pedidoProdutoViagem.Viagem;
                        }
                        break;
                }
            }
        }

        private void dgvItensSelecionados_MouseMove(object sender, MouseEventArgs e)
        {
            if (PermitirDesconto)
            {
                var hti = dgvItensSelecionados.HitTest(e.X, e.Y);
                if (hti.ColumnIndex == 4 || hti.ColumnIndex == 5)
                {
                    dgvItensSelecionados.Cursor = Cursors.Hand;
                }
                else
                {
                    dgvItensSelecionados.Cursor = Cursors.Default;
                }
            }
        }

        private void dgvItensSelecionados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (PermitirDesconto && e.RowIndex >= 0 && e.ColumnIndex == 4)
            {
                var row = dgvItensSelecionados.Rows[e.RowIndex];
                var idPedidoProduto = Convert.ToInt32(row.Cells[nameof(IDPedidoProduto)].Value);
                var pedidoProduto = ListaPedidoProduto.FirstOrDefault(p => p.IDPedidoProduto == idPedidoProduto);
                if (pedidoProduto == null)
                    pedidoProduto = ListaPedidoProduto.Where(pp => pp.ListaModificacao != null).SelectMany(pp => pp.ListaModificacao).FirstOrDefault(p => p.IDPedidoProduto == idPedidoProduto);
                var cell = row.Cells[4] as DataGridViewImageCell;
                if (pedidoProduto?.ValorDesconto > 0)
                {
                    cell.Value = ImgDesconto;
                }
                else
                {
                    cell.Value = Vazio;
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        private void txtNome_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    AdicionarProduto();
                    break;
                case Keys.Up:
                    MoveSelectionUp();
                    e.Handled = true;
                    break;
                case Keys.Down:
                    MoveSelectionDown();
                    e.Handled = true;
                    break;
                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

        private void MoveSelectionUp()
        {
            try
            {
                if (dgvItens.RowCount > 0)
                {
                    if (dgvItens.SelectedRows.Count > 0)
                    {
                        var index = dgvItens.SelectedCells[0].OwningRow.Index;

                        if (index == 0)
                            return;

                        dgvItens.Rows[index - 1].Selected = true;

                        var first = dgvItens.FirstDisplayedScrollingRowIndex;
                        if ((index - 1) < first)
                            first--;
                        if (first < 0)
                            first = 0;
                        dgvItens.FirstDisplayedScrollingRowIndex = first;
                    }
                }
            }
            catch { }
        }

        private void MoveSelectionDown()
        {
            try
            {
                if (dgvItens.RowCount > 0)
                {
                    if (dgvItens.SelectedRows.Count > 0)
                    {
                        var index = dgvItens.SelectedCells[0].OwningRow.Index;
                        var rowCount = dgvItens.Rows.Count;
                        if (index == (rowCount - 1))
                            return;

                        dgvItens.Rows[index + 1].Selected = true;
                        var first = (index + 2) - dgvItens.DisplayedRowCount(false);
                        if (first < 0)
                            first = 0;
                        dgvItens.FirstDisplayedScrollingRowIndex = first;
                    }
                }
            }
            catch { }
        }

        private void btnViagem_Click(object sender, EventArgs e)
        {
            dgvItensSelecionados.Columns[dgvItensSelecionados.Columns.Count - 1].Visible = !dgvItensSelecionados.Columns[dgvItensSelecionados.Columns.Count - 1].Visible;
        }
    }
}
