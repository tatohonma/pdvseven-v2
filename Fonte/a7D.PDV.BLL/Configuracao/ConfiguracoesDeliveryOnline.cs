using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesDeliveryOnline : ConfiguracaoBD
    {
        public ConfiguracoesDeliveryOnline() 
            : base((int)ETipoPDV.DELIVERY_ONLINE, null)
        {
        }

        [Config("Ligar Integração do Delivery Online no Integrador", ETipoPDV.DELIVERY_ONLINE, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool IntegracaoDeliveryOnline{ get; set; }
    }
}
