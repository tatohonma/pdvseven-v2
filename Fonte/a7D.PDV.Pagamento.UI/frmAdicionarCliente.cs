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
using System.Configuration;

namespace a7D.PDV.Pagamento.UI
{
    public partial class frmAdicionarCliente : Form
    {
        public ClienteInformation Cliente1;

        public frmAdicionarCliente()
        {
            InitializeComponent();

            Cliente1 = new ClienteInformation();
        }

        public frmAdicionarCliente(string nome, string numero, string documento)
        {
            InitializeComponent();

            String ddd = Configuracao.RetornarValor("DDDPadrao");
            txtNomeCompleto.Text = nome;
            txtTelefone1DDD.Text = ddd;
            txtTelefone1Numero.Text = numero;
            txtDocumento1.Text = documento;

            Cliente1 = new ClienteInformation();
        }

        public frmAdicionarCliente(Int32 idCliente)
        {
            InitializeComponent();

            lblTitulo.Text = "ALTERAR CLIENTE";

            Cliente1 = Cliente.Carregar(idCliente);

            txtNomeCompleto.Text = Cliente1.NomeCompleto;
            txtEndereco.Text = Cliente1.Endereco;
            txtEnderecoNumero.Text = Cliente1.EnderecoNumero;
            txtComplemento.Text = Cliente1.Complemento;
            txtBairro.Text = Cliente1.Bairro;
            txtCidade.Text = Cliente1.Cidade;
            txtObservacao.Text = Cliente1.Observacao;
            ckbBloqueado.Checked = Cliente1.Bloqueado.Value;
            txtTelefone1DDD.Text = Cliente1.Telefone1DDD.ToString();
            txtTelefone1Numero.Text = Cliente1.Telefone1Numero.ToString();
            txtDocumento1.Text = Cliente1.Documento1;
            txtEmail.Text = Cliente1.Email;

            if (Cliente1.DataNascimento != null)
                txtDataNascimento.Text = Cliente1.DataNascimento.Value.ToString("dd/MM");

            if (Cliente1.Sexo == "m")
                rdbMasculino.Checked = true;
            else if (Cliente1.Sexo == "f")
                rdbFeminino.Checked = true;

            if (Cliente1.Bloqueado == true)
                ckbBloqueado.BackColor = Color.Red;
            else
                ckbBloqueado.BackColor = Color.Transparent;
        }

        private void frmAdicionarCliente_Load(object sender, EventArgs e)
        {
            cbbEstado.DataSource = Estado.Listar().OrderBy(l => l.Sigla).ToList();

            if (Cliente1.Estado != null && Cliente1.Estado.IDEstado != null)
            {
                cbbEstado.SelectedIndex = -1;
                cbbEstado.SelectedValue = Cliente1.Estado.IDEstado;
            }
            else
            {
                cbbEstado.SelectedIndex = -1;
                cbbEstado.SelectedValue = 25;
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            DateTime dataNascimento;

            if (Validar())
            {
                Cliente1.NomeCompleto = txtNomeCompleto.Text;
                Cliente1.Endereco = txtEndereco.Text;
                Cliente1.EnderecoNumero = txtEnderecoNumero.Text;
                Cliente1.Complemento = txtComplemento.Text;
                Cliente1.Bairro = txtBairro.Text;
                Cliente1.Cidade = txtCidade.Text;
                Cliente1.Observacao = txtObservacao.Text;
                Cliente1.Bloqueado = ckbBloqueado.Checked;
                Cliente1.Email = txtEmail.Text;

                if (rdbMasculino.Checked == true)
                    Cliente1.Sexo = "m";
                else if (rdbFeminino.Checked == true)
                    Cliente1.Sexo = "f";

                if (txtTelefone1DDD.Text != "")
                    Cliente1.Telefone1DDD = Convert.ToInt32(txtTelefone1DDD.Text);

                if (txtTelefone1Numero.Text != "")
                    Cliente1.Telefone1Numero = Convert.ToInt32(txtTelefone1Numero.Text);

                if (txtDataNascimento.Text != "" && DateTime.TryParse(txtDataNascimento.Text, out dataNascimento))
                    Cliente1.DataNascimento = dataNascimento;

                if (txtDocumento1.Text != "")
                    Cliente1.Documento1 = txtDocumento1.Text;

                if (cbbEstado.SelectedValue.ToString() != "")
                    Cliente1.Estado = new EstadoInformation { IDEstado = Convert.ToInt32(cbbEstado.SelectedValue) };

                Cliente.Salvar(Cliente1);

                this.Close();
            }
        }

        private bool Validar()
        {
            String msg = "";
            DateTime dt;

            if (txtNomeCompleto.Text == "")
                msg += "Campo \"Nome\" é obrigatório\n";

            //if (txtDataNascimento.Text == "")
            //    msg += "Campo \"Data de nascimento\" é obrigatório\n";

            if (txtDataNascimento.Text != "" && !DateTime.TryParse(txtDataNascimento.Text, out dt))
                msg += "Campo \"Data de nascimento\" inválido\n";

            if (txtTelefone1Numero.Text == "")
                msg += "Campo \"Telefone\" é obrigatório\n";

            if (rdbMasculino.Checked == false && rdbFeminino.Checked == false)
                msg += "Campo \"Sexo\" é obrigatório\n";

            if (msg != "")
            {
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                return true;
            }

        }

        private void ApenasNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = true;
        }

        private void NumeroDecimal_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || (e.KeyChar == (char)44 && (txt.Text.Contains(",") == true || txt.Text.Length == 0)))
                e.Handled = true;
        }

        private void txtDataNascimento_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = true;

            if (txtDataNascimento.Text.Length == 2 && e.KeyChar != (char)8)
                txtDataNascimento.Text += "/";

            txtDataNascimento.SelectionStart = txtDataNascimento.Text.Length + 1;
        }

        private void ckbBloqueado_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbBloqueado.Checked == true)
                ckbBloqueado.BackColor = Color.Red;
            else
                ckbBloqueado.BackColor = Color.Transparent;
        }
    }
}
