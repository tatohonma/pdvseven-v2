using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Terminal.UI
{
    public partial class frmDetalhesPedidoProduto : FormTouch
    {
        String GUIDIdentificacao_selecionado;

        public frmDetalhesPedidoProduto(String guidIdentificacao_selecionado)
        {
            GUIDIdentificacao_selecionado = guidIdentificacao_selecionado;
            InitializeComponent();
        }

        private void frmDetalhesPedidoProduto_Load(object sender, EventArgs e)
        {
            CarregarLista();
        }

        private void CarregarLista()
        {
            PedidoInformation pedido = new PedidoInformation();
            pedido = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);

            #region Carregar lista Produtos
            dgvItens.Rows.Clear();
            Object[] row;

            Font f = dgvItens.DefaultCellStyle.Font;

            foreach (var item in pedido.ListaProduto.Where(l => l.Produto.TipoProduto.IDTipoProduto == 10 || l.Produto.TipoProduto.IDTipoProduto == 30).ToList())
            {
                row = new Object[] 
                    { 
                        null,
                        item.Produto.Nome + System.Environment.NewLine + item.Notas,
                        item.Quantidade,
                        item.ValorUnitario.Value.ToString("#,##0.00"),
                        item.ValorTotal.ToString("#,##0.00"),
                        item.IDPedidoProduto,
                        item.PDV.Nome,
                        item.Usuario.Nome,
                        item.DtInclusao.Value.ToString("HH:mm")
                    };

                dgvItens.Rows.Add(row);
                if (item.Cancelado == true)
                    dgvItens.Rows[dgvItens.Rows.Count - 1].DefaultCellStyle.Font = new Font(f, FontStyle.Strikeout);

                if (item.ListaModificacao != null)
                {
                    foreach (var modificacao in item.ListaModificacao)
                    {
                        row = new Object[] 
                        { 
                            null,
                            "- " + modificacao.Produto.Nome,
                            modificacao.Quantidade.Value * item.Quantidade.Value,
                            modificacao.ValorUnitario.Value.ToString("#,##0.00"),
                            (item.Quantidade.Value * modificacao.ValorTotal).ToString("#,##0.00"),
                            modificacao.IDPedidoProduto,
                            "",
                            "",
                            ""
                        };

                        dgvItens.Rows.Add(row);

                        if (item.Cancelado == true || modificacao.Cancelado == true)
                            dgvItens.Rows[dgvItens.Rows.Count - 1].DefaultCellStyle.Font = new Font(f, FontStyle.Strikeout);
                    }
                }
            }
            #endregion
        }

        private void dgvItens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 idPedidoProduto;
            PedidoProdutoInformation pedidoProduto;

            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                idPedidoProduto = Convert.ToInt32(dgvItens.SelectedRows[0].Cells["IDPedidoProduto"].Value);
                pedidoProduto = PedidoProduto.Carregar(idPedidoProduto);

                if (pedidoProduto.Produto.TipoProduto.IDTipoProduto == 30)
                {
                    MessageBox.Show("Não é possível excluir esse tipo de produto!");
                }
                else
                {
                    var formAutenticacao = new frmAutenticacaoTouch(false, false, true, false, false);
                    if (formAutenticacao.ShowDialog() == DialogResult.OK)
                    {
                        frmPedidoProdutoCancelamento formCancelamento = new frmPedidoProdutoCancelamento(idPedidoProduto, formAutenticacao.Usuario1.IDUsuario.Value);
                        formCancelamento.ShowDialog();
                    }
                }
            }

            CarregarLista();
        }
    }
}
