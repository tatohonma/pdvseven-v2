using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmProdutoTraducao : Form
    {

        public ProdutoTraducaoInformation Traducao { get; set; }
        public bool Cancelado { get; set; }
        private ProdutoInformation produto;
        private IdiomaInformation idioma;

        //public frmProdutoTraducao()
        //{
        //    InitializeComponent();
        //}

        public frmProdutoTraducao(ProdutoInformation produto, IdiomaInformation idioma)
        {
            InitializeComponent();
            Traducao = null;
            PreencherCampos(produto, idioma);
        }


        public frmProdutoTraducao(ProdutoTraducaoInformation traducao)
        {
            InitializeComponent();
            Traducao = traducao;
            PreencherCampos(Traducao.Produto, Traducao.Idioma);
        }

        private void PreencherCampos(ProdutoInformation produto, IdiomaInformation idioma)
        {
            this.produto = produto;
            this.idioma = idioma;

            txtNome.Text = produto.Nome;
            txtDescricao.Text = produto.Descricao;

            if (Traducao != null)
            {
                txtNomeTraducao.Text = Traducao.Nome;
                txtDescricaoTraducao.Text = Traducao.Descricao;
            }

            lblIdioma.Text = idioma.Nome;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            if (EstaVazio(txtNomeTraducao))
            {
                MessageBox.Show("Campo \"NOME\" é obrigatório!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Traducao == null)
            {
                Traducao = new ProdutoTraducaoInformation();
                Traducao.StatusModel = StatusModel.Novo;
            }
            else
                Traducao.StatusModel = StatusModel.Alterado;

            Traducao.Produto = produto;
            Traducao.Idioma = idioma;
            Traducao.Nome = txtNomeTraducao.Text;
            Traducao.Descricao = txtDescricaoTraducao.Text;
            Cancelado = false;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Cancelado = true;
            Close();
        }

        private void txtNomeTraducao_Validating(object sender, CancelEventArgs e)
        {
            EstaVazio(txtNomeTraducao);
        }

        private bool EstaVazio(TextBox txt)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                errorProvider1.SetError(txt, "Campo obrigatório");
                return true;
            }
            else
            {
                errorProvider1.SetError(txt, string.Empty);
                return false;

            }
        }

        private void frmProdutoTraducao_Load(object sender, EventArgs e)
        {
            GA.Post(this);
        }
    }
}
