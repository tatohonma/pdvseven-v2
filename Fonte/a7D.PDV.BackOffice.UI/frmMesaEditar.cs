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
    public partial class frmMesaEditar : Form
    {
        private MesaInformation Mesa1;
        private Boolean AdicionarDiversos;

        public frmMesaEditar()
        {
            InitializeComponent();

            Mesa1 = new MesaInformation();
            Mesa1.StatusMesa = new StatusMesaInformation { IDStatusMesa = 10 };
            Mesa1.GUIDIdentificacao = Guid.NewGuid().ToString();
        }

        public frmMesaEditar(Boolean adicionarDiversos)
        {
            InitializeComponent();

            Mesa1 = new MesaInformation();
            AdicionarDiversos = true;

            lblAte.Visible = true;
            txtNumero2.Visible = true;
        }

        public frmMesaEditar(Int32 idMesa)
        {
            InitializeComponent();

            Mesa1 = Mesa.Carregar(idMesa);

            txtNumero1.Text = Mesa1.Numero.ToString();
            txtCapacidade.Text = Mesa1.Capacidade.ToString();
        }

        private void frmMesaEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar() == true)
            {
                if (AdicionarDiversos == true)
                {
                    Int32 numero1 = Convert.ToInt32(txtNumero1.Text);
                    Int32 numero2 = Convert.ToInt32(txtNumero2.Text);

                    for (int i = numero1; i <= numero2; i++)
                    {
                        MesaInformation mesa = Mesa.CarregarPorNumero(i);
                        if (mesa.IDMesa == null)
                        {
                            Mesa1 = new MesaInformation();

                            Mesa1.StatusMesa = new StatusMesaInformation { IDStatusMesa = 10 };
                            Mesa1.GUIDIdentificacao = Guid.NewGuid().ToString();
                            Mesa1.Numero = i;

                            if (txtCapacidade.Text != "")
                                Mesa1.Capacidade = Convert.ToInt32(txtCapacidade.Text);

                            Mesa.Salvar(Mesa1);
                        }
                    }
                }
                else
                {
                    Mesa1.Numero = Convert.ToInt32(txtNumero1.Text);

                    if (txtCapacidade.Text != "")
                        Mesa1.Capacidade = Convert.ToInt32(txtCapacidade.Text);

                    Mesa.Salvar(Mesa1);
                }

                this.Close();
            }
        }

        private Boolean Validar()
        {
            String msg = "";
            Int32 i;

            if (txtNumero1.Text == "")
                msg += "Campo \"Mesa\" é obrigatório. \n";

            if (Int32.TryParse(txtNumero1.Text, out i) == false)
                msg += "Campo \"Mesa\" deve ser numérico. \n";

            if (txtNumero2.Text.Length > 0 && Int32.TryParse(txtNumero2.Text, out i) == false)
                msg += "Campo \"Mesa (Até)\" deve ser numérico. \n";

            if (txtNumero2.Text.Length > 0 && Convert.ToInt32(txtNumero1.Text) >= Convert.ToInt32(txtNumero2.Text))
                msg += "Campo \"Mesa (Até)\" deve ser maior que \"Mesa (De)\". \n";

            if (txtCapacidade.Text.Length > 0 && Int32.TryParse(txtCapacidade.Text, out i) == false)
                msg += "Campo \"Capacidade\" deve ser numérico. \n";

            if (msg.Length == 0 && txtNumero2.Text.Length == 0)
            {                
                MesaInformation mesa = Mesa.CarregarPorNumero(Convert.ToInt32(txtNumero1.Text));
                if (Mesa1.IDMesa != mesa.IDMesa && mesa.IDMesa != null)
                    msg += "Mesa já cadastrada. \n";
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
