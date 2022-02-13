using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Ativacao.API.Exceptions
{

    [Serializable]
    public class SenhaIncorretaException : Exception
    {
        public SenhaIncorretaException() { }
        public SenhaIncorretaException(string message) : base(message) { }
        public SenhaIncorretaException(string message, Exception inner) : base(message, inner) { }
        protected SenhaIncorretaException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}