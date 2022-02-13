using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using a7D.PDV.Componentes;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmAlterarEntrada : FormTouch
    {
        ComandaInformation Comanda1;
        PedidoInformation Pedido1;
        Int32 IDUsuario;

        public frmAlterarEntrada(Int32 idUsuario)
        {
            IDUsuario = idUsuario;

            InitializeComponent();
        }

        private void frmAlterarEntrada_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            List<TipoEntradaInformation> listaTipoEntrada = TipoEntrada.Listar().Where(l => l.Ativo == true).OrderBy(l => l.Nome).ToList();

            cbbTipoEntrada.Items.Add("");
            foreach (var item in listaTipoEntrada)
                cbbTipoEntrada.Items.Add(item);

            cbbTipoEntrada.SelectedValue = "";
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            Comanda1 = null;
            txtCodigoComanda.Enabled = true;
            btnAlterarTipoEntrada.Enabled = false;
            txtNomeCompleto.Text = "";
            txtTelefone1DDD.Text = "";
            txtTelefone1Numero.Text = "";
            cbbTipoEntrada.Enabled = false;

            if (txtCodigoComanda.Text != "")
            {
                Comanda1 = ComandaUtil.CarregarPorNumeroOuCodigo(txtCodigoComanda.Text);
                if (Comanda1.IDComanda == null)
                {
                    MessageBox.Show("Comanda não cadastrada!");
                }
                else if (Comanda1.ValorStatusComanda != EF.Enum.EStatusComanda.EmUso)
                {
                    MessageBox.Show("Comanda não aberta!");
                }
                else
                {
                    Pedido1 = Pedido.CarregarUltimoPedido(Comanda1.GUIDIdentificacao);
                    if (Pedido1.IDPedido != null)
                    {
                        txtNomeCompleto.Text = Pedido1.Cliente.NomeCompleto;
                        txtTelefone1DDD.Text = Pedido1.Cliente.Telefone1DDD.ToString();
                        txtTelefone1Numero.Text = Pedido1.Cliente.Telefone1Numero.ToString();
                    }

                    txtCodigoComanda.Enabled = false;
                    cbbTipoEntrada.Enabled = true;
                    btnAlterarTipoEntrada.Enabled = true;
                }
            }
        }

        private void btnAlterarTipoEntrada_Click(object sender, EventArgs e)
        {
            try
            {
                if (Comanda1 == null)
                {
                    MessageBox.Show("Informe o número da comanda!");
                }
                if (cbbTipoEntrada.SelectedIndex <= 0)
                {
                    MessageBox.Show("Informe o tipo de entrada!");
                }
                else
                {
                    Int32 idPedido = Pedido1.IDPedido.Value;
                    Int32 idTipoEntrada = ((TipoEntradaInformation)cbbTipoEntrada.SelectedItem).IDTipoEntrada.Value;

                    Pedido.AlterarTipoEntrada(idPedido, idTipoEntrada, AC.PDV.IDPDV.Value, IDUsuario);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message}\n{ex.StackTrace}";
                var inner = ex.InnerException;
                while (inner != null)
                {
                    msg += $"\n{inner.Message}\n{inner.StackTrace}";
                    inner = inner.InnerException;
                }
                if (MessageBox.Show("Ocorreu um erro ao alterar o tipo de entrada.\nVer detalhes?", "Erro", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    MessageBox.Show("Erro", msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Close();
            }
        }

        private void txtCodigoComanda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                btnPesquisar.PerformClick();
                cbbTipoEntrada.Focus();
                AcceptButton = btnAlterarTipoEntrada;
            }
        }
    }
}
