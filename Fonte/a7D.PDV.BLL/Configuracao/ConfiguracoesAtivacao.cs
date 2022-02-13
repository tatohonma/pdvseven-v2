using a7D.PDV.EF;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesAtivacao : ConfiguracaoBD
    {
        public ConfiguracoesAtivacao() : base(null, null)
        {
            if (!string.IsNullOrWhiteSpace(ChaveAtivacao))
                ChaveAtivacao = ChaveAtivacao.Replace("\r", "").Replace("\n", "");
        }

        [Config("Chave de Ativação", Chave = EConfig.ChaveAtivacao, Obrigatorio = true)]
        public string ChaveAtivacao { get; protected set; }

        [Config("")]
        public string DtUltimaVerificacao { get; protected set; }

        [Config("")]
        public string DtValidade { get; protected set; }

        [Config("", Chave = EConfig._VersaoServidor)]
        public string _VersaoServidor { get; protected set; }
    }
}