using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class GranitoFieldValue
    {
        [DataMember(Name = "fieldId", EmitDefaultValue = false)]
        public string FieldId { get; set; }
        [DataMember(Name = "value", EmitDefaultValue = false)]
        public string Value { get; set; }

    }
}
