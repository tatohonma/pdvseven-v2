using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using a7D.PDV.Caixa.UI.Properties;
using a7D.PDV.Componentes;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmDetalhesPedidoProduto : FormTouch
    {
        frmPrincipal FormPrincipal;
        String GUIDIdentificacao_selecionado;
        private PedidoInformation Pedido1 { get; set; }
        private Image ImgDesconto { get; } = Resources.desconto;
        private Bitmap Vazio { get; } = Resources.semImagem;

        public frmDetalhesPedidoProduto(frmPrincipal formPrincipal, String guidIdentificacao_selecionado)
        {
            FormPrincipal = formPrincipal;
            GUIDIdentificacao_selecionado = guidIdentificacao_selecionado;

            InitializeComponent();
        }

        private void frmDetalhesPedidoProduto_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarLista();
        }

        private void CarregarLista()
        {
            Pedido1 = Pedido.CarregarUltimoPedido(GUIDIdentificacao_selecionado);

            #region Carregar lista Produtos
            dgvItens.Rows.Clear();
            Object[] row;

            Font f = dgvItens.DefaultCellStyle.Font;

            foreach (var item in Pedido1.ListaProduto
                .Where(l => l.Produto.TipoProduto.IDTipoProduto == 10 
                         || l.Produto.TipoProduto.IDTipoProduto == 30
                         || l.Produto.TipoProduto.IDTipoProduto == 50)
                         .ToList())
            {
                row = new Object[]
                    {
                        null,
                        item.Produto.Nome + (string.IsNullOrEmpty(item.Notas) ? "": (System.Environment.NewLine + "("+item.Notas+")")),
                        item.Quantidade,
                        null,
                        item.ValorUnitarioString,
                        item.ValorTotalString,
                        item.IDPedidoProduto,
                        item.PDV.Nome,
                        item.Usuario.Nome,
                        item.DtInclusao.Value.ToString("dd/MM HH:mm")
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
                            "- " + modificacao.Produto.Nome + (string.IsNullOrEmpty(modificacao.Notas) ? "": (System.Environment.NewLine + "  ("+modificacao.Notas+")")),
                            modificacao.Quantidade.Value,
                            null,
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
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    var idPedidoProduto = Convert.ToInt32(dgvItens.SelectedRows[0].Cells["IDPedidoProduto"].Value);
                    var pedidoProduto = PedidoProduto.Carregar(idPedidoProduto);

                    if (pedidoProduto.PedidoProdutoPai != null)
                        return;

                    else if (pedidoProduto.Produto.TipoProduto.IDTipoProduto == 30)
                    {
                        MessageBox.Show("Não é possível alterar esse tipo de produto!");
                        return;
                    }

                    if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) != DialogResult.OK)
                        return;

                    using (var formCancelamento = new frmPedidoProdutoCancelamento(idPedidoProduto, usuario.IDUsuario.Value))
                    {
                        formCancelamento.ShowDialog();
                    }

                    CarregarLista();
                }
            }
        }

        private void dgvItens_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 3)
            {
                dgvItens.SuspendLayout();
                var row = dgvItens.Rows[e.RowIndex];
                FormatarCelula(row);
                dgvItens.ResumeLayout();
            }
        }

        private void FormatarCelula(DataGridViewRow row)
        {
            var idPedidoProduto = Convert.ToInt32(row.Cells[nameof(IDPedidoProduto)].Value);
            var pedidoProduto = Pedido1.ListaProduto.FirstOrDefault(p => p.IDPedidoProduto == idPedidoProduto);
            if (pedidoProduto == null)
                pedidoProduto = Pedido1.ListaProduto.Where(pp => pp.ListaModificacao != null).SelectMany(pp => pp.ListaModificacao).FirstOrDefault(p => p.IDPedidoProduto == idPedidoProduto);
            var cell3 = row.Cells[3] as DataGridViewImageCell;
            if (pedidoProduto?.ValorDesconto > 0)
            {
                cell3.Value = ImgDesconto;
            }
            else
            {
                cell3.Value = Vazio;
            }

            var cell0 = row.Cells[0] as DataGridViewImageCell;
            if (pedidoProduto?.PedidoProdutoPai != null)
            {
                cell0.Value = Vazio;
            }
        }

        private void dgvItens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 4)
                {
                    var idPedidoProduto = Convert.ToInt32(dgvItens.SelectedRows[0].Cells["IDPedidoProduto"].Value);
                    var pedidoProduto = PedidoProduto.Carregar(idPedidoProduto);
                    if (pedidoProduto.Produto.TipoProduto.IDTipoProduto == 30)
                    {
                        MessageBox.Show("Não é possível alterar esse tipo de produto!");
                        return;
                    }
                    else if (pedidoProduto.ValorUnitario == 0)
                    {
                        MessageBox.Show("Não é possível dar desconto para produtos sem valor!");
                        return;
                    }
                    else if (pedidoProduto.Produto.ValorUnitario == 0)
                    {
                        MessageBox.Show("Não é possível dar desconto para produtos sem preço!");
                        return;
                    }

                    UsuarioInformation usuario;
                    if (ConfiguracoesSistema.Valores.SolicitarSenhaDesconto)
                    {
                        if (NormalOuTouch.Autenticacao(false, true, false, false, out usuario) != DialogResult.OK)
                            return;
                    }
                    else
                        usuario = AC.Usuario;

                    using (var frmDesconto = new frmPedidoProdutoDesconto(pedidoProduto, usuario))
                    {
                        frmDesconto.ShowDialog();
                    }
                    CarregarLista();
                }
            }
        }

        private void dgvItens_MouseHover(object sender, EventArgs e)
        {

        }

        private void dgvItens_MouseMove(object sender, MouseEventArgs e)
        {
            var hti = dgvItens.HitTest(e.X, e.Y);
            if (hti.ColumnIndex == 4)
            {
                dgvItens.Cursor = Cursors.Hand;
            }
            else
            {
                dgvItens.Cursor = Cursors.Default;
            }
        }
    }
}
