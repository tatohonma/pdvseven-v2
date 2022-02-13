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
    public partial class frmTipoMovimentacao : Form
    {
        private string pesquisa;
        private string tipo;
        private List<TipoMovimentacaoInformation> listaTiposMovimentacao;

        public frmTipoMovimentacao()
        {
            pesquisa = string.Empty;
            tipo = "";
            InitializeComponent();
        }

        private void frmTipoMovimentacao_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            cbTipo.Items.Add(new { Nome = "Todos" });
            cbTipo.Items.Add(new { Nome = "Entrada" });
            cbTipo.Items.Add(new { Nome = "Saída" });

            cbTipo.DisplayMember = "Nome";
            cbTipo.SelectedIndexChanged -= cbTipo_SelectedIndexChanged;
            cbTipo.SelectedIndex = 0;
            cbTipo.SelectedIndexChanged += cbTipo_SelectedIndexChanged;

            CarregarTiposDeMovimentacao();
            Filtrar();
        }

        private void Filtrar()
        {
            var lista = from tm in listaTiposMovimentacao
                        select new { tm.IDTipoMovimentacao, Tipo = tm.Tipo == "+" ? "Entrada" : "Saída", tm.Nome };

            if (string.IsNullOrWhiteSpace(pesquisa) == false)
                lista = lista.Where(tm => tm.Nome.ToLowerInvariant().Contains(pesquisa.ToLowerInvariant()));

            if (string.IsNullOrWhiteSpace(tipo) == false)
                lista = lista.Where(tm => tm.Tipo == tipo);

            dgvPrincipal.DataSource = lista.ToArray();
        }

        private void CarregarTiposDeMovimentacao()
        {
            listaTiposMovimentacao = TipoMovimentacao.Listar().Where(tm => !tm.Excluido).ToList();
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
                MessageBox.Show("Selecione o registro que deseja excluir", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                new frmTipoMovimentacaoEditar(Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDTipoMovimentacao"].Value)).ShowDialog();
                CarregarTiposDeMovimentacao();
                Filtrar();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count < 1)
                MessageBox.Show("Selecione o registro que deseja excluir", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                var resp = MessageBox.Show("Deseja realmente excluir o registro selecionado?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    TipoMovimentacao.Excluir(Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDTipoMovimentacao"].Value));
                    CarregarTiposDeMovimentacao();
                    Filtrar();
                }
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            new frmTipoMovimentacaoEditar().ShowDialog();
            CarregarTiposDeMovimentacao();
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

        private void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbTipo.SelectedIndex)
            {
                case 1:
                    tipo = "Entrada";
                    break;
                case 2:
                    tipo = "Saída";
                    break;
                default:
                    tipo = string.Empty;
                    break;
            }
            Filtrar();
        }
    }
}
