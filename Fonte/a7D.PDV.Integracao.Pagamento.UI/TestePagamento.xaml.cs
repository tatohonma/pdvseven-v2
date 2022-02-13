using a7D.PDV.Integracao.Pagamento;
using a7D.PDV.Integracao.Pagamento.GranitoTEF;
using System.Windows;

namespace a7D.PDV.Integracao.NTK.UI
{
    public partial class TestePagamento : Window
    {
        public TestePagamento()
        {
            InitializeComponent();
            GranitoLogin.Decript("001-06079-14", "wDJ66i2jK18PMwi2Mvbr7lFvls2YdSXXewLtOA0PBPE=");
            Pagamento.StoneTEF.PinpadStoneTEF.StoneCode = "112634281"; // PRODUÇÃO PDVSeven;
        }

        private void Pagar_Click(object sender, RoutedEventArgs e)
        {
            decimal valor = decimal.Parse(txtValor.Text);
            int parcelas = int.Parse(txtParcelas.Text);
            var pagamento = PinpadTEF.Pagar((TipoTEF)cmbTipo.SelectedIndex, valor, 34567, "teste", "pdv", 3, "11966072730", parcelas);

            if (pagamento == null)
                txtResp.Text = "FIM";
            else
                txtResp.Text = pagamento.ViaEstabelecimento;
        }

        private void Administrar_Click(object sender, RoutedEventArgs e)
        {
            //txtResp.Text = PinpadTEF.Administrar((TipoTEF)cmbTipo.SelectedIndex, TipoOperacao.Administrar);
        }

        private void ConfirmarPendente_Click(object sender, RoutedEventArgs e)
        {
            //txtResp.Text = PinpadTEF.Administrar((TipoTEF)cmbTipo.SelectedIndex, TipoOperacao.ConfirmarPendente);
        }
        private void CancelarPendente_Click(object sender, RoutedEventArgs e)
        {
            //txtResp.Text = PinpadTEF.Administrar((TipoTEF)cmbTipo.SelectedIndex, TipoOperacao.CancelarPendente);
        }
    }
}
