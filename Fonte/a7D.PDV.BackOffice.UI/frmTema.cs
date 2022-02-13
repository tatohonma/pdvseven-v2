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
    public partial class frmTema : Form
    {
        List<TemaCardapioInformation> Lista;

        public frmTema()
        {
            InitializeComponent();
        }

        private void frmTema_Load(object sender, EventArgs e)
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
            Lista = TemaCardapio.Listar();
        }

        private void AtualizarGrid()
        {
            if (Lista != null)
            {
                var lista = from l in Lista
                            where l.Nome.ToLower().Contains(txtPesquisa.Text.ToLower())
                            orderby l.Nome
                            select new { l.IDTemaCardapio, l.Nome, Imagem = "Ir para imagens..." };

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
            frmTemaEditar frm = new frmTemaEditar();
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
                        Int32 idTemaCardapio = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDTemaCardapio"].Value);
                        TemaCardapio.Excluir(idTemaCardapio);

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
            Int32 idTemaCardapio;

            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                idTemaCardapio = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["idTemaCardapio"].Value);

                frmTemaEditar frm = new frmTemaEditar(idTemaCardapio);
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

        private void dgvPrincipal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                var frmPrincipal = ParentForm as frmPrincipal;
                if (frmPrincipal != null)
                {
                    int idTemaCardapio = Convert.ToInt32(dgvPrincipal.Rows[e.RowIndex].Cells[0].Value);
                    frmPrincipal.FecharJanelas();

                    var frmImagemTema = new frmImagemTema(idTemaCardapio);
                    frmImagemTema.MdiParent = frmPrincipal;
                    frmImagemTema.Dock = DockStyle.Fill;

                    frmImagemTema.Show();

                }
            }
        }
    }
}
