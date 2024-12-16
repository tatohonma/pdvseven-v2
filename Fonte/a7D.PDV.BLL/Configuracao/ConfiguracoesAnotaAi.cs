using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesAnotaAi: ConfiguracaoBD
    {
        public ConfiguracoesAnotaAi() : base((int)ETipoPDV.ANOTA_AI, null) {}

        [Config("Aceitar pedido automático", ETipoPDV.ANOTA_AI, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool AceitarAutomatico{ get; set; }

        [Config("IDPDV do Caixa para Contabilizar o Anota-ai", ETipoPDV.ANOTA_AI, Valor = "", Obrigatorio = true)]
        public Int32 IDPDV { get; set; }
    }
}
