using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Models;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmContaCliente : FormTouch
    {
        public string Documento
        {
            get => txtDocumento.Text;
            set => txtDocumento.Text = value;
        }

        public decimal Valor { get; internal set; }

        public bool NaoValidarLimite { get; set; }

        public int IDCliente { get; set; }

        public decimal SaldoAtual { get; set; }

        public frmContaCliente()
        {
            InitializeComponent();
        }

        private void frmContaCliente_Load(object sender, System.EventArgs e)
        {
            lblInfo.Text = "";
        }

        private void txtDocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            lblInfo.Text = "";
            var txt = (TextBox)sender;
            if (e.KeyChar == 13) // ENTER
            {
                btnPesquisar_Click(null, null);
            }
            else if (e.KeyChar != 8 // BACKSPACE
                  && e.KeyChar != 9 // TAB
                  && e.KeyChar != '\u0018' // CTRL+X
                  && e.KeyChar != '\u0003' // CTRL+C
                  && e.KeyChar != '\u0016' // CTRL+V
                  && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnPesquisar_Click(object sender, System.EventArgs e)
        {
            try
            {
                string documento = Documento;
                if (string.IsNullOrEmpty(documento))
                {
                    lblInfo.Text = "Informe o documento do cliente!";
                    return;
                }
                else if (documento.Length < 11)
                {
                    lblInfo.Text = "Informe o documento completo do cliente!";
                    return;
                }

                var clientes = EF.Repositorio.Listar<tbCliente>(c => c.Documento1 == documento).ToList();
                if (clientes.Count > 1)
                    lblInfo.Text = "Há mais de um cadastro com este documento";
                else if (clientes.Count == 0)
                    lblInfo.Text = "Nenhum cliente encontrado";
                else
                {
                    var cliente = clientes.First();
                    IDCliente = cliente.IDCliente;
                    SaldoAtual = Saldo.ClienteSaldoBruto(IDCliente);
                    var final = SaldoAtual - Valor;
                    lblInfo.Text = $"Cliente: {cliente.NomeCompleto}\r\nSaldo: R$ {SaldoAtual.ToString("N2")} - R$ {Valor.ToString("N2") } = R$ { final.ToString("N2")}";
                    // TODO: Exibir o numero total de outros pedidos em aberto?
                    if (final < 0 && NaoValidarLimite)
                    {
                        lblInfo.Text += "\r\nATENÇÃO: O saldo final ficará negativo!";
                        btnPesquisar.Visible = false;
                        btnConfirmar.Visible = true;
                        btnConfirmar.Focus();
                    }
                    else if (final >= -ConfiguracoesSistema.Valores.LimiteFiado)
                    {
                        btnPesquisar.Visible = false;
                        btnConfirmar.Visible = true;
                        btnConfirmar.Focus();
                    }
                    else
                        lblInfo.Text += "\r\nSaldo final ultrapassa o limite configurado de R$ " + ConfiguracoesSistema.Valores.LimiteFiado.ToString("N2");
                }
            }
            catch (System.Exception ex)
            {
                lblInfo.Text = ex.Message;
            }
        }

        private void btnConfirmar_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtDocumento_TextChanged(object sender, System.EventArgs e)
        {
            btnPesquisar.Visible = true;
            btnConfirmar.Visible = false;
            lblInfo.Text = "";
        }
    }
}
