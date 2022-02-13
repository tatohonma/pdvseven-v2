using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{

    public partial class Referencia 
    {
      
        public Referencia(string _Referencia = default(string))
        {
            this._Referencia = _Referencia;
        }
 
        [DataMember(Name="Referencia", EmitDefaultValue=false)]
        public string _Referencia { get; set; }
    }
}
