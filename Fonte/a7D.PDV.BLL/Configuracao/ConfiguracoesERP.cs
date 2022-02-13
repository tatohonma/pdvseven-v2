using a7D.PDV.EF;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesERP : ConfiguracaoBD
    {
        public ConfiguracoesERP()
            : base((int)ETipoPDV.ERP, null)
        {
        }

        [Config("Ligar Integração Cake ERP no Integrador", ETipoPDV.ERP, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool IntegracaoERP { get; set; }

        [Config("Token para integração", ETipoPDV.ERP, Valor = "", Obrigatorio = false)]
        public string Token { get; set; }

        [Config("Data inicial para envio de vendas", ETipoPDV.ERP, Valor = "", Obrigatorio = false)]
        public string PedidoDataInicio { get; set; }

        [Config("", ETipoPDV.ERP, EConfig._ERPUltimoSincronismo)]
        public string _ERPUltimoSincronismo { get; set; } // Para evitar carregar todos produtos e clientes
    }
}
