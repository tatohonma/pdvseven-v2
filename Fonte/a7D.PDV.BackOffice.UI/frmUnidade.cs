using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using a7D.PDV.Model;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmUnidade : Form
    {
        private string pesquisa;
        private List<UnidadeInformation> listaUnidades;

        public frmUnidade()
        {
            InitializeComponent();
        }

        private void frmUnidade_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            pesquisa = string.Empty;
            CarregarUnidades();
            Filtrar();
        }

        private void Filtrar()
        {
            var lista = from u in listaUnidades
                        select new { u.IDUnidade, u.Nome, u.Simbolo };
            if (string.IsNullOrWhiteSpace(pesquisa) == false)
                lista = lista.Where(u => u.Nome.ToLowerInvariant().Contains(pesquisa.ToLowerInvariant()));

            dgvPrincipal.DataSource = lista.ToArray();
        }

        private void CarregarUnidades()
        {
            listaUnidades = Unidade.Listar().Where(u => u.Excluido == false).ToList();
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            pesquisa = txtPesquisa.Text;
            Filtrar();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count < 1)
                MessageBox.Show("Selecione o registro que deseja alterar", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                new frmUnidadeEditar(Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDUnidade"].Value)).ShowDialog();
                CarregarUnidades();
                Filtrar();
            }
        }

        private void dgvPrincipal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }

        private void dgvPrincipal_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            new frmUnidadeEditar().ShowDialog();
            CarregarUnidades();
            Filtrar();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count < 1)
                MessageBox.Show("Selecione o registro que deseja excluir", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                var resp = MessageBox.Show("Deseja excluir o registro selecionado?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    Unidade.Excluir(Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDUnidade"].Value));
                    CarregarUnidades();
                    Filtrar();
                }
            }
        }
    }
}
