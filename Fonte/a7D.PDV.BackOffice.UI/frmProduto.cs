using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using System.Globalization;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmProduto : Form
    {
        private List<ProdutoInformation> Lista { get; set; }
        private string categoriaSelecionada;
        private int indiceClassificacaoFiscalSelecionado;
        private string textoPesquisa;

        private ETipoProduto TipoProduto { get; }

        private string Titulo
        {
            get
            {
                switch (TipoProduto)
                {
                    case ETipoProduto.Item:
                        return "ITENS";
                    case ETipoProduto.Modificacao:
                        return "MODIFICAÇÕES";
                    case ETipoProduto.Servico:
                        return "SERVIÇOS";
                    case ETipoProduto.Ingrediente:
                        return "INGREDIENTES";
                    case ETipoProduto.Credito:
                        return "CREDITO";
                    default:
                        return "ITENS";
                }
            }
        }

        private List<ProdutoCategoriaProdutoInformation> ListaProdutoCategoriaProduto;

        private frmProduto()
        {
            InitializeComponent();
        }

        internal frmProduto(ETipoProduto tipoProduto) : this()
        {
            TipoProduto = tipoProduto;
        }

        private void frmProduto_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            lblTitulo.Text = Titulo;
            if (TipoProduto != ETipoProduto.Item)
            {
                cbbCategoria.Visible = false;
                dgvPrincipal.Columns[nameof(Categoria)].Visible = false;
            }
            Refresh();
            textoPesquisa = string.Empty;
            categoriaSelecionada = "Todos";
            Carregar();
            indiceClassificacaoFiscalSelecionado = 0;
        }

        private void Carregar()
        {
            CarregarLista();
            AtualizarGrid();
        }

        private void CarregarCategorias()
        {
            ListaProdutoCategoriaProduto = ProdutoCategoriaProduto.ListarComCategoria();
            List<CategoriaProdutoInformation> listaCategoria = CategoriaProduto.Listar().OrderBy(l => l.Nome).ToList();

            cbbCategoria.Items.Clear();
            cbbCategoria.Items.Add("Todos");
            cbbCategoria.Items.Add("Sem Categoria");

            int ind = -1, i = 2;

            if (categoriaSelecionada == "Todos")
                ind = 0;
            else if (categoriaSelecionada == "Sem Categoria")
                ind = 1;

            foreach (var item in listaCategoria)
            {
                if (ind == -1 && item.IDCategoriaProduto.Value == Convert.ToInt32(categoriaSelecionada))
                    ind = i;
                cbbCategoria.Items.Add(item);
                i++;
            }
            cbbCategoria.SelectedIndex = ind;
        }

        private void CarregarLista()
        {

            Lista = Produto.ListarNaoExcluidosDAL();
            CarregarCategorias();
            cbbClassificacaoFiscal.SelectedIndex = indiceClassificacaoFiscalSelecionado;
        }

        private void AtualizarGrid()
        {
            dgvPrincipal.SuspendLayout();
            if (Lista != null)
            {
                Int32? idCategoriaProduto;
                if (categoriaSelecionada == "Todos")
                    idCategoriaProduto = 0;
                else if (categoriaSelecionada == "Sem Categoria")
                    idCategoriaProduto = -1;
                else
                    idCategoriaProduto = Convert.ToInt32(categoriaSelecionada);

                var produtoServico = Lista.Where(p => p.IDProduto == ProdutoInformation.IDProdutoServico).FirstOrDefault();
                var listaFiltrada = Lista.Where(p => p.TipoProduto.IDTipoProduto == (int)TipoProduto).ToList();

                if (ConfiguracoesSistema.Valores.ServicoComoItem && TipoProduto==ETipoProduto.Item)
                    listaFiltrada.Insert(0, produtoServico);

                switch (cbbClassificacaoFiscal.SelectedIndex)
                {
                    case 1:
                        listaFiltrada = listaFiltrada.Where(p => p.ClassificacaoFiscal != null && p.ClassificacaoFiscal.IDClassificacaoFiscal.HasValue).ToList();
                        break;
                    case 2:
                        listaFiltrada = listaFiltrada.Where(p => p.ClassificacaoFiscal == null || p.ClassificacaoFiscal.IDClassificacaoFiscal.HasValue == false).ToList();
                        break;
                    default:
                        break;
                }

                var lista = from produto in listaFiltrada
                            where
                                (CultureInfo.CurrentCulture.CompareInfo.IndexOf(produto.Nome, textoPesquisa, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0
                                || CultureInfo.CurrentCulture.CompareInfo.IndexOf(produto.IDProduto.ToString(), textoPesquisa, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0) &&
                                (
                                    (idCategoriaProduto == 0) ||
                                    (idCategoriaProduto == -1 && !ListaProdutoCategoriaProduto.Select(pcp => pcp.Produto).Contains(produto)) ||
                                    (ListaProdutoCategoriaProduto
                                    .Where(pcp => pcp.CategoriaProduto?.IDCategoriaProduto == idCategoriaProduto)
                                    .Select(pcp => pcp.Produto)
                                    .ToList()
                                    .Contains(produto))
                                //(produto.CategoriaProduto != null && produto.CategoriaProduto.IDCategoriaProduto == idCategoriaProduto)
                                )
                            orderby produto.Nome
                            select new { produto.IDProduto, Categoria = NomearCategoria(produto), produto.Nome, produto.Ativo, produto.ValorUnitario };

                lblContagem.Text = string.Format("({0})", lista.Count());

                dgvPrincipal.DataSource = lista.ToArray();
                dgvPrincipal.ClearSelection();
            }
            dgvPrincipal.ResumeLayout();
        }

        private string NomearCategoria(ProdutoInformation produto)
        {
            var categorias = ListaProdutoCategoriaProduto.Where(pcp => pcp.Produto.IDProduto == produto.IDProduto).ToList();
            if (categorias != null && categorias.Count == 1)
            {
                return categorias[0].CategoriaProduto?.Nome;
            }
            else if (categorias != null && categorias.Count > 1)
            {
                return string.Format("{0}(+{1})", categorias.First().CategoriaProduto?.Nome, categorias.Count - 1);
            }
            return null;
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            textoPesquisa = txtPesquisa.Text?.ToLower();
            AtualizarGrid();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            using (frmProdutoEditar frm = new frmProdutoEditar(Lista, TipoProduto))
            {
                frm.ShowDialog();
            }

            Carregar();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                Int32 idProduto = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDProduto"].Value);

                frmProdutoEditar frm = new frmProdutoEditar(idProduto, Lista);
                frm.ShowDialog();

                CarregarLista();
                AtualizarGrid();
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja alterar!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                Int32 idProduto = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDProduto"].Value);
                if(idProduto == ProdutoInformation.IDProdutoServico)
                {
                    MessageBox.Show("Este item não pode ser excluído", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    DialogResult r = MessageBox.Show("Tem certeza que deseja excluir esse item?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (r == DialogResult.Yes)
                    {
                        try
                        {

                            Produto.Excluir(idProduto);
                            Carregar();
                        }
                        catch (Exception _e)
                        {
                            MessageBox.Show(_e.Message, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja excluir!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            categoriaSelecionada = cbbCategoria.SelectedItem.ToString();
            if (categoriaSelecionada != "Todos" && categoriaSelecionada != "Sem Categoria")
                categoriaSelecionada = ((CategoriaProdutoInformation)cbbCategoria.SelectedItem).IDCategoriaProduto.Value.ToString();
            AtualizarGrid();
        }

        private void dgvPrincipal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }

        private void frmProduto_VisibleChanged(object sender, EventArgs e)
        {
            Carregar();
        }

        private void dgvPrincipal_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvPrincipal.Columns["Categoria"].Index)
            {
                var cellCategoria = dgvPrincipal.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cellCategoria.Value != null && string.IsNullOrEmpty(cellCategoria.Value.ToString()) == false)
                {
                    var cellProduto = dgvPrincipal.Rows[e.RowIndex].Cells[dgvPrincipal.Columns["IDProduto"].Index];
                    cellCategoria.ToolTipText = CategoriasPorProduto(Convert.ToInt32(cellProduto.Value));
                }
            }
        }

        private string CategoriasPorProduto(int idProduto)
        {
            var listaCategorias = ListaProdutoCategoriaProduto.Where(pcp => pcp.Produto.IDProduto == idProduto).ToList();
            string categorias = string.Empty;

            foreach (var pcp in listaCategorias)
            {
                categorias += pcp.CategoriaProduto.Nome + "\n";
            }

            if (categorias.Length > 1)
            {
                categorias = categorias.Substring(0, categorias.Length - 1);
            }

            return categorias;
        }

        private void ckbSemClassificacaoFIscal_CheckedChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void cbbClassificacaoFiscal_SelectedIndexChanged(object sender, EventArgs e)
        {
            indiceClassificacaoFiscalSelecionado = cbbClassificacaoFiscal.SelectedIndex;
            AtualizarGrid();
        }
    }
}
