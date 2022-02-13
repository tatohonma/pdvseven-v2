using a7D.PDV.EF;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesSAT : ConfiguracaoBD
    {
        public ConfiguracoesSAT() : base(null, null)
        {
            //Prefixo = "InfCFe_";
        }

        [Config("Código de Ativação", Valor = "123456789", Obrigatorio = true)]
        public string InfCFe_codigoAtivacao { get; protected set; }

        [Config("CNPJ do Emitente", Valor = "14200166000166", Obrigatorio = true)]
        public string InfCFe_emit_CNPJ { get; protected set; }

        //[Config("Caminho para salvar os XMLs do S@T", Valor = @"C:\PDVSeven\SAT", ValoresAceitos = "", Obrigatorio = true)]
        //public string CaminhoXmlSat { get; protected set; }

        //[Config("Salvar XMLs do S@T", Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        //public bool SalvarXmlSat { get; protected set; }

        [Config("IE do Emitente", Valor = "111111111111", Obrigatorio = true)]
        public string InfCFe_emit_IE { get; protected set; }

        [Config("IM do Emitente", Valor = "111111")]
        public string InfCFe_emit_IM { get; protected set; }

        [Config("Indicador de rateio do Desconto sobre subtotal entre itens sujeitos à tributação pelo ISSQN", Valor = "N", Obrigatorio = true)]
        public string InfCFe_emit_indRatISSQN { get; protected set; }

        [Config("CNPJ da Softwarehouse", Valor = "14200166000166", Obrigatorio = true)]
        public string InfCFe_ide_CNPJ { get; protected set; }

        [Config("", Valor = null, Obrigatorio = true)]
        public string InfCFe_ide_signAC { get; protected set; }

        [Config("Versão do XML", Valor = "0.07", ValoresAceitos = "0.07:Versão 7|0.08:Versão 8", Obrigatorio = true)]
        public string InfCFe_versaoDadosEnt { get; protected set; }

        [Config("Imprimir Lei da Transparência no Cupom S@T", Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool InfCFe_12741 { get; protected set; }

        [Config("UF", Valor = "35", ValoresAceitos = "11:RO|12:AC|13:AM|14:RR|15:PA|16:AP|17:TO|21:MA|22:PI|23:CE|24:RN|25:PB|26:PE|27:AL|28:SE|29:BA|31:MG|32:ES|33:RJ|35:SP|41:PR|42:SC|43:RS|50:MS|51:MT|52:GO|53:DF")]
        public string InfCFe_UF { get; protected set; }

        [Config("Nome Fantasia", Valor = "PDVSeven", Obrigatorio = true)]
        public string infCFe_nome_fantasia { get; protected set; }

        [Config("Email para envio de CF-e", Valor = "")]
        public string infCFe_email_destinatario_padrao { get; protected set; }

        [Config("URL do WS S@T", Valor = "http://localhost:8888", Obrigatorio = true)]
        public string InfCFe_urlServicoSAT { get; protected set; }
    }
}
