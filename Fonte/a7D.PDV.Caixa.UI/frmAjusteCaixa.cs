using System;
using System.Collections.Generic;
using System.Windows.Forms;
using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmAjusteCaixa : FormTouch
    {
        private frmPrincipal FormPrincipal;
        private UsuarioInformation Usuario1;

        private frmAjusteCaixa()
        {
            InitializeComponent();
        }

        public frmAjusteCaixa(frmPrincipal formPrincipal, UsuarioInformation usuario) : this()
        {
            FormPrincipal = formPrincipal;
            Usuario1 = usuario;
        }

        private void btnSangria_Click(object sender, EventArgs e)
        {
            try
            {
                CaixaAjusteInformation ajuste = new CaixaAjusteInformation();
                CaixaInformation caixa = BLL.Caixa.Status(AC.PDV.IDPDV.Value);

                if (txtValor.Text == "")
                {
                    MessageBox.Show("Valor do ajuste não foi informado! Favor verificar.");
                }
                else
                {
                    ajuste.Valor = Convert.ToDecimal(txtValor.Text);
                    ajuste.Caixa = frmPrincipal.Caixa1;
                    ajuste.Caixa.PDV = BLL.PDV.Carregar(ajuste.Caixa.PDV.IDPDV.Value);

                    if (rdbSangria.Checked == true)
                        ajuste.CaixaTipoAjuste = new CaixaTipoAjusteInformation { IDCaixaTipoAjuste = 30 };
                    else
                        ajuste.CaixaTipoAjuste = new CaixaTipoAjusteInformation { IDCaixaTipoAjuste = 20 };

                    ajuste.Descricao = txtDescricao.Text;
                    ajuste.DtAjuste = DateTime.Now;

                    CaixaAjuste.Salvar(ajuste);

                    var relatorio = new List<string>();

                    relatorio.Add($"Data/hora: {ajuste.DtAjuste.Value.ToString("dd/MM/yyyy HH:mm")}");
                    relatorio.Add($"PDV: {ajuste.Caixa.PDV.Nome}");
                    relatorio.Add($"Usuário autorização: {Usuario1.Nome}\n");
                    relatorio.Add("");
                    relatorio.Add($"{(ajuste.CaixaTipoAjuste.IDCaixaTipoAjuste == 30 ? "--SANGRIA--" : "--SUPRIMENTO--")}");
                    relatorio.Add("");
                    relatorio.Add($"Valor: {ajuste.Valor.Value.ToString("R$ #,##0.00")}");
                    relatorio.Add($"Descricao:");
                    relatorio.Add(ajuste.Descricao);
                    relatorio.Add($"Ass. responsável: _______________________________\n\n\n");

                    frmPrincipal.Impressora1.RelatorioGerencial(relatorio);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E810, ex);
            }
        }

        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || (e.KeyChar == (char)44 && (txt.Text.Contains(",") == true || txt.Text.Length == 0)))
                e.Handled = true;
        }
    }
}
