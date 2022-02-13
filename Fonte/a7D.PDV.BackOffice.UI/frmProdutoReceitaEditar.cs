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
    public partial class frmProdutoReceitaEditar : Form
    {
        private ProdutoReceitaInformation ProdutoReceita1 { get; set; }

        public frmProdutoReceitaEditar(int idProduto)
        {
            ProdutoReceita1 = new ProdutoReceitaInformation
            {
                Produto = new ProdutoInformation
                {
                    IDProduto = idProduto
                }
            };
            InitializeComponent();
        }

        public frmProdutoReceitaEditar(ProdutoReceitaInformation produtoReceita)
        {
            ProdutoReceita1 = produtoReceita;
            InitializeComponent();
        }

        private void frmProdutoReceitaEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            var listaProdutos = Produto.Listar(new ProdutoInformation() { Excluido = false, Ativo = true });
            listaProdutos.Insert(0, new ProdutoInformation { IDProduto = -1, Nome = "Selecione..." });
            cbbProduto.DisplayMember = "Nome";
            cbbProduto.ValueMember = "IDProduto";
            cbbProduto.DataSource = listaProdutos;

            cbbUnidade.ValueMember = "IDUnidade";
            cbbUnidade.DisplayMember = "Nome";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbbProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idProduto = Convert.ToInt32(cbbProduto.SelectedValue);
            if (idProduto < 0)
            {

            }
            else
            {
                var unidade = Produto.Carregar(idProduto).Unidade;
                var conversoes = ConversaoUnidade.ListarUnidadesConversaoPara(unidade);
                conversoes.Insert(0, unidade);
                cbbUnidade.DataSource = conversoes;
            }
        }
    }
}
