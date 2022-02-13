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
using System.Transactions;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmMovimentacao : Form
    {
        private List<MovimentacaoInformation> Movimentacoes { get; set; }
        private string Pesquisa { get; set; }

        public frmMovimentacao()
        {
            Pesquisa = string.Empty;
            InitializeComponent();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (Produto.ExistemProdutosComEstoqueControlado())
            {
                new frmMovimentacaoSelecionarTipo().ShowDialog();
                Filtrar();
            } else
            {
                MessageBox.Show("Marque produtos com \"Controlar Estoque\" no cadastro de produto antes de utilizar essa funcionalidade.", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void frmMovimentacao_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            Filtrar();
        }

        private void Filtrar()
        {
            Movimentacoes = Movimentacao.Listar()
                .Where(m => m.Excluido == false)
                .Where(m => m.Reversa == false)
                .ToList();

            var movFiltradas = Movimentacoes;
            if (string.IsNullOrWhiteSpace(Pesquisa) == false)
                movFiltradas = movFiltradas
                    .Where(m => m.NumeroPedido.ToLower().Contains(Pesquisa)
                        || m.Descricao.ToLower().Contains(Pesquisa)
                        || m.DataMovimentacao.Value.ToShortDateString().Contains(Pesquisa))
                    .ToList();

            var lista = movFiltradas
                .Select(m => new
                {
                    m.IDMovimentacao,
                    m.Processado,
                    TipoMovimentacao = m.TipoMovimentacao.Nome,
                    m.NumeroPedido,
                    m.Descricao,
                    m.DataMovimentacao
                })
                .ToArray();

            dgvPrincipal.DataSource = lista;
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            Pesquisa = txtPesquisa.Text;
            Pesquisa = Pesquisa.ToLowerInvariant();
            Filtrar();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                var row = dgvPrincipal.SelectedRows[0];

                var idMovimentacao = row.Cells["IDMovimentacao"].Value as int?;
                new frmMovimentacaoEditar(Movimentacao.Carregar(idMovimentacao.Value)).ShowDialog();
                Filtrar();
            }
        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count < 1)
                MessageBox.Show("Selecione o registro que deseja processar", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                var row = dgvPrincipal.SelectedRows[0];
                var resp = MessageBox.Show("Deseja processar o registro selecionado? Esta ação não pode ser desfeita!", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            var idMovimentacao = row.Cells["IDMovimentacao"].Value as int?;
                            var movimentacao = Movimentacao.Carregar(idMovimentacao.Value);
                            var movimentado = EntradaSaida.Movimentar(movimentacao);
                            movimentacao.Processado = true;
                            Movimentacao.Salvar(movimentacao);
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Logs.ErroBox( CodigoErro.E013, ex);
                        }
                    }
                    Filtrar();
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count < 1)
                MessageBox.Show("Selecione o registro que deseja excluir", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                var row = dgvPrincipal.SelectedRows[0];
                var idMovimentacao = row.Cells["IDMovimentacao"].Value as int?;
                var movimentacao = Movimentacao.Carregar(idMovimentacao.Value);

                var resp = MessageBox.Show("Deseja excluir a movimentação selecionada?\nSe a movimentação estiver processada esta ação irá atlerar seu estoque atual.", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    Movimentacao.Excluir(movimentacao);
                    Filtrar();
                }

            }
        }
    }
}
