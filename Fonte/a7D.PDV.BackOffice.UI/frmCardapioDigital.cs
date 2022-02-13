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
    public partial class frmCardapioDigital : Form
    {
        List<TemaCardapioPDVInformation> Lista;

        public frmCardapioDigital()
        {
            InitializeComponent();
        }

        private void frmCardapioDigital_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            Carregar();
        }

        private void Carregar()
        {
            CarregarLista();
            AtualizarGrid();
        }

        private void CarregarLista()
        {
            Lista = TemaCardapioPDV.Listar();
        }

        private void AtualizarGrid()
        {
            if (Lista != null)
            {
                var lista = from l in Lista
                    select new { l.IDTemaCardapioPDV, pdv = (l.PDV != null ? l.PDV.Nome : "Padrão"), IDPDV = (l.PDV != null ? l.PDV.IDPDV : null), tema = l.TemaCardapio.Nome };

                dgvPrincipal.DataSource = lista.ToArray();
                dgvPrincipal.ClearSelection();
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            frmCardapioDigitalEditar frm = new frmCardapioDigitalEditar();
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
            if (dgvPrincipal.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione o item que deseja excluir!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(dgvPrincipal.SelectedRows[0].Cells["IDPDV"].Value == null)
            {
                MessageBox.Show("Não é possível excluir o tema padrão!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult r = MessageBox.Show("Tem certeza que deseja excluir esse item?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (r == DialogResult.Yes)
                {
                    try
                    {
                        Int32 idTemaCardapioPDV = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDTemaCardapioPDV"].Value);
                        TemaCardapioPDV.Excluir(idTemaCardapioPDV);

                        CarregarLista();
                        AtualizarGrid();
                    }
                    catch (Exception _e)
                    {
                        MessageBox.Show(_e.Message, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
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
                Int32 idTemaCardapioPDV = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDTemaCardapioPDV"].Value);

                frmCardapioDigitalEditar frm = new frmCardapioDigitalEditar(idTemaCardapioPDV);
                frm.ShowDialog();

                CarregarLista();
                AtualizarGrid();
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja alterar!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            Int32 idTemaCardapioPDV = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDTemaCardapioPDV"].Value);
            TemaCardapioPDV.AtualizarUltimaAlteracao(idTemaCardapioPDV);

            MessageBox.Show("Atualizado com sucesso.", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAtualizarTodos_Click(object sender, EventArgs e)
        {
            TemaCardapioPDV.AtualizarUltimaAlteracao();
        }
    }
}