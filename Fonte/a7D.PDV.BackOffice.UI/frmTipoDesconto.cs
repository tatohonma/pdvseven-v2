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
    public partial class frmTipoDesconto : Form
    {
        private List<TipoDescontoInformation> descontos;

        public frmTipoDesconto()
        {
            InitializeComponent();
        }

        private void frmTipoDesconto_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarGrid();
        }

        private void CarregarGrid()
        {
            descontos = TipoDesconto.Listar();

            var lista = from l in descontos
                        where l.Nome.ToLowerInvariant().Contains(txtPesquisa.Text.ToLowerInvariant())
                        orderby l.Nome
                        select new { l.IDTipoDesconto, l.Nome, l.Ativo };

            dgvPrincipal.DataSource = lista.ToArray();
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            var frm = new frmTipoDescontoEditar();
            frm.ShowDialog();
            CarregarGrid();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                var idTipoDesconto = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDTipoDesconto"].Value);
                var f = new frmTipoDescontoEditar(idTipoDesconto);
                f.ShowDialog();
                CarregarGrid();
            }
            else
                MessageBox.Show("Selecione o Tipo de Desconto que deseja alterar!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvPrincipal_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                        var idTipoDesconto = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDTipoDesconto"].Value);
                        TipoDesconto.Excluir(TipoDesconto.Carregar(idTipoDesconto));
                    }
                    catch (Exception ex)
                    {
                        Logs.ErroBox(CodigoErro.E013, ex);
                    }
                    finally
                    {
                        CarregarGrid();
                    }
                }
            }
            else
                MessageBox.Show("Selecione o Tipo de Desconto que deseja excluir!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
