using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public partial class ResultadoOuErro : ErroResult
    {
        //[DataMember(Name = "Resultado", EmitDefaultValue = false)]
        //public string Resultado { get; set; }

        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int? Id { get; set; }

        [DataMember(Name = "Valor", EmitDefaultValue = false)]
        public decimal? Valor { get; set; }

        [DataMember(Name = "Arquivo", EmitDefaultValue = false)]
        public string Arquivo { get; set; }

        public ResultadoOuErro()
        {
        }

        public ResultadoOuErro(string Mensagem = default(string), int? ID = default(int?)) : base(0, Mensagem)
        {
            Id = ID;
        }

        public ResultadoOuErro(
            int? Codigo = default(int?),
            string Mensagem = default(string),
            int? ID = default(int?)) : base(Codigo, Mensagem)
        {
            Id = ID;
        }

        public override string ToString()
        {
            return $"{Id}:{Codigo}:{Mensagem}";
        }
    }
}
