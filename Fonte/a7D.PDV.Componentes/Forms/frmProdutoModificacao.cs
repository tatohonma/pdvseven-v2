using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public partial class frmProdutoModificacao : Form
    {
        private PedidoProdutoInformation pedidoProduto;
        private int idUser;
        private int idPDV;

        public frmProdutoModificacao(ref PedidoProdutoInformation pedidoProduto, int idFrmUser, int idFrmPdv)
        {
            InitializeComponent();

            this.idPDV = idFrmPdv;
            this.idUser = idFrmUser;
            this.pedidoProduto = pedidoProduto;
            if (pedidoProduto.ListaModificacao == null)
                pedidoProduto.ListaModificacao = new List<PedidoProdutoInformation>();
            txtNotas.Text = pedidoProduto.Notas;
        }

        private void frmProdutoModificacao_Load(object sender, EventArgs e)
        {
            List<ProdutoPainelModificacaoInformation> listaPainelModificacao = null;
            lblProduto.Text = pedidoProduto.Produto.Nome;
            listaPainelModificacao = ProdutoPainelModificacao.Listar(pedidoProduto.Produto.IDProduto.Value);

            object[] row;

            foreach (var painel in listaPainelModificacao)
            {
                foreach (var produto in painel.PainelModificacao.ListaProduto)
                {
                    if (produto.Produto == null)
                        continue;

                    row = new object[]
                    {
                        produto.Produto.IDProduto,
                        painel.PainelModificacao.IDPainelModificacao.ToString(),
                        produto.Produto.Nome,
                        produto.Produto.ValorUnitario.Value.ToString("#,##0.00"),
                        pedidoProduto.ListaModificacao.Any(pp => pp.Produto.IDProduto == produto.Produto.IDProduto),
                    };

                    dgvItens.Rows.Add(row);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            pedidoProduto.ListaModificacao = new List<PedidoProdutoInformation>();
            foreach (DataGridViewRow item in dgvItens.Rows)
            {
                if (Convert.ToBoolean(item.Cells["Selecionado"].Value) == true)
                {
                    var pedidoProdutoModificacao = new PedidoProdutoInformation();
                    pedidoProdutoModificacao.PedidoProdutoPai = pedidoProduto;
                    pedidoProdutoModificacao.Produto = new ProdutoInformation();

                    pedidoProdutoModificacao.CodigoAliquota = null;
                    pedidoProdutoModificacao.ValorUnitario = Convert.ToDecimal(item.Cells["Valor"].Value);
                    pedidoProdutoModificacao.Quantidade = 1;

                    pedidoProdutoModificacao.Produto.IDProduto = Convert.ToInt32(item.Cells["IDProduto"].Value);
                    pedidoProdutoModificacao.Produto.Nome = item.Cells["Nome"].Value.ToString();
                    pedidoProdutoModificacao.Produto.TipoProduto = TipoProduto.Carregar((int)ETipoProduto.Modificacao);
                    pedidoProdutoModificacao.PainelModificacao = new PainelModificacaoInformation { IDPainelModificacao = Convert.ToInt32(item.Cells["IDPainelModificacao"].Value) };

                    pedidoProdutoModificacao.PDV = new PDVInformation { IDPDV = idPDV };
                    pedidoProdutoModificacao.Usuario = new UsuarioInformation { IDUsuario = idUser };

                    pedidoProdutoModificacao.Cancelado = false;

                    pedidoProduto.ListaModificacao.Add(pedidoProdutoModificacao);
                }
            }

            pedidoProduto.Notas = txtNotas.Text;

            Close();
        }

        private void dgvItens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvItens.Rows.Count <= 0)
                return;

            var selecionado = Convert.ToBoolean(dgvItens.Rows[e.RowIndex].Cells["Selecionado"].Value);
            dgvItens.Rows[e.RowIndex].Cells["Selecionado"].Value = !selecionado;
        }

        private void dgvItens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvItens.Rows.Count <= 0)
                return;

            var selecionado = Convert.ToBoolean(dgvItens.Rows[e.RowIndex].Cells["Selecionado"].Value);
            dgvItens.Rows[e.RowIndex].Cells["Selecionado"].Value = !selecionado;
        }
    }
}
