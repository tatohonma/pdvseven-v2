using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public partial class AreaDeImpressao
    {
        public AreaDeImpressao(int idAreaImpressao, string nome = null, int? idTipo = null)
        {
            this.IDArea = idAreaImpressao;
            this.Nome = nome;
            this.IDTIpo = idTipo;
        }

        [DataMember(Name = "IDAreaImpressao", EmitDefaultValue = false)]
        public int IDArea { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "IDTIpo", EmitDefaultValue = false)]
        public int? IDTIpo { get; set; }
    }
}
