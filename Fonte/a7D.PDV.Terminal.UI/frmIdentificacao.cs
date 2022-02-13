using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Terminal.UI
{
    public partial class frmIdentificacao : FormTouch
    {
        public Boolean Fechar;
        public ETipoPedido tipoPedido_selecionado;
        public String GUIDIdentificacao_selecionado;
        private bool removerUsuario = false;

        public frmIdentificacao()
        {
            InitializeComponent();
        }

        private void frmIdentificacao_Load(object sender, EventArgs e)
        {
            removerUsuario = true;

            if (ConfiguracoesTerminalWindows.Valores.EsconderMesa & !ConfiguracoesTerminalWindows.Valores.EsconderComanda)
            {
                tipoPedido_selecionado = ETipoPedido.Comanda;
                lblTipo.Text = "Comanda";
                lblTipo.Visible = true;
                rdbComandas.Visible = rdbMesas.Visible = false;
            }
            else if (!ConfiguracoesTerminalWindows.Valores.EsconderMesa & ConfiguracoesTerminalWindows.Valores.EsconderComanda)
            {
                tipoPedido_selecionado = ETipoPedido.Mesa;
                lblTipo.Text = "Mesa";
                lblTipo.Visible = true;
                rdbComandas.Visible = rdbMesas.Visible = false;
            }
            else
            {
                tipoPedido_selecionado = ETipoPedido.Comanda;
                lblTipo.Visible = false;
            }

            teclado1.TecladoClick += new a7D.PDV.Componentes.Controles.TecladoClickEventHandler(teclado1_TecladoClick);
            teclado1.Confirmar += new EventHandler(teclado1_Confirmar);

            GUIDIdentificacao_selecionado = null;
        }

        private void rdbMesas_CheckedChanged(object sender, EventArgs e)
        {
            tipoPedido_selecionado = ETipoPedido.Mesa;
        }

        private void rdbComandas_CheckedChanged(object sender, EventArgs e)
        {
            tipoPedido_selecionado = ETipoPedido.Comanda;
        }

        private void Confirmar()
        {
            Int32 numero;
            MesaInformation mesa;
            ComandaInformation comanda;

            try
            {
                if (txtIdentificacao.Text == "")
                    throw new Exception("Informar o número da Mesa/Comanda");

                numero = Convert.ToInt32(txtIdentificacao.Text);

                switch (tipoPedido_selecionado)
                {
                    case ETipoPedido.Mesa:
                        mesa = Mesa.Validar(numero);
                        GUIDIdentificacao_selecionado = mesa.GUIDIdentificacao;
                        removerUsuario = false;
                        this.Close();

                        break;
                    case ETipoPedido.Comanda:
                        comanda = Comanda.Validar(numero);
                        GUIDIdentificacao_selecionado = comanda.GUIDIdentificacao;
                        removerUsuario = false;
                        this.Close();

                        break;
                }
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.A012, ex);
            }
        }

        protected void teclado1_TecladoClick(object sender, EventArgs e, Int32 n)
        {
            txtIdentificacao.Text += n.ToString();
        }

        protected void teclado1_Confirmar(object sender, EventArgs e)
        {
            Confirmar();
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (txtIdentificacao.Text.Length > 0)
                txtIdentificacao.Text = txtIdentificacao.Text.Substring(0, txtIdentificacao.Text.Length - 1);
        }

        private void ApenasNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar))
                e.Handled = true;
        }

        private void txtIdentificacao_Leave(object sender, EventArgs e)
        {
            txtIdentificacao.Focus();
        }

        private void txtIdentificacao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Confirmar();
            }
        }

        private void frmIdentificacao_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (removerUsuario)
                AC.RegitraUsuario(null);
        }
    }
}
