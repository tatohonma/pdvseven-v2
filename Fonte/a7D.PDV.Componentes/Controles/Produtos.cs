using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Componentes.Controles
{
    public partial class Produtos : UserControl
    {
        public event EventHandler ProdutoSelecionado;
        public bool ListarCreditos { get; set; }
        private List<int> indisponivelPorCategoria;
        private List<BotaoItem> itens;

        // Cache dos produtos 
        private static Dictionary<int, List<BotaoItem>> cacheBotoes = new Dictionary<int, List<BotaoItem>>();

        public static int TempoCache { get; set; } = 1;

        public Produtos()
        {
            InitializeComponent();
            if (TempoCache > 0)
                tmrClear.Interval = TempoCache * 60 * 1000;
            else
                tmrClear.Enabled = false;
        }

        public void ListarProdutos(Int32 idCategorioProduto)
        {
            if (!cacheBotoes.TryGetValue(idCategorioProduto, out itens))
            {
                List<ProdutoInformation> listaProduto;
                if (idCategorioProduto == 0)
                    listaProduto = Produto.ListarSemCategorias(true); // Por causa do cache precisa listar sempre
                else
                    listaProduto = Produto.ListarPorCategorias(new int[] { idCategorioProduto }, true);

                if (indisponivelPorCategoria == null)
                    indisponivelPorCategoria = Produto.ProdutosIndisponivelPorCategoria();

                itens = listaProduto
                    .Select(i => new BotaoItem(i.IDProduto.Value, i.Nome, i.Disponibilidade == true && !indisponivelPorCategoria.Contains(i.IDProduto.Value), i))
                    .ToList();
                cacheBotoes.Add(idCategorioProduto, itens);
            }

            Produtos_Resize(null, null);
        }

        private void Produtos_Resize(object sender, EventArgs e)
        {
            if (DesignMode || itens == null)
                return;

            int itensPoLinha = 0;
            // Filta itens em cache removendo credito quando necessário
            BotaoGrid.CriaBotoes(this, itens.Where(p => ListarCreditos ? true : ((ProdutoInformation)p.Tag).TipoProduto.TipoProduto != EF.Enum.ETipoProduto.Credito), ProdutoSelecionado, ref itensPoLinha, BotaoGrid.Cinza, true);
        }

        private void tmrClear_Tick(object sender, EventArgs e)
        {
            indisponivelPorCategoria = null;
            cacheBotoes.Clear();
        }
    }
}
