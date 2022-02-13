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
    public partial class frmTipoEntradaEditar : Form
    {
        private TipoEntradaInformation TipoEntrada1;

        public frmTipoEntradaEditar()
        {
            InitializeComponent();
            TipoEntrada1 = new TipoEntradaInformation();
        }

        public frmTipoEntradaEditar(Int32 idTipoEntrada)
        {
            InitializeComponent();

            TipoEntrada1 = TipoEntrada.Carregar(idTipoEntrada);
            txtNome.Text = TipoEntrada1.Nome;
            txtValorEntrada.Text = TipoEntrada1.ValorEntrada.Value.ToString("#,##0.00");
            txtValorConsumacaoMinima.Text = TipoEntrada1.ValorConsumacaoMinima.Value.ToString("#,##0.00");
            ckbAtivo.Checked = TipoEntrada1.Ativo.Value;
            ckbPadrao.Checked = TipoEntrada1.Padrao == true;
        }

        private void frmTipoEntradaEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar() == true)
            {
                TipoEntrada1.Nome = txtNome.Text;
                TipoEntrada1.Ativo = ckbAtivo.Checked;
                TipoEntrada1.Padrao = ckbPadrao.Checked;

                if (txtValorEntrada.Text != "")
                    TipoEntrada1.ValorEntrada = Convert.ToDecimal(txtValorEntrada.Text);
                else
                    TipoEntrada1.ValorEntrada = 0;

                if (txtValorConsumacaoMinima.Text != "")
                    TipoEntrada1.ValorConsumacaoMinima = Convert.ToDecimal(txtValorConsumacaoMinima.Text);
                else
                    TipoEntrada1.ValorConsumacaoMinima = 0;

                TipoEntrada.Salvar(TipoEntrada1);

                this.Close();
            }
        }

        private Boolean Validar()
        {
            String msg = "";
            Decimal d = 0;

            if (txtNome.Text == "")
                msg += "Campo \"Nome\" é obrigatório. \n";

            if (Decimal.TryParse(txtValorEntrada.Text, out d) == false || d < 0)
                msg += "Campo \"Valor entrada \" deve ser numérico. \n";

            if (Decimal.TryParse(txtValorConsumacaoMinima.Text, out d) == false || d < 0)
                msg += "Campo \"Valor consumação mínima\" deve ser numérico. \n";

            var entradaPadrao = TipoEntrada.BuscarPadrao();

            if (entradaPadrao?.IDTipoEntrada == null && ckbPadrao.Checked == false)
                msg += "Nenhuma comanda padrão definida!\n";

            else if (entradaPadrao?.IDTipoEntrada == TipoEntrada1.IDTipoEntrada && ckbPadrao.Checked == false)
                msg += "É necessário sempre haver uma entrada padrão ativa\n";

            if (msg.Length > 0)
            {
                MessageBox.Show(msg, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
