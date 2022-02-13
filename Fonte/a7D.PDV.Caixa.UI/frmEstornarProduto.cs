using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmEstornarProduto : FormTouch
    {

        public List<PedidoProdutoInformation> Linhas { get; private set; }
        private readonly string GUIDPedido;

        public frmEstornarProduto(List<PedidoProdutoInformation> pedidoProdutos)
        {
            InitializeComponent();
            Linhas = pedidoProdutos;
            var pedidoproduto = Linhas.FirstOrDefault();
            GUIDPedido = pedidoproduto != null ? Pedido.Carregar(pedidoproduto.Pedido.IDPedido.Value).GUIDIdentificacao : string.Empty;
        }

        public frmEstornarProduto(PedidoInformation pedido) : this(pedido.ListaProduto) { }

        public frmEstornarProduto(PedidoProdutoInformation pedidoProduto) : this(new List<PedidoProdutoInformation>() { pedidoProduto }) { }

        private void frmEstornarProduto_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            foreach (var item in Linhas)
                dgvPrincipal.Rows.Add(new object[] { item.IDPedidoProduto.Value, false, item.Produto.Nome, item.Quantidade });
        }

        private void btnEstornar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvPrincipal.Rows)
            {
                var cb = row.Cells["Estornar"] as DataGridViewCheckBoxCell;
                if (cb != null && Convert.ToBoolean(cb.Value))
                    Linhas.First(p => p.IDPedidoProduto.Value == Convert.ToInt32(row.Cells["IDPedidoProduto"].Value)).RetornarAoEstoque = true;
            }
            Close();
        }
    }
}
