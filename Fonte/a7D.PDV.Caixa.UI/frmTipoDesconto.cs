using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmTipoDesconto : FormTouch
    {
        public frmTipoDesconto()
        {
            InitializeComponent();
        }

        public int IDTipoDesconto { get; private set; }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (cbbTipoDesconto.SelectedIndex > 0)
            {
                IDTipoDesconto = ((TipoDescontoInformation)cbbTipoDesconto.SelectedItem).IDTipoDesconto.Value;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
                MessageBox.Show("Selecione o Tipo de Desconto", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmTipoDesconto_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            var listaTipoDesconto = TipoDesconto.ListarAtivos();
            listaTipoDesconto.Insert(0, new TipoDescontoInformation());

            cbbTipoDesconto.DataSource = listaTipoDesconto;
        }
    }
}
