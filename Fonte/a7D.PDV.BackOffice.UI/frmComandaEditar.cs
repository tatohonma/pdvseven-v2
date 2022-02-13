using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmComandaEditar : Form
    {
        private ComandaInformation Comanda1;

        public frmComandaEditar()
        {
            InitializeComponent();
            if (DesignMode)
                return;

            Comanda1 = new ComandaInformation
            {
                StatusComanda = EStatusComanda.Liberada.ToObjInfo(),
                GUIDIdentificacao = Guid.NewGuid().ToString(),
            };
        }

        public frmComandaEditar(Int32 idComanda)
        {
            InitializeComponent();
            Comanda1 = Comanda.Carregar(idComanda);
            txtNumero1.Text = Comanda1.Numero.ToString();
        }

        private void frmComandaEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            txtNumero1.MaxLength = Comanda.DigitosCodigo - 1;
            if (Comanda1.Codigo.HasValue)
            {
                txtLeitoraDEC.Text = Comanda1.Codigo.Value.ToString();
                txtLeitoraHEX.Text = Comanda1.Codigo.Value.ToString("X8");
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;

            try
            {
                Comanda1.Numero = Convert.ToInt32(txtNumero1.Text);
                if (!string.IsNullOrEmpty(txtLeitoraDEC.Text))
                    Comanda1.Codigo = Convert.ToInt64(txtLeitoraDEC.Text);

                Comanda.Salvar(Comanda1);
                this.Close();
            }
            catch (Exception ex)
            {
                Logs.Erro(ex);
            }
        }

        private Boolean Validar()
        {
            String msg = "";

            if (string.IsNullOrEmpty(txtNumero1.Text))
                msg += "Campo \"Comanda\" é obrigatório. \n";
            else if (!Int32.TryParse(txtNumero1.Text, out int icmd))
                msg += "Campo \"Comanda\" deve ser numérico. \n";
            else if (icmd.ToString().Length >= Comanda.DigitosCodigo)
                msg += $"O número da comanda deve ter até {Comanda.DigitosCodigo - 1} dígitos\n";
            else
            {
                var comanda = Comanda.CarregarPorNumeroOuCodigo(icmd);
                if (Comanda1.IDComanda != comanda.IDComanda && comanda.IDComanda != null)
                    msg += "Comanda número " + txtNumero1.Text + " já cadastrada. \n";
            }

            if (msg.Length == 0 && !string.IsNullOrEmpty(txtLeitoraDEC.Text))
            {
                if (!long.TryParse(txtLeitoraDEC.Text, out long cod))
                    msg += $"Código da Leitora (DEC) inválido\n";
                else if (cod.ToString().Length < Comanda.DigitosCodigo)
                    msg += $"Campo \"Código Leitora (DEC)\" deve ser numérico com no mínimo {Comanda.DigitosCodigo} dígitos.\n";
                else
                {
                    var comanda = Comanda.CarregarPorNumeroOuCodigo(cod);
                    if (Comanda1.IDComanda != comanda.IDComanda && comanda.IDComanda != null)
                        msg += "Código " + txtLeitoraDEC.Text + " já cadastrada na comanda número " + comanda.Numero + ". \n";
                }
            }

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

        private void txtLeitoraHEX_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtLeitoraHEX.Text.Length > 0)
                {
                    var cod = Convert.ToInt64(txtLeitoraHEX.Text, 16);
                    txtLeitoraDEC.Text = cod.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Código Leitora (HEX) inválido");
            }
        }

        private void txtLeitoraDEC_Leave(object sender, EventArgs e)
        {
            if (txtLeitoraDEC.Text.Length > 0 && long.TryParse(txtLeitoraDEC.Text, out long cod))
                txtLeitoraHEX.Text = cod.ToString("X8");
            else
                txtLeitoraHEX.Text = "";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
