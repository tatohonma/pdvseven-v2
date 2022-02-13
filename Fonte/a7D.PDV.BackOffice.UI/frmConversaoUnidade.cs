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
    public partial class frmConversaoUnidade : Form
    {
        private string pesquisa;
        private List<ConversaoUnidadeInformation> listaConversoes;

        public frmConversaoUnidade()
        {
            pesquisa = string.Empty;
            InitializeComponent();
        }

        private void frmConversaoUnidade_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarConversoes();
            Filtrar();
        }

        private void Filtrar()
        {
            var lista = from c in listaConversoes
                        select new { c.IDConversaoUnidade, Descricao = string.Format("Um(a) {0} é igual à {1} {2}(s).", c.Unidade_de.Nome, c.Divisao.Value.ToString("#,##0.00#####"), c.Unidade_para.Nome) };

            if (string.IsNullOrWhiteSpace(pesquisa))
                lista = lista.Where(c => c.Descricao.ToLowerInvariant().Contains(pesquisa));

            dgvPrincipal.DataSource = lista.ToArray();
        }

        private void CarregarConversoes()
        {
            listaConversoes = ConversaoUnidade.Listar();
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            pesquisa = txtPesquisa.Text;
            Filtrar();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            new frmConversaoUnidadeEditar().ShowDialog();
            CarregarConversoes();
            Filtrar();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count < 1)
                MessageBox.Show("Selecione o registro que você deseja alterar", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                new frmConversaoUnidadeEditar(Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDConversaoUnidade"].Value)).ShowDialog();
                CarregarConversoes();
                Filtrar();
            }
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
            if (dgvPrincipal.SelectedRows.Count < 1)
                MessageBox.Show("Selecione o registro que você deseja excluir", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                var resp = MessageBox.Show("Deseja excluir o registro selecionado?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    ConversaoUnidade.Excluir(Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDConversaoUnidade"].Value));
                    CarregarConversoes();
                    Filtrar();
                }
            }
        }
    }
}
