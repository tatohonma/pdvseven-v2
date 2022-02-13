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

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmCategoria : Form
    {
        List<CategoriaProdutoInformation> Lista;

        public frmCategoria()
        {
            InitializeComponent();
        }

        private void frmCategoria_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            Carregar();
        }

        private void Carregar()
        {
            CarregarLista();
            AtualizarGrid();
        }

        private void CarregarLista()
        {
            Lista = CategoriaProduto.Listar();
        }

        private void AtualizarGrid()
        {
            if (Lista != null)
            {
                var lista = from l in Lista
                            where l.Nome.ToLower().Contains(txtPesquisa.Text.ToLower())
                            orderby l.Nome
                            select new { l.IDCategoriaProduto, l.CodigoERP, l.Nome };

                dgvPrincipal.DataSource = lista.ToArray();
                dgvPrincipal.ClearSelection();
            }
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            frmCategoriaEditar frm = new frmCategoriaEditar();
            frm.ShowDialog();

            CarregarLista();
            AtualizarGrid();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                DialogResult r = MessageBox.Show("Tem certeza que deseja excluir esse item?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (r == DialogResult.Yes)
                {
                    try
                    {
                        Int32 idCategoriaProduto = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDCategoriaProduto"].Value);
                        CategoriaProduto.Excluir(idCategoriaProduto);

                        CarregarLista();
                        AtualizarGrid();
                    }
                    catch (Exception ex)
                    {
                        Logs.ErroBox(CodigoErro.E013, ex);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja excluir!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvPrincipal_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                Int32 idCategoriaProduto = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDCategoriaProduto"].Value);

                frmCategoriaEditar frm = new frmCategoriaEditar(idCategoriaProduto);
                frm.ShowDialog();

                CarregarLista();
                AtualizarGrid();
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja alterar!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmCategoria_VisibleChanged(object sender, EventArgs e)
        {
            Carregar();
        }
    }
}
