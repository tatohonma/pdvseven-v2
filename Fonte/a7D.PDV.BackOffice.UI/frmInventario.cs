using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmInventario : Form
    {
        private string Filtro { get; set; }
        private List<InventarioInformation> Inventarios { get; set; }
        public frmInventario()
        {
            InitializeComponent();
        }

        private void frmInventario_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            Filtrar();
        }

        private void Filtrar()
        {
            Inventarios = Inventario.Listar()
                .Where(m => m.Excluido == false)
                .ToList();
            var filtrados = Inventarios;

            if (string.IsNullOrWhiteSpace(Filtro) == false)
                filtrados = filtrados.Where(m => m.Descricao.ToLower().Contains(Filtro)).ToList();

            var lista = filtrados.Select(i => new { i.IDInventario, i.Processado, i.Descricao, i.Data }).ToArray();

            dgvPrincipal.DataSource = lista;

        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (Produto.ExistemProdutosComEstoqueControlado())
            {
                new frmInventarioEditar().ShowDialog();
                Filtrar();
            } else
            {
                MessageBox.Show("Marque produtos com \"Controlar Estoque\" no cadastro de produto antes de utilizar essa funcionalidade.", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                Cursor = Cursors.WaitCursor;
                dgvPrincipal.Cursor = Cursor;
                var idInventario = dgvPrincipal.SelectedRows[0].Cells["IDInventario"].Value as int?;
                var inventario = Inventario.Carregar(idInventario.Value);
                if (inventario.Processado == true)
                    MessageBox.Show("Não é possível alterar a data ou as quantidades de um inventário processado.\nVocê poderá alterar somente a descrição.");
                new frmInventarioEditar(inventario).ShowDialog();
                dgvPrincipal.Cursor = Cursors.Default;
                Cursor = Cursors.Default;
                UseWaitCursor = false;

            }
            else
                MessageBox.Show("Selecione a linha que deseja alterar", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                var resp = MessageBox.Show("Deseja processar o inventário selecionado?\nEsta ação não pode ser desfeita e irá alterar seu estoque.", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resp == DialogResult.Yes)
                {
                    var row = dgvPrincipal.SelectedRows[0];
                    var idInventario = row.Cells["IDInventario"].Value as int?;
                    var inventario = Inventario.Carregar(idInventario.Value);
                    var movimentacoesApos = Movimentacao.MovimentacoesApos(inventario.Data.Value);

                    if (movimentacoesApos > 0)
                        resp = MessageBox.Show(string.Format("Existem {0} movimentações após a data do inventário selecionado ({1}).\nDeseja processar mesmo assim?", movimentacoesApos, inventario.Data.Value.ToString("dd/MM/yyyy")), "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    try
                    {
                        if (resp == DialogResult.Yes)
                        {
                            Inventario.Processar(inventario);
                            new frmRelatorioInventario(inventario.IDInventario.Value).ShowDialog();
                        }
                        Filtrar();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
                MessageBox.Show("Selecione o item que deseja processar", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            Filtro = txtPesquisa.Text.ToLower();
            Filtrar();
        }

        private void dgvPrincipal_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }

        private void dgvPrincipal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                var resp = MessageBox.Show("Deseja excluir o registro selecionado?\nEsta ação não pode ser desfeita.", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resp == DialogResult.Yes)
                {
                    try
                    {
                        var row = dgvPrincipal.SelectedRows[0];
                        var idInventario = row.Cells["IDInventario"].Value as int?;
                        Inventario.Excluir(idInventario.Value);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Filtrar();
                    }
                }
            }
            else
                MessageBox.Show("Selecione o item que deseja excluir", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                ExportarRelatorio(dgvPrincipal.SelectedRows[0].Cells["IDInventario"].Value as int?);
            }
        }

        private void ExportarRelatorio(int? idInventario)
        {
            if (idInventario.HasValue)
            {
                try
                {
                    using (var dtRelatorioInventario = Inventario.RelatorioInventario(idInventario.Value))
                    {
                        if (dtRelatorioInventario != null)
                        {
                            var nomeArquivo = BackofficeUtil.NomeRelatorio("Inventario_" + idInventario.Value + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmssmmm") + ".csv");
                            BLL.Relatorio.ExportarParaTxt(dtRelatorioInventario, nomeArquivo);

                            MessageBox.Show("Relatório gerado em " + nomeArquivo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logs.ErroBox(CodigoErro.E013, ex);
                }
            }
        }

        private void dgvPrincipal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPrincipal.Rows[e.RowIndex].Cells["Relatorio"].ColumnIndex)
            {
                new frmRelatorioInventario(Convert.ToInt32(dgvPrincipal.Rows[e.RowIndex].Cells["IDInventario"].Value)).ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new frmFichaInventario().ShowDialog();
        }
    }
}
