using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesFood99 : ConfiguracaoBD
    {
        public ConfiguracoesFood99() : base((int)ETipoPDV.FOOD99, null) { }

        [Config("Aceitar pedido automático", ETipoPDV.FOOD99, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool AceitarAutomatico { get; set; }

        [Config("IDPDV do Caixa para Contabilizar o Food99", ETipoPDV.FOOD99, Valor = "", Obrigatorio = true)]
        public Int32 IDPDV { get; set; }
    }
}
