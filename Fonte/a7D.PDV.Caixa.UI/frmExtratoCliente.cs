using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using System;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmExtratoCliente : FormTouch
    {
        public frmExtratoCliente()
        {
            InitializeComponent();
        }

        private void frmExtratoCliente_Load(object sender, EventArgs e)
        {
            this.LoadLocationSize(EConfig._ClienteSaldoLocationSize);
            txtNome_TextChanged(sender, e);
        }

        private void frmExtratoCliente_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            GA.Post(this);
            this.SaveLocationSize(EConfig._ClienteSaldoLocationSize);
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            this.dgvClientes.DataSource = Saldo.BuscaClientes(txtNome.Text);
        }

        private void dgvClientes_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var idCliente = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells[0].Value);
                var frm = new frmExtratoCreditos(idCliente);
                frm.ShowDialog();
            }
        }
    }
}
