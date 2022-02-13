using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.Model;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmTipoTributacao : Form
    {
        public List<TipoTributacaoInformation> ListaTipoTributacao { get; private set; }
        public List<TipoTributacaoInformation> ListaTipoTributacaoFiltrada { get; private set; }

        public frmTipoTributacao()
        {
            InitializeComponent();
        }

        private void frmTipoTributacao_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarListas();
            AtualizarGrid();
            txtNomeFiltro.Text = string.Empty;
            txtCFOP.Text = string.Empty;
        }

        private void AtualizarGrid()
        {
            var lista = from l in ListaTipoTributacaoFiltrada
                        orderby l.Nome
                        select new { l.IDTipoTributacao, l.Nome, l.CFOP };

            dgvPrincipal.DataSource = lista.ToArray();
        }

        private void CarregarListas()
        {
            ListaTipoTributacao = TipoTributacao.Listar();
            ListaTipoTributacaoFiltrada = ListaTipoTributacao;
        }

        private void FiltrarGrid()
        {
            ListaTipoTributacaoFiltrada = ListaTipoTributacao;

            if (string.IsNullOrWhiteSpace(txtNomeFiltro.Text) == false)
            {
                ListaTipoTributacaoFiltrada = ListaTipoTributacaoFiltrada.Where(cf => cf.Nome.ToLowerInvariant().Contains(txtNomeFiltro.Text.ToLowerInvariant())).ToList();
            }

            if (string.IsNullOrEmpty(txtCFOP.Text) == false)
            {
                ListaTipoTributacaoFiltrada = ListaTipoTributacaoFiltrada.Where(cf => cf.CFOP.ToLowerInvariant().Contains(txtCFOP.Text.ToLowerInvariant())).ToList();
            }
            AtualizarGrid();
        }

        private void txtNomeFiltro_KeyUp(object sender, KeyEventArgs e)
        {
            FiltrarGrid();
        }

        private void txtCFOP_KeyUp(object sender, KeyEventArgs e)
        {
            FiltrarGrid();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                int idTipoTributacao = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDTipoTributacao"].Value);
                var tipoTributacao = TipoTributacao.Carregar(idTipoTributacao);

                var resp = MessageBox.Show(string.Format("Deseja realmente excluir Tipo de Tributação {0}?", tipoTributacao.Nome), "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resp == DialogResult.Yes)
                {
                    TipoTributacao.Excluir(idTipoTributacao);
                    CarregarListas();
                    FiltrarGrid();
                }
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja excluir!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                int idTipoTributacao = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDTipoTributacao"].Value);
                var fTipoTributacaoEditar = new frmTipoTributacaoEditar(idTipoTributacao);
                fTipoTributacaoEditar.ShowDialog();

                CarregarListas();
                FiltrarGrid();
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja alterar!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvPrincipal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }

        private void btInserir_Click(object sender, EventArgs e)
        {
            var fTipoTributacaoEditar = new frmTipoTributacaoEditar();
            fTipoTributacaoEditar.ShowDialog();

            CarregarListas();
            FiltrarGrid();
        }
    }
}
