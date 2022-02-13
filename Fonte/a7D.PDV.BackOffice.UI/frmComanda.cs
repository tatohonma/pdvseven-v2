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
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmComanda : Form
    {
        //List<ComandaInformation> Lista;
        bool usarHex;

        public frmComanda()
        {
            InitializeComponent();
        }

        private void frmComanda_Load(object sender, EventArgs e)
        {
            GA.Post(this);

            usarHex = ConfiguracaoBD.ValorOuPadrao(EConfig.ComandaCodigoHEX, ETipoPDV.CAIXA) == "1";
            lblTitulo.Text = usarHex ? "Número ou Código Leitora (HEX)" : "Número ou Código Leitora (DEC)";

            CarregarLista();
        }

        private void CarregarLista()
        {
            var tbComanda = Comanda.ListarTodasOuCodigo(textBox1.Text, usarHex);
            var results = from l in tbComanda.AsEnumerable()
                          select new
                          {
                              IDComanda = l.Field<int>("IDComanda"),
                              Numero = l.Field<int>("Numero"),
                              Codigo = CodigoHex(l.Field<long?>("Codigo"))
                          };

            dgvPrincipal.DataSource = results.ToArray();
        }

        private static string CodigoHex(long? codigo)
        {
            return codigo == null ? "" : $"{codigo.Value.ToString("X8")} / {codigo.Value}";
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            frmComandaEditar frm = new frmComandaEditar();
            frm.ShowDialog();

            CarregarLista();
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
                        Int32 idComanda = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDComanda"].Value);
                        Comanda.Excluir(idComanda);

                        CarregarLista();
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

        private void btnAdicionarDiversos_Click(object sender, EventArgs e)
        {
            var frm = new frmComandaDiversas();
            frm.ShowDialog();

            CarregarLista();
        }

        private void dgvPrincipal_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Alterar();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count > 0)
            {
                Int32 idComanda = Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells["IDComanda"].Value);

                frmComandaEditar frm = new frmComandaEditar(idComanda);
                frm.ShowDialog();

                CarregarLista();
            }
            else
            {
                MessageBox.Show("Selecione o item que deseja alterar!", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Comanda.KeyPressValid(e))
                e.Handled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CarregarLista();
        }
    }
}
