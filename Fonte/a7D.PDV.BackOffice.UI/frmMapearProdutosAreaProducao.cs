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

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmMapearProdutosAreaProducao : Form
    {
        private List<ProdutoInformation> ListaProduto;

        public frmMapearProdutosAreaProducao()
        {
            InitializeComponent();
        }

        private void cbbAreaImpressao_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarListas();
        }

        private void AtualizarListas()
        {
            Cursor = Cursors.WaitCursor;
            Refresh();
            ProdutoInformation produto;

            Int32? idAreaImpressao = null;
            Int32? idCategoriaProduto = null;
            String nomeProduto = txtNomeProduto.Text;
            List<MapAreaImpressaoProdutoInformation> listaMap = new List<MapAreaImpressaoProdutoInformation>();

            if (cbbAreaImpressao.SelectedItem != null && cbbAreaImpressao.SelectedItem.ToString() != "")
            {
                idAreaImpressao = ((AreaImpressaoInformation)cbbAreaImpressao.SelectedItem).IDAreaImpressao;
                listaMap = MapAreaImpressaoProduto.ListarPorAreaImpressao(idAreaImpressao.Value).OrderBy(l => l.Produto.Nome).ToList();

                btnAdicionar.Enabled = true;
                btnAdicionarTodos.Enabled = true;
                btnRemover.Enabled = true;
                btnRemoverTodos.Enabled = true;
            }
            else
            {
                btnAdicionar.Enabled = false;
                btnAdicionarTodos.Enabled = false;
                btnRemover.Enabled = false;
                btnRemoverTodos.Enabled = false;
            }

            if (cbbCategoria.SelectedItem != null && cbbCategoria.SelectedItem.ToString() != "Todos")
                idCategoriaProduto = ((CategoriaProdutoInformation)cbbCategoria.SelectedItem).IDCategoriaProduto;

            ltbProdutos.Items.Clear();
            ltbProdutosMapeados.Items.Clear();

            List<ProdutoCategoriaProdutoInformation> listaCategoriasProduto = ProdutoCategoriaProduto.ListarCompleto().Where(pcp => pcp.Produto != null).ToList();

            if (idCategoriaProduto.HasValue)
                listaCategoriasProduto = listaCategoriasProduto.Where(pcp => pcp.CategoriaProduto.IDCategoriaProduto == idCategoriaProduto).ToList();

            //List<ProdutoInformation> listaProduto = ListaProduto.Where(l =>
            //    (
            //        (l.CategoriaProduto != null && l.CategoriaProduto.IDCategoriaProduto == idCategoriaProduto) ||
            //        (idCategoriaProduto == null)
            //    ) &&
            //    l.Nome.ToLower().Contains(nomeProduto.ToLower())).OrderBy(l => l.Nome).ToList();

            var listaProduto = listaCategoriasProduto
                .Where(pcp => pcp.Produto.Ativo == true)
                .Where(pcp => pcp.Produto.Excluido == false)
                .Select(pcp => pcp.Produto)
                .Distinct();
            if (!idCategoriaProduto.HasValue)
                listaProduto = listaProduto.Union(Produto.ListarCompletoSemCategoria());

            listaProduto = listaProduto.Where(p => p.Ativo == true);
            listaProduto = listaProduto.Where(p => p.Excluido == false);
            listaProduto = listaProduto.Where(p => p.TipoProduto.IDTipoProduto == 10);
            listaProduto = listaProduto.Where(p => p.IDProduto.Value != 2 && p.IDProduto.Value != 3 && p.IDProduto.Value != 4);

            foreach (var item in listaProduto.Where(p => p.Nome.ToLower().Contains(nomeProduto.ToLower())).OrderBy(p => p.Nome))
                ltbProdutos.Items.Add(item);

            foreach (var item in listaMap)
            {
                produto = ListaProduto.FirstOrDefault(l => l.IDProduto == item.Produto.IDProduto);
                if (produto != null)
                {
                    ltbProdutosMapeados.Items.Add(produto);
                    ltbProdutos.Items.Remove(produto);
                }
            }
            Cursor = Cursors.Default;
            Refresh();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            MapAreaImpressaoProdutoInformation map;
            AreaImpressaoInformation areaImpressao;

            if (cbbAreaImpressao.SelectedItem != null && cbbAreaImpressao.SelectedItem.ToString() != "")
            {
                areaImpressao = (AreaImpressaoInformation)cbbAreaImpressao.SelectedItem;

                foreach (ProdutoInformation item in ltbProdutos.SelectedItems)
                {
                    map = new MapAreaImpressaoProdutoInformation();
                    map.AreaImpressao = areaImpressao;
                    map.Produto = item;

                    MapAreaImpressaoProduto.Salvar(map);
                }

                AtualizarListas();
            }
        }

        private void btnAdicionarTodos_Click(object sender, EventArgs e)
        {
            MapAreaImpressaoProdutoInformation map;
            AreaImpressaoInformation areaImpressao;

            if (cbbAreaImpressao.SelectedItem != null && cbbAreaImpressao.SelectedItem.ToString() != "")
            {
                areaImpressao = (AreaImpressaoInformation)cbbAreaImpressao.SelectedItem;

                foreach (ProdutoInformation item in ltbProdutos.Items)
                {
                    map = new MapAreaImpressaoProdutoInformation();
                    map.AreaImpressao = areaImpressao;
                    map.Produto = item;

                    MapAreaImpressaoProduto.Salvar(map);
                }

                AtualizarListas();
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            AreaImpressaoInformation areaImpressao = (AreaImpressaoInformation)cbbAreaImpressao.SelectedItem;

            foreach (ProdutoInformation item in ltbProdutosMapeados.SelectedItems)
                MapAreaImpressaoProduto.Excluir(areaImpressao.IDAreaImpressao.Value, item.IDProduto.Value);

            AtualizarListas();
        }

        private void btnRemoverTodos_Click(object sender, EventArgs e)
        {
            AreaImpressaoInformation areaImpressao = (AreaImpressaoInformation)cbbAreaImpressao.SelectedItem;

            foreach (ProdutoInformation item in ltbProdutosMapeados.Items)
                MapAreaImpressaoProduto.Excluir(areaImpressao.IDAreaImpressao.Value, item.IDProduto.Value);

            AtualizarListas();
        }

        private void frmMapearProdutosAreaProducao_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            Carregar();
        }

        private void Carregar()
        {
            ListaProduto = Produto.Listar(new ProdutoInformation() { Excluido = false }).OrderBy(l => l.Nome).ToList();

            cbbAreaImpressao.Items.Clear();
            foreach (var item in AreaImpressao.ListarSomenteProducao().OrderBy(l => l.Nome).ToList())
                cbbAreaImpressao.Items.Add(item);

            cbbCategoria.Items.Clear();
            cbbCategoria.Items.Add("Todos");
            foreach (var item in CategoriaProduto.Listar().OrderBy(l => l.Nome).ToList())
                cbbCategoria.Items.Add(item);
        }

        private void txtNomeProduto_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarListas();
        }

        private void frmMapearProdutosAreaProducao_VisibleChanged(object sender, EventArgs e)
        {
            Carregar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            AtualizarListas();
        }
    }
}
