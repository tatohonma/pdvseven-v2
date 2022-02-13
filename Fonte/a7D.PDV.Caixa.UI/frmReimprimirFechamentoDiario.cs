using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmReimprimirFechamentoDiario : FormTouch
    {
        private frmPrincipal frmPrincipal;

        public frmReimprimirFechamentoDiario(frmPrincipal frmPrincipal)
        {
            InitializeComponent();
            this.frmPrincipal = frmPrincipal;
        }

        private void frmReimprimirFechamentoDiario_Load(object sender, EventArgs e)
        {
            List<FechamentoInformation> listaFechamento = Fechamento.Listar().OrderByDescending(l => l.DtFechamento).ToList();
            Dictionary<int, string> listaFechamento2 = new Dictionary<int, string>();
            var list = new List<FechamentoItem>();

            foreach (var fechamento in listaFechamento)
            {
                var caixa = BLL.Caixa.ListarPorFechamento(fechamento.IDFechamento.Value).OrderBy(l => l.DtAbertura).FirstOrDefault();

                if (caixa != null)
                {
                    var display = $"{caixa.DtAbertura.Value.ToString("dd/MM/yy HH:mm")} até {fechamento.DtFechamento.Value.ToString("dd/MM/yy HH:mm")}";
                    list.Add(new FechamentoItem(fechamento, display));
                }
            }

            if (list.Count > 0)
            {
                list.Insert(0, new FechamentoItem(null, string.Empty));
                cbbFechamento.DataSource = list;
                cbbFechamento.DisplayMember = "Display";
                cbbFechamento.ValueMember = "IDFechamento";
            }
            else
            {
                MessageBox.Show("Não há fechamentos diários no sistema", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private void btnReimprimir_Click(object sender, EventArgs e)
        {
            if (cbbFechamento.Items.Count > 0 && cbbFechamento.SelectedItem != null)
            {
                var fechamento = cbbFechamento.SelectedItem as FechamentoItem;
                if (fechamento.IDFechamento < 0)
                {
                    MessageBox.Show("Selecione o fechamento que deseja reimprimir", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbbFechamento.Focus();
                }
                else
                {
                    var text = btnReimprimir.Text;
                    try
                    {
                        btnReimprimir.Text = "REIMPRIMINDO...";
                        Enabled = false;
                        Refresh();

                        var relatorio = Relatorio.Fechamento(fechamento.IDFechamento, true);

                        frmPrincipal.Impressora1.RelatorioGerencial(relatorio, 7);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        btnReimprimir.Text = text;
                        Enabled = true;
                        Refresh();
                    }
                }
            }
        }

        public class FechamentoItem
        {
            private string display;
            private FechamentoInformation fechamento;

            public FechamentoItem(FechamentoInformation fechamento, string display)
            {
                this.fechamento = fechamento;
                this.display = display;
            }

            public string Display { get { return display; } }
            public int IDFechamento { get { return (fechamento != null ? fechamento.IDFechamento.Value : -1); } }
        }
    }
}
