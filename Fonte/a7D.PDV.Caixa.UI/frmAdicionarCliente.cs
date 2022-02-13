using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Models;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmAdicionarCliente : FormTouch
    {
        public tbCliente Cliente1;
        public bool Delivery { get; set; }

        public frmAdicionarCliente()
        {
            InitializeComponent();
        }

        public frmAdicionarCliente(string nome, string numero, string documento, string email = "") : this()
        {
            this.Text = "INCLUIR CLIENTE";
            Cliente1 = new tbCliente();
            btnExtrato.Visible = false;
            txtNomeCompleto.Text = nome;
            txtTelefone1DDD.Text = ConfiguracoesSistema.Valores.DDDPadrao.ToString();
            txtTelefone1Numero.Text = numero;
            txtDocumento1.Text = documento;
            txtEmail.Text = email;
        }

        public frmAdicionarCliente(Int32 idCliente) : this()
        {
            Cliente1 = EF.Repositorio.Carregar<tbCliente>(c => c.IDCliente == idCliente);

            this.Text = "ALTERAR CLIENTE";
            btnExtrato.Visible = frmPrincipal.ContaCliente;
            txtNomeCompleto.Text = Cliente1.NomeCompleto;
            txtEndereco.Text = Cliente1.Endereco;
            txtEnderecoNumero.Text = Cliente1.EnderecoNumero;
            txtComplemento.Text = Cliente1.Complemento;
            txtBairro.Text = Cliente1.Bairro;
            txtCidade.Text = Cliente1.Cidade;
            txtObservacao.Text = Cliente1.Observacao;
            ckbBloqueado.Checked = Cliente1.Bloqueado;
            txtTelefone1DDD.Text = Cliente1.Telefone1DDD.ToString();
            txtTelefone1Numero.Text = Cliente1.Telefone1Numero.ToString();
            txtDocumento1.Text = Cliente1.Documento1;
            txtRG.Text = Cliente1.RG;
            txtEmail.Text = Cliente1.Email;

            if (Cliente1.DataNascimento != null)
                txtDataNascimento.Text = Cliente1.DataNascimento.Value.ToString("dd/MM/yyyy");

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
            GA.Post(this);
            cbbEstado.DataSource = Estado.Listar().OrderBy(l => l.Sigla).ToList();

            if (Cliente1.IDEstado != null)
                cbbEstado.SelectedValue = Cliente1.IDEstado;
            else
                cbbEstado.SelectedValue = 25; // SP
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {

            if (!Validar())
                return;

            Cliente1.NomeCompleto = txtNomeCompleto.Text;
            Cliente1.Endereco = txtEndereco.Text;
            Cliente1.EnderecoNumero = txtEnderecoNumero.Text;
            Cliente1.Complemento = txtComplemento.Text;
            Cliente1.Bairro = txtBairro.Text;
            Cliente1.Cidade = txtCidade.Text;
            Cliente1.Observacao = txtObservacao.Text;
            Cliente1.Bloqueado = ckbBloqueado.Checked;
            Cliente1.Email = txtEmail.Text;
            Cliente1.Documento1 = txtDocumento1.Text;
            Cliente1.RG = txtRG.Text;

            if (rdbMasculino.Checked == true)
                Cliente1.Sexo = "m";
            else if (rdbFeminino.Checked == true)
                Cliente1.Sexo = "f";

            if (string.IsNullOrEmpty(txtDataNascimento.Text))
                Cliente1.DataNascimento = null;
            else
                Cliente1.DataNascimento = DateTime.Parse(txtDataNascimento.Text);

            if (string.IsNullOrEmpty(txtTelefone1DDD.Text))
                Cliente1.Telefone1DDD = 0;
            else
                Cliente1.Telefone1DDD = Convert.ToInt32(txtTelefone1DDD.Text);

            if (string.IsNullOrEmpty(txtTelefone1Numero.Text))
                Cliente1.Telefone1Numero = 0;
            else
                Cliente1.Telefone1Numero = Convert.ToInt32(txtTelefone1Numero.Text);

            if (string.IsNullOrEmpty(cbbEstado.SelectedValue.ToString()))
                Cliente1.IDEstado = null;
            else
                Cliente1.IDEstado = Convert.ToInt32(cbbEstado.SelectedValue);
            if (Cliente1.IDCliente == 0)
            {
                Cliente1.DtInclusao = DateTime.Now;
                EF.Repositorio.Inserir(Cliente1);
            }
            else
                EF.Repositorio.Atualizar(Cliente1);

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool Validar()
        {
            txtDocumento1.Text = txtDocumento1.Text.Trim();
            txtRG.Text = txtRG.Text.Trim();

            string msg = Cliente.ValidarCliente(Cliente1.IDCliente,
                txtNomeCompleto.Text, txtEndereco.Text + txtEnderecoNumero, txtComplemento.Text, txtBairro.Text, txtCidade.Text,
                txtDocumento1.Text, txtRG.Text, txtDataNascimento.Text, Delivery);

            if (txtTelefone1Numero.Text == "")
                msg += "Campo \"Telefone\" é obrigatório\n";

            if (rdbMasculino.Checked == false && rdbFeminino.Checked == false)
                msg += "Campo \"Sexo\" é obrigatório\n";

            if (!string.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg);
                return false;
            }
            else
                return true;
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

            if ((txtDataNascimento.Text.Length == 2 || txtDataNascimento.Text.Length == 5) && e.KeyChar != (char)8)
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

        private void txtDocumento1_Leave(object sender, EventArgs e)
        {
            if (txtDocumento1.Text.Length == 11 && !ValidacaoDocumento.IsCpf(txtDocumento1.Text))
            {
                MessageBox.Show("CPF Inválido!");
                txtDocumento1.Focus();
            }
            else if (txtDocumento1.Text.Length == 14 && !ValidacaoDocumento.IsCnpj(txtDocumento1.Text))
            {
                MessageBox.Show("CNPJ Inválido!");
                txtDocumento1.Focus();
            }
            else if (txtDocumento1.Text.Length != 0 && txtDocumento1.Text.Length != 11 && txtDocumento1.Text.Length != 14)
            {
                MessageBox.Show("CPF/CNPJ Incompleto!");
                txtDocumento1.Focus();
            }
        }

        private void btnExtrato_Click(object sender, EventArgs e)
        {
            using (var frm = new frmExtratoCreditos(Cliente1.IDCliente))
                frm.ShowDialog();
        }
    }
}
