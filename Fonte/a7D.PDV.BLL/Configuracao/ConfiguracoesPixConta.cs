using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesPixConta: ConfiguracaoBD
    {
        public ConfiguracoesPixConta() 
            : base((int)ETipoPDV.PIX_CONTA, null)
        {
        }

        [Config("Token IUGU", ETipoPDV.PIX_CONTA)]
        public string Token_IUGU { get; set; }

        [Config("Ativar Conta-Cliente", ETipoPDV.PIX_CONTA, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ContaCliente{ get; set; }
    }
}
