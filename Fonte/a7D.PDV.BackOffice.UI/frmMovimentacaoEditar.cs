using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using a7D.PDV.Model;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmMovimentacaoEditar : Form
    {
        private List<ProdutoInformation> Produtos { get; set; }

        private Dictionary<int, ProdutoInformation> ProdutosSelecionados { get; set; }

        private MovimentacaoInformation Movimentacao1 { get; set; }

        private List<TipoMovimentacaoInformation> TiposMovimentacao { get; set; }

        private string StrTipoMovimentacao { get; set; }

        private bool Novo { get; set; }

        public frmMovimentacaoEditar(string tipoMovimentacao)
        {
            Novo = true;
            StrTipoMovimentacao = tipoMovimentacao;
            Movimentacao1 = new MovimentacaoInformation
            {
                GUID = Guid.NewGuid().ToString(),
                MovimentacaoProdutos = new List<MovimentacaoProdutosInformation>(),
                Processado = false,
                Excluido = false,
                Reversa = false
            };
            InitializeComponent();
        }

        public frmMovimentacaoEditar(MovimentacaoInformation movimentacao)
        {
            Novo = false;
            StrTipoMovimentacao = movimentacao.TipoMovimentacao.Tipo;
            Movimentacao1 = movimentacao;
            InitializeComponent();
        }

        private void frmMovimentacaoEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            ProdutosSelecionados = new Dictionary<int, ProdutoInformation>();
            if (Movimentacao1.Processado == false)
                IniciarAutoComplete();
            AtualizarTiposMovimentacao();

            dgvPrincipal.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (Novo)
            {
                LimparCampos();
                txtDescricao.Text = "Movimentação - " + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                cbbMovimentacao.SelectedIndex = TiposMovimentacao.IndexOf(Movimentacao1.TipoMovimentacao);
                txtDescricao.Text = Movimentacao1.Descricao;
                txtPedido.Text = Movimentacao1.NumeroPedido;
                dtpData.Value = Movimentacao1.DataMovimentacao.Value;
                var indice = 0;

                foreach (var mp in Movimentacao1.MovimentacaoProdutos)
                {
                    //{ null, produto.IDProduto, produto.Nome, produto.Unidade.Nome, null, 1 }
                    var linhaProduto = new object[] { null, mp.IDMovimentacaoProdutos, mp.Produto.IDProduto, mp.Produto.Nome, mp.Unidade.Nome, null, mp.Quantidade };
                    dgvPrincipal.Rows.Add(linhaProduto);
                    ProdutosSelecionados.Add(indice++, mp.Produto);

                    var cb = dgvPrincipal.Rows[dgvPrincipal.Rows.Count - 1].Cells["UnidadeMovimentacao"] as DataGridViewComboBoxCell;

                    var unidade = mp.Unidade;

                    if (cb != null && unidade != null)
                    {
                        var unidades = ConversaoUnidade.ListarTodasConversoes(unidade);
                        unidades.Insert(0, unidade);
                        cb.DisplayMember = "Nome";
                        cb.ValueMember = "IDUnidade";
                        cb.DataSource = unidades;
                        cb.Value = unidade.IDUnidade.Value;
                    }
                }

                var processado = Movimentacao1.Processado.Value;
                txtProdutos.Enabled = !processado;
                btnTodosProdutos.Enabled = !processado;
                cbbMovimentacao.Enabled = !processado;
                dtpData.Enabled = !processado;


                foreach (DataGridViewColumn column in dgvPrincipal.Columns)
                {
                    column.ReadOnly = processado;
                }

                if (processado)
                {
                    dgvPrincipal.Columns.RemoveAt(0);
                    dgvPrincipal.Columns.RemoveAt(3);
                }

                dgvPrincipal.CurrentCell = null;
                dgvPrincipal.ClearSelection();
            }
        }

        private void LimparCampos()
        {
            cbbMovimentacao.SelectedIndex = 0;
            txtDescricao.Text = string.Empty;
            txtPedido.Text = string.Empty;
            dtpData.Value = DateTime.Now;
        }

        private void AtualizarTiposMovimentacao()
        {
            TiposMovimentacao = TipoMovimentacao.Listar()
                            .Where(tm => tm.Tipo == StrTipoMovimentacao)
                            .ToList();

            TiposMovimentacao.Insert(0, new TipoMovimentacaoInformation { Nome = "Selecione..." });
            cbbMovimentacao.DisplayMember = "Nome";
            cbbMovimentacao.ValueMember = "IDTipoMovimentacao";
            cbbMovimentacao.DataSource = TiposMovimentacao;
        }

        private void IniciarAutoComplete()
        {
            Produtos = Produto.Listar(new ProdutoInformation()
            {
                Excluido = false,
                Ativo = true,
                ControlarEstoque = true
            })
                .Where(p => ProdutoReceita.ListarPorProduto(p).Count() == 0)
                .OrderBy(p => p.Nome)
                .ToList();

            var produtosPesquisa = Produtos.Select(p => p.Nome);
            txtProdutos.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtProdutos.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(produtosPesquisa.ToArray());
            txtProdutos.AutoCompleteCustomSource = autoComplete;
        }

        private void txtProdutos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string nomeProduto = txtProdutos.Text;
                var produto = Produtos.FirstOrDefault(p => p.Nome == nomeProduto);
                AdicionarProduto(produto);
                txtProdutos.Text = string.Empty;
            }
        }

        private void AdicionarProduto(ProdutoInformation produto)
        {
            if (produto == null)
                return;

            produto.Unidade = BLL.Unidade.Carregar(produto.Unidade.IDUnidade.Value);

            if (ProdutosSelecionados.Values.Contains(produto))
            {
                var indice = ProdutosSelecionados.FirstOrDefault(p => p.Value == produto).Key;
                var cellQuantidade = dgvPrincipal.Rows[indice].Cells["Quantidade"];
                var quantidade = cellQuantidade.Value as int?;

                if (quantidade.HasValue)
                    cellQuantidade.Value = quantidade.Value + 1;
                else
                    cellQuantidade.Value = 1;

                dgvPrincipal.CurrentCell = cellQuantidade;
                dgvPrincipal.BeginEdit(true);
            }
            else
            {
                var indice = 0;
                try
                {
                    indice = ProdutosSelecionados.Keys.Max() + 1;
                }
                catch { }

                ProdutosSelecionados.Add(indice, produto);

                var unidade = produto.Unidade;

                var linhaProduto = new object[] { null, -1, produto.IDProduto, produto.Nome, produto.Unidade.Nome, null, 1 };
                dgvPrincipal.Rows.Add(linhaProduto);
                var cb = dgvPrincipal.Rows[dgvPrincipal.Rows.Count - 1].Cells["UnidadeMovimentacao"] as DataGridViewComboBoxCell;

                if (cb != null && unidade != null)
                {
                    var unidades = ConversaoUnidade.ListarTodasConversoes(unidade);
                    unidades.Insert(0, unidade);
                    cb.DisplayMember = "Nome";
                    cb.ValueMember = "IDUnidade";
                    cb.DataSource = unidades;
                    cb.Value = unidade.IDUnidade.Value;
                }

                dgvPrincipal.CurrentCell = dgvPrincipal.Rows[dgvPrincipal.Rows.Count - 1].Cells["Quantidade"];
                dgvPrincipal.BeginEdit(true);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTodosProdutos_Click(object sender, EventArgs e)
        {
            txtProdutos.Text = string.Empty;
            var frm = new frmMovimentacaoTodosProdutos(Produtos);
            var resp = frm.ShowDialog();
            if (resp == DialogResult.OK)
            {
                AdicionarProduto(frm.Produto);
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                try
                {
                    Movimentacao1.DataMovimentacao = dtpData.Value;
                    Movimentacao1.Descricao = txtDescricao.Text;
                    Movimentacao1.Fornecedor = cbbFornecedor.SelectedItem as ClienteInformation;
                    Movimentacao1.TipoMovimentacao = cbbMovimentacao.SelectedItem as TipoMovimentacaoInformation;
                    Movimentacao1.NumeroPedido = txtPedido.Text;

                    foreach (DataGridViewRow row in dgvPrincipal.Rows)
                    {
                        MovimentacaoProdutosInformation movimentacaoProdutos = null;

                        var idMovimentacao = row.Cells["IDMovimentacaoProdutos"].Value as int?;

                        if (idMovimentacao.HasValue && idMovimentacao.Value > 0)
                            movimentacaoProdutos = Movimentacao1.MovimentacaoProdutos.Where(mp => mp.IDMovimentacaoProdutos == idMovimentacao.Value).FirstOrDefault();
                        else
                            movimentacaoProdutos = new MovimentacaoProdutosInformation { StatusModel = StatusModel.Novo };

                        movimentacaoProdutos.Quantidade = decimal.Parse(row.Cells["Quantidade"].Value.ToString());
                        movimentacaoProdutos.Produto = new ProdutoInformation { IDProduto = Convert.ToInt32(row.Cells["IDProduto"].Value) };
                        movimentacaoProdutos.Unidade = new UnidadeInformation { IDUnidade = Convert.ToInt32(((DataGridViewComboBoxCell)row.Cells["UnidadeMovimentacao"]).Value) };
                        Movimentacao1.MovimentacaoProdutos.Add(movimentacaoProdutos);
                    }
                    Movimentacao.Salvar(Movimentacao1);
                    Close();
                }
                catch (Exception ex)
                {
                    Logs.ErroBox(CodigoErro.E013, ex);
                }
            }
        }

        private bool Validar()
        {
            var msg = new StringBuilder();

            if (cbbMovimentacao.SelectedIndex == 0)
                msg.AppendLine("Por favor selecione um tipo de movimentação");

            if (dgvPrincipal.Rows.Count < 1)
                msg.AppendLine("Por favor adicione pelo menos 1 produto");

            var valido = msg.Length == 0;

            if (!valido)
                MessageBox.Show(msg.ToString(), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return msg.Length == 0;
        }

        private void dgvPrincipal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && Movimentacao1.Processado == false)
            {
                var resp = MessageBox.Show("Excluir linha?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    var idMovimentacaoProdutos = dgvPrincipal.Rows[e.RowIndex].Cells["IDMovimentacaoProdutos"].Value as int?;
                    var idProduto = dgvPrincipal.Rows[e.RowIndex].Cells["IDProduto"].Value as int?;
                    var movimentacaoProdutos = Movimentacao1.MovimentacaoProdutos.Where(mp => mp.IDMovimentacaoProdutos.Value == idMovimentacaoProdutos.Value).FirstOrDefault();

                    if (movimentacaoProdutos != null)
                        movimentacaoProdutos.StatusModel = StatusModel.Excluido;

                    dgvPrincipal.Rows.RemoveAt(e.RowIndex);
                    var key = ProdutosSelecionados.FirstOrDefault(ps => ps.Value.IDProduto == idProduto).Key;
                    ProdutosSelecionados.Remove(key);
                }
            }
        }
    }
}
