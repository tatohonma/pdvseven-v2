using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using System.Globalization;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmPainelModificacaoEditar : Form
    {
        private static readonly IFormatProvider _format = new CultureInfo("pt-BR");
        private PainelModificacaoInformation PainelModificacao1;

        public frmPainelModificacaoEditar()
        {
            InitializeComponent();

            PainelModificacao1 = new PainelModificacaoInformation();
            PainelModificacao1.ListaProduto = new List<PainelModificacaoProdutoInformation>();
        }

        public frmPainelModificacaoEditar(Int32 idPainelModificacao)
        {
            InitializeComponent();

            PainelModificacao1 = PainelModificacao.CarregarCompleto(idPainelModificacao);
        }

        private void CarregarListas()
        {
            List<ProdutoInformation> listaProduto = Produto.ListarApenasModificacao().OrderBy(l => l.Nome).ToList();
            listaProduto.Insert(0, new ProdutoInformation());

            cbbProduto.DataSource = listaProduto.Select(p => new ProdutoModificacaoDisplay(p)).ToList();

            cbbOperacao.DataSource = PainelModificacaoOperacao.Listar();
        }

        private void frmPainelModificacaoEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarListas();

            if (PainelModificacao1.IDPainelModificacao != null)
            {

                txtNome.Text = PainelModificacao1.Nome;
                txtTitulo.Text = PainelModificacao1.Titulo;
                txtMinimo.Text = PainelModificacao1.Minimo.ToString();
                txtMaximo.Text = PainelModificacao1.Maximo.ToString();
                cbbOperacao.SelectedItem = PainelModificacao1.PainelModificacaoOperacao;

                //Paineis Modificação
                AtualizarListaProduto();
            }
        }

        private void AtualizarListaProduto()
        {
            var lista = from l in PainelModificacao1.ListaProduto
                        orderby l.Ordem
                        select new { l.IDPainelModificacaoProduto, l.Produto.Nome };

            dgvProduto.SuspendLayout();
            dgvProduto.DataSource = lista.ToArray();
            dgvProduto.ResumeLayout();
            dgvProduto.ClearSelection();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar() == true)
            {
                PainelModificacao1.Nome = txtNome.Text;
                PainelModificacao1.Titulo = txtTitulo.Text;

                if (string.IsNullOrWhiteSpace(txtMinimo.Text))
                    PainelModificacao1.Minimo = null;
                else
                    PainelModificacao1.Minimo = Convert.ToInt32(txtMinimo.Text);

                if (string.IsNullOrWhiteSpace(txtMaximo.Text))
                    PainelModificacao1.Maximo = null;
                else
                    PainelModificacao1.Maximo = Convert.ToInt32(txtMaximo.Text);

                PainelModificacao1.PainelModificacaoOperacao = new PainelModificacaoOperacaoInformation { IDPainelModificacaoOperacao = 1 };
                //PainelModificacao1.PainelModificacaoOperacao = cbbOperacao.SelectedItem as PainelModificacaoOperacaoInformation;

                PainelModificacao.Salvar(PainelModificacao1);

                this.Close();
            }
        }

        private Boolean Validar()
        {
            String msg = "";
            decimal min, max;
            bool minParse, maxParse;

            if (txtNome.Text == "")
                msg += "Campo \"Nome\" é obrigatório. \n";

            minParse = decimal.TryParse(txtMinimo.Text, out min);
            if (txtMinimo.Text != "" && minParse == false)
                msg += "Campo \"Mínimo\" deve ser numérico. \n";

            maxParse = decimal.TryParse(txtMaximo.Text, out max);
            if (txtMaximo.Text != "" && maxParse == false)
                msg += "Campo \"Máximo\" deve ser numérico. \n";

            if (minParse && !maxParse)
                msg += "Se há um valor mínimo precisa especificar um valor máximo\n";
            else if (!minParse && maxParse)
                msg += "Se há um valor máximo precisa especificar um valor mínimo\n";
            else if (minParse && maxParse)
            {
                if (min > max)
                    msg += "Mínimo não pode ser maior que o máximo\n";
                if (max < min)
                    msg += "Máximo não pode ser menor que o mínimo\n";
            }

            if (msg.Length > 0)
            {
                MessageBox.Show(msg, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvProduto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 idPainelModificacaoProduto;
            PainelModificacaoProdutoInformation painelModificacaoProduto;
            Int32 i = 0;

            if (e.RowIndex >= 0)
            {
                switch (dgvProduto.Columns[e.ColumnIndex].Name)
                {
                    case "Subir":
                        idPainelModificacaoProduto = Convert.ToInt32(dgvProduto["IDPainelModificacaoProduto", e.RowIndex].Value);
                        painelModificacaoProduto = PainelModificacao1.ListaProduto.First(l => l.IDPainelModificacaoProduto == idPainelModificacaoProduto);
                        painelModificacaoProduto.Ordem = painelModificacaoProduto.Ordem.Value - 3;

                        foreach (var item in PainelModificacao1.ListaProduto.OrderBy(l => l.Ordem).ToList())
                        {
                            item.Ordem = i;
                            i = i + 2;
                        }

                        AtualizarListaProduto();

                        break;

                    case "Descer":
                        idPainelModificacaoProduto = Convert.ToInt32(dgvProduto["IDPainelModificacaoProduto", e.RowIndex].Value);
                        painelModificacaoProduto = PainelModificacao1.ListaProduto.First(l => l.IDPainelModificacaoProduto == idPainelModificacaoProduto);
                        painelModificacaoProduto.Ordem = painelModificacaoProduto.Ordem.Value + 3;

                        foreach (var item in PainelModificacao1.ListaProduto.OrderBy(l => l.Ordem).ToList())
                        {
                            item.Ordem = i;
                            i = i + 2;
                        }

                        AtualizarListaProduto();

                        break;

                    case "Remover":
                        idPainelModificacaoProduto = Convert.ToInt32(dgvProduto["IDPainelModificacaoProduto", e.RowIndex].Value);
                        painelModificacaoProduto = PainelModificacao1.ListaProduto.First(l => l.IDPainelModificacaoProduto == idPainelModificacaoProduto);
                        PainelModificacao1.ListaProduto.Remove(painelModificacaoProduto);

                        AtualizarListaProduto();

                        break;
                }
            }
        }

        private void btnAdicionarProduto_Click(object sender, EventArgs e)
        {
            if (cbbProduto.SelectedIndex > 0)
            {
                Int32 idPainelModificacaoProduto = -1;
                Int32 ordem = 0;

                if (PainelModificacao1.ListaProduto.Count > 0)
                {
                    idPainelModificacaoProduto = PainelModificacao1.ListaProduto.Min(l => l.IDPainelModificacaoProduto).Value - 1;
                    ordem = PainelModificacao1.ListaProduto.Max(l => l.Ordem).Value + 2;
                }

                Int32 idProduto = Convert.ToInt32(cbbProduto.SelectedValue);
                ProdutoInformation produto = Produto.Carregar(idProduto);

                PainelModificacaoProdutoInformation painelModificacaoProduto = new PainelModificacaoProdutoInformation();
                painelModificacaoProduto.IDPainelModificacaoProduto = idPainelModificacaoProduto;
                painelModificacaoProduto.Ordem = ordem;
                painelModificacaoProduto.Produto = produto;

                PainelModificacao1.ListaProduto.Add(painelModificacaoProduto);
                AtualizarListaProduto();
            }
        }

        private partial class ProdutoModificacaoDisplay
        {

            public ProdutoModificacaoDisplay(ProdutoInformation produto)
            {
                IDProduto = produto.IDProduto ?? 0;
                Nome = produto.Nome;
                Preco = produto.ValorUnitario ?? 0m;
            }

            public ProdutoModificacaoDisplay()
            {

            }

            public int IDProduto { get; set; }
            private string Nome { get; set; }
            private decimal Preco { get; set; }
            public string Display
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(Nome))
                        return string.Empty;
                    return $"{Nome} ({Preco.ToString("R$ #,##0.00", _format)})";
                }
            }
        }
    }
}
