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
    public partial class frmMotivoCancelamento : Form
    {

        private List<MotivoCancelamentoInformation> MotivosCancelamento { get; set; }

        public frmMotivoCancelamento()
        {
            InitializeComponent();
        }

        private void frmMotivoCancelamento_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            try
            {
                ListarMotivos();
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E013, ex);
            }
        }

        private void ListarMotivos()
        {
            MotivosCancelamento = MotivoCancelamento.Listar();
            dgvPrincipal.DataSource = MotivosCancelamento.Select(m => new { m.IDMotivoCancelamento, m.Nome }).ToArray();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count < 1)
            {
                MessageBox.Show("Selecione o registro para alterar", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    if (MessageBox.Show("Deseja excluir o registro selecionado?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var id = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells[nameof(IDMotivoCancelamento)].Value);
                        MotivoCancelamento.Excluir(id);
                        ListarMotivos();
                    }
                }
                catch (Exception ex)
                {
                    Logs.ErroBox(CodigoErro.E013, ex);
                }
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            using (var frm = new frmMotivoCancelamentoEditar())
            {
                frm.ShowDialog();
            }
            ListarMotivos();
        }

        private void dgvPrincipal_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }

        private void dgvPrincipal_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Alterar();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count < 1)
            {
                MessageBox.Show("Selecione o registro para alterar", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var id = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells[nameof(IDMotivoCancelamento)].Value);
                using (var frm = new frmMotivoCancelamentoEditar(id))
                {
                    frm.ShowDialog();
                }
                ListarMotivos();
            }
        }
    }
}
