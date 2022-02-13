using System;
using System.Windows.Forms;
using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmAlterarStatusComanda : FormTouch
    {
        ComandaInformation Comanda1;
        PedidoInformation Pedido1;

        public frmAlterarStatusComanda()
        {
            InitializeComponent();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (txtCodigoComanda.Text != "")
            {
                Comanda1 = ComandaUtil.CarregarPorNumeroOuCodigo(txtCodigoComanda.Text);
                if (Comanda1.IDComanda == null)
                {
                    MessageBox.Show("Comanda não cadastrada!");
                }
                else
                {
                    Pedido1 = Pedido.CarregarUltimoPedido(Comanda1.GUIDIdentificacao);
                    if (Pedido1.IDPedido != null)
                    {
                        if (ConfiguracoesSistema.Valores.ComandaComCheckin)
                        {
                            txtNomeCompleto.Text = Pedido1.Cliente?.NomeCompleto;
                            txtTelefone1DDD.Text = Pedido1.Cliente?.Telefone1DDD?.ToString();
                            txtTelefone1Numero.Text = Pedido1.Cliente?.Telefone1Numero?.ToString();
                        }
                    }

                    txtCodigoComanda.Enabled = false;
                    btnPesquisar.Enabled = false;
                    rdbLiberada.Enabled = true;
                    rdbPerdida.Enabled = true;
                    rdbCancelada.Enabled = true;
                    txtObservacao.Enabled = true;
                    btnAlterarStatus.Enabled = true;

                    switch (Comanda1.ValorStatusComanda)
                    {
                        case EStatusComanda.Liberada:
                        case EStatusComanda.EmUso:
                            rdbLiberada.Checked = true;
                            break;
                        case EStatusComanda.Cancelada:
                            rdbCancelada.Checked = true;
                            break;
                        case EStatusComanda.Perdida:
                            rdbPerdida.Checked = true;
                            break;
                    }

                    txtObservacao.Text = Comanda1.Observacao;
                }
            }
        }

        private void btnAlterarStatus_Click(object sender, EventArgs e)
        {
            if (rdbLiberada.Checked == true)
            {

                if (Pedido1.IDPedido == null)
                    Comanda1.StatusComanda = StatusComanda.Carregar(EStatusComanda.Liberada);
                else
                    Comanda1.StatusComanda = StatusComanda.Carregar(EStatusComanda.EmUso);
            }
            else if (rdbPerdida.Checked == true)
            {
                Comanda1.StatusComanda = StatusComanda.Carregar(EStatusComanda.Perdida);
            }
            else if (rdbCancelada.Checked == true)
            {
                Comanda1.StatusComanda = StatusComanda.Carregar(EStatusComanda.Cancelada);
            }

            Comanda.Salvar(Comanda1);

            this.Close();
        }

        private void txtCodigoComanda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnPesquisar_Click(sender, e);
            }
        }
    }
}
