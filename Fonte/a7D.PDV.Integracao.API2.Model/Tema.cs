using System;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class Tema
    {
        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "TemaXML", EmitDefaultValue = false)]
        public string TemaXML { get; set; }

        [DataMember(Name = "UltimaAlteracao", EmitDefaultValue = false)]
        public DateTime UltimaAlteracao { get; set; }

        [DataMember(Name = "Versao", EmitDefaultValue = false)]
        public string Versao { get; set; }
    }
}
