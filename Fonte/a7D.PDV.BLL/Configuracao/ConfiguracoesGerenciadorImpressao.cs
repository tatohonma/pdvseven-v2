using a7D.PDV.EF;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesGerenciadorImpressao : ConfiguracaoBD
    {
        public ConfiguracoesGerenciadorImpressao() 
            : base((int)ETipoPDV.GERENCIADOR_IMPRESSAO, null)
        {
        }

        [Config("Margem de Impressão Windows", ETipoPDV.GERENCIADOR_IMPRESSAO, EConfig.MargemImpressaoWindows, Valor = "0", Obrigatorio = true)]
        public int MargemImpressaoWindows { get; protected set; }

        [Config("Largura de Impressão Windows", ETipoPDV.GERENCIADOR_IMPRESSAO, EConfig.LarguraImpressaoWindows, Valor = "280", Obrigatorio = true)]
        public int LarguraImpressaoWindows { get; protected set; }

        [Config("Nome da Fonte de Impressão Padrão", ETipoPDV.GERENCIADOR_IMPRESSAO, EConfig.FonteNomeImpressaoWindows, Valor = "NK57 Monospace Cd Rg", Obrigatorio = true)]
        public string FonteNomeImpressaoWindows { get; protected set; }

        [Config("Tamanho da Fonte de Impressão Padrão", ETipoPDV.GERENCIADOR_IMPRESSAO, EConfig.FonteTamanhoImpressaoWindows, Valor = "11", Obrigatorio = true)]
        public int FonteTamanhoImpressaoWindows { get; protected set; }
    }
}
