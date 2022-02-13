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
    public partial class frmConfigurarFechamento : Form
    {
        private const string _relatorio = "RelatorioFechamento-";

        private List<ConfiguracaoBDInformation> ConfiguracoesFechamento { get; set; }

        public frmConfigurarFechamento()
        {
            InitializeComponent();
        }

        private void frmConfigurarFechamento_Load(object sender, EventArgs e)
        {
            ConfiguracoesFechamento = ConfiguracaoBD.ListarConfiguracoes()
                .Where(c => c.Chave.StartsWith(_relatorio))
                .ToList();

            tableFechamento.SuspendLayout();
            tableFechamento.Controls.Clear();
            tableFechamento.RowCount = 0;

            foreach (var config in ConfiguracoesFechamento)
            {
                tableFechamento.RowCount = tableFechamento.RowCount + 1;
                tableFechamento.RowStyles.Add(new RowStyle(SizeType.AutoSize, 144f));
                var ucc = new UCConfiguracao(config);
                tableFechamento.Controls.Add(ucc, 0, tableFechamento.RowCount - 1);
                ucc.Dock = DockStyle.Fill;
            }

            tableFechamento.RowCount = tableFechamento.RowCount + 1;
            tableFechamento.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
            tableFechamento.Controls.Add(new Control(), 0, tableFechamento.RowCount - 1);

            tableFechamento.ResumeLayout();
        }
    }
}
