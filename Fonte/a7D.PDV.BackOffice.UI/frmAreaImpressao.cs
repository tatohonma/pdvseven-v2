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
    public partial class frmAreaImpressao : Form
    {
        List<AreaImpressaoInformation> Lista;

        public frmAreaImpressao()
        {
            InitializeComponent();
        }

        private void frmAreaImpressao_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarLista();
            AtualizarGrid();
        }

        private void CarregarLista()
        {
            Lista = AreaImpressao.Listar();
        }

        private void AtualizarGrid()
        {
            var lista = from l in Lista
                        orderby l.TipoAreaImpressao.IDTipoAreaImpressao, l.Nome
                        select new { l.IDAreaImpressao, l.Nome, Tipo = l.TipoAreaImpressao.Nome, l.NomeImpressora };

            dgvPrincipal.DataSource = lista.ToArray();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            frmAreaImpressaoEditar frm = new frmAreaImpressaoEditar();
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
                        Int32 idAreaImpressao = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDAreaImpressao"].Value);
                        AreaImpressao.Excluir(idAreaImpressao);

                        CarregarLista();
                        AtualizarGrid();
                    }
                    catch (Exception _e)
                    {
                        MessageBox.Show(_e.Message, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                Int32 idAreaImpressao = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDAreaImpressao"].Value);

                frmAreaImpressaoEditar frm = new frmAreaImpressaoEditar(idAreaImpressao);
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
