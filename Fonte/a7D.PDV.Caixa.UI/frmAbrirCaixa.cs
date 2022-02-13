using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmAbrirCaixa : FormTouch
    {
        frmPrincipal FormPrincipal;
        public frmAbrirCaixa(frmPrincipal formPrincipal)
        {
            FormPrincipal = formPrincipal;

            InitializeComponent();
        }

        private void btnAbrirCaixa_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                this.Refresh();

                if (Validar() == false)
                {
                    this.Enabled = true;
                    return;
                }

                CaixaValorRegistroInformation registro;

                frmPrincipal.Caixa1 = new CaixaInformation();
                frmPrincipal.Caixa1.PDV = AC.PDV;
                frmPrincipal.Caixa1.DtAbertura = DateTime.Now;
                frmPrincipal.Caixa1.Usuario = AC.Usuario;
                frmPrincipal.Caixa1.SincERP = false;
                BLL.Caixa.Salvar(frmPrincipal.Caixa1);

                List<string> relatorioAbertura = new List<string>
                {
                    $"Abertura de Caixa",
                    "\n",
                    $"Data/Hora: {frmPrincipal.Caixa1.DtAbertura.Value.ToString("dd/MM/yyyy hh:mm")}",
                    $"{frmPrincipal.Caixa1.PDV.Nome}",
                    $"Usuário: {AC.Usuario.Nome}",
                    "\n",
                    $"Valores:"
                };

                foreach (DataGridViewRow item in dgvValoresRecebidos.Rows)
                {
                    registro = new CaixaValorRegistroInformation();
                    registro.Caixa = frmPrincipal.Caixa1;
                    registro.TipoPagamento = new TipoPagamentoInformation { IDTipoPagamento = Convert.ToInt32(item.Cells["IDTipoPagamento"].Value) };

                    if (item.Cells["Valor"].Value == null)
                        registro.ValorAbertura = 0;
                    else
                        registro.ValorAbertura = Convert.ToDecimal(item.Cells["Valor"].Value);
                    relatorioAbertura.Add($"{(item.Cells["Column1"].Value as string)}: {registro.ValorAbertura.Value.ToString("R$ #,##0.00")}\n");
                    CaixaValorRegistro.Salvar(registro);
                }

                relatorioAbertura.Add("\n\n");

                relatorioAbertura.Add("Caixa: ___________________________\n\n");
                relatorioAbertura.Add("Gerente: ___________________________\n\n");

                frmPrincipal.Impressora1.RelatorioGerencial(relatorioAbertura);

                frmPrincipal.Impressora1.LeituraX();
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E806, ex);
            }
            this.Close();
        }

        private Boolean Validar()
        {
            Boolean ret = true;
            Decimal n = 0;

            foreach (DataGridViewRow item in dgvValoresRecebidos.Rows)
            {
                if (item.Cells["Valor"].Value != null && Decimal.TryParse(item.Cells["Valor"].Value.ToString(), out n) == false)
                {
                    MessageBox.Show("O \"Valor\" deve ser numérico!");
                    ret = false;

                    break;
                }
            }

            return ret;
        }

        private void frmAbrirCaixa_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            CarregarTipoPagamento();
        }

        private void CarregarTipoPagamento()
        {
            var list = from l in TipoPagamento.Listar().Where(l => l.Ativo == true && l.RegistrarValores == true)
                       select new { l.IDTipoPagamento, l.Nome };

            dgvValoresRecebidos.DataSource = null;
            dgvValoresRecebidos.DataSource = list.ToArray();
        }
    }
}
