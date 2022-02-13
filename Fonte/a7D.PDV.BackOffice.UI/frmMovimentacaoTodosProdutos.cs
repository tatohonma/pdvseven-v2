using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmMovimentacaoTodosProdutos : Form
    {
        private List<ProdutoInformation> Produtos { get; set; }

        public ProdutoInformation Produto { get; set; }

        public frmMovimentacaoTodosProdutos(List<ProdutoInformation> produtos)
        {
            Produtos = produtos;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void frmMovimentacaoTodosProdutos_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            Filtrar();
        }

        private void Filtrar()
        {
            var lista = Produtos.Select(p => new { p.IDProduto, p.Nome, p.ValorUnitario });
            if (string.IsNullOrWhiteSpace(txtPesquisa.Text) == false)
                lista = lista.Where(l => l.Nome.ToLower().Contains(txtPesquisa.Text.ToLower()));

            dgvPrincipal.DataSource = lista.ToArray();
        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                Fechar();
            }
        }

        private void Fechar()
        {
            Produto = BLL.Produto.CarregarCompleto(Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDProduto"].Value));
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtPesquisa_KeyDown(object sender, KeyEventArgs e)
        {
            Filtrar();
        }

        private void dgvPrincipal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Fechar();
        }

        private void dgvPrincipal_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Fechar();
        }
    }
}
