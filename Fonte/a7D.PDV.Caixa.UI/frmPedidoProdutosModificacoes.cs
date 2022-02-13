using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPedidoProdutosModificacoes : FormTouch
    {
        PedidoProdutoInformation PedidoProduto1;
        public List<PedidoProdutoInformation> ListaProduto { get; set; }
        public String Notas { get; set; }
        private Dictionary<PainelModificacaoInformation, List<ProdutoInformation>> paineis;
        private int minId;

        public frmPedidoProdutosModificacoes(PedidoProdutoInformation pedidoProduto, int? minId)
        {
            InitializeComponent();

            PedidoProduto1 = pedidoProduto;
            this.minId = minId ?? 0;
        }

        private void frmPedidoProdutosModificacoes_Load(object sender, EventArgs e)
        {
            Int32 idProduto = 0;
            try
            {
                lblProduto.Text = PedidoProduto1.Produto.Nome;
                txtNotas.Text = PedidoProduto1.Notas;

                List<ProdutoPainelModificacaoInformation> listaPainelModificacao = ProdutoPainelModificacao.Listar(PedidoProduto1.Produto.IDProduto.Value);

                paineis = new Dictionary<PainelModificacaoInformation, List<ProdutoInformation>>();

                foreach (var painel in listaPainelModificacao)
                {
                    foreach (var produto in painel.PainelModificacao.ListaProduto)
                    {
                        if (paineis.ContainsKey(painel.PainelModificacao))
                            paineis[painel.PainelModificacao].Add(produto.Produto);
                        else
                            paineis.Add(painel.PainelModificacao, new List<ProdutoInformation> { produto.Produto });

                        var row = new object[]
                        {
                        painel.PainelModificacao.IDPainelModificacao,
                        produto.Produto.IDProduto,
                        produto.Produto.Nome,
                        produto.Produto.ValorUnitario.Value.ToString("#,##0.00")
                        };

                        dgvItens.Rows.Add(row);
                    }
                }

                foreach (DataGridViewRow item in dgvItens.Rows)
                {
                    idProduto = Convert.ToInt32(item.Cells["IDProduto"].Value);
                    if (PedidoProduto1.ListaModificacao != null && PedidoProduto1.ListaModificacao.FirstOrDefault(l => l.Produto.IDProduto == idProduto) != null)
                        item.Cells["Selecionado"].Value = true;
                }
            }
            catch (Exception ex)
            {   
                ex.Data.Add("Produto.IDProduto", PedidoProduto1?.Produto?.IDProduto);
                ex.Data.Add("Produto.Nome", PedidoProduto1?.Produto?.Nome);
                ex.Data.Add("idProduto", idProduto);
                throw new ExceptionPDV(CodigoErro.E803, ex);
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            ListaProduto = new List<PedidoProdutoInformation>();
            PedidoProdutoInformation pedidoProduto;

            foreach (DataGridViewRow item in dgvItens.Rows)
            {
                if (Convert.ToBoolean(item.Cells["Selecionado"].Value) == true)
                {
                    pedidoProduto = new PedidoProdutoInformation()
                    {
                        IDPedidoProduto = --minId,
                        PedidoProdutoPai = new PedidoProdutoInformation(),
                        Produto = new ProdutoInformation(),

                        CodigoAliquota = null,
                        ValorUnitario = Convert.ToDecimal(item.Cells["Valor"].Value),
                        Quantidade = 1,

                        PDV = AC.PDV,
                        Usuario = AC.Usuario,

                        Cancelado = false
                    };
                    pedidoProduto.PedidoProdutoPai.IDPedidoProduto = PedidoProduto1.IDPedidoProduto.Value;

                    pedidoProduto.Produto.IDProduto = Convert.ToInt32(item.Cells["IDProduto"].Value);
                    pedidoProduto.Produto.Nome = item.Cells["Nome"].Value.ToString();
                    pedidoProduto.PainelModificacao = new PainelModificacaoInformation { IDPainelModificacao = Convert.ToInt32(item.Cells["IDPainelModificacao"].Value) };

                    ListaProduto.Add(pedidoProduto);
                }
            }

            Notas = txtNotas.Text;

            this.Close();
        }

        private void dgvItens_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvItens.SelectedRows.Count > 1)
            {
                for (int i = 1; i < dgvItens.SelectedRows.Count; i++)
                {
                    dgvItens.SelectedRows[i].Selected = false;

                }
            }
        }

        private void dgvItens_CellValueChanged(object sender, EventArgs e)
        {
            foreach (var painel in paineis.Keys)
            {
                var produtos = paineis[painel].Select(p => p.IDProduto.Value).ToList();
                var produtosSelecionados = new List<DataGridViewRow>();
                var maximo = painel.Maximo;
                if (maximo.HasValue && maximo > 0)
                {
                    foreach (DataGridViewRow row in dgvItens.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["IDPainelModificacao"].Value) == painel.IDPainelModificacao.Value
                            && Convert.ToBoolean(row.Cells["Selecionado"].Value))
                            produtosSelecionados.Add(row);
                    }
                    if (produtosSelecionados.Count > maximo.Value)
                    {
                        for (int i = maximo.Value; i < produtosSelecionados.Count; i++)
                            produtosSelecionados[i].Cells["Selecionado"].Value = false;
                    }
                }
            }
            dgvItens.Refresh();
        }

        private void dgvItens_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //dgvItens.Refresh();
        }
    }
}
