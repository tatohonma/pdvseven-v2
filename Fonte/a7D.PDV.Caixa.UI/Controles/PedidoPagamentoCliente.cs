using a7D.PDV.BLL;
using a7D.PDV.BLL.Utils;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI.Controles
{
    public partial class PedidoPagamentoCliente : UserControl
    {
        private IPedidoPagamentoForm frmPP;
        private PedidoInformation PedidoAtual => frmPP?.PedidoAtual;

        public bool DocumentoNaNota => ckbNF.Checked;

        public string DocumentoCliente => txtDocumentoCliente.Text;

        public string EmailCliente => txtEmail.Text;

        public PedidoPagamentoCliente()
        {
            InitializeComponent();
        }

        private void PedidoPagamentoCliente_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            frmPP = this.ParentForm as IPedidoPagamentoForm;
            if (PedidoAtual == null)
                return;

            txtDocumentoCliente.Text = string.IsNullOrWhiteSpace(PedidoAtual.DocumentoCliente) ? string.Empty : PedidoAtual.DocumentoCliente;
            txtEmail.Text = string.IsNullOrWhiteSpace(PedidoAtual.EmailCliente) ? string.Empty : PedidoAtual.EmailCliente;
            ckbEnviarEmail.Checked = !string.IsNullOrEmpty(txtEmail.Text);

            SelecaoCliente();
        }

        private void SelecaoCliente()
        {
            if (PedidoAtual.Cliente != null && PedidoAtual.Cliente.IDCliente != null)
            {
                txtNomeCliente.Text = PedidoAtual.Cliente.NomeCompleto;

                if (string.IsNullOrWhiteSpace(txtDocumentoCliente.Text))
                    txtDocumentoCliente.Text = PedidoAtual.Cliente.Documento1;

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                    txtEmail.Text = PedidoAtual.Cliente.Email;
            }
        }

        void txtDocumentoCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar))
                e.Handled = true;
        }

        private void txtDocumentoCliente_Leave(object sender, EventArgs e)
        {
            if (txtDocumentoCliente.Text.Length == 11 && !ValidacaoDocumento.IsCpf(txtDocumentoCliente.Text))
            {
                MessageBox.Show("CPF Inválido!");
                txtDocumentoCliente.Focus();
            }
            else if (txtDocumentoCliente.Text.Length == 14 && !ValidacaoDocumento.IsCnpj(txtDocumentoCliente.Text))
            {
                MessageBox.Show("CNPJ Inválido!");
                txtDocumentoCliente.Focus();
            }
            else if (txtDocumentoCliente.Text.Length != 0 && txtDocumentoCliente.Text.Length != 11 && txtDocumentoCliente.Text.Length != 14)
            {
                MessageBox.Show("CPF/CNPJ Incompleto!");
                txtDocumentoCliente.Focus();
            }
        }

        private void btnSelecionarCliente_Click(object sender, EventArgs e)
        {
            if (sender is TextBox && PedidoAtual.Cliente != null && PedidoAtual.Cliente.IDCliente != null)
            {
                if (NormalOuTouch.Autenticacao(false, true, false, false, out UsuarioInformation usuario) == DialogResult.OK)
                {
                    var form = new frmAdicionarCliente(PedidoAtual.Cliente.IDCliente.Value);
                    form.ShowDialog();
                }
            }
            else
            {
                var frm = new frmPesquisarCliente();
                var result = frm.ShowDialog();

                PedidoAtual.Cliente = frm.Cliente1;
                if (PedidoAtual.Cliente != null && PedidoAtual.Cliente.IDCliente != null)
                    txtNomeCliente.Text = PedidoAtual.Cliente.NomeCompleto;
            }

            SelecaoCliente();
        }

        private void ckbEnviarEmail_CheckedChanged(object sender, EventArgs e)
        {
            PedidoAtual.EnviarNfEmailCliente = false;
            if (!ckbEnviarEmail.Checked)
                return;

            if (txtEmail.Text == "")
            {
                ckbEnviarEmail.Checked = false;
                MessageBox.Show("Digite um email.");
                txtEmail.Focus();
                return;
            }

            if (!EmailUtil.IsValidEmail(txtEmail.Text))
            {
                ckbEnviarEmail.Checked = false;
                MessageBox.Show("Digite um email válido.");
                txtEmail.Focus();
                return;
            }

            PedidoAtual.EmailCliente = txtEmail.Text;
            PedidoAtual.EnviarNfEmailCliente = true;
        }

        private void txtDocumentoCliente_DoubleClick(object sender, EventArgs e)
        {
            btnTeclado_Click(sender, e);
        }

        private void btnTeclado_Click(object sender, EventArgs e)
        {
            decimal.TryParse(txtDocumentoCliente.Text, out decimal valor);

            var frm = new frmTecladoValor(valorInicial: valor)
            {
                Text = "Informe CPF ou CNPJ",
                Formato = @"000\.###\.###-##",
                LimiteDigitoFormato1 = 11, // CPF 
                Formato2 = @"00\.###\.###/####-##",
                Divisor = 1, // Sem divisor
                MaxDigits = 14,
                PermitirZero = true
            };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.Valor > 0)
                {
                    txtDocumentoCliente.Text = frm.Valor.ToString();
                    if (txtDocumentoCliente.Text.Length <= 11)
                        txtDocumentoCliente.Text = txtDocumentoCliente.Text.PadLeft(11, '0');
                    else if (txtDocumentoCliente.Text.Length <= 14)
                        txtDocumentoCliente.Text = txtDocumentoCliente.Text.PadLeft(14, '0');

                    txtDocumentoCliente_Leave(sender, e);
                }
                else
                    txtDocumentoCliente.Text = "";
            }
        }
    }
}
