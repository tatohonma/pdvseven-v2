using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class GranitoResponse
    {
        [DataMember(Name = "responseCode", EmitDefaultValue = true)]
        public int ResponseCode { get; set; }
        [DataMember(Name = "message", EmitDefaultValue = false)]
        public string Message { get; set; }
        [DataMember(Name = "next", EmitDefaultValue = false)]
        public string Next { get; set; }

    }
}
