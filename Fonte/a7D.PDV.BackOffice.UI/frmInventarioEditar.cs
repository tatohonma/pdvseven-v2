using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmInventarioEditar : Form
    {
        private InventarioInformation Inventario1 { get; set; }
        private bool Novo { get; set; }
        private List<ProdutoInformation> Produtos { get; set; }
        private Dictionary<int, ProdutoInformation> ProdutosSelecionados { get; set; }

        private IFormatProvider format = new CultureInfo("pt-BR");

        private bool leaving = false;

        public frmInventarioEditar()
        {
            Inventario1 = new InventarioInformation
            {
                GUID = Guid.NewGuid().ToString(),
                Processado = false,
                Excluido = false,
                InventarioProdutos = new List<InventarioProdutosInformation>()
            };
            Novo = true;
            InitializeComponent();
        }

        public frmInventarioEditar(InventarioInformation inventario)
        {
            Inventario1 = inventario;
            Novo = false;
            InitializeComponent();
        }

        private void frmInventarioEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            dgvPrincipal.Columns["NovaQuantidade"].ValueType = typeof(decimal?);

            ProdutosSelecionados = new Dictionary<int, ProdutoInformation>();
            if (Novo)
            {
                LimparCampos();
                txtDescricao.Text = "Contagem - " + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                txtDescricao.Text = Inventario1.Descricao;
                dtpData.Value = Inventario1.Data.Value;
            }

            if (Inventario1.Processado == false)
                IniciarAutoComplete();
            else
            {
                txtProdutos.Enabled = false;
                btnTodosProdutos.Enabled = false;
                btnAdicionarTodos.Enabled = false;
                dtpData.Enabled = false;
            }

            dgvPrincipal.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            foreach (var ip in Inventario1.InventarioProdutos)
                AdicionarProduto(ip.Produto, 0, ip.Quantidade.Value, ip.Unidade, ip.IDInventarioProdutos.Value);

            dgvPrincipal.CurrentCell = null;
            dgvPrincipal.ClearSelection();
        }

        //private void AtualizarEstoque()
        //{
        //    var estoqueAtual = EntradaSaida.EstoqueAtual();
        //    foreach (var estoque in estoqueAtual)
        //    {
        //        if (ProdutosSelecionados.Values.Contains(estoque.Produto))
        //        {
        //            var row = dgvPrincipal.Rows[ProdutosSelecionados.FirstOrDefault(p => p.Value.IDProduto == estoque.Produto.IDProduto).Key];
        //            var celQuantidadeEstoque = row.Cells["Quantidade"];
        //            var novaQuantidade = (celQuantidadeEstoque.Value as decimal?).HasValue ? (celQuantidadeEstoque.Value as decimal?).Value : 0m;
        //            //novaQuantidade += ;
        //            celQuantidadeEstoque.Value = ConversaoUnidade.Converter(estoque.Quantidade, estoque.Unidade, estoque.Produto.Unidade, true);
        //        }
        //        else
        //        {
        //            var indice = 0;
        //            try
        //            {
        //                indice = ProdutosSelecionados.Keys.Max() + 1;
        //            }
        //            catch { }

        //            ProdutosSelecionados.Add(indice, estoque.Produto);

        //            var unidade = estoque.Produto.Unidade;

        //            var quantidadeConvertida = ConversaoUnidade.Converter(estoque.Quantidade, estoque.Unidade, estoque.Produto.Unidade, true);
        //            var linhaProduto = new object[] { null, -1, estoque.Produto.IDProduto, estoque.Produto.Nome, estoque.Produto.Unidade.Nome, quantidadeConvertida, null, quantidadeConvertida };
        //            dgvPrincipal.Rows.Add(linhaProduto);
        //            var cb = dgvPrincipal.Rows[dgvPrincipal.Rows.Count - 1].Cells["NovaUnidade"] as DataGridViewComboBoxCell;

        //            if (cb != null && unidade != null)
        //            {
        //                var unidades = ConversaoUnidade.ListarUnidadesConversaoPara(unidade);
        //                unidades.Insert(0, unidade);
        //                cb.DisplayMember = "Nome";
        //                cb.ValueMember = "IDUnidade";
        //                cb.DataSource = unidades;
        //                cb.Value = unidade.IDUnidade.Value;
        //            }
        //        }
        //    }
        //}

        private void LimparCampos()
        {
            txtDescricao.Text = string.Empty;
            dtpData.Value = DateTime.Now;
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

            //Produtos = Produto.Listar(new ProdutoInformation() { })
            //                .Where(p => p.Excluido == false)
            //                .Where(p => p.Ativo == true)
            //                .Where(p => p.ControlarEstoque == true)
            //                .Where(p => ProdutoReceita.ListarPorProduto(p).Count() == 0)
            //                .OrderBy(p => p.Nome)
            //                .ToList();

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
                if (!leaving)
                {
                    leaving = true;
                    VerificarProduto();
                    leaving = false;
                }
            }
        }

        private void VerificarProduto()
        {
            string nomeProduto = txtProdutos.Text;
            var produto = Produtos.FirstOrDefault(p => string.Compare(p.Nome, nomeProduto, true) == 0);
            AdicionarProduto(produto);
            txtProdutos.Text = string.Empty;
        }

        private void AdicionarProduto(ProdutoInformation produto, decimal quantidadeAnterior = 0, decimal? novaQuantidade = 0, UnidadeInformation novaUnidade = null, int idInventarioProdutos = -1)
        {
            if (produto == null)
                return;
            if (produto.Unidade?.IDUnidade == null)
                return;

            produto.Unidade = BLL.Unidade.Carregar(produto.Unidade.IDUnidade.Value);

            //if (ProdutosSelecionados.Values.Contains(produto))
            //{
            //    var indice = ProdutosSelecionados.FirstOrDefault(p => p.Value.IDProduto == produto.IDProduto.Value).Key;
            //    var cellQuantidade = dgvPrincipal.Rows[indice].Cells["NovaQuantidade"];
            //    //var quantidadeAtual = cellQuantidade.Value as decimal?;

            //    //if (quantidadeAtual.HasValue)
            //    //    cellQuantidade.Value = quantidadeAtual.Value + novaQuantidade;
            //    //else
            //    //    cellQuantidade.Value = 1;

            //    dgvPrincipal.CurrentCell = cellQuantidade;
            //    dgvPrincipal.BeginEdit(true);
            //}
            //else
            //{
            var indice = 0;
            try
            {
                indice = ProdutosSelecionados.Keys.Max() + 1;
            }
            catch { }

            ProdutosSelecionados.Add(indice, produto);

            var unidade = novaUnidade == null ? produto.Unidade : novaUnidade;

            var linhaProduto = new object[] { null, idInventarioProdutos, produto.IDProduto, produto.Nome, produto.Unidade.Nome, quantidadeAnterior, null, novaQuantidade?.ToString("0.#############################", format) };
            dgvPrincipal.Rows.Add(linhaProduto);
            var cb = dgvPrincipal.Rows[dgvPrincipal.Rows.Count - 1].Cells["NovaUnidade"] as DataGridViewComboBoxCell;

            if (cb != null && unidade != null)
            {
                var unidades = ConversaoUnidade.ListarUnidadesConversaoPara(unidade);
                unidades.Insert(0, unidade);
                cb.DisplayMember = "Nome";
                cb.ValueMember = "IDUnidade";
                cb.DataSource = unidades;
                cb.Value = unidade.IDUnidade.Value;
            }

            dgvPrincipal.CurrentCell = dgvPrincipal.Rows[dgvPrincipal.Rows.Count - 1].Cells["NovaQuantidade"];
            dgvPrincipal.BeginEdit(true);
            //}
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                if (!Validar())
                    return;

                UseWaitCursor = true;
                Cursor = Cursors.WaitCursor;
                dgvPrincipal.Cursor = Cursor;

                try
                {
                    Inventario1.Descricao = txtDescricao.Text;
                    Inventario1.Data = dtpData.Value;

                    foreach (DataGridViewRow row in dgvPrincipal.Rows)
                    {
                        InventarioProdutosInformation inventarioProdutos = null;

                        var idInventarioProdutos = row.Cells["IDInventarioProdutos"].Value as int?;

                        if (idInventarioProdutos.HasValue && idInventarioProdutos.Value > 0)
                            inventarioProdutos = Inventario1.InventarioProdutos.Where(mp => mp.IDInventarioProdutos == idInventarioProdutos.Value).FirstOrDefault();
                        else
                            inventarioProdutos = new InventarioProdutosInformation { StatusModel = StatusModel.Novo };

                        var produto = Produto.Carregar(Convert.ToInt32(row.Cells["IDProduto"].Value));
                        var unidade = BLL.Unidade.Carregar(Convert.ToInt32(((DataGridViewComboBoxCell)row.Cells["NovaUnidade"]).Value));

                        var vQuantidade = row.Cells["NovaQuantidade"].Value;

                        if (vQuantidade is decimal)
                        {
                            inventarioProdutos.Quantidade = (decimal)vQuantidade;

                        }
                        else if (decimal.TryParse(row.Cells["NovaQuantidade"].Value as string, NumberStyles.Any, format, out decimal dValue))
                        {
                            inventarioProdutos.Quantidade = dValue;
                        }
                        else
                        {
                            inventarioProdutos.Quantidade = 0;
                        }
                        inventarioProdutos.Produto = produto;
                        inventarioProdutos.Unidade = unidade;
                        inventarioProdutos.QuantidadeAnterior = ConversaoUnidade.Converter(EntradaSaida.EstoqueAtual(produto.IDProduto.Value), produto.Unidade, unidade, true);
                        Inventario1.InventarioProdutos.Add(inventarioProdutos);
                    }

                    Inventario.Salvar(Inventario1);
                    Close();
                }
                catch (Exception ex)
                {
                    Logs.ErroBox(CodigoErro.E013, ex);
                }
                finally
                {
                    Cursor = Cursors.Default;
                    Refresh();
                }
            }
        }

        private void LimparProdutosVazios()
        {
            //foreach (DataGridViewRow row in dgvPrincipal.Rows)
            //{
            //    if (row.Cells["NovaQuantidade"].Value as decimal? == null)
            //        ExcluirLinha(row, false);
            //}
            for (int row = 0; row < dgvPrincipal.Rows.Count; ++row)
            {
                bool isEmpty = true;
                object value = dgvPrincipal.Rows[row].Cells["NovaQuantidade"].Value;
                if (value is decimal)
                {
                    if (((decimal)value) > 0)
                        isEmpty = false;
                }
                else if (value != null && decimal.TryParse(value as string, NumberStyles.Any, format, out decimal dValue))
                {
                    if (dValue > 0)
                        isEmpty = false;
                }
                if (isEmpty)
                {
                    ExcluirLinha(dgvPrincipal.Rows[row--], false);
                }
            }
        }

        private bool Validar()
        {
            var msg = new StringBuilder();

            if (string.IsNullOrEmpty(txtDescricao.Text))
                msg.AppendLine("Informe uma descrição.");

            if (ProdutosSelecionados.Count < 1)
                msg.AppendLine("O inventário deve ter pelo menos 1 linha");

            if (string.IsNullOrEmpty(msg.ToString()) == false)
                MessageBox.Show(msg.ToString(), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return string.IsNullOrEmpty(msg.ToString());
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

        private void dgvPrincipal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && Inventario1.Processado == false)
            {
                ExcluirLinha(dgvPrincipal.Rows[e.RowIndex]);
            }
        }

        private void ExcluirLinha(DataGridViewRow row, bool confirmar = true)
        {
            DialogResult resp = DialogResult.None;
            if (confirmar)
                resp = MessageBox.Show("Excluir linha?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (!confirmar || resp == DialogResult.Yes)
            {
                var idInventarioProdutos = dgvPrincipal.Rows[row.Index].Cells["IDInventarioProdutos"].Value as int?;
                var idProduto = dgvPrincipal.Rows[row.Index].Cells["IDProduto"].Value as int?;
                var movimentacaoProdutos = Inventario1.InventarioProdutos.Where(mp => mp.IDInventarioProdutos.Value == idInventarioProdutos.Value).FirstOrDefault();

                if (movimentacaoProdutos != null)
                    movimentacaoProdutos.StatusModel = StatusModel.Excluido;

                dgvPrincipal.Rows.Remove(row);
                var key = ProdutosSelecionados.FirstOrDefault(ps => ps.Value.IDProduto == idProduto).Key;
                ProdutosSelecionados.Remove(key);
            }
        }

        private void btnAdicionarTodos_Click(object sender, EventArgs e)
        {
            HashSet<int> produtos = new HashSet<int>();

            foreach (DataGridViewRow row in dgvPrincipal.Rows)
            {
                produtos.Add(Convert.ToInt32(row.Cells["IDProduto"].Value));
            }

            var nProdutos = Produtos.Where(p => !produtos.Contains(p.IDProduto.Value));

            foreach (var produto in nProdutos)
            {
                AdicionarProduto(produto, novaQuantidade: null);
            }
        }

        private void dgvPrincipal_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvPrincipal_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void txtProdutos_Leave(object sender, EventArgs e)
        {
            if (!leaving)
            {
                leaving = true;
                VerificarProduto();
                leaving = false;
            }
        }
    }
}
