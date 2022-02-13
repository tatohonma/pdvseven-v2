using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmUsuario : Form
    {
        List<UsuarioInformation> Lista;

        public frmUsuario()
        {
            InitializeComponent();
        }

        private void frmUsuario_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarLista();
            AtualizarGrid();
        }

        private void CarregarLista()
        {
            Lista = Usuario.Listar();
        }

        private void AtualizarGrid()
        {
            if (Lista != null)
            {
                var lista = from l in Lista
                            where l.IDUsuario>1 && l.Nome.ToLower().Contains(txtPesquisa.Text.ToLower())
                            orderby l.Nome
                            select new { l.IDUsuario, Nome = l.Nome, l.PermissaoAdm, l.PermissaoGerente, l.PermissaoCaixa, l.PermissaoGarcom, l.Ativo };

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
            frmUsuarioEditar frm = new frmUsuarioEditar();
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
                        Int32 idUsuario = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDUsuario"].Value);
                        Usuario.Excluir(idUsuario);

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
                Int32 idUsuario = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDUsuario"].Value);

                frmUsuarioEditar frm = new frmUsuarioEditar(idUsuario);
                frm.ShowDialog();

                CarregarLista();
                AtualizarGrid();
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja alterar!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
