using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmPDV : Form
    {
        List<PDVInformation> Lista;

        public frmPDV()
        {
            InitializeComponent();
        }

        private void frmPDV_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarLista();
            AtualizarGrid();
        }

        private void CarregarLista()
        {
            Lista = BLL.PDV.Listar();
        }

        private void AtualizarGrid()
        {
            var lista = from l in Lista
                        where l.Ativo == true
                        orderby l.TipoPDV.IDTipoPDV, l.Nome
                        select new { l.IDPDV, l.Nome, TipoPDV = l.TipoPDV.Tipo.ToString(), l.UltimaAlteracao, l.UltimoAcesso, l.ChaveHardware };

            dgvPrincipal.DataSource = lista.ToArray();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                DialogResult r = MessageBox.Show("Tem certeza que deseja substitur esse PDV? A substituição só pode ser realizada 1x a cada 48 horas.", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (r == DialogResult.Yes)
                {
                    try
                    {
                        Int32 idPDV = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDPDV"].Value);
                        BLL.PDV.Remover(idPDV);

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

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                Int32 idPDV = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDPDV"].Value);

                using (var frm = new frmRenomearPDV(Lista.First(p => p.IDPDV == idPDV)))
                    frm.ShowDialog();

                CarregarLista();
                AtualizarGrid();
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja alterar!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRenomear_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void dgvPrincipal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }
    }
}
