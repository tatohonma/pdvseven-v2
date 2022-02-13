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
    public partial class frmCategoriaEditar : Form
    {
        private CategoriaProdutoInformation CategoriaProduto1;

        public frmCategoriaEditar()
        {
            InitializeComponent();

            CategoriaProduto1 = new CategoriaProdutoInformation();
        }

        public frmCategoriaEditar(Int32 idCategoriaProduto)
        {
            InitializeComponent();

            CategoriaProduto1 = CategoriaProduto.Carregar(idCategoriaProduto);

            txtNome.Text = CategoriaProduto1.Nome;
            txtCodigoERP.Text = CategoriaProduto1.CodigoERP;
        }

        private void frmCategoriaEditar_Load(object sender, EventArgs e)
        {

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar() == true)
            {
                CategoriaProduto1.Nome = txtNome.Text;
                CategoriaProduto1.CodigoERP = txtCodigoERP.Text;

                CategoriaProduto.Salvar(CategoriaProduto1);

                this.Close();
            }
        }

        private Boolean Validar()
        {
            String msg = "";

            if (txtNome.Text == "")
                msg += "Campo \"Nome\" é obrigatório. \n";

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
