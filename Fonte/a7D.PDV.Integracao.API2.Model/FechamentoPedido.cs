using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public partial class FechamentoPedido
    {
        public FechamentoPedido(int idPedido = 0, int idPDV = 0, string chave = null, bool gerarProducao = false, string cpf = null, string docFidelidade = null)
        {
            IDPedido = idPedido;
            IDPdv = idPDV;
            ChaveAcesso = chave;
            GerarOrdemProducao = gerarProducao;
            DocNfe = cpf;
            DocFidelidade = docFidelidade;
            Cancelar = false;
            ImagemComprovante = null;
        }

        [DataMember(Name = "IDPedido", EmitDefaultValue = false)]
        public int IDPedido { get; set; }

        [DataMember(Name = "IDPdv", EmitDefaultValue = false)]
        public int IDPdv { get; set; }

        [DataMember(Name = "ChaveAcesso", EmitDefaultValue = false)]
        public string ChaveAcesso { get; set; }

        [DataMember(Name = "DocNfe", EmitDefaultValue = false)]
        public string DocNfe { get; set; }

        [DataMember(Name = "DocFidelidade", EmitDefaultValue = false)]
        public string DocFidelidade { get; set; }

        [DataMember(Name = "GerarOrdemProducao", EmitDefaultValue = false)]
        public bool? GerarOrdemProducao { get; set; }

        [DataMember(Name = "Cancelar", EmitDefaultValue = false)]
        public bool? Cancelar { get; set; }

        [DataMember(Name = "ImagemComprovante", EmitDefaultValue = false)]
        public int? ImagemComprovante { get; set; }
    }
}
