using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmUnidadeEditar : Form
    {

        private UnidadeInformation Unidade1 { get; set; }

        public frmUnidadeEditar()
        {
            InitializeComponent();
            Unidade1 = new UnidadeInformation
            {
                Excluido = false
            };
        }

        public frmUnidadeEditar(int idUnidade)
        {
            Unidade1 = Unidade.Carregar(idUnidade);
            InitializeComponent();
        }

        private void frmUnidadeEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            if (Unidade1.IDUnidade.HasValue)
            {
                txtNome.Text = Unidade1.Nome;
                txtSimbolo.Text = Unidade1.Simbolo;
            }
            else
                LimparCampos();
        }

        private void LimparCampos()
        {
            txtNome.Text = string.Empty;
            txtSimbolo.Text = string.Empty;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                Unidade1.Nome = txtNome.Text;
                Unidade1.Simbolo = txtSimbolo.Text;

                Unidade.Salvar(Unidade1);
                Close();
            }
        }

        private bool Validar()
        {
            var message = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtNome.Text))
                message.AppendLine("Nome não pode ser vazio");

            if (string.IsNullOrWhiteSpace(txtSimbolo.Text))
                message.AppendLine("Símbolo não pode ser vazio");
            else if (Unidade.ExisteSimbolo(Unidade1.IDUnidade, txtSimbolo.Text))
                message.AppendLine("Esse símbolo já foi cadastrado");

            if (string.IsNullOrWhiteSpace(message.ToString()) == false)
                MessageBox.Show(message.ToString(), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return string.IsNullOrWhiteSpace(message.ToString());
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
