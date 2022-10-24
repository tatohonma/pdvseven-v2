using a7D.PDV.BLL;
using a7D.PDV.Componentes.Controles;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmConfiguracoes : Form
    {
        private List<ConfiguracaoBDInformation> TodasConfiguracoesPadrao;

        private IEnumerable<ConfiguracaoBDInformation> ConfiguracoesSistema => TodasConfiguracoesPadrao
            .Where(c => c.TipoPDV == null);

        // Lista dos PDV
        private List<PDVInformation> PDVs { get; set; }

        private IEnumerable<PDVExibicao> Filtro(ETipoPDV tipo) => PDVs.Where(p => p.TipoPDV.IDTipoPDV == (int)tipo).Select(p => new PDVExibicao(p));

        public frmConfiguracoes()
        {
            InitializeComponent();
        }

        public frmConfiguracoes(List<PDVInformation> pdvs) : this()
        {
            try
            {
                PDVs = pdvs;
                TodasConfiguracoesPadrao = ConfiguracaoBD.ListarConfiguracoes()
                    .Where(c => c.PDV == null)
                    .Where(c => !c.Chave.StartsWith("_")) // Convenção! variáveis que inician com undeline, são ocultas de controle interno!
                    .Where(c => !string.Equals(c.Chave, "chaveAtivacao", StringComparison.InvariantCultureIgnoreCase))
                    .Where(c => !string.Equals(c.Chave, "dtUltimaVerificacao", StringComparison.InvariantCultureIgnoreCase))
                    .Where(c => !string.Equals(c.Chave, "dtValidade", StringComparison.InvariantCultureIgnoreCase))
                    .Where(c => !c.Chave.StartsWith("RelatorioFechamento-"))
                    .Where(c => !c.Chave.StartsWith("infCFe_", StringComparison.InvariantCultureIgnoreCase))
                    .Where(c => !c.Chave.StartsWith("NFCe_", StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Provavel 'Chave' nula detectada", ex);
            }
        }

        private void frmConfiguracoes_Load(object sender, EventArgs e)
        {
            ListarConfiguracoes(ConfiguracoesSistema, tableSistema);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTab = tabControl1.SelectedTab;
            if (selectedTab == tabCaixa)
                FillConfig(ETipoPDV.CAIXA, cbbPDVCaixa, tableCaixa);
            else if (selectedTab == tabTerminalWin)
                FillConfig(ETipoPDV.TERMINAL_WIN, cbbPDVTerminalWin, tableTerminalWin);
            else if (selectedTab == tabComanda)
                FillConfig(ETipoPDV.COMANDA_ELETRONICA, cbbPDVComanda, tableComanda);
            else if (selectedTab == tabTerminalTab)
                FillConfig(ETipoPDV.TERMINAL_TAB, cbbPDVTerminalTab, tableTerminalTab);
            else if (selectedTab == tabCardapio)
                FillConfig(ETipoPDV.CARDAPIO_DIGITAL, cbbPDVCardapio, tableCardapio);
            else if (selectedTab == tabSaida)
                FillConfig(ETipoPDV.SAIDA, cbbPDVSaida, tableSaida);
            else if (selectedTab == tabAutoatendimento)
                FillConfig(ETipoPDV.AUTOATENDIMENTO, cbbPDVAutoatendimento, tableAutoatendimento);
            else if (selectedTab == tabGerenciadorImpressao)
                FillConfig(ETipoPDV.GERENCIADOR_IMPRESSAO, null, tableGerenciadorImpressao);
            else if (selectedTab == tabIFood)
                FillConfig(ETipoPDV.IFOOD, null, tableIFood);
            else if (selectedTab == tabDeliveryOnline)
                FillConfig(ETipoPDV.DELIVERY_ONLINE, null, tableDeliveryOnline);
        }

        delegate void CallBackFillConfig(ETipoPDV tipo, ComboBox ddl, TableLayoutPanel table, object sender = null);

        private void FillConfig(ETipoPDV tipo, ComboBox ddl, TableLayoutPanel table, object sender = null)
        {
            if (table.InvokeRequired)
            {
                var callBack = new CallBackFillConfig(FillConfig);
                Invoke(callBack, new object[] { tipo, ddl, table, sender });
                return;
            }

            if (ddl != null && ddl.Items.Count == 0)
                FillDropDown(ddl, tipo);

            int idPDV = -1;
            if (sender is PDVExibicao item)
                idPDV = item.IDPDV;

            var configPadraoPorTipo = TodasConfiguracoesPadrao.Where(c => c.TipoPDV != null && c.TipoPDV.Tipo == tipo);
            ListarConfiguracoes(configPadraoPorTipo, table, idPDV);
        }

        private void FillDropDown(ComboBox ddl, ETipoPDV tipo)
        {
            var lista = Filtro(tipo).ToList();
            lista.Insert(0, new PDVExibicao
            {
                IDPDV = -1,
                Nome = "Configurações Padrão"
            });
            ddl.DataSource = lista;
        }

        private void ListarConfiguracoes(IEnumerable<ConfiguracaoBDInformation> configs, TableLayoutPanel table, int idPdv = -1)
        {
            table.SuspendLayout();
            table.Controls.Clear();
            table.RowCount = 1;

            table.RowStyles[0] = new RowStyle(SizeType.Absolute, 1f);
            table.Controls.Add(new Control(), 0, 0);

            foreach (var config in configs)
            {
                table.RowCount = table.RowCount + 1;
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                var ucc = new UCConfiguracao(config, idPdv);
                ucc.Dock = DockStyle.Fill;
                table.Controls.Add(ucc, 0, table.RowCount - 1);
            }

            table.RowCount = table.RowCount + 1;
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 1f));
            table.Controls.Add(new Control());

            table.ResumeLayout();
        }

        #region Dropdown Select

        private void cbbPDVCaixa_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillConfig(ETipoPDV.CAIXA, null, tableCaixa, cbbPDVCaixa.SelectedItem);
        }

        private void cbbPDVTerminalWin_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillConfig(ETipoPDV.TERMINAL_WIN, null, tableTerminalWin, cbbPDVTerminalWin.SelectedItem);
        }

        private void cbbPDVComanda_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillConfig(ETipoPDV.COMANDA_ELETRONICA, null, tableComanda, cbbPDVComanda.SelectedItem);
        }

        private void cbbPDVTerminalTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillConfig(ETipoPDV.TERMINAL_TAB, null, tableTerminalTab, cbbPDVTerminalTab.SelectedItem);
        }

        private void cbbPDVCardapio_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillConfig(ETipoPDV.CARDAPIO_DIGITAL, null, tableCardapio, cbbPDVCardapio.SelectedItem);
        }

        private void cbbPDVAutoatendimento_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillConfig(ETipoPDV.AUTOATENDIMENTO, null, tableAutoatendimento, cbbPDVAutoatendimento.SelectedItem);
        }

        private void cbbPDVSaida_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillConfig(ETipoPDV.SAIDA, null, tableSaida, cbbPDVSaida.SelectedItem);
        }
        #endregion

    }
}