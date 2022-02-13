using a7D.PDV.BLL;
using a7D.PDV.Componentes.Controles;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmConfiguracoesFiscal : Form
    {
        private List<ConfiguracaoBDInformation> Configuracoes { get; set; }

        public frmConfiguracoesFiscal()
        {
            InitializeComponent();
        }

        private void frmConfiguracoesSAT_Load(object sender, EventArgs e)
        {
            TableLayoutPanel table;
            ConfiguracoesSistema.Recarregar();
            if (ConfiguracoesSistema.Valores.Fiscal == "SAT")
            {
                tabControl1.TabPages.Remove(tabPageNFCe);

                table = tableSat;

                Configuracoes = ConfiguracaoBD.ListarConfiguracoes()
                   .Where(c => c.Chave.StartsWith("infCFe_", StringComparison.InvariantCultureIgnoreCase))
                   .Where(c => !c.Chave.Equals("infCFe_ide_signAC", StringComparison.InvariantCultureIgnoreCase))
                   .ToList();

                txtinfCFe_ide_signAC.Text = ConfiguracaoBD.ConfiguracaoOuPadrao("infCFe_ide_signAC")?.Valor;
            }
            else if (ConfiguracoesSistema.Valores.Fiscal == "NFCe")
            {
                tabControl1.TabPages.Remove(tabPageSATcfg);
                tabControl1.TabPages.Remove(tabPageSATac);

                table = tableNFCe;

                Configuracoes = ConfiguracaoBD.ListarConfiguracoes()
                   .Where(c => c.Chave.StartsWith("NFCe_", StringComparison.InvariantCultureIgnoreCase))
                   .ToList();
            }
            else
            {
                tabControl1.TabPages.Remove(tabPageNFCe);
                tabControl1.TabPages.Remove(tabPageSATcfg);
                tabControl1.TabPages.Remove(tabPageSATac);
                return;
            }

            table.SuspendLayout();
            table.Controls.Clear();
            table.RowCount = 1;
            table.RowStyles[0] = new RowStyle(SizeType.Absolute, 1f);

            foreach (var config in Configuracoes)
            {
                table.RowCount = table.RowCount + 1;
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                var ucc = new UCConfiguracao(config);
                table.Controls.Add(ucc, 0, table.RowCount - 1);
                ucc.Dock = DockStyle.Fill;
            }

            table.ResumeLayout();
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void btnConfigurar_Click(object sender, EventArgs e)
        {
            var error = string.Empty;

            if (txtinfCFe_ide_signAC.Text.Length != 344)
            {
                if (txtinfCFe_ide_signAC.Text == "SGR-SAT SISTEMA DE GESTAO E RETAGUARDA DO SAT")
                    MessageBox.Show("Assinatura inserida é a de desenvolvimento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    error += "Assinatura inválida\n";
            }



            if (string.IsNullOrWhiteSpace(error) == false)
            {
                MessageBox.Show(error, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ConfiguracaoBD.AlterarConfiguracaoSistema("infCFe_ide_signAC", txtinfCFe_ide_signAC.Text);
            MessageBox.Show("Assinatura salva com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
