using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmComandaDiversas : Form
    {
        private ComandaInformation Comanda1;

        public frmComandaDiversas()
        {
            InitializeComponent();

            Comanda1 = new ComandaInformation
            {
                StatusComanda = EStatusComanda.Liberada.ToObjInfo(),
                GUIDIdentificacao = Guid.NewGuid().ToString()
            };

            Comanda1 = new ComandaInformation();

            lblAte.Visible = true;
            txtNumero2.Visible = true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar() == true
             && Int32.TryParse(txtNumero1.Text, out Int32 numero1)
             && Int32.TryParse(txtNumero2.Text, out Int32 numero2))
            {
                for (int i = numero1; i <= numero2; i++)
                {
                    ComandaInformation mesa = Comanda.CarregarPorNumeroOuCodigo(i);
                    if (mesa.IDComanda == null)
                    {
                        Comanda1 = new ComandaInformation
                        {
                            StatusComanda = EStatusComanda.Liberada.ToObjInfo(),
                            GUIDIdentificacao = Guid.NewGuid().ToString(),
                            Numero = i
                        };
                        Comanda.Salvar(Comanda1);
                    }
                }

                this.Close();
            }
        }

        private Boolean Validar()
        {
            String msg = "";

            if (txtNumero1.Text == "")
                msg += "Campo \"Comanda\" é obrigatório. \n";

            if (Int32.TryParse(txtNumero1.Text, out int inicio) == false)
                msg += "Campo \"Comanda\" deve ser numérico. \n";

            if (Int32.TryParse(txtNumero2.Text, out int fim) == false)
                msg += "Campo \"Comanda (Até)\" deve ser numérico. \n";

            if (inicio >= fim)
                msg += "Campo \"Comanda (Até)\" deve ser maior que \"Comanda (De)\". \n";

            if (fim - inicio > 1000)
                msg += "Por segurança, só é permitido cadastrar 1000 de cada vez\n";

            if (msg.Length == 0 && txtNumero2.Text.Length == 0)
            {
                ComandaInformation mesa = Comanda.CarregarPorNumeroOuCodigo(Convert.ToInt32(txtNumero1.Text));
                if (Comanda1.IDComanda != mesa.IDComanda && mesa.IDComanda != null)
                    msg += "Comanda já cadastrada. \n";
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
