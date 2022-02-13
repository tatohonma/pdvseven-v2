using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using a7D.PDV.Componentes;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmDisponibilidadeProduto : FormTouch
    {
        public frmDisponibilidadeProduto()
        {
            InitializeComponent();
        }

        private void frmDisponibilidadeProduto_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            var listaCategoria = CategoriaProduto.Listar().OrderBy(l => l.Nome);

            //cbbCategoria.Items.Add("");
            foreach (var item in listaCategoria)
                cbbCategoria.Items.Add(item);

            if (cbbCategoria.Items.Count > 0)
                cbbCategoria.SelectedIndex = 0;
        }

        private void cbbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 idCategoriaProduto;

            if (cbbCategoria.SelectedItem == null || cbbCategoria.SelectedItem.ToString() == "")
            {
                ListarProdutos(null);
            }
            else
            {
                idCategoriaProduto = ((CategoriaProdutoInformation)cbbCategoria.SelectedItem).IDCategoriaProduto.Value;
                ListarProdutos(idCategoriaProduto);
            }
        }

        public void ListarProdutos(int? idCategorioProduto)
        {
            List<ProdutoDisponibilidade> listaProduto;
            pnlProdutos.Controls.Clear();

            if (idCategorioProduto == null)
            {
                return;
                //listaProduto = Produto.ListarAtivos().Select(p => new ProdutoDisponibilidade(p)).OrderBy(l => l.Nome).ToList();
            }
            else
            {
                listaProduto = ProdutoCategoriaProduto.ListarPorCategoriaCompleto(idCategorioProduto.Value).Select(pcp => new ProdutoDisponibilidade(pcp.Produto, pcp.CategoriaProduto.Disponibilidade.Value)).Where(p => p.Ativo == true && p.Excluido == false).Distinct().OrderBy(p => p.Nome).ToList();
                //listaProduto = Produto.ListarAtivos().Where(l => l.CategoriaProduto != null && l.CategoriaProduto.IDCategoriaProduto == idCategorioProduto).OrderBy(l => l.Nome).ToList();
            }

            Button btn;
            Int32 altura = 60;
            Int32 largura = 132;
            Int32 botoesPorLinha = (int)Math.Floor((decimal)Width / largura);
            Int32 linha = 0;
            Int32 i = 0;

            foreach (var item in listaProduto)
            {
                linha = (Int32)Math.Floor(i / (decimal)botoesPorLinha);

                btn = new Button();
                btn.Name = "btnProduto_" + item.IDProduto;
                btn.Text = item.Nome.ToUpper();
                btn.Width = largura;
                btn.Height = altura;
                btn.Location = new Point(largura * i - (botoesPorLinha * largura * linha), altura * linha);
                btn.Click += new EventHandler(this.btnProduto_Click);

                #region Aparencia
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btn.ForeColor = System.Drawing.Color.White;
                btn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                btn.UseVisualStyleBackColor = false;

                if (item.Disponibilidade == true)
                    btn.BackColor = Color.DarkGreen;
                else
                    btn.BackColor = Color.LightGreen;

                btn.Enabled = item.CategoriaInativa;
                #endregion

                pnlProdutos.Controls.Add(btn);

                i++;
            }
        }

        public void ListarCategorias()
        {
            var categorias = CategoriaProduto.Listar().OrderBy(c => c.Nome).ThenBy(c => c.Disponibilidade);
            pnCategorias.Controls.Clear();

            Int32 altura = 60;
            Int32 largura = 132;
            Int32 botoesPorLinha = (int)Math.Floor((decimal)Width / largura);
            Int32 linha = 0;
            Int32 i = 0;
            foreach (var item in categorias)
            {
                linha = (Int32)Math.Floor(i / (decimal)botoesPorLinha);

                var btn = new Button();
                btn.Name = "btnCategoria_" + item.IDCategoriaProduto;
                btn.Text = item.Nome.ToUpper();
                btn.Width = largura;
                btn.Height = altura;
                btn.Location = new Point(largura * i - (botoesPorLinha * largura * linha), altura * linha);
                btn.Click += new EventHandler(btnCategoria_Click);

                #region Aparencia
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                btn.ForeColor = Color.White;
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.UseVisualStyleBackColor = false;

                if (item.Disponibilidade == true)
                {
                    btn.BackColor = Color.DarkGreen;
                }
                else
                {
                    btn.BackColor = Color.LightGreen;
                }
                #endregion

                pnCategorias.Controls.Add(btn);

                i++;
            }
        }

        private void btnCategoria_Click(object sender, EventArgs e)
        {
            var ctr = (Control)sender;
            var idCategoria = Convert.ToInt32(ctr.Name.Substring(ctr.Name.LastIndexOf('_') + 1, ctr.Name.Length - ctr.Name.LastIndexOf('_') - 1));
            var categoria = CategoriaProduto.Carregar(idCategoria);

            bool disponivel = false;
            if (ctr.BackColor == Color.LightGreen)
                disponivel = true;

            categoria.Disponibilidade = disponivel;
            categoria.DtAlteracaoDisponibilidade = DateTime.Now;

            CategoriaProduto.Salvar(categoria);

            if (disponivel)
                ctr.BackColor = Color.DarkGreen;
            else
                ctr.BackColor = Color.LightGreen;
        }

        private void btnProduto_Click(object sender, EventArgs e)
        {
            Control ctr = (Control)sender;
            Int32 idProduto = Convert.ToInt32(ctr.Name.Substring(11));
            ProdutoInformation produto = Produto.Carregar(idProduto);
            Boolean disponibilidade = (produto.Disponibilidade == null ? true : !produto.Disponibilidade.Value);

            Produto.SalvarDisponibilidade(idProduto, disponibilidade);

            if (disponibilidade == true)
                ctr.BackColor = Color.DarkGreen;
            else
                ctr.BackColor = Color.LightGreen;
        }

        private void tcProdutoCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcProdutoCategoria.SelectedTab == tbProdutos)
            {
                cbbCategoria_SelectedIndexChanged(sender, e);
            }
            else
            {
                ListarCategorias();
            }
        }

        private class ProdutoDisponibilidade
        {

            public ProdutoDisponibilidade(ProdutoInformation produto, bool categoriaInativa = false)
            {
                IDProduto = produto.IDProduto.Value;
                Nome = produto.Nome;
                Disponibilidade = produto.Disponibilidade == true;
                Ativo = produto.Ativo == true;
                Excluido = produto.Excluido == true;
                CategoriaInativa = categoriaInativa;
            }

            public int IDProduto { get; set; }
            public string Nome { get; set; }
            public bool Ativo { get; set; }
            public bool Disponibilidade { get; set; }
            public bool Excluido { get; set; }
            public bool CategoriaInativa { get; set; }
        }
    }
}
