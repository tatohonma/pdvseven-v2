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
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmTaxaEntrega : Form
    {
        private List<TaxaEntregaInformation> Lista { get; set; }

        public frmTaxaEntrega()
        {
            InitializeComponent();
        }

        private void frmTaxaEntrega_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            AtualizarLista();
        }

        private void AtualizarLista()
        {
            Lista = TaxaEntrega.Listar();
            dgvPrincipal.DataSource = Lista.Select(t => new { t.IDTaxaEntrega, t.Nome, t.Valor, Ativo = (t.Ativo == true ? "SIM" : "NÃO") }).ToArray();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            new frmTaxaEntregaEditar().ShowDialog();
            AtualizarLista();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            var idTaxaEntrega = VerificarSelecionado();
            if (idTaxaEntrega.HasValue)
            {
                new frmTaxaEntregaEditar(idTaxaEntrega.Value).ShowDialog();
                AtualizarLista();
            }
        }

        public int? VerificarSelecionado(bool excluir = false)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
                return Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["colID"].Value);
            MessageBox.Show($"Selecione o registro que deseja { (excluir ? "excluir" : "alterar") }!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            var idTaxaEntrega = VerificarSelecionado(true);
            if (idTaxaEntrega.HasValue)
            {
                if (MessageBox.Show("Deseja realmente excluir o registro selecionado?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TaxaEntrega.Excluir(idTaxaEntrega.Value);
                    AtualizarLista();
                }
            }
        }

        private void dgvPrincipal_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnAlterar_Click(null, null);
        }
    }
}
