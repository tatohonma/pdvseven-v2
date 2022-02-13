using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmSaidaAvulsa : FormTouch
    {
        private List<PedidoProdutoInformation> ListaPedidoProduto;
        private List<ProdutoInformation> ListaProdutos;

        public frmSaidaAvulsa()
        {
            InitializeComponent();
        }

        private void frmSaidaAvulsa_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            ListaProdutos = Produto.Listar(new ProdutoInformation()
            {
                Excluido = false,
                Ativo = true,
                ControlarEstoque = true
            })
                .Where(p => ProdutoReceita.ListarPorProduto(p).Count() == 0)
                .OrderBy(p => p.Nome)
                .ToList();

            //ListaProdutos = Produto.Listar(new ProdutoInformation() { })
            //                .Where(p => p.Excluido == false)
            //                .Where(p => p.Ativo == true)
            //                .Where(p => p.ControlarEstoque == true)
            //.Where(p => ProdutoReceita.ListarPorProduto(p).Count() == 0)
            //.OrderBy(p => p.Nome)
            //.ToList();

            ListaPedidoProduto = new List<PedidoProdutoInformation>();

            ListarItens();
        }

        private void ListarItens()
        {
            string nome = txtNome.Text;

            var list = from l in ListaProdutos
                       where
                          (l.Nome.ToLower().Contains(nome.ToLower()))
                          ||
                          (l.Codigo != null && l.Codigo.ToLower().Contains(nome.ToLower()))
                       orderby l.Nome
                       select new { l.IDProduto, l.Codigo, l.Nome, l.ValorUnitario };

            dgvItens.DataSource = list.ToArray();
        }

        private void btnMenos_Click(object sender, EventArgs e)
        {
            var qtd = Convert.ToInt32(txtQuantidade.Text);

            if (qtd > 1)
                qtd--;

            txtQuantidade.Text = qtd.ToString();
        }

        private void btnMais_Click(object sender, EventArgs e)
        {
            var qtd = Convert.ToInt32(txtQuantidade.Text);
            qtd++;

            txtQuantidade.Text = qtd.ToString();
        }

        private void btnAdicionarProduto_Click(object sender, EventArgs e)
        {
            AdicionarProduto();
        }

        private void AdicionarProduto()
        {
            if (dgvItens.SelectedRows.Count > 0)
            {
                var minID = ListaPedidoProduto.Where(l => l.IDPedidoProduto <= 0).Min(l => l.IDPedidoProduto);
                int idPedidoProduto;
                if (minID == null)
                    idPedidoProduto = -1;
                else
                    idPedidoProduto = minID.Value - 1;

                var idProduto = Convert.ToInt32(dgvItens.SelectedRows[0].Cells["IDProduto"].Value);
                ProdutoInformation produto = Produto.Carregar(idProduto);
                produto.Unidade = BLL.Unidade.Carregar(produto.Unidade.IDUnidade.Value);
                var valorUnitario = produto.ValorUnitario.Value;
                var codigoAliquita = produto.CodigoAliquota;
                var notas = "";

                if (valorUnitario == 0)
                {
                    frmPedidoProdutosValor frm = new frmPedidoProdutosValor();
                    frm.ShowDialog();

                    if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        valorUnitario = frm.Valor;
                        notas = frm.Notas;
                    }
                }

                //if (produto.CodigoAliquota == null || produto.CodigoAliquota == "")
                //    codigoAliquita = CodigoAliquitaPadrao;

                PedidoProdutoInformation pedidoProduto = new PedidoProdutoInformation();
                pedidoProduto.IDPedidoProduto = idPedidoProduto;
                pedidoProduto.CodigoAliquota = codigoAliquita;
                pedidoProduto.ValorUnitario = valorUnitario;
                pedidoProduto.Quantidade = Convert.ToDecimal(txtQuantidade.Text);
                pedidoProduto.Notas = notas;
                pedidoProduto.Cancelado = false;
                pedidoProduto.Usuario = AC.Usuario;
                pedidoProduto.PDV = AC.PDV;

                pedidoProduto.Produto = new ProdutoInformation
                {
                    IDProduto = idProduto,
                    Nome = produto.Nome,
                    Unidade = produto.Unidade
                };

                ListaPedidoProduto.Add(pedidoProduto);

                txtNome.Text = "";
                txtQuantidade.Text = "1";

                VisualizarProdutos();
            }

            txtNome.Focus();
        }

        private void VisualizarProdutos()
        {
            dgvItensSelecionados.Rows.Clear();

            foreach (var item in ListaPedidoProduto)
            {
                var row = new object[]
                {
                    null,
                    null,
                    item.Produto.Nome + Environment.NewLine + item.Notas,
                    item.Produto.Unidade.Nome,
                    item.Quantidade,
                    item.ValorUnitarioString,
                    item.ValorTotalString,
                    item.IDPedidoProduto
                };

                dgvItensSelecionados.Rows.Add(row);

                if (item.ListaModificacao != null)
                {
                    foreach (var modificacao in item.ListaModificacao)
                    {
                        row = new object[]
                        {
                            null,
                            null,
                            "- " + modificacao.Produto.Nome,
                            modificacao.Quantidade.Value * item.Quantidade.Value,
                            modificacao.ValorUnitarioString,
                            (item.Quantidade.Value * modificacao.ValorTotal).ToString("N2"),
                            modificacao.IDPedidoProduto
                        };

                        dgvItensSelecionados.Rows.Add(row);
                        dgvItensSelecionados.Rows[dgvItensSelecionados.Rows.Count - 1].Cells[0].Value = Properties.Resources.semImagem;
                        dgvItensSelecionados.Rows[dgvItensSelecionados.Rows.Count - 1].Cells[1].Value = Properties.Resources.semImagem;
                    }
                }
            }
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            ListarItens();
        }

        private void dgvItensSelecionados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 idPedidoProduto;
            PedidoProdutoInformation pedidoProduto;

            if (e.RowIndex >= 0)
            {
                switch (e.ColumnIndex)
                {
                    case 0:
                        idPedidoProduto = Convert.ToInt32(dgvItensSelecionados.Rows[e.RowIndex].Cells["IDPedidoProduto"].Value);
                        ListaPedidoProduto = ListaPedidoProduto.Where(pp => pp.IDPedidoProduto != idPedidoProduto).ToList();
                        VisualizarProdutos();

                        break;
                    case 1:
                        idPedidoProduto = Convert.ToInt32(dgvItensSelecionados["IDPedidoProduto", e.RowIndex].Value);

                        if (idPedidoProduto != 0)
                        {
                            pedidoProduto = ListaPedidoProduto.First(l => l.IDPedidoProduto == idPedidoProduto);
                            int? minID = ListaPedidoProduto.Where(l => l.IDPedidoProduto <= 0).Min(l => l.IDPedidoProduto);
                            if (pedidoProduto.Produto.AssistenteModificacoes == true)
                            {
                                var form = new frmPedidoProdutosModificacoesAssistente(pedidoProduto, minID);
                                if (form.ShowDialog() == DialogResult.OK)
                                    form.PainelRetornaProdutoItens(pedidoProduto);
                                //pedidoProduto.Notas = form.Notas;
                            }
                            else
                            {
                                var form = new frmPedidoProdutosModificacoes(pedidoProduto, minID);
                                form.ShowDialog();

                                pedidoProduto.ListaModificacao = form.ListaProduto;
                                pedidoProduto.Notas = form.Notas;
                            }

                            VisualizarProdutos();
                        }

                        break;
                }
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            Enabled = false;
            Cursor = Cursors.WaitCursor;
            dgvItens.Cursor = Cursor;
            dgvItensSelecionados.Cursor = Cursor;
            Refresh();

            try
            {
                EntradaSaida.Movimentar(Guid.NewGuid().ToString(), ListaPedidoProduto);
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E60C, ex);
                Enabled = true;
                Cursor = Cursors.Default;
                dgvItens.Cursor = Cursor;
                dgvItensSelecionados.Cursor = Cursor;
            }

            Close();
        }

        private void dgvItens_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AdicionarProduto();
        }

        private void dgvItens_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AdicionarProduto();
        }
    }
}
