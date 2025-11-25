using a7D.PDV.EF;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesNFCe : ConfiguracaoBD
    {
        public ConfiguracoesNFCe() : base(null, null)
        {
        }

        [Config("Certificado A1 Arquivo (caminho completo do arquivo .PFX)", Valor = "", Obrigatorio = true)]
        public string NFCe_CertificadoArquivo { get; protected set; }

        [Config("Senha do Certificado A1", Valor = "", Obrigatorio = true)]
        public string NFCe_CertificadoSenha { get; protected set; }

        [Config("ID Token CSC", Valor = "000001", Obrigatorio = true)]
        public string NFCe_CIdToken { get; protected set; }

        [Config("Valor do Token CSC", Valor = "", Obrigatorio = true)]
        public string NFCe_CSC { get; protected set; }

        [Config("Tempo de espera para emissão (segundos)", Valor = "30", Obrigatorio = true)]
        public int NFCe_Timeout { get; protected set; }

        [Config("CNPJ (mesmo do certificado, sómente números)", Valor = "", Obrigatorio = true)]
        public string NFCe_CNPJ { get; protected set; }

        [Config("Rasão Social (mesma do certificado)", Valor = "", Obrigatorio = true)]
        public string NFCe_RazaoScocial { get; protected set; }

        [Config("Nome Fantasia", Valor = "", Obrigatorio = true)]
        public string NFCe_Fantasia { get; protected set; }

        [Config("IE emitente (sómente números)", Valor = "", Obrigatorio = true)]
        public string NFCe_IE { get; protected set; }

        [Config("CPF emitente", Valor = "", Obrigatorio = true)]
        public string NFCe_CPF { get; protected set; }

        [Config("Nome Contato", Valor = "", Obrigatorio = true)]
        public string NFCe_Contato { get; protected set; }

        [Config("Regime Tributário", Valor = "1", ValoresAceitos = "1:Simples Nacional|2:Simples Nacional Excesso Sublimite|3:Regime Normal", Obrigatorio = true)]
        public int NFCe_CRT { get; protected set; }

        [Config("Logradouro (nem número)", Valor = "", Obrigatorio = true)]
        public string NFCe_Logradouro { get; protected set; }

        [Config("Numero (do logradouro)", Valor = "", Obrigatorio = true)]
        public string NFCe_Numero { get; protected set; }

        [Config("Complemento", Valor = "", Obrigatorio = true)]
        public string NFCe_Complemento { get; protected set; }

        [Config("Bairro", Valor = "", Obrigatorio = true)]
        public string NFCe_Bairro { get; protected set; }

        [Config("Nome do Município", Valor = "", Obrigatorio = true)]
        public string NFCe_Municipio { get; protected set; }

        [Config("Código IBGE do Município", Valor = "", Obrigatorio = true)]
        public long NFCe_MuninipioIBGE { get; protected set; }

        [Config("CEP (sómente números)", Valor = "", Obrigatorio = true)]
        public long NFCe_CEP { get; protected set; }

        [Config("Telefone", Valor = "", Obrigatorio = true)]
        public long NFCe_Telefone { get; protected set; }

        [Config("UF", Valor = "35", ValoresAceitos = "11:RO|12:AC|13:AM|14:RR|15:PA|16:AP|17:TO|21:MA|22:PI|23:CE|24:RN|25:PB|26:PE|27:AL|28:SE|29:BA|31:MG|32:ES|33:RJ|35:SP|41:PR|42:SC|43:RS|50:MS|51:MT|52:GO|53:DF")]
        public int NFCe_UF { get; protected set; }
        public string xNFCe_UF;

        [Config("URL do WS Fiscal", Valor = "http://localhost:8888", Obrigatorio = true)]
        public string NFCe_urlServico { get; protected set; }

        //[Config("Nº Lote", Valor = "1", Obrigatorio = true)]
        public int Lote = 1;

        [Config("Nº Série", Valor = "1", Obrigatorio = true)]
        public int NFCe_Serie { get; protected set; }

        [Config("Pasta a cravar XML ", Valor = "")]
        public string NFCe_SalvarXML { get; protected set; }
    }
}
