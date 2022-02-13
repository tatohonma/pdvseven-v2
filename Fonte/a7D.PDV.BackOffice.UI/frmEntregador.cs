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
    public partial class frmEntregador : Form
    {
        private List<EntregadorInformation> Lista { get; set; }

        public frmEntregador()
        {
            InitializeComponent();
        }

        private void AtualizarLista()
        {
            Lista = Entregador.Listar();
            dgvPrincipal.DataSource = Lista.Select(e => new { e.IDEntregador, e.Nome, e.Ativo }).ToArray();
        }

        private void frmEntregador_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            AtualizarLista();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            new frmEntregadorEditar().ShowDialog();
            AtualizarLista();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            var idEntregador = VerificarSelecionado();
            if (idEntregador.HasValue)
            {
                new frmEntregadorEditar(idEntregador.Value).ShowDialog();
                AtualizarLista();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            var idEntregador = VerificarSelecionado(true);
            if (idEntregador.HasValue)
            {
                if (MessageBox.Show("Deseja realmente excluir o registro selecionado?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Entregador.Excluir(idEntregador.Value);
                    AtualizarLista();
                }
            }
        }

        public int? VerificarSelecionado(bool excluir = false)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
                return Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["colID"].Value);
            MessageBox.Show($"Selecione o registro que deseja { (excluir ? "excluir" : "alterar") }!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }

        private void dgvPrincipal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnAlterar_Click(null, null);
        }
    }
}
